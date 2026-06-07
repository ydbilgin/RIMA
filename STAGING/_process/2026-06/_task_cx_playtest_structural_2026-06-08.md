ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.
DO NOT DELEGATE — do this yourself.

# Amaç
Apply the 4 structural playtest fixes YOU already root-caused in `CODEX_DONE_yekta.md` (the prior playtest investigation). The 2 safe fixes (reward scale, camera zoom) are already in the working tree uncommitted — include them in the final commit. Implement each fix independently; if any single one is risky/unclear, do the others and mark that one `BLOCKED:` rather than guessing.

## FIX 3 — Scene/chamber bleed (HIGHEST PRIORITY: kills "3 Warblade + pedestal ring in combat room")
Root cause (your finding): two selection systems active + no cleanup of stale chamber roots. `CharacterSelectScreen.Awake()` auto-attaches `ChamberSelectBootstrap` (CharacterSelectScreen.cs:174); `MainMenuScreen.OnPlayClicked()` makes a persistent object with both (MainMenuScreen.cs:239,242); `ChamberSelectBootstrap` creates a new `AttunementChamber_Runtime` every time with no pre-build cleanup (ChamberSelectBootstrap.cs:268,276,438).
SURGICAL FIX (do this, low risk):
- Before `BuildWorldRoom()` (or at the start of the chamber build), DESTROY any existing `AttunementChamber_Runtime` root + `EchoStations` + chamber-spawned player/dummy roots (idempotent — no duplicate/stale pedestals).
- On COMBAT scene load (`_Arena`, and `_IsoGame` if reachable), DESTROY any leaked chamber roots (`AttunementChamber_Runtime`/EchoStations) so pedestals never bleed into a combat room. Put this guard where the combat room initializes (RoomRunDirector init or a small scene-load hook).
- DO NOT rip out the `_IsoGame` bypass start paths in this task — that dual-system consolidation is a separate decision. Just guarantee cleanup so the visible bleed is gone. If you believe a deeper change is required to fully fix it, note it as `FOLLOW-UP:` and still apply the cleanup.

## FIX 4 — Run-room exit door = [G]-interact + locator (user explicitly wants this)
Current `_Arena` model is walk-trigger (RoomRunDirector.cs:1021,1032,1039 advance on OnTriggerEnter2D). User wants: walking into the door must NOT pass/advance; door opens only on [G].
FIX:
- Change `RoomRunExitDoorTrigger` to track `playerInRange` (trigger collider = range only), show the HUD interaction prompt (use the existing localized prompt path — chamber rift uses `Loc.T("chamber_select.prompt.enter_rift")` = "Rift'e Gir"; reuse or an equivalent "Geç" key), and call `director.TryEnterDoor(choiceIndex)` ONLY on `Keyboard.current[Key.G].wasPressedThisFrame`.
- LOCATOR (door hard to find): when `ConfigureExitDoors(true)` opens doors, add a simple readable cue — a pulsing glow/ring at each open door (reuse existing VFX if available) AND show the [G] prompt when in range. Keep it lean; a HUD arrow is optional/nice-to-have, not required.

## FIX 5 — Chamber/entry portal forced BLACK silhouette (user: "kapı her türlü siyah görünmeli")
Root cause: chamber exit/rift is position+light+prompt only, no SpriteRenderer silhouette (ChamberSelectBootstrap.cs:137,145,207,242).
FIX: spawn a dedicated `ChamberExitPortal` SpriteRenderer at `exitWorld` during `BuildWorldRoom()`/`ApplyAtmospherePass()`, set its sprite to the portal/arch asset, force color to solid black (high alpha), correct sorting so it reads as a silhouette. Keep the blue `DoorLight` as a secondary accent only.

## FIX 6 — Dark / empty run room ("hiçbir şey görünmüyor")
Root cause (your finding): NOT global lighting (Global Light 2D intensity 1.4). Room CONTENT is too sparse — `DemoRoomBank` Library rooms: `Combat_Large_01` 1 prop, `Elite_01` props:[], `Boss_Intro_01` props:[].
FIX (pick the LOWER-RISK path and VERIFY the run still builds):
- Preferred: enrich the sparse `DemoRoomBank` Library templates with props/decals via the existing AutoProps utility (`RoomTemplateAutoPropsUtility` / the Rooms tab Auto-Props) so each combat room has readable content + contrast. OR swap `DemoRoomBank` entries to validated `Assets/Data/Rooms/Generated/` templates that already have props AND valid exit sockets + spawn markers.
- ALSO confirm enemies actually SPAWN in that room (image 4 showed no mobs) — if the dark room is a combat room with no spawn, report `BLOCKED:` or fix the spawn marker if it's a clear data gap.
- Whichever path: run `RIMA/Rooms/QC/Smoke Test All Templates` (or the smoke suite) and confirm it stays green (26/26) and `_Arena` still builds a playable room.

## VERIFY (mandatory)
- After each fix: `refresh_unity` + `read_console` → 0 compile errors.
- Run the relevant EditMode/smoke tests; paste pass/fail counts. If Unity readiness times out, retry once; if still blocked, report compile status + `BLOCKED: tests not run`.
- DO NOT visual-claim ("looks better") — report VALUES and what you changed.

## COMMIT (after all verified)
Commit everything (the 2 pre-applied safe fixes + the structural fixes that succeeded):
`fix(playtest): reward scale + camera zoom + chamber-bleed cleanup + [G]-interact doors + black portal + room content`
List per-fix status (DONE / BLOCKED / FOLLOW-UP) and the commit hash in CODEX_DONE.md.
