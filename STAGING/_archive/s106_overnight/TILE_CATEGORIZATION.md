# Iso Tile Categorization (16 PixelLab 35° tiles)

**Source:** `Assets/Sprites/AssetPackV3/floor_iso_pixellab_35deg/tile_<0..15>.png`
**Verified:** 2026-05-25 visual inspection by Opus, all 16 read

## Categories (CORRECTED)

| Category | Tile indices | Count | Visual |
|---|---|---|---|
| **STONE_BASE** (plain cobblestone, dark gray) | 0, 1, 2, 3 | 4 | 4×4 cobblestone grid, no cyan |
| **STONE_CYAN_CRACK** (energy seam variants) | 4, 5, 6 | 3 | Cobblestone + cyan crack/cross |
| **DIRT** (brown earth) | 7, 8, 9, 10 | 4 | Brown dirt, mottled |
| **RUNE_FOCAL** (ritual circles) | 11, 12, 13, 14, 15 | 5 | Cyan rune circles (diff patterns) |

## Stream O.1 painter bug
Stream O.1 spec uses WRONG index map:
- "8-11 = dirt" — actually 7-10 are dirt, 11 is rune
- "4-7 = cyan" — actually 4-6 are cyan, 7 is dirt
- "12-15 = ritual" — actually 11-15 are runes (5, not 4)

Result: tile_7 (dirt) painted as cyan crack tile; tile_11 (rune) painted as dirt tile. Adds extra noise to user's "tiles mismatched" complaint.

## Repaint options (after wall-less verdict)

### Option A — Pure stone arena (recommended if wall-less path)
- STONE_BASE: 90% (random pick from 0-3)
- STONE_CYAN_CRACK: 8% (random from 4-6)
- RUNE_FOCAL: 2% (random from 11-15), placed as **3 clusters of 1-3 cells each** (centerpieces, not scattered)
- DIRT: 0% — drop entirely (no biome mixing)
- Why: clean dungeon look, runes draw eye to focal points, no material dissonance

### Option B — Two-zone arena
- Left half (~half cells): STONE_BASE 95% + RUNE 5% (dungeon zone)
- Right half: DIRT 100% (earthen zone)
- Boundary: clear visual line, no mixing
- Why: showcases both tile families, narrative cohesion

### Option C — Pure stone + dirt path
- STONE_BASE: 85%
- STONE_CYAN: 5%
- DIRT: 10% as a **single connected path** (8-12 connected cells, organic shape)
- RUNE: 1-2 cells total, focal
- Why: dirt feels intentional (worn path), not noise

## Locked categorization (do not re-derive)
Saved as memory pointer: see [[project-playable-arena-iso-proof-2026-05-25]] for live scene state, this file for tile->category mapping.
