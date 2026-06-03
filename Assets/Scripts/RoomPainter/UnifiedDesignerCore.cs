using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.RoomPainter
{
    /// <summary>
    /// Surface-agnostic authoring core for the Unified Designer. Holds the shared state and
    /// operations that BOTH surfaces drive identically:
    ///   - the Unity Editor window (RIMA.Editor.MapDesigner.UnifiedMapDesigner), and
    ///   - the in-game F2 overlay (RIMA.DevTools.InPlayMapPaintOverlay).
    ///
    /// It is runtime-safe (no UnityEditor / no AssetDatabase). Editor-only concerns (Undo,
    /// asset write) are injected by the host via callbacks so the same Paint/Erase/Generate/Save
    /// logic runs on both surfaces and can never drift. The active <see cref="RoomData"/> is the
    /// single source of truth; the JSON sidecar is the portable mirror.
    /// </summary>
    public sealed class UnifiedDesignerCore
    {
        // ── shared authoring state ─────────────────────────────────────────
        public RoomData ActiveRoom { get; private set; }
        public DesignerCategory Category { get; set; } = DesignerCategory.Floor;
        public string SelectedAssetId { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; } = Vector2.one;
        public bool EraseMode { get; set; }
        public int BrushSize { get; set; } = 1;
        public int SouthClearCells { get; set; } = 5;

        /// <summary>Raised after any mutation so a host view can repaint / recompose.</summary>
        public event Action Changed;

        /// <summary>Optional host hook invoked right before a mutation (e.g. Undo.RecordObject in Editor).</summary>
        public Action<RoomData, string> BeforeMutate;

        public bool HasRoom => ActiveRoom != null;

        public void SetActiveRoom(RoomData room)
        {
            ActiveRoom = room;
            if (room != null) room.EnsureDefaults();
            RaiseChanged();
        }

        // ── placement (category-routed, identical on both surfaces) ────────
        public void Paint(Vector3Int cell, Vector3 worldPos)
        {
            if (ActiveRoom == null) return;
            BeforeMutate?.Invoke(ActiveRoom, "Paint " + DesignerCategoryMap.Label(Category));

            int min = -(BrushSize - 1) / 2;
            int max = BrushSize / 2;
            for (int dx = min; dx <= max; dx++)
            {
                for (int dy = min; dy <= max; dy++)
                {
                    Vector3Int t = cell + new Vector3Int(dx, dy, 0);
                    Vector3 tWorld = worldPos + new Vector3(dx, dy, 0f);
                    UnifiedPaintVariantResolver.PaintResolution resolution =
                        UnifiedPaintVariantResolver.Resolve(ActiveRoom, Category, SelectedAssetId, t);
                    RoomDataMutator.PutCategory(
                        ActiveRoom,
                        Category,
                        resolution.assetGuidOrName,
                        resolution.sourceGroupId,
                        t,
                        tWorld,
                        Rotation,
                        Scale);
                    UnifiedPaintVariantResolver.ReResolveAffected(ActiveRoom, Category, t);
                }
            }

            RaiseChanged();
        }

        public void Erase(Vector3Int cell)
        {
            if (ActiveRoom == null) return;
            BeforeMutate?.Invoke(ActiveRoom, "Erase " + DesignerCategoryMap.Label(Category));

            int min = -(BrushSize - 1) / 2;
            int max = BrushSize / 2;
            for (int dx = min; dx <= max; dx++)
            {
                for (int dy = min; dy <= max; dy++)
                {
                    Vector3Int t = cell + new Vector3Int(dx, dy, 0);
                    RoomDataMutator.RemoveCategory(ActiveRoom, Category, t);
                    UnifiedPaintVariantResolver.ReResolveAffected(ActiveRoom, Category, t);
                }
            }

            RaiseChanged();
        }

        /// <summary>Place or erase based on <see cref="EraseMode"/>.</summary>
        public void Apply(Vector3Int cell, Vector3 worldPos)
        {
            if (EraseMode) Erase(cell);
            else Paint(cell, worldPos);
        }

        // ── logical cliff generation (RoomData-first, the user's #1 fix) ───
        /// <summary>
        /// Regenerate the cliff ring purely from the active room's floor shape. Replaces
        /// cliffCells with the solver output. cellToWorld converts a cell to a world position
        /// for the stored record (pass the grid's CellToCenter; identity-safe if null).
        /// Returns the number of cliff cells generated.
        /// </summary>
        public int GenerateCliffsFromFloor(Func<Vector3Int, Vector3> cellToWorld = null, string cliffAssetId = null)
        {
            if (ActiveRoom == null) return 0;
            BeforeMutate?.Invoke(ActiveRoom, "Generate Cliffs");

            HashSet<Vector3Int> cliffs = RoomCliffSolver.SolveFromRoom(ActiveRoom, SouthClearCells);

            // Preserve existing cliff art id when caller doesn't specify one.
            string assetId = cliffAssetId;
            if (string.IsNullOrEmpty(assetId) && ActiveRoom.cliffCells.Count > 0)
            {
                assetId = ActiveRoom.cliffCells[0].assetGuidOrName;
            }

            ActiveRoom.cliffCells.Clear();
            foreach (Vector3Int c in cliffs)
            {
                Vector3 world = cellToWorld != null ? cellToWorld(c) : Vector3.zero;
                UnifiedPaintVariantResolver.PaintResolution resolution =
                    UnifiedPaintVariantResolver.Resolve(ActiveRoom, DesignerCategory.Cliff, assetId, c);
                RoomDataMutator.PutCliffCell(
                    ActiveRoom,
                    resolution.assetGuidOrName,
                    resolution.sourceGroupId,
                    c,
                    world,
                    0f,
                    Vector2.one);
            }

            RaiseChanged();
            return cliffs.Count;
        }

        /// <summary>Preview-only count without mutating (for UI feedback).</summary>
        public int PreviewCliffCount()
        {
            if (ActiveRoom == null) return 0;
            return RoomCliffSolver.SolveFromRoom(ActiveRoom, SouthClearCells).Count;
        }

        // ── persistence (JSON portable; asset write is host-injected) ──────
        /// <summary>Save the JSON sidecar. Asset (.asset) sync is the host's responsibility (Editor-only).</summary>
        public void SaveJson()
        {
            if (ActiveRoom == null) return;
            ActiveRoom.EnsureDefaults();
            RoomDataJson.Write(ActiveRoom, RoomDataPaths.JsonFor(ActiveRoom.roomId));
        }

        public void RaiseChanged() => Changed?.Invoke();
    }
}
