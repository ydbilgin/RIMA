# Codex Task — Wall Alignment Fix + Sorting Layer Setup (S95)

> **Profile:** any active cx profile (Unity açık)
> **Effort:** high
> **Output:** `STAGING/CODEX_DONE_wall_alignment_fix_plus_layers_s95.md`
> **Geri dönülebilir:** .meta + TagManager.asset değişiklikleri. Auto-commit YOK.

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev — İki Bölüm

### Bölüm 1 — Wall PNG Custom Alignment Fix

**Sorun:** Önceki re-import (`STAGING/CODEX_DONE_wall_png_reimport_s95.md`) `spritePivot` set etti AMA `spriteAlignment` enum `9 = Custom` yapmadı. Unity custom pivot'u görmüyor, center pivot (0.5, 0.5) yüklüyor.

**5 dosya:**
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_straight_horizontal_v01.png`
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_corner_L_NE_v01.png`
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_arch_opening_v01.png`
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_partition_low_stub_v01.png`
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_cyan_rift_integrated_v01.png`

**Her dosya için TextureImporter ayarı:**
```csharp
var importer = AssetImporter.GetAtPath(path) as TextureImporter;
var settings = new TextureImporterSettings();
importer.ReadTextureSettings(settings);
settings.spriteAlignment = (int)SpriteAlignment.Custom;  // 9
settings.spritePivot = new Vector2(0.5f, customPivotY);  // 0.0313 (128h) / 0.0417 (96h)
importer.SetTextureSettings(settings);
importer.SaveAndReimport();
```

`customPivotY` per file:
| File | png_height | bottom_empty | pivot_Y |
|---|---|---|---|
| straight_horizontal | 128 | 4 | 0.03125 |
| corner_L_NE | 128 | 4 | 0.03125 |
| arch_opening | 128 | 4 | 0.03125 |
| cyan_rift_integrated | 128 | 4 | 0.03125 |
| partition_low_stub | 96 | 4 | 0.041666668 |

**Verify (her dosya için):**
- Sprite asset'i `AssetDatabase.LoadAssetAtPath<Sprite>()` ile yükle
- `sprite.pivot` değerini oku (pixel units)
- Beklenen: pixel pivot.x = sprite.rect.width × 0.5 ve pixel pivot.y = sprite.rect.height × customPivotY (≈4 pixel)
- Mismatch'i raporla, ama re-import'u tekrar deneme — sadece rapor

### Bölüm 2 — Sorting Layer Setup

**Sorun:** C spec ve Opus verdict `Floor` ve `Entities` sorting layer'ları kullanıyor. Projede yok. Mevcut: `Default`, `Ground` (Faz B Codex Bölüm 1 raporu).

**Adımlar:**

1. **TagManager.asset oku:**
   - Path: `ProjectSettings/TagManager.asset` (YAML)
   - `m_SortingLayers` array'ini bul
   - Mevcut layer'ları rapor et (id, name, uniqueID, locked, userAdded)

2. **2 yeni layer ekle:**
   - `Floor` — order: en altta (Default'tan sonra, üstüne karakter/wall çizilebilsin)
   - `Entities` — order: Floor'un üstünde, en üstte (karakter + wall L2b)
   
   Yeni layer eklerken `uniqueID` unique olmalı (mevcut id'lerle çakışmasın). En basit: max existing uniqueID + 1.

3. **Sırası (TagManager m_SortingLayers array order):**
   - Index 0: `Default` (mevcut, dokunma)
   - Index 1: `Ground` (mevcut, dokunma) — eğer Default'tan sonra geliyorsa
   - Index 2: `Floor` (yeni)
   - Index 3: `Entities` (yeni)
   
   Render order: array index düşük → arkada, yüksek → önde. Yani `Default → Ground → Floor → Entities` aşağıdan yukarıya.

4. **PathC_BaseTest sahnesinde Floor_Tilemap sortingLayerName güncelle:**
   - Mevcut: `Default`
   - Yeni: `Floor`
   - Sadece bu sahne, başka sahneye dokunma
   - **Scene SAVE ET** — bu kalıcı yapılacak, user istedi

5. **Verify:**
   - TagManager.asset'te 2 yeni layer var
   - Layer order doğru (Default → Ground → Floor → Entities)
   - Floor_Tilemap sortingLayerName: Floor
   - Scene save edildi, console error 0

### Output Format

```markdown
# Wall Alignment Fix + Sorting Layer Setup — Codex Report

## Bölüm 1: Alignment Fix

### Before / After (per file)
| File | spriteAlignment before→after | spritePivot before→after | Sprite.pivot (pixel) | Verify |
|---|---|---|---|---|
| straight_horizontal | 0 (Center) → 9 (Custom) | (0.5, 0.03125) (no-op) → loaded as (64, 4)px | PASS |
| ... |

## Bölüm 2: Sorting Layer Setup

### TagManager Before
- Index 0: Default (uniqueID 0)
- Index 1: Ground (uniqueID 1234)

### TagManager After
- Index 0: Default
- Index 1: Ground
- Index 2: Floor (uniqueID 5001, userAdded)
- Index 3: Entities (uniqueID 5002, userAdded)

### Scene Update
- PathC_BaseTest.unity Floor_Tilemap.sortingLayerName: Default → Floor
- Scene saved: YES

## Verify
- 5 wall sprites Pivot pixel = (W*0.5, ~4): PASS
- 2 new sorting layers added: PASS
- Floor_Tilemap on Floor layer: PASS
- dotnet build: 0 error
- Unity console: clean

## Git Diff Summary
- 5 .meta files (walls): pivot/alignment update
- ProjectSettings/TagManager.asset: +2 sorting layers
- Assets/Scenes/Demo/PathC_BaseTest.unity: Floor_Tilemap sortingLayerName

## Açık Sorular
- {varsa}
```

## Hard Constraints

- **Sadece listelenen dosyalar:** 5 wall .meta + TagManager.asset + PathC_BaseTest.unity
- **Auto-commit YOK** — user manual commit
- **BLOCKED if unclear:** Mevcut sorting layer durumu beklenenden farklıysa veya TagManager YAML parse zorluğu varsa durdur
- **PathC_BaseTest sahnesini save ET** — bölüm 2'de bu istisna, user istedi (kalıcı setup)
