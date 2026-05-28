"""
Studio Hibrit Farming Sim — World-Space Splatmap Shader Proof

Codex Q1.2'deki HLSL shader mantığının Python (NumPy) simülasyonu.
Wang tile DEĞİL — per-pixel splatmap blend.

Üretir:
  1. Hand-drawn-tarzı organik dirt path mask (freehand polyline + brush radius)
  2. Same-size base textures (32×32 tileable, world-space sampled)
  3. Bayer 4×4 dither blend → per-pixel grass/dirt
  4. 768×512 output — tile sınırı GÖRÜNMEZ

Bu, kullanıcının Paint çiziminde gösterdiği yumuşak/akışkan kenar disiplini.
"""

import os
import sys
import numpy as np
from PIL import Image, ImageDraw, ImageFilter

# Same Studio Forest palette
COLORS = {
    "grass_dark":  (38, 78, 28),
    "grass_mid":   (54, 102, 42),
    "grass_light": (76, 130, 58),
    "grass_tip":   (104, 158, 78),
    "dirt_dark":   (56, 38, 22),
    "dirt_mid":    (84, 60, 36),
    "dirt_light":  (118, 88, 56),
    "dirt_tip":    (148, 116, 80),
    "stone_mid":   (96, 92, 88),
    "moss_accent": (68, 92, 32),
}

PALETTE_FLAT = sum([list(rgb) for rgb in COLORS.values()], [])
PALETTE_FLAT += [0] * (768 - len(PALETTE_FLAT))

TILE_SIZE = 32
WORLD_WIDTH = 768   # 24 tile × 32 px (same as garden map for direct comparison)
WORLD_HEIGHT = 512  # 16 tile × 32 px

BAYER_4X4 = np.array([
    [ 0,  8,  2, 10],
    [12,  4, 14,  6],
    [ 3, 11,  1,  9],
    [15,  7, 13,  5]
]) / 16.0


def noise_tile_grass(seed=42):
    np.random.seed(seed)
    arr = np.zeros((TILE_SIZE, TILE_SIZE, 3), dtype=np.uint8)
    arr[:, :] = COLORS["grass_mid"]
    for _ in range(80):
        x = np.random.randint(0, TILE_SIZE)
        y = np.random.randint(0, TILE_SIZE)
        r = np.random.random()
        if r < 0.4:
            arr[y, x] = COLORS["grass_light"]
        elif r < 0.7:
            arr[y, x] = COLORS["grass_dark"]
        else:
            arr[y, x] = COLORS["grass_tip"]
    for _ in range(8):
        x = np.random.randint(0, TILE_SIZE)
        y = np.random.randint(0, TILE_SIZE)
        arr[y, x] = COLORS["moss_accent"]
    return arr


def noise_tile_dirt(seed=137):
    np.random.seed(seed)
    arr = np.zeros((TILE_SIZE, TILE_SIZE, 3), dtype=np.uint8)
    arr[:, :] = COLORS["dirt_mid"]
    for _ in range(80):
        x = np.random.randint(0, TILE_SIZE)
        y = np.random.randint(0, TILE_SIZE)
        r = np.random.random()
        if r < 0.4:
            arr[y, x] = COLORS["dirt_light"]
        elif r < 0.7:
            arr[y, x] = COLORS["dirt_dark"]
        elif r < 0.9:
            arr[y, x] = COLORS["dirt_tip"]
        else:
            arr[y, x] = COLORS["stone_mid"]
    for _ in range(4):
        x = np.random.randint(1, TILE_SIZE - 1)
        y = np.random.randint(1, TILE_SIZE - 1)
        arr[y:y+2, x:x+2] = COLORS["stone_mid"]
    return arr


def build_organic_splatmap(width=WORLD_WIDTH, height=WORLD_HEIGHT):
    """
    Build a freehand-style splatmap mask.
    R = dirt strength (0..1). Painted with anti-aliased polyline + Gaussian blur.
    Mimics user's Paint sketch: curved L-shaped path through grass area.
    """
    # Start with full grass (0)
    mask_img = Image.new("L", (width, height), 0)
    draw = ImageDraw.Draw(mask_img)

    # Outer dirt border (~60 px wide on each side, like garden map's 2-tile border)
    # Use polygon fill for organic border
    border_pts = [
        (0, 0), (width, 0), (width, height), (0, height)
    ]
    # Inner organic grass shape (offset from border) — irregular polygon
    inner_grass_left = [
        (90, 60),
        (120, 50),
        (260, 55),
        (290, 70),
        (310, 130),
        (305, 220),
        (280, 280),
        (270, 360),
        (290, 430),
        (260, 460),
        (140, 465),
        (95, 440),
        (75, 380),
        (80, 290),
        (95, 210),
        (85, 140),
        (80, 90),
    ]
    inner_grass_right = [
        (430, 70),
        (480, 55),
        (640, 60),
        (690, 80),
        (705, 160),
        (700, 280),
        (715, 380),
        (690, 440),
        (650, 460),
        (490, 462),
        (440, 440),
        (420, 380),
        (425, 250),
        (415, 160),
    ]
    # Fill the outer dirt
    draw.rectangle([(0, 0), (width, height)], fill=255)
    # Paint grass islands (organic)
    draw.polygon(inner_grass_left, fill=0)
    draw.polygon(inner_grass_right, fill=0)

    # Now add organic dirt features INSIDE the left grass island
    # 1. Curved L-shaped path (like user's Paint sketch)
    curved_path = [
        (200, 70),    # entry from top
        (200, 90),
        (195, 130),
        (190, 180),
        (185, 240),
        (180, 300),
        (175, 350),
        (180, 380),
        (200, 400),
        (240, 415),
        (270, 410),
    ]
    # Draw the path with a thick brush
    for i in range(len(curved_path) - 1):
        draw.line([curved_path[i], curved_path[i + 1]], fill=255, width=22)
    # Add rounded ends (circles) at path nodes for smooth corners
    for px, py in curved_path:
        draw.ellipse([px - 12, py - 12, px + 12, py + 12], fill=255)

    # 2. Small organic dirt blob (like the kidney shape from garden map)
    blob_pts = [
        (220, 230), (235, 220), (250, 225), (260, 240), (255, 260), (245, 270),
        (230, 275), (215, 268), (208, 250)
    ]
    draw.polygon(blob_pts, fill=255)

    # Right island: small dirt patch (bottom-right of right plot)
    right_patch_pts = [
        (560, 360), (605, 355), (640, 370), (635, 410), (605, 425), (565, 420), (555, 390)
    ]
    draw.polygon(right_patch_pts, fill=255)

    # Apply Gaussian blur for soft transitions — like brush bleed
    mask_blurred = mask_img.filter(ImageFilter.GaussianBlur(radius=2.5))

    return np.array(mask_blurred, dtype=np.float32) / 255.0


def sample_tileable(texture, world_x, world_y):
    """Sample a tileable 32x32 texture at world coords (wrap)."""
    h, w = texture.shape[:2]
    return texture[world_y % h, world_x % w]


def render_splatmap(grass_tile, dirt_tile, mask, width=WORLD_WIDTH, height=WORLD_HEIGHT):
    """
    Per-pixel splatmap shader simulation.
    For each pixel:
      - Sample grass and dirt at world position (tileable wrap)
      - Get splatmap mask value
      - Apply Bayer 4x4 dither threshold for crisp pixel-art edges
      - Choose grass or dirt
    """
    canvas = np.zeros((height, width, 3), dtype=np.uint8)

    # Pre-tile Bayer pattern
    bayer = np.tile(BAYER_4X4, (height // 4 + 1, width // 4 + 1))[:height, :width]

    for y in range(height):
        for x in range(width):
            g = sample_tileable(grass_tile, x, y)
            d = sample_tileable(dirt_tile, x, y)
            m = mask[y, x]
            # Bayer dither threshold for pixel-art crisp edges
            if m > bayer[y, x]:
                canvas[y, x] = d
            else:
                canvas[y, x] = g

    return canvas


def render_splatmap_vectorized(grass_tile, dirt_tile, mask, width=WORLD_WIDTH, height=WORLD_HEIGHT):
    """Vectorized version (much faster than per-pixel loop)."""
    bayer = np.tile(BAYER_4X4, (height // 4 + 1, width // 4 + 1))[:height, :width]

    # Tile grass/dirt to full size
    h_t, w_t = grass_tile.shape[:2]
    ys = np.arange(height) % h_t
    xs = np.arange(width) % w_t
    grass_full = grass_tile[np.ix_(ys, xs)]
    dirt_full = dirt_tile[np.ix_(ys, xs)]

    # Per-pixel choice
    dirt_mask = mask > bayer
    canvas = np.where(dirt_mask[..., None], dirt_full, grass_full)
    return canvas.astype(np.uint8)


def apply_palette(arr_rgb):
    pil = Image.fromarray(arr_rgb, mode="RGB")
    pal_img = Image.new("P", (1, 1))
    pal_img.putpalette(PALETTE_FLAT)
    quantized = pil.quantize(palette=pal_img, dither=Image.Dither.NONE)
    return np.array(quantized.convert("RGB"))


def save_png(arr, path, scale=1):
    img = Image.fromarray(arr, mode="RGB")
    if scale > 1:
        img = img.resize((arr.shape[1] * scale, arr.shape[0] * scale), Image.NEAREST)
    img.save(path)


def main():
    base = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
    out_dir = os.path.join(base, "STAGING", "wang_proof")
    os.makedirs(out_dir, exist_ok=True)
    print(f"[Splatmap Shader Proof] Output: {out_dir}")

    # Base tiles (same as Wang demo for direct comparison)
    grass = noise_tile_grass(seed=42)
    dirt = noise_tile_dirt(seed=137)
    grass_pal = apply_palette(grass)
    dirt_pal = apply_palette(dirt)

    # Build organic splatmap mask (freehand-style)
    print("  Building organic splatmap mask...")
    mask = build_organic_splatmap()
    # Save mask as grayscale PNG for inspection
    mask_uint8 = (mask * 255).astype(np.uint8)
    Image.fromarray(mask_uint8, mode="L").save(os.path.join(out_dir, "10_splatmap_mask.png"))
    print("  [OK] Splatmap mask saved: 10_splatmap_mask.png")

    # Render
    print("  Rendering splatmap shader (vectorized)...")
    canvas = render_splatmap_vectorized(grass_pal, dirt_pal, mask)

    # Apply palette snap to final
    canvas_pal = apply_palette(canvas)

    save_png(canvas_pal, os.path.join(out_dir, "11_splatmap_organic_map.png"), scale=1)
    save_png(canvas_pal, os.path.join(out_dir, "11_splatmap_organic_map_2x.png"), scale=2)
    print(f"  [OK] Organic splatmap map rendered: 11_splatmap_organic_map.png ({canvas_pal.shape[1]}x{canvas_pal.shape[0]} px)")
    print(f"  [OK] 2x scale version: 11_splatmap_organic_map_2x.png")

    print("\n[KARSILASTIRMA]")
    print(f"  Wang 16 map:      04_garden_map_24x16.png    (tile-bound, kose limit)")
    print(f"  Splatmap shader:  11_splatmap_organic_map.png (per-pixel, freehand)")
    print(f"\n  Aynı ölçü (768x512), aynı base tile'lar, AMA splatmap tile sınırı YOK.")
    print(f"  Wang 16 = building/prop için; Splatmap = zemin geçişleri için.")
    print(f"  Codex Q1.2 HLSL shader = bu pipeline'ın Unity URP versiyonu.")

    print(f"\n[Done]")


if __name__ == "__main__":
    main()
