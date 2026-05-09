Aşağıdaki 6 sprite sheet'i sırayla üret. Her biri ayrı bir görsel. Bir sonrakine geçmeden önce bir öncekini bitir.

Tüm sheet'lerde geçerli ortak stil:
- Pixel art, hard pixel edges, no anti-aliasing, no blur, no gradients
- Pixel clusters minimum 4px wide — no sub-pixel detail
- Cold muted desaturated dark stone palette
- Background: solid pure green #00FF00 (RGB 0,255,0) flat, no gradient, no atmospheric lighting
- No green pixels inside tiles — no moss, no vegetation

---

SHEET 1 — F1 Floor (Cold Gray Stone)

OUTPUT: 1024×1024px. GRID: 4 columns × 4 rows = 16 tiles, zero gaps. Each cell exactly 256×256px.

Each tile is a 2:1 isometric diamond (64×32 logical pixels rendered at 256×128, centered vertically in its 256×256 cell). Background #00FF00 fills remainder of each cell.

Stone: charcoal-gray cut stone slabs. 2×2 grid of cut stone blocks per diamond. Block colors alternating #1e2030 and #262838. Mortar lines #161620 1px wide. All 4 diamond edges exactly #161620 — do not deviate.

16 tiles: subtle variation across tiles — minor mortar cracks, small chip marks, worn patches. All 16 read as same floor set. No symbols, no puddles, no dramatic color differences.

---

SHEET 2 — F2 Floor (Cracked Stone, Mid-Dungeon)

OUTPUT: 1024×1024px. GRID: 4×4 = 16 tiles. Each cell 256×256px.

Same isometric diamond shape and base stone colors as Sheet 1 (#1e2030, #262838, mortar #161620). Mid-dungeon — more damaged.

Added damage: horizontal cracks along mortar joints #0e0e18, diagonal hairline cracks on some blocks, rubble dust patches #1a1c28. Each of the 16 tiles has different crack placement and severity. No glow, no bioluminescence.

All 4 diamond edges exactly #161620.

---

SHEET 3 — F3 Floor (Dark Volcanic Stone, Deep Zone)

OUTPUT: 1024×1024px. GRID: 4×4 = 16 tiles. Each cell 256×256px.

Darker than F1/F2. Stone colors shifted: #161822 and #1e1e2c, mortar #0e0e16. Heavy cracking, some blocks nearly shattered. In the deepest cracks only: faint dark cyan tinge #0a1a1a, 1px wide — barely visible, not bright.

All 4 diamond edges exactly #161620. F3 must look clearly darker and more deteriorated than F1/F2.

---

SHEET 4 — W1 Wall Connector Set (Clean Stone)

OUTPUT: 1024×768px. GRID: 4 columns × 2 rows = 8 cells, zero gaps. Each cell exactly 256×384px.

Each cell contains one isometric wall tile (64×96 logical pixels). All cells share: dark stone masonry #2e3040 with variation #2a2c3c and #323446, mortar joints #12141e, block chip marks #12141e.

SHARED EDGE CONTRACT for all 8 tiles — exact, no deviation:
Top surface strip 16px: #3a3c50
Left/right profile edges: #1a1c28
Base edge: #12141a

BACKGROUND: Solid #00FF00 everywhere outside each wall shape.

CELL CONTENTS — row 1 left to right, then row 2 left to right:

[R1-C1] OUTER CORNER A — west wall and south wall meet, corner protrudes toward camera (convex). Vertical stone pilaster at center-top, 4px wide #1e2030, 1px highlight #3a3c50 on corner edge. L-shaped top strip #3a3c50. Shadow gradient west face #1a1c28 top to #12141a base.

[R1-C2] OUTER CORNER B — same convex corner geometry, slightly different mortar crack and chip mark placement for visual variety. Identical edge colors.

[R1-C3] INNER CORNER A — two wall runs meet concave, receding away from camera. Vertical crevice 2px wide #0e0e18 at back junction, deepest shadow. Blocks near crevice #1e2030. L-shaped top strip #3a3c50, inner point #0e0e18.

[R1-C4] INNER CORNER B — same concave geometry, different interior block variation. Identical crevice and edge colors.

[R2-C1] DOOR ARCH — pointed stone arch opening centered horizontally. Arch interior fully open (#00FF00 shows through — this is a passageway). Voussoir stones #2a2c3c radiating from keystone #262838. Lintel above arch 4px #2e3040. Block columns flanking arch. Arch soffit shadow #0a0c12. No warm light, cold palette.

[R2-C2] WEST WALL END CAP — wall terminates at right side. Full stone block rows on left portion. Right: finished end face 8px wide #262838 showing cut cross-section. Top of end face: 2px strip #3a3c50. End face has 3 horizontal mortar marks #12141e. Open space right of end face (#00FF00).

[R2-C3] SOUTH WALL END CAP — wall terminates at top. Full stone block columns on south face. Top: coping stone cap 24px tall #3a3c50, dark rim 2px #1a1c28 around perimeter, bevel front edge #464858 1px.

[R2-C4] OUTER CORNER C — third outer corner variant, more pronounced chip marks and a hairline diagonal crack on the pilaster. Still matches identical edge contract.

---

SHEET 5 — W2 Wall Connector Set (Damaged Stone)

OUTPUT: 1024×768px. GRID: 4×2 = 8 cells. Each cell 256×384px.

Same grid layout and same 8 tile types as Sheet 4 (W1), but W2 damaged style throughout:
- Horizontal cracks 1-2px #0e0e18 along mortar joints on all faces
- More chipped corners #0a0c12
- Deeper base shadow
- Door arch: crack running through lintel above opening
- Coping stone: diagonal crack on cap surface
- No glow, no warm colors, cold palette only

All tiles share same edge contract as Sheet 4: Top #3a3c50, Left/Right #1a1c28, Base #12141a.

Background #00FF00. Arch interior #00FF00. End cap open space #00FF00.

---

SHEET 6 — Floor Decals F1

OUTPUT: 512×512px. GRID: 2 columns × 2 rows = 4 tiles, zero gaps. Each cell exactly 256×256px.

Each cell contains a 2:1 isometric diamond (64×32 logical, rendered 256×128, centered in cell). Background #00FF00 outside diamond. Floor surface: cold gray stone #1e2030 to #262838. All 4 diamond edges exactly #161620.

[R1-C1] WORN SCUFF — drag marks and boot scuffs: dark gray streaks #14161e, 2-3 crossing lines 2-3px wide. Subtle.

[R1-C2] DRIED BLOOD — irregular blob stain #2a1a18 to #1e1010, very dark dried brownish-red, offset to one side. Small splatter dots 1-2px nearby. Not bright, ancient.

[R2-C1] SCORCH MARK — circular ash ring from fallen torch. Center soot #0e0e0e, concentric ring gradient #161616 to #1a1a1a, diameter ~80px. Fully cold, no glow, no embers.

[R2-C2] CRACKED SLAB — diagonal structural crack #0a0a12 2px wide crossing the diamond. Halves slightly offset 1-2px. Shadow along crack edge #12141a. Corner chips #0e0e16.

---

PROCESS REFERENCE (üretim bittikten sonra):
Sheet 1-3 (floor):   --cols 4 --rows 4 --width 64 --height 64
Sheet 4-5 (wall set): --cols 4 --rows 2 --width 64 --height 96
Sheet 6 (decal):     --cols 2 --rows 2 --width 64 --height 64
