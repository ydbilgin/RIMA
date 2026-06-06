using System.Collections.Generic;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Environment
{
    /// <summary>
    /// Authority for "is this cell walkable / dashable?".
    /// Primary source: RoomTemplateSO.walkableGrid (set by InitFromTemplate each room load).
    /// Fallback: bound Floor Tilemap when no template grid is present.
    /// </summary>
    [ExecuteAlways]
    public sealed class WalkabilityMap : MonoBehaviour
    {
        [Tooltip("The Floor Tilemap that defines walkable ground. Cells without a tile here are treated as void.")]
        public Tilemap floorTilemap;

        [Tooltip("Optional VoidBlocker Tilemap. If assigned together with voidTile, AutoFillVoidBlocker mirrors Floor changes by filling every empty cell (inside Floor.cellBounds +1 pad) with voidTile so the bound CompositeCollider2D physics-blocks the player from leaving the map.")]
        public Tilemap voidBlockerTilemap;

        [Tooltip("Tile placed in every void cell when AutoFillVoidBlocker runs. Sprite can be null; only collider matters.")]
        public TileBase voidTile;

        [Tooltip("Future IObstacle providers. MVP ignores entries that do not implement IObstacle.")]
        public List<MonoBehaviour> obstacleSources = new();

        private static WalkabilityMap _instance;
        private bool _tileChangedSubscribed;

        // Template-sourced walkability grid (takes priority over tilemap lookup).
        // Populated by InitFromTemplate; cleared between rooms.
        private bool[] _templateWalkableGrid;
        private RectInt _templateBounds;
        private bool _hasTemplateGrid;

        // Phase 1 reachability cache (flood-fill from Player). Invalidated on any tilemap edit.
        private HashSet<Vector3Int> _reachableCache;
        private Vector3Int _reachableCacheOrigin;

        /// <summary>
        /// First active WalkabilityMap in the scene. Cached lookup for hot paths.
        /// </summary>
        public static WalkabilityMap Instance
        {
            get
            {
                if (_instance == null)
                {
#if UNITY_2023_1_OR_NEWER
                    _instance = Object.FindFirstObjectByType<WalkabilityMap>();
#else
                    _instance = Object.FindObjectOfType<WalkabilityMap>();
#endif
                }
                return _instance;
            }
        }

        private void OnEnable()
        {
            _instance = this;
            if (!_tileChangedSubscribed)
            {
                Tilemap.tilemapTileChanged += OnAnyTileChanged;
                _tileChangedSubscribed = true;
            }
        }

        private void OnDisable()
        {
            if (_instance == this) _instance = null;
            if (_tileChangedSubscribed)
            {
                Tilemap.tilemapTileChanged -= OnAnyTileChanged;
                _tileChangedSubscribed = false;
            }
        }

        private void OnAnyTileChanged(Tilemap changedMap, Tilemap.SyncTile[] tiles)
        {
            // Any tilemap edit invalidates the reachability cache.
            if (changedMap == floorTilemap)
            {
                _reachableCache = null;
            }

            if (changedMap == null || changedMap != floorTilemap) return;
            if (voidBlockerTilemap == null || voidTile == null) return;
            // Skip the no-op case where the change came from our own void fill.
            AutoFillVoidBlocker();
        }

        /// <summary>
        /// Phase 1 portal reachability gate. BFS flood-fill from the Player tag GameObject's
        /// cell; returns true if <paramref name="cell"/> is in the reachable set.
        /// Cached until any floor tile changes.
        /// </summary>
        public bool IsReachableFromPlayer(Vector3Int cell)
        {
            if (floorTilemap == null) return false;

            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogWarning("[WalkabilityMap] IsReachableFromPlayer: no GameObject with tag 'Player'.");
                return false;
            }

            Vector3Int origin = floorTilemap.WorldToCell(player.transform.position);
            // If the Player is parked off-floor (spawn race), snap to nearest walkable in a 1-cell radius.
            if (!IsWalkable(origin))
            {
                bool snapped = false;
                for (int dy = -1; dy <= 1 && !snapped; dy++)
                {
                    for (int dx = -1; dx <= 1 && !snapped; dx++)
                    {
                        var candidate = new Vector3Int(origin.x + dx, origin.y + dy, origin.z);
                        if (IsWalkable(candidate)) { origin = candidate; snapped = true; }
                    }
                }
                if (!snapped)
                {
                    Debug.LogWarning($"[WalkabilityMap] Player cell {origin} and 1-ring neighbors are all non-walkable.");
                    return false;
                }
            }

            if (_reachableCache == null || _reachableCacheOrigin != origin)
            {
                _reachableCache = BuildReachableSet(origin);
                _reachableCacheOrigin = origin;
            }
            return _reachableCache.Contains(cell);
        }

        private HashSet<Vector3Int> BuildReachableSet(Vector3Int start)
        {
            var visited = new HashSet<Vector3Int>();
            var queue = new Queue<Vector3Int>();
            visited.Add(start);
            queue.Enqueue(start);

            // 4-neighborhood; matches grid movement / dash semantics.
            var dirs = new[]
            {
                new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0),
                new Vector3Int(0, 1, 0), new Vector3Int(0, -1, 0),
            };

            while (queue.Count > 0)
            {
                var c = queue.Dequeue();
                for (int i = 0; i < dirs.Length; i++)
                {
                    var n = c + dirs[i];
                    if (visited.Contains(n)) continue;
                    if (!IsWalkable(n)) continue;
                    visited.Add(n);
                    queue.Enqueue(n);
                }
            }
            return visited;
        }

        /// <summary>
        /// Mirrors Floor into VoidBlocker: every empty cell inside Floor.cellBounds (+1 cell pad)
        /// gets voidTile. Caller-friendly via context menu; also invoked by tilemapTileChanged.
        /// </summary>
        [ContextMenu("Auto-Fill VoidBlocker Now")]
        public void AutoFillVoidBlocker()
        {
            if (floorTilemap == null || voidBlockerTilemap == null || voidTile == null) return;

            voidBlockerTilemap.ClearAllTiles();

            BoundsInt floorBounds = floorTilemap.cellBounds;
            BoundsInt expanded = new BoundsInt(
                floorBounds.xMin - 1, floorBounds.yMin - 1, 0,
                floorBounds.size.x + 2, floorBounds.size.y + 2, 1
            );

            foreach (Vector3Int cell in expanded.allPositionsWithin)
            {
                if (!floorTilemap.HasTile(cell))
                {
                    voidBlockerTilemap.SetTile(cell, voidTile);
                }
            }
        }

        /// <summary>
        /// Populate walkability from a RoomTemplateSO. Call once per room load (e.g. from
        /// RoomRunDirector.BuildCurrentRoom before spawning actors). The template grid takes
        /// priority over the floor tilemap for all IsWalkable / IsWalkableWorld queries.
        /// Pass null to clear the template grid and revert to tilemap-based lookup.
        /// </summary>
        public void InitFromTemplate(RoomTemplateSO template)
        {
            if (template == null || template.walkableGrid == null || template.walkableGrid.Length == 0)
            {
                _hasTemplateGrid = false;
                _templateWalkableGrid = null;
                _reachableCache = null;
                return;
            }

            _templateWalkableGrid = template.walkableGrid;
            _templateBounds = template.bounds;
            _hasTemplateGrid = true;
            _reachableCache = null;
        }

        private bool IsWalkableByTemplate(Vector3Int cell)
        {
            int lx = cell.x - _templateBounds.xMin;
            int ly = cell.y - _templateBounds.yMin;
            if (lx < 0 || lx >= _templateBounds.width || ly < 0 || ly >= _templateBounds.height)
                return false;
            int idx = (ly * _templateBounds.width) + lx;
            return idx >= 0 && idx < _templateWalkableGrid.Length && _templateWalkableGrid[idx];
        }

        public bool IsWalkable(Vector3Int cell)
        {
            if (_hasTemplateGrid)
                return IsWalkableByTemplate(cell);

            if (floorTilemap == null) return false;
            if (!floorTilemap.HasTile(cell)) return false;

            // Future: obstacleSources iteration — IObstacle.IsWalkable at matching cell.
            return true;
        }

        public bool IsWalkableWorld(Vector3 worldPos)
        {
            if (_hasTemplateGrid)
            {
                // Use the floor tilemap for WorldToCell conversion when available, otherwise
                // fall back to a raw integer truncation (works for integer-cell grids).
                if (floorTilemap != null)
                {
                    Vector3Int cell = floorTilemap.WorldToCell(worldPos);
                    return IsWalkable(cell);
                }
                // Approximate: floor(worldPos) → cell coordinate (assumes 1-unit cell alignment)
                Vector3Int approxCell = new Vector3Int(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y), 0);
                return IsWalkable(approxCell);
            }

            if (floorTilemap == null) return false;
            Vector3Int c = floorTilemap.WorldToCell(worldPos);
            return IsWalkable(c);
        }

        public bool IsDashable(Vector3Int cell)
        {
            // MVP: dashable == walkable (no blocking-but-dashable obstacles yet).
            // Future: blocking statues allow dash but not walk.
            return IsWalkable(cell);
        }

        public bool IsDashableWorld(Vector3 worldPos)
        {
            if (_hasTemplateGrid)
            {
                if (floorTilemap != null)
                {
                    Vector3Int cell = floorTilemap.WorldToCell(worldPos);
                    return IsDashable(cell);
                }
                Vector3Int approxCell = new Vector3Int(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y), 0);
                return IsDashable(approxCell);
            }
            if (floorTilemap == null) return false;
            Vector3Int c = floorTilemap.WorldToCell(worldPos);
            return IsDashable(c);
        }

        /// <summary>
        /// Shared axis-slide helper used by PlayerController and BaseMobBehavior.
        /// Given a desired velocity, current world position, and fixedDeltaTime, returns the
        /// clamped velocity that keeps the actor on walkable cells.
        ///
        /// Rules (no corner-cutting):
        ///   1. Diagonal: target cell must be walkable. If not, try slide on X axis, then Y.
        ///      Both axes must individually be walkable to slide (no diagonal corner-cut).
        ///   2. Axis-aligned: if target unwalkable, zero.
        ///
        /// Returns Vector2.zero when all directions are blocked.
        /// Permissive (returns desiredVelocity unchanged) when walkMap is null.
        /// </summary>
        public static Vector2 ClampVelocityToWalkable(WalkabilityMap walkMap, Vector3 currentPos, Vector2 desiredVelocity, float dt)
        {
            if (walkMap == null) return desiredVelocity;

            const float deadzoneSqr = 0.0001f;
            if (desiredVelocity.sqrMagnitude <= deadzoneSqr) return desiredVelocity;

            Vector3 nextPos = currentPos + (Vector3)(desiredVelocity * dt);

            if (walkMap.IsWalkableWorld(nextPos))
                return desiredVelocity;

            bool diagonal = Mathf.Abs(desiredVelocity.x) > 0.001f && Mathf.Abs(desiredVelocity.y) > 0.001f;

            if (diagonal)
            {
                // Try X-only slide: target cell (x+dx, y) must be walkable.
                Vector3 xOnlyPos = currentPos + new Vector3(desiredVelocity.x * dt, 0f, 0f);
                bool xOk = walkMap.IsWalkableWorld(xOnlyPos);
                // Try Y-only slide: target cell (x, y+dy) must be walkable.
                Vector3 yOnlyPos = currentPos + new Vector3(0f, desiredVelocity.y * dt, 0f);
                bool yOk = walkMap.IsWalkableWorld(yOnlyPos);

                if (xOk) return new Vector2(desiredVelocity.x, 0f);
                if (yOk) return new Vector2(0f, desiredVelocity.y);
                return Vector2.zero;
            }
            else
            {
                return Vector2.zero;
            }
        }
    }
}
