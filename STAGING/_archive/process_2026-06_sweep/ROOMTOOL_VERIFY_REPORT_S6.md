# ROOMTOOL VERIFY REPORT — S6 (Townscaper-2D map tool, STEP 0-11)

Tech-lead consolidation of 4 adversarial audits (A Wang / B runtime-safety / C shared-RoomData / D composer-regressions).
Status: compiles 0-error, 16-mask EditMode test passed, interactive smoke NOT run. Verdicts cross-checked against live code.

## Per-dimension verdict

| Dim | Area | Verdict | Why |
|---|---|---|---|
| A | Wang resolver / rotation | **FAIL** | T-family masks render 90deg off; whole End/Corner/T table is CW-authored vs Unity CCW render (art-recoverable but must be pinned). |
| B | Runtime-safety / DEVELOPMENT_BUILD | **PASS** | All `UnityEditor.*` correctly guarded; runtime asmdef clean; preprocessor pairs balanced. No leaks. |
| C | Shared RoomData / persistence | **FAIL** | roomId-bridge gap (Editor GUID vs RoomConfig string) -> two surfaces read/write different JSON; `wallSegments` dropped on JSON round-trip; F2-new rooms never get an .asset. |
| D | Composer / regressions | **PASS** | Variant fallback, Y-sort, segment->cell migration, STEP-0 cleanup all correct. 3 minor watch-items only. |

Two dimensions FAIL -> overall **FIX-FIRST**. The failures are not in the headline mechanic plumbing (single resolver, single mutator, struct writeback, migration, guards are all correct) — they are in (1) the rotation lookup table and (2) the id contract that ties the two surfaces to the same file.

---

## ORDERED FIX LIST (blocking first)

### BLOCKING

**1. roomId bridge: Editor `.asset` and F2 overlay read/write DIFFERENT JSON files.** (owner: Opus — design decision + code)
- `RoomData.EnsureIdentity` (`RoomData.cs:51-56`) assigns `roomId = Guid.NewGuid().ToString("N")`. The Editor saves/loads JSON keyed on this GUID.
- F2 `HandleRoomLoaded` (`InPlayMapPaintOverlay.cs:617`) sets `_activeRoomId = config.roomId` from the hand-authored `RoomConfig` string, then `LoadActiveRoomData` (`:528`) reads `RoomDataPaths.JsonFor(_activeRoomId)`.
- `CopyRoomDataToAsset` (`InPlayMapPaintOverlay.cs:565`) matches purely on `target.roomId == source.roomId`.
- Result: unless an author manually sets `RoomConfig.roomId` == the RoomData GUID, F2 edits land in `<RoomConfig.roomId>.room.json` while the Editor uses `<GUID>.room.json` — the "ONE shared RoomData" goal is defeated. Worse: blank `RoomConfig.roomId` collapses ALL live rooms to `runtime_room.room.json` (`:527` fallback), so different rooms clobber each other.
- Fix: establish ONE id contract. Either (a) make `RoomConfig` carry/point at the canonical RoomData asset (resolve the RoomData reference directly and read its `roomId`), or (b) have the Editor adopt `RoomConfig.roomId` as the canonical `roomId` when authoring a spine room (stop auto-GUID for spine-linked rooms). Then make `CopyRoomDataToAsset` create the `.asset` if none matches (see fix 3) so the link is bidirectional. Also reject/error on blank `RoomConfig.roomId` instead of silently falling back to `runtime_room` (prevents cross-room clobber).

**2. T-family rotations are 90deg off; whole End/Corner/T table is CW vs Unity CCW.** (owner: Opus to LOCK the convention, then cx-code applies)
- `WangResolver.Resolve4` (`WangResolver.cs:73-76`). Open-side mapping of the popcount-3 masks:
  - mask 7 (N+E+S, open **W**) -> code 180f
  - mask 11 (N+E+W, open **S**) -> code 270f
  - mask 13 (N+S+W, open **E**) -> code 90f
  - mask 14 (W+E+S, open **N**) -> code 0f (correct anchor)
- Unity `Quaternion.Euler(0,0,+theta)` (applied verbatim in `WallRunBuilder.PlaceOne`, `WallRunBuilder.cs:134`) rotates **CCW**. Canonical T = open-N. Under CCW the open side travels N->W->S->E as theta increases, so the correct table is open-N->0, open-W->90, open-S->180, open-E->270.
- The code instead gives open-E->90, open-W->180, open-S->270 — a CW table. This is the SAME root cause as the End/Corner rows, which are also CW (spec comment says "CCW from canonical" but End has W->90/E->270, which is CW). The 16-mask EditMode test passed only because it asserts the table against itself, not against render direction.
- Verdict A additionally proved masks 7 and 11 are self-inconsistent even within a CW reading and contradict the spec's own T row (openS->180, openW->270). Either way the fix touches the same lines.
- Fix: LOCK CCW (to match Unity) and re-derive the whole popcount-1/2bent/3 table in one pass:
  - End: S->0, **W->270**, N->180, **E->90** (currently W->90, E->270)
  - Corner: S+E->0, **S+W->270**, N+W->180, **N+E->90** (currently S+W->90, N+E->270)
  - T: open-N->0, open-W->90 (mask7), open-S->180 (mask11), open-E->270 (mask13)
  - Straight/Cross/Single are flip-symmetric, unchanged.
  Then update the EditMode test to assert the CCW table so it can't silently re-pass a CW regression. (Alternative if canonical sprites are authored mirrored: negate — `Quaternion.Euler(0,0,-rotationDegrees)` — but CCW-correct table is cleaner and matches the spec comment.) This is art-recoverable but MUST be visually pinned in the interactive smoke (place a single End next to one neighbor: the cap must face the neighbor, not away).

**3. F2-authored brand-new room never produces an `.asset` (silent Editor<->F2 divergence).** (owner: cx-code)
- `CopyRoomDataToAsset` (`InPlayMapPaintOverlay.cs:556-573`) only scans existing `Assets/Data/Rooms` assets and matches on `roomId`. On the first-run path (`LoadActiveRoomData:532` creates a fresh in-memory `ScriptableObject`), no `.asset` matches, the loop falls through, and only the JSON is written. The room is invisible to the Editor Map Library (`t:RoomData` listing) until someone hand-creates the asset.
- Fix: when the scan finds no match, create the asset — `AssetDatabase.CreateAsset(Object.Instantiate(source), $"Assets/Data/Rooms/{source.roomId}.asset")` then `SaveAssets` (all under the existing `#if UNITY_EDITOR`). Pairs with fix 1 so the GUID/RoomConfig id contract is honored on creation.

**4. JSON round-trip drops `wallSegments` -> silent loss for un-migrated rooms.** (owner: cx-code)
- `RoomDataDTO` (`RoomDataJson.cs:9-17`) has no `wallSegments` field; `ToDto` (`:21`) and `ApplyTo` (`:39`) never carry it. The Editor migrates segments->cells on Compose (`RoomDataComposer.MigrateSegmentsToCells`), but F2 `LoadActiveRoomData` reads straight from JSON and never migrates — so a legacy segment-only room opened first in F2 yields ZERO walls, and any room round-tripped through F2 save permanently loses its `wallSegments`.
- Fix (pick one): (a) add `wallSegments` to `RoomDataDTO` + carry it in `ToDto`/`ApplyTo`, OR (b) call `RoomDataMutator.MigrateSegmentsToCells(room)` inside `RoomDataJson.ReadRoom` (or in `LoadActiveRoomData` right after read) so segments are folded into `wallCells` before they can be dropped. (b) is preferred — it converges both surfaces on the cell model and lets the segment field stay Editor-only legacy. Migration is already idempotent (`AppendWallRun` overwrites by coordinate), so it is safe to run on every load.

### MINOR (polish — do not block live smoke)

**5. `worldPos == Vector3.zero` sentinel in composer.** (owner: cx-code) — `RoomDataComposer.cs:151` (tiles) and `:206` (props): `position = cell.worldPos == Vector3.zero ? grid.GetCellCenterWorld(cell.cell) : cell.worldPos`. Harmless today (walls recompute via `FootPosition`; cell coord is source of truth for floor/props), but a record legitimately at exact world-origin AND off its cell center would misplace. Recommend always using `grid.GetCellCenterWorld(cell.cell)` for cell-based records and dropping the sentinel.

**6. Rotation pivot for off-center variant art.** (owner: cx-code / art) — `WallRunBuilder.PlaceOne` (`:133-134`) rotates the GameObject about its foot origin AFTER positioning. Fine for square/grey-box and straight (bottom-center pivot), but future Corner/T/Cross variant ART with off-center pivots will visually offset when spun 90/180/270. Document that variant sprites must use bottom-center pivots, or switch to per-shape pivot-aware placement before wiring real corner/T art.

**7. `wallSegments` never cleared after migration.** (owner: cx-code) — `RoomDataMutator.MigrateSegmentsToCells` folds segments into cells but never clears the segment list, so legacy data lingers in the asset forever. Idempotent (no duplication), harmless, just stale. Optional: clear `wallSegments` post-migration once fix 4 guarantees cells are authoritative on both surfaces.

**8. Per-surface `ResolveWallPiece` divergence (visual only).** (owner: cx-code, defer) — Overlay resolves pieceId against the runtime registry/Resources (`InPlayMapPaintOverlay.cs:468`); Editor resolves against AssetDatabase (`RoomDataComposer.cs:336`). Same `pieceId` can map to different/absent sprites between surfaces. No stored-data divergence (occupancy/shape/rotation are identical), purely which sprite draws. Acceptable for placeholder phase; unify the piece registry before shipping real art.

---

## Confirmed CLEAN (no action)
- Struct writeback in `WangRebuild.ReorientWallCells` (`:50-54`) — modified `WallCell` value-type copy reassigned back into the list. Correct.
- Neighbor rebuild set (`WangRebuild:33-40`) — dirty cell + its exactly-4 neighbors, occupancy via the full `wallCells` index. No off-by-one.
- Preview/commit parity — `BuildRun` preview and committed cells both go through `WangResolver.Resolve4`; no divergence.
- All runtime files (WangResolver, WangRebuild, RoomDataMutator, RoomDataJson, RoomDataPaths, RoomData, RoomPlacementTypes) — zero `UnityEditor`/`AssetDatabase`/`Undo` refs; DEVELOPMENT_BUILD-safe. Overlay's three `UnityEditor.*` clusters all correctly `#if UNITY_EDITOR`-guarded inside the outer `#if UNITY_EDITOR || DEVELOPMENT_BUILD`. Preprocessor pairs balanced.
- Variant-slot sprite fallback (`WallRunBuilder.SpriteForShape:84-104`) — every shape falls back to `straightSprite ?? sprite`; `PlaceOne` returns null rather than attaching a null-sprite renderer. No NullRef when variant art is absent.
- Y-sort (`WallRunBuilder.ApplySpriteRules:147-157`) — forces Entities layer / order 0 / `SpriteSortPoint.Pivot` on all child renderers; foot-dropped position matches Custom-Axis (0,1,0).
- STEP-0 cleanup — dead `RoomData.placements`/`PlacementRecord` gone; "paint into the void" footgun routed through the sink; F2 undo stores the exact tilemap per edit (no wrong-tilemap undo).
- OnRoomLoaded first-run load is NRE-safe (`LoadActiveRoomData:530` null branch creates a fresh RoomData).

---

## OVERALL VERDICT: **FIX-FIRST**

Two BLOCKING failures gate live smoke:
- **Fix 1** (roomId bridge) — without it the two surfaces silently edit different files and live rooms can clobber each other; the entire "shared RoomData" premise is unverified.
- **Fix 2** (CCW rotation table) — every End/Corner and the open-E/W/S T-junctions render rotated wrong; the Townscaper "pieces snap together correctly" feel is broken on screen.

Fixes 3 and 4 are also BLOCKING for persistence correctness (F2-new rooms invisible to Editor; segment-only rooms lose all walls in F2) but are smaller, self-contained code changes.

Recommended order: 2 (Opus locks CCW + cx applies, fastest visual unblock) -> 4 (migrate-on-read, prevents data loss during smoke) -> 1 (Opus id contract) -> 3 (asset-create, pairs with 1). Re-run the EditMode test asserting the CCW table after fix 2. Then run the interactive smoke: place a single End/Corner/T and confirm orientation faces neighbors; place in F2, open Editor window, confirm same walls appear (validates 1+3+4 end-to-end).

Report: `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\ROOMTOOL_VERIFY_REPORT_S6.md`
