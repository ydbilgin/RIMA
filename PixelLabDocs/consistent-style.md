Source: https://www.pixellab.ai/docs/tools/consistent-style

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

Create images from style references (Pro)

# Create images from style references (Pro)

The Create Images from Style References tool generates multiple new images that match the visual style of your reference images. Perfect for creating characters, items, and objects that fit together in your game.

## How It Works

1. Add one or more style reference images using the "Add style image" button
2. Enter a description of what you want to generate (e.g., "warrior with sword", "small slime", "treasure chest")
3. Generate

The tool creates multiple variations that match the style of your reference images.

## Frame Output by Image Size

The number of generated frames depends on the largest style image dimension. Cost can vary non-linearly as the grid adjusts per size:

* **≤32px** → 64 frames (8×8 grid) — 20 generations
* **33-42px** → 64 frames (8×8 grid) — 25 generations
* **43-64px** → 16 frames (4×4 grid) — 20 generations
* **65-85px** → 16 frames (4×4 grid) — 25 generations
* **86-128px** → 4 frames (2×2 grid) — 20 generations
* **129-170px** → 4 frames (2×2 grid) — 25 generations
* **171-256px** → 1 frame — 20 generations
* **257-341px** → 1 frame — 25 generations
* **342-512px** → 1 frame — 40 generations

## Maximum Style Images

The maximum number of style reference images you can add equals the number of frames for that size:

* **32x32**: up to 64 style images
* **64x64**: up to 16 style images
* **128x128**: up to 4 style images
* **256x256**: up to 1 style image

## Options

* **Style Images**: Reference images that define the visual style
* **Description**: What to generate (e.g., "similar characters", "food icons", "potions")
* **Style Description**: Optional text to guide the style (e.g., "pixel art RPG style", "8-bit retro style")
* **No Background**: Generate with transparent background (default: on)
* [Seed](/docs/options/general#seed)

## Tips

* Use multiple style images that share similar aesthetics for more consistent results
* The more style images you provide, the better the model understands your desired style

## Limitations

* Maximum image size is 512x512 pixels
* Requires Tier 1 subscription
* Available in Aseprite and Pixelorama extensions and Creator

## On this page

[How It Works](#how-it-works)[Frame Output by Image Size](#frame-output-by-image-size)[Maximum Style Images](#maximum-style-images)[Options](#options)[Tips](#tips)[Limitations](#limitations)