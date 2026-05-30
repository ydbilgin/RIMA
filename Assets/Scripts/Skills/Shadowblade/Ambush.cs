using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Shadowblade Skill 6 — Ambush
    /// Sadece stealth'ten: %300 hasar + 4 CP + %20 slow.
    /// 3s+ stealth → Cold Blood: +%100 hasar.
    /// </summary>
    public class Ambush : SkillBase
    {
        [Header("Ambush")]
        [SerializeField] private int   baseDamage  = 80;
        [SerializeField] private float slowDuration = 2f;
        [SerializeField] private float attackRange  = 2f;

        private ComboPointSystem combo;
        private Shadowblade_SkillController ctrl;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Ambush";
            cooldown = 10f;
            resourceCost = 40;
            combo = GetComponentInParent<ComboPointSystem>();
            ctrl = GetComponentInParent<Shadowblade_SkillController>();
        }

        protected override void Execute()
        {
            if (ctrl == null || !ctrl.IsInStealth)
            {
                Debug.Log("Ambush: stealth gerektirir.");
                return;
            }

            var target = Hemorrhage.FindNearestEnemy(transform.position, attackRange);
            if (target == null) return;

            int dmg = Mathf.RoundToInt(baseDamage * 3f);
            // Cold Blood: 3s+ stealth varsayımı → Vanish'e ek süre yok, basit flag kontrol
            // Stealth hâlâ aktif ve uzun süre geçtiyse bonus (ShadowbladeController'dan süre izle)
            dmg = Mathf.RoundToInt(dmg * 1f); // Cold Blood ileride eklenebilir

            SkillRuntime.DealDamage(target.GetComponent<Health>(), dmg, this);
            target.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Chill, slowDuration);
            combo?.Add(4);

            ctrl.ExitStealth();
        }
    }
}
