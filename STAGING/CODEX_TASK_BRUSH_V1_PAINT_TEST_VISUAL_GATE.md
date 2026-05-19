# Codex Task — Brush V1 Paint Test + Visual Gate Verdict

**Profile:** laurethgame (parallel-safe: laurethayday boş, yasinderyabilgin boş)
**Effort:** high
**Type:** Unity-MCP scene setup + Editor-side paint execution + Scene view screenshot

## Context

Step 4 DONE — 84 PNG imported, 7 PatchAtlasSO + 1 SpriteAtlas at `Assets/Data/Brush/AssetParts_v2/`. 333/333 EditMode PASS. Phase 1.5 data-first executors LIVE behind feature flag.

This task validates the **end-to-end Brush V1 paint pipeline** with the new asset parts: floor → macro patches → organic decals → detail scatter → focal accents. Output is a Scene view screenshot used as the visual gate verdict.

Per ChatGPT FINAL plan (L0-L11 14-dispatch) and 3-verdict consensus LOCK: if visual gate PASS → V1 ship trajectory continues. If FAIL → HD-2D escape hatch (`STAGING/CODEX_STRATEGIC_2D_vs_HD2D.md`).

## Inputs

| Input | Path |
|---|---|
| Sample RoomTemplateSO | `Assets/Data/Rooms/Library/Combat_Small_01.asset` |
| Test scene | `Assets/Scenes/Demo/RoomPipelineTest.unity` |
| BaseFloor atlas | `Assets/Data/Brush/AssetParts_v2/BaseFloor.asset` (24 variants — 16 floor + 8 macro) |
| Moss atlas | `Assets/Data/Brush/AssetParts_v2/OrganicDecal_Moss.asset` (16) |
| Dirt atlas | `Assets/Data/Brush/AssetParts_v2/OrganicDecal_Dirt.asset` (12) |
| Pebbles atlas | `Assets/Data/Brush/AssetParts_v2/DetailScatter_Pebbles.asset` (12) |
| Cracks/bones atlas | `Assets/Data/Brush/AssetParts_v2/DetailScatter_CracksBones.asset` (12) |
| Rift atlas | `Assets/Data/Brush/AssetParts_v2/Accent_Rift.asset` (4) |
| Ritual atlas | `Assets/Data/Brush/AssetParts_v2/Accent_Ritual.asset` (4) |

## CRITICAL — Unity instance pre-check

Unity is ALREADY open with `Assets/Scenes/Demo/RoomPipelineTest.unity` loaded as active scene. Before any tool call:

1. List active Unity instances via `mcpforunity://instances` resource. If multiple, the project name is `RIMA` — call `set_active_instance` with that hash to pin routing.
2. Call `manage_scene action=get_active` — verify name == `RoomPipelineTest`. If different, call `manage_scene action=load path=Assets/Scenes/Demo/RoomPipelineTest.unity`.
3. If `unityMCP` MCP returns no instances, document failure in DONE marker and STOP.

## Tasks

### 1. Scene setup

- Active scene already `RoomPipelineTest.unity`. Verify hierarchy via `manage_scene action=get_hierarchy` — expect roots: Grid, StageRoot, Main Camera, Global Light 2D, Pipeline Controller, EventSystem, ScatterRoot.
- A previous manual test created `BrushV1PaintTest` root that was deleted — start fresh under `StageRoot`.
- Create empty GameObject `BrushV1PaintTestRig` under `StageRoot` at origin.
- Add camera if missing — orthographic, near=-100 far=100, framed on Combat_Small_01 bounds (read bounds from the RoomTemplateSO, room is 8×6 tiles or similar; centre camera on bounds midpoint, ortho size = `bounds.height / 2 + 2` for padding).
- Ensure Main Light directional, color warm white, intensity 0.6.

### 2. Brush pipeline config

- Create or locate `BrushPipelineConfigSO.asset` at `Assets/Data/Brush/AssetParts_v2/PaintTest_PipelineConfig.asset`.
- Enable `useDataFirstDecals = true` (Phase 1.5 path validation).
- Enable `useDataFirstScatter = true`.
- Other defaults.

### 3. Composed paint operations (Editor-side code, `execute_code`)

Generate `Assets/Editor/Brush/PaintTestExecutor.cs` (or reuse if exists) — a one-shot Editor menu item `Tools/Brush V1/Run Paint Test` that:

1. Reads Combat_Small_01.asset bounds.
2. Stamps `BaseFloor` tiles across full bounds (deterministic — seeded random tile pick per cell, seed=42).
3. Adds 3-5 macro patches via `MacroPatch` role from BaseFloor atlas at organic floor positions.
4. Places 8-12 `OrganicDecal_Moss` via FreeformDecalDataExecutor (data-first path) at random positions inside bounds, seed=42 derived per index.
5. Places 6-8 `OrganicDecal_Dirt` similarly.
6. Scatters 12-16 `DetailScatter_Pebbles` via ScatterAlongStrokeDataExecutor along an imaginary stroke (corner-to-corner diagonal).
7. Scatters 6-8 `DetailScatter_CracksBones` similarly.
8. Places **1** `Accent_Rift` near room center (focal beat — rare).
9. Places **1** `Accent_Ritual` at offset position.
10. Commits all placements to `RoomDecalDataSO` instances, then triggers `RoomDecalChunkRenderer.RebuildAll()`.

If `FreeformDecalDataExecutor` requires a different invocation pattern, read its source and adapt — do NOT modify the executor itself, only the test caller.

### 4. Execute + capture

- Invoke `Tools/Brush V1/Run Paint Test` programmatically (`execute_menu_item`).
- Wait for compilation + rebuild to settle (`editor_state.isCompiling = false`).
- Capture Scene view as PNG (1280×720 minimum):
  - Output: `STAGING/Brush_V1_paint_test_screenshot_01.png`
  - Use `EditorWindow.GetWindow<SceneView>()` and read its render texture, or Camera.Render to RenderTexture + ReadPixels.
  - Frame must show ALL placements (full Combat_Small_01 bounds visible).

### 5. Validate

- `read_console` — no compile errors, no NullReference, no missing sprite warnings.
- Run EditMode tests: expect **333/333 PASS** still (no regression).
- Verify chunked renderer mesh exists in scene (count of `RoomDecalChunkRenderer` MeshRenderer instances).

### 6. Visual gate verdict (Codex judgment)

In DONE marker, give an opinion on the Scene view screenshot:

- **PASS criteria:**
  - Composition reads as "natural roguelite floor" (Hades/Diablo 2 tone)
  - No grid leakage (no visible per-tile borders on floor)
  - No magenta/cyan/pink artifacts (alpha clamp verification)
  - Decals lay flat-ish on floor with 30-35° angle consistency
  - Focal accents (rift + ritual) feel rare/intentional, not noisy

- **FAIL criteria:**
  - Visible per-tile floor seams (grid)
  - Color cast bleed on transparent edges
  - Decals appear floating or wrong angle
  - Overall reads "tile-pack" not "painted room"

### 7. Write DONE marker

`STAGING/CODEX_TASK_BRUSH_V1_PAINT_TEST_VISUAL_GATE_DONE.md`:
- Scene file path + setup notes
- Paint operation count (per category, actual placed)
- Screenshot path + dimensions
- Console error/warning count
- EditMode test status (delta)
- **Visual gate verdict: PASS or FAIL** with 3-5 sentence justification
- If FAIL: specific fixable issues vs systemic projection problem (helps decide if Phase 1A tweak vs HD-2D escape)

## Constraints

- Do NOT modify any PatchAtlasSO assets created in Step 4
- Do NOT modify Phase 1A SO contract scripts
- Do NOT modify Phase 1.5 data-first executor scripts
- Use existing `BrushPipelineConfigSO` schema — do not add fields
- If Editor MCP times out, retry once with smaller paint operation set (skip macro patches, focus floor+moss+rift)
- Unity must be open. If not connectable, document failure in DONE marker

## Memory references

- `[[multi-projection-architecture-lock]]` — 6 hard rule, visual gate criteria
- `[[room-composer-paint-intent-lock]]` — paint-intent semantic brush 3-mode
- `[[brush-tool-v1]]` — Brush V1 ship-ready state + executors

## NEXT_SIGNAL after DONE

If PASS: ChatGPT FINAL Phase 1A continues (L0-L11 remaining dispatches). User reviews screenshot in next session.
If FAIL: orchestrator dispatches HD-2D kill-criteria prototype per `STAGING/CODEX_STRATEGIC_2D_vs_HD2D.md:39-66`.
