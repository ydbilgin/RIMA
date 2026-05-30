# RIMA — S6 Strategic Decisions (Opus final, cx+ax review as validators)

## DECISION 1 — Audio & Animation production timing  ✅ LOCKED
**Spec NOW, produce LATER. Animation = user-gated. Audio production = AFTER animation.**

Reasoning — the **mechanical timing layer IS the animation skeleton**, and it's already locked in code:
attack startup, Beat3 commit frame (0.417 normalized), hitstop tiers (0.04/0.07/0.12/0.18), swing strike-frame
alignment, telegraph durations, dash i-frames, finisher commit-beat. Animation hangs on these anchors → it WILL
"sit" correctly because the timing contract exists before the art does.

Audio must sync to *animation frames* (swing whoosh = strike frame, footstep = walk cycle, impact = hit frame).
Producing audio before animation = rework. So:
1. Keep building mechanical anchors + juice hooks NOW (this IS deciding the timing animation sits on).
2. Write the **audio SPEC** now (palette + event→sound map) — design 2F. No production.
3. Animation production: **USER-GATED** (hard rule).
4. Audio production: **after animation** (so SFX lock to real frames), via Sora + Gemini / RTX.

Answer to "would deciding now help animation sit?": **YES — the mechanical timing being built now is the skeleton.**

> **ax review: AGREE.** Refinement: a *minimal procedural/placeholder SFX pass* mapped to the locked code anchors
> can land NOW for playtest feel (AudioManager already does procedural SFX) — only the FINAL sampled audio waits for animation.

## DECISION 2 — Mob / Boss art direction  ✅ LOCKED (decide HOW, gen stays GATED)
**Demo NOW = coherent graybox (already in scene). FINAL = archive-restore base mobs + gen'd boss. No gen runs yet.**

- **Now (playable demo):** FractureImp + HollowHulk_GB + ShardWalker_GB graybox already exist → rooms are playable
  and readable with ZERO art spend. This is what makes "rooms connected, playable" possible immediately.
- **Final base mobs = archive-restore (path A):** reuse `ARCHIVE/Sprites_Enemies_old/`, recolor to the slate palette
  + subtle cyan rift accents. 0-gen, Python-cheap palette swap, autonomous-able when greenlit.
- **Final boss (Penitent Sovereign) = high-quality gen (PixelLab Style-Ref OR RTX-local Flux):** 128-192px,
  chained silhouette, cyan seal-energy emissive that intensifies as chains break (66%/33%), 8-dir. **Gen GATED.**
- **Color/readability budget (ties to design 2G):** mobs = muted slate/desaturated; **cyan is reserved for
  player / rift / telegraph** only. Boss = slate body + cyan chains/seal-cracks. The boss carries the cyan because
  it IS the seal made flesh.
- **Why:** fastest path to a playable demo (user priority), zero gen spend now, art direction fully locked so the
  eventual gen is coherent; the boss gets the gen budget because it's the demo's climax.

> **ax review: AGREE.** Caveat to bake into the art spec: muted slate mobs must keep enough **value contrast**
> (not just hue) against the slate background tiles so they don't visually blend — readability over palette purity.

## Routing for the autonomous run (user directive)
Opus = decisions + hard/multi-system code. **Sonnet (sub-agents) = mechanical bulk.** **cx + ax = REVIEWERS**
(not writers). Big conflict → report to user with options; otherwise decide and continue. Animate = never without approval.
