# RIMA — CharacterSelect final polish + demo unlock — LEAN lens (Gemini 3.5 Flash)

READ first: F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\CHARSELECT_FINAL_BRIEF_2026-06-04.md

LEAN / over-engineering-critic. The screen is "very close"; this is the FINAL polish + a demo-only unlock. Blunt bullets:
1. **Demo Echo — cheapest correct:** simplest way to seed a starting Echo balance for a demo (a constant / PlayerPrefs default), and make ONE unlock work, WITHOUT building a real meta-economy. What's the minimum? Number for starting Echo.
2. **Unlock flow:** minimum viable — click locked → "KİLİDİ AÇ {cost}" enabled if Echo≥cost → spend + flip unlocked flag + re-tint sprite normal. Persist or session-only for a demo? Argue the simplest that survives the demo.
3. **Locked silhouette:** is it just `image.color = near-black (alpha kept)` and revert to white on unlock? Confirm it's one-line per state, no shader/mask.
4. **Bigger panels:** is this just bumping font sizes + panel widths + nudging char coords? Confirm no layout-engine needed. Don't over-tune.
5. **class-carry bug:** likely a one-line wiring fix (SelectClass must set PlayerClassManager.SelectedClass). Confirm it's small, not a refactor.
6. **Traps:** what to NOT build (persistent save system, Echo-earning loop, unlock animations beyond a flash, per-class custom silhouettes). The 20% that demos well.

Terse, practical, numbers. Favor the simplest demo-ready path.
