Source: https://www.pixellab.ai/docs

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

Introduction

# Introduction

Welcome to the PixelLab Documentation. PixelLab offers multiple ways to generate and edit pixel art, including a simple web creator, an in-browser editor (PixelLab Pixelorama), an Aseprite extension, and API access. It is primarily aimed at artists and game developers.

While this documentation strives to be a comprehensive resource, guiding you through PixelLab's features, it is, much like our tool, a continuous work in progress. As our team develops new tools and enhances existing ones, there might be times when the documentation plays a bit of catch-up.

To get started, you can learn about the **[different ways to use PixelLab](/docs/ways-to-use-pixellab)** or explore some tool fundamentals with our **[getting started guide](/docs/getting-started)**.

We appreciate your patience and feedback as we work to refine both our tools and this reference guide. Please reach out on [Discord](https://discord.gg/pBeyTBF8T7) with any suggestions or inquiries.

---

## RIMA Reference Index

| File | Content |
|---|---|
| `mcp_docs.md` | Full MCP tool list + parameters (auto-generated from FastMCP) |
| `api-reference.md` | v2 endpoint list |
| `animate-with-text-pro.md` | Animate with Text Pro — frame table + RIMA note |
| `animate-with-skeleton.md` | Skeleton animation full workflow + generation modes |
| `edit-image-pro.md` | Edit Image Pro parameters + RIMA weapon pass scenario |
| `create-isometric-tile.md` | Create Isometric Tile parameters + RIMA note |
| `create-tiles-pro.md` | Create Tiles Pro — all tile types + RIMA scenario |
| `interpolation.md` | Interpolation tool — 64x64 limit + RIMA workarounds |

---

## RIMA Critical Findings

### 1. Interpolation — 64x64 LIMIT
Interpolation tool accepts **only 64x64** canvas. 128x128 sprites cannot use this tool directly.
**Options:** produce at 64px → scale up in Unity, or use skeleton animation instead.

### 2. Animate with Text Pro — Frame Count
| Input Size | Output | Cost |
|---|---|---|
| 32x32 or 64x64 | 16 frames (4x4 grid) | 20 gen |
| 65-128px | 4 frames (2x2 grid) | 20 gen |
| 129-170px | 4 frames (2x2 grid) | 25 gen |
| 171-256px | 4 frames (2x2 grid) | 40 gen |

64px → 16 frames; prefer 64px for smoother animation.

### 3. MCP Async Workflow
MCP tools return job ID immediately; processing takes 2-5 min in background. Poll with `get_*` tools.

### 4. MCP Setup
```
claude mcp add pixellab https://api.pixellab.ai/mcp -t http -H "Authorization: Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0"
```
Docs tip: `@https://api.pixellab.ai/mcp/docs` can be added to prompts.

## On this page