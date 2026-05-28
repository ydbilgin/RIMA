ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

**Output dosyası:** `CODEX_DONE_room_painter_day2.md` (kısa, max 400 kelime)
**Code dosyaları:** doğrudan `Assets/Editor/RoomPainter/` altına yaz/düzenle

---

# Amaç

Phase A Day 2 — Asset Palette Panel implement. Day 1 skeleton LIVE (`RimaRoomPainterWindow.cs` 102 LOC).

## Mevcut state (Day 1 LIVE)

- Window: `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` (3-pane stub)
- SO: `RoomPainterAsset`, `RoomLayerData`, `RoomData` (`Assets/Scripts/RoomPainter/`)
- Enum: `RoomLayer` (10 değer: Floor, Edge, Cliff, Wall, Props, Decals, Lighting, Collision, Occlusion, Parallax)
- Compile clean
- Menu: `RIMA > Room Painter` LIVE

## Görev — Asset Palette panel implement

### 1. Folder scan helper
**Path:** `Assets/Editor/RoomPainter/Helpers/RoomPainterAssetScanner.cs`

- Static class `RoomPainterAssetScanner`
- Method: `List<AssetEntry> Scan(string folderPath, string[] categoryFilters = null)`
- AssetEntry struct: `string path, Sprite sprite, GameObject prefab, RoomLayer suggestedLayer, string category`
- Use `AssetDatabase.FindAssets("t:Sprite", new[]{folderPath})` + `AssetDatabase.FindAssets("t:Prefab", new[]{folderPath})`
- `suggestedLayer` inferred from folder name keywords: "cliff" → RoomLayer.Cliff, "floor" → RoomLayer.Floor, "prop" → RoomLayer.Props, "parallax"/"bg" → RoomLayer.Parallax, default → Floor
- `category` = parent folder name

### 2. Asset palette UI in RimaRoomPainterWindow
**Path:** `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` (Day 1 stub'ı upgrade)

UI:
- Top toolbar: "Folder:" + EditorGUILayout.TextField (default = `Assets/Sprites/Environment`) + "Refresh" button
- Below toolbar: category filter chips (yan yana button group: All / Floor / Cliff / Props / Parallax / All others)
- Main palette area: scrollview with **grid of thumbnails (4 columns)** using `AssetPreview.GetAssetPreview` per sprite/prefab
- Each thumbnail: 64×64 button, hover shows asset name, click selects (highlight selected with cyan tint)
- Selected asset: stored in `_selectedAsset` private field (AssetEntry)
- Bottom status bar: "Selected: <name> [<layer>]" or "No selection"
- Right side panel placeholder for "Day 3 — inspector" (bırak boş 200px width)

### 3. Layer selector (Cliff/Parallax depth chooser hazırlık)
Top toolbar'a EKLE:
- Layer dropdown: `EditorGUILayout.EnumPopup("Target Layer:", _targetLayer)`
- Override: eğer asset'in `suggestedLayer != _targetLayer` ise warning ikonu göster
- Bu Day 2 için scaffold — Day 3'te SceneView placement'ta kullanılacak

### 4. Selection state
- `_selectedAsset` field — currently selected AssetEntry
- `_targetLayer` field — current target RoomLayer (default RoomLayer.Floor)
- `_filterCategory` field — current category filter ("" = All)
- `_folderPath` field — current scan folder
- `_assetCache` field — List<AssetEntry>, refreshed on "Refresh" button or folder change
- OnEnable: initial scan
- All state preserved via `[SerializeField]` for window restore

### 5. Helper subfolder asmdef coverage
- Eğer `Helpers/` subfolder asmdef'i koparıyorsa, mevcut `RIMA.RoomPainter.Editor.asmdef` zaten parent folder'da, otomatik kapsam alır. Test et: compile clean kalmalı.

## Yapma

- SceneView placement YOK (Day 3)
- Drag-drop asset YOK (Day 2 sadece folder scan + browse + select)
- Save/Load RoomData YOK (Day 5)
- VirtualizedScrollView (lazy load) YOK — basit `BeginScrollView` yeterli
- Inspector tarafı doldurma YOK — sadece placeholder

## Verification

1. `grep -n "AssetPreview.GetAssetPreview\|RoomPainterAssetScanner" Assets/Editor/RoomPainter/` → en az 3 hit
2. `grep -n "_selectedAsset\|_targetLayer\|_filterCategory" Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` → 4+ hit
3. Unity console compile error 0 (orchestrator verify edecek)
4. Menu `RIMA > Room Painter` → window açılır, palette görünür, `Assets/Sprites/Environment` altındaki sprite'lar grid'de listelenir

## Çıktı

`CODEX_DONE_room_painter_day2.md` — dosyalar + LOC + grep çıktıları + compile durumu.
