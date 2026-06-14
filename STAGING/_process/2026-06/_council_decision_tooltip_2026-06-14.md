# COUNCIL DECISION — P2 tooltip fix (2026-06-14)

Advisors: auditor-opus · ax 3.1 Pro · ax 3.5 Flash · cx (yekta). Synthesis: Opus.

## Verdicts
- auditor-opus: PASS · ax 3.1 Pro: PASS(+HIGH) · ax 3.5 Flash: PASS · cx: **FAIL**(+HIGH+MEDIUM)

## Agreements
- FIX B (TooltipSystem EnsureBuilt lazy+idempotent): ALL → SAFE, strong defensive code (built+panel guard survives scene/canvas teardown). Keep.
- Relaxed guard (description→skillName): ALL → safe; empty slots still null → no tip. UX improvement.

## Key finding (cx FAIL + ax Pro HIGH) — REAL BUG
Basic-attack tooltip damage is correct ONLY for melee/combo classes (Warblade). For ranged/non-melee:
- **LMB:** uses `comboDamage[0]` but CastRhythm/ShotCadence/VeilStrike/HeatGauge actually use `projectileDamage`. Currently incidental-correct because assets happen to match.
- **RMB:** always prints `rmbDamage`, but Ranger RMB fires ExecuteArrow→`projectileDamage` with asset `rmbDamage=0` → shows "Hasar: 0" (wrong).
auditor missed it (only tested Warblade=melee).

## DECISION (P2b fix-up)
1. **[HIGH] behaviorType-aware damage selection** for both LMB + RMB: mirror what each BasicAttack behavior class actually uses (combo types → comboDamage[0]; projectile types ShotCadence/CastRhythm/etc → projectileDamage; RMB likewise). READ the behavior classes to get the mapping right.
2. **[MINOR fold]** FormatSkill: guard the unconditional `description` append (name-only skills — now tippable via FIX A — must not show a blank line).

## Deferred (noted, not done — RIMA "prototipte modülerleştirme yok")
- Encapsulation refactor: move BuildBasicAttackTooltip into BasicAttackProfile.GetTooltipText(isLmb) (ax Pro LOW, architectural debt).
- TooltipSystem.Awake Destroy(gameObject)→Destroy(this) (ax Flash NIT; not triggered in current dynamic-GO flow).
- Explicit per-profile tooltip damage fields (cx alternative; asset-wide change).
