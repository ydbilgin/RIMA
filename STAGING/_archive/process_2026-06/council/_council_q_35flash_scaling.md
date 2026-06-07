# RESEARCH — Pixel-art generate-small-then-scale — LEAN / PRACTICAL lens (Gemini 3.5 Flash)

READ first: F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\PIXELART_SCALING_BRIEF_2026-06-04.md
Use web grounding. Context: solo dev, Unity URP 2D, PPU 64, Pixel Perfect Camera.

Pragmatic, blunt bullets (feeds a synthesized report):
1. **Bottom line:** is "generate 32/64px then scale in Unity" the RIGHT default? One-line yes/no + the single most important rule (integer scale + Point filter).
2. **Simplest correct pipeline** a solo dev should actually follow (gen → import settings → Pixel Perfect Camera → done). Exact Unity import checkboxes.
3. **The traps that cause quality loss** (non-integer scale, bilinear filter, compression, mip-maps, sub-pixel camera, scaling in Photoshop with bicubic) — and the one-line fix for each.
4. **AI-gen practical:** PixelLab (native small, ideal) vs Imagen/imagegen (big → must nearest-downscale + cleanup). Which to use for what, cheapest path. When is generating big + downscaling acceptable vs a waste?
5. **Don't-overthink-it:** what's the 20% that gives 99% crispness, and what fancy stuff (xBRZ/hqx upscalers, custom shaders) is NOT worth it for a normal pixel game?

Terse, practical, numbers. Favor the simple correct path.
