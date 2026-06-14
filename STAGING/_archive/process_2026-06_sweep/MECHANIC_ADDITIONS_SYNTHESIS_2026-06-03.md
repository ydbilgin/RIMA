# RIMA — Mechanic Additions Synthesis (2026-06-03)

**Source bank:** `F:\LaurethStudio\03_IDEAS\MECHANIC_BANK\Youtube_60_Mechanics.md` (now complete, 61 mechanics).
**Method:** 4-agent fan-out (orchestrator = Opus). Agents: ax Gemini 3.1 Pro (video↔file check), rima-sonnet (RIMA-fit), cx xhigh (codebase feasibility, file/line-level), rima-design Opus (deep canon synthesis + NLM). This doc = the orchestrator's combined synthesis.

---

## 0. Video-research consistency finding (user's question)
The notes file claimed "60" but had only **37** entries; the video actually has **61**. The 37 were **accurate** — the author simply stopped midway. **No fabrication, just incompleteness.** Missing 24 (#38–61) recovered + merged back into the source file. Several of the recovered 24 are strong roguelite/card fits (see §4).

---

## 1. CONVERGENCE — what all agents agree on (the signal)
Cross-referencing the 3 evaluators (rima-sonnet fit · cx feasibility · rima-design design):

| # | Mechanic | rima-sonnet | cx feasibility (evidence) | rima-design | **VERDICT** |
|---|---|---|---|---|---|
| **14** | **Dynamic Wave Spawn** (next wave @50–70% cleared) | top, easy | **TRIVIAL — near-done**: `EncounterController.nextWaveKillFraction=0.5` already exists | TOP-5 #2, **highest combat-feel ROI** | **SHIP FIRST** |
| **26** | **Card Weight / Draw Rate** (draft cards weighted; heavy=rarer/slower reveal) | top, med | **EASY**: `SkillOfferGenerator.WeightedPick` + tier weights exist | TOP-5 #5 (staged cyan reveal = dopamine) | **SHIP 2nd** |
| **17** | **Echo Mote Heal** (state-gated execute kills drop 1–3% HP motes) | top, easy | **EASY**: `Health.Heal` + `RewardPickup`/`DraftManager` hooks | TOP-5 #3 — **solves the "no in-combat heal" retention gap** | **SHIP 3rd** |
| **7+33** | **Sundered Counter** (dash INTO cyan-tagged attack = parry + applies YOUR class's BREAK stack) | top, easy-med | MED each (`PlayerController.TryDash`/`DashEvent`, `IronCounter`, `MobAttack_Throw`) | **TOP-5 #1 flagship** — routes evasion INTO BREAK→EXECUTE | **SHIP 4th** (gates Big Idea) |
| 2 | Action-Based Reward (EXECUTE/BREAK/parry → bonus) | top | EASY/MED (`CombatEventBus.OnHit/OnKill/OnDash/OnCommitBeat`) | folded into others | secondary |
| 32 | Mid-Fight Hacking (skill-check on Broken enemy → ×2 execute) | top, med | MED-HARD (BREAK system real: `SkillStateTracker`, `DeathBlow`) | not top | later |
| 11 | Extraction Roguelite (permanent loss + "extract now") | top, med | **HARD** (no extraction inventory; `Portal` transition TODO) | risky | **design pass first** |

**Bottom line:** #14, #26, #17 are low-risk wins where the infrastructure ALREADY exists (cx confirmed file-level). #7+#33 is the higher-ambition flagship that unlocks the Big Idea.

---

## 2. THE BIG IDEA — "Cyan Echo Anchor" (rima-design original)
Combine **#33 Dash-as-Parry + #13 Anchor Webbing + cyan-seal lore + cliff geometry + cross-class states**:

A successful dash-parry (Sundered Counter) plants a persistent **cyan Anchor Thread** (~6s) at the parry point. It does 3 things:
1. **Pull-strike combo:** light-attack toward the anchor = free dash-strike that consumes 1 state stack (BREAK → ANCHOR → BREAK → EXECUTE — adds a rhythmic measure to Sundered Beat).
2. **Cliff kill-floor:** anchor on a floor-edge cell pulls the next crossing enemy 1.5 tiles toward the void → **finally makes RIMA's cliff geometry MATTER** (currently cosmetic).
3. **Cross-class detonation:** a teammate class-tag touching the anchor detonates a cyan ring applying the origin class's state in radius → **first cross-class skill that's a SPATIAL SHAPE, not a number.**

Respects all locked rules (no HP-execute gate, no CD reset, boss exemptions, OWNS/AVOIDS state ownership). Build AFTER #7+#33 lands + one demo session confirms feel. Needs `TASARIM/CYAN_ANCHOR_SYSTEM.md` first.

---

## 3. RISKY (record, don't build yet)
- **Symptom Triage / "Echo Salvage"** (3-way corpse judgment) — interrupt-the-fight UI, StS-creep risk. Only if outside combat rooms.
- **Action-Speed time-warp during Ultis** (#8) — "this build is insane" feel, but collides with locked 50–150ms hit-stop `Time.timeScale` channel; do via player-rig anim speed only, prototype first.
- **Void Mirror Layer** (#4 Sintopia) — brilliant Act 2/3 variety but huge scope (second pass of every room). Phase 3 wishlist; the small version (single Void room/Act) already exists as Rift Portals.

## 3b. SKIP
- #9 Co-op Hot Potato (single-player). · #19 Match-3 Base Defense (genre incoherence). · #3/#35 QTE typing/charge-jump (collides with rhythmic commit-beat). · #2 mission-timer variant (runs aren't timed).

---

## 4. FROM THE RECOVERED 24 (#38–61) — roguelite/card additions the design agents didn't see
These only existed in the recovered half; worth a second synthesis pass:
- **#59 Custom Death Card (Inscryption)** — on death, fuse deck cards into a permanent boss/relic for next run → maps onto **Shattered Echoes meta-currency**. Strong roguelite meta-progression fit.
- **#60 Self-Damage Bash (Shroom & Gloom)** — sacrifice HP to break a locked door → **anti-softlock for RIMA's portal/key gating**. Cheap, thematic.
- **#45 Stamina-Based Deck Building (Nutmeg)** — power-as-stamina gating expensive skills → alt resource lever for the draft economy.
- **#61 Persistent Nightmare Fuel (Bag of Dreams)** — rising cross-room "dread meter" that stuffs your deck with debuffs → escalation/tension system for late Acts.

---

## 5. DISPATCH ORDER (orchestrator)
1. **#14 Dynamic Wave %** — smallest scope, biggest ROI, infra exists. (`rima-doc` updates ODA_MEKANIGI §4.1 + diegetic "mobs ride cyan rift-cracks from void edges"; trivial codex on existing wave manager.)
2. **#26 Card Weight** — pure UI/data; add Weight int to `SkillData`, multiply into `WeightedPick`.
3. **#17 Echo Mote Heal** — one universal "enemy died while state X held" hook off the combat event bus.
4. **#7+#33 Sundered Counter** — `DashParryDetector` vs telegraph layer + cyan-tag contract. Prereq for Big Idea.
5. **Big Idea: Cyan Echo Anchor** — after #4 + demo confirms feel.
6. **#4 Stance Friction (#25)** — lock design now, build with Phase-2 Weapon Forms.
7. Second pass on the recovered-24 roguelite/card ideas (#59/#60/#45/#61) for the META layer.

*Full per-mechanic detail: cx report in `CODEX_DONE_laurethayday.md`; rima-design full text in this session's transcript. Sync this doc to NLM when promoted from STAGING.*
