ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
UnityMCP read_console UNRELIABLE — verify with `dotnet build RIMA.Runtime.csproj --no-restore` (0 errors required). Do NOT self-certify; Opus reviews.

# BUILD — PHASE 2 Story: room-start monolog + boss intro/phase-2 (PURE .cs, self-building UI)

Spec = `STAGING/DESIGN_LOCK_DEMO_S6.md` §1.5 (per-room beats + EXACT lines). Delivers the "coherent story" surface,
environment-first/text-minimal. C# only — NO scene edits. Self-build the UI in code (mirror `SkillBarUI.cs` /
`MapProgressController.cs` self-building Canvas pattern). Use `RimaUITheme` tokens; monolog text cyan `#48E0FF` (incidental UI per §4.2).

## E1 — New `Assets/Scripts/UI/RoomMonologController.cs`
- Self-bootstrapping MonoBehaviour (RuntimeInitializeOnLoad or attaches to the UI/HUD root like other RIMA UI; do NOT
  require manual scene-wiring). Subscribes to the LIVE `Assets/Scripts/Systems/Map/RoomLoader.cs` `OnRoomChanged` event
  and reads `CurrentRoomData` / current room index.
- Bottom-center small typewriter line box: faint dark gradient (no hard border), ~30 chars/sec, hold ~3s, fade out
  ~1s. Skippable on key press. One line per room ENTER, shown once.
- EXACT lines by room (1-indexed demo order; show NOTHING where blank):
  - R1 Tutorial: (none)
  - R2 Guard Hall: "Someone kept this fire. Long after the order fell."
  - R3 Ambush Cloister: "The chains here lead somewhere. They always have."
  - R4 Map-Fragment Vestibule: "You knew this once."   (R4 is the reward/quiet room)
  - R5 Penitent Containment (approach): "The Sovereign's breath is colder here."
- R5 ALSO shows a larger centered TITLE CARD (separate from the line, brief): main "THE PENITENT SOVEREIGN",
  subtitle "He took the wound so the seal would hold." (fade in/hold/fade).
- Map room→line by RoomSequenceData identity or index; if room identity is ambiguous, key off the 1-based load index
  from RoomLoader. If you cannot cleanly get the index/data, mark E1 BLOCKED with what you found — do NOT guess.

## E2 — Boss phase-2 monolog hook (`Assets/Scripts/Enemies/Boss/PenitentSovereign.cs`)
- The boss transitions to Phase2 at 33% HP (chains shatter). At that transition point, trigger ONE monolog line via
  RoomMonologController: "Discipline breaks before the chain does."
- Surgical: just call the monolog at the existing phase-2 transition; do not restructure boss logic. If no clean
  static entry on RoomMonologController, expose a simple `RoomMonologController.Say(string)` static/singleton method.

## CONSTRAINTS
- Demo scope: do NOT add Nexus Core / Rift March / meta-hub references (§7 #7). Lines above ONLY.
- No scene edits, no new asmdef.

## DELIVER (write to DONE file)
Files changed/created (file:line), how the controller self-builds + hooks RoomLoader, how room→line mapping works,
how the boss phase-2 line fires, `dotnet build RIMA.Runtime.csproj` result. List BLOCKED + why. Concise.
