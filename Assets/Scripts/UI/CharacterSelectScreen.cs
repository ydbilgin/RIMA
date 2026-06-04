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
        private const string PackPedestal   = "UI/RIMA/Pack/pedestal_seal";      // 512 circular seal platform
        private const string PackCardFrame  = "UI/RIMA/Pack/card_frame_9slice";  // 256x384 portrait card frame (border 28)
        private const string PackButton     = "UI/RIMA/Pack/button_9slice";      // 192x64 filled button bg (border 16)
        private const string PackPanelFrame = "UI/RIMA/Pack/panel_frame_9slice";
        private const string RoomBackdrop   = "UI/RIMA/CharacterSelect/room_bg";
        private const int RoomBackdropRetryFrames = 10;

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
        private Button    startButton;
        private TMP_Text  startButtonLabel;
        private TMP_Text  skillEmptyLabel;
        private RectTransform skillContent;

        private Image showcaseFlashImage;
        private float selectFlashTimer;

        private static readonly ClassType[] AllClasses =
        {
            ClassType.Warblade, ClassType.Elementalist, ClassType.Shadowblade,
            ClassType.Ranger, ClassType.Ravager, ClassType.Ronin,
            ClassType.Gunslinger, ClassType.Brawler, ClassType.Summoner, ClassType.Hexer
        };

        private static readonly RoomPlacement[] RosterPlacements =
        {
            new RoomPlacement(ClassType.Ronin,        new Vector2(0.270f, 0.62f), new Vector2(200f, 285f), 0.74f),
            new RoomPlacement(ClassType.Ravager,      new Vector2(0.375f, 0.62f), new Vector2(200f, 285f), 0.74f),
            new RoomPlacement(ClassType.Gunslinger,   new Vector2(0.480f, 0.62f), new Vector2(200f, 285f), 0.74f),
            new RoomPlacement(ClassType.Brawler,      new Vector2(0.585f, 0.62f), new Vector2(200f, 285f), 0.74f),
            new RoomPlacement(ClassType.Summoner,     new Vector2(0.690f, 0.62f), new Vector2(200f, 285f), 0.74f),
            new RoomPlacement(ClassType.Hexer,        new Vector2(0.790f, 0.62f), new Vector2(200f, 285f), 0.74f),
            new RoomPlacement(ClassType.Warblade,     new Vector2(0.340f, 0.34f), new Vector2(250f, 350f), 0.92f),
            new RoomPlacement(ClassType.Elementalist, new Vector2(0.470f, 0.34f), new Vector2(250f, 350f), 0.92f),
            new RoomPlacement(ClassType.Ranger,       new Vector2(0.610f, 0.34f), new Vector2(250f, 350f), 0.92f),
            new RoomPlacement(ClassType.Shadowblade,  new Vector2(0.730f, 0.34f), new Vector2(250f, 350f), 0.92f),
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
            public Image         lockGlyph;
            public TMP_Text      costChip;
            public CanvasGroup   canvasGroup;
            public ClassType     classType;
            public Vector2       basePosition;
            public float         baseScale;
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

            var identityPanel = MakeFramedPanel("IdentityPopupPanel", root, new Vector2(0.012f, 0.34f), new Vector2(0.175f, 0.84f));
            identityPanel.offsetMin = new Vector2(10f, 8f);
            identityPanel.offsetMax = new Vector2(-8f, -8f);
            BuildIdentityPanel(identityPanel);

            var skillPanel = MakeFramedPanel("SkillZone", root, new Vector2(0.86f, 0.14f), new Vector2(0.988f, 0.96f));
            skillPanel.offsetMin = new Vector2(6f, 6f);
            skillPanel.offsetMax = new Vector2(-8f, -8f);
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
            Image backdropImage = RimaUITheme.CreateFullScreenBackdrop(parent, RoomBackdrop, RimaUITheme.BackgroundDark);
            NameRoomBackdropSprite(backdropImage);
            if (backdropImage != null && backdropImage.sprite == null)
                StartCoroutine(RetryLoadRoomBackdrop(backdropImage));

            foreach (var placement in RosterPlacements.OrderByDescending(p => p.anchor.y))
                BuildRoomCharacter(parent, placement);

            showcaseFlashImage = MakePanel("SelectionFlash", parent).GetComponent<Image>();
            var flashRt = showcaseFlashImage.transform as RectTransform;
            flashRt.anchorMin = Vector2.zero; flashRt.anchorMax = Vector2.one;
            flashRt.offsetMin = flashRt.offsetMax = Vector2.zero;
            showcaseFlashImage.color = Color.clear;
            showcaseFlashImage.raycastTarget = false;
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
            var root = MakeRect($"RoomCharacter_{cls}", parent, placement.anchor, placement.anchor);
            root.pivot = new Vector2(0.5f, 0f);
            root.anchoredPosition = Vector2.zero;
            root.sizeDelta = placement.size;
            root.localScale = new Vector3(placement.baseScale, placement.baseScale, 1f);

            var canvasGroup = root.gameObject.AddComponent<CanvasGroup>();

            var glowRt = MakePanel("HoverGlow", root);
            glowRt.anchorMin = new Vector2(0.5f, 0f); glowRt.anchorMax = new Vector2(0.5f, 0f);
            glowRt.pivot = new Vector2(0.5f, 0.5f);
            glowRt.anchoredPosition = new Vector2(0f, 34f);
            glowRt.sizeDelta = new Vector2(250f, 92f);
            var glow = glowRt.GetComponent<Image>();
            glow.sprite = Resources.Load<Sprite>(PackPedestal) ?? RimaUITheme.SmallPanelFrame;
            glow.type = Image.Type.Simple;
            glow.preserveAspect = true;
            glow.raycastTarget = false;

            var sealRt = MakePanel("PedestalSeal", root);
            sealRt.anchorMin = new Vector2(0.5f, 0f); sealRt.anchorMax = new Vector2(0.5f, 0f);
            sealRt.pivot = new Vector2(0.5f, 0.5f);
            sealRt.anchoredPosition = new Vector2(0f, 28f);
            sealRt.sizeDelta = new Vector2(210f, 74f);
            var seal = sealRt.GetComponent<Image>();
            seal.sprite = Resources.Load<Sprite>(PackPedestal) ?? RimaUITheme.SmallPanelFrame;
            seal.type = Image.Type.Simple;
            seal.preserveAspect = true;
            seal.raycastTarget = false;

            var selectionVfxRoot = BuildSelectionVfx(root, placement.size);
            var selectionVfxImages = selectionVfxRoot.GetComponentsInChildren<Image>(true);
            var selectionGlowDisk = selectionVfxRoot.Find("PulseGlowDisk").GetComponent<Image>();
            var selectionRing = selectionVfxRoot.Find("RotatingCyanRing").GetComponent<Image>();
            var selectionMotes = selectionVfxImages
                .Where(img => img != null && img.name.StartsWith("GlowMote", StringComparison.Ordinal))
                .ToArray();

            var spriteRt = MakePanel("Sprite", root);
            spriteRt.anchorMin = new Vector2(0.5f, 0f); spriteRt.anchorMax = new Vector2(0.5f, 0f);
            spriteRt.pivot = new Vector2(0.5f, 0f);
            spriteRt.anchoredPosition = Vector2.zero;
            spriteRt.sizeDelta = placement.size;
            var sprite = spriteRt.GetComponent<Image>();
            sprite.sprite = LoadCanonicalSprite(cls);
            sprite.preserveAspect = true;
            sprite.raycastTarget = false;

            var lockRt = MakePanel("LockGlyph", root);
            lockRt.anchorMin = new Vector2(0.5f, 1f); lockRt.anchorMax = new Vector2(0.5f, 1f);
            lockRt.pivot = new Vector2(0.5f, 0.5f);
            lockRt.anchoredPosition = new Vector2(0f, -28f);
            lockRt.sizeDelta = new Vector2(58f, 34f);
            var lockImg = lockRt.GetComponent<Image>();
            lockImg.sprite = RimaUITheme.SmallPanelFrame;
            lockImg.type = Image.Type.Sliced;
            lockImg.raycastTarget = false;

            var lockLabel = MakeText("LOCK", lockRt, 10f, FontStyles.Bold, RimaUITheme.Cyan);
            lockLabel.alignment = TextAlignmentOptions.Center;
            var lockLabelRt = lockLabel.transform as RectTransform;
            lockLabelRt.anchorMin = Vector2.zero; lockLabelRt.anchorMax = Vector2.one;
            lockLabelRt.offsetMin = lockLabelRt.offsetMax = Vector2.zero;

            var chipRt = MakePanel("CostChip", root);
            chipRt.anchorMin = new Vector2(0.5f, 0f); chipRt.anchorMax = new Vector2(0.5f, 0f);
            chipRt.pivot = new Vector2(0.5f, 0.5f);
            chipRt.anchoredPosition = new Vector2(0f, -18f);
            chipRt.sizeDelta = new Vector2(150f, 30f);
            var chipImg = chipRt.GetComponent<Image>();
            chipImg.sprite = RimaUITheme.SmallPanelFrame;
            chipImg.type = Image.Type.Sliced;
            chipImg.color = new Color(0.03f, 0.02f, 0.05f, 0.90f);
            chipImg.raycastTarget = false;

            var costChip = MakeText($"{UnlockCost(cls)} Echo", chipRt, 11f, FontStyles.Bold, RimaUITheme.Cyan);
            costChip.alignment = TextAlignmentOptions.Center;
            var costRt = costChip.transform as RectTransform;
            costRt.anchorMin = Vector2.zero; costRt.anchorMax = Vector2.one;
            costRt.offsetMin = costRt.offsetMax = Vector2.zero;

            var hitRt = MakePanel("Hit", root);
            hitRt.anchorMin = new Vector2(0.5f, 0f);
            hitRt.anchorMax = new Vector2(0.5f, 0f);
            hitRt.pivot = new Vector2(0.5f, 0.5f);
            hitRt.anchoredPosition = new Vector2(0f, placement.size.y * 0.38f);
            hitRt.sizeDelta = new Vector2(placement.size.x * 0.45f, placement.size.y * 0.70f);
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
                lockGlyph = lockImg,
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
                    entry.glow.color = WithAlpha(RimaUITheme.ClassAccent(captured), 0.22f);
                }
            });
            AddEventTrigger(hitRt.gameObject, EventTriggerType.PointerExit, _ =>
            {
                ApplyRoomEntryVisual(entry, selectedClass == captured);
            });

            roomEntries[cls] = entry;
            ApplyRoomEntryVisual(entry, false);
        }

        private static RectTransform BuildSelectionVfx(RectTransform parent, Vector2 characterSize)
        {
            var root = MakeRect("SelectionUIGlowVFX", parent, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f));
            root.pivot = new Vector2(0.5f, 0.5f);
            root.anchoredPosition = new Vector2(0f, 32f);
            root.sizeDelta = new Vector2(characterSize.x * 0.92f, 96f);

            var glowDisk = MakePanel("PulseGlowDisk", root);
            glowDisk.anchorMin = new Vector2(0.5f, 0.5f); glowDisk.anchorMax = new Vector2(0.5f, 0.5f);
            glowDisk.pivot = new Vector2(0.5f, 0.5f);
            glowDisk.anchoredPosition = Vector2.zero;
            glowDisk.sizeDelta = new Vector2(characterSize.x * 0.90f, 76f);
            var glowDiskImg = glowDisk.GetComponent<Image>();
            glowDiskImg.sprite = Resources.Load<Sprite>(PackPedestal) ?? RimaUITheme.SmallPanelFrame;
            glowDiskImg.type = Image.Type.Simple;
            glowDiskImg.preserveAspect = true;
            glowDiskImg.color = new Color(0f, 1f, 0.80f, 0.0f);
            glowDiskImg.raycastTarget = false;

            var ring = MakePanel("RotatingCyanRing", root);
            ring.anchorMin = new Vector2(0.5f, 0.5f); ring.anchorMax = new Vector2(0.5f, 0.5f);
            ring.pivot = new Vector2(0.5f, 0.5f);
            ring.anchoredPosition = Vector2.zero;
            ring.sizeDelta = new Vector2(characterSize.x * 0.74f, 64f);
            var ringImg = ring.GetComponent<Image>();
            ringImg.sprite = Resources.Load<Sprite>(PackPedestal) ?? RimaUITheme.SmallPanelFrame;
            ringImg.type = Image.Type.Simple;
            ringImg.preserveAspect = true;
            ringImg.color = new Color(0f, 1f, 0.80f, 0.0f);
            ringImg.raycastTarget = false;

            for (int i = 0; i < 3; i++)
            {
                var mote = MakePanel("GlowMote" + i, root);
                mote.anchorMin = new Vector2(0.5f, 0.5f); mote.anchorMax = new Vector2(0.5f, 0.5f);
                mote.pivot = new Vector2(0.5f, 0.5f);
                mote.sizeDelta = new Vector2(12f, 12f);
                var moteImg = mote.GetComponent<Image>();
                moteImg.sprite = RimaUITheme.SmallPanelFrame;
                moteImg.type = Image.Type.Sliced;
                moteImg.color = new Color(0f, 1f, 0.80f, 0.0f);
                moteImg.raycastTarget = false;
            }

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
            portraitFrame.anchoredPosition = new Vector2(0f, -28f);
            portraitFrame.sizeDelta = new Vector2(118f, 118f);
            var portraitFrameImg = portraitFrame.GetComponent<Image>();
            portraitFrameImg.sprite = RimaUITheme.SmallPanelFrame;
            portraitFrameImg.type = Image.Type.Sliced;
            portraitFrameImg.color = new Color(0.02f, 0.01f, 0.04f, 0.78f);
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

            classNameLabel = MakeText("WARBLADE", parent, 25, FontStyles.Bold, Color.white);
            classNameLabel.alignment = TextAlignmentOptions.Center;
            var cnRt = classNameLabel.transform as RectTransform;
            cnRt.anchorMin = new Vector2(0.05f, 0.66f); cnRt.anchorMax = new Vector2(0.95f, 0.75f);
            cnRt.offsetMin = cnRt.offsetMax = Vector2.zero;

            tagline1Label = MakeText("HEAVY · MELEE · RAGE", parent, 11, FontStyles.Bold, RimaUITheme.TextMuted);
            tagline1Label.alignment = TextAlignmentOptions.Center;
            var t1Rt = tagline1Label.transform as RectTransform;
            t1Rt.anchorMin = new Vector2(0.05f, 0.59f); t1Rt.anchorMax = new Vector2(0.95f, 0.66f);
            t1Rt.offsetMin = t1Rt.offsetMax = Vector2.zero;

            tagline2Label = MakeText("", parent, 11, FontStyles.Normal, RimaUITheme.TextMuted);
            tagline2Label.alignment = TextAlignmentOptions.Center;
            tagline2Label.enableWordWrapping = true;
            var t2Rt = tagline2Label.transform as RectTransform;
            t2Rt.anchorMin = new Vector2(0.08f, 0.49f); t2Rt.anchorMax = new Vector2(0.92f, 0.59f);
            t2Rt.offsetMin = t2Rt.offsetMax = Vector2.zero;

            identityMottoLabel = MakeText("", parent, 13, FontStyles.Bold, RimaUITheme.Cyan);
            identityMottoLabel.alignment = TextAlignmentOptions.Left;
            identityMottoLabel.enableWordWrapping = true;
            var mottoRt = identityMottoLabel.transform as RectTransform;
            mottoRt.anchorMin = new Vector2(0.08f, 0.35f); mottoRt.anchorMax = new Vector2(0.92f, 0.48f);
            mottoRt.offsetMin = mottoRt.offsetMax = Vector2.zero;

            identityPlaystyleLabel = MakeText("", parent, 10, FontStyles.Normal, RimaUITheme.TextMuted);
            identityPlaystyleLabel.alignment = TextAlignmentOptions.TopLeft;
            identityPlaystyleLabel.enableWordWrapping = true;
            var playstyleRt = identityPlaystyleLabel.transform as RectTransform;
            playstyleRt.anchorMin = new Vector2(0.08f, 0.20f); playstyleRt.anchorMax = new Vector2(0.92f, 0.34f);
            playstyleRt.offsetMin = playstyleRt.offsetMax = Vector2.zero;

            identityResourceLabel = MakeText("", parent, 10, FontStyles.Bold, RimaUITheme.TextPrimary);
            identityResourceLabel.alignment = TextAlignmentOptions.TopLeft;
            identityResourceLabel.enableWordWrapping = true;
            var resourceRt = identityResourceLabel.transform as RectTransform;
            resourceRt.anchorMin = new Vector2(0.08f, 0.09f); resourceRt.anchorMax = new Vector2(0.92f, 0.19f);
            resourceRt.offsetMin = resourceRt.offsetMax = Vector2.zero;

            identityLockLabel = MakeText("", parent, 10, FontStyles.Bold, RimaUITheme.TextMuted);
            identityLockLabel.alignment = TextAlignmentOptions.Left;
            identityLockLabel.enableWordWrapping = true;
            var lockRt = identityLockLabel.transform as RectTransform;
            lockRt.anchorMin = new Vector2(0.08f, 0.02f); lockRt.anchorMax = new Vector2(0.92f, 0.08f);
            lockRt.offsetMin = lockRt.offsetMax = Vector2.zero;
        }

        private void BuildSkillPanel(RectTransform parent)
        {
            var skillsHeader = MakeText("SKILLS", parent, 13, FontStyles.Bold, RimaUITheme.Cyan);
            skillsHeader.alignment = TextAlignmentOptions.Left;
            var shRt = skillsHeader.transform as RectTransform;
            shRt.anchorMin = new Vector2(0f, 0.93f); shRt.anchorMax = new Vector2(1f, 1f);
            shRt.offsetMin = new Vector2(14f, 0f); shRt.offsetMax = new Vector2(-8f, 0f);

            skillContent = MakeSkillStripArea(parent, "SkillStripArea");

            skillEmptyLabel = MakeText("Yetenekler yakinda", parent, 13, FontStyles.Normal, RimaUITheme.TextMuted);
            skillEmptyLabel.alignment = TextAlignmentOptions.Center;
            var emptyRt = skillEmptyLabel.transform as RectTransform;
            emptyRt.anchorMin = new Vector2(0f, 0f); emptyRt.anchorMax = new Vector2(1f, 0.92f);
            emptyRt.offsetMin = new Vector2(10f, 0f); emptyRt.offsetMax = new Vector2(-10f, 0f);
        }

        private void BuildActionStrip(RectTransform parent)
        {
            var bg = parent.GetComponent<Image>();
            bg.sprite = RimaUITheme.SmallPanelFrame;
            bg.type = Image.Type.Sliced;
            bg.color = new Color(0.05f, 0.03f, 0.07f, 0.86f);
            bg.raycastTarget = false;

            var topBorder = MakePanel("CyanTopBorder", parent);
            SetStretch(topBorder, new Vector2(0f, 1f), Vector2.one, new Vector2(0f, -2f), Vector2.zero);
            var topBorderImg = topBorder.GetComponent<Image>();
            topBorderImg.color = RimaUITheme.Cyan;
            topBorderImg.raycastTarget = false;

        }

        private void BuildStartButton(RectTransform parent)
        {
            var btnRoot = MakePanel("StartButton", parent);
            btnRoot.anchorMin = new Vector2(0.50f, 0.06f);
            btnRoot.anchorMax = new Vector2(0.50f, 0.06f);
            btnRoot.pivot = new Vector2(0.5f, 0.5f);
            btnRoot.anchoredPosition = Vector2.zero;
            btnRoot.sizeDelta = new Vector2(360f, 52f);

            var bgImg = btnRoot.GetComponent<Image>();
            // On-brand 9-slice button background (filled center). Falls back to the
            // procedural ResourceFrame if the Pack sprite is missing.
            var buttonSprite = Resources.Load<Sprite>(PackButton);
            bgImg.sprite = buttonSprite != null ? buttonSprite : RimaUITheme.ResourceFrame;
            bgImg.type = Image.Type.Sliced;
            bgImg.color = RimaUITheme.ClassAccent(ClassType.Warblade);
            bgImg.raycastTarget = true;

            startButtonLabel = MakeText("SEÇ", btnRoot, 21, FontStyles.Bold, Color.white);
            startButtonLabel.alignment = TextAlignmentOptions.Center;
            startButtonLabel.enableWordWrapping = true;
            startButtonLabel.overflowMode = TextOverflowModes.Ellipsis;
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
            btnRoot.anchorMin = new Vector2(0.28f, 0.06f);
            btnRoot.anchorMax = new Vector2(0.28f, 0.06f);
            btnRoot.pivot = new Vector2(0.5f, 0.5f);
            btnRoot.anchoredPosition = Vector2.zero;
            btnRoot.sizeDelta = new Vector2(180f, 44f);

            var bgImg = btnRoot.GetComponent<Image>();
            bgImg.sprite = Resources.Load<Sprite>(PackButton) ?? RimaUITheme.ResourceFrame;
            bgImg.type = Image.Type.Sliced;
            bgImg.color = new Color(0.10f, 0.11f, 0.15f, 0.82f);
            bgImg.raycastTarget = true;

            var label = MakeText("GERİ", btnRoot, 16, FontStyles.Bold, RimaUITheme.TextPrimary);
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

            foreach (var kv in roomEntries)
                ApplyRoomEntryVisual(kv.Value, kv.Key == cls);

            if (accentBar != null) accentBar.color = accent;

            if (classNameLabel != null)
            {
                classNameLabel.text  = cls.ToString().ToUpperInvariant();
                classNameLabel.color = accent;
            }

            if (identityPortraitImage != null)
                identityPortraitImage.sprite = LoadCanonicalSprite(cls);

            var (tl1, tl2) = RimaUITheme.ClassTagline(cls);
            if (tagline1Label != null) tagline1Label.text = tl1;
            if (tagline2Label != null) tagline2Label.text = tl2;
            RefreshIdentityPanel(cls);
            ShowIdentityPopup();
            RefreshSkillList(cls);
            selectFlashTimer = 0.28f;

            // Start button color
            if (startButton != null)
            {
                var img = startButton.GetComponent<Image>();
                if (img != null) img.color = IsUnlocked(cls) ? accent : new Color(0.08f, 0.13f, 0.16f, 0.88f);
                startButton.interactable = IsUnlocked(cls);
                if (startButtonLabel != null)
                {
                    startButtonLabel.text = IsUnlocked(cls) ? "SEÇ" : $"KİLİDİ AÇ — {LockedButtonText(cls)}";
                    startButtonLabel.fontSize = IsUnlocked(cls) ? 21f : 12f;
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
            row.sizeDelta = new Vector2(204f, 76f);
            var layoutElement = row.gameObject.AddComponent<LayoutElement>();
            layoutElement.preferredWidth = 204f;
            layoutElement.minWidth = 188f;
            layoutElement.preferredHeight = 76f;
            layoutElement.minHeight = 76f;
            var rowImg = row.GetComponent<Image>();
            rowImg.sprite = RimaUITheme.SmallPanelFrame;
            rowImg.type = Image.Type.Sliced;
            rowImg.color = new Color(0.06f, 0.07f, 0.10f, 0.72f);
            rowImg.raycastTarget = false;

            var iconFrame = MakePanel("IconFrame", row);
            iconFrame.anchorMin = new Vector2(0f, 1f);
            iconFrame.anchorMax = new Vector2(0f, 1f);
            iconFrame.pivot = new Vector2(0.5f, 0.5f);
            iconFrame.anchoredPosition = new Vector2(28f, -28f);
            iconFrame.sizeDelta = new Vector2(40f, 40f);
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

            var name = MakeText(skill.skillName?.ToUpperInvariant() ?? "UNKNOWN", row, 10, FontStyles.Bold, RimaUITheme.ClassAccent(cls));
            name.alignment = TextAlignmentOptions.TopLeft;
            name.enableWordWrapping = true;
            name.overflowMode = TextOverflowModes.Ellipsis;
            var nameRt = name.transform as RectTransform;
            nameRt.anchorMin = new Vector2(0f, 0.56f); nameRt.anchorMax = new Vector2(1f, 0.94f);
            nameRt.offsetMin = new Vector2(54f, 0f); nameRt.offsetMax = new Vector2(-6f, 0f);

            var desc = MakeText(skill.description, row, 8, FontStyles.Normal, new Color(1f, 1f, 1f, 0.75f));
            desc.alignment = TextAlignmentOptions.TopLeft;
            desc.enableWordWrapping = true;
            desc.overflowMode = TextOverflowModes.Ellipsis;
            var descRt = desc.transform as RectTransform;
            descRt.anchorMin = new Vector2(0f, 0.08f); descRt.anchorMax = new Vector2(1f, 0.55f);
            descRt.offsetMin = new Vector2(54f, 0f); descRt.offsetMax = new Vector2(-6f, 0f);
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

        private static void ApplyRoomEntryVisual(RoomEntry entry, bool selected)
        {
            bool unlocked = IsUnlocked(entry.classType);
            var accent = RimaUITheme.ClassAccent(entry.classType);
            float alpha = selected ? 1f : (unlocked ? 0.75f : 0.40f);
            float scale = entry.baseScale * (selected ? 1.12f : 1f);

            if (entry.root != null)
            {
                entry.root.localScale = new Vector3(scale, scale, 1f);
                entry.root.anchoredPosition = entry.basePosition;
            }

            if (entry.canvasGroup != null)
                entry.canvasGroup.alpha = alpha;

            if (entry.sprite != null)
            {
                entry.sprite.color = unlocked
                    ? Color.white
                    : new Color(0.42f, 0.30f, 0.55f, 0.92f);
            }

            if (entry.seal != null)
            {
                entry.seal.enabled = selected;
                entry.seal.color = Color.white;
            }

            if (entry.glow != null)
            {
                entry.glow.enabled = selected;
                entry.glow.color = WithAlpha(accent, selected ? 0.32f : 0.0f);
            }

            if (entry.selectionVfxRoot != null)
                entry.selectionVfxRoot.gameObject.SetActive(selected);

            if (entry.selectionGlowDisk != null)
                entry.selectionGlowDisk.color = new Color(0f, 1f, 0.80f, selected ? 0.22f : 0.0f);

            if (entry.selectionRing != null)
                entry.selectionRing.color = new Color(0f, 1f, 0.80f, selected ? 0.46f : 0.0f);

            if (entry.selectionMotes != null)
            {
                foreach (var mote in entry.selectionMotes)
                {
                    if (mote != null)
                        mote.color = new Color(0f, 1f, 0.80f, 0.0f);
                }
            }

            if (entry.lockGlyph != null)
            {
                entry.lockGlyph.gameObject.SetActive(!unlocked);
                entry.lockGlyph.color = WithAlpha(RimaUITheme.Cyan, selected ? 0.30f : 0.18f);
            }

            if (entry.costChip != null)
            {
                entry.costChip.transform.parent.gameObject.SetActive(!unlocked);
                entry.costChip.text = entry.classType == ClassType.Hexer ? "250 Echo" : $"{UnlockCost(entry.classType)} Echo";
                entry.costChip.color = selected ? RimaUITheme.Cyan : RimaUITheme.TextMuted;
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
            float t = Time.unscaledTime;
            float pulse = 0.5f + 0.5f * Mathf.Sin(t * 3.4f);

            if (roomEntries.TryGetValue(selectedClass, out var selected))
            {
                float bob = Mathf.Sin(t * 2.25f) * 8f;
                if (selected.root != null)
                    selected.root.anchoredPosition = selected.basePosition + new Vector2(0f, bob);

                if (selected.glow != null)
                    selected.glow.color = new Color(0f, 1f, 0.80f, Mathf.Lerp(0.18f, 0.38f, pulse));

                if (selected.seal != null)
                {
                    float sealScale = Mathf.Lerp(0.98f, 1.04f, pulse);
                    selected.seal.rectTransform.localScale = new Vector3(sealScale, sealScale, 1f);
                }

                if (selected.selectionGlowDisk != null)
                {
                    float glowScale = Mathf.Lerp(0.96f, 1.10f, pulse);
                    selected.selectionGlowDisk.rectTransform.localScale = new Vector3(glowScale, glowScale, 1f);
                    selected.selectionGlowDisk.color = new Color(0f, 1f, 0.80f, Mathf.Lerp(0.16f, 0.34f, pulse));
                }

                if (selected.selectionRing != null)
                {
                    selected.selectionRing.rectTransform.localEulerAngles = new Vector3(0f, 0f, t * -28f);
                    selected.selectionRing.color = new Color(0f, 1f, 0.80f, Mathf.Lerp(0.34f, 0.56f, pulse));
                }

                if (selected.selectionMotes != null)
                {
                    for (int i = 0; i < selected.selectionMotes.Length; i++)
                    {
                        var mote = selected.selectionMotes[i];
                        if (mote == null)
                            continue;

                        float phase = Mathf.Repeat(t * 0.45f + i * 0.33f, 1f);
                        float angle = (i / 3f) * Mathf.PI * 2f + t * 0.38f;
                        float radius = Mathf.Lerp(40f, 92f, phase);
                        float y = Mathf.Lerp(-10f, 42f, phase);
                        mote.rectTransform.anchoredPosition = new Vector2(Mathf.Cos(angle) * radius, y);
                        mote.rectTransform.localScale = Vector3.one * Mathf.Lerp(0.78f, 1.15f, 1f - phase);
                        mote.color = new Color(0f, 1f, 0.80f, Mathf.Sin(phase * Mathf.PI) * 0.42f);
                    }
                }
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

        private static RectTransform MakeSkillStripArea(RectTransform parent, string name)
        {
            var root = MakePanel(name, parent);
            SetStretch(root, new Vector2(0f, 0f), new Vector2(1f, 0.92f), new Vector2(10f, 8f), new Vector2(-10f, -4f));
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
            layout.childForceExpandWidth = false;
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
            img.color = new Color(0.067f, 0.031f, 0.090f, 0.88f);
            img.raycastTarget = false;

            var edge = MakePanel("CyanEdge", rt);
            SetStretch(edge, Vector2.zero, Vector2.one, new Vector2(-2f, -2f), new Vector2(2f, 2f));
            var edgeImg = edge.GetComponent<Image>();
            edgeImg.sprite = img.sprite;
            edgeImg.type = Image.Type.Sliced;
            edgeImg.color = new Color(0f, 1f, 0.80f, 0.38f);
            edgeImg.raycastTarget = false;

            edge.SetAsFirstSibling();
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
