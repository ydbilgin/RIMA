Source: https://www.pixellab.ai/docs/tools/create-tiles-pro

[PixelLab](/)[Pricing](/#checkout)[Docs](/docs)[API](/pixellab-api)[Vibe coding](/mcp)[Enterprise](/enterprise)[Sign in](/signin)

[Documentation](/docs)[Community](https://discord.gg/pBeyTBF8T7)[API Documentation](https://api.pixellab.ai/v1/docs)[AI Agent Toolkit](/mcp)

Ask AI...

## Introduction

[Introduction](/docs)[Ways to use PixelLab](/docs/ways-to-use-pixellab)[Installation (Aseprite)](/docs/installation)[Introduction to Pixelorama](/docs/introduction-pixelorama)[FAQ](/docs/faq)

## Guides

[Init images and inpainting](/docs/getting-started)[Creating maps](/docs/guides/map-tiles)[Rotating a character](/docs/guides/rotating-a-character)

## Create image

[Create images from style references (Pro)](/docs/tools/consistent-style)[Create S-L image (Pro)](/docs/tools/create-sl-image-pro)[Create M-XL image (new)](/docs/tools/create-image-flux)[Image to image (depth)](/docs/tools/image-to-image-depth)[Create S-M image](/docs/tools/style)[Create S-M image (old)](/docs/tools/style_old)[Pose to image](/docs/tools/pose-to-image)[Image to pixel art](/docs/tools/image-to-pixel-art)

## Edit image

[Edit image](/docs/tools/edit-image)[Edit image (Pro)](/docs/tools/edit-image-pro)[Remove background](/docs/tools/remove-background)[Resize](/docs/tools/resize)[Unzoom pixel art](/docs/tools/unzoom-pixelart)

## Rotate

[Rotate](/docs/tools/rotate)[Create 8-directional sprite (Pro)](/docs/tools/create-8-rotations-pro)

## Animate

[Animate with text (New)](/docs/tools/animate-with-text-new)[Animate with text (Pro)](/docs/tools/animate-with-text-pro)[Create animated object/character (Pro)](/docs/tools/text2animation)[Animation to animation](/docs/tools/animation-to-animation)[Animate with skeleton](/docs/tools/animate-with-skeleton)[Edit animation (Pro)](/docs/tools/edit-animation-pro)[Transfer outfit (Pro)](/docs/tools/transfer-outfit-pro)[Re-pose](/docs/tools/re-pose)[Animate with text (old)](/docs/tools/animation)[Interpolate (old)](/docs/tools/interpolation)[Create animations (automatic)](/docs/tools/create-animations-automatic)

## Map

[Create map (pixflux)](/docs/tools/create-map)[Extend map (v2)](/docs/tools/extend-map-v2)[Extend map](/docs/tools/extend-map)[Extend map (old)](/docs/tools/extend-map-old)[Create texture](/docs/tools/create-texture)[Create tileset](/docs/tools/create-tileset)[Create isometric tile](/docs/tools/create-isometric-tile)[Create tiles (Pro)](/docs/tools/create-tiles-pro)

## Inpaint

[Inpaint](/docs/tools/inpaint)[Inpaint v3](/docs/tools/inpaint-v3)[Inpaint M-L (pixpatch v2)](/docs/tools/inpaint-pixpatch-v2)

## Reduce colors

[Reduce colors](/docs/tools/reduce-colors)

## Experimental tools

[Create walking character](/docs/tools/create-instant-character)[Try on](/docs/tools/try-on)[Multi image](/docs/tools/multi-image)

## Extra tools

[Create S-M image (style)](/docs/tools/style)[Create S-M image (style, old)](/docs/tools/style_old)[Reshape](/docs/tools/reshape)[Create UI elements](/docs/tools/create-ui-elements)[Create UI elements (Pro)](/docs/tools/create-ui-elements-pro)

## Tool options

[General](/docs/options/general)[Init image](/docs/options/init-image)[Inpainting](/docs/options/inpainting)[Guidance](/docs/options/guidance)[Character](/docs/options/character)[Colors](/docs/options/color)[Camera](/docs/options/camera)[Projection](/docs/options/projection)

docs

tools

Create tiles (Pro)

# Create tiles (Pro)

The Create tiles (Pro) tool generates tile variations for game maps from a text description. It supports multiple tile shapes (top-down square, hex, hex pointy, isometric, octagon) and sizes, so you can build terrain transitions and map assets in one go.

## How It Works

1. Enter a **description** of the tiles (e.g. "grass to dirt", "stone wall", "water edge").
2. Choose **tile type**: Square (top-down), Hex, Hex pointy, Isometric, or Octagon.
3. Set **tile size** (and height for non-square tiles if needed).
4. Optionally set **view angle**, **thickness**, or add **style reference tiles** for consistency.
5. Generate. You get a set of tiles ready for use in your map.

## Cost

Cost is **20 or 25 generations** depending on tile type and size. Most small and medium sizes cost 20; larger sizes (e.g. 64×64, 96, 128, or 64×128 depending on shape) cost 25. When style reference tiles are used, cost can vary (typically 20–40).

## Options

* **Description**: What the tiles should look like (e.g. "medieval cobblestone", "grass and sand transition").
* **Tile type**: Square (top-down), Hex, Hex pointy, Isometric, Octagon.
* **Tile size**: 16, 32, 48, 64, 96, 128 (square) or 16×32, 32×64, 48×96, 64×128 (width×height).
* **View angle**: Low top-down (default), high top-down, or side.
* **Tile thickness**: Visual thickness of tiles (where applicable).
* **Style tiles** (optional): Reference images to match style; when used, cost is not fixed to the grid above.
* [Seed](/docs/options/general#seed)

## Limitations

* Requires a paid subscription (tier 1 or above).

## RIMA Kullanım Senaryosu

Production Playbook Adım 4-8 (F1/F2/F3 floor tiles + transitions). MCP üzerinden `create_tiles_pro` ile çağrılabilir.

Tile boyutları özeti:
- Kare: 16, 32, 48, **64**, 96, 128 px
- Dikdörtgen: 16x32, 32x64, 48x96, **64x128** (RIMA duvar boyutu)
- Ana kullanım: Isometric tile tipi

## Related

* [Create tileset](/docs/tools/create-tileset) — non-Pro tileset generation
* [Create isometric tile](/docs/tools/create-isometric-tile) — isometric tiles (non-Pro)
* [Creating maps](/docs/guides/map-tiles) — map and tiles guide

## On this page

[How It Works](#how-it-works)[Cost](#cost)[Options](#options)[Limitations](#limitations)[Related](#related)