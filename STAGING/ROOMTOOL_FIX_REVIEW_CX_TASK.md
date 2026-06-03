# ROOMTOOL FIX REVIEW — adversarial code review (cx)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

## Amaç (goal)
Opus applied the 4 BLOCKING fixes from `STAGING/ROOMTOOL_VERIFY_REPORT_S6.md` (Townscaper-2D map tool, STEP 0-11).
This is a REVIEW-ONLY task (writer != reviewer). DO NOT rewrite. Read the current files, confirm each fix is
correctly + minimally applied, and hunt for regressions/edge-cases the author missed. Report PASS/FAIL per fix.

Already verified by author: Unity compile 0-error; EditMode `RIMA.Tests.RoomPainter.WangResolverTests` = 19/19 pass
(incl. new CCW table cases + new segment-migration test). So focus on logic correctness the tests DON'T cover:
runtime persistence flow, Editor asset-create, id-contract edge cases, and the rotation table vs Unity's CCW render.

## Changed files (4 are NEW/untracked from STEP 0-11, 1 tracked-modified)
1. `Assets/Scripts/RoomPainter/WangResolver.cs` — Fix 2: Resolve4 CCW rotation table.
2. `Assets/Tests/EditMode/RoomPainter/WangResolverTests.cs` — Fix 2 CCW cases + Fix 4 migration test.
3. `Assets/Scripts/RoomPainter/RoomDataJson.cs` — Fix 4: ToDto migrates wallSegments->cells before copy.
4. `Assets/Scripts/Systems/Map/RoomConfig.cs` — Fix 1: new `RoomData roomData` reference field.
5. `Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs` — Fix 1 (HandleRoomLoaded/LoadActiveRoomData) + Fix 3
   (CopyRoomDataToAsset create-on-no-match) + Fix 4B (canonical-asset seed/migrate on missing JSON).

## What to verify per fix
- **Fix 2 (CCW table):** Independently re-derive the open-side/connection rotations assuming Unity
  `Quaternion.Euler(0,0,+theta)` rotates CCW and canonical art = End-connects-South@0 / Corner-connects-S+E@0 /
  T-opens-North@0. Confirm the switch matches: End S0/E90/N180/W270, Corner S+E0/N+E90/N+W180/S+W270,
  T mask14(openN)0 / mask7(openW)90 / mask11(openS)180 / mask13(openE)270. Flag ANY value that would render
  mirrored or 90deg off on screen. (Grey-box is square so this is logic-only until real directional art.)
- **Fix 1 (id contract):** Does F2 now derive the canonical id from `config.roomData.roomId` (not the loose
  `config.roomId` string)? Edge case: `roomData` set but its `roomId` blank at runtime -> what happens? Blank-id
  warn + `runtime_room` fallback acceptable? Any path where two different rooms still collapse to one file?
- **Fix 3 (asset-create):** `CopyRoomDataToAsset` no-match branch: `Instantiate(source)` + `CreateAsset` +
  folder-create guards. All inside `#if UNITY_EDITOR`? Will it double-create if called twice (same roomId)?
  (Second call should now MATCH the just-created asset and CopySerialized, not create a dup — confirm.)
- **Fix 4 (migrate-on-write):** `MigrateSegmentsToCells` in `ToDto` is idempotent + safe to run every write?
  Any room where this loses data or duplicates cells? `wallSegments` left uncleared (stale) — acceptable?
- **Cross-cutting:** runtime-safety (no UnityEditor symbol leaks outside guards), NullRefs, the
  `Instantiate` in static `CopyRoomDataToAsset` (inherited static — compiles? it did, but confirm intent).

## Output format
For each fix: `FIX n: PASS | FAIL — <one-line evidence>`. Then a short `RISKS` list (anything for the F5 smoke to
pin). End with `OVERALL: SHIP | FIX-FIRST`. Keep it tight — no code rewrites, evidence + line refs only.
