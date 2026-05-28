"""
Slice 4 PixelLab sheets into per-piece PNGs with UNIFORM CANVAS.

Each piece is saved on a fixed-size transparent canvas so that:
- Pivot stays consistent (center / bottom-center) across all pieces
- WallChainBuilder snap (`x step 2`) works with predictable world width
- Unity import settings can apply uniformly

Canvas sizes:
  Sheet 1: 128 x 256 (4x2 grid)
  Sheet 2: 256 x 192 (asymmetric -> max blob bbox padded)
  Sheet 3: 128 x 128 (4x4 grid)
  Sheet 4: 128 x 128 (asymmetric -> max blob bbox padded)

Output: STAGING/concepts/asset_pack_v3/sliced/sheet_<N>/<piece>.png
Old tight versions archived to: sliced/_archive_tight/sheet_<N>/
"""
from PIL import Image
import os, json, shutil
import numpy as np

ROOT = "STAGING/concepts/asset_pack_v3"
OUT = f"{ROOT}/sliced"
ARCHIVE = f"{OUT}/_archive_tight"

SHEET_1_NAMES = [
    "01_straight", "02_outer_corner", "03_inner_corner", "04_end",
    "05_door_l", "06_door_r", "07_alcove", "08_protrusion",
]
SHEET_3_NAMES = [
    "01_arch_portal_frame", "02_outer_corner_l", "03_outer_corner_r", "04_pillar_join",
    "05_niche_l", "06_niche_r", "07_wall_step", "08_edge_cap",
    "09_tower_door_l", "10_tower_door_r", "11_wide_doorway_top", "12_pillar_terminator",
    "13_grand_arch_doorway", "14_window_large", "15_window_small", "16_tall_pillar",
]

SHEET_1_CANVAS = (128, 256)
SHEET_3_CANVAS = (128, 128)
SHEET_2_CANVAS = (256, 192)
SHEET_4_CANVAS = (128, 128)


def archive_existing():
    for n in [1, 2, 3, 4]:
        src = f"{OUT}/sheet_{n}"
        if not os.path.isdir(src):
            continue
        dst = f"{ARCHIVE}/sheet_{n}"
        os.makedirs(ARCHIVE, exist_ok=True)
        if os.path.isdir(dst):
            shutil.rmtree(dst)
        shutil.move(src, dst)
        print(f"  Archived: {src} -> {dst}")


def center_on_canvas(cell, canvas_size):
    """Crop cell to bbox, then paste centered on a fresh transparent canvas."""
    bbox = cell.getbbox()
    if not bbox:
        return Image.new("RGBA", canvas_size, (0, 0, 0, 0))
    tight = cell.crop(bbox)
    cw, ch = canvas_size
    tw, th = tight.size
    if tw > cw or th > ch:
        print(f"    WARN: piece {tw}x{th} > canvas {cw}x{ch}, resizing canvas not piece")
        cw = max(cw, tw)
        ch = max(ch, th)
    canvas = Image.new("RGBA", (cw, ch), (0, 0, 0, 0))
    x = (cw - tw) // 2
    y = (ch - th) // 2  # center vertical; for bottom-center use: ch - th
    canvas.paste(tight, (x, y), tight)
    return canvas


def slice_grid_uniform(sheet_n, cols, rows, names, canvas_size):
    src = f"{ROOT}/sheet_{sheet_n}_cleaned.png"
    img = Image.open(src).convert("RGBA")
    W, H = img.size
    cw, ch = W // cols, H // rows
    out_dir = f"{OUT}/sheet_{sheet_n}"
    os.makedirs(out_dir, exist_ok=True)
    manifest = []
    for idx, name in enumerate(names):
        row, col = idx // cols, idx % cols
        cell = img.crop((col * cw, row * ch, (col + 1) * cw, (row + 1) * ch))
        out = center_on_canvas(cell, canvas_size)
        out.save(f"{out_dir}/{name}.png")
        bbox = cell.getbbox()
        tight_size = (bbox[2] - bbox[0], bbox[3] - bbox[1]) if bbox else (0, 0)
        manifest.append({
            "name": name, "row": row, "col": col,
            "canvas": list(canvas_size), "tight": list(tight_size),
        })
        print(f"  sheet_{sheet_n} [{row},{col}] {name}: canvas={canvas_size} tight={tight_size}")
    return manifest


def label_components(mask):
    H, W = mask.shape
    labels = np.zeros((H, W), dtype=np.int32)
    cur = 0
    for y in range(H):
        for x in range(W):
            if mask[y, x] and labels[y, x] == 0:
                cur += 1
                stack = [(y, x)]
                while stack:
                    cy, cx = stack.pop()
                    if 0 <= cy < H and 0 <= cx < W and mask[cy, cx] and labels[cy, cx] == 0:
                        labels[cy, cx] = cur
                        stack.append((cy + 1, cx))
                        stack.append((cy - 1, cx))
                        stack.append((cy, cx + 1))
                        stack.append((cy, cx - 1))
    return labels, cur


def slice_blob_uniform(sheet_n, canvas_size, min_pixels=200, row_quantize=40):
    src = f"{ROOT}/sheet_{sheet_n}_cleaned.png"
    img = Image.open(src).convert("RGBA")
    arr = np.array(img)
    alpha = arr[:, :, 3] > 0
    labels, num = label_components(alpha)
    out_dir = f"{OUT}/sheet_{sheet_n}"
    os.makedirs(out_dir, exist_ok=True)
    bboxes = []
    for i in range(1, num + 1):
        ys, xs = np.where(labels == i)
        if len(ys) < min_pixels:
            continue
        bboxes.append({
            "label": i, "y0": int(ys.min()), "y1": int(ys.max()),
            "x0": int(xs.min()), "x1": int(xs.max()), "pixels": int(len(ys)),
        })
    bboxes.sort(key=lambda b: (b["y0"] // row_quantize, b["x0"]))
    manifest = []
    for idx, b in enumerate(bboxes, 1):
        tight = img.crop((b["x0"], b["y0"], b["x1"] + 1, b["y1"] + 1))
        cw, ch = canvas_size
        tw, th = tight.size
        if tw > cw or th > ch:
            print(f"    WARN piece_{idx:02d} {tw}x{th} > canvas {cw}x{ch}, expanding")
            cw, ch = max(cw, tw), max(ch, th)
        canvas = Image.new("RGBA", (cw, ch), (0, 0, 0, 0))
        x = (cw - tw) // 2
        y = (ch - th) // 2
        canvas.paste(tight, (x, y), tight)
        canvas.save(f"{out_dir}/piece_{idx:02d}.png")
        manifest.append({
            "id": idx, "src_pos": [b["x0"], b["y0"]],
            "canvas": [cw, ch], "tight": [tw, th], "pixels": b["pixels"],
        })
        print(f"  sheet_{sheet_n} piece_{idx:02d}: canvas={canvas.size} tight={(tw, th)}")
    return manifest


if __name__ == "__main__":
    print("[Archive] moving tight versions to _archive_tight/")
    archive_existing()
    manifest = {}
    print(f"\n[Sheet 1] uniform canvas {SHEET_1_CANVAS}")
    manifest["sheet_1"] = slice_grid_uniform(1, 4, 2, SHEET_1_NAMES, SHEET_1_CANVAS)
    print(f"\n[Sheet 2] uniform canvas {SHEET_2_CANVAS}")
    manifest["sheet_2"] = slice_blob_uniform(2, SHEET_2_CANVAS)
    print(f"\n[Sheet 3] uniform canvas {SHEET_3_CANVAS}")
    manifest["sheet_3"] = slice_grid_uniform(3, 4, 4, SHEET_3_NAMES, SHEET_3_CANVAS)
    print(f"\n[Sheet 4] uniform canvas {SHEET_4_CANVAS}")
    manifest["sheet_4"] = slice_blob_uniform(4, SHEET_4_CANVAS)

    with open(f"{OUT}/_manifest_v3.json", "w") as f:
        json.dump(manifest, f, indent=2)
    print(f"\n[DONE] manifest -> {OUT}/_manifest_v3.json")
