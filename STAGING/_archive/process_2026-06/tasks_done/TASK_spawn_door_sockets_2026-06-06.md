# TASK: Socket-based player spawn + direction-correct exit doors

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

## Amaç
User playtest complaint: player start positions and door positions in rooms are not well-defined / wrong direction. Make both **explicit, data-driven, and direction-correct** across all 26 RoomTemplateSO templates and the runtime builder.

## Current state (verified findings, file:line)
- `RoomTemplateSO.playerSpawn` exists (RoomTemplateSO.cs:16; PlayerSpawnSocket = socketId/position/facing). `doorSockets` list exists (DoorSocket = socketId/position/direction/widthInTiles/isExit; DoorDirection enum in Assets/Scripts/Core/DoorTrigger.cs:8 — 0=North,1=South,2=East,3=West per asset data).
- `IsoRoomBuilder.BuildMarkers()` (IsoRoomBuilder.cs:551-589) creates spawn + door markers from sockets; `RoomRunDirector.EnsurePlayerAtSpawn()` (RoomRunDirector.cs:289-332) hard-teleports player to `builder.PlayerSpawnMarker.position`. No walkability validation.
- **`IsoRoomBuilder.BuildExitDoors()` (IsoRoomBuilder.cs:699-785) IGNORES authored doorSockets entirely** — heuristic row at 72% room height, all gates use gateNorthSprite, direction data never applied. Run director attaches triggers to the returned door GameObjects (keep that contract).
- Data: 15 `Assets/Data/Rooms/Generated/*.asset` + `Special/Chamber_CharSelect.asset` HAVE sockets; **10 `Assets/Data/Rooms/Library/*.asset` have NO playerSpawn/doorSockets at all**.
- Data bug example: `combat_large_donut_01` playerSpawn=(14,18) directly under north door socket (14,19) → player spawns on top of the exit gates.

## Canon constraints (locked — USER CONFIRMED 2026-06-06)
- **Exit doors are ALWAYS a Hades-style side-by-side row at the BACK (north) of the room.** Do NOT scatter doors to East/West edges even if E/W sockets exist. One row, 1-3 doors, evenly spaced, all facing the player.
- **Door COUNT comes from the run graph** (current node's child count — existing behavior, keep). **Door POSITIONS are fixed template slots** (anchor-centered row). **Door DESTINATIONS (room types) stay random per run** (existing graph behavior, keep). Only the positions become deterministic.
- **SOUTH EXIT FORBIDDEN** (south = open cliff front).
- Player spawn: **south side of walkable footprint, bottom-center, facing North** (Hades-style: enter from the open front, exits at the back). Must be walkable, ≥4 tiles from the door row.
- Iso room: walkable footprint = `template.IsWalkable()` (RoomTemplateSO.cs:40-54).

## Work items
1. **IsoRoomBuilder.BuildExitDoors — anchor-based row placement.**
   - Use the authored NORTH exit socket (`isExit && direction == North`; first one) as the **row anchor**: the row of `doorTypes.Count` gates is centered on `grid.GetCellCenterWorld(anchorSocket.position)` + existing gateTuck offset, spaced by gateRowSpacing (keep the existing clamp-to-usable-width logic, measured around the anchor row's Y instead of the 72% lerp).
   - Keep rune child + sorting logic + returned-list contract unchanged. gateNorthSprite for all gates (row is always north-facing).
   - E/W exit sockets in data are IGNORED for runtime exits (markers may still show them in editor).
   - Fallback: if no North exit socket → keep current 72% heuristic row and log a single warning naming the template.
2. **RoomRunDirector.EnsurePlayerAtSpawn — validation + fallback.**
   - If spawn marker missing OR socket position not walkable → fallback to bottom-center walkable cell (min-y walkable cell nearest footprint center-x), warn once.
3. **Editor audit/auto-fix tool** (new, `Assets/Editor/...`, menu `RIMA/Rooms/QC/Audit Sockets` + `RIMA/Rooms/QC/Fix Sockets`):
   - Scan all RoomTemplateSO under `Assets/Data/Rooms/` (Generated + Library + Special; skip non-template assets).
   - Audit report: missing playerSpawn, spawn not walkable, spawn <4 tiles from exit socket, missing exit sockets, socket not on walkable edge cell matching its direction (cell walkable AND neighbor in `direction` not walkable), South exits.
   - Fix: auto-author playerSpawn (rule above, facing=North) and exit sockets — 1 North (top walkable edge, center-x) + East + West where a walkable edge cell exists in that direction; widthInTiles=2; deterministic socketIds (`door_N_01` etc.). Repair existing bad spawns (e.g. donut). Do NOT touch Chamber_CharSelect.asset (special, hand-authored). Use Undo/SetDirty/SaveAssets properly.
4. **RoomTemplateValidator** (RoomTemplateValidator.cs): add checks — spawn walkable, no South exit sockets, each door socket on walkable cell with non-walkable neighbor in its direction. Keep existing checks.
5. **Run Fix Sockets on all templates**, then verify:
   - `RIMA/Rooms/QC/Smoke Test All Templates` → must stay 26/26, 0 exceptions.
   - EditMode tests (UnifiedDesignerTests etc.) green; add/extend a small EditMode test asserting: every template (except Chamber) has walkable spawn + ≥1 North exit + no South exits.
   - Play-probe `_Arena` flow: player appears at south-bottom of first room, exit doors at socket positions (not the old 72% row).
6. Commit (identity ydbilgin, English message, NO Co-Authored-By), e.g. `fix(rooms): socket-driven exit doors + validated south spawn across all templates`.

## Notes
- Unity is open; UnityMCP available. Check console for compile errors after each script change.
- ⚠️ Working tree has unrelated changes (`Assets/TextMesh Pro/...`, `Assets/_Recovery/*`) — do NOT commit or revert those.
- Surgical: only IsoRoomBuilder.cs, RoomRunDirector.cs, RoomTemplateValidator.cs, new editor tool, test file, and the room .asset data files.
