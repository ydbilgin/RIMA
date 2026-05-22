# Codex Review - Asset Combination Pipeline v1

## 1. Read + comprehend

Reviewed in full: `STAGING/asset_combination_pipeline_v1.md`, `STAGING/weapon_pipeline_v1.md`, `STAGING/map_schema_v1.json`, `STAGING/act1_shattered_keep_layout_v1.json`, `CURRENT_STATUS.md`. The requested `memory/reference_pixellab_create_tiles_pro_4type.md` is missing from this checkout; substitute source used: `PixelLabDocs/create-tiles-pro.md` plus `PixelLabDocs/mcp_docs.md`. PixelLab balance read-only check: 2265 / 5000 generations.

NLM canon query adds important constraints: floor batches should use pure top-down square tiles, no extrusion, preferably segmentation/no outline artifacts; walls are a separate overlay class, not floor tiles; missing canonical classes include gate sockets, dash-only gaps, hazards, interactives, and some lore objects.

## 2. Batch organization assessment

| Batch | Verdict | Reasoning |
|---|---|---|
| Floor 1 Granite | TWEAK | 64x64 opaque is correct for 64 PPU. Variant count is right. Add explicit `tile_type:square_topdown`, `tile_view:top-down`, `outline_mode:segmentation`; avoid "flagstone/slab/mortar" if canon prompt lock is strict. |
| Floor 2 Rubble | PASS | Good role, size, opaque BG, and 4x4 cost. Keep `tile_view_angle:90` + `tile_depth_ratio:0`; prompt is less grid-prone than granite. |
| Floor 3 Walkway | TWEAK | Needed by schema and rooms, but "rectangular flagstone grid" risks visible repetition. Make it worn path surface, not formal grid pattern. |
| Floor 4 Rift | PASS | High-impact Act 1 identity tile. Correct opaque 64x64 floor material. |
| Wall 1 Straight | SPLIT | 64x64 is too small for perimeter caps at screenshot scale, and mixing directions plus variants in one sheet risks unusable frames. Split horizontal/vertical, or at least lock direction labels. |
| Wall 2 Corner | TWEAK | Needed, but corners need exact NE/NW/SE/SW geometry. 64x64 may be cramped; 128x128 safer for readable wall mass. |
| Wall 3 Archway | PASS | Correct hero/gate-adjacent batch. 64x96 is usable, though gate sockets may need more variants later. |
| Prop 1 Column | PASS | Directly matches current layout `column_hero`/`column_broken`. 64x128 transparent is appropriate. |
| Prop 2 Banner | PASS | Used heavily in current rooms; 32x64 transparent is efficient. Add chain/hanging variants if keeping canon L6 accent. |
| Prop 3 Candle/Torch | TWEAK | Current layout references `torch_wall`, not just floor candles. Split wall torch/sconce from candle if import prefab IDs must match. |
| Prop 4 Urn/Vase | PASS | Good secondary prop. No current six-room dependency, but supports mockups. |
| Prop 5 Skull/Bone | PASS | Current boss room uses `skull_pile`; 32x32 is fine. |
| Prop 6 Chest | TWEAK | Reward schema has chest tiers common/rare/epic/legendary, while prompt says common/uncommon/rare/epic. Align names and include locked state if key gates matter. |
| Prop 7 Brazier | PASS | Good lighting anchor; transparent 32x64 works. |
| Prop 8 Sarcophagus | PASS | Not in current six rooms, but important for crypt/throne concepts. |
| Decal 1 Moss | TWEAK | Good category, but 6 frames may overproduce. Four strong alpha-clean variants may be enough. |
| Decal 2 Crack | PASS | Critical for floor polish and rift pressure. 32x32 transparent is right. |
| Decal 3 Rift Glyph | PASS | Correct Act 1 identity layer. |
| Decal 4 Liquid | MERGE | Batch is valid, but blood/water/ichor have different narrative meanings and palette rules. Keep merged only for cost; split if QC matters. |
| Decal 5 Detritus | MERGE | Efficient but semantically crowded. Good for first pass; later split footprint/scorch if readability fails. |
| Decal 6 Summoning Circle | PASS | Correct 64x64 hero decal with low count. |

Redundancy: no hard redundant batches, but Liquid and Detritus are cost merges. Biggest structural issue is walls: current plan treats each frame as a direction/variant, but wall geometry needs deterministic semantic placement.

## 3. Missing assets / categories check

- Current layout uses `rubble_pile_small` and `rubble_pile_large`; pipeline only has rubble floor, not rubble pile props. Add prop batch: 32x32/64x64 transparent, 6 variants, "loose collapsed stone pile, small and large readable silhouettes, no floor-tile square edge".
- Current layout uses `shrine_pedestal`; pipeline has no shrine/altar/pedestal prop. Add hero prop: 64x64 or 64x96 transparent, 4 variants, "rift shrine pedestal, worn stone altar, cyan hairline core, no UI icon".
- Schema has `rewards.pickups`; no pickup/key/loot shard asset class. Add micro prop batch: 32x32 transparent, 4-8 variants, "small pickup tokens, key shard, relic shard, potion-like pickup, readable top-down".
- Schema has `doors` and NLM canon requires in-world gate sockets. Archway alone is not enough. Add gate socket batch: 64x96/128x128 transparent or opaque by wall integration, 6 variants, "stone arch, breach, chained doorway, rift threshold, locked seal, bridge mouth".
- NLM canon flags hazards and dash-only spaces. Add hazard/decal batch: 64x64 transparent, 4-6 variants, "rift tear core, rift bloom crack telegraph, dash gap seam, hard navy outline, cyan core".
- Mockups call out forge/library/apothecary but no anvil/forge, shelves/books/scrolls, table/workbench, chains, broken statue, or lore note/relic shelf. These can wait after vertical slice, but they are needed before the 10-room set is believable.

Weapon pipeline is orthogonal and does not overlap, except Elementalist orb/weapon sprites should remain in `Assets/Art/Weapons`, not environmental prop batches.

## 4. Priority recommendation

If only 1 batch: Prop 1 Column. The Phase K screenshots show floor repetition, but the six-room layout already places many columns and broken columns; one batch makes rooms read as authored spaces instead of a tiled plane.

If 5 batches: Column, Banner, Candle/Torch, Rubble Pile add-on, Rift/Crack decal. This targets actual prefab IDs in the current six rooms and adds immediate scale cues, wall dressing, light anchors, collapse story, and surface polish.

If 10 batches: add Granite, Walkway, Rift floor, Archway/Gate, Chest, Skull/Bone. This covers the current materials, entry/throne gate language, reward room, and boss room death dressing. Do wall straight/corner after a wall geometry decision, not before.

## 5. MCP vs web UI recommendation

| Type | Recommendation | Reasoning |
|---|---|---|
| Tile | EITHER | MCP exposes `tile_type`, `tile_size`, `tile_view`, `tile_view_angle`, `tile_depth_ratio`, `style_images`, and `outline_mode`. Web UI is safer for visual checking. |
| Wall | WEB | Current MCP object tool available here does not expose the old `n_frames` interface from the plan; wall semantic slicing and direction labeling need manual QC. Prior timeout history lowers confidence. |
| Prop | WEB | Transparent BG and canvas control exist in object-style tools, but web UI gives faster candidate rejection and avoids autonomous timeout risk. |
| Decal | WEB | Alpha-clean transparent decals are QC-heavy. MCP can be fallback for small batches, but web UI is the default tonight. |

MCP feasibility verdict: medium for tiles, low-medium for object batches. Default to web UI; use MCP for read-only status and small retries.

## 6. Budget reality check

`create_tiles_pro` 64x64 is documented at about 25 generations per batch, so 4 floor batches cost about 100. Current MCP object tools appear closer to fixed per-call/candidate costs (roughly 20-40 by size), not "n_frames = exact cost"; the older `create_object n_frames` interface in local MCP docs is not exposed as-is in this session. For 17 object/decal/wall calls, realistic range is 340-680. Revised full pipeline range: about 440-780 generations. The stated 625 is plausible, not dangerously low. Spare after full run: about 1485-1825 generations for re-gens, mobs, character states, and Act 2-4 prep.

## 7. Risks + open questions

1. Wall geometry is under-specified. Decide pure flat 90-degree wall vs Hades-style front-face depth before any wall generation. Mitigation: one wall prototype, then lock.
2. Prefab ID mismatch is real: `torch_wall`, `rubble_pile_*`, `shrine_pedestal`, and `column_hero` are not cleanly named in the batch list. Mitigation: write asset catalog IDs before production.
3. Transparent alpha may fringe on decals/props. Mitigation: require alpha check and Unity import Alpha Is Transparency.
4. Style consistency across 21 batches can drift. Mitigation: pick one approved floor/column output as style anchor for later web UI or MCP reference where supported.
5. Missing gates/hazards/interactives will block richer room function. Mitigation: add gate socket and hazard batches before claiming "Act 1 complete".
6. Asset folders should be locked before import: `Assets/Art/Tiles/Act1_ShatteredKeep`, `Assets/Art/Props/Act1_ShatteredKeep`, `Assets/Art/Decals/Act1_ShatteredKeep`, with registries under `Assets/Data/...`.

## 8. Final recommendation

Verdict: ready after tweaks, not ready as written. Keep the floor/prop/decal plan, but fix wall batching, add missing current-prefab batches, and align chest/gate/schema terminology before generation. MCP feasibility is medium for tiles and low for wall/prop/decal production tonight; web UI should be default, MCP only fallback. Start with columns if one batch, or the 5-batch current-layout set above. Web UI time for the revised vertical-slice-first set is about 60-90 minutes; full 21+added batches remain about 3-4 hours.
