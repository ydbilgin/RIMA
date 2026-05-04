---
name: PixelLab Direction Mapping (Updated 2026-05-03)
type: feedback
trigger: direction, idle, east, west, north, south, anim, GUID, sprite, rotation
description: Canonical game-direction to sprite mapping for wave-1 classes. Hades 4-diagonal run system. flipX bug fixed.
---

## STATUS NOTICE (2026-05-04)

This file documents an existing/legacy direction implementation state. Current production source of
truth is `TASARIM/STYLE_BIBLE.md` + `TASARIM/MASTER_KARAR_BELGESI.md`: 35 degree ARPG, PPU=128,
4 cardinal production. Do not use this file for new asset production without an explicit direction
cleanup task.

## CANONICAL SYSTEM (2026-05-03 FINAL)

**Idle:** 8-way states `idle_S / idle_SE / idle_E / idle_NE / idle_N / idle_NW / idle_W / idle_SW`.
**Run (production):** 4 diagonal only: `run_SE / run_NE / run_NW / run_SW`. No cardinal run states.

PlayerAnimator snaps movement to nearest 45-degree sector. Preserves last snapped facing on stop.
Facing switch delay: adjacent = 0.05s, opposite = 0.10s (hysteresis to reduce hard cuts).

## DIRECTION -> ROTATION FILE MAPPING

Anchor rotation files corrected 2026-05-02 (PixelLab SW-facing shift fixed):

| Game Dir (code)    | Rotation file to use  |
|--------------------|-----------------------|
| east  (DirX=+1)    | south.png             |
| north-east (diag)  | south-east.png        |
| north (DirY=+1)    | east.png              |
| north-west (diag)  | north-east.png        |
| west  (DirX=-1)    | north.png             |
| south-west (diag)  | north-west.png        |
| south (DirY=-1)    | west.png              |
| south-east (diag)  | south-west.png        |

Rotation reference images: `Characters/anchors/<class>/rotations/`

## SPRITE VISUAL CONTENT (wave-1, verified 2026-05-02)

### Pattern A: Elementalist, Ranger, Shadowblade
- `*_idle_south.png` = full FRONT view      -> game SE diagonal
- `*_idle_east.png`  = LEFT side profile    -> game SOUTH
- `*_idle_north.png` = BACK view            -> game WEST
- `*_idle_west.png`  = RIGHT side profile   -> game NORTH

### Pattern B: Warblade
- `warblade_idle_south.png` = full FRONT view          -> game SE
- `warblade_idle_east.png`  = slightly-right-of-front  -> game EAST
- `warblade_idle_north.png` = 3/4 right-back           -> game NW diagonal
- `warblade_idle_west.png`  = pure BACK view           -> game WEST

## .anim -> SPRITE GUID MAPPING (IMPLEMENTED 2026-05-03)

### Elementalist, Ranger, Shadowblade:
- `*_idle_east.anim`  -> `*_idle_south.png` GUID
- `*_idle_north.anim` -> `*_idle_west.png`  GUID
- `*_idle_west.anim`  -> `*_idle_north.png` GUID
- `*_idle_south.anim` -> `*_idle_east.png`  GUID

### Warblade:
- `warblade_idle_east.anim`  -> `warblade_idle_east.png`  GUID
- `warblade_idle_north.anim` -> `warblade_idle_north.png` GUID
- `warblade_idle_west.anim`  -> `warblade_idle_west.png`  GUID
- `warblade_idle_south.anim` -> `warblade_idle_south.png` GUID

## flipX BUG: FIXED (2026-05-03)

`if (!controller.IsMoving) shouldFlip = lastCardinal.x < 0f;` block removed.
Each direction has its own distinct sprite. FlipX no longer needed for idle/movement facing.

## WARNINGS
- DO NOT use east.png GUID for east.anim on Elementalist/Ranger/Shadowblade
- DO NOT mirror east sprite for west -- west has its own sprite (back view)
- DO NOT add cardinal run states (S/E/N/W) for production -- 4-diagonal only
