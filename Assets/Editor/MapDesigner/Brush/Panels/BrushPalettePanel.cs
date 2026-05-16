#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RIMA.MapDesigner.Brush.Data;

namespace RIMA.MapDesigner.Brush.Editor.UI.Panels
{
    public class BrushPalettePanel
    {
        private const string ThumbSizeKey = "RIMA.Brush.ThumbSize";
        private readonly MapDesignerBrushWindow window;
        private Vector2 scroll;
        private float thumbSize;

        public BrushPalettePanel(MapDesignerBrushWindow w)
        {
            window = w;
            thumbSize = SessionState.GetFloat(ThumbSizeKey, 56f);
        }

        public void Draw(ref MapDesignerBrushPresetSO selected, BrushPackSO pack, ref string searchFilter, ref BrushCategory category, ref bool categoryAll)
        {
            EditorGUILayout.LabelField("Brush Palette", EditorStyles.boldLabel);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("🔍", GUILayout.Width(18));
                searchFilter = EditorGUILayout.TextField(searchFilter ?? string.Empty);
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Category:", GUILayout.Width(70));
                categoryAll = EditorGUILayout.ToggleLeft("All", categoryAll, GUILayout.Width(50));
                using (new EditorGUI.DisabledScope(categoryAll))
                {
                    category = (BrushCategory)EditorGUILayout.EnumPopup(category);
                }
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Thumb size:", GUILayout.Width(80));
                float newSize = GUILayout.HorizontalSlider(thumbSize, 32f, 96f);
                if (!Mathf.Approximately(newSize, thumbSize))
                {
                    thumbSize = newSize;
                    SessionState.SetFloat(ThumbSizeKey, thumbSize);
                }
            }

            EditorGUILayout.Space(4);
            DrawBrushGrid(ref selected, pack, searchFilter, category, categoryAll);

            EditorGUILayout.Space(8);
            DrawSelectedInfo(selected);
        }

        private void DrawBrushGrid(ref MapDesignerBrushPresetSO selected, BrushPackSO pack, string filter, BrushCategory category, bool categoryAll)
        {
            if (pack == null || pack.brushes == null || pack.brushes.Count == 0)
            {
                EditorGUILayout.HelpBox("No brush pack loaded. Drag a BrushPackSO into the top bar.", MessageType.Info);
                return;
            }

            var filtered = new List<MapDesignerBrushPresetSO>();
            string filterLower = string.IsNullOrEmpty(filter) ? null : filter.ToLowerInvariant();
            foreach (var b in pack.brushes)
            {
                if (b == null || !b.showInPalette) continue;
                if (!categoryAll && b.category != category) continue;
                if (filterLower != null && (string.IsNullOrEmpty(b.brushName) || !b.brushName.ToLowerInvariant().Contains(filterLower))) continue;
                filtered.Add(b);
            }

            scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(280));
            int cols = Mathf.Max(1, (int)((MapDesignerBrushWindow.LeftPanelWidth - 24f) / (thumbSize + 8f)));
            int idx = 0;
            EditorGUILayout.BeginHorizontal();
            foreach (var b in filtered)
            {
                if (idx > 0 && idx % cols == 0)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                }
                DrawBrushTile(b, ref selected);
                idx++;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }

        private void DrawBrushTile(MapDesignerBrushPresetSO b, ref MapDesignerBrushPresetSO selected)
        {
            var content = new GUIContent(b.previewIcon != null ? b.previewIcon.texture : null, b.brushName);
            bool isSelected = selected == b;
            var style = isSelected ? "SelectionRect" : "Button";
            if (GUILayout.Button(content, GUI.skin.GetStyle(style), GUILayout.Width(thumbSize), GUILayout.Height(thumbSize)))
            {
                window.SetBrush(b);
            }
        }

        private void DrawSelectedInfo(MapDesignerBrushPresetSO selected)
        {
            EditorGUILayout.LabelField("Selected:", EditorStyles.boldLabel);
            if (selected == null)
            {
                EditorGUILayout.LabelField("(none)", EditorStyles.miniLabel);
                return;
            }
            EditorGUILayout.LabelField(selected.brushName);
            EditorGUILayout.LabelField($"Category: {selected.category}", EditorStyles.miniLabel);
            EditorGUILayout.LabelField($"PaintMode: {selected.paintMode}", EditorStyles.miniLabel);
            if (selected.hotkeyIndex > 0)
            {
                EditorGUILayout.LabelField($"Hotkey: Alt+{selected.hotkeyIndex}", EditorStyles.miniLabel);
            }
            if (selected.category == BrushCategory.Composite && selected.operations != null && selected.operations.Count > 1)
            {
                EditorGUILayout.LabelField("Composite ops:", EditorStyles.miniBoldLabel);
                foreach (var op in selected.operations)
                {
                    if (op == null) continue;
                    string poolName = op.assetPool != null ? op.assetPool.poolName : "(no pool)";
                    EditorGUILayout.LabelField($"  • {op.targetLayer}: {poolName} ×{op.density:F2}", EditorStyles.miniLabel);
                }
            }
        }
    }
}
#endif
