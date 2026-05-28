ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Codex Task — A6 Backwall 8-piece Modular Mockup (gpt-image-1 ile)

**Amaç:** RIMA Fractured Chamber için 8-piece modüler duvar sistemi mockup'larını **Codex imagegen (gpt-image-1)** ile üret. ChatGPT reference görsellerini stil/composition kaynağı olarak kullan. Çıktılar PixelLab S-XL "new" init image olarak kullanılacak (user 8 credit ile pixel art versiyonlarını üretecek).

User decision (LOCK):
- **Pillar seam-cover strategy** — wall mid pieces arasına pillar konulur, seam'leri gizler. Bu modülerliği "free composition" yapar (mid'ler doğrudan tile etmek zorunda değil).
- **2 yön:** North wall (back, kameraya bakar) + West wall (yan profil). RIMA top-down 70-80° → "isometric" RIMA terminolojide 3/4 view top-down anlamına gelir, true iso 30/45° DEĞİL.
- **Boyutlar:** North 128×192, West 96×192, Pillar 64×192, Doorway 128×192
- **Element grammar:** wall, pillar, doorway, banner, torch socket, rift portal landmark

## Reference görseller (Codex AÇMALI ve incelemelidir)

### chatgpt_ref/ ana klasör (ilk 6, full game mockup'lar)
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_46 (1).png` — combat room, stone arches, banners, torches, cyan rift center
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_47 (2).png` — combat room v2, bookshelf side + ornate backwall + banner
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_48 (4).png` — boss room, dramatic rift portal arch + stone columns + banners
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_48 (5).png` — ritual chamber, cyan crystal altar, multi-tier platform
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_48 (6).png` — library room, dense bookshelves + scattered debris

(16_12_47 (3).png UI screen, SKIP)

### chatgpt_ref/new_chatgpt/ (önceki 6, kompakt render örnekleri)
- `ChatGPT Image 23 May 2026 21_11_22.png` — Duvar yerine alternative methods spec sheet
- `ChatGPT Image 23 May 2026 21_29_22 (1).png` — combat room fractured chamber
- `ChatGPT Image 23 May 2026 21_29_29.png` — RIMA modüler backwall sistem spec (16-piece grammar reference)

Ortak stil çıkarımı (bunlar Codex'in concept üretiminde lock olacak):
- Charcoal fractured granite stone
- Cyan rift cracks (subtle, hairline)
- Amber torch highlights (local, restrained)
- Kırmızı banner accents (dramatic)
- Dark dungeon mood, low ambient
- Stone arches recurring element
- Top-down 70-80° from horizon, subtle 3/4 perspective (NOT true iso)
- Visible top cap + side face on wall pieces (3/4 thickness illusion)
- Baseline alignment kritik (all pieces same bottom Y)

## 8 Piece Spec (gpt-image-1 her biri için 1 concept üret)

Her piece için Codex:
1. gpt-image-1 ile 1024×1024 detailed illustrative concept gen (reference imageları "style guide" olarak)
2. PIL ile target dimension'a resize/crop (nearest neighbor, no anti-alias için)
3. Background transparent yap (siyah/temiz BG'yi alpha 0'a çevir)
4. Output: `STAGING/concepts/fractured_chamber/wall_mockups/{piece_name}.png`

### Piece 1: North Wall Mid — Plain (Filler)
- Boyut: 128×192
- Composition: temiz charcoal granite wall section, full height kapalı, no opening, no decor, repeatable filler
- Stil: chatgpt_ref görsellerin plain wall sections, no banner no torch
- Output: `wall_north_mid_plain.png`

### Piece 2: North Wall Mid — Banner (Kırmızı Hanging)
- Boyut: 128×192
- Composition: aynı plain wall + üst tarafta hanging kırmızı banner (yarı uzun, RIMA Shattered Keep identity rengi)
- Banner: faded rouge red, slightly tattered edges, suspended from upper wall
- Stil: chatgpt_ref görsel 1 ve 4'teki banner pattern
- Output: `wall_north_mid_banner.png`

### Piece 3: North Wall Mid — Torch Socket (Amber Alcove)
- Boyut: 128×192
- Composition: wall mid + recessed alcove with empty torch bracket (NO flame baked, Unity 2D Light overlay yapacak)
- Alcove: stone niche, slight darker interior
- Stil: chatgpt_ref görsel 1'deki torch placement
- Output: `wall_north_mid_torch.png`

### Piece 4: North Doorway (Stone Arch + Black Void)
- Boyut: 128×192
- Composition: kapalı stone archway, dark void interior visible through opening (NO wood door), broken/cracked stones around arch
- Stil: chatgpt_ref görsel 4'teki dramatic arch + new_chatgpt 21_29_29 doorway spec
- Output: `wall_north_doorway.png`

### Piece 5: North Center Landmark — Cyan Rift Portal
- Boyut: 128×192
- Composition: large dramatic stone arch with cyan rift portal center (glowing cyan crystal/portal, medium emissive)
- Stil: chatgpt_ref görsel 4 (boss arch) + görsel 5 (ritual crystal) hybrid
- IMPORTANT: emissive medium (gameplay telegraph emissive'den DÜŞÜK, base decor seviye)
- Output: `wall_north_center_rift_portal.png`

### Piece 6: Pillar — Seam Cover (Universal)
- Boyut: 64×192
- Composition: standalone broken granite pillar, full height, 3/4 perspective showing front face + slight side face, chipped/damaged stone, cyan hairline cracks
- Designed to be placed between wall mid pieces as seam cover + decorative break
- Stil: chatgpt_ref görsel 4'teki stone columns + ruined pillar look
- Output: `wall_pillar_universal.png`

### Piece 7: West Wall Mid — Side Profile
- Boyut: 96×192
- Composition: side-facing wall section, perspective different from north (less width, more depth/profile), same stone palette
- Şu öneme dikkat: This is the WEST wall (left side of room), so wall faces RIGHT (toward room interior). For E wall, this sprite mirrored (flipX) in Unity.
- Stil: chatgpt_ref görsel 2'deki side wall (bookshelf side hariç, plain stone)
- Output: `wall_west_mid_plain.png`

### Piece 8: West Doorway — Side Opening
- Boyut: 96×192
- Composition: stone archway facing right (west wall doorway openning toward room interior), dark void inside
- Stil: chatgpt_ref görsellerinde yan duvarlarda göründüğü gibi side opening
- Output: `wall_west_doorway.png`

## Codex Production Workflow

For each piece (8 toplam):

1. **gpt-image-1 prompt yaz** — her piece için piece-spesifik + ortak style lock paragrafı
   ```
   Ortak style lock (her piece'in prompt'una append):
   "Dark fantasy pixel art reference for RIMA Shattered Keep, fractured charcoal granite stone, cyan rift hairline cracks, amber torch highlights (subtle), faded rouge banner accents, top-down ARPG 70-80 degree from horizon with subtle 3/4 perspective, visible top cap and slight side face for 3/4 thickness illusion, baseline at bottom of canvas, dark dungeon mood, low ambient, no characters, no UI, no text, no labels."
   ```

2. **gpt-image-1 çağrı** — Codex imagegen tool ile (S101'de wall_pilot için kullanıldı, workflow biliniyor)
   - Resolution: 1024×1024
   - Style: detailed illustrative concept (NOT pixel art — PixelLab pixel art'a çevirecek)
   - Reference: chatgpt_ref görselleri style guide olarak attach (eğer tool destekliyorsa)

3. **PIL post-process:**
   - Resize/crop to target dimension (nearest neighbor)
   - Background → alpha 0 (transparent BG threshold-based)
   - Baseline align (alt kenar pixel 0 olmalı)
   - Save as PNG RGBA

4. **PixelLab S-XL "new" prompt yaz** — user için, init image ile birlikte
   ```
   Pixel art [piece_name] for RIMA Shattered Keep dungeon backwall, charcoal fractured granite, [piece-specific element: banner/torch/arch/rift portal/pillar], 3/4 top-down perspective, sparse thin cyan rift hairline cracks, transparent background, crisp hard pixels, no antialiasing, readable [WxH] game sprite, matching RIMA wall palette.
   ```

## User Guide Doc

`STAGING/a6_wall_user_pixellab_guide.md` — user için tam rehber:

```markdown
# A6 Backwall - PixelLab Üretim Rehberi (User için)

## Workflow
1. PixelLab web UI'a git
2. Create S-XL Image (new) tool seç (1 credit/gen)
3. Settings:
   - Init image: yüklenecek wall_mockups/{piece}.png
   - Size: mockup ile aynı (init image dimension lock)
   - Remove Background: AÇIK (transparent BG output)
   - Style image: (opsiyonel) STAGING/concepts/fractured_chamber/a1_floor_wang16_FINAL.png

## 8 Piece için Init Image + Prompt

### Piece 1: North Wall Mid Plain
- Init image: STAGING/concepts/fractured_chamber/wall_mockups/wall_north_mid_plain.png
- Size: 128×192
- Prompt: [copy-paste Codex hazırlayacak]

### Piece 2: North Wall Mid Banner
- Init image: wall_north_mid_banner.png
- Size: 128×192
- Prompt: [copy-paste]

[... 6 piece daha ...]

## Ortak Negative Prompt
text, labels, numbers, watermarks, characters, UI, full room scene, floor visible, isometric 30 degree projection, perspective receding into depth, opaque background, grid lines, multiple sprites, soft glow, bloom, anti-aliasing.

## Beklenen Çıktı
- 8 PNG, transparent BG, pixel art
- RIMA Shattered Keep stil consistent
- 3/4 perspective with visible top cap
- Wall mid pieces baseline aligned
- Pillar tall enough to cover seams
- Doorway opening readable

## Kullanım (Unity)
- WallNorth GameObject (Empty) altına: wall mid'leri yan yana, pillar'ları aralarına seam cover olarak yerleştir
- WallWest GameObject altına: west mid + pillar + west doorway
- 64×192 pillar mid'lerin arasına snap (seam'i gizler)
- Modüler kompozisyon: oda boyutuna göre wall mid sayısı ayarla
```

## Çıktı dosyaları

```
STAGING/concepts/fractured_chamber/wall_mockups/
├─ wall_north_mid_plain.png         (128×192)
├─ wall_north_mid_banner.png        (128×192)
├─ wall_north_mid_torch.png         (128×192)
├─ wall_north_doorway.png           (128×192)
├─ wall_north_center_rift_portal.png (128×192)
├─ wall_pillar_universal.png        (64×192)
├─ wall_west_mid_plain.png          (96×192)
└─ wall_west_doorway.png            (96×192)

STAGING/a6_wall_user_pixellab_guide.md
STAGING/a6_wall_mockup_codex_summary.md
```

## Final Summary

`STAGING/a6_wall_mockup_codex_summary.md`:
- 8 piece path + boyut + composition note
- 8 PixelLab prompt (kopya-yapıştır hazır)
- 1 ortak negative prompt
- Total user cost: 8 credit
- Estimated user time: ~20 dk
- Pillar seam cover strategy açıklama (Unity'de nasıl kullanılır)

## Önemli Notlar

- **Codex imagegen tool'u (gpt-image-1) S101'de wall_pilot için kullanıldı, workflow biliniyor.** Erişim varsa kullan.
- Eğer gpt-image-1 erişimi yok ise: PIL ile daha detaylı silüet üret (önceki A2 edge yaklaşımından daha zengin shapes — granite block patterns, basit shading)
- BLOCKED ver eğer gpt-image-1 erişimi yok VE silüet kalitesi yetersiz görünüyorsa
- Reference görseller her piece'in prompt'una style guidance verecek — gpt-image-1'in inheriting kalitesi kritik
- User MVP odaklı: library/ritual/boss tema piece'leri SONRA (faz 2). Şu an sadece "general combat backwall" 8 piece.
- Codex DOĞRUDAN PixelLab MCP çağırmasın — sadece mockup üret + user için prompt rehberi yaz
