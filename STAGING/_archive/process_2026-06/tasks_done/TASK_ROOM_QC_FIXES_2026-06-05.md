# TASK: Room QC fixes — off-island props + interior cliff seam + tiny Treasure room

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

## Amaç
A visual QC pass over all 26 RoomTemplateSO assets (report: `STAGING/ROOM_QC_REPORT_2026-06-05.md`, screenshots: `Assets/Screenshots/RoomQC/`) found 2 FAIL + several systemic issues. Fix them so every room builds visually clean. User has a presentation soon — prioritize correctness over polish.

## Issues to fix (in priority order)

### 1. Off-island props (affects 6 rooms — likely ONE root cause)
Rooms: `combat_large_diamond_01` (worst, 31 props many floating in void), `combat_large_donut_01` (altar cluster below island), `chest_large_reliquary_diamond_01`, `combat_large_hourglass_01`, `combat_large_lshape_01`, `elite_large_trident_01`.
- Props were baked into the RoomTemplateSO assets by an auto-populate pass (commit `e6502fd9`, BridsonPoissonAutoPlacer + CompositionRoleMap). Investigate whether the bug is (a) bad baked prop cells in the template assets, or (b) IsoRoomBuilder placing valid cells at wrong world positions.
- If (a): write/extend an editor utility to PRUNE or RE-SEED template props so every prop cell is inside the template's floor cell set (and not on a hole). Re-bake the 6 affected templates. Do NOT touch templates that QC marked OK.
- If (b): fix the builder's prop placement math.
- Also enforce for the future: the auto-placer must reject cells outside the floor set.

### 2. combatlarge_twin_basins_01 — cliff wall THROUGH the floor interior (FAIL)
The cliff auto-placement fires on the interior seam between the two basins, building a cliff wall across playable floor. Fix the cliff solve so cliffs only spawn on floor cells truly adjacent to void (all-floor neighborhoods must not fire). Check whether the bug is in the template data (a 1-cell gap between basins?) or in the cliff solver. If the template has an unintended 1-cell void seam, prefer fixing the template data (fill the seam or widen it into a real channel) over changing solver logic that works for 25 other rooms.

### 3. Treasure_01 — too small (14 floor tiles)
Enlarge to roughly Shrine_01 scale (~40 floor cells), keep its identity (small intimate vault is fine, 14 is unusable). Keep door edges valid (no south doors — canon: doors only N/E/W).

### 4. (Light touch, only if cheap) combat_large_cross_01 2 misaligned props at top arm + chest_large_reliquary bottom-edge cluster — covered by fix #1 if root cause is shared.

## Verification (MANDATORY before commit)
- Unity Editor is OPEN — use UnityMCP. Rebuild each FIXED room in `_Arena` (edit mode: open Assets/Scenes/_Arena.unity, FindFirstObjectByType<IsoRoomBuilder>, builder.Build(template)) and confirm: no prop transform outside the floor bounds (check prop world positions against floor tilemap cell bounds programmatically — don't rely on eyeballing), twin_basins has no interior cliff children, Treasure_01 floor count ≥ 35.
- Console: 0 errors after rebuilds.
- Do NOT save _Arena scene. Do NOT enter Play mode.

## Commit
After verification passes: `git add` ONLY the files you changed (template .asset files, any editor utility, any builder fix) and commit with a conventional message (e.g. `fix(rooms): prune off-island props + twin-basins interior cliff seam + enlarge Treasure_01`). Identity = ydbilgin, NO Co-Authored-By trailer.

## Output
Write results to CODEX_DONE (standard). Include: root cause for #1 (a or b), per-room before/after prop counts, twin_basins diagnosis, Treasure_01 new floor count, commit hash.
