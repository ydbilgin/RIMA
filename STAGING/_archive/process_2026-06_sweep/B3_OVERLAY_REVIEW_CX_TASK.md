# Review: InPlayMapPaintOverlay.cs (in-play map-paint overlay) — CX

ACTIVE RULES: (1) think before judging (2) report only real issues, no speculation (3) you are the REVIEWER, not the writer — do NOT rewrite the file; list findings as file:line + concrete fix (4) write BLOCKED if you cannot read the file.

NLM ACCESS: not needed (pure code review).

## Context
A new dev-tool MonoBehaviour was written by another AI. In Play mode (Editor or DEVELOPMENT_BUILD), the user presses F2 to toggle a runtime map-paint overlay and paints tiles onto the active room's Tilemaps — no Tool.exe build needed. Entire file is wrapped in `#if UNITY_EDITOR || DEVELOPMENT_BUILD` and auto-bootstraps via `[RuntimeInitializeOnLoadMethod]`. It must NOT affect release builds and must NOT disturb normal gameplay when the overlay is inactive.

## File to review
`Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs`

Reference (do not modify): tilemap discovery convention in `Assets/Scripts/LiveTool/.../LiveRoomReloader.cs`; room event `RIMA.Systems.Map.RoomLoader.OnRoomLoaded` (static `Action<RoomConfig,GameObject>`); tile registry `RIMA.Live.RuntimeAssetRegistry`.

## Review focus — for each, PASS/FAIL + file:line
1. **Mouse→cell math.** Camera.main.ScreenToWorldPoint on an orthographic Pixel Perfect Camera (640x360), then `grid.WorldToCell`. Is z handled so the cell under the cursor is the one painted? Grid cellSize is 0.1667 — confirm no manual scaling is mis-applied.
2. **IMGUI pointer-over-palette guard.** It flips Y between `Mouse.current.position` (bottom-left origin) and the GUI rect (top-left origin) before `Contains`. Verify clicking the palette UI does NOT also paint the map underneath.
3. **Null-safety / never-throw.** room not loaded, no floor/cliff tilemap found, RuntimeAssetRegistry null or empty, Camera.main null, Grid null. Must degrade gracefully.
4. **Event lifecycle.** Subscribes to RoomLoader.OnRoomLoaded (static event). Does it unsubscribe OnDestroy? Static-event leak across play sessions / domain reload?
5. **Release-build strip.** Confirm no UnityEditor API outside `#if UNITY_EDITOR`; the DEVELOPMENT_BUILD half must compile without UnityEditor.
6. **Per-frame cost / GC.** Any per-frame allocations in Update/OnGUI (LINQ, new lists, string concat) that would spam GC while active.
7. **Gameplay non-interference when inactive.** When the overlay is toggled OFF, does it consume input or draw anything?

## Output
Top line: `STATUS: PASS` or `STATUS: FAIL`. Then the findings list. Keep it tight — this is a dev tool, weight findings by real impact.
