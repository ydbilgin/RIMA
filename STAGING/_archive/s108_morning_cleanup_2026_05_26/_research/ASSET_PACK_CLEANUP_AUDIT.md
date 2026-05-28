# Asset Pack Cleanup Audit - 2026-05-21

Scope: `Assets/Art/AssetPacks/Act1_ShatteredKeep/`

Confirmed archive already executed:
- `decals/act1_decal_dust_var0_08.png` -> ARCHIVE, user-explicit dust removal.
- `decals/act1_decal_dust_var1_09.png` -> ARCHIVE, user-explicit dust removal.
- `decals/act1_decal_dust_var2_10.png` -> ARCHIVE, user-explicit dust removal.
- `decals/act1_decal_dust_var3_11.png` -> ARCHIVE, user-explicit dust removal.
- `patches/act1_patch_dust_drift_v01.png` -> ARCHIVE, user-explicit dust removal.

Archive destination:
`Assets/Art/AssetPacks/Act1_ShatteredKeep/_archive/dust_removed_2026_05_21/`

## Additional Removal Candidates

| Path | Reason | Verdict |
|---|---|---|
| `decor_silhouettes/decor_silhouette_cyan_slime_v01.png` | Enemy silhouette decor reads like a leftover mob placeholder, not a grounded combat-room prop. It also overlaps the already-removed mob bucket conceptually. | ARCHIVE pending user approval |
| `decor_silhouettes/decor_silhouette_cyan_wisp_v01.png` | Same issue: abstract enemy silhouette has unclear placement rules and can read as random scatter instead of authored dungeon atmosphere. | ARCHIVE pending user approval |
| `decor_silhouettes/decor_silhouette_goblin_v01.png` | Goblin silhouette is off-identity for the current RIMA Act 1 keep mood and risks generic fantasy drift. | ARCHIVE pending user approval |
| `decor_silhouettes/decor_silhouette_husk_v01.png` | Could fit if treated as a wall stain/shadow, but currently named and grouped as a monster silhouette, so placement intent is unclear. | ARCHIVE pending user approval |
| `decor_silhouettes/decor_silhouette_imp_demon_v01.png` | Demon/imp silhouette pushes generic bestiary flavor and does not match the canonical Act 1 prop/decal list. | ARCHIVE pending user approval |
| `decor_silhouettes/decor_silhouette_rat_king_v01.png` | Reads as a stray enemy concept rather than architecture, ritual, rubble, or wall dressing. | ARCHIVE pending user approval |
| `decor_silhouettes/decor_silhouette_specter_ghost_v01.png` | Specter silhouette can be atmospheric, but as a loose decor asset it risks random haunted-house scatter. | REGEN-LATER pending user approval |
| `decor_silhouettes/decor_silhouette_wraith_specter_v01.png` | Redundant with the specter silhouette and not tied to an explicit room use case. | REGEN-LATER pending user approval |
| `decals/act1_decal_pebble_var0_04.png` | Four pebble decals are close to the archived dust problem: low-information scatter that can clutter combat rooms. | KEEP for now; archive only if user wants stricter decal cleanup |
| `decals/act1_decal_pebble_var1_05.png` | Same functional group redundancy as other pebble variants. | KEEP for now; archive only if user wants stricter decal cleanup |
| `decals/act1_decal_pebble_var2_06.png` | Same functional group redundancy as other pebble variants. | KEEP for now; archive only if user wants stricter decal cleanup |
| `decals/act1_decal_pebble_var3_07.png` | Same functional group redundancy as other pebble variants. | KEEP for now; archive only if user wants stricter decal cleanup |

## Suspicious Cases Needing User Approval

- `decor_silhouettes/*`: strongest archive candidate group. These feel like inherited mob placeholders and are not in the explicit keep list.
- `decals/act1_decal_pebble_var*.png`: lower severity than dust. Keep if small stone scatter is still desired; archive if the asset pack should remove all weak floor noise.
- `props/act1_prop_lever_wall_v01.png` and `props/act1_prop_spike_trap_dormant_v01.png`: keep for now. They are not in the explicit keep list, but both are coherent dungeon gameplay props and should not be archived without a trap/interaction design decision.
- `wall_decoration/act1_trophy_*.png`, `act1_wall_candle_bracket_v02.png`, `act1_wall_torch_sconce_v02.png`: keep for now. They are extra wall dressing, but they fit the dungeon context.

## Reference Check

Dust GUIDs were searched in `*.unity`, `*.prefab`, and `*.asset`.

Found references:
- `Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity` references `act1_patch_dust_drift_v01.png` GUID `715edde31444b9b40948e4eb19f69963` at lines 1342, 4416, 10451, and 24212.

Because the `.meta` file moved with the PNG, the GUID is preserved and those scene references should continue to resolve to the archived asset. No references were found for the four dust decal GUIDs.
