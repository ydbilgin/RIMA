using TMPro;
using UnityEditor;
using UnityEngine;

namespace RIMA.Editor
{
    /// <summary>
    /// RIMA/Setup/Game Feel Setup menüsünden çalıştır.
    /// - HitStop singleton sahnede oluşturur
    /// - Sahnedeki tüm düşmanlara HitFlash + KnockbackReceiver ekler
    /// - DamagePopup prefabını Resources/ altında oluşturur
    /// </summary>
    public static class GameFeelSetup
    {
        [MenuItem("RIMA/Setup/Game Feel Setup (HitStop + HitFlash + Knockback + DamagePopup)")]
        public static void Run()
        {
            EnsureHitStop();
            EnsureCameraShake();
            AddEnemyComponents();
            EnsureDamagePopupPrefab();

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene());

            Debug.Log("[RIMA] Game Feel Setup tamamlandı.");
        }

        // ─── HitStop ─────────────────────────────────────────────────────────

        static void EnsureHitStop()
        {
            if (Object.FindFirstObjectByType<HitStop>() != null) return;

            var go = new GameObject("HitStop");
            go.AddComponent<HitStop>();
            Debug.Log("[RIMA] HitStop sahnede oluşturuldu.");
        }

        // ─── CameraShake ─────────────────────────────────────────────────────

        static void EnsureCameraShake()
        {
            var cam = Camera.main;
            if (cam == null) { Debug.LogWarning("[RIMA] Main Camera bulunamadı."); return; }
            if (cam.GetComponent<CameraShake>() == null)
                cam.gameObject.AddComponent<CameraShake>();
            Debug.Log("[RIMA] CameraShake kameraya eklendi.");
        }

        // ─── Enemy Components ─────────────────────────────────────────────────

        static void AddEnemyComponents()
        {
            // Sahnedeki tüm Health bileşenlerine sahip düşmanları bul
            // Player haricinde)
            var allHealth = Object.FindObjectsByType<Health>(FindObjectsSortMode.None);
            int count = 0;

            foreach (var hp in allHealth)
            {
                // Player'ı atla
                if (hp.GetComponent<PlayerController>() != null) continue;

                var go = hp.gameObject;

                if (go.GetComponent<HitFlash>() == null)
                    go.AddComponent<HitFlash>();

                if (go.GetComponent<KnockbackReceiver>() == null)
                {
                    var rb = go.GetComponent<Rigidbody2D>();
                    if (rb != null) go.AddComponent<KnockbackReceiver>();
                }

                count++;
            }

            Debug.Log($"[RIMA] {count} düşmana HitFlash + KnockbackReceiver eklendi.");
        }

        // ─── DamagePopup Prefab ───────────────────────────────────────────────

        static void EnsureDamagePopupPrefab()
        {
            const string prefabPath = "Assets/Resources/DamagePopup.prefab";

            // Zaten varsa skip
            if (AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath) != null)
            {
                Debug.Log("[RIMA] DamagePopup prefabı zaten mevcut.");
                return;
            }

            // Resources klasörü yoksa oluştur
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            // Prefab oluştur
            var go = new GameObject("DamagePopup");

            // TextMeshPro component
            var tmp = go.AddComponent<TextMeshPro>();
            tmp.text             = "0";
            tmp.fontSize         = 4f;
            tmp.color            = Color.white;
            tmp.alignment        = TextAlignmentOptions.Center;
            tmp.outlineWidth     = 0.2f;
            tmp.outlineColor     = new Color32(0, 0, 0, 255);
            tmp.sortingLayerID   = SortingLayer.NameToID("UI");
            tmp.sortingOrder     = 100;

            // DamagePopup script
            go.AddComponent<DamagePopup>();

            // Save as prefab
            var prefab = PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
            Object.DestroyImmediate(go);

            if (prefab != null)
                Debug.Log("[RIMA] DamagePopup.prefab Resources/ altında oluşturuldu.");
            else
                Debug.LogError("[RIMA] DamagePopup.prefab oluşturulamadı!");
        }
    }
}
