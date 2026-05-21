# Mob PNG Reorganize - Codex Report

## Moved Files (16)
### Universal (8)
| # | Old -> New | GUID | Import OK |
|---|---|---|---|
| 1 | act1_mob_dungeon_rat_v01 -> _Universal/.../decor_silhouette_rat_v01 | eb4653386c871324da54cf54f94fd3ae | OK |
| 2 | act1_mob_bat_v01 -> _Universal/.../decor_silhouette_bat_v01 | 1c6ac5ebda250e9479cc6b2a77c81a8a | OK |
| 3 | act1_mob_giant_spider_v01 -> _Universal/.../decor_silhouette_spider_v01 | 3b3e6786918e1564ba19472eb1276ab0 | OK |
| 4 | act1_mob_bone_walker_v01 -> _Universal/.../decor_silhouette_bone_walker_v01 | 20aa4505ecb872444a361dd9ab616d32 | OK |
| 5 | act1_mob_ground_crawler_v01 -> _Universal/.../decor_silhouette_ground_crawler_v01 | 265260f8735cafa40a3d28ee86249de5 | OK |
| 6 | act1_mob_animated_skull_v01 -> _Universal/.../decor_silhouette_animated_skull_v01 | b5eab47464ddad94eb5f27d56263cf86 | OK |
| 7 | act1_mob_bone_hand_v01 -> _Universal/.../decor_silhouette_bone_hand_v01 | 2b9236d1a54fd0544a155240b17753b6 | OK |
| 8 | act1_mob_bone_archer_v01 -> _Universal/.../decor_silhouette_bone_archer_v01 | 020102a2d47bb6044bae44d0c6f890f4 | OK |

### Act1 (8)
| # | Old -> New | GUID | Import OK |
|---|---|---|---|
| 9 | act1_mob_cyan_slime_v01 -> Act1_ShatteredKeep/.../decor_silhouette_cyan_slime_v01 | 42bd8e9d7e871d6418c04714b5da8609 | OK |
| 10 | act1_mob_cyan_wisp_v01 -> Act1_ShatteredKeep/.../decor_silhouette_cyan_wisp_v01 | dabd6851f296a294e8cf5f3d948b3399 | OK |
| 11 | act1_mob_imp_demon_v01 -> Act1_ShatteredKeep/.../decor_silhouette_imp_demon_v01 | 11ab8ce60eaa28b48a3dc4e3e2cbb06a | OK |
| 12 | act1_mob_goblin_v01 -> Act1_ShatteredKeep/.../decor_silhouette_goblin_v01 | 59f9455a84add3e4da40cc5f2cfb57f3 | OK |
| 13 | act1_mob_wraith_specter_v01 -> Act1_ShatteredKeep/.../decor_silhouette_wraith_specter_v01 | 3372816b51705134e92df79c6ca8c71d | OK |
| 14 | act1_mob_husk_v01 -> Act1_ShatteredKeep/.../decor_silhouette_husk_v01 | 1d63202c4c7b08547b98e2c2d8b7c11b | OK |
| 15 | act1_mob_specter_ghost_v01 -> Act1_ShatteredKeep/.../decor_silhouette_specter_ghost_v01 | e661dc1d4381e3643a597967d3412438 | OK |
| 16 | act1_mob_rat_king_v01 -> Act1_ShatteredKeep/.../decor_silhouette_rat_king_v01 | 290a3cdc916c08549b8c68860f4a3b7a | OK |

## Old Folder Cleanup
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/mobs/`: removed (was empty after move)

## Verify
- AssetDatabase.MoveAsset executed for 16 files in the active Unity editor.
- 16 files moved, 16 GUIDs preserved against HEAD meta GUIDs.
- New target counts: 8 Universal PNGs, 8 Act1 PNGs.
- Import settings passed Unity and meta checks for all 16 files: Single, PPU 64, Point, alpha transparency, Clamp, bottom-center pivot, FullRect, maxTextureSize 256, Uncompressed.
- Explicit DefaultTexturePlatform, Standalone, and WebGL platform settings normalized to maxTextureSize 256 / Uncompressed.
- AssetDatabase.GetDependencies executed for all 16 moved files.
- Unity console error check after cleanup: clean.
- Git status scope: old `mobs.meta` and 16 old PNG metas deleted; two new `decor_silhouettes/` target folders and this report are untracked. PNG files are ignored by repo `.gitignore` rule `*.png`, but the files exist on disk.
- Auto-commit: not run.

## Output
- `STAGING/CODEX_DONE_mob_png_reorganize_s95.md` written.
- `CODEX_DONE_yasinderyabilgin.md` written as the last step.

## Acik Sorular
- None
