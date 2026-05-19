# Hitspark -- PixelLab Create Image Pro (V3 Web UI)

## Specs
- Canvas: 64x64 px, transparent background
- Frames: 6, one-shot (no loop, plays once on hit)
- Color: white (#FFFFFF) core burst -> class-color edge (parametric; see note below)
- Direction: radial contact burst, no directional bias
- Blend mode in Unity: Additive
- Playback: 12 fps (total duration ~0.5 seconds)

## Parametric color note
The edge fade color is per-class in Unity (via Material Property Block or tinted SpriteRenderer).
Generate this prompt with NEUTRAL WARM ORANGE edge as placeholder for pilot testing.
Replace with class color at Unity material level; do not re-generate per class.
Class color map (for reference):
  Warblade:    #E53935 (red)
  Elementalist:#42A5F5 (blue)
  Ranger:      #66BB6A (green)
  Hexer:       #AB47BC (purple)
  Gunslinger:  #FFA726 (orange)  <- use as pilot placeholder
  Brawler:     #EF5350 (crimson)
  Ravager:     #FF7043 (deep orange)
  Cursemark:   #5C6BC0 (indigo)
  Shadowstep:  #26C6DA (cyan)
  Arcanist:    #EC407A (pink)

## Prompt

Low detailed pixel art VFX sprite sheet, 6 frames horizontal strip on transparent background, 64x64 pixels per frame. Contact hitspark burst effect: radial starburst of sharp spike lines emanating from center, white (#FFFFFF) bright core, warm orange edge color fading to transparent. Painterly muted weathered aesthetic, 6-8 spike rays per burst, no circular outline ring. Frame-by-frame: frame 1 compact white flash 8px, frame 2 burst expands to 20px spikes, frame 3 spikes at full extent 28px, frame 4 spikes shrink 20px dimmer, frame 5 faint residual 12px, frame 6 near-transparent final fade. No character. No directional arrow. No floor. Background fully transparent PNG. Negative Prompt : photorealistic, 3D render, neon glow, bloom, solid background, white fill background, character silhouette, ring outline, circular halo, motion blur, text, watermark

## Acceptance criteria
- 6 frames in a single horizontal strip (384x64 total canvas)
- One-shot: frame 6 fully transparent so Unity animation can stop cleanly
- White core at frame 2-3 peak (most visually intense frames)
- Radial spike pattern readable at 64px; no blurry smear
- No opaque edges; alpha gradient confirmed at edge pixels
- No painted directional bias (burst must be symmetric)

## Cost estimate
- Provider: PixelLab Pro credits (Create Image Pro)
- Estimated: 1-2 credits per generation attempt
- Recommended: generate 2-3 variants; pilot on Warblade (red edge) by tinting SR in Unity
