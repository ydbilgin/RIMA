using System.Collections;
using RIMA.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RIMA
{
    /// <summary>
    /// T6 Boss-A — 1.5s intro sekansı.
    /// PenitentSovereign.StartBossIntro() çağırır; birincil aktör bu sınıftır.
    ///
    /// Sekans:
    ///   0.0s  Arena hafif kararır (overlay fade-in)
    ///   0.2s  Boss adı büyük tipografiyle belirir (fade-in)
    ///   0.5s  Ritual circle pulse başlar (zemin decal, asset: Art/Boss/decal_boss_ritual_circle.png)
    ///   0.8s  Intro seal ring pulse (Art/Boss/boss_intro_seal_ring.png)
    ///   1.0s  HP bar slide-in
    ///   1.5s  Sekans biter; boss loopa geçer
    ///
    /// 1.5s kısa → skip edilemez (spec).
    /// </summary>
    public class BossIntroController : MonoBehaviour
    {
        private const float IntroDuration = 1.5f;

        // ─── Visual Config ────────────────────────────────────────────────────
        private static readonly Color DimColor   = new Color(0f, 0f, 0f, 0.52f);
        private static readonly Color TitleColor = new Color(0.90f, 0.84f, 0.70f, 1f);
        private static readonly Color SealColor  = new Color(0.28f, 0.88f, 1f,   0.80f);   // cyan

        // ─── State ────────────────────────────────────────────────────────────
        private bool complete;
        private bool aborted;

        // MAJOR-2 fix: track canvas created by EnsureCanvas so it can be cleaned up.
        private Canvas createdCanvas;

        // ─── Public Factory ───────────────────────────────────────────────────

        /// <summary>
        /// Boss sahnede oluştuğunda bu metot çağrılır.
        /// Sekans tamamlandığında <paramref name="onComplete"/> çağrılır.
        /// MAJOR-1 fix: controller GO boss'un child'ı olur → boss Destroy edildiğinde ikisi de ölür.
        /// Ayrıca boss Health.OnDeath'e abone olup erken abort.
        /// </summary>
        public static BossIntroController Begin(
            string bossName,
            Transform bossTransform,
            BossHealthBar healthBar,
            System.Action onComplete)
        {
            var go = new GameObject("BossIntroController");

            // MAJOR-1: parent under boss so Destroy(boss) kills this GO too.
            if (bossTransform != null)
                go.transform.SetParent(bossTransform, false);

            var ctrl = go.AddComponent<BossIntroController>();

            // MAJOR-1: subscribe to boss OnDeath for early abort.
            if (bossTransform != null)
            {
                var bossHealth = bossTransform.GetComponent<Health>();
                if (bossHealth != null)
                    bossHealth.OnDeath.AddListener(ctrl.AbortIntro);
            }

            ctrl.StartCoroutine(ctrl.RunIntro(bossName, bossTransform, healthBar, onComplete));
            return ctrl;
        }

        // MAJOR-1: called by boss Health.OnDeath listener to abort mid-intro.
        private void AbortIntro()
        {
            if (aborted || complete) return;
            aborted = true;
            StopAllCoroutines();
            CleanupCanvas();
            Destroy(gameObject);
        }

        // ─── Intro Sequence ───────────────────────────────────────────────────

        private IEnumerator RunIntro(
            string bossName,
            Transform bossTransform,
            BossHealthBar healthBar,
            System.Action onComplete)
        {
            // ── BossIntro SFX ────────────────────────────────────────────────
            AudioManager.Play(Sfx.BossIntro);

            // ── ADIM 0: Dim overlay ──────────────────────────────────────────
            Canvas canvas = EnsureCanvas();
            Image dimOverlay = CreateImage("DimOverlay", canvas.transform, Color.clear);
            Stretch(dimOverlay.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            dimOverlay.raycastTarget = false;

            yield return FadeImage(dimOverlay, Color.clear, DimColor, 0.25f);

            // ── ADIM 1: Boss adı büyük tipografiyle ──────────────────────────
            TextMeshProUGUI titleText = CreateTitleText(canvas.transform, bossName);
            yield return FadeText(titleText, 0f, 1f, 0.30f);
            yield return new WaitForSecondsRealtime(0.25f);

            // ── ADIM 2: Ritual circle pulse (zemin decal) ─────────────────────
            if (bossTransform != null)
            {
                StartCoroutine(PulseDecal("Boss/decal_boss_ritual_circle", bossTransform.position, 0.7f));
            }

            yield return new WaitForSecondsRealtime(0.20f);

            // ── ADIM 3: Intro seal ring pulse ─────────────────────────────────
            if (bossTransform != null)
            {
                StartCoroutine(PulseSealRing("Boss/boss_intro_seal_ring", bossTransform, 0.5f));
            }

            yield return new WaitForSecondsRealtime(0.15f);

            // ── ADIM 4: HP bar slide-in ───────────────────────────────────────
            healthBar?.Show();

            yield return new WaitForSecondsRealtime(0.20f);

            // ── Cleanup dim + title (fade-out) ────────────────────────────────
            StartCoroutine(FadeImage(dimOverlay, DimColor, Color.clear, 0.30f));
            StartCoroutine(FadeText(titleText, 1f, 0f, 0.30f));

            yield return new WaitForSecondsRealtime(0.35f);

            complete = true;
            Destroy(dimOverlay.gameObject);
            Destroy(titleText.gameObject);
            CleanupCanvas();
            Destroy(gameObject);

            onComplete?.Invoke();
        }

        // ─── Decal / Seal ─────────────────────────────────────────────────────

        private IEnumerator PulseDecal(string resourcePath, Vector3 worldPos, float duration)
        {
            Sprite spr = Resources.Load<Sprite>(resourcePath);
            if (spr == null) yield break;

            var go = new GameObject("BossRitualCircle");
            go.transform.position = worldPos + Vector3.forward * 0.01f;
            go.transform.localScale = Vector3.one * 3f;

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = spr;
            sr.sortingLayerName = "Ground";
            sr.sortingOrder = 5;
            sr.color = new Color(SealColor.r, SealColor.g, SealColor.b, 0f);

            float half = duration * 0.5f;
            yield return PulseSRAlpha(sr, 0f, 0.60f, half);
            yield return PulseSRAlpha(sr, 0.60f, 0f, half);
            Destroy(go);
        }

        private IEnumerator PulseSealRing(string resourcePath, Transform parent, float duration)
        {
            Sprite spr = Resources.Load<Sprite>(resourcePath);
            if (spr == null) yield break;

            var go = new GameObject("BossIntroSealRing");
            go.transform.SetParent(parent, false);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one * 2.5f;

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = spr;
            sr.sortingLayerName = "Entities";
            sr.sortingOrder = -1;
            sr.color = new Color(SealColor.r, SealColor.g, SealColor.b, 0f);

            float half = duration * 0.5f;
            yield return PulseSRAlpha(sr, 0f, 0.80f, half);
            yield return PulseSRAlpha(sr, 0.80f, 0f, half);
            Destroy(go);
        }

        // ─── Helpers ──────────────────────────────────────────────────────────

        private static IEnumerator FadeImage(Image img, Color from, Color to, float t)
        {
            float elapsed = 0f;
            while (elapsed < t)
            {
                elapsed += Time.unscaledDeltaTime;
                img.color = Color.Lerp(from, to, elapsed / t);
                yield return null;
            }
            img.color = to;
        }

        private static IEnumerator FadeText(TextMeshProUGUI txt, float fromAlpha, float toAlpha, float t)
        {
            float elapsed = 0f;
            Color c = txt.color;
            while (elapsed < t)
            {
                elapsed += Time.unscaledDeltaTime;
                c.a = Mathf.Lerp(fromAlpha, toAlpha, elapsed / t);
                txt.color = c;
                yield return null;
            }
            c.a = toAlpha;
            txt.color = c;
        }

        private static IEnumerator PulseSRAlpha(SpriteRenderer sr, float from, float to, float t)
        {
            float elapsed = 0f;
            Color c = sr.color;
            while (elapsed < t && sr != null)
            {
                elapsed += Time.unscaledDeltaTime;
                c.a = Mathf.Lerp(from, to, elapsed / t);
                sr.color = c;
                yield return null;
            }
            if (sr != null) { c.a = to; sr.color = c; }
        }

        // MAJOR-2: non-static so we can track the canvas we created.
        private Canvas EnsureCanvas()
        {
            Canvas[] all = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
            foreach (var c in all)
                if (c.renderMode == RenderMode.ScreenSpaceOverlay && c.sortingOrder >= 150)
                    return c;   // reused existing canvas — we do NOT own it, do not destroy it.

            var go = new GameObject("BossIntroCanvas");
            var canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 180;
            go.AddComponent<CanvasScaler>();
            go.AddComponent<GraphicRaycaster>();
            createdCanvas = canvas;   // MAJOR-2: remember for cleanup.
            return canvas;
        }

        // MAJOR-2: destroy the canvas we created (not one we merely reused).
        private void CleanupCanvas()
        {
            if (createdCanvas != null)
            {
                Destroy(createdCanvas.gameObject);
                createdCanvas = null;
            }
        }

        private static TextMeshProUGUI CreateTitleText(Transform parent, string bosName)
        {
            var go = new GameObject("BossTitleText", typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.05f, 0.55f);
            rt.anchorMax = new Vector2(0.95f, 0.75f);
            rt.offsetMin = rt.offsetMax = Vector2.zero;

            var txt = go.AddComponent<TextMeshProUGUI>();
            txt.text = bosName;
            txt.fontSize = 52f;
            txt.fontStyle = FontStyles.Bold;
            txt.alignment = TextAlignmentOptions.Center;
            txt.color = new Color(TitleColor.r, TitleColor.g, TitleColor.b, 0f);
            txt.raycastTarget = false;
            return txt;
        }

        private static Image CreateImage(string name, Transform parent, Color color)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var img = go.AddComponent<Image>();
            img.color = color;
            return img;
        }

        private static void Stretch(RectTransform rt, Vector2 min, Vector2 max, Vector2 offsetMin, Vector2 offsetMax)
        {
            rt.anchorMin = min;
            rt.anchorMax = max;
            rt.offsetMin = offsetMin;
            rt.offsetMax = offsetMax;
        }
    }
}
