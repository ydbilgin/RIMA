"""flatten_floor_tiles.py — make PixelLab iso floor tiles read as a cohesive floor
TEXTURE instead of individually-popping cobblestones.

Problem: each cobble has strong relief (bright top / dark grout) so stones "pop" one
by one and the surface looks bumpy rather than like one tiled floor.

Fix (no AI gen): pull every pixel toward the tile's per-channel mean (contrast
compression) so grout lines + highlights soften and stones blend. Optional gentle
desaturation unifies colour speckle. Deterministic per-pixel + identical per tile, so
SEAMLESS tiling is preserved. Alpha untouched.

Originals are copied to PixelLabFloor/_orig/ on first run; the script always reads from
_orig so re-running with a different factor never compounds.

Usage:
  python STAGING/flatten_floor_tiles.py [contrast=0.55] [desat=0.15] [lift=0.04]
    contrast: 1.0 = unchanged, 0.5 = half contrast (flatter). Lower = blends more.
    desat:    0.0 = keep colour, 1.0 = grayscale. Small values unify speckle.
    lift:     0..0.2 raises the darkest values so grout isn't near-black.
"""
import os, sys, shutil
import numpy as np
from PIL import Image

DIR = os.path.join("Assets", "Sprites", "Environment", "PixelLabFloor")
ORIG = os.path.join(DIR, "_orig")

contrast = float(sys.argv[1]) if len(sys.argv) > 1 else 0.55
desat    = float(sys.argv[2]) if len(sys.argv) > 2 else 0.15
lift     = float(sys.argv[3]) if len(sys.argv) > 3 else 0.04

os.makedirs(ORIG, exist_ok=True)

tiles = [f for f in os.listdir(DIR) if f.lower().endswith(".png")]
tiles.sort()

processed = 0
for name in tiles:
    src_live = os.path.join(DIR, name)
    src_orig = os.path.join(ORIG, name)
    # back up once, then always read from the pristine original
    if not os.path.exists(src_orig):
        shutil.copy2(src_live, src_orig)
    im = Image.open(src_orig).convert("RGBA")
    arr = np.asarray(im).astype(np.float32)
    rgb = arr[..., :3]
    a = arr[..., 3:4]

    # per-channel mean over opaque pixels
    opaque = (a[..., 0] > 8)
    if opaque.sum() == 0:
        shutil.copy2(src_orig, src_live)
        continue
    mean = rgb[opaque].mean(axis=0)  # (3,)

    # contrast compression toward the mean
    out = mean + (rgb - mean) * contrast

    # gentle desaturation toward per-pixel luminance
    if desat > 0:
        lum = (0.299 * out[..., 0] + 0.587 * out[..., 1] + 0.114 * out[..., 2])[..., None]
        out = out * (1 - desat) + lum * desat

    # lift the floor so grout isn't crushed black
    if lift > 0:
        out = out * (1 - lift) + 255.0 * lift * 0.18  # raise toward a dark-grey, not white

    out = np.clip(out, 0, 255)
    res = np.concatenate([out, a], axis=-1).astype(np.uint8)
    Image.fromarray(res, "RGBA").save(src_live)
    processed += 1

print(f"flattened {processed} tiles (contrast={contrast} desat={desat} lift={lift}); originals in {ORIG}")
