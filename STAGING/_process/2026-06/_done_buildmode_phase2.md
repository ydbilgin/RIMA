# DONE — Build Mode PHASE 2 (placement layer) — 2026-06-13

Spec: `STAGING/INGAME_BUILD_MODE_DESIGN_2026-06-13.md` (sections 3.5 BINDING, 4, Phase 2 row).
Status: IMPLEMENTED + COMPILES CLEAN (Unity, 0 errors / 0 warnings). NOT play-mode verified
(per task: do NOT enter Play Mode — orchestrator runtime-verifies).

## Files

NEW
- `Assets/Scripts/UI/BuildMode/BuildCommandStack.cs` (~88 lines)
  - `IBuildOp` (Do/Undo) + linear undo/redo stack. Place/erase are REVERSIBLE COMMANDS, not state
    snapshots. Execute clears the redo branch; Undo/Redo move ops between stacks.
- `Assets/Scripts/UI/BuildMode/BuildPlacementController.cs` (~640 lines)
  - The Phase 2 placement layer. Self-bootstrapped MonoBehaviour, ACTIVE ONLY while
    BuildModeController.IsActive (SetBuildModeActive(true/false) on enter/exit).

EDIT
- `Assets/Scripts/UI/BuildModeController.cs` (+4 hooks, ~20 lines total)
  - `using RIMA.UI.BuildMode;`
  - EnterBuildMode -> `BuildPlacementController.Instance.SetBuildModeActive(true)` (after ShowTab).
  - ExitBuildMode -> SetBuildModeActive(false) (before state restore).
  - OnDestroy safety -> SetBuildModeActive(false) (mirrors RestoreCameraRig safety).

## What it does (Phase 2 checklist)
- PALETTE from `PropRegistrySO.AllProps` (Resources.Load "Props/PropRegistry"; AssetDatabase
  fallback in-editor). Runtime ScreenSpaceOverlay panel, slot buttons, DirectorMode-style skin.
- GHOST follows mouse, SNAPPED to iso cell center, GREEN/RED tinted by validity.
- LMB place / RMB erase (nearest tracked prop within EraseRadius).
- `[` / `]` rotate 90deg (over GetRotatedFootprint iso-adjacent footprint); `F` flip (flipX);
  `E` eyedropper (pick prop type + rot + flip under cursor into the palette).
- Ctrl+Z / Ctrl+Y UNDO/REDO via BuildCommandStack (PlaceOp / EraseOp).
- `*ForValidation` data-proof hooks (overlay invisible to MCP screenshots):
  SelectFirstPropForValidation, PlaceForValidation(cell), EraseForValidation(cell),
  UndoForValidation, RedoForValidation, RotateForValidation, PlacedCountForValidation,
  HasGhostForValidation, LastGhostValidForValidation.

## SECTION 3.5 COMPLIANCE (the #1 review criterion) — ALL Grid API, zero rectangular math
- mouse -> cell: `grid.WorldToCell(mouseWorld)`.
- cell -> world (ghost + spawn): `grid.GetCellCenterWorld(cell)`, z flattened to 0.
- Grid resolved from `WalkabilityMap.Instance.floorTilemap.layoutGrid` (canonical runtime path),
  fallback `FindObjectOfType<Grid>()`. The SHARED iso grid is reused; no new Grid is created.
- Footprint/rotation: `PropFootprintValidator.GetRotatedFootprint`; validity loop iterates the same
  RectInt cell span the validator enumerates (origin = min corner).
- Props spawned via the RUNTIME RECIPE (PropSorterRuntime + PropColliderAutoBuilder) mirroring
  `IsoRoomBuilder.BuildProps`, NOT a bare Instantiate at a hand-computed position.
  NOTE: deliberately did NOT use `PropRuntimeSpawner.SpawnSingle`'s path — it places at
  `tilePosition * tileSize` (rectangular, flat XY), which violates §3.5 rule 1 on an iso grid.
  IsoRoomBuilder.BuildProps is the iso-correct precedent and is what I mirrored.

## Integration contract verification
Read every cited source file. All signatures matched reality — NO BLOCK needed.
PropRegistrySO.AllProps/ResolveGuid/RebuildIndex, PropDefinitionSO fields/PickVariantIndexForTile,
PropFootprintValidator.Validate(+rotation)/GetRotatedFootprint/ValidationResult,
PropSorterRuntime.PropDef, PropColliderAutoBuilder.PropDef/RotationSteps/EnsureCollider,
PropPlacementData(guid,pos)+rotationSteps/flipX/variantIndex/placedByUser,
RoomTemplateSO.props/IsWalkable, WalkabilityMap.Instance/IsWalkable/InitFromTemplate/floorTilemap,
IsoRoomBuilder.TryGetCellCenterWorld + BuildProps pattern, RoomRunDirector.CurrentTemplate,
CompositionRoleMapGenerator.GenerateFromRoom, BuildModeController.IsActive/Instance.

## Deviations / interpretation flags
1. SEPARATE controller instead of extending DirectorMode.UpdatePropTool (contract suggested
   extend). Reason: the existing DirectorMode prop tool is PREFAB-based + SQUARE-SNAP
   (`SnapWorld` Mathf.Round on a 1f lattice) — extending it inherits the §3.5 violation. §3.5 is
   the #1 review criterion, so Phase 2 is a clean PropRegistry/PropDefinitionSO + Grid-API layer.
   Architecture matches the task's "suggested" Assets/Scripts/UI/BuildMode/ layout.
2. PropRegistry resolution: IsoRoomBuilder/RoomRunDirector hold it privately with no accessor,
   so I load the Resources asset (Assets/Resources/Props/PropRegistry.asset confirmed present).
3. WalkabilityMap.InitFromTemplate called after place/erase to keep the walkability authority in
   sync (design-doc "walkability desync" risk). If a template grid is loaded this re-reads it.
4. Footprint visual is anchored at the origin cell (single GetCellCenterWorld), same as the
   existing IsoRoomBuilder.BuildProps spawner; multi-cell footprint VALIDITY is checked across all
   occupied cells. No new sort basis introduced (PropSorterRuntime owns Y-sort).
5. Self-bootstrap: BuildModeController.Instance creates BuildPlacementController if missing. No
   scene/.unity/.prefab edits (task constraint honored).

## DisableDomainReload safety
Lazy `_instance` getter (alive -> Find -> create DontDestroyOnLoad); OnDestroy nulls the static and
TeardownAll() destroys ghost + all placed instances + palette canvas + clears the command stack.
SetBuildModeActive(false) hides ghost/palette and clears undo history (editor-only state never
persists into the room save).

## Verification
- `refresh_unity (compile=request, force, scripts)` -> compiling -> idle.
- `read_console (error+warning)` -> 0 entries after fixing the one iteration's missing
  `MarkTemplateDirty` helper (added; recompiled clean).
- Filter `BuildMode` over errors -> 0.
- Play-mode behavior NOT verified (scope) — orchestrator to runtime-verify via the *ForValidation
  hooks.
