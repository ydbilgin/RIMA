Source: https://www.pixellab.ai/docs/tools/animation

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

Animation with text

# Animation with text

We recommend using the [Animate with skeleton](/docs/tools/skeleton-animation) tool for characters.

The Animation with text tool allows you to generate 4 animation frames that depict various types of animations for a character.

It allows you to condition the creation on existing character images:

* Condition on a single idle character and generate 3 animation frames.
* Condition on 3 animation frames and create a single new frame that better matches the flow of the animation.
* Condition on images in the first and last positions and generate 2 frames between that interpolate the animation.

Recommended workflow:

1. Use an existing reference image where the character is looking in the same direction as you want it to look during the animation.
2. (Optional) Modify an existing animation as a rough init image. Try to color match approximately with the reference image.
3. Select a simple action prompt like "run" or "jump".
4. Generate 3 new frames from the reference image a few times. Pick out the frames you like and reorder them appropriately to create a convincing animation.
5. Do rough manual fixes to the frames.
6. Generate new frames using the roughly fixed frames as init images. Use inpainting to freeze the parts that you do not want to be changed.
7. Repeat from step 4 and gradually increase the init image strength as you get closer to what you want.

This is demonstrated here in a tutorial showing how to create a running animation using a template:

Tutorial on how to create a walking animation without a template:

## Attack animations

Recommended workflow for attack animations:

Generating attack animations works similarly to generating movement with the exception that if you want to create large special effects,
it is recommended to move your character to the side of the canvas so the model has more space to create special effects.

Here you can find a demo of generating attack animations:

## Extra demos

Note that these demo videos are from an earlier version of the animation tool that was weaker when it did not support inpainting.

## Options

* [Number of reference images](/docs/options/general)
* [Guidance weight](/docs/options/guidance#guidance-weight) of the original image.

### Character

* [Character description](/docs/options/character#character-description)
* [Negative description](/docs/options/guidance#negative-description)
* [Camera view](/docs/options/guidance#camera-view)
* [Direction](/docs/options/character#view-and-direction)

### Action

* [Action description](/docs/options/animation#action-description)
* [Guidance weight](/docs/options/guidance#guidance-weight) for [Action description](/docs/options/animation#action-description)

### Color

* [Target Palette](/docs/options/color#target-palette)

### Init image

* [Init image](/docs/options/init-image#init-image)

### General

* [Seed](/docs/options/general#seed)

## Limitations

* The canvas size must be exactly 64x64.
* This feature requires at least Tier 1.

## On this page

[Attack animations](#attack-animations)[Extra demos](#extra-demos)[Options](#options)[Character](#character)[Action](#action)[Color](#color)[Init image](#init-image)[General](#general)[Limitations](#limitations)