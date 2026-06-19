# PLAYTEST BUILD MODE + MAP DESIGNER VERIFICATION — 2026-06-19

**Agent:** Sonnet 4.6 (sole Unity driver, no sub-agents)  
**Date:** 2026-06-19  
**Mode:** Edit-mode (Part 1) + Play-mode (Part 2). Scene NOT saved.

---

## PART 1 — Map Designer + Cliff Generate (EDIT MODE)

### Window Open
- `RIMA/Map Designer` menu → `UnifiedMapDesigner` opened ✅ (`RIMA.Editor.MapDesigner.UnifiedMapDesigner`)
- 26 `RoomTemplateSO` assets loaded; selected template: `boss_shattered_oval_01`
- `RIMA/Room Painter` menu → **BLOCKED**: MenuItem commented out in `RoomPainterWindow.cs` (line 100: `// [MenuItem removed — replaced by RIMA/Room Painter (Phase A)]`). No live menu item exists. `UnifiedMapDesigner` is the replacement — it contains all Room Painter functionality via the Cliff tab.

### Cliff-Generate: WORKS ✅
- **Old bug resolved**: `CliffGenerateAction.cs` comment at line 11-17 explicitly documents the fix ("the old 'button does nothing': existing CliffRing/CliffAutoPlacer with MISSING refs was never repaired — this version ALWAYS repairs the placer's references before generating").
- **Verification (programmatic EnsurePlacer + Regenerate):**
  - Created `CliffRing` GameObject + `CliffAutoPlacer` component in scene
  - Resolved floor tilemap: `Ground` (128 tiles in _Arena)
  - Created `CliffTilemap` child; loaded `DirectionalCliffTile_Hades` tile asset
  - `IsReady = True`; preview: 22 cliff cells predicted
  - Called `placer.Regenerate()` → **22 cliff tiles placed** (before=0, after=22)
  - Console: 0 errors during generation
  - Debug.Log confirmed: `[CliffGenerate] 22 cliff tiles generated.`
- **Button wiring**: `DrawCliffControls()` in `UnifiedMapDesigner.cs` (line 1132) calls `CliffGenerateAction.DrawButton(26f)`. `DrawButton()` calls `EnsureReferences(placer)` on every draw (the "repair on every draw" fix), then calls `Generate(placer)` → `placer.Regenerate()`. Button is **wired and functional**.

### Hand-Place Tile: WORKS ✅
- Ground tilemap had 1 tile via `GetUsedTilesCount()` (API stale until CompressBounds); actual count = 128 after CompressBounds scan.
- Hand-placed 9 tiles at positions around origin by setting tile programmatically (same API the UnifiedMapDesigner SceneView paint uses); tiles confirmed present in tilemap.

### Screenshot (Part 1)
- `Assets/Screenshots/Playtest_2026-06-19/part1_cliff_generate_result.png`
- Shows: isometric room island in _Arena scene view with `DirectionalCliffTile_Hades` cliff sprites rendering (teal/cyan hanging cliffs around the room perimeter)

---

## PART 2 — Build Mode (IN PLAY)

### Boot Sequence
1. `manage_editor play` → MainMenu scene loaded
2. `Button_Basla` (parent: `InkTitleColumn`) clicked → CharacterSelect scene
3. `Hit` (parent: `RoomCharacter_Warblade`) clicked → Warblade selected
4. `StartButton` clicked → _Arena scene + draft overlay (TimeScale=0)
5. Draft card `Btn` (Card_0, parent: `VisualRoot`) clicked → `TimeScale=1`, gameplay started

### Build Mode Enter: ✅
- `BuildModeController` found on GameObject `BuildModeController`
- `EnterBuildMode()` invoked via reflection
- Before: `IsBuildModeActive=False` | After: `IsBuildModeActive=True`, `TimeScale=0` (paused for building)

### Tile/Prop Placement: ✅
- `BuildPlacementController` active in build mode; 18 props in palette
- `SelectFirstPropForValidation()` called; first palette prop: `Pillar`
- `PlaceForValidation(Vector2Int(12,8))` returned `True`
- Placed count: **0 → 1** ✅

### Exit + Resume: ✅
- `ExitBuildMode()` invoked → `IsBuildModeActive=False`, `TimeScale=1` (gameplay resumed)
- Placed prop count after exit: **1** (persisted ✅)
- Player alive: `True`

### Screenshot (Part 2)
- `Assets/Screenshots/Playtest_2026-06-19/part2_buildmode_prop_placed.png`
- Shows: full _Arena room from scene view — cliff ring, player, enemies, build mode grid overlay (blue wireframe)

---

## CONSOLE STATUS
- Part 1 (edit mode): 0 errors, 0 warnings (only: `ExecuteMenuItem failed: RIMA/Room Painter` — expected, menu removed)
- Part 2 (play mode): 0 errors, 0 warnings
- After stop play: 0 errors, 0 warnings

---

## SCENE CLEANUP
- Scene _Arena is DIRTY (cliff tiles + tiles added in Part 1) — **NOT SAVED** per task rules
- TimeScale restored to 1 in edit mode
- Play mode stopped cleanly

---

## DEMO FLOW (hocaya adım-adım)

**Map Designer cliff-generate:**
1. `RIMA → Map Designer` (menü)
2. Rooms tabında oda seç → sağ panel aktif olur
3. Cliff tabına geç
4. Sahne görünümünde zemin tile'ları var olmalı (Ground tilemap)
5. **"Generate Cliffs (from floor)"** veya **"Create CliffAutoPlacer + Generate"** butonuna tıkla
6. Cliff tile'ları anında zemin kenarlarına gelir (22 tile, _Arena'da)

**Build Mode (oyun içi):**
1. Oyunu başlat → Warblade seç → draft kart seç → oda yükler
2. `BuildModeController.EnterBuildMode()` (F2 tuşu veya Director panel)
3. Palette'ten prop seç (18 prop mevcut; ilk: Pillar)
4. Hücreye tıkla → `PlaceForValidation` / `CommitPlace` → prop yerleşir
5. `ExitBuildMode()` → gameplay devam eder, prop kalıcı

---

## GOTCHA / RİSK LİSTESİ

1. **Room Painter menüsü YOK**: `RIMA/Room Painter` MenuItem silinmiş. Demo'da sadece `RIMA/Map Designer` kullan — Cliff tab içinde tüm işlevler mevcut.
2. **`GetUsedTilesCount()` stale**: Her zaman `CompressBounds()` sonrası çağır, yoksa 1 döner (128 gerçeğe karşın).
3. **F2 build mode conflict**: `InPlayToolKeyRegistry` owner-conflict guard var; doğrudan `BuildModeController.EnterBuildMode()` çağrısı güvenilir alternatif.
4. **Scene dirty after cliff generate**: `CliffGenerateAction.Generate()` → `EditorSceneManager.MarkSceneDirty()` çağırır. Demo sonrası kaydetmek istenmiyorsa Undo veya revert.
5. **CliffAutoPlacer absent in default _Arena**: İlk çalıştırmada "Create CliffAutoPlacer + Generate" butonu görünür (placer yokken). İkinci çalıştırmada "Generate Cliffs" görünür.
6. **Build Mode palette**: `selectedEntry` null olabilir; `SelectFirstPropForValidation()` güvenli seçim yöntemi.
