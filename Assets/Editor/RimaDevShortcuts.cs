using UnityEditor;
using UnityEditor.SceneManagement;

namespace RIMA.EditorTools
{
    public static class RimaDevShortcuts
    {
        private const string ArenaScenePath = "Assets/Scenes/Test/PlayableArena_Test01.unity";

        [MenuItem("RIMA/Play Arena _F5")]
        private static void PlayArena()
        {
            if (!EditorSceneManager.SaveOpenScenes()) return;

            EditorSceneManager.OpenScene(ArenaScenePath);
            EditorApplication.isPlaying = true;
        }

        [MenuItem("RIMA/Stop Play _F6")]
        private static void StopPlay()
        {
            EditorApplication.isPlaying = false;
        }
    }
}
