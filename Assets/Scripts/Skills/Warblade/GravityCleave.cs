using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Warblade — Gravity Cleave
    /// Kılıcı yere çarpar: 4m çapında düşmanları çeker + %140 hasar + 0.8s slow.
    /// Chain: Iron Charge sonrası → çekilenler 1.5s stun + Rage+15.
    /// </summary>
    public class GravityCleave : SkillBase
    {
        [Header("Gravity Cleave")]
        [SerializeField] private float radius = 4f;
        [SerializeField] private int baseDamage = 55;
        [SerializeField] private float pullForce = 14f;
        [SerializeField] private float slowDuration = 0.8f;
        [SerializeField] private int rageOnUse = 10;

        private IronCharge ironCharge;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Gravity Cleave";
            cooldown = 9f;
            rageCost = 25;
            ironCharge = GetComponentInParent<IronCharge>();
        }

        protected override void Execute()
        {
            bool chained = ironCharge != null && ironCharge.CooldownPercent > 0.85f;

            var hits = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Default"));
            foreach (var h in hits)
            {
                if (h.gameObject == player.gameObject) continue;
                var hp = h.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;

                hp.TakeDamage(baseDamage);

                // Çek
                var rb = h.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 pullDir = ((Vector2)transform.position - rb.position).normalized;
                    rb.AddForce(pullDir * pullForce, ForceMode2D.Impulse);
                }

                // Slow veya stun
                var status = h.GetComponent<StatusEffectSystem>();
                if (status != null)
                {
                    if (chained)
                        status.ApplyEffect(StatusEffectType.Stunned, 1.5f, 1);
                    else
                        status.ApplyEffect(StatusEffectType.Chill, slowDuration, 1);
                }
            }

            rage?.AddRage(rageOnUse);
            if (chained) rage?.AddRage(15);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.4f, 0.2f, 0.8f, 0.25f);
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
