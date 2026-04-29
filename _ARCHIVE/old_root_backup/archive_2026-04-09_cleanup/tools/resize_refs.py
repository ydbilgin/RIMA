"""
resize_refs.py — Klasördeki resimleri 200x200 altına indirir (PixelLab S-M uyumlu)
Kullanım:
    python tools/resize_refs.py ART/dusmanlar/grunt_shard
    python tools/resize_refs.py ART/karakterler/warblade
    python tools/resize_refs.py ART  (tüm ART klasörü, recursive)
"""

import sys
import os
from pathlib import Path
from PIL import Image

MAX_SIZE = 180  # 200'den biraz küçük bırak, güvenli margin

def resize_image(path: Path):
    img = Image.open(path)
    w, h = img.size

    if w <= MAX_SIZE and h <= MAX_SIZE:
        print(f"  ATLA  {path.name} ({w}x{h}) — zaten küçük")
        return

    scale = MAX_SIZE / max(w, h)
    new_w = max(1, int(w * scale))
    new_h = max(1, int(h * scale))

    resized = img.resize((new_w, new_h), Image.LANCZOS)
    resized.save(path)
    print(f"  OK    {path.name}  {w}x{h} -> {new_w}x{new_h}")

def main():
    if len(sys.argv) < 2:
        print("Kullanim: python tools/resize_refs.py <klasor_yolu>")
        sys.exit(1)

    folder = Path(sys.argv[1])
    if not folder.exists():
        print(f"HATA: Klasor bulunamadi: {folder}")
        sys.exit(1)

    extensions = {".png", ".jpg", ".jpeg", ".webp"}
    images = [p for p in folder.rglob("*") if p.suffix.lower() in extensions]

    if not images:
        print(f"Klasorde resim bulunamadi: {folder}")
        sys.exit(0)

    print(f"{len(images)} resim bulundu -> max {MAX_SIZE}x{MAX_SIZE} yapiliyor...\n")
    for img_path in sorted(images):
        try:
            resize_image(img_path)
        except Exception as e:
            print(f"  HATA  {img_path.name}: {e}")

    print("\nBitti.")

if __name__ == "__main__":
    main()
