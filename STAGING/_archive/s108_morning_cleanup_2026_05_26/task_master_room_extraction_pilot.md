# Task: Master Extraction Room — Codex imagegen (gpt-image-1)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
"Room-first extraction" pipeline pilot — full empty dungeon room üret, sonra chunk extraction phase'e geçilecek. Paralel test: user PixelLab web UI'da da aynı odayı üretiyor, ikisinin output'u karşılaştırılacak.

**Tool:** Codex native imagegen skill (gpt-image-1 force). Python script YOK, OPENAI_API_KEY YOK. S98/S99/pilot referans.

## Output
- **Dosya:** `STAGING/concepts/master_room_pilot/room_v1_gptimage.png`
- **Boyut:** 1024x1024 (generation native res, downsample later to logical 512x512)

## Reference / Style Anchor
Bunları imagegen call'ında reference olarak attach et:
- `STAGING/concepts/chatgpt ref/ChatGPT Image 22 May 2026 16_12_46 (1).png` (main layout)
- `STAGING/concepts/chatgpt ref/ChatGPT Image 22 May 2026 16_12_48 (5).png` (ritual chamber camera)
- `STAGING/concepts/chatgpt ref/ChatGPT Image 22 May 2026 16_12_48 (7).png` (ornate decor)
- `STAGING/concepts/chatgpt ref/ChatGPT Image 22 May 2026 16_12_49 (8).png` (ruined cutaway)
- `STAGING/concepts/wall_pilot/north_straight_clean.png` (pilot wall style)

---

## Prompt

```
Dark fantasy dungeon empty extraction room, top-down 3/4 ARPG isometric view, 1024x1024 canvas. 
Shattered Keep style: cracked charcoal granite walls, warm torch glow, subtle cyan rift hairline cracks, dark moody lighting matching the reference images.

LAYOUT:
- TOP edge (BACK wall, camera-facing back): horizontal stone wall run, full height, vertical stone pillars and iron torch sconces with warm flame, one tattered deep navy banner hanging from upper section, one arched stone alcove with a small hooded statue inside niche.
- LEFT edge (SIDE wall, camera-facing left): vertical stone wall run, full height, stone pillars with iron torch sconces, one cyan rift hairline crack on stone.
- BACK-LEFT CORNER: tall stone pillar joining the two walls with iron torch sconce on outer face.
- BACK wall has one OPEN ARCHED DOORWAY opening in middle section, dark passage hint visible beyond arch.
- BOTTOM edge (camera-front): NOT pure void — series of knee-height ruined stone column stubs, scattered broken stones, low rubble debris cluster fading into darkness. Cutaway dollhouse style.
- RIGHT edge (camera-right): NOT pure void — broken wall fragment stubs, rubble pile, fading darkness, occasional torch on a remaining stub.
- INTERIOR FLOOR: large granite stone slab tiles visible from above with subtle joint lines and slight color variation, faint cyan rift hairline cracks zigzagging across floor in 2-3 spots (small magical fractures, not floor-breaking).
- 2 minimal props placed in interior space: one stone brazier with burning warm fire approximately one-third in from BACK wall, one small candle cluster near BACK-LEFT corner.

NO characters, NO enemies, NO player, NO UI overlay, NO health bars, NO text, NO labels, NO minimap. 
Fixed 3/4 top-down ARPG camera angle (~75-80 degrees from horizontal, matching reference images). 
Pixel art aesthetic with painterly polish matching reference style. 
Transparent or pure black outside the room footprint. 
Empty extraction room — designed so wall chunks (back wall sections, left wall sections, corner, doorway, cutaway stubs, props) can be cleanly cropped out as modular sprites.
```

---

## Acceptance Test (CODEX_DONE.md'ye yaz)

1. **File:** `STAGING/concepts/master_room_pilot/room_v1_gptimage.png` mevcut mı?
2. **Layout check:**
   - BACK wall var mı (top edge, full height, pillars + torches + banner + alcove statue)?
   - LEFT wall var mı (left edge, full height, pillars + torches)?
   - BACK-LEFT corner clear mı?
   - Doorway opening BACK wall'da görünüyor mu?
   - BOTTOM edge ruined stubs (NOT void) mu?
   - RIGHT edge ruined stubs mu?
   - Floor granite + cyan cracks mı?
   - 2 props (brazier + candles) interior'da mı?
3. **Style check:** Reference image'lardaki dark moody Shattered Keep palette tutuyor mu?
4. **Extraction readiness:** Wall chunks cleanly cropable mı görünüyor (overlap olmadan)?
5. **Disqualifiers:** Character/UI/text varsa FAIL — re-run gerekir
6. **Reference attach:** 5 reference image gerçekten kullanıldı mı (yes/no)

## Output → CODEX_DONE.md
- PNG path
- Acceptance test 6 madde cevap
- Hata varsa not
