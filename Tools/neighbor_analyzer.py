#!/usr/bin/env python3
"""
PainterSuite v2 - NeighborAnalyzer.py
Automates the detection of Wang 16 autotile ID from 32x32 pixel cells.
"""

import sys
import argparse
import numpy as np
from PIL import Image

def get_pixel_class(rgb_pixel, ref_grass, ref_dirt):
    """Classifies a single pixel based on Euclidean distance in RGB space."""
    dist_grass = np.linalg.norm(np.array(rgb_pixel) - np.array(ref_grass))
    dist_dirt = np.linalg.norm(np.array(rgb_pixel) - np.array(ref_dirt))
    return 1 if dist_dirt < dist_grass else 0

def analyze_edge(cell_img, edge_pixels=4,
                 dominant_color_grass=(46, 92, 30),
                 dominant_color_dirt=(120, 80, 50),
                 threshold=0.5,
                 corner_exclude=4):
    """
    Analyzes the 4 edge bands of a 32x32 tile and calculates the Wang 16 ID.

    Wang 16 Encoding Formula:
    ID = N*8 + E*4 + S*2 + W*1

    Where:
    0: Grass (Zemin/Background)
    1: Dirt (Toprak/Active connection)

    corner_exclude: number of pixels at each end of edge band to exclude
    from analysis (avoids cross-edge contamination at corners). Default 4.
    """
    img = cell_img.convert('RGB')
    arr = np.array(img)

    height, width, _ = arr.shape
    if height != 32 or width != 32:
        raise ValueError(f"Invalid tile size: {width}x{height}. Must be 32x32 pixels.")

    # Slice the edge bands, EXCLUDING corner zones (corner_exclude pixels at each end)
    # This isolates the central section of each edge to avoid contamination
    # from adjacent dirt edges (e.g. ID 5 East+West dirt should not bleed N/S detection).
    ce = corner_exclude
    north_band = arr[0:edge_pixels, ce:width-ce]              # Top, excluding W/E corners
    east_band = arr[ce:height-ce, width-edge_pixels:width]    # Right, excluding N/S corners
    south_band = arr[height-edge_pixels:height, ce:width-ce]  # Bottom, excluding W/E corners
    west_band = arr[ce:height-ce, 0:edge_pixels]              # Left, excluding N/S corners

    bands = {
        'N': north_band,
        'E': east_band,
        'S': south_band,
        'W': west_band
    }
    
    binary_results = {}
    
    for direction, band in bands.items():
        pixels = band.reshape(-1, 3)
        dirt_count = 0
        total_pixels = len(pixels)
        
        for px in pixels:
            if get_pixel_class(px, dominant_color_grass, dominant_color_dirt) == 1:
                dirt_count += 1
                
        dirt_ratio = dirt_count / total_pixels
        binary_results[direction] = 1 if dirt_ratio >= threshold else 0
        
    n = binary_results['N']
    e = binary_results['E']
    s = binary_results['S']
    w = binary_results['W']
    
    wang_id = (n * 8) + (e * 4) + (s * 2) + (w * 1)
    
    return wang_id, binary_results

def parse_color(color_str):
    """Parses a comma-separated RGB string (e.g. '46,92,30') into an RGB tuple."""
    try:
        return tuple(map(int, color_str.split(',')))
    except Exception:
        raise argparse.ArgumentTypeError("Colors must be in R,G,B format (e.g., 46,92,30)")

if __name__ == '__main__':
    parser = argparse.ArgumentParser(description="Analyze a 32x32 tile's edges for Wang 16 autotiling.")
    parser.add_argument("--image", type=str, help="Path to the 32x32 image file. If omitted, a self-test will be run.")
    parser.add_argument("--edge-width", type=int, default=4, help="Width of edge band in pixels (default: 4)")
    parser.add_argument("--grass-color", type=parse_color, default="46,92,30", help="RGB grass color (default: 46,92,30)")
    parser.add_argument("--dirt-color", type=parse_color, default="120,80,50", help="RGB dirt color (default: 120,80,50)")
    parser.add_argument("--threshold", type=float, default=0.5, help="Dirt ratio threshold for edge detection (default: 0.5)")
    
    args = parser.parse_args()
    
    if args.image:
        try:
            img = Image.open(args.image)
            wang_id, bitmask = analyze_edge(img, args.edge_width, args.grass_color, args.dirt_color, args.threshold)
            print(f"File: {args.image}")
            print(f"Wang 16 ID: {wang_id}")
            print(f"Bitmask (N,E,S,W): {bitmask}")
        except Exception as e:
            print(f"Error: {e}")
            sys.exit(1)
    else:
        print("[Self-Test] Running NeighborAnalyzer self-test with a generated dummy image...")
        # Create a dummy 32x32 image: 
        # Left half grass, Right half dirt (W=0, E=1, N/S mixed depending on threshold)
        test_arr = np.zeros((32, 32, 3), dtype=np.uint8)
        
        # Fill left 16 cols with Grass (46, 92, 30)
        test_arr[:, 0:16] = [46, 92, 30]
        # Fill right 16 cols with Dirt (120, 80, 50)
        test_arr[:, 16:32] = [120, 80, 50]
        
        dummy_img = Image.fromarray(test_arr)
        
        wang_id, bitmask = analyze_edge(dummy_img, args.edge_width, args.grass_color, args.dirt_color, args.threshold)
        print(f"Generated test image: Left half GRASS, Right half DIRT.")
        print(f"Resulting Wang 16 ID: {wang_id}")
        print(f"Bitmask results: {bitmask}")
        print("Expected: N=0 or 1 (mixed 50-50, threshold=0.5 counts as 1), E=1, S=0 or 1, W=0.")
        print("Self-test completed successfully!")
