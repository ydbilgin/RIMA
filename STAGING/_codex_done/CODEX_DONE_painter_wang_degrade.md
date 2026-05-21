CODEX DONE - Painter Wang Adjacency Graceful Degrade

Scope:
- Modified only `Assets/Editor/RimaUnifiedPainterWindow.cs`.
- No scene or prefab files were saved or intentionally edited.

Fix lines:
- `UpdateWallConnectionsAt` lines 3582-3586: builds an auto-connect-only wall prefab list that excludes prefab names containing `arch`.
- `UpdateWallConnectionsAt` lines 3596-3602: uses the filtered connection list and detects single-face fallback with `isSingleFaceFallback`.
- `UpdateWallConnectionsAt` lines 3604-3612: prevents random crack replacement when the crack slot falls back to the face prefab, and rotates NW/SE single-face fallback by 90 degrees.
- `ApplyWallConnectionFamily` lines 3688-3697: excludes `arch` from the connection family and removes `arch` from damaged/crack fallback.

Compile / validation:
- Unity `read_console` error check: 0 errors.
- Unity `validate_script Assets/Editor/RimaUnifiedPainterWindow.cs`: 0 errors, 3 pre-existing/general warnings about FindObjectOfType/GameObject.Find/string concatenation in Update.

UnityMCP test:
- Loaded `Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity`.
- Opened/instantiated `RimaUnifiedPainterWindow` through editor reflection.
- Target tilemap: `IsoShowcaseRoom_S95_Root/Grid/Floor_Tilemap`.
- Target parent: `IsoShowcaseRoom_S95_Root/Walls_Root`.
- Selected prefab: `Assets/Prefabs/Walls/pilot_a/pilot_a_wall_face_EW.prefab`.
- Auto-connect ON, randomize wall cracks ON, random variants OFF.

Required 3-cell paint:
- Painted cells `(5,5,0)`, `(5,6,0)`, `(5,7,0)`.
- Result before cleanup: 3 wall prefab roots under `Walls_Root/Walls`.
- Rows:
  - `(5,5,0): pilot_a_wall_face_EW, rotZSteps=0`
  - `(5,6,0): pilot_a_wall_face_EW, rotZSteps=0`
  - `(5,7,0): pilot_a_wall_face_EW, rotZSteps=0`
- Arch hits: 0.
- Cleanup by Undo: 0 remaining objects in the test cells.

Degrade-specific rotation check:
- Painted perpendicular row `(5,5,0)`, `(6,5,0)`, `(7,5,0)`.
- Result before cleanup: 3 wall prefab roots under `Walls_Root/Walls`, all `pilot_a_wall_face_EW` with `rotZSteps=1`.
- Arch hits: 0.
- Cleanup by Undo: 0 remaining objects in the test cells.

Notes:
- `ANTIGRAVITY.md` was not present at repo root when requested.
- Worktree had many pre-existing unrelated changes; left untouched.
