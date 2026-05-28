using UnityEditor;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    public static class PlacementSection
    {
        private static readonly OrderPreset[] OrderPresets =
        {
            new OrderPreset("Background", -100),
            new OrderPreset("Floor", 0),
            new OrderPreset("Mid", 10),
            new OrderPreset("Above", 50),
            new OrderPreset("Front", 100)
        };

        private static string[] _sortingLayerNames;

        static PlacementSection()
        {
            EditorApplication.projectChanged += ClearSortingLayerCache;
            EditorApplication.hierarchyChanged += ClearSortingLayerCache;
        }

        public static void Draw(RoomPainterAsset asset)
        {
            asset.defaultLayer = (RoomLayer)EditorGUILayout.EnumPopup("Default Layer", asset.defaultLayer);
            asset.defaultSortingLayer = SortingLayerPopup("Sorting Layer", asset.defaultSortingLayer);

            DrawOrderControl(asset);

            asset.ySortEnabled = EditorGUILayout.Toggle("Y Sort", asset.ySortEnabled);
            using (new EditorGUI.DisabledScope(!asset.ySortEnabled))
            {
                asset.ySortAxisOverride = (YSortAxis)EditorGUILayout.EnumPopup("Y Sort Axis", asset.ySortAxisOverride);
                asset.pivotAnchor = EditorGUILayout.Vector2Field("Pivot Anchor", asset.pivotAnchor);
            }

            asset.defaultScale = EditorGUILayout.Vector2Field("Scale", asset.defaultScale);
            asset.defaultVisualOffset = EditorGUILayout.Vector2Field("Visual Offset", asset.defaultVisualOffset);
        }

        private static void DrawOrderControl(RoomPainterAsset asset)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                asset.defaultOrder = EditorGUILayout.IntField("Order", asset.defaultOrder);

                if (GUILayout.Button("◀", EditorStyles.miniButtonLeft, GUILayout.Width(22f)))
                {
                    asset.defaultOrder = Mathf.Max(-999, asset.defaultOrder - 1);
                }

                if (GUILayout.Button("▶", EditorStyles.miniButtonRight, GUILayout.Width(22f)))
                {
                    asset.defaultOrder = Mathf.Min(999, asset.defaultOrder + 1);
                }
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                for (int i = 0; i < OrderPresets.Length; i++)
                {
                    bool active = asset.defaultOrder == OrderPresets[i].Order;
                    Color previous = GUI.backgroundColor;
                    if (active)
                    {
                        GUI.backgroundColor = new Color(0.40f, 0.85f, 0.50f, 1f);
                    }

                    GUIStyle style = StyleForIndex(i);
                    if (GUILayout.Button(OrderPresets[i].Label, style, GUILayout.MinWidth(58f)))
                    {
                        asset.defaultOrder = OrderPresets[i].Order;
                    }

                    GUI.backgroundColor = previous;
                }
            }
        }

        private static GUIStyle StyleForIndex(int index)
        {
            if (OrderPresets.Length == 1)
            {
                return EditorStyles.miniButton;
            }

            if (index == 0)
            {
                return EditorStyles.miniButtonLeft;
            }

            if (index == OrderPresets.Length - 1)
            {
                return EditorStyles.miniButtonRight;
            }

            return EditorStyles.miniButtonMid;
        }

        private static string SortingLayerPopup(string label, string current)
        {
            string[] names = GetSortingLayerNames();
            if (names == null || names.Length == 0)
            {
                return EditorGUILayout.TextField(label, current);
            }

            int index = 0;
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i] == current)
                {
                    index = i;
                    break;
                }
            }

            index = EditorGUILayout.Popup(label, index, names);
            return names[Mathf.Clamp(index, 0, names.Length - 1)];
        }

        private static string[] GetSortingLayerNames()
        {
            if (_sortingLayerNames != null)
            {
                return _sortingLayerNames;
            }

            SortingLayer[] layers = SortingLayer.layers;
            if (layers == null || layers.Length == 0)
            {
                _sortingLayerNames = new string[0];
                return _sortingLayerNames;
            }

            _sortingLayerNames = new string[layers.Length];
            for (int i = 0; i < layers.Length; i++)
            {
                _sortingLayerNames[i] = layers[i].name;
            }

            return _sortingLayerNames;
        }

        private static void ClearSortingLayerCache()
        {
            _sortingLayerNames = null;
        }

        private readonly struct OrderPreset
        {
            public readonly string Label;
            public readonly int Order;

            public OrderPreset(string label, int order)
            {
                Label = label;
                Order = order;
            }
        }
    }
}
