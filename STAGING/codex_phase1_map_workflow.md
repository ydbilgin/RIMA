# Codex Phase 1 — Map Workflow Full Implementation

**Karar refs:** #128/#129 PatchAtlas + organic patch overlay / #121 Scatter Brush / #131 Corner Wang / #132 Map Designer / #134 Procedural+Polish pivot / #135 Phase 1 Hybrid Workflow LOCK
**Master spec (read-only reference):** `STAGING/room_designer_master_spec_v3.md` — 330 satır canonical architecture, 9-stage generation pipeline
**Tahmini süre:** 2-3 saat
**Background:** evet (cx_dispatch.py)
**Dotnet build PASS zorunlu** her commit öncesi

---

## Görev: Phase 1 Map Workflow tam implementasyonu

Procedural generator + paint editor + organic katmanlar hibrit workflow. Ben (Claude) + Codex base layout dizayn ediyor; kullanıcı Map Designer'da brush ile fine-tune; Corner Wang + multi-variant + patch overlay + scatter brush otomatik organic render veriyor.

---

## 6 DELIVERABLE (sırayla uygula, her birinde dotnet build PASS + commit)

### D1. TerrainDefinition + TilesetPairing SO genişletme

Mevcut: `Assets/Scripts/Data/TerrainDefinition.cs` (S73), `TilesetPairing.cs` (S74-A).

Ekle:
- `TerrainDefinition.walkable` (bool), `elevationLevel` (int 0-3), `collisionType` (enum: None/Wall/Cliff/Hazard)
- `TerrainDefinition.variantPool` (List<TileBase>) — multi-variant random için (S75-B 528 stub variant'larını bu listeye besle, default 4-8 variant per terrain)
- `TilesetPairing.patchAtlasRef` (PatchAtlasSO reference, nullable)

### D2. PatchAtlasSO + PatchOverlayPainter (Karar #128/#129)

Master spec v3 §PatchAtlas referans.

- `Assets/Scripts/Data/PatchAtlasSO.cs` — ScriptableObject. Fields: `List<PatchEntry> patches`, her PatchEntry: `Sprite sprite`, `Vector2 size`, `float density`, `float rotationJitter`, `Color tintMin/tintMax`.
- `Assets/Scripts/MapDesigner/PatchOverlayPainter.cs` — runtime/edit-mode component. API: `void PaintPatches(Tilemap baseTilemap, PatchAtlasSO atlas, int seed)`. Tile grid'i üstüne PatchAtlas'tan sprite'lar atar; pozisyon **grid-bağımsız** (Vector2 dünya koordinatında). Sort layer: tile'ın 1 üstü ("Patch"). Rotation jitter ±15° default.
- 2 örnek PatchAtlasSO asset oluştur:
  - `Assets/Data/PatchAtlas_Moss_ShatteredKeep.asset` — yeşilimsi yosun lekeleri (PatchEntry stub sprite'larla doldur, 4-6 entry)
  - `Assets/Data/PatchAtlas_Rift_Fracture.asset` — violet/cyan rift çatlakları stub

### D3. ScatterBrush production (Karar #121)

Mevcut: `Assets/Editor/ScatterBrushWindow.cs` stub (S73).

Production'a çıkar:
- `Assets/Scripts/MapDesigner/ScatterBrushSO.cs` — ScriptableObject. Fields: `List<ScatterEntry> entries`, her entry: `Sprite sprite`, `float perlinFrequency`, `float perlinThreshold`, `int minCount/maxCount per chunk`.
- `Assets/Scripts/MapDesigner/ScatterBrushPainter.cs` — runtime component. Perlin noise tabanlı dağılım, seed-deterministic. Sort layer: "Scatter" (Patch'in 1 üstü).
- ScatterBrushWindow'u SO-based hale getir; preview gizmo ekle.
- 2 örnek ScatterBrushSO:
  - `Assets/Data/Scatter_Stones_ShatteredKeep.asset` (taş + çakıl, 3-5 entry)
  - `Assets/Data/Scatter_Moss_Tufts.asset` (yosun tutamları, 2-3 entry)

### D4. Multi-variant Wang runtime random

S75-B'de 528 stub variant'ı `Assets/Wang/Variants/` altına ürettin. Şu an runtime'da `CornerWangPainter` tek variant kullanıyor.

- `CornerWangPainter.cs` modifiye: tile boyamada `TerrainDefinition.variantPool` boş değilse seed-deterministic random'la (cell coords + global seed → variant index) seç. Boşsa eski single-tile davranışı.
- Map Designer UI'a "Reseed Variants" butonu ekle (test için).

### D5. RoomRecipe + DungeonRecipe + PropCluster SO + ProceduralRoomGenerator

Master spec v3 §RoomRecipe / §DungeonRecipe / §PropCluster referans.

- `Assets/Scripts/Data/RoomRecipe.cs` (ScriptableObject) — fields: `Vector2Int size`, `List<TerrainDefinition> allowedTerrains`, `List<EncounterSlot> encounters`, `BiomePreset biome`, `PatchAtlasSO patchAtlas`, `ScatterBrushSO scatterBrush`, `int seed`.
- `Assets/Scripts/Data/EncounterSlot.cs` — struct: `Vector2Int gridPos`, `string slotType` (e.g. "spawn", "loot", "objective"), `GameObject prefabHint`.
- `Assets/Scripts/Data/PropCluster.cs` (ScriptableObject) — `List<GameObject> propPrefabs`, `Vector2Int clusterSize`, `int minCount/maxCount`, `float spacingMin`.
- `Assets/Scripts/Data/DungeonRecipe.cs` (ScriptableObject) — `List<RoomRecipe> rooms`, `List<RoomConnection> connections`.
- `Assets/Scripts/MapDesigner/ProceduralRoomGenerator.cs` — Editor utility. API: `static RoomData Generate(RoomRecipe recipe)`. Output: `RoomData` struct (vertex grid `int[,]` + encounter positions + patch/scatter override metadata). JSON serialize/deserialize.
- 2 örnek RoomRecipe asset:
  - `Assets/Data/RoomRecipe_ShatteredKeep_Combat_01.asset` (16x12, 3 encounter slot, rubble+wall+path)
  - `Assets/Data/RoomRecipe_ShatteredKeep_Corridor_01.asset` (24x8, 1 encounter, rubble+path)

### D6. Map Designer "Load from Generator" + integrasyon

`Assets/Editor/RimaMapDesignerWindow.cs` (S72-S75 paint editor):

- Toolbar'a "Generate Room" butonu: dropdown'dan RoomRecipe seç, ProceduralRoomGenerator çağır, sonuç vertex grid'i canvas'a yükle.
- "Reseed" butonu: aynı recipe + yeni seed → regenerate (lock-and-regenerate pattern).
- "Apply Patch Atlas" toggle: scene'deki PatchOverlayPainter component'ine RoomRecipe.patchAtlas inject et + PaintPatches() çağır.
- "Apply Scatter Brush" toggle: aynı şekilde ScatterBrushPainter inject.
- Test sceni `Assets/Scenes/Phase1_ProceduralMap_Test.unity` oluştur — RoomRecipe_ShatteredKeep_Combat_01'i Generate + Apply tüm katmanlar.

---

## File Scope

**ALLOWED (oluştur veya modifiye):**
- `Assets/Scripts/Data/**` (yeni SO sınıfları)
- `Assets/Scripts/MapDesigner/**` (painter + generator + helper)
- `Assets/Editor/RimaMapDesignerWindow.cs` (toolbar genişletme)
- `Assets/Editor/ScatterBrushWindow.cs` (production'a çıkarma)
- `Assets/Data/**` (örnek SO asset'leri)
- `Assets/Scenes/Phase1_ProceduralMap_Test.unity`

**FORBIDDEN (dokunma):**
- `Assets/Scripts/Animation/**`
- `Assets/Scripts/Combat/**`
- `Assets/Scripts/Character/**` (Karar #122/#123/#80 character scripts)
- `Assets/Scripts/UI/**` (Karar #133 Game UI)
- Mevcut `CornerWangPainter.cs` core lookup table (sadece variant seçim ekle)

**READ-ONLY (referans):**
- `STAGING/room_designer_master_spec_v3.md` (canonical architecture)
- `STAGING/pixellab_tilesets_dump/INDEX.md` (11 tileset mapping)

---

## Acceptance Criteria

Her deliverable bittiğinde:
1. `dotnet build` PASS (sticky scriptCompilationFailed olmasın)
2. UnityMCP `read_console` ile compile error sıfır olduğunu doğrula
3. Commit format: `[S78][D<N>] <açıklama>` (örn. `[S78][D2] PatchAtlasSO + PatchOverlayPainter (Karar #128/#129)`)
4. Her commit standalone working — başka deliverable'a bağımlılık yok (D6 hariç, D1-D5 prerequisite)

Final acceptance (D6 sonrası):
- `Phase1_ProceduralMap_Test.unity` açılır → "Generate Room" → Map Designer canvas dolar → "Apply Patch" + "Apply Scatter" toggle'lar → Scene view'da organic render (kare kare hissi minimum)
- "Reseed" butonu çalışır, deterministic (aynı seed = aynı output)
- Multi-variant random aktif (Inspector'da TerrainDefinition.variantPool dolu olan terrain'ler çeşitleniyor)

---

## Workflow (Codex execute)

1. Read `STAGING/room_designer_master_spec_v3.md` (canonical architecture)
2. D1 → dotnet build → commit
3. D2 → dotnet build → commit
4. D3 → dotnet build → commit
5. D4 → dotnet build → commit
6. D5 → dotnet build → commit
7. D6 → dotnet build → UnityMCP read_console verify → commit
8. CODEX_DONE_<profile>.md'ye final özet yaz: 6 commit hash + acceptance test sonucu + bilinen issue listesi

**Hata durumunda:** Compile error veya design soru → CODEX_DONE'a "BLOCKED: <neden>" yaz, durup orchestrator'a sor. Otomatik rollback YAPMA.

**Forbidden actions:**
- Character/animation/combat dosyalarına dokunma
- Yeni karar lock yazma (orchestrator yapacak Karar #135)
- Mevcut Wang lookup table'ı değiştirme
- master spec v3'ü modifiye etme (read-only)
