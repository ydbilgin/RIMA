# COUNCIL QUESTION — LEAN / SHIP-FAST / OVER-ENGINEERING CRITIQUE LENS (Gemini 3.5 Flash)

READ this file fully: `C:\Users\ydbil\Downloads\RIMA_Ayrintili_Gorsel_Oynanis_Raporu_V2.md` (ChatGPT's "V2" presentation-upgrade plan for RIMA, 1912 lines, Turkish). Also skim `CURRENT_STATUS.md` top block for current state.

Context: graduation project, jury presentation SOON. One developer + AI agents. Locked: no new character animations, no shop, doors=back-row slots (shipped), cyan=player-only.

Your lens: RUTHLESS LEAN. The V2 plan is 1912 lines and proposes ~10 workstreams + 7-day plan. Assume we have far less time than 7 full days. Answer:
1. THE CUT: If we could only do 3 things from V2 before the jury, which 3 maximize perceived quality per hour? Why these?
2. OVER-ENGINEERING CALLS: Name every V2 item that is over-engineered for a demo and give the cheaper trick that gets 80% of the effect (e.g., full ScreenshotModeController class vs a 20-line "hide objects tagged Debug + load camera preset" static method; 5-theme RoomVisualProfile vs per-room tint+1 landmark; door state machine vs enable/disable + scale tween; 26 SFX vs which 8?).
3. FAKE-IT LIST: Which report figures should be produced as STAGED screenshots / external compositions instead of building runtime systems (and where is staging honest vs deceptive per V2 sec 13.4's own honesty rule)?
4. TIME BOMBS: Which V2 items look small but historically blow up (lighting setup in URP 2D? particle authoring? audio sourcing/licensing? parallax)? Flag with why.
5. Minimum audio: V2 says 18-26 SFX. Give your true minimum list (count + which) + fastest legal sourcing route.
6. Your lean 3-day version of V2's 7-day plan, hour-blocked.

Output: concise, bullet-heavy, Turkish or English. Max ~900 words. Disagree with V2 freely.
