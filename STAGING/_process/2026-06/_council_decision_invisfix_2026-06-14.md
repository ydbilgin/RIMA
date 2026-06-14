# COUNCIL DECISION — P7.5 invisible-sprite fix (2026-06-14)

Advisors: auditor-opus · ax 3.1 Pro · ax 3.5 Flash · cx (yekta). Synthesis: Opus. **DEMO-CRITICAL.**

## Verdicts — UNANIMOUS FAIL on FIX A (4/4)
auditor: FAIL · ax 3.1 Pro: FAIL · ax 3.5 Flash: FAIL · cx: FAIL — all on the SAME finding.

## The bug (P7 surfaced it)
At runtime Elementalist player body + FractureImp/Penitent enemy bodies render NULL (invisible); Warblade fine. Root: animator idle clips reference stale/missing sprite GUIDs → animator writes m_Sprite=null to the SpriteRenderer EVERY frame.

## Finding: FIX A (one-time) is INSUFFICIENT — asset-proven
- Elementalist controller default state = idle_SE/idle_S, WriteDefaultValues=1; idle clip m_PPtrCurves m_Sprite → GUID 927669a7 (NO .meta owns it = missing). Animator samples this every frame → null.
- PlayerAnimator never assigns sr.sprite and re-pokes the animator every Update → the one-time PlayerClassManager fallback is overwritten on frame 2 → Elementalist flashes visible 1 frame then invisible.
- This is the SAME mechanism as the enemy (which correctly got a per-frame LateUpdate keeper in FIX B).

## What's SOUND
- FIX B (EnemyAnimator LateUpdate sprite-keeper): commit-ready. _isDead-guarded (no death-revival), 14 healthy enemies no-op, FractureImp/Penitent prefab static sprites valid pre-Update, LateUpdate runs after the animator write. cx NIT: a future enemy prefab with m_Sprite fileID 0 → null cache → needs a data fix.
- FindBodySprite reads the correct (animator-driven Body) SR. Warblade no regression (its idle GUID resolves → fallback skipped).

## DECISION (P7.5b fix-up)
1. **PlayerAnimator: add a PERSISTENT LateUpdate sprite-keeper** (mirror EnemyAnimator) — cache the class idle sprite (published by PlayerClassManager.ApplyClassIdleSprite), restore when Body sr.sprite==null, guard against the player death state. This is the agreed fix from all 4 advisors.
2. Keep FIX A (PlayerClassManager) as the initial set + fallback source (necessary, not sufficient alone). Dedup FindBodySprite/ApplyClassIdleSprite Body-search loop (auditor MINOR).
3. FIX B unchanged.

## Deferred follow-up (post-demo, documented)
- ⭐ ROOT FIX: repair the Elementalist idle clip sprite GUIDs (927669a7→existing elementalist_idle_*.png) for full 8-way animation. The runtime keeper makes Elementalist VISIBLE (likely static idle pose) meanwhile and no-ops once clips are repaired.
- Re-import archived enemy frames (FractureImp/Penitent in ARCHIVE/Sprites_Enemies_old) for animated enemies (currently static-but-visible via keeper).
- Architecture: ax Pro suggested a single shared SpriteKeeper component for player+enemy instead of two sites — optional unification post-demo.
