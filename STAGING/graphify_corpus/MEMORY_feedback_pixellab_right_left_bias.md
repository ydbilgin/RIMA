---
name: PixelLab Right/Left Hand Bias
description: AI interprets "right" as viewer`s right (screen right), not character`s perspective.
type: feedback
---
* **Constraint:** Avoid "right hand" / "left hand" in prompts for south-facing sprites.
* **Action:** Use position-based terms: "one open palm raised at chest height", "other arm at side".
* **Fix (S43):** Use PixelLab **Mirror** tool if hand comes out wrong. No Aseprite mirror needed.
