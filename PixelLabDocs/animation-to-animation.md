Source: https://www.pixellab.ai/docs/tools/animation-to-animation

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

Animation to animation

# Animation to animation

The **Animation to animation** tool allows you to generate animation frames based on your descriptions and preferences. This powerful tool creates consistent character animations by generating multiple frames that maintain character appearance and style throughout the sequence.

## 1. Opening the Animation Window

![Animation2AnimationMbutton](/docs/tools/animation-to-animation/Animation2AnimationMM.png)

![Animation2AnimationMbutton](/docs/tools/animation-to-animation/Animation2AnimationMM.png)

To get started with the "Animation to animation" feature, the first thing you'll do is open the Animation to Animation window.

## 2. Setting Up Your Animation

![Animation2AnimationMenu](/docs/tools/animation-to-animation/A2AMENU.png)

![Animation2AnimationMenu](/docs/tools/animation-to-animation/A2AMENU.png)

Inside the window, you'll find several settings to configure your animation.

### 2.1. Number of Images

At the top, you'll see a slider labeled **Number of images**. Use this to set how many frames you want the tool to generate for your animation sequence in one go. Keep in mind that the maximum number of frames you can generate at once might depend on your canvas size.

![Animation2Animation#img](/docs/tools/animation-to-animation/Imageamnt.png)

![Animation2Animation#img](/docs/tools/animation-to-animation/Imageamnt.png)

### Starting Small

It's recommended to start with as few frames as possible (typically 2) to cycle through good references faster and establish a consistent character appearance.

### 2.2. Animation Reference

Below that, you'll see the **Animation reference** section. This is where you can choose to use an existing reference image or an init image. This is handy if you have a specific look or character you want to start with. We'll touch on using a generated frame as a reference later to help keep things consistent.

![Animation reference settings](/docs/tools/animation-to-animation/animationrefarea.png)

![Animation reference settings](/docs/tools/animation-to-animation/animationrefarea.png)

![Animation reference settings](/docs/tools/animation-to-animation/initrefchecksa2a.png)

![Animation reference settings](/docs/tools/animation-to-animation/initrefchecksa2a.png)

### 2.3. Description

Next is the **Description** field. Type in a description of the character or the art style you want to animate. For example, "man with a straw hat and blue overalls" as shown in the image.

![Character description field](/docs/tools/animation-to-animation/desca2a.png)

![Character description field](/docs/tools/animation-to-animation/desca2a.png)

### 2.4. Action Description

In the **Action description** field, briefly describe the movement or action you want the character to perform, like "walk".

![Action description field](/docs/tools/animation-to-animation/actiona2a.png)

![Action description field](/docs/tools/animation-to-animation/actiona2a.png)

### 2.5. Camera View

Select the perspective or camera view for your animation from the dropdown menu.

![Camera view selection](/docs/tools/animation-to-animation/cameraa2a.png)

![Camera view selection](/docs/tools/animation-to-animation/cameraa2a.png)

### 2.6. Direction

Choose the direction of the action if applicable.

![Direction selection](/docs/tools/animation-to-animation/directiona2a.png)

![Direction selection](/docs/tools/animation-to-animation/directiona2a.png)

### 2.7. Appearance Settings

You can adjust the appearance of your animation here by picking options for outline, shading, and details. Feel free to experiment with these settings like "selective outline", "basic shading", and "medium detail" to see what fits your style best.

![Outline, shading, and details settings](/docs/tools/animation-to-animation/apperance2a.png)

![Outline, shading, and details settings](/docs/tools/animation-to-animation/apperance2a.png)

### 2.8. Advanced Options

Under the **Advanced Options**, you can fine-tune how the AI generates the animation:

## 2.8.1. AI Freedom / Robust

* **AI freedom**: Influences how much creative latitude the AI takes during generation.
* **AI freedom (robust)**: Similar to the above, but less strict when following the template. This may offer more robust control over AI variability.
* **Guidance Weight**: This controls how closely the AI sticks to your description. Adjusting these helps balance adherence to your input versus allowing the AI to add its own variations.

![Guidance weight settings](/docs/tools/animation-to-animation/robustfreedomguidance.png)

![Guidance weight settings](/docs/tools/animation-to-animation/robustfreedomguidance.png)

## 3. Generating Your Animation

Once you've set up all your preferences:

1. Click the **Generate** button at the bottom of the window
2. Start with as few frames as possible to cycle through good references faster. (least amount would be **2**)
3. The tool will process your settings and generate the specified number of frames based on your input

![Generate button](/docs/tools/animation-to-animation/numgena2a.png)

![Generate button](/docs/tools/animation-to-animation/numgena2a.png)

## 4. Maintaining Consistency with a Reference

When using Animation to Animation with no reference, in a perfect world after your first generation, you might get a frame you really like. To help ensure subsequent sets of frames stay consistent with that look:

1. **Take one of the generated frames** that you like
2. **Go back to the Animation reference section**
3. **Set that preferred frame as your reference image** by checking the "Use reference image" box and "setting" your frame image there as a reference image
4. This helps the tool keep the character and style consistent as you generate more frames

![Setting reference image from generated frame](/docs/tools/animation-to-animation/framerefa2a.png)

![Setting reference image from generated frame](/docs/tools/animation-to-animation/framerefa2a.png)

![Setting reference image from generated frame](/docs/tools/animation-to-animation/refa2agen.png)

![Setting reference image from generated frame](/docs/tools/animation-to-animation/refa2agen.png)

![Animation to Animation generating reference frame](/docs/tools/animation-to-animation/genninga2a.png)

![Animation to Animation generating reference frame](/docs/tools/animation-to-animation/genninga2a.png)

**Repeat the generation process** as needed, using the reference image to build out your full animation sequence

### Manual Cleanup

While the tool gets you most of the way there, expect that you might need to do some manual cleanup on the frames afterward.

## 5. Workflow Tips

### 5.1. Starting Your Animation

1. **Begin with minimal frames**: Start with 2 frames to establish your character
2. **Use clear descriptions**: Be specific about character appearance and actions
3. **Experiment with settings**: Try different appearance options to find your style

### 5.2. Building Your Sequence

1. **Find your reference frame**: After the first generation, identify the frame you like best
2. **Set it as reference**: Use that frame as your reference image for consistency
3. **Generate more frames**: Continue generating with the reference to build your sequence
4. **Iterate and refine**: Make adjustments to descriptions and settings as needed

### 5.3. Final Polish

1. **Review all frames**: Check for consistency across your animation
2. **Manual adjustments**: Use editing tools to fix any issues
3. **Test the animation**: Play through your sequence to ensure smooth motion

## 6. Best Practices

1. **Start simple**: Begin with basic actions like "walk" or "idle"
2. **Be descriptive**: Provide detailed character descriptions for better results
3. **Use references**: Always set a reference image after finding a frame you like
4. **Iterate gradually**: Make small changes and test frequently
5. **Plan your sequence**: Think about the full animation before starting

## 7. Common Use Cases

* **Character walking animations**: Create smooth walking cycles
* **Idle animations**: Add subtle movement to static characters
* **Action sequences**: Generate attack, jump, or other action animations
* **Character variations**: Create different versions of the same character

## 8. Related Tools

* **[Animation with Text](/docs/tools/animation)**: Alternative animation method using text prompts
* **[Skeleton Animation](/docs/tools/skeleton-animation)**: Advanced animation with skeleton-based control
* **[Interpolation](/docs/tools/interpolation)**: Add intermediate frames between keyframes
* **[Rotate Tool](/docs/tools/rotate)**: Create character rotations for multi-directional animations

## 9. Result Preview

![Animation to Animation results](/docs/tools/animation-to-animation/resultsa2a.gif)

![Animation to Animation results](/docs/tools/animation-to-animation/resultsa2a.gif)

## 10. Tutorial Videos

## 11. Limitations

* Requires at least **Tier 2** subscription
* Canvas size must be one of:128x128, 64x64, 32x32, or 16x16
* Maximum number of frames per generation depends on canvas size
* Quality depends on the clarity of your descriptions and original Animation quality
* May require manual cleanup for perfect results

## On this page

[1. Opening the Animation Window](#1-opening-the-animation-window)[2. Setting Up Your Animation](#2-setting-up-your-animation)[2.1. Number of Images](#21-number-of-images)[2.2. Animation Reference](#22-animation-reference)[2.3. Description](#23-description)[2.4. Action Description](#24-action-description)[2.5. Camera View](#25-camera-view)[2.6. Direction](#26-direction)[2.7. Appearance Settings](#27-appearance-settings)[2.8. Advanced Options](#28-advanced-options)[2.8.1. AI Freedom / Robust](#281-ai-freedom--robust)[3. Generating Your Animation](#3-generating-your-animation)[4. Maintaining Consistency with a Reference](#4-maintaining-consistency-with-a-reference)[5. Workflow Tips](#5-workflow-tips)[5.1. Starting Your Animation](#51-starting-your-animation)[5.2. Building Your Sequence](#52-building-your-sequence)[5.3. Final Polish](#53-final-polish)[6. Best Practices](#6-best-practices)[7. Common Use Cases](#7-common-use-cases)[8. Related Tools](#8-related-tools)[9. Result Preview](#9-result-preview)[10. Tutorial Videos](#10-tutorial-videos)[11. Limitations](#11-limitations)