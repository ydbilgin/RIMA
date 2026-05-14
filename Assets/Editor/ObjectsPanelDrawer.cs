using System;
using System.Collections.Generic;
using System.IO;
using RIMA.Systems.Map;
using UnityEditor;
using UnityEngine;

namespace RIMA.Editor
{
    [Serializable]
    public class ObjectsPanelDrawer
    {
        private static readonly string[] FolderLabels = { "Mobs", "Props", "SpawnPoints" };
        private static readonly string[] FolderPaths =
        {
            "Assets/Prefabs/Mobs",
            "Assets/Prefabs/Props",
            "Assets/Prefabs/SpawnPoints"
        };

        public bool isOpen = false;
        public string activePrefabFolder = "Assets/Prefabs/Mobs";
        public GameObject selectedPrefab;
        public bool placeMode = false;
        public MapObjectPlacement selectedPlacement;

        [NonSerialized] private Vector2 prefabScroll;
        [NonSerialized] private Vector2 placedScroll;

        public void Draw(
            Rect panelRect,
            List<MapObjectPlacement> objects,
            Action<MapObjectPlacement> onAdd,
            Action<MapObjectPlacement> onRemove)
        {
            if (!isOpen)
            {
                return;
            }

            GUILayout.BeginArea(panelRect, GUIContent.none, EditorStyles.helpBox);
            GUILayout.Label("Objects", EditorStyles.boldLabel);

            int folderIndex = Mathf.Max(0, Array.IndexOf(FolderPaths, activePrefabFolder));
            EditorGUI.BeginChangeCheck();
            folderIndex = GUILayout.Toolbar(folderIndex, FolderLabels);
            if (EditorGUI.EndChangeCheck())
            {
                activePrefabFolder = FolderPaths[Mathf.Clamp(folderIndex, 0, FolderPaths.Length - 1)];
                selectedPrefab = null;
                placeMode = false;
            }

            EditorGUILayout.Space(4f);
            GUILayout.Label(activePrefabFolder, EditorStyles.miniLabel);
            DrawPrefabList();

            EditorGUILayout.Space(4f);
            EditorGUI.BeginDisabledGroup(selectedPrefab == null);
            placeMode = EditorGUILayout.ToggleLeft("Place Mode", placeMode);
            if (GUILayout.Button("Add At Origin"))
            {
                onAdd?.Invoke(CreatePlacement(Vector2.zero));
            }
            EditorGUI.EndDisabledGroup();

            if (selectedPrefab != null)
            {
                EditorGUILayout.LabelField("Selected", selectedPrefab.name, EditorStyles.miniLabel);
            }

            EditorGUILayout.Space(6f);
            DrawSeparator();
            GUILayout.Label("Placed", EditorStyles.boldLabel);
            placedScroll = EditorGUILayout.BeginScrollView(placedScroll);
            if (objects == null || objects.Count == 0)
            {
                EditorGUILayout.HelpBox("No placed objects.", MessageType.Info);
            }
            else
            {
                for (int i = 0; i < objects.Count; i++)
                {
                    MapObjectPlacement placement = objects[i];
                    if (placement == null)
                    {
                        continue;
                    }

                    DrawPlacementRow(placement, onRemove);
                }
            }

            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        public MapObjectPlacement CreatePlacement(Vector2 positionPx)
        {
            if (selectedPrefab == null)
            {
                return null;
            }

            string path = AssetDatabase.GetAssetPath(selectedPrefab);
            return new MapObjectPlacement
            {
                prefabPath = path,
                positionPx = positionPx,
                displayName = selectedPrefab.name,
                visible = true
            };
        }

        private void DrawPrefabList()
        {
            prefabScroll = EditorGUILayout.BeginScrollView(prefabScroll, GUILayout.Height(116f));
            if (!AssetDatabase.IsValidFolder(activePrefabFolder))
            {
                EditorGUILayout.HelpBox("Folder missing.", MessageType.Warning);
                EditorGUILayout.EndScrollView();
                return;
            }

            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { activePrefabFolder });
            if (guids.Length == 0)
            {
                EditorGUILayout.HelpBox("No prefabs found.", MessageType.Info);
            }

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null)
                {
                    continue;
                }

                Rect row = GUILayoutUtility.GetRect(1f, 34f, GUILayout.ExpandWidth(true));
                bool active = selectedPrefab == prefab;
                if (active)
                {
                    EditorGUI.DrawRect(row, new Color(0.18f, 0.42f, 0.7f, 0.45f));
                }

                Texture preview = AssetPreview.GetAssetPreview(prefab);
                if (preview == null)
                {
                    preview = AssetPreview.GetMiniThumbnail(prefab);
                }

                if (preview != null)
                {
                    GUI.DrawTexture(new Rect(row.x + 3f, row.y + 3f, 28f, 28f), preview, ScaleMode.ScaleToFit);
                }

                GUI.Label(new Rect(row.x + 36f, row.y + 4f, row.width - 40f, 16f), prefab.name, EditorStyles.miniBoldLabel);
                GUI.Label(new Rect(row.x + 36f, row.y + 18f, row.width - 40f, 14f), Path.GetFileName(path), EditorStyles.miniLabel);

                if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && row.Contains(Event.current.mousePosition))
                {
                    selectedPrefab = prefab;
                    Event.current.Use();
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawPlacementRow(MapObjectPlacement placement, Action<MapObjectPlacement> onRemove)
        {
            GUIStyle rowStyle = selectedPlacement == placement ? EditorStyles.helpBox : GUI.skin.box;
            EditorGUILayout.BeginVertical(rowStyle);
            EditorGUILayout.BeginHorizontal();
            placement.visible = EditorGUILayout.Toggle(placement.visible, GUILayout.Width(18f));
            string name = !string.IsNullOrEmpty(placement.displayName)
                ? placement.displayName
                : Path.GetFileNameWithoutExtension(placement.prefabPath);
            if (GUILayout.Button(name, EditorStyles.miniButtonLeft))
            {
                selectedPlacement = placement;
            }

            if (GUILayout.Button("Remove", EditorStyles.miniButtonRight, GUILayout.Width(62f)))
            {
                onRemove?.Invoke(placement);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField(placement.id, EditorStyles.miniLabel);
            EditorGUILayout.LabelField(string.Format("px ({0:0}, {1:0}) layer {2}", placement.positionPx.x, placement.positionPx.y, placement.layer), EditorStyles.miniLabel);
            EditorGUILayout.EndVertical();
        }

        private static void DrawSeparator()
        {
            Rect rect = GUILayoutUtility.GetRect(1f, 8f, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(new Rect(rect.x, rect.y + 4f, rect.width, 1f), new Color(0.28f, 0.28f, 0.28f, 1f));
        }
    }
}
