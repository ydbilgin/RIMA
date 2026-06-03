ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — ONE room only (4) BLOCKED if Phase 0 incomplete.

NLM ACCESS: If you need RIMA design context:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

Amaç: USER feedback (2026-05-25): "bana gerçek bi oda ver mantığını anladıktan sonra yeni odaları yaparız zaten". Stream F çıktısında 3 critical visual bug var (sprite scale patlamış, floor invisible, wall chain gaps). Bu task TEK ODA üzerinde (Combat Basic) odaklı fix — chatgpt_ref kalitesine 7.5+/10 yaklaşacak şekilde. Diğer 4 odaya DOKUNMA.

---

# STREAM G — COMBAT BASIC PROOF ROOM (focused single-room fix)

## ⚠️ Phase 0 — INTERNALIZE chatgpt_ref INTENT (MANDATORY FIRST, no skipping)

Read this carefully BEFORE touching any code. Open these PNGs and study them:
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (1).png`
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (2).png`
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (3).png`
- `STAGING/concepts/chatgpt_ref/blueprint_room/ChatGPT Image 24 May 2026 23_42_09 (1).png`

Write 300-500 words at the TOP of your CODEX_DONE response describing what you see:
- Camera angle (degrees from horizontal)
- Floor coverage (what % of frame is floor visible?)
- Wall scale ratio (how big are walls relative to floor?)
- Wall continuity (gaps allowed? where?)
- Lighting cones (where do torches sit? color temp?)
- Pixel detail level (brick texture, seams, ornaments)

THEN compare to current `STAGING/s106_overnight/stream_e_rooms/combat_basic/scene_v2.png` and identify the TOP 5 specific visual deltas with pixel-level reasoning.

If Phase 0 is shorter than 300 words OR doesn't cite specific PNGs you opened → invalid output, dispatch will be rejected.

---

## Phase 1 — Investigate ROOT CAUSE of sprite scale bug (10 min)

The current `wpd_rear_wall_2x_real.asset` likely has wrong `footprintSize` × Sprite PPU calculation. Walls render at ~4× intended size.

Investigation:
1. Read `Assets/ScriptableObjects/Walls/V2/wpd_rear_wall_2x_real.asset` and note `footprintSize`
2. Read the sprite asset import settings: `Assets/Sprites/AssetPackV3/walls/sheet_2/piece_01.png.meta` and find PPU
3. Calculate: expected world size = footprintSize (cells) × cellSize (1 unit). Actual world size = sprite pixels / PPU.
4. Verify the math: if `footprintSize.x=2` and sprite is 256px wide, PPU should be 128 (so 256/128 = 2 units = 2 cells). If PPU=64 (default), sprite renders at 4 units wide, which is 2× too big.

Document findings in CODEX_DONE.

### Fix options (pick ONE)

**Option A (recommended — affects only the 4 _real sprites):**
Re-import the 4 real sprite PNGs with custom PPU so that natural sprite size = footprintSize in cells. Update `.meta` files for:
- sheet_2/piece_01.png → PPU 128 (256px / 2 cells)
- sheet_2/piece_06.png → PPU 128 (side wall 2x)
- sheet_4/piece_02.png → PPU 128 (1 cell, sprite likely 128px → PPU 128 means 1 unit = 1 cell)
- sheet_3/cell_01_R0C0.png → PPU 128 (door arch 2x, 256px → 2 cells)

Use UnityMCP `manage_asset` or direct .meta edit + AssetDatabase.Refresh.

**Option B (lighter touch — overrides per-prefab):**
In each `wp_*_real.prefab` Visual SpriteRenderer, set `drawMode` to Sliced or fixed size matching footprintSize. Adjust Visual.transform.localScale to compensate for sprite native size.

**Option C (data fix — change WallPiece.ApplyMetadata):**
Modify `WallPiece.cs` to use `data.footprintSize / (sprite.bounds.size)` for visual.transform.localScale. So visual ends up exactly `footprintSize` cells regardless of sprite native PPU.

Recommend **Option C** — fix once in code, all current + future _real assets benefit. Don't touch sprite imports.

### Verify after fix
Spawn a single test rear_wall_2x_real prefab in an empty scene at (0,0). Confirm it renders at exactly 2 units wide × 1 unit tall (not 4×3). UnityMCP can use `find_gameobjects` + `unity_reflect` to verify Transform/Renderer bounds.

---

## Phase 2 — Fix floor tilemap visibility (15 min)

Stream F claimed "Floor Tilemap" exists but in all 5 scene_v2.png images the floor is completely INVISIBLE (just dark background).

Investigation steps:
1. Open `Assets/Scenes/Test/PainterTestE_combat_basic.unity`
2. Find the Floor GameObject (Codex Stream F said it's a child of room)
3. Check:
   - Tilemap component has TileBase assignments (not empty)
   - Tilemap Renderer sortingLayer + sortingOrder (likely needs lower order than walls to be behind, but visible)
   - Tilemap GameObject position z (must be in camera frustum range — ortho z=0 to z=10)
   - URP 2D Renderer is active for this scene (Graphics Settings + Project Settings)
   - Floor tile material/shader (URP 2D Sprite material expected)

### Fix
Most likely: tilemap was created but tiles were not assigned. Re-fill with sheet_3/4 floor tiles via UnityMCP `manage_tilemap` or scripted fill:
```csharp
for each walkable cell in RoomSpec.walkableCells:
    var tile = floorTilesArray[Random.Range(0, floorTilesArray.Length)];
    tilemap.SetTile(new Vector3Int(cell.x, cell.y, 0), tile);
```

Use floor tiles from `Assets/Sprites/AssetPackV3/floor/tile_0..15.png` — create Tile assets if not present (`AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<Tile>())`).

Set Tilemap GameObject z = 1 (behind walls at z=0), sortingLayerName matches default.

### Verify
After fix, the combat_basic scene's walkable area should show floor texture filling the inside of the wall perimeter.

---

## Phase 3 — Fix wall chain continuity in Combat Basic (10 min)

Combat Basic scene_v2 shows visible GAPS between top wall and side walls (corner pieces missing/disconnected). Other rooms have worse issues but this is a 14×12 rectangle so simplest to fix.

Investigation:
1. Open the scene, count actual WallPiece GameObjects under PaintedRoom_combat_basic
2. Compare to expected: 14×12 rectangle perimeter = 2×(14+12)-4 = 48 cells. Each cell needs a wall piece (or part of a multi-cell wall) UNLESS it's a low-front cell.
3. Codex Stream F report said real_piece counts were 17 (combat) — wildly insufficient. Find why other pieces didn't spawn.

### Hypothesis: WallChainRoomBuilder preferReal logic returning null
The `preferReal=true` may be requesting `_real` variant for piece types that don't have one (e.g. `Connector`, `OuterCorner`), and `realBest ?? best` should fall back to placeholder but maybe doesn't if `realBest` is `null` while `best` is also null due to filter issue.

### Fix
Read `WallChainRoomBuilder.cs:806` (preferReal wiring). Make absolutely sure:
- When `preferReal=true` and `_real` variant doesn't exist, builder falls back to placeholder piece, NOT null/skip
- For corner/connector/door pieces (which mostly have only placeholder), the fallback path works

Add Debug.Log statements temporarily to trace which pieces are being skipped vs spawned. Remove logs after fix verified.

### Verify
After fix, combat_basic should have ~40-48 wall pieces spawned (corners + chain + low front). No visible gaps.

---

## Phase 4 — Re-render ONE room ONLY (5 min)

After Phase 1-3 fixes:
1. Open `Assets/Scenes/Test/PainterTestE_combat_basic.unity`
2. Adjust camera ortho size to frame the room cleanly (~9-10 for 14×12 rect with 1-cell margin)
3. Take screenshot to `STAGING/s106_overnight/stream_e_rooms/combat_basic/scene_v3.png` (NEW name, don't overwrite v2)
4. Take gizmo-on version to `combat_basic/gizmo_v3.png`

### Comparison shot
Also save side-by-side comparison: `STAGING/s106_overnight/stream_e_rooms/combat_basic/comparison_v3.png` — chatgpt_ref left, scene_v3 right, similar zoom.

---

## Output

Write to `CODEX_DONE_<profile>.md`:

```
# STREAM G - COMBAT PROOF - <profile> - 2026-05-25 <time>

## Phase 0 — chatgpt_ref intent (300-500 words)
<your description>

## Phase 0 — Top 5 visual deltas vs scene_v2
1. ...

## Phase 1 — Sprite scale root cause
<findings + fix option chosen + diff>

## Phase 2 — Floor visibility fix
<root cause + fix + verification>

## Phase 3 — Wall chain continuity
<piece count before/after + fix>

## Phase 4 — Re-render
- scene_v3.png path
- gizmo_v3.png path
- comparison_v3.png path
- Estimated chatgpt_ref likeness: N/10

## STATUS: DONE | PARTIAL | FAILED

## Compile check
- 0 errors / 0 warnings

## Time: N min
```

## Safety constraints (HARD)
- ❌ DO NOT touch the other 4 PainterTestE scenes
- ❌ DO NOT modify the 14 placeholder prefabs
- ❌ DO NOT create new wpd_*_real assets
- ❌ NO Unity crash — AssetDatabase batch + try/finally + scene save
- ✅ ONE room (Combat Basic) ONLY
- ✅ Single AssetDatabase.Refresh at end

## Estimated time: 40-50 min total
