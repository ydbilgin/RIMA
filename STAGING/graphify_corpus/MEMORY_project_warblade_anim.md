---
name: Warblade Animation Pipeline
description: S43 - 128px workflow. S42 64px DEPRECATED.
type: project
---

# STATUS (S43)
* Anchor: warblade_128_v7.png (Image #52 selected)
* character_id: Pending registration
* Symmetry: Horizontal flip OK for W/E (West not generated separately)

# PIPELINE
1. warblade_128_v7.png -> Create Character -> Reference Standard
2. ID registration in character_ids.md
3. MCP animate_character (4 directions x 5 anims)

# ANIMATION SPEC
* Run: 6f, 4 directions, NO flipping
* Iron Combo: 3-segment workflow (Peak frame priority)
* Iron Surge (Dash): Forward lean, 3-4 frames
* Idle: First+Last frame interpolation
