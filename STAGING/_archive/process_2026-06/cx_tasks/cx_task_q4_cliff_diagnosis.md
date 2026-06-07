# CX TASK — Q4: DIAGNOSE why the iso floating-island cliffs look broken (TESPIT ONLY, no changes)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

## Amaç (purpose)
User says the cliffs look "saçma" (silly/wrong): in a scene-view of the SW floor edge, the cliff faces are DETACHED from the floor lip — thin vertical rock slivers at corners, a dark gap between the floor diamond edge and the cliff tops, isolated cliff chunks floating in the void, and the cliffs do NOT form a continuous "island underside." DIAGNOSE the root cause precisely. **DO NOT change anything — this is tespit (diagnosis) only.** Orchestrator + ax are diagnosing in parallel; we will synthesize and fix after.

## What the orchestrator already found (live, _IsoGame, Unity RIMA@ed023e0b)
- Cliffs are TILEMAP-based: `CliffTilemap` (sortLayer Ground/-50, **tileAnchor=(0.50, 0.00, 0.00)** = bottom-center, grid cellSize iso (0.960, 0.585), Isometric layout).
- `CliffAutoPlacer` on GameObject `CliffRing` (active) does the placement.
- 0 cliff SpriteRenderer GOs (all via the tilemap tile).
- History notes (may be stale, verify): cliff sprites ~128×192px (CliffKit_RefB, PPU64, pivot ~0.84) while an iso cell ≈ 62×38px → sprite spans ~2×3 cells. "STAGE-A" applied `spriteScale.y=0.55` + `transformOffset.y=-0.11` + alpha-melt fade shader (CliffVoidFade). `DirectionalCliffTile` picks a direction sprite from neighbor bitmask.

## Investigate (read code + inspect live scene via UnityMCP execute_code, instance RIMA@ed023e0b)
1. `CliffAutoPlacer.cs` (on CliffRing) — Regenerate logic: which cells/edges get a cliff, what world/cell positions, any per-tile offset. Are cliffs placed on the OUTER void-facing edge of the floor diamond, and do those cells line up with the floor's actual boundary cells?
2. `DirectionalCliffTile.cs` (the TileBase asset on CliffTilemap) — its `GetTileData`: what `transform` matrix (offset/scale/rotation), `flags`, `colliderType` does it set? What are the per-direction sprites and their pixel sizes + pivots? This is the prime suspect for the detach/sliver look.
3. Inspect the live CliffTilemap: how many cells are actually painted, their cell coords, and for a few sample cells compute `CellToWorld` vs where the floor diamond edge is — quantify the GAP between floor lip and cliff top.
4. Cliff sprite assets: real pixel dimensions, PPU, pivot (read .meta or via AssetDatabase). Confirm sprite-vs-cell size ratio.
5. The CliffVoidFade material/shader + the STAGE-A scale/offset: are they applied via the tile's transform matrix, the tilemap transform, or the material? Confirm the actual current values.

## Deliverable (write to CODEX_DONE.md, Q4 section) — DIAGNOSIS ONLY
- Ranked list of ROOT CAUSES for: (a) thin vertical slivers at corners, (b) gap between floor lip and cliff, (c) isolated floating cliff chunks, (d) non-continuous underside.
- For each, the exact file:line / value responsible.
- A concrete FIX RECOMMENDATION (params or approach) — but DO NOT apply it. Options to weigh and pick among: (i) fix tile transform anchor/scale/offset + use a seamless per-cell cliff sprite, (ii) replace tilemap-cliff with a single baked cliff-skirt sprite per diamond edge (4 edges), (iii) proper dual-grid / 9-slice cliff tiling, (iv) render cliff as a downward extrusion strip flush to the floor lip. Recommend ONE with reasoning + rough effort.
- State BLOCKED + reason if a file/asset can't be found. Do NOT commit. Do NOT edit scenes.
