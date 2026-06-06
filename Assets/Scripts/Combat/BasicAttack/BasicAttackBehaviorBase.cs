using UnityEngine;
using RIMA.Combat;
using RIMA.Audio;

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
                bool isFinisher = step == profile.comboLength - 1;
                CombatEventBus.PublishHit(new HitEvent
                {
                    worldPos = col.transform.position,
                    attacker = owner.gameObject,
                    target = col.gameObject,
                    damage = finalDmg,
                    element = "physical",
                    isCrit = isFinisher,
                    hitDirection = facing
                });
                // T2: light vs heavy swing SFX (finisher = heavy tier)
                if (isFinisher)
                {
                    AudioManager.Play(Sfx.SwingHeavy);
                    AudioManager.Play(Sfx.Finisher);
                }
                else
                {
                    AudioManager.Play(Sfx.SwingLight);
                }

                if (hp.IsDead)
                {
                    CombatEventBus.PublishKill(new KillEvent
                    {
                        worldPos = col.transform.position,
                        killer = owner.gameObject,
                        victim = col.gameObject,
                        mobFamily = col.tag
                    });
                    AudioManager.Play(Sfx.EnemyDeath);
                }
                rage?.OnHitEnemy();

                if (!hp.IsDead && col.TryGetComponent(out KnockbackReceiver kb))
                {
                    kb.ApplyImpulse(profile.GetImpulseForStep(step, facing));
                }
            }
        }
    }
}
