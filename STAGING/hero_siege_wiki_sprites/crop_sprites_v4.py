"""
Hero Siege wiki sprite cleaner V4 — conservative flood-fill, protect character.

Key fix: V3 was too aggressive with is_frame_color, killing dark character armor.
V4 strategy:
1. Identify the ornamental frame by its specific geometry (border ring pattern)
2. Use only flood-fill from edges with moderate threshold
3. Protect interior pixels by stopping flood at brightness jumps
4. Upscale to fill 64x64 canvas properly (nearest-neighbor for pixel art)
"""

from PIL import Image
import numpy as np
from pathlib import Path
from scipy import ndimage
from collections import deque

INPUT_DIR = Path(__file__).parent
OUTPUT_DIR = INPUT_DIR.parent / "hero_siege_wiki_sprites_64"
OUTPUT_DIR.mkdir(exist_ok=True)

TARGET_SIZE = 64
PADDING = 3  # px padding in final canvas


def extract_character(img_path: Path) -> Image.Image:
    img = Image.open(img_path).convert("RGBA")
    arr = np.array(img)
    h, w = arr.shape[:2]
    
    # === STEP 1: Geometric frame removal ===
    # The wiki images have a specific ornamental frame.
    # Inner content area is approximately: top=12, bottom=130, left=12, right=150 in 162x162
    # But varies slightly. Use a safe inner crop.
    
    # Detect frame: frame pixels form a connected ring at the border.
    # Strategy: mask out the outermost N pixels, then flood-fill remaining dark bg.
    
    # Hard crop: remove definite frame border
    crop_top = 12
    crop_bot = 12
    crop_left = 12
    crop_right = 12
    
    inner = arr[crop_top:h-crop_bot, crop_left:w-crop_right].copy()
    ih, iw = inner.shape[:2]
    
    # === STEP 2: Flood-fill background from edges ===
    rgb = inner[:, :, :3].astype(float)
    brightness = rgb.max(axis=2)
    
    # Background is very dark (brightness < 30) or has the frame reddish tone
    # Character pixels are brighter or have distinct hue
    
    bg_mask = np.zeros((ih, iw), dtype=bool)
    visited = np.zeros((ih, iw), dtype=bool)
    queue = deque()
    
    # Seed from edges - be generous, seed anything < 55 brightness
    for x in range(iw):
        for y_e in range(min(5, ih)):
            if not visited[y_e, x]:
                visited[y_e, x] = True
                queue.append((y_e, x))
        for y_e in range(max(0, ih-5), ih):
            if not visited[y_e, x]:
                visited[y_e, x] = True
                queue.append((y_e, x))
    for y in range(ih):
        for x_e in range(min(5, iw)):
            if not visited[y, x_e]:
                visited[y, x_e] = True
                queue.append((y, x_e))
        for x_e in range(max(0, iw-5), iw):
            if not visited[y, x_e]:
                visited[y, x_e] = True
                queue.append((y, x_e))
    
    # BFS: expand through dark pixels only
    # Use a conservative threshold - only truly dark bg pixels
    BG_THRESH = 35  # dark dungeon background
    
    while queue:
        cy, cx = queue.popleft()
        
        if brightness[cy, cx] <= BG_THRESH:
            bg_mask[cy, cx] = True
            
            for dy, dx in [(-1,0),(1,0),(0,-1),(0,1)]:
                ny, nx = cy+dy, cx+dx
                if 0 <= ny < ih and 0 <= nx < iw and not visited[ny, nx]:
                    visited[ny, nx] = True
                    if brightness[ny, nx] <= BG_THRESH:
                        queue.append((ny, nx))
        else:
            # Not dark enough = potential character, mark as bg only if definitely frame
            # Check if this is frame ornament (reddish, at edge zone)
            r, g, b = int(rgb[cy, cx, 0]), int(rgb[cy, cx, 1]), int(rgb[cy, cx, 2])
            is_edge_zone = cy < 8 or cy >= ih-8 or cx < 8 or cx >= iw-8
            is_frame_reddish = (r > g * 1.4 and r > b * 1.6 and r < 85)
            
            if is_edge_zone and is_frame_reddish:
                bg_mask[cy, cx] = True
                for dy, dx in [(-1,0),(1,0),(0,-1),(0,1)]:
                    ny, nx = cy+dy, cx+dx
                    if 0 <= ny < ih and 0 <= nx < iw and not visited[ny, nx]:
                        visited[ny, nx] = True
                        queue.append((ny, nx))
    
    # === STEP 3: Character mask ===
    char_mask = ~bg_mask & (inner[:, :, 3] > 128)
    
    # === STEP 4: Remove platform at bottom ===
    # Find character body width from top 60% of masked area
    rows_any = np.any(char_mask, axis=1)
    if rows_any.any():
        first_row = np.where(rows_any)[0][0]
        last_row = np.where(rows_any)[0][-1]
        char_height = last_row - first_row + 1
        
        # Measure body width in upper portion
        upper_end = first_row + int(char_height * 0.6)
        upper_mask = char_mask[first_row:upper_end, :]
        if upper_mask.any():
            upper_widths = upper_mask.sum(axis=1)
            body_width = np.percentile(upper_widths[upper_widths > 0], 75) if (upper_widths > 0).any() else 20
        else:
            body_width = 20
        
        # Bottom removal: if row width > 2x body width, it's platform
        for y in range(ih - 1, first_row + int(char_height * 0.5), -1):
            row_w = char_mask[y, :].sum()
            if row_w > body_width * 2.0:
                char_mask[y, :] = False
    
    # === STEP 5: Cleanup small components ===
    labeled, num = ndimage.label(char_mask)
    if num > 0:
        sizes = ndimage.sum(char_mask, labeled, range(1, num + 1))
        max_sz = max(sizes)
        for i, sz in enumerate(sizes):
            if sz < max_sz * 0.05:
                char_mask[labeled == (i + 1)] = False
    
    # === STEP 6: Bounding box + extract ===
    rows_final = np.any(char_mask, axis=1)
    cols_final = np.any(char_mask, axis=0)
    
    if not rows_final.any():
        print(f"  WARNING: Empty {img_path.name}")
        return Image.new("RGBA", (TARGET_SIZE, TARGET_SIZE), (0,0,0,0))
    
    rmin, rmax = np.where(rows_final)[0][[0, -1]]
    cmin, cmax = np.where(cols_final)[0][[0, -1]]
    
    char_region = inner[rmin:rmax+1, cmin:cmax+1].copy()
    mask_region = char_mask[rmin:rmax+1, cmin:cmax+1]
    char_region[~mask_region, 3] = 0
    
    # === STEP 7: Scale to fill 64x64 with nearest-neighbor (pixel art!) ===
    char_h, char_w = char_region.shape[:2]
    available = TARGET_SIZE - (PADDING * 2)
    scale = min(available / char_w, available / char_h)
    
    char_img = Image.fromarray(char_region)
    
    # Always scale (up or down) using NEAREST for clean pixel art
    new_w = max(1, int(char_w * scale))
    new_h = max(1, int(char_h * scale))
    char_img = char_img.resize((new_w, new_h), Image.NEAREST)
    
    canvas = Image.new("RGBA", (TARGET_SIZE, TARGET_SIZE), (0, 0, 0, 0))
    paste_x = (TARGET_SIZE - char_img.width) // 2
    paste_y = (TARGET_SIZE - char_img.height) // 2 + 1  # slight bottom bias
    canvas.paste(char_img, (paste_x, paste_y), char_img)
    
    return canvas


def main():
    png_files = sorted(INPUT_DIR.glob("*.png"))
    png_files = [f for f in png_files if not f.name.startswith("crop_")]
    
    print(f"Processing {len(png_files)} sprites (V4 conservative flood-fill + NEAREST scale)...")
    print(f"Output: {OUTPUT_DIR}")
    print()
    
    results = []
    for f in png_files:
        try:
            result = extract_character(f)
            out_path = OUTPUT_DIR / f.name
            result.save(out_path)
            
            # Report actual character pixel count
            arr = np.array(result)
            opaque = (arr[:,:,3] > 10).sum()
            print(f"  OK {f.stem:20s} -> {out_path.name} ({opaque}px)")
            results.append((f.stem, out_path))
        except Exception as e:
            print(f"  FAIL {f.stem:20s} -> ERROR: {e}")
            import traceback
            traceback.print_exc()
    
    print(f"\nDone! {len(results)}/{len(png_files)} sprites processed.")
    
    # Contact sheet
    if results:
        cols = 6
        rows_needed = (len(results) + cols - 1) // cols
        cell = TARGET_SIZE + 4
        sheet = Image.new("RGBA", (cols * cell + 4, rows_needed * cell + 4), (30, 30, 30, 255))
        
        for i, (name, path) in enumerate(results):
            sprite = Image.open(path)
            r = i // cols
            c = i % cols
            sheet.paste(sprite, (4 + c * cell, 4 + r * cell), sprite)
        
        sheet_path = OUTPUT_DIR / "_contact_sheet.png"
        sheet.save(sheet_path)
        print(f"Contact sheet: {sheet_path}")


if __name__ == "__main__":
    main()
