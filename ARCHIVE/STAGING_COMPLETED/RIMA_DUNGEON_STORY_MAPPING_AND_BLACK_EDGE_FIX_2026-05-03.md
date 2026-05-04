# RIMA Dungeon Story Mapping and Black Edge Fix - 2026-05-03

## User Feedback

The dungeon maps must be meaningful, connected to RIMA's story, and visually closer to the
`rima_anchor_style` direction. The camera must not reveal black emptiness outside the map.

## Decision

RIMA rooms should not be random rectangles. They should follow a narrative descent:

1. Threshold / Entry ruin
   - BrokenEntryHall
   - ChainGallery
   - NarrowApproach

2. Ossuary / Memory crypt
   - CryptBasin
   - ForkedOssuary
   - ReliquaryLoop

3. Sanctum / Ritual architecture
   - ShrineCrossroad
   - RitualHall
   - CrescentSanctum

4. Rift / Broken reality
   - SplitVault
   - AmbushCloister
   - RiftWell
   - PillarArena

Special room mapping:
- Chest -> ReliquaryLoop
- Merchant -> BrokenEntryHall
- Forge -> BrokenCauseway
- Event -> CrescentSanctum or RiftWell
- Elite -> AmbushCloister or ForkedOssuary
- Boss -> BossAntechamber

This makes room order read as story progression instead of a shuffled template list.

## Black Edge Attempt and Correction

File:
- `Assets/Scripts/Core/RuntimeRoomManager.cs`

Changes:
- Default opening map size increased from `156x108` to `220x150`.
- Explicit room sizes increased across all layouts.
- Main Camera background changed from pure black to a dark stone-blue fallback:
  `RGBA(0.055, 0.065, 0.075, 1.000)`.

Correction after user reported Unity crash:
- The large `cameraSafetyFloorPadding = 80` floor apron was too brute-force and likely unsafe.
- It produced about `117800` floor tiles in the scene smoke.
- `cameraSafetyFloorPadding` is now default `0` with a hard runtime cap of `16`.
- Do not use giant tile floods to hide black edges.
- Correct direction is Hades-like staging: authored high perimeter walls / foreground occluders,
  camera clamp, dark backdrop, and only small overscan where needed.
- Unity closed before post-correction script validation could run through MCP.
- Scene size check after the correction: `_IsoGame.unity` is about `586 KB`, so the huge test
  tilemap was not left as a multi-MB serialized scene bloat.

Verification screenshot:
- `Assets/Screenshots/debug_after_narrative_map_player_center_no_black_2026_05_03.png`

Result:
- The screenshot proved the black edge can be hidden, but the implementation was not acceptable
  as a production fix.
- Current art still needs real modular wall/height pieces before judging final dungeon quality.

## Tooling Check

Local Unity package check:
- `com.unity.2d.tilemap.extras` is installed at version `6.0.1`.

Use it for:
- Isometric RuleTile wall/corner/floor rules
- Rule Override Tile variants per biome
- RandomTile-style visual breakup

External authoring tools to use next:
- LDtk first candidate:
  - Entity layers and custom fields can store spawn zones, light radius, sockets, and metadata.
  - LDtkToUnity separate level files are useful for modular dungeon pieces.
  - Sources:
    - https://ldtk.io/docs/general/editor-components/entities/
    - https://cammin.github.io/LDtkToUnity/documentation/Importer/topic_LevelImporter.html
- Tiled second candidate:
  - Object layers and custom properties are suitable for spawn zones, collision polygons, light
    points, prop zones, and room metadata.
  - Sources:
    - https://doc.mapeditor.org/en/stable/manual/layers/
    - https://doc.mapeditor.org/manual/custom-properties/
- Unity 2D Tilemap Extras:
  - Rule Tiles are for terrain/pipeline/random/animated tile behavior, including isometric grids.
  - Source:
    - https://docs.unity.cn/Packages/com.unity.2d.tilemap.extras%403.1/manual/index.html

## Next Production Step

Do not add more random procedural templates until room meaning is locked.

Next pass should define a small authored room schema:
- `RoomId`
- `NarrativeBand`
- `RoomType`
- `FloorMask`
- `WallMask`
- `DoorSockets`
- `EnemySpawnZones`
- `LightSockets`
- `PropZones`
- `LandmarkSlots`

Then convert either LDtk or Tiled authored data into the existing Unity Tilemap painter.
