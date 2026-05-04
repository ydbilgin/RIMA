---
topic: project_room_blueprints
updated: 2026-05-03
---

# Project Room Blueprints

Use when: room catalogue, map blueprint, Hades-like inspiration, Act 1 room design, room families,
combat question, dungeon layout design.

Current design inputs:
- `TASARIM/ROOM_INSPIRATION_PATTERNS_AND_RIMA_ADAPTATION_2026-05-03.md`
- `TASARIM/ACT1_SHATTERED_KEEP_ROOM_BLUEPRINT_CATALOGUE_2026-05-03.md`
- `TASARIM/ACT1_SHATTERED_KEEP_ROOM_BLOCKOUT_SET_2026-05-04.md`
- `TASARIM/ROOM_CONCEPTS/act1_room_blockout_sheet_2026-05-04.svg`

Key rules:
- RIMA borrows chamber grammar, biome identity, route decisions, and readability patterns from
  Hades-like roguelites, but not their setting or exact room shapes.
- Every room records a `CombatQuestion`.
- Detail comes from connected motif sources, not random scatter.
- Act 1 blueprint set has 30 room drafts across Threshold, Guard/Prison, Chain Gallery,
  Ossuary/Reliquary, Ritual/Rift, and Boss approach bands.

Next recommended step:
- Claude reviews the 30-room catalogue.
- Pick 8-10 first blockout rooms for Unity placeholder implementation.
- Keep PixelLab final modules pending until room grammar and floor-size test decision are approved.

Current draft selection:
- First blockout set picks 10 Act 1 rooms: Broken Entry Gate, Ordered Guard Hall, Cell Spine,
  Broken Causeway, Cross-Chain Clamp, Sunken Crypt Basin, Reliquary Loop, Shrine Crossroad,
  Rift Well Edge Tear, Penitent Containment Arena.
- New candidate mask: `DashTraverseGap`, walk-blocked but dash-crossable if later implemented.
- Asset pack baseline follows STYLE_BIBLE: floor `128x64`, wall/module `128x192`.
