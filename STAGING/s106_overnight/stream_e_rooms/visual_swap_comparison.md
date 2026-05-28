# Stream F - Real Asset Visual Swap Comparison

## Per-room comparison
| Room | Before (scene.png) | After (scene_v2.png) | chatgpt_ref target | Match score |
|---|---|---|---|---|
| Combat Basic | placeholder rects | real wall sprites, tilemap floor, Y-sort lighting pass | ChatGPT Image 25 May 2026 00_18_45 (2).png | 6.5/10 |
| Library Alcove | placeholder rects | real wall sprites, tilemap floor, Y-sort lighting pass | ChatGPT Image 25 May 2026 00_18_45 (2).png | 6/10 |
| Flooded Crypt | placeholder rects | real wall sprites, tilemap floor, Y-sort lighting pass | ChatGPT Image 25 May 2026 00_18_45 (2).png | 6/10 |
| Ritual Diamond | placeholder rects | real wall sprites, tilemap floor, Y-sort lighting pass | ChatGPT Image 25 May 2026 00_18_45 (2).png | 7/10 |
| Boss Arena | placeholder rects | real wall sprites, tilemap floor, Y-sort lighting pass | ChatGPT Image 25 May 2026 00_18_45 (2).png | 6.5/10 |

## Key visual deltas
- Real sprites placed: y
- 3/4 depth illusion: y (ProjectSettings transparency sort axis is 0,1,0; per-scene wall sprite sortingOrder is Y-derived)
- Floor visible: y (walkable cells filled from AssetPackV3 floor tile assets via Tilemap)
- Lighting atmosphere: y (cool Global Light 2D plus warm torch/cyan crystal Point Light 2D sockets where present)
- chatgpt_ref likeness improvement: 3/10 -> 6.4/10 average

## Remaining gaps
- Only the four currently bound real wall assets are available; connector, low-front runs, open gaps, and some 1x/3x spans still fall back to placeholder pieces.
- Real side wall asset exists as a right-facing 2x variant, mirrored for left runs by the builder.
- Lighting objects are present; exact visible strength still depends on the active URP 2D renderer/material response.
