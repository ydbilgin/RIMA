using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// ESC ile açılan Ayarlar/Pause menüsü.
    /// Tuş atamaları değiştirilebilir (skill Q/E/R/F).
    /// RIMA rift paleti.
    /// </summary>
    public class SettingsMenuUI : MonoBehaviour
    {
        [Header("Style")]
        [SerializeField] private Color panelBg     = new Color(0.025f, 0.027f, 0.030f, 0.92f);
        [SerializeField] private Color headerColor  = new Color(0.66f, 0.82f, 0.88f);
        [SerializeField] private Color btnNormal    = new Color(0.08f, 0.095f, 0.115f, 0.96f);
        [SerializeField] private Color btnHover     = new Color(0.15f, 0.22f, 0.27f, 1f);
        [SerializeField] private Color textColor    = new Color(0.88f, 0.90f, 0.88f);
        [SerializeField] private Color accentColor  = new Color(0.70f, 0.56f, 0.30f);
        [SerializeField] private Color frameColor   = new Color(0.23f, 0.20f, 0.19f, 0.96f);
        [SerializeField] private Color cardColor    = new Color(0.055f, 0.060f, 0.070f, 0.94f);

        private GameObject root;
        private bool isOpen;
        private float previousTimeScale = 1f;
        private InputAction escAction;

        // Rebind state
        private int rebindSlot = -1;
        private TextMeshProUGUI[] keyTexts = new TextMeshProUGUI[4];
        private TextMeshProUGUI rebindPrompt;
        private TextMeshProUGUI attackAimText;
        private TextMeshProUGUI dashModeText;

        private static readonly string[] SlotLabels = { "Skill 1", "Skill 2", "Skill 3", "Skill 4" };

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoInit()
        {
            if (FindFirstObjectByType<SettingsMenuUI>() != null) return;

            EnsureEventSystem();

            var canvasGo = new GameObject("[SettingsMenuUI]");
            DontDestroyOnLoad(canvasGo);

            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 500;

            var scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            canvasGo.AddComponent<GraphicRaycaster>();
            canvasGo.AddComponent<SettingsMenuUI>();
        }

        private static void EnsureEventSystem()
        {
            if (FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() != null) return;

            var es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
            es.AddComponent<InputSystemUIInputModule>();
            DontDestroyOnLoad(es);
        }

        private void Awake()
        {
            escAction = new InputAction("Escape", InputActionType.Button);
            escAction.AddBinding("<Keyboard>/escape");
        }

        private void OnEnable()
        {
            escAction.Enable();
            escAction.performed += OnEsc;
        }

        private void OnDisable()
        {
            escAction.performed -= OnEsc;
            escAction.Disable();
        }

        private void OnEsc(InputAction.CallbackContext ctx)
        {
            if (rebindSlot >= 0) return; // Rebind modundayken ESC ile çıkma
            if (isOpen) Close();
            else Open();
        }

        public void Open()
        {
            if (root == null) BuildMenu();
            root.SetActive(true);
            isOpen = true;
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            RefreshKeyTexts();
        }

        public void Close()
        {
            if (root != null) root.SetActive(false);
            isOpen = false;
            rebindSlot = -1;
            Time.timeScale = previousTimeScale;
        }

        private void Update()
        {
            // Tuş yakalama modu
            if (rebindSlot >= 0 && Keyboard.current != null)
            {
                foreach (var key in Keyboard.current.allKeys)
                {
                    if (key.wasPressedThisFrame)
                    {
                        string newKey = key.keyCode.ToString().ToLower();

                        // Bazı tuşları engelle
                        if (newKey == "escape") { rebindSlot = -1; RefreshKeyTexts(); return; }

                        KeyBindManager.SetKey(rebindSlot, newKey);
                        Debug.Log($"[Settings] Slot {rebindSlot} → {newKey.ToUpper()}");

                        rebindSlot = -1;
                        RefreshKeyTexts();

                        RefreshPlayerSkillBindings();

                        // SkillBar etiketlerini güncelle
                        var bar = FindAnyObjectByType<SkillBarUI>();
                        bar?.RefreshKeyLabels();
                        return;
                    }
                }
            }
        }

        // ─── Menu Build ──────────────────────────────────

        private void BuildMenu()
        {
            root = new GameObject("SettingsMenu", typeof(RectTransform));
            root.transform.SetParent(transform, false);
            var rootRt = root.GetComponent<RectTransform>();
            rootRt.anchorMin = Vector2.zero;
            rootRt.anchorMax = Vector2.one;
            rootRt.offsetMin = Vector2.zero;
            rootRt.offsetMax = Vector2.zero;

            // Arka plan
            var bgImg = root.AddComponent<Image>();
            bgImg.sprite = RimaUITheme.MenuDungeonBackground;
            bgImg.color = new Color(0.70f, 0.78f, 0.82f, 0.95f);
            bgImg.preserveAspect = false;

            var shade = MakeChild(root.transform, "Shade");
            var shadeRt = shade.GetComponent<RectTransform>();
            shadeRt.anchorMin = Vector2.zero;
            shadeRt.anchorMax = Vector2.one;
            shadeRt.offsetMin = Vector2.zero;
            shadeRt.offsetMax = Vector2.zero;
            shade.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.58f);

            BuildPanelBackdrop(root.transform);

            // Başlık
            var titleGo = MakeChild(root.transform, "Title");
            var titleRt = titleGo.GetComponent<RectTransform>();
            titleRt.anchorMin = new Vector2(0.16f, 0.865f);
            titleRt.anchorMax = new Vector2(0.84f, 0.93f);
            titleRt.offsetMin = Vector2.zero; titleRt.offsetMax = Vector2.zero;
            var titleTmp = titleGo.AddComponent<TextMeshProUGUI>();
            titleTmp.text = "RIMA AYARLAR / TEST PANELI";
            titleTmp.fontSize = 30;
            titleTmp.fontStyle = FontStyles.Bold;
            titleTmp.color = headerColor;
            titleTmp.alignment = TextAlignmentOptions.Center;

            // Tuş atamaları bölümü
            var sectionGo = MakeChild(root.transform, "KeybindSection");
            var secRt = sectionGo.GetComponent<RectTransform>();
            secRt.anchorMin = new Vector2(0.25f, 0.72f);
            secRt.anchorMax = new Vector2(0.75f, 0.78f);
            secRt.offsetMin = Vector2.zero; secRt.offsetMax = Vector2.zero;
            var secTmp = sectionGo.AddComponent<TextMeshProUGUI>();
            secTmp.text = "TUŞ ATAMALARI";
            secTmp.fontSize = 14;
            secTmp.color = accentColor;
            secTmp.alignment = TextAlignmentOptions.Center;

            // 4 tuş satırı
            for (int i = 0; i < 4; i++)
            {
                float yTop = 0.67f - i * 0.09f;
                BuildKeybindRow(root.transform, i, yTop);
            }

            // Rebind prompt
            var promptGo = MakeChild(root.transform, "Prompt");
            var prt = promptGo.GetComponent<RectTransform>();
            prt.anchorMin = new Vector2(0.2f, 0.14f);
            prt.anchorMax = new Vector2(0.8f, 0.18f);
            prt.offsetMin = Vector2.zero; prt.offsetMax = Vector2.zero;
            rebindPrompt = promptGo.AddComponent<TextMeshProUGUI>();
            rebindPrompt.text = "";
            rebindPrompt.fontSize = 14;
            rebindPrompt.color = headerColor;
            rebindPrompt.alignment = TextAlignmentOptions.Center;

            // ── Kontrol Ayarlari bölümü ─────────────────────────────
            var controlSectionGo = MakeChild(root.transform, "ControlSection");
            var controlRt = controlSectionGo.GetComponent<RectTransform>();
            controlRt.anchorMin = new Vector2(0.25f, 0.32f);
            controlRt.anchorMax = new Vector2(0.75f, 0.36f);
            controlRt.offsetMin = Vector2.zero; controlRt.offsetMax = Vector2.zero;
            var controlTmp = controlSectionGo.AddComponent<TextMeshProUGUI>();
            controlTmp.text = "OYNANIS";
            controlTmp.fontSize = 14;
            controlTmp.color = accentColor;
            controlTmp.alignment = TextAlignmentOptions.Center;

            BuildAttackAimToggleRow(root.transform, 0.30f);
            BuildDashModeToggleRow(root.transform, 0.24f);

            // ── Görsel Ayarlar bölümü ─────────────────────────────
            var vizSectionGo = MakeChild(root.transform, "VizSection");
            var vizRt = vizSectionGo.GetComponent<RectTransform>();
            vizRt.anchorMin = new Vector2(0.25f, 0.18f);
            vizRt.anchorMax = new Vector2(0.75f, 0.22f);
            vizRt.offsetMin = Vector2.zero; vizRt.offsetMax = Vector2.zero;
            var vizTmp = vizSectionGo.AddComponent<TextMeshProUGUI>();
            vizTmp.text = "GÖRSEL AYARLAR";
            vizTmp.fontSize = 14;
            vizTmp.color = accentColor;
            vizTmp.alignment = TextAlignmentOptions.Center;

            // Karakter HP Bar toggle
            BuildToggleRow(root.transform, "Karakter HP Barı", CharacterHPBar.PrefKey, 0.17f);

            // ── Test class switch bölümü ─────────────────────────────
            var testSectionGo = MakeChild(root.transform, "TestClassSection");
            var testRt = testSectionGo.GetComponent<RectTransform>();
            testRt.anchorMin = new Vector2(0.25f, 0.105f);
            testRt.anchorMax = new Vector2(0.75f, 0.135f);
            testRt.offsetMin = Vector2.zero; testRt.offsetMax = Vector2.zero;
            var testTmp = testSectionGo.AddComponent<TextMeshProUGUI>();
            testTmp.text = "TEST SINIFI";
            testTmp.fontSize = 13;
            testTmp.color = accentColor;
            testTmp.alignment = TextAlignmentOptions.Center;

            BuildClassSwitchButtons(root.transform, 0.063f, 0.102f);

            BuildRoomPreviewSection(root.transform);

            // Varsayılana dön butonu
            BuildButton(root.transform, "Varsayılana Dön", 0.25f, 0.47f, 0.01f, 0.055f, () =>
            {
                KeyBindManager.ResetToDefaults();
                PlayerPrefs.SetInt(PlayerController.AttackAimModePrefKey, (int)CombatAimMode.TowardsMouse);
                PlayerPrefs.SetInt("DashMode", (int)DashMode.FacingDirection);
                PlayerPrefs.Save();
                RefreshKeyTexts();
                if (attackAimText != null) RefreshAttackAimText(attackAimText);
                if (dashModeText != null) RefreshDashModeText(dashModeText);
                RefreshPlayerSkillBindings();
                var bar = FindAnyObjectByType<SkillBarUI>();
                bar?.RefreshKeyLabels();
            });

            // Devam et butonu
            BuildButton(root.transform, "DEVAM ET", 0.53f, 0.75f, 0.01f, 0.055f, Close);

            root.SetActive(false);
        }

        private void BuildPanelBackdrop(Transform parent)
        {
            BuildDecorPanel(parent, "MainStonePanel", 0.17f, 0.83f, 0.045f, 0.94f, new Color(0.030f, 0.032f, 0.036f, 0.96f));
            BuildDecorPanel(parent, "LeftSettingsCard", 0.22f, 0.77f, 0.135f, 0.82f, cardColor);
            BuildDecorPanel(parent, "RoomPreviewCard", 0.785f, 0.965f, 0.145f, 0.82f, cardColor);
            BuildDecorPanel(parent, "BottomActionRail", 0.22f, 0.77f, 0.000f, 0.075f, new Color(0.035f, 0.038f, 0.044f, 0.88f));
        }

        private void BuildDecorPanel(Transform parent, string name, float xMin, float xMax, float yMin, float yMax, Color color)
        {
            var go = MakeChild(parent, name);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(xMin, yMin);
            rt.anchorMax = new Vector2(xMax, yMax);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            var img = go.AddComponent<Image>();
            img.sprite = name.Contains("Main") ? RimaUITheme.SmallPanelFrame : RimaUITheme.PromptFrame;
            img.color = new Color(1f, 1f, 1f, Mathf.Clamp(color.a, 0.72f, 0.96f));
            img.raycastTarget = false;
            var outline = go.AddComponent<Outline>();
            outline.effectColor = frameColor;
            outline.effectDistance = new Vector2(2f, -2f);
        }

        private void BuildKeybindRow(Transform parent, int slot, float yTop)
        {
            float yBot = yTop - 0.06f;

            // Label
            var labelGo = MakeChild(parent, $"Label_{slot}");
            var lrt = labelGo.GetComponent<RectTransform>();
            lrt.anchorMin = new Vector2(0.25f, yBot);
            lrt.anchorMax = new Vector2(0.48f, yTop);
            lrt.offsetMin = Vector2.zero; lrt.offsetMax = Vector2.zero;
            var ltmp = labelGo.AddComponent<TextMeshProUGUI>();
            ltmp.text = SlotLabels[slot];
            ltmp.fontSize = 16;
            ltmp.color = textColor;
            ltmp.alignment = TextAlignmentOptions.MidlineLeft;

            // Key button
            var btnGo = MakeChild(parent, $"KeyBtn_{slot}");
            var brt = btnGo.GetComponent<RectTransform>();
            brt.anchorMin = new Vector2(0.52f, yBot);
            brt.anchorMax = new Vector2(0.68f, yTop);
            brt.offsetMin = Vector2.zero; brt.offsetMax = Vector2.zero;

            var img = btnGo.AddComponent<Image>();
            img.sprite = RimaUITheme.PromptFrame;
            img.color = RimaUITheme.PanelTint;
            var btn = btnGo.AddComponent<Button>();
            var cols = btn.colors;
            cols.highlightedColor = btnHover;
            btn.colors = cols;

            var txtGo = MakeChild(btnGo.transform, "Txt");
            var trt = txtGo.GetComponent<RectTransform>();
            trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one;
            trt.offsetMin = Vector2.zero; trt.offsetMax = Vector2.zero;
            var tmp = txtGo.AddComponent<TextMeshProUGUI>();
            tmp.fontSize = 18;
            tmp.fontStyle = FontStyles.Bold;
            tmp.color = headerColor;
            tmp.alignment = TextAlignmentOptions.Center;

            keyTexts[slot] = tmp;

            int capturedSlot = slot;
            btn.onClick.AddListener(() =>
            {
                rebindSlot = capturedSlot;
                rebindPrompt.text = $"'{SlotLabels[capturedSlot]}' için yeni tuşa basın... (ESC = İptal)";
                tmp.text = "...";
            });
        }

        private void BuildButton(Transform parent, string label, float xMin, float xMax, float yMin, float yMax, System.Action onClick)
        {
            var go = MakeChild(parent, $"Btn_{label}");
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(xMin, yMin);
            rt.anchorMax = new Vector2(xMax, yMax);
            rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;

            var img = go.AddComponent<Image>();
            img.sprite = RimaUITheme.PromptFrame;
            img.color = RimaUITheme.PanelTint;
            var btn = go.AddComponent<Button>();
            var cols = btn.colors;
            cols.highlightedColor = btnHover;
            btn.colors = cols;
            btn.onClick.AddListener(() => onClick?.Invoke());

            var txtGo = MakeChild(go.transform, "Txt");
            var trt = txtGo.GetComponent<RectTransform>();
            trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one;
            trt.offsetMin = Vector2.zero; trt.offsetMax = Vector2.zero;
            var tmp = txtGo.AddComponent<TextMeshProUGUI>();
            tmp.text = label;
            tmp.fontSize = 14;
            tmp.fontStyle = FontStyles.Bold;
            tmp.color = textColor;
            tmp.alignment = TextAlignmentOptions.Center;
        }

        private void RefreshKeyTexts()
        {
            for (int i = 0; i < 4; i++)
                if (keyTexts[i] != null)
                    keyTexts[i].text = KeyBindManager.GetKeyName(i);
            if (rebindPrompt != null)
                rebindPrompt.text = "";
        }

        private void BuildToggleRow(Transform parent, string label, string prefKey, float yTop)
        {
            float yBot = yTop - 0.06f;

            // Label
            var labelGo = MakeChild(parent, $"ToggleLabel_{prefKey}");
            var lrt = labelGo.GetComponent<RectTransform>();
            lrt.anchorMin = new Vector2(0.25f, yBot);
            lrt.anchorMax = new Vector2(0.60f, yTop);
            lrt.offsetMin = Vector2.zero; lrt.offsetMax = Vector2.zero;
            var ltmp = labelGo.AddComponent<TextMeshProUGUI>();
            ltmp.text = label;
            ltmp.fontSize = 16;
            ltmp.color = textColor;
            ltmp.alignment = TextAlignmentOptions.MidlineLeft;

            // Toggle butonu — AÇIK / KAPALI
            var btnGo = MakeChild(parent, $"ToggleBtn_{prefKey}");
            var brt = btnGo.GetComponent<RectTransform>();
            brt.anchorMin = new Vector2(0.62f, yBot + 0.01f);
            brt.anchorMax = new Vector2(0.75f, yTop - 0.01f);
            brt.offsetMin = Vector2.zero; brt.offsetMax = Vector2.zero;

            var img = btnGo.AddComponent<Image>();
            img.color = btnNormal;
            var btn = btnGo.AddComponent<Button>();
            var cols = btn.colors;
            cols.highlightedColor = btnHover;
            btn.colors = cols;

            var txtGo = MakeChild(btnGo.transform, "Txt");
            var trt = txtGo.GetComponent<RectTransform>();
            trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one;
            trt.offsetMin = Vector2.zero; trt.offsetMax = Vector2.zero;
            var tmp = txtGo.AddComponent<TextMeshProUGUI>();
            tmp.fontSize = 13;
            tmp.fontStyle = FontStyles.Bold;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.raycastTarget = false;

            // Başlangıç durumu
            RefreshToggleText(tmp, prefKey);

            btn.onClick.AddListener(() =>
            {
                int current = PlayerPrefs.GetInt(prefKey, 0);
                int next = current == 0 ? 1 : 0;
                PlayerPrefs.SetInt(prefKey, next);
                PlayerPrefs.Save();
                RefreshToggleText(tmp, prefKey);

                // Sahnedeki CharacterHPBar varsa anında uygula
                var hpBar = FindAnyObjectByType<CharacterHPBar>();
                hpBar?.ApplyVisibility();
            });
        }

        private void BuildAttackAimToggleRow(Transform parent, float yTop)
        {
            float yBot = yTop - 0.06f;

            var labelGo = MakeChild(parent, "ToggleLabel_AttackAimMode");
            var lrt = labelGo.GetComponent<RectTransform>();
            lrt.anchorMin = new Vector2(0.25f, yBot);
            lrt.anchorMax = new Vector2(0.60f, yTop);
            lrt.offsetMin = Vector2.zero; lrt.offsetMax = Vector2.zero;
            var ltmp = labelGo.AddComponent<TextMeshProUGUI>();
            ltmp.text = "Saldırı hedefi";
            ltmp.fontSize = 16;
            ltmp.color = textColor;
            ltmp.alignment = TextAlignmentOptions.MidlineLeft;

            var btnGo = MakeChild(parent, "ToggleBtn_AttackAimMode");
            var brt = btnGo.GetComponent<RectTransform>();
            brt.anchorMin = new Vector2(0.62f, yBot + 0.01f);
            brt.anchorMax = new Vector2(0.75f, yTop - 0.01f);
            brt.offsetMin = Vector2.zero; brt.offsetMax = Vector2.zero;

            var img = btnGo.AddComponent<Image>();
            img.color = btnNormal;
            var btn = btnGo.AddComponent<Button>();
            var cols = btn.colors;
            cols.highlightedColor = btnHover;
            btn.colors = cols;

            var txtGo = MakeChild(btnGo.transform, "Txt");
            var trt = txtGo.GetComponent<RectTransform>();
            trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one;
            trt.offsetMin = Vector2.zero; trt.offsetMax = Vector2.zero;
            var tmp = txtGo.AddComponent<TextMeshProUGUI>();
            tmp.fontSize = 12;
            tmp.fontStyle = FontStyles.Bold;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.raycastTarget = false;
            attackAimText = tmp;

            RefreshAttackAimText(tmp);

            btn.onClick.AddListener(() =>
            {
                bool mouseAim = PlayerPrefs.GetInt(PlayerController.AttackAimModePrefKey, (int)CombatAimMode.TowardsMouse) == (int)CombatAimMode.TowardsMouse;
                PlayerPrefs.SetInt(PlayerController.AttackAimModePrefKey,
                    mouseAim ? (int)CombatAimMode.CharacterFacing : (int)CombatAimMode.TowardsMouse);
                PlayerPrefs.Save();
                RefreshAttackAimText(tmp);
            });
        }

        private void BuildDashModeToggleRow(Transform parent, float yTop)
        {
            float yBot = yTop - 0.055f;

            var labelGo = MakeChild(parent, "ToggleLabel_DashMode");
            var lrt = labelGo.GetComponent<RectTransform>();
            lrt.anchorMin = new Vector2(0.25f, yBot);
            lrt.anchorMax = new Vector2(0.60f, yTop);
            lrt.offsetMin = Vector2.zero; lrt.offsetMax = Vector2.zero;
            var ltmp = labelGo.AddComponent<TextMeshProUGUI>();
            ltmp.text = "Dash yönü";
            ltmp.fontSize = 16;
            ltmp.color = textColor;
            ltmp.alignment = TextAlignmentOptions.MidlineLeft;

            var btnGo = MakeChild(parent, "ToggleBtn_DashMode");
            var brt = btnGo.GetComponent<RectTransform>();
            brt.anchorMin = new Vector2(0.62f, yBot + 0.01f);
            brt.anchorMax = new Vector2(0.75f, yTop - 0.01f);
            brt.offsetMin = Vector2.zero; brt.offsetMax = Vector2.zero;

            var img = btnGo.AddComponent<Image>();
            img.color = btnNormal;
            var btn = btnGo.AddComponent<Button>();
            var cols = btn.colors;
            cols.highlightedColor = btnHover;
            btn.colors = cols;

            var txtGo = MakeChild(btnGo.transform, "Txt");
            var trt = txtGo.GetComponent<RectTransform>();
            trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one;
            trt.offsetMin = Vector2.zero; trt.offsetMax = Vector2.zero;
            var tmp = txtGo.AddComponent<TextMeshProUGUI>();
            tmp.fontSize = 12;
            tmp.fontStyle = FontStyles.Bold;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.raycastTarget = false;
            dashModeText = tmp;

            RefreshDashModeText(tmp);

            btn.onClick.AddListener(() =>
            {
                bool mouseDash = PlayerPrefs.GetInt("DashMode", (int)DashMode.FacingDirection) == (int)DashMode.TowardsMouse;
                PlayerPrefs.SetInt("DashMode", mouseDash ? (int)DashMode.FacingDirection : (int)DashMode.TowardsMouse);
                PlayerPrefs.Save();
                RefreshDashModeText(tmp);
            });
        }

        private void BuildClassSwitchButtons(Transform parent, float yMin, float yMax)
        {
            BuildClassButton(parent, "Warblade", ClassType.Warblade, 0.25f, 0.37f, yMin, yMax);
            BuildClassButton(parent, "Elementalist", ClassType.Elementalist, 0.38f, 0.51f, yMin, yMax);
            BuildClassButton(parent, "Ranger", ClassType.Ranger, 0.52f, 0.63f, yMin, yMax);
            BuildClassButton(parent, "Shadowblade", ClassType.Shadowblade, 0.64f, 0.77f, yMin, yMax);
        }

        private void BuildClassButton(Transform parent, string label, ClassType type, float xMin, float xMax, float yMin, float yMax)
        {
            BuildButton(parent, label, xMin, xMax, yMin, yMax, () =>
            {
                var manager = PlayerClassManager.Instance ?? FindAnyObjectByType<PlayerClassManager>();
                manager?.SetPrimaryClass(type);
                var bar = FindAnyObjectByType<SkillBarUI>();
                bar?.RefreshKeyLabels();
            });
        }

        private void BuildRoomPreviewSection(Transform parent)
        {
            var titleGo = MakeChild(parent, "RoomPreviewTitle");
            var titleRt = titleGo.GetComponent<RectTransform>();
            titleRt.anchorMin = new Vector2(0.800f, 0.765f);
            titleRt.anchorMax = new Vector2(0.950f, 0.805f);
            titleRt.offsetMin = Vector2.zero;
            titleRt.offsetMax = Vector2.zero;
            var title = titleGo.AddComponent<TextMeshProUGUI>();
            title.text = "ODA TESTI";
            title.fontSize = 14;
            title.fontStyle = FontStyles.Bold;
            title.color = accentColor;
            title.alignment = TextAlignmentOptions.Center;

            var hintGo = MakeChild(parent, "RoomPreviewHint");
            var hintRt = hintGo.GetComponent<RectTransform>();
            hintRt.anchorMin = new Vector2(0.800f, 0.712f);
            hintRt.anchorMax = new Vector2(0.950f, 0.755f);
            hintRt.offsetMin = Vector2.zero;
            hintRt.offsetMax = Vector2.zero;
            var hint = hintGo.AddComponent<TextMeshProUGUI>();
            hint.text = "Buton odayi yukler ve menuyu kapatir.";
            hint.fontSize = 10;
            hint.color = new Color(0.62f, 0.72f, 0.76f);
            hint.alignment = TextAlignmentOptions.Center;

            int count = LargeDungeonMapPainterBase.DefaultPreviewLayoutCount;
            for (int i = 0; i < count; i++)
            {
                float row = i;
                float yMax = 0.688f - row * 0.052f;
                float yMin = yMax - 0.040f;
                string label = LargeDungeonMapPainterBase.GetDefaultPreviewLayoutName(i);
                BuildRoomPreviewButton(parent, i, label, 0.800f, 0.950f, yMin, yMax);
            }
        }

        private void BuildRoomPreviewButton(Transform parent, int index, string label, float xMin, float xMax, float yMin, float yMax)
        {
            string shortLabel = label;
            int firstSpace = label.IndexOf(' ');
            if (firstSpace >= 0 && firstSpace + 1 < label.Length)
                shortLabel = label.Substring(0, Mathf.Min(label.Length, firstSpace + 18));

            BuildButton(parent, shortLabel, xMin, xMax, yMin, yMax, () =>
            {
                var manager = RuntimeRoomManager.Instance ?? FindAnyObjectByType<RuntimeRoomManager>();
                if (manager == null)
                {
                    Debug.LogWarning("[SettingsMenuUI] RuntimeRoomManager missing; cannot preview room.");
                    return;
                }

                manager.PreviewRoomByIndex(index);
                Close();
            });
        }

        private void RefreshAttackAimText(TextMeshProUGUI tmp)
        {
            bool mouseAim = PlayerPrefs.GetInt(PlayerController.AttackAimModePrefKey, (int)CombatAimMode.TowardsMouse) == (int)CombatAimMode.TowardsMouse;
            tmp.text = mouseAim ? "İMLEÇ" : "YÜRÜYÜŞ";
            tmp.color = mouseAim ? new Color(0.3f, 0.9f, 0.4f) : headerColor;
        }

        private void RefreshDashModeText(TextMeshProUGUI tmp)
        {
            bool mouseDash = PlayerPrefs.GetInt("DashMode", (int)DashMode.FacingDirection) == (int)DashMode.TowardsMouse;
            tmp.text = mouseDash ? "İMLEÇ" : "YÜRÜYÜŞ";
            tmp.color = mouseDash ? new Color(0.3f, 0.9f, 0.4f) : headerColor;
        }

        private void RefreshToggleText(TextMeshProUGUI tmp, string prefKey)
        {
            bool on = PlayerPrefs.GetInt(prefKey, 0) == 1;
            tmp.text  = on ? "AÇIK"  : "KAPALI";
            tmp.color = on ? new Color(0.3f, 0.9f, 0.4f) : new Color(0.6f, 0.6f, 0.6f);
        }

        private static GameObject MakeChild(Transform parent, string name)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return go;
        }

        private static void RefreshPlayerSkillBindings()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            player.GetComponent<Warblade_SkillController>()?.RebuildBindings();
            player.GetComponent<Elementalist_SkillController>()?.RebuildBindings();
            player.GetComponent<Ranger_SkillController>()?.RebuildBindings();
            player.GetComponent<Shadowblade_SkillController>()?.RebuildBindings();
        }
    }
}
