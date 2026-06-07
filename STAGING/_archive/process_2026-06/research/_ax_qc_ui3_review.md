# RIMA — UI Redesign QC / CODE REVIEW (Gemini 3.1 Pro High, independent verification)

You are doing an INDEPENDENT QC review of a just-implemented 3-screen UI redesign. READ these files (use your file tools; do NOT trust my summary — read the actual code):

CODE (the implementation under review):
- F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\UI\CharacterSelectScreen.cs
- F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\UI\MainMenuController.cs
- F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\UI\SettingsMenuUI.cs

SPEC (what it was supposed to implement):
- F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\UI_REDESIGN_3SCREENS_DECISION_2026-06-04.md

Project coding rules to judge against: (1) think before coding (2) minimum code, no speculation (3) surgical (4) verifiable. UI is procedural runtime-built, Unity URP, CanvasScaler 1920x1080, new Input System.

Produce a precise QC report. For EACH screen, answer:

1. **Spec conformance** — does the code implement the decision spec? CharacterSelect: 3-column normalized anchors (left ~0.20 / center ~0.52-0.72 / right ~0.28), 10-row roster with masked idle_south portraits + selected accent bar/scale + locked alpha + Echo cost, center static idle showcase with pedestal + cyan pulse + bob + select flash, RIGHT scrollable skill list (ScrollRect) querying SkillDatabase by classType + "Yetenekler yakinda" empty state + SEÇ/GERİ. MainMenu: lower-left column, RIMA title tracked, warm-orange "Yine geldin." whisper, 2px cyan divider, hover cyan+1.08+caret / pressed warm-orange, AYARLAR opens real SettingsMenuUI (not the Yakinda stub), forced scaler 1920x1080. SettingsMenuUI: AutoInit scaler 1920x1080, Aim/Dash gameplay-only rows hidden when no Player.

2. **Bugs / risks** — find real issues. Specifically scrutinize:
   - `EnsureSkillDatabase()` uses `AddComponent<SkillDatabase>()` whose `Awake()` sets `Instance` + `Build()`. Is there a FIRST-FRAME race where the skill list is empty because `Build()` hasn't run yet when the initial `SelectClass`/`RefreshSkillList` queries it? Should the DB be pre-built in `BuildScreen` before the first selection?
   - `RefreshSkillList` uses `Destroy()` (deferred to end of frame) to clear old rows — any accumulation risk on rapid re-selection?
   - Null-refs (Resources.Load returns null for any Pack/sprite path?), missing-icon fallback, locked-class click handling.
   - Layout: do main columns use NORMALIZED anchors (not fixed 1920px widths) so they don't collapse at 4K/ultrawide? Any hardcoded pixel widths that break non-1080p?
   - Raycast/hover correctness (transparent hit areas, button targetGraphic), ScrollRect viewport mask setup.

3. **Code quality** — over-engineering, dead code, duplicated logic, violations of surgical/min-code.

4. **Verdict per screen:** PASS / PASS-WITH-NITS / FAIL, with the top 3 most important findings as `file:line — issue — fix`.

Be precise and cite real line numbers from the files you read. Terse.
