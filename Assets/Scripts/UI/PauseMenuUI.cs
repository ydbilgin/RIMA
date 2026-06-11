using TMPro;
using RIMA.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace RIMA
{
    /// <summary>
    /// Runtime-built pause menu. UIManager owns open state and timeScale.
    /// Open/Close only show/hide the panel; logical pauseOpen state lives in UIManager.
    /// </summary>
    public class PauseMenuUI : MonoBehaviour
    {
        public static PauseMenuUI Instance { get; private set; }

        private CanvasGroup canvasGroup;
        private bool built;

        // ─── Factory ────────────────────────────────────────────────────────

        public static PauseMenuUI EnsureInstance()
        {
            var existing = FindAnyObjectByType<PauseMenuUI>();
            if (existing != null) return existing;

            EnsureEventSystem();

            var canvasGo = new GameObject("[PauseMenuUI]");
            DontDestroyOnLoad(canvasGo);

            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1090; // below Settings (1100) so Settings layers on top

            var scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            canvasGo.AddComponent<GraphicRaycaster>();
            return canvasGo.AddComponent<PauseMenuUI>();
        }

        // ─── Lifecycle ───────────────────────────────────────────────────────

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            EnsureReady();
            Close();
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ─── Public API (called by UIManager) ───────────────────────────────

        /// <summary>Makes the panel visible. Does NOT touch timeScale or pauseOpen — UIManager owns those.</summary>
        public void Open()
        {
            EnsureReady();
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        /// <summary>Hides the panel. Does NOT touch timeScale or pauseOpen — UIManager owns those.</summary>
        public void Close()
        {
            EnsureReady();
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        // ─── Build ──────────────────────────────────────────────────────────

        private void EnsureReady()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            if (built) return;
            BuildUI();
            built = true;
        }

        private void BuildUI()
        {
            var root = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
            root.anchorMin = Vector2.zero;
            root.anchorMax = Vector2.one;
            root.offsetMin = root.offsetMax = Vector2.zero;

            // Dark full-screen overlay — absorbs clicks so game is not interactable
            var overlayGo = new GameObject("Overlay", typeof(RectTransform), typeof(Image));
            overlayGo.transform.SetParent(root, false);
            var overlayRt = overlayGo.GetComponent<RectTransform>();
            overlayRt.anchorMin = Vector2.zero;
            overlayRt.anchorMax = Vector2.one;
            overlayRt.offsetMin = overlayRt.offsetMax = Vector2.zero;
            var overlayImg = overlayGo.GetComponent<Image>();
            overlayImg.color = RimaUITheme.Act1Overlay;
            overlayImg.raycastTarget = true;

            // Center panel
            var panelGo = new GameObject("Panel", typeof(RectTransform), typeof(Image));
            panelGo.transform.SetParent(root, false);
            var panelRt = panelGo.GetComponent<RectTransform>();
            panelRt.anchorMin = new Vector2(0.5f, 0.5f);
            panelRt.anchorMax = new Vector2(0.5f, 0.5f);
            panelRt.pivot = new Vector2(0.5f, 0.5f);
            panelRt.anchoredPosition = Vector2.zero;
            panelRt.sizeDelta = new Vector2(356f, 366f);
            var panelImg = panelGo.GetComponent<Image>();
            panelImg.sprite = RimaUITheme.Act1PanelFrame;
            panelImg.type = Image.Type.Sliced;
            panelImg.color = new Color(RimaUITheme.Act1PanelFill.r, RimaUITheme.Act1PanelFill.g, RimaUITheme.Act1PanelFill.b, 0.96f);

            var innerGlowGo = new GameObject("VoidInnerGlow", typeof(RectTransform), typeof(Image));
            innerGlowGo.transform.SetParent(panelRt, false);
            var innerGlowRt = innerGlowGo.GetComponent<RectTransform>();
            innerGlowRt.anchorMin = Vector2.zero;
            innerGlowRt.anchorMax = Vector2.one;
            innerGlowRt.offsetMin = new Vector2(12f, 12f);
            innerGlowRt.offsetMax = new Vector2(-12f, -12f);
            var innerGlowImg = innerGlowGo.GetComponent<Image>();
            innerGlowImg.sprite = RimaUITheme.Act1PanelFrame;
            innerGlowImg.type = Image.Type.Sliced;
            innerGlowImg.color = new Color(RimaUITheme.Act1VoidPurple.r, RimaUITheme.Act1VoidPurple.g, RimaUITheme.Act1VoidPurple.b, 0.22f);
            innerGlowImg.raycastTarget = false;

            // Title
            var titleGo = new GameObject("Title", typeof(RectTransform));
            titleGo.transform.SetParent(panelRt, false);
            var titleRt = titleGo.GetComponent<RectTransform>();
            titleRt.anchorMin = new Vector2(0f, 1f);
            titleRt.anchorMax = new Vector2(1f, 1f);
            titleRt.pivot = new Vector2(0.5f, 1f);
            titleRt.anchoredPosition = new Vector2(0f, -20f);
            titleRt.sizeDelta = new Vector2(0f, 36f);
            var titleTmp = titleGo.AddComponent<TextMeshProUGUI>();
            titleTmp.text = "PAUSED";
            titleTmp.fontSize = 24f;
            titleTmp.fontStyle = FontStyles.Bold;
            titleTmp.color = RimaUITheme.Act1Ember;
            titleTmp.alignment = TextAlignmentOptions.Center;
            titleTmp.raycastTarget = false;

            var ruleGo = new GameObject("TitleRule", typeof(RectTransform), typeof(Image));
            ruleGo.transform.SetParent(panelRt, false);
            var ruleRt = ruleGo.GetComponent<RectTransform>();
            ruleRt.anchorMin = new Vector2(0.5f, 1f);
            ruleRt.anchorMax = new Vector2(0.5f, 1f);
            ruleRt.pivot = new Vector2(0.5f, 1f);
            ruleRt.anchoredPosition = new Vector2(0f, -58f);
            ruleRt.sizeDelta = new Vector2(246f, 6f);
            var ruleImg = ruleGo.GetComponent<Image>();
            ruleImg.sprite = RimaUITheme.Act1TitleRule;
            ruleImg.type = Image.Type.Sliced;
            ruleImg.color = new Color(RimaUITheme.Act1Ember.r, RimaUITheme.Act1Ember.g, RimaUITheme.Act1Ember.b, 0.68f);
            ruleImg.raycastTarget = false;

            // Buttons — vertical layout, centered
            float startY = -84f;
            float step   = 50f;

            AddButton(panelRt, "RESUME",      new Vector2(0f, startY + step * 0), OnResume);
            AddButton(panelRt, "SETTINGS",    new Vector2(0f, startY - step * 1), OnSettings);
            AddButton(panelRt, "SKILL CODEX", new Vector2(0f, startY - step * 2), OnSkillCodex);
            AddButton(panelRt, "MAIN MENU",   new Vector2(0f, startY - step * 3), OnMainMenu);
            AddButton(panelRt, "QUIT",         new Vector2(0f, startY - step * 4), OnQuit);
        }

        private void AddButton(RectTransform parent, string label, Vector2 pos, UnityEngine.Events.UnityAction onClick)
        {
            var btnGo = new GameObject($"Btn_{label}", typeof(RectTransform));
            btnGo.transform.SetParent(parent, false);
            var rt = btnGo.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 1f);
            rt.anchorMax = new Vector2(0.5f, 1f);
            rt.pivot = new Vector2(0.5f, 1f);
            rt.anchoredPosition = pos;
            rt.sizeDelta = new Vector2(236f, 38f);

            var img = btnGo.AddComponent<Image>();
            img.sprite = RimaUITheme.Act1ButtonFrame;
            img.type = Image.Type.Sliced;
            img.color = new Color(RimaUITheme.Act1ButtonFill.r, RimaUITheme.Act1ButtonFill.g, RimaUITheme.Act1ButtonFill.b, 0.96f);

            var outline = btnGo.AddComponent<Outline>();
            outline.effectColor = new Color(RimaUITheme.Act1Ember.r, RimaUITheme.Act1Ember.g, RimaUITheme.Act1Ember.b, 0.55f);
            outline.effectDistance = Vector2.one;

            var txtGo = new GameObject("Label", typeof(RectTransform));
            txtGo.transform.SetParent(rt, false);
            var txtRt = txtGo.GetComponent<RectTransform>();
            txtRt.anchorMin = Vector2.zero;
            txtRt.anchorMax = Vector2.one;
            txtRt.offsetMin = txtRt.offsetMax = Vector2.zero;

            var tmp = txtGo.AddComponent<TextMeshProUGUI>();
            tmp.text = label;
            tmp.fontSize = 14f;
            tmp.fontStyle = FontStyles.Bold;
            tmp.color = RimaUITheme.CharSelectParchment;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.raycastTarget = false;

            var btn = btnGo.AddComponent<Button>();
            btn.targetGraphic = img;
            btn.transition = Selectable.Transition.ColorTint;
            btn.colors = new ColorBlock
            {
                normalColor = new Color(RimaUITheme.Act1ButtonFill.r, RimaUITheme.Act1ButtonFill.g, RimaUITheme.Act1ButtonFill.b, 0.96f),
                highlightedColor = new Color(RimaUITheme.Act1Ember.r, RimaUITheme.Act1Ember.g, RimaUITheme.Act1Ember.b, 0.40f),
                pressedColor = new Color(RimaUITheme.Act1Ember.r, RimaUITheme.Act1Ember.g, RimaUITheme.Act1Ember.b, 0.58f),
                selectedColor = new Color(RimaUITheme.Act1Ember.r, RimaUITheme.Act1Ember.g, RimaUITheme.Act1Ember.b, 0.42f),
                disabledColor = new Color(RimaUITheme.Act1Slate.r, RimaUITheme.Act1Slate.g, RimaUITheme.Act1Slate.b, 0.30f),
                colorMultiplier = 1f,
                fadeDuration = 0.08f
            };
            btn.onClick.AddListener(onClick);

            btnGo.AddComponent<RimaUIButtonFeedback>();
        }

        // ─── Button actions ─────────────────────────────────────────────────

        private void OnResume()
        {
            if (UIManager.Instance != null)
                UIManager.Instance.ClosePause();
        }

        private void OnSettings()
        {
            if (UIManager.Instance != null)
                UIManager.Instance.OpenSettings();
        }

        private void OnSkillCodex()
        {
            if (UIManager.Instance != null)
                UIManager.Instance.OpenSkillCodex();
        }

        private void OnMainMenu()
        {
            if (UIManager.Instance != null)
                UIManager.Instance.ResumeGame();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

        private void OnQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        // ─── Util ───────────────────────────────────────────────────────────

        private static void EnsureEventSystem()
        {
            if (EventSystem.current != null) return;
            var go = new GameObject("EventSystem");
            DontDestroyOnLoad(go);
            go.AddComponent<EventSystem>();
            go.AddComponent<InputSystemUIInputModule>();
        }
    }
}
