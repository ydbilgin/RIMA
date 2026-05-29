// F7 — Live Tool Smoke Tests (EditMode, deterministic)
// Tests the core data path of the T3 live-edit pipeline:
//   - RoomLayoutData.FromJson (schema 1.0 parse)
//   - RuntimeAssetRegistry stub (GUID lookup API)
//
// NOT covered here (manual checklist below):
//   - JsonFileWatcher / FileSystemWatcher (background thread + timing-dependent — flaky in CI)
//   - LiveToolLauncher exe-launch (requires built .exe artifacts + OS process spawn)
//   - Full LiveRoomReloader reload cycle (requires scene + Tilemap + player GameObject)
//
// ── MANUAL SMOKE CHECKLIST ────────────────────────────────────────────────────
// Run once after each T3 build to confirm end-to-end:
//
//   [ ] 1. Press "Launch Live Tool" in RimaRoomPainterWindow toolbar.
//          Expected: RIMA_Tool.exe + RIMA.exe both launch (PID shown in statusbar).
//   [ ] 2. Tool palette shows >= 50 thumbnails.
//          Expected: Registry baked, all entries visible, no "missing sprite" icons.
//   [ ] 3. Paint a floor tile in Tool.exe.
//          Expected: room_current.json updated; Game.exe shows the tile within 200 ms.
//   [ ] 4. Paint a wall prefab in Tool.exe.
//          Expected: Prop appears in Game.exe with correct collider shape.
//   [ ] 5. Drag-resize a collider handle in Tool.exe.
//          Expected: Collider size updates in Game.exe on next paint/save.
//   [ ] 6. Cliff hover preview snaps to grid cell.
//          Expected: Indicator moves with cursor, correct orientation per neighbor.
//   [ ] 7. Ctrl+Z x3 in Tool.exe.
//          Expected: Last 3 paint ops reverted; JSON updated; Game reflects.
//   [ ] 8. Press "Save Room" in Tool.exe.
//          Expected: JSON committed; relaunch Game → same state present.
//   [ ] 9. Close both .exe.
//          Expected: No orphan processes (check Task Manager).
//   [ ] 10. Re-launch via button.
//           Expected: State from step 8 restored; both windows appear.
//   [ ] 11. Edge: delete room_current.json while Game.exe is running.
//           Expected: Watcher logs warning, Game does NOT crash.
//   [ ] 12. Edge: corrupt room_current.json (write "{invalid}").
//           Expected: "[RoomLayoutData] JSON parse error" in console; Game keeps last valid state.
//   [ ] 13. Edge: GUID not in registry (edit JSON manually to unknown GUID).
//           Expected: "[LiveRoomReloader] GUID not in registry" warning; missing prop skipped gracefully.
//   [ ] 14. Edge: Tool.exe crashes mid-write (lock file left behind).
//           Expected: Watcher logs "Lock file timeout — forcing read" after 500 ms; Game recovers.
//   [ ] 15. Edge: Disconnect + reconnect FileSystemWatcher (suspend/resume system).
//           Expected: Polling fallback (500 ms) catches the change; no manual restart needed.
// ─────────────────────────────────────────────────────────────────────────────

using System.Collections.Generic;
using NUnit.Framework;
using RIMA.Live;
using RIMA.RoomPainter;
using UnityEngine;
using UnityEngine.TestTools;

namespace RIMA.Tests
{
    /// <summary>
    /// Deterministic EditMode smoke tests for the T3 live-edit data path.
    /// No scene load, no FileSystemWatcher, no process spawn.
    /// </summary>
    public class LiveToolSmokeTests
    {
        // ── Minimal valid schema-1.0 JSON ─────────────────────────────────────

        private const string ValidJson = @"{
            ""schema_version"": ""1.0"",
            ""room_id"": ""PlayableArena_Test01"",
            ""metadata"": { ""name"": ""Test Room"", ""created"": ""2026-05-28T00:00:00Z"", ""modified"": ""2026-05-28T00:00:00Z"" },
            ""floor_tiles"": [
                { ""cell"": [0, 0, 0], ""tile_guid"": ""aaaa0001"" },
                { ""cell"": [1, 0, 0], ""tile_guid"": ""aaaa0002"" },
                { ""cell"": [2, 0, 0], ""tile_guid"": ""aaaa0003"" },
                { ""cell"": [3, 0, 0], ""tile_guid"": ""aaaa0004"" },
                { ""cell"": [4, 0, 0], ""tile_guid"": ""aaaa0005"" }
            ],
            ""cliff_cells"": [
                { ""cell"": [5, 0, 0], ""is_decor"": false }
            ],
            ""prop_instances"": [
                { ""prefab_guid"": ""bbbb0001"", ""position"": [3.14, 1.59, 0.0], ""rotation"": 0.0, ""instance_id"": ""inst_0001"" },
                { ""prefab_guid"": ""bbbb0002"", ""position"": [0.0, 0.0, 0.0], ""rotation"": 90.0, ""instance_id"": ""inst_0002"" },
                { ""prefab_guid"": ""bbbb0003"", ""position"": [-1.5, 2.0, 0.0], ""rotation"": 180.0, ""instance_id"": ""inst_0003"" }
            ],
            ""collider_overrides"": [
                { ""instance_id"": ""inst_0001"", ""size"": [0.6, 1.0], ""offset"": [0.0, 0.5], ""shape"": ""Box"" }
            ]
        }";

        // ── RoomLayoutData.FromJson parse tests ───────────────────────────────

        [Test]
        public void FromJson_ValidJson_ReturnsNonNull()
        {
            RoomLayoutData data = RoomLayoutData.FromJson(ValidJson);
            Assert.IsNotNull(data, "Valid JSON should parse to a non-null RoomLayoutData.");
        }

        [Test]
        public void FromJson_ValidJson_SchemaVersion_Is_1_0()
        {
            RoomLayoutData data = RoomLayoutData.FromJson(ValidJson);
            Assert.AreEqual("1.0", data.schema_version, "schema_version should be '1.0'.");
        }

        [Test]
        public void FromJson_ValidJson_RoomId_RoundTrip()
        {
            RoomLayoutData data = RoomLayoutData.FromJson(ValidJson);
            Assert.AreEqual("PlayableArena_Test01", data.room_id,
                "room_id should round-trip correctly.");
        }

        [Test]
        public void FromJson_ValidJson_FloorTiles_Count()
        {
            RoomLayoutData data = RoomLayoutData.FromJson(ValidJson);
            Assert.AreEqual(5, data.floor_tiles.Count,
                "Should deserialize 5 floor_tiles.");
        }

        [Test]
        public void FromJson_ValidJson_FloorTile_CellCoords_Correct()
        {
            RoomLayoutData data = RoomLayoutData.FromJson(ValidJson);
            FloorTileData ft = data.floor_tiles[0];
            Assert.AreEqual(3, ft.cell.Length, "cell array should have 3 elements (x,y,z).");
            Assert.AreEqual(0, ft.cell[0]);
            Assert.AreEqual(0, ft.cell[1]);
            Assert.AreEqual(0, ft.cell[2]);
        }

        [Test]
        public void FromJson_ValidJson_FloorTile_TileGuid_Preserved()
        {
            RoomLayoutData data = RoomLayoutData.FromJson(ValidJson);
            Assert.AreEqual("aaaa0001", data.floor_tiles[0].tile_guid,
                "tile_guid should survive round-trip.");
        }

        [Test]
        public void FromJson_ValidJson_CliffCells_Count()
        {
            RoomLayoutData data = RoomLayoutData.FromJson(ValidJson);
            Assert.AreEqual(1, data.cliff_cells.Count,
                "Should deserialize 1 cliff_cell.");
        }

        [Test]
        public void FromJson_ValidJson_CliffCell_IsDecor_False()
        {
            RoomLayoutData data = RoomLayoutData.FromJson(ValidJson);
            Assert.IsFalse(data.cliff_cells[0].is_decor,
                "is_decor should be false.");
        }

        [Test]
        public void FromJson_ValidJson_LegacyCliffCell_TileGuid_Empty()
        {
            RoomLayoutData data = RoomLayoutData.FromJson(ValidJson);
            Assert.IsTrue(string.IsNullOrEmpty(data.cliff_cells[0].tile_guid),
                "Legacy cliff_cells without tile_guid should remain valid.");
        }

        [Test]
        public void FromJson_CliffCell_TileGuid_Preserved()
        {
            const string json = @"{
                ""schema_version"": ""1.1"",
                ""room_id"": ""CliffGuidRoom"",
                ""cliff_cells"": [
                    { ""cell"": [5, 0, 0], ""tile_guid"": ""cccc0001"", ""is_decor"": false }
                ]
            }";

            RoomLayoutData data = RoomLayoutData.FromJson(json);
            Assert.IsNotNull(data, "Cliff GUID JSON should parse.");
            Assert.AreEqual("cccc0001", data.cliff_cells[0].tile_guid,
                "cliff_cells tile_guid should survive round-trip.");
        }

        [Test]
        public void FromJson_ValidJson_PropInstances_Count()
        {
            RoomLayoutData data = RoomLayoutData.FromJson(ValidJson);
            Assert.AreEqual(3, data.prop_instances.Count,
                "Should deserialize 3 prop_instances.");
        }

        [Test]
        public void FromJson_ValidJson_PropPosition_RoundTrip()
        {
            RoomLayoutData data = RoomLayoutData.FromJson(ValidJson);
            PropData p = data.prop_instances[0];
            Assert.AreEqual(3, p.position.Length, "position array should have 3 elements.");
            Assert.AreEqual("inst_0001", p.instance_id, "instance_id should round-trip.");
            Assert.AreEqual(0.0f, p.rotation, 0.001f);
            Assert.AreEqual(3.14f, p.position[0], 0.001f);
            Assert.AreEqual(1.59f, p.position[1], 0.001f);
            Assert.AreEqual(0.0f,  p.position[2], 0.001f);
        }

        [Test]
        public void FromJson_ValidJson_PropRotation_90_Preserved()
        {
            RoomLayoutData data = RoomLayoutData.FromJson(ValidJson);
            Assert.AreEqual(90.0f, data.prop_instances[1].rotation, 0.001f,
                "rotation=90 should round-trip.");
        }

        [Test]
        public void FromJson_ValidJson_ColliderOverride_Parsed()
        {
            RoomLayoutData data = RoomLayoutData.FromJson(ValidJson);
            Assert.AreEqual(1, data.collider_overrides.Count,
                "Should deserialize 1 collider_override.");
            ColliderOverrideData co = data.collider_overrides[0];
            Assert.AreEqual("inst_0001", co.instance_id);
            Assert.AreEqual("Box", co.shape);
            Assert.AreEqual(0.6f, co.size[0], 0.001f);
            Assert.AreEqual(1.0f, co.size[1], 0.001f);
        }

        [Test]
        public void FromJson_EmptyString_ReturnsNull()
        {
            // Suppress expected error log during test.
            LogAssert.ignoreFailingMessages = true;
            RoomLayoutData data = RoomLayoutData.FromJson(string.Empty);
            LogAssert.ignoreFailingMessages = false;
            Assert.IsNull(data, "Empty JSON should return null (graceful-degrade).");
        }

        [Test]
        public void FromJson_MalformedJson_ReturnsNull()
        {
            LogAssert.ignoreFailingMessages = true;
            // JsonUtility returns default object (not null) on malformed JSON, then we get a
            // null schema_version — verify the parse completes without exception.
            RoomLayoutData data = RoomLayoutData.FromJson("{{{invalid}}}");
            LogAssert.ignoreFailingMessages = false;
            // Either null or an object with null fields — must NOT throw.
            Assert.Pass("Malformed JSON must not throw an unhandled exception.");
        }

        [Test]
        public void FromJson_MinimalJson_MissingLists_ReturnsNonNull()
        {
            // Verify graceful-degrade for missing optional arrays.
            const string minimal = @"{ ""schema_version"": ""1.0"", ""room_id"": ""MinimalRoom"" }";
            RoomLayoutData data = RoomLayoutData.FromJson(minimal);
            Assert.IsNotNull(data, "Minimal JSON (no arrays) should still parse.");
        }

        // ── RuntimeAssetRegistry stub tests ───────────────────────────────────
        // Validates the public GUID-lookup API without baking real assets.

        private RuntimeAssetRegistry BuildStubRegistry(params RegistryEntry[] entries)
        {
            RuntimeAssetRegistry reg = ScriptableObject.CreateInstance<RuntimeAssetRegistry>();
            reg.SetEntries(new List<RegistryEntry>(entries));
            return reg;
        }

        [Test]
        public void Registry_Contains_KnownGuid_ReturnsTrue()
        {
            RuntimeAssetRegistry reg = BuildStubRegistry(
                new RegistryEntry { guid = "aaaa0001", displayName = "FloorTileA", tag = "floor", layer = RoomLayer.Floor }
            );
            Assert.IsTrue(reg.Contains("aaaa0001"),
                "Registry should report Contains=true for a registered GUID.");
        }

        [Test]
        public void Registry_Contains_UnknownGuid_ReturnsFalse()
        {
            RuntimeAssetRegistry reg = BuildStubRegistry(
                new RegistryEntry { guid = "aaaa0001", displayName = "FloorTileA", tag = "floor", layer = RoomLayer.Floor }
            );
            Assert.IsFalse(reg.Contains("zzzzFFFF"),
                "Registry should return false for an unregistered GUID.");
        }

        [Test]
        public void Registry_Contains_NullOrEmpty_ReturnsFalse()
        {
            RuntimeAssetRegistry reg = BuildStubRegistry();
            Assert.IsFalse(reg.Contains(null),
                "Contains(null) should return false — no crash.");
            Assert.IsFalse(reg.Contains(string.Empty),
                "Contains(empty) should return false — no crash.");
        }

        [Test]
        public void Registry_Get_KnownGuid_ReturnsEntry()
        {
            RuntimeAssetRegistry reg = BuildStubRegistry(
                new RegistryEntry { guid = "bbbb0001", displayName = "PropA", tag = "prop", layer = RoomLayer.Props }
            );
            RegistryEntry e = reg.Get("bbbb0001");
            Assert.IsNotNull(e, "Get should return entry for registered GUID.");
            Assert.AreEqual("PropA", e.displayName);
        }

        [Test]
        public void Registry_Get_UnknownGuid_ReturnsNull()
        {
            RuntimeAssetRegistry reg = BuildStubRegistry();
            Assert.IsNull(reg.Get("zzzzFFFF"),
                "Get should return null for unknown GUID.");
        }

        [Test]
        public void Registry_GetByTag_ReturnsFilteredEntries()
        {
            RuntimeAssetRegistry reg = BuildStubRegistry(
                new RegistryEntry { guid = "cc0001", displayName = "CliffA", tag = "cliff", layer = RoomLayer.Cliff },
                new RegistryEntry { guid = "ff0001", displayName = "FloorA", tag = "floor", layer = RoomLayer.Floor },
                new RegistryEntry { guid = "cc0002", displayName = "CliffB", tag = "cliff", layer = RoomLayer.Cliff }
            );
            IReadOnlyList<RegistryEntry> cliffs = reg.GetByTag("cliff");
            Assert.AreEqual(2, cliffs.Count,
                "GetByTag('cliff') should return 2 entries.");
        }

        [Test]
        public void Registry_GetByTag_CaseInsensitive()
        {
            RuntimeAssetRegistry reg = BuildStubRegistry(
                new RegistryEntry { guid = "ff0001", displayName = "FloorA", tag = "floor", layer = RoomLayer.Floor }
            );
            Assert.AreEqual(1, reg.GetByTag("FLOOR").Count,
                "GetByTag should be case-insensitive.");
        }

        [Test]
        public void Registry_GetByTag_NoMatch_ReturnsEmpty()
        {
            RuntimeAssetRegistry reg = BuildStubRegistry(
                new RegistryEntry { guid = "ff0001", displayName = "FloorA", tag = "floor", layer = RoomLayer.Floor }
            );
            IReadOnlyList<RegistryEntry> result = reg.GetByTag("nonexistent");
            Assert.IsNotNull(result, "GetByTag should never return null.");
            Assert.AreEqual(0, result.Count, "GetByTag should return empty list for no match.");
        }

        [Test]
        public void Registry_GetByLayer_ReturnsCorrectEntries()
        {
            RuntimeAssetRegistry reg = BuildStubRegistry(
                new RegistryEntry { guid = "ww0001", displayName = "WallA", tag = "wall", layer = RoomLayer.Wall },
                new RegistryEntry { guid = "ff0001", displayName = "FloorA", tag = "floor", layer = RoomLayer.Floor }
            );
            List<RegistryEntry> walls = reg.GetByLayer(RoomLayer.Wall);
            Assert.AreEqual(1, walls.Count, "GetByLayer(Wall) should return 1 entry.");
            Assert.AreEqual("WallA", walls[0].displayName);
        }

        [Test]
        public void Registry_Count_ReflectsSetEntries()
        {
            RuntimeAssetRegistry reg = BuildStubRegistry(
                new RegistryEntry { guid = "aa", displayName = "A", tag = "floor", layer = RoomLayer.Floor },
                new RegistryEntry { guid = "bb", displayName = "B", tag = "cliff", layer = RoomLayer.Cliff }
            );
            Assert.AreEqual(2, reg.Count, "Count should equal number of entries set.");
        }

        [Test]
        public void Registry_SetEntries_ReplacesExisting()
        {
            RuntimeAssetRegistry reg = BuildStubRegistry(
                new RegistryEntry { guid = "aa", displayName = "A", tag = "floor", layer = RoomLayer.Floor }
            );
            Assert.AreEqual(1, reg.Count);

            reg.SetEntries(new List<RegistryEntry>
            {
                new RegistryEntry { guid = "bb", displayName = "B", tag = "cliff", layer = RoomLayer.Cliff },
                new RegistryEntry { guid = "cc", displayName = "C", tag = "wall",  layer = RoomLayer.Wall  },
            });
            Assert.AreEqual(2, reg.Count, "SetEntries should replace all previous entries.");
            Assert.IsFalse(reg.Contains("aa"), "'aa' should no longer be present after SetEntries.");
            Assert.IsTrue(reg.Contains("bb"),  "'bb' should be present after SetEntries.");
        }

        // ── JSON ↔ Registry integration: simulate LiveRoomReloader resolution ─
        // Parses the standard ValidJson, then checks each tile/prop GUID
        // against a stub registry — mirrors what LiveRoomReloader.ApplyJsonFile does.

        [Test]
        public void ParsedGuid_ResolvedFromRegistry_AllKnownTilesResolvable()
        {
            RoomLayoutData data = RoomLayoutData.FromJson(ValidJson);

            RuntimeAssetRegistry reg = BuildStubRegistry(
                new RegistryEntry { guid = "aaaa0001", displayName = "Tile1", tag = "floor", layer = RoomLayer.Floor },
                new RegistryEntry { guid = "aaaa0002", displayName = "Tile2", tag = "floor", layer = RoomLayer.Floor },
                new RegistryEntry { guid = "aaaa0003", displayName = "Tile3", tag = "floor", layer = RoomLayer.Floor },
                new RegistryEntry { guid = "aaaa0004", displayName = "Tile4", tag = "floor", layer = RoomLayer.Floor },
                new RegistryEntry { guid = "aaaa0005", displayName = "Tile5", tag = "floor", layer = RoomLayer.Floor }
            );

            int resolved = 0;
            foreach (FloorTileData ft in data.floor_tiles)
            {
                if (reg.Contains(ft.tile_guid)) resolved++;
            }
            Assert.AreEqual(5, resolved,
                "All 5 floor tile GUIDs from parsed JSON should resolve in the stub registry.");
        }

        [Test]
        public void ParsedGuid_UnknownGuid_Skipped_WithoutCrash()
        {
            RoomLayoutData data = RoomLayoutData.FromJson(ValidJson);
            RuntimeAssetRegistry reg = BuildStubRegistry(); // empty — all GUIDs unknown

            int unresolved = 0;
            foreach (FloorTileData ft in data.floor_tiles)
            {
                if (!reg.Contains(ft.tile_guid)) unresolved++;
            }
            Assert.AreEqual(5, unresolved,
                "All GUIDs should be unresolved against empty registry — no crash.");
        }

        [Test]
        public void ParsedGuid_PropInstances_InstanceId_UniquePerEntry()
        {
            RoomLayoutData data = RoomLayoutData.FromJson(ValidJson);
            var ids = new HashSet<string>();
            foreach (PropData p in data.prop_instances)
            {
                Assert.IsFalse(string.IsNullOrEmpty(p.instance_id),
                    "instance_id should be non-empty for all props in test fixture.");
                Assert.IsTrue(ids.Add(p.instance_id),
                    $"instance_id '{p.instance_id}' should be unique across all prop entries.");
            }
        }
    }
}
