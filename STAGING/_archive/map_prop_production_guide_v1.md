---
status: GUIDE_LIVE
faz: post-Brush-V1
tarih: 2026-05-18
ozet: "Map prop production guide — PixelLab'da 12 prop nasıl üretilir, nereye kaydedilir, nasıl PropDefinitionSO yapılır"
priority: TONIGHT (user explicit: "map yarına hazırla")
---

# Map Prop Production Guide v1 — Kullanıcı Üretim Rehberi

**Hedef:** Bu gece/yarın PixelLab'da 12 prop sprite üret → Unity import → PropDefinitionSO oluştur → Brush V1 ile odalara yerleştir → ilk güzel map screenshot.

---

## 1. PixelLab Settings (12 prop için ortak)

| Setting | Değer | Not |
|---|---|---|
| **Mode** | PixelLab → **Create Image Pro V3** | Karakter sprite'larıyla aynı pipeline |
| **Style ref panel** | **BOŞ** (her prompt'ta) | Tüm sinyal MAIN PROMPT'tan |
| **Camera (UI dropdown)** | **High top-down** | Karakter ile uyum kritik |
| **Output size** | **64×64** (Tier 1 small) veya **48×64** (uzun) veya **96×96** (Tier 2) | Boyut tablosu aşağıda |
| **Describe reference kutusu** | **BOŞ** | |

---

## 2. Prop Üretim Sırası (öncelik P0 → P2)

### 🔴 P0 — Sample room için MUTLAKA (4 prop)
Bu 4 prop olmadan room kompozisyonu eksik kalır. **Bu gece üret.**

| # | Prop adı | Boyut | Sample room oda |
|---|---|---|---|
| 1 | **Wooden Crate (small)** | 32×32 | Combat rooms |
| 2 | **Stone Urn (broken)** | 32×48 | Treasure, Combat, Shrine |
| 3 | **Candle + iron holder** | 16×24 | Shrine, Treasure |
| 4 | **Debris pile (rubble + bones)** | 32×32 | Combat, post-fight rooms |

### 🟡 P1 — Daha iyi görünüm için (4 prop)
Yarın üret. Brush V1'le yerleştirilince oda **çok** canlanır.

| # | Prop adı | Boyut | Sample room oda |
|---|---|---|---|
| 5 | **Stone column (intact)** | 32×64 | Combat Medium/Large |
| 6 | **Stone column (broken/cracked)** | 32×64 | Combat (variety) |
| 7 | **Brazier (lit, ember glow)** | 32×48 | Shrine, Boss Intro, Hub |
| 8 | **Hanging banner (torn)** | 32×64 | Combat, Boss Intro |

### 🟢 P2 — Polish (4 prop)
3-7 gün içinde. Phase 2 başı için yeter.

| # | Prop adı | Boyut | Sample room oda |
|---|---|---|---|
| 9 | **Stone altar (small)** | 48×48 | Shrine, Boss Intro |
| 10 | **Treasure pile (gold + gems)** | 48×32 | Treasure room |
| 11 | **Hanging chains (coiled)** | 16×64 | Combat, Dungeon corridor |
| 12 | **Kneeling statue (broken)** | 48×80 | Shrine, Boss Intro centerpiece |

---

## 3. Ortak Prompt Header (her prop'un başına koy)

```
ABSOLUTE CAMERA RULE: high top-down camera angle, EXACTLY 30 to 35 degrees downward tilt from horizontal, ARPG-style overhead bird's-eye perspective like Hades and Hyper Light Drifter. The prop is viewed clearly from above at a diagonal — the top plane of the prop is visible as a rounded or flat shape, side surfaces are visibly angled diagonally toward the camera. NOT a flat front view, NOT a side profile, NOT an isometric projection, NOT a pure 90-degree top-down.

ABSOLUTE TEXT RULE: NO text, NO words, NO letters, NO labels, NO numbers, NO typography of any kind.

ABSOLUTE STYLE: Pixel art prop sprite, transparent background, 1px solid black outline, hard pixel edges, no anti-aliasing, no gradients, flat cell shading max 2 tones per color. Tone family: Hades + Hyper Light Drifter + Salt-and-Sanctuary dark gritty mood — "Vivid Vulnerability + Ritual Catastrophe" palette (faded earth tones with subtle accent colors). Soft oval ground shadow beneath the prop.

The prop is a static environmental piece, no characters, no creatures, no weapons attached.
```

---

## 4. Per-Prop Prompts (Copy-Paste Hazır)

### Prop 1 — Wooden Crate (small) — 32×32

```
A small wooden crate, weathered and chipped, dark brown stained planks with iron banding strips at the top and bottom, metal nail rivets visible. The crate has subtle wear marks and a few pixel-sized scuff marks on the side panels suggesting heavy use. Slight tilt of the top plane visible from the 30-35 degree downward camera. Palette: dark brown wood dominant, faded rust iron bands, faint cold blue rim highlight on one edge. Pixel art at 32x32, single isolated prop, transparent background.
```

### Prop 2 — Stone Urn (broken) — 32×48

```
A broken stone urn, ancient ritual pottery, dark gray weathered stone body with hairline cracks running vertically, a section of the top rim chipped away revealing the hollow interior, dust and pixel-debris around the base. The urn body has subtle Sigil-like engraved lines around its midsection. Slight tilt of the rim visible from the 30-35 degree downward camera. Palette: dark slate gray dominant, dirty cream lip highlight, faint dark red rune accent in the engraved lines. Pixel art at 32x48, single isolated prop, transparent background.
```

### Prop 3 — Candle + iron holder — 16×24

```
A single tall thin wax candle in a small iron candleholder base, soft warm orange flame at the wick with a faint glow halo, dripped wax frozen in pixel droplets running down the candle side. The iron holder base is dark cast iron with a hammered texture. Slight tilt visible from the 30-35 degree downward camera showing the holder dish. Palette: warm cream wax dominant, dark iron base, vibrant orange flame accent with faint warm yellow halo. Pixel art at 16x24, single isolated prop, transparent background.
```

### Prop 4 — Debris pile (rubble + bones) — 32×32

```
A small debris pile, mixed weathered stone rubble chunks and a few skeletal bone fragments scattered together, suggesting an aftermath of battle or ancient ruin. Stone pieces are dark gray with chipped edges, bones are dirty cream with faint dust marks. The pile has irregular outline with a few pixel pebbles scattered around the base. Slight perspective tilt from the 30-35 degree downward camera. Palette: dark slate gray stone dominant, dirty cream bones, faint cold blue dust rim highlight. Pixel art at 32x32, single isolated prop, transparent background.
```

### Prop 5 — Stone column intact — 32×64

```
A tall thin intact stone column, ancient temple architecture, dark gray weathered stone shaft with subtle vertical channels and a simple top capital plate, a wider stone base at the foot. The column body has minor moss tufts on the lower portion suggesting age. Slight perspective tilt from the 30-35 degree downward camera showing the top capital plate. Palette: dark slate gray stone dominant, faint moss green tufts at the base, faint cold blue rim highlight on one edge. Pixel art at 32x64, single isolated prop, transparent background.
```

### Prop 6 — Stone column broken/cracked — 32×64

```
A broken stone column, ancient temple architecture, dark gray weathered stone shaft with a large diagonal break midway down exposing the rough fractured interior, the top half visibly leaning or fallen, pixel debris chunks scattered at the base. Subtle moss tufts on the lower portion. Slight perspective tilt from the 30-35 degree downward camera. Palette: dark slate gray stone dominant, faint moss green at the base, fractured rough interior in lighter weathered gray tones, faint cold blue rim highlight. Pixel art at 32x64, single isolated prop, transparent background.
```

### Prop 7 — Brazier (lit, ember glow) — 32×48

```
A standing iron brazier with a flickering ember fire inside, dark cast iron three-legged base supporting a shallow wide dish, bright warm orange and red ember coals at the center with a small flickering flame and faint warm glow halo radiating outward. The iron base has a hammered weathered texture with rust spots. Slight perspective tilt from the 30-35 degree downward camera showing the burning dish interior. Palette: dark iron base dominant, vibrant warm orange ember accent with red center and warm yellow flame edge, faint warm glow halo around the dish. Pixel art at 32x48, single isolated prop, transparent background.
```

### Prop 8 — Hanging banner (torn) — 32×64

```
A long torn cloth banner hanging vertically, dark crimson red fabric with frayed bottom edges and a horizontal tear midway suggesting age and battle, an old faded sigil or rune design barely visible on the upper half of the cloth, attached at the top to a thin dark iron rod. The fabric has subtle weathered fabric texture and a few pixel-tear holes. Slight gentle sway implied by the tilt of the iron rod. Palette: dark crimson red dominant, faded dirty cream sigil accent at the top, dark iron rod, faint cold blue rim highlight. Pixel art at 32x64, single isolated prop, transparent background.
```

### Prop 9 — Stone altar (small) — 48×48

```
A small ritual stone altar, square dark gray weathered stone block with a flat ritual top surface, ancient rune engravings carved deeply into the front and side panels glowing very faintly with dark red light. The altar has subtle dripping stain marks on the top suggesting old rituals. Slight perspective tilt from the 30-35 degree downward camera showing the engraved top surface. Palette: dark slate gray stone dominant, faint dark red rune accent glow in the engravings, faint cold blue rim highlight. Pixel art at 48x48, single isolated prop, transparent background.
```

### Prop 10 — Treasure pile (gold + gems) — 48×32

```
A small treasure pile of scattered gold coins and a few colorful gemstones, gold coins stacked irregularly in pixel-perfect stacks with some loose coins scattered, a few cyan and emerald green gemstones glinting among the gold. The pile is sitting on dark stone floor with faint cold blue rim glow on the gems. Slight perspective tilt from the 30-35 degree downward camera. Palette: warm gold dominant, vibrant cyan and emerald green gem accents, dark stone base, faint warm yellow shine on coin highlights. Pixel art at 48x32, single isolated prop, transparent background.
```

### Prop 11 — Hanging chains (coiled) — 16×64

```
A vertical hanging chain of dark iron, weathered links going from top to bottom with a few twist coils midway suggesting kinetic motion, the bottom link ending in a frozen mid-sway position. The chain has subtle rust and dirt staining on the links. Attached at the top by a small iron bracket. Slight perspective alignment from the 30-35 degree downward camera. Palette: dark iron gray dominant, faint rust accent on a few links, faint cold blue rim highlight on one edge. Pixel art at 16x64, single isolated prop, transparent background.
```

### Prop 12 — Kneeling statue (broken) — 48×80

```
A broken stone statue of a kneeling humanoid figure, ancient temple guardian, dark gray weathered stone with subtle moss tufts on the lower portion, the head of the statue is completely broken off and missing with rough fractured neck stump, the hands are clasped in a prayer pose at chest level. The body has subtle armor or robe carved details. Slight perspective tilt from the 30-35 degree downward camera. Palette: dark slate gray stone dominant, faint moss green tufts at the base, fractured rough interior on the broken neck stump, faint cold blue rim highlight. Pixel art at 48x80, single isolated prop, transparent background.
```

---

## 5. Ortak Negative Prompt (her prop için aynı)

```
characters, humans, creatures, weapons, sword, axe, bow, gun, staff, side view, side profile, three-quarter view, isometric projection, flat front view, low angle, looking up, 3d render, soft shading, blur, smooth gradient, anti-aliasing, soft edges, painterly style, anime style, modern style, photographic, realistic, text, words, letters, alphabet, captions, labels, numbers, typography, watermark, signatures, multiple props in one image, prop set, prop grid
```

---

## 6. Üretim Sonrası — Unity Import + PropDefinitionSO

### Adım 1: PNG Import
1. Üretilen PNG'leri **`Assets/Sprites/Props/`** altında kategorize et:
   - `Assets/Sprites/Props/Crates/wooden_crate_small.png`
   - `Assets/Sprites/Props/Urns/stone_urn_broken.png`
   - `Assets/Sprites/Props/Lighting/candle_iron.png`
   - `Assets/Sprites/Props/Debris/debris_rubble_bones.png`
   - vb.

### Adım 2: Sprite Import Settings (Unity)
- **Sprite Mode:** Single
- **Pixels Per Unit:** 64 (Karar #100 LOCK)
- **Filter Mode:** Point (no filter) — pixel sharp
- **Compression:** None
- **Pivot:** Bottom (props yere oturmalı)

### Adım 3: PropDefinitionSO Oluştur
Brush V1 sistemine register etmek için her prop'a karşılık bir `.asset` gerek.

**Manuel yöntem (basit):**
1. `Assets/Data/Brush/Props/{Category}/` klasörü oluştur
2. Project window'da sağ tık → **Create → RIMA → Brush → PropDefinition**
3. Inspector'da doldur:
   - `propId`: GUID otomatik (PropDefinitionPostprocessor S13 LIVE)
   - `displayName`: "Wooden Crate (small)"
   - `category`: "Crate"
   - `sprite`: ilgili PNG referansı
   - `footprintTiles`: e.g., 1×1 (32×32 prop için), 1×2 (urn için)
   - `variantSprites`: opsiyonel (alternate sprite'lar — sonradan)

**Otomatik yöntem (Codex):**
Eğer 12 prop'un tümü hazırsa, Opus Codex'e Editor script yazdırabilir:
- Menu: `RIMA → MapDesigner → Brush → Generate PropDefinitions from /Sprites/Props/`
- Tüm PNG'leri tarayıp PropDefinitionSO oluşturur
- Footprint default 1×1, kategori dosya path'inden derive

### Adım 4: Sample Room'a Yerleştir
1. Unity menu: `RIMA → MapDesigner → Brush → Open Editor`
2. **Props Tab** aç (Sprint 12 LIVE)
3. Library/Combat_Small_01.asset yükle
4. PropPlacer ile yeni prop'ları yerleştir (mevcut barrel placeholder'ları değiştir)
5. R hotkey ile rotation (S13 LIVE)
6. Save

---

## 7. Hızlı Yol — "Bu Gece Sample Room Screenshot"

**4 saatlik plan (P0 props ile):**

| Saat | İş |
|---|---|
| 0:00-1:00 | PixelLab'da 4 P0 prop üret (Wooden Crate, Stone Urn, Candle, Debris) — her biri ~15 dk |
| 1:00-1:30 | Unity'e import + Sprite settings + PropDefinitionSO oluştur (manuel veya Codex script) |
| 1:30-2:30 | Brush V1 editor'da Combat_Small_01.asset aç + propPlacer ile yerleştir + scatter |
| 2:30-3:00 | Lighting: 2-3 torch (RIMA_wall_torch + LightFlicker) + ambient deep blue-teal |
| 3:00-3:30 | Camera setup + Pixel Perfect Camera + screenshot |
| 3:30-4:00 | Tweak + 2-3 farklı oda screenshot |

→ **Bu gece 4-6 güzel oda screenshot'u**.

---

## 8. Karar #145 v2 ile Variant Üretim (Phase 2 hazırlık)

Her base prop için **Use #6 conditional variant** ile renk/durum variant'ları üretilebilir (5000 gen budget rahat):

- `wooden_crate_small.png` → "burned variant" / "broken variant"
- `stone_urn_broken.png` → "intact variant" (yeni state)
- `candle_iron.png` → "extinguished variant" (no flame)
- `brazier_lit.png` → "cold/dead variant"

→ Karar #155 Use #6 ile her base prop 2-3 variant = 24-36 effective prop sayısı.

---

## ÖZET

| Aksiyon | Süre | Çıktı |
|---|---|---|
| **Bu gece:** 4 P0 prop üret | 1-2 saat | Sample room kompoze edilebilir |
| **Yarın:** 4 P1 prop + sample room screenshot | 4-6 saat | Beautiful map demo screenshot |
| **3-7 gün:** 4 P2 prop + variants | 6-10 saat | Phase 1 prop roster complete |

Hazırsın. PixelLab'a yapıştır + üret + 1-2 saatte ilk 4 prop'a sahip ol.
