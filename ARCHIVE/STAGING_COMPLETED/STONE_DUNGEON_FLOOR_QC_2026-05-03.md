# Stone Dungeon Floor QC
Date: 2026-05-03
Status: PARTIAL / FAIL FOR FINAL CONTINUOUS FLOOR

## Visual Verdict

The current Stone Dungeon floor tiles are not good enough for final room flooring.

They are acceptable as temporary test-scene placeholders, but they do not read as one continuous floor. In the room mockup they look like stacked or overlapping stone slabs instead of a single walkable surface.

Visual mockups:

- Current room: `STAGING/stone_dungeon_room_mockup.png`
- Selected floor contact sheet: `STAGING/stone_floor_contact_sheet.png`
- Rejected/older floor contact sheet: `STAGING/stone_floor_rejected_contact_sheet.png`

## Why It Fails

Measured selected floor tiles:

- PNG canvas: `64x64`
- Opaque bounds: roughly `63x43`
- Expected RIMA isometric floor footprint: `64x32`

That extra ~10-11 px vertical footprint is the main overlap problem. When painted on an isometric tilemap, each tile visually intrudes into the next tile, so the floor reads like many raised chunks sitting on top of each other.

Second problem: many candidates have a visible internal 2x2 or small-grid slab design. Once repeated, the eye reads the room as hundreds of little block tops, not a unified stone floor.

Third problem: several variants have bevel/side-face depth. That is useful for platforms or raised blocks, but wrong for base floor.

## Current Selected Tiles

| Tile | Current use | QC |
|---|---|---|
| `stone_floor_pro_0.png` | base | Too much raised slab / internal grid. |
| `stone_floor_pro_2.png` | cracked | Same raised slab issue. |
| `stone_floor_pro_4.png` | worn | Better texture, still too chunky. |
| `stone_floor_pro_5.png` | damaged | Reads as standalone damaged slab. |
| `stone_floor_pro_11.png` | grate | Keep only as rare trap/detail tile, not normal floor. |
| `stone_floor_pro_15.png` | minimal | Best of selected set, but still not a clean continuous floor. |

## Older Rejected Tiles

The old one-off `stone_floor_A/B/C/D.png` set is also not a fix. Those tiles have opaque bounds near `64x40+` and visible side thickness, so they read even more like raised platforms.

Some rejected pro tiles can be reused as props/accents later:

- `stone_floor_pro_10.png` -- small floor detail / emblem, not base floor.
- `stone_floor_pro_12.png` -- rubble/rock prop, not floor.
- `stone_floor_pro_8.png` -- Mossy Crypt candidate, but still needs flat-floor check.

## Correct Next Generation Target

Regenerate base floor with stricter wording:

```text
flat isometric diamond floor tile, top surface only, no raised slab, no side faces,
no bevel thickness, no platform edge, exact 64x32 diamond footprint on transparent 64x64 canvas,
single continuous stone floor surface, subtle grout lines, edges align seamlessly,
no 2x2 mini-tile panel layout, no chunky cobblestone blocks, no object sitting on tile
```

Recommended batch:

1. flat base stone floor
2. subtle hairline crack
3. faint worn surface
4. light dirt stain
5. rare small chip
6. very subtle color-value variation

Do not include grates, moss clumps, rubble, holes, or heavy cracks in the base floor batch. Generate those as separate decals/props or rare overlay tiles.

## Scene Status

`Assets/Scenes/_IsoGame.unity` currently uses the selected floor set for a larger visual test room. This is useful for scale/blockout testing only.

Before committing to production environment art, replace the floor set with a flat continuous floor pass.
