# PixelLab `create_tiles_pro` — F1 Shattered Keep Floor Pilot (Natural)

**Tarih:** 2026-05-13
**Tool:** PixelLab web UI → `create_tiles_pro`
**Amaç:** Room designer test için F1 floor batch (4 doğal varyant tek generation)

## Form Settings

| Alan | Değer | Gerekçe |
|---|---|---|
| **Tile type** | **Square top-down** | Karar #100 chibi top-down, 32x32 grid |
| **Tile size** | **32×32 px** | S59 LOCKED |
| **View angle** | **High top-down ~30-35°** (en yüksek top-down açı, hafif eğim) | Karar #100, Hades match |
| **Thickness** | **0 / minimal** (none veya en düşük) | Floor tile — kalınlık görünmemeli, sadece zemin |

## Description (numbered — multi-tile generation)

```
1) cracked dark rubble stone floor, irregular masonry chunks in #2C2A2A, hairline cyan rift cracks scattered asymmetrically, fine grey dust gathered between stones, weathered edges

2) worn stone floor with cold grey-green lichen patches in corners, dark #2C2A2A stone base, occasional moss tufts, subtle mineral staining, no uniform pattern

3) broken flagstone slab, fragmented stone pieces with chipped edges, scattered pale ash and bone-dust, half-buried silver rune fragments, faint cold blue glow #7BA7BC residue

4) worn smooth stone path tile, foot-traffic polished surface, soft gradient from #2C2A2A to #4A3F3F, sparse hairline cracks, dust-pocked micro-texture
```

## General Style Block (genel açıklama)

```
Style: Salt and Sanctuary chibi-serious + Hades theatrical mythic. Vivid Vulnerability tonal model — dramatic via color contrast and scale, NOT through silence or restraint.

Palette: dark rubble stone (#2C2A2A primary), front-face stone (#4A3F3F shadow), cold blue rift accent (#7BA7BC, sparingly), pale dust whites. Rift cracks = cyan only, NEVER blood red, NEVER yellow.

Mood: ruined controlled architecture — old keep that still has readable order under the fracture damage. Ritual catastrophe aesthetic (void cracks, broken sigils), NOT gore/horror.

Pixel art: mat painterly, no anti-alias soft blur, high contrast within palette, crisp pixel boundaries. Each tile must read as a unique weathered piece — asymmetric crack placement, irregular debris, no two tiles identical-looking.

Variation principles for natural look:
- Asymmetric weathering patterns (cracks/moss/debris off-center)
- Each tile micro-detail unique (no copy-paste feel)
- Subtle within-palette color noise (avoid flat color blocks)
- Edge irregularity — no perfect borders, organic stone shape variation
- Tile seams should read as natural cracks, not procedural grid lines
```

## Negative Prompt

```
NO bright cartoon colors, NO green grass, NO outdoor forest greenery (this is interior keep ruins).
NO blood splatter, NO gore — rift theme is void/ritual catastrophe, not horror.
NO uniform repetition, NO perfect grid alignment, NO copy-paste micro-detail.
NO outlined cartoon style, NO cel-shading. Use mat painterly pixel art only.
NO isometric or hex perspective — square top-down ONLY.
NO yellow/orange torch glow on tile (lighting handled in Unity, not baked).
NO floor borders or frames — tiles must be edge-to-edge seamless.
NO smooth anti-aliased gradients — pixel-honest dithering only.
```

## Pilot → Batch Disiplini (Karar #90)

1. Bu 4 varyantı **pilot** olarak üret (1 generation).
2. Aseprite cleanup: kenar artefakt, palet düzeltme, tileable check.
3. Unity import: `TileImportWizard` → PPU=64, Point filter, transparent.
4. Test scene'de `WallAutoConnect` (dikişler) + `FloorVariantPainter` (variant blend) ile dik kontrol.
5. PASS ise: aynı style block ile **8×8 / 64 cell** batch generation — F1 floor library tam set.
6. FAIL ise: hangi varyant başarısız → ayrı re-gen, style block revize.

## Sonraki Adım (Wall W1)

Floor PASS sonrası ayrı dispatch: W1 wall pilot — 4-bit NSEW maskeli 8 variant için ayrı prompt.
Wall thickness: low-medium (yapı hissi için), view angle aynı.

## Açık Sorular

1. **View angle slider** PixelLab'da kaç derece? Eğer numeric ise 30-35 ver. Eğer preset ise "high top-down" / "highest" seç.
2. **Thickness 0 mı yoksa minimal mi?** Floor için ideali 0 — duvar değil. Test edip görmek lazım.
