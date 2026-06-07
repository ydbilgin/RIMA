# RIMA — Elementalist Spell-Cast VFX Production — ART/TECH DIRECTION (Gemini 3.1 Pro)

CONTEXT: RIMA = dark pixel-art roguelite, cyan #00FFCC Rift energy on void-purple, PPU 64, 8-direction characters, URP 2D + 2D lights, "Vivid Vulnerability". Building a 2-character gameplay demo (Elementalist + Warblade). Elementalist is the mage: signature spells = fireball (projectile), glacial spike (projectile), meteor (AoE). Need to decide HOW to produce the spell-cast / projectile VISUALS.

You are the ART/TECH-DIRECTION advisor (feasibility + lean advisors answer separately). Answer concretely:
1. **Production method** for spell projectiles/AoE in a pixel-art ARPG: rank these for RIMA — (A) PixelLab animated sprite-sheets, (B) imagegen frame-sheets sliced + animated, (C) Unity ParticleSystem + shader/trail code VFX (with a few cyan/orange spark sprites), (D) hybrid (sprite core + particle trail/impact). Which gives the best on-brand look for the EFFORT, given PixelLab generation is GATED?
2. **Sheet design** if sprite-based: frame counts for fireball/ice/meteor (cast → travel → impact phases), canvas size, PPU 64, loop vs one-shot, how impact/AoE reads at iso 3/4 angle. 8-direction needed for projectiles or is rotation/flip enough?
3. **On-brand spell language:** fire and ice are NOT cyan — how to keep Elementalist spells readable yet cohesive with RIMA's cyan-Rift palette? Should mage spells be a warm/cold accent set distinct from Rift-cyan, or tinted toward the canon? Give a palette stance.
4. **Cohesion with characters:** the Elementalist sprite is PixelLab pixel-art; the VFX must match (PPU, outline, dithering). Concrete rules so projectiles don't look like a different engine.
5. **Recommended approach for the DEMO** (ship-quality but not final): your single best path + why.

Tight, concrete, numbers + palette hex.
