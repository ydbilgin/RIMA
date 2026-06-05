#if UNITY_EDITOR
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RIMA.Editor.Map
{
    public static class RoomTemplateBuildUtility
    {
        public const string ArenaScenePath = "Assets/Scenes/_Arena.unity";

        public static bool BuildInArena(RoomTemplateSO template, string sourceLabel)
        {
            if (template == null)
            {
                return false;
            }

            if (Application.isPlaying)
            {
                EditorUtility.DisplayDialog(sourceLabel, "Play mode acikken kullanilamaz. Once Play'i durdur (F6).", "Tamam");
                return false;
            }

            Scene active = EditorSceneManager.GetActiveScene();
            if (active.path != ArenaScenePath)
            {
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    return false;
                }

                EditorSceneManager.OpenScene(ArenaScenePath, OpenSceneMode.Single);
            }

            IsoRoomBuilder builder = Object.FindFirstObjectByType<IsoRoomBuilder>();
            if (builder == null)
            {
                EditorUtility.DisplayDialog(sourceLabel, "_Arena sahnesinde IsoRoomBuilder bulunamadi.", "Tamam");
                return false;
            }

            IsoRoomBuilderTester tester = builder.GetComponent<IsoRoomBuilderTester>();
            if (tester != null)
            {
                SerializedObject so = new SerializedObject(tester);
                SerializedProperty prop = so.FindProperty("template");
                if (prop != null)
                {
                    prop.objectReferenceValue = template;
                    so.ApplyModifiedPropertiesWithoutUndo();
                }
            }

            BuildAndFrame(builder, template);
            Debug.Log($"[RoomTemplateBuildUtility] Built room '{template.name}' in _Arena from {sourceLabel}.");
            return true;
        }

        public static bool TryRebuildIfActiveArenaTemplate(RoomTemplateSO template)
        {
            if (template == null || Application.isPlaying)
            {
                return false;
            }

            if (EditorSceneManager.GetActiveScene().path != ArenaScenePath)
            {
                return false;
            }

            IsoRoomBuilder builder = Object.FindFirstObjectByType<IsoRoomBuilder>();
            if (builder == null)
            {
                return false;
            }

            IsoRoomBuilderTester tester = builder.GetComponent<IsoRoomBuilderTester>();
            if (tester != null)
            {
                SerializedObject so = new SerializedObject(tester);
                SerializedProperty prop = so.FindProperty("template");
                if (prop != null && prop.objectReferenceValue != template)
                {
                    return false;
                }
            }

            BuildAndFrame(builder, template);
            Debug.Log($"[RoomTemplateBuildUtility] Rebuilt active room '{template.name}' in _Arena.");
            return true;
        }

        private static void BuildAndFrame(IsoRoomBuilder builder, RoomTemplateSO template)
        {
            if (builder == null || template == null)
            {
                return;
            }

            builder.Build(template);
            Selection.activeObject = builder.gameObject;
            if (SceneView.lastActiveSceneView == null)
            {
                return;
            }

            if (builder.TryGetLastFloorWorldBounds(out Bounds bounds))
            {
                SceneView.lastActiveSceneView.Frame(bounds, false);
            }
            else
            {
                SceneView.lastActiveSceneView.Frame(
                    new Bounds(builder.transform.position + new Vector3(6f, 3f, 0f), new Vector3(28f, 16f, 1f)),
                    false);
            }
        }
    }
}
#endif
