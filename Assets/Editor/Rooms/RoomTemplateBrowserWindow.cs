using System.Collections.Generic;
using System.Linq;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RIMA.EditorTools
{
    /// <summary>
    /// Quick room showcase tool: lists every RoomTemplateSO and builds the
    /// clicked one in the _Arena scene via IsoRoomBuilder. Made for live demos.
    /// </summary>
    public sealed class RoomTemplateBrowserWindow : EditorWindow
    {
        private const string ArenaScenePath = "Assets/Scenes/_Arena.unity";
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
                    if (GUILayout.Button(template.name, GUILayout.Height(22f)))
                    {
                        BuildInArena(template);
                    }
                    if (GUILayout.Button("Göster", GUILayout.Width(52f)))
                    {
                        EditorGUIUtility.PingObject(template);
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private static void BuildInArena(RoomTemplateSO template)
        {
            if (Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Room Browser", "Play mode açıkken kullanılamaz. Önce Play'i durdur (F6).", "Tamam");
                return;
            }

            // Make sure _Arena is the open scene.
            var active = EditorSceneManager.GetActiveScene();
            if (active.path != ArenaScenePath)
            {
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    return;
                }
                EditorSceneManager.OpenScene(ArenaScenePath, OpenSceneMode.Single);
            }

            var builder = Object.FindFirstObjectByType<IsoRoomBuilder>();
            if (builder == null)
            {
                EditorUtility.DisplayDialog("Room Browser", "_Arena sahnesinde IsoRoomBuilder bulunamadı.", "Tamam");
                return;
            }

            // Keep the tester in sync so a later Play uses the same template.
            var tester = builder.GetComponent<IsoRoomBuilderTester>();
            if (tester != null)
            {
                var so = new SerializedObject(tester);
                var prop = so.FindProperty("template");
                if (prop != null)
                {
                    prop.objectReferenceValue = template;
                    so.ApplyModifiedPropertiesWithoutUndo();
                }
            }

            builder.Build(template);
            SceneView.lastActiveSceneView?.FrameSelected();
            Selection.activeObject = builder.gameObject;
            if (SceneView.lastActiveSceneView != null)
            {
                SceneView.lastActiveSceneView.Frame(new Bounds(builder.transform.position + new Vector3(6f, 3f, 0f), new Vector3(28f, 16f, 1f)), false);
            }
            Debug.Log($"[RoomBrowser] Built room '{template.name}' in _Arena.");
        }
    }
}
