using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using RIMA.Systems.Map;

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
        [SerializeField] private string gameSceneName = "_IsoGame";

        [Header("Optional Overrides")]
        [SerializeField] private Canvas targetCanvas;

        // ── On-brand UI Pack (Resources/UI/RIMA/Pack/*) ──────────────────
        // Loaded once in BuildScreen; any may be null (sprite missing) -> wiring
        // silently skips that graphic and the prior flat-color visual remains.
        private const string PackBackdrop   = "UI/RIMA/Pack/bg_seal_keep";       // 1920x1080 full-screen backdrop
        private const string PackPedestal   = "UI/RIMA/Pack/pedestal_seal";      // 512 circular seal platform
        private const string PackCardFrame  = "UI/RIMA/Pack/card_frame_9slice";  // 256x384 portrait card frame (border 28)
        private const string PackButton     = "UI/RIMA/Pack/button_9slice";      // 192x64 filled button bg (border 16)

        // ── Internal layout state ─────────────────────────────────────────
        private ClassType selectedClass = ClassType.Warblade;
        private readonly Dictionary<ClassType, CardEntry> cards = new();

        private Image     portraitImage;
        private TMP_Text  classNameLabel;
        private TMP_Text  tagline1Label;
        private TMP_Text  tagline2Label;
        private Image     accentBar;
        private Button    startButton;
        private TMP_Text  startButtonLabel;

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
            public Image         portrait;
            public TMP_Text      nameLabel;
            public TMP_Text      actionLabel;
            public ClassType     classType;
        }

        // ─── Lifecycle ────────────────────────────────────────────────────

        private void Awake()
        {
            if (targetCanvas == null) targetCanvas = GetComponentInParent<Canvas>();
        }

        private void Start()
        {
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

            if (root == null)
            {
                Debug.LogWarning("[CharacterSelectScreen] BuildScreen: root RectTransform null, aborting build.");
                return;
            }

            // Full-screen dark overlay (also the parent container for all content).
            var bg = MakePanel("CSS_Background", root);
            bg.SetAsFirstSibling(); // keep backdrop behind any pre-existing canvas content
            bg.anchorMin = Vector2.zero; bg.anchorMax = Vector2.one;
            bg.offsetMin = bg.offsetMax = Vector2.zero;
            SetImg(bg, RimaUITheme.BackgroundDark);

            // On-brand seal-keep backdrop (Simple, white). Falls back to the flat
            // dark tint above if the sprite is missing.
            var backdropSprite = Resources.Load<Sprite>(PackBackdrop);
            if (backdropSprite != null)
            {
                var bgImg = bg.GetComponent<Image>();
                bgImg.sprite          = backdropSprite;
                bgImg.type            = Image.Type.Simple;
                bgImg.preserveAspect  = false; // stretch to fill the whole screen
                bgImg.color           = Color.white;
            }

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
            nRt.offsetMin = new Vector2(58f, 0f); nRt.offsetMax = new Vector2(-4f, 0f);

            var portraitRoot = MakePanel("Portrait", cardRoot);
            SetStretch(portraitRoot, new Vector2(0.12f, 0.14f), new Vector2(0.38f, 0.86f), Vector2.zero, Vector2.zero);
            var portrait = portraitRoot.GetComponent<Image>();
            portrait.sprite = LoadCanonicalSprite(cls);
            portrait.preserveAspect = true;
            portrait.raycastTarget = false;

            var actionLabel = MakeText(CardActionText(cls), cardRoot, 8, FontStyles.Normal, RimaUITheme.TextMuted);
            actionLabel.alignment = TextAlignmentOptions.Left;
            actionLabel.enableWordWrapping = true;
            var aRt = actionLabel.transform as RectTransform;
            aRt.anchorMin = new Vector2(0f, 0f); aRt.anchorMax = new Vector2(1f, 0.48f);
            aRt.offsetMin = new Vector2(58f, 0f); aRt.offsetMax = new Vector2(-4f, 0f);

            // Button
            var btn = cardRoot.gameObject.AddComponent<Button>();
            var colors = btn.colors;
            colors.normalColor      = new Color(1f, 1f, 1f, 0f);
            colors.highlightedColor = new Color(1f, 1f, 1f, 0.08f);
            colors.pressedColor     = new Color(1f, 1f, 1f, 0.15f);
            btn.colors = colors;
            var captured = cls;
            btn.onClick.AddListener(() => SelectClass(captured));

            // On-brand 9-slice card frame overlaid on the edges (transparent center
            // lets the portrait/text show through). Non-raycast so the card Button
            // still works. Added last -> drawn on top. Skipped if sprite missing.
            var cardFrameSprite = Resources.Load<Sprite>(PackCardFrame);
            if (cardFrameSprite != null)
            {
                var frame = MakePanel("CardFrame", cardRoot);
                frame.SetAsLastSibling();
                frame.anchorMin = Vector2.zero; frame.anchorMax = Vector2.one;
                frame.offsetMin = frame.offsetMax = Vector2.zero;
                var frameImg = frame.GetComponent<Image>();
                frameImg.sprite        = cardFrameSprite;
                frameImg.type          = Image.Type.Sliced;
                frameImg.color         = Color.white;
                frameImg.raycastTarget = false;
            }

            cards[cls] = new CardEntry
            {
                root = cardRoot, bg = bg, accentLine = accentImg,
                portrait = portrait, nameLabel = nameLabel,
                actionLabel = actionLabel, classType = cls
            };

            ApplyCardLockVisual(cards[cls], false);
        }

        private void BuildCenterPanel(RectTransform parent)
        {
            // Seal-stone pedestal UNDER the character's feet. Added first so it
            // renders behind the portrait; skipped entirely if the sprite is missing.
            var pedestalSprite = Resources.Load<Sprite>(PackPedestal);
            if (pedestalSprite != null)
            {
                var pedestal = MakePanel("Pedestal", parent);
                pedestal.SetAsFirstSibling(); // behind the portrait/labels
                pedestal.anchorMin = new Vector2(0.5f, 0.28f); // ~feet level of the portrait
                pedestal.anchorMax = new Vector2(0.5f, 0.28f);
                pedestal.pivot     = new Vector2(0.5f, 0.5f);
                pedestal.anchoredPosition = Vector2.zero;
                pedestal.sizeDelta = new Vector2(360f, 360f); // ~360px wide circular platform
                var pedImg = pedestal.GetComponent<Image>();
                pedImg.sprite         = pedestalSprite;
                pedImg.type           = Image.Type.Simple;
                pedImg.preserveAspect = true;
                pedImg.color          = Color.white;
                pedImg.raycastTarget  = false;
            }

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
            // On-brand 9-slice button background (filled center). Falls back to the
            // procedural ResourceFrame if the Pack sprite is missing.
            var buttonSprite = Resources.Load<Sprite>(PackButton);
            bgImg.sprite = buttonSprite != null ? buttonSprite : RimaUITheme.ResourceFrame;
            bgImg.type = Image.Type.Sliced;
            bgImg.color = RimaUITheme.ClassAccent(ClassType.Warblade);
            bgImg.raycastTarget = true;

            startButtonLabel = MakeText("START RUN", btnRoot, 20, FontStyles.Bold, Color.white);
            startButtonLabel.alignment = TextAlignmentOptions.Center;
            var lRt = startButtonLabel.transform as RectTransform;
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
                ApplyCardLockVisual(kv.Value, sel);
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
                portraitImage.sprite = LoadCanonicalSprite(cls);
                portraitImage.color = IsUnlocked(cls) ? Color.white : new Color(0.02f, 0.02f, 0.025f, 0.90f);
            }

            // Start button color
            if (startButton != null)
            {
                var img = startButton.GetComponent<Image>();
                if (img != null) img.color = accent;
                startButton.interactable = IsUnlocked(cls);
                if (startButtonLabel != null)
                {
                    startButtonLabel.text = IsUnlocked(cls) ? "START RUN" : LockedButtonText(cls).ToUpperInvariant();
                    startButtonLabel.fontSize = IsUnlocked(cls) ? 20f : 13f;
                }
            }
        }

        private void OnStartRun()
        {
            if (!IsUnlocked(selectedClass)) return;

            UIManager.Instance?.ResumeGame();
            PlayerClassManager.SelectedClass = selectedClass;
            if (PlayerClassManager.Instance != null)
                PlayerClassManager.Instance.SetPrimaryClass(selectedClass);
            RunStats.Instance?.StartNewRun();
            MapFlowManager.Instance?.ResetRun();
            SceneManager.LoadScene(gameSceneName);
            Destroy(gameObject); // Cleanup select screen when moving to game
        }

        private static bool IsUnlocked(ClassType cls) =>
            cls == ClassType.Warblade ||
            cls == ClassType.Elementalist ||
            cls == ClassType.Ranger ||
            cls == ClassType.Shadowblade;

        private static string CardActionText(ClassType cls)
        {
            if (IsUnlocked(cls)) return "Click to Start";
            return cls == ClassType.Hexer
                ? "Unlock for 250 Echoes\nElementalist ile 1 run yap"
                : $"Unlock for {UnlockCost(cls)} Echoes";
        }

        private static string LockedButtonText(ClassType cls)
        {
            return cls == ClassType.Hexer
                ? "250 Echoes + Elementalist run"
                : $"{UnlockCost(cls)} Echoes required";
        }

        private static int UnlockCost(ClassType cls) => cls switch
        {
            ClassType.Ronin => 120,
            ClassType.Ravager => 120,
            ClassType.Gunslinger => 180,
            ClassType.Brawler => 180,
            ClassType.Summoner => 180,
            ClassType.Hexer => 250,
            _ => 0,
        };

        private static void ApplyCardLockVisual(CardEntry card, bool selected)
        {
            bool unlocked = IsUnlocked(card.classType);
            if (card.portrait != null)
            {
                card.portrait.color = unlocked
                    ? Color.white
                    : new Color(0.015f, 0.015f, 0.018f, selected ? 0.95f : 0.82f);
            }

            if (card.actionLabel != null)
            {
                card.actionLabel.text = CardActionText(card.classType);
                card.actionLabel.color = unlocked ? RimaUITheme.Cyan : RimaUITheme.TextMuted;
            }
        }

        private static Sprite LoadCanonicalSprite(ClassType cls)
        {
            // Load the imported sprite directly so Editor and build share the same native pivot and PPU64
            // (avoids the Sprite.Create pivot mismatch that shifted characters vertically in builds).
            return Resources.Load<Sprite>(RimaUITheme.AnchorPath(cls));
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
