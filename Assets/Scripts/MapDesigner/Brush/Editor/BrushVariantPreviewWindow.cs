#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using RIMA.MapDesigner.Brush.Data;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Editor
{
    public class BrushVariantPreviewWindow : EditorWindow
    {
        private AssetPoolSO selectedPool;
        private Vector2 scroll;

        [MenuItem("RIMA/Brush/Variant Preview")]
        public static void Open()
        {
            GetWindow<BrushVariantPreviewWindow>("Brush Variant Preview");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Brush Variant Preview", EditorStyles.boldLabel);
            selectedPool = (AssetPoolSO)EditorGUILayout.ObjectField("Pool", selectedPool, typeof(AssetPoolSO), false);

            if (selectedPool == null)
            {
                EditorGUILayout.HelpBox("Select an AssetPoolSO to preview its variants.", MessageType.Info);
                return;
            }

            int variantCount = selectedPool.variants != null ? selectedPool.variants.Count : 0;
            EditorGUILayout.LabelField($"Pool: {selectedPool.poolName}", EditorStyles.miniBoldLabel);
            EditorGUILayout.LabelField($"Variants: {variantCount}");
            EditorGUILayout.LabelField($"Default Layer: {selectedPool.defaultTargetLayer}");
            EditorGUILayout.Space();

            scroll = EditorGUILayout.BeginScrollView(scroll);
            foreach (SizeBucket bucket in System.Enum.GetValues(typeof(SizeBucket)))
            {
                var inBucket = (selectedPool.variants ?? new List<BrushAssetVariant>())
                    .Where(v => v != null && v.bucket == bucket).ToList();
                if (inBucket.Count == 0)
                {
                    continue;
                }

                EditorGUILayout.LabelField($"{bucket} ({inBucket.Count})", EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal();
                int col = 0;
                foreach (var v in inBucket)
                {
                    EditorGUILayout.BeginVertical(GUILayout.Width(96));
                    Rect previewRect = GUILayoutUtility.GetRect(80, 80);
                    if (v.sprite != null)
                    {
                        var tex = AssetPreview.GetAssetPreview(v.sprite);
                        if (tex != null)
                        {
                            GUI.DrawTexture(previewRect, tex, ScaleMode.ScaleToFit);
                        }
                    }
                    EditorGUILayout.LabelField(v.variantId ?? "?", EditorStyles.miniLabel);
                    EditorGUILayout.LabelField($"w:{v.weight:F2} hero:{(v.heroAllowed ? "Y" : "N")}", EditorStyles.miniLabel);
                    EditorGUILayout.EndVertical();

                    col++;
                    if (col >= 6)
                    {
                        col = 0;
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
#endif
