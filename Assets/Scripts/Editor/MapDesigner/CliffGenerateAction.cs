#if UNITY_EDITOR
using RIMA.Environment;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.MapDesigner
{
    internal static class CliffGenerateAction
    {
        private const string RulesAssetPath = "Assets/ScriptableObjects/Environment/CliffPlacementRules_Hades.asset";
        private const string TileAssetPath = "Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset";

        public static void DrawButton(float height = 32f)
        {
            CliffAutoPlacer placer = Object.FindObjectOfType<CliffAutoPlacer>();

            if (placer == null)
            {
                GUIContent createContent = new GUIContent(
                    "🪨 Create CliffAutoPlacer + Generate",
                    "Sahnede CliffAutoPlacer yok — tikla, otomatik olustur ve cliff'leri uret.");
                if (GUILayout.Button(createContent, GUILayout.Height(height)))
                {
                    CliffAutoPlacer created = AutoCreatePlacer();
                    if (created == null)
                    {
                        Debug.LogWarning("[CliffGenerate] Could not create CliffAutoPlacer.");
                        return;
                    }

                    EditorSceneManager.MarkSceneDirty(created.gameObject.scene);

                    if (created.IsReady)
                    {
                        created.Regenerate();
                        EditorUtility.SetDirty(created.gameObject);
                        Debug.Log($"[CliffGenerate] Created + generated {created.LastGeneratedCount} cliff tiles.");
                    }
                    else
                    {
                        Selection.activeGameObject = created.gameObject;
                        Debug.LogWarning("[CliffGenerate] Placer created but not ready — assign floorTilemap + cliffTilemap + cliffTile on CliffRing then click again.");
                    }
                }
                return;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                bool enabled = placer.IsReady;

                using (new EditorGUI.DisabledScope(!enabled))
                {
                    GUIContent content = new GUIContent(
                        "Generate Cliffs",
                        enabled
                            ? $"Auto-place {placer.CountPreviewPlacements()} cliff tiles based on current floor shape"
                            : "Assign floorTilemap + cliffTilemap + cliffTile on CliffRing to enable");
                    if (GUILayout.Button(content, GUILayout.Height(height)))
                    {
                        placer.Regenerate();
                        EditorUtility.SetDirty(placer.gameObject);
                        EditorSceneManager.MarkSceneDirty(placer.gameObject.scene);
                        Debug.Log($"[CliffGenerate] {placer.LastGeneratedCount} cliff tiles generated.");
                    }
                }

                if (!enabled)
                {
                    EditorGUILayout.LabelField("Placer not ready", EditorStyles.miniLabel, GUILayout.Width(180f));
                }
            }
        }

        private static CliffAutoPlacer AutoCreatePlacer()
        {
            GameObject ringGO = GameObject.Find("CliffRing");
            if (ringGO == null)
            {
                ringGO = new GameObject("CliffRing");
                Undo.RegisterCreatedObjectUndo(ringGO, "Create CliffRing");
            }

            CliffAutoPlacer placer = ringGO.GetComponent<CliffAutoPlacer>();
            if (placer == null)
            {
                placer = Undo.AddComponent<CliffAutoPlacer>(ringGO);
            }

            if (placer.floorTilemap == null)
            {
                Tilemap[] tilemaps = Object.FindObjectsOfType<Tilemap>();
                foreach (Tilemap tm in tilemaps)
                {
                    if (tm == null) continue;
                    string n = tm.gameObject.name.ToLowerInvariant();
                    if (n.Contains("void")) continue;
                    if (n.Contains("cliff")) continue;
                    placer.floorTilemap = tm;
                    break;
                }
            }

            if (placer.cliffTilemap == null)
            {
                placer.cliffTilemap = EnsureCliffTilemap(ringGO, placer.floorTilemap);
            }

            if (placer.cliffTile == null)
            {
                placer.cliffTile = AssetDatabase.LoadAssetAtPath<TileBase>(TileAssetPath);
                if (placer.cliffTile == null)
                {
                    Debug.LogWarning($"[CliffGenerate] CliffTile asset not found at {TileAssetPath}.");
                    Selection.activeGameObject = ringGO;
                }
            }

            if (placer.rules == null)
            {
                placer.rules = AssetDatabase.LoadAssetAtPath<CliffPlacementRules>(RulesAssetPath);
            }

            EditorUtility.SetDirty(placer);
            return placer;
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
            tr.sortingLayerName = "Ground";
            tr.sortingOrder = -50;

            // Parent CliffRing to floor's Grid so cellSize/layout match
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
