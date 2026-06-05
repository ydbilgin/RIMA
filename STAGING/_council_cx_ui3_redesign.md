ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
3-ekran UI redesign (MainMenu / Settings-wire / CharacterSelect) için FEASIBILITY + REUSE lensi: RIMA'da neyin zaten hazır olduğunu, neyin yeniden kullanılabileceğini, neyin yeni-üretim gerektirdiğini ve en az-kod yolunu söyle. ANALİZ ONLY — kod değiştirme.

# Görev
READ this brief file first: STAGING/UI_REDESIGN_3SCREENS_BRIEF_2026-06-04b.md
Also relevant code (read only as needed): Assets/Scripts/UI/CharacterSelectScreen.cs, Assets/Scripts/UI/MainMenuController.cs, Assets/Scripts/UI/SettingsMenuUI.cs, Assets/Scripts/Core/KeyBindManager.cs, Assets/Scripts/UI/RimaUITheme.cs.

ANALYSIS ONLY, no code changes. Answer from a FEASIBILITY / WHAT-ALREADY-EXISTS-IN-RIMA / REUSE-vs-BUILD lens. Do NOT reproduce a generic audit — focus on the build decisions below. Write the result to CODEX_DONE.md.

Answer these, concretely (cite files/paths/methods that already exist):
1. CharacterSelect rebuild: CharacterSelectScreen.cs already builds 3 columns procedurally. What is the LEAST-code path to convert it to the target wireframe (LEFT vertical roster list w/ avatars + CENTER idle-animation showcase on a backdrop + RIGHT scrollable skill/description panel + CONFIRM)? Which existing helpers/methods (RimaUITheme.AnchorPath, ClassIdentity, ClassAccent, Pack 9-slice frames, ScrollRect usage elsewhere) can be reused? Is there an existing ScrollRect-based list anywhere in the codebase to copy?
2. CENTER idle animation: can we play the existing RuntimeAnimatorController (Resources Characters/{type}/{type}) inside a UI Image/RawImage easily, or is a static idle_south sprite the pragmatic choice? Cheapest approach that still looks "alive"?
3. LEFT roster avatars: can we reuse idle_south sprites cropped as small avatars per row (no new art), or is there an existing icon set? Any sprite-cropping helper already in the codebase?
4. RIGHT skill panel: do per-class skill DATA (skill names + descriptions) exist anywhere queryable (a ScriptableObject, a skills DB, PlayerClassManager, skill controllers)? And per-skill ICONS — is the PassiveIcon sheet (RimaUITheme.PassiveIcon) reusable for skill icons, or do we need a small generate batch? List exact skill-data source if it exists.
5. Settings wiring: confirm SettingsMenuUI.cs can be opened from the MainMenu scene (not just in-game). What's the minimal hook to replace MainMenuController's "Yakında." overlay (BuildSettingsOverlay) with opening the real SettingsMenuUI? Any dependency (KeyBindManager init, pause/timeScale, UIManager) that would break it outside gameplay?
6. Resolution-robustness: what CanvasScaler settings do these screens currently use, and what's the fix so layout doesn't collapse at 4K (the CharSelect single-panel bug)?
7. Minimal NEW-asset list: from a reuse-first stance, what genuinely must be generated (e.g. a CharSelect center backdrop, skill icons) vs reused (Pack frames, idle sprites, existing backgrounds main_menu_bg/bg_seal_keep)?

Be concrete and terse. Cite real file paths and method names.
