---
title: RIMA Map Production Sequence v6 — Codex-Reviewed Clean Prompts
status: LIVE_v6
faz: post-Brush-V1
tarih: 2026-05-18
ozet: "Codex review sonrası temizlenmiş prompt'lar — canvas size kaldırıldı, body size kaldırıldı, rune/sigil → abstract marks, asymmetry edits applied"
revisions: "Codex laurethayday profile review applied per master revision list"
v5_archive: STAGING/_archive/RIMA_MAP_PRODUCTION_SEQUENCE_v5.md
---

# 🗺️ RIMA Map Production — Sırayla Git Guide v6

**Codex review sonrası temizlik uygulandı.** Gen israfı önlemek için:
- ✅ `ABSOLUTE CANVAS [size]` satırı kaldırıldı (UI dropdown halleder)
- ✅ Exact body pixel sizes kaldırıldı (PixelLab soft hint olarak almıyor)
- ✅ `rune/sigil` keyword'leri `abstract non-letter carved marks`'a değişti (text drift kapatır)
- ✅ Composition language eklendi: `1 prop only, full object visible, no duplicate copies`
- ✅ Negative global additions: `sprite sheet, grid, border, frame, cropped, duplicate, logo`
- ✅ Step-spesifik düzeltmeler (Statue humanoid çatışması, Chains tek-zincir, Brazier glow sınırlandı)

---

## ⚙️ ORTAK PIXELLAB AYARLARI

| Setting | Değer |
|---|---|
| **Mode** | PixelLab → **Create Image Pro V3** |
| **Style ref panel** | **BOŞ** |
| **Describe reference kutusu** | **BOŞ** |
| **Output size** | Her step'te belirtilir (UI dropdown) |
| **Custom Size** | Gerekirse Beta'dan seç (wall tile + tall prop) |

**Notlar:**
- Create Image Pro'da **camera UI dropdown YOK** → kamera açısı **prompt içinde** `ABSOLUTE CAMERA RULE` ile veriliyor (her step'in prompt'unda zaten ilk satırda var)
- PixelLab UI **otomatik** size + variant count'u söyler → prompt'a yazmak gereksiz
- Negative prompt için **ayrı field YOK** → main prompt sonunda inline

---

## 🎨 256/512 "Doğal Görünüm" Tekniği — Vazgeçmedik, Akıllı Kullanıyoruz

**Soru:** 256/512'den vazgeçtik mi? **HAYIR.** Stratejimiz daha akıllı oldu — gerçekten ihtiyaç olan prop'larda kullanıyoruz.

### Mantık: "Doğal görünüm" iki farklı yoldan geliyor

**Yol A — Variant Selection ile doğallık (16 variant pick):**
- Direct 64×64 output → PixelLab 16 farklı yorum üretir
- Her variant biraz farklı: chip pozisyonu, wear pattern, asymmetric shape
- **En organik/asymmetric variant'ı seçer**sin → doğal görünüm
- Simple geometric props (crate, urn, candle, columns, banner, chains, brazier) için **bu yeterli**

**Yol B — High-Res Downsample ile doğallık (256→64):**
- 256×256 high-res output → 1 image, daha çok pixel
- AI organik şekilleri (curves, irregular edges, fracture detail) yüksek-res'te **daha iyi** çiziyor
- Nearest-neighbor downsample → sharp pixel art with preserved organic shapes
- **Kayıp:** 16 variant pick → 1 variant only
- Organic complex props (treasure pile gem cluster, kneeling statue fracture+moss) için **bu kazançlı**

### Per-Prop Verdict

| Prop | Strategy | Sebep |
|---|---|---|
| Wooden Crate | Direct 64×64 | Geometric, variant pick > organic detail |
| Stone Urn | Direct 64×64 | Curved ama simple, variant pick yeterli |
| Candle | Direct 64×64 | Küçük, organic flame variant pick'ten geliyor |
| Debris Pile | Direct 64×64 | 16 variant rubble shape variety daha iyi |
| Column intact | 64×128 CUSTOM | Tall native, edge alignment kritik |
| Column broken | 64×128 CUSTOM | Tall native + fracture detail variant pick |
| Brazier | Direct 64×64 | Glow variant pick'ten geliyor (Codex revize) |
| Banner torn | 64×128 CUSTOM | Tall native, variant pick wrinkle/tear için |
| Stone Altar | Direct 64×64 | Geometric blok, variant pick yeterli |
| **Treasure Pile** | **256×256 → 64** ★ | **Organic gem cluster — high-res çok değerli** |
| Hanging Chains | 64×128 CUSTOM | Tall native, link spacing exact gerek |
| **Kneeling Statue** | **256×256 → 64** ★ | **Organic fracture + moss + pose — high-res mandatory** |

### Codex Logic (gen tasarrufu)

- **Bu gece (4 P0 prop):** Hepsi direct 64×64 → 16 variant her birinde → en iyileri pick → ~4 gen = 4 prop
- **256→64 (sadece STEP 10 ve 12):** Tek tek üretim, ama bu prop'lar zaten önemli detail content
- **Toplam gen budget:** ~14 gen total tüm 12 prop için (bunlar yarınki 5000 budget'tan rahat)

→ **Doğal görünüm hedefi her prop'a uygulanıyor**, sadece hangi tekniğin işe yaradığına göre.

---

# 🔴 P0 BATCH — BU GECE 4 PROP

---

## STEP 1 — Wooden Crate (Prop 1.1)

### PixelLab Settings
- **Output:** **64×64** (standard, 16 variant)

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective like Hades and Hyper Light Drifter. The prop is viewed from above at a diagonal — the top plane is visible as a flat plane, the front face is visibly angled toward the camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE COMPOSITION: transparent background, 1 prop only, full object visible, centered, clean transparent padding around the silhouette, no border or frame.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color. Tone family: Hades + Hyper Light Drifter + Salt-and-Sanctuary dark gritty mood, Vivid Vulnerability palette. Soft oval ground shadow beneath the prop.

A small wooden crate, weathered and chipped, dark brown stained planks with iron banding strips at the top and bottom of the crate body, metal nail rivets visible at the corners. Subtle wear marks and a few small scuff marks on the side panels. Palette: dark brown wood dominant, faded rust iron bands, faint cold blue rim highlight.

1 prop only, single isolated full object, centered on transparent background, no characters, no creatures, no weapons, no duplicate copies.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, sprite sheet, grid, border, frame, cropped object, duplicate object, logo, multiple crates
```

**Output:** `wooden_crate_01.png` (16 variant'tan en iyi)

---

## STEP 2 — Stone Urn (Prop 1.2)

### PixelLab Settings
- **Output:** **64×64** (standard, 16 variant)

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective like Hades. The prop is viewed from above at a diagonal — the top opening is visible as an oval rim, the side curvature angled toward camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE COMPOSITION: transparent background, 1 prop only, full object visible, centered, clean transparent padding around the silhouette, no border or frame.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades + Salt-and-Sanctuary dark gritty ritual mood. Soft oval ground shadow.

A broken ancient stone urn, ritual pottery, dark gray weathered stone body with hairline vertical cracks, a section of the top rim chipped away revealing the hollow interior, small dust around the base, subtle abstract non-letter carved grooves around the midsection. Palette: dark slate gray dominant, dirty cream lip highlight, faint dark red accent in the carved grooves.

1 prop only, single isolated full object, centered on transparent background, no characters, no creatures, no weapons, no duplicate copies.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, sprite sheet, grid, border, frame, cropped object, duplicate object, logo, multiple urns, intact undamaged urn, readable symbols, alphabetic symbols, actual letters, runes
```

**Output:** `stone_urn_broken_01.png`

---

## STEP 3 — Candle + Iron Holder (Prop 1.3)

### PixelLab Settings
- **Output:** **64×64** (standard, 16 variant)

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The candle is viewed from above at a diagonal — the dish of the iron holder is visible as a shallow oval, candle body vertical. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE COMPOSITION: transparent background, 1 prop only, full object visible, centered, clean transparent padding around the silhouette, no border or frame.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades dark gritty mood. Soft oval ground shadow beneath the iron holder base.

A single tall thin wax candle in a small iron candleholder base, soft warm orange flame at the wick with a small contained warm yellow glow tight to the wick, solid cooled wax drips in small pixel drops running down the candle side, the iron holder base dark cast iron with hammered texture. Palette: warm cream wax dominant, dark iron base, vibrant orange flame with small warm yellow glow.

1 prop only, single isolated full object, centered on transparent background, no characters, no creatures, no weapons, no duplicate copies.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, sprite sheet, grid, border, frame, cropped object, duplicate object, logo, multiple candles, ice, frozen, large aura, glow filling whole canvas
```

**Output:** `candle_iron_01.png`

---

## STEP 4 — Debris Pile (Prop 1.4)

### PixelLab Settings
- **Output:** **64×64** (standard, 16 variant) ★ Codex önerisi: bu gece direct 64×64

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The debris pile is viewed from above at a diagonal — top surfaces of rubble pieces visible, edges angled toward camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE COMPOSITION: transparent background, centered pile, full silhouette visible, clean transparent padding around the silhouette, no border or frame.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color. Hades + Salt-and-Sanctuary dark gritty aftermath mood. Soft oval ground shadow blending under the pile.

A small debris pile of mixed weathered stone rubble chunks and a few skeletal bone fragments scattered together, suggesting battle aftermath or ancient ruin. Stone pieces dark gray with chipped edges, bones dirty cream with faint dust marks. Irregular asymmetric outline with a few small pebbles scattered around the base. Palette: dark slate gray stone dominant, dirty cream bones, faint cold blue dust rim highlight.

1 prop only, single isolated full debris pile, centered on transparent background, no characters, no creatures, no weapons, no duplicate copies.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, sprite sheet, grid, border, frame, cropped object, duplicate object, logo, multiple piles, single solid block
```

**Output:** `debris_pile_01.png`

---

# 🔵 BU GECE BURADA DUR

**4 P0 prop yeterli.** Yarın Unity import + Brush V1 yerleştir + sample room screenshot.

---

# 🟡 P1 BATCH — POLISH (yarın)

---

## STEP 5 — Stone Column (intact)

### PixelLab Settings
- **Output:** **64×128 CUSTOM (Beta)** — native tall, 4 variant

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The column is viewed from above at a diagonal — top capital plate visible as a flat plane, shaft tapers slightly. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE COMPOSITION: transparent background, 1 prop only, tall narrow centered prop, full height visible, not cropped, clean padding on left and right sides, no border or frame.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades dark gritty temple mood. Soft oval ground shadow at the base.

A tall thin intact stone temple column, ancient architecture, dark gray weathered stone shaft with subtle vertical channels and a simple flat capital plate at the top, wider stone base block at the foot. Subtle moss tufts on the lower third of the shaft suggesting age. Palette: dark slate gray stone dominant, faint moss green tufts at the base, faint cold blue rim highlight.

1 prop only, single isolated full column, centered on transparent background, no characters, no creatures, no weapons, no duplicated columns.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, captions, numbers, watermark, sprite sheet, grid, border, frame, cropped object, duplicate object, multiple columns
```

**Output:** `stone_column_intact_01.png`

---

## STEP 6 — Stone Column (broken/cracked)

### PixelLab Settings
- **Output:** **64×128 CUSTOM (Beta)**, 4 variant

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The broken column is viewed from above at a diagonal — top fractured edge visible, side of the shaft angled toward camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE COMPOSITION: transparent background, 1 prop only, tall narrow centered prop, full height visible, not cropped, clean padding on sides, no border or frame.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades dark gritty ruined temple mood. Soft oval ground shadow blending around the base.

A broken stone column, ancient temple architecture in ruin, dark gray weathered stone shaft with a large diagonal break midway down exposing rough fractured interior in lighter weathered gray, upper portion leaning but still one connected broken column, small debris chunks scattered around the base. Subtle moss tufts on the lower third. Palette: dark slate gray stone dominant, faint moss green at the base, fractured rough interior in lighter weathered gray, faint cold blue rim highlight.

1 prop only, single isolated one continuous broken column, centered on transparent background, no characters, no creatures, no weapons, no duplicated columns.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, captions, numbers, watermark, sprite sheet, grid, border, frame, cropped object, duplicate object, multiple columns, two separate columns, intact complete column
```

**Output:** `stone_column_broken_01.png`

---

## STEP 7 — Brazier (lit, ember glow)

### PixelLab Settings (Önerilen — direct 64×64 ile başla, kalite yetmezse 256→64)
- **Output:** **64×64** (standard, 16 variant)

### Alternative Settings (organic ember glow için)
- Output: **256×256** (1 image) → Pixelorama nearest-neighbor 64×64

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The brazier is viewed from above at a diagonal — burning dish interior visible as a shallow bowl, three legs of the base angled toward camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE COMPOSITION: transparent background, 1 prop only, full object visible, centered, clean transparent padding, no border or frame.

ABSOLUTE STYLE: Pixel art prop, bold dark outline that remains readable after downsample, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades dark gritty ritual fire mood. Soft warm glow close to bowl only, not filling canvas.

A standing iron brazier with a small contained ember fire inside, dark cast iron three-legged base supporting a shallow wide dish, bright warm orange and red ember coals at the center with a small contained flame and a tight warm glow close to the bowl only. The iron base has a hammered weathered texture with subtle rust spots. Palette: dark iron base dominant, vibrant warm orange ember accent with red center and warm yellow flame edge, soft warm glow tight to the dish only.

1 prop only, single isolated full brazier, centered on transparent background, no characters, no creatures, no weapons, no duplicate copies.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, 3d render, soft shading, blur, smooth gradient at final size, anti-aliasing at final size, painterly style, anime style, modern style, photographic, realistic, text, words, letters, captions, numbers, watermark, sprite sheet, grid, border, frame, cropped object, duplicate object, multiple braziers, fire covering whole canvas, smoke effects covering canvas, glow filling background
```

**Output:** `brazier_lit_01.png`

---

## STEP 8 — Hanging Banner (torn)

### PixelLab Settings
- **Output:** **64×128 CUSTOM (Beta)**, 4 variant

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The banner is viewed from above at a diagonal — iron rod at the top visible as a thin horizontal element, hanging cloth drapes vertically. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE COMPOSITION: transparent background, 1 prop only, tall narrow centered prop, full height visible, not cropped, clean padding on sides, no border or frame.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades + Salt-and-Sanctuary dark gritty old battlefield mood. Soft oval ground shadow below the bottom of the cloth.

A long torn cloth banner hanging vertically, dark crimson red fabric with frayed bottom edges and a horizontal tear midway suggesting age and battle, an abstract non-letter faded emblem shape barely visible on the upper half of the cloth (no readable symbols, no letters), attached at the top to a thin dark iron rod. The fabric has subtle weathered texture and a few small tear holes. Palette: dark crimson red dominant, faded dirty cream emblem shape on upper half, dark iron rod, faint cold blue rim highlight.

1 prop only, single isolated full banner, centered on transparent background, no characters, no creatures, no weapons, no duplicate copies.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, numbers, watermark, sprite sheet, grid, border, frame, cropped object, duplicate object, logo, readable symbols, multiple banners, intact undamaged banner, decorative frame
```

**Output:** `banner_torn_01.png`

---

# 🟢 P2 BATCH — POLISH (3-7 gün)

---

## STEP 9 — Stone Altar

### PixelLab Settings (Önerilen — direct 64×64 ile başla)
- **Output:** **64×64** (standard, 16 variant)

### Alternative (organic stains/marks için)
- Output: **256×256** → downsample 64×64

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The altar viewed from above at a diagonal — flat ritual top surface clearly visible, front and side panels angled toward camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE COMPOSITION: transparent background, 1 prop only, full object visible, centered, clean transparent padding, no border or frame.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades + Salt-and-Sanctuary dark gritty ritual mood. Faint dark red glow accent in carved marks. Soft oval ground shadow.

A small ritual stone altar, square dark gray weathered stone block with a flat ritual top surface, abstract non-letter ritual grooves carved deeply into the front and visible side panels glowing very faintly with dark red light (no readable symbols, no letters). The altar top has subtle dripping stain marks suggesting old rituals. Palette: dark slate gray stone dominant, faint dark red accent glow in the carved grooves, faint cold blue rim highlight.

1 prop only, single isolated full altar, centered on transparent background, no characters, no creatures, no weapons, no duplicate copies.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, numbers, typography, watermark, sprite sheet, grid, border, frame, cropped object, duplicate object, logo, readable symbols, runes, multiple altars, intact pristine altar
```

**Output:** `stone_altar_01.png`

---

## STEP 10 — Treasure Pile ★ 256→64 ÖNERİLEN

### PixelLab Settings
- **Output:** **256×256** (1 single image) → Pixelorama nearest-neighbor 64×64

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The treasure pile viewed from above at a diagonal — tops of gold stacks visible as ovals, gemstones glint upward. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE COMPOSITION: transparent background, 1 prop only, full object visible, centered, clean transparent padding, no border or frame. Designed to remain readable after nearest-neighbor downsample to 64x64.

ABSOLUTE STYLE: Pixel art prop, bold dark outline that remains about 1 pixel after downsample, hard pixel edges target, no smooth gradients at final size, max 2 tones per color. Hades dark dungeon treasure mood. Soft warm rim glow around the gold. Soft oval ground shadow.

A small treasure pile of scattered gold coins and a few colorful gemstones, gold coins stacked irregularly in compact stacks with some loose coins scattered, a limited few cyan and emerald green gemstones glinting among the gold (not a noisy rainbow). The pile is sitting on dark stone floor with faint cold blue rim glow on the gems. Palette: warm gold dominant, vibrant cyan and emerald green gem accents, dark stone base, faint warm yellow shine on coin highlights.

1 prop only, single isolated full treasure pile, centered on transparent background, no chest, no bag, no container, no characters, no creatures, no weapons, no duplicate piles.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, 3d render, soft shading at low resolution, blur, smooth gradient at final size, anti-aliasing at final size, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, numbers, typography, watermark, sprite sheet, grid, border, frame, cropped object, duplicate object, logo, multiple treasure piles, treasure chest container, treasure bag, rainbow gem pile
```

**Output (256→64 downsample):** `treasure_pile_01.png`

---

## STEP 11 — Hanging Chains

### PixelLab Settings
- **Output:** **64×128 CUSTOM (Beta)**, 4 variant

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The chain viewed from above at a diagonal — top iron bracket visible at the top edge, individual chain links go downward with slightly rotated links suggesting weight and slight motion. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE COMPOSITION: transparent background, 1 prop only, narrow tall centered prop, full height visible, not cropped, clean padding on sides, no border or frame.

ABSOLUTE STYLE: Pixel art prop, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading. Hades + Salt-and-Sanctuary dark gritty dungeon mood. Soft oval ground shadow below the bottom link.

A single vertical hanging chain of dark iron, weathered links going from top to bottom in one continuous chain with slightly rotated individual links suggesting weight, the bottom link ending just above the ground. The chain has subtle rust and dirt staining on the links. Attached at the top by a small iron bracket protruding from a stone ceiling fragment. Palette: dark iron gray dominant, faint rust accent on a few links, faint cold blue rim highlight on one edge.

1 prop only, one continuous chain only, single isolated full chain, centered on transparent background, no characters, no creatures, no weapons, no duplicate chains.

Negative Prompt :
characters, humans, creatures, animals, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, captions, numbers, watermark, sprite sheet, grid, border, frame, cropped object, duplicate object, multiple chains, chain with attached prisoner, manacles, swirling rope, twisted multiple coils
```

**Output:** `hanging_chains_01.png`

---

## STEP 12 — Kneeling Statue ★ 256→64 ÖNERİLEN

### PixelLab Settings
- **Output:** **256×256** (1 single image) → Pixelorama nearest-neighbor 64×64

### Prompt:

```
ABSOLUTE CAMERA RULE: high top-down camera angle, exactly 30 to 35 degrees downward tilt, ARPG perspective. The kneeling statue viewed from above at a diagonal — broken neck stump top surface visible as fractured plane, kneeling body angles toward camera. NOT flat front view, NOT side profile, NOT isometric.

ABSOLUTE COMPOSITION: transparent background, 1 prop only, compact pose (kneeling, head broken off compresses height), full object visible, centered, clean transparent padding, no border or frame. Designed to remain readable after nearest-neighbor downsample to 64x64.

ABSOLUTE STYLE: Pixel art prop, bold dark outline that remains about 1 pixel after downsample, hard pixel edges target, no smooth gradients at final size, max 2 tones per color. Hades + Salt-and-Sanctuary dark gritty ruined temple guardian mood. Soft oval ground shadow at base.

A stone statue only, not alive, of a kneeling humanoid figure in a compact pose, ancient temple guardian, dark gray weathered stone with subtle moss tufts on the lower portion, the head is completely broken off and missing with rough fractured neck stump showing lighter weathered stone interior, the stone hands clasped in a prayer pose at chest level. The body has subtle stone-carved armor or robe details. Palette: dark slate gray stone dominant, faint moss green tufts at the base, fractured rough interior on broken neck stump in lighter weathered gray, faint cold blue rim highlight.

1 prop only, stone statue only, not alive, single isolated full statue, centered on transparent background, no living characters, no living humans, no creatures, no weapons, no duplicate statues.

Negative Prompt :
alive creature, living human, living character, breathing, animated pose, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, 3d render, soft shading, blur, smooth gradient at final size, anti-aliasing at final size, painterly style, anime style, modern style, photographic, realistic, text, words, letters, captions, numbers, watermark, sprite sheet, grid, border, frame, cropped object, duplicate object, multiple statues, standing statue, complete intact statue with head
```

**Output (256→64 downsample):** `kneeling_statue_01.png`

---

# 📋 ÜRETİM SUMMARY TABLE

| Step | Prop | Output | Variant | 256→64? |
|---|---|---|---|---|
| 1 | Wooden Crate | 64×64 standard | 16 | ❌ Direct |
| 2 | Stone Urn | 64×64 standard | 16 | ❌ Direct |
| 3 | Candle | 64×64 standard | 16 | ❌ Direct |
| 4 | Debris Pile | 64×64 standard | 16 | ❌ Direct (Codex önerisi) |
| 5 | Column intact | 64×128 CUSTOM | 4 | ❌ Native tall |
| 6 | Column broken | 64×128 CUSTOM | 4 | ❌ Native tall |
| 7 | Brazier | 64×64 standard | 16 | ◐ Opsiyonel (kalite yetmezse 256→64) |
| 8 | Banner torn | 64×128 CUSTOM | 4 | ❌ Native tall |
| 9 | Stone Altar | 64×64 standard | 16 | ◐ Opsiyonel |
| 10 | Treasure Pile | **256×256 → 64** | 1 | ✅ MUTLAKA (organic detail) |
| 11 | Hanging Chains | 64×128 CUSTOM | 4 | ❌ Native tall |
| 12 | Kneeling Statue | **256×256 → 64** | 1 | ✅ MUTLAKA (organic fracture) |

---

# 🎯 BU GECE — Codex önerisi

**Sadece STEP 1-4 yeter (4 P0 prop, hepsi direct 64×64).** Gen budget'ı koru. Yarın STEP 5-8 (P1), 3-7 gün STEP 9-12 (P2).

PixelLab'a git, **STEP 1 (Wooden Crate)** prompt'unu kopyala, başla. 🎯

---

# 🔧 256→64 Downsample Workflow (STEP 10, 12 için)

1. PixelLab Output: 256×256 → Generate (1 single image)
2. PNG indir
3. Pixelorama / GIMP aç → Image → Scale Image → **Width: 64, Height: 64, Filter: Nearest-neighbor**
4. Save as 64×64 PNG

(Veya Python script ile batch downsample — söyle, yazarım.)

---

# 📚 Cross-references

- Codex review (kaynak): `STAGING/codex_review_prop_prompts_DONE.md`
- v5 archive (önceki): `STAGING/_archive/RIMA_MAP_PRODUCTION_SEQUENCE_v5.md`
- Laureth Studio pipeline: `F:/LaurethStudio/01_PIPELINE/pixellab_production_knowledge.md`
- Karar #145 v2 (6 Use Cases): `memory/project_pixellab_character_states_workflow.md`
- Character anchor PROVEN template: `F:/LaurethStudio/04_TEMPLATES/character_anchor_prompt_PROVEN.md`
