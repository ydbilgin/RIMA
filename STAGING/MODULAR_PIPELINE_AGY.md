# MODULAR PIXEL-ART PIPELINE: VISUAL DECOMPOSITION
**Target:** RIMA 2D Isometric Roguelite
**Source:** Concept paintings 01-09

## 1. DECOMPOSITION
To reproduce the painterly concepts in engine, we must break the baked single images into the following modular layers:

*   **GEOMETRY (Tileable)**
    *   **Base Floor:** Flat isometric granite stone tiles (some clean, some broken).
    *   **Edges:** Isometric floating cliff edges (straight lines, inner corners, outer corners).
    *   **Undersides:** Hanging rocky undersides for the floating islands.
    *   **Walls:** Broken stone wall segments and corner pieces.
    *   **Specialized Floors:** Circular arena segments and chained corner nodes.
*   **DECORATION (Props)**
    *   **Rubble:** Small floating debris, broken rocks, loose stones.
    *   **Runestones:** Floating rocks with engraved runes.
    *   **Interactables:** Chests, stone portals, chains.
*   **ENERGY (Cyan VFX)**
    *   **Decals:** Glowing cyan floor cracks and rune circles.
    *   **VFX:** Portal swirl energy, combat shockwaves, boss flame auras.
*   **ACTORS**
    *   Hero character.
    *   Armored Knight Boss.
    *   Cyan Flame Demon Boss.

## 2. TOOL MAPPING
*   **Base Floor Tiles:** `create_tiles_pro` (numbered prompt, `tile_type=isometric`). Since Wang16 is top-down oriented in the KB, isometric tiles are best batched here.
*   **Walls, Edges & Undersides:** `create_1_direction_object` (size ≤85 for 16 candidates/call). These are grid-aligned props, so batch generation is the most cost-efficient.
*   **Props & Rubble (Chests, Chains, Debris):** `create_1_direction_object` (size ≤42 for 64 candidates/call). The ultimate batch efficiency for small set dressing.
*   **Energy / Cyan VFX:** `create_1_direction_object` with strong cyan prompting and transparent backgrounds.
*   **Actors:** `create_character` (Standard or Pro) for the 8-directional base sprites, followed by `animate_with_text v3` (first_frame=ON) for the animations.

## 3. PROMPT CRAFT
**Base RIMA Style Lock:** `dark gritty fantasy pixel art, matte hand-pixeled clusters, hard pixel edges, no anti-aliasing, charcoal slate (#2C2A2A-#4E5260), cold blue-grey shadows, sparse cyan-violet rift accent, high top-down ARPG ~70-80°, transparent background`
**Negative Prompt:** `no text, no labels, no numbers, no watermarks, no logo, no UI, no frame, no blurry edges, no anti-aliasing, no smooth vector gradients`

*   **Floor Tile:** `[ASSET TYPE], [SUBJECT], [CAMERA VIEW], [STYLE], [PALETTE/MATERIALS], [SILHOUETTE], transparent background, crisp pixel art, no anti-aliasing`
    -> `isometric floor tile, cracked granite stone slab, isometric view, dark gritty fantasy pixel art, matte hand-pixeled clusters, hard pixel edges, no anti-aliasing, charcoal slate (#2C2A2A-#4E5260), cold blue-grey shadows, high top-down ARPG ~70-80°, transparent background` *(Note: removed cyan rift accent to enforce cyan discipline, see below).*
*   **Cliff/Island-Edge Tile:** `isometric floating island edge, broken cliff face with hanging rock underneath, isometric view, dark gritty fantasy pixel art, matte hand-pixeled clusters, hard pixel edges, no anti-aliasing, charcoal slate (#2C2A2A-#4E5260), cold blue-grey shadows, high top-down ARPG ~70-80°, transparent background`
*   **Rubble Prop:** `small floating rock rubble piece, broken stone debris, isometric view, dark gritty fantasy pixel art, matte hand-pixeled clusters, hard pixel edges, no anti-aliasing, charcoal slate (#2C2A2A-#4E5260), cold blue-grey shadows, high top-down ARPG ~70-80°, transparent background`

## 4. STYLE-REFERENCE STRATEGY
Do not feed the entire concept painting as an init image, as the AI will try to recreate the whole room.
*   **Cropping:** Crop specific 256x256 sub-regions from the concept art (e.g., just a patch of floor from `concept01`, or just the chest from `concept05`) and use these as `style_images`.
*   **Init-Strength 200-300 (Color/Vibe):** Use low strength when generating entirely new props (like chains) to just inherit the charcoal/cyan palette.
*   **Init-Strength 400-600 (Variations):** Use this when feeding a cropped floor tile to generate 10 new variant tiles that perfectly match the original concept's texture.
*   **Init-Strength 600-800 (Lock-in):** Use only when you have a nearly perfect piece and just need PixelLab to fix minor seams or pixel clusters.

## 5. CYAN DISCIPLINE
To prevent the procedural rooms from becoming overwhelmingly bright, **do not bake the cyan glow into the base floor tiles.**
*   **Base Layer:** Generate all granite tiles, walls, and cliffs completely devoid of cyan energy.
*   **Overlay Layer:** Generate glowing cyan cracks, runes, and energy leaks as separate, transparent decals/props using `create_1_direction_object`.
*   **Implementation:** In Unity, lay down the pure granite floor, then procedurally spawn the cyan decals on top of a strict 5-8% subset of the tiles. This allows the cyan to act as an emissive light source, pop visually, and respect the budget while allowing for dynamic combat VFX to stand out.

## 6. VARIANT COUNTS
Balanced for non-repetitive procedural generation vs. PixelLab gen cost:
*   **Floor Tiles:** 12-16 variants (1 call of `create_tiles_pro` ~20-25 gen).
*   **Walls & Cliff Edges:** 4 straight walls, 4 straight edges, 4 inner corners, 4 outer corners (1 call `create_1_direction_object` size=85 ~20-40 gen).
*   **Rubble & Debris:** 32-64 small variants (1 call `create_1_direction_object` size=32 ~20-40 gen).
*   **Cyan Decals:** 8-12 variants (cracks, runes).
*   **Props (Chests/Portals):** 1-2 variants each.
*   **Total Initial Env Cost:** ~100-150 gen to produce a massive, fully modular, procedural-ready tileset.
