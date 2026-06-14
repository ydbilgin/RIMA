ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
This is a DESIGN + TECHNICAL FEASIBILITY consult. DO NOT write/modify any game code. Read the relevant files in THIS repo and answer the questions below with code-level hook points and effort estimates (S/M/L). Your answer is read by Opus, who synthesizes with a parallel Gemini consult and the user. Be concrete and grounded in the actual code.

# CONTEXT (RIMA — iso roguelite, LOCKED direction)
- Perspective = ISOMETRIC floating-island rooms. Top-down rejected. Connected-wall Wang tessellation ABANDONED (we can't make walls connect). Rooms = iso diamond floor + cliff edges floating in void + free-placed objects. NO connected walls.
- Run = StS2-style branching node graph: player travels room -> room, picks ONE path at each fork.

# EXISTING CODE (build on it — do NOT reinvent; READ these)
- `Assets/Scripts/Environment/Portal.cs` — placeholder portal: SpriteRenderer (cyan square) + CircleCollider2D trigger, `DestinationType` enum {Combat,Treasure,Ritual,BossApproach,Bridge}, fires `OnEntered(Portal)` on player trigger. Has TODO: "hook to scene transition / NextRoomLoader."
- `Assets/Scripts/Core/RoomTransitionFX.cs` — singleton screen-fade: `DoTransition(onBlack)` = fade-to-black -> run callback while black -> fade-in.
- `Assets/Scripts/Core/DungeonGraph.cs` — RoomNode {roomType, depth, lane, exits(dir->id), visited, revealed}. `Navigate(dir, out next)` moves current node. `RevealAhead(steps)` reveals N nodes ahead — CALLED BY MapFragment pickup. Default reveals 1 node ahead.
- `Assets/Scripts/Environment/MapFragment.cs` + `MapFragmentSpawner.cs` + `MapFragmentBridge.cs` — pickup -> DungeonGraph.RevealAhead -> reveals more map.
- `Assets/Scripts/Systems/Map/RoomLoader.cs` — live room-loading spine. Also `Assets/Scripts/Environment/PortalSpawnController.cs`, `Assets/Scripts/Core/DoorTrigger.cs`. (Investigate which transition path is actually LIVE — there are duplicate RoomLoader/MapFragment files; flag the live one.)

# USER'S NEW DESIGN PROPOSALS TO EVALUATE
1. DOORS -> PORTALS: a portal is a radially-symmetric rift = ONE sprite + a center VFX effect (animated cyan/colored swirl), color-coded per destination. Avoids 8-directional door art. Each room shows N portals = N branch choices from current DungeonGraph node exits.
2. PORTAL TRAVEL = MORPH + TELEPORT: entering a portal, the character morphs into a glowing ORB in the PORTAL'S COLOR, is pulled in, teleported to the chosen next room; camera follows into the new room. (Replaces/augments the plain black fade.)
3. MAP-FRAGMENT mechanic: remove, keep, or repurpose? Currently it reveals more abstract-map nodes ahead.
4. REPLACE the abstract overhead map with REAL ROOM PREVIEWS: user does NOT want a full node-map showing everything. Instead show the actual upcoming room(s) as REAL room views (mini renders of the genuine next rooms) SIDE BY SIDE — player sees only as far as their reveal allows (1 or a few next rooms). The "choice" = looking at real next rooms, possibly diegetic (next rooms visible in the void beside the current island, like concept_room_3door / concept_room_transition).

# QUESTIONS (answer each: VERDICT [feasible/risky/no] + 2-4 line code-grounded recommendation + key risk + effort S/M/L)
Q1. Portal-as-rift (1 sprite + center VFX, color-coded): how to extend Portal.cs cleanly? URP 2D: animated sprite vs Shader Graph vs particle for the center swirl — which is least-effort, on-brand (cyan #00FFCC sparing), pixel-friendly?
Q2. Morph-into-orb + teleport + camera-follow: concrete path. Sprite-swap to orb vs separate orb GO + hide player; disable PlayerController during travel; integrate with RoomTransitionFX (fade) or replace it; how camera follow re-targets in the new room. Effort?
Q3. Map-fragment: recommend remove/keep/repurpose. If map becomes "real room previews of next nodes," what should map-fragment DO instead (reveal MORE previews / see further ahead / reveal hidden room CONTENTS like elite/treasure)? Tie to DungeonGraph.revealed + RevealAhead.
Q4. Real-room-preview-instead-of-overhead-map: implementation options + trade-offs for a DEMO: (a) live RenderTexture mini-cameras rendering pre-built next-room instances, (b) pre-rendered thumbnails, (c) fully diegetic in-world previews floating in the void next to the island. Which is demo-realistic? How does each bind to DungeonGraph node exits + Portal selection (portal i shows/leads to preview i)?

# OUTPUT
Write your answer to CODEX_DONE.md. Per-question verdicts as above, then a final one-paragraph "RECOMMENDED OVERALL DIRECTION" + the single biggest decision you'd put to the user. Concise. No code changes.
