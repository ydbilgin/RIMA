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
        MarkPulse,
        IaidoStance
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
        /// <summary>
        /// Startup (windup) duration before the hit resolves. 0 = immediate (legacy).
        /// A5 can expose per-step array; for graybox, one global value is sufficient.
        /// </summary>
        public float attackStartup = 0.08f;
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

        [Header("Dash-Cancel")]
        [Tooltip("-1 = use class default. 0 = not cancellable. 0-1 = cancel window fraction of commit duration.")]
        public float cancelWindowFraction = -1f;

        /// <summary>Returns the effective dash-cancel window (0-1). 0 = blocked during commitment.</summary>
        public float GetCancelWindow()
        {
            if (cancelWindowFraction >= 0f) return cancelWindowFraction;
            return classType switch
            {
                ClassType.Warblade or ClassType.Brawler    => 0.67f,
                ClassType.Ranger or ClassType.Gunslinger   => 0.42f,
                ClassType.Ravager or ClassType.Shadowblade => 0.20f,
                _                                          => 0f,
            };
        }

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
                BasicAttackBehaviorType.HeatGauge => new HeatGaugeBehavior(),
                BasicAttackBehaviorType.MarkPulse => new MarkPulseBehavior(),
                BasicAttackBehaviorType.IaidoStance => new MeleeChainBehavior(),
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
