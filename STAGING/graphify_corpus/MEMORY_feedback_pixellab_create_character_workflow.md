---
name: PixelLab Workflow v3 — Anchor Chaining
description: S42 v3 Character Production Pipeline.
type: feedback
---
* **S43 Pivot (LOCKED):**
  - Camera: **Low Top-Down** (v7 evidence).
  - Size: **128x128**.
  - Style Anchor: `Elementalist_128_anchor`.
* **Method:** Create from Reference -> **Standard**.
* **Output:** Output Size = Reference Size (64x64 ref -> 64x64 output). **Upscale BANNED**.
* **UI Config:** Humanoid, Low Top-Down, Heroic Preset, South-facing ref in slot 1.
* **Description:** Identity-only (Proportions/Camera handled by fields).
* **Pipeline (Anchor Chaining):**
  - Stage A: Hero Siege Ref -> Warblade Anchor lock.
  - Stage B: Warblade Anchor -> Elementalist Anchor lock.
  - Stage C: Use Anchors to gen remaining 14 classes.
* **Banned:** Multi-class sheets (text leak risk), Upscaling during generation, Pro mode (unstable).
* **character_id:** Log in `_STAGING/character_ids.md` for `animate_character` calls.
