# COUNCIL R4 — DEEP ARCHITECTURE LENS (Gemini 3.1 Pro): UI↔JSON room editing + autonomous chain

READ: `STAGING/MASTER_PLAN_FINAL_2026-06-06.md` + `STAGING/GATESLOT_DECISION_2026-06-07.md` + `CURRENT_STATUS.md` top block.

## TOPIC 1 — UI↔JSON two-way room editing (USER REQUEST)
User wants: when he changes anything in a room via the Map Designer UI (player spawn position, exit gate slots, props/objects add-remove), it should be EASY in the UI and IMMEDIATELY persisted to JSON. JSON schema should be well-designed: start position, exit positions, objects — all easily addable/removable. Context: rooms were originally imported from ChatGPT-generated JSON via a one-way RoomJsonImporter → RoomTemplateSO; runtime builds from SO; Map Designer Rooms tab edits/saves SO assets. The user keeps generating new rooms via LLM (schema must stay promptable).

Architecture questions:
1. **Source of truth:** (a) SO canonical + deterministic auto-export to JSON on every save (JSON = always-fresh mirror + LLM seed), (b) JSON canonical + SO is a build artifact (reimport on change/file-watch), (c) dual with hash-based drift detection. Pick one for a solo dev + LLM workflow; justify. Failure modes of each (merge conflicts, Unity asset DB races, lost edits).
2. **Schema v2 design:** propose the JSON shape (concise example): meta(id/type/size), walkable representation (ASCII grid rows — human+LLM friendly — vs cell list), playerSpawn, exitSlots (NW/N/NE per GATESLOT convention), props[{id, cell, flip?}], optional decals/visualTheme. Versioning field + forward-compat rules. Must round-trip losslessly with RoomTemplateSO.
3. **"Immediately" semantics:** per-edit autosave (file churn + asset DB thrash risk in Unity) vs debounced autosave vs explicit Save writing BOTH SO+JSON atomically. Recommend with Unity-specific risks (Undo integration, SerializedObject dirty state, source-control noise).
4. **Edit operations** the Rooms tab needs (priority order): move spawn / move exit slots / prop add-remove from a palette / walkable cell paint? Which need new editor code vs which can reuse existing (Auto Props, schematic preview, RoomTemplateSOInspector)?

## TOPIC 2 — Autonomous chain protocol
Tonight the orchestrator runs MASTER_PLAN tasks autonomously (2 cx lanes possible + ax fallback + Claude subagents). Advise architecture of the chain: max parallel mutation lanes touching Unity project (1 code + 1 asset safe? two code lanes = scene/asset collision risk — history: cx scene-restore wiped another agent's changes), verification gate per task type (data/editor/runtime), commit-per-task vs batch, and a "post-DONE checklist" to catch cx silent-fails (DONE timestamp, git log, compile state). Short protocol, max 10 rules.

Output: Topic1 = recommendation + schema example + ops table; Topic2 = numbered protocol. ~1000 words max.
