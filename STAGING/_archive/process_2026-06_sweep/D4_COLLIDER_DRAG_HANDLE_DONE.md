# D4: Per-Prefab Collider Drag-Handle — DONE

Date: 2026-05-27
Status: COMPLETE — 0 compile errors, 0 warnings

## Changed Files

### NEW
- `Assets/Editor/RoomPainter/AssetPipeline/ColliderShapeSwapper.cs` (~80 LOC)
  - Static helper: `SwapShape(GameObject root, ColliderShape newShape)`
  - Destroys existing Collider2D, adds new one, migrates size/offset/isTrigger best-fit
  - Box↔Circle↔Capsule all covered; Polygon guard with LogWarning
  - Undo-safe: Undo.DestroyObjectImmediate + Undo.AddComponent

### EXTENDED
- `Assets/Editor/RoomPainter/Inspector/Sections/PhysicsSection.cs` (+~110 LOC)
  - Added `using UnityEditor.SceneManagement`
  - `DrawPrefabColliderSection(asset)` called from `Draw()` before `DrawSceneInstanceControls`
  - Shape selector dropdown (Box/Circle/Capsule) — calls ColliderShapeSwapper.SwapShape on change
  - Read-only live display: "Size: W x H | Offset: (x, y) | Trigger: Yes/No"
  - "Edit in Prefab Mode" button → AssetDatabase.OpenAsset(prefab)
  - Polygon guard: logs warning, does not swap

- `Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs` (+~25 LOC)
  - Added `using UnityEditor.SceneManagement`
  - `IsInPrefabMode` property: `PrefabStageUtility.GetCurrentPrefabStage() != null`
  - `PrefabStageRoot` property: returns prefabContentsRoot
  - `OnSceneGui`: when in Prefab Mode, draws handles on selected prefab root/child (no binding required); exits early so scene mode logic is untouched

- `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` (+~15 LOC)
  - Added `using UnityEditor.SceneManagement`
  - `DrawStatusBar()`: D4 block checks `PrefabStageUtility.GetCurrentPrefabStage()`
  - When Prefab Mode active: shows bold yellow "Editing prefab: <name>" label in statusbar
  - Calls `Repaint()` to keep label live; auto-hides when Prefab Mode exits

## Verify Checklist
- [x] 0 compile errors
- [x] 0 compile warnings
- [ ] RimaRoomPainterWindow aç (RIMA > Room Painter)
- [ ] Decor (3) veya Object (4) mode seç
- [ ] Palette'ten prefab içeren asset seç
- [ ] Inspector "Prefab Collider (D4)" section görünür
- [ ] "Collider Shape" dropdown Box/Circle/Capsule içerir
- [ ] Shape değiştirince prefab collider type değişir
- [ ] "Edit in Prefab Mode" butonu tıklanınca Prefab Mode açılır
- [ ] SceneView'de 8 sarı handle drag çalışır (Box)
- [ ] Statusbar "Editing prefab: <name>" Prefab Mode'da görünür
- [ ] Ctrl+S → prefab save → Prefab Mode'dan çık → scene instance refresh

## YASAK uyumu
- PolygonCollider2D: LogWarning ile guard, swap edilmez
- D5a 3-pane layout: dokunulmadı
- D3 mode tabs: dokunulmadı
- Mevcut drag handle logic: sadece prefab mode awareness eklendi
