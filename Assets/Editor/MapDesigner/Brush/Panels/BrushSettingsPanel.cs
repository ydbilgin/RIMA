#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using RIMA.MapDesigner.Brush.Data;

namespace RIMA.MapDesigner.Brush.Editor.UI.Panels
{
    public class BrushSettingsPanel
    {
        private readonly MapDesignerBrushWindow window;
        private bool curveFoldout;

        public BrushSettingsPanel(MapDesignerBrushWindow w) { window = w; }

        public void Draw(MapDesignerBrushPresetSO brush, ref float brushSize, ref int seed)
        {
            EditorGUILayout.LabelField("Brush Settings", EditorStyles.boldLabel);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Size:", GUILayout.Width(70));
                brushSize = EditorGUILayout.Slider(brushSize, 8f, 512f);
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Seed:", GUILayout.Width(70));
                seed = EditorGUILayout.IntField(seed);
                if (GUILayout.Button("⟳", GUILayout.Width(24)))
                {
                    seed = (int)(Random.value * int.MaxValue);
                }
            }

            EditorGUILayout.Space(6);

            if (brush == null || brush.operations == null || brush.operations.Count == 0)
            {
                EditorGUILayout.HelpBox("No brush selected.", MessageType.None);
                return;
            }

            var firstOp = brush.operations[0];
            if (firstOp == null) return;

            EditorGUILayout.LabelField("Active Operation [0]", EditorStyles.miniBoldLabel);
            EditorGUILayout.LabelField($"Target: {firstOp.targetLayer}");
            EditorGUILayout.LabelField($"AssetPool: {(firstOp.assetPool != null ? firstOp.assetPool.poolName : "(null)")}");

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Density:", GUILayout.Width(70));
                GUILayout.Label($"{firstOp.density:F2}");
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("MinDist:", GUILayout.Width(70));
                GUILayout.Label($"{firstOp.minDistance:F0}px");
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Scale:", GUILayout.Width(70));
                GUILayout.Label($"{firstOp.scaleRange.x:F2} → {firstOp.scaleRange.y:F2}");
            }

            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField("Rotation / Flip", EditorStyles.miniBoldLabel);
            EditorGUILayout.LabelField($"  Random rotation: {(firstOp.allowRotation ? "ON" : "off")}", EditorStyles.miniLabel);
            EditorGUILayout.LabelField($"  Snap: {firstOp.rotationSnapDegrees:F0}°", EditorStyles.miniLabel);
            EditorGUILayout.LabelField($"  Flip X: {(firstOp.allowFlipX ? "ON" : "off")}    Y: {(firstOp.allowFlipY ? "ON" : "off")}", EditorStyles.miniLabel);

            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField("Karar #143 Filters", EditorStyles.miniBoldLabel);
            EditorGUILayout.LabelField($"  ☑ Walkable filter: {(firstOp.respectsWalkableMask ? "ON" : "off")}", EditorStyles.miniLabel);
            EditorGUILayout.LabelField($"  ☑ Feature mask: {(firstOp.featureMaskMultiplier != null ? "ON (" + firstOp.featureMaskMultiplier.name + ")" : "off")}", EditorStyles.miniLabel);

            curveFoldout = EditorGUILayout.Foldout(curveFoldout, "Wall Proximity Curve", true);
            if (curveFoldout && firstOp.wallProximityCurve != null)
            {
                EditorGUILayout.CurveField(firstOp.wallProximityCurve, GUILayout.Height(60));
            }
        }
    }
}
#endif
