Source: https://www.pixellab.ai/docs/ways-to-use-pixellab

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

Ways to Use PixelLab

# Ways to Use PixelLab

PixelLab offers several ways to access its image generation and editing tools, catering to different workflows and preferences. Whether you're experimenting on your phone, building a game with AI assistance, or integrating into your pro workflow, there's an option that fits.

## Overview

1. **Simple Web Creator** — Quick, lightweight tool for fast generations. Works on desktop and mobile.
2. **Characters (NEW)** — Instant character creator for 4/8 directional animated sprites.
3. **PixelLab Pixelorama (In-browser Editor)** — Full-featured browser-based editor using the open-source Pixelorama.
4. **Aseprite Extension** — Bring PixelLab tools directly into your local Aseprite environment.
5. **Vibe Coding (AI Agent Toolkit)** — Give your AI coding assistant pixel art superpowers via MCP.
6. **API Access** — Use PixelLab's backend tools in your own app or pipeline.
7. **Videos** — demos and tutorials

---

## 1. Simple Web Creator

Our Simple Web Creator is the fastest way to generate images. Runs entirely in your browser and supports mobile devices.

* **Access:** Visit the [Create Page](/create).
* **Features:** Uses our "PixFlux" (for medium to extra-large images) and "BitForge" (for small to medium images) models.
* **Best for:** Quick experiments, base image generation, or working on a mobile device.
* **Limitations:** Streamlined interface with fewer features than the editor integrations.

---

## 2. Characters

Create game-ready characters with 4 or 8 directional views and custom animations — all from simple text descriptions.

* **Access:** Visit the [Characters Page](/create-character).
* **Features:**
  + Generate characters in 4 or 8 directions from text prompts
  + Add walk, run, idle, and other animations with one click
  + Export as sprite sheets or individual frames
  + Built-in animation preview
* **Best for:** Game developers needing quick character assets, RPG makers, and anyone creating directional sprites.
* **Platform:** Works on desktop and mobile browsers.

---

## 3. PixelLab Pixelorama (In-browser Editor)

Pixelorama is a free, open-source pixel art editor. We've integrated PixelLab directly into it for a full editing + AI generation experience — all in your browser.

* **Access:** Open the [In-browser Editor](/editor).
* **Features:** Full Pixelorama toolset + PixelLab's AI generation tools.
* **Platform:** Desktop browsers only.
* **Best for:** Users wanting an advanced browser-based editor with full creation + generation features.
* **Limitations:** No support for mobile browsers.

---

## 4. Aseprite Extension

Use PixelLab inside Aseprite, the professional-grade pixel art tool.

* **Access:**
  + Download from your [Account Page](/account#tools).
  + Follow the [Installation Guide](/docs/installation).
* **Features:** Seamless AI-powered generation inside your Aseprite workflow.
* **Requirements:** Aseprite v1.3+. Trial version does not support extensions.
* **Best for:** Existing Aseprite users who want native integration.
* **Limitations:** Requires Aseprite and local installation.

---

## 5. Vibe Coding (AI Agent Toolkit)

Enable AI-powered game development by giving your coding assistant direct access to PixelLab's generation tools through the Model Context Protocol (MCP).

* **Access:** Visit the [MCP Integration Page](/mcp) for setup instructions.
* **Features:**
  + Generate characters, animations, tilesets, and isometric tiles from your IDE
  + Works with Claude Code, Cursor, VS Code, and other MCP-compatible AI assistants
  + Godot-specific tooling for headless game development
  + Complete asset-to-code workflow automation
* **Best for:** Developers using AI assistants, rapid game prototyping, "vibe coding" workflows.
* **Requirements:** MCP-compatible AI assistant and active subscription.

---

## 6. API Access

Use PixelLab in your own software, games, or pipelines through our API.

* **Access:**
  + Directly via the [API Docs](https://api.pixellab.ai/v1/docs).
  + For Python users, we offer a [Python SDK](https://github.com/pixellab-code/pixellab-python) to simplify integration.
* **Features:** Programmatic access to image generation (PixFlux, BitForge), animation (skeleton, text), inpainting, rotation, and more
* **Best for:** Automation, dynamic in-game asset creation, and custom tool development.
* **Limitations:** Requires setup and coding knowledge.

---

## 7. Videos

Check out our [YouTube channel](https://www.youtube.com/@PixelLab_AI) for tutorials, demos, and behind-the-scenes looks at how to use PixelLab in various ways.

## On this page

[Overview](#overview)[1. Simple Web Creator](#1-simple-web-creator)[2. Characters](#2-characters)[3. PixelLab Pixelorama (In-browser Editor)](#3-pixellab-pixelorama-in-browser-editor)[4. Aseprite Extension](#4-aseprite-extension)[5. Vibe Coding (AI Agent Toolkit)](#5-vibe-coding-ai-agent-toolkit)[6. API Access](#6-api-access)[7. Videos](#7-videos)