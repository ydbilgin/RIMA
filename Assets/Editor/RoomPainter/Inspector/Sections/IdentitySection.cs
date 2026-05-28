using UnityEditor;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    public static class IdentitySection
    {
        private static readonly Color DropZoneIdle = new Color(0.18f, 0.18f, 0.22f, 1f);
        private static readonly Color DropZoneHover = new Color(0.18f, 0.45f, 0.55f, 1f);

        public static void Draw(RoomPainterAsset asset)
        {
            Object source = SourceObject(asset);
            string sourcePath = source != null ? AssetDatabase.GetAssetPath(source) : AssetDatabase.GetAssetPath(asset);
            string guid = !string.IsNullOrEmpty(sourcePath) ? AssetDatabase.AssetPathToGUID(sourcePath) : asset.id;

            asset.displayName = EditorGUILayout.TextField("Display Name", asset.displayName);

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.TextField("Path", sourcePath);
                EditorGUILayout.TextField("GUID", guid);
            }

            EditorGUILayout.Space(2f);
            DrawSourceField(asset);
            DrawDropZone(asset);

            EditorGUILayout.Space(2f);
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                using (new EditorGUI.DisabledScope(source == null))
                {
                    if (GUILayout.Button("Ping Source", EditorStyles.miniButtonLeft, GUILayout.Width(96f)))
                    {
                        EditorGUIUtility.PingObject(source);
                    }
                }

                if (GUILayout.Button("Select in Project", EditorStyles.miniButtonRight, GUILayout.Width(124f)))
                {
                    if (source != null)
                    {
                        Selection.activeObject = source;
                        EditorGUIUtility.PingObject(source);
                    }
                    else if (asset != null)
                    {
                        Selection.activeObject = asset;
                        EditorGUIUtility.PingObject(asset);
                    }
                }
            }
        }

        private static void DrawSourceField(RoomPainterAsset asset)
        {
            Object current = asset.sprite != null ? (Object)asset.sprite : asset.prefab;
            EditorGUI.BeginChangeCheck();
            Object next = EditorGUILayout.ObjectField("Source", current, typeof(Object), false);
            if (EditorGUI.EndChangeCheck())
            {
                AssignSource(asset, next);
            }
        }

        private static void DrawDropZone(RoomPainterAsset asset)
        {
            Rect zone = GUILayoutUtility.GetRect(0f, 32f, GUILayout.ExpandWidth(true));
            Event evt = Event.current;
            bool hover = zone.Contains(evt.mousePosition);
            bool isDragging = DragAndDrop.objectReferences != null && DragAndDrop.objectReferences.Length > 0;

            EditorGUI.DrawRect(zone, hover && isDragging ? DropZoneHover : DropZoneIdle);

            GUIStyle hint = new GUIStyle(EditorStyles.miniLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = new Color(0.75f, 0.78f, 0.80f, 1f) }
            };
            GUI.Label(zone, "Drag a sprite or prefab here to swap source", hint);

            if (!hover)
            {
                return;
            }

            if (evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    Object[] refs = DragAndDrop.objectReferences;
                    if (refs != null && refs.Length > 0)
                    {
                        AssignSource(asset, refs[0]);
                    }

                    evt.Use();
                }
            }
        }

        private static void AssignSource(RoomPainterAsset asset, Object next)
        {
            if (next == null)
            {
                asset.sprite = null;
                asset.prefab = null;
                return;
            }

            Sprite asSprite = next as Sprite;
            if (asSprite == null && next is Texture2D texture)
            {
                string path = AssetDatabase.GetAssetPath(texture);
                if (!string.IsNullOrEmpty(path))
                {
                    asSprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                }
            }

            GameObject asPrefab = next as GameObject;

            if (asSprite != null)
            {
                asset.sprite = asSprite;
                asset.prefab = null;
            }
            else if (asPrefab != null)
            {
                asset.prefab = asPrefab;
                asset.sprite = null;
            }
            else
            {
                Debug.LogWarning("Room Painter: dropped asset is neither sprite nor prefab.");
                return;
            }

            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssetIfDirty(asset);
        }

        private static Object SourceObject(RoomPainterAsset asset)
        {
            if (asset.sprite != null)
            {
                return asset.sprite;
            }

            return asset.prefab;
        }
    }
}
