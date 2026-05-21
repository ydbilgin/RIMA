# Faz B Visual Test — Wall Seating Codex Report

## Test Setup
- Sahne: PathC_BaseTest.unity (NOT SAVED — user manual save)
- Test parent: WallSeatingTest_S95 (created for test, then deleted)
- Base cell: (4, 4, 0), Grid.CellToWorld = (-70.9000, -45.3200, 0.0000)
- Asset: act1_wall_straight_horizontal_v01
- Requested pivot: (0.5, 0.0313)
- Unity loaded pivot: (64, 64) pixels on 128x128 sprite, i.e. center pivot, not requested bottom pivot
- Sorting layer note: requested `Floor` layer does not exist in project; Variant 4 used existing `Ground` layer as the floor-equivalent layer. No TagManager/project settings were modified.

## Variant Results

### Variant 1 — OPUS VERDICT (pivot 0.0313, offset 0, Entities, Y-sort)
- Screenshot: `STAGING/phaseB_wall_seating_v01_variant_1.png`
- Visual: wall appears seated with the imported center pivot, not the requested bottom pivot. The foot is not a clean confirmation of the diamond lower edge.
- Verdict: FAIL — Opus verdict not confirmable because Unity did not load the sprite with pivot (0.5, 0.0313).

### Variant 2 — +0.25 Y offset
- Screenshot: `STAGING/phaseB_wall_seating_v01_variant_2.png`
- Visual: wall is visibly higher relative to the target floor diamond than Variant 1.
- Verdict: FAIL — diagnostic wrong placement.

### Variant 3 — -0.25 Y offset
- Screenshot: `STAGING/phaseB_wall_seating_v01_variant_3.png`
- Visual: wall is visibly lower/sunk relative to the target floor diamond than Variant 1.
- Verdict: FAIL — diagnostic wrong placement.

### Variant 4 — Floor sortingLayer
- Screenshot: `STAGING/phaseB_wall_seating_v01_variant_4.png`
- Visual: requested `Floor` layer was unavailable; used `Ground`. Wall renders as floor/ground-layer diagnostic rather than proper entity Y-sort.
- Verdict: FAIL — sorting-layer diagnostic confirms this is not the correct entity wall configuration.

### Variant 5 — fixed sortingOrder 0
- Screenshot: `STAGING/phaseB_wall_seating_v01_variant_5.png`
- Visual: fixed order ignores wall Y position and is unsuitable for character occlusion.
- Verdict: FAIL — diagnostic wrong sorting.

## Summary
- Best fit: Variant 1 among tested variants, but only relative to the currently imported center pivot.
- Opus verdict confirmed: NO
- Açık tartışma: The `.meta` file contains `spritePivot: {x: 0.5, y: 0.03125}`, but Unity reports the loaded sprite pivot as `(64,64)` pixels. The importer has `alignment: 0`, so the custom pivot appears not to be active in the imported sprite. Asset `.meta` was not edited per constraint.

## Cleanup
- WallSeatingTest_S95 GameObject deleted: YES
- Camera transform restored: YES for scene asset camera; Unity Scene View was used for screenshots
- Scene dirty flag: NO (scene reloaded without save)
