# Detailed Findings

Severity terminology follows the original review:

- **WOULD-BREAK-A-LIVE-DEMO**
- **COSMETIC**
- **NON-ISSUE-OR-FUTURE**

---

## RIMA-001 — Conflicting `Time.timeScale` ownership

**Severity:** WOULD-BREAK-A-LIVE-DEMO  
**Confidence:** CONFIRMED  
**Primary source:** `Assets/Scripts/UI/DirectorMode.cs`  
**Relevant methods:** `SetState`, `ResolveTimeScaleForState`  
**Related source:** `Assets/Scripts/UI/UIManager.cs`, `Assets/Scripts/UI/BuildModeController.cs`

### Evidence

`UIManager` describes itself as the central UI state and time-scale controller, but `DirectorMode.SetState` independently writes `Time.timeScale`.

When Director Mode changes to `Test`, its resolver generally returns `1f` unless the death screen is active. It does not account for:

- skill offer open,
- pause menu open,
- settings open,
- skill codex open,
- another modal owner.

Build Mode exits by restoring the captured Director state, which can invoke this resume path.

### Failure scenario

1. A skill draft is open and `UIManager` has paused time.
2. Build Mode or Director Mode changes state.
3. `DirectorMode.SetState(Test)` assigns `Time.timeScale = 1`.
4. Draft remains visible and logically active.
5. Combat resumes behind the modal.

Equivalent races exist for pause/settings/codex.

### Recommended fix

**Preferred:** introduce a single time-scale arbiter with explicit reasons, for example:

```csharp
public enum PauseReason
{
    SkillOffer,
    PauseMenu,
    Settings,
    SkillCodex,
    DirectorMode,
    BuildMode,
    Death,
    MainMenu
}
```

Each system registers/releases a reason. The arbiter computes the effective scale.

**Minimum demo-safe alternative:**

- `DirectorMode` may force `0` when entering Director state.
- When leaving Director state, it must not directly force `1`.
- It must ask the central UI/death-state owner to recompute the correct scale.
- `UIManager` should expose a safe `RefreshTimeScale()` or `GetDesiredTimeScale()`.
- Any tab slow-motion behavior (`0.1f`) must remain intact.

### Required tests

- Draft open + Director enter/exit → final time scale remains `0`.
- Pause open + Build enter attempt → Build is blocked or coordinated; time remains `0`.
- No overlay + Director exit → final time scale is `1`.
- Death active + Director exit → final time scale remains `0`.
- Tab overlay + Director exit → final time scale is `0.1`, if TAB behavior is still intended.

---

## RIMA-002 — Director Mode may never bootstrap after normal menu entry

**Severity:** WOULD-BREAK-A-LIVE-DEMO  
**Confidence:** SUSPECTED, strongly supported by source  
**Primary source:** `Assets/Scripts/UI/DirectorMode.cs`  
**Relevant method:** `Bootstrap`  
**Related source:** `Assets/Scripts/UI/BuildModeController.cs`, `EnterBuildMode`

### Evidence

`DirectorMode.Bootstrap` runs `AfterSceneLoad` and intentionally returns when the initial active scene is `MainMenu` or `CharacterSelect`.

The source comment states this prevents the developer overlay from appearing during the real game-entry flow. However, no reviewed code showed a later scene-load hook that creates `DirectorMode` upon entering gameplay.

`BuildModeController` bootstraps independently and refuses to enter Build Mode when `DirectorMode.Instance == null`.

### Failure scenario

1. Launch game in `MainMenu`.
2. Move through `CharacterSelect`.
3. Enter `_Arena`.
4. Press F2.
5. `BuildModeController` exists, but `DirectorMode.Instance` is null.
6. Build Mode logs a warning and does not open.

Direct `_Arena` Play Mode may still work, hiding the defect during development.

### Recommended fix

Preserve clean menu flow, but create `DirectorMode` on the first eligible gameplay scene.

Possible approaches:

- subscribe once to `SceneManager.sceneLoaded`,
- use a gameplay-scene eligibility predicate,
- create lazily when Build Mode is requested in an eligible scene,
- never create in menu/character-selection scenes,
- ensure only one instance survives with domain reload disabled.

### Required tests

- Start in `MainMenu`, load `CharacterSelect`, then `_Arena`: `DirectorMode.Instance != null`.
- Start directly in `_Arena`: exactly one instance.
- Return to menu: overlay remains hidden or is safely disabled according to intended design.
- Re-enter gameplay: no duplicate instance.
- Disable Domain Reload: stale static references are recovered.

---

## RIMA-003 — Build Mode can hide active modal UI without resolving its state

**Severity:** WOULD-BREAK-A-LIVE-DEMO  
**Confidence:** CONFIRMED  
**Primary source:** `Assets/Scripts/UI/BuildModeController.cs`  
**Relevant methods:** `Update`, `Toggle`, `EnterBuildMode`, `HideOtherUiCanvases`  
**Related source:** `Assets/Scripts/UI/UIManager.cs`, `Assets/Scripts/Skills/DraftManager.cs`

### Evidence

Build Mode input does not check whether a draft, pause screen, settings screen, codex, or another modal is active.

On entry, it hides every other enabled root canvas except its own Build Mode canvases. Hiding a canvas does not clear:

- `UIManager` flags,
- `DraftManager.IsDraftActive`,
- modal callbacks,
- unscaled timeout coroutines,
- pending selections.

### Failure scenario

1. Draft is active.
2. Player presses F2.
3. Draft canvas becomes disabled and invisible.
4. Draft state remains active and time remains paused or later becomes incorrectly resumed.
5. Build Mode appears over a hidden unresolved modal.
6. Exiting Build Mode restores the canvas, possibly after timeout logic has already changed room state.

### Recommended fix

Safest demo behavior: **block Build Mode entry while any modal is active.**

Guard with all relevant authorities:

- `UIManager.Instance?.IsAnyOverlayOpen`
- `DraftManager.Instance?.IsDraftActive`
- `DraftManager.Instance?.IsDraftPending`
- death screen state
- scene-transition state, if applicable.

Do not silently close a draft from F2 unless product design explicitly requires it.

Build Mode should display a concise status/log message explaining why entry was rejected.

### Required tests

- Draft active → F2 does not enter Build Mode.
- Draft pending → F2 does not enter Build Mode.
- Pause/settings/codex active → F2 does not enter Build Mode.
- No modal → F2 enters Build Mode.
- Rejected entry does not alter camera, canvases, player active state, or time scale.

---

## RIMA-004 — Room-clear delayed draft is not serialized

**Severity:** WOULD-BREAK-A-LIVE-DEMO  
**Confidence:** SUSPECTED, highly plausible  
**Primary source:** `Assets/Scripts/Skills/DraftManager.cs`  
**Relevant methods:** `HandleRoomCleared`, `ShowDraftDelayed`, `TriggerDraftFromFragment`, `ShowDraft`

### Evidence

`HandleRoomCleared` starts `ShowDraftDelayed(RoomClearDraftDelay)` but does not:

- set `IsDraftPending`,
- retain the coroutine handle,
- prevent another delayed draft,
- cancel the pending draft when a portal/reward draft opens.

`IsDraftPending` is only raised in the secondary-class event path.

### Failure scenario

1. Room clear schedules a draft for two seconds later.
2. Before the delay expires, reward collection or portal logic opens a draft.
3. First draft is active.
4. Delayed coroutine wakes and calls `ShowDraft` again.
5. UI is rebuilt and callback state can be replaced.
6. Selection resolves the wrong draft or leaves the room flow waiting.

A duplicate room-clear event can produce the same outcome.

### Recommended fix

Create one serialized draft queue/slot:

```csharp
private Coroutine delayedDraftRoutine;
private int draftGeneration;
```

Required behavior:

- starting a delayed draft sets `IsDraftPending = true`,
- only one delayed draft may exist,
- immediate draft entry either cancels the pending timer or consumes it,
- `ShowDraft` refuses unsafe re-entry unless explicitly replacing the same request,
- `HideDraft`, `OnDisable`, and run reset clear pending state,
- use unscaled time,
- use a request token/reason if multiple draft sources exist.

Consider a small internal enum:

```csharp
enum DraftRequestSource
{
    OpeningKit,
    RoomClear,
    SecondaryUnlock,
    Reward,
    Portal
}
```

### Required tests

- Two room-clear events → one draft.
- Room-clear pending + reward draft → one visible draft and one callback.
- Room-clear pending + portal draft → one visible draft.
- Disable manager during delay → no later draft.
- Pending flag is true during the entire delay.
- Pending flag is false after cancellation or opening.

---

## RIMA-005 — Anonymous secondary-class event subscription leaks

**Severity:** WOULD-BREAK-A-LIVE-DEMO  
**Confidence:** CONFIRMED  
**Primary source:** `Assets/Scripts/Skills/DraftManager.cs`  
**Relevant methods:** `Start`, `OnDisable`

### Evidence

`Start` subscribes to:

```csharp
PlayerClassManager.Instance.OnSecondaryClassSelected += _ => { ... };
```

The anonymous delegate is never retained, so it cannot be removed. `OnDisable` only removes `RoomLoader` listeners.

There is also a lifecycle gap: if `PlayerClassManager.Instance` is null during `Start`, this manager never subscribes later.

### Failure scenario

1. DraftManager is destroyed/recreated or disabled/re-enabled while `PlayerClassManager` persists.
2. Old delegate remains attached.
3. Secondary class selection fires multiple callbacks.
4. Multiple delayed unlock drafts are scheduled.
5. Stale callbacks may target destroyed Unity objects.

### Recommended fix

Replace the lambda with a named method and symmetric hook state:

```csharp
private bool secondaryClassHooked;

private void TryHookSecondaryClass()
{
    if (secondaryClassHooked) return;
    var manager = PlayerClassManager.Instance;
    if (manager == null) return;
    manager.OnSecondaryClassSelected += HandleSecondaryClassSelected;
    secondaryClassHooked = true;
}

private void UnhookSecondaryClass()
{
    if (!secondaryClassHooked) return;
    var manager = PlayerClassManager.Instance;
    if (manager != null)
        manager.OnSecondaryClassSelected -= HandleSecondaryClassSelected;
    secondaryClassHooked = false;
}
```

Hook in `OnEnable` and retry when dependencies become available. Unhook in `OnDisable`/`OnDestroy`.

The handler should use the shared serialized draft-request path from RIMA-004.

### Required tests

- Disable/enable DraftManager repeatedly → one callback.
- Destroy/recreate DraftManager → one callback.
- PlayerClassManager created after DraftManager Start → hook eventually succeeds.
- Secondary class selected twice intentionally → exactly two logical requests, never accumulated duplicates.

---

## RIMA-006 — Reward timeout opens doors while draft may remain active

**Severity:** WOULD-BREAK-A-LIVE-DEMO  
**Confidence:** CONFIRMED  
**Primary source:** `Assets/Scripts/Core/RewardPickup.cs`  
**Relevant method:** `DraftThenOpenExit`

### Evidence

The coroutine waits while:

```csharp
draft.IsDraftActive && guard < 90f
```

After 90 seconds, it exits the loop and opens doors. It does not:

- call `draft.HideDraft()`,
- clear the current draft,
- resolve a reward,
- ensure `UIManager` closes the skill offer,
- restore time scale.

### Failure scenario

1. Player ignores reward draft.
2. Guard reaches 90 seconds using unscaled time.
3. Exit doors open.
4. Draft remains active and visible.
5. `UIManager` still considers the skill offer open and keeps time paused.
6. The room is technically open but the player cannot proceed.

### Recommended fix

Define one explicit timeout policy.

**Best demo policy:** deterministically resolve the draft:

- choose the first valid offer, or
- choose a documented fallback reward,
- invoke normal selection flow,
- close UI,
- clear active/pending state,
- then open doors.

**Acceptable emergency policy:** close/cancel the draft explicitly, ask `UIManager` to recompute time scale, then open doors. This discards the reward and should be logged loudly.

Do not open doors while `IsDraftActive` remains true.

### Required tests

- Timeout closes/resolves draft.
- `UIManager.IsSkillOfferOpen == false` after timeout.
- `DraftManager.IsDraftActive == false` after timeout.
- Exit doors open only after draft is inactive.
- Final time scale matches remaining modal/death state.
- Manual selection before timeout still follows normal path.

---

## RIMA-007 — Opening draft coroutine may survive same-scene run restart

**Severity:** WOULD-BREAK-A-LIVE-DEMO  
**Confidence:** SUSPECTED  
**Primary source:** `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs`  
**Relevant methods:** `BeginRun`, `OpeningKitDraftSequence`, `StopClearSequences`

### Evidence

`BeginRun` resets `openingDraftShown` and starts `OpeningKitDraftSequence`, but the reviewed fields only clearly retain `clearSequence` and `slowMoSequence`.

If `BeginRun` is called again before the opening sequence completes, the previous coroutine can remain alive unless `StopClearSequences` also stops all coroutines or specifically includes this sequence. That must be verified in the current complete source.

### Failure scenario

1. Run begins and opening draft sequence waits.
2. Developer restarts the run in the same scene.
3. New opening sequence starts.
4. Old sequence wakes later.
5. It opens or force-resolves an outdated draft in the new run.

### Recommended fix

Verify `StopClearSequences`.

If opening draft is not included:

- add `Coroutine openingDraftSequence`,
- stop and clear it at the start of `BeginRun`,
- clear it when the routine completes,
- stop it in `OnDisable`/`OnDestroy`,
- optionally use a run-generation token so stale routines self-abort.

### Required tests

- Call `BeginRun` twice before one frame passes → one opening draft.
- Restart while draft active → old routine cannot reopen it.
- Restart during prior-draft wait → only current run may proceed.
- Disable director → no delayed opening draft.

---

## RIMA-008 — Build tool hotkeys fire while typing in search input

**Severity:** COSMETIC, with authoring-data risk  
**Confidence:** CONFIRMED by input structure  
**Primary source:** `Assets/Scripts/UI/BuildMode/BuildPlacementController.cs`  
**Relevant methods:** `Update`, `HandleKeyboard`

### Evidence

Build Mode has a `TMP_InputField searchField`, but `HandleKeyboard` processes:

- Ctrl+Z / Ctrl+Y,
- brackets,
- F,
- E,

without checking whether a text input currently owns focus.

### Failure scenario

Typing a search such as “fence” can:

- flip a prop on `F`,
- eyedrop on `E`,
- undo placement with Ctrl+Z,
- rotate with brackets.

### Recommended fix

Before tool hotkeys:

```csharp
GameObject selected = EventSystem.current?.currentSelectedGameObject;
if (selected != null && selected.GetComponentInParent<TMP_InputField>() != null)
    return;
```

Alternatively check `searchField.isFocused`.

Mouse placement must remain guarded by `IsPointerOverUi`.

### Required tests

- Focus search field and type F/E/brackets → no tool mutation.
- Ctrl+Z in input edits text rather than room history.
- Unfocus field → tool shortcuts work normally.

---

## RIMA-009 — Rapid Build Mode re-entry can capture an intermediate camera size

**Severity:** COSMETIC  
**Confidence:** CONFIRMED by coroutine transition logic  
**Primary source:** `Assets/Scripts/UI/BuildModeController.cs`  
**Relevant methods:** `ExitBuildMode`, `StartZoom`, `ZoomRoutine`, `EnterBuildMode`

### Evidence

Exiting starts a zoom coroutine that restores the rig only after interpolation completes.

A rapid re-entry calls `StartZoom`, stops the existing coroutine, and may prevent `RestoreCameraRig`. `EnterBuildMode` then captures the current intermediate orthographic size as the new baseline.

### Failure scenario

Repeated rapid F2 toggles cause camera size to drift or camera components to remain disabled longer than intended.

### Recommended fix

Use an explicit transition state:

```csharp
enum BuildModeTransition
{
    Inactive,
    Entering,
    Active,
    Exiting
}
```

Simpler demo-safe option:

- ignore toggle while entering/exiting, or
- on re-entry during exit, finish restore synchronously before capturing a new baseline.

Never cancel a restoration coroutine without restoring captured camera components.

### Required tests

- Spam F2 during exit → final camera size equals original.
- CameraZoom, PixelPerfectCamera, and CameraFollow are enabled after final exit.
- No cumulative orthographic drift after ten toggle cycles.

---

## RIMA-010 — Positive base damage is always clamped to at least one

**Severity:** COSMETIC / CONTRACT-DEPENDENT  
**Confidence:** SUSPECTED  
**Primary source:** `Assets/Scripts/Balance/DamageCalculator.cs`  
**Relevant method:** `Calculate`

### Evidence

For positive `baseDamage`, final output is:

```csharp
Mathf.Max(1, Mathf.RoundToInt(rawDamage))
```

Therefore zero multipliers still result in one damage.

### Why this may be intentional

Many action games enforce chip damage for a successful hit. The current code may deliberately avoid zero-damage attacks.

### Why this may be wrong

A debug multiplier of zero or a deliberately disabled attack cannot produce zero damage.

### Recommended action

Do not change this without confirming the combat contract.

Add tests documenting the intended answer:

- Is positive base damage with `debugGlobalDamageMult = 0` expected to be `0` or `1`?
- Should immunity be represented by damage type/defense or by a bypass flag?
- Is a zero situational multiplier valid?

### Required tests

Only after the design decision is recorded.

---

## RIMA-011 — Power scaling by `/ 100f` is correct

**Severity:** NON-ISSUE-OR-FUTURE  
**Confidence:** CONFIRMED  
**Source:** `Assets/Scripts/Balance/DamageCalculator.cs`, `ClassStatRuntime.cs`, `ClassStatProfile.cs`

`physPower` and `abilityPower` default to `100`. Dividing by `100f` correctly maps the neutral value to a `1.0` multiplier.

Do not change this to `1 + power/100` unless the data contract changes from “100 equals baseline total power” to “power is bonus percentage.”

---

## RIMA-012 — Posture overflow is explicitly future work

**Severity:** NON-ISSUE-OR-FUTURE  
**Confidence:** CONFIRMED  
**Source:** `Assets/Scripts/Balance/DamageCalculator.cs`

The calculator reports posture overflow separately and explicitly contains a TODO for a future consumer. It does not accidentally add overflow into `finalDamage`.

Do not route this into health damage merely to eliminate the TODO.

---

# Cross-cutting observation

The code frequently uses self-bootstrapped `DontDestroyOnLoad` singletons and realtime coroutines. That combination is useful for a demo but dangerous when ownership is implicit.

Every persistent manager should answer four questions:

1. Who creates it?
2. Who destroys or resets it?
3. Which scene transitions preserve it?
4. Which pending coroutines/events are cancelled on reset?

The fixes above should add those guarantees only where needed for the demo, not trigger a repository-wide singleton crusade three minutes before presentation.
