using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RIMA
{
    /// <summary>
    /// Screen fade overlay for room transitions.
    /// Creates a full-screen black panel and provides fade coroutines.
    /// Singleton — lives on Systems GO alongside RuntimeRoomManager.
    /// </summary>
    public class RoomTransitionFX : MonoBehaviour
    {
        public static RoomTransitionFX Instance { get; private set; }

        [Header("Fade Settings")]
        [SerializeField] private float fadeOutDuration = 0.25f;
        [SerializeField] private float fadeInDuration  = 0.35f;
        [SerializeField] private float holdDuration    = 0.15f;

        private Canvas fxCanvas;
        private Image  fadePanel;
        private bool   isFading;

        public bool IsFading => isFading;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(this); return; }
            Instance = this;
            SetupOverlay();
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        /// <summary>Full transition: fade out, callback while black, fade in.</summary>
        public Coroutine DoTransition(System.Action onBlack = null)
        {
            return StartCoroutine(TransitionCoroutine(onBlack));
        }

        private IEnumerator TransitionCoroutine(System.Action onBlack)
        {
            isFading = true;
            // Fade to black
            yield return FadeCoroutine(0f, 1f, fadeOutDuration);
            yield return new WaitForSeconds(holdDuration);
            // Execute while screen is black
            onBlack?.Invoke();
            // Fade back
            yield return FadeCoroutine(1f, 0f, fadeInDuration);
            isFading = false;
        }

        private IEnumerator FadeCoroutine(float from, float to, float dur)
        {
            float t = 0f;
            SetFadeAlpha(from);
            fadePanel.raycastTarget = true;
            while (t < dur)
            {
                t += Time.unscaledDeltaTime;
                SetFadeAlpha(Mathf.Lerp(from, to, Mathf.Clamp01(t / dur)));
                yield return null;
            }
            SetFadeAlpha(to);
            fadePanel.raycastTarget = (to > 0.01f);
        }

        private void SetFadeAlpha(float a)
        {
            if (fadePanel == null) return;
            fadePanel.color = new Color(0f, 0f, 0f, a);
        }

        private void SetupOverlay()
        {
            var canvasGO = new GameObject("TransitionOverlay");
            canvasGO.transform.SetParent(transform, false);
            fxCanvas = canvasGO.AddComponent<Canvas>();
            fxCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            fxCanvas.sortingOrder = 100;
            var scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            var panelGO = new GameObject("FadePanel");
            panelGO.transform.SetParent(canvasGO.transform, false);
            fadePanel = panelGO.AddComponent<Image>();
            fadePanel.color = new Color(0f, 0f, 0f, 0f);
            fadePanel.raycastTarget = false;
            var rt = panelGO.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
    }
}
