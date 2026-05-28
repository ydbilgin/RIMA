# T3-F1: JSON Schema Lock + RoomLayoutSerializer

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
T3 path implementation F1: JSON layout schema lock (semver 1.0) + RoomLayoutSerializer Editor side. T3 Tool .exe ↔ Game .exe JSON bridge contract foundation.

## Bağlam
- T3 design: `STAGING/T3_TOOL_FULL_DESIGN.md` (509 satır, F1-F7 plan)
- Defaults locked: Resources asset load / semver 1.0 JSON / 100ms debounce / normal window / registry-only palette
- Oda transitions impl PASS — RoomManifestSO çakışma riski bitti
- Mevcut altyapı: `Assets/Scripts/Map/Data/RoomManifestSO.cs` (LIVE, jsonLayout TextAsset)

## İş kalemleri

### 1. JSON schema lock (semver 1.0)
- Schema:
  ```json
  {
    "schema_version": "1.0",
    "room_id": "phase1_room1_tutorial",
    "metadata": {
      "name": "Tutorial Combat",
      "created": "2026-05-27T22:00:00Z",
      "modified": "2026-05-27T22:00:00Z"
    },
    "floor_tiles": [
      { "cell": [x, y, z], "tile_guid": "..." }
    ],
    "cliff_cells": [
      { "cell": [x, y, z], "is_decor": false }
    ],
    "prop_instances": [
      { "prefab_guid": "...", "position": [x, y, z], "rotation": 0 }
    ],
    "collider_overrides": [
      { "instance_id": "...", "size": [w, h], "offset": [x, y], "shape": "Box" }
    ]
  }
  ```
- Schema validation Newtonsoft.Json schema (Unity built-in JsonUtility yetersizse Newtonsoft package)

### 2. RoomLayoutSerializer.cs (Editor side)
- File: `Assets/Editor/RoomPainter/LiveTool/RoomLayoutSerializer.cs` NEW (~120 LOC)
- Static class:
  - `Serialize(Scene scene, out string json)` — sahnedeki Tilemap + prop GameObject envanteri JSON'a çevir
  - `Deserialize(string json, Scene targetScene)` — JSON'dan sahne content restore
  - GUID-based asset reference (AssetDatabase.GUIDFromAssetPath)
  - Versioning check: schema_version mismatch → migration veya error
- Singleton output: `Application.streamingAssetsPath/live/room_current.json`

### 3. RoomManifestSO.cs schema extend
- File: `Assets/Scripts/Map/Data/RoomManifestSO.cs` (mevcut)
- Yeni field: `[SerializeField] private string schemaVersion = "1.0";`
- Validation method `IsCompatibleSchema(string incomingVersion)`
- Migration stub Phase 2 (semver upgrade)

### 4. Newtonsoft.Json package check
- `Packages/manifest.json` `com.unity.nuget.newtonsoft-json` var mı kontrol
- Yoksa add: `"com.unity.nuget.newtonsoft-json": "3.2.1"` ekle
- Veya JsonUtility yeterliyse skip (basit schema için yeterli olabilir)

## Dosyalar (scope)
- `Assets/Editor/RoomPainter/LiveTool/RoomLayoutSerializer.cs` NEW (~120 LOC)
- `Assets/Scripts/Map/Data/RoomManifestSO.cs` (EXTEND ~10 LOC schemaVersion + validation)
- `Packages/manifest.json` (Newtonsoft.Json ekle gerekirse)
- `Assets/StreamingAssets/live/` (directory create, .gitkeep)
- Toplam ~140 LOC + 1 directory

## YASAK
- Player Build / Game side impl (T3-F5 scope)
- Tool .exe UI Toolkit Runtime (T3-F3 scope)
- Asset GUID registry baker (T3-F2 scope)
- Oda transitions RoomLoader paralel impl dokunma (LIVE)
- Cliff F2-F5 subsystem ile çakışma yok (farklı dosyalar)

## Verify
- UnityMCP compile 0 err / 0 warn
- RoomLayoutSerializer.cs LIVE, static method'lar var
- RoomManifestSO schemaVersion field LIVE
- StreamingAssets/live/ directory created
- Manual test: Editor sahnede RoomLayoutSerializer.Serialize çağır → JSON output

## Code rotation (HARD `feedback_code_writer_rotation`)
- **Yazan:** Codex xhigh (algoritma yoğun JSON serialize/GUID lookup)
- **Reviewer:** Sonnet F1 PASS sonrası

## Output
- `STAGING/T3_F1_DONE.md` — değişen dosyalar + verify checklist + sample JSON output

## Süre
~45-60 dk Codex bg. Lock cleanup memory `feedback_codex_stale_lock_after_taskstop`.

BLOCKED: (a) Newtonsoft.Json package değilse JsonUtility fallback (basit schema OK). (b) RoomManifestSO mevcut field çakışması → diff before extend. (c) AssetDatabase.GUIDFromAssetPath null edge case → fallback path-based.
