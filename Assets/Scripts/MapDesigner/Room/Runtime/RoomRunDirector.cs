using System.Collections.Generic;
using RIMA.Background;
using RIMA.Encounter;
using RIMA.MapDesigner.Room.Data;
using RIMA.Shop;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RIMA.MapDesigner.Room.Runtime
{
    public enum RoomRunLifecycleState
    {
        Idle,
        Combat,
        Cleared,
        RewardTaken,
        DoorOpen,
        Advancing,
        Victory
    }

    public sealed class RoomRunLifecycle
    {
        public RoomRunLifecycleState State { get; private set; } = RoomRunLifecycleState.Idle;

        public void BeginCombat()
        {
            State = RoomRunLifecycleState.Combat;
        }

        public bool MarkCleared()
        {
            if (State != RoomRunLifecycleState.Combat)
            {
                return false;
            }

            State = RoomRunLifecycleState.Cleared;
            return true;
        }

        public bool MarkRewardTaken()
        {
            if (State != RoomRunLifecycleState.Cleared)
            {
                return false;
            }

            State = RoomRunLifecycleState.RewardTaken;
            return true;
        }

        public bool MarkDoorsOpened()
        {
            if (State != RoomRunLifecycleState.RewardTaken)
            {
                return false;
            }

            State = RoomRunLifecycleState.DoorOpen;
            return true;
        }

        public bool MarkAdvancing()
        {
            if (State != RoomRunLifecycleState.DoorOpen)
            {
                return false;
            }

            State = RoomRunLifecycleState.Advancing;
            return true;
        }

        public void MarkVictory()
        {
            State = RoomRunLifecycleState.Victory;
        }
    }

    public sealed class RoomRunDirector : MonoBehaviour
    {
        [SerializeField] private IsoRoomBuilder builder;
        [SerializeField] private RoomBankSO roomBank;
        [SerializeField] private Transform player;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Camera arenaCamera;
        [SerializeField] private EncounterBankSO encounterBank;
        [SerializeField] private EncounterController encounterController;
        [SerializeField] private GameObject defaultEnemyPrefab;
        [SerializeField] private float encounterDifficulty = 10f;
        [SerializeField] private Transform enemyContainer;
        [SerializeField] private float cameraPadding = 1.25f;

        // ── Demo mode ─────────────────────────────────────────────────────────────
        // When true, BeginRun uses DungeonGraph.BuildDemoSequence() instead of
        // the random generator — producing an exact 5-node linear run:
        //   Combat → Combat → Merchant → Combat → Boss
        // Set to false (or uncheck in Inspector) to restore the random path.
        [SerializeField] private bool forceDemoSequence = true;

        // When true, every room uses a fixed orthographicSize instead of
        // FitCameraToRoom so the zoom is identical in every room. The follow
        // camera continues to track the player for large rooms.
        [SerializeField] private bool useFixedDemoCamera = true;
        [SerializeField] private float fixedOrthographicSize = 5.0f;
        [SerializeField] private Sprite rewardSprite;
        [SerializeField] private float rewardColliderRadius = 0.45f;
        [SerializeField] private float clearSlowMoScale = 0.3f;
        [SerializeField] private float clearSlowMoReturnDuration = 0.6f;
        [SerializeField] private RoomTemplateSO fallbackTemplate;
        [SerializeField] private int runSeed = 12345;
        [SerializeField] private bool buildOnStart = true;
        // Design target = 6 depths (0..5): Combat → Combat → branch → Elite/Merchant → convergence → Boss.
        [SerializeField] private int depthCount = 6;

        [Header("Boss Spawn (demo path)")]
        [Tooltip("Prefab spawned at the Boss node. Must have PenitentSovereign + Health components. Falls back to Resources/Prefabs/Enemies/Boss/PenitentSovereign.")]
        [SerializeField] private GameObject bossPrefab;

        private const string DefaultPlayerPrefabPath = "Prefabs/Warblade";
        private const string DefaultEncounterBankPath = "Encounters/Act1_EncounterBank_Pilot";
        // The chest art is a 2-sprite SHEET; Resources.Load cannot address a sub-sprite by path+suffix
        // (old ".../Chest_1" returned NULL -> invisible reward; ".../Chest" returns the wrong 10x36 _0
        // slice). Load the sheet and pick the usable 62x72 icon by sub-name via LoadRewardChestSprite().
        private const string DefaultRewardSpritePath = "UI/RIMA/RIMA_UI_Node_Chest";
        private const string RewardSpriteSubName = "RIMA_UI_Node_Chest_1";
        private const string DefaultEnemyPrefabEditorPath = "Assets/Prefabs/Enemies/FractureImp.prefab";
        private const string DefaultBossPrefabEditorPath = "Assets/Prefabs/Enemies/Boss/PenitentSovereign.prefab";

        private bool warnedMissingPlayer;
        private bool warnedMissingEncounterBank;
        private bool warnedMissingArenaCamera;
        private bool warnedSpawnFallback;
        private DungeonGraph graph;
        private readonly List<GameObject> activeEnemies = new List<GameObject>();
        private readonly List<GameObject> activeDoors = new List<GameObject>();
        private readonly RoomRunLifecycle lifecycle = new RoomRunLifecycle();
        private RewardPickup activeReward;
        private ShopRoomController activeShopController;
        private Coroutine clearSequence;
        private Coroutine slowMoSequence;
        private Coroutine openingDraftSequence;
        // FIX-1 (B1): reconcile watchdog — runs in parallel with RoomClearSequence and
        // GUARANTEES the exit doors open (and timeScale is restored) even if the reward/draft
        // chain throws, stalls, or never spawns a reward. Door-open is decoupled from reward.
        private Coroutine reconcileSequence;

        public DungeonGraph Graph => graph;
        public int CurrentNodeId { get; private set; }
        public DungeonNode CurrentNode => graph?.Get(CurrentNodeId);
        public RIMA.RoomType CurrentRoomType => CurrentNode != null ? CurrentNode.roomType : RIMA.RoomType.Combat;
        public List<DungeonNode> CurrentChoices => graph != null ? graph.ChildrenOf(CurrentNodeId) : new List<DungeonNode>();
        public bool IsRunComplete => CurrentNode == null || CurrentChoices.Count == 0;
        public RoomTemplateSO CurrentTemplate { get; private set; }
        public RoomRunLifecycleState LifecycleState => lifecycle.State;
        public UnityEvent RoomCleared = new UnityEvent();

        private void Awake()
        {
            RIMA.ChamberSelectBootstrap.CleanupLeakedCombatChamberObjects();
        }

        private void Start()
        {
            // BUG-5 FIX: EnsureHUD unconditionally in Start so the HUD is always bootstrapped
            // on the main entry path (MainMenu→Chamber→_Arena). BuildCurrentRoom also calls it,
            // but calling here first guarantees it exists before any other Awake/Start code runs.
            if (Application.isPlaying)
                EnsureHUD();

            if (buildOnStart)
            {
                BeginRun();
            }
        }

        public void BeginRun()
        {
            BeginRun(depthCount);
        }

        public void BeginRun(int requestedDepthCount)
        {
            depthCount = Mathf.Max(2, requestedDepthCount);
            StopClearSequences();
            DestroyActiveReward();
            DestroyActiveShop();
            ClearActiveEnemies();
            BuildPersistentBackgroundIfPresent();

            if (forceDemoSequence)
            {
                // DEMO MODE: deterministic linear sequence — ignores seed and depthCount.
                graph = DungeonGraph.BuildDemoSequence();
                Debug.Log("[RoomRunDirector] DEMO MODE: using fixed linear sequence Combat→Combat→Merchant→Combat→Boss→Combat(post-boss)");
            }
            else
            {
                // Per-run seed: every BeginRun rolls a fresh seed so the branching map differs
                // each run (demo thesis: "her run procedural değişen harita"). Deterministic given
                // the seed, so the same seed reproduces the same map for demo replay.
                runSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
                Debug.Log($"[RunMap] seed={runSeed} depthCount={depthCount}");
                graph = DungeonGraph.Generate(runSeed, depthCount);
            }

            CurrentNodeId = graph.startId;
            openingDraftShown = false;
            BuildCurrentRoom();

            // F5 (2026-06-10): the run starts with an EMPTY loadout — open a 3-card draft from
            // the primary class KIT as soon as the first room is built so the player picks their
            // opening skill (slot 0 / Q). Play-mode only: EditMode tests call BeginRun directly.
            if (Application.isPlaying && isActiveAndEnabled)
            {
                openingDraftSequence = StartCoroutine(OpeningKitDraftSequence());
            }
        }

        private static void BuildPersistentBackgroundIfPresent()
        {
            PersistentBackgroundController background = FindObjectOfType<PersistentBackgroundController>();
            if (background != null)
            {
                background.BuildIfEnabled();
            }
        }

        private bool openingDraftShown;

        /// <summary>
        /// F5 run-start draft: waits for any pending/active draft to close (reuses the unlock-draft
        /// wait pattern from RoomClearSequence), then opens the class-kit draft. Timeout mirrors
        /// DraftAutoCloseTimeoutSec so an unattended draft can never softlock the run — on timeout
        /// the draft closes and the player starts skill-less (first room-clear draft fills Q).
        /// </summary>
        private System.Collections.IEnumerator OpeningKitDraftSequence()
        {
            if (openingDraftShown)
            {
                openingDraftSequence = null;
                yield break;
            }

            openingDraftShown = true;
            EnsureDraftManager();
            yield return null; // let DraftManager_Auto run Awake/Start before we use it

            DraftManager draft = DraftManager.Instance;
            if (draft == null)
            {
                Debug.LogWarning("[RoomRunDirector] Opening kit draft skipped: no DraftManager.");
                openingDraftSequence = null;
                yield break;
            }

            float waitTimer = 0f;
            while (draft.IsDraftPending || draft.IsDraftActive)
            {
                waitTimer += Time.unscaledDeltaTime;
                if (waitTimer >= DraftAutoCloseTimeoutSec)
                {
                    Debug.LogWarning($"[RoomRunDirector] Opening kit draft: prior draft not closed after {DraftAutoCloseTimeoutSec}s — force-closing.");
                    draft.HideDraft();
                    break;
                }
                yield return null;
            }

            draft.ShowOpeningKitDraft();

            float openTimer = 0f;
            while (draft.IsDraftActive)
            {
                openTimer += Time.unscaledDeltaTime;
                if (openTimer >= DraftAutoCloseTimeoutSec)
                {
                    Debug.LogWarning($"[RoomRunDirector] Opening kit draft not resolved after {DraftAutoCloseTimeoutSec}s — force-picking first kit skill to unblock run.");
                    draft.HideDraft();
                    // P0-7: auto-equip so player is never skill-less after timeout
                    draft.ForcePickFirstOpeningKitSkill();
                    break;
                }
                yield return null;
            }
            openingDraftSequence = null;
        }

        public void BuildCurrentRoom()
        {
            // Ensure HUD (HP bar, skill bar, interaction prompt) exists in the scene.
            // Called here so it runs once per room-build regardless of player-spawn result.
            if (Application.isPlaying)
                EnsureHUD();

            if (builder == null)
            {
                Debug.LogError("[RoomRunDirector] Missing IsoRoomBuilder reference.");
                return;
            }

            if (graph == null)
            {
                Debug.LogWarning("[RoomRunDirector] Missing dungeon graph.");
                return;
            }

            DungeonNode node = CurrentNode;
            if (node == null)
            {
                Debug.LogWarning($"[RoomRunDirector] Missing current node id={CurrentNodeId}.");
                return;
            }

            ResolveRuntimeReferences();
            StopClearSequences();
            DestroyActiveReward();
            DestroyActiveShop();
            ClearActiveEnemies();
            lifecycle.BeginCombat();

            List<DungeonNode> choices = CurrentChoices;
            RoomTemplateSO template = roomBank != null ? roomBank.Pick(CurrentRoomType, runSeed + CurrentNodeId, choices.Count) : null;
            if (template == null)
            {
                template = fallbackTemplate;
            }

            if (template == null)
            {
                Debug.LogError($"[RoomRunDirector] no template for {CurrentRoomType}");
                return;
            }

            CurrentTemplate = template;

            // F1 (2026-06-10 playtest): combat arenas are floor + cliff ONLY. Template prop
            // clusters (ritual circle / brazier / obelisk) boxed the player in at the spawn
            // between prop colliders and the south cliff. Shop/Boss/Chamber prop flows are
            // separate systems; only Combat and its Elite variant disable template props.
            builder.spawnProps = CurrentRoomType != RIMA.RoomType.Combat
                && CurrentRoomType != RIMA.RoomType.Elite;

            builder.Build(template);

            // Seed WalkabilityMap from template data so mob/knockback clamps use the
            // authoritative walkable grid rather than deriving it from rendered floor tiles.
            RIMA.Environment.WalkabilityMap walkMap = RIMA.Environment.WalkabilityMap.Instance;
            if (walkMap != null)
            {
                walkMap.InitFromTemplate(template);
            }

            if (useFixedDemoCamera)
            {
                // DEMO MODE: constant orthographic size so every room has the same zoom.
                // The follow camera continues to track the player for large rooms.
                ApplyFixedDemoCamera();
            }
            else
            {
                FitCameraToRoom();
            }

            EnsurePlayerAtSpawn();
            ConfigureFollowCamera();
            EnsureDeathScreenManager();

            Debug.Log($"[RoomRunDirector] Built node id={node.id} depth={node.depth} type={node.roomType} choices={CurrentChoices.Count} template={template.roomId}");

            // Exit doors = this node's branch choices (door count + each door's destination type).
            List<RIMA.RoomType> doorTypes = new List<RIMA.RoomType>();
            foreach (DungeonNode child in choices)
            {
                doorTypes.Add(child.roomType);
            }
            activeDoors.Clear();
            activeDoors.AddRange(builder.BuildExitDoors(doorTypes));
            ConfigureExitDoors(false);

            StartRoomEncounter();

            // K5.1 (DEMO_DESIGN_PLAN): show "ODA n/6 — TİP" label + entry banner.
            // Deferred by 1 frame so HUDController.Start() (which calls BuildHUD) has run first.
            if (Application.isPlaying)
                StartCoroutine(ShowRoomIdentityLabelDeferred());
        }

        private System.Collections.IEnumerator ShowRoomIdentityLabelDeferred()
        {
            yield return null; // one frame: lets HUDController.Start() → BuildHUD() run
            ShowRoomIdentityLabel();
        }

        private void ShowRoomIdentityLabel()
        {
            int roomNumber = CurrentNodeId + 1;
            int totalRooms = graph != null ? graph.nodes.Count : 6;
            string typeName = CurrentRoomType switch
            {
                RIMA.RoomType.Combat   => "SAVAŞ",
                RIMA.RoomType.Merchant => "TÜCCAR",
                RIMA.RoomType.Boss     => "BOSS",
                RIMA.RoomType.Elite    => "ELİT SAVAŞ",
                RIMA.RoomType.Chest    => "SANDIK",
                RIMA.RoomType.Forge    => "FORGE",
                _                     => CurrentRoomType.ToString().ToUpperInvariant(),
            };
            // Post-boss final Combat node has no children = run terminal.
            if (IsRunComplete) typeName = "SON ODA";
            string label = $"ODA {roomNumber}/{totalRooms} — {typeName}";
            HUDController.Instance?.SetRoomLabel(label);
        }

        private void ApplyFixedDemoCamera()
        {
            Camera targetCamera = arenaCamera != null ? arenaCamera : Camera.main;
            if (targetCamera == null)
            {
                if (!warnedMissingArenaCamera)
                {
                    Debug.LogWarning("[RoomRunDirector] Missing arenaCamera and Camera.main; fixed demo camera skipped.");
                    warnedMissingArenaCamera = true;
                }
                return;
            }

            float size = Mathf.Max(1f, fixedOrthographicSize);
            targetCamera.orthographic = true;
            targetCamera.orthographicSize = size;
            // NOTE: do NOT reposition the camera — let the follow-camera system track the player.
            float aspect = targetCamera.aspect > 0f ? targetCamera.aspect : 16f / 9f;
            ConfigurePixelPerfectCamera(targetCamera, size, aspect);
            Debug.Log($"[RoomRunDirector] Fixed demo camera orthographicSize={size} node={CurrentNodeId} type={CurrentRoomType}");
        }

        /// <summary>
        /// BUG-2 (2026-06-10): generated rooms (24x18, boss 36x28) no longer fit in the fixed
        /// 5.0-ortho view, and _Arena's Main Camera has no follow component — the camera never
        /// moved. Attach the live CameraFollow (RIMA.CameraSystem) at runtime, target the player,
        /// clamp to the built room's floor bounds and snap so room transitions don't pan across
        /// the map. Zoom stays fixed at 5.0 (ApplyFixedDemoCamera); chamber scene is untouched.
        /// </summary>
        private void ConfigureFollowCamera()
        {
            if (!useFixedDemoCamera)
            {
                return;
            }

            Camera targetCamera = arenaCamera != null ? arenaCamera : Camera.main;
            if (targetCamera == null)
            {
                return;
            }

            RIMA.CameraSystem.CameraFollow follow = targetCamera.GetComponent<RIMA.CameraSystem.CameraFollow>();
            if (follow == null)
            {
                follow = targetCamera.gameObject.AddComponent<RIMA.CameraSystem.CameraFollow>();
            }

            if (player != null)
            {
                follow.target = player;
            }

            if (builder != null && builder.TryGetLastFloorWorldBounds(out Bounds floorBounds))
            {
                follow.SetBounds(floorBounds);
            }
            else
            {
                follow.ClearBounds();
            }

            follow.SnapToTarget();
        }

        private void FitCameraToRoom()
        {
            Camera targetCamera = arenaCamera != null ? arenaCamera : Camera.main;
            if (targetCamera == null || !builder.TryGetLastFloorWorldBounds(out Bounds bounds))
            {
                if (targetCamera == null && !warnedMissingArenaCamera)
                {
                    Debug.LogWarning("[RoomRunDirector] Missing arenaCamera and Camera.main; room camera fit skipped.");
                    warnedMissingArenaCamera = true;
                }
                return;
            }

            float aspect = targetCamera.aspect > 0f ? targetCamera.aspect : 16f / 9f;
            float halfHeight = bounds.extents.y + cameraPadding;
            float halfWidthAsHeight = (bounds.extents.x + cameraPadding) / aspect;
            float orthographicSize = Mathf.Max(1f, halfHeight, halfWidthAsHeight);

            targetCamera.orthographic = true;
            targetCamera.orthographicSize = orthographicSize;
            targetCamera.transform.position = new Vector3(bounds.center.x, bounds.center.y, targetCamera.transform.position.z);
            ConfigurePixelPerfectCamera(targetCamera, orthographicSize, aspect);
        }

        private static void ConfigurePixelPerfectCamera(Camera targetCamera, float orthographicSize, float aspect)
        {
            MonoBehaviour[] behaviours = targetCamera.GetComponents<MonoBehaviour>();
            for (int i = 0; i < behaviours.Length; i++)
            {
                MonoBehaviour behaviour = behaviours[i];
                if (behaviour == null || behaviour.GetType().Name != "PixelPerfectCamera")
                {
                    continue;
                }

                int assetsPPU = Mathf.Max(1, GetIntProperty(behaviour, "assetsPPU", 64));
                SetIntProperty(behaviour, "refResolutionY", Mathf.CeilToInt(orthographicSize * 2f * assetsPPU));
                SetIntProperty(behaviour, "refResolutionX", Mathf.CeilToInt(orthographicSize * 2f * aspect * assetsPPU));
                SetBoolProperty(behaviour, "cropFrameX", false);
                SetBoolProperty(behaviour, "cropFrameY", false);
                SetBoolProperty(behaviour, "upscaleRT", false);
            }
        }

        private static int GetIntProperty(object target, string propertyName, int fallback)
        {
            System.Reflection.PropertyInfo property = target.GetType().GetProperty(propertyName);
            if (property == null || property.PropertyType != typeof(int))
            {
                return fallback;
            }

            return (int)property.GetValue(target);
        }

        private static void SetIntProperty(object target, string propertyName, int value)
        {
            System.Reflection.PropertyInfo property = target.GetType().GetProperty(propertyName);
            if (property != null && property.PropertyType == typeof(int) && property.CanWrite)
            {
                property.SetValue(target, value);
            }
        }

        private static void SetBoolProperty(object target, string propertyName, bool value)
        {
            System.Reflection.PropertyInfo property = target.GetType().GetProperty(propertyName);
            if (property != null && property.PropertyType == typeof(bool) && property.CanWrite)
            {
                property.SetValue(target, value);
            }
        }

        private void EnsurePlayerAtSpawn()
        {
            if (!TryResolvePlayerSpawnWorld(out Vector3 spawnWorld))
            {
                Debug.LogWarning("[RoomRunDirector] Missing player spawn marker and no walkable fallback cell; player spawn skipped.");
                return;
            }

            if (player != null && !IsFunctionalPlayer(player.gameObject))
            {
                DestroyRuntimeObject(player.gameObject);
                player = null;
            }

            if (player == null)
            {
                GameObject taggedPlayer = GameObject.FindGameObjectWithTag("Player");
                if (taggedPlayer != null)
                {
                    player = taggedPlayer.transform;
                }
            }

            if (player == null && playerPrefab != null)
            {
                GameObject instance = Instantiate(playerPrefab, spawnWorld, Quaternion.identity);
                instance.name = "Player";
                player = instance.transform;
            }

            if (player == null)
            {
                if (!warnedMissingPlayer)
                {
                    Debug.LogWarning($"[RoomRunDirector] Missing playerPrefab and Resources.Load<GameObject>(\"{DefaultPlayerPrefabPath}\") failed; player spawn skipped.");
                    warnedMissingPlayer = true;
                }
                return;
            }

            player.position = spawnWorld;
            EnsurePlayerRuntime(player.gameObject);
            EnsurePrimaryClass();
        }

        private bool TryResolvePlayerSpawnWorld(out Vector3 spawnWorld)
        {
            spawnWorld = default;
            bool hasUsableSocket = CurrentTemplate != null
                && CurrentTemplate.playerSpawn != null
                && CurrentTemplate.IsWalkable(CurrentTemplate.playerSpawn.position)
                && builder.PlayerSpawnMarker != null;

            if (hasUsableSocket)
            {
                // F2: prefer a 3x3 walkable clearance around the spawn. Generated sockets sit
                // on the bottom edge row (south cliff); when the socket itself has no clearance
                // nudge to the nearest cleared interior cell so the player can always move.
                Vector2Int socketCell = CurrentTemplate.playerSpawn.position;
                if (!HasWalkableClearance(CurrentTemplate, socketCell)
                    && TryFindNearestClearanceCell(CurrentTemplate, socketCell, out Vector2Int clearedCell)
                    && builder.TryGetCellCenterWorld(clearedCell, out Vector3 clearedWorld))
                {
                    spawnWorld = clearedWorld;
                    return true;
                }

                spawnWorld = builder.PlayerSpawnMarker.position;
                return true;
            }

            if (!warnedSpawnFallback)
            {
                string reason = builder.PlayerSpawnMarker == null ? "missing spawn marker" : "spawn socket is not walkable";
                string roomName = CurrentTemplate != null && !string.IsNullOrEmpty(CurrentTemplate.roomId) ? CurrentTemplate.roomId : "unknown";
                Debug.LogWarning($"[RoomRunDirector] Using fallback player spawn for {roomName}: {reason}.");
                warnedSpawnFallback = true;
            }

            if (CurrentTemplate == null || !TryFindBottomCenterWalkableCell(CurrentTemplate, out Vector2Int fallbackCell))
            {
                return false;
            }

            if (builder.TryGetCellCenterWorld(fallbackCell, out spawnWorld))
            {
                return true;
            }

            spawnWorld = new Vector3(fallbackCell.x, fallbackCell.y, 0f);
            return true;
        }

        // F2: fallback spawn = bottom-center INTERIOR cell. Cells with a full 3x3 walkable
        // clearance are preferred (so the spawn never hugs the south cliff edge or a wall);
        // the old "lowest Y, closest to horizontal center" pick survives only as last resort.
        public static bool TryFindBottomCenterWalkableCell(RoomTemplateSO template, out Vector2Int cell)
        {
            cell = default;
            bool foundAny = false;
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            for (int y = template.bounds.yMin; y < template.bounds.yMax; y++)
            {
                for (int x = template.bounds.xMin; x < template.bounds.xMax; x++)
                {
                    Vector2Int candidate = new Vector2Int(x, y);
                    if (!template.IsWalkable(candidate))
                    {
                        continue;
                    }

                    foundAny = true;
                    minX = Mathf.Min(minX, x);
                    maxX = Mathf.Max(maxX, x);
                }
            }

            if (!foundAny)
            {
                return false;
            }

            float centerX = (minX + maxX) * 0.5f;
            if (TryFindBottomCenterCell(template, centerX, true, out cell))
            {
                return true;
            }

            return TryFindBottomCenterCell(template, centerX, false, out cell);
        }

        private static bool TryFindBottomCenterCell(RoomTemplateSO template, float centerX, bool requireClearance, out Vector2Int cell)
        {
            cell = default;
            bool found = false;
            int bestY = int.MaxValue;
            float bestDistance = float.MaxValue;
            for (int y = template.bounds.yMin; y < template.bounds.yMax; y++)
            {
                for (int x = template.bounds.xMin; x < template.bounds.xMax; x++)
                {
                    Vector2Int candidate = new Vector2Int(x, y);
                    if (!template.IsWalkable(candidate))
                    {
                        continue;
                    }

                    if (requireClearance && !HasWalkableClearance(template, candidate))
                    {
                        continue;
                    }

                    float distance = Mathf.Abs(x - centerX);
                    if (y < bestY || (y == bestY && distance < bestDistance))
                    {
                        cell = candidate;
                        bestY = y;
                        bestDistance = distance;
                        found = true;
                    }
                }
            }

            return found;
        }

        // F2: 3x3 walkable clearance — the cell itself plus all 8 neighbors must be walkable.
        public static bool HasWalkableClearance(RoomTemplateSO template, Vector2Int cell)
        {
            if (template == null)
            {
                return false;
            }

            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (!template.IsWalkable(new Vector2Int(cell.x + dx, cell.y + dy)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // F2: nearest (squared euclidean) walkable cell with full 3x3 clearance to origin.
        public static bool TryFindNearestClearanceCell(RoomTemplateSO template, Vector2Int origin, out Vector2Int cell)
        {
            cell = default;
            if (template == null)
            {
                return false;
            }

            bool found = false;
            int bestSqr = int.MaxValue;
            for (int y = template.bounds.yMin; y < template.bounds.yMax; y++)
            {
                for (int x = template.bounds.xMin; x < template.bounds.xMax; x++)
                {
                    Vector2Int candidate = new Vector2Int(x, y);
                    if (!template.IsWalkable(candidate) || !HasWalkableClearance(template, candidate))
                    {
                        continue;
                    }

                    int dx = x - origin.x;
                    int dy = y - origin.y;
                    int sqr = dx * dx + dy * dy;
                    if (sqr < bestSqr)
                    {
                        bestSqr = sqr;
                        cell = candidate;
                        found = true;
                    }
                }
            }

            return found;
        }

        private static bool IsFunctionalPlayer(GameObject candidate)
        {
            return candidate != null
                && candidate.CompareTag("Player")
                && candidate.GetComponent<PlayerController>() != null
                && candidate.GetComponent<PlayerAttack>() != null
                && candidate.GetComponent<Health>() != null;
        }

        private static void EnsurePlayerRuntime(GameObject playerObject)
        {
            playerObject.tag = "Player";
            int playerLayer = LayerMask.NameToLayer("Player");
            if (playerLayer >= 0)
            {
                playerObject.layer = playerLayer;
            }

            Rigidbody2D body = AddIfMissing<Rigidbody2D>(playerObject);
            body.gravityScale = 0f;
            body.freezeRotation = true;
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            if (playerObject.GetComponent<Collider2D>() == null)
            {
                CapsuleCollider2D collider = playerObject.AddComponent<CapsuleCollider2D>();
                collider.offset = new Vector2(0f, 0.15f);
                collider.size = new Vector2(0.46f, 0.34f);
            }

            AddIfMissing<Health>(playerObject);
            AddIfMissing<InputBufferService>(playerObject);
            AddIfMissing<SkillFlowTracker>(playerObject);
            AddIfMissing<PlayerController>(playerObject);
            AddIfMissing<RageSystem>(playerObject);
            AddIfMissing<PlayerAttack>(playerObject);
        }

        private static void EnsurePrimaryClass()
        {
            if (PlayerClassManager.SelectedClass == ClassType.None)
            {
                PlayerClassManager.SelectedClass = ClassType.Warblade;
            }

            PlayerClassManager manager = PlayerClassManager.Instance;
            if (manager == null)
            {
                GameObject managerObject = new GameObject("PlayerClassManager");
                manager = managerObject.AddComponent<PlayerClassManager>();
            }

            manager.SetPrimaryClass(PlayerClassManager.SelectedClass);
        }

        private void StartRoomEncounter()
        {
            // ── DEMO MERCHANT PATH ───────────────────────────────────────────────
            // Check Merchant FIRST — before the enemy-socket guard — because merchant
            // templates intentionally have zero enemy sockets. If we let the guard run
            // first it would call HandleEncounterCleared() and skip the shop entirely.
            if (CurrentRoomType == RIMA.RoomType.Merchant)
            {
                HandleMerchantRoom();
                return;
            }

            if (CurrentTemplate == null || CurrentTemplate.enemySpawnSockets == null || CurrentTemplate.enemySpawnSockets.Count == 0)
            {
                HandleEncounterCleared();
                return;
            }

            IReadOnlyList<Transform> markerList = builder.EnemySpawnMarkers;
            if (markerList == null || markerList.Count == 0)
            {
                Debug.LogWarning("[RoomRunDirector] Missing enemy spawn markers; treating room as cleared.");
                HandleEncounterCleared();
                return;
            }

            // ── DEMO BOSS PATH ────────────────────────────────────────────────────
            // When the current node is a Boss node, bypass the EncounterController wave
            // system and spawn PenitentSovereign directly. Health.OnDeath hooks back to
            // HandleEncounterCleared so the victory/clear sequence fires normally.
            if (CurrentRoomType == RIMA.RoomType.Boss)
            {
                SpawnBossDirectly(markerList[0]);
                return;
            }

            Transform[] spawnPoints = new Transform[markerList.Count];
            for (int i = 0; i < markerList.Count; i++)
            {
                spawnPoints[i] = markerList[i];
            }

            EnsureEnemyContainer();
            EnsureEncounterController();
            bool eliteRoom = CurrentRoomType == RIMA.RoomType.Elite || CurrentRoomType == RIMA.RoomType.Boss;
            EncounterWaveSO wave = ResolveEncounterWave(spawnPoints.Length, eliteRoom);
            if (wave == null)
            {
                HandleEncounterCleared();
                return;
            }

            encounterController.OnRoomCleared.RemoveListener(HandleEncounterCleared);
            encounterController.OnRoomCleared.AddListener(HandleEncounterCleared);
            encounterController.BeginEncounter(wave, spawnPoints, DifficultyForCurrentRoom(), runSeed + CurrentNodeId, eliteRoom);
            ReparentEnemyContainerChildren();
        }

        /// <summary>
        /// Demo-path boss spawn: instantiates PenitentSovereign at the first enemy socket
        /// and wires Health.OnDeath → HandleEncounterCleared so the run's clear/victory
        /// sequence fires when the boss dies — without touching legacy RuntimeRoomManager.
        /// </summary>
        private void SpawnBossDirectly(Transform spawnMarker)
        {
            GameObject prefab = ResolveBossPrefab();
            if (prefab == null)
            {
                Debug.LogWarning("[RoomRunDirector] Boss prefab not found; treating Boss room as cleared.");
                HandleEncounterCleared();
                return;
            }

            Vector3 spawnPos = spawnMarker != null ? spawnMarker.position : transform.position;
            EnsureEnemyContainer();
            GameObject bossInstance = Instantiate(prefab, spawnPos, Quaternion.identity);
            bossInstance.name = "PenitentSovereign_Boss";
            bossInstance.tag = "Enemy";
            AlignBossFeetToArena(bossInstance);
            if (enemyContainer != null)
                bossInstance.transform.SetParent(enemyContainer, true);
            activeEnemies.Add(bossInstance);

            // Hook Health.OnDeath → HandleEncounterCleared (single-fire: boss is the only enemy).
            Health bossHealth = bossInstance.GetComponent<Health>();
            if (bossHealth != null)
            {
                bossHealth.OnDeath.AddListener(HandleEncounterCleared);
            }
            else
            {
                Debug.LogWarning("[RoomRunDirector] PenitentSovereign prefab has no Health component — boss death will not trigger room clear.");
            }

            // Also ensure BossHealthBar exists in scene (PenitentSovereign.Start() uses FindAnyObjectByType).
            EnsureBossHealthBar();

            Debug.Log($"[RoomRunDirector] Boss spawned: {bossInstance.name} at {spawnPos} (node={CurrentNodeId})");
        }

        /// <summary>
        /// Merchant room: spawn 3 shop stands at the room centre, open exit doors immediately.
        /// No EncounterController wave is started. Lifecycle is force-advanced to DoorOpen so
        /// the player is never locked in — buying is optional.
        /// </summary>
        private void HandleMerchantRoom()
        {
            // Spawn ShopRoomController (self-contained; owns stands + cleanup on Destroy).
            Vector3 center = ResolveRoomCenter();
            DestroyActiveShop();
            GameObject shopGO = new GameObject("ShopRoomController");
            ShopRoomController shopController = shopGO.AddComponent<ShopRoomController>();
            activeShopController = shopController;
            shopController.Setup(center);

            // Advance lifecycle from Combat → DoorOpen so exit doors open immediately.
            // lifecycle is already in Combat (set in BuildCurrentRoom before StartRoomEncounter).
            lifecycle.MarkCleared();
            lifecycle.MarkRewardTaken();
            // MarkDoorsOpened requires RewardTaken state — we are there now.
            if (!lifecycle.MarkDoorsOpened())
            {
                Debug.LogWarning($"[RoomRunDirector] Merchant: MarkDoorsOpened failed at state={lifecycle.State}; using force-open.");
                ForceOpenExitDoorsFromAnyClearedState();
            }
            else
            {
                EnsureAtLeastOneExitDoor();
                ConfigureExitDoors(true);
            }

            Debug.Log($"[RoomRunDirector] Merchant room set up: 3 stands at {center}, exit doors open. node={CurrentNodeId}");
        }

        private GameObject ResolveBossPrefab()
        {
            if (bossPrefab != null)
                return bossPrefab;

#if UNITY_EDITOR
            bossPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(DefaultBossPrefabEditorPath);
#endif
            if (bossPrefab == null)
                bossPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Boss/PenitentSovereign");

            return bossPrefab;
        }

        private static void EnsureBossHealthBar()
        {
            if (UnityEngine.Object.FindAnyObjectByType<BossHealthBar>() != null)
                return;
            new GameObject("BossHealthBar_Auto").AddComponent<BossHealthBar>();
        }

        private void EnsureEnemyContainer()
        {
            if (enemyContainer != null)
            {
                return;
            }

            GameObject container = new GameObject("Enemies");
            enemyContainer = container.transform;
        }

        private void EnsureEncounterController()
        {
            if (encounterController == null)
            {
                encounterController = GetComponent<EncounterController>();
            }

            if (encounterController == null)
            {
                encounterController = gameObject.AddComponent<EncounterController>();
            }
        }

        private EncounterWaveSO ResolveEncounterWave(int spawnPointCount, bool eliteRoom)
        {
            EncounterWaveSO wave = encounterBank != null ? encounterBank.PickWave(DifficultyForCurrentRoom(), runSeed + CurrentNodeId) : null;
            if (HasSpawnableEntries(wave))
            {
                return wave;
            }

            if (encounterBank == null && !warnedMissingEncounterBank)
            {
                Debug.LogWarning($"[RoomRunDirector] Missing encounterBank and Resources.Load<EncounterBankSO>(\"{DefaultEncounterBankPath}\") failed; using default combat fallback.");
                warnedMissingEncounterBank = true;
            }
            else if (wave != null)
            {
                Debug.LogWarning("[RoomRunDirector] Encounter wave has no spawnable entries; using default combat fallback.");
            }

            return CreateDefaultCombatFallbackWave(spawnPointCount, eliteRoom);
        }

        private float DifficultyForCurrentRoom()
        {
            if (CurrentRoomType == RIMA.RoomType.Boss)
            {
                return Mathf.Max(encounterDifficulty + 6f, encounterDifficulty * 1.6f);
            }

            if (CurrentRoomType == RIMA.RoomType.Elite)
            {
                return Mathf.Max(encounterDifficulty + 3f, encounterDifficulty * 1.3f);
            }

            return encounterDifficulty;
        }

        private static bool HasSpawnableEntries(EncounterWaveSO wave)
        {
            if (wave == null || wave.entries == null)
            {
                return false;
            }

            for (int i = 0; i < wave.entries.Count; i++)
            {
                EncounterEnemyEntry entry = wave.entries[i];
                if (entry != null && entry.prefab != null && entry.count > 0 && entry.threatCost > 0f)
                {
                    return true;
                }
            }

            return false;
        }

        private EncounterWaveSO CreateDefaultCombatFallbackWave(int spawnPointCount, bool eliteRoom)
        {
            GameObject prefab = ResolveDefaultEnemyPrefab();
            if (prefab == null)
            {
                Debug.LogWarning("[RoomRunDirector] Default combat fallback has no enemy prefab; treating room as cleared.");
                return null;
            }

            int count = Mathf.Max(1, spawnPointCount);
            EncounterWaveSO wave = ScriptableObject.CreateInstance<EncounterWaveSO>();
            wave.hideFlags = HideFlags.HideAndDontSave;
            wave.threatBudget = Mathf.Max(1f, count);
            wave.openingBudgetFraction = count > 1 ? 0.6f : 1f;
            wave.nextWaveKillFraction = 0.5f;
            wave.normalRoomT2Cap = 0;
            wave.eliteRoomT2Cap = eliteRoom ? 1 : 0;
            wave.entries.Add(new EncounterEnemyEntry
            {
                enemyType = EncounterEnemyType.FractureImp,
                prefab = prefab,
                count = count,
                threatCost = 1f,
                weight = 1f,
                maxSimultaneous = Mathf.Max(1, count),
                eliteOnly = false,
                t2Capable = false
            });

            return wave;
        }

        private GameObject ResolveDefaultEnemyPrefab()
        {
            if (defaultEnemyPrefab != null)
            {
                return defaultEnemyPrefab;
            }

#if UNITY_EDITOR
            defaultEnemyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(DefaultEnemyPrefabEditorPath);
#endif
            return defaultEnemyPrefab;
        }

        private void ReparentEnemyContainerChildren()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemies.Length; i++)
            {
                GameObject enemy = enemies[i];
                if (enemy == null || enemy.transform.IsChildOf(enemyContainer))
                {
                    continue;
                }

                if (enemy.GetComponent<Health>() != null)
                {
                    enemy.transform.SetParent(enemyContainer, true);
                    activeEnemies.Add(enemy);
                }
            }
        }

        private void ResolveRuntimeReferences()
        {
            if (arenaCamera == null)
            {
                arenaCamera = Camera.main;
            }

            if (playerPrefab == null)
            {
                playerPrefab = Resources.Load<GameObject>(DefaultPlayerPrefabPath);
            }

            if (encounterBank == null)
            {
                encounterBank = Resources.Load<EncounterBankSO>(DefaultEncounterBankPath);
            }

            if (rewardSprite == null)
            {
                rewardSprite = LoadRewardChestSprite();
            }

            EnsureEnemyContainer();
        }

        private void ClearActiveEnemies()
        {
            if (encounterController != null)
            {
                encounterController.OnRoomCleared.RemoveListener(HandleEncounterCleared);
            }

            for (int i = activeEnemies.Count - 1; i >= 0; i--)
            {
                if (activeEnemies[i] != null)
                {
                    DestroyRuntimeObject(activeEnemies[i]);
                }
            }

            activeEnemies.Clear();

            if (enemyContainer != null)
            {
                for (int i = enemyContainer.childCount - 1; i >= 0; i--)
                {
                    DestroyRuntimeObject(enemyContainer.GetChild(i).gameObject);
                }
            }
        }

        private void DestroyActiveShop()
        {
            if (activeShopController == null)
            {
                return;
            }

            activeShopController.Cleanup();
            activeShopController.gameObject.SetActive(false);
            DestroyRuntimeObject(activeShopController.gameObject);
            activeShopController = null;
        }

        private void AlignBossFeetToArena(GameObject bossInstance)
        {
            if (bossInstance == null || builder == null || !builder.TryGetLastFloorWorldBounds(out Bounds floorBounds))
            {
                return;
            }

            SpriteRenderer spriteRenderer = bossInstance.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                return;
            }

            bossInstance.transform.localScale = Vector3.one;
            AlignSpriteBottomToColliderBase(bossInstance, spriteRenderer);

            Bounds bossBounds = spriteRenderer.bounds;
            float x = Mathf.Clamp(
                bossInstance.transform.position.x,
                floorBounds.min.x + bossBounds.extents.x,
                floorBounds.max.x - bossBounds.extents.x);

            float targetBottom = Mathf.Lerp(floorBounds.min.y, floorBounds.center.y, 0.28f);
            float y = bossInstance.transform.position.y + (targetBottom - bossBounds.min.y);
            bossInstance.transform.position = new Vector3(x, y, bossInstance.transform.position.z);
        }

        private static void AlignSpriteBottomToColliderBase(GameObject bossInstance, SpriteRenderer spriteRenderer)
        {
            Collider2D bodyCollider = bossInstance.GetComponent<Collider2D>();
            if (bodyCollider == null)
            {
                return;
            }

            float deltaY = bodyCollider.bounds.min.y - spriteRenderer.bounds.min.y;
            Transform spriteTransform = spriteRenderer.transform;
            spriteTransform.position = new Vector3(
                spriteTransform.position.x,
                spriteTransform.position.y + deltaY,
                spriteTransform.position.z);
        }

        private void HandleEncounterCleared()
        {
            if (!lifecycle.MarkCleared())
            {
                return;
            }

            RoomCleared?.Invoke();
            RIMA.Audio.AudioManager.Play(RIMA.Audio.Sfx.RoomClear);
            Debug.Log($"[RoomRunDirector] RoomCleared node={CurrentNodeId} type={CurrentRoomType}");

            if (clearSequence != null)
            {
                StopCoroutine(clearSequence);
            }

            clearSequence = StartCoroutine(RoomClearSequence());

            // FIX-1 (B1) DECOUPLE + safety-net: independent watchdog so the exit doors and
            // timeScale recover even if RoomClearSequence throws or stalls. Started here (on
            // Cleared) so it is sarsılmaz — it does not depend on the reward chain succeeding.
            if (reconcileSequence != null)
            {
                StopCoroutine(reconcileSequence);
            }

            reconcileSequence = StartCoroutine(RoomClearReconcile());
        }

        // FIX-1 (B1) safety-net window: how long (real seconds) after Cleared the watchdog waits
        // before force-opening doors / restoring timeScale if the normal chain hasn't already.
        // Covers the boss class-select draft (which legitimately keeps the room un-doored longer)
        // by skipping the force while a draft is pending/active.
        private const float RoomClearReconcileTimeoutSec = 3f;

        /// <summary>
        /// FIX-1 (B1): reconcile watchdog. Runs alongside RoomClearSequence and guarantees the
        /// room never soft-locks: after RoomClearReconcileTimeoutSec real seconds in the Cleared
        /// state with no reward AND no door, it forces the doors open and restores timeScale.
        /// Door-opening is decoupled from reward-spawning (Binding of Isaac pattern: door-open is
        /// logical/instant, it must not wait on reward or draft animation). It defers while a
        /// draft (opening kit / boss class-select / reward draft) is legitimately pending/active,
        /// and never fires if the run is already complete.
        /// </summary>
        private System.Collections.IEnumerator RoomClearReconcile()
        {
            float elapsed = 0f;

            // Stay alive while the room is still resolving its clear (Cleared/RewardTaken).
            while (lifecycle.State == RoomRunLifecycleState.Cleared
                   || lifecycle.State == RoomRunLifecycleState.RewardTaken)
            {
                // Doors are already open (DoorOpen reached via the normal path) — nothing to do.
                if (lifecycle.State == RoomRunLifecycleState.DoorOpen)
                {
                    break;
                }

                // A draft is legitimately holding the flow (opening kit, boss class-select, or the
                // reward draft the player is actively picking). Pause the timer — do not force.
                bool draftBlocking = DraftManager.Instance != null
                    && (DraftManager.Instance.IsDraftPending || DraftManager.Instance.IsDraftActive);

                // The player has a reward sitting in the room they haven't collected yet — that is
                // an intentional wait (RewardAutoCollectTimeoutSec=0 means it never disappears), so
                // don't force doors based on the reward, but DO keep the timeScale guard below.
                bool rewardWaiting = activeReward != null && !activeReward.WasCollected;

                if (!draftBlocking && !rewardWaiting)
                {
                    elapsed += Time.unscaledDeltaTime;
                    if (elapsed >= RoomClearReconcileTimeoutSec && !IsRunComplete)
                    {
                        Debug.LogWarning($"[RoomRunDirector] RoomClearReconcile: clear flow stalled after {RoomClearReconcileTimeoutSec}s (state={lifecycle.State}, reward={(activeReward != null)}). Forcing exit doors + timeScale recovery (node={CurrentNodeId}).");
                        ForceOpenExitDoorsFromAnyClearedState();
                        RestoreGameplayTimeScale();
                        break;
                    }
                }
                else
                {
                    // Reset the stall timer whenever a legitimate wait is in progress so the player
                    // gets the full window after the draft closes / reward is taken.
                    elapsed = 0f;
                }

                // Hard timeScale guard regardless of draft/reward: a slow-mo blip should never be
                // stuck for multiple seconds during an active clear.
                if (elapsed >= RoomClearReconcileTimeoutSec
                    && Time.timeScale > 0f && Time.timeScale < 1f)
                {
                    RestoreGameplayTimeScale();
                }

                yield return null;
            }

            reconcileSequence = null;
        }

        // 0 = NO auto-collect timeout (user 2026-06-13: an uncollected reward must NOT disappear or
        // auto-grant). The collect loop guards on `> 0f`, so 0 makes the reward persist until the
        // player takes it with G; the exit door simply waits for the pickup. (Was 90s ForceCollect.)
        private const float RewardAutoCollectTimeoutSec = 0f;

        // How many real-time seconds to wait for a draft UI to close before forcing it away.
        private const float DraftAutoCloseTimeoutSec = 90f;

        private System.Collections.IEnumerator RoomClearSequence()
        {
            try
            {
                yield return ClearSlowMoBlip();

                // K3.2 — P2: "ODA TEMİZLENDİ" flash immediately after slow-mo blip.
                HUDController.Instance?.SetRoomStatus("ODA TEMİZLENDİ");

                // ── Dual-class gate (Boss room clear) ─────────────────────────────────
                // Fires when the Boss room is cleared. Boss is no longer the terminal node:
                // after the player picks a secondary class and the unlock draft closes, we
                // fall through to the normal reward/door flow so the exit door opens to the
                // post-boss Combat room where the combined kit is actually PLAYED.
                if (CurrentRoomType == RIMA.RoomType.Boss
                    && PlayerClassManager.Instance != null
                    && PlayerClassManager.Instance.SecondaryClass == ClassType.None)
                {
                    PlayerClassManager.Instance.TriggerClassSelection();

                    // WaitUntil works at timeScale=0 (uses real-time under the hood for the check).
                    yield return new WaitUntil(() =>
                        PlayerClassManager.Instance == null ||
                        PlayerClassManager.Instance.SecondaryClass != ClassType.None);

                    // Wait for the unlock draft to fully complete before opening the exit door.
                    // IsDraftPending covers the 2 s ShowDraftDelayed window (race condition fix):
                    // secondary class is selected → IsDraftPending=true → 2 s later ShowDraft()
                    // runs → IsDraftPending=false, IsDraftActive=true → player picks → IsDraftActive=false.
                    // Without IsDraftPending the old WaitWhile(IsDraftActive) would pass immediately
                    // because IsDraftActive is still false during the delay, opening the door too early.
                    // Timeout mirrors DraftAutoCloseTimeoutSec so this wait can never softlock the run.
                    {
                        float unlockDraftTimer = 0f;
                        while (DraftManager.Instance != null &&
                               (DraftManager.Instance.IsDraftPending || DraftManager.Instance.IsDraftActive))
                        {
                            unlockDraftTimer += Time.unscaledDeltaTime;
                            if (unlockDraftTimer >= DraftAutoCloseTimeoutSec)
                            {
                                Debug.LogWarning($"[RoomRunDirector] Unlock draft not closed after {DraftAutoCloseTimeoutSec}s — force-closing to unblock run (node={CurrentNodeId}).");
                                DraftManager.Instance.HideDraft();
                                break;
                            }
                            yield return null;
                        }
                    }
                }

                // ── Run-complete check (post-boss Combat terminal node) ────────────────
                if (IsRunComplete)
                {
                    RestoreGameplayTimeScale();
                    lifecycle.MarkVictory();
                    DemoCompleteOverlay.Show();
                    clearSequence = null;
                    yield break;
                }

                // K3.2 — P2: 0.5 s pause before reward appears (gives slow-mo moment to breathe).
                yield return new WaitForSecondsRealtime(0.5f);

                activeReward = SpawnRewardPickup();
                if (activeReward == null)
                {
                    lifecycle.MarkRewardTaken();
                    OpenExitDoors();
                    clearSequence = null;
                    yield break;
                }

                // K3.2 — P2: reward spawn choreography — scale-pop + EchoPuffBurst + bob.
                StartCoroutine(RewardSpawnPop(activeReward));


                // BUG-1 FIX: wait for player to collect the reward, but enforce a hard
                // timeout. On timeout: GRANT the reward (ForceCollect) instead of silently
                // discarding it — the player always gets their skill pick even if they missed
                // the visual pickup. Doors open only after the draft resolves.
                float rewardTimer = 0f;
                while (activeReward != null && !activeReward.WasCollected)
                {
                    rewardTimer += Time.unscaledDeltaTime;
                    if (RewardAutoCollectTimeoutSec > 0f && rewardTimer >= RewardAutoCollectTimeoutSec)
                    {
                        Debug.LogWarning($"[RoomRunDirector] Reward not collected after {RewardAutoCollectTimeoutSec}s — auto-granting to player (node={CurrentNodeId}).");
                        // ForceCollect triggers the draft and marks WasCollected; the while-loop
                        // then exits naturally on the next frame. Do NOT destroy the reward here —
                        // ForceCollect/Collect does so after the draft closes.
                        if (activeReward != null)
                            activeReward.ForceCollect();
                        break;
                    }
                    yield return null;
                }

                // Wait for WasCollected (ForceCollect sets it synchronously) so the draft has
                // been initiated. The existing draft-wait below handles the rest.
                while (activeReward != null && !activeReward.WasCollected)
                    yield return null;

                // ROOT FIX (Bug 2): MarkRewardTaken() can only fail if lifecycle isn't in
                // Cleared state — that shouldn't happen here, but if it does (e.g. AdvanceTo
                // was called concurrently), log it and bail safely rather than silently stalling.
                if (!lifecycle.MarkRewardTaken())
                {
                    Debug.LogWarning($"[RoomRunDirector] MarkRewardTaken failed at node={CurrentNodeId} state={lifecycle.State} — skipping to door-open fallback.");
                    ForceOpenExitDoorsFromAnyClearedState();
                    clearSequence = null;
                    yield break;
                }

                // ROOT FIX (Bug 3): draft wait now also has a hard timeout so a never-closing
                // draft UI can't permanently stall the exit doors.
                DraftManager draft = DraftManager.Instance;
                float draftTimer = 0f;
                while (draft != null && draft.IsDraftActive)
                {
                    draftTimer += Time.unscaledDeltaTime;
                    if (draftTimer >= DraftAutoCloseTimeoutSec)
                    {
                        Debug.LogWarning($"[RoomRunDirector] Draft still active after {DraftAutoCloseTimeoutSec}s — force-closing to unblock run (node={CurrentNodeId}).");
                        draft.HideDraft();
                        break;
                    }
                    yield return null;
                }

                OpenExitDoors();
                clearSequence = null;
            }
            finally
            {
                RestoreGameplayTimeScale();
            }
        }

        private System.Collections.IEnumerator ClearSlowMoBlip()
        {
            if (slowMoSequence != null || Time.timeScale <= 0f)
            {
                yield break;
            }

            float restoreScale = Time.timeScale;
            float lastSetScale = Mathf.Clamp(clearSlowMoScale, 0.05f, restoreScale);
            Time.timeScale = lastSetScale;
            float elapsed = 0f;

            while (elapsed < clearSlowMoReturnDuration)
            {
                yield return null;
                if (Mathf.Abs(Time.timeScale - lastSetScale) > 0.02f)
                {
                    yield break;
                }

                elapsed += Time.unscaledDeltaTime;
                float t = clearSlowMoReturnDuration > 0f ? Mathf.Clamp01(elapsed / clearSlowMoReturnDuration) : 1f;
                lastSetScale = Mathf.Lerp(clearSlowMoScale, restoreScale, t);
                Time.timeScale = lastSetScale;
            }

            if (Mathf.Abs(Time.timeScale - lastSetScale) <= 0.02f)
            {
                Time.timeScale = restoreScale;
            }
        }

        // Loads the 62x72 chest icon from the multi-sprite sheet by sub-name (Resources.Load returns
        // only the first slice, so LoadAll + name match is required). Returns null if not found.
        private static Sprite LoadRewardChestSprite()
        {
            Sprite[] sheet = Resources.LoadAll<Sprite>(DefaultRewardSpritePath);
            return sheet != null ? System.Array.Find(sheet, s => s != null && s.name == RewardSpriteSubName) : null;
        }

        private RewardPickup SpawnRewardPickup()
        {
            EnsureDraftManager();

            GameObject rewardObject = new GameObject("RewardPickup");
            rewardObject.transform.position = ResolveRewardSpawnPosition();

            SpriteRenderer spriteRenderer = rewardObject.AddComponent<SpriteRenderer>();

            // BUG-4 (2026-06-10): rewardSprite is null in _Arena (fileID=0). The old fallback
            // created a 1×1 pixel at PPU=16 (world size ≈0.034) — invisible on screen. Load the
            // chest icon from Resources first; only fall back to the programmatic sprite if that
            // also fails. Leave sprite = null when all sources fail so RewardPickup.Awake() can
            // load the rift shard as its own fallback.
            if (rewardSprite != null)
            {
                spriteRenderer.sprite = rewardSprite;
            }
            else
            {
                Sprite loaded = LoadRewardChestSprite();
                if (loaded != null)
                    spriteRenderer.sprite = loaded;
                // else leave null — RewardPickup.Awake() will load the rift-shard fallback
            }

            spriteRenderer.color = Color.white;
            spriteRenderer.sortingLayerName = "Entities";
            spriteRenderer.sortingOrder = 5;

            // K3.1 (DEMO_DESIGN_PLAN): IsoSorter for correct depth-sorting (baseOrder=+5 = in front
            // of same-tile props/cliffs). Proje kuralı: "dinamik/yerde-duran her sprite IsoSorter taşır".
            var isoSorter = rewardObject.AddComponent<IsoSorter>();
            // Set baseOrder via field reflection (serialized) so it applies from first LateUpdate.
            var baseOrderField = typeof(IsoSorter).GetField("baseOrder",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (baseOrderField != null) baseOrderField.SetValue(isoSorter, 5);

            // K3.2 (P2): idle bob so the reward reads as "alive / pick me up".
            rewardObject.AddComponent<PlaceholderFloat>();

            CircleCollider2D collider = rewardObject.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = Mathf.Max(0.05f, rewardColliderRadius);

            RewardPickup pickup = rewardObject.AddComponent<RewardPickup>();
            Debug.Log($"[RoomRunDirector] RewardPickup spawned at {rewardObject.transform.position}");
            return pickup;
        }

        /// <summary>
        /// K3.2 (P2): scale-pop (0→1.15→1.0 in 0.25s) + EchoPuffBurst cyan + light shake.
        /// Runs in parallel with the main clear-sequence; safe if the reward is destroyed early.
        /// </summary>
        private System.Collections.IEnumerator RewardSpawnPop(RewardPickup reward)
        {
            if (reward == null) yield break;
            Transform tf = reward.transform;

            // Start scaled to 0 so the pop reads as "appearing from nothing".
            tf.localScale = Vector3.zero;

            // EchoPuffBurst cyan materialize puff.
            EchoPuffBurst.Spawn(tf.position, 0.5f, dissolve: false, moteCount: 9);

            // Light screen shake (null-safe: ScreenShakeDriver may not be present in all scenes).
            RIMA.Combat.ScreenShakeDriver.Instance?.Shake(0.08f, 0.2f);

            // BUG-2 FIX: end at reward.VisualScale (1.1) instead of always 1.0.
            // Scale-pop: 0 → 1.15×base → 1.0×base over 0.25 s.
            float baseScale = reward != null ? reward.VisualScale : 1f;
            const float popTime = 0.25f;
            float t = 0f;
            while (t < popTime && reward != null)
            {
                t += Time.unscaledDeltaTime;
                float progress = t / popTime;
                // Ease-out overshoot curve: grows past 1.15×base then settles at base.
                float scale = progress < 0.7f
                    ? Mathf.Lerp(0f, 1.15f * baseScale, progress / 0.7f)
                    : Mathf.Lerp(1.15f * baseScale, baseScale, (progress - 0.7f) / 0.3f);
                if (tf != null) tf.localScale = Vector3.one * scale;
                yield return null;
            }
            if (tf != null) tf.localScale = Vector3.one * baseScale;
        }

        /// <summary>
        /// BUG-3 (2026-06-10): the raw floor-bounds centre can be a non-walkable cell (donut hole,
        /// cliff void) — the reward then sits where the player can't reach it and the 12 s timeout
        /// silently auto-collects it ("reward never dropped"). Prefer the nearest walkable cell
        /// (with 3x3 clearance) to the room centre; fall back to the old centre with a warning.
        /// </summary>
        private Vector3 ResolveRewardSpawnPosition()
        {
            if (CurrentTemplate != null && builder != null)
            {
                Vector2Int centerCell = new Vector2Int(
                    Mathf.RoundToInt(CurrentTemplate.bounds.center.x),
                    Mathf.RoundToInt(CurrentTemplate.bounds.center.y));

                if (TryFindNearestClearanceCell(CurrentTemplate, centerCell, out Vector2Int cell)
                    && builder.TryGetCellCenterWorld(cell, out Vector3 world))
                {
                    return world;
                }

                Debug.LogWarning($"[RoomRunDirector] Reward spawn: no walkable cell with clearance near centre of '{CurrentTemplate.roomId}' — using raw room centre fallback (node={CurrentNodeId}).");
            }

            return ResolveRoomCenter();
        }

        private Vector3 ResolveRoomCenter()
        {
            if (builder != null && builder.TryGetLastFloorWorldBounds(out Bounds bounds))
            {
                return new Vector3(bounds.center.x, bounds.center.y, 0f);
            }

            return player != null ? player.position : transform.position;
        }

        private static Sprite CreateFallbackRewardSprite()
        {
            Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture.hideFlags = HideFlags.HideAndDontSave;
            texture.SetPixel(0, 0, new Color(0.1f, 1f, 0.85f, 1f));
            texture.Apply();
            Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 16f);
            sprite.hideFlags = HideFlags.HideAndDontSave;
            return sprite;
        }

        private static void EnsureDraftManager()
        {
            if (DraftManager.Instance != null || FindObjectOfType<DraftManager>() != null)
            {
                return;
            }

            new GameObject("DraftManager_Auto").AddComponent<DraftManager>();
        }

        private static void EnsureDeathScreenManager()
        {
            if (FindObjectOfType<DeathScreenManager>() != null)
            {
                return;
            }

            new GameObject("DeathScreenManager_Auto").AddComponent<DeathScreenManager>();
        }

        /// <summary>
        /// Ensures HUDController (HP bar, resource bar, interaction prompt) and SkillBarUI
        /// exist in the scene. Both are built programmatically via their own BuildHUD / BuildSlots
        /// methods, so we only need to create the Canvas host and attach the components.
        /// Called once per BeginRun after the player is placed.
        /// </summary>
        private static void EnsureHUD()
        {
            // EventSystem required for UI input (drag-drop, button clicks).
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                var esGo = new GameObject("EventSystem");
                esGo.AddComponent<UnityEngine.EventSystems.EventSystem>();
                esGo.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
                Debug.Log("[RoomRunDirector] EventSystem auto-created.");
            }

            // HUDController ─ top-level ScreenSpaceOverlay Canvas
            if (HUDController.Instance == null && FindObjectOfType<HUDController>() == null)
            {
                var canvasGo = new GameObject("HUD_Canvas");
                var canvas = canvasGo.AddComponent<UnityEngine.Canvas>();
                canvas.renderMode = UnityEngine.RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 10;
                canvasGo.AddComponent<UnityEngine.UI.CanvasScaler>();
                canvasGo.AddComponent<UnityEngine.UI.GraphicRaycaster>();

                // HUDController sits directly on the Canvas root so its RectTransform is the root
                canvasGo.AddComponent<HUDController>();
                Debug.Log("[RoomRunDirector] HUD_Canvas + HUDController auto-created.");
            }

            // SkillBarUI — needs its own Canvas (or child) with a RectTransform. Place it on a
            // dedicated child so anchoring is independent of the HUD bars.
            if (FindObjectOfType<SkillBarUI>() == null)
            {
                HUDController hud = HUDController.Instance ?? FindObjectOfType<HUDController>();
                UnityEngine.Canvas parentCanvas = hud != null
                    ? hud.GetComponent<UnityEngine.Canvas>() ?? hud.GetComponentInParent<UnityEngine.Canvas>()
                    : null;

                if (parentCanvas == null)
                {
                    // Fallback: find any ScreenSpaceOverlay canvas
                    foreach (var cv in FindObjectsOfType<UnityEngine.Canvas>())
                    {
                        if (cv.renderMode == UnityEngine.RenderMode.ScreenSpaceOverlay) { parentCanvas = cv; break; }
                    }
                }

                if (parentCanvas != null)
                {
                    var barGo = new GameObject("SkillBar", typeof(UnityEngine.RectTransform));
                    barGo.transform.SetParent(parentCanvas.transform, false);
                    var rt = barGo.GetComponent<UnityEngine.RectTransform>();
                    // Bottom-center anchor
                    rt.anchorMin = new UnityEngine.Vector2(0.5f, 0f);
                    rt.anchorMax = new UnityEngine.Vector2(0.5f, 0f);
                    rt.pivot     = new UnityEngine.Vector2(0.5f, 0f);
                    rt.anchoredPosition = new UnityEngine.Vector2(0f, 16f);
                    rt.sizeDelta = new UnityEngine.Vector2(400f, 72f);
                    barGo.AddComponent<SkillBarUI>();
                    Debug.Log("[RoomRunDirector] SkillBar + SkillBarUI auto-created.");
                }
                else
                {
                    Debug.LogWarning("[RoomRunDirector] EnsureHUD: no ScreenSpaceOverlay canvas found for SkillBarUI.");
                }
            }
        }

        private void OpenExitDoors()
        {
            if (!lifecycle.MarkDoorsOpened())
            {
                // ROOT FIX (Bug 2 guard): MarkDoorsOpened only fails if state != RewardTaken.
                // Log the state so we can diagnose future regressions; then use the force-path
                // to avoid silently leaving the player stuck with no exit.
                Debug.LogWarning($"[RoomRunDirector] OpenExitDoors: MarkDoorsOpened failed at state={lifecycle.State} node={CurrentNodeId}. Attempting force-open.");
                ForceOpenExitDoorsFromAnyClearedState();
                return;
            }

            EnsureAtLeastOneExitDoor();
            ConfigureExitDoors(true);
            Debug.Log($"[RoomRunDirector] Exit doors opened count={activeDoors.Count} node={CurrentNodeId}");
        }

        /// <summary>
        /// Force-advances the lifecycle to DoorOpen (from any post-Combat state) and opens
        /// the exit doors. Used as a fallback whenever the normal state-machine path could not
        /// reach MarkDoorsOpened — guarantees the player always gets a working exit after a clear.
        /// </summary>
        public void ForceOpenExitDoorsFromAnyClearedState()
        {
            // Walk the lifecycle forward to RewardTaken if needed, then to DoorOpen.
            // Each Mark* method is a no-op if the state is already past it, so this is safe
            // to call from Cleared, RewardTaken, or even DoorOpen.
            if (lifecycle.State == RoomRunLifecycleState.Cleared)
            {
                lifecycle.MarkRewardTaken();
            }

            if (lifecycle.State == RoomRunLifecycleState.RewardTaken)
            {
                lifecycle.MarkDoorsOpened();
            }

            if (lifecycle.State != RoomRunLifecycleState.DoorOpen)
            {
                // Still failed — lifecycle is in an unexpected state (e.g. Advancing or Victory).
                // Do not open doors; just log and bail to avoid double-advancing the run.
                Debug.LogWarning($"[RoomRunDirector] ForceOpenExitDoorsFromAnyClearedState: cannot open doors from state={lifecycle.State} node={CurrentNodeId}. Run may be completing or already advanced.");
                return;
            }

            EnsureAtLeastOneExitDoor();
            ConfigureExitDoors(true);
            Debug.LogWarning($"[RoomRunDirector] Force-opened exit doors count={activeDoors.Count} node={CurrentNodeId}");
        }

        private void EnsureAtLeastOneExitDoor()
        {
            if (IsRunComplete)
            {
                return;
            }

            for (int i = activeDoors.Count - 1; i >= 0; i--)
            {
                if (activeDoors[i] == null)
                {
                    activeDoors.RemoveAt(i);
                }
            }

            if (activeDoors.Count > 0)
            {
                return;
            }

            List<RIMA.RoomType> doorTypes = new List<RIMA.RoomType>();
            List<DungeonNode> choices = CurrentChoices;
            for (int i = 0; i < choices.Count; i++)
            {
                doorTypes.Add(choices[i].roomType);
            }

            if (builder != null && doorTypes.Count > 0)
            {
                activeDoors.AddRange(builder.BuildExitDoors(doorTypes));
            }

            if (activeDoors.Count > 0)
            {
                return;
            }

            GameObject fallbackDoor = new GameObject("ExitDoor_Fallback_0");
            fallbackDoor.transform.position = ResolveRoomCenter() + Vector3.up;
            activeDoors.Add(fallbackDoor);
            Debug.LogWarning($"[RoomRunDirector] Created fallback exit door for node={CurrentNodeId} type={CurrentRoomType}.");
        }

        private void ConfigureExitDoors(bool open)
        {
            Color tint = open ? new Color(0f, 1f, 0.9f, 1f) : new Color(0.05f, 0.07f, 0.09f, 0.92f);
            for (int i = 0; i < activeDoors.Count; i++)
            {
                GameObject door = activeDoors[i];
                if (door == null)
                {
                    continue;
                }

                foreach (SpriteRenderer renderer in door.GetComponentsInChildren<SpriteRenderer>(true))
                {
                    renderer.color = tint;
                }

                BoxCollider2D collider = door.GetComponent<BoxCollider2D>();
                if (collider == null)
                {
                    collider = door.AddComponent<BoxCollider2D>();
                    // Gate trigger zone: modest footprint (was 1.2x1.0 = ~3x player, felt oversized).
                    // 0.8x0.7 still comfortably larger than the player (0.46x0.34) so walking into the
                    // open door reliably advances, without an oversized "near the door" auto-trigger.
                    collider.size = new Vector2(0.8f, 0.7f);
                }

                RoomRunExitDoorTrigger trigger = door.GetComponent<RoomRunExitDoorTrigger>();
                if (trigger == null)
                {
                    trigger = door.AddComponent<RoomRunExitDoorTrigger>();
                }

                trigger.Configure(this, i);
                trigger.SetOpen(open);
                RoomRunDoorLocatorPulse.SetDoorOpen(door, open);
            }
        }

        private void DestroyActiveReward()
        {
            if (activeReward != null)
            {
                DestroyRuntimeObject(activeReward.gameObject);
                activeReward = null;
            }
        }

        private void StopClearSequences()
        {
            if (clearSequence != null)
            {
                StopCoroutine(clearSequence);
                clearSequence = null;
            }

            if (slowMoSequence != null)
            {
                StopCoroutine(slowMoSequence);
                slowMoSequence = null;
            }

            if (openingDraftSequence != null)
            {
                StopCoroutine(openingDraftSequence);
                openingDraftSequence = null;
            }

            RestoreGameplayTimeScale();
        }

        private static void RestoreGameplayTimeScale()
        {
            if (Mathf.Abs(Time.timeScale - 1f) > 0.001f)
            {
                Time.timeScale = 1f;
            }
        }

        private static T AddIfMissing<T>(GameObject target) where T : Component
        {
            T existing = target.GetComponent<T>();
            return existing != null ? existing : target.AddComponent<T>();
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

        public void AdvanceTo(int choiceIndex)
        {
            if (IsRunComplete)
            {
                Debug.Log("[RoomRunDirector] run complete");
                return;
            }

            List<DungeonNode> choices = CurrentChoices;
            if (choiceIndex < 0 || choiceIndex >= choices.Count)
            {
                Debug.LogWarning($"[RoomRunDirector] Invalid choice index {choiceIndex}; choices={choices.Count}.");
                return;
            }

            CurrentNodeId = choices[choiceIndex].id;
            BuildCurrentRoom();
        }

        public void TryEnterDoor(int choiceIndex)
        {
            if (!lifecycle.MarkAdvancing())
            {
                return;
            }

            AdvanceTo(choiceIndex);
        }

        [ContextMenu("Advance First Choice")]
        private void DebugAdvance()
        {
            if (!IsRunComplete)
            {
                AdvanceTo(0);
            }
        }
    }

    [RequireComponent(typeof(Collider2D))]
    public sealed class RoomRunExitDoorTrigger : MonoBehaviour
    {
        private RoomRunDirector director;
        private int choiceIndex = -1;
        private bool isOpen;
        private bool playerInRange;
        private const Key InteractKey = Key.G;

        public void Configure(RoomRunDirector owner, int index)
        {
            director = owner;
            choiceIndex = index;
        }

        public void SetOpen(bool open)
        {
            isOpen = open;
            Collider2D trigger = GetComponent<Collider2D>();
            if (trigger != null)
            {
                trigger.isTrigger = true;
                trigger.enabled = open;
            }

            if (!open)
            {
                ClearPlayerRange();
            }
        }

        private void Update()
        {
            if (!isOpen || !playerInRange || director == null || choiceIndex < 0)
            {
                return;
            }

            if (Keyboard.current == null || !Keyboard.current[InteractKey].wasPressedThisFrame)
            {
                return;
            }

            ClearPlayerRange();
            director.TryEnterDoor(choiceIndex);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isOpen || director == null || choiceIndex < 0 || other == null || !other.CompareTag("Player"))
            {
                return;
            }

            playerInRange = true;
            RIMA.HUDController.Instance?.SetInteractionPrompt(RIMA.Loc.T("chamber_select.prompt.enter_rift"));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other == null || !other.CompareTag("Player"))
            {
                return;
            }

            ClearPlayerRange();
        }

        private void ClearPlayerRange()
        {
            if (!playerInRange)
            {
                return;
            }

            playerInRange = false;
            RIMA.HUDController.Instance?.HideInteractionPrompt();
        }
    }

    public sealed class RoomRunDoorLocatorPulse : MonoBehaviour
    {
        private const string LocatorName = "OpenDoorLocator";
        private const int SegmentCount = 40;
        private LineRenderer ring;
        private Color baseColor = new Color(0f, 1f, 0.9f, 0.72f);

        public static void SetDoorOpen(GameObject door, bool open)
        {
            if (door == null)
            {
                return;
            }

            RoomRunDoorLocatorPulse locator = door.GetComponentInChildren<RoomRunDoorLocatorPulse>(true);
            if (locator == null && open)
            {
                GameObject go = new GameObject(LocatorName);
                go.transform.SetParent(door.transform, false);
                go.transform.localPosition = new Vector3(0f, -0.28f, 0f);
                locator = go.AddComponent<RoomRunDoorLocatorPulse>();
            }

            if (locator != null)
            {
                locator.SetVisible(open);
            }
        }

        private void Awake()
        {
            EnsureRing();
        }

        private void Update()
        {
            if (ring == null || !ring.enabled)
            {
                return;
            }

            float pulse = 0.5f + 0.5f * Mathf.Sin(Time.time * 4.5f);
            float alpha = Mathf.Lerp(0.30f, 0.82f, pulse);
            Color color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            ring.startColor = color;
            ring.endColor = color;
            ring.startWidth = Mathf.Lerp(0.035f, 0.065f, pulse);
            ring.endWidth = ring.startWidth;
        }

        private void SetVisible(bool visible)
        {
            EnsureRing();
            if (ring != null)
            {
                ring.enabled = visible;
            }

            gameObject.SetActive(visible);
        }

        private void EnsureRing()
        {
            if (ring != null)
            {
                return;
            }

            ring = GetComponent<LineRenderer>();
            if (ring == null)
            {
                ring = gameObject.AddComponent<LineRenderer>();
            }

            ring.useWorldSpace = false;
            ring.loop = true;
            ring.positionCount = SegmentCount;
            ring.material = new Material(Shader.Find("Sprites/Default"));
            ring.sortingLayerName = "Floor";
            ring.sortingOrder = 80;
            ring.startColor = baseColor;
            ring.endColor = baseColor;

            const float radiusX = 0.72f;
            const float radiusY = 0.32f;
            for (int i = 0; i < SegmentCount; i++)
            {
                float angle = (Mathf.PI * 2f * i) / SegmentCount;
                ring.SetPosition(i, new Vector3(Mathf.Cos(angle) * radiusX, Mathf.Sin(angle) * radiusY, 0f));
            }
        }
    }
}
