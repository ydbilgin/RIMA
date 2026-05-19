---
status: REFERENCE
faz: 1
tarih: 2026-05-04
ozet: "Act 1 oda blockout seti"
---
# Act 1 Shattered Keep Room Blockout Set - 2026-05-04

Status: production design draft
Audience: AI agents first; compact implementation-facing spec

Visual mockup:
- `TASARIM/ROOM_CONCEPTS/act1_room_blockout_sheet_2026-05-04.svg`

Source docs:
- `TASARIM/ROOM_STAGING_AND_MAP_VARIANTS_DECISION_2026-05-03.md`
- `TASARIM/ACT1_SHATTERED_KEEP_ROOM_BLUEPRINT_CATALOGUE_2026-05-03.md`
- `TASARIM/ROOM_INSPIRATION_PATTERNS_AND_RIMA_ADAPTATION_2026-05-03.md`
- `TASARIM/STYLE_BIBLE.md`

## Blockout Rules

Canonical asset scale:
- floor tile: `128x64` isometric diamond
- wall tile/module: `128x192`
- PPU: 128
- camera: 35 degree ARPG / low top-down

Room masks:
- `PlayableFloorMask`: walkable combat floor only
- `WallCollisionMask`: hard blockers
- `VisualShellMask`: non-playable visual staging
- `ForegroundOccluderMask`: low/front occluders, never combat-critical for long
- `VoidBackdropMask`: off-room darkness/chasm/water depth
- `GateSockets`: data-driven exits
- `DashTraverseGap`: candidate traversal zones, walk-blocked but dash-crossable

Important:
- `DashTraverseGap` is a design contract candidate, not implemented code yet.
- If implemented, walking is blocked but dash trajectory can cross from valid floor to valid floor.
- Gaps must be short and readable, typically 1-2 floor tiles wide.
- No enemy spawn, pickup, or critical projectile telegraph should sit inside the gap.

## First 10 Room Set

### R01 Broken Entry Gate

Source: A01.

Purpose:
- first readable combat room
- teach movement, first target priority, and south-to-north flow

Layout:
- wide south entry neck
- broad octagonal center
- thick north wall
- two side shell chunks, no center blockers

Motifs:
- broken gate stones fan inward from the south threshold
- low cyan cracks near north seal
- calm torch sockets

Gates:
- south entrance
- north or northeast exit after reward

Assets needed:
- flat floor base
- gate rubble fan
- north wall straight
- torch socket
- small crack decal

### R02 Ordered Guard Hall

Source: A05.

Purpose:
- kite around light cover without losing dash lanes

Layout:
- rectangular/soft-octagonal arena
- two small L-cover blockers near left/right side bands
- center lane remains clear

Motifs:
- guard posts align to wall bases
- controlled masonry, low noise

Gates:
- 2 exits possible: north combat/elite, east chest/forge

Assets needed:
- L-cover wall cap
- guard post
- wall base shadow
- metal torch bracket

### R03 Cell Spine

Source: A08.

Purpose:
- flank pressure from side cells while center lane stays readable

Layout:
- long center floor spine
- shallow side cell pockets are mostly shell
- two broken-open cell gaps can spawn enemies

Motifs:
- cage bars and chain rows repeat along shell
- scratch marks radiate from opened cells

Gates:
- north main gate
- optional west broken cell breach as unknown/event

Assets needed:
- cell bar module
- broken bar module
- chain segment
- cell shadow patch

### R04 Broken Causeway

Source: A12.

Purpose:
- dash lane discipline and edge pressure

Layout:
- long central causeway
- side void/water shell
- 1-2 short `DashTraverseGap` cuts across broken stone

Motifs:
- collapsed side lips
- dark water/chasm below
- loose bridge stones marking dashable crossings

Gates:
- north bridge mouth
- optional side stair only after clear

Assets needed:
- chasm lip
- broken bridge cap
- dash-gap marker stone
- shallow water strip
- falling rubble decal

### R05 Cross-Chain Clamp

Source: A13.

Purpose:
- elite pressure; punish panic dash while keeping counter windows

Layout:
- square/octagonal center
- four chain anchors outside playable floor
- diagonal chain lines as visual/decal pressure, not blockers

Motifs:
- chain anchors pull visual lines inward
- cold center seal

Gates:
- 1-3 exits depending route promise

Assets needed:
- chain anchor large
- diagonal chain decal
- elite seal floor decal
- heavy back wall

### R06 Sunken Crypt Basin

Source: A14.

Purpose:
- circular movement and swarm control

Layout:
- oval basin-like floor
- clean ring lane
- small shallow puddles on edge cells only

Motifs:
- water collects in low basin cuts
- bone/dust clusters collect at wall bases
- center remains mostly dry/readable

Gates:
- north crypt arch
- east/west optional reliquary path

Assets needed:
- shallow puddle decal small/medium
- wet edge tile
- bone dust cluster
- basin wall curve

### R07 Reliquary Loop

Source: A17.

Purpose:
- priority target and support enemy pressure

Layout:
- loop path around a relic plinth
- two clean dash cuts through loop
- plinth can be non-blocking visual or edge-shifted blocker

Motifs:
- candle/relic light rings
- organized shelves
- few gold accents

Gates:
- chest/reward path common
- combat path optional

Assets needed:
- reliquary plinth
- candle cluster
- relic shelf wall
- gold dust decal

### R08 Shrine Crossroad

Source: A20.

Purpose:
- two-arm pressure and route choice

Layout:
- cross-shaped floor
- shrine north of center, not blocking the central lane
- all arms keep gate clearance

Motifs:
- cold shrine light
- ritual cracks point to gate sockets
- torch/cyan sconce mix

Gates:
- 2-3 visible exits after clear
- one fogged step-2 promise can remain dim

Assets needed:
- shrine base
- ritual line decal
- cold sconce
- gate socket base

### R09 Rift Well Edge Tear

Source: A25.

Purpose:
- area control near a dangerous side source

Layout:
- combat core shifted away from one side
- rift tear stays in VisualShell or edge overlap
- no center clog

Motifs:
- all cracks radiate from the side tear
- cyan light with red danger only if hazardous

Gates:
- one gate near safe side
- one rift threshold can become curse/unknown

Assets needed:
- rift tear shell
- crack fan decal
- floating shard cluster
- cold glow overlay

### R10 Penitent Containment Arena

Source: A30.

Purpose:
- Act 1 boss room; containment breaks during fight

Layout:
- circular/octagonal platform
- thick perimeter shell
- huge north/back wall
- chain anchors outside playable space
- two or more clean dash lanes

Motifs:
- built for containment, not random ruins
- center rift appears by boss phase, not static clutter
- back wall carries scale

Gates:
- south entry
- north boss lock / post-boss exit

Assets needed:
- ritual platform edge
- massive chain anchor
- boss seal door
- back wall monument
- phase rift overlay

## Dash-Only Space Rules

Allowed forms:
- broken bridge gap
- cracked floor seam
- shallow water channel if dash splashes across
- narrow chasm cut
- collapsed stair skip

Not allowed:
- long platforming gaps
- ambiguous floor height
- gaps under enemies or pickups
- gaps that punish normal combat camera visibility
- gaps that look walkable but are not

Implementation requirement later:
- draw `DashTraverseGap` separately from `VoidBackdropMask`
- dash-cross validation must check start floor, end floor, gap length, and landing clearance
- if code support is not ready, render gaps as visual shell only

## Act 1 Asset Pack Request

### Floor

Production target:
- `128x64` flat top-surface floor tile

Set:
- 12 base floor variations
- 8 cracked/worn variations
- 6 wet edge/puddle-adjacent variations
- 6 rift hairline variations

Reject:
- raised slab
- thick side face
- chunky boardgame isometric block
- over-bright cracks on every tile

### Decals

Native:
- `32x32`, `64x64`, or transparent cropped PNGs

Set:
- crack hairlines
- dirt stains
- chips
- puddle highlights
- bone dust
- small rubble
- ritual lines
- chain scrape marks

### Walls And Shell

Native:
- `128x192` wall modules

Set:
- straight wall
- inner corner
- outer corner
- broken cap
- low foreground parapet
- arch/gate frame
- cell wall/bars
- reliquary shelf wall
- boss back wall segment

### Water / Chasm / Height

Use as non-playable or dash-only accents:
- shallow puddle decal
- wet floor edge
- chasm lip
- dark water strip
- broken bridge cap
- ledge shadow
- rubble mound

### Gates

Native:
- 1.5x-2x character height at gameplay zoom

Set:
- neutral gate socket
- combat gate
- elite gate
- chest gate
- forge gate
- unknown/event gate
- curse/rift gate
- boss gate
- unrevealed fog overlay

## Production Sequence

1. Generate 4-6 style reference tiles and decals.
2. A/B test:
   - `128x64` floor target from PixelLab tile workflow.
   - `64x64/16` style-reference variations as source/style mining.
3. Build a 4x4 flat floor mockup.
4. Add wall/shell edge modules.
5. Test R04 Broken Causeway and R06 Sunken Crypt Basin first because they stress gaps/water.
6. Import only PASS assets into Unity prototype folders.


