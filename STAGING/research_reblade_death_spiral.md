# ReBlade: The Death Spiral — Combat Design Research
*Date: 2026-05-16 | Model: gemini-3.1-pro-preview (default)*
*Purpose: RIMA combat feel sprint — steal-worthy mechanics from 3D character action*

---

## 1. Core Combat Loop
**Confidence: HIGH**
*Sources: Steam Page, IGN, AllKeyShop*

ReBlade combines high-speed Soulslike ARPG combat with a Hades-style one-more-run loop. The rhythm emphasizes deliberate, strategic precision over button-mashing. The central mechanic revolves around dismantling high enemy defenses and stagger meters to create brief, massive damage windows. Progression is gated by modular "Chips" (run-specific combat augments) and "Augments" (permanent meta-progression).

**RIMA relevance:** Defense-break creating burst windows maps cleanly onto posture v1 boolean. The "stagger meter -> burst window" loop is directly portable — posture break already gatekeeps the high-damage phase in RIMA's design.

---

## 2. Parry / Dodge / Block
**Confidence: HIGH**
*Sources: Reddit r/Unity3D, VeloxGame, TWStalker*

Parrying is the cornerstone of both offense and defense. Players can deflect almost anything: melee strikes, ranged attacks, and even bombs.

- **Mechanics:** Successfully parrying projectiles kicks them back at the sender.
- **Rhythm:** Rapid-fire enemy attacks can be parried sequentially, creating a satisfying metal-on-metal rhythm.
- **Accessibility:** A "simple mode" offers more forgiving parry windows for readability.
- **Reward:** Successful parry does not just negate damage — it opens a counter window and can stagger the attacker.

**RIMA relevance:** Sequential parry rhythm maps onto Warblade's absorb-break and Brawler's whiff-evade (Karar #57). The projectile-kick-back is a strong Warblade secondary ability candidate.

---

## 3. Weapon Variety + Swap
**Confidence: MEDIUM**
*Sources: VeloxGame, Steam*

Arsenal includes daggers, longswords, shield-axes, scythes, and spears. Each weapon has dedicated primary and secondary skills. Players can transfer attribute modifiers between gear mid-run.

**Gap:** Not clear from early previews whether fluid mid-combo weapon swapping (DMC-style) is present, or if players commit to a weapon per run.

**RIMA relevance:** Per-weapon primary+secondary skill structure maps onto RIMA's class-bound primary weapon design. No mid-combo swap needed — RIMA already locks weapon per class (Karar #144). The attribute-transfer mechanic echoes RIMA's chip/augment run customization design.

---

## 4. Boss Design + Tells
**Confidence: HIGH**
*Sources: VeloxGame, Early Gameplay Previews*

Bosses are mythological Chinese beasts with mechanical/cybernetic ascensions (e.g., the Qiongqi). Encounters are "pattern recognition trials." Bosses have heavy elemental/technological shields requiring systematic breaking via perfect parries.

- **Tells:** Explicit visual flashes (sparks/glints) immediately preceding an attack signal the parry window.
- **Phase transitions:** Shield-breaking changes the boss's attack set and removes elemental damage gating.
- **Design philosophy:** Multiple defense layers (elemental shield, stagger meter, HP) create a structured takedown sequence.

**RIMA relevance:** The layered defense sequence (shield -> stagger -> HP) maps onto Penitent Sovereign design. The visual flash tell system is the single most portable mechanic for RIMA's 64x64 enemies (see Takeaway #2).

---

## 5. Hit Feedback
**Confidence: MEDIUM**
*Sources: Reddit r/roguelite, Developer Devlogs*

Developer ChillyRoom explicitly targets "crunchy" and "chunky" feel:

- **Hitstop:** A "waiting system" (hitstop) applied on heavy weapon follow-throughs and shield slams conveys physical weight.
- **Audio/Visuals:** High-contrast particle sparks + crisp, punchy audio cues trigger on successful deflections.
- **Camera:** 3D camera zooms toward enemy on crits — not directly portable to 2D top-down.

**RIMA relevance:** Hitstop duration and audio pairing is directly portable. RIMA's Feel Toggles (Shake/Vignette/Hitstop ON) already lay the groundwork. Tuning: 0.05s freeze on heavy crits / perfect parries, paired with a distinct "clang" SFX, achieves the same effect without 2D spatial disruption.

---

## 6. Why Combat Feels Good
**Confidence: HIGH**
*Sources: Reddit Impressions, IGN*

1. **Rhythmic Flow:** Stringing sequential parries against fast combos induces a satisfying flow state.
2. **Impactful Weight:** Deliberate hitstop + high-fidelity sound makes strikes feel grounded, not floaty.
3. **Rewarding Mastery:** Precise, punishing-yet-fair mechanics reward pattern learning without feeling cheap.

---

## 7. What Does NOT Translate to 2D Top-Down
**Confidence: HIGH**

- **Camera-dependent reads:** 3D attacks can arrive from outside FOV or the Z-axis. In 2D everything is visible — telegraphs must be purely sprite-animation or ground indicators.
- **Cinematic camera work:** FOV zooms, dynamic angle shifts, heavy screen shake on crits. In fixed top-down 2D, excessive shake disrupts spatial awareness and can cause motion sickness.
- **Verticality:** Airborne combos and jumping slashes lose mechanical depth entirely.
- **Behind-camera dodge:** 3D spatial repositioning reads that rely on camera angle are undefined in top-down.
- **Free-look parry timing:** In 3D, parry timing is partly informed by 3D body animation from any viewing angle. In 2D top-down, directional attack animation reads are limited to 4/8 sprite directions.

---

## 8. Top 3 RIMA-Actionable Takeaways

### Takeaway 1 — Projectile Rebound (Warblade / Brawler)
**Mechanic:** Timed parry reflects projectiles back at the sender.
**Implementation for 64x64:** On a successful block input during enemy projectile active frames, reverse the projectile's velocity vector + set allegiance to "player." A white flash on the projectile signals the reflect.
**Class benefit:** Warblade (absorb-break archetype, Karar #57) — reflect is a natural extension of its "absorb energy, turn it back" fantasy. Brawler (whiff-evade) could get a variant: catch-and-throw rather than reflect.

### Takeaway 2 — Explicit "Parryable" Sprite Flash (System-Wide)
**Mechanic:** Flash enemy sprite white or red for 0.2–0.3 seconds immediately before a parryable attack connects.
**Implementation for 64x64:** A simple Material.SetColor("_FlashColor") on the enemy SpriteRenderer at the animation's wind-up frame (frame N-2 before hitbox activation). No expensive shader needed — a two-frame white-fill tween on the sprite.
**Class benefit:** Universal readability. At 64x64 chibi size, wind-up pose reads are limited. This compensates for limited animation fidelity across all 10 classes' enemies without additional art cost.

### Takeaway 3 — Rhythmic Micro-Hitstop + Audio Pairing (System-Wide, Heavy Hits / Perfect Parry)
**Mechanic:** Freeze game state 0.05s on heavy class crits or perfect parries. Pair with a sharp distinct audio cue (not a generic hit SFX — a "clang" or "crack" reserved only for these moments).
**Implementation:** `Time.timeScale = 0f` for 0.05s via coroutine, or `Physics2D.simulationMode` pause on the hit frame. RIMA's Feel Toggles already expose a hitstop bool — this sharpens the tuning.
**Class benefit:** Ravager (heavy follow-through fantasy), Warblade (perfect parry), Brawler (unarmed strike weight). Prevents the "floaty" complaint common in 2D roguelites.

---

## Sources
- Steam Store Page: https://store.steampowered.com/app/3489340/ReBlade_The_Death_Spiral/
- IGN coverage
- VeloxGame preview
- AllKeyShop listing
- Reddit r/Unity3D, r/roguelite impressions
- Developer devlogs (ChillyRoom)

## Confidence Summary
| Section | Confidence | Gap |
|---|---|---|
| Core Loop | HIGH | none |
| Parry/Dodge | HIGH | exact frame data not published |
| Weapon Swap | MEDIUM | mid-combo swap mechanic unclear |
| Boss Design | HIGH | only 1 boss confirmed in previews |
| Hit Feedback | MEDIUM | no published frame data for hitstop durations |
| Feel Good | HIGH | none |
| No-translate | HIGH | none |
| Takeaways | HIGH | none |
