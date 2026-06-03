# Scene V2 Report

## Phase Summary

Phase 0 - Reference and scene read
- Read `STAGING/s106_overnight/walless_v1_batch2_M3.png` as the target.
- Opened `Assets/Scenes/Test/PlayableArena.unity` in the Unity editor.
- Verified the old floor state was 2533 tiles across x=-22..75, y=-35..24 and not close to the compact floating reference.

Phase 1 - Compact arena repaint
- Cleared the existing Floor/Tilemap completely.
- Repainted a compact iso-cell arena using the requested oval mask: `abs(x)/6 + abs(y)/3.5 < 1`.
- Final tilemap is 41 painted cells with bounds x=-5..5, y=-3..3.
- Final mix: 5 ritual rune cells, 20 cyan vein cells, 16 cobblestone cells, 0 dirt.
- Tilemap was offset locally to center the painted floor around world origin.

Phase 2 - Player, camera, and rigs
- Player moved to (0, 0, 0).
- Main Camera moved to (0, -0.5, -10), orthographic size 3.5.
- Pixel Perfect Camera remains disabled.
- CameraFollow target field was set to Player.
- RoomBackgroundRig and CliffRing are at world origin.

Phase 3 - Cliff ring placement
- Reused the existing 24 CliffRing children.
- Reassigned them to 6 north, 6 south, 4 west, 4 east, and 4 corner cliff sprites.
- Set cliff SpriteRenderers to sortingLayer `Ground`, sortingOrder -50.
- Positioned the ring around the new compact tilemap bounds so the cliff faces hang down into the void.

Phase 4 - Kit C background placement
- Placed BG layers at the requested coordinates:
  - L0_Void: (0, 0, 30)
  - L1_Nebula: (0, 2, 25)
  - L2_Ruins: (0, 5, 20)
  - L3_Island_Small: (-7, 4, 15), scale (0.4, 0.4, 1)
  - L3_Island_Large: (8, 5, 15), scale (0.6, 0.6, 1)
  - L4_Fog: (0, -2, 10)
- BG layers use the built-in Sprites-Default material with darker tints so the nebula stays visible even with dim global light.

Phase 5 - Lights
- Warm Torch Light NW and NE placed near the upper arena corners with intensity 1.5 and radius 1.45.
- Cyan Crystal Light SW and SE placed near the lower arena corners with intensity 1.25 and radius 1.25.
- Global Light 2D set to intensity 0.3.

Phase 6 - Captures
- Game View render: `STAGING/s106_overnight/scene_v2_match_attempt.png` at 1280x720.
- Scene overview render: `STAGING/s106_overnight/scene_v2_match_attempt_scene.png` at 1280x720.
- Side-by-side comparison: `STAGING/s106_overnight/scene_v2_vs_M3.png` at 2560x720, implementation on left and M3 reference on right.

## Console Status

Final console recheck after scene save and captures: 0 errors, 0 warnings.

## Self Assessment vs M3

Matches:
- The huge 2533-cell floor is gone; the arena is now compact and centered.
- The scene now reads as a floating island in a visible void/nebula background.
- Cliff faces surround the perimeter instead of being scattered in the center.
- Cyan rune/vein accents and warm top-corner lighting are present.
- Camera, player, BG rig, and cliff rig are aligned around origin.

Still not close enough:
- The M3 reference has a much larger, cleaner stone platform with a strong central cyan ring. The current Kit A tiles can only approximate this with individual rune/vein tiles.
- The M3 reference has four clear orange brazier props/light pools. This pass only reuses the existing two warm and two cyan lights; no separate brazier prop sprites were available in the allowed inputs.
- The cliff sprites are very tall relative to the 41-cell floor, so the arena reads more vertical and enclosed than M3.
- The reference has stronger purple lightning at the edges; the reused Kit C layers provide more cyan nebula than purple lightning in the final camera crop.

## Suggested Next Iteration

- Add or reuse actual brazier prop sprites at all four corners, with four warm orange point lights, while keeping the cyan crystal pulse as a secondary accent.
- Either expand the floor to a larger 14x9 mask or zoom/crop differently if the goal is closer M3 visual mass rather than the current compact 12x7 spec.
- Add a dedicated central cyan circle decal/sprite or tile cluster; individual rune tiles cannot reproduce the M3 focal ring.
- Tune or replace Kit C edge layers to bring back purple lightning on the left and right borders.
