# RIMA Asset Production Prompts v1

Scope: Fractured Chamber asset prompt/spec reference for manual PixelLab Web UI and Aseprite production. No assets were generated in this pass.

Source checks run:
- Read `CODEX_TASK_laurethayday.md`.
- Read `CODEX.md`; `ANTIGRAVITY.md` was not present at repo root.
- Read local PixelLab docs: `PixelLabDocs/create-sl-image-pro.md`, `PixelLabDocs/create-image-flux.md`, `PixelLabDocs/create-tiles-pro.md`, `PixelLabDocs/create-tileset.md`, `PixelLabDocs/mcp_docs.md`, `PixelLabDocs/pixellab_tool_map.md`.
- Read `MEMORY/feedback_pixellab_init_image_dimension_lock.md`.
- Checked existing RIMA tileset inventory: `STAGING/all_tilesets_inventory.json`.
- Opened local style references under `STAGING/concepts/chatgpt_ref/new_chatgpt/*.png`.

## Tool Capability Findings

PixelLab Create S-XL Image (Pro) docs say square canvases are supported from 16x16 up to 512x512; non-square canvases are supported by closest aspect ratio, with a documented example up to 688x384 for 16:9. The local docs do not support 1024x1024 or 1024x768 for S-XL Pro, and M-XL/pixflux is area-limited by tier rather than offering 1024-class production in the docs checked. Init image use in S-XL new locks or proportionally constrains output dimensions, so it is risky for mixed-aspect or strict sheet layout work. Create Tiles Pro is documented as a current Pro tool, not listed under experimental tools, and supports top-down square tiles at 16, 32, 48, 64, 96, and 128 px. The MCP top-down Wang tileset workflow is documented as a stable 16-tile corner-based tileset system; no active outer-corner bug was found in the local docs or RIMA inventory, but manual outer-corner QC remains required after generation.

## Final Workflow Decisions

A1 final workflow: use Create Tiles Pro first for the 64x64 top-down square floor set, because it directly supports 64 px square tiles and is suitable for seamless game-map tile output. Keep the Create Image Pro 4-cell sheet as fallback when the user needs hand-picked art-directed variants or if manual QC finds seam or outer-corner issues in the generated tile set.

A6 final workflow: use prompt-only Create S-XL Image (Pro) as a 512x512 4x4 modular backwall sheet with 128x128 cells, because 768x768 and 1024x1024 are not supported by the checked S-XL Pro docs. If 192x192 or 256x256 source cells are mandatory later, split A6 into four row sheets or individual landmark cells; do not depend on a 1024-class single sheet until the Web UI exposes and verifies that size. Init image is not recommended for A6 v1 because dimension locking can fight the 4x4 sheet layout; use a style image only if the Web UI exposes a separate style-reference slot that does not lock output dimensions.

Shared style lock for every generated asset:
- Dark fantasy pixel art.
- RIMA Shattered Keep / Fractured Chamber mood.
- Fractured charcoal granite, cyan rift cracks, amber torch highlights.
- Near-pure top-down ARPG 85-90 degree angle; subtle 3/4 sprite styling only.
- No true isometric 30/45 degree projection.
- No characters, no UI.
- Transparent background where the asset is not a full floor tile.
- Base decor emissive stays low/medium; VFX and combat telegraph emissive can be high.

Reference style observations from local ChatGPT renders:
- Stone palette: dark warm charcoal granite, blue-cyan rim accents in cracks, restrained amber firelight.
- Tile language: broken rectangular flagstones, chipped slab edges, dust in mortar, scattered rubble at boundaries.
- Readability: cyan rifts are thin and high contrast; amber torch light is local and does not wash the whole sprite.
- Modular wall direction: north wall/backwall reads as a band with aligned bottom baseline; large pillars in references are useful visually but A6 v1 is pillar-less unless the cell explicitly calls for a socket or landmark.
- Room composition: floor remains readable and mostly low-emissive; identity comes from backwall landmarks, portals, prison bars, library shelves, sarcophagus, ritual stones, props, and decals.

## A1 - Floor Tileset

**Hedef:** Fractured dark granite floor tiles for the Fractured Chamber foundation layer, with clean, cracked, rift-glow, and broken-corner variants.

**Boyut:** 64x64 per tile; Create Tiles Pro sheet output should be sliced as 64x64 tiles. Fallback sheet is 256x256 total with four 128x128 cells, downscaled/imported to 64x64 visual target if needed.

**Workflow:** Recommended: PixelLab Create Tiles Pro, top-down square, tile_size 64, 16 tiles. Fallback: Create S-XL Image (Pro) or Create Image Pro 4-cell sheet, 256x256, transparent/background optional based on Web UI behavior.

**PixelLab Prompt - Workflow A, Create Tiles Pro (copy-paste ready):**
> 1. clean fractured dark granite floor tile, 2. cracked fractured dark granite floor tile, 3. dark granite floor tile with sparse low-emissive cyan rift hairline cracks, 4. broken corner dark granite floor tile with chipped slab edge, 5-16. seamless transition and corner variants between those four floor states, all tiles are flat walkable ground at the same height, dark fantasy pixel art, fractured granite, cyan rift cracks, amber torch highlights only as subtle reflected flecks, top-down ARPG 85-90 degree angle, NO characters, NO UI, no props, no walls, no raised bevels, no baked directional light, tileable square top-down floor texture, restrained gritty RIMA Shattered Keep palette, charcoal grey stone with tiny dust and mortar pixels, readable at 64x64.

**PixelLab Prompt - Workflow B, 4-cell fallback sheet (copy-paste ready):**
> Four-cell 2x2 pixel art floor variant sheet, total canvas 256x256, each cell exactly 128x128, cell 1 clean fractured dark granite floor, cell 2 cracked fractured granite floor, cell 3 fractured granite floor with sparse low-emissive cyan rift cracks, cell 4 broken-corner granite floor with chipped slab and small rubble at one corner, all four cells share the same stone palette, same top-down ARPG 85-90 degree angle, same scale, same lighting, dark fantasy pixel art, fractured granite, cyan rift cracks, amber torch highlights only as very subtle reflected accents, NO characters, NO UI, no walls, no props, no shadows crossing cell borders, seamless-looking flat walkable ground.

**Negative Prompt:**
> characters, enemies, weapons, UI, text, labels, true isometric view, 30 degree isometric projection, thick outlines, big glow bloom, large portal, wall, pillar, stairs, water, grass, wood planks, baked torch cone, perspective room render, camera tilt, non-tileable edges.

**Unity Import:**
| Setting | Value |
|---|---|
| PPU | 64 |
| Sprite Mode | Multiple |
| Slice | Workflow A: Grid by Cell Size 64x64; Workflow B: Grid by Cell Size 128x128 then use at 64 PPU or downscale externally if approved |
| Filter Mode | Point (no filter) |
| Compression | None |
| Pivot | Bottom |
| Mesh Type | Tight |
| Read/Write | Disabled |

**Notes / Risks:** Create Tiles Pro is preferred, but manual QC must check all outer-corner and diagonal-corner joins. If Create Tiles Pro returns 32 px source tiles through a MCP path, use Web UI Create Tiles Pro at 64 px or upscale only after checking pixel crispness. Keep cyan cracks thin; floor crack emissive must stay lower than telegraph VFX.

Production checklist:
- Generate one Create Tiles Pro candidate first.
- Inspect pure floor tile, rift tile, and all corner joins on a checkerboard preview.
- Reject if the floor reads as wall, cliff, or raised platform.
- Reject if cyan crack density competes with combat telegraphs.
- Keep best 16-tile output and import as the foundation tile palette.

## A2 - Edge 4-cell Sheet

**Hedef:** Low broken stone edge pieces for fractured room boundaries, not full walls.

**Boyut:** 256x256 total, transparent background. Cell 1 is 128x64 inside its quadrant, Cell 2 is 64x128 inside its quadrant, Cell 3 is 64x64, Cell 4 is 64x64.

**Workflow:** Create S-XL Image (Pro) or Create Image Pro 4-cell sheet, one generation, transparent background.

**PixelLab Prompt (copy-paste ready):**
> Four-cell 2x2 modular edge sprite sheet on transparent background, total canvas 256x256, dark fantasy pixel art, fractured granite, cyan rift cracks, amber torch highlights, top-down ARPG 85-90 degree angle, NO characters, NO UI. Cell 1 top-left: north-facing straight low broken stone edge, 128x64 sprite footprint, dark fractured granite blocks forming a low boundary ledge with chipped top and rubble pixels. Cell 2 top-right: west/east side straight low broken stone edge, 64x128 sprite footprint, same stone color and same baseline height, designed to rotate or mirror for side coverage. Cell 3 bottom-left: compact outer corner edge, 64x64 footprint, broken granite corner cap with a few cyan hairline cracks. Cell 4 bottom-right: rubble cluster seam-cover piece, 64x64 footprint, loose dark stone chunks and dust for hiding edge joins. All cells share identical palette, lighting from above, low vertical height, and clean transparent padding around each sprite.

**Negative Prompt:**
> full wall, tall pillar, high cliff, true isometric projection, characters, UI, labels, floor-only tile, wood, grass, water, huge portal glow, baked flame, long cast shadow, mismatched cell scale, background.

**Unity Import:**
| Setting | Value |
|---|---|
| PPU | 64 |
| Sprite Mode | Multiple |
| Slice | Manual rectangles: 128x64, 64x128, 64x64, 64x64 |
| Filter Mode | Point (no filter) |
| Compression | None |
| Pivot | Bottom |
| Mesh Type | Tight |
| Read/Write | Disabled |

**Notes / Risks:** Edge matching is not required, but stone color, baseline height, and light direction must be consistent. The edge should read as a low room boundary, not a north backwall band. Use this to frame Battered Hall while keeping combat silhouettes clear.

Production checklist:
- Check that Cell 1 can sit on the north edge without hiding the floor.
- Check that Cell 2 reads correctly when mirrored.
- Check that Cell 3 covers an outside corner without creating an isometric cliff.
- Check that Cell 4 can hide joins without becoming a prop.

## A3 - Cyan Rift Crack Decal (linear)

**Hedef:** Linear cyan glowing floor crack decal for overlay placement on top of floor tiles.

**Boyut:** 128x64, transparent background.

**Workflow:** Aseprite hand-paint, one transparent PNG. Optional PixelLab reference prompt only if the user wants a loose concept image.

**Aseprite Paint Brief (copy-paste ready):**
> Paint a 128x64 transparent-background linear cyan rift crack decal for RIMA Fractured Chamber. Use dark fantasy pixel art language but only the crack and glow are visible; no stone background. Main crack is a jagged geometric line traveling left-to-right across the center third, with 6-9 hard angular segments and 2-3 tiny offshoots. Core pixels use bright cyan-white only at the thinnest center points, base crack color #4DD4FF, secondary glow #1AA8CC, outer halo sparse and transparent. Emissive level is medium, lower than combat telegraph VFX. Keep the shape readable when placed over dark fractured granite, with broken discontinuous pixels rather than a smooth neon stroke.

**PixelLab Prompt (reference only, copy-paste ready):**
> Single transparent-background linear cyan rift crack decal, 128x64, dark fantasy pixel art, fractured granite style influence but no stone background, cyan rift cracks, subtle amber torch highlights only as tiny warm edge flecks if any, top-down ARPG 85-90 degree angle, NO characters, NO UI, jagged geometric crack line crossing horizontally, #4DD4FF cyan core, sparse lighter cyan glow halo, medium emissive, transparent background, clean alpha, no floor tile, no portal, no spell circle.

**Negative Prompt:**
> stone background, floor tile, circular magic rune, character, UI, text, giant bloom, soft airbrush glow, blue fog cloud, lightning bolt, fire, orange crack, true isometric perspective.

**Unity Import:**
| Setting | Value |
|---|---|
| PPU | 64 |
| Sprite Mode | Single |
| Slice | None |
| Filter Mode | Point (no filter) |
| Compression | None |
| Pivot | Bottom |
| Mesh Type | Tight |
| Read/Write | Disabled |

**Notes / Risks:** This is best hand-painted because the asset needs clean alpha and exact emissive control. Keep opacity varied: 100% for the inner 1 px crack core, 45-70% for close glow pixels, 10-25% for the outer halo. Do not overfill the 128x64 canvas; empty transparent space helps placement.

Pixel-by-pixel reference description:
- Start near x=10, y=34; break rightward to x=28, y=29; kink down to x=42, y=36.
- Continue with a 1-2 px wide core toward x=68, y=31, then x=88, y=35, then x=116, y=28.
- Add one short branch around x=40, y=36 down-left for 9 px.
- Add one short branch around x=78, y=33 up-right for 12 px.
- Use single-pixel cyan sparks near branch tips only.
- Keep the glow asymmetric and broken, not a continuous outline.

## A4 - Cyan Rift Crack Decal (branching)

**Hedef:** Branching/web cyan crack overlay decal for larger room identity and rift focus points.

**Boyut:** 256x128, transparent background.

**Workflow:** Aseprite hand-paint, one transparent PNG. Optional PixelLab reference prompt only if the user wants a loose concept image.

**Aseprite Paint Brief (copy-paste ready):**
> Paint a 256x128 transparent-background branching cyan rift crack decal for RIMA Fractured Chamber. Use a central jagged fracture spine running diagonally from lower-left toward upper-right, then 3-5 angular branches spreading like broken glass across the middle of the canvas. Core color #4DD4FF, inner highlight near #B8F6FF at tiny intersection pixels, secondary glow #1AA8CC, sparse outer halo with transparent pixels. The crack should feel magical but not like a combat telegraph; emissive is medium, lower than active VFX. No floor background, no rune circle, no smooth lightning, only fractured geometric pixel cracks.

**PixelLab Prompt (reference only, copy-paste ready):**
> Single transparent-background branching cyan rift crack decal, 256x128, dark fantasy pixel art, fractured granite style influence but no stone background, cyan rift cracks, top-down ARPG 85-90 degree angle, NO characters, NO UI, 3-5 jagged branches from one central fracture spine, #4DD4FF cyan core with sparse lighter cyan glow halo, medium emissive, clean alpha, broken pixel geometry, no circular rune, no portal, no floor tile, no spell effect blast.

**Negative Prompt:**
> stone background, complete floor tile, circular magic circle, portal, character, UI, text, huge bloom, smoke, blue fog, soft gradient, thick lightning bolt, fire, true isometric perspective.

**Unity Import:**
| Setting | Value |
|---|---|
| PPU | 64 |
| Sprite Mode | Single |
| Slice | None |
| Filter Mode | Point (no filter) |
| Compression | None |
| Pivot | Bottom |
| Mesh Type | Tight |
| Read/Write | Disabled |

**Notes / Risks:** Use A4 less often than A3; it can dominate floor readability. Good use cases: Rift Gate Chamber center, Ritual Chamber under altar, flooded crypt emphasis, or a boss-room focal crack. Keep alpha clean around the halo.

Pixel-by-pixel reference description:
- Main spine: x=30,y=96 to x=70,y=80 to x=112,y=66 to x=160,y=54 to x=224,y=32.
- Branch 1: from x=78,y=78 to x=58,y=48 to x=44,y=30.
- Branch 2: from x=116,y=66 to x=126,y=98 to x=150,y=116.
- Branch 3: from x=160,y=54 to x=184,y=78 to x=210,y=84.
- Branch 4 optional: from x=190,y=44 to x=202,y=24.
- Place the brightest pixels only at intersections and broken core gaps.

## A5 - Prop 4-cell Sheet

**Hedef:** Four reusable Fractured Chamber props: brazier base, broken pillar base, rubble pile, and candle cluster.

**Boyut:** 256x256 total, 2x2 cells, 128x128 per cell. Final visible sprite footprints should stay mostly within 64x64-96x96; extra cell room keeps alpha and larger silhouettes safe.

**Workflow:** Create S-XL Image (Pro) or Create Image Pro 4-cell sheet, one generation, transparent background. Lit flame should be separated later as Aseprite/Unity particle or dedicated animation; base sprite emissive stays low.

**PixelLab Prompt (copy-paste ready):**
> Four-cell 2x2 prop sprite sheet on transparent background, total canvas 256x256, each cell 128x128, dark fantasy pixel art, fractured granite, cyan rift cracks, amber torch highlights, top-down ARPG 85-90 degree angle, NO characters, NO UI. Cell 1 top-left: amber brazier stone-and-metal base, dark granite bowl with small low amber ember glow only, no animated flame, footprint about 64x64. Cell 2 top-right: broken pillar base, fractured charcoal granite, chipped circular top, a few cyan hairline cracks, footprint about 80x80. Cell 3 bottom-left: rubble pile of dark stone chunks, dust and broken slab pieces, low silhouette, footprint about 80x64. Cell 4 bottom-right: candle cluster with 3 candles on a small dark stone base, tiny amber wick glow, footprint about 64x64. All props share the same scale, palette, top-down ARPG perspective, transparent padding, same light direction from above, and low base-decor emissive.

**Negative Prompt:**
> characters, UI, labels, large animated flame, huge glow bloom, full wall, floor tile sheet, wood furniture dominance, true isometric projection, large cast shadow, non-transparent background, mismatched scale, photorealistic rendering.

**Unity Import:**
| Setting | Value |
|---|---|
| PPU | 64 |
| Sprite Mode | Multiple |
| Slice | Grid by Cell Size 128x128 |
| Filter Mode | Point (no filter) |
| Compression | None |
| Pivot | Bottom |
| Mesh Type | Tight |
| Read/Write | Disabled |

**Notes / Risks:** Choose 128x128 cells because brazier and pillar need more alpha and top-down depth than a strict 64x64 cell. In Unity, visible footprint should still feel compatible with 64x64 floor tiles. Do not bake strong light into prop sprites; use separate 2D Light prefab for actual glow.

Production checklist:
- Check brazier base with flame hidden; it should still read as a prop.
- Check candle cluster at 1x and 2x zoom for alpha noise.
- Reject if props contain floor squares or large background patches.
- Reject if pillar reads as full-height wall pillar; this is only a base.

## A6 - Backwall Modular Set (16-piece, 4x4 sheet)

**Hedef:** Modular north backwall band pieces for normal rooms, larger rooms, corridors, and landmark rooms.

**Boyut:** Recommended v1: 512x512 total, 4x4 cells, 128x128 per cell. Blocked higher-res target: 768x768 or 1024x1024 single sheet was not supported by the checked S-XL Pro docs.

**Workflow:** Create S-XL Image (Pro) prompt-only 4x4 sheet, one generation, transparent background if available. Do not use init image for v1. If the Web UI supports a separate style image slot without dimension lock, optionally use one local ChatGPT reference render as style only.

**PixelLab Prompt (copy-paste ready):**
> 4x4 grid, 16 modular north backwall band pieces, total canvas 512x512, each cell exactly 128x128, transparent background, dark fantasy pixel art, fractured granite, cyan rift cracks, amber torch highlights, top-down ARPG 85-90 degree angle, NO characters, NO UI. Each cell is self-contained but designed to tile horizontally with adjacent backwall pieces; all cells share the same bottom baseline, same top cap height, same charcoal granite palette, same lighting from above, same scale, and matching left/right seam edges. PILLAR-LESS modular wall band, no baked torches, no baked loose props, no characters. Row 1: cell 1 Left End cap closing the wall run, cell 2 Mid Broken A repeatable damaged mid section, cell 3 Mid Broken B repeatable alternate damage pattern, cell 4 Mid Stone Cover cleaner mid section for seam hiding. Row 2: cell 1 Mid Torch Socket with an empty stone socket only, no flame, cell 2 Mid Banner with one hanging rogue-red banner attached to the wall, cell 3 Damaged Mid partly collapsed with a small dark void leak under the stones, cell 4 Open Gap with broken wall gap and low cyan rift glow in the void. Row 3: cell 1 Boss Gate Center with compact closed arch gate landmark fitting one cell, cell 2 Rift Gate Center with cyan rift portal contained inside one-cell wall arch, cell 3 Prison Bars Center with iron bars built flush into the wall and dim interior darkness, cell 4 Library Shelf Center with bookshelf flush against the wall, no loose floor props. Row 4: cell 1 Crypt Statue Center with a stone statue niche built into the wall, cell 2 Inner Corner NW concave room corner, cell 3 Inner Corner NE concave room corner mirrored-compatible, cell 4 Right End cap closing the wall run. Keep all landmark cells the same width as mid cells, with aligned bottom baseline and top cap; left end and right end visually close the run.

**Negative Prompt:**
> full room scene, floor island, characters, enemies, UI, labels, true isometric view, 30 degree projection, giant standalone pillars between every cell, baked torch flames, loose props covering the cells, inconsistent baseline, inconsistent top height, mismatched stone palette, non-grid layout, perspective wall receding into depth, monolithic single wall panorama, blurry seams.

**Unity Import:**
| Setting | Value |
|---|---|
| PPU | 64 |
| Sprite Mode | Multiple |
| Slice | Grid by Cell Size 128x128 |
| Filter Mode | Point (no filter) |
| Compression | None |
| Pivot | Bottom |
| Mesh Type | Tight |
| Read/Write | Disabled |

**Notes / Risks:** This is the most fragile asset. The documented S-XL Pro max forces 128x128 cells, so details like boss gate, bookshelf, and prison bars must be compact. If the user needs high-detail 192x192 or 256x256 cells, split the job into four 4x1 row sheets or generate landmark cells individually, then pack manually. Do not accept a result with pillars between all pieces; the pipeline decision is modular backwall band, pillar-less base, with separate overlays as needed.

Recommended A6 QC steps:
- Slice the 4x4 sheet and place Left End + Mid Broken A + Mid Stone Cover + Right End in Unity.
- Check a five-piece wall run: Left End, Mid Broken A, Mid Torch Socket, Mid Broken B, Right End.
- Check baseline by aligning all pivots to the same Y value.
- Check the top cap height in all 16 cells.
- Check that center landmarks do not require a wider cell than one module.
- Check that Open Gap and Rift Gate glow do not overpower combat telegraphs.

Cell-by-cell content notes:
- 1,1 Left End cap: closed vertical side face at left edge, no tall pillar, broken stones step inward.
- 1,2 Mid Broken A: repeatable mid wall, light damage, small cyan hairline cracks.
- 1,3 Mid Broken B: alternate crack and missing-stone pattern, same seam edge height.
- 1,4 Mid Stone Cover: cleaner mid segment, useful to hide repeated damage.
- 2,1 Mid Torch Socket: empty metal/stone bracket or recess, no flame sprite.
- 2,2 Mid Banner: one dark rogue-red hanging banner, low saturation, attached flush.
- 2,3 Damaged Mid: partial collapse at lower edge, dark void leak, restrained cyan.
- 2,4 Open Gap: one-cell broken wall opening, cyan rift glow inside the void.
- 3,1 Boss Gate Center: closed arch/gate, compact landmark, not a full doorway system.
- 3,2 Rift Gate Center: contained cyan portal, brighter than decor but still background.
- 3,3 Prison Bars Center: vertical iron bars, dim back darkness, no prisoner.
- 3,4 Library Shelf Center: bookshelf flush into wall, no loose readable books on floor.
- 4,1 Crypt Statue Center: statue niche carved into wall, silhouette clear at 128 px.
- 4,2 Inner Corner NW: concave wall turn for north-west room corner.
- 4,3 Inner Corner NE: concave wall turn for north-east room corner.
- 4,4 Right End cap: closed vertical side face at right edge, mirrors left end logic.

Init image decision:
- Do not use init image for A6 v1 because dimension locking can break strict 4x4 sheet output.
- A style image slot can be tested only if it does not force output dimensions.
- If style drift is high, generate a 512x512 backwall sheet without init first, then regenerate only failed rows/cells manually.

## A7 - Doorway Sprites (2 single)

**Hedef:** Two doorway sprites covering N/S and W/E directions through rotation or mirroring.

**Boyut:** A7a vertical doorway 96x128. A7b horizontal doorway 128x96. Transparent background.

**Workflow:** Create S-XL Image (Pro) or Create Image Pro, two single transparent assets.

**PixelLab Prompt - A7a Vertical N/S (copy-paste ready):**
> Single vertical stone archway doorway sprite on transparent background, 96x128, dark fantasy pixel art, fractured granite, cyan rift cracks, amber torch highlights, top-down ARPG 85-90 degree angle, NO characters, NO UI, no wood door, only a dark granite stone arch with black void interior visible, compact fractured stones, optional empty torch socket on one side with no flame, same palette and baseline as RIMA modular backwall sheet, clean alpha, bottom pivot friendly, readable as north/south doorway.

**PixelLab Prompt - A7b Horizontal W/E (copy-paste ready):**
> Single horizontal side doorway stone arch sprite on transparent background, 128x96, dark fantasy pixel art, fractured granite, cyan rift cracks, amber torch highlights, top-down ARPG 85-90 degree angle, NO characters, NO UI, no wood door, dark granite archway adapted for west/east side opening, black void interior visible, compact chipped stones, optional empty torch socket, same palette and baseline as RIMA modular backwall sheet, clean alpha, mirror-compatible for opposite direction.

**Negative Prompt:**
> wood door, closed wooden planks, character, UI, labels, full wall sheet, giant portal, bright VFX, true isometric projection, floor island, heavy baked torch light, large staircase, non-transparent background.

**Unity Import:**
| Setting | Value |
|---|---|
| PPU | 64 |
| Sprite Mode | Single |
| Slice | None |
| Filter Mode | Point (no filter) |
| Compression | None |
| Pivot | Bottom |
| Mesh Type | Tight |
| Read/Write | Disabled |

**Notes / Risks:** Doorways must share A6 stone palette and baseline. A7a should cover N/S by placement or vertical flip only if art still reads correctly; A7b should cover W/E by flipX. No wooden door, because Fractured Chamber transitions are stone arches, voids, or rift portals.

Production checklist:
- Place A7a against A6 Mid Stone Cover and check stone color match.
- Place A7b at east and west room sides with flipX.
- Reject if it looks like a full boss portal rather than a normal doorway.
- Reject if a baked flame appears in the socket.

## A8 - Theme Props (3 single)

**Hedef:** Three room-theme props for crypt, ritual, and prison identity.

**Boyut:** A8a Sarcophagus 128x96. A8b Ritual Stone 96x96. A8c Prison Cage 96x128. Transparent background.

**Workflow:** Create S-XL Image (Pro) or Create Image Pro, three single transparent assets.

**PixelLab Prompt - A8a Sarcophagus (copy-paste ready):**
> Single granite sarcophagus prop sprite on transparent background, 128x96, dark fantasy pixel art, fractured granite, cyan rift cracks, amber torch highlights, top-down ARPG 85-90 degree angle, NO characters, NO UI, heavy rectangular tomb lid, chipped charcoal stone, subtle cracks, optional very faint cyan rift glow in one seam, low-emissive decor only, clean alpha, same scale and palette as RIMA prop sheet, bottom pivot friendly.

**PixelLab Prompt - A8b Ritual Stone (copy-paste ready):**
> Single ritual stone altar prop sprite on transparent background, 96x96, dark fantasy pixel art, fractured granite, cyan rift cracks, amber torch highlights, top-down ARPG 85-90 degree angle, NO characters, NO UI, compact altar-like dark stone slab with angular top face, cyan rune glow carved into the top, medium emissive but below combat telegraph brightness, chipped edges, clean alpha, same scale and palette as RIMA prop sheet.

**PixelLab Prompt - A8c Prison Cage (copy-paste ready):**
> Single vertical prison cage prop sprite on transparent background, 96x128, dark fantasy pixel art, fractured granite, cyan rift cracks, amber torch highlights, top-down ARPG 85-90 degree angle, NO characters, NO UI, iron bars cage with dark interior, weathered black metal, small granite base stones, restrained amber edge highlights, no prisoner, no skeleton, clean alpha, bottom pivot friendly, same scale as RIMA prop sheet and prison backwall cells.

**Negative Prompt:**
> character, prisoner, skeleton, UI, labels, full wall, floor tile, true isometric projection, huge portal, oversized bloom, bright combat telegraph, non-transparent background, photorealistic metal, wood dominance, modern cage.

**Unity Import:**
| Setting | Value |
|---|---|
| PPU | 64 |
| Sprite Mode | Single |
| Slice | None |
| Filter Mode | Point (no filter) |
| Compression | None |
| Pivot | Bottom |
| Mesh Type | Tight |
| Read/Write | Disabled |

**Notes / Risks:** A8 props should be more distinctive than A5 props but not so bright that they read as active VFX. Ritual Stone can be the brightest of the three, but its cyan rune glow must remain below telegraph intensity. Prison Cage must not include an NPC or enemy silhouette.

Production checklist:
- Place sarcophagus in Flooded Crypt and Crypt room mockups.
- Place ritual stone with A4 branching crack underneath.
- Place prison cage near A6 Prison Bars Center and verify material match.
- Reject if any prop includes a character, skeleton, or readable UI-like symbol.

## Unity Import Settings Summary

Default for all assets:

| Setting | Value |
|---|---|
| PPU | 64 |
| Filter Mode | Point (no filter) |
| Compression | None |
| Pivot | Bottom |
| Mesh Type | Tight |
| Read/Write | Disabled |

Sprite Mode by asset:

| Asset | Sprite Mode | Slice |
|---|---|---|
| A1 Workflow A | Multiple | 64x64 grid |
| A1 Workflow B | Multiple | 128x128 grid |
| A2 | Multiple | Manual rectangles |
| A3 | Single | None |
| A4 | Single | None |
| A5 | Multiple | 128x128 grid |
| A6 | Multiple | 128x128 grid |
| A7a | Single | None |
| A7b | Single | None |
| A8a | Single | None |
| A8b | Single | None |
| A8c | Single | None |

Layering guidance:
- Floor tiles are lowest.
- Edge sprites sit above floor but below props.
- Backwall band sits behind props but must use bottom pivot for Y-sort.
- Props sit above floor and edge.
- Rift decals sit above floor and below actors unless a specific scene needs them above props.
- Actual light should come from Unity 2D Lights or particles, not baked sprite glow.

## Uretim Sirasi ve Dependency Tree

```text
Phase 1 (foundation, blocker):
  A1 floor -> A2 edge -> Battered Hall test scene

Phase 2 (parallel):
  A3 + A4 decals (Aseprite, about 10 min total)
  A5 prop sheet
  -> Battered Hall full compose test

Phase 3 (identity):
  A6 backwall 16-piece sheet -> Rift Gate Chamber test
  A7 doorway 2 sprite -> door socket pipeline test

Phase 4 (theme):
  A8 theme props -> Ritual Chamber + 5+ room coverage
```

Operational order:
1. Produce A1 first and reject anything that fails walkable-floor readability.
2. Produce A2 and build a Battered Hall room with only floor and low edges.
3. Hand-paint A3 and A4 while A5 is being generated.
4. Generate A5 and compose Battered Hall with props and separate Unity lights.
5. Generate A6 and immediately test a horizontal wall run before judging landmark cells.
6. Generate A7 only after A6 palette is accepted.
7. Generate A8 last, because theme props should match the accepted A5/A6 style.

## Production QC Matrix

Global accept criteria:
- Asset reads correctly at 1x zoom and 2x zoom.
- Asset stays in the RIMA Shattered Keep palette.
- Cyan rift pixels are intentional accents, not a full blue wash.
- Amber highlights are local and do not bake room lighting into base sprites.
- No characters or enemy silhouettes appear in any asset.
- No UI glyphs, labels, readable text, or instruction overlays appear.
- Top-down ARPG angle is near-pure 85-90 degree, not true isometric.
- Transparent-background assets have clean alpha with no dark rectangular backing.
- Sheet cells keep their requested cell boundaries and do not bleed into neighbors.
- Bottom pivots are visually plausible for Y-sort.

A1 accept criteria:
- Every floor tile reads as flat walkable ground.
- No tile creates a cliff, wall, stair, or raised platform illusion.
- Clean tile can be repeated in a 5x5 block without obvious seams.
- Cracked tile remains low-emissive and does not look like a spell telegraph.
- Rift-glow tile uses thin cyan hairlines, not thick luminous rivers.
- Broken-corner tile can sit at room edge without implying a full wall.
- Transition/corner tiles connect without hard square artifacts.
- Outer-corner joins pass manual checkerboard test.
- Diagonal corner joins do not create missing pixels or broken terrain gaps.
- Tile color matches A2 edge stone closely enough for Battered Hall.

A2 accept criteria:
- Low edge reads lower than A6 backwall.
- North edge cell does not hide too much floor space.
- Side edge cell remains mirror-compatible.
- Corner cell covers one corner without becoming a pillar.
- Rubble cover cell hides seams without reading as a major prop.
- All four cells share one stone palette.
- All four cells share one light direction.
- Transparent padding is sufficient for manual slicing.
- No cell contains baked flame or strong glow.
- No cell contains a full backwall.

A3 accept criteria:
- Transparent background is clean.
- Main crack fits comfortably inside 128x64.
- Core is mostly 1 px with occasional 2 px fragments.
- Glow halo is sparse and broken.
- Emissive value is medium, below telegraph VFX.
- Crack line is jagged, not smooth lightning.
- Works on both clean and cracked A1 floor tiles.
- Does not form a magic circle.
- Can be rotated slightly without looking wrong.
- No floor texture is baked into the decal.

A4 accept criteria:
- Transparent background is clean.
- Branching web fits inside 256x128.
- Branch count is 3-5, not a dense scribble.
- Core intersections are the brightest points.
- Outer halo is sparse enough for combat readability.
- Shape can serve as a room focal decal.
- Does not look like active AoE telegraph.
- Does not include portal center or rune circle.
- Can layer under A8b Ritual Stone.
- Does not overpower A6 Rift Gate Center.

A5 accept criteria:
- All four props fit in their 128x128 cells.
- Visible footprints remain compatible with 64x64 floor scale.
- Brazier has base/ember only, no baked flame animation.
- Pillar base is low and broken, not full-height architecture.
- Rubble pile is readable but not too tall.
- Candle cluster has tiny amber points only.
- Prop silhouettes are clear at 1x.
- No prop contains a floor tile background.
- Props share palette and light direction.
- Alpha padding supports individual sprite extraction.

A6 accept criteria:
- 4x4 grid is readable and sliceable.
- All 16 cells share the same bottom baseline.
- All 16 cells share the same top cap height.
- Left and right seams align when cells are placed horizontally.
- Mid cells can repeat without visible scale jumps.
- End caps close the wall run.
- Landmark cells fit in one module width.
- No unwanted full-height pillars are baked into base cells.
- Torch Socket has no flame.
- Open Gap and Rift Gate glow stay behind gameplay readability.
- Prison Bars and Library Shelf read as flush wall features.
- Inner corners support concave room corners.
- Right End cap mirrors the logic of Left End cap.
- Wall band remains north-backwall, not a full-room diorama.
- Transparent alpha is clean if generated with no background.

A7 accept criteria:
- A7a vertical doorway fits 96x128.
- A7b horizontal doorway fits 128x96.
- Both share A6 stone palette.
- Both share A6 baseline logic.
- Neither includes a wood door.
- Void interior is dark and readable.
- Optional torch socket is empty.
- Horizontal door mirrors cleanly with flipX.
- Doorway reads as connection socket, not boss portal.
- Alpha is clean around arch silhouette.

A8 accept criteria:
- Sarcophagus fits 128x96 and reads as granite tomb.
- Sarcophagus glow, if present, is faint.
- Ritual Stone fits 96x96 and reads as altar-like.
- Ritual rune glow is medium, not telegraph-high.
- Prison Cage fits 96x128 and reads as iron bars.
- Prison Cage contains no prisoner, skeleton, or enemy.
- All three props share A5 prop scale.
- All three props share A6/A5 material palette.
- All three props have clean transparent background.
- Theme identity is visible without cluttering walkable space.

## Manual Slice Plan

A1 Workflow A:
- Import as Multiple.
- Slice grid 64x64.
- Name tiles `floor_clean`, `floor_cracked`, `floor_rift`, `floor_broken_corner`, then transition names after visual inspection.
- Build a small 8x8 test tilemap with random clean/cracked/rift ratios.

A1 Workflow B:
- Import as Multiple.
- Slice grid 128x128.
- Export or downscale only if orchestrator approves external resizing.
- Name cells `floor_clean_large`, `floor_cracked_large`, `floor_rift_large`, `floor_broken_corner_large`.

A2:
- Use manual sprite editor rectangles.
- Cell 1 rectangle 128x64 inside top-left quadrant.
- Cell 2 rectangle 64x128 inside top-right quadrant.
- Cell 3 rectangle 64x64 inside bottom-left quadrant.
- Cell 4 rectangle 64x64 inside bottom-right quadrant.
- Name sprites `edge_n_low`, `edge_side_low`, `edge_outer_corner`, `edge_rubble_cover`.

A5:
- Slice grid 128x128.
- Name sprites `prop_brazier_base`, `prop_broken_pillar_base`, `prop_rubble_pile`, `prop_candle_cluster`.
- Attach actual lights in prefabs, not in import settings.

A6:
- Slice grid 128x128.
- Name sprites row-major: `backwall_left_end`, `backwall_mid_broken_a`, `backwall_mid_broken_b`, `backwall_mid_cover`, `backwall_torch_socket`, `backwall_banner`, `backwall_damaged_mid`, `backwall_open_gap`, `backwall_boss_gate`, `backwall_rift_gate`, `backwall_prison_bars`, `backwall_library_shelf`, `backwall_crypt_statue`, `backwall_inner_corner_nw`, `backwall_inner_corner_ne`, `backwall_right_end`.
- Build a prefab palette only after the horizontal seam test passes.

A7:
- Import each as Single.
- Name vertical `doorway_vertical_ns`.
- Name horizontal `doorway_horizontal_we`.
- Use flipX variants in prefab setup instead of generating duplicate art.

A8:
- Import each as Single.
- Name `theme_sarcophagus`, `theme_ritual_stone`, `theme_prison_cage`.
- Keep each prop in the same sorting layer family as A5 props.

## Open Questions

1. Should A6 v1 accept the documented 512x512 sheet limit with 128x128 cells, or should production split A6 into four row sheets to preserve 192x192/256x256 source-cell detail?
2. Should A1 final import use true 64x64 Create Tiles Pro output, or should larger 128x128 fallback cells be downscaled externally before Unity import?
3. Should A6 landmark cells with strong cyan glow, especially Rift Gate Center and Open Gap, be authored as low-emissive base art plus separate VFX overlays?
4. Should A5 brazier/candle flame assets be created now as separate animated sprites, or deferred to Unity ParticleSystem/2D Light prefabs?
5. Should A7 vertical doorway include an empty torch socket by default, or stay pure stone arch for cleaner reuse?
