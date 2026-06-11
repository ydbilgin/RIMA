using System;
using UnityEngine;
using RIMA.Combat;
using RIMA.Balance;

namespace RIMA
{
    /// <summary>
    /// Oyuncu skill'lerinin fırlattığı mermi. Düşmanlara çarpar.
    /// Rigidbody2D + CircleCollider2D (IsTrigger) gerektirir.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class PlayerProjectile : MonoBehaviour
    {
        private int damage;
        private float lifetime = 4f;
        private bool piercing;
        private bool applyBurning;
        private float burnDuration;
        private bool applyChill;
        private float chillDuration;
        private bool applyPoison;
        private float poisonDuration;
        private int knockbackForce;
        private GameObject attacker;
        private string hitElement = "projectile";
        private bool hasDamagePacket;
        private DamagePacket damagePacket;
        private Action<Collider2D> onHit;

        public void Init(
            Vector2 velocity, int dmg,
            float life = 4f,
            bool piercing = false,
            bool applyBurning = false, float burnDuration = 0f,
            bool applyChill = false,   float chillDuration = 0f,
            bool applyPoison = false,  float poisonDuration = 0f,
            int knockback = 0,
            GameObject attacker = null,
            string element = "projectile")
        {
            damage           = dmg;
            lifetime         = life;
            this.piercing    = piercing;
            this.applyBurning = applyBurning;
            this.burnDuration = burnDuration;
            this.applyChill   = applyChill;
            this.chillDuration = chillDuration;
            this.applyPoison  = applyPoison;
            this.poisonDuration = poisonDuration;
            knockbackForce    = knockback;
            this.attacker     = attacker;
            hitElement        = element;

            var rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.linearVelocity = velocity;

            var col = GetComponent<CircleCollider2D>();
            col.isTrigger = true;

            Destroy(gameObject, lifetime);
        }

        public void SetOnHit(Action<Collider2D> callback)
        {
            onHit = callback;
        }

        public void SetDamagePacket(DamagePacket packet)
        {
            damagePacket = packet;
            hasDamagePacket = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) return;

            var hp = other.GetComponent<Health>();
            if (hp == null || hp.IsDead) return;

            Vector2 hitDirection = GetComponent<Rigidbody2D>() != null
                ? GetComponent<Rigidbody2D>().linearVelocity.normalized
                : ((Vector2)other.transform.position - (Vector2)transform.position).normalized;
            int finalDamage;
            if (hasDamagePacket)
            {
                damagePacket.target = other.gameObject;
                if (damagePacket.attacker == null)
                    damagePacket.attacker = attacker != null ? attacker : gameObject;
                finalDamage = SkillRuntime.DealDamage(hp, damagePacket, false, damagePacket.attacker, hitDirection);
            }
            else
            {
                finalDamage = damage;
                hp.TakeDamage(finalDamage);
                SkillRuntime.PublishSkillHit(hp, finalDamage, attacker != null ? attacker : gameObject, hitDirection, hitElement);
            }
            onHit?.Invoke(other);

            var status = other.GetComponent<StatusEffectSystem>();
            if (status != null)
            {
                if (applyBurning) status.ApplyEffect(StatusEffectType.Burning, burnDuration);
                if (applyChill)   status.ApplyEffect(StatusEffectType.Chill,   chillDuration);
                if (applyPoison)  status.ApplyEffect(StatusEffectType.Poison,  poisonDuration);
            }

            if (knockbackForce > 0)
            {
                var rb = other.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 dir = (other.transform.position - transform.position).normalized;
                    rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
                }
            }

            if (!piercing) Destroy(gameObject);
        }
    }
}
