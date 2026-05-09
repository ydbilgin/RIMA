Source: https://www.pixellab.ai/docs/tools/interpolation

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

Interpolation

# Interpolation

The Interpolation tool generates intermediate frames between two keyframes in an animation.

## Options

* Intermediate [Guidance weight](/docs/options/guidance#guidance-weight)
* Reference [Guidance weight](/docs/options/guidance#guidance-weight)

### Character

* [Character description](/docs/options/character#character-description)
* [Negative description](/docs/options/character#negative-description)
* [Action description](/docs/options/character#action-description)
* [Camera view](/docs/options/guidance#camera-view)
* [Direction](/docs/options/character#view-and-direction)
* Character options [Guidance weight](/docs/options/guidance#guidance-weight)

### Color

* [Target Palette](/docs/options/color#target-palette)

### General

* [Seed](/docs/options/general#seed)

## Limitations

* The canvas must be exactly 64x64 pixels.

## RIMA — Kritik Uyarı

128x128 sprite → interpolation çalışmaz. Production Playbook Adım 21c (Walk Cycle) ve Adım 22b/c (Attack) interpolation pipeline kullanıyor — 128px karakter sprite'ları bu tool ile doğrudan çalışmaz.

**Çözüm Seçenekleri:**
1. Karakter üretimini 64x64'te yap → 16 frame (daha iyi animasyon) → Unity'de scale up
2. 128x128 için skeleton animation kullan (interpolation yerine)
3. 64x64 çalışma → Aseprite'ta 2x nearest-neighbor upscale → 128x128

## Workflow

```
1. A Keyframe üret (extreme pose)
2. B Keyframe üret (opposite extreme)
3. Interpolate: A → B → smooth ara frame'ler
4. Sonuç: akıcı walk/attack cycle
```

## On this page

[Options](#options)[Character](#character)[Color](#color)[General](#general)[Limitations](#limitations)