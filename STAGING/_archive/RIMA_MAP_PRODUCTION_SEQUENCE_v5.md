---
title: RIMA Map Production Sequence — Step-by-Step
status: LIVE
faz: post-Brush-V1
tarih: 2026-05-18
ozet: "Sıralı üretim guide — her prop için tek block prompt, copy-paste ready, sırayla git"
karar: #74 (64×64), #100 (chibi 30-35°), Custom Size Beta LIVE
---

# 🗺️ RIMA Map Production — Sırayla Git Guide

**Bu doküman:** User PixelLab Web UI'ı açar → STEP 1'den başlar → her step tek block prompt → kopyala-yapıştır-generate → sonraki step. Sırayla.

---

## ⚙️ ORTAK PIXELLAB AYARLARI (her step'te aynı, sadece SIZE değişir)

| Setting | Değer |
|---|---|
| **Mode** | PixelLab → **Create Image Pro V3** |
| **Style ref panel** | **BOŞ** |
| **Describe reference kutusu** | **BOŞ** |
| **Camera (UI dropdown)** | **High top-down** |
| **Output size** | Her step'te belirtilir (genellikle 64×64) |
| **Custom Size** | Her step'te belirtilir (Wang16 + tall props için) |

---

## 🎨 256/512 "DOĞAL GÖRÜNÜM" TEKNİĞİ — Ne zaman kullanmalı?

User'ın sorduğu — **YES, 256/512 üretim "doğal/oval görünüm" için işe yarar:**

| Üretim Tipi | Çıktı | İşlem | Sonuç |
|---|---|---|---|
| **Direct 64×64** | Native 64×64, 16 variant | Hiç | Sharp pixel, geometric |
| **256×256 → downsample 64** | High-res 256×256 (1 image) | Pixelorama/GIMP nearest-neighbor → 64×64 | **Doğal/organik şekiller** + sharp pixel |
| **512×512 → downsample 64** | Higher-res 512×512 (1 image) | Pixelorama nearest-neighbor → 64×64 | En doğal şekiller, **çok detail loss** ihtimali |

**Use case:**
- **Direct 64×64:** Tile-exact assets (walls, floor tiles, geometric props — crate, urn, candle)
- **256/512 downsample:** Organic props (debris, statue, treasure pile, fire, moss decals) — "doğal/oval" şekiller için

**Her step'te belirteceğim:** Direct 64×64 mı, 256→64 mı tercih edilmeli.

---

# 🔴 P0 BATCH — SAMPLE ROOM İÇİN MUTLAKA (4 prop)

---

## STEP 1 — Wooden Crate (Prop 1.1)

### PixelLab Settings
- **Output:** **64×64** (standard, 16 variant)
- Custom Size: NO

### Prompt (kopyala → main input):

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective like Hades and Hyper Light Drifter. The prop is viewed from above at a diagonal — the top plane is visible as a flat plane, the front face is visibly angled toward the camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: 64x64 pixel canvas, transparent background, the prop fits completely within with at least 4 pixels transparent padding on all sides.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color. Tone family: Hades + Hyper Light Drifter + Salt-and-Sanctuary dark gritty mood, Vivid Vulnerability palette. Soft oval ground shadow beneath the prop.

A small wooden crate, weathered and chipped, dark brown stained planks with iron banding strips at the top and bottom of the crate body, metal nail rivets visible at the corners. Subtle wear marks and a few pixel-sized scuff marks on the side panels. Approximate body size 36x36 pixels centered within the 64x64 canvas. Palette: dark brown wood dominant, faded rust iron bands, faint cold blue rim highlight.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple crates in one image
```

### Sonra:
- 16 variant'tan **en iyisini** seç → export 64×64 PNG
- Dosya adı: `wooden_crate_01.png`
- Save: `C:/Users/ydbil/Downloads/rima_props/wooden_crate_01.png` (geçici)

---

## STEP 2 — Stone Urn (Prop 1.2)

### PixelLab Settings
- **Output:** **64×64** (standard, 16 variant)
- Custom Size: NO

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective like Hades and Hyper Light Drifter. The prop is viewed from above at a diagonal — the top opening is visible as an oval rim, the side curvature angled toward camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: 64x64 pixel canvas, transparent background, the prop fits within with at least 4 pixels transparent padding.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades + Salt-and-Sanctuary dark gritty ritual mood. Soft oval ground shadow.

A broken ancient stone urn, ritual pottery, dark gray weathered stone body with hairline vertical cracks, a section of the top rim chipped away revealing the hollow interior, pixel dust around the base, subtle sigil-like engraved lines around the midsection. Approximate body size 32x48 pixels centered within the 64x64 canvas. Palette: dark slate gray dominant, dirty cream lip highlight, faint dark red rune accent in the engraved lines.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple urns, intact undamaged urn
```

### Sonra:
- En iyi variant → `stone_urn_broken_01.png`

---

## STEP 3 — Candle + Iron Holder (Prop 1.3)

### PixelLab Settings
- **Output:** **64×64** (standard, 16 variant)
- Custom Size: NO

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The candle is viewed from above at a diagonal — the dish of the iron holder is visible as a shallow oval, candle body vertical. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: 64x64 pixel canvas, transparent background, the prop is small and occupies the center vertical strip with significant transparent padding on left and right sides.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades dark gritty mood. Soft oval ground shadow beneath the iron holder base.

A single tall thin wax candle in a small iron candleholder base, soft warm orange flame at the wick with a faint warm yellow glow halo, dripped wax frozen in pixel droplets running down the candle side, the iron holder base dark cast iron with hammered texture. Approximate body size 20x40 pixels centered within the 64x64 canvas. Palette: warm cream wax dominant, dark iron base, vibrant orange flame with warm yellow glow halo.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple candles
```

### Sonra:
- En iyi variant → `candle_iron_01.png`

---

## STEP 4 — Debris Pile (Prop 1.4) 🌿 ORGANIC

> **Alternatif:** Bu prop "organic" — istersen **256×256 high-res → downsample 64** workflow ile daha doğal görünüm alabilirsin. Direct 64×64 de OK.

### PixelLab Settings (Direct 64×64 — basit)
- **Output:** **64×64** (standard, 16 variant)
- Custom Size: NO

### Alternative: PixelLab Settings (256×256 → downsample, doğal görünüm)
- **Output:** **256×256** (single image, 1 variant)
- Sonra Pixelorama/GIMP'te nearest-neighbor downsample 64×64

### Prompt (her iki opsiyon için aynı):

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The debris pile is viewed from above at a diagonal — top surfaces of rubble pieces visible, edges angled toward camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: target final size 64x64 pixel, transparent background, the prop fits within with at least 4 pixels padding.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color. Hades + Salt-and-Sanctuary dark gritty aftermath mood. Soft oval ground shadow blending under the pile.

A small debris pile of mixed weathered stone rubble chunks and a few skeletal bone fragments scattered together, suggesting battle aftermath or ancient ruin. Stone pieces dark gray with chipped edges, bones dirty cream with faint dust marks. Irregular outline with a few pixel pebbles scattered around the base. Palette: dark slate gray stone dominant, dirty cream bones, faint cold blue dust rim highlight.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple piles in one image
```

### Sonra:
- 64×64 direct: en iyi variant export
- 256→64: Pixelorama'da Image → Scale Image → 64×64 (Filter: Nearest)
- Dosya adı: `debris_pile_01.png`

---

# 🔵 BU GECE BURADA DUR

**4 P0 prop yeterli sample room demo için.** Yarın Unity import + Brush V1 yerleştir + screenshot.

---

# 🟡 P1 BATCH — POLISH (yarın)

Aşağıdaki 4 prop yarın PixelLab'a gidip aynı akışla.

---

## STEP 5 — Stone Column (intact)

### PixelLab Settings
- **Output:** **64×128 CUSTOM (Beta)** — native tall vertical, 4 variant
- Custom Size: **YES, 64×128**

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The column is viewed from above at a diagonal — top capital plate visible as a flat plane, shaft tapers slightly. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: 64x128 pixel canvas, transparent background, the prop is tall and occupies full vertical height with transparent padding only on left and right sides.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades dark gritty temple mood. Soft oval ground shadow at the base.

A tall thin intact stone temple column, ancient architecture, dark gray weathered stone shaft with subtle vertical channels and a simple flat capital plate at the top, wider stone base block at the foot. Subtle moss tufts on the lower third of the shaft suggesting age. Approximate body size 32x120 pixels centered horizontally within the 64x128 canvas. Palette: dark slate gray stone dominant, faint moss green tufts at the base, faint cold blue rim highlight.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple columns
```

### Sonra:
- 4 variant'tan en iyisi → `stone_column_intact_01.png`

---

## STEP 6 — Stone Column (broken/cracked)

### PixelLab Settings
- **Output:** **64×128 CUSTOM (Beta)**, 4 variant

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The broken column is viewed from above at a diagonal — top fractured edge visible, side of the shaft angled toward camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: 64x128 pixel canvas, transparent background, the broken column occupies vertical canvas with padding on sides.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades dark gritty ruined temple mood. Soft oval ground shadow blending around the base.

A broken stone column, ancient temple architecture in ruin, dark gray weathered stone shaft with a large diagonal break midway down exposing rough fractured interior in lighter weathered gray, the upper portion visibly tilted/leaning, pixel debris chunks scattered around the base. Subtle moss tufts on the lower third. Approximate body size 32x110 pixels filling vertical canvas. Palette: dark slate gray stone dominant, faint moss green at the base, fractured rough interior in lighter weathered gray tones, faint cold blue rim highlight.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, intact complete column, multiple columns
```

### Sonra:
- En iyi variant → `stone_column_broken_01.png`

---

## STEP 7 — Brazier (lit, ember glow) 🌿 ORGANIC

> **Önerilen:** 256×256 → downsample 64 (doğal ember glow için)

### PixelLab Settings (Önerilen — doğal görünüm)
- **Output:** **256×256** (1 single image)

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The brazier is viewed from above at a diagonal — burning dish interior visible as a shallow bowl, three legs of the base angled toward camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: target final size 64x64 pixel after downsample, transparent background.

ABSOLUTE STYLE: Pixel art prop (will be downsampled to 64x64 with nearest-neighbor), 1px solid black outline at final resolution, hard pixel edges target, no gradients in final, flat cell shading max 2 tones per color. Hades dark gritty ritual fire mood. Soft warm glow halo around the dish.

A standing iron brazier with a flickering ember fire inside, dark cast iron three-legged base supporting a shallow wide dish, bright warm orange and red ember coals at the center with a small flickering flame and faint warm glow halo radiating outward. The iron base has a hammered weathered texture with subtle rust spots. Palette: dark iron base dominant, vibrant warm orange ember accent with red center and warm yellow flame edge, faint warm glow halo around the dish.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading at low resolution, blur, smooth gradient at final size, anti-aliasing at final size, soft edges at final size, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple braziers, fire covering whole canvas
```

### Sonra:
- 256×256 PNG indir
- Pixelorama aç → Image → Scale Image → 64×64, **Filter: Nearest**
- Save: `brazier_lit_01.png`

---

## STEP 8 — Hanging Banner (torn)

### PixelLab Settings
- **Output:** **64×128 CUSTOM (Beta)**, 4 variant

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The banner is viewed from above at a diagonal — iron rod at the top visible as a thin horizontal element, hanging cloth drapes vertically. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: 64x128 pixel canvas, transparent background, the prop occupies full vertical height with transparent padding on sides.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades + Salt-and-Sanctuary dark gritty old battlefield mood. Soft oval ground shadow below the bottom of the cloth.

A long torn cloth banner hanging vertically, dark crimson red fabric with frayed bottom edges and a horizontal tear midway suggesting age and battle, an old faded sigil or rune design barely visible on the upper half of the cloth, attached at the top to a thin dark iron rod. The fabric has subtle weathered texture and a few pixel-tear holes. Approximate body size 32x120 pixels filling vertical canvas. Palette: dark crimson red dominant, faded dirty cream sigil accent at the top, dark iron rod, faint cold blue rim highlight.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple banners, intact undamaged banner
```

### Sonra:
- En iyi variant → `banner_torn_01.png`

---

# 🟢 P2 BATCH — POLISH (3-7 gün)

---

## STEP 9 — Stone Altar 🌿 ORGANIC

> **Önerilen:** 256×256 → downsample 64 (doğal rune glow + stain için)

### PixelLab Settings (Önerilen)
- **Output:** **256×256** (1 image)

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The altar viewed from above at a diagonal — flat ritual top surface clearly visible. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: target final size 64x64 pixel after downsample, transparent background.

ABSOLUTE STYLE: Pixel art prop (downsampled to 64x64 with nearest-neighbor), 1px outline at final, hard pixel edges, no gradients at final, max 2 tones per color. Hades + Salt-and-Sanctuary dark gritty ritual mood. Faint dark red rune glow accent. Soft oval ground shadow.

A small ritual stone altar, square dark gray weathered stone block with a flat ritual top surface, ancient rune engravings carved deeply into the front and visible side panels glowing very faintly with dark red light. The altar top has subtle dripping stain marks suggesting old rituals. Palette: dark slate gray stone dominant, faint dark red rune accent glow in the engravings, faint cold blue rim highlight.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading at low resolution, blur, smooth gradient at final size, anti-aliasing at final size, painterly, anime, modern, text, words, letters, captions, numbers, watermark, multiple altars, intact pristine altar
```

### Sonra: 256→64 downsample, save `stone_altar_01.png`

---

## STEP 10 — Treasure Pile 🌿 ORGANIC

> **Önerilen:** 256×256 → downsample 64 (gem + coin doğal görünüm)

### PixelLab Settings (Önerilen)
- **Output:** **256×256** (1 image)

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The treasure pile viewed from above at a diagonal — tops of gold stacks visible as ovals, gemstones glint upward. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: target final size 64x64 pixel after downsample, transparent background.

ABSOLUTE STYLE: Pixel art prop (downsampled to 64x64 nearest-neighbor), 1px outline final, hard pixel edges, no gradients final, max 2 tones per color. Hades dark dungeon treasure mood. Soft warm rim glow around the gold. Soft oval ground shadow.

A small treasure pile of scattered gold coins and a few colorful gemstones, gold coins stacked irregularly in pixel-perfect stacks with some loose coins scattered, a few cyan and emerald green gemstones glinting among the gold. The pile is sitting on dark stone floor with faint cold blue rim glow on the gems. Palette: warm gold dominant, vibrant cyan and emerald green gem accents, dark stone base, faint warm yellow shine on coin highlights.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, 3d render, soft shading, blur, smooth gradient at final, anti-aliasing at final, painterly, anime, modern, text, words, captions, numbers, watermark, multiple piles, treasure chest container
```

### Sonra: downsample 256→64, save `treasure_pile_01.png`

---

## STEP 11 — Hanging Chains

### PixelLab Settings
- **Output:** **64×128 CUSTOM (Beta)**, 4 variant

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The chain viewed from above at a diagonal — top iron bracket visible at top, individual chain links twist downward. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: 64x128 pixel canvas, transparent background, the prop is narrow and tall, occupies vertical canvas with padding on sides.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades + Salt-and-Sanctuary dark gritty dungeon mood. Soft oval ground shadow below bottom link.

A vertical hanging chain of dark iron, weathered links going from top to bottom with a few twist coils midway suggesting kinetic motion, the bottom link ending in a frozen mid-sway position just above the ground. The chain has subtle rust and dirt staining on the links. Attached at the top by a small iron bracket protruding from a stone ceiling fragment. Approximate body size 16x120 pixels centered horizontally. Palette: dark iron gray dominant, faint rust accent on a few links, faint cold blue rim highlight.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly, anime, modern, text, words, captions, numbers, watermark, multiple chains, chain with attached prisoner, manacles
```

### Sonra: En iyi variant → `hanging_chains_01.png`

---

## STEP 12 — Kneeling Statue 🌿 ORGANIC

> **Önerilen:** 256×256 → downsample 64 (doğal statue + moss + fracture detail)

### PixelLab Settings (Önerilen)
- **Output:** **256×256** (1 image)

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The kneeling statue viewed from above at a diagonal — broken neck stump top surface visible as fractured plane, kneeling body angles toward camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE CANVAS: target final size 64x64 pixel after downsample, transparent background.

ABSOLUTE STYLE: Pixel art prop (downsampled 64x64 nearest-neighbor), 1px outline final, hard pixel edges, no gradients final, max 2 tones per color. Hades + Salt-and-Sanctuary dark gritty ruined temple guardian mood. Soft oval ground shadow at base.

A broken stone statue of a kneeling humanoid figure in a compact pose, ancient temple guardian, dark gray weathered stone with subtle moss tufts on the lower portion, the head is completely broken off and missing with rough fractured neck stump showing lighter weathered stone interior, the hands clasped in a prayer pose at chest level. The body has subtle armor or robe carved details. Palette: dark slate gray stone dominant, faint moss green tufts at the base, fractured rough interior on broken neck stump in lighter weathered gray, faint cold blue rim highlight.

Single isolated prop, no characters, no creatures, no weapons.

Negative Prompt :
characters, humans, creatures, animals, alive creature, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, 3d render, soft shading, blur, smooth gradient final, anti-aliasing final, painterly, anime, modern, text, words, captions, numbers, watermark, multiple statues, complete intact statue with head, standing statue, full body statue
```

### Sonra: downsample 256→64, save `kneeling_statue_01.png`

---

# 📋 ÜRETİM ÖZET TABLOSU

| Step | Prop | Output | Variant | Notes |
|---|---|---|---|---|
| 1 | Wooden Crate | 64×64 standard | 16 | Direct |
| 2 | Stone Urn | 64×64 standard | 16 | Direct |
| 3 | Candle | 64×64 standard | 16 | Direct |
| 4 | Debris Pile | 64×64 OR 256→64 | 16 OR 1 | Organic — opsiyonel high-res |
| 5 | Column intact | 64×128 CUSTOM | 4 | Native tall |
| 6 | Column broken | 64×128 CUSTOM | 4 | Native tall |
| 7 | Brazier | **256→64** ★ | 1 | Organic ember glow |
| 8 | Banner torn | 64×128 CUSTOM | 4 | Native tall |
| 9 | Stone Altar | **256→64** ★ | 1 | Organic rune glow |
| 10 | Treasure Pile | **256→64** ★ | 1 | Organic gem detail |
| 11 | Hanging Chains | 64×128 CUSTOM | 4 | Native tall |
| 12 | Kneeling Statue | **256→64** ★ | 1 | Organic + fracture |

★ = High-res downsample önerilen ("doğal/oval görünüm")

---

# 🎯 BU GECE — SADECE STEP 1-4 yeter

PixelLab'a git, **STEP 1** prompt'unu kopyala, başla. 4 P0 prop = ~40-60 dk. Sonra Unity import + Brush V1 yerleştir = sample room demo.

Yarın STEP 5-8 (P1), 3-7 gün STEP 9-12 (P2).

---

# 🔧 256→64 Downsample Workflow

PixelLab 256×256 PNG indir → Pixelorama veya GIMP aç → Image → Scale Image → **Width: 64, Height: 64, Filter: Nearest-neighbor** → Save as new 64×64 PNG.

(Veya Python script ile batch downsample yapılabilir — söyle, yazarım.)

---

# 📚 Related Docs

- Laureth Studio global pipeline: `F:/LaurethStudio/01_PIPELINE/pixellab_production_knowledge.md`
- RIMA layer-by-layer asset guide: `STAGING/map_asset_size_guide_LAYERS.md`
- Karar #145 v2 Character States: `memory/project_pixellab_character_states_workflow.md`
