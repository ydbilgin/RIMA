# Codex Review: Multi-Layer Painter Phase 1 — IMPLEMENTATION

## Context

Plan LOCKED: `STAGING/MULTILAYER_PAINTER_PLAN_v1.md`. Sonnet returned diffs, orchestrator applied. Unity compiled clean (no console errors per `read_console`). Şimdi Phase 1 değişikliklerinin code-quality + regression review'u.

## Değişen / yeni dosyalar (4)

1. **NEW:** `Assets/Scripts/MapDesigner/Room/Data/BackgroundLayerData.cs` — [Serializable] class, 7 field (layerName, sprite, sortingOrder, offset, scale, tint, visible).
2. **EDIT:** `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs` — `Sprite backgroundSprite` field removed, `List<BackgroundLayerData> backgroundLayers = new()` added in same position. `schemaVersion` STAYS "1.0".
3. **EDIT:** `Assets/Scripts/MapDesigner/Room/Runtime/RoomBankRuntimeTester.cs` — single-sprite spawn block replaced with foreach-layer block using **localPosition** (room-local coord, per plan §3 LOCK).
4. **NEW:** `Assets/Editor/MapDesigner/Inspectors/RoomTemplateSOInspector.cs` — `[CustomEditor(typeof(RoomTemplateSO))]` ReorderableList for backgroundLayers + iterator-based default field draw (excludes `backgroundLayers` to avoid double-render).

## Review checklist (PASS/FAIL evidence required)

1. **Compile clean** — `read_console` zaten temiz. Confirm via UnityMCP if you have access; otherwise grep for obvious syntax issues.

2. **EditMode test suite hala PASS mi?** — Was 419/419 this morning. Plan claims no test refs `backgroundSprite`. RUN `mcp__UnityMCP__run_tests` action=run, mode=editmode. Report passed/total.

3. **10/10 existing RoomTemplate `.asset` deserialize?** — Field removal may break old assets. Use `mcp__UnityMCP__execute_code` to LoadAssetAtPath all 10 (`Assets/Data/Rooms/Library/*.asset`) and verify each loads non-null + new `backgroundLayers` field is empty list (not null).

4. **Custom Inspector renders correctly?** — Open Spawn_01.asset in Unity, switch Inspector. Should show:
   - All default fields (schemaVersion, roomId, biomeId, etc.) drawn ONCE
   - "Painted Background Layers" header with empty ReorderableList
   - Walkable grid section still works
   No console errors on selection.

5. **Coordinate space LOCK respected?** — Inspect runtime spawn block (Section 3 of plan). Verify uses `localPosition`, NOT `position`. This was the Codex revision FAIL fix.

6. **Phase 1 success criteria coverage** (Plan §9):
   - BackgroundLayerData compiles ✓ (auto-confirmed by Unity compile pass)
   - List<BackgroundLayerData> replaces single Sprite ✓
   - ReorderableList Inspector with drag/thumbs ✓ (verify visually if possible)
   - 419 tests PASS ✓/❌
   - 10 assets deserialize ✓/❌
   - Demo deferred (Task #9 after this review)

7. **Reserved sortingOrder range docstring?** — Optional check: is the convention `-200..-50` for bg layers documented somewhere in the code (XML doc / Tooltip)? If only in plan, suggest adding to BackgroundLayerData class doc.

8. **Asmdef check** — Inspector lives under `Assets/Editor/MapDesigner/Inspectors/`. Parent asmdef `RIMA.MapDesigner.Editor.asmdef` covers subfolders. Confirm no separate asmdef needed.

9. **No double-Inspector-render** — Walk through `OnInspectorGUI` of `RoomTemplateSOInspector.cs`. The iterator skips `backgroundLayers` propertyPath, then ReorderableList draws it. Confirm logic.

## Output

`CODEX_DONE_multilayer_phase1_impl_review.md`:

```markdown
# Codex Review: Multi-Layer Painter Phase 1 Impl — VERDICT

## Verdict: PASS | FAIL | PASS_WITH_NOTES

## Checklist
1. Compile clean: PASS/FAIL
2. EditMode tests: X/Y PASSED  
3. Asset deserialize: 10/10 ✓ or list failures
4. Inspector renders: PASS/FAIL
5. Coordinate space LOCK (localPosition): PASS/FAIL
6. Phase 1 success criteria: 6/6 ✓ or details
7. sortingOrder docstring: PRESENT/MISSING (non-blocking)
8. Asmdef: OK/NEEDS_CHANGE
9. No double-render: PASS/FAIL

## Critical findings (FIX FIRST)
[list]

## Recommendations (non-blocking)
[list]

## Final go/no-go
[1-line: "PROCEED to Task #9 UnityMCP demo" or "FIX X before demo"]
```

## Hard limits

- KOD YAZMA. Review-only.
- UnityMCP available — use `run_tests` for EditMode, `execute_code` for asset load check, `read_console` for compile state.
- 10-min effort cap.
- Bittiğinde tek satır özet + verdict path'i.
