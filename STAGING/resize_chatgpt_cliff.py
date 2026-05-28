"""Resize ChatGPT cliff image to 64x192 for PixelLab init image.

Source: C:/Users/ydbil/Downloads/ChatGPT Image 26 May 2026 16_17_50.png
Output: STAGING/chatgpt_cliff_init_64x192.png

Strategy: Nearest neighbor downscale (pixel art preservation), fit to 64x192
with transparent background and centered placement.
"""

from PIL import Image
import os

SRC = r'C:\Users\ydbil\Downloads\ChatGPT Image 26 May 2026 16_17_50.png'
OUT = os.path.join(os.path.dirname(__file__), 'chatgpt_cliff_init_64x192.png')
W, H = 64, 192


def main() -> None:
    src = Image.open(SRC)
    print(f'Source size: {src.size}  (aspect {src.size[0]/src.size[1]:.2f})')
    print(f'Source mode: {src.mode}')

    # Convert to RGBA if not already
    if src.mode != 'RGBA':
        src = src.convert('RGBA')

    # Auto-detect content bbox using alpha or non-white pixels
    if src.mode == 'RGBA':
        alpha = src.split()[-1]
        bbox = alpha.getbbox()
    else:
        bbox = src.getbbox()

    if bbox:
        cropped = src.crop(bbox)
        print(f'Cropped to content: {cropped.size}')
    else:
        cropped = src

    # Fit to 64x192 maintaining aspect ratio
    src_w, src_h = cropped.size
    scale = min(W / src_w, H / src_h)
    new_w = max(1, int(src_w * scale))
    new_h = max(1, int(src_h * scale))
    resized = cropped.resize((new_w, new_h), Image.NEAREST)
    print(f'Resized: {resized.size}  (scale {scale:.3f})')

    # Center on 64x192 transparent canvas
    canvas = Image.new('RGBA', (W, H), (0, 0, 0, 0))
    x = (W - new_w) // 2
    y = (H - new_h) // 2
    canvas.paste(resized, (x, y), resized)
    canvas.save(OUT)
    print(f'Saved: {OUT}')


if __name__ == '__main__':
    main()
