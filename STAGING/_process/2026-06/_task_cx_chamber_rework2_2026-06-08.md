ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.
DO NOT DELEGATE — do this yourself.

# Amaç
User playtested the new 5+5 chamber (commit 260cf159) and reported a CRASH + walkability + a layout/selection rework. Fix all. Reference screenshot markers (described below) + the prior chamber file `Assets/Scripts/UI/ChamberSelectBootstrap.cs` + `Assets/Data/Rooms/Special/Chamber_CharSelect.asset`.

## P0 — CRASH (record + FIX first)
Recorded stack trace (repro: enter chamber as Ranger, approach Warblade station, press G):
```
MissingReferenceException: The object of type 'UnityEngine.SpriteRenderer' has been destroyed but you are still trying to access it.
  RIMA.ChamberSelectBootstrap+<AttuneRoutine>d__66.MoveNext () at Assets/Scripts/UI/ChamberSelectBootstrap.cs:785
  RIMA.ChamberSelectBootstrap:Update() at Assets/Scripts/UI/ChamberSelectBootstrap.cs:253
```
Root-cause + FIX: `AttuneRoutine` accesses a destroyed SpriteRenderer's `.transform` (line 785). Likely the statue/sprite was destroyed (stray-cleanup or rebuild) while the coroutine was mid-flight. Add null/destroyed guards (`if (sr == null) yield break;` and cache the transform before any `yield`/await; don't touch destroyed refs). Make attune safe if the target was removed. Verify by repro after fix = no exception.

## P0 — WALKABILITY (player gets stuck, can't walk)
User: "arenada yürüyemiyorum, takılıyorum bir şeylere." The pedestal DISC props block movement. FIX: **remove the pedestal disc props entirely** (see P1) and ensure the chamber floor is fully walkable — no leftover blocking colliders where silhouettes/dummy stand (silhouettes must NOT block walking; give them no collider or a tiny trigger-only one). Player must walk freely across the whole room.

## P1 — LAYOUT (per screenshot markers)
Screenshot markers (chamber scene): RED diagonal line = upper-left (NW) edge · YELLOW diagonal line = lower-left (SW) edge · YELLOW SQUARE = upper-right (NE) area · PURPLE SQUARE = center · player spawn = bottom.
- **Remove the round pedestal DISC props** (they clutter + block). Keep ONLY the 10 character figures.
- **Place 10 character silhouettes 5+5 side-by-side along the two left edges:** 5 along the RED line (NW edge), 5 along the YELLOW line (SW edge), evenly spaced.
- **Silhouettes FRONT-FACING (face the screen):** use the front/idle_south sprite so each character's face looks toward the camera. (FOLLOW-UP note only, do NOT build now: later these become proper idle poses at the correct angle.)
- **Move the rift exit DOOR to the YELLOW-SQUARE position** (upper-right / NE area).
- **Place the DUMMY at the PURPLE-SQUARE position** (center).

## P2 — DUMMY = character select + immortal combat target
- Dummy is **named "Dummy"**, **NOT transparent** (solid sprite), shows the **currently-selected class**.
- Dummy is **interactable (G)** → opens a **character-select popup/screen** → pick a class → (a) set the player's play-class (`PlayerClassManager.SelectedClass`/`PrimaryClass`), (b) the dummy's visual becomes the selected class. "Whatever we pick appears as the dummy."
- Dummy **combat**: HP scaled high to stats, **refills (regen)**, **does NOT die** (training dummy — clamp HP > 0 / ignore lethal).
- Selecting via the dummy popup is the selection mechanism. The 10 silhouettes = front-facing roster display.
- **AMBIGUITY — flag, don't wild-guess:** if you're unsure whether the 10 silhouettes should ALSO be walk-to-select (vs display-only), implement dummy-popup-select as primary + silhouettes as display, and write the open question in your report.

## INVESTIGATE — "multiple start screens"
User: "oyuna başlama ekranı birden fazla var galiba." Likely the un-removed `_IsoGame` dual-system bypass (`CharacterSelectScreen.OnStartRun`/`CharacterSelectController.OnConfirmClicked` → `_IsoGame`) causing a second entry screen. Investigate + report; fix if it's the clear cause (route to the single chamber/_Arena flow). If risky, report a plan.

## VERIFY
- `refresh_unity` + `read_console` → 0 compile errors. Re-trigger the crash repro → confirm NO MissingReferenceException.
- Run chamber/relevant smoke + EditMode tests; paste counts.
- Live probe: silhouette count/positions (5+5 on the two edges), front-facing sprite, walkableTrue, dummy name="Dummy"/HP-refills-doesn't-die, door at NE. Capture 1-2 Game-view screenshots + write absolute paths.

## COMMIT (after verified)
`fix(chamber): crash guard + remove blocking pedestals + 5+5 front-facing silhouettes + dummy select/immortal + door/dummy reposition`
Per-item status (DONE / BLOCKED / FOLLOW-UP) + commit hash + screenshot paths in CODEX_DONE.md.
