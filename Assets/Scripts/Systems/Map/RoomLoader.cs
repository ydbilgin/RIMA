// ⚠️ LEGACY (2026-06-07): Bu sınıf CANLI demo yolunda DEĞİL (kanıt: STAGING/audit/LIVE_FLOW_PROOF_2026-06-07.md).
// Canlı yol: _Arena → RoomRunDirector → IsoRoomBuilder.BuildExitDoors. Yeni iş BURAYA BAĞLANMAZ.
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
        private FragmentDropAnchor _currentFragmentDropAnchor;
        private Action _clearToUnlockHandler;

        public static void RaiseDemoComplete() => OnDemoComplete?.Invoke();

        private void Start()
        {
            ValidateContract(null);
            if (autoStart) LoadFirstRoom();
        }

        private void OnDestroy()
        {
            // Scene reload (death / demo-complete) destroys RoomLoader without a room swap —
            // drop the static fragment-pickup subscriber so it can't leak across the reload (A3 review).
            ClearPendingFragmentPickup();
            ClearPendingRoomClearUnlock();
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
            ClearPendingRoomClearUnlock();
            _currentFragmentDropAnchor = null;

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

            if (data.decorProps != null)
            {
                foreach (RoomSequenceData.DecorProp d in data.decorProps)
                {
                    if (d == null || d.prefab == null) continue;
                    Vector3 worldPos = _currentRoomContent.transform.TransformPoint(d.localPosition);
                    Instantiate(d.prefab, worldPos, Quaternion.identity, _currentRoomContent.transform);
                }
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
            Vector2 gateSize = data.gateSize == Vector2.zero ? new Vector2(1.5f, 2f) : data.gateSize;

            // ── Trigger collider lives on the gate ROOT, UNSCALED, centred on gatePosition. ──
            // The visual arch scale must NOT touch this collider, or the trigger's world size/centre
            // would drift off the authored gatePosition (the bug this fixes). Root stays scale 1,
            // col.size == gateSize, col.offset == 0 → world trigger is exactly gateSize at gatePosition.
            BoxCollider2D col = gateGO.AddComponent<BoxCollider2D>();
            col.size = gateSize;
            // (no col.offset, no root localScale — trigger world size + centre == pre-gate-visual behaviour)

            // Gate requires a SpriteRenderer on its own GO (RequireComponent) and Gate.Awake builds a
            // grey placeholder when that SR has no sprite. We keep the root SR for Gate's state machine
            // (tint/alpha/squash) but DISABLE its renderer so the placeholder never draws — the real
            // on-brand art is a scaled child below. Gate only touches SR.color + col.enabled, never
            // SR.enabled, so disabling here is safe and leaves Gate's logic untouched.
            SpriteRenderer rootSr = gateGO.AddComponent<SpriteRenderer>();
            rootSr.enabled = false;
            Gate gate = gateGO.AddComponent<Gate>();

            // ── Visual child carries the arch scale + offset; root + collider stay put. ──
            // On-brand visual: cyan-rift stone arch on a "GateVisual" child SpriteRenderer.
            GameObject visualGO = new GameObject("GateVisual");
            visualGO.transform.SetParent(gateGO.transform, false);
            SpriteRenderer archSr = visualGO.AddComponent<SpriteRenderer>();
            Sprite archSprite = LoadLargestSprite("Environment/Gate/gate_arch");
            // Arch sub-sprite has a bottom-left pivot; track its scaled span so the arch is recentred
            // onto gatePosition (centre-X, base-Y of the visible arch) and the barrier can nest inside.
            float archScale = 1f;
            Vector2 archUnits = Vector2.zero;
            if (archSprite != null)
            {
                archSr.sprite = archSprite;
                archUnits = archSprite.bounds.size;                  // world units at the sprite's PPU
                if (archUnits.y > 0.01f)
                {
                    archScale = gateSize.y / archUnits.y;            // uniform fit to target height (no distortion)
                    visualGO.transform.localScale = Vector3.one * archScale;
                }
                // Recentre the bottom-left-pivot arch so its centre-X / base-Y sits on gatePosition.
                // localPosition is in the parent (root, unscaled) space → use scaled world spans directly.
                visualGO.transform.localPosition = new Vector3(-archUnits.x * 0.5f * archScale, 0f, 0f);
            }

            // Custom-Axis Y-sort: layer "Entities", order 0, sort by Pivot (no manual sortingOrder bump).
            archSr.sortingLayerName = "Entities";
            archSr.sortingOrder = 0;
            archSr.spriteSortPoint = SpriteSortPoint.Pivot;

            // Sealed energy barrier: rift-fracture overlay tinted cyan, sitting in the arch opening.
            // Visible while locked/sealed; hidden when the gate unlocks (via Gate.OnUnlocked hook).
            // Nested UNDER GateVisual so it inherits the arch scale (its localPosition is in arch space).
            Sprite barrierSprite = LoadLargestSprite("Environment/Gate/gate_seal_barrier");
            if (barrierSprite != null)
            {
                GameObject barrierGO = new GameObject("SealBarrier");
                barrierGO.transform.SetParent(visualGO.transform, false);
                Vector2 barrierUnits = barrierSprite.bounds.size;
                // Centre the bottom-left-pivot overlay in the arch opening (arch centre-X, sitting up
                // from the base). localPosition is in the parent (GateVisual) local, pre-scale space.
                barrierGO.transform.localPosition = new Vector3(
                    archUnits.x * 0.5f - barrierUnits.x * 0.5f,
                    (gateSize.y * 0.30f) / Mathf.Max(archScale, 0.0001f),
                    0f);
                SpriteRenderer barrierSr = barrierGO.AddComponent<SpriteRenderer>();
                barrierSr.sprite = barrierSprite;
                barrierSr.color = new Color(0f, 1f, 0.8f, 0.7f); // cyan #00FFCC, alpha ~0.7
                barrierSr.sortingLayerName = "Entities";
                barrierSr.sortingOrder = 0;
                barrierSr.spriteSortPoint = SpriteSortPoint.Pivot;

                // Hide the barrier when the gate unlocks (one-shot; cleans itself up).
                Action<Gate> onUnlocked = null;
                onUnlocked = g =>
                {
                    g.OnUnlocked -= onUnlocked;
                    if (barrierGO != null) barrierGO.SetActive(false);
                };
                gate.OnUnlocked += onUnlocked;
            }

            // Wire gate-entered → LoadNext (one-shot: unsubscribe after first entry to prevent double-trigger).
            Action<Gate> onEntered = null;
            onEntered = _ =>
            {
                gate.OnPlayerEntered -= onEntered;
                LoadNext();
            };
            gate.OnPlayerEntered += onEntered;

            _currentFragmentDropAnchor = CreateFragmentDropAnchor(data);

            if (data.isRewardRoom)
            {
                // Reward room: wait 2s, spawn fragment, listen for pickup → unlock gate.
                StartCoroutine(RewardRoomAutoTrigger(data, gate));
            }
            else
            {
                // Combat room: mobs dead → drop a fragment; pickup → draft → gate unlock (B3 collect loop).
                ClearPendingRoomClearUnlock();
                _clearToUnlockHandler = () =>
                {
                    ClearPendingRoomClearUnlock();
                    Debug.Log($"[RoomLoader] Room {data.roomIndex} cleared — spawning fragment before draft/unlock.");
                    SpawnFragmentThenDraftUnlock(gate);
                };
                OnRoomCleared += _clearToUnlockHandler;
            }
        }

        private FragmentDropAnchor CreateFragmentDropAnchor(RoomSequenceData data)
        {
            GameObject anchorGO = new GameObject("FragmentDropAnchor");
            anchorGO.transform.position = ResolveFragmentDropPosition(data);
            anchorGO.transform.SetParent(_currentRoomContent.transform);
            return anchorGO.AddComponent<FragmentDropAnchor>();
        }

        private static Vector3 ResolveFragmentDropPosition(RoomSequenceData data)
        {
            if (data.fragmentDropOverride != Vector3.zero) return data.fragmentDropOverride;
            return Vector3.Lerp(data.playerStartPos, data.gatePosition, 0.5f);
        }

        // Gate art is imported in Multiple sprite-mode (auto-sliced), so Resources.Load<Sprite> can
        // return an arbitrary sub-sprite. LoadAll + pick-largest reliably grabs the main piece.
        // Returns null if the resource is missing (caller falls back to the placeholder behaviour).
        private static Sprite LoadLargestSprite(string resourcePath)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(resourcePath);
            if (sprites == null || sprites.Length == 0) return null;
            Sprite best = null;
            float bestArea = -1f;
            foreach (Sprite s in sprites)
            {
                if (s == null) continue;
                float area = s.rect.width * s.rect.height;
                if (area > bestArea) { bestArea = area; best = s; }
            }
            return best;
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
        // pickup → draft → gate unlock. Drop position = the room's FragmentDropAnchor.
        // Combat rooms without an authored override get a deterministic midpoint anchor
        // between player start and gate instead of hiding the drop under the player.
        // Direct draft/unlock fallback only if no anchor exists, so the gate never softlocks.
        // LOCK: RoomLoader is the SINGLE fragment-spawn authority — MapFragmentSpawner is a
        // passive prefab helper, driven only from here (no auto-subscribe → no double-spawn).
        private void SpawnFragmentThenDraftUnlock(Gate gate)
        {
            ClearPendingFragmentPickup(); // defensive: drop any stale subscriber

            // Reward must drop where the player can SEE it. The old midpoint(playerStart, gate)
            // anchor dropped the fragment off the TOP of the follow-camera view, so it read as
            // "no reward appeared". Drop at the player's CURRENT position (where the fight ended),
            // nudged a little toward the gate so it isn't under-foot and leads toward the exit.
            // An authored fragmentDropOverride still wins; anchor/player are the fallbacks.
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 dropPos;
            if (CurrentRoomData != null && CurrentRoomData.fragmentDropOverride != Vector3.zero)
            {
                dropPos = CurrentRoomData.fragmentDropOverride;
            }
            else if (player != null)
            {
                Vector3 p = player.transform.position;
                Vector3 toGate = gate != null ? (gate.transform.position - p) : Vector3.up;
                Vector3 dir = toGate.sqrMagnitude > 0.01f ? toGate.normalized : Vector3.up;
                dropPos = p + dir * 2.0f; // ahead of the player (clear of the body), on-screen + reachable
            }
            else if (_currentFragmentDropAnchor != null)
            {
                dropPos = _currentFragmentDropAnchor.transform.position;
            }
            else
            {
                Debug.LogWarning("[RoomLoader] No player/anchor — draft/unlock fallback (no fragment).");
                StartCoroutine(UnlockGateAfterDraft(gate));
                return;
            }

            // Single deterministic spawn at the visible position. We do NOT route through
            // MapFragmentSpawner.SendMessage/FindFirstObjectByType anymore — that re-find could
            // grab a stale scene anchor (e.g. a leftover root PortalSpawnAnchor) and hide the
            // reward off-room. RoomLoader stays the single spawn authority (one fragment).
            if (FindFirstObjectByType<EnvironmentMapFragment>() == null)
            {
                MapFragmentSpawner spawner = FindFirstObjectByType<MapFragmentSpawner>();
                EnvironmentMapFragment prefab = spawner != null ? spawner.fragmentPrefab : null;
                if (prefab != null)
                {
                    Instantiate(prefab, dropPos, Quaternion.identity);
                }
                else
                {
                    var go = new GameObject("MapFragment_Drop");
                    go.transform.position = dropPos;
                    go.AddComponent<EnvironmentMapFragment>();
                }
                Debug.Log($"[RoomLoader] Reward fragment dropped at {dropPos} (player-relative, on-screen).");
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

        private void ClearPendingRoomClearUnlock()
        {
            if (_clearToUnlockHandler != null)
            {
                OnRoomCleared -= _clearToUnlockHandler;
                _clearToUnlockHandler = null;
            }
        }

        private IEnumerator WireBossDeathListener(GameObject roomContent)
        {
            // fix: poll for the boss Health over ~0.5s instead of a single frame.
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
            Grid grid = ResolveBaseGrid();
            if (grid == null) return;

            Vector3 cellSize = config != null && config.cellSize != Vector3.zero
                ? config.cellSize
                : RoomConfig.IsoCellSize;
            GridLayout.CellLayout layout = config != null && config.gridLayout == RoomConfig.IsoGridLayout
                ? config.gridLayout
                : RoomConfig.IsoGridLayout;

            grid.cellLayout = layout;
            grid.cellSize = cellSize;
        }

        private Grid ResolveBaseGrid()
        {
            if (baseGrid != null) return baseGrid;

            GameObject go = GameObject.Find("IsoGrid/Ground")
                ?? GameObject.Find("Room/Floor")
                ?? GameObject.Find("Grid/BaseTilemap")
                ?? GameObject.Find("BaseTilemap")
                ?? GameObject.Find("Floor");

            if (go == null) return null;

            Grid grid = go.GetComponent<Grid>() ?? go.GetComponentInParent<Grid>();
            if (grid == null)
            {
                Tilemap tilemap = go.GetComponent<Tilemap>() ?? go.GetComponentInChildren<Tilemap>();
                if (tilemap != null) grid = tilemap.GetComponentInParent<Grid>();
            }

            if (grid != null) baseGrid = grid;
            return grid;
        }
    }
}
