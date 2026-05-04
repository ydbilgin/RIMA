---
name: animate_character MCP forbidden for characters
description: MCP animate_character must never be called for character skill/run animations. User does this manually.
type: feedback
---

Never call animate_character MCP for character animations. User generates all character animations manually in PixelLab UI.

**Why:** MCP output had critical quality issues:
- Hard 4-frame limit at 128px (UI supports 8-16 frames)
- AI embeds impact sparks and weapon VFX directly into sprite frames (wrong -- VFX must be separate)
- Template run animations looked unnatural/meaningless
- char_ids intentionally deleted from all memory files to prevent accidental calls

**How to apply:**
- Do NOT look up char_ids from any source
- Do NOT call animate_character, even if the user forgets this rule
- Claude's role: prepare action description prompt documents for the user to execute in UI
- MCP is allowed ONLY for: create_isometric_tile, create_object, create_map_object, create_tiles_pro (tiles and objects, NOT characters)
- If user asks to generate a character animation via MCP, remind them of this rule and offer to prepare a prompt document instead
