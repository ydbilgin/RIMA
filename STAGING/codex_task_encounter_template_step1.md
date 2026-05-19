# Codex Task — Encounter Template SO + Validator (Step 1 of 7)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

---

## Mission

Implement `EncounterTemplateSO` + `SubRoomEntry` + `SubRoomLink` data model, `EncounterTemplateValidator`, a minimal custom Inspector (Validate button + HelpBox), and two Editor Menu items, per Opus spec Karar #149 LIVE. This is Step 1 of 7 in the encounter sub-room system. All classes live in namespace `RIMA.MapDesigner.Encounter`. Stop after Step 1 — do not start Step 2 (SubRoomSequenceController runtime), which is a separate dispatch.

## Source spec

Full spec: `STAGING/SUBROOM_ENCOUNTER_TECH_SPEC_OPUS.md`. Read it before coding. This task = Step 1 of 7 in section 10. Stop after Step 1.

## Files to create

- `Assets/Scripts/MapDesigner/Encounter/Data/EncounterTemplateSO.cs`
- `Assets/Scripts/MapDesigner/Encounter/Data/SubRoomEntry.cs`
- `Assets/Scripts/MapDesigner/Encounter/Data/SubRoomLink.cs`
- `Assets/Scripts/MapDesigner/Encounter/EncounterTemplateValidator.cs`
- `Assets/Editor/MapDesigner/EncounterMenu.cs`
- `Assets/Editor/MapDesigner/EncounterTemplateSOEditor.cs`

## Files to read (no modifications)

- `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs`
- `Assets/Scripts/MapDesigner/Room/Data/DoorSocket.cs`
- `Assets/Scripts/MapDesigner/Room/Data/EnemySpawnSocket.cs`
- `Assets/Scripts/MapDesigner/Room/Data/PropPlacementData.cs`
- `STAGING/SUBROOM_ENCOUNTER_TECH_SPEC_OPUS.md` (section 1 + section 8)

## Implementation requirements

### SubRoomEntry

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
```

### SubRoomLink

```csharp
[Serializable]
public class SubRoomLink
{
    public string fromDoorSocketId;           // matches RoomTemplateSO.doorSockets[].socketId on source
    public string toSubRoomKey;               // destination SubRoomEntry.subRoomKey
    public string toEntryDoorSocketId;        // matches RoomTemplateSO.doorSockets[].socketId on destination (player spawn-side)
    public bool requiresClear = true;         // gate locked until source sub-room kill-quota met
}
```

### EncounterTemplateSO

```csharp
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

Validation rules (all 8, enforced by Validator):
1. `subRooms.Count` in `[3,5]`. Outside range = error.
2. Exactly 1 `isEntry == true`, exactly 1 `isFinal == true`. Not the same entry.
3. Every `SubRoomLink.toSubRoomKey` resolves to an existing `SubRoomEntry.subRoomKey`.
4. Every `fromDoorSocketId` / `toEntryDoorSocketId` resolves to a `DoorSocket.socketId` on the referenced room.
5. Graph reachability: BFS from entry must visit every sub-room including final. No orphans, no unreachable final.
6. Per sub-room: every `EnemySpawnSocket.position` lies in `bounds`, is `IsWalkable(...)`, and is not occluded by a prop (`PropPlacementData` AABB intersection check). Error: "spawn socket S blocked by prop P".
7. `macroRoomType` must be `Combat` or `Elite`.
8. `encounterId` non-empty, unique among all loaded `EncounterTemplateSO` assets in project.

### EncounterTemplateValidator

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

Pure function, no side effects. 5 rule categories in Validate:
1. Sub-room count range `[3,5]`
2. Entry/Final uniqueness — exactly 1 entry, exactly 1 final, distinct
3. Link resolution — every `toSubRoomKey`, `fromDoorSocketId`, `toEntryDoorSocketId` resolves
4. Reachability — BFS from entry visits every sub-room; final must be reachable
5. Spawn-socket physical validity — inside bounds, walkable, not occluded by `PropPlacementData` AABB

### Inspector (minimal)

File: `EncounterTemplateSOEditor.cs`. `CustomEditor` for `EncounterTemplateSO`.
- Call `EditorGUILayout.PropertyField` on `subRooms` (renders as ReorderableList via Unity default).
- Add a "Validate" button: runs `EncounterTemplateValidator.Validate`, prints result to Console, shows `EditorGUILayout.HelpBox` with error/warning summary.
- No node-graph visual. No `EditorUtility.DisplayDialog` (BANNED — use Debug.Log).

### Editor Menu

File: `EncounterMenu.cs`.
- `Tools/RIMA/Validate Encounter` — validates single selected `EncounterTemplateSO`, prints to Console.
- `Tools/RIMA/Validate All Encounters` — `AssetDatabase.FindAssets("t:EncounterTemplateSO")`, validates all, prints per-asset results + total error count.
- `Tools/RIMA/Encounter/Build From Selected Room Templates` — user multi-selects 3-5 `RoomTemplateSO`, creates `EncounterTemplateSO` with auto-wired entry (index 0) + final (last) + linear links. Nice-to-have, include in Step 1.

## Acceptance criteria

- All 6 files compile — run `dotnet build` after script creation; check `read_console` for zero errors.
- ScriptableObject creatable via `Assets/Create/RIMA/Encounter/EncounterTemplate`.
- Validator catches: bad sub-room count, missing entry/final, bad socket refs, unreachable sub-rooms.
- 2 EditMode tests added to `RoomFlowTests.cs`:
  - `EncounterTemplateValidator_RejectsOrphanSubRoom` — one sub-room not reached by BFS from entry → Validate returns errors.
  - `EncounterTemplateValidator_RequiresExactlyOneEntryAndFinal` — 0 or 2+ entries → Validate returns errors.
- `read_console` shows zero compile errors after domain reload.

## Out of scope (Step 2+)

- `SubRoomSequenceController` runtime
- `IntraEncounterDoorTrigger`
- `LegacyRuntimeRoomManager` reward fork
- `CameraFollow.SetBounds`
- `EnemySpawnSocket.avoidRadius` field addition
- Actual sub-room composition / vertical slice

## Hard rules

- DO read `STAGING/SUBROOM_ENCOUNTER_TECH_SPEC_OPUS.md` before coding — do not invent fields
- DO use namespace `RIMA.MapDesigner.Encounter` for all new classes
- DO compile-check after script create — `read_console` for errors before continuing
- DO NOT touch any file outside the allowlist above
- DO NOT implement Step 2 runtime controller
- DO NOT skip the validator — it gates authoring correctness
- DO NOT call `EditorUtility.DisplayDialog` — use `Debug.Log` only

## Effort

~4-6 hours per Opus estimate. Mechanical data model + validator logic + simple Editor utility.
