# D3: Painter Mode Tabs — DONE

Date: 2026-05-27

## Compile status
Unity compile: 0 errors / 0 warnings (verified via read_console after force refresh)

## Changed files

### NEW
- `Assets/Editor/RoomPainter/RoomPainterMode.cs`
  - Enum: `{ Tile=0, Cliff=1, Decor=2, Object=3 }`
  - ~12 LOC

### EXTENDED
- `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` (+~170 LOC)
  - D3 constants: `PainterModeKey`, `LayerFilterMaskKey`
  - D3 static data: `ModeColorTile/Cliff/Decor/Object`, `ModeDefaultLayerMask[4]`, `ModeNames[4]`, `LayerFilterNames[10]`
  - Serialized fields: `_currentMode`, `_layerFilterMask`
  - `OnEnable`: restores mode + layer mask from EditorPrefs
  - `OnGUI`: calls `HandleModeHotkeys()` → `DrawModeToolbar()` → `DrawLayerSubFilter()` before `DrawCategoryFilters()`
  - `HandleModeHotkeys()`: KeyDown Alpha1-4 / Keypad1-4 → `ApplyModeDefaults()` + evt.Use()
  - `DrawModeToolbar()`: horizontal toolbar row, 4 coloured toggle buttons, each 90px
  - `DrawLayerSubFilter()`: 2nd row with "All" + 10 individual layer toggles (bitmask multi-select)
  - `DrawStatusBar()`: "Mode: Tile" bold coloured label on left, existing hitbox status preserved right
  - `SetMode(mode)` public method (used by menu items)
  - `ApplyModeDefaults()`: sets layer mask to mode preset, persists to EditorPrefs
  - `GetModeColor(mode)` static helper
  - `GetModeStatusStyle()` lazy-init bold miniLabel style
  - Menu items: `RIMA/Room Painter Tools/Mode/Tile (1-4)` (4-surface rule)
  - `MatchesCurrentFilter`: layer bitmask gating added before category filter
  - `DrawInspectorPanel`: passes `_currentMode` to `_inspectorPanel.Draw(..., mode)`

- `Assets/Editor/RoomPainter/Inspector/RoomPainterInspectorPanel.cs` (+~130 LOC)
  - `Draw(asset, sceneInstance)` now delegates to overload with `RoomPainterMode.Tile`
  - `Draw(asset, sceneInstance, mode)` new overload — calls `DrawModeSpecificSection()` at top of scroll area
  - `DrawModeSpecificSection()`: collapsible section header (same band style as existing sections) for each mode
  - `DrawTileModeBody()`: Floor/Tile context, layer display
  - `DrawCliffModeBody()`: Cliff variant context, stub Regenerate button (D4 wire note)
  - `DrawDecorModeBody()`: Decor Walkable/Wall context, layer display
  - `DrawObjectModeBody()`: Gameplay object context, trigger collider read from sceneInstance

## Verify checklist
- [x] Unity compile: 0 error / 0 warning
- [x] `RoomPainterMode.cs` new file created, namespace `RIMA.RoomPainter.Editor`
- [x] `DrawModeToolbar()`: 4 coloured toggle buttons in toolbar row 2 (Tile/Cliff/Decor/Object)
- [x] Hotkeys 1-4 (Alpha + Keypad) switch mode inside the window
- [x] Mode switch snaps layer filter to preset defaults
- [x] `DrawLayerSubFilter()`: "All" + 10 individual layer toggles (bitmask multi-select, row 3)
- [x] Statusbar: "Mode: Tile" bold coloured label left, hitbox status right (preserved)
- [x] Inspector mode-specific section foldable band at top of scroll area
- [x] Menu: `RIMA/Room Painter Tools/Mode/Tile(1)` through `Object(4)` added (4-surface rule)
- [x] 3-pane layout (palette/preview/inspector widths) UNTOUCHED — only new rows above DrawMainPanels
- [ ] Manual playtest: open window, press 1-4, observe toolbar highlight + layer filter snap + statusbar label change

## D2 consumed
- `RoomLayer` enum values used for bitmask mapping (Floor=0, Cliff=2, Decals=5, Wall=3, Props=4)
- `AssetCategory` enum NOT directly consumed in D3 (D4 palette filter integration scope)

## Notes
- `BrushExecutorRouter` NOT touched (D5 scope, per YASAK)
- `AssetPostprocessor` pipeline NOT touched (D2 already done)
- Input.GetKey* NOT used — mode hotkeys use `EventType.KeyDown` on `Event.current` (IMGUI, no InputSystem dependency in Editor tools, correct per project rules)
- Layer filter "All" = all 10 bits set; default per mode matches spec (Tile→Floor, Cliff→Cliff, Decor→Decals+Wall, Object→Props)
