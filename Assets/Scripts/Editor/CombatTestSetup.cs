using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering.Universal;

namespace RIMA
{
    /// <summary>
    /// RIMA → Combat Test Setup menüsünden çalıştır.
    /// Player'a combat bileşenlerini ekler, Systems objesi kurar, düşman yerleştirir.
    /// </summary>
    public static class CombatTestSetup
    {
        [MenuItem("RIMA/Combat Test Setup")]
        public static void Run()
        {
            int changes = 0;

            // ── 1. Player bileşenleri ──────────────────────────────────────────
            var playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO == null)
            {
                Debug.LogError("[CombatSetup] Player bulunamadı — tag='Player' olan bir GameObject olmalı.");
                return;
            }

            if (playerGO.GetComponent<Health>() == null)
            {
                var hp = playerGO.AddComponent<Health>();
                // PlayerStats default HP 100 — Health kendi default'unu kullanır
                Debug.Log("[CombatSetup] Player → Health eklendi.");
                changes++;
            }

            if (playerGO.GetComponent<PlayerAttack>() == null)
            {
                playerGO.AddComponent<PlayerAttack>();
                Debug.Log("[CombatSetup] Player → PlayerAttack eklendi.");
                changes++;
            }

            if (playerGO.GetComponent<KnockbackReceiver>() == null)
            {
                playerGO.AddComponent<KnockbackReceiver>();
                Debug.Log("[CombatSetup] Player → KnockbackReceiver eklendi.");
                changes++;
            }

            // RageSystem — optional, PlayerAttack null-safe ama varsa daha iyi
            if (playerGO.GetComponent<RageSystem>() == null)
            {
                playerGO.AddComponent<RageSystem>();
                Debug.Log("[CombatSetup] Player → RageSystem eklendi.");
                changes++;
            }

            // ── 2. Systems singleton objesi ───────────────────────────────────
            var systemsGO = GameObject.Find("Systems");
            if (systemsGO == null)
            {
                systemsGO = new GameObject("Systems");
                Debug.Log("[CombatSetup] 'Systems' GameObject oluşturuldu.");
                changes++;
            }

            if (systemsGO.GetComponent<HitStop>() == null)
            {
                systemsGO.AddComponent<HitStop>();
                Debug.Log("[CombatSetup] Systems → HitStop eklendi.");
                changes++;
            }

            if (systemsGO.GetComponent<CameraShake>() == null)
            {
                systemsGO.AddComponent<CameraShake>();
                Debug.Log("[CombatSetup] Systems → CameraShake eklendi.");
                changes++;
            }

            // ── 3. LightPulse — Global Light 2D'ye ekle ──────────────────────
#pragma warning disable CS0618
            var globalLight = Object.FindObjectOfType<Light2D>();
#pragma warning restore CS0618
            if (globalLight != null && globalLight.GetComponent<LightPulse>() == null)
            {
                globalLight.gameObject.AddComponent<LightPulse>();
                Debug.Log("[CombatSetup] GlobalLight2D → LightPulse eklendi.");
                changes++;
            }

            // ── 4. Test düşmanı yerleştir ─────────────────────────────────────
            // Önce sahnede zaten EnemyAI var mı bak
#pragma warning disable CS0618
            var existingEnemy = Object.FindObjectOfType<EnemyAI>();
#pragma warning restore CS0618
            if (existingEnemy == null)
            {
                // FractureImp prefabı dene (en hafif düşman)
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/FractureImp.prefab");
                if (prefab == null)
                {
                    // Herhangi bir enemy prefab dene
                    var guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs/Enemies" });
                    if (guids.Length > 0)
                        prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guids[0]));
                }

                if (prefab != null)
                {
                    // Player'ın 2 unit sağına koy
                    Vector3 spawnPos = playerGO.transform.position + new Vector3(2f, 0f, 0f);
                    var enemy = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    enemy.transform.position = spawnPos;
                    Debug.Log($"[CombatSetup] '{prefab.name}' prefabı sahnede {spawnPos} konumuna yerleştirildi.");
                    changes++;
                }
                else
                {
                    // Prefab yoksa manuel oluştur
                    var enemyGO = new GameObject("EnemyTest");
                    enemyGO.transform.position = playerGO.transform.position + new Vector3(2f, 0f, 0f);
                    enemyGO.tag = "Enemy";
                    enemyGO.AddComponent<SpriteRenderer>();
                    var rb = enemyGO.AddComponent<Rigidbody2D>();
                    rb.gravityScale = 0f;
                    rb.freezeRotation = true;
                    var col = enemyGO.AddComponent<CapsuleCollider2D>();
                    col.size = new Vector2(0.6f, 1f);
                    enemyGO.AddComponent<EnemyPlaceholder>();
                    var health = enemyGO.AddComponent<Health>();
                    enemyGO.AddComponent<EnemyAI>();
                    Debug.Log("[CombatSetup] Manuel EnemyTest GameObject oluşturuldu.");
                    changes++;
                }
            }
            else
            {
                Debug.Log($"[CombatSetup] Düşman zaten var ({existingEnemy.gameObject.name}) — atlandı.");
            }

            // ── 5. Sahneyi dirty işaretle ─────────────────────────────────────
            if (changes > 0)
            {
                UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
                Debug.Log($"[CombatSetup] TAMAMLANDI — {changes} değişiklik. Ctrl+S ile kaydet, ardından Play.");
            }
            else
            {
                Debug.Log("[CombatSetup] Zaten kurulu — değişiklik yok.");
            }
        }

        [MenuItem("RIMA/Combat Test Setup", true)]
        private static bool ValidateRun()
        {
            // Edit modunda çalış
            return !Application.isPlaying;
        }
    }
}
