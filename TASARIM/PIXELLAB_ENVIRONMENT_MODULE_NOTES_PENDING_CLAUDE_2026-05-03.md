# PixelLab Environment Module Notes Pending Claude - 2026-05-03

Status: PENDING CLAUDE FINAL DECISION
Scope: PixelLab tool choice, tile size, environment module production for RIMA rooms

## User Intent

User wants to discuss with Claude before making a final decision.

This file is a note, not a locked decision.

## Current Understanding

RIMA should not ask PixelLab to generate final playable rooms as one image.

PixelLab should generate reusable modules:
- floor tile variants
- floor detail decals
- wall modules
- shell/edge modules
- rubble pieces
- chain/prison props
- rift crack decals
- light fixtures
- landmarks

Unity/LDtk/Tiled should assemble these modules by masks, adjacency, and influence fields.

## Tool Routing Notes

Likely routing:

| Asset | PixelLab Tool | Notes |
|---|---|---|
| Base floor tiles | Create tiles PRO | best first test for consistent floor sets |
| Floor transition/edge tiles | Create tiles PRO or Create tileset | use adjacency/autotile needs |
| Single floor experiments | Create isometric tile | quick test only |
| Wall modules | Create map object | transparent structural pieces |
| Shell/void/rubble/chain/rift pieces | Create map object | object/decal style pieces |
| Landmarks | Create object or Create map object | 64x64 or 128x128 depending on size |
| Whole scene/map | Create map / Extend map | concept/reference only, not final playable room |

## Floor Size Question

Important: the PixelLab UI setting name is not the final production truth. The acceptance criterion
is the exported sprite footprint.

Target for RIMA base floor:

```text
visible floor footprint: about 64x32 px diamond
top surface only
no side face
no raised slab
no bevel thickness
```

Recommended first test:

```text
Tool: Create tiles PRO
Tile type: Isometric
Tile size: 64px
View angle: about 45 degrees
Thickness: 0%
Outline mode: test segmentation or low outline if available
```

Why not lock `32px` now:
- `32px` may produce about a `32x16` visible diamond, likely too small/detail-poor for RIMA's
  128px character scale and ARPG camera.

Why not lock `32x64` now:
- The label may represent a non-square/isometric canvas option, but it is not guaranteed to produce
  the desired visible `64x32` flat top surface.
- It should be A/B tested only if `64px + thickness 0%` fails.

Fallback A/B test:

```text
A: Isometric, 64px, thickness 0%
B: Isometric, 32x64, thickness 0%
```

Pick whichever produces the best measured footprint and visual read:

```text
opaque bounds near 64x32
continuous floor read in 4x4 mockup
no raised slab / no side face / no chunky cobblestone paneling
```

## First Module Pack Candidate

Candidate first PixelLab pack after Claude approval:

```text
Act 1 Shattered Keep module pack

Floor:
- 6 flat 64x32 top-surface floor variants
- base, subtle crack, faint wear, light dirt stain, rare chip, subtle value variation

Wall/Shell:
- straight wall
- inner corner
- outer corner
- broken cap
- doorway/arch
- foreground parapet

Connected motif pieces:
- rubble fan small/medium/large
- rift crack decal set
- chain anchor
- chain segment / hanging chain
- torch sconce
- cold rift sconce
- void/chasm edge lip

Landmarks:
- reliquary plinth
- chain seal
- rift well
- boss door fragment
```

## Open Questions For Claude

1. Should first test use PixelLab `Create tiles PRO` UI or API/MCP?
2. Should floor test start with `64px`, or should `64px` and `32x64` be tested in the same batch?
3. Should first pack be named `Shattered Keep`, `Shattered Ruins`, or `Sunken Keep`?
4. Should the first batch prioritize floor correctness before walls/props, or produce a full small
   module pack for style cohesion?

## 2026-05-04 User Proposal: Style References + Variation Packs

Status: candidate workflow, pending Claude review.

User proposal:
- Codex/ChatGPT can generate or select style reference tiles first.
- Feed those references into PixelLab Create Image Pro / style reference mode.
- Generate either:
  - `32x32` with `64 variations`, or
  - `64x64` with `16 variations`.
- Use the best outputs as shared visual language.
- Use some direct isometric tile generations as occasional raised/accent pieces for natural height.

Initial evaluation:
- The idea is directionally good if the outputs are treated as source/style material and selected
  modules, not as a guaranteed finished autotileset.
- RIMA still needs a flat readable playable floor as the default layer.
- Height should not appear randomly in the playable floor unless it has collision/readability rules.
- Raised outputs are better used as:
  - visual shell tiles
  - broken ledges
  - rubble mounds
  - chasm lips
  - non-playable edge caps
  - authored obstacles
- A few subtle height/accent pieces can make rooms feel natural, but they must not break combat
  readability or imply false walkable elevation.

32x32 / 64 variations:
- Pros: many candidates, fast style exploration, good for decals/small floor marks.
- Cons: likely too small for RIMA 128px characters; may produce noisy stamps instead of readable
  64x32 isometric floor pieces.
- Best use: style mining, crack/dirt/decal candidates, not final base floor.

64x64 / 16 variations:
- Pros: more detail budget, better chance of readable 64x32 diamond footprint after crop/selection,
  fewer candidates to QC.
- Cons: may add slab thickness or side faces if prompt/tool settings are loose.
- Best use: first serious test for base floor/reference pack.

Recommended A/B before final choice:
```text
A. 64x64 Create Image Pro, 16 variations
   Goal: flat top-surface Shattered Keep floor reference tiles.

B. Create tiles PRO, isometric 64px, thickness 0%, 6-8 variations
   Goal: measured 64x32 flat diamond production candidates.

C. 32x32 Create Image Pro, 64 variations
   Goal: small decals, cracks, dirt, chips, rift hairlines, not base floor.
```

Selection criteria:
- base floor reads flat and continuous in a 4x4 mockup
- no side wall, bevel, chunky raised slab, or false platform
- silhouette/opaque bounds near the target footprint
- style matches RIMA: fractured epic, controlled ruin, not cozy grass/boardgame tiles
- high-value details reserved for accent tiles/decals, not every base tile

Suggested Claude question:
```text
Claude, evaluate this RIMA environment production workflow:

We may create style reference tiles first, then use PixelLab style-reference generation to build
sets. Options:
1. 32x32 Create Image Pro with 64 variations
2. 64x64 Create Image Pro with 16 variations
3. Create tiles PRO isometric 64px thickness 0%

Question:
- Should RIMA's playable floor stay strictly flat top-surface, with height only as visual shell /
  obstacle / ledge modules?
- Should first production test be 64x64/16 style-reference variations, Create tiles PRO 64px
  isometric, or both in an A/B test?
- Is 32x32/64 useful only for decals/style mining, or can it become production floor?

Return:
- decision
- first batch settings
- reject criteria
- folder/import naming plan
```
