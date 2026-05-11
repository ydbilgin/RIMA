# Codex Task — Room Designer F1 BRUSH

**Status:** WAIT for SKELETON commit (depends on `IRoomDesignerContext`)
**Branch:** master
**Estimated:** ~700-1000 LOC
**Allowed paths:**
- `Assets/Editor/RoomDesigner/Brushes/**`
- `Assets/Editor/RoomDesigner/UI/RoomDesignerWindow.uss` (sadece `.rd-brush-toolbar` selector'ları ekle)

## Hedef

4 brush mode: **Stamp / Eraser / Picker / Bucket Fill**. Multi-tilemap undo grouping (tek brush stroke = tek undo step). 60fps performance gate (1000+ cell drag senaryosunda canvas dirty bayrağı doğru tetikleniyor).

## Bağımlılık (LOCKED)

- `IRoomDesignerContext` SKELETON commit sonrası
- `ctx.InvokeBrush(mouseButton, cell)` Canvas'tan çağrılıyor — `BrushController.OnInvoke` olarak dinle
- `ctx.ActiveTile` Palette'ten geliyor
- `ctx.ActiveBrush` BrushController state'i — toolbar UI ile değişiyor

## Yapılacak Dosyalar

### 1. `Assets/Editor/RoomDesigner/Brushes/IBrush.cs` (~40 LOC)
```csharp
public interface IBrush
{
    BrushMode Mode { get; }
    void OnStrokeBegin(IRoomDesignerContext ctx, Vector3Int cell, int mouseButton);
    void OnStrokeContinue(IRoomDesignerContext ctx, Vector3Int cell);
    void OnStrokeEnd(IRoomDesignerContext ctx);
}

public readonly struct CellEdit
{
    public readonly Tilemap Target;
    public readonly Vector3Int Cell;
    public readonly TileBase Tile;
    public CellEdit(Tilemap t, Vector3Int c, TileBase tile) { Target = t; Cell = c; Tile = tile; }
}
```

### 2. `Assets/Editor/RoomDesigner/Brushes/BrushController.cs` (~250 LOC)
- Active brush state machine: stroke begin/continue/end lifecycle
- Mouse down → `OnStrokeBegin` (undo group başlat)
- Mouse drag → `OnStrokeContinue` (aynı group'a ekle)
- Mouse up → `OnStrokeEnd` (group collapse + repaint)
- Toolbar UI: 4 button (Stamp / Eraser / Picker / Bucket) — `ctx.RightPanel`'e mount
- Hotkey: B=Stamp, E=Eraser, I=Picker, G=Bucket (window focus iken)

**Multi-tilemap undo (Codex review Soru 5 pattern):**
```csharp
public void ApplyStroke(IRoomDesignerContext ctx, IList<CellEdit> edits, string strokeName)
{
    if (edits.Count == 0) return;
    Undo.IncrementCurrentGroup();
    int group = Undo.GetCurrentGroup();
    Undo.SetCurrentGroupName(strokeName);
    var maps = edits.Select(e => e.Target).Distinct().Cast<Object>().ToArray();
    Undo.RegisterCompleteObjectUndo(maps, strokeName);
    foreach (var e in edits) e.Target.SetTile(e.Cell, e.Tile);
    foreach (var m in maps.OfType<Tilemap>()) m.RefreshAllTiles();
    Undo.CollapseUndoOperations(group);
    ctx.MarkDirty();
}
```

Drag senaryosunda **mouse down/up = bir stroke**. Move event'leri stroke'un içinde — her move yeni group açma. Stroke buffer:
- `OnStrokeBegin` → buffer init
- `OnStrokeContinue` → cell'i buffer'a ekle (duplicate check), `ctx.MarkDirty()` ile preview repaint
- `OnStrokeEnd` → `ApplyStroke` ile tek seferde commit

### 3. `Assets/Editor/RoomDesigner/Brushes/StampBrush.cs` (~80 LOC)
- `OnStrokeContinue` cell'e `ctx.ActiveTile`'i ekle (active layer tilemap'ine)
- Aynı cell'e tekrar yazma (buffer'da varsa atla)

### 4. `Assets/Editor/RoomDesigner/Brushes/EraserBrush.cs` (~60 LOC)
- `OnStrokeContinue` cell'e `null` yaz (active layer tilemap'inde)

### 5. `Assets/Editor/RoomDesigner/Brushes/PickerBrush.cs` (~60 LOC)
- Click → cell'deki mevcut tile'ı oku → `ctx.ActiveTile = tile`, `ctx.ActiveBrush = Stamp` (otomatik geri stamp'a dön)
- Drag yok — sadece click

### 6. `Assets/Editor/RoomDesigner/Brushes/BucketFillBrush.cs` (~150 LOC)
- 4-yönlü flood fill (active tilemap, hedef tile = clicked cell'in mevcut tile'ı, replace = `ctx.ActiveTile`)
- Performans: BFS queue, visited HashSet, max cell limit (örn 10000) — limit aşılırsa abort + EditorUtility.DisplayDialog uyarı
- Aynı tile zaten orada ise no-op

### 7. USS değişiklikleri

```css
.rd-brush-toolbar { flex-direction: row; padding: 4px; background-color: #1A1C20; }
.rd-brush-button { width: 32px; height: 32px; margin: 2px; }
.rd-brush-button--active { background-color: #7BA7BC; }
```

## Performans Gate

- Drag 1000 cell senaryosu: ortalama frame time 16ms altında kalmalı (60fps+)
- `MarkDirty` debounce: 32ms'den hızlı tekrar repaint isteme — `EditorApplication.timeSinceStartup` ile son repaint zamanını sakla
- Buffer'da duplicate cell yazımı yok (HashSet)

## Acceptance Criteria

- [ ] 4 brush mode toolbar'dan switch'leniyor
- [ ] Stamp drag: 60+ cell tek undo step (Ctrl+Z hepsini geri alır)
- [ ] Eraser drag: aynı pattern, tek undo
- [ ] Picker click → ActiveTile değişir, mode otomatik Stamp'a döner
- [ ] Bucket fill: bağlı bölgeyi doldurur, 10000+ cell durumunda abort uyarı verir
- [ ] Drag 1000+ cell senaryosunda canvas 60fps+
- [ ] Undo/Redo Ctrl+Z / Ctrl+Y çalışıyor
- [ ] Compile error yok

## CODEX_DISPATCH Global Kurallar

- Model: **gpt-5.5**
- Yorum yazma — WHY açık değilse istisna
- Test: `Assets/Tests/EditMode/Editor/BrushTests.cs`:
  - StampBrush stroke happy path → tilemap'te tile var
  - EraserBrush stroke → tile null
  - BucketFillBrush küçük 4×4 alanda doğru fill, 1000+ cell limit testi
  - Multi-tilemap undo: 3 farklı tilemap'e yazıp tek Ctrl+Z senaryosu (`Undo.PerformUndo()`)

## Güven Döngüsü

1. "%100 güven var mı?"
2. Açıklar → düzelt
3. %100 güven → commit

## Commit Message

```
feat(room-designer): F1 brush — Stamp/Eraser/Picker/Bucket + multi-tilemap undo grouping

- IBrush + CellEdit struct
- BrushController (stroke lifecycle, undo grouping, hotkeys B/E/I/G)
- StampBrush + EraserBrush + PickerBrush + BucketFillBrush
- USS brush toolbar selectors
- EditMode tests (stroke happy path + bucket fill + undo grouping)
```

## Kaynak

- `STAGING/roomdesigner_codex_review.md` — Soru 5 (multi-tilemap undo pattern)
- `MEMORY/project_room_designer_plan.md`
- SKELETON commit'i — `IRoomDesignerContext` API sözleşmesi
