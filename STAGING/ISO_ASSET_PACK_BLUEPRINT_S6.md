# RIMA ISO ASSET PACK — EXECUTABLE PRODUCTION BLUEPRINT (S6)

Single source of truth for the iso diamond-room pivot (Diablo-Immortal / Hades look). Synthesizes the FLOOR,
WALL, and COMPOSITION specs into one deterministic build. The orchestrator can execute top-to-bottom with no
further design decisions. All paths absolute. Refs verified (`00_18_45 (1)`, `21_29_23 (2)`, `16_12_48 (4)`,
ADIM-1 + ADIM-5 boards). Code facts verified against live source — see **§0 CODE-VERIFIED FACTS + FLAGS** first.

Target sprite root (orchestrator brief): `Assets/Sprites/Environment/IsoKit/` with `floor/` and `walls/` subfolders.
PPU: floor 128, walls 64. Grid `cellLayout=Isometric`, `cellSize=(1,0.5,1)`, 2:1 dimetric. Pivot = BottomCenter
(diamond bottom vertex) for every piece. On-brand: charcoal/blackened-iron `#1C1D24→#2E303F`, cyan `#00FFCC`
SPARING (<15%, seal/rift only), warm brazier accents added by lighting (NOT baked). Transparent PNG, no magenta.

---

## §0. CODE-VERIFIED FACTS + FLAGS (read before executing — these CHANGE the input specs)

Verified live in `WangResolver.cs`, `WallRunBuilder.cs`, `AssetPackV3Importer.cs`, `RoomPlacementTypes.cs`,
`InPlayMapPaintOverlay.cs`, `RoomData.cs`, `RoomDataMutator.cs`, `RoomDataComposer.cs`.

**FLAG 1 — Wang bit order: `Resolve4` ≠ `EdgeMask8`. The floor spec's cardinal values are WRONG; fix in the LUT.**
- `WangResolver.Resolve4` cardinal bits: **`N=1, E=2, S=4, W=8`** (4-neighbor).
- `WangResolver.EdgeMask8` bits: `N=1, NE=2, E=4, SE=8, S=16, SW=32, W=64, NW=128` (8-neighbor).
- The FLOOR spec wrote the 16-tile table in `EdgeMask8` cardinal values (`E=4,S=16,W=64`). That is fine ONLY
  if the floor LUT is keyed off `EdgeMask8` and masks `(m&1)|(m&4)|(m&16)|(m&64)`. **Do NOT reuse `Resolve4`'s
  `N=1,E=2,S=4,W=8` for the floor.** Floor uses `EdgeMask8`; walls use `Resolve4`. They are different bit spaces.
  The blueprint floor LUT (§5.2) is written against `EdgeMask8` and is correct.

**FLAG 2 — CCW rotation table MATCHES the context table exactly. No mismatch. Wall mapping (§4) verified true.**
`Resolve4` returns: End S@0 / E@90 / N@180 / W@270 · Corner S+E@0 / N+E@90 / N+W@180 / S+W@270 ·
T openN@0 / openW@90 / openS@180 / openE@270 · Straight E+W@0 / N+S@90 · Cross@0 · Single@0. Identical to the
locked context table. The WALL spec's resolved→sprite table is therefore valid as-is.

**FLAG 3 — `SpriteForShape` IGNORES rotation+mask today → depthful art WILL be Z-rotated → task #7 is mandatory.**
Current `WallRunBuilder.SpriteForShape(piece, shape)` swaps sprite by `WangShape` ONLY, then
`PlaceOne` applies raw `Quaternion.Euler(0,0,rotationDegrees)`. So a depthful corner at θ=90 gets its front face
laid on its side. **Placeholders (flat) tolerate this; FINAL depthful art does NOT.** Task #7 (`WallFacingResolver`)
must intercept: take `WangResult` → return `(sprite, flipX, residualZ=0)` and set `sr.flipX`, forcing the Euler-Z
to 0 for depthful pieces. Until #7 lands, the pack still imports and composes (just rotated wrong) — so generate
the pack now, wire #7 before the final render. **This is the single highest-risk code dependency.**

**FLAG 4 — `FootprintFromSpriteSize` auto-derives footprint from sprite px. Tall column must stay 1 cell WIDE.**
`InPlayMapPaintOverlay` calls `FootprintFromSpriteSize(sprite)` to set `WallPiece.footprint`. A 2-cell-WIDE arch
sprite auto-reports a wider footprint (intended → 2×1). A TALL column must NOT be wide: author the column sprite
so its diamond FOOT is 1 cell wide (height overhang only) or footprint mis-derives to >1 wide. Keep column art
narrow at the base; verticality is pure overdraw above the foot. Arch IS meant to derive 2-wide.

**FLAG 5 — Importer paths are hard-coded to `Assets/Sprites/AssetPackV3/{walls,floor}/`, PPU 64/32. Two options.**
`AssetPackV3Importer.cs` only triggers on `AssetPackV3/walls/` (PPU64) and `AssetPackV3/floor/` (PPU32).
The brief targets `IsoKit/`. **CHOSEN: extend the importer with IsoKit path rules (floor PPU 128, walls PPU 64)**
rather than relocate — keeps AssetPackV3 untouched and gives the floor its 128px painterly headroom. Patch in §6.1.

**FLAG 6 — Variant loader naming contract is exact.** `InPlayMapPaintOverlay.VariantSprite` resolves
`<NormalizeName(displayName)>_<suffix>` where `suffix ∈ {straight,corner,t,cross,end,single}` (also tries compact
`<name><suffix>`). So the SIX base-suffix wall sprite names are **mandatory exact spellings**. Facing variants
(`_straightside`, `_endside`, `_tfront`, `_tside`, `_endrear`) are NOT loaded by legacy code — they are resolved
by the new `WallFacingResolver` (task #7) only.

**FLAG 7 — Floor uses NO rotation (sprite-swap only). Walls use sprite-swap + flipX, residual Z forced to 0.**
Floor: 16 pre-resolved silhouettes, zero rotation ever (matches §0 no-Z-rotate principle by pre-baking the shape).
Storage: `RoomData.floorCells : List<TileCellRecord{assetGuidOrName, cell, worldPos, rotation, scale}>` (verified).
Compose via existing `RoomDataComposer.ComposeTileCells(room.floorCells, grid, floorParent, RoomLayer.Floor)` — no
composer change. Paint/mutate via existing `RoomDataMutator.PutTileCell` / `RemoveTileCell` (verified) — extend the
sink to re-resolve the 9-cell neighborhood (§5.3).

---

## §1. FINAL ASSET LIST (every tile + piece: Wang mask · footprint · pivot · target path)

### 1A. FLOOR TILES — 20 cells (16 Wang core + 3 interior variants + 1 rift), one sheet, sprite-swap (no rotation)

Edge-mask cardinal value uses **`EdgeMask8` bits** `N=1, E=4, S=16, W=64` (FLAG 1). "Present neighbor" = same-layer
floor cell exists → that edge is INTERIOR (blend). Path root: `Assets/Sprites/Environment/IsoKit/floor/`.

| # | File (`floor/`) | Cardinal mask (N,E,S,W present) | EdgeMask8 value | Footprint | Pivot | Role |
|---|---|---|---|---|---|---|
| 0 | `floor_iso_isolated.png` | none | 0 | 1×1 | BottomCenter | standalone, 4 rims capped |
| 1 | `floor_iso_end_N.png` | N | 1 | 1×1 | BottomCenter | open N |
| 2 | `floor_iso_end_E.png` | E | 4 | 1×1 | BottomCenter | open E |
| 3 | `floor_iso_end_S.png` | S | 16 | 1×1 | BottomCenter | open S |
| 4 | `floor_iso_end_W.png` | W | 64 | 1×1 | BottomCenter | open W |
| 5 | `floor_iso_str_NS.png` | N+S | 17 | 1×1 | BottomCenter | vertical run |
| 6 | `floor_iso_str_EW.png` | E+W | 68 | 1×1 | BottomCenter | horizontal run |
| 7 | `floor_iso_cor_NE.png` | N+E | 5 | 1×1 | BottomCenter | inner corner |
| 8 | `floor_iso_cor_SE.png` | E+S | 20 | 1×1 | BottomCenter | inner corner |
| 9 | `floor_iso_cor_SW.png` | S+W | 80 | 1×1 | BottomCenter | inner corner |
| 10 | `floor_iso_cor_NW.png` | N+W | 65 | 1×1 | BottomCenter | inner corner |
| 11 | `floor_iso_T_openN.png` | E+S+W | 84 | 1×1 | BottomCenter | capped N |
| 12 | `floor_iso_T_openE.png` | N+S+W | 81 | 1×1 | BottomCenter | capped E |
| 13 | `floor_iso_T_openS.png` | N+E+W | 69 | 1×1 | BottomCenter | capped S |
| 14 | `floor_iso_T_openW.png` | N+E+S | 21 | 1×1 | BottomCenter | capped W |
| 15 | `floor_iso_fill_a.png` | N+E+S+W | 85 | 1×1 | BottomCenter | interior fill A (default) |
| 16 | `floor_iso_fill_b.png` | (interior variant) | 85 | 1×1 | BottomCenter | interior fill B (hash-picked) |
| 17 | `floor_iso_fill_c.png` | (interior variant) | 85 | 1×1 | BottomCenter | interior fill C (hash-picked) |
| 18 | `floor_iso_fill_rift.png` | (interior, hand/scatter) | 85 | 1×1 | BottomCenter | cyan rift seam interior (<15%) |
| 19 | `floor_iso_reserved.png` | — | — | — | — | empty/transparent (slice slot) |

Interior cell (#15) deterministically picks a/b/c by `cell.GetHashCode()` for stable non-repeating fields; rift
(#18) is hand-toggled or scattered <15%. 16 core silhouettes are the only Wang-resolved set.

### 1B. WALL PIECES — 18 unique sprites (12 structural + 6 functional) → ~26 facings via flipX

Path root: `Assets/Sprites/Environment/IsoKit/walls/`. **`basename` for the F2 palette = `keepwall`** (the 6
suffix names below are mandatory; FLAG 6). Functional pieces are separate palette items. All pivot BottomCenter.

**Structural (Wang-resolved, consumed by `WallRunBuilder` via `Resolve4` shapes):**

| Piece | File (`walls/`) | `Resolve4` mask (NESW, N=1 E=2 S=4 W=8) | WangShape | Footprint | flipX siblings | Pivot |
|---|---|---|---|---|---|---|
| Straight (rear run) | `keepwall_straight.png` | E+W = 10 | Straight | 1×1 | — | BottomCenter |
| Straight (side run) | `keepwall_straightside.png` | N+S = 5 | Straight@90 | 1×1 | — | BottomCenter |
| Corner | `keepwall_corner.png` | S+E=4+2=6 (canon @0) | Corner | 1×1 | flipX→S+W; reuse for N-corners | BottomCenter |
| T (openN, rear junction) | `keepwall_t.png` | W+E+S = 14 | T | 1×1 | — | BottomCenter |
| T (openS, front junction) | `keepwall_tfront.png` | N+E+W = 11 | T@180 | 1×1 | — | BottomCenter |
| T (openE/openW side) | `keepwall_tside.png` | N+S+W=13 / N+E+S=7 | T@270 / @90 | 1×1 | flipX → openW | BottomCenter |
| Cross | `keepwall_cross.png` | NESW = 15 | Cross | 1×1 | — | BottomCenter |
| End (cap-S, front) | `keepwall_end.png` | S = 4 | End | 1×1 | — | BottomCenter |
| End (cap-N, rear) | `keepwall_endrear.png` | N = 1 | End@180 | 1×1 | — | BottomCenter |
| End (cap-E/cap-W side) | `keepwall_endside.png` | E=2 / W=8 | End@90 / @270 | 1×1 | flipX → cap-W | BottomCenter |
| Single | `keepwall_single.png` | 0 | Single | 1×1 | — | BottomCenter |

(12 unique structural sprites = straight, straightside, corner, t, tfront, tside, cross, end, endrear, endside,
single = 11 named + base `keepwall` sprite alias to `_straight` = the 12th `sprite` field. flipX → 16 facings.)

**Functional (ADIM-1 groups 1/4/5/6; hand-placed via F2 click, snap to iso grid, NOT Wang-resolved):**

| ADIM-1 group | File (`walls/`) | Footprint | Pivot | flipX siblings | Placement |
|---|---|---|---|---|---|
| 1 Connector/Column | `keepcolumn.png` | 1×1 (TALL overhang ~3 cells) | BottomCenter | flipX | single-cell, 1-cell occupancy (FLAG 4) |
| 4 Door/Arch | `keeparch.png` | **2×1 wide** | BottomCenter of 2-cell span | flipX | 2-cell occupancy, span-midpoint offset (§6.4) |
| 5 Low Front Edge | `keeplow_mid.png` | 1×1 low (~0.6 cell) | BottomCenter | flipX | front diagonals; `End` caps at mouth |
| 5 Low corner (opt) | `keeplow_corner.png` | 1×1 low | BottomCenter | flipX | front-edge corners |
| 6 Seam/Filler A | `keepseam_a.png` | 1×1 | BottomCenter | — | scatter over gaps |
| 6 Seam/Filler B | `keepseam_b.png` | 1×1 | BottomCenter | — | scatter |
| 6 Seam/Filler C | `keepseam_c.png` | 1×1 | BottomCenter | — | scatter |

Group 2 (Rear Wall) = reuse `keepwall_straight`. Group 3 (Corner/Turn) = reuse `keepwall_corner`. No separate assets.

**GRAND TOTAL: 20 floor cells (16 Wang core) + 18 wall sprites = correct logical minimum the masks emit.**

---

## §2. EXACT cx IMAGEGEN PROMPTS (ChatGPT image_gen via cx — ref-style, on-brand, transparent)

cx = ChatGPT image_gen via codex. Refs are ChatGPT-made → this exact look is achievable. If cx returns oversized,
nearest-neighbor downscale to the stated px BEFORE slicing. Generate in this order: FLOOR sheet → WALL sheet →
column → arch.

### PROMPT A — FLOOR sheet `floor_iso_sheet.png` (640×256, 5×4 @ 128×64)
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

### PROMPT B — WALL sheet `keepwall_sheet.png` (1024×1024, 4×4 @ 256×256)
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

### PROMPT C — COLUMN `keepcolumn.png` (256×384, individual)
```
Single transparent PNG, 256x384, one tall RUINED KEEP support COLUMN, 2D 3/4 isometric (2:1 dimetric), hand-painted
flat-painterly, blackened charcoal iron-stone palette (#1C1D24-#2E303F, top cap #3A3D4E, base shadow #0E0F14).
Three faces visible (lit top cap, tall front face, dark side face). Chipped, cracked, ruined. The FOOTING diamond
is NARROW (one floor-cell wide) at the bottom-center of the canvas; the column rises tall with headroom above the
foot. Faint cyan #00FFCC rune glow at one crack only, sparing. Cool top key + warm south fill. Alpha transparent,
no background, no magenta, no text.
```

### PROMPT D — ARCH `keeparch.png` (384×320, individual, 2-cell wide)
```
Single transparent PNG, 384x320, one RUINED KEEP doorway ARCH spanning TWO floor cells wide, 2D 3/4 isometric
(2:1 dimetric), hand-painted flat-painterly, blackened charcoal iron-stone palette (#1C1D24-#2E303F, lit top cap,
#0E0F14 base shadow). Two stone jambs + a broken arch lintel, three faces visible per jamb (top cap, front, dark
side). A glowing cyan #00FFCC seal-rift energy fills the opening (the ONLY strong cyan in the pack), faint and flat,
no bloom gloss. Footing diamonds at bottom-center spanning the 2-cell base. Cool top key + warm fill. Alpha
transparent, no background, no magenta, no text.
```

(If cx bleeds cells on either sheet → fall back to per-piece fixed-canvas gens; the slice rects in §3 stay
identical. If floor rims read as rounded blobs vs refs → regenerate Prompt A with "subtle hairline grout, NOT
rounded blob corners".)

---

## §3. SPLIT TECHNIQUE + PER-CELL PIXEL COORDS + SPLITTABLE-vs-INDIVIDUAL VERDICT

Technique = **math-grid crop, no chroma, alpha-native** (uniform cells, alpha separates). Top-left origin in the
crop math; Unity sprite slicing is bottom-left but we ship pre-split PNGs so the importer treats each as Single.

### 3A. FLOOR — SPLITTABLE ✅ (verdict: identical 1×1 footprint + identical foot anchor → grid-safe)
- `TILE_W=128, TILE_H=64, COLS=5, ROWS=4`, sheet 640×256. Crop rect `(c*128, r*64, 128, 64)`, `c=i%5, r=i//5`.
```
idx0  (0,0,128,64)    idx1  (128,0,128,64)   idx2  (256,0,128,64)   idx3  (384,0,128,64)   idx4  (512,0,128,64)
idx5  (0,64,128,64)   idx6  (128,64,128,64)  idx7  (256,64,128,64)  idx8  (384,64,128,64)  idx9  (512,64,128,64)
idx10 (0,128,128,64)  idx11 (128,128,128,64) idx12 (256,128,128,64) idx13 (384,128,128,64) idx14 (512,128,128,64)
idx15 (0,192,128,64)  idx16 (128,192,128,64) idx17 (256,192,128,64) idx18 (384,192,128,64) idx19 (512,192,128,64)
```

### 3B. WALL Sheet 1 — SPLITTABLE ✅ (verdict: identical 1×1 footprint + same baked foot-Y → grid-safe)
- `TILE=256, COLS=4, ROWS=4`, sheet 1024×1024. Crop rect `(c*256, r*256, 256, 256)`, `c=i%4, r=i//4`.
```
row0: keepwall_straight(0,0) keepwall_straightside(256,0) keepwall_corner(512,0) keepwall_t(768,0)
row1: keepwall_tfront(0,256) keepwall_tside(256,256) keepwall_cross(512,256) keepwall_end(768,256)
row2: keepwall_endrear(0,512) keepwall_endside(256,512) keepwall_single(512,512) keeplow_mid(768,512)
row3: keeplow_corner(0,768) keepseam_a(256,768) keepseam_b(512,768) keepseam_c(768,768)
```
(All rects `w=256,h=256`.)

### 3C. INDIVIDUAL (verdict: geometric necessity — escape the uniform grid)
- `keepcolumn.png` — **INDIVIDUAL 256×384.** Reason: ~3-cell vertical overhang clips any 256-tall uniform cell.
- `keeparch.png` — **INDIVIDUAL 384×320.** Reason: 2-cell-wide footprint does not fit a 1×1 uniform cell.

### 3D. SLICE SCRIPTS (drop-in, Pillow)
Floor → `Assets/Sprites/Environment/IsoKit/floor/`:
```python
from PIL import Image
TW,TH,COLS = 128,64,5
N=["isolated","end_N","end_E","end_S","end_W","str_NS","str_EW","cor_NE","cor_SE","cor_SW",
   "cor_NW","T_openN","T_openE","T_openS","T_openW","fill_a","fill_b","fill_c","fill_rift","reserved"]
img=Image.open("floor_iso_sheet.png").convert("RGBA")
for i,n in enumerate(N):
    c,r=i%COLS,i//COLS
    img.crop((c*TW,r*TH,c*TW+TW,r*TH+TH)).save(f"floor_iso_{n}.png")
```
Wall sheet → `Assets/Sprites/Environment/IsoKit/walls/`:
```python
from PIL import Image
T,COLS=256,4
N=["keepwall_straight","keepwall_straightside","keepwall_corner","keepwall_t",
   "keepwall_tfront","keepwall_tside","keepwall_cross","keepwall_end",
   "keepwall_endrear","keepwall_endside","keepwall_single","keeplow_mid",
   "keeplow_corner","keepseam_a","keepseam_b","keepseam_c"]
img=Image.open("keepwall_sheet.png").convert("RGBA")
for i,n in enumerate(N):
    c,r=i%COLS,i//COLS
    img.crop((c*T,r*T,c*T+T,r*T+T)).save(f"{n}.png")
```
Column + arch ship as-is (already individual). After slicing, set `keepwall_straight.png` ALSO copied/aliased so
the F2 palette base `keepwall` resolves a base `sprite` (the loader uses `_straight` as the straightSprite and the
plain registry sprite as `sprite`; ship a `keepwall.png` = copy of `keepwall_straight.png` so the base entry exists).

---

## §4. WANG PIECE → FILE → RESOLVED SHAPE/ROTATION MAP (verified vs CCW table; no-Z-rotate aware)

`Resolve4` emits `(WangShape, θ, neighborMask)`. Because depthful iso CANNOT Z-rotate (FLAG 3), the
`WallFacingResolver` (task #7) consumes `WangResult` and returns `(spriteFile, flipX, residualZ=0)`. **Every
residual Z = 0** for depthful art — the shape is pre-baked into the chosen sprite + flipX. Verified row-by-row
against the locked CCW table (FLAG 2 — all match):

| `Resolve4` output (shape @ θ, mask NESW) | sprite file | flipX | residual Z |
|---|---|---|---|
| Straight @0 (E+W=10) | `keepwall_straight` | no | 0 |
| Straight @90 (N+S=5) | `keepwall_straightside` | no | **0 (swap, not rotate)** |
| Corner @0 (S+E=6) | `keepwall_corner` | no | 0 |
| Corner @270 (S+W=12) | `keepwall_corner` | **yes** | 0 |
| Corner @90 (N+E=3) | `keepwall_corner` (reads back-corner) | no | 0 |
| Corner @180 (N+W=9) | `keepwall_corner` | **yes** | 0 |
| T @0 (openN, W+E+S=14) | `keepwall_t` | no | 0 |
| T @180 (openS, N+E+W=11) | `keepwall_tfront` | no | 0 |
| T @270 (openE, N+S+W=13) | `keepwall_tside` | no | 0 |
| T @90 (openW, N+E+S=7) | `keepwall_tside` | **yes** | 0 |
| Cross @0 (NESW=15) | `keepwall_cross` | no | 0 |
| End @0 (S=4) | `keepwall_end` | no | 0 |
| End @180 (N=1) | `keepwall_endrear` | no | 0 |
| End @90 (E=2) | `keepwall_endside` | no | 0 |
| End @270 (W=8) | `keepwall_endside` | **yes** | 0 |
| Single (0) | `keepwall_single` | no | 0 |

**Verification note:** mask values use `Resolve4` bits `N=1,E=2,S=4,W=8` (e.g. S+E = 4+2 = 6). The CCW θ for each
row was cross-checked against the live `switch` in `WangResolver.cs` lines 52-89 and the locked context table —
**zero mismatches.** The only "rotation" the spec context lists that we do NOT physically apply is the Z-rotate;
it is absorbed into sprite-swap + flipX, which is the correct handling for depthful vertical-faced art.

**Wiring (one `WallPiece` for basename `keepwall`):** `sprite = keepwall` (copy of straight), `straightSprite =
keepwall_straight`, `cornerSprite = keepwall_corner`, `tSprite = keepwall_t`, `crossSprite = keepwall_cross`,
`endSprite = keepwall_end`, `singleSprite = keepwall_single` (loader auto-fills these via `_<suffix>`, FLAG 6).
The `_straightside/_tfront/_tside/_endrear/_endside` + flipX are selected ONLY by the new `WallFacingResolver`.
**Pre-#7 behavior:** legacy path Z-rotates placeholders (acceptable). Post-#7: depthful swap, Z=0.

---

## §5. FLOOR AUTO-MERGE (resolver LUT + paint sink) — same-layer Wang blend

### 5.1 No new resolver. Reuse `WangResolver.EdgeMask8`. Add a floor LUT helper beside it.
### 5.2 Cardinal→tile LUT (against `EdgeMask8` bits — FLAG 1)
```csharp
// New file: Assets/Scripts/RoomPainter/FloorWangResolver.cs
public static class FloorWangResolver
{
    // EdgeMask8 cardinal bits: N=1, E=4, S=16, W=64. Collapse to a 0..15 key.
    public static int ResolveFloorTile(Vector3Int cell, System.Func<Vector3Int,bool> sameLayerOccupied)
    {
        int m = WangResolver.EdgeMask8(cell, sameLayerOccupied);
        bool n=(m&1)!=0, e=(m&4)!=0, s=(m&16)!=0, w=(m&64)!=0;
        int key=(n?1:0)|(e?2:0)|(s?4:0)|(w?8:0); // 0..15
        return key; // map key -> file name via FloorTileName[key]
    }
    // key bit order here: N=1,E=2,S=4,W=8 (local compact). Map -> sprite index:
    public static readonly string[] FloorTileName = new string[16]{
        "isolated",        // 0  none
        "end_N",           // 1  N
        "end_E",           // 2  E
        "cor_NE",          // 3  N+E
        "end_S",           // 4  S
        "str_NS",          // 5  N+S
        "cor_SE",          // 6  E+S
        "T_openW",         // 7  N+E+S (capped W)
        "end_W",           // 8  W
        "cor_NW",          // 9  N+W
        "str_EW",          // 10 E+W
        "T_openS",         // 11 N+E+W (capped S)
        "cor_SW",          // 12 S+W
        "T_openE",         // 13 N+S+W (capped E)
        "T_openN",         // 14 E+S+W (capped N)
        "fill_a"           // 15 N+E+S+W interior
    };
    // interior (15): pick fill_a/b/c by cell hash; rift hand-toggled. assetName = "floor_iso_" + name.
}
```
(Index 15 interior → `fill_a/b/c` via `((cell.x*73856093)^(cell.y*19349663))%3`. Rift = explicit scatter <15%.)

### 5.3 Paint sink — re-resolve 9-cell neighborhood, rewrite `floorCells`
On any floor add/remove (sink: `Assets/Editor/RoomPainter/Authoring/RoomDataPlacementSink.cs`, mutate via
`RoomDataMutator.PutTileCell`/`RemoveTileCell`):
1. Maintain `HashSet<Vector3Int>` of the layer's occupied floor cells.
2. On add/remove, re-resolve the dirty cell + 8 neighbors (9 total) with `FloorWangResolver.ResolveFloorTile`.
3. For each, rewrite that cell's `TileCellRecord.assetGuidOrName = "floor_iso_" + name`, rotation=0, scale=1.
4. `RoomDataComposer.ComposeTileCells(room.floorCells, grid, floorParent, RoomLayer.Floor)` rebuilds SRs — NO
   composer change, NO rotation. Pure sprite-swap (the splittable-first payoff).

---

## §6. CODE PATCHES (minimal, deterministic)

### 6.1 Importer — add IsoKit path rules (FLAG 5). File: `Assets/Editor/AssetPackV3Importer.cs`
Add two roots + PPU branch:
```csharp
private const string IsoWallsRoot = "Assets/Sprites/Environment/IsoKit/walls/";
private const string IsoFloorRoot = "Assets/Sprites/Environment/IsoKit/floor/";
private const int IsoFloorPPU = 128; // 128px-wide 2:1 diamond -> 1 world unit
// in OnPreprocessTexture: bool isIsoWall=path.StartsWith(IsoWallsRoot); bool isIsoFloor=path.StartsWith(IsoFloorRoot);
// if (!isWall && !isFloor && !isIsoWall && !isIsoFloor) return;
// ti.spritePixelsPerUnit = (isFloor) ? FloorPPU : (isIsoFloor) ? IsoFloorPPU : WallsPPU;  // walls/isowalls = 64
```
All other settings (Point, RGBA32, no-mip, alphaIsTransparency, Tight, extrude=1, BottomCenter) already correct
and apply to IsoKit too. Verify: a placed floor tile spans exactly 1 cell, no seam to neighbor.

### 6.2 `FloorWangResolver.cs` — NEW (§5.2). Beside `Assets/Scripts/RoomPainter/WangResolver.cs`.

### 6.3 `WallFacingResolver.cs` — NEW (task #7). Beside `WallRunBuilder.cs`. Signature:
```csharp
public static (Sprite sprite, bool flipX) ResolveFacing(WallPiece piece, WangResult r);
```
Maps §4 table; hook into `WallRunBuilder.PlaceOne` to set `sr.flipX` and force `Quaternion.Euler(0,0,0)` when a
facing sprite is returned (depthful). Keep legacy `SpriteForShape` Z-rotate as the placeholder fallback.

### 6.4 `WallRunBuilder` arch span (FLAG 4). File: `Assets/Scripts/DevTools/WallRunBuilder.cs`
For `piece.footprint.x > 1`: (a) register BOTH cells in `occupied`/`previewOccupied`; (b) in `PlaceOne` add
span-midpoint offset `pos += 0.5*(FootPosition(cell+dir) - FootPosition(cell))`. Run-step `step=footprint.x=2`
already skips the second cell (verified line 31). Tall column footprint(1,1) needs nothing.

---

## §7. COMPOSITION RECIPE + CAMERA VALUES

### 7.1 Diamond room — ~13×10 footprint (open-front, ADIM-5)
- Iso diamond 13 wide (NE↔SW) × 10 deep (NW↔SE). Origin pivot = S-most cell (bottom-center). FRONT (S/SE/SW)
  left OPEN to camera (min 3 cells open) — nothing occludes the hero.
- Rear-Left (NW) + Rear-Right (NE) = `Straight` runs + `Corner` at apex (flipX mirror). Back apex N = `keeparch`
  landmark (cyan seal-rift). Side-L/R (W/E) = `Straight` + `T` spurs, leave 2-3 cell void-gaps (ruined, min-3-then-break).
- Front-L/R (SE/SW) = `keeplow_mid` low edge + `End` caps at mouth. Seams = `keepseam_a/b/c` over ugly gaps (NOT
  the deliberate void-gaps).
- Interior 3 zones: center 3×3 hero zone GEOMETRY-FREE (cyan seal-glyph here, the money shot) · mid-ring 4-6
  `keepcolumn` on ODD cells inset ~2 from walls · outer ring sarcophagi/rubble + 6 brazier pedestals.

### 7.2 Lighting (URP 2D Light2D)
- Global: void-violet `#1F0C2B`, intensity `0.18`. Outside diamond = pure black `#000000` (do NOT light void).
- 6 brazier Point lights `#FF8030`, intensity `1.15`, outer `3.2`, inner `0.4`, falloff `0.6`, y+0.6 at flame;
  placement: 2 flank arch apex, 1 each rear-wall mid-run (NW/NE), 2 front-corner `End` caps. Flicker 1.0↔1.25, 0.1-0.2Hz.
- Cyan rift Point `#00FFCC`, intensity `0.9`, outer `1.4`, on emissive crack decals; total cyan <15% lit area;
  center concentric glyph (strongest) + branching veins to arch + isolated rift pockets near void-gaps; pulse
  0.6↔0.95 ~0.3Hz; one cyan point inside the arch keystone.

### 7.3 Camera (locked PPC standard — DO NOT hand-set orthographicSize)
- Pixel-Perfect Camera: `refResolution 640×360`, `assetsPPU 64`, upscaleRT ON, pixelSnapping OFF, cropFrame None.
- Effective half-height ≈ `360/2/64 = 2.81` world units (tighter than legacy 5.15 → "inside a room" framing).
- 13×10 diamond ≈ 11.5w × 5.75d world units; frame ≈ 9.1×5.1 → hero centered, walls crop at edges (matches
  `16_12_48 (4)`). Zoom = edit refResolution ONLY (16:9): tighter `560×315`, wider `768×432`. Never touch ortho.
- Apply refRes to the LIVE camera PPC (`Assets/Scripts/Player/CameraFollow.cs` is `[Obsolete]`; use the live spine
  camera). Keep bounds-clamp (`autoBoundsFromFloorTilemap`) so the open mouth never reveals void below the hero.

### 7.4 Sorting (already correct — lock, do not fight)
- `GraphicsSettings.transparencySortMode = CustomAxis (0,1,0)` (enforced by `GraphicsSettingsBootstrap`).
- ALL walls/columns/props/enemies/hero: sortingLayer `Entities`, order 0, `SpriteSortPoint.Pivot`, pivot
  BottomCenter (verified `WallRunBuilder.ApplySpriteRules`). Foot-Y drives mutual occlusion.
- Do NOT enable `IsoSorter` manual-order under Custom-Axis. Tall rear walls → `WallOcclusionFader` (~0.4 alpha
  when hero behind).

### 7.5 Character (FIXED PixelLab high-3/4 8-dir, `Assets/Resources/Characters`, PPU64 — do NOT redraw)
- Scale 1:1 (no rescale). Hero ~1.6-1.8 cells tall, ~⅔ a column. Columns authored ~2.2-2.5× hero height.
- Pivot bottom-center (feet) → Custom-Axis-Y uses feet contact. Best in center 3×3 on the cyan glyph.
- Add soft radial drop-shadow/AO blob under hero + each enemy (sorted just under feet) to seat the sprite on the
  flat floor (kills "floating sticker"). Foot-circle collider for movement + separate body hurtbox (locked spec).

---

## §8. EXECUTION CHECKLIST (ordered, deterministic)

1. **Generate** — cx Prompt A → `floor_iso_sheet.png` (640×256); Prompt B → `keepwall_sheet.png` (1024×1024);
   Prompt C → `keepcolumn.png` (256×384); Prompt D → `keeparch.png` (384×320). Downscale NN to spec if oversized.
2. **Split** — run §3D floor script → 20 PNGs to `IsoKit/floor/`; wall script → 16 PNGs to `IsoKit/walls/`; drop
   `keepcolumn.png` + `keeparch.png` into `IsoKit/walls/`; copy `keepwall_straight.png` → `keepwall.png` (base).
3. **Import** — patch `AssetPackV3Importer.cs` per §6.1 (IsoKit rules, floor PPU 128, walls PPU 64). Reimport;
   verify Point/RGBA32/BottomCenter/alphaIsTransparency applied; verify 1 floor tile = exactly 1 cell, no seam.
4. **Code** — add `FloorWangResolver.cs` (§5.2); add `WallFacingResolver.cs` + hook (§6.3, task #7); add arch-span
   handling (§6.4). Compile-clean in Unity, check console (no red).
5. **Set Grid** — scene Grid `cellLayout=Isometric`, `cellSize=(1,0.5,1)`. Confirm `GetCellCenterWorld` diamonds.
6. **Compose** — paint floor Wang terrain (center 3×3 clear) → rear-wall chains on far diagonals + Corners @ apex →
   `keeparch` @ N apex → `keeplow_mid` front diagonals (S mouth open) → scatter 4-6 `keepcolumn` mid-ring odd cells
   → `keepseam_*` over gaps. RoomData persists via `floorCells`/wall segments; compose via `RoomDataComposer`.
7. **Light** — Global void-violet 0.18; 6 braziers (§7.2) + point-lights; cyan seal-glyph + sparse rift cracks;
   void stays black.
8. **Camera + hero** — PPC refRes 640×360 on live camera; drop hero 1:1 on center glyph; add AO blobs.
9. **Render / verify** — F5 smoke: walls connect (no sideways faces post-#7), floor blends seamless, occlusion
   correct (hero between columns), cyan <15%, camera "inside-room". Compare to `00_18_45 (1)` / `21_29_23 (2)`.

---

## §9. KEY FILES (absolute)
- `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\RoomPainter\WangResolver.cs` (reuse `EdgeMask8`+`Resolve4`; canonical, do not edit)
- `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\RoomPainter\FloorWangResolver.cs` (NEW — §5.2)
- `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\DevTools\WallRunBuilder.cs` (arch span §6.4; facing hook §6.3)
- `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\DevTools\WallFacingResolver.cs` (NEW — task #7, §6.3)
- `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Editor\AssetPackV3Importer.cs` (IsoKit rules §6.1)
- `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\RoomPainter\RoomPlacementTypes.cs` (`WallPiece` fields)
- `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\DevTools\InPlayMapPaintOverlay.cs` (`VariantSprite` naming, line 810)
- `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Editor\RoomPainter\Authoring\RoomDataPlacementSink.cs` (floor paint sink §5.3)
- `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Editor\RoomPainter\Authoring\RoomDataComposer.cs` (`ComposeTileCells`, no change)
- `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\RoomPainter\RoomDataMutator.cs` (`PutTileCell`/`RemoveTileCell`)
- Sprite drop: `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Sprites\Environment\IsoKit\floor\` + `\walls\`
- Refs: `STAGING\concepts\chatgpt_ref\ChatGPT Image 25 May 2026 00_18_45 (1).png`, `...\new_chatgpt\ChatGPT Image 23 May 2026 21_29_23 (2).png`, `...\ChatGPT Image 22 May 2026 16_12_48 (4).png`, ADIM-1 `...\blueprint_room\ChatGPT Image 24 May 2026 23_42_09 (1).png`, ADIM-5 `...\blueprint_room\ChatGPT Image 24 May 2026 23_42_10 (5).png`

---

## §10. COUNTS + VERDICT + RISKS
- **Floor:** 1 sheet (640×256) → 20 cells (16 Wang core + 3 interior + 1 rift). SPLITTABLE.
- **Walls:** 1 sheet (1024×1024) → 16 cells SPLITTABLE; + 2 INDIVIDUAL (column 256×384, arch 384×320). 18 unique → ~26 facings via flipX.
- **Overall verdict: SPLITTABLE-FIRST.** Everything math-grids EXCEPT the tall column (3-cell overhang) and
  2-wide arch — both INDIVIDUAL by geometric necessity (vertical/horizontal overflow of a uniform 1×1 cell). 36 of
  38 assets clean-split from 2 sheets.
