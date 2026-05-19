# SUBROOM_ENCOUNTER_TECH_SPEC_OPUS.md

Owner: rima-opus | Karar #149 LIVE | Codex hand-off ready | 2026-05-19

Scope: `EncounterTemplateSO` data, `SubRoomSequenceController` runtime, `IntraEncounterDoorTrigger`, reward gating fork in `LegacyRuntimeRoomManager`, `CameraFollow.SetBounds`, encounter validator, Karar #143 spawn-pocket conflict mitigation. EncounterBank (enemy identity + threat budget) is a separate spec — only the consumer interface is defined here.

Reference files (read by Opus 2026-05-19):
- `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs`
- `Assets/Scripts/MapDesigner/Room/Data/DoorSocket.cs`
- `Assets/Scripts/MapDesigner/Room/Data/EnemySpawnSocket.cs`
- `Assets/Scripts/Runtime/Rooms/GateSocket.cs`
- `Assets/Scripts/Core/DoorTrigger.cs`
- `Assets/Scripts/Core/LegacyRuntimeRoomManager.cs`
- `Assets/Scripts/Core/RoomTransitionFX.cs`
- `Assets/Scripts/Player/CameraFollow.cs`
- `Assets/Scripts/Core/DungeonGraph.cs`
- `Assets/Scripts/MapDesigner/MapLayerOrchestrator.cs`

---

## 1. EncounterTemplateSO — data model

Namespace `RIMA.MapDesigner.Encounter`. Path `Assets/Scripts/MapDesigner/Encounter/Data/EncounterTemplateSO.cs`. `CreateAssetMenu` path `RIMA/Encounter/EncounterTemplate`.

```csharp
[Serializable]
public class SubRoomEntry
{
    public string subRoomKey;                 // unique within encounter ("entry","pocket_w","final")
    public RoomTemplateSO room;               // existing SO, sockets/tags/visual only
    public List<SubRoomLink> links;           // outgoing internal links to other subRoomKeys
    public bool isEntry;                      // exactly 1 per encounter
    public bool isFinal;                      // exactly 1 per encounter (carries reward)
    public string subRoomTag;                 // "warmup" | "flank_pocket" | "elite_pocket" | "final_mix" | "connector"
}

[Serializable]
public class SubRoomLink
{
    public string fromDoorSocketId;           // matches RoomTemplateSO.doorSockets[].socketId on source
    public string toSubRoomKey;               // destination SubRoomEntry.subRoomKey
    public string toEntryDoorSocketId;        // matches RoomTemplateSO.doorSockets[].socketId on destination (player spawn-side)
    public bool requiresClear = true;         // gate locked until source sub-room kill-quota met
}

[CreateAssetMenu(menuName="RIMA/Encounter/EncounterTemplate", fileName="Encounter_New", order=210)]
public class EncounterTemplateSO : ScriptableObject
{
    public string schemaVersion = "1.0";
    public string encounterId;                // stable id, used by Death Imprint + EncounterBank
    public RIMA.RoomType macroRoomType;       // Combat | Elite (only valid values)
    public string biomeId;
    public int encounterSeed;                 // root seed; sub-room seeds derived
    public List<SubRoomEntry> subRooms = new(); // 3..5
    public List<string> encounterTags = new();// "two_pocket","elite_pinch","fork_branch"
    public string encounterBankKey;           // EncounterBank lookup; threat budget owner
}
```

Validation rules (enforced by `EncounterTemplateValidator.Validate`, return list of error strings):
1. `subRooms.Count` in `[3,5]`. Outside range = error.
2. Exactly 1 `isEntry == true`, exactly 1 `isFinal == true`. Not the same entry.
3. Every `SubRoomLink.toSubRoomKey` resolves to an existing `SubRoomEntry.subRoomKey`.
4. Every `fromDoorSocketId` / `toEntryDoorSocketId` resolves to a `DoorSocket.socketId` on the referenced room.
5. Graph reachability: BFS from entry must visit every sub-room including final. No orphans, no unreachable final.
6. Per sub-room: every `EnemySpawnSocket.position` lies in `bounds`, is `IsWalkable(...)`, and is not occluded by a prop (`PropPlacementData` AABB intersection check). Otherwise error: "spawn socket S blocked by prop P".
7. `macroRoomType` must be `Combat` or `Elite`. Other types reject (no Chest/Merchant/Event/Forge as encounters).
8. `encounterId` non-empty, unique among loaded `EncounterTemplateSO` assets in project.

Inspector editor — minimum sufficient:
- Default Unity inspector + `[ReorderableList]` (via simple `CustomEditor` calling `EditorGUILayout.PropertyField` on `subRooms`). No custom graph drawer.
- Add a button "Validate" running `EncounterTemplateValidator` and printing results to Console + a HelpBox.
- Defer node-graph visual to Phase 2. Reorder + drag-and-drop is enough for 3-5 nodes.

---

## 2. SubRoomSequenceController — runtime

Namespace `RIMA.Runtime.Encounter`. Path `Assets/Scripts/Runtime/Encounter/SubRoomSequenceController.cs`. MonoBehaviour, instantiated under the same root as `LegacyRuntimeRoomManager` (sibling). Singleton-light: `public static SubRoomSequenceController Active { get; private set; }` set in `Awake` only when an encounter is loaded.

State enum:
```csharp
public enum SubRoomState { Idle, Loading, Active, Cleared, Transitioning, EncounterComplete }
```

State transitions (text diagram, who fires what):

| From | To | Fired by | Action |
|---|---|---|---|
| Idle | Loading | `LegacyRuntimeRoomManager.StartRoom()` when node is Combat/Elite and the resolved `EncounterTemplateSO != null` | calls `StartEncounter(template)`; loads entry sub-room template; paints background, registers triggers, spawns enemies via EncounterBank |
| Loading | Active | end of `LoadSubRoomCoroutine` | enables player input, enemy AI, starts kill tracker |
| Active | Cleared | `OnEnemyKilled` when sub-room kill quota satisfied (kills >= required, supplied by EncounterBank for that sub-room) | unlocks outgoing `IntraEncounterDoorTrigger`s; HUD prompt "kapı açıldı" |
| Cleared | Transitioning | `IntraEncounterDoorTrigger.OnPlayerInteract` (G key) | `RoomTransitionFX.DoTransition(SwapSubRoomWhileBlack)` |
| Transitioning | Loading | `RoomTransitionFX` callback while black (`SwapSubRoomWhileBlack` body) | tears down old sub-room (clear enemies, props, decals, gates); paints new sub-room; resets player position to destination `toEntryDoorSocketId`; calls `CameraFollow.SetBounds(newBounds)` |
| Active OR Cleared | (no change) | macro Pause / Quit | save policy = no mid-encounter save (see §6). On quit, encounter state discarded; on reload, encounter restarts at entry. |
| Cleared | EncounterComplete | Cleared on the `isFinal == true` sub-room | yields to `LegacyRuntimeRoomManager.OnEncounterFinalCleared()` which runs `RoomClearedSequence` with `isFinalSubRoom = true` |
| EncounterComplete | Idle | `LegacyRuntimeRoomManager` after reward collected → `OpenDoorsAfterReward()` → player walks macro door → `DungeonGraph.Navigate()` | controller sets `Active = null`, destroys self or pools |

Edge cases (Codex must implement):
- **Transition mid-combat:** not possible. `IntraEncounterDoorTrigger` is disabled while `state == Active`. Only enabled on `Cleared`.
- **Cleared before all enemies dead:** not allowed. Kill quota = `EncounterBank.GetSubRoomKillQuota(encounterId, subRoomIndex)`; default = "all spawned enemies dead". Quota can be < total only if EncounterBank explicitly returns a smaller number (e.g. "kill 3 of 5 to proceed" — out of MVP scope, but API allows it).
- **Player dies mid-encounter:** death flow unchanged (DeathScreenManager etc.). On respawn (run restart), controller is destroyed; macro flow resumes.
- **Transition during fade input lock:** `RoomTransitionFX.IsFading == true` blocks any new transition; `IntraEncounterDoorTrigger.Update` checks this.
- **Sub-room with zero enemies (connector):** Cleared fires immediately on Loading→Active boundary (treat as "0 kills required, 0 spawned").

Public API:
```csharp
public class SubRoomSequenceController : MonoBehaviour
{
    public static SubRoomSequenceController Active { get; private set; }
    public SubRoomState State { get; private set; }
    public EncounterTemplateSO Template { get; private set; }
    public int CurrentSubRoomIndex { get; private set; }   // index into Template.subRooms
    public string CurrentSubRoomKey => Template.subRooms[CurrentSubRoomIndex].subRoomKey;
    public bool IsFinalSubRoom => Template.subRooms[CurrentSubRoomIndex].isFinal;

    public void StartEncounter(EncounterTemplateSO template);
    public RoomTemplateSO GetCurrentSubRoom();
    public void AdvanceSubRoom(string viaFromDoorSocketId); // called by IntraEncounterDoorTrigger
    public void OnEnemyKilled(GameObject enemy);            // wired in spawn step
    public void TerminateEncounter();                       // hard reset (e.g. player death)

    public event Action<int> OnSubRoomEntered;              // payload = index
    public event Action<int> OnSubRoomCleared;              // payload = index
    public event Action OnEncounterComplete;                // fires once on final clear
}
```

Ownership: `LegacyRuntimeRoomManager.StartRoom()` is the single creator. New code path:

```
StartRoom():
  if (DungeonGraph.CurrentNode.roomType is Combat or Elite)
      AND EncounterBank.TryResolve(node, biome) returns EncounterTemplateSO et:
      InstantiateOrReuseController().StartEncounter(et)
      // controller drives sub-room load, NOT LegacyRuntimeRoomManager
      return
  else:
      // existing single-room flow (Chest, Merchant, Event, Forge, Boss, or fallback combat)
      ...existing code...
```

`LegacyRuntimeRoomManager` retains: macro tilemap reset between encounters, macro reward sequence, macro door open + `DungeonGraph.Navigate`. It surrenders: enemy spawning, kill tracking, door triggers during an active encounter.

---

## 3. IntraEncounterDoorTrigger — semantics

Namespace `RIMA.Runtime.Encounter`. Path `Assets/Scripts/Runtime/Encounter/IntraEncounterDoorTrigger.cs`. New class — does **not** subclass `DoorTrigger`. Same `BoxCollider2D + isTrigger` + G-key interaction pattern, copied behavior only.

Differences vs `DoorTrigger`:

| Aspect | `DoorTrigger` (macro) | `IntraEncounterDoorTrigger` (intra) |
|---|---|---|
| Activated when | macro room cleared AND macro doors open | sub-room cleared AND `state == Cleared` |
| On interact, calls | `LegacyRuntimeRoomManager.Instance.OnPlayerEnteredDoor(direction)` | `SubRoomSequenceController.Active.AdvanceSubRoom(this.fromDoorSocketId)` |
| Calls `DungeonGraph.Navigate`? | YES | **NO — must not, ever** |
| Lock condition | room cleared | source sub-room kill quota met (controller polls `state == Cleared`) |
| Identity | `DoorDirection` enum | `string fromDoorSocketId` (matches `SubRoomLink.fromDoorSocketId`) |
| Visual | gate sprite via `GateBehavior` | reuse `GateSocket` visual under sub-room prefab (sub-room template carries the gate) |

```csharp
public class IntraEncounterDoorTrigger : MonoBehaviour
{
    [SerializeField] private string fromDoorSocketId;   // wired from SubRoomLink.fromDoorSocketId at sub-room build time
    private BoxCollider2D col;
    private bool isActive;
    private bool playerInRange;
    private const Key InteractKey = Key.G;

    public void Configure(string socketId) { fromDoorSocketId = socketId; }
    public void SetActive(bool active);     // controller calls this on Cleared / Loading transitions

    private void Update();                  // identical G-key check pattern; on press → controller.AdvanceSubRoom(fromDoorSocketId)
    private void OnTriggerEnter2D(Collider2D); // identical to DoorTrigger
}
```

Spawn timing of triggers: when controller loads sub-room N, it instantiates one `IntraEncounterDoorTrigger` per outgoing `SubRoomLink` in `Template.subRooms[N].links`. The trigger is parented under the loaded room prefab so it gets torn down on sub-room swap. World position = `RoomTemplateSO.doorSockets` entry whose `socketId == fromDoorSocketId`.

Lock policy: triggers start `SetActive(false)` on Loading. Controller calls `SetActive(true)` on Cleared. Source of truth for "is cleared" = `EncounterBank.GetSubRoomKillQuota(encounterId, currentSubRoomIndex)` matched against the controller's local kill counter. **Kill counter, not socket count** — socket count includes pre-authored positions, actual spawn count may be lower based on threat budget.

---

## 4. Reward gating fork in LegacyRuntimeRoomManager

Exact fork point: `RoomClearedSequence` coroutine (currently lines ~575-621). Wrap the reward + map fragment + draft + chest spawn block in an `isFinalSubRoom` gate.

Refactor:

```csharp
// New field
private bool isEncounterContext;            // true while SubRoomSequenceController is active
private bool isFinalSubRoom;                // true only on the final sub-room of an encounter

// New public method, called by controller when final sub-room clears
public void OnEncounterFinalCleared()
{
    isEncounterContext = true;
    isFinalSubRoom = true;
    roomCleared = true;
    StartCoroutine(RoomClearedSequence());
}

// In RoomClearedSequence, gate reward block:
private IEnumerator RoomClearedSequence()
{
    // ... slowdown unchanged ...

    bool spawnReward = (roomType == RoomType.Combat || roomType == RoomType.Elite)
                       && (!isEncounterContext || isFinalSubRoom);

    // rewardPickup + draft block — unchanged but gated on spawnReward (already exists, just add encounter check)
    // TrySpawnMapFragment() — gate same way
    // TrySpawnChest() — gate same way

    // Reset encounter flags AFTER sequence
    isEncounterContext = false;
    isFinalSubRoom = false;
}
```

Sub-room clears (non-final) do **not** call `RoomClearedSequence`. They call only the controller's internal handler which:
- skips reward / fragment / draft / chest entirely
- enables `IntraEncounterDoorTrigger`s
- HUD: "kapı açıldı — devam et"

Macro reward fires once: on final sub-room clear → reward orb + map fragment + skill draft (via existing `RewardPickup` flow) + macro chest spawn + macro doors open (calls `OpenDoorsAfterReward()` → `OpenDoorsNow()` → `DungeonGraph.Navigate` on macro door interact, unchanged).

`Time.timeScale` slowdown still fires on final clear (good — preserves Hades clear-feel). Sub-room transitions do **not** trigger slowdown (the fade already provides pacing).

`RoomTransitionFX.DoTransition` reuse: on macro `StartRoom` entry, only fired by `OnPlayerEnteredDoor` macro flow (existing line ~903). On sub-room transitions, controller calls `RoomTransitionFX.Instance.DoTransition(SwapSubRoomWhileBlack)` directly — same singleton, no new fade infra.

---

## 5. CameraFollow bounds refresh

Add public method to `CameraFollow`:

```csharp
public void SetBounds(Vector2 min, Vector2 max)
{
    roomMin = min;
    roomMax = max;
    useBounds = true;
}

// Optional convenience overload using a Bounds
public void SetBounds(Bounds worldBounds)
{
    roomMin = new Vector2(worldBounds.min.x + boundsPadding.x, worldBounds.min.y + boundsPadding.y);
    roomMax = new Vector2(worldBounds.max.x - boundsPadding.x, worldBounds.max.y - boundsPadding.y);
    useBounds = true;
}
```

Caller: `SubRoomSequenceController` after painting the new sub-room while screen is black:
```csharp
var floorRenderer = newSubRoomFloorTilemap.GetComponent<Renderer>();
Camera.main.GetComponent<CameraFollow>().SetBounds(floorRenderer.bounds);
```

Pixel Perfect Camera + Branch E 6° camera tilt compatibility: orthographic size unchanged (still driven by `combatOrthographicSize = 5.15f`); `SetBounds` only updates world-space clamp rect. Tilt is applied by separate transform — bounds are evaluated in world XY and remain valid. Fade overlay is screen-space (`ScreenSpaceOverlay`), so tilt does not affect transition.

---

## 6. Save/load policy — MVP

**Locked decision: no mid-encounter save.**

- On run start / continue: save state granularity = `DungeonGraph.CurrentNodeId` + `EncounterCompletedFlag[nodeId] : bool`.
- Mid-encounter quit → encounter state (`currentSubRoomIndex`, kill counter, spawned enemies) is discarded. On reload, the run resumes at the macro node with the encounter freshly restarted from sub-room 0.
- Saved fields added (in whatever run-state SO/JSON already exists):
  - `int currentNodeId`
  - `Dictionary<int,bool> encounterCleared` keyed by node id, only set `true` on `OnEncounterComplete`.
- Phase 2: full mid-encounter save (sub-room index, kill counter, enemy persistence, RNG seed, gate states) — explicitly **out of scope**.

Rationale: encounters are 1-4 minutes of play. Restart from sub-room 0 on quit is acceptable for MVP; full state serialization triples the controller surface and is unjustified before the vertical slice ships.

---

## 7. EncounterBank handoff (interface only)

`EncounterBank` is a separate spec (next sprint). Only the consumer contract is locked here:

```csharp
public interface IEncounterBank
{
    bool TryResolve(RoomNode macroNode, string biomeId, out EncounterTemplateSO template);

    // Per sub-room: returns the enemy roster the controller should spawn at the EnemySpawnSocket positions.
    // Resolution time: when the sub-room begins loading (Transitioning → Loading boundary).
    SubRoomEnemyPlan GetSubRoomEnemies(string encounterId, int subRoomIndex);

    // Kill quota for advancing the gate. Default behavior = "all spawned dead" (return plan.Enemies.Count).
    int GetSubRoomKillQuota(string encounterId, int subRoomIndex);
}

public class SubRoomEnemyPlan
{
    public List<EnemyAssignment> Enemies;   // one per used EnemySpawnSocket
}

public class EnemyAssignment
{
    public string socketId;                 // matches EnemySpawnSocket.socketId
    public GameObject enemyPrefab;
    public bool isElite;
}
```

Spawn timing: hybrid as approved by Karar #149.
- **Sockets**: pre-authored in `RoomTemplateSO.enemySpawnSockets` (already exists). Authoring picks position + tier hint, never enemy identity.
- **Identity resolution**: `EncounterBank.GetSubRoomEnemies` called on sub-room enter (during the black-screen `SwapSubRoomWhileBlack` callback). Spawns happen behind the fade, no pop-in.
- **No pre-instantiation across hidden sub-rooms.** Each sub-room's enemies exist only while that sub-room is loaded. Teardown on transition.

If `EncounterBank.TryResolve` returns `false` for a Combat/Elite node, controller is not created; `LegacyRuntimeRoomManager` falls back to existing single-room logic (graceful degradation during incremental rollout).

EncounterBank full implementation, threat budget computation, wave-trigger logic (40% wave 1 budget, wave 2 at 50% wave 1 dead, max 4 same type — per canon) → separate spec, out of this scope.

---

## 8. Validator

`EncounterTemplateValidator.Validate(EncounterTemplateSO) : ValidationResult` — pure function, no side effects.

```csharp
public class ValidationResult
{
    public bool IsValid => Errors.Count == 0;
    public List<string> Errors = new();
    public List<string> Warnings = new();
}

public static class EncounterTemplateValidator
{
    public static ValidationResult Validate(EncounterTemplateSO et);
}
```

Editor menu: `Tools/RIMA/Validate Encounter` (single selected SO) and `Tools/RIMA/Validate All Encounters` (project-wide via `AssetDatabase.FindAssets("t:EncounterTemplateSO")`). Both print to Console + return error count as exit code in CI.

Error rules (max 5 categories, expanded inline from §1 validation rules):
1. Sub-room count range `[3,5]`. Out of range = error.
2. Entry/Final uniqueness — exactly 1 entry, exactly 1 final, distinct.
3. Link resolution — every `toSubRoomKey`, `fromDoorSocketId`, `toEntryDoorSocketId` resolves.
4. Reachability — BFS from entry visits every sub-room; final must be reached.
5. Spawn-socket physical validity — inside bounds, walkable, not occluded by any `PropPlacementData` AABB.

Add `Tools/RIMA/Encounter/Build From Selected Room Templates` helper (Phase 1 nice-to-have, low cost): user multi-selects 3-5 `RoomTemplateSO`, menu creates `EncounterTemplateSO` with auto-wired entry (index 0) + final (last) + linear links. Eases authoring of the 3-sub-room vertical slice.

---

## 9. Karar #143 spawn-pocket conflict mitigation

Conflict source: Karar #143 6-stage Multi-Layer Painter places edge-biased decoration (clusters near walls / room edges). Sub-room flank pockets place enemy spawns near edges. In a 16×10 room, the overlap region is ~3 tiles wide on each side — props can occlude flank spawns, breaking spawn validity (§1 rule 6) and visually cluttering enemy silhouettes (Karar #80 violation).

Solution: avoid-radius mask owned by `EnemySpawnSocket`.

```csharp
[Serializable]
public class EnemySpawnSocket
{
    public string socketId;
    public Vector2Int position;
    public string tierHint = "standard";
    public float avoidRadius = 1.5f;        // NEW — tiles; props/decals skip within this radius around position
}
```

`MapLayerOrchestrator.Paint(...)` consumer: before placing each prop or decal, query the room's enemy spawn sockets and reject candidate positions within `avoidRadius`. Implementation: `RoomData` already carries room-level data; extend it with `IReadOnlyList<EnemySpawnSocket> EnemyAvoidMask`, populated from `RoomTemplateSO.enemySpawnSockets` at room-load time. Each per-layer placement loop adds a check:

```csharp
foreach (var socket in room.EnemyAvoidMask)
{
    Vector2 socketWorld = TileToWorld(socket.position);
    if (Vector2.Distance(candidate.position, socketWorld) < socket.avoidRadius)
        return SkipPlacement;
}
```

This preserves Karar #143 edge-density visually (decoration still clusters on edges) but punches small holes where flank pockets need to read cleanly. Default 1.5 tiles ≈ 1 enemy silhouette diameter + 0.5 tile margin — enough for Karar #80 64px readability without gutting edge density.

Authoring discipline: in 16×10 rooms, no more than 2 flank pocket sockets per room (one per side), avoid-radius left at 1.5. In 12×8 connector rooms, 0-1 socket (connectors are not combat-heavy by design).

---

## 10. Implementation order — Codex hand-off

| # | Step | Files | Hours |
|---|---|---|---|
| 1 | `EncounterTemplateSO` + `SubRoomEntry` + `SubRoomLink` + Inspector + `EncounterTemplateValidator` + menu items | new files under `Assets/Scripts/MapDesigner/Encounter/` + `Assets/Editor/MapDesigner/EncounterMenu.cs` | 4-6 |
| 2 | `SubRoomSequenceController` state machine + enemy spawn handoff to EncounterBank stub | new files under `Assets/Scripts/Runtime/Encounter/` + thin `EncounterBankStub` returning a single fixed roster for vertical slice | 10-16 |
| 3 | `IntraEncounterDoorTrigger` + lock/unlock wiring | new file under `Assets/Scripts/Runtime/Encounter/` | 3-5 |
| 4 | `LegacyRuntimeRoomManager` reward gating fork (`isEncounterContext` + `isFinalSubRoom` + `OnEncounterFinalCleared` + controller bootstrap in `StartRoom`) | `Assets/Scripts/Core/LegacyRuntimeRoomManager.cs` (modify) | 3-5 |
| 5 | `CameraFollow.SetBounds(...)` + sub-room enter call | `Assets/Scripts/Player/CameraFollow.cs` (modify) + controller call site | 2 |
| 6 | 3 sub-room vertical slice — manual compose 3 `RoomTemplateSO` assets (entry warmup, flank pocket, final mix), one `EncounterTemplateSO`, wire into `EncounterBankStub` for one Combat node | data only + editor authoring time | 10-16 |
| 7 | Playtest + tune (kill quotas, gate feel, fade timing, camera bounds smoothness, Branch E tilt sanity check) | iteration | 1-2 days |

Sub-total engineering: 32-50 hours. Plus 1-2 design/art days. Aligns with Codex 5-7 day MVP estimate.

Add 1-2 EditMode tests to `RoomFlowTests.cs` per existing project rule:
- `EncounterTemplateValidator_RejectsOrphanSubRoom`
- `SubRoomSequenceController_FinalClearTriggersReward`
Mid-system PlayMode validation deferred to Step 7 playtest.

---

## 11. Karar #149 final wording (for MASTER_KARAR_BELGESI)

**Karar #149 — Combat Encounter Sub-Room Sequence (LIVE 2026-05-19)**

A Combat or Elite macro node may be implemented as one `EncounterTemplateSO` containing 3-5 connected `RoomTemplateSO` sub-rooms. Internal transitions use `RoomTransitionFX` fade-to-black and never advance `DungeonGraph`. Sub-rooms do not grant skill drafts, map fragments, Echo / Death Imprint cadence ticks, chests, or any macro-route reward — the macro reward sequence runs only after the final sub-room clears. Enemy identity and threat budget remain in `EncounterBank` (separate system); `RoomTemplateSO` continues to store only sockets, tags, masks, props, walkable data, and visual layers. Default combat sub-room size is 16×10 (Karar #80 silhouette readability + dash lanes preserved); 12×8 is allowed only for connector or low-threat pocket sub-rooms. Death Imprint signatures must record `(encounterId, currentSubRoomIndex, subRoomTag)` rather than the macro node alone, so the imprint cadence still ticks per macro encounter (Karar #27 preserved) while signature granularity improves. `EnemySpawnSocket.avoidRadius` (default 1.5 tiles) protects flank pockets from Karar #143 edge-biased decoration. No mid-encounter save in MVP — quit during encounter resets to entry sub-room on reload.

---

## 12. Conflict check

| Karar | Status | Note |
|---|---|---|
| #27 Echo / Death Imprint | **Preserved.** Cadence counts macro encounters only — one cadence tick per `OnEncounterComplete`, not per sub-room. Death Imprint signature, however, records `(encounterId, subRoomIndex, subRoomTag)` to make "died in flank pocket of node 6" a usable signature vs the weak "died in node 6". This is a strict enrichment, not a cadence change. |
| #80 Class Silhouette Bible | **Preserved.** 16×10 default sub-room size keeps dash lanes + 64px class readability. `EnemySpawnSocket.avoidRadius = 1.5` punches small clean holes around enemies so silhouettes read against Karar #143 edge decoration. 12×8 only for connectors / non-combat beats — no class fight ever happens in 12×8. |
| #143 6-layer Multi-Layer Painter | **Preserved with mitigation.** Edge-density placement logic unchanged. New `EnemyAvoidMask` filter at placement time skips cells within `avoidRadius` of flank sockets — visual density preserved, gameplay readability won. Resolution = additive (new filter), not subtractive (no painter logic deleted). |
| #147 Multi-Layer Painter LIVE | **Preserved.** `RoomTemplateSO.backgroundLayers` continues to be the per-room visual contract. Each sub-room loads its own `backgroundLayers` independently — sub-room visual diversity is a feature, not a conflict. Painter ownership unchanged. |
| #25 Meta progression | **Preserved (implicit).** Sub-room rewards = none. No hub-meta currency from sub-rooms. Only encounter-final macro reward feeds meta. |

No locked-rule violation. All resolutions are additive (new field, new filter, new code path) rather than rewriting existing systems.

---

## Final notes for orchestrator

- Spec stays within RIMA architecture; nothing here recommends EncounterBank implementation, threat budget math, or wave trigger logic — those belong to the next spec.
- `RoomTransitionFX`, `DungeonGraph`, `CameraFollow` are touched minimally (one new public method only on `CameraFollow`).
- `LegacyRuntimeRoomManager` keeps macro authority. Controller borrows the stage for sub-room runs and returns it cleanly on `EncounterComplete`.
- Phase 2 backlog explicitly: mid-encounter save (full state), encounter graph editor visualization, 12×8 connector sub-rooms with non-combat objective beats, Light2D presets per sub-room mood, EncounterBank dynamic wave triggers.

**ORCHESTRATOR NEXT STEP:** Codex Step 1 dispatch — `EncounterTemplateSO` + `SubRoomEntry` + `SubRoomLink` + Validator + Inspector + Menu.

Allowed file paths for Codex (Step 1 only):
- `Assets/Scripts/MapDesigner/Encounter/Data/EncounterTemplateSO.cs` (create)
- `Assets/Scripts/MapDesigner/Encounter/Data/SubRoomEntry.cs` (create)
- `Assets/Scripts/MapDesigner/Encounter/Data/SubRoomLink.cs` (create)
- `Assets/Scripts/MapDesigner/Encounter/EncounterTemplateValidator.cs` (create)
- `Assets/Editor/MapDesigner/EncounterMenu.cs` (create)
- `Assets/Editor/MapDesigner/EncounterTemplateSOEditor.cs` (create, optional minimal inspector)
- READ-ONLY refs: `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs`, `DoorSocket.cs`, `EnemySpawnSocket.cs`, `PropPlacementData.cs`
