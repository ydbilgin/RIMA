# Wall Alignment + Layer Cleanup Atomic - Codex Report

## Bolum 1: Wall .meta Alignment Fix
| File | spriteAlignment | spritePivot | Verify (pixel) |
|---|---:|---|---|
| straight_horizontal | 0 -> 9 | (0.5, 0.03125) | (64, 4) PASS |
| corner_L_NE | 0 -> 9 | (0.5, 0.03125) | (64, 4) PASS |
| arch_opening | 0 -> 9 | (0.5, 0.03125) | (64, 4) PASS |
| partition_low_stub | 7 -> 9 | (0.5, 0.041666668) | (48, 4) PASS |
| cyan_rift_integrated | 0 -> 9 | (0.5, 0.03125) | (64, 4) PASS |

## Bolum 2: PathC_BaseTest Floor_Tilemap
- sortingLayerID: 0 (Default) -> 2024493761 (Ground)
- m_SortingLayer: 0 -> 1
- Scene saved: YES

## Bolum 3: Orphan Layers Deleted
- Detail (351335743): DELETED
- Accent (1570199623): DELETED
- Props (399489520): DELETED
- Wall singular (2024493762): DELETED
- Patch (1365605006): DELETED after Phase1 archive
- Scatter (27625511): DELETED after Phase1 archive

## Bolum 4: Script Drift Fix
- IsometricSortSetup.cs: canonical `"Walls"` sorting layer; no standalone `"Wall"` string remains.
- RimaSortingLayerValidator.cs: canonical 5 layer set only: Default, Ground, Walls, Entities, VFX.
- Removed Patch/Scatter/Detail/Accent/Props creation.

## Bolum 5: Phase1 Sahne Archive
- `Assets/Scenes/Phase1_ProceduralMap_Test.unity` -> `Assets/_ARCHIVE/Scenes/Phase1_ProceduralMap_Test.unity`
- `.meta` moved with GUID preserved: `0b540b97e2fde464d95a830e75f24ebc`
- Build Settings'te yoktu.
- Note: requested `Assets/_archive` resolves to existing `Assets/_ARCHIVE` on this Windows workspace.

## Bolum 6: Final Verify
- dotnet build targeted:
  - `RIMA.Runtime.csproj`: 0 warnings, 0 errors
  - `Assembly-CSharp.csproj`: 0 warnings, 0 errors
  - `Assembly-CSharp-Editor.csproj`: 0 errors, 18 existing warnings in unrelated editor files
- Unity Console: clean, 0 entries
- Sprite pivot: 5/5 PASS
- Sorting layers final: [Default, Ground, Walls, Entities, VFX]
- Floor_Tilemap on Ground: PASS
- Scripts canonical: PASS

## Git Diff Summary
- 5 wall `.png.meta` files: custom alignment enabled; pivots verified
- `ProjectSettings/TagManager.asset`: removed Patch, Scatter, Detail, Accent, Props, Wall
- `Assets/Editor/DevTools/IsometricSortSetup.cs`: Wall -> Walls canonical cleanup
- `Assets/Editor/RimaSortingLayerValidator.cs`: canonical 5 layer validator
- `Assets/Scenes/Phase1_ProceduralMap_Test.unity` + `.meta` moved to `Assets/_ARCHIVE/Scenes/`
- `Assets/_ARCHIVE/Scenes.meta` created by Unity
- `Assets/Scenes/Demo/PathC_BaseTest.unity`: Floor_Tilemap sorting layer set to Ground

## Acik Sorular
- None
