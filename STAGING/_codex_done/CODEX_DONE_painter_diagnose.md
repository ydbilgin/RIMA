# Unified Painter Paint Regression Diagnose (S95)

Scope: diagnosis only. No code or scene edits made.

## Files / commands checked

- Assets/Editor/RimaUnifiedPainterWindow.cs
- Assets/Editor/CollisionRulesSO.cs
- Assets/Editor/MapDesigner/Rules/DefaultCollisionRules.asset
- Assets/Editor/DevTools/IsometricSortSetup.cs
- Assets/Editor/RimaSortingLayerValidator.cs
- Assets/Scenes/Demo/PathC_BaseTest.unity
- Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity
- git log --oneline -20 -- Assets/Editor/
- targeted git show / git diff / rg checks

Note: task named `Assets/Editor/IsometricSortSetup.cs`, but the file exists at
`Assets/Editor/DevTools/IsometricSortSetup.cs`.

## Hypothesis list

1. Palette discovery broken: partially checked, not strongest.
   Evidence: `RimaUnifiedPainterWindow.cs:58-64` serializes wall scan folders with
   `Assets/Prefabs/Props/ShatteredKeep_PixelLab`, `Assets/Prefabs/Walls/pilot_a`,
   and `Assets/Prefabs/Walls`; `RimaUnifiedPainterWindow.cs:593-623` scans every
   prefab in those folders and accepts filenames containing the pattern `wall`.
   Shell checks found `pilot_a_wall_arch_opening.prefab`,
   `pilot_a_wall_corner_outer.prefab`, and `pilot_a_wall_face_EW.prefab` under
   `Assets/Prefabs/Walls/pilot_a`. So the pilot_a wall palette should not be empty.

2. Collision rule denying: low probability.
   Evidence: `CollisionRulesSO.cs:21-40` only resolves a matching rule; there is no
   placement deny path. `DefaultCollisionRules.asset` maps `wall_*` to mode 4
   (`WallBlock`), but `RimaUnifiedPainterWindow.cs:2307-2327` calls paint before
   collision setup, and `ConfigureCollider` only removes/adds colliders. No code path
   rejects paint based on collision.

3. GameObject parent missing / wrong parent: strongest.
   Evidence: `TryAutoAssignTargets` sets `targetParent = targetTilemap.transform.parent`
   at `RimaUnifiedPainterWindow.cs:752-755`. In both checked scenes the
   `Floor_Tilemap` parent is the `Grid`: `PathC_BaseTest.unity:134,153,4647`,
   `IsoShowcaseRoom_S95.unity:4597,4616,3309`. After that, `PeekTargetParent` returns
   `targetParent` immediately at `RimaUnifiedPainterWindow.cs:3203-3206`, so the newer
   grid-detection fallback at `RimaUnifiedPainterWindow.cs:3211-3220` never runs.
   Therefore `GetTargetParent` returns `Grid`, not `Props_Root`, `Walls_Root`, or the
   new S95 room root. In `PathC_BaseTest`, this makes new object/wall paint route under
   `Grid`; in `IsoShowcaseRoom_S95`, it routes under `Grid` even though existing
   authored objects live under root-level containers like `Walls_Root` and `Props_Root`.
   This is also inconsistent with the S95 cleanup intent shown by the current diff:
   old code converted grid/tilemap parents into `Props_Root`, but the auto-assigned
   serialized `targetParent` now bypasses that branch.

4. Sorting layer literal mismatch: low probability for paint failure.
   Evidence: hardcoded singular `Wall` in the requested editor files appears fixed.
   `IsometricSortSetup.cs:27-60` uses `Walls`; `RimaSortingLayerValidator.cs:8-55`
   canonicalizes `Ground / Walls / Entities / VFX`; `RimaUnifiedPainterWindow.cs:320-327`
   resolves wall sorting to `Walls`. Sorting can hide/misorder visuals if TagManager is
   wrong, but the checked code no longer uses the singular wall layer in the active path.

5. Selected Instance Editor scope bug: low probability.
   Evidence: `DrawSelectedInstanceEditor` starts at `RimaUnifiedPainterWindow.cs:1340`,
   but it is only an IMGUI panel over `Selection.activeGameObject`. It does not gate
   `OnSceneGUI`, `PerformAction`, `PaintTile`, `PaintPrefab`, or
   `PaintWallWithConnections`. It may show/edit the wrong root when target parent is
   wrong, but it does not directly block painting.

6. Multi-folder palette scanner regression: low probability.
   Evidence: current scanner is broader than the old single-folder `wall_` scan.
   `RimaUnifiedPainterWindow.cs:593-623` searches all configured folders and accepts
   any filename containing `wall`. This should include old `wall_00...` prefabs and new
   `pilot_a_wall_*` prefabs. Risk remains only for stale serialized window state if a
   previously-open Unity editor window preserved a custom `wallScanFolders` list, but
   `EnsureWallScanDefaults` at `RimaUnifiedPainterWindow.cs:625-640` appends missing
   defaults during scan.

7. GroupClassifier strict: medium-low probability.
   Evidence: `GroupClassifier.Classify` at `RimaUnifiedPainterWindow.cs:151-164`
   classifies category Wall or any name containing `wall` as `Walls`. It is not too strict
   for pilot_a assets. However `PaintWallWithConnections` at
   `RimaUnifiedPainterWindow.cs:3511-3546` does not use `GetOrCreateGroupParent`, so
   wall auto-connect paint bypasses the new canonical `Walls` subfolder entirely and
   places directly under the root returned by `GetTargetParent`.

## Most likely cause

Most likely cause: stale/wrong target parent selection caused by auto-assign plus the new
Props_Root/group refactor.

The critical chain is:

- `TryAutoAssignTargets` finds `Floor_Tilemap` and writes `targetParent` to its parent
  (`Grid`) at `RimaUnifiedPainterWindow.cs:747-755`.
- `GetTargetParent` delegates to `PeekTargetParent` at `RimaUnifiedPainterWindow.cs:3178-3184`.
- `PeekTargetParent` returns any non-null `targetParent` before checking whether it is a
  Grid/Tilemap at `RimaUnifiedPainterWindow.cs:3203-3206`.
- The intended S95 fallback that auto-uses or creates `Props_Root` only runs when
  `targetParent == null` and the tilemap parent is grid-like (`RimaUnifiedPainterWindow.cs:3211-3220`,
  `3178-3200`). The auto assignment makes that condition unreachable.
- In `PathC_BaseTest`, `Props_Root` exists at `PathC_BaseTest.unity:4215`, and there is
  also a `Walls` child under `Grid` at `PathC_BaseTest.unity:5896-5915`. In
  `IsoShowcaseRoom_S95`, `Walls_Root` exists at `IsoShowcaseRoom_S95.unity:4071-4107`,
  `Props_Root` exists at `IsoShowcaseRoom_S95.unity:9583-9608`, and `Grid` contains only
  `Floor_Tilemap` (`IsoShowcaseRoom_S95.unity:3309-3329`, `4597-4616`). The painter root
  and scene organization are therefore not aligned.

Why this matches the user report:

- A user can select a palette item and click in Scene View, but the result appears missing
  or not part of the intended scene canvas because it is parented under `Grid` or an
  unexpected root instead of the S95 object containers.
- Wall paint is especially exposed because `PaintWallWithConnections` does not call
  `GetOrCreateGroupParent`; it parents directly to `GetTargetParent`.
- The regression aligns with S95 cleanup commit `9fc0364e`, which explicitly touched
  `RimaUnifiedPainterWindow.cs` for `Props_Root sub-groups`.

## Reproduction step matching user experience

1. Open `Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity` or `Assets/Scenes/Demo/PathC_BaseTest.unity`.
2. Open `RIMA/Tools/Unified Painter`.
3. Let auto target assignment run, or click `Rescan & Sync Assets`.
4. Select a wall prefab such as `pilot_a_wall_face_EW` from the Wall palette.
5. Paint in Scene View.
6. Expected: wall appears under the intended wall/object scene container (`Walls_Root`,
   `Props_Root/Walls`, or canonical painter group).
7. Actual likely result: painter target parent is `Grid`; wall auto-connect paints under
   `Grid` directly, or appears outside the expected container/organization, making the
   user experience look like paint failed.

## Suggested fix direction (design only, no code written)

- Do not auto-store `targetParent = targetTilemap.transform.parent` when that parent is a
  Grid or Tilemap. Leave `targetParent` null so `PeekTargetParent` can infer the intended
  object root.
- Or change `PeekTargetParent` so an explicit/auto targetParent that is Grid/Tilemap is
  treated as invalid for object placement and falls through to `Props_Root`.
- Decide one canonical root policy for S95 scenes: either `Props_Root` with groups
  `Walls/Statues/WallMountings/Patches/Mobs/FloorProps`, or root-level `Walls_Root` /
  `Props_Root`. Current code and scenes are mixed.
- Route `PaintWallWithConnections` through `GetOrCreateGroupParent(parent, prefabName,
  PaletteCategory.Wall)` so walls land in the same canonical group path as normal prefab
  paint.
- Add a small editor diagnostic/status line: show `TargetParent source = explicit / auto /
  inferred` and warn if object target is a Grid/Tilemap.

## Risk

Yes, this regression can remain permanent after the refactor if not fixed. Because
`targetParent` is serialized in the editor window and auto-assigned on enable/rescan, the
bad `Grid` value can persist across sessions and keep bypassing the new root inference.
The collision-rule and sorting-layer changes do not explain a hard paint failure; the
parent/root mismatch does, and it affects both current live scenes named in the task.
