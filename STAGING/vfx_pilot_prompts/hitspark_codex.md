# Hitspark -- Codex Imagegen (gpt-image-1)

## Specs
- Canvas per frame: 64x64 px, transparent background
- Frames: 6, assembled into horizontal strip (384x64)
- Color: white (#FFFFFF) core -> warm orange edge (pilot placeholder; tinted per-class in Unity)
- Blend mode in Unity: Additive (SpriteRenderer)
- Type: one-shot (frame 6 fully transparent)
- Playback: 12 fps (~0.5 second total)

## Codex Task Prompt

### System context
You are a pixel art VFX production assistant for RIMA, a top-down 2D ARPG.
Style: painterly, muted desaturated palette, weathered field-worn.
All output: PNG, transparent background, 64x64 pixels per frame.
Do NOT add any background fill. Do NOT add character silhouettes.

### Task

Generate 6 individual PNG images (Frame_01 through Frame_06) for a contact hitspark VFX effect.
Assemble them into a single horizontal sprite sheet: 384x64 pixels total (6 frames x 64px wide).
Save the assembled sheet as: RIMA_Hitspark_v1_sheet.png

Frame design rules:
- Shape: radial starburst of spike rays from center, symmetric, 6-8 spikes
- Core color: white (#FFFFFF), fully opaque at center point
- Edge color: warm orange (#FFA726), fading to fully transparent at spike tips
- Alpha: gradient from white center to transparent tips over each spike length
- Style: low detail pixel art, painterly, no hard outlines, no ring border
- No character silhouette. No directional bias. No floor shadow.

Frame-by-frame animation breakdown (one-shot expand and fade):
  Frame 01: compact white flash, radius 6px, no visible spikes yet, pure white core
  Frame 02: spikes emerge, length 14px, white core 8px, orange edges faint
  Frame 03: spikes at full extent 24px, white core 10px, orange edges bright -- PEAK frame
  Frame 04: spikes shrink to 18px, brightness -30%, orange fades toward transparent
  Frame 05: spikes 10px, brightness -60%, mostly transparent except small core
  Frame 06: near-fully transparent, max 4px residual, alpha <15% -- one-shot terminal frame

Assembly instructions:
1. Generate each frame at exactly 64x64 with transparent background (RGBA PNG)
2. Confirm alpha channel on each frame before assembly
3. Place frames left-to-right: Frame_01 at x=0, Frame_02 at x=64, ..., Frame_06 at x=320
4. Final canvas: 384x64, save as RIMA_Hitspark_v1_sheet.png
5. Output path: Assets/VFX/Sprites/Hitspark/RIMA_Hitspark_v1_sheet.png

### Quality check before saving
- Pixel-peek Frame_03 center: should be close to #FFFFFF
- Pixel-peek Frame_03 spike tip: should be near #FFA726 or transparent
- Pixel-peek Frame_01 corner (0,0): alpha=0 (fully transparent)
- Pixel-peek Frame_06 center: alpha < 40 (near-invisible)
- Spike pattern must be symmetric (not skewed left, right, up, or down)

## Unity integration
- Import as sprite sheet: 6 columns, 1 row, 64x64 per cell
- Assign Additive material (Sprites/Additive or custom URP Additive shader)
- Set WrapMode: Once (ClampForever) -- one-shot, do not loop
- Tint SpriteRenderer.color at runtime via MaterialPropertyBlock for per-class color
  Example: MPB.SetColor("_Color", classColor) where classColor replaces warm orange bias
  White core (#FFFFFF) is unaffected by tint; only edge hue shifts
- Spawn via VFXManager.PlayHitspark(position, classColor) on hit confirm

## Acceptance criteria
- 384x64 horizontal sheet, 6 equal frames at 64px each
- Frame 03 is the visual peak -- must be clearly more intense than Frame 02 and 04
- Frame 06 is terminal -- near-transparent, suitable for one-shot WrapMode.Once stop
- White core visible in Frames 01-04
- No opaque background anywhere on sheet
- Radial spike symmetry: spike count equal in all quadrants

## Cost estimate
- Provider: gpt-image-1 via Codex imagegen
- Estimated: 6 individual frame generations + 1 assembly step
- Cost: low at 64px size; typical $0.01-0.05 per frame at gpt-image-1 pricing
- Total estimated: under $0.50 per pilot run
- No PixelLab credits consumed
