ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Codex Task — RIMA ISO DIAMOND Asset Set (17 piece, $imagegen)

**Amaç:** RIMA için **TRUE ISOMETRIC DIAMOND** geometry asset seti üret. ChatGPT_ref tarzı diamond perspective oda kurulumu için. 6 iso floor tile + 11 iso wall piece = 17 toplam. **`$imagegen` skill kullan**, env var KONTROLÜ YAPMA.

User 2026-05-24 LIVE direktifleri:
- TRUE 45° isometric diamond shape (NOT top-down, NOT rectangular)
- Sadece NW + NE walls visible (south + east open, camera tarafı)
- NO banner (önceki landmark'ta banner vardı, kaldırıldı)
- Doorway içi boş (geometric arch + black void, no wood door no decor)
- Torch alcove sadece empty bracket (Unity 2D Light flame ekler, baked YOK)
- Pillar seam-cover live (S101 PILLAR-LESS REVOKED, memory: project_pillar_seam_cover_lock_2026_05_24.md)

## Geometry Lock (TÜM asset'ler için)

```
              N VERTEX (top corner)
                  /\
                 /  \
             NW /    \ NE
               /      \
              /        \
       W VERT  FLOOR    E VERT
              \        /
               \      /
                \    /
                 \  /
                  \/
              S VERTEX (open, camera direction)
```

- Camera angle: TRUE 45° isometric, axis-aligned diamond
- Floor tile shape: rhombus (2:1 aspect ratio, top-down with iso projection)
- Wall pieces: diagonal slope along NW (left-up) or NE (right-up) direction
- Reference: chatgpt_ref/ChatGPT Image 22 May 2026 16_12_46 (1).png — combat room
- Reference: chatgpt_ref/ChatGPT Image 22 May 2026 16_12_48 (4).png — boss room
- Reference: chatgpt_ref/ChatGPT Image 22 May 2026 16_12_48 (5).png — ritual chamber

## Style Lock Prompt (her piece'in prompt'una append)

```
True 45 degree isometric diamond view pixel art, classic ARPG perspective like Diablo II / Children of Morta / Hades, NOT top-down, NOT rectangular front-facing.
Dark fantasy RIMA Shattered Keep dungeon style.
Dark fractured charcoal granite stone, crumbling medieval keep.
Palette: dark grey stone (#3A3040), warm mid-tone (#6B5C6E), cyan rift crack accent (#00FFCC), amber glow (#FFA864). NO red banner accent (banner removed from spec).
Hard pixel edges, limited palette, no anti-aliasing, no dithering gradients.
Transparent background, asset designed to compose with adjacent iso pieces seamlessly.
No characters, no UI, no text, no labels, no floor visible inside wall sprites.
```

## 17 Piece Spec

### A. ISO FLOOR TILESET (6 piece, 128×64 paralelkenar)

| # | Piece | Filename | Açıklama |
|---|---|---|---|
| 1 | Iso-Floor-Clean | iso_floor_clean.png | Düz iso flagstone, rhombus shape |
| 2 | Iso-Floor-Cracked | iso_floor_cracked.png | Cracked stone variant |
| 3 | Iso-Floor-Rift-Glow | iso_floor_rift_glow.png | Subtle cyan hairline cracks |
| 4 | Iso-Floor-Broken | iso_floor_broken.png | Chipped corner stone, slight rubble |
| 5 | Iso-Floor-Edge-Light | iso_floor_edge_light.png | Torch-close stone, faint amber tint |
| 6 | Iso-Floor-Debris | iso_floor_debris.png | Small loose rubble overlay on stone |

**Floor tile geometry:** Diamond/rhombus shape, 128 wide × 64 tall, paralelkenar (2:1 iso aspect). Top-down with classic iso projection — tile looks like a flat ground rhombus when placed in iso grid.

### B. NW WALL family (4 piece, 128×192 diagonal NW slope) [REVIZE 2026-05-24]

| # | Piece | Filename | Açıklama |
|---|---|---|---|
| 7 | NW-Mid-Plain | wall_nw_mid_plain.png | Plain stone wall diagonal NW slope (left-up direction), filler |
| 8 | NW-Mid-Variant | wall_nw_mid_variant.png | Alternative brick pattern, same slope angle |
| 9 | NW-Doorway | wall_nw_doorway.png | Diagonal stone arch opening, empty void interior (NO door, NO decor) |
| 10 | NW-Torch-Alcove | wall_nw_torch.png | Recessed alcove + empty iron bracket (NO flame baked) |

**NW wall geometry:** Wall band slopes from lower-left to upper-right (NW direction in iso). **128 wide × 192 tall** (Codex+Claude consensus: char:wall ratio 1:2.4-3.0 chatgpt_ref ile uyumlu). Top edge follows iso angle (~26.57°, atan(0.5)). Bottom edge at canvas baseline. Wall base width = floor tile width (128) — iso snapping seamless.

### C. NE WALL family (4 piece, 128×192 diagonal NE slope — NW flipX değil, ışık yönü farklı)

| # | Piece | Filename | Açıklama |
|---|---|---|---|
| 11 | NE-Mid-Plain | wall_ne_mid_plain.png | Plain stone wall diagonal NE slope (right-up direction), filler |
| 12 | NE-Mid-Variant | wall_ne_mid_variant.png | Alternative brick pattern |
| 13 | NE-Doorway | wall_ne_doorway.png | Diagonal stone arch opening, empty void interior |
| 14 | NE-Torch-Alcove | wall_ne_torch.png | Recessed alcove + empty iron bracket |

**NE wall geometry:** Wall band slopes from lower-right to upper-left (NE direction). **128×192**. Mirror of NW direction-wise but lighting hits from upper-right (different shadows). Same iso angle + base width = floor width.

### D. TOP VERTEX + LANDMARK + PILLAR (3 piece)

| # | Piece | Filename | Boyut | Açıklama |
|---|---|---|---|---|
| 15 | N-Corner-Vertex | wall_n_corner.png | **128×192** | Top vertex of diamond — NW + NE walls birleşim, peak stone column. Standard wall family height. |
| 16 | N-Landmark | wall_n_landmark.png | **128×224** | Top vertex dramatic — cyan rift portal/crystal + iso arch frame, NO banner. Setpiece variant (32px ekstra height for dramatic landmark). |
| 17 | Pillar-Universal | wall_pillar_universal.png | **64×192** | Diagonal seam cover, fits between NW/NE mid pieces, iso column. Wall mid ile aynı yükseklik (snap uyumu). |

## Per-Piece Additional Prompt

1. **Iso-Floor-Clean:** "Single iso pixel art ground tile, 128x64 rhombus paralelkenar shape (2:1 aspect), classic 45 degree isometric projection of flat dark granite stone floor, uniform masonry, no cracks, no rubble, no walls, top-down with iso projection — tile fits in iso grid like Diablo II floor."

2. **Iso-Floor-Cracked:** "Same as iso floor clean but with sparse hairline cracks running across the stone, slight chipped edges, still flat walkable iso rhombus tile."

3. **Iso-Floor-Rift-Glow:** "Iso floor tile with subtle thin cyan rift hairline crack pattern, medium-low emissive cyan (below gameplay VFX brightness), flat walkable rhombus shape."

4. **Iso-Floor-Broken:** "Iso floor tile with one chipped corner showing small rubble/dust pixels, otherwise flat walkable rhombus stone."

5. **Iso-Floor-Edge-Light:** "Iso floor tile with faint warm amber tint on one edge, as if a nearby torch casts warm light onto the stone, otherwise plain rhombus floor."

6. **Iso-Floor-Debris:** "Iso floor tile with small loose stone debris/rubble scattered on top, low silhouette overlay, still walkable rhombus stone underneath."

7. **NW-Mid-Plain:** "Plain stone wall sloping diagonally from lower-left to upper-right (NW direction in iso view), 128x192, classic iso 45 degree perspective, uniform charcoal granite masonry, tall dramatic dungeon wall (3x character height for chatgpt_ref combat readability), consistent slope angle (~26.57 degrees atan(0.5) iso angle), baseline at bottom of canvas, top edge follows iso slope with visible top cap, visible side face (3D thickness illusion in iso), NO decorations, NO banner, NO torch."

8. **NW-Mid-Variant:** "Alternative NW diagonal wall stone pattern, 128x192, same iso slope angle, slightly different brick layout, some chipped stones, used for repetition variation."

9. **NW-Doorway:** "Diagonal stone archway opening on NW-sloping wall, 128x192, iso perspective, completely black void interior visible through arch (NO wood door, NO iron gate, NO banners, NO decor), broken stones framing arch, dramatic tall arch."

10. **NW-Torch-Alcove:** "NW diagonal wall with recessed stone alcove containing empty iron torch bracket (no flame baked — Unity 2D Light will add glow), 128x192 iso perspective, faint amber tint on stones near bracket."

11. **NE-Mid-Plain:** "Plain stone wall sloping diagonally from lower-right to upper-left (NE direction in iso view), 128x192, iso 45 degree perspective, lighting hits from upper-right side (different shadow pattern than NW), tall dramatic wall, consistent slope angle, baseline at bottom, visible side face thickness illusion."

12. **NE-Mid-Variant:** "Alternative NE diagonal wall stone pattern, 128x192, same iso slope, slight pattern variation."

13. **NE-Doorway:** "Diagonal stone archway opening on NE-sloping wall, 128x192, empty void interior, no door no decor."

14. **NE-Torch-Alcove:** "NE diagonal wall with recessed alcove + empty iron bracket, 128x192, faint amber tint."

15. **N-Corner-Vertex:** "Top corner vertex stone column where NW and NE walls meet at the top of the iso diamond room, 128x192, classic iso perspective, like a peak stone column with both wall slopes converging at the top, weathered granite, possibly slight cyan rift crack at the seam point, same height as wall mid pieces for seamless top alignment."

16. **N-Landmark:** "Top vertex dramatic SETPIECE landmark feature, 128x224 (extra 32px height for dramatic emphasis above standard wall), large iso stone archway containing a cyan rift portal/crystal at the top center of the diamond room, glowing cyan energy emanating from a central point, arcane rune fragments embedded around it, medium emissive (NOT brighter than gameplay VFX), NO banner, classic iso perspective."

17. **Pillar-Universal:** "Narrow standalone iso stone pillar, 64x192, full vertical height matching wall mid pieces, iso 3/4 perspective showing front + slight side face, square cross-section, decorative carved capital at top, slightly darker than wall pieces, weathered surface, used as seam cover between adjacent iso wall mid pieces — vertical alignment critical."

## Reference Images (her $imagegen call'a attach)

Her piece için 3 reference image base64 encode olarak:
1. Style ref combat room: `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_46 (1).png`
2. Style ref boss: `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_48 (4).png`
3. Style ref ritual: `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_48 (5).png`

## Generation Procedure (her piece için)

```
For each piece in 17_piece_spec:
  1. Compose prompt: style_lock + per_piece_additional
  2. Call $imagegen skill:
     - prompt: composed_prompt
     - size: 1024x1024 (raw)
     - reference_images: [combat, boss, ritual]
     - output: STAGING/concepts/fractured_chamber/iso_assets/raw/{name}_raw.png
  3. PIL downscale to target dim (NEAREST resampling pixel-perfect):
     - Save to STAGING/concepts/fractured_chamber/iso_assets/{name}.png
  4. Background cleanup:
     - Near-black pixels → alpha 0 (threshold-based)
     - Save final RGBA PNG
  5. Log: filename, raw size, target size, SUCCESS/FAIL
```

## Önemli Notlar

- **`$imagegen` skill USE, NOT openai.images.generate manual**
- NEAREST resampling sadece
- Background transparent
- 17 piece sequential
- Eğer $imagegen fail → error rapor, BLOCKED ver (env var fallback YAPMA)
- Iso geometry KRİTİK — eğer çıktı rectangular/top-down geliyorsa prompt'a "ABSOLUTELY 45 degree isometric, NOT top-down" agresif ekle

## Çıktı Dosyaları

```
STAGING/concepts/fractured_chamber/iso_assets/raw/
├─ iso_floor_clean_raw.png ... iso_floor_debris_raw.png (6 floor)
├─ wall_nw_mid_plain_raw.png ... wall_nw_torch_raw.png (4 NW)
├─ wall_ne_mid_plain_raw.png ... wall_ne_torch_raw.png (4 NE)
├─ wall_n_corner_raw.png
├─ wall_n_landmark_raw.png
└─ wall_pillar_universal_raw.png

STAGING/concepts/fractured_chamber/iso_assets/
├─ iso_floor_clean.png ... iso_floor_debris.png        (128×64)
├─ wall_nw_mid_plain.png ... wall_nw_torch.png        (128×192)
├─ wall_ne_mid_plain.png ... wall_ne_torch.png        (128×192)
├─ wall_n_corner.png                                  (128×192)
├─ wall_n_landmark.png                                (128×224)
└─ wall_pillar_universal.png                          (64×192)

STAGING/iso_asset_codex_qc.md
```

## QC Raporu

`STAGING/iso_asset_codex_qc.md`:

```markdown
# Iso Diamond Asset Set QC

## Setup
- $imagegen skill: WORKING / FAIL
- Reference images: ATTACHED

## Generation Results (17 piece)
| # | Piece | Raw Path | Final Path | Status |
| 1 | Iso-Floor-Clean | ... | ... | PASS/FAIL |
| ... | ... | ... | ... | ... |

## Visual QC
- [ ] TRUE iso diamond perspective (NOT top-down rectangular)
- [ ] NO banner anywhere
- [ ] Doorway içi boş (empty void interior)
- [ ] Torch alcove empty bracket (no flame baked)
- [ ] Stone palette tutarlı
- [ ] Pixel art aesthetic
- [ ] Transparent BG temiz

## Verdict
PASS / PARTIAL / BLOCKED

## Next Step
- Sub-agent'a teslim et — Unity import + iso grid setup
```

## Git Commit

```bash
git add STAGING/concepts/fractured_chamber/iso_assets STAGING/iso_asset_codex_qc.md
git commit -m "[Codex] [ISO DIAMOND] 17 piece iso asset set via \$imagegen"
```
