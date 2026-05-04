# RIMA Isometric Environment Production Feedback - 2026-05-03

Reference image:
`F:/Antigravity Projeler/Pixellab/RIMA_REFS/rima_style_anchor.png`

## Verdict

RIMA can keep this camera/projection look. The target should be a fixed orthographic
isometric 2D gameplay view, not a perspective camera and not a single baked room image.

Use the reference as art direction for:
- camera framing
- wall height/read
- floor texture density
- prop density
- local light color
- black negative space outside rooms

Do not use it as a direct production asset format.

## Camera Direction

Use Unity orthographic camera.

Current practical baseline:
- Orthographic camera, no perspective tilt.
- Isometric grid stays in Unity.
- Camera follows player.
- Room framing should show enough floor around the player for combat readability.
- For larger test rooms, `orthographicSize` around `6.0-7.0` reads closer to the reference than the previous tight `5.0`.

The reference shows a room-level composition, but gameplay should not always show the
whole room if enemies/projectiles need readability. The correct compromise is:
medium zoom during combat, optional slight zoom-out for room reveal/map moments.

## Production Format

Produce rooms from modular pieces:
- 64x32 flat isometric floor top-surface tiles.
- Dedicated wall pieces, not floor tiles pretending to be walls.
- Wall categories: straight north wall, straight east/west wall, inner corner, outer corner,
  doorway/arch, pillar, broken wall cap.
- Props as separate transparent sprites: rubble, bones, chains, candles, plaques, broken slabs,
  moss stains, cracks, blood stains.
- Lights as Unity 2D lights/VFX, not baked into every tile.

Do not generate whole dungeon rooms as one image except for concept/reference. Whole-room images
will not support collision, random room assembly, combat readability, or reuse.

## Floor Feedback

The current Stone Dungeon floor tiles are useful only as test placeholders. They read as
overlapping raised stone blocks because the visual footprint is too tall and too individually
outlined. Final floors need:
- flat top-surface only
- 64x32 diamond footprint
- no raised side faces
- weaker individual tile outline
- controlled value range so repeated tiles merge into one floor plane
- 4-8 variants maximum per biome, weighted random

Good prompt direction:
"flat isometric dungeon floor top surface, 64x32 diamond tile, worn stone slabs, subtle cracks,
no side walls, no raised block, no bevel height, seamless edge continuity, hand-painted dark fantasy"

## Wall Feedback

The current wall set passes for a temporary test scene, but it reads more like chunky block/parapet
border than the final reference wall. Final walls should be generated as structural wall modules:
- visible vertical wall face
- top cap
- consistent height
- modular left/right/center pieces
- corner/pillar pieces
- doorway pieces
- damage/moss variants kept sparse

Good prompt direction:
"isometric dungeon wall module, 64x96 transparent background, vertical stone wall face with top cap,
matches 64x32 floor diamond grid, hand-painted dark fantasy, modular straight segment, no floor attached"

## Foot Shadow

Use Unity-side blob shadows under characters/enemies.

Recommended implementation:
- Add a child GameObject under each actor, e.g. `GroundShadow`.
- SpriteRenderer uses a soft black/dark ellipse sprite.
- Local position around feet: `(0, -0.08, 0)`, tuned per character.
- Scale around `(0.55, 0.22, 1)` for player, smaller/larger per actor.
- Alpha around `0.25-0.40`.
- Sorting layer/order below character, above floor if needed.
- Do not rely on real-time 2D light shadows for this. Character grounding needs art-directed,
  stable contact shadows.

Optional polish:
- Squash/stretch shadow slightly during dash/jump-like motion.
- Fade shadow for teleport/stealth.
- Use larger softer shadows for bosses.

## Room/Dungeon Assembly

Generate dungeons room-by-room, not as long continuous baked corridors.

Recommended system:
- Rooms are modular chunks with entrances on N/E/S/W.
- Each room paints floor from weighted variants.
- Walls use rule/brush placement for borders and corners.
- Door thresholds hide biome transitions.
- Props are scattered after room layout with density rules.
- Landmarks are hand-placed or placed from room templates.

Long dungeons should be a graph of rooms connected by door/threshold segments. The player sees
one room/combat arena at a time, while art stays modular and reusable.

## PixelLab/MCP Use

Good MCP targets:
- floor variant batches
- wall module candidates
- props/landmarks
- icon/item art

Risky MCP targets:
- whole-room final assets
- transition/wang tiles if output is top-down square instead of 64x32 isometric
- floor tiles with side faces or heavy bevels

For consistency:
1. Create one approved style anchor tile/module.
2. Use it as style reference for batch variants.
3. QC by contact sheet and 4x4 repeated mockup before importing to Unity.
4. Reject any tile that creates height, overlap, or checkerboard noise.

