#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using RIMA.MapDesigner.Brush.Data;

namespace RIMA.MapDesigner.Brush.Editor.UI.Panels
{
    public class LayerVisibilityPanel
    {
        private const string VisibleKeyPrefix = "RIMA.Brush.LayerVisible.";
        private const string SoloKey = "RIMA.Brush.LayerSolo";

        private static readonly TargetLayer[] LayersOrder = new[]
        {
            TargetLayer.L1, TargetLayer.L2, TargetLayer.L3,
            TargetLayer.L4, TargetLayer.L5, TargetLayer.L6
        };

        private static readonly string[] LayerLabels = new[]
        {
            "L1 Floor", "L2 Variation", "L3 Wall", "L4 Transition", "L5 Detail", "L6 Accent"
        };

        public void Draw()
        {
            EditorGUILayout.LabelField("Layer Visibility", EditorStyles.boldLabel);
            int soloIdx = SessionState.GetInt(SoloKey, -1);

            for (int i = 0; i < LayersOrder.Length; i++)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label(LayerLabels[i], GUILayout.Width(110));

                    bool visible = SessionState.GetBool(VisibleKeyPrefix + LayersOrder[i], true);
                    bool newVisible = GUILayout.Toggle(visible, "👁", "Button", GUILayout.Width(28));
                    if (newVisible != visible)
                    {
                        SessionState.SetBool(VisibleKeyPrefix + LayersOrder[i], newVisible);
                        ApplyVisibility(LayersOrder[i], newVisible || soloIdx == -1 || soloIdx == i);
                    }

                    bool isSolo = soloIdx == i;
                    bool newSolo = GUILayout.Toggle(isSolo, "S", "Button", GUILayout.Width(24));
                    if (newSolo != isSolo)
                    {
                        soloIdx = newSolo ? i : -1;
                        SessionState.SetInt(SoloKey, soloIdx);
                        ApplyAllVisibility(soloIdx);
                    }
                }
            }
            EditorGUILayout.LabelField(soloIdx >= 0 ? $"Solo: {LayerLabels[soloIdx]}" : "Solo: none", EditorStyles.miniLabel);
        }

        private void ApplyAllVisibility(int soloIdx)
        {
            for (int i = 0; i < LayersOrder.Length; i++)
            {
                bool baseVisible = SessionState.GetBool(VisibleKeyPrefix + LayersOrder[i], true);
                bool effective = soloIdx == -1 ? baseVisible : (soloIdx == i);
                ApplyVisibility(LayersOrder[i], effective);
            }
        }

        private void ApplyVisibility(TargetLayer layer, bool visible)
        {
            var container = GameObject.Find($"Layer_{layer}");
            if (container == null) return;
            container.SetActive(visible);
        }
    }
}
#endif
