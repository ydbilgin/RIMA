using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Gate = RIMA.Environment.Gate;
using FragmentDropAnchor = RIMA.Environment.FragmentDropAnchor;
using EnvironmentMapFragment = RIMA.Environment.MapFragment;
using MapFragmentSpawner = RIMA.Environment.MapFragmentSpawner;

namespace RIMA.Systems.Map
{
    using RoomType = global::RIMA.RoomType;

    public class RoomLoader : MonoBehaviour
    {
        public static event Action<RoomConfig, GameObject> OnRoomLoaded;
        public static event Action OnRoomCleared;
        public static event Action<int> OnRoomChanged;
        public static event Action OnDemoComplete;

        public static int CurrentRoomIndex { get; private set; } = 0;

        [SerializeField] private RoomRegistry registry;
        [SerializeField] private Grid baseGrid;
        [SerializeField] private RoomSequenceData[] _sequence;
        [SerializeField] private bool autoStart = true;

        private GameObject _currentInstance;
        private GameObject _currentRoomContent;

        public static void RaiseDemoComplete() => OnDemoComplete?.Invoke();

        private void Start()
        {
            if (autoStart) LoadFirstRoom();
        }

        public void Load(RoomType type, int depth)
        {
            if (_currentInstance != null) Unload();

            GameObject prefab = registry.GetRandom(type, depth);
            if (prefab == null) { Debug.LogError($"[RoomLoader] No prefab for {type} depth {depth}"); return; }

            _currentInstance = Instantiate(prefab, Vector3.zero, Quaternion.identity);

            RoomConfig config = _currentInstance.GetComponent<RoomConfig>();
            if (config == null) { Debug.LogError("[RoomLoader] Prefab missing RoomConfig"); Destroy(_currentInstance); return; }

            ValidateContract(config);
            OnRoomLoaded?.Invoke(config, _currentInstance);
        }

        public void Unload()
        {
            if (_currentInstance == null) return;
            Destroy(_currentInstance);
            _currentInstance = null;
            OnRoomCleared?.Invoke();
        }

        public static void LoadNext()
        {
            RoomLoader loader = FindFirstObjectByType<RoomLoader>();
            if (loader == null)
            {
                Debug.LogError("[RoomLoader] No instance in scene.");
                return;
            }

            loader.LoadNextInstance();
        }

        public void LoadFirstRoom()
        {
            if (!TryGetRoomData(0, out RoomSequenceData firstData)) return;

            TeardownCurrentRoom();
            TeleportPlayer(firstData.playerStartPos);
            BuildRoomContent(firstData);

            CurrentRoomIndex = 0;
            OnRoomChanged?.Invoke(CurrentRoomIndex);
            SetHudRoomStatus(firstData);
        }

        private void LoadNextInstance()
        {
            if (_sequence == null || _sequence.Length == 0)
            {
                Debug.LogError("[RoomLoader] Room sequence is empty.");
                return;
            }

            if (CurrentRoomIndex >= _sequence.Length - 1)
            {
                Debug.LogWarning("[RoomLoader] LoadNext called on final room — ignoring.");
                return;
            }

            int nextIndex = CurrentRoomIndex + 1;
            if (!TryGetRoomData(nextIndex, out RoomSequenceData nextData)) return;

            PlayerController pc = FindFirstObjectByType<PlayerController>();
            if (pc != null) pc.enabled = false;

            if (RoomTransitionFX.Instance != null)
            {
                RoomTransitionFX.Instance.DoTransition(() => SwapRoomWhileBlack(nextIndex, nextData));
            }
            else
            {
                SwapRoomWhileBlack(nextIndex, nextData);
            }

            StartCoroutine(ReenableAfterFade(pc, nextData));
        }

        private bool TryGetRoomData(int index, out RoomSequenceData data)
        {
            data = null;
            if (_sequence == null || index < 0 || index >= _sequence.Length)
            {
                Debug.LogError($"[RoomLoader] Missing room sequence entry at index {index}.");
                return false;
            }

            data = _sequence[index];
            if (data == null)
            {
                Debug.LogError($"[RoomLoader] Room sequence entry {index} is null.");
                return false;
            }

            return true;
        }

        private void SwapRoomWhileBlack(int nextIndex, RoomSequenceData nextData)
        {
            TeardownCurrentRoom();
            TeleportPlayer(nextData.playerStartPos);
            BuildRoomContent(nextData);

            CurrentRoomIndex = nextIndex;
            OnRoomChanged?.Invoke(nextIndex);
        }

        private void TeardownCurrentRoom()
        {
            if (_currentRoomContent != null)
            {
                Destroy(_currentRoomContent);
                _currentRoomContent = null;
            }

            foreach (Gate gate in FindObjectsByType<Gate>(FindObjectsSortMode.None))
            {
                if (gate != null) Destroy(gate.gameObject);
            }

            foreach (EnvironmentMapFragment fragment in FindObjectsByType<EnvironmentMapFragment>(FindObjectsSortMode.None))
            {
                if (fragment != null) Destroy(fragment.gameObject);
            }
        }

        private static void TeleportPlayer(Vector3 position)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.position = position;
            else player.transform.position = position;
        }

        private void BuildRoomContent(RoomSequenceData data)
        {
            _currentRoomContent = new GameObject($"RoomContent_{data.roomIndex}_{data.displayName}");
            int remainingMobs = 0;
            bool roomClearedRaised = false;

            if (!data.isRewardRoom && data.mobSpawns != null)
            {
                foreach (RoomSequenceData.EnemySpawnEntry entry in data.mobSpawns)
                {
                    if (entry == null || entry.prefab == null) continue;

                    GameObject mob = Instantiate(entry.prefab, entry.position, Quaternion.identity, _currentRoomContent.transform);
                    if (entry.isElite) EliteAffix.Apply(mob, EliteAffix.RandomAffix());

                    Health health = mob.GetComponentInChildren<Health>();
                    if (health == null) continue;

                    remainingMobs++;
                    bool counted = false;
                    if (health.OnDeath == null) health.OnDeath = new UnityEngine.Events.UnityEvent();
                    health.OnDeath.AddListener(() =>
                    {
                        if (counted) return;
                        counted = true;
                        remainingMobs = Mathf.Max(0, remainingMobs - 1);
                        if (remainingMobs == 0 && !roomClearedRaised)
                        {
                            roomClearedRaised = true;
                            OnRoomCleared?.Invoke();
                        }
                    });
                }
            }

            if (data.focalElementPrefab != null)
            {
                Instantiate(data.focalElementPrefab, data.focalElementPos, Quaternion.identity, _currentRoomContent.transform);
            }

            GameObject gateGO = new GameObject($"Gate_Room{data.roomIndex}_Exit");
            gateGO.transform.position = data.gatePosition;
            gateGO.transform.SetParent(_currentRoomContent.transform);
            gateGO.AddComponent<SpriteRenderer>();
            BoxCollider2D col = gateGO.AddComponent<BoxCollider2D>();
            col.size = data.gateSize == Vector2.zero ? new Vector2(1.5f, 2f) : data.gateSize;
            gateGO.AddComponent<Gate>();

            if (data.fragmentDropOverride != Vector3.zero)
            {
                GameObject anchorGO = new GameObject("FragmentDropAnchor");
                anchorGO.transform.position = data.fragmentDropOverride;
                anchorGO.transform.SetParent(_currentRoomContent.transform);
                anchorGO.AddComponent<FragmentDropAnchor>();
            }

            if (data.isRewardRoom)
            {
                StartCoroutine(RewardRoomAutoTrigger(data));
            }

            if (data.isBossRoom)
            {
                StartCoroutine(WireBossDeathListener(_currentRoomContent));
            }
        }

        private IEnumerator RewardRoomAutoTrigger(RoomSequenceData data)
        {
            yield return new WaitForSeconds(2f);

            FragmentDropAnchor anchor = FindFirstObjectByType<FragmentDropAnchor>();
            if (anchor == null) yield break;

            MapFragmentSpawner spawner = FindFirstObjectByType<MapFragmentSpawner>();
            spawner?.SendMessage("HandleRoomCleared", SendMessageOptions.DontRequireReceiver);
        }

        private IEnumerator WireBossDeathListener(GameObject roomContent)
        {
            yield return null;

            Health bossHealth = roomContent != null ? roomContent.GetComponentInChildren<Health>() : null;
            if (bossHealth == null) yield break;

            if (bossHealth.OnDeath == null) bossHealth.OnDeath = new UnityEngine.Events.UnityEvent();
            bossHealth.OnDeath.AddListener(RaiseDemoComplete);
        }

        private IEnumerator ReenableAfterFade(PlayerController pc, RoomSequenceData data)
        {
            yield return new WaitUntil(() => RoomTransitionFX.Instance == null || !RoomTransitionFX.Instance.IsFading);
            if (pc != null) pc.enabled = true;
            SetHudRoomStatus(data);
        }

        private static void SetHudRoomStatus(RoomSequenceData data)
        {
            HUDController hud = FindFirstObjectByType<HUDController>();
            hud?.SetRoomStatus($"Room {data.roomIndex + 1}/5 — {data.displayName}");
        }

        private void ValidateContract(RoomConfig config)
        {
            if (baseGrid == null) return;
            if (config.cellSize != baseGrid.cellSize)
                Debug.LogWarning($"[RoomLoader] cellSize mismatch: {config.cellSize} vs {baseGrid.cellSize}");
            if (config.gridLayout != baseGrid.cellLayout)
                Debug.LogWarning($"[RoomLoader] gridLayout mismatch: {config.gridLayout} vs {baseGrid.cellLayout}");
        }
    }
}
