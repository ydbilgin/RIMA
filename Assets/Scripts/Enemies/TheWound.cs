using System.Collections;
using UnityEngine;
using RIMA.Environment;

namespace RIMA
{
    /// <summary>
    /// The Wound — Özel mob (nadir). Yakındaki düşmanlara 2 HP/s iyileşme verir.
    /// Ölünce: flash + tüm düşmanlar %20 HP hasar alır.
    ///
    /// Tasarım: pasif destek, combat'a katılmaz — sadece arkada durur.
    /// Sprite gelince EnemyAnimator eklenir.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Health))]
    public class TheWound : MonoBehaviour
    {
        [Header("Aura")]
        [SerializeField] private float healRadius    = 6f;
        [SerializeField] private float healPerSecond = 2f;
        [SerializeField] private LayerMask enemyLayer;

        [Header("Death Burst")]
        [SerializeField] private float deathDamagePercent = 0.20f; // %20 HP
        [SerializeField] private float flashDuration       = 0.15f;

        [Header("Idle Wander")]
        [SerializeField] private float wanderSpeed  = 1f;
        [SerializeField] private float wanderRadius = 2f;

        private Rigidbody2D rb;
        private Health health;
        private SpriteRenderer sr;
        private bool isDead;
        private Vector2 wanderTarget;
        private float healTick;

        private void Awake()
        {
            rb     = GetComponent<Rigidbody2D>();
            health = GetComponent<Health>();
            sr     = GetComponentInChildren<SpriteRenderer>();

            rb.gravityScale  = 0f;
            rb.freezeRotation = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.useFullKinematicContacts = true;
            GroundBlobShadow.Ensure(transform, new Vector2(0.9f, 0.30f), 0.28f);

            health.OnDeath.AddListener(OnDeath);

            wanderTarget = transform.position;
        }

        private void Update()
        {
            if (isDead) return;

            HealNearbyEnemies();
            Wander();
        }

        private void HealNearbyEnemies()
        {
            healTick += Time.deltaTime;
            if (healTick < 1f) return;
            healTick = 0f;

            var cols = Physics2D.OverlapCircleAll(transform.position, healRadius, enemyLayer);
            foreach (var col in cols)
            {
                if (col.gameObject == gameObject) continue;
                col.GetComponent<Health>()?.Heal(Mathf.RoundToInt(healPerSecond));
            }
        }

        private void Wander()
        {
            if (Vector2.Distance(transform.position, wanderTarget) < 0.2f)
            {
                // Pick new wander point
                Vector2 offset = Random.insideUnitCircle * wanderRadius;
                wanderTarget = (Vector2)transform.position + offset;
            }

            Vector2 dir = (wanderTarget - (Vector2)transform.position).normalized;
            rb.linearVelocity = dir * wanderSpeed;
        }

        private void OnDeath()
        {
            isDead = true;
            rb.linearVelocity = Vector2.zero;
            StartCoroutine(DeathSequence());
        }

        private IEnumerator DeathSequence()
        {
            // Flash white
            if (sr != null)
            {
                Color orig = sr.color;
                sr.color = Color.white;
                yield return new WaitForSeconds(flashDuration);
                sr.color = orig;
            }

            // Damage all enemies on screen by 20% current HP
            var allEnemies = FindObjectsByType<Health>(FindObjectsSortMode.None);
            foreach (var h in allEnemies)
            {
                if (h.gameObject == gameObject) continue;
                if (h.CompareTag("Player")) continue; // skip player

                int dmg = Mathf.RoundToInt(h.CurrentHP * deathDamagePercent);
                if (dmg > 0) h.TakeDamage(dmg);
            }

            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.3f, 0.3f, 0.3f);
            Gizmos.DrawWireSphere(transform.position, healRadius);
        }
    }
}
