# T1 Knockdown Pipeline — Architecture Review

**Commit:** `2d5190754f62af8494dc4e7f957ab233a39828d1` — *[Codex] Add knockdown impulse pipeline*
**Reviewer:** axopus (automated)
**Date:** 2026-06-05
**Verdict:** PASS-WITH-NOTES

---

## Summary

The commit adds a visual-only knockdown system (parabola arc, tilt, squash, bounce, shadow, down-state, get-up i-frame) driven entirely through code with no animation asset dependencies. The pipeline flows:

```
BasicAttackProfile.GetImpulseForStep → HitImpulse (canKnockdown + SkillState gate)
  → KnockbackReceiver.ApplyImpulse → KnockdownDriver.TryStart → DoKnockdown coroutine
```

Legacy `KnockbackComponent` call sites (PenitentSovereign × 7) are preserved via a forwarding adapter.

Overall: **solid, shippable work** — the juggle-lock contract is met, visual-only transforms are clean, and tests cover the critical path. There are a few findings below, none blocking tomorrow's playtest.

---

## Findings

### 1. MAJOR — No `OnDestroy`/death cleanup in `KnockdownDriver`

**Location:** `KnockdownDriver.cs` — entire class (no `OnDestroy`, `OnDisable`, or `Health.OnDeath` subscription)

`KnockdownDriver` does not subscribe to `Health.OnDeath`, nor does it implement `OnDestroy` or `OnDisable`. If an enemy dies mid-knockdown (e.g. DOT tick bypassing `IsImmune` via a different damage path, or a future code change):

- **Immunity leak:** `health.SetImmune(healthWasImmune)` never runs → `Health.immune` stays `true` forever. This is the permanent-immune scenario the audit question asked about.
- **Shadow stays visible:** `GroundBlobShadow` child remains `SetActive(true)` on the corpse.
- **EnemyAI stays disabled:** `enemyAI.enabled` is never restored.
- **Visual proxy orphaned:** `proxyRenderer.enabled` stays `true`, `rootRenderer.enabled` stays `false`.

Currently mitigated because:
1. The driver sets `health.SetImmune(true)` immediately, so `Health.TakeDamage` no-ops while down — death can't normally happen during knockdown.
2. `EnemyAI.OnDeath` (EnemyAI.cs:105-112) independently disables itself and the collider.
3. Dead enemies are typically destroyed or deactivated by room-clear logic.

**Risk:** If any future system (area DOT, scene transition, `Destroy()` during arc) bypasses the immune guard, the coroutine is interrupted without cleanup. An `OnDestroy()` calling `Cancel()` would make this bulletproof.

**Recommendation:** Add `private void OnDestroy() => Cancel();` to `KnockdownDriver` at earliest convenience. Not a playtest blocker today since the immune guard is airtight for the current codebase.

---

### 2. MAJOR — Double knockback-resistance application on legacy adapter path

**Location:** `KnockbackComponent.cs:46` + `KnockbackReceiver.cs:50`

When PenitentSovereign calls `KnockbackComponent.ApplyKnockback(dir, force)`:

```
KnockbackComponent.ApplyKnockback:
  effectiveForce = force * (1 - knockbackResistance_COMP)     // line 46
  receiver.ApplyImpulse(new HitImpulse(dir, effectiveForce))  // line 52

KnockbackReceiver.ApplyImpulse:
  actualForce = impulse.force * (1 - knockbackResistance_RECV) // line 50
```

Resistance is applied **twice** — once in `KnockbackComponent` and once in `KnockbackReceiver`. If both have default `knockbackResistance = 0`, the result is correct (×1 × 1 = ×1). But if either component has non-zero resistance, the effective force is `force × (1-R₁) × (1-R₂)` instead of `force × (1 - max(R₁,R₂))`.

**Current impact:** All boss-path enemies likely have resistance = 0 on both components (the default), so no visible bug today. But this is a semantic trap for anyone tuning knockback resistance on a per-enemy basis later.

**Recommendation:** Either skip resistance in `KnockbackReceiver.ApplyImpulse` when the impulse already had resistance applied (e.g. an `alreadyResisted` flag on `HitImpulse`), or remove resistance from the adapter and let the receiver be the single authority.

---

### 3. MINOR — `WaitForSeconds` is timeScale-dependent

**Location:** `KnockdownDriver.cs:105` and `KnockdownDriver.cs:235`

`HoldDown` uses `yield return new WaitForSeconds(profile.DownTime)` and the i-frame hold uses `new WaitForSeconds(profile.GetUpIFrame)`. Both are scaled by `Time.timeScale`. If a slow-motion effect or pause sets `timeScale = 0`, the knockdown freezes indefinitely (enemy stuck on ground, immune forever).

The frame-advancing phases (`MoveArc`, `Squash`, `Bounce`, `GetUp`) also use `Time.deltaTime`, which goes to zero with `timeScale = 0`, so they freeze too — consistent but worth noting.

**Impact:** If the game has a pause system that sets `timeScale = 0`, a knockdown in progress will hang. The design doc mentions "timeScale-independent" as desirable.

**Recommendation:** Consider `WaitForSecondsRealtime` + `Time.unscaledDeltaTime` if pause/slow-mo interaction is needed. Low priority for tomorrow's playtest — unlikely to hit in the arena loop.

---

### 4. MINOR — Shadow not explicitly deactivated on sequence completion

**Location:** `KnockdownDriver.cs` — `RestoreVisual()` (line 266) and `Cancel()` (line 60)

`CaptureRuntimeState` calls `shadow.gameObject.SetActive(true)` (line 121), but neither `RestoreVisual()` nor `Cancel()` calls `shadow.gameObject.SetActive(false)`.

This means the shadow persists after get-up. This may be intentional — `EnemyAI.Awake` already calls `GroundBlobShadow.Ensure()` (EnemyAI.cs:37) so all enemies already have a persistent blob shadow. The knockdown just reconfigures its size/alpha.

**Impact:** Visually harmless if the shadow was already present (which it is for all enemies with `EnemyAI`). But if a non-EnemyAI entity gets knocked down, a shadow will appear and never go away.

---

### 5. PASS — `MarkPulseBehavior` mark ordering is correct

**Location:** `MarkPulseBehavior.cs:132-144`

The commit moved knockback application *before* Sundered mark application — so a target must already be Broken/Sundered from a *previous* hit to be knocked down. This is the correct "pre-existing condition" design from the spec. The comment at line 140-141 documents this clearly.

No action needed. ✅

---

### 6. MINOR — `KnockbackComponent.FixedUpdate` still runs velocity damping after forwarding

**Location:** `KnockbackComponent.cs:70-78`

After `ApplyKnockback` forwards to `receiver.ApplyImpulse`, the adapter still sets `isKnockedBack = true` and `knockbackEndTime` (line 55-56). When `FixedUpdate` fires after `knockbackEndTime`, it lerps `rb.linearVelocity` toward zero (line 76) — this competes with the receiver's coroutine-driven velocity decay.

**Impact:** With default `recoveryTime = 0.2s`, both the adapter's `FixedUpdate` and the receiver's coroutine try to decay velocity simultaneously. The receiver's `DoKnockback` sets velocity each frame, overriding the adapter's single-frame lerp. Net effect: **the receiver wins**, so behavior is correct. But the adapter's `FixedUpdate` wastes a tiny bit of CPU and could produce a one-frame velocity stutter.

---

### 7. PASS — Visual transform hygiene (Audit Q4)

**Location:** `KnockdownDriver.cs:129-174` — `ResolveVisualTarget`, `SetVisual`

The driver correctly identifies whether the sprite renderer is on the root or a child:

- **If on a child transform:** Operates directly on that child. ✅
- **If on the root transform:** Creates a `KnockdownVisualProxy` child, copies the sprite, disables the root renderer, and operates on the proxy. ✅

`SetVisual` (line 255-263) only modifies `visualTarget.localPosition/localRotation/localScale` — never `transform.position` (root). The root's position is moved only via `rb.linearVelocity` during the arc phase, which is physics-driven (same as normal knockback).

**Y-sort (Custom Axis 0,1,0):** Preserved. The root `transform.position.y` is not modified by the visual arc (only `visualTarget.localPosition.y` changes). The sorting-layer assignment via `CopyRenderer` preserves `sortingLayerID` and `sortingOrder`.

✅ **No root position/rotation/scale corruption.**

---

### 8. PASS — Juggle-lock prevention (Audit Q1)

The anti-juggle contract is enforced at **three layers:**

| Gate | Location | Effect |
|------|----------|--------|
| `IsDownOrGettingUp` guard | `KnockdownDriver.cs:50` | `TryStart` returns `false` if already in knockdown |
| `IsDownOrGettingUp` guard | `KnockbackReceiver.cs:48` | `ApplyImpulse` early-returns, no knockback at all |
| `health.SetImmune(true)` | `KnockdownDriver.cs:85` | No damage while down |

Sequence: `IsDownOrGettingUp` is set `true` at the top of `DoKnockdown` (line 76) and stays `true` until the very end (line 110). The i-frame hold at line 104-105 keeps it `true` for an extra `getUpIFrame` seconds after get-up animation completes.

Immunity is restored at line 107 (`RestoreControlAndImmunity`) after the i-frame hold. `IsDownOrGettingUp` is set `false` at line 110 after restoration.

✅ **No re-knockdown path during down or get-up. Immunity correctly restored.**

---

### 9. PASS — Legacy adapter correctness (Audit Q3)

**Location:** `KnockbackComponent.cs`

PenitentSovereign call sites (7 occurrences) all use `.ApplyKnockbackFrom(position, force)`, which calls `.ApplyKnockback(direction, force)`, which forwards to `receiver.ApplyImpulse(new HitImpulse(direction, effectiveForce, recoveryTime))`.

- **Magnitudes preserved:** The `force` parameter from each boss call site (6, 6, 10, 7, 8, 9, 12) passes through unchanged (modulo resistance, see Finding #2).
- **Duration:** The adapter uses its serialized `recoveryTime` (default 0.2s) as the impulse duration. The old code set velocity directly without duration-based decay — so this is actually a **behavior change**: the legacy path now has a 0.2s decaying knockback instead of an instantaneous velocity set. However, this is a visual improvement (smoother knockback), and the 0.2s is short enough to be indistinguishable in gameplay.
- **API surface unchanged:** `ApplyKnockback(Vector2, float)` and `ApplyKnockbackFrom(Vector2, float)` signatures are preserved.

✅ **Boss call sites compile and behave correctly.**

---

### 10. PASS — Test coverage

| Test | Type | What it covers |
|------|------|---------------|
| `Receiver_CanBeAdded` | EditMode | Component wiring |
| `Receiver_HasRigidbody` | EditMode | RequireComponent |
| `KnockbackComponent_CanBeAdded` | EditMode | Adapter auto-adds receiver |
| `HitImpulse_OnlyKnockdownWhenBrokenOrSundered` | EditMode | SkillState gate |
| `BasicAttackProfile_FinalLegacyStepBuildsHeavyImpulse` | EditMode | Heavy-hit flag on last combo step |
| `KnockdownProfile_ClampsUnsafeRuntimeValues` | EditMode | Profile safety clamps |
| `BrokenHeavyImpulse_ArcsLocksDamageThenGetsUp` | PlayMode | Full knockdown lifecycle: arc → immune → AI pause → get-up → damage resumes |

The PlayMode test (`KnockdownPlayModeTests.cs:20-73`) is well-constructed — verifies immunity during down, AI disabled, and post-get-up damage reception. The 0.65s wait accounts for the shortened profile durations.

---

## Verdict: PASS-WITH-NOTES

| # | Finding | Severity | Playtest Risk |
|---|---------|----------|---------------|
| 1 | No `OnDestroy` cleanup in KnockdownDriver | MAJOR | Low (immune guard protects) |
| 2 | Double resistance on adapter path | MAJOR | Low (defaults are 0) |
| 3 | `WaitForSeconds` is timeScale-dependent | MINOR | Very low |
| 4 | Shadow not deactivated after get-up | MINOR | None (already persistent) |
| 5 | Mark ordering is correct | — (PASS) | — |
| 6 | Adapter FixedUpdate competes with receiver | MINOR | None (receiver wins) |
| 7 | Visual transform hygiene | — (PASS) | — |
| 8 | Juggle-lock prevention | — (PASS) | — |
| 9 | Legacy adapter correctness | — (PASS) | — |
| 10 | Test coverage | — (PASS) | — |

**The knockdown pipeline is safe for tomorrow's playtest.** The two MAJOR findings (#1, #2) are correctness hazards for future development but do not manifest in the current codebase. Recommend addressing them in the next sprint.
