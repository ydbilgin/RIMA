# COUNCIL — DEEP ARCHITECTURE LENS (Gemini 3.1 Pro): Authored gate slots

READ: `STAGING/PORTAL_PACK_DECISION_2026-06-06.md` (your round-2 "3 discrete slots" advice was deferred there) + `STAGING/MASTER_PLAN_FINAL_2026-06-06.md` (T3 portal task) + `CURRENT_STATUS.md` top block.

NEW INPUT: The USER now explicitly wants authored slots: ENTRY defined + 3 EXIT slots per template in Map Designer; runtime renders 1/2/3 doors at those slots based on the run graph ("statement'ları iyi ayarla" — the selection logic must be clean). Your earlier recommendation is effectively accepted by the user; now design it RIGHT.

Current shipped facts: RoomTemplateSO.doorSockets (DoorSocket: socketId/position/direction/isExit), north-anchor centered row in IsoRoomBuilder.BuildExitDoors (spacing+clamp+fallback), RoomSocketQCTool auto-fix, 26 templates already carry door_N + side door_W/door_E sockets (W/E are SIDE edges — NOT back-edge NW/NE; migration needed), validator forbids South exits, smoke test 26/26.

Questions (~900 words max):
1. **Schema:** cleanest representation of ordered back-edge slots — naming convention on existing DoorSocket list vs explicit ExitSlot enum field vs separate exitSlots[3] array? Consider: validator rules, QC auto-fix, designer serialization, backward compat with 26 templates, and that T3 will attach PortalSkin per door.
2. **Selection statements:** specify exact deterministic logic: door count 1 → which slot; 2 → which pair; 3 → all. Justify symmetric (NW+NE) vs adjacent (N+NE) for count=2. Also: graph children → slot assignment order (left-to-right by child index? by room type weight?). Edge: what if a template has only 2 valid slots (narrow back edge)?
3. **Broken back edges (donut/crescent/zigzag):** slot auto-placement policy per walkable back-edge SEGMENT; fallback when <3 valid positions. Should slot count be a per-template capability (maxExits: 2 for narrow rooms) that the GRAPH respects when assigning children (i.e., graph child count clamped by room's slot capacity)? Architectural implications of that coupling (graph generated before room selection? order of operations in RoomRunDirector/DungeonGraph).
4. **Designer integration:** schematic-preview slot editing (click/drag in Rooms tab) vs SceneView handles vs Inspector-only. Which fits the existing Rooms tab architecture (template list + 2D preview + build buttons) with least new surface?
5. **Sequencing:** slots before T3 portal prefab (portal lands on slots) or merged? Migration plan for 26 templates (extend Fix Sockets) — risks?
Output: schema recommendation + exact selection pseudo-statements + slot-capacity/graph-coupling verdict + table ACCEPT/SIMPLIFY/REJECT for designer UX options.
