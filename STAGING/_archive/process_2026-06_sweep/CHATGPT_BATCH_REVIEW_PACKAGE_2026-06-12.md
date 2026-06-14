# ChatGPT Batch Review Package — Interconnected File Clusters (2026-06-12)

> **For:** ChatGPT (web, repo-reading) — review each batch ONE AT A TIME, in order.
> **Repo:** https://github.com/ydbilgin/RIMA (branch `master`)
> **Why batched:** these files form tight functional chains; a bug in one only makes sense when read with its neighbors. Review per-batch, then report per-batch.
> **Already known (do NOT re-report):** the findings in `STAGING/OVERNIGHT_REVIEW_FIX_DECISION_2026-06-12.md` (A1 finisher-crit, A2 defender-stats, A3 projectile bypass incl. 5 SpawnProjectile sites, B1 zero-damage, B2 posture, E1 Health DR min-1, E2 bypassStatScaling+crit). Find what's NEW or what those fixes might miss.

For each batch, ChatGPT should output:
`Batch N — verdict (clean / issues) · new findings (file:line) · cross-file inconsistencies · whether the planned fixes in the DECISION doc fully cover this batch`.

---

## Batch 1 — Damage hot-path (swing → packet → calc → HP)
**Files:**
- `Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs`
- `Assets/Scripts/Balance/DamagePacket.cs`
- `Assets/Scripts/Balance/DamageCalculator.cs`
- `Assets/Scripts/Skills/SkillRuntime.cs`
- `Assets/Scripts/Core/Health.cs`

**Chain:** behavior builds a `DamagePacket` → `SkillRuntime.DealDamage` → `DamageCalculator.Calculate` → `Health.TakeDamage`.
**Review focus:** end-to-end correctness of ONE hit. Order-of-operations in `Calculate` (stat×identity×situational×debug×crit), the TWO independent `Mathf.Max(1,...)` clamps (calculator:58 + Health), `bypassStatScaling` semantics, any double-application of a multiplier. Are E1/E2 the only leaks or are there more?

## Batch 2 — Basic-attack archetypes ↔ projectile pipeline
**Files:** `Assets/Scripts/Combat/BasicAttack/` (all: `IBasicAttackBehavior`, `BasicAttackProfile`, `MeleeChainBehavior`, `CastRhythmBehavior`, `ShotCadenceBehavior`, `HeatGaugeBehavior`, `MarkPulseBehavior`, `VeilStrikeBehavior`) + `Assets/Scripts/Skills/PlayerProjectile.cs` + `SkillRuntime.SpawnProjectile` (`SkillRuntime.cs:210`).
**Chain:** each class's basic attack is a behavior; melee uses `DealDamage(packet)`, projectile archetypes either `SetDamagePacket` (CastRhythm ✅) or call raw `SpawnProjectile` (ShotCadence, HeatGauge×2 ❌).
**Review focus:** do ALL 8 archetypes route damage through `DamagePacket` consistently? Confirm/extend the 5 raw `SpawnProjectile` sites. Does any archetype mis-handle element tag / source type / owner? MarkPulse & VeilStrike & HeatGauge — verify their damage path (under-reviewed overnight).

## Batch 3 — Class stats ↔ player components ↔ HP authority
**Files:**
- `Assets/Scripts/Balance/ClassStatProfile.cs`, `ClassStatRuntime.cs`, `ClassStatDatabase.cs`
- `Assets/Scripts/Systems/PlayerClassManager.cs`
- `Assets/Scripts/Player/PlayerStats.cs`, `PlayerController.cs`, `PlayerAttack.cs`
- `Assets/Scripts/Core/Health.cs`
- 10 class assets under `Assets/Data/` (or `Assets/Resources/`) + `STAGING/_process/2026-06/chatgpt_class_handoff/.../02_B_CLASS_NUMERIC_TABLE.md`
**Chain:** class asset → `ClassStatRuntime` → `PlayerClassManager.ApplyCurrentPrimaryStatsToPlayer` writes maxHP/moveSpeed/atkSpeed to PlayerStats/Controller/Attack; combat reads `Health`.
**Review focus:** (1) **C4 numeric verify** — do the 10 class assets match `02_B_CLASS_NUMERIC_TABLE.md` exactly? Build a Class × (table vs asset) table; say "could not verify" if a file is missing, never assume. (2) **B3 HP authority** — PlayerStats HP vs Health HP divergence; is maxHP change refilling correctly?

## Batch 4 — Director Mode ↔ production-system coupling
**Files:** `Assets/Scripts/UI/DirectorMode.cs` + everything it touches: `PlayerClassManager`, `ClassStatRuntime`, spawn path (`EncounterWaveSO`), telemetry hooks, `RoomRunDirector`.
**Review focus:** every point where the Director (a dev tool) MUTATES shipping state (class, skill slots, runtime stats, timeScale, spawned actors) WITHOUT a restore. State-drift over a long session. The A4 raycast/input-eat in TEST mode (visual-unverified). Is the `DirectorBypassClassUnlock` static reachable in a shipping build (guard)?

---

## Copy-paste prompt for ChatGPT (run once, then say "next batch")
```
RIMA Unity 2D ARPG (repo: github.com/ydbilgin/RIMA, branch master). Bir batch review yapacağız.
Önce oku: STAGING/CHATGPT_BATCH_REVIEW_PACKAGE_2026-06-12.md ve STAGING/OVERNIGHT_REVIEW_FIX_DECISION_2026-06-12.md.
Şimdi sadece BATCH 1'i incele (oradaki dosyaları oku). Çıktı formatı paketteki gibi.
DECISION dosyasında zaten bilinen bulguları TEKRAR etme; YENİ bulgu + cross-file tutarsızlık + planlanan fix'lerin bu batch'i tam kapsayıp kapsamadığı.
Bitince dur, ben "next batch" deyince Batch 2'ye geç.
```
