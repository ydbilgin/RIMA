# TASK: Cliff Generate Algorithm — Codex Verdict Implement (Sonnet)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Codex'in verdiği Hybrid A+B refactor'u (`CliffAutoPlacer.CollectCliffCells`) uygula. Floor connectivity filter + Outer void flood-fill. İzole cliff cluster bug'ı çözülür.

## Hedef Dosya
`Assets/Scripts/Environment/CliffAutoPlacer.cs` — sadece `CollectCliffCells` method refactor

## Drop-in Replacement (Codex'ten, autonomous applicable)

```csharp
private HashSet<Vector3Int> CollectCliffCells()
{
    var cells = new HashSet<Vector3Int>();
    BoundsInt floorBounds = floorTilemap.cellBounds;
    var searchBounds = new BoundsInt(
        floorBounds.xMin - 1,
        floorBounds.yMin - 1,
        floorBounds.zMin,
        floorBounds.size.x + 2,
        floorBounds.size.y + 2,
        Mathf.Max(1, floorBounds.size.z));

    HashSet<Vector3Int> outerVoid = FloodFillOuterVoid(searchBounds);

    foreach (Vector3Int cell in floorBounds.allPositionsWithin)
    {
        if (!floorTilemap.HasTile(cell)) continue;
        if (CountFloorNeighbors(cell) < 2) continue;

        Vector3Int south = cell + SouthCell;
        Vector3Int se = cell + SouthCell + EastCell;
        Vector3Int east = cell + EastCell;

        if (outerVoid.Contains(south)) cells.Add(south);
        if (outerVoid.Contains(se)) cells.Add(se);
        if (outerVoid.Contains(east)) cells.Add(east);
    }

    return cells;

    int CountFloorNeighbors(Vector3Int cell)
    {
        int count = 0;
        if (floorTilemap.HasTile(cell + NorthCell)) count++;
        if (floorTilemap.HasTile(cell + SouthCell)) count++;
        if (floorTilemap.HasTile(cell + EastCell)) count++;
        if (floorTilemap.HasTile(cell + WestCell)) count++;
        return count;
    }

    HashSet<Vector3Int> FloodFillOuterVoid(BoundsInt bounds)
    {
        var visited = new HashSet<Vector3Int>();
        var queue = new Queue<Vector3Int>();
        Vector3Int start = new Vector3Int(bounds.xMin, bounds.yMin, floorBounds.zMin);

        visited.Add(start);
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            Vector3Int current = queue.Dequeue();
            TryVisit(current + NorthCell);
            TryVisit(current + SouthCell);
            TryVisit(current + EastCell);
            TryVisit(current + WestCell);
        }

        return visited;

        void TryVisit(Vector3Int next)
        {
            if (!bounds.Contains(next)) return;
            if (visited.Contains(next)) return;
            if (floorTilemap.HasTile(next)) return;
            visited.Add(next);
            queue.Enqueue(next);
        }
    }
}
```

## Yapılacaklar

1. **Edit:** `CliffAutoPlacer.cs` mevcut `CollectCliffCells` method'unu yukarıdaki ile değiştir
2. **Using:** `using System.Collections.Generic;` zaten olmalı — kontrol et, yoksa ekle
3. **Compile:** `read_console` 0 error doğrula
4. **Regen + Save:** execute_code ile placer.Regenerate() + SaveScene
5. **Screenshot:** game_view + scene_view (karşılaştırma için)

## Doğrulama Kriterleri
- Compile 0 error
- regen tile count azalmış olmalı (önceki 213, yeni ~150-180 hedef — izole adalar + inner pocket cliff'ler silinir)
- Screenshot'ta izole cliff cluster'lar gitmiş olmalı
- Sahnedeki Player + Floor + UI dokunulmamış

## Hard Constraints
- Surgical — sadece CollectCliffCells method değişir
- Backward-compat YASAK (clean refactor, Codex verdict)
- BLOCKED: compile error fix edilemezse veya runtime exception varsa
- Commit YAPMA

## Inline rapor (<400 kelime)
- Edit yapıldı mı
- Compile error count
- Regen tile count (önceki 213 → yeni X)
- Screenshot path
- BLOCKED varsa neden
