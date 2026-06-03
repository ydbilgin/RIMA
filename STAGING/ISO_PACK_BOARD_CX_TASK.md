# ISO ASSET PACK EXPLAINER BOARD (cx imagegen, profile laurethayday)

ACTIVE RULES: (1) one image only (2) exact prompt below (3) save to STAGING/iso_raw/ (4) BLOCKED if unclear.

## Amaç
Generate ONE infographic / design-board image (like the project's ADIM-1 / ADIM-5 reference boards) that explains the
RIMA iso asset pack per the LOCKED tiling-logic decision: what the pack is, how pieces CONNECT, how they MERGE, and an
example irregular room. Use ChatGPT image_gen. Save to `STAGING/iso_raw/iso_pack_board.png` (1536x1024). Append 1 line
to STAGING/IMAGEGEN_PLACEHOLDER_REGISTRY.md.

## PROMPT (use verbatim)
```
A dark-fantasy video-game ASSET-PACK DESIGN BOARD / infographic, 1536x1024 landscape, in the style of a clean labeled
reference sheet. Dark charcoal background #15161C with thin panel dividers and small cyan #00FFCC section labels. All
art is flat-painterly 2D top-down 3/4 ISOMETRIC (2:1 dimetric) ruined-keep stone, charcoal-to-blackened-iron palette
(#1C1D24 to #2E303F) with sparing cyan #00FFCC seal accents and warm orange brazier glow. NO photoreal, NO gloss, NO
gradient sky, NO 3D render look — hand-painted game-art reference board. Title across the top: "RIMA ISO ASSET PACK —
CONNECT & MERGE". Four labeled panels:

PANEL 1 (top-left) labeled "FLOOR — 4-EDGE AUTO-MERGE": a neat 4x4 grid of 16 isometric diamond stone floor tiles
showing every edge combination — isolated capped slab, single-edge ends, straight runs, L corners, T junctions, and a
full interior fill — each diamond with broken capped outer rims. Beside it a small before/after: a few scattered
separate floor diamonds, an arrow, then them merged into ONE continuous stone island with auto-capped cracked rims
floating over black void.

PANEL 2 (top-right) labeled "WALLS — DEPTHFUL PIECES": a tidy row-grid of distinct isometric ruined stone wall pieces,
each clearly showing a lit TOP CAP + a tall camera-facing FRONT face + a darker SIDE face, small labels under each:
"Straight", "Inner Corner" (a concave turn), "Outer Corner" (a convex turn wrapping out), "T", "Cross", "End",
"Low Front", "Column" (tall), "Arch" (broken doorway with a cyan seal glow inside).

PANEL 3 (bottom-left) labeled "CONNECT & FACE": a small logic diagram — a single wall cell with N/E/S/W neighbor arrows
auto-resolving into the right shape (a corner / a T); and an inset showing the SAME corner rendered as INNER (concave,
stone floor on the inside of the bend) vs OUTER (convex, floor outside, wall wraps a protrusion), with a tiny floor-fill
icon marking the inside. A short note "OPEN FRONT: camera-side walls drop to LOW cutaway so the hero stays visible",
illustrated by a tall rear wall and a low broken front lip.

PANEL 4 (bottom-right) labeled "EXAMPLE ROOM": ONE finished IRREGULAR (non-rectangular, with indentations and
protrusions / alcoves and jutting bays) isometric diamond ruined-keep room built from the pack — a floating broken
stone island in dark void, tall masonry walls on the rear and sides turning at inner and outer corners, a low open
front, a couple of columns, braziers with warm orange glow, cyan rift cracks across the floor, and a tiny hero figure
in the center for scale. This panel should look like an actual Diablo/Hades dungeon room screenshot.

Clean infographic layout, readable small labels, cohesive dark dungeon mood, cyan used sparingly. No watermark.
```

## Output
Save STAGING/iso_raw/iso_pack_board.png at 1536x1024 (downscale if oversized). Report the path + one line on legibility.
