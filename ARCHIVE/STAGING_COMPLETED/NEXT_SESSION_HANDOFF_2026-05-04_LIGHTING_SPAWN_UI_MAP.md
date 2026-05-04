# Next Session Handoff - Lighting, Spawn, UI, Map - 2026-05-04

## What Changed

- Removed the stray scene root `RIMA Lighting Preview`.
- Repainted `_IsoGame` with the current `LargeDungeonMapPainter`.
- Replaced over-bright/mixed room lighting with a smaller controlled set:
  - one global ambient fill,
  - one low moon/room fill,
  - two to three local torch/rift/landmark accents depending on layout.
- Added a hard spawn rule through `LargeDungeonMapPainterBase`:
  - `lastPlayableFloorMask` is stored separately from visual floor shell.
  - `GetNearestPlayableFloorPosition(...)` clamps to playable floor only.
  - `IsPlayableWorldPosition(...)` verifies a world position is inside playable floor and not on a wall.
- `RuntimeRoomManager.GetRoomEntrancePosition()` now uses playable floor clamping instead of visual floor clamping.
- Scene default `Player` and `PlayerStartMarker` were moved to a playable center tile and scene was saved.

## User Dissatisfaction To Preserve

These are active product-quality complaints, not solved by calling the pass "done":

- UI is still not trusted by the user. They find current screens visually poor and want actual game-screen scale, not enlarged template pieces.
- Class select, HUD, menu, ESC, reward screens need screenshot-by-screenshot review in the next pass.
- Minimap must be a true simplified version of the active map: correct shape, directions, player/enemy/exit positions, and useful interaction.
- Dungeon rooms must read as human-built RIMA keep/shelter/seal spaces, not only a large floor pattern.
- Lighting must serve room landmarks and ambience. No random point lights, no leftover preview lights.
- Player must never start outside the playable map. This is now a code rule and should remain locked.

## Known Remaining Issues

- Current camera view still tends to show a large floor field around the player. Room walls/landmarks exist, but the camera/framing/layout scale may still need another pass so room architecture is visible in the combat view.
- UI overlay capture through Unity MCP camera does not include Screen Space Overlay UI. Use actual Game View/OS screenshot or PlayMode screen capture for visual QA.
- `MiniMap.cs` still has validation warnings about `GameObject.Find`/runtime work in Update. Functional for now, but should be cached in a cleanup pass.
- `RIMA_MainMenuScreen` bootstrap remains in scene because it was previously needed for menu startup reliability. Re-evaluate if runtime auto-init is now sufficient.

## Next Session Start

1. Open `CURRENT_STATUS.md`.
2. Open this file.
3. Inspect screenshot: `Assets/Screenshots/rima_controlled_lighting_center_spawn_2026_05_04.png`.
4. Run Play Mode and verify:
   - Player starts on playable floor.
   - No `RIMA Lighting Preview` root exists.
   - Light2D count is controlled and only comes from global + procedural room roots.
   - Minimap shape matches the room.
   - UI screens are captured with a method that includes overlay UI.

