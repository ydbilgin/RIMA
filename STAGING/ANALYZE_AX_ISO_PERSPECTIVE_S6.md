# TASK (ax / Gemini) — Diagnose top-down vs ISOMETRIC + confirm the iso recipe (analysis, inline)

NLM ACCESS: Confirm the canonical perspective + iso decisions via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

RESPOND INLINE (the dispatcher captures stdout). You ANALYZE only — cx applies. Do NOT edit files.

## Amaç (Goal)
The game map / Map Designer currently renders **TOP-DOWN**, but the locked direction is **ISOMETRIC**. Confirm the exact iso recipe and diagnose every cause that makes a map render top-down instead of iso, so cx can apply the fix correctly.

## Inputs (read)
- `CURRENT_STATUS.md` top block — section "🎯 İSO FLOOR KÖK-NEDEN" (the already-solved iso recipe).
- `STAGING/ISO_TILING_LOGIC_DECISION.md` (4-source iso tiling logic).
- NLM canon on perspective (is isometric the locked choice? top-down was rejected?).

## Deliver (tight, inline)
1. **Confirm canon:** Is ISOMETRIC the locked perspective per NLM, and was top-down explicitly rejected? Quote the decision.
2. **The exact iso recipe** (verify against CURRENT_STATUS): which floor tiles to use (PixelLab Floor **451bbfd8** ORIGINAL iso granite — NOT flat top-down `ce6f15c7`/`flat_tile`, NOT flattened `pl_floor`); the Grid **cellSize = measured diamond ratio ≈ (0.96, 0.585)** [451 diamond ~62×38px @ PPU64]; **NO mathematical squash** (root scale Y=0.5 was rejected by the user as artificial); layout/sort notes.
3. **Diagnose top-down causes** — list every reason a map would read top-down instead of iso: tile generated with `tile_view_angle:90` (=top-down), flatten step removing iso depth, Grid cellSize square (e.g. 0.94×0.94) instead of diamond ratio, camera/sprite-sort issues, wrong default floor group in the Map Designer.
4. **Apply checklist for cx** + exactly what must be **visually verified in Unity** (iso floor tessellates seamlessly, character sits correctly, no vertical gaps).

End with one line: `RECOMMENDATION: <the single most likely current cause + first fix>`.
