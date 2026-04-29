using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// SeamCrawler Elite — homing projectile saldırısı.
    /// Normal varyanttaki trail YOK, bu component eklenir.
    /// BaseMobBehavior.OnAttackReady'e subscribe olur.
    /// </summary>
    [RequireComponent(typeof(BaseMobBehavior))]
    public class SeamCrawler_Homing : MonoBehaviour
    {
        [Header("Homing Projectile")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private int   damage         = 18;
        [SerializeField] private float launchSpeed    = 5f;
        [SerializeField] private float homingStrength = 4f;
        [SerializeField] private float lifetime       = 4f;
        [SerializeField] private float attackCooldown = 3f;

        private BaseMobBehavior mob;

        private void Awake()
        {
            mob = GetComponent<BaseMobBehavior>();
            mob.attackCooldown = attackCooldown;
            mob.attackRange    = 8f;
            mob.OnAttackReady += FireHoming;
        }

        private void OnDestroy() => mob.OnAttackReady -= FireHoming;

        private void FireHoming(Vector2 dir)
        {
            if (projectilePrefab == null || mob.Player == null) return;

            var go = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            var rb = go.GetComponent<Rigidbody2D>();
            if (rb != null) { rb.gravityScale = 0f; rb.linearVelocity = dir * launchSpeed; }

            var projectile = go.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.Init(damage, lifetime);
            }
            else
            {
                var dmg = go.AddComponent<EnemyProjectileDamage>();
                dmg.Init(damage, lifetime);
            }

            var homing = go.AddComponent<HomingBehavior>();
            homing.Init(mob.Player, homingStrength);
        }
    }

    internal class HomingBehavior : MonoBehaviour
    {
        private Transform target;
        private float     strength;
        private Rigidbody2D rb;

        public void Init(Transform t, float s)
        {
            target = t; strength = s;
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (target == null || rb == null) return;
            Vector2 dir = ((Vector2)target.position - rb.position).normalized;
            rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, dir * rb.linearVelocity.magnitude, strength * Time.fixedDeltaTime);
            // Also steer
            rb.linearVelocity += dir * strength * Time.fixedDeltaTime;
            if (rb.linearVelocity.magnitude > 8f)
                rb.linearVelocity = rb.linearVelocity.normalized * 8f;
        }

    }
}
