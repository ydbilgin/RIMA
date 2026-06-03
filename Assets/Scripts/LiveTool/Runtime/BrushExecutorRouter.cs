// TARGET: Assets/Scripts/LiveTool/Runtime/BrushExecutorRouter.cs
// C-? (BrushExecutorRouter, runtime) — NEW, not a copy of the Editor router.
//
// Responsibility (BUILD CONTRACT §2 "BrushExecutorRouter (runtime)"):
//   Minimal runtime paint dispatcher. Given a selected RegistryEntry, a paint
//   PaletteMode, and a grid cell / world position, it:
//     (a) tiles  -> floorTilemap.SetTile(cell, entry.tile)
//     (b) cliffs -> cliffTilemap.SetTile(cell, entry.tile) + records tile_guid
//     (c) props  -> Instantiate(entry.prefab, pos, rot, propRoot)
//     (d) erase  -> clear tile / destroy nearest instance
//   and mutates the in-memory RoomLayoutData so ToolBootstrap.RequestSave()
//   serializes truth.
//
//   It deliberately does NOT replicate the Editor router's
//   MapDesignerBrushPresetSO / RoomData / Undo / composite / scatter pipeline
//   (spec §1.5 "lean", §5.3 "70% of Editor quality, accepted").
//
// Player-build-safe: NO UnityEditor, NO Undo, NO AssetDatabase, NO Handles.
// Compiles only under the RIMA_LIVE_TOOL define (see §1 Assembly Strategy:
// the whole RIMA.LiveTool asmdef is gated by defineConstraints RIMA_LIVE_TOOL,
// so the #if here is belt-and-suspenders parity with the spec, and keeps the
// file inert if it is ever pulled into a non-Tool assembly).

#if RIMA_LIVE_TOOL

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using RIMA.Live;                 // RoomLayoutData, RegistryEntry, RuntimeAssetRegistry, FloorTileData, CliffCellData, PropData
using RIMA.RoomPainter;          // RoomLayer (referenced via RegistryEntry.layer + cliff classification)

namespace RIMA.LiveTool
{
    /// <summary>
    /// Runtime paint-action router for the standalone Live Tool (Tool.exe).
    /// Applies brush placements to the live room (tilemaps + prefab instances)
    /// and keeps the in-memory <see cref="RoomLayoutData"/> in sync so the
    /// serializer writes ground truth. No Editor APIs — ships in Tool.exe.
    /// </summary>
    public sealed class BrushExecutorRouter
    {
        // ── Result of a paint/erase call ───────────────────────────────────────

        /// <summary>Outcome of a single Paint/Erase operation.</summary>
        public struct PaintResult
        {
            /// <summary>True if the room state actually changed (tile set/cleared, prop spawned/destroyed).</summary>
            public bool changed;

            /// <summary>The instance spawned by a prop/decor paint, else null.</summary>
            public GameObject spawned;
        }

        // ── Dependencies (injected) ────────────────────────────────────────────

        private readonly Tilemap _floorTilemap;
        private readonly Tilemap _cliffTilemap;
        private readonly Transform _propRoot;
        private readonly RuntimeAssetRegistry _registry;

        // Live mapping from a prop instance_id -> spawned GameObject, so Erase can
        // destroy the exact instance and keep RoomLayoutData.prop_instances aligned.
        private readonly Dictionary<string, GameObject> _spawnedProps =
            new Dictionary<string, GameObject>(System.StringComparer.Ordinal);

        // Monotonic counter for unique instance ids within this Tool session.
        private int _propSeq;

        // ── Construction ───────────────────────────────────────────────────────

        /// <summary>
        /// Wire the router to the live scene's two tilemaps, the prop parent
        /// transform, and the baked registry. All four are required.
        /// </summary>
        public BrushExecutorRouter(Tilemap floorTilemap, Tilemap cliffTilemap, Transform propRoot, RuntimeAssetRegistry registry)
        {
            _floorTilemap = floorTilemap;
            _cliffTilemap = cliffTilemap;
            _propRoot     = propRoot;
            _registry     = registry;

            if (_floorTilemap == null) Debug.LogWarning("[BrushExecutorRouter] floorTilemap is null — tile paints will no-op.");
            if (_cliffTilemap == null) Debug.LogWarning("[BrushExecutorRouter] cliffTilemap is null — cliff paints will no-op.");
            if (_registry     == null) Debug.LogError  ("[BrushExecutorRouter] registry is null — all paints will no-op.");
        }

        // ── Paint ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Apply a brush placement of <paramref name="entry"/> at <paramref name="cell"/> /
        /// <paramref name="worldPos"/> according to <paramref name="mode"/>, mutating
        /// <paramref name="liveDoc"/> so the next save reflects the change.
        /// </summary>
        /// <param name="mode">Active palette mode (Tile/Cliff/Decor/Object; All resolves per entry kind).</param>
        /// <param name="entry">Selected registry entry to place. No-op if null.</param>
        /// <param name="cell">Snapped grid cell (z used as-is).</param>
        /// <param name="worldPos">World position for prop instantiation.</param>
        /// <param name="rotationZ">Z-axis rotation in degrees for props.</param>
        /// <param name="liveDoc">In-memory room document to mutate. No-op if null.</param>
        public PaintResult Paint(PaletteMode mode, RegistryEntry entry, Vector3Int cell, Vector3 worldPos, float rotationZ, RoomLayoutData liveDoc)
        {
            PaintResult result = default;

            if (entry == null || liveDoc == null || _registry == null)
                return result;

            // Resolve effective action from mode; "All" defers to the entry's kind.
            EffectiveAction action = ResolveAction(mode, entry);

            switch (action)
            {
                case EffectiveAction.Tile:  return PaintTile(entry, cell, liveDoc);
                case EffectiveAction.Cliff: return PaintCliff(entry, cell, liveDoc);
                case EffectiveAction.Prop:  return PaintProp(entry, worldPos, rotationZ, liveDoc);
                default:                    return result;
            }
        }

        // ── Erase ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Erase at <paramref name="cell"/> / <paramref name="worldPos"/> according to
        /// <paramref name="mode"/>: clears the relevant tile, or destroys the nearest
        /// prop instance. Mutates <paramref name="liveDoc"/> accordingly.
        /// </summary>
        public PaintResult Erase(PaletteMode mode, Vector3Int cell, Vector3 worldPos, RoomLayoutData liveDoc)
        {
            PaintResult result = default;
            if (liveDoc == null) return result;

            switch (mode)
            {
                case PaletteMode.Tile:
                    return EraseTile(cell, liveDoc);

                case PaletteMode.Cliff:
                    return EraseCliff(cell, liveDoc);

                case PaletteMode.Decor:
                case PaletteMode.Object:
                    return EraseNearestProp(worldPos, liveDoc);

                case PaletteMode.All:
                default:
                    // In "All" mode try props first (most likely intent under cursor),
                    // then fall back to clearing tiles at the cell.
                    PaintResult propErase = EraseNearestProp(worldPos, liveDoc);
                    if (propErase.changed) return propErase;
                    PaintResult cliffErase = EraseCliff(cell, liveDoc);
                    if (cliffErase.changed) return cliffErase;
                    return EraseTile(cell, liveDoc);
            }
        }

        // ── Floor tiles ────────────────────────────────────────────────────────

        private PaintResult PaintTile(RegistryEntry entry, Vector3Int cell, RoomLayoutData liveDoc)
        {
            PaintResult result = default;

            TileBase tile = entry.tile != null ? entry.tile : _registry.GetTile(entry.guid);
            if (tile == null || _floorTilemap == null)
                return result; // graceful-degrade: nothing to place

            _floorTilemap.SetTile(cell, tile);

            // Mirror into RoomLayoutData.floor_tiles (replace existing at this cell).
            FloorTileData existing = FindFloorAt(liveDoc, cell);
            if (existing != null)
            {
                existing.tile_guid = entry.guid;
            }
            else
            {
                liveDoc.floor_tiles.Add(new FloorTileData
                {
                    cell = new[] { cell.x, cell.y, cell.z },
                    tile_guid = entry.guid,
                });
            }

            result.changed = true;
            return result;
        }

        private PaintResult EraseTile(Vector3Int cell, RoomLayoutData liveDoc)
        {
            PaintResult result = default;
            if (_floorTilemap == null) return result;

            bool hadTile = _floorTilemap.GetTile(cell) != null;
            _floorTilemap.SetTile(cell, null);

            FloorTileData existing = FindFloorAt(liveDoc, cell);
            if (existing != null)
            {
                liveDoc.floor_tiles.Remove(existing);
                result.changed = true;
            }
            else if (hadTile)
            {
                // Tilemap had a tile not tracked in the doc (e.g. baked) — still a change.
                result.changed = true;
            }

            return result;
        }

        // ── Cliff tiles (closes the gap's #1 "cliff no-op" by writing tile_guid) ─

        private PaintResult PaintCliff(RegistryEntry entry, Vector3Int cell, RoomLayoutData liveDoc)
        {
            PaintResult result = default;

            TileBase tile = entry.tile != null ? entry.tile : _registry.GetTile(entry.guid);
            if (tile == null || _cliffTilemap == null)
                return result;

            _cliffTilemap.SetTile(cell, tile);

            // is_decor mirrors the palette/Decals classification of the entry.
            // RoomLayer.Decals => decorative cliff overlay (BUILD CONTRACT §C7/§F4 layer stack).
            bool isDecor = entry.layer == RoomLayer.Decals;

            CliffCellData existing = FindCliffAt(liveDoc, cell);
            if (existing != null)
            {
                existing.tile_guid = entry.guid;   // mandatory on write — LiveRoomReloader.ApplyCliffTiles consumes it
                existing.is_decor  = isDecor;
            }
            else
            {
                liveDoc.cliff_cells.Add(new CliffCellData
                {
                    cell = new[] { cell.x, cell.y, cell.z },
                    tile_guid = entry.guid,
                    is_decor = isDecor,
                });
            }

            result.changed = true;
            return result;
        }

        private PaintResult EraseCliff(Vector3Int cell, RoomLayoutData liveDoc)
        {
            PaintResult result = default;
            if (_cliffTilemap == null) return result;

            bool hadTile = _cliffTilemap.GetTile(cell) != null;
            _cliffTilemap.SetTile(cell, null);

            CliffCellData existing = FindCliffAt(liveDoc, cell);
            if (existing != null)
            {
                liveDoc.cliff_cells.Remove(existing);
                result.changed = true;
            }
            else if (hadTile)
            {
                result.changed = true;
            }

            return result;
        }

        // ── Props / decor objects ──────────────────────────────────────────────

        private PaintResult PaintProp(RegistryEntry entry, Vector3 worldPos, float rotationZ, RoomLayoutData liveDoc)
        {
            PaintResult result = default;

            GameObject prefab = entry.prefab != null ? entry.prefab : _registry.GetPrefab(entry.guid);
            if (prefab == null)
                return result; // graceful-degrade: prefab-less entry, nothing to spawn

            Quaternion rot = Quaternion.Euler(0f, 0f, rotationZ);
            GameObject instance = Object.Instantiate(prefab, worldPos, rot, _propRoot);

            // Stable, explicit instance_id so LiveRoomReloader's StableId() diffing
            // (Assets/Scripts/Live/LiveRoomReloader.cs:371) keys on it directly and
            // does NOT recreate every later prop when the middle of the list changes.
            string instanceId = $"{entry.guid}_{_propSeq++}";
            _spawnedProps[instanceId] = instance;

            liveDoc.prop_instances.Add(new PropData
            {
                prefab_guid = entry.guid,
                position    = new[] { worldPos.x, worldPos.y, worldPos.z },
                rotation    = rotationZ,
                instance_id = instanceId,
            });

            result.changed = true;
            result.spawned = instance;
            return result;
        }

        private PaintResult EraseNearestProp(Vector3 worldPos, RoomLayoutData liveDoc)
        {
            PaintResult result = default;
            if (liveDoc.prop_instances == null || liveDoc.prop_instances.Count == 0)
                return result;

            // Find the closest tracked prop in the doc by recorded position.
            PropData nearest = null;
            float bestSqr = float.MaxValue;
            foreach (PropData p in liveDoc.prop_instances)
            {
                Vector3 pos = ToVector3(p.position);
                float sqr = (pos - worldPos).sqrMagnitude;
                if (sqr < bestSqr)
                {
                    bestSqr = sqr;
                    nearest = p;
                }
            }

            if (nearest == null)
                return result;

            // ASSUMPTION: an erase-radius of 1.0 world unit (= 1 cell at PPU64 cellSize 1)
            // is the hit tolerance for "nearest" prop. Beyond that, treat as a miss so the
            // user doesn't accidentally delete a far prop. Spec gives no explicit radius;
            // 1 cell matches the grid snap granularity used elsewhere in the contract.
            const float eraseRadiusSqr = 1.0f * 1.0f;
            if (bestSqr > eraseRadiusSqr)
                return result;

            // Destroy the live instance if we still hold it.
            if (!string.IsNullOrEmpty(nearest.instance_id) &&
                _spawnedProps.TryGetValue(nearest.instance_id, out GameObject go))
            {
                if (go != null) Object.Destroy(go);
                _spawnedProps.Remove(nearest.instance_id);
            }

            // Also drop any collider override bound to this instance so it isn't orphaned.
            RemoveColliderOverride(liveDoc, nearest.instance_id);

            liveDoc.prop_instances.Remove(nearest);
            result.changed = true;
            return result;
        }

        // ── Mode resolution ────────────────────────────────────────────────────

        private enum EffectiveAction { None, Tile, Cliff, Prop }

        private static EffectiveAction ResolveAction(PaletteMode mode, RegistryEntry entry)
        {
            switch (mode)
            {
                case PaletteMode.Tile:   return EffectiveAction.Tile;
                case PaletteMode.Cliff:  return EffectiveAction.Cliff;
                case PaletteMode.Decor:  return EffectiveAction.Prop;
                case PaletteMode.Object: return EffectiveAction.Prop;

                case PaletteMode.All:
                default:
                    // Resolve by the entry's runtime role (mirrors PassesModeFilter logic
                    // in RuntimeBrushPalette: cliff tag, then tile kind, then prefab).
                    if (entry.tag == "cliff") return EffectiveAction.Cliff;
                    if (entry.kind == AssetKind.Tile || entry.kind == AssetKind.TileAndSprite)
                        return EffectiveAction.Tile;
                    if (entry.prefab != null || entry.kind == AssetKind.Prefab)
                        return EffectiveAction.Prop;
                    // Fall back to tile if it carries a TileBase, else prop, else nothing.
                    if (entry.tile != null) return EffectiveAction.Tile;
                    return EffectiveAction.None;
            }
        }

        // ── RoomLayoutData lookup helpers ──────────────────────────────────────

        private static FloorTileData FindFloorAt(RoomLayoutData doc, Vector3Int cell)
        {
            if (doc.floor_tiles == null) return null;
            foreach (FloorTileData ft in doc.floor_tiles)
                if (CellEquals(ft.cell, cell)) return ft;
            return null;
        }

        private static CliffCellData FindCliffAt(RoomLayoutData doc, Vector3Int cell)
        {
            if (doc.cliff_cells == null) return null;
            foreach (CliffCellData ct in doc.cliff_cells)
                if (CellEquals(ct.cell, cell)) return ct;
            return null;
        }

        private static void RemoveColliderOverride(RoomLayoutData doc, string instanceId)
        {
            if (doc.collider_overrides == null || string.IsNullOrEmpty(instanceId)) return;
            for (int i = doc.collider_overrides.Count - 1; i >= 0; i--)
            {
                if (doc.collider_overrides[i] != null &&
                    doc.collider_overrides[i].instance_id == instanceId)
                {
                    doc.collider_overrides.RemoveAt(i);
                }
            }
        }

        private static bool CellEquals(int[] cell, Vector3Int v)
        {
            return cell != null && cell.Length >= 3 &&
                   cell[0] == v.x && cell[1] == v.y && cell[2] == v.z;
        }

        private static Vector3 ToVector3(float[] v)
        {
            if (v == null || v.Length < 3) return Vector3.zero;
            return new Vector3(v[0], v[1], v[2]);
        }
    }
}

#endif // RIMA_LIVE_TOOL
