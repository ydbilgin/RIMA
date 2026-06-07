#!/usr/bin/env python3
"""
Post-process the 3 regenerated raw images:
1. Downscale (NEAREST) to target size
2. Chroma-key: magenta (#FF00FF +-tolerance) -> alpha=0
3. Defringe: remaining edge magenta pixels -> alpha=0
4. Save over the target file
"""
from PIL import Image
import os

FOLDER = r"F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\imagegen\assets\batch3_2026-06-07"

JOBS = [
    {
        "raw": "raw_scratches.png",
        "target": "ground_decal_combat_scratches.png",
        "size": (64, 32),
    },
    {
        "raw": "raw_scorch.png",
        "target": "ground_decal_portal_scorch.png",
        "size": (128, 64),
    },
    {
        "raw": "raw_rim_glow.png",
        "target": "hole_rim_glow.png",
        "size": (64, 32),
    },
]

def chroma_key(img, tolerance=30):
    """Remove magenta background: pixels where R>200, G<80, B>150."""
    img = img.convert("RGBA")
    data = img.load()
    w, h = img.size
    removed = 0
    for y in range(h):
        for x in range(w):
            r, g, b, a = data[x, y]
            if r > 200 and g < 80 and b > 150:
                data[x, y] = (0, 0, 0, 0)
                removed += 1
    return img, removed

def defringe(img):
    """Remove edge magenta contamination."""
    data = img.load()
    w, h = img.size
    removed = 0
    for y in range(h):
        for x in range(w):
            r, g, b, a = data[x, y]
            if a > 0 and r > 150 and g < 80 and b > 100:
                data[x, y] = (0, 0, 0, 0)
                removed += 1
    return img, removed

for job in JOBS:
    raw_path = os.path.join(FOLDER, job["raw"])
    target_path = os.path.join(FOLDER, job["target"])

    print(f"\nProcessing: {job['raw']} -> {job['target']} ({job['size'][0]}x{job['size'][1]})")

    # 1. Open raw
    img = Image.open(raw_path).convert("RGBA")
    orig_w, orig_h = img.size
    print(f"  Raw size: {orig_w}x{orig_h}")

    # 2. Downscale with NEAREST
    img = img.resize(job["size"], Image.NEAREST)
    print(f"  After downscale: {img.size}")

    # 3. Chroma-key
    img, chroma_removed = chroma_key(img)
    print(f"  Chroma-key removed: {chroma_removed} pixels")

    # 4. Defringe
    img, fringe_removed = defringe(img)
    print(f"  Defringe removed: {fringe_removed} pixels")

    # 5. Check non-transparent pixels remain
    data = img.load()
    w, h = img.size
    opaque = sum(1 for y in range(h) for x in range(w) if data[x, y][3] > 0)
    print(f"  Non-transparent pixels remaining: {opaque} / {w*h}")

    # 6. Save
    img.save(target_path)
    print(f"  Saved: {target_path}")

print("\nDone!")
