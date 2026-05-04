# Stone Dungeon Wall QC
Date: 2026-05-03
Status: APPLIED TO TEST SCENE

## Applied Set

These wall candidates were selected and added as Unity Tile assets:

| Source PNG | Tile asset | Verdict |
|---|---|---|
| `stone_wall_pro_0.png` | `Assets/Tiles/StoneDungeon/stone_wall_base_tile.asset` | Main base wall block. |
| `stone_wall_pro_1.png` | `Assets/Tiles/StoneDungeon/stone_wall_alt_tile.asset` | Subtle alternate wall block. |
| `stone_wall_pro_4.png` | `Assets/Tiles/StoneDungeon/stone_wall_cracked_tile.asset` | Cracked variant, good at low frequency. |
| `stone_wall_pro_5.png` | `Assets/Tiles/StoneDungeon/stone_wall_damaged_tile.asset` | Heavy damage variant, use sparingly. |
| `stone_wall_pro_10.png` | `Assets/Tiles/StoneDungeon/stone_wall_sconce_tile.asset` | Decorative wall detail, only 2 placements in test border. |

Palette updated:

- `Assets/TilePalettes/StoneDungeon_Palette.prefab`
- Row 0: 6 floor tiles.
- Row 1: 5 wall tiles.

Scene updated:

- `Assets/Scenes/_IsoGame.unity`
- Ground tilemap: 36x24 interior floor, selected 6-floor distribution.
- Walls tilemap: 40x28 outer room, 2-cell-thick border, selected 5-wall distribution.

Visual mockup:

- `STAGING/stone_dungeon_room_mockup.png`

## Visual Verdict

PASS for test scene.

The selected wall set reads coherently as a stone block border. It is visually consistent with the floor palette after reducing the `stone_grate` floor frequency.

Important caveat: these wall sprites read more like low isometric block/parapet pieces than tall room wall faces. This is acceptable for the current test room, but final environment art should still get a dedicated `straight wall / corner / pillar` pass using the guide.

## Rejected / Do Not Use For Current Border

| Source PNG | Reason |
|---|---|
| `stone_wall_pro_2.png` | Too close to base; not needed. |
| `stone_wall_pro_3.png` | Dotted/concrete style mismatch. |
| `stone_wall_pro_6.png` | Crack density too high; noisy as repeated border. |
| `stone_wall_pro_7.png` | Broken corner silhouette, okay as rare prop but bad as repeated wall. |
| `stone_wall_pro_8.png` | Base duplicate; not needed. |
| `stone_wall_pro_9.png` | Top chunk damage reads awkwardly in repetition. |
| `stone_wall_pro_11.png` | Window/grate is visually strong; keep as rare landmark, not border tile. |
| `stone_wall_pro_12.png` | Hole pattern reads like metal dice/block, not dungeon wall. |
| `stone_wall_pro_13.png` | Dripping/ruined face style mismatch. |
| `stone_wall_pro_14.png` | Rubble pile, not a wall tile. |
| `stone_wall_pro_15.png` | Boulder wall style mismatch with cut-stone set. |

## Next Art Pass

When moving beyond test-room visuals, generate a dedicated wall set:

- straight north-east wall face
- straight north-west wall face
- inside corner
- outside corner
- pillar / column
- broken wall cap

Use `GUIDES/TILE_WALL_OBJECT_PRODUCTION_GUIDE.md` and prefer `create_map_object` for tall wall pieces.
