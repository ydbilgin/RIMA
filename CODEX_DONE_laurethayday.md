Executed CODEX_TASK_laurethayday.md read-only review.

Output written:
- STAGING/CODEX_DONE_combat_juice_review_s95.md

Verdict:
- FAIL

Key results:
- CombatEventBus PublishHit/PublishKill placement is mechanically valid, including int finalDmg to float HitEvent.damage assignment.
- High risk found: BasicAttackBehaviorBase still triggers legacy HitStop/DamagePopup/CameraShake beside new CombatEventBus subscribers, causing duplicate or racing combat juice.
- High risk found: EnemyAI windup can pause during Chase while the telegraph expires, then later apply damage without a current warning or range/state recheck.
- Medium risk found: PlayerController dash immunity restore preserves only a boolean and can clobber overlapping Health.SetImmune users such as Passive_Unyielding.
- Medium risk found: ApplyMeleeHit assumes BasicAttackProfile melee arrays are non-null and non-empty; Validate exists but is not enforced.

Notes:
- No source code changes were made.
- ANTIGRAVITY.md was not present at the project root when checked.
