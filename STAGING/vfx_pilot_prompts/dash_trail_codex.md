# Dash Trail -- Codex Imagegen (gpt-image-1)

## Specs
- Canvas per frame: 64x64 px, transparent background
- Frames: 8, assembled into horizontal strip (512x64)
- Color: cold blue (#4FC3F7) core, fully transparent edges
- Blend mode in Unity: Additive (SpriteRenderer)
- Direction: non-directional (Unity rotates at runtime)
- Playback: 12 fps

## Codex Task Prompt

### System context
You are a pixel art VFX production assistant for RIMA, a top-down 2D ARPG.
Style: painterly, muted desaturated palette, weathered field-worn.
All output: PNG, transparent background, 64x64 pixels per frame.
Do NOT add any background fill. Do NOT add character silhouettes.

### Task

Generate 8 individual PNG images (Frame_01 through Frame_08) for a dash trail VFX effect.
Assemble them into a single horizontal sprite sheet: 512x64 pixels total (8 frames x 64px wide).
Save the assembled sheet as: RIMA_DashTrail_v1_sheet.png

Frame design rules:
- Shape: soft elongated smear blob, orientation-neutral (no left/right bias), roughly circular with slight radial stretch
- Core color: cold icy blue (#4FC3F7), fully opaque at center
- Edge: alpha gradient to fully transparent over 8-12 pixels from edge
- Particle detail: 3-5 small crystalline fleck pixels orbiting outer edge of smear
- Low detail pixel art style, painterly texture, no hard outlines, no black borders

Frame-by-frame animation breakdown:
  Frame 01: smear at full brightness, core 20px diameter, particles at 24px radius
  Frame 02: smear expands slightly to 22px, brightness -10%, particles drift outward to 26px
  Frame 03: smear 24px, brightness -20%, particles at 28px, flecks slightly dimmer
  Frame 04: smear 22px, brightness -35%, particles at 30px, some flecks disappear
  Frame 05: smear 18px, brightness -50%, only 2-3 particles remain at 32px
  Frame 06: smear 14px, brightness -65%, particles near-transparent
  Frame 07: smear 10px, brightness -80%, 1 particle faint
  Frame 08: smear 6px, brightness -92%, near-invisible, prepares to loop back to Frame 01

Assembly instructions:
1. Generate each frame at exactly 64x64 with transparent background (RGBA PNG)
2. Confirm alpha channel on each frame before assembly
3. Place frames left-to-right: Frame_01 at x=0, Frame_02 at x=64, ..., Frame_08 at x=448
4. Final canvas: 512x64, save as RIMA_DashTrail_v1_sheet.png
5. Output path: Assets/VFX/Sprites/DashTrail/RIMA_DashTrail_v1_sheet.png

### Quality check before saving
- Pixel-peek Frame_01 center: should be close to #4FC3F7
- Pixel-peek Frame_01 corner (0,0): should be fully transparent (alpha=0)
- Frame_08 should be visually near-invisible but not identically blank
- No frame should contain white or black fills

## Acceptance criteria
- 512x64 horizontal sheet, 8 equal frames at 64px each
- Core color visually matches #4FC3F7 cold blue (allow +/- 15 hue)
- All edge pixels fully transparent
- Loop reads seamlessly at 12 fps in Unity additive blend mode
- No opaque background, no character silhouette, no directional shape

## Cost estimate
- Provider: gpt-image-1 via Codex imagegen
- Estimated: 8 individual frame generations + 1 assembly step
- Cost: depends on Codex/OpenAI plan -- typically $0.02-0.08 per 1024px generation
- At 64px: cost likely low; actual rate subject to API pricing
- No PixelLab credits consumed
