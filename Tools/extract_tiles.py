"""
PixelLab GIF → individual tile PNGs
Usage:
    python extract_tiles.py <input.gif> [output_folder]

Output: tile_01.png, tile_02.png, ... in output_folder (default: same dir as GIF)
"""

import sys
from pathlib import Path
from PIL import Image


def extract(gif_path: str, out_dir: str | None = None):
    gif = Image.open(gif_path)
    stem = Path(gif_path).stem[:30]  # short name for output prefix
    out = Path(out_dir) if out_dir else Path(gif_path).parent / f"{stem}_tiles"
    out.mkdir(parents=True, exist_ok=True)

    n = gif.n_frames
    pad = len(str(n))

    for i in range(n):
        gif.seek(i)
        frame = gif.convert("RGBA")
        name = out / f"tile_{str(i + 1).zfill(pad)}.png"
        frame.save(name, "PNG")
        print(f"  {name.name}  {frame.size}")

    print(f"\n{n} tiles saved to: {out}")


if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python extract_tiles.py <input.gif> [output_folder]")
        sys.exit(1)
    extract(sys.argv[1], sys.argv[2] if len(sys.argv) > 2 else None)
