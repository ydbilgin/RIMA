"""
remove_bg.py — PixelLab sprite background remover
Usage:
  python remove_bg.py <file.png>              # tek dosya
  python remove_bg.py <klasor/>               # klasördeki tüm PNG
  python remove_bg.py <klasor/> --tolerance 30  # tolerans ayarla (default 25)
  python remove_bg.py <klasor/> --dry-run     # sadece önizle, kaydetme

Gereksinim: pip install pillow numpy
"""

import sys
import argparse
from pathlib import Path
from collections import deque

import numpy as np
from PIL import Image


def remove_bg(path: Path, tolerance: int = 25) -> tuple[int, int]:
    """
    4 köşeden BFS flood fill ile arka planı şeffaf yapar.
    Returns: (silinen piksel sayısı, toplam piksel)
    """
    img = Image.open(path).convert("RGBA")
    data = np.array(img, dtype=np.int32)
    h, w = data.shape[:2]

    # Köşe renklerinin medyanı = arka plan rengi (tek köşe yanlış olabilir)
    corners = [data[0, 0, :3], data[0, w-1, :3], data[h-1, 0, :3], data[h-1, w-1, :3]]
    bg_color = np.median(corners, axis=0)

    visited = np.zeros((h, w), dtype=bool)
    queue = deque([(0, 0), (0, w-1), (h-1, 0), (h-1, w-1)])
    removed = 0

    while queue:
        r, c = queue.popleft()
        if visited[r, c]:
            continue
        visited[r, c] = True

        pixel_rgb = data[r, c, :3]
        if np.abs(pixel_rgb - bg_color).max() <= tolerance:
            data[r, c, 3] = 0
            removed += 1
            for dr, dc in ((-1, 0), (1, 0), (0, -1), (0, 1)):
                nr, nc = r + dr, c + dc
                if 0 <= nr < h and 0 <= nc < w and not visited[nr, nc]:
                    queue.append((nr, nc))

    result = Image.fromarray(data.astype(np.uint8), "RGBA")
    result.save(path)
    return removed, h * w


def process(target: Path, tolerance: int, dry_run: bool) -> None:
    if target.is_file():
        files = [target]
    elif target.is_dir():
        files = sorted(target.glob("*.png"))
    else:
        print(f"HATA: {target} bulunamadı")
        sys.exit(1)

    if not files:
        print("PNG dosyası bulunamadı.")
        return

    for f in files:
        if dry_run:
            print(f"[DRY] {f.name}")
            continue
        removed, total = remove_bg(f, tolerance)
        pct = removed / total * 100
        print(f"OK  {f.name}  ({removed} px silindi, %{pct:.1f})")

    if not dry_run:
        print(f"\n{len(files)} dosya işlendi.")


def main():
    parser = argparse.ArgumentParser(description="PixelLab sprite BG remover")
    parser.add_argument("target", help="PNG dosyası veya klasör yolu")
    parser.add_argument("--tolerance", type=int, default=25,
                        help="Renk toleransı 0-255 (default 25)")
    parser.add_argument("--dry-run", action="store_true",
                        help="Kaydetmeden listele")
    args = parser.parse_args()

    process(Path(args.target), args.tolerance, args.dry_run)


if __name__ == "__main__":
    main()
