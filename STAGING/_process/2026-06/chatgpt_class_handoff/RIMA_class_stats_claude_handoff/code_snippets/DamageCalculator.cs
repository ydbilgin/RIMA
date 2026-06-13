using UnityEngine;

namespace RIMA.Balance
{
    public struct DamageCalculationResult
    {
        public int finalDamage;
        public float statMultiplier;
        public float cappedIdentityBuildMultiplier;
        public float cappedSituationalMultiplier;
        public float postureOverflowMultiplier;
        public int postureOverflowDamage;
    }

    public static class DamageCalculator
    {
        public const float IdentityBuildCap = 3f;
        public const float SituationalCap = 2f;

        public static DamageCalculationResult Calculate(DamagePacket packet, ClassStatRuntime attackerStats)
        {
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

            float rawDamage = packet.baseDamage
                              * statMultiplier
                              * cappedIdentityBuild
                              * cappedSituational
                              * debugMult;

            int finalDamage = Mathf.Max(1, Mathf.RoundToInt(rawDamage));

            // Placeholder: route this to a Posture/Break system later.
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
                postureOverflowMultiplier = overflowMultiplier,
                postureOverflowDamage = postureOverflowDamage
            };
        }
    }
}
