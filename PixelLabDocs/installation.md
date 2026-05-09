Source: https://www.pixellab.ai/docs/installation

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

Aseprite Extension Installation Instructions

# Aseprite Extension Installation Instructions

## Get Aseprite

Make sure you have the latest Aseprite (minimum version is v1.3.7). The trial version does not allow plugins.

Aseprite is open source, but it is not allowed to be redistributed. There are multiple ways to get it. You can, for example:

* [Buy it to support the developers](https://www.aseprite.org/download/)
* [Use a helper to build it locally (Windows)](https://github.com/Perndoe/Aseductor)
* [Build it with Docker (Linux)](https://github.com/pixellab-code/docker-aseprite-linux)

## Download the extension

After subscribing or signing up for the free trial, there will be a button to download the PixelLab extension from your [account page](/account).

## Install the extension

You should be able to double click the downloaded file to install it. If you can't, you can also install it manually.

First, open Aseprite. From the menu bar, go to,

```
Edit > Preferences
```

Choose *extensions* in the list to the left, then click on "Add Extension" and locate the file that you downloaded in [the first step](/docs/installation#download-the-extension).

![The preferences window with the extensions tab selected](/docs/installation/add-extension.png)

## Allow file and internet access

After finishing the steps above restart Aseprite and you should be prompted to give the plugin access to a file called `package.json` and internet access via websockets.

The plugin requires access to these and if you want the auto-update function to work you also need to give the plugin "full trust" so that it can overwrite itself in the future.

![File access](/docs/installation/file-access.png)![Websocket access](/docs/installation/websocket-access.png)

## Finish the installation

You should now see a new window, the PixelLab menu. If you don't see the window or if you accidentally closed it, you can open it from the menu bar,

```
Edit > PixelLab > Open plugin
```

or by pressing,

`ctrl` + `space` + `p`.

![The PixelLab menu](/docs/installation/pixellab-menu.png)

## On this page

[Get Aseprite](#get-aseprite)[Download the extension](#download-the-extension)[Install the extension](#install-the-extension)[Allow file and internet access](#allow-file-and-internet-access)[Finish the installation](#finish-the-installation)