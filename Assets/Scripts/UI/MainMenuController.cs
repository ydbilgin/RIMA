using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RIMA.UI
{
    public class MainMenuController : MonoBehaviour
    {
        private static readonly Color Cyan = new Color(0f, 1f, 0.8f, 1f);
        private static readonly Color WarmOrange = new Color(0.91f, 0.56f, 0.13f, 1f);

        [SerializeField] private string characterSelectScene = "CharacterSelect";

        private RectTransform runtimeRoot;

        private void Start()
        {
            BuildRuntimeMenu();
        }

        public void OnStartClicked()
        {
            SceneManager.LoadScene(characterSelectScene);
        }

        public void OnQuitClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void OnSettingsClicked()
        {
            if (UIManager.Instance != null)
                UIManager.Instance.OpenSettings();
            else
                FindFirstObjectByType<SettingsMenuUI>()?.Open();
        }

        private void BuildRuntimeMenu()
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas == null) canvas = FindFirstObjectByType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("[MainMenu] Runtime menu build failed: Canvas not found.");
                return;
            }

            EnsureRuntimeScaler(canvas);

            var raycaster = canvas.GetComponent<GraphicRaycaster>();
            if (raycaster == null) canvas.gameObject.AddComponent<GraphicRaycaster>();

            DisableAuthoredRoot(canvas.transform);
            RemoveExistingRuntimeRoot(canvas.transform);

            RimaUITheme.CreateFullScreenBackdrop(canvas.transform, "UI/Backgrounds/main_menu_bg", RimaUITheme.BackgroundDark);

            runtimeRoot = MakeRect("RuntimeRoot_MainMenu", canvas.transform, Vector2.zero, Vector2.one);
            runtimeRoot.offsetMin = runtimeRoot.offsetMax = Vector2.zero;

            AddVignette(runtimeRoot);
            AddTitleColumn(runtimeRoot);
            AddVersion(runtimeRoot);
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

        private void DisableAuthoredRoot(Transform canvasTransform)
        {
            Transform authoredRoot = canvasTransform.Find("Root");
            if (authoredRoot == null) return;

            if (authoredRoot == transform)
            {
                for (int i = authoredRoot.childCount - 1; i >= 0; i--)
                {
                    Destroy(authoredRoot.GetChild(i).gameObject);
                }
                return;
            }

            if (transform.IsChildOf(authoredRoot))
            {
                for (int i = authoredRoot.childCount - 1; i >= 0; i--)
                {
                    Transform child = authoredRoot.GetChild(i);
                    if (child == transform || transform.IsChildOf(child)) continue;
                    Destroy(child.gameObject);
                }
                return;
            }

            authoredRoot.gameObject.SetActive(false);
        }

        private void RemoveExistingRuntimeRoot(Transform canvasTransform)
        {
            Transform oldRoot = canvasTransform.Find("RuntimeRoot_MainMenu");
            if (oldRoot != null) Destroy(oldRoot.gameObject);
        }

        private void AddVignette(RectTransform parent)
        {
            var go = MakeRect("RadialVignette", parent, Vector2.zero, Vector2.one);
            var img = go.gameObject.AddComponent<Image>();
            img.sprite = CreateVignetteSprite();
            img.color = Color.white;
            img.raycastTarget = false;
        }

        private void AddTitleColumn(RectTransform parent)
        {
            var column = MakeRect("InkTitleColumn", parent, Vector2.zero, Vector2.zero);
            column.pivot = Vector2.zero;
            column.anchoredPosition = new Vector2(112f, 116f);
            column.sizeDelta = new Vector2(560f, 470f);

            var title = AddText(column, "Title_RIMA", "RIMA", 82f, RimaUITheme.TextPrimary, TextAlignmentOptions.Left);
            title.fontStyle = FontStyles.Bold;
            title.characterSpacing = 16f;
            var titleRt = title.rectTransform;
            titleRt.anchorMin = titleRt.anchorMax = new Vector2(0f, 1f);
            titleRt.pivot = new Vector2(0f, 1f);
            titleRt.anchoredPosition = new Vector2(0f, 0f);
            titleRt.sizeDelta = new Vector2(500f, 96f);

            var shadow = title.gameObject.AddComponent<Shadow>();
            shadow.effectColor = new Color(0.18f, 0f, 0.32f, 0.62f);
            shadow.effectDistance = new Vector2(5f, -5f);

            var subtitle = AddText(column, "Subtitle_RiftHunters", "THE RIFT HUNTERS", 15f, WithAlpha(RimaUITheme.TextMuted, 0.72f), TextAlignmentOptions.Left);
            subtitle.fontStyle = FontStyles.Bold;
            subtitle.characterSpacing = 18f;
            var subRt = subtitle.rectTransform;
            subRt.anchorMin = subRt.anchorMax = new Vector2(0f, 1f);
            subRt.pivot = new Vector2(0f, 1f);
            subRt.anchoredPosition = new Vector2(3f, -94f);
            subRt.sizeDelta = new Vector2(500f, 28f);

            var whisper = AddText(column, "Whisper_YineGeldin", "Yine geldin.", 12f, WithAlpha(RimaUITheme.TextMuted, 0.58f), TextAlignmentOptions.Left);
            whisper.fontStyle = FontStyles.Italic;
            whisper.color = WarmOrange;
            var whisperRt = whisper.rectTransform;
            whisperRt.anchorMin = whisperRt.anchorMax = new Vector2(0f, 1f);
            whisperRt.pivot = new Vector2(0f, 1f);
            whisperRt.anchoredPosition = new Vector2(4f, -214f);
            whisperRt.sizeDelta = new Vector2(260f, 24f);

            AddCyanDivider(column, new Vector2(0f, -248f), 146f);
            AddNakedButton(column, "Button_Basla", "BAŞLA", new Vector2(18f, -242f), OnStartClicked);
            AddNakedButton(column, "Button_Ayarlar", "AYARLAR", new Vector2(18f, -292f), OnSettingsClicked);
            AddNakedButton(column, "Button_Cikis", "ÇIKIŞ", new Vector2(18f, -342f), OnQuitClicked);
        }

        private void AddCyanDivider(RectTransform parent, Vector2 position, float height)
        {
            AddDividerSegment(parent, "CyanDivider_TopFade", position, 18f, WithAlpha(Cyan, 0.24f));
            AddDividerSegment(parent, "CyanDivider_Core", new Vector2(position.x, position.y - 18f), height - 36f, WithAlpha(Cyan, 0.92f));
            AddDividerSegment(parent, "CyanDivider_BottomFade", new Vector2(position.x, position.y - height + 18f), 18f, WithAlpha(Cyan, 0.24f));
        }

        private void AddDividerSegment(RectTransform parent, string name, Vector2 position, float height, Color color)
        {
            var rt = MakeRect(name, parent, new Vector2(0f, 1f), new Vector2(0f, 1f));
            rt.pivot = new Vector2(0.5f, 1f);
            rt.anchoredPosition = position;
            rt.sizeDelta = new Vector2(2f, height);

            var image = rt.gameObject.AddComponent<Image>();
            image.color = color;
            image.raycastTarget = false;
        }

        private TMP_Text AddText(RectTransform parent, string name, string text, float size, Color color, TextAlignmentOptions alignment)
        {
            var rt = MakeRect(name, parent, Vector2.zero, Vector2.one);
            var tmp = rt.gameObject.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = size;
            tmp.color = color;
            tmp.alignment = alignment;
            tmp.raycastTarget = false;
            return tmp;
        }

        private void AddNakedButton(RectTransform parent, string name, string text, Vector2 position, UnityAction onClick)
        {
            var rt = MakeRect(name, parent, new Vector2(0f, 1f), new Vector2(0f, 1f));
            rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = position;
            rt.sizeDelta = new Vector2(260f, 34f);

            var hitArea = rt.gameObject.AddComponent<Image>();
            hitArea.color = Color.clear;
            hitArea.raycastTarget = true;

            var button = rt.gameObject.AddComponent<Button>();
            button.targetGraphic = hitArea;
            var colors = button.colors;
            colors.normalColor = Color.clear;
            colors.highlightedColor = Color.clear;
            colors.pressedColor = Color.clear;
            colors.selectedColor = Color.clear;
            colors.disabledColor = Color.clear;
            button.colors = colors;
            button.onClick.AddListener(onClick);

            var arrow = AddText(rt, "HoverArrow", ">", 20f, Cyan, TextAlignmentOptions.Left);
            arrow.fontStyle = FontStyles.Bold;
            var arrowRt = arrow.rectTransform;
            arrowRt.anchorMin = arrowRt.anchorMax = new Vector2(0f, 0.5f);
            arrowRt.pivot = new Vector2(0f, 0.5f);
            arrowRt.anchoredPosition = new Vector2(0f, 0f);
            arrowRt.sizeDelta = new Vector2(22f, 28f);
            arrow.gameObject.SetActive(false);

            var label = AddText(rt, "Label", text, 22f, WithAlpha(RimaUITheme.TextPrimary, 0.70f), TextAlignmentOptions.Left);
            label.fontStyle = FontStyles.Bold;
            var labelRt = label.rectTransform;
            labelRt.anchorMin = labelRt.anchorMax = new Vector2(0f, 0.5f);
            labelRt.pivot = new Vector2(0f, 0.5f);
            labelRt.anchoredPosition = new Vector2(28f, 0f);
            labelRt.sizeDelta = new Vector2(220f, 34f);

            var hover = rt.gameObject.AddComponent<NakedMenuButtonHover>();
            hover.Bind(label, arrow);
        }

        private void AddVersion(RectTransform parent)
        {
            var version = AddText(parent, "Version", $"v{Application.version}", 8f, WithAlpha(RimaUITheme.TextMuted, 0.35f), TextAlignmentOptions.Right);
            var rt = version.rectTransform;
            rt.anchorMin = rt.anchorMax = new Vector2(1f, 0f);
            rt.pivot = new Vector2(1f, 0f);
            rt.anchoredPosition = new Vector2(-18f, 14f);
            rt.sizeDelta = new Vector2(180f, 18f);
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

        private static Sprite CreateVignetteSprite()
        {
            const int size = 256;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            Color edge = new Color(0.02f, 0f, 0.05f, 0.60f);
            Vector2 center = new Vector2((size - 1) * 0.5f, (size - 1) * 0.5f);
            float maxDistance = center.magnitude;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), center) / maxDistance;
                    float alpha = Mathf.SmoothStep(0f, edge.a, Mathf.InverseLerp(0.28f, 1f, distance));
                    texture.SetPixel(x, y, new Color(edge.r, edge.g, edge.b, alpha));
                }
            }

            texture.Apply(false, true);
            return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        private static Color WithAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        private sealed class NakedMenuButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
        {
            private TMP_Text label;
            private TMP_Text arrow;
            private Color normalColor;
            private bool hovering;
            private bool pressed;

            public void Bind(TMP_Text labelText, TMP_Text arrowText)
            {
                label = labelText;
                arrow = arrowText;
                normalColor = label != null ? label.color : Color.white;
                Apply(false);
            }

            public void OnPointerEnter(PointerEventData eventData)
            {
                hovering = true;
                Apply(true);
            }

            public void OnPointerExit(PointerEventData eventData)
            {
                hovering = false;
                pressed = false;
                Apply(false);
            }

            public void OnPointerDown(PointerEventData eventData)
            {
                pressed = true;
                Apply(hovering);
            }

            public void OnPointerUp(PointerEventData eventData)
            {
                pressed = false;
                Apply(hovering);
            }

            private void Apply(bool hovering)
            {
                if (label != null) label.color = pressed ? WarmOrange : (hovering ? Cyan : normalColor);
                if (arrow != null) arrow.gameObject.SetActive(hovering);
                transform.localScale = hovering ? Vector3.one * 1.08f : Vector3.one;
            }
        }
    }
}
