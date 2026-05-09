using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// Attack Forge UI — LMB ekol seçim/yükseltme ekranı.
    /// DraftManager.OnForgeRoom event'ine abone olur.
    /// Forge #1 (Oda 4): 3 ecol seçeneği sun.
    /// Forge #2 (Oda 8): seçili ecolu yükselt (Lv2).
    /// </summary>
    public class ForgeUI : MonoBehaviour
    {
        public static ForgeUI Instance { get; private set; }

        /// <summary>Oyuncu ecol seçince ateşlenir — arg: ecol adı.</summary>
        public UnityEvent<string> OnEcolChosen = new UnityEvent<string>();

        private GameObject panel;
        private Transform  cardContainer;
        private readonly List<GameObject> cards = new List<GameObject>();

        private static readonly Color forgeColor = new Color(0.85f, 0.45f, 0.10f); // amber

        // Warblade LMB ecollari (Faz 1)
        private static readonly (string name, string desc)[] WarbladeLMBEcols =
        {
            ("Fury Strikes",  "Her isabet +2 Rage.\nHer 3. isabet +8 Rage bonus.\nRage %80+ → LMB +%30 hasar."),
            ("Savage Edge",   "3. isabet bleed uygular (3s).\nBleed hedefe +%20 LMB hasar.\nBleed tick LMB CD -0.1s."),
            ("Bone Breaker",  "Son isabet hedefi yavaşlatır.\nHP<%50 hedefe +%25 LMB hasar.\nKnockback + 1s mikro stun."),
        };

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start()
        {
            BuildPanel();
            // DraftManager'ın OnForgeRoom event'ine abone ol
            StartCoroutine(SubscribeDelayed());
        }

        private System.Collections.IEnumerator SubscribeDelayed()
        {
            yield return null; // DraftManager.Start bekle
            if (DraftManager.Instance != null)
                DraftManager.Instance.OnForgeRoom.AddListener(OnForgeRoomTriggered);
        }

        private void OnDestroy()
        {
            if (DraftManager.Instance != null)
                DraftManager.Instance.OnForgeRoom.RemoveListener(OnForgeRoomTriggered);
        }

        // ── Public ───────────────────────────────────────────────

        public void ShowForge1()
        {
            ClearCards();
            panel?.SetActive(true);
            SetTitle("SALDIRI OCAĞI  —  LMB EKOL SEÇ  (Lv1)");
            SetSubtitle("Seçtiğin ekol Oda 8'de güçlendirilecek.");
            foreach (var (name, desc) in WarbladeLMBEcols)
                BuildEcolCard(name, desc, forgeNumber: 1);
            UIManager.Instance.PauseForMenu();
        }

        public void ShowForge2(string chosenEcol)
        {
            ClearCards();
            panel?.SetActive(true);
            SetTitle("SALDIRI OCAĞI  —  EKOL YÜKSELT  (Lv2)");
            SetSubtitle("Seçtiğin ekol: " + (chosenEcol.Length > 0 ? chosenEcol : "?"));

            if (!string.IsNullOrEmpty(chosenEcol))
            {
                // Sadece seçili ecolu göster (upgrade)
                foreach (var (name, desc) in WarbladeLMBEcols)
                    if (name == chosenEcol)
                        BuildEcolCard(name, desc + "\n\n<color=#aaffaa>→ Lv2 aktif</color>", forgeNumber: 2);
            }
            else
            {
                SetSubtitle("Henüz ecol seçilmemiş — sistem hata.");
            }
            UIManager.Instance.PauseForMenu();
        }

        public void Hide()
        {
            ClearCards();
            panel?.SetActive(false);
            UIManager.Instance.ResumeFromMenu();
        }

        // ── Internal ─────────────────────────────────────────────

        private void OnForgeRoomTriggered(int forgeNumber)
        {
            if (forgeNumber == 1) ShowForge1();
            else if (forgeNumber == 2) ShowForge2(LMBEcolSystem.ChosenEcol);
        }

        private void BuildPanel()
        {
            var canvas = FindObjectOfType<Canvas>();
            if (canvas == null) return;

            panel = MakeAnchored("ForgePanel", canvas.transform,
                new Vector2(0.05f, 0.10f), new Vector2(0.95f, 0.90f));
            panel.AddComponent<Image>().color = new Color(0.05f, 0.03f, 0.02f, 0.96f);

            // Header strip
            var hdrStrip = MakeAnchored("HdrStrip", panel.transform,
                new Vector2(0f, 0.92f), Vector2.one);
            hdrStrip.AddComponent<Image>().color = new Color(forgeColor.r * 0.40f, forgeColor.g * 0.25f, 0.02f, 1f);

            var titleGo = MakeAnchored("Title", panel.transform,
                new Vector2(0.05f, 0.87f), new Vector2(0.95f, 0.97f));
            var titleTmp = titleGo.AddComponent<TextMeshProUGUI>();
            titleTmp.name = "ForgeTitle";
            titleTmp.text = "SALDIRI OCAĞI"; titleTmp.fontSize = 20; titleTmp.fontStyle = FontStyles.Bold;
            titleTmp.color = forgeColor; titleTmp.alignment = TextAlignmentOptions.Center;
            titleTmp.raycastTarget = false;

            var subGo = MakeAnchored("Subtitle", panel.transform,
                new Vector2(0.05f, 0.81f), new Vector2(0.95f, 0.87f));
            var subTmp = subGo.AddComponent<TextMeshProUGUI>();
            subTmp.name = "ForgeSubtitle";
            subTmp.fontSize = 11; subTmp.color = new Color(0.60f, 0.45f, 0.28f);
            subTmp.alignment = TextAlignmentOptions.Center; subTmp.raycastTarget = false;

            var cc = MakeAnchored("Cards", panel.transform,
                new Vector2(0.04f, 0.12f), new Vector2(0.96f, 0.80f));
            var hg = cc.AddComponent<HorizontalLayoutGroup>();
            hg.spacing = 18; hg.childAlignment = TextAnchor.MiddleCenter;
            hg.childForceExpandWidth = true; hg.childForceExpandHeight = true;
            hg.padding = new RectOffset(6, 6, 0, 0);
            cardContainer = cc.transform;

            panel.SetActive(false);
        }

        private void BuildEcolCard(string ecolName, string desc, int forgeNumber)
        {
            var card = new GameObject("Ecol_" + ecolName, typeof(RectTransform));
            card.transform.SetParent(cardContainer, false);
            card.AddComponent<LayoutElement>();
            card.AddComponent<Image>().color = new Color(0.12f, 0.07f, 0.02f, 0.98f);

            MakeAnchored("Strip", card.transform, new Vector2(0f, 0.93f), Vector2.one)
                .AddComponent<Image>().color = forgeColor;

            var nameGo = MakeAnchored("Name", card.transform, new Vector2(0.04f, 0.70f), new Vector2(0.96f, 0.92f));
            var nt = nameGo.AddComponent<TextMeshProUGUI>();
            nt.text = ecolName.ToUpper(); nt.fontSize = 14; nt.fontStyle = FontStyles.Bold;
            nt.color = forgeColor; nt.alignment = TextAlignmentOptions.Center;
            nt.enableWordWrapping = true; nt.raycastTarget = false;

            var descGo = MakeAnchored("Desc", card.transform, new Vector2(0.04f, 0.18f), new Vector2(0.96f, 0.69f));
            var dt = descGo.AddComponent<TextMeshProUGUI>();
            dt.text = desc; dt.fontSize = 10;
            dt.color = new Color(0.80f, 0.72f, 0.60f); dt.alignment = TextAlignmentOptions.Top;
            dt.enableWordWrapping = true; dt.raycastTarget = false;

            var btnGo = MakeAnchored("Btn", card.transform, new Vector2(0.08f, 0.03f), new Vector2(0.92f, 0.15f));
            btnGo.AddComponent<Image>().color = new Color(0.38f, 0.18f, 0.04f, 1f);
            var btn = btnGo.AddComponent<Button>();
            var bc = btn.colors;
            bc.normalColor      = new Color(0.38f, 0.18f, 0.04f, 1f);
            bc.highlightedColor = new Color(0.65f, 0.32f, 0.08f, 1f);
            bc.pressedColor     = forgeColor;
            btn.colors = bc;
            string capName = ecolName;
            int capForge = forgeNumber;
            btn.onClick.AddListener(() =>
            {
                if (capForge == 1) LMBEcolSystem.SetEcol(capName);
                else               LMBEcolSystem.UpgradeEcol();
                OnEcolChosen.Invoke(capName);
                Hide();
            });

            var bl = MakeAnchored("T", btnGo.transform, Vector2.zero, Vector2.one).AddComponent<TextMeshProUGUI>();
            bl.text = forgeNumber == 1 ? "SEÇ" : "YÜKSELT";
            bl.fontSize = 12; bl.fontStyle = FontStyles.Bold;
            bl.color = Color.white; bl.alignment = TextAlignmentOptions.Center; bl.raycastTarget = false;

            cards.Add(card);
        }

        private void SetTitle(string text)
        {
            var t = panel?.transform.Find("ForgeTitle")?.GetComponent<TextMeshProUGUI>();
            if (t != null) t.text = text;
        }

        private void SetSubtitle(string text)
        {
            var t = panel?.transform.Find("ForgeSubtitle")?.GetComponent<TextMeshProUGUI>();
            if (t != null) t.text = text;
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
