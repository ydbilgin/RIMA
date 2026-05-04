# RIMA Large Room Map System - 2026-05-03

## Goal

RIMA rooms should feel closer to Hades / Hades II scale: large combat arenas and broad chambers,
not small test rooms where the camera immediately sees black void around the map.

## Implemented Direction

`Assets/Scripts/Core/LargeDungeonMapPainter.cs`
`Assets/Scripts/Core/RuntimeRoomManager.cs`

The scene now has a runtime painter that can generate multiple large room layouts into the
existing `IsoGrid/Ground` and `IsoGrid/Walls` tilemaps.

Current layout families:
- `GrandArena` - broad opening combat arena.
- `LongGallery` - long east/west chamber for ranged movement.
- `Crossroads` - four-way combat chamber with interior blockers.
- `TwinChambers` - two connected halves with central breaks.
- `SpiralVault` - broken spiral-like wall flow.
- `BossHall` - larger high-readability boss layout.

The system reuses the current Stone Dungeon tile assets already present in the tilemaps. It does
not require generating new PNGs to test map scale.

## Startup Fix

On the first room:
- map is painted before gameplay camera starts
- player is moved to the room center
- camera snaps to player after room paint
- camera reads bounds from the large floor tilemap

This means game start should show playable floor around the character, not mostly black void.

## Scale

Current generated sizes:
- opening/default: around `156x108`
- long gallery: around `190x72`
- crossroads: around `168x112`
- twin chambers: around `178x106`
- spiral vault: around `164x118`
- boss hall: around `100x64`

These are intentionally much bigger than the earlier `32x24` runtime room setting.

## Unity Binding Note

`LargeDungeonMapPainter.cs` is a thin MonoBehaviour wrapper with the correct file/class name for
Unity component binding. The implementation lives in `LargeDungeonMapPainterBase` inside
`RuntimeRoomManager.cs`. This avoids Unity dropping the component on Play Mode because of a
script filename/class mismatch.

## Current Scene Settings

`Assets/Scenes/_IsoGame.unity`

- `Systems` has exactly one `LargeDungeonMapPainter` component.
- Main Camera orthographic size is `3.6`.
- First room moves Player to the generated room center before camera startup.
- Verification screenshot:
  `Assets/Screenshots/debug_game_view_large_map_component_fixed.png`

## Design Notes

The player should not see the whole room at once. Large rooms are for:
- combat kiting space
- projectile readability
- enemy wave staging
- camera staying on character
- wall/void staying outside the immediate view most of the time

Black outside the room is still valid as void/background, but normal gameplay framing should keep
the character surrounded by floor and nearby walls, not empty screen edges.

## Next Art Constraint

This system solves scale and startup framing. The current wall/floor art is still temporary:
- floor tiles still read too raised/overlapping for final
- wall modules still read as chunky block/parapet borders

Final production should replace art while keeping this large-room layout system.
