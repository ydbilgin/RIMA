using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RIMA
{
    public class CharacterSelectScreen : MonoBehaviour
    {
        private static readonly (ClassType type, string label, string desc, Color color)[] Classes =
        {
            (ClassType.Warblade,
             "WARBLADE",
             "Yakin dovus\nYuksek dayaniklilik",
             new Color(0.85f, 0.60f, 0.28f)),

            (ClassType.Elementalist,
             "ELEMENTALIST",
             "Ates ve buz\nAlan hasari",
             new Color(0.22f, 0.52f, 0.95f)),

            (ClassType.Shadowblade,
             "SHADOWBLADE",
             "Zehir, kanama\nAni burst",
             new Color(0.52f, 0.16f, 0.85f)),

            (ClassType.Ranger,
             "RANGER",
             "Mesafe, CC\nTuzak sinerji",
             new Color(0.22f, 0.72f, 0.32f)),
        };

        private void Start()
        {
            Time.timeScale = 0f;
            BuildUI();
        }

        private void BuildUI()
        {
            var canvasGO = new GameObject("CharSelectCanvas");
            canvasGO.transform.SetParent(transform);
            var canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode   = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
            var scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode         = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight  = 0.5f;
            canvasGO.AddComponent<GraphicRaycaster>();

            var root = MakeRect("Root", canvasGO.transform, Vector2.zero, Vector2.one);
            var bg = root.AddComponent<Image>();
            bg.sprite = RimaUITheme.MenuDungeonBackground;
            bg.color = Color.white;
            bg.preserveAspect = true;

            MakeRect("Shade", root.transform, Vector2.zero, Vector2.one)
                .AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.56f);

            var leftPanel = MakeRect("LeftPanel", root.transform,
                new Vector2(0.055f, 0.17f), new Vector2(0.30f, 0.78f));
            var leftImage = leftPanel.AddComponent<Image>();
            leftImage.sprite = RimaUITheme.SmallPanelFrame;
            leftImage.color = new Color(1f, 1f, 1f, 0.94f);

            // Header bar
            var header = MakeRect("Header", leftPanel.transform,
                new Vector2(0.08f, 0.61f), new Vector2(0.92f, 0.88f));
            var headerImage = header.AddComponent<Image>();
            headerImage.sprite = RimaUITheme.RoomBannerFrame;
            headerImage.color = new Color(1f, 1f, 1f, 0.92f);

            Txt(header.transform, "SINIF SEC",
                new Vector2(0.08f, 0.50f), new Vector2(0.94f, 0.95f),
                24, FontStyles.Bold, new Color(0.90f, 0.92f, 1f), TextAlignmentOptions.Left);

            Txt(header.transform, "Secim kalicidir — beceri setin sectigine gore sekillenir.",
                new Vector2(0.08f, 0.05f), new Vector2(0.94f, 0.50f),
                11, FontStyles.Normal, new Color(0.48f, 0.52f, 0.62f), TextAlignmentOptions.Left);

            Txt(leftPanel.transform, "PRIMARY CLASS",
                new Vector2(0.10f, 0.43f), new Vector2(0.90f, 0.52f),
                13, FontStyles.Bold, RimaUITheme.Cyan, TextAlignmentOptions.Left);

            Txt(leftPanel.transform, "Baslangic skill barini ve run teklif havuzunu belirler. Sonraki odalarda build yonunu tekliflerle degistirirsin.",
                new Vector2(0.10f, 0.21f), new Vector2(0.90f, 0.43f),
                11, FontStyles.Normal, new Color(0.60f, 0.64f, 0.74f), TextAlignmentOptions.Left);

            // Back button
            var backGO = MakeRect("BackBtn", root.transform,
                new Vector2(0.86f, 0.87f), new Vector2(0.965f, 0.93f));
            backGO.AddComponent<Image>().color = new Color(0.15f, 0.15f, 0.25f, 1f);
            var back       = backGO.AddComponent<Button>();
            var backColors = back.colors;
            backColors.highlightedColor = new Color(0.25f, 0.25f, 0.40f, 1f);
            back.colors                 = backColors;
            Txt(backGO.transform, "< GERI",
                Vector2.zero, Vector2.one, 13, FontStyles.Normal,
                new Color(0.65f, 0.68f, 0.78f), TextAlignmentOptions.Center);
            back.onClick.AddListener(OnBack);

            // Cards
            float cardW  = 0.145f;
            float gap    = 0.018f;
            float totalW = Classes.Length * cardW + (Classes.Length - 1) * gap;
            float startX = 0.345f;

            for (int i = 0; i < Classes.Length; i++)
            {
                float x0 = startX + i * (cardW + gap);
                BuildCard(root.transform, Classes[i], x0, x0 + cardW);
            }
        }

        private void BuildCard(Transform parent,
            (ClassType type, string label, string desc, Color color) info,
            float x0, float x1)
        {
            var card = MakeRect("Card_" + info.type, parent,
                new Vector2(x0, 0.19f), new Vector2(x1, 0.76f));
            var cardImage = card.AddComponent<Image>();
            cardImage.sprite = RimaUITheme.SmallPanelFrame;
            cardImage.color = new Color(1f, 1f, 1f, 0.92f);

            // Left accent bar
            MakeRect("Accent", card.transform, new Vector2(0f, 0f), new Vector2(0.035f, 1f))
                .AddComponent<Image>().color = new Color(info.color.r, info.color.g, info.color.b, 0.70f);

            // Top color strip
            MakeRect("TopStrip", card.transform, new Vector2(0.035f, 0.935f), Vector2.one)
                .AddComponent<Image>().color = info.color;

            // Portrait
            var portraitGO  = MakeRect("Portrait", card.transform,
                new Vector2(0.10f, 0.50f), new Vector2(0.90f, 0.90f));
            var portraitImg = portraitGO.AddComponent<Image>();
            portraitImg.color = new Color(info.color.r * 0.05f, info.color.g * 0.05f, info.color.b * 0.08f, 1f);

            string classLower = info.type.ToString().ToLower();
            var sprite = Resources.Load<Sprite>($"Characters/{info.type}/{classLower}_idle_south");
            if (sprite != null)
            {
                portraitImg.sprite         = sprite;
                portraitImg.preserveAspect = true;
                portraitImg.color          = Color.white;
            }

            // Class name
            Txt(card.transform, info.label,
                new Vector2(0.06f, 0.38f), new Vector2(0.97f, 0.47f),
                13, FontStyles.Bold, Color.white, TextAlignmentOptions.Center);

            // Description
            Txt(card.transform, info.desc,
                new Vector2(0.06f, 0.22f), new Vector2(0.97f, 0.36f),
                9, FontStyles.Normal,
                new Color(info.color.r * 0.7f + 0.3f, info.color.g * 0.7f + 0.3f, info.color.b * 0.7f + 0.3f, 0.90f),
                TextAlignmentOptions.Center);

            // Divider
            MakeRect("Div", card.transform, new Vector2(0.06f, 0.215f), new Vector2(0.94f, 0.222f))
                .AddComponent<Image>().color = new Color(1f, 1f, 1f, 0.08f);

            // SEC (select) button
            var btnGO = MakeRect("SelectBtn", card.transform,
                new Vector2(0.10f, 0.055f), new Vector2(0.90f, 0.17f));
            btnGO.AddComponent<Image>().color =
                new Color(info.color.r * 0.25f, info.color.g * 0.25f, info.color.b * 0.32f, 1f);

            var btn    = btnGO.AddComponent<Button>();
            var colors = btn.colors;
            colors.normalColor      = new Color(info.color.r * 0.25f, info.color.g * 0.25f, info.color.b * 0.32f, 1f);
            colors.highlightedColor = new Color(info.color.r * 0.50f, info.color.g * 0.50f, info.color.b * 0.60f, 1f);
            colors.pressedColor     = info.color;
            btn.colors              = colors;

            Txt(btnGO.transform, "SEC",
                Vector2.zero, Vector2.one, 12, FontStyles.Bold, Color.white, TextAlignmentOptions.Center);

            var capturedType = info.type;
            btn.onClick.AddListener(() => OnClassSelected(capturedType));
        }

        private void OnClassSelected(ClassType type)
        {
            Time.timeScale = 1f;
            PlayerClassManager.Instance?.SetPrimaryClass(type);

            Destroy(gameObject);
        }

        private void OnBack()
        {
            Time.timeScale = 1f;
            Destroy(gameObject);
            var go = new GameObject("[MainMenuScreen]");
            DontDestroyOnLoad(go);
            go.AddComponent<MainMenuScreen>();
        }

        private static GameObject MakeRect(string name, Transform parent, Vector2 ancMin, Vector2 ancMax)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt       = go.GetComponent<RectTransform>();
            rt.anchorMin = ancMin;
            rt.anchorMax = ancMax;
            rt.offsetMin = rt.offsetMax = Vector2.zero;
            return go;
        }

        private static void Txt(Transform parent, string text,
            Vector2 ancMin, Vector2 ancMax,
            int size, FontStyles style, Color color, TextAlignmentOptions align)
        {
            var go  = MakeRect("T", parent, ancMin, ancMax);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.text               = text;
            tmp.fontSize           = size;
            tmp.fontStyle          = style;
            tmp.color              = color;
            tmp.alignment          = align;
            tmp.enableWordWrapping = true;
            tmp.raycastTarget      = false;
        }
    }
}
