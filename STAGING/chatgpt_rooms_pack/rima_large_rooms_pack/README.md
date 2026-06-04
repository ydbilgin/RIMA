# RIMA - The Shattered Keep: Large Room Pack

This zip contains 15 large isometric floating-island room layouts for RIMA.

## Contents

- `rooms/rima_shattered_keep_rooms_large.json`
  - Main room data. Use this in Claude, Codex, Unity tools, or your own importer.
- `docs/ROOM_PREVIEW.md`
  - Human-readable ASCII previews for every room.
- `docs/CLAUDE_IMPORT_PROMPT.md`
  - Prompt to paste into Claude together with the JSON.
- `schema/room_schema.json`
  - Lightweight schema reference.
- `tools/validate_rooms.py`
  - Python validator for row width, connectivity, one P spawn, door rules, and room counts.

## Design Target

Biome: `The Shattered Keep`

Rooms are floating stone islands in a void. The grid defines only walkable floor shape. Cliff/void visuals should be generated later by the map tool or Unity importer.

Shape variety matters more than rectangles. This pack includes:
diamond, cross, L-shape, bridge-lobes, hourglass, donut, teardrop, organic blob, twin basin, trident, crescent, boss oval, chest vaults, and a zigzag bridge corridor.

## Symbol Legend

```text
. = walkable floor
P = player entry spawn, exactly 1
e = enemy spawn
C = chest spot
B = boss spawn
  = void
```

Doors are stored separately in `doors`, not drawn into the ASCII grid.

## Canon Door Rule

Doors may face only:

- `N`
- `E`
- `W`

Never add `S` doors. The south side is reserved for composition/readability and cliff falloff.
