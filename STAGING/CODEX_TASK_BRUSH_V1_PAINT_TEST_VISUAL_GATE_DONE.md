# Brush V1 Paint Test Visual Gate DONE

Scene: `Assets/Scenes/Demo/RoomPipelineTest.unity`

Setup notes:
- Unity instance was available as `RIMA@ed023e0b`.
- Active scene was already `RoomPipelineTest`.
- Created fresh `BrushV1PaintTestRig` under `StageRoot`.
- Configured `PaintTest_PipelineConfig.asset` with `useDataFirstDecals=true` and `useDataFirstScatter=true`.
- Added a directional `Main Light` at warm white intensity 0.6 and kept `Global Light 2D` warmed to the same color/intensity.
- `RoomDecalChunkRenderer` has no static `RebuildAll()` API in the current codebase, so the test caller rebuilt each generated renderer with `Build()`.

Paint operation count:
- BaseFloor: 48
- MacroPatch: 4
- OrganicDecal_Moss: 10
- OrganicDecal_Dirt: 7
- DetailScatter_Pebbles: 12
- DetailScatter_CracksBones: 6
- Accent_Rift: 1
- Accent_Ritual: 1
- Total placements: 89

Screenshot:
- Path: `STAGING/Brush_V1_paint_test_screenshot_01.png`
- Dimensions: 1280x720

Console validation:
- Compile errors: 0 after final script refresh.
- NullReference / MissingReference in paint execution: 0 after final run.
- Missing sprite warnings: 0.
- Unity console after test validation contained 1 MCP transport error, 16 MCP/test-runner exception entries, and 4 warning entries. These were transport/TestRunner/performance-test noise plus one legacy brush warning from the EditMode suite, not paint execution failures.

Renderer validation:
- `RoomDecalChunkRenderer` hosts in scene: 7
- Mesh filters with meshes: 21
- Non-empty meshes: 7
- Total generated vertices: 356

EditMode test status:
- Result: PASS
- Count: 333/333 passed, 0 failed, 0 skipped.
- Delta from expected: no regression.

Visual gate verdict: FAIL

The screenshot does not meet the natural roguelite floor bar: the base floor reads as a visible square tile grid, not a painted floor. Large opaque black rectangles dominate the room, which points to transparent-background alpha/import/material handling problems in the macro/accent or large-slice assets. Focal accents are present but cannot read as rare intentional beats because the black quads occlude the composition. This looks fixable first as an alpha/material/import issue, but the strict per-tile floor seams are a separate art/composition problem that likely needs larger blended floor coverage or overlap, not just a one-line renderer tweak.

Next signal:
- FAIL path: orchestrator should decide between Phase 1A alpha/floor-seam fixes and the HD-2D escape hatch described in `STAGING/CODEX_STRATEGIC_2D_vs_HD2D.md`.
