using UnityEngine;

namespace RIMA
{
    [CreateAssetMenu(menuName = "RIMA/Combat/KnockdownProfile")]
    public class KnockdownProfile : ScriptableObject
    {
        private const string DefaultResourcePath = "Combat/Knockdown/KnockdownProfile_Heavy";
        private static KnockdownProfile defaultRuntimeProfile;

        [Header("Arc")]
        public float launchDuration = 0.18f;
        public float arcHeight = 0.46f;
        public float tiltAngle = 35f;

        [Header("Landing")]
        public float landingSquashY = 0.60f;
        public float landingSquashX = 1.12f;
        public float landingSquashDuration = 0.08f;
        public int bounceCount = 1;
        public float bounceHeight = 0.12f;
        public float bounceDuration = 0.10f;

        [Header("Recovery")]
        public float downTime = 0.55f;
        public float getUpDuration = 0.22f;
        public float getUpIFrame = 0.24f;

        [Header("Shadow")]
        public Vector2 shadowSize = new Vector2(0.95f, 0.32f);
        [Range(0f, 1f)] public float shadowAlpha = 0.34f;

        public static KnockdownProfile Default
        {
            get
            {
                if (defaultRuntimeProfile != null) return defaultRuntimeProfile;

                defaultRuntimeProfile = Resources.Load<KnockdownProfile>(DefaultResourcePath);
                if (defaultRuntimeProfile != null) return defaultRuntimeProfile;

                defaultRuntimeProfile = CreateInstance<KnockdownProfile>();
                defaultRuntimeProfile.name = "KnockdownProfile_RuntimeDefault";
                return defaultRuntimeProfile;
            }
        }

        public float LaunchDuration => Mathf.Max(0.01f, launchDuration);
        public float LandingSquashDuration => Mathf.Max(0.01f, landingSquashDuration);
        public float BounceDuration => Mathf.Max(0.01f, bounceDuration);
        public float DownTime => Mathf.Max(0f, downTime);
        public float GetUpDuration => Mathf.Max(0.01f, getUpDuration);
        public float GetUpIFrame => Mathf.Max(0f, getUpIFrame);
        public float LandingSquashY => Mathf.Clamp(landingSquashY, 0.2f, 1f);
        public float LandingSquashX => Mathf.Max(1f, landingSquashX);
        public int BounceCount => Mathf.Clamp(bounceCount, 0, 2);
        public float ArcHeight => Mathf.Max(0f, arcHeight);
        public float BounceHeight => Mathf.Max(0f, bounceHeight);
    }
}
