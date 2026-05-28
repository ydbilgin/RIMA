# D5.5: Cliff 2-Aşama Ayırma + Orphan Cleanup

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: gerekirse `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"`

## Amaç (verbatim user direktifi 2026-05-27 gece, screenshot ile)
"Cliffleri 2 aşamaya ayır. Önce tile altında olan cliffler (generate ile otomatik gelen) ve tile'sız cliffler. Ben 'cliffleri temizle generate et' dediğimde sadece tile altına mantıksal yerleştirdiğimiz gelmeli."

Şu an `PlayableArena_Test01` sahnesinde havada (floor neighbor OLMAYAN) cliff sprite'lar var. Bunlar `ManualPaintedCells` whitelist'inde duruyor → Regenerate her seferinde geri koyuyor. Kullanıcı 2 ayrı tilemap istiyor.

## Bağlam (D2-D5 LIVE)
- D2: `CliffAutoPlacer.cliffTile` → `DirectionalCliffTile_Hades.asset`, 6-layer arch LOCK, 33 prefab backfill
- D3: RoomPainter 4 mode tab + L1-L6 sub-filter
- D4: ColliderShapeSwapper + Prefab Mode collider authoring
- D5: DirectionalCliffTile_Hades sprite arrays populate + Cliff mode UI (hover indicator + Alt+click erase + C regenerate + Inspector cliff section)
- `Assets/Scripts/Environment/CliffAutoPlacer.cs` algoritma: floor cell'in S/SE/SW komşusu void → floor cell'in kendisine cliff
- Manuel override 2-katmanı: `ManualOverrideCells` (blacklist) + `ManualPaintedCells` (whitelist)

## Sorun (root cause)
1. **Eski sahnede manuel paint kalıntıları** `ManualPaintedCells` whitelist'inde
2. **CliffAutoPlacer.Regenerate()** line 154 `targets.UnionWith(ManualPaintedCells)` → orphan cell'ler geri geliyor
3. **Tek tilemap** sistem → algorithmic + manuel ayırt edilemiyor

## İş kalemleri

### 1. Sahne tarama + orphan tespit (analiz, kod öncesi)
- UnityMCP execute_code ile `PlayableArena_Test01.unity` CliffTilemap içeriği tara
- Her cliff cell için floor neighbor check (CliffAutoPlacer logic ile tutarlı: S/SE/SW komşu floor mu)
- "Orphan" tanımı: cliff cell var ama floor cell sıfır komşu (yani havada)
- Count + cell coordinate listesi rapor: `STAGING/D5_5_orphan_cliff_inventory.md`

### 2. Yeni `DecorCliffTilemap` GameObject + altyapı
- Sahnede `Grid` root altına yeni `Tilemap` child: `DecorCliffTilemap`
- TilemapRenderer settings:
  - sortingLayerName = `Decor_Cliff` (D2'de eklendi, order 50)
  - Material = mevcut CliffTilemap material kopya
- Composite collider YOK (decor cliff hitbox yok)
- Inspector'da görünür, prefab değil scene-bound

### 3. CliffAutoPlacer.cs minimal refactor
- File: `Assets/Scripts/Environment/CliffAutoPlacer.cs`
- Mevcut `cliffTilemap` field korunur (algorithmic için)
- YENİ field eklenmez (decor tilemap ayrı sistem, CliffAutoPlacer'a referans gerek yok)
- **Validation strict**: `Regenerate()` içinde `ManualPaintedCells` cell'lerinden floor neighbor OLMAYAN olanları **OTOMATİK silsin** ve `RemoveManualPainted` çağırsın
- Yeni method: `ValidateManualPainted()` — orphan whitelist entry'lerini temizler
- Regenerate akışı:
  ```
  if (clearExistingOnRegenerate) cliffTilemap.ClearAllTiles()
  ValidateManualPainted()  // YENİ — orphan whitelist temizliği
  targets = CollectCliffCells()
  targets.ExceptWith(ManualOverrideCells)
  targets.UnionWith(ManualPaintedCells)  // bu noktada whitelist temiz
  foreach: SetTile
  ```
- ~30 LOC ekleme

### 4. Yeni `DecorCliffPainter.cs` (free-form tilemap painter)
- File: `Assets/Editor/RoomPainter/SceneAuthoring/DecorCliffPainter.cs` (NEW ~80 LOC)
- Cliff mode + Shift+Click → DecorCliffTilemap'e sprite paint (free-form, floor check YOK)
- Cliff mode + Shift+Alt+Click → DecorCliffTilemap erase
- VisualEditorScenePainter'a paralel ayrı painter
- Cliff variant seçimi RoomPainter Inspector dropdown'dan
- Statusbar "Decor cliff painted: N" counter

### 5. Cliff mode UI ek section (Inspector pane)
- File: `Assets/Editor/RoomPainter/Inspector/RoomPainterInspectorPanel.cs` (EXTEND D5'in cliff section'ına)
- Yeni alt-section: "Decor Cliff (Free-form)"
- Display: "Decor cliff tile count: N" (DecorCliffTilemap.GetUsedTilesCount)
- Button "Clear Decor Cliff" (sadece DecorCliffTilemap.ClearAllTiles)
- Mevcut "Regenerate (C)" / "Clear Manual Painted" / "Clear Manual Override" sadece algorithmic etkiler — disclaimer label ekle

### 6. RoomPainterWindow hotkey extend
- Cliff mode + Shift hold → DecorCliffPainter aktif (overlay status "Free-form Decor mode" cyan label)
- Cliff mode normal (Shift YOK) → mevcut CliffAutoPlacer behavior

### 7. Orphan cleanup migration (1-shot)
- Script: `ValidateManualPainted()` ilk çağrı tüm orphan'ları temizler
- Migration log: cell coord listesi `STAGING/D5_5_orphan_cleanup_log.md`
- Kullanıcı kararı: temizlenen orphan'lar DecorCliffTilemap'e taşınsın mı yoksa silinsin mi?
  - **Default (öneri):** Sil. Kullanıcı bilinçli decor cliff istiyorsa Shift+Click ile yeniden çizer.
  - Alternatif: orphan'ları DecorCliffTilemap'e taşı (data preservation).
  - **Migration choice:** İlk seçenek (Sil) default, hot fix. Alternatif kullanıcı geri requestlerse.

## Dosyalar (scope)
- `Assets/Scripts/Environment/CliffAutoPlacer.cs` (EXTEND ~30 LOC ValidateManualPainted method)
- `Assets/Editor/RoomPainter/SceneAuthoring/DecorCliffPainter.cs` (NEW ~80 LOC)
- `Assets/Editor/RoomPainter/Inspector/RoomPainterInspectorPanel.cs` (EXTEND ~50 LOC Decor Cliff section)
- `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` (EXTEND ~20 LOC Shift hotkey detect)
- `Assets/Scenes/Test/PlayableArena_Test01.unity` (sadece DecorCliffTilemap GameObject ekle, mevcut CliffTilemap dokunulmaz)
- Toplam ~180 LOC + 1 scene wire

## YASAK
- Mevcut CliffTilemap (algorithmic) cell'lerine dokunma — orphan değilse korunur
- `CliffAutoPlacer.Regenerate()` core algorithm değişiklik (sadece ValidateManualPainted çağrısı eklendi)
- DecorCliffTilemap'e CliffAutoPlacer hook (bağımsız sistem, ayrı painter)
- Composite collider DecorCliffTilemap'e (decor, hitbox yok)
- D2/D3/D4/D5 LIVE özelliklerini bozma
- Yeni .cs → `refresh_unity scope=all mode=force` ZORUNLU
- Input.GetKey* YASAK → InputSystem.Keyboard.current

## Verify
- UnityMCP: `refresh_unity scope=all mode=force` + `read_console` → 0 error / 0 warning
- PlayableArena_Test01 aç → orphan cliff'ler kaybolmuş (D5_5_orphan_cleanup_log.md count = silinen)
- Beyaz çerçeve bölgesi (floor altı cliff'ler) korunur — algorithmic LIVE
- RoomPainter Cliff mode → Regenerate (C) → sadece floor altı cliff geri gelir, orphan gelmez
- Shift+Click (RoomPainter Cliff mode aktifken) → DecorCliffTilemap'e free-form sprite paint
- Inspector "Decor Cliff" section: count, Clear button çalışır
- DecorCliffTilemap sortingOrder 50, Decor_Cliff layer

## Output
- `STAGING/D5_5_orphan_cliff_inventory.md` — sahnedeki orphan cell coord listesi (cleanup öncesi)
- `STAGING/D5_5_orphan_cleanup_log.md` — silinen cell coord listesi (cleanup sonrası)
- `STAGING/D5_5_CLIFF_2STAGE_DONE.md` — değişen dosyalar + verify + compile durum + screenshot path

## Süre
~45-60 dk Sonnet bg.

BLOCKED durumu: (a) Mevcut sahnede `ManualPaintedCells` count çok yüksek (>500) ve cleanup destructive → kullanıcı onayı için orchestrator'a flag. (b) DecorCliffTilemap için TilemapRenderer settings (material, blend mode) bilinmiyor → mevcut CliffTilemap'ten kopya, fork yapma. (c) Shift hotkey D3 mode tab keyDown logic ile çakışma → modifier check (Event.current.shift) safe.
