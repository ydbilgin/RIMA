ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
USER REQUEST feasibility: In Map Designer (RIMA/Map Designer, Rooms tab), the user wants per-template AUTHORED gate slots: 1 explicit ENTRY marker + 3 explicit EXIT slots (left/center/right on back edge), placed "logically" by the designer/tooling. At runtime the graph decides 1, 2, or 3 doors and renders them AT THOSE AUTHORED SLOTS (selection logic: which slots for 1 door? for 2?). This replaces/extends the current north-anchor+spacing row. ANALYSIS ONLY → CODEX_DONE.md.

# Context (shipped today, commit 20d1f09c)
- RoomTemplateSO.doorSockets (DoorSocket: socketId/position/direction/widthInTiles/isExit), playerSpawn.
- IsoRoomBuilder.BuildExitDoors: row CENTERED on first North exit socket, gateRowSpacing, clamp to usable width; fallback 72% heuristic.
- RoomSocketQCTool: RIMA/Rooms/QC/Audit Sockets + Fix Sockets (auto-authors N/E/W sockets + spawn).
- Map Designer Rooms tab (268848ce): template list + 2D schematic preview + Build in Arena + Auto Props. Phase-2 backlog already mentions "socket handle" editing.

# Questions (answer with file:line evidence)
1. **Data model delta:** smallest change to represent 3 ordered exit slots? Options: (a) keep DoorSocket list + convention socketIds door_NW/door_N/door_NE + an ordering rule; (b) add explicit enum field (ExitSlot { NW, N, NE }) to DoorSocket; (c) new array field RoomTemplateSO.exitSlots[3]. Which is least invasive given validator + QC tool + existing 26 templates' data (they currently have door_N/door_W/door_E style sockets — note W/E are SIDE sockets, NOT back-edge NW/NE: data migration needed!).
2. **Runtime selection logic:** BuildExitDoors change size to: 1 door→N slot, 2 doors→NW+NE, 3→all (or 2→N+NE? note what reads best with current row code). Keep returned-list contract. Estimate S/M.
3. **Map Designer authoring UX:** what exists in the Rooms tab 2D schematic preview code (file) that could support click-to-place/drag slot markers? Size to add: render slot positions on the schematic + click-to-move + validation overlay (slot must be walkable + on/near back edge). Alternative cheaper path: rely on Fix Sockets auto-placement + Inspector nudge only?
4. **Auto-placement logic:** extend RoomSocketQCTool to compute NW/N/NE slots automatically per template (back-edge walkable cells: leftmost-third / center / rightmost-third)? Edge cases: crescent/zigzag/donut where back edge is broken — what rule (per walkable back-edge segment)?
5. **Entry marker:** playerSpawn already exists = the "entry gate belli olacak" requirement? Or does user need a visible entry POINT marker in designer separate from spawn? (probably same thing — confirm what designer preview currently shows for spawn.)
6. **Migration:** 26 templates have today's socket data (N anchor + side W/E sockets). Cost to re-author all to NW/N/NE via extended Fix Sockets + re-run smoke 26/26?
Output: per-question answer + total size estimate (S/M/L) + recommended least-invasive path. No code changes.
