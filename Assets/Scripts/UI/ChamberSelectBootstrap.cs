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

namespace RIMA
{
    public sealed class ChamberSelectBootstrap : MonoBehaviour
    {
        private const string RoomAssetPath = "Assets/Data/Rooms/Special/Chamber_CharSelect.asset";
        private const string RegistryAssetPath = "Assets/Data/Props/PropRegistry.asset";
        private const string FloorTileResource = "ChamberSelect/Tiles/ChamberFloor";
        private const string CollisionTileResource = "ChamberSelect/Tiles/ChamberCollision";
        private const string OverlayTileResource = "ChamberSelect/Tiles/ChamberOverlayPath";
        private const string ChamberExitPortalResource = "Environment/Gate/gate_arch";
        private const string ArenaRunSceneName = "_Arena";
        private const string ClassUnlockPrefsPrefix = "rima_class_unlocked_";

        [SerializeField, Min(1f)] private float chamberCameraFitMultiplier = 1.04f;
        [SerializeField, Min(0f)] private float chamberCameraFitPadding = 0.35f;
        [SerializeField, Min(1f)] private float chamberCameraMinimumOrthographicSize = 5.8f;

        private static readonly ClassType[] ChamberClasses =
        {
            ClassType.Warblade, ClassType.Elementalist, ClassType.Ranger, ClassType.Shadowblade,
            ClassType.Ronin, ClassType.Ravager, ClassType.Gunslinger, ClassType.Brawler,
            ClassType.Summoner, ClassType.Hexer
        };

        private static readonly Vector2Int[] RosterCells =
        {
            new(4, 15), new(6, 16), new(8, 17), new(10, 18), new(12, 18),
            new(4, 5), new(5, 7), new(6, 9), new(7, 11), new(8, 13)
        };

        private static readonly Vector2Int ExitCell = new(24, 17);
        private static readonly Vector2Int DummyCell = new(14, 10);

        private readonly Dictionary<ClassType, EchoStation> stations = new();

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
        private ClassType currentClass = ClassType.Warblade;
        private ClassType highlightedClass = ClassType.None;
        private bool classicTabOpen;
        private bool dummySelectOpen;
        private bool busyAttuning;
        private Vector3 exitWorld;
        private Transform dummyTransform;
        private SpriteRenderer dummyRenderer;
        private CameraZoom disabledCameraZoom;
        private Behaviour disabledLegacyPixelPerfectCamera;
        private PixelPerfectCamera disabledUrpPixelPerfectCamera;
        private RIMA.CameraSystem.CameraFollow chamberFollow;
        private Vector3 previousFollowOffset;
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

            GameObject trailGo = new GameObject("CyanGuidanceTrail");
            trailGo.transform.SetParent(chamberRoot);
            LineRenderer lr = trailGo.AddComponent<LineRenderer>();
            lr.positionCount = 4;
            Vector3 spawn = grid.GetCellCenterWorld(new Vector3Int(3, 3, 0));
            lr.SetPosition(0, spawn + new Vector3(0, -0.4f, 0));
            lr.SetPosition(1, grid.GetCellCenterWorld(new Vector3Int(8, 7, 0)) + new Vector3(0, -0.4f, 0));
            lr.SetPosition(2, grid.GetCellCenterWorld(new Vector3Int(15, 11, 0)) + new Vector3(0, -0.4f, 0));
            lr.SetPosition(3, exitWorld + new Vector3(0, -0.4f, 0));
            lr.startWidth = 0.04f;
            lr.endWidth = 0.015f;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = new Color(0f, 1f, 1f, 0.45f);
            lr.endColor = new Color(0f, 1f, 1f, 0.05f);
            lr.sortingLayerName = "Floor";
            lr.sortingOrder = 1;

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

            int lightCount = 0;
            foreach (var cell in RosterCells)
            {
                if (lightCount >= 6) break;
                if (lightCount % 2 == 0 || lightCount == 5)
                {
                    Vector3 pedPos = grid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0)) + new Vector3(0.47f, 0.5f, 0f);
                    GameObject pedLightGo = new GameObject("PedestalLight");
                    pedLightGo.transform.SetParent(chamberRoot);
                    pedLightGo.transform.position = pedPos;
                    Light2D pedLight = pedLightGo.AddComponent<Light2D>();
                    pedLight.lightType = Light2D.LightType.Point;
                    pedLight.intensity = 0.6f;
                    pedLight.pointLightOuterRadius = 3f;
                    pedLight.color = new Color(0f, 1f, 1f, 1f);
                }
                lightCount++;
            }
        }

        private void SpawnChamberExitPortal(Transform chamberRoot)
        {
            GameObject portalGo = new GameObject("ChamberExitPortal");
            portalGo.transform.SetParent(chamberRoot);
            portalGo.transform.position = exitWorld + new Vector3(0f, 0.48f, 0f);
            portalGo.transform.localScale = new Vector3(0.95f, 0.95f, 1f);

            SpriteRenderer sr = portalGo.AddComponent<SpriteRenderer>();
            sr.sprite = Resources.Load<Sprite>(ChamberExitPortalResource)
                ?? LoadAsset<Sprite>("Assets/Resources/Environment/Gate/gate_arch.png", ChamberExitPortalResource);
            sr.color = new Color(0f, 0f, 0f, 0.96f);
            sr.sortingLayerName = "Characters";
            sr.sortingOrder = SortOrder(portalGo.transform.position) - 4;
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
            }
        }

        private void Update()
        {
            if (player == null || busyAttuning)
            {
                return;
            }

            if (WasPressed(UnityEngine.InputSystem.Keyboard.current?.tabKey))
            {
                classicTabOpen = !classicTabOpen;
                SetClassicOverlayVisible(classicTabOpen);
            }

            bool nearDoor = Vector2.Distance(player.position, exitWorld) <= 0.85f;
            bool nearDummy = dummyTransform != null && Vector2.Distance(player.position, dummyTransform.position) <= 1.1f;
            if (dummySelectOpen && !nearDummy)
            {
                dummySelectOpen = false;
                classicTabOpen = false;
                SetClassicOverlayVisible(false);
            }

            if (dummySelectOpen)
            {
                ClassType selected = PlayerClassManager.SelectedClass;
                if (selected != ClassType.None && selected != currentClass && IsUnlocked(selected))
                {
                    ApplySelectedClassToPlayerAndDummy(selected);
                }

                SetClassicOverlayVisible(true);
            }

            if (nearDummy)
            {
                highlightedClass = currentClass;
                SetClassicOverlayVisible(dummySelectOpen || classicTabOpen);
                promptLabel.gameObject.SetActive(true);
                promptLabel.transform.position = dummyTransform.position + Vector3.up * 0.9f;
                promptLabel.text = dummySelectOpen
                    ? Loc.T("chamber_select.prompt.attune", currentClass.ToString().ToUpperInvariant())
                    : "G: CHARACTER SELECT";

                if (WasPressed(UnityEngine.InputSystem.Keyboard.current?.gKey))
                {
                    dummySelectOpen = !dummySelectOpen;
                    classicTabOpen = dummySelectOpen;
                    InvokeClassicSelect(currentClass);
                    SetClassicOverlayVisible(dummySelectOpen);
                }
            }
            else if (nearDoor)
            {
                highlightedClass = ClassType.None;
                SetClassicOverlayVisible(classicTabOpen);
                promptLabel.gameObject.SetActive(true);
                promptLabel.transform.position = exitWorld + Vector3.up * 0.7f;
                promptLabel.text = Loc.T("chamber_select.prompt.enter_rift");

                if (WasPressed(UnityEngine.InputSystem.Keyboard.current?.gKey))
                {
                    StartRun();
                }
            }
            else
            {
                highlightedClass = ClassType.None;
                SetClassicOverlayVisible(classicTabOpen);
                if (promptLabel != null)
                {
                    promptLabel.gameObject.SetActive(false);
                }
            }

            RefreshEchoVisuals();
        }

        private static bool WasPressed(UnityEngine.InputSystem.Controls.ButtonControl key)
        {
            return key != null && key.wasPressedThisFrame;
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

            roomTemplate = LoadAsset<RoomTemplateSO>(RoomAssetPath, null);
            PropRegistrySO registry = LoadAsset<PropRegistrySO>(RegistryAssetPath, null);
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
            SetField(builder, "cliffSouth", LoadAsset<Sprite>("Assets/Sprites/Environment/CliffKit_RefB_pixelified/cliff_S.png", null));
            SetField(builder, "cliffSouthEast", LoadAsset<Sprite>("Assets/Sprites/Environment/CliffKit_RefB_pixelified/cliff_SE.png", null));
            SetField(builder, "cliffSouthWest", LoadAsset<Sprite>("Assets/Sprites/Environment/CliffKit_RefB_pixelified/cliff_SW.png", null));
            SetField(builder, "gateNorthSprite", LoadAsset<Sprite>("Assets/Resources/Environment/Gate/gate_arch.png", "Environment/Gate/gate_arch"));

            if (roomTemplate == null || floorTile == null || collisionTile == null || registry == null)
            {
                Debug.LogError("[ChamberSelectBootstrap] BLOCKED: missing generated chamber assets. Run RIMA/Character Select/Generate Attunement Chamber Assets.");
                enabled = false;
                return;
            }

            builder.Build(roomTemplate);
            exitWorld = grid.GetCellCenterWorld(new Vector3Int(ExitCell.x, ExitCell.y, 0));

            int floorCount = builder.LastFloorCells != null ? builder.LastFloorCells.Count : 0;
            Debug.Log($"[ChamberSelectBootstrap] P1/P2 evidence: room={roomTemplate.roomId}, floor={floorCount}, props={roomTemplate.props.Count}, roster={RosterCells.Length}, no pedestal discs.");
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
                || objectName.StartsWith("EchoLabel_", StringComparison.Ordinal);
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

            Vector3 spawn = builder.PlayerSpawnMarker != null
                ? builder.PlayerSpawnMarker.position
                : grid.GetCellCenterWorld(new Vector3Int(3, 3, 0));

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
            EnsureClassManager().SetPrimaryClass(currentClass);
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

            for (int i = 0; i < ChamberClasses.Length; i++)
            {
                ClassType cls = ChamberClasses[i];
                Vector2Int cell = RosterCells[i];
                Vector3 baseWorld = grid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0));

                GameObject statue = new GameObject($"EchoStatue_{cls}");
                statue.transform.SetParent(root, false);
                statue.transform.position = baseWorld + new Vector3(0.47f, 0.48f, 0f);
                SpriteRenderer sr = statue.AddComponent<SpriteRenderer>();
                sr.sprite = LoadClassIdleSouthSprite(cls, out bool usedFallback);
                sr.sortingLayerName = "Characters";
                sr.sortingOrder = SortOrder(statue.transform.position);
                statue.transform.localScale = new Vector3(0.66f, 0.66f, 1f);
                if (usedFallback)
                {
                    Debug.LogWarning($"[ChamberSelectBootstrap] Missing idle_south sprite for {cls}; using generic dark echo silhouette instead of Warblade fallback.");
                }

                TMP_Text label = CreateWorldText($"EchoLabel_{cls}", root, baseWorld + new Vector3(0.48f, 0.08f, 0f), 2.7f);
                label.text = IsUnlocked(cls) ? cls.ToString().ToUpperInvariant() : $"{cls.ToString().ToUpperInvariant()}\n{UnlockOrPathText(cls)}";
                label.alignment = TextAlignmentOptions.Center;
                label.gameObject.SetActive(false);

                stations[cls] = new EchoStation
                {
                    classType = cls,
                    cell = cell,
                    statue = sr,
                    label = label,
                    labelAnchor = baseWorld + new Vector3(0.48f, 0.22f, 0f)
                };
            }

            Debug.Log("[ChamberSelectBootstrap] P3 evidence: 10 front-facing echo silhouettes spawned as display-only 5+5 along the two left edges.");
        }

        private void SpawnTrainingDummy()
        {
            Vector3 pos = grid.GetCellCenterWorld(new Vector3Int(DummyCell.x, DummyCell.y, 0));
            GameObject dummy = new GameObject("Dummy");
            dummy.transform.position = pos;
            dummy.tag = "Untagged";
            int enemyLayer = LayerMask.NameToLayer("Enemy");
            if (enemyLayer >= 0) dummy.layer = enemyLayer;
            dummyTransform = dummy.transform;

            SpriteRenderer sr = dummy.AddComponent<SpriteRenderer>();
            sr.sprite = LoadClassIdleSouthSprite(currentClass, out bool usedFallback);
            if (usedFallback)
            {
                Debug.LogWarning($"[ChamberSelectBootstrap] Missing {currentClass} idle_south for dummy; using generic dark echo silhouette.");
            }
            sr.color = Color.white;
            sr.sortingLayerName = "Characters";
            sr.sortingOrder = SortOrder(pos);
            dummyRenderer = sr;
            dummy.transform.localScale = new Vector3(0.72f, 0.72f, 1f);

            Rigidbody2D body = dummy.AddComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Kinematic;
            body.gravityScale = 0f;
            body.freezeRotation = true;
            body.constraints = RigidbodyConstraints2D.FreezeAll;
            BoxCollider2D collider = dummy.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(0.7f, 0.7f);
            collider.isTrigger = true;
            Health health = dummy.AddComponent<Health>();
            health.SetMaxHP(100000);
            dummy.AddComponent<RIMA.Combat.HitFlashDriver>();

            TMP_Text hpLabel = CreateWorldText("TrainingDummy_HP", dummy.transform, pos + new Vector3(0f, 1.05f, 0f), 3.1f);
            hpLabel.text = Loc.T("chamber_select.dummy_hp", health.CurrentHP, health.MaxHP);
            ScreenshotMode.Register(hpLabel.gameObject, "TrainingDummy_HP");
            dummy.AddComponent<TrainingDummyTarget>().Initialize(health, hpLabel);
            Debug.Log("[ChamberSelectBootstrap] P4 evidence: solid immortal class-select Dummy spawned at chamber center.");
        }

        private void CreatePromptLabel()
        {
            promptLabel = CreateWorldText("ChamberPrompt", transform, Vector3.zero, 1.8f);
            promptLabel.color = new Color(0.45f, 0.96f, 1f, 1f);
            promptLabel.fontStyle = FontStyles.Bold;
            promptLabel.gameObject.SetActive(false);
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

            Bounds chamberBounds = CalculateChamberBounds();
            float aspect = chamberCamera.aspect > 0.01f ? chamberCamera.aspect : 16f / 9f;
            float fittedSize = Mathf.Max(chamberBounds.extents.y, chamberBounds.extents.x / aspect);
            float orthoSize = Mathf.Max(chamberCameraMinimumOrthographicSize, fittedSize * chamberCameraFitMultiplier + chamberCameraFitPadding);
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
                previousFollowCaptured = true;
            }
            chamberFollow = follow;
            follow.target = player;
            Vector3 followOffset = chamberBounds.center - player.position;
            followOffset.z = -10f;
            follow.worldOffset = followOffset;
            chamberCamera.transform.position = player.position + followOffset;

            if (FindFirstObjectByType<Light2D>() == null)
            {
                GameObject lightGo = new GameObject("Chamber_GlobalLight2D");
                Light2D light = lightGo.AddComponent<Light2D>();
                light.lightType = Light2D.LightType.Global;
                light.intensity = 0.92f;
                light.color = new Color(0.78f, 0.86f, 1f, 1f);
            }
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

            for (int i = 0; i < RosterCells.Length; i++)
            {
                Vector2Int cell = RosterCells[i];
                Encapsulate(grid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0)) + new Vector3(0.47f, 1.35f, 0f));
            }

            Encapsulate(exitWorld + new Vector3(0f, 1.3f, 0f));
            Encapsulate(grid.GetCellCenterWorld(new Vector3Int(DummyCell.x, DummyCell.y, 0)) + new Vector3(0f, 1.2f, 0f));
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

            ApplySelectedClassToPlayerAndDummy(cls);
            player.localScale = startScale;
            foreach (SpriteRenderer sr in renderers)
            {
                if (sr != null) sr.color = Color.white;
            }
            RefreshEchoVisuals();
            Debug.Log($"[ChamberSelectBootstrap] P3 evidence: attuned to {cls}; PlayerClassManager.SelectedClass synchronized.");
            busyAttuning = false;
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

            ApplySelectedClassToPlayerAndDummy(cls);
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
            Debug.Log($"[ChamberSelectBootstrap] P3 unlock: {cls} unlocked through dummy character-select popup.");
        }

        private void ApplySelectedClassToPlayerAndDummy(ClassType cls)
        {
            currentClass = cls;
            PlayerClassManager.SelectedClass = cls;
            EnsureClassManager().SetPrimaryClass(cls);

            if (player != null)
            {
                ApplyChamberPlayerVisual(player.gameObject, cls);
            }

            if (dummyRenderer != null)
            {
                dummyRenderer.sprite = LoadClassIdleSouthSprite(cls, out bool usedFallback);
                dummyRenderer.color = Color.white;
                if (usedFallback)
                {
                    Debug.LogWarning($"[ChamberSelectBootstrap] Missing idle_south sprite for dummy {cls}; using generic dark echo silhouette.");
                }
            }
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
                Debug.LogWarning($"[ChamberSelectBootstrap] Missing idle_south sprite for player {cls}; using generic dark echo silhouette.");
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
                    station.statue.color = !unlocked
                        ? new Color(0f, 0f, 0f, 0.92f)
                        : occupied
                            ? new Color(0.45f, 0.96f, 1f, 1f)
                            : highlighted
                                ? new Color(0.86f, 1f, 1f, 1f)
                                : new Color(0.75f, 0.78f, 0.82f, 1f);
                }

                if (station.label != null)
                {
                    station.label.text = unlocked
                        ? station.classType.ToString().ToUpperInvariant()
                        : $"{station.classType.ToString().ToUpperInvariant()}\n{UnlockOrPathText(station.classType)}";
                    station.label.color = highlighted
                        ? new Color(0.50f, 0.96f, 1f, 1f)
                        : new Color(0.82f, 0.86f, 0.90f, 1f);
                    station.label.gameObject.SetActive(highlighted);
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
            return cls == ClassType.Warblade ||
                   cls == ClassType.Elementalist ||
                   cls == ClassType.Ranger ||
                   cls == ClassType.Shadowblade ||
                   PlayerPrefs.GetInt(UnlockPrefKey(cls), 0) == 1;
        }

        private static bool CanUnlock(ClassType cls)
        {
            return !IsUnlocked(cls) && EchoWallet.Balance >= UnlockCost(cls);
        }

        private static void Unlock(ClassType cls)
        {
            int cost = UnlockCost(cls);
            if (!EchoWallet.TrySpend(cost)) return;

            PlayerPrefs.SetInt(UnlockPrefKey(cls), 1);
            PlayerPrefs.Save();
        }

        private static string UnlockPrefKey(ClassType cls) => ClassUnlockPrefsPrefix + cls;

        private static int UnlockCost(ClassType cls)
        {
            return cls switch
            {
                ClassType.Ronin => 150,
                ClassType.Ravager => 150,
                ClassType.Gunslinger => 200,
                ClassType.Brawler => 200,
                ClassType.Summoner => 200,
                ClassType.Hexer => 250,
                _ => 0
            };
        }

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
            string className = cls.ToString();
            string lower = className.ToLowerInvariant();
            string editorPath = $"Assets/Resources/Characters/{className}/{lower}_idle_south.png";
            string resourcesPath = $"Characters/{className}/{lower}_idle_south";
            Sprite sprite = LoadAsset<Sprite>(editorPath, resourcesPath);
            usedFallback = sprite == null;
            return sprite != null ? sprite : GenericEchoSilhouetteSprite();
        }

        private static Sprite genericEchoSilhouetteSprite;

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
            public TMP_Text label;
            public Vector3 labelAnchor;
        }

        private sealed class TrainingDummyTarget : MonoBehaviour
        {
            private Health health;
            private TMP_Text label;
            private float lastDamageTime;

            public void Initialize(Health targetHealth, TMP_Text hpLabel)
            {
                health = targetHealth;
                label = hpLabel;
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
                    label.text = Loc.T("chamber_select.dummy_hp", current, max);
                }
            }
        }
    }
}
