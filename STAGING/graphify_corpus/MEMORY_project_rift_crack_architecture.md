---
name: Rift Crack VFX Architecture
description: Layered design: Baked sprite lines + Unity glow.
type: project
---

# ARCHITECTURE: 2 LAYERS
1. Crack LINE (PixelLab): Baked in sprite.
    * Prompt: "flat color fill inside the hairline crack only - no bloom, no aura, no ambient effect"
    * FORBIDDEN: "inner glow"
2. Crack GLOW (Unity): RiftGlowVFX.cs.
    * Type: Soft diffuse particle / point light
    * Spec: 3-5px drift tolerance (at 128px scale)

# LOGIC
* Baked LINE: Ensures visual sync with character movement/anim
* Unity GLOW: Dynamic link to gameplay (Rage, Cast, Hit, Death)

# IMPLEMENTATION (2026-04-29)
* Prefab: Add VFX_RiftGlow child + RiftGlowVFX component
* Scripts: HandGlowVFX.cs, RiftGlowVFX.cs, SkillFlowTracker.cs events (Done)
