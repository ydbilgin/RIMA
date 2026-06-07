# COUNCIL QUESTION — DEEP ARCHITECTURE / DESIGN LENS (Gemini 3.1 Pro)

READ this file fully: `C:\Users\ydbil\Downloads\RIMA_Ayrintili_Gorsel_Oynanis_Raporu_V2.md` (ChatGPT's "V2" presentation-upgrade plan for RIMA, 1912 lines, Turkish).
Also read for current state: `CURRENT_STATUS.md` (top 2 blocks) and `STAGING/report/VISION_VS_CURRENT_2026-06-06.md`.

Context: RIMA = 2D iso action roguelite graduation project. Deadline soon (jury presentation). Locked: ZERO new character animation production (code/VFX only); no shop (Echo only unlocks classes); exit doors = Hades back-row fixed slots (just shipped); cyan reserved for player/Rift magic.

Your lens: DEEP ARCHITECTURE + SYSTEM DESIGN. Answer:
1. V2 proposes several NEW data structures (RoomVisualProfile enum+fields sec 11.5, DoorSocketData/RoomExitSocket enums sec 20.1, DoorPortalController state machine sec 20.3, AudioEventSO sec 25.4, ScreenshotModeController sec 3.2). Which are architecturally sound extensions of RIMA's existing RoomTemplateSO/IsoRoomBuilder/RoomRunDirector design, and which duplicate or fight what exists? Where would YOU cut the abstraction (e.g., is a 5-theme RoomVisualProfile overkill vs 2 light presets + decal list)?
2. Sequencing: V2's 7-day plan (sec 30) = ScreenshotMode → doors → chamber → combat juice → draft → room variety → report images. Given that report screenshots are the deliverable but playtest feel also matters, is this dependency order optimal? Identify hidden dependencies (e.g., door portal visuals before/after door slot system? audio before combat juice tuning?).
3. Risk analysis: which V2 items have the highest chance of breaking existing verified systems (door row contract, draft flow, chamber bootstrap, Y-sort) if implemented hastily?
4. The state machine Hidden→Sealed→Cracking→Open→Highlighted→Selected for doors: worth full implementation, or is a 2-state (Sealed/Open) + tween sufficient for demo? Justify.
5. Scope verdict per workstream: ACCEPT / SIMPLIFY (say how) / REJECT-for-demo, as a table.
6. What does V2 architecturally MISS? (e.g., screenshot reproducibility needs deterministic seeds; lighting presets need URP 2D Light setup that may not exist in scenes.)

Output: structured Turkish or English answer, table for #5, concise. Max ~1200 words.
