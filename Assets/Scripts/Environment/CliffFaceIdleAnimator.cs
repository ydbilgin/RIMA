using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Environment
{
    /// <summary>
    /// F5: Attaches to CliffTilemap. Periodically swaps visible cliff cells to a random
    /// variant from the matching direction array in DirectionalCliffTile_Hades, giving a
    /// subtle idle animation. Default-on. Does NOT modify DirectionalCliffTile.cs or
    /// DirectionalCliffTile_Hades.asset — uses SetTile with plain Tile assets that mirror
    /// the source tile's transformOffset / spriteScale / flags.
    ///
    /// Fix log (Opus CONDITIONAL → PASS):
    ///   • 8-direction pool (spritesS/SE/SW/E/W/N/NE/NW each get their own Tile[]).
    ///   • transformOffset + spriteScale preserved via Tile.transform (Matrix4x4.TRS).
    ///   • flags = LockTransform | LockColor to match DirectionalCliffTile behaviour.
    ///   • CollectCliffCells skips cells not backed by cliffTileSource (ManualPaintedCells safe).
    ///   • OnDestroy cleans up ScriptableObject pool instances (no memory leak).
    ///   • Camera lazy re-fetch in GetCameraWorldBounds (already existed, kept intact).
    /// </summary>
    [RequireComponent(typeof(Tilemap))]
    public class CliffFaceIdleAnimator : MonoBehaviour
    {
        [Header("Source")]
        [Tooltip("Reference to the DirectionalCliffTile_Hades asset.")]
        public DirectionalCliffTile cliffTileSource;

        [Header("Optional — Placer reference for ManualPaintedCells skip")]
        [Tooltip("Assign to skip D5.5 manual-painted cells during animation.")]
        public CliffAutoPlacer cliffAutoPlacer;

        [Header("Timing")]
        [SerializeField, Range(2f, 8f)] private float cycleIntervalMin = 3f;
        [SerializeField, Range(2f, 8f)] private float cycleIntervalMax = 5f;

        [Header("Performance")]
        [SerializeField] private int maxAnimatedCells = 20;

        // ── Direction enum ───────────────────────────────────────────────────
        private enum CliffDir { S, SE, SW, E, W, N, NE, NW }

        // Iso neighbor vectors (matches CliffAutoPlacer + DirectionalCliffTile)
        private static readonly Dictionary<CliffDir, Vector3Int> DirVectors = new Dictionary<CliffDir, Vector3Int>
        {
            { CliffDir.S,  new Vector3Int(-1, -1, 0) },
            { CliffDir.N,  new Vector3Int( 1,  1, 0) },
            { CliffDir.E,  new Vector3Int( 1, -1, 0) },
            { CliffDir.W,  new Vector3Int(-1,  1, 0) },
            { CliffDir.NE, new Vector3Int( 1,  0, 0) },
            { CliffDir.NW, new Vector3Int( 0,  1, 0) },
            { CliffDir.SE, new Vector3Int( 0, -1, 0) },
            { CliffDir.SW, new Vector3Int(-1,  0, 0) },
        };

        // ── Internal ─────────────────────────────────────────────────────────
        private Tilemap _tilemap;
        private Camera  _cam;

        // 8-direction pool: _pool[dir][variantIndex]
        private Dictionary<CliffDir, Tile[]> _pool;

        // Per-cell direction cache: computed once at CollectCliffCells time
        private List<Vector3Int> _cliffCells = new List<Vector3Int>();
        private Dictionary<Vector3Int, CliffDir> _cellDir = new Dictionary<Vector3Int, CliffDir>();

        // ────────────────────────────────────────────────────────────────────
        private void Awake()
        {
            _tilemap = GetComponent<Tilemap>();
            _cam = Camera.main;
            // F5 fix: auto-find CliffAutoPlacer if not wired in scene (mirrors DirectionalCliffTile.cs:39)
            if (cliffAutoPlacer == null)
                cliffAutoPlacer = FindObjectOfType<CliffAutoPlacer>();
        }

        private void Start()
        {
            if (!ValidateSetup()) return;

            BuildVariantTilePool();
            CollectCliffCells();

            if (_cliffCells.Count > 0)
                StartCoroutine(AnimationLoop());
        }

        private void OnDestroy()
        {
            // Clean up runtime ScriptableObject instances to avoid memory leak
            if (_pool != null)
            {
                foreach (var kvp in _pool)
                    foreach (var tile in kvp.Value)
                        if (tile != null) DestroyImmediate(tile);
                _pool.Clear();
            }
        }

        // ────────────────────────────────────────────────────────────────────
        private bool ValidateSetup()
        {
            if (cliffTileSource == null)
            {
                Debug.LogWarning("[CliffFaceIdleAnimator] cliffTileSource not assigned — animator disabled.", this);
                return false;
            }
            if (cliffTileSource.spritesS == null || cliffTileSource.spritesS.Length < 2)
            {
                Debug.LogWarning("[CliffFaceIdleAnimator] cliffTileSource.spritesS has fewer than 2 sprites — nothing to animate.", this);
                return false;
            }
            return true;
        }

        // ── Pool construction ────────────────────────────────────────────────
        private void BuildVariantTilePool()
        {
            _pool = new Dictionary<CliffDir, Tile[]>();

            AddDir(CliffDir.S,  cliffTileSource.spritesS);
            AddDir(CliffDir.SE, cliffTileSource.spritesSE);
            AddDir(CliffDir.SW, cliffTileSource.spritesSW);
            AddDir(CliffDir.E,  cliffTileSource.spritesE);
            AddDir(CliffDir.W,  cliffTileSource.spritesW);
            AddDir(CliffDir.N,  cliffTileSource.spritesN);
            AddDir(CliffDir.NE, cliffTileSource.spritesNE);
            AddDir(CliffDir.NW, cliffTileSource.spritesNW);
        }

        private void AddDir(CliffDir dir, Sprite[] sprites)
        {
            if (sprites == null || sprites.Length == 0)
            {
                // No sprites for this direction — fall back to spritesS pool when needed
                return;
            }

            // TRS matrix that mirrors DirectionalCliffTile.GetTileData transform
            Matrix4x4 trs = Matrix4x4.TRS(
                cliffTileSource.transformOffset,
                Quaternion.identity,
                new Vector3(cliffTileSource.spriteScale.x, cliffTileSource.spriteScale.y, 1f));

            var pool = new Tile[sprites.Length];
            for (int i = 0; i < sprites.Length; i++)
            {
                var t = ScriptableObject.CreateInstance<Tile>();
                t.sprite        = sprites[i];
                t.colliderType  = Tile.ColliderType.None;
                t.flags         = TileFlags.LockTransform | TileFlags.LockColor;
                t.color         = Color.white;
                t.transform     = trs;
                pool[i]         = t;
            }
            _pool[dir] = pool;
        }

        // ── Cell collection ──────────────────────────────────────────────────
        private void CollectCliffCells()
        {
            _cliffCells.Clear();
            _cellDir.Clear();

            // Require a floor tilemap to determine direction (mirrors DirectionalCliffTile logic)
            Tilemap floorMap = (cliffAutoPlacer != null) ? cliffAutoPlacer.floorTilemap : null;

            foreach (var pos in _tilemap.cellBounds.allPositionsWithin)
            {
                if (!_tilemap.HasTile(pos)) continue;

                // Skip cells not backed by the source tile (ManualPaintedCells / decor tiles)
                if (_tilemap.GetTile(pos) != cliffTileSource) continue;

                // D5.5: skip manual-painted cells explicitly
                if (cliffAutoPlacer != null && cliffAutoPlacer.ManualPaintedCells.Contains(pos)) continue;

                _cliffCells.Add(pos);
                _cellDir[pos] = ComputeCliffDir(pos, floorMap);
            }
        }

        /// <summary>
        /// Mirrors the direction logic in DirectionalCliffTile.GetTileData (lines 57-77).
        /// Returns the direction whose sprite array should be used for this cliff cell.
        /// </summary>
        private CliffDir ComputeCliffDir(Vector3Int pos, Tilemap floorMap)
        {
            if (floorMap == null) return CliffDir.S; // safe default

            bool hasN  = floorMap.HasTile(pos + new Vector3Int( 1,  1, 0));
            bool hasNW = floorMap.HasTile(pos + new Vector3Int( 0,  1, 0));
            bool hasNE = floorMap.HasTile(pos + new Vector3Int( 1,  0, 0));
            bool hasW  = floorMap.HasTile(pos + new Vector3Int(-1,  1, 0));
            bool hasE  = floorMap.HasTile(pos + new Vector3Int( 1, -1, 0));
            bool hasSW = floorMap.HasTile(pos + new Vector3Int(-1,  0, 0));
            bool hasSE = floorMap.HasTile(pos + new Vector3Int( 0, -1, 0));
            bool hasS  = floorMap.HasTile(pos + new Vector3Int(-1, -1, 0));

            // Priority matches DirectionalCliffTile.GetTileData
            if (hasN)  return CliffDir.S;
            if (hasNW) return CliffDir.SE;
            if (hasNE) return CliffDir.SW;
            if (hasW)  return CliffDir.E;
            if (hasE)  return CliffDir.W;
            if (hasSW) return CliffDir.NE;
            if (hasSE) return CliffDir.NW;
            if (hasS)  return CliffDir.N;
            return CliffDir.S; // fallback
        }

        // ── Animation loop ───────────────────────────────────────────────────
        private IEnumerator AnimationLoop()
        {
            while (true)
            {
                float wait = Random.Range(cycleIntervalMin, cycleIntervalMax);
                yield return new WaitForSeconds(wait);

                AnimateVisibleBatch();
            }
        }

        private void AnimateVisibleBatch()
        {
            if (_cliffCells.Count == 0 || _pool == null) return;

            Bounds frustum = GetCameraWorldBounds();
            List<Vector3Int> visible = new List<Vector3Int>(Mathf.Min(_cliffCells.Count, maxAnimatedCells));

            foreach (var cell in _cliffCells)
            {
                // Use cell center for frustum check (more accurate than corner)
                Vector3 worldPos = _tilemap.GetCellCenterWorld(cell);
                if (!frustum.Contains(new Vector3(worldPos.x, worldPos.y, 0f)))
                    continue;

                visible.Add(cell);
                if (visible.Count >= maxAnimatedCells) break;
            }

            // Shuffle visible list so we pick different cells each cycle
            for (int i = visible.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (visible[i], visible[j]) = (visible[j], visible[i]);
            }

            int swapCount = Mathf.Min(visible.Count, maxAnimatedCells);
            for (int i = 0; i < swapCount; i++)
            {
                Vector3Int cell = visible[i];

                // Resolve direction-aware pool; fall back to S pool if direction missing
                CliffDir dir = _cellDir.TryGetValue(cell, out var d) ? d : CliffDir.S;
                Tile[] dirPool = null;
                if (!_pool.TryGetValue(dir, out dirPool) || dirPool == null || dirPool.Length == 0)
                    _pool.TryGetValue(CliffDir.S, out dirPool);

                if (dirPool == null || dirPool.Length == 0) continue;

                int hash = DeterministicHash(cell);
                int cur  = (hash & 0x7fffffff) % dirPool.Length;
                int next = (cur + 1 + (Mathf.Abs(hash) % Mathf.Max(1, dirPool.Length - 1))) % dirPool.Length;
                _tilemap.SetTile(cell, dirPool[next]);
            }
        }

        // ────────────────────────────────────────────────────────────────────
        private Bounds GetCameraWorldBounds()
        {
            if (_cam == null) _cam = Camera.main;
            if (_cam == null) return new Bounds(Vector3.zero, Vector3.one * 9999f);

            float h = _cam.orthographicSize;
            float w = h * _cam.aspect;
            Vector3 c = _cam.transform.position;
            return new Bounds(new Vector3(c.x, c.y, 0f), new Vector3(w * 2f, h * 2f, 9999f));
        }

        private static int DeterministicHash(Vector3Int pos)
        {
            unchecked
            {
                int h = 17;
                h = h * 31 + pos.x;
                h = h * 31 + pos.y;
                h ^= h << 13; h ^= h >> 17; h ^= h << 5;
                return h;
            }
        }
    }
}
