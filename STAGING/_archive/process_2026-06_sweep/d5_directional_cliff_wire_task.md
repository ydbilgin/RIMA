# D5: DirectionalCliffTile Full Wire + Cliff Mode UI

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: gerekirse `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"`

## Amaç
Cliff base variant rotation aktivasyonu + Cliff mode UI tam çalışır hale getirme. D3 mode tabs LIVE, ama Cliff mode sub-content tam değil. D5 cliff variant dropdown + hover indicator + erase/regenerate hotkey ekler.

**ÖNEMLİ TERİM AYIRMA:**
- **mounting_*.prefab** = L3 Cliff face DECOR (15 prefab, top-center pivot, sortingOrder 50). Decor mode'da palette'ten seçilip yerleştirilir (D3 ile LIVE). D5 SCOPE DIŞI.
- **cliff_S_new1..new4.png** = L2 Cliff base sprite variant. DirectionalCliffTile_Hades.spritesS[] array'inde dolacak. D5 SCOPE.
- Bu ikisi farklı render contract (Tilemap batch vs GameObject Y-sort).

## Bağlam (D2+D3+D4 LIVE)
- D2: CliffAutoPlacer.cliffTile → DirectionalCliffTile_Hades.asset (zaten doğru), 6-layer arch + 33 prefab backfill
- D3: RoomPainter 4 mode tab (Tile/Cliff/Decor/Object) + L1-L6 sub-filter
- D4: ColliderShapeSwapper + Prefab Mode collider authoring
- DirectionalCliffTile.cs: 8-direction Sprite[] array, deterministic per-cell hash, ZATEN LIVE
- DirectionalCliffTile_Hades.asset: 8 direction × 1 sprite each (variant yok henüz)
- `Assets/Sprites/Environment/KitB_Cliff/cliff_S_new1.png..new4.png` — UNUSED 4 South variant disk'te

## İş kalemleri

### 1. DirectionalCliffTile_Hades.asset sprite array populate
- File: `Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset`
- South direction array (`spritesS[]`): mevcut 1 sprite + 4 new variant = 5 toplam
  - `Assets/Sprites/Environment/KitB_Cliff/cliff_S.png` (mevcut, index 0)
  - `cliff_S_new1.png` (index 1)
  - `cliff_S_new2.png` (index 2)
  - `cliff_S_new3.png` (index 3)
  - `cliff_S_new4.png` (index 4)
- Diğer 7 direction (N/E/W/NE/NW/SE/SW): yeni variant yok → tek sprite kalır
- Programmatic: SerializedObject API ile asset modify, AssetDatabase.SaveAssets

### 2. Cliff mode SceneView hover indicator
- File: `Assets/Editor/RoomPainter/SceneAuthoring/` (yeni veya extend)
- Cliff mode aktifken (`_currentMode == Cliff`):
  - SceneView mouse hover position → cell coordinate (CliffTilemap.WorldToCell)
  - Cell durumu tespit:
    - `ManualOverrideCells` set'inde → kırmızı outline (erased)
    - `ManualPaintedCells` set'inde → yeşil outline (painted)
    - `cliffTilemap.HasTile(cell)` && yukarıdaki ikisinde değil → gri outline (auto)
    - Boş cell → transparan (no indicator)
- Handles.DrawSolidRectangleWithOutline veya Handles.DrawAAPolyLine — iso diamond shape
- ~80 LOC

### 3. Alt+Click erase hotkey
- File: `Assets/Editor/MapDesigner/VisualEditor/VisualEditorScenePainter.cs` (line 546 PAINT hook'a paralel ERASE hook)
- Cliff mode + Alt+Left-Click event:
  - Cell coordinate hesapla
  - `cliffAutoPlacer.AddManualOverride(cell)` çağır
  - `cliffTilemap.SetTile(cell, null)` (görsel anlık silinir)
  - `cliffAutoPlacer.RemoveManualPainted(cell)` (whitelist'ten de sil)
  - SceneView.RepaintAll
- Statusbar "Erased: N" counter increment
- ~40 LOC

### 4. C hotkey Regenerate
- File: `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` (D3'te keyDown handle var)
- Cliff mode aktifken C tuşu:
  - CliffAutoPlacer instance bul (sahnedeki ilk active)
  - `Regenerate()` çağır
  - Statusbar "Cliff regenerated: <count> tiles"
- ~30 LOC

### 5. Cliff mode Inspector sub-section
- File: `Assets/Editor/RoomPainter/Inspector/RoomPainterInspectorPanel.cs` (D3'te mode-specific section eklenmiş)
- Cliff mode section content:
  - **"Active Cliff Variant" dropdown:** DirectionalCliffTile_Hades.spritesS[] preview thumbnails (5 entry), seçili index storage
  - **"Manual painted: N" label** (live count from `cliffAutoPlacer.ManualPaintedCells.Count`)
  - **"Manual erased: N" label** (live count from `cliffAutoPlacer.ManualOverrideCells.Count`)
  - **"Clear Manual Painted" button:** confirmation dialog + ClearManualPainted() çağrı
  - **"Clear Manual Override" button:** confirmation dialog + ClearManualOverrides() çağrı
  - **"Regenerate (C)" button:** Regenerate() çağrı
- ~80 LOC

## Dosyalar (scope)
- `Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset` (asset modify, programmatic)
- `Assets/Editor/RoomPainter/SceneAuthoring/CliffHoverIndicator.cs` (NEW ~80 LOC) VEYA existing collider editor'a extend
- `Assets/Editor/MapDesigner/VisualEditor/VisualEditorScenePainter.cs` (EXTEND ~40 LOC ERASE hook)
- `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` (EXTEND ~30 LOC C hotkey)
- `Assets/Editor/RoomPainter/Inspector/RoomPainterInspectorPanel.cs` (EXTEND ~80 LOC Cliff section content)
- Toplam ~230 LOC

## YASAK
- mounting_*.prefab DirectionalCliffTile sprite array'ine YAZMA (L3 cliff face decor, L2 cliff base'den farklı render contract)
- Yeni Cliff variant ARRAY DIRECTION (N/E/W) — sadece South 5 variant, diğer direction tek sprite kalır
- CliffAutoPlacer.cs ALGORITHM değişiklik (sadece manual override/painted hook'larını kullan)
- D3 mode tab yapısı bozma
- D4 Prefab Mode collider authoring bozma
- Yeni .cs → `refresh_unity scope=all mode=force` ZORUNLU
- Input.GetKey* YASAK → InputSystem.Keyboard.current

## Verify
- UnityMCP: `refresh_unity scope=all mode=force` + `read_console` → 0 error / 0 warning
- DirectionalCliffTile_Hades.asset Inspector → spritesS[] array 5 sprite gösterir
- PlayMode aç → cliff cell'leri her cell hash'e göre farklı variant (variant rotation aktif)
- RoomPainter aç → Cliff mode (hotkey 3)
- SceneView'de cliff cell hover → cell outline rengi durumuna göre (kırmızı/yeşil/gri)
- Alt+Click cliff cell → silinir, statusbar "Erased: 1"
- C tuşu → Regenerate çağrı, statusbar count update
- Inspector pane'de cliff section: 5 variant thumbnail, painted/erased count, 3 button çalışır

## Output
- `STAGING/D5_DIRECTIONAL_CLIFF_WIRE_DONE.md` — değişen dosyalar + verify checklist + compile durum + variant rotation görsel test sonucu

## Süre
~45-60 dk Sonnet bg. Background dispatch.

BLOCKED durumu: (a) DirectionalCliffTile.cs script direction lookup logic anlaşılmıyor → CliffAutoPlacer.cs ile uyumlu yön mapping varsayım. (b) VisualEditorScenePainter.cs ERASE hook line 546 PAINT hook'una simetrik nokta bilinmiyor → grep + read + simetri pattern uygula. (c) Programmatic asset modify Unity import-time race → AssetDatabase.StartAssetEditing + SaveAssets + StopAssetEditing wrap.
