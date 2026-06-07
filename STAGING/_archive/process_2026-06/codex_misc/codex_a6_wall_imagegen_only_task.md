ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Codex Task — A6 Wall Pixel Art (gpt-image-1) — PHASE 1 ONLY

**Amaç:** 8 wall piece için gpt-image-1 ile pixel art concept üret + PIL ile target boyuta downscale + STAGING'e kaydet. Unity scene KURMA — sonra rima-sonnet sub-agent UnityMCP ile yapacak.

## Pre-flight
1. `OPENAI_API_KEY` env var kontrol et. Yoksa: **BLOCKED — User must set it before running.**
2. Output klasörü: `STAGING/concepts/fractured_chamber/wall_pixelart/` (varsa overwrite, yoksa create)
3. Raw subfolder: `STAGING/concepts/fractured_chamber/wall_pixelart/raw/`

## 8 Piece Spec

| Piece | Filename | gpt-image-1 canvas | Target PNG |
|---|---|---|---|
| Plain north mid | wall_north_mid_plain.png | 1024×1024 | 128×192 |
| Banner north mid | wall_north_mid_banner.png | 1024×1024 | 128×192 |
| Torch north mid | wall_north_mid_torch.png | 1024×1024 | 128×192 |
| Doorway north | wall_north_doorway.png | 1024×1024 | 128×192 |
| Rift portal center | wall_north_center_rift_portal.png | 1024×1024 | 128×192 |
| Pillar universal | wall_pillar_universal.png | 1024×1024 | 64×192 |
| West plain mid | wall_west_mid_plain.png | 1024×1024 | 96×192 |
| West doorway | wall_west_doorway.png | 1024×1024 | 96×192 |

## Style Lock Prompt (her piece'in prompt'una append)

```
Pixel art, 2D top-down dungeon game, RIMA Shattered Keep dungeon, Act 1.
Dark stone masonry, crumbling medieval keep, warm amber/orange torch glow lighting from below.
Palette: dark grey stone (#3A3040), warm mid-tone (#6B5C6E), cyan rift crack accent (#00FFCC), amber glow (#FFA864), banner rouge (#7A2832).
Clean pixel art, limited palette, no anti-aliasing, no dithering gradients, hard pixel edges.
Pure 2D front-facing wall piece, flat orthographic view (NO perspective receding into depth, NO true isometric 30 degree angle).
Solid stone wall segment — heavy, oppressive, slightly worn, fractured granite.
Subtle 3/4 top-down thickness illusion (visible top cap + slight front face).
Transparent background, baseline at bottom.
No characters, no UI, no text, no labels, no floor visible.
```

## Per-piece Additional Prompt

- **wall_north_mid_plain:** "Plain stone wall section, uniform masonry, no decorations. Consistent stone edge on left and right for seamless tiling between segments."
- **wall_north_mid_banner:** "Stone wall section with a hanging banner/tapestry in dark crimson rouge, tattered edges, faint heraldic symbol (stylized rune or skull)."
- **wall_north_mid_torch:** "Stone wall section with a recessed alcove containing an iron wall torch bracket at center, empty bracket (no flame baked — Unity 2D Light will add glow), small soot stains around alcove."
- **wall_north_doorway:** "Stone wall with a tall stone archway opening, completely black void interior (NO wood door), broken/cracked stones framing the arch, slight rubble at base corners."
- **wall_north_center_rift_portal:** "Stone wall with a dramatic central arch containing a rift portal — glowing cyan energy crystal/portal, jagged crack emanating from a central point, arcane rune fragments embedded in stone around it. Medium emissive (NOT brighter than gameplay VFX). Landmark piece."
- **wall_pillar_universal:** "Narrow standalone stone pillar, full vertical height, front-facing 3/4 view, square cross-section, decorative carved capital at top, slightly darker shade than wall pieces, weathered surface. Used as seam cover between wall segments. Transparent BG."
- **wall_west_mid_plain:** "Side wall (west/left) stone section, same stone style as north pieces, slightly narrower profile (perspective from side angle), seamless vertical tiling. NO doorway, just wall."
- **wall_west_doorway:** "Side wall (west/left) stone section with a tall stone archway opening facing toward room interior, black void inside, same stone style as north doorway."

## Reference Images (encode as base64, pass as reference)

For each piece, include these references:
1. `STAGING/concepts/fractured_chamber/wall_mockups/{piece_name}.png` — composition guide (piece-specific PIL mockup, layout reference)
2. `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_46 (1).png` — style ref (combat room)
3. `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_48 (4).png` — style ref (boss room)

## Generation Procedure (per piece)

```python
import openai
from PIL import Image
import base64, os

for piece in pieces:
    # 1. Encode references as base64
    refs = [base64_encode(open(p, 'rb').read()) for p in piece.references]
    
    # 2. Call gpt-image-1
    response = openai.images.generate(
        model="gpt-image-1",
        prompt=style_lock + piece.additional_prompt,
        size="1024x1024",
        quality="high",
        # reference_images=refs  # if API supports
    )
    
    # 3. Save raw
    raw_path = f"STAGING/concepts/fractured_chamber/wall_pixelart/raw/{piece.name}_raw.png"
    with open(raw_path, 'wb') as f:
        f.write(base64.b64decode(response.data[0].b64_json))
    
    # 4. PIL downscale (NEAREST — pixel-perfect)
    img = Image.open(raw_path)
    img_resized = img.resize((piece.target_w, piece.target_h), Image.NEAREST)
    
    # 5. Background transparent (threshold near-black → alpha 0)
    img_rgba = img_resized.convert('RGBA')
    # threshold logic: if pixel ≈ near-black, set alpha 0
    
    # 6. Save final
    final_path = f"STAGING/concepts/fractured_chamber/wall_pixelart/{piece.name}.png"
    img_rgba.save(final_path, 'PNG')
    
    # 7. Log
    print(f"[OK] {piece.name}: {raw_path} → {final_path}")
```

## Fallback

Eğer 1 piece API call fail ederse: log `[WARN] {name} failed: {error}`, continue with kalan piece'ler. Phase 1 toplu abort DEĞİL.

Tüm 8 fail ederse VEYA `OPENAI_API_KEY` yoksa: BLOCKED report, user'a env var setup gerekliliği bildir.

## QC Raporu

`STAGING/a6_wall_pixelart_codex_qc.md`:

```markdown
# A6 Wall Pixel Art Generation QC

## Setup
- OPENAI_API_KEY: SET/MISSING
- gpt-image-1 erişim: OK/FAIL

## Piece Generation
| Piece | Raw Path | Final Path | Raw Size | Target Size | Status |
|---|---|---|---|---|---|
| ... | ... | ... | ... | ... | PASS/FAIL |

## Visual QC (her piece için)
- [ ] RIMA style consistent (charcoal + cyan + amber + rouge palette)
- [ ] 3/4 thickness hint görünür
- [ ] Baseline alt kenarda
- [ ] Transparent BG temiz
- [ ] Pixel art (no anti-aliasing)
- [ ] No characters/UI/text baked

## Verdict
- PASS / PARTIAL / BLOCKED

## Next Step
- Sonnet'e teslim et (UnityMCP scene setup için)
```

## Hard Constraints

- **NO Unity setup bu task'ta** (sonnet yapacak)
- NEAREST resampling sadece (no bilinear)
- Background transparent şart
- Reference images mockup + chatgpt_ref ikisi de attach
- Git commit at end: `git add STAGING/concepts/fractured_chamber/wall_pixelart STAGING/a6_wall_pixelart_codex_qc.md && git commit -m "[Codex] [A6 IMAGEGEN] 8 wall piece pixel art via gpt-image-1"`

## Output Files

```
STAGING/concepts/fractured_chamber/wall_pixelart/raw/
├─ wall_north_mid_plain_raw.png       (1024×1024)
├─ wall_north_mid_banner_raw.png      (1024×1024)
├─ wall_north_mid_torch_raw.png       (1024×1024)
├─ wall_north_doorway_raw.png         (1024×1024)
├─ wall_north_center_rift_portal_raw.png (1024×1024)
├─ wall_pillar_universal_raw.png      (1024×1024)
├─ wall_west_mid_plain_raw.png        (1024×1024)
└─ wall_west_doorway_raw.png          (1024×1024)

STAGING/concepts/fractured_chamber/wall_pixelart/
├─ wall_north_mid_plain.png           (128×192)
├─ wall_north_mid_banner.png          (128×192)
├─ wall_north_mid_torch.png           (128×192)
├─ wall_north_doorway.png             (128×192)
├─ wall_north_center_rift_portal.png  (128×192)
├─ wall_pillar_universal.png          (64×192)
├─ wall_west_mid_plain.png            (96×192)
└─ wall_west_doorway.png              (96×192)

STAGING/a6_wall_pixelart_codex_qc.md
```
