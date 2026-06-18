# Codex Handoff - Human-Like Playtest Sweep - 2026-06-18

## Session / Limit
- `functions.get_goal` returned no active goal and no remaining-token report: `goal=null`, `remainingTokens=null`.
- User asked to stop, leave a handoff note, and continue from another account.
- No commit was made.

## Starting Context
- `CURRENT_STATUS.md` said the active pending work was Warblade mount tuner / 8-way facing QC.
- `ANTIGRAVITY.md` was not present in repo root.
- Graphify graph path from AGENTS.md: `STAGING/_process/2026-06/graphify_fullmap/graphify-out/graph.json`.
- Useful graphify syntax:
  - `graphify query "what calls SkillDatabase" --graph STAGING/_process/2026-06/graphify_fullmap/graphify-out/graph.json`
  - `graphify query "how does DraftManager opening draft flow" --graph STAGING/_process/2026-06/graphify_fullmap/graphify-out/graph.json`

## Important Worktree State
Worktree was already dirty. Do not assume all changed files are from this Codex session.

Relevant files touched in this playtest/fix pass:
- `Assets/Scripts/Skills/SkillDatabase.cs`
- `Assets/Scripts/Skills/DraftManager.cs`
- `Assets/Scripts/Skills/SkillOfferGenerator.cs`
- `Assets/Scripts/UI/SkillBarUI.cs`
- `Assets/Scripts/UI/MainMenuController.cs`

Other dirty files seen in `git status --short` include Warblade/player prefabs, controllers, orientation/animation/mount scripts, `CURRENT_STATUS.md`, an editor tuner window, recovery scene, and a screenshot zip. Treat those as potentially pre-existing unless diff proves otherwise.

## Verified Before Stop
### Warblade 8-way facing / mount QC
Runtime controlled test passed by directly setting `PlayerController` private movement/facing fields and invoking animator/anchor updates.

Observed good cases:
- `W`: player facing north, visual north, idle north clip, weapon rotation 90, sorting -1.
- `W+D`: player facing NE, visual NE, idle NE clip, weapon rotation 45, sorting -1.
- `D`: player facing east, visual east, idle east clip, weapon rotation 0, sorting 1.
- Opposed inputs (`W+S`, `A+D`) produced zero speed and kept last valid facing without jitter.

Note: Synthetic `KeyboardState` input in Unity MCP was unreliable here. Keyboard controls showed pressed, but `moveAction.ReadValue<Vector2>()` stayed zero. Do not count that as a gameplay bug without manual confirmation.

### Reward flow after manual database repair
Before code fixes, a stale `SkillDatabase` caused opening draft problems. Manually destroying/recreating DB at runtime made opening skill draft work:
- Opening kit showed skill cards such as `Earthsplitter`, `Gravity Cleave`, `Iron Charge`.
- Selecting Iron Charge filled slot 0 and HUD Q icon after stepping Unity frames.
- Killing enemies spawned `RewardPickup`.
- `RewardPickup.ForceCollect()` opened normal reward draft with Warblade offers such as `Tempered Fury`, `Sunder Mark`, `Deep Wound`.

Unity frame advancement note:
- Shell sleep does not reliably advance Unity play frames in this MCP setup.
- Use `UnityEditor.EditorApplication.Step()` loops while paused for deterministic coroutine/UI verification.

## Bugs Found
### 1. MainMenu start button had no runtime listener
Fresh Play on MainMenu showed `Button_Basla` active/interactable but runtime `onClick` listener count was 0.
- `btn.onClick.Invoke()` did not change scene.
- Direct `MainMenuController.OnStartClicked()` did change to CharacterSelect.
- Reflection check was validated on a temporary test button, so listener count probe was meaningful.

Patch already applied:
- `Assets/Scripts/UI/MainMenuController.cs`
- In `AddNakedButton`, changed listener registration to clear first and add a lambda:
  - `button.onClick.RemoveAllListeners();`
  - `button.onClick.AddListener(() => onClick?.Invoke());`

Still needs fresh verification after compile/reload and frame stepping.

### 2. Chamber `SkillBarUI` had null slot refs despite child UI existing
In CharacterSelect chamber, Warblade practice loadout slots were populated internally:
- slot0 Iron Charge
- slot1 Gravity Cleave
- slot2 Earthsplitter
- slot3 Cleave

But `ChamberOverlayCanvas/ChamberSkillBar` displayed empty Q/E/R/F icon/name fields.
Root cause found:
- `SkillBarUI.slots[]` private refs were null (`root`, `icon`, `nameLabel`, etc.).
- Child objects existed (`Slot_LMB`, `Slot_RMB`, `Slot_Q`, `Slot_E`, `Slot_R`, `Slot_F`), but the component was not rebound to them.

Patch already applied:
- `Assets/Scripts/UI/SkillBarUI.cs`
- Added runtime slot reference recovery:
  - `Update()` calls `EnsureSlotReferences()`.
  - Existing child slots are scanned by name and rebound.
  - If children are missing, `BuildSlots()` is used.
  - `EnsureSkillDatabase()` now calls `SkillDatabase.EnsureRuntime()`.

Still needs fresh CharacterSelect verification.

### 3. Arena opening draft skipped / player started skill-less due stale `SkillDatabase`
In `_Arena`, after enough frame stepping:
- Player spawned and enemies were active.
- Q/E/R/F slots were all NULL.
- `DraftManager` state showed no active/pending draft and `openingDraftShown=true`.
- `SkillOfferUI` was absent.

Root cause found:
- `SkillDatabase_Auto` component existed and was enabled.
- `SkillDatabase.Instance == null`.
- Existing component private state was `built=true`, but private `db.Count=0`.
- `FindByName("Iron Charge")`, etc. returned null.
- Creating a temporary new `SkillDatabase` component produced a populated DB (`count=111`, first entry Iron Charge), proving data definitions were fine.
- `DraftManager.EnsureDependencies()` only created a DB when both `SkillDatabase.Instance == null` and no `SkillDatabase` component existed, so the stale empty component blocked repair.

Patches already applied:
- `Assets/Scripts/Skills/SkillDatabase.cs`
  - Added `EnsureRuntime()`.
  - `Awake()` now accepts/rebuilds usable instances and avoids keeping empty stale DBs.
  - Added `IsUsable()`.
  - `EnsureBuilt()` now tries to rebuild when `db.Count == 0`.
- `Assets/Scripts/Skills/DraftManager.cs`
  - Opening kit paths and dependency setup now use `SkillDatabase.EnsureRuntime()`.
- `Assets/Scripts/Skills/SkillOfferGenerator.cs`
  - Pool source now uses `SkillDatabase.EnsureRuntime()`.

Critical unfinished patch note:
- I caught one issue just before the user asked to stop.
- `SkillDatabase.EnsureBuilt()` can request rebuild when `db.Count == 0`, but `Build()` still likely has an old early return like `if (built) return;`.
- Next account should patch `Build()` before retesting:
  - preferred guard: `if (built && db.Count > 0) return;`
  - or set `built = false` before calling `Build()` from `EnsureBuilt()`.
- Without this, stale `built=true/db.Count=0` databases may still fail to rebuild.

## Next Account Checklist
1. Patch `SkillDatabase.Build()` guard so `built=true/db.Count=0` can rebuild.
2. Request Unity refresh/compile and wait until ready.
3. Clear console and verify no compile errors.
4. Fresh Play MainMenu:
   - step about 10 frames with `EditorApplication.Step()`;
   - verify `Button_Basla` listener count is >0;
   - invoke button and verify scene changes to `CharacterSelect`.
5. In CharacterSelect chamber:
   - step frames until chamber UI is ready;
   - verify `SkillBarUI.slots[]` refs are non-null;
   - verify Q/E/R/F icons/names match loaded Warblade practice skills.
6. Start run / enter `_Arena`:
   - verify `SkillDatabase.Instance` is non-null and usable (`GetAll().Count > 0` or equivalent);
   - verify opening draft appears automatically with skill cards, not gold/heal fallback;
   - select Iron Charge or another skill card by button invoke;
   - step frames and verify draft closes and Q slot fills.
7. Kill all active enemies:
   - two HalfThralls plus one FractureImp were seen before;
   - step frames until clear slow-mo resolves;
   - verify `RewardPickup` spawns;
   - collect reward and verify reward draft opens.
8. Capture Unity MCP screenshots at MainMenu, CharacterSelect chamber, opening draft, and reward draft.
9. Run final console check.
10. Review `git diff --` only for relevant scripts; do not revert unrelated dirty work.

## Useful Unity MCP Notes
- `manage_camera` supports screenshots; use `include_image=false` for file evidence or `include_image=true` for quick inspection.
- For frame stepping in Play Mode, execute code similar to:
```csharp
UnityEditor.EditorApplication.isPaused = true;
for (int i = 0; i < 60; i++) UnityEditor.EditorApplication.Step();
UnityEditor.EditorApplication.isPaused = false;
return UnityEngine.Time.frameCount.ToString();
```
- For console:
  - `read_console` with `types=["error","warning"]`.
- For compile:
  - `refresh_unity` with `scope="scripts"`, `compile="request"`, `wait_for_ready=true`.

## Do Not Report As Final Yet
The playtest sweep is not complete.
MainMenu, SkillBarUI, and opening draft patches need a clean compile plus fresh Play Mode verification.

## Deep Loop Test Analysis
This is the deeper test plan requested after the initial smoke/root-cause pass. The short checklist above only closes the fixes already found. The actual demo loop needs a broader matrix.

### Flow Map From Graphify
Graphify queries pointed to these runtime hubs:
- Entry / character flow: `MainMenuController`, `CharacterSelectScreen`, `ChamberSelectBootstrap`.
- Chamber skill display: `SkillBarUI`, `PlayerSkillController`, `PlayerClassManager`.
- Draft / offer flow: `DraftManager`, `SkillDatabase`, `SkillOfferGenerator`, `SkillOfferUI`.
- Room and reward flow: `RuntimeRoomManager`, `RoomLoader`, `RewardPickup`, `RoomClearVictoryTrigger`.
- Run-map style flow also touches `RoomRunDirector`, `MapPanelUI`, and room transition routines.

### Test Layers
1. Code health gate
   - Run script compile/refresh.
   - Console must have no new errors/warnings before playtest.
   - If errors exist, stop and fix compile/runtime errors first.

2. UI entry gate
   - Start from fresh `MainMenu`.
   - Use real UI button click when possible; if MCP click is unreliable, use `Button.onClick.Invoke()` and state that limitation.
   - Evidence:
     - `Button_Basla` active/interactable.
     - Runtime listener count > 0.
     - Scene changes from `MainMenu` to `CharacterSelect`.
   - Failure class caught:
     - Dead buttons, missing listeners, bad scene loading, UI not initialized after domain reload.

3. Character selection / chamber gate
   - Pick Warblade in `CharacterSelect`.
   - Verify the player preview/chamber state and practice loadout.
   - Evidence:
     - Selected class is Warblade in `PlayerClassManager`.
     - `PlayerSkillController` has expected active slots.
     - `SkillBarUI.slots[]` child refs are non-null.
     - Q/E/R/F icons and names are visible in screenshot.
   - Failure class caught:
     - SkillBar lost references, wrong class loadout, stale UI, missing icon registry.

4. Arena first-entry gate
   - Enter `_Arena` from the normal start/rift path.
   - Verify database and draft before selecting anything.
   - Evidence:
     - `SkillDatabase.Instance != null`.
     - DB usable count > 0.
     - `DraftManager` has active opening draft.
     - `SkillOfferUI` visible.
     - Cards are skill offers, not fallback gold/heal rewards.
   - Failure class caught:
     - Stale empty `SkillDatabase`, opening draft skipped, draft active flag mismatch, wrong offer source.

5. Opening skill selection gate
   - Select one opening skill card, preferably Iron Charge for Warblade.
   - Step frames after click/invoke.
   - Evidence:
     - Draft closes.
     - Time scale returns to normal.
     - Q slot receives selected skill.
     - Skill icon visible on HUD.
   - Failure class caught:
     - Button listener dead, draft not closing, slot assignment failure, timeScale stuck.

6. Combat usability gate
   - Move in 8 directions.
   - Use selected skill on enemies.
   - For Warblade, explicitly check W -> W+D -> W again.
   - Evidence:
     - Body and weapon face together.
     - Skill fires only when allowed.
     - Cooldown starts and countdown renders.
     - No console errors.
   - Failure class caught:
     - Facing desync, input lock, cooldown UI drift, failed-cast regression.

7. Room clear / reward gate
   - Kill all active enemies through normal combat if feasible; forced damage is acceptable as a secondary verifier.
   - Step enough frames for clear sequence and slow-mo recovery.
   - Evidence:
     - All enemies dead.
     - Clear sequence completes.
     - `Time.timeScale == 1`.
     - `RewardPickup` exists and is interactable.
   - Failure class caught:
     - Room clear never fires, slow-mo stuck, reward missing, enemy counter mismatch.

8. Reward draft gate
   - Collect reward pickup.
   - Verify reward draft opens and can be resolved.
   - Evidence:
     - `SkillOfferUI` visible in reward mode.
     - Offers make sense for current class/run state.
     - Taking a reward updates slot/passive/run state.
     - UI closes cleanly.
   - Failure class caught:
     - Reward collect double-trigger, bad draft state reuse, duplicate/empty offers, UI stuck.

9. Multi-loop endurance gate
   - Continue for at least two reward loops in the same run.
   - If the next room path is not fully available, use the closest normal transition method and document the shortcut.
   - Evidence:
     - Skills persist between rooms.
     - Rewards do not reset class/HUD incorrectly.
     - Draft flags reset between drafts.
     - Console remains clean.
   - Failure class caught:
     - One-shot initialization bugs, stale singleton state, draft flags not reset, reward/room transition desync.

10. Reset/death gate
   - Test death or run reset if available.
   - Return to menu/selection/start a new run.
   - Evidence:
     - Old draft UI gone.
     - Old reward pickup gone.
     - SkillDatabase remains usable.
     - Fresh run gets fresh opening draft.
   - Failure class caught:
     - Persistent singleton pollution, leaked UI, stale player state after restart.

11. Class smoke matrix
   - Warblade: full path.
   - Elementalist, Ranger, Shadowblade: CharacterSelect -> Arena -> opening draft -> select first skill -> HUD slot visible.
   - Evidence:
     - No class has missing controller/HUD/draft data.
   - Failure class caught:
     - Warblade-only fix hiding cross-class breakage.

### Reporting Format For Next Agent
For each phase, report:
- `PASS/FAIL/BLOCKED`.
- Exact scene and frame count.
- Screenshot path if visual.
- Console error/warning count.
- Runtime state values that prove the claim.
- If a shortcut was used, name it clearly, e.g. `Button.onClick.Invoke()` instead of physical click.

### Priority
If time is short:
1. MainMenu -> CharacterSelect -> Arena opening draft -> skill select -> room clear -> reward draft.
2. Two-loop endurance.
3. Reset/death.
4. Non-Warblade class smoke.
