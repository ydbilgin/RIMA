# Wall Pack v3 Tile Audit

Source: `Assets/Art/AssetPacks/Act1_ShatteredKeep/wall_pack_v3/`

Assumption: footprint is sprite rect divided by 64 PPU. All sprites use a bottom-foot pivot, so placement is treated as cell-bottom unless noted.

| Tile | Footprint cells | Edge orientation | Placement anchor | Recommended usage rule |
|---|---:|---|---|---|
| tile_archway_NE | 1.80 x 2.52 | NE-running full doorway | cell-bottom | Use singly on back/north or west-facing connections. Do not pair adjacent archway halves. |
| tile_archway_SE | 1.81 x 2.53 | SE-running full doorway | cell-bottom | Use singly on front/south or east-facing connections. Leave one neighboring cell open when possible. |
| tile_column_freestanding | 1.00 x 2.53 | no edge, vertical prop | cell-bottom | Use as an interior prop with at least one clear cell around visual contact points. |
| tile_corner_inner_a | 1.31 x 1.56 | inner corner, one rotation | cell-bottom | Use at L-shape recesses; verify rotation by screenshot because only two inner variants exist. |
| tile_corner_inner_b | 1.33 x 1.56 | inner corner, alternate rotation | cell-bottom | Use at opposite L-shape recesses; avoid tight T-junction adjacency. |
| tile_corner_outer_a | 1.16 x 1.30 | outer corner, front/left variant | cell-bottom | Use on rectangular perimeter corners where full-height mass is acceptable. |
| tile_corner_outer_b | 1.22 x 1.30 | outer corner, front/right variant | cell-bottom | Use on rectangular perimeter corners where full-height mass is acceptable. |
| tile_corner_outer_c | 1.25 x 1.30 | outer corner, back/left variant | cell-bottom | Preferred for north-west/back-left corner under back-wall-only convention. |
| tile_corner_outer_d | 1.31 x 1.20 | outer corner, back/right variant | cell-bottom | Preferred for north-east/back-right corner under back-wall-only convention. |
| tile_floor_edge | 1.31 x 1.00 | low floor lip | cell-bottom | Use as a visual containment edge when no wall height is desired. |
| tile_foundation_a | 1.31 x 1.08 | low foundation edge | cell-bottom | Use on front/right edges to keep sightlines open. |
| tile_foundation_b | 1.33 x 1.14 | low foundation edge, alternate | cell-bottom | Use as alternate low edge; needs orientation-specific test before broad use. |
| tile_low_wall_corner | 1.23 x 1.28 | low corner | cell-bottom | Use on front corners when full outer corners block floor readability. |
| tile_low_wall_endcap | 1.27 x 1.08 | low endcap | cell-bottom | Use beside low-wall breaks, especially single archway openings. |
| tile_low_wall_straight | 1.23 x 1.28 | low straight wall | cell-bottom | Use on south/front edges; keep it off back edges where room silhouette needs height. |
| tile_straight_NE | 1.33 x 1.47 | NE-running full wall | cell-bottom | Use for back/north and west silhouettes. Adjacent cells are acceptable on straight runs. |
| tile_straight_SE | 1.31 x 1.48 | SE-running full wall | cell-bottom | Use sparingly on right/front full-height tests; avoid all-side enclosure in gameplay rooms. |
| tile_T_junction_a | 1.30 x 1.52 | T-junction, variant A | cell-bottom | Use only after a dedicated junction map test; it visually occupies more than one cell. |
| tile_T_junction_b | 1.28 x 1.52 | T-junction, variant B | cell-bottom | Use only after a dedicated junction map test; it visually occupies more than one cell. |
| tile_T_junction_c | 1.03 x 1.53 | T-junction, narrow variant C | cell-bottom | Candidate for compact junctions; requires orientation validation before production paint. |
| tile_T_junction_d | 0.98 x 1.53 | T-junction, narrow variant D | cell-bottom | Candidate for compact junctions; requires orientation validation before production paint. |
| tile_wall_hero | 2.38 x 2.59 | wide hero back-wall feature | cell-bottom, multi-cell | Place once only. Reserve a two-cell span and never paint adjacent hero tiles. |

## Strategy Decision

Selected: Strategy A, back walls only.

Rationale: the failing scenes used full-height walls on all four sides, which hid the floor and made the room outline read as overlapping wall chunks. Strategy A matches the camera-facing iso convention: north and west edges carry the full-height silhouette, while south and east edges use low walls or foundations. This keeps dungeon mass on the back edges without blocking the playable floor. It also avoids the known multi-cell failures by placing archways and the hero tile as single sprites.

Rejected:
- Strategy B keeps the root overlap problem and depends on a camera change outside the scene asset.
- Strategy C changes sprite import pivots globally and would invalidate existing paints before the wall placement convention is proven.
