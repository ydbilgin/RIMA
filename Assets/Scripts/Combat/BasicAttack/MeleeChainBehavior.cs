using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Melee combo chain behavior for Warblade and generic melee classes.
    /// LMB: multi-step combo with commitment, buffering, and knockback.
    /// RMB: Rage Outlet AoE burst (spends Rage).
    ///
    /// A3 — Startup deferral: ExecuteCombo registers FaceCombatTarget + timers + RaiseComboStep
    /// immediately (so windup anim fires on input frame), then defers EmitSlashArc +
    /// ApplyMeleeHit + finisher trigger until _startupTimer expires via OnUpdate.
    /// attackStartup &lt;= 0 falls back to immediate hit (legacy / graceful zero-value).
    /// </summary>
    public class MeleeChainBehavior : BasicAttackBehaviorBase
    {
        private float rageOutletTimer;

        // ── A3 startup pending state ──────────────────────────────────────────────
        private bool  _hitPending;
        private int   _pendingStep;
        private float _pendingChainMult;
        private float _startupTimer;
        // Store the owner reference so OnUpdate can invoke without re-discovery.
        // (behavior instance is owned per PlayerAttack; ref is safe to hold.)
        private PlayerAttack _pendingOwner;
        private BasicAttackProfile _pendingProfile;
        // ─────────────────────────────────────────────────────────────────────────

        public override void OnUpdate(PlayerAttack owner, BasicAttackProfile profile, float dt)
        {
            base.OnUpdate(owner, profile, dt);

            if (rageOutletTimer > 0f)
                rageOutletTimer -= dt;

            // A3: tick startup timer; resolve hit when it expires
            if (_hitPending)
            {
                _startupTimer -= dt;
                if (_startupTimer <= 0f)
                    ResolvePendingHit();
            }
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

            // Windup anim starts immediately on input frame.
            owner.RaiseComboStep(isChained ? step + profile.comboLength : step);

            float chainMult = owner.FlowTracker != null ? owner.FlowTracker.ConsumeBasicChain() : 1f;

            if (profile.attackStartup > 0f)
            {
                // A3: defer the active hit until startup window elapses. If a prior hit is
                // still pending (profile timing where startup outlives the commit gate),
                // resolve it now so the buffered combo step doesn't silently drop it.
                if (_hitPending) ResolvePendingHit();
                _hitPending       = true;
                _pendingStep      = step;
                _pendingChainMult = chainMult;
                _startupTimer     = profile.attackStartup;
                _pendingOwner     = owner;
                _pendingProfile   = profile;
            }
            else
            {
                // Immediate (legacy / attackStartup == 0).
                // Warblade basic swing uses the ember SkillVfx.MeleeArc sprite only; the old blue
                // SlashArcVFX LineRenderer was removed here to avoid a doubled, color-clashing arc.
                SkillVfx.MeleeArc(GetHitCenter(owner, profile, step), owner.Controller.FacingDirection, VfxElement.Physical);
                ApplyMeleeHit(owner, profile, step, chainMult);
                TriggerWarbladeFinisher(profile, step, owner);
            }
        }

        private void ResolvePendingHit()
        {
            _hitPending = false;
            if (_pendingOwner == null || _pendingProfile == null) return;

            // STRIKE FRAME (fires at t = attackStartup, aligned to the swing's strike fraction
            // via PlayerAttack.CurrentStrikeFraction). Warblade basic swing uses the ember
            // SkillVfx.MeleeArc sprite only; the old blue SlashArcVFX LineRenderer was removed
            // here to avoid a doubled, color-clashing arc.
            // VFXRouter handles downstream hit/kill bursts via CombatEventBus in ApplyMeleeHit.
            SkillVfx.MeleeArc(GetHitCenter(_pendingOwner, _pendingProfile, _pendingStep), _pendingOwner.Controller.FacingDirection, VfxElement.Physical);
            ApplyMeleeHit(_pendingOwner, _pendingProfile, _pendingStep, _pendingChainMult);
            TriggerWarbladeFinisher(_pendingProfile, _pendingStep, _pendingOwner);

            _pendingOwner   = null;
            _pendingProfile = null;
        }

        private static Vector2 GetHitCenter(PlayerAttack owner, BasicAttackProfile profile, int step)
        {
            Vector2 facing = owner.Controller.FacingDirection;
            return (Vector2)owner.transform.position + facing * profile.GetHitRangeForStep(step);
        }

        private static void TriggerWarbladeFinisher(BasicAttackProfile profile, int step, PlayerAttack owner)
        {
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
                SkillRuntime.DealDamage(health, RIMA.Balance.DamagePacket.Create(
                    profile.rmbDamage,
                    profile.rmbDamageType,
                    profile.rmbSourceType,
                    owner.gameObject,
                    health.gameObject,
                    "basic_rmb",
                    elementTag: profile.rmbElementTag));
                health.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Stunned, 0.45f);
            }

            rageOutletTimer = owner.ApplyAttackSpeed(profile.rmbCooldown);
            LightPulse.Emit(new Color(0.85f, 0.36f, 0.2f), 1f, 0.08f);
        }
    }
}
