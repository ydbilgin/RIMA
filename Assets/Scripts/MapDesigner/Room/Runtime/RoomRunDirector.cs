using System.Collections.Generic;
using RIMA.Encounter;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;
using UnityEngine.Events;

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
        [SerializeField] private Sprite rewardSprite;
        [SerializeField] private float rewardColliderRadius = 0.45f;
        [SerializeField] private float clearSlowMoScale = 0.3f;
        [SerializeField] private float clearSlowMoReturnDuration = 0.6f;
        [SerializeField] private RoomTemplateSO fallbackTemplate;
        [SerializeField] private int runSeed = 12345;
        [SerializeField] private bool buildOnStart = true;
        [SerializeField] private int depthCount = 5;

        private const string DefaultPlayerPrefabPath = "Prefabs/Warblade";
        private const string DefaultEncounterBankPath = "Encounters/Act1_EncounterBank_Pilot";
        private const string DefaultRewardSpritePath = "UI/RIMA/RIMA_UI_Node_Chest";
        private const string DefaultEnemyPrefabEditorPath = "Assets/Prefabs/Enemies/FractureImp.prefab";

        private bool warnedMissingPlayer;
        private bool warnedMissingEncounterBank;
        private bool warnedMissingArenaCamera;
        private bool warnedSpawnFallback;
        private DungeonGraph graph;
        private readonly List<GameObject> activeEnemies = new List<GameObject>();
        private readonly List<GameObject> activeDoors = new List<GameObject>();
        private readonly RoomRunLifecycle lifecycle = new RoomRunLifecycle();
        private RewardPickup activeReward;
        private Coroutine clearSequence;
        private Coroutine slowMoSequence;

        public DungeonGraph Graph => graph;
        public int CurrentNodeId { get; private set; }
        public DungeonNode CurrentNode => graph?.Get(CurrentNodeId);
        public RIMA.RoomType CurrentRoomType => CurrentNode != null ? CurrentNode.roomType : RIMA.RoomType.Combat;
        public List<DungeonNode> CurrentChoices => graph != null ? graph.ChildrenOf(CurrentNodeId) : new List<DungeonNode>();
        public bool IsRunComplete => CurrentNode == null || CurrentChoices.Count == 0;
        public RoomTemplateSO CurrentTemplate { get; private set; }
        public RoomRunLifecycleState LifecycleState => lifecycle.State;
        public UnityEvent RoomCleared = new UnityEvent();

        private void Start()
        {
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
            ClearActiveEnemies();
            graph = DungeonGraph.Generate(runSeed, depthCount);
            CurrentNodeId = graph.startId;
            BuildCurrentRoom();
        }

        public void BuildCurrentRoom()
        {
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
            ClearActiveEnemies();
            lifecycle.BeginCombat();

            RoomTemplateSO template = roomBank != null ? roomBank.Pick(CurrentRoomType, runSeed + CurrentNodeId) : null;
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
            builder.Build(template);
            FitCameraToRoom();

            EnsurePlayerAtSpawn();
            EnsureDeathScreenManager();

            Debug.Log($"[RoomRunDirector] Built node id={node.id} depth={node.depth} type={node.roomType} choices={CurrentChoices.Count} template={template.roomId}");

            // Exit doors = this node's branch choices (door count + each door's destination type).
            List<RIMA.RoomType> doorTypes = new List<RIMA.RoomType>();
            foreach (DungeonNode child in CurrentChoices)
            {
                doorTypes.Add(child.roomType);
            }
            activeDoors.Clear();
            activeDoors.AddRange(builder.BuildExitDoors(doorTypes));
            ConfigureExitDoors(false);

            StartRoomEncounter();
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

        private static bool TryFindBottomCenterWalkableCell(RoomTemplateSO template, out Vector2Int cell)
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

                    float distance = Mathf.Abs(x - centerX);
                    if (y < bestY || (y == bestY && distance < bestDistance))
                    {
                        cell = candidate;
                        bestY = y;
                        bestDistance = distance;
                    }
                }
            }

            return true;
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
                rewardSprite = Resources.Load<Sprite>(DefaultRewardSpritePath);
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

        private void HandleEncounterCleared()
        {
            if (!lifecycle.MarkCleared())
            {
                return;
            }

            RoomCleared?.Invoke();
            Debug.Log($"[RoomRunDirector] RoomCleared node={CurrentNodeId} type={CurrentRoomType}");

            if (clearSequence != null)
            {
                StopCoroutine(clearSequence);
            }

            clearSequence = StartCoroutine(RoomClearSequence());
        }

        private System.Collections.IEnumerator RoomClearSequence()
        {
            yield return ClearSlowMoBlip();

            if (IsRunComplete)
            {
                lifecycle.MarkVictory();
                DemoCompleteOverlay.Show();
                clearSequence = null;
                yield break;
            }

            activeReward = SpawnRewardPickup();
            if (activeReward == null)
            {
                lifecycle.MarkRewardTaken();
                OpenExitDoors();
                clearSequence = null;
                yield break;
            }

            while (activeReward != null && !activeReward.WasCollected)
            {
                yield return null;
            }

            if (!lifecycle.MarkRewardTaken())
            {
                clearSequence = null;
                yield break;
            }

            DraftManager draft = DraftManager.Instance;
            while (draft != null && draft.IsDraftActive)
            {
                yield return null;
            }

            OpenExitDoors();
            clearSequence = null;
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

        private RewardPickup SpawnRewardPickup()
        {
            EnsureDraftManager();

            GameObject rewardObject = new GameObject("RewardPickup");
            rewardObject.transform.position = ResolveRoomCenter();

            SpriteRenderer spriteRenderer = rewardObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = rewardSprite != null ? rewardSprite : CreateFallbackRewardSprite();
            spriteRenderer.color = Color.white;
            spriteRenderer.sortingLayerName = "Entities";
            spriteRenderer.sortingOrder = 0;

            CircleCollider2D collider = rewardObject.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = Mathf.Max(0.05f, rewardColliderRadius);

            RewardPickup pickup = rewardObject.AddComponent<RewardPickup>();
            Debug.Log($"[RoomRunDirector] RewardPickup spawned at {rewardObject.transform.position}");
            return pickup;
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

        private void OpenExitDoors()
        {
            if (!lifecycle.MarkDoorsOpened())
            {
                return;
            }

            ConfigureExitDoors(true);
            Debug.Log($"[RoomRunDirector] Exit doors opened count={activeDoors.Count}");
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
                    collider.size = new Vector2(1.2f, 1.0f);
                }

                RoomRunExitDoorTrigger trigger = door.GetComponent<RoomRunExitDoorTrigger>();
                if (trigger == null)
                {
                    trigger = door.AddComponent<RoomRunExitDoorTrigger>();
                }

                trigger.Configure(this, i);
                trigger.SetOpen(open);
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
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isOpen || director == null || choiceIndex < 0 || other == null || !other.CompareTag("Player"))
            {
                return;
            }

            director.TryEnterDoor(choiceIndex);
        }
    }
}
