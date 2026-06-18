using UnityEngine;
using System.Collections.Generic;

namespace RIMA
{
    /// <summary>
    /// Warblade Skill 1 — Iron Charge
    /// Bakış yönünde hızlı fırla, yoldaki tüm düşmanlara hasar ver + knockback uygula.
    /// Rage kazanır (her isabet).
    /// </summary>
    public class IronCharge : SkillBase
    {
        [Header("Iron Charge")]
        [SerializeField] private float chargeSpeed = 22f;
        [SerializeField] private float chargeDuration = 0.18f;
        [SerializeField] private float hitRadius = 0.6f;
        [SerializeField] private int damage = 25;
        [SerializeField] private float knockbackForce = 8f;
        [SerializeField] private int ragePerHit = 15;

        private Rigidbody2D rb;
        private bool charging;
        private float chargeTimer;
        private Vector2 chargeDir;
        private readonly HashSet<Health> hitTargets = new();

        protected override void Awake()
        {
            base.Awake();
            rb = GetComponentInParent<Rigidbody2D>();
            skillName = "Iron Charge";
            cooldown = 3f;
            rageCost = 0;
        }

        protected override void Execute()
        {
            chargeDir = player.FacingDirection;
            charging = true;
            chargeTimer = chargeDuration;
            hitTargets.Clear();

            // A4: open the follow-up chain window read by Crippling Blow / Gravity Cleave
            // (replaces their old `ironCharge.CooldownPercent > 0.85f` proxy).
            ChainWindowTracker.For(this)?.OpenWindow(ChainWindowTracker.IronChargeNextHit);
        }

        private void FixedUpdate()
        {
            if (!charging) return;

            chargeTimer -= Time.fixedDeltaTime;
            rb.linearVelocity = chargeDir * chargeSpeed;

            // Yoldaki düşmanları bul ve vur
            var hits = Physics2D.CircleCastAll(
                rb.position, hitRadius, chargeDir, 0.05f,
                LayerMask.GetMask("Enemy")
            );

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject == rb.gameObject) continue;
                var hp = hit.collider.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;
                if (!hitTargets.Add(hp)) continue;

                SkillRuntime.DealDamage(hp, damage, true,
                    player != null ? player.gameObject : null, chargeDir, element: "physical");
                rage?.OnHitEnemy();
                rage?.AddRage(ragePerHit);
                hp.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Stunned, 1.5f);

                var enemyRb = hit.collider.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                    enemyRb.AddForce(chargeDir * knockbackForce, ForceMode2D.Impulse);
            }

            if (chargeTimer <= 0f)
            {
                charging = false;
                hitTargets.Clear();
            }
        }
    }
}
