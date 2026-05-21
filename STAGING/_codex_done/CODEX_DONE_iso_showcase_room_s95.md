# CODEX DONE - Iso Showcase Room Build (S95)

## Build Result
- Scene path: Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity
- Save status: PASS - scene saved clean after final hierarchy repair and screenshot refresh.
- Console: 0 errors, 0 warnings.

## Step Status (spec section 15 build order)
1. Scene scaffold - PASS. Created additive blank scene so dirty PathC_BaseTest stayed untouched; added camera and root hierarchy.
2. Grid + Floor_Tilemap - PASS. Grid is Isometric, cell size (1, 0.5, 1), Floor_Tilemap scale (1, 0.819, 1), Ground sorting.
3. Floor fill - PASS. Created 3 Tile assets, filled 80 cells, applied 8 manual overrides.
4. Walls - PASS. N1-N5, E1-E3, W1 placed. East wall Y=90 path kept after render test.
5. Primary focal reinforcement - PASS. Rift overlay, pulse component, L1 cyan Light2D.
6. Secondary focal A - PASS. Altar, marker, headstone, two braziers, L4/L5.
7. Secondary focal B - PASS. S1 toppled, S2 intact.
8. Pillars - PASS. P1-P4 placed outside walking band.
9. Containers + Rubble - PASS. C1-C4 and RB1-RB5 placed.
10. Floor hazards - PASS. FH1-FH3 placed; spike trap uses dormant asset.
11. Wall decorations + mountings - PASS. WD1-WD12 attached to real wall pieces with mounting pairs where required.
12. Wall sconce / brazier lights - PASS. L2/L3 added after correcting Light2D assembly lookup.
13. Patches - PASS. PA1-PA3 placed on Ground+1.
14. Decals - PASS. 12 seeded decals on floor only.
15. Silhouettes - PASS. SH1, SH2, SH3 placed at fringe/rift/exit positions.
16. Global light - PASS. Global Light2D ambient created with #1A1A2A, intensity 0.25.
17. Final pass - PASS. Scene saved, console clean, screenshot captured.
18. Self-QC - PASS with visual concern: final image is intentionally dark; rift/altar/statue read, but orchestrator may want brightness review.

## Decisions Made
- East wall rotation path: rotation Y=90 kept; test render showed readable wall/arch geometry, not edge-on.
- Statue picks: S1 = statue_06 (toppled debris/warrior read), S2 = statue_08 (intact warrior pedestal read).
- Mounting picks: WD1 mounting_04 (banner hook/pole plausible), WD4 mounting_11 (skull shelf/peg plausible), WD5/WD6 mounting_13 (ceiling hook), WD7 mounting_14 (lantern hook), WD10 mounting_05 (wall shackle).
- Tile assets: created 3 new Tile assets under Assets/Data/Tiles/Act1_ShatteredKeep/iso_v01/.
- Light2D assembly: project uses Unity.RenderPipelines.Universal.2D.Runtime, corrected after initial lookup returned no Light2D type.
- Screenshot isolation: temporarily hid non-S95 loaded scene roots during camera render only; PathC_BaseTest file was not saved or modified.

## Spec section 14 Sacmalik Check
- Floating decoration: PASS - WD1-WD12 are children of real wall pieces N1, N2, N4, N5, E1, E2, E3, W1.
- 3+ statues in row: PASS - exactly 2 statues, split west/northeast.
- Brazier without light: PASS - both braziers have L4/L5; torches have L2/L3; rift has L1.
- Decal on wall: PASS - decals are parented under Decals_Root and placed on floor cells only.
- Mounting prefab inside without decor pair: PASS - every mounting is paired with its corresponding decoration.
- Iso tile crack on focal hotspot: PASS - altar tiles forced to chiseled; walking band overrides clean.
- Pillar in walking path: PASS - pillars avoid (1,1), (5,2), (6,3), (8,4).
- Library too small / visible duplication: PASS - only pillar asset and torch repeat intentionally; no duplicate container spam.
- Missing wall pieces: PASS - asymmetric wall plan uses available pieces only.
- face_NS prefab absent: PASS - W1 built from pilot_a_frame_0_face_NS.png with existing pivot (0.5, 0.03125).
- Iso parent scale Y=0.819: PASS - Floor_Tilemap localScale is (1, 0.819, 1).
- Spike trap reading as armed: PASS - dormant filename/preview used.
- Treasure pile on path: PASS - C4 is offset south in cell (6,1), outside center walking band.

## Screenshot
- Path: STAGING/screenshots/IsoShowcaseRoom_S95_first_build.png
- Camera position: world (1.500, 1.838, -10.000)
- Visual notes: cyan rift is visible on north arch, warm altar/brazier focal is readable, toppled statue narrative beat is visible west/center-left, east exit wall reads. Overall render is dark due to specified low ambient.

## Flags / Risks for Orchestrator Review
- Final screenshot reads as the intended Forgotten Rift Antechamber, not a random asset dump.
- Visual risk: brightness is low. This follows the specified ambient/light hierarchy, but may deserve art-direction review.
- Mounting risk: PixelLab mounting prefabs are not all pure mounts; selected lowest plausible variants and recorded choices above.
