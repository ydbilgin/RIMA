using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Shot cadence behavior for Ranger.
    /// LMB: tap = quick arrow, hold = charged shot with mark.
    /// RMB: Tactical Roll + immediate arrow release.
    /// </summary>
    public class ShotCadenceBehavior : BasicAttackBehaviorBase
    {
        public override void OnLMBInput(PlayerAttack owner, BasicAttackProfile profile,
            bool pressed, bool released)
        {
            if (pressed)
                owner.RangerAttackStartedAt = Time.time;

            if (released && owner.RangerAttackStartedAt >= 0f)
            {
                bool charged = Time.time - owner.RangerAttackStartedAt >= profile.chargeThreshold;
                ExecuteArrow(owner, profile, charged);
                owner.RangerAttackStartedAt = -1f;
            }
        }

        public override void OnRMBInput(PlayerAttack owner, BasicAttackProfile profile,
            bool pressed, bool released)
        {
            if (!pressed) return;
            owner.Controller.FaceCombatTarget();

            var rangerCtrl = owner.GetComponent<Ranger_SkillController>();
            if (rangerCtrl != null && rangerCtrl.TryTacticalRoll(owner.Controller.FacingDirection))
                ExecuteArrow(owner, profile, false);
        }

        private void ExecuteArrow(PlayerAttack owner, BasicAttackProfile profile, bool charged)
        {
            if (owner.CommitTimer > 0f) return;
            owner.Controller.FaceCombatTarget();
            owner.CommitTimer = profile.projectileCooldown;
            owner.ComboTimer = 0f;
            owner.ComboStep = 0;

            Vector2 dir = owner.Controller.FacingDirection.sqrMagnitude > 0.01f
                ? owner.Controller.FacingDirection.normalized
                : Vector2.right;
            int damage = charged
                ? Mathf.RoundToInt(profile.projectileDamage * 1.8f)
                : profile.projectileDamage;
            float scale = charged ? 0.42f : 0.28f;

            var projectile = SkillRuntime.SpawnProjectile(
                (Vector2)owner.transform.position + dir * 0.35f,
                dir, profile.projectileSpeed, damage,
                new Color(0.4f, 0.95f, 0.42f, 0.95f), scale, 3f, "RiftArrow_Runtime", owner.gameObject);
            projectile.SetDamagePacket(RIMA.Balance.DamagePacket.Create(
                damage,
                profile.lmbDamageType,
                profile.lmbSourceType,
                owner.gameObject,
                null,
                charged ? "basic_lmb_charged" : "basic_lmb",
                elementTag: profile.lmbElementTag));

            projectile.SetOnHit(hit =>
            {
                owner.GetComponent<FocusSystem>()?.Add(charged ? 8 : 4);
                var health = hit.GetComponent<Health>();
                if (charged && health != null)
                    SkillRuntime.State(health)?.Apply(
                        SkillStateTracker.RangerMarked, 8f, 2, 5);
            });
        }
    }
}
