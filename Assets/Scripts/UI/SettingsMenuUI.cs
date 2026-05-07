using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// ESC settings menu — Ashen Glyph spec.
    /// Sections: Gameplay, Accessibility, Audio. Buttons: Resume, Quit to Menu.
    /// Auto-inits via RuntimeInitializeOnLoadMethod. Opened/closed by UIManager.
    /// </summary>
    public class SettingsMenuUI : MonoBehaviour
    {
        // ── State ────────────────────────────────────────────────────
        private bool isOpen;
        private CanvasGroup canvasGroup;

        // ── References (built at runtime) ────────────────────────────
        private Slider masterSlider, musicSlider, sfxSlider;

        // ── Prefs keys ───────────────────────────────────────────────
        private const string PrefAimMode     = "setting_aim_mode";      // 0=mouse, 1=last dir
        private const string PrefScreenShake = "setting_screen_shake";  // 1=on
        private const string PrefHitStop     = "setting_hit_stop";
        private const string PrefLowHpVig    = "setting_low_hp_vignette";
        private const string PrefDmgNumbers  = "setting_damage_numbers";
        private const string PrefChromatic   = "setting_chromatic";
        private const string PrefMaster      = "setting_vol_master";
        private const string PrefMusic       = "setting_vol_music";
        private const string PrefSfx         = "setting_vol_sfx";

        // ── Auto-init ────────────────────────────────────────────────

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoInit()
        {
            if (FindFirstObjectByType<SettingsMenuUI>() != null) return;

            var canvasGo = new GameObject("[SettingsMenuUI]");
            DontDestroyOnLoad(canvasGo);

            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1100;

            canvasGo.AddComponent<CanvasScaler>();
            canvasGo.AddComponent<GraphicRaycaster>();
            canvasGo.AddComponent<SettingsMenuUI>();
        }

        // ─── Lifecycle ──────────────────────────────────────────────

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

            BuildUI();
            Close();
        }

        // ─── Public API (called by UIManager) ───────────────────────

        public void Open()
        {
            isOpen = true;
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        public void Close()
        {
            isOpen = false;
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        public bool IsOpen => isOpen;

        // ─── Build ──────────────────────────────────────────────────

        private void BuildUI()
        {
            var root = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();

            // Full-screen dark overlay
            var overlay = MakeRect("Overlay", root);
            overlay.anchorMin = Vector2.zero;
            overlay.anchorMax = Vector2.one;
            overlay.offsetMin = overlay.offsetMax = Vector2.zero;
            var overlayImg = overlay.gameObject.AddComponent<Image>();
            overlayImg.color = RimaUITheme.OverlayDark;
            overlayImg.raycastTarget = true;

            // Center panel (stone)
            var panel = MakeRect("Panel", overlay);
            panel.anchorMin = new Vector2(0.5f, 0.5f);
            panel.anchorMax = new Vector2(0.5f, 0.5f);
            panel.pivot = new Vector2(0.5f, 0.5f);
            panel.sizeDelta = new Vector2(400f, 500f);
            panel.anchoredPosition = Vector2.zero;

            var panelImg = panel.gameObject.AddComponent<Image>();
            panelImg.sprite = RimaUITheme.ResourceFrame;
            panelImg.color = RimaUITheme.PanelTint;

            // Title
            var title = MakeTMP("Title", panel);
            var titleRt = title.GetComponent<RectTransform>();
            titleRt.anchorMin = new Vector2(0f, 1f);
            titleRt.anchorMax = new Vector2(1f, 1f);
            titleRt.pivot = new Vector2(0.5f, 1f);
            titleRt.anchoredPosition = new Vector2(0f, -16f);
            titleRt.sizeDelta = new Vector2(0f, 30f);
            title.text = "SETTINGS";
            title.fontSize = 20f;
            title.fontStyle = FontStyles.Bold;
            title.color = RimaUITheme.Gold;
            title.alignment = TextAlignmentOptions.Center;

            float y = -60f;

            // ── Gameplay Section ─────────────────────────────────────
            y = AddSectionHeader(panel, "GAMEPLAY", y);
            y = AddToggleRow(panel, "Aim Mode", "MOUSE | SON YON", PrefAimMode, 0, y);

            // ── Accessibility Section ────────────────────────────────
            y -= 12f;
            y = AddSectionHeader(panel, "ACCESSIBILITY", y);
            y = AddToggleRow(panel, "Screen Shake",     null, PrefScreenShake, 1, y);
            y = AddToggleRow(panel, "Hit Stop",          null, PrefHitStop,     1, y);
            y = AddToggleRow(panel, "Low HP Vignette",   null, PrefLowHpVig,    1, y);
            y = AddToggleRow(panel, "Damage Numbers",    null, PrefDmgNumbers,  1, y);
            y = AddToggleRow(panel, "Chromatic Aberration", null, PrefChromatic, 1, y);

            // ── Audio Section ────────────────────────────────────────
            y -= 12f;
            y = AddSectionHeader(panel, "AUDIO", y);
            masterSlider = AddSliderRow(panel, "Master", PrefMaster, ref y);
            musicSlider  = AddSliderRow(panel, "Music",  PrefMusic,  ref y);
            sfxSlider    = AddSliderRow(panel, "SFX",    PrefSfx,    ref y);

            // ── Buttons ──────────────────────────────────────────────
            y -= 20f;
            AddButton(panel, "RESUME", new Vector2(0f, y), OnResume);
            y -= 40f;
            AddButton(panel, "QUIT TO MENU", new Vector2(0f, y), OnQuitToMenu);
        }

        // ─── Section helpers ────────────────────────────────────────

        private float AddSectionHeader(RectTransform parent, string text, float y)
        {
            var tmp = MakeTMP($"Header_{text}", parent);
            var rt = tmp.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(1f, 1f);
            rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = new Vector2(20f, y);
            rt.sizeDelta = new Vector2(-40f, 18f);
            tmp.text = text;
            tmp.fontSize = 11f;
            tmp.fontStyle = FontStyles.Bold;
            tmp.color = RimaUITheme.Gold;
            return y - 22f;
        }

        private float AddToggleRow(RectTransform parent, string label, string customText, string prefKey, int defaultVal, float y)
        {
            var row = MakeRect($"Toggle_{label}", parent);
            row.anchorMin = new Vector2(0f, 1f);
            row.anchorMax = new Vector2(1f, 1f);
            row.pivot = new Vector2(0f, 1f);
            row.anchoredPosition = new Vector2(30f, y);
            row.sizeDelta = new Vector2(-60f, 22f);

            // Label
            var lbl = MakeTMP("Label", row);
            var lr = lbl.GetComponent<RectTransform>();
            lr.anchorMin = Vector2.zero;
            lr.anchorMax = new Vector2(0.6f, 1f);
            lr.offsetMin = lr.offsetMax = Vector2.zero;
            lbl.text = label;
            lbl.fontSize = 10f;
            lbl.color = new Color(0.8f, 0.85f, 0.9f, 0.9f);
            lbl.alignment = TextAlignmentOptions.Left;

            // Toggle button
            int val = PlayerPrefs.GetInt(prefKey, defaultVal);
            var btnGo = new GameObject("Btn", typeof(RectTransform));
            btnGo.transform.SetParent(row, false);
            var btnRt = btnGo.GetComponent<RectTransform>();
            btnRt.anchorMin = new Vector2(0.65f, 0f);
            btnRt.anchorMax = new Vector2(1f, 1f);
            btnRt.offsetMin = btnRt.offsetMax = Vector2.zero;

            var btnImg = btnGo.AddComponent<Image>();
            btnImg.color = val == 1
                ? new Color(RimaUITheme.Cyan.r, RimaUITheme.Cyan.g, RimaUITheme.Cyan.b, 0.3f)
                : new Color(0.2f, 0.2f, 0.25f, 0.5f);

            var btnTxt = MakeTMP("BtnTxt", btnGo.GetComponent<RectTransform>());
            var btRt = btnTxt.GetComponent<RectTransform>();
            btRt.anchorMin = Vector2.zero;
            btRt.anchorMax = Vector2.one;
            btRt.offsetMin = btRt.offsetMax = Vector2.zero;
            btnTxt.text = customText ?? (val == 1 ? "ON" : "OFF");
            btnTxt.fontSize = 9f;
            btnTxt.fontStyle = FontStyles.Bold;
            btnTxt.color = Color.white;
            btnTxt.alignment = TextAlignmentOptions.Center;

            var btn = btnGo.AddComponent<Button>();
            string key = prefKey;
            int dv = defaultVal;
            btn.onClick.AddListener(() =>
            {
                int cur = PlayerPrefs.GetInt(key, dv);
                int next = cur == 1 ? 0 : 1;
                PlayerPrefs.SetInt(key, next);
                btnTxt.text = customText ?? (next == 1 ? "ON" : "OFF");
                btnImg.color = next == 1
                    ? new Color(RimaUITheme.Cyan.r, RimaUITheme.Cyan.g, RimaUITheme.Cyan.b, 0.3f)
                    : new Color(0.2f, 0.2f, 0.25f, 0.5f);
            });

            return y - 26f;
        }

        private Slider AddSliderRow(RectTransform parent, string label, string prefKey, ref float y)
        {
            var row = MakeRect($"Slider_{label}", parent);
            row.anchorMin = new Vector2(0f, 1f);
            row.anchorMax = new Vector2(1f, 1f);
            row.pivot = new Vector2(0f, 1f);
            row.anchoredPosition = new Vector2(30f, y);
            row.sizeDelta = new Vector2(-60f, 22f);

            // Label
            var lbl = MakeTMP("Label", row);
            var lr = lbl.GetComponent<RectTransform>();
            lr.anchorMin = Vector2.zero;
            lr.anchorMax = new Vector2(0.35f, 1f);
            lr.offsetMin = lr.offsetMax = Vector2.zero;
            lbl.text = label;
            lbl.fontSize = 10f;
            lbl.color = new Color(0.8f, 0.85f, 0.9f, 0.9f);
            lbl.alignment = TextAlignmentOptions.Left;

            // Slider
            var sliderGo = new GameObject("Slider", typeof(RectTransform));
            sliderGo.transform.SetParent(row, false);
            var srt = sliderGo.GetComponent<RectTransform>();
            srt.anchorMin = new Vector2(0.38f, 0.2f);
            srt.anchorMax = new Vector2(0.85f, 0.8f);
            srt.offsetMin = srt.offsetMax = Vector2.zero;

            // Slider bg
            var bgGo = new GameObject("BG", typeof(RectTransform));
            bgGo.transform.SetParent(sliderGo.transform, false);
            var bgRt = bgGo.GetComponent<RectTransform>();
            bgRt.anchorMin = Vector2.zero;
            bgRt.anchorMax = Vector2.one;
            bgRt.offsetMin = bgRt.offsetMax = Vector2.zero;
            var bgImg = bgGo.AddComponent<Image>();
            bgImg.color = new Color(0.15f, 0.15f, 0.2f, 0.6f);

            // Fill
            var fillArea = MakeRect("FillArea", sliderGo.GetComponent<RectTransform>());
            fillArea.anchorMin = Vector2.zero;
            fillArea.anchorMax = Vector2.one;
            fillArea.offsetMin = fillArea.offsetMax = Vector2.zero;

            var fillGo = new GameObject("Fill", typeof(RectTransform));
            fillGo.transform.SetParent(fillArea, false);
            var fillRt = fillGo.GetComponent<RectTransform>();
            fillRt.anchorMin = Vector2.zero;
            fillRt.anchorMax = Vector2.one;
            fillRt.offsetMin = fillRt.offsetMax = Vector2.zero;
            var fillImg = fillGo.AddComponent<Image>();
            fillImg.color = RimaUITheme.Cyan;

            // Handle
            var handleArea = MakeRect("HandleArea", sliderGo.GetComponent<RectTransform>());
            handleArea.anchorMin = Vector2.zero;
            handleArea.anchorMax = Vector2.one;
            handleArea.offsetMin = handleArea.offsetMax = Vector2.zero;

            var handleGo = new GameObject("Handle", typeof(RectTransform));
            handleGo.transform.SetParent(handleArea, false);
            var handleRt = handleGo.GetComponent<RectTransform>();
            handleRt.sizeDelta = new Vector2(8f, 0f);
            var handleImg = handleGo.AddComponent<Image>();
            handleImg.color = Color.white;

            var slider = sliderGo.AddComponent<Slider>();
            slider.fillRect = fillRt;
            slider.handleRect = handleRt;
            slider.minValue = 0f;
            slider.maxValue = 100f;
            slider.wholeNumbers = true;
            slider.value = PlayerPrefs.GetInt(prefKey, 80);

            // Value label
            var valTmp = MakeTMP("Val", row);
            var vr = valTmp.GetComponent<RectTransform>();
            vr.anchorMin = new Vector2(0.87f, 0f);
            vr.anchorMax = new Vector2(1f, 1f);
            vr.offsetMin = vr.offsetMax = Vector2.zero;
            valTmp.text = slider.value.ToString("0");
            valTmp.fontSize = 10f;
            valTmp.fontStyle = FontStyles.Bold;
            valTmp.color = Color.white;
            valTmp.alignment = TextAlignmentOptions.Center;

            string key = prefKey;
            slider.onValueChanged.AddListener(v =>
            {
                PlayerPrefs.SetInt(key, (int)v);
                valTmp.text = v.ToString("0");
            });

            y -= 26f;
            return slider;
        }

        private void AddButton(RectTransform parent, string text, Vector2 pos, UnityEngine.Events.UnityAction onClick)
        {
            var btnGo = new GameObject($"Btn_{text}", typeof(RectTransform));
            btnGo.transform.SetParent(parent, false);
            var rt = btnGo.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 1f);
            rt.anchorMax = new Vector2(0.5f, 1f);
            rt.pivot = new Vector2(0.5f, 1f);
            rt.anchoredPosition = pos;
            rt.sizeDelta = new Vector2(200f, 32f);

            var img = btnGo.AddComponent<Image>();
            img.sprite = RimaUITheme.ResourceFrame;
            img.color = new Color(RimaUITheme.Cyan.r, RimaUITheme.Cyan.g, RimaUITheme.Cyan.b, 0.25f);

            var tmp = MakeTMP("Label", rt);
            var lr = tmp.GetComponent<RectTransform>();
            lr.anchorMin = Vector2.zero;
            lr.anchorMax = Vector2.one;
            lr.offsetMin = lr.offsetMax = Vector2.zero;
            tmp.text = text;
            tmp.fontSize = 12f;
            tmp.fontStyle = FontStyles.Bold;
            tmp.color = Color.white;
            tmp.alignment = TextAlignmentOptions.Center;

            var btn = btnGo.AddComponent<Button>();
            btn.onClick.AddListener(onClick);
        }

        // ─── Actions ────────────────────────────────────────────────

        private void OnResume()
        {
            if (UIManager.Instance != null)
                UIManager.Instance.CloseSettings();
        }

        private void OnQuitToMenu()
        {
            // UIManager owns timeScale -- close the overlay so it restores scale before scene load
            if (UIManager.Instance != null) UIManager.Instance.CloseSettings();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

        // ─── Util ───────────────────────────────────────────────────

        private static RectTransform MakeRect(string name, RectTransform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return go.GetComponent<RectTransform>();
        }

        private static TextMeshProUGUI MakeTMP(string name, RectTransform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.raycastTarget = false;
            return tmp;
        }
    }
}
