# CX IMAGEGEN TASK — Ruined-Keep TILEABLE WALL-RUN segments (flat front-face, seamless, PLACEHOLDER)

ACTIVE RULES: (1) think (2) on-brand not realistic (3) only the run kit (4) BLOCKED note on tool limit.

## GOAL
The scattered 3/4 DIORAMA chunks can't fake a continuous wall (each has a baked side-face + finished ends → doubled seams). Per `STAGING/DEPTH_AND_WALLRUN_RECIPE_S6.md` §2b, RIMA needs a **flat front-facing tileable run kit** so back+side walls read as ONE unbroken edge-to-edge masonry run (chatgpt_ref look). Target look: `Assets/Art/ConceptRefs/chatgpt_ref_wall_anchor.png` back wall. STYLE MUST MATCH the existing kit exactly: `Assets/Sprites/Environment/RuinedKeepKit/wall_tall_intact.png` + `corner_buttress.png` + `arch_gate.png` (blocky beveled charcoal/blue-slate granite courses, cyan rune-cracks). PixelLab re-makes later → PixelLab-standard + cleanly sliceable + transparent → log to `STAGING/IMAGEGEN_PLACEHOLDER_REGISTRY.md`.

## ART SPEC (HARD, non-negotiable)
- **FLAT FRONT FACE ONLY — NOT 3/4 box, NO receding side face.** This is the one kit that is drawn flat-on so copies tile into one wall. (Contrast the existing 3/4 chunks: those keep their side faces; do NOT reuse that geometry here.)
- **SEAMLESS LEFT↔RIGHT TILING:** brick courses run straight horizontally across BOTH vertical edges; leftmost + rightmost pixel columns must match so N copies butt-join into one unbroken wall with NO visible seam. Crenellated top line at a CONSTANT height (flat, not jagged-per-tile).
- **Bottom 0.5 cell = foot** (sits on/behind the floor edge); pivot bottom-center.
- On-brand palette: floor-darker context, but wall FRONT FACE lit value `#3b3950`, top/crenellation `#4c4a63` (cool rim), shadow/recess `#211f2e`, masonry base charcoal/blackened-iron/blue-slate `#1C1D24`→`#2E303F`. Cyan rune crack `#27e0c8` SPARING (<15% of pixels, only on the *_cracked variant + a touch on caps).
- HARD-NO: photoreal, gloss/bevel-shine, vector gradient, gold, parchment, neon, baked text, realistic. Transparent PNG-32 (NOT magenta-baked). Generate at 2–4× then downsample to the EXACT pixel targets below. Clean cell-split, one piece per file, centered.

## DELIVERABLES → `Assets/Sprites/Environment/RuinedKeepKit/` (individual transparent PNGs, EXACT names + sizes @PPU64)
1. `wall_run_mid.png` — **64×192** (1 cell wide; 2.5-cell visible flat face + 0.5-cell foot). Continuous brick courses, flat crenellated top at constant height, seamless L/R edges. The base tile. PROMPT: *flat front-facing dark granite dungeon wall segment, blocky stacked masonry courses, charcoal blue-slate blackened iron stone (#1C1D24 to #2E303F), lit front face value #3b3950, cooler crenellated top #4c4a63, recessed mortar shadows #211f2e, no side face, no perspective box, seamless tiling brick courses straight across both vertical edges, flat constant-height battlement top, top-down 3/4 dungeon, on-brand Diablo/Hades masonry, transparent background, pixel art*.
2. `wall_run_cracked.png` — **64×192** — same footprint + seamless edges as #1, plus ONE cyan rift crack `#27e0c8` glowing faintly down the face + a few displaced/missing bricks. In-place jitter variant. PROMPT: as #1 PLUS *a single glowing cyan #27e0c8 rune-crack splitting the stone, a few displaced and missing bricks, faint cyan seal-energy seeping from the crack, sparing cyan (under 15% of pixels)*.
3. `wall_run_low.png` — **64×96** — 0.75-cell parapet/half-wall version, seamless L/R edges, crumbled low top. For front/S stubs + run step-downs. PROMPT: as #1 but *a low crumbled parapet half-wall, ruined uneven top edge, seamless tiling sides, never blocks the camera*.
4. `wall_cap_left.png` — **64×192** — finished LEFT end-jamb: clean vertical stone jamb on the LEFT edge, courses tile seamlessly on the RIGHT edge (joins a run to its left). For where a run terminates at a void gap so the END looks intentional, not torn. PROMPT: as #1 but *a clean finished vertical wall end-jamb on the left side, structural corner pilaster, right edge tiles seamlessly into the run, faint cyan seam-rune*.
5. `wall_cap_right.png` — **64×192** — mirror of #4: clean vertical jamb on the RIGHT edge, courses tile seamlessly on the LEFT edge (joins a run to its right). PROMPT: as #4 but *finished vertical end-jamb on the RIGHT side, left edge tiles seamlessly into the run*.

Optional if tool budget allows (low priority, recipe §2c STEP 2 corner): `wall_run_corner_NE.png` / `wall_run_corner_NW.png` — **64×192** L-shaped connector joining a back-run to a side-run (inside corner). Skip if it risks the 5 core pieces; `corner_buttress.png` already covers corners as an anchor.

NOTE: corner_buttress (3c, 192px), pillar_tall (3c), arch_gate (4c), rubble_pile, pillar_broken from the EXISTING kit are KEPT as anchors/breakers — do NOT regenerate them. This task only adds the 5 (+2 optional) seamless run pieces.

## OUTPUT
Report the produced paths + verify each is exact-size, transparent, seamless L/R. Append the lines below to `STAGING/IMAGEGEN_PLACEHOLDER_REGISTRY.md` (🟡 placeholder). Touch nothing else.

### Registry log lines to append (after the wallkit row, line 19):
```
| 2026-05-31 | Ruined-Keep TILEABLE wall-run kit (wall_run_mid, wall_run_cracked, wall_run_low, wall_cap_left, wall_cap_right [+ optional wall_run_corner_NE/NW]) | `Assets/Sprites/Environment/RuinedKeepKit/` | flat front-face seamless-tiling wall-run segments, transparent PNG-32 @PPU64, clean cell-split placeholder | 🟡 placeholder |
```
