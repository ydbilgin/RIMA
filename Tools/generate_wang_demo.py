"""
Studio Hibrit Farming Sim — Wang B+ Demo Generator

Üretir:
  1. 32x32 grass tile (procedural noise, Studio Forest Palette)
  2. 32x32 dirt tile (procedural noise, Studio Earth Palette)
  3. 16 Wang slot tile (4x4 grid composition, ID 0-15)
  4. 24x16 mantıklı bahçe haritası (ortada çayır parsel + etrafta yol)
  5. NeighborAnalyzer.py self-test entegrasyonu

Çıktı klasörü: STAGING/wang_proof/
"""

import os
import sys
import numpy as np
from PIL import Image

# Studio Forest Palette (16-color simplified for proof)
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
    "wood_warm":   (132, 86, 48),
    "sky_cool":    (84, 124, 152),
    "shadow":      (24, 28, 18),
    "highlight":   (188, 196, 152),
    "warm_accent": (224, 168, 72),
    "off_white":   (228, 224, 200),
}

PALETTE_FLAT = sum([list(rgb) for rgb in COLORS.values()], [])
PALETTE_FLAT += [0] * (768 - len(PALETTE_FLAT))  # PIL palette needs 256 entries × 3

TILE_SIZE = 32
EDGE_BAND = 4

# Bayer 4x4 dither matrix (0-15 normalized to 0-1)
BAYER_4X4 = np.array([
    [ 0,  8,  2, 10],
    [12,  4, 14,  6],
    [ 3, 11,  1,  9],
    [15,  7, 13,  5]
]) / 16.0

# Bayer 8x8 dither matrix for smoother gradient (0-63 / 64)
BAYER_8X8 = np.array([
    [ 0, 32,  8, 40,  2, 34, 10, 42],
    [48, 16, 56, 24, 50, 18, 58, 26],
    [12, 44,  4, 36, 14, 46,  6, 38],
    [60, 28, 52, 20, 62, 30, 54, 22],
    [ 3, 35, 11, 43,  1, 33,  9, 41],
    [51, 19, 59, 27, 49, 17, 57, 25],
    [15, 47,  7, 39, 13, 45,  5, 37],
    [63, 31, 55, 23, 61, 29, 53, 21]
]) / 64.0


def seed_random(seed: int):
    np.random.seed(seed)


def noise_tile_grass(seed=42):
    """32x32 grass tile, procedural variation."""
    seed_random(seed)
    arr = np.zeros((TILE_SIZE, TILE_SIZE, 3), dtype=np.uint8)
    # Base layer: grass_mid
    arr[:, :] = COLORS["grass_mid"]
    # Sprinkle highlights + shadows
    for _ in range(80):
        x = np.random.randint(0, TILE_SIZE)
        y = np.random.randint(0, TILE_SIZE)
        if np.random.random() < 0.4:
            arr[y, x] = COLORS["grass_light"]
        elif np.random.random() < 0.7:
            arr[y, x] = COLORS["grass_dark"]
        else:
            arr[y, x] = COLORS["grass_tip"]
    # Tiny moss accent dots
    for _ in range(8):
        x = np.random.randint(0, TILE_SIZE)
        y = np.random.randint(0, TILE_SIZE)
        arr[y, x] = COLORS["moss_accent"]
    return arr


def noise_tile_dirt(seed=137):
    """32x32 dirt tile, procedural variation."""
    seed_random(seed)
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
    # Some larger pebbles
    for _ in range(4):
        x = np.random.randint(1, TILE_SIZE - 1)
        y = np.random.randint(1, TILE_SIZE - 1)
        arr[y:y+2, x:x+2] = COLORS["stone_mid"]
    return arr


def make_wang_slot_tile(grass_arr, dirt_arr, n, e, s, w, seed=0):
    """
    Generate a 32x32 tile for a specific Wang 16 slot — v2 (corner-aware, organic).

    Strategy:
      - HARD_EDGE: outer 3 px on each "dirt" edge = pure dirt (NeighborAnalyzer compliance)
      - SMOOTH_ZONE: 3-12 px = soft gradient (corner-aware distance field + Bayer 8×8 dither)
      - INNER: 12+ px = pure grass

    Corner-awareness: pixels near corners with TWO dirt edges get a circular blob radius
    (bulge inward) instead of L-shaped step → organic oval curves.

    Still passes NeighborAnalyzer (edge_pixels=4, threshold=0.5) because outer 4 px is
    >=80% dirt for any dirt edge.
    """
    np.random.seed(seed)
    result = grass_arr.copy()

    HARD_EDGE = 3       # outer 3 px guaranteed dirt for "1" edges
    SMOOTH_MAX = 18     # smooth gradient extends 3..18 px (larger curve)
    CORNER_BULGE = 7    # aggressive inward bulge at corners — more oval

    bayer_tiled = np.tile(BAYER_8X8, (TILE_SIZE // 8 + 1, TILE_SIZE // 8 + 1))[:TILE_SIZE, :TILE_SIZE]

    # Per-edge influence with corner-aware curve
    for y in range(TILE_SIZE):
        for x in range(TILE_SIZE):
            d_n = y
            d_s = TILE_SIZE - 1 - y
            d_e = TILE_SIZE - 1 - x
            d_w = x

            # Collect dirt-edge distances (only the "1" edges contribute)
            dirt_dists = []
            if n: dirt_dists.append(("N", d_n))
            if s: dirt_dists.append(("S", d_s))
            if e: dirt_dists.append(("E", d_e))
            if w: dirt_dists.append(("W", d_w))

            if not dirt_dists:
                continue  # pure grass tile, no dirt

            # Effective distance: min(edge distance) BUT corner-aware
            best_effective = min(d for _, d in dirt_dists)

            # Corner-aware blob: only for ADJACENT dirt edges (NE, NW, SE, SW corners)
            # Opposite edges (N-S, E-W) form a band/corridor, NOT a corner — skip them.
            dirt_set = {name for name, _ in dirt_dists}
            adjacent_pairs = [
                ("N", "E"),  # top-right corner
                ("N", "W"),  # top-left corner
                ("S", "E"),  # bottom-right corner
                ("S", "W"),  # bottom-left corner
            ]

            dist_map = {name: d for name, d in dirt_dists}

            for a, b in adjacent_pairs:
                if a in dirt_set and b in dirt_set:
                    da = dist_map[a]
                    db = dist_map[b]
                    if da < SMOOTH_MAX and db < SMOOTH_MAX:
                        # Euclidean inward blob: closer to corner = more dirt
                        # Equivalent to circular arc of radius ~SMOOTH_MAX from corner
                        blob_dist = np.sqrt(da * da + db * db) - CORNER_BULGE
                        # CORNER_BULGE pulls the curve inward → oval bulge instead of L-step
                        best_effective = min(best_effective, blob_dist)

            # Map effective distance to dirt strength (0..1)
            if best_effective < HARD_EDGE:
                # Pure dirt zone
                result[y, x] = dirt_arr[y, x]
            elif best_effective < SMOOTH_MAX:
                # Smooth gradient + Bayer 8×8 dither
                # strength 1.0 at HARD_EDGE, 0.0 at SMOOTH_MAX (linear)
                strength = 1.0 - (best_effective - HARD_EDGE) / (SMOOTH_MAX - HARD_EDGE)
                if strength > bayer_tiled[y, x]:
                    result[y, x] = dirt_arr[y, x]
                # else: stays grass

    return result


def build_canonical_wang_grid(grass, dirt):
    """4x4 grid (128x128) of all 16 Wang ID slots, layout ID = Y*4 + X."""
    canvas = np.zeros((4 * TILE_SIZE, 4 * TILE_SIZE, 3), dtype=np.uint8)
    for wang_id in range(16):
        n = (wang_id >> 3) & 1
        e = (wang_id >> 2) & 1
        s = (wang_id >> 1) & 1
        w = (wang_id >> 0) & 1
        tile = make_wang_slot_tile(grass, dirt, n, e, s, w, seed=wang_id * 7)
        gy = wang_id // 4
        gx = wang_id % 4
        canvas[gy * TILE_SIZE:(gy + 1) * TILE_SIZE,
               gx * TILE_SIZE:(gx + 1) * TILE_SIZE] = tile
    return canvas


def design_garden_map(width=24, height=16):
    """
    Design a logical garden map: center plot of grass, surrounded by dirt path.
    Returns a 2D array of biome IDs (0=grass, 1=dirt).
    Layout (24 wide × 16 tall):
      - Outer ring: dirt path
      - Inner plot: grass (rows 3-12, cols 3-20)
      - A vertical dirt path cuts through center column (separates plots)
      - Top-left has a small path entry into the plot
    """
    grid = np.zeros((height, width), dtype=np.int8)  # 0 = grass default

    # Outer 2-tile dirt border
    grid[0:2, :] = 1
    grid[height - 2:height, :] = 1
    grid[:, 0:2] = 1
    grid[:, width - 2:width] = 1

    # Central vertical path (column 11-12)
    grid[2:height - 2, 11:13] = 1

    # Top entry path opening (slot in north border at column 5-6)
    grid[0:3, 5:7] = 1

    # Right plot lower-left organic dirt patch
    grid[10:13, 15:18] = 1

    # Small island plot in left
    grid[5:8, 7:10] = 1
    # Make island slightly irregular
    grid[5, 7] = 0
    grid[7, 9] = 0

    return grid


def neighbor_wang_id(grid, x, y):
    """For a grass cell, look at N/E/S/W neighbors; if neighbor is dirt (1), edge=1."""
    h, w = grid.shape
    # Only compute Wang ID for "grass" cells; dirt cells are pure dirt (rendered with dirt tile)
    if grid[y, x] == 1:
        return None  # pure dirt

    def is_dirt(yy, xx):
        if yy < 0 or yy >= h or xx < 0 or xx >= w:
            return True  # out-of-bounds treated as dirt (world border)
        return grid[yy, xx] == 1

    n_edge = 1 if is_dirt(y - 1, x) else 0
    e_edge = 1 if is_dirt(y, x + 1) else 0
    s_edge = 1 if is_dirt(y + 1, x) else 0
    w_edge = 1 if is_dirt(y, x - 1) else 0
    return n_edge * 8 + e_edge * 4 + s_edge * 2 + w_edge * 1


def render_garden_map(grid, grass, dirt, wang_slots):
    """Render the garden map to PNG using Wang tiles."""
    h, w = grid.shape
    canvas = np.zeros((h * TILE_SIZE, w * TILE_SIZE, 3), dtype=np.uint8)
    for y in range(h):
        for x in range(w):
            if grid[y, x] == 1:
                # Pure dirt cell; could also use Wang ID 15 for variation
                tile = dirt
            else:
                wid = neighbor_wang_id(grid, x, y)
                if wid is None or wid == 0:
                    tile = grass
                else:
                    tile = wang_slots[wid]
            canvas[y * TILE_SIZE:(y + 1) * TILE_SIZE,
                   x * TILE_SIZE:(x + 1) * TILE_SIZE] = tile
    return canvas


def apply_palette(arr_rgb):
    """Snap to Studio palette using PIL P mode."""
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


def run_neighbor_analyzer_self_test(wang_grid_arr):
    """Slice the 4x4 grid and run NeighborAnalyzer.py on each cell."""
    sys.path.insert(0, os.path.dirname(__file__))
    try:
        from NeighborAnalyzer import analyze_edge
    except ImportError as exc:
        return f"FAIL: NeighborAnalyzer import error — {exc}"

    results = []
    for wang_id in range(16):
        gy = wang_id // 4
        gx = wang_id % 4
        cell = wang_grid_arr[gy * TILE_SIZE:(gy + 1) * TILE_SIZE,
                             gx * TILE_SIZE:(gx + 1) * TILE_SIZE]
        cell_img = Image.fromarray(cell, mode="RGB")
        try:
            detected_id, bitmask = analyze_edge(
                cell_img,
                edge_pixels=EDGE_BAND,
                dominant_color_grass=COLORS["grass_mid"],
                dominant_color_dirt=COLORS["dirt_mid"],
                threshold=0.5
            )
            expected_n = (wang_id >> 3) & 1
            expected_e = (wang_id >> 2) & 1
            expected_s = (wang_id >> 1) & 1
            expected_w = (wang_id >> 0) & 1
            status = "OK" if detected_id == wang_id else "MISMATCH"
            results.append({
                "expected_id": wang_id,
                "expected_mask": (expected_n, expected_e, expected_s, expected_w),
                "detected_id": detected_id,
                "detected_mask": bitmask,
                "status": status,
            })
        except Exception as exc:
            results.append({
                "expected_id": wang_id,
                "error": str(exc),
                "status": "ERROR",
            })
    return results


def main():
    base = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
    out_dir = os.path.join(base, "STAGING", "wang_proof")
    os.makedirs(out_dir, exist_ok=True)
    print(f"[Wang B+ Demo] Output: {out_dir}")

    # Step 1: Generate base tiles
    grass = noise_tile_grass(seed=42)
    dirt = noise_tile_dirt(seed=137)
    grass_pal = apply_palette(grass)
    dirt_pal = apply_palette(dirt)
    save_png(grass_pal, os.path.join(out_dir, "01_grass_32x32.png"), scale=4)
    save_png(dirt_pal, os.path.join(out_dir, "02_dirt_32x32.png"), scale=4)
    print("  [OK]Base tiles: 01_grass, 02_dirt")

    # Step 2: 16 Wang slot canonical grid
    wang_grid = build_canonical_wang_grid(grass_pal, dirt_pal)
    save_png(wang_grid, os.path.join(out_dir, "03_wang_16_canonical_grid.png"), scale=4)
    print("  [OK]Wang 16 canonical grid (4x4 layout)")

    # Step 3: NeighborAnalyzer self-test
    print("\n[NeighborAnalyzer Self-Test]")
    test_results = run_neighbor_analyzer_self_test(wang_grid)
    if isinstance(test_results, str):
        print("  ERROR:", test_results)
    else:
        ok_count = sum(1 for r in test_results if r.get("status") == "OK")
        mismatch_count = sum(1 for r in test_results if r.get("status") == "MISMATCH")
        error_count = sum(1 for r in test_results if r.get("status") == "ERROR")
        print(f"  OK: {ok_count}/16, MISMATCH: {mismatch_count}, ERROR: {error_count}")
        for r in test_results:
            if r["status"] == "OK":
                continue
            if "error" in r:
                print(f"    ID {r['expected_id']}: ERROR — {r['error']}")
            else:
                print(f"    ID {r['expected_id']} expected mask {r['expected_mask']} -> "
                      f"detected ID {r['detected_id']} mask {r['detected_mask']} [{r['status']}]")

    # Step 4: Build slot dictionary for map render
    wang_slots = {}
    for wang_id in range(16):
        gy = wang_id // 4
        gx = wang_id % 4
        wang_slots[wang_id] = wang_grid[gy * TILE_SIZE:(gy + 1) * TILE_SIZE,
                                        gx * TILE_SIZE:(gx + 1) * TILE_SIZE]

    # Step 5: Design + render mantıklı bahçe haritası
    grid = design_garden_map(width=24, height=16)
    print(f"\n[Garden Map Design] {grid.shape[1]}x{grid.shape[0]} tiles")
    print("  Layout (0=grass, 1=dirt):")
    for row in grid:
        print("    " + "".join("." if v == 0 else "#" for v in row))

    map_render = render_garden_map(grid, grass_pal, dirt_pal, wang_slots)
    save_png(map_render, os.path.join(out_dir, "04_garden_map_24x16.png"), scale=2)
    print(f"  [OK]Garden map rendered: 04_garden_map_24x16.png ({map_render.shape[1]}x{map_render.shape[0]} px @ scale 2x)")

    # Step 6: Save a 1x scale version too
    save_png(map_render, os.path.join(out_dir, "04_garden_map_24x16_1x.png"), scale=1)
    print(f"  [OK]Native 1x version: 04_garden_map_24x16_1x.png")

    # Step 7: Wang ID usage stats
    used_ids = {}
    for y in range(grid.shape[0]):
        for x in range(grid.shape[1]):
            if grid[y, x] == 0:
                wid = neighbor_wang_id(grid, x, y)
                if wid is not None:
                    used_ids[wid] = used_ids.get(wid, 0) + 1

    print("\n[Wang ID Kullanım İstatistiği — map içinde hangi slot kaç kez]")
    for wid in sorted(used_ids.keys()):
        n = (wid >> 3) & 1
        e = (wid >> 2) & 1
        s = (wid >> 1) & 1
        w = (wid >> 0) & 1
        print(f"  ID {wid:2d} (N={n} E={e} S={s} W={w}): {used_ids[wid]} kez")

    print(f"\n[Done] Tüm çıktılar: {out_dir}")


if __name__ == "__main__":
    main()
