# Task: North + West Wall Kit Batch + Room Mockup — Codex imagegen

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
Pilot 3 chunk PASS sonrası full wall kit production. North + West facade chunks + corners + 1 composed room mockup. Hedef visual: `STAGING/concepts/chatgpt ref/ChatGPT Image 22 May 2026 16_12_46 (1).png` (Shattered Keep ARPG screenshot — girintili çıkıntılı oda layout reference).

**Tool:** Codex native imagegen skill (gpt-image-1 force). Python script YOK. S98/S99/pilot referans.

## Style Anchor (HER prompt'a attach)
Pilot 3 chunk style lock için reference:
- `STAGING/concepts/wall_pilot/north_straight_clean.png`
- `STAGING/concepts/wall_pilot/north_straight_banner.png`
- `STAGING/concepts/wall_pilot/north_straight_cracked.png`

Ek hedef visual:
- `STAGING/concepts/chatgpt ref/ChatGPT Image 22 May 2026 16_12_46 (1).png`

Output klasör: `STAGING/concepts/wall_kit_v1/`

---

## Kritik Stil Kurallar (ALL prompts zorunlu)

1. **Pillar-bracketed:** Sol 16px + sağ 16px = vertical granite stone pillar + iron torch sconce + warm flame. Identical mirror.
2. **Stil consistency:** Pilot 3 chunk'la AYNI palet (granite #2A2D33, torch #FFB454, cyan #5DEFFF). Aynı painterly polish, aynı camera angle.
3. **NEGATIVE:** NO text, NO labels, NO floor visible, NO characters, NO bright daylight, NO modern brick, NO grid lines.
4. **Pivot:** wall bottom = canvas bottom edge.
5. **Background:** transparent veya pure black.

---

## NORTH SET (camera-facing back wall — top of view)

Canvas: **128x96** (Hero piece 256x96)

### N1 — `wall_kit_v1/north_alcove_statue.png` (128x96)
```
Top-down 3/4 iso view, dark fantasy stone wall facade chunk 128x96. 
LEFT 16px + RIGHT 16px: identical mirror granite pillar with iron torch sconce + warm flame.
CENTER 96px: granite stone wall with deep alcove niche carved into center, weathered hooded stone statue standing inside niche, faint warm candlelight at statue base, shadow depth in alcove. 
Shattered Keep failed shelter aesthetic. Pixel art painterly polish, transparent BG.
```

### N2 — `wall_kit_v1/north_doorway.png` (128x96)
```
Top-down 3/4 iso, stone wall chunk 128x96. 
LEFT+RIGHT 16px: mirror pillar+torch.
CENTER 96px: arched doorway opening with iron-banded wooden door, rusted hinges, heraldic emblem on door, warm light bleeding from below door gap, dark passage hint visible through cracks. 
Shattered Keep style. Pixel art painterly polish, transparent BG.
```

### N3 — `wall_kit_v1/north_archway_open.png` (128x96)
```
Top-down 3/4 iso, stone wall chunk 128x96. 
LEFT+RIGHT 16px: mirror pillar+torch.
CENTER 96px: open arched passageway, no door, cut stone arch with keystone showing faint cyan rift inset, deep black darkness beyond arch (passage extends backward), subtle warm torch light at threshold. 
Shattered Keep style. Pixel art painterly polish, transparent BG.
```

### N4 — `wall_kit_v1/north_broken_collapse.png` (128x96)
```
Top-down 3/4 iso, ruined stone wall chunk 128x96. 
LEFT 16px: intact pillar+torch (still standing).
RIGHT 16px: damaged pillar, half-collapsed, torch broken/dim.
CENTER 96px: stone wall partially collapsed, rubble pile at base, jagged broken stones at top, cyan rift glow bleeding from cracks where structure failed, exposed inner stonework. 
Shattered Keep FAILED ward aesthetic. Pixel art painterly polish, transparent BG.
```

### N5 — `wall_kit_v1/north_dense_torches.png` (128x96)
```
Top-down 3/4 iso, stone wall chunk 128x96. 
LEFT+RIGHT 16px: mirror pillar+torch (standard).
CENTER 96px: granite wall with 2 additional iron wall-mounted torch sconces evenly spaced, warm flame pools, heraldic shield mounted between torches, rich warm ambient illumination. 
Hero-room vibe, ceremonial. Pixel art painterly polish, transparent BG.
```

### N6 — `wall_kit_v1/north_hero_long.png` (256x96)
```
Top-down 3/4 iso, EXTRA LONG stone wall chunk 256x96. 
LEFT 16px + RIGHT 16px: mirror pillar+torch.
CENTER 224px: ceremonial wide stone facade — central tall arched alcove with hanging deep navy tattered banner showing rift-touched emblem, flanking pair of mounted iron torch sconces, cyan rift hairline crack zigzagging across stones near alcove, chain hanging down from upper-left edge. 
Boss room / hero wall reading. Pixel art painterly polish, transparent BG.
```

---

## WEST SET (camera-facing left side wall — left edge of view)

**IMPORTANT:** West is NOT North rotated 90°. Separately authored — viewer sees wall fronting them from the LEFT side, top-cap stones shown in side-profile, lighting hits from camera-right.

Canvas: **96x128** (vertical orientation — taller than wide)

### W1 — `wall_kit_v1/west_straight_clean.png` (96x128)
```
Top-down 3/4 iso view, dark fantasy stone wall facade chunk 96x128 — WEST FACING wall (camera-facing right, left-side room wall). 
TOP 16px + BOTTOM 16px: identical mirror horizontal granite pillar segment with iron torch sconce + warm flame on the camera-facing side.
CENTER 96px: granite stone block wall, clean intact masonry, dark slate gray, top-cap stones shown in side profile (depth visible). 
Shattered Keep style. Pixel art painterly polish, transparent BG. Wall fills canvas, no floor visible.
```

### W2 — `wall_kit_v1/west_straight_banner.png` (96x128)
```
Top-down 3/4 iso WEST FACING wall 96x128. 
TOP+BOTTOM 16px: mirror horizontal pillar+torch.
CENTER 96px: granite wall with hanging deep crimson/navy tattered banner draped from upper edge, faded heraldic emblem, fabric showing tears, side-profile top cap. 
Shattered Keep style. Pixel art, transparent BG.
```

### W3 — `wall_kit_v1/west_straight_cracked.png` (96x128)
```
Top-down 3/4 iso WEST FACING wall 96x128. 
TOP+BOTTOM 16px: mirror pillar+torch.
CENTER 96px: granite wall with cyan glowing rift crack zigzagging vertically, magical hairline fracture #5DEFFF, faint glow bleeding from crack, surrounding stress fractures, side-profile top cap. 
Warm torch + cool cyan contrast. Pixel art, transparent BG.
```

### W4 — `wall_kit_v1/west_alcove_statue.png` (96x128)
```
Top-down 3/4 iso WEST FACING wall 96x128. 
TOP+BOTTOM 16px: mirror pillar+torch.
CENTER 96px: granite wall with deep alcove niche carved in, weathered hooded stone statue inside, faint candle base glow, shadow depth in alcove, side-profile top cap. 
Shattered Keep style. Pixel art, transparent BG.
```

### W5 — `wall_kit_v1/west_doorway.png` (96x128)
```
Top-down 3/4 iso WEST FACING wall 96x128. 
TOP+BOTTOM 16px: mirror pillar+torch.
CENTER 96px: arched stone doorway with iron-banded wooden door, rusted hinges, warm light leak at threshold, side-profile top cap with keystone visible. 
Shattered Keep style. Pixel art, transparent BG.
```

### W6 — `wall_kit_v1/west_broken_collapse.png` (96x128)
```
Top-down 3/4 iso WEST FACING ruined wall chunk 96x128. 
TOP 16px: intact pillar+torch. 
BOTTOM 16px: damaged pillar, half-collapsed.
CENTER 96px: stone wall partially collapsed, rubble pile, jagged broken top, cyan rift glow from cracks, exposed inner stones, side-profile broken cap. 
Shattered Keep failed structure. Pixel art, transparent BG.
```

---

## CORNERS

### C1 — `wall_kit_v1/corner_NW_inner.png` (128x128)
```
Top-down 3/4 iso INNER CORNER piece 128x128 where NORTH wall meets WEST wall in upper-left of room. 
North wall extends from corner toward right (camera-facing back), West wall extends from corner downward (camera-facing right). 
Tall granite pillar at exact corner junction with mounted iron torch sconce, warm flame, slight cyan rift hairline near base where two wall faces meet. 
Top-cap stones show realistic 3D corner geometry (depth). 
Shattered Keep style. Pixel art painterly polish, transparent BG.
```

### C2 — `wall_kit_v1/corner_NE_outer.png` (96x128)
```
Top-down 3/4 iso OUTER CORNER piece 96x128 where NORTH wall ends and turns inward (creates a protrusion sticking out into room). 
Tall granite pillar at corner, iron torch sconce on outer face, north wall stub extending left, side return surface visible going down-right. 
Top-cap stones show outer-corner geometry. 
Shattered Keep style. Pixel art, transparent BG.
```

### C3 — `wall_kit_v1/corner_alcove_protrusion.png` (128x128)
```
Top-down 3/4 iso ALCOVE OUTER CORNER cluster 128x128 — used when an alcove or buttress juts INTO the room from a facade. 
Two outer corners back-to-back creating a protruding rectangular bay, granite pillar caps at both visible corners, torch sconces, banner hanging on protruding face, top-cap shows protrusion geometry from above. 
Irregular layout enabler. Shattered Keep style. Pixel art, transparent BG.
```

### C4 — `wall_kit_v1/corner_SW_end_void.png` (96x128)
```
Top-down 3/4 iso WEST WALL END piece 96x128 where the west wall ends at the bottom (camera-facing south edge fades to black void). 
Granite pillar termination, last torch sconce, wall structurally tapers/fades into darkness at bottom, rubble at base, cyan rift hint, void darkness encroaches. 
Shattered Keep boundary aesthetic. Pixel art, transparent BG.
```

---

## ROOM MOCKUP (extra deliverable)

### R1 — `wall_kit_v1/room_mockup_v1.png` (1280x800)
```
Top-down 3/4 iso view, irregular shaped Shattered Keep dungeon room composition study, 1280x800. 
TOP edge (camera-back): assembled north facade chunks — straight, banner, alcove statue, doorway, broken collapsed. Pieces visually connect via mirrored pillar+torch columns at chunk boundaries. 
LEFT edge (camera-left): assembled west facade chunks — straight, cracked rift, alcove, broken collapsed. Pieces connect at horizontal pillar segments. 
NW upper-left CORNER: tall stone pillar joining north and west walls. 
Room has irregular outline — one alcove juts INTO room from north wall (statue niche bay), one buttress protrudes inward from west wall. 
INTERIOR FLOOR: granite stone slab tiles with subtle joint lines, cyan rift hairline cracks zigzagging across floor (small magical fractures, not floor-breaking), warm ambient pools at torches, cooler shadow center. 
PROPS placed in room: stone brazier with burning fire near center-left, scattered candles/altar on small table top-right area, broken column stub mid-floor, hanging chain prop from upper edge, skull pile in one corner, tattered banner on west wall. 
BOTTOM edge (camera-front): floor stones taper, fade to pure black void — NO south wall. 
RIGHT edge (camera-right): floor stones taper, fade to pure black void — NO east wall. 
Dark moody lighting, warm torch pools + cool cyan rift accent, deep ambient darkness in negative space. 
NO characters, NO HUD, NO labels. Pixel art painterly polish, fixed 3/4 iso camera, granite + cyan + warm torch palette. 
Reference vibe: ARPG dungeon room screenshot, irregular layout reading natural and inhabited, multiple readable sub-spaces within one room. 
Production-quality concept art, transparent or pure black background outside the room footprint.
```

---

## Acceptance Test (CODEX_DONE.md'ye yaz)

1. **File output:** Tüm 15 PNG üretildi mi (N1-N6 + W1-W6 + C1-C4 + R1)
2. **Style match:** Yeni chunk'lar pilot 3'le stil olarak tutarlı mı (granite tone, torch glow, painterly polish)
3. **Pillar mirror:** N1-N5 ve W1-W6 chunklarda left/right pillar identical mi (mirror check)
4. **West vs North fark:** West chunk'lar North'tan görsel olarak ayrı authored mi (lighting direction, top-cap profile farklı mı)
5. **Room mockup:** R1'de chunk'lar visually birleşmiş mi (seam'ler okunabilir görünüyor mu yoksa pillar overlap mantığı işliyor mu)
6. **Failures:** Hangi prompt failed varsa not düş

## Output → CODEX_DONE.md
- 15 PNG path listesi
- Acceptance test 5 madde cevap
- Style anchor reference image'ları gerçekten kullanıldı mı (yes/no)
