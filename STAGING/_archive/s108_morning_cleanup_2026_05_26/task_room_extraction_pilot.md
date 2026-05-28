# Task: PixelLab Master Room — Chunk Extraction Pilot

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
PixelLab'da üretilen 512x512 master extraction room'dan 14 chunk'ı Python PIL ile crop et + transparent BG'li PNG'ler olarak kaydet. Sonra Unity'ye import edilecek.

## Kaynak
**Input image:** `C:\Users\ydbil\Downloads\pixellab--Dark-fantasy-dungeon-extracti-1779488816366.png`
- 512x512 pixel art
- Pure pixel art (not painterly)
- Iso V-shape oda, granite + cyan rift + torch palette

**Output klasör:** `STAGING/concepts/master_room_pilot/extracted_chunks/`

## Extraction Targets (koordinatlar approximate — fine-tune gerekebilir)

Image koordinat sistemi: (0,0) = sol üst, x ekseni sağa, y ekseni aşağı.

### Set 1 — LEFT WALL (NW diagonal)
| ID | Filename | Bbox (x1, y1, x2, y2) | Notes |
|---|---|---|---|
| L1 | `wall_L1_archway.png` | (15, 60, 220, 280) | Archway opening section, torch + flame içinde |
| L2 | `wall_L2_banner.png` | (170, 75, 280, 200) | Banner hanging wall (üst kısım) |
| L3 | `wall_L3_pillar_torch.png` | (15, 130, 65, 280) | Sol-üst kenar pillar with torch (izole) |

### Set 2 — RIGHT WALL (NE diagonal)
| ID | Filename | Bbox | Notes |
|---|---|---|---|
| R1 | `wall_R1_alcove.png` | (260, 95, 380, 250) | Alcove with skull figure niche |
| R2 | `wall_R2_cracked.png` | (370, 75, 485, 235) | Cracked rift section |
| R3 | `wall_R3_end_pillar.png` | (445, 100, 510, 245) | Right wall end pillar |

### Set 3 — TOP CORNER
| ID | Filename | Bbox | Notes |
|---|---|---|---|
| C1 | `corner_C1_top_v.png` | (230, 20, 295, 105) | V-junction tall pillar (iki duvar buluşması) |

### Set 4 — CUTAWAY EDGES
| ID | Filename | Bbox | Notes |
|---|---|---|---|
| E1 | `edge_E1_pillar_left.png` | (130, 295, 195, 460) | Sol-ön pillar stub |
| E2 | `edge_E2_pillar_right.png` | (305, 305, 375, 470) | Sağ-ön pillar stub |
| E3 | `edge_E3_rubble_right.png` | (425, 290, 510, 420) | Sağ-alt rubble pile |
| E4 | `edge_E4_front_broken.png` | (185, 405, 380, 510) | Ön-alt low broken wall (E1-E2 arası) |

### Set 5 — PROPS
| ID | Filename | Bbox | Notes |
|---|---|---|---|
| P1 | `prop_P1_brazier.png` | (215, 240, 295, 360) | Brazier + flame |
| P2 | `prop_P2_candles.png` | (255, 285, 310, 345) | Candle cluster |

### Set 6 — FLOOR (sample tile)
| ID | Filename | Bbox | Notes |
|---|---|---|---|
| F1 | `floor_F1_granite_sample.png` | (340, 280, 372, 312) | 32x32 clean granite tile patch |

**Toplam: 14 chunk**

## Python Script

`STAGING/extract_chunks.py` olarak yaz, sonra çalıştır:

```python
from PIL import Image
import pathlib
import os

SRC = r"C:\Users\ydbil\Downloads\pixellab--Dark-fantasy-dungeon-extracti-1779488816366.png"
OUT = pathlib.Path("STAGING/concepts/master_room_pilot/extracted_chunks")
OUT.mkdir(parents=True, exist_ok=True)

CHUNKS = [
    ("wall_L1_archway.png",       (15, 60, 220, 280)),
    ("wall_L2_banner.png",        (170, 75, 280, 200)),
    ("wall_L3_pillar_torch.png",  (15, 130, 65, 280)),
    ("wall_R1_alcove.png",        (260, 95, 380, 250)),
    ("wall_R2_cracked.png",       (370, 75, 485, 235)),
    ("wall_R3_end_pillar.png",    (445, 100, 510, 245)),
    ("corner_C1_top_v.png",       (230, 20, 295, 105)),
    ("edge_E1_pillar_left.png",   (130, 295, 195, 460)),
    ("edge_E2_pillar_right.png",  (305, 305, 375, 470)),
    ("edge_E3_rubble_right.png",  (425, 290, 510, 420)),
    ("edge_E4_front_broken.png",  (185, 405, 380, 510)),
    ("prop_P1_brazier.png",       (215, 240, 295, 360)),
    ("prop_P2_candles.png",       (255, 285, 310, 345)),
    ("floor_F1_granite_sample.png", (340, 280, 372, 312)),
]

img = Image.open(SRC).convert("RGBA")
for name, bbox in CHUNKS:
    chunk = img.crop(bbox)
    # Background removal: black pixels (R+G+B < 30) become transparent
    pixels = chunk.load()
    w, h = chunk.size
    for y in range(h):
        for x in range(w):
            r, g, b, a = pixels[x, y]
            if r + g + b < 30:  # near-black
                pixels[x, y] = (0, 0, 0, 0)
    chunk.save(OUT / name)
    print(f"Saved: {name} ({chunk.size[0]}x{chunk.size[1]})")

print(f"\nDone. {len(CHUNKS)} chunks extracted to {OUT}")
```

## Acceptance Test (CODEX_DONE.md)

1. **File count:** 14 PNG'nin tümü `STAGING/concepts/master_room_pilot/extracted_chunks/` altında mı?
2. **Boyut sanity:** Her PNG'nin boyutları bbox ile tutarlı mı?
3. **Transparent BG:** PNG'lerin RGBA modunda olduğunu doğrula (mode == "RGBA")
4. **Black-to-alpha:** Near-black piksellerin alpha=0 olduğunu spot check et (en az 1 chunk için)
5. **Failures:** Source path bulunamadı / PIL hatası / file system hatası varsa not düş

## Output → CODEX_DONE.md
- 14 PNG path listesi (boyutlarıyla)
- Acceptance test 5 madde
- Issue varsa not (özellikle coordinate fine-tuning gereken chunk varsa)
