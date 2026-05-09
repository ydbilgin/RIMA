Source: https://www.pixellab.ai/docs/tools/inpaint

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

Inpaint

# Inpaint

The Inpaint tool allows you to edit specific parts of existing images. For example, you can use it to add a weapon to a character, change a character's outfit, or remove elements from a scene. It is also excellent for creating style-consistent characters, check out this [tutorial](/docs/tools/inpaint#tutorials).

When you select **Inpaint** from the PixelLab menu, a new layer called **Inpainting** will be added to your image. Drawing on this layer specifies which parts of your image the model is allowed to modify.

## Tutorials

For a written tutorial, see the [getting started guide](/docs/getting-started).

## Options

* [Paint in selection](/docs/options/general#paint-in-selection)

### Guidance

* [Description](/docs/options/guidance#description)
* [Negative description](/docs/options/guidance#negative-description)
* [Use view and direction](/docs/options/camera#use-view-and-direction)
* [Outline](/docs/options/outline) / [Shading](/docs/options/shading) / [Details](/docs/options/details)
* [Isometric](/docs/options/projection#isometric) / [Oblique projection](/docs/options/projection#oblique-projection)
* [Guidance weight](/docs/options/guidance#guidance-weight)

### Initial image

* [Initial image](/docs/options/init-image#init-image)

### Color

* [Target Palette](/docs/options/color#target-palette)

### General

* [Output method](/docs/options/general#output-method)
* [Seed](/docs/options/general#seed)

## Limitations

* The minimum inpainting size is 32x32 pixels.
* The maximum inpainting size is 100x100 pixels for tier 1 and 160x160 pixels for tier 2+.

## On this page

[Tutorials](#tutorials)[Options](#options)[Guidance](#guidance)[Initial image](#initial-image)[Color](#color)[General](#general)[Limitations](#limitations)