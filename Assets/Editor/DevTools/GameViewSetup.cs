using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RIMA.Editor.DevTools
{
    /// <summary>
    /// Configures Game View settings on project open:
    ///   - Maximize on Play enabled
    ///   - Forces a fixed EVEN resolution (1280x720, a clean 4x of the 320x180
    ///     Pixel Perfect ref). Free Aspect produces odd window widths which make
    ///     PixelPerfectCamera spam "Rendering at an odd-numbered resolution".
    ///     We add the size if missing and select it whenever the current target
    ///     size is odd (self-healing, never fights a deliberate user choice).
    /// </summary>
    [InitializeOnLoad]
    public static class GameViewSetup
    {
        private const string Label = "RIMA 1920x1080";
        private const int W = 1920;
        private const int H = 1080;

        // Static constructor runs on every domain reload (project open / recompile).
        static GameViewSetup()
        {
            // Defer one tick so EditorWindows are ready after a domain reload.
            EditorApplication.delayCall += () => EnsureEvenResolution(forced: false);
        }

        [MenuItem("RIMA/Utilities/Setup Game View (720p even + Maximize)")]
        private static void RunSetupForced()
        {
            EnsureEvenResolution(forced: true);
        }

        public static void RunSetupForcedForTools()
        {
            EnsureEvenResolution(forced: true);
        }

        private static void EnsureEvenResolution(bool forced)
        {
            EditorPrefs.SetBool("GameView.maximizeOnPlay", true);

            try
            {
                var asm = typeof(UnityEditor.Editor).Assembly;
                var gameViewType = asm.GetType("UnityEditor.GameView");
                if (gameViewType == null) return;

                var window = EditorWindow.GetWindow(gameViewType, utility: false,
                                                    title: null, focus: false);
                if (window == null) return;

                // Skip if current size is already even and we are not forcing.
                if (!forced)
                {
                    var targetSizeProp = gameViewType.GetProperty("targetSize",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                    if (targetSizeProp != null && targetSizeProp.GetValue(window) is Vector2 cur)
                    {
                        bool even = ((int)cur.x % 2 == 0) && ((int)cur.y % 2 == 0);
                        if (even) return;
                    }
                }

                int index = AddOrFindSize(asm, out object group, out MethodInfo getGameViewSize);
                if (index < 0 || group == null) return;

                var sizeCb = gameViewType.GetMethod("SizeSelectionCallback",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var theSize = getGameViewSize.Invoke(group, new object[] { index });
                sizeCb.Invoke(window, new object[] { index, theSize });
                window.Repaint();

                if (forced)
                    Debug.Log($"RIMA: Game View set to {Label} (even, pixel-perfect) + Maximize on Play.");
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"RIMA GameViewSetup: could not auto-set resolution ({ex.Message}). " +
                                 "Pick a fixed even resolution in the Game View dropdown manually.");
            }
        }

        /// <summary>
        /// Ensures the fixed 1280x720 custom size exists in the current group and returns its index.
        /// </summary>
        private static int AddOrFindSize(Assembly asm, out object group, out MethodInfo getGameViewSize)
        {
            group = null;
            getGameViewSize = null;

            var sizesType = asm.GetType("UnityEditor.GameViewSizes");
            var singleton = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
            var instance = singleton.GetProperty("instance").GetValue(null);
            var currentGroupType = sizesType.GetProperty("currentGroupType").GetValue(instance);
            group = sizesType.GetMethod("GetGroup").Invoke(instance, new object[] { (int)currentGroupType });
            var groupType = group.GetType();

            getGameViewSize = groupType.GetMethod("GetGameViewSize");
            var getBuiltinCount = groupType.GetMethod("GetBuiltinCount");
            var getCustomCount = groupType.GetMethod("GetCustomCount");

            int builtin = (int)getBuiltinCount.Invoke(group, null);
            int custom = (int)getCustomCount.Invoke(group, null);
            int total = builtin + custom;

            for (int i = 0; i < total; i++)
            {
                var s = getGameViewSize.Invoke(group, new object[] { i });
                if (s.GetType().GetProperty("baseText").GetValue(s) as string == Label)
                    return i;
            }

            var gvSizeType = asm.GetType("UnityEditor.GameViewSize");
            var gvSizeTypeEnum = asm.GetType("UnityEditor.GameViewSizeType");
            var ctor = gvSizeType.GetConstructor(new[] { gvSizeTypeEnum, typeof(int), typeof(int), typeof(string) });
            var newSize = ctor.Invoke(new object[] { 1 /*FixedResolution*/, W, H, Label });
            groupType.GetMethod("AddCustomSize").Invoke(group, new object[] { newSize });
            EditorUtility.SetDirty(instance as UnityEngine.Object);

            return builtin + (int)getCustomCount.Invoke(group, null) - 1;
        }
    }
}
