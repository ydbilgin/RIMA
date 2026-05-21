# Codex Task — Modular Wall Pack v2 Imagegen + Auto-Review

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev

İki aşama:
1. **Imagegen:** Aşağıdaki temizlenmiş prompt ile modular wall asset pack v2 üret (Codex imagegen / gpt-image-1).
2. **Auto-Review:** Üretilen v2'yi PIL ile analiz et + mevcut ChatGPT v1 ve Codex v1 ile kıyasla.

## Aşama 1 — Imagegen v2

**Output path:** `STAGING/_pixellab_outputs/walls/v2/act1_wall_modular_pack_codex_v2.png`

**Boyut:** 512×512 RGBA (transparent background korunsun, alpha kanalı zorunlu)

**Prompt (imagegen tool'una EXACT olarak ver):**

```
A 512x512 pixel art sprite sheet asset pack for a dark fantasy isometric dungeon game inspired by Hades and Diablo. The sheet contains distinct tiles organized in 4 horizontal sections, all in unified dark moody pixel art style.

GLOBAL STYLE: 30-degree dimetric isometric perspective for structural pieces. Flat 2D style for magical effects and decoration. Dark granite block stonework with deep dark mortar lines. Painterly chunky pixel art density. Sharp pixel edges, no anti-aliasing, no smooth gradients. Transparent background everywhere between tiles and around sprites.

PALETTE: Dark granite mid-tone, lighter granite highlight catching light from top-left, deep dark mortar lines, vivid cyan magical accent for rift effects, warm orange for flame, deep purple for banner, dark earthy green for moss.

ABSOLUTELY NO TEXT, NO LETTERS, NO WORDS, NO NUMBERS, NO LABELS, NO CAPTIONS, NO INSCRIPTIONS, NO TAGS, NO NAMES anywhere in the image. Pure visual pixel art content only. Do not write any names beneath the tiles. Do not annotate. Pure imagery only.

TOP ROW — 4 large hero feature tiles, each filling about a quarter of the sheet width:
- A massive isometric stone archway with dark granite blocks flanking, a prominent keystone block at the apex, and glowing cyan swirling magical portal energy visible inside the arch opening
- A large L-shape outer corner stonework where two perpendicular walls meet at a sharp 90-degree corner, weathered dark granite with crenellation top
- A tall freestanding stone column with a wider decorative capstone on top and a wider base block, dark granite stacked vertically with thin cyan glow at the very base
- A tall straight stone wall feature with multiple rows of large dark granite blocks, deep mortar lines, crenellation top, and a subtle cyan magical crack accent on the lower right portion

MIDDLE TWO ROWS — 16 modular wall tile pieces designed to seamlessly tile together:
- An isometric wall section running along the northeast diagonal axis with granite blocks designed to mate seamlessly with copies placed beside it
- An isometric wall section running along the northwest diagonal axis (mirror direction)
- Two slightly different small convex outer corner variants
- Two slightly different small concave inner corner variants
- Two T-shape three-way wall meeting variants
- Two wall terminator endcap variants
- A shorter waist-height straight wall version
- A short waist-height wall L-shape corner
- A short waist-height wall endcap
- Two iso platform foundation block variants
- A floor-to-floor transition step block

BOTTOM TWO ROWS — 32 small overlay sprites on transparent background:

First overlay row contains 16 magical cyan energy effect patterns (each one floating on transparent background, glowing semi-transparent cyan magical energy):
- A horizontal lightning-bolt crack
- A vertical lightning-bolt crack
- A small starburst with 6 rays
- A larger starburst with 12 rays
- Three parallel diagonal scratch marks
- A crosshatch scratch pattern
- A round glowing halo ring
- An oval glowing halo
- A single dripping teardrop shape
- Multiple teardrops together
- A spiral swirl
- A jagged zigzag lightning shape
- Concentric ring pulses
- An asymmetric energy pulse
- Horizontal directional burst lines
- Vertical directional burst lines

Second overlay row contains 16 small dungeon decoration sprites:
- Four organic moss patch variants in dark earthy green (one cluster, one vertical strand, one spreading patch, one hanging vines)
- A small standing candle with a warm orange flame
- A wall-mounted candle sconce with bracket
- An unlit torch silhouette
- A lit torch with bright warm orange flame
- An intact deep purple hanging fabric banner with gold trim
- The same banner torn and tattered
- A short hanging dark metal chain with visible links
- A long hanging dark metal chain
- A small stone scatter cluster of 3 to 4 rocks
- A low gray-brown dust mound silhouette
- A weathered skeleton skull lying on its side
- A small angular gem crystal with cyan glow

CRITICAL REQUIREMENTS: True pixel art with sharp 1:1 pixel boundaries throughout. Every pixel is a solid color block. No anti-aliasing, no smooth gradients, no blurry edges. All tiles use unified pixel density and palette. Transparent background between tiles and outside all sprites.

FINAL REMINDER: This image contains ONLY visual pixel art. ZERO text characters. ZERO labels under tiles. ZERO written words. ZERO names. ZERO numbers. ZERO inscriptions. Just paintings of dungeon walls, magical effects, and decorations. If you would normally put a label, leave that area completely transparent instead.

NEGATIVE: text, letters, words, numbers, labels, captions, names, tags, annotations, inscriptions, anti-aliasing, blur, gradient, modern brick, photorealistic, characters, vegetation outside moss tiles, white background, light sandstone colors.
```

## Aşama 2 — Auto-Review

v2 üretildikten sonra Python PIL ile analiz et + 3 sheet karşılaştırması yap:

**Karşılaştırılacak sheet'ler:**
- `STAGING/_pixellab_outputs/walls/v2/act1_wall_modular_pack_codex_v1.png` (v1)
- `STAGING/_pixellab_outputs/walls/v2/act1_wall_modular_pack_chatgpt_v1.png` (ChatGPT)
- `STAGING/_pixellab_outputs/walls/v2/act1_wall_modular_pack_codex_v2.png` (v2 — yeni üretim)

**Analiz metrikleri:**
- Image dimensions + mode
- Transparent background percentage
- Distinct alpha components (tile separation quality)
- Cyan presence percentage
- Pixel art sharpness (color count, edge ratio)
- **Text detection** — herhangi bir alanda text varsa flag et (önceki ChatGPT'de label sızdı, v2'de var mı kontrol)

**Output:** `STAGING/_research/MASTER_SHEET_V2_COMPARISON.md`

Bölümler:
1. v2 analiz sonucu
2. v1 vs v2 vs ChatGPT karşılaştırma tablosu
3. Text artifact check (v2'de label var mı)
4. Production verdict — hangisi en iyi
5. Slicing recommendation (winner için)

## Effort

high — imagegen + analiz, prompt iteration gerekebilir text-free için.

## Output Confirmation

- v2 PNG path + dimensions
- Comparison doc path
- Final winner verdict
