using System;
using System.Collections;
using UnityEngine;

namespace RIMA.Combat
{
    /// <summary>
    /// Places trap, sigil, totem, or mark prefabs with lifetime and trigger behavior.
    /// </summary>
    public class PlacedEffectSpawner : MonoBehaviour
    {
        public GameObject Place(Vector3 position, PlacedEffectConfig config)
        {
            if (config == null || config.effectPrefab == null)
            {
                return null;
            }

            GameObject instance = Instantiate(config.effectPrefab, position, Quaternion.identity);
            ConfigureInstance(instance, null, config);
            return instance;
        }

        public GameObject PlaceOnEnemy(Transform target, PlacedEffectConfig config)
        {
            if (target == null)
            {
                return null;
            }

            GameObject instance = Place(target.position, config);
            if (instance != null && config != null && config.parentToTarget)
            {
                instance.transform.SetParent(target, true);
            }

            return instance;
        }

        private void ConfigureInstance(GameObject instance, Transform target, PlacedEffectConfig config)
        {
            PlacedEffectRuntime runtime = instance.GetComponent<PlacedEffectRuntime>();
            if (runtime == null)
            {
                runtime = instance.AddComponent<PlacedEffectRuntime>();
            }

            runtime.Initialize(config, target);
        }
    }

    /// <summary>
    /// Runtime configuration for placed effect lifetime and trigger behavior.
    /// </summary>
    [Serializable]
    public class PlacedEffectConfig
    {
        public GameObject effectPrefab;
        public float lifetime = 5f;
        public bool parentToTarget = false;
        public TriggerType triggerType = TriggerType.OnEnter;
        public float triggerRadius = 0.5f;
        public float damage = 0f;
        public Action<Transform> onTrigger;
    }

    /// <summary>
    /// Trigger mode used by placed effect runtime polling.
    /// </summary>
    public enum TriggerType
    {
        OnEnter,
        OnExit,
        Continuous,
        OnTimer
    }

    /// <summary>
    /// Runtime driver attached to placed effect instances.
    /// </summary>
    internal class PlacedEffectRuntime : MonoBehaviour
    {
        private PlacedEffectConfig config;
        private readonly Collider2D[] hits = new Collider2D[16];
        private readonly System.Collections.Generic.HashSet<Transform> inside = new System.Collections.Generic.HashSet<Transform>();
        private float expireTime;

        public void Initialize(PlacedEffectConfig config, Transform target)
        {
            this.config = config;
            expireTime = Time.time + Mathf.Max(0.01f, config.lifetime);

            if (config.triggerType == TriggerType.OnTimer)
            {
                StartCoroutine(TriggerOnTimer());
            }
        }

        private void Update()
        {
            if (config == null)
            {
                Destroy(gameObject);
                return;
            }

            if (Time.time >= expireTime)
            {
                Destroy(gameObject);
                return;
            }

            if (config.triggerType != TriggerType.OnTimer)
            {
                PollTriggers();
            }
        }

        private IEnumerator TriggerOnTimer()
        {
            yield return new WaitForSeconds(Mathf.Max(0.01f, config.lifetime));
            Trigger(null);
        }

        private void PollTriggers()
        {
            int count = Physics2D.OverlapCircleNonAlloc(transform.position, config.triggerRadius, hits);
            System.Collections.Generic.HashSet<Transform> current = new System.Collections.Generic.HashSet<Transform>();

            for (int i = 0; i < count; i++)
            {
                Collider2D hit = hits[i];
                if (hit == null || hit.transform == transform)
                {
                    continue;
                }

                Transform hitTransform = hit.transform;
                current.Add(hitTransform);

                if (config.triggerType == TriggerType.OnEnter && !inside.Contains(hitTransform))
                {
                    Trigger(hitTransform);
                }
                else if (config.triggerType == TriggerType.Continuous)
                {
                    Trigger(hitTransform);
                }
            }

            if (config.triggerType == TriggerType.OnExit)
            {
                foreach (Transform previous in inside)
                {
                    if (previous != null && !current.Contains(previous))
                    {
                        Trigger(previous);
                    }
                }
            }

            inside.Clear();
            foreach (Transform hitTransform in current)
            {
                inside.Add(hitTransform);
            }
        }

        private void Trigger(Transform target)
        {
            config.onTrigger?.Invoke(target);

            if (target != null && config.damage > 0f)
            {
                target.SendMessage("TakeDamage", config.damage, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
