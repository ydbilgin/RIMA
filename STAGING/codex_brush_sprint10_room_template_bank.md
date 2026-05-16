# Sprint 10 — RoomTemplate + RoomBank Vertical Slice
**Codex Task Spec v1.0 | 2026-05-16**

---

## Header

| Field | Value |
|---|---|
| **Task name** | Sprint 10 RoomTemplate + RoomBank Vertical Slice |
| **Owner** | Opus implement → Codex review (16-18 May window); spec written for Codex-as-reviewer |
| **Estimate** | 1–1.5 day |
| **Dependencies** | Sprint 9 `RoomTemplateV1` stub + existing `DungeonMap`/`RoomFlow` patterns |
| **Workflow override** | Opus implement, Codex final-pass review. Normal routing resumes 19 May. [[codex-as-reviewer-until-may18]] |
| **Source of truth** | `STAGING/sprite_strategy_FINAL_LOCK.md` §4 + §10; `STAGING/codex_meta_review.md` §Sprint 10 + §Risk #7 |

---

## 1. Context + Rationale

**S86 PREP-3 Opus signoff:** Sprint 9 delivered `RoomTemplateV1` as a **TRUE THIN 5-field stub** (`schemaVersion`, `roomId`, `biomeId`, `roomType` = `RIMA.RoomType` global enum, `bounds` only). Sprint 10 **ADDS helper types** (DoorSocket / PlayerSpawnSocket / EnemySpawnSocket / CameraBounds) — Sprint 9 stub'a dokunmaz, sadece extend eder. Bu sıralama Codex review blocker #1 fix'i: Sprint 9→10 arası type churn yok.

Sprint 10 fills that stub to a production-ready V1 and proves the full vertical loop:

```
author room → save prefab+SO → reload → RoomBank.Pick → PlayMode spawn → exit validate
```

**Why before Natural Engine (Sprint 11):** If the save/load/RoomBank loop is broken, Natural Engine investment is wasted. Prove the game loop first. [[room-library-architecture]] [[brush-tool-v1-design]]

**RoomTemplate ≠ Encounter (non-negotiable):** Room exposes sockets + tags only. EncounterBank is a separate system. Runtime: `RoomBank ∪ EncounterBank → combine`. No enemy identity lives in `RoomTemplateSO`. [[karar-143-layered-pipeline]]

---

## 2. Deliverables — Exhaustive Checklist

### 2.1 `RoomTemplateSO` V1 Full

Full data model (NOT the Sprint 9 3-field placeholder):

```csharp
/// <summary>
/// Single room definition. Prefab holds visual hierarchy; SO holds metadata + sockets.
/// Pure SO: BANNED. Scene-per-room: BANNED. [[room-library-architecture]]
/// Sprint 9 stub: schemaVersion / roomId / biomeId / roomType (RIMA.RoomType global) / bounds.
/// Sprint 10 ADDS: doorSockets, playerSpawn, enemySpawnSockets, cameraBounds, prefabRef, *Tags.
/// </summary>
[CreateAssetMenu(menuName = "RIMA/Room/RoomTemplate")]
public class RoomTemplateSO : ScriptableObject
{
    // Sprint 9 stub fields (unchanged):
    public string schemaVersion;           // "1.0" — migration tests key on this
    public string roomId;                  // "combat_shatteredkeep_001" — stable, human-readable
    public string biomeId;                 // "ShatteredKeep"
    public RIMA.RoomType roomType;         // **GLOBAL ENUM REUSE** — Combat/Elite/Boss/Merchant/Event/Curse/Chest/Forge/Corridor
    public RectInt bounds;                 // tile-space; (0,0) origin, width/height in tiles

    // Sprint 10 additions (NEW in this sprint):
    public List<DoorSocket> doorSockets;
    public PlayerSpawnSocket playerSpawn;
    public List<EnemySpawnSocket> enemySpawnSockets;
    public CameraBounds cameraBounds;
    public GameObject prefabRef;           // SO+Prefab hybrid; null = authoring incomplete
    public List<string> encounterTags;     // ["elite_wave","ambush"] — no enemy identity
    public List<string> difficultyTags;    // ["hard","endgame"]
    public List<string> blockerTags;       // ["no_shrine","no_boss"]
}
```

**RoomType mapping (Opus signoff):** Global `RIMA.RoomType` reuse. Sprint 9 spec orijinal `{ Combat, Shop, Shrine, Elite, Boss }` enum iptal. V1 RoomBank surface: Combat / Elite / Boss / **Merchant** (Shop yerine) / **Event** (Shrine yerine). Chest/Forge/Curse/Corridor V1+ opsiyonel.

Why `roomId` pattern `{roomType}_{biomeId}_{index}`: deterministic, diffable, no GUID collisions in RoomBank. IDs must be stable across reimport. [[room-library-architecture]]

### 2.2 Helper Types

```csharp
/// <summary>Door connection point. direction = which wall this door is on.</summary>
[Serializable]
public class DoorSocket
{
    public string socketId;         // "door_N_01" — stable, deterministic
    public Vector2Int position;     // tile-space center of door
    public DoorDirection direction; // enum: North / South / East / West
    public int widthInTiles;        // default 2
}

// Open question: DoorDirection enum vs Vector2Int normalized — see §6.

/// <summary>Where the player enters this room.</summary>
[Serializable]
public class PlayerSpawnSocket
{
    public string socketId;         // "player_spawn_01"
    public Vector2Int position;     // tile-space
    public DoorDirection facing;    // initial facing direction on spawn
}

/// <summary>Candidate enemy spawn. Tier is a hint to EncounterBank, not a mandate.</summary>
[Serializable]
public class EnemySpawnSocket
{
    public string socketId;         // "enemy_spawn_01"
    public Vector2Int position;     // tile-space
    public string tierHint;         // "standard" / "elite" / "boss" — string tag, not enum
    // Open question: string tag vs enum — see §6.
}

/// <summary>Camera confine bounds for this room.</summary>
[Serializable]
public struct CameraBounds
{
    public RectInt tileRect;        // tile-space; must contain all walkable tiles
    // Open question: tile-space RectInt vs world-space Bounds — see §6.
}
```

### 2.3 `RoomBankSO`

```csharp
/// <summary>
/// **Editor-authored runtime-readable asset.** (Opus signoff S86 PREP-3.)
/// - Authored: Editor only (RoomTemplateSaver, RoomTemplateLoader Editor-only assemblies).
/// - Read: Runtime safe — RoomBank.Pick callable from runtime code.
/// - NO runtime procedural room generation. [[room-library-architecture]]
/// </summary>
[CreateAssetMenu(menuName = "RIMA/Room/RoomBank")]
public class RoomBankSO : ScriptableObject
{
    // V1 surface (RIMA.RoomType subset):
    public List<RoomTemplateSO> combatRooms;
    public List<RoomTemplateSO> eliteRooms;
    public List<RoomTemplateSO> bossRooms;
    public List<RoomTemplateSO> merchantRooms;  // ex-shopRooms (RIMA.RoomType.Merchant)
    public List<RoomTemplateSO> eventRooms;     // ex-shrineRooms (RIMA.RoomType.Event)
    // V1+ optional: chestRooms / forgeRooms / curseRooms / corridorRooms

    /// <summary>Deterministic pick. Same seed + roomType → same room always.</summary>
    public RoomTemplateSO Pick(RoomType roomType, int seed);

    /// <summary>Returns structured validation issues across all registered rooms.</summary>
    public List<RoomValidationIssue> ValidateAll();
}
```

Why `List<RoomTemplateSO>` per type (not Addressables in V1): keeps Sprint 10 scope tight. Addressable-ready means no direct instantiation from bank — always go through `Pick` → `prefabRef`. Swap to Addressable handles in V1.5 without API change. Open question: §6.

### 2.4 Editor Utility — `RoomTemplateSaver`

```csharp
/// <summary>
/// EDITOR ONLY. Never referenced from runtime code.
/// Saves current authoring scene root as prefab + SO pair.
/// Save path: Assets/Data/Rooms/{biomeId}/{roomId}.prefab + .asset
/// GUID must be preserved on resave (in-place update, not delete/recreate).
/// Child naming: deterministic — "{layer}_{type}_{index:000}" pattern.
/// </summary>
public static class RoomTemplateSaver   // Editor namespace
{
    public static SaveResult SaveRoom(
        GameObject authoringRoot,
        RoomTemplateSO template,
        bool overwriteExisting = true);
}
```

Why deterministic child naming: prefab diffs in git become unreadable with auto-named children (GameObject(1), GameObject(2)...). [[codex-meta-review]] Risk #6.

### 2.5 Editor Utility — `RoomTemplateLoader`

```csharp
/// <summary>
/// EDITOR ONLY. Loads a saved RoomTemplateSO prefab into a clean authoring scene/root.
/// Does not modify runtime state.
/// </summary>
public static class RoomTemplateLoader  // Editor namespace
{
    public static LoadResult LoadIntoAuthoringScene(RoomTemplateSO template);
}
```

### 2.6 Runtime Test Loader — `RoomBankRuntimeTester`

```csharp
/// <summary>
/// Runtime MonoBehaviour for PlayMode integration test.
/// Not production code — test harness only.
/// Flow: Pick combat room → Instantiate prefab → spawn player at PlayerSpawnSocket
///       → spawn 1 placeholder enemy at first EnemySpawnSocket → validate exit socket exists.
/// No nav check. Data validation only.
/// </summary>
public class RoomBankRuntimeTester : MonoBehaviour
{
    public RoomBankSO bank;
    public GameObject playerPrefab;
    public GameObject enemyPlaceholderPrefab;
    public int testSeed = 42;

    // Called by PlayMode test runner.
    public RoomTestResult RunTest();
}
```

### 2.7 `RoomTemplateValidator`

```csharp
/// <summary>
/// Severity-typed validation. Error = BLOCK (hard fail). Warning = advise, allow. Info = advisory.
/// Art-quality failure as Error: BANNED. [[sprite_strategy_FINAL_LOCK]] §0 CORE LOCKS.
/// </summary>
public static class RoomTemplateValidator
{
    public static List<RoomValidationIssue> Validate(RoomTemplateSO template);
    public static List<RoomValidationIssue> ValidateBank(RoomBankSO bank);
}

[Serializable]
public class RoomValidationIssue
{
    public ValidationSeverity severity;  // Error / Warning / Info
    public string code;                  // "ERR_NO_PLAYER_SPAWN", "WARN_SMALL_BOUNDS", etc.
    public string message;
    public string roomId;
}
```

**Error rules (BLOCK — hard fail):**
- Exactly 1 `PlayerSpawnSocket` required; 0 or 2+ = Error `ERR_NO_PLAYER_SPAWN` / `ERR_MULTIPLE_PLAYER_SPAWN`
- 0 `DoorSocket` with exit role = Error `ERR_NO_EXIT_SOCKET`
- `CameraBounds.tileRect` does not contain at least 1 walkable tile = Error `ERR_CAMERA_BOUNDS_NO_WALKABLE`
- Any `EnemySpawnSocket.position` falls inside a prop footprint cell = Error `ERR_ENEMY_IN_PROP_FOOTPRINT`
- `prefabRef` is null = Error `ERR_MISSING_PREFAB_REF`
- `biomeId` is null or empty = Error `ERR_MISSING_BIOME_ID`
- Duplicate `roomId` found in same `RoomBankSO` = Error `ERR_DUPLICATE_ROOM_ID`

**Warning rules (advise, allow):**
- `bounds` width or height < 6 = Warning `WARN_SMALL_BOUNDS`
- Aspect ratio outside 0.5–2.0 = Warning `WARN_UNUSUAL_ASPECT`
- `encounterTags` is empty = Warning `WARN_NO_ENCOUNTER_TAGS` (allowed, flagged)

**Info rules (advisory):**
- Room dimensions in tiles
- Socket count per type
- Tag coverage summary

### 2.8 Test Suite

**EditMode — Save/Load Roundtrip** (`RoomTemplateSaveLoadTests.cs`):
- Create `RoomTemplateSO` in memory with all fields populated
- `RoomTemplateSaver.SaveRoom(...)` → write prefab + SO to temp path
- Clear references
- Reload via `AssetDatabase`
- Assert: all fields equal, no null sockets, `schemaVersion == "1.0"`
- GUID stability: call `SaveRoom` again → assert asset GUID unchanged

**EditMode — RoomBank Pick Determinism** (`RoomBankPickTests.cs`):
- Populate bank with 3 stub `RoomTemplateSO` combat entries
- `Pick(RoomType.Combat, seed: 42)` × 10 → assert all results identical
- `Pick(RoomType.Combat, seed: 99)` → may differ from seed 42 (test both don't crash)

**PlayMode — Runtime Spawn** (`RoomBankRuntimeSpawnTests.cs`):
- `RoomBankRuntimeTester.RunTest()` in Play mode
- Assert: player GameObject at `PlayerSpawnSocket.position` (within 0.1 world unit)
- Assert: enemy placeholder GameObject at `EnemySpawnSocket[0].position`
- Assert: at least 1 `DoorSocket` exists with exit direction set
- Assert: no exceptions thrown during spawn

**Test fixture asset** (stub, Codex may generate):
`Assets/Data/Rooms/ShatteredKeep/combat_shatteredkeep_test_001.asset`

---

## 3. File Scope (LOCK)

Codex may only touch these files. Any other file requires orchestrator approval.

| Path | Status |
|---|---|
| `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs` | NEW |
| `Assets/Scripts/MapDesigner/Room/Data/RoomBankSO.cs` | NEW |
| `Assets/Scripts/MapDesigner/Room/Data/DoorSocket.cs` | NEW |
| `Assets/Scripts/MapDesigner/Room/Data/PlayerSpawnSocket.cs` | NEW |
| `Assets/Scripts/MapDesigner/Room/Data/EnemySpawnSocket.cs` | NEW |
| `Assets/Scripts/MapDesigner/Room/Data/CameraBounds.cs` | NEW |
| `Assets/Scripts/MapDesigner/Room/Editor/RoomTemplateSaver.cs` | NEW — Editor namespace only |
| `Assets/Scripts/MapDesigner/Room/Editor/RoomTemplateLoader.cs` | NEW — Editor namespace only |
| `Assets/Scripts/MapDesigner/Room/Validation/RoomTemplateValidator.cs` | NEW |
| `Assets/Scripts/MapDesigner/Room/Runtime/RoomBankRuntimeTester.cs` | NEW — test harness only |
| `Assets/Tests/EditMode/Room/RoomTemplateSaveLoadTests.cs` | NEW |
| `Assets/Tests/EditMode/Room/RoomBankPickTests.cs` | NEW |
| `Assets/Tests/PlayMode/Room/RoomBankRuntimeSpawnTests.cs` | NEW |
| `Assets/Data/Rooms/ShatteredKeep/combat_shatteredkeep_test_001.asset` | NEW — optional stub |

**EditMode test temp assets** must write under `Assets/TempTests/Room/...` and clean up via `AssetDatabase.DeleteAsset` in `[TearDown]`. Never pollute `Assets/Data/` from tests.

---

## 4. Forbidden — Sprint 11+ Scope

Codex must NOT implement any of the following in Sprint 10:

- `CompositionRoleMap` or any Natural Engine composition logic
- Bridson Poisson sampler
- Density mask sampling beyond existing `FeatureMaskSO`
- `PropDefinitionSO` or any Props Mode code
- SpriteAtlas integration or per-biome packing
- AI tag suggestion (any form)
- Auto-Dress integration with `RoomBankSO`
- Markov clustering or sub-template rooms

Violation of this list = immediate review fail, no exceptions.

---

## 5. Exit Criteria

Codex's review PASS gate. All must be green — no partial credit.

| # | Criterion | How to verify |
|---|---|---|
| EC-1 | `dotnet build` PASS, zero new errors | CI output |
| EC-2 | All existing Brush V1 tests still green (37+ tests) | `dotnet test` |
| EC-3 | 1 `RoomTemplateSO` authored → saved → cleared → reloaded → all fields restored | `RoomTemplateSaveLoadTests` PASS |
| EC-4 | GUID stability: resave does not change asset GUID | GUID stability sub-test in EC-3 suite |
| EC-5 | `RoomBank.Pick` deterministic: same seed → same room | `RoomBankPickTests` PASS |
| EC-6 | PlayMode: player spawns at `PlayerSpawnSocket`, 1 enemy at `EnemySpawnSocket[0]`, exit socket data valid | `RoomBankRuntimeSpawnTests` PASS |
| EC-7 | `RoomTemplateValidator` returns `List<RoomValidationIssue>` with typed severity, not raw exception strings | Manual inspect + unit test |
| EC-8 | No editor-only class referenced from runtime assembly | Compile with `UNITY_EDITOR` stripped — no error |
| EC-9 | No runtime non-integer scale applied anywhere in Sprint 10 code | Code review — no `transform.localScale` with non-integer float except (1,1,1) |
| EC-10 | Sorting layers Patch / Detail / Accent / Props / Entities used where applicable; `RimaSortingLayerValidator` passes | Validator output |

The vertical loop sentence: **one room saves, reloads, loads through RoomBank in PlayMode, player + enemy spawn at sockets, exit data valid.** If any link in this chain is broken, Sprint 10 is not done.

---

## 6. Open Questions — Opus Signoff Required Before Implementation

These must be resolved before Codex writes a single line of runtime code.

| # | Question | Options | Stakes |
|---|---|---|---|
| OQ-1 | `DoorSocket.direction`: `DoorDirection` enum (N/S/E/W) vs `Vector2Int` normalized | Enum: simpler, V1 safe. Vector2: extensible to diagonal. **Recommendation: enum V1, extend later.** | Socket ID generation, Wang tile adjacency |
| OQ-2 | `CameraBounds`: `RectInt` tile-space vs `Bounds` world-space | Tile-space: consistent with `bounds` field, integer-clean. World: Cinemachine-friendly. **Recommendation: RectInt tile-space, convert at runtime.** | EC-10 validation, Cinemachine confiner |
| OQ-3 | `RoomBankSO` lazy load: `AssetReference` (Addressable) vs direct `RoomTemplateSO` ref | Direct ref: simpler V1, no Addressable setup cost. Addressable: necessary if room count > 60 and memory matters. **Recommendation: direct ref V1, field name future-proof.** | Memory, load time |
| OQ-4 | `EnemySpawnSocket.tierHint`: `string` tag vs enum (`TierHint.Standard/Elite/Boss`) | String: EncounterBank extensible, no recompile on new tier. Enum: compile-safe, IDE autocomplete. **Recommendation: string tag V1 per RoomTemplate ≠ Encounter principle.** | EncounterBank API surface |

---

## 7. Review Checklist (Codex Review Pass)

When Opus submits implementation, Codex reviews against this list:

- [ ] All Sprint 10 exit criteria (§5) verified — no partial
- [ ] `RoomTemplate ≠ Encounter` principle holds: no enemy type, prefab, or spawn logic owned by `RoomTemplateSO`
- [ ] All IDs (`roomId`, `socketId`) are deterministic and human-readable, not GUID strings
- [ ] GUID preservation actually implemented in `RoomTemplateSaver` (in-place update, not delete/recreate)
- [ ] `ValidationSeverity.Error` blocks hard; `Warning` allows; no art-quality check is `Error`
- [ ] No editor-only class (`RoomTemplateSaver`, `RoomTemplateLoader`) referenced from any runtime assembly path
- [ ] `RoomBankSO` exposes a clear path to Sprint 11: `CompositionRoleMap` can query `RoomTemplateSO` sockets without restructuring `RoomBankSO`
- [ ] EditMode tests use temp paths under `Assets/TempTests/Room/...` and clean up in `[TearDown]`
- [ ] All open questions (§6) answered or explicitly deferred with a fallback

---

## 8. References

- `STAGING/sprite_strategy_FINAL_LOCK.md` §4 Data Model, §10 Sprint 10, §14 Workflow Override
- `STAGING/codex_meta_review.md` §Sprint 10 Deliverables, §Risk #7 RoomTemplate, §Risk #6 GUID
- Memory: [[room-library-architecture]] [[brush-tool-v1-design]] [[karar-143-layered-pipeline]] [[codex-as-reviewer-until-may18]]
- Sprint 9 spec: `STAGING/codex_brush_sprint9_atlas_importer.md`
- Sprint 11 forward ref: `CompositionRoleMap` must not require `RoomBankSO` restructure

---

*End of Sprint 10 spec. Codex review dispatch: `cx_dispatch.py --task-file STAGING/codex_brush_sprint10_room_template_bank.md --effort high`*
