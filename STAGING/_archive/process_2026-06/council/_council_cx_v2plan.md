ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Feasibility/reuse audit of an external presentation-upgrade plan (ChatGPT "V2") against RIMA's ACTUAL codebase. ANALYSIS ONLY, no code changes. Write result to CODEX_DONE.md.

# Source to read
READ this file fully: `C:\Users\ydbil\Downloads\RIMA_Ayrintili_Gorsel_Oynanis_Raporu_V2.md` (1912 lines; sections 3-31 are the operational plan).

# Your lens: CODE FEASIBILITY / WHAT-ALREADY-EXISTS / REUSE-VS-BUILD
For each major workstream in the V2 plan, answer from the codebase (grep/read actual files, cite file:line):

1. **ScreenshotMode (sec 3):** Does anything like this exist (GameViewSetup? debug toggles? F-key overlays)? What debug objects/overlays actually exist that would need hiding (green squares, markers, dummy HP, death overlay leak)? Estimated size S/M/L to build the proposed ScreenshotModeController + presets.
2. **World-space door portals (sec 9, 20):** Compare proposed DoorPortal/DoorSocketData model vs our JUST-SHIPPED socket system (commit 20d1f09c: north-anchor row in IsoRoomBuilder.BuildExitDoors, DoorSocket data, GateBehavior, RoomRunDirector trigger wiring). How much of the proposal is already done? What's genuinely missing (rune per type exists? reward badge? danger pips? door state machine Hidden→Sealed→Cracking→Open? opening animation)? Reuse-vs-build verdict per piece.
3. **Chamber visual pass (sec 5-6):** What does ChamberSelectBootstrap already build (pedestals, rings, statues, lighting)? Is there ANY 2D Light usage in chamber/_Arena today? Effort for: pedestal cyan lights + purple ambient + class weapon-marker silhouettes + possession VFX stages (sec 6.6) + prompt restyle.
4. **Combat juice (sec 7, 21):** Which pieces exist (JuiceManager? hit-stop? screen shake? hit flash? SlashArcVFX? BROKEN/SUNDERED indicators? execute prompt)? Cite files. Which of the proposed timing values (21.2-21.4) map to existing serialized fields vs need new code?
5. **Draft UI upgrade (sec 8, 23):** SkillOfferUI/CardJuiceHandler/TooltipSystem current state vs proposed (rarity frames? tag chips? synergy line? build meter? select animation?). SkillData: does it have icon field, tags, rarity today?
6. **RoomVisualProfile + decals + void parallax (sec 11):** Does RoomTemplateSO have any theme/visual fields? Decal infrastructure? Void backdrop system (backdrops were imported at some point — what's live)? Landmark prop support in placer?
7. **Audio (sec 25):** Any AudioManager/AudioMixer/SFX hooks in code today? (Memory says audio=deferred, confirm it's truly zero or partial.)
8. **Echo breakdown screen + chamber restoration (sec 24):** EchoWallet/ComputeRunAward exists — how hard is the run-end breakdown panel? Chamber restoration = new system (size?).

Output format in CODEX_DONE.md: per-workstream table — V2 item | exists today (file:line) | missing delta | reuse-vs-build | size S/M/L | feasibility note. End with your TOP-8 by ROI (impact/effort) strictly from a code perspective. Do NOT reproduce prior audits; fresh evidence only.
