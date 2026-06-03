# TASK — Apply: Map Designer schema fix (code) + STAGING declutter (safe moves)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / asset files / AGY_DONE_ydbilgin.md.

Profile: laurethayday. Effort: high. Language: English report.

## Amaç (Goal)
Apply the approved cleanup/fix plan in `AGY_DONE_ydbilgin.md` (read it — sections C and D are your spec). This dispatch does ONLY the two SAFE, high-value parts. **Do NOT do Assets/ asset archiving (section A) or scene edits (section B) — those need Unity open + reference repointing and are explicitly DEFERRED.**

**SAFETY (user has lost work before):** archive/move, do NOT hard-delete. Surgical. Do NOT commit (gated — leave in working tree for Opus review; writer ≠ reviewer).

---

## PART 1 (PRIORITY) — Map Designer schema unification (CODE)
Root cause (from ax/NLM): the Editor saves rooms as `RoomDataDTO` JSON (`floorCells` → `TileCellRecord.assetGuidOrName`), but the runtime `LiveRoomReloader` deserializes as `RoomLayoutData` (`floor_tiles` → `FloorTileData.tile_guid`) → schema mismatch → saved rooms never appear at F5/runtime. Plus a path mismatch (editor writes `Assets/Data/Rooms/<id>.room.json`, reloader watches `StreamingAssets/live/room_current.json`).

Do (grep for exact paths/signatures first; report them):
1. **Unify on `RoomDataDTO`:** Modify `LiveRoomReloader.cs` to deserialize `room_current.json` as `RoomDataDTO` (not `RoomLayoutData`). Update its `ApplyFloorTiles` / `ApplyCliffTiles` to iterate `RoomDataDTO.floorCells` / `cliffCells` and resolve via `RuntimeAssetRegistry.Instance.GetTile(assetGuidOrName)`; update `ApplyPropDiff` to iterate `RoomDataDTO.propPlacements` and spawn via `RuntimeAssetRegistry.Instance.GetPrefab(...)`. Verify these RuntimeAssetRegistry methods exist with those signatures (grep) — if names differ, use the real ones and note it.
2. **Standalone LiveTool:** Change `ToolBootstrap.cs` internal doc model (`_liveDoc`) from `RoomLayoutData` to `RoomDataDTO`; adjust `RequestSave()` to serialize `RoomDataDTO`.
3. **Editor → live bridge:** Make `UnifiedMapDesigner.cs` / `RoomDataAuthoringController.cs` (whichever owns Save) ALSO write the serialized `RoomDataDTO` JSON to `StreamingAssets/live/room_current.json` on save/paint, so editor changes reach the running game. Keep the existing `.asset` + `Assets/Data/Rooms/<id>.room.json` writes.
4. **Retire `RoomLayoutData.cs`:** If nothing else references it after the above, mark it `[Obsolete]` (do NOT delete — surgical). Grep for ALL references first; if other live code uses it, do NOT obsolete — instead report the remaining users.
5. Keep changes minimal and compile-consistent. You likely CANNOT run Unity tests headlessly — do NOT claim tests pass. Instead: ensure the code is internally consistent (fields/methods exist), and in your report list exactly what must be verified in Unity (compile + `UnifiedDesignerTests.cs`).

## PART 2 — STAGING declutter (SAFE file moves, OUTSIDE Assets/, zero Unity risk)
Per `AGY_DONE_ydbilgin.md` section D. Move clearly-old/superseded directories under `STAGING/` into `STAGING/_archive/` (create if missing). Move (not delete):
- Old session/task dirs: `s106_*`, `s107_*`, `s108_*`, `_archive_old_tasks`, `_archive_old_analysis`, `_archive_pre_s99`, `_archive_mocks`, `AUTO_TEST_REPORT_*`, and any other dir clearly tied to a CLOSED sprint (S99–S114) or an abandoned pivot.
- **KEEP LIVE (do NOT move):** `agy_snapshots/` (LIVE config — NEVER touch), `agy_test/`, `livetool_t3/`, and current S6 diagnostic dirs: `tiles_pro_flat_floor/`, `ISO_*`, `ROOMTOOL_*`, `FLOOR_*`, `iso_tile_candidates/`, `iso_thin_candidates/`, `UNIFIED_*`, `FIXFWD_*`, `concepts/`, and any `*_S6.md` task/spec files referenced by the current CURRENT_STATUS top block.
- When unsure whether a dir is closed-sprint vs live → KEEP it and list it as "unsure" in your report. Conservative.
- Report: how many dirs/files moved, the resulting top-level STAGING count.

## DEFER — do NOT do in this dispatch (note only)
- **Section A (Assets/ floor/asset archiving):** the redundant floor sets (`PixelLabFloor`, `PixelLabFloorFlat`, `PixelLabFloorIso*`) are still WIRED into `DemoMap_S6.unity` + `RuntimeAssetRegistry` → moving them breaks refs. Needs repoint-to-451 in Unity first. DEFERRED.
- **Section B (scene object cleanup):** needs Unity open. DEFERRED.
- **Root-level .md cleanup:** DEFERRED (must protect active dispatch I/O — see below).

## DO NOT TOUCH (hard list)
- Active dispatch I/O & tooling: `CODEX_TASK.md`, `CODEX_DONE*.md`, `AGY_DONE*.md`, `cx_dispatch.py`, `agy_dispatch.*`, `agy_detached*`, `ax_*`, `cx_profiles.local.json`, `.cx_dispatch_locks/`, `.agy_*`.
- Canonical root files: `CLAUDE.md`, `RULES.md`, `AGENTS.md`, `CURRENT_STATUS.md`, `README.md`, `SYSTEM_MAP.md`.
- `STAGING/agy_snapshots/` (LIVE).
- Anything under `Assets/` (no asset moves this dispatch).

## Deliverable (write to CODEX_DONE_laurethayday.md, last step)
- Part 1: exact files + functions changed, what you verified statically, and a checklist of what MUST be verified in Unity (compile + UnifiedDesignerTests). List any RuntimeAssetRegistry signature differences you hit.
- Part 2: dirs moved + counts + new STAGING top-level count + any "unsure, kept" items.
- End with a `STATUS:` line. NO commit.
