using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// ShardWalker'ın attığı parça mermisi.
    /// Rigidbody2D + CircleCollider2D gerektirir.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private int damage = 8;
        [SerializeField] private float lifetime = 3f;
        private bool initialized;

        private void Start()
        {
            if (!initialized)
                Init(damage, lifetime);
        }

        public void Init(int dmg, float life)
        {
            damage = dmg;
            lifetime = life;
            initialized = true;

            var rb = GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.gravityScale = 0f;

            var col = GetComponent<CircleCollider2D>();
            if (col != null)
                col.isTrigger = true;

            if (Application.isPlaying)
                Destroy(gameObject, lifetime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            Health ph = other.GetComponent<Health>();
            if (ph != null) ph.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
