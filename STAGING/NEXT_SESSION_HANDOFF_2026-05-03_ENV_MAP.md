# Next Session Handoff - Environment / Map / Camera - 2026-05-03

## User Intent

Continue from the RIMA environment/map pass.

User wants:
- Big Hades / Hades II / Last Epoch-like combat maps.
- Not small rectangular test rooms.
- Player should see nearby playable floor and nearby walls, not black void at game start.
- Map should feel natural and composed like:
  `F:/Antigravity Projeler/Pixellab/RIMA_REFS/rima_style_anchor.png`
- Dungeon can have sharp structural parts, but the full map silhouette should not read as a plain rectangle.
- Need many map designs, not one map.
- Lighting needs adjustment.
- Need research/options for map design tools/workflows.

## What Was Implemented This Session

### Scene visibility/camera repair

File:
- `Assets/Scenes/_IsoGame.unity`

Fixed Unity YAML separator breaks caused by earlier tilemap rewrite:
- `e33: 1--- !u!...` before Walls TilemapRenderer
- `e33: 1--- !u!...` before Player GameObject

Result:
- Player loads into Unity hierarchy again.
- Player visible in Play Mode.

Verification screenshots:
- `Assets/Screenshots/debug_game_view_play_after_fix.png`
- `Assets/Screenshots/debug_game_view_play_camera_6_5_settled.png`

### Camera startup/framing

File:
- `Assets/Scripts/Player/CameraFollow.cs`

Changes:
- Added `DefaultExecutionOrder(100)` so room/map paint can happen before camera start.
- Camera now finds Player by tag if no target is assigned.
- Camera can auto-read bounds from `IsoGrid/Ground`.
- Camera snaps to target on `Start()`.
- Camera clamps to floor bounds.

Current scene setting:
- Main Camera `orthographicSize: 3.6`

### Wall occlusion fade

File:
- `Assets/Scripts/Core/WallOcclusionFader.cs`

Scene:
- Attached to `IsoGrid/Walls`.

Behavior:
- Finds Player by tag.
- Fades nearby wall tile cells with runtime `Tilemap.SetColor`.
- Restores tile alpha when Player moves away.
- Does not modify PNGs or tile assets.

Doc:
- `GUIDES/RIMA_CAMERA_AND_WALL_OCCLUSION_SYSTEM_2026-05-03.md`

### Large room map system

Files:
- `Assets/Scripts/Core/RuntimeRoomManager.cs`
- `Assets/Scripts/Core/LargeDungeonMapPainter.cs`

Important implementation detail:
- `LargeDungeonMapPainter.cs` is a thin Unity MonoBehaviour wrapper.
- Implementation lives in `LargeDungeonMapPainterBase` inside `RuntimeRoomManager.cs`.
- This was done because Unity drops MonoBehaviour components when class/file binding is wrong.

Scene:
- `Systems` has exactly one `LargeDungeonMapPainter` component.

Behavior:
- RuntimeRoomManager repaints a large map at room start.
- RuntimeRoomManager updates room size from painter.
- On first room, Player is moved to generated room center before camera startup.

Current layout families:
- GrandArena
- LongGallery
- Crossroads
- TwinChambers
- SpiralVault
- BossHall

Current rough sizes:
- Opening/default: `156x108`
- LongGallery: `190x72`
- Crossroads: `168x112`
- TwinChambers: `178x106`
- SpiralVault: `164x118`
- BossHall: `100x64`

Verification screenshot:
- `Assets/Screenshots/debug_game_view_large_map_component_fixed.png`

Doc:
- `GUIDES/RIMA_LARGE_ROOM_MAP_SYSTEM_2026-05-03.md`

## Current Visual Verdict

The system now fixes the worst startup issue:
- Player starts inside a much larger map.
- Camera starts on Player.
- Game start no longer reads as mostly black void.

But user rejected the current map read:
- Too rectangular / artificial.
- Too much like a generated grid room.
- Does not yet match the natural composed dungeon feel of `rima_style_anchor.png`.
- Current floor tiles still read as overlapping raised slabs.
- Current wall tiles still read as chunky block/parapet border.
- Lighting is not yet art-directed.

## Next Session Should Do

### 1. Stop treating map as rectangle with random blockers

Replace or evolve `LargeDungeonMapPainterBase` generation.

Need map silhouettes like:
- offset chamber shapes
- broken edges
- alcoves
- partial wall runs
- carved-out negative spaces
- side rooms / pockets
- non-symmetric pillar groups
- doorway/threshold zones

Still keep combat-readable center space.

### 2. Add authored room templates

Recommended next direction:
- Keep procedural system, but use authored template masks.
- Create 8-12 room templates first, each with:
  - floor mask
  - wall mask
  - prop zones
  - enemy spawn zones
  - door sockets
  - landmark slots

Possible template names:
- Broken Entry Hall
- Chain Gallery
- Shrine Crossroad
- Crypt Basin
- Pillar Arena
- Split Vault
- Ritual Hall
- Collapsed Library
- Narrow Approach
- Boss Antechamber

### 3. Research map tools

User asked to research useful map design tools/workflows.

Research targets:
- Unity RuleTile / 2D Tilemap Extras
- Tiled Map Editor
- LDtk
- Dungeon Architect for Unity
- DunGen for Unity
- Wave Function Collapse / Wave Function Collapse asset workflows
- Ogmo Editor
- ProBuilder only if needed for blockout, not final 2D tilemaps

Need verdict for RIMA:
- Best likely workflow may be LDtk or Tiled for authored room masks, imported into Unity.
- Unity Tilemap remains runtime renderer.
- PixelLab/MCP should generate art modules, not final whole maps.

### 4. Lighting pass

Add art-directed 2D lights:
- warm torch lights on wall sockets
- cold blue/cyan magic lights
- low ambient dungeon fill
- local pools of light around landmarks
- keep black void outside rooms, but do not frame it at game start

Avoid baking light into every tile.

### 5. Environment art production remains blocked by floor/wall quality

Before final environment:
- regenerate flat `64x32` floor top-surface tiles
- generate proper wall modules:
  - straight wall
  - corner
  - door/arch
  - pillar
  - broken wall
  - wall cap

Current art is acceptable only for system testing.

## Files To Open First Next Session

Read these first:
- `CURRENT_STATUS.md`
- `STAGING/NEXT_SESSION_HANDOFF_2026-05-03_ENV_MAP.md`
- `GUIDES/RIMA_LARGE_ROOM_MAP_SYSTEM_2026-05-03.md`
- `GUIDES/RIMA_ISOMETRIC_ENVIRONMENT_PRODUCTION_FEEDBACK_2026-05-03.md`
- `Assets/Scripts/Core/RuntimeRoomManager.cs`
- `Assets/Scripts/Core/LargeDungeonMapPainter.cs`
- `Assets/Scripts/Player/CameraFollow.cs`
- `Assets/Scripts/Core/WallOcclusionFader.cs`

Reference image:
- `F:/Antigravity Projeler/Pixellab/RIMA_REFS/rima_style_anchor.png`

## Current Verification State

Verified with Unity MCP:
- Player visible.
- Camera follows Player.
- Large map painter active in Play Mode.
- `Systems` has one `LargeDungeonMapPainter`.
- No Unity compile/game errors in console after final refresh.

Not verified:
- Full EditMode/PlayMode test suite was not rerun after large map system.
- Natural map composition not solved yet.
- Lighting pass not implemented yet.
- Map design tool research not done yet.

