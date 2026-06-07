# RESEARCH — Pixel-art generate-small-then-scale: quality + algorithm — DEEP/TECHNICAL lens (Gemini 3.1 Pro)

READ first: F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\PIXELART_SCALING_BRIEF_2026-06-04.md
Use web grounding for current best-practice. Context: solo dev, Unity URP 2D, PPU 64, Pixel Perfect Camera, characters ~64px visible, pixel-art roguelite.

Answer DEEPLY + concretely (this feeds a synthesized report):
1. **Is small→scale quality-preserving?** Definitive: yes/no + exact conditions. Cover integer-scale (2x/3x/4x nearest) = lossless-crisp vs non-integer = pixel destruction/shimmer. The math (why integer preserves the pixel grid).
2. **The correct algorithm/method** for (a) RUNTIME rendering (you do NOT pre-upscale the asset — import native + render via Pixel Perfect Camera at integer screen-scale; nearest/Point sampling) and (b) OFFLINE/asset-prep if ever upscaling (nearest for purity; or pixel-art upscalers scale2x/EPX/hqx/xBRZ/AdvMAME — explain each, when stylistically OK, when NOT for a coherent pixel game).
3. **Generate-small vs generate-big:** for TRUE pixel art, why native-small (artist/PixelLab) beats AI-big-then-downscale; what downscaling a 1024² "pixel-style" image actually requires (nearest downscale to native grid + palette snap + cleanup) and its pitfalls (off-grid, mixed pixels, anti-alias halos).
4. **Unity specifics:** Pixel Perfect Camera settings (Assets-PPU, Reference Resolution, Upscale Render Texture, Crop, Grid Snapping), sprite import (PPU, FilterMode Point, Compression None, Mip off), and how runtime camera zoom must be integer pixel-ratios to stay crisp. Godot equivalent in 2 lines.
5. **Per asset class** recommended native res + pipeline: character (64px visible), tile (32px), VFX, UI icon (64px), full-screen backdrop (the ONE case where big-gen is OK — why).

Tight, technical, numbers + algorithm names. Honest about trade-offs.
