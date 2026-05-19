---
name: room-library-architecture
type: project
description: "LOCK S86 + Sprint 10 LIVE: Editor-only RoomBank, SO+Prefab hybrid, 20-30 oda/tip MVP, RoomTemplate != Encounter."
metadata:
  node_type: memory
  type: project
  originSessionId: m2-memory-consolidation-s86-prep3
---

# Room Library Architecture — LOCK S86 PREP-3 + Sprint 10 LIVE

**Status:** Sprint 10 implementation LIVE (2026-05-16 S86 SPRINT10_IMPL — Opus impl, Codex review pending). Karar otoritesi: sprite_strategy_FINAL_LOCK.md (TEK YETKI).
**Sprint scope:** Sprint 10 vertical slice (RoomTemplate + RoomBank) LIVE, Sprint 12 Props Mode pending.

## Sprint 10 LIVE Files (2026-05-16)

- `Assets/Scripts/MapDesigner/Room/Data/` — DoorSocket, PlayerSpawnSocket, EnemySpawnSocket, CameraBounds, RoomBankSO, RoomTemplateSO (extended from Sprint 9 5-field stub)
- `Assets/Scripts/MapDesigner/Room/Validation/` — RoomValidationIssue (+ ValidationSeverity), RoomTemplateValidator
- `Assets/Scripts/MapDesigner/Room/Editor/` — SaveLoadResults, RoomTemplateSaver (in-place GUID-preserving), RoomTemplateLoader (all `#if UNITY_EDITOR`)
- `Assets/Scripts/MapDesigner/Room/Runtime/RoomBankRuntimeTester.cs`
- Tests: `Assets/Tests/EditMode/Room/{RoomTemplateSaveLoadTests,RoomBankPickTests}.cs` + `Assets/Tests/PlayMode/Room/RoomBankRuntimeSpawnTests.cs`

**OQ resolutions applied (spec §6):**
- DoorDirection enum REUSED from existing `RIMA.DoorDirection` (DoorTrigger.cs) — no duplicate
- CameraBounds: RectInt tile-space (consistent with bounds field)
- RoomBank: direct ref (Addressable swap deferred to V1.5)
- EnemySpawnSocket.tierHint: string tag (preserves RoomTemplate != Encounter)

**ERR_ENEMY_IN_PROP_FOOTPRINT DEFER:** Sprint 10'da prop data §4 forbidden (PropDefinitionSO Sprint 11+). Yerine `ERR_ENEMY_OUT_OF_BOUNDS` (bounds containment) impl.

## Core Locks

**Why:** Runtime procedural room drawing RIMA icin yanlis risk. Oyun curated combat readability, wall closure, prop collision, spawn sockets ve camera bounds gerektiriyor — runtime novelty degil.
**How to apply:** RoomBank'i editor-only tut. Runtime'da RoomBank.Pick → prefab instantiate → validation. Hicbir runtime procedural room drawing kodu yazma.

| Karar | LOCK |
|---|---|
| RoomBank modu | Editor-only — NO runtime procedural room drawing |
| Storage | SO + Prefab hybrid (RoomTemplateSO + prefabRef) |
| MVP target | 20-30 oda/tip (Combat/Shop/Shrine/Elite/Boss). Sonraki gate: 40-60 combat. **100 oda hedefi YOK V1.** |
| Scene-per-room | YASAK (versiyon kontrol + navigasyon friction at 100-300 oda) |
| Pure SO | YASAK (visual authoring painful without prefab) |

## RoomTemplate != Encounter (Encounter Decoupling)

**Why:** Room'un encounter'i dogrudan icermesi 40 odayi finite yapar. Decoupling ile 40 oda x N encounter = combinatorial variety. Yeni room kategorisi eklenirse encounter logic kirilmaz.
**How to apply:** RoomTemplate sadece sockets/tags expose eder. EncounterBank ayri sistem. Runtime'da RoomBank ∪ EncounterBank combine.

## RoomTemplateSO V1 Fields

```csharp
public class RoomTemplateSO : ScriptableObject
{
    public string schemaVersion;        // "1.0"
    public string roomId;               // human-readable stable: "combat_shatteredkeep_001"
    public string biomeId;
    public RoomType roomType;           // Combat / Shop / Shrine / Elite / Boss
    public RectInt bounds;              // tile-space room bounds
    public List<DoorSocket> doorSockets;
    public PlayerSpawnSocket playerSpawn;
    public List<EnemySpawnSocket> enemySpawnSockets;
    public CameraBounds cameraBounds;
    public GameObject prefabRef;        // SO + Prefab hybrid
    public List<string> encounterTags;  // RoomTemplate != Encounter; tags only
    public List<string> difficultyTags;
    public List<string> blockerTags;
}
```

## RoomBankSO

- Room type lists (Combat / Shop / Shrine / Elite / Boss ayrimi)
- Deterministic random pick by seed
- Validation report (summary per room type)
- Lazy refs veya Addressable-ready references

## Validation Rules (V1 — Error = BLOCK)

**Why:** Yanlis validation gecen oda runtime'da crash veya oynanamaz durum uretir.
**How to apply:** Sprint 10 importer ve Sprint 13 batch gate oncesi her odaya bu kurallar uygulanmali.

- Exactly 1 player spawn
- ≥1 exit socket
- Camera bounds camera bounds contains walkable bounds
- No enemy spawn inside prop footprint
- All dependencies exist (sprites, templates, GUIDs)

## Deterministic IDs

**Why:** ID generation afterthought olursa RoomTemplate diff ve migration cok gurultulu olur.
**How to apply:** Variant ID, Room ID, Prop instance ID, Socket ID — stable + human-readable. Importer reimport'ta GUID preserve etmeli (delete/recreate YASAK).

- Variant ID pattern: `{pool}_{tag}` (ornek: `L4_moss_patch_hero`)
- Room ID pattern: `{roomType}_{biomeId}_{index}` (ornek: `combat_shatteredkeep_001`)
- Importer GUID preserve on reimport — AssetPoolSO in-place update

## Sprint 10 Vertical Slice Gate

**Why:** Natural Engine'e yatirim yapmadan once RoomBank'in calistigini kanitlamak gerekiyor. Calismiyor ise Natural Engine masrafi bosa gider.
**How to apply:** Sprint 10 exit criteria gecmeden Sprint 11'e gecme.

Exit: `RoomBank.Pick → spawn player + 1 placeholder enemy + exit valid` → PlayMode test PASS.

Full criteria:
- 1 room paint/import → save → reload → RoomBank pick → PlayMode load → exit valid
- Save/load roundtrip test passes
- No editor-only class runtime dependency

## MVP vs V2 Split

**V1 (RIMA shipping):**
- 20-30 oda/tip starter library
- RoomTemplateSO + RoomBank + Props Mode
- 5 room types (Combat/Shop/Shrine/Elite/Boss)
- 1 biome (ShatteredKeep)

**V2 (post-ship):**
- 40-60 combat rooms (sonraki gate)
- 100 combat rooms hedefi
- Markov sub-template clustering
- Multi-biome library

## Cross-links

[[brush-tool-v1-design]] [[karar-143-layered-pipeline]]
