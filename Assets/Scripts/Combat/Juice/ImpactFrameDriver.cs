using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RIMA.Combat.Juice
{
    /// <summary>
    /// Chromatic impact frame — a brief full-screen cyan->purple flash on HEAVY hits
    /// (crit/finisher) and kills, to sell crunch (DESIGN_LOCK_DEMO_S6 §9).
    /// Cheap approximation of a true chromatic-aberration shader.
    /// TODO: replace with a URP renderer feature for real RGB split.
    /// Self-building (RuntimeInitializeOnLoad), unscaled-time so it reads during hitstop.
    /// Single instance, debounced, never blocks input.
    /// </summary>
    public class ImpactFrameDriver : MonoBehaviour
    {
        public static ImpactFrameDriver Instance { get; private set; }

        [SerializeField] private Color cyan = new Color(0f, 1f, 0.8f);            // #00FFCC
        [SerializeField] private Color purple = new Color(0.482f, 0.247f, 0.627f); // #7B3FA0
        [SerializeField] private float cyanAlpha = 0.18f;
        [SerializeField] private float purpleAlpha = 0.12f;
        [SerializeField] private float cyanDuration = 0.03f;   // ~2 frames @ 60fps
        [SerializeField] private float purpleDuration = 0.03f;
        [SerializeField] private float debounce = 0.12f;       // > total flash (~0.06s) so rapid crits can't flicker

        private Image flash;
        private float lastFlashTime = -10f;
        private Coroutine routine;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            if (Instance != null) return;
            var go = new GameObject("ImpactFrameDriver_Auto");
            go.AddComponent<ImpactFrameDriver>();
        }

        // Clear stale static when Enter-Play-Mode has domain reload disabled.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics() => Instance = null;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            BuildOverlay();
        }

        private void BuildOverlay()
        {
            var canvasGo = new GameObject("ImpactFrameCanvas", typeof(Canvas), typeof(CanvasGroup));
            canvasGo.transform.SetParent(transform, false);

            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 32000; // above everything

            var cg = canvasGo.GetComponent<CanvasGroup>();
            cg.interactable = false;
            cg.blocksRaycasts = false;

            var imgGo = new GameObject("Flash", typeof(Image));
            imgGo.transform.SetParent(canvasGo.transform, false);

            flash = imgGo.GetComponent<Image>();
            flash.raycastTarget = false;
            var rt = flash.rectTransform;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            flash.color = new Color(0f, 0f, 0f, 0f);
        }

        private void OnEnable()
        {
            CombatEventBus.OnHit += HandleHit;
            CombatEventBus.OnKill += HandleKill;
        }

        private void OnDisable()
        {
            CombatEventBus.OnHit -= HandleHit;
            CombatEventBus.OnKill -= HandleKill;
            if (routine != null) { StopCoroutine(routine); routine = null; }
            if (flash != null) flash.color = new Color(0f, 0f, 0f, 0f); // never leave the screen tinted
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void HandleHit(HitEvent e)
        {
            if (e.isCrit) TriggerFlash(); // crit == combo finisher in this codebase
        }

        private void HandleKill(KillEvent e)
        {
            TriggerFlash();
        }

        private void TriggerFlash()
        {
            if (Time.unscaledTime - lastFlashTime < debounce) return;
            lastFlashTime = Time.unscaledTime;
            if (routine != null) StopCoroutine(routine);
            routine = StartCoroutine(FlashRoutine());
        }

        private IEnumerator FlashRoutine()
        {
            if (flash == null) yield break;
            flash.color = new Color(cyan.r, cyan.g, cyan.b, cyanAlpha);
            yield return new WaitForSecondsRealtime(cyanDuration);
            flash.color = new Color(purple.r, purple.g, purple.b, purpleAlpha);
            yield return new WaitForSecondsRealtime(purpleDuration);
            flash.color = new Color(0f, 0f, 0f, 0f);
            routine = null;
        }
    }
}
