---
status: LIVE_v4
faz: post-Brush-V1
tarih: 2026-05-18
ozet: "Map prop production v4 — 3 BATCH, her batch tek block (negative inline), Create Image Pro V3 format"
karar: #74 (64×64 final), #100 (chibi 30-35°)
archive: STAGING/_archive/map_prop_production_guide_v1.md, v2.md, v3.md
---

# Map Prop Production Guide v4 — Tek Block / Batch (Negative Inline)

## ⚙️ PIXELLAB SETTINGS (her batch için aynı)

| Setting | Değer |
|---|---|
| **Mode** | PixelLab → **Create Image Pro V3** |
| **Output canvas** | **128×128** (2×2 grid = 4 prop × 64×64 each) |
| **Camera (UI dropdown)** | **High top-down** |
| **Style ref panel** | **BOŞ** |
| **Negative prompt field** | YOK — inline yazılır (her batch block'unun sonunda "Negative Prompt :" satırı) |

**Çıktı:** 128×128 PNG, 4 prop 2×2 grid'de, her prop kendi 64×64 quadrant'ında.

---

# 🔴 BATCH 1 — P0 Critical Props (BU GECE)

> **Aşağıdaki TEK BLOCK'u kopyala → PixelLab Create Image Pro V3 prompt kutusuna yapıştır → Generate.**

```
ABSOLUTE LAYOUT RULE: 128x128 pixel canvas, transparent background, divided into a clean 2x2 grid of four 64x64 quadrants. Each quadrant contains exactly ONE distinct isolated prop, with clear empty pixel space separating quadrants. No props overlap or share quadrants.

ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective like Hades and Hyper Light Drifter. Each prop viewed from above at a diagonal — top plane visible, sides angled toward camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE STYLE: Pixel art props, 1px solid black outline per prop, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color. Tone family: Hades + Hyper Light Drifter + Salt-and-Sanctuary dark gritty mood, Vivid Vulnerability palette. Each prop has its own soft oval ground shadow at its base.

FOUR PROPS in the 2x2 grid, one per quadrant:

TOP-LEFT QUADRANT (64x64): A small wooden crate, weathered dark brown stained planks with iron banding strips at the top and bottom of the crate body, metal nail rivets at the corners, subtle wear marks and scuff marks on the side panels. Approximate body size 36x36 pixels centered in its quadrant. Palette: dark brown wood dominant, faded rust iron bands, faint cold blue rim highlight.

TOP-RIGHT QUADRANT (64x64): A broken ancient stone urn, ritual pottery, dark gray weathered stone body with hairline vertical cracks, a section of the top rim chipped away revealing the hollow interior, pixel dust around the base, subtle sigil-like engraved lines around the midsection. Approximate body size 32x48 pixels centered in its quadrant. Palette: dark slate gray dominant, dirty cream lip highlight, faint dark red rune accent in the engraved lines.

BOTTOM-LEFT QUADRANT (64x64): A single tall thin wax candle in a small iron candleholder base, soft warm orange flame at the wick with a faint warm yellow glow halo, dripped wax frozen in pixel droplets running down the candle side, the iron holder base dark cast iron with hammered texture. Approximate body size 20x40 pixels centered in its quadrant with lots of transparent padding. Palette: warm cream wax, dark iron base, vibrant orange flame with warm yellow glow halo.

BOTTOM-RIGHT QUADRANT (64x64): A small debris pile of mixed weathered stone rubble chunks and a few skeletal bone fragments scattered together suggesting battle aftermath, stone pieces dark gray with chipped edges, bones dirty cream with faint dust marks, irregular outline with a few pixel pebbles around the base. Approximate body size 40x32 pixels centered in its quadrant. Palette: dark slate gray stone dominant, dirty cream bones, faint cold blue dust rim highlight.

Four single isolated props in 2x2 grid layout, no characters, no creatures, no weapons. Each prop is centered in its 64x64 quadrant with at least 4 pixels of transparent padding around it.

Negative Prompt : 
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, props overlapping, props touching, single large prop filling whole canvas, prop catalogue, more than four props
```

---

# 🟡 BATCH 2 — P1 Polish Props (YARIN)

> **Aşağıdaki TEK BLOCK'u kopyala → PixelLab Create Image Pro V3 prompt kutusuna yapıştır → Generate.**

```
ABSOLUTE LAYOUT RULE: 128x128 pixel canvas, transparent background, divided into a clean 2x2 grid of four 64x64 quadrants. Each quadrant contains exactly ONE distinct isolated prop, with clear empty pixel space separating quadrants. No props overlap or share quadrants.

ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective like Hades and Hyper Light Drifter. Each prop viewed from above at a diagonal — top plane visible, sides angled toward camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE STYLE: Pixel art props, 1px solid black outline per prop, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color. Hades + Hyper Light Drifter + Salt-and-Sanctuary dark gritty ruined temple mood. Each prop has its own soft oval ground shadow.

FOUR PROPS in the 2x2 grid, one per quadrant:

TOP-LEFT QUADRANT (64x64): A tall thin INTACT stone temple column, dark gray weathered stone shaft with subtle vertical channels and a simple flat capital plate at the top, wider stone base block at the foot, subtle moss tufts on the lower third of the shaft. Approximate body size 28x60 pixels filling full vertical height of its quadrant with padding on sides. Palette: dark slate gray stone dominant, faint moss green tufts at the base, faint cold blue rim highlight.

TOP-RIGHT QUADRANT (64x64): A BROKEN stone column, dark gray weathered stone shaft with a large diagonal break midway exposing rough fractured interior in lighter weathered gray, upper portion visibly tilted/leaning, pixel debris chunks scattered at the base, subtle moss tufts lower. Approximate body size 32x60 pixels filling vertical height with padding on sides. Palette: dark slate gray stone dominant, fractured rough interior in lighter weathered gray, faint moss green at base, faint cold blue rim highlight.

BOTTOM-LEFT QUADRANT (64x64): A standing iron brazier with a flickering ember fire inside, dark cast iron three-legged base supporting a shallow wide dish, bright warm orange and red ember coals at the center with small flickering flame and faint warm glow halo, hammered iron texture with rust spots. Approximate body size 40x48 pixels centered in its quadrant. Palette: dark iron base dominant, vibrant warm orange ember with red center, warm yellow flame edge, soft warm glow halo around dish.

BOTTOM-RIGHT QUADRANT (64x64): A long torn cloth banner hanging vertically, dark crimson red fabric with frayed bottom edges and horizontal tear midway, faded sigil design barely visible on upper half, attached at the top to a thin dark iron rod, subtle weathered texture and pixel-tear holes. Approximate body size 28x60 pixels filling vertical height with padding on sides. Palette: dark crimson red dominant, faded dirty cream sigil accent at top, dark iron rod, faint cold blue rim highlight.

Four single isolated props in 2x2 grid layout, no characters, no creatures, no weapons. Each prop centered in its 64x64 quadrant with at least 4 pixels transparent padding around it.

Negative Prompt : 
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, props overlapping, props touching, single large prop filling whole canvas, prop catalogue, more than four props, fire smoke effects covering other quadrants
```

---

# 🟢 BATCH 3 — P2 Polish Props (3-7 GÜN)

> **Aşağıdaki TEK BLOCK'u kopyala → PixelLab Create Image Pro V3 prompt kutusuna yapıştır → Generate.**

```
ABSOLUTE LAYOUT RULE: 128x128 pixel canvas, transparent background, divided into a clean 2x2 grid of four 64x64 quadrants. Each quadrant contains exactly ONE distinct isolated prop, with clear empty pixel space separating quadrants. No props overlap or share quadrants.

ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective like Hades and Hyper Light Drifter. Each prop viewed from above at a diagonal. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE STYLE: Pixel art props, 1px solid black outline per prop, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color. Hades + Salt-and-Sanctuary dark gritty ritual mood. Each prop has its own soft oval ground shadow.

FOUR PROPS in the 2x2 grid, one per quadrant:

TOP-LEFT QUADRANT (64x64): A small ritual stone altar, square dark gray weathered stone block with a flat ritual top surface, ancient rune engravings carved deeply into the front and visible side panels glowing very faintly with dark red light, subtle dripping stain marks on the top suggesting old rituals. Approximate body size 48x44 pixels centered in its quadrant. Palette: dark slate gray stone dominant, faint dark red rune accent glow in the engravings, faint cold blue rim highlight.

TOP-RIGHT QUADRANT (64x64): A small treasure pile of scattered gold coins and a few colorful gemstones, gold coins stacked irregularly with some loose coins scattered, cyan and emerald green gemstones glinting among the gold, sitting on dark stone floor base. Approximate body size 48x36 pixels centered in its quadrant. Palette: warm gold dominant, vibrant cyan and emerald green gem accents, dark stone base, faint warm yellow shine on coin highlights.

BOTTOM-LEFT QUADRANT (64x64): A vertical hanging chain of dark iron, weathered links going top to bottom with twist coils midway suggesting kinetic motion, bottom link in frozen mid-sway just above ground, subtle rust and dirt staining, attached at top by small iron bracket protruding from a stone ceiling fragment. Approximate body size 16x60 pixels centered horizontally in its quadrant, filling vertical height. Palette: dark iron gray dominant, faint rust accent on some links, faint cold blue rim highlight.

BOTTOM-RIGHT QUADRANT (64x64): A broken stone statue of a kneeling humanoid figure in compact pose, ancient temple guardian, dark gray weathered stone with subtle moss tufts on lower portion, the head completely broken off and missing with rough fractured neck stump showing lighter weathered stone interior, hands clasped in prayer pose at chest level, subtle armor or robe carved details. Approximate body size 44x58 pixels centered in its quadrant with minimal padding. Palette: dark slate gray stone dominant, faint moss green tufts at base, fractured rough interior on broken neck stump in lighter weathered gray, faint cold blue rim highlight.

Four single isolated props in 2x2 grid layout, no characters, no creatures, no weapons. Each prop centered in its 64x64 quadrant with at least 4 pixels transparent padding around it.

Negative Prompt : 
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, props overlapping, props touching, single large prop filling whole canvas, prop catalogue, more than four props, complete intact statue with head, full body statue
```

---

# 📋 ÜRETİM SIRASI

| Sıra | Batch | Süre | PixelLab call |
|---|---|---|---|
| 1 | **BATCH 1 P0** (Crate, Urn, Candle, Debris) | ~5-10 dk | Tek call → 128×128 sprite sheet |
| 2 | **BATCH 2 P1** (Column intact, Column broken, Brazier, Banner) | ~5-10 dk | Tek call → 128×128 sprite sheet |
| 3 | **BATCH 3 P2** (Altar, Treasure, Chains, Statue) | ~5-10 dk | Tek call → 128×128 sprite sheet |

**Toplam:** 3 PixelLab call = **15-30 dakika**.

---

# 🔪 POST-PROCESS — Sprite Sheet'i 4 PNG'ye Böl

Her batch sonrası 128×128 sprite sheet'i 4 ayrı 64×64 PNG'ye böl. **Python script aşağıda — istediğinde çalıştırırım.**

```python
from PIL import Image
import os

def split_batch(src_path, output_dir, prop_names):
    """128x128 sprite sheet → 4x 64x64 PNG."""
    os.makedirs(output_dir, exist_ok=True)
    img = Image.open(src_path)
    quadrants = [
        ((0, 0, 64, 64), prop_names[0]),       # TL
        ((64, 0, 128, 64), prop_names[1]),     # TR
        ((0, 64, 64, 128), prop_names[2]),     # BL
        ((64, 64, 128, 128), prop_names[3]),   # BR
    ]
    for (x1, y1, x2, y2), name in quadrants:
        crop = img.crop((x1, y1, x2, y2))
        crop.save(os.path.join(output_dir, f"{name}.png"), "PNG")

# Batch 1
split_batch("downloads/batch1_pixellab.png", "Assets/Sprites/Props/Batch1/",
    ["wooden_crate", "stone_urn_broken", "candle_iron", "debris_pile"])

# Batch 2
split_batch("downloads/batch2_pixellab.png", "Assets/Sprites/Props/Batch2/",
    ["stone_column_intact", "stone_column_broken", "brazier_lit", "banner_torn"])

# Batch 3
split_batch("downloads/batch3_pixellab.png", "Assets/Sprites/Props/Batch3/",
    ["stone_altar", "treasure_pile", "hanging_chains", "kneeling_statue"])
```

İndirme path'lerini söyle (`C:/Users/ydbil/Downloads/...` vb.), ben Python script'i çalıştırıp 4 PNG'ye bölerim.

---

# 🎯 Bu Gece Workflow

1. PixelLab → Create Image Pro V3
2. Output canvas: **128×128**, Camera: **High top-down**, Style ref: **boş**
3. **BATCH 1 TEK BLOCK'u** kopyala → prompt kutusuna yapıştır → Generate
4. 128×128 sprite sheet indir
5. (Yarın) Python script ile 4 ayrı 64×64 PNG'ye böl
6. Unity import + PropDefinitionSO + Brush V1

**Bu gece sadece BATCH 1 yeter.** Yarın 2 ve 3 hallolur.
