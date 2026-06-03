# UI/HUD Pack — imagegen BATCH 2 (card / button / bar) — CX

ACTIVE RULES: (1) think before generating (2) HARD style constraints below — ON-BRAND or rejected (3) save to the exact paths + report each file (4) BLOCKED if imagegen unavailable.

NLM ACCESS: not needed (style locked below).

## Goal
Batch 1 (backdrop / pedestal / panel-frame) passed ON-BRAND. Now produce the interactive-chrome set: a class-select CARD frame, a BUTTON surface, and a resource BAR (frame + fill). Same engine + workflow as batch 1 (generate HIGH RES, downsample to target). These sit on top of the dark backdrop, so they must read against #1C1D24-#2E303F.

## 🔒 HARD STYLE LOCK (identical to batch 1 — violate = rejected)
- **Palette:** charcoal / blue-slate / cold-grey stone / blackened iron (#1C1D24 -> #2E303F), desaturated teal shadows. **Cyan #00FFCC SPARINGLY** — only seal/rune/active seams. NO gold, NO parchment/beige, NO bright purple, NO sci-fi neon.
- **Motif:** cracked stone-masonry, thin iron bands + chains, rift cracks, circular seals, rune ticks.
- **Finish:** FLAT painterly / pixel-leaning, chunky readable forms, low noise. **HARD NO:** photoreal stone, glossy 3D bevels, smooth-vector gradients, soft airbrush, realistic lighting, baked TEXT, characters.
- **Format:** transparent PNG-32 (RGBA), NOT magenta.
- Negative prompt suggestion: `--no photoreal, realistic, 3d, gloss, bevel, gradient, vector, gold, parchment, neon, text, watermark, character, blur`.

## The 4 assets
1. **Class-select CARD frame (9-slice)** -> `Assets/Resources/UI/RIMA/Pack/card_frame_9slice.png`
   - Portrait card border (think Slay-the-Spire card chrome but dark-fantasy stone): stone-masonry edge + thin iron corner brackets + tiny cyan rune tick top-center. **EMPTY TRANSPARENT CENTER** (character art + text overlay on top). **UNIFORM border width all 4 sides** (corners ~28-32px at source). Source **256x384** (portrait).
2. **BUTTON surface (9-slice)** -> `Assets/Resources/UI/RIMA/Pack/button_9slice.png`
   - A FILLED dark-stone button plate (NOT transparent center — text sits on it) with a thin iron rim and a faint cyan edge seam at the bottom. Subtle, readable, low-detail. **UNIFORM border ~16px.** Source **192x64**.
3. **BAR frame (9-slice, horizontal)** -> `Assets/Resources/UI/RIMA/Pack/bar_frame_9slice.png`
   - An empty horizontal resource-bar socket: iron-banded stone trough, **EMPTY TRANSPARENT CENTER** (the fill shows through), thin rivets at the ends. **UNIFORM L/R border ~12px, thin T/B.** Source **256x48**.
4. **BAR fill (tileable horizontal)** -> `Assets/Resources/UI/RIMA/Pack/bar_fill.png`
   - A solid cyan #00FFCC energy fill with a faint inner glow + very subtle vertical rift striations, horizontally TILEABLE (seamless left-right). Opaque. This is the one place cyan is dominant (it's the active seal-energy fill). Source **64x32**.

## Output
- Save to the exact paths above (folder `Assets/Resources/UI/RIMA/Pack/` already exists).
- Report: filename + WxH + transparent? + style-lock match.
- imagegen DRAFTS, PixelLab-replaceable later.
