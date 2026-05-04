using UnityEngine;

namespace RIMA
{
    public abstract class ShadowbladeSkillBase : SkillBase
    {
        protected Shadowblade_SkillController shadow;
        protected EnergySystem energy;

        protected override void Awake()
        {
            base.Awake();
            shadow = GetComponentInParent<Shadowblade_SkillController>();
            energy = GetComponentInParent<EnergySystem>();
        }

        protected void Scar(Health target, float duration = 8f, int stacks = 1)
        {
            SkillRuntime.State(target)?.Apply(SkillStateTracker.RiftScar, duration, stacks, 5);
        }

        protected void ApplyBackstabMark(Health target, float duration = 8f)
        {
            SkillRuntime.State(target)?.Apply(SkillStateTracker.BackstabMarked, duration, 1, 1);
        }
    }
}
