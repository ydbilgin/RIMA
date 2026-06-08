ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.
Do NOT delegate. Implement, then write a summary to CODEX_DONE.md.

# Amaç
Implement the chamber presentation fixes from the council decision (STAGING/CHAMBER_PRESENTATION_DECISION_2026-06-08.md). Camera framing, screen-space interaction prompt, lighting, dummy/spawn placement, chamber Echo HUD, and the locked-class bypass bug. Surgical — only the files listed below. Keep the existing EditMode suite (27/27 incl. RoomCompletionInvariantTests) GREEN.

# Files in scope
- Assets/Scripts/UI/ChamberSelectBootstrap.cs (primary)
- Assets/Scripts/UI/CharacterSelectScreen.cs (lock policy)
- Assets/Scripts/UI/CharacterSelectController.cs (lock policy)
- Assets/Scripts/Systems/PlayerClassManager.cs (lock guard)
- NEW (small): Assets/Scripts/Systems/ClassUnlockPolicy.cs (shared unlock policy)
- NEW test: Assets/Tests/EditMode/... a focused unlock-policy test
- Do NOT touch PlayerAnimator, weapon system, combat scenes, or unrelated rooms.

You already audited this code (see your CODEX_DONE_yasinderyabilgin.md analysis). Reuse those file:line findings.

## 1. CAMERA — fixed framing, player at ~60% from top
Root cause: ChamberSelectBootstrap ~L1006 `followOffset = chamberBounds.center - player.position` locks camera to room center while spawn (~L481 `frontY=minY+2`) is at the extreme front edge → player at bottom, void above.
- Add SerializeField `[Range(0.45f,0.72f)] private float chamberPlayerScreenY = 0.60f;` near the other chamber camera fields (L37-39).
- Keep the existing fit-to-room ortho logic (multiplier 1.04 / padding 0.35 / min 5.8) so the whole diamond stays visible. Do NOT switch to pixel-perfect 1:1 ortho.
- Replace the room-center follow offset with framing that places the PLAYER (at spawn) at `chamberPlayerScreenY` fraction down from the top of the view. i.e. camera center.y = player.position.y + (orthoSize * (2*chamberPlayerScreenY - 1)). Keep center.x = chamberBounds.center.x, z = -10. (At 0.60 this is roughly +0.20*orthoSize above the player; verify the sign so the player ends up in the LOWER third, NOT pushed off-screen.)
- Use a FIXED anchor: either disable CameraFollow target-follow for the chamber, or set `follow.target` to a static anchor GO placed at that computed center with `follow.worldOffset=(0,0,-10)`. Do NOT rewrite CameraFollow.
- Move spawn off the bottom edge: change the spawn front-Y from `minY+2` to `minY+4` (≈ (14,4)). Keep it on the central aisle (axisX).

## 2. INTERACTION PROMPT — chamber-local bottom-center SCREEN canvas
Currently world-space TMP: CreatePromptLabel (936-942), CreateWorldText (944-957), ShowPrompt (319-329).
- Build a chamber-local `ScreenSpaceOverlay` Canvas + a TMP UGUI prompt panel. Anchor min=max=(0.5,0.15), pivot (0.5,0.5), size 260x36, anchoredPosition (0,0).
- `[G]` shown as a key-cap: a dark rounded box (~24x24) with white "G", followed by the action text. Keep it simple (a child Image + TMP is fine; if that's heavy, a single TMP with a boxed-glyph rich-text is acceptable).
- 0.15s alpha fade-in when shown.
- Convert ShowPrompt to ignore the world position arg and only set text + active state on the screen panel. Update all call sites (class station 212-213, locked station 242-243, dummy 277) to pass the same text; positions are ignored now.
- Remove/disable the world-space TMP prompt path (CreatePromptLabel/CreateWorldText) so nothing floats above characters.
- Do NOT add multi-interactable priority logic — last-entered-wins is fine.

## 3. LIGHTING — readable but moody
- Global Light2D: 0.92 → 1.10 (keep color (0.78,0.86,1)). Expose as SerializeField `chamberGlobalLightIntensity = 1.10f` and use it.
- Add ONE soft fill Point Light2D at the aisle midpoint between spawn and ExitCell(14,18): intensity 0.35 (expose as SerializeField `chamberFillLightIntensity = 0.35f`), outerRadius 10, color (0.50,0.68,1,1).
- Pedestal base: intensity 0.42 → 0.70, radius 2.2 → 3.0 (L850-857).
- Pedestal occupied: 0.72 → 0.95; highlighted: 1.05 → 1.30, radius 3.2 → 3.8 (L1286-1292).
- Locked pedestal: keep gray-blue color but intensity floor ≥ 0.45 so the locked figure still reads.
- Dummy: add a dedicated Point Light2D intensity 0.8, radius 3.0, cool color, parented to the dummy.

## 4. DUMMY + SPAWN
- Dummy cell (dynamic, L513-515): replace with `dummyX = Clamp(axisX + columnOffset + 2, minX+2, maxX-3)`, `dummyY = RoundToInt(Lerp(chamberSpawnCell.y, ExitCell.y, 0.42f))`, then `PickNearestWalkableCell(...)`. Keep it off the right edge and out of the side figure columns.
- Fallback dummy (L473-474): move from (ExitCell.x+7,10) to something inside bounds like (ExitCell.x+4, 9).
- Do NOT resize Chamber_CharSelect.asset in this pass (repositioning within 28x20 is enough). If you believe it's still cramped, NOTE it in CODEX_DONE — do not regen the asset.

## 5. CHAMBER ECHO HUD — chamber-local label (NOT combat HUDController)
HUDController is combat-only and absent in the chamber → that's why no HUD shows.
- Add a chamber-local screen overlay Echo label (reuse the VISUAL pattern of HUDController.BuildEchoDisplay 560-601, but standalone). Place on the chamber ScreenSpaceOverlay canvas: anchor (0,1), pivot (0,1), anchoredPosition (12,-12), size ~140x20. Cyan-diamond icon + EchoWallet.Balance number. Update only when balance changes (poll EchoWallet.Balance in the chamber Update; cheap).

## 6. LOCK BUG — centralize unlock policy; starter = {Warblade, Elementalist}
Root cause: hardcoded always-unlocked list {Warblade,Elementalist,Ranger,Shadowblade} duplicated in ChamberSelectBootstrap.IsUnlocked (1353-1359), CharacterSelectScreen.IsUnlocked (1244-1249), separate policy CharacterSelectController.IsUnlocked (156-160); PlayerClassManager.SetPrimaryClass (40-47)/SwitchClass (145) accept any class with no validation.
- Create `Assets/Scripts/Systems/ClassUnlockPolicy.cs`: `public static class ClassUnlockPolicy { public static bool IsUnlocked(ClassType c); public static int UnlockCost(ClassType c); ... }`. Starter unlocked = ONLY Warblade + Elementalist; all others require the PlayerPrefs unlock flag (reuse the existing UnlockPrefKey/UnlockCost conventions from ChamberSelectBootstrap). Keep the PlayerPrefs unlock path identical so already-unlocked classes persist.
- Replace the 3 duplicated IsUnlocked methods to delegate to ClassUnlockPolicy.IsUnlocked (keep their signatures/call sites; just change the body). Remove Ranger & Shadowblade from the starter allow-list.
- Defense in depth: in PlayerClassManager.SetPrimaryClass, if `!ClassUnlockPolicy.IsUnlocked(cls)` then log a warning and return WITHOUT changing SelectedClass/PrimaryClass. (SwitchClass forwards through it.)
- Add a focused EditMode test (new file under Assets/Tests/EditMode/): asserts ClassUnlockPolicy.IsUnlocked(Warblade)==true, IsUnlocked(Elementalist)==true, IsUnlocked(Shadowblade)==false, IsUnlocked(Ranger)==false (before any unlock), and that PlayerClassManager.SetPrimaryClass(Shadowblade) does NOT set SelectedClass to Shadowblade.

## 7. "odalara bak" (REPORT ONLY)
Find the COMBAT scene/global light intensity (the _Arena combat lighting, not the chamber). REPORT its value in CODEX_DONE. Do NOT change it here.

# Verify before done
- Unity compiles 0 errors (read_console after edits).
- Run EditMode tests: existing suite stays green + new unlock test passes. Report counts.
- Unity is OPEN so you CANNOT screenshot/playtest — the user will playtest. Make framing + lighting Inspector-tunable as specified so the user can converge live.
- CODEX_DONE.md: list every changed file with what changed + final test counts + the combat-light value from §7 + any BLOCKED items.
