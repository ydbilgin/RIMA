# RIMA Undercroft Tileset -- Connected Wall Set
# Generated: 2026-05-07
# Extends: PIXELLAB_TILESET_UNDERCROFT_2026-05-06.md
# Scope: Full isometric connected wall system + object overlays
# Spec: S43 -- 64px tile, Unity URP, RuleTile-ready

---

## INHERITED EDGE CONTRACT (DO NOT CHANGE)

All tiles inherit from the 2026-05-06 base set. Edges must match exactly.

Floor diamond edges (all 4): #161620 (mortar dark)
Wall left/right profile edges: #1a1c28 (shadow side)
Wall top edge: #3a3c50 (cut top surface)
Wall base edge: #12141a (deep shadow)

Isometric wall face colors:
  Front face (south-facing): stone #2e3040 with per-block variation #2a2c3c / #323446
  Top surface (cut edge): #3a3c50
  Shadow side face: vertical gradient #1a1c28 (top) to #12141a (base)
  Mortar joints: #12141e, 1-2px wide

These values are locked. No prompt may deviate from them on edge pixels.

---

## ISOMETRIC WALL ANATOMY NOTE (READ BEFORE GENERATING)

In this isometric projection (steep overhead, approx 60-65 degree), each wall tile
shows two faces simultaneously:
  - SOUTH face: wide front face, faces bottom-right toward the camera
  - WEST face (shadow): narrow left face, faces bottom-left, always in shadow

A "left wall" (west wall) and "right wall" (south wall) are distinct tiles because
the camera angle means each orientation shows a different combination of faces.

West wall (runs left-right in world space): south face is narrow, west face is wide
South wall (runs top-bottom in world space): south face is wide, west face is narrow

For RuleTile purposes Unity sees these as the same "wall" type but different rotation
variants -- specify neighbor rules accordingly in the tile notes.

---

## TILE GROUP A: FLOOR (INHERITED FROM 2026-05-06)

Tiles A1-A4 (Floor Base, Cracked, Mossy, Wet Stone) are defined in the base file.
Do NOT regenerate unless QC fails. Use base file prompts exactly.
RuleTile: Floor Rule Tile asset, weights 40/30/20/10.

---

## TILE GROUP B: DIRECTIONAL WALL TILES

### TILE B1: West Wall -- Straight Run

Canvas: 64x96px isometric wall.
Tags: wall, west-face, straight, seamless-run
RuleTile neighbor rule: HAS wall-above AND HAS wall-below on west axis. No corner neighbor.
Weight: 85% when neighbor conditions met.

PROMPT:
isometric pixel art wall tile 64x96px, west wall straight run segment. The wall runs horizontally in world space; camera sees the south face as a narrow vertical strip on the right side of the tile and the wide west face occupies the left two-thirds of the tile. West face: 3 rows of cut stone blocks, block face color #2e3040 with per-block variation #2a2c3c and #323446, horizontal mortar joints #12141e 1px wide. Block height approximately 20px each row. Top surface strip 16px: color #3a3c50, cut flat, 1px highlight at very top edge #464858. Shadow south face (right strip, 16px wide): vertical gradient #1a1c28 at top to #12141a at base. Block corners show 2-3px chip marks #12141e. Left edge of tile: #1a1c28 seamless (connects to next west wall segment). Right edge of tile: #1a1c28 seamless (connects to next west wall segment). Top edge: #3a3c50. Base edge: #12141a. Hard pixel edges, no anti-aliasing, no blur. Muted desaturated palette, weathered field-worn. Constructed masonry, natural mortar joints, worn edges.

Unity RuleTile note: Tile outputs when neighbor[Left]=Wall AND neighbor[Right]=Wall, neighbor[Up]=Empty (open above) OR neighbor[Up]=Wall.

---

### TILE B2: South Wall -- Straight Run

Canvas: 64x96px isometric wall.
Tags: wall, south-face, straight, seamless-run
RuleTile neighbor rule: HAS wall-above AND HAS wall-below on south axis.
Weight: 85% when neighbor conditions met.

PROMPT:
isometric pixel art wall tile 64x96px, south wall straight run segment. The wall runs vertically in world space; camera sees the wide south face occupying the right two-thirds of the tile and the narrow west face shadow strip on the left. South face: 3 rows of cut stone blocks running across the full width of the tile, block face color #2e3040 with per-block variation #2a2c3c and #323446, mortar joints #12141e 1-2px horizontal and vertical. Block width approximately 20px each, 3 blocks per row. Top surface strip 16px: color #3a3c50 cut flat. Left shadow face (8px wide strip): vertical gradient #1a1c28 top to #12141a base. Block chip marks 2-3px at corners #12141e. Top edge: #3a3c50 seamless (connects to wall above). Base edge: #12141a seamless (connects to wall below). Left edge: #1a1c28. Right edge: #1a1c28. Hard pixel edges, no anti-aliasing. Muted desaturated palette, weathered field-worn. Constructed masonry, aged stone, natural mortar joints.

Unity RuleTile note: Tile outputs when neighbor[Up]=Wall AND neighbor[Down]=Wall, neighbor[Left]=Empty OR neighbor[Left]=Wall.

---

### TILE B3: Outer Corner

Canvas: 64x96px isometric wall.
Tags: wall, corner, outer, convex
RuleTile neighbor rule: West wall segment meets South wall segment, wall bends outward (convex toward camera). Placed where west-run and south-run meet at outside corner.

PROMPT:
isometric pixel art wall tile 64x96px, outer corner where west wall and south wall meet. The corner protrudes toward the camera (convex, not recessed). At the center-top of the tile the two wall faces meet at a vertical stone corner pilaster -- a 4px wide vertical ridge of darker stone #1e2030 with 1px highlight #3a3c50 on the exact corner edge. West face fills left portion of tile: cut stone blocks #2e3040 variation #2a2c3c #323446, mortar joints #12141e. South face fills right portion of tile: same block colors and mortar. Top surface: L-shaped #3a3c50 strip spanning both faces meeting at corner. Shadow gradient on west face: #1a1c28 to #12141a. The corner ridge itself catches slight highlight: top 2px of ridge #464858. Left edge: #1a1c28. Right edge: #1a1c28. Top edge: #3a3c50 across full width. Base edge: #12141a. Block chip marks near corner #12141e. Hard pixels, no anti-aliasing. Muted desaturated palette, weathered field-worn.

Unity RuleTile note: Tile outputs when neighbor[Left]=Wall AND neighbor[Down]=Wall AND neighbor[Left-Down]=Empty (the outside of the corner is open floor).

---

### TILE B4: Inner Corner

Canvas: 64x96px isometric wall.
Tags: wall, corner, inner, concave
RuleTile neighbor rule: Wall bends inward (concave, corner faces away from camera). Placed where two wall runs meet at inside corner of a room.

PROMPT:
isometric pixel art wall tile 64x96px, inner corner where two wall runs meet, concave corner receding away from camera. The corner is at the back of the tile -- walls meet in shadow. West face portion on left: cut stone blocks #2e3040 #2a2c3c #323446 running toward the corner. South face portion on right: same blocks running toward the corner from the other side. At the inner corner junction (center-back): a vertical crevice 2px wide, color #0e0e18, deepest shadow point. The stone blocks nearest the crevice are slightly darker #1e2030 indicating shadow depth. Top surface: L-shaped #3a3c50 strip, inner corner of L also #0e0e18 at the very back point. Left edge: #1a1c28. Right edge: #1a1c28. Top edge: #3a3c50. Base edge: #12141a. Hard pixels no anti-aliasing. Muted desaturated palette, weathered field-worn. The concave corner creates a deep shadow pocket at its back.

Unity RuleTile note: Tile outputs when neighbor[Left]=Wall AND neighbor[Down]=Wall AND neighbor[Left-Down]=Wall (all three surrounding cells are wall -- the corner is enclosed).

---

### TILE B5: West Wall End Cap

Canvas: 64x96px isometric wall.
Tags: wall, west-face, end-cap, terminus
RuleTile neighbor rule: West wall run ends, no wall continues beyond this point in the run direction.

PROMPT:
isometric pixel art wall tile 64x96px, west wall end cap -- the wall terminates here, no continuation beyond. The west face occupies the left portion of the tile with full stone block rows. At the right side of the tile where the wall ends: a finished stone end face, 8px wide, showing the cut cross-section of the wall thickness. End face color #262838 slightly different from front face to show it is a perpendicular surface. Top edge of end face: 2px strip #3a3c50 showing wall thickness. The end face has 3 horizontal mortar-line marks #12141e matching the block row heights to indicate the wall is made of stacked courses. No continuation of blocks beyond the end face -- right side of tile is empty (transparent or open). Left edge: #1a1c28 seamless (connects to wall behind). Top edge: #3a3c50. Base edge: #12141a. Right edge: open / transparent beyond end face. Hard pixels, no anti-aliasing. Muted desaturated palette, weathered field-worn. Natural stone masonry terminus, slightly worn corner edge.

Unity RuleTile note: Tile outputs when neighbor[Left]=Wall AND neighbor[Right]=Empty (end of west run, right side is open space).

---

### TILE B6: South Wall End Cap

Canvas: 64x96px isometric wall.
Tags: wall, south-face, end-cap, terminus
RuleTile neighbor rule: South wall run ends, no wall continues above this point.

PROMPT:
isometric pixel art wall tile 64x96px, south wall end cap -- the wall terminates here at its top end. The south face occupies the right portion of the tile with full stone block columns. At the top of the tile where the wall ends: the uppermost block row has a full finished top surface cap, #3a3c50 slightly wider than the standard top strip (24px instead of 16px) to emphasize the cap. A 2px dark rim #1a1c28 outlines the top perimeter of the cap, showing it is a finished coping stone. The coping stone has a very slight bevel: top face #3a3c50, front bevel edge #464858 1px, bevel base back to stone face #2e3040. Top of tile: #3a3c50 dominant. Left edge: #1a1c28. Right edge: #1a1c28. Base edge: #12141a seamless (connects to wall below). Hard pixels, no anti-aliasing. Muted desaturated palette, weathered field-worn. The capping stone reads as deliberate architectural termination.

Unity RuleTile note: Tile outputs when neighbor[Up]=Empty AND neighbor[Down]=Wall (top of a south run, nothing above).

---

### TILE B7: Pillar (Freestanding Column)

Canvas: 64x96px isometric wall.
Tags: wall, pillar, column, freestanding
RuleTile neighbor rule: No wall neighbors required -- pillar stands alone or at wall junctions as a decorative structural element.

PROMPT:
isometric pixel art wall tile 64x96px, freestanding square pillar / column. Both the south face and west face of the pillar are visible simultaneously as the column stands alone. Pillar body: a square column approximately 24px wide. South face of pillar: 3 stacked block sections #2e3040 with mortar joints #12141e and per-block variation. West face of pillar: same block stack but in shadow #1a1c28 gradient to #12141a, slightly narrower face (12px) due to viewing angle. Top cap of pillar: a square capital stone, flat top surface #3a3c50 with a 2px chamfered edge #464858 around its perimeter. Pillar base: a wider plinth 4px on each side, same stone color with slightly darker base #1e2030. Block chip marks at corners and edges #12141e. Left side of tile beyond pillar: transparent / open. Right side: transparent / open. Top edge: #3a3c50 (pillar cap only). Base edge: #12141a (plinth base). Hard pixels, no anti-aliasing. Muted desaturated palette, weathered field-worn. This is a load-bearing architectural column, not decorative.

Unity RuleTile note: Tile is placed as standalone; no neighbor matching required. Place manually or as a rare random variant (5%) at wall junctions.

---

### TILE B8: Doorway Opening

Canvas: 64x96px isometric wall.
Tags: wall, doorway, opening, arch
RuleTile neighbor rule: South wall run contains a doorway gap -- manually placed, not auto-tiled.

PROMPT:
isometric pixel art wall tile 64x96px, doorway opening in a south-facing wall with stone arch. The wall continues on both left and right sides of the opening. Center 24px of tile: open archway void, dark interior visible #080810 deep shadow. Arch structure: rough-cut stone voussoir blocks forming a semicircular arch over the opening. Arch stones color #262838 with individual voussoir outlines in mortar #12141e. Keystone at arch apex slightly lighter #323446 and 2px taller than flanking stones to emphasize it. Left wall pier: standard south-face masonry blocks #2e3040 variation, mortar joints. Right wall pier: same. Top surface across both piers: #3a3c50, interrupted by the arch opening in center. Arch soffit (inner arch face visible from below): deep shadow #0e0e18 with 1px edge highlight where arch meets wall face #1a1c28. Left edge: #1a1c28. Right edge: #1a1c28. Top edge: #3a3c50 (pier tops only, open in center). Base edge: #12141a. Hard pixels no anti-aliasing. Muted desaturated palette, weathered field-worn. Constructed masonry arch, aged stone, natural mortar joints.

Unity RuleTile note: Manually placed as plain Tile asset (not part of Rule Tile group). Replaces a south wall segment. Ensure left and right neighboring wall tiles are south wall straight runs for seamless connection.

---

## TILE GROUP C: OBJECT OVERLAY TILES

Object tiles are placed on a separate Unity Tilemap layer above the floor/wall layer.
They do not participate in Rule Tile auto-tiling. All placed manually.
Canvas: 64x96px (wall-mounted objects) or 64x32px (floor objects).

---

### TILE C1: Wall Torch Sconce (Extended -- West Wall Mount)

Canvas: 64x96px.
Tags: object, overlay, torch, west-wall-mount, animated-placeholder
Note: This is the base static frame. For animated flame see Animation note below.

PROMPT:
isometric pixel art overlay tile 64x96px, wall torch sconce mounted on west-facing stone wall. Background: transparent (alpha) except for the sconce object itself -- this overlays on top of a west wall tile. Sconce bracket: wrought iron L-bracket, 5px wide, color #1c1c24, 1px highlight edge #28282e on top surface, attached to wall 32px from left edge and 40px from top. Bracket arm extends 6px outward from wall face. Torch cup at bracket end: 4px wide circular cup #1c1c24 holding torch stick. Torch stick: 3px wide, 8px tall, color #2a1810 (charred wood). Flame above torch tip: 5px wide, 7px tall, warm amber flame, base color #b04010 becoming #d06020 at midpoint and #e08030 at tip, leftmost flame pixel slightly cooler #c05020. Warm light spill: 3px radius around flame base on transparent background uses additive overlay suggestion -- 4 pixels at flame base color #3a2820 at 50% opacity hint. Sconce shadow on wall: 2px dark ellipse below bracket #0e0e18. Entire background outside sconce object: fully transparent. Hard pixels, no anti-aliasing.

Animation note: To animate flame, create 4 frames (4x this tile). Each frame: shift flame tip 1px left/right alternating, change tip pixel between #e08030 and #d06020, keep base stable. Frame timing 150ms each (approx 6fps flame flicker).

Unity RuleTile note: Place on Object layer above a B1 (West Wall) tile. Layer order: Floor (0), Wall (1), Object (2).

---

### TILE C2: Rubble Pile (Floor Object)

Canvas: 64x32px isometric diamond footprint.
Tags: object, overlay, floor, rubble, debris
Note: Overlays on floor tiles. Blocks movement (set collider in Unity).

PROMPT:
isometric pixel art overlay tile 64x32px, rubble pile of broken stone fragments on dungeon floor. Background transparent outside the rubble shape. Rubble pile: irregular mound of broken stone chunks, 40px wide at base, 18px tall at highest point (isometric volume). Stone chunk colors: #262838 large chunks, #1e2030 medium, #12141a shadow sides. Individual chunks: 4-8px irregular polygonal shapes with 1px dark outline #0e0e18. Mortar dust between chunks: scattered 1-2px pixels #2a2830 suggesting pulverized mortar. Chunk arrangement: largest chunk at center-top, smaller chunks radiating outward and downward, smallest debris pixels at perimeter. Shadow of pile cast on floor (south side of pile): 6px semi-transparent dark area #12141a at 60% -- rendered as solid dark pixels bordering the pile base. One chunk: slightly lighter #3a3c50 on its top face showing it caught a sliver of ambient light. Tile edges: transparent. Hard pixels no anti-aliasing. Muted desaturated palette, weathered field-worn.

Unity RuleTile note: Place on Object layer. Add BoxCollider2D or TilemapCollider for blocking. Randomize rotation at 0/90/180/270 degrees in Unity for variety. 2-3 sprite variants recommended.

---

### TILE C3: Fallen Stone Block

Canvas: 64x32px isometric diamond footprint plus 32px vertical extent (64x64px total canvas, top 32px contain the block height).
Tags: object, overlay, floor, large-debris, structural
Note: A large architectural block that has fallen and half-embedded in floor. Larger visual presence than rubble.

PROMPT:
isometric pixel art overlay tile 64x64px, large fallen cut stone block half-sunken into dungeon floor. The block is a substantial architectural element (lintel or wall block) that has collapsed. Block dimensions visible: 32px wide, 22px tall in isometric view. Block face (south side): color #2a2c3c, slightly darker than wall face, showing the block is worn and dirty. Block top face: #323446 catching more light than south face. Block left shadow face: #1a1c28. A diagonal crack runs across the block face from upper-right to lower-left, crack color #0e0e14 2px wide with 1px lighter edge #1e2030 on one side suggesting the crack has depth. Where block meets floor: 4px of crushed debris and mortar dust #1e1c28 settling around the base. Block corners: heavily chipped, 3-5px irregular notches #12141e. Beneath block: deep shadow #080810. Background outside block: transparent. Top canvas half (32px): block body. Bottom canvas half (32px): floor-level debris and shadow. Hard pixels no anti-aliasing. Muted desaturated palette, weathered field-worn.

Unity RuleTile note: Place on Object layer, pivot at bottom-center. Occupies 1 tile cell visually but sprite extends upward. Ensure sorting order places this above floor tile sprite.

---

### TILE C4: Wall Chain Ring (Wall-Mounted Atmosphere)

Canvas: 64x96px.
Tags: object, overlay, atmosphere, chain, west-wall-mount OR south-wall-mount
Note: Two variants needed -- one for west wall mount, one for south wall mount.

PROMPT (West Wall variant):
isometric pixel art overlay tile 64x96px, iron chain ring set into west-facing dungeon wall. Background fully transparent. A large iron ring bolt: ring outer diameter 10px, ring inner diameter 7px, ring color #1c1c24 with 1px highlight #282830 on upper arc and 1px deep shadow #0e0e0e on lower arc. The ring hangs from an iron eyebolt driven into the wall. Eyebolt: 4px wide, 6px long, same iron color #1c1c24, embedded into wall at ring 12 o'clock. From the ring hang 3 chain links: each link 5px tall, 4px wide, oval, alternating orientation (flat / upright) as is correct for chain perspective. Chain link color #1c1c24 with 1px highlight on upper edge #282830. Chain hangs vertically downward from ring, total length 18px. Chain end: a broken link, jagged 2px break, suggesting chain was once longer. Wall mount position: centered horizontally at 32px, eyebolt at 48px from top. Shadow of ring and chain on wall: 2px dark ellipse/line offset 1px right and 1px down from objects #0e0e14. Background: fully transparent. Hard pixels no anti-aliasing.

PROMPT (South Wall variant):
isometric pixel art overlay tile 64x96px, iron chain ring set into south-facing dungeon wall. Background fully transparent. Same ring, eyebolt and chain as west wall variant but adjusted for south-face perspective: ring appears more circular (less elliptical) because south face is more front-on to camera. Ring outer diameter 10px, inner 7px, iron #1c1c24, highlight #282830 upper arc. Eyebolt embedded at ring top. Chain hangs below ring, 3 links, 18px total, broken end. Wall mount: centered at 32px horizontal, 48px from top. Shadow offset 2px. Background fully transparent. Hard pixels no anti-aliasing.

Unity RuleTile note: Place on Object layer. West variant: overlay on B1 (West Wall). South variant: overlay on B2 (South Wall). No collision needed -- atmosphere only. Slightly randomize vertical position within the tile on placement for variety (+/- 4px offset).

---

## CONNECTIVITY SUMMARY TABLE (FULL SET)

All tiles from the 2026-05-06 base file plus this extended set:

| Tile ID | Name                    | Left edge | Right edge | Top edge | Bottom edge | Canvas     |
|---------|-------------------------|-----------|------------|----------|-------------|------------|
| A1      | Floor Base              | #161620   | #161620    | #161620  | #161620     | 64x32px    |
| A2      | Floor Cracked           | #161620   | #161620    | #161620  | #161620     | 64x32px    |
| A3      | Floor Mossy             | #161620   | #161620    | #161620  | #161620     | 64x32px    |
| A4      | Floor Wet Stone         | #161620   | #161620    | #161620  | #161620     | 64x32px    |
| B1      | West Wall Straight      | #1a1c28   | #1a1c28    | #3a3c50  | #12141a     | 64x96px    |
| B2      | South Wall Straight     | #1a1c28   | #1a1c28    | #3a3c50  | #12141a     | 64x96px    |
| B3      | Outer Corner            | #1a1c28   | #1a1c28    | #3a3c50  | #12141a     | 64x96px    |
| B4      | Inner Corner            | #1a1c28   | #1a1c28    | #3a3c50  | #12141a     | 64x96px    |
| B5      | West Wall End Cap       | #1a1c28   | open/trans  | #3a3c50  | #12141a     | 64x96px    |
| B6      | South Wall End Cap      | #1a1c28   | #1a1c28    | open/cap  | #12141a     | 64x96px    |
| B7      | Pillar                  | open/trans | open/trans | #3a3c50  | #12141a     | 64x96px    |
| B8      | Doorway Opening         | #1a1c28   | #1a1c28    | #3a3c50  | #12141a     | 64x96px    |
| C1      | Wall Torch Sconce       | transparent | transparent | transparent | transparent | 64x96px |
| C2      | Rubble Pile             | transparent | transparent | transparent | transparent | 64x32px |
| C3      | Fallen Stone Block      | transparent | transparent | transparent | transparent | 64x64px |
| C4a     | Chain Ring (West Wall)  | transparent | transparent | transparent | transparent | 64x96px |
| C4b     | Chain Ring (South Wall) | transparent | transparent | transparent | transparent | 64x96px |

---

## UNITY RULETILE SETUP (EXTENDED)

### Tilemap Layer Stack
- Layer 0: Floor -- Floor Rule Tile (A1-A4)
- Layer 1: Wall -- Wall Rule Tile (B1-B8, see rules below)
- Layer 2: Object -- Plain Tile assets (C1-C4, placed manually)

### Floor Rule Tile
One Rule Tile asset containing A1(40%), A2(30%), A3(20%), A4(10%).
All outputs: seamless because all edges are identical.

### Wall Rule Tile (Extended)
Rule evaluation order (highest priority first):

1. Doorway (B8): manually placed -- remove from Rule Tile, use plain Tile asset.
2. Pillar (B7): manually placed -- plain Tile asset.
3. Outer Corner (B3): neighbor[Left]=Wall AND neighbor[Down]=Wall AND neighbor[LeftDown]=Empty
4. Inner Corner (B4): neighbor[Left]=Wall AND neighbor[Down]=Wall AND neighbor[LeftDown]=Wall
5. West End Cap (B5): neighbor[Left]=Wall AND neighbor[Right]=Empty
6. South End Cap (B6): neighbor[Up]=Empty AND neighbor[Down]=Wall
7. West Straight (B1): neighbor[Left]=Wall AND neighbor[Right]=Wall
8. South Straight (B2): neighbor[Up]=Wall AND neighbor[Down]=Wall
9. Default: B1 or B2 based on orientation hint (tag each cell with orientation flag).

PPU for all tiles: 128 (S43 spec).
Floor tile pivot: center-bottom of diamond.
Wall tile pivot: bottom-center of 64x96 canvas.
Object tile pivot: bottom-center.

### Object Layer Placement Guidelines
- Wall Torch (C1): 1 per 6-10 wall segments average. Never two adjacent.
- Rubble (C2): scatter on floor, density 5-10% of floor cells. Never on wall cell.
- Fallen Block (C3): rare, 1-3 per room, away from doorways.
- Chain Ring (C4): 1-2 per dungeon corridor, preferably near dead ends or former cell walls.

---

## GENERATION ORDER

Generate in this sequence -- each tile must be approved before using it as reference for the next:

Phase 1 -- Base walls (one face at a time):
  1. B1 (West Wall Straight) -- primary wall reference
  2. B2 (South Wall Straight) -- confirm face consistency with B1

Phase 2 -- Corners (require B1+B2 as visual reference):
  3. B3 (Outer Corner) -- reference B1 and B2 edge colors
  4. B4 (Inner Corner) -- reference B1 and B2 edge colors

Phase 3 -- Terminus tiles:
  5. B5 (West End Cap)
  6. B6 (South End Cap)

Phase 4 -- Special wall tiles:
  7. B7 (Pillar)
  8. B8 (Doorway)

Phase 5 -- Object overlays (no dependencies on each other):
  9. C1 (Torch Sconce)
  10. C2 (Rubble Pile)
  11. C3 (Fallen Block)
  12. C4a and C4b (Chain Rings, generate as a pair)

QC after each phase: check edge pixels at 1:1 zoom against edge contract before proceeding.
If PixelLab adds anti-aliasing on tile edges: reject and regenerate with added instruction
"strictly hard pixel edges on all tile borders, no sub-pixel blending, no anti-aliasing at tile perimeter".

---

## CROSS-REFERENCE

Base floor and wall prompts: STAGING/PIXELLAB_TILESET_UNDERCROFT_2026-05-06.md
Wall torch sconce in base file (Tile 6) superseded by C1 (more detailed, transparent background).
Masonry-to-rock transition (Tile 7) and floor-to-earth transition (Tile 8) in base file remain valid
for biome boundary use -- not duplicated here.
