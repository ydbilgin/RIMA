using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Shadowblade Skill 7 — Fan of Knives
    /// 360° AoE, tüm aktif debuffları tüm düşmanlara uygular.
    /// </summary>
    public class FanOfKnives : SkillBase
    {
        [Header("Fan of Knives")]
        [SerializeField] private int   damage = 30;
        [SerializeField] private float radius = 3.5f;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Fan of Knives";
            cooldown = 5f;
            resourceCost = 30;
        }

        protected override void Execute()
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, radius);
            StatusEffectSystem referenceStatus = null;

            // İlk düşmandan mevcut debuffları oku
            foreach (var h in hits)
            {
                if (h.CompareTag("Player")) continue;
                var s = h.GetComponent<StatusEffectSystem>();
                if (s != null) { referenceStatus = s; break; }
            }

            foreach (var h in hits)
            {
                if (h.CompareTag("Player")) continue;
                var hp = h.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;
                hp.TakeDamage(damage);

                // Debuffları yay
                if (referenceStatus != null)
                {
                    var status = h.GetComponent<StatusEffectSystem>();
                    if (status != null)
                    {
                        if (referenceStatus.GetStacks(StatusEffectType.Poison) > 0)
                            status.ApplyEffect(StatusEffectType.Poison, 6f);
                        if (referenceStatus.GetStacks(StatusEffectType.Weakened) > 0)
                            status.ApplyEffect(StatusEffectType.Weakened, 4f);
                    }
                }
            }
        }
    }
}
