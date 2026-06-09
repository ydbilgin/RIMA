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
            if (_subscribedManager != null)
                _subscribedManager.OnSecondaryClassSelected -= HandleSecondaryClassSelected;
            _subscribedManager = null;
        }

        private void Start() => SubscribeToManager();

        // PlayerClassManager is a SCENE object — it is destroyed and recreated on every
        // restart, while this banner is DDOL. Track the subscribed instance and re-subscribe
        // whenever it changes (a one-shot flag would go stale after the first scene reload).
        private PlayerClassManager _subscribedManager;

        private void Update()
        {
            SubscribeToManager();
        }

        private void SubscribeToManager()
        {
            var mgr = PlayerClassManager.Instance;
            if (mgr == null || ReferenceEquals(mgr, _subscribedManager)) return;
            if (_subscribedManager != null)
                _subscribedManager.OnSecondaryClassSelected -= HandleSecondaryClassSelected;
            mgr.OnSecondaryClassSelected += HandleSecondaryClassSelected;
            _subscribedManager = mgr;
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
