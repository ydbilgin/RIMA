using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using RIMA.DebugTools;
using RIMA.CameraSystem;
using RIMA.Environment;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Runtime;
using RIMA.Systems.Map;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace RIMA
{
    public sealed class ChamberSelectBootstrap : MonoBehaviour
    {
        private const string RoomAssetPath = "Assets/Resources/ChamberSelect/Chamber_CharSelect.asset";
        private const string RoomResourcePath = "ChamberSelect/Chamber_CharSelect";
        private const string RegistryAssetPath = "Assets/Resources/Props/PropRegistry.asset";
        private const string RegistryResourcePath = "Props/PropRegistry";
        private const string FloorTileResource = "ChamberSelect/Tiles/ChamberFloor";
        private const string CollisionTileResource = "ChamberSelect/Tiles/ChamberCollision";
        private const string OverlayTileResource = "ChamberSelect/Tiles/ChamberOverlayPath";
        private const string ChamberExitPortalResource = "Environment/Gate/gate_arch";
        private const string ChamberExitPortalEditorPath = "Assets/Resources/Environment/Portal/portal_rift.png";
        private const string ChamberExitPortalResourcePath = "Environment/Portal/portal_rift";
        private const string ArenaRunSceneName = "_Arena";
        private const float ClassInteractRadius = 1.5f;
        private const float ClassConfirmRadius = 1.7f;
        private const float DoorInteractRadius = 2.5f;
        private const float DummyInteractRadius = 1.6f;
        [SerializeField, Min(1f)] private float chamberCameraFitMultiplier = 1.08f;
        [SerializeField, Min(0f)] private float chamberCameraFitPadding = 0.55f;
        [SerializeField, Min(1f)] private float chamberCameraMinimumOrthographicSize = 6.2f;
        [SerializeField, Range(0.45f, 0.72f)] private float chamberPlayerScreenY = 0.60f;
        // Chamber fit baseline; CalculateChamberCameraOrthoSize expands this when the crescent needs it.
        [SerializeField, Range(2.8f, 8f)] private float chamberCameraOrthoSize = 6.2f;
        [SerializeField, Min(0f)] private float chamberGlobalLightIntensity = 1.10f;
        [SerializeField, Min(0f)] private float chamberFillLightIntensity = 0.35f;
        // FIX E: dummy scale and cell override (x=0,y=0 means use auto placement)
        [SerializeField] private Vector2Int chamberDummyCellOverride = new Vector2Int(0, 0);
        [SerializeField, Range(0.8f, 2.5f)] private float chamberDummyScale = 1.70f;

        // DEMO LOCK (2026-06-10): all echoes are visible, but only Warblade + Elementalist
        // can be selected until every class has a complete demo-safe kit.
        private static readonly ClassType[] ChamberClasses =
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
            ClassType.Hexer,
        };

        private static readonly Vector2Int ExitCell = new(14, 18);
        private static readonly Vector2Int LegacyExitArtifactCell = new(24, 17);

        private readonly Dictionary<ClassType, EchoStation> stations = new();
        private readonly List<EchoStationLayout> stationLayouts = new();

        private CharacterSelectScreen classicScreen;
        private Canvas classicCanvas;
        private CanvasGroup classicCanvasGroup;
        private MethodInfo selectClassMethod;
        private Grid grid;
        private Tilemap groundTilemap;
        private Tilemap collisionTilemap;
        private IsoRoomBuilder builder;
        private RoomTemplateSO roomTemplate;
        private Transform player;
        private Camera chamberCamera;
        private TMP_Text promptLabel;
        private Canvas chamberOverlayCanvas;
        private CanvasGroup promptCanvasGroup;
        private CanvasGroup _promptBorderCanvasGroup;
        private GameObject _promptBorderGo;
        private RectTransform promptTextRect;
        private GameObject promptKeycap;
        private Coroutine promptFadeRoutine;
        private TextMeshProUGUI chamberEchoBalanceLabel;
        private int lastEchoBalance = int.MinValue;
        private ClassType currentClass = ClassType.Warblade;
        private ClassType highlightedClass = ClassType.None;
        private ClassType pendingConfirmClass = ClassType.None;
        private bool classicTabOpen;
        private bool dummySelectOpen;
        private bool busyAttuning;
        // Chamber-only skill picker (K): assign any implemented active skill of the current class to Q/E/R/F.
        private bool skillPickerOpen;
        private int pickerSelectedSkill = -1;          // index into the current class's implemented active-skill list
        private readonly string[] pickerSlotKit = new string[4];   // pending Q/E/R/F selection (null = keep default)
        private Vector2 pickerSkillScroll;
        private float practiceRefillTimer;   // chamber-only: keep skill resource topped so Q/E/R/F can be practised
        private SkillBarUI chamberSkillBar;   // chamber-only practice skill bar (Q/E/R/F readout)
        private Vector3 exitWorld;
        private Vector2Int chamberSpawnCell;
        private Vector2Int chamberDummyCell;
        private Transform dummyTransform;
        private SpriteRenderer dummyRenderer;
        private CameraZoom disabledCameraZoom;
        private Behaviour disabledLegacyPixelPerfectCamera;
        private PixelPerfectCamera disabledUrpPixelPerfectCamera;
        private RIMA.CameraSystem.CameraFollow chamberFollow;
        private Vector3 previousFollowOffset;
        private Transform previousFollowTarget;
        private bool previousFollowCaptured;

        private void Start()
        {
            StartCoroutine(BootstrapRoutine());
        }

        private IEnumerator BootstrapRoutine()
        {
            EchoWallet.EnsureInitialized();
            classicScreen = GetComponent<CharacterSelectScreen>();
            selectClassMethod = typeof(CharacterSelectScreen).GetMethod("SelectClass", BindingFlags.Instance | BindingFlags.NonPublic);

            yield return null;
            CacheClassicCanvas();
            SetClassicOverlayVisible(false);

            BuildWorldRoom();
            RemoveChamberEnemyLeakage();
            SpawnPlayer();
            SpawnEchoStations();
            SpawnTrainingDummy();
            CreatePromptLabel();
            ConfigureCameraAndLight();
            RefreshEchoVisuals();
            ApplyAtmospherePass();

            Debug.Log("[ChamberSelectBootstrap] P2 chamber bootstrap complete: room built, player spawned, echo stations and dummy active.");
        }

        private void ApplyAtmospherePass()
        {
            Transform chamberRoot = GameObject.Find("AttunementChamber_Runtime")?.transform;
            if (chamberRoot == null)
            {
                return;
            }

            SpawnChamberExitPortal(chamberRoot);

            Vector3 doorPos = exitWorld + new Vector3(0f, 0.5f, 0f);
            GameObject doorLightGo = new GameObject("DoorLight");
            doorLightGo.transform.SetParent(chamberRoot);
            doorLightGo.transform.position = doorPos;
            Light2D doorLight = doorLightGo.AddComponent<Light2D>();
            doorLight.lightType = Light2D.LightType.Point;
            doorLight.intensity = 1.2f;
            doorLight.pointLightOuterRadius = 4.5f;
            doorLight.color = new Color(0.6f, 0.8f, 1f, 1f);

        }

        private void SpawnChamberExitPortal(Transform chamberRoot)
        {
            GameObject portalGo = new GameObject("ChamberExitPortal");
            portalGo.transform.SetParent(chamberRoot);
            // FIX A: ground portal — base sits on floor (was 0.28f which floated it).
            portalGo.transform.position = exitWorld + new Vector3(0f, 0.04f, 0f);
            portalGo.transform.localScale = new Vector3(0.78f, 0.78f, 1f);

            SpriteRenderer sr = portalGo.AddComponent<SpriteRenderer>();
            sr.sprite = LoadAsset<Sprite>(ChamberExitPortalEditorPath, ChamberExitPortalResourcePath)
                ?? Resources.Load<Sprite>(ChamberExitPortalResource)
                ?? LoadAsset<Sprite>("Assets/Resources/Environment/Gate/gate_arch.png", ChamberExitPortalResource);
            sr.color = Color.white;
            sr.sortingLayerName = "Characters";
            sr.sortingOrder = SortOrder(portalGo.transform.position) - 2;

            // FIX F: solid body-blocker at portal base so player cannot walk behind/through it.
            // Size matches only the arch base footprint (narrow width, short height).
            Rigidbody2D portalBody = portalGo.AddComponent<Rigidbody2D>();
            portalBody.bodyType = RigidbodyType2D.Static;
            portalBody.gravityScale = 0f;
            BoxCollider2D blocker = portalGo.AddComponent<BoxCollider2D>();
            blocker.offset = new Vector2(0f, -0.05f);
            blocker.size = new Vector2(0.55f, 0.22f);   // base footprint only
            blocker.isTrigger = false;
            // Set to Default layer so it collides with the player.
            portalGo.layer = 0;

            // FIX F: separate larger trigger for [G] proximity prompt detection.
            BoxCollider2D proximityTrigger = portalGo.AddComponent<BoxCollider2D>();
            proximityTrigger.offset = new Vector2(0f, 0.15f);
            proximityTrigger.size = new Vector2(1.2f, 0.9f);
            proximityTrigger.isTrigger = true;
        }

        private void OnDestroy()
        {
            if (disabledCameraZoom != null)
            {
                disabledCameraZoom.enabled = true;
                disabledCameraZoom = null;
            }

            if (disabledLegacyPixelPerfectCamera != null)
            {
                disabledLegacyPixelPerfectCamera.enabled = true;
                disabledLegacyPixelPerfectCamera = null;
            }

            if (disabledUrpPixelPerfectCamera != null)
            {
                disabledUrpPixelPerfectCamera.enabled = true;
                disabledUrpPixelPerfectCamera = null;
            }

            if (chamberFollow != null && previousFollowCaptured)
            {
                chamberFollow.worldOffset = previousFollowOffset;
                chamberFollow.target = previousFollowTarget;
            }
        }

        private void Update()
        {
            RefreshChamberEchoHud();

            if (player == null || busyAttuning)
            {
                return;
            }

            // Chamber practice: keep the active skill resource topped (Rage decays to 0 otherwise)
            // so the player can repeatedly cast Q/E/R/F on the dummy without grinding.
            practiceRefillTimer -= Time.deltaTime;
            if (practiceRefillTimer <= 0f)
            {
                practiceRefillTimer = 0.5f;
                TopUpPracticeResource();
            }

            if (WasPressed(UnityEngine.InputSystem.Keyboard.current?.tabKey))
            {
                classicTabOpen = !classicTabOpen;
                SetClassicOverlayVisible(classicTabOpen);
            }

            ClassType nearbyClass = FindNearbyClassStation(out EchoStation nearbyStation);
            bool nearDoor = Vector2.Distance(player.position, exitWorld) <= DoorInteractRadius;
            bool nearDummy = dummyTransform != null && Vector2.Distance(player.position, dummyTransform.position) <= DummyInteractRadius;

            // Chamber-only: K toggles the skill picker while near the dummy; auto-close when leaving.
            if (nearDummy && IsDemoSelectable(currentClass) &&
                WasPressed(UnityEngine.InputSystem.Keyboard.current?.kKey))
            {
                skillPickerOpen = !skillPickerOpen;
                if (skillPickerOpen) pickerSelectedSkill = -1;
            }
            if (skillPickerOpen && !nearDummy) skillPickerOpen = false;

            if (dummySelectOpen && !nearDummy)
            {
                dummySelectOpen = false;
                classicTabOpen = false;
                SetClassicOverlayVisible(false);
            }

            // FIX 2 (broad): while dummy overlay is open, suppress ALL world prompts and skip interaction.
            if (dummySelectOpen)
            {
                HidePrompt();
                RefreshEchoVisuals();
                return;
            }

            if (pendingConfirmClass != ClassType.None)
            {
                bool stillNearPending = IsNearClassStation(pendingConfirmClass, ClassConfirmRadius, out EchoStation pendingStation);
                highlightedClass = stillNearPending ? pendingConfirmClass : ClassType.None;
                SetClassicOverlayVisible(classicTabOpen);

                if (!stillNearPending || WasCancelPressed())
                {
                    pendingConfirmClass = ClassType.None;
                    HidePrompt();
                }
                else
                {
                    ShowPrompt(Vector3.zero,
                        $"[G] {pendingConfirmClass.ToString().ToUpperInvariant()} — Onayla    ESC: İptal");

                    if (WasConfirmPressed())
                    {
                        ClassType confirmed = pendingConfirmClass;
                        pendingConfirmClass = ClassType.None;
                        if (IsUnlocked(confirmed))
                        {
                            HidePrompt();
                            StartCoroutine(AttuneRoutine(confirmed));
                        }
                    }
                }

                RefreshEchoVisuals();
                return;
            }

            if (nearbyClass != ClassType.None && nearbyStation != null)
            {
                highlightedClass = nearbyClass;
                SetClassicOverlayVisible(classicTabOpen);

                if (!IsDemoSelectable(nearbyClass))
                {
                    ShowPrompt(Vector3.zero,
                        $"{nearbyClass.ToString().ToUpperInvariant()} — {Loc.T("char_select.in_development")}",
                        PromptTint.Unaffordable);
                }
                else if (!IsUnlocked(nearbyClass))
                {
                    int cost = UnlockCost(nearbyClass);
                    bool affordable = CanUnlock(nearbyClass);
                    string lockedPrompt = affordable
                        ? $"[G] {nearbyClass.ToString().ToUpperInvariant()} — Kilidi Aç ({cost} Echo)"
                        : $"{nearbyClass.ToString().ToUpperInvariant()} — Kilitli ({cost} Echo)";
                    ShowPrompt(Vector3.zero, lockedPrompt, affordable ? PromptTint.Normal : PromptTint.Unaffordable);

                    if (WasPressed(UnityEngine.InputSystem.Keyboard.current?.gKey) && affordable)
                    {
                        Unlock(nearbyClass);
                        Debug.Log($"[ChamberSelectBootstrap] Class figure unlocked {nearbyClass} for {cost} Shattered Echo.");
                    }
                }
                else
                {
                    ShowPrompt(Vector3.zero, $"[G] {nearbyClass.ToString().ToUpperInvariant()} — Bürün");

                    if (WasPressed(UnityEngine.InputSystem.Keyboard.current?.gKey))
                    {
                        pendingConfirmClass = nearbyClass;
                    }
                }
            }
            else if (nearDoor)
            {
                highlightedClass = ClassType.None;
                SetClassicOverlayVisible(classicTabOpen);
                // FIX B: fixed bottom-center prompt — pass Vector3.zero (world pos ignored by panel).
                ShowPrompt(Vector3.zero, "[G] RİFT — Gir");

                if (WasPressed(UnityEngine.InputSystem.Keyboard.current?.gKey))
                {
                    StartRun();
                }
            }
            else if (nearDummy)
            {
                highlightedClass = ClassType.None;
                SetClassicOverlayVisible(dummySelectOpen || classicTabOpen);
                ShowPrompt(Vector3.zero, "Dummy — LMB + Q/E/R/F ile dene    [K] Skill Seç    [G] Sınıf Seç");

                if (WasPressed(UnityEngine.InputSystem.Keyboard.current?.gKey))
                {
                    dummySelectOpen = !dummySelectOpen;
                    classicTabOpen = dummySelectOpen;
                    if (dummySelectOpen)
                    {
                        InvokeClassicSelect(currentClass);
                    }
                    SetClassicOverlayVisible(dummySelectOpen);
                }
            }
            else
            {
                highlightedClass = ClassType.None;
                SetClassicOverlayVisible(classicTabOpen);
                HidePrompt();
            }

            RefreshEchoVisuals();
        }

        private static bool WasPressed(UnityEngine.InputSystem.Controls.ButtonControl key)
        {
            return key != null && key.wasPressedThisFrame;
        }

        private static bool WasConfirmPressed()
        {
            UnityEngine.InputSystem.Keyboard keyboard = UnityEngine.InputSystem.Keyboard.current;
            return WasPressed(keyboard?.gKey) || WasPressed(keyboard?.enterKey) || WasPressed(keyboard?.numpadEnterKey);
        }

        private static bool WasCancelPressed()
        {
            return WasPressed(UnityEngine.InputSystem.Keyboard.current?.escapeKey);
        }

        private enum PromptTint { Normal, Unaffordable }

        private void ShowPrompt(Vector3 position, string text, PromptTint tint = PromptTint.Normal)
        {
            if (promptLabel == null || promptCanvasGroup == null)
            {
                return;
            }

            bool hasKeycap = text.Contains("[G]");
            string displayText = text.Replace("[G] ", string.Empty).Replace("[G]", string.Empty).Trim();
            promptKeycap.SetActive(hasKeycap);
            if (promptTextRect != null)
            {
                promptTextRect.offsetMin = hasKeycap ? new Vector2(58f, 0f) : new Vector2(12f, 0f);
            }

            promptLabel.color = tint == PromptTint.Unaffordable
                ? new Color(0.72f, 0.38f, 0.38f, 1f)   // muted red-grey for locked/unaffordable
                : new Color(0.85f, 0.97f, 1f, 1f);     // normal cyan-white

            bool wasHidden = !promptCanvasGroup.gameObject.activeSelf;
            promptLabel.text = displayText;
            promptCanvasGroup.gameObject.SetActive(true);
            if (_promptBorderGo != null) _promptBorderGo.SetActive(true);
            if (wasHidden)
            {
                if (promptFadeRoutine != null)
                {
                    StopCoroutine(promptFadeRoutine);
                }

                promptFadeRoutine = StartCoroutine(FadePromptIn());
            }
        }

        private void HidePrompt()
        {
            if (promptFadeRoutine != null)
            {
                StopCoroutine(promptFadeRoutine);
                promptFadeRoutine = null;
            }

            if (promptCanvasGroup != null)
            {
                promptCanvasGroup.gameObject.SetActive(false);
            }

            if (_promptBorderGo != null) _promptBorderGo.SetActive(false);
        }

        private IEnumerator FadePromptIn()
        {
            const float duration = 0.14f;
            promptCanvasGroup.alpha = 0f;
            if (_promptBorderCanvasGroup != null) _promptBorderCanvasGroup.alpha = 0f;
            for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
            {
                float a = Mathf.Clamp01(t / duration);
                promptCanvasGroup.alpha = a;
                if (_promptBorderCanvasGroup != null) _promptBorderCanvasGroup.alpha = a;
                yield return null;
            }

            promptCanvasGroup.alpha = 1f;
            if (_promptBorderCanvasGroup != null) _promptBorderCanvasGroup.alpha = 1f;
            promptFadeRoutine = null;
        }

        private ClassType FindNearbyClassStation(out EchoStation nearestStation)
        {
            nearestStation = null;
            if (player == null)
            {
                return ClassType.None;
            }

            ClassType nearestClass = ClassType.None;
            float nearestDistance = ClassInteractRadius;
            foreach (EchoStation station in stations.Values)
            {
                if (station?.statue == null)
                {
                    continue;
                }

                float distance = Vector2.Distance(player.position, station.statue.transform.position);
                if (distance <= nearestDistance)
                {
                    nearestDistance = distance;
                    nearestClass = station.classType;
                    nearestStation = station;
                }
            }

            return nearestClass;
        }

        private bool IsNearClassStation(ClassType cls, float radius, out EchoStation station)
        {
            station = null;
            if (player == null || !stations.TryGetValue(cls, out station) || station?.statue == null)
            {
                return false;
            }

            return Vector2.Distance(player.position, station.statue.transform.position) <= radius;
        }

        private void BuildWorldRoom()
        {
            CleanupStaleChamberRuntimeObjects(true);

            GameObject root = new GameObject("AttunementChamber_Runtime");
            grid = root.AddComponent<Grid>();
            grid.cellLayout = GridLayout.CellLayout.Isometric;
            grid.cellSwizzle = GridLayout.CellSwizzle.XYZ;
            grid.cellSize = new Vector3(0.96f, 0.585f, 1f);

            groundTilemap = CreateTilemap(root.transform, "GroundTilemap", "Floor", 0, true);
            collisionTilemap = CreateTilemap(root.transform, "CollisionTilemap", "Floor", -10, false);

            WalkabilityMap walkability = root.AddComponent<WalkabilityMap>();
            walkability.floorTilemap = groundTilemap;

            builder = new GameObject("IsoRoomBuilder").AddComponent<IsoRoomBuilder>();
            builder.transform.SetParent(root.transform, false);
            Transform cliffContainer = CreateContainer(builder.transform, "CliffSprites");
            Transform markerContainer = CreateContainer(builder.transform, "RoomMarkers");
            Transform propsContainer = CreateContainer(builder.transform, "Props");
            Transform gatesContainer = CreateContainer(builder.transform, "Gates");

            roomTemplate = LoadAsset<RoomTemplateSO>(RoomAssetPath, RoomResourcePath);
            PropRegistrySO registry = LoadAsset<PropRegistrySO>(RegistryAssetPath, RegistryResourcePath);
            TileBase floorTile = Resources.Load<TileBase>(FloorTileResource);
            TileBase collisionTile = Resources.Load<TileBase>(CollisionTileResource);
            TileBase overlayTile = Resources.Load<TileBase>(OverlayTileResource);

            SetField(builder, "grid", grid);
            SetField(builder, "groundTilemap", groundTilemap);
            SetField(builder, "collisionTilemap", collisionTilemap);
            SetField(builder, "cliffContainer", cliffContainer);
            SetField(builder, "markerContainer", markerContainer);
            SetField(builder, "propsContainer", propsContainer);
            SetField(builder, "gatesContainer", gatesContainer);
            SetField(builder, "floorTile", floorTile);
            SetField(builder, "collisionTile", collisionTile);
            SetField(builder, "overlayTiles", new[] { overlayTile });
            SetField(builder, "propRegistry", registry);
            SetField(builder, "cliffSouth", LoadAsset<Sprite>("Assets/Resources/Environment/Cliff/cliff_S.png", "Environment/Cliff/cliff_S"));
            SetField(builder, "cliffSouthEast", LoadAsset<Sprite>("Assets/Resources/Environment/Cliff/cliff_SE.png", "Environment/Cliff/cliff_SE"));
            SetField(builder, "cliffSouthWest", LoadAsset<Sprite>("Assets/Resources/Environment/Cliff/cliff_SW.png", "Environment/Cliff/cliff_SW"));
            SetField(builder, "gateNorthSprite", LoadAsset<Sprite>("Assets/Resources/Environment/Gate/gate_arch.png", "Environment/Gate/gate_arch"));

            if (roomTemplate == null || floorTile == null || collisionTile == null || registry == null)
            {
                Debug.LogError("[ChamberSelectBootstrap] BLOCKED: missing generated chamber assets. Run RIMA/Character Select/Generate Attunement Chamber Assets.");
                enabled = false;
                return;
            }

            builder.Build(roomTemplate);
            RemoveGeneratedChamberGateArtifact(root.transform);
            exitWorld = grid.GetCellCenterWorld(new Vector3Int(ExitCell.x, ExitCell.y, 0));
            GenerateChamberLayout();

            int floorCount = builder.LastFloorCells != null ? builder.LastFloorCells.Count : 0;
            Debug.Log($"[ChamberSelectBootstrap] P1/P2 evidence: room={roomTemplate.roomId}, floor={floorCount}, props={roomTemplate.props.Count}, spawn={chamberSpawnCell}, exit={ExitCell}, stations={stationLayouts.Count}.");
        }

        private static void RemoveGeneratedChamberGateArtifact(Transform chamberRoot)
        {
            Transform props = chamberRoot != null ? chamberRoot.Find("IsoRoomBuilder/Props") : null;
            if (props == null)
            {
                return;
            }

            string legacySuffix = $"_{LegacyExitArtifactCell.x}_{LegacyExitArtifactCell.y}";
            for (int i = props.childCount - 1; i >= 0; i--)
            {
                Transform child = props.GetChild(i);
                if (child != null && child.name.EndsWith(legacySuffix, StringComparison.Ordinal))
                {
                    child.gameObject.SetActive(false);
                    if (Application.isPlaying)
                    {
                        Destroy(child.gameObject);
                    }
                    else
                    {
                        DestroyImmediate(child.gameObject);
                    }
                }
            }
        }

        private void GenerateChamberLayout()
        {
            stationLayouts.Clear();

            if (!TryGetFloorBounds(out int minX, out int maxX, out int minY, out int maxY))
            {
                chamberSpawnCell = new Vector2Int(ExitCell.x, 6);
                chamberDummyCell = new Vector2Int(ExitCell.x, 11);
                BuildFallbackStationLayout();
                return;
            }

            int minU = minX - maxY;
            int maxU = maxX - minY;
            int minV = minX + minY;
            int maxV = maxX + maxY;
            int axisU = Mathf.RoundToInt((minU + maxU) * 0.5f);
            int projectedWidth = Mathf.Max(12, maxU - minU + 1);
            int projectedHeight = Mathf.Max(12, maxV - minV + 1);
            int horizontalRadius = Mathf.Clamp(
                Mathf.RoundToInt(projectedWidth * 0.30f),
                12,
                Mathf.Max(12, projectedWidth / 2 - 4));
            int verticalRadius = Mathf.Clamp(
                Mathf.RoundToInt(horizontalRadius * 0.50f),
                5,
                Mathf.Max(5, projectedHeight / 4));
            int arcCenterV = Mathf.Clamp(maxV - 18, minV + 20, maxV - 10);

            Vector2Int ProjectedToCell(int u, int v)
            {
                int x = Mathf.RoundToInt((v + u) * 0.5f);
                int y = Mathf.RoundToInt((v - u) * 0.5f);
                return new Vector2Int(
                    Mathf.Clamp(x, minX + 2, maxX - 2),
                    Mathf.Clamp(y, minY + 2, maxY - 2));
            }

            HashSet<Vector2Int> reserved = new HashSet<Vector2Int> { ExitCell };
            const float startDeg = 200f;
            const float endDeg = 340f;
            for (int i = 0; i < ChamberClasses.Length; i++)
            {
                float t = ChamberClasses.Length <= 1 ? 0f : i / (float)(ChamberClasses.Length - 1);
                float angle = Mathf.Lerp(startDeg, endDeg, t) * Mathf.Deg2Rad;
                int targetU = axisU + Mathf.RoundToInt(horizontalRadius * Mathf.Cos(angle));
                int targetV = arcCenterV + Mathf.RoundToInt(verticalRadius * Mathf.Sin(angle));
                Vector2Int target = ProjectedToCell(targetU, targetV);
                Vector2Int cell = PickNearestWalkableCell(target, reserved, 6);
                stationLayouts.Add(new EchoStationLayout(cell));
                reserved.Add(cell);
            }

            // FIX E: use Inspector override if set (non-zero), else auto-place.
            if (chamberDummyCellOverride.x != 0 || chamberDummyCellOverride.y != 0)
            {
                chamberDummyCell = chamberDummyCellOverride;
            }
            else
            {
                int dummyV = arcCenterV - Mathf.RoundToInt(verticalRadius * 0.55f);
                chamberDummyCell = PickNearestWalkableCell(ProjectedToCell(axisU, dummyV), reserved, 8);
            }

            reserved.Add(chamberDummyCell);
            int spawnV = chamberDummyCell.x + chamberDummyCell.y - 5;
            chamberSpawnCell = PickNearestWalkableCell(ProjectedToCell(axisU, spawnV), reserved, 10);
        }

        private void BuildFallbackStationLayout()
        {
            stationLayouts.Clear();
            int centerX = ExitCell.x;
            int arcBackY = 16;
            int horizontalRadius = 9;
            int verticalRadius = 5;
            const float startDeg = 200f;
            const float endDeg = 340f;
            HashSet<Vector2Int> reserved = new HashSet<Vector2Int>();
            for (int i = 0; i < ChamberClasses.Length; i++)
            {
                float t = ChamberClasses.Length <= 1 ? 0f : i / (float)(ChamberClasses.Length - 1);
                float angle = Mathf.Lerp(startDeg, endDeg, t) * Mathf.Deg2Rad;
                Vector2Int cell = new Vector2Int(
                    centerX + Mathf.RoundToInt(horizontalRadius * Mathf.Cos(angle)),
                    arcBackY + Mathf.RoundToInt(verticalRadius * Mathf.Sin(angle)));
                while (reserved.Contains(cell))
                {
                    cell.x++;
                }

                reserved.Add(cell);
                stationLayouts.Add(new EchoStationLayout(cell));
            }
        }

        private bool TryGetFloorBounds(out int minX, out int maxX, out int minY, out int maxY)
        {
            minX = minY = int.MaxValue;
            maxX = maxY = int.MinValue;

            if (builder?.LastFloorCells == null || builder.LastFloorCells.Count == 0)
            {
                return false;
            }

            foreach (Vector3Int cell in builder.LastFloorCells)
            {
                minX = Mathf.Min(minX, cell.x);
                maxX = Mathf.Max(maxX, cell.x);
                minY = Mathf.Min(minY, cell.y);
                maxY = Mathf.Max(maxY, cell.y);
            }

            return minX <= maxX && minY <= maxY;
        }

        private bool IsWalkableCell(Vector2Int cell)
        {
            return builder?.LastFloorCells != null && builder.LastFloorCells.Contains(new Vector3Int(cell.x, cell.y, 0));
        }

        private Vector2Int PickNearestWalkableCell(Vector2Int target, HashSet<Vector2Int> reserved, int searchRadius)
        {
            Vector2Int best = target;
            int bestScore = int.MaxValue;

            for (int radius = 0; radius <= searchRadius; radius++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    for (int dy = -radius; dy <= radius; dy++)
                    {
                        if (Mathf.Abs(dx) != radius && Mathf.Abs(dy) != radius)
                        {
                            continue;
                        }

                        Vector2Int candidate = new Vector2Int(target.x + dx, target.y + dy);
                        if (!IsWalkableCell(candidate) || reserved.Contains(candidate))
                        {
                            continue;
                        }

                        int score = dx * dx + dy * dy;
                        if (score < bestScore)
                        {
                            best = candidate;
                            bestScore = score;
                        }
                    }
                }

                if (bestScore < int.MaxValue)
                {
                    return best;
                }
            }

            return target;
        }

        /// <summary>
        /// FIX 3a: Snaps X within the same Y row for equal vertical spacing.
        /// Tries ideal cell first, then scans ±searchRadius on X only at the same Y.
        /// Falls back to PickNearestWalkableCell if no same-row cell found.
        /// </summary>
        private Vector2Int PickNearestWalkableSameRowY(Vector2Int target, HashSet<Vector2Int> reserved, int searchRadius)
        {
            // Try ideal cell first
            if (IsWalkableCell(target) && !reserved.Contains(target))
            {
                return target;
            }

            // Scan X at same Y
            for (int r = 1; r <= searchRadius; r++)
            {
                Vector2Int candidateLeft = new Vector2Int(target.x - r, target.y);
                if (IsWalkableCell(candidateLeft) && !reserved.Contains(candidateLeft))
                {
                    return candidateLeft;
                }

                Vector2Int candidateRight = new Vector2Int(target.x + r, target.y);
                if (IsWalkableCell(candidateRight) && !reserved.Contains(candidateRight))
                {
                    return candidateRight;
                }
            }

            // Fallback: allow Y drift
            return PickNearestWalkableCell(target, reserved, searchRadius);
        }

        /// <summary>
        /// Variant of PickNearestWalkableCell that additionally rejects:
        /// - Any candidate within Chebyshev distance <see cref="StationClearance"/> of any station cell.
        /// - Any candidate on the central aisle column (axisX-1, axisX, axisX+1).
        /// Falls back to PickNearestWalkableCell without station check if nothing valid is found.
        /// </summary>
        private Vector2Int PickNearestWalkableCellClearOfStations(
            Vector2Int target, HashSet<Vector2Int> reserved,
            List<Vector2Int> stationCells, int axisX, int searchRadius)
        {
            const int StationClearance = 3; // Chebyshev min distance from any station cell

            Vector2Int best = target;
            int bestScore = int.MaxValue;

            for (int radius = 0; radius <= searchRadius; radius++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    for (int dy = -radius; dy <= radius; dy++)
                    {
                        if (Mathf.Abs(dx) != radius && Mathf.Abs(dy) != radius)
                        {
                            continue;
                        }

                        Vector2Int candidate = new Vector2Int(target.x + dx, target.y + dy);
                        if (!IsWalkableCell(candidate) || reserved.Contains(candidate))
                        {
                            continue;
                        }

                        // Reject central aisle
                        if (Mathf.Abs(candidate.x - axisX) <= 1)
                        {
                            continue;
                        }

                        // Reject if too close to any figure station
                        bool tooClose = false;
                        foreach (Vector2Int sc in stationCells)
                        {
                            int chebDist = Mathf.Max(Mathf.Abs(candidate.x - sc.x), Mathf.Abs(candidate.y - sc.y));
                            if (chebDist < StationClearance)
                            {
                                tooClose = true;
                                break;
                            }
                        }
                        if (tooClose)
                        {
                            continue;
                        }

                        int score = dx * dx + dy * dy;
                        if (score < bestScore)
                        {
                            best = candidate;
                            bestScore = score;
                        }
                    }
                }

                if (bestScore < int.MaxValue)
                {
                    return best;
                }
            }

            // Fallback: drop station-clearance requirement if no valid cell found in radius
            Debug.LogWarning("[ChamberSelectBootstrap] Dummy placement: no station-clear cell found within radius, falling back to basic walkable search.");
            return PickNearestWalkableCell(target, reserved, searchRadius);
        }

        public static void CleanupLeakedCombatChamberObjects()
        {
            CleanupStaleChamberRuntimeObjects(false);
        }

        private static void CleanupStaleChamberRuntimeObjects(bool includePlayerRoots)
        {
            string[] exactRootNames = includePlayerRoots
                ? new[]
                {
                    "AttunementChamber_Runtime",
                    "IsoRoomBuilder",
                    "EchoStations",
                    "TrainingDummy_RealDamageable",
                    "Dummy",
                    "TrainingDummy_HP",
                    "ArrivalRing",
                    "ChamberPrompt",
                    "Player"
                }
                : new[]
                {
                    "AttunementChamber_Runtime",
                    "EchoStations",
                    "TrainingDummy_RealDamageable",
                    "Dummy",
                    "TrainingDummy_HP",
                    "ArrivalRing",
                    "ChamberPrompt",
                    "ChamberExitPortal",
                    "DoorLight"
                };

            GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();
            for (int i = objects.Length - 1; i >= 0; i--)
            {
                GameObject go = objects[i];
                if (go == null || !go.scene.IsValid() || go.transform.parent != null)
                {
                    continue;
                }

                if (MatchesChamberCleanupName(go.name, exactRootNames))
                {
                    DestroyRuntimeObject(go);
                }
            }
        }

        private static bool MatchesChamberCleanupName(string objectName, string[] exactRootNames)
        {
            for (int i = 0; i < exactRootNames.Length; i++)
            {
                if (objectName == exactRootNames[i])
                {
                    return true;
                }
            }

            return objectName.StartsWith("EchoStatue_", StringComparison.Ordinal)
                || objectName.StartsWith("EchoPedestal_", StringComparison.Ordinal);
        }

        private static void DestroyRuntimeObject(GameObject target)
        {
            if (target == null)
            {
                return;
            }

            if (Application.isPlaying)
            {
                Destroy(target);
            }
            else
            {
                DestroyImmediate(target);
            }
        }

        private static Tilemap CreateTilemap(Transform parent, string name, string sortingLayer, int order, bool visible)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            Tilemap tilemap = go.AddComponent<Tilemap>();
            TilemapRenderer renderer = go.AddComponent<TilemapRenderer>();
            renderer.sortingLayerName = sortingLayer;
            renderer.sortingOrder = order;
            renderer.enabled = visible;
            return tilemap;
        }

        private static Transform CreateContainer(Transform parent, string name)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            return go.transform;
        }

        private void SpawnPlayer()
        {
            currentClass = PlayerClassManager.SelectedClass != ClassType.None &&
                           IsDemoSelectable(PlayerClassManager.SelectedClass) &&
                           IsUnlocked(PlayerClassManager.SelectedClass)
                ? PlayerClassManager.SelectedClass
                : ClassType.Warblade;
            PlayerClassManager.SelectedClass = currentClass;

            Vector3 spawn = grid.GetCellCenterWorld(new Vector3Int(chamberSpawnCell.x, chamberSpawnCell.y, 0));

            GameObject prefab = Resources.Load<GameObject>("Prefabs/Warblade") ?? Resources.Load<GameObject>("Prefabs/Player");
            GameObject instance = prefab != null
                ? Instantiate(prefab, spawn, Quaternion.identity)
                : new GameObject("Player");

            instance.name = "Player";
            instance.transform.position = spawn;
            player = instance.transform;

            GameObject ringGo = new GameObject("ArrivalRing");
            ringGo.transform.position = spawn + new Vector3(0, -0.1f, 0);
            SpriteRenderer ringSr = ringGo.AddComponent<SpriteRenderer>();
            ringSr.sprite = Resources.Load<Sprite>("Props/arrival_ring_0") ?? Resources.Load<Sprite>("Props/arrival_ring");
            ringSr.sortingLayerName = "Floor";
            ringSr.sortingOrder = 10;
            StartCoroutine(ArrivalRingRoutine(ringGo.transform, ringSr));

            EnsurePlayerRuntime(instance);
            // FIX 1: Assign profile BEFORE SetPrimaryClass so PlayerAttack.Awake can init correctly.
            // SetPrimaryClass calls ApplyBasicAttackProfile which calls SetBasicAttackProfile(profile)
            // and that re-enables the component even if Awake disabled it.
            PlayerClassManager.SelectedClass = currentClass;
            EnsureClassManager().SetPrimaryClass(currentClass);
            // Assign profile directly in case SetPrimaryClass ran before the player had its tag set
            // (FindGameObjectWithTag won't find it if the player object is not yet tagged).
            AssignAttackProfileToPlayer(instance, currentClass);
            // FIX 6: Assign SlashArcVFX so hit VFX fires.
            AssignSlashArcVFXToPlayer(instance);
            ApplyChamberPlayerVisual(instance, currentClass);
            // Chamber practice: grant the class demo kit + top up resources so Q/E/R/F can be
            // tried on the dummy before committing at the rift. Chamber-only; run-start loadout
            // (DraftManager) is untouched.
            GrantPracticeLoadout(currentClass);
            // Show the practice skill bar (Q/E/R/F readout) so the player can see what each key does.
            ShowChamberSkillBar();
            // Off-map safety: dash/charge skills (e.g. Iron Charge, Blink) can fling the player off the
            // small diamond platform. Attach a chamber-only guard that snaps them back to spawn.
            AttachChamberBoundsGuard(instance, spawn);
            Debug.Log($"[ChamberSelectBootstrap] P3 evidence: player spawned as {currentClass} at {spawn}.");
        }

        /// <summary>
        /// Chamber-only: host the standard gameplay <see cref="SkillBarUI"/> on the chamber overlay
        /// canvas so the granted practice Q/E/R/F (icon + key + name) are visible. The bar auto-resolves
        /// the active player + class and populates itself, exactly like the in-run HUD. The whole canvas
        /// (and this bar) is torn down with the chamber scene / cleanup, so it is hidden on leaving.
        /// </summary>
        private void ShowChamberSkillBar()
        {
            if (chamberSkillBar != null) return;

            RectTransform canvasRoot = EnsureChamberOverlayCanvas();
            GameObject barGo = new GameObject("ChamberSkillBar", typeof(RectTransform));
            barGo.transform.SetParent(canvasRoot, false);
            RectTransform rt = barGo.GetComponent<RectTransform>();
            // Bottom-center, raised ABOVE the [G] prompt panel (prompt sits at anchor y=0.06).
            rt.anchorMin = new Vector2(0.5f, 0f);
            rt.anchorMax = new Vector2(0.5f, 0f);
            rt.pivot = new Vector2(0.5f, 0f);
            rt.anchoredPosition = new Vector2(0f, 96f);
            rt.sizeDelta = new Vector2(420f, 72f);
            chamberSkillBar = barGo.AddComponent<SkillBarUI>();
            Debug.Log("[ChamberSelectBootstrap] Practice skill bar shown on chamber overlay (Q/E/R/F readout).");
        }

        /// <summary>
        /// Chamber-only: attach a snap-back guard to the practice player so movement/dash skills cannot
        /// strand it off the small platform. Walkable bounds are derived from the built floor cells; if
        /// the player leaves them (or drops below a Y floor), it is teleported back to the spawn point.
        /// This component is ONLY ever added here, so real gameplay rooms are unaffected.
        /// </summary>
        private void AttachChamberBoundsGuard(GameObject playerObject, Vector3 spawnWorld)
        {
            if (playerObject == null) return;

            // World-space center + radius of the walkable diamond, with a small margin.
            Vector3 center = spawnWorld;
            float radius = 6f;
            float yFloor = spawnWorld.y - 8f;
            if (builder?.LastFloorCells != null && builder.LastFloorCells.Count > 0 && grid != null)
            {
                Vector3 sum = Vector3.zero;
                float maxSqr = 0f;
                int count = 0;
                Vector3 minW = new Vector3(float.MaxValue, float.MaxValue, 0f);
                foreach (Vector3Int cell in builder.LastFloorCells)
                {
                    Vector3 w = grid.GetCellCenterWorld(cell);
                    sum += w;
                    count++;
                    if (w.y < minW.y) minW.y = w.y;
                }
                if (count > 0)
                {
                    center = sum / count;
                    foreach (Vector3Int cell in builder.LastFloorCells)
                    {
                        Vector3 w = grid.GetCellCenterWorld(cell);
                        float sqr = (w - center).sqrMagnitude;
                        if (sqr > maxSqr) maxSqr = sqr;
                    }
                    radius = Mathf.Sqrt(maxSqr) + 1.0f;   // margin so the edge tile stays valid
                    yFloor = minW.y - 1.5f;               // anything below the lowest tile is off-map
                }
            }

            ChamberBoundsGuard guard = playerObject.AddComponent<ChamberBoundsGuard>();
            guard.Initialize(spawnWorld, center, radius, yFloor);
            Debug.Log($"[ChamberSelectBootstrap] Chamber bounds guard attached: center={center}, radius={radius:F2}, yFloor={yFloor:F2}, spawn={spawnWorld}.");
        }

        private IEnumerator ArrivalRingRoutine(Transform ring, SpriteRenderer sr)
        {
            if (sr.sprite == null) yield break;
            float duration = 0.8f;
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                if (ring == null) yield break;
                float k = t / duration;
                ring.Rotate(0, 0, 180f * Time.deltaTime);
                sr.color = new Color(0f, 1f, 1f, 1f - k);
                ring.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.5f, k);
                yield return null;
            }
            if (ring != null) Destroy(ring.gameObject);
        }

        private static void EnsurePlayerRuntime(GameObject playerObject)
        {
            playerObject.tag = "Player";
            int layer = LayerMask.NameToLayer("Player");
            if (layer >= 0) playerObject.layer = layer;

            Rigidbody2D body = AddIfMissing<Rigidbody2D>(playerObject);
            body.gravityScale = 0f;
            body.freezeRotation = true;
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            if (playerObject.GetComponent<Collider2D>() == null)
            {
                CapsuleCollider2D col = playerObject.AddComponent<CapsuleCollider2D>();
                col.offset = new Vector2(0f, 0.15f);
                col.size = new Vector2(0.46f, 0.34f);
            }

            AddIfMissing<Health>(playerObject);
            AddIfMissing<InputBufferService>(playerObject);
            AddIfMissing<SkillFlowTracker>(playerObject);
            AddIfMissing<PlayerController>(playerObject);
            AddIfMissing<RageSystem>(playerObject);
            AddIfMissing<PlayerAttack>(playerObject);
            AddIfMissing<ExecutePromptDriver>(playerObject);
        }

        private static PlayerClassManager EnsureClassManager()
        {
            if (PlayerClassManager.Instance != null)
            {
                return PlayerClassManager.Instance;
            }

            GameObject go = new GameObject("PlayerClassManager");
            DontDestroyOnLoad(go);
            return go.AddComponent<PlayerClassManager>();
        }

        private void SpawnEchoStations()
        {
            stations.Clear();
            Transform root = CreateContainer(GameObject.Find("AttunementChamber_Runtime").transform, "EchoStations");
            if (stationLayouts.Count < ChamberClasses.Length)
            {
                BuildFallbackStationLayout();
            }

            for (int i = 0; i < ChamberClasses.Length; i++)
            {
                ClassType cls = ChamberClasses[i];
                EchoStationLayout layout = stationLayouts[i];
                Vector2Int cell = layout.cell;
                Vector3 baseWorld = grid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0));
                SpriteRenderer pedestal = CreateEchoPedestal(root, cls, baseWorld, out Light2D pedestalLight);

                GameObject statue = new GameObject($"EchoStatue_{cls}");
                statue.transform.SetParent(root, false);
                // FIX A: lower figure so feet sit ON the oval (oval center at baseWorld.y+0.18).
                // With center-ish pivot, placing at +0.22 puts feet at ~floor level visually.
                statue.transform.position = baseWorld + new Vector3(0.47f, 0.22f, 0f);
                SpriteRenderer sr = statue.AddComponent<SpriteRenderer>();
                sr.sprite = LoadClassIdleSouthSprite(cls, out bool usedFallback);
                sr.sortingLayerName = "Characters";
                sr.sortingOrder = SortOrder(statue.transform.position);
                statue.transform.localScale = PlayerFigureScale();
                if (usedFallback)
                {
                    Debug.LogWarning($"[ChamberSelectBootstrap] Missing idle_south sprite for {cls}; using readable class sprite fallback.");
                }

                stations[cls] = new EchoStation
                {
                    classType = cls,
                    cell = cell,
                    statue = sr,
                    pedestal = pedestal,
                    pedestalLight = pedestalLight,
                };
            }

            Debug.Log("[ChamberSelectBootstrap] Class figures spawned as a 10-class crescent around the chamber centerline.");
        }

        private SpriteRenderer CreateEchoPedestal(Transform root, ClassType cls, Vector3 baseWorld, out Light2D pedestalLight)
        {
            GameObject pedestalGo = new GameObject($"EchoPedestal_{cls}");
            pedestalGo.transform.SetParent(root, false);
            // FIX A: oval foot-ring raised to match lowered figure so feet visually sit ON it.
            pedestalGo.transform.position = baseWorld + new Vector3(0.47f, 0.10f, 0f);
            pedestalGo.transform.localScale = new Vector3(1.28f, 0.42f, 1f);

            SpriteRenderer pedestal = pedestalGo.AddComponent<SpriteRenderer>();
            pedestal.sprite = EchoPedestalSprite();
            pedestal.sortingLayerName = "Floor";
            pedestal.sortingOrder = 14;
            pedestal.color = new Color(0.16f, 0.42f, 0.48f, 0.72f);

            GameObject lightGo = new GameObject($"EchoPedestalLight_{cls}");
            lightGo.transform.SetParent(pedestalGo.transform, false);
            lightGo.transform.localPosition = new Vector3(0f, 0.24f, 0f);
            pedestalLight = lightGo.AddComponent<Light2D>();
            pedestalLight.lightType = Light2D.LightType.Point;
            pedestalLight.intensity = 0.70f;
            pedestalLight.pointLightOuterRadius = 3.0f;
            pedestalLight.color = new Color(0.18f, 0.88f, 1f, 1f);

            return pedestal;
        }

        private Vector3 PlayerFigureScale()
        {
            return player != null ? player.localScale : Vector3.one;
        }

        private void SpawnTrainingDummy()
        {
            Vector3 pos = grid.GetCellCenterWorld(new Vector3Int(chamberDummyCell.x, chamberDummyCell.y, 0));
            GameObject dummy = new GameObject("Dummy");
            // FIX A: ground the dummy — small downward offset so sprite base sits on floor.
            dummy.transform.position = pos + new Vector3(0f, 0.08f, 0f);
            dummy.tag = "Untagged";
            int enemyLayer = LayerMask.NameToLayer("Enemy");
            if (enemyLayer >= 0) dummy.layer = enemyLayer;
            dummyTransform = dummy.transform;

            SpriteRenderer sr = dummy.AddComponent<SpriteRenderer>();
            sr.sprite = LoadClassIdleSouthSprite(currentClass, out bool usedFallback);
            if (usedFallback)
            {
                Debug.LogWarning($"[ChamberSelectBootstrap] Missing {currentClass} idle_south for dummy; using readable class sprite fallback.");
            }
            sr.color = Color.white;
            sr.sortingLayerName = "Characters";
            sr.sortingOrder = SortOrder(dummy.transform.position);
            dummyRenderer = sr;
            // FIX E: dummy is noticeably bigger (chamberDummyScale, default 1.5x figure).
            float dummyS = chamberDummyScale;
            dummy.transform.localScale = new Vector3(dummyS, dummyS, 1f);

            Rigidbody2D body = dummy.AddComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Kinematic;
            body.gravityScale = 0f;
            body.freezeRotation = true;
            body.constraints = RigidbodyConstraints2D.FreezeAll;
            BoxCollider2D collider = dummy.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(0.7f, 0.7f);
            collider.isTrigger = true;
            Health health = dummy.AddComponent<Health>();
            health.SetMaxHP(1000);
            dummy.AddComponent<RIMA.Combat.HitFlashDriver>();

            GameObject lightGo = new GameObject("TrainingDummy_Light");
            lightGo.transform.SetParent(dummy.transform, false);
            lightGo.transform.localPosition = new Vector3(0f, 0.35f, 0f);
            Light2D dummyLight = lightGo.AddComponent<Light2D>();
            dummyLight.lightType = Light2D.LightType.Point;
            dummyLight.intensity = 0.8f;
            dummyLight.pointLightOuterRadius = 3.0f;
            dummyLight.color = new Color(0.50f, 0.76f, 1f, 1f);

            // FIX 1: Derive bar/label world-Y from sprite bounds so they sit above the VISIBLE sprite top.
            //        sprite.bounds.max.y covers the full FullRect including transparent padding.
            //        We apply a visible-ratio correction (0.78) to approximate the opaque pixel top.
            //        This is the fallback path — texture.isReadable is not guaranteed at runtime for
            //        imported sprites, so per-pixel scanning is unreliable. 0.78 calibrated empirically
            //        to place the bar flush with the character head.
            const float visibleRatioCorrection = 0.78f;
            float spriteTopWorld;
            if (sr.sprite != null)
            {
                spriteTopWorld = dummy.transform.position.y + sr.sprite.bounds.max.y * dummyS * visibleRatioCorrection;
            }
            else
            {
                // Fallback: use the old heuristic if sprite is missing.
                spriteTopWorld = dummy.transform.position.y + 0.95f + (chamberDummyScale - 0.72f) * 0.4f - 0.30f;
            }
            const float barMargin    = 0.05f;   // compact gap between visible sprite top and bar bottom
            const float barHeight    = 0.13f;   // matches CreateDummyHpBar Back sprite scale.y
            const float labelGap     = 0.03f;   // tight gap between bar top and name label bottom
            float barCenterY  = spriteTopWorld + barMargin + barHeight * 0.5f;
            float labelCenterY = barCenterY + barHeight * 0.5f + labelGap + 0.09f; // +0.09 = half text height at 1.6

            // FIX 2: Name label says "Dummy"; show/hide on hover (default hidden).
            TMP_Text hpLabel = CreateWorldText("TrainingDummy_HP", dummy.transform, new Vector3(dummy.transform.position.x, labelCenterY, dummy.transform.position.z), 1.6f);
            hpLabel.text = "Dummy";
            hpLabel.gameObject.SetActive(false);   // hidden until hover
            ScreenshotMode.Register(hpLabel.gameObject, "TrainingDummy_HP");
            // FIX 2b: HP bar first (no separate number label above it).
            DummyHpBar hpBar = CreateDummyHpBar(dummy.transform, new Vector3(dummy.transform.position.x, barCenterY, dummy.transform.position.z));
            hpBar.RootGo.SetActive(false);   // hidden until hover
            // FIX 2b: HP number INSIDE the bar (centered on the red fill area).
            TMP_Text hpNumberLabel = CreateWorldText("TrainingDummy_HPNumber", hpBar.RootGo.transform,
                hpBar.RootGo.transform.position, 1.8f);
            hpNumberLabel.text = $"{health.CurrentHP} / {health.MaxHP}";
            // FIX 2c: Hover component shows/hides name + bar when mouse is over dummy collider.
            DummyHoverVisibility hoverVis = dummy.AddComponent<DummyHoverVisibility>();
            hoverVis.Initialize(hpLabel.gameObject, hpBar.RootGo);
            dummy.AddComponent<TrainingDummyTarget>().Initialize(health, hpLabel, hpBar, hpNumberLabel);
            Debug.Log($"[ChamberSelectBootstrap] P4 evidence: immortal trigger-only training dummy spawned at chamber cell {chamberDummyCell}.");
        }

        private DummyHpBar CreateDummyHpBar(Transform parent, Vector3 position)
        {
            GameObject bar = new GameObject("TrainingDummy_HPBar");
            bar.transform.SetParent(parent, false);
            bar.transform.position = position;

            CreateBarSprite("Back", bar.transform, Vector3.zero, new Vector3(1.15f, 0.13f, 1f), new Color(0.06f, 0.08f, 0.10f, 0.92f), 298);
            SpriteRenderer fill = CreateBarSprite("Fill", bar.transform, new Vector3(-0.55f, 0f, 0f), new Vector3(1.08f, 0.07f, 1f), new Color(0.7f, 0.05f, 0.08f, 0.98f), 299);
            fill.transform.localScale = new Vector3(1.08f, 0.07f, 1f);
            return new DummyHpBar(bar, fill, 1.08f);
        }

        private static SpriteRenderer CreateBarSprite(string name, Transform parent, Vector3 localPosition, Vector3 scale, Color color, int sortingOrder)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPosition;
            go.transform.localScale = scale;

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = WhiteSprite();
            sr.sortingLayerName = "Characters";
            sr.sortingOrder = sortingOrder;
            sr.color = color;
            return sr;
        }

        private void CreatePromptLabel()
        {
            RectTransform canvasRoot = EnsureChamberOverlayCanvas();
            BuildChamberEchoDisplay(canvasRoot);

            // FIX 1: cyan-border frame behind the panel (2-3px visual outline).
            GameObject borderGo = new GameObject("ChamberPromptBorder", typeof(RectTransform));
            borderGo.transform.SetParent(canvasRoot, false);
            RectTransform borderRt = borderGo.GetComponent<RectTransform>();
            borderRt.anchorMin = new Vector2(0.5f, 0.06f);
            borderRt.anchorMax = new Vector2(0.5f, 0.06f);
            borderRt.pivot = new Vector2(0.5f, 0.5f);
            borderRt.anchoredPosition = Vector2.zero;
            borderRt.sizeDelta = new Vector2(692f, 68f);   // 4px larger than panel on each side
            Image borderImg = borderGo.AddComponent<Image>();
            borderImg.color = new Color(0.2f, 0.85f, 1f, 0.9f);
            borderImg.raycastTarget = false;

            GameObject promptGo = new GameObject("ChamberPromptPanel", typeof(RectTransform));
            promptGo.transform.SetParent(canvasRoot, false);
            RectTransform promptRt = promptGo.GetComponent<RectTransform>();
            promptRt.anchorMin = new Vector2(0.5f, 0.06f);
            promptRt.anchorMax = new Vector2(0.5f, 0.06f);
            promptRt.pivot = new Vector2(0.5f, 0.5f);
            promptRt.anchoredPosition = Vector2.zero;
            promptRt.sizeDelta = new Vector2(684f, 60f);

            Image background = promptGo.AddComponent<Image>();
            background.color = new Color(0.06f, 0.09f, 0.12f, 0.92f);
            background.raycastTarget = false;

            promptCanvasGroup = promptGo.AddComponent<CanvasGroup>();
            promptCanvasGroup.alpha = 0f;

            // Border is already a sibling of promptGo (both children of canvasRoot).
            // It was created first so it renders behind the panel. Give it its own CanvasGroup for alpha sync.
            CanvasGroup borderCg = borderGo.AddComponent<CanvasGroup>();
            borderCg.alpha = 0f;

            promptKeycap = new GameObject("Keycap_G", typeof(RectTransform));
            promptKeycap.transform.SetParent(promptGo.transform, false);
            RectTransform keyRt = promptKeycap.GetComponent<RectTransform>();
            keyRt.anchorMin = new Vector2(0f, 0.5f);
            keyRt.anchorMax = new Vector2(0f, 0.5f);
            keyRt.pivot = new Vector2(0.5f, 0.5f);
            keyRt.anchoredPosition = new Vector2(36f, 0f);
            keyRt.sizeDelta = new Vector2(34f, 34f);

            Image keyBg = promptKeycap.AddComponent<Image>();
            keyBg.color = new Color(0.25f, 0.55f, 0.65f, 0.98f);
            keyBg.raycastTarget = false;

            GameObject keyTextGo = new GameObject("Glyph", typeof(RectTransform));
            keyTextGo.transform.SetParent(promptKeycap.transform, false);
            RectTransform keyTextRt = keyTextGo.GetComponent<RectTransform>();
            keyTextRt.anchorMin = Vector2.zero;
            keyTextRt.anchorMax = Vector2.one;
            keyTextRt.offsetMin = Vector2.zero;
            keyTextRt.offsetMax = Vector2.zero;
            TextMeshProUGUI keyText = keyTextGo.AddComponent<TextMeshProUGUI>();
            keyText.text = "G";
            keyText.fontSize = 20f;
            keyText.fontStyle = FontStyles.Bold;
            keyText.color = Color.white;
            keyText.alignment = TextAlignmentOptions.Center;
            keyText.raycastTarget = false;

            GameObject textGo = new GameObject("Text", typeof(RectTransform));
            textGo.transform.SetParent(promptGo.transform, false);
            promptTextRect = textGo.GetComponent<RectTransform>();
            promptTextRect.anchorMin = Vector2.zero;
            promptTextRect.anchorMax = Vector2.one;
            promptTextRect.offsetMin = new Vector2(58f, 0f);
            promptTextRect.offsetMax = new Vector2(-10f, 0f);

            promptLabel = textGo.AddComponent<TextMeshProUGUI>();
            promptLabel.color = new Color(0.85f, 0.97f, 1f, 1f);
            promptLabel.fontSize = 24f;
            promptLabel.fontStyle = FontStyles.Bold;
            promptLabel.alignment = TextAlignmentOptions.MidlineLeft;
            promptLabel.enableWordWrapping = false;
            promptLabel.raycastTarget = false;
            promptGo.SetActive(false);
            borderGo.SetActive(false);
            // Store border reference for sync with promptCanvasGroup.
            _promptBorderCanvasGroup = borderCg;
            _promptBorderGo = borderGo;
        }

        private RectTransform EnsureChamberOverlayCanvas()
        {
            if (chamberOverlayCanvas != null)
            {
                return (RectTransform)chamberOverlayCanvas.transform;
            }

            // FIX RENDERING: Parent to scene root (NOT to this transform/CharacterSelectCanvas whose
            // CanvasGroup.alpha=0 hid all children). ScreenSpaceOverlay so the HUD + prompt ALWAYS
            // draw ON TOP of every world sprite (floor, cliff, figures) — overlay has no per-sprite
            // sorting conflict (ScreenSpaceCamera let tall cliff sprites occlude the HUD).
            GameObject canvasGo = new GameObject("ChamberOverlayCanvas", typeof(RectTransform));
            canvasGo.transform.SetParent(null, false);
            chamberOverlayCanvas = canvasGo.AddComponent<Canvas>();
            chamberOverlayCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            chamberOverlayCanvas.sortingOrder = 500;
            CanvasScaler scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;
            canvasGo.AddComponent<GraphicRaycaster>();
            return (RectTransform)canvasGo.transform;
        }

        private void BuildChamberEchoDisplay(RectTransform root)
        {
            // FIX 4: Proper top-left HUD. Dark background panel for readability.
            GameObject bgGo = new GameObject("ChamberEchoDisplayBg", typeof(RectTransform));
            bgGo.transform.SetParent(root, false);
            RectTransform bgRt = bgGo.GetComponent<RectTransform>();
            bgRt.anchorMin = new Vector2(0f, 1f);
            bgRt.anchorMax = new Vector2(0f, 1f);
            bgRt.pivot = new Vector2(0f, 1f);
            bgRt.anchoredPosition = new Vector2(8f, -8f);
            bgRt.sizeDelta = new Vector2(160f, 32f);
            Image bgImg = bgGo.AddComponent<Image>();
            bgImg.color = new Color(0.04f, 0.07f, 0.10f, 0.82f);
            bgImg.raycastTarget = false;

            GameObject groupGo = new GameObject("ChamberEchoDisplay", typeof(RectTransform));
            groupGo.transform.SetParent(bgGo.transform, false);
            RectTransform groupRt = groupGo.GetComponent<RectTransform>();
            groupRt.anchorMin = Vector2.zero;
            groupRt.anchorMax = Vector2.one;
            groupRt.offsetMin = new Vector2(6f, 4f);
            groupRt.offsetMax = new Vector2(-6f, -4f);

            // Cyan diamond icon
            GameObject iconGo = new GameObject("EchoIcon", typeof(RectTransform));
            iconGo.transform.SetParent(groupGo.transform, false);
            RectTransform iconRt = iconGo.GetComponent<RectTransform>();
            iconRt.anchorMin = new Vector2(0f, 0.5f);
            iconRt.anchorMax = new Vector2(0f, 0.5f);
            iconRt.pivot = new Vector2(0.5f, 0.5f);
            iconRt.anchoredPosition = new Vector2(8f, 0f);
            iconRt.sizeDelta = new Vector2(12f, 12f);
            iconRt.localEulerAngles = new Vector3(0f, 0f, 45f);
            Image icon = iconGo.AddComponent<Image>();
            icon.color = new Color(0.28f, 0.96f, 1f, 0.95f);
            icon.raycastTarget = false;

            // Balance label to the right of icon
            GameObject labelGo = new GameObject("EchoBalance", typeof(RectTransform));
            labelGo.transform.SetParent(groupGo.transform, false);
            RectTransform labelRt = labelGo.GetComponent<RectTransform>();
            labelRt.anchorMin = Vector2.zero;
            labelRt.anchorMax = Vector2.one;
            labelRt.offsetMin = new Vector2(20f, 0f);
            labelRt.offsetMax = Vector2.zero;

            chamberEchoBalanceLabel = labelGo.AddComponent<TextMeshProUGUI>();
            chamberEchoBalanceLabel.text = "0";
            chamberEchoBalanceLabel.fontSize = 16f;
            chamberEchoBalanceLabel.fontStyle = FontStyles.Bold;
            chamberEchoBalanceLabel.color = new Color(0.82f, 0.96f, 1f, 0.95f);
            chamberEchoBalanceLabel.alignment = TextAlignmentOptions.MidlineLeft;
            chamberEchoBalanceLabel.raycastTarget = false;
            RefreshChamberEchoHud();
        }

        private void RefreshChamberEchoHud()
        {
            if (chamberEchoBalanceLabel == null)
            {
                return;
            }

            int balance = EchoWallet.Balance;
            if (lastEchoBalance == balance)
            {
                return;
            }

            lastEchoBalance = balance;
            chamberEchoBalanceLabel.text = balance.ToString();
        }

        private static TMP_Text CreateWorldText(string name, Transform parent, Vector3 position, float fontSize)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.position = position;
            TextMeshPro text = go.AddComponent<TextMeshPro>();
            text.fontSize = fontSize;
            text.color = new Color(0.82f, 0.86f, 0.90f, 1f);
            text.alignment = TextAlignmentOptions.Center;
            text.enableWordWrapping = false;
            text.sortingLayerID = SortingLayer.NameToID("UI");
            text.sortingOrder = 200;
            return text;
        }

        private void ConfigureCameraAndLight()
        {
            chamberCamera = Camera.main;
            if (chamberCamera == null)
            {
                GameObject go = new GameObject("Main Camera");
                chamberCamera = go.AddComponent<Camera>();
                go.tag = "MainCamera";
            }

            chamberCamera.orthographic = true;
            Behaviour legacyPpc = FindLegacyPixelPerfectCamera(chamberCamera);
            if (legacyPpc != null && legacyPpc.enabled)
            {
                legacyPpc.enabled = false;
                disabledLegacyPixelPerfectCamera = legacyPpc;
            }

            PixelPerfectCamera urpPpc = chamberCamera.GetComponent<PixelPerfectCamera>();
            if (urpPpc != null && urpPpc.enabled)
            {
                urpPpc.enabled = false;
                disabledUrpPixelPerfectCamera = urpPpc;
            }

            float orthoSize = CalculateChamberCameraOrthoSize();
            chamberCamera.orthographicSize = orthoSize;

            CameraZoom zoom = chamberCamera.GetComponent<CameraZoom>();
            if (zoom != null && zoom.enabled)
            {
                zoom.enabled = false;
                disabledCameraZoom = zoom;
            }

            RIMA.CameraSystem.CameraFollow follow = chamberCamera.GetComponent<RIMA.CameraSystem.CameraFollow>()
                ?? chamberCamera.gameObject.AddComponent<RIMA.CameraSystem.CameraFollow>();
            if (!previousFollowCaptured)
            {
                previousFollowOffset = follow.worldOffset;
                previousFollowTarget = follow.target;
                previousFollowCaptured = true;
            }
            chamberFollow = follow;
            // FIX 2: Follow the player (not a static anchor), player sits at chamberPlayerScreenY
            // down from top. worldOffset shifts the camera so the player appears in the lower third.
            follow.target = player;
            follow.worldOffset = new Vector3(0f, orthoSize * (2f * chamberPlayerScreenY - 1f), -10f);
            chamberCamera.transform.position = new Vector3(player.position.x,
                player.position.y + orthoSize * (2f * chamberPlayerScreenY - 1f), -10f);

            Light2D globalLight = null;
            foreach (Light2D candidate in FindObjectsByType<Light2D>(FindObjectsSortMode.None))
            {
                if (candidate.lightType == Light2D.LightType.Global)
                {
                    globalLight = candidate;
                    break;
                }
            }

            if (globalLight == null)
            {
                GameObject lightGo = new GameObject("Chamber_GlobalLight2D");
                globalLight = lightGo.AddComponent<Light2D>();
                globalLight.lightType = Light2D.LightType.Global;
            }

            globalLight.intensity = chamberGlobalLightIntensity;
            globalLight.color = new Color(0.78f, 0.86f, 1f, 1f);

            Vector3 fillPos = Vector3.Lerp(player.position, exitWorld, 0.5f);
            GameObject fillLightGo = new GameObject("Chamber_AisleFillLight");
            fillLightGo.transform.SetParent(transform, false);
            fillLightGo.transform.position = fillPos;
            Light2D fillLight = fillLightGo.AddComponent<Light2D>();
            fillLight.lightType = Light2D.LightType.Point;
            fillLight.intensity = chamberFillLightIntensity;
            fillLight.pointLightOuterRadius = 10f;
            fillLight.color = new Color(0.50f, 0.68f, 1f, 1f);
        }

        private static Behaviour FindLegacyPixelPerfectCamera(Camera camera)
        {
            if (camera == null)
            {
                return null;
            }

            Behaviour[] behaviours = camera.GetComponents<Behaviour>();
            for (int i = 0; i < behaviours.Length; i++)
            {
                Behaviour behaviour = behaviours[i];
                if (behaviour != null && behaviour.GetType().FullName == "UnityEngine.U2D.PixelPerfectCamera")
                {
                    return behaviour;
                }
            }

            return null;
        }

        private float CalculateChamberCameraOrthoSize()
        {
            Bounds bounds = CalculateChamberBounds();
            float aspect = chamberCamera != null && chamberCamera.aspect > 0.01f
                ? chamberCamera.aspect
                : 16f / 9f;
            float baseSize = Mathf.Max(chamberCameraOrthoSize, chamberCameraMinimumOrthographicSize);

            if (player == null)
            {
                return baseSize;
            }

            float offsetFactor = 2f * chamberPlayerScreenY - 1f;
            float leftRequired = Mathf.Abs(player.position.x - bounds.min.x) / aspect;
            float rightRequired = Mathf.Abs(bounds.max.x - player.position.x) / aspect;
            float bottomRequired = Mathf.Abs(player.position.y - bounds.min.y) / Mathf.Max(0.1f, 1f - offsetFactor);
            float topRequired = Mathf.Abs(bounds.max.y - player.position.y) / Mathf.Max(0.1f, 1f + offsetFactor);
            float required = Mathf.Max(leftRequired, rightRequired, bottomRequired, topRequired) + chamberCameraFitPadding;
            return Mathf.Max(baseSize, required * chamberCameraFitMultiplier);
        }

        private Bounds CalculateChamberBounds()
        {
            bool initialized = false;
            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);

            void Encapsulate(Vector3 point)
            {
                if (!initialized)
                {
                    bounds = new Bounds(point, Vector3.zero);
                    initialized = true;
                    return;
                }

                bounds.Encapsulate(point);
            }

            for (int i = 0; i < stationLayouts.Count; i++)
            {
                Vector2Int cell = stationLayouts[i].cell;
                Encapsulate(grid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0)) + new Vector3(0.47f, 1.35f, 0f));
            }

            Encapsulate(exitWorld + new Vector3(0f, 1.3f, 0f));
            Encapsulate(grid.GetCellCenterWorld(new Vector3Int(chamberDummyCell.x, chamberDummyCell.y, 0)) + new Vector3(0f, 1.2f, 0f));
            if (player != null) Encapsulate(player.position);

            return initialized ? bounds : new Bounds(Vector3.zero, new Vector3(12f, 8f, 0f));
        }

        private IEnumerator AttuneRoutine(ClassType cls)
        {
            if (player == null)
            {
                yield break;
            }

            busyAttuning = true;
            Vector3 startScale = player.localScale;
            SpriteRenderer[] renderers = player.GetComponentsInChildren<SpriteRenderer>();

            EchoStation station = stations.TryGetValue(cls, out var st) ? st : null;
            SpriteRenderer stationRenderer = station != null ? station.statue : null;
            Transform stationTransform = stationRenderer != null ? stationRenderer.transform : null;
            Vector3 stationStartScale = stationTransform != null ? stationTransform.localScale : Vector3.one;

            GameObject burst = new GameObject("CyanBurst");
            if (stationTransform != null) burst.transform.position = stationTransform.position + new Vector3(0f, 0.4f, 0f);
            SpriteRenderer burstSr = burst.AddComponent<SpriteRenderer>();
            burstSr.sprite = Resources.Load<Sprite>("Props/arrival_ring_0") ?? Resources.Load<Sprite>("Props/arrival_ring");
            burstSr.sortingLayerName = "Characters";
            burstSr.sortingOrder = 300;
            burstSr.color = new Color(0f, 1f, 1f, 0f);

            for (float t = 0f; t < 0.5f; t += Time.deltaTime)
            {
                if (player == null)
                {
                    if (burst != null) Destroy(burst);
                    busyAttuning = false;
                    yield break;
                }

                float k = Mathf.Clamp01(t / 0.5f);
                float sinK = Mathf.Sin(k * Mathf.PI);
                player.localScale = Vector3.Lerp(startScale, startScale * 0.86f, sinK);
                foreach (SpriteRenderer sr in renderers)
                {
                    if (sr != null)
                    {
                        sr.color = Color.Lerp(Color.white, new Color(0.55f, 1f, 1f, 1f), sinK);
                    }
                }
                
                if (stationRenderer != null && stationTransform != null)
                {
                    stationTransform.localScale = Vector3.Lerp(stationStartScale, stationStartScale * 1.3f, sinK);
                    stationRenderer.color = Color.Lerp(new Color(0.75f, 0.78f, 0.82f, 1f), new Color(0f, 1f, 1f, 1f), sinK);
                }

                if (burstSr != null)
                {
                    burstSr.transform.localScale = Vector3.one * Mathf.Lerp(0f, 3.5f, k);
                    burstSr.color = new Color(0f, 1f, 1f, 1f - k);
                }

                yield return null;
            }

            if (burst != null) Destroy(burst);

            if (stationTransform != null)
            {
                stationTransform.localScale = stationStartScale;
            }

            ApplySelectedClassToPlayer(cls);
            player.localScale = startScale;
            foreach (SpriteRenderer sr in renderers)
            {
                if (sr != null) sr.color = Color.white;
            }
            RefreshEchoVisuals();
            Debug.Log($"[ChamberSelectBootstrap] P3 evidence: attuned to {cls}; PlayerClassManager.SelectedClass synchronized.");
            busyAttuning = false;
        }

        /// <summary>
        /// FIX 3: Called by CharacterSelectScreen.OnBackClicked when running in chamber-embed mode.
        /// Closes the overlay and resets state so the world prompt can reappear.
        /// </summary>
        public bool CloseChamberOverlay()
        {
            if (!dummySelectOpen && !classicTabOpen)
            {
                return false;   // not in chamber-embed mode, caller should handle normally
            }

            dummySelectOpen = false;
            classicTabOpen = false;
            SetClassicOverlayVisible(false);
            return true;
        }

        public bool AcceptClassicSelectionFromPopup(ClassType cls)
        {
            if (!dummySelectOpen)
            {
                return false;
            }

            if (!IsDemoSelectable(cls))
            {
                InvokeClassicSelect(currentClass);
                return true;
            }

            if (!IsUnlocked(cls))
            {
                TryUnlockFromPopup(cls);
                return true;
            }

            ApplySelectedClassToDummyOnly(cls);
            dummySelectOpen = false;
            classicTabOpen = false;
            SetClassicOverlayVisible(false);
            return true;
        }

        private void TryUnlockFromPopup(ClassType cls)
        {
            if (!IsDemoSelectable(cls))
            {
                return;
            }

            if (!CanUnlock(cls))
            {
                return;
            }

            Unlock(cls);
            InvokeClassicSelect(cls);
            RefreshEchoVisuals();
            Debug.Log($"[ChamberSelectBootstrap] P3 unlock: {cls} unlocked through chamber popup.");
        }

        private void ApplySelectedClassToPlayer(ClassType cls)
        {
            currentClass = cls;
            PlayerClassManager.SelectedClass = cls;
            EnsureClassManager().SetPrimaryClass(cls);

            if (player != null)
            {
                // FIX 1: Update attack profile when class changes via attune.
                AssignAttackProfileToPlayer(player.gameObject, cls);
                ApplyChamberPlayerVisual(player.gameObject, cls);
                // Re-grant the practice loadout for the newly attuned class so the dummy stays castable.
                GrantPracticeLoadout(cls);
            }
        }

        // Chamber-only demo kit (Q/E/R/F). Names resolve to SkillData via SkillDatabase exactly like
        // the run-start draft, so practice skills carry their real cooldown/cost/icon. The first three
        // mirror DraftManager.ClassKits; a 4th implemented skill fills the F slot. Only the two
        // demo-selectable classes (Warblade/Elementalist) need an entry.
        private static readonly System.Collections.Generic.Dictionary<ClassType, string[]> PracticeKits =
            new System.Collections.Generic.Dictionary<ClassType, string[]>
            {
                { ClassType.Warblade,     new[] { "Iron Charge", "Gravity Cleave", "Earthsplitter", "Cleave" } },
                { ClassType.Elementalist, new[] { "Fireball", "Glacial Spike", "Chain Lightning", "Frozen Orb" } },
            };

        /// <summary>
        /// Chamber-only: fill the active class controller's Q/E/R/F slots with its demo kit so the
        /// player can cast skills on the training dummy before committing at the rift. Mirrors the
        /// bind pattern in DraftManager.AssignActive (AddComponent(skillType) → copy name/icon/cd →
        /// SetSlot). Does NOT touch the run-start empty-loadout design lock — this only runs in the
        /// Attunement Chamber and the player gets a fresh _Arena instance + draft when they enter a run.
        /// </summary>
        private void GrantPracticeLoadout(ClassType cls)
        {
            if (player == null) return;
            if (!PracticeKits.TryGetValue(cls, out string[] kit)) return;   // non-demo class: leave empty
            ApplyLoadoutKit(cls, kit, "Practice");
        }

        /// <summary>
        /// Chamber-only: bind a player-chosen Q/E/R/F kit (full-roster picker). Starts from the class's
        /// default practice kit and overlays any non-null entry from <paramref name="picked"/>, so slots
        /// the player left untouched keep the default. Same chamber-only guarantees as GrantPracticeLoadout:
        /// in-memory components destroyed on scene unload; the run gets a fresh _Arena + empty draft.
        /// </summary>
        private void GrantCustomLoadout(ClassType cls, string[] picked)
        {
            if (player == null) return;
            if (!PracticeKits.TryGetValue(cls, out string[] defaults)) return;   // non-demo class: ignore

            string[] kit = (string[])defaults.Clone();
            if (picked != null)
                for (int i = 0; i < kit.Length && i < picked.Length; i++)
                    if (!string.IsNullOrEmpty(picked[i])) kit[i] = picked[i];

            ApplyLoadoutKit(cls, kit, "Custom");
        }

        // Shared bind loop for GrantPracticeLoadout / GrantCustomLoadout. Mirrors DraftManager.AssignActive
        // (AddComponent(skillType) → copy name/icon/cd → SetSlot Q/E/R/F).
        private void ApplyLoadoutKit(ClassType cls, string[] kit, string label)
        {
            Component host = cls == ClassType.Elementalist
                ? (Component)player.GetComponent<Elementalist_SkillController>()
                : player.GetComponent<Warblade_SkillController>();
            if (host == null) return;

            SkillDatabase db = SkillDatabase.Instance;
            db?.EnsureBuilt();

            for (int i = 0; i < kit.Length; i++)
            {
                SkillData data = db?.FindByName(kit[i]);
                if (data == null || data.skillType == null)
                {
                    Debug.LogWarning($"[ChamberSelectBootstrap] {label} kit skill '{kit[i]}' unresolved for {cls}; slot {i} left empty.");
                    continue;
                }

                var comp = host.GetComponent(data.skillType) as SkillBase
                        ?? host.gameObject.AddComponent(data.skillType) as SkillBase;
                if (comp == null)
                {
                    Debug.LogWarning($"[ChamberSelectBootstrap] {label} kit '{kit[i]}' component add failed (type={data.skillType}).");
                    continue;
                }

                comp.skillName = data.skillName;
                comp.icon = data.icon;
                comp.cooldown = data.cooldown;

                if (host is Elementalist_SkillController el) el.SetSlot(i, comp);
                else if (host is Warblade_SkillController wb) wb.SetSlot(i, comp);
            }

            practiceRefillTimer = 0f;   // top up resource on the next Update tick
            TopUpPracticeResource();
            Debug.Log($"[ChamberSelectBootstrap] {label} loadout granted for {cls}: {kit.Length} skills on Q/E/R/F.");
        }

        /// <summary>Chamber-only: refill the player's active skill resource to max so practice casts are
        /// never blocked. Resolves the SAME resource SkillBase.TryActivate spends (Elementalist=Mana,
        /// else Rage) — the player carries both RageSystem and ManaSystem after an Elementalist attune,
        /// so a bare GetComponent&lt;PlayerResourceBase&gt; could top up the wrong one.</summary>
        private void TopUpPracticeResource()
        {
            if (player == null) return;
            PlayerResourceBase resource = currentClass == ClassType.Elementalist
                ? (PlayerResourceBase)player.GetComponent<ManaSystem>()
                : player.GetComponent<RageSystem>();
            if (resource == null) resource = player.GetComponent<PlayerResourceBase>();
            if (resource != null && resource.Current < resource.Max)
                resource.Add(resource.Max);
        }

        private void ApplySelectedClassToDummyOnly(ClassType cls)
        {
            if (dummyRenderer != null)
            {
                dummyRenderer.sprite = LoadClassIdleSouthSprite(cls, out bool usedFallback);
                dummyRenderer.color = Color.white;
                if (usedFallback)
                {
                    Debug.LogWarning($"[ChamberSelectBootstrap] Missing idle_south sprite for dummy {cls}; using readable class sprite fallback.");
                }
            }
        }

        /// <summary>
        /// FIX 1: Forcefully assign BasicAttackProfile to PlayerAttack, re-enabling it if Awake
        /// disabled it due to the profile being null at construction time.
        /// </summary>
        private static void AssignAttackProfileToPlayer(GameObject playerObject, ClassType cls)
        {
            if (playerObject == null) return;
            PlayerAttack attack = playerObject.GetComponent<PlayerAttack>();
            if (attack == null) return;

            BasicAttackProfile profile = Resources.Load<BasicAttackProfile>($"Combat/BasicAttack/BasicAttackProfile_{cls}");
#if UNITY_EDITOR
            if (profile == null && cls == ClassType.Ronin)
                profile = UnityEditor.AssetDatabase.LoadAssetAtPath<BasicAttackProfile>(
                    "Assets/Data/Combat/Profiles/Ronin_BasicAttackProfile.asset");
#endif
            if (profile == null)
            {
                Debug.LogWarning($"[ChamberSelectBootstrap] BasicAttackProfile not found for {cls}; attack will remain disabled.");
                return;
            }

            // SetBasicAttackProfile re-enables the component and reinitialises behaviour.
            attack.SetBasicAttackProfile(profile);
        }

        /// <summary>
        /// FIX 6: Instantiate SlashArcVFX as a child of the player and wire it to PlayerAttack.
        /// </summary>
        private static void AssignSlashArcVFXToPlayer(GameObject playerObject)
        {
            if (playerObject == null) return;
            PlayerAttack attack = playerObject.GetComponent<PlayerAttack>();
            if (attack == null) return;

            // Skip if already wired (e.g. prefab had it serialized).
            System.Reflection.FieldInfo field = typeof(PlayerAttack).GetField("slashArcVFX",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (field != null && field.GetValue(attack) != null) return;

            // Check both known prefab locations.
            GameObject vfxPrefab = Resources.Load<GameObject>("Prefabs/VFX/SlashArcVFX")
                ?? Resources.Load<GameObject>("Prefabs/Combat/SlashArcVFX");
#if UNITY_EDITOR
            if (vfxPrefab == null)
                vfxPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/VFX/SlashArcVFX.prefab")
                    ?? UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Combat/SlashArcVFX.prefab");
#endif
            if (vfxPrefab == null)
            {
                Debug.LogWarning("[ChamberSelectBootstrap] SlashArcVFX prefab not found; hit VFX will be silent.");
                return;
            }

            // Instantiate as child of player so it moves with them and uses their position.
            GameObject vfxInstance = Instantiate(vfxPrefab, playerObject.transform.position, Quaternion.identity);
            vfxInstance.transform.SetParent(playerObject.transform, true);
            vfxInstance.transform.localPosition = Vector3.zero;
            vfxInstance.name = "SlashArcVFX";

            SlashArcVFX vfxComponent = vfxInstance.GetComponent<SlashArcVFX>();
            if (vfxComponent == null) return;

            field?.SetValue(attack, vfxComponent);
        }

        private static void ApplyChamberPlayerVisual(GameObject playerObject, ClassType cls)
        {
            // BUG-1 (2026-06-10): do NOT early-return when an Animator exists. The demo animator
            // skeleton (Warblade/Elementalist controllers) only has EMPTY placeholder clips that
            // drive no sprite curves, so the manual sprite assignment below is still required —
            // otherwise selecting Elementalist leaves the Warblade prefab sprite on screen.
            // Controller swap per class is handled by PlayerClassManager.SetPrimaryClass.
            if (playerObject == null)
            {
                return;
            }

            Sprite sprite = LoadClassIdleSouthSprite(cls, out bool usedFallback);
            SpriteRenderer[] renderers = playerObject.GetComponentsInChildren<SpriteRenderer>(true);
            SpriteRenderer body = null;
            for (int i = 0; i < renderers.Length; i++)
            {
                SpriteRenderer candidate = renderers[i];
                if (candidate != null && candidate.gameObject.name == "Body")
                {
                    body = candidate;
                    break;
                }
            }

            if (body == null && renderers.Length > 0)
            {
                body = renderers[0];
            }

            if (body == null || sprite == null)
            {
                return;
            }

            body.sprite = sprite;
            body.color = Color.white;
            if (usedFallback)
            {
                Debug.LogWarning($"[ChamberSelectBootstrap] Missing idle_south sprite for player {cls}; using readable class sprite fallback.");
            }
        }

        private void RefreshEchoVisuals()
        {
            foreach (EchoStation station in stations.Values)
            {
                bool unlocked = IsDemoSelectable(station.classType) && IsUnlocked(station.classType);
                bool highlighted = station.classType == highlightedClass;
                bool occupied = station.classType == currentClass;

                if (station.statue != null)
                {
                    // FIX 4: Clearly distinct unlocked vs locked.
                    // Locked = very dark/desaturated. Unlocked = bright warm-white. Occupied = cyan.
                    station.statue.color = !unlocked
                        ? new Color(0.22f, 0.23f, 0.26f, 0.80f)   // clearly dark/locked
                        : occupied
                            ? new Color(0.45f, 0.96f, 1f, 1f)       // cyan = current class
                            : highlighted
                                ? new Color(0.95f, 1f, 1f, 1f)       // near-white when hovered
                                : new Color(0.92f, 0.94f, 1f, 1f);  // bright for available classes
                }

                if (station.pedestal != null)
                {
                    // FIX 3c: Pedestal oval glows ONLY when highlighted (player adjacent) or occupied.
                    // Default (unlocked, not highlighted, not occupied) = very dim; locked = almost off.
                    station.pedestal.color = highlighted || occupied
                        ? new Color(0.28f, 0.92f, 1f, 0.95f)   // bright cyan when near or current class
                        : unlocked
                            ? new Color(0.10f, 0.22f, 0.26f, 0.30f)   // very dim — almost invisible at rest
                            : new Color(0.10f, 0.11f, 0.13f, 0.20f);  // nearly off for locked
                    station.pedestal.transform.localScale = highlighted
                        ? new Vector3(1.46f, 0.50f, 1f)
                        : new Vector3(1.28f, 0.42f, 1f);
                }

                if (station.pedestalLight != null)
                {
                    // FIX 3c: Light is OFF by default; only the highlighted station glows.
                    station.pedestalLight.intensity = !unlocked ? 0.04f : highlighted ? 1.40f : occupied ? 0.30f : 0.04f;
                    station.pedestalLight.pointLightOuterRadius = highlighted ? 3.8f : 3.0f;
                    station.pedestalLight.color = !unlocked
                        ? new Color(0.25f, 0.28f, 0.32f, 1f)   // cold dark for locked
                        : new Color(0.18f, 0.88f, 1f, 1f);     // cyan for unlocked
                }
            }
        }

        private void InvokeClassicSelect(ClassType cls)
        {
            if (classicScreen == null || selectClassMethod == null)
            {
                return;
            }

            selectClassMethod.Invoke(classicScreen, new object[] { cls });
        }

        private void CacheClassicCanvas()
        {
            classicCanvas = GetComponentInChildren<Canvas>(true);
            if (classicCanvas == null)
            {
                return;
            }

            classicCanvasGroup = classicCanvas.GetComponent<CanvasGroup>();
            if (classicCanvasGroup == null)
            {
                classicCanvasGroup = classicCanvas.gameObject.AddComponent<CanvasGroup>();
            }
        }

        private void SetClassicOverlayVisible(bool visible)
        {
            if (classicCanvas == null)
            {
                CacheClassicCanvas();
            }

            if (classicCanvasGroup == null)
            {
                return;
            }

            classicCanvasGroup.alpha = visible ? 1f : 0f;
            classicCanvasGroup.interactable = visible;
            classicCanvasGroup.blocksRaycasts = visible;
        }

        // Chamber-only IMGUI skill picker. Lists the active class's implemented active skills; the player
        // picks one then assigns it to a Q/E/R/F slot, then "Uygula" binds via GrantCustomLoadout. Reset is
        // automatic (in-memory skill components destroyed on scene unload) — no persistence, run untouched.
        private static readonly string[] PickerSlotLabels = { "Q", "E", "R", "F" };

        private void OnGUI()
        {
            if (!skillPickerOpen) return;

            SkillDatabase db = SkillDatabase.Instance;
            if (db == null) return;
            db.EnsureBuilt();

            // Implemented, slottable (non-passive) skills of the active class.
            List<SkillData> pool = db.GetPool(currentClass, ClassType.None);
            var skills = new List<SkillData>(pool.Count);
            foreach (SkillData s in pool)
                if (!s.isPassive && s.skillType != null) skills.Add(s);

            const float panelW = 340f;
            float panelH = Mathf.Min(Screen.height - 80f, 430f);
            var panel = new Rect(Screen.width - panelW - 24f, 60f, panelW, panelH);

            GUI.color = new Color(0f, 0f, 0f, 0.82f);
            GUI.Box(panel, GUIContent.none);
            GUI.color = Color.white;

            GUILayout.BeginArea(new Rect(panel.x + 12f, panel.y + 10f, panel.width - 24f, panel.height - 20f));

            GUILayout.Label($"SKİLL SEÇİCİ — {currentClass.ToString().ToUpperInvariant()}");
            GUILayout.Label("Skill seç, sonra bir slota (Q/E/R/F) ata.");

            // Pending Q/E/R/F slot buttons — click to assign the selected skill.
            GUILayout.BeginHorizontal();
            for (int slot = 0; slot < 4; slot++)
            {
                string assigned = string.IsNullOrEmpty(pickerSlotKit[slot]) ? "—" : pickerSlotKit[slot];
                if (GUILayout.Button($"{PickerSlotLabels[slot]}\n{assigned}", GUILayout.Height(46f)))
                {
                    if (pickerSelectedSkill >= 0 && pickerSelectedSkill < skills.Count)
                        pickerSlotKit[slot] = skills[pickerSelectedSkill].skillName;
                    else
                        pickerSlotKit[slot] = null;   // no selection: clear back to default
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(4f);

            pickerSkillScroll = GUILayout.BeginScrollView(pickerSkillScroll);
            for (int i = 0; i < skills.Count; i++)
            {
                bool sel = i == pickerSelectedSkill;
                GUI.color = sel ? new Color(1f, 0.85f, 0.4f, 1f) : Color.white;
                if (GUILayout.Button(skills[i].skillName, GUILayout.Height(26f)))
                    pickerSelectedSkill = i;
            }
            GUI.color = Color.white;
            GUILayout.EndScrollView();

            GUILayout.Space(4f);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Uygula", GUILayout.Height(30f)))
            {
                GrantCustomLoadout(currentClass, pickerSlotKit);
            }
            if (GUILayout.Button("Varsayılan", GUILayout.Height(30f)))
            {
                for (int s = 0; s < pickerSlotKit.Length; s++) pickerSlotKit[s] = null;
                pickerSelectedSkill = -1;
                GrantPracticeLoadout(currentClass);
            }
            if (GUILayout.Button("Kapat [K]", GUILayout.Height(30f)))
            {
                skillPickerOpen = false;
            }
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void StartRun()
        {
            UIManager.Instance?.ResumeGame();
            PlayerClassManager.SelectedClass = currentClass;
            EnsureClassManager().SetPrimaryClass(currentClass);
            RunStats.Instance?.StartNewRun();
            MapFlowManager.Instance?.ResetRun();

            string targetScene = ArenaRunSceneName;
            Debug.Log($"[ChamberSelectBootstrap] P3 evidence: rift door confirmed; loading {targetScene} with class {currentClass}.");
            SceneManager.LoadScene(targetScene);
            Destroy(gameObject);
        }

        private static bool IsUnlocked(ClassType cls)
        {
            return ClassUnlockPolicy.IsUnlocked(cls);
        }

        private static bool IsDemoSelectable(ClassType cls)
        {
            return ClassUnlockPolicy.IsDemoPlayable(cls);
        }

        private static bool CanUnlock(ClassType cls)
        {
            return ClassUnlockPolicy.CanUnlockWithEcho(cls);
        }

        private static void Unlock(ClassType cls)
        {
            ClassUnlockPolicy.TryUnlockWithEcho(cls);
        }

        private static string UnlockPrefKey(ClassType cls) => ClassUnlockPolicy.UnlockPrefKey(cls);

        private static int UnlockCost(ClassType cls) => ClassUnlockPolicy.UnlockCost(cls);

        private static string UnlockConditionText(ClassType cls)
        {
            return cls switch
            {
                ClassType.Ravager => "Act2 boss'u Warblade ile",
                ClassType.Ronin => "Act2 boss'u Shadowblade ile",
                ClassType.Gunslinger => "Ranger ile Act2'ye ulas",
                ClassType.Brawler => "Ravager ile Act2'ye ulas",
                ClassType.Summoner => "art arda 3 run Act2",
                ClassType.Hexer => "Elementalist ile run bitir",
                _ => ""
            };
        }

        private static string UnlockOrPathText(ClassType cls)
        {
            int cost = UnlockCost(cls);
            return cost <= 0 ? "" : $"{cost} SHATTERED ECHO veya {UnlockConditionText(cls)}";
        }

        private static int SortOrder(Vector3 position)
        {
            return 80 + Mathf.RoundToInt(30f - position.y * 10f);
        }

        private int RemoveChamberEnemyLeakage()
        {
            HashSet<GameObject> removals = new HashSet<GameObject>();

            foreach (GameObject go in FindObjectsByType<GameObject>(FindObjectsSortMode.None))
            {
                if (go == null || go.name.StartsWith("TrainingDummy_", StringComparison.Ordinal) || go.name == "Dummy")
                {
                    continue;
                }

                if (IsChamberEnemyLeak(go))
                {
                    removals.Add(go);
                }
            }

            foreach (GameObject go in removals)
            {
                if (go != null)
                {
                    Destroy(go);
                }
            }

            if (removals.Count > 0)
            {
                Debug.Log($"[ChamberSelectBootstrap] Removed {removals.Count} pre-existing enemy/mob object(s) from chamber scene.");
            }

            return removals.Count;
        }

        private static bool IsChamberEnemyLeak(GameObject go)
        {
            if (go.CompareTag("Enemy")) return true;
            if (go.GetComponent<EnemyAI>() != null) return true;
            if (go.GetComponent<BaseMobBehavior>() != null) return true;
            if (go.GetComponent<HollowMite>() != null) return true;
            if (go.GetComponent<SeamCrawler_Trail>() != null) return true;
            if (go.GetComponent<SeamCrawler_Homing>() != null) return true;

            string name = go.name;
            return name.StartsWith("Mob_", StringComparison.Ordinal) ||
                   name.StartsWith("SeamCrawler", StringComparison.Ordinal) ||
                   name.StartsWith("HollowMite", StringComparison.Ordinal);
        }

        private static Sprite LoadClassIdleSouthSprite(ClassType cls, out bool usedFallback)
        {
            Sprite sprite = TryLoadClassIdleSouthSprite(cls);
            if (sprite != null)
            {
                usedFallback = false;
                return sprite;
            }

            usedFallback = true;
            ClassType fallbackClass = cls == ClassType.Warblade ? ClassType.Ranger : ClassType.Warblade;
            Sprite fallback = TryLoadClassIdleSouthSprite(fallbackClass);
            return fallback != null ? fallback : GenericEchoSilhouetteSprite();
        }

        private static Sprite TryLoadClassIdleSouthSprite(ClassType cls)
        {
            string className = cls.ToString();
            string lower = className.ToLowerInvariant();
            string editorPath = $"Assets/Resources/Characters/{className}/{lower}_idle_south.png";
            string resourcesPath = $"Characters/{className}/{lower}_idle_south";
            return LoadAsset<Sprite>(editorPath, resourcesPath);
        }

        private static Sprite genericEchoSilhouetteSprite;
        private static Sprite echoPedestalSprite;
        private static Sprite whiteSprite;

        private static Sprite EchoPedestalSprite()
        {
            if (echoPedestalSprite != null)
            {
                return echoPedestalSprite;
            }

            const int width = 64;
            const int height = 32;
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            texture.wrapMode = TextureWrapMode.Clamp;

            Color clear = Color.clear;
            Color rim = Color.white;
            Color core = new Color(1f, 1f, 1f, 0.55f);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float nx = (x + 0.5f - width * 0.5f) / (width * 0.5f);
                    float ny = (y + 0.5f - height * 0.5f) / (height * 0.5f);
                    float d = nx * nx + ny * ny;
                    if (d <= 0.55f)
                    {
                        texture.SetPixel(x, y, core);
                    }
                    else if (d <= 0.88f)
                    {
                        texture.SetPixel(x, y, rim);
                    }
                    else
                    {
                        texture.SetPixel(x, y, clear);
                    }
                }
            }

            texture.Apply(false, true);
            echoPedestalSprite = Sprite.Create(texture, new Rect(0f, 0f, width, height), new Vector2(0.5f, 0.5f), 64f);
            return echoPedestalSprite;
        }

        private static Sprite WhiteSprite()
        {
            if (whiteSprite != null)
            {
                return whiteSprite;
            }

            Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply(false, true);
            whiteSprite = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            return whiteSprite;
        }

        private static Sprite GenericEchoSilhouetteSprite()
        {
            if (genericEchoSilhouetteSprite != null)
            {
                return genericEchoSilhouetteSprite;
            }

            const int width = 32;
            const int height = 48;
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;

            Color clear = Color.clear;
            Color fill = Color.white;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float nx = (x - width * 0.5f) / (width * 0.5f);
                    float ny = y / (float)(height - 1);
                    bool head = (nx * nx / 0.22f) + ((ny - 0.74f) * (ny - 0.74f) / 0.035f) <= 1f;
                    bool torso = Mathf.Abs(nx) <= Mathf.Lerp(0.20f, 0.42f, Mathf.InverseLerp(0.66f, 0.25f, ny)) && ny > 0.22f && ny < 0.66f;
                    bool baseShadow = (nx * nx / 0.55f) + ((ny - 0.14f) * (ny - 0.14f) / 0.018f) <= 1f;
                    texture.SetPixel(x, y, head || torso || baseShadow ? fill : clear);
                }
            }

            texture.Apply(false, true);
            genericEchoSilhouetteSprite = Sprite.Create(texture, new Rect(0f, 0f, width, height), new Vector2(0.5f, 0.08f), 64f);
            return genericEchoSilhouetteSprite;
        }

        private static int RoundToEven(int value)
        {
            return (value & 1) == 0 ? value : value + 1;
        }

        private static T AddIfMissing<T>(GameObject target) where T : Component
        {
            T existing = target.GetComponent<T>();
            return existing != null ? existing : target.AddComponent<T>();
        }

        private static void SetField(object target, string name, object value)
        {
            FieldInfo field = target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
            field?.SetValue(target, value);
        }

        private static T LoadAsset<T>(string editorPath, string resourcesPath) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            T editorAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(editorPath);
            if (editorAsset != null)
            {
                return editorAsset;
            }
#endif
            return string.IsNullOrEmpty(resourcesPath) ? null : Resources.Load<T>(resourcesPath);
        }

        private sealed class EchoStation
        {
            public ClassType classType;
            public Vector2Int cell;
            public SpriteRenderer statue;
            public SpriteRenderer pedestal;
            public Light2D pedestalLight;
        }

        private readonly struct EchoStationLayout
        {
            public readonly Vector2Int cell;

            public EchoStationLayout(Vector2Int cell)
            {
                this.cell = cell;
            }
        }

        private readonly struct DummyHpBar
        {
            private readonly SpriteRenderer fill;
            private readonly float width;
            public readonly GameObject RootGo;

            public DummyHpBar(GameObject rootGo, SpriteRenderer fill, float width)
            {
                RootGo = rootGo;
                this.fill = fill;
                this.width = width;
            }

            public void SetPercent(float pct)
            {
                if (fill == null)
                {
                    return;
                }

                float clamped = Mathf.Clamp01(pct);
                fill.transform.localScale = new Vector3(width * clamped, fill.transform.localScale.y, 1f);
                fill.transform.localPosition = new Vector3(-width * 0.5f + width * clamped * 0.5f, 0f, 0f);
            }
        }

        /// <summary>
        /// FIX 2c: Shows the dummy's name label + HP bar only while the mouse cursor is over the dummy.
        /// Uses a per-frame screen-space raycast so it works with trigger colliders.
        /// </summary>
        private sealed class DummyHoverVisibility : MonoBehaviour
        {
            private GameObject nameGo;
            private GameObject barGo;
            private bool isHovered;
            private Collider2D col;

            public void Initialize(GameObject nameLabelGo, GameObject hpBarGo)
            {
                nameGo = nameLabelGo;
                barGo = hpBarGo;
                col = GetComponent<Collider2D>();
                SetVisible(false);
            }

            private void Update()
            {
                if (col == null)
                {
                    return;
                }

                Camera cam = Camera.main;
                if (cam == null)
                {
                    return;
                }

                Vector2 mouseWorld = cam.ScreenToWorldPoint(UnityEngine.InputSystem.Mouse.current != null
                    ? (Vector2)UnityEngine.InputSystem.Mouse.current.position.ReadValue()
                    : (Vector2)Input.mousePosition);

                bool nowHovered = col.OverlapPoint(mouseWorld);
                if (nowHovered != isHovered)
                {
                    isHovered = nowHovered;
                    SetVisible(isHovered);
                }
            }

            private void SetVisible(bool visible)
            {
                if (nameGo != null) nameGo.SetActive(visible);
                if (barGo != null) barGo.SetActive(visible);
            }
        }

        private sealed class TrainingDummyTarget : MonoBehaviour
        {
            private Health health;
            private TMP_Text label;
            private TMP_Text hpNumberLabel;
            private DummyHpBar hpBar;
            private float lastDamageTime;

            public void Initialize(Health targetHealth, TMP_Text hpLabel, DummyHpBar bar, TMP_Text numberLabel = null)
            {
                health = targetHealth;
                label = hpLabel;
                hpBar = bar;
                hpNumberLabel = numberLabel;
                health.OnHealthChanged.AddListener(OnHealthChanged);
                health.OnDamageTaken.AddListener(_ => lastDamageTime = Time.time);
                health.OnDeath.AddListener(() =>
                {
                    health.RestoreToFull();
                    lastDamageTime = Time.time;
                    Debug.Log("[ChamberSelectBootstrap] P4 evidence: dummy reached 0 HP and was restored instead of dying.");
                });
                OnHealthChanged(health.CurrentHP, health.MaxHP);
            }

            private void Update()
            {
                if (health == null || health.CurrentHP >= health.MaxHP)
                {
                    return;
                }

                if (Time.time - lastDamageTime >= 2f)
                {
                    health.RestoreToFull();
                    Debug.Log("[ChamberSelectBootstrap] P4 evidence: dummy HP regenerated after 2 seconds without damage.");
                }
            }

            private void OnHealthChanged(int current, int max)
            {
                if (label != null)
                {
                    label.text = "Dummy";
                }

                // FIX STEP4: update numeric HP display
                if (hpNumberLabel != null)
                {
                    hpNumberLabel.text = $"{current} / {max}";
                }

                hpBar.SetPercent(max > 0 ? current / (float)max : 0f);
            }
        }

        /// <summary>
        /// Chamber-only off-map safety. If the practice player leaves the walkable platform (outside the
        /// floor radius or below the Y floor) — e.g. flung by a dash/charge skill such as Iron Charge or
        /// Blink — snap it back to the spawn point and kill velocity. Robust against the small diamond
        /// platform (radius/Y test, not fragile perimeter colliders). Only added by ChamberSelectBootstrap,
        /// so real gameplay rooms never carry this component.
        /// </summary>
        private sealed class ChamberBoundsGuard : MonoBehaviour
        {
            private Vector3 spawnWorld;
            private Vector3 center;
            private float radiusSqr;
            private float yFloor;
            private Rigidbody2D body;

            public void Initialize(Vector3 spawn, Vector3 platformCenter, float platformRadius, float yLimit)
            {
                spawnWorld = spawn;
                center = platformCenter;
                radiusSqr = platformRadius * platformRadius;
                yFloor = yLimit;
                body = GetComponent<Rigidbody2D>();
            }

            // FixedUpdate so the check runs in the same step the dash physics move the body.
            private void FixedUpdate()
            {
                Vector3 pos = body != null ? (Vector3)body.position : transform.position;
                Vector2 planar = new Vector2(pos.x - center.x, pos.y - center.y);
                bool outOfRadius = planar.sqrMagnitude > radiusSqr;
                bool belowFloor = pos.y < yFloor;
                if (!outOfRadius && !belowFloor) return;

                if (body != null)
                {
                    body.linearVelocity = Vector2.zero;
                    body.angularVelocity = 0f;
                    body.position = spawnWorld;
                }
                transform.position = spawnWorld;
                Debug.Log($"[ChamberSelectBootstrap] Off-map guard: player at {pos} snapped back to spawn {spawnWorld}.");
            }
        }
    }
}
