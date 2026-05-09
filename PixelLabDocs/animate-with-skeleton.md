Source: https://www.pixellab.ai/docs/tools/animate-with-skeleton

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

Animate with skeleton

# Animate with skeleton

The skeleton animation tool is used for better control over animating your character. You can use inpainting and init images to better control the output of the model.
The skeleton you create can be saved as images/aseprite files and reused for your other characters. Just make sure that they are in the "Pose - PixelLab" layer.

Accepted canvas sizes are 256x256, 128x128, 64x64, 32x32, and 16x16. It is recommended to have a canvas size that best fits the character/object you wish to animate.

## 1. Skeleton setup

We currently have two recommended ways of quickly creating skeletons, "Template skeleton" and "Animation to animation".
You can modify these manually or create skeleton animations entirely by hand using edit skeleton.

### 1.1. Template skeleton

1. Start of by selecting the character you want to animate as reference image. This is done by going to the frame where you character is and clicking "Set reference". (Set reference will automatically "estimate skeleton" and give you a skeleton for your character)
2. Edit the skeleton (shortcut -> ctrl + space + e) if needed, estimate skeleton isn't perfect and might require some touch ups.
3. Change template view and direction so it matches what you want.
4. Select the animation template you want to use.
5. Use the settings and edits to the reference skeleton to make adjustments so the template skeleton is as similar to your character as possible.
6. Once you think it's good enough insert template.
7. Go through the frames and edit skeleton where it is required.

![create skeleton from template](/docs/guides/skeleton-animation/template-skeleton.png)

![create skeleton from template](/docs/guides/skeleton-animation/template-skeleton.png)

Tip! If it's fine that the characters head doesn't move much in the animation we recommend setting fixed head -> "always" (setting found in advanced options).
This will help ensure that your characters face looks the same throughout the animation as it will copy over the head of reference skeleton to the template.

### 1.2. Animation to animation

1. Find an animation you like and place them in the same project as your reference image.
2. Set your character you want to animate as reference. This is done by going to the frame where you character is and clicking "Set reference".
   (Set reference will automatically "estimate skeleton" and give you a skeleton for your character)
3. Select the frames that contains the animation and click "Set animation". This will automatically add skeletons to the frames selected. (In the example below we have selected frames 2 -> 5)

   ![example of selecting frames](/docs/guides/skeleton-animation/select-frames.png)

   ![example of selecting frames](/docs/guides/skeleton-animation/select-frames.png)
4. Check the animation skeletons for any issues and fix them using "Edit skeleton".
5. Use the settings to change the size / position so it matches your character as best as possible. You can click show reference, to have an easier time comparing the animation to the reference skeleton.
6. Once it looks good, click "rescale", check all of the frames and "edit skeleton" if you see any issues.

![example of animation to animation](/docs/guides/skeleton-animation/animation-to-animation.png)

![example of animation to animation](/docs/guides/skeleton-animation/animation-to-animation.png)

Tip 1! If it's fine that the characters head doesn't move much in the animation
we recommend setting fixed head -> "always" (setting found in advanced options). This will help ensure that your characters
face looks the same throughout the animation as it will copy over the head of
reference skeleton to the template.

Tip 2! Animation to animation can also be used to rescale any skeletons, so if you wish to for example rescale your template skeleton you can do that as well.

## 2. Generate

### 2.1. Freeze 1 -> Generate 2 frames

Once you have the skeletons for the animation it is time to generate!

1. For the first generation it is recommended to use "Freeze 1 -> Generate 2 frames".
2. Change the direction and view so it matches the animation you are creating.
3. Set your character as reference.
4. Position yourself on the reference frame so that the model has a good frozen frame. (see image below)
5. Click "Generate"

![example of generation](/docs/guides/skeleton-animation/generate-1-frozen.png)

![example of generation](/docs/guides/skeleton-animation/generate-1-frozen.png)

## 2.1.1. Result from first generation

Animation we copied (made by user JosephT) and the result

![result from first generation](/docs/guides/skeleton-animation/generate_1_and_template.gif)

![result from first generation](/docs/guides/skeleton-animation/generate_1_and_template.gif)

### 2.2. Custom

Lets say we now want to fix the arm in the first generated frame. We can use Generation setup "Custom" which allows us to pick which frames we want to be frozen and
which we want to edit using inpainting (You can use inpainting in the other settings as well). We decide to use custom because we want to freeze frame 1, 3 and 4 because we like how they look.
(See settings below)

![example of animation to animation](/docs/guides/skeleton-animation/generate-custom.png)

![example of animation to animation](/docs/guides/skeleton-animation/generate-custom.png)

Only the arm of the character in frame two will be
edited because it has been inpainted.

## 2.2.1. Result from second generation

![result from second generation](/docs/guides/skeleton-animation/generate_2_and_template.gif)

![result from second generation](/docs/guides/skeleton-animation/generate_2_and_template.gif)

### 2.3. Freeze 2 -> Generate 1 frame

Works like generation setup "Freeze 1 -> Generate 2 frames" but instead of having one frame for guidance we now have 2.
Best to use when you have existing animation frames and you want to add more because the model will produce better results with more information.

In the following example we improvw the last frame using the previous three.

![example of animation to animation](/docs/guides/skeleton-animation/generate-3-frozen.png)

![example of animation to animation](/docs/guides/skeleton-animation/generate-3-frozen.png)

## 2.3.1. Result from third generation

![result from third generation](/docs/guides/skeleton-animation/generate_3_and_template.gif)

![result from third generation](/docs/guides/skeleton-animation/generate_3_and_template.gif)

## 3. Recommended workflow

1. Setup skeleton using previous mentioned methods
2. Set your character as a reference image by clicking on "Set reference image".
3. Set the view and direction which fits your character.
4. Position yourself so you have good frozen frames for the model to use as guidance.
5. Generate
6. Do very rough manual fixes / edit skeleton.
7. Generate new frames using the roughly fixed frames as init images. Use inpainting to decide which parts that you want to be changed.
8. Repeat from step 6 and gradually increase the init image strength as you get closer to what you want.

## 4. Tutorial Videos

## 5. Options

* [Guidance weight](/docs/options/guidance#guidance-weight) of the original image

### 5.1. Character

* [Camera view](/docs/options/guidance#camera-view)
* [Guidance weight](/docs/options/guidance#guidance-weight)

### 5.2. Color

* [Target Palette](/docs/options/color#target-palette)

### 5.3. Init image

* [Init image](/docs/options/init-image#init-image)

### 5.4. General

* [Seed](/docs/options/general#seed)

## 6. Limitations

* The canvas size must be one of the following: 256x256, 128x128, 64x64, 32x32, or 16x16.
* This feature requires at least Tier 1.

## Generation Mode Summary

| Mode | Description |
|---|---|
| Freeze 1 → Generate 2 | 1 reference frame frozen, model generates remaining |
| Custom | Manually select which frames freeze and which get inpainted |
| Freeze 2 → Generate 1 | 2 reference frames → ideal for extending existing animation |

## On this page

[1. Skeleton setup](#1-skeleton-setup)[1.1. Template skeleton](#11-template-skeleton)[1.2. Animation to animation](#12-animation-to-animation)[2. Generate](#2-generate)[2.1. Freeze 1 -> Generate 2 frames](#21-freeze-1---generate-2-frames)[2.1.1. Result from first generation](#211-result-from-first-generation)[2.2. Custom](#22-custom)[2.2.1. Result from second generation](#221-result-from-second-generation)[2.3. Freeze 2 -> Generate 1 frame](#23-freeze-2---generate-1-frame)[2.3.1. Result from third generation](#231-result-from-third-generation)[3. Recommended workflow](#3-recommended-workflow)[4. Tutorial Videos](#4-tutorial-videos)[5. Options](#5-options)[5.1. Character](#51-character)[5.2. Color](#52-color)[5.3. Init image](#53-init-image)[5.4. General](#54-general)[6. Limitations](#6-limitations)