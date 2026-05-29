# Live Editor T3 — Build Status & Entry Point (S114 S5, Opus autonomous)

Triple-AI (workflow 8-agent + Codex + agy) drove this. Goal: standalone Tool.exe (UI Toolkit runtime) that edits rooms while Game.exe live-reloads. Current state = T2-hybrid; this advances toward full T3.

## What's DONE (safe, in repo)
- ✅ **RuntimeAssetRegistry baked** — `Assets/Resources/Live/RuntimeAssetRegistry.asset`, 67 entries (was MISSING). Live-reload can resolve assets at runtime now.
- ✅ **Cliff live-reload no-op CLOSED** (earlier this session) — tile_guid in serializer/data/reloader + null-safe cliff-tilemap discovery (TilemapNameContains) + 2 smoke tests. EditMode tests: LiveTool PASS.
- ✅ **6 scaffold files authored** → `STAGING/livetool_t3/` (collision-safe, Unity does NOT compile them yet): ToolBootstrap.cs, BrushExecutorRouter.cs, RuntimeCliffHoverIndicator.cs, LiveToolBuildProcessor.cs, ToolMain.uxml, ToolMain.uss.
- ✅ **Adversarial review + integration plan** → `REVIEW_AND_INTEGRATION.md` (THE runbook). Verdict: 1 PASS / 3 CONDITIONAL / 2 FAIL.

## Triple-AI architecture convergence (locked)
- **C6 RuntimeBrushPalette:** REUSE — copy to runtime asm, namespace RIMA.LiveTool, drop `using UnityEditor`, carry `PaletteMode` enum. (~30 min, LOW)
- **C7 RuntimeColliderHandles:** REWRITE view, REUSE math. Runtime API `Initialize/SetTarget/Tick/Undo/CurrentShape`, UI Toolkit absolute handle-dots + LineRenderer outline, edits `RoomLayoutData.collider_overrides` (instance overrides, NOT prefab assets). (~1-1.5 day, HIGH — critical path)
- **C9 RuntimeAssetLoader:** OMIT — `RuntimeAssetRegistry.Instance` already covers it (ToolBootstrap calls it directly).
- **asmdef (CORRECTED — review §3):** `Assets/Scripts/` is ONE all-platform asm `RIMA.Runtime` (NOT Assembly-CSharp). So create ONLY: `RIMA.LiveTool.asmdef` (refs `RIMA.Runtime` + `Unity.InputSystem`, defineConstraints `RIMA_LIVE_TOOL`, all-platform) + optional `RIMA.Build.Editor.asmdef`. **Do NOT create RIMA.Live.asmdef** (breaks existing consumers).
- **Build:** 2 StandaloneWindows64, `RIMA_LIVE_TOOL` define via LiveToolBuildProcessor (Tool ON / Game OFF), ToolMain.unity only in Tool build.
- **Risks:** C7 view-rewrite quality (Tool = quick-iterate, Editor stays canonical authoring + "Open in Editor" shortcut); FileSystemWatcher reliability (lock+debounce+poll — already in JsonFileWatcher); player-stuck-after-reload (WalkabilityMap.NearestWalkable — already in LiveRoomReloader); IL2CPP stripping uncertainty (test at F6).

## ✅ TWINS DONE (Codex bsacqjlwz + Opus sanity-pass) — scaffold now COMPLETE in STAGING
- `RuntimeBrushPalette.cs` (C6 twin: namespace RIMA.LiveTool, dropped UnityEditor, full API, PaletteMode enum carried) — resolves B1.
- `RuntimeColliderHandles.cs` (C7 twin: exact API Initialize/SetTarget/Tick/Undo/CurrentShape, reused math + UITK view + LineRenderer + collider_overrides persistence, zero Editor API) — resolves B3. Opus skim: structurally clean, spec-faithful.
- 2 Codex assumptions flagged: C7 resolves instance_id via prop_instances guid+nearest-position match (SetTarget passes no id); size/shape edits picked up via Tick state-compare. Verify at integration.
- **STAGING/livetool_t3/ now has all 8 files (6 scaffold + 2 twins). Compile-readiness pending Unity integration only.**

## NEXT — Assets/ integration (NOT done autonomously — needs Unity care + user; would red-console-block playtest if done blind before C7 lands)
Follow `REVIEW_AND_INTEGRATION.md §4` (ordered): author/verify C6+C7 twins → create the 2 asmdefs → move 8 STAGING files to TARGET Assets/ paths → enable Editor `RIMA_LIVE_TOOL` define → (bake done) → create `ToolMain.unity` scene (UIDocument+PanelSettings+ortho camera+grid+floor/cliff tilemaps+propRoot, wire ToolBootstrap fields) → dual-build config → Build Tool+Game → smoke (Tool paints → room_current.json+.lock → Game reloads <100ms).

Migration order (Codex+agy): stabilize Editor-hybrid (✅ bake, reloader) → C6/C7 twins → ToolMain+bootstrap (palette milestone) → BrushExecutorRouter+JSON (paint 1 floor → Game reloads) → cliff hover → runtime collider → launcher switch. Editor hybrid stays working throughout (fallback until F7 PASS).
