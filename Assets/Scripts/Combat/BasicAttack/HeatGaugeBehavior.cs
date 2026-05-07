using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Heat gauge firing behavior for Gunslinger.
    /// LMB: Dual Fire — fast 2-beat alternating pistol cadence with Heat buildup.
    ///       Can be held for auto-fire. Every Nth shot (heatThreshold) triggers an
    ///       empowered overheated shot.
    /// RMB: Hip Shot — lateral side-step + precise single shot. Spends Heat for bonus damage.
    /// </summary>
    public class HeatGaugeBehavior : BasicAttackBehaviorBase
    {
        private int heatStep;
        private float hipShotTimer;

        // Internal heat counter: builds per shot, consumed by Hip Shot / overheated shot.
        private int heat;
        private const int HeatPerShot = 12;
        private const int HeatThreshold = 60; // 5 shots to overheat
        private const float OverheatDamageMultiplier = 2.2f;
        private const int HipShotHeatCost = 30;
        private const float HipShotDamageMultiplier = 1.5f;

        public override void OnUpdate(PlayerAttack owner, BasicAttackProfile profile, float dt)
        {
            base.OnUpdate(owner, profile, dt);

            if (hipShotTimer > 0f)
                hipShotTimer -= dt;
        }

        public override void OnLMBInput(PlayerAttack owner, BasicAttackProfile profile,
            bool pressed, bool released)
        {
            if (!pressed) return;

            if (owner.CommitTimer > 0f)
                owner.BufferedAttack = true;
            else
                ExecuteDualFire(owner, profile);
        }

        public override void OnRMBInput(PlayerAttack owner, BasicAttackProfile profile,
            bool pressed, bool released)
        {
            if (!pressed) return;
            owner.Controller.FaceCombatTarget();
            ExecuteHipShot(owner, profile);
        }

        protected override void ExecuteBufferedLMB(PlayerAttack owner, BasicAttackProfile profile)
        {
            ExecuteDualFire(owner, profile);
        }

        private void ExecuteDualFire(PlayerAttack owner, BasicAttackProfile profile)
        {
            if (owner.CommitTimer > 0f) return;
            owner.Controller.FaceCombatTarget();

            owner.CommitTimer = profile.projectileCooldown;
            owner.ComboTimer = 0f;
            owner.ComboStep = 0;

            heat += HeatPerShot;
            bool overheated = heat >= HeatThreshold;
            if (overheated)
                heat = 0;

            Vector2 dir = owner.Controller.FacingDirection.sqrMagnitude > 0.01f
                ? owner.Controller.FacingDirection.normalized
                : Vector2.right;

            // Alternate pistol stance for visual variety
            int stance = heatStep % 2;
            heatStep++;

            int baseDmg = profile.projectileDamage;
            int damage = overheated
                ? Mathf.RoundToInt(baseDmg * OverheatDamageMultiplier)
                : baseDmg;
            float scale = overheated ? 0.48f : 0.30f;

            // Slight offset based on stance (left/right pistol)
            Vector2 perpendicular = new Vector2(-dir.y, dir.x) * (stance == 0 ? 0.15f : -0.15f);
            Vector2 spawnPos = (Vector2)owner.transform.position + dir * 0.35f + perpendicular;

            var projectile = SkillRuntime.SpawnProjectile(
                spawnPos, dir, profile.projectileSpeed, damage,
                overheated
                    ? new Color(1f, 0.55f, 0.12f, 0.95f)   // orange-hot overheated
                    : new Color(1f, 0.85f, 0.28f, 0.90f),   // warm gold normal
                scale, 2.5f, "DualFire_Runtime");

            projectile.SetOnHit(hit =>
            {
                // Heat shots always build some resource
                owner.GetComponent<FocusSystem>()?.Add(overheated ? 10 : 4);
                if (overheated)
                {
                    var status = hit.GetComponent<StatusEffectSystem>();
                    status?.ApplyEffect(StatusEffectType.Burning, 2f);
                    CameraShake.Instance?.Shake(0.14f, 0.10f);
                }
            });

            Color pulseColor = overheated
                ? new Color(1f, 0.45f, 0.1f)
                : new Color(1f, 0.9f, 0.4f);
            LightPulse.Emit(pulseColor, overheated ? 1.2f : 0.6f, 0.05f);
        }

        private void ExecuteHipShot(PlayerAttack owner, BasicAttackProfile profile)
        {
            if (hipShotTimer > 0f) return;

            bool hasHeat = heat >= HipShotHeatCost;
            if (hasHeat)
                heat -= HipShotHeatCost;

            Vector2 dir = owner.Controller.FacingDirection.sqrMagnitude > 0.01f
                ? owner.Controller.FacingDirection.normalized
                : Vector2.right;

            int damage = hasHeat
                ? Mathf.RoundToInt(profile.projectileDamage * HipShotDamageMultiplier)
                : profile.projectileDamage;

            var projectile = SkillRuntime.SpawnProjectile(
                (Vector2)owner.transform.position + dir * 0.35f,
                dir, profile.projectileSpeed * 1.2f, damage,
                new Color(1f, 0.72f, 0.18f, 0.95f), 0.36f, 2.8f, "HipShot_Runtime");

            projectile.SetOnHit(hit =>
            {
                owner.GetComponent<FocusSystem>()?.Add(6);
                if (hasHeat)
                {
                    SkillRuntime.State(hit.GetComponent<Health>())
                        ?.Apply(SkillStateTracker.Sundered, 4f, 1, 3);
                }
            });

            hipShotTimer = profile.rmbCooldown;
            LightPulse.Emit(new Color(1f, 0.65f, 0.15f), 1f, 0.07f);
        }
    }
}
