# TASK: Cliff Generate Algorithm — Izole Cliff Bug Visual Analysis (Antigravity)

ACTIVE RULES: (1) think before reviewing (2) min response, cite specific image areas (3) inline only (4) BLOCKED if image unreadable.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

Amaç: User screenshot atmış (cliff_morning_state.png + ek user upload), bazı bölgelerde cliff sprite'lar **floor'dan tamamen izole** havada asılı görünüyor (2-3 cliff cluster çevresinde floor yok). User dedi: "bazı yerler çok saçma görüyosun generate'te bu problemi nasıl çözeriz". Generate algorithm bug — connectivity check eksik.

## Screenshot Paths
1. `Assets/Screenshots/scene_full_overview_s108.png` — current state (agy önceki verdict applied: south+se+east)
2. `Assets/Screenshots/cliff_morning_state.png` — sabah game view
3. User upload image (chat'te): floor edge'in DIŞINDA 2-3 izole cliff cluster (havada asılı görünüyor)

## Mevcut Config
```
CliffAutoPlacer.CollectCliffCells(): 3-dir (south + se + east) — agy S107 verdict applied
DeterministicVariantTile transformOffset.y = 1.21875 (2 cell exact)
Sorting: Cliff layer=Floor, order=-1
```

## Algoritma (current)
```csharp
foreach (Vector3Int cell in bounds.allPositionsWithin) {
    if (!floorTilemap.HasTile(cell)) continue;  // skip non-floor
    Vector3Int south = cell + SouthCell;
    Vector3Int se = cell + SouthCell + EastCell;
    Vector3Int east = cell + EastCell;
    if (!floorTilemap.HasTile(south)) cells.Add(south);
    if (!floorTilemap.HasTile(se)) cells.Add(se);
    if (!floorTilemap.HasTile(east)) cells.Add(east);
}
```

**Hipotez bug**: Her floor cell için 3 komşu ekleniyor — ama eğer floor cell'in kendisi ayrı bir izole "ada" ise, o ada'nın çevresine cliff koyulur ama o cliff'ler ana arenadan tamamen ayrı kalır → "havada asılı" görünür.

## Sorular (somut cevap)

### 1. Saçma izole cliff bölgeleri
- Hangi bölgelerde cliff floor'dan tamamen izole?
- Floor "izole ada" mı, yoksa cliff cell adjacent floor olmadan mı konulmuş?

### 2. Sebep analizi
- **H1 (Izole ada)**: Floor tilemap'te 1-cell genişlikte "izole ada"lar var → çevresinde cliff bandı yaratıyor, görsel olarak floor görünmüyor
- **H2 (Outer perimeter only)**: Cliff sadece "outer perimeter"e konulmalı, "inner pocket"larda değil (flood-fill outside detection gerek)
- **H3 (Minimum adjacency)**: Cliff cell eklemeden önce, o cell'in çevresinde min 2 floor komşusu olmalı (zayıf bağlantılı cell'leri skip)

### 3. Fix önerisi (autonomous applicable)
Hangi filter eklenmeli:
- **A. Connectivity check**: Floor cell minimum 2 floor komşusu olmalı (izole 1-cell adaları skip)
- **B. Outer perimeter only**: Flood-fill outside void → sadece outer void cell'lere cliff
- **C. Cliff cell adjacency**: Cliff cell eklenince o cell'in min 1 başka cliff komşusu olmalı (yalnız cliff'i skip)
- **D. Min cliff cluster size**: Cliff cell'leri gruplara ayır, <3 cell'lik gruplar silinsin

### 4. Verdict (net karar, autonomous uygulanacak)
- Tek satır karar
- C# code snippet (mümkünse CollectCliffCells refactor önerisi)
- Effort tahmini

## Hard Constraints
- Inline only
- Spesifik image bölgeleri cite et
- Net karar zorunlu
- BLOCKED: bilgi yetersiz

## Beklenen uzunluk
500-700 kelime.
