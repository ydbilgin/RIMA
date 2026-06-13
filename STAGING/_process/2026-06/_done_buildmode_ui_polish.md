# Build Mode UI polish + overlap fix — DONE (2026-06-13)

## Files
- NEW `Assets/Scripts/UI/BuildMode/BuildModeUiStyle.cs` (~290 lines) — shared Act1 style helper (palette consts, Jersey10 font loader, MakePanel/MakeHeader/MakeButton/MakeHintBox, ApplySelected).
- `Assets/Scripts/UI/BuildModeController.cs` (+~70) — overlap-hide + restore + OwnCanvas-aware exemptions.
- `Assets/Scripts/UI/BuildMode/BuildPlacementController.cs` (~ -10 net) — left BUILD panel rebuilt on helper; ButtonStyle highlights; OwnCanvas accessor.
- `Assets/Scripts/UI/BuildMode/BuildTileBrushController.cs` (~ -5 net) — right TILE BRUSH panel rebuilt on helper; radius indicator; OwnCanvas accessor.

## Overlap-hide mechanism (Task A)
On EnterBuildMode (after the palette canvas is built) HideOtherUiCanvases() does FindObjectsOfType<Canvas>(true) -> keeps only isRootCanvas && enabled && not our palette/brush canvas -> sets Canvas.enabled=false (NOT the GameObject) and records each. RestoreOtherUiCanvases() re-enables exactly that recorded set on ExitBuildMode + OnDestroy. Our two overlay canvases are exempt by reference; EventSystem/cursor/ghost are not canvases so untouched.

## Visual changes (Task B)
Both panels now share one helper: dark-slate #16181C@0.93 bg + 1px slate border, bold UPPERCASE header with ember underline, premium buttons (slate idle -> EMBER fill + dark text + left ember accent bar when selected, replacing the old bright cyan), segmented PROP|TILE, bottom-left hint box (muted hotkeys) + ember radius indicator. Jersey10 SDF font (matches DirectorMode). CanvasScaler ScaleWithScreenSize ref 1920x1080. Side-anchored panels leave the center play area clear.

## Compile
refresh_unity force/all (imported the new .cs + .meta) -> read_console: 0 errors, 0 Build Mode warnings.

## Notes
- NOT VERIFIED visually: Unity not in Play Mode (per spec, user screenshots). ScreenSpaceOverlay not MCP-screenshottable.
- No scene/prefab/.unity edits; no gameplay/placement/brush logic touched (UI construction + canvas-hide only).
- New script needed a force/all asset refresh before it compiled (scope=scripts alone did not import the new file's .meta).
