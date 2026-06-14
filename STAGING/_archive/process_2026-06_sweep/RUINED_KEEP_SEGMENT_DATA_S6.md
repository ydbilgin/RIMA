# Ruined Keep — authored perimeter segments (mapped to ACTUAL floor 19×14)

**Floor (verified via MCP):** Grid cellSize=1.0u; tilemap cellBounds min(-9,-5) max(10,9) → usable cells **x ∈ [-9, 9]** (19 cols), **y ∈ [-5, 8]** (14 rows), center ≈ (0.5, 2). Arch centered at **x=0**. 1 cell = 1 world unit = 64px @PPU64.
Wall foot = pivot bottom-center placed AT the perimeter cell; sprite rises up/back (192px = 3 cells). N wall faces south (toward player) = flat-front sprites perfect. E/W = flat-front sprites tiled along Y (depth), Custom-Axis Y-sort overlaps them into a receding side wall.

> These are the BuildRun / single-place calls for STEP 6. Build N first, screenshot, then E/W, then S. Adjust ranges to the floor's true painted edge (may be jagged) after the first screenshot.

## NORTH (back) wall — foot row y=8 — CONTINUOUS (the headline "walls connect" line)
| piece | kind | from→to (cell) | call |
|---|---|---|---|
| corner_buttress | Anchor | (-9,8) | PlaceOne |
| wall_run_mid | SolidWall | (-8,8)→(-3,8) | BuildRun (6) |
| pillar_tall | Anchor | (-2,8) | PlaceOne (flank arch L) |
| arch_gate | Entrance | (0,8) | PlaceOne (centered) |
| pillar_tall | Anchor | (2,8) | PlaceOne (flank arch R) |
| wall_run_mid | SolidWall | (3,8)→(8,8) | BuildRun (6) |
| corner_buttress | Anchor | (9,8) | PlaceOne |

→ N reads as ONE unbroken edge-to-edge masonry line (buttress · run · pillar · arch · pillar · run · buttress).

## WEST (left) wall — col x=-9 — upper solid + 3-cell void gap (asymmetric)
| piece | kind | from→to | call |
|---|---|---|---|
| wall_run_mid | SolidWall | (-9,7)→(-9,3) | BuildRun (5) |
| — | VoidEdge | (-9,2)→(-9,0) | skip (3-cell gap, cyan rim) |
| wall_run_mid | SolidWall | (-9,-1)→(-9,-3) | BuildRun (3) |
| (y=-4,-5 open toward S corner) |

## EAST (right) wall — col x=9 — upper solid + 2-cell void gap (different position = asymmetry)
| piece | kind | from→to | call |
|---|---|---|---|
| wall_run_mid | SolidWall | (9,7)→(9,4) | BuildRun (4) |
| — | VoidEdge | (9,3)→(9,2) | skip (2-cell gap) |
| wall_run_mid | SolidWall | (9,1)→(9,-3) | BuildRun (5) |

## SOUTH (front) — y=-5 — ~80% OPEN VOID (floating-island edge + cyan rim) + low stubs only
| piece | kind | from→to | call |
|---|---|---|---|
| wall_run_low | Anchor | (-8,-5) | PlaceOne (corner stub) |
| wall_run_low | Anchor | (7,-5) | PlaceOne (corner stub) |
| — | VoidEdge | rest of S | open (camera clearance) |

## RATIO CHECK
Solid perimeter ≈ N(19) + W(8) + E(9) = 36 of ~58 perimeter cells ≈ 62% solid / 38% void → LOCK-compliant (60–75% solid). Fortress back+upper-sides, floating-island open front.

## POST-BUILD passes (STEP 7-9)
- 7: confirm faces fill top ~40% (runs 2.5c, buttress/pillar 3c, arch 4c). Value split wall #3b3950 > floor #15131c.
- 8: contact shadow under every piece (#000 45%, 1.1×w × 0.35c oval). Wall torches every 4-6c (warm radial #ff9a24→dark). Banners IN FRONT of N face (separate sprites). Braziers flank arch. Rubble overlaps run bases.
- 9: swap 25-35% wall_run_mid → wall_run_cracked/low in-place (run line stays straight). Cyan rim ONLY on void edges (W gap, E gap, S front). cyan <15% pixels.
