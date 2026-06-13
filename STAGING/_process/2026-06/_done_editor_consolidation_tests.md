# DONE — Editor Consolidation Regression Test Suite (2026-06-13)

Binding: `STAGING/EDITOR_CONSOLIDATION_DECISION_2026-06-13.md` item 5. User #1 ask: the
F2-conflict / keybind / overlap / placement bug classes must never recur.

## Run result
- EditMode (RIMA.Tests.EditMode / group RIMA.Tests.BuildMode): **25/25 PASS**, 0 fail, 0 skip.
- PlayMode (RIMA.Tests.PlayMode / group RIMA.Tests.BuildMode): **8/8 PASS**, 0 fail, 0 skip.
- Compile: **0 errors** (read_console filter '.cs(' empty after force recompile).
- No impl fixes needed — suite was GREEN on first run against the implemented API.

## Coverage map (decision item 5 -> tests)
- KEY-OWNERSHIP (the bug that started this) -> InPlayToolKeyRegistryTests x5
  (first register OK; 2nd owner FAILS + logs "already owned"; same-owner re-register idempotent;
  Release frees + reclaim; Release by non-owner no-op) + PlayMode F2_IsOwnedBy_BuildModeController.
- LEGACY OVERLAY retired -> PlayMode LegacyOverlay_IsRetired_NotPresent (FindObjectOfType null).
- ENTER/EXIT + no source pollution -> PlayMode Enter_CreatesWorkingCopy_OrNoOpWithoutSource
  (working copy != source instance; null after exit) + Save_WithoutWorkingCopy_ReturnsFalse.
- OVERLAP-HIDE -> PlayMode OverlapHide_DisablesOtherCanvas_RestoresOnExit
  (foreign canvas hidden on enter, restored on exit, pre-disabled stays disabled).
- TOOL EXCLUSIVITY / shared undo -> PlayMode ToolExclusivity_TileHidesPropGhost_PropRestores
  + EditMode BuildCommandStackTests x7 (Execute/Undo/Redo, redo-branch clear, LIFO interleave, Clear).
- F2 TOGGLE -> PlayMode F2Toggle_BuildModeController_IsSoleOwner.
- WALKABILITY/OVERLAY + ISO row-major math -> RoomTemplateGridLogicTests x5
  (y*w+x read-back, out-of-bounds, overlay index, non-zero bounds origin).
- ISO-GRID OVERLAY lifecycle -> PlayMode GridOverlay_ActiveInBuildMode_InactiveOnExit.
- PALETTE brightness (re-darkening guard) -> BuildModePaletteBrightnessTests x5.
- TMP fallback wired (garbled-text guard) -> TmpFallbackWiredTests x3.

## Notes / limitations
- PlayMode tests build a minimal scene IN CODE (Camera + iso Grid/Tilemap + WalkabilityMap) rather
  than loading PlayableArena_Test01 (not in build profile; same reason RoomFlowTests is [Ignore]'d).
  Each PlayMode test Assert.Ignores cleanly if DirectorMode/Camera.main is absent — none ignored on
  this run (all 8 executed and passed).
- SAVE/LOAD asset-roundtrip (edit->Save->reload from .asset) is asserted at the logic+no-op level:
  no-source Save returns false; working copy is a distinct instance from source (no pollution). The
  JSON Y-flip + populated-template roundtrip is already guarded by the pre-existing
  RoomTemplateJsonRoundTripTests (not duplicated).
- Resize/"Expand Bounds" (decision item 7) is out of scope this pass per the task; no test added.

## Test files
- Assets/Tests/EditMode/BuildMode/InPlayToolKeyRegistryTests.cs
- Assets/Tests/EditMode/BuildMode/BuildCommandStackTests.cs
- Assets/Tests/EditMode/BuildMode/RoomTemplateGridLogicTests.cs
- Assets/Tests/EditMode/BuildMode/BuildModePaletteBrightnessTests.cs
- Assets/Tests/EditMode/BuildMode/TmpFallbackWiredTests.cs
- Assets/Tests/PlayMode/BuildMode/BuildModeConsolidationPlayModeTests.cs
