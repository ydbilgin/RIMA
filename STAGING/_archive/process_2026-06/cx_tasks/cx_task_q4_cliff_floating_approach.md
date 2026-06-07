# CX (yekta) TASK — Critique the cliff "floating-island depth" approach (DESIGN/feasibility, no code changes)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: code / STAGING / memory files.

## Amaç (purpose)
Get an INDEPENDENT implementation/feasibility critique of the orchestrator's recommended cliff approach. The goal: make the iso floating-island CLIFF convey DEPTH so the island reads as FLOATING IN THE AIR, and make it GENERATIVE (auto-wrap outer perimeter AND inner holes). DIAGNOSIS / DESIGN ONLY — do NOT change code/assets. Inspect code if useful (Unity instance RIMA@ed023e0b is live).

## Context
RIMA = 2D isometric ARPG (high top-down 3/4, Hades / Children of Morta). Room = floating iso-diamond granite island over a purple void. Cliff = the rocky underside/edge. It currently looks broken.
Diagnosed root causes (already confirmed): (1) all 52 cliff cells render the same `cliff_S` sprite — `DirectionalCliffTile.cs:99` first-branch-wins, placer paints the floor cell not the void cell, so SE/SW/E/W/N/corner sprites (which EXIST in ref_kit_b) are never selected; (2) `transformOffset.y=-0.11` (DirectionalCliffTile_Hades.asset) = measured 0.11u gap between floor lip and cliff top; (3) filters (`hasNorthVoid`, `voidNeighbors>=5`, monotonic-south in CliffAutoPlacer.cs:277-385) drop ~11 of 63 edge cells = floating chunks; (4) 128x192 (2x3-cell) sprites placed per-cell = overlap/slivers; STAGE-A `spriteScale.y=0.55` squashed the cliff and KILLED depth. Existing art `ref_kit_b` = 8-directional tapering stone SPIRE/stalactite pieces + corners, transparent PNG. Relevant files: `Assets/Scripts/Environment/CliffAutoPlacer.cs`, `Assets/Scripts/Environment/DirectionalCliffTile.cs`, `Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset`, `Assets/Materials/CliffVoidFade.mat`, `Assets/Shaders/CliffVoidFade.shader`.

## NEW REQUIREMENT
The cliff must be GENERATIVE — auto-wrap not only the outer island perimeter but also INNER HOLES (empty void gaps in the middle of the floor). Arbitrary boundaries.

## Orchestrator's recommended approach (CRITIQUE THIS)
- **Floating depth** = underside must be TALL + TAPER toward the bottom + DISSOLVE into the void (flat-bottomed slab reads as "sitting on something"; tapering/dissolving bottom reads as "hanging in air"). 4 layers: (1) tall rock skirt, no squash; (2) overhang shadow under floor lip; (3) vertical light gradient cool-top→black-bottom (extend `CliffVoidFade` shader to darken+desaturate, not just alpha-melt); (4) bottom dissolves into purple void (alpha fade + a few tapering rock teeth, no hard bottom edge). Plus framing/bg: island surrounded by void; optional parallax void-bg.
- **Look = HYBRID:** solid continuous stratified rock at the TOP (seamless across the edge, no slivers) breaking into tapering teeth that dissolve into void at the bottom.
- **Technique = generative modular placer** that wraps ANY boundary (outer perimeter + inner holes), selects the correct direction/corner piece per edge cell from the floor-vs-void adjacency, removes the -0.11 gap, places tall pieces flush, + the gradient shader. Art-agnostic.
- **Sequence:** fix the placer first → test free with existing ref_kit_b → if not floating-enough, produce a new modular hybrid pack (magenta bg, tileable strip + corners) → final via PixelLab (floor451 as style-ref).

## Deliver (write to CODEX_DONE.md, focus on IMPLEMENTATION)
1. Do you AGREE the generative modular tilemap-placer is the right technique, or is a procedural MESH/polyline extrusion strip (follow the boundary polyline, generate a tapering skirt quad/mesh) cleaner and more robust for the floating-depth look + arbitrary inner holes? Recommend ONE with reasoning.
2. Concrete technical plan for: (a) boundary detection that yields BOTH outer perimeter and inner-hole loops from the floor tilemap, (b) correct per-edge direction/corner selection, (c) flush placement (kill the 0.11 gap), (d) the tall+taper+dissolve depth (shader vs art vs mesh), (e) the gradient/void-fade shader changes.
3. Biggest risks/gotchas (depth-sorting with the player, inner-hole sorting, performance, seam artifacts).
4. Effort estimate per option. NO code changes — design/feasibility only. BLOCKED + reason if a file can't be read.
