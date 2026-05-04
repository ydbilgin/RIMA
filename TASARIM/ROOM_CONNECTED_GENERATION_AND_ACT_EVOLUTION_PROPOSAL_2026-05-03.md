# RIMA Connected Room Generation and Act Evolution Proposal - 2026-05-03

Status: PROPOSED FOR CLAUDE FINAL DECISION
Owner: design/orchestration
Scope: room naturalization, connected detail generation, act-to-act dungeon form language

## Starting Point

User feedback:
- Rooms should look clean and natural, not like random props scattered on a tile grid.
- Details should be generated as connected structures.
- Each act should have a different room form.
- Within an act, room form can evolve as the player goes deeper.
- Claude should later review and make the final call.

Research basis:
- Slynyrd texture/tile guidance: natural pixel-art texture uses balanced clusters, negative space,
  limited colors, and controlled variation. Single-tile repetition and evenly distributed noise
  look artificial.
- Blob/Wang/autotile systems: terrain borders look natural when tiles are selected from neighbor
  relationships, not by isolated random choice.
- Unity Rule Tile/Tilemap Extras: adjacency rules are the correct Unity-side mechanism for
  connected wall/floor/edge visuals.
- WFC: useful for local adjacency constraints, but weak at global room structure unless guided by
  higher-level rules.
- Red Blob noise guidance: noise is useful for variation fields, but local calculations do not
  create meaningful global relationships by themselves.
- Unexplored / graph replacement analysis: strong procedural dungeons use abstract structure first,
  then progressively resolve into room type, setpiece, terrain, item, and beauty passes.

## Core Recommendation

RIMA should use connected generation, not scatter generation.

Wrong model:

```text
make floor -> randomly place cracks/rubble/props/lights
```

Correct model:

```text
choose room story role
build semantic skeleton
derive connected masks and influence fields
paint floor/wall/shell by adjacency
place motif packages from anchors
validate combat readability
```

Details should belong to a source:
- collapse starts from a broken wall or ceiling fall
- rift cracks radiate from a tear or shrine
- moss/blood/dust gathers along wall bases, low ground, and corners
- chains run between anchors, cells, doors, or ritual devices
- rubble fans outward from a breach
- light pools belong to torches, rift seams, relics, or boss doors

If a detail cannot answer "what caused this and what is it connected to?", it should usually not be
placed.

## Generation Layers

### 1. Macro: Act Flow

The run graph chooses the sequence of narrative bands and room types.

Example:

```text
Act -> Band -> RoomFamily -> Variant -> CombatQuestion
```

Macro does not paint tiles. It decides meaning and pacing.

### 2. Meso: Room Skeleton

Each room gets a connected skeleton:

```text
Entrance
Exit sockets
Combat core
Primary landmark
Dash lanes
Wall mass
Visual shell
Occluder bands
Spawn bands
Light anchors
Damage/rift/collapse vectors
```

This is where natural form is decided. It should be authored or generated from templates, not from
free random scatter.

### 3. Micro: Tile and Prop Resolution

Tiles and props are selected from connected masks:

```text
FloorMask -> floor variants
WallMask -> rule/edge/corner wall modules
ShellMask -> rubble/void/wall caps/chasm backdrop
RiftMask -> cracks/glow/tear decals
CollapseMask -> broken stones/dust/edge damage
```

This layer can use weighted randomness, but weights come from fields and masks.

## Required Connected Fields

Every generated room should compute these fields before painting final tiles:

| Field | Use |
|---|---|
| `DistanceToWall` | wall grime, rubble, moss, blood, foreground occluder density |
| `DistanceToDoor` | keep doors clean, place threshold props, avoid spawn clutter |
| `DistanceToLandmark` | shrine/rift/relic decoration intensity |
| `CombatClearance` | forbid clutter in dash lanes and core combat space |
| `RiftInfluence` | cyan/violet cracks, void bite-outs, magic light sockets |
| `CollapseInfluence` | rubble fans, broken edges, missing wall chunks |
| `Age/DustInfluence` | low-contrast floor wear and visual continuity |
| `LightInfluence` | warm/cold pools tied to actual sockets |
| `ShellDepth` | how far a cell is from playable floor into non-playable staging |

The important part: props do not roll independently. They query fields.

## Motif Packages

A motif package is a connected decoration/system rule, not a loose prop list.

### Collapse Motif

Source:
- wall breach
- ceiling fall
- broken column

Generated pieces:
- missing wall segment
- rubble fan
- cracked floor trail
- dust/darker floor near rubble
- maybe a blocked non-playable shell pocket

Placement rules:
- never blocks door clearance
- never cuts all dash lanes
- rubble density decreases with distance from source

### Rift Motif

Source:
- rift tear
- shrine wound
- boss-door seal
- broken relic

Generated pieces:
- connected crack path
- cyan/violet glow decals
- void bite at room edge or shell
- cold light socket
- optional hazard marker in late rooms

Placement rules:
- rift lines should follow one or two dominant vectors
- do not pepper random cracks everywhere
- center hazard only if the combat question uses it

### Chain/Prison Motif

Source:
- anchor points
- cell wall
- boss door
- guard platform

Generated pieces:
- chain line or hanging chain props
- anchors at both ends
- shadow under chains
- rust/blood floor stains below
- cell/cage props near walls

Placement rules:
- chains imply direction and containment
- avoid loose single chain props with no anchor logic

### Reliquary/Ossuary Motif

Source:
- relic plinth
- bone well
- tomb row

Generated pieces:
- circular or loop-like floor wear
- bone/rubble clusters around perimeter
- low candles/relic lights
- wall plaques or shrine caps

Placement rules:
- props collect in arcs, rows, or clusters
- center remains readable unless landmark is the combat focus

## Act Form Language

### Act 1: Shattered Ruins / Sunken Keep

Core read:

```text
old controlled architecture, broken by the Fracturing, still trying to preserve order
```

Shape language:
- built halls
- thick walls
- cells, chains, reliquaries, ritual platforms
- symmetry still visible
- broken edges and rift wounds increase over depth

Act 1 progression:

| Depth | Band | Form |
|---:|---|---|
| 1-2 | Threshold | clean built halls, simple breaks, readable entry rooms |
| 3-4 | Guard/Prison | cells, chains, side pockets, controlled corridors |
| 5-6 | Ossuary/Reliquary | loops, relic anchors, basin shapes, dead order |
| 7-8 | Ritual/Rift | sacred geometry breaks, rift wells, shell bite-outs |
| Boss | Containment | deliberate arena, thick perimeter, chain anchors outside playable floor |

Naturalization rule:
- Act 1 does not become a cave.
- It becomes an ordered keep with damaged connected motifs.

### Act 2: Bleeding Wastes

Core read:

```text
the wound beneath the keep; architecture is being consumed by living terrain
```

Shape language:
- lobed rooms
- vein-like side paths
- broken platforms in a red/purple wound field
- organic edges over old stone foundations
- fewer straight guard-room lines

Act 2 progression:

| Depth | Band | Form |
|---:|---|---|
| 1-2 | Descent Wound | old stone shell with bleeding cracks |
| 3-4 | Vein Fields | branching red/purple channels, soft lobed floor masks |
| 5-6 | Devoured Works | forge/shop/event rooms partly swallowed by living matter |
| 7-8 | Heart Scar | large open arenas with pulsing shell and blood-lit anchors |
| Boss | Echo Twin Threshold | dual-lobed arena: two identities sharing one body |

Naturalization rule:
- Act 2 can look more organic, but it should still show remnants of constructed rooms.
- Organic growth should follow connected veins and pools, not random red stains.

### Act 3: Core Approach

Core read:

```text
reality is thinning; impossible architecture, echoes, mirrors, and void-gold order
```

Shape language:
- angular fragments
- floating platform reads
- mirrored halves that do not perfectly match
- recursive loops
- void gaps outside playable shell
- gold/black high-contrast landmarks

Act 3 progression:

| Depth | Band | Form |
|---:|---|---|
| 1-2 | Echo Archive | structured rooms with repeated/mirrored fragments |
| 3-4 | Null Courts | cleaner void borders, fewer props, stronger silhouette |
| 5-6 | Recursion Lattice | repeated room motifs, shifted copies, impossible side pockets |
| 7-8 | Core Bridge | broken platforms, heavy void shell, gold anchor lines |
| Boss | Wound / Recursion | arena changes under the player, controlled instability |

Naturalization rule:
- Act 3 is not noisy.
- It is cleaner, stranger, and more intentional than Act 2.

### Final: Nexus Core

Core read:

```text
the source remembers every form; clean mirror-space with deliberate fractures
```

Shape language:
- white/black contrast
- class-color echoes
- mirror symmetry that breaks at key moments
- minimal prop noise
- huge readable silhouettes

Progression:
- memory threshold
- class reflection rooms
- fracture decision spaces
- final mirror arena

Naturalization rule:
- Final rooms should feel designed, not eroded.
- Breaks are symbolic and surgical.

## RIMA Room Generation Pipeline

1. Select `ActFormProfile`.
2. Select `NarrativeBand`.
3. Select `RoomFamily`.
4. Select `Variant`.
5. Build semantic skeleton:
   - entrances/exits
   - combat core
   - landmark
   - dash lanes
   - wall masses
   - shell thickness by direction
6. Generate connected masks:
   - playable floor
   - collision walls
   - visual shell
   - foreground occluders
   - rift/collapse/chain/reliquary masks
7. Resolve tiles by adjacency:
   - RuleTile / Isometric RuleTile
   - Blob/Wang-style transition logic for edges
   - no isolated tile choices at borders
8. Place motif packages:
   - source anchored
   - field weighted
   - collision aware
9. Place lights:
   - socket based
   - color tied to motif and act profile
10. Validate:
   - walkable connectivity
   - enemy spawn validity
   - at least two dash lanes
   - door clearance
   - no combat-critical occlusion
   - camera sees floor plus shell, not raw void
   - tile count budget

## Practical Unity Direction

Use Unity Tilemap as renderer, but do not rely on manual scatter.

Recommended:
- Author room skeletons in code first, later LDtk/Tiled.
- Use RuleTile/Isometric RuleTile for wall/floor edge selection.
- Use separate tilemaps/layers:
  - Ground
  - FloorDetail
  - Walls
  - VisualShell
  - ForegroundOccluder
  - Decals
  - LightSockets/RuntimeLights
- Store per-cell semantic tags during generation, not just tiles.

Avoid:
- one global Random.Range prop scatter
- per-tile noise directly deciding final props
- WFC as full room generator
- rectangular room fill followed by random blockers

## Design Locks Proposed

1. Natural room detail must be connected to sources or fields.
2. Every act has a distinct form language.
3. Every act evolves internally from readable/orderly to deeper/more transformed.
4. Noise is only a modulation field, not the designer.
5. WFC/autotile/RuleTile are micro-resolution tools, not macro room directors.
6. RIMA's core room identity is "authored combat skeleton plus connected naturalization pass."

## Open Questions For Claude

1. Should Act 1 be named `Shattered Ruins`, `Sunken Keep`, or combine them as
   `Shattered Keep` for production language?
2. Should Act 2 lean more body-horror organic or more cursed battlefield?
3. Should Act 3 be mostly void-gold clean architecture, or retain more ruined-stone continuity?
4. Should LDtk/Tiled room authoring start before or after the code-side descriptor prototype?
