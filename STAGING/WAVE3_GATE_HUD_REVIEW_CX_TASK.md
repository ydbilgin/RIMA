# Review: WAVE-3 gate visual (3a) + HUD bar wiring (3b, incl. lazy-load fix) — CX

ACTIVE RULES: (1) think (2) real issues only, file:line + fix (3) reviewer not writer (4) BLOCKED if can't read.
NLM ACCESS: optional.

## 3a — Gate visual
Files: `Assets/Scripts/Systems/Map/RoomLoader.cs` (gate creation ~285-353 + `LoadLargestSprite`), `Assets/Scripts/Environment/Gate.cs` (new `OnUnlocked` event + fired in `Unlock()`).
- Arch sprite (`Resources/Environment/Gate/gate_arch`, Multiple sprite-mode → `LoadLargestSprite`) set on gate root SR BEFORE AddComponent<Gate> (so Awake keeps it); scaled to gateSize, no distortion.
- Sealed barrier child `SealBarrier` (`gate_seal_barrier`, cyan #00FFCC α0.7) in arch opening; hidden via one-shot lambda on `OnUnlocked`, unsubscribes itself.
- Sort: Entities / Pivot / order 0 (overrides Gate.Awake legacy order 5).

Review: PASS/FAIL + file:line
1. Collision/unlock UNCHANGED: BoxCollider size, OnPlayerEntered→LoadNext, MapFragment→Unlock flow intact; `col.offset` recenter doesn't shift the trigger off the gate position. 
2. `OnUnlocked` event: fired once in Unlock (additive, after SetState); the barrier-hide lambda unsubscribes (no leak / no double-fire on re-unlock).
3. `LoadLargestSprite` null-guards (missing → falls back to Gate.Awake placeholder, no throw); picks the main piece for both 1-sub (arch) and 4-sub (barrier) sprites.
4. Sort correct (no manual sortingOrder beyond resetting the legacy 5 to 0 on Entities/Pivot).

## 3b — HUD bars (+ lazy-load fix)
File: `Assets/Scripts/UI/HUDController.cs`.
- bar_frame_9slice (Sliced socket) + bar_fill (Simple) on HP+Rage; HP fill keeps value-driven threshold colors (never cyan), Rage fill cyan.
- **FIX:** Resources.Load moved OUT of static field initializers into lazy `EnsurePackLoaded()` (called from ApplyPackSocket/Fill, which run in BuildHUD/Start context) — the prior field-init caused a runtime "Load not allowed from constructor" exception.

Review: PASS/FAIL + file:line
5. Confirm `EnsurePackLoaded` is only reachable from Start/BuildHUD context (no constructor/field-init path); `_packLoaded` guard prevents repeat loads; null-guard keeps flat fallback.
6. Value/fillAmount/sizeDelta logic untouched; HP never renders cyan; low-HP vignette/boss bar untouched.

## Output
`STATUS: PASS`/`FAIL` + findings. Tight.
