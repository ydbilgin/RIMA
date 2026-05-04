using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using TMPro;

namespace RIMA
{
    public class MainMenuScreen : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoInit()
        {
            if (Object.FindFirstObjectByType<MainMenuScreen>() != null) return;
            EnsureEventSystem();
            var go = new GameObject("[MainMenuScreen]");
            DontDestroyOnLoad(go);
            go.AddComponent<MainMenuScreen>();
        }

        private static void EnsureEventSystem()
        {
            if (Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() != null) return;
            var es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
            es.AddComponent<InputSystemUIInputModule>();
            Object.DontDestroyOnLoad(es);
        }

        private void Start()
        {
            Time.timeScale = 0f;
            BuildUI();
        }

        private void BuildUI()
        {
            var canvasGO = new GameObject("MainMenuCanvas");
            canvasGO.transform.SetParent(transform);
            var canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode   = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
            var scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode         = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight  = 0.5f;
            canvasGO.AddComponent<GraphicRaycaster>();

            var root = MakeRect("Root", canvasGO.transform, Vector2.zero, Vector2.one);
            var bg   = root.AddComponent<Image>();
            bg.sprite = RimaUITheme.MenuDungeonBackground;
            bg.color = Color.white;
            bg.preserveAspect = true;

            var shade = MakeRect("Shade", root.transform, Vector2.zero, Vector2.one);
            shade.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.34f);

            var menuPanel = MakeRect("MenuPanel", root.transform,
                new Vector2(0.075f, 0.18f), new Vector2(0.405f, 0.66f));
            var panelImage = menuPanel.AddComponent<Image>();
            panelImage.sprite = RimaUITheme.SmallPanelFrame;
            panelImage.color = new Color(1f, 1f, 1f, 0.94f);

            // Title
            var titleGO = MakeRect("Title", menuPanel.transform,
                new Vector2(0.10f, 0.54f), new Vector2(0.92f, 0.86f));
            var title = titleGO.AddComponent<TextMeshProUGUI>();
            title.text      = "RIMA";
            title.fontSize  = 74;
            title.fontStyle = FontStyles.Bold;
            title.color     = Color.white;
            title.alignment = TextAlignmentOptions.Left;
            title.raycastTarget = false;

            // Subtitle
            var subGO = MakeRect("Subtitle", menuPanel.transform,
                new Vector2(0.105f, 0.42f), new Vector2(0.92f, 0.54f));
            var sub = subGO.AddComponent<TextMeshProUGUI>();
            sub.text      = "THE SEAL BENEATH THE KEEP";
            sub.fontSize  = 18;
            sub.color     = new Color(0.55f, 0.60f, 0.75f, 1f);
            sub.alignment = TextAlignmentOptions.Left;
            sub.raycastTarget = false;

            // PLAY button
            var btnGO = MakeRect("PlayBtn", menuPanel.transform,
                new Vector2(0.10f, 0.16f), new Vector2(0.58f, 0.30f));
            var btnImg = btnGO.AddComponent<Image>();
            btnImg.sprite = RimaUITheme.PromptFrame;
            btnImg.color = RimaUITheme.PanelTint;
            var btn    = btnGO.AddComponent<Button>();
            var colors = btn.colors;
            colors.normalColor      = new Color(0.22f, 0.44f, 1.00f, 1f);
            colors.highlightedColor = new Color(0.35f, 0.58f, 1.00f, 1f);
            colors.pressedColor     = new Color(0.12f, 0.28f, 0.80f, 1f);
            btn.colors              = colors;

            var lblGO = MakeRect("Label", btnGO.transform, Vector2.zero, Vector2.one);
            var lbl   = lblGO.AddComponent<TextMeshProUGUI>();
            lbl.text             = "OYNA";
            lbl.fontSize         = 22;
            lbl.fontStyle        = FontStyles.Bold;
            lbl.color            = Color.white;
            lbl.alignment        = TextAlignmentOptions.Center;
            lbl.raycastTarget    = false;

            // Version
            var verGO = MakeRect("Version", menuPanel.transform,
                new Vector2(0.10f, 0.05f), new Vector2(0.70f, 0.11f));
            var ver = verGO.AddComponent<TextMeshProUGUI>();
            ver.text          = "S43 Dev Build";
            ver.fontSize      = 14;
            ver.color         = new Color(0.35f, 0.38f, 0.45f, 1f);
            ver.alignment     = TextAlignmentOptions.BottomLeft;
            ver.raycastTarget = false;

            btn.onClick.AddListener(OnPlayClicked);
        }

        private void OnPlayClicked()
        {
            Destroy(gameObject);
            var go = new GameObject("[CharacterSelectScreen]");
            DontDestroyOnLoad(go);
            go.AddComponent<CharacterSelectScreen>();
        }

        private static GameObject MakeRect(string name, Transform parent, Vector2 ancMin, Vector2 ancMax)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt       = go.GetComponent<RectTransform>();
            rt.anchorMin = ancMin;
            rt.anchorMax = ancMax;
            rt.offsetMin = rt.offsetMax = Vector2.zero;
            return go;
        }
    }
}
