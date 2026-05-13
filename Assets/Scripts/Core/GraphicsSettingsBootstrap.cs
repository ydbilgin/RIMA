using UnityEngine;
using UnityEngine.Rendering;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RIMA
{
    public static class GraphicsSettingsBootstrap
    {
        private static readonly Vector3 YAxisTransparencySort = new Vector3(0f, 1f, 0f);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ApplyBeforeSceneLoad()
        {
            Apply();
        }

        public static void Apply()
        {
            GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
            GraphicsSettings.transparencySortAxis = YAxisTransparencySort;
        }
    }

#if UNITY_EDITOR
    [InitializeOnLoad]
    internal static class GraphicsSettingsBootstrapEditor
    {
        static GraphicsSettingsBootstrapEditor()
        {
            GraphicsSettingsBootstrap.Apply();
        }
    }
#endif
}
