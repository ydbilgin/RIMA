# RIMA Camera and Wall Occlusion System - 2026-05-03

## Goal

Small or dense isometric dungeon rooms must stay readable. The player should not disappear
behind wall borders, and the game should not open with the camera looking at empty black space.

## Implemented

### Camera startup/framing

`Assets/Scripts/Player/CameraFollow.cs`

- Finds the Player by tag if no target is assigned.
- Reads room bounds from `IsoGrid/Ground` when enabled.
- Snaps the camera to the player on `Start()` before the first follow lerp.
- Clamps camera movement inside the current floor renderer bounds.

This prevents the old startup behavior where the camera began at its serialized scene position
and only later lerped toward the player, briefly showing the wrong framing or large black areas.

### Wall transparency

`Assets/Scripts/Core/WallOcclusionFader.cs`

- Attach to the `IsoGrid/Walls` tilemap.
- Finds the Player by tag.
- Fades only nearby wall cells using `Tilemap.SetColor`.
- Restores cell alpha when the player moves away or the component disables.
- Uses runtime tile colors only; it does not modify tile assets or generated PNG files.

Recommended baseline values:
- `fadeRadius`: `2.2`
- `minimumAlpha`: `0.38`
- `fadeSpeed`: `10`
- `cellSearchRadius`: `5`
- `verticalDistanceWeight`: `0.75`

## Design Decision

Use Unity-side occlusion, not baked-transparent wall sprites.

Reason:
- Transparency depends on player position.
- Different rooms can be tighter or wider.
- Enemies and bosses may need different readability rules later.
- Runtime tile color fading keeps PixelLab wall art reusable.

## Remaining Art Constraint

This solves readability, not final wall art quality. The current wall sprites are still a temporary
test-room set and read as block/parapet borders. Final wall production should still generate proper
straight/corner/pillar/door wall modules.

