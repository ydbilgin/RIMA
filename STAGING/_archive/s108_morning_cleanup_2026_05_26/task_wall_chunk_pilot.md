# Task: North Wall Chunk Pilot — gpt-image-1 (Codex imagegen)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
RIMA chunk-based wall production approach'unun pilot testi. Pillar-bracketed mantığı geçerli mi, snap overlap çalışıyor mu görmek için **3 adet north wall straight chunk** üret. Sonuç PASS olursa full north facade kit batch'i sonraki adım.

**Tool:** Codex native imagegen skill (gpt-image-1 force). Python script YOK, OPENAI_API_KEY YOK. S98/S99 imagegen task'ları referans.

## Production
- **Boyut:** 128x96 px (her chunk)
- **Output klasör:** `STAGING/concepts/wall_pilot/`
- **Adet:** 3 chunk (her biri ayrı imagegen call)

---

## Kritik Stil Kurallar (HER prompt'ta zorunlu)

1. **Pillar-bracketed:** Sol 16px = vertical stone pillar + iron torch sconce + warm flame. Sağ 16px = AYNI pillar + torch column (mirror/identical). Bu seam-hider zorunlu, snap point'leri belirler.
2. **Orta 96px:** Wall content area (stone facade + decor).
3. **Açı:** Top-down 3/4 iso view (Hades / Shattered Keep camera), kameraya bakar yüz (north wall = camera-facing back wall).
4. **Stil:** Dark fantasy pixel art, painterly polish. Granite stone blocks (dark slate gray, #2A2D33). Warm torch glow (#FFB454). Cyan rift accent (#5DEFFF). Black void background (transparent OK).
5. **Lighting direction:** Torch on each pillar = warm pool light. Subtle cyan ambient from above.
6. **Negative:** NO floor visible (wall facade only), NO characters, NO grid lines, NO bright daylight, NO modern brick, NO bottom edge cut (whole wall visible top-to-bottom in 96px height).
7. **Pivot logic:** Wall bottom should sit at bottom edge of canvas (placement pivot = bottom-center).

---

## 3 Chunk Variants

### Pilot 1 — `STAGING/concepts/wall_pilot/north_straight_clean.png`
```
Top-down 3/4 isometric view pixel art, dark fantasy dungeon wall facade chunk, 128x96 canvas. 
LEFT 16px column: vertical granite stone pillar with iron torch sconce, lit warm orange flame, identical to right edge. 
CENTER 96px: smooth granite stone block wall, clean intact masonry, dark slate gray (#2A2D33) with subtle stone course rhythm, no major decor, plain wall reading. 
RIGHT 16px column: vertical granite pillar with iron torch sconce, warm flame, MIRROR of left edge (identical pillar+torch shape so two chunks placed adjacent visually merge into one pillar). 
Shattered Keep style, ancient stone shelter aesthetic. 
Warm torch glow pools at pillars, dark void background. 
Pixel art with painterly polish, sharp readable silhouette, fixed iso camera angle. 
Wall fills entire 96px canvas height from bottom to top, no floor visible, transparent or pure black background.
```

### Pilot 2 — `STAGING/concepts/wall_pilot/north_straight_banner.png`
```
Top-down 3/4 isometric view pixel art, dark fantasy dungeon wall facade chunk, 128x96 canvas. 
LEFT 16px: vertical granite pillar + iron torch sconce + warm flame (identical to right edge). 
CENTER 96px: granite stone wall with a hanging deep navy/crimson tattered banner draped from upper wall, faded heraldic emblem in gold thread, fabric showing wear and tear. 
RIGHT 16px: MIRROR pillar+torch (identical to left). 
Shattered Keep style, ancient failed shelter aesthetic. 
Warm torch glow on banner edges, subtle cyan ambient from above. 
Pixel art with painterly polish, fixed iso camera angle. 
Wall fills entire canvas height, no floor visible, no characters, transparent background.
```

### Pilot 3 — `STAGING/concepts/wall_pilot/north_straight_cracked.png`
```
Top-down 3/4 isometric view pixel art, dark fantasy dungeon wall facade chunk, 128x96 canvas. 
LEFT 16px: vertical granite pillar + iron torch + warm flame (identical to right edge). 
CENTER 96px: granite stone wall with a cyan glowing rift crack zigzagging diagonally across the masonry, magical hairline fracture #5DEFFF, faint glow bleeding from crack, surrounding stones show stress fracture. 
RIGHT 16px: MIRROR pillar+torch. 
Shattered Keep style, FAILED ward shelter aesthetic, rift energy breaking through ancient stone. 
Warm torch + cool cyan rift = warm/cool contrast. 
Pixel art with painterly polish, fixed iso camera angle. 
Wall fills entire canvas height, no floor visible, transparent background.
```

---

## Acceptance Test

3 PNG üretildikten sonra şunu CODEX_DONE.md'ye yaz:
1. 3 dosya path'i listele
2. Her chunk için: **Left pillar shape ↔ Right pillar shape** identical mi (gözle bak, mirror check)
3. 3 chunk yan yana mental olarak konunca pillar overlap mantıklı mı görünüyor (her chunk'ın sağ pillar'ı + sonraki chunk'ın sol pillar'ı = single pillar gibi okunuyor mu)
4. Stil consistency: 3 chunk arasında lighting direction + stone tone + torch glow eşleşiyor mu

## Kullanıcı sonra ne yapacak
- 3 PNG'yi Unity'ye import edip overlap snap test
- Karakter Warblade'in yanına koyup scale check
- Stil tutarlıysa → full north facade kit batch (12 sprite)
- Stil tutmuyorsa → PixelLab img2img pass eklenir

## Output → CODEX_DONE.md
- 3 PNG dosya yolu
- Acceptance test cevapları (4 madde)
- Hata varsa hangi prompt failed
