using UnityEngine;

namespace RIMA.Balance
{
    public struct DamageCalculationResult
    {
        public int finalDamage;
        public float statMultiplier;
        public float cappedIdentityBuildMultiplier;
        public float cappedSituationalMultiplier;
        public float defenseMultiplier;
        public float postureOverflowMultiplier;
        public int postureOverflowDamage;
    }

    public static class DamageCalculator
    {
        public const float IdentityBuildCap = 3f;
        public const float SituationalCap = 2f;
        public const float DefenseK = 100f;
        private const float DefaultCritMultiplier = 1.5f;

        public static DamageCalculationResult Calculate(
            DamagePacket packet,
            ClassStatRuntime attackerStats = null,
            ClassStatRuntime defenderStats = null)
        {
            attackerStats ??= ClassStatRuntime.Neutral;

            float statMultiplier = packet.damageType switch
            {
                DamageType.Physical => attackerStats.physPower / 100f,
                DamageType.Ability => attackerStats.abilityPower / 100f,
                DamageType.True => 1f,
                _ => 1f
            };

            float rawIdentityBuild = Mathf.Max(0f, attackerStats.identityBuildMultiplier);
            float cappedIdentityBuild = Mathf.Min(rawIdentityBuild, IdentityBuildCap);
            float overflowMultiplier = Mathf.Max(0f, rawIdentityBuild - IdentityBuildCap);

            float cappedSituational = Mathf.Min(Mathf.Max(0f, attackerStats.situationalMultiplier), SituationalCap);
            float debugMult = Mathf.Max(0f, attackerStats.debugGlobalDamageMult);
            float critMult = packet.isCrit ? Mathf.Max(packet.critMultiplier > 0f ? packet.critMultiplier : DefaultCritMultiplier, 1f) : 1f;

            float rawDamage = packet.baseDamage
                              * statMultiplier
                              * cappedIdentityBuild
                              * cappedSituational
                              * debugMult
                              * critMult;

            float defenseMultiplier = GetDefenseMultiplier(packet.damageType, defenderStats);
            rawDamage *= defenseMultiplier;

            int finalDamage = Mathf.Max(1, Mathf.RoundToInt(rawDamage));

            int postureOverflowDamage = 0;
            if (overflowMultiplier > 0f)
            {
                postureOverflowDamage = Mathf.RoundToInt(packet.baseDamage * statMultiplier * overflowMultiplier);
            }

            return new DamageCalculationResult
            {
                finalDamage = finalDamage,
                statMultiplier = statMultiplier,
                cappedIdentityBuildMultiplier = cappedIdentityBuild,
                cappedSituationalMultiplier = cappedSituational,
                defenseMultiplier = defenseMultiplier,
                postureOverflowMultiplier = overflowMultiplier,
                postureOverflowDamage = postureOverflowDamage
            };
        }

        private static float GetDefenseMultiplier(DamageType damageType, ClassStatRuntime defenderStats)
        {
            if (defenderStats == null || damageType == DamageType.True) return 1f;

            float resistance = damageType switch
            {
                DamageType.Physical => defenderStats.armor,
                DamageType.Ability => defenderStats.magicResist,
                _ => 0f
            };

            resistance = Mathf.Max(0f, resistance);
            float reduction = resistance / (resistance + DefenseK);
            return Mathf.Clamp01(1f - reduction);
        }
    }
}
