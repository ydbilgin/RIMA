# Dash Trail -- PixelLab Create Image Pro (V3 Web UI)

## Specs
- Canvas: 64x64 px, transparent background
- Frames: 8, cyclic loop
- Color: cold blue (#4FC3F7) solid core fading to transparent at edges
- Blend mode in Unity: Additive (SpriteRenderer material)
- Direction: non-directional; Unity rotates SR at runtime
- Playback: 12 fps

## Prompt

Low detailed pixel art VFX sprite sheet, 8 frames horizontal strip on transparent background, 64x64 pixels per frame. Dash trail afterimage effect: soft elongated teardrop smear, cold icy blue (#4FC3F7) luminous core at center fading to fully transparent edges, faint crystalline particle flecks orbiting the smear edge. Painterly muted palette, field-worn weathered aesthetic. Cyclic loop -- frame 1 fully visible bright smear, frames 2-4 smear stretches and dims, frames 5-7 particles scatter outward and fade, frame 8 near-transparent returning to frame 1 start. Radial gradient alpha on every frame. No hard outlines. No directional arrow. No character silhouette. No floor shadow. Background fully transparent PNG. Negative Prompt : photorealistic, 3D render, gradient mesh, neon glow, bright saturation, bloom flare, text, watermark, frame border, solid white background, solid black background, character limbs, motion blur streaks pointing left or right only

## Acceptance criteria
- 8 frames in a single horizontal strip (512x64 total canvas)
- Loop reads seamlessly at 12 fps in Unity additive blend
- Core color visually matches #4FC3F7 cold blue
- No opaque pixels at edges; alpha gradient confirmed in PS/Aseprite
- No painted direction bias (smear must be orientation-neutral)
- Painterly texture consistent with RIMA tile/prop aesthetic

## Cost estimate
- Provider: PixelLab Pro credits (Create Image Pro)
- Estimated: 1-2 credits per generation attempt
- Recommended: generate 2-3 variants, pick best
