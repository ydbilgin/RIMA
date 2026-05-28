ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

# Codex Task — Walls HIGH TOP-DOWN 3/4 Retry (NOT true iso, Hades/CoM tarzı)

**Amaç:** Önceki "TRUE 45 degree iso diamond" wall spec'i yanlıştı (terim hatası, memory: project_high_top_down_3_4_lock_2026_05_24). Doğru spec: HIGH TOP-DOWN 3/4 (Hades, Children of Morta, Diablo III tarzı). Mevcut 14 raw zaten high top-down 3/4 uyumlu (front-facing brick + 3/4 thickness illusion) — sadece downscale yeni boyutla + pillar gen + final asset set.

## KRİTİK — Spec Lock Updates

1. **HIGH TOP-DOWN 3/4 projection LOCK** (memory: project_high_top_down_3_4_lock_2026_05_24)
   - Camera ~70-80 degree from horizon + 3/4 sprite styling
   - NOT true 45 degree iso diamond
   - Reference: Hades, Children of Morta, Diablo III
   - Anti-reference: Diablo II (true iso)

2. **$imagegen BUILT-IN TOOL MODE** (memory: feedback_codex_imagegen_skill_not_env_var)
   - Use built-in image_gen tool, OPENAI_API_KEY NOT needed
   - DO NOT use CLI fallback

3. **Pillar seam-cover LIVE** (memory: project_pillar_seam_cover_lock_2026_05_24)
   - S101 PILLAR-LESS REVOKED

## Asset Spec

### Yeni Boyutlar (Hades 1:6 char ratio, MAX dramatic)

| Asset | Boyut | Notlar |
|---|---|---|
| Floor (6 tile) | 128×64 | Hafif perspective (NOT true diamond, slight 3/4 illusion OK) |
| **Wall mid** (NW + NE families) | **128×384** | Front-facing rectangular band, 3/4 thickness top cap + side face hint |
| Wall doorway | 128×384 | Same |
| Wall broken (kırık) variant | 128×384 | Damaged section |
| N-Corner | 128×384 | Same family height |
| **N-Landmark** | **256×384** | 2 tile wide setpiece, dramatic feature |
| Pillar | 64×384 | Seam cover, same wall height |

**Char (120):wall (384) ratio = 1:3.2** — chatgpt_ref dramatic range içinde

### Asset Listesi (15 toplam)

1-6: Floor (mevcut raw'lar)
7. NW-Mid-Plain (mevcut raw — high top-down 3/4 uyumlu)
8. NW-Mid-Variant
9. NW-Mid-Broken (kırık variant, raw mevcut değil — YENİ üretilecek)
10. NW-Doorway
11. NE-Mid-Plain
12. NE-Mid-Variant
13. NE-Mid-Broken (YENİ)
14. NE-Doorway
15. N-Corner-Vertex
16. N-Landmark (256×384, setpiece)
17. Pillar-Universal (YENİ üretilecek)

NW-Torch + NE-Torch ALCOVE assets ARCHIVE (kullanılmayacak, user torch'u ayrı yapacak).

## Görevler

### Görev 1: Mevcut raw'ları kontrol + archive

Mevcut raw'lar `STAGING/concepts/fractured_chamber/iso_assets/raw/`:
- iso_floor_*_raw.png (6 floor — KULLAN)
- wall_nw_mid_plain_raw.png — KULLAN
- wall_nw_mid_variant_raw.png — KULLAN
- wall_nw_doorway_raw.png — KULLAN
- wall_nw_torch_raw.png — ARCHIVE (_archive_v1/)
- wall_ne_mid_plain_raw.png — KULLAN
- wall_ne_mid_variant_raw.png — KULLAN
- wall_ne_doorway_raw.png — KULLAN
- wall_ne_torch_raw.png — ARCHIVE
- wall_n_corner_raw.png — KULLAN
- wall_n_landmark_raw.png — KULLAN

Archive komutu:
```bash
mkdir -p STAGING/concepts/fractured_chamber/iso_assets/raw/_archive_v1/
mv STAGING/concepts/fractured_chamber/iso_assets/raw/wall_nw_torch_raw.png STAGING/concepts/fractured_chamber/iso_assets/raw/_archive_v1/
mv STAGING/concepts/fractured_chamber/iso_assets/raw/wall_ne_torch_raw.png STAGING/concepts/fractured_chamber/iso_assets/raw/_archive_v1/
```

### Görev 2: 3 yeni raw üret ($imagegen built-in)

**NW-Mid-Broken (1024×1024 raw):**
```
Single tall rectangular stone wall band, HIGH TOP-DOWN 3/4 perspective for RIMA Shattered Keep dungeon (Hades / Children of Morta style, camera ~70-80 degree from horizon with subtle 3/4 sprite tilt, NOT true 45 degree isometric diamond, NOT top-down 90 degree).
Heavily DAMAGED variant of NW wall — partially collapsed at upper section, missing stones expose void behind, faint cyan rift glow leaking through cracks, low rubble at base.
Charcoal fractured granite stone palette (#3A3040 / #6B5C6E), thin cyan rift cracks (#00FFCC).
Front-facing wall band with subtle top cap + slight visible side face (3/4 thickness illusion in iso-ish view).
Baseline at bottom, dramatic tall vertical wall.
Transparent green chroma key background.
No banner, no torch baked, no characters, no UI, no text.
```

**NE-Mid-Broken (1024×1024):** Aynı, NE-specific lighting.

**Pillar-Universal (1024×1024):**
```
Single narrow standalone stone pillar, HIGH TOP-DOWN 3/4 perspective (Hades / CoM style, NOT true iso).
Tall vertical column, full height matching wall mid pieces (~6x character height), iso-ish 3/4 view showing front + slight side face.
Square cross-section, decorative carved capital at top, slightly darker than wall pieces, weathered surface, thin cyan rift hairline crack on one side.
Used as seam cover between adjacent wall mid pieces.
Transparent green chroma key background.
Dark fantasy pixel art, no banner, no torch, no characters.
```

### Görev 3: 17 raw → final downscale (PIL NEAREST)

| # | Raw | Final | Target |
|---|---|---|---|
| 1 | iso_floor_clean_raw | iso_floor_clean.png | 128×64 |
| 2 | iso_floor_cracked_raw | iso_floor_cracked.png | 128×64 |
| 3 | iso_floor_rift_glow_raw | iso_floor_rift_glow.png | 128×64 |
| 4 | iso_floor_broken_raw | iso_floor_broken.png | 128×64 |
| 5 | iso_floor_edge_light_raw | iso_floor_edge_light.png | 128×64 |
| 6 | iso_floor_debris_raw | iso_floor_debris.png | 128×64 |
| 7 | wall_nw_mid_plain_raw | wall_nw_mid_plain.png | **128×384** |
| 8 | wall_nw_mid_variant_raw | wall_nw_mid_variant.png | 128×384 |
| 9 | wall_nw_mid_broken_raw | wall_nw_mid_broken.png | 128×384 |
| 10 | wall_nw_doorway_raw | wall_nw_doorway.png | 128×384 |
| 11 | wall_ne_mid_plain_raw | wall_ne_mid_plain.png | 128×384 |
| 12 | wall_ne_mid_variant_raw | wall_ne_mid_variant.png | 128×384 |
| 13 | wall_ne_mid_broken_raw | wall_ne_mid_broken.png | 128×384 |
| 14 | wall_ne_doorway_raw | wall_ne_doorway.png | 128×384 |
| 15 | wall_n_corner_raw | wall_n_corner.png | 128×384 |
| 16 | wall_n_landmark_raw | wall_n_landmark.png | **256×384** (full width setpiece) |
| 17 | wall_pillar_universal_raw | wall_pillar_universal.png | 64×384 |

PIL procedure:
```python
from PIL import Image
img = Image.open(raw_path).convert('RGBA')

if asset_type == 'floor':
    img_resized = img.resize((128, 64), Image.NEAREST)
elif asset_type == 'wall_mid' or asset_type == 'wall_doorway' or asset_type == 'wall_corner':
    img_resized = img.resize((128, 384), Image.NEAREST)
elif asset_type == 'landmark':
    img_resized = img.resize((256, 384), Image.NEAREST)
elif asset_type == 'pillar':
    img_resized = img.resize((64, 384), Image.NEAREST)

# Green chroma key cleanup
pixels = img_resized.load()
for x in range(img_resized.width):
    for y in range(img_resized.height):
        r, g, b, a = pixels[x, y]
        if g > 200 and r < 80 and b < 80:
            pixels[x, y] = (0, 0, 0, 0)

img_resized.save(final_path, 'PNG')
```

### Görev 4: QC Raporu

`STAGING/walls_high_topdown_34_qc.md`:
- 3 yeni raw gen status (NW-broken + NE-broken + pillar)
- 17 final downscale results
- Archive operations
- Visual QC checklist (HIGH TOP-DOWN 3/4 uyumu, no banner/torch, palette, baseline)
- Verdict

## Git Commit

```bash
git add STAGING/concepts/fractured_chamber/iso_assets STAGING/walls_high_topdown_34_qc.md
git commit -m "[Codex] [WALLS HIGH TOPDOWN 3/4] 17 asset final (true iso terim REVOKED)"
```
