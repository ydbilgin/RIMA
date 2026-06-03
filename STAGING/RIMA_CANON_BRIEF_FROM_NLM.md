# RIMA Canonical Design Brief (extracted from NotebookLM, 2026-05-30)

> This is the AUTHORITATIVE design spec pulled from the RIMA design notebook (NLM 30ddffa5). Drift hierarchy: **NLM canon > local memory > code**. Where code disagrees with this, canon wins unless there's a deliberate, documented reason. Use THIS to re-evaluate the skill/mechanic system.

## 1. Classes — 10 canonical, each a distinct resource + fantasy + [V] burst
| Class | Fantasy | Resource (how it fills) | [V] Burst |
|---|---|---|---|
| Warblade | "Approach. Pin. Armor-break. Execute." | **Rage** 0-100 — fills by dealing damage / CC; decays out of combat | Bladestorm |
| Elementalist | "I burn everything. But first I find the rhythm." | **Mana + Elemental State** (Fire/Frost stacks → "Lightbreak" synthesis) | — |
| Shadowblade | "You don't see it. It's already late." | **Sever** — fills by geometric phase-through movement + leaving **Rift Scars** (NOT by hitting) | — |
| Ranger | "You can't reach. You lose every second." | **Focus** — distance discipline: regen at 4m+, drains in melee | — |
| Ravager | "More dangerous at low health." | **Fury** — fills ONLY by taking damage (low-HP aggression) | — |
| Ronin | "Draw. Cut. Sheath. In one breath." | **Tension** — accumulates while sheathed → frame-perfect counter | — |
| Gunslinger | "No bullets. No time." | **Heat** — shoot-and-vent; Heat ZERO = ultimate condition | — |
| Brawler | "If you fall, get up. My fist rises first." | **Charge** stacks via melee combo timing → Overdrive via weaves | — |
| Summoner | "I sacrifice. The moment of sacrifice is strongest." | **Charges** — regen over time / instant on minion death/sacrifice | — |
| Hexer | "Patience. At 10, you are finished." | **Hex Stacks** 0-10 per enemy — escalate from debuff to lethal | — |

## 2. Skill design philosophy (CANON)
- **NO skill trees.** Run-based draft only.
- **Exactly 12 skills per class.**
- **6-Line Test:** an ACTIVE skill must create a readable combat EVENT — apply/consume a state, create a zone, or move the player. Skills that only give flat damage/buff are **converted to PASSIVES**.
- **Active taxonomy (4):** STRIKE (instant heavy) · ZONE (persistent ground entity) · REACTIVE (auto-trigger on HP/hit/dodge) · STATE (temporary mode change).
- **Passive taxonomy (3):** KEYSTONE (1 required per run, fundamentally alters the kit) · MODIFIER (2 optional always-on) · RESONANCE (cross-class synergy bridges).
- **Tiers:** Common · Rare (+30% + minor mechanic) · Epic (+60% + meaningful mechanic) · Legendary (fundamentally changes behavior). **NO "Mythic" tier in canon.**
- **Tags = TWO axes:** SHAPE (melee/line/cone/dash) + STATE (broken/burning/scar/marked). State-tags drive synergies.

## 3. Combat pillars (CANON)
- **3 verbs per class** (Warblade: engage/break/execute · Elementalist: switch/shape/detonate).
- **VFX-first:** hide static weapon sprite during swing → big painterly slash-arc VFX. Timings locked on graybox before art.
- **Juice:** hit-stop 0.04s normal / 0.12s+ crit-kill · directional screen shake · floating damage numbers · white hit-flash.
- **Fluidity:** input buffering · animation cancel (dash-strike) · coyote time · hitbox leniency (player weapon bigger, enemy projectiles smaller than visual).
- **"Commit-Beat":** cross-class synergies + unique effects trigger ONLY on the final hit of a class's basic combo (basic attacks = deliberate rhythm, not spam).

## 4. Roguelite loop (CANON)
- Clear room → cyan **Map Fragment** drops → pick up reveals next 1-2 room types → opens **Hades-style 3-card draft** (new active / upgrade / passive / tag-synergy / risk-modifier) → selecting unlocks exit gates.
- **HOOK = Dual-Class Break:** after Act-1 boss the game "breaks in half" — add a Secondary Class (+2 active slots, cross-class passives + ultimates).
- **Meta:** collect "Shattered Echoes" to permanently unlock classes + hub upgrades ("recovering fragmented faces of the past").

## 5. Demo scope vs full game (CANON)
- **Phase-1 Demo (~10-min vertical slice, wishlist):** 1 class (Warblade), 1 Act = 5 rooms (3 combat + 1 reward vestibule + Act-1 boss Penitent Sovereign at 50% HP, 1-phase placeholder), ~4 enemy types, draft restricted to **Common tier**, Death/Victory + Wishlist CTA.
- **Full game (Phase-5):** 10 classes, 3 Acts + Final Boss (The Architect, 4 phases), 90 cross-class ultimates, 90 cross-class passives, Epic/Legendary tiers, 4 difficulty modes, Grudge/Nemesis (bosses adapt to how you kill them), meta hub.

## 6. CODE ↔ CANON DRIFTS (Opus-identified from code read — re-verify & extend)
1. **Skill count:** canon = **12/class**; code = 14-22/class → over-produced. Prune to 12 or document deviation.
2. **Tier enum:** code has **"Mythic"**; canon has none (Common/Rare/Epic/Legendary) → remove or justify.
3. **Tags:** code = one flat enum mixing shape+element; canon = **two axes shape + STATE (broken/burning/scar/marked)**. Code has NO state-tags → canon's synergy system can't function.
4. **Taxonomy:** canon STRIKE/ZONE/REACTIVE/STATE + KEYSTONE/MODIFIER/RESONANCE; code = only `isPassive` bool → architectural gap. Many code "actives" are flat-damage → should be passives per 6-Line Test.
5. **Commit-Beat:** code's dead 0.6s combo-window (only logs) = should BE the canon Commit-Beat (final-combo-hit trigger for synergies).
6. **Per-class resource:** code has only RageSystem; canon needs 10 distinct resources (Sever/Focus/Fury/Tension/Heat/Charge/Charges/Hex/Mana+ElementalState).

## 7. Round-1 agy (design) key findings (for continuity)
- Over-produced breadth confirmed (~50 demo-unplayable skills); IronCharge≈BladeRush redundancy.
- Zero combat-synergy (Fire doesn't interact with Frost; tags only bias draft, not combat).
- Rage decay too aggressive → frantic rush vs deliberate swing frames.
- Rift-Break currently a macro room-transition, NOT a combat verb → missing "hook."
- agy generative: **Rift-Scar Detonate** signature verb (aligns w/ canon Shadowblade Rift-Scars + Commit-Beat), Cross-Class Resonance Proc Matrix, Echo Memory bias.
- Bank pickups: M68 synergy-UI, M204 aggression-heal, M205 rolling-health, M208 slot-cap, M211 env-vulnerability boss.

## 8. Round-1 cx (systems) — CONFIRMED corrections to the drift list
- **Drift #6 was WRONG:** it is NOT "only RageSystem." CONFIRMED **6 resource systems exist**: `RageSystem`, `ManaSystem`, `EnergySystem`, `FocusSystem`, `TensionSystem`, `ComboPointSystem`. Problem = **coherence, not absence**: Rage + Focus are identity-rich (Focus: far-regen/close-decay matches canon); Mana/Energy are generic regen bars; **Shadowblade's fantasy is FRAGMENTED** across Energy + Sever + RiftScar + ComboPoint.
- **"Duplicate SkillData" = two definition TRACKS:** live `RIMA.SkillData` (`Skills/SkillData.cs`) vs newer `RIMA.Combat.Skills.ActiveSkillData` (`Combat/Skills/SkillData.cs`). Not a literal name clash now — conceptual duplication. Pick ONE (demo-blocker).
- **Tags are INERT:** `SkillDatabase.Build()` wires skillType/tier/class/cooldown/passive but **never populates `SkillTag[] tags` or `appliesEffect`**. So tags do literally nothing.
- **Skill counts CONFIRMED:** 14 Warblade / 16 Elementalist / 24 Shadowblade / 22 Ranger + **4 Ronin** (`Combat/Classes/Ronin/Skills/`). PlayerClassManager can enable WB/Elem/Shadow/Ranger/**Ronin**; SkillDatabase offers only WB/Elem/Shadow/Ranger + neutral passives. Ravager/Gunslinger/Brawler have basic-attack hints only; Summoner/Hexer enum-only.
- **DraftManager hardcodes Warblade** (caches `Warblade_SkillController`, WB slot count, WB secondary unlock) → blocks primary-class correctness. Fix = `IClassSkillController` interface.
- **Combo = TWO systems:** the 0.6s log in `Warblade_SkillController.TryUse` is dead → CUT. Real path = `BasicAttackProfile.comboLength/comboWindow` + `MeleeChainBehavior` + `Beat3CommitTrigger` + `CombatHandler.OnCommitBeat` → KEEP.
- cx architecture: `SkillDefinitionSO` single-source (skillId/classType/activeType/tier/tags/slotKind/resourceCosts/statesProduced/statesConsumed/effects/cooldown/upgradeTracks/offerCategory) + `ClassDefinitionSO` + `OfferRule` assets.
- cx bank pickups: M208 (highest), M68, M204, M205, M207, M211. (Converges with agy.)

## 9. Round-1 Opus (deep design) — verdict
- **Core finding:** "the one system that makes RIMA *RIMA* is the one that's a `Debug.Log`." Commit-beat IS implemented (`Beat3CommitTrigger`→`OnCommitBeat`) but consumed ONLY by juice (VFX/shake/hit-pause) — **no gameplay payload** (`CommitBeatEvent` has no target/family).
- **Canon detail beyond the brief:** canon defines a **9-family tag system** (Fracture/Veil/Pierce/Bleed/Echo/Cut/Pressure/Strike/Rift) with **3+ families → Rift proc (50% resist ignore)** capstone = the "I built something" engine, UNBUILT. Canon's Warblade DEMO doc lists **12 skills with explicit CHAIN-BONUS windows** (Iron Charge→Crippling Blow→Iron Crush; "Iron Charge → next hit +80% on stunned") — the demo's real depth, UNIMPLEMENTED.
- **Scope verdict:** DEPTH not breadth — 4 classes × 70 skills has STARVED the demo. Ship ONE class (Warblade) deep; freeze the other 70 skills OUT of demo draft (keep in repo = Phase-2 capital, do NOT delete).
- **Signature verb:** "BREAK, then EXECUTE" — commit-beat applies **Sundered** (state exists in `SkillStateTracker`), Sundered gates chain-bonus payoff, executing Sundered/low-HP refunds Rage + extends out-of-combat drain grace = "aggression pays for itself."
- **Target arch:** add `roleTags[]` SEPARATE from damage-type tags (don't break 70 assets); demo activates only Fracture-family + chain-window path; FamilyTagSystem/Rift/cross-class exist as no-ops → Phase-2 = wiring not rewriting. New system needed = `ChainWindowTracker`.
- Opus model = **"The Sundered Beat"**: collapse identity to one verb felt every ~3s (BREAK); the canon's 3 verbs (engage/break/execute) become a closed rage-economy loop; Fracture is the first of the 9 families, Rift = "break 3 ways at once."

---

## ROUND-2 RE-THINK INSTRUCTION (for agy + cx + Opus)
You ALL already queried NLM in round-1 and CONVERGED. This round: react to the CONSOLIDATED picture above (canon §1-5 + drifts §6 + all peers §7-9). Specifically:
1. **Challenge or confirm** each peer's load-bearing claims — anything wrong, overstated, or missed?
2. **Resolve the open contradictions** (e.g. cut vs keep which combo; SkillData vs ActiveSkillData as the single track; how much resource-system unification belongs in the DEMO vs Phase-2).
3. **Lock the DEMO-scope reorg** (Warblade-only): exact, ordered, minimal change-set to get "I built something" + signature verb live — distinguish demo-blocker vs Phase-2.
4. Keep your own lens (cx=systems/data-model, agy=design/feel, Opus=cross-system architecture). Be decisive; this feeds the FINAL lock.
