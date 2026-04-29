using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Penitent Sovereign — Shackle Throw projectile.
    /// Oyuncuya isabet edince hasar + 2s slow (Chill × 3 = Frozen) uygular.
    /// PlaceholderSprite veya gerçek sprite ile kullanılabilir.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class BossChainProjectile : MonoBehaviour
    {
        private int   damage;
        private float speed;
        private float lifetime;
        private bool  hit;

        /// <summary>Spawn sonrası çağrılır.</summary>
        public void Init(int dmg, float spd, Vector2 direction, float life)
        {
            damage   = dmg;
            speed    = spd;
            lifetime = life;

            var rb = GetComponent<Rigidbody2D>();
            rb.gravityScale  = 0f;
            rb.freezeRotation = true;
            rb.linearVelocity = direction.normalized * speed;

            var col = GetComponent<CircleCollider2D>();
            col.isTrigger = true;
            if (col.radius < 0.1f) col.radius = 0.2f;

            Destroy(gameObject, lifetime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (hit || !other.CompareTag("Player")) return;
            hit = true;

            other.GetComponent<Health>()?.TakeDamage(damage);

            // Apply slow: Chill × 2, duration 2s
            var status = other.GetComponent<StatusEffectSystem>();
            if (status != null)
                status.ApplyEffect(StatusEffectType.Chill, 2f, 2);

            Destroy(gameObject);
        }
    }
}
