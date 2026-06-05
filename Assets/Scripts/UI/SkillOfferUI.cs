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
        private SkillBarUI skillBar;
        private bool isConfirmingPick;
        private int hoverSerial;

        // ── Layout constants ─────────────────────────────────────────
        private const float CardWidth  = 280f;
        private const float CardHeight = 400f;
        private const float CardGap    = 36f;
        private const float SlideInDuration = 0.3f;
        private const float StaggerDelay    = 0.08f;
        private const float HoverDuration = 0.12f;
        private const float HoverScale = 1.05f;
        private const float DimmedScale = 0.95f;
        private const float HoverLift = 20f;
        private const float DimmedAlpha = 0.4f;
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
            EnsureTooltipSystem();
            skillBar = FindObjectOfType<SkillBarUI>();

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
            EnsureTooltipSystem();
            skillBar = FindObjectOfType<SkillBarUI>();

            if (titleLabel != null) titleLabel.text = "SLOT DOLU";
            if (subtitleLabel != null)
                subtitleLabel.text = $"{incoming.skillName.ToUpperInvariant()} almak icin hangisini birakmak istiyorsun?";

            // Build replace cards
            for (int i = 0; i < currentActives.Count; i++)
            {
                var sd = currentActives[i];
                var card = BuildSkillCard(sd.skillName, sd.tier, sd.description, i, currentActives.Count, sd.icon, null, sd);
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
            trt.sizeDelta = new Vector2(600f, 44f);
            titleLabel.fontSize = 34f;
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
            srt.sizeDelta = new Vector2(600f, 24f);
            subtitleLabel.fontSize = 15f;
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
            crt.anchoredPosition = Vector2.zero;
            crt.sizeDelta = new Vector2(3f * CardWidth + 2f * CardGap, CardHeight);
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
                tierColor,
                offer.skill);

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
            chipGo.transform.SetParent(GetCardVisualRoot(card), false);
            var chipRt = chipGo.GetComponent<RectTransform>();
            chipRt.anchorMin = new Vector2(0f, 1f);
            chipRt.anchorMax = new Vector2(0f, 1f);
            chipRt.pivot = new Vector2(0f, 1f);
            chipRt.anchoredPosition = new Vector2(10f, -10f);
            chipRt.sizeDelta = new Vector2(80f, 22f);

            var chipImg = chipGo.AddComponent<Image>();
            chipImg.color = tierColor;
            chipImg.raycastTarget = false;

            var chipTxt = MakeTMP("ChipTxt", chipRt);
            var ctr = chipTxt.GetComponent<RectTransform>();
            ctr.anchorMin = Vector2.zero;
            ctr.anchorMax = Vector2.one;
            ctr.offsetMin = ctr.offsetMax = Vector2.zero;
            chipTxt.text = tierTag;
            chipTxt.fontSize = 10f;
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
            chipGo.transform.SetParent(GetCardVisualRoot(card), false);
            var chipRt = chipGo.GetComponent<RectTransform>();
            chipRt.anchorMin = new Vector2(0.5f, 0f);
            chipRt.anchorMax = new Vector2(0.5f, 0f);
            chipRt.pivot = new Vector2(0.5f, 0f);
            chipRt.anchoredPosition = new Vector2(0f, 76f); // just above the SEC button
            chipRt.sizeDelta = new Vector2(CardWidth - 20f, 22f);

            var chipImg = chipGo.AddComponent<Image>();
            chipImg.color = new Color(RimaUITheme.Cyan.r, RimaUITheme.Cyan.g, RimaUITheme.Cyan.b, 0.18f);
            chipImg.raycastTarget = false;

            var chipTxt = MakeTMP("ChainTxt", chipRt);
            var ctr = chipTxt.GetComponent<RectTransform>();
            ctr.anchorMin = Vector2.zero;
            ctr.anchorMax = Vector2.one;
            ctr.offsetMin = ctr.offsetMax = Vector2.zero;
            chipTxt.text = $"⟂ pairs with {partner.ToUpperInvariant()}";
            chipTxt.fontSize = 10f;
            chipTxt.fontStyle = FontStyles.Bold;
            chipTxt.color = RimaUITheme.Cyan;
            chipTxt.alignment = TextAlignmentOptions.Center;
            chipTxt.enableWordWrapping = false;
            chipTxt.overflowMode = TextOverflowModes.Ellipsis;
        }

        // ─── Shared Card Builder ────────────────────────────────────

        private GameObject BuildSkillCard(string skillName, SkillTier tier, string description, int index, int total, Sprite icon = null, Color? glowTintOverride = null, SkillData tooltipSkill = null)
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

            var cardGroup = cardGo.AddComponent<CanvasGroup>();
            cardGroup.alpha = 1f;
            cardGroup.interactable = true;
            cardGroup.blocksRaycasts = true;

            var hitboxImg = cardGo.AddComponent<Image>();
            hitboxImg.color = new Color(1f, 1f, 1f, 0f);
            hitboxImg.raycastTarget = true;

            var visualRootGo = new GameObject("VisualRoot", typeof(RectTransform));
            visualRootGo.transform.SetParent(cardGo.transform, false);
            var visualRoot = visualRootGo.GetComponent<RectTransform>();
            visualRoot.anchorMin = Vector2.zero;
            visualRoot.anchorMax = Vector2.one;
            visualRoot.offsetMin = visualRoot.offsetMax = Vector2.zero;
            visualRoot.localScale = Vector3.one;

            var glowGo = new GameObject("RarityGlow", typeof(RectTransform));
            glowGo.transform.SetParent(visualRoot, false);
            var glowRt = glowGo.GetComponent<RectTransform>();
            glowRt.anchorMin = new Vector2(0.5f, 0.5f);
            glowRt.anchorMax = new Vector2(0.5f, 0.5f);
            glowRt.pivot = new Vector2(0.5f, 0.5f);
            glowRt.anchoredPosition = Vector2.zero;
            glowRt.sizeDelta = new Vector2(CardWidth + 40f, CardHeight + 40f);
            glowRt.localScale = Vector3.one * IdleGlowScale;

            var glowImg = glowGo.AddComponent<Image>();
            glowImg.sprite = RimaUITheme.RarityGlow(tier);
            glowImg.type = Image.Type.Simple;
            Color glowTint = Color.white;
            glowImg.color = WithAlpha(glowTint, IdleGlowMinAlpha);
            glowImg.raycastTarget = false;

            var bgGo = new GameObject("Frame", typeof(RectTransform));
            bgGo.transform.SetParent(visualRoot, false);
            var bgRt = bgGo.GetComponent<RectTransform>();
            bgRt.anchorMin = Vector2.zero;
            bgRt.anchorMax = Vector2.one;
            bgRt.offsetMin = bgRt.offsetMax = Vector2.zero;

            var bgImg = bgGo.AddComponent<Image>();
            bgImg.sprite = RimaUITheme.DraftCardFrame;
            bgImg.type = Image.Type.Sliced;
            bgImg.color = bgImg.sprite != null ? Color.white : RimaUITheme.PanelTint;
            bgImg.raycastTarget = false;

            var shadeGo = new GameObject("InnerShade", typeof(RectTransform));
            shadeGo.transform.SetParent(visualRoot, false);
            var shadeRt = shadeGo.GetComponent<RectTransform>();
            shadeRt.anchorMin = Vector2.zero;
            shadeRt.anchorMax = Vector2.one;
            shadeRt.offsetMin = new Vector2(18f, 18f);
            shadeRt.offsetMax = new Vector2(-18f, -18f);

            var shadeImg = shadeGo.AddComponent<Image>();
            shadeImg.color = new Color(0.02f, 0.025f, 0.04f, 0.42f);
            shadeImg.raycastTarget = false;

            var selectFlashGo = new GameObject("SelectFlash", typeof(RectTransform));
            selectFlashGo.transform.SetParent(visualRoot, false);
            var selectFlashRt = selectFlashGo.GetComponent<RectTransform>();
            selectFlashRt.anchorMin = Vector2.zero;
            selectFlashRt.anchorMax = Vector2.one;
            selectFlashRt.offsetMin = selectFlashRt.offsetMax = Vector2.zero;

            var selectFlashImg = selectFlashGo.AddComponent<Image>();
            selectFlashImg.sprite = RimaUITheme.CardSelectFlash;
            selectFlashImg.type = Image.Type.Simple;
            selectFlashImg.color = Color.clear;
            selectFlashImg.raycastTarget = false;

            var juiceState = new CardJuiceState
            {
                Card = cardGo,
                Root = rt,
                VisualRoot = visualRoot,
                BaseVisualPosition = visualRoot.anchoredPosition,
                Glow = glowRt,
                GlowImage = glowImg,
                SelectFlash = selectFlashImg,
                CanvasGroup = cardGroup,
                GlowTint = glowTint,
                TooltipSkill = tooltipSkill,
                Phase = index,
                OriginalSiblingIndex = cardGo.transform.GetSiblingIndex()
            };
            cardJuiceStates.Add(juiceState);
            AttachCardJuiceHandler(cardGo, juiceState);
            juiceState.IdleGlow = StartCoroutine(PulseGlow(juiceState));

            // Icon placeholder (100x100 centered)
            var iconGo = new GameObject("Icon", typeof(RectTransform));
            iconGo.transform.SetParent(visualRoot, false);
            var iconRt = iconGo.GetComponent<RectTransform>();
            iconRt.anchorMin = new Vector2(0.5f, 1f);
            iconRt.anchorMax = new Vector2(0.5f, 1f);
            iconRt.pivot = new Vector2(0.5f, 1f);
            iconRt.anchoredPosition = new Vector2(0f, -56f);
            iconRt.sizeDelta = new Vector2(100f, 100f);

            var iconImg = iconGo.AddComponent<Image>();
            // Active skills carry their own icon; passives/relics fall back to a stable
            // on-brand icon from the generated passive pack (by skill-name hash).
            Sprite resolvedIcon = icon != null ? icon : RimaUITheme.PassiveIcon(skillName);
            if (resolvedIcon != null)
            {
                iconImg.sprite = resolvedIcon;
                iconImg.color = Color.white;
            }
            else
            {
                iconImg.color = new Color(0.15f, 0.15f, 0.2f, 0.8f); // placeholder
            }
            iconImg.raycastTarget = false;

            // Skill name
            var nameTmp = MakeTMP("Name", visualRoot);
            var nrt = nameTmp.GetComponent<RectTransform>();
            nrt.anchorMin = new Vector2(0f, 1f);
            nrt.anchorMax = new Vector2(1f, 1f);
            nrt.pivot = new Vector2(0.5f, 1f);
            nrt.anchoredPosition = new Vector2(0f, -178f);
            nrt.sizeDelta = new Vector2(-24f, 30f);
            nameTmp.text = skillName?.ToUpperInvariant() ?? "???";
            nameTmp.fontSize = 22f;
            nameTmp.fontStyle = FontStyles.Bold;
            nameTmp.color = Color.white;
            nameTmp.alignment = TextAlignmentOptions.Center;

            // Description
            var descTmp = MakeTMP("Desc", visualRoot);
            var drt = descTmp.GetComponent<RectTransform>();
            drt.anchorMin = new Vector2(0f, 1f);
            drt.anchorMax = new Vector2(1f, 1f);
            drt.pivot = new Vector2(0.5f, 1f);
            drt.anchoredPosition = new Vector2(0f, -220f);
            drt.sizeDelta = new Vector2(-32f, 72f);
            descTmp.text = description ?? "";
            descTmp.fontSize = 14f;
            descTmp.color = new Color(0.7f, 0.75f, 0.8f, 0.9f);
            descTmp.alignment = TextAlignmentOptions.Center;
            descTmp.enableWordWrapping = true;

            // Select button
            var btnGo = new GameObject("Btn", typeof(RectTransform));
            btnGo.transform.SetParent(visualRoot, false);
            var btnRt = btnGo.GetComponent<RectTransform>();
            btnRt.anchorMin = new Vector2(0.5f, 0f);
            btnRt.anchorMax = new Vector2(0.5f, 0f);
            btnRt.pivot = new Vector2(0.5f, 0f);
            btnRt.anchoredPosition = new Vector2(0f, 24f);
            btnRt.sizeDelta = new Vector2(160f, 40f);

            var btnImg = btnGo.AddComponent<Image>();
            btnImg.color = new Color(RimaUITheme.Cyan.r, RimaUITheme.Cyan.g, RimaUITheme.Cyan.b, 0.3f);
            juiceState.Button = btnGo.AddComponent<Button>();

            var btnTxt = MakeTMP("BtnLabel", btnRt);
            var btr = btnTxt.GetComponent<RectTransform>();
            btr.anchorMin = Vector2.zero;
            btr.anchorMax = Vector2.one;
            btr.offsetMin = btr.offsetMax = Vector2.zero;
            btnTxt.text = "SEC";
            btnTxt.fontSize = 15f;
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
            TooltipSystem.Instance?.Hide();
            StopAllCoroutines();

            CardJuiceState chosen = null;
            for (int i = 0; i < cardJuiceStates.Count; i++)
            {
                var state = cardJuiceStates[i];
                state.HoverTween = null;
                state.IdleGlow = null;
                state.IsHoverTweening = false;
                if (state.Button != null) state.Button.interactable = false;

                if (state.Card == chosenCard) chosen = state;
            }

            if (chosen == null)
            {
                onPick?.Invoke(offer, index);
                return;
            }

            chosen.Card.transform.SetAsLastSibling();
            StartCoroutine(ConfirmPickRoutine(chosen, offer, index));
        }

        private IEnumerator ConfirmPickRoutine(CardJuiceState chosen, RewardOffer offer, int index)
        {
            EnsureScreenFlash();
            StartCoroutine(ScreenFlashRoutine());
            SetSelectFlash(chosen, 0.85f);

            float anticipationTime = 0.06f;
            Vector3 startScale = chosen.VisualRoot.localScale;
            Vector3 squashScale = Vector3.one * 0.94f;
            float elapsed = 0f;
            while (elapsed < anticipationTime)
            {
                elapsed += Time.unscaledDeltaTime;
                float ease = EaseInQuad(Mathf.Clamp01(elapsed / anticipationTime));
                chosen.VisualRoot.localScale = Vector3.LerpUnclamped(startScale, squashScale, ease);
                SetGlow(chosen, HoverGlowAlpha, HoverGlowScale);
                yield return null;
            }

            Vector2 chosenStartPos = chosen.Root.anchoredPosition;
            Vector2 chosenStartVisualPos = chosen.VisualRoot.anchoredPosition;
            Vector3 chosenStartScale = chosen.VisualRoot.localScale;
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
                chosen.VisualRoot.anchoredPosition = Vector2.LerpUnclamped(chosenStartVisualPos, Vector2.zero, flyEase);
                chosen.VisualRoot.localScale = Vector3.LerpUnclamped(chosenStartScale, Vector3.one * 1.25f, flyEase);
                SetGlow(chosen, HoverGlowAlpha, HoverGlowScale);
                SetSelectFlash(chosen, Mathf.Lerp(0.85f, 0f, flyT));

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

            SetSelectFlash(chosen, 0f);
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

            if (entered)
            {
                ShowCardTooltip(state);
                PulseOwnedChainSlots(state);
            }
            else if (state.PointerInsideCount == 0)
            {
                TooltipSystem.Instance?.Hide();
            }
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
            if (shouldHover) state.HoverOrder = ++hoverSerial;
            RefreshAllHoverTweens();
        }

        private void RefreshAllHoverTweens()
        {
            CardJuiceState active = null;
            for (int i = 0; i < cardJuiceStates.Count; i++)
            {
                var state = cardJuiceStates[i];
                if (state.IsHovered && (active == null || state.HoverOrder > active.HoverOrder))
                {
                    active = state;
                }
            }

            for (int i = 0; i < cardJuiceStates.Count; i++)
            {
                var state = cardJuiceStates[i];
                bool hovered = state == active;
                bool dimmed = active != null && !hovered;

                if (state.HoverTween != null) StopCoroutine(state.HoverTween);
                state.HoverTween = StartCoroutine(TweenHover(
                    state,
                    hovered,
                    dimmed ? DimmedScale : hovered ? HoverScale : 1f,
                    dimmed ? DimmedAlpha : 1f,
                    hovered ? HoverLift : 0f));
            }
        }

        private IEnumerator TweenHover(CardJuiceState state, bool hover, float targetScale, float targetAlpha, float targetLift)
        {
            state.IsHoverTweening = true;
            if (hover && !state.IsRaised)
            {
                state.OriginalSiblingIndex = state.Card.transform.GetSiblingIndex();
                state.Card.transform.SetAsLastSibling();
                state.IsRaised = true;
            }

            float startScale = state.VisualRoot.localScale.x;
            float startAlpha = state.CanvasGroup.alpha;
            float startLift = state.VisualRoot.anchoredPosition.y - state.BaseVisualPosition.y;
            float startGlowAlpha = state.GlowImage.color.a;
            float targetGlowAlpha = hover ? HoverGlowAlpha : IdleGlowMinAlpha;
            float startGlowScale = state.Glow.localScale.x;
            float targetGlowScale = hover ? HoverGlowScale : IdleGlowScale;

            float elapsed = 0f;
            while (elapsed < HoverDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float ease = EaseOutCubic(Mathf.Clamp01(elapsed / HoverDuration));
                state.VisualRoot.localScale = Vector3.one * Mathf.LerpUnclamped(startScale, targetScale, ease);
                state.VisualRoot.anchoredPosition = state.BaseVisualPosition + new Vector2(0f, Mathf.LerpUnclamped(startLift, targetLift, ease));
                state.CanvasGroup.alpha = Mathf.LerpUnclamped(startAlpha, targetAlpha, ease);
                SetGlow(
                    state,
                    Mathf.LerpUnclamped(startGlowAlpha, targetGlowAlpha, ease),
                    Mathf.LerpUnclamped(startGlowScale, targetGlowScale, ease));
                yield return null;
            }

            state.VisualRoot.localScale = Vector3.one * targetScale;
            state.VisualRoot.anchoredPosition = state.BaseVisualPosition + new Vector2(0f, targetLift);
            state.CanvasGroup.alpha = targetAlpha;
            SetGlow(state, targetGlowAlpha, targetGlowScale);
            if (!hover && state.IsRaised)
            {
                state.Card.transform.SetSiblingIndex(Mathf.Min(state.OriginalSiblingIndex, cardContainer.childCount - 1));
                state.IsRaised = false;
            }

            state.IsHoverTweening = false;
            state.HoverTween = null;
        }

        // ─── Helpers ────────────────────────────────────────────────

        private void ClearCards()
        {
            TooltipSystem.Instance?.Hide();
            StopAllCoroutines();
            foreach (var c in cards)
                if (c != null) Destroy(c);
            cards.Clear();
            cardJuiceStates.Clear();
            currentOfferNames.Clear();
            isConfirmingPick = false;
            hoverSerial = 0;
            SetScreenFlashAlpha(0f);
        }

        private void ShowCardTooltip(CardJuiceState state)
        {
            if (state?.TooltipSkill == null) return;
            EnsureTooltipSystem();
            TooltipSystem.Instance?.Show(TooltipSystem.FormatSkill(state.TooltipSkill));
        }

        private void PulseOwnedChainSlots(CardJuiceState state)
        {
            string skillName = state?.TooltipSkill?.skillName;
            if (string.IsNullOrEmpty(skillName)) return;

            var owned = DraftManager.Instance?.OwnedActiveSkillNames;
            if (owned == null) return;

            if (skillBar == null) skillBar = FindObjectOfType<SkillBarUI>();
            if (skillBar == null) return;

            for (int i = 0; i < owned.Count; i++)
            {
                string other = owned[i];
                if (string.IsNullOrEmpty(other) || other == skillName) continue;
                if (Chains(skillName, other)) skillBar.PulseSynergySlot(other);
            }
        }

        private void EnsureTooltipSystem()
        {
            if (TooltipSystem.Instance != null) return;
            gameObject.AddComponent<TooltipSystem>();
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

        private static void SetSelectFlash(CardJuiceState state, float alpha)
        {
            if (state.SelectFlash == null) return;
            state.SelectFlash.color = WithAlpha(Color.white, alpha);
        }

        private static Color WithAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        private static RectTransform GetCardVisualRoot(GameObject card)
        {
            return card.transform.Find("VisualRoot") as RectTransform ?? card.transform as RectTransform;
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
            public RectTransform VisualRoot;
            public Vector2 BaseVisualPosition;
            public RectTransform Glow;
            public Image GlowImage;
            public Image SelectFlash;
            public CanvasGroup CanvasGroup;
            public Button Button;
            public SkillData TooltipSkill;
            public Coroutine HoverTween;
            public Coroutine IdleGlow;
            public Color GlowTint;
            public float Phase;
            public int PointerInsideCount;
            public bool Selected;
            public bool IsHovered;
            public bool IsHoverTweening;
            public bool IsRaised;
            public int OriginalSiblingIndex;
            public int HoverOrder;
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
