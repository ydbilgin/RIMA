"""
anim_demo.py — Gemini BASE görselinden demo idle animasyon üretir.
ComfyUI gerekmez. Sadece PIL.

Kullanim:
    python tools/anim_demo.py ART/karakterler/warblade/warblade_gemini_base.png

Cikti:
    ART/karakterler/warblade/demo_idle_sheet.png  (4 frame yan yana)
    ART/karakterler/warblade/demo_idle_*.png      (tek tek frameler)

Bu bir DEMO. Gerçek animasyon için PixelLab Animate veya MimicMotion kullan.
"""

import sys
from pathlib import Path
from PIL import Image, ImageFilter, ImageEnhance
import math

TARGET_SIZE = 64   # piksel, 32 veya 64 yap
FRAME_COUNT = 4    # idle animasyon frame sayisi


def make_idle_frames(img: Image.Image, count: int) -> list[Image.Image]:
    """
    Basit idle animasyon: hafif yukarı-aşağı sallanma + nefes (scale).
    Gerçek AI animasyon değil — konsepti göstermek için.
    """
    frames = []
    for i in range(count):
        t = i / count  # 0.0 → 1.0
        wave = math.sin(t * 2 * math.pi)  # -1 → +1 sinüs dalgası

        # Hafif yukarı-aşağı kayma (1-2 piksel)
        shift_y = int(wave * 1.5)

        # Hafif nefes: yüksekliği %2-3 oranında değiştir
        breathe = 1.0 + wave * 0.015
        new_h = max(1, int(img.height * breathe))
        scaled = img.resize((img.width, new_h), Image.LANCZOS)

        # Orijinal boyuta geri al, ortaya yapıştır
        frame = Image.new("RGBA", img.size, (0, 0, 0, 0))
        paste_y = (img.height - scaled.height) // 2 + shift_y
        frame.paste(scaled, (0, paste_y), scaled if scaled.mode == "RGBA" else None)
        frames.append(frame)

    return frames


def pixelate(img: Image.Image, target: int) -> Image.Image:
    """Görüntüyü target×target piksel sanat boyutuna indir."""
    w, h = img.size
    scale = target / max(w, h)
    new_w = max(1, int(w * scale))
    new_h = max(1, int(h * scale))
    # Küçült (NEAREST = piksel sanat görünümü)
    small = img.resize((new_w, new_h), Image.LANCZOS)
    # Kare canvas'a ortala
    canvas = Image.new("RGBA", (target, target), (0, 0, 0, 0))
    ox = (target - new_w) // 2
    oy = (target - new_h) // 2
    canvas.paste(small, (ox, oy), small if small.mode == "RGBA" else None)
    return canvas


def make_sprite_sheet(frames: list[Image.Image]) -> Image.Image:
    """Frame'leri yatay olarak birleştir."""
    w = frames[0].width * len(frames)
    h = frames[0].height
    sheet = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    for i, f in enumerate(frames):
        sheet.paste(f, (i * frames[0].width, 0), f)
    return sheet


def main():
    if len(sys.argv) < 2:
        print("Kullanim: python tools/anim_demo.py <gorsel_yolu>")
        sys.exit(1)

    src = Path(sys.argv[1])
    if not src.exists():
        print(f"HATA: Dosya bulunamadi: {src}")
        sys.exit(1)

    out_dir = src.parent
    name = src.stem

    print(f"Kaynak: {src} ({TARGET_SIZE}x{TARGET_SIZE} hedef)")
    img = Image.open(src).convert("RGBA")

    # 1) Pixelate
    print("Pikselestiriliyor...")
    pixel_img = pixelate(img, TARGET_SIZE)

    # 2) Idle frame'leri üret
    print(f"{FRAME_COUNT} idle frame uretiliyor...")
    frames = make_idle_frames(pixel_img, FRAME_COUNT)

    # 3) Tek tek kaydet
    for i, f in enumerate(frames):
        p = out_dir / f"{name}_idle_{i+1}.png"
        f.save(p)
        print(f"  Kaydedildi: {p.name}")

    # 4) Sprite sheet
    sheet = make_sprite_sheet(frames)
    sheet_path = out_dir / f"{name}_idle_sheet.png"
    sheet.save(sheet_path)
    print(f"\nSprite sheet: {sheet_path}")
    print(f"Boyut: {sheet.width}x{sheet.height} ({FRAME_COUNT} frame x {TARGET_SIZE}px)")
    print("\nBitti. Gercek animasyon icin PixelLab Animate veya MimicMotion kullan.")


if __name__ == "__main__":
    main()
