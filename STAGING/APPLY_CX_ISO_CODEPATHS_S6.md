# TASK — Complete the iso fix: kill the square-cellSize + Y-squash that CODE re-forces at runtime

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

Profile: laurethayday. Effort: high. Language: English. **No commit** (writer ≠ reviewer).

## Amaç (Goal)
The scene `_IsoGame.unity` + Map Designer default were already fixed to the iso recipe (cellSize (0.96,0.585), no squash, 451 floor). BUT analysis (ax) + grep found CODE PATHS that still hardcode the REJECTED square cellSize `(0.94,0.94)` and the REJECTED Y-squash `localScale (1, 0.5, 1)`. These OVERRIDE the scene at runtime/rebuild (e.g. `RoomBuilder.GetOrCreateGrid` does `GameObject.Find("IsoGrid")` then overwrites its cellSize/scale). Complete the iso fix by correcting these code defaults so the iso recipe is durable.

**Iso recipe (locked, user-confirmed):** Isometric cellLayout · `cellSize = (0.96f, 0.585f, 1f)` (measured 451 diamond ratio) · root `localScale = (1f, 1f, 1f)` (NO Y-squash — the (1,0.5,1) squash was explicitly REJECTED as artificial).

## Fix these (verified line numbers — confirm before edit):
1. **`Assets/Scripts/Map/RoomBuilder.cs`** lines ~326-329 (`GetOrCreateGrid`):
   - `grid.cellSize = new Vector3(0.94f, 0.94f, 1f);` → `new Vector3(0.96f, 0.585f, 1f);`
   - `go.transform.localScale = new Vector3(1f, 0.5f, 1f);` → `new Vector3(1f, 1f, 1f);`
   - Keep `grid.cellLayout = GridLayout.CellLayout.Isometric;`. Update the `// RIMA cell size (0.94 x 0.94) + scaled Y` comment to reflect the new values.
2. **`Assets/Scripts/Systems/Map/RoomConfig.cs`** line ~30:
   - `public Vector3 cellSize = new Vector3(0.94f, 0.94f, 1f);` → `new Vector3(0.96f, 0.585f, 1f);` (iso default). Check who reads `RoomConfig.cellSize` and applies it to a Grid — if any consumer also squashes, fix consistently.
3. **`Assets/Scripts/Editor/RIMAWallChainBuilderMenu.cs`** line ~421:
   - `grid.cellSize = new Vector3(1f, 0.5f, 1f);` — assess: if this builds an ISO floor grid, change to `(0.96f, 0.585f, 1f)` + ensure no Y-squash. If it is a separate legacy/unrelated tool that is NOT the iso floor path, DO NOT change it — just note it in the report with your reasoning.
4. **Sweep:** grep the whole `Assets/Scripts` (and `Assets/Editor`) for any OTHER hardcoded `new Vector3(0.94f, 0.94f` cellSize or `localScale = new Vector3(1f, 0.5f, 1f)` Y-squash applied to an iso/floor grid. Fix the iso-floor ones to the recipe; list anything you intentionally leave + why.

## Verify
- `dotnet build` the affected csproj(s) → report 0 errors.
- Do NOT claim the iso LOOK is correct (needs Unity). List what to verify visually in Unity (open `_IsoGame`, and if `RoomBuilder.Build`/DungeonSetup is run, confirm the IsoGrid keeps cellSize (0.96,0.585) + scale (1,1,1), 451 floor, seamless tessellation, no squash).

## Deliverable (CODEX_DONE_laurethayday.md, last step)
Exact files/lines/values changed + the sweep results (what else found, fixed-or-left + why) + dotnet build result + Unity visual-verify checklist. End with `STATUS:`. No commit.
