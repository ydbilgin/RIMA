using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Shadowblade Skill 11 — Preparation (Advanced)
    /// Tüm Rogue CD'leri sıfırla, 90s CD.
    /// Evasion aktifken → Preparation CD 60s.
    /// </summary>
    public class Preparation : SkillBase
    {
        private Shadowblade_SkillController ctrl;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Preparation";
            cooldown = 90f;
            resourceCost = 0;
            ctrl = GetComponentInParent<Shadowblade_SkillController>();
        }

        protected override void Execute()
        {
            if (ctrl != null && ctrl.EvasionActive)
                cooldown = 60f;
            else
                cooldown = 90f;

            var skills = GetComponentsInParent<SkillBase>();
            foreach (var s in skills)
            {
                if (s == this) continue;
                s.ForceReady();
            }
        }
    }
}
