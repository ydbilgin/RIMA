# PixelLab Map Workshop -- RIMA Isometric Usage Note
Date: 2026-05-06
Status: Draft for Claude review

Source:
- YouTube: https://youtu.be/O9maOTbLuHQ
- Full external review: `STAGING/PIXELLAB_MOVEMENT_SHEET_AND_MAP_WORKSHOP_REVIEW_2026-05-05.md`

## Verdict

PixelLab Map Workshop is useful for RIMA isometric production, but only as a prototype/reference
layer. It should not become the final room-authoring tool.

Use it for:
- tile transition exploration
- visual blockouts
- prop context generation
- player scale/readability tests
- palette/style reference generation

Do not use it for:
- final gameplay collision
- final gate sockets
- final minimap topology
- spawn layout
- route reveal logic
- interactable/destructible object authority

## Best Uses For RIMA

### 1. Act 1 Terrain Transition Tests

Use the lower/higher terrain workflow to quickly test transitions:

- cracked stone floor -> cyan rift water
- worn flagstone -> rubble
- ash-stained floor -> blood-marked floor
- dungeon stone -> broken bridge edge
- shallow rift fissure -> walkable stone lip

Output is a visual candidate only. Rebuild accepted patterns in Unity using the locked room pipeline.

### 2. Style Reference Tiles

Generate a small visual set, then select the best tile as a style reference for:

- PixelLab Create Image Pro
- Tiles Pro
- hand cleanup in Pixelorama/Aseprite
- Unity RuleTile/RandomTile texture targets

Do not ask PixelLab for a full finished dungeon room in one prompt.

### 3. Static Inpaint For Baked Set Dressing

Use Inpaint Map for things that can be baked into floor/wall art:

- cracked stone crossing over a rift fissure
- broken floor lip
- moss/rubble edge
- scorch mark
- ritual stain
- non-interactable collapsed debris

Avoid inpaint for anything the player should collide with, destroy, target, loot, or trigger.

### 4. Create Object For Movable Props

Use Create Object / map object generation for separate props:

- broken shrine fragment
- torch stand
- chest shell
- cracked pillar
- gate marker
- loose rubble pile
- trap object shell

These should remain separate sprites so Unity can place, sort, collide, destroy, or animate them.

### 5. Player Scale / Readability Check

Drag or composite a RIMA character sprite onto the generated map concept and check:

- player contrast against floor
- silhouette read at combat zoom
- enemy/projectile readability
- cyan rift accent not overpowering skill VFX
- navigable floor vs decoration clarity

If the player does not read clearly, reject the tile/prop direction before production import.

## Required RIMA Test Output

Every Map Workshop experiment should produce exactly three review artifacts:

1. Terrain transition sheet.
2. Movable object/prop sheet.
3. Unity or mock screenshot with player scale overlay.

Anything beyond these three artifacts is likely overproduction for Phase 1.

## Prompt Patterns

Terrain transition:

```text
Lower terrain: dark cyan rift water visible through cracked stone.
Higher terrain: worn grey castle flagstone floor.
Transition: broken uneven stone lip with subtle cyan glow leaking between slabs.
Compact 2D isometric ARPG dungeon tile, readable at 64px, muted grey palette, cyan accent only.
```

Static inpaint:

```text
collapsed narrow stone crossing over a cyan rift fissure, broken slabs aligned as a walkable bridge, subtle cyan glow from below
```

Movable prop:

```text
broken stone shrine fragment with cyan rift cracks, compact low top-down dungeon prop, readable silhouette, muted grey stone, transparent background
```

## Production Rule

Map Workshop can suggest visual language. Unity remains the source of truth for playable rooms.

Accepted Map Workshop outputs must be translated into:
- authored room skeleton
- socket-compatible layout
- collision map
- sorting layers
- minimap-safe topology
- spawn-safe combat space

