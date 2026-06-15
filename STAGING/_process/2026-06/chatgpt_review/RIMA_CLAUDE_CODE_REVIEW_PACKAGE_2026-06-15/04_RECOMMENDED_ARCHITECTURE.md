# Recommended Architecture

## 1. Time-scale ownership

### Problem

`UIManager`, `DirectorMode`, death flow, and menu flow can all write `Time.timeScale`.

The system currently behaves like several people grabbing one steering wheel and confidently turning in different directions.

### Preferred design: reason-based arbiter

Create one component, possibly `GameTimeCoordinator`, with a set of active reasons.

Example model:

```csharp
public enum GameTimeReason
{
    MainMenu,
    PauseMenu,
    Settings,
    SkillCodex,
    SkillOffer,
    DirectorMode,
    BuildMode,
    DeathScreen,
    CharacterSheetSlowMotion
}
```

Rules:

- Any hard-pause reason → scale `0`.
- Else CharacterSheet slow-motion reason → scale `0.1`.
- Else scale `1`.
- Systems request/release reasons; they never directly restore to `1`.
- Scene load clears transient reasons and retains only intentional persistent menu state.
- Death remains authoritative until death flow releases it.
- F12 panic clears recoverable reasons but must not violate a scene transition.

Suggested API:

```csharp
public void Acquire(GameTimeReason reason);
public void Release(GameTimeReason reason);
public bool Has(GameTimeReason reason);
public float EffectiveScale { get; }
public void ClearTransientReasons();
```

### Minimum-risk demo patch

If a new coordinator is too invasive:

1. Keep `UIManager.ApplyTimeScale` as central recomputation.
2. Expose `RefreshTimeScale()`.
3. Add Director state to the calculation or let Director call a shared resolver.
4. Remove “resume by assignment” from Director/Build exit.
5. Death screen remains checked before resuming.
6. Add regression tests for every overlay combination.

## 2. Draft request serialization

Drafts have multiple sources:

- opening kit,
- room clear,
- secondary class unlock,
- reward,
- portal/gate.

They should all go through one request function.

Suggested internal model:

```csharp
private sealed class DraftRequest
{
    public DraftRequestSource source;
    public float delay;
    public SkillData forcedSkill;
    public int room;
}
```

Minimum required behavior:

- one active draft,
- at most one pending request,
- immediate requests can supersede/cancel delayed room-clear requests,
- duplicate same-source requests collapse,
- pending state is visible to room flow,
- all pending work is cancelled on disable/run reset,
- timeout resolution is centralized,
- UI callback cannot be silently replaced.

Suggested API shape:

```csharp
public bool TryQueueDraft(DraftRequest request);
public bool TryOpenDraftNow(DraftRequest request);
public void CancelPendingDraft(DraftCancelReason reason);
public bool ForceResolveCurrentDraft(DraftTimeoutPolicy policy);
```

## 3. Persistent-manager lifecycle

For `DontDestroyOnLoad` objects:

- static instance must tolerate Disable Domain Reload,
- scene load must reset scene-specific state,
- subscriptions must be symmetric,
- coroutine handles must be retained,
- run generation/version should invalidate stale work,
- no anonymous persistent event handlers.

## 4. Build Mode state machine

Current boolean state is insufficient once entry/exit animations exist.

Recommended states:

```csharp
Inactive
Entering
Active
Exiting
```

Rules:

- modal UI blocks transition from `Inactive` to `Entering`,
- input is accepted only in `Active`,
- rapid toggle either queues one reversal or is ignored,
- camera restore runs exactly once,
- hidden canvases are restored exactly once,
- working template exists only in `Entering/Active`,
- tools are disabled before destruction of the working template.

## 5. Error recovery

F12 panic is valuable for a live demo. Keep it, but ensure it interacts with the same central state APIs rather than manually mutating flags and time scale.

Panic should:

- close/resolve active modal UI,
- cancel pending draft requests,
- exit Build Mode safely,
- restore camera rig,
- clear recoverable pause reasons,
- re-enable player,
- open exits only after modal cleanup,
- log a single structured recovery summary.
