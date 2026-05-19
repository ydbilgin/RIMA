# Brush V1 Visual Gate Quick-Fix DONE

## Pre-check

- Unity instance: `RIMA@ed023e0b` only active instance.
- Active scene: `Assets/Scenes/Demo/RoomPipelineTest.unity`.
- Existing rig found under `StageRoot/BrushV1PaintTestRig`; paint test menu recreates it.

## Bug #1 - Opaque black rectangles

Root cause: Step 1c, chunk renderer material path.

- Step 1a importer check passed for `macro_01.png`, `rift_01.png`, and `ritual_01.png`.
  - `textureType=Sprite`
  - `alphaIsTransparency=True`
  - `textureCompression=Uncompressed`
  - `mipmapEnabled=False`
- Step 1b atlas check passed for alpha preservation.
  - Default platform override: `RGBA32`, `Uncompressed`.
  - Packed editor texture inspected through atlas-backed sprites: `RGBA32`.
- Step 1d did not apply: `RoomDecalChunkRenderer` does not assign vertex colors.
- The renderer used `Universal Render Pipeline/2D/Sprite-Lit-Default` on a `MeshRenderer`. In the paint-test camera path, transparent black source pixels rendered as opaque black quads.

Fix applied:

- `Assets/Scripts/MapDesigner/Brush/Runtime/RoomDecalChunkRenderer.cs`
  - Prefer `Universal Render Pipeline/Unlit` for generated chunk materials.
  - Configure generated materials as transparent:
    - `_Surface=1`
    - `_Blend=0`
    - `_SrcBlend=SrcAlpha`
    - `_DstBlend=OneMinusSrcAlpha`
    - `_ZWrite=0`
    - `_SURFACE_TYPE_TRANSPARENT`
    - transparent render queue
  - Keep the existing sprite shaders as fallback only.

Result:

- Black rectangles are gone in the rerun screenshot.
- Macro, rift, and ritual alpha now renders correctly.

## Bug #2 - Floor tile seams

Root cause: not Step 2a/2b padding.

- `Assets/Sprites/Environment/RIMA_AssetParts_v2/floor/floor_01.png`
  - Size: `(32, 32)`
  - Alpha bbox: `32 x 32`
  - Alpha min/max: `255/255`
- Strict grid downsample from `STAGING/RIMA_AssetParts_v2/sheet_01_floor_tiles_32x32.png` exactly matches the current `floor_01.png`.
- Floor sprites already have `spriteExtrude=1`, `filter=Point`, `compression=Uncompressed`.
- No floor PNG overwrite was performed; therefore no `_pre_quickfix_backup/` was needed.

Remaining issue:

- The visible 32x32 floor grid remains source-art-level / tile-content-level, not transparent padding or importer extrude.
- The current generated floor tile art still reads as repeated square/circuit tiles rather than a natural continuous roguelite floor.

## Files Modified

- `Assets/Scripts/MapDesigner/Brush/Runtime/RoomDecalChunkRenderer.cs`
- `Assets/Scenes/Demo/RoomPipelineTest.unity` was saved by `Tools/Brush V1/Run Paint Test`.
- `STAGING/Brush_V1_paint_test_screenshot_02.png`

No SO contract scripts were modified. No Phase 1.5 data-first executor scripts were modified.

## Screenshot

- New screenshot: `STAGING/Brush_V1_paint_test_screenshot_02.png`
- Paint test menu also refreshed `STAGING/Brush_V1_paint_test_screenshot_01.png`.

## Visual Gate Verdict

PARTIAL.

- PASS: opaque black rectangles are gone.
- PASS: decals and focal accents lay flat with alpha preserved.
- FAIL: per-tile floor grid remains visually obvious, but the cause is source-art/tile-content, not alpha padding, slicer padding, or missing sprite extrude.
- Overall composition still does not fully read as a natural roguelite floor because the floor base is visibly tiled.

## Tests

- EditMode: `333/333` passed.
- Delta: unchanged from required count.

## NEXT_SIGNAL

STILL FAIL / PARTIAL: orchestrator should escalate floor source-art direction or HD-2D escape hatch review. The material bug is fixed; the remaining visual gate blocker is the floor source art.
