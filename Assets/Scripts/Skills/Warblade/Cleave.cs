using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Warblade Skill 2 — Cleave
    /// Etrafta dairesel bir saldırı. Tüm yakın düşmanlara hasar ver.
    /// Rage miktarına göre hasar artar (Enrage bonus).
    /// </summary>
    public class Cleave : SkillBase
    {
        [Header("Cleave")]
        [SerializeField] private float radius = 2.2f;
        [SerializeField] private int baseDamage = 18;
        [SerializeField] private float enrageBonusPerRage = 0.3f; // %30 bonus per 100 rage
        [SerializeField] private float knockbackForce = 5f;
        [SerializeField] private int ragePerHit = 8;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Cleave";
            cooldown = 5f;
            rageCost = 20;
        }

        protected override void Execute()
        {
            float rageBonus = rage != null ? rage.RagePercent * enrageBonusPerRage : 0f;
            int finalDamage = Mathf.RoundToInt(baseDamage * (1f + rageBonus));

            var hits = Physics2D.CircleCastAll(
                transform.position, radius, Vector2.zero, 0f,
                LayerMask.GetMask("Default")
            );

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject == player.gameObject) continue;
                var hp = hit.collider.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;

                hp.TakeDamage(finalDamage);
                rage?.AddRage(ragePerHit);

                var enemyRb = hit.collider.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    Vector2 dir = ((Vector2)hit.collider.transform.position - (Vector2)transform.position).normalized;
                    enemyRb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.3f, 0f, 0.3f);
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
