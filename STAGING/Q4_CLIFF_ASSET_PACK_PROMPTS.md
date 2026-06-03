# Q4 ISO FLOATING-ISLAND CLIFF ASSET PACK -- PRODUCTION PROMPTS v1

**Date:** 2026-06-02 | **Status:** DRAFT v1 -- refine with user before any generation
**Purpose:** Prompt library for the iso floating-island cliff/edge system.
Pipeline = imagegen depth-preview (cheap) -> user picks look -> PixelLab finals.

---

## REFERENCE LOCKS (must be respected by all prompts)

- Floor reference: `Assets/Sprites/Environment/PixelLabFloor451/floor451_0.png`
  (clean 8-color pixel-art dark-slate granite, iso diamond, PPU=64)
- Palette: dark slate #2C2A2A-#4E5260 / iron grey #606878 / cold blue-grey highlights
  accent: cyan seam-energy #00FFCC (sparse) / purple void #1A0033-#2D0052
- Camera: high top-down ARPG ~70-80 degree from horizon (Hades / Children of Morta)
  NOT 45 degree iso math -- sprite is 3/4 styled
- Style: muted desaturated palette, weathered field-worn; dark gritty pixel art,
  matte hand-pixeled clusters, hard pixel edges, no anti-aliasing
- FORBIDDEN in prompts: "dark fantasy" / "3/4 view" / any game name / "80 degree"
  / "extreme top-down bird's eye" / "no eyes" / "eyeless"

---

## SECTION 1 -- IMAGEGEN DEPTH-PREVIEW PROMPTS

Tool: `generate_image` (Imagen / imagegen) -- painterly HD, NOT pixel art.
Purpose: cheap previews to pick the cliff LOOK before spending PixelLab credits.
These are concept references, NOT final assets.
Generate each as a separate call. Pick 1-2 winners for PixelLab.

### VAR-A: Tall Stratified Rock Wall (preferred starting point)

```
Isometric floating rock island edge, thick granite cliff face showing horizontal rock strata and stress fractures, viewed from a steep high angle ARPG camera (~75 degrees overhead). The top lip is a sharp flat floor edge with a slight overhang shadow beneath it. Rock face descends 4-5 visible strata bands, each band slightly darker and more cracked than the one above. The bottommost edge dissolves into a dark purple void with a faint cyan energy seam running along the topmost crack under the floor. Palette: dark slate, iron grey, cold blue-grey highlights on the top lip, deep purple void beneath. Style: painterly pixel-art concept art, muted desaturated colors, weathered stone, no vegetation. On-brand for a gritty supernatural dungeon. Transparent-friendly composition, no background fill, cliff face centered.
```

### VAR-B: Stepped Receding Shelves

```
Isometric floating dungeon island edge, cliff rendered as 3 receding stepped stone shelves descending into purple void. Each shelf is a narrow horizontal ledge of dark slate, viewed at steep high ARPG angle (~75 degrees overhead). Top step sits directly under the granite floor edge with a hard shadow. Middle step is set back and darker. Bottom step is barely visible, fading to void purple. Thin cyan glowing hairline crack runs along the first step break (under the floor). Cold blue-grey light rakes across the top face of each step from the upper-left. Palette: charcoal slate, iron grey, #00FFCC cyan accent (sparing), void purple. Painterly pixel-art concept, muted, weathered, supernatural. No background fill, no text.
```

### VAR-C: Jagged Broken-Keep Masonry

```
Isometric floating island cliff edge built from irregular broken stone blocks, like the underside of a ruined castle keep. Viewed from steep high angle (~75 degrees overhead). Blocks are roughly hewn dark slate, with broken corners exposing lighter grey interiors. Chunks of masonry jut out at different depths creating strong shadow pockets. A faint cyan energy vein runs horizontally through the rock 1/4 from the top, as if the keep was shattered by magical force. Bottom of the cliff breaks into raw void purple darkness with no smooth edge. Top lip aligns flush with the floor. Style: muted desaturated painterly pixel concept, weathered field-worn masonry, gritty supernatural. No background, no text.
```

### VAR-D: Organic Cracked Monolith Slabs (vertical emphasis)

```
Isometric floating rock island, cliff edge shown as 2-3 tall vertical monolith slab panels of dark grey granite, each panel slightly offset in depth, creating a paneled depth illusion. Steep ARPG high-angle view (~75 degrees). Panels face outward and slightly downward. Surface shows horizontal stress cracks and vertical hairline fissures. Cold light hits the topmost edge; shadow deepens toward the void below. One thin cyan glowing crack bisects the tallest panel near its top. Palette: dark slate, iron grey, charcoal, #00FFCC cyan (one crack only), deep purple void at base. Painterly pixel-art concept, muted desaturated, no smooth gradients. No background fill, no text, no labels.
```

---

## SECTION 2 -- PIXELLAB FINAL PROMPTS

### 2a. create_tiles_pro -- Long Continuous Cliff-Face Strip

**When to use:** Generate a tileable horizontal cliff-face strip that lines the
entire island edge. Use style_images to match floor451_0.png's palette exactly.

**Tool:** `mcp__pixellab__create_tiles_pro`
**Params:**
```
description: (see prompt below)
tile_type: "isometric"
tile_size: 64
tile_view: "low top-down"
tile_depth_ratio: 0.85
outline_mode: "segmentation"
style_images: [{"base64": "<floor451_0.png encoded>", "width": 64, "height": 80}]
style_options: {"color_palette": true, "outline": true, "detail": true, "shading": true}
```

**Prompt text (description field):**
```
1). cliff face straight edge segment -- dark slate granite rock face, thick vertical drop
below the iso floor lip, 3 visible horizontal strata bands with hairline cracks, cool
blue-grey highlight on the top overhang edge, rock darkens toward bottom, faint cyan
energy seam in the uppermost crack, bottom edge dissolves into purple void, muted
desaturated palette, weathered field-worn stone, hard pixel edges, no anti-aliasing,
transparent background 2). cliff face straight edge segment variant -- same strata but
center crack is wider and has a small stone chip broken off, slightly darker mid-band,
cyan seam is more pronounced 3). cliff inner corner segment -- concave 90-degree turn
of the same stratified cliff face, depth reads correctly from ARPG angle, matching
palette 4). cliff outer corner cap -- convex 90-degree corner of the cliff edge,
slightly more rock exposed, 4 strata bands visible, matching palette and strata style
```

**Depth description notes (in the prompt):**
- "thick vertical drop below the iso floor lip" -- signals non-flat, tall face
- "3 visible horizontal strata bands" -- forces layered depth, not a flat strip
- "cool blue-grey highlight on top overhang / darkens toward bottom" -- vertical
  light gradient that reads as depth from above
- "faint cyan energy seam in the uppermost crack" -- RIMA palette anchor
- "dissolves into purple void" -- connects to the void environment, avoids
  hard bottom edge that would look like a flat cutout

**Estimated cost:** 20-25 gen (style_images pushes to upper band)
**Output transparency:** YES (segmentation mode produces clean alpha)
**Iso perspective:** tile_type=isometric + tile_view=low top-down + tile_depth_ratio=0.85
**Uncertainty flag:** tile_depth_ratio=0.85 is the maximum practical value for visible
face height. If the face still looks too thin, try tile_view="side" (depth_ratio ~0.5
is overridden by the explicit param). TEST with one tile before full batch.

---

### 2b. create_map_object -- Discrete Modular Cliff Pieces

**When to use:** Individual placeable pieces (straight segment, inner/outer corner caps)
as transparent map objects. Use background_image with floor451_0.png to auto-match style.

**Tool:** `mcp__pixellab__create_map_object`

**Params for all pieces:**
```
background_image: {"type": "path", "path": "Assets/Sprites/Environment/PixelLabFloor451/floor451_0.png"}
inpainting: {"type": "rectangle", "fraction": 0.7}
detail: "high detail"
shading: "detailed shading"
outline: "selective outline"
view: "high top-down"
```

**Piece A -- Straight edge segment (primary)**
```
description: "Isometric floating island cliff edge, straight segment, dark slate
granite vertical rock face with 3 horizontal strata bands, slight overhang shadow
under the floor lip, cool blue-grey highlight on top edge, rock darkens to purple
void at bottom, one thin cyan energy seam in the top crack, weathered field-worn
stone, muted desaturated pixel art, transparent background"
width: 192
height: 192
```

**Piece B -- Outer corner cap (convex)**
```
description: "Isometric floating island cliff outer corner, convex 90-degree turn,
dark slate granite with 4 horizontal strata bands wrapping the corner, overhang
shadow under the floor lip on both faces, top edge highlight, faint cyan seam crack,
void purple at base, muted desaturated pixel art, weathered stone, transparent background"
width: 192
height: 192
```

**Piece C -- Inner corner cap (concave)**
```
description: "Isometric floating island cliff inner corner, concave 90-degree recess,
dark slate granite strata bands continuing into the corner pocket, deep shadow in the
corner crease, top floor-lip edge visible on both sides, faint cyan seam, void at base,
muted desaturated pixel art, weathered stone, transparent background"
width: 192
height: 192
```

**Piece D -- Cyan seam accent (optional overlay)**
```
description: "Thin horizontal glowing cyan energy seam, pixel art, #00FFCC color,
slight bloom softness, 1-2 pixels thick, runs the full width of the canvas, slight
variation in brightness along its length, transparent background, no stone, no border"
width: 192
height: 32
```

**Estimated cost per piece:** ~1 gen (basic mode without background_image) or
~1-2 gen (with background_image inpainting -- schema says ~1 gen equivalent)
**Output transparency:** YES (map_object always transparent bg)
**Iso perspective control:** view="high top-down" is the closest param available.
NOTE: create_map_object does NOT have tile_type or tile_depth_ratio params -- it relies
entirely on the prompt text for perspective. For iso cliff face, the description must
explicitly state "isometric viewing angle, rock face visible below the floor" or the
model may generate a flat top-down silhouette. Use VAR-A imagegen result as
background_image instead of floor451 for better style lock if floor451 gives too-flat results.

---

## SECTION 3 -- RECOMMENDED PACK STRUCTURE

### Geometry reference
- Island diamond edge at PPU=64: ~870-1180px wide (depends on room size, typically
  16-20 tiles across = 1024-1280px diagonal edge)
- Cliff face height target: 192-256px tall (3-4 tile heights) -- DO NOT go below 128px
  or depth detail disappears entirely (the y0.55 squash problem documented in context)

### Piece inventory (minimum viable pack)

| Piece | Count | Size (px) | Notes |
|---|---|---|---|
| Straight edge segment | 2-3 variants | 192x192 | tile horizontally along edge |
| Outer corner cap | 1 | 192x192 | convex turn, 2 diagonal edges meet |
| Inner corner cap | 1 | 192x192 | concave recess, room nook |
| Cyan seam accent strip | 1 | 192x32 | optional overlay on top crack |

**Total: 5-6 pieces.** Enough to cover any island shape by repeating straight segments.

### Tiling strategy
- Straight segments tile at 192px steps (= 3 tiles at PPU64).
  For a 1024px diagonal edge: ~5-6 segments + 2 corner caps.
- Segments should be seamless on left/right edges (strata bands must
  align horizontally). Request "seamless left-right edges" in the prompt.
- Corner caps are non-tiling, placed once per corner.

### Why 192x192 and not a single 1024px strip
- PixelLab create_map_object caps at 400x400 basic, 192x192 inpaint.
- create_tiles_pro at tile_size=64 can output a wide strip but the
  numbered-tile approach gives 4 tiles per call max at this size (budget).
- 192px segments are easier to QC and re-gen individually.
- Single-strip alternative: create_image_pro (512x512 max, 688x384 landscape)
  could produce one long face -- good for a hero detail reference but not a
  tileable pack. Use it for the FIRST showcase render if desired.

---

## SECTION 4 -- PIXELLAB TOOL / PARAM AUDIT

### create_map_object
- Style/init reference image: YES (background_image param, path or base64)
- Credit cost: ~1 gen (basic), auto-detected from background for inpaint mode
- Output transparency: YES always
- Iso perspective/size control: WEAK -- only view enum (low/high top-down / side),
  no tile_type or depth ratio. Prompt text must compensate.
- Canvas max: 400x400 basic / 192x192 inpaint
- Auto-deletes after 8 hours -- must save immediately after job completes

### create_tiles_pro
- Style/init reference image: YES (style_images JSON array, base64+dimensions)
- Credit cost: 20-25 gen (style ref adds ~5 gen)
- Output transparency: YES (segmentation mode especially clean)
- Iso perspective control: STRONG -- tile_type=isometric + tile_view + tile_depth_ratio
  (0.0-1.0) + tile_view_angle (0-90 degrees continuous)
- tile_size max: 128px
- UNCERTAINTY: tile_depth_ratio at 0.85 is near max -- may produce exaggerated depth.
  No direct schema documentation on what 0.85 looks like vs 0.5. Recommend testing
  0.5, 0.7, 0.85 on a single tile before batch.
- Wang16 guarantee: NO (use create_topdown_tileset for Wang; cliff is NOT Wang terrain)

### create_isometric_tile
- Style/init reference image: NO (no reference param in schema)
- Credit cost: included / cheap
- Output transparency: implied (iso tile)
- Iso perspective control: tile_shape (thin/thick/block), size (16-64px max)
- LIMITATION: size max 64px -- too small for a detailed cliff face at PPU64.
  A 64px iso tile at PPU64 is exactly 1 tile wide. For a cliff face needing
  192px height this tool is unsuitable. Use create_tiles_pro instead.
- Best use in this pack: NOT recommended for cliff face. Could work for a tiny
  "void edge shimmer" accent tile (size=32, thin/block).

### create_image_pro (Web/API, not MCP)
- Style/init reference image: YES (up to 4 ref + 1 style image)
- Credit cost: 20 gen
- Output transparency: YES
- Canvas max: 512x512 / 688x384 landscape
- Best use in this pack: a single HERO cliff-face render at 688x384 (landscape
  orientation = perfect for a wide cliff strip reference). Feed floor451_0.png
  as style image + the winning imagegen VAR as init reference.
  This is the best single-image quality option but NOT tileable.
- IMPORTANT: This is a Web tool -- MCP schema not loaded. Confirm availability
  before calling. Cannot be invoked from MCP directly.

### Flags / Uncertainties
1. create_map_object view="high top-down" does NOT equal ~75 degree ARPG iso.
   The enum has only 3 steps. For a cliff face the "side" view might actually
   produce better results (rock face is a vertical surface). RECOMMEND: test
   one piece with view="side" vs view="high top-down" on the same prompt.
2. create_tiles_pro style_images takes width+height from the reference -- if
   floor451_0.png is a non-standard size (e.g. 64x80 iso diamond), the output
   tile dimensions will match that, which is correct. Verify floor451 actual
   pixel dimensions before calling.
3. The 8-hour auto-delete on create_map_object means all pieces must be
   downloaded immediately after the poll confirms completion.

---

## SECTION 5 -- PIPELINE SEQUENCE REVIEW

Proposed pipeline:
```
imagegen preview (VAR A/B/C/D) -> user picks look
  -> PixelLab create_tiles_pro (style_images=floor451) for tileable straight segments
  -> PixelLab create_map_object (background_image=floor451) for corner caps
  -> optional create_image_pro hero render at 688x384
  -> QC + seam alignment check in Aseprite
  -> Unity import as Sprite (PPU=64, pivot=bottom-center)
```

**Assessment: CONFIRMED with two caveats.**

CAVEAT 1 -- Init reference vs style reference distinction:
The pipeline says "PixelLab create_image_pro with chosen preview + floor451 as
style-ref." In PixelLab these are two different inputs:
- style-ref = locks PALETTE + outline style (floor451 is correct here)
- init/reference image = locks COMPOSITION + shapes (winning imagegen VAR goes here)
Both should be passed simultaneously when possible. For create_tiles_pro,
style_images handles the palette lock; the composition shaping comes entirely from
the description prompt (no separate init-image param in MCP schema). For
create_image_pro (Web), pass floor451 as style_image and winning imagegen VAR
as one of the 4 reference slots with init_image_strength ~400-600 for
"loose composition guidance."

CAVEAT 2 -- Straight-segment seam alignment:
PixelLab does not guarantee seamless tiling between separate map_object calls.
The strata bands in Piece A call 1 may not align with Piece A call 2.
Mitigation: use create_tiles_pro for the repeating straight segment (its
numbered-tile batch generates all variants in one pass under the same seed,
maximizing internal consistency). Reserve create_map_object for corner caps
only (those are non-tiling, placed once each).

**Recommended sequence adjustment:**
```
1. imagegen VAR-A through VAR-D -> user picks 1 winner
2. create_tiles_pro (4-tile call): straight-edge + wide-crack-variant +
   inner-corner + outer-corner, all in one batch with style_images=floor451
   -- maximizes palette consistency across the pack
3. create_map_object: cyan seam accent strip only (32px tall, simple)
4. Optional: create_image_pro (Web) hero showcase at 688x384
5. Aseprite: seam-check straight segment left/right edges, adjust 1-2 pixels
   if strata bands do not align
6. Unity: import all pieces, PPU=64, pivot bottom-center, set to Entities sorting layer
```

---

## QUICK REFERENCE -- COPY-PASTE PROMPTS

### imagegen (pick one to start):
**VAR-A (tall strata -- recommended first):**
Isometric floating rock island edge, thick granite cliff face showing horizontal rock strata and stress fractures, viewed from a steep high angle ARPG camera (~75 degrees overhead). The top lip is a sharp flat floor edge with a slight overhang shadow beneath it. Rock face descends 4-5 visible strata bands, each band slightly darker and more cracked than the one above. The bottommost edge dissolves into a dark purple void with a faint cyan energy seam running along the topmost crack under the floor. Palette: dark slate, iron grey, cold blue-grey highlights on the top lip, deep purple void beneath. Style: painterly pixel-art concept art, muted desaturated colors, weathered stone, no vegetation. On-brand for a gritty supernatural dungeon. Transparent-friendly composition, no background fill, cliff face centered.

### PixelLab create_tiles_pro (4-piece batch):
description: 1). cliff face straight edge segment -- dark slate granite vertical rock face with 3 horizontal strata bands with hairline cracks, cool blue-grey highlight on the top overhang edge, rock darkens toward bottom, faint cyan energy seam in the uppermost crack, bottom edge dissolves into purple void, muted desaturated palette, weathered field-worn stone, hard pixel edges, transparent background, seamless left-right edges 2). cliff face straight edge variant -- same strata, center crack wider with small chip, cyan seam more pronounced, seamless left-right edges 3). cliff inner corner segment -- concave 90-degree turn of stratified cliff face, depth reads correctly from ARPG angle, matching palette 4). cliff outer corner cap -- convex 90-degree corner, 4 strata bands visible, matching palette and strata style

### PixelLab create_map_object (straight piece -- standalone test):
description: Isometric floating island cliff edge, straight segment, dark slate granite vertical rock face with 3 horizontal strata bands, slight overhang shadow under the floor lip, cool blue-grey highlight on top edge, rock darkens to purple void at bottom, one thin cyan energy seam in the top crack, weathered field-worn stone, muted desaturated pixel art, transparent background, seamless left and right edges, high top-down ARPG viewing angle, rock face visible below the floor
width: 192, height: 192, view: "high top-down", shading: "detailed shading", detail: "high detail"
```
