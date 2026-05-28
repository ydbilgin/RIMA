# D4: Per-Prefab Collider Drag-Handle (Option A — Prefab Mode)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: gerekirse `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"`

## Amaç
Per-prefab collider drag-handle authoring. Kullanıcı palette'te prefab seçer → Inspector pane'de Shape dropdown + "Edit in Prefab Mode" button → AssetDatabase.OpenAsset(prefab) → Prefab Mode'da SceneView drag handles → Ctrl+S → tüm scene instance'lar otomatik refresh.

## Bağlam (locked decisions 2026-05-27 gece)
- Option A: Prefab Mode (kullanıcı default kabul ettiği önerim)
- D5b Visual Collider Authoring infrastructure LIVE: `Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs` (262 LOC)
- Box / Circle / Capsule shape — Polygon DEFERRED
- Spec ref: `STAGING/RIMA_LIVE_TOOL_DECISION.md` Section 3 (drag-handle workflow)

## İş kalemleri

### 1. ColliderShapeSwapper helper (NEW)
- File: `Assets/Editor/RoomPainter/AssetPipeline/ColliderShapeSwapper.cs`
- Static class — `SwapShape(GameObject root, ColliderShape newShape)` method
- Mevcut Collider2D component'i (Box/Circle/Capsule) sil, yenisini ekle. Eski size/offset/isTrigger'ı best-fit migrate et:
  - Box → Circle: radius = max(size.x, size.y) / 2
  - Box → Capsule: size copy, direction Vertical default
  - Circle → Box: size = (radius*2, radius*2)
  - Circle → Capsule: size = (radius*2, radius*2), direction Vertical
  - Capsule → Box: size copy
  - Capsule → Circle: radius = max(size.x, size.y) / 2
- isTrigger flag her swap'ta korunur
- Undo.RecordObject + Undo.DestroyObjectImmediate + Undo.AddComponent
- enum ColliderShape { Box, Circle, Capsule }
- ~80 LOC

### 2. PhysicsSection.cs extend
- File: `Assets/Editor/RoomPainter/Inspector/Sections/PhysicsSection.cs`
- Yeni UI elementler:
  - **Shape selector dropdown:** "Collider Shape: [Box ▼]" (3 entry: Box/Circle/Capsule)
  - Shape değişince ColliderShapeSwapper.SwapShape çağır
  - **"Edit in Prefab Mode" button:** disabled if no prefab asset, enabled if SelectedAsset is prefab
  - Click → AssetDatabase.OpenAsset(prefab) → Prefab Mode açılır, Selection.activeGameObject prefab root
  - **Current size/offset/trigger display:** "Size: 0.48 × 0.84 | Offset: (0, 0.32) | Trigger: ☐"
  - Read-only — kullanıcı drag handle ile değiştirir, label live update
- ~40 LOC dropdown + 30 LOC button + display = ~70 LOC

### 3. RoomPainterColliderEditor.cs — Prefab Mode awareness
- File: `Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs` (262 LOC EXTEND)
- Mevcut OnSceneGUI handle drag logic korunur
- Prefab Mode tespiti: `PrefabStageUtility.GetCurrentPrefabStage() != null`
- Prefab Mode aktif iken:
  - Selection.activeGameObject prefab root mu kontrol
  - Handle drag ile değişen Collider2D component → Undo.RecordObject prefab asset version
  - Ctrl+S native Unity prefab save handle eder (extra kod yok)
- Scene mode aktif iken eski davranış korunur (instance edit)
- ~20 LOC ekleme

### 4. RimaRoomPainterWindow.cs — statusbar prefab indicator
- File: `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs`
- Statusbar sağ tarafa ek label: "Editing prefab: <name>" — sadece Prefab Mode aktifken görünür
- Prefab Mode'dan çıkınca otomatik gizlenir
- ~20 LOC

## Dosyalar (scope)
- `Assets/Editor/RoomPainter/AssetPipeline/ColliderShapeSwapper.cs` (NEW ~80 LOC)
- `Assets/Editor/RoomPainter/Inspector/Sections/PhysicsSection.cs` (EXTEND ~70 LOC)
- `Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs` (EXTEND ~20 LOC)
- `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` (EXTEND ~20 LOC)
- Toplam ~190 LOC

## YASAK
- PolygonCollider2D support (Phase 2 scope)
- Option B (instance + apply override) — sadece Option A
- D5a 3-pane layout bozma
- D3 mode tabs bozma
- Mevcut RoomPainterColliderEditor.cs drag handle behavior değişiklik (sadece prefab awareness eklenir)
- Yeni .cs → `mcp__UnityMCP__refresh_unity scope=all mode=force` ZORUNLU
- Input.GetKey* YASAK

## Verify
- UnityMCP: `refresh_unity scope=all mode=force` + `read_console` → 0 error / 0 warning
- RimaRoomPainterWindow aç (`RIMA > Room Painter`)
- Decor mode (3) veya Object mode (4) aktive et
- Palette'ten bir prefab seç (örn. statue_05 veya Chest)
- Inspector'da "Collider Shape: [Box]" dropdown görünür
- "Edit in Prefab Mode" button enabled → tıkla → Prefab Mode açılır
- SceneView'de 8 yellow handle drag çalışır
- Shape dropdown Box→Circle: collider type otomatik değişir, size→radius migrate
- Ctrl+S → prefab kaydet → Prefab Mode'dan çık → tüm scene instance refresh
- Statusbar "Editing prefab: statue_05" Prefab Mode'da görünür

## Output
- `STAGING/D4_COLLIDER_DRAG_HANDLE_DONE.md` — değişen dosyalar + verify checklist + compile durum

## Süre
~30-45 dk Sonnet bg. Background dispatch.

BLOCKED durumu: (a) PrefabStageUtility API Unity version uyumsuzluğu → fallback PrefabUtility.IsPartOfPrefabAsset check. (b) Collider migrate size/radius math edge case → conservative default (1×1 box / 0.5 radius circle / 1×1.5 capsule) kullan, log warning. (c) Mevcut drag handle logic Unity Handles API kullanmıyorsa partial impl YASAK → orchestrator'a flag.
