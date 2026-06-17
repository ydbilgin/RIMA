# DONE capture_v3

Output folder: STAGING/_process/2026-06/demo_screenshots/capture_v3/

## Summary
- Screens captured: 35
- Duplicate retry count: 1 (28_buildmode_tile_erased.png first matched 26_buildmode_tile_tool.png; recaptured after visible tile submode change)
- Duplicate SHA groups after final audit: 0
- Capture method: Unity MCP game-view ScreenCapture path with overlay UI visible; SHA256 checked after captures.
- Source/scene edits: none. Runtime-only actions plus screenshot/report files.
- playModeStartScene restored: Assets/Scenes/UI/MainMenu.unity
- Final console status: 0 warnings, 0 errors from read_console.

## Manifest
| filename | screen-state | sha12 | unique |
|---|---|---:|---|
| 01_menu_mainmenu.png | main menu | E07EA67FB99E | yes |
| 02_menu_settings.png | main menu settings | 19C14536DC0E | yes |
| 03_menu_characterselect.png | character select with class cards | A389BCC0EC1F | yes |
| 04_fullflow_arena_opening_draft.png | full-flow arena opening skill draft/offer | 05F96EAD36AA | yes |
| 05_combat_empty_fullhp_hud.png | empty combat/full-HP HUD | 0D89B0B94573 | yes |
| 06_run_map_branching_open.png | run map branching overlay | C67462C7DC98 | yes |
| 07_pause_menu_open.png | pause menu | C8F425D8DF7A | yes |
| 08_settings_overlay_gameplay.png | gameplay settings overlay | FECF03315545 | yes |
| 09_codex_skill_codex_open.png | skill codex | 0CD7DE8F3C85 | yes |
| 10_character_sheet_tab.png | TAB character sheet | 8576F3E0D34C | yes |
| 11_combat_multi_enemy_5mobs.png | multi-enemy combat, 5 spawned mob types | 49CC7E513839 | yes |
| 12_combat_enemy_attack_projectiles.png | enemy attack/projectile state | 786948C108B5 | yes |
| 13_combat_player_lmb_vfx.png | player LMB/basic attack VFX state | 553C4DABEF8B | yes |
| 14_death_screen_enemy_kill.png | death screen from enemy kill | C88ACFD95BC1 | yes |
| 15_hud_mid_hp.png | mid-HP HUD | 0398E6B26C15 | yes |
| 16_hud_low_hp_vignette.png | low-HP HUD/vignette | 0D66EB4ABFDA | yes |
| 17_director_tab_spawn.png | Director Mode Spawn tab | 992308C1ECE7 | yes |
| 18_director_tab_class_skill.png | Director Mode Class/Skill tab | D915C074C006 | yes |
| 19_director_tab_stats_phys177.png | Director Mode Stats tab, physPower 177 | 89CD90F6BCBF | yes |
| 20_director_tab_prop_build_placed.png | Director Mode Build/prop tab with prop placed | 047F23CF2F45 | yes |
| 21_director_tab_map.png | Director Mode Map tab | CB7DFA8C414F | yes |
| 22_director_tab_telemetry.png | Director Mode Telemetry tab | 6C61A59D8CD2 | yes |
| 23_director_freecam_shifted.png | Director Mode free-cam shifted | 242F2BD93655 | yes |
| 24_buildmode_entry_prop_palette.png | Build Mode entry with prop palette | 58D2547BAEAA | yes |
| 25_buildmode_prop_placed.png | Build Mode prop placed | D87D55895EFD | yes |
| 26_buildmode_tile_tool.png | Build Mode tile tool | EA9B3383E9D8 | yes |
| 27_buildmode_tile_painted.png | Build Mode tile painted | 4467FD2D850D | yes |
| 28_buildmode_tile_erased.png | Build Mode erased/blocked tile state after retry | 01D29292914F | yes |
| 29_room_transition_portals_open.png | room transition/portals open | A173130DCE7C | yes |
| 30_merchant_shop_room_stands.png | merchant shop room with stands | 77B8FF79CED8 | yes |
| 31_elite_room.png | elite room | D78DB13C9782 | yes |
| 32_boss_spawn_full_hp_bar.png | boss spawn/full HP bar, P0 | A205A01D4855 | yes |
| 33_boss_mid_hp_phase_transition.png | boss mid HP/phase transition, P1 | 2933A5B0AEF4 | yes |
| 34_boss_attack_residue.png | boss attack/residue state | A0DBCBC1940B | yes |
| 35_boss_low_phase_p2.png | boss low HP/second threshold phase, P2 | 01997074B9F7 | yes |

## Enemy and Boss Combat Status
- Multi-enemy combat captured with 5 runtime-spawned mob prefabs: VoidThrall, SeamCrawler, FractureImp, HollowMite, RelicCaster.
- Enemy attack/projectile state captured as 12_combat_enemy_attack_projectiles.png.
- Player LMB/basic attack VFX state captured as 13_combat_player_lmb_vfx.png.
- Mid-HP, low-HP/vignette, and death states captured.
- Boss graph exit was not exposed from the reached elite room, so boss-specific states used the live PenitentSovereign prefab spawned at runtime.
- Boss captured: P0/full HP bar, P1/mid HP below 50 percent, P2/low HP below one-third threshold, and attack/residue state.
- No enemy-invisible/unbound blocker was detected during capture; screenshots are capture-truth evidence for visual review.

## Unreachable / Not Present
- ChamberSelect was not reached: CharacterSelect Start loaded _Arena directly in the live flow.
- Separate minimap/DungeonMapUI was not present in the loaded _Arena runtime; RunMapOverlay was captured instead.
- Chest, Forge, Event, Victory/DemoComplete were not exposed in the reachable live graph during this pass.
- Director light tab was not present in the runtime enum; available Director tabs captured were Spawn, ClassSkill, Stats, Build/prop, Map, Telemetry, plus free-cam.
- Boss room through the run graph was not exposed after merchant -> elite; boss states captured by runtime prefab spawn.

## Console
Final read_console(Error+Warning): 0 entries.
