Task 5 Director skin: `Assets/Scripts/UI/DirectorMode.cs` runtime factory only.
Evidence screenshots: `director_task5_spawn_layout_final2.png`, `director_task5_stats_physpower.png`.
Layout: 56px top app bar, 64px icon rail, 320px library, 340px inspector, 30px status bar.
Viewport: approx 1196x994 at 1920x1080 = 57.3%, above 55%.
Color/font: slate IDE dock palette, cyan selection, ember reset/export, LiberationSans TMP SDF, no TMP under 12px.
Global guard: removed EventSystem-wide `IsPointerOverGameObject`; Director spawn/prop guard now checks registered dock rects only.
Runtime assert: Director open, Spawn selected, spawn count 0->1, `physPower=177`, telemetry not persistent, Play/paused visible.
Tests: `RIMA.Tests.DirectorModeValidationTests` EditMode 3/3 passed.
Console: final Unity `read_console` Error+Warning returned 0 entries.
Remaining risk: visual screenshot is manual Game View evidence; no automated pixel/layout assert exists for this shell.
