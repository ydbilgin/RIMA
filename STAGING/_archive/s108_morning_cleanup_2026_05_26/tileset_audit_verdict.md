## Tileset Audit Verdict - 2026-05-21

### Inventory Summary
- Total audited: 36 (11 local + 25 cloud)
- KEEP: 18 rows including duplicate local/cloud records. Unique Act 1 KEEP IDs: 04633962, 49913501, 9ffbb4d1, 9591f35a, ecfee0a0, 02a5a97b, 8c154e37, bdca2623, d43914a8, b41919aa, 4c962284 with caution.
- MAYBE: 12. Useful later or with palette correction: granite variants, slate/moss, pink/cream, mauve/hex, fractured_keep 25-tile.
- DELETE/IRRELEVANT: 6. Main reasons: grass/meadow palette, 16 px mismatch, too generic for RIMA Act 1.

### Strategy Chosen
- Selected: C - Hybrid.
- Rationale: Strategy A gives immediate coverage but different tilesets still form sharp seams where separate Wang sheets meet. Strategy B is the cleanest final answer but costs roughly 280 generations for all acts. Strategy C gives a playable Act 1 baseline now using the existing Keep chain, while reserving strategic re-generation for the seams that matter most.

| Strategy | Cost | Strength | Risk | Verdict |
|---|---:|---|---|---|
| A - as-is library | 0 new gens | Fastest, broad existing coverage | Cross-sheet seams remain visible | Good for prototype only |
| B - base_tile_id chain regen | About 280 gens all acts | Best seamless quality | Expensive and slower | Best final production target |
| C - hybrid | 50-80 gens later | Best current speed/quality balance | Some seams remain until regen | Selected |

### Phase 3-4 Outcomes
- Imported: 11 cloud PNG tilesets to `Assets/Art/Tiles/Act1_ShatteredKeep/wang_pack/`.
- RuleTiles built: 11 in `Assets/Data/Tiles/Act1_ShatteredKeep/wang_rules/`.
- Sliced PNG files: 11. Ten sheets are 16 sprites; `dark_debris_to_rift.png` sliced to 32 sprites, with only first 16 used by the RuleTile builder.
- Painter integration: `RimaWorldPainterWindow` now scans `Assets/Data/Tiles/Act1_ShatteredKeep/wang_rules` for floor RuleTiles and uses the generated broken-wall RuleTile as default wall RuleTile.
- Compile errors: 0 after Unity refresh.

### Phase 5 Test Verdict
- Wang autotile within single tileset: PASS for generated RuleTile asset creation and scene rendering.
- Adjacent different tilesets edge: SHARP_SEAM in places; expected under Strategy C until base-tile chained regen fills critical transitions.
- Visual quality: placeholder-to-production baseline. The dark Keep palette is correct, but some RuleTile outputs are visually busy and should be curated before final rooms.
- Screenshot: `Assets/Screenshots/TopDownTest_WangTilesets_v1.png`

### Recommendation
- Use Strategy C for the current Act 1 top-down baseline.
- Next strategic regen should target a chained Act 1 core set:
  1. rubble floor to worn path, using rubble base anchor
  2. rubble floor to broken wall, chained from same rubble base
  3. rubble floor to cyan rift, chained from same rubble base
  4. worn path to rift and broken wall to path only if those seams remain frequent in rooms
- Estimated regen: 4 calls minimum, about 80 generations, for Act 1 critical seams.

### Console Errors
- 0 errors after refresh.

### Next Steps for User
1. Review screenshot and decide whether the busy cyan/wall seams are acceptable for placeholder production.
2. If the screenshot reads too tiled, approve Strategy B for Act 1 only before spending generations on Acts 2 and 3.
3. Keep the current imported pack as the fallback/test baseline even if new chained sheets are generated.
