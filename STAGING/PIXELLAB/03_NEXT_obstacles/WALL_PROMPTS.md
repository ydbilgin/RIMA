# Wall Sprite Production Prompts — W1 (Act 1 Stone Walls)
*Companion to HOWTO.md. Copy-paste ready. Do not edit palette hex values.*

---

## Settings Reference (do not modify per call)

| Param | Value |
|---|---|
| Tool | Create Object (Create S-L Image Pro) |
| View | Low top-down (35 degree angle -- front face + top face visible) |
| Background | Transparent ON (no chromakey needed) |
| Pixel Art | ON |
| Upscale | OFF |
| Anti-aliasing | OFF |
| Canvas size | 128px square |
| Logical wall size | 64x128px (top face ~20px, front face ~110px) |
| Style Reference | Previous approved W1 tile (mandatory from var02 onward) |

> Wall sprites are 64x128 logical pixels. Canvas is 128px square.
> Top face occupies approximately the upper 20px strip; front face the lower 110px.
> Transparent background -- no process_tiles.py needed; drag directly to Unity.

---

## Shared Palette (W1 -- 5 locked colors)

```
Shadow / outline:  #1A1C20
Dark stone:        #2A2D34
Mid stone:         #3A3D48
Lit face:          #4E5260
Highlight:         #606575
```

---

## W1 — Dungeon Stone Wall Segment
**8 variants | Act 1 dungeon | Low top-down 35 degree view**

Block anatomy (applies to all W1 variants):
- Block size: approximately 16x12px, irregular (not perfectly uniform)
- Block mortar lines: 1-2px, color #1A1C20
- Top face (narrow strip, upper ~20px): dark stone #2A2D34, slight lit edge #3A3D48
- Front face (tall, lower ~110px): rough-cut stone blocks stacked in rows
- One block per variant slightly lighter or darker for variation

---

### W1-straight-H (horizontal straight wall section)

```
Pixel art dungeon stone wall segment, horizontal straight section, low top-down 35 degree view.
Top face (narrow, top ~20px): dark stone slab #2A2D34, slight lit edge #3A3D48.
Front face (tall, bottom ~110px): rough-cut stone blocks, stacked rows.
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Block mortar lines 1-2px #1A1C20. Block size ~16x12px irregular.
Subtle variation: one block slightly lighter/darker per segment.
Transparent background. Hard pixel edges, NO anti-aliasing.
```

---

### W1-straight-V (vertical straight wall section)

```
Pixel art dungeon stone wall segment, vertical straight section perpendicular to horizontal, low top-down 35 degree view.
Wall runs depth-wise from viewer perspective (receding into background).
Top face (narrow, upper strip): dark stone slab #2A2D34, lit edge #3A3D48.
Front face or side face visible: rough-cut stone blocks, stacked rows, same palette.
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Block mortar lines 1-2px #1A1C20. Block size ~16x12px irregular.
Subtle variation: one block slightly lighter/darker.
Transparent background. Hard pixel edges, NO anti-aliasing.
```

---

### W1-corner-NW (north-west outer corner)

```
Pixel art dungeon stone wall corner piece, NW outer corner junction, low top-down 35 degree view.
Two wall faces meet at 90 degree outer corner (convex). Left wall face and right wall face both partially visible.
Corner block is a large quoin stone (alternating long-short course pattern for structural look).
Top face visible on both arms, meeting at corner apex.
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Mortar lines 1-2px #1A1C20. Hard pixel edges, transparent background.
```

---

### W1-corner-NE (north-east outer corner)

```
Pixel art dungeon stone wall corner piece, NE outer corner junction, low top-down 35 degree view.
Two wall faces meet at 90 degree outer corner (convex). Mirrored from NW -- right arm extends right, left arm extends toward viewer.
Corner block is a large quoin stone (alternating long-short course).
Top face visible on both arms.
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Mortar lines 1-2px #1A1C20. Hard pixel edges, transparent background.
```

---

### W1-corner-SW (south-west inner corner)

```
Pixel art dungeon stone wall corner piece, SW inner corner junction, low top-down 35 degree view.
Two wall faces meet at 90 degree inner corner (concave). Interior recess visible.
Corner joint where blocks interlock; darker shadow in recess (#1A1C20 fill in concave zone).
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Mortar lines 1-2px #1A1C20. Hard pixel edges, transparent background.
```

---

### W1-corner-SE (south-east inner corner)

```
Pixel art dungeon stone wall corner piece, SE inner corner junction, low top-down 35 degree view.
Two wall faces meet at 90 degree inner corner (concave). Mirrored from SW.
Interior recess with deep shadow (#1A1C20) where faces meet.
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Mortar lines 1-2px #1A1C20. Hard pixel edges, transparent background.
```

---

### W1-end-L (wall end cap, left terminal)

```
Pixel art dungeon stone wall end cap, left terminal end, low top-down 35 degree view.
Front face visible, plus a perpendicular end face (left side).
End face uses quoin stone pattern: alternating long and short blocks at the terminal edge.
End face is slightly darker than front face (less direct light).
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Mortar lines 1-2px #1A1C20. Transparent background. Hard pixel edges, NO anti-aliasing.
```

---

### W1-end-R (wall end cap, right terminal)

```
Pixel art dungeon stone wall end cap, right terminal end, low top-down 35 degree view.
Front face visible, plus a perpendicular end face (right side).
End face uses quoin stone pattern: alternating long and short blocks at the terminal edge.
End face is slightly darker than front face (less direct light).
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Mortar lines 1-2px #1A1C20. Transparent background. Hard pixel edges, NO anti-aliasing.
```

---

## Production Order

1. W1-straight-H first (style anchor for all others)
2. W1-straight-V (load W1-straight-H as style reference)
3. W1-corner-NW and W1-corner-NE (pair, style ref: straight-H)
4. W1-corner-SW and W1-corner-SE (pair, style ref: corner-NW)
5. W1-end-L and W1-end-R (last, style ref: straight-H)

Generate 4 candidates per call, QC, pick 1, then move to next variant.
Do not batch all 8 in one session without QC stops.

---

## Unity Import (after selection)

- Folder: `Assets/Art/Props/Act1/Walls/W1/`
- Sprite Mode: Single
- Pivot: Bottom Center (0.5, 0.0)
- PPU: 64
- Sorting layer: Walls
- Filter: Point, Compression: None
- Collider: Polygon (Physics Shape) based on visible pixels only

---

## QC Checklist (per W1 variant before proceeding)

- [ ] Top face + front face both visible (low top-down 35 degree angle correct)
- [ ] Top face approximately 20px strip, front face approximately 110px tall
- [ ] Block mortar lines 1-2px color #1A1C20 (not thicker, not missing)
- [ ] Block size approximately 16x12px irregular (not perfectly uniform grid)
- [ ] Palette only: #1A1C20 / #2A2D34 / #3A3D48 / #4E5260 / #606575 (no drift)
- [ ] Transparent background (alpha 0 everywhere outside sprite)
- [ ] 128px square canvas
- [ ] Hard pixel edges, zero anti-aliasing fringe
- [ ] Corner pieces: both arms consistent with straight section block size
- [ ] End caps: quoin stone pattern visible on terminal edge
- [ ] Style reference was loaded for all variants after W1-straight-H
