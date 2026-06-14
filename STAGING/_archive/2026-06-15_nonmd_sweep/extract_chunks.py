from PIL import Image
import pathlib
import os

SRC = r"C:\Users\ydbil\Downloads\pixellab--Dark-fantasy-dungeon-extracti-1779488816366.png"
OUT = pathlib.Path("STAGING/concepts/master_room_pilot/extracted_chunks")
OUT.mkdir(parents=True, exist_ok=True)

CHUNKS = [
    ("wall_L1_archway.png", (15, 60, 220, 280)),
    ("wall_L2_banner.png", (170, 75, 280, 200)),
    ("wall_L3_pillar_torch.png", (15, 130, 65, 280)),
    ("wall_R1_alcove.png", (260, 95, 380, 250)),
    ("wall_R2_cracked.png", (370, 75, 485, 235)),
    ("wall_R3_end_pillar.png", (445, 100, 510, 245)),
    ("corner_C1_top_v.png", (230, 20, 295, 105)),
    ("edge_E1_pillar_left.png", (130, 295, 195, 460)),
    ("edge_E2_pillar_right.png", (305, 305, 375, 470)),
    ("edge_E3_rubble_right.png", (425, 290, 510, 420)),
    ("edge_E4_front_broken.png", (185, 405, 380, 510)),
    ("prop_P1_brazier.png", (215, 240, 295, 360)),
    ("prop_P2_candles.png", (255, 285, 310, 345)),
    ("floor_F1_granite_sample.png", (340, 280, 372, 312)),
]

img = Image.open(SRC).convert("RGBA")
for name, bbox in CHUNKS:
    chunk = img.crop(bbox)
    pixels = chunk.load()
    w, h = chunk.size
    for y in range(h):
        for x in range(w):
            r, g, b, a = pixels[x, y]
            if r + g + b < 30:
                pixels[x, y] = (0, 0, 0, 0)
    chunk.save(OUT / name)
    print(f"Saved: {name} ({chunk.size[0]}x{chunk.size[1]})")

print(f"\nDone. {len(CHUNKS)} chunks extracted to {OUT}")
