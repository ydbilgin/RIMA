# DONE — Attunement Chamber Pedestal Lock (demo safety)

Date: 2026-06-18 · Sole Unity agent · NO git commits · Console: 0 Error / 0 Warning

## Goal
Prevent the jury from selecting a non-playable class in the Attunement Chamber (class-select),
which would launch a broken/empty live run and contradict the report demo.

## Discovered mechanism
The class-selection flow has TWO surfaces, both attached to the same GameObject in scene
`Assets/Resources/ChamberSelect/Chamber_CharSelect.asset` (authored scene only hosts the classic
screen; `CharacterSelectScreen.Awake` auto-attaches `ChamberSelectBootstrap`):

1. **World pedestals** — `ChamberSelectBootstrap.cs` builds 10 "EchoStation" statues (one per
   `ChamberClasses` entry: Warblade, Elementalist, Shadowblade, Ranger, Ravager, Ronin, Gunslinger,
   Brawler, Summoner, Hexer). The walk-up [G] interaction in `Update()` is gated by
   `IsDemoSelectable(cls)` (Warblade + Elementalist only). Non-selectable pedestals already showed a
   locked prompt and `[G]` did nothing, and `RefreshEchoVisuals()` already dims them. This surface
   was already demo-safe.

2. **Classic overlay** — `CharacterSelectScreen.cs` (TAB fallback + training-dummy popup). The
   dummy-popup path (`AcceptClassicSelectionFromPopup`) was gated by `IsDemoSelectable`, but the
   **TAB-overlay `OnStartRun()` path was NOT** — it only checked `ClassUnlockPolicy.IsUnlocked`,
   which is the *echo-purchase economy* gate, not a playability gate.

How "playable" was marked: `ClassUnlockPolicy.IsUnlocked` = Warblade/Elementalist always true +
PlayerPrefs echo-purchase for Ravager/Ronin/Gunslinger/Brawler/Summoner/Hexer (cost>0). Ranger and
Shadowblade have cost 0 → never purchasable there. There was NO single "demo-playable" flag; the
chamber hard-coded its own `Warblade||Elementalist` check.

### The real leak (confirmed)
Live data-proof showed this dev environment already has PlayerPrefs unlocks:
`IsUnlocked TRUE (default) = Warblade, Elementalist, Ravager, Gunslinger, Brawler, Summoner, Hexer`.
So via the TAB overlay, `OnStartRun()` would have passed `IsUnlocked` for the controller-less
Ravager/Gunslinger/Brawler/Summoner/Hexer and loaded `_Arena` with a broken class. This was a real,
not theoretical, leak.

## Decision — locked vs selectable
- **SELECTABLE: Warblade, Elementalist** — confirmed end-to-end playable (matches the existing
  demo lock and report).
- **LOCKED: Ranger, Shadowblade** — they have controllers but end-to-end could not be confirmed in
  scope; the existing demo lock already excluded them. Per task guidance ("if uncertain, LOCK —
  safer for demo"), kept locked.
- **LOCKED: Ronin, Ravager, Gunslinger, Brawler, Summoner, Hexer** — controller-less, MUST lock
  (the active leak). Locked.

Net: exactly 2 selectable, 8 locked.

## Implementation (surgical, reversible — no pedestals deleted, no class-select flow altered)
Single source of truth added; all gates routed through it.

1. `Assets/Scripts/Systems/ClassUnlockPolicy.cs` (~+9 lines)
   - Added `public static bool IsDemoPlayable(ClassType cls)` → `Warblade || Elementalist`.
     Canonical "safe to play in demo" gate, separate from the unchanged `IsUnlocked` economy.

2. `Assets/Scripts/UI/ChamberSelectBootstrap.cs` (2 edits, ~2 lines net)
   - `IsDemoSelectable` now delegates to `ClassUnlockPolicy.IsDemoPlayable` (behavior identical,
     now centralized so the two surfaces can never drift).
   - Locked world-pedestal prompt now reads `"{CLASS} — GELİŞTİRME AŞAMASINDA"`
     (was `"{CLASS} — Kilitli"`), via `Loc.T("char_select.in_development")`.

3. `Assets/Scripts/UI/CharacterSelectScreen.cs` (3 edits)
   - **Leak close:** `OnStartRun()` — after the chamber-popup delegation, added a hard gate:
     `if (!ClassUnlockPolicy.IsDemoPlayable(selectedClass)) { warn; return; }` BEFORE the
     `IsUnlocked`/run-start logic. A non-demo class can no longer launch `_Arena`.
   - Per-card lock label: demo-locked classes now show `char_select.in_development` and the
     echo cost line is suppressed (so the jury isn't invited to buy a broken class).
   - Start/action button: non-demo-playable classes are non-interactable and the button reads
     "GELİŞTİRME AŞAMASINDA" instead of "UNLOCK — N SHATTERED ECHO".

4. `Assets/Scripts/Core/Loc.cs` (2 edits)
   - New key `char_select.in_development`: TR "GELİŞTİRME AŞAMASINDA" / EN "IN DEVELOPMENT".

The safe-class selection/play path (`IsDemoSelectable` true → "Bürün"/confirm → `AttuneRoutine` /
`StartRun` / `SetPrimaryClass`) was not touched.

## Verification
- `refresh_unity` (compile request) → polled to `idle`/ready. `read_console` (Error+Warning) = **0/0**
  (before and after the data-proof run).
- Data-proof via `execute_code` (pure static logic — NO scene objects created, no leak):
  - `IsDemoPlayable TRUE for: Warblade,Elementalist`
  - `Chamber IsDemoSelectable == policy for all 10: True`
  - `Loc[char_select.in_development]='GELİŞTİRME AŞAMASINDA' empty=False`
  - `IsUnlocked TRUE (default): Warblade,Elementalist,Ravager,Gunslinger,Brawler,Summoner,Hexer`
    → proves the leak existed (5 controller-less classes were already "unlocked") and that
    `IsDemoPlayable` correctly excludes them.
  - `Non-demo-playable (locked) count: 8 / expected 8`
- Scene untouched; no temp objects to clean up.

## BLOCKED / notes
- None blocked.
- Pre-existing (not changed): the existing chamber unit tests in
  `Assets/Tests/EditMode/Chamber/ChamberRobustnessTests.cs` assert exactly 2 demo-selectable classes
  via reflection on `IsDemoSelectable` — still holds (delegates to the same result).
- Reversible: delete `IsDemoPlayable` and revert the 4 files' edits to restore prior behavior.
- The dev PlayerPrefs unlocks (Ravager/Gunslinger/etc.) are environment state, not code — left as-is.
