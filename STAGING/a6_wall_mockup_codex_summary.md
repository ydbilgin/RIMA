# A6 Backwall Mockup Codex Summary

Generation note: OPENAI_API_KEY was not set in the shell, so gpt-image-1 API generation was unavailable. The task fallback was used: detailed PIL mockups with transparent RGBA backgrounds, fixed dimensions, hard pixels, and RIMA style cues from the inspected references.

## Output Pieces
- STAGING/concepts/fractured_chamber/wall_mockups/wall_north_mid_plain.png (128x192) - Clean closed charcoal granite north wall filler, no decor.
- STAGING/concepts/fractured_chamber/wall_mockups/wall_north_mid_banner.png (128x192) - North wall filler with faded rouge hanging banner.
- STAGING/concepts/fractured_chamber/wall_mockups/wall_north_mid_torch.png (128x192) - North wall filler with recessed empty torch bracket and amber alcove tint.
- STAGING/concepts/fractured_chamber/wall_mockups/wall_north_doorway.png (128x192) - North stone arch doorway with black void interior.
- STAGING/concepts/fractured_chamber/wall_mockups/wall_north_center_rift_portal.png (128x192) - North landmark arch with medium cyan rift portal.
- STAGING/concepts/fractured_chamber/wall_mockups/wall_pillar_universal.png (64x192) - Standalone broken granite pillar for seam cover.
- STAGING/concepts/fractured_chamber/wall_mockups/wall_west_mid_plain.png (96x192) - West side-profile wall section facing right into room.
- STAGING/concepts/fractured_chamber/wall_mockups/wall_west_doorway.png (96x192) - West side-profile stone doorway opening toward room interior.

## PixelLab Prompts

1. North Wall Mid Plain
Pixel art north wall mid plain for RIMA Shattered Keep dungeon backwall, charcoal fractured granite closed wall filler, no opening, no decor, 3/4 top-down perspective, visible top cap, sparse thin cyan rift hairline cracks, transparent background, crisp hard pixels, no antialiasing, readable 128x192 game sprite, matching RIMA wall palette.

2. North Wall Mid Banner
Pixel art north wall mid banner for RIMA Shattered Keep dungeon backwall, charcoal fractured granite with faded rouge hanging banner, tattered lower edge, 3/4 top-down perspective, visible top cap, sparse thin cyan rift hairline cracks, transparent background, crisp hard pixels, no antialiasing, readable 128x192 game sprite, matching RIMA wall palette.

3. North Wall Mid Torch
Pixel art north wall mid torch socket for RIMA Shattered Keep dungeon backwall, charcoal fractured granite with recessed empty torch bracket alcove, no flame baked in, restrained amber stone highlights only, 3/4 top-down perspective, sparse thin cyan rift hairline cracks, transparent background, crisp hard pixels, no antialiasing, readable 128x192 game sprite, matching RIMA wall palette.

4. North Doorway
Pixel art north doorway for RIMA Shattered Keep dungeon backwall, cracked charcoal granite stone archway with black void opening, no wood door, 3/4 top-down perspective, visible top cap, sparse thin cyan rift hairline cracks, transparent background, crisp hard pixels, no antialiasing, readable 128x192 game sprite, matching RIMA wall palette.

5. North Center Rift Portal
Pixel art north center rift portal landmark for RIMA Shattered Keep dungeon backwall, dramatic charcoal granite arch with medium emissive cyan rift portal center, base decor glow lower than gameplay telegraph, 3/4 top-down perspective, sparse thin cyan rift hairline cracks, transparent background, crisp hard pixels, no antialiasing, readable 128x192 game sprite, matching RIMA wall palette.

6. Pillar Universal
Pixel art universal seam cover pillar for RIMA Shattered Keep dungeon backwall, standalone broken charcoal granite pillar, chipped stone blocks, front face and slight side face, sparse thin cyan rift hairline cracks, transparent background, crisp hard pixels, no antialiasing, readable 64x192 game sprite, matching RIMA wall palette.

7. West Wall Mid Plain
Pixel art west wall mid plain for RIMA Shattered Keep dungeon backwall, side-profile charcoal fractured granite wall facing right toward room interior, more depth profile than north wall, sparse thin cyan rift hairline cracks, transparent background, crisp hard pixels, no antialiasing, readable 96x192 game sprite, matching RIMA wall palette.

8. West Doorway
Pixel art west wall doorway for RIMA Shattered Keep dungeon backwall, side-profile cracked charcoal granite stone archway facing right, dark void opening toward room interior, sparse thin cyan rift hairline cracks, transparent background, crisp hard pixels, no antialiasing, readable 96x192 game sprite, matching RIMA wall palette.

## Common Negative Prompt
text, labels, numbers, watermarks, characters, UI, full room scene, floor visible, isometric 30 degree projection, perspective receding into depth, opaque background, grid lines, multiple sprites, soft glow, bloom, anti-aliasing.

## User Cost / Time
- Total user cost: 8 credits
- Estimated user time: ~20 min

## Pillar Seam Cover Strategy
Place wall mid pieces freely along WallNorth or WallWest, then snap wall_pillar_universal.png between neighboring modules to hide seams and create decorative rhythm. The pillar is full height and shares the same bottom baseline, so Unity composition can vary room width without requiring mid pieces to tile perfectly. East wall can mirror west sprites with flipX.
