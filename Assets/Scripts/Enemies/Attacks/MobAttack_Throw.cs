using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Fırlatma saldırısı — ShardWalker, VoidThrall kullanır.
    /// BaseMobBehavior.OnAttackReady'e subscribe olur.
    /// </summary>
    [RequireComponent(typeof(BaseMobBehavior))]
    public class MobAttack_Throw : MonoBehaviour
    {
        [Header("Projectile")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private int   damage           = 12;
        [SerializeField] private float projectileSpeed  = 7f;
        [SerializeField] private float projectileLife   = 5f;
        [SerializeField] private float attackCooldown   = 1.8f;

        [Header("Multi-throw (1 = single)")]
        [SerializeField] private int   throwCount       = 1;
        [SerializeField] private float spreadAngle      = 20f;

        [Header("Telegraph")]
        [SerializeField] private bool  useTelegraph      = true;
        [SerializeField] private float telegraphDuration = 0.35f;
        [SerializeField] private float telegraphWidth    = 0.45f;

        private BaseMobBehavior mob;
        private IMobAffix[] affixes;
        private bool windingUp;

        private void Awake()
        {
            mob = GetComponent<BaseMobBehavior>();
            mob.attackCooldown = attackCooldown;
            mob.OnAttackReady += Fire;
        }

        private void Start() => affixes = GetComponents<IMobAffix>();

        private void OnDestroy() => mob.OnAttackReady -= Fire;

        private void Fire(Vector2 dir)
        {
            if (projectilePrefab == null || windingUp) return;

            StartCoroutine(FireRoutine(dir));
        }

        private IEnumerator FireRoutine(Vector2 dir)
        {
            windingUp = true;

            if (useTelegraph && telegraphDuration > 0f)
            {
                float length = Mathf.Max(1f, mob.attackRange);
                if (throwCount > 1)
                    EnemyTelegraph.SpawnCone(transform.position, dir, length, spreadAngle * 2f, telegraphDuration);
                else
                    EnemyTelegraph.SpawnLine(transform.position, dir, length, telegraphWidth, telegraphDuration);

                yield return new WaitForSeconds(telegraphDuration);
            }

            if (throwCount == 1)
            {
                SpawnProjectile(dir);
            }
            else
            {
                float step = throwCount > 1 ? spreadAngle * 2f / (throwCount - 1) : 0f;
                for (int i = 0; i < throwCount; i++)
                {
                    float a = -spreadAngle + step * i;
                    SpawnProjectile(Rotate(dir, a));
                }
            }

            windingUp = false;
        }

        private void SpawnProjectile(Vector2 dir)
        {
            var go = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            var rb = go.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0f;
                rb.linearVelocity = dir * projectileSpeed;
            }

            // Affix'ler fırlanan mermiye efekt ekleyebilir
            if (affixes != null)
                foreach (var a in affixes) a.OnProjectileSpawned(go);

            var projectile = go.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.Init(damage, projectileLife);
            }
            else
            {
                var dmg = go.AddComponent<EnemyProjectileDamage>();
                dmg.Init(damage, projectileLife);
            }
        }

        private static Vector2 Rotate(Vector2 v, float deg)
        {
            float r = deg * Mathf.Deg2Rad;
            float c = Mathf.Cos(r), s = Mathf.Sin(r);
            return new Vector2(v.x * c - v.y * s, v.x * s + v.y * c);
        }
    }

    /// <summary>Düşman mermisinin oyuncuya hasar verme bileşeni.</summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyProjectileDamage : MonoBehaviour
    {
        private int damage;

        public void Init(int dmg, float lifetime = 5f)
        {
            damage = dmg;

            var col = GetComponent<Collider2D>();
            if (col != null)
                col.isTrigger = true;

            if (Application.isPlaying)
                Destroy(gameObject, lifetime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            other.GetComponent<Health>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
