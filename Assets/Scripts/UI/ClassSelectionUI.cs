using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RIMA
{
    public class ClassSelectionUI : MonoBehaviour
    {
        private GameObject panel;

        private static readonly (ClassType type, string name, string tag, string passive, Color color)[] Classes =
        {
            (ClassType.Elementalist,
             "ELEMENTALİST", "Alan kontrolü + ateş/buz zinciri",
             "Rage >50 → Ateş hasarı +25%\nAteş isabet → Rage +5",
             new Color(0.22f, 0.52f, 0.95f)),

            (ClassType.Shadowblade,
             "SHADOWBLADE", "Zehir ve kanama DoT, ani burst",
             "Kanama tick → Energy +3\nKill → Energy +15",
             new Color(0.52f, 0.16f, 0.85f)),

            (ClassType.Ranger,
             "RANGER", "Mesafe hasarı, CC sinerjisi",
             "CC'li hedefe +30% hasar\nCC biter → 3s Exposed",
             new Color(0.22f, 0.72f, 0.32f)),
        };

        private void Start()
        {
            if (PlayerClassManager.Instance != null)
                PlayerClassManager.Instance.OnClassSelectionRequested += Show;
            else
                Debug.LogWarning("[ClassSelectionUI] PlayerClassManager bulunamadı.");
        }

        private void OnDestroy()
        {
            if (PlayerClassManager.Instance != null)
                PlayerClassManager.Instance.OnClassSelectionRequested -= Show;
        }

        private void Show()
        {
            if (panel != null) return;
            DraftManager.Instance?.HideDraft(); // skill draft açıksa kapat
            Time.timeScale = 0f;

            var canvas = FindObjectOfType<Canvas>();
            if (canvas == null) { Debug.LogWarning("[ClassSelectionUI] Canvas yok."); return; }

            // ── Tam ekran ──────────────────────────────────────
            panel = MakeRect("ClassSelect", canvas.transform, Vector2.zero, Vector2.one);
            panel.AddComponent<Image>().color = new Color(0.03f, 0.03f, 0.08f, 0.95f);

            // ── Üst çubuk ──────────────────────────────────────
            var topBar = MakeRect("TopBar", panel.transform,
                new Vector2(0, 0.88f), new Vector2(1, 1f));
            topBar.AddComponent<Image>().color = new Color(0.07f, 0.07f, 0.14f, 1f);

            Label(topBar.transform, "İKİNCİ SINIF SEÇ",
                new Vector2(0.05f, 0.55f), new Vector2(0.95f, 0.95f),
                22, FontStyles.Bold, new Color(0.90f, 0.92f, 1f), TextAlignmentOptions.Left);

            Label(topBar.transform,
                "Seçim kalıcıdır. İki sınıfın yetenekleri birlikte çalışır — Z/X slotları açılır.",
                new Vector2(0.05f, 0.08f), new Vector2(0.95f, 0.52f),
                11, FontStyles.Normal, new Color(0.48f, 0.52f, 0.62f), TextAlignmentOptions.Left);

            // Primary class etiketi (sağ üst)
            Label(topBar.transform, "PRIMARY: WARBLADE",
                new Vector2(0.72f, 0.55f), new Vector2(0.98f, 0.95f),
                10, FontStyles.Bold, new Color(0.85f, 0.60f, 0.28f, 0.85f), TextAlignmentOptions.Right);

            // ── Kartlar ────────────────────────────────────────
            float cardW = 0.26f, gap = 0.03f;
            float totalW = Classes.Length * cardW + (Classes.Length - 1) * gap;
            float startX = (1f - totalW) / 2f;

            for (int i = 0; i < Classes.Length; i++)
            {
                float x0 = startX + i * (cardW + gap);
                BuildCard(panel.transform, Classes[i], x0, x0 + cardW);
            }
        }

        private void BuildCard(Transform parent,
            (ClassType type, string name, string tag, string passive, Color color) info,
            float x0, float x1)
        {
            var card = MakeRect("Card_" + info.type, parent,
                new Vector2(x0, 0.07f), new Vector2(x1, 0.86f));

            // Arka plan
            card.AddComponent<Image>().color =
                new Color(info.color.r * 0.08f, info.color.g * 0.08f, info.color.b * 0.12f, 1f);

            // Sol renk şeridi
            var accent = MakeRect("Accent", card.transform,
                new Vector2(0, 0), new Vector2(0.03f, 1f));
            accent.AddComponent<Image>().color =
                new Color(info.color.r, info.color.g, info.color.b, 0.70f);

            // Üst şerit
            var topStrip = MakeRect("Top", card.transform,
                new Vector2(0.03f, 0.93f), new Vector2(1f, 1f));
            topStrip.AddComponent<Image>().color = info.color;

            // Class adı
            Label(card.transform, info.name,
                new Vector2(0.06f, 0.84f), new Vector2(0.97f, 0.93f),
                17, FontStyles.Bold, Color.white, TextAlignmentOptions.Left);

            // Kısa tag
            Label(card.transform, info.tag,
                new Vector2(0.06f, 0.76f), new Vector2(0.97f, 0.84f),
                10, FontStyles.Normal, new Color(info.color.r * 0.8f + 0.2f, info.color.g * 0.8f + 0.2f, info.color.b * 0.8f + 0.2f, 0.85f),
                TextAlignmentOptions.Left);

            // Ayırıcı
            var div = MakeRect("Div", card.transform,
                new Vector2(0.06f, 0.745f), new Vector2(0.94f, 0.75f));
            div.AddComponent<Image>().color = new Color(1, 1, 1, 0.08f);

            // Pasif başlık
            Label(card.transform, "CROSS-CLASS PASİF",
                new Vector2(0.06f, 0.66f), new Vector2(0.94f, 0.74f),
                8, FontStyles.Bold,
                new Color(info.color.r, info.color.g, info.color.b, 0.75f),
                TextAlignmentOptions.Left);

            // Pasif şerit
            var pDiv = MakeRect("PDiv", card.transform,
                new Vector2(0.06f, 0.648f), new Vector2(0.94f, 0.653f));
            pDiv.AddComponent<Image>().color =
                new Color(info.color.r, info.color.g, info.color.b, 0.25f);

            // Pasif metin
            Label(card.transform, info.passive,
                new Vector2(0.06f, 0.44f), new Vector2(0.94f, 0.645f),
                11, FontStyles.Normal,
                new Color(info.color.r * 0.85f + 0.15f, info.color.g * 0.85f + 0.15f, info.color.b * 0.85f + 0.15f),
                TextAlignmentOptions.Left);

            // SEÇ butonu
            var btn = MakeRect("Btn", card.transform,
                new Vector2(0.06f, 0.04f), new Vector2(0.94f, 0.16f));
            var btnImg = btn.AddComponent<Image>();
            btnImg.color = new Color(info.color.r * 0.25f, info.color.g * 0.25f, info.color.b * 0.32f, 1f);

            var b = btn.AddComponent<Button>();
            var bc = b.colors;
            bc.normalColor      = new Color(info.color.r * 0.25f, info.color.g * 0.25f, info.color.b * 0.32f, 1f);
            bc.highlightedColor = new Color(info.color.r * 0.50f, info.color.g * 0.50f, info.color.b * 0.60f, 1f);
            bc.pressedColor     = info.color;
            b.colors = bc;

            Label(btn.transform, "SEÇ",
                Vector2.zero, Vector2.one,
                14, FontStyles.Bold, Color.white, TextAlignmentOptions.Center);

            var cap = info.type;
            var capColor = info.color;
            var capName = info.name;
            b.onClick.AddListener(() => OnSelect(cap, capName, capColor));
        }

        private void OnSelect(ClassType type, string className, Color color)
        {
            Time.timeScale = 1f;
            Destroy(panel);
            panel = null;
            PlayerClassManager.Instance?.SelectSecondaryClass(type);
            StartCoroutine(ShowBanner(className, color));
        }

        private IEnumerator ShowBanner(string className, Color color)
        {
            var canvas = FindObjectOfType<Canvas>();
            if (canvas == null) yield break;

            var banner = MakeRect("ClassBanner", canvas.transform,
                new Vector2(0.15f, 0.72f), new Vector2(0.85f, 0.84f));
            banner.AddComponent<Image>().color =
                new Color(color.r * 0.15f, color.g * 0.15f, color.b * 0.20f, 0.96f);

            // Sol şerit
            var accent = MakeRect("A", banner.transform, new Vector2(0, 0), new Vector2(0.008f, 1f));
            accent.AddComponent<Image>().color = color;

            Label(banner.transform, $"SECONDARY CLASS: {className}",
                new Vector2(0.03f, 0.52f), new Vector2(0.97f, 0.95f),
                16, FontStyles.Bold, Color.white, TextAlignmentOptions.Left);

            Label(banner.transform, "Z/X slotlari açildi — yeni yetenekler draft'a eklendi.",
                new Vector2(0.03f, 0.08f), new Vector2(0.97f, 0.50f),
                10, FontStyles.Normal, new Color(0.70f, 0.75f, 0.85f), TextAlignmentOptions.Left);

            yield return new WaitForSeconds(1.5f);

            // Yavaş kaybolma
            var img  = banner.GetComponent<Image>();
            var tmps = banner.GetComponentsInChildren<TextMeshProUGUI>();
            float t = 0f;
            Color baseColor = img ? img.color : Color.clear;
            while (t < 0.5f)
            {
                t += Time.deltaTime;
                float a = 1f - t / 0.5f;
                if (img) img.color = new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a * a);
                foreach (var tmp in tmps) tmp.alpha = a;
                yield return null;
            }

            Destroy(banner);
        }

        // ── Helpers ─────────────────────────────────────────────

        private static GameObject MakeRect(string name, Transform parent,
            Vector2 ancMin, Vector2 ancMax)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = ancMin; rt.anchorMax = ancMax;
            rt.offsetMin = rt.offsetMax = Vector2.zero;
            return go;
        }

        private static void Label(Transform parent, string text,
            Vector2 ancMin, Vector2 ancMax,
            int size, FontStyles style, Color color, TextAlignmentOptions align)
        {
            var go = MakeRect("T", parent, ancMin, ancMax);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.text             = text;
            tmp.fontSize         = size;
            tmp.fontStyle        = style;
            tmp.color            = color;
            tmp.alignment        = align;
            tmp.enableWordWrapping = true;
            tmp.raycastTarget    = false;
        }
    }
}
