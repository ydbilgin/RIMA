Source: https://www.pixellab.ai/docs/getting-started

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

Init images and inpainting

# Init images and inpainting

Welcome to the PixelLab documentation! This guide will help you get started with init images and inpainting using the [Create S-M image tool](/docs/tools/style_old).

## The Create S-M image tool

Open Aseprite or Pixelorama and select `Create S-M image` from the menu by first clicking `Create image >`.

*PixelLab opens automatically when you start Aseprite but if it is closed then you can open it again through the menus:*

```
Edit > PixelLab > Open plugin
```

or by pressing,

`ctrl` + `space` + `p`.

![the PixelLab menu](/docs/getting-started/pixellab-menu1.png)![the create image menu](/docs/getting-started/pixellab-menu2.png)

## Configuration

It's usually a good idea to start from an [*init image*](/docs/options/init-image). The init image is optional but it is one of the best ways to improve the results you're getting from PixelLab. You can start from a very rough sketch like this:

![a sketch of a human mage](/docs/getting-started/init-image.png)

![a sketch of a human mage](/docs/getting-started/init-image.png)

We'll use the following settings for the generation:

* `Description`: Human mage
* `Use init image`: Yes

Everything else was left as the defaults you see in the image. Let's click `Generate`!

![the create s-m image tool dialog in Aseprite](/docs/getting-started/style-dialog.png)

## The first result

The model gave the following output. If you're following along you will get another result since the generation is random.

![the generated human mage](/docs/getting-started/human-mage.png)

![the generated human mage](/docs/getting-started/human-mage.png)

The sprite is pretty good but there are definitely some parts that can be improved, it's not exactly clear what he's holding in his hand and the head looks a bit weird. Let's try to use the [inpaint tool](/docs/tools/inpaint) to fix these issues.

## Inpainting

Click `Back` and then `Cancel`to return to the main menu and then, select `Inpaint`.

This will create a new layer on top of the generated image that we can use to paint black over the parts that we want to change. The model can only modify the black parts, anything else will be left unchanged.

Let's try to fix the head first, this is how the image looks after painting over the head in the inpainting layer,

Image layerInpainting layer

![Changing the head with inpainting](/docs/getting-started/inpainting.png)![Changing the head with inpainting](/docs/getting-started/inpainting-inpaint.png)

![Changing the head with inpainting](/docs/getting-started/inpainting.png)![Changing the head with inpainting](/docs/getting-started/inpainting-inpaint.png)

Notice in the settings image that we now use our generated image as init image. We'll lower the `Init image strength` slightly which gives the model more freedom to change the image. We'll set the output method to `Modify current layer` so that the changes are applied directly to the image. Then we'll click `Generate` again.

This time, the output looks like this:

![The mage with the head fixed](/docs/getting-started/fixed-head.png)

![The mage with the head fixed](/docs/getting-started/fixed-head.png)

![use inpainting selected in the dialog](/docs/getting-started/use-inpainting.png)

## Init image and inpainting

Ok, now that we're satisfied with the head let's try to fix the hand as well. This time let's combine both *inpainting* and *init image*.

We'll modify the image slightly and then paint over the area around the hand in the inpainting layer. After drawing a rough sketch of a fireball hovering above the character's hand and then inpainting the area it should look something like this:

Image layerInpainting layer

![Changing the head with inpainting](/docs/getting-started/inpaint-plus-init.png)![Changing the head with inpainting](/docs/getting-started/inpaint-plus-init-inpaint.png)

![Changing the head with inpainting](/docs/getting-started/inpaint-plus-init.png)![Changing the head with inpainting](/docs/getting-started/inpaint-plus-init-inpaint.png)

![Setting for inpainting and init image](/docs/getting-started/init-and-inpaint-settings.png)

## The final result

After clicking *Generate* again we get the following output:

![The final mage](/docs/getting-started/final-mage.png)

![The final mage](/docs/getting-started/final-mage.png)

You can continue iterating like this as many times as you want until you are happy with the result.

## On this page

[The Create S-M image tool](#the-create-s-m-image-tool)[Configuration](#configuration)[The first result](#the-first-result)[Inpainting](#inpainting)[Init image and inpainting](#init-image-and-inpainting)[The final result](#the-final-result)