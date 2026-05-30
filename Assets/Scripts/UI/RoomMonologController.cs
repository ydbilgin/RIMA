using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using RIMA.Systems.Map;

namespace RIMA
{
    public class RoomMonologController : MonoBehaviour
    {
        private const float CharsPerSecond = 30f;
        private const float LineHoldSeconds = 3f;
        private const float LineFadeSeconds = 1f;

        private static readonly Color MonologCyan = new Color(0.282f, 0.878f, 1f, 1f); // #48E0FF
        private static RoomMonologController instance;

        private readonly HashSet<int> shownRooms = new HashSet<int>();
        private CanvasGroup lineGroup;
        private CanvasGroup titleGroup;
        private TextMeshProUGUI lineText;
        private TextMeshProUGUI titleText;
        private TextMeshProUGUI subtitleText;
        private Coroutine lineRoutine;
        private Coroutine titleRoutine;
        private int skipToken;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            EnsureInstance();
        }

        public static void Say(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            EnsureInstance().ShowLine(text);
        }

        private static RoomMonologController EnsureInstance()
        {
            if (instance != null) return instance;

            instance = FindFirstObjectByType<RoomMonologController>(FindObjectsInactive.Include);
            if (instance != null) return instance;

            var go = new GameObject("RoomMonologController_Auto");
            instance = go.AddComponent<RoomMonologController>();
            return instance;
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
            BuildUI();
        }

        private void OnEnable()
        {
            RoomLoader.OnRoomChanged += HandleRoomChanged;
        }

        private void OnDisable()
        {
            RoomLoader.OnRoomChanged -= HandleRoomChanged;
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
                skipToken++;
        }

        private void HandleRoomChanged(int zeroBasedIndex)
        {
            RoomSequenceData data = RoomLoader.CurrentRoomData;
            int roomNumber = data != null && data.roomIndex == zeroBasedIndex
                ? data.roomIndex + 1
                : zeroBasedIndex + 1;
            if (!shownRooms.Add(roomNumber)) return;

            string line = LineForRoom(roomNumber);
            if (!string.IsNullOrEmpty(line)) ShowLine(line);

            if (roomNumber == 5)
            {
                ShowTitle(
                    "THE PENITENT SOVEREIGN",
                    "He took the wound so the seal would hold.");
            }
        }

        private static string LineForRoom(int roomNumber)
        {
            return roomNumber switch
            {
                2 => "Someone kept this fire. Long after the order fell.",
                3 => "The chains here lead somewhere. They always have.",
                4 => "You knew this once.",
                5 => "The Sovereign's breath is colder here.",
                _ => null
            };
        }

        private void ShowLine(string text)
        {
            if (lineRoutine != null) StopCoroutine(lineRoutine);
            lineRoutine = StartCoroutine(LineSequence(text));
        }

        private void ShowTitle(string main, string subtitle)
        {
            if (titleRoutine != null) StopCoroutine(titleRoutine);
            titleRoutine = StartCoroutine(TitleSequence(main, subtitle));
        }

        private IEnumerator LineSequence(string text)
        {
            int token = skipToken;
            lineText.text = string.Empty;
            lineGroup.gameObject.SetActive(true);
            yield return Fade(lineGroup, 0f, 1f, 0.15f, token);

            float elapsed = 0f;
            while (elapsed * CharsPerSecond < text.Length && !Skipped(token))
            {
                elapsed += Time.unscaledDeltaTime;
                int count = Mathf.Clamp(Mathf.FloorToInt(elapsed * CharsPerSecond), 0, text.Length);
                lineText.text = text.Substring(0, count);
                yield return null;
            }

            lineText.text = text;
            yield return Hold(LineHoldSeconds, token);
            yield return Fade(lineGroup, lineGroup.alpha, 0f, LineFadeSeconds, token);
            lineGroup.gameObject.SetActive(false);
            lineRoutine = null;
        }

        private IEnumerator TitleSequence(string main, string subtitle)
        {
            int token = skipToken;
            titleText.text = main;
            subtitleText.text = subtitle;
            titleGroup.gameObject.SetActive(true);
            yield return Fade(titleGroup, 0f, 1f, 0.45f, token);
            yield return Hold(1.8f, token);
            yield return Fade(titleGroup, titleGroup.alpha, 0f, 0.8f, token);
            titleGroup.gameObject.SetActive(false);
            titleRoutine = null;
        }

        private IEnumerator Hold(float seconds, int token)
        {
            float elapsed = 0f;
            while (elapsed < seconds && !Skipped(token))
            {
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        private IEnumerator Fade(CanvasGroup group, float from, float to, float seconds, int token)
        {
            float elapsed = 0f;
            group.alpha = from;
            while (elapsed < seconds && !Skipped(token))
            {
                elapsed += Time.unscaledDeltaTime;
                group.alpha = Mathf.Lerp(from, to, Mathf.Clamp01(elapsed / seconds));
                yield return null;
            }
            group.alpha = Skipped(token) ? 0f : to;
        }

        private bool Skipped(int token) => skipToken != token;

        private void BuildUI()
        {
            var canvas = gameObject.GetComponent<Canvas>() ?? gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1030;

            var scaler = gameObject.GetComponent<CanvasScaler>() ?? gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(640f, 360f);
            scaler.matchWidthOrHeight = 0.5f;

            if (gameObject.GetComponent<GraphicRaycaster>() == null)
                gameObject.AddComponent<GraphicRaycaster>();

            RectTransform root = GetComponent<RectTransform>();
            root.anchorMin = Vector2.zero;
            root.anchorMax = Vector2.one;
            root.offsetMin = Vector2.zero;
            root.offsetMax = Vector2.zero;

            lineGroup = CreateGroup("RoomMonolog_Line", root);
            RectTransform linePanel = CreatePanel("Panel", lineGroup.transform, new Vector2(540f, 46f));
            linePanel.anchorMin = linePanel.anchorMax = new Vector2(0.5f, 0f);
            linePanel.pivot = new Vector2(0.5f, 0f);
            linePanel.anchoredPosition = new Vector2(0f, 34f);

            lineText = CreateText("Text", linePanel, 13f, MonologCyan, TextAlignmentOptions.Center);
            Stretch(lineText.rectTransform, 24f, 8f);

            titleGroup = CreateGroup("RoomMonolog_Title", root);
            RectTransform titlePanel = CreatePanel("Panel", titleGroup.transform, new Vector2(520f, 124f));
            titlePanel.anchorMin = titlePanel.anchorMax = new Vector2(0.5f, 0.5f);
            titlePanel.pivot = new Vector2(0.5f, 0.5f);
            titlePanel.anchoredPosition = Vector2.zero;

            titleText = CreateText("Title", titlePanel, 26f, RimaUITheme.TextPrimary, TextAlignmentOptions.Center);
            titleText.fontStyle = FontStyles.Bold;
            titleText.rectTransform.anchorMin = new Vector2(0f, 0.48f);
            titleText.rectTransform.anchorMax = new Vector2(1f, 0.9f);
            titleText.rectTransform.offsetMin = new Vector2(20f, 0f);
            titleText.rectTransform.offsetMax = new Vector2(-20f, 0f);

            subtitleText = CreateText("Subtitle", titlePanel, 14f, MonologCyan, TextAlignmentOptions.Center);
            subtitleText.rectTransform.anchorMin = new Vector2(0f, 0.18f);
            subtitleText.rectTransform.anchorMax = new Vector2(1f, 0.48f);
            subtitleText.rectTransform.offsetMin = new Vector2(20f, 0f);
            subtitleText.rectTransform.offsetMax = new Vector2(-20f, 0f);

            lineGroup.gameObject.SetActive(false);
            titleGroup.gameObject.SetActive(false);
        }

        private static CanvasGroup CreateGroup(string name, Transform parent)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasGroup));
            go.transform.SetParent(parent, false);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            CanvasGroup group = go.GetComponent<CanvasGroup>();
            group.alpha = 0f;
            group.interactable = false;
            group.blocksRaycasts = false;
            return group;
        }

        private static RectTransform CreatePanel(string name, Transform parent, Vector2 size)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = size;

            Image image = go.GetComponent<Image>();
            image.sprite = CreateGradientSprite();
            image.type = Image.Type.Sliced;
            image.color = Color.white;
            image.raycastTarget = false;
            return rt;
        }

        private static TextMeshProUGUI CreateText(string name, Transform parent, float size, Color color, TextAlignmentOptions alignment)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.fontSize = size;
            tmp.color = color;
            tmp.alignment = alignment;
            tmp.raycastTarget = false;
            tmp.textWrappingMode = TextWrappingModes.Normal;
            return tmp;
        }

        private static void Stretch(RectTransform rt, float xPad, float yPad)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(xPad, yPad);
            rt.offsetMax = new Vector2(-xPad, -yPad);
        }

        private static Sprite CreateGradientSprite()
        {
            const int width = 8;
            const int height = 32;
            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };

            for (int y = 0; y < height; y++)
            {
                float center = 1f - Mathf.Abs((y / (height - 1f)) * 2f - 1f);
                Color c = Color.Lerp(
                    new Color(0.02f, 0.025f, 0.04f, 0.08f),
                    new Color(0.02f, 0.025f, 0.04f, 0.58f),
                    center);

                for (int x = 0; x < width; x++)
                    tex.SetPixel(x, y, c);
            }

            tex.Apply(false, true);
            return Sprite.Create(tex, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 100f, 0,
                SpriteMeshType.FullRect, new Vector4(2f, 12f, 2f, 12f));
        }
    }
}
