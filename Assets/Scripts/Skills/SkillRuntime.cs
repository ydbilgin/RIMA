using System.Collections.Generic;
using UnityEngine;
using RIMA.Combat;

namespace RIMA
{
    public static class SkillRuntime
    {
        public static SkillStateTracker State(Component component)
        {
            return component != null ? State(component.gameObject) : null;
        }

        public static SkillStateTracker State(GameObject target)
        {
            if (target == null) return null;
            if (!target.TryGetComponent(out SkillStateTracker tracker))
                tracker = target.AddComponent<SkillStateTracker>();
            return tracker;
        }

        public static Health FindNearestEnemy(Vector2 origin, float maxDistance = 20f)
        {
            Health result = null;
            float best = maxDistance * maxDistance;
            foreach (var enemy in Object.FindObjectsByType<EnemyAI>(FindObjectsSortMode.None))
            {
                if (enemy == null) continue;
                var health = enemy.GetComponent<Health>();
                if (health == null || health.IsDead) continue;

                float dist = ((Vector2)enemy.transform.position - origin).sqrMagnitude;
                if (dist < best)
                {
                    best = dist;
                    result = health;
                }
            }

            return result;
        }

        public static List<Health> EnemiesInCircle(Vector2 center, float radius)
        {
            var result = new List<Health>();
            var hits = Physics2D.OverlapCircleAll(center, radius);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i] == null || hits[i].CompareTag("Player")) continue;
                var health = hits[i].GetComponent<Health>();
                if (health != null && !health.IsDead && !result.Contains(health))
                    result.Add(health);
            }
            return result;
        }

        public static List<Health> EnemiesInLine(Vector2 origin, Vector2 direction, float length, float width)
        {
            direction = direction.sqrMagnitude > 0.001f ? direction.normalized : Vector2.right;
            Vector2 center = origin + direction * (length * 0.5f);
            float angle = Vector2.SignedAngle(Vector2.up, direction);
            var hits = Physics2D.BoxCastAll(center, new Vector2(width, length), angle, Vector2.zero);

            var result = new List<Health>();
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider == null || hits[i].collider.CompareTag("Player")) continue;
                var health = hits[i].collider.GetComponent<Health>();
                if (health != null && !health.IsDead && !result.Contains(health))
                    result.Add(health);
            }
            return result;
        }

        public static List<Health> EnemiesInCone(Vector2 origin, Vector2 direction, float radius, float halfAngle)
        {
            direction = direction.sqrMagnitude > 0.001f ? direction.normalized : Vector2.right;
            var result = new List<Health>();
            foreach (var health in EnemiesInCircle(origin, radius))
            {
                Vector2 toTarget = (Vector2)health.transform.position - origin;
                if (toTarget.sqrMagnitude < 0.001f || Vector2.Angle(direction, toTarget) <= halfAngle)
                    result.Add(health);
            }
            return result;
        }

        public static void DealDamage(Health health, int damage, bool popup = true)
        {
            DealDamage(health, damage, popup, null, null);
        }

        public static void DealDamage(Health health, int damage, Component source, bool popup = false)
        {
            GameObject attacker = source != null ? source.gameObject : null;
            Vector2? hitDirection = null;
            if (source != null && health != null)
                hitDirection = ((Vector2)health.transform.position - (Vector2)source.transform.position).normalized;

            DealDamage(health, damage, popup, attacker, hitDirection, applyStatusMultiplier: false);
        }

        public static void DealDamage(Health health, int damage, bool popup, GameObject attacker, Vector2? hitDirection,
            string element = "skill", bool isCrit = false, bool applyStatusMultiplier = true)
        {
            if (health == null || health.IsDead) return;

            int finalDamage = damage;
            if (applyStatusMultiplier)
            {
                var status = health.GetComponent<StatusEffectSystem>();
                if (status != null)
                    finalDamage = Mathf.RoundToInt(finalDamage * status.damageMultiplierIncoming);
            }

            health.TakeDamage(finalDamage);
            PublishSkillHit(health, finalDamage, attacker, hitDirection, element, isCrit);
            if (popup)
                DamagePopup.Show(health.transform.position, finalDamage);
        }

        public static void PublishSkillHit(Health health, int damage, GameObject attacker = null, Vector2? hitDirection = null,
            string element = "skill", bool isCrit = false)
        {
            if (health == null) return;

            attacker ??= GameObject.FindGameObjectWithTag("Player");
            Vector2 direction = hitDirection ?? Vector2.zero;
            if (direction.sqrMagnitude < 0.001f && attacker != null)
                direction = ((Vector2)health.transform.position - (Vector2)attacker.transform.position).normalized;

            CombatEventBus.PublishHit(new HitEvent
            {
                worldPos = health.transform.position,
                attacker = attacker,
                target = health.gameObject,
                damage = damage,
                element = element,
                isCrit = isCrit,
                hitDirection = direction
            });
        }

        public static void ApplyKnockback(Health health, Vector2 from, float force)
        {
            if (health == null || force <= 0f) return;
            var rb = health.GetComponent<Rigidbody2D>();
            if (rb == null) return;
            Vector2 dir = ((Vector2)health.transform.position - from).normalized;
            rb.AddForce(dir * force, ForceMode2D.Impulse);
        }

        public static GameObject SpawnCircleVisual(Vector2 position, Color color, float scale, float life, string name = "SkillVisual")
        {
            var go = new GameObject(name);
            go.transform.position = position;
            go.transform.localScale = new Vector3(scale, scale, 1f);

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = ElementalistRuntimeVisuals.GetCircleSprite();
            renderer.color = color;
            renderer.sortingLayerName = "VFX";
            renderer.sortingOrder = 20;

            Object.Destroy(go, life);
            return go;
        }

        public static PlayerProjectile SpawnProjectile(Vector2 position, Vector2 direction, float speed, int damage, Color color, float scale, float life, string name,
            GameObject owner = null)
        {
            direction = direction.sqrMagnitude > 0.001f ? direction.normalized : Vector2.right;

            var go = new GameObject(name);
            go.transform.position = position;
            go.transform.localScale = new Vector3(scale, scale, 1f);

            var rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;

            var col = go.AddComponent<CircleCollider2D>();
            col.radius = 0.16f;
            col.isTrigger = true;

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = ElementalistRuntimeVisuals.GetCircleSprite();
            renderer.color = color;
            renderer.sortingLayerName = "VFX";
            renderer.sortingOrder = 20;

            var projectile = go.AddComponent<PlayerProjectile>();
            projectile.Init(direction * speed, damage, life: life, attacker: owner);
            return projectile;
        }
    }
}
