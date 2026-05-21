# Wall Iso Placement Fix Report

## Selected Strategy

Selected Strategy A: back walls only.

Why: Map 1 and Map 2 were visually noisy because all four sides used full-height wall sprites with 1.3+ cell visual footprints. The repaint keeps full height on north/west back silhouettes, uses low walls/foundations on south/east front edges, and places multi-cell features only once. This preserves the room outline and leaves the floor visible.

## Map 1 Verdict

Scene: `Assets/Scenes/Demo/WallTest_Map1_Rectangle.unity`

Screenshot: `Assets/Screenshots/WallTest_Map1_v2_Rectangle.png`

| Connection / feature | v1 issue | v2 verdict |
|---|---|---|
| Rectangle silhouette | Full-height walls on all sides made the room read as cluttered wall mass. | IMPROVED |
| North/back wall | Hero feature was split by adjacent hero placement. | PASS |
| South/front entrance | Paired archway pieces overlapped and dominated the front edge. | IMPROVED |
| South/front edge | Full-height wall blocked floor readability. | PASS |
| East/right edge | Full-height wall blocked floor readability. | PASS |
| Interior column | Two columns crowded the room center. | PASS |
| Floor visibility | Walls dominated the screenshot. | PASS |

Remaining concern: the south archway is still a tall visual object by design, so it remains the strongest front-edge element. It is now a single sprite with space around it.

## Map 2 Verdict

Scene: `Assets/Scenes/Demo/WallTest_Map2_LShape.unity`

Screenshot: `Assets/Screenshots/WallTest_Map2_v2_LShape.png`

| Connection / feature | v1 issue | v2 verdict |
|---|---|---|
| L-shape silhouette | All-side full walls made the L turn hard to read. | IMPROVED |
| Inner corner | Recess was obscured by adjacent full-height pieces. | IMPROVED |
| North/top archway | Paired archway placement overlapped at the top-left connection. | PASS |
| South/front archway | Paired archway placement overlapped and blocked the front. | PASS |
| South/front edge | Full-height wall blocked floor readability. | PASS |
| East/right edge | Full-height wall blocked floor readability. | PASS |
| Interior column | Center column no longer competes with paired archways. | PASS |

Remaining concern: only `tile_corner_inner_a` was used for the L recess in this pass. `tile_corner_inner_b` still needs a mirrored-turn test.

## Gaps

- T-junction variants A/B/C/D were audited but not validated in a scene.
- Low wall endcap was audited but not tested beside an archway.
- Foundation B and floor edge were audited but not used in the v2 paints.
- Floor tile pivot correction was not applied because Strategy A does not require a global import-setting change.
- ITERATE_NEEDED: run one focused junction map before promoting these wall rules to production painting.

## Recommended Next Test

Create Map 3 as a junction stress test:

- 8 x 8 floor area with a north-west back-wall corner.
- One T-junction using each T variant in isolation.
- One low-wall front edge with endcaps around a single archway.
- One mirrored L-turn to compare `tile_corner_inner_a` against `tile_corner_inner_b`.

Pass criteria: no paired multi-cell overlaps, clear floor silhouette, and each junction variant has an assigned orientation rule.
