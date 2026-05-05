using UnityEngine;

namespace RIMA
{
    public enum BasicAttackBehaviorType
    {
        Melee,
        CastRhythm,
        ShotCadence,
        HeatGauge,
        MarkPulse
    }

    [CreateAssetMenu(menuName = "RIMA/Combat/BasicAttackProfile")]
    public class BasicAttackProfile : ScriptableObject
    {
        // If ClassType enum exists in project use it; otherwise int is used as fallback
        public int classType;
        public int comboLength = 3;
        public float comboWindow = 1.2f;
        public float commitment = 0.28f;
        public int[] comboDamage = { 25, 30, 40 };
        public float[] hitRange = { 1.2f, 1.3f, 1.5f };
        public float[] hitRadius = { 0.75f, 0.75f, 0.9f };
        public float[] knockbackForce;
        public float[] knockbackDuration;
        public BasicAttackBehaviorType behaviorType;

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
