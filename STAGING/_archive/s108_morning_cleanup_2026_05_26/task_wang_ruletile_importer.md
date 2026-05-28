# TASK: Wang RuleTile Importer + Demo Rebuild (Alabaster Dawn Natural Look)

## EXECUTE EVERY STEP — DO NOT JUST PLAN. WRITE CODE, BUILD, COMMIT.

---

## Scope (4-5h estimate)

- **PixelLab Wang Tileset Importer** — yeni editor menu: PixelLab 4×4 Wang sheet PNG + metadata.json → Unity RuleTile asset (proper corner-based autotile)
- **Demo sahne yeniden inşa** — mevcut RoomPipelineTest sahnesini Alabaster Dawn / Loop Hero tarzı doğal görünüme getir: Wang autotile floor + path + wall, decal scatter, organik zone boundaries
- **F1 template SO doldur** — yeni RuleTile asset'leri + decal sprite'ları assign et, manuel sürükle-bırak gerekmeden Generate çalışır
- **5-test EditMode** — Wang importer correctness, RuleTile generation, paint smoke
- Tek commit: `[demo] Wang RuleTile importer + Alabaster Dawn natural map demo`

---

## Mevcut Asset'ler (kullanılacak)

**Mevcut Wang sheet'ler (PixelLab'dan indirilmiş, hazır):**
```
F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\TILESET_OUTPUT\F1_Wang_Rubble_Path_Existing\
  wang_rubble_path.png          (128×128, 4×4 Wang grid, floor+path 16 tile)
  metadata.json                  (corner-based Wang data)

F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\TILESET_OUTPUT\F1_Wang_Floor_Wall_Existing\
  wang_floor_wall.png            (eski mature batch, palette uyumsuz — bu task'ta kullanma, F2 reserve)
```

**Yakında gelecek (background MCP üretimi sürüyor — script'i ÇIKTI HAZIR olmasını bekleyen async-await pattern ile yaz):**
- `wang_cold_dark_wall.png` (PixelLab tileset_id `bdca2623-62ac-4624-9ffb-d1728f86e3c3`) — cold dark floor+wall
- Decal sprite pack'leri (moss/dust/rift, transparent BG)

**ÖNEMLİ:** Codex bu task'ı RUN ettiğinde wall tileset henüz hazır olmayabilir. **Önce importer'ı yaz, sonra mevcut Tileset 1 (rubble+path) ile demo'yu rebuild et. Wall tileset bittikten sonra ayrı bir step'te wall layer'ı eklersin (manuel veya 2. commit).**

**Decal sprite'lar için:** Eğer hazır değilse, demo'da placeholder decal sprite olarak yine PixelLab'dan herhangi small object/sprite kullanılabilir, ya da decal layer şimdilik boş bırakılır.

---

## Mevcut Kod Yapısı (oku, sonra yaz)

```
Assets/Editor/TileImport/PixelLabPngSheetImporter.cs   -- Faz 1.0 commit a4757ae'de yazıldı, 8×8 RandomTile için
                                                          -- Wang importer için yeni dosya yaz (paralel kalsın)
Assets/Editor/RoomDesigner/RimaRoomDesignerWindow.cs    -- mevcut window
Assets/Editor/RoomDesigner/WallAutoConnect.cs            -- 4-bit NSEW mask + GetTransitionVariantIndex (Faz 1.5)
Assets/Editor/RoomDesigner/DecalPainter.cs               -- Poisson disk + Perlin (Faz 1.5)
Assets/Scripts/Systems/Map/RimaRoomBaselineTemplate.cs   -- floorVariants, wallVariants, decalSprites alanları
Assets/Scenes/Demo/RoomPipelineTest.unity                -- Codex önceki adımında oluşturdu
Assets/Art/Templates/F1_ShatteredRuins.asset             -- şu an boş skeleton template
Assets/Art/Tiles/F1/wang_rubble_path.png                 -- daha önce kopyalandı, Unity import edildi
Assets/Art/Tiles/F1/wang_floor_wall.png                  -- eski wall (F1 için kullanma)
Assets/Art/Tiles/F1/Generated/wang_rubble_path_tile_0..15.asset  -- Tile asset'leri (mevcut, RuleTile'a evrilecek)
```

---

## YENİ Dosyalar — yaz

### 1. `Assets/Editor/TileImport/PixelLabWangImporter.cs`

```csharp
// namespace: RIMA.Editor.TileImport
// [MenuItem("RIMA/PixelLab Wang Tileset Importer")]
// 
// Workflow:
//   1. EditorUtility.OpenFilePanel → Wang sheet PNG seç (örn wang_rubble_path.png)
//   2. Aynı klasörde metadata.json varsa oku, yoksa LogError "metadata.json gerekli"
//   3. Texture importer ayarla: spriteMode=Multiple, slice 32×32, point filter, PPU=32
//   4. 16 sub-sprite garantile (4×4 grid)
//   5. RuleTile asset oluştur:
//        - ScriptableObject.CreateInstance<UnityEngine.Tilemaps.RuleTile>()
//        - Her metadata.tiles[i] entry'sini bir TilingRule olarak ekle
//        - corners: { NW, NE, SW, SE } → Wang 2x2 corner mask
//        - bounding_box → sprite index map
//        - Default sprite: all-lower tile (NW=NE=SW=SE=lower)
//   6. Asset kaydet: Assets/Art/Tiles/F1/Generated/{sheet_name}_RuleTile.asset
//   7. AssetDatabase.SaveAssets()
//   8. LogInfo "Wang RuleTile created at {path}"
//
// Wang RuleTile yapısı:
//   Unity'nin built-in RuleTile sınıfı NSEW + diagonal neighbor lookup yapıyor.
//   PixelLab Wang formatı CORNER-based (NW/NE/SW/SE), Unity NEIGHBOR-based.
//   Convert mantığı:
//     - Bir hücrenin 4 neighbor durumu (this-vs-neighbor terrain match)
//     - Wang corner: bu hücrenin köşelerine 4 farklı terrain
//   Simple convert: Her PixelLab tile metadata corner = upper/lower → 
//     Unity RuleTile NSEW mask: if NW=upper && N=upper → top-left transition
//   Detail: format docs `pixellab://docs/godot/wang-tilesets` (MCP resource) referans
//
// Fallback (basit yaklaşım):
//   - 16 tile'ı sırayla TilingRule olarak ekle, NeighborMatch sırasıyla
//   - Hangi tile hangi NSEW kombinasyonunda kullanılacak — metadata corner'dan derive et
//   - Eğer Unity RuleTile mapping çok karmaşıksa: alternatif olarak 
//     RandomTile + Wang lookup table custom ScriptableObject yaz
```

### 2. `Assets/Tests/EditMode/Editor/WangImporterTests.cs`

```csharp
// namespace: RIMA.Tests.Editor
// Test 1: WangImporter_ParsesMetadata_Returns16Tiles
//   - mock metadata.json oku → 16 tile entry parse et
//   - Assert: tiles.Count == 16, her tile'ın corners + bounding_box doğru
// Test 2: WangImporter_CreatesRuleTileAsset_FromSampleSheet
//   - Sample PNG + metadata.json kullan → RuleTile asset oluştur
//   - Assert: asset dosyası exist, RuleTile m_TilingRules.Count > 0
// Test 3: WangImporter_AllLowerTile_DefaultSprite
//   - all-lower corners tile bul (NW=NE=SW=SE=lower) → default sprite
//   - Assert: m_DefaultSprite != null
```

### 3. Demo rebuild — `RoomDesignerCanvas` veya ayrı static helper kullan

Demo sahne `RoomPipelineTest.unity`'i şu kuralla rebuild et (script veya manuel adımlar):

**Hierarchy korunur:**
```
Grid (cellLayout=Rectangle, cellSize=1×1×0)
  BaseTilemap     ← Wang RuleTile (floor + path) auto-paint
  DecalsTilemap   ← decal sprite scatter (Faz 1.5 DecalPainter)
  WallsTilemap    ← Wang wall (yeni cold tileset gelince)
StageRoot
Main Camera (orto size 6, pos 5,4,-10)
Global Light 2D
RoomPipelineTestController
```

**Paint stratejisi:**
- 16×12 room boyutu (önceki 10×8'den büyük, doğal feel için)
- BaseTilemap: tüm hücreler RuleTile referansı (Tilemap.SetTile her hücreye aynı RuleTile asset koyar, RuleTile kendisi runtime'da corner lookup yapıp doğru sprite seçer)
- Path zone: orta sütun (x=7-9, y=2-9) için terrain=upper işaretle (Wang corner state)
- Wall layer: perimeter (yeni cold tileset hazırsa, değilse skip)
- Decal scatter: F1 template `decalSprites` field'ı dolu ise DecalPainter çalıştır

**Önemli — Wang corner state nasıl set edilir:**
- Unity built-in RuleTile her hücrenin sprite'ını NEIGHBOR-based seçer
- Tilemap'te her hücreye SetTile(cellPos, ruletile) yapınca runtime'da RuleTile.RefreshTile() çağrılır
- "Path zone" oluşturmak için: o hücrelere FARKLI bir RuleTile koy (path-specific) veya 
- Tek RuleTile + alt-terrain identification için: Tilemap.SetColor() veya
- En basit: 2 ayrı RuleTile asset (floor-rule + path-rule), her zone'da farklı RuleTile

---

## Demo SO Setup (F1 template doldur)

`Assets/Art/Templates/F1_ShatteredRuins.asset` SO field'ları doldur:
- `floorVariants[]`: wang_rubble_path floor tiles (16 ayrı Tile asset, RandomTile fallback için)
- `wallVariants[]`: wang_cold_wall floor+wall tiles (hazırsa, değilse boş)
- `decalSprites[]`: moss/dust/rift sprite'lar (hazırsa, değilse boş)
- `propSpecs[]`: boş (Faz 2)
- `decalDensity`: 0.25 (sparse, doğal feel)
- `generatorVersion`: 1

Script ile programatik doldur (AssetDatabase.LoadAssetAtPath + reflection veya direct serialized property edit).

---

## Implementation Steps

**ADIM 0** — `read_console`, mevcut compile temiz mi?

**ADIM 1** — `PixelLabWangImporter.cs` yaz, MenuItem ekle, metadata.json parser + RuleTile builder.

**ADIM 2** — `WangImporterTests.cs` yaz (3 test min).

**ADIM 3** — `read_console` → PASS.

**ADIM 4** — Wang importer çalıştır (programatik veya menüden):
- `wang_rubble_path.png` + `metadata.json` → `wang_rubble_path_RuleTile.asset` oluştur
- Konum: `Assets/Art/Tiles/F1/Generated/`

**ADIM 5** — F1 template SO doldur (programatik script):
- floorVariants[] → 16 Tile asset (mevcut wang_rubble_path_tile_0..15)
- decalDensity = 0.25
- generatorVersion = 1
- Diğer alanlar (wall, decal, prop) boş

**ADIM 6** — Demo sahne rebuild:
- Mevcut RoomPipelineTest.unity aç
- BaseTilemap clear et
- 16×12 alana wang_rubble_path_RuleTile paint et (her hücreye SetTile)
- WallsTilemap perimeter boş bırak (yeni wall tileset gelene kadar)
- DecalsTilemap boş bırak (decal sprite gelene kadar)
- Camera orto size 6, pos 8,6,-10 (16×12 oda merkez)

**ADIM 7** — Sahne save, console temiz mi check.

**ADIM 8** — Screenshot al via `mcp__UnityMCP__manage_camera action=screenshot capture_source=game_view` ve raporla.

**ADIM 9** — Commit: `[demo] Wang RuleTile importer + Alabaster Dawn natural map demo`

---

## Acceptance Criteria

- [ ] `RIMA/PixelLab Wang Tileset Importer` menu item açılır
- [ ] Sample sheet `wang_rubble_path.png` + `metadata.json` → RuleTile asset üretir
- [ ] Üretilen RuleTile'da `m_TilingRules.Count > 0` ve `m_DefaultSprite != null`
- [ ] F1_ShatteredRuins.asset doldurulmuş (floorVariants ≥ 16 Tile)
- [ ] Demo sahnede BaseTilemap 16×12 floor ile dolu
- [ ] Camera 16×12 oda tam ekran (orto size 6, pos 8,6,-10)
- [ ] `read_console` compile PASS, error YOK
- [ ] EditMode tests: ≥3 yeni Wang importer test GREEN
- [ ] Mevcut Faz 1.0/1.5 testler regresyon yok (176/176 PASS kalır)

---

## Git

Tek commit: `[demo] Wang RuleTile importer + Alabaster Dawn natural map demo`

---

## Out of Scope

- Cold wall tileset entegrasyonu (henüz hazır değil, ayrı commit)
- Decal scatter (sprite'lar henüz hazır değil, Faz 1.5 DecalPainter zaten yazıldı, integrate gerekmez)
- Prop placement (Faz 2)
- Multi-room linking
- Runtime URP 2D Light setup (sahnede zaten Global Light 2D var, yeterli)

---

## Risk Notları

1. **Unity RuleTile corner-mapping** — PixelLab Wang format CORNER-based (NW/NE/SW/SE per cell vertex), Unity RuleTile NEIGHBOR-based (N/S/E/W per neighbor cell). Convert mantığı kritik. `pixellab://docs/godot/wang-tilesets` MCP resource referans veriyor — onu kullan.
2. **Custom RuleTile fallback** — Eğer Unity built-in RuleTile convert çok karmaşık olursa, custom ScriptableObject yaz: `WangAutoTile.cs` extends `TileBase`, override `GetTileData()` ile corner-based lookup yap.
3. **PNG path** — Tileset PNG'leri `Assets/Art/Tiles/F1/` altında zaten kopyalanmış olmalı (önceki Codex adımında). Yoksa STAGING'den copy ile başla.
4. **metadata.json copy** — Aynı şekilde Wang sheet ile aynı klasörde olmalı. Yoksa importer error.
5. **F1 template SO doldurma** — SerializedObject + SerializedProperty.objectReferenceValue ile programatik doldur, AssetDatabase.SaveAssets().
6. **EditMode test asset path** — Test'te runtime'da AssetDatabase.LoadAssetAtPath kullan, hardcoded path Test fixture konstant.
