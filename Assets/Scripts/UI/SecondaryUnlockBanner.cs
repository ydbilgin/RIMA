using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RIMA
{
    /// <summary>
    /// Displays a brief "İKİNCİL SINIF AÇILDI" banner when the player selects a secondary class.
    /// Bootstrap: [RuntimeInitializeOnLoadMethod] — no scene wiring required.
    /// Design rule: 'UI yoktur bilgi vardır' — text-only, no panel/box, outline only.
    /// </summary>
    public class SecondaryUnlockBanner : MonoBehaviour
    {
        private static SecondaryUnlockBanner _instance;

        private const float DisplayDuration = 2.5f;
        private const int SortingOrder = 195; // above gameplay, below DemoCompleteOverlay (200)

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            if (_instance != null) return;
            var go = new GameObject("[SecondaryUnlockBanner]");
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<SecondaryUnlockBanner>();
        }

        private void Awake()
        {
            if (_instance != null && _instance != this) { Destroy(gameObject); return; }
            _instance = this;
        }

        private void OnEnable() => SubscribeToManager();

        private void OnDisable()
        {
            if (PlayerClassManager.Instance != null)
                PlayerClassManager.Instance.OnSecondaryClassSelected -= HandleSecondaryClassSelected;
            _subscribed = false;
        }

        private void Start() => SubscribeToManager();

        private bool _subscribed;

        // Try to subscribe each frame until the manager arrives (late bootstrap scenarios).
        private void Update()
        {
            if (!_subscribed) SubscribeToManager();
        }

        private void SubscribeToManager()
        {
            if (_subscribed) return;
            if (PlayerClassManager.Instance == null) return;
            PlayerClassManager.Instance.OnSecondaryClassSelected += HandleSecondaryClassSelected;
            _subscribed = true;
        }

        private void HandleSecondaryClassSelected(ClassType type)
        {
            StartCoroutine(ShowBanner(type));
        }

        private IEnumerator ShowBanner(ClassType type)
        {
            var canvasGo = new GameObject("SecondaryUnlockBanner_Canvas");
            canvasGo.transform.SetParent(transform, false);
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode   = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = SortingOrder;
            canvasGo.AddComponent<CanvasScaler>();

            var textGo = new GameObject("BannerText", typeof(RectTransform));
            textGo.transform.SetParent(canvas.transform, false);

            var tmp = textGo.AddComponent<TextMeshProUGUI>();
            tmp.text      = $"İKİNCİL SINIF AÇILDI\n<size=60%>{type.ToString().ToUpper()}</size>";
            tmp.fontSize  = 52f;
            tmp.fontStyle = FontStyles.Bold;
            tmp.color     = RimaUITheme.ClassAccent(type);
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.outlineWidth = 0.28f;
            tmp.outlineColor = new Color32(0, 0, 0, 230);
            tmp.raycastTarget = false;

            var rt = tmp.rectTransform;
            rt.anchorMin = new Vector2(0f, 0.62f);
            rt.anchorMax = new Vector2(1f, 0.82f);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            // Unscaled time so the banner survives timeScale changes around class selection.
            float elapsed = 0f;
            const float fadeLen = 0.4f;
            while (elapsed < DisplayDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float fadeStart = DisplayDuration - fadeLen;
                if (elapsed > fadeStart)
                {
                    Color c = tmp.color;
                    c.a = 1f - Mathf.Clamp01((elapsed - fadeStart) / fadeLen);
                    tmp.color = c;
                }
                yield return null;
            }

            Destroy(canvasGo);
        }
    }
}
