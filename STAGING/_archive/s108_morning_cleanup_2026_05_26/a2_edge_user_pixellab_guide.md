# A2 Edge - PixelLab Uretim Rehberi (User icin)

## Workflow
1. PixelLab web UI'a git
2. **Create S-XL Image (new)** tool sec (1 credit/gen)
3. Settings:
   - Init image: yuklenecek mockup PNG
   - Size: mockup ile ayni (init image dimension lock)
   - Remove Background: ACIK (transparent BG cikti icin)
   - Style image: (opsiyonel) STAGING/concepts/fractured_chamber/a1_floor_wang16_FINAL.png

## 4 Asset icin Prompt + Init Image

### Asset 1: North-facing low edge (yatay)
- **Init image:** STAGING/concepts/fractured_chamber/edge_mockups/edge_n_horizontal_mockup_128x64.png
- **Size:** 128x64
- **Prompt:**
> Pixel art low broken floor edge for RIMA Shattered Keep dungeon, charcoal granite blocks, north-facing horizontal ledge, low edge not a wall, subtle 3/4 thickness side face, chipped jagged top silhouette, transparent background, sparse thin cyan rift hairline cracks, readable 128x64 game sprite, crisp hard pixels, no antialiasing.

### Asset 2: West-facing low edge (dikey, E icin mirror)
- **Init image:** STAGING/concepts/fractured_chamber/edge_mockups/edge_w_vertical_mockup_64x128.png
- **Size:** 64x128
- **Prompt:**
> Pixel art low broken floor edge for RIMA Shattered Keep dungeon, charcoal granite blocks, west-facing vertical ledge, mirror-compatible for east edge, low edge not a wall, subtle 3/4 thickness side face, chipped inner side, transparent background, sparse thin cyan rift hairline cracks, readable 64x128 game sprite, crisp hard pixels, no antialiasing.

### Asset 3: Outer corner low edge cap
- **Init image:** STAGING/concepts/fractured_chamber/edge_mockups/edge_corner_outer_mockup_64x64.png
- **Size:** 64x64
- **Prompt:**
> Pixel art outer corner cap for a low broken floor edge in RIMA Shattered Keep dungeon, charcoal granite L-shaped ledge, low edge not a wall, subtle 3/4 thickness side face, chipped inner corner, transparent background, sparse thin cyan rift hairline cracks, readable 64x64 game sprite, crisp hard pixels, no antialiasing.

### Asset 4: Rubble cluster (seam cover)
- **Init image:** STAGING/concepts/fractured_chamber/edge_mockups/edge_rubble_cluster_mockup_64x64.png
- **Size:** 64x64
- **Prompt:**
> Pixel art scattered rubble cluster for RIMA Shattered Keep dungeon seam cover, charcoal granite chunks, low silhouette, broken floor-edge debris, transparent background, sparse thin cyan rift hairline cracks on a few stones, readable 64x64 game sprite, crisp hard pixels, no antialiasing.

## Negative Prompt (ortak, 4 asset icin)
```text
text, labels, numbers, watermarks, characters, UI, full wall, tall wall, defensive wall, pillar, tower, high cliff, building facade, opaque background, grid lines, cell separators, multiple sprites, soft glow, bloom, anti-aliasing.
```

## Beklenen cikti
- 4 PNG, transparent BG, pixel art
- Stil A1 floor sheet ile uyumlu (charcoal granite, RIMA Shattered Keep)
- Low edge (NOT full wall)
- 3/4 thickness illusion subtle side face
- Cyan rift crack hairline sparse

## Ciktilari kaydet
- STAGING/concepts/fractured_chamber/edge_outputs/
  - edge_n_horizontal_v1.png
  - edge_w_vertical_v1.png
  - edge_corner_outer_v1.png
  - edge_rubble_cluster_v1.png

Sonra Claude orchestrator'a paylas, QC + Unity import dispatch atilacak.
