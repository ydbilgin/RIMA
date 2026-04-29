"""
Hero Siege wiki sprite cleaner V3 — color-aware frame removal + flood-fill.

Key insight: The ornamental frame has a distinct reddish-brown color signature
(R dominant, R~50-72, G~20-34, B~15-27). We specifically target and remove these pixels.
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
PADDING = 2


def is_frame_color(r, g, b):
    """Detect the ornamental frame's reddish-brown color signature."""
    # Frame colors: R=40-80, G=15-40, B=10-30, R is dominant
    # Also detect the grayish stone platform: low saturation, brightness 30-70
    
    brightness = max(r, g, b)
    saturation = max(r, g, b) - min(r, g, b)
    
    # Reddish-brown frame ornament
    if r > g * 1.3 and r > b * 1.5 and 30 < r < 90 and g < 45 and b < 35:
        return True
    
    # Grayish stone platform (low saturation, medium brightness)
    if saturation < 20 and 25 < brightness < 75 and r > 20:
        return True
    
    # Dark brownish stone (the platform body)  
    if 25 < brightness < 60 and saturation < 25:
        return True
    
    return False


def extract_character(img_path: Path) -> Image.Image:
    """Extract character using combined frame-color removal + flood-fill."""
    img = Image.open(img_path).convert("RGBA")
    arr = np.array(img)
    h, w = arr.shape[:2]
    
    # Step 1: More aggressive frame crop
    margin = 8
    cropped = arr[margin:h-margin, margin:w-margin].copy()
    ch, cw = cropped.shape[:2]
    
    # Step 2: Build frame color mask
    frame_mask = np.zeros((ch, cw), dtype=bool)
    rgb = cropped[:, :, :3]
    
    for y in range(ch):
        for x in range(cw):
            r, g, b = int(rgb[y, x, 0]), int(rgb[y, x, 1]), int(rgb[y, x, 2])
            if is_frame_color(r, g, b):
                frame_mask[y, x] = True
    
    # Step 3: Flood-fill dark background from edges
    brightness = rgb.astype(float).max(axis=2)
    
    bg_mask = np.zeros((ch, cw), dtype=bool)
    visited = np.zeros((ch, cw), dtype=bool)
    queue = deque()
    
    # Seed edges - anything dark OR frame-colored
    for x in range(cw):
        for y_edge in [0, 1, 2, ch-1, ch-2, ch-3]:
            if brightness[y_edge, x] <= 50 or frame_mask[y_edge, x]:
                if not visited[y_edge, x]:
                    queue.append((y_edge, x))
                    visited[y_edge, x] = True
    for y in range(ch):
        for x_edge in [0, 1, 2, cw-1, cw-2, cw-3]:
            if brightness[y, x_edge] <= 50 or frame_mask[y, x_edge]:
                if not visited[y, x_edge]:
                    queue.append((y, x_edge))
                    visited[y, x_edge] = True
    
    # BFS flood fill: expand through dark AND frame-colored pixels
    while queue:
        cy, cx = queue.popleft()
        bg_mask[cy, cx] = True
        
        for dy, dx in [(-1, 0), (1, 0), (0, -1), (0, 1), (-1, -1), (-1, 1), (1, -1), (1, 1)]:
            ny, nx = cy + dy, cx + dx
            if 0 <= ny < ch and 0 <= nx < cw and not visited[ny, nx]:
                visited[ny, nx] = True
                # Expand to: dark pixels, frame-colored pixels, or very dark pixels
                if brightness[ny, nx] <= 50 or frame_mask[ny, nx] or brightness[ny, nx] <= 25:
                    queue.append((ny, nx))
    
    # Step 4: Also mark frame-colored pixels that weren't reached by flood-fill
    # but are within the outer 30% band of the image (definitely frame, not character)
    border_band_h = int(ch * 0.2)
    border_band_w = int(cw * 0.2)
    
    for y in range(ch):
        for x in range(cw):
            if frame_mask[y, x]:
                # If near edge, definitely remove
                if y < border_band_h or y >= ch - border_band_h or x < border_band_w or x >= cw - border_band_w:
                    bg_mask[y, x] = True
    
    # Step 5: Bottom platform removal - aggressive
    # The platform spans the bottom portion and is wider than the character
    # Scan from bottom, remove rows where width > character body width
    char_mask = ~bg_mask & (cropped[:, :, 3] > 128)
    
    # Find character body center columns (top half of character)
    top_half = char_mask[:ch//2, :]
    if top_half.any():
        top_cols = np.any(top_half, axis=0)
        body_width = top_cols.sum()
    else:
        body_width = cw // 3
    
    # From bottom up, remove rows that are significantly wider than body
    for y in range(ch - 1, ch // 2, -1):
        row_width = char_mask[y, :].sum()
        if row_width > body_width * 1.8 and row_width > 15:
            char_mask[y, :] = False
        elif row_width > cw * 0.5:
            char_mask[y, :] = False
    
    # Step 6: Clean up small components
    labeled, num = ndimage.label(char_mask)
    if num > 0:
        sizes = ndimage.sum(char_mask, labeled, range(1, num + 1))
        max_size = max(sizes)
        for i, sz in enumerate(sizes):
            if sz < max_size * 0.04:
                char_mask[labeled == (i + 1)] = False
    
    # Step 7: Bounding box
    rows = np.any(char_mask, axis=1)
    cols = np.any(char_mask, axis=0)
    
    if not rows.any():
        print(f"  WARNING: Empty result for {img_path.name}")
        rmin, rmax = ch // 4, ch * 3 // 4
        cmin, cmax = cw // 4, cw * 3 // 4
    else:
        rmin, rmax = np.where(rows)[0][[0, -1]]
        cmin, cmax = np.where(cols)[0][[0, -1]]
    
    # Step 8: Extract + transparency
    char_region = cropped[rmin:rmax+1, cmin:cmax+1].copy()
    mask_region = char_mask[rmin:rmax+1, cmin:cmax+1]
    char_region[~mask_region, 3] = 0
    
    # Step 9: Fit into 64x64
    char_h, char_w = char_region.shape[:2]
    available = TARGET_SIZE - (PADDING * 2)
    scale = min(available / char_w, available / char_h)
    
    char_img = Image.fromarray(char_region)
    if scale < 1.0:
        new_w = max(1, int(char_w * scale))
        new_h = max(1, int(char_h * scale))
        char_img = char_img.resize((new_w, new_h), Image.LANCZOS)
    
    canvas = Image.new("RGBA", (TARGET_SIZE, TARGET_SIZE), (0, 0, 0, 0))
    paste_x = (TARGET_SIZE - char_img.width) // 2
    paste_y = (TARGET_SIZE - char_img.height) // 2 + 1
    canvas.paste(char_img, (paste_x, paste_y), char_img)
    
    return canvas


def main():
    png_files = sorted(INPUT_DIR.glob("*.png"))
    png_files = [f for f in png_files if not f.name.startswith("crop_")]
    
    print(f"Processing {len(png_files)} sprites (V3 color-aware)...")
    print(f"Output: {OUTPUT_DIR}")
    print()
    
    results = []
    for f in png_files:
        try:
            result = extract_character(f)
            out_path = OUTPUT_DIR / f.name
            result.save(out_path)
            print(f"  OK {f.stem:20s} -> {out_path.name}")
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
