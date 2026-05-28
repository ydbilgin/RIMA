using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        private GameObject panel;
        private Transform cardContainer;
        private TextMeshProUGUI titleLabel;
        private TextMeshProUGUI subtitleLabel;

        // ── Layout constants ─────────────────────────────────────────
        private const float CardWidth  = 180f;
        private const float CardHeight = 260f;
        private const float CardGap    = 20f;
        private const float SlideInDuration = 0.3f;
        private const float StaggerDelay    = 0.08f;

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
            else
            {
                name = offer.skill?.skillName?.ToUpperInvariant() ?? "???";
                desc = offer.skill?.description ?? "";
                tierTag = offer.skill?.tier.ToString().ToUpperInvariant() ?? "";
            }

            var card = BuildSkillCard(name, offer.skill?.tier ?? SkillTier.Common, desc, index, total, offer.skill?.icon);

            // Select button
            var btn = card.GetComponentInChildren<Button>();
            if (btn != null)
            {
                int idx = index;
                var captured = offer;
                btn.onClick.AddListener(() =>
                {
                    onPick?.Invoke(captured, idx);
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
        }

        // ─── Shared Card Builder ────────────────────────────────────

        private GameObject BuildSkillCard(string skillName, SkillTier tier, string description, int index, int total, Sprite icon = null)
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

            // Card background (dark stone)
            var bgImg = cardGo.AddComponent<Image>();
            bgImg.sprite = RimaUITheme.SmallPanelFrame;
            bgImg.color = RimaUITheme.PanelTint;

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
            btnGo.AddComponent<Button>();

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

        // ─── Helpers ────────────────────────────────────────────────

        private void ClearCards()
        {
            StopAllCoroutines();
            foreach (var c in cards)
                if (c != null) Destroy(c);
            cards.Clear();
        }

        private Color TierColor(RewardOffer offer)
        {
            if (offer.type == RewardType.Gold) return RimaUITheme.Gold;
            if (offer.type == RewardType.Heal) return new Color(0.28f, 0.78f, 0.45f);
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
