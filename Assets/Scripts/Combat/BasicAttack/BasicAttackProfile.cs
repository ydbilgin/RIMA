using UnityEngine;

namespace RIMA
{
    public enum BasicAttackBehaviorType
    {
        Melee,
        CastRhythm,
        ShotCadence,
        VeilStrike,
        HeatGauge,
        MarkPulse
    }

    /// <summary>
    /// Pure-data ScriptableObject for basic attack configuration.
    /// All execution logic lives in IBasicAttackBehavior implementations.
    /// One asset per class. No per-class branching here.
    /// </summary>
    [CreateAssetMenu(menuName = "RIMA/Combat/BasicAttackProfile")]
    public class BasicAttackProfile : ScriptableObject
    {
        [Header("Class Identity")]
        public ClassType classType;
        public BasicAttackBehaviorType behaviorType;

        [Header("Melee Combo")]
        public int comboLength = 3;
        public float comboWindow = 1.2f;
        public float commitment = 0.28f;
        public int[] comboDamage = { 25, 30, 40 };
        public float[] hitRange = { 1.2f, 1.3f, 1.5f };
        public float[] hitRadius = { 0.75f, 0.75f, 0.9f };
        public float[] knockbackForce = { 4f, 5f, 8f };
        public float[] knockbackDuration = { 0.10f, 0.12f, 0.18f };

        [Header("Projectile / Strike (CastRhythm, ShotCadence, VeilStrike)")]
        public int projectileDamage = 18;
        public float projectileSpeed = 15f;
        public float projectileCooldown = 0.34f;
        public float chargeThreshold = 1f;

        [Header("RMB")]
        public int rmbCost = 30;
        public int rmbDamage = 34;
        public float rmbRadius = 2.2f;
        public float rmbCooldown = 1.5f;

        /// <summary>
        /// Factory: creates the correct behavior instance based on behaviorType.
        /// </summary>
        public IBasicAttackBehavior CreateBehavior()
        {
            return behaviorType switch
            {
                BasicAttackBehaviorType.CastRhythm => new CastRhythmBehavior(),
                BasicAttackBehaviorType.ShotCadence => new ShotCadenceBehavior(),
                BasicAttackBehaviorType.VeilStrike => new VeilStrikeBehavior(),
                // HeatGauge and MarkPulse will get their own behaviors when implemented.
                // For now they fall back to MeleeChainBehavior.
                _ => new MeleeChainBehavior()
            };
        }

        public float GetHitRangeForStep(int step)
        {
            if (hitRange == null || hitRange.Length == 0) return 0f;
            return hitRange[Mathf.Min(step, hitRange.Length - 1)];
        }

        public float GetHitRadiusForStep(int step)
        {
            if (hitRadius == null || hitRadius.Length == 0) return 0f;
            return hitRadius[Mathf.Min(step, hitRadius.Length - 1)];
        }

        public bool Validate(out string error)
        {
            if (comboDamage == null || comboDamage.Length != comboLength)
            { error = $"comboDamage length must be {comboLength}"; return false; }
            if (hitRange == null || hitRange.Length != comboLength)
            { error = $"hitRange length must be {comboLength}"; return false; }
            if (hitRadius == null || hitRadius.Length != comboLength)
            { error = $"hitRadius length must be {comboLength}"; return false; }
            if (knockbackForce == null || knockbackForce.Length != comboLength)
            { error = $"knockbackForce length must be {comboLength}"; return false; }
            if (knockbackDuration == null || knockbackDuration.Length != comboLength)
            { error = $"knockbackDuration length must be {comboLength}"; return false; }
            error = null;
            return true;
        }
    }
}
