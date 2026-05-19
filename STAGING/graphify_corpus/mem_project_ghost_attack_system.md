---
name: RIMA Ghost Attack System
description: Option C locked - Spec, triggers, budget for 10 classes.
type: project
---

# DECISION: OPTION C (LOCKED 2026-04-17)
* Trigger: Cross-class skill pool + Secondary skills (Z/X).
* Purpose: Visualize "borrowed power".
* Implementation: CrossClassGhostEffect.cs (trigger on SkillManager + Z/X use).

# SPECIFICATION
* Frame Count: 12 frames
* Segments: 2 (6f windup-peak, 6f peak-settle)
* Directions: 4 (S, N, W, E)
* Rendering: Neutral sprite + runtime tint (MaterialPropertyBlock)
* VFX: 0.6s fade (alpha 0.5 -> 0, additive blend)

# BUDGET
* Generation: ~240g total (4.8% of 5000g total budget)

# REFERENCES
* GUIDES/GHOST_ATTACK_SPEC.md (Prompts)
* TASARIM/CROSS_CLASS_SKILLS.md (Colors)
