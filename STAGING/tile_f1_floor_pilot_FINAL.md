# PixelLab F1 Shattered Keep Tileset — FINAL Pilot Prompt

**Tarih:** 2026-05-13 (S66 LOCK sonrası)
**Tool:** PixelLab web UI → **Maps > Tileset > Pro** (Gemini-backed, 20 generations)
**Amaç:** Room designer test için connected floor↔wall Wang autotile + natural look

**Locked kararlar:**
- Karar #113: ~35° high top-down tek konverjans (Hades reference)
- Karar #114: 8 yön animasyon (tile için angle bilgisi)
- Karar #100: 32x32, PPU=64, chibi
- Karar #77: Vivid Vulnerability (chibi-but-serious + neon)
- Karar #75 REVISION: `create_topdown_tileset` Pro mode onaylı

---

## Form Ayarları (UI'da bunları seç)

| Alan | Değer | Gerekçe |
|---|---|---|
| **Mode** | **Pro** (Standard değil) | Gemini kalite, palet sadakati |
| **Tile Size** | **32×32** | Karar #100 |
| **Map Orientation** (Standard'da görünür) | Top-down | RIMA core |
| **Shape Controls** | Default rounded rectangle | RIMA için optimal |
| **Transition Height** | **10%** | Düz zemin, hafif yükselti hissi |

### Advanced Options

| Slider | Değer | Gerekçe |
|---|---|---|
| **Transition** | **10%** | Keskin duvar (keep ruins sert mimari) |
| **Spread** | **25%** | Orta — taş duvar dik ama tamamen sert değil |
| **Raggedness** | **45%** | **🔑 DOĞAL GÖRÜNÜM ANAHTARI** — kırık taş hissi |

---

## Lower Terrain (Zemin)

```
Dark rubble stone floor for a shattered keep, 32x32 top-down pixel art tile terrain, viewed from approximately 35 degrees high top-down angle (Hades reference). Uneven charcoal flagstones, worn cracked mortar, chipped slab edges, subtle cold blue shadow tint, muted #2C2A2A base value, sparse dust and tiny stone chips, hairline cyan rift cracks scattered asymmetrically. Gritty Vivid Vulnerability mood, Salt and Sanctuary chibi-but-serious tone, readable at 32x32, tileable, no props, no characters, mat painterly pixel art, no anti-aliased gradients, heavy texture, dark gritty palette.
```

---

## Upper Terrain (Duvar)

```
Dark broken stone wall terrain for a ruined keep, 32x32 high top-down pixel art viewed from ~35 degrees (Hades reference). Raised rough wall stones and collapsed block mass, muted #4A3F3F value, darker crevices, restrained cold blue rim shadows #7BA7BC, ancient fortress masonry, heavy but readable silhouette. Must connect cleanly against the floor terrain. Mat painterly pixel, dark gritty palette, no gradients, heavy texture, asymmetric weathering.
```

---

## Transition (Eğer text input alanı varsa)

```
Broken rubble seam between floor and wall: loose dark stones, cracked mortar, small debris piles, chipped flagstone edges, a few pale lichen flecks, very subtle cyan-violet rift dust only in tiny accents. Keep the transition narrow and readable so room walkable space remains clear.
```

---

## Negative Prompt (varsa)

```
no bright cartoon colors, no green grass, no outdoor forest greenery, no blood splatter, no gore, no grimdark blood horror — rift theme is void/ritual catastrophe only.
no uniform repetition, no perfect grid alignment, no copy-paste micro-detail.
no outlined cartoon style, no cel-shading. Mat painterly pixel only.
no isometric or hex perspective — square top-down only.
no yellow/orange torch glow baked on tile (lighting in Unity, not generation).
no tile borders or frames — edge-to-edge seamless.
no smooth anti-aliased gradients — pixel-honest dithering only.
no 45 degree perspective or steeper angle — strict ~35 degree Hades reference.
no character/enemy/prop/decor — terrain ONLY.
```

---

## Pilot Test Disiplini

1. **Generate Pro** → 20 generations ücret
2. **QC 100% + 400% zoom:**
   - Floor-wall okunabilirliği var mı?
   - Tile seams doğal mı, grid-block mu?
   - Palet tutarlılığı (#2C2A2A / #4A3F3F / #7BA7BC)
   - View angle ~35° hissini veriyor mu (45° dik kafa yukarıdan değil)?
3. **PASS ise:**
   - Sheet download (16-tile Wang)
   - Aseprite cleanup (kenar artefakt, palet snap, tileable check) — 5-15dk/tile
   - Unity import: PPU=64, Point filter, no compression, no mipmap
   - Test scene'de RuleTile kur, 1 test oda boya
4. **FAIL ise:**
   - Transition 0.25 yerine 0.5 dene
   - Raggedness 45→55 yükselt (daha pürüzlü)
   - Spread 25→35 yükselt (daha yumuşak geçiş)

---

## PASS Sonrası — Floor Varyantları (Create tiles PRO ile)

Maps > Tileset Pro PASS olunca, kabul ettiğin floor tile'larını **style reference** olarak yükle (max 12, 128x128) → `Create tiles PRO` web UI:

**Description:**
```
1) cracked flagstone variant — same dark stone base, fresh chip patterns, scattered debris
2) lichen-covered flagstone — same base + cold grey-green moss in corners
3) rune dust flagstone — same base + half-buried silver rune fragments, faint cold blue glow residue
4) chipped rubble flagstone — same base + heavier weathering, more crack count
5) cold damp dark stone — same base + faint moisture sheen, mineral staining
6) plain dark stone — same base + reduced weathering, baseline tile
```

**Settings:**
- Tile type: **square_topdown**
- Tile size: **32px**
- View angle: **35°** (slider veya preset)
- Thickness: **0**
- Outline mode: **segmentation** (no outline)

Bu 6 varyant `FloorVariantPainter` tier dağıtımına girer:
- Base/plain ağırlıklı (40-50%)
- Cracked + chipped orta (15-20% each)
- Lichen + rune dust seyrek (5-10% each)
- Cold damp ara (10-15%)

---

## Sonraki Adımlar (Tileset PASS sonrası)

1. Floor varyant batch (yukarıdaki)
2. Wall variant set — ayrı `create_topdown_tileset` çağrısı (lower=wall base, upper=wall top/cap)
3. Decoration objects — `create_map_object` (rubble pile, broken pillar, brazier, chain segment)
4. Vista template prompts (Karar #85, 3 vista: kırık duvar / balkon / cliff edge)

---

## Açık Sorular (PixelLab'a ilk basışta gözlemle)

1. UI Pro mode'da `Generate Pro` butonu tıklayınca **gerçek angle slider değeri** ne? "high top-down" preset'i ~35° mi yoksa 60°+ mı? **400% zoom ile screenshot al, doğrula.**
2. Raggedness 45% ile kenar görsel olarak doğal mı? 30 ile karşılaştır (transition 0.25 sabit).
3. Sheet export sırasında 16 tile mı 23 tile mı geliyor (transition_size = 0.25 ise 16 normal)?
