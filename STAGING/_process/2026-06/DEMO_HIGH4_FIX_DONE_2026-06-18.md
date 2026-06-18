# DEMO HIGH-4 FIX BATCH — DONE (2026-06-18)

Spec: `STAGING/_process/2026-06/DEMO_HIGH4_FIX_SPEC_2026-06-18.md`. All 4 fixes applied surgically.
Console after forced recompile + domain reload: **0 errors, 0 warnings**. No Play / no runtime test (per spec).
Unity instance: `RIMA@ed023e0b`.

---

## FIX 1 — MOVEMENT OFF-MAP (ROOT) — DONE

Pattern mirrored from the canonical existing API (read first):
- Velocity movement → `WalkabilityMap.ClampVelocityToWalkable(walkMap, pos, desiredVel, dt)`
  exactly as `PlayerController.FixedUpdate` (line ~334/343) and `KnockbackReceiver.DoKnockback` (line ~93).
- Teleport/destination → `walkMap.IsDashableWorld(worldPos)` exactly as
  `PlayerController.IsReachableDashDestination` / `TryDash` (line ~257, ~400).
- Both helpers are **permissive when no WalkabilityMap exists** (returns desired / true), so behavior
  is unchanged in scenes without a WalkabilityMap (legacy preserved).

### IronCharge.cs (Assets/Scripts/Skills/Warblade/IronCharge.cs), FixedUpdate ~line 52
- WAS: `rb.linearVelocity = chargeDir * chargeSpeed;` (raw, unclamped).
- NOW: velocity passed through `RIMA.Environment.WalkabilityMap.ClampVelocityToWalkable(..., rb.position, chargeDir * chargeSpeed, Time.fixedDeltaTime)` before assignment.
- Why: charge could push the player off-map into void → irrecoverable strand.

### BladeRush.cs (Assets/Scripts/Skills/Warblade/BladeRush.cs), FixedUpdate ~line 39
- WAS: `rb.linearVelocity = chargeDir * chargeSpeed;` (raw).
- NOW: same `ClampVelocityToWalkable` clamp as IronCharge.
- Why: identical off-map risk.

### Blink.cs (Assets/Scripts/Skills/Elementalist/Blink.cs), Execute ~line 38
- The dead "Wall" raycast is **kept** (spec: do not delete, only add walkability guard).
- ADDED after raycast, before applying the teleport: if `WalkabilityMap.Instance != null` and the
  computed `end` is not `IsDashableWorld`, walk back along the blink ray (12 lerp samples from `end`
  toward `start`) to the furthest walkable point; if none is walkable, `end = start` (teleport canceled).
- Why: hard teleport bypassed walkability entirely → blink into void. Snap-back keeps the skill
  responsive while guaranteeing the landing cell is on the map. Mirrors the IsDashableWorld contract
  used by the player dash; permissive when no WalkabilityMap.
- Risk/assumption: damage CircleCast still uses `start`→`end` distance; if `end` snaps back, the
  damage path shortens accordingly (correct — we should not damage along a path the player did not
  travel). No other side effects (empowerment, VFX) changed.

---

## FIX 2 — SkillBase SPEND-BEFORE-VETO (ROOT, base class) — DONE (conservative)

File: Assets/Scripts/Skills/Base/SkillBase.cs

### Contract change (FULL description)
- ADDED: `protected virtual bool CanExecute() => true;`
- ADDED in `TryActivate()`: a single `if (!CanExecute()) return false;` placed AFTER the `IsReady`
  check and component re-resolution, but **BEFORE** any `rage.TrySpend` / `resource.TrySpend` and
  before `Execute()` / `cooldownTimer = cooldown`.
- `Execute()` signature is **unchanged** (still `protected abstract void Execute()`).

### Subclass impact
- **Zero.** No subclass touched. Because the new method is `virtual` with default `true` and is purely
  additive, every existing skill (Warblade / Elementalist / Shadowblade / Ranger / Ronin / Echo, etc.)
  compiles and behaves **exactly as before** — cost/cooldown are spent on the same code path, in the
  same order, whenever `CanExecute()` is true (which it always is by default).
- The veto can only ever PREVENT a spend, never add one. An override returning `true` is identical to
  legacy behavior; only a future override returning `false` (range-gated no-op skill) changes anything.
- Verified: no pre-existing `CanExecute` symbol anywhere in `Assets/Scripts` (grep → 0 hits), so no
  name collision / accidental override.

### NOT done (out of scope, by spec)
- I did **not** wire any specific skill (e.g. Chain Lightning) to override `CanExecute()`. The spec
  scopes this batch to the 4 named files and flags "silent no-op feedback" as a separate MED bug.
  The infrastructure is in place; per-skill range gating is a follow-up that touches subclass files
  not listed in this batch. **This is not a BLOCKED — the base-class root fix is complete and safe;**
  the remaining wiring is intentionally deferred to keep the change surgical.

---

## FIX 3 — RunStats PROGRESSION-DESYNC — DONE

Root cause confirmed: `_Arena`'s `RoomRunDirector` drives clears through its own
`EncounterController.OnRoomCleared` (UnityEvent) and the internal `HandleEncounterCleared()` funnel.
`RunStats` only listens to the static `RoomLoader.OnRoomCleared` event, which the demo path never
fires → `roomsCleared` stays 0 → Echo award floored + death/victory screen stuck on "ODA 1".

### RunStats.cs (Assets/Scripts/Core/RunStats.cs) ~line 174
- ADDED public bridge `NotifyRoomCleared()` that calls the existing private `OnRoomCleared()`
  (which does `StartRunIfNeeded(); roomsCleared++;`). No new state invented — reuses the existing
  increment exactly.

### RoomRunDirector.cs (Assets/Scripts/.../RoomRunDirector.cs) HandleEncounterCleared ~line 1269
- ADDED `RIMA.RunStats.Instance?.NotifyRoomCleared();` right after the existing `RoomCleared?.Invoke()`.
- Guarded upstream by `if (!lifecycle.MarkCleared()) return;` → fires **exactly once per room**.
- Placement matches the existing single clear-funnel (combat clear, boss death, and empty/socket-less
  rooms all route through `HandleEncounterCleared`).
- Note/assumption: Merchant rooms advance the lifecycle directly in `HandleMerchantRoom` (they do NOT
  pass through `HandleEncounterCleared`), so they are intentionally NOT counted as cleared combat
  rooms — this matches their non-combat nature and avoids a scope-creep behavior change. No double
  count: in `_Arena` the static `RoomLoader.OnRoomCleared` is never raised, so the existing
  RunStats subscriber and this new bridge cannot both fire for the same room.

---

## FIX 4 — Boss PHASE-2 BURST-SKIP — DONE

File: Assets/Scripts/Enemies/Boss/PenitentSovereign.cs (BossLoop)

- ADDED fields (~line 117): `private const float Phase2MinDuration = 8f;` and
  `private float phase2EnterTime = float.NegativeInfinity;`.
- On Phase-2 entry (the 50% HP transition where `phaseTransitionDone = true` is set, ~line 228):
  ADDED `phase2EnterTime = Time.time;`.
- Phase-3 ("Unleashed" 33% overlay) trigger (~line 234): ADDED
  `&& Time.time - phase2EnterTime >= Phase2MinDuration` to the existing
  `phaseTransitionDone && !phase3Done && HP <= 33%` condition.
- Effect: even if a single burst drops HP from >50% to <33% in one hit, Phase 3 cannot start until
  8 real seconds of Phase 2 have elapsed; the BossLoop re-checks the 33% threshold every loop until
  then, so Phase 3 still triggers the instant the lock expires. Phase-2 mechanics are guaranteed to
  be shown. No change to the 50% transition or any attack pattern.
- Mapping note: code uses `phase3Done/phase3Active` for the "Unleashed" overlay (spec "Faz-3") and
  `phaseTransitionDone` for the 50% chains-break (spec "Faz-2"). Confirmed against the file's own
  doc header.

---

## VERIFICATION
1. `refresh_unity` (compile=request, mode=force, scope=scripts) → state `compiling`.
2. `editor/state`: `is_compiling=false`, `is_domain_reload_pending=false`, `ready_for_tools=true`,
   domain reload completed after edits.
3. `read_console` (error+warning) → **0 entries**. `read_console` (error only) → **0 entries**.
4. No Play / no runtime test (spec + single-Unity-agent constraint). Runtime verification left to
   orchestrator + auditor.

## FILES TOUCHED (line counts after edit)
- Assets/Scripts/Skills/Warblade/IronCharge.cs (~92 lines)
- Assets/Scripts/Skills/Warblade/BladeRush.cs (~63 lines)
- Assets/Scripts/Skills/Elementalist/Blink.cs (~85 lines)
- Assets/Scripts/Skills/Base/SkillBase.cs (~165 lines)
- Assets/Scripts/Core/RunStats.cs (~239 lines)
- Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs (~2184 lines)
- Assets/Scripts/Enemies/Boss/PenitentSovereign.cs (~+9 lines net)

## BLOCKED
None.
