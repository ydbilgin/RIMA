ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.

NLM ACCESS: query NLM if needed: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory.

# Amaç
ROUND 2 — deeper PRODUCTION/IMPLEMENTATION recipe consult. No code changes; read the actual files and give concrete Unity URP 2D build recipes with effort (S/M/L). Read by Opus, synthesized with a parallel Gemini consult. Round-1 already established feasibility & hook points (Portal.cs, PortalSpawnController, Systems/Map/RoomLoader.cs = live spine, RoomTransitionFX, DungeonGraph.RevealAhead/exits, two MapFragment classes). DO NOT repeat round-1; go DEEPER on the HOW.

# LOCKED DECISIONS (don't re-litigate; build the recipe around these)
- Doors -> PORTALS: center = color + floating diegetic rune-icon (combat/elite/rest), count = DungeonGraph.CurrentNode.exits.
- Travel = morph player into a glowing ORB (portal color), zip across the void, crash-land, illuminate target island. Target < 0.8s, camera sweeps ahead. (Dead Cells-style.)
- Next rooms shown DIEGETICALLY as REAL iso room-islands floating in the void, but DARK / STATIC / UNLIT / MOB-LESS; they illuminate + scale up only when selected.
- Reveal depth = map-fragment level: basic next-step preview ALWAYS FREE; map-fragment repurposed as "scout further / reveal more previews / reveal contents" via DungeonGraph.RevealAhead(steps).
- Rooms are iso diamonds (not square).

# QUESTIONS (per-Q: concrete recipe + components/scripts + effort S/M/L + key risk)
Q1. ORB-TRAVEL build recipe: exact GameObjects/components. Separate orb GO (SpriteRenderer + Light2D + TrailRenderer?) vs particle. How to hide the player visual root (multiple renderers/weapon/VFX children) + disable PlayerController safely. Tween path across the void (coroutine vs DOTween — is DOTween in the project?). Camera retarget: set CameraFollow.target=orb, restore to player after spawn. Minimal new script surface. Which beats are sprite-anim vs TrailRenderer vs particle vs camera-tween.
Q2. DIEGETIC PREVIEW render recipe — compare for "2-3 real next-room islands floating dark in the void": (A) instantiate a LOW-DETAIL static preview prefab of the room beside the island; (B) live RenderTexture mini-camera; (C) pre-rendered thumbnail quad. Exact Unity setup + perf budget (draw calls, extra cameras) for each. Recommend demo vs final. How portal i binds to preview i (shared DoorDirection/targetNodeId).
Q3. MOB-LESS preview: how to instantiate a room's VISUAL layout WITHOUT running its enemy spawners/combat logic. Is there a preview/disabled flag path in RoomLoader/RoomSequenceData, or do we need a lightweight "preview prefab" variant? Cleanest hook in EXISTING code (name the file+method).
Q4. REVEAL-MOMENT sequencing: room-cleared -> portals activate -> camera pulls back -> preview islands fade-in. Give the concrete coroutine sequence reusing RoomTransitionFX / CameraFollow / PortalSpawnController / Gate. Where does it slot into Systems/Map/RoomLoader's clear->fragment->draft->gate flow?

# OUTPUT -> CODEX_DONE.md
Per-Q recipe as above, then a final "MINIMAL PRODUCTION RECIPE FOR DEMO" paragraph (what to build first, in order) + the single biggest technical decision for the user. Concise.
