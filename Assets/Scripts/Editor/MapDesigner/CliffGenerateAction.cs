#if UNITY_EDITOR
using RIMA.Environment;
using RIMA.RoomPainter;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.MapDesigner
{
    /// <summary>
    /// Logical cliff generation for the Map Designer. Root cause of the old "button does nothing":
    /// an existing CliffRing/CliffAutoPlacer with MISSING refs was never repaired (the action only
    /// auto-created when none existed), and the floor tilemap was auto-picked as "first tilemap".
    /// This version ALWAYS repairs the placer's references before generating, resolves the floor
    /// tilemap by name/context, surfaces the readiness reason, and normalizes the cliff sorting
    /// through <see cref="RoomDepthStack"/> (one source of truth, no scattered -50/"Ground" magic).
    /// </summary>
    internal static class CliffGenerateAction
    {
        private const string RulesAssetPath = "Assets/ScriptableObjects/Environment/CliffPlacementRules_Hades.asset";
        private const string TileAssetPath = "Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset";

        public static void DrawButton(float height = 32f)
        {
            CliffAutoPlacer placer = Object.FindObjectOfType<CliffAutoPlacer>();

            // Always (re)wire references — repairs an existing-but-unready placer instead of
            // silently no-opping (the historical failure mode).
            if (placer == null)
            {
                if (GUILayout.Button(new GUIContent("Create CliffAutoPlacer + Generate",
                        "No CliffAutoPlacer in scene — create one, wire floor/cliff tilemap + tile, and generate."),
                        GUILayout.Height(height)))
                {
                    placer = EnsurePlacer();
                    if (placer != null) Generate(placer);
                }
                return;
            }

            EnsureReferences(placer); // repair on every draw so the button is never dead

            using (new EditorGUILayout.HorizontalScope())
            {
                bool ready = placer.IsReady;
                using (new EditorGUI.DisabledScope(!ready))
                {
                    string tip = ready
                        ? $"Auto-place {placer.CountPreviewPlacements()} cliff tiles from the current floor shape"
                        : "Could not resolve floor/cliff tilemap or cliff tile — see status";
                    if (GUILayout.Button(new GUIContent("Generate Cliffs", tip), GUILayout.Height(height)))
                    {
                        Generate(placer);
                    }
                }

                if (!ready)
                {
                    EditorGUILayout.LabelField(ReadinessReason(placer), EditorStyles.miniLabel, GUILayout.Width(220f));
                }
            }

            if (placer.IsReady)
            {
                EditorGUILayout.LabelField(
                    $"Floor: {SafeName(placer.floorTilemap)}   Cliff: {SafeName(placer.cliffTilemap)}   Preview: {placer.CountPreviewPlacements()}",
                    EditorStyles.miniLabel);
            }
        }

        private static void Generate(CliffAutoPlacer placer)
        {
            if (!placer.IsReady)
            {
                Selection.activeGameObject = placer.gameObject;
                Debug.LogWarning($"[CliffGenerate] Placer not ready: {ReadinessReason(placer)}");
                return;
            }

            placer.Regenerate();
            EditorUtility.SetDirty(placer);
            EditorUtility.SetDirty(placer.gameObject);
            EditorSceneManager.MarkSceneDirty(placer.gameObject.scene);
            Debug.Log($"[CliffGenerate] {placer.LastGeneratedCount} cliff tiles generated.");
        }

        private static string ReadinessReason(CliffAutoPlacer placer)
        {
            if (placer.floorTilemap == null) return "No floor tilemap found";
            if (placer.cliffTilemap == null) return "No cliff tilemap";
            if (placer.cliffTile == null) return "No cliff tile asset";
            return "Ready";
        }

        private static string SafeName(Object o) => o != null ? o.name : "<none>";

        private static CliffAutoPlacer EnsurePlacer()
        {
            GameObject ringGO = GameObject.Find("CliffRing");
            if (ringGO == null)
            {
                ringGO = new GameObject("CliffRing");
                Undo.RegisterCreatedObjectUndo(ringGO, "Create CliffRing");
            }

            CliffAutoPlacer placer = ringGO.GetComponent<CliffAutoPlacer>();
            if (placer == null) placer = Undo.AddComponent<CliffAutoPlacer>(ringGO);

            EnsureReferences(placer);
            EditorUtility.SetDirty(placer);
            EditorSceneManager.MarkSceneDirty(placer.gameObject.scene);
            return placer;
        }

        /// <summary>
        /// Repair every missing reference on an existing OR new placer. Safe to call every frame
        /// (only assigns when a field is null). This is the fix for "existing placer never repaired".
        /// </summary>
        private static void EnsureReferences(CliffAutoPlacer placer)
        {
            if (placer.floorTilemap == null)
            {
                placer.floorTilemap = ResolveFloorTilemap();
            }

            if (placer.cliffTilemap == null && placer.floorTilemap != null)
            {
                placer.cliffTilemap = EnsureCliffTilemap(placer.gameObject, placer.floorTilemap);
            }

            if (placer.cliffTile == null)
            {
                placer.cliffTile = AssetDatabase.LoadAssetAtPath<TileBase>(TileAssetPath);
            }

            if (placer.rules == null)
            {
                placer.rules = AssetDatabase.LoadAssetAtPath<CliffPlacementRules>(RulesAssetPath);
            }
        }

        /// <summary>
        /// Resolve the floor tilemap by NAME/context rather than "first tilemap". Prefers a tilemap
        /// whose name contains "floor"; falls back to the first tilemap that is neither void nor cliff.
        /// </summary>
        private static Tilemap ResolveFloorTilemap()
        {
            Tilemap[] tilemaps = Object.FindObjectsOfType<Tilemap>();

            // 1) explicit "floor" name
            foreach (Tilemap tm in tilemaps)
            {
                if (tm == null) continue;
                if (tm.gameObject.name.ToLowerInvariant().Contains("floor")) return tm;
            }

            // 2) first non-void / non-cliff tilemap that actually has tiles
            foreach (Tilemap tm in tilemaps)
            {
                if (tm == null) continue;
                string n = tm.gameObject.name.ToLowerInvariant();
                if (n.Contains("void") || n.Contains("cliff")) continue;
                tm.CompressBounds();
                if (tm.GetUsedTilesCount() > 0) return tm;
            }

            // 3) any non-void / non-cliff tilemap
            foreach (Tilemap tm in tilemaps)
            {
                if (tm == null) continue;
                string n = tm.gameObject.name.ToLowerInvariant();
                if (n.Contains("void") || n.Contains("cliff")) continue;
                return tm;
            }

            return null;
        }

        private static Tilemap EnsureCliffTilemap(GameObject ringGO, Tilemap floorReference)
        {
            Transform existing = ringGO.transform.Find("CliffTilemap");
            GameObject tmGO;
            if (existing != null)
            {
                tmGO = existing.gameObject;
            }
            else
            {
                tmGO = new GameObject("CliffTilemap");
                Undo.RegisterCreatedObjectUndo(tmGO, "Create CliffTilemap");
                tmGO.transform.SetParent(ringGO.transform, false);
            }

            Tilemap tm = tmGO.GetComponent<Tilemap>();
            if (tm == null) tm = Undo.AddComponent<Tilemap>(tmGO);

            TilemapRenderer tr = tmGO.GetComponent<TilemapRenderer>();
            if (tr == null) tr = Undo.AddComponent<TilemapRenderer>(tmGO);

            // One source of truth for the cliff depth slot (L2 under floor).
            RoomDepthStack.DepthSlot slot = RoomDepthStack.SlotFor(RoomLayer.Cliff);
            tr.sortingLayerName = slot.sortingLayer;
            tr.sortingOrder = slot.sortingOrder;

            if (floorReference != null && floorReference.layoutGrid != null)
            {
                Transform grid = floorReference.layoutGrid.transform;
                if (ringGO.transform.parent != grid)
                {
                    Undo.SetTransformParent(ringGO.transform, grid, "Parent CliffRing to Grid");
                    ringGO.transform.localPosition = Vector3.zero;
                    ringGO.transform.localRotation = Quaternion.identity;
                    ringGO.transform.localScale = Vector3.one;
                }
            }

            return tm;
        }
    }
}
#endif
