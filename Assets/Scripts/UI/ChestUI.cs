using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// Sandık açılınca gösterilir. 3 seçenek: İyileşme / Altın / Skill.
    /// ChestBehavior.Open() → ChestUI.Instance.Show(offers, onPicked)
    /// </summary>
    public class ChestUI : MonoBehaviour
    {
        public static ChestUI Instance { get; private set; }

        private GameObject panel;
        private Transform  cardContainer;
        private readonly List<GameObject> cards = new List<GameObject>();

        private readonly Color healColor = new Color(0.28f, 0.78f, 0.45f);
        private readonly Color goldColor = new Color(0.92f, 0.75f, 0.20f);
        private readonly Color skillCol  = new Color(0.22f, 0.52f, 0.90f);

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start() => BuildPanel();

        // ── Public ───────────────────────────────────────────────

        public void Show(List<RewardOffer> offers, Action<RewardOffer> onPicked)
        {
            ClearCards();
            panel?.SetActive(true);

            foreach (var offer in offers)
                BuildCard(offer, onPicked);

            UIManager.Instance.PauseForMenu();
        }

        public void Hide()
        {
            ClearCards();
            panel?.SetActive(false);
            UIManager.Instance.ResumeFromMenu();
        }

        // ── Build ────────────────────────────────────────────────

        private void BuildPanel()
        {
            var canvas = FindObjectOfType<Canvas>();
            if (canvas == null) return;

            panel = MakeAnchored("ChestPanel", canvas.transform,
                new Vector2(0.15f, 0.25f), new Vector2(0.85f, 0.75f));
            panel.AddComponent<Image>().color = new Color(0.04f, 0.04f, 0.08f, 0.96f);

            // Header
            var hdr = MakeAnchored("Hdr", panel.transform, new Vector2(0f, 0.82f), Vector2.one);
            hdr.AddComponent<Image>().color = new Color(0.12f, 0.10f, 0.03f, 1f);
            var ht = MakeAnchored("T", hdr.transform, Vector2.zero, Vector2.one).AddComponent<TextMeshProUGUI>();
            ht.text = "SANDIK ÖDÜLÜ — BİRİNİ SEÇ"; ht.fontSize = 16; ht.fontStyle = FontStyles.Bold;
            ht.color = new Color(0.92f, 0.80f, 0.35f); ht.alignment = TextAlignmentOptions.Center;
            ht.raycastTarget = false;

            // Card container
            var cc = MakeAnchored("Cards", panel.transform,
                new Vector2(0.04f, 0.08f), new Vector2(0.96f, 0.80f));
            var hg = cc.AddComponent<HorizontalLayoutGroup>();
            hg.spacing = 14; hg.childAlignment = TextAnchor.MiddleCenter;
            hg.childForceExpandWidth = true; hg.childForceExpandHeight = true;
            hg.padding = new RectOffset(4, 4, 0, 0);
            cardContainer = cc.transform;

            panel.SetActive(false);
        }

        private void BuildCard(RewardOffer offer, Action<RewardOffer> onPicked)
        {
            Color tc = offer.type == RewardType.Heal ? healColor
                     : offer.type == RewardType.Gold ? goldColor
                     : skillCol;

            var card = new GameObject("Card_" + offer.DisplayName, typeof(RectTransform));
            card.transform.SetParent(cardContainer, false);
            card.AddComponent<LayoutElement>();
            card.AddComponent<Image>().color = new Color(tc.r * 0.10f, tc.g * 0.10f, tc.b * 0.14f, 0.98f);

            MakeAnchored("Strip", card.transform, new Vector2(0f, 0.93f), Vector2.one)
                .AddComponent<Image>().color = tc;

            // Center label
            var lbl = MakeAnchored("Lbl", card.transform, new Vector2(0.04f, 0.35f), new Vector2(0.96f, 0.92f));
            var lt = lbl.AddComponent<TextMeshProUGUI>();
            lt.text = offer.DisplayName.ToUpper(); lt.fontSize = 14; lt.fontStyle = FontStyles.Bold;
            lt.color = Color.white; lt.alignment = TextAlignmentOptions.Center;
            lt.enableWordWrapping = true; lt.raycastTarget = false;

            var desc = MakeAnchored("Desc", card.transform, new Vector2(0.04f, 0.18f), new Vector2(0.96f, 0.34f));
            var dt = desc.AddComponent<TextMeshProUGUI>();
            dt.text = offer.Description; dt.fontSize = 9;
            dt.color = new Color(0.65f, 0.68f, 0.80f); dt.alignment = TextAlignmentOptions.Center;
            dt.enableWordWrapping = true; dt.raycastTarget = false;

            var btn = MakeAnchored("Btn", card.transform, new Vector2(0.08f, 0.04f), new Vector2(0.92f, 0.15f));
            btn.AddComponent<Image>().color = new Color(tc.r * 0.30f, tc.g * 0.30f, tc.b * 0.38f, 1f);
            var b = btn.AddComponent<Button>();
            var bc = b.colors;
            bc.normalColor      = new Color(tc.r * 0.30f, tc.g * 0.30f, tc.b * 0.38f, 1f);
            bc.highlightedColor = new Color(tc.r * 0.55f, tc.g * 0.55f, tc.b * 0.65f, 1f);
            b.colors = bc;
            var cap = offer;
            b.onClick.AddListener(() => { onPicked?.Invoke(cap); Hide(); });

            var bl = MakeAnchored("T", btn.transform, Vector2.zero, Vector2.one).AddComponent<TextMeshProUGUI>();
            bl.text = "AL"; bl.fontSize = 11; bl.fontStyle = FontStyles.Bold;
            bl.color = Color.white; bl.alignment = TextAlignmentOptions.Center; bl.raycastTarget = false;

            cards.Add(card);
        }

        private void ClearCards()
        {
            foreach (var c in cards) if (c) Destroy(c);
            cards.Clear();
        }

        private static GameObject MakeAnchored(string name, Transform parent, Vector2 min, Vector2 max)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = min; rt.anchorMax = max;
            rt.offsetMin = rt.offsetMax = Vector2.zero;
            return go;
        }
    }
}
