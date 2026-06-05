ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
CharacterSelect v2 "diegetic roster room" redesign için FEASIBILITY/REUSE lensi. ANALİZ ONLY — kod değiştirme. Sonucu profil-DONE dosyasına yaz.

# READ
STAGING/CHARSELECT_ROSTERROOM_BRIEF_2026-06-04.md (full vision + 6 questions).
Code: Assets/Scripts/UI/CharacterSelectScreen.cs (current 3-column build), RimaUITheme.cs (AnchorPath/ClassAccent/ClassIdentity/Pack consts), SkillDatabase.cs.

# Answer (concrete, cite paths/methods)
1. Reuse map: how much of current CharacterSelectScreen (BuildScreen/RuntimeRoot/EnsureSkillDatabase/RefreshSkillList/SkillDatabase query/SEÇ-GERİ/ClassIdentity) survives into v2 vs rebuilt? The left roster list + center showcase get replaced by a room-with-placed-characters; the skill/identity/confirm wiring should be KEPT.
2. Placing 10 character sprites at fixed positions OVER a full-screen background Image in procedural UI: best approach (anchored Image per character with normalized anchorMin/Max so it scales at any res; or a RectTransform layout). How to keep PPU64 crispness + correct draw order (front chars over back chars = sibling order by Y).
3. Per-character CLICK select: Button + transparent hit Image per character (precedent: MainMenuController AddNakedButton transparent hitArea + Button). Hover/selected state. Keyboard nav optional.
4. Selection effect feasibility: cyan seal ring under selected char — reuse `UI/RIMA/Pack/pedestal_seal` as a ring Image toggled under the selected char + a glow + dim others (CanvasGroup alpha). Any existing glow/flash helper in CharacterSelectScreen (selectFlash/portraitGlow) reusable?
5. Background: load a generated Resources sprite as full-screen backdrop via RimaUITheme.CreateFullScreenBackdrop (cover/crop). Where to put the generated PNG (Resources/UI/RIMA/CharacterSelect/room_bg)? Import settings (PPU64, Point, no-compress, alpha).
6. Locked-class: IsUnlocked()/UnlockCost()/LockedButtonText() already exist — how to reuse for in-room locked presentation (dim sprite + lock glyph + Echo cost). 
7. Least-code build plan for v2 (which methods to add/replace, keep file procedural).

Terse, cite real paths/methods.
