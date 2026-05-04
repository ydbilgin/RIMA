using System.Collections;
using UnityEngine;
using RIMA.Environment;

namespace RIMA
{
    /// <summary>
    /// ShardWalker — uzaktan parça atan kristal düşman.
    /// EnemyAI yerine kullanılır (aynı GameObject'te ikisi olmasın).
    /// Gerekli bileşenler: Rigidbody2D, CircleCollider2D, Health, EnemyAnimator.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Health))]
    public class ShardWalker : MonoBehaviour
    {
        [Header("Detection")]
        [SerializeField] private float detectionRange = 9f;
        [SerializeField] private float attackRange    = 5f;
        [SerializeField] private float minChaseRange  = 2f; // bu mesafenin altına inmez

        [Header("Movement")]
        [SerializeField] private float chaseSpeed = 2.5f;

        [Header("Projectile")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private int        projectileDamage = 8;
        [SerializeField] private float      projectileLife   = 3f;
        [SerializeField] private float      projectileSpeed  = 7f;
        [SerializeField] private float      attackCooldown   = 1.8f;
        [SerializeField] private int        projectileCount  = 1; // tek atış (upgrade: 3 yönlü)

        [Header("Telegraph")]
        [SerializeField] private bool  useTelegraph      = true;
        [SerializeField] private float telegraphDuration = 0.35f;
        [SerializeField] private float telegraphWidth    = 0.45f;

        [Header("Death Burst")]
        [SerializeField] private int   burstCount = 6;
        [SerializeField] private float burstSpeed = 5f;

        private enum State { Idle, Chase, Attack }
        private State state = State.Idle;

        private Rigidbody2D rb;
        private Health health;
        private Transform player;
        private float attackTimer;
        private bool windingUp;

        private void Awake()
        {
            rb     = GetComponent<Rigidbody2D>();
            rb.gravityScale  = 0f;
            rb.freezeRotation = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.useFullKinematicContacts = true;
            GroundBlobShadow.Ensure(transform, new Vector2(0.9f, 0.30f), 0.28f);
            health = GetComponent<Health>();
            health.OnDeath.AddListener(OnDeath);
        }

        private void Start()
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        private void Update()
        {
            if (health.IsDead || player == null) return;

            attackTimer -= Time.deltaTime;
            float dist = Vector2.Distance(transform.position, player.position);

            if (dist <= attackRange)
                state = State.Attack;
            else if (dist <= detectionRange)
                state = State.Chase;
            else
                state = State.Idle;
        }

        private void FixedUpdate()
        {
            if (health.IsDead)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }
            if (player == null) return;

            Vector2 toPlayer = (Vector2)player.position - (Vector2)transform.position;
            float dist = toPlayer.magnitude;

            if (state == State.Chase)
            {
                // Çok yaklaşırsa durur
                if (dist > minChaseRange)
                    rb.linearVelocity = toPlayer.normalized * chaseSpeed;
                else
                    rb.linearVelocity = Vector2.zero;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;

                if (state == State.Attack && attackTimer <= 0f)
                {
                    attackTimer = attackCooldown;
                    StartCoroutine(FireProjectileRoutine(toPlayer.normalized));
                }
            }
        }

        private IEnumerator FireProjectileRoutine(Vector2 dir)
        {
            if (windingUp) yield break;
            windingUp = true;

            if (useTelegraph && telegraphDuration > 0f)
            {
                if (projectileCount > 1)
                    EnemyTelegraph.SpawnCone(transform.position, dir, attackRange, 40f, telegraphDuration);
                else
                    EnemyTelegraph.SpawnLine(transform.position, dir, attackRange, telegraphWidth, telegraphDuration);

                yield return new WaitForSeconds(telegraphDuration);
            }

            FireProjectile(dir);
            windingUp = false;
        }

        private void FireProjectile(Vector2 dir)
        {
            if (projectilePrefab == null) return;

            if (projectileCount == 1)
            {
                SpawnShard(dir);
            }
            else
            {
                // Çok yönlü atış (projectileCount = 3 → -20°, 0°, +20°)
                float spread = 20f;
                float step   = projectileCount > 1 ? spread * 2 / (projectileCount - 1) : 0f;
                for (int i = 0; i < projectileCount; i++)
                {
                    float angle = -spread + step * i;
                    Vector2 rotDir = RotateVector(dir, angle);
                    SpawnShard(rotDir);
                }
            }
        }

        private void SpawnShard(Vector2 dir, float speed = -1f)
        {
            GameObject shard = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            var projectile = shard.GetComponent<Projectile>();
            if (projectile != null)
                projectile.Init(projectileDamage, projectileLife);

            var shardRb = shard.GetComponent<Rigidbody2D>();
            if (shardRb != null)
            {
                shardRb.gravityScale = 0f;
                shardRb.linearVelocity = dir * (speed < 0f ? projectileSpeed : speed);
            }
        }

        private void OnDeath()
        {
            rb.linearVelocity = Vector2.zero;
            var col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
            this.enabled = false;

            StartCoroutine(DeathBurst());
        }

        private IEnumerator DeathBurst()
        {
            if (projectilePrefab == null) yield break;

            for (int i = 0; i < burstCount; i++)
            {
                float angle = i * (360f / burstCount);
                Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                SpawnShard(dir, burstSpeed);
                yield return null;
            }
        }

        private static Vector2 RotateVector(Vector2 v, float degrees)
        {
            float rad = degrees * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad), sin = Mathf.Sin(rad);
            return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, minChaseRange);
        }
    }
}
