from __future__ import annotations

import argparse
from pathlib import Path

from PIL import Image, ImageChops, ImageStat


def edge_score(img: Image.Image) -> dict[str, float]:
    rgb = img.convert("RGB")
    w, h = rgb.size
    top = rgb.crop((0, 0, w, 1))
    bottom = rgb.crop((0, h - 1, w, h))
    left = rgb.crop((0, 0, 1, h))
    right = rgb.crop((w - 1, 0, w, h))
    tb = ImageChops.difference(top, bottom)
    lr = ImageChops.difference(left, right)
    tb_stat = ImageStat.Stat(tb)
    lr_stat = ImageStat.Stat(lr)
    return {
        "top_bottom_mean": sum(tb_stat.mean) / 3.0,
        "left_right_mean": sum(lr_stat.mean) / 3.0,
        "top_bottom_max": max(tb.getextrema()[i][1] for i in range(3)),
        "left_right_max": max(lr.getextrema()[i][1] for i in range(3)),
    }


def extract(src: Path, dst: Path, size: int) -> dict[str, float]:
    img = Image.open(src).convert("RGBA")
    w, h = img.size
    if w < size or h < size:
        raise ValueError(f"source too small: {w}x{h}, requested {size}x{size}")

    if w == 256 and h == 256 and size == 32:
        x0 = 96
        y0 = 96
    else:
        x0 = (w - size) // 2
        y0 = (h - size) // 2
    tile = img.crop((x0, y0, x0 + size, y0 + size))
    dst.parent.mkdir(parents=True, exist_ok=True)
    tile.save(dst)
    return edge_score(tile)


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("--src", required=True)
    parser.add_argument("--dst", required=True)
    parser.add_argument("--size", type=int, default=32)
    args = parser.parse_args()

    scores = extract(Path(args.src), Path(args.dst), args.size)
    for key, value in scores.items():
        print(f"{key}: {value:.2f}")


if __name__ == "__main__":
    main()
