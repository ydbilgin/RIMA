# Oval Feel Architecture — Codex Verdict

## Q1 — Oval feel approach
Verdict: E
Reasoning: Use Wang autotiles for biome-to-biome chains because the edge problem is structural, not just cosmetic. Add organic decal overlays where props, dirt scars, roots, puddles, and local breakup need to hide remaining square-grid reads. Larger tiles only reduce symptoms; shader blending is too risky for v15f.

## Q2 — Full PixelLab vs Hybrid
Verdict: FULL
Reasoning: Test FULL PixelLab now because the budget removes the main constraint and `create_topdown_tileset` directly targets the missing Wang-transition layer. PixelLab already proved stronger organic/painterly tile content, while Codex tiles are proven mainly for hard Wang16. Keep v15d as rollback, but do not keep splitting providers until the full PixelLab Wang test fails.
Migration path if FULL:
- Generate one PixelLab topdown Wang chain: foundation lower -> path/dirt/biome upper, using segmentation/no hard outline style.
- Import into a duplicate CombatBiome_v15f tileset path and wire without touching v15d rollback assets.
- Run one room visual QC pass against master reference: oval zone read, transition softness, player visibility, purple scatter suppression.

## Q3 — Unity render approach
v15f immediate: Standard Unity Tilemap
Phase 1.5 future: Custom chunked sprite mesh
Reasoning: For this week, keep Blueprint Painter + AutoPopulator + Tilemap because the immediate risk is art-source validation, not renderer architecture. Phase 1.5 should move to a chunked visual renderer for L2b/L4/L5/L6 because it can support layered blends, decals, and richer organic masks without fighting Tilemap cell boundaries. Shader blending remains S90+ deferred until art direction and chunked layering prove they need it.

## Confidence (per verdict)
Q1: HIGH
Q2: MED
Q3: HIGH
