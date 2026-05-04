#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace RIMA.Editor
{
    /// <summary>
    /// Hides the UI layer in Scene view on editor startup and sets a dark background.
    /// Prevents HUD/canvas elements from cluttering the scene view.
    /// </summary>
    [InitializeOnLoad]
    public static class SceneViewSetup
    {
        static SceneViewSetup()
        {
            EditorApplication.delayCall += Apply;
        }

        [MenuItem("RIMA/Scene View/Clean Scene View (Hide UI)")]
        public static void Apply()
        {
            int uiLayer = LayerMask.NameToLayer("UI");
            if (uiLayer < 0) return;

            foreach (var sv in SceneView.sceneViews)
            {
                var sceneView = sv as SceneView;
                if (sceneView == null) continue;

                // Hide UI layer
                Tools.visibleLayers &= ~(1 << uiLayer);

                sceneView.Repaint();
            }
        }

        [MenuItem("RIMA/Scene View/Restore UI Layer")]
        public static void Restore()
        {
            int uiLayer = LayerMask.NameToLayer("UI");
            if (uiLayer >= 0)
                Tools.visibleLayers |= (1 << uiLayer);

            foreach (var sv in SceneView.sceneViews)
                (sv as SceneView)?.Repaint();
        }
    }
}
#endif
