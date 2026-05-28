# pixel_cleanup.py

Pure Python cleanup and reporting for AI-generated pixel art PNGs. It detects stray components, palette outliers, semi-transparent edges, and local color noise without any AI calls or credit cost.

## Quick Start

```bash
cd "F:/Antigravity Projeler/2d roguelite/RIMA/Tools/pixel_cleanup"
python pixel_cleanup.py --input input.png --palette palette_examples/rima_shattered_keep.json --report report.json --preview preview.png
```

Default mode is report-only. It writes reports, masks, and previews when those paths are requested, but it does not write a cleaned PNG unless `--apply_cleanup` is present.

```bash
python pixel_cleanup.py --input input.png --output cleaned.png --palette palette_examples/rima_shattered_keep.json --remove_stray --snap_to_palette --fix_color_noise --apply_cleanup
```

## Batch Usage

```bash
python pixel_cleanup.py --input_dir assets/raw --output_dir assets/cleaned --palette palette_examples/rima_shattered_keep.json --apply_cleanup
```

Batch mode writes one cleaned PNG per input when `--apply_cleanup` is set, plus one `*_report.json` and mask exports per source image.

## Common Flags

- `--alpha_threshold 128`: semi-transparent pixels below the threshold become transparent, pixels at or above it become opaque.
- `--keep_alpha`: skip alpha binarization for VFX assets.
- `--min_component_area 4`: disconnected alpha components below this area are stray pixels.
- `--remove_stray`: remove detected stray components when cleanup is applied.
- `--palette_tolerance 24`: max RGB Euclidean distance allowed from the palette.
- `--snap_to_palette`: snap palette outliers to their nearest palette color.
- `--noise_threshold 40`: local RGB distance threshold for 3x3 noise checks.
- `--fix_color_noise`: replace detected noise pixels with a local median color.
- `--apply_cleanup`: write the cleaned PNG.
- `--force`: allow cleaned PNG overwrite.
- `--make_palette 32`: extract the most common colors to `suggested_palette.json` when no palette is supplied.

## Palette Format

```json
{
  "name": "rima_shattered_keep",
  "colors": [
    [26, 22, 18],
    [43, 33, 24]
  ]
}
```

Colors are RGB arrays with channel values from 0 to 255.

## Preview Output

`--preview preview.png` creates a contact sheet with the original image, a combined overlay, and separate masks:

- red: stray pixels
- magenta: palette outliers
- yellow: semi-transparent pixels
- cyan: local color noise

Mask files use the names `mask_stray.png`, `mask_palette_outliers.png`, `mask_alpha.png`, and `mask_color_noise.png` in single-image mode. Batch mode prefixes each file with the source image stem.

## RIMA Integration

Use this before Unity import in the wall production pipeline:

1. Run report-only on PixelLab or image-generation output.
2. Check `recommended_actions` in the JSON report.
3. Re-run with `--apply_cleanup` and only the needed cleanup flags.
4. Import the cleaned PNG into Unity.

Example wall pass:

```bash
python pixel_cleanup.py --input wall_nw_mid_plain.png --palette palette_examples/rima_shattered_keep.json --report report.json --preview preview.png
python pixel_cleanup.py --input wall_nw_mid_plain.png --output wall_nw_mid_plain_clean.png --palette palette_examples/rima_shattered_keep.json --remove_stray --snap_to_palette --apply_cleanup
```
