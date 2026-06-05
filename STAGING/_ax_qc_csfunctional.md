# RIMA — CharacterSelect functional fixes — INDEPENDENT CODE QC (Gemini 3.1 Pro High)

Independent QC code-review. READ actual code (don't trust summary):
- F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\UI\CharacterSelectScreen.cs
The just-applied functional fixes: (1) class-carry bug — SelectClass now writes PlayerClassManager.SelectedClass immediately for unlocked classes + OnStartRun re-writes before SceneManager.LoadScene; (2) demo Echo seeded 200 (PlayerPrefs) + Echo chip; (3) functional unlock — locked CTA "KİLİDİ AÇ — {cost} Echo", if Echo>=cost spend + persist unlock + flip sprite to normal + selectable; (4) locked NOT selectable-to-play (locked click = preview/unlock only, does not set playable); (5) locked = near-black silhouette (#0A0510, alpha kept) → white on unlock.

Verify + find real bugs (focus on logic/edge cases in the economy):
1. **class-carry:** is PlayerClassManager.SelectedClass written for the selected unlocked class BOTH on SelectClass and before LoadScene? Could a default/Warblade still leak (e.g. initial auto-select, or locked-class select path)?
2. **Echo economy edge cases:** Echo exactly == cost (>= vs >)? double-spend (clicking unlock twice fast)? Echo persists correctly (PlayerPrefs Save)? negative Echo guarded? Hexer prerequisite (250 + Elementalist run) gated correctly — does it block unlock if prereq unmet even with enough Echo?
3. **unlock persistence:** unlocked flag persists (PlayerPrefs)? On re-entering CharSelect, already-unlocked stays unlocked + normal color + selectable? Does seeding "200 on first start" re-seed/overwrite a spent balance on every entry (BUG) or only once (guard with a 'seeded' flag)?
4. **locked not selectable:** can a locked class ever become PlayerClassManager.SelectedClass before unlock? SEÇ disabled/blocked for locked?
5. **silhouette:** color applied only to locked sprites; unlocked/just-unlocked = white; no leak to portrait?
6. **regressions:** SkillDatabase query, RefreshSkillList, backdrop, VFX-ring, layout untouched?
7. **Verdict:** PASS / PASS-WITH-NITS / FAIL + top 3 findings as `file:line — issue — fix`.

Cite real line numbers. Terse. The "re-seed Echo on every entry" bug (#3) is the most important to check.
