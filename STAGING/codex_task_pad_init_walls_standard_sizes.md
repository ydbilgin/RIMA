# Codex Task — Pad Cropped Walls to Standard Sizes for Init Image

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev

Mevcut cropped init PNG'leri (RIMA style baked-in) **standard power-of-2 sizes**'a **transparent padding** ile genişlet. Pixel-perfect, scale/resize YASAK — sadece transparent border ekle.

Create Image S-XL (new) tool init image dimensions = output dimensions kilitliyor. Standard size init = standard size output.

## Input → Output

| Input | Current size | **Target standard** | Action |
|---|---|---|---|
| `solo/act1_wall_tall_straight_init.png` | 118×212 | **128×256** | Pad: 5px H each side, 22px V each side |
| `solo/act1_wall_tall_corner_init.png` | 195×217 | **256×256** | Pad: 30px H each side, 19px V each side |
| `solo/act1_wall_archway_init.png` | 179×243 | **256×256** | Pad: 38px H each side, 6px V each side |
| `solo/act1_wall_endcap_column_init.png` | 76×203 | **128×256** | Pad: 26px H each side, 26px V each side |

(Endcap 64'e sığmadığı için 128'e gidiyor. Iso column'da transparent margin OK.)

## Output Dosyalar

Yeni PNG'ler `_init_padded.png` suffix ile:
- `STAGING/_pixellab_inputs/solo/act1_wall_tall_straight_init_padded.png` (128×256)
- `STAGING/_pixellab_inputs/solo/act1_wall_tall_corner_init_padded.png` (256×256)
- `STAGING/_pixellab_inputs/solo/act1_wall_archway_init_padded.png` (256×256)
- `STAGING/_pixellab_inputs/solo/act1_wall_endcap_column_init_padded.png` (128×256)

## Padding Behavior

- Mevcut wall sprite **MERKEZLENMİŞ** new canvas içinde
- Çevresi tamamen **transparent (alpha=0)**
- Wall pixel'leri DEĞİŞMEZ — sadece yeni canvas içinde merkeze yerleştir
- **ASLA scale/resize/upscale yapma** — pixel-perfect korunmalı

## Python Skeleton

```python
from PIL import Image
import os

src_dir = "STAGING/_pixellab_inputs/solo"
operations = [
    ("act1_wall_tall_straight_init.png",   "act1_wall_tall_straight_init_padded.png",   (128, 256)),
    ("act1_wall_tall_corner_init.png",     "act1_wall_tall_corner_init_padded.png",     (256, 256)),
    ("act1_wall_archway_init.png",         "act1_wall_archway_init_padded.png",         (256, 256)),
    ("act1_wall_endcap_column_init.png",   "act1_wall_endcap_column_init_padded.png",   (128, 256)),
]

for src_name, dst_name, target_size in operations:
    src_path = os.path.join(src_dir, src_name)
    src = Image.open(src_path).convert("RGBA")
    sw, sh = src.size
    tw, th = target_size
    # Center the source in a new transparent canvas
    canvas = Image.new("RGBA", target_size, (0, 0, 0, 0))
    offset_x = (tw - sw) // 2
    offset_y = (th - sh) // 2
    canvas.paste(src, (offset_x, offset_y), src)  # use src as mask for alpha
    dst_path = os.path.join(src_dir, dst_name)
    canvas.save(dst_path)
    print(f"{dst_name} -> {canvas.size}, source centered at ({offset_x},{offset_y})")
```

## Verification

Her PNG için:
- ✅ Canvas tam target standard size
- ✅ Source wall pixel'leri unchanged (no resize)
- ✅ Wall merkezlenmiş (offset = (target - source) / 2)
- ✅ Transparent background outside the wall

## Effort

low — basit PIL paste, math hazır.

## Output Confirmation

4 padded PNG path + dimensions.
