# COUNCIL DECISION — P7.5c enemy combat visibility (2026-06-14)

Advisors: auditor-opus · ax 3.1 Pro · ax 3.5 Flash · cx (yasinderyabilgin). Synthesis: Opus. DEMO-CRITICAL.

## Verdicts — UNANIMOUS PASS (4/4)
auditor: PASS · ax 3.1 Pro: PASS · ax 3.5 Flash: PASS · cx: PASS-with-nits.

## What it fixes (P7 re-capture surfaced it; reconciled a contradiction)
P7.5's enemy keeper used a `sprite==null` guard, but in REAL combat the enemy body was a RED SQUARE, not null. TRUE root: enemy idle/walk clips drive the body sprite to null each frame, then `BaseMobBehavior.EnsureVisibleSprite` (LateUpdate, order 0) writes a runtime red 48x48 placeholder (a NON-NULL sprite with an EMPTY name) — so `sprite==null` never fired. The two prior agents differed because P7.5 measured the keeper in isolation (null→restore) while P7 measured real combat (red placeholder wins the same-GameObject LateUpdate race). Both correct for their conditions.

## Fix (committed)
- EnemyAnimator: `[DefaultExecutionOrder(100)]` (its LateUpdate runs LAST, after BaseMobBehavior) + guard broadened to `sprite==null || string.IsNullOrEmpty(sprite.name)` → restores the cached authored sprite over the nameless red placeholder. Covers all 3 sprite-driven enemies (FractureImp, HalfThrall, Penitent).
- PlayerAnimator: guard broadened to `sprite==null || sprite.texture==null` (defensive; player already visible).

## Correctness — CONFIRMED (4/4)
- #1 name-guard NO over-fire: all real imported frames carry non-empty names (cx: 1,405 clip refs → non-empty archived names, 0 loadable; 11 prefab fallbacks all named); only the runtime red placeholder is nameless. Healthy 14 enemies + Warblade untouched.
- #2 DefaultExecutionOrder(100) harmless: EnemyAnimator is a pure consumer (reads state/velocity, writes anim params + flipX before Animator sampling); running later = fresher state, no regression.
- #3 No 1-frame red flash (same-frame: order 0 red → order 100 real, before render). #4 player guard inert for Warblade. #5 _isDead + 14 no-op + death-hide preserved; decal SRs (MobDeathResidue/EnemyTelegraph) don't race the body.
- Verified in real combat: enemy + player body sprite+texture+enabled 40/40 frames incl. movement/aggro; screenshot shows real art, no red square.

## Deferred (post-demo debt — all advisors agree)
- ⭐ ROOT FIX: re-import / re-point the broken enemy + Elementalist clip sprite refs (frames archived out of Assets) → full animation; BOTH keepers then go inert.
- BaseMobBehavior: cache/restore the authored sprite instead of creating a red placeholder (single source of truth), so the order-100 dependency disappears.
- Split keeper to a LateUpdate-only component (or StateMachineBehaviour) instead of class-wide DefaultExecutionOrder.
- Unify the guard (player texture-based vs enemy name-based) via a shared Sprite.IsValid() helper.
