using System.Collections.Generic;
using RIMA.Encounter;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.MapDesigner.Room.Runtime
{
    public sealed class RoomRunDirector : MonoBehaviour
    {
        [SerializeField] private IsoRoomBuilder builder;
        [SerializeField] private RoomBankSO roomBank;
        [SerializeField] private Transform player;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Camera arenaCamera;
        [SerializeField] private EncounterBankSO encounterBank;
        [SerializeField] private float encounterDifficulty = 10f;
        [SerializeField] private Transform enemyContainer;
        [SerializeField] private float cameraPadding = 1.25f;
        [SerializeField] private RoomTemplateSO fallbackTemplate;
        [SerializeField] private int runSeed = 12345;
        [SerializeField] private bool buildOnStart = true;
        [SerializeField] private int depthCount = 5;

        private const string DefaultPlayerPrefabPath = "Prefabs/Warblade";
        private const string DefaultEncounterBankPath = "Encounters/Act1_EncounterBank_Pilot";

        private bool warnedMissingPlayer;
        private bool warnedMissingEncounterBank;
        private bool warnedMissingArenaCamera;
        private DungeonGraph graph;
        private readonly List<GameObject> activeEnemies = new List<GameObject>();

        public DungeonGraph Graph => graph;
        public int CurrentNodeId { get; private set; }
        public DungeonNode CurrentNode => graph?.Get(CurrentNodeId);
        public RIMA.RoomType CurrentRoomType => CurrentNode != null ? CurrentNode.roomType : RIMA.RoomType.Combat;
        public List<DungeonNode> CurrentChoices => graph != null ? graph.ChildrenOf(CurrentNodeId) : new List<DungeonNode>();
        public bool IsRunComplete => CurrentNode == null || CurrentChoices.Count == 0;
        public RoomTemplateSO CurrentTemplate { get; private set; }

        private void Start()
        {
            if (buildOnStart)
            {
                BeginRun();
            }
        }

        public void BeginRun()
        {
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
            SpawnRoomEnemies();

            Debug.Log($"[RoomRunDirector] Built node id={node.id} depth={node.depth} type={node.roomType} choices={CurrentChoices.Count} template={template.roomId}");

            // Exit doors = this node's branch choices (door count + each door's destination type).
            List<RIMA.RoomType> doorTypes = new List<RIMA.RoomType>();
            foreach (DungeonNode child in CurrentChoices)
            {
                doorTypes.Add(child.roomType);
            }
            builder.BuildExitDoors(doorTypes);

            // TODO: encounter start -> clear -> slow-mo -> reward -> illuminate doors -> walk-to-advance.
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
            if (builder.PlayerSpawnMarker == null)
            {
                Debug.LogWarning("[RoomRunDirector] Missing player spawn marker; player spawn skipped.");
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
                GameObject instance = Instantiate(playerPrefab, builder.PlayerSpawnMarker.position, Quaternion.identity);
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

            player.position = builder.PlayerSpawnMarker.position;
            EnsurePlayerRuntime(player.gameObject);
            EnsurePrimaryClass();
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

        private void SpawnRoomEnemies()
        {
            ClearActiveEnemies();
            if (CurrentTemplate == null || CurrentTemplate.enemySpawnSockets == null || CurrentTemplate.enemySpawnSockets.Count == 0)
            {
                return;
            }

            if (encounterBank == null)
            {
                if (!warnedMissingEncounterBank)
                {
                    Debug.LogWarning($"[RoomRunDirector] Missing encounterBank and Resources.Load<EncounterBankSO>(\"{DefaultEncounterBankPath}\") failed; enemy spawn skipped.");
                    warnedMissingEncounterBank = true;
                }
                return;
            }

            EncounterWaveSO wave = encounterBank.PickWave(encounterDifficulty, runSeed + CurrentNodeId);
            if (wave == null)
            {
                Debug.LogWarning("[RoomRunDirector] Encounter bank returned no wave; enemy spawn skipped.");
                return;
            }

            IReadOnlyList<Transform> markerList = builder.EnemySpawnMarkers;
            if (markerList == null || markerList.Count == 0)
            {
                Debug.LogWarning("[RoomRunDirector] Missing enemy spawn markers; enemy spawn skipped.");
                return;
            }

            Transform[] spawnPoints = new Transform[markerList.Count];
            for (int i = 0; i < markerList.Count; i++)
            {
                spawnPoints[i] = markerList[i];
            }

            EnsureEnemyContainer();
            ThreatBudget budget = new ThreatBudget();
            bool eliteRoom = CurrentRoomType == RIMA.RoomType.Elite || CurrentRoomType == RIMA.RoomType.Boss;
            List<GameObject> spawned = budget.Spawn(wave, spawnPoints, wave.threatBudget, eliteRoom);
            for (int i = 0; i < spawned.Count; i++)
            {
                GameObject enemy = spawned[i];
                if (enemy == null)
                {
                    continue;
                }

                enemy.transform.SetParent(enemyContainer, true);
                activeEnemies.Add(enemy);
                Health health = enemy.GetComponent<Health>();
                if (health != null)
                {
                    GameObject trackedEnemy = enemy;
                    health.OnDeath.AddListener(() => activeEnemies.Remove(trackedEnemy));
                }
            }
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

            EnsureEnemyContainer();
        }

        private void ClearActiveEnemies()
        {
            for (int i = activeEnemies.Count - 1; i >= 0; i--)
            {
                if (activeEnemies[i] != null)
                {
                    DestroyRuntimeObject(activeEnemies[i]);
                }
            }

            activeEnemies.Clear();
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

        [ContextMenu("Advance First Choice")]
        private void DebugAdvance()
        {
            if (!IsRunComplete)
            {
                AdvanceTo(0);
            }
        }
    }
}
