# CODEX_DONE_rift_threshold_imagegen

Task: Rift Threshold 4-state concept image generation.

## Outputs

| State | Output path | Validation |
|---|---|---|
| Locked | F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\concepts\rift_threshold_locked_act1.png | 128x128 RGBA, transparent corners |
| Active | F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\concepts\rift_threshold_active_act1.png | 128x128 RGBA, transparent corners |
| Portal | F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\concepts\rift_threshold_portal_act1.png | 128x128 RGBA, transparent corners |
| Final | F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\concepts\rift_threshold_final_act1.png | 128x128 RGBA, transparent corners |

## Generation Parameters

- Requested backend: gpt-image-1.
- Execution route used: Codex built-in imagegen route, then shell post-process.
- Shell API/CLI precheck: OPENAI_API_KEY missing, so direct shell gpt-image-1 CLI/API could not be used.
- Source output directory: C:\Users\ydbil\.codex-profiles\laurethgame\generated_images\019e46c7-e106-7380-8806-af94ea9e1dc6
- Prompt type: stylized-concept, RIMA game concept sprite.
- Source composition: 1:1 square, centered isometric 35-degree fake top-down granite arch opening, Act 1 cyan Rift Threshold state.
- Chroma key: flat #00ff00 background requested for alpha removal.
- Post-process: copied source PNGs to tmp/rift_threshold_imagegen, removed chroma key with remove_chroma_key.py, downsampled to 128x128 PNG with nearest-neighbor Pillow resize.
- Output format: PNG RGBA.
- Final target directory: STAGING/concepts/.

## Source Files

| State | Source file |
|---|---|
| Locked | C:\Users\ydbil\.codex-profiles\laurethgame\generated_images\019e46c7-e106-7380-8806-af94ea9e1dc6\ig_0f52f3f537065846016a0e067182f48191b8d6723119cc458a.png |
| Active | C:\Users\ydbil\.codex-profiles\laurethgame\generated_images\019e46c7-e106-7380-8806-af94ea9e1dc6\ig_0f52f3f537065846016a0e06cf8df081919934b91e7d1b0c68.png |
| Portal | C:\Users\ydbil\.codex-profiles\laurethgame\generated_images\019e46c7-e106-7380-8806-af94ea9e1dc6\ig_0f52f3f537065846016a0e071ae9388191b1603d89cfb4e211.png |
| Final | C:\Users\ydbil\.codex-profiles\laurethgame\generated_images\019e46c7-e106-7380-8806-af94ea9e1dc6\ig_0f52f3f537065846016a0e0759e0ac8191a39be0239c7a7e8b.png |

## Alpha / Size Validation

| File | Size | Transparent px | Partial alpha px | Opaque px | Corners |
|---|---:|---:|---:|---:|---|
| rift_threshold_locked_act1.png | 128x128 | 10727 | 34 | 5623 | 0,0,0,0 |
| rift_threshold_active_act1.png | 128x128 | 10056 | 106 | 6222 | 0,0,0,0 |
| rift_threshold_portal_act1.png | 128x128 | 10571 | 60 | 5753 | 0,0,0,0 |
| rift_threshold_final_act1.png | 128x128 | 9683 | 71 | 6630 | 0,0,0,0 |

## Visual Notes

- Locked: readable granite arch with narrow dim cyan vertical rift seam; low-intensity dormant read is clear.
- Active: same arch silhouette with brighter cyan core, stronger halo, and rising particle read.
- Portal: strong white-cyan vertical burst, arch still visible at left/right edges; strongest transition frame read.
- Final: active-like bright rift with denser particle column; arch remains centered and readable. It is brighter, but the generated arch scale is close to active rather than dramatically larger.

## User-visible Inline Preview Suggestion

`md
![Rift Threshold locked](STAGING/concepts/rift_threshold_locked_act1.png)
![Rift Threshold active](STAGING/concepts/rift_threshold_active_act1.png)
![Rift Threshold portal](STAGING/concepts/rift_threshold_portal_act1.png)
![Rift Threshold final](STAGING/concepts/rift_threshold_final_act1.png)
`

## Contact Sheet

	mp/rift_threshold_imagegen/contact_sheet.png

## Scope

- No Unity scene changes.
- No Pilot A wall files modified.
- Concept-only output, not production PixelLab game asset.
