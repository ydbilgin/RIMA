# UI/HUD Pack — imagegen BATCH 1 (CALIBRATION) — CX

ACTIVE RULES: (1) think before generating (2) HARD style constraints below — ON-BRAND or it's rejected (a prior realistic imagegen pass was rejected as off-style) (3) save to the exact paths + report each file (4) BLOCKED if imagegen unavailable.

NLM ACCESS: not needed (style locked below).

## Goal
Produce the FIRST 3 (calibration) UI assets to make RIMA's screens look like a real game. If these read ON-BRAND, a follow-up batch does cards/buttons/bars. Use your imagegen skill (gpt-image-2 / image_gen), generate at HIGH RES then these target sizes.

## 🔒 HARD STYLE LOCK (both cx+agy + Opus agreed — violate = rejected)
- **Palette:** charcoal / blue-slate / cold-grey stone / blackened iron (#1C1D24 → #2E303F), desaturated teal shadows. **Cyan #00FFCC used SPARINGLY** — only seals/runes/active glow seams. NO gold, NO parchment/beige, NO bright purple, NO sci-fi neon.
- **Motif:** floating seal-keep, cracked stone-masonry blocks, thin iron bands + chains, rift cracks, circular seals, rune ticks.
- **Finish:** FLAT painterly / pixel-leaning, chunky readable forms, low noise. **HARD NO:** photoreal stone, glossy 3D bevels, smooth-vector gradients, soft airbrush, realistic lighting, baked TEXT, characters.
- **Format:** **transparent PNG-32 (RGBA), NOT magenta.** Empty transparent center where applicable.
- Negative prompt suggestion: `--no photoreal, realistic, 3d, gloss, bevel, gradient, vector, gold, parchment, neon, text, watermark, character, blur`.

## The 3 calibration assets
1. **Menu/class-select BACKDROP** → `Assets/Resources/UI/RIMA/Pack/bg_seal_keep.png`
   - Full-screen dark-fantasy backdrop: a floating shattered seal-keep in deep void, cracked stone, faint cyan rift seams + soft vignette darkening to edges. Atmospheric, low-detail, NOT busy (UI sits on top). Target **1920x1080** (or 2048x1152), transparent-or-dark.
2. **Center PEDESTAL / seal-plate** → `Assets/Resources/UI/RIMA/Pack/pedestal_seal.png`
   - Circular/arched carved-stone platform with a glowing cyan circular seal/rune ring on top — where a chibi character stands in class-select. Transparent bg, top-down-ish 3/4. Target **512x512**.
3. **9-SLICE PANEL FRAME** → `Assets/Resources/UI/RIMA/Pack/panel_frame_9slice.png`
   - Dark stone-masonry panel border with thin iron band + tiny cyan rune ticks at corners; **EMPTY TRANSPARENT CENTER**; **UNIFORM border width on all 4 sides** (critical for Unity 9-slice — corners ~32px at source). Square source **256x256** (or 384x384).

## Output
- Save to the exact paths above (create `Assets/Resources/UI/RIMA/Pack/`).
- Report: filename + WxH + transparent? + that style matches the lock.
- These are imagegen DRAFTS, PixelLab-replaceable later (role-named, layer-clean).
