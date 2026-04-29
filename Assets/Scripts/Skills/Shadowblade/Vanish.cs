using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Shadowblade Skill 12 — Vanish (Master)
    /// Savaşta anlık stealth 3s, 50s CD.
    /// Vanish sonrası Ambush → Cold Blood garantili.
    /// </summary>
    public class Vanish : SkillBase
    {
        [Header("Vanish")]
        [SerializeField] private float stealthDuration = 3f;

        private Shadowblade_SkillController ctrl;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Vanish";
            cooldown = 50f;
            resourceCost = 0;
            ctrl = GetComponentInParent<Shadowblade_SkillController>();
        }

        protected override void Execute()
        {
            ctrl?.EnterStealth(stealthDuration);
        }
    }
}
