# cliff_generator.py

Geometric base generator for RIMA cliff sprites. Produces **64 x 96** RGBA
silhouettes intended as **init images** for PixelLab S-XL New, guaranteeing
correct perspective + silhouette while PixelLab adds texture/lighting.

**Tile-cell-aligned (REVIZE 2026-05-26):** Cliff top edge width is **TAM 1
floor tile cell pixel width = 64 px**, so the cliff aligns exactly with the
64x32 dimetric floor grid. Height = 1.5 cells (top deck 16 px + drop face
80 px).

## Run

```
python tools/cliff_generator.py                 # 10 variants (seeds 1-10)
python tools/cliff_generator.py --count 20      # 20 variants
python tools/cliff_generator.py --seed 42       # single seed
python tools/cliff_generator.py --out path/dir  # custom output
```

Default output: `STAGING/cliff_bases/cliff_v01.png ... cliff_v10.png`

## Output spec

- **64 x 96 px**, RGBA, transparent background
- 3D dimetric mock (top deck + drop face)
- Top deck (y 0-16): lighter grey `#6e6e6e` with jagged top edge (+/-2 px)
- Drop face (y 16-95): mid-dark grey `#464646`
- Outline: `#282828` (sharp dark edge)
- Right side shadow strip (2 px wide): `#323232`
- 1-2 subtle vertical cracks per variant
- Single-tone grey family (PixelLab AI adds texture later)
- Deterministic per seed

## 3D Dimetric Mock Layout

```
y=0   -----------------------------  (transparent above top edge)
y=4   +---- jagged top edge ----+    <- top edge points (+/-2 px)
      |       TOP DECK          |    <- #6e6e6e (light)
y=16  +-------------------------+    <- face starts
      |                         |
      |      DROP FACE          |    <- #464646 (mid-dark)
      |  (subtle 1-2 cracks)    |
      |                       ||     <- right shadow strip #323232
y=95  +-------------------------+    <- sharp outline
      0 px                   63 px
```

## PixelLab S-XL New workflow (Web UI, user manual)

1. Pick a base from `STAGING/cliff_bases/` (try 2-3 seeds for variety, e.g.
   `cliff_v05.png`).
2. PixelLab Web UI -> **S-XL New** -> upload as **Init Image**.
3. **Canvas: 64 x 96** (matches init image).
4. **AI Freedom: 0.3 - 0.4** (low; keep silhouette, add texture only).
5. Prompt:
   `pixel art version, dark grey weathered rock texture, subtle moss patches,
   natural cliff face, sharp pixel edges, no anti-aliasing`
6. Generate -> review -> import to
   `Assets/Sprites/Environment/KitB_Cliff/v2/`.

## Dependencies

- Pillow only (`pip install Pillow`).
