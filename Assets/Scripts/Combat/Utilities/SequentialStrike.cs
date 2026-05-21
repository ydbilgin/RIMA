using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Combat
{
    /// <summary>
    /// Executes a nearest-target sequential strike chain with optional jump-line flashes.
    /// </summary>
    public class SequentialStrike : MonoBehaviour
    {
        private const float JumpLineLifetime = 0.1f;

        public IEnumerator StrikeChain(Vector3 origin, SequentialStrikeConfig config)
        {
            if (config == null || config.maxJumps <= 0)
            {
                yield break;
            }

            Vector3 currentPosition = origin;
            HashSet<Transform> hitTargets = new HashSet<Transform>();
            int jumps = Mathf.Max(0, config.maxJumps);

            for (int i = 0; i < jumps; i++)
            {
                Transform target = FindNearestUnhit(currentPosition, config, hitTargets);
                if (target == null)
                {
                    yield break;
                }

                if (config.showJumpLines)
                {
                    SpawnJumpLine(currentPosition, target.position, config);
                }

                SpawnStrike(target.position, config);
                ApplyHit(target, config);
                hitTargets.Add(target);
                currentPosition = target.position;

                if (i < jumps - 1 && config.jumpDelay > 0f)
                {
                    yield return new WaitForSeconds(config.jumpDelay);
                }
            }
        }

        private static Transform FindNearestUnhit(Vector3 origin, SequentialStrikeConfig config, HashSet<Transform> hitTargets)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(origin, config.searchRadius, config.enemyMask);
            Transform nearest = null;
            float nearestDistance = float.MaxValue;

            for (int i = 0; i < hits.Length; i++)
            {
                Collider2D hit = hits[i];
                if (hit == null || hit.transform == null || hitTargets.Contains(hit.transform))
                {
                    continue;
                }

                float distance = (hit.transform.position - origin).sqrMagnitude;
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = hit.transform;
                }
            }

            return nearest;
        }

        private static void SpawnStrike(Vector3 position, SequentialStrikeConfig config)
        {
            if (config.strikePrefab != null)
            {
                Instantiate(config.strikePrefab, position, Quaternion.identity);
            }
        }

        private static void ApplyHit(Transform target, SequentialStrikeConfig config)
        {
            config.onHit?.Invoke(target);

            if (config.damage > 0f)
            {
                target.SendMessage("TakeDamage", config.damage, SendMessageOptions.DontRequireReceiver);
            }
        }

        private static void SpawnJumpLine(Vector3 start, Vector3 end, SequentialStrikeConfig config)
        {
            GameObject go = new GameObject("SequentialStrike_JumpLine");
            LineRenderer line = go.AddComponent<LineRenderer>();
            line.positionCount = 2;
            line.useWorldSpace = true;
            line.startWidth = 0.08f;
            line.endWidth = 0.02f;
            line.numCapVertices = 2;
            line.SetPosition(0, start);
            line.SetPosition(1, end);

            if (config.jumpLineMaterial != null)
            {
                line.material = config.jumpLineMaterial;
            }

            Destroy(go, JumpLineLifetime);
        }
    }

    /// <summary>
    /// Runtime configuration for sequential target strike chains.
    /// </summary>
    [Serializable]
    public class SequentialStrikeConfig
    {
        public GameObject strikePrefab;
        public int maxJumps = 3;
        public float jumpDelay = 0.08f;
        public float searchRadius = 4f;
        public LayerMask enemyMask;
        public float damage = 25f;
        public Action<Transform> onHit;
        public bool showJumpLines = true;
        public Material jumpLineMaterial;
    }
}
