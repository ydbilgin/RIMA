using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Shadowblade Skill 3 — Rupture
    /// 5 CP finisher: bleed + hasar, CP'ye göre süre uzar.
    /// Zaten bleed varsa → birikmiş hasar anında patlar.
    /// </summary>
    public class Rupture : SkillBase
    {
        [Header("Rupture")]
        [SerializeField] private int   baseDamage   = 50;
        [SerializeField] private float baseBleedDur = 6f;
        [SerializeField] private float attackRange  = 1.8f;

        private ComboPointSystem combo;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Rupture";
            cooldown = 0f; // Finisher: CD finisher kullanıldıktan sonra CP0'dan doluyor
            resourceCost = 0;
            combo = GetComponentInParent<ComboPointSystem>();
        }

        public override bool Equals(object other) => base.Equals(other);

        protected override void Execute()
        {
            if (combo == null) return;
            int pts = combo.PeekAndSpend();
            if (pts < 1) return;

            var target = Hemorrhage.FindNearestEnemy(transform.position, attackRange);
            if (target == null) return;

            var hp = target.GetComponent<Health>();
            if (hp == null || hp.IsDead) return;

            int dmg = Mathf.RoundToInt(baseDamage * (1f + pts * 0.4f));
            float bleedDur = baseBleedDur + pts * 1.5f;

            var status = target.GetComponent<StatusEffectSystem>();
            bool hasBleed = status != null && status.GetStacks(StatusEffectType.Poison) > 0;

            if (hasBleed)
            {
                // Birikmiş bleed hasarı anında patla
                int bleedStacks = status.GetStacks(StatusEffectType.Poison);
                dmg += bleedStacks * 15;
            }

            SkillRuntime.DealDamage(hp, dmg, this);
            status?.ApplyEffect(StatusEffectType.Poison, bleedDur);
        }
    }
}
