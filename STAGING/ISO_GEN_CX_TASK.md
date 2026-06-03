# ISO ASSET PACK — RAW SHEET GENERATION (cx imagegen, profile laurethayday)

ACTIVE RULES: (1) think before generating (2) exactly the 4 outputs listed (3) surgical — only write the 4 PNGs (4) BLOCKED if unclear.

## Amaç
Generate the 4 RAW sheets for the RIMA iso diamond-room asset pack (per STAGING/ISO_ASSET_PACK_BLUEPRINT_S6.md §2).
These are RAW sheets — the orchestrator slices them afterward. Use ChatGPT image_gen. Output to `STAGING/iso_raw/`
(create the folder). Each: TRANSPARENT PNG (alpha, NO magenta, NO checkerboard, NO solid fill). If image_gen returns
oversized, nearest-neighbor downscale to the EXACT stated pixel size BEFORE saving. Generate in order A,B,C,D.

## OUTPUT 1 — `STAGING/iso_raw/floor_iso_sheet.png` (EXACTLY 640x256)
```
Generate a single transparent PNG sprite sheet, exactly 640x256 pixels, fully transparent background (alpha, no
checkerboard, no solid fill, no magenta). Strict uniform grid of 5 columns x 4 rows, each cell exactly 128x64 px,
no gutters, content centered. In EVERY cell draw one 2:1 isometric (dimetric) diamond floor tile — a single worn
flagstone seen from a top-down 3/4 angle — inscribed with a 4px top/bottom margin (diamond apex top, apex bottom,
left & right points touching cell mid-edges).
Style: dark-fantasy ruined-keep stone, flat painterly pixel-art, hard edges, NO anti-alias gradients, NO gloss, NO
bevel emboss, NO photoreal. Stone palette charcoal-to-blackened-iron only: base #1C1D24 to #2E303F, deep grout
shadow #121319, sparing upper-left rim highlight #3A3D4E. Single light source upper-left. Each diamond is an
individually cracked, pitted, worn slab with a 1px dark grout line on its edges and a 3px dark chamfer on the
lower-left/lower-right edges to imply slight thickness (floor is FLAT, only a hint of depth).
Tiles row-major left-to-right, top-to-bottom:
Row1: (1) isolated slab, all 4 diamond edges capped with a thick rounded grout rim. (2) open at top-N edge, other 3
capped. (3) open at right-E edge. (4) open at bottom-S edge. (5) open at left-W edge.
Row2: (6) open top&bottom (N+S), left&right capped. (7) open left&right (E+W), top&bottom capped. (8) inner corner
open N+E. (9) open E+S. (10) open S+W.
Row3: (11) open N+W. (12) T open E+S+W, N capped. (13) open N+S+W, E capped. (14) open N+E+W, S capped. (15) open
N+E+S, W capped.
Row4: (16),(17),(18) three DIFFERENT fully-interior slabs, all 4 edges open/blending (no rim cap), varied crack
patterns. (19) interior slab with a thin cyan #00FFCC glowing rift line seeping along ONE grout seam (sparse,
hairline, do not flood). (20) leave empty/transparent.
Keep all tiles the same diamond size, aligned to the grid for math-slicing. Subtle hairline grout, NOT rounded blob
corners. No text, labels, borders, drop shadows outside the diamond, no gold, no parchment. Cool neutral tone (warm
light added later in engine). Output: 640x256 RGBA PNG.
```

## OUTPUT 2 — `STAGING/iso_raw/keepwall_sheet.png` (EXACTLY 1024x1024)
```
Generate a 1024x1024 transparent PNG sprite sheet, exactly 4 columns by 4 rows of 256x256 cells, hard transparent
gutters between every cell, each cell perfectly centered, no cell bleeding into another.
Style: 2D 3/4 top-down isometric (2:1 dimetric) RUINED KEEP stone wall pieces, hand-painted flat-painterly, NO
photoreal, NO gloss, NO bevel emboss, NO gradient background, NO text. Material: blackened charcoal iron-stone
#1C1D24 to #2E303F, lit top cap up to #3A3D4E, deep contact-shadow #0E0F14 at the base. Each piece shows THREE
faces: a flat lit diamond TOP CAP, a tall camera-facing FRONT face (carrying chipped bricks/cracks), and a darker
SIDE face for depth. Cracked, chipped corners, a few missing bricks, exposed rubble core. Lighting: cool top-down
key + faint warm south fill, dark dungeon. Each piece's footing diamond sits at the SAME low position in its cell
(bottom-center).
Cells in reading order:
1 long straight REAR wall run (faces camera, ~2.2 cells tall). 2 SIDE wall run (faces left, edge-on). 3 outer
CORNER turn. 4 T-junction opening AWAY from camera. 5 T-junction opening TOWARD camera. 6 T-junction opening
SIDEWAYS. 7 four-way CROSS junction. 8 wall END-cap facing camera. 9 wall END-cap facing away. 10 wall END-cap
facing side. 11 single isolated BLOCK. 12 LOW front PARAPET edge (short, see over it, ~0.6 cell). 13 low corner
parapet. 14 rubble debris pile variant A. 15 rubble debris pile variant B. 16 rubble debris pile variant C.
Tiny cyan #00FFCC rune-crack accents on at most one or two pieces, very sparing. Alpha transparency, no magenta,
no text, no labels.
```

## OUTPUT 3 — `STAGING/iso_raw/keepcolumn.png` (EXACTLY 256x384)
```
Single transparent PNG, 256x384, one tall RUINED KEEP support COLUMN, 2D 3/4 isometric (2:1 dimetric), hand-painted
flat-painterly, blackened charcoal iron-stone palette (#1C1D24-#2E303F, top cap #3A3D4E, base shadow #0E0F14).
Three faces visible (lit top cap, tall front face, dark side face). Chipped, cracked, ruined. The FOOTING diamond
is NARROW (one floor-cell wide) at the bottom-center of the canvas; the column rises tall with headroom above the
foot. Faint cyan #00FFCC rune glow at one crack only, sparing. Cool top key + warm south fill. Alpha transparent,
no background, no magenta, no text.
```

## OUTPUT 4 — `STAGING/iso_raw/keeparch.png` (EXACTLY 384x320)
```
Single transparent PNG, 384x320, one RUINED KEEP doorway ARCH spanning TWO floor cells wide, 2D 3/4 isometric
(2:1 dimetric), hand-painted flat-painterly, blackened charcoal iron-stone palette (#1C1D24-#2E303F, lit top cap,
#0E0F14 base shadow). Two stone jambs + a broken arch lintel, three faces visible per jamb (top cap, front, dark
side). A glowing cyan #00FFCC seal-rift energy fills the opening (the ONLY strong cyan in the pack), faint and flat,
no bloom gloss. Footing diamonds at bottom-center spanning the 2-cell base. Cool top key + warm fill. Alpha
transparent, no background, no magenta, no text.
```

## Output
Save the 4 PNGs to STAGING/iso_raw/ at the EXACT pixel sizes. Append 4 lines to STAGING/IMAGEGEN_PLACEHOLDER_REGISTRY.md
(path + "iso asset pack RAW sheet -> sliced into IsoKit, PixelLab-replaceable later"). Report the 4 final paths +
one line each on whether the grid/cells look clean for math-slicing.
