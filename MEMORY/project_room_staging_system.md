---
topic: project_room_staging_system
updated: 2026-05-04
---

# Project Room Staging System

Use when: map staging, Hades-like room edges, black camera edge, room variants, authored masks,
visual shell, dungeon composition.

Locked decision:
- Do not hide camera edges by flooding the world with large extra playable floor padding.
- RIMA rooms use `PlayableFloorMask + non-playable VisualShellMask + authored perimeter
  walls/occluders + camera clamp`.
- `cameraSafetyFloorPadding` stays small: default `0`, hard cap `16`.
- Movement/collision/spawns use playable floor only.
- Camera safety can include playable floor plus visual shell.
- Visual shell is built from walls, broken caps, rubble, chasm/void backdrop, pillars, chains,
  arches, and light anchors. It is not walkable floor.

Canonical decision doc:
- `TASARIM/ROOM_STAGING_AND_MAP_VARIANTS_DECISION_2026-05-03.md`

Connected generation / act evolution proposal for Claude final review:
- `TASARIM/ROOM_CONNECTED_GENERATION_AND_ACT_EVOLUTION_PROPOSAL_2026-05-03.md`

PixelLab environment module notes pending Claude:
- `TASARIM/PIXELLAB_ENVIRONMENT_MODULE_NOTES_PENDING_CLAUDE_2026-05-03.md`

Story position:
- Act 1 reads as Shattered Ruins / Sunken Keep: an old controlled structure broken by the
  Fracturing, still trying to preserve order.
- Room variants should range from built/orderly to collapsed to rift-torn, but each combat question
  stays authored and readable.

Connected generation rule:
- Do not scatter detail independently.
- Build a semantic skeleton first, then generate connected masks and influence fields.
- Props/details should belong to a source: breach, rift tear, chain anchor, shrine, relic, wall base,
  door threshold, or landmark.

Door and route rule:
- `DoorEast`, `DoorWest`, `DoorNorth`, and `DoorSouth` are temporary implementation anchors, not
  the final design contract.
- Final rooms use blueprint-defined `GateSockets`.
- Gate count can be 1, 2, 3, or more depending on route and room design.
- Gates are visible in-world thresholds: arches, breaches, stairs, chained doors, rift thresholds,
  lifts, bridge mouths, or shrine passages.
- Gate visuals must match route promise and room logic; do not place doors only because a cardinal
  direction exists.
- Dungeon route should branch like an effective STS/Hades hybrid, but the player should not see the
  full map by default.
- Map fragments reveal only the next 1 or 2 nodes unless a special rule reveals more.
- Randomization selects from prepared room pools by act band, route depth, room type, and story
  pressure.

Next implementation direction:
- Add data-driven room descriptors with masks and metadata.
- Replace fixed cardinal door assumptions with data-driven gate sockets.
- Generate visual shell by dilation around playable floor.
- Clamp camera to playable floor plus shell.
- Keep enemy spawn validation out of wall/shell/void cells.

PixelLab size note:
- Do not lock `32px` or `32x64` yet.
- First floor test recommendation is Create tiles PRO, Isometric, `64px`, view angle about 45,
  thickness 0%.
- Accept by measured output, not UI label: visible footprint near `64x32`, top surface only,
  no side face or raised slab.
