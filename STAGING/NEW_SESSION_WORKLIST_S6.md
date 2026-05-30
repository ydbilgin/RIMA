# RIMA — NEW-SESSION WORKLIST (S6 → next) — from user F5 playtest + Unity MCP console

> Built 2026-05-30 after the user actually played (death screen screenshot, room 1, died in 9s, 0 kills) and reported
> issues. Unity MCP was REACHABLE and console-read worked. **This session = NOTE only; execute all of this next session.**
> Pickup order: P0 console bugs → P1 playtest/UX → P2 conflicts → NLM (ax research in flight) → backlog.

---

## 🔴 P0 — Unity console errors (root-caused via MCP, exact fixes ready)

### P0-1 — EventSystem uses the OLD input module (breaks ALL UI clicks + spams console every frame)
- **Error (spamming):** `InvalidOperationException: You are trying to read Input using the UnityEngine.Input class,
  but you have switched active Input handling to Input System package` — UGUI `BaseInput.cs:134`, ~every frame.
- **Root cause:** two self-built EventSystems add the legacy `StandaloneInputModule`:
  - `Assets/Scripts/Core/DeathScreenManager.cs:393` → `go.AddComponent<StandaloneInputModule>();`
  - `Assets/Scripts/Core/DemoCompleteOverlay.cs:214` → `go.AddComponent<StandaloneInputModule>();`
  - (`MainMenuScreen.cs:95` is CORRECT — uses `InputSystemUIInputModule`.)
- **Impact:** the death + victory screen buttons (TRY AGAIN / COPY BUILD SEED / WISHLIST / MAIN MENU) may not click;
  console floods. This is what the user saw on the death screen.
- **FIX (2 lines):** in both files replace `StandaloneInputModule` → `InputSystemUIInputModule`
  (`using UnityEngine.InputSystem.UI;`). Also guard: if an EventSystem already exists, don't add a second.
  Consider a shared `UiEventSystem.Ensure()` helper so all overlays use one correct EventSystem.

### P0-2 — RoomMonologController auto-GO has no Canvas (boss/room monologs throw)
- **Error:** `MissingComponentException: There is no 'Canvas' attached to "RoomMonologController_Auto"` — `RoomMonologController.cs:50`.
- **Root cause:** `EnsureInstance()` (line 49-51) creates a bare `new GameObject(...)` + `AddComponent<RoomMonologController>()`;
  the script then tries to access a Canvas that isn't there.
- **Impact:** every boss/room monolog (`RoomMonologController.Say(...)` — incl. the new Phase-2/3 boss lines) throws.
- **FIX:** in `EnsureInstance()` build a proper canvas on the auto-GO (Canvas + CanvasScaler + GraphicRaycaster, overlay,
  high sortingOrder) BEFORE/така the controller reads it; or make the controller `RequireComponent`/self-add the Canvas in Awake.

---

## 🟠 P1 — Playtest / UX issues (from the user's F5 run)

### P1-1 — No boot flow: no intro / character-select screen
- Demo drops straight into gameplay; no Main Menu → Character Select → arena like a real game.
- Assets exist (`MainMenuScreen.cs`, `CharacterSelectScreen.cs`), but the boot scene/flow skips them (F5 dev-tool opens
  PlayableArena directly; build-settings first scene likely the arena).
- **FIX (next session):** make boot = MainMenu → CharacterSelect → PlayableArena. Set build-settings scene 0 = MainMenu;
  verify CharacterSelect.gameSceneName → PlayableArena_Test01 (already set). Keep F5 as a dev shortcut that bypasses.

### P1-2 — Audio is meaningless ("sesler manasız")
- AudioManager procedural SFX sound random/unpleasant.
- **Decision (DECISIONS_S6, ax-AGREE):** real audio is produced AFTER animation (SFX must lock to anim frames).
- **FIX now-ish:** either (a) MUTE/disable the procedural SFX for the demo (better silence than bad noise) or
  (b) tune the procedural set down to a few tasteful cues (hit/pickup/gate). Decide next session; lean toward (a) mute
  until real SFX. Keep the AudioManager hooks.

### P1-3 — Player dies in Room 1 in ~9s with 0 kills
- Build: "Warblade + 0 skills", Room 1, Time 00:09, Kills 0 → the player couldn't fight / was overwhelmed instantly.
- Suspects: spawn count/aggression too high for R1; the kinematic-shove drift regressed; attack not connecting;
  or the player can't read what to do (no telegraphtelegraph clarity). FractureImp HP was just dropped 100→60 (should help).
- **FIX (next session):** F5 playtest R1, check: does LMB attack land? are mobs too many/aggressive for a first room?
  Apply the design's R1 = 3 FractureImp single wave, winnable first try. Verify Player(10)/Enemy(11) layer split still holds.

### P1-4 — Red squares + red circles on screen (placeholder/visual audit)
- Death-screen bg shows red squares near the player + 2 red circles around it.
- **Likely:** red squares = graybox mobs (expected placeholder art); red circles = enemy attack telegraphs
  (EnemyTelegraph default is warm red-orange `(1,0.18,0.08)` — BY DESIGN), frozen on the death screen.
- **ACTION (next session):** confirm via play it's just graybox + warm telegraphs (working), NOT a debug-gizmo leak.
  If graybox reads too rough → that's the gated art pass (archive-restore mobs per DECISIONS_S6). Telegraph color is intended.

---

## 🟡 P2 — Conflicts (lint, note only)

### P2-1 — Boss phase wording (direction-lock stale vs implemented "2+1")
- `RIMA_DIRECTION_LOCK_S6.md` §2 (line 15) still says *"50%-HP single placeholder kill phase only (full 3-phase deferred)"*
  + line 41 lists "full 3-phase boss" as DEFERRED.
- But this session shipped the **3-beat "2+1" boss** (Phase-3 overlay @33%, commit `ab96137f`), per the later
  `DESIGN_LOCK_DEMO_S6.md` §1.3 + user brief (which supersede §2). `BOSS_MOB_DESIGN_S6.md` §0 documents the supersede.
- **FIX:** update direction-lock §2 to "2+1" (50% chains-break + lightweight 33% overlay; full raid-boss still deferred).
- Other lint: clean (skill-count 4 = CONTROL_SCHEME §7 OK; cyan budget consistent; status current).

### P2-2 — Resolved-but-flagged (no action unless user wants)
- FractureImp HP 100→60 (applied) — playtest-tunable. Boss-death→Victory route (suppressClassSelectOnDeath=true, verify in scene).

---

## 🔵 NLM — broken again (ax research IN FLIGHT)
- NLM (notebooklm-mcp-cli) broke again like before; full-reset fix didn't durably hold.
- **ax research dispatched** (`STAGING/REVIEW_NLM_RECOVERY_AX.md`): root cause of recurring expiry + a DURABLE fix
  (refresh-token persistence / alt auth / version pin / keepalive / or reduce hard dependency by snapshotting canon to
  local md). Fold its findings next session; user likely needs to run `! nlm login` (full reset) to unblock meanwhile.

---

## ▶ EXECUTION ORDER (next session)
1. **P0-1 + P0-2** (console bugs — trivial, unblocks UI + monologs; do FIRST so F5 is clean).
2. **P1-3** (R1 difficulty — make the demo winnable) + **P1-1** (boot flow menu→select→arena).
3. **P1-2** (mute/tune procedural audio).
4. **P2-1** (direction-lock §2 update) + fold **NLM** ax research.
5. **P1-4** visual audit + then the gated art/scene/F5 passes.
All: dotnet green + writer≠reviewer (ax/cx) + commit. Then the remaining AUTONOMOUS_BACKLOG_S6 tracks.
