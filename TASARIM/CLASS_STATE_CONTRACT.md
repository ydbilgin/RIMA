# CLASS STATE CONTRACT (v0.1)
**Date:** 2026-04-30 | **Source:** Claude R4 final lock (Codex R4 ACCEPT_WITH_CHANGES)
**Status:** v0.1 draft. Iterative: v0.1 -> per-class pass -> v0.2 lock.

## Purpose
Single source of truth for every state in RIMA. For each state: who produces it, who consumes it, public vs internal, how it reads on screen, how bosses respond, and how abuse is capped. Cross-class skill design pulls from this doc only.

## State Taxonomy
- **Public state** = visible HUD/overlay, can be consumed by other classes (cross-class window).
- **Internal state** = class-private, no cross-class consumption (still visible to player as feedback).
- **Self state** = applies to caster only.
- **Hybrid** = public threshold gate over an internal accumulator.

Public count target: 12-16. Current: **14 PASS**.

## Ownership Lock (Critical)
| Term | Owner | Rule |
|---|---|---|
| Broken | Warblade public | Warblade-only producer |
| Sundered | Warblade internal | **Warblade-only producer**. Brawler/Shadowblade may consume |
| Cracked | Brawler public | Brawler-only producer |
| **Shattered** | Brawler internal | Cracked upgrade. Replaces "Brawler Sundered". Distinct from Warblade Sundered |
| Wounded | Ravager enemy public | enemy state |
| Blood Debt | Ravager self internal | caster state, distinct from Wounded |

---

## Mandatory Fields per State
Each state must define:
1. **Producer** -- which skill(s) apply it
2. **Consumers** -- which skill(s) read/consume it (own class + cross-class)
3. **Public/Internal** -- classification
4. **Readability** -- HUD/overlay/VFX cue
5. **Boss Rule** -- behavior vs boss-tier (often: no full effect, micro-stagger or stack-only)
6. **Abuse Cap** -- ICD, max stacks, decay, cooldown gate

---

## Warblade

### Broken (public)
- **Producer:** Iron Crush (6s window, stacks), Iron Charge (impact)
- **Consumers:** Warblade Death Blow execute gate; cross-class: Brawler Glass Strike consumes; Shadowblade Severance can chain
- **Readability:** red crack overlay on enemy chest, brief metal-snap SFX
- **Boss Rule:** Broken applies as **stack only**, no full stagger. Damage bonus halved (1.5x not 2x)
- **Abuse Cap:** 6s decay; max 3 stacks per enemy; ICD 0.4s between Iron Crush hits per target

### Sundered (internal)
- **Producer:** **Warblade only.** 3 stacks of Broken auto-converts to Sundered (Iron Crush threshold) OR Iron Roar AoE (Phase 2)
- **Consumers:** Death Blow execute gate; cross-class: Brawler Glass Strike consumes; Shadowblade Severance double-payoff
- **Readability:** orange-red split crack overlay, persistent until consumed or 8s timeout
- **Boss Rule:** boss takes **micro-stagger** on Sundered apply (0.4s), no full stagger. Damage 1.5x not 2x
- **Abuse Cap:** 1 Sundered per enemy at a time; consume relocks Broken stack counter

---

## Elementalist

### Burning (public)
- **Producer:** Fire skills (Pyre, Ember Burst, Fire Trail)
- **Consumers:** Elementalist Convergence Meter (fills LEFT bar); cross-class: Hexer Hex stacks +1 on burning target; Ranger Trap detonate bonus
- **Readability:** red ember particles + tick numbers (DoT)
- **Boss Rule:** boss receives Burning at 50% duration, full DoT scaling
- **Abuse Cap:** stack max 5; refresh duration not stack on reapply

### Frozen (public)
- **Producer:** Frost skills (Frost Lance, Glacier Fall, Frost Trail)
- **Consumers:** Convergence Meter (fills RIGHT bar); cross-class: Warblade Iron Charge shatter bonus; Ronin Iaido critical
- **Readability:** ice crystal overlay + slow VFX
- **Boss Rule:** boss never fully freezes. Slow only (30%), no movement lock
- **Abuse Cap:** Frozen lock max 1.5s on normal mob; ICD 4s per enemy on full freeze

### Converged / Lightbroken (internal hybrid)
- **Producer:** Convergence Meter both bars filled simultaneously -> Lightbreak system trigger
- **Consumers:** Trinity Storm (ulti), Lightbreak detonation
- **Readability:** **Convergence Meter** single bar HUD, white flash when full
- **Boss Rule:** Lightbreak detonation deals reduced burst (60%) on boss
- **Abuse Cap:** Convergence Meter resets after Lightbreak; cannot pre-fill before combat

---

## Shadowblade

### Rift Scar (public)
- **Producer:** RMB (basic rift cut), Dash (rift-dash variant), Mirror Cut (Phase 2)
- **Consumers:** Severance (execute gate via Scar collapse), Veil Flicker; cross-class: any high-damage hit on Scar triggers minor rift damage
- **Readability:** purple-blue line scar on enemy body, persists 6s
- **Boss Rule:** Scar applies but Severance execute disabled on boss; collapse damage 50%
- **Abuse Cap:** max 3 Scars per enemy; ICD 0.6s between Scar applications

### Collapsing (internal)
- **Producer:** Severance trigger on Scar
- **Consumers:** caster only; ends with Scar consume + damage burst
- **Readability:** Scar lines pulse white before collapse
- **Boss Rule:** Collapsing produces damage but no execute
- **Abuse Cap:** Collapsing windup 0.4s; cancels if caster takes hit

### Phased Through (internal self)
- **Producer:** Veil Flicker
- **Consumers:** caster invuln frame
- **Readability:** caster sprite flickers + afterimage trail
- **Boss Rule:** boss attacks still register hit-frames at end of phase (no full ignore)
- **Abuse Cap:** Smoke Veil Faz 1 minimum spec: projectile tracking break + aim cone reset + boss no-invis abuse. Phase max 0.6s; CD 8s

---

## Ranger

### Marked (public)
- **Producer:** Hawk Eye (Phase 2 -- merged into Final Strike aim upgrade), Wireline Trap, Mark shot
- **Consumers:** Final Strike execute gate (Marked + trap-triggered); Quiver Pulse spreads to other Marked; cross-class: Hexer Hex stacks +1
- **Readability:** small red diamond above enemy head
- **Boss Rule:** boss can be Marked but Final Strike execute requires trap-triggered AND Marked
- **Abuse Cap:** Marked persists 10s; max 1 Mark per enemy; refresh on reapply

### Trapped (public)
- **Producer:** Wireline Trap (Phase 2), Snare Bomb
- **Consumers:** Final Strike execute gate (trap-triggered = Trapped); Quiver Pulse damage bonus
- **Readability:** wire/rope line VFX, brief slow
- **Boss Rule:** boss takes Trapped as **slow only** (40%), no movement lock
- **Abuse Cap:** Trapped duration 2s; ICD 6s per enemy

### Snared (internal)
- **Producer:** Wireline Trap secondary effect
- **Consumers:** caster setup state only
- **Readability:** ground line VFX
- **Boss Rule:** boss ignores Snared root, slow only
- **Abuse Cap:** Snared max 1.5s on normal mob

---

## Ravager

### Wounded (public)
- **Producer:** Bloodied Roar, Carnage Spin, basic claw hits
- **Consumers:** Ravager Fury fill bonus; cross-class: Hexer Hex stacks +1; Brawler Glass Strike damage bonus
- **Readability:** red blood drip overlay + bleeding particle trail
- **Boss Rule:** boss receives Wounded at full DoT scaling but no panic state
- **Abuse Cap:** stack max 5; refresh duration not stack damage

### Frenzied (internal self)
- **Producer:** Fury MAX (Perfect Condition)
- **Consumers:** Bloodied Roar ulti, Carnage Spin ulti
- **Readability:** caster red aura + heavier breath SFX
- **Boss Rule:** Frenzied empowers caster vs boss but no extra damage scaling
- **Abuse Cap:** Frenzied window 6s; Fury drain on cast

### Blood Debt (internal self)
- **Producer:** Crimson Pact (Phase 2), Undying Tenacity proc
- **Consumers:** caster trade-off state, empowers Wounded application
- **Readability:** red HP missing bar overlay + red border vignette
- **Boss Rule:** Blood Debt is caster-side, boss unaffected directly
- **Abuse Cap:** Blood Debt max 30% missing HP cost; cannot stack with Undying Tenacity proc

---

## Ronin

### Opened (public)
- **Producer:** Wind Read on enemy whiff (Phase 2/Later), Sheath Pressure on parry (Later)
- **Consumers:** Iaido Strike crit bonus, Flash Draw bonus; cross-class: Shadowblade Severance double-Scar; Ranger Final Strike crit bonus
- **Readability:** white slash mark on enemy + brief Tension pulse
- **Boss Rule:** boss can be Opened but window halved (1.5s vs 3s normal)
- **Abuse Cap:** Opened window 3s; ICD 4s per enemy

### Draw Window (internal hybrid)
- **Producer:** Tension MAX (Perfect Condition) opens 1.5s draw window
- **Consumers:** Flash Draw ulti gate, Iaido Strike empowered
- **Readability:** Tension bar pulses gold + sword sheath glow
- **Boss Rule:** Draw Window applies normally; boss damage scaling reduced 25%
- **Abuse Cap:** window 1.5s; missing the window drains Tension to 50%

### Afterimage (internal self)
- **Producer:** Iaido Strike on dash-cancel
- **Consumers:** caster movement utility
- **Readability:** caster afterimage trail + sword glint
- **Boss Rule:** Afterimage works vs boss
- **Abuse Cap:** Afterimage CD 6s; one per dash chain

---

## Gunslinger

### Suppressed (public)
- **Producer:** **Empty Mag Burst (R4 lock)**, Heavy Round (Phase 2)
- **Consumers:** Deadshot crit bonus on Suppressed; cross-class: Ranger Marked synergy bonus damage
- **Readability:** smoke ring overlay on enemy + reduced enemy fire rate VFX
- **Boss Rule:** boss Suppressed reduces fire rate 20% only, no full silence
- **Abuse Cap:** Suppressed duration 3s; ICD 5s per enemy

### Exposed Line (internal)
- **Producer:** **Backfire Shot (R4 lock)**, line aim shot
- **Consumers:** Deadshot line aim execute, Reload Roll exit shot
- **Readability:** thin red laser line between caster and target, 0.4s wind-up
- **Boss Rule:** boss Exposed Line damage 1.5x not 2x
- **Abuse Cap:** Exposed Line breaks if caster moves laterally; ICD 2s

### Backfire (internal self) **NEW R4**
- **Producer:** Backfire Shot
- **Consumers:** caster trade-off state, Heat reset bonus
- **Readability:** caster smoke trail + minor self-damage tick
- **Boss Rule:** Backfire is self-only; boss unaffected
- **Abuse Cap:** Backfire DoT 3s; cannot stack

---

## Brawler

### Cracked (public)
- **Producer:** Pulverize, Shockwave Fist (R4: applies Off-Balance internal); LMB combo finisher
- **Consumers:** Liver Shot branch (Pulverize Cracked branch), Shattered upgrade gate; cross-class: Warblade Iron Crush bonus damage on Cracked
- **Readability:** white-blue crack overlay on enemy torso
- **Boss Rule:** boss Cracked applies but Liver Shot branch disabled, micro-stagger only
- **Abuse Cap:** Cracked decay 5s; max 3 stacks; 3 stacks -> Shattered conversion

### Wall-Slammed (public, Phase 2/Later)
- **Producer:** **Wall Eat (Pulverize wall finisher branch, R4 reorganized; was Wall Slam Combo)**
- **Consumers:** wall impact damage burst; cross-class: Warblade Iron Crush bonus on Wall-Slammed
- **Readability:** dust burst at wall + crack decal + camera shake + hit-stop
- **Boss Rule:** boss converts Wall-Slammed to **micro-stagger + Cracked refresh, no slide**
- **Abuse Cap:** **Required fallback if no wall in range -> Ground-Slammed (Cracked refresh + dust VFX, no slide).** Nav-safe sweep, no blind lerp through obstacles
- **Phase:** Wall Eat = **Later** (high collision/layout risk, NOT Phase 1 dependency)

### Pinned (internal)
- **Producer:** Liver Shot (Pulverize Cracked branch)
- **Consumers:** Brawler body-shot followup window
- **Readability:** brief enemy freeze + cracked overlay + body-shot opportunity glow. **No fake float.**
- **Boss Rule:** Pinned freeze max 0.3s on boss (vs 0.8s normal); body-shot bonus 1.5x not 2x
- **Abuse Cap:** Pinned ICD 4s per enemy; freeze auto-breaks if Brawler doesn't follow up in 1s

### Off-Balance (internal) **NEW R4**
- **Producer:** Shockwave Fist
- **Consumers:** Pulverize damage bonus, Liver Shot conversion bonus
- **Readability:** enemy stagger sway + dust ring at feet
- **Boss Rule:** Off-Balance reduces boss damage output 20% but no movement disrupt
- **Abuse Cap:** Off-Balance window 2.5s; ICD 5s per enemy

### Shattered (internal) **R4 RENAMED from "Brawler Sundered"**
- **Producer:** **Brawler only.** 3 Cracked stacks auto-converts to Shattered (Codex R4 ownership lock)
- **Consumers:** Glass Strike consumer, Pulverize finisher empowered
- **Readability:** glass-crack overlay (distinct from Warblade Sundered orange-red split)
- **Boss Rule:** boss Shattered applies as stack-only, micro-stagger 0.3s
- **Abuse Cap:** 1 Shattered per enemy; Glass Strike consumes; 6s timeout

---

## Summoner

### Corpse Field (public)
- **Producer:** enemy death within 6s window after Summoner kill or sacrifice
- **Consumers:** Mass Sacrifice ulti (Sacrifice Charge fill); cross-class: Hexer Cursed Link spread; Ravager Frenzied bonus on corpse
- **Readability:** dark mist ground overlay + corpse marker icons
- **Boss Rule:** boss death produces 1 Corpse Field tile only (not multi); ulti gain reduced 50%
- **Abuse Cap:** Corpse persists 6s; max 8 corpses tracked; **see SUMMONER_ECONOMY_RULES.md**

### Sacrifice Mark (internal)
- **Producer:** sacrifice cast on minion
- **Consumers:** Sacrifice Charge fill, Beacon Pull (Command Beacon upgrade) target
- **Readability:** sacrificed minion shows X-mark + brief soul VFX
- **Boss Rule:** boss unaffected directly
- **Abuse Cap:** **see SUMMONER_ECONOMY_RULES.md**

### Commanded (internal)
- **Producer:** Command Beacon, minion order
- **Consumers:** minion behavior state, Beacon Pull recall target
- **Readability:** minion glow ring under feet
- **Boss Rule:** boss unaffected
- **Abuse Cap:** Commanded persists until minion death or new order; **see SUMMONER_ECONOMY_RULES.md**

---

## Hexer

### Hexed (public)
- **Producer:** basic curse cast, Whisper Mark (Phase 2 passive aura), Bleed Tax
- **Consumers:** Hexblast (Hex Stack 10 = Perfect Condition gate), Hex Cascade spread; cross-class: Ravager Wounded gain bonus on Hexed; Ranger Marked combo
- **Readability:** purple smoke spiral around enemy + stack counter (1-10)
- **Boss Rule:** boss Hexed applies but **stack max 5**, not 10. Hexblast on boss = damage burst, no execute
- **Abuse Cap:** Hexed decay 8s; refresh on reapply; ICD 0.5s between Hex stack adds

### Overloaded (public hybrid)
- **Producer:** Hexed stack 10 reached -> Overloaded threshold
- **Consumers:** Hexblast empowered, Hex Cascade chain trigger
- **Readability:** purple lightning arc between Overloaded enemies
- **Boss Rule:** Overloaded does NOT trigger on boss (stack capped 5)
- **Abuse Cap:** Overloaded resets Hex stacks to 0 on consume; 4s window before auto-detonate

### Cursed Link (internal)
- **Producer:** Hex Cascade chain
- **Consumers:** chain damage routing, Hex spread vector
- **Readability:** purple line between linked enemies
- **Boss Rule:** boss can receive Cursed Link as link target but not link source
- **Abuse Cap:** chain max 5 enemies; chain damage falloff 70% per hop

---

## Cross-Class State Matrix Summary

| State | Owner | External Consumers |
|---|---|---|
| Broken | Warblade | Brawler Glass Strike, Shadowblade Severance |
| Sundered | Warblade | Brawler Glass Strike, Shadowblade Severance (double payoff) |
| Burning | Elementalist | Hexer Hex+1, Ranger Trap detonate |
| Frozen | Elementalist | Warblade Iron Charge, Ronin Iaido crit |
| Rift Scar | Shadowblade | (any class) high-damage hit triggers rift damage |
| Marked | Ranger | Hexer Hex+1 |
| Trapped | Ranger | (Ranger-only consumer for now) |
| Wounded | Ravager | Hexer Hex+1, Brawler Glass Strike bonus |
| Opened | Ronin | Shadowblade Severance double-Scar, Ranger Final Strike crit |
| Suppressed | Gunslinger | Ranger Marked synergy |
| Cracked | Brawler | Warblade Iron Crush bonus |
| Wall-Slammed | Brawler | Warblade Iron Crush bonus |
| Corpse Field | Summoner | Hexer Cursed Link spread, Ravager Frenzied bonus |
| Hexed | Hexer | Ravager Wounded bonus, Ranger Marked combo |

---

## Boss Rule Pattern (Universal)
1. **No execute** on boss-tier (Broken/Sundered/Marked+Trapped/Tension MAX/Scar/Hex 10 all gated)
2. **Stack-only** apply (no full stagger/freeze/lock)
3. **Damage scaling reduced** (typical 1.5x not 2x)
4. **Micro-stagger** (0.3-0.4s) replaces full stagger
5. **Phase 1 demo:** Penitent Sovereign already follows this pattern

## Abuse Cap Pattern (Universal)
1. **ICD** (internal cooldown) per enemy on state apply
2. **Decay timer** (5-10s typical)
3. **Stack ceiling** (3-10 depending on state)
4. **Refresh, not stack** on reapply (most states)
5. **No CD reset loops** (movement skill rule extends here: no infinite state apply chains)

---

## Open Questions (v0.1 -> v0.2)
1. Shadowblade Scar mechanic integration -- still shallow per CURRENT_STATUS. R3 partial: only RMB + Dash create Scars. **TODO Phase 2:** Mirror Cut + Scar Echo passive expand Scar producers.
2. Ravager Pain Reservoir interaction with Blood Debt cap -- needs numerical pass.
3. Ronin Sheath Pressure proximity tuning -- "Later" tag.
4. Hexer Whisper Mark aura range vs Cursed Link range -- needs combat test.

## Versioning
- v0.1 (this doc, 2026-04-30): R4 lock applied. State ownership locked. Boss/abuse patterns universal.
- v0.2 (target after Phase 1 playtest): tune numerical caps based on PlaytestScenarios data.
