# RIMA — Portal→Orb→Next-Map Travel: PixelLab Asset Research (IDEA ONLY)

**Status:** 💡 IDEA / RESEARCH — NO PRODUCTION. User must approve before any PixelLab gen.
**Date:** 2026-05-31 PM·6
**Relates to:** `STAGING/PORTAL_PREVIEW_SYSTEM_SPEC_S6.md` (the locked portal/preview/orb system).

---

## The question
"Portaldan diğer map'e top olup gitme" — player morphs into a glowing orb, flies
across the void to the chosen island, lands. What of this is PixelLab-generated art
vs engine-procedural? This doc captures the idea so production is fast later.

## Core finding: MOSTLY ENGINE-PROCEDURAL, MINIMAL PixelLab
The orb-travel "juice" is overwhelmingly runtime VFX (TrailRenderer + Light2D + tween +
particles), per the locked spec §2B/§4. Hand-drawn per-orientation orb frames would be
WASTED — the orb is radially symmetric and moves fast. So PixelLab's role is small and
targeted. This matches the project rule: procedural morph, never hand-drawn per-orientation.

## What COULD be PixelLab (candidate gen list — NOT a production order)
1. **Orb core sprite (1 asset, ~32-48px).** A single glowing cyan energy-sphere with a
   bright center + soft rim. Static; the TrailRenderer + Light2D do the motion read.
   Optionally 2-3 variants tinted per room-type (combat/elite/treasure) to color-match the portal.
   - PixelLab path: `create_1_direction_object` (single object, no rotation needed — radial).
2. **Portal rift frame (1 asset, ~96px) + center swirl (sheet).** The portal the user
   enters. Static rift ring + a small looping swirl. Swirl could be a `create_object` +
   `animate_object` short loop (gated — animation needs approval per HARD rule).
3. **Rune-icons per room-type (~32px each).** combat=crossed-swords, elite=skull,
   treasure=chalice/gem, shop=coin, boss=crown, rest=flame. Small icon set, on-brand
   (cyan accent). These float in the portal center (spec §1). Batchable via `create_1_direction_object`.
4. **Crash-land impact decal (1 asset, ~64px), optional.** A brief burst sprite at landing.
   Could also be pure ParticleSystem — likely skip PixelLab here.

## What should NOT be PixelLab (engine-procedural)
- The orb's flight trail (TrailRenderer / ribbon) — §4 #1, highest juice / lowest effort.
- The morph squash-stretch (engine scale/dissolve) — §1, never hand-drawn.
- Light2D glow, camera lead, screen-shake, crash flash — all runtime.
- Preview islands — those reuse the real room geometry/floor/prop sprites (already have PixelLab floor);
  no NEW gen needed for travel.

## Style lock (carry into any future gen)
- On-brand: charcoal/iron + cyan #00FFCC sparing; flat painterly/pixel-leaning; transparent PNG;
  PPU 64; NO photoreal/gloss/gold/text. (Same lock as UI pack + env.)
- Orb = energy, not a solid ball: bright core, translucent rim, slight irregular edge so the
  trail reads as motion not a bouncing marble.

## Minimal viable production (WHEN approved)
Smallest set that delivers the fantasy: **(1) orb core sprite + (3) 5-6 rune-icons.**
Everything else (portal rift, swirl, crash) can start as engine VFX / placeholder and be
upgraded later. Rift frame is the next tier if the cyan-rect placeholder reads poorly.

## Open question for user (later, not now)
- Orb tint: single cyan for all portals, or color-matched per room-type? (Spec leans color-matched.)
- Portal rift: PixelLab art now, or ship with engine-VFX rift + cyan glow first?

**DO NOT GENERATE ANYTHING from this doc without explicit user approval.**
