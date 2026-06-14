ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

**Output dosyası:** `CODEX_DONE_room_painter_day4_patch.md` (max 500 kelime)

---

# Amaç

Phase A Day 4 patch — Sonnet Day 4 review'undan 3 HIGH + 5 MED fix. Day 5 başlamadan önce zorunlu.

## HIGH fixes — 3 adet, zorunlu

### H1 — Double-trigger anti-pattern (RoomPainterAssetPostprocessor.cs)

**Sorun:** Codex hem `EditorApplication.projectChanged` hem 2-saniyelik `PollMissingMetadata` poll wire'lemiş. Memory `feedback_double_auto_regen_avoid.md` der ki "tek event source". Şu an 3 path: OnPostprocessAllAssets + projectChanged + 2sn poll.

**Fix:**
- `EditorApplication.update` üzerindeki `PollMissingMetadata` subscription'ını **tamamen sil**
- `OnPostprocessAllAssets` (Postprocessor entry point) + `projectChanged` (manual file ops için) **sadece bu iki path**
- Eğer `projectChanged` + `OnPostprocessAllAssets` çakışıyorsa, projectChanged içinde `_isFlushing` flag ile çift-flush'ı engelle

### H2 — Keyword substring false positives (RoomPainterPhysicsRules.cs:73)

**Sorun:** `lowerPath.IndexOf("wall")` → `Assets/Sprites/Walless/floor.png` veya `Pillars/decal_pillar_shadow.png` yanlış match. Folder name veya parent path keyword içerirse decal block oluyor.

**Fix:**
```csharp
// Eski:
// string lowerPath = assetPath.ToLowerInvariant();
// if (lowerPath.IndexOf(kw, ...) >= 0) ...

// Yeni — sadece filename stem match:
string filename = System.IO.Path.GetFileNameWithoutExtension(assetPath).ToLowerInvariant();
if (filename.IndexOf(kw, StringComparison.Ordinal) >= 0) ...
```
Tüm `Resolve` ve keyword scan path'lerinde `filename` kullan, `assetPath` veya `lowerPath` kullanma.

### H3 — Inspector undo stack pollution (RoomPainterInspectorPanel.cs:31)

**Sorun:** `Undo.RecordObject` ve `AssetDatabase.SaveAssets()` her OnGUI repaint'te çağrılıyor — undo stack ve disk thrash.

**Fix:**
```csharp
EditorGUI.BeginChangeCheck();
// section drawing here
if (EditorGUI.EndChangeCheck())
{
    Undo.RecordObject(activeAsset, "Edit Room Painter Asset");
    EditorUtility.SetDirty(activeAsset);
    AssetDatabase.SaveAssetIfDirty(activeAsset);
}
```
`SaveAssets()` global çağrıyı `SaveAssetIfDirty(activeAsset)` per-asset ile değiştir.

## MED fixes — 5 adet, zorunlu

### M1 — ySortAxis per-asset (RoomPainterAsset.cs)
- `RoomPainterAsset`'a `YSortAxis ySortAxisOverride = YSortAxis.UseLayerDefault;` field ekle
- `YSortAxis` enum'a `UseLayerDefault` value ekle (RoomLayerData.cs)
- PlacementSection bu field'ı edit etsin

### M2 — Parallax SO-persistence (ParallaxSection.cs:24-38)
- EditorPrefs yerine `activeAsset.parallaxFactor`, `cameraRelative`, `pixelSnap`, `parallaxTier` fields oku/yaz
- RoomPainterAsset.cs'e bu 4 field ekle
- Per-asset override mantığı: eğer override set'lenmemişse (varsayılan değer 0) layer default'ı kullan

### M3 — VisualSection MaterialPropertyBlock (VisualSection.cs:47)
- `sharedMaterial` atama kalır (URP 2D Lit/Unlit shader değişimi için)
- TINT için: `MaterialPropertyBlock block = new MaterialPropertyBlock(); block.SetColor("_Color", tint); renderer.SetPropertyBlock(block);`
- Per-instance tint GC leak'i önle

### M4 — PhysicsApplier prefab collider preserve (RoomPainterPhysicsApplier.cs:17-20)
- `!isBlock && !isTrigger` durumunda mevcut tüm Collider2D'leri silme
- Sadece **bu Applier'ın eklediği** collider'ı sil: collider'ı eklerken `[AddedByRoomPainter] tag` veya `RoomPainterAssetBinding`'in collider GUID listesinde tut
- Veya basit yol: `metadata.respectPrefabColliders` toggle (default true) — true ise mevcut collider'lara dokunma

### M5 — Postprocessor recursive import (RoomPainterAssetPostprocessor.cs:450)
- `OnPostprocessTexture` içinden `importer.SaveAndReimport()` çağırma — Unity recursive import warning + potansiyel hang
- `EnsureSpriteImporter` çağrısını **`delayCall` queue**'sine taşı, OnPostprocessTexture içinde değil

## LOW — 2 adet quick fix (3-5 kaldı pas geç)

### L3 — PhysicsSection Trigger toggle disabled lock-out
- `PhysicsSection.cs:18` Trigger toggle DisabledScope dışına çıkar — kullanıcı Trigger açtıktan sonra Block kapatınca Trigger'ı tekrar kapatabilmeli

### L5 — IdentitySection async preview repaint guard
- `AssetPreview.IsLoadingAssetPreview(asset)` ise `EditorApplication.update` ile repaint poll kur, preview gelince refresh

LOW L1, L2, L4 atla — Day 5 sonrası polish'e it.

## Bonus (opsiyonel, time permits)

### B1 — RoomPainterAssetBindingPostBuildStripper
- `Assets/Editor/RoomPainter/RoomPainterAssetBindingPostBuildStripper.cs` yeni
- `IPreprocessBuildWithReport` implement
- Build sırasında `RoomPainterAssetBinding` component'lerini stripple
- 15-20 LOC

Time yoksa skip — Day 5 sonrası ekle.

## Verification

1. `grep -n "EditorApplication.update\|PollMissingMetadata" Assets/Editor/RoomPainter/AssetPipeline/RoomPainterAssetPostprocessor.cs` → 0 hit (H1 fix)
2. `grep -n "GetFileNameWithoutExtension" Assets/Editor/RoomPainter/AssetPipeline/RoomPainterPhysicsRules.cs` → en az 1 hit (H2 fix)
3. `grep -n "EndChangeCheck\|SaveAssetIfDirty" Assets/Editor/RoomPainter/Inspector/RoomPainterInspectorPanel.cs` → en az 2 hit (H3 fix)
4. `grep -n "ySortAxisOverride\|UseLayerDefault" Assets/Scripts/RoomPainter/` → en az 2 hit (M1 fix)
5. `grep -n "parallaxFactor\|parallaxTier\|cameraRelative\|pixelSnap" Assets/Scripts/RoomPainter/RoomPainterAsset.cs` → en az 4 hit (M2 fix)
6. `grep -n "MaterialPropertyBlock" Assets/Editor/RoomPainter/Inspector/Sections/VisualSection.cs` → en az 1 hit (M3 fix)
7. `grep -n "respectPrefabColliders\|AddedByRoomPainter" Assets/Editor/RoomPainter/` → en az 1 hit (M4 fix)
8. Unity compile error 0

## Yapma

- Day 5 features (Erase/Pick/Box-select/Drag-drop) **YOK**
- Save/Load RoomData **YOK** (Day 6)
- Mevcut RimaRoomPainterWindow, RoomPainterScenePlacer, RoomPainterAssetBinding **dokunma** (Sonnet linter zaten edit etti)
- Mevcut PainterSuite package'a dokunma
- Cliff sahne state'i (auto LIVE 311 tile) dokunma

## Çıktı

`CODEX_DONE_room_painter_day4_patch.md` — 10 fix listesi (H1/H2/H3 + M1-M5 + L3/L5 + opsiyonel B1) + her birinin dosya:satır + compile durumu + grep çıktıları.

**Day 5 başlamadan önce H1/H2/H3 zorunlu.** Diğerleri quality of life.
