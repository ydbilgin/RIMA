# Research: Bandit Knight — RIMA Design Analysis
**Date:** 2026-05-16
**Model:** Gemini (default — gemini-3.1-pro-preview via ~/.gemini/settings.json)
**Source game:** https://store.steampowered.com/app/2421000/BANDIT_KNIGHT/

---

## 1. Combat Feel

- **STEAL:** Snappy responsiveness with clear risk/reward mechanics. High-stakes room encounters via immediate vulnerability on mistake — mirrors what RIMA's "no free hits" philosophy needs.
- **STEAL:** Fast, readable enemy-attack telegraphs that work at small pixel-art sprite sizes — important for 64x64 chibi at PPU=64.
- **AVOID:** Floaty, comical lightness of impacts. RIMA is "Fractured Epic"; hits must feel grounded and punishing, never comedic.
- **AVOID:** Unpunished attack spamming. RIMA needs heavy hitstop, localized screen shake, and distinct knockback to give Brawler/Warblade visceral weight that Bandit Knight intentionally lacks.

*Sources: Steam Next Fest reviews, Churape Reviews, Get More XP*

---

## 2. VFX / Effects

- **STEAL:** Dynamic lighting that actively interacts with small sprites (2.5D approach) to ground characters in environment — read at 64x64.
- **STEAL:** High-contrast particle bursts for object destruction (smashing crates/glass) that read clearly at small resolutions. Simple shapes, high contrast.
- **AVOID:** Overly cute or exaggerated "gag" VFX (enemies throwing comical loot piles). Incompatible with Fractured Epic tone.
- **AVOID:** Soft, rounded slash arcs. RIMA must use sharp, jagged slash arcs and heavy dust kicks to sell the dramatic tone.
- **NOTE:** Game features a "Fever Time" visual state — a usable reference point for RIMA rage/ultimate mode VFX escalation.

*Sources: Steam Store Page, Community Playtest feedback*

---

## 3. Music / SFX

- **Dynamic Layers:** Reception is highly positive — audio shifts dynamically from "regal" exploration to high-energy chase/combat tracks. RIMA's Stable Audio Open pipeline should mimic this two-state system (Exploration vs. Combat) with seamless layer transitions.
- **Tactile SFX:** Bandit Knight praised for highly satisfying, tactile SFX during interactions. RIMA's ChipTone pipeline must prioritize sharp, distinct audio cues for crits, parries, and dashes over ambient fill.
- **Direction:** Target high-BPM percussive tracks on combat initiation; tense, atmospheric underscore during empty-room traversal.
- **Genre hint:** "Regal" + high-energy suggests orchestral hybrid with percussion emphasis — compatible with Stable Audio Open prompting strategy.

*Sources: YouTube gameplay analysis, preview articles*

---

## 4. Punch / Unarmed Combat

- **Context:** In Bandit Knight, unarmed combat (punches/takedowns) functions as a lifeline or stealth utility, NOT a primary offensive system.
- **STEAL:** Low anticipation frame count on strikes — makes close-quarters hits feel highly responsive. Keep Brawler startup frames lean.
- **STEAL:** Tight hitbox detection on takedown strikes — requires precise positioning, reads as rewarding.
- **AVOID:** Treating punches as utility taps. RIMA's Brawler must drive unarmed combat as primary offense: forward momentum (step-in animations), heavy hitstop on connection.
- **Brawler delta:** Bandit Knight punches are quick and light; RIMA Brawler punches should be quick BUT heavy — short windup, explosive impact frame, slow recovery that rewards commitment.

*Sources: Reddit demo discussions, YouTube playthroughs*

---

## 5. Weapon Attach Approach

- **Inferred modular socket system:** Visual analysis of Bandit Knight's varied toolset (throwing knives, master keys, melee) in a 2.5D space implies separate weapon sprites attached to a hand socket/bone on the base rig, not baked per-frame.
- **RIMA confirmation:** This validates RIMA's existing plan — base body rig with Unity child SpriteRenderers for weapons. 10 classes share body animation frames; weapons swap independently.
- **Note:** No public devlogs confirm this from Game Float — inferred from standard 2.5D pixel workflow and gameplay footage.

*Sources: Inferred from gameplay footage and standard 2.5D pixel art technical workflows*

---

## 6. What NOT to Copy

- **Tone mismatch:** Do NOT copy the adorable, jovial, comical aesthetic. RIMA is strictly "Fractured Epic" — vivid/dramatic, not cute.
- **Stealth pacing:** Bandit Knight is built around avoiding combat. RIMA is Hades-style — do not slow room flow with waiting or avoidance mechanics.
- **Arcade clutter:** Avoid score-attack visual noise in UI/VFX. RIMA's screen must stay clean to emphasize dramatic combat moments.
- **Impact weight:** Bandit Knight's hits feel light by design. Copying that lightness directly would undermine RIMA's combat identity.

*Sources: Previews highlighting comical, stealth-heavy loop*

---

## 7. Top 3 Takeaways

1. **Dynamic Audio System — Implement Now:** Two-state music (Exploration vs. Combat) with snappy, tactile SFX per action. Stable Audio Open prompts should target orchestral-percussive hybrid. ChipTone layered for hit/parry/dash feedback. This is the single highest-value audio lift this phase.

2. **Weight Over Speed for Impact:** Replace any arcade-light hit feedback with heavy hitstop (3-5 frames), localized screen shake, and sharp high-contrast VFX. At 64x64 sprite size, impact must read in 1-2 frames — no soft/rounded arcs. Jagged slash frames + dust kick + screen flash.

3. **Brawler Step-In Animation — Forward Momentum:** Bandit Knight's punches are responsive but light utility taps. RIMA's Brawler needs a step-in translation (character body moves forward ~4-8px) on the impact frame, with explosive hit-pause, so unarmed strikes feel grounded at 64x64 size without relying on weapon reach.

---

## Confidence Assessment
**CONFIDENCE: MEDIUM**
Reason: Bandit Knight is a smaller indie title with limited public developer documentation. No official devlogs or technical GDCs found. Analysis draws from Steam store page, Steam reviews, YouTube gameplay footage, preview articles, and Reddit discussions. Weapon attach approach is inferred, not confirmed. Audio genre description is based on reviewer language, not developer statements.

**GAPS:** No official devlog / postmortem available. Exact frame-count data for punch animations not found in public sources. Gemini flagged no hallucinations but acknowledged inference on the weapon socket section.
