# Painter Rename + Per-Category Scale Fix

## Rename verdict
- PASS
- File moved: `Assets/Editor/RimaUnifiedPainterWindow.cs` -> `Assets/Editor/RimaWorldPainterWindow.cs`
- Meta moved: `Assets/Editor/RimaUnifiedPainterWindow.cs.meta` -> `Assets/Editor/RimaWorldPainterWindow.cs.meta`
- GUID preserved: `af191c66b83e95f4c82e860e54de3d00`
- Class renamed: `RimaUnifiedPainterWindow` -> `RimaWorldPainterWindow`
- Menu path renamed: `RIMA/Tools/World Painter`

## Reference update list
- `Assets/Editor/RimaWorldPainterWindow.cs`: class, menu title/path, preview object name, and old "Unified Painter" UI/log labels updated.
- `Assets/Editor/CollisionRulesSO.cs`: collision mode reference updated to `RimaWorldPainterWindow.CollisionMode`.
- `grep -r "RimaUnifiedPainterWindow" Assets/`: 0 matches.
- `grep -r "Unified Painter" Assets/`: 0 matches.

## Per-category scale verdict
- PASS
- Defaults: Floor `1.0`, Wall `0.5`, Prop `0.4`, Mob `1.0`.
- Toggle: `useCategoryScale = true` by default.
- UI: `Per-Category Scale` toggle plus Floor/Wall/Prop/Mob sliders added under Scale.
- Apply logic: preview, prefab placement, auto-connected wall placement/replacement, collision preview, active collider gizmo, default collision resolution, and base-alignment offset now use effective category scale.
- Backward compatibility: `useCategoryScale = false` returns `prefabScaleMultiplier` as universal override.

## Compile status
- PASS
- Unity `read_console` errors/warnings: `Retrieved 0 log entries.`
- `validate_script` on `RimaWorldPainterWindow.cs`: 0 errors.
- `validate_script` on `CollisionRulesSO.cs`: 0 errors, 0 warnings.

## Runtime validation
- Window open: PASS via `RIMA/Tools/World Painter`.
- Reflection check: `WINDOW_OK fields=True category=1/0.5/0.4/1 universal=1.37`.

## Commit
- `bf6492571022c9f03710724eb6e787f3468728f2`
- Message: `[S96] Painter rename + per-category scale fix`

## Next-step öneri
- No old `RimaUnifiedPainterWindow` or `Unified Painter` references remain under `Assets/`.
