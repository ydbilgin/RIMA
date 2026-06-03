# RESEARCH — pixel-art "style-preserving upscale" tools (small sprite → large, same style)

ACTIVE RULES: (1) think before answering (2) cite specific tools/versions (3) flag what's hype vs real (4) BLOCKED if unclear.
Respond INLINE (dispatcher captures stdout). ~1 page. This extends an Opus analysis; focus on the CURRENT TOOL LANDSCAPE.

## The problem (a real Discord question)
"I have small reference tiles with ~32px-tall trees. I want to generate a much larger tree (~128×256) keeping
EXACTLY the same pixel-art style, level of detail, and visual language. What prompt techniques, workflows, or tools
work for scaling up pixel-art assets while preserving the original style?"

## What I need (current tools + techniques, 2025-2026, with specifics)
1. **Dedicated pixel-art-aware tools** for style-conditioned upscale / generation: PixelLab (Style Reference / init
   image + AI Freedom), Retro Diffusion (model + Aseprite plugin), Pixelorama, Astortion, any Stable-Diffusion pixel
   LoRAs / ControlNet (tile/reference), kohya, etc. Which actually preserve palette + outline + iso-angle vs which
   drift to painterly or voxel? Name them.
2. **The naive-upscale traps** to warn against: nearest-neighbor (blocky), ESRGAN/Real-ESRGAN/waifu2x (smooths, breaks
   pixel grid), generic SD upscale. Confirm why each fails for this use.
3. **The actual pro workflow** studios use: style contract (locked palette + shade-band count + outline rule), author
   large at target size with small as reference, palette-quantize back. Any known tutorials/threads/GDC talks.
4. **Palette extraction + enforcement** tools (e.g. Aseprite palette-from-sprite, lospec, palette-LUT shaders).
5. The core honest caveat: is "same style AND same level of detail at 8× size" even fully achievable, or does scaling
   necessarily force adding detail? What do experienced pixel artists say.

Tight, tool-specific, link names. No code.
