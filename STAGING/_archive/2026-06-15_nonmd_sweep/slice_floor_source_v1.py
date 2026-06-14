"""Slice Codex floor source into 64/128/256 macro patch candidates.

Pipeline (ChatGPT FINAL §5):
  1. Crop 64/128/256 candidates from interior
  2. LANCZOS downsample
  3. RIMA palette quantize (adaptive 32 colors)
  4. Irregular alpha mask (soft radial + noise)
  5. Output inspection grid + individual chunks
"""
from pathlib import Path
from PIL import Image, ImageDraw, ImageFilter, ImageChops
import random
import math

random.seed(17)

SRC = Path(r"F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/Phase1A_L2b_Source/codex_floor_source_v1.png")
OUT_DIR = Path(r"F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/Phase1A_L2b_Source/chunks")
OUT_DIR.mkdir(exist_ok=True, parents=True)

source = Image.open(SRC).convert("RGBA")
W, H = source.size
print(f"Source: {W}x{H}")

# RIMA palette: 16-color slate-amber clamp
def quantize_rima(img):
    rgb = img.convert("RGB")
    quantized = rgb.quantize(colors=24, method=Image.MEDIANCUT, dither=Image.FLOYDSTEINBERG)
    return quantized.convert("RGBA")

# Irregular alpha mask: soft radial with perturbation
def make_alpha_mask(size, seed):
    rng = random.Random(seed)
    mask = Image.new("L", (size, size), 0)
    draw = ImageDraw.Draw(mask)
    cx, cy = size // 2, size // 2
    base_r = size * 0.42
    for i in range(36):
        angle = (i / 36) * 2 * math.pi
        wobble = rng.uniform(0.82, 1.05)
        rx = base_r * wobble
        ry = base_r * rng.uniform(0.82, 1.05)
        x = cx + math.cos(angle) * rx
        y = cy + math.sin(angle) * ry
        draw.ellipse([cx - rx, cy - ry, cx + rx, cy + ry], fill=200)
    mask = mask.filter(ImageFilter.GaussianBlur(radius=size * 0.10))
    return mask

def extract_chunks(chunk_size, count, margin=None):
    if margin is None:
        margin = chunk_size // 2
    candidates = []
    attempts = 0
    while len(candidates) < count and attempts < count * 10:
        x = random.randint(margin, W - chunk_size - margin)
        y = random.randint(margin, H - chunk_size - margin)
        attempts += 1
        chunk = source.crop((x, y, x + chunk_size, y + chunk_size))
        target_native = min(chunk_size, 128)
        if chunk_size > target_native:
            chunk = chunk.resize((target_native, target_native), Image.LANCZOS)
        quantized = quantize_rima(chunk)
        mask = make_alpha_mask(quantized.width, seed=len(candidates))
        r, g, b, _ = quantized.split()
        masked = Image.merge("RGBA", (r, g, b, mask))
        candidates.append((chunk_size, x, y, masked))
    return candidates

print("Extracting chunks...")
c64 = extract_chunks(64, 20)
c128 = extract_chunks(128, 16)
c256 = extract_chunks(256, 8)
print(f"  64px: {len(c64)} | 128px: {len(c128)} | 256px: {len(c256)}")

print("Saving individual chunks...")
for i, (sz, x, y, img) in enumerate(c64):
    img.save(OUT_DIR / f"chunk_64_{i:02d}_x{x}y{y}.png")
for i, (sz, x, y, img) in enumerate(c128):
    img.save(OUT_DIR / f"chunk_128_{i:02d}_x{x}y{y}.png")
for i, (sz, x, y, img) in enumerate(c256):
    img.save(OUT_DIR / f"chunk_256_{i:02d}_x{x}y{y}.png")

# Inspection grid: show all 64s, 128s, 256s in rows
def build_inspection_grid(c64, c128, c256, scale=4):
    pad = 8
    row64_h = 64 * scale + pad
    row128_h = 128 * scale + pad
    row256_h = 128 * scale + pad
    cols64 = min(10, len(c64))
    cols128 = min(8, len(c128))
    cols256 = min(8, len(c256))
    W_grid = max(cols64 * (64 * scale + pad), cols128 * (128 * scale + pad), cols256 * (128 * scale + pad)) + pad
    H_grid = row64_h * 2 + row128_h * 2 + row256_h + pad * 6
    canvas = Image.new("RGBA", (W_grid, H_grid), (20, 18, 24, 255))
    y = pad
    for row_start in range(0, len(c64), cols64):
        for col, idx in enumerate(range(row_start, min(row_start + cols64, len(c64)))):
            _, _, _, img = c64[idx]
            big = img.resize((64 * scale, 64 * scale), Image.NEAREST)
            canvas.paste(big, (pad + col * (64 * scale + pad), y), big)
        y += row64_h
    y += pad
    for row_start in range(0, len(c128), cols128):
        for col, idx in enumerate(range(row_start, min(row_start + cols128, len(c128)))):
            _, _, _, img = c128[idx]
            big = img.resize((128 * scale, 128 * scale), Image.NEAREST)
            canvas.paste(big, (pad + col * (128 * scale + pad), y), big)
        y += row128_h
    y += pad
    for col, item in enumerate(c256[:cols256]):
        _, _, _, img = item
        small = img.resize((128 * scale, 128 * scale), Image.LANCZOS)
        canvas.paste(small, (pad + col * (128 * scale + pad), y), small)
    return canvas

grid = build_inspection_grid(c64, c128, c256)
grid_path = OUT_DIR / "_inspection_grid.png"
grid.save(grid_path)
print(f"Grid: {grid_path} ({grid.size})")

# Resize for Read tool 2000px limit
if grid.width > 1800:
    ratio = 1800 / grid.width
    grid_preview = grid.resize((1800, int(grid.height * ratio)), Image.LANCZOS)
    preview_path = OUT_DIR / "_inspection_grid_preview.png"
    grid_preview.save(preview_path)
    print(f"Preview (resized): {preview_path} ({grid_preview.size})")
