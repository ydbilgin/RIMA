## Verdict
FAIL

## Bulgu 1: CombatEventBus hit/kill creates duplicate juice with legacy calls
- Severity: high
- File: F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs:74
- Issue: PublishHit/PublishKill are mechanically valid, but the same method still directly calls legacy HitStop, LightPulse, DamagePopup, and CameraShake at lines 95-100. Current OnHit/OnKill subscribers also run hit pause, damage numbers, screen shake, camera punch, vignette, and VFX. This can double damage numbers, double/compete camera motion, and make HitStop and HitPauseDriver race on Time.timeScale restore.
- Suggested fix: Pick one authority for shared juice. Prefer moving basic-attack hitstop, damage numbers, camera feedback, and kill feedback behind CombatEventBus subscribers, then remove the overlapping direct calls from BasicAttackBehaviorBase. Keep only effects that are intentionally basic-attack-specific.

## Bulgu 2: Enemy windup damage is not gated by range/state at release
- Severity: high
- File: F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Scripts/Enemies/EnemyAI.cs:87
- Issue: Once attackWindupTimer starts, the release path calls player.GetComponent<Health>() and TakeDamage without rechecking state == State.Attack or current distance <= attackRange. If the player leaves attack range or detection range after the telegraph, the hit can still land later.
- Suggested fix: On windup completion, recompute distance and require state/range validity before damage. If invalid, cancel the attack or return to chase/idle without applying damage.

## Bulgu 3: Enemy windup pauses during chase while telegraph expires
- Severity: high
- File: F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Scripts/Enemies/EnemyAI.cs:77
- Issue: The attackWindupTimer branch only runs in the non-Chase else path. If windup starts, then state becomes Chase, the timer stops decrementing while the spawned EnemyTelegraph continues fading independently and is destroyed. The eventual damage can occur later with no active warning, or after a stale warning.
- Suggested fix: Make windup an explicit state that either keeps ticking regardless of movement state or cancels when leaving attack range. Store the spawned EnemyTelegraph if cancellation is needed.

## Bulgu 4: Dash immunity restore can clobber overlapping immunity systems
- Severity: medium
- File: F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Scripts/Player/PlayerController.cs:187
- Issue: dashWasImmune preserves only the boolean value at dash start. If another system sets immunity true during the dash, dash end can restore false and cut it off. If another system's immunity expires during a dash that started while immune, dash end can restore true and extend expired immunity. Passive_Unyielding uses Health.SetImmune(true/false) directly, so this overlap is real.
- Suggested fix: Replace bare SetImmune(bool) coordination with a token/ref-count/source system, or add a Health immunity lease API so dash only removes its own immunity source.

## Bulgu 5: ApplyMeleeHit assumes profile arrays are non-null and non-empty
- Severity: medium
- File: F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs:51
- Issue: profile itself is normally guaranteed by PlayerAttack disabling itself when no BasicAttackProfile exists, and owner.Controller is guaranteed by [RequireComponent(typeof(PlayerController))]. However comboDamage, hitRange, and hitRadius are indexed before any null/empty guard. BasicAttackProfile.Validate exists but is not called in PlayerAttack.SetBasicAttackProfile or Awake.
- Suggested fix: Validate profile before assigning/creating behavior, or use BasicAttackProfile.GetHitRangeForStep/GetHitRadiusForStep plus safe comboDamage fallback. Fail closed with a logged error if melee profile data is invalid.

## Bulgu 6: HitEvent.damage is not necessarily effective HP damage
- Severity: low
- File: F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs:79
- Issue: int finalDmg implicitly casts to HitEvent.damage float, so compile-time integrity is OK. Semantically, Health.TakeDamage applies Health.incomingDamageMultiplier afterward, so the event damage may differ from actual HP removed when that multiplier is not 1.
- Suggested fix: If bus consumers need displayed/effective damage, have Health return effective damage or publish after computing the effective amount. If raw/pre-health damage is intended, rename/document the event field.

## Cift Event Analizi
- CombatEventBus.OnHit subscriber'lari: VFXRouter, ScreenShakeDriver, HitPauseDriver, DamageNumberDriver, CameraPunchController, VignetteFlashController.
- CombatEventBus.OnKill subscriber'lari: VFXRouter, ScreenShakeDriver, HitPauseDriver, CameraPunchController, VignetteFlashController.
- Cift event riski: var.
- Recommendation: CombatEventBus should become the single path for shared combat juice. Remove BasicAttackBehaviorBase direct calls to HitStop, DamagePopup, and CameraShake after equivalent bus subscribers are confirmed. Decide whether LightPulse should stay basic-attack-specific or move behind a bus subscriber/toggle.

## Combat Loop Butunlugu Notu
- P0: Event publish placement is mechanically correct: PublishHit runs after TakeDamage, PublishKill runs after hp.IsDead is updated, and int to float assignment is valid.
- P1: Dash i-frame behavior works for the simple case and is null-safe for missing Health, but it is not robust with overlapping immunity sources.
- P2: Windup and telegraph give the intended warning concept, but the state-machine implementation is not behaviorally safe because warning lifetime, windup timer, movement state, and damage range are not tied together.
- Eksik P0/P1/P2 element: A single-authority juice path, source-scoped immunity, and enemy windup cancellation/range validation are needed before this set should be treated as shippable.
