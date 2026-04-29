using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
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
        [SerializeField] private Color panelBg     = new Color(0.06f, 0.06f, 0.12f, 0.96f);
        [SerializeField] private Color headerColor  = new Color(0.28f, 0.72f, 0.95f);
        [SerializeField] private Color btnNormal    = new Color(0.12f, 0.14f, 0.22f, 0.95f);
        [SerializeField] private Color btnHover     = new Color(0.20f, 0.25f, 0.40f, 1f);
        [SerializeField] private Color textColor    = new Color(0.88f, 0.90f, 0.95f);
        [SerializeField] private Color accentColor  = new Color(0.55f, 0.28f, 0.90f);

        private GameObject root;
        private bool isOpen;
        private InputAction escAction;

        // Rebind state
        private int rebindSlot = -1;
        private TextMeshProUGUI[] keyTexts = new TextMeshProUGUI[4];
        private TextMeshProUGUI rebindPrompt;

        private static readonly string[] SlotLabels = { "Skill 1", "Skill 2", "Skill 3", "Skill 4" };

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
            Time.timeScale = 0f;
            RefreshKeyTexts();
        }

        public void Close()
        {
            if (root != null) root.SetActive(false);
            isOpen = false;
            rebindSlot = -1;
            Time.timeScale = 1f;
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

                        // Controller'ı güncelle
                        var player = GameObject.FindGameObjectWithTag("Player");
                        if (player != null)
                        {
                            var sc = player.GetComponent<Warblade_SkillController>();
                            sc?.RebuildBindings();
                        }

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
            bgImg.color = panelBg;

            // Başlık
            var titleGo = MakeChild(root.transform, "Title");
            var titleRt = titleGo.GetComponent<RectTransform>();
            titleRt.anchorMin = new Vector2(0.2f, 0.82f);
            titleRt.anchorMax = new Vector2(0.8f, 0.92f);
            titleRt.offsetMin = Vector2.zero; titleRt.offsetMax = Vector2.zero;
            var titleTmp = titleGo.AddComponent<TextMeshProUGUI>();
            titleTmp.text = "AYARLAR";
            titleTmp.fontSize = 28;
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
            prt.anchorMin = new Vector2(0.2f, 0.25f);
            prt.anchorMax = new Vector2(0.8f, 0.32f);
            prt.offsetMin = Vector2.zero; prt.offsetMax = Vector2.zero;
            rebindPrompt = promptGo.AddComponent<TextMeshProUGUI>();
            rebindPrompt.text = "";
            rebindPrompt.fontSize = 14;
            rebindPrompt.color = headerColor;
            rebindPrompt.alignment = TextAlignmentOptions.Center;

            // ── Görsel Ayarlar bölümü ─────────────────────────────
            var vizSectionGo = MakeChild(root.transform, "VizSection");
            var vizRt = vizSectionGo.GetComponent<RectTransform>();
            vizRt.anchorMin = new Vector2(0.25f, 0.26f);
            vizRt.anchorMax = new Vector2(0.75f, 0.32f);
            vizRt.offsetMin = Vector2.zero; vizRt.offsetMax = Vector2.zero;
            var vizTmp = vizSectionGo.AddComponent<TextMeshProUGUI>();
            vizTmp.text = "GÖRSEL AYARLAR";
            vizTmp.fontSize = 14;
            vizTmp.color = accentColor;
            vizTmp.alignment = TextAlignmentOptions.Center;

            // Karakter HP Bar toggle
            BuildToggleRow(root.transform, "Karakter HP Barı", CharacterHPBar.PrefKey, 0.30f);

            // Varsayılana dön butonu
            BuildButton(root.transform, "Varsayılana Dön", 0.35f, 0.65f, 0.15f, 0.22f, () =>
            {
                KeyBindManager.ResetToDefaults();
                RefreshKeyTexts();
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) player.GetComponent<Warblade_SkillController>()?.RebuildBindings();
                var bar = FindAnyObjectByType<SkillBarUI>();
                bar?.RefreshKeyLabels();
            });

            // Devam et butonu
            BuildButton(root.transform, "DEVAM ET", 0.38f, 0.62f, 0.06f, 0.13f, Close);

            root.SetActive(false);
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
            img.color = btnNormal;
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
    }
}
