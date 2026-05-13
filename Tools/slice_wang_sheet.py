"""
Wang Tileset Sheet Slicer
==========================
PixelLab Wang sheet (128x128, 4x4 grid) → 16 ayrı 32x32 tile.
Style reference için kullanışlı subset seçer + kaydeder.

Kullanım:
    python tools/slice_wang_sheet.py <input_sheet.png> [output_dir]

Default output: <input>_tiles/ klasörü
Üretir:
    tile_00.png .. tile_15.png  (Wang 4x4 grid'de soldan sağa, üstten alta)
    style_refs/ klasörü (4 önerilen style reference tile, palette anchor için)

Wang index sırası (standard):
    00 01 02 03
    04 05 06 07
    08 09 10 11
    12 13 14 15

Style ref önerisi:
    - tile_00 = pure lower terrain (sol üst köşe, en saf zemin)
    - tile_15 = pure upper terrain (sağ alt köşe, en saf duvar)
    - tile_05 = mixed corner (transition)
    - tile_10 = mixed corner (transition)
"""

from __future__ import annotations
import sys
from pathlib import Path
from PIL import Image

TILE_SIZE = 32
GRID = 4  # 4x4

# Wang standard'da en bilgi yoğun 4 tile (palette anchor için ideal)
STYLE_REF_INDICES = [0, 5, 10, 15]


def slice_sheet(sheet_path: Path, out_dir: Path | None = None) -> Path:
    if not sheet_path.exists():
        raise FileNotFoundError(f"Sheet not found: {sheet_path}")

    img = Image.open(sheet_path)
    w, h = img.size
    expected = TILE_SIZE * GRID

    if w != expected or h != expected:
        print(f"[warn] sheet {w}x{h}, expected {expected}x{expected} — slicing anyway")

    if out_dir is None:
        out_dir = sheet_path.parent / f"{sheet_path.stem}_tiles"

    out_dir.mkdir(parents=True, exist_ok=True)
    style_dir = out_dir / "style_refs"
    style_dir.mkdir(exist_ok=True)

    tiles_saved = []
    for row in range(GRID):
        for col in range(GRID):
            idx = row * GRID + col
            box = (col * TILE_SIZE, row * TILE_SIZE,
                   (col + 1) * TILE_SIZE, (row + 1) * TILE_SIZE)
            tile = img.crop(box)
            out_path = out_dir / f"tile_{idx:02d}.png"
            tile.save(out_path)
            tiles_saved.append(out_path)
            if idx in STYLE_REF_INDICES:
                tile.save(style_dir / f"style_ref_{idx:02d}.png")

    print(f"[ok] {sheet_path.name} -> {out_dir}")
    print(f"     {len(tiles_saved)} tiles sliced")
    print(f"     style_refs/: {len(STYLE_REF_INDICES)} tile (Style Reference için)")
    return out_dir


def main():
    if len(sys.argv) < 2:
        print(__doc__)
        sys.exit(1)

    src = Path(sys.argv[1])
    dst = Path(sys.argv[2]) if len(sys.argv) > 2 else None
    out = slice_sheet(src, dst)
    print(str(out))


if __name__ == "__main__":
    main()
