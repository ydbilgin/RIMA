Source: https://www.pixellab.ai/docs/tools/animate-with-text-new

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

Animate with text (New)

# Animate with text (New)

The new Animate with Text tool generates animation frames from a single reference image using a text action description. It produces smoother and more natural animations compared to the previous version.

## How It Works

1. Upload a reference image (your character or object)
2. Enter an action description (e.g., "walking", "jumping", "attacking")
3. Choose the number of frames (4-16, must be even)
4. Generate

The tool animates your reference image based on the action you describe, keeping the first frame identical to your input.

## Key Differences from Previous Version

* **Free for all users** — no subscription required
* **Flexible frame count** — choose 4 to 16 frames (must be even)
* **Dynamic pricing** — cost scales with image size and frame count
* **Faster generation** — uses a new animation model
* **First frame preserved** — your input image is always kept as frame 1

## Cost

Cost depends on image size and frame count. Common examples:

| Size | 4 frames | 8 frames | 16 frames |
| --- | --- | --- | --- |
| 64x64 | 1 gen | 2 gens | 3 gens |
| 96x96 | 2 gens | 3 gens | 5 gens |
| 128x128 | 3 gens | 5 gens | 9 gens |
| 256x256 | 9 gens | — | — |

Larger images support fewer frames due to a total pixel budget. The exact cost is shown in the UI before generating.

## Options

* **Reference Image** (required): Your character or object sprite to animate
* **Animation Action** (required): What animation to create (e.g., "walk", "run", "jump", "attack", "idle")
* **Frame Count**: Number of animation frames, 4-16 (must be even). Maximum depends on image size.
* **Remove Background**: Generate with transparent background
* [Seed](/docs/options/general#seed)

## Limitations

* Maximum reference image size is 256x256 pixels
* Frame count must be even (4, 6, 8, 10, 12, 14, or 16)
* Total pixel budget: width × height × frames cannot exceed 524,288
  + For example, a 256x256 image can only have 4-8 frames
  + A 64x64 image can have up to 16 frames
* Available in Pixelorama, Aseprite, and Creator

## On this page

[How It Works](#how-it-works)[Key Differences from Previous Version](#key-differences-from-previous-version)[Cost](#cost)[Options](#options)[Limitations](#limitations)