# T3 Integration — Q2 REVIEW + Q3 wiring-spec extraction

ACTIVE RULES: (1) think before answering (2) terse (3) REVIEW + SPEC EXTRACTION ONLY — no edits, no code (4) PASS/FAIL + evidence.

NLM ACCESS: not needed. Direct-read: Assets/**, STAGING/livetool_t3/REVIEW_AND_INTEGRATION.md.

## Context — Q2 just executed
Flipped `RIMA_LIVE_TOOL` Standalone scripting define ON. Orchestrator hard-verified via reflection: `RIMA.LiveTool` assembly LOADED (15 types), all 5 runtime types resolve (ToolBootstrap/RuntimeBrushPalette/RuntimeColliderHandles/BrushExecutorRouter/RuntimeCliffHoverIndicator), `isCompiling=False`, console **0 errors / 0 warnings**. Files live under `Assets/Scripts/LiveTool/` + `Assets/UI/LiveTool/`.

## Part A — Q2 regression review (PASS/FAIL + evidence)
1. **Regression:** does flipping `RIMA_LIVE_TOOL` (Standalone) risk anything elsewhere? Any other assembly/`#if RIMA_LIVE_TOOL` site in the project that now activates unexpectedly? (grep the repo for `RIMA_LIVE_TOOL`.)
2. **Define scope:** is Standalone-only correct, or do other named build targets need it for the Editor/in-editor play to see the Tool code? (Editor uses the active build target's defines — confirm in-Editor compilation is covered.)
3. **Any concern** the 0-error compile masks (e.g., a type that compiles but will NRE at runtime due to the B6/B8/B9 known issues)?

## Part B — Q3 scene wiring SPEC (this is the real deliverable — extract exactly, so orchestrator builds the scene without reading the 600-line bootstrap)
From `Assets/Scripts/LiveTool/ToolBootstrap.cs` (+ RuntimeCliffHoverIndicator/BrushExecutorRouter as needed), extract:
1. **Every `[SerializeField]` / public serialized field** on ToolBootstrap that must be wired in the scene — exact field name + type (e.g., `uiDocument:UIDocument`, `previewCamera:Camera`, `grid:Grid`, `floorTilemap:Tilemap`, `cliffTilemap:Tilemap`, `propRoot:Transform`, + any others). Give the COMPLETE list, nothing missed.
2. **Tilemap name-substring requirement:** confirm what substrings `LiveRoomReloader`/discovery expects ("floor"/"cliff"?) and whether ToolBootstrap discovers by name or by the serialized refs.
3. **UIDocument setup:** does ToolBootstrap need a `PanelSettings` asset assigned, or just the UXML source? Does the UXML `<Style src>` self-attach the USS (so no separate stylesheet wiring)?
4. **Camera params:** orthographic, PPU 64 — any camera-z assumption (the B6 note) the scene must honor? Default z = -10?
5. **Grid params:** cellSize (1,1,1) Rectangular?
6. **Any Awake/Start auto-bootstrap** that will run on play, and any registry precondition (RuntimeAssetRegistry baked — confirm it exists at `Assets/Resources/Live/RuntimeAssetRegistry.asset`).
7. **Anything else** the scene MUST have or the Tool silently no-ops.

Terse, structured. Part B is a checklist orchestrator will follow literally for Q3.
