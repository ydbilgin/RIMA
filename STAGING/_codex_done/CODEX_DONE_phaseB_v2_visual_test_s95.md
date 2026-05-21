# Faz B v2 Visual Test - Codex Report

## Onkosul Verify
- Wall pivot pixel: (64, 4) PASS
- Floor_Tilemap layer: Ground PASS
- Layer set: [Default, Ground, Walls, Entities, VFX] PASS
- Console: clean PASS

## Variant Results

### Variant 1 - OPUS VERDICT (Entities, Y-sort)
- Screenshot: STAGING/phaseB_v2_wall_seating_variant_1.png
- Wall foot: diamond lower edge YES
- Cube vs wall: cube onde YES (Y-sort dogru)
- Wall order: 4532
- Cube order: 4632
- Verdict: PASS

### Variant 2 - diagnostic (Entities, Y-sort, +0.25 Y)
- Screenshot: STAGING/phaseB_v2_wall_seating_variant_2.png
- Wall foot: diamond lower edge NO (wall too high)
- Verdict: FAIL

### Variant 3 - diagnostic (Entities, Y-sort, -0.25 Y)
- Screenshot: STAGING/phaseB_v2_wall_seating_variant_3.png
- Wall foot: diamond lower edge NO (wall too low)
- Verdict: FAIL

### Variant 4 - diagnostic (Ground, fixed -100)
- Screenshot: STAGING/phaseB_v2_wall_seating_variant_4.png
- Wall foot: diamond lower edge YES
- Cube vs wall: cube onde YES, but wall is on Ground so occluder behavior is wrong
- Verdict: FAIL

### Variant 5 - diagnostic (Entities, fixed 0)
- Screenshot: STAGING/phaseB_v2_wall_seating_variant_5.png
- Wall foot: diamond lower edge YES
- Cube vs wall: sorting is not tied to Y; fixed wall order creates incorrect depth behavior
- Verdict: FAIL

## Summary
- Best fit: Variant 1
- Opus verdict confirmed: YES
- Acik tartisma: none

## Cleanup
- WallSeatingTest_S95v2 deleted: YES
- Camera restored: YES
- Scene dirty: NO
