# Council Question — PLAYABLE ROADMAP ordering (DEEP DEPENDENCY-ANALYSIS lens)

You are ONE advisor in a RIMA council (others: Gemini 3.5 Flash lean, Opus design+code-audit). YOUR LENS = DEEP DEPENDENCY/CRITICAL-PATH ANALYSIS (production planning).

READ FIRST (file tools):
1. `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\_council_brief_playable_roadmap.md` — full inventory + 4 questions + "playable" definition.
2. `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\CODEANIM_DECISION_2026-06-05.md` — today's anim decision (zero produced anims for demo).
3. `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\_nlm_weapons_anim.json` — weapon canon + locked architecture.

ANSWER the brief's 4 questions. Emphasis for YOUR lens:
- Build the dependency graph explicitly: which items BLOCK which (e.g., weapon sprites block hand-fitting; does B-11 combat lifecycle block weapon feel-testing or vice versa? does knockdown depend on B-11?).
- Critical path to the defined playable loop; what is genuinely on it vs what only LOOKS urgent.
- Parallelization plan: user-gated PixelLab sessions (weapons, later mobs/icons) vs autonomous code lanes (cx/Flash/Sonnet) — design 2-3 concrete "sessions" with deliverables.
- Weapon production session design: pilot-2 (which two classes give max architecture-risk coverage? e.g., one melee greatsword + one dual/floating weapon) vs all-10; twin weapons single-vs-double sprite trade-off; how anchor-fitting chains into production same-session.
- Rework risk: weapons before vs after B-11; knockdown timing.

OUTPUT to STDOUT: ordered roadmap table (phase -> items -> size -> owner -> depends-on) + critical path + parallel lanes + top risks. English OK, concrete.