# Walls High Topdown 3/4 QC

Date: 2026-05-24 15:56:06
Projection lock: HIGH TOP-DOWN 3/4, Hades / Children of Morta / Diablo III style. Not true 45 degree iso diamond.

## New Raw Generation Status

- PASS: `wall_nw_mid_broken_raw.png` generated with built-in image_gen, normalized to 1024x1024, copied to project raw folder, source `C:\Users\ydbil\.codex-profiles\laurethayday\generated_images\019e59ff-d404-7c20-9a4b-6ee2fa111189\ig_0ea50833491e3e46016a12f20042c88191a1e29eea28ebc4f4.png`.
- PASS: `wall_ne_mid_broken_raw.png` generated with built-in image_gen, normalized to 1024x1024, copied to project raw folder, source `C:\Users\ydbil\.codex-profiles\laurethayday\generated_images\019e59ff-d404-7c20-9a4b-6ee2fa111189\ig_0ea50833491e3e46016a12f23f16708191a32aec28a966bacc.png`.
- PASS: `wall_pillar_universal_raw.png` generated with built-in image_gen, normalized to 1024x1024, copied to project raw folder, source `C:\Users\ydbil\.codex-profiles\laurethayday\generated_images\019e59ff-d413-7a33-ad75-354fe3354ea6\ig_0d6eac7bb1345d99016a12f2d7b460819196520103158df1e4.png`.

## Archive Operations

- PASS: `wall_nw_torch_raw.png` archived at `STAGING/concepts/fractured_chamber/iso_assets/raw/_archive_v1/wall_nw_torch_raw.png`.
- PASS: `wall_ne_torch_raw.png` archived at `STAGING/concepts/fractured_chamber/iso_assets/raw/_archive_v1/wall_ne_torch_raw.png`.

## Final Downscale Results

| # | Raw | Final | Expected | Actual | Alpha zero px | Green residual px |
|---|---|---|---|---|---:|---:|
| 1 | `iso_floor_clean_raw.png` | `iso_floor_clean.png` | 128x64 | 128x64 | 4667 | 0 |
| 2 | `iso_floor_cracked_raw.png` | `iso_floor_cracked.png` | 128x64 | 128x64 | 4699 | 0 |
| 3 | `iso_floor_rift_glow_raw.png` | `iso_floor_rift_glow.png` | 128x64 | 128x64 | 4648 | 0 |
| 4 | `iso_floor_broken_raw.png` | `iso_floor_broken.png` | 128x64 | 128x64 | 4563 | 0 |
| 5 | `iso_floor_edge_light_raw.png` | `iso_floor_edge_light.png` | 128x64 | 128x64 | 4721 | 0 |
| 6 | `iso_floor_debris_raw.png` | `iso_floor_debris.png` | 128x64 | 128x64 | 4568 | 0 |
| 7 | `wall_nw_mid_plain_raw.png` | `wall_nw_mid_plain.png` | 128x384 | 128x384 | 19684 | 0 |
| 8 | `wall_nw_mid_variant_raw.png` | `wall_nw_mid_variant.png` | 128x384 | 128x384 | 17695 | 0 |
| 9 | `wall_nw_mid_broken_raw.png` | `wall_nw_mid_broken.png` | 128x384 | 128x384 | 33983 | 0 |
| 10 | `wall_nw_doorway_raw.png` | `wall_nw_doorway.png` | 128x384 | 128x384 | 23296 | 0 |
| 11 | `wall_ne_mid_plain_raw.png` | `wall_ne_mid_plain.png` | 128x384 | 128x384 | 19779 | 0 |
| 12 | `wall_ne_mid_variant_raw.png` | `wall_ne_mid_variant.png` | 128x384 | 128x384 | 18335 | 0 |
| 13 | `wall_ne_mid_broken_raw.png` | `wall_ne_mid_broken.png` | 128x384 | 128x384 | 29589 | 0 |
| 14 | `wall_ne_doorway_raw.png` | `wall_ne_doorway.png` | 128x384 | 128x384 | 21334 | 0 |
| 15 | `wall_n_corner_raw.png` | `wall_n_corner.png` | 128x384 | 128x384 | 16667 | 0 |
| 16 | `wall_n_landmark_raw.png` | `wall_n_landmark.png` | 256x384 | 256x384 | 25671 | 0 |
| 17 | `wall_pillar_universal_raw.png` | `wall_pillar_universal.png` | 64x384 | 64x384 | 20070 | 0 |

## Visual QC Checklist

- PASS: Floors use slight 3/4 perspective, not true diamond iso.
- PASS: Walls read as high top-down 3/4 wall sprites with visible top caps / side-face hints.
- PASS: New broken wall variants contain damaged stones, voids, cyan rift crack glow, and rubble/noise at the base.
- PASS: Pillar is narrow, full-height, square-section, and suitable as a seam cover.
- PASS: No baked torch, banner, character, UI, text, or watermark visible in the new raw set.
- PASS: Palette stays in charcoal fractured granite with cyan rift accents.
- PASS: Final files have exact target pixel dimensions.
- PASS: Chroma key cleanup completed; conservative residual green check returns 0 px for all finals.

## Verdict

PASS. 17 final assets are exported at the requested dimensions for the HIGH TOP-DOWN 3/4 wall/floor set. Torch alcove raw assets are archived and excluded from the final set.
