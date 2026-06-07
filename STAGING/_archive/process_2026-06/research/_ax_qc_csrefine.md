# RIMA — CharacterSelect v2 Refinement — INDEPENDENT CODE QC (Gemini 3.1 Pro High)

You are doing an INDEPENDENT QC code-review of a just-implemented layout refinement. READ the actual code (do NOT trust a summary):
- F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\UI\CharacterSelectScreen.cs
Against the spec:
- F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\CHARSELECT_REFINE_DECISION_2026-06-04.md

Project rules to judge: (1) think before coding (2) minimum code (3) surgical (4) verifiable. Procedural Unity UI, 1920x1080 CanvasScaler, normalized anchors.

Verify conformance + find real issues:
1. **Framed boxes:** are IdentityZone/SkillZone/ActionZone now framed (panel_frame_9slice, Image.Type.Sliced, dark tint + cyan edge), or still flat MakeRect? Is inner content/wiring intact?
2. **Skills placement:** skills in bottom-bar RIGHT zone (NOT a new vertical side-panel / RoomLayer child)? Zone order = skills rightmost?
3. **Character spread:** RosterPlacements updated to the wide two-row coords (front .22/.41/.59/.78 @ y~.39-.44; back .08/.25/.42/.58/.75/.92 @ y~.54-.61)? Sizes/scales sane? Front-over-back sibling order preserved?
4. **Left-box portrait:** IdentityZone has a selected-char portrait Image updated in SelectClass via LoadCanonicalSprite? No null-ref risk?
5. **Bugs/risks:** null-refs (Resources.Load fallbacks), raycast (zone Images raycastTarget=false so they don't block?), did anything in the KEPT list get broken (RefreshSkillList/MakeSkillStripArea/backdrop resilient-load/SelectClass data wiring)? Any over-engineering (dynamic layout engine)?
6. **Verdict:** PASS / PASS-WITH-NITS / FAIL + top 3 findings as `file:line — issue — fix`.

Cite real line numbers. Terse.
