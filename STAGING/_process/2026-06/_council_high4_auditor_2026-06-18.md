# AUDITOR — DEMO HIGH-4 FIX BATCH (2026-06-18)

**Verdict: PASS-WITH-FIXES** (FIX 1/3/4 PASS clean; FIX 2 infra-correct but does NOT close the named symptom — needs per-skill overrides).

Read-only static audit. No code/git/Unity touched. Evidence below is file:line from working tree.

---

## SAFETY SWEEP — CLEAN
- Write-boundary: all 7 edited files are inside permitted targets (RunStats, PenitentSovereign, RoomRunDirector, SkillBase, Blink, BladeRush, IronCharge). No writes outside.
- Secret reads / logging: none touched. Only existing `Debug.Log` calls retained.
- Command-allowlist / injection: none. No process spawn, no eval.
- Forbidden patterns: grepped — no network (Invoke-WebRequest/WebClient/Socket), no Stop-Process, no robocopy, no path traversal in the diff.

---

## Q1 — API CORRECTNESS (FIX 1): PASS
Both signatures are REAL and used BIT-FOR-BIT identically to the canonical callers.

- `WalkabilityMap.ClampVelocityToWalkable(WalkabilityMap walkMap, Vector3 currentPos, Vector2 desiredVelocity, float dt)` — static, `WalkabilityMap.cs:295`.
  - IronCharge.cs:56-57 and BladeRush.cs:42-43 call it with `(WalkabilityMap.Instance, rb.position, chargeDir*chargeSpeed, Time.fixedDeltaTime)` — matches PlayerController.cs:334 (`...transform.position, dashDir*dashSpeed, Time.fixedDeltaTime`) and KnockbackReceiver.cs:93 / KnockdownDriver.cs:201 / BaseMobBehavior.cs:233.
  - Null-permissive (`if (walkMap==null) return desiredVelocity`, WalkabilityMap.cs:297) → legacy behavior preserved when no map. Blocked → returns Vector2.zero (stop at edge), correct, mirrors player dash.
- `IsDashableWorld(Vector3 worldPos)` — instance, `WalkabilityMap.cs:265`.
  - Blink.cs:48,55 call `walkMap.IsDashableWorld(end)` / `(candidate)` — matches PlayerController.cs:383,400 (`IsReachableDashDestination`/dash gate). Ray-walkback (12 lerp samples, end→start) then cancel-to-start fallback is sound; null-permissive guard at Blink.cs:48.
- Movement writes go ONLY through `rb.linearVelocity` (IronCharge.cs:58, BladeRush.cs); Blink writes `rb.position=end` AFTER the snap-back (Blink.cs:78) so the landing cell is validated. No bypass.

REGRESSION: none. The clamp is the same one the player dash already uses every frame, so charge/rush now behave consistently — not a false-positive void-clamp.

---

## Q2 — FIX 2 RULING (CRITICAL): infra-only does NOT fix the symptom. [MAJOR]
`SkillBase.cs:80` adds `protected virtual bool CanExecute() => true;`, gated in TryActivate at SkillBase.cs:91 BEFORE cost/cooldown (correct placement, conservative default). BUT:

- **Zero subclasses override CanExecute()** (grep `CanExecute` over Assets/Scripts → only the 3 hits in SkillBase.cs). So for EVERY skill the veto is a no-op.
- The named symptom is **ChainLightning** (`ChainLightning.cs:33-34`): `var first = FindNearestEnemy(...); if (first==null) return;` — the early-return is INSIDE `Execute()`, which runs only AFTER TryActivate already spent mana (SkillBase.cs:93) and set cooldown (SkillBase.cs:96). The new hook never fires for it → **mana+CD still burned on empty cast. Symptom UNRESOLVED.**

To actually close the bug, the range-gated no-op skills must override `CanExecute()` to repeat their target check. Confirmed range-gated no-op `Execute` patterns (all spend-then-no-op today):
- `Elementalist/ChainLightning.cs:33-34` — `FindNearestEnemy==null → return` (THE named case). Override: `FindNearestEnemy(transform.position,null)!=null`.
- `Warblade/CripplingBlow.cs:32-33` — `FindNearest(range)==null → return`.
- `Warblade/SunderMark.cs:31-32` (and :35 status==null) — `FindNearest(range)==null → return`.
- `Warblade/DeepWound.cs:35` — `target==null → return` (nearest-enemy scan above).
- `Warblade/IronCounter.cs:82` — `target==null → return`.
These five share the "scan enemies in radius, bail if none" shape and are the demo-relevant melee/targeted skills. Self-buffs, dashes (IronCharge/BladeRush/Blink), AoE-at-point, and projectile skills (Fireball/Volley etc.) are NOT no-ops with no target and must NOT override (would wrongly block valid casts).

DEMO-CRITICAL? The user-reported symptom is Chain Lightning specifically → at minimum ChainLightning MUST get the override for the demo, else the reported dead-button persists on camera. The other four are the same class of bug and cheap to add; recommend adding all five. Risk of the override is near-zero (it only repeats the check Execute already does). The base-class infra is correct and ready — the overrides were deferred (builder flagged this in the spec at FIX-2 ⚠️), so this is a known gap, not a hidden one.

---

## Q3 — FIX 3 DOUBLE-COUNT: PASS (no double-count on _Arena path)
- `RunStats.OnRoomCleared()` (RunStats.cs:169-173) does ONLY `roomsCleared++` (+ StartRunIfNeeded). NO reward spawn, NO event, NO side effect. `NotifyRoomCleared()` (RunStats.cs:181-184) is a pure pass-through.
- Call site RoomRunDirector.cs:1274 is inside `HandleEncounterCleared()`, guarded by `if (!lifecycle.MarkCleared()) return;` (RoomRunDirector.cs:1264). `MarkCleared()` is one-way Combat→Cleared (RoomRunDirector.cs:36-45) → idempotent → fires exactly once per room.
- Double-count would require the static `RoomLoader.OnRoomCleared` (which RunStats subscribes to, RunStats.cs:83) ALSO firing on the _Arena path. It is raised only from RoomLoader.cs / RuntimeRoomManager.cs — the _Arena/RoomRunDirector path uses `EncounterController.OnRoomCleared` (instance UnityEvent), NOT the static event. So on _Arena only the bridge increments. CLEAN.
- Residual (NOT demo-blocking): a scene running BOTH RuntimeRoomManager (static event) AND RoomRunDirector would double-count. Not the _Arena demo config; note for post-demo. [LOW]

---

## Q4 — FIX 4 PHASE-LOCK: PASS
- `phase2EnterTime` stamped exactly once at the 50% transition, guarded by `phaseTransitionDone` (PenitentSovereign.cs:234-237).
- Phase-3 trigger now requires `Time.time - phase2EnterTime >= 8f` AND HP<=33% (PenitentSovereign.cs:245-247). It sits in a `while(!dead)` loop re-evaluated every iteration (PenitentSovereign.cs:219), so once 8s elapse the trigger fires on the next loop.
- No softlock: when HP<33% but <8s elapsed, the Phase-3 `if` is skipped and the boss keeps running `Phase2Turn()` (PenitentSovereign.cs:268, phase is Phase2). Boss continues fighting; threshold re-checked. If boss dies during the lock, loop exits via `!dead`. Intent (guarantee Phase 2 shown >=8s) correctly implemented.
- Edge: a boss that drops <33% before 8s stays in Phase 2 until 8s — this IS the intended behavior, not a bug.

---

## Q5 — REGRESSION (SkillBase base-class): PASS
- `CanExecute()` default true + veto placed before cost (SkillBase.cs:91) → all existing skills unaffected; no subclass overrides so behavior is byte-identical to before (only an extra always-true branch). Compile-safe for all ~50 subclasses.
- ExecuteAt (Echo path, SkillBase.cs:113-125) bypasses TryActivate entirely → CanExecute does NOT gate echo casts (correct; echo manages its own cooldown).
- Movement clamp does not restrict legitimate dashes (same helper the player dash uses).

---

## FINDINGS (priority)
- [MAJOR] SkillBase.cs:80 / ChainLightning.cs:33 — FIX 2 infra has zero overrides → named symptom (Chain Lightning empty-cast mana/CD burn) NOT fixed. Add `protected override bool CanExecute()` returning the nearest-enemy check to ChainLightning (demo-critical) and recommended to CripplingBlow/SunderMark/DeepWound/IronCounter.
- [LOW] RunStats.cs:83 vs RoomRunDirector.cs:1274 — double-count only if a scene runs RuntimeRoomManager + RoomRunDirector together (not _Arena). Post-demo note.

Other than the [MAJOR], the batch is correct, surgical, regression-free, and API-faithful.
