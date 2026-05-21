# Codex Task — Brush V1 Visual Gate Quick-Fix (Alpha + Floor Seam)

**Profile:** laurethayday (laurethgame limit dolu 03:13'e kadar, yasinderyabilgin önceki dispatch yaptı)
**Effort:** high
**Type:** Bug diagnosis + import/atlas/slicer fix + paint test re-run

## Context

Previous dispatch (`STAGING/CODEX_TASK_BRUSH_V1_PAINT_TEST_VISUAL_GATE_DONE.md`) — visual gate FAIL with 2 bugs:

1. **Opaque black rectangles** where macro patches + Rift accent + Ritual accent should render. Sprites have alpha but render solid black (mesh material or atlas pack lost alpha).
2. **Per-tile floor seams** — visible dark grid between 32×32 floor tiles. Sliced PNGs have transparent padding (Python slicer `pad=2`) causing UV bleed in chunked mesh.

Screenshot evidence: `STAGING/Brush_V1_paint_test_screenshot_01.png`.

This task diagnoses the root cause for each, applies the minimal fix, re-runs the same `Tools/Brush V1/Run Paint Test` menu, captures a new screenshot, and gives a fresh visual gate verdict.

## CRITICAL — Unity instance pre-check

Unity is OPEN. Previous dispatch verified instance `RIMA@ed023e0b` and scene `Assets/Scenes/Demo/RoomPipelineTest.unity`. Before any tool call:

1. List active Unity instances via `mcpforunity://instances`. Pin `RIMA` via `set_active_instance` if multiple.
2. `manage_scene action=get_active` — must be `RoomPipelineTest`. If not, load it.
3. Existing setup: `BrushV1PaintTestRig` GameObject under `StageRoot` (from previous run). Reuse or recreate.

## Task 1 — Diagnose bug #1 (opaque black rectangles)

Investigate which step lost alpha for the large sprites (macro 128×128, rift/ritual 256×256). Check IN ORDER:

### Step 1a — TextureImporter settings on the large sprites

For each of these 3 sample paths, read `TextureImporterPlatformSettings` + base properties:
- `Assets/Sprites/Environment/RIMA_AssetParts_v2/macro/macro_01.png`
- `Assets/Sprites/Environment/RIMA_AssetParts_v2/rift/rift_01.png`
- `Assets/Sprites/Environment/RIMA_AssetParts_v2/ritual/ritual_01.png`

Expected (per Step 4 DONE marker):
- `textureType = Sprite`
- `alphaIsTransparency = true`
- `textureCompression = Uncompressed`
- `mipmapEnabled = false`

If any field differs, document — but this likely isn't the bug since the Step 4 DONE marker explicitly verified these. Move to Step 1b.

### Step 1b — SpriteAtlas pack inspection

Inspect `Assets/Data/Brush/AssetParts_v2/RIMA_AssetParts_v2.spriteatlas`:

- Open via `AssetDatabase.LoadAssetAtPath<UnityEngine.U2D.SpriteAtlas>`
- Check `SpriteAtlas.GetPlatformSettings("DefaultTexturePlatform")` and per-platform `TextureFormat`
- The Step 4 DONE marker says "Compression: None / Uncompressed preview settings" but verify the actual packed format does NOT discard alpha (e.g., `RGB24` vs `RGBA32`)
- Try `SpriteAtlasUtility.PackAtlases(BuildTargetGroup.Standalone, AtlasPackTarget.Editor)` then re-inspect

If atlas packed sprites lost alpha → fix: set platform format to `RGBA32` (or `RGBA Crunched`), re-pack.

### Step 1c — Chunk renderer material

Inspect what material the `RoomDecalChunkRenderer` MeshRenderer uses. Read source `Assets/Scripts/MapDesigner/Brush/Runtime/RoomDecalChunkRenderer.cs` to see if it assigns a specific shader (e.g., `Sprites/Default` vs `Universal Render Pipeline/Sprite-Lit-Default`). If unlit `Sprites/Default` AND project uses URP 2D Renderer, the shader may render as opaque black under 2D Light environment (no light hitting the unlit material). Try forcing `Sprites/Default` to all chunk renderer materials in a test bypass, OR assign `Universal Render Pipeline/Sprite-Lit-Default`.

### Step 1d — Mesh UV / vertex color

If shader is correct but still black, the chunked mesh's vertex colors may be set to `(0,0,0,0)` and the shader multiplies. Check `RoomDecalChunkRenderer.Build()` source for vertex color assignment.

**Apply the SMALLEST fix that resolves bug #1.** Do NOT refactor the executor or chunked renderer architecture.

## Task 2 — Diagnose bug #2 (floor tile seams)

The floor sliced PNGs (`floor_01..16.png` at 32×32) have transparent padding from Python slicer (`pad=2` from STAGING/RIMA_AssetParts_v2/sliced/floor/). When chunked mesh tiles them in a grid, the transparent padding creates visible seams between tiles.

### Step 2a — Verify sliced floor PNG content size

Read pixel dimensions and alpha bounding box for `Assets/Sprites/Environment/RIMA_AssetParts_v2/floor/floor_01.png`:

```python
from PIL import Image
import numpy as np
img = Image.open('Assets/Sprites/Environment/RIMA_AssetParts_v2/floor/floor_01.png').convert('RGBA')
arr = np.array(img)
print('Size:', img.size, 'Alpha bbox:', arr[:,:,3].any(axis=0).sum(), 'x', arr[:,:,3].any(axis=1).sum())
```

Expected: `Size: (32, 32) Alpha bbox: 32 x 32` (full cell opaque).
If alpha bbox is 28×28 or similar → padding exists → re-slice required.

### Step 2b — Re-slice floor with no autocrop, no padding

The original slicer used `do_crop=False` for floor sheet (since floor is opaque). But the bbox check will tell. If padding exists:

Run a fresh Python slicer over source sheet `STAGING/RIMA_AssetParts_v2/sheet_01_floor_tiles_32x32.png` that strict-grids to 4×4 cells, downsamples each 256×256 cell to 32×32 nearest-neighbor with NO autocrop, NO padding. Overwrite `STAGING/RIMA_AssetParts_v2/sliced/floor/floor_01..16.png`. Then `AssetDatabase.ImportAsset` for each target Asset/ path.

### Step 2c — Sprite Editor "Extrude Border" (alternative if seams persist)

In Unity's TextureImporter for each floor sprite, set `extrudeEdges = 1` (one-pixel border duplication on sprite UV) — prevents UV bleed at adjacent tile edges.

**Apply the SMALLEST fix.**

## Task 3 — Re-run paint test

After fixes:

1. `execute_menu_item Tools/Brush V1/Run Paint Test`
2. Wait for `editor_state.isCompiling = false`
3. `manage_camera action=screenshot capture_source=scene_view screenshot_file_name=Brush_V1_paint_test_screenshot_02.png include_image=true max_resolution=1280`
4. Output: `STAGING/Brush_V1_paint_test_screenshot_02.png`

## Task 4 — Visual gate verdict (re-evaluate)

Looking at the new screenshot:

- **Black rectangles GONE?** (alpha fix worked)
- **Floor tile seams GONE?** (slicer fix worked)
- **Overall composition** — does it read as natural roguelite floor now?

**PASS criteria:** No opaque black rectangles, no per-tile floor grid, decals lay flat on floor, focal accents read as rare beats.
**STILL FAIL:** Document what remains broken with concrete cause (mesh-level vs material-level vs source-art-level).

## Task 5 — Write DONE marker

`STAGING/CODEX_TASK_BRUSH_V1_VISUAL_GATE_QUICKFIX_DONE.md`:

- Bug #1 root cause (which step 1a/b/c/d) + fix applied
- Bug #2 root cause (Step 2a/b/c) + fix applied
- Files modified (TextureImporter overrides, atlas re-pack, slicer re-run, etc.)
- New screenshot path
- Visual gate verdict: PASS / FAIL / PARTIAL with specifics
- EditMode test count delta (must remain 333/333)

## Constraints

- Do NOT modify SO contract scripts (`PatchAtlasSO.cs`, etc.)
- Do NOT modify Phase 1.5 data-first executor scripts
- Do NOT refactor `RoomDecalChunkRenderer` — investigate only, minimal fix
- Use existing `PaintTestExecutor.cs` (do not rewrite)
- Backup originals before overwriting sliced PNGs (`_pre_quickfix_backup/`)

## NEXT_SIGNAL

If PASS: ChatGPT FINAL Phase 1A continues. User reviews new screenshot in next session.
If STILL FAIL: orchestrator escalates — likely HD-2D escape hatch (`STAGING/CODEX_STRATEGIC_2D_vs_HD2D.md`) since 2 bug fixes still didn't yield natural composition.
