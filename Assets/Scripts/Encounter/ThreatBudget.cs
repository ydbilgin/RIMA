using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Encounter
{
    public class ThreatBudget
    {
        private readonly Dictionary<EncounterEnemyEntry, int> spawnedByEntry = new Dictionary<EncounterEnemyEntry, int>();
        private readonly Dictionary<EncounterEnemyType, int> activeByType = new Dictionary<EncounterEnemyType, int>();
        private readonly Dictionary<GameObject, EncounterEnemyEntry> activeEntries = new Dictionary<GameObject, EncounterEnemyEntry>();
        private int activeT2Count;

        public float LastSpawnCost { get; private set; }

        public void Reset()
        {
            spawnedByEntry.Clear();
            activeByType.Clear();
            activeEntries.Clear();
            activeT2Count = 0;
            LastSpawnCost = 0f;
        }

        public void Release(GameObject enemy)
        {
            if (enemy == null || !activeEntries.TryGetValue(enemy, out EncounterEnemyEntry entry))
                return;

            activeEntries.Remove(enemy);

            activeByType.TryGetValue(entry.enemyType, out int activeTypeCount);
            activeByType[entry.enemyType] = Mathf.Max(0, activeTypeCount - 1);

            if (entry.t2Capable)
                activeT2Count = Mathf.Max(0, activeT2Count - 1);
        }

        public List<GameObject> Spawn(EncounterWaveSO wave, Transform[] spawnPoints)
        {
            return Spawn(wave, spawnPoints, wave != null ? wave.threatBudget : 0f, false);
        }

        public List<GameObject> Spawn(EncounterWaveSO wave, Transform[] spawnPoints, float budget, bool isEliteRoom = false)
        {
            return Spawn(wave, spawnPoints, budget, wave != null ? wave.normalRoomT2Cap : 0, isEliteRoom);
        }

        public List<GameObject> Spawn(EncounterWaveSO wave, Transform[] spawnPoints, float budget, int t2Cap, bool isEliteRoom = false)
        {
            LastSpawnCost = 0f;
            List<GameObject> spawned = new List<GameObject>();
            if (wave == null || wave.entries == null || spawnPoints == null || spawnPoints.Length == 0 || budget <= 0f)
                return spawned;

            float remaining = budget;
            int guard = 0;

            while (guard++ < 128)
            {
                EncounterEnemyEntry entry = PickEntry(wave.entries, remaining, t2Cap, isEliteRoom);
                if (entry == null)
                    break;

                Transform point = spawnPoints[spawned.Count % spawnPoints.Length];
                if (point == null)
                    break;

                GameObject instance = Object.Instantiate(entry.prefab, point.position, point.rotation);
                spawned.Add(instance);
                LastSpawnCost += entry.threatCost;
                remaining -= entry.threatCost;

                spawnedByEntry.TryGetValue(entry, out int usedCount);
                spawnedByEntry[entry] = usedCount + 1;

                activeEntries[instance] = entry;

                activeByType.TryGetValue(entry.enemyType, out int typeCount);
                activeByType[entry.enemyType] = typeCount + 1;

                if (entry.t2Capable)
                    activeT2Count++;
            }

            return spawned;
        }

        private EncounterEnemyEntry PickEntry(
            List<EncounterEnemyEntry> entries,
            float remainingBudget,
            int t2Cap,
            bool isEliteRoom)
        {
            float totalWeight = 0f;
            for (int i = 0; i < entries.Count; i++)
            {
                EncounterEnemyEntry entry = entries[i];
                if (!IsEligible(entry, remainingBudget, t2Cap, isEliteRoom))
                    continue;

                totalWeight += Mathf.Max(0f, entry.weight);
            }

            if (totalWeight <= 0f)
                return null;

            float roll = Random.value * totalWeight;
            for (int i = 0; i < entries.Count; i++)
            {
                EncounterEnemyEntry entry = entries[i];
                if (!IsEligible(entry, remainingBudget, t2Cap, isEliteRoom))
                    continue;

                roll -= Mathf.Max(0f, entry.weight);
                if (roll <= 0f)
                    return entry;
            }

            return null;
        }

        private bool IsEligible(
            EncounterEnemyEntry entry,
            float remainingBudget,
            int t2Cap,
            bool isEliteRoom)
        {
            if (entry == null || entry.prefab == null || entry.count <= 0 || entry.threatCost <= 0f)
                return false;
            if (entry.threatCost > remainingBudget)
                return false;
            if (entry.eliteOnly && !isEliteRoom)
                return false;

            spawnedByEntry.TryGetValue(entry, out int usedCount);
            if (usedCount >= entry.count)
                return false;

            activeByType.TryGetValue(entry.enemyType, out int typeCount);
            if (typeCount >= Mathf.Max(1, entry.maxSimultaneous))
                return false;

            return !entry.t2Capable || activeT2Count < t2Cap;
        }
    }
}
