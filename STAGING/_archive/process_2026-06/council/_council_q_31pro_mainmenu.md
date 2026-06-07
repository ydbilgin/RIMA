# Council — Gemini 3.1 Pro (High): MainMenu restyle (design + approach)

RIMA = 2D iso roguelite (Hades+Dead Cells), "The Shattered Keep" dark fantasy. Tone = "Vivid Vulnerability". UI canon (NLM, LOCKED): "UI yoktur, sadece bilgi vardır" — opaque rectangular panels / big frames / black blocks FORBIDDEN; translucent, thin, sharp-edged, "ink on dirty paper". Palette: slate #3A3D42 base, cyan #00FFCC accent (seal/energy), void-purple depth, warm #E89020 only for reward. Font: pixel serif + tiny pixel. Title screen canon: NO epic slogan — a quiet statement ("Yine geldin." / "Burası hatırlar." / "Rift açık. Her zaman açık oldu.").

## Current MainMenu state
- Scene `Assets/Scenes/UI/MainMenu.unity`: `MainMenuCanvas` (ScreenSpaceOverlay) + `MainMenuController` + authored child "Root".
- Authored visual (in scene): "RIMA" logo + "THE RIFT HUNTERS" subtitle + 3 Turkish buttons **BAŞLA** (start→CharacterSelect) / **AYARLAR** (settings) / **ÇIKIŞ** (quit). Background = main_menu_bg (shattered keep silhouette + void + cyan cracks, on-brand, imported).
- `MainMenuController.cs` = thin button-wirer; Settings is a STUB (shows "Yakında" tooltip — not implemented in this scene).
- ALSO exists `MainMenuScreen.cs` = a separate RUNTIME menu builder (RuntimeInitializeOnLoadMethod bootstrap + _gameStarted guard; referenced by tests). Already canon-styled by us (RIMA + "Yine geldin." + buttons + version) but NOT used by the MainMenu scene.
- User wants a FULL restyle (nice, premium, on-brand). MCP cannot screenshot overlay UI, so the user will eyeball in Game View; design must be robust/clear without screenshot iteration.

## Questions (concise, opinionated, actionable)
1. **Approach:** restyle the AUTHORED scene objects in-place, OR rebuild the menu at runtime in code (cleaner, full control, matches our other runtime-built screens — could reuse MainMenuScreen)? Which is lower-risk + better result given no screenshot iteration?
2. **Subtitle:** keep "THE RIFT HUNTERS" (legit game subtitle) or replace with quiet canon line ("Yine geldin.")? Or both (title + tiny quiet line)?
3. **Premium layout:** concrete on-brand layout for a striking-yet-minimal title screen (logo placement, tagline, button column style/position, version). What 2-3 touches make it feel premium (e.g. subtle title cyan glow, button hover, vignette over backdrop) WITHOUT violating "no opaque boxes"?
4. **Settings:** AYARLAR is a stub here. Keep it (wire to the real settings system if one exists globally), hide it, or leave as "soon"?
5. Buttons in Turkish (BAŞLA/AYARLAR/ÇIKIŞ) — keep Turkish (user is Turkish) or English? 

This feeds an Opus synthesis + implementation. Tight.
