---
name: Write MCP tool params from schema
description: Verify MCP tool schema via ToolSearch before use.
type: feedback
---
* **Rule:** Call `ToolSearch` for every MCP tool before writing task files.
* **Reason:** REST API docs != MCP tool signatures. Prevents invalid parameters (e.g. `direction` in PixelLab tool).
* **Action:** Verify real parameters from schema, do not copy from documentation.
