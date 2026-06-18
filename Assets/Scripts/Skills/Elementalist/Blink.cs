using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Elementalist Skill 4 — Blink
    /// 6m ışınlanma, geçilen düşmanlara hasar.
    /// Düşmanın içinden → 0.5s stun.
    /// Sonraki spell +%20 hasar.
    /// </summary>
    public class Blink : SkillBase
    {
        [Header("Blink")]
        [SerializeField] private float blinkDistance = 6f;
        [SerializeField] private int   damage        = 20;

        private Rigidbody2D rb;
        private bool nextSpellEmpowered;

        public bool ConsumeEmpowerment() { bool v = nextSpellEmpowered; nextSpellEmpowered = false; return v; }
        public float EmpowerMultiplier => 1.2f;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Blink";
            cooldown = 6f;
            resourceCost = 30;
            rb = GetComponentInParent<Rigidbody2D>();
        }

        protected override void Execute()
        {
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            Vector2 start = rb != null ? rb.position : (Vector2)transform.position;
            Vector2 end   = start + dir * blinkDistance;

            // Engeli kontrol et
            var hit = Physics2D.Raycast(start, dir, blinkDistance, LayerMask.GetMask("Wall"));
            if (hit.collider != null) end = hit.point - dir * 0.2f;

            // Geçilen yoldaki düşmanları vur
            var hits = Physics2D.CircleCastAll(start, 0.4f, dir,
                Vector2.Distance(start, end), LayerMask.GetMask("Enemy"));

            foreach (var h in hits)
            {
                if (h.collider.CompareTag("Player")) continue;
                var hp = h.collider.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;
                SkillRuntime.DealDamage(hp, damage, this);

                var status = h.collider.GetComponent<StatusEffectSystem>();
                status?.ApplyEffect(StatusEffectType.Stunned, 0.5f);
            }

            // Arcane blink-out spark at the start point, blink-in spark at the destination.
            SkillVfx.ImpactBurst((Vector3)start, VfxElement.Arcane);

            if (rb != null) rb.position = end;
            else transform.position = end;

            SkillVfx.ImpactBurst((Vector3)end, VfxElement.Arcane);

            nextSpellEmpowered = true;
        }
    }
}
