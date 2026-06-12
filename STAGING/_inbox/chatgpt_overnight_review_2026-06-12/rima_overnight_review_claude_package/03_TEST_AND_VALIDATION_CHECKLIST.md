# 03 — Test and Validation Checklist

Bu checklist Claude/Codex fix sonrası kullanılacak. Test yoksa minimal EditMode/PlayMode test eklenmeli ya da manual validation açık raporlanmalı.

---

## 1. DamageCalculator unit/edit tests

### Test: Physical armor mitigation
Given:
- packet: baseDamage 100, DamageType.Physical
- attacker physPower 100
- defender armor 100
Expected:
- reduction = 100 / (100 + 100) = 0.5
- finalDamage = 50

### Test: Ability MR mitigation
Given:
- packet: baseDamage 100, DamageType.Ability
- attacker abilityPower 100
- defender magicResist 100
Expected:
- finalDamage = 50

### Test: True damage bypass
Given:
- packet: baseDamage 100, DamageType.True
- defender armor 999, MR 999
Expected:
- finalDamage = 100

### Test: Stat scaling
Given:
- Physical base 100, attacker physPower 150
Expected:
- before defense = 150

Given:
- Ability base 100, attacker abilityPower 150
Expected:
- before defense = 150

### Test: Zero damage
Given:
- baseDamage 0
Expected:
- finalDamage 0

### Test: Nonzero minimum
Given:
- baseDamage 1, very high defense
Expected:
- finalDamage at least 1 unless baseDamage <= 0

---

## 2. Basic attack tests

### Warblade/Melee finisher is not crit
Setup:
- comboLength 3
- comboDamage = 25, 30, 40
- attacker physPower 100
- no defender armor
Expected:
- step 2 final damage = 40
- not 60
- telemetry packet isCrit false

### Heavy SFX/event still works
Expected:
- finisher SFX/event behavior remains.
- Only crit math changes.

---

## 3. Ranger projectile tests

### Ranger DamagePacket path
Setup:
- Ranger ShotCadenceBehavior fires arrow.
- Target Health present.
Expected:
- `SkillRuntime.OnDamageApplied` event fires.
- Damage source is LMB.
- DamageType = profile.lmbDamageType.
- attacker = player.

### Ranger physPower scaling
Setup:
- physPower 100 → record damage.
- physPower 150 → record damage.
Expected:
- damage increases roughly 1.5x before defense.

### Ranger target armor
Setup:
- target has defenderStats armor 100 via provider.
Expected:
- Physical arrow damage halves.

---

## 4. Elementalist projectile regression

### Elementalist still DamagePacket path
Expected:
- projectile still sets DamagePacket.
- ElementTag Fire/Frost/Lightning or chosen tag still propagates.
- OnDamageApplied telemetry fires.

### Status effects still apply
Expected:
- Fire applies Burning.
- Frost applies Chill.
- Third element applies existing intended status.

---

## 5. SkillRuntime stat resolver tests

### Null attacker
Expected:
- Neutral stats.
- No NullReferenceException.

### Player attacker
Expected:
- uses PlayerClassManager.CurrentPrimaryStats.

### Target provider
Expected:
- defender provider armor/MR used.

### No provider target
Expected:
- Neutral defender stats.
- Old no-defense behavior preserved.

---

## 6. Health zero damage

### TakeDamage(0)
Expected:
- CurrentHP unchanged.
- OnDamageTaken not fired unless deliberately designed otherwise.
- OnHealthChanged not fired.
- OnDeath not fired.

### TakeDamage(-5)
Expected:
- no-op.

---

## 7. Director Mode manual playtest

Because all Director commits were visual-unverified, this must be manual.

### Toggle
- Press backtick in play.
- DIRECTOR opens.
- timeScale = 0.
- Free camera works using unscaled dt.

### Start/Test
- Press Başlat / toggle to TEST.
- timeScale = 1.
- Player movement works.
- LMB attack works.
- RMB action works.
- Mouse aim works.
- UI does not consume raycasts.

### Back to Director
- Press backtick again.
- DIRECTOR opens.
- Existing spawned mobs remain.
- This is intentional cumulative scene behavior.

### Spawn
- Select enemy.
- Place enemy.
- Right click erase.
- Confirm spawned list pruning still works.

### Stats
- Change physPower/abilityPower.
- Verify damage changes in telemetry.

---

## 8. Regression suite

Run or report inability:

```bash
# Project-specific command may differ.
# Run CombatContract tests, because overnight gate used this.
```

Expected:
- CombatContract remains green.

Known:
- Full EditMode suite has pre-existing unrelated failures. Do not claim new regression unless diff proves it.

---

## 9. Required report fields

Claude must report:

- Files changed
- Tests run
- Tests not run and why
- Manual validation done/not done
- Remaining design decisions
- Any risky behavior left intentionally unchanged
