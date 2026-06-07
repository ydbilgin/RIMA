# CX (yekta) TASK — Cliff looks ARTIFICIAL: critique + path to NATURAL (design/feasibility, no code changes)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: query NLM if needed: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"
Direct-read only: code / STAGING / memory.

## Amaç (purpose)
The user says the new PROCEDURAL-MESH cliff "looks too artificial" and that the older hand-made cliffs looked more natural. Give an INDEPENDENT critique + recommend the cleanest path to NATURAL-looking iso cliffs. DESIGN/FEASIBILITY ONLY — no code/asset changes. You may inspect the implemented files.

## Context / what exists now
- RIMA = 2D isometric ARPG (high top-down 3/4, Hades / Children of Morta). Floating iso-diamond granite island over a purple void.
- We REPLACED the broken tilemap cliff with a PROCEDURAL BOUNDARY MESH (your own earlier recommendation): `Assets/Scripts/Environment/CliffMeshGenerator.cs` builds a skirt ribbon hanging below the floor boundary loop (outer + inner holes), `cliffHeightWorld=3`, taper, irregular bottom teeth. `Assets/Shaders/CliffVoidFade.shader` does: horizontal strata bands (procedural), value-noise mottle, top-lip cool light, AO under lip, color-temp gradient → purple void dissolve. Material `CliffVoidFade_Mesh.mat`.
- PROBLEM (user): it looks ARTIFICIAL — the regular horizontal strata terraces + smooth mesh geometry + flat procedural shading read as CG/fake, not natural rock.
- The PREVIOUS hand-made art `Assets/Sprites/Environment/CliffKit_RefB/` (= `STAGING/_archive/s106_overnight/ref_kit_b/`: 8-directional tapering stone SPIRE/stalactite sprites + corners + cyan_glow, hand-pixeled, transparent) looked MORE NATURAL/organic — but had placement bugs (all rendered as `cliff_S`, gaps, slivers).

## Question — recommend ONE path to NATURAL cliffs (with reasoning + effort)
(a) **Keep the mesh** (it gives continuity, inner-hole support, flush edge) but DROP procedural strata fakeness and instead **map a HAND-PAINTED irregular rock TEXTURE onto the mesh face** (mesh = shape/silhouette, painted tileable texture = natural rock detail). How would you UV/tile it (seamless horizontal rock-face strip, magenta-bg produced then keyed, or PixelLab)? How to avoid visible repetition?
(b) **Go back to ART-BASED modular sprite pieces** (ref_kit_b organic spires) but FIX placement (correct per-edge direction selection from the void-side mask, dense, no gaps, kill the 0.11 offset). Accept the "spire curtain" look as the intended natural style.
(c) **HYBRID:** mesh for the continuous silhouette/occlusion + hand-painted sprite detail/teeth overlaid on top.

Also answer: what SPECIFICALLY makes a 2D iso cliff read NATURAL vs ARTIFICIAL (silhouette irregularity, hand-painted vs procedural texture, broken horizontal lines, non-uniform lighting, etc.)? Be concrete. Inspect `CliffMeshGenerator.cs` + `CliffVoidFade.shader` if useful.

## Deliverable (CODEX_DONE.md)
Ranked recommendation (a/b/c) with WHY + concrete how + effort estimate, and the natural-vs-artificial principles. No code changes. BLOCKED + reason if files unreadable.
