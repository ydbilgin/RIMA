using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Hard-caps the frame rate at startup. The 2D game was running UNCAPPED
    /// (vSyncCount=0, targetFrameRate=-1) which on a high-end GPU pushes 1000+ FPS →
    /// sustained 100% GPU load → long-session graphics-device instability
    /// (native D3D "Assertion failed on expression: 'SUCCEEDED(hr)'" + editor stalls).
    /// A 60 FPS cap removes the pointless GPU thrash and keeps editor Play sessions stable
    /// for the live demo. Applies in the editor and standalone builds.
    /// </summary>
    internal static class FrameRateGuard
    {
        private const int TargetFps = 60;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Apply()
        {
            // targetFrameRate only takes effect when vSync is off; force a hard, predictable cap.
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = TargetFps;
        }
    }
}
