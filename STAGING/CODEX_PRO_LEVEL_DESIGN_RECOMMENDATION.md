# Codex Pro Level Design Recommendation

## 1. What is broken in v11

### Floor
- The room reads as one large tinted plane. The biome colors divide the canvas, but they do not create walkable terrain, paths, wear, elevation cues, or authored transitions.
- The ritual marks sit on top of the floor instead of being embedded into it. They feel like decals placed after the fact.
- There is not enough mid-scale texture: cracks, worn stone, dirt trails, scorch, moss creep, and edge shadow are too sparse or too uniform.

### Composition
- The strongest visual is the center sigil, but the wall row, two portals, banners, statues, and braziers compete for attention without a clear hierarchy.
- The negative space is mostly empty field, not purposeful combat/readability space. Empty space needs edges and directional pressure.
- The north wall row forms a flat decorative band. It frames nothing and makes the top feel like a prop lineup.

### Placement
- Zone props are arranged as isolated clusters. NW garden, SW pool, east rift, SE debris, and center columns do not share an authored route.
- Several props are equally spaced or mirrored in a way that exposes the algorithm: wall segments at fixed intervals, scatter clumps inside rectangular zones, and repeated pebbles/cracks.
- The portals are too far apart and both demand focal weight, so neither one reads as the designed objective.

### Visual flow
- The eye starts at the bright center mark, jumps to the east portal, then jumps to the southwest portal, then to the north wall. That is not a controlled path.
- The room lacks a readable entry-to-objective sequence. There should be a diagonal approach, a central decision/fight arena, and one dominant exit/objective.
- Dead zones exist in the far west, mid south, and top center because the floor does not carry the eye through them.

### Camera
- Ortho 11 is too wide for gameplay. It makes the room feel like an editor overview and shrinks the character. Recommended gameplay ortho: 7.25 to 8.0.
- Keep a separate overview screenshot camera setting only for QC captures. Gameplay should center the Warblade and preserve readable sprite scale.

## 2. Root causes

- The floor solution is a procedural color field, not a level-design surface. It lacks authored path geometry and edge treatment.
- Scatter placement is zone-based instead of composition-based. It fills named regions rather than supporting a focal triangle and player route.
- The scene uses too many equal-strength themes. A real room needs one dominant story, one secondary story, and tertiary dressing.
- Prop repetition is not being hidden by clustering, silhouette grouping, or occlusion. Similar assets appear as repeated stamps.

## 3. Concrete redesign plan

### New floor approach
- Replace the flat biome sprite with a generated 1152x704 PPU=32 floor named `PlayableRoom_DesignedFloor_v12.png`.
- Build an authored stone-and-earth floor: dark vignette edges, central worn plaza, diagonal southwest-to-center-to-east path, north threshold wear, moss creep from the west, rift scorch from the east.
- Add three detail layers directly into the floor texture: hairline cracks, dirt grit/noise, and soft trail highlights.
- Add prop decal overlays from existing v2/v3 sprites only: cracks, dirt, moss, pebbles, ritual marks. No Codex imagegen is required for this iteration because v2+v3 already cover the needed visual vocabulary.

### Zone restructure
- Reduce the room to four authored zones:
  1. West overgrown approach: statue, moss, small pool/rift remnant, low clutter hugging edges.
  2. Center ritual arena: clear combat oval, broken columns as left/right gates, central sigil, paired braziers as light/focal anchors.
  3. East objective breach: one dominant portal/rift, banners, broken wall teeth, debris funnel.
  4. North ruin backdrop: irregular wall silhouette with gaps and two broken pillar accents, not a flat row.
- Remove the equal-weight eight-zone layout. Keep the content, but re-parent it into intentional clusters.

### Placement rules
- Use a focal triangle: center ritual at (18,11), west statue/pool at (6.5,13.5), east breach at (30,10.5).
- Use a readable movement S-curve: southwest entry hint -> center arena -> east exit breach.
- Keep the arena interior mostly playable: hard clutter outside the oval, low decals inside.
- Use edge density: largest props near north/east/west boundaries, small debris fading toward the center.
- Break grid reads: wall segments use irregular x positions, varied y offsets, and mixed wall sprites.

### Asset gaps
- No new imagegen sheet for Phase A v12. Existing v2/v3 assets are sufficient.
- If v12 still feels too flat after screenshot QC, the fallback is one imagegen sheet: larger broken stone slabs, root-vein floor overlays, and cracked threshold pieces in `STAGING/RIMA_AssetParts_v4/`.

## 4. Risks and failure modes

- Procedural floor may still look too painterly or too noisy if contrast is wrong.
- Reusing existing props may reveal repeated silhouettes if grouping is not disciplined.
- Camera may pass screenshot QC at overview while gameplay ortho feels too close; both settings must be documented.
- Without collision authored for every prop, visual map quality can improve while gameplay blockers remain approximate.
