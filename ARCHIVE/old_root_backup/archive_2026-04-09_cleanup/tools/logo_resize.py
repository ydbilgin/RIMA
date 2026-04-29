"""
RIMA Logo Resize Aracı
======================
Gemini'den gelen yüksek çözünürlüklü logo görselini
320x80, 160x40, 64x64 boyutlarına dönüştürür.

Kullanım:
    python logo_resize.py <kaynak_dosya.png>

Örnek:
    python logo_resize.py "ART/logo/rima_logo_kaynak.png"

Çıktılar:
    ART/logo/rima_logo_320x80.png   ← ana logo
    ART/logo/rima_logo_160x40.png   ← kompakt
    ART/logo/rima_icon_64x64.png    ← kare ikon
"""

import sys
import os
from pathlib import Path

try:
    from PIL import Image
except ImportError:
    print("Pillow yüklü değil. Yüklemek için:")
    print("  pip install Pillow")
    sys.exit(1)


def fit_and_crop(img: Image.Image, target_w: int, target_h: int) -> Image.Image:
    """
    Görseli hedef boyuta sığdırır:
    - En-boy oranını koruyarak ölçekler (en az boyut hedefi karşılasın)
    - Sonra tam ortadan kırpar
    """
    src_w, src_h = img.size
    scale = max(target_w / src_w, target_h / src_h)
    new_w = int(src_w * scale)
    new_h = int(src_h * scale)
    img = img.resize((new_w, new_h), Image.LANCZOS)

    left = (new_w - target_w) // 2
    top  = (new_h - target_h) // 2
    return img.crop((left, top, left + target_w, top + target_h))


def fit_inside(img: Image.Image, target_w: int, target_h: int,
               bg_color=(0, 0, 0, 0)) -> Image.Image:
    """
    Görseli boyutlara sığdırır, kırpmaz — boşlukları şeffaf veya bg_color ile doldurur.
    Logo gibi şeylerde bozulma olmadan sığdırmak için kullan.
    """
    src_w, src_h = img.size
    scale = min(target_w / src_w, target_h / src_h)
    new_w = int(src_w * scale)
    new_h = int(src_h * scale)
    img = img.resize((new_w, new_h), Image.LANCZOS)

    canvas = Image.new("RGBA", (target_w, target_h), bg_color)
    offset_x = (target_w - new_w) // 2
    offset_y = (target_h - new_h) // 2
    canvas.paste(img, (offset_x, offset_y), mask=img if img.mode == "RGBA" else None)
    return canvas


def main():
    if len(sys.argv) < 2:
        print("Kullanım: python logo_resize.py <kaynak.png>")
        print("Örnek:    python logo_resize.py ART/logo/rima_logo_kaynak.png")
        sys.exit(1)

    src_path = Path(sys.argv[1])
    if not src_path.exists():
        print(f"Hata: Dosya bulunamadı: {src_path}")
        sys.exit(1)

    out_dir = src_path.parent
    img = Image.open(src_path).convert("RGBA")
    print(f"Kaynak: {src_path.name}  ({img.size[0]}x{img.size[1]})")

    # ── 320x80 — ana logo ────────────────────────────────────────────────
    logo_main = fit_inside(img, 320, 80)
    out_main = out_dir / "rima_logo_320x80.png"
    logo_main.save(out_main)
    print(f"  ✓ {out_main.name}  (320x80)")

    # ── 160x40 — kompakt ─────────────────────────────────────────────────
    logo_compact = fit_inside(img, 160, 40)
    out_compact = out_dir / "rima_logo_160x40.png"
    logo_compact.save(out_compact)
    print(f"  ✓ {out_compact.name}  (160x40)")

    # ── 64x64 — kare ikon (ortadan kare kırp) ────────────────────────────
    short = min(img.size)
    cx, cy = img.size[0] // 2, img.size[1] // 2
    half = short // 2
    square = img.crop((cx - half, cy - half, cx + half, cy + half))
    icon = square.resize((64, 64), Image.LANCZOS)
    out_icon = out_dir / "rima_icon_64x64.png"
    icon.save(out_icon)
    print(f"  ✓ {out_icon.name}  (64x64)")

    print("\nTamamlandı! Dosyalar:", out_dir)


if __name__ == "__main__":
    main()
