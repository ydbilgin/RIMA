#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using RIMA.Data;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Stroke;
using RIMA.MapDesigner.Brush.Executors.Editor;

namespace RIMA.MapDesigner.VisualEditor
{
    public class VisualEditorScenePainter
    {
        private readonly RimaVisualMapEditorWindow window;
        private readonly BrushExecutorRouter router;
        private bool dragActive;
        private Vector2 lastDragWorld;
        private int activeControlId;
        private int undoGroupAtPress;

        private GameObject ghostObject;
        private GameObject lastSelectedPrefab;
        private Sprite lastSelectedSprite;

        // Reusable dummy walkable grid for ApplyStroke RoomData synthesis.
        // 500x500 = 250K bools allocated once, cleared on reuse (S110 Phase 1).
        private static bool[,] _dummyWalkableCache;

        public VisualEditorScenePainter(RimaVisualMapEditorWindow w)
        {
            window = w;
            router = new BrushExecutorRouter();
        }

        public void Cleanup()
        {
            UpdateGhostObject(null, null, Vector3.zero, 0f);
        }

        public void OnSceneGUI(SceneView sceneView)
        {
            if (window == null) return;

            Event e = Event.current;

            // Handle keyboard shortcut for Rotation (R key)
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.R)
            {
                window.RotateBrush(90f);
                e.Use();
                return;
            }

            if (window.ToolMode != VisualBrushToolMode.Brush && window.ToolMode != VisualBrushToolMode.Erase)
            {
                UpdateGhostObject(null, null, Vector3.zero, 0f);
                return;
            }

            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            HandleUtility.AddDefaultControl(controlID);

            Vector2 worldPos = GetMouseWorldPos(e);
            
            // Map world pos to grid cell using active grid in scene
            Grid activeGrid = Object.FindAnyObjectByType<Grid>();
            Vector3Int cell3D = Vector3Int.zero;
            if (activeGrid != null)
            {
                cell3D = activeGrid.WorldToCell(new Vector3(worldPos.x, worldPos.y, 0f));
            }
            Vector2Int cell = new Vector2Int(cell3D.x, cell3D.y);

            // Snapped position for preview
            Vector3 snappedWorldPos = new Vector3(worldPos.x, worldPos.y, 0f);
            if (activeGrid != null)
            {
                snappedWorldPos = activeGrid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0));
            }

            // Draw visual snapping preview
            DrawSnappingGhost(worldPos, cell, activeGrid);

            // Manage temporary Ghost GameObject for Prefabs/Sprites preview
            GameObject targetPrefab = null;
            Sprite targetSprite = null;

            if (window.ToolMode == VisualBrushToolMode.Brush && window.SelectedBrush != null && 
                window.SelectedBrush.operations != null && window.SelectedBrush.operations.Count > 0)
            {
                AssetPoolSO pool = window.SelectedBrush.operations[0].assetPool;
                if (pool != null)
                {
                    if (pool.prefabs != null && pool.prefabs.Count > 0)
                    {
                        targetPrefab = pool.prefabs[0];
                    }
                    else if (pool.sprites != null && pool.sprites.Count > 0)
                    {
                        targetSprite = pool.sprites[0];
                    }
                    else if (pool.variants != null && pool.variants.Count > 0 && pool.variants[0] != null)
                    {
                        targetSprite = pool.variants[0].sprite;
                    }
                }
            }

            UpdateGhostObject(targetPrefab, targetSprite, snappedWorldPos, window.CurrentRotation);

            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0 && e.alt)
                    {
                        // D5: Alt+Click erase — register cliff cell in manual override blacklist
                        ApplyCliffErase(worldPos, cell3D, activeGrid);
                        e.Use();
                    }
                    else if (e.button == 0 && !e.alt)
                    {
                        GUIUtility.hotControl = controlID;
                        activeControlId = controlID;
                        dragActive = true;
                        lastDragWorld = worldPos;
                        BeginStrokeUndoGroup();
                        ApplyStroke(worldPos, cell, isDrag: false);
                        e.Use();
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == controlID && dragActive)
                    {
                        ApplyStroke(worldPos, cell, isDrag: true);
                        lastDragWorld = worldPos;
                        e.Use();
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlID && dragActive)
                    {
                        EndStrokeUndoGroup();
                        dragActive = false;
                        GUIUtility.hotControl = 0;
                        e.Use();
                        
                        // Trigger autotile on completion of stroke
                        LiveAutotiler.TriggerLiveAutotile();
                    }
                    break;
                case EventType.Repaint:
                    SceneView.RepaintAll();
                    break;
            }
        }

        private Vector2 GetMouseWorldPos(Event e)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            return new Vector2(ray.origin.x, ray.origin.y);
        }

        private void UpdateGhostObject(GameObject targetPrefab, Sprite targetSprite, Vector3 position, float rotationAngle)
        {
            if (targetPrefab == null && targetSprite == null)
            {
                if (ghostObject != null)
                {
                    Object.DestroyImmediate(ghostObject);
                    ghostObject = null;
                }
                lastSelectedPrefab = null;
                lastSelectedSprite = null;
                return;
            }

            if (ghostObject == null || lastSelectedPrefab != targetPrefab || lastSelectedSprite != targetSprite)
            {
                if (ghostObject != null)
                {
                    Object.DestroyImmediate(ghostObject);
                }
                
                lastSelectedPrefab = targetPrefab;
                lastSelectedSprite = targetSprite;

                if (targetPrefab != null)
                {
                    ghostObject = Object.Instantiate(targetPrefab);
                }
                else
                {
                    ghostObject = new GameObject("[Visual Editor Preview Ghost]");
                    SpriteRenderer sr = ghostObject.AddComponent<SpriteRenderer>();
                    sr.sprite = targetSprite;
                }

                ghostObject.name = "[Visual Editor Preview Ghost]";
                ghostObject.hideFlags = HideFlags.HideAndDontSave;

                // Strip script components & colliders to avoid physics issues
                foreach (var col in ghostObject.GetComponentsInChildren<Collider>())
                {
                    Object.DestroyImmediate(col);
                }
                foreach (var rb in ghostObject.GetComponentsInChildren<Rigidbody>())
                {
                    Object.DestroyImmediate(rb);
                }
                foreach (var behaviour in ghostObject.GetComponentsInChildren<MonoBehaviour>())
                {
                    // Keep SpriteRenderer for visual feedback, destroy everything else
                    if (!(behaviour is SpriteRenderer))
                    {
                        Object.DestroyImmediate(behaviour);
                    }
                }

                // Render semi-transparent (alpha 0.5)
                foreach (var sr in ghostObject.GetComponentsInChildren<SpriteRenderer>())
                {
                    sr.color = new Color(0.6f, 0.9f, 1f, 0.6f); // Cool cyan tinted transparent look
                }
                foreach (var mr in ghostObject.GetComponentsInChildren<MeshRenderer>())
                {
                    foreach (var mat in mr.sharedMaterials)
                    {
                        if (mat.HasProperty("_Color"))
                        {
                            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.5f);
                        }
                    }
                }
            }

            if (ghostObject != null)
            {
                ghostObject.transform.position = position;
                ghostObject.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
            }
        }

        private void DrawSnappingGhost(Vector2 worldPos, Vector2Int cell, Grid grid)
        {
            if (window.SelectedBrush == null) return;
            Handles.color = new Color(0.2f, 0.7f, 1f, 0.4f);

            if (grid != null)
            {
                // Draw snapping diamond representing the target cell
                Vector3 cellCenter = grid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0));
                Vector3 cellSize = grid.cellSize;
                
                // Draw snapping diamond outline in isometric space
                Vector3 pTop = cellCenter + new Vector3(0, cellSize.y / 2f, 0);
                Vector3 pBottom = cellCenter - new Vector3(0, cellSize.y / 2f, 0);
                Vector3 pLeft = cellCenter - new Vector3(cellSize.x / 2f, 0, 0);
                Vector3 pRight = cellCenter + new Vector3(cellSize.x / 2f, 0, 0);
                
                Handles.DrawAAConvexPolygon(pTop, pRight, pBottom, pLeft);
                
                // Outer circle for brush radius preview
                float radius = Mathf.Max(0.25f, window.BrushSize / 64f);
                Handles.color = new Color(0.1f, 0.6f, 1f, 0.6f);
                Handles.DrawWireDisc(new Vector3(worldPos.x, worldPos.y, 0f), Vector3.forward, radius);
            }
        }

        private void BeginStrokeUndoGroup()
        {
            Undo.IncrementCurrentGroup();
            undoGroupAtPress = Undo.GetCurrentGroup();
            string label = window.SelectedBrush != null ? $"RIMA Visual Paint: {window.SelectedBrush.brushName}" : "RIMA Visual Paint Stroke";
            Undo.SetCurrentGroupName(label);
        }

        private void EndStrokeUndoGroup()
        {
            Undo.CollapseUndoOperations(undoGroupAtPress);
        }

        private void ApplyStroke(Vector2 worldPos, Vector2Int cell, bool isDrag)
        {
            var brush = window.SelectedBrush;
            if (brush == null || brush.operations == null || brush.operations.Count == 0) return;

            Grid activeGrid = Object.FindAnyObjectByType<Grid>();
            float brushRadiusUnits = window.BrushSize / 64f;

            // 1. Build a dummy RoomData walkable grid (all true) to satisfy proximity checks.
            // S110 Phase 1: reuse static cache instead of per-stroke 250K bool alloc.
            if (_dummyWalkableCache == null || _dummyWalkableCache.GetLength(0) != 500)
            {
                _dummyWalkableCache = new bool[500, 500];
            }
            else
            {
                System.Array.Clear(_dummyWalkableCache, 0, _dummyWalkableCache.Length);
            }
            for (int x = 0; x < 500; x++)
            {
                for (int y = 0; y < 500; y++)
                {
                    _dummyWalkableCache[x, y] = true;
                }
            }
            RoomData dummyRoom = new RoomData
            {
                size = new Vector2Int(500, 500),
                seed = window.ActiveSeed,
                walkable = _dummyWalkableCache,
                wallEdges = new List<WallSegment>()
            };

            // 2. Base BrushStroke template
            BrushStroke baseStroke = new BrushStroke
            {
                startPositionWorld = isDrag ? lastDragWorld : worldPos,
                currentPositionWorld = worldPos,
                startCell = cell,
                currentCell = cell,
                isDrag = isDrag,
                seed = window.ActiveSeed + cell.x * 17 + cell.y * 31, // salt seed by position
                biomeSkin = window.ActiveSkin,
                room = dummyRoom
            };

            if (window.ToolMode == VisualBrushToolMode.Brush)
            {
                Random.InitState(baseStroke.seed);

                foreach (var op in brush.operations)
                {
                    if (op == null) continue;

                    bool isTileLayer = (op.targetLayer == TargetLayer.L1 || op.targetLayer == TargetLayer.L2);

                    if (isTileLayer)
                    {
                        // S110 Phase 2B: detect if we are painting the cliff tilemap so we can
                        // update the manual-painted whitelist (and clear the blacklist) for the
                        // painted cells — mirrors the ERASE branch cliff detection below.
                        RIMA.Environment.CliffAutoPlacer paintCliffPlacer = null;
                        bool isPaintingCliffTilemap = false;
                        if (brush.category == BrushCategory.Cliff ||
                            brush.category == BrushCategory.Transition ||
                            brush.category == BrushCategory.Composite)
                        {
                            string errMsg;
                            Tilemap paintTargetTilemap = AutoLayeringService.FindTargetTilemap(brush, out errMsg);
                            if (paintTargetTilemap != null)
                            {
                                paintCliffPlacer = Object.FindAnyObjectByType<RIMA.Environment.CliffAutoPlacer>();
                                isPaintingCliffTilemap = paintCliffPlacer != null && paintCliffPlacer.cliffTilemap == paintTargetTilemap;
                            }
                        }

                        if (brushRadiusUnits <= 0.5f)
                        {
                            // Single snapped tile placement
                            var stroke = baseStroke;
                            stroke.currentCell = cell;
                            if (activeGrid != null)
                            {
                                stroke.currentPositionWorld = activeGrid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0));
                            }
                            stroke.startCell = stroke.currentCell;

                            // Pre-emptive: update cliff override lists before dispatch so the
                            // tile survives even if dispatch partially fails.
                            if (isPaintingCliffTilemap)
                            {
                                Vector3Int cell3D = new Vector3Int(cell.x, cell.y, 0);
                                paintCliffPlacer.AddManualPainted(cell3D);
                            }

                            bool originalWalkable = op.respectsWalkableMask;
                            op.respectsWalkableMask = false;
                            try
                            {
                                router.Dispatch(stroke, op, brush);
                            }
                            finally
                            {
                                op.respectsWalkableMask = originalWalkable;
                            }
                        }
                        else
                        {
                            // Multi-tile radius placement
                            int radiusInCells = Mathf.Max(1, Mathf.RoundToInt(brushRadiusUnits));
                            for (int x = -radiusInCells; x <= radiusInCells; x++)
                            {
                                for (int y = -radiusInCells; y <= radiusInCells; y++)
                                {
                                    Vector3Int cellPos3D = new Vector3Int(cell.x + x, cell.y + y, 0);
                                    Vector3 cellWorldPos = cellPos3D;
                                    if (activeGrid != null)
                                    {
                                        cellWorldPos = activeGrid.GetCellCenterWorld(cellPos3D);
                                    }

                                    float dist = Vector2.Distance(cellWorldPos, worldPos);
                                    if (dist <= brushRadiusUnits)
                                    {
                                        if (Random.value <= window.BrushDensity)
                                        {
                                            var stroke = baseStroke;
                                            stroke.currentCell = new Vector2Int(cellPos3D.x, cellPos3D.y);
                                            stroke.currentPositionWorld = cellWorldPos;
                                            stroke.startCell = stroke.currentCell;

                                            // Pre-emptive: update cliff override lists before dispatch.
                                            if (isPaintingCliffTilemap)
                                            {
                                                paintCliffPlacer.AddManualPainted(cellPos3D);
                                            }

                                            bool originalWalkable = op.respectsWalkableMask;
                                            op.respectsWalkableMask = false;
                                            try
                                            {
                                                router.Dispatch(stroke, op, brush);
                                            }
                                            finally
                                            {
                                                op.respectsWalkableMask = originalWalkable;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // Decorative/Prop layer L3, L4, L5, L6
                        if (brushRadiusUnits <= 0.5f)
                        {
                            // Single snapped placement
                            var stroke = baseStroke;
                            stroke.currentCell = cell;
                            if (activeGrid != null)
                            {
                                stroke.currentPositionWorld = activeGrid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0));
                            }
                            stroke.startCell = stroke.currentCell;

                            // Prevent duplicates
                            Transform root = FindLayerRoot(op.targetLayer);
                            bool alreadyExists = false;
                            if (root != null)
                            {
                                foreach (Transform child in root)
                                {
                                    if (Vector3.Distance(child.position, stroke.currentPositionWorld) < 0.1f)
                                    {
                                        alreadyExists = true;
                                        break;
                                    }
                                }
                            }

                            if (!alreadyExists)
                            {
                                bool originalWalkable = op.respectsWalkableMask;
                                op.respectsWalkableMask = false;
                                try
                                {
                                    var result = router.Dispatch(stroke, op, brush);
                                    if (result.success && result.spawnedObjects != null)
                                    {
                                        foreach (var go in result.spawnedObjects)
                                        {
                                            if (go != null)
                                            {
                                                go.transform.rotation = Quaternion.Euler(0f, 0f, window.CurrentRotation);
                                            }
                                        }
                                    }
                                }
                                finally
                                {
                                    op.respectsWalkableMask = originalWalkable;
                                }
                            }
                        }
                        else
                        {
                            // Scattered radius placement
                            float area = Mathf.PI * brushRadiusUnits * brushRadiusUnits;
                            int spawnAttempts = Mathf.Max(1, Mathf.RoundToInt(area * window.BrushDensity * 2f));

                            for (int i = 0; i < spawnAttempts; i++)
                            {
                                Vector2 offset = Random.insideUnitCircle * brushRadiusUnits;
                                Vector3 spawnPos = new Vector3(worldPos.x + offset.x, worldPos.y + offset.y, 0f);
                                Vector2Int spawnCell = cell;
                                if (activeGrid != null)
                                {
                                    Vector3Int c3d = activeGrid.WorldToCell(spawnPos);
                                    spawnCell = new Vector2Int(c3d.x, c3d.y);
                                }

                                var stroke = baseStroke;
                                stroke.currentPositionWorld = spawnPos;
                                stroke.currentCell = spawnCell;
                                stroke.startCell = spawnCell;

                                bool originalWalkable = op.respectsWalkableMask;
                                op.respectsWalkableMask = false;
                                try
                                {
                                    var result = router.Dispatch(stroke, op, brush);
                                    if (result.success && result.spawnedObjects != null)
                                    {
                                        foreach (var go in result.spawnedObjects)
                                        {
                                            if (go != null)
                                            {
                                                go.transform.rotation = Quaternion.Euler(0f, 0f, window.CurrentRotation);
                                            }
                                        }
                                    }
                                }
                                finally
                                {
                                    op.respectsWalkableMask = originalWalkable;
                                }
                            }
                        }
                    }
                }
            }
            else if (window.ToolMode == VisualBrushToolMode.Erase)
            {
                foreach (var op in brush.operations)
                {
                    if (op == null) continue;

                    bool isTileLayer = (op.targetLayer == TargetLayer.L1 || op.targetLayer == TargetLayer.L2);

                    if (isTileLayer)
                    {
                        string errorMsg;
                        Tilemap targetTilemap = AutoLayeringService.FindTargetTilemap(brush, out errorMsg);
                        if (targetTilemap != null)
                        {
                            // S110 Phase 2: if erasing the cliff tilemap, register erased cells
                            // in CliffAutoPlacer's manual-override blacklist so the next
                            // Regenerate() won't repopulate them.
                            RIMA.Environment.CliffAutoPlacer cliffPlacer = null;
                            bool isCliffTilemap = false;
                            if (brush.category == BrushCategory.Cliff ||
                                brush.category == BrushCategory.Transition ||
                                brush.category == BrushCategory.Composite)
                            {
                                cliffPlacer = Object.FindAnyObjectByType<RIMA.Environment.CliffAutoPlacer>();
                                isCliffTilemap = cliffPlacer != null && cliffPlacer.cliffTilemap == targetTilemap;
                            }

                            int radiusInCells = Mathf.Max(1, Mathf.RoundToInt(brushRadiusUnits));
                            for (int x = -radiusInCells; x <= radiusInCells; x++)
                            {
                                for (int y = -radiusInCells; y <= radiusInCells; y++)
                                {
                                    Vector3Int cellPos3D = new Vector3Int(cell.x + x, cell.y + y, 0);
                                    Vector3 cellWorldPos = cellPos3D;
                                    if (activeGrid != null)
                                    {
                                        cellWorldPos = activeGrid.GetCellCenterWorld(cellPos3D);
                                    }

                                    float dist = Vector2.Distance(cellWorldPos, worldPos);
                                    if (dist <= brushRadiusUnits)
                                    {
                                        if (targetTilemap.HasTile(cellPos3D))
                                        {
                                            Undo.RegisterCompleteObjectUndo(targetTilemap, "Erase Tile");
                                            targetTilemap.SetTile(cellPos3D, null);
                                            if (isCliffTilemap)
                                            {
                                                cliffPlacer.AddManualOverride(cellPos3D);
                                                cliffPlacer.RemoveManualPainted(cellPos3D);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // Decorative/Prop Layer L3, L4, L5, L6
                        Transform root = FindLayerRoot(op.targetLayer);
                        if (root != null)
                        {
                            float radiusSq = brushRadiusUnits * brushRadiusUnits;
                            for (int i = root.childCount - 1; i >= 0; i--)
                            {
                                GameObject child = root.GetChild(i).gameObject;
                                Vector2 delta = (Vector2)child.transform.position - worldPos;
                                if (delta.sqrMagnitude <= radiusSq)
                                {
                                    Undo.DestroyObjectImmediate(child);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static Transform FindLayerRoot(TargetLayer layer)
        {
            string rootName = RootNameFor(layer);
            if (string.IsNullOrEmpty(rootName))
            {
                return null;
            }

            GameObject rootObject = GameObject.Find(rootName);
            if (rootObject != null)
            {
                return rootObject.transform;
            }

            Component[] hosts = Object.FindObjectsByType<Component>(FindObjectsSortMode.None);
            for (int i = 0; i < hosts.Length; i++)
            {
                Transform child = hosts[i].transform.Find(rootName);
                if (child != null)
                {
                    return child;
                }
            }

            return null;
        }

        private static string RootNameFor(TargetLayer layer)
        {
            switch (layer)
            {
                case TargetLayer.L3:
                    return "WallOverlay";
                case TargetLayer.L4:
                    return "TransitionBrushLayer";
                case TargetLayer.L5:
                    return "DetailDecalLayer";
                case TargetLayer.L6:
                    return "AccentLayer";
                default:
                    return null;
            }
        }

        // D5: Alt+Click erase — removes a cliff cell and registers it in the
        // CliffAutoPlacer manual-override blacklist so Regenerate() won't repopulate it.
        private static void ApplyCliffErase(Vector2 worldPos, Vector3Int cell, Grid activeGrid)
        {
            RIMA.Environment.CliffAutoPlacer placer =
                Object.FindAnyObjectByType<RIMA.Environment.CliffAutoPlacer>();
            if (placer == null || placer.cliffTilemap == null) return;

            Tilemap cliff = placer.cliffTilemap;
            if (!cliff.HasTile(cell)) return;

            Undo.RegisterCompleteObjectUndo(cliff, "Cliff Erase");
            cliff.SetTile(cell, null);
            placer.AddManualOverride(cell);
            placer.RemoveManualPainted(cell);

            // Increment D5 erase counter visible in statusbar
            CliffEraseCounter.Increment();
            SceneView.RepaintAll();
        }
    }

    /// <summary>D5: Simple static counter for the "Erased: N" statusbar label.</summary>
    internal static class CliffEraseCounter
    {
        public static int Count { get; private set; }
        public static void Increment() { Count++; }
        public static void Reset() { Count = 0; }
    }
}
#endif
