# chatgpt_ref Object Inventory — rima-sonnet — 2026-05-25

## Per-image counts

### Image 1 (combat arena, rectangular room, ~12x10 cells)
- Wall-mounted torches: 6 visible — bracket-style, amber flame, placed at pillar faces and wall mid-points
- Floor torches / braziers: 2 — freestanding, bottom-center and bottom-left corner
- Stone pillars: 8 — massive, 1.5x4 cells, all four corners + mid-wall intervals
- Glowing archway (portal): 1 — north wall center, cyan energy fill, arched stone frame, ~3x4 cells
- Cyan floor rune/circuit lines: present across entire floor, thin line network, not discrete objects but a floor layer pattern
- Cyan energy orbs (ground): 4 scattered mid-floor, ~0.5x0.5 cells each
- Characters (enemies + player): ~9, excluded from prop inventory

### Image 2 (circular-ish boss arena, ~14x12 cells)
- Freestanding braziers / floor torches: 6 — perimeter placement, amber flame, ~0.5x1.5 cells
- Stone pillars: 8 — perimeter, matching Image 1 style, 1.5x4 cells
- Glowing archway (portal): 1 — north wall, same cyan arch type, ~3x4 cells
- Cyan concentric floor rune circle: 1 large — center of room, 6x6 cell diameter, circuit-style engraved pattern, this is a FLOOR DECAL not a raised object
- Cyan energy orbs (ground): 3 scattered near center
- Ruined/seated stone figure (gargoyle/statue): 1 — lower-left area, ~1x1.5 cells, crouching silhouette
- Characters: ~5, excluded

### Image 3 (flooded/wet dungeon corridor room, irregular shape, ~14x10 cells)
- Wall-mounted torches: 3 — on pillar faces, amber, same bracket style
- Floor torches: 2 — scattered mid-room
- Stone pillars: 6 — perimeter, same style
- Glowing archway (portal): 1 — north wall, cyan arch
- Raised stone platform / altar: 1 — right side, rectangular, ~3x2 cells, elevated ~0.5 cells, dark stone slab
- Glowing cyan ghost/spirit entity: 1 — near altar area, ~1x2 cells, translucent upright figure
- Cyan floor rune lines: present, denser than Image 1, teal tint on floor suggesting wet surface interaction
- Debris / rubble piles: 3 — scattered floor, ~1x1 cells each, broken stone chunks
- Characters: ~12, excluded

### Image 4 (large hall, prison/library feel, ~16x12 cells)
- Wall-mounted torches: 5 — wall faces and pillar brackets
- Candelabra / multi-arm torch stand: 2 — right side cluster, ~1x2.5 cells, amber multi-flame
- Stone pillars: 10 — densest concentration, full perimeter + interior pair
- Iron gate / portcullis: 1 — north wall, amber-lit, vertical bar gate ~5x3 cells
- Bookshelf / storage shelving: 2 clusters — right wall, ~2x1.5 cells each, stacked rectangular forms
- Raised stone dais / platform: 1 — center-left area, ~3x2 cells
- Glowing archway (portal): 1 — right wall, cyan arch
- Cyan floor rune lines: present, floor-wide
- Chest or crate: 1 possible — lower right, small rectangular, ~1x1 cells
- Characters: ~7, excluded

## Grouped class table

| Class | Total count | Size (cells) | Placement pattern | Critical/Decorative | Priority |
|---|---|---|---|---|---|
| Stone pillar | 32 total | 1.5x4 | Perimeter corners + mid-wall | CRITICAL | P0 |
| Wall/bracket torch | 14 total | 0.5x1.5 | Pillar faces, wall mid-points | CRITICAL | P0 |
| Freestanding brazier | 10 total | 0.5x1.5 | Perimeter, evenly spaced | CRITICAL | P1 |
| Cyan archway / portal | 4 total (1/image) | 3x4 | North wall center, focal point | CRITICAL | P0 |
| Cyan floor rune lines | All 4 images | Floor layer | Full-room network pattern | CRITICAL | P1 |
| Cyan concentric rune circle | 1 (Image 2) | 6x6 diam. | Room center | CRITICAL (boss room) | P1 |
| Raised stone platform/altar | 2 (Img 3+4) | 3x2 | Off-center, against wall | DECORATIVE | P2 |
| Cyan energy orbs (ground) | 7 scattered | 0.5x0.5 | Mid-floor scattered | DECORATIVE | P3 |
| Rubble/debris pile | 3 (Img 3) | 1x1 | Scattered floor | DECORATIVE | P3 |
| Iron gate / portcullis | 1 (Img 4) | 5x3 | North wall | DECORATIVE | P2 |
| Candelabra stand | 2 (Img 4) | 1x2.5 | Wall cluster | DECORATIVE | P2 |
| Stone statue/gargoyle | 1 (Img 2) | 1x1.5 | Corner/perimeter | DECORATIVE | P3 |
| Bookshelf/storage | 2 (Img 4) | 2x1.5 | Right wall cluster | DECORATIVE | P3 |
| Ghost/spirit pillar | 1 (Img 3) | 1x2 | Near altar | DECORATIVE | P3 |
| Chest/crate | 1 (Img 4) | 1x1 | Floor scattered | DECORATIVE | P3 |

## Top 10 priority objects

1. **Stone pillar** (P0 critical) — 8-10 per room, the entire room boundary identity depends on these
2. **Cyan archway / portal** (P0 critical) — single highest-impact focal object; cyan energy fill is the visual signature
3. **Wall-mounted bracket torch** (P0 critical) — 4-6 per room; delivers all warm amber light
4. **Freestanding brazier** (P1 critical) — perimeter rhythm object
5. **Cyan floor rune network** (P1 critical) — floor decal layer, glowing circuit lines, tileable overlay sheet
6. **Cyan concentric rune circle** (P1 critical) — boss-room-specific center decal
7. **Iron gate / portcullis** (P2) — strong dungeon identity signal
8. **Raised stone platform/altar** (P2) — height variation focal
9. **Candelabra multi-arm stand** (P2) — library/ritual room flavor
10. **Rubble/debris pile** (P3 decorative) — entropy/clutter prop

## Pixel-art style notes

- **Color palette**: Dominant = near-black stone (#0d0d12 range), secondary = amber-orange flame (#c87820 to #ffb840), accent = bright cyan (#00e5ff to #40ffee). Three-color logic: dark / warm / cold. No midtones — high contrast only.
- **Lighting**: Dual-source per scene. Amber from torches (warm, localized, radial glow on adjacent floor tiles). Cyan from runes/portal (cool, diffuse, floor-level emission). No overhead fill — scenes deliberately dark between sources.
- **Light source direction**: Torches emit radially. Portal emits forward/downward. No consistent directional sun — all lights are point/area sources embedded in props.
- **Detail density**: Medium-high per cell. Pillars: carved block seams, chipped edges, mortar lines. Floor tiles: cracked stone with rune overlays. Torches: bracket arm + flame sprite. Objects read at ~64px cell width.
- **Silhouette priority**: Every critical object (pillar, arch, brazier) has strongly readable silhouette against dark background. Detail secondary to shape clarity.

## Cross-reference notes

- **Existing RIMA modular pipeline**: Pillars and archways align directly with the "wall perimeter" sheet (Karar #151). Pillar as seam-cover lock (2026-05-24) maps exactly to Image 1-4 pillar placement.
- **PixelLab inventory to check**: Wall torch bracket, freestanding brazier, stone altar slab, rubble pile — standard dungeon prop vocabulary, likely exist as inventory candidates. Cyan archway is custom — requires fresh generate.
- **Floor rune layer**: Not a PixelLab object — Unity Tilemap overlay OR second floor layer with tinted sprite. Tile sheet pipeline, not prop pipeline.

**KEY FINDING:** Cyan archway (portal) is the single most identity-defining object — top P0 custom generate. Floor rune network → tileset pipeline.
