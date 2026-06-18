using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Mark pulse melee behavior for Ravager.
    /// LMB: Brutal Swing — slow 3-hit heavy axe combo with Fury buildup.
    ///       Heavier and less disciplined than Warblade; more body lurch.
    ///       Each hit applies Sundered mark stacks on enemies.
    /// RMB: Blood Pact — self-wound HP trade into Fury/damage window.
    ///       Spends HP to gain Fury burst and temporary damage amplification.
    /// </summary>
    public class MarkPulseBehavior : BasicAttackBehaviorBase
    {
        private float bloodPactTimer;
        private float bloodPactDamageWindow;

        // Internal fury counter: builds per hit, consumed by Blood Pact payoff
        private int fury;
        private const int FuryPerHit = 15;
        private const int FuryThreshold = 60;
        private const float FuryEmpowerMult = 1.5f;

        // Blood Pact constants
        private const float BloodPactHPCostPercent = 0.08f; // 8% current HP
        private const int BloodPactFuryGain = 40;
        private const float BloodPactDamageWindowDuration = 4f;
        private const float BloodPactDamageBuff = 1.35f;

        public override void OnUpdate(PlayerAttack owner, BasicAttackProfile profile, float dt)
        {
            base.OnUpdate(owner, profile, dt);

            if (bloodPactTimer > 0f)
                bloodPactTimer -= dt;

            if (bloodPactDamageWindow > 0f)
                bloodPactDamageWindow -= dt;
        }

        public override void OnLMBInput(PlayerAttack owner, BasicAttackProfile profile,
            bool pressed, bool released)
        {
            if (!pressed) return;

            if (owner.CommitTimer > 0f)
                owner.BufferedAttack = true;
            else
                ExecuteBrutalSwing(owner, profile);
        }

        public override void OnRMBInput(PlayerAttack owner, BasicAttackProfile profile,
            bool pressed, bool released)
        {
            if (!pressed) return;
            owner.Controller.FaceCombatTarget();
            ExecuteBloodPact(owner, profile);
        }

        private void ExecuteBrutalSwing(PlayerAttack owner, BasicAttackProfile profile)
        {
            owner.Controller.FaceCombatTarget();

            int step = owner.ComboStep;

            owner.CommitTimer = profile.commitment;
            owner.ComboTimer = profile.comboWindow;
            owner.ComboStep = (owner.ComboStep + 1) % profile.comboLength;

            owner.RaiseComboStep(step);
            owner.EmitSlashArc(owner.Controller.FacingDirection, step);

            // Fury buildup
            fury += FuryPerHit;
            bool furyEmpowered = fury >= FuryThreshold;
            if (furyEmpowered)
                fury = 0;

            // Blood Pact damage window active?
            float pactMult = bloodPactDamageWindow > 0f ? BloodPactDamageBuff : 1f;
            float furyMult = furyEmpowered ? FuryEmpowerMult : 1f;
            float totalMult = pactMult * furyMult;

            // Apply melee hit with combined multipliers
            ApplyMeleeHitWithMarks(owner, profile, step, totalMult, furyEmpowered);

            if (furyEmpowered)
            {
                CameraShake.Instance?.Shake(0.20f, 0.14f);
                LightPulse.Emit(new Color(0.85f, 0.15f, 0.08f), 1.6f, 0.10f);
            }
            else
            {
                LightPulse.Emit(new Color(0.75f, 0.28f, 0.12f), 0.8f, 0.06f);
            }
        }

        /// <summary>
        /// Extended melee hit that also applies Sundered mark stacks to enemies.
        /// Ravager's identity: bigger arcs, mark application, rewards staying in pocket.
        /// </summary>
        private void ApplyMeleeHitWithMarks(PlayerAttack owner, BasicAttackProfile profile,
            int step, float totalMult, bool furyEmpowered)
        {
            RageSystem rage = owner.Rage;
            int damageIndex = Mathf.Min(step, profile.comboDamage.Length - 1);
            int dmg = Mathf.RoundToInt(
                (profile.comboDamage[damageIndex] + owner.baseDamage)
                * owner.outgoingDamageMultiplier * totalMult);
            float range = profile.hitRange[Mathf.Min(step, profile.hitRange.Length - 1)];
            float radius = profile.hitRadius[Mathf.Min(step, profile.hitRadius.Length - 1)];

            Vector2 facing = owner.Controller.FacingDirection;
            Vector2 hitCenter = (Vector2)owner.transform.position + facing * range;

            foreach (var col in Physics2D.OverlapCircleAll(hitCenter, radius))
            {
                if (col.gameObject == owner.gameObject) continue;
                var hp = col.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;

                var status = col.GetComponent<StatusEffectSystem>();
                int finalDmg = status != null
                    ? Mathf.RoundToInt(dmg * status.damageMultiplierIncoming)
                    : dmg;

                hp.TakeDamage(finalDmg);
                rage?.OnHitEnemy();
                RIMA.Combat.HitPauseDriver.Instance?.TriggerPause(0.03f); // single timeScale owner — obsolete HitStop here caused a dual-owner stuck-0 (frozen game) race in combat
                DamagePopup.Show(col.transform.position, finalDmg);

                var kb = col.GetComponent<KnockbackReceiver>();
                if (!hp.IsDead && kb != null)
                {
                    var impulse = profile.GetImpulseForStep(step, facing);
                    impulse.force *= 1.15f;
                    kb.ApplyImpulse(impulse);
                }

                // Ravager identity: apply Sundered mark stacks after impulse routing so
                // this hit only knocks down targets that were already Broken/Sundered.
                int markStacks = furyEmpowered ? 3 : 1;
                SkillRuntime.State(hp)?.Apply(
                    SkillStateTracker.Sundered, 6f, markStacks, 5);

                // Final combo hit: heavy knockback + camera shake
                if (step == profile.comboLength - 1)
                {
                    CameraShake.Instance?.Shake(0.22f, 0.15f);
                }

            }
        }

        private void ExecuteBloodPact(PlayerAttack owner, BasicAttackProfile profile)
        {
            if (bloodPactTimer > 0f) return;

            // Blood Pact: HP sacrifice for Fury and damage window
            var health = owner.GetComponent<Health>();
            if (health == null || health.IsDead) return;

            int hpCost = Mathf.Max(1, Mathf.RoundToInt(health.CurrentHP * BloodPactHPCostPercent));

            // Must have enough HP (don't let player kill themselves)
            if (health.CurrentHP <= hpCost + 1) return;

            health.TakeDamage(hpCost);

            // Gain Fury
            fury = Mathf.Min(fury + BloodPactFuryGain, FuryThreshold);

            // Activate damage window
            bloodPactDamageWindow = BloodPactDamageWindowDuration;

            // Also feed Rage if available
            owner.Rage?.OnHitEnemy();

            bloodPactTimer = profile.rmbCooldown;

            // Blood VFX
            LightPulse.Emit(new Color(0.75f, 0.08f, 0.08f), 1.8f, 0.12f);
            CameraShake.Instance?.Shake(0.12f, 0.08f);

            // Visual indicator: spawn blood pull circle
            SkillRuntime.SpawnCircleVisual(
                owner.transform.position,
                new Color(0.65f, 0.05f, 0.05f, 0.55f), 1.2f, 0.18f, "BloodPact_Runtime");
        }
    }
}
