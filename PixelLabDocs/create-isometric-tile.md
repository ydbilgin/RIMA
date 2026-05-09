Source: https://www.pixellab.ai/docs/tools/create-isometric-tile

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

Create isometric tile

# Create isometric tile

![Setting reference image from generated frame](/docs/tools/isometric/menu.png)

![Setting reference image from generated frame](/docs/tools/isometric/menu.png)

The **Create Tileset - Isometric Tool** generates isometric tilesets based on your init image or text descriptions.

![Setting reference image from generated frame](/docs/tools/isometric/menuwindow.png)

![Setting reference image from generated frame](/docs/tools/isometric/menuwindow.png)

## Options

### 1. Isometric Tile Shape

This dropdown menu allows you to select the fundamental shape of your isometric tile.

![Setting reference image from generated frame](/docs/tools/isometric/shapes.png)

![Setting reference image from generated frame](/docs/tools/isometric/shapes.png)

**Options:**

* **Thick**: Creates tiles with a thicker, more substantial appearance
* **Thin**: Produces tiles with a thinner, more delicate look
* **Block**: Generates blocky, cubic-style tiles
* **Reference**: This option utilizes a reference image to define the tile's specific shape, shading, and other visual characteristics. You can use the "Set" and "Clear" buttons to manage your reference image.
* **None**: No specific shape constraint applied

When a shape is selected, the preview area at the top of the dialog updates to reflect the chosen type.

### 2. Description

The Description text field is where you provide a textual prompt for the desired appearance of your tile.

![Setting reference image from generated frame](/docs/tools/isometric/description.png)

![Setting reference image from generated frame](/docs/tools/isometric/description.png)

**Purpose:** Tell the model what the tile should look like (e.g., "grass on top of dirt", "rocky path", "wooden floor").
**Detail:** Descriptions can range from simple to highly detailed.
**Creativity:** Feel free to be creative with your descriptions to achieve unique results.

### 3. Outline/Shading/Details

These dropdown menus offer granular control over the visual style of the generated tile. Experiment with different combinations to find the best fit for your project.

![Setting reference image from generated frame](/docs/tools/isometric/osd.png)

![Setting reference image from generated frame](/docs/tools/isometric/osd.png)

* [Outline](/docs/options/outline): Controls the tile's border definition
* [Shading](/docs/options/shading): Determines the lighting and shadow effects
* [Details](/docs/options/details): Influences the level of intricate visual elements

### 4. Tile Size

Select the desired resolution for your isometric tile. The Tile Size does not determine the canvas size, it influences the art style. Choose a size that best fits your canvas or project requirements for optimal detail and readability.

![Setting reference image from generated frame](/docs/tools/isometric/tilesize.png)

![Setting reference image from generated frame](/docs/tools/isometric/tilesize.png)

**Options:**

* **32x32**: Recommended for most isometric tilesets, provides optimal detail and readability
* **16x16**: Smaller tiles, good for retro-style games or when space is limited

### 5. Init Image

The [Init Image](/docs/options/init-image) setting plays a crucial role in guiding the generation process by providing an initial visual reference.

![Setting reference image from generated frame](/docs/tools/isometric/init.png)

![Setting reference image from generated frame](/docs/tools/isometric/init.png)

**Purpose:** Helps define the **shape and overall appearance** of the generated tile.
**Influence:**

* **Higher value**: The generated tile will more closely resemble the provided initial image.
* **Lower value**: The generated tile will less closely resemble the initial image, allowing the model more creative freedom based on the description.

**Recommendation:** Experiment with different values to find what works best for achieving your desired end result, as the Init Image helps push the model towards your vision.

### 6. Advanced Options

Clicking "Advanced Options" reveals additional settings for fine-tuning the tile generation.

![Setting reference image from generated frame](/docs/tools/isometric/guidance.png)

![Setting reference image from generated frame](/docs/tools/isometric/guidance.png)

**[Guidance Weight](/docs/options/guidance#guidance-weight)**

* This parameter controls how strongly the model adheres to your text description.
* Higher values will push the model to generate an image that aligns more precisely with your descriptive text.
* It also impacts the outline, shading, details in the generated output.

**Load Previous Settings:**

* This feature allows you to reload previously used [Seed](/docs/options/general#seed) or other generation parameters from a `.json` file. This is useful for reproducing past results or iterating on existing designs.

  ![Setting reference image from generated frame](/docs/tools/isometric/json.png)

  ![Setting reference image from generated frame](/docs/tools/isometric/json.png)

### Color

* [Target palette](/docs/options/color#target-palette)

### General

* [Output method](/docs/options/general#output-method)

## RIMA Notu

Playbook'ta Adım 1-3 için kullandığımız tool budur (W1/W2/OBW duvarları). MCP üzerinden `create_isometric_tile` ile de çağrılabilir. Tutarlı stil için aynı seed kullan.

## Tutorial

## On this page

[Options](#options)[1. Isometric Tile Shape](#1-isometric-tile-shape)[2. Description](#2-description)[3. Outline/Shading/Details](#3-outlineshadingdetails)[4. Tile Size](#4-tile-size)[5. Init Image](#5-init-image)[6. Advanced Options](#6-advanced-options)[Color](#color)[General](#general)[Tutorial](#tutorial)