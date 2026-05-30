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

        public static bool DraftDrivenByRoomLoader = true;
        public static int CurrentRoomIndex { get; private set; } = 0;

        /// <summary>The RoomSequenceData for the room currently loaded. Null before first load.</summary>
        public static RoomSequenceData CurrentRoomData { get; private set; }

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

        public static void JumpToRoom(int i)
        {
            FindFirstObjectByType<RoomLoader>()?.LoadRoomByIndex(i);
        }

        public static void DebugForceRoomCleared()
        {
            OnRoomCleared?.Invoke();
        }

        public void LoadFirstRoom()
        {
            if (!TryGetRoomData(0, out RoomSequenceData firstData)) return;

            TeardownCurrentRoom();
            TeleportPlayer(firstData.playerStartPos);
            BuildRoomContent(firstData);

            CurrentRoomIndex = 0;
            CurrentRoomData = firstData;
            OnRoomChanged?.Invoke(CurrentRoomIndex);
            SetHudRoomStatus(firstData);
        }

        public void LoadRoomByIndex(int index)
        {
            if (!TryGetRoomData(index, out RoomSequenceData roomData)) return;

            PlayerController pc = FindFirstObjectByType<PlayerController>();
            if (pc != null) pc.enabled = false;

            if (RoomTransitionFX.Instance != null)
            {
                RoomTransitionFX.Instance.DoTransition(() => SwapRoomWhileBlack(index, roomData));
            }
            else
            {
                SwapRoomWhileBlack(index, roomData);
            }

            StartCoroutine(ReenableAfterFade(pc, roomData));
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
            CurrentRoomData = nextData;
            OnRoomChanged?.Invoke(nextIndex);
        }

        private void TeardownCurrentRoom()
        {
            // Drop any pending fragment-pickup subscriber before destroying fragments,
            // so a not-yet-collected fragment can't unlock the next room's gate (cx A3 issue 3).
            ClearPendingFragmentPickup();

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

            // Boss room has no gate — boss death fires RaiseDemoComplete directly.
            if (data.isBossRoom)
            {
                StartCoroutine(WireBossDeathListener(_currentRoomContent));
                return;
            }

            // Build gate for non-boss rooms (visible + locked from the start).
            GameObject gateGO = new GameObject($"Gate_Room{data.roomIndex}_Exit");
            gateGO.transform.position = data.gatePosition;
            gateGO.transform.SetParent(_currentRoomContent.transform);
            gateGO.AddComponent<SpriteRenderer>();
            BoxCollider2D col = gateGO.AddComponent<BoxCollider2D>();
            col.size = data.gateSize == Vector2.zero ? new Vector2(1.5f, 2f) : data.gateSize;
            Gate gate = gateGO.AddComponent<Gate>();

            // Wire gate-entered → LoadNext (one-shot: unsubscribe after first entry to prevent double-trigger).
            Action<Gate> onEntered = null;
            onEntered = _ =>
            {
                gate.OnPlayerEntered -= onEntered;
                LoadNext();
            };
            gate.OnPlayerEntered += onEntered;

            if (data.fragmentDropOverride != Vector3.zero)
            {
                GameObject anchorGO = new GameObject("FragmentDropAnchor");
                anchorGO.transform.position = data.fragmentDropOverride;
                anchorGO.transform.SetParent(_currentRoomContent.transform);
                anchorGO.AddComponent<FragmentDropAnchor>();
            }

            if (data.isRewardRoom)
            {
                // Reward room: wait 2s, spawn fragment, listen for pickup → unlock gate.
                StartCoroutine(RewardRoomAutoTrigger(data, gate));
            }
            else
            {
                // Combat room: mobs dead → drop a fragment; pickup → draft → gate unlock (B3 collect loop).
                Action clearToUnlock = null;
                clearToUnlock = () =>
                {
                    OnRoomCleared -= clearToUnlock;
                    Debug.Log($"[RoomLoader] Room {data.roomIndex} cleared — spawning fragment before draft/unlock.");
                    SpawnFragmentThenDraftUnlock(gate);
                };
                OnRoomCleared += clearToUnlock;
            }
        }

        private IEnumerator UnlockGateAfterDraft(Gate gate)
        {
            if (DraftManager.Instance != null)
            {
                DraftManager.Instance.ShowDraft();
                yield return null;
                while (DraftManager.Instance != null && DraftManager.Instance.IsDraftActive)
                    yield return null;
            }

            if (gate != null) gate.Unlock();
        }

        private IEnumerator RewardRoomAutoTrigger(RoomSequenceData data, Gate rewardGate)
        {
            yield return new WaitForSeconds(2f);
            SpawnFragmentThenDraftUnlock(rewardGate);
        }

        // Tracks the active one-shot fragment-pickup subscriber so TeardownCurrentRoom can
        // unsubscribe it if the room is swapped before pickup — prevents a stale static-event
        // subscriber from unlocking the wrong room's gate (cx review A3, issue 3).
        private Action<EnvironmentMapFragment> _pendingFragmentPickup;

        // Shared fragment flow (combat + reward rooms): drop one fragment, then
        // pickup → draft → gate unlock. Drop position = the room's FragmentDropAnchor when it
        // authored one (reward rooms), else the player's feet (combat assets have
        // fragmentDropOverride == 0 → no anchor) which is provably reachable, so the player
        // can always collect it. Direct draft/unlock fallback only if there is no anchor AND
        // no player, so the gate ALWAYS unlocks (never softlocks).
        // LOCK: RoomLoader is the SINGLE fragment-spawn authority — MapFragmentSpawner is a
        // passive prefab helper, driven only from here (no auto-subscribe → no double-spawn).
        private void SpawnFragmentThenDraftUnlock(Gate gate)
        {
            ClearPendingFragmentPickup(); // defensive: drop any stale subscriber

            FragmentDropAnchor anchor = FindFirstObjectByType<FragmentDropAnchor>();
            Vector3 dropPos;
            if (anchor != null)
            {
                dropPos = anchor.transform.position;
            }
            else
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player == null)
                {
                    Debug.LogWarning("[RoomLoader] No anchor and no player — draft/unlock fallback (no fragment).");
                    StartCoroutine(UnlockGateAfterDraft(gate));
                    return;
                }
                dropPos = player.transform.position;
            }

            // Prefer the prefab-based spawner for visuals when an anchor exists; guarantee a fragment regardless.
            if (anchor != null)
            {
                MapFragmentSpawner spawner = FindFirstObjectByType<MapFragmentSpawner>();
                if (spawner != null)
                    spawner.SendMessage("HandleRoomCleared", SendMessageOptions.DontRequireReceiver);
            }

            if (FindFirstObjectByType<EnvironmentMapFragment>() == null)
            {
                var go = new GameObject("MapFragment_Drop");
                go.transform.position = dropPos;
                go.AddComponent<EnvironmentMapFragment>();
            }

            // Fragment pickup → draft → gate unlock (one-shot; tracked for teardown-safe unsubscribe).
            _pendingFragmentPickup = _ =>
            {
                ClearPendingFragmentPickup();
                if (gate != null)
                {
                    StartCoroutine(UnlockGateAfterDraft(gate));
                    Debug.Log("[RoomLoader] Fragment picked up — draft started before gate unlock.");
                }
            };
            EnvironmentMapFragment.OnAnyFragmentPickedUp += _pendingFragmentPickup;
        }

        private void ClearPendingFragmentPickup()
        {
            if (_pendingFragmentPickup != null)
            {
                EnvironmentMapFragment.OnAnyFragmentPickedUp -= _pendingFragmentPickup;
                _pendingFragmentPickup = null;
            }
        }

        private IEnumerator WireBossDeathListener(GameObject roomContent)
        {
            // Codex #2 fix: poll for the boss Health over ~0.5s instead of a single frame.
            // If Health is on a child added in Start(), a one-frame wait misses it and DemoComplete never fires (win softlock).
            Health bossHealth = null;
            for (int i = 0; i < 30 && bossHealth == null; i++)
            {
                bossHealth = roomContent != null ? roomContent.GetComponentInChildren<Health>(true) : null;
                if (bossHealth == null) yield return null;
            }
            if (bossHealth == null)
            {
                Debug.LogWarning("[RoomLoader] Boss Health not found after 30 frames — DemoComplete won't fire.");
                yield break;
            }

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
