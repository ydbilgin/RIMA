# Phase A v15c 8-Layer Refactor DONE

Status: DONE
Date: 2026-05-18
Profile: laurethgame

## Files Modified

Count: 15 tracked/new task files in the requested scope.

- `Assets/Scripts/Rima/MapDesigner/SO/BlueprintZoneTypeSO.cs`
- `Assets/Scripts/Rima/MapDesigner/SO/BlueprintProfileSO.cs`
- `Assets/Editor/MapDesigner/Blueprint/AutoPopulator.cs`
- `Assets/Editor/MapDesigner/PropPlacementService.cs`
- `Assets/Editor/MapDesigner/Blueprint/BlueprintPainterWindow.cs`
- `Assets/Tests/EditMode/MapDesigner/Blueprint/AutoPopulator8LayerTests.cs`
- `Assets/Tests/EditMode/MapDesigner/Blueprint/BlueprintPainterWindowTests.cs`
- `Assets/Data/Blueprint/ZoneTypes/zone_feature.asset`
- `Assets/Data/Blueprint/ZoneTypes/zone_grass.asset`
- `Assets/Data/Blueprint/ZoneTypes/zone_path.asset`
- `Assets/Data/Blueprint/ZoneTypes/zone_stone.asset`
- `Assets/Data/Blueprint/ZoneTypes/zone_wall.asset`
- `Assets/Data/Blueprint/ZoneTypes/zone_water.asset`
- `Assets/Scenes/Demo/RoomPipelineTest.unity`
- `STAGING/v15c_8layer_metrics.txt`

## Verification

- EditMode tests: PASS, 392 passed / 0 failed / 0 skipped. One existing prefab health test returned inconclusive because `_IsoGame` scene was not found.
- Console after final clear/check: 0 errors.
- Blueprint Painter window opened through Unity menu.
- Layer Visibility supports L1-L8; new tests cover L1 and L8 dimming.
- v15c scene root created: `Pro_Redesign_v15c_8LayerPainted_CombatRoom`
- v15b root deactivated: `Pro_Redesign_v15b_FullAdjacency_CombatRoom`
- Screenshot: `Assets/Screenshots/PlayableRoom_combat_v15c_8layer.png`

## Placement Metrics

- Painted cells: 375
- Zone placements: 841
- Adjacency placements: 1
- Total v15c children: 842
- L1: 0
- L2: 375
- L3: 149
- L4: 101
- L5: 150
- L6: 55
- L7: 11
- L8: 0

## Coverage

- L1 coverage: 0.000 because all Layer 1 macro sprite arrays are intentionally empty and logged as imagegen gaps.
- L2 coverage: 1.000

## Asset Gaps

Layer 1 macro fill missing:
- path
- grass
- stone
- wall
- water
- feature

Layer 8 atmospheric overlay missing:
- path
- grass
- stone
- wall
- water
- feature

## Test Delta

- Added 10 AutoPopulator 8-layer tests.
- Added 2 Blueprint Painter layer visibility tests.
