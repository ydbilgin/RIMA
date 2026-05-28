// C10 — LiveRoomReloader (F5).
// Self-bootstraps via RuntimeInitializeOnLoadMethod — NO scene edits required.
// Subscribes to RoomLoader.OnRoomLoaded static event.
// Uses JsonFileWatcher (C11) to detect room_current.json changes.
// Applies a prop/tile diff on the main thread (< 100 ms target).
//
// Active ONLY in Development Builds and Editor (compile-guard at bottom).
// PlayableArena scene is NOT modified by this file.

#if DEVELOPMENT_BUILD || UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using RIMA.Systems.Map;

namespace RIMA.Live
{
    /// <summary>
    /// Listens for JSON file changes and live-reloads the active room's
    /// tilemap + prop instances without reloading the scene.
    ///
    /// Bootstrap: added as a component to a DontDestroyOnLoad GameObject
    /// automatically at game start — attach nothing manually.
    /// </summary>
    public sealed class LiveRoomReloader : MonoBehaviour
    {
        // ── Bootstrap ──────────────────────────────────────────────────────────

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            GameObject host = new GameObject("[LiveRoomReloader]");
            DontDestroyOnLoad(host);
            host.AddComponent<LiveRoomReloader>();
            UnityEngine.Debug.Log("[LiveRoomReloader] Bootstrapped (Development Build).");
        }

        // ── Config ─────────────────────────────────────────────────────────────

        /// <summary>Path watched for JSON changes. Matches RoomLayoutSerializer.CurrentJsonPath.</summary>
        private static string JsonPath =>
            Path.Combine(Application.streamingAssetsPath, "live", "room_current.json");

        // ── State ──────────────────────────────────────────────────────────────

        private JsonFileWatcher _watcher;
        private RuntimeAssetRegistry _registry;

        // Runtime room state — updated on each reload / room load.
        private GameObject _roomRoot;          // parent for all live-managed objects
        private Tilemap    _floorTilemap;
        private Tilemap    _cliffTilemap;

        // Active prop instances keyed by a stable id (prefab_guid + index in JSON list).
        private readonly Dictionary<string, GameObject> _activeProps =
            new Dictionary<string, GameObject>(StringComparer.Ordinal);

        // Last snapshot for diffing.
        private RoomLayoutData _lastData;

        // ── MonoBehaviour lifecycle ────────────────────────────────────────────

        private void Awake()
        {
            _registry = RuntimeAssetRegistry.Instance;
            if (_registry == null)
                UnityEngine.Debug.LogWarning("[LiveRoomReloader] RuntimeAssetRegistry not found — asset resolution will be skipped. Bake the registry first (RIMA → Live Tool → Bake Registry).");

            // Subscribe to RoomLoader static event (peer component, no scene edit).
            RoomLoader.OnRoomLoaded += HandleRoomLoaded;
            RoomLoader.OnRoomCleared += HandleRoomCleared;

            // Set up file watcher.
            _watcher = gameObject.AddComponent<JsonFileWatcher>();
            _watcher.OnFileChanged += HandleFileChanged;

            if (File.Exists(JsonPath))
            {
                _watcher.StartWatching(JsonPath);
            }
            else
            {
                UnityEngine.Debug.Log($"[LiveRoomReloader] JSON not yet present at {JsonPath}. Watcher will start on first room load.");
            }
        }

        private void OnDestroy()
        {
            RoomLoader.OnRoomLoaded  -= HandleRoomLoaded;
            RoomLoader.OnRoomCleared -= HandleRoomCleared;
        }

        // ── RoomLoader event handlers ──────────────────────────────────────────

        private void HandleRoomLoaded(RoomConfig config, GameObject roomInstance)
        {
            // Discover tilemaps in the newly loaded room.
            _floorTilemap = null;
            _cliffTilemap = null;

            foreach (Tilemap tm in roomInstance.GetComponentsInChildren<Tilemap>(true))
            {
                string n = tm.gameObject.name.ToLowerInvariant();
                if (_floorTilemap == null && n.Contains("floor"))  _floorTilemap = tm;
                if (_cliffTilemap == null && n.Contains("cliff"))  _cliffTilemap = tm;
            }

            // Create/reset live-managed root under the room instance.
            Transform existing = roomInstance.transform.Find("[LiveProps]");
            if (existing != null) Destroy(existing.gameObject);
            _roomRoot = new GameObject("[LiveProps]");
            _roomRoot.transform.SetParent(roomInstance.transform, false);

            // Clear prop table — new room, fresh state.
            _activeProps.Clear();
            _lastData = null;

            // Start watcher if JSON exists now.
            if (File.Exists(JsonPath) && _watcher != null)
                _watcher.StartWatching(JsonPath);

            // Apply current JSON if present.
            if (File.Exists(JsonPath))
                ApplyJsonFile();
        }

        private void HandleRoomCleared()
        {
            // Prop table cleaned when next HandleRoomLoaded fires.
        }

        // ── File change handler (main thread — fired by JsonFileWatcher.Update) ─

        private void HandleFileChanged()
        {
            ApplyJsonFile();
        }

        // ── Core reload logic ──────────────────────────────────────────────────

        private void ApplyJsonFile()
        {
            // Read file.
            string json;
            try { json = File.ReadAllText(JsonPath); }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"[LiveRoomReloader] Read failed: {ex.Message}");
                return;
            }

            // Parse.
            RoomLayoutData data = RoomLayoutData.FromJson(json);
            if (data == null) return; // error logged inside FromJson

            Stopwatch sw = Stopwatch.StartNew();

            // ── Tilemap reload ─────────────────────────────────────────────────
            ApplyFloorTiles(data);
            ApplyCliffTiles(data);

            // ── Prop diff ─────────────────────────────────────────────────────
            ApplyPropDiff(data);

            // ── Player safety ─────────────────────────────────────────────────
            SnapPlayerIfNeeded();

            sw.Stop();

            int added   = 0, removed = 0, moved = 0;
            CountDiff(_lastData, data, out added, out removed, out moved);

            _lastData = data;

            UnityEngine.Debug.Log(
                $"[LiveRoomReloader] Reload applied in {sw.ElapsedMilliseconds}ms — " +
                $"Δ{added}+ / {removed}- / {moved}~"
            );
        }

        // ── Tilemap application ────────────────────────────────────────────────

        private void ApplyFloorTiles(RoomLayoutData data)
        {
            if (_floorTilemap == null || data.floor_tiles == null || data.floor_tiles.Count == 0) return;

            _floorTilemap.ClearAllTiles();

            foreach (FloorTileData ft in data.floor_tiles)
            {
                if (ft.cell == null || ft.cell.Length < 3) continue;
                Vector3Int pos = new Vector3Int(ft.cell[0], ft.cell[1], ft.cell[2]);

                TileBase tile = ResolveTile(ft.tile_guid);
                if (tile != null)
                    _floorTilemap.SetTile(pos, tile);
                // graceful-degrade: missing tile guid → skip (no crash)
            }
        }

        private void ApplyCliffTiles(RoomLayoutData data)
        {
            if (_cliffTilemap == null || data.cliff_cells == null || data.cliff_cells.Count == 0) return;

            bool hasExplicitGuid = false;
            foreach (CliffCellData ct in data.cliff_cells)
            {
                if (!string.IsNullOrEmpty(ct.tile_guid))
                {
                    hasExplicitGuid = true;
                    break;
                }
            }
            if (!hasExplicitGuid) return;

            _cliffTilemap.ClearAllTiles();

            foreach (CliffCellData ct in data.cliff_cells)
            {
                if (ct.cell == null || ct.cell.Length < 3) continue;
                Vector3Int pos = new Vector3Int(ct.cell[0], ct.cell[1], ct.cell[2]);

                TileBase tile = ResolveTile(ct.tile_guid);
                if (tile != null)
                    _cliffTilemap.SetTile(pos, tile);
                // graceful-degrade: legacy cliff cell without tile_guid -> skip
            }
        }

        // ── Prop diff ─────────────────────────────────────────────────────────

        private void ApplyPropDiff(RoomLayoutData data)
        {
            if (data.prop_instances == null) return;

            // Build a set of desired stable ids from new data.
            HashSet<string> desired = new HashSet<string>(StringComparer.Ordinal);

            for (int i = 0; i < data.prop_instances.Count; i++)
            {
                PropData p = data.prop_instances[i];
                string id = StableId(p, i);
                desired.Add(id);

                if (_activeProps.TryGetValue(id, out GameObject existing))
                {
                    // Move if position/rotation changed.
                    Vector3 wantPos = ToVector3(p.position);
                    Quaternion wantRot = Quaternion.Euler(0f, 0f, p.rotation);
                    if (existing.transform.position != wantPos ||
                        existing.transform.rotation != wantRot)
                    {
                        existing.transform.SetPositionAndRotation(wantPos, wantRot);
                    }
                }
                else
                {
                    // Instantiate new prop.
                    GameObject prefab = ResolvePrefab(p.prefab_guid);
                    if (prefab == null) continue; // graceful-degrade

                    GameObject instance = Instantiate(prefab, ToVector3(p.position),
                        Quaternion.Euler(0f, 0f, p.rotation),
                        _roomRoot != null ? _roomRoot.transform : null);
                    _activeProps[id] = instance;
                }
            }

            // Remove props that are no longer in the JSON.
            List<string> toRemove = new List<string>();
            foreach (KeyValuePair<string, GameObject> kv in _activeProps)
            {
                if (!desired.Contains(kv.Key))
                    toRemove.Add(kv.Key);
            }
            foreach (string id in toRemove)
            {
                if (_activeProps.TryGetValue(id, out GameObject go) && go != null)
                    Destroy(go);
                _activeProps.Remove(id);
            }
        }

        // ── Player safety ─────────────────────────────────────────────────────

        private static void SnapPlayerIfNeeded()
        {
            // If WalkabilityMap is available, validate player cell.
            // Gracefully skips if the system is not present.
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            // Attempt to use WalkabilityMap if it exists in scene.
            // We resolve via reflection-free duck-typing: if the type doesn't
            // exist the typeof lookup returns null at runtime in a stripped build,
            // so we use a try/catch to gracefully skip.
            try
            {
                var wmType = Type.GetType("RIMA.Systems.Map.WalkabilityMap, RIMA.Runtime");
                if (wmType == null) return;

                var method = wmType.GetMethod("IsWalkable",
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                if (method == null) return;

                Vector3Int cell = Vector3Int.RoundToInt(player.transform.position);
                bool walkable = (bool)method.Invoke(null, new object[] { cell });
                if (walkable) return;

                // Find nearest walkable via NearestWalkable if available.
                var nearMethod = wmType.GetMethod("NearestWalkable",
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                if (nearMethod == null) return;

                Vector3Int? nearest = nearMethod.Invoke(null, new object[] { cell, 5 }) as Vector3Int?;
                if (nearest.HasValue)
                {
                    Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                    Vector3 snap = new Vector3(nearest.Value.x, nearest.Value.y, 0f);
                    if (rb != null) rb.position = snap;
                    else player.transform.position = snap;

                    UnityEngine.Debug.Log("[LiveRoomReloader] Player snapped to nearest walkable after reload.");
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning($"[LiveRoomReloader] Player safety check skipped: {ex.Message}");
            }
        }

        // ── Asset resolution (graceful-degrade) ───────────────────────────────

        private TileBase ResolveTile(string guid)
        {
            if (string.IsNullOrEmpty(guid)) return null;
            if (_registry == null) return null;
            if (!_registry.Contains(guid))
            {
                UnityEngine.Debug.LogWarning($"[LiveRoomReloader] GUID not in registry (tile skipped): {guid}");
                return null;
            }
            return _registry.GetTile(guid);
        }

        private GameObject ResolvePrefab(string guid)
        {
            if (string.IsNullOrEmpty(guid)) return null;
            if (_registry == null) return null;
            if (!_registry.Contains(guid))
            {
                UnityEngine.Debug.LogWarning($"[LiveRoomReloader] GUID not in registry (prop skipped): {guid}");
                return null;
            }
            return _registry.GetPrefab(guid);
        }

        // ── Helpers ────────────────────────────────────────────────────────────

        /// <summary>
        /// Stable prop identity: prefer explicit instance_id, fall back to guid+index.
        /// This means adding a prop in the middle of the list will not cause
        /// all subsequent props to be recreated — only truly new ones are added.
        /// </summary>
        private static string StableId(PropData p, int fallbackIndex)
        {
            if (!string.IsNullOrEmpty(p.instance_id)) return p.instance_id;
            return $"{p.prefab_guid}_{fallbackIndex}";
        }

        private static Vector3 ToVector3(float[] v)
        {
            if (v == null || v.Length < 3) return Vector3.zero;
            return new Vector3(v[0], v[1], v[2]);
        }

        private static void CountDiff(RoomLayoutData prev, RoomLayoutData next,
            out int added, out int removed, out int moved)
        {
            added   = 0;
            removed = 0;
            moved   = 0;

            if (prev == null || prev.prop_instances == null)
            {
                added = next?.prop_instances?.Count ?? 0;
                return;
            }

            HashSet<string> prevIds = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < prev.prop_instances.Count; i++)
                prevIds.Add(StableId(prev.prop_instances[i], i));

            HashSet<string> nextIds = new HashSet<string>(StringComparer.Ordinal);
            if (next?.prop_instances != null)
                for (int i = 0; i < next.prop_instances.Count; i++)
                    nextIds.Add(StableId(next.prop_instances[i], i));

            foreach (string id in nextIds) if (!prevIds.Contains(id)) added++;
            foreach (string id in prevIds) if (!nextIds.Contains(id)) removed++;

            // Moved = present in both but position changed (approximation via count delta).
            moved = Mathf.Max(0, Mathf.Min(prevIds.Count, nextIds.Count) - added);
        }
    }
}

#endif // DEVELOPMENT_BUILD || UNITY_EDITOR
