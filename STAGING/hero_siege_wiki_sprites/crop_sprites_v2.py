"""
Hero Siege wiki sprite cleaner V2 — flood-fill background removal.

Strategy:
1. Hard-crop the ornamental frame (outer border)
2. Flood-fill from edges to remove connected dark background
3. The character sits on a dark pedestal/platform — detect and remove it
4. Find tight bounding box of remaining character pixels
5. Center on 64x64 transparent canvas
"""

from PIL import Image
import numpy as np
from pathlib import Path
from scipy import ndimage
from collections import deque

INPUT_DIR = Path(__file__).parent
OUTPUT_DIR = INPUT_DIR.parent / "hero_siege_wiki_sprites_64"
OUTPUT_DIR.mkdir(exist_ok=True)

FRAME_MARGIN = 14       # crop ornamental border
BG_THRESHOLD = 45       # max brightness to consider as "background" for flood fill
PLATFORM_CUT_ROWS = 20  # rows from bottom to aggressively cut (pedestal/platform area)
TARGET_SIZE = 64
PADDING = 2


def flood_fill_background(arr, threshold):
    """Flood-fill from edges to find connected background region."""
    h, w = arr.shape[:2]
    rgb = arr[:, :, :3].astype(float)
    brightness = rgb.max(axis=2)
    
    # Background mask: True = background
    bg_mask = np.zeros((h, w), dtype=bool)
    visited = np.zeros((h, w), dtype=bool)
    
    queue = deque()
    
    # Seed from all edge pixels that are dark enough
    for x in range(w):
        for y in [0, h - 1]:
            if brightness[y, x] <= threshold:
                queue.append((y, x))
                visited[y, x] = True
    for y in range(h):
        for x in [0, w - 1]:
            if brightness[y, x] <= threshold:
                queue.append((y, x))
                visited[y, x] = True
    
    # BFS flood fill
    while queue:
        cy, cx = queue.popleft()
        bg_mask[cy, cx] = True
        
        for dy, dx in [(-1, 0), (1, 0), (0, -1), (0, 1)]:
            ny, nx = cy + dy, cx + dx
            if 0 <= ny < h and 0 <= nx < w and not visited[ny, nx]:
                visited[ny, nx] = True
                if brightness[ny, nx] <= threshold:
                    queue.append((ny, nx))
    
    return bg_mask


def remove_platform(char_mask, arr, h, w):
    """Remove the pedestal/platform from the bottom of the image.
    
    The platform is a wide horizontal structure at the bottom.
    Strategy: find the row where horizontal coverage suddenly jumps.
    """
    # Scan from bottom up, find where the character "body" starts vs platform
    for y in range(h - 1, max(h - PLATFORM_CUT_ROWS, 0), -1):
        row_coverage = char_mask[y, :].sum()
        row_above_coverage = char_mask[max(0, y - 3), :].sum() if y > 3 else 0
        
        # Platform: wide coverage at bottom that's wider than the body above
        if row_coverage > w * 0.4:  # more than 40% width = likely platform
            char_mask[y, :] = False
        elif row_coverage > row_above_coverage * 2.0 and row_coverage > 20:
            # Sudden width increase = platform top edge
            char_mask[y, :] = False
    
    return char_mask


def extract_character(img_path: Path) -> Image.Image:
    """Extract character from frame+dungeon background using flood fill."""
    img = Image.open(img_path).convert("RGBA")
    arr = np.array(img)
    h, w = arr.shape[:2]
    
    # Step 1: Hard-crop frame
    cropped = arr[FRAME_MARGIN:h-FRAME_MARGIN, FRAME_MARGIN:w-FRAME_MARGIN].copy()
    ch, cw = cropped.shape[:2]
    
    # Step 2: Flood-fill background from edges
    bg_mask = flood_fill_background(cropped, BG_THRESHOLD)
    
    # Character mask = NOT background AND has alpha
    char_mask = ~bg_mask & (cropped[:, :, 3] > 128)
    
    # Step 3: Remove small noise components
    labeled, num_features = ndimage.label(char_mask)
    if num_features > 0:
        component_sizes = ndimage.sum(char_mask, labeled, range(1, num_features + 1))
        # Keep only the largest component (the character) + any big enough parts
        max_size = max(component_sizes)
        for i, size in enumerate(component_sizes):
            if size < max_size * 0.05 and size < 30:  # less than 5% of largest or < 30px
                char_mask[labeled == (i + 1)] = False
    
    # Step 4: Remove platform aggressively
    char_mask = remove_platform(char_mask, cropped, ch, cw)
    
    # Step 5: Additional cleanup — remove frame corner remnants
    # Frame corners are typically in the outer 15% of each edge
    frame_zone = int(ch * 0.12)
    # Top-left corner
    char_mask[:frame_zone, :frame_zone] = False
    # Top-right corner
    char_mask[:frame_zone, cw-frame_zone:] = False
    # Bottom-left corner  
    char_mask[ch-frame_zone:, :frame_zone] = False
    # Bottom-right corner
    char_mask[ch-frame_zone:, cw-frame_zone:] = False
    
    # Step 6: Remove remaining isolated small blobs after corner cleanup
    labeled2, num2 = ndimage.label(char_mask)
    if num2 > 0:
        sizes2 = ndimage.sum(char_mask, labeled2, range(1, num2 + 1))
        max_size2 = max(sizes2)
        for i, sz in enumerate(sizes2):
            if sz < max_size2 * 0.03:
                char_mask[labeled2 == (i + 1)] = False
    
    # Step 7: Find bounding box
    rows = np.any(char_mask, axis=1)
    cols = np.any(char_mask, axis=0)
    
    if not rows.any():
        print(f"  WARNING: No character found in {img_path.name}")
        rmin, rmax = ch // 4, ch * 3 // 4
        cmin, cmax = cw // 4, cw * 3 // 4
    else:
        rmin, rmax = np.where(rows)[0][[0, -1]]
        cmin, cmax = np.where(cols)[0][[0, -1]]
    
    # Step 8: Extract with transparency
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
    
    print(f"Processing {len(png_files)} sprites (V2 flood-fill)...")
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
