using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RIMA.Map
{
    public class RoomInstance : MonoBehaviour
    {
        public string roomId;
        public string roomType;
        public bool isLocked;
        public List<RoomMobSpawn> mobSpawns = new List<RoomMobSpawn>();
        public List<GameObject> mobInstances = new List<GameObject>();
        public List<global::RIMA.DoorTrigger> doors = new List<global::RIMA.DoorTrigger>();

        private readonly Dictionary<global::RIMA.Health, UnityAction> deathListeners = new Dictionary<global::RIMA.Health, UnityAction>();
        private int currentWave;
        private int highestWave;
        private int aliveMobCount;
        private bool combatCleared;

        public void OnEnter()
        {
            RefreshSpawnMarkers();

            if (!IsCombatRoom())
            {
                if (isLocked) LockDoors();
                return;
            }

            if (combatCleared)
            {
                UnlockDoors();
                return;
            }

            currentWave = 1;
            highestWave = GetHighestWave();
            LockDoors();
            SpawnWave(currentWave);
        }

        public void OnExit()
        {
            UnsubscribeEnemyDeaths();
        }

        public void LockDoors()
        {
            isLocked = true;
            SetDoorsActive(false);
            SpawnDoorVFX(new Color(1f, 0.25f, 0.15f, 0.85f));
        }

        public void UnlockDoors()
        {
            isLocked = false;
            SetDoorsActive(true);
            SpawnDoorVFX(new Color(0.25f, 1f, 0.45f, 0.85f));
        }

        public void OnEnemyDeath()
        {
            aliveMobCount = Mathf.Max(0, aliveMobCount - 1);
            if (aliveMobCount > 0) return;

            int nextWave = FindNextWave(currentWave + 1);
            if (nextWave > 0)
            {
                currentWave = nextWave;
                SpawnWave(currentWave);
                return;
            }

            combatCleared = true;
            UnlockDoors();
        }

        private void SetDoorsActive(bool active)
        {
            if (doors == null) return;

            for (int i = 0; i < doors.Count; i++)
            {
                if (doors[i] != null) doors[i].SetActive(active);
            }
        }

        private bool IsCombatRoom()
        {
            string type = string.IsNullOrEmpty(roomType) ? string.Empty : roomType.ToLowerInvariant();
            if (type == "combat_small" || type == "combat_medium" || type == "combat_large" || type == "boss_arena")
                return true;

            return mobSpawns != null && mobSpawns.Count > 0;
        }

        private int GetHighestWave()
        {
            int highest = 0;
            for (int i = 0; i < mobSpawns.Count; i++)
                highest = Mathf.Max(highest, Mathf.Max(1, mobSpawns[i].wave));
            return highest;
        }

        private int FindNextWave(int startWave)
        {
            for (int wave = startWave; wave <= highestWave; wave++)
            {
                for (int i = 0; i < mobSpawns.Count; i++)
                {
                    if (Mathf.Max(1, mobSpawns[i].wave) == wave)
                        return wave;
                }
            }

            return 0;
        }

        private void SpawnWave(int wave)
        {
            aliveMobCount = 0;

            for (int i = 0; i < mobSpawns.Count; i++)
            {
                RoomMobSpawn spawn = mobSpawns[i];
                if (spawn == null || Mathf.Max(1, spawn.wave) != wave) continue;

                GameObject mob = SpawnMob(spawn);
                if (mob == null) continue;

                mobInstances.Add(mob);
                global::RIMA.Health health = mob.GetComponent<global::RIMA.Health>();
                if (health == null) health = mob.AddComponent<global::RIMA.Health>();

                GameObject trackedMob = mob;
                UnityAction listener = () => HandleEnemyDeath(health, trackedMob);
                deathListeners[health] = listener;
                health.OnDeath.AddListener(listener);
                aliveMobCount++;
            }

            if (aliveMobCount == 0)
                OnEnemyDeath();
        }

        private GameObject SpawnMob(RoomMobSpawn spawn)
        {
            GameObject prefab = ResolveMobPrefab(spawn.mob_id);
            Vector3 position = new Vector3(spawn.x, spawn.y, 0f);

            if (prefab != null)
                return Instantiate(prefab, position, Quaternion.identity, transform);

            GameObject fallback = new GameObject("Mob_" + spawn.mob_id + "_W" + Mathf.Max(1, spawn.wave));
            fallback.transform.SetParent(transform, false);
            fallback.transform.position = position;
            fallback.AddComponent<global::RIMA.Health>();
            fallback.AddComponent<CircleCollider2D>().isTrigger = false;
            return fallback;
        }

        private GameObject ResolveMobPrefab(string mobId)
        {
            if (string.IsNullOrEmpty(mobId)) return null;

            GameObject prefab = Resources.Load<GameObject>("Mobs/" + mobId)
                ?? Resources.Load<GameObject>("Enemies/" + mobId)
                ?? Resources.Load<GameObject>(mobId);
            if (prefab != null) return prefab;

#if UNITY_EDITOR
            string path = mobId switch
            {
                "rift_husk" => "Assets/Prefabs/Enemies/HollowMite.prefab",
                "shattered_knight" => "Assets/Prefabs/Enemies/Penitent.prefab",
                "rift_acolyte" => "Assets/Prefabs/Enemies/RelicCaster.prefab",
                "act1_boss_shattered_king" => "Assets/Prefabs/Enemies/Boss/PenitentSovereign.prefab",
                _ => null
            };

            if (!string.IsNullOrEmpty(path))
                return AssetDatabase.LoadAssetAtPath<GameObject>(path);
#endif

            return null;
        }

        private void HandleEnemyDeath(global::RIMA.Health health, GameObject mob)
        {
            if (health != null && deathListeners.TryGetValue(health, out UnityAction listener))
            {
                health.OnDeath.RemoveListener(listener);
                deathListeners.Remove(health);
            }

            OnEnemyDeath();
        }

        private void UnsubscribeEnemyDeaths()
        {
            foreach (var pair in deathListeners)
            {
                if (pair.Key != null)
                    pair.Key.OnDeath.RemoveListener(pair.Value);
            }

            deathListeners.Clear();
        }

        private void RefreshSpawnMarkers()
        {
            if (mobSpawns == null) mobSpawns = new List<RoomMobSpawn>();
            if (mobSpawns.Count > 0) return;

            foreach (Transform child in transform)
            {
                if (!child.name.StartsWith("MobSpawn_")) continue;

                RoomMobSpawn spawn = new RoomMobSpawn();
                ParseMobSpawnMarker(child, spawn);
                spawn.x = child.position.x;
                spawn.y = child.position.y;
                mobSpawns.Add(spawn);
            }
        }

        private static void ParseMobSpawnMarker(Transform marker, RoomMobSpawn spawn)
        {
            string name = marker.name.Substring("MobSpawn_".Length);
            int waveIndex = name.LastIndexOf("_W", System.StringComparison.Ordinal);
            if (waveIndex >= 0)
            {
                spawn.mob_id = name.Substring(0, waveIndex);
                if (int.TryParse(name.Substring(waveIndex + 2), out int wave))
                    spawn.wave = Mathf.Max(1, wave);
                else
                    spawn.wave = 1;
            }
            else
            {
                spawn.mob_id = name;
                spawn.wave = 1;
            }
        }

        private void SpawnDoorVFX(Color color)
        {
            if (doors == null) return;

            for (int i = 0; i < doors.Count; i++)
            {
                if (doors[i] == null) continue;

                GameObject fx = new GameObject(isLocked ? "DoorLockVFX" : "DoorUnlockVFX");
                fx.transform.SetParent(doors[i].transform, false);
                fx.transform.localPosition = Vector3.zero;

                ParticleSystem particles = fx.AddComponent<ParticleSystem>();
                ParticleSystem.MainModule main = particles.main;
                main.startLifetime = 0.35f;
                main.startSpeed = 0.25f;
                main.startSize = 0.18f;
                main.startColor = color;
                main.loop = false;

                ParticleSystem.EmissionModule emission = particles.emission;
                emission.rateOverTime = 0f;
                particles.Emit(12);
                Destroy(fx, 0.8f);
            }
        }
    }
}
