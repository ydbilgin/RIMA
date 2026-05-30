using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Elementalist Skill 10 — Arcane Surge (Advanced)
    /// 8s: mana regen +%100, cast süresi -%50.
    /// Blink sonrası → Blink konumunda patlama + sonraki Meteor/FrozenOrb mana maliyeti 0.
    /// </summary>
    public class ArcaneSurge : SkillBase
    {
        [Header("Arcane Surge")]
        [SerializeField] private float duration = 8f;
        [SerializeField] private float blinkExplosionRadius = 2.5f;
        [SerializeField] private int   blinkExplosionDamage = 40;

        private Elementalist_SkillController ctrl;
        private ManaSystem mana;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Arcane Surge";
            cooldown = 20f;
            resourceCost = 0;
            ctrl = GetComponentInParent<Elementalist_SkillController>();
            mana = GetComponentInParent<ManaSystem>();
        }

        protected override void Execute()
        {
            ctrl?.ActivateArcaneSurge(duration);

            // Blink gücü kontrolü — Blink.ConsumeEmpowerment() çağrılmışsa patlama
            var blink = GetComponentInParent<Blink>();
            if (blink != null && blink.ConsumeEmpowerment())
            {
                var hits = Physics2D.OverlapCircleAll(transform.position, blinkExplosionRadius);
                foreach (var h in hits)
                {
                    if (h.CompareTag("Player")) continue;
                    SkillRuntime.DealDamage(h.GetComponent<Health>(), blinkExplosionDamage, this);
                }
            }
        }
    }
}
