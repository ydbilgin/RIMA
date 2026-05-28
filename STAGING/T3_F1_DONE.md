# T3-F1 Done Report

## Changed Files
- `Assets/Editor/RoomPainter/LiveTool/RoomLayoutSerializer.cs`
- `Assets/Scripts/Map/Data/RoomManifestSO.cs`
- `Packages/manifest.json`
- `Assets/StreamingAssets/live/.gitkeep`
- Unity generated metadata for the new script and StreamingAssets folders.
- Unity package refresh also touched `Packages/packages-lock.json`.

## Implementation
- Locked serializer schema version to `1.0`.
- Added editor-only `RoomLayoutSerializer` static class.
- `Serialize(Scene scene, out string json)` collects scene Tilemap cells and prefab instance roots, writes JSON to `Application.streamingAssetsPath/live/room_current.json`, and returns the JSON string.
- `Deserialize(string json, Scene targetScene)` validates schema version, restores a generated Grid/Tilemap root, and instantiates prefab references by GUID/path fallback.
- GUID references use `AssetDatabase.AssetPathToGUID` and `AssetDatabase.GUIDToAssetPath`, with path fallback for missing GUID resolution.
- `RoomManifestSO` now has serialized `schemaVersion = "1.0"`, `SchemaVersion`, `IsCompatibleSchema`, and a Phase 2 migration stub.
- Added `com.unity.nuget.newtonsoft-json` to `Packages/manifest.json`.
- Created `Assets/StreamingAssets/live/`.

## Verify Checklist
- PASS: `RoomLayoutSerializer.cs` exists.
- PASS: `Serialize(Scene scene, out string json)` exists.
- PASS: `Deserialize(string json, Scene targetScene)` exists.
- PASS: `RoomManifestSO` has `schemaVersion` field.
- PASS: `RoomManifestSO.IsCompatibleSchema(string incomingVersion)` exists.
- PASS: `Assets/StreamingAssets/live/` exists.
- PASS: `Packages/manifest.json` contains `com.unity.nuget.newtonsoft-json`.
- BLOCKED: Unity compile is not 0 err / 0 warn because the project currently has out-of-scope compile errors.
- BLOCKED: Manual editor serialize call could not be executed because project assemblies do not compile.

## Compile Blockers Observed
These are outside the allowed T3-F1 file scope:

- `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs(5,24)`: `RIMA.MapDesigner.VisualEditor` namespace missing.
- `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs(713,27)`: duplicate local `prevContent`.
- `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs(728,31)`: duplicate local `prevContent`.
- `Assets/Tests/PlayMode/Phase1Demo/Phase1TestHarness.cs(128,24)`: `Object` ambiguous between `UnityEngine.Object` and `object`.
- `Assets/Tests/PlayMode/Phase1Demo/T2_GateFlowTest.cs(195,27)`: `MapFragment.isPickedUp` missing.

No compile diagnostics referenced `RoomLayoutSerializer.cs` or `RoomManifestSO.cs` in the final targeted log scan.

## Sample JSON Output
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
    { "cell": [0, 0, 0], "tile_guid": "00000000000000000000000000000000" }
  ],
  "cliff_cells": [],
  "prop_instances": [
    { "prefab_guid": "11111111111111111111111111111111", "position": [2.0, 1.0, 0.0], "rotation": 0.0 }
  ],
  "collider_overrides": [
    { "instance_id": "GlobalObjectId_V1-0-22222222222222222222222222222222-0-0", "size": [1.0, 1.0], "offset": [0.0, 0.0], "shape": "Box" }
  ]
}
```
