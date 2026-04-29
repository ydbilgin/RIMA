"""
Hero Siege wiki sprite cleaner — crop frame, remove dark background, isolate character, resize 64x64.

Strategy:
1. Hard-crop the ornamental frame border (outer ~10px on each side)
2. Use brightness + saturation threshold to separate character from dark dungeon bg
3. Morphological cleanup (remove stray pixels)
4. Find tight bounding box of character
5. Center on 64x64 transparent canvas
"""

from PIL import Image
import numpy as np
from pathlib import Path
import sys

INPUT_DIR = Path(__file__).parent
OUTPUT_DIR = INPUT_DIR.parent / "hero_siege_wiki_sprites_64"
OUTPUT_DIR.mkdir(exist_ok=True)

# Frame crop margins (the ornamental border)
FRAME_MARGIN = 12  # pixels to crop from each edge

# Threshold for "character pixel" vs "dark background"
# Hero Siege sprites have bright characters on very dark dungeon bg
BRIGHTNESS_THRESH = 35  # max RGB channel > this = potential character
SATURATION_THRESH = 15  # for colored pixels with low brightness

TARGET_SIZE = 64
PADDING = 2  # pixel padding around character in final 64x64


def extract_character(img_path: Path) -> Image.Image:
    """Extract character from frame+dungeon background."""
    img = Image.open(img_path).convert("RGBA")
    arr = np.array(img)
    
    h, w = arr.shape[:2]
    
    # Step 1: Hard-crop the ornamental frame
    cropped = arr[FRAME_MARGIN:h-FRAME_MARGIN, FRAME_MARGIN:w-FRAME_MARGIN].copy()
    ch, cw = cropped.shape[:2]
    
    # Step 2: Build character mask using brightness + saturation
    rgb = cropped[:, :, :3].astype(float)
    
    # Max channel brightness
    brightness = rgb.max(axis=2)
    
    # Saturation (max - min channel difference)
    saturation = rgb.max(axis=2) - rgb.min(axis=2)
    
    # Character pixels: bright enough OR colorful enough
    char_mask = (brightness > BRIGHTNESS_THRESH) | (saturation > SATURATION_THRESH)
    
    # Step 3: Morphological cleanup — remove isolated pixels
    # Simple: require at least 2 neighbors
    from scipy import ndimage
    
    # Dilate then erode to fill small gaps in character
    struct = ndimage.generate_binary_structure(2, 1)
    char_mask_clean = ndimage.binary_dilation(char_mask, struct, iterations=1)
    char_mask_clean = ndimage.binary_erosion(char_mask_clean, struct, iterations=1)
    
    # Remove small connected components (noise)
    labeled, num_features = ndimage.label(char_mask_clean)
    if num_features > 0:
        component_sizes = ndimage.sum(char_mask_clean, labeled, range(1, num_features + 1))
        # Keep only components larger than 50 pixels (character body)
        for i, size in enumerate(component_sizes):
            if size < 50:
                char_mask_clean[labeled == (i + 1)] = False
    
    # Step 4: Find bounding box
    rows = np.any(char_mask_clean, axis=1)
    cols = np.any(char_mask_clean, axis=0)
    
    if not rows.any():
        print(f"  WARNING: No character found in {img_path.name}, using fallback")
        # Fallback: use center crop
        rmin, rmax = ch // 4, ch * 3 // 4
        cmin, cmax = cw // 4, cw * 3 // 4
    else:
        rmin, rmax = np.where(rows)[0][[0, -1]]
        cmin, cmax = np.where(cols)[0][[0, -1]]
    
    # Step 5: Extract character region with transparency
    char_region = cropped[rmin:rmax+1, cmin:cmax+1].copy()
    
    # Set non-character pixels to transparent
    mask_region = char_mask_clean[rmin:rmax+1, cmin:cmax+1]
    char_region[~mask_region, 3] = 0  # transparent
    
    # Step 6: Fit into 64x64 canvas centered
    char_h, char_w = char_region.shape[:2]
    
    # Calculate scale to fit in TARGET_SIZE with padding
    available = TARGET_SIZE - (PADDING * 2)
    scale = min(available / char_w, available / char_h)
    
    if scale < 1.0:
        # Need to shrink
        new_w = max(1, int(char_w * scale))
        new_h = max(1, int(char_h * scale))
    else:
        # Keep original size or upscale slightly if too small
        new_w = char_w
        new_h = char_h
    
    char_img = Image.fromarray(char_region)
    if scale < 1.0:
        char_img = char_img.resize((new_w, new_h), Image.LANCZOS)
    
    # Create 64x64 transparent canvas
    canvas = Image.new("RGBA", (TARGET_SIZE, TARGET_SIZE), (0, 0, 0, 0))
    
    # Center the character (slightly toward bottom for top-down feel)
    paste_x = (TARGET_SIZE - char_img.width) // 2
    paste_y = (TARGET_SIZE - char_img.height) // 2 + 1  # slight bottom bias
    
    canvas.paste(char_img, (paste_x, paste_y), char_img)
    
    return canvas


def main():
    png_files = sorted(INPUT_DIR.glob("*.png"))
    png_files = [f for f in png_files if f.name != "crop_sprites.py"]
    
    print(f"Processing {len(png_files)} sprites...")
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
    
    print(f"\nDone! {len(results)}/{len(png_files)} sprites processed.")
    print(f"Output directory: {OUTPUT_DIR}")
    
    # Create a contact sheet for easy review
    if results:
        cols = 6
        rows_needed = (len(results) + cols - 1) // cols
        sheet_w = cols * (TARGET_SIZE + 4) + 4
        sheet_h = rows_needed * (TARGET_SIZE + 4) + 4
        
        sheet = Image.new("RGBA", (sheet_w, sheet_h), (30, 30, 30, 255))
        
        for i, (name, path) in enumerate(results):
            sprite = Image.open(path)
            r = i // cols
            c = i % cols
            x = 4 + c * (TARGET_SIZE + 4)
            y = 4 + r * (TARGET_SIZE + 4)
            sheet.paste(sprite, (x, y), sprite)
        
        sheet_path = OUTPUT_DIR / "_contact_sheet.png"
        sheet.save(sheet_path)
        print(f"Contact sheet: {sheet_path}")


if __name__ == "__main__":
    main()
