using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Mob ölümünde:
    ///   (a) Sprite'ı ~0.2s'de Y-squash → 0 + alpha fade (bağımsız runner GO'da — mob destroy
    ///       olunca coroutine ölmesin).
    ///   (b) Mobun pozisyonunda bağımsız decal GO spawn (koyu yarı-saydam leke),
    ///       ~8s sonra fade-out + destroy.
    ///
    /// BaseMobBehavior.Awake tarafından otomatik eklenir.
    /// Guard flag ile EnemyAnimator'ın erken destroy path'inde çift-spawn engellenir.
    /// </summary>
    [DisallowMultipleComponent]
    public class MobDeathResidue : MonoBehaviour
    {
        // ── Config (tuning knobs — Inspector'dan override edilebilir) ──────────
        [Header("Death Squash")]
        [SerializeField] private float squashDuration = 0.2f;

        [Header("Ground Decal")]
        [SerializeField] private float decalScale       = 0.7f;
        [SerializeField] private float decalAlpha       = 0.35f;
        [SerializeField] private float decalLingerTime  = 8f;   // tam opaklıkta bekle
        [SerializeField] private float decalFadeDuration = 2f;  // sonra fade out

        // ── Runtime ───────────────────────────────────────────────────────────
        private Health health;
        private SpriteRenderer sr;
        private bool triggered; // çift-spawn guard

        // ── Static shared sprite for decal (lazily created, same approach as GroundBlobShadow) ──
        private static Sprite s_decalSprite;

        private void Awake()
        {
            health = GetComponent<Health>();
            sr     = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnEnable()
        {
            if (health != null) health.OnDeath.AddListener(OnDeath);
        }

        private void OnDisable()
        {
            if (health != null) health.OnDeath.RemoveListener(OnDeath);
        }

        // ──────────────────────────────────────────────────────────────────────
        //  DEATH HANDLER
        // ──────────────────────────────────────────────────────────────────────

        private void OnDeath()
        {
            if (triggered) return;
            triggered = true;

            Vector3 deathPos = transform.position;

            // (a) Squash + fade — bağımsız runner GO'da coroutine çalıştır
            //     (mob destroy olunca coroutine ölmesin)
            if (sr != null && sr.sprite != null)
            {
                // Snapshot sprite & material for the squash proxy
                StartDetachedCoroutine(SquashFadeRoutine(sr, deathPos));
            }

            // (b) Ground decal spawn
            SpawnDecal(deathPos);
        }

        // ──────────────────────────────────────────────────────────────────────
        //  (a) SQUASH + FADE
        // ──────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Detached coroutine runner: yeni bir GO oluşturup coroutine'i orada başlatır.
        /// Böylece mob destroy olunca coroutine yaşamaya devam eder.
        /// </summary>
        private static void StartDetachedCoroutine(IEnumerator routine)
        {
            var runner = new GameObject("MobDeathResidue_Runner");
            var host   = runner.AddComponent<CoroutineHost>();
            host.Run(routine, selfDestroy: true);
        }

        private IEnumerator SquashFadeRoutine(SpriteRenderer sourceRenderer, Vector3 position)
        {
            // Bağımsız sprite proxy oluştur (mob'un child'ı değil!)
            var proxyGO = new GameObject("DeathSquashProxy");
            proxyGO.transform.position = position;

            var proxySR = proxyGO.AddComponent<SpriteRenderer>();
            proxySR.sprite   = sourceRenderer.sprite;
            proxySR.color    = sourceRenderer.color;
            proxySR.flipX    = sourceRenderer.flipX;
            proxySR.flipY    = sourceRenderer.flipY;
            proxySR.sortingLayerName = sourceRenderer.sortingLayerName;
            proxySR.sortingOrder     = sourceRenderer.sortingOrder;
            if (sourceRenderer.sharedMaterial != null)
                proxySR.sharedMaterial = sourceRenderer.sharedMaterial;

            // Orijinal sprite'ı hemen gizle (EnemyAnimator de yapıyor ama güvence)
            sourceRenderer.enabled = false;

            // Squash + fade
            float elapsed = 0f;
            Vector3 baseScale = proxyGO.transform.localScale;
            Color baseColor   = proxySR.color;

            while (elapsed < squashDuration)
            {
                float t = Mathf.Clamp01(elapsed / squashDuration);
                float eased = t * t; // ease-in

                // Y-squash: 1 → 0,  X-stretch: 1 → ~1.3
                float scaleY = Mathf.Lerp(1f, 0f, eased);
                float scaleX = Mathf.Lerp(1f, 1.3f, eased);
                proxyGO.transform.localScale = new Vector3(
                    baseScale.x * scaleX,
                    baseScale.y * scaleY,
                    baseScale.z);

                // Alpha fade: 1 → 0
                proxySR.color = new Color(baseColor.r, baseColor.g, baseColor.b,
                    Mathf.Lerp(baseColor.a, 0f, eased));

                elapsed += Time.deltaTime;
                yield return null;
            }

            Object.Destroy(proxyGO);
        }

        // ──────────────────────────────────────────────────────────────────────
        //  (b) GROUND DECAL
        // ──────────────────────────────────────────────────────────────────────

        private void SpawnDecal(Vector3 position)
        {
            var decalGO = new GameObject("MobDeathDecal");
            decalGO.transform.position = position;
            decalGO.transform.localScale = new Vector3(decalScale, decalScale * 0.5f, 1f);

            var decalSR = decalGO.AddComponent<SpriteRenderer>();
            decalSR.sprite = GetDecalSprite();
            decalSR.color  = new Color(0.08f, 0.02f, 0.02f, decalAlpha); // koyu kırmızımsı-siyah

            // Sorting: Ground layer, shadow'un biraz üstünde ama entity'lerin altında
            decalSR.sortingLayerName = "Ground";
            decalSR.sortingOrder     = 125; // GroundBlobShadow = 120, entity'ler üstte

            // Unlit material — URP 2D lit sorun çıkarmasın
            var shader = Shader.Find("Sprites/Default");
            if (shader != null) decalSR.sharedMaterial = new Material(shader);

            // Fade out sonra destroy — detached coroutine
            StartDetachedCoroutine(DecalFadeRoutine(decalGO, decalSR));
        }

        private IEnumerator DecalFadeRoutine(GameObject decalGO, SpriteRenderer decalSR)
        {
            // Tam opaklıkta bekle
            yield return new WaitForSeconds(decalLingerTime);

            // Fade out
            Color startColor = decalSR.color;
            float elapsed = 0f;

            while (elapsed < decalFadeDuration)
            {
                float t = Mathf.Clamp01(elapsed / decalFadeDuration);
                decalSR.color = new Color(startColor.r, startColor.g, startColor.b,
                    Mathf.Lerp(startColor.a, 0f, t));
                elapsed += Time.deltaTime;
                yield return null;
            }

            Object.Destroy(decalGO);
        }

        // ──────────────────────────────────────────────────────────────────────
        //  SHARED DECAL SPRITE (lazy, runtime-generated)
        // ──────────────────────────────────────────────────────────────────────

        private static Sprite GetDecalSprite()
        {
            if (s_decalSprite != null) return s_decalSprite;

            // Soft radial blob — same technique as GroundBlobShadow but slightly dirtier edge
            const int sizePx = 64;
            var tex = new Texture2D(sizePx, sizePx, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Bilinear,
                wrapMode   = TextureWrapMode.Clamp
            };

            Vector2 center = new Vector2((sizePx - 1) * 0.5f, (sizePx - 1) * 0.5f);
            float radius = sizePx * 0.44f;

            // Deterministic pseudo-random for irregular edge
            System.Random rng = new System.Random(42);

            for (int y = 0; y < sizePx; y++)
            {
                for (int x = 0; x < sizePx; x++)
                {
                    float dist = Vector2.Distance(new Vector2(x, y), center);
                    float t = Mathf.Clamp01(dist / radius);

                    // Irregular edge: küçük noise ekle
                    float noise = (float)(rng.NextDouble() * 0.12);
                    float a = Mathf.Pow(Mathf.Clamp01(1f - t - noise * t), 1.8f);

                    tex.SetPixel(x, y, new Color(1f, 1f, 1f, a));
                }
            }

            tex.Apply();
            s_decalSprite = Sprite.Create(tex, new Rect(0, 0, sizePx, sizePx),
                new Vector2(0.5f, 0.5f), sizePx);
            s_decalSprite.name = "RIMA_MobDeath_Decal";
            return s_decalSprite;
        }

        // ──────────────────────────────────────────────────────────────────────
        //  COROUTINE HOST — lightweight MonoBehaviour for detached coroutines
        // ──────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Minimal MonoBehaviour sadece coroutine barındırmak için.
        /// selfDestroy = true ise coroutine bitince GO'yu siler.
        /// </summary>
        private class CoroutineHost : MonoBehaviour
        {
            public void Run(IEnumerator routine, bool selfDestroy)
            {
                StartCoroutine(Wrap(routine, selfDestroy));
            }

            private IEnumerator Wrap(IEnumerator routine, bool selfDestroy)
            {
                yield return routine;
                if (selfDestroy) Destroy(gameObject);
            }
        }
    }
}
