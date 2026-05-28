# F1: AdaptiveClusterFilter (Cliff Floating Feel impl day 1)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"`

## Amaç
CliffAutoPlacer.cs'e AdaptiveClusterFilter ekle. 283 cliff → ~128 cliff (155 orphan ≤3 cell temizlik). Editor-time, Regenerate başında çağrılır. ManualPaintedCells whitelist intact.

## Bağlam
- D5.7 F design: `STAGING/CLIFF_F_FULL_SANG_DESIGN.md` (518 satır spec)
- Mevcut: `Assets/Scripts/Environment/CliffAutoPlacer.cs` 211 LOC LIVE
- D5.6 reality: 283 cliff tile, 166 cluster, 155 cluster ≤3 cell orphan
- Iso neighbor HARD rule `feedback_iso_grid_neighbor_vectors` — S=(-1,-1), N=(1,1), E=(1,-1), W=(-1,1), SE=(0,-1), SW=(-1,0)
- DirectionalCliffTile_Hades aktif LIVE D5

## Heuristic formul
```csharp
foreach cluster in BFS_FloodFill(floorCells):
    cluster_size = cluster.Count;
    cluster_ratio = (float)cluster_size / floorTotal;
    if (cluster_size >= minN || cluster_ratio >= coverageRatio):
        validClusters.Add(cluster);  // cliff koy
    else:
        skipClusters.Add(cluster);   // orphan, atla
```
Default: minN=4, coverageRatio=0.005 (0.5%).

## İş kalemleri

### 1. CliffClusterRules ScriptableObject (NEW)
- File: `Assets/ScriptableObjects/Environment/CliffClusterRules.asset` + `CliffClusterRules.cs` class
- File path script: `Assets/Scripts/Environment/CliffClusterRules.cs`
- Fields:
  ```csharp
  [CreateAssetMenu(menuName="RIMA/Cliff Cluster Rules")]
  public class CliffClusterRules : ScriptableObject {
      [Tooltip("Minimum cluster size to keep (cluster count)")] public int minClusterSize = 4;
      [Tooltip("Coverage ratio fallback (cluster/floorTotal)")] [Range(0f, 0.1f)] public float coverageRatioFallback = 0.005f;
      [Tooltip("Iso neighbor 8-connectivity (true) veya 4-connectivity (false)")] public bool use8Connectivity = true;
  }
  ```
- Asset olarak `CliffClusterRules_Default.asset` oluştur

### 2. CliffAutoPlacer.cs AdaptiveClusterFilter method
- File: `Assets/Scripts/Environment/CliffAutoPlacer.cs` EXTEND (mevcut 211 LOC)
- Yeni field: `[SerializeField] private CliffClusterRules clusterRules;`
- Yeni method:
  ```csharp
  /// <summary>F1: Adaptive cluster filter — orphan (small isolated) floor cluster'ları tespit eder.</summary>
  private HashSet<Vector3Int> ComputeOrphanClusters(HashSet<Vector3Int> floorCells)
  {
      var orphan = new HashSet<Vector3Int>();
      if (clusterRules == null) return orphan; // no filter
      
      var visited = new HashSet<Vector3Int>();
      int floorTotal = floorCells.Count;
      
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
      
      // Iso 8-connectivity neighbors (memory feedback_iso_grid_neighbor_vectors)
      Vector3Int[] neighbors = clusterRules.use8Connectivity ?
          new[] { SouthCell, NorthCell, EastCell, WestCell, SouthEastCell, SouthWestCell, new Vector3Int(0, 1, 0), new Vector3Int(1, 0, 0) } :
          new[] { SouthCell, NorthCell, EastCell, WestCell };
      
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
  ```

### 3. CollectCliffCells integration
- Mevcut method (line 177-209) içine orphan check ekle:
  ```csharp
  private HashSet<Vector3Int> CollectCliffCells()
  {
      var cells = new HashSet<Vector3Int>();
      
      // F1: Floor cell envanteri al
      var floorCells = new HashSet<Vector3Int>();
      foreach (Vector3Int cell in floorTilemap.cellBounds.allPositionsWithin)
      {
          if (floorTilemap.HasTile(cell)) floorCells.Add(cell);
      }
      
      // F1: Orphan cluster tespit
      var orphanCells = ComputeOrphanClusters(floorCells);
      
      // Mevcut S/SE/SW void check
      foreach (Vector3Int cell in floorCells)
      {
          if (orphanCells.Contains(cell)) continue;  // F1: orphan floor cell'lere cliff koyma
          
          bool sEmpty  = !floorTilemap.HasTile(cell + SouthCell);
          bool seEmpty = !floorTilemap.HasTile(cell + SouthEastCell);
          bool swEmpty = !floorTilemap.HasTile(cell + SouthWestCell);
          
          if (sEmpty || seEmpty || swEmpty) cells.Add(cell);
      }
      return cells;
  }
  ```

### 4. CliffAutoPlacerEditor inspector hint
- File: `Assets/Editor/Environment/CliffAutoPlacerEditor.cs` EXTEND
- `clusterRules` slot Inspector'da görünür (auto SerializedProperty)
- "Orphan cell count" live label (computed from current floor tilemap)
- "Adaptive filter ENABLED" badge eğer rules atanmışsa

## Dosyalar (scope)
- `Assets/Scripts/Environment/CliffClusterRules.cs` (NEW ~30 LOC)
- `Assets/ScriptableObjects/Environment/CliffClusterRules_Default.asset` (NEW asset)
- `Assets/Scripts/Environment/CliffAutoPlacer.cs` (EXTEND ~80 LOC AdaptiveClusterFilter + BFSFloodFill + CollectCliffCells integration)
- `Assets/Editor/Environment/CliffAutoPlacerEditor.cs` (EXTEND ~20 LOC orphan count display)
- PlayableArena_Test01.unity (CliffAutoPlacer.clusterRules slot atama)
- Toplam ~130 LOC

## YASAK
- Yeni Tilemap GameObject (DecorCliffTilemap D5.5 LIVE, dokunma)
- ManualPaintedCells whitelist değişiklik (D5.5 LIVE)
- DirectionalCliffTile_Hades modify (D5 LIVE)
- ValidateManualPainted method değişiklik (D5.5 LIVE)
- Iso neighbor vectors değiştirme (memory HARD `feedback_iso_grid_neighbor_vectors`)
- Yeni .cs → `refresh_unity scope=all mode=force` ZORUNLU

## Verify
- UnityMCP: `refresh_unity scope=all mode=force` + `read_console` → 0 error / 0 warning
- PlayableArena_Test01.unity CliffAutoPlacer Inspector: clusterRules slot doludu
- Regenerate (C) çağır → cliff count 283 → ~128 (155 orphan temizlendi)
- Screenshot'taki kırmızı çerçeve bölgeleri orphan olarak silinmiş olmalı
- Beyaz çerçeve (ana floor cluster) cliff'leri korunur
- Manual paint (Shift+Click → D5.5 LIVE DecorCliffTilemap) hala çalışır

## Output
- `STAGING/F1_ADAPTIVE_CLUSTER_FILTER_DONE.md` — değişen dosyalar + verify checklist + before/after cliff count + screenshot karşılaştırma

## Süre
~30-45 dk Sonnet bg. Background dispatch.

## Code-writer rotation (HARD `feedback_code_writer_rotation`)
- **Yazan:** Sonnet (mekanik impl, BFS pattern tahmin edilebilir)
- **Reviewer:** Codex xhigh review F1 PASS sonrası (algoritma audit, edge case)
- Codex stuck risk var (`feedback_codex_stale_lock_after_taskstop`) → fallback Opus review

BLOCKED durumu: (a) clusterRules slot atanmazsa → no-op (orphan = empty set) safe default. (b) BFS performance large floor map'te → max 10k cell limit veya yield optimization. (c) Iso neighbor vector 8-connectivity bug → 4-conn safe default fallback.
