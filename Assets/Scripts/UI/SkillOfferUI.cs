using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using RIMA.Combat.Skills;

namespace RIMA
{
    /// <summary>
    /// Hades-style skill draft — 3 cards, slide-in animation, replace mode.
    /// TimeScale managed by UIManager (no direct Time.timeScale writes).
    /// </summary>
    public class SkillOfferUI : MonoBehaviour
    {
        // ── Callback ─────────────────────────────────────────────────
        private Action<RewardOffer, int> onPick;

        // ── Card tracking ────────────────────────────────────────────
        private readonly List<GameObject> cards = new List<GameObject>();
        private readonly List<CardJuiceState> cardJuiceStates = new List<CardJuiceState>();
        // A5 chain-UI: skillNames of skills offered in the CURRENT draft, so a card can detect a
        // Sundered-Beat interlock with a sibling card (not just an already-owned skill).
        private readonly List<string> currentOfferNames = new List<string>();
        private GameObject panel;
        private Transform cardContainer;
        private TextMeshProUGUI titleLabel;
        private TextMeshProUGUI subtitleLabel;
        private Image screenFlash;
        private bool isConfirmingPick;

        // ── Layout constants ─────────────────────────────────────────
        private const float CardWidth  = 180f;
        private const float CardHeight = 260f;
        private const float CardGap    = 20f;
        private const float SlideInDuration = 0.3f;
        private const float StaggerDelay    = 0.08f;
        private const float HoverDuration = 0.12f;
        private const float HoverScale = 1.08f;
        private const float IdleGlowMinAlpha = 0.15f;
        private const float IdleGlowMaxAlpha = 0.35f;
        private const float HoverGlowAlpha = 0.75f;
        private const float IdleGlowScale = 0.95f;
        private const float HoverGlowScale = 1.12f;

        private void Awake()
        {
            if (panel != null) panel.SetActive(false);
        }

        // ─── Public API (DraftManager contract) ─────────────────────

        /// <summary>Show reward cards (3 offers).</summary>
        public void Show(List<RewardOffer> offers, Action<RewardOffer, int> callback, int roomNumber = 0)
        {
            onPick = callback;
            ClearCards();

            if (panel == null) BuildPanel();
            else panel.SetActive(true);

            string title = roomNumber > 0 ? $"ODA {roomNumber}  —  ODUL SEC" : "ODUL SEC";
            if (titleLabel != null) titleLabel.text = title;
            if (subtitleLabel != null) subtitleLabel.text = "Birini sec — digerleri kaybolur";

            // A5 chain-UI: snapshot the offered skillNames so each card can detect a chain with a sibling card.
            currentOfferNames.Clear();
            for (int i = 0; i < offers.Count; i++)
            {
                var nm = offers[i]?.skill?.skillName;
                if (!string.IsNullOrEmpty(nm)) currentOfferNames.Add(nm);
            }

            for (int i = 0; i < offers.Count; i++)
                BuildRewardCard(offers[i], i, offers.Count);

            // Notify UIManager
            if (UIManager.Instance != null) UIManager.Instance.OpenSkillOffer();
        }

        /// <summary>Replace mode — current skills shown for swap.</summary>
        public void ShowReplaceMode(List<SkillData> currentActives, SkillData incoming,
                                    Action<SkillData> onReplace, Action onSkip)
        {
            ClearCards();

            if (panel == null) BuildPanel();
            else panel.SetActive(true);

            if (titleLabel != null) titleLabel.text = "SLOT DOLU";
            if (subtitleLabel != null)
                subtitleLabel.text = $"{incoming.skillName.ToUpperInvariant()} almak icin hangisini birakmak istiyorsun?";

            // Build replace cards
            for (int i = 0; i < currentActives.Count; i++)
            {
                var sd = currentActives[i];
                var card = BuildSkillCard(sd.skillName, sd.tier, sd.description, i, currentActives.Count, sd.icon);
                var btn = card.GetComponentInChildren<Button>();
                if (btn != null)
                {
                    var captured = sd;
                    btn.onClick.AddListener(() => onReplace?.Invoke(captured));
                }
            }

            // Skip button
            var skipGo = new GameObject("SkipBtn", typeof(RectTransform));
            skipGo.transform.SetParent(panel.transform, false);
            var skipRt = skipGo.GetComponent<RectTransform>();
            skipRt.anchorMin = new Vector2(0.5f, 0f);
            skipRt.anchorMax = new Vector2(0.5f, 0f);
            skipRt.pivot = new Vector2(0.5f, 0f);
            skipRt.anchoredPosition = new Vector2(0f, 20f);
            skipRt.sizeDelta = new Vector2(200f, 36f);

            var skipImg = skipGo.AddComponent<Image>();
            skipImg.color = new Color(0.10f, 0.10f, 0.13f, 0.92f);
            var skipBtn = skipGo.AddComponent<Button>();
            skipBtn.onClick.AddListener(() => onSkip?.Invoke());

            var skipTxt = MakeTMP("Label", skipGo.GetComponent<RectTransform>());
            var str = skipTxt.GetComponent<RectTransform>();
            str.anchorMin = Vector2.zero;
            str.anchorMax = Vector2.one;
            str.offsetMin = str.offsetMax = Vector2.zero;
            skipTxt.text = "ATLA — alma";
            skipTxt.fontSize = 11f;
            skipTxt.fontStyle = FontStyles.Bold;
            skipTxt.color = new Color(0.48f, 0.48f, 0.55f);
            skipTxt.alignment = TextAlignmentOptions.Center;
            cards.Add(skipGo);

            if (UIManager.Instance != null) UIManager.Instance.OpenSkillOffer();
        }

        public void Hide()
        {
            ClearCards();
            if (panel != null) panel.SetActive(false);
            if (UIManager.Instance != null) UIManager.Instance.CloseSkillOffer();
        }

        // ─── Build Panel ────────────────────────────────────────────

        private void BuildPanel()
        {
            var canvasGo = new GameObject("[SkillOfferPanel]", typeof(RectTransform));
            canvasGo.transform.SetParent(transform, false);

            // Check for existing canvas on transform.root
            var rootCanvas = GetComponentInParent<Canvas>();
            if (rootCanvas == null)
            {
                var canvas = canvasGo.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 1050;
                canvasGo.AddComponent<CanvasScaler>();
                canvasGo.AddComponent<GraphicRaycaster>();
            }

            panel = canvasGo;
            var panelRt = panel.GetComponent<RectTransform>();
            panelRt.anchorMin = Vector2.zero;
            panelRt.anchorMax = Vector2.one;
            panelRt.offsetMin = panelRt.offsetMax = Vector2.zero;

            // Dark overlay
            var overlayImg = panel.AddComponent<Image>();
            overlayImg.color = RimaUITheme.OverlayDark;
            overlayImg.raycastTarget = true;

            // Title
            titleLabel = MakeTMP("Title", panelRt);
            var trt = titleLabel.GetComponent<RectTransform>();
            trt.anchorMin = new Vector2(0.5f, 1f);
            trt.anchorMax = new Vector2(0.5f, 1f);
            trt.pivot = new Vector2(0.5f, 1f);
            trt.anchoredPosition = new Vector2(0f, -30f);
            trt.sizeDelta = new Vector2(400f, 30f);
            titleLabel.fontSize = 18f;
            titleLabel.fontStyle = FontStyles.Bold;
            titleLabel.color = RimaUITheme.Gold;
            titleLabel.alignment = TextAlignmentOptions.Center;

            // Subtitle
            subtitleLabel = MakeTMP("Subtitle", panelRt);
            var srt = subtitleLabel.GetComponent<RectTransform>();
            srt.anchorMin = new Vector2(0.5f, 1f);
            srt.anchorMax = new Vector2(0.5f, 1f);
            srt.pivot = new Vector2(0.5f, 1f);
            srt.anchoredPosition = new Vector2(0f, -58f);
            srt.sizeDelta = new Vector2(400f, 20f);
            subtitleLabel.fontSize = 11f;
            subtitleLabel.color = new Color(0.6f, 0.65f, 0.7f, 0.85f);
            subtitleLabel.alignment = TextAlignmentOptions.Center;

            // Card container
            var containerGo = new GameObject("Cards", typeof(RectTransform));
            containerGo.transform.SetParent(panel.transform, false);
            cardContainer = containerGo.transform;
            var crt = containerGo.GetComponent<RectTransform>();
            crt.anchorMin = new Vector2(0.5f, 0.5f);
            crt.anchorMax = new Vector2(0.5f, 0.5f);
            crt.pivot = new Vector2(0.5f, 0.5f);
            crt.anchoredPosition = new Vector2(0f, -20f);
            crt.sizeDelta = new Vector2(600f, CardHeight);
        }

        // ─── Reward Card ────────────────────────────────────────────

        private void BuildRewardCard(RewardOffer offer, int index, int total)
        {
            Color tierColor = TierColor(offer);
            string name, desc;
            string tierTag = "";

            if (offer.type == RewardType.Gold)
            {
                name = $"+{offer.goldAmount} ALTIN";
                desc = "Hazinene ekle";
                tierTag = "GOLD";
                tierColor = RimaUITheme.Gold;
            }
            else if (offer.type == RewardType.Heal)
            {
                name = $"+%{offer.healPercent} CAN";
                desc = "Aninda iyiles";
                tierTag = "HEAL";
                tierColor = new Color(0.28f, 0.78f, 0.45f);
            }
            else if (offer.type == RewardType.CrossClassEcho)
            {
                // B5: distinct cyan ECHO treatment (cyan = echo / seal energy).
                name = (offer.crossClass != null ? $"Echo of {offer.crossClass.sourceClass}" : "Echo").ToUpperInvariant();
                desc = offer.crossClass?.description ?? "Cagir bir yankisi (C).";
                tierTag = "ECHO";
                tierColor = RimaUITheme.Cyan;
            }
            else
            {
                name = offer.skill?.skillName?.ToUpperInvariant() ?? "???";
                desc = offer.skill?.description ?? "";
                tierTag = offer.skill?.tier.ToString().ToUpperInvariant() ?? "";
            }

            var card = BuildSkillCard(name, offer.skill?.tier ?? SkillTier.Common, desc, index, total,
                offer.type == RewardType.CrossClassEcho ? offer.crossClass?.icon : offer.skill?.icon,
                tierColor);

            // Select button
            var btn = card.GetComponentInChildren<Button>();
            if (btn != null)
            {
                int idx = index;
                var captured = offer;
                btn.onClick.AddListener(() =>
                {
                    BeginConfirmPick(captured, idx, card);
                });
            }

            // Tier chip
            var chipGo = new GameObject("TierChip", typeof(RectTransform));
            chipGo.transform.SetParent(card.transform, false);
            var chipRt = chipGo.GetComponent<RectTransform>();
            chipRt.anchorMin = new Vector2(0f, 1f);
            chipRt.anchorMax = new Vector2(0f, 1f);
            chipRt.pivot = new Vector2(0f, 1f);
            chipRt.anchoredPosition = new Vector2(8f, -8f);
            chipRt.sizeDelta = new Vector2(60f, 16f);

            var chipImg = chipGo.AddComponent<Image>();
            chipImg.color = tierColor;

            var chipTxt = MakeTMP("ChipTxt", chipRt);
            var ctr = chipTxt.GetComponent<RectTransform>();
            ctr.anchorMin = Vector2.zero;
            ctr.anchorMax = Vector2.one;
            ctr.offsetMin = ctr.offsetMax = Vector2.zero;
            chipTxt.text = tierTag;
            chipTxt.fontSize = 7f;
            chipTxt.fontStyle = FontStyles.Bold;
            chipTxt.color = Color.white;
            chipTxt.alignment = TextAlignmentOptions.Center;

            // A5 chain-UI: if this offered skill interlocks (Sundered-Beat chain) with another
            // offered card OR an already-owned active, show a cyan "pairs with X" chip.
            string partner = FindChainPartner(offer.skill?.skillName);
            if (!string.IsNullOrEmpty(partner))
                BuildChainChip(card, partner);
        }

        // ─── A5 Chain Chip (Sundered-Beat interlock tell) ───────────

        /// <summary>Find a skill that the given offered skill chains with — either a sibling offered
        /// card or an already-owned active. Uses the static ChainWindowTracker producer↔consumer table
        /// (no runtime state). Checks BOTH directions so the chip shows whether this card is the
        /// producer OR the consumer of the chain. Returns the partner's display name, or null.</summary>
        private string FindChainPartner(string skillName)
        {
            if (string.IsNullOrEmpty(skillName)) return null;

            // Sibling offered cards (skip self), then already-owned actives.
            for (int i = 0; i < currentOfferNames.Count; i++)
            {
                string other = currentOfferNames[i];
                if (string.IsNullOrEmpty(other) || other == skillName) continue;
                if (Chains(skillName, other)) return other;
            }

            var owned = DraftManager.Instance?.OwnedActiveSkillNames;
            if (owned != null)
            {
                for (int i = 0; i < owned.Count; i++)
                {
                    string other = owned[i];
                    if (string.IsNullOrEmpty(other) || other == skillName) continue;
                    if (Chains(skillName, other)) return other;
                }
            }

            return null;
        }

        /// <summary>True if skills a and b form a Sundered-Beat chain in either order.</summary>
        private static bool Chains(string a, string b)
            => ChainWindowTracker.ChainsWith(a, b) || ChainWindowTracker.ChainsWith(b, a);

        /// <summary>Cyan synergy chip pinned to the card's bottom-right — "⟂ pairs with {partner}".
        /// Mirrors the TierChip build pattern; cyan = RIMA seal/synergy energy (distinct from the
        /// red/orange enemy break tells).</summary>
        private void BuildChainChip(GameObject card, string partner)
        {
            var chipGo = new GameObject("ChainChip", typeof(RectTransform));
            chipGo.transform.SetParent(card.transform, false);
            var chipRt = chipGo.GetComponent<RectTransform>();
            chipRt.anchorMin = new Vector2(0.5f, 0f);
            chipRt.anchorMax = new Vector2(0.5f, 0f);
            chipRt.pivot = new Vector2(0.5f, 0f);
            chipRt.anchoredPosition = new Vector2(0f, 44f); // just above the SEC button
            chipRt.sizeDelta = new Vector2(CardWidth - 16f, 14f);

            var chipImg = chipGo.AddComponent<Image>();
            chipImg.color = new Color(RimaUITheme.Cyan.r, RimaUITheme.Cyan.g, RimaUITheme.Cyan.b, 0.18f);
            chipImg.raycastTarget = false;

            var chipTxt = MakeTMP("ChainTxt", chipRt);
            var ctr = chipTxt.GetComponent<RectTransform>();
            ctr.anchorMin = Vector2.zero;
            ctr.anchorMax = Vector2.one;
            ctr.offsetMin = ctr.offsetMax = Vector2.zero;
            chipTxt.text = $"⟂ pairs with {partner.ToUpperInvariant()}";
            chipTxt.fontSize = 7.5f;
            chipTxt.fontStyle = FontStyles.Bold;
            chipTxt.color = RimaUITheme.Cyan;
            chipTxt.alignment = TextAlignmentOptions.Center;
            chipTxt.enableWordWrapping = false;
            chipTxt.overflowMode = TextOverflowModes.Ellipsis;
        }

        // ─── Shared Card Builder ────────────────────────────────────

        private GameObject BuildSkillCard(string skillName, SkillTier tier, string description, int index, int total, Sprite icon = null, Color? glowTintOverride = null)
        {
            var cardGo = new GameObject($"Card_{index}", typeof(RectTransform));
            cardGo.transform.SetParent(cardContainer, false);
            var rt = cardGo.GetComponent<RectTransform>();

            // Position cards centered horizontally
            float totalW = total * CardWidth + (total - 1) * CardGap;
            float startX = -totalW / 2f + CardWidth / 2f;
            float x = startX + index * (CardWidth + CardGap);

            rt.anchoredPosition = new Vector2(x, -CardHeight); // start below (for slide-in)
            rt.sizeDelta = new Vector2(CardWidth, CardHeight);
            rt.localScale = Vector3.one;

            var cardCanvas = cardGo.AddComponent<Canvas>();
            cardCanvas.overrideSorting = false;
            cardCanvas.sortingOrder = 0;
            cardGo.AddComponent<GraphicRaycaster>();

            var cardGroup = cardGo.AddComponent<CanvasGroup>();
            cardGroup.alpha = 1f;
            cardGroup.interactable = true;
            cardGroup.blocksRaycasts = true;

            var glowGo = new GameObject("CyanGlow", typeof(RectTransform));
            glowGo.transform.SetParent(cardGo.transform, false);
            var glowRt = glowGo.GetComponent<RectTransform>();
            glowRt.anchorMin = new Vector2(0.5f, 0.5f);
            glowRt.anchorMax = new Vector2(0.5f, 0.5f);
            glowRt.pivot = new Vector2(0.5f, 0.5f);
            glowRt.anchoredPosition = Vector2.zero;
            glowRt.sizeDelta = new Vector2(CardWidth + 24f, CardHeight + 24f);
            glowRt.localScale = Vector3.one * IdleGlowScale;

            var glowImg = glowGo.AddComponent<Image>();
            Color glowTint = glowTintOverride ?? TierColor(tier);
            glowImg.color = WithAlpha(glowTint, IdleGlowMinAlpha);
            glowImg.raycastTarget = false;

            // Card background (dark stone)
            var bgGo = new GameObject("Bg", typeof(RectTransform));
            bgGo.transform.SetParent(cardGo.transform, false);
            var bgRt = bgGo.GetComponent<RectTransform>();
            bgRt.anchorMin = Vector2.zero;
            bgRt.anchorMax = Vector2.one;
            bgRt.offsetMin = bgRt.offsetMax = Vector2.zero;

            var bgImg = bgGo.AddComponent<Image>();
            bgImg.sprite = RimaUITheme.SmallPanelFrame;
            bgImg.color = RimaUITheme.PanelTint;

            var juiceState = new CardJuiceState
            {
                Card = cardGo,
                Root = rt,
                Glow = glowRt,
                GlowImage = glowImg,
                Canvas = cardCanvas,
                CanvasGroup = cardGroup,
                GlowTint = glowTint,
                Phase = index
            };
            cardJuiceStates.Add(juiceState);
            AttachCardJuiceHandler(bgGo, juiceState);
            juiceState.IdleGlow = StartCoroutine(PulseGlow(juiceState));

            // Icon placeholder (64x64 centered)
            var iconGo = new GameObject("Icon", typeof(RectTransform));
            iconGo.transform.SetParent(cardGo.transform, false);
            var iconRt = iconGo.GetComponent<RectTransform>();
            iconRt.anchorMin = new Vector2(0.5f, 1f);
            iconRt.anchorMax = new Vector2(0.5f, 1f);
            iconRt.pivot = new Vector2(0.5f, 1f);
            iconRt.anchoredPosition = new Vector2(0f, -30f);
            iconRt.sizeDelta = new Vector2(64f, 64f);

            var iconImg = iconGo.AddComponent<Image>();
            if (icon != null)
            {
                iconImg.sprite = icon;
                iconImg.color = Color.white;
            }
            else
            {
                iconImg.color = new Color(0.15f, 0.15f, 0.2f, 0.8f); // placeholder
            }
            iconImg.raycastTarget = false;

            // Skill name
            var nameTmp = MakeTMP("Name", rt);
            var nrt = nameTmp.GetComponent<RectTransform>();
            nrt.anchorMin = new Vector2(0f, 1f);
            nrt.anchorMax = new Vector2(1f, 1f);
            nrt.pivot = new Vector2(0.5f, 1f);
            nrt.anchoredPosition = new Vector2(0f, -100f);
            nrt.sizeDelta = new Vector2(0f, 22f);
            nameTmp.text = skillName?.ToUpperInvariant() ?? "???";
            nameTmp.fontSize = 14f;
            nameTmp.fontStyle = FontStyles.Bold;
            nameTmp.color = Color.white;
            nameTmp.alignment = TextAlignmentOptions.Center;

            // Description
            var descTmp = MakeTMP("Desc", rt);
            var drt = descTmp.GetComponent<RectTransform>();
            drt.anchorMin = new Vector2(0f, 1f);
            drt.anchorMax = new Vector2(1f, 1f);
            drt.pivot = new Vector2(0.5f, 1f);
            drt.anchoredPosition = new Vector2(0f, -126f);
            drt.sizeDelta = new Vector2(-16f, 60f);
            descTmp.text = description ?? "";
            descTmp.fontSize = 9f;
            descTmp.color = new Color(0.7f, 0.75f, 0.8f, 0.9f);
            descTmp.alignment = TextAlignmentOptions.Center;
            descTmp.enableWordWrapping = true;

            // Select button
            var btnGo = new GameObject("Btn", typeof(RectTransform));
            btnGo.transform.SetParent(cardGo.transform, false);
            var btnRt = btnGo.GetComponent<RectTransform>();
            btnRt.anchorMin = new Vector2(0.5f, 0f);
            btnRt.anchorMax = new Vector2(0.5f, 0f);
            btnRt.pivot = new Vector2(0.5f, 0f);
            btnRt.anchoredPosition = new Vector2(0f, 12f);
            btnRt.sizeDelta = new Vector2(120f, 28f);

            var btnImg = btnGo.AddComponent<Image>();
            btnImg.color = new Color(RimaUITheme.Cyan.r, RimaUITheme.Cyan.g, RimaUITheme.Cyan.b, 0.3f);
            juiceState.Button = btnGo.AddComponent<Button>();
            AttachCardJuiceHandler(btnGo, juiceState);

            var btnTxt = MakeTMP("BtnLabel", btnRt);
            var btr = btnTxt.GetComponent<RectTransform>();
            btr.anchorMin = Vector2.zero;
            btr.anchorMax = Vector2.one;
            btr.offsetMin = btr.offsetMax = Vector2.zero;
            btnTxt.text = "SEC";
            btnTxt.fontSize = 11f;
            btnTxt.fontStyle = FontStyles.Bold;
            btnTxt.color = Color.white;
            btnTxt.alignment = TextAlignmentOptions.Center;

            cards.Add(cardGo);

            // Slide-in animation
            StartCoroutine(SlideIn(rt, new Vector2(x, 0f), index));

            return cardGo;
        }

        // ─── Animations ─────────────────────────────────────────────

        private IEnumerator SlideIn(RectTransform rt, Vector2 target, int index)
        {
            // Stagger delay
            float delay = index * StaggerDelay;
            float elapsed = 0f;
            while (elapsed < delay)
            {
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            Vector2 start = rt.anchoredPosition;
            float t = 0f;
            while (t < SlideInDuration)
            {
                t += Time.unscaledDeltaTime;
                float ease = 1f - Mathf.Pow(1f - Mathf.Clamp01(t / SlideInDuration), 3f); // ease-out cubic
                rt.anchoredPosition = Vector2.Lerp(start, target, ease);
                yield return null;
            }
            rt.anchoredPosition = target;
        }

        private void BeginConfirmPick(RewardOffer offer, int index, GameObject chosenCard)
        {
            if (isConfirmingPick) return;

            isConfirmingPick = true;
            StopAllCoroutines();

            CardJuiceState chosen = null;
            for (int i = 0; i < cardJuiceStates.Count; i++)
            {
                var state = cardJuiceStates[i];
                state.HoverTween = null;
                state.IdleGlow = null;
                state.IsHoverTweening = false;
                state.Canvas.overrideSorting = state.Card == chosenCard;
                state.Canvas.sortingOrder = state.Card == chosenCard ? 20 : 0;
                if (state.Button != null) state.Button.interactable = false;

                if (state.Card == chosenCard) chosen = state;
            }

            if (chosen == null)
            {
                onPick?.Invoke(offer, index);
                return;
            }

            StartCoroutine(ConfirmPickRoutine(chosen, offer, index));
        }

        private IEnumerator ConfirmPickRoutine(CardJuiceState chosen, RewardOffer offer, int index)
        {
            EnsureScreenFlash();
            StartCoroutine(ScreenFlashRoutine());

            float anticipationTime = 0.06f;
            Vector3 startScale = chosen.Root.localScale;
            Vector3 squashScale = Vector3.one * 0.94f;
            float elapsed = 0f;
            while (elapsed < anticipationTime)
            {
                elapsed += Time.unscaledDeltaTime;
                float ease = EaseInQuad(Mathf.Clamp01(elapsed / anticipationTime));
                chosen.Root.localScale = Vector3.LerpUnclamped(startScale, squashScale, ease);
                SetGlow(chosen, HoverGlowAlpha, HoverGlowScale);
                yield return null;
            }

            Vector2 chosenStartPos = chosen.Root.anchoredPosition;
            Vector3 chosenStartScale = chosen.Root.localScale;
            Vector2[] otherStartPositions = new Vector2[cardJuiceStates.Count];
            float[] otherStartAlphas = new float[cardJuiceStates.Count];
            for (int i = 0; i < cardJuiceStates.Count; i++)
            {
                var state = cardJuiceStates[i];
                otherStartPositions[i] = state.Root.anchoredPosition;
                otherStartAlphas[i] = state.CanvasGroup.alpha;
                if (state != chosen)
                {
                    state.CanvasGroup.interactable = false;
                    state.CanvasGroup.blocksRaycasts = false;
                }
            }

            float flyTime = 0.30f;
            float dropTime = 0.22f;
            elapsed = 0f;
            while (elapsed < flyTime)
            {
                elapsed += Time.unscaledDeltaTime;
                float flyT = Mathf.Clamp01(elapsed / flyTime);
                float flyEase = EaseOutBack(flyT);

                chosen.Root.anchoredPosition = Vector2.LerpUnclamped(chosenStartPos, Vector2.zero, flyEase);
                chosen.Root.localScale = Vector3.LerpUnclamped(chosenStartScale, Vector3.one * 1.25f, flyEase);
                SetGlow(chosen, HoverGlowAlpha, HoverGlowScale);

                float dropT = Mathf.Clamp01(elapsed / dropTime);
                float dropEase = EaseInCubic(dropT);
                for (int i = 0; i < cardJuiceStates.Count; i++)
                {
                    var state = cardJuiceStates[i];
                    if (state == chosen) continue;

                    state.Root.anchoredPosition = Vector2.LerpUnclamped(
                        otherStartPositions[i],
                        otherStartPositions[i] + new Vector2(0f, -500f),
                        dropEase);
                    state.CanvasGroup.alpha = Mathf.Lerp(otherStartAlphas[i], 0f, dropEase);
                    SetGlow(state, 0f, IdleGlowScale);
                }

                yield return null;
            }

            onPick?.Invoke(offer, index);
        }

        private IEnumerator ScreenFlashRoutine()
        {
            float elapsed = 0f;
            while (elapsed < 0.04f)
            {
                elapsed += Time.unscaledDeltaTime;
                SetScreenFlashAlpha(Mathf.Lerp(0f, 0.5f, Mathf.Clamp01(elapsed / 0.04f)));
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < 0.16f)
            {
                elapsed += Time.unscaledDeltaTime;
                SetScreenFlashAlpha(Mathf.Lerp(0.5f, 0f, Mathf.Clamp01(elapsed / 0.16f)));
                yield return null;
            }

            SetScreenFlashAlpha(0f);
        }

        private IEnumerator PulseGlow(CardJuiceState state)
        {
            while (true)
            {
                if (!isConfirmingPick && !state.IsHovered && !state.IsHoverTweening)
                {
                    float wave = (Mathf.Sin(Time.unscaledTime * 1.2f + state.Phase) + 1f) * 0.5f;
                    float alpha = Mathf.Lerp(IdleGlowMinAlpha, IdleGlowMaxAlpha, wave);
                    SetGlow(state, alpha, IdleGlowScale);
                }

                yield return null;
            }
        }

        private void SetPointerHover(CardJuiceState state, bool entered)
        {
            if (state == null || isConfirmingPick) return;

            state.PointerInsideCount += entered ? 1 : -1;
            if (state.PointerInsideCount < 0) state.PointerInsideCount = 0;
            RefreshHover(state);
        }

        private void SetSelectionHover(CardJuiceState state, bool selected)
        {
            if (state == null || isConfirmingPick) return;

            state.Selected = selected;
            RefreshHover(state);
        }

        private void RefreshHover(CardJuiceState state)
        {
            bool shouldHover = state.PointerInsideCount > 0 || state.Selected;
            if (state.IsHovered == shouldHover) return;

            state.IsHovered = shouldHover;
            if (state.HoverTween != null) StopCoroutine(state.HoverTween);
            state.HoverTween = StartCoroutine(TweenHover(state, shouldHover));
        }

        private IEnumerator TweenHover(CardJuiceState state, bool hover)
        {
            state.IsHoverTweening = true;
            if (hover)
            {
                state.Canvas.overrideSorting = true;
                state.Canvas.sortingOrder = 10;
            }

            float startScale = state.Root.localScale.x;
            float targetScale = hover ? HoverScale : 1f;
            float startGlowAlpha = state.GlowImage.color.a;
            float targetGlowAlpha = hover ? HoverGlowAlpha : IdleGlowMinAlpha;
            float startGlowScale = state.Glow.localScale.x;
            float targetGlowScale = hover ? HoverGlowScale : IdleGlowScale;

            float elapsed = 0f;
            while (elapsed < HoverDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float ease = EaseOutCubic(Mathf.Clamp01(elapsed / HoverDuration));
                state.Root.localScale = Vector3.one * Mathf.LerpUnclamped(startScale, targetScale, ease);
                SetGlow(
                    state,
                    Mathf.LerpUnclamped(startGlowAlpha, targetGlowAlpha, ease),
                    Mathf.LerpUnclamped(startGlowScale, targetGlowScale, ease));
                yield return null;
            }

            state.Root.localScale = Vector3.one * targetScale;
            SetGlow(state, targetGlowAlpha, targetGlowScale);
            if (!hover)
            {
                state.Canvas.sortingOrder = 0;
                state.Canvas.overrideSorting = false;
            }

            state.IsHoverTweening = false;
            state.HoverTween = null;
        }

        // ─── Helpers ────────────────────────────────────────────────

        private void ClearCards()
        {
            StopAllCoroutines();
            foreach (var c in cards)
                if (c != null) Destroy(c);
            cards.Clear();
            cardJuiceStates.Clear();
            currentOfferNames.Clear();
            isConfirmingPick = false;
            SetScreenFlashAlpha(0f);
        }

        private Color TierColor(RewardOffer offer)
        {
            if (offer.type == RewardType.Gold) return RimaUITheme.Gold;
            if (offer.type == RewardType.Heal) return new Color(0.28f, 0.78f, 0.45f);
            if (offer.type == RewardType.CrossClassEcho) return RimaUITheme.Cyan;
            if (offer.skill == null) return RimaUITheme.TierCommon;

            return offer.skill.tier switch
            {
                SkillTier.Common    => RimaUITheme.TierCommon,
                SkillTier.Rare      => RimaUITheme.TierRare,
                SkillTier.Epic      => RimaUITheme.TierEpic,
                SkillTier.Legendary => RimaUITheme.TierLegendary,
                _                   => RimaUITheme.TierCommon
            };
        }

        private static Color TierColor(SkillTier tier)
        {
            return tier switch
            {
                SkillTier.Common    => RimaUITheme.TierCommon,
                SkillTier.Rare      => RimaUITheme.TierRare,
                SkillTier.Epic      => RimaUITheme.TierEpic,
                SkillTier.Legendary => RimaUITheme.TierLegendary,
                _                   => RimaUITheme.TierCommon
            };
        }

        private void AttachCardJuiceHandler(GameObject target, CardJuiceState state)
        {
            var handler = target.AddComponent<CardJuiceHandler>();
            handler.Init(this, state);
        }

        private void EnsureScreenFlash()
        {
            if (screenFlash != null) return;
            if (panel == null) return;

            var flashGo = new GameObject("ScreenFlash", typeof(RectTransform));
            flashGo.transform.SetParent(panel.transform, false);
            var flashRt = flashGo.GetComponent<RectTransform>();
            flashRt.anchorMin = Vector2.zero;
            flashRt.anchorMax = Vector2.one;
            flashRt.offsetMin = flashRt.offsetMax = Vector2.zero;
            screenFlash = flashGo.AddComponent<Image>();
            screenFlash.color = WithAlpha(RimaUITheme.Cyan, 0f);
            screenFlash.raycastTarget = false;
            flashGo.transform.SetAsLastSibling();
        }

        private void SetScreenFlashAlpha(float alpha)
        {
            if (screenFlash == null) return;
            screenFlash.color = WithAlpha(RimaUITheme.Cyan, alpha);
        }

        private static void SetGlow(CardJuiceState state, float alpha, float scale)
        {
            state.GlowImage.color = WithAlpha(state.GlowTint, alpha);
            state.Glow.localScale = Vector3.one * scale;
        }

        private static Color WithAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        private static float EaseOutCubic(float t)
        {
            return 1f - Mathf.Pow(1f - t, 3f);
        }

        private static float EaseOutBack(float t)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1f;
            return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
        }

        private static float EaseInQuad(float t)
        {
            return t * t;
        }

        private static float EaseInCubic(float t)
        {
            return t * t * t;
        }

        private static TextMeshProUGUI MakeTMP(string name, RectTransform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.raycastTarget = false;
            return tmp;
        }

        private sealed class CardJuiceState
        {
            public GameObject Card;
            public RectTransform Root;
            public RectTransform Glow;
            public Image GlowImage;
            public Canvas Canvas;
            public CanvasGroup CanvasGroup;
            public Button Button;
            public Coroutine HoverTween;
            public Coroutine IdleGlow;
            public Color GlowTint;
            public float Phase;
            public int PointerInsideCount;
            public bool Selected;
            public bool IsHovered;
            public bool IsHoverTweening;
        }

        private sealed class CardJuiceHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
        {
            private SkillOfferUI owner;
            private CardJuiceState state;

            public void Init(SkillOfferUI owner, CardJuiceState state)
            {
                this.owner = owner;
                this.state = state;
            }

            public void OnPointerEnter(PointerEventData eventData)
            {
                owner?.SetPointerHover(state, true);
            }

            public void OnPointerExit(PointerEventData eventData)
            {
                owner?.SetPointerHover(state, false);
            }

            public void OnSelect(BaseEventData eventData)
            {
                owner?.SetSelectionHover(state, true);
            }

            public void OnDeselect(BaseEventData eventData)
            {
                owner?.SetSelectionHover(state, false);
            }
        }
    }
}
