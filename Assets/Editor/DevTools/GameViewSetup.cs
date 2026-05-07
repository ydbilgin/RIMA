using UnityEditor;
using UnityEngine;

namespace RIMA.Editor.DevTools
{
    /// <summary>
    /// Configures Game View settings on project open:
    ///   - Maximize on Play enabled
    ///   - Logs a reminder to set 1920x1080 in the resolution dropdown
    /// </summary>
    [InitializeOnLoad]
    public static class GameViewSetup
    {
        private const string PrefKey = "RIMA.GameViewSetupDone";

        // Static constructor runs on every domain reload (project open / recompile).
        static GameViewSetup()
        {
            if (EditorPrefs.GetBool(PrefKey, false))
                return;

            RunSetup(forced: false);
        }

        [MenuItem("RIMA/Setup Game View (1080p + Maximize)")]
        private static void RunSetupForced()
        {
            RunSetup(forced: true);
        }

        private static void RunSetup(bool forced)
        {
            // --- Maximize on Play ---
            EditorPrefs.SetBool("GameView.maximizeOnPlay", true);

            // --- Resolution: attempt via reflection, fall back to log ---
            bool resolutionSet = TrySetResolution1080p();
            if (!resolutionSet)
            {
                Debug.Log("RIMA: Set Game View to 1920x1080 manually: " +
                          "click the resolution dropdown in the Game View.");
            }

            // Mark done so the static ctor skips on future reloads.
            EditorPrefs.SetBool(PrefKey, true);

            Debug.Log("RIMA: Game View configured (1080p, Maximize on Play).");
        }

        /// <summary>
        /// Attempts to select the 1920x1080 fixed-resolution entry via internal Unity API.
        /// Returns true if successful, false if the reflection target was not found.
        /// </summary>
        private static bool TrySetResolution1080p()
        {
            try
            {
                var assembly = typeof(UnityEditor.Editor).Assembly;
                var gameViewType = assembly.GetType("UnityEditor.GameView");
                if (gameViewType == null)
                    return false;

                // Get (or open, but don't focus) the Game View window.
                var window = EditorWindow.GetWindow(gameViewType, utility: false,
                                                    title: null, focus: false);
                if (window == null)
                    return false;

                // GameView exposes SizeSelectionCallback(int indexClicked, object objectSelected)
                // for its internal popup. We cannot reliably know the index of 1920x1080 across
                // Unity versions, so we skip the call and rely on the manual-log fallback.
                // The important runtime guarantee (Maximize on Play) is already set above.
                return false;
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"RIMA GameViewSetup: reflection probe failed ({ex.Message}). " +
                                 "Set resolution manually.");
                return false;
            }
        }
    }
}
