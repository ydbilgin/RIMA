ACTIVE RULES: (1) think before reviewing (2) be specific — file:line evidence (3) findings only, do NOT edit code (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# TASK: CODE REVIEW (findings-only) — Unified Designer consolidation (Opus-written, ultracode)

**REVIEW ONLY. Do NOT edit code/scenes. Output a structured report appended to CODEX_DONE.md.**

## What was built (review these NEW/CHANGED files)
A unified room/level designer: ONE Editor front door + the in-game F2 overlay share a
surface-agnostic core + the same RoomData. Architecture lock: `STAGING/UNIFIED_DESIGNER_ARCHITECTURE_LOCK_S6.md`.

NEW (RIMA.Runtime, runtime-safe — no UnityEditor):
- `Assets/Scripts/RoomPainter/DesignerCategory.cs` (enum Floor/Cliff/Object/Portal/Light)
- `Assets/Scripts/RoomPainter/DesignerCategoryMap.cs` (category -> RoomLayer/tag routing)
- `Assets/Scripts/RoomPainter/RoomDepthStack.cs` (single source of truth for sorting layer+order per RoomLayer; the user's L1 floor / L2 cliff / preview / L3 backdrop stack)
- `Assets/Scripts/RoomPainter/RoomCliffSolver.cs` (pure cliff solver from floor cells; ported from CliffAutoPlacer.CollectCliffCells)
- `Assets/Scripts/RoomPainter/UnifiedDesignerCore.cs` (shared state + Paint/Erase/GenerateCliffsFromFloor/SaveJson; BeforeMutate hook for Undo)

NEW (Editor):
- `Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs` (REWRITTEN — tabbed window: Library/Floor/Cliff/Object/Portal/Light/Layers; drives the core; SceneView paint)

CHANGED:
- `Assets/Scripts/RoomPainter/RoomData.cs` (+portalPlacements list + PortalPlacement struct)
- `Assets/Scripts/RoomPainter/RoomDataJson.cs` (DTO + ToDto/ApplyTo carry portalPlacements)
- `Assets/Scripts/RoomPainter/RoomDataMutator.cs` (+PutPortal/RemovePortal + PutCategory/RemoveCategory)
- `Assets/Scripts/Editor/MapDesigner/CliffGenerateAction.cs` (REWRITTEN — repairs existing unready placer, resolves floor tilemap by name, RoomDepthStack sorting, readiness reason UI)
- `Assets/Editor/RoomPainter/Authoring/RoomDataAuthoringController.cs` + `RoomDataComposer.cs` (internal -> public for cross-asmdef use)
- `Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs` (+GenerateCliffsInPlay button; replaced dead ComposeRuinedKeep)
- 3 legacy windows demoted to `RIMA/Legacy/` menu (MapDesignerBrushWindow, RimaVisualMapEditorWindow, RimaRoomPainterWindow)
- `Assets/Tests/EditMode/RoomPainter/UnifiedDesignerTests.cs` (NEW — 9 tests, ALL 363 EditMode tests pass)

## Review focus
1. **Correctness:** RoomCliffSolver vs the original CliffAutoPlacer.CollectCliffCells — is the port faithful (exterior-void flood, monotonic-south, back-side cut, protrusion cut)? Any off-by-one / neighbour-vector error?
2. **Shared-data integrity:** Does PutCategory route every category to the right collection? Does the JSON round-trip now preserve portalPlacements (DTO sync)? Any list that EnsureDefaults forgets?
3. **Cliff fix:** Does CliffGenerateAction now actually repair an existing-but-unready placer (the documented root cause)? Any case where it still no-ops silently?
4. **Depth stack:** Are RoomDepthStack's sorting layers all present in TagManager (Default/Ground/Floor/Decals/Walls/Entities/VFX/Player/BackwallLandmark/Characters/Props/Decor_Cliff/Decor_Floor)? Is "BackwallLandmark" the right pick for backdrop, or should it be a lower layer?
5. **Runtime safety:** Are the RIMA.Runtime new files truly free of UnityEditor refs? (UnifiedDesignerCore, RoomCliffSolver, DesignerCategory*, RoomDepthStack.)
6. **F2 parity:** GenerateCliffsInPlay — correct use of _activeRoomData/_cliffTilemap/_palette? Any null-deref?
7. **Anything missed** for the user's goals: dual-surface parity, categories, shiftable layers, clean rooms, organized assets.

## Output
Append to CODEX_DONE.md under "## CX REVIEW — Unified Designer". Each finding: file:line + severity (BLOCKING/SHOULD/NIT) + concrete fix description. End with "BOTTOM LINE: ship / fix-first + the 1-3 must-fixes". NO code edits.
