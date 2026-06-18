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

            // FIX (off-map): the teleport destination must be walkable, otherwise Blink strands the
            // player in the void. Mirrors PlayerController.IsReachableDashDestination (IsDashableWorld).
            // If the destination is not dashable, walk back along the blink ray to the furthest
            // walkable point; if none is found, cancel the teleport (stay at start). Permissive when
            // no WalkabilityMap exists (legacy behavior preserved).
            var walkMap = RIMA.Environment.WalkabilityMap.Instance;
            if (walkMap != null && !walkMap.IsDashableWorld(end))
            {
                bool snapped = false;
                const int steps = 12;
                for (int i = steps - 1; i >= 1; i--)
                {
                    Vector2 candidate = Vector2.Lerp(start, end, i / (float)steps);
                    if (walkMap.IsDashableWorld(candidate)) { end = candidate; snapped = true; break; }
                }
                if (!snapped) end = start; // no walkable point along the path → cancel teleport
            }

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
