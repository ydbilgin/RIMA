using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RIMA.Encounter
{
    public class EncounterController : MonoBehaviour
    {
        [SerializeField] private EncounterBankSO bank;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField, Min(0f)] private float difficulty = 10f;
        [SerializeField] private int seed;
        [SerializeField] private bool eliteRoom;
        [SerializeField] private bool startOnEnable;

        public UnityEvent<int> OnWaveStarted;
        public UnityEvent OnRoomCleared;

        private readonly ThreatBudget threatBudget = new ThreatBudget();
        private readonly List<GameObject> activeEnemies = new List<GameObject>();
        private readonly List<GameObject> openingWaveEnemies = new List<GameObject>();
        private EncounterWaveSO activeWave;
        private float spentBudget;
        private bool secondWaveSpawned;
        private bool encounterActive;

        private void OnEnable()
        {
            if (startOnEnable)
                OnRoomEnter();
        }

        public void OnRoomEnter()
        {
            activeWave = bank != null ? bank.PickWave(difficulty, seed) : null;
            if (activeWave == null)
            {
                Debug.LogWarning("[EncounterController] No EncounterWaveSO available.");
                return;
            }

            ClearRuntimeState();
            encounterActive = true;
            secondWaveSpawned = false;
            SpawnBudget(activeWave.threatBudget * activeWave.openingBudgetFraction, true, 0);

            if (activeEnemies.Count == 0)
                SpawnSecondWaveOrClear();
        }

        public void OnAllEnemiesDead()
        {
            if (!encounterActive)
                return;

            SpawnSecondWaveOrClear();
        }

        private void SpawnSecondWaveOrClear()
        {
            if (!secondWaveSpawned && spentBudget < activeWave.threatBudget)
            {
                secondWaveSpawned = true;
                SpawnBudget(activeWave.threatBudget - spentBudget, false, 1);
                if (activeEnemies.Count > 0)
                    return;
            }

            encounterActive = false;
            OnRoomCleared?.Invoke();
        }

        private void SpawnBudget(float budget, bool openingWave, int waveIndex)
        {
            Transform[] points = spawnPoints != null && spawnPoints.Length > 0
                ? spawnPoints
                : new[] { transform };

            int t2Cap = eliteRoom ? activeWave.eliteRoomT2Cap : activeWave.normalRoomT2Cap;
            List<GameObject> spawned = threatBudget.Spawn(activeWave, points, budget, t2Cap, eliteRoom);
            spentBudget += threatBudget.LastSpawnCost;

            for (int i = 0; i < spawned.Count; i++)
            {
                GameObject enemy = spawned[i];
                if (enemy == null) continue;

                activeEnemies.Add(enemy);
                if (openingWave)
                    openingWaveEnemies.Add(enemy);

                Health health = enemy.GetComponent<Health>();
                if (health == null)
                    health = enemy.AddComponent<Health>();

                GameObject trackedEnemy = enemy;
                health.OnDeath.AddListener(() => OnEnemyDead(trackedEnemy));
            }

            OnWaveStarted?.Invoke(waveIndex);
        }

        private void OnEnemyDead(GameObject enemy)
        {
            activeEnemies.Remove(enemy);
            threatBudget.Release(enemy);

            if (!secondWaveSpawned && openingWaveEnemies.Count > 0)
            {
                int deadOpeningEnemies = 0;
                for (int i = 0; i < openingWaveEnemies.Count; i++)
                {
                    GameObject tracked = openingWaveEnemies[i];
                    if (tracked == null || !activeEnemies.Contains(tracked))
                        deadOpeningEnemies++;
                }

                float deadFraction = deadOpeningEnemies / (float)openingWaveEnemies.Count;
                if (deadFraction >= activeWave.nextWaveKillFraction)
                    SpawnSecondWaveOrClear();
            }

            if (activeEnemies.Count == 0)
                OnAllEnemiesDead();
        }

        private void ClearRuntimeState()
        {
            threatBudget.Reset();
            activeEnemies.Clear();
            openingWaveEnemies.Clear();
            spentBudget = 0f;
        }
    }
}
