# ISO ASSET PACK — FULL GENERATION (cx imagegen, profile yekta)

ACTIVE RULES: (1) exactly the outputs listed (2) TRANSPARENT PNG (alpha), NO magenta (3) keep raw copies, do NOT delete (4) BLOCKED if unclear.

## Amaç
Generate the full RIMA iso asset pack (modular pieces) matching the locked design board "RIMA ISO ASSET PACK — CONNECT
& MERGE": dark ruined-keep isometric pieces that assemble into a Diablo/Hades diamond room. ChatGPT image_gen. Save RAW
sheets to `STAGING/iso_raw/` (KEEP them — future PixelLab reference). Append registry lines. If a sheet bleeds cells,
fall back to per-piece individual gen at the same per-cell size.

## SHARED STYLE (every output)
2D top-down 3/4 ISOMETRIC (2:1 dimetric), flat-painterly hand-painted game art, dark ruined-keep stone. Palette:
charcoal/blackened-iron #1C1D24 to #2E303F, lit top cap up to #3A3D4E, deep contact-shadow #0E0F14, cyan #00FFCC
ONLY as sparing seal/rift accents (<10%), warm orange brazier glow only on the brazier piece. NO photoreal, NO gloss,
NO bevel, NO gradient background, NO text/labels, NO drop-shadow baked outside the piece. TRANSPARENT RGBA, hard clean
alpha edges, no magenta, no checkerboard. Each piece's foot sits at the SAME bottom-center position in its cell.

## OUTPUT 1 — `STAGING/iso_raw/iso_floor_256.png` (EXACTLY 1024x1024, 4x4 @ 256x256)
```
A 1024x1024 transparent PNG sprite sheet, strict 4x4 uniform grid, each cell EXACTLY 256x256, no gutters. In each cell
draw ONE 2:1 isometric diamond stone floor tile, the diamond exactly 256 wide x 128 tall, centered vertically. Worn
cracked flagstone, dark charcoal #1C1D24-#2E303F, 1px dark grout on edges, faint upper-left rim light. Each tile is a
4-EDGE auto-merge silhouette: a present neighbour edge = that diamond side blends open (no cap); a missing neighbour =
that side has a thick broken capped rim with a slight dark cliff underside.
Row-major: (1) all 4 sides capped isolated slab. (2) open N only. (3) open E only. (4) open S only. (5) open W only.
(6) open N+E. (7) open E+S. (8) open S+W. (9) open W+N. (10) open N+S (a straight run, capped E+W). (11) open E+W
(capped N+S). (12) open N+E+S, cap W. (13) open E+S+W, cap N. (14) open S+W+N, cap E. (15) open W+N+E, cap S. (16) all
4 open, full interior fill with subtle cracks. Same diamond size in every cell, grid-aligned for math-slicing. Cool
neutral tone (warm light added in engine). 1024x1024 RGBA, transparent.
```

## OUTPUT 2 — `STAGING/iso_raw/iso_walls_256.png` (EXACTLY 1024x1536, 4x3 @ 256x512)
```
A 1024x1536 transparent PNG sprite sheet, strict 4 columns x 3 rows, each cell EXACTLY 256x512, hard transparent
gutters, each piece centered, nothing bleeds between cells. Each is a DEPTHFUL 2:1 isometric RUINED KEEP stone wall
piece showing THREE faces: a lit diamond TOP CAP, a tall camera-facing FRONT face (chipped bricks/cracks), and a darker
SIDE face. The footing diamond (256 wide x 128 tall) sits at the BOTTOM-CENTER of every cell; the wall rises up ~2 cells
tall. Charcoal stone #1C1D24-#2E303F, top cap #3A3D4E, base shadow #0E0F14, chipped corners, a few missing bricks.
Cells reading order: (1) STRAIGHT rear wall run (faces camera). (2) STRAIGHT side wall run (edge-on, faces left).
(3) INNER corner (concave turn, the open notch faces the camera/interior). (4) INNER corner mirrored. (5) OUTER corner
(convex turn wrapping around a protrusion). (6) OUTER corner mirrored. (7) T-junction opening toward rear. (8) T-junction
opening toward camera/front. (9) four-way CROSS junction. (10) wall END cap. (11) LOW FRONT cutaway wall (short, ~0.7
cell, see over it). (12) tall COLUMN/pillar (narrow 1-cell foot, ruined, one faint cyan rune crack). 1024x1536 RGBA,
transparent.
```

## OUTPUT 3 — `STAGING/iso_raw/iso_decor_256.png` (EXACTLY 1024x1024, 4x4 @ 256x256)
```
A 1024x1024 transparent PNG sprite sheet, strict 4x4 uniform grid, each cell EXACTLY 256x256, no gutters, each prop
centered with its foot at bottom-center. 2:1 isometric ruined-keep dark stone style. Cells: (1) lit stone BRAZIER with
a warm orange #FF8030 flame glow. (2) unlit brazier/cauldron. (3) cyan #00FFCC RIFT crack decal, a thin branching glowing
crack on transparent (floor overlay). (4) a different rift crack variant. (5) a large cyan #00FFCC circular SEAL glyph
decal (concentric magic circle, flat, for the floor center). (6) a small cyan rune mark decal. (7) rubble/debris pile A.
(8) rubble pile B. (9) loose broken blocks. (10) a hanging tattered BANNER (charcoal cloth, faint cyan sigil). (11) a
stone sarcophagus/chest. (12) scattered bones. (13) a cracked stone slab decal. (14) a wall-foot debris cap. (15) a small
floor moss/stain decal. (16) a stack of broken bricks. Dark charcoal, cyan sparing, warm only on the lit brazier. 1024x1024
RGBA, transparent.
```

## OUTPUT 4 — `STAGING/iso_raw/iso_arch_256.png` (EXACTLY 512x512, individual)
```
A 512x512 transparent PNG, ONE ruined-keep doorway ARCH spanning TWO floor cells wide, 2:1 isometric, depthful (two stone
jambs each with top cap + front + side face, broken lintel). A glowing cyan #00FFCC seal-rift energy fills the opening (the
only strong cyan). Footing diamonds at bottom-center spanning the 2-cell base. Charcoal #1C1D24-#2E303F, base shadow
#0E0F14, warm-neutral. 512x512 RGBA, transparent, no magenta, no text.
```

## Output
Save all 4 to STAGING/iso_raw/ at the EXACT sizes (downscale NN if oversized). KEEP the raw PNGs (do not delete) — they are
the PixelLab-reference master copies. Append 4 lines to STAGING/IMAGEGEN_PLACEHOLDER_REGISTRY.md. Report the 4 paths +
one line each on cell cleanliness for math-slicing.
