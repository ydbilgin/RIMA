# Codex Architecture Review

## Verdict
Choice: C
Confidence: high
Rationale (3 bullets, code-perspective):
- It fits RimaWorldPainterWindow: Rooms, prefabs, props, mobs, collision rules, grouped placement, and JSON save/load.
- It avoids A's biggest cost: broad wall/floor/decor connection grammar.
- It avoids B's repaint loop for collision, spawns, doors, and combat lanes.

## Per-option analysis
### A) Modular
1. Unity integration complexity: medium-high, about 500-900 LOC beyond current code. The painter already has floor tiles, wall prefabs, props, mobs, auto-connect walls, sorting, collision, and saves, but production modularity needs strict naming, validation, and connection coverage.
2. Compatibility with existing RimaWorldPainterWindow: strongest fit. Extend it.
3. Procgen architecture: tile/prefab grammar. Room selection is layout seed plus tile/wall/decor rules; encounters use mob prefabs or spawn markers.
4. Performance: acceptable for small rooms, but sprites, colliders, and draw calls rise with decor density.
5. Implementation time estimate: 4-7 days.
6. Iteration friendliness: good after rules exist; slow when adding asset families because scale, collision, sorting, and connections must be checked.

### B) Full rooms
1. Unity integration complexity: low, about 150-300 LOC for import metadata, room prefab creation, PolygonCollider2D, and spawn transforms.
2. Compatibility with existing RimaWorldPainterWindow: weak. It mostly bypasses the painter; Rooms can catalog entries, but painting becomes secondary.
3. Procgen architecture: room catalog picker with authored spawn transforms. Variation requires many paintings.
4. Performance: best sprite count. Risks are texture memory and overly detailed polygon colliders.
5. Implementation time estimate: 1-2 days.
6. Iteration friendliness: poor. Door, obstacle, lane, and collision edits need repainting or metadata rework.

### C) Hybrid
1. Unity integration complexity: medium-low, about 300-550 LOC. Add template prefab/manifest conventions, socket validation, and decor randomization while keeping the painter workflow.
2. Compatibility with existing RimaWorldPainterWindow: strong. Use Rooms for templates, Prop/Mob for overlays, and current collision/sorting for blockers.
3. Procgen architecture: choose one of 5-8 base templates, then fill authored sockets with encounters, doors, blockers, rift decals, and decor.
4. Performance: favorable. One base sprite or limited chunks plus controlled overlays; colliders stay coarse except blockers.
5. Implementation time estimate: 2-4 days.
6. Iteration friendliness: best balance. New rooms are templates plus sockets; new decor drops into existing prefab categories.

## Implementation outline (chosen)
1. Define RoomTemplate prefab/manifest rules: base sprite, bounds collider, door sockets, encounter sockets, decor sockets, and camera/combat bounds.
2. Extend Rooms to instantiate templates into the painter scene without changing manual prop/wall/mob placement.
3. Add validators for sockets, references, collider overlaps, sorting/layers, and pixels-per-unit.
4. Add runtime selection by biome/tags, then fill sockets from small weighted tables.
5. Keep walls, doors, torches, banners, cracks, and blockers as overlay prefabs using current collision rules.

## Code-perspective risks
- Template metadata can drift from art unless validation is mandatory.
- Large sprites need consistent Pixel Perfect Camera import settings.
- Decor sockets can block lanes or overlap colliders.
- Auto-connect wall logic should not be forced onto baked bases.
- Too much baked detail turns C into B.
