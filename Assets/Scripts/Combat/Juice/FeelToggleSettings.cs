using UnityEngine;

namespace RIMA.Combat.Juice
{
    public static class FeelToggleSettings
    {
        public static bool ShakeEnabled = true;
        public static bool HitstopEnabled = true;
        public static bool VignetteEnabled = true;
        public static bool CameraPunchEnabled = true;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetOnDomainReload()
        {
            ShakeEnabled = true;
            HitstopEnabled = true;
            VignetteEnabled = true;
            CameraPunchEnabled = true;
        }
    }
}
