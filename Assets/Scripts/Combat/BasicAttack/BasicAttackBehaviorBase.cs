using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Shared timer logic for all basic attack behaviors.
    /// Handles commit window, combo timeout, and input buffering.
    /// </summary>
    public abstract class BasicAttackBehaviorBase : IBasicAttackBehavior
    {
        public virtual void OnUpdate(PlayerAttack owner, BasicAttackProfile profile, float dt)
        {
            if (owner.CommitTimer > 0f)
            {
                owner.CommitTimer -= dt;
                if (owner.CommitTimer <= 0f && owner.BufferedAttack)
                {
                    owner.BufferedAttack = false;
                    ExecuteBufferedLMB(owner, profile);
                }
            }

            if (owner.ComboTimer > 0f)
            {
                owner.ComboTimer -= dt;
                if (owner.ComboTimer <= 0f)
                    owner.ComboStep = 0;
            }
        }

        protected virtual void ExecuteBufferedLMB(PlayerAttack owner, BasicAttackProfile profile)
        {
            OnLMBInput(owner, profile, true, false);
        }

        public abstract void OnLMBInput(PlayerAttack owner, BasicAttackProfile profile,
            bool pressed, bool released);

        public abstract void OnRMBInput(PlayerAttack owner, BasicAttackProfile profile,
            bool pressed, bool released);

        /// <summary>
        /// Shared melee hit application: Physics2D overlap, damage, knockback, VFX.
        /// Used by MeleeChainBehavior, VeilStrikeBehavior, and future melee behaviors.
        /// </summary>
        protected void ApplyMeleeHit(PlayerAttack owner, BasicAttackProfile profile,
            int step, float chainMult = 1f)
        {
            RageSystem rage = owner.Rage;
            int damageIndex = Mathf.Min(step, profile.comboDamage.Length - 1);
            int dmg = Mathf.RoundToInt(
                (profile.comboDamage[damageIndex] + owner.baseDamage)
                * owner.outgoingDamageMultiplier * chainMult);
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
                HitStop.Instance?.FreezeLight();
                LightPulse.Emit(new Color(0.4f, 0.7f, 1f), 1.5f, 0.10f);
                DamagePopup.Show(col.transform.position, finalDmg);

                if (step == profile.comboLength - 1)
                    CameraShake.Instance?.Shake(0.18f, 0.12f);

                var kb = col.GetComponent<KnockbackReceiver>();
                if (kb != null)
                {
                    float kbForce = profile.knockbackForce != null && profile.knockbackForce.Length > 0
                        ? profile.knockbackForce[Mathf.Min(step, profile.knockbackForce.Length - 1)]
                        : 0f;
                    float kbDur = profile.knockbackDuration != null && profile.knockbackDuration.Length > 0
                        ? profile.knockbackDuration[Mathf.Min(step, profile.knockbackDuration.Length - 1)]
                        : 0f;
                    kb.ApplyKnockback(facing, kbForce, kbDur);
                }
            }
        }
    }
}
