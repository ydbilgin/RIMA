# Codex Task — Slice Wall Pack v3 (Pure PixelLab Output)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev

Final wall pack `STAGING/_pixellab_outputs/walls/v2/act1_wall_pure_pixellab_v3_clean.png` (512×512 RGBA) içindeki ~25 individual wall tile'ı alpha-bbox detection ile slice et + Unity tile asset olarak hazırla.

## Input

`STAGING/_pixellab_outputs/walls/v2/act1_wall_pure_pixellab_v3_clean.png`

Layout (visual inspection):
- **Row 1 (Features, top):** 4 tile ~128×170 each — archway_NE, archway_SE, column, wall_hero
- **Row 2 (Straight/Corner modular):** 6 tile ~70×70 each
- **Row 3 (Junctions/Specials):** 6-7 tile ~70×70 each
- **Row 4 (Low walls + Foundation, bottom):** 5 tile ~70×70 each

Total ~22-25 unique wall tiles.

## Output

Klasör: `Assets/Art/AssetPacks/Act1_ShatteredKeep/wall_pack_v3/`

Her tile ayrı PNG, anlamlı isimle:
- `tile_archway_NE.png`
- `tile_archway_SE.png`
- `tile_column_freestanding.png`
- `tile_wall_hero.png`
- `tile_straight_NE.png`
- `tile_straight_SE.png`
- `tile_corner_outer_a.png` through `_d.png`
- `tile_corner_inner_a.png` / `_b.png`
- `tile_T_junction_a.png` through `_d.png`
- `tile_endcap_a.png` / `_b.png`
- `tile_low_wall_straight.png`
- `tile_low_wall_corner.png`
- `tile_low_wall_endcap.png`
- `tile_foundation_a.png` / `_b.png`
- `tile_floor_edge.png`

## Slicing Approach

**Step 1:** Alpha-based connected component detection via PIL + numpy
```python
from PIL import Image
import numpy as np
from scipy import ndimage  # if available, otherwise manual

img = Image.open(input_path).convert('RGBA')
arr = np.array(img)
alpha = arr[:,:,3]
mask = alpha > 30

# Label connected components
labeled, n_components = ndimage.label(mask)

# For each component, find bbox
for i in range(1, n_components + 1):
    component = (labeled == i)
    ys, xs = np.where(component)
    bbox = (xs.min(), ys.min(), xs.max() + 1, ys.max() + 1)
    # Filter tiny noise (area < 100 pixels)
    if component.sum() > 100:
        # Crop with 2px padding
        cropped = img.crop((bbox[0]-2, bbox[1]-2, bbox[2]+2, bbox[3]+2))
        # Save
```

**Step 2:** Group components by Y-row (cluster centers):
- Row 1: Y center 0-170
- Row 2: Y center 170-240
- Row 3: Y center 240-340
- Row 4: Y center 340-512

**Step 3:** Sort left-to-right within each row, assign expected name from list.

**Step 4:** Save each cropped sprite with .png to output dir.

## Verification

- Sliced tile count: report (should be ~22-25)
- Each tile dimensions
- Visual sanity check
- If component count != expected, list discrepancies

## Effort

medium — alpha component detection straightforward, naming based on order.

## Output Confirmation

- Output dir path
- List of all sliced PNGs with dimensions
- Total tile count
- Any anomalies
