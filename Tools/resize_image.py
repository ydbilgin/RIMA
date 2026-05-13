"""
RIMA Image Resize Utility
=========================
Claude Code'un Read tool'u 2000x2000 px üstündeki görselleri reddediyor.
Bu script büyük görselleri RAM-only resize edip yan dosya olarak kaydeder.

Kullanım:
    python tools/resize_image.py <input> [output]
    python tools/resize_image.py "C:/path/to/img.png"
    python tools/resize_image.py img.png resized.png

Default max: 1800x1800 (2000 sınırı altında güvenli marj).
Output yoksa: <input>_resized.<ext>
Aspect ratio korunur, en uzun kenara göre scale.
"""

from __future__ import annotations
import sys
from pathlib import Path
from PIL import Image

MAX_DIM = 1800


def resize_image(src: Path, dst: Path | None = None, max_dim: int = MAX_DIM) -> Path:
    if not src.exists():
        raise FileNotFoundError(f"Source not found: {src}")

    img = Image.open(src)
    w, h = img.size

    if max(w, h) <= max_dim:
        print(f"[skip] {src.name} already {w}x{h} <= {max_dim}")
        return src

    scale = max_dim / max(w, h)
    new_size = (int(w * scale), int(h * scale))

    img_resized = img.resize(new_size, Image.LANCZOS)

    if dst is None:
        dst = src.with_name(f"{src.stem}_resized{src.suffix}")

    if img_resized.mode in ("RGBA", "LA") and dst.suffix.lower() in (".jpg", ".jpeg"):
        img_resized = img_resized.convert("RGB")

    img_resized.save(dst, quality=92 if dst.suffix.lower() in (".jpg", ".jpeg") else None)
    print(f"[ok] {src.name} {w}x{h} -> {dst.name} {new_size[0]}x{new_size[1]}")
    return dst


def main():
    if len(sys.argv) < 2:
        print(__doc__)
        sys.exit(1)

    src = Path(sys.argv[1])
    dst = Path(sys.argv[2]) if len(sys.argv) > 2 else None
    out = resize_image(src, dst)
    print(str(out))


if __name__ == "__main__":
    main()
