Source: https://www.pixellab.ai/docs/tools/create-8-rotations-pro

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

Create 8-directional sprite (Pro)

# Create 8-directional sprite (Pro)

The Create 8-directional sprite (Pro) tool generates all 8 rotational views of a character or object in one go. It uses AI to produce south, south-east, east, north-east, north, north-west, west, and south-west views in a single 3×3 grid (8 directions plus center reference). Ideal for top-down or isometric games.

## Methods

* **Create with style** — Describe the character (e.g. "cute wizard", "slime") and optionally provide a reference image. If no reference is given, a default pose is used.
* **Create from concept** — Upload a concept image (e.g. artwork or sketch); the model generates 8 rotations matching that concept.
* **Rotate character** — Provide a single reference image (one direction); the model generates the other 7 directions.

## How It Works

1. Choose method: Create with style, Create from concept, or Rotate character.
2. Depending on method: enter a description, upload a concept image, or upload a reference image.
3. Set **output size** (width × height). Max 168×168 per frame.
4. Optionally set **view** (low top-down, high top-down, side) and **body type** (bipedal, quadrupedal).
5. Generate. You get 8 directional frames in one run.

## Frame Output by Output Size

* 32×32 or 48×48 or 64×64 (or 1–85px) → 8 frames (3×3 grid) — 20 generations
* 86–113px → 8 frames (3×3 grid) — 25 generations
* 114–168px → 8 frames (3×3 grid) — 40 generations

## Options

* **Method**: Create with style / Create from concept / Rotate character
* **Description**: What to generate (create with style)
* **Concept image**: Reference artwork (create from concept), max 1024×1024
* **Reference image**: Single-direction reference (rotate character), max 168×168; optional for create with style (default pose used if omitted)
* **View**: low top-down, high top-down, side
* **Output size**: Width and height per frame; max 168×168
* **Body type**: bipedal, quadrupedal
* **No background**: Transparent background (default: on)
* [Seed](/docs/options/general#seed)

## Limitations

* Output frame size max **168×168** pixels
* Reference image max **168×168**; concept image max **1024×1024**
* Requires **Tier 1** subscription
* Available in Creator and editor extensions

## Related

* [View and direction](/docs/options/character#view-and-direction) — direction order and naming
* [Rotate](/docs/tools/rotate) — non-Pro rotate tool (different pipeline)
* [Rotating a character](/docs/guides/rotating-a-character) — general guide

## On this page

[Methods](#methods)[How It Works](#how-it-works)[Frame Output by Output Size](#frame-output-by-output-size)[Options](#options)[Limitations](#limitations)[Related](#related)