# A2 Edge Mockup Summary

## Mockup PNG Outputs
- STAGING/concepts/fractured_chamber/edge_mockups/edge_n_horizontal_mockup_128x64.png - 128x64 RGBA transparent; horizontal bottom granite block row, broken top silhouette, flat bottom baseline.
- STAGING/concepts/fractured_chamber/edge_mockups/edge_w_vertical_mockup_64x128.png - 64x128 RGBA transparent; right-side vertical granite block row, chipped inner edge, mirror-compatible for east.
- STAGING/concepts/fractured_chamber/edge_mockups/edge_corner_outer_mockup_64x64.png - 64x64 RGBA transparent; L-shape outer corner, right vertical leg plus bottom horizontal leg, chipped inner corner.
- STAGING/concepts/fractured_chamber/edge_mockups/edge_rubble_cluster_mockup_64x64.png - 64x64 RGBA transparent; scattered low granite chunks, denser near bottom, sparse upper debris.

## Copy-Paste Prompts

### North-facing low edge
Pixel art low broken floor edge for RIMA Shattered Keep dungeon, charcoal granite blocks, north-facing horizontal ledge, low edge not a wall, subtle 3/4 thickness side face, chipped jagged top silhouette, transparent background, sparse thin cyan rift hairline cracks, readable 128x64 game sprite, crisp hard pixels, no antialiasing.

### West-facing low edge
Pixel art low broken floor edge for RIMA Shattered Keep dungeon, charcoal granite blocks, west-facing vertical ledge, mirror-compatible for east edge, low edge not a wall, subtle 3/4 thickness side face, chipped inner side, transparent background, sparse thin cyan rift hairline cracks, readable 64x128 game sprite, crisp hard pixels, no antialiasing.

### Outer corner low edge cap
Pixel art outer corner cap for a low broken floor edge in RIMA Shattered Keep dungeon, charcoal granite L-shaped ledge, low edge not a wall, subtle 3/4 thickness side face, chipped inner corner, transparent background, sparse thin cyan rift hairline cracks, readable 64x64 game sprite, crisp hard pixels, no antialiasing.

### Rubble cluster
Pixel art scattered rubble cluster for RIMA Shattered Keep dungeon seam cover, charcoal granite chunks, low silhouette, broken floor-edge debris, transparent background, sparse thin cyan rift hairline cracks on a few stones, readable 64x64 game sprite, crisp hard pixels, no antialiasing.

## Common Negative Prompt
```text
text, labels, numbers, watermarks, characters, UI, full wall, tall wall, defensive wall, pillar, tower, high cliff, building facade, opaque background, grid lines, cell separators, multiple sprites, soft glow, bloom, anti-aliasing.
```

## Workflow Summary
- Use PixelLab web UI.
- Choose Create S-XL Image (new).
- Upload the matching mockup PNG as init image.
- Keep output size locked to the init image dimensions.
- Enable Remove Background for transparent PNG output.
- Optional style image: STAGING/concepts/fractured_chamber/a1_floor_wang16_FINAL.png.

## Cost / Time
- Total user cost: 4 credits.
- Estimated manual time: ~15 min.
