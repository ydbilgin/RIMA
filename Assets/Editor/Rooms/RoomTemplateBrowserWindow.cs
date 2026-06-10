using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RIMA.MapDesigner.Room.Data;
using UnityEditor;
using UnityEngine;

namespace RIMA.EditorTools
{
    /// <summary>
    /// Quick room showcase tool: lists every RoomTemplateSO and builds the
    /// clicked one in the _Arena scene via IsoRoomBuilder. Made for live demos.
    /// </summary>
    public sealed class RoomTemplateBrowserWindow : EditorWindow
    {
        private const string RoomsRoot = "Assets/Data/Rooms";

        private Vector2 scroll;
        private string search = string.Empty;
        private List<RoomTemplateSO> templates = new();

        [MenuItem("RIMA/Room Browser", priority = 2)]
        private static void Open()
        {
            var window = GetWindow<RoomTemplateBrowserWindow>("Room Browser");
            window.minSize = new Vector2(300f, 400f);
            window.RefreshList();
        }

        private void OnEnable()
        {
            RefreshList();
        }

        private void RefreshList()
        {
            templates = AssetDatabase.FindAssets("t:RoomTemplateSO", new[] { RoomsRoot })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<RoomTemplateSO>)
                .Where(t => t != null)
                .OrderBy(t => AssetDatabase.GetAssetPath(t))
                .ToList();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(4f);
            using (new EditorGUILayout.HorizontalScope())
            {
                search = EditorGUILayout.TextField(search, EditorStyles.toolbarSearchField);
                if (GUILayout.Button("Yenile", GUILayout.Width(60f)))
                {
                    RefreshList();
                }
            }

            EditorGUILayout.HelpBox("Tıkla → oda _Arena sahnesinde kurulur (edit mode, Play gerekmez).", MessageType.Info);

            scroll = EditorGUILayout.BeginScrollView(scroll);
            string currentFolder = null;
            foreach (var template in templates)
            {
                string path = AssetDatabase.GetAssetPath(template);
                // GetAssetPath, asset import/build sırasında geçici olarak "" dönebilir;
                // Mono'da Path.GetDirectoryName("") ArgumentException fırlatır → GUILayout state bozulur.
                if (string.IsNullOrEmpty(path)) continue;
                if (!string.IsNullOrEmpty(search) &&
                    !template.name.ToLowerInvariant().Contains(search.ToLowerInvariant()))
                {
                    continue;
                }

                string folder = System.IO.Path.GetDirectoryName(path)?.Replace('\\', '/');
                if (folder != currentFolder)
                {
                    currentFolder = folder;
                    EditorGUILayout.Space(6f);
                    EditorGUILayout.LabelField(folder?.Replace(RoomsRoot, "Rooms"), EditorStyles.boldLabel);
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(new GUIContent(template.name, "Odayı _Arena sahnesinde kur"), GUILayout.Height(22f)))
                    {
                        BuildInArena(template);
                    }
                    if (GUILayout.Button(new GUIContent("Sahnede Kur ▶", "Odayı _Arena sahnesinde kur"), GUILayout.Width(96f), GUILayout.Height(22f)))
                    {
                        BuildInArena(template);
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private static void BuildInArena(RoomTemplateSO template)
        {
            System.Type utilityType = System.Type.GetType("RIMA.Editor.Map.RoomTemplateBuildUtility, RIMA.Editor");
            MethodInfo method = utilityType != null
                ? utilityType.GetMethod("BuildInArena", BindingFlags.Public | BindingFlags.Static)
                : null;
            if (method == null)
            {
                EditorUtility.DisplayDialog("Room Browser", "RoomTemplateBuildUtility bulunamadı.", "Tamam");
                return;
            }

            method.Invoke(null, new object[] { template, "Room Browser" });
        }
    }
}
