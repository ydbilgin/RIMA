ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

# Codex Task — Iso Walls MAX Dramatic (17 asset, $imagegen built-in, 256×768 native)

**Amaç:** RIMA için MAX DRAMATIC iso duvar sistemi. PixelLab S-XL "new" non-square max **256×768** (veya 340×768) native üretim. chatgpt_ref'in tam dramatic ratio'su (char:wall = 1:6.4 max). Sadece duvarlar (torch yok, user PixelLab'dan ayrı yapacak).

## KRİTİK — $imagegen BUILT-IN TOOL MODE

Memory: `feedback_codex_imagegen_skill_not_env_var.md`

**Use the `$imagegen` skill in DEFAULT BUILT-IN TOOL MODE only.**
- Built-in `image_gen` tool — **does NOT require OPENAI_API_KEY**
- DO NOT use CLI fallback
- DO NOT export OPENAI_API_KEY
- Skill: `~/.codex/skills/.system/imagegen/`

## PixelLab Üretim Boyutu

**Native raw aspect:** 256×768 (1:3 dikey, non-square mode)
**Final downscale:** 128×768 (wall pieces), 256×768 (landmark setpiece keeps full raw), 64×768 (pillar slim)

PixelLab S-XL "new" non-square mode 256×768 destekliyor (user 2026-05-24 confirmed). Bazı tool'larda 340×768 max. 256×768 güvenli.

## Pipeline Bağlamı

- Karakter: Warblade sprite 120×120 PNG (gerçek effective ~80-100px silüet)
- **Char:Wall ratio: 1:6.4** (chatgpt_ref MAX dramatic — char 120 × wall 768)
- TRUE 45° iso diamond perspective (NOT top-down rectangular)
- Sadece NW + NE walls visible (south + east open)
- **NO TORCH** (user PixelLab'dan ayrı obje yapacak)
- NO banner, doorway içi boş
- Pillar seam-cover strategy LIVE (memory: project_pillar_seam_cover_lock_2026_05_24.md)
- Background green chroma key (user temizler, alpha cleanup PIL ile final aşamada)

## Üretilecek 14 Wall Asset (PixelLab raw 256×768 each)

### NW WALL family (4 piece)

**1. wall_nw_mid_plain_raw.png** (256×768)
```
Single ULTRA TALL pixel art wall piece for RIMA Shattered Keep iso dungeon.
TRUE 45 degree isometric perspective like Diablo II / Children of Morta / Hades, NOT top-down, NOT rectangular front-facing.
NW direction diagonal stone wall sloping from lower-left to upper-right (~26.57 degree iso angle), 256x768 canvas with 1:6.4 char-wall ratio (TALL DRAMATIC DUNGEON WALL).
Plain uniform charcoal granite masonry, no banner, no torch, no decorations.
Visible top cap + slight side face (3/4 thickness illusion in iso).
Baseline at bottom of canvas, top edge follows iso slope with stone cap.
Material: charcoal fractured granite (#3A3040), warm mid-tone (#6B5C6E), thin cyan rift hairline cracks sparse (#00FFCC).
Green chroma key background (#00FF00) for transparent removal.
Dark fantasy pixel art aesthetic, hard pixel edges, limited palette, no anti-aliasing.
NO characters, NO UI, NO text, NO labels, NO banner, NO torch, NO floor visible.
```

**2. wall_nw_mid_variant_raw.png** (256×768)
Same as plain but alternative stone pattern, slightly different brick layout, some chipped stones, repetition variation.

**3. wall_nw_mid_broken_raw.png** (256×768)
Heavily damaged NW wall — partially collapsed at upper section, missing stones expose void behind, faint cyan rift glow leaking through cracks, low rubble at base, organic decay feel. Still same wall family height (768) and width (256) for tile compatibility.

**4. wall_nw_doorway_raw.png** (256×768)
NW diagonal stone archway opening, tall stone arch, completely black void interior visible (NO wood door, NO iron gate, NO banners), broken stones framing arch top, slight rubble at base corners.

### NE WALL family (4 piece — NW'nin flipX değil, ışık yönü farklı)

**5. wall_ne_mid_plain_raw.png** (256×768)
NE direction diagonal stone wall sloping from lower-right to upper-left, lighting hits from upper-right (different shadow than NW), 256x768, plain uniform stone.

**6. wall_ne_mid_variant_raw.png** (256×768)
NE variant pattern.

**7. wall_ne_mid_broken_raw.png** (256×768)
NE heavily damaged variant.

**8. wall_ne_doorway_raw.png** (256×768)
NE diagonal archway opening, empty void interior.

### TOP VERTEX + LANDMARK + PILLAR (3 piece)

**9. wall_n_corner_raw.png** (256×768)
Top corner vertex stone column where NW and NE walls meet at the top of iso diamond room, peak stone column with both wall slopes converging, weathered granite, slight cyan crack at seam point, same height (768) as wall mid pieces for seamless top alignment.

**10. wall_n_landmark_raw.png** (256×768)
SETPIECE landmark feature — large iso stone archway at top vertex of diamond room, containing cyan rift portal/crystal, glowing cyan energy emanating from central point, arcane rune fragments embedded around it, medium emissive (NOT brighter than gameplay VFX), NO banner, dramatic setpiece for boss room or ritual chamber.

**11. wall_pillar_universal_raw.png** (256×768 — daha sonra 64×768'e crop edilecek)
TALL standalone iso stone pillar, full vertical height matching wall mid pieces (768), iso 3/4 perspective with front + slight side face, square cross-section, decorative carved capital at top, slightly darker than wall pieces, weathered surface, thin cyan rift hairline crack on one side. Used as seam cover between adjacent iso wall mid pieces. NARROW silhouette in 256-wide canvas (centered, transparent padding around).

## Mevcut 6 Floor Raw KULLAN (downscale)

`STAGING/concepts/fractured_chamber/iso_assets/raw/`:
- iso_floor_clean_raw.png (1024×1024)
- iso_floor_cracked_raw.png
- iso_floor_rift_glow_raw.png
- iso_floor_broken_raw.png
- iso_floor_edge_light_raw.png
- iso_floor_debris_raw.png

Bunlar 1024×1024 sq — 128×64 final için downscale OK.

## ARCHIVE eski wall raw'ları

```bash
mkdir -p STAGING/concepts/fractured_chamber/iso_assets/raw/_archive_v1/
mv STAGING/concepts/fractured_chamber/iso_assets/raw/wall_*_raw.png STAGING/concepts/fractured_chamber/iso_assets/raw/_archive_v1/
```

## Reference Images (her $imagegen call'a attach)

1. `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_46 (1).png` — combat room
2. `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_48 (4).png` — boss room
3. `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_48 (5).png` — ritual chamber

## 17 Final Downscale Spec (PIL NEAREST)

| # | Raw | Final Path | Target Boyut |
|---|---|---|---|
| 1 | iso_floor_clean_raw.png (mevcut) | iso_floor_clean.png | 128×64 |
| 2 | iso_floor_cracked_raw.png | iso_floor_cracked.png | 128×64 |
| 3 | iso_floor_rift_glow_raw.png | iso_floor_rift_glow.png | 128×64 |
| 4 | iso_floor_broken_raw.png | iso_floor_broken.png | 128×64 |
| 5 | iso_floor_edge_light_raw.png | iso_floor_edge_light.png | 128×64 |
| 6 | iso_floor_debris_raw.png | iso_floor_debris.png | 128×64 |
| 7 | wall_nw_mid_plain_raw.png (yeni 256×768) | wall_nw_mid_plain.png | **128×768** |
| 8 | wall_nw_mid_variant_raw.png | wall_nw_mid_variant.png | 128×768 |
| 9 | wall_nw_mid_broken_raw.png | wall_nw_mid_broken.png | 128×768 |
| 10 | wall_nw_doorway_raw.png | wall_nw_doorway.png | 128×768 |
| 11 | wall_ne_mid_plain_raw.png | wall_ne_mid_plain.png | 128×768 |
| 12 | wall_ne_mid_variant_raw.png | wall_ne_mid_variant.png | 128×768 |
| 13 | wall_ne_mid_broken_raw.png | wall_ne_mid_broken.png | 128×768 |
| 14 | wall_ne_doorway_raw.png | wall_ne_doorway.png | 128×768 |
| 15 | wall_n_corner_raw.png | wall_n_corner.png | 128×768 |
| 16 | wall_n_landmark_raw.png | wall_n_landmark.png | **256×768** (full raw, setpiece) |
| 17 | wall_pillar_universal_raw.png | wall_pillar_universal.png | 64×768 (crop slim from 256) |

**PIL downscale procedure:**
```python
from PIL import Image
img = Image.open(raw_path).convert('RGBA')

# For wall pieces (256→128): aspect korunur, NEAREST downscale
if target == "wall_mid":
    img_resized = img.resize((128, 768), Image.NEAREST)
elif target == "pillar":
    # Center crop slim pillar from 256 wide
    left = (256 - 64) // 2
    img_cropped = img.crop((left, 0, left + 64, 768))
    img_resized = img_cropped  # Already 64×768
elif target == "landmark":
    img_resized = img  # Keep full 256×768
elif target == "floor":
    img_resized = img.resize((128, 64), Image.NEAREST)

# Green chroma key cleanup → alpha 0
pixels = img_resized.load()
for x in range(img_resized.width):
    for y in range(img_resized.height):
        r, g, b, a = pixels[x, y]
        if g > 200 and r < 80 and b < 80:
            pixels[x, y] = (0, 0, 0, 0)

img_resized.save(final_path, 'PNG')
```

## Git Commit

```bash
git add STAGING/concepts/fractured_chamber/iso_assets STAGING/iso_walls_max_dramatic_qc.md
git commit -m "[Codex] [ISO MAX DRAMATIC] 14 wall yenidenüret 256×768, landmark 256×768 setpiece, char ratio 1:6.4"
```

## QC Raporu

`STAGING/iso_walls_max_dramatic_qc.md`:
- $imagegen mode (BUILT-IN/CLI/FAIL)
- 14 wall raw production status
- Eski wall raw'lar archive
- 17 final downscale results
- Visual QC checklist
- Verdict

## Hard Constraints

- **$imagegen BUILT-IN TOOL mode** (CLI ASLA, OPENAI_API_KEY arama)
- **256×768 raw native üretim** (PixelLab non-square mode)
- NEAREST resampling
- Green chroma → alpha 0
- 17 final asset standart yerinde
- Torch YOK (user yapacak)
- Eğer $imagegen 256×768 destek vermezse → 256×512 fallback (raw 1024 sq → 128×512 downscale yine dramatic)
