# RESEARCH BRIEF — Pixel-art: generate SMALL (32/64px) then SCALE in engine — quality + algorithm
Date: 2026-06-04 · SEPARATE from CharSelect work · Council (cx + ax-3.1 + ax-3.5) + Opus research → synthesis report

## THE QUESTION (user, after a Gemini chat)
A Gemini chat advised: instead of generating BIG assets, generate small (32px or 64px) then scale up in Unity/Godot. The user wants a council + Opus-researched, synthesized report answering:
1. Is scaling small pixel-art up in-engine **quality-preserving** (no loss/blur)? Under what conditions?
2. What is the correct **algorithm / method** for scaling pixel art without quality loss (runtime AND asset-prep)?
3. For RIMA specifically (Unity URP 2D, PPU 64, Pixel Perfect Camera, characters ~64px visible, tiles, VFX, UI icons): what's the right native-resolution + scaling pipeline?
4. Generate-small vs generate-big — which is better for crisp pixel art, and why (esp. given AI generators: PixelLab true-small vs imagegen/Imagen big "pixel-style")?

## KEY CONCEPTS TO ADDRESS (so the report is concrete)
- Integer vs non-integer scaling (2x/3x/4x nearest-neighbor = crisp; 1.5x/2.3x = broken pixels / shimmer).
- Nearest-neighbor (Point filter) vs bilinear; why Point for pixel art.
- Unity: Pixel Perfect Camera component (ref resolution, PPU, upscale-RT, crop), Sprite import (PPU, Point, Compression None, FilterMode), how runtime zoom should be integer pixel-ratios.
- Godot equivalent (texture filter Nearest, viewport stretch, integer scaling) — brief, for completeness.
- Pixel-art UPSCALE algorithms (scale2x/EPX, hqx, xBRZ, AdvMAME) — when (stylistic/offline) vs when NOT (runtime, where you just import native + integer-render).
- Sub-pixel movement / camera snapping / "pixel shimmer" pitfalls.
- AI gen reality: PixelLab outputs true native small pixel art (ideal for this); Imagen/imagegen outputs 1024² high-res "pixel-style" that must be DOWNSCALED (nearest) + cleaned to become true pixel art — opposite of "generate small".

## DELIVERABLE
A synthesized report (Opus): definitive answer (yes/no + conditions), the recommended algorithm/pipeline for RIMA, generate-small-vs-big verdict per asset class (character / tile / VFX / UI icon / backdrop), and concrete Unity import/camera settings. Honest about limits.
