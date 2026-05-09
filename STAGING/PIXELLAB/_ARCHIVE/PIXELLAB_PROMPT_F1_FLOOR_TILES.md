# PixelLab Prompt — F1 Floor Tiles (UPDATED 2026-05-07)

## Tool
PixelLab MCP — `create_isometric_tile` (geometry/angle/size handled by tool params)

## Key Insight
Tool handles isometric type, size, and angle. Prompt = ONLY visual appearance.
Do NOT include: isometric, diamond shape, face lighting, 64x64, top-down angle specs.
These caused 3D-render artifacts in previous batches.

## Usage
Call `create_isometric_tile` 16 times. Each call uses the FIXED BASE + one SURFACE_DETAIL from the list below.

---

## Fixed Base (constant across all 16 calls)

```
Dark ancient hand-cut slate dungeon floor stone.
[SURFACE_DETAIL]
Hand-painted pixel art — visible pixel grain, dithered 2-3 pixel
transitions, no smooth gradients, no soft shadows.
Dominant palette: #0d0e17, #111320, #1a1d2e, #12151f, #1f2233.
Organic exceptions: moss #2a3020–#1e2818, wet stone #141a20,
crystal #252535. No pure white, no pure black.
Edge pixels #0d0e17 (seamless tiling).
```

---

## 16 Surface Detail Variants (replace [SURFACE_DETAIL] per call)

1.  Plain worn stone, smooth from centuries of foot traffic
2.  Hairline crack running diagonally corner to corner
3.  Fine chisel marks, parallel grooves
4.  Subtle grip grooves, deliberate stonework
5.  Spiderweb fracture pattern radiating from center
6.  Dark moss patch in two corners, desaturated olive
7.  Water stain ring, dried mineral deposit edge
8.  Chipped and eroded at one edge, crumbled corner
9.  Shallow carved rune, barely legible
10. Arcane sigil fragment, partial circle
11. Faded ward circle, concentric thin lines
12. Boot-worn hollow, slightly depressed center
13. Dense moss spread across half the surface
14. Scorch mark, carbon smear from old fire
15. Pale crystal vein, thin diagonal streak
16. Dark mold bloom, irregular organic patch

---

## Why Previous Batches Failed

| Batch | Problem | Result |
|---|---|---|
| Batch 1 | "MAXIMUM diversity", no craft keywords | Varied but inconsistent lighting |
| Batch 2 | Per-face lighting specs (#NW light, left face #X) | 3D render / plastic look |
| **This version** | Tool handles geometry; prompt = material + craft + palette only | Target: authentic pixel art |

## Lessons
- Face-lighting geometry in prompt = PixelLab renders like 3D shader, not pixel art
- "Dithered transitions", "pixel grain", "hand-painted" = pixel art mode
- Organic color exceptions (moss, wet, crystal) needed for visual variation
- Redundant geometry specs (isometric, 64px, angle) cause conflicts with tool params

## Output Target
- 16 individual tiles via MCP
- Assemble into 4×4 sheet: `Assets/Art/Tiles/Act1/f1variants.png`
- Slicing: 64×64 grid, no gaps