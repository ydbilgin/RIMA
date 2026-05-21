# Codex Task — Wall PNG Re-Import (4 dosya, .meta only)

> **Profile:** any active cx profile
> **Effort:** high
> **Output:** `STAGING/CODEX_DONE_wall_png_reimport_s95.md`
> **Geri dönülebilir:** Sadece .meta dosyaları değişir. Asıl PNG dokunulmaz.

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed .meta files only (4) BLOCKED if unclear.

## Görev

4 wall PNG'nin TextureImporter ayarlarını standardize et. Asıl PNG'lere DOKUNMA — sadece import setting (.meta dosyaları).

### Dosyalar

| # | PNG Path | Mevcut | Hedef |
|---|---|---|---|
| 1 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_straight_horizontal_v01.png` | PPU 100, Bilinear, Pivot (0.5, 0.5), no trim | PPU 64, Point, Pivot (0.5, 0.0), bottom-row foot |
| 2 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_corner_L_NE_v01.png` | aynı | aynı hedef |
| 3 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_arch_opening_v01.png` | aynı | aynı hedef |
| 4 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_cyan_rift_integrated_v01.png` | aynı | aynı hedef |

**Not:** `act1_wall_partition_low_stub_v01.png` zaten PPU 64 + Point — DOKUNMA, sadece pivot (0.5, 0.0)'a güncelle (bu da L2b short variant).

### Hedef Import Ayarları (her dosya için)

```
TextureImporter:
  textureType: Sprite (Default = 8)
  spriteMode: Single (1)        ← Multiple yerine Single (tek sprite tek PNG)
  spritePixelsPerUnit: 64
  filterMode: Point (0)
  alphaIsTransparency: true
  wrapMode: Clamp
  spritePivot: (0.5, 0.0)        ← bottom-center
  spriteMeshType: FullRect (0)   ← Tight değil, padding sorunu çıkmasın
  spriteExtrude: 1
```

### Bottom Padding Trim Stratejisi

PNG'lerin alt kısmında 4-39 boş satır var. **PNG'yi değiştirme**, bunun yerine:

**Yol A (önerilen):** Pivot'u "alpha bbox bottom"a göre hesapla.
- Her PNG için Codex diagnose raporundaki `alpha bbox y=` değerinden alt y'yi al
- Pivot Y normalize = `alpha_bbox_bottom_y / png_height`
- Örnek: straight_horizontal alpha bbox y=36..88, png 128h → pivot Y = 36/128 = 0.281
- Pivot X = 0.5 (orta)

**Yol B (basit, foot-bbox-tabanlı):**
- Pivot Y her zaman = (bottom_empty_row_count / png_height)
- straight_horizontal: bottom 4 empty → 4/128 = 0.031 → pivot Y ≈ 0.031
- Bu da "foot pixel = 0 world Y" sonucunu verir

**Codex karar:** Yol B daha güvenli (alpha bbox bottom = padding sonrası ilk dolu satır, pivot orada olmalı). Her dosya için:

| # | PNG | png_height | bottom_empty | pivot_Y |
|---|---|---|---|---|
| 1 | straight_horizontal | 128 | 4 | 0.0313 |
| 2 | corner_L_NE | 128 | 4 | 0.0313 |
| 3 | arch_opening | 128 | 4 | 0.0313 |
| 4 | cyan_rift_integrated | 128 | 4 | 0.0313 |
| 5 | partition_low_stub | 96 | 4 | 0.0417 |

**Custom pivot:** Yukarıdaki Y değerini kullan. X = 0.5.

### Adımlar

1. Her .meta dosyasını oku (`Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/*.png.meta`)
2. Hedef ayarları uygula (`AssetDatabase.ImportAsset` veya `TextureImporter` ile programatik)
3. Re-import tetikle
4. **Verify:** Her PNG için TextureImporter'dan yeni ayarları oku, hedefle karşılaştır
5. Git diff al — sadece .meta dosyaları değişmiş olmalı, başka değişiklik YASAK

### Verify (build + visual sanity check)

- `dotnet build` 0 error olmalı (asset import sonrası shader/sprite atlas warning kabul)
- Yeni pivot ile sprite'ın foot pixel'i Y=0 world'de olmalı (Unity Project window > sprite preview > pivot konumu görsel kontrol)

### Output Format

```markdown
# Wall PNG Re-Import — Codex Report

## Applied Settings (per file)
| # | File | PPU before→after | Filter before→after | Pivot before→after | spriteMode |
|---|---|---|---|---|---|
| 1 | straight_horizontal | 100→64 | Bilinear→Point | (0.5,0.5)→(0.5,0.0313) | Multiple→Single |
| ...

## Verify Result
- dotnet build: 0 error
- Sprite pivot visual check: PASS (foot pixel at Y=0)
- Git diff: 5 .meta files modified, no other changes

## Git Diff Summary
{diff stat}

## Açık Sorular (varsa)
- {soru}
```

### Hard Constraints

- **Sadece .meta dosyaları değişsin.** PNG, .cs, scene, prefab dosyalarına dokunma.
- **Auto-commit YOK** — user manual commit eder.
- **BLOCKED if unclear:** Pivot Y hesabı veya spriteMode değişimi belirsizse durdur.
