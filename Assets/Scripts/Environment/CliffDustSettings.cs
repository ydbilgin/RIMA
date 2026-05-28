using UnityEngine;

namespace RIMA.Environment
{
    /// <summary>F4: ScriptableObject config for CliffEdgeDustEmitter particle settings.</summary>
    [CreateAssetMenu(fileName = "CliffDustSettings_Default", menuName = "RIMA/Environment/Cliff Dust Settings")]
    public sealed class CliffDustSettings : ScriptableObject
    {
        [Header("Emission")]
        [Tooltip("Particles emitted per second per edge cell.")]
        [Range(0.1f, 3f)]
        public float emissionRate = 0.75f;

        [Header("Lifetime")]
        [Range(0.5f, 4f)]
        public float lifetimeMin = 1.5f;
        [Range(0.5f, 4f)]
        public float lifetimeMax = 2.0f;

        [Header("Velocity")]
        [Tooltip("Downward speed (positive = faster fall).")]
        [Range(0f, 2f)]
        public float fallSpeed = 0.3f;
        [Range(0f, 0.5f)]
        public float lateralSpread = 0.1f;
        [Range(0f, 1f)]
        public float gravityModifier = 0.1f;

        [Header("Appearance")]
        [Tooltip("Base dust tint. Alpha is the maximum opacity at birth.")]
        public Color colorTint = new Color(0.55f, 0.45f, 0.35f, 0.3f);
        [Range(0.02f, 0.2f)]
        public float startSizeMin = 0.04f;
        [Range(0.02f, 0.2f)]
        public float startSizeMax = 0.10f;

        [Header("Performance")]
        [Tooltip("Scene-wide particle cap. Emitters reduce output to stay under this total.")]
        public int maxTotalParticles = 200;
        [Tooltip("Camera distance beyond which emitters disable emission.")]
        public float lodCullDistance = 20f;
    }
}
