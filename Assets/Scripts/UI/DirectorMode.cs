#if DEMO_BUILD || DEVELOPMENT_BUILD || UNITY_EDITOR
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using RIMA.Balance;

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
        [SerializeField] private Sprite rewardCard;
        [SerializeField] private Sprite ribbonBase;
        [SerializeField] private Sprite menuButton;
        [SerializeField] private Sprite tooltipBox;

        [Header("Free Camera")]
        [SerializeField] private float panSpeed = 8f;
        [SerializeField] private float cameraLerp = 16f;

        private readonly List<TabBinding> tabs = new List<TabBinding>();
        private readonly Dictionary<DirectorTab, CanvasGroup> panels = new Dictionary<DirectorTab, CanvasGroup>();
        private readonly List<StatSliderBinding> statSliders = new List<StatSliderBinding>();
        private readonly List<ClassButtonBinding> classButtons = new List<ClassButtonBinding>();
        private readonly List<SkillCardBinding> skillCards = new List<SkillCardBinding>();
        private readonly List<LocalizedTextBinding> localizedTexts = new List<LocalizedTextBinding>();

        private CanvasGroup rootGroup;
        private TextMeshProUGUI subtitleText;
        private TextMeshProUGUI startButtonText;
        private TextMeshProUGUI modeStripText;
        private TextMeshProUGUI classSkillStatusText;
        private TextMeshProUGUI statsStatusText;
        private RectTransform classSkillCardsRoot;
        private SkillData selectedDirectorSkill;
        private Camera directorCamera;
        private Vector3 targetCameraPosition;
        private bool hasCameraTarget;
        private bool suppressStatSliderCallbacks;

        public DirectorModeState State { get; private set; } = DirectorModeState.Test;
        public DirectorTab ActiveTab { get; private set; } = DirectorTab.Spawn;

        private static readonly ClassType[] DirectorClasses =
        {
            ClassType.Warblade,
            ClassType.Elementalist,
            ClassType.Shadowblade,
            ClassType.Ranger,
            ClassType.Ravager,
            ClassType.Ronin,
            ClassType.Gunslinger,
            ClassType.Brawler,
            ClassType.Summoner,
            ClassType.Hexer
        };

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
            Loc.OnLanguageChanged += RefreshLocalizedText;
            RefreshLocalizedText();
            SetState(DirectorModeState.Test);
            ShowTab(DirectorTab.Spawn);
        }

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.backquoteKey.wasPressedThisFrame)
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

            Loc.OnLanguageChanged -= RefreshLocalizedText;
        }

        private void OnEnable()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            if (rootGroup == null || panels.Count == 0 || tabs.Count == 0)
            {
                RebuildOverlayRuntime();
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

            if (tab == DirectorTab.Stats)
            {
                RefreshStatsSlidersFromRuntime();
            }
            else if (tab == DirectorTab.ClassSkill)
            {
                RefreshClassSkillPanel();
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

        public void SelectClassForValidation(ClassType type)
        {
            SelectDirectorClass(type);
        }

        public bool AssignSkillForValidation(string skillName, int slot)
        {
            SkillData skill = FindDirectorSkill(skillName);
            if (skill == null)
            {
                SetClassSkillStatus("director.class_skill.status.skill_missing", skillName);
                return false;
            }

            selectedDirectorSkill = skill;
            return AssignSelectedSkillToSlot(slot);
        }

        public bool AssignBasicAttackButtonsForValidation()
        {
            return AssignBasicAction(GameAction.Attack, "<Mouse>/leftButton")
                & AssignBasicAction(GameAction.ClassSecondary, "<Mouse>/rightButton");
        }

        private void UpdateFreeCamera(float dt)
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            Vector2 input = Vector2.zero;
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) input.y += 1f;
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) input.y -= 1f;
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) input.x += 1f;
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) input.x -= 1f;

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

        private void RebuildOverlayRuntime()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                if (child.name == "Canvas_DirectorOverlay")
                {
                    DestroyImmediate(child.gameObject);
                }
            }

            tabs.Clear();
            panels.Clear();
            statSliders.Clear();
            classButtons.Clear();
            skillCards.Clear();
            localizedTexts.Clear();
            rootGroup = null;
            classSkillStatusText = null;
            statsStatusText = null;
            classSkillCardsRoot = null;
            selectedDirectorSkill = null;

            LoadEditorSkin();
            BuildOverlay();
            Loc.OnLanguageChanged -= RefreshLocalizedText;
            Loc.OnLanguageChanged += RefreshLocalizedText;
            RefreshLocalizedText();
            ApplyStateText();
            ShowTab(ActiveTab);
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
            AddClassSkillPanel(content);
            AddStatsPanel(content);
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

        private void AddClassSkillPanel(RectTransform parent)
        {
            RectTransform panel = CreateFill("Panel_" + DirectorTab.ClassSkill, parent);
            CanvasGroup group = panel.gameObject.AddComponent<CanvasGroup>();
            panels[DirectorTab.ClassSkill] = group;

            RectTransform window = CreatePanel("Window", panel, minimapFrame, Color.white, Image.Type.Sliced);
            Stretch(window, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            RectTransform header = CreateFill("Header", panel);
            Anchor(header, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -26f), new Vector2(-64f, 74f));

            TextMeshProUGUI titleText = CreateText("TMP_Title", header, Loc.T("director.class_skill.title"), 30f, FontStyles.Bold, TextAlignmentOptions.MidlineLeft);
            Stretch(titleText.rectTransform, Vector2.zero, Vector2.one, new Vector2(34f, 26f), new Vector2(-34f, 0f));
            localizedTexts.Add(new LocalizedTextBinding(titleText, "director.class_skill.title"));

            TextMeshProUGUI hint = CreateText("TMP_Hint", header, Loc.T("director.class_skill.hint"), 20f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
            hint.color = new Color(0.78f, 0.84f, 0.86f, 0.72f);
            Stretch(hint.rectTransform, Vector2.zero, Vector2.one, new Vector2(34f, 0f), new Vector2(-34f, -30f));
            localizedTexts.Add(new LocalizedTextBinding(hint, "director.class_skill.hint"));

            RectTransform classGrid = CreatePanel("ClassGrid", panel, null, new Color(0.02f, 0.025f, 0.035f, 0.28f), Image.Type.Simple);
            Anchor(classGrid, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(42f, -132f), new Vector2(520f, 288f));
            GridLayoutGroup classLayout = classGrid.gameObject.AddComponent<GridLayoutGroup>();
            classLayout.padding = new RectOffset(14, 14, 14, 14);
            classLayout.spacing = new Vector2(10f, 10f);
            classLayout.cellSize = new Vector2(92f, 74f);
            classLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            classLayout.constraintCount = 5;

            for (int i = 0; i < DirectorClasses.Length; i++)
                AddClassButton(classGrid, DirectorClasses[i]);

            classSkillCardsRoot = CreatePanel("SkillCards", panel, null, new Color(0.02f, 0.025f, 0.035f, 0.28f), Image.Type.Simple);
            Anchor(classSkillCardsRoot, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, 1f), new Vector2(590f, -132f), new Vector2(-90f, 410f));
            GridLayoutGroup skillLayout = classSkillCardsRoot.gameObject.AddComponent<GridLayoutGroup>();
            skillLayout.padding = new RectOffset(16, 16, 16, 16);
            skillLayout.spacing = new Vector2(12f, 12f);
            skillLayout.cellSize = new Vector2(218f, 92f);
            skillLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            skillLayout.constraintCount = 3;

            RectTransform actions = CreateFill("ClassSkillActions", panel);
            Anchor(actions, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(42f, 78f), new Vector2(990f, 54f));

            AddSkillSlotButton(actions, "Button_AssignQ", "director.class_skill.assign.q", 0, new Vector2(0f, 0f));
            AddSkillSlotButton(actions, "Button_AssignE", "director.class_skill.assign.e", 1, new Vector2(122f, 0f));
            AddSkillSlotButton(actions, "Button_AssignR", "director.class_skill.assign.r", 2, new Vector2(244f, 0f));
            AddSkillSlotButton(actions, "Button_AssignF", "director.class_skill.assign.f", 3, new Vector2(366f, 0f));
            AddBasicButton(actions, "Button_LMB", "director.class_skill.assign.lmb", GameAction.Attack, "<Mouse>/leftButton", new Vector2(520f, 0f));
            AddBasicButton(actions, "Button_RMB", "director.class_skill.assign.rmb", GameAction.ClassSecondary, "<Mouse>/rightButton", new Vector2(674f, 0f));

            classSkillStatusText = CreateText("TMP_Status", panel, "", 18f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
            classSkillStatusText.color = new Color(0.78f, 0.84f, 0.86f, 0.72f);
            Anchor(classSkillStatusText.rectTransform, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0.5f, 0f), new Vector2(42f, 32f), new Vector2(-120f, 30f));

            RefreshClassSkillPanel();
        }

        private void AddStatsPanel(RectTransform parent)
        {
            RectTransform panel = CreateFill("Panel_" + DirectorTab.Stats, parent);
            CanvasGroup group = panel.gameObject.AddComponent<CanvasGroup>();
            panels[DirectorTab.Stats] = group;

            RectTransform window = CreatePanel("Window", panel, minimapFrame, Color.white, Image.Type.Sliced);
            Stretch(window, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            RectTransform header = CreateFill("Header", panel);
            Anchor(header, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -26f), new Vector2(-64f, 74f));

            TextMeshProUGUI titleText = CreateText("TMP_Title", header, Loc.T("director.stats.title"), 30f, FontStyles.Bold, TextAlignmentOptions.MidlineLeft);
            Stretch(titleText.rectTransform, Vector2.zero, Vector2.one, new Vector2(34f, 26f), new Vector2(-34f, 0f));
            localizedTexts.Add(new LocalizedTextBinding(titleText, "director.stats.title"));

            TextMeshProUGUI hint = CreateText("TMP_Hint", header, Loc.T("director.stats.hint"), 20f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
            hint.color = new Color(0.78f, 0.84f, 0.86f, 0.72f);
            Stretch(hint.rectTransform, Vector2.zero, Vector2.one, new Vector2(34f, 0f), new Vector2(-34f, -30f));
            localizedTexts.Add(new LocalizedTextBinding(hint, "director.stats.hint"));

            RectTransform rows = CreatePanel("StatsRows", panel, null, new Color(0.02f, 0.025f, 0.035f, 0.28f), Image.Type.Simple);
            Anchor(rows, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(42f, -132f), new Vector2(860f, 390f));

            VerticalLayoutGroup layout = rows.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(18, 18, 18, 18);
            layout.spacing = 12f;
            layout.childAlignment = TextAnchor.UpperLeft;
            layout.childControlHeight = false;
            layout.childControlWidth = true;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = true;

            AddStatRow(rows, "maxHP", "director.stats.maxHP", 1f, 300f, true, HtmlColor("#C82026"), s => s.maxHP, (s, v) => s.maxHP = Mathf.RoundToInt(v));
            AddStatRow(rows, "physPower", "director.stats.physPower", 0f, 300f, false, HtmlColor("#E89020"), s => s.physPower, (s, v) => s.physPower = v);
            AddStatRow(rows, "abilityPower", "director.stats.abilityPower", 0f, 300f, false, HtmlColor("#E89020"), s => s.abilityPower, (s, v) => s.abilityPower = v);
            AddStatRow(rows, "attackSpeedMult", "director.stats.attackSpeedMult", 0.25f, 3f, false, HtmlColor("#00FFCC"), s => s.attackSpeedMult, (s, v) => s.attackSpeedMult = v);
            AddStatRow(rows, "moveSpeed", "director.stats.moveSpeed", 0f, 12f, false, HtmlColor("#00FFCC"), s => s.moveSpeed, (s, v) => s.moveSpeed = v);
            AddStatRow(rows, "debugGlobalDamageMult", "director.stats.debugGlobalDamageMult", 0f, 5f, false, HtmlColor("#E89020"), s => s.debugGlobalDamageMult, (s, v) => s.debugGlobalDamageMult = v);

            RectTransform actions = CreateFill("StatsActions", panel);
            Anchor(actions, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(42f, 74f), new Vector2(640f, 54f));

            Button reset = CreateButton("Button_ResetStats", actions, menuButton, Color.white);
            Anchor(reset.GetComponent<RectTransform>(), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0f), new Vector2(150f, 42f));
            reset.onClick.AddListener(ResetStatsFromProfile);
            AddButtonText(reset, "director.stats.reset");

            Button save = CreateButton("Button_SaveStats", actions, menuButton, Color.white);
            Anchor(save.GetComponent<RectTransform>(), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(170f, 0f), new Vector2(150f, 42f));
            save.onClick.AddListener(SaveStatsPreset);
            AddButtonText(save, "director.stats.save");

            Button export = CreateButton("Button_ExportStats", actions, ribbonBase, new Color(0.80f, 0.34f, 0.09f, 1f));
            Anchor(export.GetComponent<RectTransform>(), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(340f, 0f), new Vector2(200f, 48f));
            export.onClick.AddListener(ExportStatsJson);
            AddButtonText(export, "director.stats.export");

            statsStatusText = CreateText("TMP_Status", panel, "", 18f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
            statsStatusText.color = new Color(0.78f, 0.84f, 0.86f, 0.72f);
            Anchor(statsStatusText.rectTransform, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0.5f, 0f), new Vector2(42f, 32f), new Vector2(-120f, 30f));

            RefreshStatsSlidersFromRuntime();
        }

        private void AddStatRow(RectTransform parent, string statKey, string locKey, float minValue, float maxValue, bool wholeNumbers, Color color, Func<ClassStatRuntime, float> read, Action<ClassStatRuntime, float> write)
        {
            RectTransform row = CreateFill("Row_" + statKey, parent);
            row.sizeDelta = new Vector2(0f, 44f);
            LayoutElement layout = row.gameObject.AddComponent<LayoutElement>();
            layout.preferredHeight = 44f;
            layout.minHeight = 44f;

            TextMeshProUGUI label = CreateText("TMP_Label", row, Loc.T(locKey), 20f, FontStyles.Bold, TextAlignmentOptions.MidlineLeft);
            label.color = color;
            Anchor(label.rectTransform, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0f), new Vector2(240f, 34f));
            localizedTexts.Add(new LocalizedTextBinding(label, locKey));

            Slider slider = CreateStatSlider(row, statKey, minValue, maxValue, wholeNumbers, color);
            Anchor(slider.GetComponent<RectTransform>(), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(260f, 0f), new Vector2(410f, 32f));

            TextMeshProUGUI valueText = CreateText("TMP_Value", row, "", 20f, FontStyles.Bold, TextAlignmentOptions.MidlineRight);
            Anchor(valueText.rectTransform, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(690f, 0f), new Vector2(110f, 34f));

            StatSliderBinding binding = new StatSliderBinding(statKey, slider, valueText, read, write);
            statSliders.Add(binding);
            slider.onValueChanged.AddListener(value => OnStatSliderChanged(binding, value));
        }

        private Slider CreateStatSlider(RectTransform parent, string statKey, float minValue, float maxValue, bool wholeNumbers, Color fillColor)
        {
            RectTransform root = CreateFill("Slider_" + statKey, parent);

            Image background = CreateImage("Background", root, null, new Color(0.06f, 0.07f, 0.09f, 0.95f), Image.Type.Simple);
            Stretch(background.rectTransform, Vector2.zero, Vector2.one, new Vector2(0f, 10f), new Vector2(0f, -10f));

            RectTransform fillArea = CreateFill("Fill Area", root);
            Stretch(fillArea, Vector2.zero, Vector2.one, new Vector2(0f, 10f), new Vector2(0f, -10f));
            Image fill = CreateImage("Fill", fillArea, null, fillColor, Image.Type.Simple);
            Stretch(fill.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            RectTransform handleArea = CreateFill("Handle Slide Area", root);
            Stretch(handleArea, Vector2.zero, Vector2.one, new Vector2(0f, 2f), new Vector2(0f, -2f));
            Image handle = CreateImage("Handle", handleArea, null, new Color(0.96f, 0.94f, 0.86f, 1f), Image.Type.Simple);
            Anchor(handle.rectTransform, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(18f, 28f));

            Slider slider = root.gameObject.AddComponent<Slider>();
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.wholeNumbers = wholeNumbers;
            slider.targetGraphic = handle;
            slider.fillRect = fill.rectTransform;
            slider.handleRect = handle.rectTransform;
            slider.direction = Slider.Direction.LeftToRight;
            return slider;
        }

        private void AddButtonText(Button button, string locKey)
        {
            TextMeshProUGUI label = CreateText("TMP", button.transform as RectTransform, Loc.T(locKey), 18f, FontStyles.Bold, TextAlignmentOptions.Center);
            Stretch(label.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            localizedTexts.Add(new LocalizedTextBinding(label, locKey));
        }

        private void AddClassButton(RectTransform parent, ClassType type)
        {
            Button button = CreateButton("Button_Class_" + type, parent, slotNormal, Color.white);
            TextMeshProUGUI label = CreateText("TMP_Label", button.transform as RectTransform, type.ToString(), 15f, FontStyles.Bold, TextAlignmentOptions.Center);
            Stretch(label.rectTransform, Vector2.zero, Vector2.one, new Vector2(5f, 6f), new Vector2(-5f, -6f));
            label.enableWordWrapping = true;

            button.onClick.AddListener(() => SelectDirectorClass(type));
            classButtons.Add(new ClassButtonBinding(type, button.GetComponent<Image>()));
        }

        private void AddSkillSlotButton(RectTransform parent, string name, string locKey, int slot, Vector2 position)
        {
            Button button = CreateButton(name, parent, menuButton, Color.white);
            Anchor(button.GetComponent<RectTransform>(), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), position, new Vector2(108f, 42f));
            button.onClick.AddListener(() => AssignSelectedSkillToSlot(slot));
            AddButtonText(button, locKey);
        }

        private void AddBasicButton(RectTransform parent, string name, string locKey, GameAction action, string bindingPath, Vector2 position)
        {
            Button button = CreateButton(name, parent, ribbonBase, new Color(0.80f, 0.34f, 0.09f, 1f));
            Anchor(button.GetComponent<RectTransform>(), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), position, new Vector2(138f, 46f));
            button.onClick.AddListener(() => AssignBasicAction(action, bindingPath));
            AddButtonText(button, locKey);
        }

        private void SelectDirectorClass(ClassType type)
        {
            PlayerClassManager manager = PlayerClassManager.Instance;
            if (manager == null)
            {
                SetClassSkillStatus("director.class_skill.status.no_manager");
                return;
            }

            PlayerClassManager.DirectorBypassClassUnlock = true;
            try
            {
                manager.SetPrimaryClass(type);
            }
            finally
            {
                PlayerClassManager.DirectorBypassClassUnlock = false;
            }

            selectedDirectorSkill = null;
            RefreshClassSkillPanel();
            SetClassSkillStatus("director.class_skill.status.class", type);
        }

        private void RefreshClassSkillPanel()
        {
            RefreshClassButtons();
            RebuildSkillCards();
        }

        private void RefreshClassButtons()
        {
            ClassType active = PlayerClassManager.Instance != null ? PlayerClassManager.Instance.PrimaryClass : ClassType.None;
            foreach (ClassButtonBinding binding in classButtons)
            {
                if (binding.Background != null)
                    binding.Background.sprite = binding.Type == active && slotActive != null ? slotActive : slotNormal;
            }
        }

        private void RebuildSkillCards()
        {
            if (classSkillCardsRoot == null) return;

            for (int i = classSkillCardsRoot.childCount - 1; i >= 0; i--)
                DestroyRuntimeObject(classSkillCardsRoot.GetChild(i).gameObject);
            skillCards.Clear();

            EnsureSkillDatabase();
            ClassType active = PlayerClassManager.Instance != null ? PlayerClassManager.Instance.PrimaryClass : ClassType.Warblade;
            List<SkillData> all = SkillDatabase.Instance != null ? SkillDatabase.Instance.GetAll() : null;
            if (all == null) return;

            int count = 0;
            for (int i = 0; i < all.Count; i++)
            {
                SkillData skill = all[i];
                if (skill == null || skill.classType != active || skill.isPassive || !skill.isImplemented) continue;
                AddSkillCard(classSkillCardsRoot, skill);
                count++;
            }

            if (count == 0)
                SetClassSkillStatus("director.class_skill.status.no_skills", active);
        }

        private void AddSkillCard(RectTransform parent, SkillData skill)
        {
            Button button = CreateButton("Button_Skill_" + SanitizeName(skill.skillName), parent, rewardCard, Color.white);
            RectTransform rt = button.transform as RectTransform;

            TextMeshProUGUI title = CreateText("TMP_Title", rt, skill.skillName, 17f, FontStyles.Bold, TextAlignmentOptions.MidlineLeft);
            Stretch(title.rectTransform, Vector2.zero, Vector2.one, new Vector2(14f, 44f), new Vector2(-12f, -8f));

            TextMeshProUGUI desc = CreateText("TMP_Desc", rt, skill.description, 11f, FontStyles.Normal, TextAlignmentOptions.TopLeft);
            desc.color = new Color(0.78f, 0.84f, 0.86f, 0.76f);
            desc.enableWordWrapping = true;
            Stretch(desc.rectTransform, Vector2.zero, Vector2.one, new Vector2(14f, 8f), new Vector2(-12f, -38f));

            button.onClick.AddListener(() => SelectDirectorSkill(skill));
            skillCards.Add(new SkillCardBinding(skill, button.GetComponent<Image>()));
        }

        private void SelectDirectorSkill(SkillData skill)
        {
            selectedDirectorSkill = skill;
            RefreshSkillCards();
            SetClassSkillStatus("director.class_skill.status.selected", skill.skillName);
        }

        private void RefreshSkillCards()
        {
            foreach (SkillCardBinding binding in skillCards)
            {
                if (binding.Background != null)
                    binding.Background.color = binding.Skill == selectedDirectorSkill ? new Color(1f, 0.72f, 0.34f, 1f) : Color.white;
            }
        }

        private bool AssignSelectedSkillToSlot(int slot)
        {
            if (selectedDirectorSkill == null)
            {
                SetClassSkillStatus("director.class_skill.status.no_skill_selected");
                return false;
            }

            DraftManager draft = DraftManager.Instance;
            if (draft == null)
            {
                SetClassSkillStatus("director.class_skill.status.no_draft");
                return false;
            }

            if (!draft.TryDirectorAssignSkill(selectedDirectorSkill, slot, out string error))
            {
                SetClassSkillStatus("director.class_skill.status.assign_failed", error);
                return false;
            }

            SetClassSkillStatus("director.class_skill.status.assigned", selectedDirectorSkill.skillName, SlotLabel(slot));
            return true;
        }

        private bool AssignBasicAction(GameAction action, string bindingPath)
        {
            if (!KeyBindManager.TrySetBinding(action, bindingPath, out string error))
            {
                SetClassSkillStatus("director.class_skill.status.binding_failed", error);
                return false;
            }

            SetClassSkillStatus("director.class_skill.status.binding", KeyBindManager.PathToLabel(bindingPath));
            return true;
        }

        private SkillData FindDirectorSkill(string skillName)
        {
            if (string.IsNullOrEmpty(skillName)) return null;
            EnsureSkillDatabase();
            return SkillDatabase.Instance != null ? SkillDatabase.Instance.FindByName(skillName) : null;
        }

        private static void EnsureSkillDatabase()
        {
            if (SkillDatabase.Instance != null) { SkillDatabase.Instance.EnsureBuilt(); return; }
            SkillDatabase existing = FindAnyObjectByType<SkillDatabase>();
            if (existing != null) { existing.EnsureBuilt(); return; }
            GameObject go = new GameObject("SkillDatabase_Director");
            go.AddComponent<SkillDatabase>();
        }

        private void SetClassSkillStatus(string locKey, params object[] args)
        {
            if (classSkillStatusText != null)
                classSkillStatusText.text = args != null && args.Length > 0 ? Loc.T(locKey, args) : Loc.T(locKey);
        }

        private static string SlotLabel(int slot)
        {
            switch (slot)
            {
                case 0: return "Q";
                case 1: return "E";
                case 2: return "R";
                case 3: return "F";
                default: return "?";
            }
        }

        private static string SanitizeName(string value)
        {
            if (string.IsNullOrEmpty(value)) return "Unknown";
            char[] chars = value.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
                if (!char.IsLetterOrDigit(chars[i])) chars[i] = '_';
            return new string(chars);
        }

        private static void DestroyRuntimeObject(UnityEngine.Object target)
        {
            if (target == null) return;
            if (Application.isPlaying) Destroy(target);
            else DestroyImmediate(target);
        }

        private void OnStatSliderChanged(StatSliderBinding binding, float value)
        {
            if (suppressStatSliderCallbacks)
            {
                return;
            }

            PlayerClassManager manager = PlayerClassManager.Instance;
            if (manager == null)
            {
                SetStatsStatus("director.stats.status.no_manager");
                return;
            }

            ClassStatRuntime stats = manager.EnsureCurrentPrimaryStats();
            binding.Write(stats, value);
            manager.ApplyCurrentPrimaryStatsToPlayer();
            binding.SetValueText(binding.Read(stats));
            SetStatsStatus("director.stats.status.live", manager.PrimaryClass);
        }

        private void RefreshStatsSlidersFromRuntime()
        {
            if (statSliders.Count == 0)
            {
                return;
            }

            PlayerClassManager manager = PlayerClassManager.Instance;
            if (manager == null)
            {
                SetStatsStatus("director.stats.status.no_manager");
                return;
            }

            ClassStatRuntime stats = manager.EnsureCurrentPrimaryStats();
            suppressStatSliderCallbacks = true;
            foreach (StatSliderBinding binding in statSliders)
            {
                float value = binding.Read(stats);
                binding.Slider.SetValueWithoutNotify(value);
                binding.SetValueText(value);
            }
            suppressStatSliderCallbacks = false;
            SetStatsStatus("director.stats.status.live", manager.PrimaryClass);
        }

        private void ResetStatsFromProfile()
        {
            PlayerClassManager manager = PlayerClassManager.Instance;
            if (manager == null)
            {
                SetStatsStatus("director.stats.status.no_manager");
                return;
            }

            manager.ResetCurrentPrimaryStatsFromProfile();
            RefreshStatsSlidersFromRuntime();
            SetStatsStatus("director.stats.status.reset", manager.PrimaryClass);
        }

        private void SaveStatsPreset()
        {
            PlayerClassManager manager = PlayerClassManager.Instance;
            if (manager == null)
            {
                SetStatsStatus("director.stats.status.no_manager");
                return;
            }

            ClassStatRuntime stats = manager.EnsureCurrentPrimaryStats();
            string json = JsonUtility.ToJson(stats);
            PlayerPrefs.SetString("rima.director.stats." + manager.PrimaryClass, json);
            PlayerPrefs.Save();
            Debug.Log("[DirectorMode] Saved Stats preset: " + json);
            SetStatsStatus("director.stats.status.saved", manager.PrimaryClass);
        }

        private void ExportStatsJson()
        {
            PlayerClassManager manager = PlayerClassManager.Instance;
            if (manager == null)
            {
                SetStatsStatus("director.stats.status.no_manager");
                return;
            }

            string json = JsonUtility.ToJson(manager.EnsureCurrentPrimaryStats(), true);
            GUIUtility.systemCopyBuffer = json;
            Debug.Log("[DirectorMode] Stats export: " + json);
            SetStatsStatus("director.stats.status.exported", manager.PrimaryClass);
        }

        private void SetStatsStatus(string locKey, params object[] args)
        {
            if (statsStatusText != null)
            {
                statsStatusText.text = args != null && args.Length > 0 ? Loc.T(locKey, args) : Loc.T(locKey);
            }
        }

        private void RefreshLocalizedText()
        {
            foreach (LocalizedTextBinding binding in localizedTexts)
            {
                if (binding.Text != null)
                {
                    binding.Text.text = Loc.T(binding.Key);
                }
            }
        }

        private static Color HtmlColor(string html)
        {
            return ColorUtility.TryParseHtmlString(html, out Color color) ? color : Color.white;
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
            rewardCard = rewardCard != null ? rewardCard : AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Chrome/reward_card.png");
            ribbonBase = ribbonBase != null ? ribbonBase : AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Chrome/ribbon_base.png");
            menuButton = menuButton != null ? menuButton : AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Chrome/menu_button.png");
            tooltipBox = tooltipBox != null ? tooltipBox : AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Chrome/tooltip_box.png");
#endif
        }

        private sealed class StatSliderBinding
        {
            public readonly string Key;
            public readonly Slider Slider;
            public readonly TextMeshProUGUI ValueText;
            public readonly Func<ClassStatRuntime, float> Read;
            public readonly Action<ClassStatRuntime, float> Write;

            public StatSliderBinding(string key, Slider slider, TextMeshProUGUI valueText, Func<ClassStatRuntime, float> read, Action<ClassStatRuntime, float> write)
            {
                Key = key;
                Slider = slider;
                ValueText = valueText;
                Read = read;
                Write = write;
            }

            public void SetValueText(float value)
            {
                if (ValueText == null)
                {
                    return;
                }

                ValueText.text = Slider != null && Slider.wholeNumbers
                    ? Mathf.RoundToInt(value).ToString()
                    : value.ToString("0.##");
            }
        }

        private readonly struct LocalizedTextBinding
        {
            public readonly TextMeshProUGUI Text;
            public readonly string Key;

            public LocalizedTextBinding(TextMeshProUGUI text, string key)
            {
                Text = text;
                Key = key;
            }
        }

        private readonly struct ClassButtonBinding
        {
            public readonly ClassType Type;
            public readonly Image Background;

            public ClassButtonBinding(ClassType type, Image background)
            {
                Type = type;
                Background = background;
            }
        }

        private readonly struct SkillCardBinding
        {
            public readonly SkillData Skill;
            public readonly Image Background;

            public SkillCardBinding(SkillData skill, Image background)
            {
                Skill = skill;
                Background = background;
            }
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
