using UnityEngine;

namespace RIMA
{
    public abstract class RangerSkillBase : SkillBase
    {
        protected FocusSystem focus;

        protected override void Awake()
        {
            base.Awake();
            focus = GetComponentInParent<FocusSystem>();
        }

        protected int FocusDamage(int damage)
        {
            return focus != null && focus.IsDamageBoosted
                ? Mathf.RoundToInt(damage * 1.25f)
                : damage;
        }

        protected void Mark(Health target, float duration = 8f, int stacks = 1)
        {
            SkillRuntime.State(target)?.Apply(SkillStateTracker.RangerMarked, duration, stacks, 5);
        }

        protected void Trap(Health target, float duration = 4f)
        {
            SkillRuntime.State(target)?.Apply(SkillStateTracker.Trapped, duration, 1, 1);
        }
    }
}
