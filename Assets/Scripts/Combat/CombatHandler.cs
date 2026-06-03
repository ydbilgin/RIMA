using UnityEngine;
using RIMA;
using RIMA.Combat;

public class CombatHandler : MonoBehaviour
{
    private float _lastCommitBeatTime = -999f;
    private int _comboCount = 0;

    [SerializeField] private float commitBeatICD = 1.2f; // Karar #122 T1

    [Header("A2 — Commit-beat BREAK")]
    [Tooltip("Broken state duration (s) applied by the commit-beat to the struck enemy.")]
    [SerializeField] private float brokenDuration = 6f;

    private PlayerAttack _attack;

    private void Awake()
    {
        _attack = GetComponent<PlayerAttack>();
    }

    /// <summary>Karar #122 T1 - called by Beat3CommitTrigger on impact frame.</summary>
    public void OnCommitBeat()
    {
        // ICD check
        if (Time.time - _lastCommitBeatTime < commitBeatICD) return;
        _lastCommitBeatTime = Time.time;

        _comboCount++;

        // A2: resolve the enemy this 3rd-hit finisher actually struck, mirroring the basic-attack
        // finisher reach (BasicAttackBehaviorBase.ApplyMeleeHit), so the BREAK lands where the
        // swing connects. Layer "Enemy" per project rule (basic attack uses no mask + Health filter;
        // here the explicit mask is cheaper and matches IronCharge/SunderMark/DeathBlow).
        GameObject struck = ResolveStruckEnemy();
        if (struck != null)
        {
            // Apply Broken (1 stack, max 3) — 3 stacks auto-escalate to Sundered (A1b).
            SkillRuntime.State(struck)?.Apply(SkillStateTracker.Broken, brokenDuration, 1, 3);

            // Drive the A1 visual tell (VFXRouter routes status_<id> off OnStatusApplied).
            CombatEventBus.PublishStatusApplied(new StatusEvent
            {
                worldPos = struck.transform.position,
                target = struck,
                statusId = SkillStateTracker.Broken,
                duration = brokenDuration
            });
        }

        // A6: drive the finisher juice — HitPauseDriver + ScreenShakeDriver + VFXRouter
        // all subscribe to CombatEventBus.OnCommitBeat. The live Beat3 path never published it
        // (only VFXBusDemo did), so finisher hitstop/shake never fired in real combat.
        CombatEventBus.PublishCommitBeat(new CommitBeatEvent
        {
            worldPos = struck != null ? struck.transform.position : transform.position,
            attacker = gameObject,
            beatIndex = 3,
            target = struck
        });
    }

    /// <summary>
    /// Picks the nearest live enemy inside the finisher hit-circle. Mirrors the basic-attack
    /// finisher: hitCenter = pos + facing * range, OverlapCircle(hitCenter, radius).
    /// </summary>
    private GameObject ResolveStruckEnemy()
    {
        if (_attack == null) return null;
        if (!_attack.TryGetFinisherReach(out Vector2 facing, out float range, out float radius))
            return null;

        Vector2 hitCenter = (Vector2)transform.position + facing * range;
        var hits = Physics2D.OverlapCircleAll(hitCenter, radius, LayerMask.GetMask("Enemy"));
        float best = float.MaxValue;
        GameObject bestGo = null;
        foreach (var col in hits)
        {
            if (col == null || col.gameObject == gameObject) continue;
            var hp = col.GetComponent<Health>();
            if (hp == null || hp.IsDead) continue;
            float d = ((Vector2)col.transform.position - hitCenter).sqrMagnitude;
            if (d < best) { best = d; bestGo = hp.gameObject; }
        }
        return bestGo;
    }
}
