#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RIMA.Editor
{
    public sealed class RoomPreviewPanel : EditorWindow
    {
        private int selectedIndex;
        private Vector2 scroll;

        [MenuItem("RIMA/Scene View/Room Preview Panel")]
        public static void Open()
        {
            var window = GetWindow<RoomPreviewPanel>("RIMA Rooms");
            window.minSize = new Vector2(300f, 360f);
            window.Show();
        }

        [MenuItem("RIMA/Scene View/Preview Room 01")]
        public static void PreviewFirstRoom()
        {
            PaintRoom(0);
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(8f);
            EditorGUILayout.LabelField("RIMA Room Preview", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "Use this to inspect procedural room layouts in the Scene view. Edit Mode paints only the map. Play Mode uses the runtime test transition.",
                MessageType.Info);

            int count = LargeDungeonMapPainterBase.DefaultPreviewLayoutCount;
            string[] labels = new string[count];
            for (int i = 0; i < count; i++)
                labels[i] = LargeDungeonMapPainterBase.GetDefaultPreviewLayoutName(i);

            selectedIndex = EditorGUILayout.Popup("Room", Mathf.Clamp(selectedIndex, 0, count - 1), labels);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Paint Selected", GUILayout.Height(32f)))
                    PaintRoom(selectedIndex);

                if (GUILayout.Button("Frame Scene", GUILayout.Height(32f)))
                    FrameCurrentRoom();
            }

            EditorGUILayout.Space(8f);
            EditorGUILayout.LabelField("Quick Rooms", EditorStyles.boldLabel);

            scroll = EditorGUILayout.BeginScrollView(scroll);
            for (int i = 0; i < count; i++)
            {
                if (GUILayout.Button(LargeDungeonMapPainterBase.GetDefaultPreviewLayoutName(i), GUILayout.Height(26f)))
                {
                    selectedIndex = i;
                    PaintRoom(i);
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private static void PaintRoom(int index)
        {
            if (Application.isPlaying)
            {
                RuntimeRoomManager.Instance?.PreviewRoomByIndex(index);
                FrameCurrentRoom();
                return;
            }

            LargeDungeonMapPainter painter = Object.FindAnyObjectByType<LargeDungeonMapPainter>();
            if (painter == null)
            {
                Debug.LogWarning("[RoomPreviewPanel] LargeDungeonMapPainter not found in the loaded scene.");
                return;
            }

            foreach (GameObject root in painter.gameObject.scene.GetRootGameObjects())
                Undo.RegisterFullObjectHierarchyUndo(root, "Paint RIMA Preview Room");
            painter.PaintPreviewLayout(index);

            EditorUtility.SetDirty(painter);
            EditorSceneManager.MarkSceneDirty(painter.gameObject.scene);
            Selection.activeGameObject = painter.gameObject;
            FrameCurrentRoom(painter);

            Debug.Log($"[RoomPreviewPanel] Painted preview room: {LargeDungeonMapPainterBase.GetDefaultPreviewLayoutName(index)}");
        }

        private static void FrameCurrentRoom()
        {
            LargeDungeonMapPainter painter = Object.FindAnyObjectByType<LargeDungeonMapPainter>();
            FrameCurrentRoom(painter);
        }

        private static void FrameCurrentRoom(LargeDungeonMapPainter painter)
        {
            if (painter == null) return;

            SceneView sceneView = SceneView.lastActiveSceneView;
            if (sceneView == null && SceneView.sceneViews.Count > 0)
                sceneView = SceneView.sceneViews[0] as SceneView;
            if (sceneView == null) return;

            float width = Mathf.Max(1f, painter.RoomWidth);
            float height = Mathf.Max(1f, painter.RoomHeight);
            Vector3 center = new Vector3(width * 0.5f, height * 0.5f, 0f);
            float size = Mathf.Max(width, height) * 0.62f;

            sceneView.in2DMode = true;
            sceneView.LookAt(center, sceneView.rotation, size);
            sceneView.Repaint();
        }
    }
}
#endif
