# CX IMAGEGEN TASK — Ruined-Keep WALL KIT (top-down 3/4, varied pieces, cleanly sliced PLACEHOLDER)

ACTIVE RULES: (1) think (2) on-brand not realistic (3) only the kit (4) BLOCKED note on tool limit.

## GOAL
RIMA needs a VARIED wall kit to build NATURAL/organic Ruined-Keep rooms (not one repeated pillar). Reference look: `STAGING/concepts/chatgpt_ref/` (broken masonry, varied heights, rubble) + `STAGING/floor_perspective_concepts/03_wallless_improved.png`. PixelLab re-makes later → PixelLab-standard + cleanly sliceable + transparent ([[clean cell-split rule]] → log to `STAGING/IMAGEGEN_PLACEHOLDER_REGISTRY.md`).

## ART SPEC
- **Top-down 3/4** (visible front face + height, like the 128x192 cliff sprites), **bottom-center pivot-friendly** (object sits on its base).
- On-brand: charcoal/blackened-iron/blue-slate (#1C1D24-#2E303F) broken granite masonry, **faint cyan #00FFCC seal-cracks SPARING**. HARD-NO: photoreal, gloss, gold, neon.
- Transparent PNG-32. Generate larger then it downsamples to the pixel targets below.

## DELIVERABLES → `STAGING/ruinedkeep_wallkit/` (individual transparent PNGs, exact names + sizes)
1. `wall_tall_intact.png` — tall broken wall segment, mostly intact (128x192).
2. `wall_mid_cracked.png` — medium wall, cracked, top crumbling (128x160).
3. `wall_low_parapet.png` — low crumbled parapet / ruined slab (128x96).
4. `pillar_tall.png` — tall standing column (64x160).
5. `pillar_broken.png` — broken/snapped column stub (64x96).
6. `rubble_pile.png` — pile of fallen masonry rubble (96x64).
7. `arch_gate.png` — broken stone archway / gate frame, cyan seal energy in opening (160x192).
8. `corner_buttress.png` — corner buttress/tower fragment (128x176).
Each: ONE piece per file, centered, transparent, exact size. Verify each.

## OUTPUT
Report the 8 paths + append to `STAGING/IMAGEGEN_PLACEHOLDER_REGISTRY.md` (🟡 placeholder). Touch nothing else.
