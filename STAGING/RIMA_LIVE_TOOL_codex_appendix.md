# RIMA Live Tool + Asset Layer Codebase Reality Check

Scope: read-only codebase inventory. Binary PNG evidence cites the sidecar `.meta` file because PNGs do not have line numbers.

## Executive Asset Table

| Layer candidate | Asset path glob | Count | Has collider? | Collider shape | sortingOrder current |
|---|---:|---:|---|---|---|
| Floor sprite layer | `Assets/Sprites/Environment/RIMA_AssetParts_v2/{floor,dirt,macro}/*.png` | 36 | No prefab collider | n/a | RoomPainter default order 0 (`Assets/RoomPainter/LayerData/Layer_Floor.asset:15-17`) |
| Cliff tile sprites | `Assets/Sprites/Environment/KitB_Cliff/*.png` | 13 | No prefab collider | n/a | Directional tile has no renderer order; legacy cliff rules use order -50 (`Assets/ScriptableObjects/Environment/CliffPlacementRules_Hades.asset:29`) |
| Parallax/background sprites | `Assets/Sprites/Environment/KitC_BG/*.png`, `Assets/Sprites/Environment/RIMA_AssetParts_v2/rift/*.png` | 11 | No prefab collider | n/a | RoomPainter default order -100 (`Assets/RoomPainter/LayerData/Layer_Parallax.asset:15-17`) |
| Decal/decor sprites | `Assets/Sprites/Environment/PixelLab_Selected_Assets/*.png`, `RIMA_AssetParts_v2/{cracks_bones,moss,pebbles,ritual}/*.png` | 45 | No prefab collider | n/a | Decals enum exists (`Assets/Scripts/RoomPainter/RoomLayer.cs:10`); no `Layer_Decals.asset` currently present |
| Walkable decor prefabs | `Assets/Prefabs/Environment/Decorations/**/*.prefab` | 0 | n/a | n/a | n/a |
| Wall blockers | `Assets/Prefabs/Environment/Walls/AssetPackV3/*.prefab` | 16 | Yes | BoxCollider2D | Mostly renderer order 0; real examples cite `Assets/Prefabs/Environment/Walls/AssetPackV3/wp_rear_wall_2x_real.prefab:218` |
| Obstacle blockers | `Assets/Prefabs/Obstacles/*.prefab` | 3 | Yes | BoxCollider2D | Order 5 (`Assets/Prefabs/Obstacles/StoneColumn.prefab:85`) |
| Cliff face mounting decor | `Assets/Prefabs/Props/ShatteredKeep_PixelLab/mounting_*.prefab` | 15 | No | n/a | Order 0 (`Assets/Prefabs/Props/ShatteredKeep_PixelLab/mounting_00_7227fa35-ade1-406e-af2e-600fb4176af2.prefab:82`) |
| Cliff face statue decor | `Assets/Prefabs/Props/ShatteredKeep_PixelLab/statue_*.prefab` | 14 | Yes | BoxCollider2D | Order 0 (`Assets/Prefabs/Props/ShatteredKeep_PixelLab/statue_00_e899a33d-e043-4f51-860d-ff0c046f89f3.prefab:83`) |
| Gameplay pickups/markers | listed under Gameplay Object Prefabs below | 5 | Mixed | Circle or none | MapFragment root order 10 (`Assets/Prefabs/MapFragment.prefab:85`), RewardPickup order 10 (`Assets/Prefabs/RewardPickup.prefab:84`) |

## A - Asset and Prefab Inventory

### A1. TileBase ScriptableObjects under `Assets/ScriptableObjects/Environment/`

There are 3 TileBase-derived assets under this folder:

| Asset | Type evidence | Notes |
|---|---|---|
| `Assets/ScriptableObjects/Environment/CliffTile_Hades.asset` | Uses `DeterministicVariantTile`, a `TileBase` subclass (`Assets/ScriptableObjects/Environment/CliffTile_Hades.asset:12`, `Assets/Scripts/Environment/DeterministicVariantTile.cs:7`) | 9 variant sprite refs (`Assets/ScriptableObjects/Environment/CliffTile_Hades.asset:15-25`) |
| `Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset` | Uses `DirectionalCliffTile`, a `TileBase` subclass (`Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset:12`, `Assets/Scripts/Environment/DirectionalCliffTile.cs:7`) | 8 direction arrays, each populated with 1 sprite (`Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset:15-30`) |
| `Assets/ScriptableObjects/Environment/VoidBlocker_Tile.asset` | Built-in `UnityEngine.Tilemaps.Tile` (`Assets/ScriptableObjects/Environment/VoidBlocker_Tile.asset:12-14`) | Invisible collider tile: no sprite, collider type 2 (`Assets/ScriptableObjects/Environment/VoidBlocker_Tile.asset:15-16`, `Assets/ScriptableObjects/Environment/VoidBlocker_Tile.asset:36`) |

`CliffPlacementRules_Hades.asset` is not a TileBase; it references `CliffPlacementRules : ScriptableObject` (`Assets/ScriptableObjects/Environment/CliffPlacementRules_Hades.asset:12`, `Assets/Scripts/Environment/CliffPlacementRules.cs:7`).

### A1b. PNG sprites in requested environment folders

Classification heuristic follows the current RoomPainter scanner keywords: `cliff` -> Cliff (`Assets/Editor/RoomPainter/Helpers/RoomPainterAssetScanner.cs:138-140`), `moss/pebbles/cracks_bones/decal/crack` -> Decals (`Assets/Editor/RoomPainter/Helpers/RoomPainterAssetScanner.cs:145-151`), `bg/rift` -> Parallax (`Assets/Editor/RoomPainter/Helpers/RoomPainterAssetScanner.cs:169-173`), and `floor/dirt/macro` -> Floor (`Assets/Editor/RoomPainter/Helpers/RoomPainterAssetScanner.cs:177-185`).

| Category | Count | Names |
|---|---:|---|
| Floor | 36 | `dirt_01`..`dirt_12` (`Assets/Sprites/Environment/RIMA_AssetParts_v2/dirt/dirt_01.png.meta:1`, `Assets/Sprites/Environment/RIMA_AssetParts_v2/dirt/dirt_12.png.meta:1`); `floor_01`..`floor_16` (`Assets/Sprites/Environment/RIMA_AssetParts_v2/floor/floor_01.png.meta:1`, `Assets/Sprites/Environment/RIMA_AssetParts_v2/floor/floor_16.png.meta:1`); `macro_01`..`macro_08` (`Assets/Sprites/Environment/RIMA_AssetParts_v2/macro/macro_01.png.meta:1`, `Assets/Sprites/Environment/RIMA_AssetParts_v2/macro/macro_08.png.meta:1`) |
| Cliff | 13 | `cliff_S`, `cliff_N`, `cliff_E`, `cliff_W`, `cliff_NE`, `cliff_NW`, `cliff_SE`, `cliff_SW`, `cliff_S_new1`..`cliff_S_new4`, `cliff_cyan_glow` (`Assets/Sprites/Environment/KitB_Cliff/cliff_S.png.meta:1`, `Assets/Sprites/Environment/KitB_Cliff/cliff_cyan_glow.png.meta:1`) |
| Decor/decal | 45 | `alabaster_decal_*` (`Assets/Sprites/Environment/PixelLab_Selected_Assets/alabaster_decal_5ccc5721-3007-4d9a-8fce-86e99bc6a078.png.meta:1`); `cracks_bones_01`..`12` (`Assets/Sprites/Environment/RIMA_AssetParts_v2/cracks_bones/cracks_bones_01.png.meta:1`); `moss_01`..`16` (`Assets/Sprites/Environment/RIMA_AssetParts_v2/moss/moss_01.png.meta:1`); `pebbles_01`..`12` (`Assets/Sprites/Environment/RIMA_AssetParts_v2/pebbles/pebbles_01.png.meta:1`); `ritual_01`..`04` (`Assets/Sprites/Environment/RIMA_AssetParts_v2/ritual/ritual_01.png.meta:1`) |
| Parallax/background | 11 | `bg_L0_void`, `bg_L1_nebula`, `bg_L2_ruins_A`, `bg_L2_ruins_B`, `bg_L3_island_large`, `bg_L3_island_small`, `bg_L4_fog` (`Assets/Sprites/Environment/KitC_BG/bg_L0_void.png.meta:1`); `rift_01`..`rift_04` (`Assets/Sprites/Environment/RIMA_AssetParts_v2/rift/rift_01.png.meta:1`) |
| Wall-ish unclassified | 1 | `room_painter_day4_ship_wall` (`Assets/Sprites/Environment/RIMA_AssetParts_v2/room_painter_day4_ship_wall.png.meta:1`) |

### A2. Cliff assets

`DirectionalCliffTile_Hades.asset` has all 8 direction arrays populated with exactly 1 sprite each: `spritesS`, `spritesSE`, `spritesSW`, `spritesE`, `spritesW`, `spritesN`, `spritesNE`, `spritesNW` (`Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset:15-30`). The referenced sprites are the eight directional KitB cliff PNGs; examples: `spritesS` points to `cliff_S.png` via GUID (`Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset:15-16`, `Assets/Sprites/Environment/KitB_Cliff/cliff_S.png.meta:2`), and `spritesNW` points to `cliff_NW.png` (`Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset:29-30`, `Assets/Sprites/Environment/KitB_Cliff/cliff_NW.png.meta:2`).

`CliffAutoPlacer.cs` uses a single `TileBase cliffTile`, not a list (`Assets/Scripts/Environment/CliffAutoPlacer.cs:16`). Its readiness check also requires one `cliffTile` (`Assets/Scripts/Environment/CliffAutoPlacer.cs:136`), and placement writes that single tile into the cliff tilemap (`Assets/Scripts/Environment/CliffAutoPlacer.cs:157-158`).

KitB cliff PNGs are all cliff face/direction sprites, with no mounting or cap-prefixed files in that folder: `cliff_S`, `cliff_N`, `cliff_E`, `cliff_W`, `cliff_NE`, `cliff_NW`, `cliff_SE`, `cliff_SW`, `cliff_S_new1`..`new4`, `cliff_cyan_glow` (`Assets/Sprites/Environment/KitB_Cliff/cliff_S.png.meta:1`, `Assets/Sprites/Environment/KitB_Cliff/cliff_S_new4.png.meta:1`, `Assets/Sprites/Environment/KitB_Cliff/cliff_cyan_glow.png.meta:1`).

### A3. Walkable decor prefabs

`Assets/Prefabs/Environment/Decorations/` exists but contains no `.prefab` files. No bone/vine/rune/plinth/decal prefabs were found under `Assets/Prefabs/Environment/Decorations/**/*.prefab`. A broader prefab-name search for `bone|vine|rune|plinth|decal|ritual|moss|pebble|crack` under `Assets/Prefabs` also returned no prefab paths. Therefore no Collider2D audit is possible for this specific requested glob.

### A4. Wall blocker prefabs

`Assets/Prefabs/Environment/Walls/AssetPackV3/*.prefab` contains 16 prefabs: `01_straight`, `02_outer_corner`, `03_inner_corner`, `04_end`, `05_door_l`, `06_door_r`, `07_alcove`, `08_protrusion`, `piece_01`..`piece_04`, and four `wp_*_real` prefabs.

The numbered and `piece_*` prefabs are prefab variants with stripped BoxCollider2D components, so trigger state is inherited from the source template unless overridden. Example: `01_straight.prefab` strips a BoxCollider2D at `Assets/Prefabs/Environment/Walls/AssetPackV3/01_straight.prefab:161-165`; its source is `WallChunk_Template.prefab` (`Assets/Prefabs/Environment/Walls/AssetPackV3/01_straight.prefab:155`), whose BoxCollider2D has `m_IsTrigger: 0` (`Assets/Prefabs/Environment/Walls/_template/WallChunk_Template.prefab:257-258`, `Assets/Prefabs/Environment/Walls/_template/WallChunk_Template.prefab:287`).

| Prefab group | Count | Collider evidence | Trigger |
|---|---:|---|---|
| `01_*`..`08_*` variants | 8 | Stripped BoxCollider2D in each variant, e.g. `01_straight` (`Assets/Prefabs/Environment/Walls/AssetPackV3/01_straight.prefab:161-165`) | Inherits source `m_IsTrigger: 0` (`Assets/Prefabs/Environment/Walls/_template/WallChunk_Template.prefab:287`) |
| `piece_01`..`piece_04` variants | 4 | Stripped BoxCollider2D in each variant, e.g. `piece_01` (`Assets/Prefabs/Environment/Walls/AssetPackV3/piece_01.prefab:161`) | Inherits source `m_IsTrigger: 0` (`Assets/Prefabs/Environment/Walls/_template/WallChunk_Template.prefab:287`) |
| `wp_door_arch_2x_real.prefab` | 1 | Direct BoxCollider2D (`Assets/Prefabs/Environment/Walls/AssetPackV3/wp_door_arch_2x_real.prefab:73`) | `m_IsTrigger: 0` (`Assets/Prefabs/Environment/Walls/AssetPackV3/wp_door_arch_2x_real.prefab:103`) |
| `wp_low_front_outer_corner_real.prefab` | 1 | Direct BoxCollider2D (`Assets/Prefabs/Environment/Walls/AssetPackV3/wp_low_front_outer_corner_real.prefab:257`) | `m_IsTrigger: 0` (`Assets/Prefabs/Environment/Walls/AssetPackV3/wp_low_front_outer_corner_real.prefab:287`) |
| `wp_rear_wall_2x_real.prefab` | 1 | Direct BoxCollider2D (`Assets/Prefabs/Environment/Walls/AssetPackV3/wp_rear_wall_2x_real.prefab:42`) | `m_IsTrigger: 0` (`Assets/Prefabs/Environment/Walls/AssetPackV3/wp_rear_wall_2x_real.prefab:72`) |
| `wp_side_wall_stepped_2x_real.prefab` | 1 | Direct BoxCollider2D (`Assets/Prefabs/Environment/Walls/AssetPackV3/wp_side_wall_stepped_2x_real.prefab:135`) | `m_IsTrigger: 0` (`Assets/Prefabs/Environment/Walls/AssetPackV3/wp_side_wall_stepped_2x_real.prefab:165`) |

There is also a separate `Assets/Prefabs/Environment/Walls/Placeholders/*.prefab` folder with 14 placeholder wall prefabs (`wp_connector`, `wp_door_arch`, `wp_inner_corner`, `wp_low_front_1x`, `wp_low_front_2x`, `wp_open_gap`, `wp_outer_corner`, `wp_rear_wall_1x`, `wp_rear_wall_2x`, `wp_rear_wall_3x`, `wp_seam`, `wp_side_wall_1x`, `wp_side_wall_2x`, `wp_side_wall_3x`). It is outside the requested `AssetPackV3/*.prefab` glob.

Obstacle prefabs:

| Prefab | Collider shape | isTrigger | sortingOrder |
|---|---|---:|---:|
| `Assets/Prefabs/Obstacles/Chasm.prefab` | BoxCollider2D (`Assets/Prefabs/Obstacles/Chasm.prefab:112`) | 1 (`Assets/Prefabs/Obstacles/Chasm.prefab:142`) | 5 (`Assets/Prefabs/Obstacles/Chasm.prefab:85`) |
| `Assets/Prefabs/Obstacles/NarrowPassage.prefab` | BoxCollider2D (`Assets/Prefabs/Obstacles/NarrowPassage.prefab:112`) | 1 (`Assets/Prefabs/Obstacles/NarrowPassage.prefab:142`) | 5 (`Assets/Prefabs/Obstacles/NarrowPassage.prefab:85`) |
| `Assets/Prefabs/Obstacles/StoneColumn.prefab` | BoxCollider2D (`Assets/Prefabs/Obstacles/StoneColumn.prefab:112`) | 0 (`Assets/Prefabs/Obstacles/StoneColumn.prefab:142`) | 5 (`Assets/Prefabs/Obstacles/StoneColumn.prefab:85`) |

### A5. Cliff-face decor / mounting prefabs

Mounting prefabs: 15 files, all named `mounting_00` through `mounting_14`. They have SpriteRenderer only and no Collider2D in the prefab YAML; each has `m_SortingOrder: 0`, SpriteRenderer `m_Size: {x: 2, y: 2}`, and root local position `{x: 0, y: 0, z: 0}`. Example evidence: `Assets/Prefabs/Props/ShatteredKeep_PixelLab/mounting_00_7227fa35-ade1-406e-af2e-600fb4176af2.prefab:35-36`, `Assets/Prefabs/Props/ShatteredKeep_PixelLab/mounting_00_7227fa35-ade1-406e-af2e-600fb4176af2.prefab:82`, `Assets/Prefabs/Props/ShatteredKeep_PixelLab/mounting_00_7227fa35-ade1-406e-af2e-600fb4176af2.prefab:89`, `Assets/Prefabs/Props/ShatteredKeep_PixelLab/mounting_00_7227fa35-ade1-406e-af2e-600fb4176af2.prefab:29`.

All mounting sprite imports have center pivot `{x: 0.5, y: 0.5}`; examples: `Assets/Sprites/Environment/ShatteredKeep_PixelLab/Props/mounting_00_7227fa35-ade1-406e-af2e-600fb4176af2.png.meta:53`, `Assets/Sprites/Environment/ShatteredKeep_PixelLab/Props/mounting_14_41342e20-39a8-4482-9775-48abf9f05262.png.meta:53`.

Statue prefabs: 14 files, all named `statue_00` through `statue_13`. They have SpriteRenderer order 0, SpriteRenderer size 2x2, BoxCollider2D size 1.7x1.2, and `m_IsTrigger: 0`. Example evidence: `Assets/Prefabs/Props/ShatteredKeep_PixelLab/statue_00_e899a33d-e043-4f51-860d-ff0c046f89f3.prefab:83`, `Assets/Prefabs/Props/ShatteredKeep_PixelLab/statue_00_e899a33d-e043-4f51-860d-ff0c046f89f3.prefab:90`, `Assets/Prefabs/Props/ShatteredKeep_PixelLab/statue_00_e899a33d-e043-4f51-860d-ff0c046f89f3.prefab:95`, `Assets/Prefabs/Props/ShatteredKeep_PixelLab/statue_00_e899a33d-e043-4f51-860d-ff0c046f89f3.prefab:125`, `Assets/Prefabs/Props/ShatteredKeep_PixelLab/statue_00_e899a33d-e043-4f51-860d-ff0c046f89f3.prefab:139`.

All statue sprite imports also use center pivot `{x: 0.5, y: 0.5}`; examples: `Assets/Sprites/Environment/ShatteredKeep_PixelLab/Props/statue_00_e899a33d-e043-4f51-860d-ff0c046f89f3.png.meta:62`, `Assets/Sprites/Environment/ShatteredKeep_PixelLab/Props/statue_13_c5711681-d7f9-4fdc-9a20-00200bcccd1d.png.meta:53`.

### A6. Gameplay object prefabs and gates

| Prefab | Collider | isTrigger | sortingOrder |
|---|---|---:|---:|
| `Assets/Prefabs/Chest.prefab` | CircleCollider2D (`Assets/Prefabs/Chest.prefab:130`) | 0 (`Assets/Prefabs/Chest.prefab:160`) | 0 (`Assets/Prefabs/Chest.prefab:86`) |
| `Assets/Prefabs/MapFragment.prefab` | CircleCollider2D (`Assets/Prefabs/MapFragment.prefab:97`) | 1 (`Assets/Prefabs/MapFragment.prefab:127`) | 10 (`Assets/Prefabs/MapFragment.prefab:85`) |
| `Assets/Prefabs/RewardPickup.prefab` | none in prefab YAML | n/a | 10 (`Assets/Prefabs/RewardPickup.prefab:84`) |
| `Assets/Prefabs/Environment/MapFragment.prefab` | CircleCollider2D (`Assets/Prefabs/Environment/MapFragment.prefab:96`) | 1 (`Assets/Prefabs/Environment/MapFragment.prefab:126`) | 0 (`Assets/Prefabs/Environment/MapFragment.prefab:84`) |
| `Assets/Prefabs/Environment/PlayerStartMarker.prefab` | none in prefab YAML | n/a | -20 (`Assets/Prefabs/Environment/PlayerStartMarker.prefab:83`) |

No prefab path under `Assets/Prefabs` contains `Gate`. The gate implementation is currently code-driven: `Gate.cs` requires SpriteRenderer and BoxCollider2D (`Assets/Scripts/Environment/Gate.cs:15-17`), sets the BoxCollider2D as trigger in `Awake` (`Assets/Scripts/Environment/Gate.cs:88-95`), and enables the collider only for the Unlocked state (`Assets/Scripts/Environment/Gate.cs:144-172`). The parallel `GateBehavior` script documents a planned Gate prefab setup but only requires SpriteRenderer, with optional BoxCollider2D/Animator in the comment (`Assets/Scripts/Core/GateBehavior.cs:9-25`).

## B - RoomPainter Pipeline Audit

### B1. Physics keyword gaps

`RoomPainterPhysicsRules` has one broad `cliff` physics rule: it makes all assets with `cliff` in the filename blocking static BoxCollider2D on the `Obstacle` layer (`Assets/Editor/RoomPainter/AssetPipeline/RoomPainterPhysicsRules.cs:26-30`). It does not differentiate cliff base/tile from cliff-face decor, mounting, statues, cap pieces, or pedestal pieces. No `mounting`, `statue`, or `pedestal` keyword exists in the current physics rule list (`Assets/Editor/RoomPainter/AssetPipeline/RoomPainterPhysicsRules.cs:26-66`).

Recommended new keywords for a 5-layer architecture:

| Need | Suggested keywords | Physics default |
|---|---|---|
| Floor paint | `floor`, `dirt`, `macro`, `tile`, `stone`, `cobble` | non-blocking |
| Cliff edge/base blocker | `cliff_base`, `cliff_edge`, `void_blocker`, `chasm_edge` | blocking Box |
| Cliff face visual decor | `mounting`, `cliff_face`, `cap`, `backwall`, `wall_decor` | non-blocking by default |
| Large wall blocker | `wall`, `column`, `pillar`, `statue_blocker` | blocking Box/Capsule |
| Walkable decal/decor | `moss`, `pebbles`, `cracks_bones`, `decal`, `rune`, `ritual` | non-blocking |
| Gameplay pickups | `pickup`, `fragment`, `reward`, `chest` | trigger for pickup/fragment/reward, blocking for chest |

The layer inference scanner has more visual-layer keywords than physics rules, but still no `mounting`, `statue`, `pedestal`, or `plinth` distinction (`Assets/Editor/RoomPainter/Helpers/RoomPainterAssetScanner.cs:136-194`).

### B2. RoomPainterAssetPostprocessor behavior

`RoomPainterAssetPostprocessor.cs` is under `Assets/Editor/RoomPainter/AssetPipeline/` and is an `AssetPostprocessor` (`Assets/Editor/RoomPainter/AssetPipeline/RoomPainterAssetPostprocessor.cs:7`).

It does the following at import time:

- Watches only `Assets/Sprites` and `Assets/Prefabs` (`Assets/Editor/RoomPainter/AssetPipeline/RoomPainterAssetPostprocessor.cs:12-16`).
- Queues texture and prefab imports, then flushes them on `EditorApplication.delayCall` (`Assets/Editor/RoomPainter/AssetPipeline/RoomPainterAssetPostprocessor.cs:37-48`, `Assets/Editor/RoomPainter/AssetPipeline/RoomPainterAssetPostprocessor.cs:200-209`).
- Backfills metadata by finding Sprite, Texture2D, and Prefab assets under a requested folder (`Assets/Editor/RoomPainter/AssetPipeline/RoomPainterAssetPostprocessor.cs:112-138`).
- Creates `RoomPainterAsset` metadata under `Assets/RoomPainter/AssetMetadata`, assigning display name, layer, scale, visual offset, y-sort, collider size, physics defaults, and layer defaults (`Assets/Editor/RoomPainter/AssetPipeline/RoomPainterAssetPostprocessor.cs:391-428`).
- Forces sprite import settings to Sprite / Single for image paths before metadata creation (`Assets/Editor/RoomPainter/AssetPipeline/RoomPainterAssetPostprocessor.cs:545-581`) and saves assets after flush (`Assets/Editor/RoomPainter/AssetPipeline/RoomPainterAssetPostprocessor.cs:352-383`).

### B3. RoomPainterColliderEditor

`RoomPainterColliderEditor.cs` is 262 LOC. It runs as an Editor-only SceneView hook through `[InitializeOnLoad]` and `SceneView.duringSceneGui` (`Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs:6-7`, `Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs:28-32`).

It operates on `Selection.activeGameObject` and requires the selected object to have a `RoomPainterAssetBinding` (`Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs:40-58`). It reads the selected object's current `Collider2D[]` and modifies those components in place (`Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs:58-67`). On drag it records undo and marks the collider dirty (`Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs:123-129`, `Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs:195-201`, `Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs:234-240`). There is no `PrefabUtility.SavePrefabAsset`, `PrefabUtility.ApplyPrefabInstance`, or prefab asset write call in this file.

PolygonCollider2D is not supported. The dispatch handles BoxCollider2D, CircleCollider2D, and CapsuleCollider2D only (`Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs:70-89`).

### B4. Per-prefab collider authoring path

For "user drags handle on prefab in painter -> all scene instances pick up new collider", the right Unity-native path is to edit the prefab asset and save it, not just dirty a scene instance. Current code only mutates selected scene objects (`Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs:47-67`) and marks the component dirty (`Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs:123-129`).

Recommended implementation: resolve the selected painted instance back to its prefab source, load/edit the prefab asset contents or prefab stage object, then save the prefab asset (`PrefabUtility.SavePrefabAsset` or equivalent save after `LoadPrefabContents`). `PrefabUtility.ApplyPrefabInstance` is better when the user intentionally edits a scene instance and wants to apply that override back to the source; direct scene edit plus dirty is not enough for propagation.

### B5. Visual category filtering

The visible RoomPainter palette filters are: All, Floor, Cliff, Props, Parallax, All others (`Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs:237-248`). Internally the highlighted filter layers are Floor, Cliff, Props, and Parallax (`Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs:27-33`); `OtherFilter` excludes those and keeps every other suggested layer (`Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs:579-599`). The full `RoomLayer` enum already contains Floor, Edge, Cliff, Wall, Props, Decals, Lighting, Collision, Occlusion, and Parallax (`Assets/Scripts/RoomPainter/RoomLayer.cs:3-15`).

## C - Live Tool Feasibility

### C1. Runtime room load infrastructure

`RoomManifestSO` stores `roomId`, `TextAsset jsonLayout`, default camera bounds, and connected rooms (`Assets/Scripts/Map/Data/RoomManifestSO.cs:6-11`). `MapManifestSO` stores manifest ID, display name, act order, starting/ending room, rooms, and checkpoint rooms (`Assets/Scripts/Map/Data/MapManifestSO.cs:6-15`).

`RuntimeRoomManager` has a serialized `currentManifest` (`Assets/Scripts/Core/RuntimeRoomManager.cs:56-58`). In manifest mode it builds a lookup from pre-existing children under a `Rooms` GameObject (`Assets/Scripts/Core/RuntimeRoomManager.cs:188-200`), activates only the starting room (`Assets/Scripts/Core/RuntimeRoomManager.cs:203-226`), and transitions by toggling active GameObjects (`Assets/Scripts/Core/RuntimeRoomManager.cs:228-289`). It parses `room.jsonLayout.text` only to apply camera bounds (`Assets/Scripts/Core/RuntimeRoomManager.cs:332-345`).

The actual JSON scene populate path exists in `Assets/Scripts/Map/Runtime/RoomLoader.cs`: it reads JSON from disk (`Assets/Scripts/Map/Runtime/RoomLoader.cs:24-31`, `Assets/Scripts/Map/Runtime/RoomLoader.cs:56-67`), clears and paints the floor tilemap (`Assets/Scripts/Map/Runtime/RoomLoader.cs:69-87`), instantiates walls (`Assets/Scripts/Map/Runtime/RoomLoader.cs:118-135`), instantiates props via `Resources.Load` (`Assets/Scripts/Map/Runtime/RoomLoader.cs:137-154`), places mob spawn markers (`Assets/Scripts/Map/Runtime/RoomLoader.cs:156-169`), door triggers (`Assets/Scripts/Map/Runtime/RoomLoader.cs:171-194`), and player spawn markers (`Assets/Scripts/Map/Runtime/RoomLoader.cs:196-209`).

Answer: the existing manifest runtime path does not currently reload and repopulate a room from JSON while playing; it toggles prebuilt room GameObjects and uses JSON only for camera bounds (`Assets/Scripts/Core/RuntimeRoomManager.cs:219-225`, `Assets/Scripts/Core/RuntimeRoomManager.cs:271-282`, `Assets/Scripts/Core/RuntimeRoomManager.cs:337-344`). The smallest change is to add a `ReloadCurrentRoomFromJson` hook that looks up the current `RoomManifestSO`, clears prior generated child objects under the active room root, calls the existing `RoomLoader.LoadJsonToScene`, then reapplies spawn/camera/enter state. Without cleanup, repeated calls would duplicate walls/props/markers because `RoomLoader` clears only floor tiles and instance lists, not previously instantiated child objects (`Assets/Scripts/Map/Runtime/RoomLoader.cs:41-50`, `Assets/Scripts/Map/Runtime/RoomLoader.cs:77-84`, `Assets/Scripts/Map/Runtime/RoomLoader.cs:132`, `Assets/Scripts/Map/Runtime/RoomLoader.cs:149`, `Assets/Scripts/Map/Runtime/RoomLoader.cs:165-167`, `Assets/Scripts/Map/Runtime/RoomLoader.cs:180-192`).

### C2. Hot-reload mechanism options

Addressables is not installed in `Packages/manifest.json`; there is no `com.unity.addressables` dependency in the manifest, while AssetBundle modules are present (`Packages/manifest.json:28`, `Packages/manifest.json:47`). Installing Addressables for this live-room workflow is likely net cost unless the project also needs build-time remote catalog/content management. For JSON live edit and local prefab iteration, it adds catalog/build/load lifecycle overhead that the current JSON + Resources/registry pipeline does not need.

AssetBundle hot-swap is viable for player-build prefab live reload in principle because AssetBundle modules are present (`Packages/manifest.json:28`, `Packages/manifest.json:47`), but it is a heavier Tier 2/3 path: bundle building, dependency versioning, unload/reload semantics, and reference rebinding.

`Resources.Load` is already used for JSON props and mobs (`Assets/Scripts/Map/Runtime/RoomLoader.cs:146`, `Assets/Scripts/Map/Runtime/RoomInstance.cs:171-173`). It is fine for built-in assets by path, but it is not a live replacement system for arbitrary changed prefab files in a built player. A `FileSystemWatcher` can watch external JSON and call the existing disk read path (`Assets/Scripts/Map/Runtime/RoomLoader.cs:56-67`), but it will not make a built player reload changed Unity prefab assets from `Assets/Prefabs` without an AssetBundle, Addressables, or custom runtime registry.

Lightest option: JSON file watch. Hook location is `RuntimeRoomManager` around `currentRoomId` and `currentManifest.rooms` lookup (`Assets/Scripts/Core/RuntimeRoomManager.cs:115-117`, `Assets/Scripts/Core/RuntimeRoomManager.cs:332-339`), then call a cleanup + `RoomLoader.LoadJsonToScene` flow (`Assets/Scripts/Map/Runtime/RoomLoader.cs:16-54`).

### C3. External Player Build bridge options

| Bridge | Feasibility | Expected latency | LOC estimate | Failure modes |
|---|---|---:|---:|---|
| Named pipe (`System.IO.Pipes.NamedPipeServerStream`) | Feasible for local Editor <-> Player IPC; requires main-thread dispatch before touching Unity objects | 1-20 ms local | 120-220 | Pipe name collision, dead client/server, blocking read on wrong thread, Windows-first assumptions, message framing |
| File watcher (`STAGING/live/room_current.json`) | Feasible and closest to current JSON loader; player watches file then reloads active room | 50-300 ms depending debounce | 80-160 | Partial writes, duplicate watcher events, path mismatch in build, file lock, no prefab replacement |
| Loopback TCP / OSC | Feasible and more portable for separate tools | 5-50 ms local | 150-300 | Port conflict, firewall prompts, schema drift, reconnect logic, packet loss if UDP/OSC |

The common implementation requirement is the same: all bridges must enqueue a reload onto Unity's main thread, then use the RoomLoader/RRM hook described above (`Assets/Scripts/Map/Runtime/RoomLoader.cs:16-54`, `Assets/Scripts/Core/RuntimeRoomManager.cs:228-289`).

### C4. Standalone Tool feasibility

A standalone Unity-built `.exe` cannot use the current RoomPainter editor APIs directly. Local evidence: the RoomPainter editor asmdef is Editor-only via `includePlatforms: ["Editor"]` (`Assets/Editor/RoomPainter/RIMA.RoomPainter.Editor.asmdef:8-10`), and the current tool depends on UnityEditor `EditorWindow`, `SceneView`, `Handles`, and `AssetDatabase` (`Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs:1-7`, `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs:86-95`, `Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs:1`, `Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs:107-120`, `Assets/Editor/RoomPainter/Helpers/RoomPainterAssetScanner.cs:3`, `Assets/Editor/RoomPainter/Helpers/RoomPainterAssetScanner.cs:36-37`).

Workaround: keep Tier 1/2 as an Editor extension and, for Tier 3, rebuild the tool as runtime UI using runtime-safe systems: UI Toolkit Runtime, runtime asset registry, JSON room model, and either built-in Resources paths or AssetBundle catalogs. UI Toolkit Runtime is the better standalone UI path because `EditorWindow`, `EditorGUILayout`, `SceneView`, and `Handles` do not ship; runtime IMGUI is possible for debug tools but would not reuse the current Editor UI.

Asset reference sharing options:

| Option | Fit |
|---|---|
| AssetBundle catalog | Best for standalone tool/player live asset exchange; higher build pipeline cost |
| JSON GUID/path list | Best for Editor-only or same-project tooling; not enough in player because GUID/AssetDatabase lookup is Editor-only (`Assets/Editor/RoomPainter/Helpers/RoomPainterAssetScanner.cs:36-41`) |
| Slim runtime asset registry | Best middle path: ScriptableObject or JSON maps logical IDs to Resources paths or bundled asset handles; aligns with current `Resources.Load` usage (`Assets/Scripts/Map/Runtime/RoomLoader.cs:146`, `Assets/Scripts/Map/Runtime/RoomInstance.cs:171-173`) |

### C5. DirectionalCliffTile activation status

`DirectionalCliffTile.cs` defines all 8 sprite arrays: `spritesS`, `spritesSE`, `spritesSW`, `spritesE`, `spritesW`, `spritesN`, `spritesNE`, `spritesNW` (`Assets/Scripts/Environment/DirectionalCliffTile.cs:9-17`). It maps neighboring floor cells to directions at `Assets/Scripts/Environment/DirectionalCliffTile.cs:47-77`, using the scene `CliffAutoPlacer.floorTilemap` in Editor only (`Assets/Scripts/Environment/DirectionalCliffTile.cs:37-45`).

`DirectionalCliffTile_Hades.asset` has all 8 arrays populated, one sprite per direction (`Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset:15-30`).

If the user wires existing `mounting_*.png` as cliff face decor variants, the existing `DirectionalCliffTile` will not consume those automatically as separate mounted decor. It can only select one `Sprite` into `TileData.sprite` from one of the 8 arrays (`Assets/Scripts/Environment/DirectionalCliffTile.cs:69-77`, `Assets/Scripts/Environment/DirectionalCliffTile.cs:81-85`). Mounting assets are prefabs/sprites with no connection to this tile selection path (`Assets/Prefabs/Props/ShatteredKeep_PixelLab/mounting_00_7227fa35-ade1-406e-af2e-600fb4176af2.prefab:35-36`, `Assets/Prefabs/Props/ShatteredKeep_PixelLab/mounting_00_7227fa35-ade1-406e-af2e-600fb4176af2.prefab:84`). It needs code or tooling changes for layered cliff-face decor placement.

## D - LOC Estimates

| Work item | LOC | Files | Risk |
|---|---:|---|---|
| Asset layer migration: add/extend room-layer metadata and backfill existing assets | 80-160 code LOC plus generated metadata churn | `Assets/Scripts/RoomPainter/RoomLayer.cs`, `Assets/Scripts/RoomPainter/RoomPainterAsset.cs`, `Assets/Editor/RoomPainter/Helpers/RoomPainterAssetScanner.cs`, `Assets/Editor/RoomPainter/AssetPipeline/RoomPainterAssetPostprocessor.cs`, `Assets/RoomPainter/AssetMetadata/*.asset` | M |
| Per-prefab collider drag-handle: write to prefab asset, not only scene instance | 120-220 | `Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs`, possibly inspector physics section | M/H |
| Sorting layer registration: 5-6 new sorting layers in ProjectSettings | 10-25 YAML + 20-50 code/data mapping | `ProjectSettings/TagManager.asset`, `Assets/RoomPainter/LayerData/*.asset`, `RoomPainterAssetPostprocessor.DefaultOrder` (`Assets/Editor/RoomPainter/AssetPipeline/RoomPainterAssetPostprocessor.cs:485-504`) | M |
| Tier 2 live tool: file-watcher JSON room reload during Play Mode / Player Build | 250-450 | `Assets/Scripts/Core/RuntimeRoomManager.cs`, `Assets/Scripts/Map/Runtime/RoomLoader.cs`, new runtime bridge/watcher script, optional editor writer | M/H |
| Tier 3 standalone tool: refactor Editor extension to runtime UI Toolkit window | 1200-2200+ | New runtime UI assembly/files, runtime asset registry, RoomPainter model extraction from `Assets/Editor/RoomPainter/**`, RoomLoader extensions | H |
| DirectionalCliffTile activation: wire mounting PNG variants into 8-direction arrays | 0-40 if only asset/YAML assignment; 120-250 if auto-spawned face decor layer | `Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset`, `Assets/Scripts/Environment/DirectionalCliffTile.cs`, possible cliff decor placer | L for array-only, M for decor layer |

Notes for the first row: `RoomPainterAsset` already has `defaultLayer` (`Assets/Scripts/RoomPainter/RoomPainterAsset.cs:29`) and `RoomLayer` already has 10 enum values (`Assets/Scripts/RoomPainter/RoomLayer.cs:3-15`). The migration is not "add enum from zero"; it is taxonomy refinement plus metadata backfill. Current metadata coverage is only 15 assets under `Assets/RoomPainter/AssetMetadata`, while the postprocessor import roots are all sprites and prefabs (`Assets/Editor/RoomPainter/AssetPipeline/RoomPainterAssetPostprocessor.cs:12-16`, `Assets/Editor/RoomPainter/AssetPipeline/RoomPainterAssetPostprocessor.cs:584-598`).

## Reality Check for Opus

- The "live JSON reload" path exists as `RoomLoader.LoadJsonToScene`, but `RuntimeRoomManager` does not currently use it for manifest room transitions; it toggles prebuilt room GameObjects and parses JSON only for camera bounds (`Assets/Scripts/Core/RuntimeRoomManager.cs:219-225`, `Assets/Scripts/Core/RuntimeRoomManager.cs:337-344`, `Assets/Scripts/Map/Runtime/RoomLoader.cs:16-54`).
- Cliff direction infrastructure is partially activated: 8 arrays exist and are populated, but each direction has only 1 sprite and the system selects a single Tile sprite, not separate cliff-face decor (`Assets/Scripts/Environment/DirectionalCliffTile.cs:9-17`, `Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset:15-30`).
- `mounting_*.prefab` are currently pure visuals with no colliders, order 0, size 2x2, center pivot; `statue_*.prefab` are visuals plus blocking BoxCollider2D (`Assets/Prefabs/Props/ShatteredKeep_PixelLab/mounting_00_7227fa35-ade1-406e-af2e-600fb4176af2.prefab:82-89`, `Assets/Prefabs/Props/ShatteredKeep_PixelLab/statue_00_e899a33d-e043-4f51-860d-ff0c046f89f3.prefab:95-139`).
- The current collider handle tool is scene-instance only and supports Box/Circle/Capsule, not Polygon or prefab asset save (`Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs:40-89`, `Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs:123-129`).
- Tier 3 standalone means a real runtime-tool rebuild: current RoomPainter code is Editor-only and depends on `UnityEditor`, `EditorWindow`, `SceneView`, `Handles`, and `AssetDatabase` (`Assets/Editor/RoomPainter/RIMA.RoomPainter.Editor.asmdef:8-10`, `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs:1-7`, `Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs:1`, `Assets/Editor/RoomPainter/Helpers/RoomPainterAssetScanner.cs:36-37`).
