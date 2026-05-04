# RIMA Room Staging and Map Variant Decision - 2026-05-03

Status: LOCKED
Owner: design/orchestration
Scope: Act 1 room composition, camera edge safety, Hades-like room variation

## Core Decision

RIMA must not hide black camera edges by flooding the world with extra playable floor tiles.

The locked solution is:

```text
playable room floor + non-playable visual shell + authored perimeter walls/occluders + camera clamp
```

Playable tiles exist only inside the real combat room. The area around the room is a staged
architectural shell, not a second floor field.

## Why

The previous brute-force idea of a very large floor apron is rejected because it can create huge
tile counts, weak room identity, and crash risk. It also makes the room feel like a generated
infinite tile plane instead of a composed Hades-like arena.

Hades-like staging works because the player reads a combat arena surrounded by thick architecture,
foreground masses, off-room darkness, props, height, and camera framing. The outside of the room is
not fully playable. It is visual depth.

## Required Room Masks

Every authored or procedural-authored RIMA room should be described by these masks:

```text
PlayableFloorMask
WallCollisionMask
VisualShellMask
ForegroundOccluderMask
VoidBackdropMask
DoorSockets
EnemySpawnZones
LightSockets
PropZones
LandmarkSlots
```

Rules:
- `PlayableFloorMask` is the only walkable floor source.
- `VisualShellMask` is non-playable staging around the room.
- `ForegroundOccluderMask` is allowed to overlap the lower/front read of the room, but must not
  hide combat-critical enemies, hazards, projectiles, or the player for long.
- `VoidBackdropMask` is the dark outside world. It can be visible at room edges, but normal combat
  camera framing should not show raw black emptiness around the player.
- Camera bounds should use `PlayableFloorMask + VisualShellMask`, while movement/collision uses
  `PlayableFloorMask + WallCollisionMask`.

## Door And Route Model

Current `DoorEast`, `DoorWest`, `DoorNorth`, and `DoorSouth` objects are implementation
placeholders, not the final room contract.

The final room contract is blueprint-defined `GateSockets`:
- A room can have 1, 2, 3, or more exits when the route design needs it.
- Gate count is decided by the dungeon graph and room blueprint, not by fixed cardinal child
  objects.
- A gate socket records position, facing/read direction, target node, room reward/type icon, lock
  state, reveal state, and visual frame variant.
- Cardinal names can remain as temporary editor anchors, but gameplay should read data-driven
  gate sockets.

Route structure should borrow the effective parts of Hades and Slay the Spire style maps:
- The full map is not visible by default.
- The route branches forward through authored/randomized node pools.
- Map fragments reveal only the next 1 or 2 nodes by default.
- Some rewards or special events can reveal more, but full-route reveal should be rare.
- Rooms spawn from allowed blueprint pools by act band, route depth, room type, and story pressure.
- Randomization chooses from prepared room families; it should not freely invent arbitrary room
  layouts that break combat readability.

Gate visual rule:
- Gates are real in-world thresholds, not UI-only arrows.
- A gate should read as a believable exit in the current room shell: arch, breach, stair, chained
  doorway, rift threshold, lift, bridge mouth, or shrine passage.
- Gate art must match target room promise through icon/light/frame language.
- Locked/unrevealed gates use fog, dim icon, seal, or broken frame, not a fake missing doorway.
- Gate sockets should sit on authored shell edges or meaningful interior thresholds; do not place
  exits only because a cardinal direction exists.
- Gate concepts should be built as reusable neutral templates, then style-filled with inpainting
  and animated with first/end frame interpolation where possible.

## Hard Limits

- Do not use giant floor padding to hide camera edges.
- `cameraSafetyFloorPadding` must stay small. Current lock: default `0`, hard cap `16`.
- Do not generate whole final rooms as one baked image.
- Do not make the whole room a plain rectangle and decorate it afterward.
- Do not place large center blockers in boss/elite rooms.
- Every combat room keeps at least two clean dash lanes.

## Shell Thickness Guidance

Use shell thickness as composition, not padding:

| Area | Typical Thickness | Purpose |
|---|---:|---|
| North/back wall | 8-12 tiles | heavy architecture, depth, banners, rift cracks |
| East/west sides | 6-10 tiles | broken wall runs, side pockets, door frame mass |
| South/front edge | 4-8 tiles | foreground occluders, parapet, broken stone lip |
| Boss/elite perimeter | 10-14 tiles | ceremonial scale, camera-safe staging |

The shell is sparse and structural. It uses wall modules, broken caps, rubble, chasm/void tiles,
pillars, chains, arches, and light sources. It is not filled with walkable floor.

## Story Position

Use GDD lore only as lore reference, because GDD is marked stale for S43 mechanics.

Act 1 should read as:

```text
Shattered Ruins / Sunken Keep:
an old controlled structure, broken by the Fracturing, still trying to preserve order
```

RIMA's story says the Fracturing was not only a disaster; it was a deliberate severing. The player
is both remnant and participant. Therefore the dungeon should not feel like random caves only.
It should feel like a remembered structure whose logic is still visible, but whose edges are torn.

The room language should move through four bands:

1. Threshold / Entry ruin
   - readable, built, tutorial-friendly
   - broken but not chaotic
2. Prison / Guard / Chain architecture
   - controlled, symmetrical pressure, corridors, cells, chained walls
3. Ossuary / Reliquary / Memory crypt
   - looped paths, side pockets, relic anchors, dead order
4. Ritual / Rift / Boss approach
   - geometry becomes less stable, but combat core stays readable

## Variant Philosophy

Each room archetype must support multiple variants. Some variants should look deliberately built,
some should look collapsed, and some should look almost random. The important rule is that the
combat question stays authored.

RIMA variants use three axes:

```text
StructureBias: Built / Broken / Rift-Torn / Natural-Collapsed
Symmetry: ordered / offset / chaotic
CombatQuestion: flank / kite / dash lane / priority target / swarm / elite pressure
```

A room can look messy, but it must not play messy.

## Act 1 Room Families

### 1. Broken Entry Hall

Story read: first threshold into the shattered keep.

Variants:
- Built Gate: clear rectangular hall, thick back wall, two broken columns.
- Breached Gate: one side wall collapsed into visual shell rubble, floor still readable.
- Cold Threshold: large north wall mass with cyan rift cracks and a narrow entry neck.

Combat question: low pressure, teach movement and first target priority.

### 2. Guard Hall

Story read: old defensive architecture.

Variants:
- Ordered Guard Room: strong symmetry, two L-cover pieces, built wall mass.
- Split Barracks: offset side rooms and broken furniture lanes.
- Rusted Watch: side pockets with Chain Warden style spawn bands.

Combat question: kite around cover without losing dash lanes.

### 3. Chain Gallery

Story read: prison/discipline corridor that foreshadows Penitent Sovereign.

Variants:
- Long Chain Hall: east-west lane, thick upper wall with hanging chain props.
- Broken Causeway: central path with chasm/void shell outside, not playable floor.
- Cross-Chain Gallery: two diagonal lanes, side alcoves, no center blocker.

Combat question: ranged pressure plus mobility punishment.

### 4. Prison Block

Story read: cells, cages, controlled bodies, old keep cruelty.

Variants:
- Cell Spine: center corridor, small side cells as non-critical cover.
- Opened Cells: cells collapsed into spawn pockets.
- Silent Lockup: more negative space, fewer walls, heavier foreground occluders.

Combat question: flanks emerge from sides while player keeps center route.

### 5. Crypt Basin

Story read: memory and death, the dungeon begins turning inward.

Variants:
- Sunken Basin: oval-ish combat core, thick surrounding crypt shell.
- Ossuary Steps: offset ribs of wall/short tombs, readable lanes.
- Bone Well: landmark pit in shell or edge, never blocking the combat center.

Combat question: circular movement, swarm or split enemies.

### 6. Reliquary Loop

Story read: sealed objects and old protection rituals.

Variants:
- Inner Loop: ring-like path around a relic landmark, center still readable.
- Broken Reliquary: one side of loop collapsed, alternate dash lane remains.
- Reliquary Cross: four side sockets, controlled spawn bands.

Combat question: priority target and shield/support enemy pressure.

### 7. Shrine Crossroad

Story read: player is reaching deliberate ritual architecture.

Variants:
- True Cross: built cross shape, all exits readable.
- Offset Shrine: shrine shifted off-center, asymmetric but composed.
- Cracked Cross: rift cuts one arm; visual shell fills the missing mass.

Combat question: choose target route while enemies pressure from two arms.

### 8. Crescent Sanctum

Story read: sacred geometry damaged by Fracturing.

Variants:
- Crescent Floor: curved combat floor, thick outer wall arc.
- Twin Crescent: two offset arcs with a wide dash lane between.
- Broken Moon: crescent interrupted by rift scars and shell rubble.

Combat question: enemies try to pull the player around the arc.

### 9. Rift Well

Story read: the structure can no longer fully contain the wound.

Variants:
- Center Well: center hazard/landmark with clear ring lane.
- Edge Tear: rift mass on one edge; combat core shifted away.
- Three Tears: three small non-playable rift sockets, no center clogging.

Combat question: area control and hazard awareness.

### 10. Boss Antechamber

Story read: final controlled space before the Penitent Sovereign.

Variants:
- Sealed Walk: wide ceremonial lane, huge north wall, chained boss door.
- Broken Procession: same lane, but side shell rubble implies collapse.
- Empty Court: very sparse center, heavy perimeter scale.

Combat question: preparation, readable escalation, minimal clutter.

## Boss Arena Direction

The Penitent Sovereign arena should be the most deliberate room in Act 1:

- circular or octagonal ritual platform
- thick perimeter shell
- large chain anchors outside playable space
- visible north/back architectural mass
- two or more clean dash lanes
- center can host Rift Tear only as boss mechanic, not as static clutter

The arena should feel built for containment. The fight breaks that containment.

## Implementation Direction

Immediate implementation should not add more raw random templates. Next code/design pass should:

1. Split room generation into data-driven template descriptors.
2. Add shell generation by mask dilation around playable floor.
3. Paint visual shell with non-playable wall/rubble/void modules.
4. Clamp camera against `PlayableFloorMask + VisualShellMask`.
5. Keep player navigation and enemy spawn validation tied to `PlayableFloorMask`.
6. Add per-room `StructureBias`, `Symmetry`, `ShellThickness`, `LandmarkSlot`, and
   `CombatQuestion` fields.

Suggested room descriptor:

```text
RoomId
NarrativeBand
RoomType
StructureBias
Symmetry
PlayableFloorMask
WallCollisionMask
VisualShellMask
ForegroundOccluderMask
VoidBackdropMask
DoorSockets
EnemySpawnZones
LightSockets
PropZones
LandmarkSlots
CombatQuestion
```

## Production Art Direction

PixelLab/MCP should generate reusable modules:

- flat 64x32 floor top variants
- 64x96 wall modules with vertical faces and top caps
- outer/inner corners
- broken wall caps
- doorway/arch pieces
- chain anchors
- rubble piles
- cages
- relic plinths
- rift cracks
- crypt/ossuary props
- torch and cold magic light fixtures

Do not ask PixelLab for final whole playable maps except as concept references.

## Final Lock

RIMA room maps are authored combat arenas inside a staged, non-playable architectural shell.

The player should feel that each room is a different remembered part of a broken keep: sometimes
ordered, sometimes collapsed, sometimes rift-torn. The visual shell and camera clamp solve edge
safety. Playable floor flood is rejected.
