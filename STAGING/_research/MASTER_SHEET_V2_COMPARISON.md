# Master Sheet v2 Comparison

## 1. v2 Analysis Result

- Path: `STAGING/_pixellab_outputs/walls/v2/act1_wall_modular_pack_codex_v2.png`
- Dimensions / mode: 512x512 / RGBA
- Transparent background: 62.96% true alpha
- Distinct alpha components: 88 total, 57 major components >=25 px
- Cyan presence: 1.35% of canvas, 3.63% of opaque pixels
- Pixel-art sharpness heuristic: 30131 exact opaque RGB colors, 772 quantized color buckets, 39.77% hard edge ratio, 27.46% soft-neighbor ratio
- Text artifact check: clear (none confidence)

## 2. v1 vs v2 vs ChatGPT Comparison

| Sheet | Dimensions | Mode | Transparent % | Major alpha components | Cyan % canvas / opaque | Colors exact / q16 | Edge ratio | Soft ratio | Text check | Score |
|---|---:|---|---:|---:|---:|---:|---:|---:|---|---:|
| Codex v1 | 512x512 | RGBA | 66.58% | 59 | 1.57% / 4.68% | 29661 / 664 | 43.55% | 25.43% | clear (none) | 10 |
| ChatGPT v1 | 1254x1254 | RGB | 0.00% | 1 | 1.28% / 1.28% | 129433 / 1141 | 11.72% | 15.48% | clear (none) | -1 |
| Codex v2 | 512x512 | RGBA | 62.96% | 57 | 1.35% / 3.63% | 30131 / 772 | 39.77% | 27.46% | clear (none) | 10 |

## 3. Text Artifact Check

PIL-only detection was used: dark compact connected components were grouped into repeated row patterns that resemble glyph baselines. This is a heuristic, not OCR.

- Codex v1: no label-like rows found by the PIL heuristic.
- ChatGPT v1: no label-like rows found by the PIL heuristic.
- Codex v2: no label-like rows found by the PIL heuristic.

Result for v2: no label-like row pattern was detected, and visual inspection found no written labels, letters, numbers, captions, or inscriptions.

## 4. Production Verdict

Winner: **Codex v2**.

Codex v2 is selected on tie-break because it matches the requested new v2 output format, has 512x512 RGBA with true alpha after checkerboard removal, includes clear cyan FX/decor coverage, and has no label/text artifact flag. Codex v1 remains a strong fallback with nearly identical metric strength. ChatGPT v1 has stronger render detail, but the 1254x1254 RGB/baked-background output is a production blocker for the requested pack format.

Score notes:
- Codex v1: 10 (512x512, RGBA alpha, no text flag, cyan accents present, separated components, acceptable sharpness heuristic)
- ChatGPT v1: -1 (no text flag, cyan accents present, acceptable sharpness heuristic, RGB/no true alpha, not 512x512)
- Codex v2: 10 (512x512, RGBA alpha, no text flag, cyan accents present, separated components, acceptable sharpness heuristic)

## 5. Slicing Recommendation

Slice on the 512x512 canvas: top row as 4 cells of about 128x170, middle modular area as 16 cells across two rows, bottom overlay area as two 16-cell rows. Preserve alpha and trim each sprite to tight bounds with 1-2 px transparent padding.
