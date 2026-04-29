#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Build a stable pixel-art-ready south reference pack from existing lock images.

Outputs:
  TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/pixelart_ready/<class>/<class>_south_ready_128.png
  TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/pixelart_ready/_qc_sheet.png
"""

from __future__ import annotations

from collections import deque
from pathlib import Path

import numpy as np
from PIL import Image, ImageDraw

ROOT = Path("F:/Antigravity Projeler/2d roguelite/RIMA")
SRC_A = ROOT / "TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new"
SRC_B = ROOT / "TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new"
OUT = ROOT / "TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/pixelart_ready"

CLASSES = [
    "warblade",
    "brawler",
    "elementalist",
    "gunslinger",
    "hexer",
    "ranger",
    "ravager",
    "ronin",
    "shadowblade",
    "summoner",
]


def pick_source(cls: str) -> Path:
    candidates = [
        SRC_A / f"{cls}_south_lock1.png",
        SRC_A / f"{cls}_south.png",
        SRC_A / f"{cls}.png",
        SRC_A / f"{cls}_south_lock.png",
        SRC_B / f"{cls}_south_lock.png",
    ]
    for p in candidates:
        if p.exists():
            return p
    raise FileNotFoundError(f"missing source for {cls}")


def flood_remove_bg(im: Image.Image, tolerance: int = 26) -> Image.Image:
    data = np.array(im.convert("RGBA"), dtype=np.int32)
    h, w = data.shape[:2]
    corners = np.array(
        [data[0, 0, :3], data[0, w - 1, :3], data[h - 1, 0, :3], data[h - 1, w - 1, :3]],
        dtype=np.int32,
    )
    bg = np.median(corners, axis=0)
    vis = np.zeros((h, w), dtype=bool)
    q = deque([(0, 0), (0, w - 1), (h - 1, 0), (h - 1, w - 1)])
    while q:
        r, c = q.popleft()
        if vis[r, c]:
            continue
        vis[r, c] = True
        rgb = data[r, c, :3]
        if np.abs(rgb - bg).max() <= tolerance:
            data[r, c, 3] = 0
            if r > 0:
                q.append((r - 1, c))
            if r < h - 1:
                q.append((r + 1, c))
            if c > 0:
                q.append((r, c - 1))
            if c < w - 1:
                q.append((r, c + 1))
    return Image.fromarray(data.astype(np.uint8), "RGBA")


def to_128(im: Image.Image) -> Image.Image:
    if im.size != (128, 128):
        return im.resize((128, 128), Image.NEAREST)
    return im


def main() -> None:
    OUT.mkdir(parents=True, exist_ok=True)
    thumbs = []
    for cls in CLASSES:
        src = pick_source(cls)
        class_dir = OUT / cls
        class_dir.mkdir(parents=True, exist_ok=True)
        out_path = class_dir / f"{cls}_south_ready_128.png"

        with Image.open(src).convert("RGBA") as im:
            cleaned = flood_remove_bg(im, tolerance=26)
            final = to_128(cleaned)
            final.save(out_path)

        canvas = Image.new("RGBA", (180, 180), (18, 18, 18, 255))
        with Image.open(out_path).convert("RGBA") as sprite:
            up = sprite.resize((128, 128), Image.NEAREST)
            canvas.paste(up, (26, 16), up)
        d = ImageDraw.Draw(canvas)
        d.text((8, 150), cls, fill=(235, 235, 235, 255))
        d.text((8, 164), src.name, fill=(140, 140, 140, 255))
        thumbs.append(canvas)
        print(f"[ok] {cls}: {src.name} -> {out_path}")

    cols = 5
    rows = (len(thumbs) + cols - 1) // cols
    sheet = Image.new("RGBA", (cols * 180, rows * 180), (8, 8, 8, 255))
    for i, t in enumerate(thumbs):
        sheet.paste(t, ((i % cols) * 180, (i // cols) * 180), t)
    sheet_path = OUT / "_qc_sheet.png"
    sheet.save(sheet_path)
    print(f"[done] qc sheet -> {sheet_path}")


if __name__ == "__main__":
    main()
