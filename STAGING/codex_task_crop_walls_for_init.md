# Codex Task — Crop 4 Walls from Cleaned Sheet for Init Image

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev

Mevcut cleaned sheet'ten (`STAGING/_pixellab_outputs/walls/act1_walls_sprint1_sheet_v1_clean.png`, 512×512) 4 ayrı wall'ı crop et. Bu cropped PNG'ler Create Image S-XL (new) tool'una **init image** olarak yüklenecek (mevcut RIMA stilini yeni boyutta gen için style anchor).

## Input

`STAGING/_pixellab_outputs/walls/act1_walls_sprint1_sheet_v1_clean.png` (512×512)

Bilinen alpha bounds (önceki analiz):
| Wall | Sheet abs bbox | Actual size |
|---|---|---|
| TL straight | (69,23)-(186,234) | 118×212 |
| TR corner | (286,29)-(480,245) | 195×217 |
| BL archway | (33,264)-(211,506) | 179×243 |
| BR endcap | (346,281)-(421,483) | 76×203 |

## Output Dosyalar

| Dosya | Boyut | İçerik |
|---|---|---|
| `STAGING/_pixellab_inputs/solo/act1_wall_tall_straight_init.png` | **118×212** | Cleaned straight wall (RIMA style baked) |
| `STAGING/_pixellab_inputs/solo/act1_wall_tall_corner_init.png` | **195×217** | Cleaned corner wall |
| `STAGING/_pixellab_inputs/solo/act1_wall_archway_init.png` | **179×243** | Cleaned archway |
| `STAGING/_pixellab_inputs/solo/act1_wall_endcap_column_init.png` | **76×203** | Cleaned endcap column |

## Python Skeleton

```python
from PIL import Image
import os

src = Image.open("STAGING/_pixellab_outputs/walls/act1_walls_sprint1_sheet_v1_clean.png").convert("RGBA")

# (left, top, right_exclusive, bottom_exclusive) for PIL.crop
crops = [
    ("act1_wall_tall_straight_init.png", (69, 23, 187, 235)),  # +1 to include bbox max
    ("act1_wall_tall_corner_init.png",   (286, 29, 481, 246)),
    ("act1_wall_archway_init.png",       (33, 264, 212, 507)),
    ("act1_wall_endcap_column_init.png", (346, 281, 422, 484)),
]

os.makedirs("STAGING/_pixellab_inputs/solo", exist_ok=True)
for name, box in crops:
    cropped = src.crop(box)
    out_path = f"STAGING/_pixellab_inputs/solo/{name}"
    cropped.save(out_path)
    print(f"Saved {out_path} -> {cropped.size}")
```

## Verification

Her PNG için:
- ✅ Tight crop (alpha bounds exact)
- ✅ Transparent background outside wall
- ✅ Full wall visible (granite blocks + mortar + cyan rift accents preserved)
- ✅ Size matches actual_size table

## Effort

low — basit PIL crop, math hazır.

## Output Confirmation

4 path + dimensions listele.
