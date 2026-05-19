# v15d Composition Lock Done

Status: DONE
Date: 2026-05-18
Executor: Codex laurethgame

## Files modified
- Assets/Scripts/Rima/MapDesigner/SO/BlueprintZoneTypeSO.cs
- Assets/Editor/MapDesigner/Blueprint/AutoPopulator.cs
- Assets/Editor/MapDesigner/Blueprint/RimaV15cSceneComposer.cs
- Assets/Tests/EditMode/MapDesigner/Blueprint/AutoPopulatorCompositionBudgetTests.cs
- Assets/Data/Blueprint/ZoneTypes/zone_feature.asset
- Assets/Data/Blueprint/ZoneTypes/zone_grass.asset
- Assets/Data/Blueprint/ZoneTypes/zone_path.asset
- Assets/Data/Blueprint/ZoneTypes/zone_stone.asset
- Assets/Data/Blueprint/ZoneTypes/zone_wall.asset
- Assets/Data/Blueprint/ZoneTypes/zone_water.asset
- Assets/Scenes/Demo/RoomPipelineTest.unity
- Assets/Screenshots/PlayableRoom_combat_v15d_composition_LIVE.png
- Assets/Screenshots/PlayableRoom_combat_v15d_composition_LIVE.png.meta
- STAGING/CODEX_DONE_v15d_COMPOSITION_LOCK_metrics.txt
- STAGING/CODEX_TASK_v15d_COMPOSITION_LOCK_DONE.md

## Test count delta
- Added 11 EditMode tests in AutoPopulatorCompositionBudgetTests.
- Final EditMode run: 403 passed, 0 failed.
- Unity MCP progress total was 404 because one existing PrefabHealthTests case reports inconclusive for missing _IsoGame scene; it was not a failure.

## Screenshot path
Assets/Screenshots/PlayableRoom_combat_v15d_composition_LIVE.png

## Metrics output verbatim
```text
[v15d Metrics] CombatRoom 36x22 = 375 cells
  Reserved cells: 160 (42.7%) -- path: 91 (24.3%), neg space: 69 (18.4%)
  Floor split: dominant 151 (40.3%), secondary 43 (11.5%), accent 21 (5.6%)
  Hero clusters: 3 / 3 cap -- sizes: [5, 4, 5] = 14 props
  Layer prop totals: L4=6, L5=4, L6=2, L7=2
  Budget check: OK neg space, OK floor weights, OK cluster cap, OK path ratio
v15d scene placement metrics
cells=375
zonePlaced=633
adjacencyPlaced=0
totalChildren=633
L1=375
L2=113
L3=64
L4=6
L5=4
L6=2
L7=2
L8=67
L1Coverage=1.000
L2Coverage=0.301
Layer1Gaps=
Layer8Gaps=
Screenshot=Assets/Screenshots/PlayableRoom_combat_v15d_composition_LIVE.png
```

## Deviations
- Unity batchmode shell test command was blocked because this project was already open in Unity. The same full EditMode run was executed through the live Unity editor via MCP after the shell command reported the project lock.
- ANTIGRAVITY.md was not present at the project root when requested rules were read.
- Unity SaveAssets persisted additional tracked asset diffs already present in the open editor session: Assets/Data/Blueprint/Profiles/profile_combat_room_default.asset, Assets/Data/Blueprint/AdjacencyRules/rule_grass_stone.asset, and Assets/Data/Blueprint/PropPools/pool_path.asset. They were left intact per no-revert rules.
- The metrics cell count reflects painted blueprint cells (375) on the 36x22 grid, not the full 792 grid slots.
