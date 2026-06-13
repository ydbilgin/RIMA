# Build Mode Phase 3.1 — audit fix DONE (2026-06-13)

Files (3 only):
- `Assets/Scripts/UI/BuildModeController.cs` (+~75 lines): owns `WorkingTemplate` (Object.Instantiate of RoomRunDirector.CurrentTemplate, hideFlags=DontSave); created in EnterBuildMode before tools start, destroyed in ExitBuildMode + OnDestroy. Public `WorkingTemplate` + static `ActiveWorkingTemplate` accessors.
- `Assets/Scripts/UI/BuildMode/BuildTileBrushController.cs` (~+60/-45): brush binds to `BuildModeController.ActiveWorkingTemplate` (no self-clone); removed sourceTemplate field + working-copy destroy from teardown. MINOR1 hold-to-paint (one op per NEW cell while LMB/RMB held; painting flag reset on SetActive(false)). MINOR2 multi-cell cursor = one pooled quad per FootprintCells entry (iso diamond), radius 1 = single quad.
- `Assets/Scripts/UI/BuildMode/BuildPlacementController.cs` (~+20/-12): ApplyPlace/RevertPlace/ValidatePlacement now target `WorkingTemplate()`; REMOVED MarkTemplateDirty (the source SetDirty/P2 pollution path).

Compile: refresh_unity(force,scripts) -> idle; read_console errors+warnings = 0; no BuildMode console output.

Invariant enforcement: ONE RoomTemplateSO clone exists per session, owned by BuildModeController (the lifecycle owner). Both tools resolve THAT instance and every edit path (brush SetFloor/SetWalkable/SetOverlay + prop ApplyPlace/RevertPlace) calls WalkabilityMap.InitFromTemplate(workingCopy), so the live Tilemap, pathing authority and prop validator share one grid. Switching Tile<->Prop no longer reverts pathing. Source .asset is never Instantiate-mutated or SetDirty'd (disk write-back deferred to Phase 4). DisableDomainReload-safe: DontSave hideFlags + nulled on Exit/OnDestroy.

BLOCKED: none. OUT-OF-SCOPE untouched (overlay stamp fallback, CS0618, no light/save/scene edits).
