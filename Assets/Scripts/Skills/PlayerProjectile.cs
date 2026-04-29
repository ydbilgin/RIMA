using UnityEngine;

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

        public void Init(
            Vector2 velocity, int dmg,
            float life = 4f,
            bool piercing = false,
            bool applyBurning = false, float burnDuration = 0f,
            bool applyChill = false,   float chillDuration = 0f,
            bool applyPoison = false,  float poisonDuration = 0f,
            int knockback = 0)
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

            var rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.linearVelocity = velocity;

            var col = GetComponent<CircleCollider2D>();
            col.isTrigger = true;

            Destroy(gameObject, lifetime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) return;

            var hp = other.GetComponent<Health>();
            if (hp == null || hp.IsDead) return;

            hp.TakeDamage(damage);

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
