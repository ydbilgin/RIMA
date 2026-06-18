# DONE — Chamber Skill-Tryout Fix (skill bar + off-map safety)

Date: 2026-06-18 · SOLE Unity agent · No git commits · Chamber-only, additive (run-start empty-loadout + pedestal gate untouched)

## Changes (file:line)
`Assets/Scripts/UI/ChamberSelectBootstrap.cs`
- L100: new field `chamberSkillBar` (SkillBarUI ref).
- L999-1006 (SpawnPlayer): after `GrantPracticeLoadout`, call `ShowChamberSkillBar()` + `AttachChamberBoundsGuard(instance, spawn)`.
- New `ShowChamberSkillBar()`: hosts the standard gameplay `SkillBarUI` on the existing chamber overlay canvas (bottom-center, raised above the [G] prompt). Bar self-resolves the Player + class and populates Q/E/R/F. Torn down with the chamber scene → hidden on leave.
- New `AttachChamberBoundsGuard()`: derives walkable diamond center+radius (+1.0 margin) and a Y-floor from `builder.LastFloorCells`; attaches the guard to the player.
- New nested `ChamberBoundsGuard : MonoBehaviour` (FixedUpdate): if player leaves radius OR drops below yFloor → zero velocity + snap rigidbody/transform to spawn. ONLY added here → gameplay rooms unaffected.

## REAL verification (live Play, both classes)
- Compile: 0 errors (`error CS` count = 0); new members present in RIMA.Runtime (reflection-confirmed).
- Skill bar VISIBLE: `chamber_elementalist_after_snapback.png` clearly shows the 6-slot bar (LMB/RMB + Q/E/R/F icons). Data-proof: ChamberSkillBar active, Elementalist slots Q=Fireball E=Glacial Spike R=Chain Lightning F=Frozen Orb (all icon=yes); Warblade Q=Iron Charge E=Gravity Cleave R=Earthsplitter F=Cleave.
- OFF-MAP (the key test): REAL Iron Charge (Q) cast via `TryActivate()` from platform edge → Editor.log: `Off-map guard: player at (8.95, -1.56) snapped back to spawn (1.92, 6.14)`. Player ended ON-platform, vel=0. Also deterministic: forced to (50,50)/vel(30,30) → guard FixedUpdate → snapped to spawn (snapped=True). Elementalist forced (29.02,34.12) → snapped back too.
- Dummy hit (real cast path): Elementalist volley dummy 1000→960; Warblade volley 1000→923 (damaged=True). Player stayed on-platform after Iron Charge dash.
- Console/exceptions: read_console returned empty during Play (bridge quirk this session); Editor.log = authoritative → 0 exceptions / 0 nullref / 0 error CS in session tail.
- Debug state restored: playModeStartScene=MainMenu, SelectedClass=None, Time.timeScale=1, CharacterSelect scene isDirty=false, no leftover runtime objects, Play exited clean. (Temp playModeStartScene=CharacterSelect was used to enter chamber directly; restored.)

## Screenshots
`STAGING/_process/2026-06/chamber_test/` : chamber_elementalist_skillbar.png · chamber_elementalist_after_snapback.png (bar visible) · chamber_warblade_skillbar.png

## Notes
- SkillBarUI auto-binds via FindGameObjectWithTag("Player") + PlayerClassManager.PrimaryClass; re-attune re-grants kit and bar refreshes on OnPrimaryClassSet — no extra wiring needed.
- Guard uses robust radius/Y test (not perimeter colliders), per spec, fits the small diamond.

STATUS: PASS (real evidence).
