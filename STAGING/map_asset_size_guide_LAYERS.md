---
status: LIVE
faz: post-Brush-V1
tarih: 2026-05-18
ozet: "Map designer layer-by-layer asset pixel size guide + production checklist"
karar: #74 (PPU=64), #100 (chibi 30-35°), #143-E (layered pipeline)
---

# RIMA Map Asset Sizes — Layer-by-Layer + PixelLab Üretim

## ⚙️ TEMEL BOYUT KURALI (Karar #74 LOCK)

| Birim | Pixel |
|---|---|
| **1 Unity Unit** | 64 pixel (PPU=64) |
| **Floor tile** | **64×64** (1 unit × 1 unit) |
| **Wall tile (Wang16)** | **64×96** (taban 64 + üst 32 wall cap) |
| **Player chibi** | 64×64 |
| **Prop (standart)** | 64×64 (transparent padding ile) |
| **Mob Tier 1** | 64×64 |
| **Mob Tier 2** | 96×96 |
| **Mob Tier 3 / Boss** | 128×128 |

---

## 📐 BRUSH V1 LAYER SİSTEMİ (Karar #143-E)

| Layer | İçerik | Boyut | Status |
|---|---|---|---|
| **L1 Base/Background** | Tonal RGB fill (renkli zemin) | yok (RGB değer) | ✅ Brush sistem LIVE |
| **L2 Atlas (Floor)** | Floor tile sprites | **64×64** | ◐ StoneDungeon 6 ✅ / MossyCrypt 3 partial |
| **L3 Wang16 (Walls + Edges)** | 16-tile corner set | **64×96** | ◐ StoneDungeon 16 ✅ / MossyCrypt 0 ❌ |
| **L4 Organic (Moss, Dirt patches)** | Patch decals | **64×64** | ✅ Brush def LIVE, sprite var |
| **L5 Detail (Cracks, Scatter)** | Small detail elements | **32×32 to 64×64** | ✅ Brush def LIVE |
| **L6 Accent (Decals, Scars)** | Large overlay accents | **128×128** | ✅ Brush def LIVE (rift scar, battle aftermath) |
| **Props** (Sprint 12 separate) | Static environmental objects | **64×64** | ⚠️ 1/12 (sadece barrel) |

---

## 📋 ÜRETİM ÖNCELİK LİSTESİ (User PixelLab production)

### 🔴 P0 — Sample room demo için MUTLAKA (bu gece / yarın)

| # | Asset | Boyut | Adet | PixelLab call | Status |
|---|---|---|---|---|---|
| 1 | **Props (Batch 1 P0)** | 64×64 | 4 prop (crate, urn, candle, debris) | 4 ayrı call, 16 variant each | ⏳ v5 guide ready |
| 2 | **MossyCrypt Wall Wang16** | 64×96 | **16 wall tile** | 16 ayrı call **VEYA 1 mega call** | ❌ EKSİK |
| 3 | **MossyCrypt Floor variants** | 64×64 | **3 daha (toplam 6)** | 3 call × 16 variant | ◐ partial |

### 🟡 P1 — Polish (yarın / sonra)

| # | Asset | Boyut | Adet | Status |
|---|---|---|---|---|
| 4 | **Props (Batch 2 P1)** | 64×64 | 4 (column intact/broken, brazier, banner) | ⏳ v5 guide ready |
| 5 | **L4 Organic decals biome variants** | 64×64 | 4-6 (moss, vines, water, weeds) | ⏳ |
| 6 | **L5 Detail variants** | 32×32 | 4-6 (cracks, pebbles, shards) | ⏳ |
| 7 | **L6 Accent variants** | 128×128 | 2-3 (blood splatter, scorch, glyph) | ⏳ |

### 🟢 P2 — Phase 1 closure

| # | Asset | Boyut | Adet | Status |
|---|---|---|---|---|
| 8 | **Props (Batch 3 P2)** | 64×64 | 4 (altar, treasure, chains, statue) | ⏳ v5 guide ready |
| 9 | **Wall torch variants** | 128×160 (mevcut format) veya 64×96 | 2-3 variant | ✅ 1 var |
| 10 | **Mob anchor Tier 1** | 64×64 | 4 (Sprint 14+) | ⏳ |
| 11 | **Mob anchor Tier 2** | 96×96 | 4 (Sprint 14+) | ⏳ |
| 12 | **Mob anchor Tier 3 / Boss** | 128×128 | 4 (Phase 1.5+) | ⏳ |

---

## 🎯 PIXELLAB OUTPUT BOYUT → VARIANT MAPPING (Custom Size Beta dahil)

PixelLab'ın **Custom Size (Beta)** özelliği LIVE — non-standard boyutlar **native** üretilebiliyor. Manuel crop yok = doğal görünüm korunur.

Her asset için doğru PixelLab output seçimi:

| Asset boyutu | PixelLab Output | Variant Count | Total Image | Avantaj |
|---|---|---|---|---|
| **64×64 props/floors/chibi/Mob T1** | **64×64 standard** | 16 variant | 256×256 (4×4 grid) | ★ En çok variant — pick & choose |
| **64×96 wall Wang16** | **64×96 CUSTOM (Beta)** | 4 variant | 256×256 (2×2 grid) ★ **NATIVE — crop yok** | Doğal edge tiling |
| **32×32 detail L5** | **32×32 standard** | 32 variant | 256×256 mixed grid | Çok variant, small detail |
| **96×96 Mob T2** | **96×96 CUSTOM (Beta)** | 4 variant | 384×384 ★ **NATIVE** | Native, no scaling artifacts |
| **128×128 accent L6 / Mob T3** | **128×128 standard** | 4 variant | 256×256 (2×2 grid) | Detail için yeterli |
| **64×128 tall props** (chains, banner) | **64×128 CUSTOM (Beta)** | 4 variant | 256×512 ★ **NATIVE** | Tall prop exact fit |

**KILIT NOT:** Custom size Beta sayesinde **manual crop pipeline'ı atlandı**. Native pixel size → Unity import direct = doğal look, no resampling artifacts.

---

## ⚠️ KRİTİK EKSİK — MossyCrypt Wang16 Walls

MossyCrypt biome için **16 wall tile yok**. Bunlar StoneDungeon Wang16 parity için gerekli.

**Strateji (Custom Size Beta ile):**
- StoneDungeon walls 64×96 boyutunda
- MossyCrypt walls aynı boyutta üretilmeli
- PixelLab Output: **64×96 CUSTOM (Beta)** → 4 variant native 64×96
- **NO MANUEL CROP** — direct Unity import
- 16 wall × 4 variant per call = 64 versiyon, en iyi 16 seç
- **VEYA daha akıllı:** 1 prompt'la 4 farklı wang variant üret (ama PixelLab variant grid kuralı her variant SAME prompt'tur, farklı wang config gerek farklı prompt) → 16 ayrı prompt + her biri 4 variant

**Önerilen:** 16 ayrı PixelLab call (her wang config için) → 64×96 custom output → 4 variant her call → 1 keep → 16 wall tile native size.

---

## 🧱 MOSSY CRYPT WANG16 PRODUCTION PROMPT (yarın için)

Wang16 corner set 16 ayrı tile gerek. Her biri farklı edge konfigürasyonu. PixelLab single-prompt'la 16 farklı edge variant'ı üretmek zor.

**Önerilen yaklaşım:**
- 1 PixelLab call → "moss-covered stone wall, all-around solid" (1 tile)
- 1 PixelLab call → "moss-covered stone wall, top edge only"
- ... 16 ayrı call
- VEYA: 1 mega call ile 16 farklı wang variant 256×256 contact sheet'inde

**Mossy Crypt Wall Tile Prompt Template (her variant için adapt):**

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective like Hades. The wall tile shows the top edge cap visible as a flat plane.

ABSOLUTE CANVAS: 64x96 pixel canvas (or 128x128 with 64x96 prop centered), transparent background. The wall tile body fills the canvas exactly with seamless edges for tile placement.

ABSOLUTE STYLE: Pixel art tile, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color. Hades + Salt-and-Sanctuary dark gritty crypt mood. Vivid Vulnerability palette.

A moss-covered ancient stone wall tile, dark gray weathered stone block visible from above and front, with abundant deep green moss tufts and patches growing across the surface and crevices, hairline cracks in the stone, faint dampness sheen. The wall has a clean top edge cap (the upper 32 pixels visible from top-down angle showing the wall's top surface) and clean bottom edge for seamless tiling. Palette: dark slate gray stone dominant, deep moss green secondary, faint cold blue rim highlight on wet edges.

Single seamless wall tile, no characters, no creatures, no weapons, NO TILE BORDERS, NO GRID LINES.

Negative Prompt :
characters, humans, creatures, weapons, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, 3d render, soft shading, blur, smooth gradient, anti-aliasing, painterly style, anime style, text, words, letters, captions, numbers, watermark, tile grid lines, tile borders, single solid color block
```

---

## 🌿 MOSSY CRYPT FLOOR VARIANT PROMPT (3 daha gerek)

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The floor tile viewed from above showing only the top surface.

ABSOLUTE CANVAS: 64x64 pixel canvas, transparent background OR opaque tile (no padding — fills entire canvas seamlessly for tile placement).

ABSOLUTE STYLE: Pixel art tile, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color. Hades dark gritty crypt mood. Vivid Vulnerability palette.

A moss-covered ancient stone floor tile, dark gray weathered stone slab visible from directly above with slight tilt, surface covered with patches of deep green moss in irregular organic shapes, small hairline cracks running through the stone, faint dampness sheen on the moss. The tile must seamlessly tile when repeated — no clear borders, edges should blend with adjacent tiles. Palette: dark slate gray stone dominant, deep moss green moss patches secondary, faint cold blue rim highlight on damp moss edges.

Seamless tiling stone floor with moss, no characters, no creatures, no weapons, NO TILE BORDERS, NO GRID LINES.

Negative Prompt :
characters, humans, creatures, weapons, side view, three-quarter view, isometric projection, flat front view, 3d render, soft shading, blur, smooth gradient, anti-aliasing, painterly style, anime style, text, words, captions, numbers, watermark, tile grid lines, tile borders, single solid color
```

PixelLab output: **64×64** → 16 variant → en iyi 3'ünü seç (mevcut 3 ile birlikte 6 floor variant olur).

---

## 🎯 KARARLAŞTIRMA — Bu Gece Yapılacaklar

| Sıra | İş | Süre | PixelLab Output |
|---|---|---|---|
| 1 | **Prop Batch 1 P0** (4 prop) | ~10 dk | 64×64, 4 ayrı call |
| 2 | **MossyCrypt floor variants** (3 daha) | ~10 dk | 64×64, 1 call → 16 variant → 3 seç |
| 3 | **MossyCrypt Wang16 walls** (16 tile) | ~60-90 dk | 128×128 her call, 4 variant → 16 wall toplam |

**Toplam üretim:** ~80-110 dk + variant selection süresi.

**Önerim:** Bu gece sadece **Prop Batch 1 + MossyCrypt Floor** odakta — wall tiles yarın. Sample room için Stone Dungeon biome zaten complete, mossy biome eksik kalsa da Stone Dungeon kullanarak güzel oda kompoze edilir.

---

## 📐 BOYUT SUMMARY TABLE (kısa hatırlatma — Custom Size Beta dahil)

| Asset Type | Pixel Size | PixelLab Output Setting | Variant | Notes |
|---|---|---|---|---|
| Floor tile | 64×64 | **64×64 standard** | 16 | Seamless tiling |
| Wall Wang16 | 64×96 | **64×96 CUSTOM (Beta)** | 4 | Native — no crop |
| Prop standard | 64×64 | **64×64 standard** | 16 | Best variant pool |
| Prop tall | 64×128 | **64×128 CUSTOM (Beta)** | 4 | Native vertical |
| Detail L5 | 32×32 | **32×32 standard** | 32 | Çok variant |
| Accent L6 | 128×128 | **128×128 standard** | 4 | |
| Character chibi | 64×64 | **64×64 standard** | 16 | |
| Mob Tier 1 | 64×64 | **64×64 standard** | 16 | |
| Mob Tier 2 | 96×96 | **96×96 CUSTOM (Beta)** | 4 | **NATIVE — no downsample** |
| Mob Tier 3 / Boss | 128×128 | **128×128 standard** | 4 | |
| Wall torch | 64×96 veya 64×128 | **CUSTOM** | 4 | Native vertical |

---

## ⚙️ NEXT STEPS

1. **Bu gece:** Prop Batch 1 P0 (4 prop, 64×64, ~10 dk)
2. **Yarın sabah:** MossyCrypt floor (3 variant, 64×64)
3. **Yarın öğleden sonra:** MossyCrypt Wang16 walls (16 tile, 128×128 → crop 64×96) — uzun iş, parça parça yapılabilir
4. **Yarın akşam:** Unity import + PropDefinitionSO + Brush V1 yerleştir + sample room screenshot
5. **Sonra:** Prop Batch 2 + 3 + L4/L5/L6 polish decal'ler + lighting elements

**Bu liste ile her layer için ne lazım net.** Sırayla üret, demo room kademeli gelişsin.
