#if DEMO_BUILD || DEVELOPMENT_BUILD || UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UI;
using RIMA.Balance;
using RIMA.Encounter;

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

        [Header("Director Props")]
        [SerializeField] private List<GameObject> directorPlaceableProps = new List<GameObject>();

        [Header("Free Camera")]
        [SerializeField] private float panSpeed = 8f;
        [SerializeField] private float cameraLerp = 16f;

        private readonly List<TabBinding> tabs = new List<TabBinding>();
        private readonly Dictionary<DirectorTab, CanvasGroup> panels = new Dictionary<DirectorTab, CanvasGroup>();
        private readonly List<StatSliderBinding> statSliders = new List<StatSliderBinding>();
        private readonly List<ClassButtonBinding> classButtons = new List<ClassButtonBinding>();
        private readonly List<SkillCardBinding> skillCards = new List<SkillCardBinding>();
        private readonly List<SpawnEnemyBinding> spawnEnemies = new List<SpawnEnemyBinding>();
        private readonly List<GameObject> directorSpawnedEnemies = new List<GameObject>();
        private readonly List<PropBinding> propBindings = new List<PropBinding>();
        private readonly List<GameObject> directorPlacedProps = new List<GameObject>();
        private readonly List<LocalizedTextBinding> localizedTexts = new List<LocalizedTextBinding>();
        private readonly List<DamageTelemetryRecord> telemetryRecords = new List<DamageTelemetryRecord>();
        private readonly Dictionary<DamageSourceType, int> telemetryDamageBySource = new Dictionary<DamageSourceType, int>();
        private readonly Dictionary<Health, float> telemetryFirstHitTimes = new Dictionary<Health, float>();
        private readonly Dictionary<Health, UnityAction> telemetryDeathListeners = new Dictionary<Health, UnityAction>();
        private readonly List<TelemetrySourceBinding> telemetrySourceBindings = new List<TelemetrySourceBinding>();

        private CanvasGroup rootGroup;
        private TextMeshProUGUI subtitleText;
        private TextMeshProUGUI startButtonText;
        private TextMeshProUGUI modeStripText;
        private TextMeshProUGUI spawnStatusText;
        private TextMeshProUGUI classSkillStatusText;
        private TextMeshProUGUI statsStatusText;
        private TextMeshProUGUI buildStatusText;
        private TextMeshProUGUI telemetryDpsText;
        private TextMeshProUGUI telemetryTtkText;
        private TextMeshProUGUI telemetryEventCountText;
        private TextMeshProUGUI telemetryStatusText;
        private TextMeshProUGUI statToastText;
        private CanvasGroup statToastGroup;
        private RectTransform spawnPaletteRoot;
        private RectTransform propPaletteRoot;
        private RectTransform classSkillCardsRoot;
        private SpawnEnemyBinding selectedSpawnEnemy;
        private PropBinding selectedProp;
        private GameObject spawnGhost;
        private SpriteRenderer spawnGhostRenderer;
        private GameObject propGhost;
        private SpriteRenderer propGhostRenderer;
        private SkillData selectedDirectorSkill;
        private Camera directorCamera;
        private Vector3 targetCameraPosition;
        private bool hasCameraTarget;
        private bool suppressStatSliderCallbacks;
        private float spawnGhostPulse;
        private float propGhostPulse;
        private float telemetryLastTtk = -1f;
        private float telemetryLastRefreshTime = -1f;
        private string telemetryLastCsv = "";
        private float telemetryLastDps;
        private bool telemetryHasLastDps;
        // Director pause sırasında DPS saatini dondurmak için biriken toplam pause süresi.
        private float telemetryPausedTotal;
        private float telemetryPauseStartTime = -1f;
        private Coroutine statToastRoutine;

        private const string DirectorWaveResourcePath = "Encounters/Act1_Wave_Pilot";
        private const string DefaultRiftCrystalPrefabPath = "Assets/Resources/DirectorProps/rift_crystal.prefab";
        private const string RiftCrystalResourcePath = "DirectorProps/rift_crystal";
        private const float SpawnGridSize = 1f;
        private const float EraseRadius = 0.7f;
        private const float TelemetryDpsWindowSeconds = 5f;
        private const float TelemetryRefreshInterval = 0.15f;
        private const float StatToastDuration = 1.2f;
        private const int MaxDirectorSpawnedEnemies = 10;

        public DirectorModeState State { get; private set; } = DirectorModeState.Test;
        public DirectorTab ActiveTab { get; private set; } = DirectorTab.Spawn;

        private static readonly ClassType[] DirectorClasses =
        {
            ClassType.Warblade,
            ClassType.Elementalist,
            ClassType.Shadowblade,
            ClassType.Ranger,
            ClassType.Ronin
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
            UpdateTelemetryDisplay(false);

            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.backquoteKey.wasPressedThisFrame)
            {
                ToggleState();
            }

            if (State == DirectorModeState.Director)
            {
                UpdateFreeCamera(Time.unscaledDeltaTime);
                UpdateSpawnTool(Time.unscaledDeltaTime);
            }
            else
            {
                HideSpawnGhost();
                HidePropGhost();
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }

            HideSpawnGhost();
            HidePropGhost();
            SkillRuntime.OnDamageApplied -= OnDamageAppliedTelemetry;
            ClearTelemetryDeathListeners();
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

            SkillRuntime.OnDamageApplied -= OnDamageAppliedTelemetry;
            SkillRuntime.OnDamageApplied += OnDamageAppliedTelemetry;
        }

        private void OnDisable()
        {
            SkillRuntime.OnDamageApplied -= OnDamageAppliedTelemetry;
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
                Time.timeScale = ResolveTimeScaleForState(state);
                SetPlayerActiveForState(state);
                return;
            }

            UpdateTelemetryPauseClock(state);

            State = state;
            Time.timeScale = ResolveTimeScaleForState(state);
            SetPlayerActiveForState(state);
            if (state == DirectorModeState.Director)
            {
                CacheCameraTarget();
            }

            ApplyStateText();
            OnStateChanged?.Invoke(State);
        }

        private float ResolveTimeScaleForState(DirectorModeState state)
        {
            if (state == DirectorModeState.Director)
            {
                return 0f;
            }

            DeathScreenManager deathScreen = FindDeathScreenManager();
            return deathScreen != null && deathScreen.IsDeathActiveForDemo ? 0f : 1f;
        }

        private static void SetPlayerActiveForState(DirectorModeState state)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (state == DirectorModeState.Director)
            {
                PlayerController.SetPlayerActive(player, false);
                return;
            }

            DeathScreenManager deathScreen = FindDeathScreenManager();
            if (deathScreen == null || !deathScreen.IsDeathActiveForDemo)
            {
                PlayerController.SetPlayerActive(player, true);
            }
        }

        private static DeathScreenManager FindDeathScreenManager()
        {
            DeathScreenManager[] managers = FindObjectsByType<DeathScreenManager>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            return managers.Length > 0 ? managers[0] : null;
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
            else if (tab == DirectorTab.Build)
            {
                EnsurePropPaletteLoaded();
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
            EnsureOverlayReadyForValidation();
            return panels.TryGetValue(tab, out CanvasGroup group) && group != null;
        }

        public bool IsTabInteractableForValidation(DirectorTab tab)
        {
            EnsureOverlayReadyForValidation();
            return panels.TryGetValue(tab, out CanvasGroup group) && group.alpha > 0.99f && group.interactable && group.blocksRaycasts;
        }

        public void SelectClassForValidation(ClassType type)
        {
            EnsureOverlayReadyForValidation();
            SelectDirectorClass(type);
        }

        public bool AssignSkillForValidation(string skillName, int slot)
        {
            EnsureOverlayReadyForValidation();
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
            EnsureOverlayReadyForValidation();
            return AssignBasicAction(GameAction.Attack, "<Mouse>/leftButton")
                & AssignBasicAction(GameAction.ClassSecondary, "<Mouse>/rightButton");
        }

        public bool SelectFirstSpawnEnemyForValidation()
        {
            EnsureOverlayReadyForValidation();
            EnsureSpawnPaletteLoaded();
            if (spawnEnemies.Count == 0)
                return false;

            SelectSpawnEnemy(spawnEnemies[0]);
            return selectedSpawnEnemy.Prefab != null;
        }

        public int DirectorSpawnedEnemyCountForValidation()
        {
            PruneDirectorSpawnedEnemies();
            return directorSpawnedEnemies.Count;
        }

        public bool SpawnSelectedEnemyAtForValidation(Vector2 position)
        {
            EnsureOverlayReadyForValidation();
            EnsureSpawnPaletteLoaded();
            if ((selectedSpawnEnemy == null || selectedSpawnEnemy.Prefab == null) && spawnEnemies.Count > 0)
                SelectSpawnEnemy(spawnEnemies[0]);

            return SpawnSelectedEnemy(SnapWorld(position)) != null;
        }

        public bool EraseDirectorEnemyAtForValidation(Vector2 position)
        {
            EnsureOverlayReadyForValidation();
            return EraseDirectorEnemyAt(SnapWorld(position));
        }

        public bool HasSpawnGhostForValidation()
        {
            EnsureOverlayReadyForValidation();
            return spawnGhost != null && spawnGhost.activeSelf && spawnGhostRenderer != null && spawnGhostRenderer.sprite != null;
        }

        public bool SelectFirstPropForValidation()
        {
            EnsureOverlayReadyForValidation();
            EnsurePropPaletteLoaded();
            if (propBindings.Count == 0)
                return false;

            SelectProp(propBindings[0]);
            return selectedProp.Prefab != null;
        }

        public bool PlaceSelectedPropAtForValidation(Vector2 position)
        {
            EnsureOverlayReadyForValidation();
            EnsurePropPaletteLoaded();
            if ((selectedProp == null || selectedProp.Prefab == null) && propBindings.Count > 0)
                SelectProp(propBindings[0]);

            return PlaceSelectedProp(SnapWorld(position)) != null;
        }

        public int DirectorPlacedPropCountForValidation()
        {
            EnsureOverlayReadyForValidation();
            PruneDirectorPlacedProps();
            return directorPlacedProps.Count;
        }

        public bool ErasePlacedPropAtForValidation(Vector2 position)
        {
            EnsureOverlayReadyForValidation();
            return ErasePlacedPropAt(SnapWorld(position));
        }

        public bool HasPropGhostForValidation()
        {
            EnsureOverlayReadyForValidation();
            return propGhost != null && propGhost.activeSelf && propGhostRenderer != null && propGhostRenderer.sprite != null;
        }

        public bool SetStatForValidation(string statKey, float value)
        {
            EnsureOverlayReadyForValidation();
            if (string.IsNullOrEmpty(statKey))
            {
                return false;
            }

            if (PlayerClassManager.Instance == null)
            {
                SetStatsStatus("director.stats.status.no_manager");
                return false;
            }

            for (int i = 0; i < statSliders.Count; i++)
            {
                StatSliderBinding binding = statSliders[i];
                if (!string.Equals(binding.Key, statKey, StringComparison.Ordinal))
                {
                    continue;
                }

                float clamped = ClampStatValue(binding, value);
                if (binding.Slider != null)
                {
                    binding.Slider.SetValueWithoutNotify(clamped);
                }

                OnStatSliderChanged(binding, clamped);
                return true;
            }

            return false;
        }

        private void EnsureOverlayReadyForValidation()
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

        public int TelemetryEventCountForValidation()
        {
            return telemetryRecords.Count;
        }

        public float TelemetryDpsForValidation()
        {
            return CalculateTelemetryDps(TelemetryClock());
        }

        public int TelemetrySourceDamageForValidation(DamageSourceType sourceType)
        {
            return telemetryDamageBySource.TryGetValue(sourceType, out int damage) ? damage : 0;
        }

        public string ExportTelemetryCsvForValidation()
        {
            telemetryLastCsv = BuildTelemetryCsv();
            return telemetryLastCsv;
        }

        public void ClearTelemetryForValidation()
        {
            ClearTelemetry();
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
            dimmer.raycastTarget = false;
            Stretch(dimmer.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            BuildTopBadge(root);
            BuildTabRail(root);
            BuildContentArea(root);
            BuildWorldCursorOverlay(root);
            BuildMinimapMini(root);
            BuildSelectionInspector(root);
            BuildBottomTelemetryStrip(root);
            BuildStatToast(root);
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
            spawnEnemies.Clear();
            directorSpawnedEnemies.Clear();
            propBindings.Clear();
            directorPlacedProps.Clear();
            localizedTexts.Clear();
            rootGroup = null;
            spawnStatusText = null;
            classSkillStatusText = null;
            statsStatusText = null;
            buildStatusText = null;
            statToastText = null;
            statToastGroup = null;
            spawnPaletteRoot = null;
            propPaletteRoot = null;
            classSkillCardsRoot = null;
            selectedSpawnEnemy = null;
            selectedProp = null;
            selectedDirectorSkill = null;
            HideSpawnGhost();
            HidePropGhost();

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

            AddSpawnPanel(content);
            AddClassSkillPanel(content);
            AddStatsPanel(content);
            AddBuildPanel(content);
            AddEmptyPanel(content, DirectorTab.Map, "MAP", "yakinda");
            AddTelemetryPanel(content);
        }

        private void AddSpawnPanel(RectTransform parent)
        {
            RectTransform panel = CreateFill("Panel_" + DirectorTab.Spawn, parent);
            CanvasGroup group = panel.gameObject.AddComponent<CanvasGroup>();
            panels[DirectorTab.Spawn] = group;

            RectTransform window = CreatePanel("Window", panel, minimapFrame, Color.white, Image.Type.Sliced);
            Stretch(window, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            RectTransform header = CreateFill("Header", panel);
            Anchor(header, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -26f), new Vector2(-64f, 74f));

            TextMeshProUGUI titleText = CreateText("TMP_Title", header, Loc.T("director.spawn.title"), 30f, FontStyles.Bold, TextAlignmentOptions.MidlineLeft);
            Stretch(titleText.rectTransform, Vector2.zero, Vector2.one, new Vector2(34f, 26f), new Vector2(-34f, 0f));
            localizedTexts.Add(new LocalizedTextBinding(titleText, "director.spawn.title"));

            TextMeshProUGUI hint = CreateText("TMP_Hint", header, Loc.T("director.spawn.hint"), 20f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
            hint.color = new Color(0.78f, 0.84f, 0.86f, 0.72f);
            Stretch(hint.rectTransform, Vector2.zero, Vector2.one, new Vector2(34f, 0f), new Vector2(-34f, -30f));
            localizedTexts.Add(new LocalizedTextBinding(hint, "director.spawn.hint"));

            spawnPaletteRoot = CreatePanel("EnemyPalette", panel, null, new Color(0.02f, 0.025f, 0.035f, 0.28f), Image.Type.Simple);
            Anchor(spawnPaletteRoot, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, 1f), new Vector2(42f, -132f), new Vector2(-90f, 330f));

            GridLayoutGroup paletteLayout = spawnPaletteRoot.gameObject.AddComponent<GridLayoutGroup>();
            paletteLayout.padding = new RectOffset(16, 16, 16, 16);
            paletteLayout.spacing = new Vector2(12f, 12f);
            paletteLayout.cellSize = new Vector2(168f, 78f);
            paletteLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            paletteLayout.constraintCount = 5;

            RectTransform actions = CreateFill("SpawnActions", panel);
            Anchor(actions, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(42f, 74f), new Vector2(520f, 54f));

            Button clear = CreateButton("Button_ClearDirectorSpawns", actions, menuButton, Color.white);
            Anchor(clear.GetComponent<RectTransform>(), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), Vector2.zero, new Vector2(180f, 42f));
            clear.onClick.AddListener(ClearDirectorSpawns);
            AddButtonText(clear, "director.spawn.clear");

            spawnStatusText = CreateText("TMP_Status", panel, "", 18f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
            spawnStatusText.color = new Color(0.78f, 0.84f, 0.86f, 0.72f);
            Anchor(spawnStatusText.rectTransform, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0.5f, 0f), new Vector2(42f, 32f), new Vector2(-120f, 30f));

            EnsureSpawnPaletteLoaded();
        }

        private void EnsureSpawnPaletteLoaded()
        {
            if (spawnPaletteRoot == null || spawnEnemies.Count > 0)
                return;

            EncounterWaveSO wave = Resources.Load<EncounterWaveSO>(DirectorWaveResourcePath);
            if (wave == null || wave.entries == null || wave.entries.Count == 0)
            {
                SetSpawnStatus("director.spawn.status.no_wave");
                return;
            }

            HashSet<GameObject> seen = new HashSet<GameObject>();
            for (int i = 0; i < wave.entries.Count; i++)
            {
                EncounterEnemyEntry entry = wave.entries[i];
                if (entry == null || entry.prefab == null || seen.Contains(entry.prefab))
                    continue;

                seen.Add(entry.prefab);
                AddSpawnEnemyButton(spawnPaletteRoot, entry);
            }

            if (spawnEnemies.Count > 0)
            {
                SelectSpawnEnemy(spawnEnemies[0]);
                SetSpawnStatus("director.spawn.status.loaded", spawnEnemies.Count);
            }
            else
            {
                SetSpawnStatus("director.spawn.status.no_wave");
            }
        }

        private void AddSpawnEnemyButton(RectTransform parent, EncounterEnemyEntry entry)
        {
            string label = entry.prefab != null ? entry.prefab.name : entry.enemyType.ToString();
            Button button = CreateButton("Button_Spawn_" + SanitizeName(label), parent, slotNormal, Color.white);
            RectTransform rt = button.transform as RectTransform;

            Image swatch = CreateImage("Swatch", rt, null, EnemyTypeColor(entry.enemyType), Image.Type.Simple);
            Anchor(swatch.rectTransform, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(12f, 0f), new Vector2(18f, 44f));

            TextMeshProUGUI title = CreateText("TMP_Label", rt, label, 16f, FontStyles.Bold, TextAlignmentOptions.MidlineLeft);
            Stretch(title.rectTransform, Vector2.zero, Vector2.one, new Vector2(38f, 22f), new Vector2(-10f, -8f));
            title.enableWordWrapping = true;

            TextMeshProUGUI meta = CreateText("TMP_Meta", rt, entry.enemyType.ToString(), 11f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
            meta.color = new Color(0.78f, 0.84f, 0.86f, 0.70f);
            Stretch(meta.rectTransform, Vector2.zero, Vector2.one, new Vector2(38f, 6f), new Vector2(-10f, -48f));

            SpawnEnemyBinding binding = new SpawnEnemyBinding(entry.enemyType, entry.prefab, button.GetComponent<Image>());
            button.onClick.AddListener(() => SelectSpawnEnemy(binding));
            spawnEnemies.Add(binding);
        }

        private void SelectSpawnEnemy(SpawnEnemyBinding binding)
        {
            selectedSpawnEnemy = binding;
            RefreshSpawnButtons();
            RebuildSpawnGhost();
            if (binding != null)
                SetSpawnStatus("director.spawn.status.selected", binding.Label);
        }

        private void RefreshSpawnButtons()
        {
            foreach (SpawnEnemyBinding binding in spawnEnemies)
            {
                if (binding.Background != null)
                    binding.Background.sprite = binding == selectedSpawnEnemy && slotActive != null ? slotActive : slotNormal;
            }
        }

        private void UpdateSpawnTool(float dt)
        {
            if (ActiveTab != DirectorTab.Spawn)
            {
                if (ActiveTab == DirectorTab.Build)
                {
                    HideSpawnGhost();
                    UpdatePropTool(dt);
                    return;
                }

                HideSpawnGhost();
                HidePropGhost();
                return;
            }

            HidePropGhost();
            EnsureSpawnPaletteLoaded();

            Mouse mouse = Mouse.current;
            if (mouse == null || selectedSpawnEnemy == null || selectedSpawnEnemy.Prefab == null)
            {
                HideSpawnGhost();
                return;
            }

            Vector2 screen = mouse.position.ReadValue();
            Vector3 snapped = SnapWorld(MouseScreenToWorld(screen));

            UpdateSpawnGhost(snapped, dt);

            if (IsPointerOverDirectorUi())
                return;

            if (mouse.leftButton.wasPressedThisFrame)
                SpawnSelectedEnemy(snapped);
            else if (mouse.rightButton.wasPressedThisFrame)
                EraseDirectorEnemyAt(snapped);
        }

        private GameObject SpawnSelectedEnemy(Vector3 position)
        {
            if (selectedSpawnEnemy == null || selectedSpawnEnemy.Prefab == null)
                return null;

            PruneDirectorSpawnedEnemies();
            if (directorSpawnedEnemies.Count >= MaxDirectorSpawnedEnemies)
            {
                Debug.LogWarning($"[DirectorMode] Spawn cap reached ({MaxDirectorSpawnedEnemies}); spawn ignored.");
                if (spawnStatusText != null)
                    spawnStatusText.text = $"Spawn cap reached ({MaxDirectorSpawnedEnemies}).";
                return null;
            }

            GameObject instance = Instantiate(selectedSpawnEnemy.Prefab, position, Quaternion.identity);
            instance.name = selectedSpawnEnemy.Prefab.name + "_Director";
            instance.tag = "Enemy";
            directorSpawnedEnemies.Add(instance);
            if (Application.isPlaying)
                SkillVfx.ImpactBurst(position, VfxElement.Arcane);

            Health health = instance.GetComponent<Health>();
            if (health == null)
                health = instance.AddComponent<Health>();

            GameObject tracked = instance;
            health.OnDeath.AddListener(() => directorSpawnedEnemies.Remove(tracked));
            StartCoroutine(PopSpawn(instance.transform));
            SetSpawnStatus("director.spawn.status.spawned", selectedSpawnEnemy.Label, directorSpawnedEnemies.Count);
            return instance;
        }

        private bool EraseDirectorEnemyAt(Vector3 position)
        {
            PruneDirectorSpawnedEnemies();
            GameObject target = FindDirectorEnemyAt(position);
            if (target == null)
            {
                SetSpawnStatus("director.spawn.status.erase_none");
                return false;
            }

            directorSpawnedEnemies.Remove(target);
            DestroyRuntimeObject(target);
            SetSpawnStatus("director.spawn.status.erased", directorSpawnedEnemies.Count);
            return true;
        }

        private GameObject FindDirectorEnemyAt(Vector3 position)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(position, EraseRadius);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i] == null)
                    continue;

                GameObject hit = hits[i].attachedRigidbody != null ? hits[i].attachedRigidbody.gameObject : hits[i].gameObject;
                if (directorSpawnedEnemies.Contains(hit))
                    return hit;
            }

            GameObject nearest = null;
            float nearestSqr = EraseRadius * EraseRadius;
            for (int i = 0; i < directorSpawnedEnemies.Count; i++)
            {
                GameObject enemy = directorSpawnedEnemies[i];
                if (enemy == null)
                    continue;

                float sqr = ((Vector2)enemy.transform.position - (Vector2)position).sqrMagnitude;
                if (sqr <= nearestSqr)
                {
                    nearest = enemy;
                    nearestSqr = sqr;
                }
            }

            return nearest;
        }

        private void ClearDirectorSpawns()
        {
            for (int i = directorSpawnedEnemies.Count - 1; i >= 0; i--)
            {
                GameObject enemy = directorSpawnedEnemies[i];
                if (enemy != null)
                    DestroyRuntimeObject(enemy);
            }

            directorSpawnedEnemies.Clear();
            SetSpawnStatus("director.spawn.status.cleared");
        }

        private void DemoQuickReset()
        {
            Health playerHealth = FindPlayerHealth();
            if (playerHealth != null)
            {
                playerHealth.Revive();
            }

            DeathScreenManager deathScreen = FindDeathScreenManager();
            if (deathScreen != null)
            {
                deathScreen.CancelDeathForDemo();
            }

            ClearDirectorSpawns();
            ClearDirectorProps();
            SetState(DirectorModeState.Test);
            Time.timeScale = 1f;
            PlayerController.SetPlayerActive(playerHealth != null ? playerHealth.gameObject : GameObject.FindGameObjectWithTag("Player"), true);
            ApplyStateText();

            if (modeStripText != null)
            {
                modeStripText.text = "DEMO RESET - HP FULL / SPAWNS CLEARED";
            }
        }

        private static Health FindPlayerHealth()
        {
            GameObject player = null;
            try
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            catch (UnityException)
            {
                player = null;
            }

            if (player != null && player.TryGetComponent(out Health taggedHealth))
            {
                return taggedHealth;
            }

            Health[] healths = FindObjectsByType<Health>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            for (int i = 0; i < healths.Length; i++)
            {
                if (healths[i] != null && healths[i].CompareTag("Player"))
                {
                    return healths[i];
                }
            }

            return null;
        }

        private void PruneDirectorSpawnedEnemies()
        {
            for (int i = directorSpawnedEnemies.Count - 1; i >= 0; i--)
            {
                if (directorSpawnedEnemies[i] == null)
                    directorSpawnedEnemies.RemoveAt(i);
            }
        }

        private void RebuildSpawnGhost()
        {
            HideSpawnGhost();
            if (selectedSpawnEnemy == null || selectedSpawnEnemy.Prefab == null)
                return;

            SpriteRenderer prefabRenderer = selectedSpawnEnemy.Prefab.GetComponentInChildren<SpriteRenderer>();
            GameObject ghost = new GameObject("DirectorSpawnGhost", typeof(SpriteRenderer));
            SpriteRenderer renderer = ghost.GetComponent<SpriteRenderer>();
            renderer.sprite = prefabRenderer != null ? prefabRenderer.sprite : null;
            renderer.sharedMaterial = prefabRenderer != null ? prefabRenderer.sharedMaterial : null;
            renderer.sortingLayerName = prefabRenderer != null ? prefabRenderer.sortingLayerName : "Entities";
            renderer.sortingOrder = prefabRenderer != null ? prefabRenderer.sortingOrder + 8 : 8;
            renderer.color = new Color(0f, 1f, 0.8f, 0.42f);
            ghost.transform.localScale = selectedSpawnEnemy.Prefab.transform.localScale;

            spawnGhost = ghost;
            spawnGhostRenderer = renderer;
            spawnGhostPulse = 0f;
        }

        private void UpdateSpawnGhost(Vector3 position, float dt)
        {
            if (spawnGhost == null || spawnGhostRenderer == null)
                RebuildSpawnGhost();

            if (spawnGhost == null)
                return;

            spawnGhost.SetActive(true);
            spawnGhost.transform.position = position;
            spawnGhostPulse += dt * 6f;
            float pulse = 0.92f + Mathf.Sin(spawnGhostPulse) * 0.04f;
            spawnGhost.transform.localScale = selectedSpawnEnemy.Prefab.transform.localScale * pulse;
        }

        private void HideSpawnGhost()
        {
            if (spawnGhost != null)
                DestroyRuntimeObject(spawnGhost);

            spawnGhost = null;
            spawnGhostRenderer = null;
        }

        private IEnumerator PopSpawn(Transform target)
        {
            if (target == null)
                yield break;

            Vector3 baseScale = target.localScale;
            float time = 0f;
            const float duration = 0.14f;
            while (time < duration && target != null)
            {
                time += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(time / duration);
                float scale = 1f + Mathf.Sin(t * Mathf.PI) * 0.18f;
                target.localScale = baseScale * scale;
                yield return null;
            }

            if (target != null)
                target.localScale = baseScale;
        }

        private Vector3 MouseScreenToWorld(Vector2 screen)
        {
            CacheCameraTarget();
            if (directorCamera == null)
                return Vector3.zero;

            Vector3 world = directorCamera.ScreenToWorldPoint(new Vector3(screen.x, screen.y, -directorCamera.transform.position.z));
            world.z = 0f;
            return world;
        }

        private static Vector3 SnapWorld(Vector3 world)
        {
            return new Vector3(
                Mathf.Round(world.x / SpawnGridSize) * SpawnGridSize,
                Mathf.Round(world.y / SpawnGridSize) * SpawnGridSize,
                0f);
        }

        private static bool IsPointerOverDirectorUi()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }

        private void SetSpawnStatus(string locKey, params object[] args)
        {
            if (spawnStatusText != null)
                spawnStatusText.text = args != null && args.Length > 0 ? Loc.T(locKey, args) : Loc.T(locKey);
        }

        private static Color EnemyTypeColor(EncounterEnemyType type)
        {
            switch (type)
            {
                case EncounterEnemyType.FractureImp: return HtmlColor("#C82026");
                case EncounterEnemyType.SeamCrawler: return HtmlColor("#00FFCC");
                case EncounterEnemyType.PenitentBruiser: return HtmlColor("#E89020");
                case EncounterEnemyType.VoidThrall: return HtmlColor("#8F5CFF");
                default: return HtmlColor("#D8D0B0");
            }
        }

        private void AddBuildPanel(RectTransform parent)
        {
            RectTransform panel = CreateFill("Panel_" + DirectorTab.Build, parent);
            CanvasGroup group = panel.gameObject.AddComponent<CanvasGroup>();
            panels[DirectorTab.Build] = group;

            RectTransform window = CreatePanel("Window", panel, minimapFrame, Color.white, Image.Type.Sliced);
            Stretch(window, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            RectTransform header = CreateFill("Header", panel);
            Anchor(header, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -26f), new Vector2(-64f, 74f));

            TextMeshProUGUI titleText = CreateText("TMP_Title", header, "BUILD", 30f, FontStyles.Bold, TextAlignmentOptions.MidlineLeft);
            Stretch(titleText.rectTransform, Vector2.zero, Vector2.one, new Vector2(34f, 26f), new Vector2(-34f, 0f));

            TextMeshProUGUI hint = CreateText("TMP_Hint", header, "Palette -> ghost -> left click place -> right click erase", 20f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
            hint.color = new Color(0.78f, 0.84f, 0.86f, 0.72f);
            Stretch(hint.rectTransform, Vector2.zero, Vector2.one, new Vector2(34f, 0f), new Vector2(-34f, -30f));

            propPaletteRoot = CreatePanel("PropPalette", panel, null, new Color(0.02f, 0.025f, 0.035f, 0.28f), Image.Type.Simple);
            Anchor(propPaletteRoot, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, 1f), new Vector2(42f, -132f), new Vector2(-90f, 330f));

            GridLayoutGroup paletteLayout = propPaletteRoot.gameObject.AddComponent<GridLayoutGroup>();
            paletteLayout.padding = new RectOffset(16, 16, 16, 16);
            paletteLayout.spacing = new Vector2(12f, 12f);
            paletteLayout.cellSize = new Vector2(168f, 78f);
            paletteLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            paletteLayout.constraintCount = 5;

            RectTransform actions = CreateFill("BuildActions", panel);
            Anchor(actions, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(42f, 74f), new Vector2(520f, 54f));

            Button clear = CreateButton("Button_ClearDirectorProps", actions, menuButton, Color.white);
            Anchor(clear.GetComponent<RectTransform>(), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), Vector2.zero, new Vector2(180f, 42f));
            clear.onClick.AddListener(ClearDirectorProps);
            TextMeshProUGUI clearLabel = CreateText("TMP", clear.transform as RectTransform, "CLEAR PROPS", 18f, FontStyles.Bold, TextAlignmentOptions.Center);
            Stretch(clearLabel.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            buildStatusText = CreateText("TMP_Status", panel, "", 18f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
            buildStatusText.color = new Color(0.78f, 0.84f, 0.86f, 0.72f);
            Anchor(buildStatusText.rectTransform, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0.5f, 0f), new Vector2(42f, 32f), new Vector2(-120f, 30f));

            EnsurePropPaletteLoaded();
        }

        private void EnsurePropPaletteLoaded()
        {
            if (propPaletteRoot == null || propBindings.Count > 0)
                return;

            List<GameObject> source = new List<GameObject>();
            for (int i = 0; i < directorPlaceableProps.Count; i++)
            {
                if (directorPlaceableProps[i] != null)
                    source.Add(directorPlaceableProps[i]);
            }

            GameObject riftCrystal = Resources.Load<GameObject>(RiftCrystalResourcePath);
#if UNITY_EDITOR
            if (riftCrystal == null)
                riftCrystal = AssetDatabase.LoadAssetAtPath<GameObject>(DefaultRiftCrystalPrefabPath);
#endif
            if (riftCrystal != null && !source.Contains(riftCrystal))
                source.Add(riftCrystal);

            HashSet<GameObject> seen = new HashSet<GameObject>();
            for (int i = 0; i < source.Count; i++)
            {
                GameObject prefab = source[i];
                if (prefab == null || seen.Contains(prefab))
                    continue;

                seen.Add(prefab);
                AddPropButton(propPaletteRoot, prefab);
            }

            if (propBindings.Count > 0)
            {
                SelectProp(propBindings[0]);
                SetBuildStatus($"Loaded {propBindings.Count} prop(s).");
            }
            else
            {
                SetBuildStatus("No director props assigned.");
            }
        }

        private void AddPropButton(RectTransform parent, GameObject prefab)
        {
            string label = prefab != null ? prefab.name : "Prop";
            Button button = CreateButton("Button_Prop_" + SanitizeName(label), parent, slotNormal, Color.white);
            RectTransform rt = button.transform as RectTransform;

            SpriteRenderer prefabRenderer = prefab != null ? prefab.GetComponentInChildren<SpriteRenderer>() : null;
            Image swatch = CreateImage("Swatch", rt, prefabRenderer != null ? prefabRenderer.sprite : null, HtmlColor("#2BD9D9"), Image.Type.Simple);
            Anchor(swatch.rectTransform, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(12f, 0f), new Vector2(32f, 44f));

            TextMeshProUGUI title = CreateText("TMP_Label", rt, label, 16f, FontStyles.Bold, TextAlignmentOptions.MidlineLeft);
            Stretch(title.rectTransform, Vector2.zero, Vector2.one, new Vector2(52f, 22f), new Vector2(-10f, -8f));
            title.enableWordWrapping = true;

            TextMeshProUGUI meta = CreateText("TMP_Meta", rt, "PROP", 11f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
            meta.color = new Color(0.78f, 0.84f, 0.86f, 0.70f);
            Stretch(meta.rectTransform, Vector2.zero, Vector2.one, new Vector2(52f, 6f), new Vector2(-10f, -48f));

            PropBinding binding = new PropBinding(prefab, button.GetComponent<Image>());
            button.onClick.AddListener(() => SelectProp(binding));
            propBindings.Add(binding);
        }

        private void SelectProp(PropBinding binding)
        {
            selectedProp = binding;
            RefreshPropButtons();
            RebuildPropGhost();
            if (binding != null)
                SetBuildStatus("Selected " + binding.Label + ".");
        }

        private void RefreshPropButtons()
        {
            foreach (PropBinding binding in propBindings)
            {
                if (binding.Background != null)
                    binding.Background.sprite = binding == selectedProp && slotActive != null ? slotActive : slotNormal;
            }
        }

        private void UpdatePropTool(float dt)
        {
            EnsurePropPaletteLoaded();

            Mouse mouse = Mouse.current;
            if (mouse == null || selectedProp == null || selectedProp.Prefab == null)
            {
                HidePropGhost();
                return;
            }

            Vector2 screen = mouse.position.ReadValue();
            Vector3 snapped = SnapWorld(MouseScreenToWorld(screen));

            UpdatePropGhost(snapped, dt);

            if (IsPointerOverDirectorUi())
                return;

            if (mouse.leftButton.wasPressedThisFrame)
                PlaceSelectedProp(snapped);
            else if (mouse.rightButton.wasPressedThisFrame)
                ErasePlacedPropAt(snapped);
        }

        private GameObject PlaceSelectedProp(Vector3 position)
        {
            if (selectedProp == null || selectedProp.Prefab == null)
                return null;

            GameObject instance = Instantiate(selectedProp.Prefab, position, Quaternion.identity);
            instance.name = selectedProp.Prefab.name + "_DirectorProp";
            directorPlacedProps.Add(instance);
            StartCoroutine(PopSpawn(instance.transform));
            SetBuildStatus($"Placed {selectedProp.Label}. Count: {directorPlacedProps.Count}.");
            return instance;
        }

        private bool ErasePlacedPropAt(Vector3 position)
        {
            PruneDirectorPlacedProps();
            GameObject target = FindPlacedPropAt(position);
            if (target == null)
            {
                SetBuildStatus("No prop at cursor.");
                return false;
            }

            directorPlacedProps.Remove(target);
            DestroyRuntimeObject(target);
            SetBuildStatus($"Erased prop. Count: {directorPlacedProps.Count}.");
            return true;
        }

        private GameObject FindPlacedPropAt(Vector3 position)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(position, EraseRadius);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i] == null)
                    continue;

                GameObject hit = hits[i].attachedRigidbody != null ? hits[i].attachedRigidbody.gameObject : hits[i].gameObject;
                if (directorPlacedProps.Contains(hit))
                    return hit;
            }

            GameObject nearest = null;
            float nearestSqr = EraseRadius * EraseRadius;
            for (int i = 0; i < directorPlacedProps.Count; i++)
            {
                GameObject prop = directorPlacedProps[i];
                if (prop == null)
                    continue;

                float sqr = ((Vector2)prop.transform.position - (Vector2)position).sqrMagnitude;
                if (sqr <= nearestSqr)
                {
                    nearest = prop;
                    nearestSqr = sqr;
                }
            }

            return nearest;
        }

        private void ClearDirectorProps()
        {
            for (int i = directorPlacedProps.Count - 1; i >= 0; i--)
            {
                GameObject prop = directorPlacedProps[i];
                if (prop != null)
                    DestroyRuntimeObject(prop);
            }

            directorPlacedProps.Clear();
            SetBuildStatus("Cleared placed props.");
        }

        private void PruneDirectorPlacedProps()
        {
            for (int i = directorPlacedProps.Count - 1; i >= 0; i--)
            {
                if (directorPlacedProps[i] == null)
                    directorPlacedProps.RemoveAt(i);
            }
        }

        private void RebuildPropGhost()
        {
            HidePropGhost();
            if (selectedProp == null || selectedProp.Prefab == null)
                return;

            SpriteRenderer prefabRenderer = selectedProp.Prefab.GetComponentInChildren<SpriteRenderer>();
            GameObject ghost = new GameObject("DirectorPropGhost", typeof(SpriteRenderer));
            SpriteRenderer renderer = ghost.GetComponent<SpriteRenderer>();
            renderer.sprite = prefabRenderer != null ? prefabRenderer.sprite : null;
            renderer.sharedMaterial = prefabRenderer != null ? prefabRenderer.sharedMaterial : null;
            renderer.sortingLayerName = prefabRenderer != null ? prefabRenderer.sortingLayerName : "Props";
            renderer.sortingOrder = prefabRenderer != null ? prefabRenderer.sortingOrder + 8 : 108;
            renderer.color = new Color(0.17f, 0.85f, 0.85f, 0.45f);
            ghost.transform.localScale = selectedProp.Prefab.transform.localScale;

            propGhost = ghost;
            propGhostRenderer = renderer;
            propGhostPulse = 0f;
        }

        private void UpdatePropGhost(Vector3 position, float dt)
        {
            if (propGhost == null || propGhostRenderer == null)
                RebuildPropGhost();

            if (propGhost == null)
                return;

            propGhost.SetActive(true);
            propGhost.transform.position = position;
            propGhostPulse += dt * 6f;
            float pulse = 0.92f + Mathf.Sin(propGhostPulse) * 0.04f;
            propGhost.transform.localScale = selectedProp.Prefab.transform.localScale * pulse;
        }

        private void HidePropGhost()
        {
            if (propGhost != null)
                DestroyRuntimeObject(propGhost);

            propGhost = null;
            propGhostRenderer = null;
        }

        private void SetBuildStatus(string message)
        {
            if (buildStatusText != null)
                buildStatusText.text = message;
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

        private void AddTelemetryPanel(RectTransform parent)
        {
            RectTransform panel = CreateFill("Panel_" + DirectorTab.Telemetry, parent);
            CanvasGroup group = panel.gameObject.AddComponent<CanvasGroup>();
            panels[DirectorTab.Telemetry] = group;

            RectTransform window = CreatePanel("Window", panel, minimapFrame, Color.white, Image.Type.Sliced);
            Stretch(window, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            RectTransform header = CreateFill("Header", panel);
            Anchor(header, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -26f), new Vector2(-64f, 74f));

            TextMeshProUGUI titleText = CreateText("TMP_Title", header, Loc.T("director.telemetry.title"), 30f, FontStyles.Bold, TextAlignmentOptions.MidlineLeft);
            Stretch(titleText.rectTransform, Vector2.zero, Vector2.one, new Vector2(34f, 26f), new Vector2(-34f, 0f));
            localizedTexts.Add(new LocalizedTextBinding(titleText, "director.telemetry.title"));

            TextMeshProUGUI hint = CreateText("TMP_Hint", header, Loc.T("director.telemetry.hint"), 20f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
            hint.color = new Color(0.78f, 0.84f, 0.86f, 0.72f);
            Stretch(hint.rectTransform, Vector2.zero, Vector2.one, new Vector2(34f, 0f), new Vector2(-34f, -30f));
            localizedTexts.Add(new LocalizedTextBinding(hint, "director.telemetry.hint"));

            RectTransform metrics = CreateFill("TelemetryMetrics", panel);
            Anchor(metrics, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(42f, -132f), new Vector2(900f, 128f));

            telemetryDpsText = AddTelemetryMetric(metrics, "MetricDps", "director.telemetry.dps", HtmlColor("#E89020"), new Vector2(0f, 0f));
            telemetryTtkText = AddTelemetryMetric(metrics, "MetricTtk", "director.telemetry.ttk", HtmlColor("#FFD24A"), new Vector2(300f, 0f));
            telemetryEventCountText = AddTelemetryMetric(metrics, "MetricEvents", "director.telemetry.events", HtmlColor("#00FFCC"), new Vector2(600f, 0f));

            RectTransform breakdown = CreatePanel("TelemetrySourceBreakdown", panel, null, new Color(0.02f, 0.025f, 0.035f, 0.28f), Image.Type.Simple);
            Anchor(breakdown, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(42f, -292f), new Vector2(760f, 322f));

            VerticalLayoutGroup layout = breakdown.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(18, 18, 18, 18);
            layout.spacing = 8f;
            layout.childAlignment = TextAnchor.UpperLeft;
            layout.childControlHeight = false;
            layout.childControlWidth = true;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = true;

            AddTelemetrySourceRow(breakdown, DamageSourceType.LMB);
            AddTelemetrySourceRow(breakdown, DamageSourceType.RMB);
            AddTelemetrySourceRow(breakdown, DamageSourceType.Skill);
            AddTelemetrySourceRow(breakdown, DamageSourceType.Dot);
            AddTelemetrySourceRow(breakdown, DamageSourceType.Minion);
            AddTelemetrySourceRow(breakdown, DamageSourceType.Item);
            AddTelemetrySourceRow(breakdown, DamageSourceType.Unknown);

            RectTransform actions = CreateFill("TelemetryActions", panel);
            Anchor(actions, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(42f, 74f), new Vector2(520f, 54f));

            Button reset = CreateButton("Button_ClearTelemetry", actions, menuButton, Color.white);
            Anchor(reset.GetComponent<RectTransform>(), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0f), new Vector2(150f, 42f));
            reset.onClick.AddListener(ClearTelemetry);
            AddButtonText(reset, "director.telemetry.clear");

            Button export = CreateButton("Button_ExportTelemetryCsv", actions, ribbonBase, new Color(0.80f, 0.34f, 0.09f, 1f));
            Anchor(export.GetComponent<RectTransform>(), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(170f, 0f), new Vector2(210f, 48f));
            export.onClick.AddListener(ExportTelemetryCsv);
            AddButtonText(export, "director.telemetry.export");

            telemetryStatusText = CreateText("TMP_Status", panel, "", 18f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
            telemetryStatusText.color = new Color(0.78f, 0.84f, 0.86f, 0.72f);
            Anchor(telemetryStatusText.rectTransform, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0.5f, 0f), new Vector2(42f, 32f), new Vector2(-120f, 30f));

            UpdateTelemetryDisplay(true);
        }

        private TextMeshProUGUI AddTelemetryMetric(RectTransform parent, string name, string locKey, Color color, Vector2 position)
        {
            RectTransform card = CreatePanel(name, parent, rewardCard, Color.white, Image.Type.Sliced);
            Anchor(card, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f), position, new Vector2(276f, 110f));

            TextMeshProUGUI label = CreateText("TMP_Label", card, Loc.T(locKey), 18f, FontStyles.Bold, TextAlignmentOptions.MidlineLeft);
            label.color = color;
            Stretch(label.rectTransform, Vector2.zero, Vector2.one, new Vector2(18f, 62f), new Vector2(-18f, -12f));
            localizedTexts.Add(new LocalizedTextBinding(label, locKey));

            TextMeshProUGUI value = CreateText("TMP_Value", card, "0", 34f, FontStyles.Bold, TextAlignmentOptions.MidlineLeft);
            Stretch(value.rectTransform, Vector2.zero, Vector2.one, new Vector2(18f, 12f), new Vector2(-18f, -44f));
            return value;
        }

        private void AddTelemetrySourceRow(RectTransform parent, DamageSourceType sourceType)
        {
            RectTransform row = CreateFill("Row_Source_" + sourceType, parent);
            row.sizeDelta = new Vector2(0f, 32f);
            LayoutElement layout = row.gameObject.AddComponent<LayoutElement>();
            layout.preferredHeight = 32f;
            layout.minHeight = 32f;

            Color color = SourceTelemetryColor(sourceType);
            TextMeshProUGUI label = CreateText("TMP_Label", row, sourceType.ToString().ToUpperInvariant(), 18f, FontStyles.Bold, TextAlignmentOptions.MidlineLeft);
            label.color = color;
            Anchor(label.rectTransform, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0f), new Vector2(118f, 28f));

            Image background = CreateImage("BarBackground", row, null, new Color(0.06f, 0.07f, 0.09f, 0.95f), Image.Type.Simple);
            Anchor(background.rectTransform, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(132f, 0f), new Vector2(420f, 20f));

            Image fill = CreateImage("BarFill", background.rectTransform, null, color, Image.Type.Filled);
            fill.type = Image.Type.Filled;
            fill.fillMethod = Image.FillMethod.Horizontal;
            fill.fillOrigin = (int)Image.OriginHorizontal.Left;
            fill.fillAmount = 0f;
            Stretch(fill.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            TextMeshProUGUI value = CreateText("TMP_Value", row, "0  0%", 18f, FontStyles.Bold, TextAlignmentOptions.MidlineRight);
            Anchor(value.rectTransform, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(570f, 0f), new Vector2(130f, 28f));

            telemetrySourceBindings.Add(new TelemetrySourceBinding(sourceType, fill, value));
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

            bool implemented = IsDirectorClassImplemented(type);
            button.interactable = implemented;
            label.color = implemented ? label.color : new Color(label.color.r, label.color.g, label.color.b, 0.35f);
            if (implemented)
            {
                button.onClick.AddListener(() => SelectDirectorClass(type));
            }

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
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Health playerHealth = player != null ? player.GetComponentInChildren<Health>() : null;
            if (playerHealth != null && playerHealth.IsDead)
            {
                SetClassSkillStatus("director.class_skill.status.player_dead");
                return;
            }

            if (!IsDirectorClassImplemented(type))
            {
                SetClassSkillStatus("director.class_skill.status.no_skills", type);
                return;
            }

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

        private static bool IsDirectorClassImplemented(ClassType type)
        {
            switch (type)
            {
                case ClassType.Warblade:
                case ClassType.Elementalist:
                case ClassType.Shadowblade:
                case ClassType.Ranger:
                case ClassType.Ronin:
                    return true;
                default:
                    return false;
            }
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
            float oldValue = binding.Read(stats);
            float clamped = ClampStatValue(binding, value);
            binding.Write(stats, clamped);
            manager.ApplyCurrentPrimaryStatsToPlayer();
            binding.SetValueText(binding.Read(stats));
            ShowStatToast(binding, oldValue, binding.Read(stats));
            SetStatsStatus("director.stats.status.live", manager.PrimaryClass);
        }

        private void ShowStatToast(StatSliderBinding binding, float oldValue, float newValue)
        {
            if (binding == null || statToastText == null || statToastGroup == null || Mathf.Approximately(oldValue, newValue))
            {
                return;
            }

            statToastText.text = binding.Key + " " + FormatStatValue(binding, oldValue) + " -> " + FormatStatValue(binding, newValue);
            if (!Application.isPlaying)
            {
                return;
            }

            if (statToastRoutine != null)
            {
                StopCoroutine(statToastRoutine);
            }

            statToastRoutine = StartCoroutine(PlayStatToast());
        }

        private IEnumerator PlayStatToast()
        {
            statToastGroup.alpha = 1f;
            float elapsed = 0f;
            while (elapsed < StatToastDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float fade = Mathf.InverseLerp(StatToastDuration, StatToastDuration * 0.55f, elapsed);
                statToastGroup.alpha = Mathf.Clamp01(fade);
                yield return null;
            }

            statToastGroup.alpha = 0f;
            statToastRoutine = null;
        }

        private static string FormatStatValue(StatSliderBinding binding, float value)
        {
            return binding.Slider != null && binding.Slider.wholeNumbers
                ? Mathf.RoundToInt(value).ToString(CultureInfo.InvariantCulture)
                : value.ToString("0.##", CultureInfo.InvariantCulture);
        }

        private static float ClampStatValue(StatSliderBinding binding, float value)
        {
            if (binding == null || binding.Slider == null)
            {
                return value;
            }

            float clamped = Mathf.Clamp(value, binding.Slider.minValue, binding.Slider.maxValue);
            return binding.Slider.wholeNumbers ? Mathf.Round(clamped) : clamped;
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

        private void OnDamageAppliedTelemetry(DamageTelemetryEvent telemetryEvent)
        {
            if (telemetryEvent.FinalDamage <= 0)
            {
                return;
            }

            DamagePacket packet = telemetryEvent.Packet;
            DamageSourceType sourceType = packet.sourceType;
            Health target = telemetryEvent.TargetHealth;
            // Pause-arındırılmış saat: damage yalnızca oyun akarken üretilir, bu yüzden kayıt zaman damgası
            // TelemetryClock ile alınmalı (now ile aynı eksen). Olayın ham UnscaledTime'ı pause biriktirmez.
            float eventTime = TelemetryClock();

            telemetryRecords.Add(new DamageTelemetryRecord(
                eventTime,
                telemetryEvent.FinalDamage,
                packet.damageType,
                packet.elementTag,
                sourceType,
                packet.sourceId,
                ObjectName(packet.attacker),
                target != null ? ObjectName(target.gameObject) : ObjectName(packet.target)));

            telemetryDamageBySource.TryGetValue(sourceType, out int currentDamage);
            telemetryDamageBySource[sourceType] = currentDamage + telemetryEvent.FinalDamage;

            if (target != null && !telemetryFirstHitTimes.ContainsKey(target))
            {
                telemetryFirstHitTimes[target] = eventTime;
                UnityAction listener = () => OnTelemetryTargetDeath(target);
                telemetryDeathListeners[target] = listener;
                if (target.OnDeath != null)
                {
                    target.OnDeath.AddListener(listener);
                }
            }

            if (target != null && target.IsDead)
            {
                CompleteTelemetryTtk(target, eventTime);
            }

            if (telemetryStatusText != null)
            {
                telemetryStatusText.text = Loc.T("director.telemetry.status.live", telemetryEvent.FinalDamage, sourceType);
            }

            UpdateTelemetryDisplay(true);
        }

        private void OnTelemetryTargetDeath(Health target)
        {
            CompleteTelemetryTtk(target, TelemetryClock());
        }

        private void CompleteTelemetryTtk(Health target, float deathTime)
        {
            if (target == null || !telemetryFirstHitTimes.TryGetValue(target, out float startTime))
            {
                return;
            }

            telemetryLastTtk = Mathf.Max(0f, deathTime - startTime);
            RemoveTelemetryDeathListener(target);
            telemetryFirstHitTimes.Remove(target);
            UpdateTelemetryDisplay(true);
        }

        private void ClearTelemetry()
        {
            telemetryRecords.Clear();
            telemetryDamageBySource.Clear();
            telemetryFirstHitTimes.Clear();
            ClearTelemetryDeathListeners();
            telemetryLastTtk = -1f;
            telemetryLastCsv = "";
            telemetryLastDps = 0f;
            telemetryHasLastDps = false;
            telemetryLastRefreshTime = -1f;
            // Pause saatini sıfırla; hâlâ pause'daysak başlangıcı yeni baz ana çek (offset birikmesin).
            telemetryPausedTotal = 0f;
            telemetryPauseStartTime = State == DirectorModeState.Director ? Time.unscaledTime : -1f;
            if (telemetryStatusText != null)
            {
                telemetryStatusText.text = Loc.T("director.telemetry.status.cleared");
            }
            UpdateTelemetryDisplay(true);
        }

        private void ClearTelemetryDeathListeners()
        {
            foreach (KeyValuePair<Health, UnityAction> pair in telemetryDeathListeners)
            {
                if (pair.Key != null && pair.Key.OnDeath != null)
                {
                    pair.Key.OnDeath.RemoveListener(pair.Value);
                }
            }

            telemetryDeathListeners.Clear();
        }

        private void RemoveTelemetryDeathListener(Health target)
        {
            if (target == null || !telemetryDeathListeners.TryGetValue(target, out UnityAction listener))
            {
                return;
            }

            if (target.OnDeath != null)
            {
                target.OnDeath.RemoveListener(listener);
            }
            telemetryDeathListeners.Remove(target);
        }

        private void UpdateTelemetryDisplay(bool force)
        {
            float now = TelemetryClock();
            if (!force && now - telemetryLastRefreshTime < TelemetryRefreshInterval)
            {
                return;
            }

            telemetryLastRefreshTime = now;

            if (telemetryDpsText != null)
            {
                telemetryDpsText.text = CalculateTelemetryDps(now).ToString("0.0", CultureInfo.InvariantCulture);
            }

            if (telemetryTtkText != null)
            {
                telemetryTtkText.text = telemetryLastTtk >= 0f
                    ? telemetryLastTtk.ToString("0.00", CultureInfo.InvariantCulture) + "s"
                    : "--";
            }

            if (telemetryEventCountText != null)
            {
                telemetryEventCountText.text = telemetryRecords.Count.ToString(CultureInfo.InvariantCulture);
            }

            int totalDamage = 0;
            foreach (KeyValuePair<DamageSourceType, int> pair in telemetryDamageBySource)
            {
                totalDamage += pair.Value;
            }

            for (int i = 0; i < telemetrySourceBindings.Count; i++)
            {
                TelemetrySourceBinding binding = telemetrySourceBindings[i];
                telemetryDamageBySource.TryGetValue(binding.SourceType, out int sourceDamage);
                float ratio = totalDamage > 0 ? (float)sourceDamage / totalDamage : 0f;
                if (binding.Fill != null)
                {
                    binding.Fill.fillAmount = ratio;
                }
                if (binding.ValueText != null)
                {
                    binding.ValueText.text = sourceDamage.ToString(CultureInfo.InvariantCulture) + "  " + (ratio * 100f).ToString("0", CultureInfo.InvariantCulture) + "%";
                }
            }
        }

        // Director pause süresini biriktir: pause'a girince başlangıcı kaydet, çıkınca süreyi toplama ekle.
        // Böylece TelemetryClock pause boyunca ilerlemez ve DPS penceresi kaldığı yerden devam eder.
        private void UpdateTelemetryPauseClock(DirectorModeState nextState)
        {
            bool wasPaused = State == DirectorModeState.Director;
            bool willPause = nextState == DirectorModeState.Director;
            if (willPause && !wasPaused)
            {
                telemetryPauseStartTime = Time.unscaledTime;
            }
            else if (!willPause && wasPaused && telemetryPauseStartTime >= 0f)
            {
                telemetryPausedTotal += Time.unscaledTime - telemetryPauseStartTime;
                telemetryPauseStartTime = -1f;
            }
        }

        // Pause süresinden arındırılmış telemetri saati. Kayıt zaman damgaları ve DPS yaşlandırması bunu kullanır.
        // AKTİF pause aralığı da düşülür: pause boyunca saat DONAR (çıkışta toplama eklenene kadar beklemez).
        private float TelemetryClock()
        {
            float activePause = (State == DirectorModeState.Director && telemetryPauseStartTime >= 0f)
                ? Time.unscaledTime - telemetryPauseStartTime
                : 0f;
            return Time.unscaledTime - telemetryPausedTotal - activePause;
        }

        private float CalculateTelemetryDps(float now)
        {
            if (State == DirectorModeState.Director && telemetryHasLastDps)
            {
                return telemetryLastDps;
            }

            float oldest = now;
            int damage = 0;
            for (int i = telemetryRecords.Count - 1; i >= 0; i--)
            {
                DamageTelemetryRecord record = telemetryRecords[i];
                if (now - record.Time > TelemetryDpsWindowSeconds)
                {
                    break;
                }

                damage += record.FinalDamage;
                oldest = Mathf.Min(oldest, record.Time);
            }

            if (damage <= 0)
            {
                telemetryLastDps = 0f;
                telemetryHasLastDps = true;
                return 0f;
            }

            float duration = Mathf.Clamp(now - oldest, 0.1f, TelemetryDpsWindowSeconds);
            telemetryLastDps = damage / duration;
            telemetryHasLastDps = true;
            return telemetryLastDps;
        }

        private void ExportTelemetryCsv()
        {
            telemetryLastCsv = BuildTelemetryCsv();
            GUIUtility.systemCopyBuffer = telemetryLastCsv;
            Debug.Log("[DirectorMode] Telemetry CSV export:\n" + telemetryLastCsv);
            if (telemetryStatusText != null)
            {
                telemetryStatusText.text = Loc.T("director.telemetry.status.exported", telemetryRecords.Count);
            }
        }

        private string BuildTelemetryCsv()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("time,finalDamage,damageType,elementTag,sourceType,sourceId,attacker,target");
            for (int i = 0; i < telemetryRecords.Count; i++)
            {
                DamageTelemetryRecord record = telemetryRecords[i];
                builder.Append(record.Time.ToString("0.000", CultureInfo.InvariantCulture));
                builder.Append(',');
                builder.Append(record.FinalDamage.ToString(CultureInfo.InvariantCulture));
                builder.Append(',');
                builder.Append(record.DamageType);
                builder.Append(',');
                builder.Append(record.ElementTag);
                builder.Append(',');
                builder.Append(record.SourceType);
                builder.Append(',');
                AppendCsv(builder, record.SourceId);
                builder.Append(',');
                AppendCsv(builder, record.AttackerName);
                builder.Append(',');
                AppendCsv(builder, record.TargetName);
                builder.AppendLine();
            }
            return builder.ToString();
        }

        private static void AppendCsv(StringBuilder builder, string value)
        {
            value ??= "";
            bool quote = value.IndexOfAny(new[] { ',', '"', '\n', '\r' }) >= 0;
            if (!quote)
            {
                builder.Append(value);
                return;
            }

            builder.Append('"');
            builder.Append(value.Replace("\"", "\"\""));
            builder.Append('"');
        }

        private static string ObjectName(GameObject go)
        {
            return go != null ? go.name : "";
        }

        private static Color SourceTelemetryColor(DamageSourceType sourceType)
        {
            switch (sourceType)
            {
                case DamageSourceType.LMB: return HtmlColor("#F4F0E6");
                case DamageSourceType.RMB: return HtmlColor("#00FFCC");
                case DamageSourceType.Skill: return HtmlColor("#E89020");
                case DamageSourceType.Dot: return HtmlColor("#7B3FA8");
                case DamageSourceType.Minion: return HtmlColor("#FFF0B0");
                case DamageSourceType.Item: return HtmlColor("#FFD24A");
                default: return HtmlColor("#8A9098");
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
            reset.onClick.AddListener(DemoQuickReset);
            TextMeshProUGUI label = CreateText("TMP", reset.transform as RectTransform, "QUICK RESET", 16f, FontStyles.Bold, TextAlignmentOptions.Center);
            Stretch(label.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        }

        private void BuildStatToast(RectTransform root)
        {
            RectTransform toast = CreatePanel("StatToast", root, menuButton, Color.white, Image.Type.Sliced);
            Anchor(toast, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 170f), new Vector2(440f, 78f));
            statToastGroup = toast.gameObject.AddComponent<CanvasGroup>();
            statToastGroup.alpha = 0f;
            statToastGroup.interactable = false;
            statToastGroup.blocksRaycasts = false;

            statToastText = CreateText("TMP", toast, "", 32f, FontStyles.Bold, TextAlignmentOptions.Center);
            statToastText.color = HtmlColor("#00FFCC");
            Stretch(statToastText.rectTransform, Vector2.zero, Vector2.one, new Vector2(18f, 0f), new Vector2(-18f, 0f));
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

        private readonly struct DamageTelemetryRecord
        {
            public readonly float Time;
            public readonly int FinalDamage;
            public readonly DamageType DamageType;
            public readonly ElementTag ElementTag;
            public readonly DamageSourceType SourceType;
            public readonly string SourceId;
            public readonly string AttackerName;
            public readonly string TargetName;

            public DamageTelemetryRecord(float time, int finalDamage, DamageType damageType, ElementTag elementTag, DamageSourceType sourceType, string sourceId, string attackerName, string targetName)
            {
                Time = time;
                FinalDamage = finalDamage;
                DamageType = damageType;
                ElementTag = elementTag;
                SourceType = sourceType;
                SourceId = sourceId;
                AttackerName = attackerName;
                TargetName = targetName;
            }
        }

        private readonly struct TelemetrySourceBinding
        {
            public readonly DamageSourceType SourceType;
            public readonly Image Fill;
            public readonly TextMeshProUGUI ValueText;

            public TelemetrySourceBinding(DamageSourceType sourceType, Image fill, TextMeshProUGUI valueText)
            {
                SourceType = sourceType;
                Fill = fill;
                ValueText = valueText;
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

        private sealed class SpawnEnemyBinding
        {
            public readonly EncounterEnemyType Type;
            public readonly GameObject Prefab;
            public readonly Image Background;

            public SpawnEnemyBinding(EncounterEnemyType type, GameObject prefab, Image background)
            {
                Type = type;
                Prefab = prefab;
                Background = background;
            }

            public string Label => Prefab != null ? Prefab.name : Type.ToString();
        }

        private sealed class PropBinding
        {
            public readonly GameObject Prefab;
            public readonly Image Background;

            public PropBinding(GameObject prefab, Image background)
            {
                Prefab = prefab;
                Background = background;
            }

            public string Label => Prefab != null ? Prefab.name : "Prop";
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
