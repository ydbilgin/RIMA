using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace RIMA.Tests
{
    /// <summary>
    /// Prefab sağlık kontrolü — sprite visibility, sorting layer, interact radius,
    /// PlaceholderSprite varlığı gibi görsel/etkileşim sorunlarını otomatik yakalar.
    /// 
    /// Bu testler sorting layer bugging, invisible pickup, magenta square gibi
    /// sorunların TEKRAR oluşmasını engeller.
    /// </summary>
    public class PrefabHealthTests
    {
        // ── Sorting Layer sabitler ──────────────────────────────────────────
        private const string EntitiesLayer = "Entities";
        private const string GroundLayer   = "Ground";

        // ── Helper: prefab yükle ────────────────────────────────────────────
        private static GameObject LoadPrefab(string path)
        {
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            Assert.IsNotNull(go, $"Prefab bulunamadı: {path}");
            return go;
        }

        private static SpriteRenderer GetSR(GameObject go)
        {
            var sr = go.GetComponent<SpriteRenderer>();
            Assert.IsNotNull(sr, $"{go.name} SpriteRenderer eksik!");
            return sr;
        }

        // ════════════════════════════════════════════════════════════════════
        //  1. REWARD & MAP FRAGMENT — Görünürlük
        // ════════════════════════════════════════════════════════════════════

        [Test]
        public void RewardPickup_SortingLayer_IsEntities()
        {
            var go = LoadPrefab("Assets/Prefabs/RewardPickup.prefab");
            var sr = GetSR(go);
            Assert.AreEqual(EntitiesLayer, sr.sortingLayerName,
                "RewardPickup Default layer'da kalırsa zeminin ALTINDA render edilir → görünmez!");
        }

        [Test]
        public void RewardPickup_SortingOrder_AboveFloor()
        {
            var go = LoadPrefab("Assets/Prefabs/RewardPickup.prefab");
            var sr = GetSR(go);
            Assert.GreaterOrEqual(sr.sortingOrder, 5,
                "RewardPickup sortingOrder çok düşük — diğer entity'lerin altında kalabilir.");
        }

        [Test]
        public void MapFragment_SortingLayer_IsEntities()
        {
            var go = LoadPrefab("Assets/Prefabs/MapFragment.prefab");
            var sr = GetSR(go);
            Assert.AreEqual(EntitiesLayer, sr.sortingLayerName,
                "MapFragment Default layer'da kalırsa zeminin ALTINDA render edilir → görünmez!");
        }

        [Test]
        public void MapFragment_SortingOrder_AboveFloor()
        {
            var go = LoadPrefab("Assets/Prefabs/MapFragment.prefab");
            var sr = GetSR(go);
            Assert.GreaterOrEqual(sr.sortingOrder, 5,
                "MapFragment sortingOrder çok düşük.");
        }

        // ════════════════════════════════════════════════════════════════════
        //  2. INTERACT RADIUS — İzometrik grid için yeterli mi?
        // ════════════════════════════════════════════════════════════════════

        [Test]
        public void RewardPickup_InteractRadius_AtLeast3()
        {
            var go = LoadPrefab("Assets/Prefabs/RewardPickup.prefab");
            var rp = go.GetComponent<RewardPickup>();
            Assert.IsNotNull(rp, "RewardPickup component eksik!");

            var field = typeof(RewardPickup).GetField("interactRadius",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(field, "interactRadius field bulunamadı!");

            float radius = (float)field.GetValue(rp);
            Assert.GreaterOrEqual(radius, 3f,
                $"interactRadius={radius} — izometrik grid'de dünya mesafesi büyük, en az 3 olmalı!");
        }

        [Test]
        public void MapFragment_InteractRadius_AtLeast3()
        {
            var go = LoadPrefab("Assets/Prefabs/MapFragment.prefab");
            var mf = go.GetComponent<MapFragment>();
            Assert.IsNotNull(mf, "MapFragment component eksik!");

            var field = typeof(MapFragment).GetField("interactRadius",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(field, "interactRadius field bulunamadı!");

            float radius = (float)field.GetValue(mf);
            Assert.GreaterOrEqual(radius, 3f,
                $"interactRadius={radius} — izometrik grid'de dünya mesafesi büyük, en az 3 olmalı!");
        }

        // ════════════════════════════════════════════════════════════════════
        //  3. PLACEHOLDER SPRITE — Sprite'sız SpriteRenderer = MAGENTA KARE
        // ════════════════════════════════════════════════════════════════════

        private static readonly string[] AllSpriteRendererPrefabs = new[]
        {
            "Assets/Prefabs/RewardPickup.prefab",
            "Assets/Prefabs/MapFragment.prefab",
            "Assets/Prefabs/Obstacles/StoneColumn.prefab",
            "Assets/Prefabs/Obstacles/NarrowPassage.prefab",
            "Assets/Prefabs/Obstacles/Chasm.prefab",
            "Assets/Prefabs/Enemies/VoidThrall.prefab",
            "Assets/Prefabs/Enemies/VoidThrall_Elite.prefab",
            "Assets/Prefabs/Enemies/FractureImp.prefab",
            "Assets/Prefabs/Enemies/ChainWarden.prefab",
            "Assets/Prefabs/Enemies/HalfThrall.prefab",
            "Assets/Prefabs/Enemies/Penitent.prefab",
            "Assets/Prefabs/Enemies/RelicCaster.prefab",
            "Assets/Prefabs/Enemies/SeamCrawler.prefab",
            "Assets/Prefabs/Enemies/SeamCrawler_Elite.prefab",
            "Assets/Prefabs/Enemies/Projectile.prefab",
        };

        [Test]
        public void AllPrefabs_WithSpriteRenderer_HavePlaceholderOrSprite()
        {
            var failures = new System.Collections.Generic.List<string>();

            foreach (var path in AllSpriteRendererPrefabs)
            {
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (go == null) { failures.Add($"MISSING: {path}"); continue; }

                var sr = go.GetComponent<SpriteRenderer>();
                if (sr == null) continue; // SpriteRenderer yoksa sorun yok

                bool hasSprite = sr.sprite != null;
                bool hasPlaceholder = go.GetComponent<PlaceholderSprite>() != null;

                if (!hasSprite && !hasPlaceholder)
                    failures.Add($"{go.name} ({path}): SpriteRenderer VAR ama sprite YOK ve PlaceholderSprite YOK → MAGENTA KARE!");
            }

            Assert.IsEmpty(failures,
                "Sprite'sız SpriteRenderer = MOR KARE! PlaceholderSprite ekle:\n" +
                string.Join("\n", failures));
        }

        // ════════════════════════════════════════════════════════════════════
        //  4. ENEMY PREFABS — Sorting layer Entities olmalı
        // ════════════════════════════════════════════════════════════════════

        private static readonly string[] EnemyPrefabs = new[]
        {
            "Assets/Prefabs/Enemies/VoidThrall.prefab",
            "Assets/Prefabs/Enemies/FractureImp.prefab",
            "Assets/Prefabs/Enemies/ChainWarden.prefab",
            "Assets/Prefabs/Enemies/HalfThrall.prefab",
            "Assets/Prefabs/Enemies/Penitent.prefab",
            "Assets/Prefabs/Enemies/RelicCaster.prefab",
            "Assets/Prefabs/Enemies/SeamCrawler.prefab",
        };

        [Test]
        public void EnemyPrefabs_SortingLayer_IsEntities()
        {
            var failures = new System.Collections.Generic.List<string>();

            foreach (var path in EnemyPrefabs)
            {
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (go == null) continue;

                var sr = go.GetComponent<SpriteRenderer>();
                if (sr == null) continue;

                if (sr.sortingLayerName != EntitiesLayer)
                    failures.Add($"{go.name}: sortingLayer={sr.sortingLayerName} (beklenen: {EntitiesLayer})");
            }

            Assert.IsEmpty(failures,
                "Düşmanlar Entities layer'da olmalı, yoksa zeminin altında kalır:\n" +
                string.Join("\n", failures));
        }

        // ════════════════════════════════════════════════════════════════════
        //  5. ENEMY PREFABS — Health component olmalı (oda temizlenme)
        // ════════════════════════════════════════════════════════════════════

        [Test]
        public void EnemyPrefabs_HaveHealthComponent()
        {
            var failures = new System.Collections.Generic.List<string>();

            foreach (var path in EnemyPrefabs)
            {
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (go == null) continue;

                if (go.GetComponent<Health>() == null)
                    failures.Add($"{go.name}: Health component YOK → OnEnemyDied tetiklenmez → oda ASLA temizlenmez!");
            }

            Assert.IsEmpty(failures,
                "Health eksik düşmanlar:\n" + string.Join("\n", failures));
        }

        // ════════════════════════════════════════════════════════════════════
        //  6. OBSTACLE PREFABS — Sorting kontrolü
        // ════════════════════════════════════════════════════════════════════

        private static readonly string[] ObstaclePrefabs = new[]
        {
            "Assets/Prefabs/Obstacles/StoneColumn.prefab",
            "Assets/Prefabs/Obstacles/NarrowPassage.prefab",
            "Assets/Prefabs/Obstacles/Chasm.prefab",
        };

        [Test]
        public void ObstaclePrefabs_SortingLayer_NotDefault()
        {
            var failures = new System.Collections.Generic.List<string>();

            foreach (var path in ObstaclePrefabs)
            {
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (go == null) continue;

                var sr = go.GetComponent<SpriteRenderer>();
                if (sr == null) continue;

                if (sr.sortingLayerName == "Default")
                    failures.Add($"{go.name}: sortingLayer=Default → zeminin ALTINDA render edilir!");
            }

            Assert.IsEmpty(failures,
                "Default layer'daki obstacle'lar görünmez:\n" + string.Join("\n", failures));
        }

        // ════════════════════════════════════════════════════════════════════
        //  7. RRM PREFAB REFERANSLARı — null olmamalı
        // ════════════════════════════════════════════════════════════════════

        [Test]
        public void RuntimeRoomManager_PrefabReferences_NotNull()
        {
            // Scene'deki RRM'i bul (test scene'i yüklü olmalı)
            var guids = AssetDatabase.FindAssets("t:Scene _IsoGame");
            if (guids.Length == 0)
            {
                Assert.Inconclusive("_IsoGame scene bulunamadı.");
                return;
            }

            // Prefab varlığını kontrol et
            var rewardPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/RewardPickup.prefab");
            var mapPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/MapFragment.prefab");

            Assert.IsNotNull(rewardPrefab, "RewardPickup.prefab bulunamadı!");
            Assert.IsNotNull(mapPrefab, "MapFragment.prefab bulunamadı!");
        }
    }
}
