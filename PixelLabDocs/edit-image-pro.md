Source: https://www.pixellab.ai/docs/tools/edit-image-pro

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

Edit image (Pro)

# Edit image (Pro)

The Edit image (Pro) tool edits a single pixel art image using AI—either from a text instruction or by matching a reference image. Use it to change, add, or remove details (e.g. "give the character a hat") or to transfer the look of a reference onto your image.

## Methods

* **Edit with text** — Describe the edit (e.g. "add a sword", "change shirt to red"). You can optionally add a reference image to guide style.
* **Edit with reference** — Provide a reference image; the tool edits your image to match that reference's appearance.

## How It Works

1. Add the image you want to edit.
2. Choose **Edit with text** or **Edit with reference**.
3. For text: enter a description of the change. For reference: add the reference image.
4. Set output size (same as input or your choice within limits).
5. Generate. You get one edited image per run.

## Frame Output by Output Size

* 1-256px → 1 frame — 20 generations
* 257-314px → 1 frame — 25 generations
* 315-512px → 1 frame — 40 generations

## Options

* **Method**: Edit with text / Edit with reference
* **Description**: What to change (edit with text)
* **Reference image**: Target look (edit with reference), or optional style hint (edit with text)
* **Output size**: Width and height; min 32×32, max 256×256 (tier 1) or larger for tier 2+
* **No background**: Transparent background
* [Seed](/docs/options/general#seed)

## Limitations

* Single image per run (no multi-frame grid).
* Output size at least 32×32; max 256×256 for tier 1 (tier 2+ can allow larger).
* Requires paid subscription (tier 1 or above).

## RIMA Kullanım Senaryosu

Production Playbook Adım 25/34/43 — Weapon Pass: Warblade/Ranger/Shadowblade body-only sprite'lara silah eklemek için bu tool kullanılır.

## Related

* [Edit image](/docs/tools/edit-image) — non-Pro edit (inpainting-style)
* [Edit animation (Pro)](/docs/tools/edit-animation-pro) — edit multiple frames at once
* [Transfer outfit (Pro)](/docs/tools/transfer-outfit-pro) — apply outfit from reference to animation frames

## On this page

[Methods](#methods)[How It Works](#how-it-works)[Frame Output by Output Size](#frame-output-by-output-size)[Options](#options)[Limitations](#limitations)[Related](#related)