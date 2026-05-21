# Unity Diagnose Phase A - Codex Report

## Bolum 1: Console
### Errors
| # | Type | File:Line | Message | Stack (truncated) |
|---|---|---|---|---|
| - | - | - | No Unity console errors returned by `read_console` for active scene `Assets/Scenes/Demo/PathC_BaseTest.unity`. | - |

### Warnings (top 10)
- No Unity console warnings returned by `read_console`.

## Bolum 2: Scene Verify
### Active Scene
- path: `Assets/Scenes/Demo/PathC_BaseTest.unity`
- loaded: yes
- dirty: no
- rootCount: 7

### Grid
- exists: yes (`Grid`, instanceID `56300`)
- transform.position: `(-70.9, -47.2, 0)`
- transform.localScale: `(1, 0.5, 1)`
- transform.rotation.eulerAngles: `(0, 0, 0)`
- Grid.cellSize: `(0.94, 0.94, 1)`
- Grid.cellLayout: `3` (`Isometric`)
- Grid.cellSwizzle: `0` (`XYZ`)
- childCount: 52

### Tilemap Children
| Tilemap | Transform localPosition | tileAnchor | Renderer.mode | Renderer.sortOrder | sortingLayerName | sortingOrder | IsometricZAsY |
|---|---:|---:|---:|---:|---|---:|---|
| `Grid/Floor_Tilemap` | `(3.334, 1.507, 0)` | `(0.5, 0.5, 0)` | `0` | `2` | `Default` | `-100` | active: yes, ProjectSettings `m_TransparencySortMode: 3`, axis `(0, 1, 0)`; Main Camera `transparencySortMode: 3` |

### Camera
- exists: yes (`Main Camera`, instanceID `56134`)
- transform.position: `(-70.9, -47.2, -10)`
- transform.rotation.eulerAngles: `(0, 0, 0)`; tilt: none
- Camera.orthographic: yes
- Camera.orthographicSize: `5.5`
- Camera.transparencySortMode: `3`

### Props_Root
- exists: yes (`Props_Root`, instanceID `56180`)
- transform identity: yes
- transform.position: `(0, 0, 0)`
- transform.localScale: `(1, 1, 1)`
- transform.rotation.eulerAngles: `(0, 0, 0)`
- childCount: 0

### Sub-groups / Placement
- Named root groups `Walls`, `Mobs`, `WallMountings`: not found in scene roots.
- `Props_Root` children: none.
- `Grid` children include direct instances named `wall_*`, `mounting_*`, and `statue_*`.
- Mobs group / mob instances: not observed in the retrieved hierarchy.

## Bolum 3: Wall PNG Metadata
Importer enum notes: `spriteMode 2 = Multiple`, `filterMode 0 = Point`, `filterMode 1 = Bilinear`, `wrapU/V/W 1 = Clamp`.

| File | W x H | Import Mode | Pivot | PPU | Filter | alphaIsTransparency | Wrap | Foot row | Classification |
|---|---:|---|---:|---:|---|---:|---|---|---|
| `act1_wall_straight_horizontal_v01.png` | `128 x 128` | `Multiple (2)` | `(0.5, 0.5)` | `100` | `Bilinear (1)` | `1` | `Clamp (1/1/1)` | bottom 4 rows empty; trailing empty rows: 39; alpha bbox `x=3..124 y=36..88` | L2b face_EW candidate; re-import needed if pivot `(0.5, 0.0)` is intended because bottom padding would float |
| `act1_wall_corner_L_NE_v01.png` | `128 x 128` | `Multiple (2)` | `(0.5, 0.5)` | `100` | `Bilinear (1)` | `1` | `Clamp (1/1/1)` | bottom 4 rows empty; trailing empty rows: 15; alpha bbox `x=15..112 y=15..112` | L2b corner candidate; re-import needed if pivot `(0.5, 0.0)` is intended because bottom padding would float |
| `act1_wall_arch_opening_v01.png` | `128 x 128` | `Multiple (2)` | `(0.5, 0.5)` | `100` | `Bilinear (1)` | `1` | `Clamp (1/1/1)` | bottom 4 rows empty; trailing empty rows: 15; alpha bbox `x=15..112 y=13..112` | L2b arch candidate; re-import needed if pivot `(0.5, 0.0)` is intended because bottom padding would float |
| `act1_wall_partition_low_stub_v01.png` | `96 x 96` | `Multiple (2)` | `(0.5, 0.5)` | `64` | `Point (0)` | `1` | `Clamp (1/1/1)` | bottom 4 rows empty; trailing empty rows: 12; alpha bbox `x=17..76 y=14..83` | L2b short candidate, not L2a base by height heuristic: content height ratio `70/96 = 0.729`, over 30% |
| `act1_wall_cyan_rift_integrated_v01.png` | `128 x 128` | `Multiple (2)` | `(0.5, 0.5)` | `100` | `Bilinear (1)` | `1` | `Clamp (1/1/1)` | bottom 4 rows empty; trailing empty rows: 18; alpha bbox `x=15..112 y=17..109` | Variant candidate: integrated rift/damaged wall variant, not a separate archetype from filename heuristic alone |

## Bolum 4: Fix Applied (varsa)
- No fix applied.
- Reason: Bolum 1 returned zero `compile_error` and zero `missing_reference` / missing script type console errors.
- `dotnet build`: not run because the fix branch was not triggered by the task rules.
- Git diff from this task: report files only.

## Acik Sorular / Drift Tespiti
- Grid scene transform scale is `(1, 0.5, 1)` and is authoritative for current scene state.
- Tilemap renderer sorting has both `TilemapRenderer.sortOrder: 2` and renderer `sortingOrder: -100`; both are reported because Unity exposes both fields.
- All five wall PNGs have bottom padding in alpha rows. Pivot `(0.5, 0.0)` would need trimming/re-import or pivot compensation.
