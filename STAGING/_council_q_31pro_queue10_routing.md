# Council question — DEEP ARCHITECTURE lens (Gemini 3.1 Pro High)

You are one of three advisors. Your lens: deep architecture / system-design / sequencing risk. Another advisor covers code-feasibility, another covers lean/ship-fast — do NOT cover those angles, go deep on design.

## Context
RIMA: Unity 6 URP 2D top-down roguelite (pixel art, PPU 64, D3D11). Playable loop just closed (MainMenu→Chamber select→run→combat→reward→doors→boss→victory/death). An autonomous session will now execute a 10-task queue. Decision docs already lock the WHAT; we need consensus on HOW + agent routing + ordering.

Read these files (you have file tools):
- "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/CODEANIM_DECISION_2026-06-05.md"
- "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/SODAMAN_LEARNINGS_DECISION_2026-06-04.md"
- "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/MODULAR_PROPS_DECISION_2026-06-05.md"

## The 10 tasks
1. [M] Knockdown package code-only (parabola + 35° tilt + Y-squash 0.6 + bounce + ground shadow + get-up i-frame; merge 2 knockback impls + HitImpulse + 3 KnockdownProfile SOs).
2. [S] Mob death decal/ghost (code squash/fade + decal).
3. [S] Anchor-tuning editor tool (weapon hand anchors).
4. [S] Tier-1 hover: wire existing-but-unconnected TooltipSystem into skill draft cards + equipped-synergy pulse.
5. [M] ESC SkillCodexUI MVP (fullscreen pause-layer skill codex, 4 registered classes).
6. [S] Register remaining 6 classes in SkillDatabase.
7. [S] Skill-icon import settings fix (Bilinear→Point, PPU 100→64, uncompressed).
8. [M] Checker ground tint + run existing Poisson auto-placer over 15 prop-poor rooms.
9. Three small mechanics: Dynamic-Wave / Card-Weight / Echo-Mote-Heal.
10. Clutter cleanup (build settings + dead code; PlayableArena_Test01 referenced = untouchable).

## Agent fleet for routing
- cx = Codex gpt-5.5 (proven code channel; 2 parallel profiles OK)
- ax-Opus-4.6 = NEW: Claude Opus 4.6 via Antigravity CLI — code-capable + UnityMCP-connected (untested in this fleet; one ax process at a time)
- ax-Gemini-3.5-Flash = fast, good for S-isolated code/doc work
- Sonnet-MCP = Claude Sonnet subagent with UnityMCP (mechanical Unity ops, import settings, scene ops)

## Your questions (answer these, be specific)
1. Architecturally riskiest tasks of the 10 — which need a design pass before code, which are safe to fire-and-forget?
2. File-collision / system-collision analysis: knockdown (1), death-decal (2), and mechanics (9) all touch enemy/combat path — must they serialize on one agent, or can interfaces isolate them?
3. Ordering: optimal DAG (what blocks what; e.g. 6→5 SkillDatabase before codex?).
4. ax-Opus-4.6 onboarding: which task is the best LOW-RISK pilot to validate this new channel (good = isolated, observable, easy QC)? Which tasks should NOT go to an unproven channel?
5. Cross-review matrix: for each task, who reviews whom so writer≠reviewer and review skill matches task type?
6. Anything in the queue you'd cut or defer entirely from an architecture standpoint?

Be concrete. Output a numbered answer per question, then a one-table summary (task → approach note → agent → reviewer → order slot).
