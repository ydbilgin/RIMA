using System.Collections.Generic;
using System.Linq;
using RIMA;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor
{
    public class TilesetPaletteDrawer
    {
        private List<CornerWangTileSetSO> palette = new List<CornerWangTileSetSO>();

        public void Refresh()
        {
            palette = AssetDatabase.FindAssets("t:CornerWangTileSetSO")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(path => AssetDatabase.LoadAssetAtPath<CornerWangTileSetSO>(path))
                .Where(so => so != null)
                .OrderBy(so => so.name)
                .ToList();
        }

        public CornerWangTileSetSO Draw(float width, CornerWangTileSetSO currentSelection)
        {
            if (palette == null)
            {
                Refresh();
            }

            CornerWangTileSetSO clicked = null;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Tilesets", EditorStyles.boldLabel);
            if (GUILayout.Button("Refresh", EditorStyles.miniButton, GUILayout.Width(58f)))
            {
                Refresh();
            }
            EditorGUILayout.EndHorizontal();

            foreach (CornerWangTileSetSO so in palette)
            {
                TileBase coverTile = so.tiles != null && so.tiles.Length > 15 ? so.tiles[15] : null;
                Texture preview = coverTile != null ? AssetPreview.GetAssetPreview(coverTile) : null;
                bool isSelected = so == currentSelection;
                Color bg = isSelected ? new Color(0.3f, 0.6f, 1f, 0.3f) : Color.clear;
                Rect r = GUILayoutUtility.GetRect(width, 80f);
                EditorGUI.DrawRect(r, bg);

                if (preview != null)
                {
                    GUI.DrawTexture(new Rect(r.x + 8f, r.y + 8f, 56f, 56f), preview, ScaleMode.ScaleToFit);
                }

                EditorGUI.LabelField(new Rect(r.x + 72f, r.y + 30f, width - 80f, 18f), so.name);
                if (GUI.Button(r, GUIContent.none, GUIStyle.none))
                {
                    clicked = so;
                }
            }

            return clicked;
        }
    }
}
