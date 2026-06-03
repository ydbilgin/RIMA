# Style-Preserving Pixel-Art Upscale — Analysis (S6)

> Cross-cutting (RIMA **and** LaurethStudio). Triggered by a Discord post: "I have ~32px reference tree tiles, I want a
> ~128×256 tree in EXACTLY the same pixel-art style/detail/visual-language. What tools/workflow?" + 3 example images.
> Opus analysis; ax verifies the current tool landscape separately.

## 1. WHAT IT IS
The "style-preserving upscale" = produce a LARGE pixel asset from SMALL reference sprites while keeping the **style
contract** (palette, shading model, outline treatment, iso angle, dither, light direction, "visual language").
It is **RE-AUTHORING at a higher resolution under style constraints** — not enlarging.

## 2. WHAT IT IS *NOT* (the traps)
- **NOT nearest-neighbor upscale** (×4): keeps the *same 32px of detail*, just bigger blocky pixels. The "level of
  detail" does not increase — looks like enlarged low-res, not a real big sprite.
- **NOT AI super-resolution** (ESRGAN / Real-ESRGAN / waifu2x / generic SD-upscale): smooths, anti-aliases,
  hallucinates, breaks the pixel grid + palette → it stops being pixel art.
- **NOT a free lunch.** "Exactly the same style AND the same level of detail at 8× size" is *partly self-
  contradictory*: a 32px tree is low-detail *because* it's 32px; a 128×256 canvas has room for — and the eye expects —
  more detail. You preserve palette + shading-model + angle + outline; detail **density necessarily rises**. Honest
  framing: hold the **style contract**, author NEW detail to fill the bigger canvas.

## 3. THE 3 IMAGES = THE LESSON (the drift spectrum)
- **Img 2 (small refs):** low detail, ~3-4 greens, flat-ish 2:1 iso, simple silhouettes. The style SOURCE.
- **Img 3 (large, PAINTERLY drift):** preserved "pixel-ish" but shifted to high detail-density + soft realistic
  shading + rocks/flower/roots. Beautiful, but a DIFFERENT visual language than the refs.
- **Img 4 (large, VOXEL drift):** preserved low detail but shifted to chunky faceted cuboids — a voxel/3D-render look.
- **Conclusion:** neither perfectly preserves Img 2's style. Scaling **forces a detail/style decision**; if you don't
  define the contract explicitly, the tool drifts — toward painterly OR voxel depending on tool/prompt. The "right"
  answer is whichever **holds the contract**.

## 4. HOW TO DO IT WELL
1. **Define a STYLE CONTRACT first (the real skill):** exact palette (N colors), shade-band count (e.g. 3 greens + 1
   shadow), outline rule (1px dark? selective?), iso angle (2:1), dither rule, light direction. This is what you
   preserve; detail is authored to fill the size.
2. **PixelLab Style Reference** = `create_image_pro` with the small sprite as init/style image + **low AI Freedom** →
   generate the large at target size conditioned on the refs. (RIMA already pipelines this; the **`pixelify` skill**
   does exactly HD→pixel via PixelLab Style Reference.)
3. **Palette-quantize back** to the refs' exact palette (Aseprite "palette from sprite" → apply; or a palette-LUT
   shader). Locks the colour language even if the generator drifts.
4. **Hand-touch in Aseprite** at target size with the small sprites as swatch/reference — the pro path for HERO assets.
5. **Retro Diffusion / pixel-art diffusion + ControlNet (tile/reference)** for style-conditioned gen (ax verifying).
6. **Don't** use nearest-neighbor or ESRGAN as the *generator* — wrong tool for this job.

## 5. HOW RIMA USES IT (directly on-pipeline)
- **This IS RIMA's locked flow:** imagegen/placeholder → **PixelLab refine** ("sonradan pixellab ile düzenleyeceğiz,
  boyutları PixelLab-standart tut"). The menu/hero images I just gpt-image-2'd → PixelLab Style Reference to pixel-
  perfect them = exactly this move.
- **Asset families** (cliff variants, tiles, backdrop, props): define ONE style contract (PPU64 + cyan-rift palette +
  shade bands + iso), generate the whole family via Style Reference → consistency. Solves the open "cliff variant
  family" + "seamless backdrop" needs.
- **Style contract = RIMA's PPU64 detail budget.** Lock it once (off the Warblade/floor look), apply to every asset
  regardless of source size.
- **Heed Img 3/4:** when RIMA scales a small ref to a big asset (boss 64→192, hero prop), it WILL drift unless the
  contract is explicit + enforced (palette-lock + low AI Freedom). Don't trust "it'll keep the style."

## 6. HOW LAURETHSTUDIO USES IT (master studio)
- A reusable **"Style-Scaling Pipeline" standard:** a per-project STYLE CONTRACT doc (palette + bands + outline +
  angle + dither) → any asset, any size, generated/authored to it via Style Reference + palette-lock → cross-game
  consistency. Pairs with the studio's Wang-tile/modular workflow → modular tiles + style-scaled props/heroes under
  one contract = a complete asset production system.
- **Studio principle (honest):** budget detail by size. Don't promise "identical style at any size" — promise "same
  style contract, size-appropriate detail."

## 6b. VERIFIED TOOL LANDSCAPE (ax research, 2025-2026)
| Tool | Tech | Real | Drift / caveat |
|---|---|---|---|
| **PixelLab** | Bitforge (strict ≤200px) + Pixflux (≤400px), Style Reference + AI Freedom | matches palette + light direction well | high AI Freedom → painterly OR voxel; doesn't natively hold iso-grid/outlines unless refs uniform |
| **Retro Diffusion** (Aseprite plugin, local pixel-trained diffusion) | Neural Resize + Neural Detail | respects pixel grid, low colour count, no blur | 32→256 (8×) hallucinates clusters/breaks outlines → use as **rough layout**, not final |
| **SD + ControlNet Tile + Kohya LoRA** | train style LoRA on YOUR assets, img2img + `control_v11f1e_sd15_tile`, denoise 0.3-0.4 | **best structural preservation** (trunk/branch/silhouette locked to ref) | looks like soft digital painting unless + `sd-webui-pixelart` pixelizer |
| **Palette enforce** | Aseprite **Indexed Mode** (lock palette), Lospec plugin, **Palette-LUT shader** (runtime grayscale→palette in Unity) | sanitizes AI colour drift back to exact palette | — |

**ax pro workflow (confirms §4):** Style Contract → NN-scale small sprite as silhouette GUIDE → **redraw native at target size** → colour-quantize → manual polish (jaggies/clusters/dither).
**ax verdict on the paradox:** "same style + same detail at 8×" = **mathematically/visually impossible**. Style = constraints; 8× canvas MUST add detail. Upscaling pixel art is **re-authoring, not image-processing** — AI speeds layout, a human decides the translation.

## 7. THE ONE-LINE ANSWER TO THE DISCORD POST
You can't losslessly "scale up" pixel art keeping identical detail — you **re-author at the new size under a locked
style contract** (palette + shading + angle + outline), using the small sprites as reference. PixelLab Style Reference
(low AI Freedom) + palette-quantize is the fastest faithful route; Aseprite hand-touch for heroes. Img 3 added too
much (painterly drift), Img 4 simplified (voxel drift) — the goal is the version that **holds the contract**.
