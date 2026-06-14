# RIMA Skill & Mechanic — FINAL REPORT (design verdict + skill feasibility)
**2026-05-31 · Opus synthesis of 4-AI eval (Sonnet + cx + agy + Opus-subagent) + NLM canon + code ground-truth.**
Inputs: `RIMA_CANON_BRIEF_FROM_NLM.md` (NLM canon §1-5, drifts §6, all round-1/2 agent findings §7-9). Verified against code: `SkillDatabase.cs`, `IronCharge.cs`, `RageSystem.cs`, `Warblade_SkillController.cs`.

---

## A. EXECUTIVE VERDICT
The skill/mechanic system is **architecturally sound but built at the wrong layer**: the team produced Phase-2 **breadth** (76 skills, 6 resources, 10-class enum) while the Phase-1 **depth** layer is empty stubs. The single sentence: **the design intent is written into the skill descriptions, but the mechanic that delivers it is not in the code.** No rewrite needed — *wire the depth + scope to Warblade*. Nothing gets deleted (the 76 skills = Phase-2 capital).

**Does it need reorganization?** YES — but it's ~6 surgical changes + a scope-freeze, not a rebuild.

---

## B. DESIGN LOCK (condensed)
- **Signature verb (DEMO) = "Sundered Beat" (BREAK → EXECUTE).** Canon Warblade: *Approach · Pin · Armor-break · Execute* / Rage / Bladestorm[V].
- **Demo loop:** Engage (`Iron Charge`) → Build (basic combo + CC → Rage) → **Break** (`Sunder Mark`/`Earthsplitter` → Sundered/Broken state) → **Peak** (Rage 100 → `Bladestorm` climax, 6s — agy's hook, = canon [V]) → **Execute** (`Death Blow` on Sundered/low-HP → bonus dmg + Rage refund + heal + drain-grace).
- **agy's "Rift-Scar Detonate" = same engine, Shadowblade's verb → Phase-2 reskin** (Sundered=Fracture family; Rift-Scar=Rift capstone). One `ChainWindowTracker`, no blur.
- **Scope LOCKED:** ship ONE Warblade deep; freeze the other ~50 cross-class skills + higher tiers out of the draft (canon §5: demo = 1 class, Common-tier only). Do NOT delete.

---

## C. SKILL FEASIBILITY MATRIX (code-grounded)

### Tier 1 — SHIP-READY for demo (real, functional code)
| Skills | Status | Evidence |
|---|---|---|
| **WB 12 actives** (Iron Charge, Cleave, Deep Wound, Sunder Mark, Crippling Blow, Earthsplitter, Blade Rush, Gravity Cleave, Iron Counter, Iron Crush, Battle Surge, Death Blow) | **Functional** — real impl, not stubs | `IronCharge.cs` = full charge+CircleCast+damage+Rage+stun+knockback. SkillDatabase builds all 12. |
| **WB 5 passives** (Ironclade Momentum, Blood Drinker, Wrath Protocol, Tempered Fury, Berserker's Resolve) | Functional, leveled | SkillDatabase + PassiveBase |
| **10 Neutral passives** (Iron Body, Berserker's Blood, Predator's Eye, War Veteran, Unyielding, Battle Scars, Adrenaline Rush, Ancient Instinct, Opportunistic Strike, Veteran's Scar) | Functional, available to all | `ClassType.None` pool |
| **Draft scoping** | **Already works** (less work than feared) | `SkillDatabase.GetPool(primary, secondary)` filters by class + None |

### Tier 2 — EXISTS but PHASE-2 (real classes, demo-frozen)
| Skills | Why not demo-feasible |
|---|---|
| **Elementalist 12, Shadowblade 13, Ranger 11** (~36 offered + more in repo) | Labeled `// cross-class havuz` in SkillDatabase. Depend on per-class resources: Focus is identity-rich, **Mana/Energy generic, Shadowblade's Sever/RiftScar/ComboPoint fragmented**. Cross-class payoff path `CrossClassSkillManager.ApplyEffect()` = **`Debug.Log` stub**. Demo is Warblade-only. **Keep frozen.** |

### Tier 3 — NOT feasible (skeletons)
| Class | State |
|---|---|
| Ronin (4 skills) | partial, `Combat/Classes/Ronin/`, not offered |
| Ravager / Gunslinger / Brawler | basic-attack behavior hints only (MarkPulse/HeatGauge), no controller |
| Summoner / Hexer | enum-only |

### 🔴 Feasibility blockers found IN CODE (the real report)
1. **Chain bonuses are PROMISED in descriptions, ABSENT in code.** Sunder Mark "Death Blow aktifken -%60", Crippling Blow "Death Blow zinciriyle %600", Iron Crush "Bladestorm zinciriyle katlanır" — the chain table is written into the data but **no `ChainWindowTracker` exists**. The descriptions over-promise. **This is the #1 demo gap** — the "I built something" depth is documented, not wired.
2. **Tags & appliesEffect are INERT.** `SkillDatabase.Add()` never sets `tags[]` or `appliesEffect`. So tag-synergy and status-routing cannot function — the data field exists, nothing reads/writes it.
3. **Commit-beat carries no gameplay payload.** `Beat3CommitTrigger`→`OnCommitBeat` fires but `CommitBeatEvent` = {worldPos, attacker, beatIndex}; consumed only by VFX/shake/hit-pause. No target, no state application.
4. **⚠️ LAYER-MASK RISK (must verify in play).** `IronCharge` hit-scans `LayerMask.GetMask("Default")`. The recent depth-sort/collider refactor moved entities to **"Entities" + Player(10)/Enemy(11)** layers → **WB skills may MISS enemies now**. Verify every WB skill's hit-layer in a live play test — this could silently break combat.
5. **Retired-skill blacklist is HARDCODED** (`IsRetiredOfferSkill` = 10 names: Cleave, Blade Rush, Chain Lightning, Arcane Blast, Backstab, Shadow Step, Fan of Knives, Aimed Shot, Disengage, Multi Shot). Tech debt → should be a data flag (Phase-2).
6. **Two skill-definition tracks.** Live = rich `RIMA.SkillData` (used by SkillDatabase/Draft/UI/passives). `RIMA.Combat.Skills.ActiveSkillData` = thin stub (name/icon/cooldown only). **DECISION: keep `RIMA.SkillData`, evolve → `SkillDefinitionSO`; mark `ActiveSkillData` `[Obsolete]`.** (Resolves the Opus↔cx contradiction in cx's favor — cx read the actual schemas.)
7. **Tier enum has "Mythic"** (canon: Common/Rare/Epic/Legendary). Keep enum, just don't offer Mythic in demo.

---

## D. DEMO REORG — ordered change-set (feasibility-rated)
| # | Change | Files | Block | Effort | Risk |
|---|---|---|---|---|---|
| 0 | **VERIFY WB skill hit-layers in play** (blocker #4) | play-test + skill `LayerMask`s | ⛔ FIRST | XS | **HIGH if broken** |
| 1 | `roleTags[]` + RoleTag enum on `RIMA.SkillData`; seed Fracture on WB chain skills | SkillData.cs, SkillDatabase.cs | ⛔ | S | low |
| 2 | `CommitBeatEvent` + target/context; WB beat-3 applies **Sundered** to hit enemy | CombatEventBus.cs, MeleeChainBehavior/CombatHandler.cs, SkillStateTracker.cs | ⛔ | M | med |
| 3 | **`ChainWindowTracker`** (new) — named windows (IronChargeNextHit/SunderedExecute), wire canon chain table | new + IronCharge/CripplingBlow/IronCrush/DeathBlow/SunderMark | ⛔ | M | med |
| 4 | Cut dead 0.6s combo-log; keep commit-beat path | Warblade_SkillController.cs | ⛔ | XS | none |
| 5 | Rage payoff: execute → refund + drain-grace (+M204 heal) | DeathBlow.cs, RageSystem.cs | ⛔ | S | low |
| 6 | Restrict draft to 12 WB Common-only; freeze rest (no delete) | SkillDatabase.cs, SkillOfferGenerator.cs | ⛔ | S | low |
| 7 | `IClassSkillController` interface (decouple DraftManager's WB hardcode) | new + Warblade_SkillController.cs, DraftManager.cs | ⚠ optional-now | S-M | low |
| — | **Phase-2:** SkillDefinitionSO/ClassDefinitionSO/OfferRule · 9-family+Rift proc · resource unification · cross-class ApplyEffect · retired-blacklist→data · Mythic cleanup · 14→12 prune | — | ⏭ | L | — |

---

## E. FEEL TUNING (agy proposal — verify vs code)
- **Rage decay:** 10/s@1.5s → **8/s @ 2.5s** (deliberate combat breathing room).
- **Commit-beat:** 3-hit, 0.8s final-hit buffer, 3rd hit = **1.5× + 0.08s hitstop + Fracture**.
- **Hitstop tiers:** 0.04 light / **0.08** heavy-commit-skill / **0.15** crit-`Death Blow` (+0.08s white-flash).
- **Sweet-spot (M207):** outer **15%** of hitbox → **+30% dmg + 5 Rage** + golden spark + rumble.

## F. BANK PICKUPS (demo vs Phase-2 — resolved)
- **Demo:** **M68** synergy-UI (draft chain-link lines — else the chain system is invisible) · **M207** sweet-spot · **M204** execute→Rage-refund+heal.
- **Phase-2:** **M208** slot-cap (agy: no slot pressure in 4-slot Warblade demo) · **M211** boss env-vuln (too much for 5 rooms) · **M205** rolling-health (defensive — Ravager-flavored).

## G. PHASE-2 BACKLOG (frozen, NOT deleted)
9 resource classes' coherence pass (Shadowblade defrag) · 9-family tag + Rift capstone · cross-class Resonance (90 passives/ultimates) · the other 5 classes · Dual-Class Break hook · Mythic→Legendary · physical 14→12 prune · retired-skill data-flag.

---

## H. BOTTOM LINE
**The demo is closer than the breadth suggests.** Tier-1 (12 WB actives + 15 passives + working draft scoping) is real and playable. The gap is 1 new system (`ChainWindowTracker`) + 1 payload (`CommitBeatEvent`) + a hit-layer verify + a scope-freeze. The skills' designed depth already exists *as promises in the descriptions* — the job is to make the code honor what the data already says. **Highest-risk unknown = the layer-mask hit-miss (step 0): verify in play before any other work.**

---

# PART 2 — COMBO DEPTH (3-AI: cx + agy + Opus, 2026-05-31)

**User question:** more skills? compatible? comboable? many combos? **Answers (converged):**
1. **More skills? NO** — combo richness = interaction-matrix density, not skill count. Canon locks 12/class. A 13th lowers the interlock ratio.
2. **Compatible? On paper YES, in code NO** — descriptions promise chains (Crippling→Death Blow %600, Sunder→Death Blow) but `tags` inert, no `ChainWindowTracker`, commit-beat no payload → skills are isolated triggers.
3. **Comboable? Not yet — one bridge away.** `SkillStateTracker` (state container) + commit-beat (rhythm) exist; missing = the bridge (commit-beat → target+state payload, ChainWindowTracker reads "is target Sundered / in window"). Build it once → all promised combos light up.
4. **Many combos? Demo = a deliberate 5-7; Phase-2 = hundreds structurally** (cross-class state-sharing + 9-family, NOT hand-authored). Canon wants few-deep-signature, not an encyclopedia.

**cx quantified:** meaningful combos ≈ Producer × Consumer × Density. 12 skills + 7 states @ 20-30% density → 12-25 combos; @40% → 30-40 + 5-10 three-step routes. Highest ROI = retrofit the existing 12, don't expand pool.

**agy's key distinction — KINETIC vs MATHEMATICAL combos:** Kinetic (Iron Charge→stun→strike, Gravity Cleave pull→Earthsplitter AoE) change positioning, feel physical — the demo headline. Mathematical (Sunder→Iron Crush +30%, Crush→Wound 2× tick) are backend scaling — min-max depth, not headline.

**State vocabulary (demo = 3 live states): Stun · Sundered(armor) · Rage.** Design rule: each active must PRODUCE one state + CONSUME another to interlock.
- Producers: Iron Charge, Sunder Mark, Earthsplitter, Crippling Blow, Gravity Cleave
- Consumers: Death Blow, Bladestorm, Iron Crush
- **Isolated dead-ends → re-author (NOT add skills):** Cleave (→ Sundered-spreader), Deep Wound (→ "Gore Cleave": Sundered target's Bleed becomes Hemorrhage 3× detonate), Battle Surge (→ "Slayer's Feast" passive: execute Sundered/Broken → +20 Rage +5% heal = M204), Iron Counter (→ parry applies Sundered + max Rage). Iron Counter may stay pure utility.
- **Target interlock ratio ~8/12** (currently ~4). Adding skills lowers it; re-authoring raises it.

**Demo combo target = 5-7 named combos.** Roster stays 12 (canon); **demo DRAFT POOL = curated ~6-8 high-interlock skills** (freeze filler from the offer, don't delete). Named: Charge→next-hit · Sunder→Death Blow · Crippling→Death Blow · Iron Crush→Bladestorm · Earthsplitter→Death Blow · + 2 re-authored joints.

**Combo readability requirement (agy):** Sundered/Broken enemies need a PROMINENT visual tell (shattered-armor particles, cracking glow) — "can't combo what you can't see." Pairs with M68 synergy-UI. Add to the change-set.

**Phase-2 "many" structural math:** 45 class-pairs × 2 signatures = 90 cross-class ultimates (canon). 9 families C(9,3)=84 triples → Rift proc. Cross-class rule: A state-setup → B state-consume = an EVENT, not a multiplier. 3 "wow" examples: Sundered+Fire=Thermite, Broken+Pierce=Shrapnel Ricochet, RiftScar+DeathBlow=Dimensional Collapse.

**Guardrails:** anti-soup = states/family apply ONLY on commit-beat (3rd hit) + 1.2s proc floor (no spam-to-Rift). anti-dominant-combo = encounter-shape (mixed HP/count); execute-combos are finishers, Bladestorm is the sustained peak. Phase-2 caps: 2 public states/class, per-target ICDs, decay windows, boss-downgrades, Rift CD floor.

**Combo addendum bottom line:** skills aren't too few — they don't INTERLOCK. The combo fix is the SAME `ChainWindowTracker` + commit-beat payload already in the Part-1 change-set (steps 2-3), PLUS declaring `statesProduced`/`statesConsumed` in data + re-authoring 2-3 dead-ends + visual tells. NO new system beyond Part 1. This is downstream of step-0 (hit-layer verify) and basic playability — design is LOCKED and ready, implementation is NOT the current critical path.
