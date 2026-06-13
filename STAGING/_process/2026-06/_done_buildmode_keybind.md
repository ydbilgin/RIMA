# DONE: Build Mode keybind layout-independent fix (2026-06-13)

- `Assets/Scripts/UI/BuildModeController.cs` (+25): added `using UnityEngine.InputSystem;`, a `private void Update()` polling `Keyboard.current` (null-guarded) that calls `Toggle()` on **F2 (primary, layout-independent)** OR **quote (US layouts)**; uses `this` members, never the lazy `Instance` getter. Added `[RuntimeInitializeOnLoadMethod(AfterSceneLoad)] Bootstrap()` (mirrors DirectorMode) so the controller exists in play and its Update runs.
- `Assets/Scripts/UI/DirectorMode.cs` (-6): removed the `quoteKey.wasPressedThisFrame -> BuildModeController.Instance.Toggle()` branch from `Update()` (kills the double-toggle). Backquote Director toggle + line ~956 `BuildModeController.IsActive` Phase-2 check untouched.
- New key(s): **F2** (every layout) + **quote** (US). No double-toggle (single owner).
- DEVIATION (flagged): added the `Bootstrap()` RuntimeInitialize hook — required because the old quote branch was the ONLY thing that lazily created the controller; removing it would leave Update never running. Play-mode only, so no edit-mode DontDestroyOnLoad throw. Still within the 2 allowed files.
- Compile: `refresh_unity` force -> editor idle; `read_console` errors=0, warnings=0. Did NOT enter Play Mode.
