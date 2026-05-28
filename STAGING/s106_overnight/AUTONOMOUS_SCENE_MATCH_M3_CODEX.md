# Autonomous Scene Composition to Match Reference M3 — Codex (xhigh)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: PlayableArena sahnesi şu ANDA Hades Elysium tarzı referansa benzemiyor — floor ÇOK BÜYÜK (2533 tile, 97×59 cells), Kit B cliffs floor'un ORTASINDA scattered, BG kapanmış. User otonom düzelt diyor — agy + codex review loop ile referans M3 mockup'a benzet. Bu DISPATCH 1 cycle (impl); sonra agy review gelecek, gerekirse iterate.

## REFERENCE TARGET
- **`STAGING/s106_overnight/walless_v1_batch2_M3.png`** ⭐ visual target — read it first
- Compact floating diamond/hexagonal arena (~10-12 cells wide × 6-8 cells tall)
- Center: cyan rune focal circle (3-5 rune tiles tight cluster)
- 4 brazier corners (warm orange light pools)
- Cliff face hangs DOWN around the entire perimeter
- Purple-cyan nebula BG visible BEYOND the arena edge (Kit C layers)
- Lightning streaks at outer edges (Kit C nebula contribution)
- Sense of "floating in void"

## CURRENT STATE (read scene first)
- Scene: `Assets/Scenes/Test/PlayableArena.unity` (LIVE, was modified by Opus earlier)
- Existing Tilemap "Floor/Tilemap": **2533 painted cells**, x=-22..75 y=-35..24 — TOO BIG, no perimeter
- CliffRing GameObject: 24 children, currently scattered at floor center (math was for 12×8 arena, applied at painted center 15.5/6.7) — BROKEN placement
- RoomBackgroundRig GameObject: 6 children (L0-L4 Kit C BG layers), recently re-orderd Ground/-800..-500
- Player at (15.50, 6.70, 0), Camera at same XY ortho 3.5
- Sorting:
  - Player: Characters/10
  - Floor: Floor/0
  - Cliff (Kit B): Ground/-50
  - BG (Kit C): Ground/-800..-500
- Pixel Perfect Camera DISABLED (user requested off)

## INPUTS

### Kit A floor tiles (16 in `Assets/ScriptableObjects/Floor/IsoTiles35/`)
- tile_0..3 = Cobblestone (stone base)
- tile_4..6 = Cyan veins (accent)
- tile_7..10 = Dirt
- tile_11..15 = Ritual rune (focal)

### Kit B cliff sprites (already in `Assets/Sprites/Environment/KitB_Cliff/`)
- cliff_{N,S,E,W,NE,NW,SE,SW,cyan_glow}.png — 128×192 pixel art

### Kit C BG layers (already in `Assets/Sprites/Environment/KitC_BG/`)
- bg_{L0_void, L1_nebula, L2_ruins_A, L2_ruins_B, L3_island_small, L3_island_large, L4_fog}.png

### Existing lights
- Warm Torch Light NW, NE (orange)
- Cyan Crystal Light SW, SE (cyan, has LightPulse)
- Global Light 2D
- All `Light2D` components

## PRODUCTION SPEC

### Phase 0 — Read reference and current state
1. Read `STAGING/s106_overnight/walless_v1_batch2_M3.png` carefully
2. Open PlayableArena scene
3. Note: 2533 painted tiles, irregular extent, cliff/BG placed wrong

### Phase 1 — Clear and re-paint a compact arena
1. **Clear current Tilemap** (`tilemap.ClearAllTiles()`)
2. **Paint a NEW floating-island floor shape:**
   - Centered at world origin (0, 0) — easier math
   - Shape: **compact diamond / hexagon**, ~12 cells wide × 7 cells tall in iso cell coords
   - Use formula: paint cell (x, y) if `Mathf.Abs(x) / 6f + Mathf.Abs(y) / 3.5f < 1f` (oval mask)
   - **Tile composition (per existing `TILE_CATEGORIZATION.md`):**
     - **Center 3×3 cells:** Ritual Rune (tile_11..15) — focal cluster, 5-9 cells max
     - **Inner ring (next 2 cell rings around center):** Cyan Veins (tile_4..6) — accent
     - **Outer area (rest):** Cobblestone (tile_0..3) — base
     - NO dirt this composition (cleaner Hades-feel)
   - Use `UnityEngine.Random.Range` with fixed seed for reproducibility
3. Save scene

### Phase 2 — Move Player + camera + rigs to origin
1. Player position → (0, 0, 0)
2. Camera position → (0, -0.5, -10), orthographic size 3.5
3. CameraFollow.target → Player (use SetTarget if available; the active CameraFollow is `RIMA.CameraSystem.CameraFollow`)
4. RoomBackgroundRig position → (0, 0, 0)
5. CliffRing position → (0, 0, 0)
6. Lights: place at proper corners (Warm Torch Light NW ~(-4, 2), NE ~(4, 2), Cyan Crystal Light SW ~(-4, -2), SE ~(4, -2)) — adjust if needed for matching reference

### Phase 3 — Re-place CliffRing around the new compact arena
1. Use the painted floor cell extent (after Phase 1) to compute perimeter
2. South edge: 4-6 cliff_S sprites along the southern rim (y just below floor bottom rim in world coords)
3. North edge: 4-6 cliff_N at northern rim
4. East edge: 2-4 cliff_E
5. West edge: 2-4 cliff_W
6. Corners: cliff_NE, NW, SE, SW (one each)
7. Each cliff:
   - SpriteRenderer sortingLayer = "Ground", sortingOrder = -50
   - Pivot = top-center (set in importer) → sprite hangs DOWN from its world position
   - Position the top of the sprite at the floor's rim edge so cliff hangs into void below

### Phase 4 — Kit C BG layers final placement
- L0_Void: position (0, 0, 30) — large fill BG
- L1_Nebula: (0, 2, 25) — slight upward offset for top nebula
- L2_Ruins_A: (0, 5, 20) — distant horizon
- L3_Island_Small: (-7, 4, 15) scale (0.4, 0.4, 1)
- L3_Island_Large: (8, 5, 15) scale (0.6, 0.6, 1)
- L4_Fog: (0, -2, 10) — masks cliff bottoms

### Phase 5 — Brazier lights at brazier corners
- Position warm orange brazier lights at the 4 corners of the painted arena, at world Y offset to be visible
- Intensity ~1.5, falloff small (1-1.5 unit radius)
- Cyan Crystal Light pulses at outer brazier
- Make Global Light 2D dim (~0.3 ambient)

### Phase 6 — Screenshot + done report
1. Save scene
2. Take Game View screenshot at 1280×720 → `STAGING/s106_overnight/scene_v2_match_attempt.png`
3. Take Scene View screenshot framed on entire scene → `STAGING/s106_overnight/scene_v2_match_attempt_scene.png`
4. Side-by-side comparison composite (your impl + M3 reference) → `STAGING/s106_overnight/scene_v2_vs_M3.png`
5. Write report `STAGING/s106_overnight/SCENE_V2_REPORT.md`:
   - What you did per phase
   - Console errors/warnings (must be 0)
   - Honest self-assessment vs reference M3: which elements match, which don't
   - Suggested next iteration if not close enough

## CONSTRAINTS
- **NO PixelLab calls** (user halted)
- **NO Pixel Perfect Camera re-enable** (user disabled, leave off)
- **NO new dependencies / new scripts** (use existing Kit A/B/C + lights)
- **0 error 0 warning required**
- Single scene save at end (don't repeatedly save during)
- Preserve Player WASD movement + combat components

## TIME ESTIMATE
~60-90 min at xhigh.

## DELIVERABLE
- PlayableArena.unity modified per spec
- 3 screenshots
- 1 report markdown
- Final `CODEX_DONE_<profile>.md` with `STATUS: DONE`

After this DONE: Opus will dispatch agy review on `scene_v2_vs_M3.png` for visual verdict. If close enough → green-light. If not → iterate with refined spec.
