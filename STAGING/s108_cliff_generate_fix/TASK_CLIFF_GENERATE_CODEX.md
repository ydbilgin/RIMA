# TASK: Cliff Generate Algorithm — Code Review (Codex)

ACTIVE RULES: (1) think before reviewing (2) min response (3) inline only (4) cite specific lines.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

Amaç: `CliffAutoPlacer.CollectCliffCells()` algorithm review. Mevcut implementasyon "izole cliff bug"a sahip — floor sınırı dışında havada asılı cliff cluster'lar üretiyor. Code-level fix öner.

## Mevcut Implementasyon
Dosya: `Assets/Scripts/Environment/CliffAutoPlacer.cs` (CollectCliffCells method)

```csharp
private HashSet<Vector3Int> CollectCliffCells()
{
    var cells = new HashSet<Vector3Int>();
    BoundsInt bounds = floorTilemap.cellBounds;

    foreach (Vector3Int cell in bounds.allPositionsWithin)
    {
        if (!floorTilemap.HasTile(cell)) continue;

        Vector3Int south = cell + SouthCell;
        Vector3Int se = cell + SouthCell + EastCell;
        Vector3Int east = cell + EastCell;

        if (!floorTilemap.HasTile(south)) cells.Add(south);
        if (!floorTilemap.HasTile(se)) cells.Add(se);
        if (!floorTilemap.HasTile(east)) cells.Add(east);
    }

    return cells;
}
```

## Bug Tanımı
User screenshot: floor'un olmadığı bölgelerde 2-3 cliff cluster havada asılı görünüyor. Floor tilemap'te 1-cell veya 2-cell izole adalar varsa, onlara cliff konuluyor + ana floor'dan ayrı kaldığı için "saçma" görünüyor.

## Görev — 4 bölüm

### 1. Algorithm Issues
- Mevcut algorithm hangi edge case'lerde fail eder?
- Izole ada (1-cell floor) → 3 cliff cell üretir (S/SE/E), ana arenadan ayrı
- Floor'un "inner pocket" (içerideki boş cell) → her tarafına cliff (içeride saçma)
- Çevresi 1 floor'lu floor cell (peninsula end) → 3 cliff, "yalnız" cluster

### 2. Önerilen Refactor (3 strateji)

**Strategy A — Floor connectivity filter (önce)**:
```csharp
// Skip floor cells with <2 floor neighbors (izole 1-cell adaları)
int floorNeighborCount = CountFloorNeighbors(cell);
if (floorNeighborCount < 2) continue;
```

**Strategy B — Outer perimeter only (flood-fill)**:
```csharp
// Find outer void cells via flood-fill from bounds corner
HashSet<Vector3Int> outerVoid = FloodFillOuterVoid(bounds);
// Only place cliffs on cells that are outer void
if (!outerVoid.Contains(south)) skip;
```

**Strategy C — Post-process isolation filter**:
```csharp
// After collecting cells, remove cliff clusters smaller than N
List<HashSet<Vector3Int>> clusters = FindClusters(cells);
foreach (var cluster in clusters) {
    if (cluster.Count < 3) cells.ExceptWith(cluster);
}
```

### 3. Trade-off Analizi
- A: Basit, O(N) ek scan, ada'ları kökten çözer
- B: Doğru "outer only" semantiği, O(N) flood-fill, inner pocket'leri de korur (void görünür içerde)
- C: Post-process, mevcut algo'yu bozmaz, ama floor isolation'ı çözmez

### 4. Verdict — Önerilen Refactor
- Hangi strateji (A/B/C veya hibrit)?
- Tam C# kod (drop-in replacement CollectCliffCells)
- Test edge case'ler

## Hard Constraints
- Inline only
- C# code drop-in olmalı
- Unity Tilemap API kullan (HasTile, cellBounds)
- Backward-compat YASAK (clean refactor)
- Effort estimate

## Beklenen uzunluk
400-700 kelime.
