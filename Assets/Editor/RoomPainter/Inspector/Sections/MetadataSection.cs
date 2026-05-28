using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    public static class MetadataSection
    {
        private static readonly string[] QuickTags =
        {
            "walkable",
            "cover",
            "decor",
            "hazard",
            "interactive",
            "destructible"
        };

        public static void Draw(RoomPainterAsset asset)
        {
            if (asset.tags == null)
            {
                asset.tags = new List<string>();
            }

            EditorGUILayout.LabelField("Quick Tags", EditorStyles.miniBoldLabel);
            DrawChipRow(asset, 0, 3);
            DrawChipRow(asset, 3, QuickTags.Length);

            EditorGUILayout.Space(4f);
            EditorGUILayout.LabelField("Custom Tags", EditorStyles.miniBoldLabel);

            for (int i = 0; i < asset.tags.Count; i++)
            {
                bool isQuick = false;
                for (int q = 0; q < QuickTags.Length; q++)
                {
                    if (asset.tags[i] == QuickTags[q])
                    {
                        isQuick = true;
                        break;
                    }
                }

                if (isQuick)
                {
                    continue;
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    asset.tags[i] = EditorGUILayout.TextField(asset.tags[i]);
                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(24f)))
                    {
                        asset.tags.RemoveAt(i);
                        i--;
                    }
                }
            }

            if (GUILayout.Button("+ Add Custom Tag", EditorStyles.miniButton))
            {
                asset.tags.Add(string.Empty);
            }

            EditorGUILayout.Space(6f);
            EditorGUILayout.LabelField("Notes", EditorStyles.miniBoldLabel);
            asset.notes = EditorGUILayout.TextArea(asset.notes ?? string.Empty, GUILayout.MinHeight(58f));
        }

        private static void DrawChipRow(RoomPainterAsset asset, int from, int toExclusive)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                for (int i = from; i < toExclusive; i++)
                {
                    DrawTagChip(asset, QuickTags[i]);
                }

                GUILayout.FlexibleSpace();
            }
        }

        private static void DrawTagChip(RoomPainterAsset asset, string tag)
        {
            bool hasTag = asset.tags.Contains(tag);
            Color previous = GUI.backgroundColor;
            if (hasTag)
            {
                GUI.backgroundColor = ColorForTag(tag);
            }

            bool next = GUILayout.Toggle(hasTag, tag, EditorStyles.miniButton, GUILayout.MinWidth(70f), GUILayout.Height(20f));
            GUI.backgroundColor = previous;

            if (next == hasTag)
            {
                return;
            }

            if (next)
            {
                asset.tags.Add(tag);
            }
            else
            {
                asset.tags.Remove(tag);
            }
        }

        private static Color ColorForTag(string tag)
        {
            switch (tag)
            {
                case "walkable": return new Color(0.35f, 0.85f, 0.45f, 1f);
                case "cover": return new Color(0.40f, 0.60f, 0.95f, 1f);
                case "decor": return new Color(0.85f, 0.65f, 0.45f, 1f);
                case "hazard": return new Color(0.95f, 0.40f, 0.40f, 1f);
                case "interactive": return new Color(0.90f, 0.85f, 0.30f, 1f);
                case "destructible": return new Color(0.80f, 0.50f, 0.80f, 1f);
                default: return new Color(0.6f, 0.6f, 0.6f, 1f);
            }
        }
    }
}
