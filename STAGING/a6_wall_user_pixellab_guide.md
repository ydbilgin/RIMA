# A6 Backwall - PixelLab Uretim Rehberi (User icin)

## Workflow
1. PixelLab web UI'a git
2. Create S-XL Image (new) tool sec (1 credit/gen)
3. Settings:
   - Init image: yuklenecek wall_mockups/{piece}.png
   - Size: mockup ile ayni (init image dimension lock)
   - Remove Background: ACIK (transparent BG output)
   - Style image: (opsiyonel) STAGING/concepts/fractured_chamber/a1_floor_wang16_FINAL.png

## 8 Piece icin Init Image + Prompt

### Piece 1: North Wall Mid Plain
- Init image: STAGING/concepts/fractured_chamber/wall_mockups/wall_north_mid_plain.png
- Size: 128x192
- Prompt: Pixel art north wall mid plain for RIMA Shattered Keep dungeon backwall, charcoal fractured granite closed wall filler, no opening, no decor, 3/4 top-down perspective, visible top cap, sparse thin cyan rift hairline cracks, transparent background, crisp hard pixels, no antialiasing, readable 128x192 game sprite, matching RIMA wall palette.

### Piece 2: North Wall Mid Banner
- Init image: STAGING/concepts/fractured_chamber/wall_mockups/wall_north_mid_banner.png
- Size: 128x192
- Prompt: Pixel art north wall mid banner for RIMA Shattered Keep dungeon backwall, charcoal fractured granite with faded rouge hanging banner, tattered lower edge, 3/4 top-down perspective, visible top cap, sparse thin cyan rift hairline cracks, transparent background, crisp hard pixels, no antialiasing, readable 128x192 game sprite, matching RIMA wall palette.

### Piece 3: North Wall Mid Torch
- Init image: STAGING/concepts/fractured_chamber/wall_mockups/wall_north_mid_torch.png
- Size: 128x192
- Prompt: Pixel art north wall mid torch socket for RIMA Shattered Keep dungeon backwall, charcoal fractured granite with recessed empty torch bracket alcove, no flame baked in, restrained amber stone highlights only, 3/4 top-down perspective, sparse thin cyan rift hairline cracks, transparent background, crisp hard pixels, no antialiasing, readable 128x192 game sprite, matching RIMA wall palette.

### Piece 4: North Doorway
- Init image: STAGING/concepts/fractured_chamber/wall_mockups/wall_north_doorway.png
- Size: 128x192
- Prompt: Pixel art north doorway for RIMA Shattered Keep dungeon backwall, cracked charcoal granite stone archway with black void opening, no wood door, 3/4 top-down perspective, visible top cap, sparse thin cyan rift hairline cracks, transparent background, crisp hard pixels, no antialiasing, readable 128x192 game sprite, matching RIMA wall palette.

### Piece 5: North Center Rift Portal
- Init image: STAGING/concepts/fractured_chamber/wall_mockups/wall_north_center_rift_portal.png
- Size: 128x192
- Prompt: Pixel art north center rift portal landmark for RIMA Shattered Keep dungeon backwall, dramatic charcoal granite arch with medium emissive cyan rift portal center, base decor glow lower than gameplay telegraph, 3/4 top-down perspective, sparse thin cyan rift hairline cracks, transparent background, crisp hard pixels, no antialiasing, readable 128x192 game sprite, matching RIMA wall palette.

### Piece 6: Pillar Universal
- Init image: STAGING/concepts/fractured_chamber/wall_mockups/wall_pillar_universal.png
- Size: 64x192
- Prompt: Pixel art universal seam cover pillar for RIMA Shattered Keep dungeon backwall, standalone broken charcoal granite pillar, chipped stone blocks, front face and slight side face, sparse thin cyan rift hairline cracks, transparent background, crisp hard pixels, no antialiasing, readable 64x192 game sprite, matching RIMA wall palette.

### Piece 7: West Wall Mid Plain
- Init image: STAGING/concepts/fractured_chamber/wall_mockups/wall_west_mid_plain.png
- Size: 96x192
- Prompt: Pixel art west wall mid plain for RIMA Shattered Keep dungeon backwall, side-profile charcoal fractured granite wall facing right toward room interior, more depth profile than north wall, sparse thin cyan rift hairline cracks, transparent background, crisp hard pixels, no antialiasing, readable 96x192 game sprite, matching RIMA wall palette.

### Piece 8: West Doorway
- Init image: STAGING/concepts/fractured_chamber/wall_mockups/wall_west_doorway.png
- Size: 96x192
- Prompt: Pixel art west wall doorway for RIMA Shattered Keep dungeon backwall, side-profile cracked charcoal granite stone archway facing right, dark void opening toward room interior, sparse thin cyan rift hairline cracks, transparent background, crisp hard pixels, no antialiasing, readable 96x192 game sprite, matching RIMA wall palette.

## Ortak Negative Prompt
text, labels, numbers, watermarks, characters, UI, full room scene, floor visible, isometric 30 degree projection, perspective receding into depth, opaque background, grid lines, multiple sprites, soft glow, bloom, anti-aliasing.

## Beklenen Cikti
- 8 PNG, transparent BG, pixel art
- RIMA Shattered Keep stil consistent
- 3/4 perspective with visible top cap
- Wall mid pieces baseline aligned
- Pillar tall enough to cover seams
- Doorway opening readable

## Kullanim (Unity)
- WallNorth GameObject (Empty) altina: wall midleri yan yana, pillarlari aralarina seam cover olarak yerlestir
- WallWest GameObject altina: west mid + pillar + west doorway
- 64x192 pillar midlerin arasina snap (seami gizler)
- Moduler kompozisyon: oda boyutuna gore wall mid sayisi ayarla
