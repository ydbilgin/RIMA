"""Generate bell-shape cliff silhouette for PixelLab init image.

Output: STAGING/bell_silhouette_64x192.png
Shape: Top 48px (Y=0-16), flares to 60px (Y=150), pinches to 56px (Y=192).
"""

from PIL import Image
import random
import os

W, H = 64, 192
SEED = 42
random.seed(SEED)


def width_at_y(y: int) -> int:
    """Bell-shape silhouette spec (user S109 lock):
    Y=0-12:    46px (narrow neck)
    Y=12-145:  46 -> 62px linear flare
    Y=145-180: 62px peak hold
    Y=180-192: 62 -> 55px pinch
    Y=192:     55px flat base edge"""
    if y <= 12:
        return 46
    elif y <= 145:
        t = (y - 12) / (145 - 12)
        return int(46 + (62 - 46) * t)
    elif y <= 180:
        return 62
    else:
        t = (y - 180) / (192 - 180)
        return int(62 - (62 - 55) * t)


def main() -> None:
    """Pure black solid silhouette for shape lock. No gradient, no edge noise,
    no texture. PixelLab will add texture/color via prompt at AI Freedom 0.25-0.35."""
    img = Image.new('RGBA', (W, H), (0, 0, 0, 0))
    pixels = img.load()

    for y in range(H):
        w = width_at_y(y)
        x_start = (W - w) // 2
        x_end = x_start + w
        for x in range(x_start, x_end):
            pixels[x, y] = (0, 0, 0, 255)  # pure black solid, fully opaque

    out_path = os.path.join(os.path.dirname(__file__), 'bell_silhouette_64x192.png')
    img.save(out_path)
    print(f'Saved: {out_path}')
    print(f'Size: {W}x{H}')
    print('Width profile (user spec lock):')
    for y in (0, 12, 48, 96, 145, 180, 191):
        print(f'  Y={y:3d}: width={width_at_y(y)}px')


if __name__ == '__main__':
    main()
