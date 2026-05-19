---
name: PixelLab MCP — Auto-Gen BANNED, ID-based OK
description: Never auto-create characters with MCP (Credit protection).
type: feedback
---
* **Prohibition:** `mcp__pixellab__create_character` is **BANNED** for autonomous use.
* **Reason:** High credit cost (40 gen/char). Human QC mandatory for base sprites.
* **Allowed:** `mcp__pixellab__animate_character` using a user-provided `character_id`.
* **Action:** Verify schema via `ToolSearch` before calling `animate_character`.
