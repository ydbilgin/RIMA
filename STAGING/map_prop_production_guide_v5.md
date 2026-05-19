---
status: LIVE_v5
faz: post-Brush-V1
tarih: 2026-05-18
ozet: "Map prop production v5 — PixelLab variant grid mantığı doğrulu (64x64→16 variant, 128x128→4 variant). 1 prop = 1 call = N variant, en iyiyi seç."
karar: #74 (64×64 final), #100 (chibi 30-35°)
archive: STAGING/_archive/map_prop_production_guide_v1-v4.md
---

# Map Prop Production Guide v5 — Variant Grid Doğru Mantık

## ⚙️ PIXELLAB CREATE IMAGE PRO — VARIANT GRID MANTIĞI

**Önemli:** Create Image Pro V3'te seçtiğin output size'a göre **otomatik variant grid** çıkar:

| Output Size | Variant Count | Total Image | Pre-pick |
|---|---|---|---|
| **32×32** | 32 variant | 256×256 mixed grid | Çok variant, küçük |
| **64×64** | **16 variant** | **256×256 (4×4 grid)** | **★ Önerilen** |
| **128×128** | 4 variant | 256×256 (2×2 grid) | Detaylı ama az variant |
| 256×256+ | 1 variant | seçilen boyut | Tek shot, no pick |

**Tüm variant'lar AYNI PROMPT'un farklı yorumları.** Bu sayede 16 versiyondan en iyisini seçer + diğerlerini de kullanabilir (alt prop, color variant, vb.).

---

## 📐 STRATEJİ — Per-Prop Single Call

**1 prop = 1 PixelLab call = 16 variant** (64×64 output).

Sonra contact sheet'ten **en iyi 1-2 variant'ı** seç + PNG export.

12 prop × 1 call = **12 ayrı PixelLab call**, her biri ~2-3 dk = **~30-40 dk toplam**.

**Neden bu strateji?**
- Her prop'un identity'si korunur (mixed prompt'lar drift eder)
- 16 variant arasından **kalite seçimi** yapılır
- Bonus: 2-3 variant'ı "alt prop" olarak da kullanabilirsin (Karar #145 Use #2 — enemy variant matrix gibi, prop variant matrix)

---

# 🔴 BATCH 1 — P0 PROPS (4 ayrı call, BU GECE)

> **Her prop için: PixelLab → Create Image Pro V3 → Output: 64×64 → AŞAĞIDAKİ TEK BLOCK'u kopyala → prompt'a yapıştır → Generate → 16 variant çıkar → en iyiyi seç.**

---

## Prop 1.1 — Wooden Crate (small)

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective like Hades and Hyper Light Drifter. The prop is viewed from above at a diagonal — the top plane is visible as a flat plane, the front face is visibly angled toward the camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: 64x64 pixel canvas, transparent background, the prop fits completely within with at least 4 pixels transparent padding on all sides.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color. Tone family: Hades + Hyper Light Drifter + Salt-and-Sanctuary dark gritty mood, Vivid Vulnerability palette. Soft oval ground shadow beneath the prop.

A small wooden crate, weathered and chipped, dark brown stained planks with iron banding strips at the top and bottom of the crate body, metal nail rivets visible at the corners. Subtle wear marks and a few pixel-sized scuff marks on the side panels. Approximate body size 36x36 pixels centered within the 64x64 canvas. Palette: dark brown wood dominant, faded rust iron bands, faint cold blue rim highlight.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple props in one image
```

---

## Prop 1.2 — Stone Urn (broken)

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective like Hades and Hyper Light Drifter. The prop is viewed from above at a diagonal — the top opening is visible as an oval rim, the side curvature angled toward camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: 64x64 pixel canvas, transparent background, the prop fits within with at least 4 pixels transparent padding.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades + Salt-and-Sanctuary dark gritty ritual mood. Soft oval ground shadow.

A broken ancient stone urn, ritual pottery, dark gray weathered stone body with hairline vertical cracks, a section of the top rim chipped away revealing the hollow interior, pixel dust around the base, subtle sigil-like engraved lines around the midsection. Approximate body size 32x48 pixels centered within the 64x64 canvas. Palette: dark slate gray dominant, dirty cream lip highlight, faint dark red rune accent in the engraved lines.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple props in one image
```

---

## Prop 1.3 — Candle + Iron Holder

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The candle is viewed from above at a diagonal — the dish of the iron holder is visible as a shallow oval, candle body vertical. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: 64x64 pixel canvas, transparent background, the prop is small and occupies the center vertical strip with significant transparent padding on left and right sides.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades dark gritty mood. Soft oval ground shadow beneath the iron holder base.

A single tall thin wax candle in a small iron candleholder base, soft warm orange flame at the wick with a faint warm yellow glow halo, dripped wax frozen in pixel droplets running down the candle side, the iron holder base dark cast iron with hammered texture. Approximate body size 20x40 pixels centered within the 64x64 canvas. Palette: warm cream wax dominant, dark iron base, vibrant orange flame with warm yellow glow halo.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple candles in one image
```

---

## Prop 1.4 — Debris Pile (Rubble + Bones)

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The debris pile is viewed from above at a diagonal — top surfaces of rubble pieces visible, edges angled toward camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: 64x64 pixel canvas, transparent background, the prop fits within with at least 4 pixels padding.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color. Hades + Salt-and-Sanctuary dark gritty aftermath mood. Soft oval ground shadow blending under the pile.

A small debris pile of mixed weathered stone rubble chunks and a few skeletal bone fragments scattered together, suggesting battle aftermath or ancient ruin. Stone pieces dark gray with chipped edges, bones dirty cream with faint dust marks. Irregular outline with a few pixel pebbles scattered around the base. Approximate body size 40x32 pixels centered within the 64x64 canvas. Palette: dark slate gray stone dominant, dirty cream bones, faint cold blue dust rim highlight.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple piles in one image
```

---

# 🟡 BATCH 2 — P1 PROPS (4 ayrı call, YARIN)

---

## Prop 2.1 — Stone Column (intact)

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The column is viewed from above at a diagonal — top capital plate visible as a flat plane, shaft tapers slightly. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: 64x64 pixel canvas, transparent background, the prop is tall and occupies full vertical height with transparent padding on left and right sides.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades dark gritty temple mood. Soft oval ground shadow at the base.

A tall thin intact stone temple column, ancient architecture, dark gray weathered stone shaft with subtle vertical channels and a simple flat capital plate at the top, wider stone base block at the foot. Subtle moss tufts on the lower third of the shaft suggesting age. Approximate body size 28x60 pixels centered horizontally within the 64x64 canvas, filling full vertical height. Palette: dark slate gray stone dominant, faint moss green tufts at the base, faint cold blue rim highlight.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple columns in one image
```

---

## Prop 2.2 — Stone Column (broken/cracked)

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The broken column is viewed from above at a diagonal — top fractured edge visible, side of the shaft angled toward camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: 64x64 pixel canvas, transparent background, the broken column occupies full vertical height with transparent padding on sides.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades dark gritty ruined temple mood. Soft oval ground shadow blending around the base.

A broken stone column, ancient temple architecture in ruin, dark gray weathered stone shaft with a large diagonal break midway down exposing rough fractured interior in lighter weathered gray, the upper portion visibly tilted/leaning, pixel debris chunks scattered around the base. Subtle moss tufts on the lower third. Approximate body size 32x60 pixels filling vertical canvas. Palette: dark slate gray stone dominant, faint moss green at the base, fractured rough interior in lighter weathered gray tones, faint cold blue rim highlight.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, intact complete column, multiple columns
```

---

## Prop 2.3 — Brazier (lit, ember glow)

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The brazier is viewed from above at a diagonal — burning dish interior visible as a shallow bowl, three legs of the base angled toward camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: 64x64 pixel canvas, transparent background, the prop fits within with at least 4 pixels padding.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades dark gritty ritual fire mood. Soft warm glow halo around the dish.

A standing iron brazier with a flickering ember fire inside, dark cast iron three-legged base supporting a shallow wide dish, bright warm orange and red ember coals at the center with a small flickering flame and faint warm glow halo radiating outward. The iron base has a hammered weathered texture with subtle rust spots. Approximate body size 40x48 pixels centered within the 64x64 canvas. Palette: dark iron base dominant, vibrant warm orange ember accent with red center and warm yellow flame edge, faint warm glow halo around the dish.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple braziers, fire covering whole canvas
```

---

## Prop 2.4 — Hanging Banner (torn)

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The banner is viewed from above at a diagonal — iron rod at the top visible as a thin horizontal element, hanging cloth drapes vertically with subtle perspective angling. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: 64x64 pixel canvas, transparent background, the prop occupies full vertical height with transparent padding on sides.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades + Salt-and-Sanctuary dark gritty old battlefield mood. Soft oval ground shadow below the bottom of the cloth.

A long torn cloth banner hanging vertically, dark crimson red fabric with frayed bottom edges and a horizontal tear midway suggesting age and battle, an old faded sigil or rune design barely visible on the upper half of the cloth, attached at the top to a thin dark iron rod. The fabric has subtle weathered texture and a few pixel-tear holes. Approximate body size 28x60 pixels filling vertical canvas. Palette: dark crimson red dominant, faded dirty cream sigil accent at the top, dark iron rod, faint cold blue rim highlight.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple banners, full intact undamaged banner
```

---

# 🟢 BATCH 3 — P2 PROPS (4 ayrı call, 3-7 GÜN)

---

## Prop 3.1 — Stone Altar (small)

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The altar viewed from above at a diagonal — flat ritual top surface clearly visible, front and side panels angled toward camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: 64x64 pixel canvas, transparent background, the prop fits within with at least 4 pixels padding.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades + Salt-and-Sanctuary dark gritty ritual mood. Faint dark red rune glow accent. Soft oval ground shadow.

A small ritual stone altar, square dark gray weathered stone block with a flat ritual top surface, ancient rune engravings carved deeply into the front and visible side panels glowing very faintly with dark red light. The altar top has subtle dripping stain marks suggesting old rituals. Approximate body size 48x44 pixels centered within the 64x64 canvas. Palette: dark slate gray stone dominant, faint dark red rune accent glow in the engravings, faint cold blue rim highlight.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple altars, intact pristine altar
```

---

## Prop 3.2 — Treasure Pile (gold + gems)

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The treasure pile viewed from above at a diagonal — tops of gold stacks visible as ovals, gemstones glint upward, coin edges angled toward camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: 64x64 pixel canvas, transparent background, the prop fits within with at least 4 pixels padding.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades dark dungeon treasure mood. Soft warm rim glow around the gold. Soft oval ground shadow.

A small treasure pile of scattered gold coins and a few colorful gemstones, gold coins stacked irregularly in pixel-perfect stacks with some loose coins scattered, a few cyan and emerald green gemstones glinting among the gold. The pile is sitting on dark stone floor with faint cold blue rim glow on the gems. Approximate body size 48x36 pixels centered within the 64x64 canvas. Palette: warm gold dominant, vibrant cyan and emerald green gem accents, dark stone base, faint warm yellow shine on coin highlights.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple treasure piles, treasure chest container
```

---

## Prop 3.3 — Hanging Chains (coiled)

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The chain viewed from above at a diagonal — top iron bracket visible at the top edge, individual chain links twist downward with subtle perspective. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: 64x64 pixel canvas, transparent background, the prop is narrow and tall, occupying full vertical height with transparent padding on sides.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades + Salt-and-Sanctuary dark gritty dungeon mood. Soft oval ground shadow below the bottom link.

A vertical hanging chain of dark iron, weathered links going from top to bottom with a few twist coils midway suggesting kinetic motion, the bottom link ending in a frozen mid-sway position just above the ground. The chain has subtle rust and dirt staining on the links. Attached at the top by a small iron bracket protruding from a stone ceiling fragment. Approximate body size 16x60 pixels centered horizontally within the 64x64 canvas, filling vertical height. Palette: dark iron gray dominant, faint rust accent on a few links, faint cold blue rim highlight on one edge.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple chains, chain with attached prisoner, manacles
```

---

## Prop 3.4 — Kneeling Statue (broken, compact)

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The kneeling statue viewed from above at a diagonal — broken neck stump top surface visible as a fractured plane, kneeling body angles toward camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: 64x64 pixel canvas, transparent background, the statue is compact (kneeling pose, head broken off compresses height), fits within 64x64 with at least 2 pixels padding.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades + Salt-and-Sanctuary dark gritty ruined temple guardian mood. Soft oval ground shadow at the base.

A broken stone statue of a kneeling humanoid figure in a compact pose, ancient temple guardian, dark gray weathered stone with subtle moss tufts on the lower portion, the head is completely broken off and missing with rough fractured neck stump showing lighter weathered stone interior, the hands clasped in a prayer pose at chest level. The body has subtle armor or robe carved details. Approximate body size 44x58 pixels centered within the 64x64 canvas. Palette: dark slate gray stone dominant, faint moss green tufts at the base, fractured rough interior on the broken neck stump in lighter weathered gray, faint cold blue rim highlight.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, alive creature, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple statues, complete intact statue with head, standing statue, full body statue
```

---

# 📋 ÜRETİM SIRASI

| Sıra | Prop | Süre |
|---|---|---|
| 1-4 | **BATCH 1 P0** (Crate, Urn, Candle, Debris) | ~10 dk (4 call × 2-3 dk) |
| 5-8 | **BATCH 2 P1** (Column intact, Column broken, Brazier, Banner) | ~10 dk |
| 9-12 | **BATCH 3 P2** (Altar, Treasure, Chains, Statue) | ~10 dk |

**Toplam:** 12 call = **~30-40 dakika**.

Her call'da 16 variant çıkar → **en iyiyi seç → export 64×64 PNG**.

Bonus: 2-3 variant'ı **alt prop** olarak kaydet ("burned crate", "extinguished candle", vb. — Karar #145 Use #2 enemy variant mantığı gibi prop variant matrix).

---

# 🎯 Bu Gece Workflow

1. **PixelLab → Create Image Pro V3**
2. **Output: 64×64** (UI dropdown — 16 variant verir)
3. Camera: **High top-down**
4. Style ref: **boş**
5. **Prop 1.1 (Wooden Crate) prompt'unu** kopyala-yapıştır → Generate
6. 16 variant çıkar → en iyi 1-2'sini export 64×64 PNG
7. Prop 1.2, 1.3, 1.4 aynı akış
8. **Bu gece sadece BATCH 1 yeter** (4 prop) — sample room için
9. Unity import → PropDefinitionSO → Brush V1 yerleştir (yarın)
10. Yarın BATCH 2 + 3

PixelLab'a git, **Prop 1.1**'i kopyala, başla. 🎯
