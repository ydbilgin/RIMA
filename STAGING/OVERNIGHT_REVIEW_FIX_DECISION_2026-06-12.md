# Overnight Review Fix — DECISION (2026-06-12)

> Source: ChatGPT comparative review (`STAGING/_inbox/chatgpt_overnight_review_2026-06-12/`).
> Verified by Claude against live code. Risk-audited by council (cx + ax Gemini 3.1 Pro + ax Gemini 3.5 Flash).
> **Status: ChatGPT's 5 findings all CONFIRMED real. + 2 extra found by 3.1 Pro.**

## Verification (Claude, file:line)
| ID | Finding | Status |
|---|---|---|
| A1 | `BasicAttackBehaviorBase.cs:69-77` finisher → `DamagePacket.Create` 7th arg = `isCrit`; `DamageCalculator.cs:46` `critMult = isCrit?1.5:1` | ✅ real (~50% finisher inflation) |
| A2 | `SkillRuntime.cs:138-141` `Calculate(packet, attackerStats)` — defenderStats omitted | ✅ real (armor/MR dead) |
| A3 | `ShotCadenceBehavior.cs:53` Ranger `SpawnProjectile(raw dmg)` no `SetDamagePacket`; Elementalist `CastRhythmBehavior.cs:81` does set it | ✅ real (Ranger bypasses pipeline) |
| B1 | `DamageCalculator.cs:58` `Mathf.Max(1,...)` → 0 base deals 1 | ✅ real |
| B2 | `DamageCalculator.cs:60-64` postureOverflowDamage computed, no consumer | ✅ real (dead) |

## Council resolution

### Key disagreement — A2 architecture
- **3.1 Pro:** `ICombatStatsProvider` interface is correct, not over-engineering.
- **Flash:** interface is over-engineering for demo; interface-free static helper suffices.
- **DECISION (Opus):** **Flash wins for the demo timebox.** Use interface-free `static ResolveCombatStats(GameObject)` in `SkillRuntime` (Player→`PlayerClassManager.CurrentPrimaryStats`, else `Neutral`). Delivers the immediate win (player-as-defender gets real armor/MR); enemies→Neutral until they get stats. Leave `// TODO: ICombatStatsProvider when enemies carry stats` (3.1 Pro's shape = the future, not now).

### Test-breakage (Claude found, = cx's lens)
- `HealthTests.cs:62 TakeDamage_ZeroDamage_StillDeals1` asserts 0→deals 1 (codifies the bug). **B1 fix breaks it → must flip the assert to no-op (CurrentHP unchanged).**
- `CombatContractTests.cs` has NO crit/finisher/armor deps → A1/A2/A3 safe.

## Final fix plan (apply order: safe → risky)

| # | Fix | File | ~Lines | Commit safety |
|---|---|---|---|---|
| 1 | **A1** finisher ≠ crit | `BasicAttackBehaviorBase.cs` — `isCrit:false` + `sourceId:"basic_lmb_finisher"` | 1-2 | ✅ blind (telemetry C6 benefits from sourceId; comboDamage already carries finisher strength → no rebalance) |
| 2 | **A3** Ranger packet | `ShotCadenceBehavior.cs` — add `projectile.SetDamagePacket(...)` after spawn | ~7 | ✅ blind |
| 3 | **A2** defender stats (lean) | `SkillRuntime.cs` — static `ResolveCombatStats` + pass defenderStats to `Calculate` | ~10 | ✅ blind |
| 4 | **B1** zero-damage no-op | `Health.cs` — `if (amount <= 0) return;` **+ flip `HealthTests.cs:66`** | 1 + test | ⚠️ test edit mandatory |
| 5 | **B3** HP authority | `PlayerClassManager.cs` — decision NOTE + optional tiny `Health.SetMaxHP` sync | note | defer-ish |
| 6 | **A4** Director raycast | `DirectorMode.cs` — `rootGroup` alpha/interactable/blocksRaycasts by state | small | ❌ **Play-mode verify ONLY — no blind commit** |
| — | **B2** posture overflow | `DamageCalculator.cs` — TODO comment only | 1 | defer |

Core combat diff ≈ 20-25 lines (Flash estimate).

## Extra findings (3.1 Pro — ChatGPT missed)
- **E1 — Health DR min-1 leak:** `Health.cs` `effective = Mathf.Max(1, amount * incomingDamageMultiplier)` → a hit reduced to ~0 by 100% damage-reduction still deals 1. Separate clamp from B1. **Flag**; fix optional (no class has 100% DR yet — revisit when DR buffs exist).
- **E2 — bypassStatScaling still applies crit + identity/situational caps:** traps/environment dmg could be amplified by player passives. Low risk now (traps don't set isCrit). **TODO/flag.**

## cx feasibility additions (folded in)
- **A1 is backed by an existing test:** `DifficultyBalanceTests.cs:39-44` assumes Warblade combo total = 25+30+40 = **95, no crit**. Current runtime is INFLATED vs this. → A1 fix *aligns* runtime to a documented test. Even safer; no rebalance.
- **A3 scope is 5 call-sites, not 1:** `SkillRuntime.SpawnProjectile` (`SkillRuntime.cs:210`) is called raw (no packet) by: ShotCadence/Ranger (`:53`), **HeatGauge dual (`HeatGaugeBehavior.cs:89`) + hip (`:130`)**, ShadowPin (`:21`), PinningShot (`:21`). Ranger fix is correct for *this* scope, but HeatGauge is another class's **basic attack** also bypassing the pipeline. → **DECISION:** fix Ranger (A3) + **HeatGauge's 2 sites** now (both are basic-attack archetypes, the stated goal); ShadowPin/PinningShot are skills → separate follow-up. Prefer local `SetDamagePacket` per call-site over a `SpawnProjectile` overload (overload touches a shared API with 5 callers + legacy-vs-new decision).
- **bypassStatScaling overload caveat:** `SkillRuntime.DealDamage(int damage,...)` (`:107-120`) builds packet with `bypassStatScaling:true`; any skill using that overload bypasses attacker scaling even after A2. Flag for later audit (link E2).
- **cx order:** B2(doc) → A1 → A3 → A2 → B1(last). Matches our safe→risky, B1 last due to test conflict.

## Open
- **C4 numeric table:** verify 10-class assets vs `02_B_CLASS_NUMERIC_TABLE.md` (separate grep pass — do NOT claim "match" unverified).
- **B3 production behavior:** does maxHP increase mid-run refill? → no (3.1 Pro); needs `SetMaxHP(int, bool refill=true)` overload when implemented.

## Recommended execution
Single cx dispatch task for fixes 1-4 (+ B2 TODO + E1/E2 flags), CombatContract gate, then **A4 + visual playtest in the Play-mode morning session**. C4 numeric verify as a quick grep sub-task.
