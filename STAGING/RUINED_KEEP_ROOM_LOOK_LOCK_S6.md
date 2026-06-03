# RIMA Ruined-Keep Room Look — LOCK (Opus, ax+cx converged 2026-05-31)

**Target:** `STAGING/concepts/chatgpt_ref/` (walled semi-enclosed broken-masonry top-down 3/4 rooms) blended with `STAGING/floor_perspective_concepts/03_wallless_improved.png` (floating-island cyan-rim edge). = canon **Ruined Keep hybrid**.

## CAMERA (depth via closeness) — ax+cx CONVERGE
- Stay **flat 2D Orthographic**, rotation (0,0,0). **NO perspective tilt** (both: distorts cursor-aim, sprite scale, Y-sort). Depth comes from 2.5D art + Y-sort, not the camera.
- **Zoom lever = PixelPerfectCamera.refResolution, NOT orthographicSize** (PPC overrides orthoSize). Keep assetsPPU=64, upscaleRT=on.
- **Zoom: 640×360 → 480×270** (orthoSize~3.0 equiv, 1.33× closer, **integer 4× at 1080p = crisp**). cx's gentler 576×324 (1.11×) is the fallback if combat telegraphs feel cramped. **EMPIRICAL: try 480×270, screenshot-verify telegraph room, tune toward 576×324 if too tight.**

## WALLS / PILLARS (top-down 3/4) — cx specifics
- Tall broken-wall sprites with **visible front face** (like 128×192 cliffs). Module sizes @64PPU: **128×160 / 128×192** (major walls), **64×128** (pillars), **64×64–96×96** (rubble caps).
- **Pivot = bottom-center** (ground contact). Same Y-sorted layer as entities (Custom Axis (0,1,0), SpriteRenderer Sort Point = Pivot, SortingGroup for multi-sprite wall assemblies). Floor sorts below.
- **Corner pillars (1.5×4 cells / 64×128px) = modular anchors** hiding seams + defining perimeter.
- **N/back = tall** (walls/arch/portal/banners/cyan-cracks) · **E/W = mixed walls + broken void gaps** · **S/front = LOW crumbled parapet / open void** (never block camera/player).

## COLLIDERS
- Walkable collider = **inner floor polygon** (NOT wall art outline).
- Wall base sits 0.25–0.5u OUTSIDE walkable; solid collision line at inner foot (~0.25u inside visible base).
- Columns = small foot-collider only (0.4–0.7u); height is purely visual.
- Open void gaps = edge/void colliders (edge-stop, no fall — demo). [[feedback_void_blocker_collider_edge_stop_s6]]

## EDGES / HYBRID RATIO
- **60–75% solid broken masonry + 25–40% open void edge** per room (fortress silhouette + floating-island danger).
- **Cyan rim-light ONLY on open void edges + floor cracks near drops** (sparing). Small cyan cracks under solid walls, not strong rim.
- Gameplay collider 0.25–0.5u inside the visible crumbling edge. Hanging chains / vertical cyan glow / falling-rock silhouettes OUTSIDE collider sell depth.

## FLOOR
- Dark indigo/slate **#0d0d12–#161622**, void darker **#020204** (strong floor silhouette). Current placeholder too dark → lift slightly.
- **Torch amber light-pools** (#ff9a24→#4b2800 falloff) under wall torches break monotony. **Center cyan engraved runes** (pulse), fade toward edges. Slight vertical foreshortening.

## PLACEMENT ALGORITHM (logical, not scatter)
1. Mark perimeter segments: **SolidWall / VoidEdge / Entrance / BrokenGap**.
2. Major walls + corner pillars on SolidWall first; then loose props.
3. **Entrances = clean 3–5 tile gaps framed by pillars** (N-center = arched cyan gate-portal).
4. Torches/banners every 4–6u on wall segments (near walls, NOT combat center).
5. Center = focal rift-altar/dais + rune circle.

## DEPTH WITHOUT WALLS
Oval contact shadows (#000 ~40%, 0.25–0.75u deep) under every char/prop · tall props 2–3 cells · drifting dust particles · parallax dark purple/blue void bg.

## PALETTE (3-color discipline)
Dark indigo slate (floor/walls) + amber (torches) + cyan (runes/portals/rift). **NO** mid-greys, primary red, sunlit green.

## OPUS DECISIONS on 4 open Qs (user can override)
1. Zoom = **480×270** first (dramatic), verify, fallback 576×324.
2. Void edge = **solid edge-stop** (no fall) for demo.
3. Pillars = **dense perimeter (mockup) + S open + 1–2 gaps**.
4. Cyan runes = **yes, subtle pulse** (react to class resource).

## BUILD PLAN (phased, screenshot-verify each)
1. Camera refResolution 480×270 (verify framing).
2. Rectangular floor migration (grid + 64px tiles + footprint as inner polygon).
3. Cliff iso→ortho vector migration (cx audit `CODEX_DONE.md:4259`) — riskiest, one slice at a time.
4. Greybox perimeter: SolidWall/Void/Entrance segments → walls + corner pillars + N gate + center altar.
5. Edges: cyan rim on void, torch light-pools, contact shadows, dust.
6. Art polish via PixelLab (walls/pillars/altar) — placeholders logged in `IMAGEGEN_PLACEHOLDER_REGISTRY.md`.
DO NOT touch character art.
