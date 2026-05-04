# RIMA Map Production Direction and Tool Research - 2026-05-03

## User Feedback Captured

The current large runtime maps work technically, but the visual read is still wrong when the
room is treated as one large rectangle. RIMA maps should keep a readable combat core, but the
overall silhouette must feel composed and natural, closer to:

`F:/Antigravity Projeler/Pixellab/RIMA_REFS/rima_style_anchor.png`

Dungeon architecture may still have sharp wall runs, hard corners, and built structures. The
full room outline should not read as a plain generated rectangle.

## Production Rule

Do not produce final rooms as one baked image.

Use modular runtime assembly:
- floor mask
- wall/border mask
- door sockets
- light sockets
- prop zones
- enemy spawn zones
- landmark slots

Unity Tilemap remains the renderer. PixelLab/MCP should produce reusable floor, wall, prop,
pillar, rubble, shrine, arch, and light-source modules, not whole final rooms.

## Current Code Pass

File:
- `Assets/Scripts/Core/RuntimeRoomManager.cs`

`LargeDungeonMapPainterBase` now uses authored procedural floor masks instead of filling one
rectangle and adding random blockers.

Current template cycle:
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

Each template combines offset chambers, broad combat cores, alcoves, carved negative spaces,
broken edge cells, and interior wall/pillar runs. This is still system-art using temporary
tiles, but the composition direction is now mask-first instead of rectangle-first.

## Lighting Direction

Runtime map paint now creates per-room 2D lights under `Procedural Room Lights`.

Lighting rule:
- temporary full global fill while there is no player-held light / vision-radius mechanic
- warm torch pools near wall/side anchors
- cyan/violet magic pools near shrine/landmark/core anchors
- lights are Unity 2D lights, not baked into floor tiles
- black void outside rooms remains valid, but game start should frame floor and nearby walls

Important correction after user review:
- Do not leave only a small part of the dungeon visible unless there is an explicit gameplay
  mechanic for darkness, torch range, fog of war, or player-held light.
- For now the whole playable dungeon area must stay readable everywhere the player walks.
- Local lights should be low-contrast accent lights that mark composition anchors, not decide
  whether the player can see the room.

## Tool Research Verdict

### Recommended Workflow for RIMA

Use LDtk or Tiled for authored room masks and metadata, then import/convert into Unity Tilemaps.
Keep the current runtime painter as the in-engine prototype and eventual importer target.

Best target data per room:
- `FloorMask`
- `WallMask`
- `DoorSocket_N/E/S/W`
- `LightSocket_Torch/Magic`
- `EnemySpawnZone`
- `PropZone`
- `LandmarkSlot`
- `RoomType`
- `DifficultyBand`

### LDtk

Best fit if the team wants a modern 2D level editor with entity metadata and separate level files.
LDtk has a Unity importer listed by LDtk itself, and the LDtkToUnity docs explicitly call out
separate level files as useful for modular level design and randomly generated dungeon pieces.

Use for:
- room masks
- sockets
- light/entity markers
- modular authored templates

Sources:
- https://ldtk.io/api/
- https://ldtk.io/docs/game-dev/loading/
- https://cammin.github.io/LDtkToUnity/documentation/Importer/topic_LevelImporter.html

### Tiled

Strong second option. Mature, flexible, and good for tile/object layers and custom properties.
Tiled object layers can represent spawn zones, collision polygons, light points, and sockets.
SuperTiled2Unity can import Tiled maps into Unity.

Use for:
- tile layers
- object layers
- custom properties
- JSON/TMX-driven importer workflow

Sources:
- https://doc.mapeditor.org/en/stable/manual/layers/
- https://doc.mapeditor.org/en/stable/manual/custom-properties/
- https://supertiled2unity.readthedocs.io/

### Unity Tilemap + 2D Tilemap Extras

Keep as the final renderer and in-editor paint/debug layer. Tilemap Extras provides Rule Tile,
Rule Override Tile, Random Brush, Line Brush, Group Brush, and related tile helpers. Useful for
walls, corners, floor randomization, and fast visual iteration.

Use for:
- final runtime rendering
- RuleTile/RandomTile style tile selection
- wall/corner rendering from masks

Sources:
- https://docs.unity.cn/Manual/Tilemap.html
- https://docs.unity.cn/Manual/com.unity.2d.tilemap.extras.html

### Dungeon Architect / DunGen

Useful if RIMA later wants a full prefab-room dungeon generator. They are heavier than needed for
the immediate problem because RIMA already has room graph/combat flow and currently needs authored
2D room masks, not a full external dungeon system.

DunGen is room/prefab based and supports runtime dungeon generation; this can inspire the socket
model but should not replace RIMA's current graph yet.

Sources:
- https://docs.dungeonarchitect.dev/unity/unity-overview/
- https://www.aegongames.com/dungen/
- https://dungen-docs.aegongames.com/2.19/core-concepts/dungeon-generator/

### Wave Function Collapse

Useful later for micro-layout detail such as rubble distribution, floor cracks, prop clusters, and
small wall damage patterns. Do not use as the main room authoring tool yet; it can easily create
pretty but gameplay-hostile spaces without strict combat constraints.

### Ogmo

Lightweight free/open-source level editor with tile, decal, entity, and grid layers saved as JSON.
Good for quick mask prototyping, but less directly aligned with Unity import workflows than LDtk or
Tiled.

Sources:
- https://ogmo-editor-3.github.io/
- https://ogmoeditor.itch.io/editor

## Immediate Next Steps

1. Visually inspect the new runtime templates in Unity.
2. If composition reads better, split room data into a serializable authored template format.
3. Pick LDtk or Tiled for an external prototype importer.
4. Produce real floor/wall modules before judging final art quality.
