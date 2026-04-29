using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Ranger Skill 11 — Flare
    /// Önüne aydınlık işaret fişeği fırlatır. Bölgedeki tüm düşmanlara
    /// Weakened uygular + 4s boyunca alanda kalan düşman hasar alır.
    /// </summary>
    public class Flare : SkillBase
    {
        [Header("Flare")]
        [SerializeField] private float projectileSpeed  = 12f;
        [SerializeField] private float flareRange       = 6f;
        [SerializeField] private float areaRadius       = 3.5f;
        [SerializeField] private float weakenDuration   = 4f;
        [SerializeField] private int   tickDamage       = 8;
        [SerializeField] private float flareDuration    = 4f;
        [SerializeField] private float tickInterval     = 0.5f;

        protected override void Awake()
        {
            base.Awake();
            skillName    = "Flare";
            cooldown     = 16f;
            resourceCost = 35;
        }

        protected override void Execute() => StartCoroutine(FlareRoutine());

        private IEnumerator FlareRoutine()
        {
            Vector2 origin = transform.position;
            Vector2 dir    = player != null ? player.FacingDirection : Vector2.right;
            Vector2 landPos = origin + dir * flareRange;

            // Immediately weaken enemies in blast zone
            var hits = Physics2D.OverlapCircleAll(landPos, areaRadius);
            foreach (var h in hits)
            {
                if (h.CompareTag("Player")) continue;
                h.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Weakened, weakenDuration);
            }

            // Tick damage while flare burns
            float elapsed = 0f;
            while (elapsed < flareDuration)
            {
                yield return new WaitForSeconds(tickInterval);
                elapsed += tickInterval;

                hits = Physics2D.OverlapCircleAll(landPos, areaRadius);
                foreach (var h in hits)
                {
                    if (h.CompareTag("Player")) continue;
                    h.GetComponent<Health>()?.TakeDamage(tickDamage);
                }
            }
        }
    }
}
