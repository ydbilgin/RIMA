ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

# Codex Task — Iso Duvar Sistemi Final (15 asset, $imagegen built-in)

**Amaç:** RIMA Fractured Chamber için MODÜLER iso duvar sistemi — 15 asset. Torch SEPARATE (user PixelLab'dan üretecek), sen sadece **duvar + floor + pillar + corner + landmark** üret.

## KRİTİK — `$imagegen` BUILT-IN TOOL MODE

**Use the `$imagegen` skill in DEFAULT BUILT-IN TOOL MODE only.**
- Built-in `image_gen` tool — **does NOT require OPENAI_API_KEY**
- DO NOT use CLI fallback (`scripts/image_gen.py`)
- DO NOT export OPENAI_API_KEY
- Skill location: `~/.codex/skills/.system/imagegen/`

## Pipeline Bağlamı

- TRUE 45° iso diamond perspective (NOT top-down rectangular)
- chatgpt_ref dramatic high wall hedefi (char:wall ratio 1:4 ile 1:5)
- Sadece NW + NE walls visible (south + east open)
- NO banner, NO torch baked (user torch'u ayrıca yerleştirecek)
- Doorway içi boş (geometric arch + black void)
- Pillar seam-cover strategy LIVE (S101 PILLAR-LESS REVOKED, memory: project_pillar_seam_cover_lock_2026_05_24.md)

## Mevcut Raw Assets (1024×1024)

`STAGING/concepts/fractured_chamber/iso_assets/raw/`:

**Kullanılacak (14 raw):**
- iso_floor_clean_raw.png
- iso_floor_cracked_raw.png
- iso_floor_rift_glow_raw.png
- iso_floor_broken_raw.png
- iso_floor_edge_light_raw.png
- iso_floor_debris_raw.png
- wall_nw_mid_plain_raw.png
- wall_nw_mid_variant_raw.png
- wall_nw_doorway_raw.png
- wall_ne_mid_plain_raw.png
- wall_ne_mid_variant_raw.png
- wall_ne_doorway_raw.png
- wall_n_corner_raw.png
- wall_n_landmark_raw.png

**ARCHIVE (KULLANMAYACAĞIZ — torch alcove'lar, user torch'u ayrı yapacak):**
- wall_nw_torch_raw.png → move to `STAGING/concepts/fractured_chamber/iso_assets/raw/_archive/`
- wall_ne_torch_raw.png → move to `_archive/`

**Eksik (üretilecek):**
- wall_pillar_universal_raw.png (1 piece)

## Görev 1 — Pillar Raw Üret ($imagegen built-in)

**Asset:** Pillar-Universal — iso diagonal seam cover stone column
**Output raw:** `STAGING/concepts/fractured_chamber/iso_assets/raw/wall_pillar_universal_raw.png` (1024×1024)
**Final target:** 64×256

**Prompt:**
```
Single narrow standalone iso stone pillar concept art for RIMA Shattered Keep dungeon.
TRUE 45 degree isometric perspective like Diablo II / Children of Morta / Hades, NOT top-down, NOT rectangular front-facing.
TALL VERTICAL COLUMN — full height matching dramatic wall mid pieces (about 4 character heights tall, 1:4 ratio dramatic dungeon scale).
Iso 3/4 perspective showing front face + slight side face (3D thickness illusion in iso).
Square cross-section, decorative carved capital at top.
Material: charcoal fractured granite stone, slightly darker shade than wall mid pieces, weathered surface, thin cyan rift hairline crack on one side.
Used as seam cover between adjacent iso wall mid pieces.
Transparent background with chroma-key removable bg.
Dark fantasy pixel art aesthetic, hard pixel edges, limited palette: dark grey (#3A3040), mid (#6B5C6E), cyan (#00FFCC).
NO characters, NO UI, NO text, NO labels, NO banner, NO floor visible, NO torch.
1024x1024 canvas, asset centered with transparent padding for crop flexibility.
```

**Reference images** (built-in tool support varsa attach):
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_46 (1).png`
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_48 (4).png`
- `STAGING/concepts/fractured_chamber/iso_assets/raw/wall_nw_mid_plain_raw.png` (matching style)

## Görev 2 — Archive torch alcove asset'leri

```bash
mkdir -p STAGING/concepts/fractured_chamber/iso_assets/raw/_archive/
mv STAGING/concepts/fractured_chamber/iso_assets/raw/wall_nw_torch_raw.png STAGING/concepts/fractured_chamber/iso_assets/raw/_archive/
mv STAGING/concepts/fractured_chamber/iso_assets/raw/wall_ne_torch_raw.png STAGING/concepts/fractured_chamber/iso_assets/raw/_archive/
```

## Görev 3 — 15 Raw'ı Final Boyuta Downscale (PIL)

| # | Raw | Final Path | Target Boyut |
|---|---|---|---|
| 1 | iso_floor_clean_raw.png | iso_assets/iso_floor_clean.png | 128×64 |
| 2 | iso_floor_cracked_raw.png | iso_assets/iso_floor_cracked.png | 128×64 |
| 3 | iso_floor_rift_glow_raw.png | iso_assets/iso_floor_rift_glow.png | 128×64 |
| 4 | iso_floor_broken_raw.png | iso_assets/iso_floor_broken.png | 128×64 |
| 5 | iso_floor_edge_light_raw.png | iso_assets/iso_floor_edge_light.png | 128×64 |
| 6 | iso_floor_debris_raw.png | iso_assets/iso_floor_debris.png | 128×64 |
| 7 | wall_nw_mid_plain_raw.png | iso_assets/wall_nw_mid_plain.png | **128×256** |
| 8 | wall_nw_mid_variant_raw.png | iso_assets/wall_nw_mid_variant.png | 128×256 |
| 9 | wall_nw_doorway_raw.png | iso_assets/wall_nw_doorway.png | 128×256 |
| 10 | wall_ne_mid_plain_raw.png | iso_assets/wall_ne_mid_plain.png | 128×256 |
| 11 | wall_ne_mid_variant_raw.png | iso_assets/wall_ne_mid_variant.png | 128×256 |
| 12 | wall_ne_doorway_raw.png | iso_assets/wall_ne_doorway.png | 128×256 |
| 13 | wall_n_corner_raw.png | iso_assets/wall_n_corner.png | 128×256 |
| 14 | wall_n_landmark_raw.png | iso_assets/wall_n_landmark.png | **128×320** |
| 15 | wall_pillar_universal_raw.png | iso_assets/wall_pillar_universal.png | 64×256 |

**Downscale procedure (PIL):**
```python
from PIL import Image
img = Image.open(raw_path).convert('RGBA')
img_resized = img.resize((target_w, target_h), Image.NEAREST)
# Background cleanup: green chroma key #00FF00 ± 30 → alpha 0
pixels = img_resized.load()
for x in range(img_resized.width):
    for y in range(img_resized.height):
        r, g, b, a = pixels[x, y]
        if g > 200 and r < 80 and b < 80:
            pixels[x, y] = (0, 0, 0, 0)
img_resized.save(final_path, 'PNG')
```

**Mevcut iso_assets/ klasörü temizle** (eski 128×96 spec downscale'leri varsa overwrite).

## Görev 4 — QC Raporu

`STAGING/iso_walls_final_qc.md`:

```markdown
# Iso Walls Final QC (15 asset)

## $imagegen Tool Mode
- Mode: BUILT-IN / FAIL
- Pillar generation: SUCCESS / BLOCKED

## Archived Assets
- wall_nw_torch_raw.png → _archive/
- wall_ne_torch_raw.png → _archive/

## Downscale Results (15 piece)
| # | Final Path | Boyut | Status |
| 1 | iso_floor_clean.png | 128×64 | PASS |
| ... | ... | ... | ... |

## Visual QC
- [ ] True iso diamond perspective
- [ ] Transparent BG clean (green chroma removed)
- [ ] Wall pieces baseline aligned
- [ ] All 9 wall pieces height 256
- [ ] Landmark dramatic 320
- [ ] Pillar 256 (wall ile aynı)
- [ ] No torch baked (sadece düz duvar)

## Verdict
PASS / PARTIAL / BLOCKED

## Next Step
- Sub-agent UnityMCP import + iso grid setup
```

## Git Commit

```bash
git add STAGING/concepts/fractured_chamber/iso_assets STAGING/iso_walls_final_qc.md
git commit -m "[Codex] [ISO WALLS] 15 asset final (wall 128x256, landmark 128x320, torch removed)"
```

## Hard Constraints

- **$imagegen BUILT-IN TOOL mode SADECE** (CLI fallback ASLA)
- NEAREST resampling (no bilinear)
- Background green chroma → alpha 0
- 15 final asset standart yerinde
- Torch alcove asset'leri ARCHIVE (kullanmayacağız, user PixelLab torch ekleyecek)
- Eğer built-in tool pillar gen fail → BLOCKED, no fallback
