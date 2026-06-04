using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using RIMA.Systems.Map;

namespace RIMA
{
    /// <summary>
    /// RIMA Character Select Screen — fully procedural, no prefab required.
    /// Layout: left roster list (10 classes) | center idle showcase | right identity + skill list
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
        private Image     identityAccentBar;
        private TMP_Text  identityMottoLabel;
        private TMP_Text  identityPlaystyleLabel;
        private TMP_Text  identityResourceLabel;
        private TMP_Text  identityLockLabel;
        private Button    startButton;
        private TMP_Text  startButtonLabel;
        private TMP_Text  skillEmptyLabel;
        private RectTransform skillContent;

        private RectTransform showcaseRoot;
        private RectTransform pedestalRoot;
        private Image pedestalGlowImage;
        private Image portraitGlowImage;
        private Image showcaseFlashImage;
        private float selectFlashTimer;
        private Vector2 showcaseBasePosition;

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
            public Image         portraitFrame;
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

        private void Update()
        {
            AnimateShowcase();
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

            var leftPanel   = MakePanel("CSP_Left",   bg);
            var centerPanel = MakePanel("CSP_Center", bg);
            var rightPanel  = MakePanel("CSP_Right",  bg);

            SetStretch(leftPanel,   new Vector2(0.00f, 0.05f), new Vector2(0.20f, 0.91f), new Vector2(16f, 8f), new Vector2(-8f,  -8f));
            SetStretch(centerPanel, new Vector2(0.20f, 0.05f), new Vector2(0.72f, 0.91f), new Vector2(8f,  8f), new Vector2(-8f,  -8f));
            SetStretch(rightPanel,  new Vector2(0.72f, 0.05f), new Vector2(1.00f, 0.91f), new Vector2(8f,  8f), new Vector2(-16f, -8f));

            SetImg(leftPanel,   RimaUITheme.PanelTint);
            SetImg(centerPanel, Color.clear);
            SetImg(rightPanel,  RimaUITheme.PanelTint);

            BuildRosterList(leftPanel);
            BuildCenterPanel(centerPanel);
            BuildSkillDetailPanel(rightPanel);
        }

        private void BuildRosterList(RectTransform parent)
        {
            var header = MakeText("CLASSES", parent, 10, FontStyles.Bold, RimaUITheme.TextMuted);
            header.alignment = TextAlignmentOptions.Center;
            var hRt = header.transform as RectTransform;
            hRt.anchorMin = new Vector2(0f, 1f); hRt.anchorMax = new Vector2(1f, 1f);
            hRt.pivot = new Vector2(0.5f, 1f);
            hRt.anchoredPosition = new Vector2(0f, -16f);
            hRt.sizeDelta = new Vector2(0f, 20f);

            float rowH = 0.076f;
            float rowGap = 0.008f;
            float listTop = 0.90f;

            for (int i = 0; i < AllClasses.Length; i++)
            {
                float y2 = listTop - i * (rowH + rowGap);
                float y1 = y2 - rowH;
                BuildClassCard(parent, AllClasses[i], 0.04f, y1, 0.96f, y2);
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

            // Left accent bar (selected only)
            var accentLineGo = MakePanel("AccentLine", cardRoot);
            SetStretch(accentLineGo, new Vector2(0f, 0f), new Vector2(0f, 1f),
                new Vector2(0f, 0f), new Vector2(4f, 0f));
            var accentImg = accentLineGo.GetComponent<Image>();
            accentImg.color = RimaUITheme.ClassAccent(cls);
            accentImg.raycastTarget = false;

            // Class name
            var nameLabel = MakeText(cls.ToString().ToUpperInvariant(), cardRoot, 12, FontStyles.Bold, RimaUITheme.TextPrimary);
            nameLabel.alignment = TextAlignmentOptions.Left;
            var nRt = nameLabel.transform as RectTransform;
            nRt.anchorMin = new Vector2(0f, 0.30f); nRt.anchorMax = new Vector2(1f, 0.86f);
            nRt.offsetMin = new Vector2(84f, 0f); nRt.offsetMax = new Vector2(-30f, 0f);

            var portraitFrame = MakePanel("PortraitFrame", cardRoot);
            portraitFrame.anchorMin = new Vector2(0f, 0.5f);
            portraitFrame.anchorMax = new Vector2(0f, 0.5f);
            portraitFrame.pivot = new Vector2(0.5f, 0.5f);
            portraitFrame.anchoredPosition = new Vector2(42f, 0f);
            portraitFrame.sizeDelta = new Vector2(58f, 58f);
            var frameImg = portraitFrame.GetComponent<Image>();
            frameImg.sprite = RimaUITheme.SmallPanelFrame;
            frameImg.type = Image.Type.Sliced;
            frameImg.color = new Color(1f, 1f, 1f, 0.12f);
            frameImg.raycastTarget = false;

            var portraitMask = MakePanel("PortraitMask", portraitFrame);
            SetStretch(portraitMask, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(5f, 5f), new Vector2(-5f, -5f));
            portraitMask.gameObject.AddComponent<RectMask2D>();
            var maskImg = portraitMask.GetComponent<Image>();
            maskImg.color = new Color(0f, 0f, 0f, 0.28f);
            maskImg.raycastTarget = false;

            var portraitRt = MakePanel("Portrait", portraitMask);
            SetStretch(portraitRt, new Vector2(-0.15f, -0.20f), new Vector2(1.15f, 1.18f), Vector2.zero, Vector2.zero);
            var portrait = portraitRt.GetComponent<Image>();
            portrait.sprite = LoadCanonicalSprite(cls);
            portrait.preserveAspect = true;
            portrait.raycastTarget = false;

            var actionLabel = MakeText(CardActionText(cls), cardRoot, 8, FontStyles.Normal, RimaUITheme.TextMuted);
            actionLabel.alignment = TextAlignmentOptions.Left;
            actionLabel.enableWordWrapping = true;
            var aRt = actionLabel.transform as RectTransform;
            aRt.anchorMin = new Vector2(0f, 0.02f); aRt.anchorMax = new Vector2(1f, 0.34f);
            aRt.offsetMin = new Vector2(84f, 0f); aRt.offsetMax = new Vector2(-30f, 0f);

            var lockLabel = MakeText("LOCKED", cardRoot, 14, FontStyles.Bold, RimaUITheme.TextMuted);
            lockLabel.alignment = TextAlignmentOptions.Center;
            var lockRt = lockLabel.transform as RectTransform;
            lockRt.anchorMin = new Vector2(1f, 0.50f); lockRt.anchorMax = new Vector2(1f, 0.50f);
            lockRt.pivot = new Vector2(0.5f, 0.5f);
            lockRt.anchoredPosition = new Vector2(-16f, 0f);
            lockRt.sizeDelta = new Vector2(24f, 24f);
            lockLabel.text = "X";

            // Button
            var btn = cardRoot.gameObject.AddComponent<Button>();
            var colors = btn.colors;
            colors.normalColor      = new Color(1f, 1f, 1f, 0f);
            colors.highlightedColor = new Color(1f, 1f, 1f, 0.08f);
            colors.pressedColor     = new Color(1f, 1f, 1f, 0.15f);
            btn.colors = colors;
            btn.targetGraphic = bg;
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
                var overlayFrameImg = frame.GetComponent<Image>();
                overlayFrameImg.sprite        = cardFrameSprite;
                overlayFrameImg.type          = Image.Type.Sliced;
                overlayFrameImg.color         = Color.white;
                overlayFrameImg.raycastTarget = false;
            }

            cards[cls] = new CardEntry
            {
                root = cardRoot, bg = bg, accentLine = accentImg,
                portraitFrame = frameImg, portrait = portrait, nameLabel = nameLabel,
                actionLabel = actionLabel, classType = cls
            };

            ApplyCardLockVisual(cards[cls], false);
        }

        private void BuildCenterPanel(RectTransform parent)
        {
            var columnBackdrop = MakePanel("CenterSealBackdrop", parent);
            SetStretch(columnBackdrop, new Vector2(0f, 0f), new Vector2(1f, 1f), Vector2.zero, Vector2.zero);
            var backdropImg = columnBackdrop.GetComponent<Image>();
            backdropImg.sprite = Resources.Load<Sprite>(PackBackdrop);
            backdropImg.type = Image.Type.Simple;
            backdropImg.preserveAspect = false;
            backdropImg.color = new Color(0.55f, 0.60f, 0.70f, 0.50f);
            backdropImg.raycastTarget = false;

            portraitGlowImage = MakePanel("PortraitAccentGlow", parent).GetComponent<Image>();
            var glowRt = portraitGlowImage.transform as RectTransform;
            glowRt.anchorMin = new Vector2(0.5f, 0.52f); glowRt.anchorMax = new Vector2(0.5f, 0.52f);
            glowRt.pivot = new Vector2(0.5f, 0.5f);
            glowRt.sizeDelta = new Vector2(520f, 760f);
            portraitGlowImage.sprite = RimaUITheme.SmallPanelFrame;
            portraitGlowImage.type = Image.Type.Sliced;
            portraitGlowImage.raycastTarget = false;

            // Seal-stone pedestal UNDER the character's feet. Added first so it
            // renders behind the portrait; skipped entirely if the sprite is missing.
            var pedestalSprite = Resources.Load<Sprite>(PackPedestal);
            if (pedestalSprite != null)
            {
                pedestalGlowImage = MakePanel("PedestalCyanGlow", parent).GetComponent<Image>();
                var glowPedRt = pedestalGlowImage.transform as RectTransform;
                glowPedRt.anchorMin = new Vector2(0.5f, 0.17f);
                glowPedRt.anchorMax = new Vector2(0.5f, 0.17f);
                glowPedRt.pivot = new Vector2(0.5f, 0.5f);
                glowPedRt.anchoredPosition = Vector2.zero;
                glowPedRt.sizeDelta = new Vector2(640f, 220f);
                pedestalGlowImage.sprite = pedestalSprite;
                pedestalGlowImage.type = Image.Type.Simple;
                pedestalGlowImage.preserveAspect = true;
                pedestalGlowImage.raycastTarget = false;

                var pedestal = MakePanel("Pedestal", parent);
                pedestalRoot = pedestal;
                pedestal.anchorMin = new Vector2(0.5f, 0.17f);
                pedestal.anchorMax = new Vector2(0.5f, 0.17f);
                pedestal.pivot     = new Vector2(0.5f, 0.5f);
                pedestal.anchoredPosition = Vector2.zero;
                pedestal.sizeDelta = new Vector2(520f, 190f);
                var pedImg = pedestal.GetComponent<Image>();
                pedImg.sprite         = pedestalSprite;
                pedImg.type           = Image.Type.Simple;
                pedImg.preserveAspect = true;
                pedImg.color          = Color.white;
                pedImg.raycastTarget  = false;
            }

            var portraitRoot = MakePanel("PortraitRoot", parent);
            showcaseRoot = portraitRoot;
            portraitRoot.anchorMin = new Vector2(0.5f, 0.16f);
            portraitRoot.anchorMax = new Vector2(0.5f, 0.16f);
            portraitRoot.pivot = new Vector2(0.5f, 0f);
            portraitRoot.anchoredPosition = showcaseBasePosition = Vector2.zero;
            portraitRoot.sizeDelta = new Vector2(620f, 760f);

            portraitImage = portraitRoot.GetComponent<Image>();
            portraitImage.preserveAspect = true;
            portraitImage.color = Color.white;
            portraitImage.raycastTarget = false;

            // Accent bar
            var accentBarGo = MakePanel("AccentBar", parent);
            SetStretch(accentBarGo, new Vector2(0.12f, 0.105f), new Vector2(0.88f, 0.118f), Vector2.zero, Vector2.zero);
            accentBar = accentBarGo.GetComponent<Image>();
            accentBar.raycastTarget = false;

            showcaseFlashImage = MakePanel("SelectionFlash", parent).GetComponent<Image>();
            var flashRt = showcaseFlashImage.transform as RectTransform;
            flashRt.anchorMin = Vector2.zero; flashRt.anchorMax = Vector2.one;
            flashRt.offsetMin = flashRt.offsetMax = Vector2.zero;
            showcaseFlashImage.color = Color.clear;
            showcaseFlashImage.raycastTarget = false;

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

        private void BuildSkillDetailPanel(RectTransform parent)
        {
            var header = MakeText("IDENTITY", parent, 10, FontStyles.Bold, RimaUITheme.TextMuted);
            header.alignment = TextAlignmentOptions.Center;
            var hRt = header.transform as RectTransform;
            hRt.anchorMin = new Vector2(0f, 1f); hRt.anchorMax = new Vector2(1f, 1f);
            hRt.pivot = new Vector2(0.5f, 1f);
            hRt.anchoredPosition = new Vector2(0f, -16f);
            hRt.sizeDelta = new Vector2(0f, 20f);

            var bar = MakePanel("IdentityAccent", parent);
            SetStretch(bar, new Vector2(0.06f, 0.80f), new Vector2(0.08f, 0.90f), Vector2.zero, Vector2.zero);
            identityAccentBar = bar.GetComponent<Image>();
            identityAccentBar.raycastTarget = false;

            identityMottoLabel = MakeText("", parent, 18, FontStyles.Bold, RimaUITheme.Cyan);
            identityMottoLabel.alignment = TextAlignmentOptions.Left;
            identityMottoLabel.enableWordWrapping = true;
            var mottoRt = identityMottoLabel.transform as RectTransform;
            mottoRt.anchorMin = new Vector2(0.12f, 0.78f); mottoRt.anchorMax = new Vector2(0.92f, 0.90f);
            mottoRt.offsetMin = mottoRt.offsetMax = Vector2.zero;

            identityPlaystyleLabel = MakeText("", parent, 13, FontStyles.Normal, RimaUITheme.TextMuted);
            identityPlaystyleLabel.alignment = TextAlignmentOptions.TopLeft;
            identityPlaystyleLabel.enableWordWrapping = true;
            var playstyleRt = identityPlaystyleLabel.transform as RectTransform;
            playstyleRt.anchorMin = new Vector2(0.08f, 0.66f); playstyleRt.anchorMax = new Vector2(0.92f, 0.76f);
            playstyleRt.offsetMin = playstyleRt.offsetMax = Vector2.zero;

            identityResourceLabel = MakeText("", parent, 13, FontStyles.Bold, RimaUITheme.TextPrimary);
            identityResourceLabel.alignment = TextAlignmentOptions.TopLeft;
            identityResourceLabel.enableWordWrapping = true;
            var resourceRt = identityResourceLabel.transform as RectTransform;
            resourceRt.anchorMin = new Vector2(0.08f, 0.58f); resourceRt.anchorMax = new Vector2(0.92f, 0.64f);
            resourceRt.offsetMin = resourceRt.offsetMax = Vector2.zero;

            identityLockLabel = MakeText("", parent, 11, FontStyles.Bold, RimaUITheme.TextMuted);
            identityLockLabel.alignment = TextAlignmentOptions.Left;
            identityLockLabel.enableWordWrapping = true;
            var lockRt = identityLockLabel.transform as RectTransform;
            lockRt.anchorMin = new Vector2(0.08f, 0.52f); lockRt.anchorMax = new Vector2(0.92f, 0.57f);
            lockRt.offsetMin = lockRt.offsetMax = Vector2.zero;

            var skillsHeader = MakeText("SKILLS", parent, 10, FontStyles.Bold, RimaUITheme.TextMuted);
            skillsHeader.alignment = TextAlignmentOptions.Left;
            var shRt = skillsHeader.transform as RectTransform;
            shRt.anchorMin = new Vector2(0.08f, 0.48f); shRt.anchorMax = new Vector2(0.92f, 0.51f);
            shRt.offsetMin = shRt.offsetMax = Vector2.zero;

            skillContent = MakeScrollArea(parent, "SkillScrollArea");

            skillEmptyLabel = MakeText("Yetenekler yakinda", parent, 13, FontStyles.Normal, RimaUITheme.TextMuted);
            skillEmptyLabel.alignment = TextAlignmentOptions.Center;
            var emptyRt = skillEmptyLabel.transform as RectTransform;
            emptyRt.anchorMin = new Vector2(0.08f, 0.24f); emptyRt.anchorMax = new Vector2(0.92f, 0.45f);
            emptyRt.offsetMin = emptyRt.offsetMax = Vector2.zero;

            BuildStartButton(parent);
            BuildBackButton(parent);
        }

        private void BuildStartButton(RectTransform parent)
        {
            var btnRoot = MakePanel("StartButton", parent);
            btnRoot.anchorMin = new Vector2(0.5f, 0f);
            btnRoot.anchorMax = new Vector2(0.5f, 0f);
            btnRoot.pivot = new Vector2(0.5f, 0f);
            btnRoot.anchoredPosition = new Vector2(0f, 70f);
            btnRoot.sizeDelta = new Vector2(300f, 50f);

            var bgImg = btnRoot.GetComponent<Image>();
            // On-brand 9-slice button background (filled center). Falls back to the
            // procedural ResourceFrame if the Pack sprite is missing.
            var buttonSprite = Resources.Load<Sprite>(PackButton);
            bgImg.sprite = buttonSprite != null ? buttonSprite : RimaUITheme.ResourceFrame;
            bgImg.type = Image.Type.Sliced;
            bgImg.color = RimaUITheme.ClassAccent(ClassType.Warblade);
            bgImg.raycastTarget = true;

            startButtonLabel = MakeText("SEC", btnRoot, 20, FontStyles.Bold, Color.white);
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

        private void BuildBackButton(RectTransform parent)
        {
            var btnRoot = MakePanel("BackButton", parent);
            btnRoot.anchorMin = new Vector2(0.5f, 0f);
            btnRoot.anchorMax = new Vector2(0.5f, 0f);
            btnRoot.pivot = new Vector2(0.5f, 0f);
            btnRoot.anchoredPosition = new Vector2(0f, 16f);
            btnRoot.sizeDelta = new Vector2(300f, 42f);

            var bgImg = btnRoot.GetComponent<Image>();
            bgImg.sprite = Resources.Load<Sprite>(PackButton) ?? RimaUITheme.ResourceFrame;
            bgImg.type = Image.Type.Sliced;
            bgImg.color = new Color(0.10f, 0.11f, 0.15f, 0.82f);
            bgImg.raycastTarget = true;

            var label = MakeText("GERI", btnRoot, 16, FontStyles.Bold, RimaUITheme.TextPrimary);
            label.alignment = TextAlignmentOptions.Center;
            var lRt = label.transform as RectTransform;
            lRt.anchorMin = Vector2.zero; lRt.anchorMax = Vector2.one;
            lRt.offsetMin = lRt.offsetMax = Vector2.zero;

            var button = btnRoot.gameObject.AddComponent<Button>();
            button.targetGraphic = bgImg;
            button.onClick.AddListener(OnBackClicked);
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
            if (portraitGlowImage != null) portraitGlowImage.color = WithAlpha(accent, 0.18f);

            classNameLabel.text  = cls.ToString().ToUpperInvariant();
            classNameLabel.color = accent;

            var (tl1, tl2) = RimaUITheme.ClassTagline(cls);
            tagline1Label.text = tl1;
            tagline2Label.text = tl2;
            RefreshIdentityPanel(cls);
            RefreshSkillList(cls);
            selectFlashTimer = 0.28f;

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
                    startButtonLabel.text = IsUnlocked(cls) ? "SEC" : LockedButtonText(cls).ToUpperInvariant();
                    startButtonLabel.fontSize = IsUnlocked(cls) ? 20f : 13f;
                }
            }
        }

        private void RefreshIdentityPanel(ClassType cls)
        {
            var accent = RimaUITheme.ClassAccent(cls);
            var (motto, playstyle, resource) = RimaUITheme.ClassIdentity(cls);

            if (identityAccentBar != null) identityAccentBar.color = accent;
            if (identityMottoLabel != null)
            {
                identityMottoLabel.text = motto;
                identityMottoLabel.color = accent;
            }
            if (identityPlaystyleLabel != null) identityPlaystyleLabel.text = playstyle;
            if (identityResourceLabel != null) identityResourceLabel.text = resource;
            if (identityLockLabel != null)
            {
                identityLockLabel.text = IsUnlocked(cls) ? "" : IdentityLockText(cls);
                identityLockLabel.color = RimaUITheme.TextMuted;
            }
        }

        private void RefreshSkillList(ClassType cls)
        {
            if (skillContent == null) return;

            for (int i = skillContent.childCount - 1; i >= 0; i--)
                Destroy(skillContent.GetChild(i).gameObject);

            var database = EnsureSkillDatabase();
            var skills = database != null
                ? database.GetAll()
                    .Where(s => s != null && s.classType == cls && !s.isPassive)
                    .ToList()
                : new List<SkillData>();

            bool hasSkills = skills.Count > 0;
            if (skillEmptyLabel != null) skillEmptyLabel.gameObject.SetActive(!hasSkills);
            if (!hasSkills) return;

            foreach (var skill in skills)
                BuildSkillRow(skillContent, skill, cls);
        }

        private void BuildSkillRow(RectTransform parent, SkillData skill, ClassType cls)
        {
            var row = MakePanel("Skill_" + SkillIconRegistry.Normalize(skill.skillName), parent);
            row.sizeDelta = new Vector2(0f, 78f);
            var layoutElement = row.gameObject.AddComponent<LayoutElement>();
            layoutElement.preferredHeight = 78f;
            layoutElement.minHeight = 78f;
            var rowImg = row.GetComponent<Image>();
            rowImg.sprite = RimaUITheme.SmallPanelFrame;
            rowImg.type = Image.Type.Sliced;
            rowImg.color = new Color(0.06f, 0.07f, 0.10f, 0.58f);
            rowImg.raycastTarget = false;

            var iconFrame = MakePanel("IconFrame", row);
            iconFrame.anchorMin = new Vector2(0f, 0.5f);
            iconFrame.anchorMax = new Vector2(0f, 0.5f);
            iconFrame.pivot = new Vector2(0.5f, 0.5f);
            iconFrame.anchoredPosition = new Vector2(34f, 0f);
            iconFrame.sizeDelta = new Vector2(48f, 48f);
            var frameImg = iconFrame.GetComponent<Image>();
            frameImg.sprite = RimaUITheme.SmallPanelFrame;
            frameImg.type = Image.Type.Sliced;
            frameImg.color = WithAlpha(RimaUITheme.ClassAccent(cls), 0.28f);
            frameImg.raycastTarget = false;

            var iconRt = MakePanel("Icon", iconFrame);
            SetStretch(iconRt, Vector2.zero, Vector2.one, new Vector2(4f, 4f), new Vector2(-4f, -4f));
            var iconImg = iconRt.GetComponent<Image>();
            iconImg.sprite = skill.icon != null ? skill.icon : RimaUITheme.PassiveIcon(skill.skillName);
            iconImg.preserveAspect = true;
            iconImg.color = Color.white;
            iconImg.raycastTarget = false;

            var name = MakeText(skill.skillName.ToUpperInvariant(), row, 12, FontStyles.Bold, RimaUITheme.ClassAccent(cls));
            name.alignment = TextAlignmentOptions.Left;
            var nameRt = name.transform as RectTransform;
            nameRt.anchorMin = new Vector2(0f, 0.55f); nameRt.anchorMax = new Vector2(1f, 0.94f);
            nameRt.offsetMin = new Vector2(70f, 0f); nameRt.offsetMax = new Vector2(-8f, 0f);

            var desc = MakeText(skill.description, row, 10, FontStyles.Normal, new Color(1f, 1f, 1f, 0.75f));
            desc.alignment = TextAlignmentOptions.TopLeft;
            desc.enableWordWrapping = true;
            desc.overflowMode = TextOverflowModes.Ellipsis;
            var descRt = desc.transform as RectTransform;
            descRt.anchorMin = new Vector2(0f, 0.06f); descRt.anchorMax = new Vector2(1f, 0.58f);
            descRt.offsetMin = new Vector2(70f, 0f); descRt.offsetMax = new Vector2(-8f, 0f);
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

        private void OnBackClicked()
        {
            UIManager.Instance?.ResumeGame();
            SceneManager.LoadScene("MainMenu");
            Destroy(gameObject);
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

        private static string IdentityLockText(ClassType cls)
        {
            return cls == ClassType.Hexer
                ? "250 Echoes + Elementalist run gerekli"
                : $"{UnlockCost(cls)} Echoes gerekli";
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
            float alpha = selected ? 1f : (unlocked ? 0.75f : 0.35f);

            if (card.root != null)
            {
                card.root.localScale = selected ? new Vector3(1.05f, 1.05f, 1f) : Vector3.one;
            }

            if (card.bg != null)
            {
                card.bg.color = selected
                    ? WithAlpha(RimaUITheme.SlotBg, 0.96f)
                    : WithAlpha(RimaUITheme.SlotLocked, unlocked ? 0.76f : 0.48f);
            }

            if (card.accentLine != null)
            {
                card.accentLine.enabled = selected;
                card.accentLine.color = RimaUITheme.ClassAccent(card.classType);
            }

            if (card.portraitFrame != null)
                card.portraitFrame.color = selected ? WithAlpha(RimaUITheme.ClassAccent(card.classType), 0.65f) : new Color(1f, 1f, 1f, unlocked ? 0.12f : 0.06f);

            if (card.portrait != null)
            {
                card.portrait.color = unlocked
                    ? new Color(1f, 1f, 1f, alpha)
                    : new Color(0.22f, 0.22f, 0.25f, alpha);
            }

            if (card.actionLabel != null)
            {
                card.actionLabel.text = unlocked ? "READY" : CardActionText(card.classType);
                card.actionLabel.color = unlocked ? RimaUITheme.Cyan : RimaUITheme.TextMuted;
            }

            if (card.nameLabel != null)
                card.nameLabel.alpha = alpha;
        }

        private static Sprite LoadCanonicalSprite(ClassType cls)
        {
            // Load the imported sprite directly so Editor and build share the same native pivot and PPU64
            // (avoids the Sprite.Create pivot mismatch that shifted characters vertically in builds).
            return Resources.Load<Sprite>(RimaUITheme.AnchorPath(cls));
        }

        // ─── Helpers ──────────────────────────────────────────────────────

        private void AnimateShowcase()
        {
            float t = Time.unscaledTime;
            float bob = Mathf.Sin(t * 2.25f) * 10f;

            if (showcaseRoot != null)
                showcaseRoot.anchoredPosition = showcaseBasePosition + new Vector2(0f, bob);

            float pulse = 0.5f + 0.5f * Mathf.Sin(t * 3.4f);
            if (pedestalGlowImage != null)
                pedestalGlowImage.color = new Color(0f, 1f, 0.80f, Mathf.Lerp(0.16f, 0.34f, pulse));

            if (pedestalRoot != null)
            {
                float scale = Mathf.Lerp(0.98f, 1.03f, pulse);
                pedestalRoot.localScale = new Vector3(scale, scale, 1f);
            }

            if (selectFlashTimer > 0f)
                selectFlashTimer = Mathf.Max(0f, selectFlashTimer - Time.unscaledDeltaTime);

            if (showcaseFlashImage != null)
            {
                float a = selectFlashTimer > 0f ? selectFlashTimer / 0.28f : 0f;
                showcaseFlashImage.color = new Color(0.78f, 1f, 0.96f, a * 0.32f);
            }
        }

        private static SkillDatabase EnsureSkillDatabase()
        {
            if (SkillDatabase.Instance != null) return SkillDatabase.Instance;

            var existing = FindAnyObjectByType<SkillDatabase>();
            if (existing != null) return existing;

            var go = new GameObject("SkillDatabase_Auto");
            DontDestroyOnLoad(go);
            return go.AddComponent<SkillDatabase>();
        }

        private static RectTransform MakeScrollArea(RectTransform parent, string name)
        {
            var root = MakePanel(name, parent);
            SetStretch(root, new Vector2(0.06f, 0.14f), new Vector2(0.94f, 0.47f), Vector2.zero, Vector2.zero);
            var rootImg = root.GetComponent<Image>();
            rootImg.color = new Color(0f, 0f, 0f, 0f);
            rootImg.raycastTarget = false;

            var viewport = MakePanel("Viewport", root);
            SetStretch(viewport, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            viewport.gameObject.AddComponent<Mask>().showMaskGraphic = false;
            var viewportImg = viewport.GetComponent<Image>();
            viewportImg.color = new Color(0f, 0f, 0f, 0.20f);
            viewportImg.raycastTarget = true;

            var content = new GameObject("Content", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
            content.transform.SetParent(viewport, false);
            var contentRt = content.GetComponent<RectTransform>();
            contentRt.anchorMin = new Vector2(0f, 1f);
            contentRt.anchorMax = new Vector2(1f, 1f);
            contentRt.pivot = new Vector2(0.5f, 1f);
            contentRt.anchoredPosition = Vector2.zero;
            contentRt.sizeDelta = Vector2.zero;

            var layout = content.GetComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(0, 0, 0, 0);
            layout.spacing = 8f;
            layout.childAlignment = TextAnchor.UpperCenter;
            layout.childControlWidth = true;
            layout.childControlHeight = false;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            var fitter = content.GetComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            var scroll = root.gameObject.AddComponent<ScrollRect>();
            scroll.viewport = viewport;
            scroll.content = contentRt;
            scroll.horizontal = false;
            scroll.vertical = true;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            scroll.scrollSensitivity = 24f;

            return contentRt;
        }

        private static Color WithAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

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
