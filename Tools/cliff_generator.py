#!/usr/bin/env python3
"""
RIMA Cliff Geometric Base Generator (REVIZE 2026-05-26)
Tile-cell-aligned: top edge = 1 floor cell width (64 px).
Output: 64x96 RGBA dimetric-ish cliff mock for PixelLab S-XL New init image.

Usage:
    python tools/cliff_generator.py
    python tools/cliff_generator.py --count 20
    python tools/cliff_generator.py --seed 42
"""
import argparse
import os
import random
from PIL import Image, ImageDraw

OUTPUT_DIR = "STAGING/cliff_bases"
WIDTH, HEIGHT = 64, 96
TOP_EDGE_Y = 4
TOP_EDGE_VARIATION = 2
TOP_DECK_BOTTOM = 16     # face starts here
CONTROL_POINTS = 9       # every 8 px across 64 width

# Palette (single-tone grey family for AI to texture later)
COLOR_TOP_DECK = (110, 110, 110, 255)
COLOR_FACE = (70, 70, 70, 255)
COLOR_OUTLINE = (40, 40, 40, 255)
COLOR_SIDE_SHADOW = (50, 50, 50, 255)
COLOR_CRACK = (30, 30, 30, 200)


def generate_cliff(seed: int, output_path: str):
    rng = random.Random(seed)
    img = Image.new('RGBA', (WIDTH, HEIGHT), (0, 0, 0, 0))
    d = ImageDraw.Draw(img)

    # Jagged top edge points
    top_points = []
    for i in range(CONTROL_POINTS):
        x = int(i * (WIDTH - 1) / (CONTROL_POINTS - 1))
        y_offset = rng.randint(-TOP_EDGE_VARIATION, TOP_EDGE_VARIATION)
        top_points.append((x, TOP_EDGE_Y + y_offset))

    # Main polygon (jagged top + bottom rectangle)
    main_polygon = top_points + [(WIDTH - 1, HEIGHT - 1), (0, HEIGHT - 1)]
    d.polygon(main_polygon, fill=COLOR_FACE, outline=COLOR_OUTLINE)

    # Top deck overlay (lighter color from top edge to TOP_DECK_BOTTOM)
    top_deck_polygon = top_points + [(WIDTH - 1, TOP_DECK_BOTTOM), (0, TOP_DECK_BOTTOM)]
    d.polygon(top_deck_polygon, fill=COLOR_TOP_DECK)

    # Right side shadow strip (2 px wide, face only)
    for y in range(TOP_DECK_BOTTOM, HEIGHT):
        d.line([(WIDTH - 2, y), (WIDTH - 1, y)], fill=COLOR_SIDE_SHADOW, width=1)

    # Subtle vertical cracks (1-2 per cliff, face area only)
    crack_count = rng.randint(1, 2)
    for _ in range(crack_count):
        crack_x = rng.randint(8, WIDTH - 10)
        crack_y_start = TOP_DECK_BOTTOM + rng.randint(2, 6)
        crack_y_end = HEIGHT - rng.randint(5, 15)
        # 2-segment wave
        mid_y = (crack_y_start + crack_y_end) // 2
        mid_x_offset = rng.randint(-1, 1)
        d.line([(crack_x, crack_y_start), (crack_x + mid_x_offset, mid_y)], fill=COLOR_CRACK, width=1)
        d.line([(crack_x + mid_x_offset, mid_y), (crack_x, crack_y_end)], fill=COLOR_CRACK, width=1)

    img.save(output_path)
    return output_path


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--count', type=int, default=10)
    parser.add_argument('--seed', type=int, default=None)
    parser.add_argument('--out', type=str, default=OUTPUT_DIR)
    args = parser.parse_args()

    os.makedirs(args.out, exist_ok=True)

    if args.seed is not None:
        path = os.path.join(args.out, f'cliff_v{args.seed:02d}.png')
        generate_cliff(args.seed, path)
        print(f'Generated: {path}')
    else:
        for i in range(1, args.count + 1):
            path = os.path.join(args.out, f'cliff_v{i:02d}.png')
            generate_cliff(i, path)
            print(f'Generated: {path}')

    print(f'\nDone -- {args.count if args.seed is None else 1} cliff base(s) at {args.out}')


if __name__ == '__main__':
    main()
