ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — single new window, single namespace (4) BLOCKED if archive script unreadable.

NLM ACCESS: If you need RIMA design context:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

Amaç: User wants the old World Painter manual tile-painting back, but SIMPLIFIED — only floor tile features needed for now. The archived `Assets/_archive~/pre_v2_editor/RimaWorldPainterWindow.cs` had Floor/Wall/Prop/Mob/Rooms palettes + biome + RuleTile + scale modes — too much for a focused first version. Strip down to: 4 floor tile theme buttons + variant random + Paint/Erase + brush size + save scene. Object painting + wall painting come LATER as separate iterations.

**Prereq:** Stream O (`bzwg3z9yq`) must have completed — Tile assets at `Assets/ScriptableObjects/Floor/IsoTiles35/tile_<N>.asset` (16 tiles) ready to use.

---

# STREAM P — MINIMAL TILE PAINTER (Direct Floor Variant Brush)

## ⚠️ Phase 0 — Verify prereqs (5 min, MANDATORY)

Check:
- `Assets/ScriptableObjects/Floor/IsoTiles35/tile_0.asset` through `tile_15.asset` exist (Stream O output)
- `Assets/Scenes/Test/PlayableArena.unity` exists with iso Grid + Tilemap (Stream O output)
- `Assets/_archive~/pre_v2_editor/RimaWorldPainterWindow.cs` readable (1100+ lines, archived)

If any missing → write WAITING status, do not proceed.

## Phase 1 — Theme classification (10 min)

Inspect 16 tiles visually (read PNG headers via UnityMCP `manage_asset` info, or just trust the b340684f description order):

**Expected theme mapping** (based on b340684f generation prompt order):
- **Theme A — Cobblestone (base)**: tile_0, tile_1, tile_2, tile_3
- **Theme B — Cyan Veins (accent)**: tile_4, tile_5, tile_6, tile_7
- **Theme C — Dirt (variation)**: tile_8, tile_9, tile_10, tile_11
- **Theme D — Ritual Rune (focal accent)**: tile_12, tile_13, tile_14, tile_15

(Verify via visual inspection of tile_0/4/8/12 — orchestrator already confirmed: tile_0 cobblestone, tile_4 cyan, tile_8 dirt, tile_12 ritual. Confirmed pattern.)

Document tile → theme mapping in CODEX_DONE.

## Phase 2 — Create MINIMAL window (45-60 min)

### 2A. New file
`Assets/Scripts/Editor/MapDesigner/MinimalTilePainter.cs`

**Namespace:** `RIMA.Editor.MapDesigner`
**Class:** `MinimalTilePainter` (extends `EditorWindow`)
**Menu:** `RIMA > Tile Painter (Minimal)`

### 2B. UI layout (single column, simple)

```
[ Tile Painter (Minimal) ]

Active Tilemap: [ <ObjectField for Tilemap> ]   (auto-find first iso Tilemap on scene start)

Theme:
  ( ) Cobblestone (base, 4 var)
  ( ) Cyan Veins (accent, 4 var)
  ( ) Dirt (variation, 4 var)
  ( ) Ritual Rune (focal, 4 var)
  [ Refresh tile assets ]

[x] Random variant within theme (if unchecked: use first tile in theme)

Tool:
  ( ) Paint     ( ) Erase

Brush size: [1] [2] [3]   (radio)

[ Status: SceneView click to paint ]

(optional) [ Save Scene ]
```

### 2C. Functionality scope

**MUST HAVE:**
- Auto-load 16 tile assets from `Assets/ScriptableObjects/Floor/IsoTiles35/` on window open + Refresh button
- Theme button selects that theme's 4 tiles
- Paint mode: click cell in SceneView → SetTile on active Tilemap with selected tile (random within theme if checkbox on, else tile_0/4/8/12 depending on theme)
- Erase mode: click cell → SetTile(pos, null)
- Brush size: paint NxN cells (1x1, 2x2, 3x3) around hover cell
- SceneView hover cell preview (gizmo Wire diamond at iso cell)
- Undo support via `Undo.RegisterCompleteObjectUndo(tilemap, "Paint Tile")`

**DO NOT INCLUDE (defer):**
- ❌ Wall palette
- ❌ Prop/Mob/Rooms palette
- ❌ Biome preset
- ❌ RuleTile / auto-connect walls
- ❌ Rotation 0/90/180/270 (floor tiles don't need it for now)
- ❌ Custom collider modes
- ❌ Per-category scale
- ❌ Position offset
- ❌ TopDown vs Isometric toggle (locked iso)
- ❌ Eyedropper tool
- ❌ Multi-select tile palette (just theme button)
- ❌ Custom asset add/exclude (use folder scan only)

Keep code ≤300 lines if possible.

### 2D. SceneView integration

Subscribe to `SceneView.duringSceneGui += OnSceneGUI`. In handler:
- Compute mouse world position via `HandleUtility.GUIPointToWorldRay`
- Convert world → cell via `tilemap.WorldToCell()`
- Draw Wire Diamond at cell center (Gizmos cyan)
- On Event.current.type == MouseDown OR MouseDrag + button 0:
  - Apply tile to cell (brush size NxN around)
  - Use Event.current.Use() to consume
- Repaint scene view

### 2E. Compile + verify
- `mcp__UnityMCP__validate_script` on new file
- `mcp__UnityMCP__read_console` → 0 errors

## Phase 3 — Test in editor (10 min)

1. Open `Assets/Scenes/Test/PlayableArena.unity`
2. `RIMA > Tile Painter (Minimal)`
3. Click Cobblestone theme → status shows "4 tiles loaded"
4. SceneView click a few cells → tiles paint
5. Switch to Cyan Veins → click → cyan tile paints
6. Erase mode → click → cell cleared
7. Brush size 3 → 3×3 block paints
8. Ctrl+Z → last paint undoes (proves Undo working)

Take screenshot of editor window in use: `STAGING/s106_overnight/tile_painter_minimal_v1.png` (1280×720)

## Phase 4 — Report

```
# STREAM P - MINIMAL TILE PAINTER - <profile> - 2026-05-25 <time>

## STATUS: DONE | PARTIAL | FAILED

## Phase 0 — Prereqs
- IsoTiles35 folder: y (16 .asset files)
- PlayableArena.unity: y
- Archive RimaWorldPainterWindow.cs readable: y (only referenced, not modified)

## Phase 1 — Theme mapping
- A Cobblestone: tile_0..3
- B Cyan: tile_4..7
- C Dirt: tile_8..11
- D Ritual: tile_12..15
- Visual verification: y/n

## Phase 2 — Window file
- Path: Assets/Scripts/Editor/MapDesigner/MinimalTilePainter.cs
- Line count: N (target ≤300)
- Menu item: RIMA > Tile Painter (Minimal)
- Features in scope: 4 themes, Random toggle, Paint/Erase, brush 1-3, Undo, hover preview
- Features DEFERRED (intentionally absent): walls, props, mobs, biome, RuleTile, rotation, scale modes

## Phase 3 — Editor test
- Paint single cell: y/n
- Paint 3×3 brush: y/n
- Erase: y/n
- Theme switch repaints with new tile: y/n
- Ctrl+Z undo works: y/n
- Screenshot: STAGING/s106_overnight/tile_painter_minimal_v1.png

## Compile check
- 0 errors, 0 warnings

## Time: N min
```

## Safety constraints (HARD)
- ❌ Don't modify V2 RoomPainterWindow.cs (different tool, different purpose)
- ❌ Don't restore old RimaWorldPainterWindow.cs from archive — write NEW minimal version
- ❌ Don't modify any Player/Player.prefab/scene file
- ❌ No wall/prop/mob features in this stream — DEFER
- ✅ Single new file, single namespace
- ✅ Use existing 16 IsoTiles35 .asset files (don't create new)
- ✅ Test in editor + screenshot proof

## Estimated total: 60-80 min
