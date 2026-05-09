# PixelLab Üretim Kılavuzu — Dungeon Assets (Undercroft)
Date: 2026-05-07 | Status: READY TO GENERATE
Biome: Undercroft (Act 1 — Shattered Keep)

---

## YAKLAŞIM: HERO ANCHOR + style_images CHAIN

Floor tile batches (F1-F4) use `create_tiles_pro` MCP — NOT website Tileset Generation.
Wall and prop batches (W-series, O-series, VS-series) remain on website Create Image PRO.

**Why:** F1/F2/F3/F4 are the same stone at different decay states, not separate sets.
Coherence is mandatory. Website tool has no API-accessible style_images chaining.

### Edge Contract (unchanged)
| Surface | HEX |
|---------|-----|
| Floor diamond edges (all 4 sides) | #161620 |
| Wall left/right profile edges | #1a1c28 |
| Wall top surface | #3a3c50 |
| Wall base edge | #12141a |

### Coherence Protocol — 3 Steps

**STEP 1 — Hero Batch (F1, first 8 variants):**
```
tool: create_tiles_pro MCP
tile_type: isometric
tile_size: 64
tile_view: top-down        ← NOT "low top-down"
tile_depth_ratio: 0.0      ← flat floor, no depth
outline_mode: segmentation ← NOT "outline"
seed: <pick any fixed integer, keep same across all floor calls>
description: numbered (see F1 section below)
style_images: null         ← hero call only
```

**STEP 2 — Extract anchor:**
Call `get_tiles_pro` → wait for status: completed → pick cleanest output tile
→ save its base64 + width(64) + height(64) as ANCHOR

**STEP 3 — All subsequent floor batches (F1 variants 9-16, F2, F3, F4):**
```
style_images: [{"base64": "<ANCHOR>", "width": 64, "height": 64}]
style_options: {"color_palette": true, "outline": true, "detail": true, "shading": true}
NOTE: when style_images is set, tile_type/tile_size/tile_view/tile_depth_ratio are IGNORED
      (geometry is derived from reference image dimensions)
Max 8 variants per call at 64px.
Same seed across all floor calls.
```

**FORBIDDEN:** style_images empty after Step 2 | tile_view: low top-down | outline_mode: outline | one call per variant

---

## ÜRETİM SIRASI ÖZET

| Sıra | Asset | Tool | Canvas | Variants |
|------|-------|------|--------|---------|
| 1 | F1 Base Floor | Tileset Generation | 64×64 | 16 |
| 2 | F2 Cracked Floor | Tileset Generation | 64×64 | 12 |
| 3 | F3 Mossy Floor | Tileset Generation | 64×64 | 8 |
| 4 | F4 Wet Stone | Tileset Generation | 64×64 | 6 |
| 5 | W1 + W2 | Create Image Pro (Pixflux) | 64×96 | — |
| 6 | W3 + W4 + W5 | Create Image Pro (Pixflux) | 64×96 | — |
| 7 | O2 + O3 | Create Image Pro (Pixflux) | 64×96 / 64×32 | — |
| 8 | O1 | Create Image Pro (Pixflux) | 32×64 | — |
| 9 | O7 + O7b | Create Image Pro (Pixflux) | 64×64 | — |
| 10 | O6 | Create Image Pro (Pixflux) | 128×128 | — |
| 11 | O4 + O5 | Create Image Pro (Pixflux) | 64×96 / 64×48 | — |
| 12 | VS1 + VS2 | Create Image Pro (Pixflux) | 128×64 / 128×192 | — |

---

## FLOOR BATCH 1 — F1 Base Floor (16 Variants, 2 Calls)

**Tool:** `create_tiles_pro` MCP

### Call 1A — Hero Call (variants 1-8, no style_images):
```
tile_type: isometric | tile_size: 64 | tile_view: top-down
tile_depth_ratio: 0.0 | outline_mode: segmentation | seed: <fixed>
style_images: null

description:
"1). isometric dungeon floor, constructed masonry, 2x2 cut stone blocks,
block faces #1e2030 and #262838, mortar joints #161620 1px, worn flat matte
surface, edge pixels #161620, cold muted palette, pixel art no anti-aliasing
2). same stone, smooth from foot traffic, faint boot polish on center block
3). same stone, hairline diagonal crack on one block face, crack #0e0e18 1px
4). same stone, fine chisel marks visible on two blocks, parallel 1px grooves
5). same stone, slight edge erosion one corner, 2-3 chip pixels #14141e
6). same stone, barely visible carved groove pattern, shallow 1px incisions
7). same stone, one block face slightly recessed, aged settlement
8). same stone, subtle flint inclusion 2px pale spot #2a2c40 on one block"
```

→ get_tiles_pro → pick best output → save as ANCHOR for all subsequent calls

### Call 1B — Variants 9-16 (uses ANCHOR via style_images):
```
style_images: [{"base64": "<ANCHOR>", "width": 64, "height": 64}]
style_options: {"color_palette": true, "outline": true, "detail": true, "shading": true}
seed: <same fixed seed>

description:
"1). dungeon floor same stone, plain worn surface
2). same stone, faint water stain ring, dried mineral edge
3). same stone, two small debris pixels at one corner #14141e
4). same stone, mortar joint slightly wider on one seam
5). same stone, micro-crack 3px at block corner
6). same stone, one block face with subtle compression mark
7). same stone, faint dust accumulation in mortar joint
8). same stone, overall most worn variant, smoothed by age"
```

**After generation:** discard weak results, assign keepers to F1 pool → Unity RuleTile equal-weight random.

---

## FLOOR BATCH 2 — F2 Cracked Floor (8-12 Variants)

**Tool:** `create_tiles_pro` MCP | **Requires:** F1 ANCHOR from Step 2

```
style_images: [{"base64": "<F1_ANCHOR>", "width": 64, "height": 64}]
style_options: {"color_palette": true, "outline": true, "detail": true, "shading": true}
seed: <same fixed seed>

description:
"1). dungeon floor, same stone as reference, one block face has diagonal hairline crack
corner to corner #0e0e18 1px, 2-3 loose debris pixels at crack terminus #14141e
2). same stone, spiderweb fracture radiating from block center, cracks #0e0e18
3). same stone, two parallel cracks on one block, slight displacement
4). same stone, crack runs through mortar joint and across adjacent block
5). same stone, heavy crack with 2px gap suggesting block shift
6). same stone, crack with faint white mineral deposit along edge #1e1e2e
7). same stone, crack running diagonally across full tile corner to corner
8). same stone, multiple micro-cracks on two different blocks"
```

---

## FLOOR BATCH 3 — F3 Mossy Floor (8 Variants)

**Tool:** `create_tiles_pro` MCP | **Requires:** F1 ANCHOR from Step 2

```
style_images: [{"base64": "<F1_ANCHOR>", "width": 64, "height": 64}]
style_options: {"color_palette": true, "outline": true, "detail": true, "shading": true}
seed: <same fixed seed>

description:
"1). dungeon floor, same stone as reference, dark moss colony #1e2e14 covering
15-20% of one block face, irregular pixel blob 4-6px wide, accent #263a1a on edge
2). same stone, moss in mortar joints only, thin dark green line #1a2810
3). same stone, moss on two adjacent blocks, spreading pattern
4). same stone, heavy moss on one full block face 40% coverage
5). same stone, moss patch near one diamond edge corner
6). same stone, moss with 1px lighter spore dots #2a3820 on colony edge
7). same stone, dried dead moss, darker #141a0e, crumbling edge
8). same stone, moss + hairline crack combination"
```

---

## FLOOR BATCH 4 — F4 Wet Stone (6 Variants — Optional)

**Tool:** `create_tiles_pro` MCP | **Requires:** F1 ANCHOR from Step 2

```
style_images: [{"base64": "<F1_ANCHOR>", "width": 64, "height": 64}]
style_options: {"color_palette": true, "outline": true, "detail": true, "shading": true}
seed: <same fixed seed>

description:
"1). dungeon floor, same stone as reference, moisture sheen on one block face,
2-3 highlight pixels #3a3c50 along inner block edge, cool not glossy, no sparkle
2). same stone, thin water film, faint cool blue-grey shift #141a20 on one block
3). same stone, water pooled in mortar joint, 1px dark fill #0e1218
4). same stone, damp stone darker overall, slightly cooler hue shift
5). same stone, dried water stain ring #161a20 on block face surface
6). same stone, moisture + moss combination, wet moss #162010"
```

---

## BATCH 2 — W1 + W2 (Düz Duvarlar)

**Git:** PixelLab → **Create Image PRO**

| Ayar | Değer |
|------|-------|
| Canvas width | 64 |
| Canvas height | 96 |
| Style tiles | *(boş bırak)* |

Her biri için ayrı üretim yap.

**W1 — West Wall (Batı Duvarı):**
```
isometric pixel art wall tile 64x96px, west wall straight run segment. The wall runs horizontally in world space; camera sees the south face as a narrow vertical strip on the right side of the tile and the wide west face occupies the left two-thirds of the tile. West face: 3 rows of cut stone blocks, block face color #2e3040 with per-block variation #2a2c3c and #323446, horizontal mortar joints #12141e 1px wide. Block height approximately 20px each row. Top surface strip 16px: color #3a3c50, cut flat, 1px highlight at very top edge #464858. Shadow south face (right strip, 16px wide): vertical gradient #1a1c28 at top to #12141a at base. Block corners show 2-3px chip marks #12141e. Left edge of tile: #1a1c28 seamless. Right edge of tile: #1a1c28 seamless. Top edge: #3a3c50. Base edge: #12141a. Hard pixel edges, no anti-aliasing, no blur. Muted desaturated palette, weathered field-worn. Constructed masonry, natural mortar joints, worn edges.
```

**W2 — South Wall (Güney Duvarı):**
```
isometric pixel art wall tile 64x96px, south wall straight run segment. The wall runs vertically in world space; camera sees the wide south face occupying the right two-thirds of the tile and the narrow west face shadow strip on the left. South face: 3 rows of cut stone blocks running across the full width of the tile, block face color #2e3040 with per-block variation #2a2c3c and #323446, mortar joints #12141e 1-2px horizontal and vertical. Block width approximately 20px each, 3 blocks per row. Top surface strip 16px: color #3a3c50 cut flat. Left shadow face (8px wide strip): vertical gradient #1a1c28 top to #12141a base. Block chip marks 2-3px at corners #12141e. Top edge: #3a3c50 seamless. Base edge: #12141a seamless. Left edge: #1a1c28. Right edge: #1a1c28. Hard pixel edges, no anti-aliasing. Muted desaturated palette, weathered field-worn. Constructed masonry, aged stone, natural mortar joints.
```

---

## BATCH 3 — W3 + W4 + W5 (Köşeler + End Cap)

**Git:** PixelLab → **Create Image PRO** | Canvas: **64 × 96** | Style tiles: **boş**

Her biri için ayrı üretim yap.

**W3 — Outer Corner (Dış Köşe — dışbükey, kameraya doğru çıkıntı):**
```
isometric pixel art wall tile 64x96px, outer corner where west wall and south wall meet. The corner protrudes toward the camera (convex, not recessed). At the center-top of the tile the two wall faces meet at a vertical stone corner pilaster -- a 4px wide vertical ridge of darker stone #1e2030 with 1px highlight #3a3c50 on the exact corner edge. West face fills left portion of tile: cut stone blocks #2e3040 variation #2a2c3c #323446, mortar joints #12141e. South face fills right portion of tile: same block colors and mortar. Top surface: L-shaped #3a3c50 strip spanning both faces meeting at corner. Shadow gradient on west face: #1a1c28 to #12141a. The corner ridge itself catches slight highlight: top 2px of ridge #464858. Left edge: #1a1c28. Right edge: #1a1c28. Top edge: #3a3c50 across full width. Base edge: #12141a. Block chip marks near corner #12141e. Hard pixels, no anti-aliasing. Muted desaturated palette, weathered field-worn.
```

**W4 — Inner Corner (İç Köşe — içbükey, kameradan uzaklaşan gölgeli köşe):**
```
isometric pixel art wall tile 64x96px, inner corner where two wall runs meet, concave corner receding away from camera. The corner is at the back of the tile -- walls meet in shadow. West face portion on left: cut stone blocks #2e3040 #2a2c3c #323446 running toward the corner. South face portion on right: same blocks running toward the corner from the other side. At the inner corner junction (center-back): a vertical crevice 2px wide, color #0e0e18, deepest shadow point. The stone blocks nearest the crevice are slightly darker #1e2030 indicating shadow depth. Top surface: L-shaped #3a3c50 strip, inner corner of L also #0e0e18 at the very back point. Left edge: #1a1c28. Right edge: #1a1c28. Top edge: #3a3c50. Base edge: #12141a. Hard pixels no anti-aliasing. Muted desaturated palette, weathered field-worn. The concave corner creates a deep shadow pocket at its back.
```

**W5 — West Wall End Cap (Duvar sonu — blok kesilmesi):**
```
isometric pixel art wall tile 64x96px, west wall end cap -- the wall terminates here, no continuation beyond. The west face occupies the left portion of the tile with full stone block rows. At the right side of the tile where the wall ends: a finished stone end face, 8px wide, showing the cut cross-section of the wall thickness. End face color #262838 slightly different from front face to show it is a perpendicular surface. Top edge of end face: 2px strip #3a3c50 showing wall thickness. The end face has 3 horizontal mortar-line marks #12141e matching the block row heights to indicate the wall is made of stacked courses. No continuation of blocks beyond the end face -- right side of tile is empty (transparent or open). Left edge: #1a1c28 seamless. Top edge: #3a3c50. Base edge: #12141a. Right edge: open/transparent beyond end face. Hard pixels, no anti-aliasing. Muted desaturated palette, weathered field-worn. Natural stone masonry terminus, slightly worn corner edge.
```

---

## BATCH 4 — O2 + O3 (Sütun + Moloz)

**Git:** PixelLab → **Create Image PRO** | Style tiles: **boş**

**O2 — Stone Pillar (Taş Sütun)** | Canvas: **64 × 96**
```
isometric pixel art dungeon prop, transparent background, 64x96px canvas, freestanding square pillar / column. Both the south face and west face of the pillar are visible simultaneously as the column stands alone. Pillar body: a square column approximately 24px wide. South face of pillar: 3 stacked block sections #2e3040 with mortar joints #12141e and per-block variation #2a2c3c #323446. West face of pillar: same block stack but in shadow #1a1c28 gradient to #12141a, slightly narrower face (12px) due to viewing angle. Top cap of pillar: a square capital stone, flat top surface #3a3c50 with a 2px chamfered edge #464858 around its perimeter. Pillar base: a wider plinth 4px on each side, same stone color with slightly darker base #1e2030. Block chip marks at corners and edges #12141e. Left side of tile beyond pillar: transparent/open. Right side: transparent/open. Hard pixels, no anti-aliasing. Muted desaturated palette, weathered field-worn. Load-bearing architectural column, not decorative.
```

**O3 — Rubble Pile (Moloz Yığını)** | Canvas: **64 × 32**
```
isometric pixel art dungeon prop overlay, transparent background, 64x32px canvas, rubble pile of broken stone fragments on dungeon floor. Rubble pile: irregular mound of broken stone chunks, 40px wide at base, 18px tall at highest point (isometric volume). Stone chunk colors: #262838 large chunks, #1e2030 medium, #12141a shadow sides. Individual chunks: 4-8px irregular polygonal shapes with 1px dark outline #0e0e18. Mortar dust between chunks: scattered 1-2px pixels #2a2830 suggesting pulverized mortar. Chunk arrangement: largest chunk at center-top, smaller chunks radiating outward and downward, smallest debris pixels at perimeter. Shadow of pile cast on floor (south side): 6px dark area #12141a bordering the pile base. One chunk: slightly lighter #3a3c50 on top face showing caught ambient light. Tile edges: fully transparent. Hard pixels no anti-aliasing. Muted desaturated palette, weathered field-worn.
```

---

## BATCH 5 — O1 (Meşale)

**Git:** PixelLab → **Create Image PRO** | Style tiles: **boş**

**O1 — Wall Torch Sconce (Duvar Meşalesi)** | Canvas: **32 × 64**
```
isometric pixel art dungeon prop overlay, transparent background, 32x64px canvas, wall torch sconce mounted on west-facing stone wall. Background fully transparent except for the sconce object. Sconce bracket: wrought iron L-bracket, 5px wide, color #1c1c24, 1px highlight edge #28282e on top surface, attached to wall at horizontal center of canvas and 40% from top. Bracket arm extends 6px outward from wall face. Torch cup at bracket end: 4px wide circular cup #1c1c24 holding torch stick. Torch stick: 3px wide, 8px tall, color #2a1810 (charred wood). Flame above torch tip: 5px wide, 7px tall, warm amber flame, base color #b04010 becoming #d06020 at midpoint and #e08030 at tip, leftmost flame pixel slightly cooler #c05020. Warm light spill: 4 pixels at flame base color #3a2820 at 50% opacity hint. Sconce shadow on wall: 2px dark ellipse below bracket #0e0e18. All background outside sconce: fully transparent. Hard pixels, no anti-aliasing.
```

---

## BATCH 6 — O7 + O7b (Sandık Kapalı + Açık)

**Git:** PixelLab → **Create Image PRO** | Canvas: **64 × 64** | Style tiles: **boş**

Her biri için ayrı üretim yap.

**O7 — Reward Chest (closed):**
```
isometric pixel art dungeon prop, transparent background, 64x64px canvas. Closed reward chest, isometric view showing south face and top. Body: wood planks #3d2810, plank lines horizontal 6px intervals #2a1c0a 1px. South face: two visible planks plus iron corner straps. Iron corner straps: 4px wide vertical bars #1c1c24 at left and right edges of south face, highlight edge #2a2a36. Iron lock plate centered on south face: 8x8px rectangle #1c1c24, keyhole 2x3px void #0a0a10. Top surface lid: wood #3d2810, iron hinge band across full width back edge 4px #1c1c24, 1px highlight at very front top edge #464858. Iron corner brackets at all four lid corners: 4px L-shaped #1c1c24. Subtle ambient occlusion along base edge #0e0e18 1px. Hard pixel edges, no anti-aliasing, no blur. Muted desaturated palette, weathered field-worn.
```

**O7b — Reward Chest (open):**
```
isometric pixel art dungeon prop, transparent background, 64x64px canvas. Open reward chest, lid pivoted fully back, isometric view. Body identical to closed version: wood planks #3d2810, plank lines #2a1c0a, iron corner straps #1c1c24, lock plate #1c1c24 now open/unlatched. Lid: rotated back 110 degrees, visible as a narrow top-facing strip behind the chest body, lid underside dark #1c1008 facing forward. Interior cavity: dark base #12141a, inner walls #1c1008 wood. Gold glint inside: 3-4 scattered 1-2px pixels #b08020 at bottom of interior suggesting coins, faint warm light bounce #6b4c10 on inner front wall just above base. Interior shadow gradient: #12141a at base to #1a1c28 toward rim. Iron hinge visible as pivot point on back top edge: 4px cylinder suggestion #1c1c24. Hard pixel edges, no anti-aliasing, no blur. Muted desaturated palette, weathered field-worn.
```

---

## BATCH 7 — O6 (Lahit — Landmark)

**Git:** PixelLab → **Create Image PRO** | Canvas: **128 × 128** | Style tiles: **boş**

```
isometric pixel art dungeon landmark prop, transparent background, 128x128px canvas. Large stone sarcophagus, lid half-open and cracked. Sarcophagus body: cut stone block construction, body color #2e3040 with per-block variation #2a2c3c and #323446, mortar joints #12141e. South face visible: two rows of carved stone panel recesses, panel borders 2px #12141e. Top surface: lid resting at 30-degree angle, right side lifted, left hinge side still resting on body. Lid top face color #3a3c50. Crack running diagonally across lid center: 2px jagged line #0e0e18 with hairline bright edge #464858 on either side. Runes carved on lid top face: faint geometric symbols, color #464858 on #3a3c50 background, nearly invisible. Interior visible through gap: dark void #0a0a10 with faint pale suggestion #c8b89a dust at far interior edge. Stone chip debris 2-3px fragments #1a1c28 scattered around base. Shadow cast by lifted lid: vertical gradient #1a1c28 to #12141a on body south face. Hard pixel edges, no anti-aliasing, no blur. Muted desaturated palette, weathered field-worn.
```

---

## BATCH 8 — O4 + O5 (Kafes + Kemik Yığını)

**Git:** PixelLab → **Create Image PRO** | Style tiles: **boş**

**O4 — Iron Cage (empty)** | Canvas: **64 × 96**
```
isometric pixel art dungeon prop, transparent background, 64x96px canvas. Empty iron cage, prison cell style. Vertical iron bars 4px wide spaced 10px apart, bar color #1c1c24 with highlight edge #2a2a36 on left face and deep shadow #0e0e18 on right face. Top frame: square iron border 4px thick, same color. Base ring: circular/oval iron ring at ground level, 6px thick. Rust streaks along bars: #3d1a0a flecks 1-2px scattered, denser at joints and base. Cage door on south-facing side: door frame slightly offset 2px inward, door bars match main bars, closed but no padlock -- latch hook #1c1c24 rests in open position. Interior: dark void #0a0a10. Cast shadow on floor beneath base ring: #0e0e18 ellipse. Hard pixel edges, no anti-aliasing, no blur. Muted desaturated palette, weathered field-worn.
```

**O5 — Bone Pile** | Canvas: **64 × 48**
```
isometric pixel art dungeon prop, transparent background, 64x48px canvas. Scattered bone pile at ground level, spread across lower two-thirds of canvas. Skull centered near top of pile: dome color #c8b89a, eye sockets #12141e hollow 3x2px each, jaw slightly open showing 2px teeth marks #a09070. Long bones (femur, tibia) arranged diagonally beneath skull in overlapping pile: color #a09070 with highlight edge #c8b89a on upper faces and shadow underside #6e6050. Rib fragments and small bone chips fill gaps: #a09070 irregular 2-3px pieces. Pile silhouette organic and uneven, widest at base tapering toward skull. Slight dust/dirt discoloration at contact points: #2a2c3c flecks 1px. Hard pixel edges, no anti-aliasing, no blur. Muted desaturated palette, weathered field-worn.
```

---

## BATCH 9 — VS1 + VS2 (Oynanamaz Sınır — En Son Üret)

**Git:** PixelLab → **Create Image PRO** | Style tiles: **boş**

**VS1 — Chasm Drop Edge (Uçurum Kenarı)** | Canvas: **128 × 64**
```
isometric pixel art environment tile, transparent background, 128x64px canvas. Chasm drop edge, playable stone floor ends abruptly at void below. Top half of canvas: isometric stone floor surface matching dungeon floor, tile color #2a2c3c, grout lines #12141e 1px grid, worn surface flecks #323446. Floor edge runs diagonally across canvas center (isometric perspective line). Edge break: jagged irregular pixel fracture line, not straight, 2-4px deviation, edge color #161620. Below edge line: sheer stone face vertical drop, 4px strip #1a1c28 gradient top to #12141a. Below that: abyss gradient #12141a transitioning to #0a0a10 at 75% depth then to pure #000000 at base. Small stone chip fragments 1-2px #1a1c28 on edge break. Hard pixel edges, no anti-aliasing, no blur. Muted desaturated palette, weathered field-worn.
```

**VS2 — Collapsed Archway (Çökmüş Kemer)** | Canvas: **128 × 192**
```
isometric pixel art dungeon boundary prop, transparent background, 128x192px canvas. Large collapsed stone archway, non-traversable decorative boundary. Upper third of canvas: arch remnant still standing, keystone visible at apex, arch stones #2e3040 with variation #2a2c3c and #323446, mortar joints #12141e 1px, arch profile broken off on right side with jagged fractured edge #1a1c28. Middle section: partially collapsed pillar left side, stones stacked unevenly, right pillar fully collapsed. Rubble pile occupying middle and lower canvas: large stone blocks 20-30px #2e3040 tumbled at irregular angles, per-block chip marks #12141e at corners, dust powder #323446 scatter between blocks. Small debris fragments 2-4px #1a1c28 filling gaps. Ground-level rubble spread widest at base, irregular organic silhouette. Dust haze at collapse points: 1px #2a2c3c stipple. Hard pixel edges, no anti-aliasing, no blur. Muted desaturated palette, weathered field-worn.
```
