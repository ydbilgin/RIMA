# CODEX DONE - Drag Paint Auto Connect

## Fix
- `Assets/Editor/RimaUnifiedPainterWindow.cs:2744-2756`
  - Wall auto-connect paint now accepts `MouseDown` + `MouseDrag`.
  - Same-cell wall spam is blocked with `FindWallAtCell(cellPos) == null`.
  - Prop paint now accepts `MouseDown` + `MouseDrag`.
- `Assets/Editor/RimaUnifiedPainterWindow.cs:2774-2776`
  - Non-floor erase now accepts `MouseDown` + `MouseDrag`.

## UnityMCP Verification
- Opened `Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity`.
- Reflected wall paint core test over 4 adjacent cells:
  - `wallCorePaint=4`
  - `wallCoreErase=0`
- Reflected prop paint core test:
  - `propCorePaint=3`
  - Prop drag path is enabled in `PerformAction`.
- Console errors: `0`.

## Validation
- `refresh_unity` script compile: success.
- `validate_script Assets/Editor/RimaUnifiedPainterWindow.cs`: `0` errors, `3` pre-existing/performance warnings.
