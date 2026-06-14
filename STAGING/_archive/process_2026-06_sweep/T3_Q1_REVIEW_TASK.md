# T3 Integration — Q1 REVIEW (post-stage, pre-define-flip)

ACTIVE RULES: (1) think before answering (2) min output, terse (3) REVIEW ONLY — no edits, no code (4) say PASS/FAIL with evidence.

NLM ACCESS: not needed (mechanical integration review). Direct-read: Assets/**, STAGING/livetool_t3/**.

## Context
T3 Live Editor integration, sequential-autonomous. **Q1 just executed:** staged the scaffold INERT before flipping the `RIMA_LIVE_TOOL` define. The runbook is `STAGING/livetool_t3/REVIEW_AND_INTEGRATION.md` (§3 = correct asmdef contents, §4 = ordered steps). Console after Q1 = **0 errors** (define OFF → runtime .cs excluded from compilation).

**What Q1 did:**
- Created `Assets/Scripts/LiveTool/RIMA.LiveTool.asmdef` (refs `RIMA.Runtime`+`Unity.InputSystem`, `defineConstraints:["RIMA_LIVE_TOOL"]`).
- Copied 7 files (NOT read into orchestrator context) to:
  - `Assets/Scripts/LiveTool/ToolBootstrap.cs`
  - `Assets/Scripts/LiveTool/Runtime/BrushExecutorRouter.cs`
  - `Assets/Scripts/LiveTool/Authoring/RuntimeCliffHoverIndicator.cs`
  - `Assets/Scripts/LiveTool/Authoring/RuntimeColliderHandles.cs`
  - `Assets/Scripts/LiveTool/Palette/RuntimeBrushPalette.cs`
  - `Assets/UI/LiveTool/ToolMain.uxml`
  - `Assets/UI/LiveTool/ToolMain.uss`
- **DEFERRED to Q4 (intentional):** `LiveToolBuildProcessor.cs` + `RIMA.Build.Editor.asmdef` (Editor asmdef, no define-gate → would compile immediately; kept out of the zero-risk stage).

## Verify (PASS/FAIL each, with file:line evidence)
1. **asmdef correctness:** does `RIMA.LiveTool.asmdef` match runbook §3 exactly? refs/defineConstraints/platforms. Will it stay INERT while `RIMA_LIVE_TOOL` is undefined (i.e., is Q1 truly zero-risk)?
2. **File placement:** are all 7 files at the exact TARGET paths from each file's `// TARGET:` header + runbook §4.3? Any path drift?
3. **Namespace/define-gate:** do the 5 runtime .cs declare `namespace RIMA.LiveTool` and carry their `#if RIMA_LIVE_TOOL` guard (belt-and-suspenders)? Does `RuntimeBrushPalette.cs` carry the `PaletteMode` enum (B1) and drop `using UnityEditor`? Does `RuntimeColliderHandles.cs` expose the runtime API `Initialize/SetTarget/Tick/Undo/CurrentShape` (B3)?
4. **Q2 readiness prediction:** when the define flips ON, will this assembly compile clean, or do you foresee a specific compile error (missing ref, unresolved symbol, InputSystem)? Name the exact risk if any.
5. **Anything mis-staged or missing** for the next steps (scene Q3 / build Q4)?

Terse. PASS/FAIL + evidence. This gates the define-flip (Q2).
