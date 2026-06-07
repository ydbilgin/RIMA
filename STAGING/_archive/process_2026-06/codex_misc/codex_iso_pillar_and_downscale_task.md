ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

# Codex Task — Pillar Üretim + 17 Iso Asset Final Downscale

**Amaç:** 16/17 raw asset mevcut (önceki dispatch'te üretildi, 1024×1024). Eksik 1 piece (pillar) için **$imagegen built-in tool mode** ile üret, sonra 17 raw'ı revize spec'e downscale et.

## KRİTİK — `$imagegen` BUILT-IN TOOL MODE LOCK

**Use the `$imagegen` skill in DEFAULT BUILT-IN TOOL MODE (preferred path):**
- Built-in `image_gen` tool — **does NOT require OPENAI_API_KEY**
- DO NOT use CLI fallback (`scripts/image_gen.py`)
- DO NOT export OPENAI_API_KEY
- DO NOT use `openai` Python SDK direct call
- Skill location: `~/.codex/skills/.system/imagegen/` — built-in tool path

If built-in tool fails: BLOCKED report, do NOT silently switch to CLI fallback. Orchestrator will handle.

## Mevcut 16 Raw

`STAGING/concepts/fractured_chamber/iso_assets/raw/` (1024×1024 each):
- iso_floor_clean_raw.png
- iso_floor_cracked_raw.png
- iso_floor_rift_glow_raw.png
- iso_floor_broken_raw.png
- iso_floor_edge_light_raw.png
- iso_floor_debris_raw.png
- wall_nw_mid_plain_raw.png
- wall_nw_mid_variant_raw.png
- wall_nw_doorway_raw.png
- wall_nw_torch_raw.png
- wall_ne_mid_plain_raw.png
- wall_ne_mid_variant_raw.png
- wall_ne_doorway_raw.png
- wall_ne_torch_raw.png
- wall_n_corner_raw.png
- wall_n_landmark_raw.png

## Görev 1 — Pillar Raw Üret ($imagegen built-in tool)

**Asset:** Pillar-Universal — iso diagonal seam cover stone column
**Output:** `STAGING/concepts/fractured_chamber/iso_assets/raw/wall_pillar_universal_raw.png` (1024×1024)
**Target final boyut:** 64×192

**Prompt:**
```
Single narrow standalone iso stone pillar concept art for RIMA Shattered Keep dungeon.
True 45 degree isometric perspective like Diablo II / Children of Morta / Hades, NOT top-down, NOT rectangular front-facing.
Tall vertical column, full height matching wall mid pieces (about 3 character heights tall),
iso 3/4 perspective showing front face + slight side face (3D thickness illusion).
Square cross-section, decorative carved capital at top.
Material: charcoal fractured granite stone, slightly darker shade than wall mid pieces, weathered surface.
Possibly thin cyan rift hairline crack on one side.
Used as seam cover between adjacent iso wall mid pieces.
Transparent background.
Dark fantasy pixel art aesthetic, hard pixel edges, limited palette: dark grey (#3A3040), mid (#6B5C6E), cyan (#00FFCC), amber (#FFA864).
NO characters, NO UI, NO text, NO labels, NO banner, NO floor visible.
1024x1024 canvas, asset centered with transparent padding.
```

**Reference images** (encode base64, attach if built-in tool supports references):
1. `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_46 (1).png`
2. `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_48 (4).png`
3. `STAGING/concepts/fractured_chamber/iso_assets/raw/wall_nw_mid_plain_raw.png` (matching style)

## Görev 2 — 17 Raw'ı Final Boyuta Downscale (PIL)

Tüm 17 raw'ı NEAREST resampling ile downscale + background cleanup (near-black → alpha 0):

| # | Raw | Final Path | Target Boyut |
|---|---|---|---|
| 1 | iso_floor_clean_raw.png | iso_assets/iso_floor_clean.png | 128×64 |
| 2 | iso_floor_cracked_raw.png | iso_assets/iso_floor_cracked.png | 128×64 |
| 3 | iso_floor_rift_glow_raw.png | iso_assets/iso_floor_rift_glow.png | 128×64 |
| 4 | iso_floor_broken_raw.png | iso_assets/iso_floor_broken.png | 128×64 |
| 5 | iso_floor_edge_light_raw.png | iso_assets/iso_floor_edge_light.png | 128×64 |
| 6 | iso_floor_debris_raw.png | iso_assets/iso_floor_debris.png | 128×64 |
| 7 | wall_nw_mid_plain_raw.png | iso_assets/wall_nw_mid_plain.png | 128×192 |
| 8 | wall_nw_mid_variant_raw.png | iso_assets/wall_nw_mid_variant.png | 128×192 |
| 9 | wall_nw_doorway_raw.png | iso_assets/wall_nw_doorway.png | 128×192 |
| 10 | wall_nw_torch_raw.png | iso_assets/wall_nw_torch.png | 128×192 |
| 11 | wall_ne_mid_plain_raw.png | iso_assets/wall_ne_mid_plain.png | 128×192 |
| 12 | wall_ne_mid_variant_raw.png | iso_assets/wall_ne_mid_variant.png | 128×192 |
| 13 | wall_ne_doorway_raw.png | iso_assets/wall_ne_doorway.png | 128×192 |
| 14 | wall_ne_torch_raw.png | iso_assets/wall_ne_torch.png | 128×192 |
| 15 | wall_n_corner_raw.png | iso_assets/wall_n_corner.png | 128×192 |
| 16 | wall_n_landmark_raw.png | iso_assets/wall_n_landmark.png | 128×224 |
| 17 | wall_pillar_universal_raw.png | iso_assets/wall_pillar_universal.png | 64×192 |

**Downscale procedure (PIL):**
```python
from PIL import Image
img = Image.open(raw_path).convert('RGBA')
img_resized = img.resize((target_w, target_h), Image.NEAREST)
# Background cleanup: near-black/green chroma → alpha 0
# Check if raw uses green chroma (looks like #00FF00) — if yes, threshold filter
# Save as RGBA PNG
img_resized.save(final_path, 'PNG')
```

**Note:** Önceki raw'lar `#00FF00` green chroma background ile üretildi (test ettim ben). Background cleanup'ta green pixels → alpha 0 threshold (RGB ≈ 0,255,0 ± 20 tolerance).

## Görev 3 — QC Raporu

`STAGING/iso_asset_codex_qc_v2.md`:

```markdown
# Iso Asset Final QC v2

## $imagegen Tool Mode
- Mode used: BUILT-IN / CLI / FAIL
- Pillar generation: SUCCESS / BLOCKED

## Downscale Results (17 piece)
| # | Raw Path | Final Path | Status | Notes |
| 1 | iso_floor_clean_raw | iso_floor_clean | PASS | 1024→128×64 |
| ... | ... | ... | ... | ... |

## Visual QC
- [ ] True iso diamond perspective
- [ ] Transparent BG clean (no green chroma residue)
- [ ] Stone palette tutarlı
- [ ] Wall pieces baseline aligned
- [ ] Pillar height matches wall mid (192px)
- [ ] Landmark dramatic (224px setpiece)

## Verdict
- PASS / PARTIAL / BLOCKED

## Next Step
- Sub-agent UnityMCP import + iso grid setup + 1 test oda manual
```

## Git Commit

```bash
git add STAGING/concepts/fractured_chamber/iso_assets STAGING/iso_asset_codex_qc_v2.md
git commit -m "[Codex] [ISO DIAMOND v2] pillar gen + 17 final downscale (built-in imagegen tool)"
```

## Hard Constraints

- **$imagegen built-in tool mode SADECE** (CLI fallback ASLA, OPENAI_API_KEY env var arama)
- NEAREST resampling sadece (no bilinear, no anti-alias)
- Background cleanup (green chroma → alpha 0)
- 17 final asset standart yerinde olmalı
- Eğer built-in tool pillar gen fail → BLOCKED, retry için orchestrator karar verir
