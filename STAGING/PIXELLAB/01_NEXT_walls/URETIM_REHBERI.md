# Duvar Tile'lari -- Uretim Rehberi (W1 / W2 / OBW)
*MEMORY/pixellab_master_pipeline.md Bolum 0 HARD RULES uygulanir*

---

## TEMEL KURALLAR

- **Tool**: PixelLab → Create Tile — Isometric (Map bolumunde, dedicated isometric wall tool)
- **YASAK**: Create Tileset Standard (sadece 16x32 uretir) | Create Tiles Pro (floor icin)
- **Boyut**: 64x128px -- 64x96 PixelLab'da YOK
- **Background**: Pure solid green #00FF00 (chromakey) -- alpha DEGIL
- **Pixel Art Mode**: ON | Upscale: OFF | Anti-aliasing: OFF
- **Her variant icin**: 4 candidate uret → QC → 1 sec → sonrakine gec
- **Style reference zorunlu**: W1-straight-H onaylayinca tum diger W1 variantlari icin yukle
- **Uretim sirasi**: W1 → W2 → OBW (bu siradan sapma)
- **Chromakey temizligi**: process_tiles.py ile yap (wall objeler icin zorunlu)

---

## UNITY RULE TILE SİSTEMİ (LOCKED 2026-05-11)

Wall tile'lari **Unity Rule Tile** kullanir -- designer sadece "wall var/yok" cizer, variant otomatik secilir.

**Mod A -- Auto-connect (default):** Room Designer'da brush hucreye basar -> Rule Tile komsuları degerlendirir -> dogru variant gorünür.
**Mod B -- Manual override:** Palette'te 8 variant acilir; override koyulan hücre `overrideVariantIndex` ile RoomBlueprint'e yazilir. O hücre Rule Tile degerlendirmesini skip eder ama komsu degerlendirmesinde hala "wall" sayilir.

### Connection Type -> Variant Mapping

| # | N | S | E | W | Variant |
|---|---|---|---|---|---------|
| 1 | W | W | . | . | straight-V |
| 2 | . | . | W | W | straight-H |
| 3 | . | W | . | W | corner-NW |
| 4 | . | W | W | . | corner-NE |
| 5 | W | . | . | W | corner-SW |
| 6 | W | . | W | . | corner-SE |
| 7 | . | . | W | . | end-L |
| 8 | . | . | . | W | end-R |
| 9 | W | W | W | W | straight-V (T/cross fallback) |
| 10 | . | . | . | . | straight-V (isolated -- Room Designer warning) |

`W` = wall komsus var, `.` = yok, `*` = don't care. N/S/E/W grid-relative (view-relative degil).

### metadata.json Zorunlulugu

Her W1/W2 variant PNG'si `metadata.json`'a su field'i icermek ZORUNDA:

```json
{ "connection_type": "straight_V" }
```

Gecerli degerler: `straight_H`, `straight_V`, `corner_NW`, `corner_NE`, `corner_SW`, `corner_SE`, `end_L`, `end_R`

**Rule Tile asset generation scripti** bu field'lari okuyup `W1_RuleTile.asset`'i otomatik olusturur. Manuel Rule Tile yazma YASAK -- hata riskli.

### Validation

Room Designer'da izole wall (4 komsu yok) detection -> "Isolated wall at (x,y)" warning. T/cross junction -> straight-V fallback, level design hatasi isareti.

---

## PALETTE (5 LOCKED)

### W1 / OBW Palette

```
Shadow / outline:  #1A1C20
Dark stone:        #2A2D34
Mid stone:         #3A3D48
Lit face:          #4E5260
Highlight:         #606575
```

### W2 Palette (W1'den soguk/mavi tonayli)

```
Deep mortar:       #18181E
Very dark stone:   #26293A
Dark mid stone:    #363A4A
Mid stone:         #464B5E
Lit face:          #565C70
```

---

## ASSET TANIMI

| Tip | Boyut | Variant Sayisi | Tool | Style Ref |
|---|---|---|---|---|
| W1 | 64x128px | 8 (straight x2, corner x4, end-cap x2) | Create Tile — Isometric | F1 approved floor tile |
| W2 | 64x128px | 8 (W1 ile ayni set) | Create Tile — Isometric | W1 approved tile |
| OBW | 64x128px | 4 (plain, window slit, bracket, rune) | Create Tile — Isometric | W1 approved tile |

---

## VARIANT URETIM SIRASI (NEDEN BU SIRA)

```
straight-H  →  straight-V  →  corner-NW / NE  →  corner-SW / SE  →  end-L / end-R
```

- **straight-H**: Stil anchor. Tum diger variantlar bu tiledan palette ve brick pattern alir.
- **straight-V**: Ayni palette, dik orientasyonda geometry. H onceden onayli olmali ki referans yuklenebilsin.
- **corner-NW / NE**: Dis kose (convex). Straight tile'lar onayli olmali ki kose silhouette'i straight'e baglansın.
- **corner-SW / SE**: Ic kose (concave). Dis koseler onayli olmali -- ic kose shadow depth'i dis koseyi referans alir.
- **end-L / end-R**: End cap. En sona birakma nedeni: quoin stone pattern straight + corner onayli olunca daha tutarli cikar.

---

## SETTINGS (her W1/W2/OBW cagrisinda)

| Param | Value |
|---|---|
| Tool | Create Tile — Isometric |
| Background | #00FF00 (chromakey green) |
| Pixel Art Mode | ON |
| Upscale | OFF |
| Anti-aliasing | OFF |
| Style Reference | Adim aciklamasina gore yukle |

---

## ADIM 1 -- W1 Straight-H (Stil Anchor -- ilk uretim)

> Bu tile tum W1 setinin style reference'idir. 4 candidate uret, QC gec, 1 sec, sonra kaydet.
> Style reference YOKTUR bu adimda -- bu tile style anchor kendisidir.

**Settings**

| Param | Value |
|---|---|
| Tool | Create Tile — Isometric |
| Background | #00FF00 |
| Style Reference | F1 approved floor tile (palette tutarlilik icin) |
| Candidates | 4 |

**Prompt** (copy-paste)

```
Pixel art dungeon stone wall segment, horizontal straight section, low top-down 35 degree view.
Top face (narrow, top ~20px): dark stone slab #2A2D34, slight lit edge #3A3D48.
Front face (tall, bottom ~110px): rough-cut stone blocks, stacked rows.
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Block mortar lines 1-2px #1A1C20. Block size ~16x12px irregular.
Subtle variation: one block slightly lighter/darker per segment.
Transparent background. Hard pixel edges, NO anti-aliasing.
```

**process_tiles.py komutu**

```powershell
python STAGING/process_tiles.py --source "STAGING/PIXELLAB/01_NEXT_walls/outputs/w1/w1_straight_h_v1.png" --output Assets/Art/Tiles/Act1/W1 --cols 1 --rows 1 --width 64 --height 128 --prefix w1_straight_h_
```

**Unity Import**

- Folder: `Assets/Art/Tiles/Act1/W1/`
- Sprite Mode: Single | PPU: 64 | Pivot: Bottom Center (0.5, 0.0)
- Sorting layer: Walls | Y-sort aktif
- Filter: Point | Compression: None

> Bu adim tamamlanmadan ADIM 2'ye gecme. Onaylanan tile -> style reference olarak kaydet.

---

## ADIM 2 -- W1 Straight-V

> Style reference: ADIM 1'de onaylanan W1-straight-H.

**Settings**

| Param | Value |
|---|---|
| Tool | Create Tile — Isometric |
| Background | #00FF00 |
| Style Reference | W1-straight-H (ADIM 1 onayli) |
| Candidates | 4 |

**Prompt** (copy-paste)

```
Pixel art dungeon stone wall segment, vertical straight section perpendicular to horizontal, low top-down 35 degree view.
Wall runs depth-wise from viewer perspective (receding into background).
Top face (narrow, upper strip): dark stone slab #2A2D34, lit edge #3A3D48.
Front face or side face visible: rough-cut stone blocks, stacked rows, same palette.
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Block mortar lines 1-2px #1A1C20. Block size ~16x12px irregular.
Subtle variation: one block slightly lighter/darker.
Transparent background. Hard pixel edges, NO anti-aliasing.
```

**process_tiles.py komutu**

```powershell
python STAGING/process_tiles.py --source "STAGING/PIXELLAB/01_NEXT_walls/outputs/w1/w1_straight_v_v1.png" --output Assets/Art/Tiles/Act1/W1 --cols 1 --rows 1 --width 64 --height 128 --prefix w1_straight_v_
```

**Unity Import**

- Folder: `Assets/Art/Tiles/Act1/W1/`
- Sprite Mode: Single | PPU: 64 | Pivot: Bottom Center (0.5, 0.0)
- Sorting layer: Walls | Y-sort aktif

---

## ADIM 3 -- W1 Corner-NW / NE (Dis Kose Cifti)

> Ikisini ayni oturumda arka arkaya uret. Style reference: W1-straight-H.
> NW urettikten sonra QC gec, sec, sonra NE icin yeni call ac.

### ADIM 3a -- Corner-NW

**Settings**

| Param | Value |
|---|---|
| Tool | Create Tile — Isometric |
| Background | #00FF00 |
| Style Reference | W1-straight-H (ADIM 1 onayli) |
| Candidates | 4 |

**Prompt** (copy-paste)

```
Pixel art dungeon stone wall corner piece, NW outer corner junction, low top-down 35 degree view.
Two wall faces meet at 90 degree outer corner (convex). Left wall face and right wall face both partially visible.
Corner block is a large quoin stone (alternating long-short course pattern for structural look).
Top face visible on both arms, meeting at corner apex.
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Mortar lines 1-2px #1A1C20. Hard pixel edges, transparent background.
```

**process_tiles.py komutu**

```powershell
python STAGING/process_tiles.py --source "STAGING/PIXELLAB/01_NEXT_walls/outputs/w1/w1_corner_nw_v1.png" --output Assets/Art/Tiles/Act1/W1 --cols 1 --rows 1 --width 64 --height 128 --prefix w1_corner_nw_
```

### ADIM 3b -- Corner-NE

**Settings**

| Param | Value |
|---|---|
| Tool | Create Tile — Isometric |
| Background | #00FF00 |
| Style Reference | W1-straight-H (ADIM 1 onayli) |
| Candidates | 4 |

**Prompt** (copy-paste)

```
Pixel art dungeon stone wall corner piece, NE outer corner junction, low top-down 35 degree view.
Two wall faces meet at 90 degree outer corner (convex). Mirrored from NW -- right arm extends right, left arm extends toward viewer.
Corner block is a large quoin stone (alternating long-short course).
Top face visible on both arms.
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Mortar lines 1-2px #1A1C20. Hard pixel edges, transparent background.
```

**process_tiles.py komutu**

```powershell
python STAGING/process_tiles.py --source "STAGING/PIXELLAB/01_NEXT_walls/outputs/w1/w1_corner_ne_v1.png" --output Assets/Art/Tiles/Act1/W1 --cols 1 --rows 1 --width 64 --height 128 --prefix w1_corner_ne_
```

**Unity Import (her ikisi)**

- Folder: `Assets/Art/Tiles/Act1/W1/`
- Sprite Mode: Single | PPU: 64 | Pivot: Bottom Center (0.5, 0.0)
- Sorting layer: Walls | Y-sort aktif

---

## ADIM 4 -- W1 Corner-SW / SE (Ic Kose Cifti)

> Style reference: NW onayli corner (ADIM 3a). Ic kose shadow depth'i dis koseyi referans alir.
> SW once, QC gec, sonra SE.

### ADIM 4a -- Corner-SW

**Settings**

| Param | Value |
|---|---|
| Tool | Create Tile — Isometric |
| Background | #00FF00 |
| Style Reference | W1-corner-NW (ADIM 3a onayli) |
| Candidates | 4 |

**Prompt** (copy-paste)

```
Pixel art dungeon stone wall corner piece, SW inner corner junction, low top-down 35 degree view.
Two wall faces meet at 90 degree inner corner (concave). Interior recess visible.
Corner joint where blocks interlock; darker shadow in recess (#1A1C20 fill in concave zone).
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Mortar lines 1-2px #1A1C20. Hard pixel edges, transparent background.
```

**process_tiles.py komutu**

```powershell
python STAGING/process_tiles.py --source "STAGING/PIXELLAB/01_NEXT_walls/outputs/w1/w1_corner_sw_v1.png" --output Assets/Art/Tiles/Act1/W1 --cols 1 --rows 1 --width 64 --height 128 --prefix w1_corner_sw_
```

### ADIM 4b -- Corner-SE

**Settings**

| Param | Value |
|---|---|
| Tool | Create Tile — Isometric |
| Background | #00FF00 |
| Style Reference | W1-corner-NW (ADIM 3a onayli) |
| Candidates | 4 |

**Prompt** (copy-paste)

```
Pixel art dungeon stone wall corner piece, SE inner corner junction, low top-down 35 degree view.
Two wall faces meet at 90 degree inner corner (concave). Mirrored from SW.
Interior recess with deep shadow (#1A1C20) where faces meet.
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Mortar lines 1-2px #1A1C20. Hard pixel edges, transparent background.
```

**process_tiles.py komutu**

```powershell
python STAGING/process_tiles.py --source "STAGING/PIXELLAB/01_NEXT_walls/outputs/w1/w1_corner_se_v1.png" --output Assets/Art/Tiles/Act1/W1 --cols 1 --rows 1 --width 64 --height 128 --prefix w1_corner_se_
```

**Unity Import (her ikisi)**

- Folder: `Assets/Art/Tiles/Act1/W1/`
- Sprite Mode: Single | PPU: 64 | Pivot: Bottom Center (0.5, 0.0)
- Sorting layer: Walls | Y-sort aktif

---

## ADIM 5 -- W1 End-L / End-R (End Cap Cifti)

> Son W1 adimidir. Style reference: W1-straight-H. Quoin stone pattern kontrol et.
> End-L once, QC gec, sonra End-R.

### ADIM 5a -- End-L

**Settings**

| Param | Value |
|---|---|
| Tool | Create Tile — Isometric |
| Background | #00FF00 |
| Style Reference | W1-straight-H (ADIM 1 onayli) |
| Candidates | 4 |

**Prompt** (copy-paste)

```
Pixel art dungeon stone wall end cap, left terminal end, low top-down 35 degree view.
Front face visible, plus a perpendicular end face (left side).
End face uses quoin stone pattern: alternating long and short blocks at the terminal edge.
End face is slightly darker than front face (less direct light).
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Mortar lines 1-2px #1A1C20. Transparent background. Hard pixel edges, NO anti-aliasing.
```

**process_tiles.py komutu**

```powershell
python STAGING/process_tiles.py --source "STAGING/PIXELLAB/01_NEXT_walls/outputs/w1/w1_end_l_v1.png" --output Assets/Art/Tiles/Act1/W1 --cols 1 --rows 1 --width 64 --height 128 --prefix w1_end_l_
```

### ADIM 5b -- End-R

**Settings**

| Param | Value |
|---|---|
| Tool | Create Tile — Isometric |
| Background | #00FF00 |
| Style Reference | W1-straight-H (ADIM 1 onayli) |
| Candidates | 4 |

**Prompt** (copy-paste)

```
Pixel art dungeon stone wall end cap, right terminal end, low top-down 35 degree view.
Front face visible, plus a perpendicular end face (right side).
End face uses quoin stone pattern: alternating long and short blocks at the terminal edge.
End face is slightly darker than front face (less direct light).
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Mortar lines 1-2px #1A1C20. Transparent background. Hard pixel edges, NO anti-aliasing.
```

**process_tiles.py komutu**

```powershell
python STAGING/process_tiles.py --source "STAGING/PIXELLAB/01_NEXT_walls/outputs/w1/w1_end_r_v1.png" --output Assets/Art/Tiles/Act1/W1 --cols 1 --rows 1 --width 64 --height 128 --prefix w1_end_r_
```

**Unity Import (her ikisi)**

- Folder: `Assets/Art/Tiles/Act1/W1/`
- Sprite Mode: Single | PPU: 64 | Pivot: Bottom Center (0.5, 0.0)
- Sorting layer: Walls | Y-sort aktif

> W1 seti tamamlandi (8 variant). W2'ye gecmeden once Unity'de W1 set testini yap.

---

## ADIM 6 -- W2 (Daha Derin / Daha Bozulmus Duvar)

> W1 setinin tamaminin Unity'de QC'si gecmis olmali.
> Style reference: W1 approved tile (ortak palette zorunlu -- W2 palette W1'e yakin ama soguk).
> W2 de 8 variant uretir: straight x2, corner x4, end-cap x2.
> Her variant icin W1 adimlarindaki sirayi tekrarla (H → V → NW/NE → SW/SE → L/R).

**Settings**

| Param | Value |
|---|---|
| Tool | Create Tile — Isometric |
| Background | #00FF00 |
| Style Reference | W1 approved tile (W1-straight-H) |
| Candidates | 4 (per variant) |

**Prompt** (copy-paste -- tum W2 variantlari icin temel prompt; variant-specifik geometry aciklamasini W1 prompt'larindan aynen ekle)

```
Isometric pixel art stone wall tile, 64x128 pixels, 2:1 isometric projection. Pure solid green #00FF00 background.

Same three-zone structure as W1: top face 12px, front face 104px, base shadow 12px.

Palette STRICTLY: #18181E (deep mortar), #26293A (very dark stone), #363A4A (dark mid stone), #464B5E (mid stone), #565C70 (lit face). Slightly cooler/bluer than W1 -- deeper dungeon feeling.

Same flat-shaded pixel art rules: NO gradient, NO smooth shading, dithered only, hard pixel edges, pixel clusters min 4px.

Brick pattern shows MORE damage than W1: wider mortar cracks, missing brick corners, occasional horizontal fracture line crossing 2-3 bricks. One variation has a faint bioluminescent lichen vein (#1E3028, max 8px) along a crack.

Same 8 connection variants as W1: straight north, straight south, outer corner NE, outer corner NW, inner corner, T-junction, end-cap north, end-cap south.
```

> W2 variant geometry icin W1 adimlarindaki variant-specific prompt bloklarini AYNEN kullan (sadece palette satirlarini W2 palette ile degistir).

**process_tiles.py komutu (tam W2 set sheet icin)**

```powershell
python STAGING/process_tiles.py --source "STAGING/PIXELLAB/01_NEXT_walls/outputs/w2/w2_sheet_v1.png" --output Assets/Art/Tiles/Act1/W2 --cols 4 --rows 2 --width 64 --height 128 --prefix w2_
```

**Unity Import**

- Folder: `Assets/Art/Tiles/Act1/W2/`
- Sprite Mode: Single | PPU: 64 | Pivot: Bottom Center (0.5, 0.0)
- Sorting layer: Walls | Y-sort aktif

> QC: W1 ve W2'yi Unity'de yan yana koy, palette drift kontrol et. Soguk/mavi ton fark gorunmeli ama brick geometry tutarli olmali.

---

## ADIM 7 -- OBW (Outer Boundary Wall -- Yuksek Duvar)

> W1 + W2 Unity'de onaylandiktan sonra uret.
> OBW connection-aware tile degildir -- 4 standalone varyasyon.
> Style reference: W1 approved tile.

**Settings**

| Param | Value |
|---|---|
| Tool | Create Tile — Isometric |
| Background | #00FF00 |
| Style Reference | W1 approved tile |
| Candidates | 4 (tum 4 varyasyonu tek call'da uret veya ayri ayri) |

**Prompt** (copy-paste)

```
Isometric pixel art tall architectural wall section, 64x128 pixels, 2:1 isometric. Pure solid green #00FF00 background. This is a TALLER wall section -- no top face visible (wall extends above frame), no base shadow (wall extends below frame). Front face only: 128px vertical stone masonry. Palette: #1A1C20, #2A2D34, #3A3D48, #4E5260. Flat-shaded, dithered, hard pixel edges. 4 variations: plain stone, with narrow window slit (4x12px dark void), with iron wall bracket, with carved rune (simple geometric glyph, 8x8px, raised relief).
```

**process_tiles.py komutu**

```powershell
python STAGING/process_tiles.py --source "STAGING/PIXELLAB/01_NEXT_walls/outputs/obw/obw_sheet_v1.png" --output Assets/Art/Tiles/Act1/OBW --cols 2 --rows 2 --width 64 --height 128 --prefix obw_
```

**Unity Import**

- Folder: `Assets/Art/Tiles/Act1/OBW/`
- Sprite Mode: Single | PPU: 64 | Pivot: Bottom Center (0.5, 0.0)
- Sorting layer: Walls | Y-sort aktif
- Not: OBW dungeon sinir duvarlari icin -- WallOcclusionFader fadeRadius 2.2, minAlpha 0.38

---

## QC CHECKLIST

### Her variant oncesi / sonrasi

- [ ] Tool: Create Tile — Isometric mi (Create Tileset Standard / Create Tiles Pro degil)
- [ ] Boyut: 64x128px mi (64x96 cikmissa REJECT)
- [ ] Background tam #00FF00 mi (process script gri/kahverengi pixel'i tile icine sizmasin)
- [ ] Pixel Art Mode ON, Upscale OFF, Anti-aliasing OFF mi

### W1 Palette Kontrol

- [ ] Sadece #1A1C20 / #2A2D34 / #3A3D48 / #4E5260 / #606575 (baska renk REJECT)
- [ ] Pixel cluster min 4px (smooth gradient YASAK)
- [ ] Hard pixel edge (anti-alias yok)

### W2 Palette Kontrol

- [ ] Sadece #18181E / #26293A / #363A4A / #464B5E / #565C70
- [ ] W1'den belirgin sekilde soguk/mavi mi (palette drift yok ama iki set ayirt edilebilmeli)

### Geometry Kontrol

- [ ] Top face: ~12-20px strip gorunuyor mu
- [ ] Front face: kalan yukseklik brick masonry
- [ ] Straight: iki tarafta da wall devam ediyor mu (end cap degil)
- [ ] Corner NW/NE: dis kose (convex), iki kol da gorunuyor mu
- [ ] Corner SW/SE: ic kose (concave), recess shadow #1A1C20 dolu mu
- [ ] End-L/R: terminal yuzde quoin stone pattern var mi
- [ ] Block boyutu ~16x12px irregular (tam uniform grid REJECT)
- [ ] Mortar line 1-2px, #1A1C20 (daha kalin veya eksik REJECT)
- [ ] One weathering accent per tile max (W1: catlak/chipped brick/demir halka)
- [ ] W2: Hasar W1'den fazla mi (wider cracks, missing corners)

### Stil Tutarliligi

- [ ] Style reference ADIM aciklamasina gore yuklendi mi
- [ ] W1-straight-H ile W1 diger variantlar arasinda brick pattern tutarli mi
- [ ] W1 + W2 Unity yan yana: soguk ton fark gorunuyor mu

### Rule Tile Sistemi Kontrol

- [ ] Her variant PNG metadata.json connection_type field iceriyor (required for Rule Tile auto-gen)
- [ ] W1_RuleTile.asset guncellendi (script ile, manuel degil)

### Unity Import Sonrasi

- [ ] PPU = 64
- [ ] Pivot = Bottom Center (0.5, 0.0)
- [ ] Sorting layer = Walls, Y-sort aktif
- [ ] Filter = Point, Compression = None
- [ ] WallOcclusionFader ekli mi (Wall Tilemap'e: fadeRadius 2.2, minAlpha 0.38)

---

## KAYIT KLASORU

```
STAGING/PIXELLAB/01_NEXT_walls/outputs/
  w1/
    w1_straight_h_v1.png
    w1_straight_v_v1.png
    w1_corner_nw_v1.png
    w1_corner_ne_v1.png
    w1_corner_sw_v1.png
    w1_corner_se_v1.png
    w1_end_l_v1.png
    w1_end_r_v1.png
  w2/
    w2_sheet_v1.png  (veya birer birer, W1 ile ayni isimlendirme)
  obw/
    obw_sheet_v1.png

Assets/Art/Tiles/Act1/
  W1/  (8 slice'li PNG'ler, w1_ prefix)
  W2/  (8 slice'li PNG'ler, w2_ prefix)
  OBW/ (4 slice'li PNG'ler, obw_ prefix)
```

W1: 8 variant. W2: 8 variant. OBW: 4 variant. Toplam: 20 tile.
