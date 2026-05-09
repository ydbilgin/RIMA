Source: https://www.pixellab.ai/docs/tools/create-sl-image-pro

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

Create S-XL image (Pro)

# Create S-XL image (Pro)

The Create S-XL Image tool generates multiple variations of pixel art from a text description. The number of frames generated depends on your canvas size - smaller canvases produce more variations.

## How It Works

1. Choose a canvas size (any dimensions from 16px to 512px per side for square, or larger for non-square aspect ratios)
2. Describe what to generate
3. Optionally add reference images (up to 4) and/or a style image
4. Generate

## Frame Output by Canvas Size

Grid layout is determined by the largest dimension (`max(width, height)`). Cost depends on total grid content size and varies with aspect ratio — values below are for square (1:1) canvases:

* **≤32px** → 64 frames (8×8 grid) — 20 generations
* **33-42px** → 64 frames (8×8 grid) — 25 generations
* **43-64px** → 16 frames (4×4 grid) — 20 generations
* **65-85px** → 16 frames (4×4 grid) — 25 generations
* **86-128px** → 4 frames (2×2 grid) — 20 generations
* **129-170px** → 4 frames (2×2 grid) — 25 generations
* **171-256px** → 1 frame — 20 generations
* **257-341px** → 1 frame — 25 generations
* **342-512px** → 1 frame — 40 generations

## Options

* **Description**: What to generate (e.g., "cute wizard", "medieval knight")
* **No Background**: Generate with transparent background
* **Reference Images** (optional): Up to 4 reference images to guide generation
* **Style Image** (optional): Reference for color palette, outline, detail, and shading
* **Style Options**: Control which aspects to copy from the style image
* [Seed](/docs/options/general#seed)

## Limitations

* Minimum size: 16×16px
* Maximum size depends on aspect ratio (e.g., 512×512 for 1:1 square, up to 688×384 for 16:9)
* Non-square canvases supported — the closest supported aspect ratio is selected automatically

## On this page

[How It Works](#how-it-works)[Frame Output by Canvas Size](#frame-output-by-canvas-size)[Options](#options)[Limitations](#limitations)