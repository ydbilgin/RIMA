# VERDICT
Use a hybrid authored-Wang plus Karar #143 overlay pipeline: hand/direct the 16 same-elevation floor-to-floor transition cells, then cover their grid language with L4 organic patch overlays and L5/L6 scatter. Do not use PixelLab `create_topdown_tileset` for Act 1 flat floor blends and do not ship the current splat shader as the main visual solution; both solve adjacency but miss the hand-painted moss/edge-wrap look.

The 1-week MVP should produce one production pair first: Cool Granite base, Worn Stone Path base, and a Granite-to-Path transition set made from AI-assisted base/edge art assembled or corrected in Aseprite/Tilesetter, then integrated into Unity RuleTile/Tilemap with Karar #143 layers.

# 1. Colossus pipeline reverse-engineered
Evidence used:
- Local crops from public RPGFan screenshots in `STAGING/colossus_floor_crops.jpg`.
- SteamDB/RPGFan public pages:
  - https://steamdb.info/app/1722800/screenshots/
  - https://www.rpgfan.com/gallery/colossus-eternal-blight-screenshots/

Technical inference:
- The stone path to grass edge is not a splat gradient. It is discrete pixel art at tile/cell scale: stone cells keep readable pebble/cobble interiors while their boundary cells carry irregular grass intrusion, moss bits, isolated flowers, and broken outline clusters.
- The path border is not a single smooth alpha band. It uses repeated local edge motifs: stones pinch inward, grass wraps into gaps, and small dark pixels/tufts break the silhouette. This points to authored transition art, likely Wang/blob terrain tiles or hand-placed transition stamps, not only a shader.
- Grass interiors are not Perlin noise. They use large L4-style darker green patches with soft ragged edges across multiple cells, plus micro flower/scatter accents. This is exactly the role Karar #143 gives to L4 large patch overlays.
- Cliffs/walls are separate vertical side sprites with strong height language. They do not share the floor transition logic. Colossus places floor on one plane, then uses side-wall/cliff art as a separate layer around terrain drops.
- The boss sand/grass arena shows another same-height material boundary: the sand edge is a broad painted transition shape with darker inner sand and grass bites at the outside. It reads as authored edge art plus larger overpaint patches, not a raw procedural blend.

Asset structure that most likely produces the look:
- L1/L2: flat 32px-ish terrain variants for grass, stone, sand, path.
- Transition Tilemap: authored 16-corner or 47-blob transition cells for same-elevation material pairs.
- L4: large irregular darker patches, moss spreads, dirt/sand broad shapes, placed across many cells to hide repetition.
- L5/L6: flowers, pebbles, small tufts, broken stones, path chips.
- L3: cliff/wall/drop sprites, separate from floors.

# 2. F1 existing Wang16 review
Contact sheet reviewed: `STAGING/f1_tilesets_contact_sheet.png`.

| Tileset | Visual character | Decision |
|---|---|---|
| `cold_floor_wall` | Grid floor with bright rimmed raised wall/path edge. Strong platform/elevation cue. | Wall-only reference; scrap for floor-to-floor. |
| `debris_rift` | Dark vertical strips, bright side rims, block islands, rift/platform language. | Scrap for floor-to-floor; possible rift/wall mood reference only. |
| `floor_wall` | Broken wall and floor mixed with blue vertical wall faces and heavy outlines. | Wall-only; not a flat floor blend. |
| `mauve_hexagon` | Pastel raised path with side bevels and geometric cells. Wrong palette and elevation. | Scrap for Act 1. |
| `path_rift` | Dark void/rift boundary around blue brick path, hard white edge. | Scrap for floor-to-floor; possible rift hazard reference only. |
| `pink_cream` | Smooth flat pink/cream blobs, no wall face, but wrong palette/style and too clean. | Salvage conceptually as flat-blend proof; not usable for Act 1. |
| `rubble_moss` | Moss blocks and purple floor with bright white outline, grid-visible, blocky. | Scrap for Colossus-quality floor blend. |
| `rubble_path` | Stone path/rubble with hard cyan-white rim and platform-like boundary. | Scrap for floor-to-floor; too rimmed. |
| `slate_mineral` | Dark blue slate with bright yellow/white mineral border, flat but graphic/rimmed. | Scrap for Act 1 floors; possible hazard/accent reference. |
| `wall_path` | Wall body and path separated by bright rim, obvious vertical wall material. | Wall-only. |
| `wall_rift` | Wall/rift edge with bright cyan rim and dark platform side. | Wall/rift-only; not floor-to-floor. |

Overall: none of the 11 existing F1 Wang sheets should be the production solution for grass/path or granite/path same-elevation blending. `pink_cream` is the only sheet that demonstrates a flat blob transition, but it is off-palette, too smooth, and not in the Colossus chunky 32px style.

# 3. Recommended RIMA pipeline
Pipeline decision:
1. Generate only flat base material variants with `create_tiles_pro`.
   - Settings locked by NLM/Karar #143: `tile_type: square_topdown`, `tile_size: 32`, `tile_view: top-down`, `tile_depth_ratio: 0`, `outline_mode: segmentation`.
   - Generate Cool Granite and Worn Stone Path separately. No moss, no rift, no walls, no edge, no "tile" wording in prompts.
2. Create transition source art, not final PixelLab topdown tilesets.
   - Use Codex `gpt-image-1` or local FLUX/SD to make a 32px style reference strip: granite/path edge with mossy stone chips, no bevel, no height, no rim, no outline frame.
   - Use Aseprite to reduce/clean to RIMA palette and remove anti-aliasing/soft 64px paint.
   - Use Tilesetter Wang Set or manual Aseprite assembly to produce the 16 corner combinations.
   - Critical rule: transition art must sit inside the tile, at same elevation, with grass/moss/path intrusion, not as a raised border.
3. Integrate as Unity Tilemap/RuleTile.
   - L1: dominant Cool Granite base variants.
   - L2: Worn Stone Path base and 16-cell Granite-to-Path transition RuleTile.
   - L3: separate wall/perimeter cap sprites only; never baked into the floor transition.
   - L4: transparent large patches over the tilemap: darker cool moss, dust drift, mud crust, ash smear, cracked rubble, large path-wear patches.
   - L5/L6: small stones, lichen, rift cracks, flowers/ritual flecks, placed with edge-biased density.
4. Keep TerrainBlend as a preview/prototype branch only.
   - Current shader samples RGBA splat channels, perturbs with noise, raises weights by `_BlendSharpness`, then linearly mixes terrain textures. That creates a mathematically smooth boundary, not painted stone/moss edge-wrap.
   - If revived, it should only drive masks for L4 patch placement or be upgraded into a painted-edge-stamp shader. It should not be the MVP floor visual.
5. Rejection gates for every produced sheet:
   - Reject if any side face, raised rim, cliff edge, bevel, cast shadow, tile frame, or black border appears.
   - Reject if 64px smooth paint survives after downscale.
   - Reject if the edge is a uniform alpha/noise gradient.
   - Reject if central combat area becomes noisy; L4 density must be edge-biased.

Tool choice:
- Primary: Aseprite plus Tilesetter/manual assembly for the one production transition pair. This is the only path with enough control for Colossus-like edges.
- AI assist: use `gpt-image-1` or local FLUX/SD for source motifs, not as the final unreviewed Wang sheet.
- PixelLab: use `create_tiles_pro` for flat bases and `create_map_object`/`create_object` for overlays/scatter. Do not use `create_topdown_tileset` for flat floor-to-floor Act 1 MVP.

# 4. 1-week MVP asset checklist
Scope: Spawn_01, Combat_Room, Elite_Room. One biome pair first: Cool Granite <-> Worn Stone Path. Moss/dust/rift are overlays, not extra terrain pairs.

Day 1 - Base floors:
- 16 Cool Granite variants from PixelLab `create_tiles_pro`.
- 16 Worn Stone Path variants from PixelLab `create_tiles_pro`.
- Prompt anchor:
  - `1). cool muted granite floor surface, chunky 32px pixel art, top-down pure orthographic, #3A3D42 and #4E5260, tiny cold cracks, no wall, no rim, no border, no shadow, no bevel 2). ... 16). ...`
  - Negative terms: `flagstone, slab, mortar, brick, cobble, masonry, cut-stone, regular, grid, pattern, uniform, repeating, baked light, shadow cast, directional, 3d render, smooth gradient, anti-aliasing`.

Day 2 - Granite-to-Path transition:
- 1 transition source strip/motif sheet from `gpt-image-1` or local FLUX/SD.
- 16 Wang corner cells assembled in Tilesetter/Aseprite.
- 1 Unity RuleTile/Tile palette entry for the transition pair.
- Prompt anchor for source motifs:
  - `32px chunky pixel art top-down same-height floor transition between cool granite floor surface and worn stone path surface, painterly mossy organic edge wrap, broken stone chips inside the edge, muted cool palette #3A3D42 #4A4842 #5A6B5A, no height difference, no cliff, no side face, no rim, no frame, no black outline, no smooth gradient`.

Day 3 - L4 patch overlays:
- 4 darker cool moss large patches, transparent, 24-48px.
- 4 dust/ash/mud broad patches, transparent, 24-64px.
- 2 cracked rubble/path-wear large patches from existing AssetPack can be reused after palette/outline cleanup.
- Use `create_map_object` for new transparent patches.

Day 4 - L5/L6 scatter:
- 4 small stones/pebbles, transparent.
- 3 moss/lichen tufts, transparent.
- 3 rift crack/seep accents, transparent.
- Use `create_object` or existing `Assets/Art/Rooms/AssetPack/FloorStones`.

Day 5 - Unity integration:
- Import/slice base floors and transition sheet.
- Create or update RuleTile assets for Granite, Path, GranitePathTransition.
- Build three room palettes: Spawn_01 soft density, Combat_Room center-clear, Elite_Room stronger edge dressing.
- Disable shader blend for these rooms unless explicitly comparing.

Day 6 - Composition pass:
- Spawn_01: path curve + grass/moss patch analog with cool keep palette.
- Combat_Room: central readable floor, path/patches only at edges and around anchors.
- Elite_Room: stronger L3 wall perimeter, larger L4 path-wear/moss arcs, limited L6 rift.

Day 7 - QC gates:
- Screenshot each room at game camera scale.
- Pixel check: no visible tile frames, no rim/elevation artifacts, no 64px smoothness, no overdecorated combat center.
- Compare against `STAGING/colossus_floor_crops.jpg`.
- If the transition set fails, fix in Aseprite before generating any second terrain pair.

# 5. Risks + fallbacks
Risk: AI-generated transition set is not coherent across all 16 Wang cells.
Fallback: use AI only for edge motifs, then assemble the 16 cells manually in Aseprite/Tilesetter.

Risk: Tilesetter output looks too mechanical.
Fallback: override inner/outer corner cells manually in Aseprite. Tilesetter documentation supports custom corner/source replacement, so the generated sheet can be a scaffold rather than final art.

Risk: PixelLab `create_tiles_pro` still produces grid/brick language.
Fallback: simplify prompts further to "micro-textured cool stone floor surface" and use Aseprite palette/noise cleanup. Do not add moss/cracks/path words into base prompts.

Risk: Colossus quality cannot be reached with 16 cells alone.
Fallback: keep the 16-cell RuleTile for correctness, then hide repetition with L4 patch atlases and rare transition overlay stamps. Do not add a shader gradient as the first fallback.

Risk: budget/time pressure.
Fallback: ship only Granite base + Path base + one transition pair + 6 overlays for the 1-week MVP. Defer water, rift, and second biome transitions.

# 6. Open questions for user
1. Should the first production pair be `Cool Granite <-> Worn Stone Path` for Act 1, or do you still want a `grass/moss <-> stone path` showcase first to match the Colossus screenshots more literally?
2. Are you willing to buy/use Tilesetter now if it remains cheap on Steam, or should the MVP assume Aseprite-only manual assembly?
3. For the 1-week MVP, should Combat_Room be the visual target room, or should Spawn_01 be the polished screenshot target?
4. Should `TerrainBlend` be archived after this decision, or kept behind an experimental toggle for future painted-edge-mask experiments?
