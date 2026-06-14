# CX REVIEW — boss Phase-3 "Unleashed" overlay (33% HP modifier layer)

ACTIVE RULES: (1) think (2) min, no speculation (3) surgical — listed file only (4) BLOCKED if unclear.
NLM ACCESS: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>" if needed.
Verdict PASS/FAIL + file:line. Do NOT edit files.

## Scope — `Assets/Scripts/Enemies/Boss/PenitentSovereign.cs` (per STAGING/BOSS_MOB_DESIGN_S6.md §1 Phase 3)
Phase 3 is a modifier LAYER on the existing Phase-2 roster (no new attack coroutines):
- new state: `phase3Active`, `phase3Done`, `p3Rotation = {0,3,0,3,1}`, `p3Idx`.
- BossLoop: after the 50% gate, a one-shot 33% gate (`<= Ceil(MaxHP*0.33)`) sets phase3Active + runs DoPhase3Transition.
- cooldown ×0.8 when phase3Active; Phase2Turn speed ×1.15 + uses p3Rotation when phase3Active.
- Telegraph(): when phase3Active, `duration = Max(0.22f, duration*0.85f)`.
- DoPhaseTransition (50%) + DoPhase3Transition (33%) both add `HitPauseDriver.Instance?.TriggerPause(0.1f)`.
- DoPhase3Transition reassigns `baseColor` toward cyan (body floods) + monolog + AddTrauma(0.9).

## Questions (PASS/FAIL + line)
1. **Ordering/one-shot:** Can the 33% gate fire before the 50% transition completes, or fire twice? It requires
   `phaseTransitionDone && !phase3Done`. Is there a window where both gates trigger in one loop iteration or skip?
2. **baseColor reassign:** DoPhase3Transition does `baseColor = Color.Lerp(baseColor, cyan, 0.4f)`. baseColor is the
   rest color used by Telegraph/DoFlash. Any place that breaks if baseColor permanently shifts (e.g. a later
   comparison to the original, or DoFlash restoring to the new color)? Is it actually a mutable field?
2. **Telegraph floor:** is 0.22s floor applied for ALL phase-3 telegraphs, including attacks with hardcoded windups
   that DON'T route through Telegraph()? List any attack whose windup bypasses Telegraph() (so the floor misses it).
3. **Rotation:** p3Rotation indexing `p3Rotation[p3Idx++ % len]` — correct, no out-of-range? Does dropping Wrath/most
   ChainExplosion match design (bias to Strike/Charge)?
4. **Death routing unaffected:** does Phase 3 touch the death path? `suppressClassSelectOnDeath` must stay true and
   death must still fire RaiseDemoComplete→Victory (NOT class-select). Confirm Phase 3 doesn't regress HandleDeath.
5. **Anything else** (HitPauseDriver null-safety, ScreenShake obsolete usage already present, speed/cooldown sanity).

Be terse. If FAIL, give the exact minimal fix.
