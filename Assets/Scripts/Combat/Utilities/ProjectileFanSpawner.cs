using System;
using System.Collections;
using UnityEngine;

namespace RIMA.Combat
{
    /// <summary>
    /// Spawns projectile prefabs across a configurable arc from an origin transform.
    /// </summary>
    public class ProjectileFanSpawner : MonoBehaviour
    {
        public void SpawnFan(Transform origin, Vector3 aimDirection, ProjectileFanConfig config)
        {
            if (origin == null || config == null || config.projectilePrefab == null || config.projectileCount <= 0)
            {
                return;
            }

            Vector3 direction = aimDirection.sqrMagnitude > 0.0001f ? aimDirection.normalized : origin.right;
            if (config.spawnIntervalSeconds > 0f)
            {
                StartCoroutine(SpawnFanRoutine(origin, direction, config));
                return;
            }

            for (int i = 0; i < config.projectileCount; i++)
            {
                SpawnProjectile(origin.position, GetDirectionForIndex(direction, i, config), config);
            }
        }

        private static IEnumerator SpawnFanRoutine(Transform origin, Vector3 direction, ProjectileFanConfig config)
        {
            for (int i = 0; i < config.projectileCount; i++)
            {
                if (origin == null)
                {
                    yield break;
                }

                SpawnProjectile(origin.position, GetDirectionForIndex(direction, i, config), config);

                if (i < config.projectileCount - 1)
                {
                    yield return new WaitForSeconds(config.spawnIntervalSeconds);
                }
            }
        }

        private static Vector3 GetDirectionForIndex(Vector3 centerDirection, int index, ProjectileFanConfig config)
        {
            if (config.projectileCount <= 1)
            {
                return centerDirection;
            }

            float step = config.spreadAngleDegrees / (config.projectileCount - 1);
            float angle = -config.spreadAngleDegrees * 0.5f + step * index;
            return Quaternion.AngleAxis(angle, Vector3.forward) * centerDirection;
        }

        private static void SpawnProjectile(Vector3 position, Vector3 direction, ProjectileFanConfig config)
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            GameObject projectile = Instantiate(config.projectilePrefab, position, rotation);
            Rigidbody2D body = projectile.GetComponent<Rigidbody2D>();

            if (body != null)
            {
                body.linearVelocity = direction.normalized * config.projectileSpeed;
            }
        }
    }

    /// <summary>
    /// Runtime configuration for spread projectile spawning.
    /// </summary>
    [Serializable]
    public class ProjectileFanConfig
    {
        public GameObject projectilePrefab;
        public int projectileCount = 5;
        public float spreadAngleDegrees = 30f;
        public float projectileSpeed = 12f;
        public float spawnIntervalSeconds = 0f;
    }
}
