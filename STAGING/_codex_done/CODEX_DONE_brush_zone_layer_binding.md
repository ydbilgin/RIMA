Status: SUCCESS

Files:
- Added `Assets/Scripts/MapDesigner/Room/Data/ZoneToLayerMappingSO.cs`
- Added `Assets/Data/Blueprint/ZoneLayerMap_Default.asset`
- Edited `Assets/Editor/MapDesigner/Inspectors/RoomTemplateSOInspector.cs`

Compile: PASS

Tests:
- Unity EditMode MCP job `140fdfb1b0f04b489f171edc3bf05b41`
- Completed 419/419, failed 0
- Result summary: 418 passed, 0 failed, 0 skipped, 1 inconclusive (`_IsoGame scene bulunamadi.`)

Asset verification:
- `ZoneLayerMap_Default` loads in Unity
- 11 entries total
- 8 sprites bound
- Missing expected in-flight sprites: path, grass, soil

Test plan:
- User opens `Spawn_01.asset`
- Inspector shows `Brush Zone Add (auto-bind from ZoneToLayerMappingSO)`
- Clicking `Stone Floor`, `Path`, or `Grass` adds a `backgroundLayers` entry when the entry has an assigned sprite
- Scene re-spawn renders the added layer

Notes:
- `EditorUtility.DisplayDialog` was not added.
- Batchmode shell test was blocked by an already-open Unity instance, so the running Unity editor session executed the compile/test gate.
