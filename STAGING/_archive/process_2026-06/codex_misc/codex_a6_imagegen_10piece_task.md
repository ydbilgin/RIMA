ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Codex Task — A6 Perimeter Wall 10 Piece Concept Art ($imagegen)

**Amaç:** RIMA Fractured Chamber için 10 perimeter wall piece concept art üret. Her piece için **`$imagegen` skill kullan** — OPENAI_API_KEY env var KONTROLÜ YAPMA, env var setup talimatı yazma. `$imagegen` skill Codex CLI tarafında abstraction sağlıyor.

User Phase 2'de Sonnet UnityMCP ile bu pieces'ı kullanarak 5 benzersiz oda kuracak. Sonradan PixelLab S-XL "new" + init image ile pixel art polish opsiyonel.

## ÖNEMLİ — $imagegen Skill Kullanımı

**HARD RULE:** Resim üretirken `$imagegen` skill kullan. Env var fallback yapma, OPENAI_API_KEY checkleme. Eğer skill çağrısı fail ederse hata mesajını raporla — env var sorunu varsayma.

## Pipeline Bağlamı

- Karakter: 64×64 chibi @ PPU 64
- Top-down 70-80° from horizon (NOT true iso 30/45°)
- Perimeter pattern: sadece NORTH + WEST görünür (iso/3/4 ARPG standardı, Diablo/Hades/CoM)
- South + East açık (camera tarafına bakar, wall yok)
- Pillar seam-cover strategy: pillar'lar wall mid'lerin arasına snap olur, seam'leri gizler
- Style lock: RIMA Shattered Keep, charcoal granite, cyan rift cracks, amber torch glow, rouge banner accents

## 10 Piece Spec

### NORTH WALL family (5 piece, 128×192)

| # | Piece | Filename | Açıklama |
|---|---|---|---|
| 1 | N-Mid-Plain | wall_north_mid_plain.png | Düz stone filler, tekrar edilebilir |
| 2 | N-Mid-Variant | wall_north_mid_variant.png | Alternative stone pattern (1'in farklı texture/damage versiyonu) |
| 3 | N-Doorway | wall_north_doorway.png | Stone arch + black void interior, no wood door |
| 4 | N-Landmark | wall_north_landmark.png | Center landmark — rift portal / dramatic arch + cyan glow |
| 5 | N-Torch-Alcove | wall_north_torch.png | Recessed alcove + empty iron torch bracket (no flame baked) |

### WEST WALL family (3 piece, 96×192)

| # | Piece | Filename | Açıklama |
|---|---|---|---|
| 6 | W-Mid-Plain | wall_west_mid_plain.png | Yan profil filler (E için flipX) |
| 7 | W-Doorway | wall_west_doorway.png | Yan stone arch opening (toward room interior) |
| 8 | W-Torch-Alcove | wall_west_torch.png | Yan torch alcove |

### CORNER (1 piece, 96×192)

| # | Piece | Filename | Açıklama |
|---|---|---|---|
| 9 | Corner-NW | wall_corner_nw.png | North-West outer corner birleşim noktası (south+east açık olduğu için sadece NW gerekli) |

### PILLAR (1 piece, 64×192)

| # | Piece | Filename | Açıklama |
|---|---|---|---|
| 10 | Pillar-Universal | wall_pillar_universal.png | Seam cover, decorative break, wall mid'lerin arasına |

## Style Lock Prompt (her piece'in prompt'una append)

```
Pixel art concept, 2D top-down ARPG dungeon scene, RIMA Shattered Keep dungeon style.
Dark fantasy medieval keep, fractured charcoal granite stone, crumbling masonry.
Palette: dark grey stone (#3A3040), warm mid-tone (#6B5C6E), cyan rift crack accent (#00FFCC), amber glow (#FFA864), banner rouge (#7A2832).
Hard pixel edges, limited palette feel, no anti-aliasing, no dithering gradients.
Pure 2D front-facing wall piece, flat orthographic view (NO perspective receding into depth, NO true isometric 30 degree).
Subtle 3/4 top-down thickness illusion (visible top cap + slight front face).
Transparent background, baseline at bottom of canvas, piece designed to compose modularly.
No characters, no UI, no text, no labels, no floor visible.
```

## Per-Piece Additional Prompt

1. **N-Mid-Plain:** "Plain stone wall section, uniform masonry, consistent stone edge on left and right for seamless tiling with pillars between segments. No decorations, no banner, no torch."

2. **N-Mid-Variant:** "Alternative stone wall section, slightly different brick pattern from plain mid, some chipped stones, subtle wear marks. Same baseline and top cap height as plain mid. Used for repetition variation."

3. **N-Doorway:** "Stone wall with a tall stone archway opening, completely black void interior visible through opening (NO wood door, NO iron gate), broken/cracked stones framing the arch top, slight rubble at base corners."

4. **N-Landmark:** "Stone wall with a dramatic central feature — large stone arch containing a cyan rift portal/crystal, glowing cyan energy emanating from a central point, arcane rune fragments embedded in stone around it. Medium emissive level (NOT brighter than gameplay VFX). Banner can optionally hang on side."

5. **N-Torch-Alcove:** "Stone wall with a recessed alcove containing an iron wall torch bracket at center, empty bracket (Unity 2D Light will add the flame glow), small soot stains around alcove, faint amber color hint on stones near bracket."

6. **W-Mid-Plain:** "Side wall (west/left) stone section facing inward toward room interior, same stone style as north pieces, slightly narrower profile (side perspective), seamless vertical tiling. No doorway, just plain stone."

7. **W-Doorway:** "Side wall (west/left) stone section with a tall stone archway opening facing toward room interior (opens to the right), black void inside, same stone style as north doorway."

8. **W-Torch-Alcove:** "Side wall (west/left) stone section with a recessed torch alcove facing inward, empty iron bracket, soot stains, faint amber tint on nearby stones."

9. **Corner-NW:** "Outer corner stone column piece at the north-west room corner, joining north wall (extending right) and west wall (extending down). L-shape composition, visible top cap, weathered granite, possibly with a thin cyan rift crack at the seam point."

10. **Pillar-Universal:** "Narrow standalone stone pillar, full vertical height, front-facing 3/4 view, square cross-section, decorative carved capital at top, slightly darker shade than wall pieces, weathered surface, faint cyan hairline crack on one side. Designed as seam cover between wall segments."

## Reference Images (her $imagegen call'a attach)

Her piece için 3 reference image base64 encode olarak ver (varsa):
1. Mevcut PIL mockup: `STAGING/concepts/fractured_chamber/wall_mockups/{name}.png` — composition guide
2. Style ref combat: `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_46 (1).png`
3. Style ref boss: `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_48 (4).png`

## Generation Procedure (her piece için)

```
For each piece in 10_piece_spec:
  1. Compose prompt: style_lock + per_piece_additional
  2. Call $imagegen skill:
     - prompt: composed_prompt
     - size: 1024x1024 (raw, high quality)
     - reference_images: [mockup, ref_combat, ref_boss]
     - output: STAGING/concepts/fractured_chamber/wall_pixelart/raw/{name}_raw.png
  3. PIL downscale to target dim (NEAREST resampling):
     - Save to STAGING/concepts/fractured_chamber/wall_pixelart/{name}.png
  4. Background cleanup:
     - Pure black / near-black pixels → alpha 0 (threshold-based)
     - Save final RGBA
  5. Log: filename, raw size, target size, SUCCESS/FAIL
```

## Önemli Notlar

- **`$imagegen` skill USE, NOT openai.images.generate manual call**
- Eğer `$imagegen` skill çağrısı fail ederse: error message rapor, BLOCKED ver
- NEAREST resampling sadece (pixel-perfect)
- Background transparent şart
- 10 piece sequential (paralel değil, stil drift önlemek için)
- Bütün 10 başarısız ise BLOCKED ver, user'a $imagegen skill diagnostic gerek

## Çıktı Dosyaları

```
STAGING/concepts/fractured_chamber/wall_pixelart/raw/
├─ wall_north_mid_plain_raw.png       (1024×1024)
├─ wall_north_mid_variant_raw.png     (1024×1024)
├─ wall_north_doorway_raw.png         (1024×1024)
├─ wall_north_landmark_raw.png        (1024×1024)
├─ wall_north_torch_raw.png           (1024×1024)
├─ wall_west_mid_plain_raw.png        (1024×1024)
├─ wall_west_doorway_raw.png          (1024×1024)
├─ wall_west_torch_raw.png            (1024×1024)
├─ wall_corner_nw_raw.png             (1024×1024)
└─ wall_pillar_universal_raw.png      (1024×1024)

STAGING/concepts/fractured_chamber/wall_pixelart/
├─ wall_north_mid_plain.png           (128×192)
├─ wall_north_mid_variant.png         (128×192)
├─ wall_north_doorway.png             (128×192)
├─ wall_north_landmark.png            (128×192)
├─ wall_north_torch.png               (128×192)
├─ wall_west_mid_plain.png            (96×192)
├─ wall_west_doorway.png              (96×192)
├─ wall_west_torch.png                (96×192)
├─ wall_corner_nw.png                 (96×192)
└─ wall_pillar_universal.png          (64×192)

STAGING/a6_wall_imagegen_codex_qc.md
```

## QC Raporu

`STAGING/a6_wall_imagegen_codex_qc.md`:

```markdown
# A6 Wall Imagegen QC

## Setup
- $imagegen skill: WORKING / FAIL
- Reference images: ATTACHED / MISSING

## Generation Results
| # | Piece | Raw Path | Final Path | Status |
|---|---|---|---|---|
| 1 | N-Mid-Plain | ... | ... | PASS/FAIL |
... (10 piece)

## Visual QC
- [ ] RIMA palette consistent across all 10
- [ ] 3/4 thickness illusion visible
- [ ] Baseline aligned (alt kenar Y aynı)
- [ ] Transparent BG clean
- [ ] No text/UI baked
- [ ] Pixel art aesthetic (hard edges, limited palette)

## Verdict
- PASS / PARTIAL / BLOCKED

## Next Step
- Sonnet'e teslim et — UnityMCP ile 5 oda kurma için hazır
```

## Git Commit

```bash
git add STAGING/concepts/fractured_chamber/wall_pixelart STAGING/a6_wall_imagegen_codex_qc.md
git commit -m "[Codex] [A6 IMAGEGEN] 10 perimeter wall piece concept art via \$imagegen"
```
