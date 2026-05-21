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
        [SerializeField] private AudioSource playerFootstepSource;

        private Canvas fxCanvas;
        private CanvasGroup canvasGroup;
        private Image  fadePanel;
        private bool   isFading;
        private bool previousFootstepMute;

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
            yield return FadeOut(fadeOutDuration);
            yield return new WaitForSeconds(holdDuration);
            // Execute while screen is black
            onBlack?.Invoke();
            // Fade back
            yield return FadeIn(fadeInDuration);
            isFading = false;
        }

        public IEnumerator FadeOut(float duration = 0.3f)
        {
            isFading = true;
            ResolveFootstepSource();
            previousFootstepMute = playerFootstepSource != null && playerFootstepSource.mute;
            if (playerFootstepSource != null) playerFootstepSource.mute = true;
            yield return FadeCoroutine(0f, 1f, 1f, 0.3f, duration);
        }

        public IEnumerator FadeIn(float duration = 0.3f)
        {
            yield return FadeCoroutine(1f, 0f, 0.3f, 1f, duration);
            if (playerFootstepSource != null) playerFootstepSource.mute = previousFootstepMute;
            isFading = false;
        }

        private IEnumerator FadeCoroutine(float from, float to, float audioFrom, float audioTo, float dur)
        {
            float t = 0f;
            SetFadeAlpha(from);
            AudioListener.volume = audioFrom;
            fadePanel.raycastTarget = true;
            while (t < dur)
            {
                t += Time.unscaledDeltaTime;
                float pct = dur <= 0f ? 1f : Mathf.Clamp01(t / dur);
                SetFadeAlpha(Mathf.Lerp(from, to, pct));
                AudioListener.volume = Mathf.Lerp(audioFrom, audioTo, pct);
                yield return null;
            }
            SetFadeAlpha(to);
            AudioListener.volume = audioTo;
            fadePanel.raycastTarget = (to > 0.01f);
        }

        private void SetFadeAlpha(float a)
        {
            if (canvasGroup == null) return;
            canvasGroup.alpha = a;
            canvasGroup.blocksRaycasts = a > 0.01f;
        }

        private void SetupOverlay()
        {
            var canvasGO = new GameObject("TransitionOverlay");
            canvasGO.transform.SetParent(transform, false);
            fxCanvas = canvasGO.AddComponent<Canvas>();
            fxCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            fxCanvas.sortingOrder = 100;
            canvasGroup = canvasGO.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            var scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            var panelGO = new GameObject("FadePanel");
            panelGO.transform.SetParent(canvasGO.transform, false);
            fadePanel = panelGO.AddComponent<Image>();
            fadePanel.color = Color.black;
            fadePanel.raycastTarget = false;
            var rt = panelGO.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        private void ResolveFootstepSource()
        {
            if (playerFootstepSource != null) return;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            AudioSource[] sources = player.GetComponentsInChildren<AudioSource>(true);
            for (int i = 0; i < sources.Length; i++)
            {
                if (sources[i] != null && sources[i].name.ToLowerInvariant().Contains("foot"))
                {
                    playerFootstepSource = sources[i];
                    return;
                }
            }
        }
    }
}
