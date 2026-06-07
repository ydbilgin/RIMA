ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed scenes only (4) BLOCKED if unclear.
NLM ACCESS: query NLM via uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>" if needed. Direct-read: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory.

# Amaç (Purpose)
Fix cliff SIDEWAYS overflow for real. Root cause (measured): cliff sprites are 2.0 x 3.0 WORLD UNITS (128x192 @ PPU64); a 2-wide top-center sprite on a pure W/E side edge ALWAYS spills ~1 unit into the void sideways (geometry: half the sprite is on the void side of a vertical edge, with no floor to occlude it). Scaling = rejected (kills look). LOCKED APPROACH (Opus + ax reasoning, user-directed "smart hide top under floor + skip some cells"):
- Place cliffs ONLY on FRONT-facing void edges (S / SE / SW) — there the sprite hangs straight DOWN below the front silhouette, its TOP rows overlap the host + northern floor tiles (sorted behind -> occluded), and overlapping neighbors form a continuous skirt.
- SKIP pure E and W side edges entirely (place NOTHING). Side edges are seen edge-on in high-top-down; the thin floor-tile edge is enough, and the wide sprite can only overflow there.
- This removes the W/E sideways overflow the user saw (e.g. cliff_1_10_W) at the source.

# Setup (exact)
Ground = isometric Tilemap "Ground", cellSize (0.96, 0.585). Cliff sprites: Assets/Sprites/Environment/CliffKit_RefB_pixelified/ (cliff_S, cliff_SE, cliff_SW, ... TopCenter pivot, PPU64). Void-facing FRONT dirs priority + neighbor offset (dc,dr): S=(-1,-1), SE=(0,-1), SW=(-1,0), E=(1,-1), W=(-1,1). Iso world delta: dWorldX=(dc-dr)*0.48, dWorldY=(dc+dr)*0.2925.

# Algorithm (ax 3.5 + Opus synthesis — THINK then implement via execute_code; NO new .cs)
Placement dirs considered (priority) = [S=(-1,-1), SE=(0,-1), SW=(-1,0)] ONLY. E/W/N/NE/NW are side/back faces and are NEVER placement dirs, so pure-side cells automatically get nothing.
For each Ground cell that HAS a tile:
  - dir = first of [S, SE, SW] whose neighbor cell (cell+(dc,dr)) has NO tile (ground.GetTile==null). If none -> continue.
  - cellCenter = ground.GetCellCenterWorld(cell).
  - INTERIOR SHIFT (push sprite toward floor interior so its TOP is occluded by floor and horizontal spill is only ~0.04 units):
        S  -> shift = (0f,     0.2925f)
        SE -> shift = (-0.48f, 0.2925f)
        SW -> shift = ( 0.48f, 0.2925f)
    Vector3 pos = cellCenter + (Vector3)shift;   // pos = the sprite TOP-CENTER pivot point.
  - 9-POINT OCCLUSION / SKIP TEST (the smart skip — this is the key): sample 9 points across the sprite's top edge:
        foreach t in { -0.96f, -0.72f, -0.48f, -0.24f, 0f, 0.24f, 0.48f, 0.72f, 0.96f }:
            Vector3 p = new Vector3(pos.x + t, pos.y, 0f);
            if (ground.GetTile(ground.WorldToCell(p)) == null) { skip = true; break; }
        if (skip) continue;   // ANY top-sample over void => the sprite top would spill => place NOTHING.
    This automatically skips single-cell spurs, convex corners, and tight interior-hole sides.
  - else PLACE: SpriteRenderer sprite = cliff_<DIR>; sortingLayerName = "Floor"; sortingOrder = -30 + Mathf.RoundToInt(20f - pos.y); name = "cliff_<col>_<row>_<DIR>"; parent under CliffRing/CliffSprites (must be ACTIVE).
Continuity: along a straight front run, inner-cell sprites overlap and cover the outer cells (which the occlusion test skips) with a ~0.04u margin => seamless gapless skirt.

# Steps
1. **GAP-TEST first (copy scene):** open `Assets/Scenes/_IsoGame_cliffgaptest.unity` (it already exists with interior holes; if not, create from _IsoGame and punch a separated 1-cell + 3-cell + 4-cell interior hole). Clear CliffRing/CliffSprites, run the algorithm. Screenshot whole island + each hole: `Assets/Screenshots/cliff_skipEW_gaptest_{wide,holeA,holeB}.png`. Report placed vs skipped(E/W) counts. SAVE copy.
2. **APPLY to 3 LIVE scenes** (_IsoGame, _IsoGame_Map02, _IsoGame_Map03): for each, clear CliffRing/CliffSprites, run algorithm, ensure CliffRing+CliffSprites ACTIVE, SAVE. Screenshot each: `Assets/Screenshots/cliff_skipEW_{isogame,map02,map03}.png`.
3. Report per scene: placed count, skipped-E/W count, and note any cells where an S/SE/SW sprite still appears to extend sideways past the silhouette at convex corners (so Opus can judge if a corner-skip refinement is needed).

# Notes
- execute_code only, NO new .cs, no `using` (fully-qualify). Do NOT enter play mode (D3D12 risk; edit-mode screenshots only). Do NOT commit. Do NOT touch the portal/reward assets.
- profile laurethayday (orchestrator re-dispatches with yekta if quota-limited).

# Output
Per-scene placed/skipped counts + screenshot paths + convex-corner note.
