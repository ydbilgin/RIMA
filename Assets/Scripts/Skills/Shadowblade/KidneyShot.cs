using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Shadowblade Skill 5 — Kidney Shot
    /// 5 CP: 4s stun, CP'ye göre uzar.
    /// </summary>
    public class KidneyShot : SkillBase
    {
        [Header("Kidney Shot")]
        [SerializeField] private float baseStunDuration = 4f;
        [SerializeField] private float stunPerCP        = 0.4f;
        [SerializeField] private float attackRange      = 1.5f;

        private ComboPointSystem combo;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Kidney Shot";
            cooldown = 12f;
            resourceCost = 0;
            combo = GetComponentInParent<ComboPointSystem>();
        }

        protected override void Execute()
        {
            if (combo == null) return;
            int pts = combo.PeekAndSpend();
            if (pts < 1) return;

            var target = Hemorrhage.FindNearestEnemy(transform.position, attackRange);
            if (target == null) return;

            float stunDur = baseStunDuration + pts * stunPerCP;
            target.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Stunned, stunDur);
        }
    }
}
