#!/usr/bin/env python3
"""
Defringe: Remove magenta fringe from transparent PNGs.
Magenta-contaminated = alpha>0 AND R>150 AND G<80 AND B>100 (adjustable).
Strategy: set such pixels alpha=0.
Skip void_nebula_sheet.png (opaque, no alpha).
"""
import sys
import os
from PIL import Image
import struct
import zlib

FOLDER = r"F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\imagegen\assets\batch3_2026-06-07"
SKIP = {"void_nebula_sheet.png", "defringe.py"}

def is_magenta_contaminated(r, g, b, a):
    """
    Pixel is magenta-fringe if:
    - alpha is partial (0 < a < 255) or fully opaque but magenta-colored
    - R high, G low, B high (magenta hue)
    We target: R>150, G<80, B>100
    """
    return a > 0 and r > 150 and g < 80 and b > 100

def defringe_image(path):
    img = Image.open(path).convert("RGBA")
    data = img.load()
    w, h = img.size
    changed = 0

    for y in range(h):
        for x in range(w):
            r, g, b, a = data[x, y]
            if is_magenta_contaminated(r, g, b, a):
                # Set alpha=0 (remove the fringe pixel)
                data[x, y] = (0, 0, 0, 0)
                changed += 1

    if changed > 0:
        img.save(path)

    return changed, w, h

def main():
    files = sorted(f for f in os.listdir(FOLDER)
                   if f.lower().endswith('.png') and f not in SKIP)

    total_changed = 0
    results = []

    for fname in files:
        path = os.path.join(FOLDER, fname)
        try:
            changed, w, h = defringe_image(path)
            results.append((fname, w, h, changed))
            total_changed += changed
            status = "MODIFIED" if changed > 0 else "clean"
            print(f"  {fname:50s} {w}x{h}  fringe={changed:5d}  [{status}]")
        except Exception as e:
            print(f"  ERROR {fname}: {e}")

    print(f"\nTotal fringe pixels removed: {total_changed}")
    print(f"Files processed: {len(results)}")
    modified = [r for r in results if r[3] > 0]
    print(f"Files modified: {len(modified)}")
    return results

if __name__ == "__main__":
    main()
