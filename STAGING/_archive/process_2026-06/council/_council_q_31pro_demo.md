# RIMA Demo Mimarisi — DEEP ARCHITECTURE lens (Gemini 3.1 Pro)

Read these files (read them yourself via your file tools, do NOT expect inline):
- `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\_council_demo_brief.md` (FULL context + current systems + the A/B fork + 6 sub-questions — READ FIRST)
- `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\PORTAL_PREVIEW_SYSTEM_SPEC_S6.md` (preview-island spec)
- `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\ROOM_SYSTEM_DECISION_2026-06-03.md` (RoomRunDirector plan, Part 3)

You are the DEEP ARCHITECTURE advisor. Answer the brief's 6 sub-questions from a long-term-architecture + correctness + system-cohesion lens. Your special focus:
- **Fork A vs B:** Which architecture is RIGHT long-term, and does the demo's preview-island requirement (rendering the NEXT rooms' data as small islands in the void) effectively FORCE Path B (data-driven IsoRoomBuilder), since Path A scenes have no queryable next-room data? Or is a hybrid viable (keep scene loop, add a graph layer)?
- **Preview islands:** What is the architecturally-clean way to render "the run's upcoming rooms as faded mini-islands below the cliffs, above the background, far away"? How does this bind to a run-graph (node-id → RoomTemplateSO)? Reconcile with the spec's "RoomLoader must be graph-aware" blocker.
- **Typed procedural path:** Design the run-graph data structure (StS-style DAG vs linear-typed). How do typed doors map to graph edges, and how does choosing a door advance the run?
- **Sequence:** Give the dependency-ordered build sequence (what must exist before what).

Be concrete and decisive. End with a recommended Path (A/B/hybrid) + a phase-ordered build plan. Keep it focused — this feeds an Opus synthesis, not a human.
