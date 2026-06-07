# TASK: Authored gate slots (T3.0) — NW/N/NE exit slots + deterministic selection

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

## Amaç
Implement the council-approved gate-slot system per `STAGING/GATESLOT_DECISION_2026-06-07.md` (READ IT FULLY FIRST — it is the spec; this task file only lists the work breakdown). Unity is OPEN (UnityMCP available).

## Spec summary (details in decision doc)
- ZERO schema change: socketId convention `door_NW_01 / door_N_01 / door_NE_01` (all direction=North, isExit=true) on existing `RoomTemplateSO.doorSockets` + a small slot-resolver helper.
- Selection: 1 door→N · 2→NW+NE (center EMPTY) · 3→all; assignment = graph child index left-to-right; returned-door-list order = choice index (RoomRunDirector trigger contract UNCHANGED — this is the one hard constraint).
- Fallback: valid slots < doorCount → existing anchor-row path + one warning naming the template. Never a door over void.
- Pool filter: RoomRunDirector template selection skips templates with ValidExitSlotCount < node.childCount.
- ENTRY = existing playerSpawn (no new field, no entry object).

## Work items
1. **Slot resolver** (small static helper or method on RoomTemplateSO — your call, min code): returns ordered valid slots [NW?, N?, NE?] by socketId convention.
2. **RoomSocketQCTool / Fix Sockets extension** (Assets/Scripts/Editor/Map/RoomSocketQCTool.cs): replace N/E/W authoring with NW/N/NE per the auto-placement rule in the decision doc (north-edge IsDoorEdge cells → x-sort → contiguous segment clustering → largest segment preferred → left-third/center/right-third picks, min separation ≥3, no silent fail — warn-list with template names). DELETE old door_W/door_E sockets on fix. Keep Chamber skip.
3. **Validator** (RoomTemplateValidator.cs + Audit Sockets): MUST rules 1-5 and WARN rules 6-8 from the decision doc.
4. **IsoRoomBuilder.BuildExitDoors**: consume slots via resolver with the selection mapping; remove centered-row spacing/clamp math for the slot path; keep the old row code ONLY as fallback. Keep rune/sorting/returned-list behavior otherwise unchanged.
5. **RoomRunDirector**: pool filter at template selection (ValidExitSlotCount >= childCount); keep everything else.
6. **Rooms tab preview** (Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs:376-392 area): distinct colors + labels for NW/N/NE slots and "ENTRY" label on playerSpawn marker. No click/drag editing (Phase-2). Optional if trivial: small legend line showing which slots light for 1/2/3 doors.
7. **Tests**: update RoomTemplateSocketTests (and IsoRoomBuilder tests if they assert row behavior) to the 3-slot rules: every non-Chamber template has ≥1 valid slot incl. N, no South exits, slots distinct + separated.
8. **Migration run + verify**: RIMA/Rooms/QC/Fix Sockets → Audit Sockets (expect 25/25, warn-list empty or report it) → Smoke Test All Templates (26/26, 0 exceptions) → EditMode tests green → _Arena play-probe: verify 1-door, 2-door, 3-door cases land on correct slots (2-door: center empty) in at least one rectangular + donut + crescent template; door walk-through advances to the correct node (choice-index contract).
9. **Commit** (identity ydbilgin, English, NO Co-Authored-By): e.g. `feat(rooms): authored NW/N/NE exit slots with deterministic door selection`. Do NOT commit unrelated files (TextMesh Pro asset, _Recovery, STAGING). Write result summary to CODEX_DONE.md (include warn-list templates if any).

## Constraints (hard)
- `BuildExitDoors` returned list order/length contract and RoomRunDirector trigger wiring must not change.
- Do not touch Chamber_CharSelect.asset.
- Working tree has unrelated changes — leave them.
- If a rule in this file conflicts with GATESLOT_DECISION_2026-06-07.md, the decision doc wins; if still unclear → BLOCKED note, don't guess.
