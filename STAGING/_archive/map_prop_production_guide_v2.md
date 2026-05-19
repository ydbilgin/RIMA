---
status: LIVE_v2
faz: post-Brush-V1
tarih: 2026-05-18
ozet: "Map prop production v2 — 12 prop, 64×64 unified canvas, batch'lar halinde copy-paste ready prompts"
karar: #74 (64×64), #100 (chibi 30-35°)
v1_archive: STAGING/_archive/map_prop_production_guide_v1.md
---

# Map Prop Production Guide v2 — Batch Copy-Paste Ready

## ⚙️ PIXELLAB SETTINGS (her prop için aynı)

| Setting | Değer |
|---|---|
| **Mode** | PixelLab → **Create Image Pro V3** |
| **Output canvas** | **64×64** (UNIFIED — tüm props aynı boyut) |
| **Camera (UI dropdown)** | **High top-down** |
| **Style ref panel** | **BOŞ** |
| **Describe reference kutusu** | **BOŞ** |
| **Init image** | YOK (sıfırdan üretim) |

**64×64 canvas notu:** Tüm prop'lar 64×64 canvas içinde üretilir. Küçük prop'lar (candle, chain) transparent padding'le kalır. Büyük prop'lar (kneeling statue) compact tasarlanır. Prompt zorunlu olarak "fit within 64x64 canvas" der.

---

## 🎯 ORTAK NEGATIVE PROMPT (her prop için aynı — bu kutuya yapıştır)

```
characters, humans, creatures, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple props in one image, prop set, prop grid, prop catalogue
```

---

# 🔴 BATCH 1 — P0 Critical Props (4 prop, BU GECE)

> **Bu 4 prop sample room kompozisyonu için MUTLAKA gerekli. Her birini ayrı ayrı üret, ~15 dk her biri.**

---

## Prop 1.1 — Wooden Crate (small)

**MAIN PROMPT (komple kopyala):**

```
ABSOLUTE CANVAS RULE: 64x64 pixel canvas, transparent background. The prop must fit completely within the 64x64 canvas with at least 4 pixels of transparent padding on all sides.

ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective like Hades and Hyper Light Drifter. The prop is viewed from above at a diagonal — the top plane of the crate is visible as a flat plane, the front face is visibly angled toward the camera. NOT flat front view, NOT side profile, NOT isometric, NOT pure 90-degree top-down.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color. Tone family: Hades + Hyper Light Drifter + Salt-and-Sanctuary dark gritty mood. Soft oval ground shadow beneath the prop.

A small wooden crate, weathered and chipped, dark brown stained planks with iron banding strips at the top and bottom of the crate body, metal nail rivets visible at the corners. The crate has subtle wear marks and a few pixel-sized scuff marks on the side panels. Approximate prop body size 36x36 pixels centered within the 64x64 canvas, leaving transparent padding around the prop. Palette: dark brown wood dominant, faded rust iron bands, faint cold blue rim highlight on one edge.

Single isolated prop, no characters, no creatures, no weapons.
```

---

## Prop 1.2 — Stone Urn (broken)

**MAIN PROMPT (komple kopyala):**

```
ABSOLUTE CANVAS RULE: 64x64 pixel canvas, transparent background. The prop must fit completely within the 64x64 canvas with at least 4 pixels of transparent padding on top, bottom, and sides.

ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The prop is viewed from above at a diagonal — the top opening of the urn is visible as an oval rim, the side curvature is visibly angled diagonally toward the camera. NOT flat front view, NOT side profile, NOT isometric, NOT pure 90-degree top-down.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color. Hades + Salt-and-Sanctuary dark gritty mood. Soft oval ground shadow beneath the urn.

A broken ancient stone urn, ritual pottery, dark gray weathered stone body with hairline cracks running vertically, a section of the top rim chipped away revealing the hollow interior, pixel dust around the base. The urn has subtle sigil-like engraved lines around its midsection. Approximate prop body size 32x48 pixels centered within the 64x64 canvas with transparent padding. Palette: dark slate gray dominant, dirty cream lip highlight, faint dark red rune accent in the engraved lines.

Single isolated prop, no characters, no creatures, no weapons.
```

---

## Prop 1.3 — Candle + Iron Holder

**MAIN PROMPT (komple kopyala):**

```
ABSOLUTE CANVAS RULE: 64x64 pixel canvas, transparent background. The prop is small — it will occupy roughly the center vertical strip of the 64x64 canvas with significant transparent padding on left and right sides.

ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The candle is viewed from above at a diagonal — the dish of the iron holder is visible as a shallow oval, the candle body is vertical. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades dark gritty mood. Soft oval ground shadow beneath the iron holder base.

A single tall thin wax candle in a small iron candleholder base, soft warm orange flame at the wick with a faint warm yellow glow halo, dripped wax frozen in pixel droplets running down the candle side. The iron holder base is dark cast iron with a hammered texture. Approximate prop body size 20x40 pixels centered within the 64x64 canvas with transparent padding on all sides. Palette: warm cream wax dominant, dark iron base, vibrant orange flame accent with warm yellow glow halo.

Single isolated prop, no characters, no creatures, no weapons.
```

---

## Prop 1.4 — Debris Pile (Rubble + Bones)

**MAIN PROMPT (komple kopyala):**

```
ABSOLUTE CANVAS RULE: 64x64 pixel canvas, transparent background. The prop must fit completely within the 64x64 canvas with at least 4 pixels of transparent padding on all sides.

ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The debris pile is viewed from above at a diagonal — top surfaces of the rubble pieces are visible, edges are angled toward the camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color. Hades + Salt-and-Sanctuary dark gritty aftermath mood. Soft oval ground shadow blending under the pile.

A small debris pile of mixed weathered stone rubble chunks and a few skeletal bone fragments scattered together, suggesting battle aftermath or ancient ruin. Stone pieces are dark gray with chipped edges, bones are dirty cream with faint dust marks. The pile has an irregular outline with a few pixel pebbles scattered around the base. Approximate prop body size 40x32 pixels centered within the 64x64 canvas with transparent padding. Palette: dark slate gray stone dominant, dirty cream bones, faint cold blue dust rim highlight.

Single isolated prop, no characters, no creatures, no weapons.
```

---

# 🟡 BATCH 2 — P1 Props (4 prop, YARIN)

> **Daha iyi görünüm için. Sample room'a varyasyon getirir.**

---

## Prop 2.1 — Stone Column (intact)

**MAIN PROMPT:**

```
ABSOLUTE CANVAS RULE: 64x64 pixel canvas, transparent background. The prop is tall — it occupies the full vertical height with transparent padding only on left and right sides.

ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The column is viewed from above at a diagonal — the top capital plate is visible as a flat plane, the shaft tapers slightly toward the camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades dark gritty temple mood. Soft oval ground shadow at the base.

A tall thin intact stone temple column, ancient architecture, dark gray weathered stone shaft with subtle vertical channels and a simple flat capital plate at the top, a wider stone base block at the foot. Subtle moss tufts on the lower third of the shaft suggesting age. Approximate prop body size 28x60 pixels centered horizontally within the 64x64 canvas, filling full vertical height with 2 pixels padding top and bottom. Palette: dark slate gray stone dominant, faint moss green tufts at the base, faint cold blue rim highlight on one edge.

Single isolated prop, no characters, no creatures, no weapons.
```

---

## Prop 2.2 — Stone Column (broken/cracked)

**MAIN PROMPT:**

```
ABSOLUTE CANVAS RULE: 64x64 pixel canvas, transparent background. The prop is broken and partially fallen — uses full vertical height with transparent padding only on left and right sides.

ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The broken column is viewed from above at a diagonal — top fractured edge visible, side of the shaft angled toward the camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades dark gritty ruined temple mood. Soft oval ground shadow blending around the base.

A broken stone column, ancient temple architecture in ruin, dark gray weathered stone shaft with a large diagonal break midway down exposing rough fractured interior in lighter weathered gray, the upper portion visibly tilted/leaning, pixel debris chunks scattered around the base. Subtle moss tufts on the lower third. Approximate prop body size 32x60 pixels filling vertical canvas with transparent padding on sides. Palette: dark slate gray stone dominant, faint moss green at the base, fractured rough interior in lighter weathered gray tones, faint cold blue rim highlight.

Single isolated prop, no characters, no creatures, no weapons.
```

---

## Prop 2.3 — Brazier (lit, ember glow)

**MAIN PROMPT:**

```
ABSOLUTE CANVAS RULE: 64x64 pixel canvas, transparent background. The prop must fit completely within the 64x64 canvas with at least 4 pixels of padding on all sides.

ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The brazier is viewed from above at a diagonal — the burning dish interior is visible as a shallow bowl, the three legs of the base are angled toward the camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades dark gritty ritual fire mood. Soft warm glow halo around the dish.

A standing iron brazier with a flickering ember fire inside, dark cast iron three-legged base supporting a shallow wide dish, bright warm orange and red ember coals at the center with a small flickering flame and faint warm glow halo radiating outward. The iron base has a hammered weathered texture with subtle rust spots. Approximate prop body size 40x48 pixels centered within the 64x64 canvas with transparent padding. Palette: dark iron base dominant, vibrant warm orange ember accent with red center, warm yellow flame edge, faint warm glow halo around the dish.

Single isolated prop, no characters, no creatures, no weapons.
```

---

## Prop 2.4 — Hanging Banner (torn)

**MAIN PROMPT:**

```
ABSOLUTE CANVAS RULE: 64x64 pixel canvas, transparent background. The prop is tall and narrow — occupies full vertical height with transparent padding on left and right sides.

ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The banner is viewed from above at a diagonal — the iron rod at the top is visible as a thin horizontal element, the hanging cloth drapes vertically with subtle perspective angling. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades + Salt-and-Sanctuary dark gritty old battlefield mood. Soft oval ground shadow below the bottom of the cloth.

A long torn cloth banner hanging vertically, dark crimson red fabric with frayed bottom edges and a horizontal tear midway suggesting age and battle, an old faded sigil or rune design barely visible on the upper half of the cloth, attached at the top to a thin dark iron rod. The fabric has subtle weathered texture and a few pixel-tear holes. Approximate prop body size 28x60 pixels filling vertical canvas with transparent padding on sides. Palette: dark crimson red dominant, faded dirty cream sigil accent at the top, dark iron rod, faint cold blue rim highlight.

Single isolated prop, no characters, no creatures, no weapons.
```

---

# 🟢 BATCH 3 — P2 Polish Props (4 prop, 3-7 GÜN İÇİNDE)

> **Phase 1 prop roster'ı tamamlar. Treasure/Shrine room'ları zenginleştirir.**

---

## Prop 3.1 — Stone Altar (small)

**MAIN PROMPT:**

```
ABSOLUTE CANVAS RULE: 64x64 pixel canvas, transparent background. The prop fits within the 64x64 canvas with at least 4 pixels padding on all sides.

ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The altar is viewed from above at a diagonal — the flat ritual top surface is clearly visible, the front and side panels are angled toward the camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades + Salt-and-Sanctuary dark gritty ritual mood. Faint dark red rune glow accent. Soft oval ground shadow.

A small ritual stone altar, square dark gray weathered stone block with a flat ritual top surface, ancient rune engravings carved deeply into the front and visible side panels glowing very faintly with dark red light. The altar top has subtle dripping stain marks suggesting old rituals. Approximate prop body size 48x44 pixels centered within the 64x64 canvas with transparent padding. Palette: dark slate gray stone dominant, faint dark red rune accent glow in the engravings, faint cold blue rim highlight.

Single isolated prop, no characters, no creatures, no weapons.
```

---

## Prop 3.2 — Treasure Pile (gold + gems)

**MAIN PROMPT:**

```
ABSOLUTE CANVAS RULE: 64x64 pixel canvas, transparent background. The prop fits within the 64x64 canvas with at least 4 pixels padding on all sides.

ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The treasure pile is viewed from above at a diagonal — the top of the gold stacks is visible as ovals, gemstones glint upward, coin edges are angled toward the camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades dark dungeon treasure mood. Soft warm rim glow around the gold. Soft oval ground shadow.

A small treasure pile of scattered gold coins and a few colorful gemstones, gold coins stacked irregularly in pixel-perfect stacks with some loose coins scattered, a few cyan and emerald green gemstones glinting among the gold. The pile is sitting on dark stone floor with faint cold blue rim glow on the gems. Approximate prop body size 48x36 pixels centered within the 64x64 canvas with transparent padding. Palette: warm gold dominant, vibrant cyan and emerald green gem accents, dark stone base, faint warm yellow shine on coin highlights.

Single isolated prop, no characters, no creatures, no weapons.
```

---

## Prop 3.3 — Hanging Chains (coiled)

**MAIN PROMPT:**

```
ABSOLUTE CANVAS RULE: 64x64 pixel canvas, transparent background. The prop is tall and narrow — occupies full vertical height with transparent padding on left and right sides.

ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The chain is viewed from above at a diagonal — top iron bracket is visible at the top edge, individual chain links twist downward with subtle perspective. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades + Salt-and-Sanctuary dark gritty dungeon mood. Soft oval ground shadow below the bottom link.

A vertical hanging chain of dark iron, weathered links going from top to bottom with a few twist coils midway suggesting kinetic motion, the bottom link ending in a frozen mid-sway position just above the ground. The chain has subtle rust and dirt staining on the links. Attached at the top by a small iron bracket protruding from a stone ceiling fragment. Approximate prop body size 16x60 pixels centered horizontally within the 64x64 canvas, filling vertical height with transparent padding on sides. Palette: dark iron gray dominant, faint rust accent on a few links, faint cold blue rim highlight on one edge.

Single isolated prop, no characters, no creatures, no weapons.
```

---

## Prop 3.4 — Kneeling Statue (broken, compact)

**MAIN PROMPT:**

```
ABSOLUTE CANVAS RULE: 64x64 pixel canvas, transparent background. The statue is designed COMPACT to fit within 64x64 with at least 2 pixels padding all sides — kneeling pose with head broken off compresses height.

ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The kneeling statue is viewed from above at a diagonal — the broken neck stump top surface is visible as a fractured plane, the kneeling body angles toward the camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades + Salt-and-Sanctuary dark gritty ruined temple guardian mood. Soft oval ground shadow at the base.

A broken stone statue of a kneeling humanoid figure in a compact pose, ancient temple guardian, dark gray weathered stone with subtle moss tufts on the lower portion, the head is completely broken off and missing with rough fractured neck stump showing lighter weathered stone interior, the hands clasped in a prayer pose at chest level. The body has subtle armor or robe carved details. Approximate prop body size 44x58 pixels centered within the 64x64 canvas with minimal transparent padding (2 pixels each side). Palette: dark slate gray stone dominant, faint moss green tufts at the base, fractured rough interior on the broken neck stump in lighter weathered gray, faint cold blue rim highlight.

Single isolated prop, no characters, no creatures, no weapons.
```

---

# 📋 ÜRETİM CHECKLIST

## Sıra (bu gece ve yarın)

| Sıra | Prop | Süre | Notlar |
|---|---|---|---|
| 1 | **1.1 Wooden Crate** | ~15 dk | İlk üretim — settings doğrula |
| 2 | **1.2 Stone Urn** | ~15 dk | Padding doğru çıkıyor mu? |
| 3 | **1.3 Candle** | ~15 dk | Küçük prop — büyük transparent padding |
| 4 | **1.4 Debris Pile** | ~15 dk | Karışık element (taş+kemik) |
| 5 | **2.1 Stone Column intact** | ~15 dk | Tall prop — full vertical |
| 6 | **2.2 Stone Column broken** | ~15 dk | Brokenness signal doğru mu? |
| 7 | **2.3 Brazier** | ~15 dk | Ember glow — warm halo |
| 8 | **2.4 Hanging Banner** | ~15 dk | Tall narrow prop |
| 9 | **3.1 Stone Altar** | ~15 dk | Dark red rune glow accent |
| 10 | **3.2 Treasure Pile** | ~15 dk | Gold + gem multi-color |
| 11 | **3.3 Hanging Chains** | ~15 dk | Narrow vertical |
| 12 | **3.4 Kneeling Statue** | ~15 dk | Compact pose — tam 64×64 |

**Toplam üretim süresi:** 12 × 15dk = **~3 saat** (her batch için ayrı oturum yapılabilir)

## Üretim sonrası

1. **Unity import:** `Assets/Sprites/Props/{Category}/<prop_name>.png`
   - Sprite Mode: Single
   - **PPU: 64**
   - Filter Mode: Point (no filter)
   - Compression: None
   - **Pivot: Bottom Center** (prop'lar yere oturmalı)
2. **PropDefinitionSO oluştur** her PNG için:
   - Manuel: `Create → RIMA → Brush → PropDefinition` → fields doldur
   - Otomatik: Codex Editor script (yazılabilir — "Generate PropDefinitions from /Sprites/Props/")
3. **Brush V1 editor'da yerleştir:** `RIMA → MapDesigner → Brush → Open Editor` → Props Tab → place
4. **Sample room update:** `Library/Combat_Small_01.asset` → barrel placeholder'ları gerçek prop'larla değiştir

---

## 🔑 64×64 Canvas Stratejisi (önemli not)

PixelLab UI'da **output 64×64** seçildiğinde, prop o canvas içinde üretilir:
- **Square props (crate, debris, altar, treasure):** ~32-48 pixel body, padding all sides
- **Tall props (column, candle, banner, chains, statue):** full vertical height, padding left+right
- **Wide props (treasure):** full horizontal, padding top+bottom

Prompt **"Approximate prop body size XxY pixels centered within the 64x64 canvas with transparent padding"** ile PixelLab'a istediğimiz boyutu söylüyoruz. Bu yöntem v2 LIVE.

Çıktı 64×64 PNG transparent background → Unity import 64 PPU ile **1:1 tile size** = mükemmel uyum.

---

## 🎯 ÖZET — Bu Gece Yapılacaklar

1. PixelLab'a git → Create Image Pro V3
2. Output canvas: **64×64** dropdown
3. Camera: **High top-down** dropdown
4. Style ref panel: **boş**
5. **Batch 1'in 4 prop'unu sırayla üret** (~1 saat toplam)
6. Her birinin negative prompt'unu da kullan (yukarıda)
7. Üretilen 4 PNG → masaüstüne kaydet
8. Yarın Unity import + PropDefinitionSO + Brush yerleştir

**Yarın sample room screenshot'una hazırsın.**
