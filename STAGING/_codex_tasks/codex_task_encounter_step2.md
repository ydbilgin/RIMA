# Codex Task — SubRoomSequenceController + Triggers + Reward Fork + Camera Bounds (Step 2 of 7)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

---

## Mission

Implement the runtime side of the Sub-Room Encounter system (Karar #149 LIVE). Step 1 (`EncounterTemplateSO` + Validator + Editor) is already LIVE. This task implements Steps 2-5 of 7 from `STAGING/SUBROOM_ENCOUNTER_TECH_SPEC_OPUS.md`:

- Step 2: `SubRoomSequenceController` runtime state machine + `EncounterBankStub`
- Step 3: `IntraEncounterDoorTrigger`
- Step 4: `LegacyRuntimeRoomManager` reward gating fork
- Step 5: `CameraFollow.SetBounds(...)` + `EnemySpawnSocket.avoidRadius` field

Stop after Step 5. Steps 6-7 (vertical-slice authoring + playtest tuning) are user/design work, not Codex.

## Source spec — REQUIRED READ

`STAGING/SUBROOM_ENCOUNTER_TECH_SPEC_OPUS.md` — sections 2, 3, 4, 5, 7, 9. Read before coding.

Step 1 reference (already implemented, do not modify):
- `Assets/Scripts/MapDesigner/Encounter/Data/EncounterTemplateSO.cs`
- `Assets/Scripts/MapDesigner/Encounter/Data/SubRoomEntry.cs`
- `Assets/Scripts/MapDesigner/Encounter/Data/SubRoomLink.cs`
- `Assets/Scripts/MapDesigner/Encounter/EncounterTemplateValidator.cs`

## Files to create

- `Assets/Scripts/Runtime/Encounter/SubRoomSequenceController.cs`
- `Assets/Scripts/Runtime/Encounter/IntraEncounterDoorTrigger.cs`
- `Assets/Scripts/Runtime/Encounter/IEncounterBank.cs`
- `Assets/Scripts/Runtime/Encounter/SubRoomEnemyPlan.cs`
- `Assets/Scripts/Runtime/Encounter/EncounterAssignment.cs` (or merge into SubRoomEnemyPlan.cs)
- `Assets/Scripts/Runtime/Encounter/EncounterBankStub.cs`
- Assembly definition: `Assets/Scripts/Runtime/Encounter/RIMA.Runtime.Encounter.asmdef` (if existing runtime asmdef doesn't cover)

## Files to modify (surgical edits)

- `Assets/Scripts/Core/LegacyRuntimeRoomManager.cs` — add `isEncounterContext`, `isFinalSubRoom`, `OnEncounterFinalCleared()`, gate `RoomClearedSequence` reward block, controller bootstrap in `StartRoom`
- `Assets/Scripts/Player/CameraFollow.cs` — add `SetBounds(Vector2 min, Vector2 max)` + `SetBounds(Bounds worldBounds)` overload
- `Assets/Scripts/MapDesigner/Room/Data/EnemySpawnSocket.cs` — add `public float avoidRadius = 1.5f;` field

## Files to read (no modifications)

- `STAGING/SUBROOM_ENCOUNTER_TECH_SPEC_OPUS.md` (sections 2-5, 7, 9)
- `Assets/Scripts/Core/LegacyRuntimeRoomManager.cs` (full)
- `Assets/Scripts/Core/DoorTrigger.cs`
- `Assets/Scripts/Core/RoomTransitionFX.cs`
- `Assets/Scripts/Player/CameraFollow.cs`
- `Assets/Scripts/Core/DungeonGraph.cs`
- `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs`
- `Assets/Scripts/MapDesigner/Room/Data/DoorSocket.cs`
- `Assets/Scripts/MapDesigner/Room/Data/EnemySpawnSocket.cs`
- `Assets/Scripts/MapDesigner/MapLayerOrchestrator.cs` (only the per-prop placement loop, for the avoid-mask integration — see below)

## Namespace

All new classes use namespace `RIMA.Runtime.Encounter`.

---

## Implementation requirements

### 1. SubRoomSequenceController (Step 2)

Spec: §2 of `SUBROOM_ENCOUNTER_TECH_SPEC_OPUS.md`.

State enum, lifecycle, public API per spec — copy verbatim:

```csharp
public enum SubRoomState { Idle, Loading, Active, Cleared, Transitioning, EncounterComplete }

public class SubRoomSequenceController : MonoBehaviour
{
    public static SubRoomSequenceController Active { get; private set; }
    public SubRoomState State { get; private set; }
    public EncounterTemplateSO Template { get; private set; }
    public int CurrentSubRoomIndex { get; private set; }
    public string CurrentSubRoomKey => Template.subRooms[CurrentSubRoomIndex].subRoomKey;
    public bool IsFinalSubRoom => Template.subRooms[CurrentSubRoomIndex].isFinal;

    public void StartEncounter(EncounterTemplateSO template);
    public RoomTemplateSO GetCurrentSubRoom();
    public void AdvanceSubRoom(string viaFromDoorSocketId);
    public void OnEnemyKilled(GameObject enemy);
    public void TerminateEncounter();

    public event Action<int> OnSubRoomEntered;
    public event Action<int> OnSubRoomCleared;
    public event Action OnEncounterComplete;
}
```

Implement the state transition table verbatim (spec §2 table). Notes:
- Singleton-light: `Active` set in `Awake` only when an encounter is loaded; cleared on `TerminateEncounter` / `OnEncounterComplete`.
- Sub-room load coroutine (`LoadSubRoomCoroutine`): tears down old enemies/props/triggers under the current sub-room root, paints new sub-room via `MapLayerOrchestrator.Paint(roomTemplate)`, resolves enemies via `IEncounterBank.GetSubRoomEnemies(...)`, instantiates them at `EnemySpawnSocket` positions, instantiates `IntraEncounterDoorTrigger` per outgoing `SubRoomLink`, calls `CameraFollow.SetBounds(floorRenderer.bounds)`.
- Sub-room transitions: `RoomTransitionFX.Instance.DoTransition(SwapSubRoomWhileBlack)` — the `SwapSubRoomWhileBlack` body runs while screen is black: teardown old, paint new, reset player to `toEntryDoorSocketId` world position, enable input on fade out.
- `OnEnemyKilled`: increment local kill counter; when counter ≥ `IEncounterBank.GetSubRoomKillQuota(...)`, transition Active → Cleared, fire `OnSubRoomCleared`, enable `IntraEncounterDoorTrigger.SetActive(true)` on every outgoing trigger.
- Zero-enemy connector sub-rooms: Cleared fires immediately at Loading → Active boundary (kill quota 0).
- `TerminateEncounter`: hard reset on player death; destroy controller, `Active = null`. Standard death flow resumes (existing `DeathScreenManager`).
- Final sub-room clear → call `LegacyRuntimeRoomManager.Instance.OnEncounterFinalCleared()`, fire `OnEncounterComplete`, transition Cleared → EncounterComplete.

Ownership / bootstrap: spec §2 — `LegacyRuntimeRoomManager.StartRoom()` is the single creator. New code path:

```
StartRoom():
  if (DungeonGraph.CurrentNode.roomType is Combat or Elite)
      AND EncounterBank.TryResolve(node, biome) returns EncounterTemplateSO et:
      InstantiateOrReuseController().StartEncounter(et)
      return
  else:
      // existing single-room flow (unchanged)
      ...
```

Use `FindObjectOfType<IEncounterBank>()` or a `[SerializeField]` reference — DO NOT use a global singleton resolver; keep dependency explicit. For the stub, attach `EncounterBankStub` (MonoBehaviour implementing `IEncounterBank`) to the same scene root as `LegacyRuntimeRoomManager`.

### 2. IEncounterBank + SubRoomEnemyPlan + EncounterBankStub (Step 2)

Spec: §7.

```csharp
public interface IEncounterBank
{
    bool TryResolve(RoomNode macroNode, string biomeId, out EncounterTemplateSO template);
    SubRoomEnemyPlan GetSubRoomEnemies(string encounterId, int subRoomIndex);
    int GetSubRoomKillQuota(string encounterId, int subRoomIndex);
}

public class SubRoomEnemyPlan
{
    public List<EnemyAssignment> Enemies;
}

public class EnemyAssignment
{
    public string socketId;
    public GameObject enemyPrefab;
    public bool isElite;
}
```

`EncounterBankStub : MonoBehaviour, IEncounterBank` — minimal implementation for vertical slice:
- `[SerializeField] EncounterTemplateSO defaultEncounter` — used for any Combat/Elite macro node when set; otherwise `TryResolve` returns `false` (controller skipped → legacy flow runs).
- `[SerializeField] GameObject defaultEnemyPrefab` — used for every assigned socket.
- `GetSubRoomEnemies` returns one `EnemyAssignment` per `EnemySpawnSocket` in the sub-room (`SubRoomEntry.room.enemySpawnSockets`).
- `GetSubRoomKillQuota` returns the count of `EnemyAssignment` returned by `GetSubRoomEnemies` (i.e. "kill all spawned").

Public API only. Full threat-budget + wave logic is a SEPARATE next-sprint spec — DO NOT implement.

### 3. IntraEncounterDoorTrigger (Step 3)

Spec: §3.

```csharp
public class IntraEncounterDoorTrigger : MonoBehaviour
{
    [SerializeField] private string fromDoorSocketId;
    private BoxCollider2D col;
    private bool isActive;
    private bool playerInRange;
    private const Key InteractKey = Key.G;

    public void Configure(string socketId);
    public void SetActive(bool active);
    private void Update();                  // G-key check
    private void OnTriggerEnter2D(Collider2D);
    private void OnTriggerExit2D(Collider2D);
}
```

Behavior pattern: COPY `DoorTrigger.Update` G-key interaction + `OnTriggerEnter2D` / `OnTriggerExit2D` player-detection — do not subclass `DoorTrigger`. On G-key press while `isActive && playerInRange && !RoomTransitionFX.IsFading`:

```csharp
SubRoomSequenceController.Active.AdvanceSubRoom(fromDoorSocketId);
```

DO NOT call `LegacyRuntimeRoomManager.OnPlayerEnteredDoor(...)`, DO NOT call `DungeonGraph.Navigate(...)`. This trigger is for INTRA-encounter only. Spec §3 table.

Lock state: `SetActive(false)` on Loading; controller calls `SetActive(true)` on Cleared.

### 4. LegacyRuntimeRoomManager reward fork (Step 4)

Spec: §4. Fork point: `RoomClearedSequence` coroutine (currently lines ~575-621 per spec — verify in current file before edit).

Add fields:
```csharp
private bool isEncounterContext;
private bool isFinalSubRoom;
```

Add public method:
```csharp
public void OnEncounterFinalCleared()
{
    isEncounterContext = true;
    isFinalSubRoom = true;
    roomCleared = true;
    StartCoroutine(RoomClearedSequence());
}
```

Gate the reward + map fragment + draft + chest block in `RoomClearedSequence`:

```csharp
bool spawnReward = (roomType == RoomType.Combat || roomType == RoomType.Elite)
                   && (!isEncounterContext || isFinalSubRoom);
// gate: rewardPickup spawn, TrySpawnMapFragment(), TrySpawnChest(), draft trigger
```

Reset both flags after sequence completes.

Add controller bootstrap in `StartRoom()`:
- If `DungeonGraph.CurrentNode.roomType` is Combat or Elite AND `_encounterBank.TryResolve(node, biome, out var et)` succeeds → `EnsureControllerInstance().StartEncounter(et)` → `return` (skip existing single-room flow).
- Else: existing flow unchanged.

Inject `IEncounterBank _encounterBank` via `[SerializeField]` OR `FindObjectOfType<EncounterBankStub>()` lazy lookup in `Awake`. Be conservative — do not refactor existing instance lookup patterns.

Sub-room transitions never call `RoomClearedSequence` directly. Only `OnEncounterFinalCleared` does. Non-final sub-room clears do NOT trigger `Time.timeScale` slowdown (the fade is enough).

### 5. CameraFollow.SetBounds (Step 5)

Spec: §5.

Add to `CameraFollow`:

```csharp
public void SetBounds(Vector2 min, Vector2 max)
{
    roomMin = min;
    roomMax = max;
    useBounds = true;
}

public void SetBounds(Bounds worldBounds)
{
    roomMin = new Vector2(worldBounds.min.x + boundsPadding.x, worldBounds.min.y + boundsPadding.y);
    roomMax = new Vector2(worldBounds.max.x - boundsPadding.x, worldBounds.max.y - boundsPadding.y);
    useBounds = true;
}
```

Verify `roomMin`, `roomMax`, `useBounds`, `boundsPadding` field names match the actual `CameraFollow.cs` — adjust if different but match semantics. Pixel Perfect Camera ortho size unchanged. Branch E 6° tilt compat: bounds are world XY, tilt is camera transform — no conflict.

### 6. EnemySpawnSocket.avoidRadius (Step 5)

Spec: §9.

Add field:
```csharp
public float avoidRadius = 1.5f;
```

DO NOT touch `MapLayerOrchestrator.Paint(...)` in this dispatch. The avoid-mask consumer logic is out of scope for this step — flagged for the next encounter-painter dispatch. The field itself is needed so authoring assets stop breaking when the field is referenced. Leave a `// TODO: MapLayerOrchestrator avoid-radius consumer — separate spec` comment in `EnemySpawnSocket.cs`.

---

## Acceptance criteria

1. All new files compile under URP 2D + the existing assembly graph. Run `dotnet build` and check `read_console` via UnityMCP for ZERO compile errors (Unity must be open).
2. Domain reload succeeds (no `isCompiling` hang). Check `editor_state` resource.
3. `SubRoomSequenceController.Active` is `null` until `StartEncounter` runs; non-null while encounter is loaded; `null` again after `OnEncounterComplete` / `TerminateEncounter`.
4. `IntraEncounterDoorTrigger` does NOT call `DungeonGraph.Navigate` under ANY code path (grep verify).
5. `LegacyRuntimeRoomManager.RoomClearedSequence` reward block is gated by `(!isEncounterContext || isFinalSubRoom)`. Non-final sub-room clears do not spawn reward.
6. 2 EditMode tests added to `RoomFlowTests.cs`:
   - `SubRoomSequenceController_FinalClearTriggersReward` — fake `IEncounterBank` returning 1-sub-room (entry == final) encounter; verify `LegacyRuntimeRoomManager.OnEncounterFinalCleared` is invoked and `RoomClearedSequence` runs reward gated true.
   - `SubRoomSequenceController_NonFinalClearSkipsReward` — fake `IEncounterBank` returning 2-sub-room encounter; advance from sub-room 0 → 1; verify `RoomClearedSequence` did NOT spawn reward when sub-room 0 cleared.
7. `read_console` shows zero errors after compile + test run.

## Out of scope

- Step 6: vertical-slice authoring (3 `RoomTemplateSO` assets + 1 `EncounterTemplateSO` wired into `EncounterBankStub`) — user/design work
- Step 7: playtest tuning
- `EncounterBank` full implementation (threat budget, wave triggers, 40/50% wave math)
- `MapLayerOrchestrator` `EnemyAvoidMask` consumer logic
- Mid-encounter save (Phase 2 backlog)
- Encounter graph editor visualization

## Hard rules

- DO read `STAGING/SUBROOM_ENCOUNTER_TECH_SPEC_OPUS.md` sections 2-5 + 7 + 9 before coding
- DO use namespace `RIMA.Runtime.Encounter` for all new classes
- DO verify field names in `CameraFollow.cs` before adding `SetBounds` (don't invent `roomMin`/`roomMax` if real names differ)
- DO verify line range of `RoomClearedSequence` in current `LegacyRuntimeRoomManager.cs` before patching (spec says ~575-621, may have drifted)
- DO compile-check + run EditMode tests after each file batch — `read_console` between batches
- DO use Input System `Keyboard.current[Key.G].wasPressedThisFrame` pattern (match existing `DoorTrigger` Input usage)
- DO NOT modify `EncounterTemplateSO.cs` / `SubRoomEntry.cs` / `SubRoomLink.cs` / `EncounterTemplateValidator.cs` (Step 1, LIVE)
- DO NOT call `EditorUtility.DisplayDialog` anywhere (BANNED — Debug.Log only)
- DO NOT touch `MapLayerOrchestrator.Paint(...)` — avoid-radius consumer is a separate dispatch
- DO NOT add a singleton resolver / service locator — explicit `[SerializeField]` or `FindObjectOfType` only
- DO NOT add UnityEditor `using` directives in runtime files (Encounter runtime asmdef must NOT depend on Editor)

## Effort

~15-25 hours per Opus estimate. Mostly state machine + spec-following + 2 EditMode tests. Hardest part: integrating the reward gate cleanly into existing `RoomClearedSequence` without breaking single-room flow.

## Done report

Write `STAGING/CODEX_DONE_encounter_step2.md` with:
- Files created (path list)
- Files modified (path list + line diff summary)
- `dotnet build` result (PASS / FAIL + error count)
- `read_console` summary (errors / warnings)
- EditMode test result (`SubRoomSequenceController_*` tests PASS / FAIL)
- Open issues / deviations from spec (if any)
- Estimated total LOC added/modified
