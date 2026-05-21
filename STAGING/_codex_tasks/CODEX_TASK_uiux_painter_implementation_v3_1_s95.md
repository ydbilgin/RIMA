# Codex Task — UIUX Painter Implementation v3.1 (S95)

> **Profile:** any active cx profile (Unity açık)
> **Effort:** high
> **Output:** `STAGING/CODEX_DONE_uiux_painter_implementation_v3_1_s95.md`
> **Source spec:** `STAGING/UIUX_PAINTER_DESIGN_DRAFT.md` (LIVE_WITH_MINOR_NOTES verdict)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Bağlam

UIUX DRAFT v3.1 spec **LIVE** verdict aldı, implementation ready. Antigravity'nin 3 hardcoded sortingLayer literal'ı (line 1481/1485, 2628, 2746) bu implementation'da CollisionResolver/GroupClassifier helper'larına delege ile subsume edilecek. Antigravity revision: Panel 5 implementation `UnityEditorInternal.EditMode.ChangeEditMode` API kullanmalı (`ToolManager.SetActiveTool` YASAK — internal/restricted).

## Görev — Full Implementation

`STAGING/UIUX_PAINTER_DESIGN_DRAFT.md`'i baştan sona oku. 5 panel + 4 helper class + 1 SO. Targeted edit `Assets/Editor/RimaUnifiedPainterWindow.cs`'e + 1 yeni SO dosyası.

### Yeni Dosyalar

1. `Assets/Editor/CollisionRulesSO.cs` — `[CreateAssetMenu]` SO class. Pattern → CollisionMode mapping. Antigravity revision'de path lock: SO asset `Assets/Editor/MapDesigner/Rules/DefaultCollisionRules.asset` (oluşturulacak default instance).
2. `Assets/Editor/MapDesigner/Rules/DefaultCollisionRules.asset` — default rules (wall_* → WallBlock, mounting_* → Passable, vb. spec'teki mevcut hardcoded mapping'lerden geçiş).

### Edit Edilecek Dosya

`Assets/Editor/RimaUnifiedPainterWindow.cs` (2812 satır):

#### Helper Class'lar (UIUX spec v3.1 v2 genel düzeltmeler bölümü)

- `GroupClassifier` — `CanonicalGroups[]` 6 sabit (Walls, Statues, WallMountings, Patches, Mobs, FloorProps), `Classify(prefabName, category)` method. `GetOrCreateGroupParent` (line 2290) buna delege.
- `CollisionResolver` — `Resolve(prefab, cat, mode, customSize, customOffset, scaleMult, rotationSteps, rulesSO)` static, `ResolvedCollider` struct döner. `ConfigureCollider` (line 1910) ve diğer 5 caller buna delege.
- `PeekTargetParent()` — read-only `GetTargetParent` versiyonu, Panel 4 banner için auto-create yapmaz, sadece "yerleşmiş olan ne" raporlar.

#### Panel 1 — Scene Organization (in-window)

Spec line 160+ takip. Layout: ProGroup foldout, 6 canonical group satır, count + visibility + lock + select-all buttons. `SceneVisibilityManager.Hide/Show/IsHidden` ile (`includeDescendants: true` named arg). `OnHierarchyChange` event-driven dirty flag.

#### Panel 2 — Collision Inspector (always-visible)

Spec line 219+ takip. Always-expand foldout. Auto mode'da `CollisionResolver.Resolve` dry-run preview "auto detected: WallBlock — wall_face_NS matched". Custom mode'da size/offset Vector2Field açık. `PreviewCacheKey` struct, MaterialPropertyBlock yerine cache.

**Scene gizmo:** Toggle button "Show colliders in scene", `OnSceneGUI` callback ile selected/hovered renderer'ın collider gizmo'sunu renkli çiz (WallBlock=red, Passable=green, SmallFootprint=yellow, FullFootprint=orange, Custom=cyan). Colorblind hardening: cross diagonals overlay.

#### Panel 3 — Palette Tile Redesign

Spec line 365+ takip. 110×130 px tile (92×110'dan büyütüldü), label wrap (`​` escape), badge sütunu collision mode (B/P/S/F/C colored letter + tooltip).

#### Panel 4 — Target Status Banner

Spec line 440+ takip. Header altı 3-axis state matrix (Tilemap / Parent / Biome). `PeekTargetParent()` ile auto-create göstermez. Eksik olanlarda warning, dolu olanlarda info.

#### Panel 5 — Selected Instance Editor (S95 LATE NIGHT eklenti)

Spec line 542+ takip (sonradan eklenen bölüm). `Selection.activeGameObject` izle, panel doldur:
- Object info: name, group, ping/focus buttons
- Collision per-instance: mode dropdown + size/offset
- **"Edit Collider in Scene" button:** `UnityEditorInternal.EditMode.ChangeEditMode(EditMode.SceneViewEditMode.Collider2D, collider.bounds, owner)` — Antigravity revision LOCK (ToolManager YASAK).
- Live reflection: scene'de drag sırasında size/offset değerleri panel'e canlı yansır (`EditorApplication.update` callback)
- Transform: rotation/scale tweak
- "Move to ▾" group dropdown — `Transform.SetParent(otherGroup, worldPositionStays: true)`
- Delete Instance: `Undo.DestroyObjectImmediate`

### Helper Class Caller Refactor (Karpathy #3 surgical)

6 caller site `CollisionResolver.Resolve`'a delege olur (1 line each):
1. `PaintPrefab` line 1453-1458 (placement)
2. `DrawPrefabOutline` line 1606-1617 (scene preview outline)
3. `ConfigureAssetPackColliders` line 1799-1801 (setup button line 704-709, bulk asset-pack bootstrap)
4. `PaintWallWithConnections` line 2613-2614 (wall paint)
5. `UpdateWallConnectionsAt` line 2730-2731 (wall autoconnect)
6. `LoadMapData` line 2460-2545 (collision mode serialize → deserialize)

### Sorting Layer Set (3 yer)

Antigravity'nin hardcoded literal'larını delete et:
- Line 1481, 1485 (PaintPrefab `"Walls"` / `"Entities"`)
- Line 2628 (PaintWallWithConnections `"Walls"` + sortingOrder=20)
- Line 2746 (UpdateWallConnectionsAt aynı)

Yerine: `CollisionResolver.Resolve` döndüğü `ResolvedCollider.layerName` + `sortingOrder` kullan.

## Verify

1. `dotnet build` 0 error
2. Unity Editor smoke test:
   - Painter window aç-kapat (GUIClip 0)
   - Sample prefab paint (sortingLayer Walls/Entities resolver'dan gelmeli, hardcoded değil)
   - Selected instance editor: GameObject seç → Panel 5 doldurulur
   - "Edit Collider in Scene" → BoxCollider2D scene view drag handles aktif
3. EditMode test (varsa): RoomFlowTests.cs PASS
4. Git diff scope: sadece listelenen dosyalar

## Output Format

```markdown
# UIUX Painter Implementation v3.1 — Codex Report

## Files Created
- Assets/Editor/CollisionRulesSO.cs (X lines)
- Assets/Editor/MapDesigner/Rules/DefaultCollisionRules.asset

## RimaUnifiedPainterWindow.cs Changes
- GroupClassifier nested class added (X lines)
- CollisionResolver nested class added (X lines)
- PeekTargetParent method added
- Panel 1 added (X lines)
- Panel 2 added/refactored (X lines)
- Panel 3 palette redesign (X lines)
- Panel 4 status banner (X lines)
- Panel 5 selected instance editor (X lines)
- 6 caller refactored to CollisionResolver.Resolve
- 3 hardcoded sortingLayer literal removed (lines 1481/1485/2628/2746)

## Verify
- dotnet build: 0 error
- Painter window smoke: PASS / FAIL
- EditMode test: X passed / Y total
- Git diff: X files, +Y -Z lines

## Open Questions Resolved
- Q1 SO path: Assets/Editor/MapDesigner/Rules/
- Q2-Q11: spec mapping uygulandı

## Açık Sorular (varsa)
- ...
```

## Hard Constraints

- Sadece listelenen dosyalar (RimaUnifiedPainterWindow.cs + 2 yeni dosya). Başka script/scene/prefab/asset YASAK.
- Auto-commit YOK.
- Karpathy #3 cerrahi — refactor sınırı 6 caller delegasyonu.
- BLOCKED if unclear: ToolManager API confusion (Antigravity revision EditMode.ChangeEditMode kullan), EditorTools namespace ambiguity vb.
- Spec'in tam takibi — sapma yok.
