# Sprint 12 Props Mode MVP - Codex DONE

Date: 2026-05-16

## Implemented

- Added `PropDefinitionSO`, `PropPlacementData`, and `PropFootprintValidator`.
- Extended `RoomTemplateSO` with `props`.
- Added editor-only `PropsTab` and `PropPlacer`.
- Integrated `Props` mode into `MapDesignerBrushWindow`.
- Added sample asset `Assets/Data/Brush/Props/Barrel/barrel_001.asset`.
- Added 21 EditMode tests covering defaults, validation, serialization, and placement.

## Open Questions Resolved

- OQ1 sorting layer runtime integration: V1 keeps sorting as editor data and preview only. Runtime tilemap sorting integration is deferred to Sprint 13.
- OQ2 footprint origin convention: V1 uses bottom-left origin. A 2x2 prop at `(3, 3)` occupies `(3,3), (4,3), (3,4), (4,4)`.
- OQ3 GUID lookup at runtime: V1 stores GUID strings and resolves via `AssetDatabase` in editor-only paths. Runtime registry is deferred to Sprint 13.
- OQ4 empty forbidden roles after deserialization: Validator treats `null` as empty, so user-cleared forbidden roles do not hard-fail placement.

## Verification

- `dotnet build RIMA.Runtime.csproj --no-restore`: PASS
- `dotnet build RIMA.Editor.csproj --no-restore`: PASS
- `dotnet build RIMA.MapDesigner.Brush.EditorUI.csproj --no-restore`: PASS
- `dotnet build RIMA.Brush.Tests.csproj --no-restore`: PASS
- `dotnet build RIMA.Tests.EditMode.csproj --no-restore`: PASS
- Targeted Props EditMode tests: PASS 21/21
- Full EditMode tests: PASS 282/282, with 1 existing inconclusive prefab-health check reported by Unity.

## Notes

- `RoomTemplateSO` has no serialized walkable grid. Sprint 12 validator uses `cameraBounds.tileRect` as the available V1 walkable region and falls back to `bounds` when camera bounds are empty.
- Unity batchmode could not start because the project was already open. Refresh, compile, and tests were run through the connected Unity editor.
- Existing dirty worktree changes in `WallOverlayPainter.cs` and untracked `Composition/` files were present before this task and were not modified by this implementation.
