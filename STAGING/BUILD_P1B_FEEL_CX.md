ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
UnityMCP read_console is UNRELIABLE right now — do NOT block on it. After edits, do a Roslyn/static syntax check per file and report. Opus verifies compile via Unity Editor.log. Do NOT self-certify; Opus reviews.

# BUILD — PHASE 1 Batch B: camera/hitstop/shake JUICE tuning (PURE .cs, no scene edits)

Locked numbers from `STAGING/DESIGN_LOCK_DEMO_S6.md` §5 (P1). All edits are C#-file only — do NOT touch the scene.
Touch ONLY these files. Keep changes surgical.

## B1 — Hitstop tiers (`Assets/Scripts/Combat/Juice/HitPauseDriver.cs`)
- Tiers via the existing `TriggerPause(float)` path, timeScale 0, inter-cut debounce (ICD) **0.05s**:
  normal **0.04** / crit (= finisher) **0.07** / kill **0.12** / boss-death **0.20**.
- Route the combo FINISHER through the crit tier (so the 3rd combo swing reads heavier). If hit events carry an
  `isCrit`/`isFinisher` flag, map finisher→0.07; kill→0.12; boss-death→0.20 (boss death may be a special publish — if
  no boss flag exists, leave a clearly-marked TODO and use kill tier, do NOT invent a new event).

## B2 — Camera kick OPPOSITE swing (`Assets/Scripts/Combat/Juice/CameraPunchController.cs`)
- In the hit handler, negate direction: `Apply(-e.hitDirection, impulse)`. Impulse magnitudes: hit **0.08** / crit **0.16** / kill **0.22**, decay **6/s**.
- The controller must keep exposing `CurrentOffset` and must NOT write the camera transform (offset pattern).

## B3 — Directional shake + offset conversion (`Assets/Scripts/Combat/Juice/ScreenShakeDriver.cs`)
- ⚠️ cx review flagged: ScreenShakeDriver currently WRITES camera transform localPosition directly — this fights the
  additive-offset contract of `CameraFollow`. CONVERT it to the offset pattern: expose a `CurrentOffset` (like
  CameraPunchController) and stop writing the transform.
- Bias the shake along the hit axis: ~**60% along hitDirection / 40% random** (instead of pure insideUnitCircle).
  Keep magnitudes: hit 0.05/0.10, crit 0.12/0.18.

## B4 — CameraFollow adds BOTH offsets (`Assets/Scripts/Camera/CameraFollow.cs`)
- Verify/ensure `CameraFollow` LateUpdate ADDS `CameraPunchController.Instance.CurrentOffset` AND the new
  `ScreenShakeDriver` offset on top of base follow. If CameraPunch offset is already added, just add the shake one.

## B5 — Dedupe timeScale owners (`Assets/Scripts/Core/HitStop.cs`)
- Mark legacy `Core/HitStop.cs` class `[System.Obsolete("Use HitPauseDriver — single timeScale owner")]` so only
  `HitPauseDriver` mutates `Time.timeScale`. CODE ONLY (do not edit scenes to remove it — note that as pending).

## DELIVER (write to DONE file)
Per item: files changed (file:line), exact values set, Roslyn/static syntax result. List anything BLOCKED + why.
Confirm NO scene file was modified. Keep concise.
