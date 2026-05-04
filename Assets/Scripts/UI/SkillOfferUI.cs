using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// Oda sonrası ödül seçim ekranı.
    ///
    /// Layout:
    ///   Sol 72% — 3 yatay kompakt kart (icon + isim + tier badge + AL butonu)
    ///   Sağ 26% — tooltip panel (hover ile dolar, detay gösterir)
    ///
    /// Reward tipleri: Skill (aktif/pasif), Gold, Heal
    /// Tier renkler: Common/Rare/Epic/Mythic/Legendary
    /// Replace modu: slot dolu olduğunda mevcut skilllerden birini çıkar.
    /// </summary>
    public class SkillOfferUI : MonoBehaviour
    {
        [Header("UI Refs (Inspector'dan bağlanabilir)")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Transform  cardContainer;

        [Header("Tier Colors")]
        [SerializeField] private Color commonColor    = new Color(0.50f, 0.55f, 0.62f);
        [SerializeField] private Color rareColor      = new Color(0.22f, 0.52f, 0.90f);
        [SerializeField] private Color epicColor      = new Color(0.52f, 0.25f, 0.88f);
        [SerializeField] private Color mythicColor    = new Color(0.85f, 0.25f, 0.45f);
        [SerializeField] private Color legendaryColor = new Color(0.92f, 0.70f, 0.18f);
        [SerializeField] private Color goldColor      = new Color(0.92f, 0.75f, 0.20f);
        [SerializeField] private Color healColor      = new Color(0.28f, 0.78f, 0.45f);

        // Callback
        private Action<RewardOffer, int> onPick;

        // Card tracking (destroy on hide)
        private readonly List<GameObject> cards = new List<GameObject>();

        // Tooltip refs
        private GameObject      tooltipPanel;
        private Image           tooltipStrip;
        private TextMeshProUGUI tooltipName;
        private TextMeshProUGUI tooltipBadge;
        private TextMeshProUGUI tooltipDesc;
        private TextMeshProUGUI tooltipStats;

        // ── Lifecycle ────────────────────────────────────────────

        private void Awake()
        {
            if (panel != null) panel.SetActive(false);
        }

        // ── Public API ───────────────────────────────────────────

        /// <summary>Oda ödülü: 3 kart göster.</summary>
        public void Show(List<RewardOffer> offers, Action<RewardOffer, int> callback, int roomNumber = 0)
        {
            onPick = callback;
            ClearCards();

            if (panel == null) BuildRuntimePanel();
            else               panel.SetActive(true);

            if (cardContainer == null)
                cardContainer = BuildCardContainer(panel.transform);

            if (tooltipPanel == null)
                BuildTooltipPanel(panel.transform);

            string title = roomNumber > 0 ? $"ODA {roomNumber}  —  ÖDÜL SEÇ" : "ÖDÜL SEÇ";
            SetTitle(title);
            SetSubtitle("Birini seç — diğerleri kaybolur");

            ShowTooltipDefault();

            foreach (var offer in offers)
                BuildRewardCard(offer);

            Time.timeScale = 0f;
        }

        /// <summary>Slot dolu replace modu.</summary>
        public void ShowReplaceMode(List<SkillData> currentActives, SkillData incoming,
                                    Action<SkillData> onReplace, Action onSkip)
        {
            ClearCards();

            if (panel == null) BuildRuntimePanel();
            else               panel.SetActive(true);

            if (cardContainer == null)
                cardContainer = BuildCardContainer(panel.transform);

            if (tooltipPanel == null)
                BuildTooltipPanel(panel.transform);

            SetTitle("SLOT DOLU");
            SetSubtitle($"{incoming.skillName.ToUpper()} almak için hangisini bırakmak istiyorsun?");
            ShowTooltipDefault();

            foreach (var sd in currentActives)
                cards.Add(BuildReplaceCard(sd, onReplace));

            // ATLA butonu
            var skipGo = MakeAnchored("SkipBtn", panel.transform,
                new Vector2(0.30f, 0.04f), new Vector2(0.60f, 0.13f));
            var skipBg = skipGo.AddComponent<Image>();
            skipBg.color = new Color(0.10f, 0.10f, 0.13f, 0.92f);
            var skipBtn = skipGo.AddComponent<Button>();
            var sc = skipBtn.colors;
            sc.normalColor = new Color(0.10f, 0.10f, 0.13f, 0.92f);
            sc.highlightedColor = new Color(0.20f, 0.20f, 0.24f, 1f);
            skipBtn.colors = sc;
            skipBtn.onClick.AddListener(() => { onSkip?.Invoke(); });
            var skipLbl = MakeAnchored("T", skipGo.transform, Vector2.zero, Vector2.one);
            var st = skipLbl.AddComponent<TextMeshProUGUI>();
            st.text = "ATLA — alma";
            st.fontSize = 12; st.fontStyle = FontStyles.Bold;
            st.color = new Color(0.48f, 0.48f, 0.55f);
            st.alignment = TextAlignmentOptions.Center;
            st.raycastTarget = false;
            cards.Add(skipGo);

            Time.timeScale = 0f;
        }

        public void Hide()
        {
            ClearCards();
            if (panel != null)        panel.SetActive(false);
            if (tooltipPanel != null) tooltipPanel.SetActive(false);
            Time.timeScale = 1f;
        }

        // ── Reward card build ────────────────────────────────────

        private void BuildRewardCard(RewardOffer offer)
        {
            Color tc = TypeColor(offer);
            var card = MakeRect("Card_" + offer.DisplayName, cardContainer);
            card.AddComponent<LayoutElement>();

            var bg = card.AddComponent<Image>();
            bg.sprite = RimaUITheme.SmallPanelFrame;
            bg.color = new Color(1f, 1f, 1f, 0.95f);

            // Top tier strip
            MakeAnchored("Strip", card.transform, new Vector2(0f, 0.93f), Vector2.one)
                .AddComponent<Image>().color = tc;

            // Icon area (40–92%)
            var iconBg = MakeAnchored("IconBg", card.transform,
                new Vector2(0.10f, 0.40f), new Vector2(0.90f, 0.92f));
            iconBg.AddComponent<Image>().color = new Color(tc.r * 0.15f, tc.g * 0.15f, tc.b * 0.20f, 1f);

            if (offer.type == RewardType.Gold)
            {
                var amtGo = MakeAnchored("Amt", iconBg.transform, Vector2.zero, Vector2.one);
                var atmp = amtGo.AddComponent<TextMeshProUGUI>();
                atmp.text = $"+{offer.goldAmount}"; atmp.fontSize = 26;
                atmp.fontStyle = FontStyles.Bold; atmp.color = goldColor;
                atmp.alignment = TextAlignmentOptions.Center; atmp.raycastTarget = false;
            }
            else if (offer.type == RewardType.Heal)
            {
                var amtGo = MakeAnchored("Amt", iconBg.transform, Vector2.zero, Vector2.one);
                var atmp = amtGo.AddComponent<TextMeshProUGUI>();
                atmp.text = $"+%{offer.healPercent}"; atmp.fontSize = 22;
                atmp.fontStyle = FontStyles.Bold; atmp.color = healColor;
                atmp.alignment = TextAlignmentOptions.Center; atmp.raycastTarget = false;
            }
            else if (offer.skill?.icon != null)
            {
                var iconGo = MakeAnchored("Icon", iconBg.transform, new Vector2(0.05f, 0.05f), new Vector2(0.95f, 0.95f));
                var ii = iconGo.AddComponent<Image>();
                ii.sprite = offer.skill.icon; ii.preserveAspect = true; ii.raycastTarget = false;
            }

            // Separator
            MakeAnchored("Sep", card.transform, new Vector2(0.05f, 0.395f), new Vector2(0.95f, 0.405f))
                .AddComponent<Image>().color = new Color(tc.r, tc.g, tc.b, 0.30f);

            // Name (25–39%)
            var nameGo = MakeAnchored("Name", card.transform, new Vector2(0.04f, 0.25f), new Vector2(0.96f, 0.39f));
            var nameTmp = nameGo.AddComponent<TextMeshProUGUI>();
            nameTmp.text = offer.DisplayName.ToUpper();
            nameTmp.fontSize = 11; nameTmp.fontStyle = FontStyles.Bold;
            nameTmp.color = Color.white; nameTmp.alignment = TextAlignmentOptions.Center;
            nameTmp.enableWordWrapping = true; nameTmp.raycastTarget = false;

            // Type / tier badge (14–24%)
            var badgeGo = MakeAnchored("Badge", card.transform, new Vector2(0.04f, 0.14f), new Vector2(0.96f, 0.24f));
            var badgeTmp = badgeGo.AddComponent<TextMeshProUGUI>();
            badgeTmp.text = BadgeText(offer);
            badgeTmp.fontSize = 8; badgeTmp.fontStyle = FontStyles.Bold;
            badgeTmp.color = new Color(tc.r, tc.g, tc.b, 0.88f);
            badgeTmp.alignment = TextAlignmentOptions.Center;
            badgeTmp.enableWordWrapping = true; badgeTmp.raycastTarget = false;

            // Pick button (3–12%)
            var btnGo = MakeAnchored("Btn", card.transform, new Vector2(0.07f, 0.03f), new Vector2(0.93f, 0.12f));
            var btnBg = btnGo.AddComponent<Image>();
            btnBg.color = new Color(tc.r * 0.30f, tc.g * 0.30f, tc.b * 0.38f, 1f);
            var btn = btnGo.AddComponent<Button>();
            var bc = btn.colors;
            bc.normalColor      = new Color(tc.r * 0.30f, tc.g * 0.30f, tc.b * 0.38f, 1f);
            bc.highlightedColor = new Color(tc.r * 0.55f, tc.g * 0.55f, tc.b * 0.65f, 1f);
            bc.pressedColor     = new Color(tc.r * 0.80f, tc.g * 0.80f, tc.b * 0.88f, 1f);
            btn.colors = bc;
            var cap = offer;
            btn.onClick.AddListener(() => onPick?.Invoke(cap, 0));

            var lblGo = MakeAnchored("Lbl", btnGo.transform, Vector2.zero, Vector2.one);
            var lblTmp = lblGo.AddComponent<TextMeshProUGUI>();
            lblTmp.text = PickLabel(offer);
            lblTmp.color = PickLabelColor(offer);
            lblTmp.fontSize = 11; lblTmp.fontStyle = FontStyles.Bold;
            lblTmp.alignment = TextAlignmentOptions.Center; lblTmp.raycastTarget = false;

            // Hover detection
            var hover = card.AddComponent<CardHover>();
            hover.Offer   = offer;
            hover.OnEnter = OnCardHovered;
            hover.OnExit  = ShowTooltipDefault;

            cards.Add(card);
        }

        // ── Replace mode card ────────────────────────────────────

        private GameObject BuildReplaceCard(SkillData skill, Action<SkillData> onReplace)
        {
            Color tc = TierColor(skill.tier);
            var card = MakeRect("RepCard_" + skill.skillName, cardContainer);
            card.AddComponent<LayoutElement>();
            var cardImage = card.AddComponent<Image>();
            cardImage.sprite = RimaUITheme.SmallPanelFrame;
            cardImage.color = new Color(1f, 1f, 1f, 0.95f);

            MakeAnchored("Strip", card.transform, new Vector2(0f, 0.93f), Vector2.one)
                .AddComponent<Image>().color = tc;

            var nameGo = MakeAnchored("Name", card.transform, new Vector2(0.04f, 0.55f), new Vector2(0.96f, 0.92f));
            var nTmp = nameGo.AddComponent<TextMeshProUGUI>();
            nTmp.text = skill.skillName.ToUpper(); nTmp.fontSize = 11;
            nTmp.fontStyle = FontStyles.Bold; nTmp.color = Color.white;
            nTmp.alignment = TextAlignmentOptions.Center;
            nTmp.enableWordWrapping = true; nTmp.raycastTarget = false;

            var descGo = MakeAnchored("Desc", card.transform, new Vector2(0.04f, 0.18f), new Vector2(0.96f, 0.54f));
            var dTmp = descGo.AddComponent<TextMeshProUGUI>();
            dTmp.text = skill.description; dTmp.fontSize = 9;
            dTmp.color = new Color(0.65f, 0.68f, 0.80f);
            dTmp.alignment = TextAlignmentOptions.Center;
            dTmp.enableWordWrapping = true; dTmp.raycastTarget = false;

            var btnGo = MakeAnchored("Btn", card.transform, new Vector2(0.06f, 0.03f), new Vector2(0.94f, 0.15f));
            btnGo.AddComponent<Image>().color = new Color(0.38f, 0.07f, 0.07f, 0.95f);
            var btn = btnGo.AddComponent<Button>();
            var bc = btn.colors;
            bc.normalColor      = new Color(0.38f, 0.07f, 0.07f, 0.95f);
            bc.highlightedColor = new Color(0.65f, 0.12f, 0.12f, 1f);
            bc.pressedColor     = new Color(0.85f, 0.18f, 0.18f, 1f);
            btn.colors = bc;
            var cap = skill;
            btn.onClick.AddListener(() => { onReplace?.Invoke(cap); Hide(); });

            var lblGo = MakeAnchored("Lbl", btnGo.transform, Vector2.zero, Vector2.one);
            var lTmp = lblGo.AddComponent<TextMeshProUGUI>();
            lTmp.text = "ÇIKAR"; lTmp.fontSize = 11;
            lTmp.fontStyle = FontStyles.Bold;
            lTmp.color = new Color(0.95f, 0.62f, 0.62f);
            lTmp.alignment = TextAlignmentOptions.Center; lTmp.raycastTarget = false;

            // Hover
            var hover = card.AddComponent<CardHover>();
            hover.Offer   = RewardOffer.FromSkill(skill);
            hover.OnEnter = OnCardHovered;
            hover.OnExit  = ShowTooltipDefault;

            return card;
        }

        // ── Tooltip ──────────────────────────────────────────────

        private void OnCardHovered(RewardOffer offer)
        {
            if (tooltipPanel == null) return;
            tooltipPanel.SetActive(true);

            Color tc = TypeColor(offer);

            if (tooltipStrip != null)  tooltipStrip.color = tc;
            if (tooltipName  != null)
            {
                tooltipName.text  = offer.DisplayName.ToUpper();
                tooltipName.color = tc;
            }
            if (tooltipBadge != null)
            {
                tooltipBadge.text  = BadgeText(offer);
                tooltipBadge.color = new Color(tc.r, tc.g, tc.b, 0.85f);
            }
            if (tooltipDesc != null)
            {
                tooltipDesc.text  = offer.Description;
                tooltipDesc.color = new Color(0.75f, 0.78f, 0.90f);
            }
            if (tooltipStats != null)
            {
                string stats = "";
                if (offer.type == RewardType.Skill && offer.skill != null && !offer.skill.isPassive)
                {
                    if (offer.skill.cooldown > 0) stats += $"CD: {offer.skill.cooldown}s";
                    if (offer.skill.damage   > 0) stats += (stats.Length > 0 ? "   " : "") + $"Hasar: {offer.skill.damage}";
                }
                tooltipStats.text = stats;
                tooltipStats.gameObject.SetActive(stats.Length > 0);
            }
        }

        private void ShowTooltipDefault()
        {
            if (tooltipPanel == null) return;
            tooltipPanel.SetActive(true);
            if (tooltipStrip  != null) tooltipStrip.color  = new Color(0.20f, 0.20f, 0.26f);
            if (tooltipName   != null) { tooltipName.text  = "ÖDÜL DETAYI"; tooltipName.color = new Color(0.48f, 0.50f, 0.62f); }
            if (tooltipBadge  != null)   tooltipBadge.text  = "";
            if (tooltipDesc   != null) { tooltipDesc.text   = "Bir kartın üstüne gel — detaylar burada görünür."; tooltipDesc.color = new Color(0.38f, 0.40f, 0.50f); }
            if (tooltipStats  != null) { tooltipStats.text  = ""; tooltipStats.gameObject.SetActive(false); }
        }

        private void BuildTooltipPanel(Transform parent)
        {
            // Right side: 74%–98% x, 10%–90% y
            tooltipPanel = MakeAnchored("TooltipPanel", parent,
                new Vector2(0.74f, 0.10f), new Vector2(0.98f, 0.90f));
            var tooltipImage = tooltipPanel.AddComponent<Image>();
            tooltipImage.sprite = RimaUITheme.SmallPanelFrame;
            tooltipImage.color = new Color(1f, 1f, 1f, 0.92f);

            // Top strip
            var stripGo = MakeAnchored("Strip", tooltipPanel.transform,
                new Vector2(0f, 0.96f), Vector2.one);
            tooltipStrip = stripGo.AddComponent<Image>();
            tooltipStrip.color = new Color(0.20f, 0.20f, 0.26f);

            // Name (big)
            var nameGo = MakeAnchored("Name", tooltipPanel.transform,
                new Vector2(0.06f, 0.82f), new Vector2(0.94f, 0.95f));
            tooltipName = nameGo.AddComponent<TextMeshProUGUI>();
            tooltipName.fontSize = 16; tooltipName.fontStyle = FontStyles.Bold;
            tooltipName.color = new Color(0.48f, 0.50f, 0.62f);
            tooltipName.alignment = TextAlignmentOptions.Left;
            tooltipName.enableWordWrapping = true; tooltipName.raycastTarget = false;

            // Badge row
            var badgeGo = MakeAnchored("Badge", tooltipPanel.transform,
                new Vector2(0.06f, 0.75f), new Vector2(0.94f, 0.82f));
            tooltipBadge = badgeGo.AddComponent<TextMeshProUGUI>();
            tooltipBadge.fontSize = 9; tooltipBadge.fontStyle = FontStyles.Bold;
            tooltipBadge.color = new Color(0.45f, 0.48f, 0.60f);
            tooltipBadge.alignment = TextAlignmentOptions.Left;
            tooltipBadge.raycastTarget = false;

            // Separator
            MakeAnchored("Sep", tooltipPanel.transform, new Vector2(0.04f, 0.742f), new Vector2(0.96f, 0.750f))
                .AddComponent<Image>().color = new Color(0.28f, 0.28f, 0.34f, 0.80f);

            // Description (20–74%)
            var descGo = MakeAnchored("Desc", tooltipPanel.transform,
                new Vector2(0.06f, 0.20f), new Vector2(0.94f, 0.74f));
            tooltipDesc = descGo.AddComponent<TextMeshProUGUI>();
            tooltipDesc.text = "Bir kartın üstüne gel — detaylar burada görünür.";
            tooltipDesc.fontSize = 11;
            tooltipDesc.color = new Color(0.38f, 0.40f, 0.50f);
            tooltipDesc.alignment = TextAlignmentOptions.TopLeft;
            tooltipDesc.enableWordWrapping = true; tooltipDesc.raycastTarget = false;

            // Stats (bottom)
            var statsGo = MakeAnchored("Stats", tooltipPanel.transform,
                new Vector2(0.06f, 0.04f), new Vector2(0.94f, 0.19f));
            tooltipStats = statsGo.AddComponent<TextMeshProUGUI>();
            tooltipStats.fontSize = 10; tooltipStats.fontStyle = FontStyles.Bold;
            tooltipStats.color = new Color(0.72f, 0.76f, 0.88f);
            tooltipStats.alignment = TextAlignmentOptions.BottomLeft;
            tooltipStats.raycastTarget = false;
        }

        // ── Panel / container helpers ────────────────────────────

        private void SetTitle(string text)
        {
            var ex = panel?.transform.Find("Title");
            if (ex != null) { ex.GetComponent<TextMeshProUGUI>().text = text; return; }

            var go = MakeAnchored("Title", panel.transform, new Vector2(0.02f, 0.87f), new Vector2(0.72f, 0.97f));
            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.text = text; tmp.fontSize = 21; tmp.fontStyle = FontStyles.Bold;
            tmp.color = new Color(0.90f, 0.92f, 1f);
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.enableWordWrapping = true; tmp.raycastTarget = false;
        }

        private void SetSubtitle(string text)
        {
            var ex = panel?.transform.Find("Subtitle");
            if (ex != null) { ex.GetComponent<TextMeshProUGUI>().text = text; return; }

            var go = MakeAnchored("Subtitle", panel.transform, new Vector2(0.02f, 0.81f), new Vector2(0.72f, 0.88f));
            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.text = text; tmp.fontSize = 10;
            tmp.color = new Color(0.42f, 0.44f, 0.56f);
            tmp.alignment = TextAlignmentOptions.Center; tmp.raycastTarget = false;
        }

        private void BuildRuntimePanel()
        {
            var canvas = GetComponentInParent<Canvas>() ?? FindObjectOfType<Canvas>();
            if (canvas == null) return;

            panel = new GameObject("RewardPanel", typeof(RectTransform));
            panel.transform.SetParent(canvas.transform, false);
            var rt = panel.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
            rt.offsetMin = rt.offsetMax = Vector2.zero;
            var bg = panel.AddComponent<Image>();
            bg.sprite = RimaUITheme.MenuDungeonBackground;
            bg.color = new Color(0.70f, 0.78f, 0.82f, 0.95f);
            bg.preserveAspect = true;

            MakeAnchored("Shade", panel.transform, Vector2.zero, Vector2.one)
                .AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.58f);
        }

        private static Transform BuildCardContainer(Transform parent)
        {
            var go = new GameObject("CardContainer", typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.12f, 0.22f);
            rt.anchorMax = new Vector2(0.68f, 0.72f);
            rt.offsetMin = rt.offsetMax = Vector2.zero;
            var hg = go.AddComponent<HorizontalLayoutGroup>();
            hg.spacing               = 16;
            hg.childAlignment        = TextAnchor.MiddleCenter;
            hg.childForceExpandWidth = true;
            hg.childForceExpandHeight = true;
            hg.padding = new RectOffset(6, 6, 0, 0);
            return go.transform;
        }

        private void ClearCards()
        {
            foreach (var c in cards) if (c) Destroy(c);
            cards.Clear();
            if (cardContainer != null)
                foreach (Transform ch in cardContainer) Destroy(ch.gameObject);
        }

        // ── UI Utility ───────────────────────────────────────────

        private static GameObject MakeRect(string name, Transform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return go;
        }

        private static GameObject MakeAnchored(string name, Transform parent,
                                                Vector2 ancMin, Vector2 ancMax)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = ancMin; rt.anchorMax = ancMax;
            rt.offsetMin = rt.offsetMax = Vector2.zero;
            return go;
        }

        // ── Color helpers ────────────────────────────────────────

        private Color TypeColor(RewardOffer offer) => offer.type switch
        {
            RewardType.Gold => goldColor,
            RewardType.Heal => healColor,
            _               => TierColor(offer.Tier)
        };

        private Color TierColor(SkillTier t) => t switch
        {
            SkillTier.Rare      => rareColor,
            SkillTier.Epic      => epicColor,
            SkillTier.Mythic    => mythicColor,
            SkillTier.Legendary => legendaryColor,
            _                   => commonColor,
        };

        private static Color CardBg(Color tc) =>
            new Color(tc.r * 0.09f, tc.g * 0.09f, tc.b * 0.13f, 0.98f);

        // ── Label helpers ────────────────────────────────────────

        private static string BadgeText(RewardOffer offer)
        {
            if (offer.type == RewardType.Gold) return "PARA ÖDÜLÜ";
            if (offer.type == RewardType.Heal) return "İYİLEŞME";

            string passive = offer.IsPassive ? "PASİF  ·  " : "AKTİF  ·  ";
            string tier    = offer.Tier.ToString().ToUpper();
            string cls     = offer.ClassType != ClassType.None
                ? "  ·  " + offer.ClassType.ToString().ToUpper()
                : "  ·  NEUTRAL";
            return passive + tier + cls;
        }

        private static string PickLabel(RewardOffer offer)
        {
            if (offer.type != RewardType.Skill) return "AL";
            if (!offer.IsPassive) return "AL";
            int lvl = DraftManager.GetPassiveLevel(offer.skill.skillName);
            return lvl > 0 ? $"YÜKSELT ({lvl}/{PassiveBase.MaxLevel})" : "AL";
        }

        private static Color PickLabelColor(RewardOffer offer)
        {
            if (offer.type != RewardType.Skill || !offer.IsPassive) return Color.white;
            int lvl = DraftManager.GetPassiveLevel(offer.skill.skillName);
            return lvl > 0 ? new Color(0.95f, 0.80f, 0.25f) : Color.white;
        }

        // ── CardHover nested component ───────────────────────────

        private class CardHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
        {
            public RewardOffer     Offer;
            public Action<RewardOffer> OnEnter;
            public Action          OnExit;

            public void OnPointerEnter(PointerEventData e) => OnEnter?.Invoke(Offer);
            public void OnPointerExit(PointerEventData e)  => OnExit?.Invoke();
        }
    }
}
