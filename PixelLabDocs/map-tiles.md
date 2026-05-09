Source: https://www.pixellab.ai/docs/guides/map-tiles

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

guides

Using the Create Map tool

# Using the Create Map tool

In this guide, we'll generate a scene inside a cave, with stairs leading up to a treasure chest. We'll constrain ourselves to a 128x128 pixel image. Since the model is trained on 16x16 pixel tiles, this will give us an area that has a width and height of 8 by 8 tiles.

## Setting up the scene

First, let's open the [Create Map tool](/docs/tools/map-tiles). It's always a good idea to provide an [init image](/docs/options/init-image#init-image) together with the [text description](/docs/options/guidance#description) of what we want our selected area to look like. We'll start with this:

![Create Map init image](/docs/guides/map-tiles/start-init.png)

![Create Map init image](/docs/guides/map-tiles/start-init.png)

## Tell the model what to generate

We'll use the setting [*paint in selection*](/docs/options/general#paint-in-selection) and select a 4 by 4 area of tiles using the selection tool.

The model uses [inpainting](/docs/tools/inpaint). Since this is the first part of the image, we want the model to generate everything inside the selection. To achieve this, we'll fill the inpaint layer with black inside the selection.

Your canvas should now look something like this.

Image layerInpainting layer

![The initial image with the inpaint layer filled with black](/docs/guides/map-tiles/step1.png)![The initial image with the inpaint layer filled with black](/docs/guides/map-tiles/step1-inpaint.png)

![The initial image with the inpaint layer filled with black](/docs/guides/map-tiles/step1.png)![The initial image with the inpaint layer filled with black](/docs/guides/map-tiles/step1-inpaint.png)

We'll use *"stairs in a cave"* as the [description](/docs/options/guidance#description) and press generate.

### Note

The 4 by 4 tile selection tells the model what part of the map to generate.

The inpaint layer tells the model where, inside the selection, it can modify the image.

![Settings of the map tool before generating](/docs/guides/map-tiles/step1-options.png)

## The first generation

After a few retries, the model produced this result, which seems like a decent starting point.

![Result of the first generation](/docs/guides/map-tiles/step1-result.png)

![Result of the first generation](/docs/guides/map-tiles/step1-result.png)

The first result does not have to be perfect, as many things can be fixed later with inpainting, but it's good to have something that is close to the style we're looking for.

### Note

Because of how the model has been trained, to produce the best results, the *description* should describe what's in the **middle** of the selected area.

## Expanding the image

We'll now expand our generated image. Let's start by expanding it to the right.

1. Select an area that partly overlaps with what we have already generated.
2. Draw a rough sketch inside the selection.
3. Draw black in the inpaint layer where we want to generate.

Image layerInpainting layer

![Result of the first generation](/docs/guides/map-tiles/step2.png)![Result of the first generation](/docs/guides/map-tiles/step2-inpaint.png)

![Result of the first generation](/docs/guides/map-tiles/step2.png)![Result of the first generation](/docs/guides/map-tiles/step2-inpaint.png)

### Note

The model can only see what is **inside the selection** when generating. If we want to have a consistent style and smooth transitions between tiles, it is important that we **don't inpaint the whole selection area**, as that leaves no reference for the model to use.

## Generating the new area

Next, we'll update the [description](/docs/options/guidance#description), adjust the [init image strength](/docs/options/init-image#init-image-strength), and then generate the new area. You can see the values used in the picture.

![The settings used when generating the new area](/docs/guides/map-tiles/expand-settings.png)

## Drawing the rest of the map

We can repeat steps 4 and 5 to fill out the rest of the tiles. Below is an animation of the process.

Image layerInpainting layer

![Animation of the process of drawing the rest of the map](/docs/guides/map-tiles/step3.gif)![Animation of the process of drawing the rest of the map](/docs/guides/map-tiles/step3-inpaint.gif)

![Animation of the process of drawing the rest of the map](/docs/guides/map-tiles/step3.gif)![Animation of the process of drawing the rest of the map](/docs/guides/map-tiles/step3-inpaint.gif)

## Final touches

After the entire map has been generated, we can use [inpainting](/docs/tools/inpaint) to add detail and fix anything we don't like. The workflow is the same as before. For our image, the following changes were made.

Image layerInpainting layer

![Animation of the process of the final touches](/docs/guides/map-tiles/step4.gif)![Animation of the process of the final touches](/docs/guides/map-tiles/step4-inpaint.gif)

![Animation of the process of the final touches](/docs/guides/map-tiles/step4.gif)![Animation of the process of the final touches](/docs/guides/map-tiles/step4-inpaint.gif)

## The final result

![The final result](/docs/guides/map-tiles/step5.png)

![The final result](/docs/guides/map-tiles/step5.png)

## On this page

[Setting up the scene](#setting-up-the-scene)[Tell the model what to generate](#tell-the-model-what-to-generate)[The first generation](#the-first-generation)[Expanding the image](#expanding-the-image)[Generating the new area](#generating-the-new-area)[Drawing the rest of the map](#drawing-the-rest-of-the-map)[Final touches](#final-touches)[The final result](#the-final-result)