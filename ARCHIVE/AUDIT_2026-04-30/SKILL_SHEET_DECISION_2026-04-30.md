# Skill Sheet Decision (Claude)
Date: 2026-04-30
Author: Claude (Opus 4.7)
Inputs:
- 10 spritesheets in `C:/Users/ydbil/OneDrive/Masaustu/RIMA_SPRITESHEET/`
- `STAGING/SPRITESHEET_SKILL_IDENTITY_FEEDBACK_TO_CLAUDE_2026-04-30.md` (Codex)
- `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md` (canonical)

Lifecycle: Temp file. Delete after Visual Contract template lands in TASARIM/.

---

## TL;DR Decision

1. **Skill set is canonically sound.** All 80 panels (10x8) match the canonical doc. Nothing is "out of design," nothing nonsensical, nothing needs name fix. Codex's "obsolete naming" claim is wrong (verified Death Blow line 131, Mugen no Kiri line 327 — both canonical).

2. **"Skills feel similar" problem is REAL.** Confirmed. Visual identity is carried by class color, not skill verb. Many skills differ by intensity tier, not by behavior shape.

3. **Sheets stay as concept boards. NO production lock.** They are 1774x887 illustrations, not 128x128 importable sheets. Use for mood/style, not for animation contracts.

4. **PRODUCTION GATE: HOLD.** Do not generate any more PixelLab production sprites for these classes until a per-skill Visual Contract is written. Brawler char_id work continues for IDLE/MOVE/DASH only — no skill animations yet.

5. **Engine-owned vs PixelLab-owned split is locked** (see section "Ownership Split" below). State language is Unity. Peak frames are PixelLab.

---

## Per-Class Visual Identity Verdict

Scoring rubric:
- IDENTITY: how distinct skills are from each other within the class (1-5)
- READABILITY: how readable each skill is in 128px gameplay (estimate, 1-5)
- RISK: production risk if shipped as-is (LOW/MED/HIGH)

| Class | Identity | Readability | Risk | Verdict |
|---|---|---|---|---|
| Warblade | 3 | 3 | MED | Identity good across class; 4 skills compress to "ground impact" inside the class |
| Elementalist | 4 | 4 | LOW | Strong shape variety. Element Switch panel is a system not a skill — fine, keep |
| Shadowblade | 2 | 3 | HIGH | All skills read "purple slash + ghost trail." Phase/Scar/Collapse verbs invisible |
| Ranger | 3 | 4 | MED | Clear ranged identity but too "shoot arrow" — Mark/Trap/Detonate triad weak |
| Ravager | 2 | 3 | HIGH | Class has color identity but skills collapse to "axe spin + red." Trade/cost language missing |
| Ronin | 3 | 4 | MED | Pose elegance is good. Wait/draw fantasy missing — not in any panel |
| Gunslinger | 3 | 4 | MED | Strong base but Reload/Heat rhythm not shown. Rift Dash purple bleeds into Shadowblade space |
| Brawler | 2 | 2 | HIGH | **Visually weakest class in the lineup.** Risk of "low budget" perception next to Elementalist/Summoner |
| Summoner | 5 | 4 | LOW | Strongest sheet. Distinct verbs. Watch swarm density at 128px |
| Hexer | 4 | 4 | LOW | Sigil/curse identity strong. 4 skills are green-burst variants but distinguishable by shape |

---

## Cross-Class Visual Collision Map

These pairings will visually cannibalize each other in actual play if not separated:

### Movement skills converge to "color-trail dash"
- Shadowblade: Phase Step, Veil Flicker, Seam Rend
- Ronin: Sheath Walk, Haste Dash, Iaido Blur
- Ranger: Tactical Roll, Hunter's Step, Rift Step
- Gunslinger: Rift Dash
- Brawler: Weave, Flying Knee
- Hexer: Curse Step

**Fix:** body posture + trail shape must differ. Class color alone is insufficient.

### Buff/Transform modes converge to "stand still + colored aura"
- Ravager: Berserk Mode (red)
- Brawler: Overdrive (orange)
- Shadowblade: Wraith Form (purple)
- Warblade: Bladestorm (blue spin)

**Fix:** Bladestorm has motion. The other three default to aura. Need: pose change, weapon glow change, body silhouette change — not just color halo.

### Ground-impact melee triangle
- Warblade (steel/armor crack, blue rift)
- Ravager (blood/cost, red)
- Brawler (dust/body mechanics, beige)

**Fix already locked in canonical doc** (Warblade=Sundered, Ravager=Blood Pact, Brawler=Charged State). Visual must surface these states.

### Ranged projectile pair
- Ranger (mark/trap/distance)
- Gunslinger (reload/heat/slide)

**Fix:** trap geometry + heat overlay must be visible.

---

## Ownership Split (Locked)

### PixelLab generates (peak frames + idle/dash)
- Idle, walk, dash 4-direction anchors per class
- 1-2 peak pose frames per skill (the strike moment, the cast moment)
- Big readable elemental VFX shapes (Fireball ball, Glacial Spike line, Meteor descent)
- Class fantasy frames for V Burst
- Mood/concept boards (already done — these 10 sheets)

### Unity owns (engine-side VFX/UI)
All state language:
- Warblade: Sundered/Broken target overlay (cracked armor decal)
- Shadowblade: Rift Scar persistent decal, Collapse implosion
- Ranger: Mark reticle, Trap wireline, Detonate flash
- Ravager: Blood Debt UI marker, Fury bar pulse
- Ronin: Tension meter, Opened-state target indicator, hit-stop
- Gunslinger: Heat bar, Exposed Line, muzzle flash recolor at Overheat
- Brawler: Charge pips (1-5), Cracked/Shattered/Pinned/Launched target state
- Summoner: Command line (master->minion), Sacrifice mark, Corpse Field decal
- Hexer: Hex stack pips on target (1-10), Phase color bands (Debuff/Pressure/Overload/Hexblast)

Plus: hit-stop, slow-mo, projectile collision behavior, beam paths, trap endpoints, screen shake.

---

## Skill Visual Contract Template (NEW — required per skill before PixelLab)

For every active skill (12 + V per class = 130 skills), write one block:

```
SKILL: <Class>.<SkillName>
VERB: <single action — slash/punch/throw/cast/dash/curse/summon>
SHAPE: <line / cone / ring / point / area / projectile / trap / link / aura>
STATE APPLIED: <what enemy state appears — Sundered/Marked/Hexed/Cracked/etc, or NONE>
STATE CONSUMED: <what state must be present — or NONE>
SCALE TIER: <basic / setup / movement / payoff / V>
FORBIDDEN OVERLAP: <which other skill in same class it must not resemble — and which axis to differ>
PIXELLAB FRAME: <the single peak pose to generate>
UNITY VFX: <list of engine-side overlays/decals/UI cues>
READABLE AT 128px: <yes/no — if no, redesign or merge into a parent skill>
```

This is the gate. No PixelLab production work proceeds without this filled per skill.

---

## Priority Redesign Queue (visual identity, NOT mechanical)

These four classes get Visual Contract first. Mechanical design stays as canonical doc — only visual grammar gets sharpened.

### 1. Brawler — HIGHEST PRIORITY
Reason: weakest visual sheet, risks "underpowered" perception.

Per-skill differentiation must use body mechanics:
- **Jab** — single arm extend, tiny dust, no full-body weight
- **Mach Punch** — multi-arm afterimage, Brawler stays in place
- **Combo Chain** — 4 distinct pose frames (jab/cross/hook/uppercut), 5m forward translation visible
- **Pivot Hook** — feet planted, hip rotation, single heavy side impact, enemy bent off-balance
- **Weave** — Brawler offset from attack line, enemy fist/weapon visible passing through where Brawler was
- **Aerial Rave** — target clearly airborne, vertical hit chain
- **Shockwave Slam** — ground crack ring, smaller than Warblade's Earthsplitter
- **Overdrive (V)** — different from Berserk Mode — propose: phantom-arm afterimages around Brawler, not aura halo

### 2. Shadowblade
Three distinct visual verbs needed:
- **Phase** = transparent silhouette, body passing through
- **Scar** = persistent thin rift wound left in air (Unity decal)
- **Collapse** = scar implosion / snap closure VFX (Unity)

Per-skill split:
- Veil Strike — single reverse-grip slash, no scar
- Veil Flicker — body teleport with phase silhouette, leaves scar at exit
- Seam Rend — dash-through cut, scar perpendicular to path
- Phase Step — short body teleport, no scar
- Death Mark — sigil on target, not a slash
- Veil Burst — 4 phase teleport-strikes, 4 scars in cross pattern
- Severance — line drawn from Shadowblade to target, then collapse
- Wraith Form (V) — fully transparent body, scars from every dash

### 3. Ravager
Three visual languages:
- **Suffer** — self-wound visible, blood from Ravager's own body (Blood Pact, Reckless Swing)
- **Trade** — pact sigil / cost marker UI (Blood for Power chain)
- **Frenzy** — payoff after risk (Bloodlust Strike, Blood-Drunk Leap)

Berserk Mode (V) must differ from Bladestorm: propose blood ring rotating around Ravager + per-kill extension UI tick visible, Ravager body posture more hunched/feral.

### 4. Gunslinger / Ranger ranged split
Gunslinger:
- Reload pose must appear in at least 1 panel (Empty Mag Burst, Reload Roll)
- Heat bar visible at character UI level
- Rift Dash recolored: dust + sparks, NOT purple void energy (currently bleeds into Shadowblade)

Ranger:
- Mark reticle on target visible in Pinning Shot, Marked Detonate, Final Strike
- Trap wireline geometry shown in Bone Trap, Wireline Trap (R4)
- Detonation flash distinct from arrow impact

---

## What Does NOT Need Redesign

Canonical doc stands. No mechanic changes. No skill renames. No skill removals.

These classes are visually OK as-is for concept board purposes:
- Elementalist (shape variety strong)
- Summoner (verb variety strongest)
- Hexer (sigil/curse identity strong)
- Warblade (just compress 4 ground-impact skills via Sundered overlay differentiation)
- Ronin (need to add wait/draw pose to one panel — Iaido Stance is the missing visual)

---

## Production Pipeline Going Forward

1. **Now:** Visual Contract template lands in `TASARIM/SKILL_VISUAL_CONTRACT.md`
2. **Next:** Fill contract for the 4 priority classes (Brawler, Shadowblade, Ravager, Gunslinger)
3. **Then:** Generate Brawler char_id idle/walk/dash 4-direction anchor (already in progress per CURRENT_STATUS)
4. **After anchor lock:** Generate per-skill peak frames using V3 keyframe REST workflow (15 gen/dir per Codex's earlier validation)
5. **Unity-side:** Build state overlay system (Sundered decal, Rift Scar decal, Hex stack pips, Heat bar) before sprite-skill integration
6. **Validate** every skill animation at 128px PPU 64 in `Assets/Scenes/_IsoGame.unity`, not in concept-sheet form
7. **Lint loop:** When contracts collide (two skills end up with identical SHAPE+STATE), rewrite contract or merge skills

PixelLab budget reminder: 2414 gen left, deadline 2026-05-18. At 15 gen/dir x 4 dir = 60 gen per skill animation. 130 skills x 60 = 7800 gen needed for full coverage — far over budget. Must prioritize: 4 base poses + 4 active animations per class (Phase 1 only). V Burst animations later.

---

## Updates Needed Elsewhere

After this decision is read:
- [ ] `CURRENT_STATUS.md` — add "Skill Visual Contract gate" to Next Priorities
- [ ] `TASARIM/SKILL_VISUAL_CONTRACT.md` — new file, template + 4 priority class contracts
- [ ] `MEMORY/pixellab_sprites.md` — note ownership split (peak frames only, state = Unity)
- [ ] Archive `STAGING/SPRITESHEET_SKILL_IDENTITY_FEEDBACK_TO_CLAUDE_2026-04-30.md` to ARCHIVE/ once contract is written
- [ ] Delete this file once Visual Contract template is in place

---

## Bottom Line

User's instinct is correct: skills look similar. But the fix is NOT redesigning the skill set. The fix is:
1. Lock per-skill Visual Contracts (verb + shape + state + scale + forbidden overlap)
2. Move state language out of PixelLab and into Unity (engine-owned overlays)
3. Generate only peak frames in PixelLab, validate at 128px before locking
4. Hit Brawler/Shadowblade/Ravager/Gunslinger first — these four have the highest cannibalization risk

Codex's pipeline recommendation is right. Codex's canonical-naming claim is wrong (sheets are accurate). Visual identity is fixable without touching the design doc.
