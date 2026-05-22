"""
Process raw 1024x1024 imagegen output:
1. Alpha mask: detect pure black BG, convert to transparent
2. Tight crop to non-transparent bounding box
3. Downsample to canonical pixel size using NEAREST
4. Save final PNG with alpha
"""
import sys

import numpy as np
from PIL import Image


def process(raw_path, out_path, target_size):
    img = Image.open(raw_path).convert("RGBA")
    arr = np.array(img)

    rgb = arr[..., :3].astype(int)
    luma = rgb.sum(axis=-1)
    alpha = np.where(luma > 30, 255, 0).astype(np.uint8)
    arr[..., 3] = alpha

    masked = Image.fromarray(arr, "RGBA")
    bbox = masked.getbbox()
    if not bbox:
        raise RuntimeError(f"No non-transparent pixels in {raw_path}")

    masked = masked.crop(bbox)
    final = masked.resize(target_size, Image.Resampling.NEAREST)
    final.save(out_path)
    print(f"{raw_path} -> {out_path} ({target_size}, bbox={bbox})")


if __name__ == "__main__":
    asset_id, raw_path, out_path, w, h = sys.argv[1:6]
    process(raw_path, out_path, (int(w), int(h)))
