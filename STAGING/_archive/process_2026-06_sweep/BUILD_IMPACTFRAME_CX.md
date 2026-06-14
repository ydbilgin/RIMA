ACTIVE RULES: (1) think (2) min code (3) surgical (4) BLOCKED if unclear. Verify `dotnet build RIMA.Runtime.csproj --no-restore` (0 errors). UnityMCP UNRELIABLE — don't block. Opus reviews.

# BUILD — Chromatic impact frame (PURE .cs, self-building) — agy/§9 feel add

A brief full-screen "impact frame" flash on HEAVY hits only (combo finisher + boss-death), to sell crunch.
This is a CHEAP approximation of a true chromatic-aberration shader (note that in code as a TODO). C# only, self-building.

## Spec — new `Assets/Scripts/Combat/Juice/ImpactFrameDriver.cs`
- Self-bootstrapping MonoBehaviour (RuntimeInitializeOnLoad), DontDestroyOnLoad. Builds its own top-layer full-screen
  Canvas (very high sortingOrder, `raycastTarget=false`, CanvasGroup) with one full-screen Image.
- Subscribes to `CombatEventBus` hit/kill events. Trigger the flash ONLY on **finisher** (isFinisher) and **boss-death**
  (or kill if no boss flag — match how HitPauseDriver tiers it; do NOT invent a new event). Do NOT flash on every hit.
- Flash = ~2 frames / ~0.04s: frame 1 cyan `#00FFCC` at low alpha (~0.18), frame 2 purple `#7B3FA0` at ~0.12, then
  clear. Use unscaled time (works during hitstop). Keep it SUBTLE (this fires during hitstop — must not blind).
- Expose serialized alphas/duration so it's tunable. Single ImpactFrameDriver only (dup-guard like ScreenShakeDriver).

## CONSTRAINTS
- Must not interfere with input (raycastTarget off). Must not stack/flicker if hits land rapidly (debounce ~0.05s).
- No scene edits, no new asmdef. Add a `.meta` for the new .cs (minimal `fileFormatVersion: 2` + fresh guid) so Unity imports cleanly.

## DELIVER (write to DONE file)
File created (path), how it self-builds + which events trigger it, the flash timing/colors/alphas, `dotnet build
RIMA.Runtime.csproj` result. Confirm no scene edits. List BLOCKED + why. Concise.
