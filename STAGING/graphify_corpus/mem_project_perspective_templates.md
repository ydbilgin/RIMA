---
name: Perspective Templates Research
description: Geometry locking for High Top-Down. Rules and terms.
type: project
---

# GEOMETRY RULES
* Shadow Locking: 1px Contact Shadow template -> Concept Image
* Cubic Reference: 64x64 Gray cube + "Use gray cube as geometry ref"
* Wall Height: 2 tiles (128px total, 64px front face) standard

# AI TRIGGER TERMS (PixelLab)
* "Visible top surfaces" (Draws top planes)
* "3x3 bitmasking logic" (Tileset edge transitions)
* "Orthographic bias" (Removes perspective vanishing)
* "60-degree overhead angle" (RIMA High Top-Down lock #54)

# RECIPE
1. Template: _STAGING/templates/64x64_blob.png -> Concept Image
2. Style: Warblade -> Style Image (Palette)
3. Prompt: "Visible top surfaces, dungeon stone texture, high top-down"
