# CX TASK — Q4 CLIFF: build the CORE procedural boundary-mesh cliff system (implement)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: code / STAGING / memory files.

## Amaç (purpose)
Replace the broken tilemap cliff with a **procedural boundary-mesh extrusion** so the iso floating-island reads as FLOATING with depth, and works for inner holes. Implement the **CORE** now (loop extraction + skirt mesh + extended shader + _IsoGame wire) with a PLACEHOLDER rock look (vertex color or simple built-in noise) so the orchestrator can verify the floating-depth STRUCTURE before real art is produced. Real art + full polish + helper-system adaptation come in later passes — do NOT block on them.

## READ FIRST (your own prior design — implement it)
- `CODEX_DONE_yekta.md` (this repo root) contains a detailed technical plan you (Codex) already wrote for this exact system: boundary loop extraction (outer + hole loops, signed-area classification, do NOT use the exterior-flood filter), per-loop skirt mesh (tall, taper, UV u=cumulative loop distance / v=depth), shader property list, sorting normalization to `RoomDepthStack` (Cliff = Ground/-10), and the gotchas. FOLLOW IT.
- Reference the canonical iso neighbor vectors already in `Assets/Scripts/Environment/CliffAutoPlacer.cs` (screen S=(-1,-1,0), N=(1,1,0), E=(1,-1,0), W=(-1,1,0)).
- Existing shader to EXTEND (do not replace the idea): `Assets/Shaders/CliffVoidFade.shader` + `Assets/Materials/CliffVoidFade.mat`.

## CORE deliverables (this dispatch)
1. **New `Assets/Scripts/Environment/CliffMeshGenerator.cs`** (MonoBehaviour, runs in editor via `[ContextMenu("Regenerate")]` AND at runtime on Start if `generateOnStart`):
   - Input floor occupancy from the `Ground` Tilemap (use `tilemap.cellBounds` + `HasTile`), iso grid `cellSize=(0.96,0.585)`.
   - **Boundary loop extraction:** emit an oriented edge segment for every floor-cell side whose neighbor across that side is empty; stitch segments into closed loops by shared endpoints; classify each loop outer-vs-hole by signed area. Keep BOTH. (This naturally includes inner holes — that is the whole point. Do NOT special-case/skip holes.)
   - **Skirt mesh per loop:** top vertices = the loop boundary points in world space (flush to the floor lip, NO transformOffset). Bottom vertices = top offset screen-down by `cliffHeightWorld` (serialized, default ~1.8 world units — do NOT squash) and tapered inward along the loop normal by `taperAmount` (default ~0.25). Triangulate the top→bottom strip. UVs: `u = cumulative loop distance / tileWorldLength`, `v = 1` at top → `0` at bottom. One mesh (or one child GameObject) per loop for clean sorting.
   - MeshFilter + MeshRenderer using the extended `CliffVoidFade` material. Sorting via `RoomDepthStack` (Cliff = Ground / -10) — read the project's depth-stack source, do NOT hardcode -50.
   - Placeholder rock look: vertex color or a simple procedural noise in the shader is fine for now (real tileable texture later).
   - Keep a serialized `List<Vector3Int> cliffCells` (the boundary cells) as METADATA so existing helper systems (dust/shadow/editor) can still work later. Do not wire those helpers now.
2. **Extend `CliffVoidFade.shader` + a new `.mat`** with depth properties (per your plan): `_TopOpaqueBand`, `_TopTint`/`_MidTint`/`_BottomVoidColor`, `_DarkenStrength`, `_DesaturateStrength`, `_AlphaFadeStart`/`_AlphaFadeEnd`, `_NoiseTex`+`_NoiseScale`+`_NoiseStrength`, optional `_RimShadowStrength`. Fragment: top band opaque+crisp; progressively darken+desaturate toward bottom; alpha-fade only the lower band with noise so the bottom dissolves irregularly into `_BottomVoidColor` (purple). URP 2D compatible, `ZWrite Off`, transparent queue. Keep backward-compat defaults so the old tilemap still renders if used.
3. **Wire into `Assets/Scenes/_IsoGame.unity`:** add a `CliffMeshGenerator` (new GameObject or on `CliffRing`), generate from the current `Ground` floor, and DISABLE the old `CliffTilemap` renderer (SetActive false or disable TilemapRenderer — keep it in the scene as fallback, do NOT delete). Save the scene.

## Constraints
- Unity instance `RIMA@ed023e0b` is live — use UnityMCP. `execute_code` needs `action:"execute"`, NO `using` directives (fully-qualified names like `UnityEngine.Tilemaps.Tilemap`, `UnityEditor.SceneManagement.*`).
- After creating/editing scripts, `read_console` to confirm 0 compile errors before wiring.
- Do NOT touch other scenes (Map02/03) yet — _IsoGame only for this core pass.
- Do NOT enter play mode (orchestrator verifies). Do NOT commit.

## Deliverable report (CODEX_DONE.md)
- Files created/changed (diff summary), the loop-extraction result for _IsoGame (how many loops, outer vs hole counts, vertex counts), confirmation of 0 compile errors, and the serialized param defaults used. State BLOCKED + reason if loop stitching fails or Ground floor can't be read.
