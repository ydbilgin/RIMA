---
name: pixellab-create-modes
description: Comparison of character generation modes.
metadata: 
  node_type: memory
  type: reference
  originSessionId: 800899c1-572c-4f23-9c89-2fd4b064f5a0
---

# CREATE FROM REFERENCE (3 MODES)
1. Pro / Rotate: Single sprite -> 8-dir. Fast, weak diagonals. Backup only.
2. Pro / Style+Concept: Best for base S, no 8-dir. Identity drift risk.
3. Standard / 8-Dir Grid: 8 slots. Hard to maintain cross-dir consistency.

# CREATE FROM STYLE REFERENCE (PRO) - S43
* Mode: Style from Image + Identity from Text
* Ref: Elementalist anchor PNG (confirmed PASS)
* Output: 4 variations (128x128)
* Purpose: Maximum style consistency across roster

# RIMA WORKFLOW (S43)
* Style Ref: Elementalist anchor
* Identity Prompt: Skin, hair, armor, weapon ONLY (No style terms)
* Result: 4 variations -> Select best -> Create Character -> 8-dir

# CHARACTER STATES (S86 — Karar #145 LIVE 2026-05-16)
* New V3 workflow layer added between Create Character and Animate
* Click **Create State** on character card → text prompt for pose/outfit/variant
* Animation generation gains **"first frame: ON"** toggle — state becomes locked anchor frame
* **Advanced Options → Interpolation**: state-to-state (start state + end state) → AI fills transition (stand-up, transformation, death)
* Demo proved: 32×44 custom canvas, 176×176 boss canvas, animal states, costume variants ("Christmas outfit" example)
* Full 8-direction walk cycle <5 minutes when character is symmetric enough to mirror
* RIMA usage: pilot 4 sınıf state-first (Warblade → Ranger → Shadowblade → Elementalist), idle-only fallback for Batch 2 (Ravager/Ronin/Brawler/Gunslinger/Summoner) until pilot PASS
* Full spec: [[pixellab-character-states-workflow]] (RIMA) + [[pixellab-character-states-animation-workflow]] (Lauret Studio global)
* **No MCP endpoint** as of 2026-05-16 — Web UI V3 manuel ([[pixellab-character-via-web-ui-v3]])
