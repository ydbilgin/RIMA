#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text;
using RIMA;
using RIMA.MapDesigner.Composition;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Props.Auto;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.Map
{
    public static class RoomQCFixMenu
    {
        private const string PropRegistryPath = "Assets/Resources/Props/PropRegistry.asset";
        private const string ArenaScenePath = "Assets/Scenes/_Arena.unity";
        private const string RoomsRoot = "Assets/Data/Rooms";
        private const float TargetDensity = 0.30f;
        private const int SeedTries = 8;
        private const int MaxPropsPerRoom = 45;

        private static readonly string[] PropRepairRoomPaths =
        {
            "Assets/Data/Rooms/Generated/combat_large_diamond_01.asset",
            "Assets/Data/Rooms/Generated/combat_large_donut_01.asset",
            "Assets/Data/Rooms/Generated/chest_large_reliquary_diamond_01.asset",
            "Assets/Data/Rooms/Generated/combat_large_hourglass_01.asset",
            "Assets/Data/Rooms/Generated/combat_large_lshape_01.asset",
            "Assets/Data/Rooms/Generated/elite_large_trident_01.asset",
            "Assets/Data/Rooms/Generated/combat_large_cross_01.asset",
        };

        private static readonly string[] VerificationRoomPaths =
        {
            "Assets/Data/Rooms/Generated/combat_large_diamond_01.asset",
            "Assets/Data/Rooms/Generated/combat_large_donut_01.asset",
            "Assets/Data/Rooms/Generated/chest_large_reliquary_diamond_01.asset",
            "Assets/Data/Rooms/Generated/combat_large_hourglass_01.asset",
            "Assets/Data/Rooms/Generated/combat_large_lshape_01.asset",
            "Assets/Data/Rooms/Generated/elite_large_trident_01.asset",
            "Assets/Data/Rooms/Generated/combat_large_cross_01.asset",
            "Assets/Data/Rooms/Generated/combatlarge_twin_basins_01.asset",
            "Assets/Data/Rooms/Library/Treasure_01.asset",
        };

        [MenuItem("RIMA/Rooms/QC/Run Room QC Repairs")]
        public static void RunRepairs()
        {
            PropRegistrySO registry = LoadRegistry();
            if (registry == null) return;

            IReadOnlyList<PropDefinitionSO> pool = BuildPropPool(registry);
            if (pool.Count == 0)
            {
                Debug.LogError("[RoomQCFix] Prop pool is empty.");
                return;
            }

            StringBuilder report = new StringBuilder();
            report.AppendLine("[RoomQCFix] Repair report");
            report.AppendLine("Root cause #1: baked prop cells in RoomTemplateSO assets (auto-placer sampled rectangular bounds / bounds-only roles).");

            AssetDatabase.StartAssetEditing();
            try
            {
                for (int i = 0; i < PropRepairRoomPaths.Length; i++)
                {
                    RoomTemplateSO room = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(PropRepairRoomPaths[i]);
                    if (room == null)
                    {
                        report.AppendLine($"{PropRepairRoomPaths[i]}: missing");
                        continue;
                    }

                    int before = room.props != null ? room.props.Count : 0;
                    int invalidBefore = CountInvalidProps(room, registry);
                    ReseedProps(room, pool);
                    int after = room.props != null ? room.props.Count : 0;
                    int invalidAfter = CountInvalidProps(room, registry);
                    EditorUtility.SetDirty(room);
                    report.AppendLine($"{room.roomId}: props {before}->{after}, invalid footprints {invalidBefore}->{invalidAfter}");
                }

                RoomTemplateSO twin = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>("Assets/Data/Rooms/Generated/combatlarge_twin_basins_01.asset");
                if (twin != null)
                {
                    int filled = FillEnclosedVoids(twin);
                    EditorUtility.SetDirty(twin);
                    report.AppendLine($"{twin.roomId}: filled {filled} enclosed interior void cells.");
                }

                RoomTemplateSO treasure = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>("Assets/Data/Rooms/Library/Treasure_01.asset");
                if (treasure != null)
                {
                    int beforeFloor = CountWalkable(treasure);
                    BuildTreasureVault(treasure);
                    int afterFloor = CountWalkable(treasure);
                    EditorUtility.SetDirty(treasure);
                    report.AppendLine($"{treasure.roomId}: floor {beforeFloor}->{afterFloor}, doors=N/E/W.");
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            WriteReport("STAGING/ROOM_QC_FIX_REPAIR_yasinderyabilgin.txt", report.ToString());
            Debug.Log(report.ToString());
        }

        [MenuItem("RIMA/Rooms/QC/Verify Room QC Repairs")]
        public static void VerifyRepairs()
        {
            PropRegistrySO registry = LoadRegistry();
            if (registry == null) return;

            EditorSceneManager.OpenScene(ArenaScenePath, OpenSceneMode.Single);
            IsoRoomBuilder builder = Object.FindFirstObjectByType<IsoRoomBuilder>();
            if (builder == null)
            {
                Debug.LogError("[RoomQCFix] IsoRoomBuilder not found in _Arena.");
                return;
            }

            Grid grid = builder.GetComponentInParent<Grid>();
            if (grid == null)
            {
                grid = Object.FindFirstObjectByType<Grid>();
            }

            if (grid == null)
            {
                Debug.LogError("[RoomQCFix] Grid not found in _Arena.");
                return;
            }

            bool ok = true;
            StringBuilder report = new StringBuilder();
            report.AppendLine("[RoomQCFix] Verification report");

            for (int i = 0; i < VerificationRoomPaths.Length; i++)
            {
                RoomTemplateSO room = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(VerificationRoomPaths[i]);
                if (room == null)
                {
                    ok = false;
                    report.AppendLine($"{VerificationRoomPaths[i]}: missing");
                    continue;
                }

                builder.Build(room);
                int propOutside = CountBuiltPropsOutsideAuthoredFloor(builder, grid, room);
                int invalidTemplateProps = CountInvalidProps(room, registry);
                int floorCount = builder.LastFloorCells != null ? builder.LastFloorCells.Count : 0;
                int interiorCliffs = room.roomId == "combatlarge_twin_basins_01" ? CountInteriorCliffCandidates(room) : 0;
                int builtProps = CountBuiltProps(builder);

                bool roomOk = propOutside == 0 && invalidTemplateProps == 0;
                if (room.roomId == "combatlarge_twin_basins_01") roomOk &= interiorCliffs == 0;
                if (room.roomId == "Treasure_01") roomOk &= floorCount >= 35 && HasNoSouthDoor(room);
                ok &= roomOk;

                report.AppendLine($"{room.roomId}: floor={floorCount}, builtProps={builtProps}, propOutsideFloor={propOutside}, invalidTemplateProps={invalidTemplateProps}, interiorCliffCandidates={interiorCliffs}, ok={roomOk}");
            }

            if (ok)
            {
                string text = report.ToString() + "[RoomQCFix] PASS\n";
                WriteReport("STAGING/ROOM_QC_FIX_VERIFY_yasinderyabilgin.txt", text);
                Debug.Log(report.ToString());
                Debug.Log("[RoomQCFix] PASS");
            }
            else
            {
                WriteReport("STAGING/ROOM_QC_FIX_VERIFY_yasinderyabilgin.txt", report.ToString());
                Debug.LogError(report.ToString());
            }

            if (SceneManager.GetActiveScene().isDirty)
            {
                Debug.Log("[RoomQCFix] _Arena is dirty from verification builds; intentionally not saving.");
            }
        }

        [MenuItem("RIMA/Rooms/QC/Smoke Test All Templates")]
        public static void SmokeTestAllTemplates()
        {
            PropRegistrySO registry = LoadRegistry();
            if (registry == null) return;

            if (Application.isPlaying)
            {
                Debug.LogError("[RoomSmokeTest] Cannot run while Play mode is active.");
                return;
            }

            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }

            EditorSceneManager.OpenScene(ArenaScenePath, OpenSceneMode.Single);
            IsoRoomBuilder builder = Object.FindFirstObjectByType<IsoRoomBuilder>();
            if (builder == null)
            {
                Debug.LogError("[RoomSmokeTest] IsoRoomBuilder not found in _Arena.");
                return;
            }

            Grid grid = builder.GetComponentInParent<Grid>();
            if (grid == null)
            {
                grid = Object.FindFirstObjectByType<Grid>();
            }

            if (grid == null)
            {
                Debug.LogError("[RoomSmokeTest] Grid not found in _Arena.");
                return;
            }

            string[] guids = AssetDatabase.FindAssets("t:RoomTemplateSO", new[] { RoomsRoot });
            List<RoomTemplateSO> rooms = new List<RoomTemplateSO>();
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                RoomTemplateSO room = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(path);
                if (room != null)
                {
                    rooms.Add(room);
                }
            }
            rooms.Sort((a, b) => string.Compare(AssetDatabase.GetAssetPath(a), AssetDatabase.GetAssetPath(b), System.StringComparison.OrdinalIgnoreCase));

            int exceptions = 0;
            int propValidationFailures = 0;
            int totalProps = 0;
            StringBuilder report = new StringBuilder();
            report.AppendLine("[RoomSmokeTest] All RoomTemplateSO smoke test");
            report.AppendLine($"templates={rooms.Count}");

            for (int i = 0; i < rooms.Count; i++)
            {
                RoomTemplateSO room = rooms[i];
                string roomLabel = !string.IsNullOrEmpty(room.roomId) ? room.roomId : room.name;
                try
                {
                    builder.Build(room);
                    int floorCount = builder.LastFloorCells != null ? builder.LastFloorCells.Count : 0;
                    int builtProps = CountBuiltProps(builder);
                    int propOutside = CountBuiltPropsOutsideAuthoredFloor(builder, grid, room);
                    int invalidTemplateProps = CountInvalidProps(room, registry);
                    bool ok = propOutside == 0 && invalidTemplateProps == 0;
                    if (!ok) propValidationFailures++;
                    totalProps += builtProps;
                    report.AppendLine($"{roomLabel}: floor={floorCount}, builtProps={builtProps}, propOutsideFloor={propOutside}, invalidTemplateProps={invalidTemplateProps}, ok={ok}");
                }
                catch (System.Exception ex)
                {
                    exceptions++;
                    report.AppendLine($"{roomLabel}: exception={ex.GetType().Name}: {ex.Message}");
                }
            }

            bool pass = exceptions == 0;
            report.AppendLine($"exceptions={exceptions}, propValidationFailures={propValidationFailures}, totalBuiltProps={totalProps}");
            report.AppendLine(pass ? "[RoomSmokeTest] PASS (build exceptions=0)" : "[RoomSmokeTest] FAIL");
            WriteReport("STAGING/ROOM_SMOKE_ALL_TEMPLATES_laurethayday.txt", report.ToString());

            if (pass)
            {
                Debug.Log(report.ToString());
                if (propValidationFailures > 0)
                {
                    Debug.LogWarning($"[RoomSmokeTest] {propValidationFailures} template(s) have pre-existing prop walkable validation warnings. No assets were mutated.");
                }
                Debug.Log("[RoomSmokeTest] PASS");
            }
            else
            {
                Debug.LogError(report.ToString());
            }

            if (SceneManager.GetActiveScene().isDirty)
            {
                Debug.Log("[RoomSmokeTest] _Arena is dirty from smoke-test builds; intentionally not saving.");
            }
        }

        private static PropRegistrySO LoadRegistry()
        {
            PropRegistrySO registry = AssetDatabase.LoadAssetAtPath<PropRegistrySO>(PropRegistryPath);
            if (registry == null)
            {
                Debug.LogError($"[RoomQCFix] PropRegistry not found at {PropRegistryPath}");
                return null;
            }

            registry.RebuildIndex();
            return registry;
        }

        private static IReadOnlyList<PropDefinitionSO> BuildPropPool(PropRegistrySO registry)
        {
            List<PropDefinitionSO> pool = new List<PropDefinitionSO>();
            IReadOnlyList<PropDefinitionSO> allProps = registry.AllProps;
            if (allProps == null) return pool;

            for (int i = 0; i < allProps.Count; i++)
            {
                PropDefinitionSO prop = allProps[i];
                if (prop != null && prop.worldSprite != null)
                {
                    pool.Add(prop);
                }
            }

            return pool;
        }

        private static void ReseedProps(RoomTemplateSO room, IReadOnlyList<PropDefinitionSO> pool)
        {
            if (room.props == null)
            {
                room.props = new List<PropPlacementData>();
            }
            else
            {
                room.props.Clear();
            }

            CompositionRoleMap roleMap = CompositionRoleMapGenerator.GenerateFromRoom(room);
            BridsonPoissonAutoPlacer placer = new BridsonPoissonAutoPlacer();
            List<BridsonPoissonAutoPlacer.PlacementCandidate> candidates = null;
            int baseSeed = StableHash(room.name);
            for (int attempt = 0; attempt < SeedTries; attempt++)
            {
                List<BridsonPoissonAutoPlacer.PlacementCandidate> tryResult = placer.Generate(
                    room,
                    roleMap,
                    pool,
                    baseSeed + attempt * 13,
                    TargetDensity);
                if (tryResult.Count > 0)
                {
                    candidates = tryResult;
                    break;
                }
            }

            if (candidates == null) return;

            for (int i = 0; i < candidates.Count; i++)
            {
                if (room.props.Count >= MaxPropsPerRoom) break;

                BridsonPoissonAutoPlacer.PlacementCandidate candidate = candidates[i];
                if (candidate.prop == null) continue;

                string propPath = AssetDatabase.GetAssetPath(candidate.prop);
                string propGuid = string.IsNullOrEmpty(propPath) ? string.Empty : AssetDatabase.AssetPathToGUID(propPath);
                if (string.IsNullOrEmpty(propGuid)) continue;

                room.props.Add(new PropPlacementData(propGuid, candidate.tilePos)
                {
                    rotationSteps = candidate.rotationSteps,
                    flipX = candidate.flipX,
                    variantIndex = candidate.variantIndex,
                    placedByUser = "auto"
                });
            }
        }

        private static int FillEnclosedVoids(RoomTemplateSO room)
        {
            bool[] grid = EnsureWalkableGrid(room);
            HashSet<Vector2Int> exterior = CollectExteriorVoid(room);
            int filled = 0;

            for (int y = room.bounds.yMin; y < room.bounds.yMax; y++)
            {
                for (int x = room.bounds.xMin; x < room.bounds.xMax; x++)
                {
                    Vector2Int tile = new Vector2Int(x, y);
                    if (room.IsWalkable(tile) || exterior.Contains(tile)) continue;
                    SetWalkable(room, grid, tile, true);
                    filled++;
                }
            }

            return filled;
        }

        private static void BuildTreasureVault(RoomTemplateSO room)
        {
            room.bounds = new RectInt(0, 0, 8, 8);
            room.cameraBounds = CameraBounds.FromBounds(room.bounds);
            room.doorSockets = new List<DoorSocket>
            {
                new DoorSocket { socketId = "door_N_01", position = new Vector2Int(3, 7), direction = DoorDirection.North, widthInTiles = 2, isExit = true },
                new DoorSocket { socketId = "door_W_02", position = new Vector2Int(0, 3), direction = DoorDirection.West, widthInTiles = 2, isExit = true },
                new DoorSocket { socketId = "door_E_03", position = new Vector2Int(7, 3), direction = DoorDirection.East, widthInTiles = 2, isExit = true },
            };
            room.playerSpawn = new PlayerSpawnSocket { socketId = "player_spawn_01", position = new Vector2Int(4, 2), facing = DoorDirection.North };

            bool[] grid = new bool[room.bounds.width * room.bounds.height];
            for (int y = 0; y < room.bounds.height; y++)
            {
                for (int x = 0; x < room.bounds.width; x++)
                {
                    bool walkable =
                        (y == 7 && x >= 2 && x <= 5) ||
                        (y == 6 && x >= 1 && x <= 6) ||
                        (y == 5 && x >= 1 && x <= 6) ||
                        (y == 4) ||
                        (y == 3) ||
                        (y == 2 && x >= 1 && x <= 6) ||
                        (y == 1 && x >= 2 && x <= 5);
                    grid[y * room.bounds.width + x] = walkable;
                }
            }

            room.walkableGrid = grid;
            room.overlayMask = null;

            const string barrelGuid = "b50be24c830a450ba0ccd36625e57c31";
            room.props = new List<PropPlacementData>
            {
                new PropPlacementData(barrelGuid, new Vector2Int(2, 5)),
                new PropPlacementData(barrelGuid, new Vector2Int(5, 5)),
                new PropPlacementData(barrelGuid, new Vector2Int(2, 2)),
                new PropPlacementData(barrelGuid, new Vector2Int(5, 2)),
            };
        }

        private static int CountInvalidProps(RoomTemplateSO room, PropRegistrySO registry)
        {
            if (room == null || room.props == null) return 0;

            int invalid = 0;
            for (int i = 0; i < room.props.Count; i++)
            {
                PropPlacementData placement = room.props[i];
                PropDefinitionSO def = placement != null ? registry.ResolveGuid(placement.propDefinitionGuid) : null;
                if (placement == null || def == null || !FootprintFitsWalkable(room, def, placement.tilePosition, placement.rotationSteps))
                {
                    invalid++;
                }
            }

            return invalid;
        }

        private static bool FootprintFitsWalkable(RoomTemplateSO room, PropDefinitionSO def, Vector2Int tilePosition, int rotation)
        {
            Vector2Int size = PropFootprintValidator.GetRotatedFootprint(def, rotation);
            if (size.x <= 0 || size.y <= 0) return false;

            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    if (!room.IsWalkable(new Vector2Int(tilePosition.x + x, tilePosition.y + y)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static int CountWalkable(RoomTemplateSO room)
        {
            int count = 0;
            for (int y = room.bounds.yMin; y < room.bounds.yMax; y++)
            {
                for (int x = room.bounds.xMin; x < room.bounds.xMax; x++)
                {
                    if (room.IsWalkable(new Vector2Int(x, y))) count++;
                }
            }

            return count;
        }

        private static bool[] EnsureWalkableGrid(RoomTemplateSO room)
        {
            int len = Mathf.Max(0, room.bounds.width * room.bounds.height);
            if (room.walkableGrid != null && room.walkableGrid.Length == len)
            {
                return room.walkableGrid;
            }

            bool[] grid = new bool[len];
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = true;
            }

            room.walkableGrid = grid;
            return grid;
        }

        private static void SetWalkable(RoomTemplateSO room, bool[] grid, Vector2Int tile, bool walkable)
        {
            int lx = tile.x - room.bounds.xMin;
            int ly = tile.y - room.bounds.yMin;
            if (lx < 0 || lx >= room.bounds.width || ly < 0 || ly >= room.bounds.height) return;
            grid[ly * room.bounds.width + lx] = walkable;
        }

        private static HashSet<Vector2Int> CollectExteriorVoid(RoomTemplateSO room)
        {
            HashSet<Vector2Int> exterior = new HashSet<Vector2Int>();
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            RectInt bounds = room.bounds;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                EnqueueExterior(room, exterior, queue, new Vector2Int(x, bounds.yMin));
                EnqueueExterior(room, exterior, queue, new Vector2Int(x, bounds.yMax - 1));
            }

            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                EnqueueExterior(room, exterior, queue, new Vector2Int(bounds.xMin, y));
                EnqueueExterior(room, exterior, queue, new Vector2Int(bounds.xMax - 1, y));
            }

            Vector2Int[] dirs =
            {
                new Vector2Int(-1, -1),
                new Vector2Int(1, 1),
                new Vector2Int(1, -1),
                new Vector2Int(-1, 1),
                new Vector2Int(0, -1),
                new Vector2Int(-1, 0),
            };

            while (queue.Count > 0)
            {
                Vector2Int cell = queue.Dequeue();
                for (int i = 0; i < dirs.Length; i++)
                {
                    EnqueueExterior(room, exterior, queue, cell + dirs[i]);
                }
            }

            return exterior;
        }

        private static void EnqueueExterior(RoomTemplateSO room, HashSet<Vector2Int> exterior, Queue<Vector2Int> queue, Vector2Int cell)
        {
            if (!room.bounds.Contains(cell)) return;
            if (room.IsWalkable(cell)) return;
            if (exterior.Add(cell))
            {
                queue.Enqueue(cell);
            }
        }

        private static int CountInteriorCliffCandidates(RoomTemplateSO room)
        {
            HashSet<Vector2Int> exterior = CollectExteriorVoid(room);
            int count = 0;

            for (int y = room.bounds.yMin; y < room.bounds.yMax; y++)
            {
                for (int x = room.bounds.xMin; x < room.bounds.xMax; x++)
                {
                    Vector2Int cell = new Vector2Int(x, y);
                    if (!room.IsWalkable(cell)) continue;

                    Vector2Int sw = cell + new Vector2Int(-1, 0);
                    Vector2Int se = cell + new Vector2Int(0, -1);
                    bool swVoid = !room.IsWalkable(sw);
                    bool seVoid = !room.IsWalkable(se);
                    bool swInterior = swVoid && room.bounds.Contains(sw) && !exterior.Contains(sw);
                    bool seInterior = seVoid && room.bounds.Contains(se) && !exterior.Contains(se);
                    if (swInterior || seInterior)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private static int CountBuiltPropsOutsideAuthoredFloor(IsoRoomBuilder builder, Grid grid, RoomTemplateSO room)
        {
            Transform props = builder.transform.Find("Props");
            if (props == null) return 0;

            int outside = 0;
            for (int i = 0; i < props.childCount; i++)
            {
                Transform child = props.GetChild(i);
                Vector3Int cell = grid.WorldToCell(child.position);
                if (!room.IsWalkable(new Vector2Int(cell.x, cell.y)))
                {
                    outside++;
                }
            }

            return outside;
        }

        private static int CountBuiltProps(IsoRoomBuilder builder)
        {
            Transform props = builder.transform.Find("Props");
            return props != null ? props.childCount : 0;
        }

        private static bool HasNoSouthDoor(RoomTemplateSO room)
        {
            if (room.doorSockets == null) return true;
            for (int i = 0; i < room.doorSockets.Count; i++)
            {
                if (room.doorSockets[i] != null && room.doorSockets[i].direction == DoorDirection.South)
                {
                    return false;
                }
            }

            return true;
        }

        private static int StableHash(string value)
        {
            unchecked
            {
                int hash = 17;
                for (int i = 0; i < value.Length; i++)
                {
                    hash = hash * 31 + value[i];
                }

                return hash;
            }
        }

        private static void WriteReport(string path, string text)
        {
            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(path, text);
        }
    }
}
#endif
