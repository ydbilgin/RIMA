ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files/scenes only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç (Purpose)
Diagnose the "cliff overflow" problem visually by building a FLOOR-GAP STRESS TEST and rendering 3 placement variants, so the orchestrator (Opus) can pick the correct geometry from screenshots. Unity `execute_code` ONLY — create NO new .cs scripts (no recompile, keeps Unity MCP stable). DO NOT modify the live scenes `_IsoGame` / `_IsoGame_Map02` / `_IsoGame_Map03`. Work only on a COPY.

# Context
- Unity must be open (instance `RIMA@...`). Live scene = `Assets/Scenes/_IsoGame.unity`.
- Ground = isometric `Tilemap` named "Ground", cellSize (0.96, 0.585). Iso world mapping for a cell-index delta (dc,dr): dWorldX = (dc - dr) * 0.48 ; dWorldY = (dc + dr) * 0.2925.
- Cliff sprites: `Assets/Sprites/Environment/CliffKit_RefB_pixelified/` — files cliff_S, cliff_SE, cliff_SW, cliff_E, cliff_W, cliff_N, cliff_NE, cliff_NW (.png, Sprite/Single, PPU64, Point filter, TopCenter pivot, ~128x192 px). Load sprite via AssetDatabase.LoadAssetAtPath<Sprite>.
- FRONT void-facing directions (priority order) with their cell-index neighbor offsets:
  S=(-1,-1), SE=(0,-1), SW=(-1,0), E=(1,-1), W=(-1,1).   (N/NE/NW are back-facing, NOT used.)
- CURRENT placement (the one that overflows): for each Ground cell, scan dirs in [S,SE,SW,E,W]; first dir whose neighbor cell has NO ground tile (ground.GetTile(neighbor)==null) → place `cliff_<DIR>` at `ground.GetCellCenterWorld(cell) + (0, +0.85, 0)`, 1 per cell, break. Sorting on the SpriteRenderer: sortingLayerName="Floor", sortingOrder = -30 + Mathf.RoundToInt(20f - worldY).
- PROBLEM: many cliffs overflow past the floor silhouette, especially W/E side edges and convex corners. The wide 128x192 sprite (~2x3 cells) centered per-cell spills sideways over the void where it shouldn't.

# Steps (execute_code, action:"execute"; NO `using` directives — fully-qualify e.g. UnityEditor.SceneManagement.EditorSceneManager, UnityEngine.Tilemaps.Tilemap)
1. Open `_IsoGame.unity`. SaveAs a COPY to `Assets/Scenes/_IsoGame_cliffgaptest.unity` (EditorSceneManager.SaveScene(scene, path, saveAsCopy:true) then open the copy). From here ONLY touch the copy.
2. Find `CliffRing/CliffSprites` (or create `CliffRing` empty + child `CliffSprites`, ACTIVE). Delete all its children.
3. FLOOR-GAP holes — place TWO holes in SEPARATE, well-spaced regions so each hole's FULL cliff ring is independently visible (user: "ayrı ayrı yerlerde, boşlukları tam görelim").
   - Compute interior cells = cells whose 8 neighbors (4 ortho + 4 diag) are ALL currently ground.
   - HOLE A = a compact 3-cell blob in the WESTERN third of the interior (e.g. an L or short line).
   - HOLE B = a compact 4-cell blob in the EASTERN third of the interior (e.g. a 2x2 square).
   - CONSTRAINTS: at least 3 cells of solid ground between each hole and the outer boundary; at least 6 cells of solid ground between HOLE A and HOLE B (so their hanging cliff sprites do NOT visually touch — we want to read each hole's cliffs in isolation). If the interior is too small to satisfy the 6-cell gap, use the largest gap possible and NOTE it in the report.
   - Record the chosen cell coords. Remove them: ground.SetTile(cell, null). These interior holes force the cells around them to face void on many sides = exposes overflow from all directions, around each hole independently.
4. Write a parameterized helper that, GIVEN a variant int, clears CliffSprites children and places cliffs:
   for each remaining Ground cell, find first void-facing dir in [S,SE,SW,E,W]; compute cellCenter = ground.GetCellCenterWorld(cell); compute Dworld = (dWorldX,dWorldY,0) from the dir's (dc,dr); Dhat = Dworld.normalized. Place pos by variant:
     VAR0 (current):        pos = cellCenter + (0, 0.85, 0)
     VAR1 (interior-push):  pos = cellCenter + (-Dhat * 0.45) + (0, 0.55, 0)
     VAR2 (edge-anchored):  pos = cellCenter + Dworld*0.5 + (-Dhat*0.15) + (0, 0.30, 0)
   SpriteRenderer sprite = cliff_<DIR>; sortingLayerName="Floor"; sortingOrder = -30 + Mathf.RoundToInt(20f - pos.y). Name each "cliff_<col>_<row>_<DIR>".
5. For each variant 0,1,2: place it, then capture a GAME-view screenshot to `Assets/Screenshots/cliff_gaptest_var{N}_game.png` and a SCENE-view screenshot to `Assets/Screenshots/cliff_gaptest_var{N}_scene.png`. Frame so the whole island + BOTH interior holes + the left(W)/right(E) edges are clearly and FULLY visible, zoomed in enough that the cliffs around EACH hole can be judged individually (not a tiny far shot). Set the in-scene Main Camera position over the island center + orthographicSize to fit, or use scene-view; capture via manage_camera screenshot. Deactivate that variant's sprites before placing the next so screenshots don't mix. ALSO capture one EXTRA tight screenshot per variant centered on HOLE A and one on HOLE B if the wide shot can't show their cliff detail (`Assets/Screenshots/cliff_gaptest_var{N}_holeA.png` / `_holeB.png`).
6. Write report `STAGING/cliff_gaptest_report.md`: removed hole cells; per-variant total cliff count; and per-variant an OVERFLOW heuristic list = cliffs whose sprite renderer bounds.max.y > host cellCenter.y + 0.30 (poking above the floor lip = likely overflow) — give count + up to 8 sample names grouped by DIR. Note which variant has the fewest overflow and looks most "tucked".
7. Leave VAR1 active, SAVE the copy scene `_IsoGame_cliffgaptest`. Confirm the 3 live scenes were never modified.

# Output
Report: copy-scene path, hole cells, screenshot paths (6), per-variant cliff count + overflow count, and your read of which variant tucks best. Do NOT edit live scenes. Do NOT commit.
