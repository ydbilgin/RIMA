# TASK: Room Designer Faz 1.0 MVP (Karar #115)

## EXECUTE EVERY STEP — DO NOT JUST PLAN. WRITE CODE, BUILD, COMMIT.

---

## Scope (12-16h estimate)

- `RoomBaselineGenerator` — deterministic procedural floor+wall tile placement (System.Random, NOT UnityEngine.Random)
- `RoomBaselineTemplate` ScriptableObject — biome/archetype parameters (Game Layer, Karar #117 portable core split)
- Toolbar `btn-generate` button in `RimaRoomDesignerWindow` — hooks generator → canvas → bake
- `RoomSaver` extension — adds mandatory `RoomConfig` component to saved prefab (currently missing from Save pipeline)
- **PixelLab JSON Export Parser** — `asset_000.json` → auto-slice sheet → RandomTile/RuleTile asset create (Karar #118, TileImportWizard integration)
- Acceptance: From the Room Designer window (RIMA > Room Designer), press Generate with a seed+biome+archetype → canvas fills → Save → prefab+blueprint+RoomConfig asset created → RoomLoader.Load() in Play mode succeeds with zero errors

---

## Karar Bağlamı (inline)

**Karar #115 — AI-Assisted Map Builder (LOCKED 2026-05-13)**
Unity Editor Window tabanlı (mevcut F2 hotkey, `RIMA > Room Designer` menu). Fullscreen game-view editor REJECTED. LLM/PixelLab API çağrısı YASAK. Tüm AI baseline = pure C# deterministic. `RoomBaselineGenerator` kontratı: `GenerationInput { int seed, BiomeType biome, string archetypeId, int width, int height, int generatorVersion }`. Exit criteria: aynı GenerationInput → bit-identical RoomBlueprint; 5 farklı seed/biome/archetype generate → RoomLoader runtime hatasız yükler; designer manuel düzeltme oranı <%20; RoomConfig-missing hatası yok.

**Karar #116 — Tile Transition Quality Standard (LOCKED 2026-05-13)**
Floor-wall transition Raggedness >=40% (Wang autotile), 3+ variant zorunlu, Perlin-driven asimetrik dağılım. Baked light/glow tile içinde YASAK — runtime URP 2D Light. Bu task bake logic'i koyar (FloorVariantPainter mevcut Perlin algoritması reuse edilir), tile asset kalitesi bu kararla ölçülür.

**Karar #117 — Room Designer Portable Core (LOCKED 2026-05-13)**
Core katmanı (`Assets/RoomDesigner.Core/`) RIMA referansı içermez: FloorVariantPainter Perlin algo, WallAutoConnect NSEW mask logic, `RoomBaselineGenerator` abstract base, SeedPipeline, byte[] grid + LUT pattern, Editor UXML/USS şablonları. Game Layer (`Assets/Scripts/Systems/Map/Rima*/`): `RimaBiomeType` enum, `RimaRoomBaselineTemplate` concrete SO, `RimaRoomConfig` adaptor, `RimaArchetypeGenerators` (combat/loot/elite/boss/vista). Faz 1.0 exit criteria'ya "Core layer RIMA-free derlenebilirlik" testi eklenir.

**Karar #118 — Hybrid Tile Composition System (LOCKED 2026-05-13)**
Unity Tilemap stack 4 katman: (1) Base layer = RuleTile + RandomTile (floor + path + variants); (2) Decal layer = transparent overlay (collider OFF); (3) Wall layer = Wang autotile (gameplay collider); (4) Prop layer = discrete sprites. PixelLab Export Parser: folder seç → `asset_000.json` parse → sheet auto-slice → standard Wang mapping → RuleTile auto-create → multi-layer tilemap iskeleti. `create_tiles_pro` 64-batch çıktısı (256x256, 8x8 grid) bu parser'dan geçer.

---

## Mevcut Durum — Kod Okuma Gereksinimleri

**Önce bu dosyaları oku, sonra yaz:**

```
Assets/Editor/RoomDesigner/RimaRoomDesignerWindow.cs      -- toolbar BindToolbar() genişlet, btn-generate ekle
Assets/Editor/RoomDesigner/FloorVariantPainter.cs          -- BakeVariants() reuse et, generator hook noktası
Assets/Editor/RoomDesigner/WallAutoConnect.cs              -- RefreshNeighborhood() reuse et, 4-bit NSEW mask
Assets/Editor/RoomDesigner/SaveLoad/RoomSaver.cs           -- Save() genişlet: RoomConfig component ekle
Assets/Editor/RoomDesigner/Canvas/RoomDesignerCanvas.cs    -- FloorTilemap/WallsTilemap/DecalsTilemap erişimi
Assets/Editor/RoomDesigner/IRoomDesignerContext.cs         -- context interface, Generate için yeni alan gerekebilir
Assets/Scripts/Runtime/Rooms/RoomBlueprint.cs              -- byte[] floorVariantIndex, wallVariantIndex, overrideVariantIndex
Assets/Scripts/Systems/Map/RoomConfig.cs                   -- MonoBehaviour schema (LOCKED, değiştirme)
Assets/Scripts/Systems/Map/RoomLoader.cs                   -- Load() → ValidateContract() → RoomConfig missing = LogError
Assets/Editor/TileImport/TileImportWizard.cs               -- mevcut sheet-slice mantığı, JSON parser buraya veya yanına
```

---

## Dosya Konumları

### YENİ dosyalar — yaz:

```
Assets/RoomDesigner.Core/Runtime/
  GenerationInput.cs              -- struct: seed, biomeId(string), archetypeId, width, height, generatorVersion
  SeedPipeline.cs                 -- static: DeriveSubSeed(int masterSeed, string domain) -> int
  RoomBaselineGeneratorBase.cs    -- abstract: GenerationInput -> GridSnapshot (byte[] floor, byte[] wall, AnchorZone[])
  GridSnapshot.cs                 -- struct: byte[] floorMask, byte[] wallMask, int width, int height, Vector3Int origin

Assets/RoomDesigner.Core/Editor/
  CoreRoomBaselineRunner.cs       -- static: Run(RoomBaselineGeneratorBase, GenerationInput, Tilemap floor, Tilemap wall)
                                  -- calls generator.Generate() -> applies tiles to tilemaps
                                  -- calls FloorVariantPainter.BakeVariants() + WallAutoConnect.RefreshNeighborhood()

Assets/Scripts/Systems/Map/
  RimaRoomBaselineTemplate.cs     -- [CreateAssetMenu] ScriptableObject extends nothing (Game Layer)
                                  -- fields: BiomeType biome, string archetypeId, int minWidth, int maxWidth,
                                            int minHeight, int maxHeight, TileBase[] floorVariants,
                                            TileBase[] wallVariants, int generatorVersion
  RimaRoomBaselineGenerator.cs    -- concrete impl of RoomBaselineGeneratorBase (Game Layer)
                                  -- System.Random(seed), fill border walls, fill interior floor,
                                     sample anchor zones from archetype defaults
                                  -- MUST NOT use UnityEngine.Random
```

### DÜZENLE — mevcut dosyalar:

```
Assets/Editor/RoomDesigner/RimaRoomDesignerWindow.cs
  -- BindToolbar(): btn-generate button kayıt et (UXML'de btn-generate yoksa ekle)
  -- [SerializeField] RimaRoomBaselineTemplate activeTemplate alanı (Inspector'dan atanır)
  -- GenerateRoom() private method:
       1. GenerationInput oluştur (activeBp.noiseSeed, template.biome, template.archetypeId, w, h, template.generatorVersion)
       2. CoreRoomBaselineRunner.Run(new RimaRoomBaselineGenerator(), input, FloorTilemap, WallsTilemap)
       3. FloorVariantPainter.BakeVariants(FloorTilemap, activeBp, template.floorVariants)
       4. WallAutoConnect.RefreshNeighborhood(WallsTilemap, allWallCells, template.wallVariants, activeBp)
       5. MarkDirty()

Assets/Editor/RoomDesigner/SaveLoad/RoomSaver.cs
  -- Save() sonunda: prefab.AddComponent<RoomConfig>() if missing
  -- RoomConfig fields: roomId = bp.roomId, roomType default = RoomType.Combat,
     depthBandMin = 0, depthBandMax = 99, cellSize = baseGrid.cellSize (veya Vector3(1,0.5,0)),
     gridLayout = Isometric, orientation = XYZ
  -- bp.prefab bağlantısı mevcut, bunu koru

Assets/Editor/TileImport/TileImportWizard.cs  (VEYA yeni dosya yanında)
  -- PixelLab Export JSON parser ekle:
     [MenuItem("RIMA/PixelLab Export Parser")]
     -- Folder seçimi (EditorUtility.OpenFolderPanel)
     -- asset_000.json oku (JSON: {tiles: [{id, x, y, w, h}]} formatı yoksa raw sprite sheet slic yap)
     -- Sprite sheet (256x256 PNG, 8x8 grid, 32px/tile) → 64 Sprite asset slice
     -- RandomTile asset oluştur (64 tile hepsini weight=1 ile) → Assets/Art/Tiles/{biome}/{type}/random_{name}.asset
     -- RuleTile asset oluştur (Wang: 4 neighbour NSEW bitmask → tile slot) → rule_{name}.asset
     -- Tilemap iskeleti sahneye koy (veya prefab): Base + Decal + Wall + Prop tilemaps
```

---

## Implementation Steps (Codex execute)

**ADIM 0 — Derleme baseline al**
- `read_console` çalıştır, mevcut compile error var mı kontrol et. Varsa önce düzelt, sonra devam et.

**ADIM 1 — Core assembly oluştur**
- `Assets/RoomDesigner.Core/` klasörü oluştur
- `Assets/RoomDesigner.Core/RIMA.RoomDesigner.Core.asmdef` oluştur (name: RIMA.RoomDesigner.Core, no RIMA assembly references)
- `GenerationInput.cs`, `SeedPipeline.cs`, `GridSnapshot.cs`, `RoomBaselineGeneratorBase.cs` yaz
- `Assets/RoomDesigner.Core/Editor/RIMA.RoomDesigner.Core.Editor.asmdef` oluştur (references: RIMA.RoomDesigner.Core, RIMA.Editor.RoomDesigner)
- `CoreRoomBaselineRunner.cs` yaz
- `read_console` → compile PASS olana kadar hata düzelt

**ADIM 2 — Game Layer SO ve Generator**
- `RimaRoomBaselineTemplate.cs` yaz (namespace: RIMA.Systems.Map)
- `RimaRoomBaselineGenerator.cs` yaz (System.Random ZORUNLU, UnityEngine.Random YASAK)
  - Algoritma: border duvarları doldur → interior floor doldur → köşe anchor zone örnekle
  - `generatorVersion` field: GridSnapshot'a dahil et (determinizm testi için)
- `read_console` → PASS

**ADIM 3 — RimaRoomDesignerWindow Generate butonu**
- `RimaRoomDesignerWindow.cs` BindToolbar() içine btn-generate ekle
- `GenerateRoom()` metodu: GenerationInput → CoreRoomBaselineRunner.Run() → FloorVariantPainter.BakeVariants() → WallAutoConnect.RefreshNeighborhood() → MarkDirty()
- UXML'de `btn-generate` butonu yoksa `Assets/Editor/RoomDesigner/UI/RoomDesignerWindow.uxml` düzenle, btn-save'in yanına ekle
- `read_console` → PASS

**ADIM 4 — RoomSaver RoomConfig köprüsü**
- `RoomSaver.Save()` sonunda RoomConfig component ekleme bloğu yaz
- RoomConfig alanlarını doldur (roomId, RoomType.Combat default, cellSize, Isometric, XYZ)
- `read_console` → PASS

**ADIM 5 — PixelLab PNG Sheet Parser (PNG-FIRST, JSON OPTIONAL)**
- Kullanıcı PixelLab çıktısını **PNG sprite sheet** olarak verecek (`STAGING/TILESET_OUTPUT/{biome}/{type}/sheet.png`)
- `TileImportWizard.cs` veya yeni `PixelLabExportParser.cs` (`Assets/Editor/TileImport/`) içine menu item ekle: `[MenuItem("RIMA/PixelLab PNG Sheet Importer")]`
- Folder seçimi (EditorUtility.OpenFolderPanel)
- Folder içindeki PNG'leri bul (her sheet ayrı tile set)
- Her PNG için:
  - Boyut assert: 256x256 (8x8 grid, 32px/tile) — boyut farklıysa LogWarning + skip
  - TextureImporter ayar: spriteMode=Multiple, filterMode=Point, compression=None, pixelsPerUnit=32
  - 8x8 grid = 64 Sprite slice (SpriteImportData[] ile)
  - 64 Sprite asset → RandomTile (`UnityEngine.Tilemaps.RandomTile`) oluştur, hepsi weight=1
  - Wang RuleTile oluştur: 4-bit NSEW bitmask (16 kombinasyon) → standart Wang slot mapping (0=center isolated, 15=center surrounded)
  - Output: `Assets/Art/Tiles/{biome}/{type}/random_{name}.asset` + `rule_{name}.asset`
- **JSON parser OPSIYONEL** — eğer aynı klasörde `asset_000.json` veya `metadata.json` varsa oku ve tile isim/etiketlerini kullan; yoksa filename'den türet. JSON yokluğu hata değil.
- `read_console` → PASS

**ADIM 6 — Manuel smoke test (Editor Play)**
- Room Designer window aç (`RIMA > Room Designer`)
- `RimaRoomBaselineTemplate` SO oluştur (Project > Create > RIMA > Room Baseline Template), biome + archetype doldur, floorVariants + wallVariants ata (en az 1 tile yeterli)
- Window'da template ata, seed = 42, Generate bas
- Canvas floor + wall ile dolmalı
- Save bas → `Assets/_Generated/Rooms/` altında prefab + blueprint + RoomConfig component içermeli
- Play mode'da `RoomLoader.Load(RoomType.Combat, 1)` çalıştır → console'da hata YOK
- 5 farklı seed ile tekrar et (42, 137, 2501, 9999, 31415) → hepsi bit-identical mı kontrol et (aynı seed iki kez → aynı floorVariantIndex)

**ADIM 7 — Core layer izolasyon testi**
- `RIMA.RoomDesigner.Core.asmdef` içinde herhangi RIMA.* assembly referansı var mı kontrol et
- Varsa kaldır — Core RIMA-free derlenmeli
- `read_console` → PASS

**ADIM 8 — Commit**
- `git add -A`
- `git commit -m "[faz1.0] Room Designer MVP: RoomBaselineGenerator + PixelLab parser + RoomConfig pipeline"`

---

## Acceptance Criteria

- [ ] `read_console` compile PASS (sıfır error)
- [ ] `RIMA > Room Designer` window açılır, btn-generate görünür
- [ ] Seed 42 + herhangi biome + archetype → Generate → canvas floor/wall ile dolar
- [ ] Save → `Assets/_Generated/Rooms/{biome}/{roomId}.prefab` ve `.asset` oluşur
- [ ] Oluşan prefab'da `RoomConfig` component var ve roomId dolu
- [ ] `RoomLoader.Load()` Play mode'da LogError üretmez
- [ ] Aynı GenerationInput iki kez → `floorVariantIndex` byte-for-byte identical (determinizm)
- [ ] 5 farklı seed generate → hepsi runtime yüklenir, hata yok
- [ ] `RIMA.RoomDesigner.Core.asmdef` içinde RIMA game assembly referansı yok (portable core)
- [ ] `RIMA > PixelLab PNG Sheet Importer` menu item açılır, klasör seçimi → 256x256 PNG bulup 64-tile RandomTile + Wang RuleTile asset üretir
- [ ] Portable Core asmdef (`RIMA.RoomDesigner.Core`) sıfır RIMA referansı ile compile PASS (Karar #117 LOCKED Faz 1.0'da)

---

## Git

Tek commit:
```
[faz1.0] Room Designer MVP: RoomBaselineGenerator + PixelLab parser + RoomConfig pipeline
```

---

## Out of Scope (Faz 1.5)

- Inpaint Region brush mode (kilitsiz hücre re-seed)
- Force re-seed komutu (lock'ları yok say)
- Anchor Zone painter (tile-mask + zone type enum + weight float)
- RenderTexture cache + repaint debounce
- Preview kamera ~35 derece konverjans kalibrasyonu (Karar #113)
- `floorOverrideVariantIndex` (wall override var, floor override yok — Faz 1.5)
- Drop shadow (child SpriteRenderer multiply oval) — Karar #116 (g), Faz 1.5
- Brush UX polish, multi-room linking

---

## Dikkat / Risk Notları (Codex okusun)

1. **UnityEngine.Random YASAK** — `RimaRoomBaselineGenerator` sadece `System.Random(seed)` kullanır. UnityEngine.Random global state'i değiştirmez. Karar #115 exit criteria bunun üzerine kurulu.
2. **RoomConfig MonoBehaviour** — SO değil, MonoBehaviour. `prefab.GetComponent<RoomConfig>()` ile erişilir, `AddComponent` ile eklenir. `RoomSaver.Save()` zaten prefab'ı yazıyor, component sonradan eklenir.
3. **Mevcut `RoomBlueprint.roomType` string** — `RoomConfig.roomType` enum (RoomType). İkisi farklı. `RoomSaver` bp.roomType'ı (string) parse edip config.roomType'a (enum) dönüştürür; parse fail → default Combat.
4. **FloorVariantPainter.BakeVariants** `variantSet` boş olamaz — template'te en az 1 TileBase atanmış olmalı; generate öncesinde null/empty guard ekle, hata logla ve return.
5. **WallAutoConnect** `allWallCells` parametresi — generate sonrası tüm wall tilemap'teki dolu hücreleri topla: `WallsTilemap.cellBounds` iterate et.
6. **PixelLab PNG-only akışı (kullanıcı kararı)** — Kullanıcı her tile sheet'i 256x256 PNG olarak verecek. Parser PNG-first çalışır: 8x8 grid slice + RandomTile + Wang RuleTile üret. JSON varsa opsiyonel metadata, yoksa filename + grid index kullan. Sample JSON dosyası henüz yok.
7. **Core asmdef editor vs runtime split** — `CoreRoomBaselineRunner.cs` Editor-only (EditorApplication bağlamında çalışır). Editor asmdef'e koy. `GenerationInput`, `GridSnapshot`, `RoomBaselineGeneratorBase` runtime asmdef'e koy (runtime da kullanabilsin).
