# Task: V-Shape Wall-Only Reference Sheet — Codex imagegen

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
RIMA wall extraction için **sadece duvar** reference sheet üret — V-shape (NW + NE diagonals), 4'er section'lı, pillar boundaries ile, 2 kapı variety. Floor + prop YOK. Sonra bu sheet'ten 8 wall chunk + 1 corner extract edilecek.

**Tool:** Codex native imagegen skill (gpt-image-1 force). Python script YOK. S98/S99/pilot referans.

## Output
- **Dosya:** `STAGING/concepts/wall_kit_v2/v_shape_wall_sheet.png`
- **Boyut:** 1024x1024

## Reference / Style Anchor
Bunları imagegen call'ında reference olarak attach et:
- `STAGING/concepts/wall_pilot/north_straight_clean.png` (pilot wall style)
- `STAGING/concepts/wall_pilot/north_straight_banner.png`
- `STAGING/concepts/wall_pilot/north_straight_cracked.png`
- `STAGING/concepts/master_room_pilot/room_v1_gptimage.png` (master room style)
- `STAGING/concepts/chatgpt ref/ChatGPT Image 22 May 2026 16_12_46 (1).png` (target visual)

---

## Prompt

```
Dark fantasy dungeon WALL-ONLY reference sheet, 1024x1024 canvas, top-down 3/4 ARPG isometric view, Shattered Keep style. 
The image shows a V-shape composed of two long diagonal walls meeting at top-center, with NO floor, NO props, NO characters, NO ground — only the walls floating with transparent or pure black background.

V-SHAPE GEOMETRY:
- LEFT WALL runs from TOP-CENTER (~512, 80) diagonally down to BOTTOM-LEFT (~80, 600), forming the NW-SE diagonal facade.
- RIGHT WALL runs from TOP-CENTER (~512, 80) diagonally down to BOTTOM-RIGHT (~944, 600), forming the NE-SW diagonal facade.
- V-JUNCTION at top-center: tall vertical granite pillar with iron torch sconce + warm flame, joining the two wall runs.

LEFT WALL (NW diagonal) — 4 sections divided by IDENTICAL vertical stone pillars with iron torch sconces (pillars are the natural cut/seam boundaries):
- Section L1 (near top-junction): CLEAN straight granite stone wall, intact masonry, plain
- Section L2: granite wall with TATTERED DEEP NAVY BANNER hanging from upper edge, faded heraldic emblem, fabric torn
- Section L3: granite wall with IRON-BANDED WOODEN DOORWAY, arched opening, rusted hinges, dark passage hint visible through door gap, warm light leak at threshold
- Section L4 (near bottom-left end): granite wall with CYAN GLOWING RIFT CRACK zigzagging across stones, magical hairline fracture #5DEFFF, faint glow

RIGHT WALL (NE diagonal) — 4 sections divided by IDENTICAL pillars:
- Section R1 (near top-junction): CLEAN straight granite stone wall, intact masonry, plain
- Section R2: granite wall with DEEP ALCOVE NICHE carved into wall, weathered HOODED STONE STATUE standing inside niche, faint warm candlelight at statue base, shadow depth in alcove
- Section R3: granite wall with OPEN ARCHWAY passageway (no door, no wood), cut stone arch with keystone showing faint cyan rift inset, deep black darkness beyond arch
- Section R4 (near bottom-right end): granite wall partially COLLAPSED with rubble pile at base, jagged broken stones at top, cyan rift glow bleeding from cracks where structure failed

STYLE CONSISTENCY RULES (HARD):
- Same wall height across all sections
- Same top-cap stone thickness across all sections
- Same pillar width (each pillar identical — repeating motif for modular cutting)
- Same torch height + warm flame color
- Same stone-course rhythm (visible mortar lines align across sections)
- Same lighting direction (warm torch pools + cool ambient from above)
- Consistent 3/4 iso perspective throughout (NO tilt drift between sections)
- Pillars are spaced so each wall section is approximately the same width (~150-180 pixels diagonal projection)

PALETTE:
- Granite stone: charcoal #2A2D33 with darker mortar
- Top cap stones: slightly lighter slate gray
- Torch glow: warm orange #FFB454
- Cyan rift accents: #5DEFFF
- Banner fabric: deep navy with gold thread emblem
- Dark void: pure black background outside walls

CONSTRAINTS:
- NO FLOOR visible (no granite slabs, no ground tile)
- NO PROPS in front of walls (no brazier, no candle on floor, no rubble piles on ground)
- NO CHARACTERS, NO ENEMIES
- NO UI, NO TEXT, NO LABELS, NO grid numbers
- ONLY the V-shape wall structure floating on black/transparent background
- Each pillar must be IDENTICAL in shape/size/torch position (mirror across all 8 pillars used as section dividers + the V-junction central pillar) — this is for modular extraction

The image is designed so individual wall sections can be cleanly cropped along the pillar boundaries and used as modular wall pieces in a 2D top-down dungeon. Walls should look like they could be extracted and tiled in Unity without seam issues.

Pixel art aesthetic with painterly polish matching reference images. Production-quality asset reference.
```

---

## Acceptance Test (CODEX_DONE.md)

1. **File:** `STAGING/concepts/wall_kit_v2/v_shape_wall_sheet.png` mevcut + 1024x1024 mı?
2. **V-shape geometry:**
   - LEFT wall NW diagonal var mı?
   - RIGHT wall NE diagonal var mı?
   - V-junction top-center'da pillar var mı?
3. **LEFT wall 4 section:** L1 clean / L2 banner / L3 wooden door / L4 cyan crack — hepsi var mı?
4. **RIGHT wall 4 section:** R1 clean / R2 alcove statue / R3 open archway / R4 broken — hepsi var mı?
5. **Pillar consistency:** Tüm pillar'lar identical (boy, genişlik, torch position) mı? Section'lar arası geçişte tutar mı?
6. **Constraints:** Floor YOK, prop YOK, character YOK, text YOK doğrulandı mı?
7. **Style consistency:** Wall height + top-cap + stone course + lighting direction tüm sections'ta sabit mi?
8. **Reference attach:** 5 reference image gerçekten kullanıldı mı?

## Output → CODEX_DONE.md
- PNG path
- Acceptance test 8 madde cevap
- Notlar (bir şey eksik veya tutarsız ise)
