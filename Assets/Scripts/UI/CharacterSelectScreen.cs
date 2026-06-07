using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using RIMA.Systems.Map;

namespace RIMA
{
    /// <summary>
    /// RIMA Character Select Screen — fully procedural, no prefab required.
    /// Layout: diegetic roster room (10 classes) | bottom identity + skill + action strip
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
        private const string PackCardFrame  = "UI/RIMA/Pack/card_frame_9slice";  // 256x384 portrait card frame (border 28)
        private const string PackButton     = "UI/RIMA/Pack/button_9slice";      // 192x64 filled button bg (border 16)
        private const string PackPanelFrame = "UI/RIMA/Pack/panel_frame_9slice";
        private const string RoomBackdrop   = "UI/RIMA/CharacterSelect/room_bg";
        private const int RoomBackdropRetryFrames = 10;
        private const string ClassUnlockPrefsPrefix = "rima_class_unlocked_";
        private const string HexerElementalistRunPrefsKey = "rima_hexer_elementalist_run";
        private static readonly Color LockedSilhouetteColor = new(0.039f, 0.020f, 0.063f, 1f);

        // ── Internal layout state ─────────────────────────────────────────
        private ClassType selectedClass = ClassType.Warblade;
        private readonly Dictionary<ClassType, RoomEntry> roomEntries = new();

        private TMP_Text  classNameLabel;
        private TMP_Text  tagline1Label;
        private TMP_Text  tagline2Label;
        private Image     accentBar;
        private Image     identityAccentBar;
        private TMP_Text  identityMottoLabel;
        private TMP_Text  identityPlaystyleLabel;
        private TMP_Text  identityResourceLabel;
        private TMP_Text  identityLockLabel;
        private Image     identityPortraitImage;
        private CanvasGroup identityPanelGroup;
        private Coroutine identityFadeRoutine;
        private RectTransform startButtonRoot;
        private Image     startButtonBorder;
        private Image     startButtonFill;
        private Button    startButton;
        private TMP_Text  startButtonLabel;
        private TMP_Text  echoBalanceLabel;
        private TMP_Text  skillEmptyLabel;
        private RectTransform skillContent;
        private RectTransform identityStatsRoot;
        private ClassType selectionFadeClass;
        private float selectionRingFadeTimer;
        private const float SelectionRingFadeDuration = 0.20f;

        [Header("Roster Layout")]
        [Tooltip("Backdrop-normalized anchors by class order: 0 Warblade, 1 Elementalist, 2 Ranger, 3 Shadowblade, 4 Ronin, 5 Ravager, 6 Gunslinger, 7 Brawler, 8 Summoner, 9 Hexer.")]
        [SerializeField] private Vector2[] rosterAnchors =
        {
            new(0.28f, 0.69f), new(0.42f, 0.69f), new(0.56f, 0.69f), new(0.70f, 0.69f),
            new(0.35f, 0.54f), new(0.29f, 0.40f), new(0.41f, 0.40f), new(0.53f, 0.40f),
            new(0.65f, 0.40f), new(0.59f, 0.54f)
        };

        [SerializeField] private Vector2 hitPaddingScale = new(1.15f, 1.10f);
        [SerializeField] private float footRingWidth = 160f;

        private const float RosterBandMinX = 0.225f;
        private const float RosterBandMaxX = 0.745f;
        private const float VisibleCharacterHeight = 127f;

        private static readonly ClassType[] AllClasses =
        {
            ClassType.Warblade, ClassType.Elementalist, ClassType.Ranger,
            ClassType.Shadowblade, ClassType.Ronin, ClassType.Ravager,
            ClassType.Gunslinger, ClassType.Brawler, ClassType.Summoner, ClassType.Hexer
        };

        private readonly struct RoomPlacement
        {
            public readonly ClassType classType;
            public readonly Vector2 anchor;
            public readonly Vector2 size;
            public readonly float baseScale;

            public RoomPlacement(ClassType classType, Vector2 anchor, Vector2 size, float baseScale)
            {
                this.classType = classType;
                this.anchor = anchor;
                this.size = size;
                this.baseScale = baseScale;
            }
        }

        private struct RoomEntry
        {
            public RectTransform root;
            public Image         sprite;
            public Image         hit;
            public Button        button;
            public Image         seal;
            public Image         glow;
            public RectTransform selectionVfxRoot;
            public Image         selectionGlowDisk;
            public Image         selectionRing;
            public Image[]       selectionMotes;
            public Image         lockChip;
            public TMP_Text      costChip;
            public CanvasGroup   canvasGroup;
            public ClassType     classType;
            public Vector2       basePosition;
            public float         baseScale;
        }

        private readonly struct CharacterFit
        {
            public readonly float canvas;
            public readonly float visibleHeight;
            public readonly float feetGap;
            public readonly float xBias;
            public readonly float visibleWidth;

            public CharacterFit(float canvas, float visibleHeight, float feetGap, float xBias, float visibleWidth)
            {
                this.canvas = canvas;
                this.visibleHeight = visibleHeight;
                this.feetGap = feetGap;
                this.xBias = xBias;
                this.visibleWidth = visibleWidth;
            }
        }

        private readonly struct SkillPresentation
        {
            public readonly string name;
            public readonly string description;
            public readonly string iconText;
            public readonly bool locked;
            public readonly string condition;
            public readonly Sprite iconSprite;

            public SkillPresentation(string name, string description, string iconText, bool locked, string condition, Sprite iconSprite = null)
            {
                this.name = name;
                this.description = description;
                this.iconText = iconText;
                this.locked = locked;
                this.condition = condition;
                this.iconSprite = iconSprite;
            }
        }

        // ─── Lifecycle ────────────────────────────────────────────────────

        private void Awake()
        {
            if (targetCanvas == null) targetCanvas = GetComponentInParent<Canvas>();
            // Attunement Chamber (v4) is the primary selection flow. The authored scene only
            // contains this classic screen, so attach the chamber bootstrap here; it hides the
            // classic canvas and re-shows it as the TAB fallback overlay.
            if (GetComponent<ChamberSelectBootstrap>() == null)
            {
                gameObject.AddComponent<ChamberSelectBootstrap>();
            }
        }

        private void Start()
        {
            EchoWallet.EnsureInitialized();
            BuildScreen();
            // Default to the previously-selected class (if still unlocked) instead of force-overwriting to Warblade,
            // since SelectClass now writes PlayerClassManager.SelectedClass immediately.
            var defaultClass = PlayerClassManager.SelectedClass;
            SelectClass(IsUnlocked(defaultClass) ? defaultClass : ClassType.Warblade);
        }

        private void Update()
        {
            AnimateRoomSelection();
        }

        // ─── Build ────────────────────────────────────────────────────────

        private void BuildScreen()
        {
            EnsureSkillDatabase();

            if (targetCanvas == null)
            {
                var canvasGO = new GameObject("CharacterSelectCanvas");
                canvasGO.transform.SetParent(transform);
                targetCanvas = canvasGO.AddComponent<Canvas>();
                targetCanvas.renderMode   = RenderMode.ScreenSpaceOverlay;
                targetCanvas.sortingOrder = 100;
                canvasGO.AddComponent<GraphicRaycaster>();
            }

            if (targetCanvas == null)
            {
                Debug.LogWarning("[CharacterSelectScreen] BuildScreen: target Canvas null, aborting build.");
                return;
            }

            EnsureRuntimeScaler(targetCanvas);
            if (targetCanvas.GetComponent<GraphicRaycaster>() == null)
                targetCanvas.gameObject.AddComponent<GraphicRaycaster>();

            RemoveExistingRuntimeRoot(targetCanvas.transform);
            DisableAuthoredCanvasChildren(targetCanvas.transform);

            var root = MakeRect("RuntimeRoot_CharSelect", targetCanvas.transform, Vector2.zero, Vector2.one);
            root.offsetMin = root.offsetMax = Vector2.zero;

            var roomLayer = MakeRect("RoomLayer", root, Vector2.zero, Vector2.one);
            roomLayer.offsetMin = roomLayer.offsetMax = Vector2.zero;
            BuildRosterRoom(roomLayer);

            BuildTopBar(root);

            var identityPanel = MakeFramedPanel("IdentityPopupPanel", root, new Vector2(0.000f, 0.137f), new Vector2(0.212f, 0.866f));
            identityPanel.offsetMin = new Vector2(0f, 0f);
            identityPanel.offsetMax = new Vector2(-8f, 0f);
            BuildIdentityPanel(identityPanel);

            var skillPanel = MakeFramedPanel("SkillZone", root, new Vector2(0.754f, 0.137f), new Vector2(1.000f, 0.866f));
            skillPanel.offsetMin = new Vector2(8f, 0f);
            skillPanel.offsetMax = Vector2.zero;
            BuildSkillPanel(skillPanel);

            var bottomStrip = MakePanel("BottomActionStrip", root);
            SetStretch(bottomStrip, Vector2.zero, new Vector2(1f, 0.11f), Vector2.zero, Vector2.zero);
            BuildActionStrip(bottomStrip);
            BuildBackButton(root);
            BuildStartButton(root);
        }

        private static void EnsureRuntimeScaler(Canvas canvas)
        {
            var scaler = canvas.GetComponent<CanvasScaler>();
            if (scaler == null) scaler = canvas.gameObject.AddComponent<CanvasScaler>();

            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
        }

        private static void RemoveExistingRuntimeRoot(Transform canvasTransform)
        {
            Transform oldRoot = canvasTransform.Find("RuntimeRoot_CharSelect");
            if (oldRoot != null) Destroy(oldRoot.gameObject);
        }

        private static void DisableAuthoredCanvasChildren(Transform canvasTransform)
        {
            foreach (Transform child in canvasTransform)
                child.gameObject.SetActive(false);
        }

        private void BuildRosterRoom(RectTransform parent)
        {
            roomEntries.Clear();
            Image backdropImage = RimaUITheme.CreateFullScreenBackdrop(parent, RoomBackdrop, RimaUITheme.CharSelectVoidBlack);
            NameRoomBackdropSprite(backdropImage);
            if (backdropImage != null && backdropImage.sprite == null)
                StartCoroutine(RetryLoadRoomBackdrop(backdropImage));

            var rosterContainer = MakeRect("RosterContainer", parent, new Vector2(RosterBandMinX, 0f), new Vector2(RosterBandMaxX, 1f));
            rosterContainer.offsetMin = rosterContainer.offsetMax = Vector2.zero;

            foreach (var placement in BuildRosterPlacements().OrderByDescending(p => p.anchor.y))
                BuildRoomCharacter(rosterContainer, placement);
        }

        private IEnumerable<RoomPlacement> BuildRosterPlacements()
        {
            for (int i = 0; i < AllClasses.Length; i++)
            {
                var cls = AllClasses[i];
                Vector2 anchor = i < rosterAnchors.Length ? rosterAnchors[i] : DefaultRosterAnchor(cls);
                var fit = FitFor(cls);
                yield return new RoomPlacement(cls, UnityAnchorInRosterBand(anchor), DisplaySizeFor(fit), 1f);
            }
        }

        private static Vector2 DefaultRosterAnchor(ClassType cls) => cls switch
        {
            ClassType.Warblade     => new Vector2(0.28f, 0.69f),
            ClassType.Elementalist => new Vector2(0.42f, 0.69f),
            ClassType.Ranger       => new Vector2(0.56f, 0.69f),
            ClassType.Shadowblade  => new Vector2(0.70f, 0.69f),
            ClassType.Ronin        => new Vector2(0.35f, 0.54f),
            ClassType.Ravager      => new Vector2(0.29f, 0.40f),
            ClassType.Gunslinger   => new Vector2(0.41f, 0.40f),
            ClassType.Brawler      => new Vector2(0.53f, 0.40f),
            ClassType.Summoner     => new Vector2(0.65f, 0.40f),
            ClassType.Hexer        => new Vector2(0.59f, 0.54f),
            _                      => new Vector2(0.50f, 0.54f),
        };

        private static Vector2 UnityAnchorInRosterBand(Vector2 mockupAnchor)
        {
            float x = Mathf.InverseLerp(RosterBandMinX, RosterBandMaxX, mockupAnchor.x);
            return new Vector2(Mathf.Clamp01(x), Mathf.Clamp01(1f - mockupAnchor.y));
        }

        private static Vector2 DisplaySizeFor(CharacterFit fit)
        {
            float imageSize = VisibleCharacterHeight * (fit.canvas / Mathf.Max(1f, fit.visibleHeight));
            return new Vector2(imageSize, imageSize);
        }

        private static Vector2 BoxSizeFor(Vector2 displaySize)
        {
            return new Vector2(Mathf.Max(248f, displaySize.x + 18f), displaySize.y + 70f);
        }

        private static float FootYInBox(Vector2 boxSize, Vector2 displaySize, Vector2 spritePivot)
        {
            return -boxSize.y * 0.5f + 50f + spritePivot.y * displaySize.y;
        }

        private Vector2 HitSizeFor(CharacterFit fit)
        {
            float width = VisibleCharacterHeight * fit.visibleWidth / Mathf.Max(1f, fit.visibleHeight) * hitPaddingScale.x;
            float height = VisibleCharacterHeight * hitPaddingScale.y;
            return new Vector2(width, height);
        }

        private static CharacterFit FitFor(ClassType cls) => cls switch
        {
            ClassType.Warblade     => new CharacterFit(120f, 61f, 30f, -0.5f, 38f),
            ClassType.Elementalist => new CharacterFit(120f, 60f, 30f, -0.5f, 30f),
            ClassType.Ranger       => new CharacterFit(128f, 61f, 34f, -3.0f, 35f),
            ClassType.Shadowblade  => new CharacterFit(124f, 61f, 32f, -0.5f, 30f),
            ClassType.Ronin        => new CharacterFit(128f, 62f, 34f, -0.5f, 28f),
            ClassType.Ravager      => new CharacterFit(124f, 60f, 32f, -0.5f, 38f),
            ClassType.Gunslinger   => new CharacterFit(124f, 61f, 32f, -0.5f, 36f),
            ClassType.Brawler      => new CharacterFit(120f, 60f, 30f, -0.5f, 34f),
            ClassType.Summoner     => new CharacterFit(124f, 61f, 32f, -2.0f, 33f),
            ClassType.Hexer        => new CharacterFit(124f, 61f, 32f, -0.5f, 32f),
            _                      => new CharacterFit(120f, 61f, 30f, -0.5f, 38f),
        };

        private void BuildTopBar(RectTransform root)
        {
            var bar = MakePanel("TopBar", root);
            SetStretch(bar, new Vector2(0f, 0.92f), Vector2.one, Vector2.zero, Vector2.zero);
            var bg = bar.GetComponent<Image>();
            bg.sprite = RimaUITheme.SmallPanelFrame;
            bg.type = Image.Type.Sliced;
            bg.color = RimaUITheme.CharSelectPanelFill;
            bg.raycastTarget = false;

            var title = MakeText(Loc.T("char_select.title"), bar, 22f, FontStyles.Bold, RimaUITheme.CharSelectParchment);
            title.alignment = TextAlignmentOptions.Left;
            title.characterSpacing = 18f;
            var titleRt = title.transform as RectTransform;
            titleRt.anchorMin = new Vector2(0f, 0f); titleRt.anchorMax = new Vector2(0.58f, 1f);
            titleRt.offsetMin = new Vector2(50f, 0f); titleRt.offsetMax = Vector2.zero;

            var wallet = MakePanel("TopEchoBalanceChip", bar);
            wallet.anchorMin = new Vector2(1f, 0.5f); wallet.anchorMax = new Vector2(1f, 0.5f);
            wallet.pivot = new Vector2(1f, 0.5f);
            wallet.anchoredPosition = new Vector2(-50f, 0f);
            wallet.sizeDelta = new Vector2(300f, 42f);
            var walletImg = wallet.GetComponent<Image>();
            walletImg.sprite = RimaUITheme.SmallPanelFrame;
            walletImg.type = Image.Type.Sliced;
            walletImg.color = RimaUITheme.CharSelectPanelFill;
            walletImg.raycastTarget = false;

            echoBalanceLabel = MakeText("", wallet, 14f, FontStyles.Bold, RimaUITheme.CharSelectTextBody);
            echoBalanceLabel.alignment = TextAlignmentOptions.Center;
            var echoRt = echoBalanceLabel.transform as RectTransform;
            echoRt.anchorMin = Vector2.zero; echoRt.anchorMax = Vector2.one;
            echoRt.offsetMin = echoRt.offsetMax = Vector2.zero;
            RefreshEchoLabel();
        }

        private IEnumerator RetryLoadRoomBackdrop(Image backdropImage)
        {
            for (int frame = 0; frame < RoomBackdropRetryFrames; frame++)
            {
                yield return null;

                if (backdropImage == null)
                    yield break;

                Sprite spr = LoadRoomBackdropSprite();
                if (spr == null)
                    continue;

                ApplyRoomBackdropSprite(backdropImage, spr);
                yield break;
            }
        }

        private static Sprite LoadRoomBackdropSprite()
        {
            Sprite spr = Resources.Load<Sprite>(RoomBackdrop);
            if (spr != null)
            {
                NameRoomBackdropSprite(spr);
                return spr;
            }

            Texture2D tex = Resources.Load<Texture2D>(RoomBackdrop);
            if (tex == null)
                return null;

            spr = Sprite.Create(
                tex,
                new Rect(0f, 0f, tex.width, tex.height),
                new Vector2(0.5f, 0.5f),
                100f,
                0,
                SpriteMeshType.FullRect);
            spr.name = tex.name;
            return spr;
        }

        private static void NameRoomBackdropSprite(Image backdropImage)
        {
            if (backdropImage != null)
                NameRoomBackdropSprite(backdropImage.sprite);
        }

        private static void NameRoomBackdropSprite(Sprite spr)
        {
            if (spr != null && string.IsNullOrEmpty(spr.name) && spr.texture != null)
                spr.name = spr.texture.name;
        }

        private static void ApplyRoomBackdropSprite(Image backdropImage, Sprite spr)
        {
            if (backdropImage == null || spr == null)
                return;

            RectTransform rt = backdropImage.rectTransform;
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;

            backdropImage.sprite = spr;
            backdropImage.color = Color.white;
            backdropImage.type = Image.Type.Simple;

            var fitter = backdropImage.GetComponent<AspectRatioFitter>() ?? backdropImage.gameObject.AddComponent<AspectRatioFitter>();
            fitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
            fitter.aspectRatio = spr.rect.width / Mathf.Max(1f, spr.rect.height);
        }

        private void BuildRoomCharacter(RectTransform parent, RoomPlacement placement)
        {
            var cls = placement.classType;
            var fit = FitFor(cls);
            bool unlocked = IsUnlocked(cls);
            Vector2 pivot = new(
                Mathf.Clamp01(0.5f + fit.xBias / Mathf.Max(1f, fit.canvas)),
                Mathf.Clamp01(fit.feetGap / Mathf.Max(1f, fit.canvas)));
            Vector2 boxSize = BoxSizeFor(placement.size);
            float footY = FootYInBox(boxSize, placement.size, pivot);
            var root = MakeRect($"RoomCharacter_{cls}", parent, placement.anchor, placement.anchor);
            root.pivot = new Vector2(0.5f, 0.5f);
            root.anchoredPosition = Vector2.zero;
            root.sizeDelta = boxSize;
            root.localScale = new Vector3(placement.baseScale, placement.baseScale, 1f);

            var canvasGroup = root.gameObject.AddComponent<CanvasGroup>();

            var glowRt = MakePanel("HoverGlow", root);
            glowRt.anchorMin = new Vector2(0.5f, 0.5f); glowRt.anchorMax = new Vector2(0.5f, 0.5f);
            glowRt.pivot = new Vector2(0.5f, 0.5f);
            glowRt.anchoredPosition = new Vector2(0f, footY);
            glowRt.sizeDelta = new Vector2(footRingWidth, footRingWidth * 0.61f);
            var glow = glowRt.GetComponent<Image>();
            glow.sprite = RimaUITheme.ProceduralFootRing;
            glow.type = Image.Type.Simple;
            glow.preserveAspect = true;
            glow.raycastTarget = false;

            Image seal = null;

            var selectionVfxRoot = BuildSelectionVfx(root, footY, footRingWidth);
            var selectionVfxImages = selectionVfxRoot.GetComponentsInChildren<Image>(true);
            var selectionGlowDisk = selectionVfxRoot.Find("SelectionGlowDisk").GetComponent<Image>();
            var selectionRing = selectionVfxRoot.Find("SelectionFootRing").GetComponent<Image>();
            var selectionMotes = selectionVfxImages
                .Where(img => img != null && img.name.StartsWith("GlowMote", StringComparison.Ordinal))
                .ToArray();

            var spriteRt = MakePanel("Sprite", root);
            spriteRt.anchorMin = new Vector2(0.5f, 0.5f); spriteRt.anchorMax = new Vector2(0.5f, 0.5f);
            spriteRt.pivot = pivot;
            spriteRt.anchoredPosition = new Vector2(0f, footY);
            spriteRt.sizeDelta = placement.size;
            var sprite = spriteRt.GetComponent<Image>();
            sprite.sprite = LoadCanonicalSprite(cls);
            sprite.preserveAspect = true;
            sprite.raycastTarget = false;

            var nameLabel = MakeText(cls.ToString().ToUpperInvariant(), root, 11f, FontStyles.Bold, RimaUITheme.CharSelectParchment);
            nameLabel.alignment = TextAlignmentOptions.Center;
            nameLabel.enableWordWrapping = false;
            nameLabel.overflowMode = TextOverflowModes.Ellipsis;
            var nameRt = nameLabel.transform as RectTransform;
            nameRt.anchorMin = new Vector2(0.5f, 0.5f); nameRt.anchorMax = new Vector2(0.5f, 0.5f);
            nameRt.pivot = new Vector2(0.5f, 0.5f);
            nameRt.anchoredPosition = new Vector2(0f, footY - 31f);
            nameRt.sizeDelta = new Vector2(boxSize.x - 22f, 18f);

            Image lockImg = null;
            TMP_Text costChip = null;
            if (!unlocked)
            {
                var lockRt = MakePanel("LockChip", root);
                lockRt.anchorMin = new Vector2(0.5f, 0.5f); lockRt.anchorMax = new Vector2(0.5f, 0.5f);
                lockRt.pivot = new Vector2(0.5f, 0.5f);
                lockRt.anchoredPosition = new Vector2(0f, footY - 11f);
                lockRt.sizeDelta = new Vector2(72f, 20f);
                lockImg = lockRt.GetComponent<Image>();
                lockImg.sprite = RimaUITheme.SmallPanelFrame;
                lockImg.type = Image.Type.Sliced;
                lockImg.color = WithAlpha(RimaUITheme.CharSelectButtonFill, 0.52f);
                lockImg.raycastTarget = false;

                var lockLabel = MakeText(Loc.T("char_select.locked"), lockRt, 8.5f, FontStyles.Bold, RimaUITheme.CharSelectTextBody);
                lockLabel.alignment = TextAlignmentOptions.Center;
                var lockLabelRt = lockLabel.transform as RectTransform;
                lockLabelRt.anchorMin = Vector2.zero; lockLabelRt.anchorMax = Vector2.one;
                lockLabelRt.offsetMin = lockLabelRt.offsetMax = Vector2.zero;

                var chipRt = MakeRect("CostLine", root, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
                chipRt.pivot = new Vector2(0.5f, 0.5f);
                chipRt.anchoredPosition = new Vector2(0f, footY - 52f);
                chipRt.sizeDelta = new Vector2(boxSize.x - 12f, 18f);

                costChip = MakeText(UnlockOrPathText(cls), chipRt, 7.2f, FontStyles.Bold, RimaUITheme.CharSelectTextBody);
                costChip.alignment = TextAlignmentOptions.Center;
                costChip.enableWordWrapping = false;
                costChip.overflowMode = TextOverflowModes.Ellipsis;
                var costRt = costChip.transform as RectTransform;
                costRt.anchorMin = Vector2.zero; costRt.anchorMax = Vector2.one;
                costRt.offsetMin = costRt.offsetMax = Vector2.zero;
            }

            var hitRt = MakePanel("Hit", root);
            hitRt.anchorMin = new Vector2(0.5f, 0.5f);
            hitRt.anchorMax = new Vector2(0.5f, 0.5f);
            hitRt.pivot = new Vector2(0.5f, 0.5f);
            hitRt.anchoredPosition = new Vector2(0f, footY + VisibleCharacterHeight * 0.5f);
            hitRt.sizeDelta = HitSizeFor(fit);
            var hit = hitRt.GetComponent<Image>();
            hit.color = Color.clear;
            hit.raycastTarget = true;

            var button = hitRt.gameObject.AddComponent<Button>();
            button.targetGraphic = hit;
            var colors = button.colors;
            colors.normalColor = Color.clear;
            colors.highlightedColor = Color.clear;
            colors.pressedColor = Color.clear;
            colors.selectedColor = Color.clear;
            colors.disabledColor = Color.clear;
            button.colors = colors;
            var captured = cls;
            button.onClick.AddListener(() => SelectClass(captured));

            var entry = new RoomEntry
            {
                root = root,
                sprite = sprite,
                hit = hit,
                button = button,
                seal = seal,
                glow = glow,
                selectionVfxRoot = selectionVfxRoot,
                selectionGlowDisk = selectionGlowDisk,
                selectionRing = selectionRing,
                selectionMotes = selectionMotes,
                lockChip = lockImg,
                costChip = costChip,
                canvasGroup = canvasGroup,
                classType = cls,
                basePosition = Vector2.zero,
                baseScale = placement.baseScale,
            };

            AddEventTrigger(hitRt.gameObject, EventTriggerType.PointerEnter, _ =>
            {
                if (selectedClass == captured) return;
                if (entry.glow != null)
                {
                    entry.glow.enabled = true;
                    entry.glow.color = WithAlpha(RimaUITheme.CharSelectCyan, 0.28f);
                }
            });
            AddEventTrigger(hitRt.gameObject, EventTriggerType.PointerExit, _ =>
            {
                ApplyRoomEntryVisual(entry, selectedClass == captured);
            });

            roomEntries[cls] = entry;
            ApplyRoomEntryVisual(entry, false);
        }

        private static RectTransform BuildSelectionVfx(RectTransform parent, float footY, float ringWidth)
        {
            float ringHeight = ringWidth * 0.61f;
            var root = MakeRect("SelectionUIGlowVFX", parent, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
            root.pivot = new Vector2(0.5f, 0.5f);
            root.anchoredPosition = new Vector2(0f, footY);
            root.sizeDelta = new Vector2(ringWidth, ringHeight);

            var glowDisk = MakePanel("SelectionGlowDisk", root);
            glowDisk.anchorMin = new Vector2(0.5f, 0.5f); glowDisk.anchorMax = new Vector2(0.5f, 0.5f);
            glowDisk.pivot = new Vector2(0.5f, 0.5f);
            glowDisk.anchoredPosition = Vector2.zero;
            glowDisk.sizeDelta = new Vector2(ringWidth, ringHeight);
            var glowDiskImg = glowDisk.GetComponent<Image>();
            glowDiskImg.sprite = null;
            glowDiskImg.type = Image.Type.Simple;
            glowDiskImg.preserveAspect = true;
            glowDiskImg.color = Color.clear;
            glowDiskImg.raycastTarget = false;

            var ring = MakePanel("SelectionFootRing", root);
            ring.anchorMin = new Vector2(0.5f, 0.5f); ring.anchorMax = new Vector2(0.5f, 0.5f);
            ring.pivot = new Vector2(0.5f, 0.5f);
            ring.anchoredPosition = Vector2.zero;
            ring.sizeDelta = new Vector2(ringWidth, ringHeight);
            var ringImg = ring.GetComponent<Image>();
            ringImg.sprite = RimaUITheme.ProceduralFootRing;
            ringImg.type = Image.Type.Simple;
            ringImg.preserveAspect = true;
            ringImg.color = WithAlpha(RimaUITheme.CharSelectCyan, 0.0f);
            ringImg.raycastTarget = false;

            root.gameObject.SetActive(false);
            return root;
        }

        private void BuildIdentityPanel(RectTransform parent)
        {
            identityPanelGroup = parent.gameObject.AddComponent<CanvasGroup>();
            identityPanelGroup.alpha = 0f;

            var portraitFrame = MakePanel("IdentityPortraitFrame", parent);
            portraitFrame.anchorMin = new Vector2(0.5f, 1f); portraitFrame.anchorMax = new Vector2(0.5f, 1f);
            portraitFrame.pivot = new Vector2(0.5f, 1f);
            portraitFrame.anchoredPosition = new Vector2(0f, -20f);
            portraitFrame.sizeDelta = new Vector2(86f, 86f);
            var portraitFrameImg = portraitFrame.GetComponent<Image>();
            portraitFrameImg.sprite = RimaUITheme.SmallPanelFrame;
            portraitFrameImg.type = Image.Type.Sliced;
            portraitFrameImg.color = RimaUITheme.CharSelectButtonFill;
            portraitFrameImg.raycastTarget = false;

            var portraitMask = MakePanel("IdentityPortraitMask", portraitFrame);
            SetStretch(portraitMask, Vector2.zero, Vector2.one, new Vector2(6f, 6f), new Vector2(-6f, -6f));
            portraitMask.gameObject.AddComponent<RectMask2D>();
            var maskImg = portraitMask.GetComponent<Image>();
            maskImg.color = Color.clear;
            maskImg.raycastTarget = false;

            var portraitRt = MakePanel("Portrait", portraitMask);
            SetStretch(portraitRt, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            identityPortraitImage = portraitRt.GetComponent<Image>();
            identityPortraitImage.sprite = LoadCanonicalSprite(selectedClass);
            identityPortraitImage.preserveAspect = true;
            identityPortraitImage.raycastTarget = false;

            var bar = MakePanel("IdentityAccent", parent);
            SetStretch(bar, new Vector2(0f, 0.05f), new Vector2(0.012f, 0.96f), Vector2.zero, Vector2.zero);
            identityAccentBar = bar.GetComponent<Image>();
            identityAccentBar.raycastTarget = false;

            classNameLabel = MakeText("WARBLADE", parent, 25, FontStyles.Bold, RimaUITheme.CharSelectCyan);
            classNameLabel.alignment = TextAlignmentOptions.Center;
            var cnRt = classNameLabel.transform as RectTransform;
            cnRt.anchorMin = new Vector2(0.05f, 0.72f); cnRt.anchorMax = new Vector2(0.95f, 0.80f);
            cnRt.offsetMin = cnRt.offsetMax = Vector2.zero;

            tagline1Label = MakeText("HEAVY · MELEE · RAGE", parent, 11, FontStyles.Bold, RimaUITheme.CharSelectTextBody);
            tagline1Label.alignment = TextAlignmentOptions.Center;
            var t1Rt = tagline1Label.transform as RectTransform;
            t1Rt.anchorMin = new Vector2(0.05f, 0.65f); t1Rt.anchorMax = new Vector2(0.95f, 0.72f);
            t1Rt.offsetMin = t1Rt.offsetMax = Vector2.zero;

            tagline2Label = MakeText("", parent, 11, FontStyles.Normal, RimaUITheme.CharSelectTextBody);
            tagline2Label.alignment = TextAlignmentOptions.Center;
            tagline2Label.enableWordWrapping = true;
            var t2Rt = tagline2Label.transform as RectTransform;
            t2Rt.anchorMin = new Vector2(0.08f, 0.55f); t2Rt.anchorMax = new Vector2(0.92f, 0.65f);
            t2Rt.offsetMin = t2Rt.offsetMax = Vector2.zero;

            identityMottoLabel = MakeText("", parent, 13, FontStyles.Bold, RimaUITheme.CharSelectParchment);
            identityMottoLabel.alignment = TextAlignmentOptions.Left;
            identityMottoLabel.enableWordWrapping = true;
            var mottoRt = identityMottoLabel.transform as RectTransform;
            mottoRt.anchorMin = new Vector2(0.08f, 0.43f); mottoRt.anchorMax = new Vector2(0.92f, 0.54f);
            mottoRt.offsetMin = mottoRt.offsetMax = Vector2.zero;

            identityPlaystyleLabel = MakeText("", parent, 10, FontStyles.Normal, RimaUITheme.CharSelectTextBody);
            identityPlaystyleLabel.alignment = TextAlignmentOptions.TopLeft;
            identityPlaystyleLabel.enableWordWrapping = true;
            var playstyleRt = identityPlaystyleLabel.transform as RectTransform;
            playstyleRt.anchorMin = new Vector2(0.08f, 0.30f); playstyleRt.anchorMax = new Vector2(0.92f, 0.42f);
            playstyleRt.offsetMin = playstyleRt.offsetMax = Vector2.zero;

            identityResourceLabel = MakeText("", parent, 10, FontStyles.Bold, RimaUITheme.CharSelectParchment);
            identityResourceLabel.alignment = TextAlignmentOptions.TopLeft;
            identityResourceLabel.enableWordWrapping = true;
            var resourceRt = identityResourceLabel.transform as RectTransform;
            resourceRt.anchorMin = new Vector2(0.08f, 0.22f); resourceRt.anchorMax = new Vector2(0.92f, 0.29f);
            resourceRt.offsetMin = resourceRt.offsetMax = Vector2.zero;

            identityLockLabel = MakeText("", parent, 10, FontStyles.Bold, RimaUITheme.CharSelectTextBody);
            identityLockLabel.alignment = TextAlignmentOptions.Left;
            identityLockLabel.enableWordWrapping = true;
            var lockRt = identityLockLabel.transform as RectTransform;
            lockRt.anchorMin = new Vector2(0.08f, 0.15f); lockRt.anchorMax = new Vector2(0.92f, 0.21f);
            lockRt.offsetMin = lockRt.offsetMax = Vector2.zero;

            identityStatsRoot = MakeRect("StatBars", parent, new Vector2(0.08f, 0.025f), new Vector2(0.92f, 0.145f));
            identityStatsRoot.offsetMin = identityStatsRoot.offsetMax = Vector2.zero;
            var statLayout = identityStatsRoot.gameObject.AddComponent<VerticalLayoutGroup>();
            statLayout.spacing = 4f;
            statLayout.childControlWidth = true;
            statLayout.childControlHeight = false;
            statLayout.childForceExpandWidth = true;
            statLayout.childForceExpandHeight = false;
        }

        private void BuildSkillPanel(RectTransform parent)
        {
            var skillsHeader = MakeText(Loc.T("char_select.skills"), parent, 13, FontStyles.Bold, RimaUITheme.CharSelectParchment);
            skillsHeader.alignment = TextAlignmentOptions.Left;
            var shRt = skillsHeader.transform as RectTransform;
            shRt.anchorMin = new Vector2(0f, 0.93f); shRt.anchorMax = new Vector2(1f, 1f);
            shRt.offsetMin = new Vector2(14f, 0f); shRt.offsetMax = new Vector2(-8f, 0f);

            skillContent = MakeSkillStripArea(parent, "SkillStripArea");

            skillEmptyLabel = MakeText(Loc.T("char_select.skills_soon"), parent, 13, FontStyles.Normal, RimaUITheme.CharSelectTextBody);
            skillEmptyLabel.alignment = TextAlignmentOptions.Center;
            var emptyRt = skillEmptyLabel.transform as RectTransform;
            emptyRt.anchorMin = new Vector2(0f, 0f); emptyRt.anchorMax = new Vector2(1f, 0.92f);
            emptyRt.offsetMin = new Vector2(10f, 0f); emptyRt.offsetMax = new Vector2(-10f, 0f);

            EnsureTooltipSystem();
        }

        private void BuildActionStrip(RectTransform parent)
        {
            var bg = parent.GetComponent<Image>();
            bg.sprite = RimaUITheme.SmallPanelFrame;
            bg.type = Image.Type.Sliced;
            bg.color = RimaUITheme.CharSelectPanelFill;
            bg.raycastTarget = false;

            var topBorder = MakePanel("MutedTopBorder", parent);
            SetStretch(topBorder, new Vector2(0f, 1f), Vector2.one, new Vector2(0f, -2f), Vector2.zero);
            var topBorderImg = topBorder.GetComponent<Image>();
            topBorderImg.color = RimaUITheme.CharSelectDivider;
            topBorderImg.raycastTarget = false;

        }

        private void BuildStartButton(RectTransform parent)
        {
            var btnRoot = MakePanel("StartButton", parent);
            startButtonRoot = btnRoot;
            btnRoot.anchorMin = new Vector2(0.50f, 0.06f);
            btnRoot.anchorMax = new Vector2(0.50f, 0.06f);
            btnRoot.pivot = new Vector2(0.5f, 0.5f);
            btnRoot.anchoredPosition = Vector2.zero;
            btnRoot.sizeDelta = new Vector2(360f, 52f);

            startButtonBorder = btnRoot.GetComponent<Image>();
            startButtonBorder.sprite = Resources.Load<Sprite>(PackPanelFrame) ?? RimaUITheme.ResourceFrame;
            startButtonBorder.type = Image.Type.Sliced;
            startButtonBorder.color = RimaUITheme.CharSelectIronGrey;
            startButtonBorder.raycastTarget = true;

            var fillRt = MakePanel("StartButtonFill", btnRoot);
            SetStretch(fillRt, Vector2.zero, Vector2.one, new Vector2(3f, 3f), new Vector2(-3f, -3f));
            startButtonFill = fillRt.GetComponent<Image>();
            startButtonFill.sprite = Resources.Load<Sprite>(PackButton) ?? RimaUITheme.ResourceFrame;
            startButtonFill.type = Image.Type.Sliced;
            startButtonFill.color = RimaUITheme.CharSelectButtonFill;
            startButtonFill.raycastTarget = false;

            startButtonLabel = MakeText(Loc.T("char_select.btn.select"), btnRoot, 21, FontStyles.Bold, RimaUITheme.CharSelectParchment);
            startButtonLabel.alignment = TextAlignmentOptions.Center;
            startButtonLabel.enableWordWrapping = true;
            startButtonLabel.overflowMode = TextOverflowModes.Ellipsis;
            startButtonLabel.characterSpacing = 12f;
            var lRt = startButtonLabel.transform as RectTransform;
            lRt.anchorMin = Vector2.zero; lRt.anchorMax = Vector2.one;
            lRt.offsetMin = lRt.offsetMax = Vector2.zero;

            startButton = btnRoot.gameObject.AddComponent<Button>();
            startButton.targetGraphic = startButtonBorder;
            startButton.onClick.AddListener(OnStartRun);

            var colors = startButton.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = Color.white;
            colors.pressedColor = Color.white;
            colors.selectedColor = Color.white;
            colors.disabledColor = Color.white;
            startButton.colors = colors;

            AddPlateButtonFeedback(btnRoot, startButton, startButtonBorder,
                RimaUITheme.CharSelectIronGrey, RimaUITheme.CharSelectOrange, 1.05f, 1.02f);
        }

        private void BuildBackButton(RectTransform parent)
        {
            var btnRoot = MakePanel("BackButton", parent);
            btnRoot.anchorMin = new Vector2(0.28f, 0.06f);
            btnRoot.anchorMax = new Vector2(0.28f, 0.06f);
            btnRoot.pivot = new Vector2(0.5f, 0.5f);
            btnRoot.anchoredPosition = Vector2.zero;
            btnRoot.sizeDelta = new Vector2(180f, 44f);

            var borderImg = btnRoot.GetComponent<Image>();
            borderImg.sprite = Resources.Load<Sprite>(PackPanelFrame) ?? RimaUITheme.ResourceFrame;
            borderImg.type = Image.Type.Sliced;
            borderImg.color = RimaUITheme.CharSelectIronGrey;
            borderImg.raycastTarget = true;

            var fillRt = MakePanel("BackButtonFill", btnRoot);
            SetStretch(fillRt, Vector2.zero, Vector2.one, new Vector2(2f, 2f), new Vector2(-2f, -2f));
            var fillImg = fillRt.GetComponent<Image>();
            fillImg.sprite = Resources.Load<Sprite>(PackButton) ?? RimaUITheme.ResourceFrame;
            fillImg.type = Image.Type.Sliced;
            fillImg.color = RimaUITheme.CharSelectButtonFill;
            fillImg.raycastTarget = false;

            var label = MakeText(Loc.T("char_select.btn.back"), btnRoot, 16, FontStyles.Bold, new Color(0.620f, 0.620f, 0.620f, 1f));
            label.alignment = TextAlignmentOptions.Center;
            label.characterSpacing = 8f;
            var lRt = label.transform as RectTransform;
            lRt.anchorMin = Vector2.zero; lRt.anchorMax = Vector2.one;
            lRt.offsetMin = lRt.offsetMax = Vector2.zero;

            var button = btnRoot.gameObject.AddComponent<Button>();
            button.targetGraphic = borderImg;
            button.onClick.AddListener(OnBackClicked);

            var colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = Color.white;
            colors.pressedColor = Color.white;
            colors.selectedColor = Color.white;
            colors.disabledColor = Color.white;
            button.colors = colors;

            AddPlateButtonFeedback(btnRoot, button, borderImg,
                RimaUITheme.CharSelectIronGrey, RimaUITheme.CharSelectCyan, 1.02f, 1.00f);
        }

        private static void AddPlateButtonFeedback(RectTransform root, Button button, Image border,
            Color normalBorder, Color hoverBorder, float hoverScale, float pressedScale)
        {
            AddEventTrigger(root.gameObject, EventTriggerType.PointerEnter, _ =>
            {
                if (button != null && !button.interactable) return;
                if (border != null) border.color = hoverBorder;
                root.localScale = new Vector3(hoverScale, hoverScale, 1f);
            });
            AddEventTrigger(root.gameObject, EventTriggerType.PointerExit, _ =>
            {
                if (border != null) border.color = normalBorder;
                root.localScale = Vector3.one;
            });
            AddEventTrigger(root.gameObject, EventTriggerType.PointerDown, _ =>
            {
                if (button != null && !button.interactable) return;
                root.localScale = new Vector3(pressedScale, pressedScale, 1f);
            });
            AddEventTrigger(root.gameObject, EventTriggerType.PointerUp, _ =>
            {
                if (button != null && !button.interactable) return;
                root.localScale = new Vector3(hoverScale, hoverScale, 1f);
            });
        }

        // ─── Selection Logic ──────────────────────────────────────────────

        private void SelectClass(ClassType cls)
        {
            selectedClass = cls;
            bool unlocked = IsUnlocked(cls);
            selectionFadeClass = cls;
            selectionRingFadeTimer = SelectionRingFadeDuration;

            if (unlocked)
                PlayerClassManager.SelectedClass = cls;

            foreach (var kv in roomEntries)
                ApplyRoomEntryVisual(kv.Value, kv.Key == cls);

            if (roomEntries.TryGetValue(cls, out var fadeEntry))
            {
                if (fadeEntry.selectionGlowDisk != null)
                    fadeEntry.selectionGlowDisk.color = Color.clear;
                if (fadeEntry.selectionRing != null)
                    fadeEntry.selectionRing.color = WithAlpha(RimaUITheme.CharSelectCyan, 0f);
            }

            if (accentBar != null) accentBar.color = RimaUITheme.CharSelectDivider;

            if (classNameLabel != null)
            {
                classNameLabel.text  = cls.ToString().ToUpperInvariant();
                classNameLabel.color = RimaUITheme.CharSelectCyan;
            }

            if (identityPortraitImage != null)
            {
                identityPortraitImage.sprite = LoadCanonicalSprite(cls);
                float portraitAlpha = identityPortraitImage.color.a;
                identityPortraitImage.color = unlocked
                    ? Color.white
                    : WithAlpha(LockedSilhouetteColor, portraitAlpha);
            }

            var (tl1, tl2) = RimaUITheme.ClassTagline(cls);
            if (tagline1Label != null) tagline1Label.text = tl1;
            if (tagline2Label != null) tagline2Label.text = tl2;
            RefreshIdentityPanel(cls);
            ShowIdentityPopup();
            RefreshSkillList(cls);

            if (startButton != null)
            {
                bool canUnlock = CanUnlock(cls);
                startButton.interactable = unlocked || canUnlock;
                if (startButtonRoot != null) startButtonRoot.localScale = Vector3.one;
                if (startButtonBorder != null) startButtonBorder.color = RimaUITheme.CharSelectIronGrey;
                if (startButtonFill != null)
                {
                    var fill = RimaUITheme.CharSelectButtonFill;
                    fill.a = unlocked || canUnlock ? 1f : 0.50f;
                    startButtonFill.color = fill;
                }
                if (startButtonLabel != null)
                {
                    startButtonLabel.text = unlocked
                        ? Loc.T("char_select.btn.select")
                        : canUnlock
                            ? Loc.T("char_select.btn.unlock", LockedButtonText(cls))
                            : Loc.T("char_select.not_enough_echo");
                    startButtonLabel.fontSize = unlocked ? 21f : 12f;
                    startButtonLabel.color = unlocked
                        ? RimaUITheme.CharSelectParchment
                        : canUnlock
                            ? RimaUITheme.CharSelectOrange
                            : WithAlpha(RimaUITheme.CharSelectTextBody, 0.65f);
                }
            }
        }

        private void RefreshIdentityPanel(ClassType cls)
        {
            var (motto, playstyle, resource) = RimaUITheme.ClassIdentity(cls);

            if (identityAccentBar != null) identityAccentBar.color = RimaUITheme.CharSelectDivider;
            if (identityMottoLabel != null)
            {
                identityMottoLabel.text = motto;
                identityMottoLabel.color = RimaUITheme.CharSelectParchment;
            }
            if (identityPlaystyleLabel != null) identityPlaystyleLabel.text = playstyle;
            if (identityResourceLabel != null) identityResourceLabel.text = resource;
            if (identityLockLabel != null)
            {
                identityLockLabel.text = IsUnlocked(cls) ? "" : IdentityLockText(cls);
                identityLockLabel.color = RimaUITheme.CharSelectTextBody;
            }
            RefreshStatBars(cls);
        }

        private void RefreshStatBars(ClassType cls)
        {
            if (identityStatsRoot == null) return;

            for (int i = identityStatsRoot.childCount - 1; i >= 0; i--)
            {
                var c = identityStatsRoot.GetChild(i);
                c.SetParent(null, false);
                Destroy(c.gameObject);
            }

            var stats = RimaUITheme.ClassStats(cls);
            BuildStatRow(identityStatsRoot, Loc.T("char_select.stats.damage"), stats.damage);
            BuildStatRow(identityStatsRoot, Loc.T("char_select.stats.durability"), stats.durability);
            BuildStatRow(identityStatsRoot, Loc.T("char_select.stats.speed"), stats.speed);
            BuildStatRow(identityStatsRoot, Loc.T("char_select.stats.control"), stats.control);
            BuildStatRow(identityStatsRoot, Loc.T("char_select.stats.difficulty"), stats.difficulty);
        }

        private static void BuildStatRow(RectTransform parent, string label, int value)
        {
            var row = MakeRect("Stat_" + label, parent, Vector2.zero, Vector2.one);
            row.sizeDelta = new Vector2(0f, 15f);
            var layoutElement = row.gameObject.AddComponent<LayoutElement>();
            layoutElement.preferredHeight = 15f;
            layoutElement.minHeight = 15f;

            var labelText = MakeText(label, row, 7.5f, FontStyles.Bold, RimaUITheme.CharSelectTextBody);
            labelText.alignment = TextAlignmentOptions.Left;
            var labelRt = labelText.transform as RectTransform;
            labelRt.anchorMin = new Vector2(0f, 0f); labelRt.anchorMax = new Vector2(0.36f, 1f);
            labelRt.offsetMin = labelRt.offsetMax = Vector2.zero;

            var bar = MakeRect("Segments", row, new Vector2(0.40f, 0.18f), new Vector2(1f, 0.82f));
            bar.offsetMin = bar.offsetMax = Vector2.zero;
            var grid = bar.gameObject.AddComponent<GridLayoutGroup>();
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = 10;
            grid.spacing = new Vector2(2f, 0f);
            grid.cellSize = new Vector2(10f, 8f);

            int clamped = Mathf.Clamp(value, 0, 10);
            for (int i = 0; i < 10; i++)
            {
                var seg = MakePanel("Seg_" + i, bar);
                var img = seg.GetComponent<Image>();
                img.color = RimaUITheme.CharSelectIronGrey;
                img.raycastTarget = false;

                var fill = MakePanel("Fill", seg);
                SetStretch(fill, Vector2.zero, Vector2.one, new Vector2(1f, 1f), new Vector2(-1f, -1f));
                var fillImg = fill.GetComponent<Image>();
                fillImg.color = i < clamped
                    ? RimaUITheme.CharSelectStatFill
                    : RimaUITheme.CharSelectStatEmpty;
                fillImg.raycastTarget = false;
            }
        }

        private void ShowIdentityPopup()
        {
            if (identityPanelGroup == null) return;

            if (identityFadeRoutine != null)
                StopCoroutine(identityFadeRoutine);

            identityFadeRoutine = StartCoroutine(FadeIdentityPopup());
        }

        private IEnumerator FadeIdentityPopup()
        {
            const float duration = 0.15f;
            float startAlpha = identityPanelGroup.alpha;

            for (float elapsed = 0f; elapsed < duration; elapsed += Time.unscaledDeltaTime)
            {
                identityPanelGroup.alpha = Mathf.Lerp(startAlpha, 1f, Mathf.Clamp01(elapsed / duration));
                yield return null;
            }

            identityPanelGroup.alpha = 1f;
            identityFadeRoutine = null;
        }

        private void RefreshSkillList(ClassType cls)
        {
            if (skillContent == null) return;

            for (int i = skillContent.childCount - 1; i >= 0; i--)
            {
                var c = skillContent.GetChild(i);
                c.SetParent(null, false);
                Destroy(c.gameObject);
            }

            var skills = SkillPresentationFor(cls);
            bool hasSkills = skills.Count > 0;
            if (skillEmptyLabel != null) skillEmptyLabel.gameObject.SetActive(!hasSkills);
            if (!hasSkills) return;

            int featuredCount = Mathf.Min(3, skills.Count);
            for (int i = 0; i < featuredCount; i++)
                BuildSkillRow(skillContent, skills[i], cls, true);

            if (skills.Count > featuredCount)
            {
                var section = MakeText(Loc.T("char_select.full_list"), skillContent, 8.5f, FontStyles.Bold, RimaUITheme.CharSelectTextBody);
                section.alignment = TextAlignmentOptions.Left;
                var sectionRt = section.transform as RectTransform;
                sectionRt.sizeDelta = new Vector2(0f, 18f);
                var sectionLayout = section.gameObject.AddComponent<LayoutElement>();
                sectionLayout.preferredHeight = 18f;
                sectionLayout.minHeight = 18f;

                for (int i = featuredCount; i < skills.Count; i++)
                    BuildSkillRow(skillContent, skills[i], cls, false);
            }
        }

        private void BuildSkillRow(RectTransform parent, SkillPresentation skill, ClassType cls, bool featured)
        {
            var row = MakePanel("Skill_" + SkillIconRegistry.Normalize(skill.name), parent);
            row.sizeDelta = new Vector2(0f, featured ? 48f : 25f);
            var layoutElement = row.gameObject.AddComponent<LayoutElement>();
            layoutElement.preferredWidth = 0f;
            layoutElement.minWidth = 0f;
            layoutElement.preferredHeight = featured ? 48f : 25f;
            layoutElement.minHeight = featured ? 48f : 25f;
            var rowImg = row.GetComponent<Image>();
            rowImg.sprite = RimaUITheme.SmallPanelFrame;
            rowImg.type = Image.Type.Sliced;
            rowImg.color = skill.locked
                ? new Color(0f, 0f, 0f, 0.42f)
                : WithAlpha(RimaUITheme.CharSelectPanelFill, featured ? 0.76f : 0.54f);
            rowImg.raycastTarget = !skill.locked;

            var iconFrame = MakePanel("IconFrame", row);
            iconFrame.anchorMin = new Vector2(0f, 1f);
            iconFrame.anchorMax = new Vector2(0f, 1f);
            iconFrame.pivot = new Vector2(0.5f, 0.5f);
            iconFrame.anchoredPosition = new Vector2(featured ? 24f : 15f, featured ? -24f : -12.5f);
            iconFrame.sizeDelta = featured ? new Vector2(34f, 34f) : new Vector2(18f, 18f);
            var frameImg = iconFrame.GetComponent<Image>();
            frameImg.sprite = RimaUITheme.SmallPanelFrame;
            frameImg.type = Image.Type.Sliced;
            frameImg.color = skill.locked
                ? WithAlpha(RimaUITheme.CharSelectLockedText, 0.70f)
                : featured
                    ? WithAlpha(RimaUITheme.CharSelectCyan, 0.26f)
                    : WithAlpha(RimaUITheme.CharSelectIronGrey, 0.72f);
            frameImg.raycastTarget = false;

            if (skill.iconSprite != null && !skill.locked)
            {
                var iconRt = MakePanel("Icon", iconFrame);
                SetStretch(iconRt, Vector2.zero, Vector2.one, new Vector2(3f, 3f), new Vector2(-3f, -3f));
                var iconImg = iconRt.GetComponent<Image>();
                iconImg.sprite = skill.iconSprite;
                iconImg.preserveAspect = true;
                iconImg.color = Color.white;
                iconImg.raycastTarget = false;
            }
            else
            {
                var iconLabel = MakeText(skill.locked ? "?" : skill.iconText, iconFrame, featured ? 12f : 8f, FontStyles.Bold,
                    skill.locked ? RimaUITheme.CharSelectTextBody : RimaUITheme.CharSelectParchment);
                iconLabel.alignment = TextAlignmentOptions.Center;
                var iconLabelRt = iconLabel.transform as RectTransform;
                iconLabelRt.anchorMin = Vector2.zero; iconLabelRt.anchorMax = Vector2.one;
                iconLabelRt.offsetMin = iconLabelRt.offsetMax = Vector2.zero;
            }

            var name = MakeText(skill.name?.ToUpperInvariant() ?? "UNKNOWN", row, featured ? 10f : 8f, FontStyles.Bold,
                skill.locked ? RimaUITheme.CharSelectLockedText : RimaUITheme.CharSelectParchment);
            name.alignment = TextAlignmentOptions.TopLeft;
            name.enableWordWrapping = false;
            name.overflowMode = TextOverflowModes.Ellipsis;
            var nameRt = name.transform as RectTransform;
            if (featured)
            {
                nameRt.anchorMin = new Vector2(0f, 0.56f); nameRt.anchorMax = new Vector2(1f, 0.94f);
                nameRt.offsetMin = new Vector2(48f, 0f); nameRt.offsetMax = new Vector2(-6f, 0f);

                var desc = MakeText(OneLine(skill.description), row, 7.5f, FontStyles.Normal, WithAlpha(RimaUITheme.CharSelectTextBody, 0.86f));
                desc.alignment = TextAlignmentOptions.TopLeft;
                desc.enableWordWrapping = false;
                desc.overflowMode = TextOverflowModes.Ellipsis;
                var descRt = desc.transform as RectTransform;
                descRt.anchorMin = new Vector2(0f, 0.10f); descRt.anchorMax = new Vector2(1f, 0.54f);
                descRt.offsetMin = new Vector2(48f, 0f); descRt.offsetMax = new Vector2(-6f, 0f);
            }
            else
            {
                nameRt.anchorMin = new Vector2(0f, 0f); nameRt.anchorMax = skill.locked ? new Vector2(0.58f, 1f) : new Vector2(1f, 1f);
                nameRt.offsetMin = new Vector2(30f, 0f); nameRt.offsetMax = new Vector2(-4f, 0f);

                if (skill.locked)
                {
                    var condition = MakeText(skill.condition, row, 7f, FontStyles.Normal, RimaUITheme.CharSelectLockedText);
                    condition.alignment = TextAlignmentOptions.Right;
                    condition.enableWordWrapping = false;
                    condition.overflowMode = TextOverflowModes.Ellipsis;
                    var conditionRt = condition.transform as RectTransform;
                    conditionRt.anchorMin = new Vector2(0.58f, 0f); conditionRt.anchorMax = Vector2.one;
                    conditionRt.offsetMin = new Vector2(2f, 0f); conditionRt.offsetMax = new Vector2(-4f, 0f);
                }
            }

            if (!skill.locked && !string.IsNullOrWhiteSpace(skill.description))
                AddSkillTooltip(row.gameObject, $"<b>{skill.name}</b>\n{skill.description}");
        }

        private void OnStartRun()
        {
            if (!IsUnlocked(selectedClass))
            {
                TryUnlockSelectedClass();
                return;
            }

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
            cls == ClassType.Shadowblade ||
            PlayerPrefs.GetInt(UnlockPrefKey(cls), 0) == 1;

        private static string UnlockPrefKey(ClassType cls) => ClassUnlockPrefsPrefix + cls;

        private static string CardActionText(ClassType cls)
        {
            if (IsUnlocked(cls)) return "SEÇ";
            return Loc.T("char_select.unlock_condition", UnlockCost(cls), UnlockConditionText(cls));
        }

        private static string LockedButtonText(ClassType cls)
        {
            return $"{UnlockCost(cls)} SHATTERED ECHO";
        }

        private static string IdentityLockText(ClassType cls)
        {
            return $"{UnlockCost(cls)} SHATTERED ECHO · veya {UnlockConditionText(cls)}";
        }

        private static int UnlockCost(ClassType cls) => cls switch
        {
            ClassType.Ronin => 150,
            ClassType.Ravager => 150,
            ClassType.Gunslinger => 200,
            ClassType.Brawler => 200,
            ClassType.Summoner => 200,
            ClassType.Hexer => 250,
            _ => 0,
        };

        private static string UnlockConditionText(ClassType cls) => cls switch
        {
            ClassType.Ravager => "Act2 boss'u Warblade ile",
            ClassType.Ronin => "Act2 boss'u Shadowblade ile",
            ClassType.Gunslinger => "Ranger ile Act2'ye ulaş",
            ClassType.Brawler => "Ravager ile Act2'ye ulaş",
            ClassType.Summoner => "art arda 3 run Act2",
            ClassType.Hexer => "Elementalist ile run bitir",
            _ => "",
        };

        private static string UnlockOrPathText(ClassType cls)
        {
            int cost = UnlockCost(cls);
            return cost <= 0 ? "" : $"{cost} SHATTERED ECHO · veya {UnlockConditionText(cls)}";
        }

        private static bool HasHexerPrerequisite() =>
            PlayerPrefs.GetInt(HexerElementalistRunPrefsKey, 0) == 1;

        private static bool CanUnlock(ClassType cls)
        {
            if (IsUnlocked(cls)) return false;
            return EchoWallet.Balance >= UnlockCost(cls);
        }

        private void TryUnlockSelectedClass()
        {
            if (!CanUnlock(selectedClass)) return;

            int cost = UnlockCost(selectedClass);
            if (!EchoWallet.TrySpend(cost)) return;

            PlayerPrefs.SetInt(UnlockPrefKey(selectedClass), 1);
            PlayerPrefs.Save();

            RefreshEchoLabel();
            SelectClass(selectedClass);
        }

        private void RefreshEchoLabel()
        {
            if (echoBalanceLabel != null)
                echoBalanceLabel.text = $"<color=#E89020>{EchoWallet.Balance}</color> <color=#B0B3BC>SHATTERED ECHO</color>";
        }

        private static void ApplyRoomEntryVisual(RoomEntry entry, bool selected)
        {
            bool unlocked = IsUnlocked(entry.classType);
            float scale = entry.baseScale;

            if (entry.root != null)
            {
                entry.root.localScale = new Vector3(scale, scale, 1f);
                entry.root.anchoredPosition = entry.basePosition;
            }

            if (entry.canvasGroup != null)
                entry.canvasGroup.alpha = 1f;

            if (entry.sprite != null)
            {
                float spriteAlpha = entry.sprite.color.a;
                entry.sprite.color = unlocked
                    ? (selected ? Color.white : new Color(0.60f, 0.60f, 0.60f, spriteAlpha))
                    : WithAlpha(LockedSilhouetteColor, spriteAlpha);
            }

            if (entry.seal != null)
            {
                entry.seal.enabled = selected;
                entry.seal.color = Color.white;
            }

            if (entry.glow != null)
            {
                entry.glow.enabled = false;
                entry.glow.color = Color.clear;
            }

            if (entry.selectionVfxRoot != null)
                entry.selectionVfxRoot.gameObject.SetActive(selected);

            if (entry.selectionGlowDisk != null)
                entry.selectionGlowDisk.color = Color.clear;

            if (entry.selectionRing != null)
                entry.selectionRing.color = WithAlpha(RimaUITheme.CharSelectCyan, selected ? 0.46f : 0.0f);

            if (entry.selectionMotes != null)
            {
                foreach (var mote in entry.selectionMotes)
                {
                    if (mote != null)
                        mote.color = Color.clear;
                }
            }

            if (entry.lockChip != null)
            {
                entry.lockChip.gameObject.SetActive(!unlocked);
                entry.lockChip.color = WithAlpha(RimaUITheme.CharSelectButtonFill, selected ? 0.70f : 0.52f);
            }

            if (entry.costChip != null)
            {
                entry.costChip.transform.parent.gameObject.SetActive(!unlocked);
                entry.costChip.text = UnlockOrPathText(entry.classType);
                entry.costChip.color = selected ? RimaUITheme.CharSelectOrange : RimaUITheme.CharSelectTextBody;
            }

            if (entry.hit != null)
                entry.hit.color = Color.clear;

            if (entry.button != null)
                entry.button.interactable = true;
        }

        private static Sprite LoadCanonicalSprite(ClassType cls)
        {
            // Load the imported sprite directly so Editor and build share the same native pivot and PPU64
            // (avoids the Sprite.Create pivot mismatch that shifted characters vertically in builds).
            return Resources.Load<Sprite>(RimaUITheme.AnchorPath(cls));
        }

        // ─── Helpers ──────────────────────────────────────────────────────

        private void AnimateRoomSelection()
        {
            if (selectionRingFadeTimer > 0f)
                selectionRingFadeTimer = Mathf.Max(0f, selectionRingFadeTimer - Time.unscaledDeltaTime);

            if (roomEntries.TryGetValue(selectedClass, out var selected))
            {
                float fade = selectedClass == selectionFadeClass
                    ? 1f - Mathf.Clamp01(selectionRingFadeTimer / SelectionRingFadeDuration)
                    : 1f;

                if (selected.root != null)
                    selected.root.anchoredPosition = selected.basePosition;

                if (selected.seal != null)
                    selected.seal.rectTransform.localScale = Vector3.one;

                if (selected.selectionGlowDisk != null)
                {
                    selected.selectionGlowDisk.rectTransform.localScale = Vector3.one;
                    selected.selectionGlowDisk.color = Color.clear;
                }

                if (selected.selectionRing != null)
                {
                    selected.selectionRing.rectTransform.localEulerAngles = Vector3.zero;
                    selected.selectionRing.color = WithAlpha(RimaUITheme.CharSelectCyan, 0.46f * fade);
                }

                if (selected.selectionMotes != null)
                {
                    foreach (var mote in selected.selectionMotes)
                    {
                        if (mote == null)
                            continue;

                        mote.rectTransform.localScale = Vector3.one;
                        mote.color = Color.clear;
                    }
                }
            }
        }

        private static SkillDatabase EnsureSkillDatabase()
        {
            if (SkillDatabase.Instance != null)
            {
                SkillDatabase.Instance.EnsureBuilt();
                return SkillDatabase.Instance;
            }

            var existing = FindAnyObjectByType<SkillDatabase>();
            if (existing != null)
            {
                existing.EnsureBuilt();
                return existing;
            }

            var go = new GameObject("SkillDatabase_Auto");
            DontDestroyOnLoad(go);
            var db = go.AddComponent<SkillDatabase>();
            db.EnsureBuilt();
            return db;
        }

        private static List<SkillPresentation> SkillPresentationFor(ClassType cls)
        {
            var fallback = FallbackSkills(cls);
            var database = EnsureSkillDatabase();
            var dbSkills = database != null
                ? database.GetAll()
                    .Where(s => s != null && s.classType == cls)
                    .ToDictionary(s => s.skillName, StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, SkillData>(StringComparer.OrdinalIgnoreCase);

            var result = new List<SkillPresentation>(fallback.Length + dbSkills.Count);
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var item in fallback)
            {
                if (!string.IsNullOrEmpty(item.name))
                    seen.Add(item.name);

                if (!item.locked && dbSkills.TryGetValue(item.name, out var db))
                {
                    result.Add(new SkillPresentation(
                        db.skillName,
                        string.IsNullOrWhiteSpace(db.description) ? item.description : db.description,
                        IconText(db.skillName),
                        false,
                        "",
                        db.icon != null ? db.icon : RimaUITheme.PassiveIcon(db.skillName)));
                    continue;
                }

                result.Add(item);
            }

            foreach (var db in dbSkills.Values)
            {
                if (seen.Contains(db.skillName)) continue;
                result.Add(new SkillPresentation(
                    db.skillName,
                    db.description,
                    IconText(db.skillName),
                    false,
                    "",
                    db.icon != null ? db.icon : RimaUITheme.PassiveIcon(db.skillName)));
            }

            return result;
        }

        private static SkillPresentation OpenSkill(string name, string description) =>
            new(name, description, IconText(name), false, "");

        private static SkillPresentation LockedSkill(string name, string condition) =>
            new(name, "", IconText(name), true, condition);

        private static string IconText(string name) =>
            string.IsNullOrEmpty(name) ? "*" : name.Substring(0, 1).ToUpperInvariant();

        private static SkillPresentation[] FallbackSkills(ClassType cls) => cls switch
        {
            ClassType.Warblade => new[]
            {
                OpenSkill("Iron Charge", "Bakış yönüne fırla, isabette Rage kazan."),
                OpenSkill("Cleave", "Çevresel döner darbe, Rage ile güçlenir."),
                OpenSkill("Deep Wound", "Yakın hedefe ani hasar ve kanama uygular."),
                OpenSkill("Sunder Mark", "En yakın düşmanı işaretle, zırhını kır."),
                OpenSkill("Crippling Blow", "Yakındaki düşmanı yavaşlat ve sersemlet."),
                LockedSkill("Earthsplitter", "Açılış: Act 1'i Warblade ile bitir"),
                LockedSkill("Blade Rush", "Açılış: 3 odada hasar almadan ilerle"),
                LockedSkill("Gravity Cleave", "Açılış: 40 düşmanı Sundered halde yen"),
                LockedSkill("Iron Counter", "Açılış: 20 saldırıyı parry ile boz"),
                LockedSkill("Iron Crush", "Açılış: Act 2 elitini Warblade ile yen"),
                LockedSkill("Ironclade Momentum", "Açılış: 60 sn combo kaybetme"),
                OpenSkill("Blood Drinker", "Her öldürme sonrası can yeniler."),
                LockedSkill("Wrath Protocol", "Açılış: HP %50 altında oda temizle"),
                LockedSkill("Tempered Fury", "Açılış: Rage'i 80+ tutarak boss yen"),
                OpenSkill("Berserker's Resolve", "Knockback aldığında Rage üretir."),
                LockedSkill("Battle Surge", "Açılış: Act 2 bossunu Warblade ile yen"),
                LockedSkill("Death Blow", "Açılış: 25 execute zinciri tamamla"),
            },
            ClassType.Elementalist => new[]
            {
                OpenSkill("Fireball", "Hedef noktada patlama ve kısa yanma uygular."),
                OpenSkill("Glacial Spike", "Buz hattı yavaşlatır ve Frost state kurar."),
                OpenSkill("Living Bomb", "Fitil patlayınca alana hasar verir."),
                OpenSkill("Chain Lightning", "İlk düşmandan hedeflere atlayan yıldırım."),
                OpenSkill("Blink", "İmleç konumuna anında ışınlan."),
                LockedSkill("Arcane Blast", "Açılış: 3 farklı elementi aynı odada kullan"),
                LockedSkill("Frozen Orb", "Açılış: 40 düşmanı Chill altında yen"),
                LockedSkill("Prism Beam", "Açılış: Act 1'i Elementalist ile bitir"),
                LockedSkill("Meteor", "Açılış: Frozen hedefe boss hasarı ver"),
                LockedSkill("Frost Wall", "Açılış: 25 projectile engelle"),
                LockedSkill("Solar Flare", "Açılış: Light stack ile 50 düşman yen"),
                LockedSkill("Blizzard", "Açılış: Act 2'ye Elementalist ile ulaş"),
            },
            ClassType.Ranger => new[]
            {
                OpenSkill("Aimed Shot", "Nişanlı yüksek hasar, dolu Focus'ta kritik."),
                OpenSkill("Pinning Shot", "Root ve mark uygulayan rift oku."),
                OpenSkill("Marked Detonate", "Marked hedefleri patlatır."),
                OpenSkill("Hunter's Step", "Kısa reposition dash ve crit penceresi."),
                OpenSkill("Bone Trap", "Öne trap zone kurar ve hedefleri rootlar."),
                OpenSkill("Disengage", "Geri atla, yavaşlatma bırak."),
                LockedSkill("Multi Shot", "Açılış: 5 hedefi tek atışta vur"),
                LockedSkill("Sweep Volley", "Açılış: Act 1'i Ranger ile bitir"),
                LockedSkill("Predator's Mark", "Açılış: 60 mark tetikle"),
                LockedSkill("Final Strike", "Açılış: Trapped boss execute et"),
                LockedSkill("Wireline Trap", "Açılış: Act 2'ye Ranger ile ulaş"),
            },
            ClassType.Shadowblade => new[]
            {
                OpenSkill("Scarbinding", "Düşman içinden phase geçişiyle kalıcı Rift Scar bırakır."),
                OpenSkill("Phase Step", "Çizgide scar bırakan phase dash."),
                OpenSkill("Backstab Mark", "Mark ve scar kurar."),
                OpenSkill("Death Mark", "Gecikmeli patlama mark'ı koyar."),
                OpenSkill("Shadow Clone", "Kısa süreli decoy phantom."),
                OpenSkill("Shadow Pin", "Dagger projectile ile root ve scar."),
                LockedSkill("Mirror Cut", "Açılış: Act 1'i Shadowblade ile bitir"),
                LockedSkill("Veil Burst", "Açılış: 4 hedefe phase zinciri"),
                LockedSkill("Severance", "Açılış: 80 scar collapse et"),
                LockedSkill("Smoke Veil", "Açılış: hasar almadan 3 oda"),
                LockedSkill("Chain Cull", "Açılış: Act 2 bossunu Shadowblade ile yen"),
                LockedSkill("Night Aperture", "Açılış: 6 scar'i aynı anda taşıt"),
            },
            ClassType.Ronin => PlaceholderSkills(
                "Quickdraw", "Iaido Stance", "Sakura Veil", "Final Draw", "Moon Cut", "Still Water", "Petal Riposte", "Last Sheath"),
            ClassType.Ravager => PlaceholderSkills(
                "Rend Hook", "Bone Maw", "Armor Split", "Red Wake", "Hook Slam", "Gore Path", "Fury Sink", "Last Roar"),
            ClassType.Gunslinger => PlaceholderSkills(
                "Deadeye", "Quick Reload", "Ricochet", "Smoke Round", "Fan Hammer", "Silver Mark", "Pierce Shot", "Last Bullet"),
            ClassType.Brawler => PlaceholderSkills(
                "Jawbreaker", "Shoulder In", "Ground Lock", "Counter Jab", "Iron Guard", "Ring Step", "Uppercut", "No Mercy"),
            ClassType.Summoner => PlaceholderSkills(
                "Wisp Call", "Bone Pact", "Rift Totem", "Twin Shade", "Offering", "Grave Door", "Rift Swarm", "Last Familiar"),
            ClassType.Hexer => PlaceholderSkills(
                "Wither", "Hex Brand", "Black Thread", "Mire Curse", "Glass Bone", "Dread Bloom", "Witch Knot", "Last Omen"),
            _ => Array.Empty<SkillPresentation>(),
        };

        private static SkillPresentation[] PlaceholderSkills(params string[] names)
        {
            var descriptions = new[] { "Başlangıç aktif yetenek.", "Ritim kuran sınıf aracı.", "Kontrol penceresi açar." };
            var result = new SkillPresentation[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                result[i] = i < 3
                    ? OpenSkill(names[i], descriptions[i])
                    : LockedSkill(names[i], "Açılış: sınıf deed'i tamamla");
            }
            return result;
        }

        private static string OneLine(string value) =>
            string.IsNullOrWhiteSpace(value) ? "" : value.Replace('\n', ' ').Replace('\r', ' ');

        private static void EnsureTooltipSystem()
        {
            if (TooltipSystem.Instance != null) return;

            var go = new GameObject("TooltipSystem_Runtime");
            go.AddComponent<TooltipSystem>();
        }

        private static void AddSkillTooltip(GameObject target, string content)
        {
            AddEventTrigger(target, EventTriggerType.PointerEnter, _ =>
            {
                EnsureTooltipSystem();
                TooltipSystem.Instance?.Show(content);
            });
            AddEventTrigger(target, EventTriggerType.PointerExit, _ => TooltipSystem.Instance?.Hide());
        }

        private static RectTransform MakeSkillStripArea(RectTransform parent, string name)
        {
            var root = MakePanel(name, parent);
            SetStretch(root, new Vector2(0f, 0f), new Vector2(1f, 0.92f), new Vector2(10f, 8f), new Vector2(-10f, -4f));
            var rootImg = root.GetComponent<Image>();
            rootImg.color = new Color(0f, 0f, 0f, 0f);
            rootImg.raycastTarget = false;

            root.gameObject.AddComponent<RectMask2D>();

            var content = new GameObject("Content", typeof(RectTransform), typeof(VerticalLayoutGroup));
            content.transform.SetParent(root, false);
            var contentRt = content.GetComponent<RectTransform>();
            contentRt.anchorMin = Vector2.zero;
            contentRt.anchorMax = Vector2.one;
            contentRt.pivot = new Vector2(0.5f, 1f);
            contentRt.offsetMin = Vector2.zero;
            contentRt.offsetMax = Vector2.zero;

            var layout = content.GetComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(0, 0, 0, 0);
            layout.spacing = 3f;
            layout.childAlignment = TextAnchor.UpperCenter;
            layout.childControlWidth = true;
            layout.childControlHeight = false;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            return contentRt;
        }

        private static Color WithAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        private static void AddEventTrigger(GameObject target, EventTriggerType eventId, Action<BaseEventData> callback)
        {
            var trigger = target.GetComponent<EventTrigger>() ?? target.AddComponent<EventTrigger>();
            var entry = new EventTrigger.Entry { eventID = eventId };
            entry.callback.AddListener(data => callback(data));
            trigger.triggers.Add(entry);
        }

        private static RectTransform MakeFramedPanel(string name, RectTransform parent, Vector2 anchorMin, Vector2 anchorMax)
        {
            var rt = MakePanel(name, parent);
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;

            var img = rt.GetComponent<Image>();
            img.sprite = Resources.Load<Sprite>(PackPanelFrame) ?? RimaUITheme.SmallPanelFrame;
            img.type = Image.Type.Sliced;
            img.color = RimaUITheme.CharSelectIronGrey;
            img.raycastTarget = false;

            var fill = MakePanel("PanelFill", rt);
            SetStretch(fill, Vector2.zero, Vector2.one, new Vector2(1f, 1f), new Vector2(-1f, -1f));
            var fillImg = fill.GetComponent<Image>();
            fillImg.sprite = img.sprite;
            fillImg.type = Image.Type.Sliced;
            fillImg.color = RimaUITheme.CharSelectPanelFill;
            fillImg.raycastTarget = false;

            var innerHighlight = MakePanel("InnerHighlight", rt);
            SetStretch(innerHighlight, Vector2.zero, Vector2.one, new Vector2(2f, 2f), new Vector2(-2f, -2f));
            var highlightImg = innerHighlight.GetComponent<Image>();
            highlightImg.sprite = img.sprite;
            highlightImg.type = Image.Type.Sliced;
            highlightImg.color = WithAlpha(RimaUITheme.CharSelectParchment, 0.08f);
            highlightImg.raycastTarget = false;

            fill.SetAsFirstSibling();
            innerHighlight.SetSiblingIndex(1);
            return rt;
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

        private static RectTransform MakeRect(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.offsetMin = rt.offsetMax = Vector2.zero;
            return rt;
        }

        private static TMP_Text MakeText(string text, RectTransform parent, float size,
            FontStyles style, Color color)
        {
            int maxLen = Mathf.Min((text ?? "").Length, 14);
            var go = new GameObject("Lbl_" + (text ?? "").Substring(0, maxLen), typeof(RectTransform));
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
