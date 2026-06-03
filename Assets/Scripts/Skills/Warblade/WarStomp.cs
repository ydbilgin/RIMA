using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Warblade — War Stomp
    /// 3m AoE: knockback + 2s stun. Rage+25.
    /// Chain: Bladestorm sırasında → stun +1s (Faz 2).
    /// </summary>
    public class WarStomp : SkillBase
    {
        [Header("War Stomp")]
        [SerializeField] private float radius = 3f;
        [SerializeField] private int damage = 30;
        [SerializeField] private float knockbackForce = 10f;
        [SerializeField] private float stunDuration = 2f;
        [SerializeField] private int rageOnUse = 25;

        // Echo (Feature B): War Stomp is a clean melee radial AoE cast at SkillOrigin — a Shadow Echo
        // can slam it down ON the target cluster.
        public override bool SupportsEchoOrigin => true;

        protected override void Awake()
        {
            base.Awake();
            skillName = "War Stomp";
            cooldown = 10f;
            rageCost = 0;
        }

        protected override void Execute()
        {
            Vector3 origin = SkillOrigin;
            var hits = Physics2D.OverlapCircleAll(origin, radius, LayerMask.GetMask("Enemy"));
            foreach (var h in hits)
            {
                if (player != null && h.gameObject == player.gameObject) continue;
                var hp = h.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;

                SkillRuntime.DealDamage(hp, damage, this);

                // Radial knockback
                var rb = h.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 dir = ((Vector2)h.transform.position - (Vector2)origin).normalized;
                    rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
                    StartCoroutine(StunRigidbody(rb, stunDuration));
                }

                // StatusEffect stun (düşman AI kontrol eder)
                var status = h.GetComponent<StatusEffectSystem>();
                status?.ApplyEffect(StatusEffectType.Stunned, stunDuration, 1);
            }

            rage?.AddRage(rageOnUse);

            // A4: open the follow-up window read by Ironclad Momentum (was Ironclad Momentum reading
            // `warStomp.CooldownPercent > 0.7f`). WarStomp CD=10s, old proxy gave ~3s — match that.
            ChainWindowTracker.For(this)?.OpenWindow(ChainWindowTracker.WarStompFollowup, 3f);
        }

        private IEnumerator StunRigidbody(Rigidbody2D rb, float duration)
        {
            if (rb == null) yield break;
            var constraints = rb.constraints;
            rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            yield return new WaitForSeconds(duration);
            if (rb != null) rb.constraints = constraints;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.6f, 0f, 0.25f);
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
