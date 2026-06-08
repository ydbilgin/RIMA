ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Council (FEASIBILITY/REUSE lens): RIMA chamber (class-select room) presentation standards — camera framing, interaction-prompt placement, lighting, dummy/map layout. ANALYSIS ONLY, no code changes. Write result to CODEX_DONE.md.

# Lens
You are the CODE / FEASIBILITY / REUSE advisor. For each question, answer from the angle of: what ALREADY exists in RIMA that can be reused, what is feasible with minimal surgical change, and what the cheapest correct implementation path is. Ground every answer in the ACTUAL current code (file:line + current value) — they are given below. Do NOT reproduce any prior audit. Do NOT change code.

# Grounding — current code state (verified by Explore agent)
File: Assets/Scripts/UI/ChamberSelectBootstrap.cs unless noted.
- CAMERA: CameraFollow (Assets/Scripts/Camera/CameraFollow.cs) default worldOffset=(0,0,-10), smoothTime 0.15. Chamber setup at lines 997-1009: `follow.target = player; followOffset = chamberBounds.center - player.position; followOffset.z=-10; follow.worldOffset = followOffset;` → NET EFFECT: camera locks to chamber CENTER (because target.pos + (center - target.pos) = center). Player spawns LOW (chamberSpawnCell.y around 6) so player renders near bottom of screen. Camera fit: chamberCameraFitMultiplier=1.04 (L37), chamberCameraFitPadding=0.35 (L38), chamberCameraMinimumOrthographicSize=5.8 (L39). PPU 64, Pixel Perfect Camera, refRes 640x360.
- HUD ECHO: HUDController.cs BuildEchoDisplay (560-601) + UpdateEchoDisplay (288-303) build a screen-space (ScreenSpaceOverlay) top-left Echo counter. BUT HUDController is COMBAT-ONLY — it is NOT present/instantiated in the chamber scene. So the Echo HUD never renders in the chamber. The chamber currently has NO screen-space HUD at all.
- INTERACTION PROMPT: Chamber uses WORLD-SPACE TextMeshPro: CreatePromptLabel (936-942) + CreateWorldText (944-957, sortingLayer "UI" order 200) + ShowPrompt (319-329) positions it at statue.position + up*1.05. Example text L254: "[G] SHADOWBLADE'A GEC". MEANWHILE HUDController.cs ALREADY has a screen-space bottom-center prompt: BuildInteractionPrompt (637-668) anchored at (0.5, 0.15), SetInteractionPrompt (370-375). So a screen-space bottom prompt pattern already exists in the codebase but is not used by the chamber.
- LOCK: IsUnlocked (1353-1359) returns true unconditionally for Warblade/Elementalist/Ranger/Shadowblade, else checks PlayerPrefs unlock flag. Attune guard at L219 `if (IsUnlocked(confirmed))`. Popup path AcceptClassicSelectionFromPopup (1148-1179) checks IsUnlocked at 1155-1159 then TryUnlockFromPopup. EchoWallet.TrySpend (EchoWallet.cs 40-50) guards balance. NOTE: user reproduced "can switch to a class that should be locked" at RUNTIME despite these static guards — investigate which path bypasses the lock (dummy-skin preview ApplySelectedClassToDummyOnly? the 4 hardcoded-unlocked classes? popup applying before unlock confirmed?).
- DUMMY + MAP: Dummy cell dynamic at 513-515 (Lerp 0.34 spawn→exit, x = axisX + columnOffset*2 + 2, clamped to maxX-1 → pushed to far right edge), fallback (21,10) at L474. Spawned L869 grid.GetCellCenterWorld. Grid cellSize (0.96, 0.585, 1) L387 (isometric). ExitCell const (14,18) L48. Floor bounds from builder.LastFloorCells (TryGetFloorBounds 534-553), fallback bounds 12x8 L1068.
- LIGHTING: Global Light2D intensity 0.92 color (0.78,0.86,1) L1012-1018. Door point light intensity 1.2 radius 4.5 color (0.6,0.8,1) L123-131. Pedestal point light base intensity 0.42 radius 2.2 color (0.18,0.88,1) L850-858; modulated highlighted 1.05/r3.2, occupied 0.72, locked color (0.36,0.42,0.48) L1288-1293.

# Questions (answer each tightly, numeric where possible, REUSE-first)
Q1 CAMERA FRAMING: For a high 3/4 top-down ARPG close follow camera, where should the player sit vertically on screen during exploration (% from top)? Should the chamber camera FOLLOW the player or be FIXED framing the room? Given the current code locks to chamber center, what is the minimal surgical change (and exact worldOffset.y / ortho size) to put the player at the recommended vertical position? Reuse CameraFollow or change the offset math?

Q2 INTERACTION PROMPT: Cheapest correct path to move the chamber prompt from world-space (CreatePromptLabel/ShowPrompt) to a Hades-style bottom-center SCREEN prompt. Can we REUSE HUDController.BuildInteractionPrompt (already bottom-center 0.5,0.15) by adding a lightweight HUD/canvas to the chamber, or build a small chamber-local screen-space canvas? Which is less code / less risk? Give the concrete anchor + offset.

Q3 LIGHTING: Given the current values above produce a too-dark scene, what concrete value bumps make it readable-but-moody (global intensity, pedestal intensity/radius, plus a fill/ambient)? Is a single global-intensity bump enough or do we need a second fill light?

Q4 DUMMY + MAP + HUD: Where to place the dummy so it has open room (relative to portal/figures/spawn), and how to extend the floor/map so it isn't cramped at the edge (reuse a bigger room template or just enlarge bounds)? Where should the player SPAWN for the Q1 framing? AND: cheapest way to actually show the Echo HUD in the chamber (instantiate HUDController, or a small chamber-local Echo label)?

Also: BUG INVESTIGATION — trace which code path lets a locked class be selected at runtime, and name the exact fix location.

Write your answer to CODEX_DONE.md.
