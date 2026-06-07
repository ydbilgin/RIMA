# RIMA — Elementalist Spell-Cast VFX — LEAN / SHIP-FAST lens (Gemini 3.5 Flash)

CONTEXT: RIMA dark pixel-art roguelite (cyan Rift energy, PPU 64, URP 2D). Building a 2-char gameplay DEMO (Elementalist + Warblade). Elementalist spells: fireball, glacial spike, meteor. Decide cheapest way to make spell-casts FEEL good in the demo. PixelLab generation is GATED (needs user approval + credits).

You are the LEAN / over-engineering-critic advisor. Blunt bullets:
1. **Cheapest path** that makes 3 Elementalist spells feel impactful in a DEMO: pure Unity ParticleSystem/code VFX (reuse existing spark sprites + trails + light flash + screen juice) vs producing animated sprite-sheets (PixelLab/imagegen). Which is less work AND looks fine for a demo?
2. **What to AVOID generating now:** is producing per-spell animated sprite sheets (cast/travel/impact × 3 spells × directions) a trap for a demo? What's the over-engineering risk?
3. **Reuse:** what likely already exists (SlashArcVFX, JuiceManager, particle prefabs, a projectile system) that can be repurposed for projectiles with near-zero new art?
4. **Minimum viable spell feel:** the 20% (muzzle flash + moving sprite/particle + impact burst + light + hitstop) that gives 80% of the "juice".
5. **Ship order:** make spells FUNCTIONAL (damage + hit) with placeholder VFX first, then juice, then optionally swap in generated art. What blocks the demo vs what's polish?
6. **Skill icons — evaluate the user's concrete proposal:** Generate skill icons via **PixelLab create_image_pro with a STYLE REFERENCE image** (AI Freedom 0, like the `pixelify` flow) instead of imagegen — at **32px**, batched per class (header "Warblade"/"Elementalist" + a bulleted "- <icon desc>" list, generate any count), with a **hard rule: NO TEXT / no letters/numbers, symbol-only**.
   - Is this BETTER than imagegen for pixel-consistency? Is 32px the right source size or should it be 48/64 for UI clarity (icons render ~48px in CharSelect, 44-56px on skill bar)? Integer-scale concern?
   - Is generating N skill icons a worthwhile spend vs reusing existing SkillIconRegistry / PassiveIcon for the demo? For a 2-class DEMO (Warblade+Elementalist), how many icons actually needed?
   - Over-engineering risk: generating icons for all 76 skills vs just the demo's handful. What's the lean cut?
   - Note: PixelLab generation is GATED (user-approved). Flag that.

Favor reuse + defer. Numbers where useful.
