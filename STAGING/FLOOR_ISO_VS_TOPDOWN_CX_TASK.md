# CX TASK — Floor iso-grid vs top-down: CODE COUPLING + MIGRATION FEASIBILITY

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

## CONTEXT (verified by Opus via Unity MCP — do NOT re-discover, trust these facts)
Scene `Assets/Scenes/Test/PlayableArena_Test01.unity`. The floor "doesn't sit right": it is tiled ISOMETRIC while the camera + characters are TOP-DOWN.

Verified ground truth:
- `Floor` GameObject has a `Grid` with **cellLayout = Isometric**, cellSize = (1, 0.609375, 1).
- Floor tile asset = `Assets/ScriptableObjects/Floor/IsoTiles35/tile_*.asset`, sprite from texture in `Assets/Sprites/AssetPackV3/floor_iso_pixellab_35deg/` (these are 35-degree ISO DIAMOND slabs, 64x64, ppu64).
- A TOP-DOWN square stone-floor tileset ALREADY EXISTS: `Assets/Sprites/AssetPackV3/floor/tile_*.png` (square, top-down).
- Main Camera = orthographic, rotation (0,0,0) (flat 2D), `transparencySortMode = CustomAxis` (0,1,0), PixelPerfectCamera assetsPPU=64 ref 640x360.
- Cliffs = `DirectionalCliffTile_Hades.asset`, cliff_S sprite 128x192 (tall, Hades-style vertical faces), on sortingLayer "Ground".
- Floor child tilemaps under the Grid: `Tilemap` (floor, sortLayer Floor), `VoidBlocker`, `DecorCliffTilemap`, `CliffTilemap_Auto`, `EdgeFX_Auto`.
- PROJECT_RULES locked direction: "HIGH TOP-DOWN 3/4 (~70-80deg), NO iso projection math, NO true 45deg diamond." So the iso floor VIOLATES canon.

## YOUR JOB — answer ONLY these, with file:line evidence. NO code changes.
1. **Coupling audit:** Which runtime + editor systems assume the floor Grid is Isometric (cellLayout) or use iso neighbor vectors? Search the codebase. Specifically check: CliffAutoPlacer / cliff placement, DirectionalCliffTile, WalkabilityMap, RoomLoader floor painting, RoomPainter / map editor tools, any `cellLayout`, `Grid`, `WorldToCell`/`CellToWorld`, iso neighbor-vector tables (e.g. S=(-1,-1), N=(1,1)), `IsReachableFromPlayer`. List every file:line that would BREAK or MISBEHAVE if the Floor Grid switches to cellLayout=Rectangle with cellSize (1,1,1).
2. **Migration cost estimate (S/M/L):** To switch the floor to a RECTANGULAR top-down grid using the existing `AssetPackV3/floor/` square tiles — what exactly must change (grid setting, repaint tiles, cliff neighbor math, walkability coord math, edge/void tilemaps)? Rank each sub-task S/M/L and call out the single biggest risk.
3. **Alternative low-cost paths:** Is there any way to make the floor read top-down WITHOUT a full rectangular migration (e.g. swap only the tile ART to flatter/less-3D tiles while keeping iso grid; or keep grid but reduce the diamond read)? Honestly assess whether each actually fixes the "doesn't sit right" or just hides it.
4. **Your recommendation** (1 paragraph): cleanest path given canon + cost. State assumptions.

Output a concise markdown report. Evidence (file:line) required for claim #1. This is ANALYSIS ONLY — write nothing to the project.
