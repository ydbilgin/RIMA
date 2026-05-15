using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Combat
{
    public class VFXRouter : MonoBehaviour
    {
        public static VFXRouter Instance { get; private set; }

        [Serializable]
        public class VFXEntry
        {
            public string tag;
            public GameObject prefab;
            public AudioClip soundEffect;
            public float lifetime = 2f;
        }

        [SerializeField] private VFXEntry[] entries;
        [SerializeField] private int poolSize = 16;

        private readonly Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();
        private readonly Dictionary<string, VFXEntry> entryLookup = new Dictionary<string, VFXEntry>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            BuildPools();
            Subscribe();
        }

        private void OnDestroy()
        {
            Unsubscribe();

            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void BuildPools()
        {
            pools.Clear();
            entryLookup.Clear();

            if (entries == null)
            {
                return;
            }

            int safePoolSize = Mathf.Max(0, poolSize);
            for (int i = 0; i < entries.Length; i++)
            {
                VFXEntry entry = entries[i];
                if (entry == null || string.IsNullOrEmpty(entry.tag) || entry.prefab == null)
                {
                    continue;
                }

                entryLookup[entry.tag] = entry;

                Queue<GameObject> pool = new Queue<GameObject>(safePoolSize);
                for (int j = 0; j < safePoolSize; j++)
                {
                    GameObject instance = Instantiate(entry.prefab, transform);
                    instance.SetActive(false);
                    pool.Enqueue(instance);
                }

                pools[entry.tag] = pool;
            }
        }

        private void Subscribe()
        {
            CombatEventBus.OnHit += HandleHit;
            CombatEventBus.OnKill += HandleKill;
            CombatEventBus.OnDash += HandleDash;
            CombatEventBus.OnStatusApplied += HandleStatusApplied;
            CombatEventBus.OnCommitBeat += HandleCommitBeat;
        }

        private void Unsubscribe()
        {
            CombatEventBus.OnHit -= HandleHit;
            CombatEventBus.OnKill -= HandleKill;
            CombatEventBus.OnDash -= HandleDash;
            CombatEventBus.OnStatusApplied -= HandleStatusApplied;
            CombatEventBus.OnCommitBeat -= HandleCommitBeat;
        }

        private void HandleHit(HitEvent e)
        {
            string element = string.IsNullOrEmpty(e.element) ? "default" : e.element;
            SpawnByTagWithFallback("hit_" + element, "hit_default", e.worldPos, RotationFromDirection(e.hitDirection));
        }

        private void HandleKill(KillEvent e)
        {
            string mobFamily = string.IsNullOrEmpty(e.mobFamily) ? "default" : e.mobFamily;
            SpawnByTagWithFallback("kill_" + mobFamily, "kill_default", e.worldPos, Quaternion.identity);
        }

        private void HandleDash(DashEvent e)
        {
            Vector3 direction = e.endPos - e.startPos;
            Quaternion rotation = direction.sqrMagnitude > 0.0001f ? Quaternion.LookRotation(Vector3.forward, direction.normalized) : Quaternion.identity;
            SpawnByTag("dash_default", e.startPos, rotation);
        }

        private void HandleStatusApplied(StatusEvent e)
        {
            string statusId = string.IsNullOrEmpty(e.statusId) ? "default" : e.statusId;
            SpawnByTag("status_" + statusId, e.worldPos, Quaternion.identity);
        }

        private void HandleCommitBeat(CommitBeatEvent e)
        {
            SpawnByTag("commit_beat_" + e.beatIndex, e.worldPos, Quaternion.identity);
        }

        private void SpawnByTagWithFallback(string tag, string fallbackTag, Vector3 position, Quaternion rotation)
        {
            if (!SpawnByTag(tag, position, rotation) && !string.Equals(tag, fallbackTag, StringComparison.Ordinal))
            {
                SpawnByTag(fallbackTag, position, rotation);
            }
        }

        private bool SpawnByTag(string tag, Vector3 position, Quaternion rotation)
        {
            VFXEntry entry;
            if (!entryLookup.TryGetValue(tag, out entry))
            {
                return false;
            }

            if (!ProcLimiter.TryProc(tag))
            {
                return true;
            }

            GameObject instance = GetFromPool(tag, entry);
            if (instance == null)
            {
                return true;
            }

            instance.transform.SetPositionAndRotation(position, rotation);
            instance.SetActive(true);
            PlaySound(entry.soundEffect, position);
            StartCoroutine(ReturnAfterLifetime(tag, instance, Mathf.Max(0.01f, entry.lifetime)));
            return true;
        }

        private GameObject GetFromPool(string tag, VFXEntry entry)
        {
            Queue<GameObject> pool;
            if (!pools.TryGetValue(tag, out pool))
            {
                pool = new Queue<GameObject>();
                pools[tag] = pool;
            }

            while (pool.Count > 0)
            {
                GameObject pooled = pool.Dequeue();
                if (pooled != null)
                {
                    return pooled;
                }
            }

            GameObject created = Instantiate(entry.prefab, transform);
            created.SetActive(false);
            return created;
        }

        private IEnumerator ReturnAfterLifetime(string tag, GameObject instance, float lifetime)
        {
            yield return new WaitForSeconds(lifetime);

            if (instance == null)
            {
                yield break;
            }

            instance.SetActive(false);

            Queue<GameObject> pool;
            if (!pools.TryGetValue(tag, out pool))
            {
                pool = new Queue<GameObject>();
                pools[tag] = pool;
            }

            pool.Enqueue(instance);
        }

        private static void PlaySound(AudioClip clip, Vector3 position)
        {
            if (clip != null)
            {
                AudioSource.PlayClipAtPoint(clip, position);
            }
        }

        private static Quaternion RotationFromDirection(Vector2 direction)
        {
            if (direction.sqrMagnitude <= 0.0001f)
            {
                return Quaternion.identity;
            }

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return Quaternion.Euler(0f, 0f, angle);
        }
    }
}
