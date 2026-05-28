using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    internal static class RoomPainterInstanceList
    {
        private const int MaxVisibleRows = 6;
        private static readonly Color HeaderTint = new Color(0.22f, 0.28f, 0.32f, 1f);

        public static void Draw(RoomPainterAsset asset)
        {
            if (asset == null)
            {
                return;
            }

            string guid = ResolveSourceGuid(asset);
            if (string.IsNullOrEmpty(guid))
            {
                return;
            }

            List<RoomPainterAssetBinding> matches = FindBindings(guid);

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                Rect headerRect = GUILayoutUtility.GetRect(0f, 20f, GUILayout.ExpandWidth(true));
                EditorGUI.DrawRect(headerRect, HeaderTint);

                Rect labelRect = new Rect(headerRect.x + 6f, headerRect.y + 2f, headerRect.width - 12f, headerRect.height - 2f);
                GUI.Label(labelRect, "Scene Instances: " + matches.Count, EditorStyles.boldLabel);

                if (matches.Count == 0)
                {
                    EditorGUILayout.LabelField("No painted instances of this asset.", EditorStyles.miniLabel);
                    return;
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Select All", EditorStyles.miniButtonLeft))
                    {
                        Object[] gos = new Object[matches.Count];
                        for (int i = 0; i < matches.Count; i++)
                        {
                            gos[i] = matches[i].gameObject;
                        }

                        Selection.objects = gos;
                    }

                    if (GUILayout.Button("Apply Physics All", EditorStyles.miniButtonMid))
                    {
                        for (int i = 0; i < matches.Count; i++)
                        {
                            RoomPainterPhysicsApplier.Apply(matches[i].gameObject, asset);
                        }
                    }

                    if (GUILayout.Button("Frame in Scene", EditorStyles.miniButtonRight))
                    {
                        if (matches.Count > 0)
                        {
                            Selection.activeGameObject = matches[0].gameObject;
                            SceneView.FrameLastActiveSceneView();
                        }
                    }
                }

                int visibleRows = Mathf.Min(matches.Count, MaxVisibleRows);
                for (int i = 0; i < visibleRows; i++)
                {
                    GameObject go = matches[i].gameObject;
                    if (go == null)
                    {
                        continue;
                    }

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.ObjectField(go, typeof(GameObject), true);
                        if (GUILayout.Button("Ping", EditorStyles.miniButton, GUILayout.Width(44f)))
                        {
                            Selection.activeGameObject = go;
                            EditorGUIUtility.PingObject(go);
                        }
                    }
                }

                if (matches.Count > visibleRows)
                {
                    GUILayout.Label("+ " + (matches.Count - visibleRows) + " more instances", EditorStyles.miniLabel);
                }
            }

            GUILayout.Space(4f);
        }

        private static string ResolveSourceGuid(RoomPainterAsset asset)
        {
            Object source = asset.sprite != null ? (Object)asset.sprite : asset.prefab;
            string path = source != null ? AssetDatabase.GetAssetPath(source) : string.Empty;
            return string.IsNullOrEmpty(path) ? asset.id : AssetDatabase.AssetPathToGUID(path);
        }

        private static List<RoomPainterAssetBinding> FindBindings(string guid)
        {
            RoomPainterAssetBinding[] all = Object.FindObjectsByType<RoomPainterAssetBinding>(FindObjectsSortMode.None);
            List<RoomPainterAssetBinding> matches = new List<RoomPainterAssetBinding>();
            for (int i = 0; i < all.Length; i++)
            {
                if (all[i] != null && all[i].assetGuid == guid)
                {
                    matches.Add(all[i]);
                }
            }

            return matches;
        }
    }
}
