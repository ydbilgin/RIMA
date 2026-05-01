using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

namespace RIMA.Editor
{
    /// <summary>
    /// One-shot wiring for S43 Phase 1 prefabs and Death Screen UI.
    /// Run Tools → RIMA → Setup All Prefabs & UI.
    /// Each step is idempotent (skips if asset already exists).
    /// </summary>
    public static class PrefabWiringSetup
    {
        [MenuItem("Tools/RIMA/Setup All Prefabs & UI", priority = 1)]
        public static void SetupAll()
        {
            CreateHandGlowVFXPrefab();
            CreateRiftGlowVFXPrefab();
            CreateHollowMitePrefab();
            CreateTheWoundPrefab();
            CreateBossAIPrefab();
            CreateDeathScreenUI();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[RIMA Setup] All prefabs and UI wired.");
        }

        // ─── VFX Prefabs ──────────────────────────────────────────────────────

        [MenuItem("Tools/RIMA/Prefabs/Create HandGlowVFX Prefab")]
        public static void CreateHandGlowVFXPrefab()
        {
            const string path = "Assets/Prefabs/VFX/HandGlowVFX.prefab";
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
            {
                Debug.Log("[RIMA] HandGlowVFX prefab already exists — skipped.");
                return;
            }

            var go = new GameObject("HandGlowVFX");
            go.AddComponent<ParticleSystem>();
            go.AddComponent<HandGlowVFX>();

            PrefabUtility.SaveAsPrefabAsset(go, path);
            Object.DestroyImmediate(go);
            Debug.Log("[RIMA] Created HandGlowVFX prefab.");
        }

        [MenuItem("Tools/RIMA/Prefabs/Create RiftGlowVFX Prefab")]
        public static void CreateRiftGlowVFXPrefab()
        {
            const string path = "Assets/Prefabs/VFX/RiftGlowVFX.prefab";
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
            {
                Debug.Log("[RIMA] RiftGlowVFX prefab already exists — skipped.");
                return;
            }

            var go = new GameObject("RiftGlowVFX");
            go.AddComponent<ParticleSystem>();

            // Secondary particle system child
            var secondary = new GameObject("SecondaryPS");
            secondary.transform.SetParent(go.transform, false);
            var secondaryPS = secondary.AddComponent<ParticleSystem>();

            var riftGlow = go.AddComponent<RiftGlowVFX>();

            // Wire secondary PS reference
            var so = new SerializedObject(riftGlow);
            so.FindProperty("secondaryPS").objectReferenceValue = secondaryPS;
            so.ApplyModifiedPropertiesWithoutUndo();

            PrefabUtility.SaveAsPrefabAsset(go, path);
            Object.DestroyImmediate(go);
            Debug.Log("[RIMA] Created RiftGlowVFX prefab with secondary PS.");
        }

        // ─── Enemy Prefabs ────────────────────────────────────────────────────

        [MenuItem("Tools/RIMA/Prefabs/Create HollowMite Prefab")]
        public static void CreateHollowMitePrefab()
        {
            const string path = "Assets/Prefabs/Enemies/HollowMite.prefab";
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
            {
                Debug.Log("[RIMA] HollowMite prefab already exists — skipped.");
                return;
            }

            var go = new GameObject("HollowMite");
            go.tag = "Enemy";
            go.layer = LayerMask.NameToLayer("Enemy");

            var sr = go.AddComponent<SpriteRenderer>();
            sr.color = new Color(0.6f, 0.4f, 0.8f, 1f); // placeholder purple

            var rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            var col = go.AddComponent<CircleCollider2D>();
            col.radius = 0.28f;

            var health = go.AddComponent<Health>();
            var so = new SerializedObject(health);
            so.FindProperty("maxHP").intValue = 30;
            so.ApplyModifiedPropertiesWithoutUndo();

            go.AddComponent<HollowMite>();

            PrefabUtility.SaveAsPrefabAsset(go, path);
            Object.DestroyImmediate(go);
            Debug.Log("[RIMA] Created HollowMite prefab.");
        }

        [MenuItem("Tools/RIMA/Prefabs/Create TheWound Prefab")]
        public static void CreateTheWoundPrefab()
        {
            const string path = "Assets/Prefabs/Enemies/TheWound.prefab";
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
            {
                Debug.Log("[RIMA] TheWound prefab already exists — skipped.");
                return;
            }

            var go = new GameObject("TheWound");
            go.tag = "Enemy";
            go.layer = LayerMask.NameToLayer("Enemy");

            var sr = go.AddComponent<SpriteRenderer>();
            sr.color = new Color(0.8f, 0.2f, 0.2f, 1f); // placeholder crimson

            var rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;

            var col = go.AddComponent<CircleCollider2D>();
            col.radius = 0.35f;

            var health = go.AddComponent<Health>();
            var soH = new SerializedObject(health);
            soH.FindProperty("maxHP").intValue = 60;
            soH.ApplyModifiedPropertiesWithoutUndo();

            var wound = go.AddComponent<TheWound>();
            var soW = new SerializedObject(wound);
            // Set enemyLayer to "Enemy" layer
            int enemyLayerMask = LayerMask.GetMask("Enemy");
            soW.FindProperty("enemyLayer").intValue = enemyLayerMask;
            soW.ApplyModifiedPropertiesWithoutUndo();

            PrefabUtility.SaveAsPrefabAsset(go, path);
            Object.DestroyImmediate(go);
            Debug.Log("[RIMA] Created TheWound prefab.");
        }

        [MenuItem("Tools/RIMA/Prefabs/Create BossAI_PenitentSovereign Prefab")]
        public static void CreateBossAIPrefab()
        {
            const string path = "Assets/Prefabs/Enemies/Boss/BossAI_PenitentSovereign.prefab";
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
            {
                Debug.Log("[RIMA] BossAI_PenitentSovereign prefab already exists — skipped.");
                return;
            }

            var go = new GameObject("BossAI_PenitentSovereign");
            go.tag = "Enemy";
            go.layer = LayerMask.NameToLayer("Enemy");

            // Sprite child (placeholder)
            var spriteChild = new GameObject("Sprite");
            spriteChild.transform.SetParent(go.transform, false);
            var sr = spriteChild.AddComponent<SpriteRenderer>();
            sr.color = new Color(0.9f, 0.7f, 0.3f, 1f); // sovereign gold
            sr.sortingOrder = 1;

            var rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            var col = go.AddComponent<CapsuleCollider2D>();
            col.size = new Vector2(1.2f, 1.8f);
            col.offset = new Vector2(0f, 0.1f);

            var health = go.AddComponent<Health>();
            var soH = new SerializedObject(health);
            soH.FindProperty("maxHP").intValue = 500;
            soH.ApplyModifiedPropertiesWithoutUndo();

            go.AddComponent<KnockbackReceiver>();
            go.AddComponent<StatusEffectSystem>();
            go.AddComponent<BossAI_PenitentSovereign>();

            PrefabUtility.SaveAsPrefabAsset(go, path);
            Object.DestroyImmediate(go);
            Debug.Log("[RIMA] Created BossAI_PenitentSovereign prefab.");
        }

        // ─── Death Screen UI ──────────────────────────────────────────────────

        [MenuItem("Tools/RIMA/Scene/Create Death Screen UI")]
        public static void CreateDeathScreenUI()
        {
            if (GameObject.Find("DeathScreenCanvas") != null)
            {
                Debug.Log("[RIMA] DeathScreenCanvas already in scene — skipped.");
                return;
            }

            // Canvas
            var canvasGo = new GameObject("DeathScreenCanvas");
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;

            var scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            canvasGo.AddComponent<GraphicRaycaster>();
            canvasGo.AddComponent<DeathScreenManager>();

            // Panel: full-screen dark overlay
            var panelGo = new GameObject("DeathScreen");
            panelGo.transform.SetParent(canvasGo.transform, false);

            var panelImg = panelGo.AddComponent<Image>();
            panelImg.color = new Color(0f, 0f, 0f, 0.85f);
            panelGo.AddComponent<CanvasGroup>();

            var panelRT = panelGo.GetComponent<RectTransform>();
            panelRT.anchorMin = Vector2.zero;
            panelRT.anchorMax = Vector2.one;
            panelRT.offsetMin = Vector2.zero;
            panelRT.offsetMax = Vector2.zero;

            // Title: "YOU DIED"
            var titleGo = new GameObject("DeathTitle");
            titleGo.transform.SetParent(panelGo.transform, false);
            var title = titleGo.AddComponent<TextMeshProUGUI>();
            title.text = "YOU DIED";
            title.alignment = TextAlignmentOptions.Center;
            title.fontSize = 72;
            title.fontStyle = FontStyles.Bold;
            title.color = Color.white;
            SetAnchors(titleGo, 0.2f, 0.58f, 0.8f, 0.78f);

            // Stats
            var statsGo = new GameObject("DeathStats");
            statsGo.transform.SetParent(panelGo.transform, false);
            var stats = statsGo.AddComponent<TextMeshProUGUI>();
            stats.text = "Room: 0\nKills: 0";
            stats.alignment = TextAlignmentOptions.Center;
            stats.fontSize = 32;
            stats.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            SetAnchors(statsGo, 0.3f, 0.42f, 0.7f, 0.57f);

            // Restart button
            var btnGo = new GameObject("RestartButton");
            btnGo.transform.SetParent(panelGo.transform, false);
            var btnImg = btnGo.AddComponent<Image>();
            btnImg.color = new Color(0.7f, 0.15f, 0.15f, 0.9f);
            var btn = btnGo.AddComponent<Button>();
            var colors = btn.colors;
            colors.highlightedColor = new Color(0.9f, 0.2f, 0.2f, 1f);
            btn.colors = colors;
            SetAnchors(btnGo, 0.35f, 0.28f, 0.65f, 0.40f);

            var btnTextGo = new GameObject("Text");
            btnTextGo.transform.SetParent(btnGo.transform, false);
            var btnText = btnTextGo.AddComponent<TextMeshProUGUI>();
            btnText.text = "TRY AGAIN";
            btnText.alignment = TextAlignmentOptions.Center;
            btnText.fontSize = 28;
            btnText.fontStyle = FontStyles.Bold;
            btnText.color = Color.white;
            SetAnchors(btnTextGo, 0f, 0f, 1f, 1f, fillParent: true);

            panelGo.SetActive(false);

            // Mark scene dirty
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene());

            Debug.Log("[RIMA] Created DeathScreen UI in scene. Save the scene to persist.");
        }

        // ─── Helpers ──────────────────────────────────────────────────────────

        private static void SetAnchors(GameObject go, float minX, float minY, float maxX, float maxY,
                                        bool fillParent = false)
        {
            var rt = go.GetComponent<RectTransform>();
            if (rt == null) rt = go.AddComponent<RectTransform>();

            rt.anchorMin = new Vector2(minX, minY);
            rt.anchorMax = new Vector2(maxX, maxY);

            if (fillParent)
            {
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
            }
            else
            {
                rt.sizeDelta = Vector2.zero;
                rt.anchoredPosition = Vector2.zero;
            }
        }
    }
}
