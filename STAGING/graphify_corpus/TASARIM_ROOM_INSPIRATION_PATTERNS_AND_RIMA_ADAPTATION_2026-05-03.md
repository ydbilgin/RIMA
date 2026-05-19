---
status: REFERENCE
faz: 1
tarih: 2026-05-03
ozet: "Oda inspiration pattern adaptasyon notları"
---
# RIMA Room Inspiration Patterns and Adaptation - 2026-05-03

Status: DESIGN INPUT FOR CLAUDE REVIEW
Scope: Hades-like room inspiration, adjacent roguelite lessons, RIMA-specific adaptation

## Source Basis

This is not a copy plan. It identifies useful patterns from adjacent games and converts them into
RIMA-specific room rules.

References checked or used as design anchors:
- Hades: chamber-based combat, reward doors, compact authored arena reads.
  - https://hades.fandom.com/wiki/Chambers_and_Encounters
- Hades II: region identity, encounter rooms, reward/destination selection.
  - https://hades2.wiki.gg/wiki/Locations
- Dead Cells: biome identity, procedural level structure, exits, shops, treasure rooms.
  - https://deadcells.wiki.gg/wiki/Biomes
- Curse of the Dead Gods: temple-room progression, traps, curses, room reward route decisions.
  - https://curseofthedeadgods.fandom.com/wiki/Rooms

## Borrowed Patterns

### Hades Chamber Grammar

Useful:
- One room is one readable combat proposition.
- Reward doors make post-room choice immediate and legible.
- Room variants feel authored even when reused.
- Combat floor is clean; identity lives in walls, backdrop, props, and lighting.
- Entrances and exits are staged as narrative thresholds.

RIMA adaptation:
- Reward doors become skill offer / map fragment / forge / spirit / curse route decisions.
- Room walls and shells tell the Fracturing story.
- Combat arenas stay clean; visual complexity moves into non-playable shell, occluders,
  landmark zones, and light sockets.

### Hades II Regional Evolution

Useful:
- Each region changes silhouette, hazards, props, and pacing, not only palette.
- Later regions can become stranger without becoming unreadable.

RIMA adaptation:
- Act 1: controlled ruin broken by Fracturing.
- Act 2: architecture consumed by a living wound.
- Act 3: void-gold impossible architecture.
- Final: mirror-like Nexus spaces.
- Within each act, first rooms teach and late rooms distort.

### Dead Cells Biome Rules

Useful:
- Biomes are gameplay packages, not only art palettes.
- Enemy pools, route choices, hazards, treasure/shop placement, and level silhouette all change by
  biome.

RIMA adaptation:
- `ActFormProfile` controls allowed room families, motif packages, shell thickness, enemy bands,
  light color/density, hazards, and room size ranges.
- This prevents "same room, different color" syndrome.

### Curse of the Dead Gods Consequence Rooms

Useful:
- Traps and hazards belong to temple identity.
- Risk/reward rooms change path decisions.
- Hazards are systemic, not decorative.

RIMA adaptation:
- Curse Gate, Spirit, Forge, Elite, and Unknown rooms get distinct spatial grammar.
- Hazards come from connected sources:
  - chain trap from chain anchors
  - rift hazard from rift tear
  - prison ambush from cell sockets
  - relic shield field from reliquary anchor
- Darkness should wait until RIMA has an explicit torch/vision mechanic.

## RIMA Synthesis

RIMA's room identity:

```text
authored combat skeleton
+ connected naturalization pass
+ act-specific form language
+ non-playable visual shell
```

Every room should answer:

```text
What was this place before the Fracturing?
What broke it?
What combat question does it ask?
What source creates the visible detail?
What stays clean for readability?
```

Production principle:

```text
Do not make random maps look natural.
Define room role, source damage, and connected motif graph.
Then let masks, adjacency, and influence fields resolve the tiles.
```


