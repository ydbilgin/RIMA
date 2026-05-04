# RIMA S43 Production Guide -- Sprite & Animation Pipeline

## Direction System (Hades-Style, 8-Way Visuals)

RIMA uses 8 authored visual directions for player-facing clips. Movement input is
snapped to the nearest 45-degree sector by `PlayerAnimator`, and the last snapped
direction is preserved when movement stops.

Behavior rule:
- Move left -> `run_W` when available -> stop on `idle_W`
- Move right -> `run_E` when available -> stop on `idle_E`
- Move up-left -> `run_NW` when available -> stop on `idle_NW`
- Move down-right -> `run_SE` when available -> stop on `idle_SE`

| Visual direction | DirX | DirY | Sprite shown | Anchor reference |
|---|---|---|---|---|
| South | 0 | -1 | `_idle_south.anim` / `idle_S` | `south.png` |
| South-east | +1 | -1 | `_idle_SE.anim` | `south-east.png` |
| East | +1 | 0 | `_idle_east.anim` / `idle_E` | `east.png` |
| North-east | +1 | +1 | `_idle_NE.anim` | `north-east.png` |
| North | 0 | +1 | `_idle_north.anim` / `idle_N` | `north.png` |
| North-west | -1 | +1 | `_idle_NW.anim` | `north-west.png` |
| West | -1 | 0 | `_idle_west.anim` / `idle_W` | `west.png` |
| South-west | -1 | -1 | `_idle_SW.anim` | `south-west.png` |

## Anchor Reference Images

Location: `Characters/anchors/<class>/rotations/`
8 files per class, named by TRUE visual direction:

| File | Visual content |
|---|---|
| `south.png` | Full front face (character faces toward camera) |
| `south-east.png` | 3/4 front, slightly right |
| `east.png` | Pure right side profile |
| `north-east.png` | 3/4 back, right side visible |
| `north.png` | Full back (character faces away from camera) |
| `north-west.png` | 3/4 back, left side visible |
| `west.png` | Pure left side profile |
| `south-west.png` | 3/4 front, slightly left |

For the Hades-like pipeline, use all 8 anchors. Do not mirror east/west unless a
temporary placeholder is explicitly approved.

## Sprite Canvas Spec (S43)

- Canvas: 128x128 px
- Character occupies: ~80-100px height
- PPU (Pixels Per Unit): 64
- Pixel art rules: NO anti-aliasing, NO blur, hard pixel edges

## Sprite File Naming Convention

```
Assets/Sprites/Characters/<ClassName>/<classname>_idle_<direction>.png
```
Direction = `south` / `SE` / `east` / `NE` / `north` / `NW` / `west` / `SW`

## Animation Workflow (LOCKED)

- Tool: PixelLab UI "Animate with Text NEW" (manual, not MCP)
- Canvas: 252px, 8 frames, interpolation-v2 after
- `animate_character` MCP = FORBIDDEN (quality issues)
- Reference: use the correct anchor image for the direction being animated

## Unity Animator Setup

Parameters: Speed (float), DirX (float), DirY (float), IsDashing (bool), IsDead (trigger)

AnyState transitions for idle:
- Speed < 0.5 AND DirX between -0.5..0.5 AND DirY < -0.5 -> idle_S
- Speed < 0.5 AND DirX > 0.5 AND DirY < -0.5 -> idle_SE
- Speed < 0.5 AND DirX > 0.5 AND DirY between -0.5..0.5 -> idle_E
- Speed < 0.5 AND DirX > 0.5 AND DirY > 0.5 -> idle_NE
- Speed < 0.5 AND DirX between -0.5..0.5 AND DirY > 0.5 -> idle_N
- Speed < 0.5 AND DirX < -0.5 AND DirY > 0.5 -> idle_NW
- Speed < 0.5 AND DirX < -0.5 AND DirY between -0.5..0.5 -> idle_W
- Speed < 0.5 AND DirX < -0.5 AND DirY < -0.5 -> idle_SW

Future run transitions (when run clips are added):
- Speed > 0.5 AND DirX between -0.5..0.5 AND DirY < -0.5 -> run_S
- Speed > 0.5 AND DirX > 0.5 AND DirY < -0.5 -> run_SE
- Speed > 0.5 AND DirX > 0.5 AND DirY between -0.5..0.5 -> run_E
- Speed > 0.5 AND DirX > 0.5 AND DirY > 0.5 -> run_NE
- Speed > 0.5 AND DirX between -0.5..0.5 AND DirY > 0.5 -> run_N
- Speed > 0.5 AND DirX < -0.5 AND DirY > 0.5 -> run_NW
- Speed > 0.5 AND DirX < -0.5 AND DirY between -0.5..0.5 -> run_W
- Speed > 0.5 AND DirX < -0.5 AND DirY < -0.5 -> run_SW
