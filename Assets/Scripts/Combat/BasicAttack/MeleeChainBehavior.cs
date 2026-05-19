using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Melee combo chain behavior for Warblade and generic melee classes.
    /// LMB: multi-step combo with commitment, buffering, and knockback.
    /// RMB: Rage Outlet AoE burst (spends Rage).
    /// </summary>
    public class MeleeChainBehavior : BasicAttackBehaviorBase
    {
        private float rageOutletTimer;

        public override void OnUpdate(PlayerAttack owner, BasicAttackProfile profile, float dt)
        {
            base.OnUpdate(owner, profile, dt);

            if (rageOutletTimer > 0f)
                rageOutletTimer -= dt;
        }

        public override void OnLMBInput(PlayerAttack owner, BasicAttackProfile profile,
            bool pressed, bool released)
        {
            if (!pressed) return;

            if (owner.CommitTimer > 0f)
                owner.BufferedAttack = true;
            else
                ExecuteCombo(owner, profile);
        }

        public override void OnRMBInput(PlayerAttack owner, BasicAttackProfile profile,
            bool pressed, bool released)
        {
            if (!pressed) return;
            owner.Controller.FaceCombatTarget();
            ExecuteRageOutlet(owner, profile);
        }

        private void ExecuteCombo(PlayerAttack owner, BasicAttackProfile profile)
        {
            owner.Controller.FaceCombatTarget();

            int step = owner.ComboStep;
            bool isChained = owner.FlowTracker != null && owner.FlowTracker.IsChainedToBasic;

            owner.CommitTimer = profile.commitment;
            owner.ComboTimer = profile.comboWindow;
            owner.ComboStep = (owner.ComboStep + 1) % profile.comboLength;

            owner.RaiseComboStep(isChained ? step + profile.comboLength : step);
            owner.EmitSlashArc(owner.Controller.FacingDirection, step);

            float chainMult = owner.FlowTracker != null ? owner.FlowTracker.ConsumeBasicChain() : 1f;
            ApplyMeleeHit(owner, profile, step, chainMult);

            if (profile.classType == ClassType.Warblade && step == profile.comboLength - 1)
                CrossClassSkillManager.Instance?.TriggerWarbladeBeat3RoninQuickdraw(owner.transform.position);
        }

        private void ExecuteRageOutlet(PlayerAttack owner, BasicAttackProfile profile)
        {
            RageSystem rage = owner.Rage;
            if (rageOutletTimer > 0f || rage == null || !rage.TryConsume(profile.rmbCost))
                return;

            foreach (var health in SkillRuntime.EnemiesInCircle(
                owner.transform.position, profile.rmbRadius))
            {
                SkillRuntime.DealDamage(health, profile.rmbDamage);
                health.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Stunned, 0.45f);
            }

            rageOutletTimer = profile.rmbCooldown;
            LightPulse.Emit(new Color(0.85f, 0.36f, 0.2f), 1f, 0.08f);
        }
    }
}
