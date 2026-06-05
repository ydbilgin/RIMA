# RIMA — CharacterSelect Asset Pipeline — ART DIRECTION lens (Gemini 3.1 Pro)

READ this brief first: F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\ASSET_PIPELINE_BRIEF_2026-06-04.md
(It describes a user-made 1024×1024 modular pixel-art UI sheet + the existing in-game assets + 7 questions.)

You are the ART-DIRECTION advisor on a council (feasibility + lean advisors answer separately). RIMA = dark pixel-art roguelite, cyan #00FFCC Rift/seal energy on void-purple, PPU 64, "ink-on-paper / Vivid Vulnerability".

Give strong, concrete art direction:
1. **Style cohesion (the #1 risk):** imagegen-generated UI art vs the EXISTING in-game pixel sprites (idle_south, PPU 64) must NOT look like two glued styles. Concrete recipe to enforce one look: palette lock (exact hex set), resolution/PPU discipline, outline/shading rules, post-process (quantize to N colors, pixel-snap). What art-direction constraints go INTO the imagegen prompt to match the game sprites?
2. **Class icons:** the example sheet has freshly-generated chibi class portraits, but the game already has canonical idle_south sprites for all 10. Which should be the roster avatar — generated icons (cohesive set, but a 2nd interpretation of each character) OR the real in-game sprite (authentic, but maybe awkward as a small icon)? Recommend, with reasoning about authenticity vs polish.
3. **Sheet vs separate generation** for visual consistency (single-prompt sheet keeps palette/lighting unified; separates risk drift). Your call for premium cohesion.
4. **Which pieces are worth generating vs reusing** existing Pack frames/pedestal/bg, purely on a QUALITY/cohesion basis (not just feasibility)? Where does generation meaningfully raise the bar (e.g. the framed skill-card composite, the Ashen Glyph pedestal, VFX) vs where reuse is indistinguishable?
5. **Prompt direction:** sketch the key art-direction phrases for the imagegen prompt(s) so output is on-brand RIMA (not generic fantasy, not realistic) and transparent/sliceable.

Tight, concrete, hex codes + rules over adjectives.
