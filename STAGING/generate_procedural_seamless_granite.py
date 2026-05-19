from __future__ import annotations

import math
import random
from pathlib import Path

from PIL import Image, ImageDraw, ImageFilter


OUT_SOURCE = Path("STAGING/granite_seamless_procedural_source_256.png")


def wrap_distance(a: float, b: float, period: int) -> float:
    d = abs(a - b)
    return min(d, period - d)


def value_noise(size: int, cells: int, rng: random.Random) -> list[list[float]]:
    grid = [[rng.random() for _ in range(cells)] for _ in range(cells)]
    data: list[list[float]] = []
    cell_size = size / cells
    for y in range(size):
        row: list[float] = []
        gy = y / cell_size
        y0 = int(math.floor(gy)) % cells
        y1 = (y0 + 1) % cells
        fy = gy - math.floor(gy)
        sy = fy * fy * (3.0 - 2.0 * fy)
        for x in range(size):
            gx = x / cell_size
            x0 = int(math.floor(gx)) % cells
            x1 = (x0 + 1) % cells
            fx = gx - math.floor(gx)
            sx = fx * fx * (3.0 - 2.0 * fx)
            a = grid[y0][x0] * (1.0 - sx) + grid[y0][x1] * sx
            b = grid[y1][x0] * (1.0 - sx) + grid[y1][x1] * sx
            row.append(a * (1.0 - sy) + b * sy)
        data.append(row)
    return data


def make_tile() -> Image.Image:
    rng = random.Random(19052026)
    size = 32
    base = value_noise(size, 4, rng)
    fine = value_noise(size, 16, rng)
    img = Image.new("RGB", (size, size))
    px = img.load()

    for y in range(size):
        for x in range(size):
            v = 0.74 * base[y][x] + 0.26 * fine[y][x]
            grain = int((v - 0.5) * 34)
            r = max(46, min(82, 63 + grain))
            g = max(48, min(84, 66 + grain))
            b = max(54, min(96, 74 + grain + 3))
            px[x, y] = (r, g, b)

    img = img.filter(ImageFilter.GaussianBlur(radius=0.35))
    crack = Image.new("RGBA", (size, size), (0, 0, 0, 0))
    draw = ImageDraw.Draw(crack)
    lines = [
        [(4, 9), (9, 10), (13, 12)],
        [(18, 25), (22, 23), (27, 22), (30, 20)],
        [(1, 29), (5, 27), (8, 28)],
    ]
    for points in lines:
        for dx in (-size, 0, size):
            for dy in (-size, 0, size):
                shifted = [(x + dx, y + dy) for x, y in points]
                draw.line(shifted, fill=(28, 30, 35, 85), width=1)
                draw.line([(x, y + 1) for x, y in shifted], fill=(87, 90, 101, 24), width=1)

    img = Image.alpha_composite(img.convert("RGBA"), crack).convert("RGB")

    # Match opposite borders exactly for deterministic zero-seam tiling.
    px = img.load()
    for x in range(size):
        avg = tuple((px[x, 0][i] + px[x, size - 1][i]) // 2 for i in range(3))
        px[x, 0] = avg
        px[x, size - 1] = avg
    for y in range(size):
        avg = tuple((px[0, y][i] + px[size - 1, y][i]) // 2 for i in range(3))
        px[0, y] = avg
        px[size - 1, y] = avg

    return img


def main() -> None:
    tile = make_tile()
    source = Image.new("RGB", (256, 256))
    for y in range(0, 256, 32):
        for x in range(0, 256, 32):
            source.paste(tile, (x, y))
    OUT_SOURCE.parent.mkdir(parents=True, exist_ok=True)
    source.save(OUT_SOURCE)
    print(OUT_SOURCE)


if __name__ == "__main__":
    main()
