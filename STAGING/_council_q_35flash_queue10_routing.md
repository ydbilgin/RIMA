# Council question — LEAN / SHIP-FAST lens (Gemini 3.5 Flash High)

You are one of three advisors. Your lens: leanest path + over-engineering critique. Another advisor covers code-feasibility, another covers deep architecture — do NOT cover those; be the pragmatist.

## Context
RIMA: Unity 6 URP 2D top-down roguelite. Playable loop just closed. Autonomous session executes a 10-task queue tonight. User playtest happens AFTER — so the queue must leave the game stable and playable.

Read these files (you have file tools):
- "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/CODEANIM_DECISION_2026-06-05.md"
- "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/MODULAR_PROPS_DECISION_2026-06-05.md"

## The 10 tasks
1. [M] Knockdown package code-only (parabola+tilt+squash+bounce+shadow+i-frame; merge 2 knockback impls + 3 profile SOs).
2. [S] Mob death decal/ghost.
3. [S] Anchor-tuning editor tool (weapon hand anchors).
4. [S] Wire existing TooltipSystem into draft cards (hover).
5. [M] ESC SkillCodexUI MVP.
6. [S] Register remaining 6 classes in SkillDatabase.
7. [S] Skill-icon import settings fix (Bilinear→Point, PPU64, uncompressed).
8. [M] Checker ground + run existing Poisson auto-placer over 15 rooms.
9. Three small mechanics: Dynamic-Wave / Card-Weight / Echo-Mote-Heal.
10. Clutter cleanup.

Agent fleet: cx (Codex, proven), ax-Opus-4.6 (NEW untested channel, code+UnityMCP), ax-Flash (you — fast S-tasks), Sonnet-MCP (mechanical Unity ops).

## Your questions
1. MINIMUM viable version of each task — what would you cut from each spec and still call it done? (e.g. knockdown: is bounce needed v1? codex: is blur+desat needed v1?)
2. Which tasks are over-spec'd or premature for a pre-playtest build? Which give the most visible value to the user's NEXT playtest per hour spent?
3. Lean ordering: if the session dies halfway, which order maximizes banked value?
4. Which tasks are safe one-shot Flash jobs (S, isolated) vs which genuinely need the heavy code channel?
5. Stability guard: which task is most likely to break the playable loop, and what's the cheapest smoke-test gate after it?

Numbered answers + a final priority-ordered list (task → lean version → agent → est. effort).
