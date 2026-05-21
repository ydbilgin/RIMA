# Master Sheet Review Verdict - Act 1 Shattered Keep Walls

## 1. Executive Summary

- Verdict: REVISE before final shipping. For immediate production slicing, use Codex imagegen v1 because it is real RGBA transparency at 512x512 and produced sliceable separated objects. For art direction, use ChatGPT v1 as the visual quality reference, but it is not directly production-ready because the checkerboard is baked into the pixels and alpha is 100% opaque.
- Production decision: PASS WITH CONDITIONS for Codex as a temporary/importable v1 wall modular pack. FAIL for ChatGPT as a direct source sheet until it is regenerated or cleaned with true transparent background.
- Phase 2 should regenerate the pack using the ChatGPT detail level while preserving strict alpha transparency, exact canvas contract, and clearer semantic separation for inner/outer corners and wall junctions.

## 2. Quality Comparison

| Aspect | Codex imagegen | ChatGPT | Winner |
|---|---|---|---|
| Image size | 512x512 RGBA, expected 512x512 | 1254x1254 RGBA, not expected 1024x1024 | Codex for contract |
| Transparent background | 66.58% transparent, alpha bbox (10, 7, 502, 480) | 0.0% transparent; checkerboard is baked into opaque pixels | Codex |
| Tile distinctness | 65 kept alpha components; objects are separated but some B wall semantics are ambiguous | 1 kept alpha component because full canvas is opaque; visual objects are readable but not alpha-detectable | Codex for slicing, ChatGPT for human readability |
| Cyan rift palette | 5.09% cyan-like opaque pixels; cyan reads clearly on portal and overlay row | 1.34% cyan-like opaque pixels; richer portal but less total cyan ratio due huge white/checker background | Codex by metric, ChatGPT by visual portal richness |
| Pixel art density | 29661 opaque RGB colors; sharp edge ratio 0.247, smoothish ratio 0.5 | 129433 opaque RGB colors; sharp edge ratio 0.203, smoothish ratio 0.459 | Codex for pixel discipline, ChatGPT for rendered polish |
| Dominant colors | rgb(16, 16, 16) 14.87%, rgb(0, 0, 0) 12.31%, rgb(32, 32, 32) 11.36%, rgb(16, 16, 32) 5.8%, rgb(48, 48, 48) 5.7% | rgb(240, 240, 240) 60.17%, rgb(224, 224, 224) 5.48%, rgb(0, 0, 0) 4.74%, rgb(16, 16, 16) 4.46%, rgb(32, 32, 32) 3.21% | Codex for avoiding baked white background |
| RIMA atmosphere fit | Cold granite, cyan rift, chains, banners, candles, moss/debris all present; smaller and slightly noisy | Stronger Shattered Keep read, better masonry mass, better rift portal, better decor silhouettes; blocked by alpha failure | ChatGPT art direction, Codex implementation |

NLM palette check used: Act 1 should read as cold granite (#3A3D42 to #4E5260), cool shadow (#252830), cyan rift accent (#00FFCC), ice-blue cracks, restrained moss, chains, banners, candles, and a broken ancient-order ruin. Both sheets match the identity, but ChatGPT matches the monumental ruin tone better.

## 3. Per-Section Visual Inspection

### Codex imagegen v1

| Section | Visible / identifiable | Missing / unclear |
|---|---|---|
| A - Features | archway_full, big_corner, big_column, wall_tall_hero are all visible. Archway has cyan portal and wall_tall_hero has cyan crack detail. | archway_full should become empty arch + separate animated rift in final setup; current baked portal is useful as preview/reference only. |
| B - Modular Walls | Straight walls, L corners, short walls, foundations, floor-edge blocks, and junction-like shapes are visible. | The 16 requested semantics are not perfectly distinct. inner vs outer corner variants and T_junction_a/b need clearer silhouettes; there appears to be an extra/ambiguous small wall piece in the first B row. |
| C - Rift Overlays | All 16 cyan overlay sprites are visible as cracks, bursts, scars, rings, drops, spiral, zigzag, pulse/cross forms. | drop_a/drop_b and pulse_a/pulse_b need stronger difference at gameplay scale. Some effects are very thin at 512 source size. |
| D - Decoration | Moss variants, candles/torches, banners, chains, stones, dust, skull, and gem are visible. | candle_a vs candle_b and torch_unlit vs torch_lit need stricter functional separation; final two cells are tight, so skull_floor/gem_pickup need manual crop review. |

### ChatGPT v1

| Section | Visible / identifiable | Missing / unclear |
|---|---|---|
| A - Features | All 4 hero tiles are very readable, with the best arch portal, masonry weight, column, and wall mass. | Not alpha-ready. The checker background is part of the pixels, so every crop will carry white/gray squares unless cleaned. |
| B - Modular Walls | Most modular wall variants read better than Codex: clearer straight walls, corners, junctions, foundations, and floor platforms. | Still needs exact slicing grid validation after transparent regeneration because the canvas is non-standard 1254x1254. |
| C - Rift Overlays | All 16 rift overlays are highly readable and visually strong. | Effects are painterly/smooth; may need palette reduction and sharper pixel cleanup for final pixel-art import. |
| D - Decoration | Best decoration silhouettes: moss, flames, banners, chains, rubble, dust, skull, crystal. | Moss is bright/large and may violate the no generic bright green rule unless recolored toward cold gray-green. |

## 4. Slicing Plan

Winner sheet for actual slicing: Codex imagegen v1.
Reason: it is the only sheet with true transparent background and correct 512x512 production contract.

Source: `STAGING/_pixellab_outputs/walls/v2/act1_wall_modular_pack_codex_v1.png`
Slice output directory created: `Assets/Art/AssetPacks/Act1_ShatteredKeep/wall_modular_v1/`
Generated files: 52 PNG files named `tile_<name>.png`.
Preview artifact: `STAGING/_research/master_sheet_review_artifacts/codex_slice_grid_preview.png`

Recommended section Y ranges under 512x512:

| Section | Y range | Notes |
|---|---:|---|
| A - Features | 0-190 | 4 large feature tiles, variable width. |
| B - Modular Walls | 190-350 | 16 medium modular crops, two rows. Some names need Phase 2 semantic cleanup. |
| C - Rift Overlays | 345-405 | 16 small 32px-wide overlay crops. |
| D - Decoration | 405-490 | 16 small decoration crops, manually widened for larger props. |

| Section | Output file | Crop coordinates |
|---|---|---|
| A | tile_archway_full.png | (0, 0) - (155, 190) |
| A | tile_big_corner.png | (148, 0) - (300, 190) |
| A | tile_big_column.png | (292, 0) - (380, 190) |
| A | tile_wall_tall_hero.png | (378, 0) - (512, 190) |
| B | tile_straight_NE.png | (0, 190) - (70, 276) |
| B | tile_straight_NW.png | (70, 190) - (135, 276) |
| B | tile_corner_outer_a.png | (135, 190) - (180, 276) |
| B | tile_corner_outer_b.png | (180, 190) - (225, 276) |
| B | tile_corner_inner_a.png | (225, 190) - (275, 276) |
| B | tile_corner_inner_b.png | (275, 190) - (320, 276) |
| B | tile_T_junction_a.png | (320, 190) - (435, 276) |
| B | tile_T_junction_b.png | (435, 190) - (512, 276) |
| B | tile_endcap_a.png | (0, 276) - (55, 350) |
| B | tile_endcap_b.png | (55, 276) - (95, 350) |
| B | tile_low_wall_str.png | (95, 276) - (150, 350) |
| B | tile_low_wall_corner.png | (150, 276) - (210, 350) |
| B | tile_low_wall_end.png | (210, 276) - (270, 350) |
| B | tile_foundation_a.png | (270, 276) - (370, 350) |
| B | tile_foundation_b.png | (370, 276) - (440, 350) |
| B | tile_floor_edge.png | (440, 276) - (512, 350) |
| C | tile_crack_h.png | (0, 345) - (32, 405) |
| C | tile_crack_v.png | (32, 345) - (64, 405) |
| C | tile_burst_s.png | (64, 345) - (96, 405) |
| C | tile_burst_l.png | (96, 345) - (128, 405) |
| C | tile_scar_a.png | (128, 345) - (160, 405) |
| C | tile_scar_b.png | (160, 345) - (192, 405) |
| C | tile_glow_a.png | (192, 345) - (224, 405) |
| C | tile_glow_b.png | (224, 345) - (256, 405) |
| C | tile_drop_a.png | (256, 345) - (288, 405) |
| C | tile_drop_b.png | (288, 345) - (320, 405) |
| C | tile_spiral.png | (320, 345) - (352, 405) |
| C | tile_zigzag.png | (352, 345) - (384, 405) |
| C | tile_pulse_a.png | (384, 345) - (416, 405) |
| C | tile_pulse_b.png | (416, 345) - (448, 405) |
| C | tile_burst_h.png | (448, 345) - (480, 405) |
| C | tile_burst_v.png | (480, 345) - (512, 405) |
| D | tile_moss_a.png | (0, 405) - (39, 490) |
| D | tile_moss_b.png | (39, 405) - (77, 490) |
| D | tile_moss_c.png | (77, 405) - (116, 490) |
| D | tile_moss_d.png | (116, 405) - (154, 490) |
| D | tile_candle_a.png | (154, 405) - (185, 490) |
| D | tile_candle_b.png | (185, 405) - (216, 490) |
| D | tile_torch_unlit.png | (216, 405) - (244, 490) |
| D | tile_torch_lit.png | (244, 405) - (276, 490) |
| D | tile_banner_a.png | (276, 405) - (322, 490) |
| D | tile_banner_b.png | (322, 405) - (365, 490) |
| D | tile_chain_short.png | (365, 405) - (388, 490) |
| D | tile_chain_long.png | (388, 405) - (416, 490) |
| D | tile_scatter_stone.png | (416, 405) - (449, 490) |
| D | tile_dust_pile.png | (449, 405) - (476, 490) |
| D | tile_skull_floor.png | (476, 405) - (496, 490) |
| D | tile_gem_pickup.png | (496, 405) - (512, 490) |

## 5. Missing Tiles + Phase 2 List

Production status by sheet:

| Sheet | Verdict | Reason |
|---|---|---|
| Codex imagegen v1 | PASS WITH CONDITIONS | Real alpha, correct 512 canvas, sliceable 52-file output generated. Needs semantic cleanup for B row and some small-tile polish. |
| ChatGPT v1 | FAIL DIRECT IMPORT / REVISE | Best visual quality, but no real transparency and wrong canvas contract. Use as visual target, not as direct source. |

Phase 2 production list:

- Regenerate ChatGPT-quality sheet with true transparent background and exact 512x512 or deliberate higher-res contract.
- Produce archway_full as two assets: empty stone archway and separate animated rift portal overlay.
- Rebuild Section B with explicit labels or strict grid guidance so straight_NE, straight_NW, outer corners, inner corners, T junctions, endcaps, low walls, foundations, and floor_edge are unambiguous.
- Reduce/clean palette for rift overlays after final selection; target sharper cyan pixels and fewer smooth gradients.
- Recolor moss toward muted cold gray-green; avoid bright/generic green.
- Add second pass for skull_floor and gem_pickup with more spacing or larger per-cell budget.

## 6. Unity Import + RuleTile Recommendations

### Import Settings

- Texture Type: Sprite (2D and UI)
- Sprite Mode: Single for each generated `tile_<name>.png`; Multiple only if using the original master sheet.
- Pixels Per Unit: 64 for the generated individual PNGs. This aligns with the current RIMA note that character canvas uses PPU 64 and walls should remain readable against 64x32 floor tiles / 64x96 wall tiles.
- Filter Mode: Point (no filter)
- Compression: None
- Alpha Is Transparency: enabled
- Mesh Type: Full Rect for modular wall sprites unless overdraw becomes a measurable issue.
- Pivot: bottom center for wall modules and features; center for rift overlays; bottom center for decorations that sit on floor/wall.

### RuleTile Naming Convention

Use stable filenames and tile asset names:

- `tile_wall_straight_NE`
- `tile_wall_straight_NW`
- `tile_wall_corner_outer_a`
- `tile_wall_corner_outer_b`
- `tile_wall_corner_inner_a`
- `tile_wall_corner_inner_b`
- `tile_wall_T_junction_a`
- `tile_wall_T_junction_b`
- `tile_wall_endcap_a`
- `tile_wall_endcap_b`
- `tile_wall_low_str`
- `tile_wall_low_corner`
- `tile_wall_low_end`
- `tile_wall_foundation_a`
- `tile_wall_foundation_b`
- `tile_wall_floor_edge`

Current generated files use the shorter requested `tile_<name>.png` pattern. Rename to the longer `tile_wall_*` pattern only when creating Unity Tile assets, so source slices stay directly traceable to the master sheet task.

### RuleTile Setup

- Create separate Tile assets for high wall, low wall, foundation, floor-edge, feature, overlay, and decor categories.
- Use RuleTile only for the modular wall/foundation family. Do not RuleTile candles, banners, skulls, gem, or rift overlays.
- Keep rift overlays on a separate Tilemap layer above wall/floor art with additive/emissive material if available.
- Use archway_empty as the collision/wall tile; place animated rift portal as a separate non-colliding overlay object or animated tile.

### PlayableRoom_v2 Test Paint Plan

- Paint one readable threshold wall using straight_NE/NW plus outer corners.
- Paint one containment alcove with big_column, chains, banner, candle/torch, and cyan crack overlay.
- Paint one archway using the current baked arch as placeholder, then replace it with empty arch + animated rift once Phase 2 exists.
- Validate collision separately from visuals: low wall/foundation/floor_edge should not inherit full tall-wall blocking by accident.
- Check at gameplay zoom for: wall silhouette, cyan readability, moss color temperature, and whether overlay sprites shimmer too smoothly against pixel art.

## Final Recommendation

Use Codex imagegen v1 as the temporary production-import source now. Treat ChatGPT v1 as the target quality bar for Phase 2 regeneration. The pack is not final-production locked until the ChatGPT-level visual detail is regenerated with real alpha and the Section B modular semantics are made exact.
