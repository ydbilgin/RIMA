---
name: RIMA VFX Production
description: VFX sprite specs and runtime glow logic.
type: project
---

# VFX SPRITES (PIXEL DIFFUSION)
* Slash Arc (Warblade): 128x128, Cold Blue crescent, 1 peak frame
* Hit Spark: 64x64, White/Cold Blue, 2 frames
* Death Burst: 96x96, Dark Purple (#8C33E6)
* Material: ParticleAdditive.mat (Fixes pink NULL issue)

# RUNTIME GLOW SCRIPTS
* HandGlowVFX.cs: Class-based accent color, idle/cast intensity
* RiftGlowVFX.cs: Rift crack overlay, accentColor + offset
* SkillFlowTracker.OnSkillUsed: Cast state trigger event

# COLORS
* Slash/Hit/Pulse: #66B2FF (Cold Blue)
* Death: #8C33E6 (Void Purple)
