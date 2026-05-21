# CODEX DONE - Archive Legacy Non-Iso Wall + Floor Assets S95

## Result
Completed archive move for the task-listed legacy wall/floor/tile/scene assets. No file contents were edited and no commit was made.

## Moved counts
- PNG: 41 physical files in archive target.
- PNG meta: 41 in archive target.
- Tile asset: 32 in archive target.
- Tile asset meta: 32 in archive target.
- Folder meta moved: 6 in archive target.
- Scene: 1 in archive scene target.
- Scene meta: 1 in archive scene target.

Expected task inventory is present: 41 PNG, 73 PNG/asset metas, 32 .asset, 1 scene. Additional moved Unity folder metas: 6 under asset archive plus the existing scene-folder target context.

## Source cleanup
- Removed by move: walls/painted_v01, floor_tiles/granite_base, floor_tiles/granite_low_topdown_v02, floor_tiles/painted_v03, Data/Tiles/Act1_granite_low_v02, Data/Tiles/Act1_ShatteredKeep/painted_v03, Scenes/Demo/RoomPipelineTest.unity.
- Current walls source entries:
  - act1_wall_arch_opening_v01.png
  - act1_wall_arch_opening_v01.png.meta
  - pilot_a_test
  - pilot_a_test.meta

- Current floor_tiles source entries:
  - iso
  - iso.meta

- Data/Tiles grep Act1_granite_low|painted_v03: <empty>
- Scenes/Demo grep RoomPipelineTest: <empty>

## Archive tree
`
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_granite_var0_00.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_granite_var0_00.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_granite_var1_01.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_granite_var1_01.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_granite_var2_02.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_granite_var2_02.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_granite_var3_03.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_granite_var3_03.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_mossy_var0_08.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_mossy_var0_08.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_mossy_var1_09.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_mossy_var1_09.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_mossy_var2_10.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_mossy_var2_10.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_mossy_var3_11.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_mossy_var3_11.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_rift_var0_12.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_rift_var0_12.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_rift_var1_13.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_rift_var1_13.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_rift_var2_14.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_rift_var2_14.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_rift_var3_15.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_rift_var3_15.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_worn_var0_04.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_worn_var0_04.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_worn_var1_05.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_worn_var1_05.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_worn_var2_06.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_worn_var2_06.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_worn_var3_07.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_granite_low_v02\low_worn_var3_07.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_A_granite_v01_tile_0.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_A_granite_v01_tile_0.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_A_granite_v01_tile_1.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_A_granite_v01_tile_1.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_A_granite_v01_tile_2.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_A_granite_v01_tile_2.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_A_granite_v01_tile_3.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_A_granite_v01_tile_3.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_B_cracked_v01_tile_0.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_B_cracked_v01_tile_0.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_B_cracked_v01_tile_1.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_B_cracked_v01_tile_1.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_B_cracked_v01_tile_2.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_B_cracked_v01_tile_2.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_B_cracked_v01_tile_3.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_B_cracked_v01_tile_3.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_C_dirt_v01_tile_0.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_C_dirt_v01_tile_0.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_C_dirt_v01_tile_1.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_C_dirt_v01_tile_1.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_C_dirt_v01_tile_2.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_C_dirt_v01_tile_2.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_C_dirt_v01_tile_3.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_C_dirt_v01_tile_3.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_D_ritual_v01_tile_0.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_D_ritual_v01_tile_0.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_D_ritual_v01_tile_1.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_D_ritual_v01_tile_1.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_D_ritual_v01_tile_2.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_D_ritual_v01_tile_2.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_D_ritual_v01_tile_3.asset
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\Data\Tiles\Act1_ShatteredKeep_painted_v03\floor_D_ritual_v01_tile_3.asset.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_cave_moss_var00.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_cave_moss_var00.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_cave_moss_var01.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_cave_moss_var01.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_cave_moss_var02.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_cave_moss_var02.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_cave_moss_var03.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_cave_moss_var03.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_granite_var00.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_granite_var00.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_granite_var01.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_granite_var01.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_granite_var02.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_granite_var02.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_granite_var03.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_granite_var03.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_mud_var00.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_mud_var00.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_mud_var01.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_mud_var01.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_mud_var02.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_mud_var02.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_mud_var03.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_mud_var03.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_worn_stone_var00.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_worn_stone_var00.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_worn_stone_var01.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_worn_stone_var01.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_worn_stone_var02.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_worn_stone_var02.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_worn_stone_var03.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_base\act1_floor_4mat_worn_stone_var03.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_granite_var0_00.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_granite_var0_00.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_granite_var1_01.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_granite_var1_01.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_granite_var2_02.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_granite_var2_02.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_granite_var3_03.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_granite_var3_03.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_mossy_var0_08.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_mossy_var0_08.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_mossy_var1_09.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_mossy_var1_09.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_mossy_var2_10.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_mossy_var2_10.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_mossy_var3_11.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_mossy_var3_11.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_rift_var0_12.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_rift_var0_12.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_rift_var1_13.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_rift_var1_13.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_rift_var2_14.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_rift_var2_14.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_rift_var3_15.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_rift_var3_15.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_worn_var0_04.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_worn_var0_04.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_worn_var1_05.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_worn_var1_05.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_worn_var2_06.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_worn_var2_06.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_worn_var3_07.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\granite_low_topdown_v02\act1_floor_worn_var3_07.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\painted_v03
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\painted_v03.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\painted_v03\floor_A_granite_v01.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\painted_v03\floor_A_granite_v01.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\painted_v03\floor_B_cracked_v01.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\painted_v03\floor_B_cracked_v01.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\painted_v03\floor_C_dirt_v01.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\painted_v03\floor_C_dirt_v01.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\painted_v03\floor_D_ritual_v01.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\floor_tiles\painted_v03\floor_D_ritual_v01.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\walls
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\walls\act1_wall_corner_L_NE_v01.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\walls\act1_wall_corner_L_NE_v01.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\walls\act1_wall_cyan_rift_integrated_v01.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\walls\act1_wall_cyan_rift_integrated_v01.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\walls\act1_wall_partition_low_stub_v01.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\walls\act1_wall_partition_low_stub_v01.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\walls\act1_wall_straight_horizontal_v01.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\walls\act1_wall_straight_horizontal_v01.png.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\walls\painted_v01
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\walls\painted_v01.meta
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\walls\painted_v01\walls_set_v01.png
Assets\Art\AssetPacks\_archive\act1_legacy_topdown_s95\walls\painted_v01\walls_set_v01.png.meta
`

## Git status scoped
`
 M Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_arch_opening_v01.png.meta
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03.meta
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_A_granite_v01_tile_0.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_A_granite_v01_tile_0.asset
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_A_granite_v01_tile_0.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_A_granite_v01_tile_0.asset.meta
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_A_granite_v01_tile_1.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_A_granite_v01_tile_1.asset
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_A_granite_v01_tile_1.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_A_granite_v01_tile_1.asset.meta
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_A_granite_v01_tile_2.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_A_granite_v01_tile_2.asset
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_A_granite_v01_tile_2.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_A_granite_v01_tile_2.asset.meta
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_A_granite_v01_tile_3.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_A_granite_v01_tile_3.asset
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_A_granite_v01_tile_3.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_A_granite_v01_tile_3.asset.meta
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_B_cracked_v01_tile_0.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_B_cracked_v01_tile_0.asset
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_B_cracked_v01_tile_0.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_B_cracked_v01_tile_0.asset.meta
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_B_cracked_v01_tile_1.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_B_cracked_v01_tile_1.asset
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_B_cracked_v01_tile_1.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_B_cracked_v01_tile_1.asset.meta
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_B_cracked_v01_tile_2.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_B_cracked_v01_tile_2.asset
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_B_cracked_v01_tile_2.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_B_cracked_v01_tile_2.asset.meta
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_B_cracked_v01_tile_3.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_B_cracked_v01_tile_3.asset
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_B_cracked_v01_tile_3.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_B_cracked_v01_tile_3.asset.meta
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_C_dirt_v01_tile_0.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_C_dirt_v01_tile_0.asset
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_C_dirt_v01_tile_0.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_C_dirt_v01_tile_0.asset.meta
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_C_dirt_v01_tile_1.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_C_dirt_v01_tile_1.asset
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_C_dirt_v01_tile_1.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_C_dirt_v01_tile_1.asset.meta
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_C_dirt_v01_tile_2.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_C_dirt_v01_tile_2.asset
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_C_dirt_v01_tile_2.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_C_dirt_v01_tile_2.asset.meta
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_C_dirt_v01_tile_3.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_C_dirt_v01_tile_3.asset
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_C_dirt_v01_tile_3.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_C_dirt_v01_tile_3.asset.meta
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_D_ritual_v01_tile_0.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_D_ritual_v01_tile_0.asset
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_D_ritual_v01_tile_0.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_D_ritual_v01_tile_0.asset.meta
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_D_ritual_v01_tile_1.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_D_ritual_v01_tile_1.asset
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_D_ritual_v01_tile_1.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_D_ritual_v01_tile_1.asset.meta
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_D_ritual_v01_tile_2.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_D_ritual_v01_tile_2.asset
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_D_ritual_v01_tile_2.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_D_ritual_v01_tile_2.asset.meta
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_D_ritual_v01_tile_3.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_D_ritual_v01_tile_3.asset
R  Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/floor_D_ritual_v01_tile_3.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03/floor_D_ritual_v01_tile_3.asset.meta
R  Assets/Data/Tiles/Act1_granite_low_v02.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02.meta
R  Assets/Data/Tiles/Act1_granite_low_v02/low_granite_var0_00.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_granite_var0_00.asset
R  Assets/Data/Tiles/Act1_granite_low_v02/low_granite_var0_00.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_granite_var0_00.asset.meta
R  Assets/Data/Tiles/Act1_granite_low_v02/low_granite_var1_01.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_granite_var1_01.asset
R  Assets/Data/Tiles/Act1_granite_low_v02/low_granite_var1_01.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_granite_var1_01.asset.meta
R  Assets/Data/Tiles/Act1_granite_low_v02/low_granite_var2_02.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_granite_var2_02.asset
R  Assets/Data/Tiles/Act1_granite_low_v02/low_granite_var2_02.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_granite_var2_02.asset.meta
R  Assets/Data/Tiles/Act1_granite_low_v02/low_granite_var3_03.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_granite_var3_03.asset
R  Assets/Data/Tiles/Act1_granite_low_v02/low_granite_var3_03.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_granite_var3_03.asset.meta
R  Assets/Data/Tiles/Act1_granite_low_v02/low_mossy_var0_08.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_mossy_var0_08.asset
R  Assets/Data/Tiles/Act1_granite_low_v02/low_mossy_var0_08.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_mossy_var0_08.asset.meta
R  Assets/Data/Tiles/Act1_granite_low_v02/low_mossy_var1_09.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_mossy_var1_09.asset
R  Assets/Data/Tiles/Act1_granite_low_v02/low_mossy_var1_09.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_mossy_var1_09.asset.meta
R  Assets/Data/Tiles/Act1_granite_low_v02/low_mossy_var2_10.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_mossy_var2_10.asset
R  Assets/Data/Tiles/Act1_granite_low_v02/low_mossy_var2_10.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_mossy_var2_10.asset.meta
R  Assets/Data/Tiles/Act1_granite_low_v02/low_mossy_var3_11.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_mossy_var3_11.asset
R  Assets/Data/Tiles/Act1_granite_low_v02/low_mossy_var3_11.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_mossy_var3_11.asset.meta
R  Assets/Data/Tiles/Act1_granite_low_v02/low_rift_var0_12.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_rift_var0_12.asset
R  Assets/Data/Tiles/Act1_granite_low_v02/low_rift_var0_12.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_rift_var0_12.asset.meta
R  Assets/Data/Tiles/Act1_granite_low_v02/low_rift_var1_13.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_rift_var1_13.asset
R  Assets/Data/Tiles/Act1_granite_low_v02/low_rift_var1_13.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_rift_var1_13.asset.meta
R  Assets/Data/Tiles/Act1_granite_low_v02/low_rift_var2_14.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_rift_var2_14.asset
R  Assets/Data/Tiles/Act1_granite_low_v02/low_rift_var2_14.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_rift_var2_14.asset.meta
R  Assets/Data/Tiles/Act1_granite_low_v02/low_rift_var3_15.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_rift_var3_15.asset
R  Assets/Data/Tiles/Act1_granite_low_v02/low_rift_var3_15.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_rift_var3_15.asset.meta
R  Assets/Data/Tiles/Act1_granite_low_v02/low_worn_var0_04.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_worn_var0_04.asset
R  Assets/Data/Tiles/Act1_granite_low_v02/low_worn_var0_04.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_worn_var0_04.asset.meta
R  Assets/Data/Tiles/Act1_granite_low_v02/low_worn_var1_05.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_worn_var1_05.asset
R  Assets/Data/Tiles/Act1_granite_low_v02/low_worn_var1_05.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_worn_var1_05.asset.meta
R  Assets/Data/Tiles/Act1_granite_low_v02/low_worn_var2_06.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_worn_var2_06.asset
R  Assets/Data/Tiles/Act1_granite_low_v02/low_worn_var2_06.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_worn_var2_06.asset.meta
R  Assets/Data/Tiles/Act1_granite_low_v02/low_worn_var3_07.asset -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_worn_var3_07.asset
R  Assets/Data/Tiles/Act1_granite_low_v02/low_worn_var3_07.asset.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02/low_worn_var3_07.asset.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_base.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_base.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_base/act1_floor_4mat_cave_moss_var00.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_base/act1_floor_4mat_cave_moss_var00.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_base/act1_floor_4mat_cave_moss_var01.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_base/act1_floor_4mat_cave_moss_var01.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_base/act1_floor_4mat_cave_moss_var02.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_base/act1_floor_4mat_cave_moss_var02.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_base/act1_floor_4mat_cave_moss_var03.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_base/act1_floor_4mat_cave_moss_var03.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_base/act1_floor_4mat_granite_var00.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_base/act1_floor_4mat_granite_var00.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_base/act1_floor_4mat_granite_var01.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_base/act1_floor_4mat_granite_var01.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_base/act1_floor_4mat_granite_var02.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_base/act1_floor_4mat_granite_var02.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_base/act1_floor_4mat_granite_var03.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_base/act1_floor_4mat_granite_var03.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_base/act1_floor_4mat_mud_var00.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_base/act1_floor_4mat_mud_var00.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_base/act1_floor_4mat_mud_var01.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_base/act1_floor_4mat_mud_var01.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_base/act1_floor_4mat_mud_var02.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_base/act1_floor_4mat_mud_var02.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_base/act1_floor_4mat_mud_var03.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_base/act1_floor_4mat_mud_var03.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_base/act1_floor_4mat_worn_stone_var00.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_base/act1_floor_4mat_worn_stone_var00.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_base/act1_floor_4mat_worn_stone_var01.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_base/act1_floor_4mat_worn_stone_var01.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_base/act1_floor_4mat_worn_stone_var02.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_base/act1_floor_4mat_worn_stone_var02.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_base/act1_floor_4mat_worn_stone_var03.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_base/act1_floor_4mat_worn_stone_var03.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_low_topdown_v02.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_low_topdown_v02.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_low_topdown_v02/act1_floor_granite_var0_00.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_low_topdown_v02/act1_floor_granite_var0_00.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_low_topdown_v02/act1_floor_granite_var1_01.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_low_topdown_v02/act1_floor_granite_var1_01.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_low_topdown_v02/act1_floor_granite_var2_02.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_low_topdown_v02/act1_floor_granite_var2_02.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_low_topdown_v02/act1_floor_granite_var3_03.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_low_topdown_v02/act1_floor_granite_var3_03.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_low_topdown_v02/act1_floor_mossy_var0_08.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_low_topdown_v02/act1_floor_mossy_var0_08.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_low_topdown_v02/act1_floor_mossy_var1_09.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_low_topdown_v02/act1_floor_mossy_var1_09.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_low_topdown_v02/act1_floor_mossy_var2_10.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_low_topdown_v02/act1_floor_mossy_var2_10.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_low_topdown_v02/act1_floor_mossy_var3_11.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_low_topdown_v02/act1_floor_mossy_var3_11.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_low_topdown_v02/act1_floor_rift_var0_12.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_low_topdown_v02/act1_floor_rift_var0_12.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_low_topdown_v02/act1_floor_rift_var1_13.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_low_topdown_v02/act1_floor_rift_var1_13.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_low_topdown_v02/act1_floor_rift_var2_14.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_low_topdown_v02/act1_floor_rift_var2_14.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_low_topdown_v02/act1_floor_rift_var3_15.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_low_topdown_v02/act1_floor_rift_var3_15.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_low_topdown_v02/act1_floor_worn_var0_04.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_low_topdown_v02/act1_floor_worn_var0_04.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_low_topdown_v02/act1_floor_worn_var1_05.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_low_topdown_v02/act1_floor_worn_var1_05.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_low_topdown_v02/act1_floor_worn_var2_06.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_low_topdown_v02/act1_floor_worn_var2_06.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_low_topdown_v02/act1_floor_worn_var3_07.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_low_topdown_v02/act1_floor_worn_var3_07.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/painted_v03.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/painted_v03.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/painted_v03/floor_A_granite_v01.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/painted_v03/floor_A_granite_v01.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/painted_v03/floor_B_cracked_v01.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/painted_v03/floor_B_cracked_v01.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/painted_v03/floor_C_dirt_v01.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/painted_v03/floor_C_dirt_v01.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/painted_v03/floor_D_ritual_v01.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/painted_v03/floor_D_ritual_v01.png.meta
RM Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_corner_L_NE_v01.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/walls/act1_wall_corner_L_NE_v01.png.meta
RM Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_cyan_rift_integrated_v01.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/walls/act1_wall_cyan_rift_integrated_v01.png.meta
RM Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_partition_low_stub_v01.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/walls/act1_wall_partition_low_stub_v01.png.meta
RM Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_straight_horizontal_v01.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/walls/act1_wall_straight_horizontal_v01.png.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/painted_v01.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/walls/painted_v01.meta
R  Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/painted_v01/walls_set_v01.png.meta -> Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/walls/painted_v01/walls_set_v01.png.meta
R  Assets/Scenes/Demo/RoomPipelineTest.unity -> Assets/_ARCHIVE/Scenes/RoomPipelineTest.unity
R  Assets/Scenes/Demo/RoomPipelineTest.unity.meta -> Assets/_ARCHIVE/Scenes/RoomPipelineTest.unity.meta
?? Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test.meta
?? Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test/
`

## Notes / flags
- All tracked .meta/.asset/scene moves were done with git mv and show as renames in git status. Some root wall .png binaries were not under Git version control, so git mv refused those four PNGs; they were moved by filesystem rename to sit beside their git-moved .meta files. Their GUID preservation is carried by the .meta files.
- The task exact list and audit inventory specify 5 wall PNG total. Source still contains Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_arch_opening_v01.png and its .meta because that file is not in the exact move list or 41-PNG audit count. This is flagged for orchestrator decision.
- PathC_BaseTest.unity was not edited by this task.
