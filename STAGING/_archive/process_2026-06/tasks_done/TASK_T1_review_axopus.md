# Review task — Knockdown package architecture audit (commit 2d519075)

You are reviewing another agent's committed work. READ-ONLY — do not modify any file. Unity project root = "F:/Antigravity Projeler/2d roguelite/RIMA".

Scope: commit `2d5190754f62af8494dc4e7f957ab233a39828d1` ("[Codex] Add knockdown impulse pipeline"). Inspect with `git show 2d519075` and by reading the files below.

Files: Assets/Scripts/Core/{HitImpulse,KnockdownProfile,KnockdownDriver,KnockbackReceiver,KnockbackComponent}.cs · Assets/Scripts/Combat/BasicAttack/{BasicAttackProfile,BasicAttackBehaviorBase,MarkPulseBehavior}.cs · Assets/Tests/{EditMode/KnockbackTests,PlayMode/KnockdownPlayModeTests}.cs

Design contract it must satisfy (from STAGING/CODEANIM_DECISION_2026-06-05.md + STAGING/QUEUE10_ROUTING_DECISION_2026-06-05.md):
- Heavy hit + target Broken/Sundered (SkillStateTracker) → knockdown; parabola arc + ~35° tilt + Y-squash + single bounce + GroundBlobShadow + down-state invuln + get-up i-frame.
- JUGGLE-LOCK MUST BE IMPOSSIBLE: no re-knockdown while down; invulnerable while down; AI pauses and resumes; damage resumes after get-up.
- Legacy `KnockbackComponent` = compat-forwarding adapter (not invasive rewrite); PenitentSovereign call sites unchanged.
- Visual-only on a child transform (no physics position corruption); code-only, no anim assets.

Audit questions:
1. Juggle-lock & invuln: any path where a second impulse during down/get-up re-triggers knockdown or leaves Health permanently immune (exception mid-coroutine, object destroyed mid-knockdown, scene change)?
2. Death-during-knockdown: if Health dies while down, does the driver clean up (shadow, tilt, AI state, timeScale-independent)?
3. Adapter correctness: does KnockbackComponent forwarding preserve old boss behavior magnitudes/durations?
4. Visual transform hygiene: does the driver mutate root position/rotation/scale (breaks colliders/Y-sort) or only a visual child? Y-sort (Custom-Axis (0,1,0)) preserved?
5. Anything that would visibly break the playable loop (_Arena run) for tomorrow's user playtest?

Output: write `STAGING/_review_T1_knockdown_axopus.md` with verdict PASS / PASS-WITH-NOTES / FAIL + numbered findings with file:line evidence. Severity-tag each finding (BLOCKER/MAJOR/MINOR).
