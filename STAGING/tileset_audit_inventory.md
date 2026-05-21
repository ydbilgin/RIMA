# Wang Tileset Audit Inventory - 2026-05-21

Scope: 36 tilesets audited, 11 local and 25 PixelLab cloud records. Local sheets were checked from `Assets/Art/Tiles/F1/Tilesets/*/spritesheet.png` and `tileset_meta.json`; cloud sheets were checked through PixelLab `get_topdown_tileset`.

Local tile order note: local metadata supplies `cornerKeyToSpriteIndex`. Most sheets are 128x128, 32 px tiles, 4x4. `debris_rift` is 128x256 on disk but its metadata still declares a 4x4 Wang set; only the first 16 sprites are suitable for RuleTile mapping without a custom mapper.

| id | source | name | terrain | sheet | style | act_fit | preview_path | recommendation |
|---|---|---|---|---|---|---|---|---|
| bdca2623-62ac-4624-9ffb-d1728f86e3c3 | local | cold_floor_wall | cold dark floor to wall edge | 128x128, 32, 4x4 | clean, cold, readable | Act 1 / Act 3 support | Assets/Art/Tiles/F1/Tilesets/cold_floor_wall/spritesheet.png | KEEP |
| 4c962284-90f4-43ee-8192-2b108a77b6ca | local | debris_rift | dark debris to rift crack | 128x256, 32, metadata 4x4 | acceptable, extra rows need caution | Act 1 accent | Assets/Art/Tiles/F1/Tilesets/debris_rift/spritesheet.png | KEEP_WITH_CAUTION |
| 9ffbb4d1-79d0-441d-8c23-e1df62e25644 | local | floor_wall | rubble floor to broken stone wall | 128x128, 32, 4x4 | clean Act 1 core | Act 1 | Assets/Art/Tiles/F1/Tilesets/floor_wall/spritesheet.png | KEEP |
| a1b63282-7bc3-4acb-a390-e82853b8168d | local | mauve_hexagon | warm mauve to hexagon trace | 128x128, 32, 4x4 | acceptable, different palette | Act 2 / decoration | Assets/Art/Tiles/F1/Tilesets/mauve_hexagon/spritesheet.png | MAYBE |
| ecfee0a0-a5ec-4992-b435-1f1d3ae2dfdb | local | path_rift | worn path to rift | 128x128, 32, 4x4 | clean | Act 1 accent | Assets/Art/Tiles/F1/Tilesets/path_rift/spritesheet.png | KEEP |
| ea19bab2-fea4-4c36-b5ef-6db1d103cc74 | local | pink_cream | pink soil to cream sand | 128x128, 32, 4x4 | clean but off-biome | Act 2 / decoration | Assets/Art/Tiles/F1/Tilesets/pink_cream/spritesheet.png | MAYBE |
| 9591f35a-2373-4150-b737-7b4620d1834c | local | rubble_moss | rubble floor to moss | 128x128, 32, 4x4 | clean | Act 1 | Assets/Art/Tiles/F1/Tilesets/rubble_moss/spritesheet.png | KEEP |
| 49913501-fd93-41df-8a9f-f24e97a7b76c | local | rubble_path | rubble floor to worn path | 128x128, 32, 4x4 | clean Act 1 core | Act 1 | Assets/Art/Tiles/F1/Tilesets/rubble_path/spritesheet.png | KEEP |
| d43914a8-bd20-4aa4-9ded-f95a773062f9 | local | slate_mineral | cold slate to mineral variation | 128x128, 32, 4x4 | clean but low detail | Act 1 / Act 3 support | Assets/Art/Tiles/F1/Tilesets/slate_mineral/spritesheet.png | KEEP |
| 8c154e37-8c0a-450a-82fd-126cc8b35c97 | local | wall_path | wall terrain to path | 128x128, 32, 4x4 | clean Act 1 support | Act 1 | Assets/Art/Tiles/F1/Tilesets/wall_path/spritesheet.png | KEEP |
| 02a5a97b-9475-4bdb-b2e4-cde475068f4d | local | wall_rift | wall terrain to rift | 128x128, 32, 4x4 | clean Act 1 accent | Act 1 | Assets/Art/Tiles/F1/Tilesets/wall_rift/spritesheet.png | KEEP |
| 3273fdfd-4036-45ef-bf5b-d39177d1fac3 | cloud | dirt_stones_to_grass | warm dirt stones to green meadow | 16 tiles, 32 | clean but meadow palette | discard for RIMA core | PixelLab preview, download_png endpoint | DISCARD |
| 62d91ed9-c5a4-416c-a562-1af92002232d | cloud | granite_to_muddy_stone_path | granite floor to river-stone path | 16 tiles, 32 | acceptable, path too yellow | Act 1 support | PixelLab preview, download_png endpoint | MAYBE |
| 88fbb4e7-045c-4e25-8c29-331bd850da95 | cloud | granite_to_dirt | cool granite to dirt patch | 16 tiles, 32 | acceptable, generic | Act 1 support | PixelLab preview, download_png endpoint | MAYBE |
| f6c16987-cfcf-4f57-8346-47cf93d78f39 | cloud | granite_to_cobble_v2 | granite to darker cobble path | 16 tiles, 32 | acceptable | Act 1 support | PixelLab preview, download_png endpoint | MAYBE |
| 2f886879-a625-4d39-8387-dff867787630 | cloud | granite_to_grass | granite to cool grass | 16 tiles, 32 | poor for current tone | decoration only | PixelLab preview, download_png endpoint | DISCARD |
| ff9f5489-2cfa-4de1-9492-7785bb9d9516 | cloud | granite_to_moss | granite to muted moss | 16 tiles, 32 | acceptable | Act 1 support | PixelLab preview, download_png endpoint | MAYBE |
| 4235c9c1-500d-428c-a497-9c6376460a4e | cloud | dirt_to_cobble | warm dirt to grey-blue cobble | 16 tiles, 32 | acceptable but low top-down | Act 2 support | PixelLab preview, download_png endpoint | MAYBE |
| aa7ca5bb-668a-47a1-8ee6-e99f38bc78b3 | cloud | dirt_path_to_cobble_16px | warm dirt path to cobble | 16 tiles, 16 | mismatched 16 px | discard | PixelLab preview, download_png endpoint | DISCARD |
| 341338df-9f1b-42ef-8db2-33d1fee9cdc8 | cloud | dark_slate_to_moss | dark slate to moss | 16 tiles, 32 | acceptable but hard outlines | Act 1 / Act 3 support | PixelLab preview, download_png endpoint | MAYBE |
| 7b34aa6b-2031-455d-94e5-4322579c984e | cloud | fractured_keep_floor_to_wall | fractured keep floor to wall | 25 tiles, 32 | high detail but green preview base | Act 1 | PixelLab preview, download_png endpoint | MAYBE |
| 04633962-c19c-4fed-9a55-80d954d36614 | cloud | dark_rubble_to_rift | dark rubble floor to rift fracture | 16 tiles, 32 | clean Act 1 accent | Act 1 | Assets/Art/Tiles/Act1_ShatteredKeep/wang_pack/dark_rubble_to_rift.png | KEEP |
| cc1d7d6f-fe72-4934-9f34-9be83234e81b | cloud | warm_pink_to_cream | dreamy pink dust to cream dust | 16 tiles, 32 | clean but off-act | Act 2 | PixelLab preview, download_png endpoint | MAYBE |
| 0a361fb8-f1b5-42d1-98f8-c0689a9a58e1 | cloud | warm_pink_to_cream_v2 | dreamy pink dust to cream dust | 16 tiles, 32 | acceptable but visible outline | Act 2 | PixelLab preview, download_png endpoint | MAYBE |
| b41919aa-d20c-441e-a812-67e1f25f3331 | cloud | worn_path_to_moss | worn stone path to moss | 16 tiles, 32 | clean | Act 1 | Assets/Art/Tiles/Act1_ShatteredKeep/wang_pack/worn_path_to_moss.png | KEEP |
| ea19bab2-fea4-4c36-b5ef-6db1d103cc74 | cloud | pink_soil_to_cream_sand | pink soil to cream sand | 16 tiles, 32 | clean but off-act | Act 2 | PixelLab preview, download_png endpoint | MAYBE |
| 9591f35a-2373-4150-b737-7b4620d1834c | cloud | dark_rubble_to_moss | dark rubble floor to wet moss | 16 tiles, 32 | clean | Act 1 | Assets/Art/Tiles/Act1_ShatteredKeep/wang_pack/dark_rubble_to_moss.png | KEEP |
| ecfee0a0-a5ec-4992-b435-1f1d3ae2dfdb | cloud | worn_path_to_rift | worn path to rift crack | 16 tiles, 32 | clean | Act 1 | Assets/Art/Tiles/Act1_ShatteredKeep/wang_pack/worn_path_to_rift.png | KEEP |
| 02a5a97b-9475-4bdb-b2e4-cde475068f4d | cloud | broken_wall_to_rift | broken wall terrain to rift | 16 tiles, 32 | clean | Act 1 | Assets/Art/Tiles/Act1_ShatteredKeep/wang_pack/broken_wall_to_rift.png | KEEP |
| 8c154e37-8c0a-450a-82fd-126cc8b35c97 | cloud | broken_wall_to_worn_path | broken wall terrain to path | 16 tiles, 32 | clean | Act 1 | Assets/Art/Tiles/Act1_ShatteredKeep/wang_pack/broken_wall_to_worn_path.png | KEEP |
| 4c962284-90f4-43ee-8192-2b108a77b6ca | cloud | dark_debris_to_rift | dark debris stone to rift variant | 25 tiles, 32 | acceptable, non-16 sheet | Act 1 accent | Assets/Art/Tiles/Act1_ShatteredKeep/wang_pack/dark_debris_to_rift.png | KEEP_WITH_CAUTION |
| a1b63282-7bc3-4acb-a390-e82853b8168d | cloud | mauve_pink_to_hexagon | mauve ground to hex trace | 16 tiles, 32 | clean but off-act | Act 2 / decoration | PixelLab preview, download_png endpoint | MAYBE |
| d43914a8-bd20-4aa4-9ded-f95a773062f9 | cloud | cold_slate_to_mineral | cold slate to lighter mineral | 16 tiles, 32 | clean, low detail | Act 1 / Act 3 support | Assets/Art/Tiles/Act1_ShatteredKeep/wang_pack/cold_slate_to_mineral.png | KEEP |
| bdca2623-62ac-4624-9ffb-d1728f86e3c3 | cloud | cold_floor_to_wall_edge | cold dungeon floor to wall edge | 16 tiles, 32 | clean | Act 1 / Act 3 support | Assets/Art/Tiles/Act1_ShatteredKeep/wang_pack/cold_floor_to_wall_edge.png | KEEP |
| 49913501-fd93-41df-8a9f-f24e97a7b76c | cloud | dark_rubble_to_worn_path | dark rubble floor to worn path | 16 tiles, 32 | clean Act 1 core | Act 1 | Assets/Art/Tiles/Act1_ShatteredKeep/wang_pack/dark_rubble_to_worn_path.png | KEEP |
| 9ffbb4d1-79d0-441d-8c23-e1df62e25644 | cloud | dark_rubble_to_broken_wall | dark rubble floor to broken wall | 16 tiles, 32 | clean Act 1 core | Act 1 | Assets/Art/Tiles/Act1_ShatteredKeep/wang_pack/dark_rubble_to_broken_wall.png | KEEP |

Tile content summary for local sheets:
- 4x4 sheets: tiles 0-15 are the Wang corner variants listed by `cornerKeyToSpriteIndex`; low mask tiles are mostly lower terrain, high mask tiles are mostly upper terrain, middle masks are edges and corners.
- `floor_wall`, `rubble_path`, `rubble_moss`, `wall_path`, `wall_rift`, `path_rift`, `cold_floor_wall`, and `slate_mineral` are visually consistent enough for current Act 1 testing.
- `mauve_hexagon` and `pink_cream` are coherent but belong to later warm/dreamy acts, not Shattered Keep.
- `debris_rift` needs caution because the image contains 32 slots while metadata declares a 16-tile Wang set.
