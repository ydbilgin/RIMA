# Wall Alignment Fix + Sorting Layer Setup - Codex Report

## Status

BLOCKED before edits.

Reason: `ProjectSettings/TagManager.asset` current sorting layers do not match the task precondition. The task states current layers are expected to be `Default`, `Ground`, and requires adding `Floor` and `Entities`. Actual project state already has 11 sorting layers, including an existing `Entities` layer, and `Ground` is not index 1.

Per task hard constraint: "BLOCKED if unclear: Mevcut sorting layer durumu beklenenden farkliysa ... durdur".

## Bolum 1: Alignment Fix

No re-import or meta edit was performed because the task entered BLOCKED state before project changes.

### Current Meta State

| File | spriteAlignment current | spritePivot current | Action |
|---|---:|---|---|
| straight_horizontal | 0 | `{x: 0.5, y: 0.03125}` | not changed |
| corner_L_NE | 0 | `{x: 0.5, y: 0.03125}` | not changed |
| arch_opening | 0 | `{x: 0.5, y: 0.03125}` | not changed |
| partition_low_stub | 7 | `{x: 0.5, y: 0.041666668}` | not changed |
| cyan_rift_integrated | 0 | `{x: 0.5, y: 0.03125}` | not changed |

## Bolum 2: Sorting Layer Setup

### TagManager Current

- Index 0: Default (uniqueID 0, locked 0)
- Index 1: Patch (uniqueID 1365605006, locked 0)
- Index 2: Scatter (uniqueID 27625511, locked 0)
- Index 3: Detail (uniqueID 351335743, locked 0)
- Index 4: Accent (uniqueID 1570199623, locked 0)
- Index 5: Props (uniqueID 399489520, locked 0)
- Index 6: Ground (uniqueID 2024493761, locked 0)
- Index 7: Walls (uniqueID 593505845, locked 0)
- Index 8: Entities (uniqueID 1293760285, locked 0)
- Index 9: VFX (uniqueID 200, locked 0)
- Index 10: Wall (uniqueID 2024493762, locked 0)

### Scene Current

- `Assets/Scenes/Demo/PathC_BaseTest.unity` `Floor_Tilemap`
- `m_SortingLayerID: 0`
- `m_SortingLayer: 0`
- `m_SortingOrder: -100`

## Verify

- 5 wall sprites Pivot pixel verify: NOT RUN, blocked before Unity re-import.
- 2 new sorting layers added: NOT RUN, blocked by existing unexpected layer state and existing `Entities`.
- Floor_Tilemap on Floor layer: NOT RUN, blocked before scene edit.
- dotnet build: NOT RUN, no code change.
- Unity console: NOT RUN, no Unity edit.

## Git Diff Summary

- No new task edits were intentionally made after reading the task.
- Pre-existing git diff already includes the 5 listed wall `.meta` files.
- `ProjectSettings/TagManager.asset` was not modified.
- `Assets/Scenes/Demo/PathC_BaseTest.unity` was not modified.

## Acik Sorular

- Should `Floor` be inserted into the existing 11-layer stack without reordering existing layers, or should the stack be normalized to `Default -> Ground -> Floor -> Entities` despite existing `Patch`, `Scatter`, `Detail`, `Accent`, `Props`, `Walls`, `VFX`, and `Wall` layers?
- Should the existing `Entities` layer at index 8 be reused, moved, or left in place?
