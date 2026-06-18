using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Elementalist Skill 11 — Combustion (Advanced)
    /// 8s: tüm Fire spell instant cast, mana maliyet ×2.
    /// Fire State 5 stack → mana maliyet artışı yok.
    /// </summary>
    public class Combustion : SkillBase
    {
        [Header("Combustion")]
        [SerializeField] private float duration = 8f;

        private Elementalist_SkillController ctrl;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Combustion";
            cooldown = 25f;
            resourceCost = 0;
            ctrl = GetComponentInParent<Elementalist_SkillController>();
        }

        protected override void Execute()
        {
            // Fire buff activation flash on the caster.
            SkillVfx.CastFlash(player != null ? player.gameObject : gameObject, VfxElement.Fire);
            bool freeMana = ctrl != null && ctrl.FireState >= 5;
            ctrl?.ActivateCombustion(duration);
            if (freeMana) ctrl?.ConsumeFireState(5);
        }
    }
}
