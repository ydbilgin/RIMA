using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using RIMA.Systems.Map;

namespace RIMA
{
    /// <summary>
    /// Runtime room lifecycle manager — Hades-style flow:
    ///   1. Player enters room → doors CLOSE (walls fill door gaps)
    ///   2. Enemies spawn in waves
    ///   3. All enemies dead → ROOM CLEARED
    ///   4. Doors OPEN (wall tiles removed from door gaps)
    ///   5. DraftManager offers skill choices
    ///   6. Player walks through a door → triggers next room
    ///
    /// Attach to a "GameManager" root object. Persists across rooms.
    /// Works with the existing tile-based RoomBuilder output.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class RuntimeRoomManager : MonoBehaviour
    {
        public static RuntimeRoomManager Instance { get; private set; }

        [Header("Room Settings")]
        [SerializeField] private int roomWidth = 32;
        [SerializeField] private int roomHeight = 24;
        [SerializeField] private int wallThickness = 2;
        [SerializeField] private int doorWidth = 2;

        [Header("Spawn Settings")]
        [SerializeField] private GameObject[] enemyPrefabs;
        [SerializeField] private int baseEnemyCount = 4;
        [SerializeField] private float spawnDelay = 0.3f;
        [SerializeField] private float enemyCountScaling = 1.2f; // per room

        [Header("Elite Settings")]
        [SerializeField] private int eliteStartRoom = 3;        // elites appear from room 3+
        [SerializeField] private float eliteChanceBase = 0.2f;  // 20% base chance
        [SerializeField] private float eliteChancePerRoom = 0.05f; // +5% per room past threshold

        [Header("References")]
        [SerializeField] private Tilemap wallTilemap;
        [SerializeField] private Tilemap floorTilemap;
        [SerializeField] private HUDController hud;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private PlayerStartMarker playerStartMarker;
        [SerializeField] private UnityEngine.Tilemaps.TileBase wallTileRef;
        [SerializeField] private LargeDungeonMapPainter largeMapPainter;
        [SerializeField] private DungeonWorldBuilder worldBuilder;

        [Header("Map Fragment")]
        [SerializeField] private GameObject mapFragmentPrefab;
        [SerializeField, Range(0f, 1f)] private float mapFragmentChanceWhenAheadVisible = 0.35f;
        [SerializeField, Range(0f, 1f)] private float mapFragmentTwoStepChance = 0.40f;

        [Header("Reward Pickup")]
        [SerializeField] private GameObject rewardPickupPrefab;
        [SerializeField] private float rewardSpawnDistanceInFront = 2.0f;

        [Header("Room Clear Feel")]
        [SerializeField, Range(0.05f, 1f)] private float clearSlowdownScale = 0.18f;
        [SerializeField] private float clearSlowdownDuration = 0.45f;
        [SerializeField] private float clearRewardSpawnDelay = 0.12f;

        [Header("Chest")]
        [SerializeField] private GameObject chestPrefab;
        [SerializeField, Range(0f, 1f)] private float chestSpawnChance = 0.5f;

        [Header("Door Triggers")]
        [SerializeField] private DoorTrigger doorNorth;
        [SerializeField] private DoorTrigger doorSouth;
        [SerializeField] private DoorTrigger doorEast;
        [SerializeField] private DoorTrigger doorWest;

        [Header("Gate Visuals (GateBehavior per direction)")]
        [SerializeField] private GateBehavior gateNorth;
        [SerializeField] private GateBehavior gateSouth;
        [SerializeField] private GateBehavior gateEast;
        [SerializeField] private GateBehavior gateWest;

        [Header("Boss Room")]
        [SerializeField] private int bossRoomNumber = 10;
        [SerializeField] private GameObject bossPrefab;

        // Events
        public UnityEvent OnRoomStarted = new UnityEvent();
        public UnityEvent OnRoomCleared = new UnityEvent();
        public UnityEvent<int> OnRoomChanged = new UnityEvent<int>(); // room number
        public UnityEvent OnBossRoomStarted = new UnityEvent();

        // State
        private int currentRoomIndex;
        private int aliveEnemies;
        private bool roomCleared;
        private bool doorsOpen;
        private DeathScreenManager deathScreen;
        private List<GameObject> activeEnemies = new List<GameObject>();
        private List<GameObject> activeRewards  = new List<GameObject>();

        // Kuzey duvarındaki 3 kapı slotu (x başlangıcı), hepsi aynı y'de
        private int DoorYNorth  => roomHeight - wallThickness;      // 22
        private int DoorXCenter => (roomWidth - doorWidth) / 2;     // 15
        private int DoorXRight  => DoorXCenter + 7;                 // 22
        private int DoorXLeft   => DoorXCenter - 7;                 // 8
        // Güney giriş slotu (her zaman açık)
        private int DoorXSouth  => (roomWidth - doorWidth) / 2;     // 15
        private int DoorYSouth  => 0;                               // güney duvar y=0

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;

            // BoundaryCollider (layer=Boundary=12) duvar tilemap'ini (layer=Default=0) engellesin.
            // Bu ayar domain reload'da sıfırlanabileceğinden her Awake'te set edilir.
            Physics2D.IgnoreLayerCollision(12, 0, false);
        }

        private void Start()
        {
            if (playerTransform == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) playerTransform = player.transform;
            }

            // Auto-find tilemaps if not assigned
            if (wallTilemap == null)
            {
                var wallGO = GameObject.Find("IsoGrid/Walls") ?? GameObject.Find("Room/Wall");
                if (wallGO != null) wallTilemap = wallGO.GetComponent<Tilemap>();
            }
            if (floorTilemap == null)
            {
                var floorGO = GameObject.Find("IsoGrid/Ground") ?? GameObject.Find("Room/Floor");
                if (floorGO != null) floorTilemap = floorGO.GetComponent<Tilemap>();
            }
            if (largeMapPainter == null)
            {
                largeMapPainter = FindAnyObjectByType<LargeDungeonMapPainter>();
            }
            ResolvePlayerStartMarker();

            // Auto-find death screen
            deathScreen = FindAnyObjectByType<DeathScreenManager>();

            if (worldBuilder != null) worldBuilder.BuildWorld();

            // Start first room
            currentRoomIndex = 0;
            StartRoom();
        }

        // ─── Room Lifecycle ──────────────────────────────────

        public void StartRoom()
        {
            currentRoomIndex++;
            roomCleared = false;
            doorsOpen = false;

            bool isBossRoom = DungeonGraph.Instance != null
                ? DungeonGraph.Instance.IsBossRoom()
                : (currentRoomIndex == bossRoomNumber);
            RoomType roomType = DungeonGraph.Instance?.CurrentNode.roomType ?? RoomType.Combat;

            if (largeMapPainter != null)
            {
                largeMapPainter.PaintForRoom(currentRoomIndex, isBossRoom ? RoomType.Boss : roomType);
                roomWidth = largeMapPainter.RoomWidth;
                roomHeight = largeMapPainter.RoomHeight;
            }

            if (playerTransform != null)
                playerTransform.position = GetRoomEntrancePosition();

            // Close all doors (tilemap + triggers + gates)
            CloseAllDoors();
            SetAllDoorTriggersInactive();
            HideAllGates();

            // Clear previous enemies + rewards
            ClearActiveEnemies();
            ClearActiveRewards();

            // Apply floor tint based on room type
            ApplyRoomTint(isBossRoom ? RoomType.Boss : roomType);

            if (isBossRoom && bossPrefab != null)
            {
                StartCoroutine(SpawnBoss());
                OnRoomStarted?.Invoke();
                OnBossRoomStarted?.Invoke();
                OnRoomChanged?.Invoke(currentRoomIndex);
                hud?.SetRoomStatus("⚠ BOSS ODASI ⚠");
                return;
            }

            StartCoroutine(StartRoomByType(roomType));

            OnRoomStarted?.Invoke();
            OnRoomChanged?.Invoke(currentRoomIndex);
        }

        private IEnumerator StartRoomByType(RoomType type)
        {
            switch (type)
            {
                case RoomType.Chest:
                    // No enemies — open doors immediately after a beat
                    hud?.SetRoomStatus($"Room {currentRoomIndex} — Chest");
                    Debug.Log($"[RRM] Room {currentRoomIndex}: CHEST — no combat");
                    yield return new WaitForSeconds(0.3f);
                    TrySpawnChest();
                    yield return new WaitForSeconds(0.2f);
                    OpenDoorsAfterClear();
                    break;

                case RoomType.Merchant:
                    hud?.SetRoomStatus($"Room {currentRoomIndex} — Merchant");
                    Debug.Log($"[RRM] Room {currentRoomIndex}: MERCHANT — no combat (NPC placeholder)");
                    yield return new WaitForSeconds(0.3f);
                    OpenDoorsAfterClear();
                    break;

                case RoomType.Event:
                    hud?.SetRoomStatus($"Room {currentRoomIndex} — Event");
                    Debug.Log($"[RRM] Room {currentRoomIndex}: EVENT — no combat");
                    yield return new WaitForSeconds(0.3f);
                    OpenDoorsAfterClear();
                    break;

                case RoomType.Forge:
                    hud?.SetRoomStatus($"Room {currentRoomIndex} — Forge");
                    Debug.Log($"[RRM] Room {currentRoomIndex}: FORGE");
                    yield return new WaitForSeconds(0.3f);
                    OpenDoorsAfterClear();
                    break;

                case RoomType.Elite:
                {
                    // More enemies, all elites
                    int count = Mathf.Clamp(Mathf.RoundToInt(baseEnemyCount * 0.75f * Mathf.Pow(enemyCountScaling, currentRoomIndex - 1)), 2, 8);
                    hud?.SetRoomStatus($"Room {currentRoomIndex} — ⚠ ELITE ({count} enemies)");
                    Debug.Log($"[RRM] Room {currentRoomIndex}: ELITE — {count} enemies (all elites)");
                    StartCoroutine(SpawnEnemies(count, forceElite: true));
                    yield break;
                }

                default: // Combat
                {
                    int count = Mathf.Clamp(Mathf.RoundToInt(baseEnemyCount * Mathf.Pow(enemyCountScaling, currentRoomIndex - 1)), 2, 20);
                    hud?.SetRoomStatus($"Room {currentRoomIndex} — Enemies: {count}");
                    Debug.Log($"[RRM] Room {currentRoomIndex} started. {count} enemies spawning.");
                    StartCoroutine(SpawnEnemies(count));
                    yield break;
                }
            }
        }

        private void OpenDoorsAfterClear()
        {
            roomCleared = true;
            StartCoroutine(RoomClearedSequence());
        }

        private IEnumerator SpawnBoss()
        {
            aliveEnemies = 1;
            yield return new WaitForSeconds(0.8f);

            Vector3Int bossTile = new Vector3Int(roomWidth / 2, roomHeight / 2 + 3, 0);
            Vector3 spawnPos = floorTilemap != null
                ? floorTilemap.GetCellCenterWorld(bossTile)
                : new Vector3(roomWidth * 0.5f, roomHeight * 0.5f + 3f, 0f);
            var boss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);
            activeEnemies.Add(boss);

            var health = boss.GetComponent<Health>();
            if (health != null)
                health.OnDeath.AddListener(() => OnEnemyDied(boss));

            Debug.Log("[RuntimeRoomManager] Boss spawned: " + bossPrefab.name);
        }

        /// <summary>PenitentSovereign death sequence'den çağrılır — draft yerine class selection gelecek.</summary>
        public void NotifyBossDefeated()
        {
            // Room cleared sequence tetiklenecek; draft yerine ClassSelectionTrigger devralır
            Debug.Log("[RuntimeRoomManager] Boss defeated! Class selection will trigger.");
        }

        private IEnumerator SpawnEnemies(int count, bool forceElite = false)
        {
            aliveEnemies = 0;
            var reservedSpawnPositions = new List<Vector3>();
            yield return new WaitForSeconds(0.5f); // brief calm before storm

            for (int i = 0; i < count; i++)
            {
                // Pick random enemy prefab
                if (enemyPrefabs == null || enemyPrefabs.Length == 0)
                {
                    Debug.LogError("[RuntimeRoomManager] No enemy prefabs assigned — room will NOT auto-clear or spawn rewards.");
                    hud?.SetRoomStatus("ERROR: Enemy prefabs missing");
                    yield break;
                }

                var prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

                Vector3 spawnPos = GetDistributedSpawnPosition(i, count, forceElite, reservedSpawnPositions);
                reservedSpawnPositions.Add(spawnPos);

                var enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
                activeEnemies.Add(enemy);

                // Apply elite affix if forced or past threshold
                if (forceElite || (currentRoomIndex >= eliteStartRoom &&
                    Random.value < eliteChanceBase + (currentRoomIndex - eliteStartRoom) * eliteChancePerRoom))
                {
                    EliteAffix.Apply(enemy, EliteAffix.RandomAffix());
                }

                // Wire death callback
                var health = enemy.GetComponent<Health>();
                if (health != null)
                {
                    health.OnDeath.AddListener(() => OnEnemyDied(enemy));
                    Debug.Log($"[RRM] Enemy [{i}] spawned: {prefab.name} at {spawnPos}, Health wired. aliveEnemies={aliveEnemies + 1}");
                }
                else
                {
                    Debug.LogWarning($"[RRM] Enemy [{i}] {prefab.name} has NO Health component! Will never die.");
                }

                aliveEnemies++;
                yield return new WaitForSeconds(spawnDelay);
            }
            Debug.Log($"[RRM] All {count} enemies spawned. aliveEnemies={aliveEnemies}");
        }

        private Vector3 GetRandomSpawnPosition()
        {
            return GetDistributedSpawnPosition(0, 1, false, null);
        }

        private Vector3 GetDistributedSpawnPosition(int index, int totalCount, bool forceElite, List<Vector3> reservedPositions)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            float minSpacing = forceElite ? 4.2f : 2.9f;
            Vector2 preferred = SelectSpawnBand(index, totalCount, forceElite);

            if (TryFindSpawnNearBand(preferred, forceElite ? 28 : 22, reservedPositions, minSpacing, out Vector3 bandedPos))
            {
                return bandedPos;
            }

            int margin = wallThickness + 4;
            int xMin = margin, xMax = roomWidth - margin;
            int yMin = margin, yMax = roomHeight - margin;

            for (int attempt = 0; attempt < 160; attempt++)
            {
                int x = Random.Range(xMin, xMax);
                int y = Random.Range(yMin, yMax);
                var tilePos = new Vector3Int(x, y, 0);

                if (!IsValidSpawnCell(tilePos, reservedPositions, minSpacing)) continue;

                return floorTilemap != null
                    ? floorTilemap.GetCellCenterWorld(tilePos)
                    : new Vector3(x, y, 0f);
            }

            var fallback = new Vector3Int(cx, cy + Mathf.Max(7, roomHeight / 6), 0);
            if (IsValidSpawnCell(fallback, reservedPositions, minSpacing))
            {
                return floorTilemap != null
                    ? floorTilemap.GetCellCenterWorld(fallback)
                    : new Vector3(fallback.x, fallback.y, 0f);
            }

            var center = new Vector3Int(cx, cy, 0);
            return floorTilemap != null
                ? floorTilemap.GetCellCenterWorld(center)
                : new Vector3(cx, cy, 0f);
        }

        private Vector2 SelectSpawnBand(int index, int totalCount, bool forceElite)
        {
            if (forceElite)
            {
                Vector2[] eliteBands =
                {
                    new Vector2(0.50f, 0.76f),
                    new Vector2(0.27f, 0.62f),
                    new Vector2(0.73f, 0.62f),
                    new Vector2(0.34f, 0.34f),
                    new Vector2(0.66f, 0.34f),
                    new Vector2(0.50f, 0.24f),
                };
                return eliteBands[index % eliteBands.Length];
            }

            Vector2[] combatBands =
            {
                new Vector2(0.24f, 0.68f),
                new Vector2(0.76f, 0.32f),
                new Vector2(0.25f, 0.34f),
                new Vector2(0.75f, 0.66f),
                new Vector2(0.50f, 0.78f),
                new Vector2(0.50f, 0.22f),
                new Vector2(0.18f, 0.50f),
                new Vector2(0.82f, 0.50f),
                new Vector2(0.36f, 0.58f),
                new Vector2(0.64f, 0.42f),
            };

            int offset = Mathf.Abs(currentRoomIndex * 3 + totalCount) % combatBands.Length;
            return combatBands[(index + offset) % combatBands.Length];
        }

        private bool TryFindSpawnNearBand(Vector2 normalizedBand, int maxRadius, List<Vector3> reservedPositions, float minSpacing, out Vector3 worldPosition)
        {
            int preferredX = Mathf.RoundToInt(Mathf.Clamp01(normalizedBand.x) * (roomWidth - 1));
            int preferredY = Mathf.RoundToInt(Mathf.Clamp01(normalizedBand.y) * (roomHeight - 1));

            for (int radius = 0; radius <= maxRadius; radius++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    for (int dy = -radius; dy <= radius; dy++)
                    {
                        if (Mathf.Abs(dx) != radius && Mathf.Abs(dy) != radius) continue;
                        var tilePos = new Vector3Int(preferredX + dx, preferredY + dy, 0);
                        if (!IsValidSpawnCell(tilePos, reservedPositions, minSpacing)) continue;

                        worldPosition = floorTilemap != null
                            ? floorTilemap.GetCellCenterWorld(tilePos)
                            : new Vector3(tilePos.x, tilePos.y, 0f);
                        return true;
                    }
                }
            }

            worldPosition = default;
            return false;
        }

        private bool IsValidSpawnCell(Vector3Int tilePos, List<Vector3> reservedPositions, float minSpacing)
        {
            int margin = wallThickness + 3;
            if (tilePos.x < margin || tilePos.y < margin || tilePos.x >= roomWidth - margin || tilePos.y >= roomHeight - margin)
                return false;

            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            int centerSafeRadius = Mathf.Max(8, Mathf.Min(roomWidth, roomHeight) / 9);
            int centerDx = tilePos.x - cx;
            int centerDy = tilePos.y - cy;
            if (centerDx * centerDx + centerDy * centerDy < centerSafeRadius * centerSafeRadius)
                return false;

            if (floorTilemap != null && floorTilemap.GetTile(tilePos) == null)
                return false;

            for (int dx = -2; dx <= 2; dx++)
            {
                for (int dy = -2; dy <= 2; dy++)
                {
                    if (wallTilemap != null && wallTilemap.GetTile(new Vector3Int(tilePos.x + dx, tilePos.y + dy, 0)) != null)
                        return false;
                }
            }

            Vector3 worldPos = floorTilemap != null
                ? floorTilemap.GetCellCenterWorld(tilePos)
                : new Vector3(tilePos.x, tilePos.y, 0f);

            if (playerTransform != null && Vector2.Distance(worldPos, playerTransform.position) < 5.5f)
                return false;

            if (reservedPositions != null)
            {
                for (int i = 0; i < reservedPositions.Count; i++)
                {
                    if (Vector2.Distance(worldPos, reservedPositions[i]) < minSpacing)
                        return false;
                }
            }

            if (Physics2D.OverlapCircle(worldPos, 0.45f, LayerMask.GetMask("Default")) != null)
                return false;

            return true;
        }

        private void OnEnemyDied(GameObject enemy)
        {
            aliveEnemies--;
            activeEnemies.Remove(enemy);
            Debug.Log($"[RRM] Enemy died: {(enemy != null ? enemy.name : "NULL")}. aliveEnemies={aliveEnemies}");

            // Feed rage system
            if (playerTransform != null)
            {
                var rage = playerTransform.GetComponent<RageSystem>();
                if (rage != null) rage.OnKillEnemy();
            }

            // Track kill for death stats
            if (deathScreen != null)
                deathScreen.RegisterKill();

            hud?.SetRoomStatus(aliveEnemies > 0
                ? $"Room {currentRoomIndex} — Enemies: {aliveEnemies}"
                : "ROOM CLEARED!");

            if (aliveEnemies <= 0 && !roomCleared)
            {
                roomCleared = true;
                StartCoroutine(RoomClearedSequence());
            }
        }

        private IEnumerator RoomClearedSequence()
        {
            Debug.Log($"[RuntimeRoomManager] Room {currentRoomIndex} CLEARED!");
            float previousTimeScale = BeginRoomClearSlowdown();
            yield return new WaitForSecondsRealtime(Mathf.Max(0f, clearRewardSpawnDelay));

            var roomType = DungeonGraph.Instance?.CurrentNode.roomType ?? RoomType.Combat;
            bool spawnReward = roomType == RoomType.Combat || roomType == RoomType.Elite;

            // 1. Reward orb — oyuncunun baktığı yönde çıkar; G ile draft açılır.
            bool rewardSpawned = false;
            if (spawnReward)
            {
                if (rewardPickupPrefab != null)
                {
                    var orb = SpawnRewardInFrontOfPlayer();
                    Debug.Log($"[RRM] RewardPickup spawned at {orb.transform.position}");
                    rewardSpawned = true;
                }
                else if (DraftManager.Instance != null)
                {
                    Debug.LogWarning("[RRM] rewardPickupPrefab NULL — fallback: immediate draft");
                    DraftManager.Instance.ShowDraft();
                }
            }

            // 2. Harita parçası — reward'dan ayrı, köşe tarafında ve reveal durumuna göre düşer.
            TrySpawnMapFragment();

            float remainingSlowdown = Mathf.Max(0f, clearSlowdownDuration - clearRewardSpawnDelay);
            if (remainingSlowdown > 0f)
                yield return new WaitForSecondsRealtime(remainingSlowdown);
            EndRoomClearSlowdown(previousTimeScale);

            // 3. Kapılar: ödül varsa BEKLE, yoksa hemen aç
            if (!rewardSpawned)
            {
                OpenDoorsNow();
            }
            else
            {
                hud?.SetRoomStatus("ODA TEMİZLENDİ — ödülü topla!");
            }

            OnRoomCleared?.Invoke();
            TrySpawnChest();
        }

        /// <summary>Ödül toplandıktan sonra kapıları açar. RewardPickup tarafından çağrılır.</summary>
        public void OpenDoorsAfterReward()
        {
            Debug.Log("[RRM] Reward collected — opening doors!");
            OpenDoorsNow();
        }

        private void OpenDoorsNow()
        {
            var exits = DungeonGraph.Instance != null
                ? DungeonGraph.Instance.GetCurrentExits()
                : AllDirectionsFallback();

            SetDoorsOpenByDirection(exits);
            doorsOpen = true;
            SetDoorTriggersActive(exits);
            UnlockGates(exits);

            DungeonMapUI.Instance?.RefreshMap();
            int exitCount = Mathf.Clamp(exits.Count, 0, 3);
            hud?.SetRoomStatus(exitCount > 0
                ? $"{exitCount} kapı açıldı — rotanı seç"
                : "Çıkış yok");
        }

        /// <summary>
        /// Her oda temizlenince harita parçası düşer.
        /// revealSteps: 1 adım (%60) veya 2 adım (%40) ilerisini gösterir.
        /// 2 adım = 3-fork + arkasındaki 2-fork gibi derin reveal.
        /// </summary>
        private void TrySpawnMapFragment()
        {
            if (mapFragmentPrefab == null) return;
            if (!ShouldDropMapFragment(out int revealedStepsAhead, out float dropChance))
            {
                Debug.Log($"[RRM] Map fragment skipped (revealedStepsAhead={revealedStepsAhead}, chance={dropChance:P0})");
                return;
            }

            // revealSteps: 1 veya 2 (random weighted)
            int revealSteps = Random.value < mapFragmentTwoStepChance ? 2 : 1;

            var go = Instantiate(mapFragmentPrefab, GetMapFragmentCornerPosition(), Quaternion.identity);
            activeRewards.Add(go);
            var mf = go?.GetComponent<MapFragment>();
            if (mf != null) mf.revealSteps = revealSteps;
            Debug.Log($"[RRM] Map fragment spawned at {go.transform.position} (revealSteps={revealSteps}, revealedStepsAhead={revealedStepsAhead}, dropChance={dropChance:P0})");
        }

        private bool ShouldDropMapFragment(out int revealedStepsAhead, out float dropChance)
        {
            revealedStepsAhead = 0;
            dropChance = 0f;

            var graph = DungeonGraph.Instance;
            if (graph == null || !graph.HasForwardExits()) return false;

            revealedStepsAhead = graph.GetRevealedStepsAhead();
            dropChance = GetMapFragmentDropChance(revealedStepsAhead, mapFragmentChanceWhenAheadVisible);
            return Random.value <= dropChance;
        }

        public static float GetMapFragmentDropChance(int revealedStepsAhead, float chanceWhenAheadVisible)
        {
            return revealedStepsAhead <= 1 ? 1f : Mathf.Clamp01(chanceWhenAheadVisible);
        }

        private float BeginRoomClearSlowdown()
        {
            float previousScale = Time.timeScale;
            if (clearSlowdownDuration > 0f)
                Time.timeScale = Mathf.Min(previousScale, clearSlowdownScale);
            return previousScale;
        }

        private void EndRoomClearSlowdown(float previousScale)
        {
            if (Time.timeScale > 0f && Time.timeScale <= clearSlowdownScale + 0.01f)
                Time.timeScale = previousScale;
        }

        /// <summary>
        /// Önceki odadan kalan reward/fragment nesnelerini temizler.
        /// StartRoom başında çağrılır.
        /// </summary>
        private void ClearActiveRewards()
        {
            foreach (var r in activeRewards)
                if (r != null) Destroy(r);
            activeRewards.Clear();

            // Sahnede kalmış olabilecek stray nesneleri de temizle
            foreach (var mf in FindObjectsByType<MapFragment>(FindObjectsSortMode.None))
                Destroy(mf.gameObject);
            foreach (var rp in FindObjectsByType<RewardPickup>(FindObjectsSortMode.None))
                Destroy(rp.gameObject);
        }

        /// <summary>
        /// Prefabı güvenli merkez pozisyonunda üretir.
        /// offset: base pozisyondan dünya uzayında sapma (harita parçası için).
        /// </summary>
        private GameObject SpawnAtSafeCenter(GameObject prefab, Vector3 offset)
        {
            Vector3 basePos = GetSafeCenterPosition();

            GameObject go;
            if (offset != Vector3.zero && floorTilemap != null)
            {
                Vector3 candidate = basePos + offset;
                var tileAtOffset = floorTilemap.WorldToCell(candidate);
                if (floorTilemap.GetTile(tileAtOffset) == null ||
                    (wallTilemap != null && wallTilemap.GetTile(tileAtOffset) != null))
                    candidate = basePos;
                go = Instantiate(prefab, candidate, Quaternion.identity);
            }
            else
            {
                go = Instantiate(prefab, basePos, Quaternion.identity);
            }

            activeRewards.Add(go);
            return go;
        }

        private GameObject SpawnRewardInFrontOfPlayer()
        {
            Vector3 fallback = GetSafeCenterPosition();
            if (playerTransform == null)
                return SpawnAtWorldPosition(rewardPickupPrefab, fallback);

            Vector2 facing = GetPlayerFacingDirection();
            Vector3 preferred = playerTransform.position + (Vector3)(facing * rewardSpawnDistanceInFront);
            return SpawnAtWorldPosition(rewardPickupPrefab, GetNearestSafeFloorPosition(preferred, fallback));
        }

        private GameObject SpawnAtWorldPosition(GameObject prefab, Vector3 position)
        {
            var go = Instantiate(prefab, position, Quaternion.identity);
            activeRewards.Add(go);
            return go;
        }

        private Vector2 GetPlayerFacingDirection()
        {
            if (playerTransform != null &&
                playerTransform.TryGetComponent<PlayerController>(out var controller) &&
                controller.FacingDirection.sqrMagnitude > 0.01f)
            {
                return controller.FacingDirection.normalized;
            }

            return Vector2.down;
        }

        private Vector3 GetMapFragmentCornerPosition()
        {
            if (floorTilemap == null) return GetSafeCenterPosition() + new Vector3(3f, 2f, 0f);

            int margin = wallThickness + 3;
            var corners = new[]
            {
                new Vector3Int(margin, margin, 0),
                new Vector3Int(roomWidth - margin - 1, margin, 0),
                new Vector3Int(margin, roomHeight - margin - 1, 0),
                new Vector3Int(roomWidth - margin - 1, roomHeight - margin - 1, 0),
            };

            Vector3 fallback = GetSafeCenterPosition();
            Vector3 best = fallback;
            float bestScore = float.MinValue;

            foreach (var corner in corners)
            {
                Vector3 candidate = GetNearestSafeFloorPosition(
                    floorTilemap.GetCellCenterWorld(corner),
                    fallback,
                    searchRadius: 4);

                float playerDistance = playerTransform != null
                    ? Vector2.Distance(candidate, playerTransform.position)
                    : 0f;
                float rewardDistance = DistanceFromActiveRewardPickups(candidate);
                float score = playerDistance + rewardDistance * 0.5f + Random.value * 0.25f;
                if (score > bestScore)
                {
                    bestScore = score;
                    best = candidate;
                }
            }

            return best;
        }

        private float DistanceFromActiveRewardPickups(Vector3 position)
        {
            float best = 99f;
            foreach (var reward in activeRewards)
            {
                if (reward == null || reward.GetComponent<RewardPickup>() == null) continue;
                best = Mathf.Min(best, Vector2.Distance(position, reward.transform.position));
            }
            return best;
        }

        private Vector3 GetNearestSafeFloorPosition(Vector3 preferred, Vector3 fallback, int searchRadius = 5)
        {
            if (floorTilemap == null) return preferred;

            var origin = floorTilemap.WorldToCell(preferred);
            for (int r = 0; r <= searchRadius; r++)
            {
                for (int dx = -r; dx <= r; dx++)
                {
                    for (int dy = -r; dy <= r; dy++)
                    {
                        if (r > 0 && Mathf.Abs(dx) != r && Mathf.Abs(dy) != r) continue;
                        var tile = new Vector3Int(origin.x + dx, origin.y + dy, 0);
                        if (floorTilemap.GetTile(tile) == null) continue;
                        if (wallTilemap != null && wallTilemap.GetTile(tile) != null) continue;
                        return floorTilemap.GetCellCenterWorld(tile);
                    }
                }
            }

            return fallback;
        }

        /// <summary>
        /// Tilemap'teki tüm zemin tile'larının centroidini hesaplar,
        /// oradan spiral BFS ile ilk geçerli iç zemin tile'ını döner.
        /// Physics collider kontrolü yok — ölü düşman kalıntıları engellemesin.
        /// </summary>
        private Vector3 GetSafeCenterPosition()
        {
            if (floorTilemap == null)
                return new Vector3(roomWidth / 2f, roomHeight / 2f, 0f);

            // Centroid hesapla
            long sumX = 0, sumY = 0;
            int count = 0;
            foreach (var pos in floorTilemap.cellBounds.allPositionsWithin)
            {
                if (floorTilemap.GetTile(pos) != null) { sumX += pos.x; sumY += pos.y; count++; }
            }

            if (count == 0)
                return floorTilemap.GetCellCenterWorld(new Vector3Int(roomWidth / 2, roomHeight / 2, 0));

            var center = new Vector3Int((int)(sumX / count), (int)(sumY / count), 0);

            // Centroid'den spiral BFS — sadece tile kontrolü (physics yok)
            for (int r = 0; r <= 10; r++)
            {
                for (int dx = -r; dx <= r; dx++)
                {
                    for (int dy = -r; dy <= r; dy++)
                    {
                        if (r > 0 && Mathf.Abs(dx) != r && Mathf.Abs(dy) != r) continue;

                        var tile = new Vector3Int(center.x + dx, center.y + dy, 0);
                        if (floorTilemap.GetTile(tile) == null) continue;
                        if (wallTilemap != null && wallTilemap.GetTile(tile) != null) continue;

                        return floorTilemap.GetCellCenterWorld(tile);
                    }
                }
            }

            return floorTilemap.GetCellCenterWorld(center);
        }

        private static Dictionary<DoorDirection, RoomType> AllDirectionsFallback()
        {
            Debug.LogWarning("[RuntimeRoomManager] DungeonGraph.Instance is null — using North-only fallback!");
            return new Dictionary<DoorDirection, RoomType>
            {
                { DoorDirection.North, RoomType.Combat },
            };
        }

        /// <summary>Called by DoorTrigger when player walks through an open door.</summary>
        public void OnPlayerEnteredDoor(DoorDirection direction)
        {
            if (!roomCleared || !doorsOpen) return;

            // Hafif reminder — ödülü almadıysa bildir ama engelleme (Hades gibi)
            var pendingReward = FindAnyObjectByType<RewardPickup>();
            if (pendingReward != null && !pendingReward.WasCollected)
            {
                hud?.SetRoomStatus("⚠ Ödülünü almadın! Geri dönebilirsin...");
                Debug.Log("[RRM] Player leaving without collecting reward orb.");
            }

            // DungeonGraph'ta ilerle
            if (DungeonGraph.Instance != null)
            {
                if (!DungeonGraph.Instance.Navigate(direction, out _))
                {
                    Debug.LogWarning($"[RuntimeRoomManager] {direction} door has no graph exit — ignoring.");
                    return;
                }
            }

            Debug.Log($"[RuntimeRoomManager] Player entered {direction} door. Moving to room {currentRoomIndex + 1}.");

            // Fade transition — paint the next room while screen is black.
            if (RoomTransitionFX.Instance != null)
                RoomTransitionFX.Instance.DoTransition(StartRoom);
            else
                StartRoom();
        }

        private void TeleportPlayerToOppositeDoor(DoorDirection enteredFrom)
        {
            if (playerTransform == null) return;
            playerTransform.position = GetRoomEntrancePosition();
        }

        private Vector3 GetRoomEntrancePosition()
        {
            ResolvePlayerStartMarker();
            int spawnX = DoorXSouth + doorWidth / 2;
            int spawnY = wallThickness + 2;
            var tilePos = new Vector3Int(spawnX, spawnY, 0);

            Vector3 fallback = GetSafeCenterPosition();
            if (floorTilemap == null) return fallback;

            if (playerStartMarker != null && playerStartMarker.isActiveAndEnabled)
            {
                return GetNearestPlayableStartPosition(playerStartMarker.SpawnPosition, fallback, searchRadius: 22);
            }

            Vector3 candidate = floorTilemap.GetCellCenterWorld(tilePos);
            return GetNearestPlayableStartPosition(candidate, fallback, searchRadius: 12);
        }

        private Vector3 GetNearestPlayableStartPosition(Vector3 preferred, Vector3 fallback, int searchRadius)
        {
            if (largeMapPainter != null)
                return largeMapPainter.GetNearestPlayableFloorPosition(preferred, fallback, searchRadius);

            return GetNearestSafeFloorPosition(preferred, fallback, searchRadius);
        }

        private void ResolvePlayerStartMarker()
        {
            if (playerStartMarker != null) return;
            playerStartMarker = FindAnyObjectByType<PlayerStartMarker>();
        }

        // ─── Door State (Tilemap) ────────────────────────────

        /// <summary>
        /// Tüm kuzey slotlarını kapat, güney girişini aç.
        /// Oda başında çağrılır.
        /// </summary>
        private void CloseAllDoors()
        {
            if (wallTilemap == null) return;

            var wallTile = wallTileRef
                        ?? wallTilemap.GetTile(new Vector3Int(0, DoorYNorth, 0))
                        ?? wallTilemap.GetTile(new Vector3Int(0, 0, 0))
                        ?? wallTilemap.GetTile(new Vector3Int(1, 0, 0));

            if (wallTile == null)
                Debug.LogWarning("[RuntimeRoomManager] CloseAllDoors: wallTile not found — assign wallTileRef in Inspector!");
            else
                Debug.Log($"[RuntimeRoomManager] CloseAllDoors: using tile {wallTile.name}");

            // 3 kuzey slotunu kapat
            SetNorthSlotTile(DoorXCenter, wallTile);
            SetNorthSlotTile(DoorXRight,  wallTile);
            SetNorthSlotTile(DoorXLeft,   wallTile);

            // Güney girişini her zaman aç (giriş kapısı)
            SetSouthEntranceTile(null);
        }

        /// <summary>Grafın verdiği çıkışlara göre kuzey slotlarını aç.</summary>
        private void SetDoorsOpenByDirection(Dictionary<DoorDirection, RoomType> exits)
        {
            if (wallTilemap == null) return;

            if (exits.ContainsKey(DoorDirection.North)) SetNorthSlotTile(DoorXCenter, null);
            if (exits.ContainsKey(DoorDirection.East))  SetNorthSlotTile(DoorXRight,  null);
            if (exits.ContainsKey(DoorDirection.West))  SetNorthSlotTile(DoorXLeft,   null);
        }

        private void SetNorthSlotTile(int xStart, UnityEngine.Tilemaps.TileBase tile)
        {
            for (int x = xStart; x < xStart + doorWidth; x++)
                for (int t = 0; t < wallThickness; t++)
                    wallTilemap.SetTile(new Vector3Int(x, roomHeight - 1 - t, 0), tile);
        }

        private void SetSouthEntranceTile(UnityEngine.Tilemaps.TileBase tile)
        {
            for (int x = DoorXSouth; x < DoorXSouth + doorWidth; x++)
                for (int t = 0; t < wallThickness; t++)
                    wallTilemap.SetTile(new Vector3Int(x, DoorYSouth + t, 0), tile);
        }

        private void SetDoorTriggersActive(Dictionary<DoorDirection, RoomType> exits)
        {
            if (doorNorth != null) doorNorth.SetActive(exits.ContainsKey(DoorDirection.North));
            if (doorSouth != null) doorSouth.SetActive(exits.ContainsKey(DoorDirection.South));
            if (doorEast  != null) doorEast.SetActive(exits.ContainsKey(DoorDirection.East));
            if (doorWest  != null) doorWest.SetActive(exits.ContainsKey(DoorDirection.West));
        }

        private void SetAllDoorTriggersInactive()
        {
            if (doorNorth != null) doorNorth.SetActive(false);
            if (doorSouth != null) doorSouth.SetActive(false);
            if (doorEast  != null) doorEast.SetActive(false);
            if (doorWest  != null) doorWest.SetActive(false);
        }

        private void UnlockGates(Dictionary<DoorDirection, RoomType> exits)
        {
            UnlockOrHideGate(gateNorth, DoorDirection.North, exits);
            UnlockOrHideGate(gateSouth, DoorDirection.South, exits);
            UnlockOrHideGate(gateEast,  DoorDirection.East,  exits);
            UnlockOrHideGate(gateWest,  DoorDirection.West,  exits);
        }

        private void HideAllGates()
        {
            if (gateNorth != null) gateNorth.Hide();
            if (gateSouth != null) gateSouth.Hide();
            if (gateEast  != null) gateEast.Hide();
            if (gateWest  != null) gateWest.Hide();
        }

        private static void UnlockOrHideGate(GateBehavior gate, DoorDirection dir,
                                              Dictionary<DoorDirection, RoomType> exits)
        {
            if (gate == null) return;
            if (exits.TryGetValue(dir, out RoomType type))
                gate.Unlock(type);
            else
                gate.Hide();
        }

        // Milestone sandık odaları — bu odalarda garantili çıkar
        private static readonly int[] ChestMilestoneRooms = { 3, 6 };

        private void TrySpawnChest()
        {
            if (chestPrefab == null) return;

            bool milestone = System.Array.IndexOf(ChestMilestoneRooms, currentRoomIndex) >= 0;
            if (!milestone && Random.value > chestSpawnChance) return;

            Vector3 pos = GetRandomSpawnPosition();
            Instantiate(chestPrefab, pos, Quaternion.identity);
            Debug.Log($"[RuntimeRoomManager] Chest spawned at {pos} (milestone={milestone})");
        }

        private void ClearActiveEnemies()
        {
            foreach (var e in activeEnemies)
            {
                if (e != null) Destroy(e);
            }
            activeEnemies.Clear();
            aliveEnemies = 0;
        }

        // ─── Room Atmosphere (Floor Tint) ───────────────────

        private void ApplyRoomTint(RoomType type)
        {
            if (floorTilemap == null) return;

            Color tint = type switch
            {
                RoomType.Elite    => new Color(0.85f, 0.65f, 0.55f, 1f), // warm reddish
                RoomType.Boss     => new Color(0.70f, 0.50f, 0.50f, 1f), // deep crimson
                RoomType.Chest    => new Color(0.90f, 0.85f, 0.65f, 1f), // warm gold
                RoomType.Merchant => new Color(0.70f, 0.80f, 0.90f, 1f), // cool blue
                RoomType.Event    => new Color(0.75f, 0.65f, 0.85f, 1f), // mystic purple
                RoomType.Forge    => new Color(0.85f, 0.70f, 0.50f, 1f), // forge orange
                _                 => new Color(0.80f, 0.80f, 0.80f, 1f), // neutral gray (combat)
            };

            floorTilemap.color = tint;
            Debug.Log($"[RRM] Floor tint → {type}: {tint}");
        }

        // ─── Public Queries ──────────────────────────────────

        public int CurrentRoom => currentRoomIndex;
        public bool IsRoomCleared => roomCleared;
        public int AliveEnemies => aliveEnemies;
        public float RoomWidth => roomWidth;
        public float RoomHeight => roomHeight;

        public int PreviewRoomCount => largeMapPainter != null
            ? largeMapPainter.PreviewLayoutCount
            : LargeDungeonMapPainterBase.DefaultPreviewLayoutCount;

        public string GetPreviewRoomName(int index)
        {
            return largeMapPainter != null
                ? largeMapPainter.GetPreviewLayoutName(index)
                : LargeDungeonMapPainterBase.GetDefaultPreviewLayoutName(index);
        }

        public BoundsInt GetCurrentRoomBounds()
        {
            if (worldBuilder != null)
                return worldBuilder.GetRoomBounds(currentRoomIndex);
            if (largeMapPainter != null && largeMapPainter.TryGetPlayableCellBounds(out var b))
                return b;
            return default;
        }

        public void PreviewRoomByIndex(int index)
        {
            if (largeMapPainter == null)
                largeMapPainter = FindAnyObjectByType<LargeDungeonMapPainter>();

            if (largeMapPainter == null)
            {
                Debug.LogWarning("[RuntimeRoomManager] Cannot preview room: LargeDungeonMapPainter missing.");
                return;
            }

            StopAllCoroutines();
            ClearActiveEnemies();
            ClearActiveRewards();

            currentRoomIndex = Mathf.Clamp(index + 1, 1, Mathf.Max(1, PreviewRoomCount));
            roomCleared = true;
            doorsOpen = false;

            largeMapPainter.PaintPreviewLayout(index);
            roomWidth = largeMapPainter.RoomWidth;
            roomHeight = largeMapPainter.RoomHeight;

            if (playerTransform == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) playerTransform = player.transform;
            }

            if (playerTransform != null)
                playerTransform.position = GetRoomEntrancePosition();

            CloseAllDoors();
            SetAllDoorTriggersInactive();
            HideAllGates();
            ApplyRoomTint(RoomType.Event);

            string previewName = GetPreviewRoomName(index);
            hud?.SetRoomStatus($"TEST ROOM: {previewName}");
            OnRoomChanged?.Invoke(currentRoomIndex);
            Debug.Log($"[RuntimeRoomManager] Preview room loaded: {previewName} ({index + 1}/{PreviewRoomCount})");
        }
    }


}
