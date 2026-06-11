#if DEMO_BUILD || DEVELOPMENT_BUILD || UNITY_EDITOR
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RIMA
{
    public enum DirectorModeState
    {
        Director,
        Test
    }

    public enum DirectorTab
    {
        Spawn,
        ClassSkill,
        Stats,
        Build,
        Map,
        Telemetry
    }

    public sealed class DirectorMode : MonoBehaviour
    {
        public static DirectorMode Instance { get; private set; }

        public static event Action<DirectorModeState> OnStateChanged;
        public static event Action<DirectorTab> OnTabChanged;

        [Header("Skin")]
        [SerializeField] private TMP_FontAsset jersey10Font;
        [SerializeField] private Sprite minimapFrame;
        [SerializeField] private Sprite slotNormal;
        [SerializeField] private Sprite slotActive;
        [SerializeField] private Sprite ribbonBase;
        [SerializeField] private Sprite menuButton;
        [SerializeField] private Sprite tooltipBox;

        [Header("Free Camera")]
        [SerializeField] private float panSpeed = 8f;
        [SerializeField] private float cameraLerp = 16f;

        private readonly List<TabBinding> tabs = new List<TabBinding>();
        private readonly Dictionary<DirectorTab, CanvasGroup> panels = new Dictionary<DirectorTab, CanvasGroup>();

        private CanvasGroup rootGroup;
        private TextMeshProUGUI subtitleText;
        private TextMeshProUGUI startButtonText;
        private TextMeshProUGUI modeStripText;
        private Camera directorCamera;
        private Vector3 targetCameraPosition;
        private bool hasCameraTarget;

        public DirectorModeState State { get; private set; } = DirectorModeState.Test;
        public DirectorTab ActiveTab { get; private set; } = DirectorTab.Spawn;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            if (Instance != null)
            {
                return;
            }

            GameObject go = new GameObject("DirectorMode");
            DontDestroyOnLoad(go);
            go.AddComponent<DirectorMode>();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            LoadEditorSkin();
            BuildOverlay();
            SetState(DirectorModeState.Test);
            ShowTab(DirectorTab.Spawn);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                ToggleState();
            }

            if (State == DirectorModeState.Director)
            {
                UpdateFreeCamera(Time.unscaledDeltaTime);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void ToggleState()
        {
            SetState(State == DirectorModeState.Director ? DirectorModeState.Test : DirectorModeState.Director);
        }

        public void SetState(DirectorModeState state)
        {
            if (State == state)
            {
                ApplyStateText();
                Time.timeScale = state == DirectorModeState.Director ? 0f : 1f;
                return;
            }

            State = state;
            Time.timeScale = state == DirectorModeState.Director ? 0f : 1f;
            if (state == DirectorModeState.Director)
            {
                CacheCameraTarget();
            }

            ApplyStateText();
            OnStateChanged?.Invoke(State);
        }

        public void ShowTab(DirectorTab tab)
        {
            ActiveTab = tab;

            foreach (TabBinding binding in tabs)
            {
                bool active = binding.Tab == tab;
                if (binding.Background != null)
                {
                    binding.Background.sprite = active && slotActive != null ? slotActive : slotNormal;
                }
            }

            foreach (KeyValuePair<DirectorTab, CanvasGroup> pair in panels)
            {
                bool active = pair.Key == tab;
                CanvasGroup group = pair.Value;
                group.alpha = active ? 1f : 0f;
                group.interactable = active;
                group.blocksRaycasts = active;
            }

            OnTabChanged?.Invoke(tab);
        }

        public void StartButtonPressed()
        {
            ToggleState();
        }

        public void StepFreeCameraForValidation(Vector2 direction, float unscaledSeconds)
        {
            if (State != DirectorModeState.Director)
            {
                SetState(DirectorModeState.Director);
            }

            MoveCameraTarget(direction, Mathf.Max(0f, unscaledSeconds));
            ApplyCameraLerp(Mathf.Max(0f, unscaledSeconds));
        }

        public bool HasPanelForValidation(DirectorTab tab)
        {
            return panels.TryGetValue(tab, out CanvasGroup group) && group != null;
        }

        public bool IsTabInteractableForValidation(DirectorTab tab)
        {
            return panels.TryGetValue(tab, out CanvasGroup group) && group.alpha > 0.99f && group.interactable && group.blocksRaycasts;
        }

        private void UpdateFreeCamera(float dt)
        {
            Vector2 input = Vector2.zero;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) input.y += 1f;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) input.y -= 1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) input.x += 1f;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) input.x -= 1f;

            MoveCameraTarget(input, dt);
            ApplyCameraLerp(dt);
        }

        private void MoveCameraTarget(Vector2 direction, float dt)
        {
            CacheCameraTarget();
            if (directorCamera == null || direction.sqrMagnitude <= 0f)
            {
                return;
            }

            Vector2 normalized = direction.sqrMagnitude > 1f ? direction.normalized : direction;
            targetCameraPosition += new Vector3(normalized.x, normalized.y, 0f) * panSpeed * dt;
        }

        private void ApplyCameraLerp(float dt)
        {
            if (directorCamera == null)
            {
                return;
            }

            float t = 1f - Mathf.Exp(-cameraLerp * dt);
            directorCamera.transform.position = Vector3.Lerp(directorCamera.transform.position, targetCameraPosition, t);
        }

        private void CacheCameraTarget()
        {
            if (directorCamera == null)
            {
                directorCamera = Camera.main;
            }

            if (directorCamera != null && !hasCameraTarget)
            {
                targetCameraPosition = directorCamera.transform.position;
                hasCameraTarget = true;
            }
        }

        private void ApplyStateText()
        {
            bool director = State == DirectorModeState.Director;
            if (rootGroup != null)
            {
                rootGroup.alpha = 1f;
                rootGroup.interactable = true;
                rootGroup.blocksRaycasts = true;
            }

            if (subtitleText != null)
            {
                subtitleText.text = director ? "FREE-CAM - TIME SCALE 0" : "TEST MODE - TIME SCALE 1";
            }

            if (startButtonText != null)
            {
                startButtonText.text = director ? "BASLAT" : "DIRECTOR'A DON";
            }

            if (modeStripText != null)
            {
                modeStripText.text = director ? "PLACE / ERASE / PAINT / INSPECT" : "TEST RUNNING - PRESS ` TO RETURN";
            }
        }

        private void BuildOverlay()
        {
            GameObject canvasGo = new GameObject("Canvas_DirectorOverlay", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGo.transform.SetParent(transform, false);

            Canvas canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 950;

            CanvasScaler scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            rootGroup = CreateFill("DirectorRoot", canvasGo.transform).gameObject.AddComponent<CanvasGroup>();
            RectTransform root = rootGroup.transform as RectTransform;

            Image dimmer = CreateImage("ScreenDimmer", root, null, new Color(0.031f, 0.027f, 0.063f, 0.35f), Image.Type.Simple);
            Stretch(dimmer.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            BuildTopBadge(root);
            BuildTabRail(root);
            BuildContentArea(root);
            BuildWorldCursorOverlay(root);
            BuildMinimapMini(root);
            BuildSelectionInspector(root);
            BuildBottomTelemetryStrip(root);
        }

        private void BuildTopBadge(RectTransform root)
        {
            RectTransform badge = CreatePanel("TopBadge", root, ribbonBase, new Color(0.48f, 0.18f, 0.06f, 0.92f), Image.Type.Sliced);
            Anchor(badge, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -24f), new Vector2(620f, 84f));

            TextMeshProUGUI title = CreateText("TMP_Title", badge, "DIRECTOR MODE", 32f, FontStyles.Bold, TextAlignmentOptions.MidlineLeft);
            Anchor(title.rectTransform, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(32f, 12f), new Vector2(300f, 34f));

            subtitleText = CreateText("TMP_Subtitle", badge, "", 18f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
            subtitleText.color = new Color(0.82f, 0.92f, 0.94f, 0.86f);
            Anchor(subtitleText.rectTransform, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(34f, -18f), new Vector2(330f, 28f));

            Button start = CreateButton("Button_StartTest", badge, ribbonBase, new Color(0.80f, 0.34f, 0.09f, 1f));
            Anchor(start.GetComponent<RectTransform>(), new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(-30f, 0f), new Vector2(210f, 54f));
            start.onClick.AddListener(StartButtonPressed);

            startButtonText = CreateText("TMP", start.transform as RectTransform, "", 24f, FontStyles.Bold, TextAlignmentOptions.Center);
            Stretch(startButtonText.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        }

        private void BuildTabRail(RectTransform root)
        {
            RectTransform rail = CreatePanel("TabRail", root, null, new Color(0.05f, 0.06f, 0.08f, 0.80f), Image.Type.Simple);
            Anchor(rail, new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(0f, 0.5f), new Vector2(20f, 0f), new Vector2(96f, -180f));

            VerticalLayoutGroup layout = rail.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(8, 8, 18, 18);
            layout.spacing = 10f;
            layout.childAlignment = TextAnchor.UpperCenter;
            layout.childControlHeight = false;
            layout.childControlWidth = true;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = true;

            AddTabButton(rail, DirectorTab.Spawn, "Spawn");
            AddTabButton(rail, DirectorTab.ClassSkill, "Class&Skill");
            AddTabButton(rail, DirectorTab.Stats, "Stats");
            AddTabButton(rail, DirectorTab.Build, "Build");
            AddTabButton(rail, DirectorTab.Map, "Map");
            AddTabButton(rail, DirectorTab.Telemetry, "Telemetry");
        }

        private void AddTabButton(RectTransform parent, DirectorTab tab, string label)
        {
            Button button = CreateButton("Button_Tab_" + tab, parent, slotNormal, Color.white);
            RectTransform rt = button.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(80f, 76f);

            LayoutElement layout = button.gameObject.AddComponent<LayoutElement>();
            layout.preferredHeight = 76f;
            layout.minHeight = 76f;

            TextMeshProUGUI text = CreateText("TMP_Label", rt, label, 14f, FontStyles.Bold, TextAlignmentOptions.Center);
            Stretch(text.rectTransform, Vector2.zero, Vector2.one, new Vector2(4f, 8f), new Vector2(-4f, -8f));
            text.enableWordWrapping = true;

            Image icon = CreateImage("Icon", rt, null, new Color(1f, 0.55f, 0.18f, 0.35f), Image.Type.Simple);
            Anchor(icon.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -10f), new Vector2(18f, 4f));

            button.onClick.AddListener(() => ShowTab(tab));
            tabs.Add(new TabBinding(tab, button.GetComponent<Image>()));
        }

        private void BuildContentArea(RectTransform root)
        {
            RectTransform content = CreateFill("ContentArea", root);
            Stretch(content, Vector2.zero, Vector2.one, new Vector2(150f, 120f), new Vector2(-40f, -130f));

            AddEmptyPanel(content, DirectorTab.Spawn, "SPAWN", "yakinda");
            AddEmptyPanel(content, DirectorTab.ClassSkill, "CLASS & SKILL", "yakinda");
            AddEmptyPanel(content, DirectorTab.Stats, "STATS", "yakinda");
            AddEmptyPanel(content, DirectorTab.Build, "BUILD", "yakinda");
            AddEmptyPanel(content, DirectorTab.Map, "MAP", "yakinda");
            AddEmptyPanel(content, DirectorTab.Telemetry, "TELEMETRY", "yakinda");
        }

        private void AddEmptyPanel(RectTransform parent, DirectorTab tab, string title, string placeholder)
        {
            RectTransform panel = CreateFill("Panel_" + tab, parent);
            CanvasGroup group = panel.gameObject.AddComponent<CanvasGroup>();
            panels[tab] = group;

            RectTransform window = CreatePanel("Window", panel, minimapFrame, Color.white, Image.Type.Sliced);
            Stretch(window, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            RectTransform header = CreateFill("Header", panel);
            Anchor(header, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -26f), new Vector2(-64f, 74f));

            TextMeshProUGUI titleText = CreateText("TMP_Title", header, title, 30f, FontStyles.Bold, TextAlignmentOptions.MidlineLeft);
            Stretch(titleText.rectTransform, Vector2.zero, Vector2.one, new Vector2(34f, 26f), new Vector2(-34f, 0f));

            TextMeshProUGUI hint = CreateText("TMP_Hint", header, placeholder, 20f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
            hint.color = new Color(0.78f, 0.84f, 0.86f, 0.72f);
            Stretch(hint.rectTransform, Vector2.zero, Vector2.one, new Vector2(34f, 0f), new Vector2(-34f, -30f));

            TextMeshProUGUI soon = CreateText("TMP_Placeholder", panel, placeholder, 26f, FontStyles.Normal, TextAlignmentOptions.Center);
            soon.color = new Color(0.82f, 0.92f, 0.94f, 0.58f);
            Anchor(soon.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(360f, 80f));

            TextMeshProUGUI footer = CreateText("FooterModeText", panel, "PLACE / ERASE / PAINT / INSPECT", 18f, FontStyles.Bold, TextAlignmentOptions.Center);
            footer.color = new Color(1f, 0.55f, 0.18f, 0.88f);
            Anchor(footer.rectTransform, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 22f), new Vector2(-80f, 28f));
        }

        private void BuildWorldCursorOverlay(RectTransform root)
        {
            RectTransform overlay = CreateFill("WorldCursorOverlay", root);
            Stretch(overlay, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            overlay.gameObject.SetActive(false);
            CreateFill("GhostPreview", overlay);
            CreateFill("BrushCircle", overlay);
            CreateFill("GridCellHighlight", overlay);
        }

        private void BuildMinimapMini(RectTransform root)
        {
            RectTransform minimap = CreatePanel("MinimapMini", root, minimapFrame, Color.white, Image.Type.Sliced);
            Anchor(minimap, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-28f, -28f), new Vector2(180f, 180f));
            CreateFill("NodeGraphMini", minimap);
        }

        private void BuildSelectionInspector(RectTransform root)
        {
            RectTransform inspector = CreatePanel("SelectionInspector", root, tooltipBox, Color.white, Image.Type.Sliced);
            Anchor(inspector, new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-28f, 112f), new Vector2(260f, 130f));

            TextMeshProUGUI name = CreateText("TMP_Name", inspector, "NO SELECTION", 18f, FontStyles.Bold, TextAlignmentOptions.MidlineLeft);
            Stretch(name.rectTransform, Vector2.zero, Vector2.one, new Vector2(18f, 70f), new Vector2(-18f, -16f));

            TextMeshProUGUI stats = CreateText("TMP_RuntimeStats", inspector, "ID / HP / AI", 14f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
            stats.color = new Color(0.78f, 0.84f, 0.86f, 0.70f);
            Stretch(stats.rectTransform, Vector2.zero, Vector2.one, new Vector2(18f, 36f), new Vector2(-18f, -42f));

            RectTransform buttons = CreateFill("Buttons", inspector);
            Anchor(buttons, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 18f), new Vector2(-24f, 26f));
        }

        private void BuildBottomTelemetryStrip(RectTransform root)
        {
            RectTransform strip = CreatePanel("BottomTelemetryStrip", root, menuButton, Color.white, Image.Type.Sliced);
            Anchor(strip, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 20f), new Vector2(860f, 42f));

            modeStripText = CreateText("TMP_Mode", strip, "", 18f, FontStyles.Bold, TextAlignmentOptions.Center);
            Stretch(modeStripText.rectTransform, Vector2.zero, Vector2.one, new Vector2(18f, 0f), new Vector2(-180f, 0f));

            Button reset = CreateButton("Button_QuickReset", strip, menuButton, Color.white);
            Anchor(reset.GetComponent<RectTransform>(), new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(-12f, 0f), new Vector2(150f, 28f));
            TextMeshProUGUI label = CreateText("TMP", reset.transform as RectTransform, "QUICK RESET", 16f, FontStyles.Bold, TextAlignmentOptions.Center);
            Stretch(label.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        }

        private RectTransform CreateFill(string name, Transform parent)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            RectTransform rt = go.GetComponent<RectTransform>();
            Stretch(rt, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            return rt;
        }

        private RectTransform CreatePanel(string name, Transform parent, Sprite sprite, Color color, Image.Type type)
        {
            Image image = CreateImage(name, parent, sprite, color, type);
            return image.rectTransform;
        }

        private Image CreateImage(string name, Transform parent, Sprite sprite, Color color, Image.Type type)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            Image image = go.GetComponent<Image>();
            image.sprite = sprite;
            image.type = sprite != null ? type : Image.Type.Simple;
            image.color = color;
            return image;
        }

        private Button CreateButton(string name, Transform parent, Sprite sprite, Color color)
        {
            Image image = CreateImage(name, parent, sprite, color, sprite != null ? Image.Type.Sliced : Image.Type.Simple);
            Button button = image.gameObject.AddComponent<Button>();
            button.transition = Selectable.Transition.ColorTint;
            button.targetGraphic = image;
            return button;
        }

        private TextMeshProUGUI CreateText(string name, RectTransform parent, string text, float size, FontStyles style, TextAlignmentOptions alignment)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.font = jersey10Font;
            tmp.fontSize = size;
            tmp.fontStyle = style;
            tmp.alignment = alignment;
            tmp.color = new Color(0.96f, 0.94f, 0.86f, 1f);
            tmp.raycastTarget = false;
            return tmp;
        }

        private static void Anchor(RectTransform rt, Vector2 min, Vector2 max, Vector2 pivot, Vector2 anchoredPosition, Vector2 sizeDelta)
        {
            rt.anchorMin = min;
            rt.anchorMax = max;
            rt.pivot = pivot;
            rt.anchoredPosition = anchoredPosition;
            rt.sizeDelta = sizeDelta;
        }

        private static void Stretch(RectTransform rt, Vector2 min, Vector2 max, Vector2 offsetMin, Vector2 offsetMax)
        {
            rt.anchorMin = min;
            rt.anchorMax = max;
            rt.offsetMin = offsetMin;
            rt.offsetMax = offsetMax;
        }

        private void LoadEditorSkin()
        {
#if UNITY_EDITOR
            jersey10Font = jersey10Font != null ? jersey10Font : AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Fonts/Jersey10/Jersey10-Regular SDF.asset");
            minimapFrame = minimapFrame != null ? minimapFrame : AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Chrome/minimap_frame.png");
            slotNormal = slotNormal != null ? slotNormal : AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Chrome/slot_normal.png");
            slotActive = slotActive != null ? slotActive : AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Chrome/slot_active.png");
            ribbonBase = ribbonBase != null ? ribbonBase : AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Chrome/ribbon_base.png");
            menuButton = menuButton != null ? menuButton : AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Chrome/menu_button.png");
            tooltipBox = tooltipBox != null ? tooltipBox : AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Chrome/tooltip_box.png");
#endif
        }

        private readonly struct TabBinding
        {
            public readonly DirectorTab Tab;
            public readonly Image Background;

            public TabBinding(DirectorTab tab, Image background)
            {
                Tab = tab;
                Background = background;
            }
        }
    }
}
#endif
