"""
sprite_prep.py — Gemini gorseli temizle + tum animasyon framelerini uret.

1) Arka plani kaldirir (rembg, GPU)
2) Gemini logosunu siler (kose tespiti + inpaint)
3) Tum animasyon turlerini uretir: idle, walk, attack, dash, death
4) Her animasyon icin sprite sheet kaydeder

Kullanim:
    python tools/sprite_prep.py ART/karakterler/warblade/warblade_gemini_base.png
    python tools/sprite_prep.py ART/karakterler/warblade/warblade_gemini_base.png --size 256
    python tools/sprite_prep.py ART/karakterler/warblade/warblade_gemini_base.png --size 64
"""

import sys
import math
import argparse
from pathlib import Path
from PIL import Image, ImageFilter, ImageDraw
import io


# --- Adim 1: Arka plan kaldir ---

def remove_background(img_path: Path) -> Image.Image:
    try:
        from rembg import remove
        print("  [rembg] Arka plan kaldiriliyor (GPU)...")
        with open(img_path, "rb") as f:
            data = f.read()
        result = remove(data)
        img = Image.open(io.BytesIO(result)).convert("RGBA")
        print(f"  OK  {img.width}x{img.height}, transparent")
        return img
    except ImportError:
        print("  UYARI: rembg yuklu degil, arka plan kaldirilmadi")
        return Image.open(img_path).convert("RGBA")


# --- Adim 2: Logo/watermark sil ---

def remove_watermark(img: Image.Image, logo_ratio: float = 0.08) -> Image.Image:
    """
    Gemini logosu genellikle alt-sag kosede kucuk bir alan kaplar.
    O bolgeyi transparente cevir + komsu piksellerle yumusak gecis yap.
    """
    print("  [logo] Watermark siliniyor...")
    w, h = img.size

    # Alt-sag kosedeki logo alanini bul (tipik: %8 yukseklik, %15 genislik)
    logo_h = int(h * logo_ratio)
    logo_w = int(w * 0.18)

    # Kopya uzerinde calis
    result = img.copy()
    pixels = result.load()

    # Logo bolgesini transparente cevir (gradyanlı gecis)
    for y in range(h - logo_h, h):
        for x in range(w - logo_w, w):
            dist_y = (y - (h - logo_h)) / logo_h   # 0.0 -> 1.0
            dist_x = (x - (w - logo_w)) / logo_w
            fade = min(dist_y, dist_x) ** 0.5
            r, g, b, a = pixels[x, y]
            pixels[x, y] = (r, g, b, int(a * (1 - fade * 0.9)))

    # Ust-sag kosede de kontrol (bazi versiyonlarda orada)
    logo_h2 = int(h * 0.06)
    logo_w2 = int(w * 0.15)
    for y in range(0, logo_h2):
        for x in range(w - logo_w2, w):
            dist_y = 1 - (y / logo_h2)
            dist_x = (x - (w - logo_w2)) / logo_w2
            fade = min(dist_y, dist_x) ** 0.5
            r, g, b, a = pixels[x, y]
            pixels[x, y] = (r, g, b, int(a * (1 - fade * 0.8)))

    print("  OK  Logo bolgeleri temizlendi")
    return result


# --- Adim 3: Hedef boyuta getir ---

def fit_to_canvas(img: Image.Image, size: int) -> Image.Image:
    """Gorseli kare canvas'a sidir, transparant kenar birak."""
    w, h = img.size
    scale = (size * 0.9) / max(w, h)
    nw = max(1, int(w * scale))
    nh = max(1, int(h * scale))
    resized = img.resize((nw, nh), Image.LANCZOS)
    canvas = Image.new("RGBA", (size, size), (0, 0, 0, 0))
    ox = (size - nw) // 2
    oy = (size - nh) // 2
    canvas.paste(resized, (ox, oy), resized)
    return canvas


# --- Adim 4: Animasyon frameleri ---

def make_idle(base: Image.Image, frames: int = 4) -> list[Image.Image]:
    """Nefes alma: hafif yukari-asagi + scale."""
    result = []
    for i in range(frames):
        t = i / frames
        wave = math.sin(t * 2 * math.pi)
        shift_y = int(wave * 2)
        breathe = 1.0 + wave * 0.015
        nh = max(1, int(base.height * breathe))
        scaled = base.resize((base.width, nh), Image.LANCZOS)
        frame = Image.new("RGBA", base.size, (0, 0, 0, 0))
        py = (base.height - nh) // 2 + shift_y
        frame.paste(scaled, (0, py), scaled)
        result.append(frame)
    return result


def make_walk(base: Image.Image, frames: int = 6) -> list[Image.Image]:
    """Yuruyus: yan sallanma + yukari-asagi ziplama + hafif lean."""
    result = []
    size = base.size[0]
    for i in range(frames):
        t = i / frames
        # Yan sallanma (sol-sag)
        sway_x = int(math.sin(t * 2 * math.pi) * 3)
        # Ziplama (her adimda 2x yukari-asagi)
        bob_y  = int(abs(math.sin(t * 4 * math.pi)) * 4) - 2
        # Hafif one-arkaya egim (rotation)
        lean   = math.sin(t * 2 * math.pi) * 2.5  # derece

        rotated = base.rotate(lean, resample=Image.BICUBIC, expand=False)
        frame   = Image.new("RGBA", base.size, (0, 0, 0, 0))
        frame.paste(rotated, (sway_x, bob_y), rotated)
        result.append(frame)
    return result


def make_attack(base: Image.Image, frames: int = 6) -> list[Image.Image]:
    """Saldiri: windup (geri cekil) -> salding (ileri) -> recovery (geri)."""
    result = []
    # frame dagilimi: 0-1 windup, 2 impact, 3-5 recovery
    phases = [
        (-3, -2,  1.00,  0.0),   # windup 1: geri-yukari
        (-5, -3,  1.00,  0.0),   # windup 2: daha geri
        ( 6,  4,  1.04, -5.0),   # impact: ileri + buyume + lean
        ( 4,  2,  1.02, -3.0),   # recovery 1
        ( 1,  0,  1.01, -1.0),   # recovery 2
        ( 0,  0,  1.00,  0.0),   # neutral
    ]
    for dx, dy, scale, lean in phases:
        w, h = base.size
        nw = int(w * scale)
        nh = int(h * scale)
        scaled = base.resize((nw, nh), Image.LANCZOS)
        rotated = scaled.rotate(lean, resample=Image.BICUBIC, expand=False)
        frame = Image.new("RGBA", base.size, (0, 0, 0, 0))
        ox = (w - rotated.width)  // 2 + dx
        oy = (h - rotated.height) // 2 + dy
        frame.paste(rotated, (ox, oy), rotated)
        result.append(frame)
    return result


def make_dash(base: Image.Image, frames: int = 4) -> list[Image.Image]:
    """Kacis: hizli ileri + motion blur + stretch."""
    result = []
    steps = [
        ( 0,  0, 1.00, 1.00, 0),     # baslangic
        ( 8, -2, 1.15, 0.85, 3),     # hizlanma: yatay stretch
        (14, -4, 1.25, 0.75, 5),     # zirve hiz: en fazla stretch
        ( 6, -1, 1.08, 0.92, 2),     # yavasla
    ]
    for dx, dy, sx, sy, blur_r in steps:
        w, h = base.size
        nw = max(1, int(w * sx))
        nh = max(1, int(h * sy))
        stretched = base.resize((nw, nh), Image.LANCZOS)
        if blur_r > 0:
            stretched = stretched.filter(ImageFilter.GaussianBlur(radius=blur_r))
        frame = Image.new("RGBA", base.size, (0, 0, 0, 0))
        ox = (w - nw) // 2 + dx
        oy = (h - nh) // 2 + dy
        frame.paste(stretched, (ox, oy), stretched)
        result.append(frame)
    return result


def make_death(base: Image.Image, frames: int = 8) -> list[Image.Image]:
    """Olum: one dusme + solma."""
    result = []
    for i in range(frames):
        t = i / (frames - 1)
        # Ilerleyici egim (one kapanma)
        angle = t * 85
        # Asagiya kayma
        drop_y = int(t * t * base.height * 0.4)
        # Saydamlik azalma (son 3 frame)
        alpha = 1.0 if t < 0.6 else max(0.0, 1.0 - (t - 0.6) / 0.4)

        rotated = base.rotate(-angle, resample=Image.BICUBIC, expand=False)

        # Alpha uygula
        r2, g2, b2, a2 = rotated.split()
        a2 = a2.point(lambda x: int(x * alpha))
        rotated = Image.merge("RGBA", (r2, g2, b2, a2))

        frame = Image.new("RGBA", base.size, (0, 0, 0, 0))
        frame.paste(rotated, (0, drop_y), rotated)
        result.append(frame)
    return result


# --- Sprite sheet olustur ---

def make_sheet(frames: list[Image.Image]) -> Image.Image:
    w = frames[0].width * len(frames)
    h = frames[0].height
    sheet = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    for i, f in enumerate(frames):
        sheet.paste(f, (i * frames[0].width, 0), f)
    return sheet


# --- Ana ---

ANIMS = {
    "idle":   (make_idle,   4),
    "walk":   (make_walk,   6),
    "attack": (make_attack, 6),
    "dash":   (make_dash,   4),
    "death":  (make_death,  8),
}


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("image", help="Kaynak gorsel yolu")
    parser.add_argument("--size", type=int, default=256,
                        help="Cikti boyutu px (default: 256 — oyunda 64'e scale edilir)")
    parser.add_argument("--anim", choices=list(ANIMS.keys()),
                        help="Sadece tek animasyon (default: hepsi)")
    args = parser.parse_args()

    src = Path(args.image)
    if not src.exists():
        print(f"HATA: {src} bulunamadi")
        sys.exit(1)

    out_dir = src.parent
    stem    = src.stem
    size    = args.size

    print(f"\n=== sprite_prep.py ===")
    print(f"Kaynak : {src}")
    print(f"Boyut  : {size}x{size}")
    print()

    # 1) Arka plan kaldir
    img = remove_background(src)

    # 2) Logo sil
    img = remove_watermark(img)

    # 3) Temiz gorseli kaydet
    clean_path = out_dir / f"{stem}_clean.png"
    img.save(clean_path)
    print(f"  Temiz gorsel: {clean_path.name}\n")

    # 4) Hedef boyuta getir
    base = fit_to_canvas(img, size)

    # 5) Animasyonlar
    targets = {args.anim: ANIMS[args.anim]} if args.anim else ANIMS
    for name, (fn, frame_count) in targets.items():
        print(f"  [{name}] {frame_count} frame uretiliyor...")
        frames = fn(base, frame_count)
        sheet  = make_sheet(frames)
        out    = out_dir / f"{stem}_S_{name}_sheet.png"
        sheet.save(out)
        # Ayrica tek tek de kaydet
        for j, f in enumerate(frames):
            f.save(out_dir / f"{stem}_S_{name}_{j+1}.png")
        print(f"  OK  {out.name}  ({sheet.width}x{size}, {frame_count} frame)")

    print(f"\nTAMAM. Tum dosyalar: {out_dir}")
    print("Aseprite'ta ac: File -> Import Sprite Sheet -> frame width = {size}")


if __name__ == "__main__":
    main()
