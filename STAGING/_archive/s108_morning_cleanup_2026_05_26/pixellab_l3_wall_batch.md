# PixelLab MCP Batch -- Karar #143 L3 Wall Overlay Sprites
# ShatteredKeep (Act 1) Biome -- Hades-Style Perimeter Cap

**Date:** 2026-05-16
**Author:** rima-asset agent
**Layer:** L3 Wall Overlay (SpriteRenderer perimeter cap, NOT tileset)
**Architecture ref:** Karar #143 (6-layer map: L1 floor / L2 variation / L3 wall / L4 transition / L5 decal / L6 rift)
**Tool:** PixelLab MCP `create_object` endpoint

---

## Palette LOCK (reference these exact hex values in every prompt)

| Slot | Hex | Role |
|---|---|---|
| Deep void | #1A1C20 | Darkest stone body, crevice fill |
| Dark stone | #2A2D34 | Primary wall mass |
| Mid stone | #3A3D48 | Mid-tone surface, highlight band |
| Light stone | #4E5260 | Upper face light catch |
| Surface mid | #606575 | Chip highlights, mortar line catch |
| Ice-blue accent | #7BA7BC | Cyan mineral vein, rift hint (subtle only) |

**Negative palette rule:** NO pure white, NO warm orange/brown, NO bright green.
**ALL stone body pixels must be fully opaque** -- no alpha in the stone mass.

---

## Style Reference

```
style_image: Assets/Art/Tiles/F1/Tilesets/floor_wall/spritesheet.png
```

Pass this path as the `style_image` parameter in every MCP call. The reference shows dark stone with crack/vein detail and subtle blue mineral hints -- match grain, brushwork scale, and dither pattern exactly.

---

## View / Camera Lock

- **Angle:** High top-down 30-35 degrees (Hades match)
- **Reading axis:** Stone top face visible as a narrow receding ledge, full front face dominates the sprite height
- **Shadow direction:** Cast downward (toward south / toward viewer's camera) -- dramatic dark band at the inner/bottom edge where wall meets floor
- **Character camera rule:** This view matches the game camera; wall sprites must read correctly under that exact camera without any tilt correction

---

## Shared Prompt Header (paste into every prompt below)

> Hades-style painter pixel art wall sprite for a shattered keep dungeon. Top-down ~30-35 degree high overhead view. Painterly large brush strokes, organic IRREGULAR silhouette -- NO grid lines, NO repeating block pattern, NO uniform tile edge. Fully opaque stone body (#1A1C20 / #2A2D34 / #3A3D48 / #4E5260 / #606575), subtle ice-blue mineral vein accent (#7BA7BC, NOT dominant). Dramatic dark shadow band at the inner bottom edge where wall meets floor (sells height, Hades style). Crisp pixel-honest dithering, no anti-aliasing, no Gaussian blur, no smooth gradient, no modern UI sheen. Match the style_image reference exactly for stone grain and brushwork scale.

---

## Shared Negative Prompt (append to every MCP call)

```
avoid: bright colors, smooth gradient, Gaussian blur, modern UI chrome, repeating tileset grid pattern,
uniform rectangular block edges, anti-aliased rounded shapes, cartoon outline, warm brown palette,
transparent pixels inside stone body, symmetrical surface pattern
```

---

## SPRITE 1 -- Wall Horizontal Long Top Edge

**Description:** The most-used sprite. This is the top wall cap seen when the player looks northward -- the wall's front face dominates, with a narrow receding top ledge. Painted as a single organic shape, bottom edge irregular (jagged, chipped stone silhouette).

**Canvas:** 256 x 128 px
**Variants needed:** 4 (one n=16 call, pick 4 best OR four separate calls)
**Strategy:** Single n=16 call, pick 4 variants from result sheet. Label picked variants A/B/C/D.

### MCP Call -- Sprite 1

```json
{
  "tool": "create_object",
  "parameters": {
    "description": "Horizontal top wall cap sprite for a shattered keep dungeon, 256x128 pixels, Hades-style painter pixel art, top-down ~30-35 degree high overhead view. Full-width stone wall front face, narrow receding top ledge visible at the top of the sprite. ORGANIC IRREGULAR bottom silhouette -- jagged chipped stone edge, NO straight horizontal line, NO grid pattern. Painterly large brush strokes over the stone face. Stone palette #1A1C20 deep void / #2A2D34 primary mass / #3A3D48 mid-tone / #4E5260 light catch / #606575 chip highlights. Hairline ice-blue mineral vein accent #7BA7BC running diagonally across the face (subtle, NOT dominant). Dramatic dark shadow band (#1A1C20) at the bottom edge (inner face, floor-side) -- sells wall height. Left and right silhouette also irregular so sprites can butt together without forming a visible seam. Fully opaque stone interior, no alpha inside the wall body. Crisp pixel-honest dithering, no anti-aliasing, no smooth gradient.",
    "style_image": "Assets/Art/Tiles/F1/Tilesets/floor_wall/spritesheet.png",
    "sprite_size": [256, 128],
    "n": 16,
    "style_strength": 0.7,
    "negative_prompt": "bright colors, smooth gradient, Gaussian blur, modern UI chrome, repeating tileset grid pattern, uniform rectangular block edges, anti-aliased rounded shapes, cartoon outline, warm brown palette, transparent pixels inside stone body, symmetrical surface pattern, straight bottom edge"
  }
}
```

### Selection Criteria -- Sprite 1

Pick variants where:
1. Bottom silhouette is visibly irregular (jagged chips, no ruler-straight line)
2. No grid or regular block division visible on stone face
3. Dark shadow band clearly present at the bottom edge
4. Ice-blue vein is a hairline accent, not glowing or dominant
5. Left/right edges are organically ragged (no clean vertical cut)
6. Palette stays within the 6 locked hex values -- no warm tones sneaking in

Pick 4 best from the 16. Save to: `STAGING/TILESET_OUTPUT/L3_Wall_Horiz/wall_horiz_A.png` through `_D.png`.

---

## SPRITE 2 -- Wall Vertical Right/Left Edge

**Description:** The side wall cap -- seen on east and west room perimeters. Tall and narrow. Same Hades painter treatment. The "floor-facing" edge is on the inside (right edge for left wall, left edge for right wall) and must have the same irregular chipped silhouette and dark shadow band.

**Canvas:** 128 x 256 px
**Variants needed:** 4
**Strategy:** Single n=16 call, pick 4 best.

### MCP Call -- Sprite 2

```json
{
  "tool": "create_object",
  "parameters": {
    "description": "Vertical side wall cap sprite for a shattered keep dungeon, 128x256 pixels, Hades-style painter pixel art, top-down ~30-35 degree high overhead view. Tall narrow stone wall face, thin receding top ledge strip visible at the top edge. ORGANIC IRREGULAR inner silhouette (the floor-facing edge) -- jagged chipped stone, NO straight vertical line, NO grid pattern. Painterly large brush strokes across the tall stone face. Stone palette #1A1C20 / #2A2D34 / #3A3D48 / #4E5260 / #606575. Subtle ice-blue mineral vein #7BA7BC hairline on the stone face (NOT dominant). Dark shadow band (#1A1C20) along the floor-facing inner vertical edge -- sells depth. Top and bottom silhouette also organically irregular. Fully opaque stone interior. Crisp pixel dithering, no anti-aliasing, no smooth gradient. Works for both left-wall and right-wall placement (orchestrator will flip horizontally for opposite side).",
    "style_image": "Assets/Art/Tiles/F1/Tilesets/floor_wall/spritesheet.png",
    "sprite_size": [128, 256],
    "n": 16,
    "style_strength": 0.7,
    "negative_prompt": "bright colors, smooth gradient, Gaussian blur, modern UI chrome, repeating tileset grid pattern, uniform rectangular block edges, anti-aliased rounded shapes, cartoon outline, warm brown palette, transparent pixels inside stone body, straight inner edge, symmetrical surface pattern"
  }
}
```

### Selection Criteria -- Sprite 2

Pick variants where:
1. Inner (floor-facing) vertical silhouette is jagged and irregular
2. Dark shadow band visible along the inner edge
3. Stone face has large painterly brush grain, not small-tile pattern
4. Ice-blue vein hairline present but subtle
5. Top and bottom ends read as organically broken stone, not clean cuts
6. Palette within locked hex values

Pick 4 best. Save to: `STAGING/TILESET_OUTPUT/L3_Wall_Vert/wall_vert_A.png` through `_D.png`.

---

## SPRITE 3 -- Wall Corner NE

**Description:** Northeast exterior corner. Both the north wall cap and east wall cap meet here. Two stone faces visible: north-facing receding ledge and east-facing front face meeting at a corner mass. The inner concave corner (facing the room interior = SW direction) has the darkest shadow and the most irregular stone chip detail.

**Canvas:** 128 x 128 px
**Variants needed:** 1 (best pick from n=16)

### MCP Call -- Sprite 3

```json
{
  "tool": "create_object",
  "parameters": {
    "description": "Northeast wall corner sprite for a shattered keep dungeon, 128x128 pixels, Hades-style painter pixel art, top-down ~30-35 degree overhead view. Two stone wall faces meet at a corner: north face (top edge, receding narrow ledge) and east face (right side, full front face). Interior concave corner faces southwest (toward room) -- darkest shadow pool #1A1C20 here. ORGANIC IRREGULAR silhouettes on the two floor-facing edges (bottom and left inner edges) -- jagged chipped stone, NO straight lines, NO grid. Painterly large brush strokes. Stone palette #1A1C20 / #2A2D34 / #3A3D48 / #4E5260 / #606575. Ice-blue mineral vein hairline #7BA7BC on east face (subtle). Dramatic dark shadow band on both inner floor-facing edges. Fully opaque stone. Crisp pixel dithering, no anti-aliasing.",
    "style_image": "Assets/Art/Tiles/F1/Tilesets/floor_wall/spritesheet.png",
    "sprite_size": [128, 128],
    "n": 16,
    "style_strength": 0.75,
    "negative_prompt": "bright colors, smooth gradient, Gaussian blur, modern UI, repeating grid, uniform block edges, anti-aliased shapes, cartoon outline, warm brown, transparent interior, symmetrical corner, 90-degree sharp perfect corner"
  }
}
```

### Selection Criteria -- Sprite 3

1. Two distinct stone faces clearly readable under top-down view
2. Interior SW concave corner is the darkest point
3. Both floor-facing edges (bottom, left inner) are jagged
4. Corner mass itself is chunky and irregular, not a perfect right angle
5. Palette within locks, ice-blue hairline present but subtle
6. Silhouette visually matches the horizontal and vertical sprites in stone grain scale

Pick 1 best. Save to: `STAGING/TILESET_OUTPUT/L3_Wall_Corners/wall_corner_NE.png`.

---

## SPRITE 4 -- Wall Corner NW

**Description:** Northwest exterior corner. Mirror-image situation to NE: north face (receding ledge at top) meets west face (front face on left side). Interior concave corner faces southeast. Same painter treatment.

**Canvas:** 128 x 128 px
**Variants needed:** 1

### MCP Call -- Sprite 4

```json
{
  "tool": "create_object",
  "parameters": {
    "description": "Northwest wall corner sprite for a shattered keep dungeon, 128x128 pixels, Hades-style painter pixel art, top-down ~30-35 degree overhead view. Two stone wall faces meet: north face (top edge receding ledge) and west face (left side full front face). Interior concave corner faces southeast (toward room interior) -- deepest shadow pool #1A1C20. ORGANIC IRREGULAR silhouettes on the two floor-facing edges (bottom and right inner edges) -- jagged chipped stone, NO straight lines, NO grid. Painterly large brush strokes. Stone palette #1A1C20 / #2A2D34 / #3A3D48 / #4E5260 / #606575. Subtle ice-blue mineral vein hairline #7BA7BC on west face. Dark shadow bands on inner floor-facing edges. Fully opaque stone interior. Crisp pixel dithering, no anti-aliasing.",
    "style_image": "Assets/Art/Tiles/F1/Tilesets/floor_wall/spritesheet.png",
    "sprite_size": [128, 128],
    "n": 16,
    "style_strength": 0.75,
    "negative_prompt": "bright colors, smooth gradient, Gaussian blur, modern UI, repeating grid, uniform block edges, anti-aliased shapes, cartoon outline, warm brown, transparent interior, symmetrical corner, 90-degree sharp perfect corner"
  }
}
```

### Selection Criteria -- Sprite 4

Same as NE corner but flipped geometry. Pick 1 best where: interior concave corner clearly reads as SE-facing shadow, both inner floor-facing edges jagged, grain scale matches NE corner sprite. Save to: `STAGING/TILESET_OUTPUT/L3_Wall_Corners/wall_corner_NW.png`.

---

## SPRITE 5 -- Wall Corner SE

**Description:** Southeast exterior corner. The south-facing front face (viewer sees this most) meets the east-facing side face. Interior concave corner faces northwest (into room). In the 30-35 deg overhead view, the south face is the widest visible mass.

**Canvas:** 128 x 128 px
**Variants needed:** 1

### MCP Call -- Sprite 5

```json
{
  "tool": "create_object",
  "parameters": {
    "description": "Southeast wall corner sprite for a shattered keep dungeon, 128x128 pixels, Hades-style painter pixel art, top-down ~30-35 degree overhead view. Two stone wall faces: south face (bottom-dominant, widest visible front face toward viewer) and east face (right side front face). Interior concave corner faces northwest (into room) -- deep shadow #1A1C20. ORGANIC IRREGULAR silhouettes on the two room-facing inner edges (top-left area of sprite) -- jagged chipped stone, NO straight lines, NO grid. Painterly large brush strokes on both faces. Stone palette #1A1C20 / #2A2D34 / #3A3D48 / #4E5260 / #606575. Hairline ice-blue mineral vein #7BA7BC across south face (subtle). Strong dark shadow at the inner edges. Fully opaque stone. Crisp pixel dithering, no anti-aliasing.",
    "style_image": "Assets/Art/Tiles/F1/Tilesets/floor_wall/spritesheet.png",
    "sprite_size": [128, 128],
    "n": 16,
    "style_strength": 0.75,
    "negative_prompt": "bright colors, smooth gradient, Gaussian blur, modern UI, repeating grid, uniform block edges, anti-aliased shapes, cartoon outline, warm brown, transparent interior, symmetrical corner, 90-degree sharp perfect corner"
  }
}
```

### Selection Criteria -- Sprite 5

Pick 1 best where south face reads as the dominant painted surface, interior NW corner is the darkest point, and inner room-facing edges are organically jagged. Save to: `STAGING/TILESET_OUTPUT/L3_Wall_Corners/wall_corner_SE.png`.

---

## SPRITE 6 -- Wall Corner SW

**Description:** Southwest exterior corner. South face (dominant front toward viewer) meets west face. Interior concave corner faces northeast (into room). Mirror of SE but stone grain should vary slightly so the two corners do not look identical when placed adjacent.

**Canvas:** 128 x 128 px
**Variants needed:** 1

### MCP Call -- Sprite 6

```json
{
  "tool": "create_object",
  "parameters": {
    "description": "Southwest wall corner sprite for a shattered keep dungeon, 128x128 pixels, Hades-style painter pixel art, top-down ~30-35 degree overhead view. Two stone wall faces: south face (bottom-dominant, wide front face toward viewer) and west face (left side front face). Interior concave corner faces northeast (into room) -- deep shadow pool #1A1C20. ORGANIC IRREGULAR silhouettes on the two room-facing inner edges (top-right area of sprite) -- jagged chipped stone, NO straight lines, NO grid. Painterly large brush strokes, stone grain should visibly differ from SE corner to avoid cloning. Stone palette #1A1C20 / #2A2D34 / #3A3D48 / #4E5260 / #606575. Hairline ice-blue mineral vein #7BA7BC on west face (subtle). Strong dark shadow at inner room-facing edges. Fully opaque stone. Crisp pixel dithering, no anti-aliasing.",
    "style_image": "Assets/Art/Tiles/F1/Tilesets/floor_wall/spritesheet.png",
    "sprite_size": [128, 128],
    "n": 16,
    "style_strength": 0.75,
    "negative_prompt": "bright colors, smooth gradient, Gaussian blur, modern UI, repeating grid, uniform block edges, anti-aliased shapes, cartoon outline, warm brown, transparent interior, symmetrical corner, 90-degree sharp perfect corner, identical grain to SE corner"
  }
}
```

### Selection Criteria -- Sprite 6

Pick 1 best where the stone grain and vein placement visibly differs from the SE corner sprite chosen in Sprite 5 -- they must not look like mirror copies when seen together in a room. Interior NE shadow pool is darkest area. Save to: `STAGING/TILESET_OUTPUT/L3_Wall_Corners/wall_corner_SW.png`.

---

## SPRITE 7 -- Doorway Gap (Passable Opening Transition)

**Description:** A two-tone fade strip placed where a wall opening allows character passage. The top half reads as broken/terminated wall stone, the bottom half fades to floor tone with a thin ambient occlusion shadow line at the transition. This is NOT a door object -- it is a static opening sprite that visually terminates the wall at a passable gap. Variants A and B should have slightly different broken-edge shapes so that left-side and right-side of a doorway can use different variants.

**Canvas:** 128 x 96 px
**Variants needed:** 2

### MCP Call -- Sprite 7 (n=16, pick 2)

```json
{
  "tool": "create_object",
  "parameters": {
    "description": "Doorway gap transition sprite for a shattered keep dungeon wall opening, 128x96 pixels, Hades-style painter pixel art, top-down ~30-35 degree overhead view. Upper half of sprite: broken wall stone terminus -- jagged chipped stone edge, organic irregular silhouette, fully opaque stone mass (#1A1C20 / #2A2D34 / #3A3D48). Stone edge is rough and non-straight, as if the wall was broken open. Lower half of sprite: smooth two-tone transition fade from dark stone shadow (#2A2D34) to dark floor tone (#1A1C20 / #2A2D34 blended), ending in a thin ambient occlusion line at the very bottom. The transition should look like the wall casts a shallow AO shadow onto the floor at the passable gap. No door object, no frame, no arch -- raw broken stone edge only. Subtle ice-blue mineral crack hint #7BA7BC along the stone fracture edge (optional, very subtle). Fully opaque in stone zone, alpha allowed only in the floor-fade zone below the AO line. Crisp pixel dithering, no anti-aliasing, no smooth gradient.",
    "style_image": "Assets/Art/Tiles/F1/Tilesets/floor_wall/spritesheet.png",
    "sprite_size": [128, 96],
    "n": 16,
    "style_strength": 0.7,
    "negative_prompt": "bright colors, smooth gradient, Gaussian blur, door object, arch, frame, modern UI, repeating grid, uniform block edges, anti-aliased shapes, cartoon outline, warm brown palette, straight broken edge, ornamental details, hinges, metal door"
  }
}
```

### Selection Criteria -- Sprite 7

Pick 2 variants where:
1. The broken stone edge is clearly irregular -- no two chips look the same
2. The two-tone floor-fade transition is gradual but clearly reads as AO shadow, not a harsh line
3. The two selected variants have meaningfully different broken-edge silhouettes (so left-side and right-side of a doorway do not look identical)
4. No door object, arch, or decorative frame appears anywhere
5. Stone zone is fully opaque; transition zone is soft

Save to: `STAGING/TILESET_OUTPUT/L3_Wall_Doorway/wall_doorway_A.png` and `wall_doorway_B.png`.

---

## Production Order

Dispatch batches in this sequence. Do NOT generate the next batch until the QC gate for the current batch is PASS.

1. **Sprite 1 -- Horizontal top wall** (most-used type, validates style direction, painterly grain, and palette before committing to remaining types)
2. **Sprite 2 -- Vertical side wall** (after horizontal QC approved; must match grain scale)
3. **Sprites 3-6 -- Four corners** (after horizontal + vertical both approved; corner silhouettes must align with edge silhouette consistency)
4. **Sprite 7 -- Doorway gap** (last; depends on wall stone look being settled; broken edge must read as a natural continuation of the wall style)

---

## Estimated Credits

| Sprite | n | Calls | Estimated Credits |
|---|---|---|---|
| Sprite 1 (horiz wall) | 16 | 1 | ~3 |
| Sprite 2 (vert wall) | 16 | 1 | ~3 |
| Sprite 3 (corner NE) | 16 | 1 | ~3 |
| Sprite 4 (corner NW) | 16 | 1 | ~3 |
| Sprite 5 (corner SE) | 16 | 1 | ~3 |
| Sprite 6 (corner SW) | 16 | 1 | ~3 |
| Sprite 7 (doorway) | 16 | 1 | ~3 |
| **TOTAL** | | **7 calls** | **~21 credits** |

Note: 7 calls x n=16 = 112 generated images total, from which 14 are selected (1-4 per type). Credit estimate is conservative at ~3 per call; actual may vary by PixelLab pricing tier. If orchestrator chooses to split Sprite 1 into 4 separate n=16 calls to get more distinct variants, add ~9 credits (total ~30).

---

## QC Gate Before Each Next Batch

The orchestrator visually compares each produced sprite against `Assets/Art/Tiles/F1/Tilesets/floor_wall/spritesheet.png` and the locked palette before dispatching the next sprite type.

**PASS criteria (all six must be true):**

| Check | Pass Condition |
|---|---|
| Irregular silhouette | No straight or ruler-clean edges on floor-facing sides |
| Palette match | All pixels within the 6 locked hex values; no warm tones |
| No grid | Stone face shows large painterly brushwork, zero regular tile subdivision |
| No blur | All edges are pixel-crisp; no Gaussian or anti-aliased softening |
| Opaque interior | Stone body has zero transparent pixels |
| Dramatic shadow | Dark band (#1A1C20) clearly visible at the floor-facing inner edge |

**FAIL action:** Regenerate the failing sprite type (same prompt, try different `style_strength` 0.65 or 0.80), or adjust description to strengthen the failing check. Do NOT proceed to next sprite type on FAIL.

---

## Output File Map

```
STAGING/TILESET_OUTPUT/
  L3_Wall_Horiz/
    wall_horiz_A.png
    wall_horiz_B.png
    wall_horiz_C.png
    wall_horiz_D.png
  L3_Wall_Vert/
    wall_vert_A.png
    wall_vert_B.png
    wall_vert_C.png
    wall_vert_D.png
  L3_Wall_Corners/
    wall_corner_NE.png
    wall_corner_NW.png
    wall_corner_SE.png
    wall_corner_SW.png
  L3_Wall_Doorway/
    wall_doorway_A.png
    wall_doorway_B.png
```

---

## Notes for Orchestrator

- All sprites use `create_object` (NOT `create_tiles_pro`) because L3 wall overlays are SpriteRenderer perimeter caps, not tileset tiles. They do not need to be seamlessly tileable on all 4 edges.
- `style_strength` 0.7-0.75 is recommended. If style_image is pulling the result too far from the intended irregular organic look, try 0.6. If style is drifting too much from the reference, try 0.8.
- The horizontal wall (Sprite 1) at 256x128 is a non-standard aspect ratio. Confirm PixelLab MCP accepts asymmetric sprite_size before dispatching. If rejected, fall back to two 128x128 half-sections and composite in Aseprite.
- Doorway sprite (Sprite 7) is the only sprite where partial alpha (in the floor-fade zone) is intentional and correct. All other sprites must be fully opaque.
- After QC PASS on all 14 sprites, route to rima-codex for Unity SpriteRenderer placement and L3 sorting layer assignment.
