# Zemin Tile'lari -- Uretim Rehberi (F1 / F2 / F3 + Gecisler)
*MEMORY/pixellab_master_pipeline.md Bolum 0 HARD RULES uygulanir*

---

## TEMEL KURALLAR

- **Tool**: PixelLab web app -> Map bolumu -> **Create Tiles Pro** (FLOOR)
- **Gecisler icin**: **Create Tileset Standard** (Wang mode) -- Create Tiles Pro DEGIL
- **Chromakey**: #00FF00 -> `process_tiles.py` ile temizle (filter: G>200 AND R<60 AND B<60)
- **Pixel Art Mode**: ON | **Upscale**: OFF | **Anti-aliasing**: OFF
- **Uretim sirasi ZORUNLU**: F1 -> F2 -> F3 -> Trans_F1F2 -> Trans_F2F3
- **Style reference**: her set icin onceki onaylanan tile yukle (F2'den itibaren ZORUNLU)
- **YASAK**: Create Tileset Standard floor icin | gradient | anti-aliased edge | palette disi renk | belirsiz renk tanimi ("dark grey" yerine hex kullan)

---

## PALETTE (5 LOCKED RENK)

```
Shadow / outline:  #1A1C20
Dark stone:        #2A2D34
Mid stone:         #3A3D48
Lit face:          #4E5260
Highlight:         #606575
```

Gecis accent (sadece F3 lava vein): `#4A1A1A` / `#6A2A1A` (max 8px uzunluk)
Moss accent (F1/F2): `#263530`

---

## VARIANT TIER SISTEMI (LOCKED 2026-05-11)

Her floor set'in 16 variant'i **3 tier'a** bolunur. PixelLab uretiminde hangi variant hangi tier'a girdigi `metadata.json`'a `tier_tag` olarak yazilir.

| Tier | Variant sayisi | Aciklama | Per-variant weight |
|------|---------------|----------|--------------------|
| **base** | 8 | Mikro-detay (ince catlik, hafif leke) -- tekrar tolere edilir | 6 |
| **accent** | 5 | Orta detay (buyuk catlik, yosun yayilmasi, su lekesi) | 2 |
| **hero** | 3 | Nadir, dikkat cekici (derin oyuk, rune izi, kan lekesi) | 1 |

### Set-ozgu Agirlik Profili

| Set | Base % | Accent % | Hero % | Tone |
|-----|--------|----------|--------|------|
| **F1** (granit) | ~80% | ~15% | ~5% | Temiz, disiplinli |
| **F2** (asinmis) | ~60% | ~30% | ~10% | Yipranmis, kaotik doku |
| **F3** (volkanik) | ~70% | ~20% | ~10% | Dramatik, tehlikeli |

### metadata.json Zorunlulugu

Her F1/F2/F3 variant PNG'si `metadata.json`'a su field'i icermek ZORUNDA:
```json
{ "tier_tag": "base" }
```
Gecerli degerler: `base`, `accent`, `hero`

**WeightedRandomTile SO olusturma scripti** bu field'lari okuyup otomatik weight atayabilir.

---

## PERLIN NOISE PREVIEW SISTEMI (LOCKED 2026-05-11)

Floor variant secimi **edit-time Perlin noise** ile yapilir -> save aninda RoomBlueprint'e fix edilir (runtime random YOK).

**Calisma akisi:**
1. Room Designer'da floor paint -> Perlin sample -> tier secimi -> tier ici deterministik variant
2. Designer **Reseed** butonuyla farkli dagilim ceker (preview)
3. **Save / Bake** -> her hucre `variantIndex` (byte) olarak RoomBlueprint'e yazilir -> artik sabit

**Noise parametreleri:**
| Param | Deger | Aciklama |
|-------|-------|----------|
| frequency | **0.18** | 16x16 odada ~3 blob olusturur |
| octaves | **2** | Ikinci oktav freq 0.36, amplitude 0.4 |
| seed | `noiseSeed` per room | RoomBlueprint field, default = roomGuid.GetHashCode() |

**Tier esikleri (Perlin sample n ∈ [0,1]):**
- `n < 0.70` -> base tier
- `0.70 <= n < 0.92` -> accent tier
- `n >= 0.92` -> hero tier

(F1/F2/F3 profil farkli olsa da esikler sabit; agirlik tier'daki per-variant weight ile ayarlanir)

**Tier ici variant secimi (deterministik):**
`(x * 73856093 XOR y * 19349663 XOR seed) mod tier_size` -> variant index

**Cluster-bust (varsayilan ON):** Save aninda hero tier variant'in 1 hucre radius'unda baska hero varsa accent'e dusurulur.

**YASAK:** Runtime variant randomization. RoomBlueprint `variantIndex[]` save sonrasi degismez.

---

## ASSET TANIMI

| Set | Tool | Boyut | Varyasyon | Biome / Kullanim | Output Klasoru |
|---|---|---|---|---|---|
| F1 | Create Tiles Pro | 64x64px | 16 var | Act 1 giris odalari (cold grey granite) | outputs/f1/ |
| F2 | Create Tiles Pro | 64x64px | 16 var | Act 1 orta odalar (weathered dark stone) | outputs/f2/ |
| F3 | Create Tiles Pro | 64x64px | 16 var | Act 1 boss / derin odalar (volcanic black) | outputs/f3/ |
| Trans_F1F2 | Create Tileset Standard | 64x64px | 8 var | F1-F2 sinir gecis bantlari | outputs/trans/ |
| Trans_F2F3 | Create Tileset Standard | 64x64px | 8 var | F2-F3 sinir gecis bantlari | outputs/trans/ |

> Toplam: 48 floor tile + 16 transition tile = 64 unique tile.

---

## ADIM 1 -- F1 (Cold Grey Granite)

### Settings

| Param | Deger |
|---|---|
| Tool | Create Tiles Pro (Map bolumu, Pro sekmesi) |
| Shape | Isometric |
| View | High top-down |
| Background | #00FF00 |
| Pixel Art Mode | ON |
| Upscale | OFF |
| Anti-aliasing | OFF |
| Style Reference | YOK (F1 ilk set) |

### Uretim Akisi

1. PixelLab -> Map bolumu -> Create Tiles Pro'yu ac.
2. Shape: **Isometric** sec (dropdown: Square / Hex / Isometric / Octagon).
3. View: **High top-down** sec.
4. Background: `#00FF00` gir.
5. Asagidaki prompt'u kopyala-yapistir.
6. **Once 4 varyasyon uret -> QC -> onaylayinca 16'ya cikar.**

### Prompt (copy-paste)

```
Isometric pixel art dungeon floor tile, cold grey granite stone slab.
Flat top-down diamond rhombus view.
Palette strictly: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Fine mortar line (#1A1C20, 1-2px) at slab joints.
Accent (choose one per variant): hairline crack / faint moss (#263530) at grout / stone chip / water stain ring.
Hard pixel edges, NO anti-aliasing. Pixel cluster min 4px. Background #00FF00 chromakey.
```

### Accent Rotasyonu (16 varyasyon)

- **var01-04**: hairline crack (her biri farkli aci)
- **var05-08**: faint moss (#263530) at grout joint corner
- **var09-12**: stone chip near edge
- **var13-16**: water stain ring (faint #2A2D34 ring, 6-8px diameter)

### process_tiles.py Komutu

```powershell
python STAGING/process_tiles.py --source STAGING/PIXELLAB/02_NEXT_floors/outputs/f1/f1_sheet_v1.png --output Assets/Art/Tiles/Act1/F1 --cols 4 --rows 4 --width 64 --height 64 --prefix f1_
```

### Unity Import Ayarlari

| Param | Deger |
|---|---|
| PPU | 64 |
| Pivot | Center (0.5, 0.5) |
| Sorting Layer | Ground |
| Filter Mode | Point (no filter) |
| Compression | None |

> F1 onaylandi mi? En iyi 1 tile'i kaydet -> bir sonraki adimda style reference olarak kullanacaksin.

---

## ADIM 2 -- F2 (Weathered Dark Stone)

### Settings

| Param | Deger |
|---|---|
| Tool | Create Tiles Pro (Map bolumu, Pro sekmesi) |
| Shape | Isometric |
| View | High top-down |
| Background | #00FF00 |
| Pixel Art Mode | ON |
| Upscale | OFF |
| Anti-aliasing | OFF |
| Style Reference | **ZORUNLU** -- en iyi F1 onaylanan tile'i yukle |

### Uretim Akisi

1. Style Reference alanina F1'den en iyi onaylanan tile'i yukle.
2. Asagidaki prompt'u kopyala-yapistir.
3. **Once 4 varyasyon uret -> QC -> 16'ya cikar.**

### Prompt (copy-paste)

```
Isometric pixel art dungeon floor tile, weathered dark stone slab, more worn than F1.
Flat top-down diamond rhombus view.
Palette strictly: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Wider mortar fracture lines (#1A1C20, 2-3px).
Accent (choose one per variant): wider crack / lichen growth (#263530) spreading from joint / broken corner chip / old water damage pattern.
Darker overall vs F1 -- less lit face (#4E5260) usage. Hard pixel edges, NO anti-aliasing. Background #00FF00.
```

### Accent Rotasyonu (16 varyasyon)

- **var01-04**: wider crack crossing tile diagonally
- **var05-08**: lichen (#263530) spreading 4-6px from mortar joint
- **var09-12**: broken corner chip (missing pixel cluster at one corner)
- **var13-16**: old water damage pattern (darker ring + slight discoloration band)

### process_tiles.py Komutu

```powershell
python STAGING/process_tiles.py --source STAGING/PIXELLAB/02_NEXT_floors/outputs/f2/f2_sheet_v1.png --output Assets/Art/Tiles/Act1/F2 --cols 4 --rows 4 --width 64 --height 64 --prefix f2_
```

### Unity Import Ayarlari

| Param | Deger |
|---|---|
| PPU | 64 |
| Pivot | Center (0.5, 0.5) |
| Sorting Layer | Ground |
| Filter Mode | Point (no filter) |
| Compression | None |

> F2 onaylandi mi? En iyi 1 tile'i kaydet -> bir sonraki adimda style reference olarak kullanacaksin.

---

## ADIM 3 -- F3 (Volcanic Black Stone)

### Settings

| Param | Deger |
|---|---|
| Tool | Create Tiles Pro (Map bolumu, Pro sekmesi) |
| Shape | Isometric |
| View | High top-down |
| Background | #00FF00 |
| Pixel Art Mode | ON |
| Upscale | OFF |
| Anti-aliasing | OFF |
| Style Reference | **ZORUNLU** -- en iyi F2 onaylanan tile'i yukle |

### Uretim Akisi

1. Style Reference alanina F2'den en iyi onaylanan tile'i yukle.
2. Asagidaki prompt'u kopyala-yapistir.
3. **Once 4 varyasyon uret -> QC -> 16'ya cikar.**

### Prompt (copy-paste)

```
Isometric pixel art dungeon floor tile, volcanic black stone slab with lava-crack veins.
Flat top-down diamond rhombus view.
Palette strictly: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Lava-crack accent: thin vein (#4A1A1A / #6A2A1A) crossing tile, max 8px total length. NO bright orange/red glow pixels (engine VFX overlay handles glow).
Mortar lines nearly invisible -- stone is fused/continuous.
Hard pixel edges, NO anti-aliasing. Background #00FF00.
```

### Lava Vein Rotasyonu (16 varyasyon)

- **var01-04**: single straight vein, farkli acilar
- **var05-08**: Y-fork vein (short branches, max 8px total)
- **var09-12**: hairline vein near edge only
- **var13-16**: two parallel micro-veins (2px gap between)

### process_tiles.py Komutu

```powershell
python STAGING/process_tiles.py --source STAGING/PIXELLAB/02_NEXT_floors/outputs/f3/f3_sheet_v1.png --output Assets/Art/Tiles/Act1/F3 --cols 4 --rows 4 --width 64 --height 64 --prefix f3_
```

### Unity Import Ayarlari

| Param | Deger |
|---|---|
| PPU | 64 |
| Pivot | Center (0.5, 0.5) |
| Sorting Layer | Ground |
| Filter Mode | Point (no filter) |
| Compression | None |

> F1 + F2 + F3 hazir mi? Unity'de yan yana ciz, hue drift kontrol et. Drift varsa yeniden uret.

---

## ADIM 4 -- Trans_F1F2 (Cold Granite to Weathered Stone)

> **Tool: Create Tileset Standard** (Wang mode) -- Create Tiles Pro DEGIL.

### Settings

| Param | Deger |
|---|---|
| Tool | Create Tileset Standard |
| Mode | Top-Down / Wang transition |
| Tile Size | 64x64 |
| Background | #00FF00 |
| Pixel Art Mode | ON |
| Upscale | OFF |
| Anti-aliasing | OFF |

### Uretim Akisi

1. PixelLab -> Map bolumu -> Create Tileset Standard'i ac.
2. Wang transition modunu sec.
3. Lower terrain (F1 tarafi), Upper terrain (F2 tarafi) ve Transition Description alanlarina asagidaki prompt bloklarini yapistir.
4. Tile size: 64x64. Background: #00FF00.

### Lower Terrain Prompt -- F1 tarafi (copy-paste)

```
Isometric pixel art dungeon floor tile, cold grey granite stone slab.
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Fine mortar lines 1-2px. Background #00FF00.
```

### Upper Terrain Prompt -- F2 tarafi (copy-paste)

```
Isometric pixel art dungeon floor tile, weathered dark stone slab.
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Wider mortar fracture lines 2-3px. Darker overall. Background #00FF00.
```

### Transition Description (copy-paste)

```
Isometric pixel art dungeon floor tile transition from cold grey granite (F1) to weathered dark stone (F2).
Wang-style edge blend at tile boundary. F1 side: #3A3D48 / #4E5260. F2 side: #2A2D34 / #3A3D48.
Mortar lines shift from fine (F1) to wide/cracked (F2) at blend zone.
Hard pixel edges. Background #00FF00.
```

### process_tiles.py Komutu

```powershell
python STAGING/process_tiles.py --source STAGING/PIXELLAB/02_NEXT_floors/outputs/trans/trans_f1f2_v1.png --output Assets/Art/Tiles/Act1/Trans_F1F2 --cols 2 --rows 4 --width 64 --height 64 --prefix trans_f1f2_
```

### Unity Import Ayarlari

| Param | Deger |
|---|---|
| PPU | 64 |
| Pivot | Center (0.5, 0.5) |
| Sorting Layer | Ground |
| Filter Mode | Point (no filter) |
| Compression | None |

> QC: Unity'de 5x5 patch ciz, tum kose durumlarini test et. Edge blend tekrar ederse veya kosede kiriliyor ise daha guclu transition description ile yeniden uret -- manuel edit yapma.

---

## ADIM 5 -- Trans_F2F3 (Weathered Stone to Volcanic)

> **Tool: Create Tileset Standard** (Wang mode) -- Create Tiles Pro DEGIL.

### Settings

| Param | Deger |
|---|---|
| Tool | Create Tileset Standard |
| Mode | Top-Down / Wang transition |
| Tile Size | 64x64 |
| Background | #00FF00 |
| Pixel Art Mode | ON |
| Upscale | OFF |
| Anti-aliasing | OFF |

### Uretim Akisi

1. PixelLab -> Map bolumu -> Create Tileset Standard'i ac.
2. Wang transition modunu sec.
3. Lower terrain (F2 tarafi), Upper terrain (F3 tarafi) ve Transition Description alanlarina asagidaki prompt bloklarini yapistir.
4. Tile size: 64x64. Background: #00FF00.

### Lower Terrain Prompt -- F2 tarafi (copy-paste)

```
Isometric pixel art dungeon floor tile, weathered dark stone slab.
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Wider mortar fracture lines 2-3px. Background #00FF00.
```

### Upper Terrain Prompt -- F3 tarafi (copy-paste)

```
Isometric pixel art dungeon floor tile, volcanic black stone slab.
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Fused stone surface, faint lava-crack vein (#4A1A1A). Background #00FF00.
```

### Transition Description (copy-paste)

```
Isometric pixel art dungeon floor tile transition from weathered dark stone (F2) to volcanic lava-crack stone (F3).
Wang-style edge blend. F2 side: dark stone, mortar cracks. F3 side: black volcanic stone, thin lava vein hint (#4A1A1A) near edge.
NO bright glow. Hard pixel edges. Background #00FF00.
```

### process_tiles.py Komutu

```powershell
python STAGING/process_tiles.py --source STAGING/PIXELLAB/02_NEXT_floors/outputs/trans/trans_f2f3_v1.png --output Assets/Art/Tiles/Act1/Trans_F2F3 --cols 2 --rows 4 --width 64 --height 64 --prefix trans_f2f3_
```

### Unity Import Ayarlari

| Param | Deger |
|---|---|
| PPU | 64 |
| Pivot | Center (0.5, 0.5) |
| Sorting Layer | Ground |
| Filter Mode | Point (no filter) |
| Compression | None |

> QC: Unity'de 5x5 patch ciz, tum kose durumlarini test et. F3 tarafinda bright orange/red pixel gorursen REJECT et -- NO glow pixels kurali (VFX overlay engine'de yapilacak).

---

## QC CHECKLIST (her batch oncesi)

### Tile Kalitesi

- [ ] 2:1 isometric diamond rhombus sekli (kusursuz, egri degil)
- [ ] Background tam #00FF00 (yakin-yesil veya beyaz degil)
- [ ] Tum pikseller 5 locked palette rengi kullanmis (+ izin verilen accent)
- [ ] Pixel cluster min 4px (tek piksel scatter yok)
- [ ] Hard pixel edge -- anti-aliasing fringi sifir
- [ ] Seamless edge: tile diamond sinirlarinda flush (bosluk veya tasma yok)

### Set Ozellikleri

- [ ] **F1**: bir subtle accent var (crack / moss / chip / water mark)
- [ ] **F2**: wider mortar (2-3px) + bir asinma accenti; F1'den gorsel olarak daha karanlik
- [ ] **F3**: lava vein max 8px; bright orange/red YOK; tas yuzey fused/continuous gorunuyor
- [ ] **Transitions**: Create Tileset Standard kullanildi (Create Tiles Pro DEGIL)

### Tutarlilik

- [ ] F2 uretiminde F1 approved tile style reference olarak yuklendi
- [ ] F3 uretiminde F2 approved tile style reference olarak yuklendi
- [ ] Palette hex'leri prompt'ta aynen yazildi ("dark grey" gibi belirsiz terim yok)
- [ ] F1 + F2 + F3 Unity'de yan yana: hue drift yok
- [ ] Transition tiles Unity'de 5x5 patch: tum kose durumlarinda dogru blend

### Dosya / Klasor

- [ ] F1 ciktilari `outputs/f1/` altinda
- [ ] F2 ciktilari `outputs/f2/` altinda
- [ ] F3 ciktilari `outputs/f3/` altinda
- [ ] Transition ciktilari `outputs/trans/` altinda
- [ ] process_tiles.py her set icin calistirildi
- [ ] Assets/Art/Tiles/Act1/ altinda dogru alt klasorlere kopyalandi

### Variant Tier + Noise

- [ ] Her variant PNG metadata.json tier_tag iceriyor (base/accent/hero)
- [ ] F1 base-heavy (8 base var), F2 accent-agirlikli (5 accent var), F3 hero-permissive (3 hero var) -- uretim sirasinda tier dengesi kontrol
- [ ] Animated tile YOKTUR floor setinde -- animated tiles prop kategorisinde (torch vs.)

---

## KAYIT KLASORU

```
STAGING/PIXELLAB/02_NEXT_floors/
  outputs/
    f1/
      f1_sheet_v1.png          (4x4 sprite sheet, 16 var)
    f2/
      f2_sheet_v1.png
    f3/
      f3_sheet_v1.png
    trans/
      trans_f1f2_v1.png        (2x4 sprite sheet, 8 var)
      trans_f2f3_v1.png

Assets/Art/Tiles/Act1/
  F1/     (16 x f1_XX.png)
  F2/     (16 x f2_XX.png)
  F3/     (16 x f3_XX.png)
  Trans_F1F2/  (8 x trans_f1f2_XX.png)
  Trans_F2F3/  (8 x trans_f2f3_XX.png)
```

5 set x (16+16+16+8+8) = **64 unique tile**. process_tiles.py her set icin ayri calistirilir.
