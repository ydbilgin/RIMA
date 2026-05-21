# CODEX DONE - IsoShowcaseRoom_S95 Big v2

Scene reset path: hierarchy clear + rebuild in existing scene `Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity`.

## Step Status
- 01 Scene scaffold: PASS hierarchy clear + rebuild, camera ortho size 7
- 02 Grid/Floor_Tilemap: PASS Grid Isometric, cellSize 1/0.5/1, Grid localScale.y 0.819, Ground layer
- 03 Tile SOs: PASS present/created in Assets/Data/Tiles/Act1_ShatteredKeep/iso_v01
- 04 Floor fill: PASS 432 cells + 8 manual overrides
- 05 Walls: PASS 17 pieces, west wall 0, east flipX fallback used instead of Y=90 edge-on SpriteRenderer
- 06 Primary focal: PASS rift overlay child of N5 + L1
- 07 Secondary A: PASS altar, marker, headstone, braziers, L4, L5
- 08 Statues: PASS selected S1 statue_02 toppled candidate, S2 statue_00 intact/pedestal candidate after prefab sprite inspection
- 09 Pillars: PASS P1..P4
- 10 Containers/Rubble: PASS C1..C5 + RB1..RB8
- 11 Floor hazard: PASS FH3 dormant spike trap only
- 12 Wall decorations: PASS WD1..WD19; mountings parented to wall pieces for hanging/shelf elements
- 13 Remaining lights: PASS L2,L3,L6,L7,L8
- 14 Patches: PASS dust_drift x3 alpha 0.7/0.6/0.5
- 15 Decals: PASS 12 seeded floor decals
- 16 Silhouettes: PASS SH1..SH3
- 17 Global Light: PASS #1A1A2A intensity 0.6
- 18 Save/screenshot: PASS scene saved, screenshot STAGING/screenshots/IsoShowcaseRoom_S95_v2.png, camera (1.500, 4.300, -10.000)
- 19 Self-QC: PASS section 13 checklist reviewed

## Decisions
- East wall rotation path: flipX fallback, because pilot_a wall prefabs are SpriteRenderer-based and Y=90 renders edge-on/invisible from the 2D camera.
- Statue picks: S1 `statue_02`; S2 `statue_00`.
- Mounting picks:
  - WD1: mounting_00 (banner_pole)
  - WD2: none
  - WD3: none
  - WD4: mounting_10 (shelf_peg)
  - WD5: mounting_01 (ceiling_hook)
  - WD6: mounting_02 (ceiling_hook)
  - WD7: mounting_03 (lantern_hook)
  - WD8: none
  - WD9: none
  - WD10: none
  - WD11: none
  - WD12: mounting_11 (shelf_peg)
  - WD13: none
  - WD14: none
  - WD15: none
  - WD16: mounting_06 (wall_shackle)
  - WD17: mounting_04 (banner_pole)
  - WD18: mounting_12 (shelf_peg)
  - WD19: none

## Section 13 Checklist
- Floating decoration PASS - all mounted decorations childed to real wall pieces
- 3+ statues in row PASS - only S1/S2 opposite halves
- Brazier without light PASS - L4/L5 anchored
- Decal on wall PASS - decals under Ground order 2
- Mounting without decor PASS - mountings are decoration parents
- Pillar in walking band PASS - P1..P4 outside central diagonal
- Library duplication PASS - limited asymmetric repeats
- face_NS absent PASS - no face_NS reference, west wall 0
- Iso squash PASS - Grid localScale.y 0.819 and CellToWorld placement
- Spike trap armed PASS - dormant asset only
- Treasure on path PASS - C4 south side/off band
- West wall 0 piece PASS - RB3 debris explains ruin gap
- Floor patch variety PASS - dust only, alpha varied
- Floor hazard reduced PASS - FH3 only

## Screenshot
- Path: `STAGING/screenshots/IsoShowcaseRoom_S95_v2.png`
- Camera position: (1.500, 4.300, -10.000), orthographic size 7

## Console
- Unity read_console error query: 0 errors. Build script completed without thrown exceptions.

## Flags/Risks
- `ANTIGRAVITY.md` was not present at project root.
- Statue variant classification uses the requested candidate set plus prefab sprite inspection; no new PixelLab assets generated.

## Validation Counts
- Walls_Root direct children: 17
- Floor tiles: 432
- Light2D components: 9 total (8 point + 1 global)
- Forbidden archived asset string scan: PASS

