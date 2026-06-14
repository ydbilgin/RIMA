ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

**Output dosyası:** `CODEX_DONE_room_painter_day3.md` (max 500 kelime)
**Code dosyaları:** `Assets/Editor/RoomPainter/` altına yaz/düzenle

---

# Amaç

Phase A Day 3 — SceneView click placement + Cliff/Parallax sekmeli palet + ghost preview + iso cell snap.

## Mevcut state (Day 1+2 LIVE)

- Window: `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` (268 LOC, Asset Palette panel LIVE)
- Helper: `Assets/Editor/RoomPainter/Helpers/RoomPainterAssetScanner.cs` (151 LOC)
- SO: RoomPainterAsset, RoomLayerData, RoomData, RoomLayer enum (Assets/Scripts/RoomPainter/)
- Compile clean
- Menu LIVE, asset palette browse + select + Layer dropdown çalışıyor

## Sentez kararı (S110, locked memory: `cliff_pivot_manual_brush_2026_05_26.md`)

**Pattern C (sekmeli paletler) + Sonnet UX details + agy factor preset.** Detay:

### Sekmeli palet (Day 3 yapı değişikliği — Day 2 single-pane'i sekmeli yap)

Asset Palette panelin **üstüne 2 sekme** ekle:
- **"Gameplay Cliffs"** (default seçili): otomatik collider + Sorting Layer "Default" + Y-sort active
- **"Parallax BG Cliffs"**: collider yok + otomatik `ParallaxLayer` component + selected tier factor (aşağıda)

Sekme seçimine göre `_targetLayer` field otomatik set olur (`Cliff` veya `Parallax`). Mevcut Layer dropdown gizlenir (sekme zaten kontrol ediyor).

### Parallax tier preset (sekme = Parallax aktifken)

Sekme altında dropdown:
```
FG       1.20
Playable 1.00
Near     0.65   <- default Parallax BG için
Mid      0.40
Far      0.22
Skyline  0.10
Horizon  0.03
```
Tier seçimi `_selectedParallaxTier` field'ında saklanır.

## Görev — 5 ana iş

### 1. Sekmeli palet UI
`RimaRoomPainterWindow.cs` üst bölüm:
- `EditorGUILayout.BeginHorizontal` → 2 toggle button (Gameplay Cliffs / Parallax BG Cliffs)
- Aktif sekme highlight (cyan vs purple)
- Aktif sekmeye göre `_targetLayer = RoomLayer.Cliff` veya `RoomLayer.Parallax`
- Parallax sekmesi aktifken: tier dropdown göster (yukarıdaki preset list)

### 2. SceneView placement handler
**Path:** `Assets/Editor/RoomPainter/RoomPainterScenePlacer.cs` (NEW)

ColliderPainter pattern reuse (`Packages/com.laureth.painter-suite/Editor/Colliders/ColliderPainter.cs` — `OnSceneGui` + drag mouse + MouseDown/Drag/Up state):

- `SceneView.duringSceneGui += OnSceneGui` (window OnEnable)
- MouseDown (button 0): `BeginUndoGroup("Paint " + targetLayer)` + ilk cell paint
- MouseDrag: cell sınırı geçince yeni cell paint (HashSet ile aynı cell'i tekrar paint etmeyi engelle)
- MouseUp: `CollapseUndoOperations`

### 3. Iso cell snap (Sonnet R3 risk fix — kritik!)
**Helper:** `RoomPainterScenePlacer` içinde `SnapToCell` method:
```csharp
Grid grid = ...; // sahnedeki active Grid (Object.FindAnyObjectByType<Grid>())
Vector3 worldPos = sceneViewMousePos;
Vector3Int cellPos = grid.WorldToCell(worldPos);
Vector3 snappedWorld = grid.GetCellCenterWorld(cellPos);
```
Iso grid (cellLayout=Isometric, cellSize=(1, 0.609, 1)) için Grid.WorldToCell zaten iso math yapar — manuel iso vector hesap GEREKMİYOR.

### 4. Ghost preview color coding (Sonnet design)
`OnSceneGui` Repaint:
- Selected asset sprite'ını cursor-cell'de yarı saydam göster
- Aktif sekme = Cliff → cyan tint `Color(0.4, 0.9, 1.0, 0.6)`
- Aktif sekme = Parallax → purple tint `Color(0.8, 0.5, 1.0, 0.6)`
- `Handles.Label` ile küçük etiket "[Cliff]" veya "[Parallax (Near 0.65)]"

### 5. GO instantiation (placement aksiyon)
Her cell paint'inde:
- `GameObject.Instantiate(selectedAsset.prefab)` veya yeni GO + SpriteRenderer.sprite = selectedAsset.sprite
- Transform.position = `grid.GetCellCenterWorld(cellPos)`
- Sekme = Cliff:
  - GO.name = `"Cliff_" + selectedAsset.name`
  - Sorting Layer "Floor" / Order 5
  - (Phase A'da collider EKLEME — Day 5+ Phase B'ye it)
- Sekme = Parallax:
  - GO.name = `"Parallax_" + selectedAsset.name + "_" + tierName`
  - `parallaxLayer = GO.AddComponent<LaurethStudio.PainterSuite.Runtime.ParallaxLayer>()` — namespace import: `using LaurethStudio.PainterSuite.Runtime;`
  - `parallaxLayer.factor = tierValue` (preset'ten)
  - Sorting Layer "Default" / Order = -100 - tierIndex (Parallax arkada)
- `Undo.RegisterCreatedObjectUndo(go, "Paint...")` her instantiate'te
- `EditorUtility.SetDirty(sceneRoot)` + `MarkSceneDirty`

### 6. Variant cycle (Sonnet design — Scroll wheel)
SceneView'da Scroll wheel event:
- Eğer aktif palette filter "Cliff" kategorisinde → `_selectedAsset = palette[i+1 mod count]`
- `e.Use()` event consume

## Yapma

- Erase mode (Alt+click) — **opsiyonel, zaman kalırsa**, scope dışına it (Day 4 candidate)
- RoomData persist (Save) — Day 5
- Pattern Suite v1.1+ port — Phase B
- Mevcut RoomPainterAssetScanner.cs içeriği bozma — sadece scanner'ı kullan

## Verification

1. `grep -n "duringSceneGui\|RoomPainterScenePlacer" Assets/Editor/RoomPainter/` → en az 3 hit
2. `grep -n "ParallaxLayer\|_selectedParallaxTier\|Gameplay Cliffs\|Parallax BG" Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` → en az 4 hit
3. `grep -n "Grid.WorldToCell\|GetCellCenterWorld" Assets/Editor/RoomPainter/RoomPainterScenePlacer.cs` → en az 2 hit
4. Unity compile error 0
5. SceneView: Click → cliff sprite görünür, ghost overlay color sekme'ye göre değişir

## Çıktı

`CODEX_DONE_room_painter_day3.md`:
- Dosya değişiklikleri + LOC
- Verification grep çıktıları
- Compile durumu
- Eğer Pattern Suite asmdef ref edemediysen (LaurethStudio.PainterSuite.Runtime) BLOCKED + ne göründü
- Eğer iso Grid sahnede yoksa BLOCKED + nasıl fallback yaptın
