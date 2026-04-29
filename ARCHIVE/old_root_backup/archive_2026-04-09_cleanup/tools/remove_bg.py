"""
remove_bg.py — Gorsellerden arka plani kaldirir (local, GPU destekli).
rembg kullanir (U2Net modeli). Internet gerektirmez.

Kurulum (bir kez):
    pip install rembg[gpu] onnxruntime-gpu

Kullanim:
    python tools/remove_bg.py ART/karakterler/warblade/warblade_gemini_base.png
    python tools/remove_bg.py ART/karakterler/warblade/   (klasordeki hepsi)
    python tools/remove_bg.py ART/                        (tum ART, recursive)

Cikti:
    Ayni klasore _nobg.png olarak kaydeder. Orjinal dosyaya dokunmaz.
"""

import sys
from pathlib import Path


def remove_bg(path: Path) -> Path:
    from rembg import remove
    from PIL import Image
    import io

    out_path = path.parent / (path.stem + "_nobg.png")

    with open(path, "rb") as f:
        data = f.read()

    result = remove(data)
    img = Image.open(io.BytesIO(result)).convert("RGBA")
    img.save(out_path)
    print(f"  OK  {path.name} -> {out_path.name}  ({img.width}x{img.height})")
    return out_path


def main():
    if len(sys.argv) < 2:
        print("Kullanim: python tools/remove_bg.py <dosya_veya_klasor>")
        sys.exit(1)

    try:
        from rembg import remove
    except ImportError:
        print("rembg yuklu degil. Kur:")
        print("  pip install rembg[gpu] onnxruntime-gpu")
        sys.exit(1)

    target = Path(sys.argv[1])
    extensions = {".png", ".jpg", ".jpeg", ".webp"}

    if target.is_file():
        paths = [target]
    elif target.is_dir():
        paths = [p for p in target.rglob("*")
                 if p.suffix.lower() in extensions
                 and "_nobg" not in p.stem]
    else:
        print(f"HATA: Bulunamadi: {target}")
        sys.exit(1)

    if not paths:
        print("Islenecek gorsel bulunamadi.")
        sys.exit(0)

    print(f"{len(paths)} gorsel isleniyor...\n")
    for p in sorted(paths):
        try:
            remove_bg(p)
        except Exception as e:
            print(f"  HATA  {p.name}: {e}")

    print("\nBitti.")


if __name__ == "__main__":
    main()
