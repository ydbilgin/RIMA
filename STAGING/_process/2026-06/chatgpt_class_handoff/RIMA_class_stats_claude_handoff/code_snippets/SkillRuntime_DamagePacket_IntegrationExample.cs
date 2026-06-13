using UnityEngine;
using RIMA.Balance;

public partial class SkillRuntime : MonoBehaviour
{
    // Replace this with however your project stores current class stats.
    [SerializeField] private ClassStatRuntime currentClassStats;

    // New preferred path.
    private void DealDamage(Health targetHealth, DamagePacket packet)
    {
        if (targetHealth == null) return;

        var result = DamageCalculator.Calculate(packet, currentClassStats);

        // Health should only apply final damage. It should not know class stat math.
        targetHealth.TakeDamage(result.finalDamage);

        // Optional telemetry hook.
        // BalanceTelemetry.Instance?.RecordDamage(packet, result);

        // Optional future hook.
        // targetHealth.Posture?.Apply(result.postureOverflowDamage);
    }

    // Temporary compatibility path. Keep while migrating old hardcoded int calls.
    private void DealDamageLegacy(Health targetHealth, int baseDamage, GameObject attacker, GameObject target, string sourceId)
    {
        var packet = DamagePacket.Create(
            baseDamage,
            DamageType.Physical,
            DamageSourceType.Skill,
            attacker,
            target,
            sourceId
        );

        DealDamage(targetHealth, packet);
    }
}
