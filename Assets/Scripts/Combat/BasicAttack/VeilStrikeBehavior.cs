using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Fast melee behavior for Shadowblade.
    /// LMB: Veil Strike — fast single-hit with Rift Scar and Sever buildup.
    /// RMB: Veil Flicker — phase-step with afterimage.
    /// </summary>
    public class VeilStrikeBehavior : BasicAttackBehaviorBase
    {
        public override void OnLMBInput(PlayerAttack owner, BasicAttackProfile profile,
            bool pressed, bool released)
        {
            if (!pressed) return;

            if (owner.CommitTimer > 0f)
                owner.BufferedAttack = true;
            else
                ExecuteStrike(owner, profile);
        }

        public override void OnRMBInput(PlayerAttack owner, BasicAttackProfile profile,
            bool pressed, bool released)
        {
            if (!pressed) return;
            owner.Controller.FaceCombatTarget();

            owner.GetComponent<Shadowblade_SkillController>()?
                .TryVeilFlicker(owner.Controller.FacingDirection);
        }

        protected override void ExecuteBufferedLMB(PlayerAttack owner, BasicAttackProfile profile)
        {
            ExecuteStrike(owner, profile);
        }

        private void ExecuteStrike(PlayerAttack owner, BasicAttackProfile profile)
        {
            if (owner.CommitTimer > 0f) return;
            owner.Controller.FaceCombatTarget();
            owner.CommitTimer = profile.projectileCooldown; // reused as strike cooldown
            owner.ComboTimer = 0f;
            owner.ComboStep = 0;

            Vector2 facing = owner.Controller.FacingDirection.sqrMagnitude > 0.01f
                ? owner.Controller.FacingDirection.normalized
                : Vector2.right;

            float strikeRange = profile.hitRange != null && profile.hitRange.Length > 0
                ? profile.hitRange[0] : 1.35f;
            float strikeRadius = profile.hitRadius != null && profile.hitRadius.Length > 0
                ? profile.hitRadius[0] : 0.75f;

            Vector2 center = (Vector2)owner.transform.position + facing * strikeRange;
            foreach (var health in SkillRuntime.EnemiesInCircle(center, strikeRadius))
            {
                SkillRuntime.DealDamage(health, profile.projectileDamage);
                SkillRuntime.State(health)?.Apply(SkillStateTracker.RiftScar, 6f, 1, 5);
                SkillRuntime.State(health)?.Apply(SkillStateTracker.BackstabMarked, 6f, 1, 1);
                owner.GetComponent<Shadowblade_SkillController>()?.AddSever(8);
            }

            SkillRuntime.SpawnCircleVisual(center,
                new Color(0.55f, 0.22f, 0.82f, 0.48f), 0.8f, 0.12f, "VeilStrike_Runtime");
        }
    }
}
