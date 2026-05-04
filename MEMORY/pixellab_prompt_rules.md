---
name: PixelLab prompt rules
type: feedback
trigger: PixelLab prompt, edit image, prompt boilerplate
description: Short prompt rules for PixelLab edit/generation tasks
---

## Edit Image Prompts
Describe only the target change. Do not add "keep X unchanged" boilerplate -- it adds noise.

## Prompt Techniques (confirmed by PixelLab staff + community)
- Numbered list for item batches: "Various weapons and tools: 1. Longsword 2. War hammer" -- forces specific output
- Reference Pro consistency: "Follow the exact reference image's 2D Fantasy RPG spritesheet layout and style, and generate: [item]" with 2-3 reference images
- Give HEX color palette to Pro tools: paste your palette hex codes into the prompt -- significant consistency improvement (staff confirmed)
- Background: generate background separately, composite sprite on top -- never mix in one generation
- Inpaint context trick: place existing sprite on LEFT of canvas, inpaint on RIGHT -- model uses left as style context
- Animate with Text strong verbs: "running fast, alternating arms and legs as they lift their knees" / "blue fire burning, swirling flames, magic energy" / "swirling vortex, imploding energy, purple magic"
- Style reference: describe WHAT to generate, not the style -- "medieval weapons" not "pixel art style"
- Style reference count: 5-10 focused images > 60 random images

## bitforge model
AVOID. Old hidden model. Staff confirmed: "this one is a bit stupid." Use Pro tools instead.
