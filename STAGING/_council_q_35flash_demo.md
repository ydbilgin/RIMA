# RIMA Demo Mimarisi — LEAN / SHIP-FAST lens (Gemini 3.5 Flash)

Read these files yourself (file tools, NOT inline):
- `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\_council_demo_brief.md` (FULL context + current systems + A/B fork + 6 sub-questions — READ FIRST)
- `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\PORTAL_PREVIEW_SYSTEM_SPEC_S6.md`

You are the LEAN / SHIP-FAST / over-engineering-critic advisor. The other advisor (deep-architecture) is likely to recommend the "correct" full data-driven rebuild (Path B: new DungeonGraph DAG + RoomRunDirector + replace MapFlowManager + BuildPreview mode + orb-travel). Your job is the OPPOSITE pressure: what is the LEANEST path to a demo that conveys the user's vision, and where is the architecture advice OVER-ENGINEERING for a demo?

Answer the brief's 6 sub-questions from a ruthless ship-fast lens. Specifically challenge:
- Is replacing the working `MapFlowManager` + scene loop (Path A, runtime-verified) really necessary for a DEMO, or is that a risky rewrite that could break a working loop? Could the demo ship on Path A with a thin graph/preview layer bolted on?
- Preview islands: is a full `IsoRoomBuilder.BuildPreview()` of real geometry overkill? Would baked thumbnail sprites (rendered once per template, shown as faded islands) be 10x less work and look identical at "far away, faded" distance?
- StS-DAG vs just a hardcoded 5-room typed sequence with 2-3 door choices per step — does the demo NEED a real DAG, or does a simple typed list + per-step branch suffice?
- Which of the 6 vision features can be FAKED convincingly for a demo (placeholder/static) vs actually built?
- What is the SINGLE smallest build that makes the user go "yes, this is the loop"?

Be ruthless about cutting scope. End with: the leanest demo scope + the ONE riskiest thing to avoid + a 1-day vs 3-day cut line. This feeds an Opus synthesis.
