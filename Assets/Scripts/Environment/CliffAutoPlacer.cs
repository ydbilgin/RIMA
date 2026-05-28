using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RIMA.Environment
{
    [ExecuteAlways]
    public sealed class CliffAutoPlacer : MonoBehaviour
    {
        public Tilemap floorTilemap;
        public Tilemap cliffTilemap;
        public TileBase cliffTile;
        public CliffPlacementRules rules;
        public bool clearExistingOnRegenerate = true;

        // F1: Adaptive cluster filter rules (optional — if null, no filtering applied)
        [SerializeField] private CliffClusterRules clusterRules;

        [SerializeField] private int lastGeneratedCount;

        // S110 Phase 2 — manual override blacklist. User-erased cliff cells persist
        // here so subsequent Regenerate() calls don't repopulate them. Serialized
        // backing list keeps data alive across domain reload / scene save.
        [SerializeField, HideInInspector] private List<Vector3Int> manualOverrideCellsSerialized = new List<Vector3Int>();
        private HashSet<Vector3Int> manualOverrideCells;

        // S110 Phase 2B — manual painted whitelist. User-painted cliff cells persist
        // here so subsequent Regenerate() calls force-keep them even if CollectCliffCells
        // does not include them (e.g. surrounded floor cell with no void neighbor).
        [SerializeField, HideInInspector] private List<Vector3Int> manualPaintedCellsSerialized = new List<Vector3Int>();
        private HashSet<Vector3Int> manualPaintedCells;

        public HashSet<Vector3Int> ManualOverrideCells
        {
            get
            {
                if (manualOverrideCells == null)
                    manualOverrideCells = new HashSet<Vector3Int>(manualOverrideCellsSerialized);
                return manualOverrideCells;
            }
        }

        public HashSet<Vector3Int> ManualPaintedCells
        {
            get
            {
                if (manualPaintedCells == null)
                    manualPaintedCells = new HashSet<Vector3Int>(manualPaintedCellsSerialized);
                return manualPaintedCells;
            }
        }

        public void AddManualOverride(Vector3Int cell)
        {
            ManualOverrideCells.Add(cell);
            SyncToSerialized();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        public void RemoveManualOverride(Vector3Int cell)
        {
            ManualOverrideCells.Remove(cell);
            SyncToSerialized();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        [ContextMenu("Clear Manual Overrides")]
        public void ClearManualOverrides()
        {
            ManualOverrideCells.Clear();
            SyncToSerialized();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        public void AddManualPainted(Vector3Int cell)
        {
            ManualPaintedCells.Add(cell);
            // Re-painting a previously erased cell: remove from blacklist
            ManualOverrideCells.Remove(cell);
            SyncToSerialized();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        public void RemoveManualPainted(Vector3Int cell)
        {
            ManualPaintedCells.Remove(cell);
            SyncToSerialized();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        [ContextMenu("Clear Manual Painted")]
        public void ClearManualPainted()
        {
            ManualPaintedCells.Clear();
            SyncToSerialized();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        private void SyncToSerialized()
        {
            manualOverrideCellsSerialized.Clear();
            manualOverrideCellsSerialized.AddRange(ManualOverrideCells);
            manualPaintedCellsSerialized.Clear();
            manualPaintedCellsSerialized.AddRange(ManualPaintedCells);
        }

        // S109 CORRECTED via CellToWorld empirical test (2026-05-26).
        // Unity Tilemap.cellLayout=Isometric world mapping verified:
        //   cell (-1,-1) -> world (0, -0.609) = screen DOWN (true south)
        //   cell ( 1, 1) -> world (0, +0.609) = screen UP    (true north)
        //   cell ( 1,-1) -> world (+1.0, 0)   = screen RIGHT (east)
        //   cell (-1, 1) -> world (-1.0, 0)   = screen LEFT  (west)
        // Antigravity's revise was MATHEMATICALLY CORRECT — S108 memory was wrong.
        private static readonly Vector3Int SouthCell = new Vector3Int(-1, -1, 0);     // Screen South (DOWN)
        private static readonly Vector3Int NorthCell = new Vector3Int(1, 1, 0);       // Screen North (UP)
        private static readonly Vector3Int EastCell = new Vector3Int(1, -1, 0);       // Screen East (RIGHT)
        private static readonly Vector3Int WestCell = new Vector3Int(-1, 1, 0);       // Screen West (LEFT)

        private static readonly Vector3Int SouthEastCell = new Vector3Int(0, -1, 0);  // Screen South-East
        private static readonly Vector3Int SouthWestCell = new Vector3Int(-1, 0, 0);  // Screen South-West

        public int LastGeneratedCount => lastGeneratedCount;
        public bool IsReady => floorTilemap != null && cliffTilemap != null && cliffTile != null;
        public CliffClusterRules ClusterRules => clusterRules;

        /// <summary>F1: Returns count of floor cells belonging to orphan clusters (for editor display).</summary>
        public int CountOrphanCells()
        {
            if (floorTilemap == null || clusterRules == null) return 0;
            var floorCells = CollectFloorCells();
            return ComputeOrphanClusters(floorCells).Count;
        }

        // D5.5: Validates ManualPaintedCells whitelist — removes orphan entries that no
        // longer satisfy the algorithmic rule (cell has no floor tile, OR all S/SE/SW
        // neighbors are occupied). Orphans are cells the user painted but that have
        // since lost their floor context. Called at the start of Regenerate() so the
        // whitelist is clean before tile placement.
        public void ValidateManualPainted()
        {
            if (floorTilemap == null) return;

            var toRemove = new List<Vector3Int>();
            foreach (Vector3Int cell in ManualPaintedCells)
            {
                bool hasFloor = floorTilemap.HasTile(cell);
                bool sEmpty  = !floorTilemap.HasTile(cell + SouthCell);
                bool seEmpty = !floorTilemap.HasTile(cell + SouthEastCell);
                bool swEmpty = !floorTilemap.HasTile(cell + SouthWestCell);
                bool isAlgorithmicCandidate = hasFloor && (sEmpty || seEmpty || swEmpty);
                if (!isAlgorithmicCandidate)
                    toRemove.Add(cell);
            }

            if (toRemove.Count > 0)
            {
                foreach (Vector3Int cell in toRemove)
                    ManualPaintedCells.Remove(cell);
                SyncToSerialized();
                Debug.Log("[CliffAutoPlacer] ValidateManualPainted: removed " + toRemove.Count + " orphan whitelist entries.");
#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif
            }
        }

        [ContextMenu("Regenerate Cliff Ring")]
        public void Regenerate()
        {
            if (!IsReady)
            {
                Debug.LogWarning("[CliffAutoPlacer] Assign floorTilemap, cliffTilemap and cliffTile before regenerating.", this);
                return;
            }

            if (clearExistingOnRegenerate)
            {
                cliffTilemap.ClearAllTiles();
            }

            ValidateManualPainted(); // D5.5: prune orphan whitelist entries before placement

            HashSet<Vector3Int> targets = CollectCliffCells();
            targets.ExceptWith(ManualOverrideCells); // S110 Phase 2: skip user-erased cells
            targets.UnionWith(ManualPaintedCells);   // S110 Phase 2B: force-keep user-painted cells (now clean)
            foreach (Vector3Int cell in targets)
            {
                if (cliffTilemap.HasTile(cell)) continue;
                cliffTilemap.SetTile(cell, cliffTile);
            }

            lastGeneratedCount = targets.Count;

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(this);
                EditorUtility.SetDirty(cliffTilemap.gameObject);
            }
#endif
        }

        public int CountPreviewPlacements()
        {
            return floorTilemap != null ? CollectCliffCells().Count : 0;
        }

        private HashSet<Vector3Int> CollectFloorCells()
        {
            var floorCells = new HashSet<Vector3Int>();
            foreach (Vector3Int cell in floorTilemap.cellBounds.allPositionsWithin)
            {
                if (floorTilemap.HasTile(cell)) floorCells.Add(cell);
            }
            return floorCells;
        }

        private HashSet<Vector3Int> CollectCliffCells()
        {
            var cells = new HashSet<Vector3Int>();

            // F1: Collect all floor cells then compute orphan set
            var floorCells = CollectFloorCells();

            // F1: Orphan cluster detection — skip cliff placement on orphan floor cells
            var orphanCells = ComputeOrphanClusters(floorCells);

            // S110 Phase 3 — "ters yerleştirme":
            // Eğer floor cell'in S, SE veya SW komşusu BOŞ ise, cliff o floor
            // cell'in KENDİSİNE konur (void cell'e değil). Cliff sprite top-pivot
            // + transformOffset.y ile aşağı sarkar, void üzerine biner ama floor
            // sınırını aşmaz → dışa taşma yok.
            //
            // İç pocket mantığı: aynı kural doğal olarak iç boşluklar için de
            // çalışır — pocket'in etrafındaki tüm floor cell'leri otomatik cliff
            // alır (pocket'e bakan kenar). Ekstra filtre gerekmez.
            //
            // Spike filter kaldırıldı (S108 lock): void-cell yerleşimi yapmadığımız
            // için "half-drop spike" durumu artık üretilemez.
            foreach (Vector3Int cell in floorCells)
            {
                if (orphanCells.Contains(cell)) continue; // F1: orphan floor cell'e cliff koyma

                bool sEmpty  = !floorTilemap.HasTile(cell + SouthCell);
                bool seEmpty = !floorTilemap.HasTile(cell + SouthEastCell);
                bool swEmpty = !floorTilemap.HasTile(cell + SouthWestCell);

                if (sEmpty || seEmpty || swEmpty)
                {
                    cells.Add(cell);
                }
            }

            return cells;
        }

        /// <summary>F1: Adaptive cluster filter — orphan (small isolated) floor cluster'ları tespit eder.
        /// Returns set of floor cells that belong to orphan clusters (below minClusterSize AND coverageRatioFallback).</summary>
        private HashSet<Vector3Int> ComputeOrphanClusters(HashSet<Vector3Int> floorCells)
        {
            var orphan = new HashSet<Vector3Int>();
            if (clusterRules == null) return orphan; // no filter — safe default

            var visited = new HashSet<Vector3Int>();
            int floorTotal = floorCells.Count;
            if (floorTotal == 0) return orphan;

            foreach (var startCell in floorCells)
            {
                if (visited.Contains(startCell)) continue;
                var cluster = BFSFloodFill(startCell, floorCells, visited);
                float ratio = (float)cluster.Count / floorTotal;

                if (cluster.Count < clusterRules.minClusterSize && ratio < clusterRules.coverageRatioFallback)
                {
                    orphan.UnionWith(cluster);
                }
            }
            return orphan;
        }

        private HashSet<Vector3Int> BFSFloodFill(Vector3Int start, HashSet<Vector3Int> floorCells, HashSet<Vector3Int> visited)
        {
            var cluster = new HashSet<Vector3Int>();
            var queue = new Queue<Vector3Int>();
            queue.Enqueue(start);
            visited.Add(start);
            cluster.Add(start);

            // Iso neighbor vectors (memory HARD feedback_iso_grid_neighbor_vectors):
            // S=(-1,-1), N=(1,1), E=(1,-1), W=(-1,1), SE=(0,-1), SW=(-1,0)
            // 8-connectivity adds NE=(1,0) and NW=(0,1)
            Vector3Int[] neighbors = clusterRules.use8Connectivity
                ? new[] { SouthCell, NorthCell, EastCell, WestCell,
                          SouthEastCell, SouthWestCell,
                          new Vector3Int(1, 0, 0),  // NorthEast diagonal
                          new Vector3Int(0, 1, 0) } // NorthWest diagonal
                : new[] { SouthCell, NorthCell, EastCell, WestCell };

            while (queue.Count > 0)
            {
                var cell = queue.Dequeue();
                foreach (var dir in neighbors)
                {
                    var next = cell + dir;
                    if (!floorCells.Contains(next) || visited.Contains(next)) continue;
                    visited.Add(next);
                    cluster.Add(next);
                    queue.Enqueue(next);
                }
            }
            return cluster;
        }
    }
}
