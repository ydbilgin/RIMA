using System.Collections;
using UnityEngine;

namespace RIMA
{
    public class DeepWound : SkillBase
    {
        [Header("Deep Wound")]
        [SerializeField] private float range = 2f;
        [SerializeField] private int hitDamage = 20;
        [SerializeField] private int bleedDamagePerTick = 8;
        [SerializeField] private float bleedDuration = 8f;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Deep Wound";
            cooldown = 8f;
            rageCost = 0;
        }

        // FIX-2 wiring: read-only mirror of Execute's no-op gate (`target == null`). Same
        // OverlapCircleAll(range, "Enemy") + player-exclusion + non-dead Health filter, no side
        // effects — rejects the cast before cost/cooldown when no enemy is in range.
        protected override bool CanExecute()
        {
            return HasEnemyInRange();
        }

        private bool HasEnemyInRange()
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, range, LayerMask.GetMask("Enemy"));
            foreach (var h in hits)
            {
                if (h.gameObject == player.gameObject) continue;
                var hp = h.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;
                return true;
            }
            return false;
        }

        protected override void Execute()
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, range, LayerMask.GetMask("Enemy"));
            float best = float.MaxValue;
            Health target = null;
            foreach (var h in hits)
            {
                if (h.gameObject == player.gameObject) continue;
                var hp = h.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;
                float d = Vector2.Distance(transform.position, h.transform.position);
                if (d < best) { best = d; target = hp; }
            }
            if (target == null) return;

            SkillRuntime.DealDamage(target, hitDamage);
            Debug.Log($"[Damage] {hitDamage} -> {target.name} (deepwound)"); // F3 telemetry: bypasses Player-tagged DealDamage log
            target.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Bleed, bleedDuration);
            rage?.AddRage(35);
            StartCoroutine(BleedTick(target, bleedDuration));
        }

        private IEnumerator BleedTick(Health hp, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                yield return new WaitForSeconds(1f);
                elapsed += 1f;
                if (hp == null || hp.IsDead) yield break;
                SkillRuntime.DealDamage(hp, bleedDamagePerTick, this);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.6f, 0f, 0f, 0.3f);
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
