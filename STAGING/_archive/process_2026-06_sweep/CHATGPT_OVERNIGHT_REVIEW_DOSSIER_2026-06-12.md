# ChatGPT Review Dossier — Overnight Autonomous Run (2026-06-12)

> **For:** ChatGPT (web, repo-reading) comparative review.
> **Repo:** https://github.com/ydbilgin/RIMA  (branch `master`, pushed through commit `bd155294`)
> **Scope:** 20 commits pushed this morning. The 10 newest are the overnight autonomous run (writer = Codex via `cx`, reviewer = council). **All Director commits are `[visual unverified]` — code/compile/tests gated, but on-screen layout was NOT validated. That is the #1 thing to scrutinize.**

---

## What we want from you
A **comparative correctness + design review** of the overnight code, specifically:
1. **Stat/damage core** — is the math sound, are the 10-class numbers internally consistent, any integer-truncation or order-of-operations traps?
2. **Director Mode tabs** — are the runtime hooks safe (no mutation of shipping combat state, no leaks), and is the "cumulative scene" behavior a bug or intended?
3. **Cross-system coupling** — where does the demo tooling reach into production systems, and is any of that reach risky for the actual game build?

Compare against the design canon in the committed decision docs (paths below). Flag where code drifts from the decision docs.

---

## Commit map (newest first)

| Commit | System | What changed |
|---|---|---|
| `c8fd57a0` | Director C6 Telemetry | DPS/TTK tracker + damage-source breakdown + CSV export. Hooks combat events read-only. |
| `9de1f94c` | Director C1 Spawn | Enemy palette click-to-place + ghost preview + right-click delete. Director-only `SpawnSelectedEnemy(pos)`. |
| `5b5abda0` | Director C2 Class&Skill | 10-class swap + skill draft + LMB/RMB assign. `DirectorBypassClassUnlock` static. |
| `d3a3b9d1` | Director C3 Stats | Live `ClassStatRuntime` sliders + Reset/Save/Export. |
| `ddd3a97c` | Director B skeleton | Toggle (backtick key) / camera / timeScale + uGUI Canvas + chrome skin + 6 empty tabs + Jersey10 font. |
| `d8629d45` | Balance fix | `int DealDamage` neutral path — bypasses stat scaling for callers not yet migrated to `DamagePacket`. |
| `169e198e` | **Balance core** | Stat core + damage taxonomy: `DamageType`/`ElementTag`/armor/MR, `DamagePacket`, `DamageCalculator`, `ClassStatProfile`/`Runtime`/`Database` + 10 class assets + wiring + Jersey10 TMP. |
| `e09533b2` | UI assets | 3 map-node symbols (rest/unknown/player) via imagegen + atlas. |
| (earlier) | props/lighting/ui | prop pools, FAZ1 dual-global lighting fix, LMB/RMB display, skill bar/tooltip, reward sorting. |

---

## Key code files to read (overnight)

**Balance core (review math here first):**
- `Assets/Scripts/Balance/DamageType.cs` — enum taxonomy (DamageType + ElementTag)
- `Assets/Scripts/Balance/DamagePacket.cs` — struct carrying a hit's full damage description
- `Assets/Scripts/Balance/DamageCalculator.cs` — **armor/MR mitigation math — main correctness target**
- `Assets/Scripts/Balance/ClassStatProfile.cs` / `ClassStatRuntime.cs` / `ClassStatDatabase.cs` — per-class stat model + 10 assets under `Assets/Data/` (compare to `STAGING/.../02_B_CLASS_NUMERIC_TABLE.md`)
- `Assets/Scripts/Balance/DamageColors.cs` — damage-type → color map

**Wiring (where stat core meets gameplay):**
- `Assets/Scripts/Systems/PlayerClassManager.cs` — maps class → maxHP/moveSpeed/atkSpeed
- `Assets/Scripts/Player/PlayerStats.cs`, `PlayerController.cs`, `PlayerAttack.cs`
- `Assets/Scripts/Combat/BasicAttack/*` — `BasicAttackProfile`, `MeleeChainBehavior`, `CastRhythmBehavior` (2 basic-attack archetypes)
- `Assets/Scripts/Skills/DraftManager.cs`, `SkillRuntime.cs`, `PlayerProjectile.cs`

**Director tooling (where demo reaches into production — review coupling/safety):**
- `Assets/Scripts/UI/DirectorMode.cs` — **the big one**: toggle, camera, timeScale, 6 tabs, all hooks
- `Assets/Scripts/Core/Loc.cs` — bilingual TR/EN strings (note: TR chars ı/ğ/ş approximated to ASCII — known issue)

---

## Known minors already flagged (don't re-report unless they're worse than stated)
- **TR localization** uses ASCII-approximated Turkish (no ı/ğ/ş) — consistent with existing `Loc.cs`, scheduled for a cleanup pass.
- **C1 Spawn:** spawned mobs aren't cleared on Director→Test exit (may be intended "cumulative scene"); enemy palette limited to one hardcoded pilot wave; spawned mobs get default `Health` stats via AddComponent.
- **C2 Class&Skill:** `DirectorBypassClassUnlock` is a public static with no `#if DEMO_BUILD` guard.
- **C3 Stats:** `FindGameObjectWithTag` on every slider change (debug tool, tolerated); Save exists but no Load/Restore.
- **C6 Telemetry:** telemetry buffer has no cap.
- **Test baseline:** full EditMode suite has *pre-existing* unrelated failures (brush/map/prefab/perf/MCP). The gate this run was `CombatContract` only — 3/3 green every phase. Confirm our changes don't touch those failing suites.

## Blocked (need a design call, not a review)
- **C5 Map tab:** no `JumpToNode(node-id)` exists. Available: `DungeonGraph.Generate(seed,depth)` (reroll) + `RoomRunDirector.AdvanceTo(choiceIndex)` (child-choice nav). Question is whether to add an arbitrary-node-jump hook (riskier) or use child-choice nav (safe). See `STAGING/SANDBOX_DIRECTOR_DECISION_2026-06-12.md`.

---

## Design canon to compare against (committed, readable in repo)
- `STAGING/DAMAGE_TYPE_TAXONOMY_DECISION_2026-06-12.md` — the damage model the code should match
- `STAGING/DEMO_TOOLS_REPORT_AND_PLAN_2026-06-12.md` — Director Mode plan (Faz A→D)
- `STAGING/HUD_LAYOUT_DECISION_2026-06-12.md`
- `STAGING/SANDBOX_DIRECTOR_DECISION_2026-06-12.md`
- `STAGING/_process/2026-06/chatgpt_class_handoff/.../02_B_CLASS_NUMERIC_TABLE.md` — the 10-class numeric table the assets must match
- `STAGING/AUTONOMOUS_RUN_2026-06-12.md` — full run log with per-phase gate/review notes
