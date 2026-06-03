ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.
Verify with `dotnet build RIMA.Runtime.csproj --no-restore` (0 errors). UnityMCP UNRELIABLE — don't block on it. Opus reviews.

# BUILD — RunStats + Build-Seed (PURE .cs) — makes the Victory/Death CTA REAL

The Victory (`DemoCompleteOverlay`) + Death (`DeathScreenManager`) screens (built last batch) show a run summary +
"Copy Build Seed", but pull stats ad-hoc ("when available"). Make them REAL with a single source of truth. C# only,
self-bootstrapping (no scene wiring).

## G1 — New `Assets/Scripts/Core/RunStats.cs` (singleton, self-bootstrap)
- Tracks for the current run: **kills** (subscribe to the canonical kill signal — prefer `CombatEventBus.OnKill` if it
  exists, else enemy `Health.OnDeath`; do NOT double-count), **run time** (start when the first room loads via
  `RoomLoader`, accumulate unscaled, freeze on death/victory), **rooms cleared / room reached** (from RoomLoader index),
  **active build** = class name + equipped skill names (from the player's skill controller / equipped skills — find the
  canonical source; if none, use class + count).
- Self-bootstrap (RuntimeInitializeOnLoad), DontDestroyOnLoad, resets on run restart. Expose simple getters:
  `Kills`, `RunTimeSeconds`, `RoomReached`, `BuildName`, `BuildSeed`.

## G2 — Build-Seed (shareable string)
- `BuildSeed` = compact encoding of the active build, e.g. `RIMA-WB-<TAG1>-<TAG2>x<count>` style (class code + key
  skill tags). Deterministic + human-readable. Keep it short.

## G3 — Wire into the two CTA screens
- `DemoCompleteOverlay` (victory) + `DeathScreenManager` (death): replace the ad-hoc stat reads with `RunStats`
  getters for the summary (Room / Kills / Time mm:ss / Build name). Make "Copy Build Seed" actually copy
  `RunStats.BuildSeed` to clipboard via `GUIUtility.systemCopyBuffer`.
- Keep both screens' self-building UI intact; do NOT regress Batch D.

## CONSTRAINTS
- If the kill signal or equipped-skill source is ambiguous, list what you found and pick the most canonical one with a
  1-line note (do not invent a new event bus). No scene edits, no new asmdef.

## DELIVER (write to DONE file)
Files created/changed (file:line), the chosen kill/skill sources, the BuildSeed format, `dotnet build RIMA.Runtime.csproj`
result. List BLOCKED + why. Concise.
