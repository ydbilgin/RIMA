namespace RIMA.Editor.RoomDesigner
{
    using UnityEditor;
    using UnityEditor.SceneManagement;

    public static class DemoMenu
    {
        private const string ScenePath = "Assets/Scenes/Demo/RoomPipelineTest.unity";

        [MenuItem("RIMA/Demo/Open Room Pipeline Test")]
        public static void OpenRoomPipelineTest()
        {
            EditorSceneManager.OpenScene(ScenePath);
        }
    }
}
