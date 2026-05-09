Source: https://www.pixellab.ai/docs/tools/try-on

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

Try on

# Try on

The Try on tool is an **experimental** feature that applies a garment or item from one image onto a character or subject in another image. You provide a subject image (e.g. character) and a try-on image (e.g. outfit or item); the tool generates a result with the try-on content applied to the subject.

## How It Works

1. Add a **subject image** (the character or figure to dress).
2. Add a **try-on image** (the garment, outfit, or item to apply).
3. Enter a short **description** of what you want (e.g. "red cloak", "armor").
4. Optionally enable **No background** or set a **color palette**.
5. Generate. You get one image with the try-on applied to the subject.

## Cost

* **1 image per run** — 1 generation.

## Options

* **Subject image**: The character or subject (min 16×16 area, max 320×320 area).
* **Try-on image**: The garment or item to apply (max 320×320 area).
* **Description**: What to apply (e.g. "knight armor", "hooded cloak").
* **No background**: Transparent background.
* **Color palette** (optional): Reference to guide colors.
* [Seed](/docs/options/general#seed)

## Limitations

* **Experimental** — behavior and availability may change.
* Subject and try-on images must be between 16×16 and 320×320 area.
* Requires **tier 2** or higher.
* Single image output per run.

## Related

* [Edit image (Pro)](/docs/tools/edit-image-pro) — edit an image with text or reference
* [Transfer outfit (Pro)](/docs/tools/transfer-outfit-pro) — apply outfit to animation frames

## On this page

[How It Works](#how-it-works)[Cost](#cost)[Options](#options)[Limitations](#limitations)[Related](#related)