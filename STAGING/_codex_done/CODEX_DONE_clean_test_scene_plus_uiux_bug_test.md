# Clean Test Scene + UIUX Bug Test - Codex Report

## Bolum 1: Eski Wall Temizligi
- Deleted: 57 wall instance actual (45 Grid + 12 Props_Root). Task expected 55, but scene contained 57 matching wall_* under the allowed roots.
- Statue/mounting/decor preserved: YES. Delete filter was name starts with wall_* only under Grid and Props_Root.
- Scene saved: YES

## Bolum 2: Pilot A Place
- 3 prefab created:
  - Assets/Prefabs/Walls/pilot_a/pilot_a_wall_face_EW.prefab
  - Assets/Prefabs/Walls/pilot_a/pilot_a_wall_corner_outer.prefab
  - Assets/Prefabs/Walls/pilot_a/pilot_a_wall_arch_opening.prefab
- Painter place executed through RimaUnifiedPainterWindow private PaintWallWithConnections path with Auto-Connect enabled.
- Placed: 4 south face cells (3,3)-(6,3), 2 east face cells (3,4)-(3,5), 1 corner at (3,6), 1 arch at (4,5). Total Pilot A instances: 8.
- Painter auto-connect: FAIL for Pilot A. Normal wall palette scan after menu open had 4 legacy wall prefabs and contains Pilot A=false. Code scans only Assets/Prefabs/Props/ShatteredKeep_PixelLab with filename prefix wall_. FindWallAtCell also only detects instance/sprite names starting wall_, so pilot_a_wall_* is not connectable even if selected by reflection.
- Scene saved: YES

## Bolum 3.1: Console Logs
- Painter open-close 3x: 0 errors, 0 warnings
- Selection switch test: 0 errors, 0 warnings
- Edit Collider in Scene API path entered and exited: 0 errors, 0 warnings
- Captured logs: none

## Bolum 3.2: "Wall Buyuk Uste" Bug
- Replication: Selected placed Pilot A wall, opened painter, selected wall, entered collider edit mode, deselected, selected statue/ground tile. No console error reproduced.
- transform values on selected wall:
  - path: Props_Root/pilot_a_wall_face_EW
  - localScale=(0.50, 0.50, 0.50), lossyScale=(0.50, 0.50, 0.50)
  - position=(-67.57, -45.25, 0.00)
  - BoxCollider2D.size=(2.00, 1.60), offset=(0.00, 0.74)
  - sprite boundsSize=(2.00, 2.00, 0.20), pivot=(64,4), rect=128x128
- Root cause: not fully reproduced on Props_Root placement. Two likely code causes remain:
  - Selected Instance editor edits only localScale.x and writes uniform x/y scale. For instances parented under Grid, Grid has scale=(1,0.5,1), so this can destroy painter's compensated scale and make the wall appear too tall/shifted after edit.
  - Preview/selection placement uses auto bottom alignment from visible sprite bounds plus pivot. Pilot A pivot is near bottom, and transform is intentionally below the tile center to seat the wall base; this can read as "comes above/on top" if the editor UI shows transform center rather than visual base.
- Fix suggestion: in Selected Instance editor, edit/display intended world scale or preserve compensated local scale per parent lossyScale; show base anchor/cell coordinate separately from transform center.

## Bolum 3.3: "Wall Gorunmez Alti" Bug
- Replication: after Pilot A placement, Game view screenshot shows walls visible above ground.
- Wall sortingLayer/order: Walls, IsoSorter-overwritten orders 4454-4525 in this room.
- Ground sortingLayer/order: Ground, -100.
- CollisionResolver expected for WallBlock: layerName=Walls, sortingOrder=20. Actual order is then overwritten by IsoSorter because ApplySorting attaches IsoSorter to every wall SpriteRenderer.
- Root cause: not reproduced in this scene. Sorting layer is correct. Remaining risk is that ApplySorting sets 20 but IsoSorter immediately replaces it with Y sort order; if sorting layer order is broken in TagManager or a wall stays on Default, it can render under ground.
- Fix suggestion: keep wall sorting on Walls layer, but make wall IsoSorter behavior explicit: either disable IsoSorter for static walls or set a wall baseOrder that preserves intended wall banding.

## Bolum 3.4: Genel Bug Listesi
- Bug 1: Pilot A prefabs are not in the real Painter wall palette. Replication: open RIMA/Tools/Unified Painter, category Duvar; scanner only loads Assets/Prefabs/Props/ShatteredKeep_PixelLab/wall_*. Root cause: hardcoded ScanPrefabsInFolder path/prefix. Fix: include Assets/Prefabs/Walls/pilot_a or support configured wall folders/prefixes.
- Bug 2: Auto-Connect cannot detect pilot_a_wall_* instances. Replication: place Pilot A wall through PaintWallWithConnections; UpdateWallConnectionsAt/FindWallAtCell returns null because names/sprites do not start wall_. Fix: detect category/group/prefab metadata or names containing _wall_, not only prefix wall_.
- Bug 3: Selected Instance scale editing can break compensated scale for Grid-parented walls. Replication target: select an instance under Grid where parent scale is (1,0.5,1), change Scale in Panel 7. Root cause: editor writes uniform localScale from localScale.x. Fix: world-scale editor with parent compensation.
- Bug 4: Sorting order display is misleading for walls. Replication: resolver returns 20, actual SpriteRenderer order becomes 4454+ after IsoSorter LateUpdate. Root cause: ApplySorting adds IsoSorter and later it overrides resolved sortingOrder. Fix: show effective runtime order or do not attach IsoSorter to static walls.

## Bolum 4: Screenshot
- STAGING/clean_test_scene_room.png yazildi
- Unity also created Assets/Screenshots/STAGING_clean_test_scene_room.png from Main Camera; copied to required STAGING path.

## Acik Sorular
- Should Pilot A wall prefab names be renamed to wall_* or should Painter support pilot_a_wall_* as a valid wall naming convention?
- Should static walls use IsoSorter, or should walls keep CollisionResolver sortingOrder=20 with only sortingLayer separation?
