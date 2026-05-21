# DungeonCombatV3 Codex Report

## Step PASS/FAIL
- PASS: Loaded Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity.
- PASS: Found existing scene root, Grid, Floor_Tilemap, Main Camera, Walls_Root, WallDecorations_Root, Props_Root, Patches_Root, Decals_Root, Silhouettes_Root, Lights_Root, PlayerSpawn.
- PASS: Props_Root subgroup containers were missing in the live scene and were created under the existing Props_Root: Pillars, Ritual, Statues, Containers, Rubble, Floor_Hazards.
- PASS: Cleared only designated content roots and rebuilt the room.
- PASS: Floor_Tilemap filled with 320 cells using the new isometric_v01 tile bank and DungeonCombatV3 seed.
- PASS: Manual floor hot spots applied: altar twin, north rift threshold, south entry threshold, toppled statue zone.
- PASS: Walls built: 14 total, west wall 0 pieces, east wall uses flipX and no new Z rotation.
- PASS: Props built: 4 pillars, 3 ritual objects, 2 braziers, 2 statues, 4 containers, 5 rubble/scatter, 1 dormant spike trap.
- PASS: Wall decorations built: 12 holders, 12 decoration sprites, 12 mounting prefabs.
- PASS: Lights built: 6 point Light2D plus 1 Global Light2D under Lights_Root.
- PASS: Patches built: 3 dust drift sprites.
- PASS: Decals built: 10 floor decals.
- PASS: Silhouettes built: 3 sprites.
- PASS: Rift fracture overlay added as child of N5 arch with RiftPulse2D.
- PASS: Main Camera reused, positioned at (1.5, 4.3, -10), orthographic size 7.
- PASS: Screenshot produced at STAGING/screenshots/DungeonCombatV3_first.png.
- PASS: Post-build read_console error count is 0.

## Decisions
- S1 toppled statue variant: statue_05.
- S2 intact pedestal statue variant: statue_00.
- WD1 mounting: mounting_00.
- WD2 mounting: mounting_01 supplemental torch anchor.
- WD3 mounting: mounting_01 supplemental torch anchor.
- WD4 mounting: mounting_10.
- WD5 mounting: mounting_06.
- WD6 mounting: mounting_01.
- WD7 mounting: mounting_03.
- WD8 mounting: mounting_02.
- WD9 mounting: mounting_03 supplemental candle anchor.
- WD10 mounting: mounting_01 supplemental torch anchor.
- WD11 mounting: mounting_12.
- WD12 mounting: mounting_04.

## Sacmalik Checklist
- PASS: Floating deco - all 12 wall decorations are positioned from real wall piece coordinates and paired with mounting prefabs.
- PASS: Statue lining - S1 placed at cx=5, cy=11 with impact offset; S2 placed at cx=15, cy=13.
- PASS: Brazier without light - B1 has L5 cyan; B2 has L4 orange.
- PASS: Decal on wall - 10 decals are under Decals_Root on Ground layer/order 2 at floor-region coordinates.
- PASS: Pillar walking band - pillars are on edge/frame cells cx=2/17, not center walking band.
- PASS: West wall 0 piece - no west wall objects were created; RB3 explains the ruin gap.
- PASS: face_NS reference YOK - rg face_NS/pilot_a_frame_0/wall_face_NS returned 0 scene references.
- PASS: Active spike trap - only act1_prop_spike_trap_dormant_v01 was placed; no armed trap asset/script was added.

## Counts
- Floor tiles: 320.
- Walls: 14.
- Pillars: 4.
- Ritual group children: 5 total = 3 ritual + 2 brazier.
- Statues: 2.
- Containers: 4.
- Rubble/scatter: 5.
- Hazards: 1.
- Wall decoration holders: 12.
- Wall decoration SpriteRenderers: 24 = 12 deco + 12 mounting.
- Patches: 3.
- Decals: 10.
- Silhouettes: 3.
- Lights: 7 = 6 point + 1 global.

## Outputs
- Scene: Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity.
- Screenshot path: STAGING/screenshots/DungeonCombatV3_first.png.
- UnityMCP screenshot source path: Assets/Screenshots/STAGING_screenshots_DungeonCombatV3_first.png.
- Console error count: 0.
