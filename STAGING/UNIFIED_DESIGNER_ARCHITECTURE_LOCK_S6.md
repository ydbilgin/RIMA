# RIMA — UNIFIED DESIGNER: ARCHITECTURE LOCK (S6, ultracode)

**Status:** 🔒 LOCKED by Opus from the 8-agent Understand workflow (wf_675e5b21) + cx + ax detection.
**Date:** 2026-06-01. Implementation in progress.

---

## THE BIG PICTURE
One tool, two surfaces, one data spine. The Editor window and the in-game F2 overlay
are THIN VIEWS over a shared, runtime-safe core. They run in parallel and edit the SAME room.

```
                ┌─────────────────────────────────────────────┐
   Editor  ───► │  UnifiedMapDesigner (RIMA.Editor)            │  Undo/AssetDatabase wrapper
   window       │     tabs · library · layer panel · palette   │
                └───────────────┬─────────────────────────────┘
                                │ calls
                ┌───────────────▼─────────────────────────────┐
   F2      ───► │  UnifiedDesignerCore (RIMA.Runtime, NEW)     │  ◄── single source of truth
   overlay      │   active RoomData · category · layer · brush │
   (InPlay…)    │   Paint/Erase/GenerateCliff/Save/Load        │
                └───────────────┬─────────────────────────────┘
                                │ uses (all already runtime-safe)
        RoomDataMutator · RoomDataJson · RuntimeAssetRegistry · CliffAutoPlacer
                                │
                        RoomData (.asset canonical + .room.json sidecar)
```

## KEY DECISIONS (locked)

### D1. Shared core lives in RIMA.Runtime — NO new asmdef
`RIMA.Runtime` already contains RoomData, RoomDataMutator, RoomDataJson,
RuntimeAssetRegistry, CliffAutoPlacer, RoomLayer/RoomLayerData, AND the F2 overlay
(`Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs`). `RIMA.Editor` references it.
→ New `UnifiedDesignerCore` + `DesignerCategory` go in RIMA.Runtime (runtime-safe, no `UnityEditor`).
The ONE editor-only piece (`RoomDataComposer`, AssetDatabase/PrefabUtility) gets a runtime
sibling that resolves assets via RuntimeAssetRegistry.

### D2. ONE data model — RoomData wins; retire RoomLayoutData
There are TWO parallel room representations: `RoomData` (rich: Wang walls, 10 layer slots,
prop layers) and `RoomLayoutData` (LiveTool JSON). RoomData wins. RoomLayoutData becomes
a thin view or is retired. Both surfaces read/write RoomData via the shared core. This kills
the "edit on one surface, invisible on the other" bug.

### D3. Categories (user requirement: not just floor)
`enum DesignerCategory { Floor, Cliff, Object, Portal, Light }` (RIMA.Runtime). Mapping:
- Floor  → `RoomData.floorCells` (RoomLayer.Floor)
- Cliff  → `RoomData.cliffCells` (RoomLayer.Cliff) + logical Generate-Cliff
- Object → `RoomData.propPlacements` (RoomLayer.Props)
- Portal → NEW `RoomData.portalPlacements` (carries targetNodeId/exit graph metadata props can't)
- Light  → `RoomData.propPlacements` (RoomLayer.Lighting)
Walls (Wang) remain available under Object/Advanced (kept, not headline per PM·5).
The palette filters RuntimeAssetRegistry by category (registry already has category+tags).

### D4. Shiftable depth-layer stack (user's L1/L2/L3 model)
RoomLayerData ALREADY has sortingLayerName + defaultOrder + depthValue + ySort — the model
exists, it's just never shown or consistently applied. Add a Layer Panel that edits these.
Map the user's stack onto REAL existing sorting layers:
| User slot | Sorting layer | Order | Notes |
|---|---|---|---|
| L1 Floor (top, walkable) | `Ground` | 0 | iso floor tiles |
| L2 Cliff (just under floor) | `Cliff` | -10 | near-depth (normalize the hardcoded -50) |
| (between L2 & L3) preview-islands | `Backdrop` | -50 | next-room islands shown down in the void |
| L3 backdrop art (far below) | `Backdrop_Far` / `Skybox` | -100 | Codex-made depth images, camera-relative parallax |
Layers are SHIFTABLE: the panel nudges defaultOrder/depthValue per layer, no code edit.
Normalize the hardcoded `-50`/`"Ground"` in CliffGenerateAction into RoomLayerData.

### D5. Cliff-generate fix (root cause = integration, not algorithm)
The algorithm is sound. It no-ops because: (a) an existing CliffRing/CliffAutoPlacer with
missing refs is never repaired (only created-when-absent); (b) wrong/empty floor tilemap
auto-picked; (c) target tilemap hidden/under-sorted. Fix:
1. `CliffGenerateAction` (and the C-shortcut) REPAIR an existing unready placer (wire
   floor+cliff tilemap+tile), not just create-when-missing.
2. Resolve floor/cliff tilemap by explicit name + active-room context (not "first tilemap").
3. Generate from `RoomData.floorCells` when a room is active (works pre-compose), scene
   tilemap as fallback.
4. Surface count + selected tilemaps + readiness reason in the Cliff tab UI.
5. Migrate ManualPainted/ManualOverride cells into RoomData (so they survive room reload).

### D6. Consolidation (one front door, prune the rest)
- **KEEP-as-core:** `UnifiedMapDesigner` (front door), `RimaRoomPainterWindow` logic
  (RoomData-backed — fold into unified), `InPlayMapPaintOverlay` (F2), `RoomDataAuthoringController`
  (library CRUD), `AssetPackBrowserWindow` (discovery → route placement through RoomData).
- **Demote/[Obsolete] + MenuItem redirect → unified:** `MapDesignerBrushWindow`,
  `RimaVisualMapEditorWindow`+`VisualEditorScenePainter`, `BlueprintPainterWindow`,
  `MinimalTilePainter` (deprecated), `RoomPainterScenePlacer` (scene-direct), `DecorCliffPainter`.
- **Merge duplicate:** two `TileImportWizard` copies → one (move to Project context menu).
- **Keep utilities:** PixelLab importers, Tile Palette rebuild, EncounterMenu.
- Replace UnifiedMapDesigner's brittle reflection-into-two-hidden-windows with direct
  category-driven rendering against the core.

### D7. Tests (EditMode, RIMA.Runtime-testable core)
- Cliff-generate logic: floor-shape → expected cliff cells (repair path, RoomData-first).
- Category routing: each DesignerCategory writes the right RoomData collection/layer.
- Layer model: shifting a layer's order updates RoomLayerData; compose respects it.
- RoomData round-trip parity: RoomData → JSON → RoomData equals original INCLUDING new
  portalPlacements (the DTO-sync regression guard).
- Dual-surface parity: a mutation via the core from "editor context" and "runtime context"
  produces identical RoomData.

### D8. Hygiene note (SEPARATE, user-gated — NOT in main refactor)
`ProjectSettings/TagManager.asset` has 200+ junk tags AND junk sorting layers
("asd","hgj","jj","Heightt"...). This violates "everything organized" but DELETING tags/
sorting-layers is risky (scenes reference them by index/name). Flag to user; do as an
opt-in cleanup pass AFTER the tool works, with backup. Do not block the refactor on it.

## BUILD ORDER (phased, Unity-compile + console-clean after each)
1. **Data layer** — DesignerCategory enum + PortalPlacement + RoomData.portalPlacements +
   RoomDataDTO/Json sync + RoomLayer normalization constants. (foundation)
2. **UnifiedDesignerCore** — surface-agnostic state + Paint/Erase/GenerateCliff/Save/Load
   over RoomDataMutator + a runtime composer sibling.
3. **Cliff-generate fix** — repair/resolve/RoomData-first/feedback (D5).
4. **Editor view** — UnifiedMapDesigner tabs (categories) + library + layer panel, drives core.
5. **F2 view** — InPlayMapPaintOverlay mirrors the same tabs via core.
6. **Consolidation** — demote legacy windows, redirect MenuItems, retire RoomLayoutData.
7. **Tests + Unity verify** — EditMode suite + full compile/console clean.
8. **cx + ax review** — adversarial; fold sensible findings. Update status + memory.
