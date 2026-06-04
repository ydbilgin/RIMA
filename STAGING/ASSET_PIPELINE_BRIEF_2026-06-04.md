# ASSET PIPELINE BRIEF — CharacterSelect modular art (generate → slice → wire)
Date: 2026-06-04 · For council (cx feasibility / ax-3.1 art-direction / ax-3.5 lean)

## CONTEXT
User generated an EXAMPLE 1024×1024 modular UI asset sheet (Gemini imagegen) for the Character Select screen, in RIMA pixel-art style. They want council to decide HOW to produce + integrate it. The screen ITSELF is functional Unity runtime UI (C#); these are the ART ASSETS placed into it.

## THE EXAMPLE SHEET CONTENTS (what the user produced)
1. **Background "The Cosmic Void"** — 1920×1080 pixel-art: purple nebula + iso ruined-keep arches.
2. **Class slot buttons** — small 9-slice button frames (idle + cyan-active state).
3. **Skill card panel (composite)** — the RIGHT panel: stone-tablet header + parchment body + cyan resource bar + 3 skill cards (each a cyan silhouette glyph: summon-figure / portal / heal-figure).
4. **Class icons** — 10 small chibi class portraits (warblade-sword, summoner-woman-dreadlocks, ronin, gunslinger, ranger / shadowblade, ravager, hexer, etc.).
5. **Skill cards (standalone)** — 3 dark cards w/ cyan glyphs.
6. **Resource bars** — empty + cyan-filled progress bars.
7. **Character pedestal "Ashen Glyph"** — iso stone pedestal with cyan energy cracks.
8. **VFX sprites** — cyan sparkle/swoosh, orange embers, cyan lightning-cracks.

## IN-GAME REALITY (already exists — reuse candidates)
- **Canonical class sprites:** `Resources/Characters/<Class>/<class>_idle_south.png` for ALL 10 classes (PPU 64, the REAL in-game art).
- **Pack 9-slice frames:** `UI/RIMA/Pack/`: button_9slice, card_frame_9slice, panel_frame_9slice, bar_frame_9slice; plus pedestal_seal, bg_seal_keep.
- RIMA art canon: pixel-art, **PPU 64**, cyan #00FFCC, void-purple #3A1A4A, point-filter, no compression.
- **RULE:** "imagegen = ON-BRAND not realistic; **characters = PixelLab ONLY**." (gameplay character sprites must be PixelLab, not imagegen.)
- Tools available: ax `generate_image` (1024×1024) · cx `$imagegen`.

## DECISION QUESTIONS
1. **Generate as ONE sheet (like the example) then slice, OR generate each asset separately?** Trade-offs (consistency/palette vs slicing pain vs transparency/bleed). For RIMA pixel-art at PPU 64, which yields cleaner imports?
2. **Style consistency:** how to make imagegen-generated UI assets match the EXISTING in-game pixel sprites (idle_south) so the screen doesn't look like two art styles glued together? (palette lock, PPU, pixel_cleanup/quantize step?)
3. **Class icons — GENERATE (example style) vs REUSE canonical idle_south crops?** The example icons are imagegen approximations; our real game uses the idle_south sprites. Which is more consistent + does generating class icons violate "characters = PixelLab ONLY"? (These are UI icons, not gameplay sprites — is that an acceptable nuance, or must roster avatars be the real sprites?)
4. **Reuse vs regenerate per-piece:** which example pieces DUPLICATE existing Pack assets (frames / pedestal_seal / bar_frame / bg) → reuse? Which are GENUINELY new and worth generating (framed skill-card composite? Ashen Glyph pedestal? VFX? class icons?)?
5. **Can we slice the user's example sheet AS-IS** (transparency, cell alignment, quality), or must we regenerate cleaner separated pieces with transparent backgrounds?
6. **Slicing + import pipeline:** Unity multiple-sprite slicing (grid vs automatic), naming, import settings (PPU 64, Point, no-compress, alpha), and 9-slice border setup for the frame pieces. Minimal-friction recipe.
7. **Minimal high-impact GENERATE list + order** (reuse-first, but the user clearly wants the premium look — be realistic, not dogmatically zero-gen).
