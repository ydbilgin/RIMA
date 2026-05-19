# Hades / Alabaster Dawn Floor Production - Research Synthesis

## Source Survey

### Hades / Supergiant Games

- MCV/DEVELOP interview with Jen Zee: Supergiant shipped "1,400 environment textures"; Hades 2D assets were made in Photoshop; 3D work used Maya, ZBrush, Substance Painter, Marvelous Designer, and After Effects; the game pivoted from painterly to pen-and-ink during preproduction. Source: https://mcvuk.com/business-news/behind-the-art-of-hades-we-value-artistic-integrity-and-excellence-in-artistic-craft-at-supergiant-however-were-first-and-foremost-a-game-design-lead-team/
- Same interview labels an Elysium concept as "the springboard for the Elysium tileset creation." That is the clearest public clue that Hades environments were not only single full-room paintings. Source: same MCV/DEVELOP URL.
- Ed Gorinstein's public Blocktober thread, mirrored by Reddit's Twitter bot, describes an "Elysium chamber timelapse" showing his design pass through Joanne Tran's art pass. This supports a manual design/art pass workflow, not pure procedural scatter. Source: https://www.reddit.com/r/HadesTheGame/comments/j4epr4/supergiant_dev_ed_gorenstein_shares/
- Game Developer's Greg Kasavin interview documents Hades as a small-team Early Access production where readability, replayability, and fast iteration were important. It does not disclose the floor renderer. Source: https://www.gamedeveloper.com/design/supergiant-s-fourth-outing-i-hades-i-introduces-a-more-mature-organized-dev-process
- Public sources found in this pass do not document whether Hades floors are Unity Tilemaps, a custom tilemap, packed SpriteRenderers, or full-room baked images. They do document many environment textures, tileset creation, Photoshop-authored 2D assets, and manual level/art passes.

### Alabaster Dawn / Radical Fish Games

- Radical Fish's site identifies Alabaster Dawn as the new game formerly known as Project Terra. Source: https://www.radicalfishgames.com/?cat=17
- The official August 2024 update says the vertical slice had "lots of maps created and polished" and that a public demo would be similar to that slice. Source: https://www.radicalfishgames.com/?p=7519
- The January 2024 update says the team was moving from technical systems toward actual content creation, including "mapping" the next area. It also documents day/night visual handling for props and lights. Source: https://www.radicalfishgames.com/?p=7499
- The August 2023 update says new development tools were paying off while creating content, shows dungeon work, and calls out "new and improved lines on the ground." It also describes a figure editor that keeps a pixel art look while swapping/rotating sprites based on bone angle and camera context. Source: https://www.radicalfishgames.com/?p=7410
- The April 2023 update describes a waterfall implementation using a curved surface and notes that making it fit the pixel style and connect seamlessly to water was tricky. Source: https://www.radicalfishgames.com/?p=7386
- Public sources found in this pass do not expose Alabaster Dawn's exact terrain tileset format or floor asset sheet structure. They do support a handcrafted map/content workflow in a custom HTML5-derived engine lineage, not a one-click generated painted room.

### Sea of Stars / Sabotage Studio

- Public references list Sea of Stars as Unity-built, fixed-isometric 2D pixel art. The game spent early technical effort on dynamic lighting that stayed visually consistent with pixel art. Source: https://en.wikipedia.org/wiki/Sea_of_Stars
- Sabotage's official press kit describes the game as a modernized retro RPG and provides extensive screenshots/media, but it does not document floor tile internals. Source: https://sabotagestudio.com/pt-br/kits-de-imprensa/sea-of-stars/
- In Sabotage's Reddit AMA, Thierry Boulanger says traversal aimed to be "as far away from tile-based movement as possible" through rich movement and verticality; he also says the team reviewed/polished every screen and emphasized not reusing things once they had served their purpose. Source: https://www.reddit.com/r/NintendoSwitch/comments/1d4yywt/hey_were_sabotage_studio_the_creators_of_sea_of/
- Public sources found in this pass do not prove Sea of Stars uses or avoids tilemaps for art. They do support hand-authored screens, Unity, pixel-art assets, dynamic lighting, and strong per-screen polish.

### Industry / Tool References

- Aseprite tilemaps are grid cells referencing reusable tile images in a tileset. Source: https://aseprite.com/docs/tilemap/
- Aseprite tiled mode is specifically for drawing repeating patterns quickly. Source: https://www.aseprite.org/docs/tiled-mode/
- Clip Studio's tileable texture tutorial defines seamless texture work as matching opposite edges so the image can repeat. Source: https://tips.clip-studio.com/en-us/articles/1800
- RPG Maker's grass autotile tutorial warns to avoid obvious clusters/patterns because they repeat across the map. Source: https://www.rpgmakerweb.com/blog/making-a-custom-grass-autotile-from-scratch
- Unity's Tilemap manual defines Tilemap as a GameObject for quickly creating 2D levels from tiles on a grid. Source: https://docs.unity.cn/Manual/Tilemap-CreatingTilemaps.html
- Unity 2D Tilemap Extras includes Rule Tiles, Random Brush, and other tools for terrain painting and variation. Source: https://docs.unity.cn/Packages/com.unity.2d.tilemap.extras%403.1/manual/index.html
- Unity 2D sorting docs confirm SpriteRenderer, TilemapRenderer, and SpriteShapeRenderer share transparent-queue sorting through layers/order. Source: https://docs.unity.cn/Manual/2DSorting.html
- PixelLab MCP docs exposed in this workspace: `create_topdown_tileset` generates top-down Wang tilesets for seamless terrain transitions; `create_tiles_pro` can create square top-down tile sets; `create_isometric_tile` and `create_sidescroller_tileset` are designed for depth/platform use cases and should be avoided for flat floor bases.

## Technique Breakdown

### Hades approach

Documented: Hades used Photoshop-authored 2D assets, many environment textures, and tileset creation for at least Elysium. The public Blocktober material indicates a level-design pass and environment-art pass on specific chambers.

Not publicly documented in sources found: exact floor compositor, exact tile size, whether floors are rendered through a Tilemap-like system, SpriteRenderer batches, or a custom internal equivalent.

Concrete read for RIMA: treat Hades as a hybrid of reusable environment textures/tilesets plus deliberate art-directed chamber composition. Do not treat it as a single AI-painted full-room image. Do not treat it as raw procedural scatter.

### Alabaster Dawn approach

Documented: Alabaster Dawn is a top-down 2.5D pixel-art action RPG with custom tooling, handcrafted maps, improved ground-line work, dynamic props/lights, and complex environment effects. Public updates repeatedly talk about maps/content being created and polished, not generated wholesale.

Not publicly documented in sources found: exact floor tileset format, autotile rules, or terrain blending implementation.

Concrete read for RIMA: Alabaster Dawn is closer to handcrafted, layered map art than to single rectangular room paintings. Its look depends on dense authoring and pixel-art consistency, not AI floor images with hard borders.

### Sea of Stars approach

Documented: Sea of Stars is Unity-based, fixed-isometric 2D pixel art with strong per-screen polish and dynamic lighting. Sabotage explicitly focused on traversal that does not feel grid-locked.

Not publicly documented in sources found: art tilemap internals.

Concrete read for RIMA: Sea of Stars is evidence for hand-authored screens, lighting, and verticality polish. It is not evidence that a roguelite floor should be one big generated image.

## Common Patterns (cross-game)

- Base terrain is usually reusable, but the player should not read the repeat. Seamless base textures/tiles must be low-contrast and free of memorable clusters.
- Transitions are their own production problem: path-to-grass, dirt-to-stone, wet-to-dry, and corruption-to-clean need authored edge tiles, masks, or decals.
- Decoration should sit on top of the base as stamps/props/decals, not be baked into the base tile. This lets the base remain tileable and lets gameplay masks control clutter.
- Walls/cliffs/vertical faces should be separate layers from the flat walkable floor. If wall depth is baked into the floor base, the result becomes a platform/island.
- "Natural" usually means: seamless base + transition patches + irregular decals + hand pass. A pure square tilemap reads as grid; a pure full-room painting does not scale for roguelite recomposition.

## Critical findings: Where RIMA went wrong

1. The PixelLab outputs were likely using the wrong visual target: "platform", "stone base", "raised", "isometric", "cliff", "island", or side/depth-oriented endpoints produce an object, not a floor surface.
2. A rectangular or organic raised edge around a floor image is a platform cue. It tells the model to make a self-contained asset with side walls and shadows.
3. "Top-down ortho" alone is insufficient if the rest of the prompt asks for a base, platform, slab, cliff, border, or dramatic edge. The negative requirement must be explicit: no side faces, no raised rim, no object silhouette, no self-contained island.
4. Hades public evidence points to environment textures/tilesets and manual chamber art passes. RIMA was asking PixelLab for finished room-like floor bases, but the target pipeline needs reusable flat materials plus overlays.
5. Alabaster Dawn and Sea of Stars are high-authoring examples. They should be used as composition/style references, not as proof that an AI can generate one complete natural room floor without a level-art pass.

## Actionable Plan for RIMA

### Floor base (single image vs tileable)

Recommendation: use multiple flat seamless floor materials, not a single 792x688 painted scene as the source of truth.

Production shape:

1. Generate or paint 256x256 or 512x512 seamless base textures for dominant stone/dirt/grass.
2. Generate terrain-transition tiles/patches for stone-to-path, stone-to-grass, dirt-to-grass.
3. Add larger irregular decal patches on top: dirt scuffs, moss wisps, cracks, stains, worn path shapes.
4. For a locked showcase room, allow a final 792x688 paintover layer after layout is stable, but keep it optional and non-authoritative.

Reason: Hades documents many environment textures and tileset creation; industry tutorials warn that obvious clusters repeat; RIMA needs roguelite recomposition. A single full-room painting gives one good screenshot but does not solve production.

### Walls (separate sprites)

Recommendation: separate walls from the floor.

- Floor layer: flat, no vertical edge, no side face, no cast shadow implying height.
- Transition/decal layer: dirt/grass/path overlays, cracks, moss, stains.
- Wall/edge layer: separate vertical wall sprites or prefabs sorted above floor.
- Prop layer: rocks, debris, pots, vegetation, interactables.

Reason: Unity's 2D renderer sorting supports TilemapRenderer and SpriteRenderer layering. Public Hades/Alabaster Dawn sources do not disclose their exact wall renderer, so this is a RIMA production recommendation, not a claim about their internals.

### Decorations (stamp scatter)

Recommendation: hybrid placement.

- Use Bridson Poisson or similar blue-noise scatter only for candidate positions.
- Filter candidates through gameplay masks: no spawn zone, no dash corridor, no enemy arena center, no doorway, no interactable overlap.
- Convert accepted candidates into editable stamps.
- Final pass must be manual/art-directed for hero rooms and screenshots.

Reason: Hades public Blocktober material shows design and art passes on chambers. Pure random scatter will not reach that quality; manual-only placement is slower than necessary.

### PixelLab REST API prompts (revised)

Primary PixelLab endpoint/tool:

- Use `create_topdown_tileset` for terrain transitions. It is built for top-down Wang tilesets and seamless corner-based terrain blending.
- Use `create_tiles_pro` with `tile_type: "square_topdown"` and `tile_view: "top-down"` for flat material tile variants.
- Avoid `create_isometric_tile` for floor bases. It is intended to create depth/blocks/objects.
- Avoid `create_sidescroller_tileset` for floor bases. It is intended for side-view platforms.

Parameter direction:

- `view`: prefer `"low top-down"` or highest available top-down setting for map floors.
- `outline`: `"lineless"` or minimal/selective outline.
- `shading`: `"basic shading"` or `"medium shading"`; avoid dramatic side shadows.
- `transition_size`: `0.25` for subtle natural blends; test `0.5` only if transition needs broader patches.
- Avoid `tile_depth_ratio`, `tile_view_angle` below flat/top-down, "thick tile", "block", or any endpoint made for isometric depth.

Prompt formula:

```text
flat top-down seamless floor material, [material], hand-painted 2D game texture, walkable surface only, no raised edges, no side faces, no cliffs, no platform, no island silhouette, no border, no cast shadow, subtle natural variation, low contrast repeat-safe details
```

Examples:

```text
lower_description: flat worn charcoal stone floor, hand-painted 2D top-down walkable surface, subtle cracks and mineral speckles, no raised edges, no cliffs, no side faces
upper_description: flat dusty violet dirt path, hand-painted 2D top-down walkable surface, soft uneven patches, no platform, no border
transition_description: soft dust and small pebbles blending into cracked stone, flat painted surface only
```

```text
1). flat mossy grey stone floor, seamless top-down walkable surface, subtle mineral noise, no border, no side faces
2). flat packed dirt floor, seamless top-down walkable surface, small dust variation, no raised edge
3). flat sparse grass over dirt, seamless top-down walkable surface, no bushes, no props, no shadowed rim
4). flat worn path scuff decal, transparent-background style, irregular soft edge, no platform
```

PixelLab suitability: good for first-pass tileable materials and Wang transitions. Not enough by itself for Hades-level room art unless RIMA adds a manual composition/paintover pass and rejects outputs with any vertical rim/platform cue.

### Implementation order

1. Flat base validation: generate one stone-to-dirt `create_topdown_tileset` and one `create_tiles_pro` square-topdown set. Import into a small Unity Tilemap test. Gate: no side faces, no border, no raised platform illusion.
2. Layer validation: add separate wall sprites and 20-40 decals/stamps over the flat base. Gate: floor still reads flat; walls carry height.
3. Composition validation: run Poisson candidate placement, then manually lock a Spawn_01 art pass. Gate: screenshots at gameplay camera read natural, not square-grid and not island/platform.

## Risks / unknowns

- Hades' exact internal floor renderer was not found in public sources during this pass. Do not claim it uses Unity Tilemap or one specific renderer.
- Alabaster Dawn's terrain authoring format is not publicly documented in the sources found. It may use custom tools beyond what screenshots/devlogs reveal.
- Sea of Stars public material supports hand-authored Unity pixel-art screens, but not a specific tileset/floor answer.
- PixelLab may still produce depth artifacts even with correct prompts. RIMA needs an automated rejection checklist: side face visible, dark underside, rim border, island silhouette, hard rectangular boundary, or self-contained platform equals reject.
- If the desired final look is truly Hades-like painterly rather than pixel-art, PixelLab tile outputs may need external paintover or an image-generation workflow aimed at seamless painterly textures rather than pixel tiles.
