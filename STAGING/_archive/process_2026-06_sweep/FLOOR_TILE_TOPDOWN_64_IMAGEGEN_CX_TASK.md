# CX IMAGEGEN TASK — Top-down 3/4 floor tileset, 64px, math-grid sliceable (PLACEHOLDER)

ACTIVE RULES: (1) think (2) on-brand not realistic (3) only the floor tiles (4) BLOCKED note on tool limit.

## GOAL
RIMA is migrating its floor from iso-diamond to **TOP-DOWN 3/4 (square grid)** (LOCKED). Generate a placeholder **cracked-stone floor tileset** that reads as a high top-down 3/4 ground plane (Hades / Children of Morta feel — see `STAGING/floor_perspective_concepts/03_wallless_improved.png` for the target). PixelLab will re-make these later → keep PixelLab-standard + cleanly sliceable.

## ART SPEC (from ax art-direction)
- **SQUARE tiles, top-down 3/4** — NOT iso diamonds. Slight vertical foreshortening (~10-15%, tile reads slightly wider than tall) to imply the 70-80° camera tilt.
- **Low-contrast seams** — faint soft cracks between stones, NO thick black grout (a loud grid lattice fights readability).
- **Muted value range** (~15% brightness spread) — the floor is a backdrop, must not pull focus from characters/VFX.
- **On-brand palette:** charcoal / blue-slate / blackened-iron (#1C1D24-#2E303F) cracked masonry, with **faint cyan #00FFCC seal-cracks SPARING** (a few tiles, subtle). HARD-NO: photoreal, gloss bevel, gold, neon.
- **Edge-tileable / seamless** — tiles repeat across a large floor without visible seams.
- **4-6 variants** (plain + subtle cracks + 1-2 with a faint cyan seal-vein + a debris/rubble one) for organic variation.

## SLICEABILITY (HARD — [[clean cell-split rule]])
Deliver as **individual 64x64 PNG tiles** (one file per variant) — `td_floor_01.png` ... `td_floor_06.png` — AND a **math-grid sheet** `td_floor_sheet.png` laid out as exact **64x64 cells in a single row** (no gutters, so it slices by pixel math). Opaque (ground, not transparent). Verify each tile is exactly 64x64 and edge-tileable.

## OUTPUT → `STAGING/floor_topdown_tiles/`
The individual tiles + the math-grid sheet + a 1-line report. Then append these to `STAGING/IMAGEGEN_PLACEHOLDER_REGISTRY.md` (date / floor tiles / path / 🟡). Touch nothing else.
