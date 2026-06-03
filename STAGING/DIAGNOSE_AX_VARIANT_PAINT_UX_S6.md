# AX DESIGN CONSULT — context-aware variant painting UX (RIMA tile tool)

No code. Return a concise (<350 words) reference + UX description. Read by Opus to inform a fix-forward.

## Context
RIMA is a 2D isometric ARPG (Hades/CoM family). It has a custom in-editor + in-game Map Designer that paints floor/cliff/prop tiles into rooms. A previous "unification" rewrite accidentally reduced painting to "place one selected asset per cell" and REMOVED a richer feature the user described as: "we had GROUPED the variants and painted variants based on situation/state."

## Questions
1. **What is "context/state-aware variant painting"?** Describe the common patterns in tile editors: (a) Wang/auto-tiling (tile picked from neighbor context — edges/corners auto-resolve), (b) variant buckets (a "material" = a group of interchangeable variants; the brush randomly/weighted-picks within the group for natural variation), (c) deterministic per-cell variant (hash of cell → stable variant). Which of these do pro 2D tools (Tiled, Godot TileMap terrains, Unity Rule Tiles) use, and how do they combine?
2. **For a Hades-style stone floor**, what's the right blend: a "granite" material group that auto-varies per cell (so the floor isn't repetitive) PLUS edge/corner Wang for the island boundary + cliffs? Describe the ideal authoring UX: pick a MATERIAL (group), paint, and the tool auto-selects the variant by position + neighbor context.
3. **UX:** how should the palette present GROUPED variants (one swatch per material group, not 16 individual tiles) so painting feels like "paint granite" not "pick tile 7"? Brief.
4. Any pitfalls when re-wiring this into a unified tool (seam mismatches, non-determinism on repaint, undo).

Keep it tight and practical — this guides restoring the feature.
