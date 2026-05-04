using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

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

    [DefaultExecutionOrder(-200)]
    public class LargeDungeonMapPainterBase : MonoBehaviour
    {
        private enum LayoutKind
        {
            BrokenEntryHall,
            ChainGallery,
            ShrineCrossroad,
            CryptBasin,
            PillarArena,
            SplitVault,
            RitualHall,
            CollapsedLibrary,
            NarrowApproach,
            CrescentSanctum,
            BrokenCauseway,
            ReliquaryLoop,
            ForkedOssuary,
            AmbushCloister,
            RiftWell,
            BossAntechamber
        }

        private static readonly LayoutKind[] ThresholdLayouts =
        {
            LayoutKind.BrokenEntryHall,
            LayoutKind.ChainGallery,
            LayoutKind.NarrowApproach,
        };

        private static readonly LayoutKind[] OssuaryLayouts =
        {
            LayoutKind.CryptBasin,
            LayoutKind.ForkedOssuary,
            LayoutKind.ReliquaryLoop,
        };

        private static readonly LayoutKind[] SanctumLayouts =
        {
            LayoutKind.ShrineCrossroad,
            LayoutKind.RitualHall,
            LayoutKind.CrescentSanctum,
        };

        private static readonly LayoutKind[] RiftLayouts =
        {
            LayoutKind.SplitVault,
            LayoutKind.AmbushCloister,
            LayoutKind.RiftWell,
            LayoutKind.PillarArena,
        };

        private static readonly LayoutKind[] PreviewLayouts =
        {
            LayoutKind.BrokenEntryHall,
            LayoutKind.PillarArena,
            LayoutKind.CollapsedLibrary,
            LayoutKind.BrokenCauseway,
            LayoutKind.AmbushCloister,
            LayoutKind.CryptBasin,
            LayoutKind.ReliquaryLoop,
            LayoutKind.ShrineCrossroad,
            LayoutKind.RiftWell,
            LayoutKind.BossAntechamber,
        };

        private static readonly string[] PreviewLayoutNames =
        {
            "R01 Broken Entry Gate",
            "R02 Ordered Guard Hall",
            "R03 Cell Spine",
            "R04 Broken Causeway",
            "R05 Cross-Chain Clamp",
            "R06 Sunken Crypt Basin",
            "R07 Reliquary Loop",
            "R08 Shrine Crossroad",
            "R09 Rift Well Edge Tear",
            "R10 Containment Arena",
        };

        private readonly struct RoomLightSpec
        {
            public readonly Vector2 normalizedPosition;
            public readonly Color color;
            public readonly float intensity;
            public readonly float innerRadius;
            public readonly float outerRadius;
            public readonly bool flicker;

            public RoomLightSpec(Vector2 normalizedPosition, Color color, float intensity, float innerRadius, float outerRadius, bool flicker)
            {
                this.normalizedPosition = normalizedPosition;
                this.color = color;
                this.intensity = intensity;
                this.innerRadius = innerRadius;
                this.outerRadius = outerRadius;
                this.flicker = flicker;
            }
        }

        [Header("Tilemaps")]
        [SerializeField] private Tilemap floorTilemap;
        [SerializeField] private Tilemap wallTilemap;

        [Header("Size")]
        [SerializeField] private int defaultWidth = 220;
        [SerializeField] private int defaultHeight = 150;
        [SerializeField] private int wallThickness = 3;

        [Header("Tiles")]
        [SerializeField] private TileBase[] floorTiles;
        [SerializeField] private TileBase[] wallTiles;

        [Header("Layout")]
        [SerializeField] private bool paintOnAwake = false;
        [SerializeField] private int seed = 4303;
        [SerializeField, Range(0, 16)] private int cameraSafetyFloorPadding = 0;

        [Header("Lighting")]
        [SerializeField] private bool createProceduralLights = true;
        [SerializeField, Range(0.05f, 1f)] private float globalLightIntensity = 0.34f;
        [SerializeField] private Color globalLightColor = new Color(0.18f, 0.23f, 0.29f, 1f);
        [SerializeField, Range(0f, 2f)] private float localLightAccentScale = 0.82f;
        [SerializeField] private string proceduralLightRootName = "Procedural Room Lights";
        [SerializeField] private string proceduralDecorRootName = "Procedural Room Decor";

        private readonly List<TileBase> cachedFloorTiles = new List<TileBase>();
        private readonly List<TileBase> cachedWallTiles = new List<TileBase>();
        private bool[,] lastPlayableFloorMask;
        private int roomWidth;
        private int roomHeight;

        public int RoomWidth => roomWidth;
        public int RoomHeight => roomHeight;
        public int PreviewLayoutCount => PreviewLayouts.Length;
        public static int DefaultPreviewLayoutCount => PreviewLayouts.Length;

        public string GetPreviewLayoutName(int index) => GetDefaultPreviewLayoutName(index);

        public Vector3 GetNearestPlayableFloorPosition(Vector3 preferred, Vector3 fallback, int searchRadius)
        {
            if (floorTilemap == null || lastPlayableFloorMask == null)
                return fallback;

            Vector3Int origin = floorTilemap.WorldToCell(preferred);
            for (int r = 0; r <= searchRadius; r++)
            {
                for (int dx = -r; dx <= r; dx++)
                {
                    for (int dy = -r; dy <= r; dy++)
                    {
                        if (r > 0 && Mathf.Abs(dx) != r && Mathf.Abs(dy) != r) continue;
                        var tile = new Vector3Int(origin.x + dx, origin.y + dy, 0);
                        if (!IsPlayableCell(tile.x, tile.y)) continue;
                        if (wallTilemap != null && wallTilemap.GetTile(tile) != null) continue;
                        return floorTilemap.GetCellCenterWorld(tile);
                    }
                }
            }

            return GetPlayableCenterFallback(fallback);
        }

        public bool IsPlayableWorldPosition(Vector3 world)
        {
            if (floorTilemap == null || lastPlayableFloorMask == null) return false;
            Vector3Int cell = floorTilemap.WorldToCell(world);
            return IsPlayableCell(cell.x, cell.y) &&
                   (wallTilemap == null || wallTilemap.GetTile(cell) == null);
        }

        public bool IsPlayableCell(Vector3Int cell)
        {
            return IsPlayableCell(cell.x, cell.y) &&
                   (wallTilemap == null || wallTilemap.GetTile(cell) == null);
        }

        public bool TryGetPlayableCellBounds(out BoundsInt bounds)
        {
            bounds = default;
            if (lastPlayableFloorMask == null) return false;

            int minX = roomWidth;
            int minY = roomHeight;
            int maxX = -1;
            int maxY = -1;

            for (int x = 0; x < roomWidth; x++)
            {
                for (int y = 0; y < roomHeight; y++)
                {
                    if (!lastPlayableFloorMask[x, y]) continue;
                    minX = Mathf.Min(minX, x);
                    minY = Mathf.Min(minY, y);
                    maxX = Mathf.Max(maxX, x);
                    maxY = Mathf.Max(maxY, y);
                }
            }

            if (maxX < minX || maxY < minY) return false;
            bounds = new BoundsInt(minX, minY, 0, maxX - minX + 1, maxY - minY + 1, 1);
            return true;
        }

        private Vector3 GetPlayableCenterFallback(Vector3 fallback)
        {
            if (floorTilemap == null || lastPlayableFloorMask == null) return fallback;

            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            for (int r = 0; r <= Mathf.Max(roomWidth, roomHeight); r++)
            {
                for (int dx = -r; dx <= r; dx++)
                {
                    for (int dy = -r; dy <= r; dy++)
                    {
                        if (r > 0 && Mathf.Abs(dx) != r && Mathf.Abs(dy) != r) continue;
                        int x = cx + dx;
                        int y = cy + dy;
                        if (!IsPlayableCell(x, y)) continue;
                        var tile = new Vector3Int(x, y, 0);
                        if (wallTilemap != null && wallTilemap.GetTile(tile) != null) continue;
                        return floorTilemap.GetCellCenterWorld(tile);
                    }
                }
            }

            return fallback;
        }

        private bool IsPlayableCell(int x, int y)
        {
            return lastPlayableFloorMask != null &&
                   x >= 0 && y >= 0 &&
                   x < roomWidth && y < roomHeight &&
                   lastPlayableFloorMask[x, y];
        }

        public static string GetDefaultPreviewLayoutName(int index)
        {
            if (PreviewLayoutNames.Length == 0) return "Room Preview";
            int clamped = Mathf.Clamp(index, 0, PreviewLayoutNames.Length - 1);
            return PreviewLayoutNames[clamped];
        }

        private void Awake()
        {
            ResolveTilemaps();
            CacheTiles();
            if (paintOnAwake)
            {
                PaintForRoom(1, RoomType.Combat);
            }
        }

        public void PaintForRoom(int roomIndex, RoomType roomType)
        {
            ResolveTilemaps();
            CacheTiles();
            if (floorTilemap == null || wallTilemap == null || cachedFloorTiles.Count == 0 || cachedWallTiles.Count == 0)
            {
                Debug.LogWarning("[LargeDungeonMapPainter] Missing tilemaps or tiles; keeping existing room.");
                return;
            }

            LayoutKind layout = SelectLayout(roomIndex, roomType);
            Vector2Int size = GetLayoutSize(layout, roomType);
            roomWidth = size.x;
            roomHeight = size.y;

            Random.InitState(seed + roomIndex * 97 + (int)roomType * 13);
            floorTilemap.ClearAllTiles();
            wallTilemap.ClearAllTiles();

            bool[,] floorMask = BuildFloorMask(layout);
            lastPlayableFloorMask = floorMask;
            bool[,] visualFloorMask = BuildVisualFloorMask(floorMask);

            PaintFloor(visualFloorMask);
            PaintCameraSafetyFloor();
            PaintBoundaryWalls(floorMask);
            PaintShelterCellPartitions(floorMask);
            PaintLayoutFeatures(layout, floorMask);
            PaintCombatViewKeepAnchors(floorMask);
            PaintRoomLighting(layout, floorMask);
            PaintNarrativeDecor(layout, floorMask);

            floorTilemap.CompressBounds();
            wallTilemap.CompressBounds();
        }

        public void PaintPreviewLayout(int index)
        {
            int clamped = Mathf.Clamp(index, 0, PreviewLayouts.Length - 1);
            RoomType previewType = PreviewLayouts[clamped] == LayoutKind.BossAntechamber
                ? RoomType.Boss
                : RoomType.Event;
            PaintLayout(PreviewLayouts[clamped], clamped + 1, previewType);
        }

        private void PaintLayout(LayoutKind layout, int roomIndex, RoomType roomType)
        {
            ResolveTilemaps();
            CacheTiles();
            if (floorTilemap == null || wallTilemap == null || cachedFloorTiles.Count == 0 || cachedWallTiles.Count == 0)
            {
                Debug.LogWarning("[LargeDungeonMapPainter] Missing tilemaps or tiles; keeping existing room.");
                return;
            }

            Vector2Int size = GetLayoutSize(layout, roomType);
            roomWidth = size.x;
            roomHeight = size.y;

            Random.InitState(seed + roomIndex * 97 + (int)roomType * 13);
            floorTilemap.ClearAllTiles();
            wallTilemap.ClearAllTiles();

            bool[,] floorMask = BuildFloorMask(layout);
            lastPlayableFloorMask = floorMask;
            bool[,] visualFloorMask = BuildVisualFloorMask(floorMask);

            PaintFloor(visualFloorMask);
            PaintCameraSafetyFloor();
            PaintBoundaryWalls(floorMask);
            PaintShelterCellPartitions(floorMask);
            PaintLayoutFeatures(layout, floorMask);
            PaintCombatViewKeepAnchors(floorMask);
            PaintRoomLighting(layout, floorMask);
            PaintNarrativeDecor(layout, floorMask);

            floorTilemap.CompressBounds();
            wallTilemap.CompressBounds();
        }

        private void ResolveTilemaps()
        {
            if (floorTilemap == null)
            {
                var go = GameObject.Find("IsoGrid/Ground") ?? GameObject.Find("Room/Floor");
                if (go != null) floorTilemap = go.GetComponent<Tilemap>();
            }

            if (wallTilemap == null)
            {
                var go = GameObject.Find("IsoGrid/Walls") ?? GameObject.Find("Room/Wall");
                if (go != null) wallTilemap = go.GetComponent<Tilemap>();
            }
        }

        private void CacheTiles()
        {
            if (cachedFloorTiles.Count == 0)
            {
                AddTiles(floorTiles, cachedFloorTiles);
                AddUsedTiles(floorTilemap, cachedFloorTiles);
            }

            if (cachedWallTiles.Count == 0)
            {
                AddTiles(wallTiles, cachedWallTiles);
                AddUsedTiles(wallTilemap, cachedWallTiles);
            }
        }

        private static void AddTiles(TileBase[] source, List<TileBase> target)
        {
            if (source == null) return;
            foreach (TileBase tile in source)
            {
                if (tile != null && !target.Contains(tile)) target.Add(tile);
            }
        }

        private static void AddUsedTiles(Tilemap source, List<TileBase> target)
        {
            if (source == null) return;

            int count = source.GetUsedTilesCount();
            if (count <= 0) return;

            var used = new TileBase[count];
            source.GetUsedTilesNonAlloc(used);
            foreach (TileBase tile in used)
            {
                if (tile != null && !target.Contains(tile)) target.Add(tile);
            }
        }

        private LayoutKind SelectLayout(int roomIndex, RoomType roomType)
        {
            if (roomType == RoomType.Boss) return LayoutKind.BossAntechamber;
            if (roomType == RoomType.Chest) return LayoutKind.ReliquaryLoop;
            if (roomType == RoomType.Merchant) return LayoutKind.BrokenEntryHall;
            if (roomType == RoomType.Forge) return LayoutKind.BrokenCauseway;
            if (roomType == RoomType.Event) return (roomIndex % 2 == 0) ? LayoutKind.CrescentSanctum : LayoutKind.RiftWell;
            if (roomType == RoomType.Elite) return (roomIndex % 2 == 0) ? LayoutKind.AmbushCloister : LayoutKind.ForkedOssuary;

            if (roomIndex <= 3)
                return ThresholdLayouts[Mathf.Abs(roomIndex - 1) % ThresholdLayouts.Length];
            if (roomIndex <= 6)
                return OssuaryLayouts[Mathf.Abs(roomIndex - 4) % OssuaryLayouts.Length];
            if (roomIndex <= 9)
                return SanctumLayouts[Mathf.Abs(roomIndex - 7) % SanctumLayouts.Length];

            return RiftLayouts[Mathf.Abs(roomIndex - 10) % RiftLayouts.Length];
        }

        private string GetNarrativeBand(LayoutKind layout)
        {
            return layout switch
            {
                LayoutKind.BrokenEntryHall or LayoutKind.ChainGallery or LayoutKind.NarrowApproach => "threshold",
                LayoutKind.CryptBasin or LayoutKind.ForkedOssuary or LayoutKind.ReliquaryLoop or LayoutKind.CollapsedLibrary => "ossuary",
                LayoutKind.ShrineCrossroad or LayoutKind.RitualHall or LayoutKind.CrescentSanctum => "sanctum",
                LayoutKind.SplitVault or LayoutKind.AmbushCloister or LayoutKind.RiftWell or LayoutKind.PillarArena => "rift",
                LayoutKind.BossAntechamber => "boss",
                _ => "threshold",
            };
        }

        private Vector2Int GetLayoutSize(LayoutKind layout, RoomType roomType)
        {
            if (roomType == RoomType.Boss) return new Vector2Int(132, 86);

            return layout switch
            {
                LayoutKind.ChainGallery => new Vector2Int(164, 86),
                LayoutKind.ShrineCrossroad => new Vector2Int(150, 104),
                LayoutKind.CryptBasin => new Vector2Int(144, 104),
                LayoutKind.PillarArena => new Vector2Int(152, 100),
                LayoutKind.SplitVault => new Vector2Int(156, 98),
                LayoutKind.RitualHall => new Vector2Int(148, 96),
                LayoutKind.CollapsedLibrary => new Vector2Int(158, 96),
                LayoutKind.NarrowApproach => new Vector2Int(166, 88),
                LayoutKind.CrescentSanctum => new Vector2Int(150, 102),
                LayoutKind.BrokenCauseway => new Vector2Int(170, 90),
                LayoutKind.ReliquaryLoop => new Vector2Int(154, 102),
                LayoutKind.ForkedOssuary => new Vector2Int(162, 98),
                LayoutKind.AmbushCloister => new Vector2Int(156, 104),
                LayoutKind.RiftWell => new Vector2Int(150, 106),
                _ => new Vector2Int(148, 98),
            };
        }

        private bool[,] BuildFloorMask(LayoutKind layout)
        {
            bool[,] mask = new bool[roomWidth, roomHeight];
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;

            switch (layout)
            {
                case LayoutKind.ChainGallery:
                    AddRect(mask, 16, cy - 18, roomWidth - 32, 36);
                    AddRect(mask, 34, cy + 18, 34, 18);
                    AddRect(mask, roomWidth - 72, cy - 36, 42, 18);
                    AddEllipse(mask, cx - 45, cy, 26, 20);
                    AddEllipse(mask, cx + 42, cy, 28, 18);
                    RemoveRect(mask, cx - 8, cy + 12, 16, 15);
                    break;
                case LayoutKind.ShrineCrossroad:
                    AddEllipse(mask, cx, cy, 38, 28);
                    AddRect(mask, cx - 12, 14, 24, roomHeight - 28);
                    AddRect(mask, 18, cy - 11, roomWidth - 36, 22);
                    AddEllipse(mask, cx - 48, cy + 28, 22, 16);
                    AddEllipse(mask, cx + 48, cy - 27, 22, 16);
                    RemoveEllipse(mask, cx - 33, cy - 23, 12, 10);
                    RemoveEllipse(mask, cx + 35, cy + 24, 13, 11);
                    break;
                case LayoutKind.CryptBasin:
                    AddEllipse(mask, cx, cy, 58, 42);
                    AddEllipse(mask, cx - 40, cy + 26, 22, 18);
                    AddEllipse(mask, cx + 43, cy - 25, 26, 17);
                    AddRect(mask, cx - 12, 12, 24, 22);
                    AddRect(mask, cx - 14, roomHeight - 34, 28, 22);
                    RemoveEllipse(mask, cx, cy, 17, 11);
                    RemoveRect(mask, cx - 50, cy - 7, 16, 14);
                    break;
                case LayoutKind.PillarArena:
                    AddEllipse(mask, cx, cy, 60, 38);
                    AddRect(mask, cx - 50, cy - 24, 100, 48);
                    AddEllipse(mask, cx - 58, cy + 24, 20, 15);
                    AddEllipse(mask, cx + 59, cy - 22, 22, 15);
                    RemoveRect(mask, 18, 18, 15, 20);
                    RemoveRect(mask, roomWidth - 36, roomHeight - 40, 18, 24);
                    break;
                case LayoutKind.SplitVault:
                    AddEllipse(mask, cx - 38, cy, 42, 34);
                    AddEllipse(mask, cx + 40, cy + 2, 45, 33);
                    AddRect(mask, cx - 28, cy - 10, 56, 20);
                    AddRect(mask, cx - 10, 14, 20, 22);
                    AddRect(mask, cx - 12, roomHeight - 34, 24, 22);
                    RemoveRect(mask, cx - 5, cy + 16, 10, 30);
                    RemoveRect(mask, cx - 4, cy - 44, 8, 26);
                    break;
                case LayoutKind.RitualHall:
                    AddRect(mask, cx - 48, cy - 30, 96, 60);
                    AddEllipse(mask, cx, cy, 54, 36);
                    AddEllipse(mask, cx - 52, cy - 30, 20, 16);
                    AddEllipse(mask, cx + 50, cy + 28, 22, 17);
                    RemoveEllipse(mask, cx, cy, 16, 10);
                    RemoveRect(mask, cx - 62, cy + 6, 16, 16);
                    break;
                case LayoutKind.CollapsedLibrary:
                    AddRect(mask, 22, cy - 30, roomWidth - 44, 60);
                    AddEllipse(mask, cx - 48, cy + 32, 28, 18);
                    AddEllipse(mask, cx + 52, cy - 30, 30, 18);
                    AddRect(mask, cx - 12, 12, 24, 22);
                    AddRect(mask, cx - 13, roomHeight - 34, 26, 20);
                    RemoveRect(mask, cx - 62, cy - 7, 30, 14);
                    RemoveRect(mask, cx + 26, cy + 9, 34, 14);
                    break;
                case LayoutKind.NarrowApproach:
                    AddRect(mask, 16, cy - 13, roomWidth - 32, 26);
                    AddEllipse(mask, roomWidth - 58, cy, 44, 30);
                    AddEllipse(mask, 44, cy + 20, 28, 18);
                    AddRect(mask, cx - 16, cy - 30, 32, 18);
                    AddRect(mask, cx + 24, cy + 13, 40, 18);
                    RemoveRect(mask, cx - 35, cy + 4, 18, 15);
                    break;
                case LayoutKind.CrescentSanctum:
                    AddEllipse(mask, cx - 18, cy, 60, 39);
                    AddEllipse(mask, cx + 30, cy + 2, 45, 31);
                    AddEllipse(mask, cx - 46, cy + 30, 24, 17);
                    AddEllipse(mask, cx - 52, cy - 28, 23, 16);
                    AddRect(mask, cx - 10, 12, 20, 24);
                    AddRect(mask, cx + 8, roomHeight - 36, 22, 24);
                    RemoveEllipse(mask, cx + 10, cy + 2, 34, 23);
                    RemoveRect(mask, cx + 26, cy - 11, 24, 22);
                    break;
                case LayoutKind.BrokenCauseway:
                    AddRect(mask, 14, cy - 12, roomWidth - 28, 24);
                    AddRect(mask, 34, cy + 10, 42, 18);
                    AddRect(mask, cx - 20, cy - 29, 42, 18);
                    AddRect(mask, roomWidth - 78, cy + 11, 45, 18);
                    AddEllipse(mask, 42, cy, 24, 18);
                    AddEllipse(mask, roomWidth - 46, cy, 28, 20);
                    RemoveRect(mask, cx - 9, cy + 4, 18, 16);
                    RemoveRect(mask, cx + 27, cy - 17, 18, 14);
                    break;
                case LayoutKind.ReliquaryLoop:
                    AddEllipse(mask, cx - 38, cy, 36, 30);
                    AddEllipse(mask, cx + 38, cy, 36, 30);
                    AddRect(mask, cx - 52, cy - 9, 104, 18);
                    AddRect(mask, cx - 22, cy + 22, 44, 18);
                    AddRect(mask, cx - 22, cy - 40, 44, 18);
                    AddRect(mask, cx - 10, 12, 20, 24);
                    AddRect(mask, cx - 11, roomHeight - 36, 22, 24);
                    RemoveEllipse(mask, cx, cy, 18, 13);
                    RemoveRect(mask, cx - 7, cy + 9, 14, 18);
                    break;
                case LayoutKind.ForkedOssuary:
                    AddRect(mask, 16, cy - 10, roomWidth - 32, 20);
                    AddEllipse(mask, cx - 54, cy + 26, 29, 20);
                    AddEllipse(mask, cx + 56, cy + 25, 31, 19);
                    AddEllipse(mask, cx - 36, cy - 27, 28, 19);
                    AddEllipse(mask, cx + 39, cy - 29, 29, 18);
                    AddRect(mask, cx - 12, 13, 24, 26);
                    AddRect(mask, cx - 13, roomHeight - 38, 26, 24);
                    RemoveRect(mask, cx - 6, cy + 9, 12, 22);
                    break;
                case LayoutKind.AmbushCloister:
                    AddRect(mask, 24, cy - 28, roomWidth - 48, 56);
                    AddRect(mask, 38, cy + 28, roomWidth - 76, 18);
                    AddRect(mask, 38, cy - 46, roomWidth - 76, 18);
                    AddEllipse(mask, 34, cy, 22, 20);
                    AddEllipse(mask, roomWidth - 35, cy, 23, 20);
                    AddRect(mask, cx - 10, 12, 20, 24);
                    AddRect(mask, cx - 11, roomHeight - 36, 22, 24);
                    RemoveRect(mask, cx - 28, cy - 9, 56, 18);
                    RemoveEllipse(mask, cx, cy, 17, 12);
                    break;
                case LayoutKind.RiftWell:
                    AddEllipse(mask, cx, cy, 58, 42);
                    AddEllipse(mask, cx - 45, cy + 30, 24, 16);
                    AddEllipse(mask, cx + 47, cy - 28, 26, 17);
                    AddEllipse(mask, cx + 42, cy + 27, 20, 15);
                    AddRect(mask, cx - 12, 12, 24, 25);
                    AddRect(mask, cx - 13, roomHeight - 37, 26, 24);
                    RemoveEllipse(mask, cx, cy, 23, 16);
                    RemoveRect(mask, cx - 5, cy - 42, 10, 22);
                    break;
                case LayoutKind.BossAntechamber:
                    AddEllipse(mask, cx, cy, 40, 24);
                    AddRect(mask, cx - 36, cy - 20, 72, 40);
                    AddRect(mask, cx - 12, 8, 24, 18);
                    AddRect(mask, cx - 14, roomHeight - 26, 28, 18);
                    AddEllipse(mask, cx - 34, cy + 20, 16, 12);
                    AddEllipse(mask, cx + 34, cy + 20, 16, 12);
                    break;
                default:
                    AddEllipse(mask, cx, cy, 56, 37);
                    AddRect(mask, cx - 48, cy - 26, 96, 52);
                    AddEllipse(mask, cx - 45, cy + 28, 25, 18);
                    AddEllipse(mask, cx + 48, cy - 26, 27, 18);
                    AddRect(mask, cx - 10, 12, 20, 25);
                    RemoveEllipse(mask, cx + 33, cy + 24, 13, 11);
                    RemoveRect(mask, 19, roomHeight - 33, 20, 16);
                    break;
            }

            CarveBrokenEdges(mask, layout);
            AddDoorSockets(mask);
            EnsureCombatCore(mask, cx, cy);
            EnsureTraversalSpine(mask, cx, cy);
            return mask;
        }

        private bool[,] BuildVisualFloorMask(bool[,] playableMask)
        {
            bool[,] visualMask = new bool[roomWidth, roomHeight];
            int shellRadius = Mathf.Clamp(wallThickness + 8, 8, 14);

            for (int x = 0; x < roomWidth; x++)
            {
                for (int y = 0; y < roomHeight; y++)
                {
                    visualMask[x, y] = playableMask[x, y] || TouchesFloorWithin(playableMask, x, y, shellRadius);
                }
            }

            return visualMask;
        }

        private void PaintFloor(bool[,] floorMask)
        {
            for (int x = 0; x < roomWidth; x++)
            {
                for (int y = 0; y < roomHeight; y++)
                {
                    if (floorMask[x, y])
                    {
                        floorTilemap.SetTile(new Vector3Int(x, y, 0), PickFloorTile(x, y));
                    }
                }
            }
        }

        private void PaintCameraSafetyFloor()
        {
            int padding = Mathf.Clamp(cameraSafetyFloorPadding, 0, 16);
            if (padding <= 0) return;

            for (int x = -padding; x < roomWidth + padding; x++)
            {
                for (int y = -padding; y < roomHeight + padding; y++)
                {
                    if (x >= 0 && y >= 0 && x < roomWidth && y < roomHeight) continue;
                    floorTilemap.SetTile(new Vector3Int(x, y, 0), PickFloorTile(x, y));
                }
            }
        }

        private void PaintBoundaryWalls(bool[,] floorMask)
        {
            for (int x = 0; x < roomWidth; x++)
            {
                for (int y = 0; y < roomHeight; y++)
                {
                    if (floorMask[x, y]) continue;
                    if (TouchesFloorWithin(floorMask, x, y, wallThickness))
                    {
                        wallTilemap.SetTile(new Vector3Int(x, y, 0), PickWallTile(x, y));
                    }
                }
            }
        }

        private void PaintLayoutFeatures(LayoutKind layout, bool[,] floorMask)
        {
            switch (layout)
            {
                case LayoutKind.ChainGallery:
                    PaintChainGallery(floorMask);
                    break;
                case LayoutKind.ShrineCrossroad:
                    PaintShrineCrossroad(floorMask);
                    break;
                case LayoutKind.CryptBasin:
                    PaintCryptBasin(floorMask);
                    break;
                case LayoutKind.PillarArena:
                    PaintPillarArena(floorMask);
                    break;
                case LayoutKind.SplitVault:
                    PaintSplitVault(floorMask);
                    break;
                case LayoutKind.RitualHall:
                    PaintRitualHall(floorMask);
                    break;
                case LayoutKind.CollapsedLibrary:
                    PaintCollapsedLibrary(floorMask);
                    break;
                case LayoutKind.NarrowApproach:
                    PaintNarrowApproach(floorMask);
                    break;
                case LayoutKind.CrescentSanctum:
                    PaintCrescentSanctum(floorMask);
                    break;
                case LayoutKind.BrokenCauseway:
                    PaintBrokenCauseway(floorMask);
                    break;
                case LayoutKind.ReliquaryLoop:
                    PaintReliquaryLoop(floorMask);
                    break;
                case LayoutKind.ForkedOssuary:
                    PaintForkedOssuary(floorMask);
                    break;
                case LayoutKind.AmbushCloister:
                    PaintAmbushCloister(floorMask);
                    break;
                case LayoutKind.RiftWell:
                    PaintRiftWell(floorMask);
                    break;
                case LayoutKind.BossAntechamber:
                    PaintBossAntechamber(floorMask);
                    break;
                default:
                    PaintBrokenEntryHall(floorMask);
                    break;
            }
        }

        private void PaintCombatViewKeepAnchors(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;

            PaintAnchorWallRect(floorMask, cx - 23, cy + 13, 12, 3);
            PaintAnchorWallRect(floorMask, cx + 11, cy + 13, 12, 3);
            PaintAnchorWallRect(floorMask, cx - 23, cy - 16, 12, 3);
            PaintAnchorWallRect(floorMask, cx + 11, cy - 16, 12, 3);

            PaintAnchorWallRect(floorMask, cx - 29, cy - 7, 3, 14);
            PaintAnchorWallRect(floorMask, cx + 26, cy - 7, 3, 14);

            PaintAnchorWallRect(floorMask, cx - 17, cy + 8, 4, 4);
            PaintAnchorWallRect(floorMask, cx + 13, cy + 8, 4, 4);
            PaintAnchorWallRect(floorMask, cx - 17, cy - 12, 4, 4);
            PaintAnchorWallRect(floorMask, cx + 13, cy - 12, 4, 4);
        }

        private void PaintAnchorWallRect(bool[,] floorMask, int x, int y, int width, int height)
        {
            for (int px = x; px < x + width; px++)
            {
                for (int py = y; py < y + height; py++)
                {
                    if (!IsFloor(floorMask, px, py)) continue;
                    wallTilemap.SetTile(new Vector3Int(px, py, 0), PickWallTile(px, py));
                }
            }
        }

        private void PaintShelterCellPartitions(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;

            int sideRoomW = Mathf.Clamp(roomWidth / 7, 22, 36);
            int sideRoomH = Mathf.Clamp(roomHeight / 5, 18, 30);

            PaintWallRect(floorMask, wallThickness + 24, cy + sideRoomH / 2, sideRoomW, 3);
            PaintWallRect(floorMask, wallThickness + 24, cy - sideRoomH / 2 - 3, sideRoomW, 3);
            PaintWallRect(floorMask, roomWidth - wallThickness - 24 - sideRoomW, cy + sideRoomH / 2, sideRoomW, 3);
            PaintWallRect(floorMask, roomWidth - wallThickness - 24 - sideRoomW, cy - sideRoomH / 2 - 3, sideRoomW, 3);

            PaintWallRect(floorMask, cx - sideRoomW - 18, roomHeight - wallThickness - 28, sideRoomW, 3);
            PaintWallRect(floorMask, cx + 18, roomHeight - wallThickness - 28, sideRoomW, 3);
            PaintWallRect(floorMask, cx - sideRoomW - 18, wallThickness + 25, sideRoomW, 3);
            PaintWallRect(floorMask, cx + 18, wallThickness + 25, sideRoomW, 3);

            PaintWallRect(floorMask, wallThickness + 28, cy - 18, 3, 36);
            PaintWallRect(floorMask, roomWidth - wallThickness - 31, cy - 18, 3, 36);
        }

        private void PaintBrokenEntryHall(bool[,] floorMask)
        {
            PaintPillarGrid(floorMask, 20, 14, 2, 2, 2);
            PaintWallRect(floorMask, 12, roomHeight - 17, 12, 3);
            PaintWallRect(floorMask, roomWidth - 28, 13, 13, 3);
            PaintWallRect(floorMask, roomWidth / 2 - 18, roomHeight / 2 + 17, 11, 3);
        }

        private void PaintChainGallery(bool[,] floorMask)
        {
            PaintWallRect(floorMask, 28, roomHeight / 2 - 12, 5, 24);
            PaintWallRect(floorMask, roomWidth - 42, roomHeight / 2 - 16, 5, 32);
            PaintPillarGrid(floorMask, 18, 10, 4, 2, 2);
        }

        private void PaintShrineCrossroad(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 25, cy + 12, 15, 3);
            PaintWallRect(floorMask, cx + 10, cy + 12, 15, 3);
            PaintWallRect(floorMask, cx - 24, cy - 15, 14, 3);
            PaintWallRect(floorMask, cx + 11, cy - 15, 14, 3);
            PaintPillarGrid(floorMask, 18, 16, 2, 2, 2);
        }

        private void PaintCryptBasin(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 19, cy - 3, 9, 3);
            PaintWallRect(floorMask, cx + 10, cy + 2, 9, 3);
            PaintPillarGrid(floorMask, 22, 15, 2, 2, 2);
        }

        private void PaintPillarArena(bool[,] floorMask)
        {
            PaintPillarGrid(floorMask, 22, 16, 3, 2, 2);
            PaintWallRect(floorMask, 18, roomHeight - 23, 14, 3);
            PaintWallRect(floorMask, roomWidth - 36, 20, 16, 3);
        }

        private void PaintSplitVault(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            PaintWallRect(floorMask, cx - 4, roomHeight / 2 + 16, 8, 18);
            PaintWallRect(floorMask, cx - 4, roomHeight / 2 - 34, 8, 18);
            PaintWallRect(floorMask, cx - 2, roomHeight / 2 - 5, 4, 10);
            PaintPillarGrid(floorMask, 24, 14, 2, 2, 2);
        }

        private void PaintRitualHall(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 9, cy - 2, 18, 4);
            PaintWallRect(floorMask, cx - 35, cy + 18, 14, 3);
            PaintWallRect(floorMask, cx + 21, cy - 20, 14, 3);
            PaintPillarGrid(floorMask, 28, 18, 2, 2, 2);
        }

        private void PaintCollapsedLibrary(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 60, cy - 18, 30, 3);
            PaintWallRect(floorMask, cx - 58, cy + 18, 28, 3);
            PaintWallRect(floorMask, cx + 28, cy - 20, 34, 3);
            PaintWallRect(floorMask, cx + 24, cy + 17, 36, 3);
            PaintPillarGrid(floorMask, 30, 18, 2, 2, 2);
        }

        private void PaintNarrowApproach(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 8, cy + 15, 18, 3);
            PaintWallRect(floorMask, cx + 24, cy - 15, 20, 3);
            PaintPillarGrid(floorMask, 24, 12, 3, 1, 2);
        }

        private void PaintCrescentSanctum(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 48, cy + 18, 18, 3);
            PaintWallRect(floorMask, cx - 50, cy - 21, 20, 3);
            PaintWallRect(floorMask, cx + 18, cy + 22, 12, 4);
            PaintPillarGrid(floorMask, 26, 17, 2, 2, 2);
        }

        private void PaintBrokenCauseway(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 46, cy - 11, 22, 3);
            PaintWallRect(floorMask, cx - 3, cy + 14, 20, 3);
            PaintWallRect(floorMask, cx + 42, cy - 10, 24, 3);
            PaintPillarGrid(floorMask, 34, 12, 3, 1, 2);
        }

        private void PaintReliquaryLoop(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 45, cy + 14, 16, 3);
            PaintWallRect(floorMask, cx + 29, cy - 17, 17, 3);
            PaintWallRect(floorMask, cx - 6, cy - 36, 12, 4);
            PaintWallRect(floorMask, cx - 6, cy + 32, 12, 4);
            PaintPillarGrid(floorMask, 28, 20, 2, 2, 2);
        }

        private void PaintForkedOssuary(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 65, cy + 9, 17, 3);
            PaintWallRect(floorMask, cx + 47, cy + 9, 18, 3);
            PaintWallRect(floorMask, cx - 40, cy - 14, 14, 3);
            PaintWallRect(floorMask, cx + 28, cy - 17, 14, 3);
            PaintPillarGrid(floorMask, 30, 16, 2, 2, 2);
        }

        private void PaintAmbushCloister(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 54, cy + 28, 28, 3);
            PaintWallRect(floorMask, cx + 26, cy + 28, 28, 3);
            PaintWallRect(floorMask, cx - 54, cy - 31, 28, 3);
            PaintWallRect(floorMask, cx + 26, cy - 31, 28, 3);
            PaintPillarGrid(floorMask, 30, 22, 3, 2, 2);
        }

        private void PaintRiftWell(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 24, cy + 20, 16, 3);
            PaintWallRect(floorMask, cx + 8, cy - 23, 17, 3);
            PaintWallRect(floorMask, cx - 55, cy - 5, 16, 3);
            PaintWallRect(floorMask, cx + 39, cy + 5, 16, 3);
            PaintPillarGrid(floorMask, 28, 18, 2, 2, 2);
        }

        private void PaintBossAntechamber(bool[,] floorMask)
        {
            PaintWallRect(floorMask, roomWidth / 2 - 8, roomHeight - wallThickness - 13, 16, 3);
            PaintWallRect(floorMask, roomWidth / 2 - 8, wallThickness + 10, 16, 3);
            PaintPillarGrid(floorMask, 24, 16, 3, 2, 2);
        }

        private void PaintPillarGrid(bool[,] floorMask, int spacingX, int spacingY, int columns, int rows, int radius)
        {
            int startX = (roomWidth - (columns - 1) * spacingX) / 2;
            int startY = (roomHeight - (rows - 1) * spacingY) / 2;

            for (int ix = 0; ix < columns; ix++)
            {
                for (int iy = 0; iy < rows; iy++)
                {
                    PaintWallRect(floorMask, startX + ix * spacingX - radius, startY + iy * spacingY - radius, radius * 2 + 1, radius * 2 + 1);
                }
            }
        }

        private void PaintWallRect(bool[,] floorMask, int x, int y, int width, int height)
        {
            for (int px = x; px < x + width; px++)
            {
                for (int py = y; py < y + height; py++)
                {
                    if (!IsFloor(floorMask, px, py)) continue;
                    if (IsProtectedTraversalCell(px, py)) continue;
                    wallTilemap.SetTile(new Vector3Int(px, py, 0), PickWallTile(px, py));
                }
            }
        }

        private bool IsProtectedTraversalCell(int x, int y)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;

            if (IsNearCenter(x, y, 18)) return true;
            if (Mathf.Abs(x - cx) <= 12 && y <= wallThickness + 30) return true;
            if (Mathf.Abs(x - cx) <= 12 && y >= roomHeight - wallThickness - 34) return true;
            if (Mathf.Abs(y - cy) <= 9 && x <= wallThickness + 34) return true;
            if (Mathf.Abs(y - cy) <= 9 && x >= roomWidth - wallThickness - 36) return true;

            return false;
        }

        private readonly struct DecorSpec
        {
            public readonly string spritePath;
            public readonly Vector2 normalizedPosition;
            public readonly float scale;
            public readonly int sortingOrder;

            public DecorSpec(string spritePath, Vector2 normalizedPosition, float scale, int sortingOrder)
            {
                this.spritePath = spritePath;
                this.normalizedPosition = normalizedPosition;
                this.scale = scale;
                this.sortingOrder = sortingOrder;
            }
        }

        private void PaintNarrativeDecor(LayoutKind layout, bool[,] floorMask)
        {
            Transform root = GetOrCreateDecorRoot();
            ClearChildren(root);

            foreach (DecorSpec spec in GetDecorSpecs(layout))
            {
                Sprite sprite = RimaGeneratedSpriteCache.Load(spec.spritePath);
                if (sprite == null) continue;

                Vector2Int preferred = new Vector2Int(
                    Mathf.RoundToInt(spec.normalizedPosition.x * (roomWidth - 1)),
                    Mathf.RoundToInt(spec.normalizedPosition.y * (roomHeight - 1)));

                if (!TryFindNearestFloorCell(floorMask, preferred, 24, out Vector2Int cell))
                    continue;

                Vector3 world = floorTilemap.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0));
                var go = new GameObject("Room Decor - " + sprite.name);
                go.transform.SetParent(root, false);
                go.transform.position = new Vector3(world.x, world.y, -0.08f);
                go.transform.localScale = Vector3.one * spec.scale;

                var renderer = go.AddComponent<SpriteRenderer>();
                renderer.sprite = sprite;
                renderer.sortingOrder = spec.sortingOrder + cell.y;
                renderer.color = Color.white;
            }
        }

        private IEnumerable<DecorSpec> GetDecorSpecs(LayoutKind layout)
        {
            const string decor = "Environment/StoneDungeon/Decor/";
            const string walls = "Environment/StoneDungeon/Walls/";

            yield return new DecorSpec(walls + "RIMA_gate_arch", new Vector2(0.50f, 0.88f), 1.45f, 40);
            yield return new DecorSpec(walls + "RIMA_gate_spikes", new Vector2(0.50f, 0.14f), 1.25f, 40);

            switch (layout)
            {
                case LayoutKind.ChainGallery:
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.20f, 0.62f), 1.10f, 42);
                    yield return new DecorSpec(walls + "RIMA_wall_banner", new Vector2(0.78f, 0.38f), 1.12f, 42);
                    yield return new DecorSpec(decor + "RIMA_debris", new Vector2(0.36f, 0.30f), 0.88f, 35);
                    yield return new DecorSpec(decor + "RIMA_pillar_base", new Vector2(0.62f, 0.70f), 0.90f, 35);
                    break;
                case LayoutKind.ShrineCrossroad:
                    yield return new DecorSpec(decor + "RIMA_shrine", new Vector2(0.50f, 0.55f), 1.18f, 45);
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.24f, 0.72f), 1.02f, 42);
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.76f, 0.28f), 1.02f, 42);
                    break;
                case LayoutKind.CryptBasin:
                    yield return new DecorSpec(decor + "RIMA_rift_crystal", new Vector2(0.52f, 0.52f), 1.08f, 45);
                    yield return new DecorSpec(decor + "RIMA_debris", new Vector2(0.30f, 0.70f), 0.82f, 35);
                    yield return new DecorSpec(walls + "RIMA_wall_cracked", new Vector2(0.75f, 0.28f), 1.05f, 42);
                    break;
                case LayoutKind.PillarArena:
                    yield return new DecorSpec(decor + "RIMA_pillar_base", new Vector2(0.28f, 0.65f), 0.90f, 38);
                    yield return new DecorSpec(decor + "RIMA_pillar_base", new Vector2(0.72f, 0.35f), 0.90f, 38);
                    yield return new DecorSpec(walls + "RIMA_wall_iron", new Vector2(0.50f, 0.76f), 1.15f, 42);
                    break;
                case LayoutKind.SplitVault:
                    yield return new DecorSpec(walls + "RIMA_wall_iron", new Vector2(0.34f, 0.52f), 1.08f, 42);
                    yield return new DecorSpec(walls + "RIMA_wall_cracked", new Vector2(0.66f, 0.52f), 1.08f, 42);
                    yield return new DecorSpec(decor + "RIMA_rift_crystal", new Vector2(0.50f, 0.28f), 0.88f, 45);
                    break;
                case LayoutKind.RitualHall:
                    yield return new DecorSpec(decor + "RIMA_rift_crystal", new Vector2(0.50f, 0.50f), 1.12f, 45);
                    yield return new DecorSpec(decor + "RIMA_shrine", new Vector2(0.26f, 0.72f), 0.95f, 44);
                    yield return new DecorSpec(decor + "RIMA_shrine", new Vector2(0.74f, 0.28f), 0.95f, 44);
                    break;
                case LayoutKind.CollapsedLibrary:
                    yield return new DecorSpec(decor + "RIMA_debris", new Vector2(0.30f, 0.46f), 1.05f, 36);
                    yield return new DecorSpec(decor + "RIMA_debris", new Vector2(0.68f, 0.58f), 0.95f, 36);
                    yield return new DecorSpec(walls + "RIMA_wall_banner", new Vector2(0.50f, 0.78f), 1.10f, 42);
                    break;
                case LayoutKind.NarrowApproach:
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.22f, 0.60f), 1.05f, 42);
                    yield return new DecorSpec(walls + "RIMA_wall_iron", new Vector2(0.72f, 0.42f), 1.08f, 42);
                    yield return new DecorSpec(decor + "RIMA_debris", new Vector2(0.50f, 0.30f), 0.82f, 35);
                    break;
                case LayoutKind.CrescentSanctum:
                    yield return new DecorSpec(decor + "RIMA_shrine", new Vector2(0.34f, 0.42f), 1.05f, 44);
                    yield return new DecorSpec(decor + "RIMA_rift_crystal", new Vector2(0.62f, 0.58f), 0.95f, 45);
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.22f, 0.72f), 1.00f, 42);
                    break;
                case LayoutKind.BrokenCauseway:
                    yield return new DecorSpec(decor + "RIMA_debris", new Vector2(0.48f, 0.42f), 1.05f, 36);
                    yield return new DecorSpec(walls + "RIMA_wall_cracked", new Vector2(0.26f, 0.58f), 1.05f, 42);
                    yield return new DecorSpec(decor + "RIMA_rift_crystal", new Vector2(0.76f, 0.55f), 0.88f, 45);
                    break;
                case LayoutKind.ReliquaryLoop:
                    yield return new DecorSpec(decor + "RIMA_shrine", new Vector2(0.50f, 0.30f), 1.00f, 44);
                    yield return new DecorSpec(decor + "RIMA_shrine", new Vector2(0.50f, 0.72f), 1.00f, 44);
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.28f, 0.50f), 1.00f, 42);
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.72f, 0.50f), 1.00f, 42);
                    break;
                case LayoutKind.ForkedOssuary:
                    yield return new DecorSpec(decor + "RIMA_debris", new Vector2(0.34f, 0.32f), 0.92f, 36);
                    yield return new DecorSpec(decor + "RIMA_debris", new Vector2(0.66f, 0.34f), 0.92f, 36);
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.24f, 0.72f), 1.00f, 42);
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.76f, 0.72f), 1.00f, 42);
                    break;
                case LayoutKind.AmbushCloister:
                    yield return new DecorSpec(walls + "RIMA_wall_iron", new Vector2(0.28f, 0.74f), 1.10f, 42);
                    yield return new DecorSpec(walls + "RIMA_wall_iron", new Vector2(0.72f, 0.26f), 1.10f, 42);
                    yield return new DecorSpec(decor + "RIMA_pillar_base", new Vector2(0.20f, 0.48f), 0.86f, 38);
                    yield return new DecorSpec(decor + "RIMA_pillar_base", new Vector2(0.80f, 0.52f), 0.86f, 38);
                    break;
                case LayoutKind.RiftWell:
                    yield return new DecorSpec(decor + "RIMA_rift_crystal", new Vector2(0.50f, 0.52f), 1.20f, 45);
                    yield return new DecorSpec(decor + "RIMA_shrine", new Vector2(0.28f, 0.30f), 0.88f, 44);
                    yield return new DecorSpec(walls + "RIMA_wall_cracked", new Vector2(0.72f, 0.70f), 1.05f, 42);
                    break;
                case LayoutKind.BossAntechamber:
                    yield return new DecorSpec(walls + "RIMA_gate_arch", new Vector2(0.50f, 0.76f), 1.65f, 45);
                    yield return new DecorSpec(decor + "RIMA_rift_crystal", new Vector2(0.50f, 0.48f), 1.25f, 48);
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.30f, 0.68f), 1.10f, 42);
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.70f, 0.68f), 1.10f, 42);
                    break;
                default:
                    yield return new DecorSpec(decor + "RIMA_rift_crystal", new Vector2(0.50f, 0.50f), 1.00f, 45);
                    yield return new DecorSpec(decor + "RIMA_debris", new Vector2(0.32f, 0.34f), 0.86f, 35);
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.72f, 0.68f), 1.00f, 42);
                    break;
            }
        }

        private Transform GetOrCreateDecorRoot()
        {
            Transform existing = transform.Find(proceduralDecorRootName);
            if (existing != null) return existing;

            var root = new GameObject(proceduralDecorRootName);
            root.transform.SetParent(transform, false);
            return root.transform;
        }

        private void PaintRoomLighting(LayoutKind layout, bool[,] floorMask)
        {
            if (!createProceduralLights) return;

            TuneGlobalLight();

            Transform root = GetOrCreateLightRoot();
            ClearChildren(root);

            foreach (RoomLightSpec spec in GetLightSpecs(layout))
            {
                Vector2Int preferred = new Vector2Int(
                    Mathf.RoundToInt(spec.normalizedPosition.x * (roomWidth - 1)),
                    Mathf.RoundToInt(spec.normalizedPosition.y * (roomHeight - 1)));

                if (!TryFindNearestFloorCell(floorMask, preferred, 18, out Vector2Int cell))
                    continue;

                Vector3 world = floorTilemap.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0));
                var lightObject = new GameObject("Room Light");
                lightObject.transform.SetParent(root, false);
                lightObject.transform.position = new Vector3(world.x, world.y, -1f);

                Light2D light = lightObject.AddComponent<Light2D>();
                light.lightType = Light2D.LightType.Point;
                light.color = spec.color;
                light.intensity = spec.intensity * localLightAccentScale;
                light.pointLightInnerRadius = spec.innerRadius;
                light.pointLightOuterRadius = spec.outerRadius;
                light.falloffIntensity = 0.78f;
                light.shadowsEnabled = false;
                ApplyLightToAllSortingLayers(light);

                if (spec.flicker)
                {
                    lightObject.AddComponent<RIMA.Environment.LightFlicker>();
                }

                var poolObject = new GameObject("Room Light Pool");
                poolObject.transform.SetParent(root, false);
                poolObject.transform.position = new Vector3(world.x, world.y, 0f);
                float poolAlpha = Mathf.Clamp(spec.intensity * localLightAccentScale * 0.13f, 0.07f, 0.22f);
                poolObject.AddComponent<RIMA.Environment.RoomMoodLightPool>()
                    .Configure(spec.color, spec.outerRadius * 0.82f, poolAlpha);
            }
        }

        private IEnumerable<RoomLightSpec> GetLightSpecs(LayoutKind layout)
        {
            Color torch = new Color(1f, 0.43f, 0.16f, 1f);
            Color ember = new Color(1f, 0.28f, 0.10f, 1f);
            Color cyan = new Color(0.18f, 0.72f, 1f, 1f);
            Color violet = new Color(0.45f, 0.32f, 0.95f, 1f);
            Color moon = new Color(0.25f, 0.36f, 0.52f, 1f);

            yield return new RoomLightSpec(new Vector2(0.50f, 0.78f), moon, 0.24f, 2.4f, 10.0f, false);

            switch (layout)
            {
                case LayoutKind.ShrineCrossroad:
                case LayoutKind.ReliquaryLoop:
                    yield return new RoomLightSpec(new Vector2(0.50f, 0.52f), cyan, 0.72f, 1.8f, 7.2f, false);
                    yield return new RoomLightSpec(new Vector2(0.24f, 0.70f), torch, 0.86f, 0.9f, 4.7f, true);
                    yield return new RoomLightSpec(new Vector2(0.76f, 0.30f), torch, 0.72f, 0.9f, 4.4f, true);
                    break;

                case LayoutKind.CryptBasin:
                case LayoutKind.RiftWell:
                case LayoutKind.BossAntechamber:
                    yield return new RoomLightSpec(new Vector2(0.50f, 0.52f), violet, 0.82f, 2.1f, 8.0f, false);
                    yield return new RoomLightSpec(new Vector2(0.30f, 0.68f), torch, 0.72f, 0.9f, 4.5f, true);
                    yield return new RoomLightSpec(new Vector2(0.70f, 0.68f), torch, 0.72f, 0.9f, 4.5f, true);
                    break;

                case LayoutKind.BrokenCauseway:
                case LayoutKind.CollapsedLibrary:
                case LayoutKind.NarrowApproach:
                    yield return new RoomLightSpec(new Vector2(0.22f, 0.58f), torch, 0.78f, 0.9f, 4.6f, true);
                    yield return new RoomLightSpec(new Vector2(0.74f, 0.46f), cyan, 0.58f, 1.2f, 5.6f, false);
                    yield return new RoomLightSpec(new Vector2(0.48f, 0.34f), ember, 0.42f, 1.0f, 4.2f, true);
                    break;

                case LayoutKind.AmbushCloister:
                case LayoutKind.ForkedOssuary:
                case LayoutKind.PillarArena:
                    yield return new RoomLightSpec(new Vector2(0.24f, 0.72f), torch, 0.72f, 0.9f, 4.5f, true);
                    yield return new RoomLightSpec(new Vector2(0.76f, 0.28f), torch, 0.72f, 0.9f, 4.5f, true);
                    yield return new RoomLightSpec(new Vector2(0.50f, 0.50f), cyan, 0.40f, 1.5f, 6.0f, false);
                    break;

                default:
                    yield return new RoomLightSpec(new Vector2(0.28f, 0.68f), torch, 0.76f, 0.9f, 4.6f, true);
                    yield return new RoomLightSpec(new Vector2(0.72f, 0.36f), torch, 0.68f, 0.9f, 4.3f, true);
                    yield return new RoomLightSpec(new Vector2(0.50f, 0.50f), cyan, 0.50f, 1.6f, 6.2f, false);
                    break;
            }
        }

        private void TuneGlobalLight()
        {
            Light2D[] lights = FindObjectsByType<Light2D>(FindObjectsSortMode.None);
            foreach (Light2D light in lights)
            {
                if (light.lightType != Light2D.LightType.Global) continue;
                if (!light.name.Contains("Global")) continue;

                light.intensity = globalLightIntensity;
                light.color = globalLightColor;
                ApplyLightToAllSortingLayers(light);
            }
        }

        private static void ApplyLightToAllSortingLayers(Light2D light)
        {
            if (light == null) return;

            SortingLayer[] layers = SortingLayer.layers;
            if (layers == null || layers.Length == 0) return;

            int[] layerIds = new int[layers.Length];
            for (int i = 0; i < layers.Length; i++)
            {
                layerIds[i] = layers[i].id;
            }

            light.targetSortingLayers = layerIds;
        }

        private Transform GetOrCreateLightRoot()
        {
            Transform existing = transform.Find(proceduralLightRootName);
            if (existing != null) return existing;

            var root = new GameObject(proceduralLightRootName);
            root.transform.SetParent(transform, false);
            return root.transform;
        }

        private static void ClearChildren(Transform root)
        {
            for (int i = root.childCount - 1; i >= 0; i--)
            {
                GameObject child = root.GetChild(i).gameObject;
                if (Application.isPlaying)
                    Object.Destroy(child);
                else
                    Object.DestroyImmediate(child);
            }
        }

        private bool TryFindNearestFloorCell(bool[,] floorMask, Vector2Int preferred, int maxRadius, out Vector2Int result)
        {
            for (int radius = 0; radius <= maxRadius; radius++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    for (int dy = -radius; dy <= radius; dy++)
                    {
                        if (Mathf.Abs(dx) != radius && Mathf.Abs(dy) != radius) continue;
                        int x = preferred.x + dx;
                        int y = preferred.y + dy;
                        if (!IsFloor(floorMask, x, y)) continue;

                        result = new Vector2Int(x, y);
                        return true;
                    }
                }
            }

            result = default;
            return false;
        }

        private void AddDoorSockets(bool[,] mask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            AddRect(mask, cx - 6, wallThickness + 1, 12, 18);
            AddRect(mask, cx - 6, roomHeight - wallThickness - 19, 12, 18);
            AddRect(mask, wallThickness + 1, cy - 5, 18, 10);
            AddRect(mask, roomWidth - wallThickness - 19, cy - 5, 18, 10);
        }

        private void EnsureCombatCore(bool[,] mask, int cx, int cy)
        {
            AddEllipse(mask, cx, cy, 24, 17);
            AddRect(mask, cx - 22, cy - 14, 44, 28);
        }

        private void EnsureTraversalSpine(bool[,] mask, int cx, int cy)
        {
            AddRect(mask, cx - 8, wallThickness + 1, 16, roomHeight - wallThickness * 2 - 2);
            AddRect(mask, wallThickness + 1, cy - 6, roomWidth - wallThickness * 2 - 2, 12);
            AddEllipse(mask, cx, cy, 30, 21);
        }

        private void CarveBrokenEdges(bool[,] mask, LayoutKind layout)
        {
            int salt = seed + (int)layout * 4099;
            for (int pass = 0; pass < 2; pass++)
            {
                for (int x = wallThickness + 1; x < roomWidth - wallThickness - 1; x++)
                {
                    for (int y = wallThickness + 1; y < roomHeight - wallThickness - 1; y++)
                    {
                        if (!mask[x, y]) continue;
                        if (IsNearCenter(x, y, 18)) continue;
                        if (CountMissingNeighbors(mask, x, y) < 4) continue;

                        int roll = Mathf.Abs((x * 92837111) ^ (y * 689287499) ^ salt ^ pass) % 100;
                        if (roll < 24)
                        {
                            mask[x, y] = false;
                        }
                    }
                }
            }
        }

        private int CountMissingNeighbors(bool[,] mask, int x, int y)
        {
            int count = 0;
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    if (!IsFloor(mask, x + dx, y + dy)) count++;
                }
            }
            return count;
        }

        private bool TouchesFloorWithin(bool[,] mask, int x, int y, int radius)
        {
            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    if (IsFloor(mask, x + dx, y + dy)) return true;
                }
            }
            return false;
        }

        private void AddRect(bool[,] mask, int x, int y, int width, int height)
        {
            for (int px = x; px < x + width; px++)
            {
                for (int py = y; py < y + height; py++)
                {
                    SetFloor(mask, px, py, true);
                }
            }
        }

        private void RemoveRect(bool[,] mask, int x, int y, int width, int height)
        {
            for (int px = x; px < x + width; px++)
            {
                for (int py = y; py < y + height; py++)
                {
                    SetFloor(mask, px, py, false);
                }
            }
        }

        private void AddEllipse(bool[,] mask, int cx, int cy, int rx, int ry)
        {
            PaintEllipse(mask, cx, cy, rx, ry, true);
        }

        private void RemoveEllipse(bool[,] mask, int cx, int cy, int rx, int ry)
        {
            PaintEllipse(mask, cx, cy, rx, ry, false);
        }

        private void PaintEllipse(bool[,] mask, int cx, int cy, int rx, int ry, bool value)
        {
            int xMin = cx - rx;
            int xMax = cx + rx;
            int yMin = cy - ry;
            int yMax = cy + ry;
            float invRx = 1f / Mathf.Max(1, rx);
            float invRy = 1f / Mathf.Max(1, ry);

            for (int x = xMin; x <= xMax; x++)
            {
                for (int y = yMin; y <= yMax; y++)
                {
                    float nx = (x - cx) * invRx;
                    float ny = (y - cy) * invRy;
                    if (nx * nx + ny * ny <= 1f)
                    {
                        SetFloor(mask, x, y, value);
                    }
                }
            }
        }

        private void SetFloor(bool[,] mask, int x, int y, bool value)
        {
            if (x < wallThickness || y < wallThickness) return;
            if (x >= roomWidth - wallThickness || y >= roomHeight - wallThickness) return;
            mask[x, y] = value;
        }

        private bool IsFloor(bool[,] mask, int x, int y)
        {
            return x >= 0 && y >= 0 && x < roomWidth && y < roomHeight && mask[x, y];
        }

        private bool IsNearCenter(int x, int y, int radius)
        {
            int dx = x - roomWidth / 2;
            int dy = y - roomHeight / 2;
            return dx * dx + dy * dy <= radius * radius;
        }

        private TileBase PickFloorTile(int x, int y)
        {
            int roll = Mathf.Abs((x * 73856093) ^ (y * 19349663) ^ seed) % 100;
            int index = roll switch
            {
                < 68 => 0,
                < 82 => Mathf.Min(1, cachedFloorTiles.Count - 1),
                < 92 => Mathf.Min(2, cachedFloorTiles.Count - 1),
                < 97 => Mathf.Min(3, cachedFloorTiles.Count - 1),
                _ => cachedFloorTiles.Count - 1,
            };

            return cachedFloorTiles[Mathf.Clamp(index, 0, cachedFloorTiles.Count - 1)];
        }

        private TileBase PickWallTile(int x, int y)
        {
            int roll = Mathf.Abs((x * 83492791) ^ (y * 297121507) ^ seed) % 100;
            int index = roll switch
            {
                < 72 => 0,
                < 88 => Mathf.Min(1, cachedWallTiles.Count - 1),
                < 96 => Mathf.Min(2, cachedWallTiles.Count - 1),
                _ => cachedWallTiles.Count - 1,
            };

            return cachedWallTiles[Mathf.Clamp(index, 0, cachedWallTiles.Count - 1)];
        }
    }
}
