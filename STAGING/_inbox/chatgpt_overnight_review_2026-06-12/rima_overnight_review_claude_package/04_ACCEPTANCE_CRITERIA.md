# 04 — Acceptance Criteria

Bu fix pass “done” sayılması için aşağıdakiler sağlanmalı.

## Critical acceptance

### 1. Finisher no longer equals crit
- Melee combo finisher does not set `DamagePacket.isCrit = true`.
- Finisher damage no longer gets implicit 1.5x crit multiplier.
- Heavy/finisher SFX behavior remains.

### 2. Ranger projectile enters DamagePacket path
- Ranger/ShotCadence projectile calls `SetDamagePacket`.
- Ranger hits trigger `SkillRuntime.OnDamageApplied`.
- Ranger damage responds to physPower/armor.

### 3. Defender stats applied where available
- `SkillRuntime.DealDamage(DamagePacket...)` calls `DamageCalculator.Calculate(packet, attackerStats, defenderStats)`.
- Defender stats resolved from target/provider/fallback.
- Physical armor and Ability MR are both testable.
- True damage bypass preserved.

### 4. Zero damage is no-op
- `baseDamage <= 0` does not become 1 damage.
- `Health.TakeDamage(0)` or SkillRuntime finalDamage 0 path does not reduce HP.

### 5. Director TEST mode does not block gameplay input
- In TEST mode, Director overlay must not eat raycasts.
- LMB/RMB and mouse aim work.
- Cumulative scene behavior is preserved.

## Medium acceptance

### 6. HP authority risk documented or minimally bridged
One of:
- PlayerStats/Health sync implemented safely, or
- TODO/decision note added with exact recommended direction.

### 7. Stat numeric table verification attempted
- Claude searches for `02_B_CLASS_NUMERIC_TABLE.md`.
- If found, compares 10 class assets.
- If not found, reports inability honestly.

## Tests acceptance

At least:
- CombatContract run result reported.
- DamageCalculator tests added or existing tests run.
- Ranger projectile path verified by test/manual.
- Director TEST mode raycast checked manually or reported as not visually verified.

## Non-goals

Do not require:
- Full C4 Build implementation.
- C5 Map jump implementation.
- Full enemy stat system.
- Elemental resist matrix.
- UI polish.
- Localization cleanup.
- Telemetry cap.
