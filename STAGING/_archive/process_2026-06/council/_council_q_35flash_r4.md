# COUNCIL R4 — LEAN LENS (Gemini 3.5 Flash): UI↔JSON room editing + chain + door angle

READ: `STAGING/MASTER_PLAN_FINAL_2026-06-06.md` (T1-T9) + `STAGING/GATESLOT_DECISION_2026-06-07.md`.

Facts (verified): RoomJsonImporter is ONE-WAY (STAGING/chatgpt_rooms.json → RoomTemplateSO; ASCII grid schema with `.`=walkable, P/e/B/C markers, doors N/E/W). RoomTemplateSO already serializes walkableGrid bool[] + props List<PropPlacementData>{guid, tile, rotationSteps, flipX} + door sockets + playerSpawn. Map Designer Rooms tab has 2D schematic preview + Save Assets (AssetDatabase.SaveAssets only). NO export-to-JSON exists. Iso rooms render as diamonds: visual back boundary = NW+NE DIAGONAL edges; current doors all use frontal gate_north sprite (user says they look BAD); an UNUSED gate_west.png side-perspective sprite exists.

Questions (~700 words, bullets, disagree freely):
1. **JSON feature, leanest cut:** user wants UI edits (spawn/exit slots/props) instantly persisted to JSON. Cheapest correct version: (a) full two-way (JSON canonical + ScriptedImporter + file watch), (b) SO canonical + one-click "Export JSON" on save (deterministic writer mirroring importer schema), (c) export-on-save automatic. Pick + hour estimate. Is a ScriptedImporter overkill vs the existing menu importer?
2. **Which edit ops ship FIRST** for a solo dev pre-jury: move spawn / move 3 exit slots / prop add-remove palette / walkable paint? Give the minimum set + the cheapest UI for each (Inspector fields? schematic click? SceneView handle?). Hours.
3. **Door angle, cheap trick:** back edges are diagonal; do we NEED angled NW/NE portal art, or does frontal-with-correct-position read fine at jury distance (Hades uses angled doors)? If angled needed: 1 angled sprite + flipX mirror enough? Could existing gate_west.png style guide the imagegen prompt? Verdict + cost.
4. **Chain parallelism:** 3.1-Pro says STRICT 1 Unity-mutator lane (domain reload races). Tonight we ran 2 cx lanes (1 code in Unity + 1 imagegen writing only to STAGING) — was that actually safe? Define the practical rule (what counts as "outside Unity's watched folders").
5. **Time bombs** in the JSON feature (Undo, GUID stability of props across export/import round-trip, walkable ASCII vs bool[] mismatch, asset re-import loops).
