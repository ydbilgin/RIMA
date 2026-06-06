#if UNITY_EDITOR
using RIMA.DebugTools;
using UnityEditor;
using UnityEngine;

namespace RIMA.Editor.DevTools
{
    public static class ScreenshotModeMenu
    {
        [MenuItem("RIMA/Utilities/Screenshot Mode/Capture All Presets")]
        private static void CaptureAllPresets()
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("[ScreenshotMode] Enter Play Mode before capturing presets.");
                return;
            }

            GameViewSetup.RunSetupForcedForTools();
            ScreenshotMode.CaptureAllPresets();
        }

        [MenuItem("RIMA/Utilities/Screenshot Mode/Capture Current Preset")]
        private static void CaptureCurrentPreset()
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("[ScreenshotMode] Enter Play Mode before capturing the current preset.");
                return;
            }

            GameViewSetup.RunSetupForcedForTools();
            ScreenshotMode.CaptureCurrentPreset();
        }

        [MenuItem("RIMA/Utilities/Screenshot Mode/Toggle Debug Surfaces")]
        private static void ToggleDebugSurfaces()
        {
            ScreenshotMode.Toggle();
        }
    }
}
#endif
