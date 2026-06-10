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
        private const float ClassInteractRadius = 2.0f;
        private const float ClassConfirmRadius = 2.2f;
        private const float DoorInteractRadius = 3.5f;
        private const float DummyInteractRadius = 3.5f;
        private const int StandRowCount = 5;

        [SerializeField, Min(1f)] private float chamberCameraFitMultiplier = 1.04f;
        [SerializeField, Min(0f)] private float chamberCameraFitPadding = 0.35f;
        [SerializeField, Min(1f)] private float chamberCameraMinimumOrthographicSize = 5.8f;
        [SerializeField, Range(0.45f, 0.72f)] private float chamberPlayerScreenY = 0.60f;
        // DEMO camera consistency: must match RoomRunDirector.fixedOrthographicSize (5.0) so the
        // chamber -> _Arena transition has no zoom pop. (OVERLAP/DEMO Faz 1.)
        [SerializeField, Range(2.8f, 7f)] private float chamberCameraOrthoSize = 5.0f;
        [SerializeField, Min(0f)] private float chamberGlobalLightIntensity = 1.10f;
        [SerializeField, Min(0f)] private float chamberFillLightIntensity = 0.35f;
        // FIX E: dummy scale and cell override (x=0,y=0 means use auto placement)
        [SerializeField] private Vector2Int chamberDummyCellOverride = new Vector2Int(0, 0);
        [SerializeField, Range(0.8f, 2.5f)] private float chamberDummyScale = 1.70f;

        private static readonly ClassType[] ChamberClasses =
        {
            ClassType.Warblade, ClassType.Elementalist, ClassType.Ranger, ClassType.Shadowblade,
            ClassType.Ronin, ClassType.Ravager, ClassType.Gunslinger, ClassType.Brawler,
            ClassType.Summoner, ClassType.Hexer
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

            if (WasPressed(UnityEngine.InputSystem.Keyboard.current?.tabKey))
            {
                classicTabOpen = !classicTabOpen;
                SetClassicOverlayVisible(classicTabOpen);
            }

            ClassType nearbyClass = FindNearbyClassStation(out EchoStation nearbyStation);
            bool nearDoor = Vector2.Distance(player.position, exitWorld) <= DoorInteractRadius;
            bool nearDummy = dummyTransform != null && Vector2.Distance(player.position, dummyTransform.position) <= DummyInteractRadius;
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

                if (!IsUnlocked(nearbyClass))
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
                ShowPrompt(Vector3.zero, "[G] Dummy — Sınıf Seç");

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
                chamberDummyCell = new Vector2Int(ExitCell.x - 5, 9);
                BuildFallbackStationLayout();
                return;
            }

            int axisX = Mathf.Clamp(ExitCell.x, minX + 2, maxX - 2);
            int frontY = minY + 4;
            chamberSpawnCell = PickNearestWalkableCell(new Vector2Int(axisX, frontY), new HashSet<Vector2Int>(), 10);

            // FIX D: wider column offset — use ~25% of usable width, min 5 cells, max 7 cells.
            // This spreads figures to use the full available floor so labels never overlap.
            int usableWidth = Mathf.Max(10, maxX - minX + 1);
            int columnOffset = Mathf.Clamp(Mathf.RoundToInt(usableWidth * 0.25f), 5, 7);
            int firstRowY = Mathf.Max(chamberSpawnCell.y + 4, minY + 5);
            int lastRowY = Mathf.Min(ExitCell.y - 3, maxY - 3);
            if (lastRowY < firstRowY + StandRowCount - 1)
            {
                firstRowY = Mathf.Clamp(minY + 4, minY + 1, maxY - StandRowCount - 2);
                lastRowY = Mathf.Min(maxY - 3, firstRowY + (StandRowCount - 1) * 2);
            }

            // Two straight columns, evenly spaced, deterministic.
            // FIX 3a: EQUAL SPACING — compute ideal Y positions first, then snap X only.
            // By locking each row's Y to the exact Lerp value, all rows have equal vertical gaps.
            // We only snap X (left/right columns), never Y, so the column stays visually uniform.
            HashSet<Vector2Int> reserved = new HashSet<Vector2Int> { chamberSpawnCell, ExitCell };
            List<EchoStationLayout> leftLayouts = new List<EchoStationLayout>(StandRowCount);
            List<EchoStationLayout> rightLayouts = new List<EchoStationLayout>(StandRowCount);
            for (int row = 0; row < StandRowCount; row++)
            {
                float k = StandRowCount <= 1 ? 0f : row / (float)(StandRowCount - 1);
                int y = Mathf.RoundToInt(Mathf.Lerp(firstRowY, lastRowY, k));

                // Snap X within the same row only — try ideal X first, then ±1 on X at same Y.
                Vector2Int leftIdeal = new Vector2Int(axisX - columnOffset, y);
                Vector2Int left = PickNearestWalkableSameRowY(leftIdeal, reserved, 5);
                reserved.Add(left);
                Vector2Int rightIdeal = new Vector2Int(axisX + columnOffset, y);
                Vector2Int right = PickNearestWalkableSameRowY(rightIdeal, reserved, 5);
                reserved.Add(right);

                leftLayouts.Add(new EchoStationLayout(left));
                rightLayouts.Add(new EchoStationLayout(right));
            }

            // Left column = indices 0,2,4,6,8 ; right column = 1,3,5,7,9
            stationLayouts.AddRange(leftLayouts);
            stationLayouts.AddRange(rightLayouts);

            // FIX E: use Inspector override if set (non-zero), else auto-place.
            if (chamberDummyCellOverride.x != 0 || chamberDummyCellOverride.y != 0)
            {
                chamberDummyCell = chamberDummyCellOverride;
            }
            else
            {
                // Collect all station cell positions so we can enforce a hard clearance gap.
                List<Vector2Int> allStationCells = new List<Vector2Int>(stationLayouts.Count);
                foreach (EchoStationLayout sl in stationLayouts)
                {
                    allStationCells.Add(sl.cell);
                }

                // Target the front-left open area: left of the left figure column AND near spawn level.
                // Figure stations start at firstRowY (spawn.y + 4 or higher), so placing the dummy
                // near spawn.y + 2 keeps it well below (in Y) the entire station zone.
                // dummyTargetX: as far left as the floor allows, at least 3 cells past the left column.
                int dummyTargetX = Mathf.Clamp(minX + 3, minX + 2, axisX - columnOffset - 3);
                // Use spawn Y + 2 so the dummy is in the front-floor zone, below all figure rows.
                int dummyTargetY = Mathf.Clamp(chamberSpawnCell.y + 2, minY + 3, maxY - 3);
                chamberDummyCell = PickNearestWalkableCellClearOfStations(
                    new Vector2Int(dummyTargetX, dummyTargetY),
                    reserved, allStationCells, axisX, 12);
            }
        }

        private void BuildFallbackStationLayout()
        {
            stationLayouts.Clear();
            for (int row = 0; row < StandRowCount; row++)
            {
                int y = 10 + row * 2;
                stationLayouts.Add(new EchoStationLayout(new Vector2Int(ExitCell.x - 4, y)));
            }

            for (int row = 0; row < StandRowCount; row++)
            {
                int y = 10 + row * 2;
                stationLayouts.Add(new EchoStationLayout(new Vector2Int(ExitCell.x + 4, y)));
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
            currentClass = PlayerClassManager.SelectedClass != ClassType.None && IsUnlocked(PlayerClassManager.SelectedClass)
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
            Debug.Log($"[ChamberSelectBootstrap] P3 evidence: player spawned as {currentClass} at {spawn}.");
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

            Debug.Log("[ChamberSelectBootstrap] Class figures spawned as 5 left + 5 right Hades-style stands around the spawn-to-portal axis.");
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

            // FIX 1: Derive bar/label world-Y from sprite bounds so they sit above the sprite top
            //        regardless of PPU, scale, or sprite sheet.  bar.bottom = sprite.bounds.max.y + margin.
            //        sprite.bounds is pivot-relative local space; scale × localMax → world offset from pivot.
            float spriteTopWorld;
            if (sr.sprite != null)
            {
                spriteTopWorld = dummy.transform.position.y + sr.sprite.bounds.max.y * dummyS;
            }
            else
            {
                // Fallback: use the old heuristic if sprite is missing.
                spriteTopWorld = dummy.transform.position.y + 0.95f + (chamberDummyScale - 0.72f) * 0.4f - 0.30f;
            }
            const float barMargin    = 0.18f;   // gap between sprite top and bar bottom
            const float barHeight    = 0.13f;   // matches CreateDummyHpBar Back sprite scale.y
            const float labelGap     = 0.10f;   // gap between bar top and name label bottom
            float barCenterY  = spriteTopWorld + barMargin + barHeight * 0.5f;
            float labelCenterY = barCenterY + barHeight * 0.5f + labelGap + 0.12f; // +0.12 = half text height

            // FIX 2: Name label says "Dummy"; show/hide on hover (default hidden).
            TMP_Text hpLabel = CreateWorldText("TrainingDummy_HP", dummy.transform, new Vector3(dummy.transform.position.x, labelCenterY, dummy.transform.position.z), 2.6f);
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

            // FIX 2: Use hero-scale zoom (chamberCameraOrthoSize) so the player fills more screen.
            float orthoSize = chamberCameraOrthoSize;
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
            }
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
            if (playerObject == null || playerObject.GetComponentInChildren<Animator>() != null)
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
                bool unlocked = IsUnlocked(station.classType);
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
    }
}
