# PixelLab Create Image Pro (V3 Web UI) — RIMA Style Prompts

Copy-paste these into PixelLab web UI → Create Image Pro tab. Each prompt has the **Negative Prompt inline at the end** (PixelLab V3 format — no separate negative field).

## Web UI settings (her promptta aynı)

- **Size:** 512x512 (max)
- **Style:** None / Default
- **Tile strength:** Varies per prompt (seamless tiles → high; sprites → low)
- **Detail:** Medium
- **Outline:** Lineless (kritik — border'sız ister)
- **Shading:** Flat shading (basic shading for sprites)
- **Style reference image:** Optional — verirsek mevcut granit tile referans olur

---

## PROMPT 1 — Cool Granite Floor (SEAMLESS, PRIMARY)

**Size:** 512x512
**Tile strength:** Maximum (seamless gerek)
**Style ref:** None

```
Cool weathered granite floor surface, completely seamless top-down ground texture, chunky 32-pixel-scale painterly pixel art, continuous monolithic stone surface, hand-drawn brush feel, muted cool gray with subtle blue-violet undertones, sparse hairline natural cracks scattered randomly across the whole texture, tiny mineral speckles, top-down perfectly flat view, no shading direction, ancient temple foundation, worn

Negative Prompt : tile border, frame, outline, dark edge pixels, cobblestone, brick, mortar, grid, individual stones, mosaic, masonry, slab, paving, geometric pattern, repeating grid lines, square boundary, watermark, text, logo, smooth gradient, anti-aliasing, 3d render, perspective, depth, shadow direction
```

---

## PROMPT 2 — Worn Stone Path (SEAMLESS, secondary floor)

**Size:** 512x512
**Tile strength:** Maximum
**Style ref:** None (or PROMPT 1 result for palette consistency)

```
Worn stone path ground surface, completely seamless top-down texture, chunky 32-pixel-scale painterly pixel art, rounded river-stones embedded in dark mud, soft-edged organic stone placement varied size, ancient walked-on path, pale grey-brown with darker mud gaps, top-down perfectly flat view, no shading direction

Negative Prompt : tile border, frame, outline, cobblestone, brick, mortar, grid, geometric pattern, watermark, text, logo, smooth gradient, anti-aliasing, 3d render, perspective, depth, shadow direction, repeating grid lines, square boundary
```

---

## PROMPT 3 — Wall Cap (Hades-style 35° perimeter sprite)

**Size:** 256x128 (rectangular sprite, NOT seamless tile)
**Tile strength:** LOW (tile_strength: 0.2)
**Style ref:** PROMPT 1 result (palette consistency)

```
Top-down dungeon wall section at slight 30-degree angle showing visible height, cool weathered granite blocks construction, painterly hand-drawn 32-pixel-scale pixel art, horizontal wall sprite for room perimeter, subtle moss and dust at base of wall where it meets floor, ancient temple stone wall, top of wall slightly in front of base in screen space, muted cool gray palette with blue-violet undertones, sparse tiny cracks

Negative Prompt : border around image, frame, outline, watermark, text, logo, smooth gradient, anti-aliasing, 3d render, full 3D perspective, vertical wall straight elevation, brick grid, mortar, geometric pattern, repeating units
```

---

## PROMPT 4 — Gate Arch (granite-matching, vertical sprite)

**Size:** 128x192 (portrait sprite)
**Tile strength:** LOW (tile_strength: 0.2)
**Style ref:** PROMPT 1 result

```
Dungeon gate archway sprite for top-down game, cool weathered granite stone arch frame, painterly hand-drawn 32-pixel-scale pixel art, ancient temple stone block construction, iron-banded dark wood gate panels closed inside the arch opening, subtle cyan and violet rift light hint at edge of dark interior void, slight moss at base of arch where it meets floor, top-down view at 30-degree angle showing arch from front, muted cool gray palette with blue-violet undertones

Negative Prompt : border around image, frame around sprite, outline around sprite, watermark, text, logo, smooth gradient, anti-aliasing, 3d render, full 3D perspective, modern gate, clean perfect construction, brick grid, mortar, geometric pattern
```

---

## PROMPT 5 — L4 Organic Moss Patch (overlay)

**Size:** 128x128 (transparent background)
**Tile strength:** LOW (tile_strength: 0.2)
**Style ref:** PROMPT 1 result for palette

```
Organic irregular moss patch decal for top-down game floor overlay, painterly hand-drawn 32-pixel-scale pixel art, cool cave moss color #5A6B5A center fading to darker #2A4520 spots, irregular blob shape NOT circle NOT square, soft alpha feathered edges blending into surrounding floor, small moss tufts standing slightly proud, transparent background outside the moss blob, top-down perfectly flat view, NOT plant illustration

Negative Prompt : border around image, frame, outline, hard cutoff edge, opaque background, white background, watermark, text, logo, smooth gradient, anti-aliasing, 3d render, perspective, flower, plant illustration, full plant
```

---

## PROMPT 6 — L4 Dust Drift Patch (overlay)

**Size:** 128x128 (transparent background)
**Tile strength:** LOW (tile_strength: 0.2)
**Style ref:** PROMPT 1 result

```
Organic irregular dust drift patch decal for top-down stone floor overlay, painterly hand-drawn 32-pixel-scale pixel art, warm pale dust color #6A5C48 fading to #7C6A55 highlight spots, irregular wind-blown shape, soft alpha feathered edges blending into surrounding floor, tiny tiny pebbles scattered in the dust, transparent background outside the patch, top-down perfectly flat view

Negative Prompt : border around image, frame, outline, hard cutoff edge, opaque background, white background, watermark, text, logo, smooth gradient, anti-aliasing, 3d render, perspective, sandy desert, full ground texture
```

---

## PROMPT 7 — L5 Floor Scatter Stones (mini-prop set)

**Size:** 256x256 (4 stones in 2x2 grid, transparent bg)
**Tile strength:** LOW (tile_strength: 0.1)
**Style ref:** PROMPT 1 result

```
Four small floor scatter stones arranged in 2x2 grid on transparent background, top-down view, painterly hand-drawn 32-pixel-scale pixel art, ancient temple debris pebbles, cool gray granite stones varied shapes, subtle light catch on top of each stone, transparent background between stones, each stone roughly 30-50 pixels in size, painterly NOT realistic

Negative Prompt : border around image, frame, outline, hard cutoff edge, opaque background, white background, watermark, text, logo, smooth gradient, anti-aliasing, 3d render, perspective, single large stone, photorealistic, pile of stones
```

---

## Production sequence (önerim)

| Sıra | Prompt | Cost | Why first |
|---|---|---|---|
| 1 | **PROMPT 1 Granite Floor** | ~10-25 gen | Foundation — diğer hepsi bunun üstüne kurulur |
| 2 | PROMPT 3 Wall Cap | ~10 gen | Floor net olunca walls match'lensin |
| 3 | PROMPT 4 Gate Arch | ~10 gen | Walls + gates aynı palette |
| 4 | PROMPT 2 Stone Path | ~10-25 gen | Second floor material |
| 5 | PROMPT 5 Moss Patch | ~10 gen | L4 overlay |
| 6 | PROMPT 6 Dust Patch | ~10 gen | L4 overlay variant |
| 7 | PROMPT 7 Scatter Stones | ~10 gen | L5 detail |

**Toplam:** ~90-120 gen / kalan 4250.

## Each PNG geldikten sonra workflow

1. Web UI'da "Download" tıkla
2. PNG'yi shown save path'e drop et
3. Bana de "PROMPT N geldi" → ben Unity'de import + Tilemap repaint yaparım
4. Sahnede sonucu birlikte değerlendiririz
5. Beğenmediysek PROMPT iterasyon (tile_strength yükselt, color tweak, vs)

## Style reference image öğrenme — kritik trick

**PROMPT 1 geldiğinde:** İlk granite floor PNG'yi PROMPT 3, 4, 5, 6, 7'ye **style reference image** olarak yükle. Bu PixelLab'in palette + brush feel'i sonraki prompt'lara taşımasını sağlar. Tek prompt'la palette match çok zor — style reference image ile çok daha tutarlı olur.

## Quality check (yasak kelimeler dikkat et)

ChatGPT'ye yazdığım yasakların aynısı PixelLab için de geçerli:
- "tile" → "texture" / "surface"
- "cobblestone" / "brick" → "continuous painterly surface"
- "bright" / "vibrant" → "muted, desaturated, cool"

Memory'de: `feedback_pixellab_no_dark_fantasy.md` — "dark fantasy" / "grimdark" YASAK. Tarz tanımla, isimlendirme.

---

İlk PROMPT 1'i Create Image Pro web UI'da çalıştır. PNG geldiğinde paylaş, Unity'ye atıp Tilemap'i repaint ederim. Visual quality OK ise diğer prompt'lara devam, değilse Prompt 1 iterasyonu.
