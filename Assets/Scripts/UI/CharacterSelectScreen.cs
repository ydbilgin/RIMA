using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// RIMA Character Select Screen — fully procedural, no prefab required.
    /// Layout: left card grid (10 classes) | center portrait + stats | right skill preview
    /// Anchor images loaded from Resources/Characters/Anchors/*.
    /// </summary>
    public class CharacterSelectScreen : MonoBehaviour
    {
        [Header("Scene")]
        [SerializeField] private string gameSceneName = "GameScene";

        [Header("Optional Overrides")]
        [SerializeField] private Canvas targetCanvas;

        // ── Internal layout state ─────────────────────────────────────────
        private ClassType selectedClass = ClassType.Warblade;
        private readonly Dictionary<ClassType, CardEntry> cards = new();

        private Image     portraitImage;
        private TMP_Text  classNameLabel;
        private TMP_Text  tagline1Label;
        private TMP_Text  tagline2Label;
        private Image     accentBar;
        private Button    startButton;

        private static readonly ClassType[] AllClasses =
        {
            ClassType.Warblade, ClassType.Elementalist, ClassType.Shadowblade,
            ClassType.Ranger, ClassType.Ravager, ClassType.Ronin,
            ClassType.Gunslinger, ClassType.Brawler, ClassType.Summoner, ClassType.Hexer
        };

        private struct CardEntry
        {
            public RectTransform root;
            public Image         bg;
            public Image         accentLine;
            public TMP_Text      nameLabel;
            public ClassType     classType;
        }

        // ─── Lifecycle ────────────────────────────────────────────────────

        private void Awake()
        {
            if (targetCanvas == null) targetCanvas = GetComponentInParent<Canvas>();
            BuildScreen();
            SelectClass(ClassType.Warblade);
        }

        // ─── Build ────────────────────────────────────────────────────────

        private void BuildScreen()
        {
            var root = (targetCanvas != null ? targetCanvas.transform : transform) as RectTransform;

            if (targetCanvas == null)
            {
                var canvasGO = new GameObject("CharacterSelectCanvas");
                canvasGO.transform.SetParent(transform);
                var canvas = canvasGO.AddComponent<Canvas>();
                canvas.renderMode   = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 100;
                var scaler = canvasGO.AddComponent<CanvasScaler>();
                scaler.uiScaleMode         = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);
                scaler.matchWidthOrHeight  = 0.5f;
                canvasGO.AddComponent<GraphicRaycaster>();
                root = canvasGO.transform as RectTransform;
            }

            // Full-screen dark overlay
            var bg = MakePanel("CSS_Background", root);
            bg.anchorMin = Vector2.zero; bg.anchorMax = Vector2.one;
            bg.offsetMin = bg.offsetMax = Vector2.zero;
            SetImg(bg, RimaUITheme.BackgroundDark);

            // Title
            var title = MakeText("RIMA  —  SELECT YOUR CLASS", bg, 30, FontStyles.Bold, RimaUITheme.Cyan);
            title.alignment = TextAlignmentOptions.Center;
            var titleRt = title.transform as RectTransform;
            titleRt.anchorMin = new Vector2(0f, 1f); titleRt.anchorMax = new Vector2(1f, 1f);
            titleRt.pivot = new Vector2(0.5f, 1f);
            titleRt.anchoredPosition = new Vector2(0f, -40f);
            titleRt.sizeDelta = new Vector2(0f, 48f);

            // Three-column layout
            float lw = 0.26f, cw = 0.44f;

            var leftPanel   = MakePanel("CSP_Left",   bg);
            var centerPanel = MakePanel("CSP_Center", bg);
            var rightPanel  = MakePanel("CSP_Right",  bg);

            SetStretch(leftPanel,   new Vector2(0f,       0.06f), new Vector2(lw,        0.91f), new Vector2(16f, 8f),  new Vector2(-8f,  -8f));
            SetStretch(centerPanel, new Vector2(lw,       0.06f), new Vector2(lw + cw,   0.91f), new Vector2(8f,  8f),  new Vector2(-8f,  -8f));
            SetStretch(rightPanel,  new Vector2(lw + cw,  0.06f), new Vector2(1f,        0.91f), new Vector2(8f,  8f),  new Vector2(-16f, -8f));

            SetImg(leftPanel,   RimaUITheme.PanelTint);
            SetImg(centerPanel, Color.clear);
            SetImg(rightPanel,  RimaUITheme.PanelTint);

            BuildCardGrid(leftPanel);
            BuildCenterPanel(centerPanel);
            BuildRightPanel(rightPanel);
            BuildStartButton(bg);
        }

        private void BuildCardGrid(RectTransform parent)
        {
            var header = MakeText("CLASSES", parent, 10, FontStyles.Bold, RimaUITheme.TextMuted);
            header.alignment = TextAlignmentOptions.Center;
            var hRt = header.transform as RectTransform;
            hRt.anchorMin = new Vector2(0f, 1f); hRt.anchorMax = new Vector2(1f, 1f);
            hRt.pivot = new Vector2(0.5f, 1f);
            hRt.anchoredPosition = new Vector2(0f, -16f);
            hRt.sizeDelta = new Vector2(0f, 20f);

            // 5 rows x 2 cols
            float cardH    = 0.155f;
            float cardPad  = 0.010f;
            float gridTop  = 0.90f;

            for (int i = 0; i < AllClasses.Length; i++)
            {
                int row = i / 2;
                int col = i % 2;
                float y2 = gridTop - row * (cardH + cardPad);
                float y1 = y2 - cardH;
                float x1 = col == 0 ? 0.04f : 0.52f;
                float x2 = col == 0 ? 0.48f : 0.96f;
                BuildClassCard(parent, AllClasses[i], x1, y1, x2, y2);
            }
        }

        private void BuildClassCard(RectTransform parent, ClassType cls, float x1, float y1, float x2, float y2)
        {
            var cardRoot = MakePanel($"Card_{cls}", parent);
            SetStretch(cardRoot, new Vector2(x1, y1), new Vector2(x2, y2), Vector2.zero, Vector2.zero);

            var bg = cardRoot.GetComponent<Image>();
            bg.sprite = RimaUITheme.SmallPanelFrame;
            bg.color = RimaUITheme.SlotLocked;
            bg.type = Image.Type.Sliced;
            bg.raycastTarget = true;

            // Left accent bar (3px wide)
            var accentLineGo = MakePanel("AccentLine", cardRoot);
            SetStretch(accentLineGo, new Vector2(0f, 0.12f), new Vector2(0f, 0.88f),
                new Vector2(6f, 0f), new Vector2(10f, 0f));
            var accentImg = accentLineGo.GetComponent<Image>();
            accentImg.color = RimaUITheme.ClassAccent(cls);
            accentImg.raycastTarget = false;

            // Class name
            var nameLabel = MakeText(cls.ToString().ToUpperInvariant(), cardRoot, 12, FontStyles.Bold, RimaUITheme.TextPrimary);
            nameLabel.alignment = TextAlignmentOptions.Left;
            var nRt = nameLabel.transform as RectTransform;
            nRt.anchorMin = new Vector2(0f, 0.45f); nRt.anchorMax = new Vector2(1f, 1f);
            nRt.offsetMin = new Vector2(16f, 0f); nRt.offsetMax = new Vector2(-4f, 0f);

            // Tagline
            var (tl1, _) = RimaUITheme.ClassTagline(cls);
            var tagLabel = MakeText(tl1, cardRoot, 8, FontStyles.Normal, RimaUITheme.TextMuted);
            tagLabel.alignment = TextAlignmentOptions.Left;
            var tRt = tagLabel.transform as RectTransform;
            tRt.anchorMin = new Vector2(0f, 0f); tRt.anchorMax = new Vector2(1f, 0.48f);
            tRt.offsetMin = new Vector2(16f, 0f); tRt.offsetMax = new Vector2(-4f, 0f);

            // Button
            var btn = cardRoot.gameObject.AddComponent<Button>();
            var colors = btn.colors;
            colors.normalColor      = new Color(1f, 1f, 1f, 0f);
            colors.highlightedColor = new Color(1f, 1f, 1f, 0.08f);
            colors.pressedColor     = new Color(1f, 1f, 1f, 0.15f);
            btn.colors = colors;
            var captured = cls;
            btn.onClick.AddListener(() => SelectClass(captured));

            cards[cls] = new CardEntry
            {
                root = cardRoot, bg = bg, accentLine = accentImg,
                nameLabel = nameLabel, classType = cls
            };
        }

        private void BuildCenterPanel(RectTransform parent)
        {
            // Portrait fills 80% of center height
            var portraitRoot = MakePanel("PortraitRoot", parent);
            SetStretch(portraitRoot, new Vector2(0.05f, 0.28f), new Vector2(0.95f, 0.98f), Vector2.zero, Vector2.zero);

            portraitImage = portraitRoot.GetComponent<Image>();
            portraitImage.preserveAspect = true;
            portraitImage.color = Color.white;
            portraitImage.raycastTarget = false;

            // Accent bar
            var accentBarGo = MakePanel("AccentBar", parent);
            SetStretch(accentBarGo, new Vector2(0.05f, 0.245f), new Vector2(0.95f, 0.268f), Vector2.zero, Vector2.zero);
            accentBar = accentBarGo.GetComponent<Image>();
            accentBar.raycastTarget = false;

            // Class name label
            classNameLabel = MakeText("WARBLADE", parent, 28, FontStyles.Bold, Color.white);
            classNameLabel.alignment = TextAlignmentOptions.Center;
            var cnRt = classNameLabel.transform as RectTransform;
            cnRt.anchorMin = new Vector2(0f, 0f); cnRt.anchorMax = new Vector2(1f, 0f);
            cnRt.pivot = new Vector2(0.5f, 0f);
            cnRt.anchoredPosition = new Vector2(0f, 105f);
            cnRt.sizeDelta = new Vector2(0f, 36f);

            // Tagline 1
            tagline1Label = MakeText("HEAVY · MELEE · RAGE", parent, 11, FontStyles.Bold, RimaUITheme.TextMuted);
            tagline1Label.alignment = TextAlignmentOptions.Center;
            var t1Rt = tagline1Label.transform as RectTransform;
            t1Rt.anchorMin = new Vector2(0f, 0f); t1Rt.anchorMax = new Vector2(1f, 0f);
            t1Rt.pivot = new Vector2(0.5f, 0f);
            t1Rt.anchoredPosition = new Vector2(0f, 76f);
            t1Rt.sizeDelta = new Vector2(0f, 24f);

            // Tagline 2
            tagline2Label = MakeText("", parent, 10, FontStyles.Normal, RimaUITheme.TextMuted);
            tagline2Label.alignment = TextAlignmentOptions.Center;
            tagline2Label.enableWordWrapping = true;
            var t2Rt = tagline2Label.transform as RectTransform;
            t2Rt.anchorMin = new Vector2(0.05f, 0f); t2Rt.anchorMax = new Vector2(0.95f, 0f);
            t2Rt.pivot = new Vector2(0.5f, 0f);
            t2Rt.anchoredPosition = new Vector2(0f, 46f);
            t2Rt.sizeDelta = new Vector2(0f, 30f);
        }

        private void BuildRightPanel(RectTransform parent)
        {
            var header = MakeText("IDENTITY", parent, 10, FontStyles.Bold, RimaUITheme.TextMuted);
            header.alignment = TextAlignmentOptions.Center;
            var hRt = header.transform as RectTransform;
            hRt.anchorMin = new Vector2(0f, 1f); hRt.anchorMax = new Vector2(1f, 1f);
            hRt.pivot = new Vector2(0.5f, 1f);
            hRt.anchoredPosition = new Vector2(0f, -16f);
            hRt.sizeDelta = new Vector2(0f, 20f);

            string[] slotNames =
            {
                "LMB  BASIC ATTACK", "SKILL  1",
                "SKILL  2",          "SKILL  3",
                "SKILL  4",          "IDENTITY PASSIVE"
            };

            float rowH   = 0.120f;
            float rowPad = 0.012f;
            float top    = 0.88f;

            for (int i = 0; i < slotNames.Length; i++)
            {
                float y2 = top - i * (rowH + rowPad);
                float y1 = y2 - rowH;
                var row = MakePanel($"SkillRow_{i}", parent);
                SetStretch(row, new Vector2(0.04f, y1), new Vector2(0.96f, y2), Vector2.zero, Vector2.zero);
                
                var img = row.GetComponent<Image>();
                img.sprite = RimaUITheme.SmallPanelFrame;
                img.type = Image.Type.Sliced;
                img.color = RimaUITheme.SlotLocked;

                bool isPrimary = (i == 0 || i == 5);
                var lbl = MakeText(slotNames[i], row, 10,
                    isPrimary ? FontStyles.Bold : FontStyles.Normal,
                    isPrimary ? RimaUITheme.Cyan : RimaUITheme.TextMuted);
                lbl.alignment = TextAlignmentOptions.Left;
                var lRt = lbl.transform as RectTransform;
                lRt.anchorMin = Vector2.zero; lRt.anchorMax = Vector2.one;
                lRt.offsetMin = new Vector2(10f, 0f); lRt.offsetMax = new Vector2(-6f, 0f);
            }
        }

        private void BuildStartButton(RectTransform parent)
        {
            var btnRoot = MakePanel("StartButton", parent);
            btnRoot.anchorMin = new Vector2(0.5f, 0f);
            btnRoot.anchorMax = new Vector2(0.5f, 0f);
            btnRoot.pivot = new Vector2(0.5f, 0f);
            btnRoot.anchoredPosition = new Vector2(0f, 24f);
            btnRoot.sizeDelta = new Vector2(340f, 54f);

            var bgImg = btnRoot.GetComponent<Image>();
            bgImg.sprite = RimaUITheme.ResourceFrame;
            bgImg.type = Image.Type.Sliced;
            bgImg.color = RimaUITheme.ClassAccent(ClassType.Warblade);
            bgImg.raycastTarget = true;

            var lbl = MakeText("START RUN", btnRoot, 20, FontStyles.Bold, Color.white);
            lbl.alignment = TextAlignmentOptions.Center;
            var lRt = lbl.transform as RectTransform;
            lRt.anchorMin = Vector2.zero; lRt.anchorMax = Vector2.one;
            lRt.offsetMin = lRt.offsetMax = Vector2.zero;

            startButton = btnRoot.gameObject.AddComponent<Button>();
            startButton.targetGraphic = bgImg;
            startButton.onClick.AddListener(OnStartRun);

            var colors = startButton.colors;
            colors.normalColor      = Color.white;
            colors.highlightedColor = new Color(1.15f, 1.15f, 1.15f, 1f);
            colors.pressedColor     = new Color(0.80f, 0.80f, 0.80f, 1f);
            startButton.colors = colors;
        }

        // ─── Selection Logic ──────────────────────────────────────────────

        private void SelectClass(ClassType cls)
        {
            selectedClass = cls;
            var accent = RimaUITheme.ClassAccent(cls);

            foreach (var kv in cards)
            {
                bool sel = kv.Key == cls;
                kv.Value.bg.color = sel
                    ? RimaUITheme.SlotBg
                    : RimaUITheme.SlotLocked;
                kv.Value.nameLabel.color = sel ? accent : RimaUITheme.TextPrimary;
            }

            if (accentBar != null) accentBar.color = accent;

            classNameLabel.text  = cls.ToString().ToUpperInvariant();
            classNameLabel.color = accent;

            var (tl1, tl2) = RimaUITheme.ClassTagline(cls);
            tagline1Label.text = tl1;
            tagline2Label.text = tl2;

            // Portrait
            if (portraitImage != null)
            {
                var tex = Resources.Load<Texture2D>(RimaUITheme.AnchorPath(cls));
                portraitImage.sprite = tex != null
                    ? Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0f))
                    : null;
                portraitImage.color = tex != null ? Color.white : new Color(0.12f, 0.14f, 0.20f, 1f);
            }

            // Start button color
            if (startButton != null)
            {
                var img = startButton.GetComponent<Image>();
                if (img != null) img.color = accent;
            }
        }

        private void OnStartRun()
        {
            UIManager.Instance?.ResumeGame();
            if (PlayerClassManager.Instance != null)
                PlayerClassManager.Instance.SetPrimaryClass(selectedClass);
            SceneManager.LoadScene(gameSceneName);
            Destroy(gameObject); // Cleanup select screen when moving to game
        }

        // ─── Helpers ──────────────────────────────────────────────────────

        private static RectTransform MakePanel(string name, RectTransform parent)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;
            return rt;
        }

        private static TMP_Text MakeText(string text, RectTransform parent, float size,
            FontStyles style, Color color)
        {
            int maxLen = Mathf.Min(text.Length, 14);
            var go = new GameObject("Lbl_" + text.Substring(0, maxLen), typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = size;
            tmp.fontStyle = style;
            tmp.color = color;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.raycastTarget = false;
            tmp.enableWordWrapping = false;
            return tmp;
        }

        private static void SetImg(RectTransform rt, Color color)
        {
            var img = rt.GetComponent<Image>() ?? rt.gameObject.AddComponent<Image>();
            img.color = color;
            img.raycastTarget = false;
        }

        private static void SetStretch(RectTransform rt,
            Vector2 anchorMin, Vector2 anchorMax,
            Vector2 offsetMin, Vector2 offsetMax)
        {
            rt.anchorMin = anchorMin; rt.anchorMax = anchorMax;
            rt.offsetMin = offsetMin; rt.offsetMax = offsetMax;
        }
    }
}
