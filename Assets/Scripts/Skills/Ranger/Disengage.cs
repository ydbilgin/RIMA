using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Ranger Skill 6 — Disengage
    /// Geri zıpla (dash-back) + önündeki düşmanlara Chill uygular.
    /// Mesafe uzaksa Focus dolar; yakına zorunlu kalınınca bonus utility.
    /// </summary>
    public class Disengage : SkillBase
    {
        [Header("Disengage")]
        [SerializeField] private float leapForce       = 14f;
        [SerializeField] private float chillRadius     = 3.5f;
        [SerializeField] private float chillDuration   = 2.5f;
        protected override void Awake()
        {
            base.Awake();
            skillName    = "Disengage";
            cooldown     = 10f;
            resourceCost = 30;
        }

        protected override void Execute() => StartCoroutine(DisengageRoutine());

        private IEnumerator DisengageRoutine()
        {
            // Leap backward
            Vector2 back = player != null ? -player.FacingDirection : Vector2.down;
            var rb = GetComponentInParent<Rigidbody2D>();
            if (rb != null) rb.AddForce(back * leapForce, ForceMode2D.Impulse);

            // Chill enemies in front arc immediately
            var hits = Physics2D.OverlapCircleAll(transform.position, chillRadius);
            foreach (var h in hits)
            {
                if (h.CompareTag("Player")) continue;
                h.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Chill, chillDuration);
            }

            yield return null;
        }
    }
}
