# RIMA — CharacterSelect v2 Layout Refinement — LEAN lens (Gemini 3.5 Flash)

READ first: F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\CHARSELECT_REFINE_BRIEF_2026-06-04.md

LEAN / over-engineering-critic. The roster-room already works; this is a polish pass (framed boxes + wider character spread + skills placement). Blunt bullets:
1. **Skills placement — cheapest good answer:** bottom-bar right zone (no new layout system, reuse current BuildSkillDetailPanel) vs vertical right side-panel (risks covering spread chars + more rework). Which is less work AND better? Decide.
2. **Framed boxes:** is this just "wrap existing zones in a 9-slice Image (Sliced) + cyan edge child"? Confirm it's a low-cost change, not a rebuild. Which Pack frame, minimal steps.
3. **Character spread:** is changing the normalized-coordinate table enough (no new system)? Give a simple wider spread (2 rows or arc) — argue against any fancy dynamic-layout over-engineering.
4. **Trap check:** what's the over-engineering risk in this polish pass (e.g., per-char dynamic layout, animated box reveals, skill scroll), and what to skip for now.
5. **Ship order** for the 3 tweaks in one cx pass.
Favor reuse + defer. Numbers where useful.
