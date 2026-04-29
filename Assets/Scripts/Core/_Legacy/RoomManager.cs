using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Basit oda yöneticisi: düşmanları spawn eder, hepsini öldür = oda temizlendi.
    /// Spawn noktaları boş GameObject olarak Inspector'dan atanır.
    /// </summary>
    public class RoomManager : MonoBehaviour
    {
        [Header("Spawn")]
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private int enemyCount = 3;
        [SerializeField] private float spawnDelay = 0.4f;

        [Header("References")]
        [SerializeField] private HUDManager hud;

        private int aliveCount;
        private bool roomCleared;

        private void Start()
        {
            if (enemyPrefab != null)
                StartCoroutine(SpawnWave());
        }

        private IEnumerator SpawnWave()
        {
            aliveCount = 0;
            for (int i = 0; i < enemyCount; i++)
            {
                Vector3 pos = spawnPoints != null && spawnPoints.Length > 0
                    ? spawnPoints[i % spawnPoints.Length].position
                    : transform.position + (Vector3)(Random.insideUnitCircle * 5f);

                var go = Instantiate(enemyPrefab, pos, Quaternion.identity);
                var hp = go.GetComponent<Health>();
                if (hp != null)
                    hp.OnDeath.AddListener(OnEnemyDied);

                aliveCount++;
                yield return new WaitForSeconds(spawnDelay);
            }

            hud?.SetRoomStatus($"Enemies: {aliveCount}");
        }

        private void OnEnemyDied()
        {
            aliveCount--;
            hud?.SetRoomStatus(aliveCount > 0 ? $"Enemies: {aliveCount}" : "ROOM CLEARED!");

            if (aliveCount <= 0 && !roomCleared)
            {
                roomCleared = true;
                OnRoomCleared();
            }
        }

        private void OnRoomCleared()
        {
            Debug.Log("Room cleared! Skill draft açılıyor...");
            if (DraftManager.Instance != null)
                DraftManager.Instance.ShowDraft();
        }
    }
}
