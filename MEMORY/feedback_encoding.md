---
name: Encoding Rule
type: feedback
description: ASCII-only rule for shared markdown and prompts
---

## Rule
- Internal `.md` files must be ASCII-only.
- Prompt files and internal documentation should also stay ASCII-only.
- User-facing chat may use Turkish naturally.

## Reason
Claude and Codex may write files through different encoding contexts.
Turkish diacritics have caused mojibake/double-encoding corruption.
