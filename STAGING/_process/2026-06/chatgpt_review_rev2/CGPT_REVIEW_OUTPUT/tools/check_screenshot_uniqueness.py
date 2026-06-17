#!/usr/bin/env python3
"""Check screenshot dimensions, SHA uniqueness, and simple perceptual near-duplicates."""
from pathlib import Path
from PIL import Image
import hashlib, csv, itertools, argparse
import numpy as np

def dhash(path: Path, hash_size: int = 16):
    img = Image.open(path).convert("L").resize((hash_size + 1, hash_size), Image.Resampling.LANCZOS)
    arr = np.asarray(img)
    return (arr[:, 1:] > arr[:, :-1]).flatten()

def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("folder", type=Path)
    ap.add_argument("--threshold", type=int, default=5)
    args = ap.parse_args()

    files = sorted(args.folder.glob("*.png"))
    rows = []
    hashes = {}
    for f in files:
        with Image.open(f) as im:
            w, h = im.size
        sha = hashlib.sha256(f.read_bytes()).hexdigest()
        rows.append((f.name, w, h, sha))
        hashes[f.name] = dhash(f)

    print(f"PNG count: {len(files)}")
    print(f"Exact SHA duplicate groups: {len(rows) - len({r[3] for r in rows})}")

    print(f"\nPerceptual pairs with dHash distance <= {args.threshold}:")
    for a, b in itertools.combinations(files, 2):
        d = int(np.count_nonzero(hashes[a.name] != hashes[b.name]))
        if d <= args.threshold:
            print(f"{d:3d}  {a.name}  <->  {b.name}")

if __name__ == "__main__":
    main()
