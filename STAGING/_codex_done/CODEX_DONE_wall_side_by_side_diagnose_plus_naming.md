# Yan Yana Wall + Isim Diagnose - Codex Report

## Sorun 1: Yan Yana Wall Paint

### Tani Verileri

- Scene: `Assets/Scenes/Demo/PathC_BaseTest.unity`
- Scene dirty before test: `True`
- Scene dirty after cleanup: `True` (pre-existing dirty state; test objects were deleted)
- Grid: `Grid`, `cellLayout=IsometricZAsY`, `cellSize=(0.94, 0.94, 1.00)`
- Target tilemap: `Grid/Floor_Tilemap`, `cellSize=(0.94, 0.94, 1.00)`
- Adjacent cell center delta for `(5,5)->(6,5)`: `(0.47, 0.24, 0.00)`
- Pilot A PNG import: `128x128`, `PPU=64`, `pivot=(0.50, 0.03)`, sprite bounds `(2.00, 2.00, 0.20)`
- Prefab source BoxCollider2D on `pilot_a_wall_face_EW`: `size=(2.00, 0.80)`, `offset=(0.00, 0.3375)`
- Painter-created instance scale: `(0.50, 0.50, 0.50)`
- Painter-created instance sprite world bounds: `(1.00, 1.00, 0.10)`
- Painter-created instance BoxCollider2D local: `size=(2.00, 1.60)`, `offset=(0.00, 0.74)`
- Painter-created instance BoxCollider2D world bounds: `(1.00, 0.80, 0.00)`

### Mevcut Pilot A Wall Instance Positions

Expected task note said 8 Pilot A walls, but the loaded scene currently has 32 Pilot A wall SpriteRenderer objects:

- Counts by source prefab: `pilot_a_wall_arch_opening=1`, `pilot_a_wall_corner_outer=11`, `pilot_a_wall_face_EW=20`
- Duplicate occupied cells already present:
  - `(11, 3, 0)` count 2: `pilot_a_wall_corner_outer`, `pilot_a_wall_corner_outer`
  - `(11, 5, 0)` count 2: `pilot_a_wall_corner_outer`, `pilot_a_wall_face_EW`
  - `(12, 3, 0)` count 2: `pilot_a_wall_corner_outer`, `pilot_a_wall_face_EW`
  - `(13, 9, 0)` count 4: four `pilot_a_wall_face_EW`

Current instance list:

| Name | Source | Cell | Position | Parent |
|---|---|---:|---|---|
| pilot_a_wall_corner_outer | pilot_a_wall_corner_outer | (10,5,0) | (-65.22,-42.66,0.00) | Grid |
| pilot_a_wall_corner_outer | pilot_a_wall_corner_outer | (11,5,0) | (-64.75,-42.42,0.00) | Grid |
| pilot_a_wall_corner_outer | pilot_a_wall_corner_outer | (12,6,0) | (-64.75,-41.95,0.00) | Grid |
| pilot_a_wall_corner_outer | pilot_a_wall_corner_outer | (12,5,0) | (-64.28,-42.19,0.00) | Grid |
| pilot_a_wall_corner_outer | pilot_a_wall_corner_outer | (13,6,0) | (-64.28,-41.72,0.00) | Grid |
| pilot_a_wall_corner_outer | pilot_a_wall_corner_outer | (11,3,0) | (-63.81,-42.89,0.00) | Grid |
| pilot_a_wall_corner_outer | pilot_a_wall_corner_outer | (11,3,0) | (-63.81,-42.89,0.00) | Grid |
| pilot_a_wall_corner_outer | pilot_a_wall_corner_outer | (13,5,0) | (-63.81,-41.95,0.00) | Grid |
| pilot_a_wall_corner_outer | pilot_a_wall_corner_outer | (15,7,0) | (-63.81,-41.01,0.00) | Grid |
| pilot_a_wall_corner_outer | pilot_a_wall_corner_outer | (12,3,0) | (-63.34,-42.66,0.00) | Grid |
| pilot_a_wall_face_EW | pilot_a_wall_face_EW | (12,9,0) | (-66.16,-41.25,0.00) | Grid |
| pilot_a_wall_face_EW | pilot_a_wall_face_EW | (13,10,0) | (-66.16,-40.78,0.00) | Grid |
| pilot_a_wall_face_EW | pilot_a_wall_face_EW | (14,11,0) | (-66.16,-40.31,0.00) | Grid |
| pilot_a_wall_face_EW | pilot_a_wall_face_EW | (10,6,0) | (-65.69,-42.43,0.00) | Grid |
| pilot_a_wall_face_EW | pilot_a_wall_face_EW | (13,9,0) | (-65.69,-41.02,0.00) | Grid |
| pilot_a_wall_face_EW | pilot_a_wall_face_EW | (13,9,0) | (-65.69,-41.02,0.00) | Grid |
| pilot_a_wall_face_EW | pilot_a_wall_face_EW | (13,9,0) | (-65.69,-41.02,0.00) | Grid |
| pilot_a_wall_face_EW | pilot_a_wall_face_EW | (13,9,0) | (-65.69,-41.02,0.00) | Grid |
| pilot_a_wall_face_EW | pilot_a_wall_face_EW | (14,10,0) | (-65.69,-40.55,0.00) | Grid |
| pilot_a_wall_face_EW | pilot_a_wall_face_EW | (12,7,0) | (-65.22,-41.72,0.00) | Grid |
| pilot_a_wall_face_EW | pilot_a_wall_face_EW | (11,5,0) | (-64.75,-42.43,0.00) | Grid |
| pilot_a_wall_face_EW | pilot_a_wall_face_EW | (12,3,0) | (-63.34,-42.66,0.00) | Grid |
| pilot_a_wall_face_EW | pilot_a_wall_face_EW | (15,6,0) | (-63.34,-41.25,0.00) | Grid |
| pilot_a_wall_face_EW | pilot_a_wall_face_EW | (12,2,0) | (-62.87,-42.90,0.00) | Grid |
| pilot_a_wall_arch_opening | pilot_a_wall_arch_opening | (3,4,0) | (-68.04,-44.54,0.00) | Props_Root |
| pilot_a_wall_corner_outer | pilot_a_wall_corner_outer | (2,5,0) | (-68.98,-44.54,0.00) | Props_Root |
| pilot_a_wall_face_EW | pilot_a_wall_face_EW | (2,4,0) | (-68.51,-44.78,0.00) | Props_Root |
| pilot_a_wall_face_EW | pilot_a_wall_face_EW | (2,3,0) | (-68.04,-45.01,0.00) | Props_Root |
| pilot_a_wall_face_EW | pilot_a_wall_face_EW | (2,2,0) | (-67.57,-45.25,0.00) | Props_Root |
| pilot_a_wall_face_EW | pilot_a_wall_face_EW | (3,2,0) | (-67.10,-45.01,0.00) | Props_Root |
| pilot_a_wall_face_EW | pilot_a_wall_face_EW | (4,2,0) | (-66.63,-44.78,0.00) | Props_Root |
| pilot_a_wall_face_EW | pilot_a_wall_face_EW | (5,2,0) | (-66.16,-44.54,0.00) | Props_Root |

### Adjacent Paint Test `(5,5) -> (6,5)`

Executed editor-side against the actual private wall path `PaintWallWithConnections`, using `pilot_a_wall_face_EW`, target tilemap `Floor_Tilemap`, parent `Props_Root`, `autoConnectWalls=true`, `prefabScaleMultiplier=0.5`, `randomizeWallCracks=false`. Pre-existing walls at `(5,5)` and `(6,5)` were both zero. Cleanup deleted the 2 test-created objects.

Result:

- Clicked `(5,5)`, cell center `(-67.57,-43.86,0.00)`
  - Created object position: `(-67.57,-44.31,0.00)`
  - `WorldToCell(transform.position)` became `(4,4,0)`, not `(5,5,0)`
- Clicked `(6,5)`, cell center `(-67.10,-43.63,0.00)`
  - Created object position: `(-67.10,-44.07,0.00)`
  - `WorldToCell(transform.position)` became `(5,4,0)`, not `(6,5,0)`
- The two created BoxCollider2D world bounds intersected:
  - `intersects=True`
  - `overlapX=0.5300`
  - `overlapY=0.5650`

Important: the test did create both objects. There was no code-level refusal in this path. The bad behavior is that logical cell tracking is wrong after auto base alignment, and adjacent wall colliders overlap heavily.

### PaintPrefab Cell Check

- `PaintPrefab` has a same-position occupancy check at lines `2435-2444`:
  - It checks all children under the target parent with SpriteRenderer.
  - It blocks only when `Vector3.Distance(child.position, targetPos) < 0.1f`.
  - This is exact-position based, not logical-cell based.
- Wall painting normally does not use `PaintPrefab` when `autoConnectWalls=true`:
  - Lines `2320-2322`: wall category + auto connect calls `PaintWallWithConnections`.
- `PaintWallWithConnections` uses `FindWallAtCell` at lines `3490-3508`:
  - It maps existing wall `transform.position` back with `targetTilemap.WorldToCell(child.position)`.
  - Because `GetPlacementOffset` shifts the transform down for base alignment, the transform no longer maps to the originally clicked cell.
- `snapToGrid` picks a cell center correctly, but placement then adds `GetPlacementOffset`. The object root is no longer a stable logical cell anchor.

### Root Cause

Specific root cause:

1. The wall object root transform is being used for two incompatible jobs: visual/base-aligned sprite placement and logical grid-cell identity.
2. `GetPlacementOffset` moves the root transform downward after snapping. In the measured test, clicked cells `(5,5)` and `(6,5)` became stored transform cells `(4,4)` and `(5,4)`.
3. `FindWallAtCell` and wall auto-connect then query the shifted transform position, so occupancy/neighbor checks are unreliable. This explains why stacking can happen in cells that should be occupied.
4. WallBlock collider bounds also overlap adjacent isometric cells. The tested adjacent cells have center delta `(0.47,0.24)`, but each wall collider is `(1.00,0.80)` in world bounds, producing overlap `(0.53,0.565)`. Static wall colliders will not automatically push each other without Rigidbody2D, but any placement/validation/physics query that treats collider overlap as blocking will reject or misread adjacent placement.

So the primary bug is logical-cell identity drift from auto base alignment. The secondary collision bug is an oversized axis-aligned WallBlock footprint for adjacent isometric wall cells.

### Fix Onerisi

- A) Keep root at logical cell center, move only a child visual object.
  - Root position stays exactly `GetCellCenterWorld(cell)`.
  - SpriteRenderer child gets the base-align Y offset.
  - `FindWallAtCell(root.position)` becomes reliable.
  - Trade-off: Pilot A prefabs need a small hierarchy change or painter must wrap placed sprites in an anchor root.

- B) Store explicit painted cell metadata and stop deriving cell from transform.
  - Add a lightweight component or serialized map data field like `PaintedCell=(x,y,z)` on placed walls.
  - `FindWallAtCell` reads this value first, falling back to `WorldToCell` only for legacy objects.
  - Trade-off: needs migration/repair pass for existing scene walls, but minimal visual risk.

- C) Narrow or decouple WallBlock collider from painter occupancy.
  - Do not use collider intersection to decide wall placement.
  - If collider overlap is used anywhere, ignore same wall layer during painter placement checks.
  - For gameplay collision, use a separate intended footprint, not the full visible sprite width.
  - Trade-off: requires checking player collision feel separately; axis-aligned boxes are a poor fit for iso wall chains.

- D) Complete the wall auto-connect/Wang family after A or B.
  - Pilot A currently has `face_EW`, `corner_outer`, and `arch_opening` prefabs, but no `face_NS` prefab in `Assets/Prefabs/Walls/pilot_a` even though a PNG exists.
  - Auto-connect needs canonical family slots for both straight directions and corners.
  - Trade-off: best long-term behavior, but should happen after logical cell anchoring is fixed.

Recommended first fix: A if prefab structure changes are acceptable; B if a surgical code-only fix is preferred.

## Sorun 2: Isimlendirme

### Mevcut Naming Envanteri

PNG files in `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test`:

- `pilot_a_frame_0_face_NS.png`
- `pilot_a_frame_1_face_EW.png`
- `pilot_a_frame_2_corner_outer.png`
- `pilot_a_frame_3_arch_opening.png`

Prefabs in `Assets/Prefabs/Walls/pilot_a`:

- `pilot_a_wall_arch_opening.prefab`
- `pilot_a_wall_corner_outer.prefab`
- `pilot_a_wall_face_EW.prefab`

Palette display via `CleanName` lines `655-670`:

- `pilot_a_wall_face_EW` -> `pilot_a_wall`
- `pilot_a_wall_corner_outer` -> `pilot_a_wall`
- `pilot_a_wall_arch_opening` -> `pilot_a_wall`
- PNG-style names like `pilot_a_frame_1_face_EW` -> `pilot_a_frame`

Hierarchy instance naming:

- Scene instances keep raw prefab names like `pilot_a_wall_face_EW` and `pilot_a_wall_corner_outer`.
- Current scene has many identical hierarchy names with no user-facing counter/label distinction.
- Current scene source counts: `pilot_a_wall_arch_opening=1`, `pilot_a_wall_corner_outer=11`, `pilot_a_wall_face_EW=20`.

### Tutarsizliklar

- Palette display collapses all Pilot A wall prefabs to the same visible name: `pilot_a_wall`.
- PNG names use `pilot_a_frame_N_*`, but prefabs use `pilot_a_wall_*`; this is not one canonical scheme.
- `pilot_a_` is production-batch metadata, not useful to the level designer while painting.
- `face_NS` exists as a PNG but not as a Pilot A prefab in `Assets/Prefabs/Walls/pilot_a`.
- Hierarchy instances are not user-friendly and become unreadable when many copies share the same name.

### User-Friendly Oneri

- Keep asset filenames canonical and technical, but use display labels in the painter UI:
  - `Wall Face EW`
  - `Wall Face NS`
  - `Wall Corner Outer`
  - `Wall Arch Opening`
- Change `CleanName` behavior for wall prefabs so it strips only batch prefixes like `pilot_a_`, not the meaningful suffix.
  - Current output `pilot_a_wall` is not enough.
  - Desired output for `pilot_a_wall_face_EW`: `wall_face_EW` or display label `Wall Face EW`.
- For hierarchy instances, use deterministic instance labels at placement time:
  - `Wall_Face_EW_001`
  - `Wall_Corner_Outer_001`
  - Or include logical cell: `Wall_Face_EW_cell_005_005`.
- Do not rename files until the logical-cell wall placement bug is fixed. Display-label changes are safer than asset renames because they do not break prefab GUID references.

## Cleanup / Constraints

- No code changes applied.
- No asset or prefab renames applied.
- No commit made.
- Temporary test walls were deleted: `cleanupDeletedNewObjects=2`.
- Scene dirty state remained `True` because it was already dirty before the diagnostic began.
