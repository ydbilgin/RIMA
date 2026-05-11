# Codex Task — Room Designer F1.1 FIX

**Status:** READY FOR DISPATCH (post Opus + rima-qc Gemini cross-review, both NEEDS_FIX)
**Branch:** master
**Estimated:** ~400-600 LOC (mostly small surgical edits + new integration test)
**Allowed paths:**
- `Assets/Editor/RoomDesigner/**`
- `Assets/Scripts/Runtime/Rooms/RoomBlueprint.cs`, `RoomPrefabLink.cs` (namespace fix only)
- `Assets/Tests/EditMode/Editor/**` (integration test eklenecek)

**LOCKED — DOKUNMA:** `IRoomDesignerContext.cs` API yüzeyi (yeni metod eklemeden önce DUR ve raporla — kontrat değişikliği F2 işi)

## Hedef

F1 SKELETON+PALETTE+BRUSH wiring boşluklarını + Gemini cross-review bulgularını fix et. Workflow end-to-end çalışsın: window aç → palette tile seç → canvas'a tıkla → tile yerleşsin → undo geri alsın. Tek commit, tek atomik fix.

## Fix List (priority sıralı)

### KRITIK 1 — BrushController auto-init
**Sorun:** `BrushController.Instance` set edilmiyor, toolbar mount edilmiyor, hotkey kayıt yok.
**Fix:** `RimaRoomDesignerWindow.CreateGUI` sonunda (UXML clone + panel binding tamam olduktan sonra):
```csharp
EnsureBrushController();
```
Yeni method:
```csharp
private BrushController brushController;
private void EnsureBrushController()
{
    if (brushController == null) brushController = new BrushController();
    if (rightPanel != null) brushController.Initialize(this);
}
```
`OnDisable`'da `brushController = null` cleanup.

### KRITIK 2 — MouseUp event Canvas'ta
**Sorun:** Stroke hiç end olmuyor, ApplyStroke çağrılmıyor.
**Fix:** `RoomDesignerCanvas.HandleInput` switch'ine MouseUp case ekle:
```csharp
case EventType.MouseUp:
    if (evt.button == 0 || evt.button == 1)
    {
        ctx.ReleaseBrush();
        evt.Use();
        changed = true;
    }
    break;
```
**Not:** `IRoomDesignerContext`'e `ReleaseBrush()` ekleme. Onun yerine `InvokeBrush` imzasını genişletmek de YASAK (kontrat LOCKED). Çözüm: Window kendi içinde `BrushController` referansı tutuyor zaten (KRITIK 1 fix ile); Canvas mouse-up'ta `ctx as RimaRoomDesignerWindow` cast ederek public bir `OnBrushRelease()` method'unu çağırabilir. Yeni Window method:
```csharp
public void OnBrushRelease() => brushController?.OnRelease(this);
```
Canvas:
```csharp
case EventType.MouseUp:
    if ((evt.button == 0 || evt.button == 1) && ctx is RimaRoomDesignerWindow w)
    {
        w.OnBrushRelease();
        evt.Use();
        changed = true;
    }
    break;
```

### KRITIK 3 — InvokeBrush stub → real dispatch
**Sorun:** `RimaRoomDesignerWindow.InvokeBrush` sadece `MarkDirty()` yapıyor.
**Fix:** Drag state track ederek MouseDown vs MouseDrag ayrımı (Canvas event tipini Window bilmek zorunda — basit yaklaşım: drag state). Çözüm: `RimaRoomDesignerWindow`'a `_isStrokeActive` flag ekle, `InvokeBrush` ilk çağrıda Begin, sonraki çağrılarda Continue:
```csharp
private bool isStrokeActive;
public void InvokeBrush(int mouseButton, Vector3Int cell)
{
    if (brushController == null) return;
    if (!isStrokeActive)
    {
        brushController.OnInvoke(this, mouseButton, cell);
        isStrokeActive = true;
    }
    else
    {
        brushController.OnDrag(this, cell);
    }
    MarkDirty();
}
public void OnBrushRelease()
{
    if (!isStrokeActive) return;
    brushController?.OnRelease(this);
    isStrokeActive = false;
}
```

### YÜKSEK 4 — Grid IsometricZAsY
**Sorun:** `RoomDesignerCanvas.CreateSceneObjects` `Rectangle` + `XYZ` set'liyor. RIMA runtime scene `IsometricZAsY` (memory: `project_unity_isometric_setup.md`).
**Fix:**
```csharp
grid.cellLayout = GridLayout.CellLayout.Isometric;
grid.cellSwizzle = GridLayout.CellSwizzle.XYZ;
grid.cellSize = new Vector3(1f, 0.5f, 0f); // standart isometric oran
```
Veya kullanıcı projesinde mevcut isometric prefab varsa onun setting'ini referans al — proje'deki herhangi bir scene grid'inden Inspector değerini doğrula.

### YÜKSEK 5 — TileAnchor 0.5, 0, 0
**Fix:** `CreateTilemap` method'unda:
```csharp
tilemap.tileAnchor = new Vector3(0.5f, 0f, 0f);
```

### YÜKSEK 6 — Layer fallback güvenli
**Sorun:** `RoomDesigner` Unity layer yoksa cullingMask `~0` → ana scene render.
**Fix:** Layer yoksa: cullingMask = 0 (hiçbir şey render etme) + console warning + UI'da "Add 'RoomDesigner' Unity layer for preview" notu. Veya kendisi layer'ı oluştursun (TagManager edit) — basitlik için sadece warning + 0 mask.

### YÜKSEK 7 — Layer chip USS class fix
**Sorun:** `TileLibraryPanel.SyncLayerFromContext` layer button'lara `rd-biome-tab--active` class ekliyor — yanlış selector.
**Fix:**
```csharp
pair.Value.EnableInClassList("rd-layer-chip--active", pair.Key == ctx.ActiveLayer);
```
+ USS'ye selector ekle (`RoomDesignerWindow.uss`):
```css
.rd-layer-chip--active { background-color: #2A3848; border-color: #7BA7BC; }
```

### YÜKSEK 8 — RoomSaver ImportAsset reorder
**Sorun:** `bp.prefab/roomId/biome` atanmadan ImportAsset(soPath) çağrılıyor.
**Fix:** Field assignment'ları SetDirty + SaveAssets'ten ÖNCE yap, ImportAsset çağrılarını **kaldır** (CreateAsset zaten import'u tetikliyor). Yeni flow:
```csharp
var bp = ScriptableObject.CreateInstance<RoomBlueprint>();
AssetDatabase.CreateAsset(bp, soPath); made.Add(soPath);

PrefabUtility.SaveAsPrefabAsset(roomRoot, prefabPath, out bool ok);
if (!ok) throw new InvalidOperationException("Prefab save failed");
made.Add(prefabPath);

var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
bp.prefab = prefab;
bp.roomId = roomId;
bp.biome = biome;

var link = prefab.GetComponent<RoomPrefabLink>() ?? prefab.AddComponent<RoomPrefabLink>();
link.blueprint = bp;

EditorUtility.SetDirty(bp);
EditorUtility.SetDirty(prefab);
AssetDatabase.SaveAssets();
return (prefabPath, soPath);
```

### YÜKSEK 9 — RoomSaver partial orphan koruması
**Sorun:** PrefabUtility.SaveAsPrefabAsset ok=false ama dosya yazıldıysa rollback atlar.
**Fix:** SaveAsPrefabAsset call'ı sonrası `File.Exists(prefabPath)` kontrol et — varsa `made.Add` her durumda:
```csharp
PrefabUtility.SaveAsPrefabAsset(roomRoot, prefabPath, out bool ok);
if (System.IO.File.Exists(prefabPath)) made.Add(prefabPath);
if (!ok) throw new InvalidOperationException("Prefab save failed");
```

### ORTA 10 — AssetPreviewCache debounce
**Sorun:** `Tick` 60Hz `onAnyLoaded` tetikliyor → repaint storm.
**Fix:** `AssetPreviewCache.Tick` debounce:
```csharp
private double lastNotifyTime;
private const double NotifyDebounceSeconds = 0.1d;
void Tick()
{
    if (!AssetPreview.IsLoadingAssetPreviews()) return;
    double now = EditorApplication.timeSinceStartup;
    if (now - lastNotifyTime < NotifyDebounceSeconds) return;
    lastNotifyTime = now;
    onAnyLoaded?.Invoke();
}
```

### ORTA 11 — Namespace tutarlılık
**Fix:**
- Tüm Brush dosyaları (`IBrush.cs`, `BrushController.cs`, `Stamp/Eraser/Picker/BucketFillBrush.cs`): using'leri namespace block içine taşı, ayrıca namespace'i `RIMA.Editor.RoomDesigner.Brushes` sub-namespace'e çevir (Palette ile parallel)
- `RoomBlueprint.cs`, `RoomPrefabLink.cs`: namespace `RIMA.Runtime.Rooms` ekle (yeni dosyalar — F1 SKELETON'da global namespace bırakılmıştı)
- `RoomSaver.cs`: `using RIMA.Runtime.Rooms;` ekle (RoomBlueprint + RoomPrefabLink referansları için)

### ORTA 12 — PickerBrush test + FakeContext layer routing
**Fix:** `BrushTests.cs`'e:
- `FakeContext.GetActiveTilemap()` `ActiveLayer`'a göre Floor/Walls/Decals dönsün
- Yeni test: `PickerBrush_PicksTileAndSwitchesToStamp` — pre-populate Floor[0,0,0] ile tile A, OnStrokeBegin → ActiveTile == A, ActiveBrush == Stamp

### ORTA 13 — MCP poll log spam
**Fix:** `RimaRoomDesignerWindow.PollMcp` `Debug.Log` çağrısını sadece count > 0 ve önceki count'tan farklıysa log'la — veya tamamen `[Conditional("ROOM_DESIGNER_DEBUG_PERF")]`.

## Yeni Integration Test (ZORUNLU)

`Assets/Tests/EditMode/Editor/RoomDesignerIntegrationTests.cs`:
```csharp
[Test]
public void EndToEnd_StampStroke_PlacesTileInActiveTilemap()
{
    var window = EditorWindow.GetWindow<RimaRoomDesignerWindow>();
    window.Show();
    try
    {
        // CreateGUI tetikle (eğer manuel gerekirse)
        // BrushController initialized mi?
        // ActiveTile = test tile
        // InvokeBrush(0, new Vector3Int(0,0,0)) → stroke begin
        // OnBrushRelease() → ApplyStroke
        // window.FloorTilemap.GetTile(Vector3Int.zero) == test tile
        // Undo.PerformUndo() → tile null
    }
    finally { window.Close(); }
}
```
Eğer Headless EditMode'da EditorWindow lifecycle zor gelirse, Window class'ına `[MenuItem]` benzeri test-only public hooks ekle (`#if UNITY_EDITOR_TEST` veya internal + InternalsVisibleTo asmdef).

## Acceptance Criteria

- [ ] Window aç → toolbar B/E/I/G button'ları + cell debug label var
- [ ] Hotkey B/E/I/G çalışıyor (right panel focus)
- [ ] Tile palette sol panele auto-mount (TileLibraryPanelBootstrap zaten var)
- [ ] Tile click → ctx.ActiveTile set, kart highlight
- [ ] Canvas'a click → tile yerleşiyor (active layer'da)
- [ ] Drag → tek undo step (Ctrl+Z hepsini geri alıyor)
- [ ] Eraser drag → cell null
- [ ] Picker click → ActiveTile değişti, Stamp'a otomatik dönüş
- [ ] Bucket fill → bağlı bölge dolduruluyor (10000+ abort uyarı)
- [ ] Save Room atomic + rollback (var olan tests PASS)
- [ ] **Yeni integration test PASS** — end-to-end stroke
- [ ] Tüm mevcut tests (15) hala PASS
- [ ] Compile clean, console temiz (MCP poll log spam yok)
- [ ] Grid IsometricZAsY (Inspector'da doğrula)
- [ ] Namespace consistency: Brushes sub-namespace, RoomBlueprint/Link `RIMA.Runtime.Rooms`

## CODEX_DISPATCH Global Kurallar

- Model: gpt-5.5
- Yorum yazma — WHY açık değilse istisna
- Güven Döngüsü zorunlu (CODEX_DISPATCH.md): %100 güven, sonra commit
- Test PASS olmadan commit etme

## Commit Message

```
fix(room-designer): F1.1 wiring + cross-review fixes

- BrushController auto-init in window CreateGUI
- MouseUp handling in canvas + OnBrushRelease window method
- InvokeBrush real dispatch (Begin/Drag stroke state)
- Grid IsometricZAsY + TileAnchor (0.5,0,0)
- Layer fallback safe (cullingMask=0 + warning)
- USS rd-layer-chip--active fix
- RoomSaver: ImportAsset reorder, partial orphan File.Exists guard
- AssetPreviewCache debounce 100ms
- Namespace consistency: RIMA.Editor.RoomDesigner.Brushes, RIMA.Runtime.Rooms
- PickerBrush test + FakeContext layer routing
- MCP poll log spam fix
- New integration test: end-to-end stamp stroke
```

## Kaynak

- Opus code review (bu prompt yukarıda özet)
- rima-qc Gemini review (full report orchestrator state'inde)
- `STAGING/roomdesigner_codex_review.md` — orijinal 6 teknik soru
- `MEMORY/project_unity_isometric_setup.md` — IsometricZAsY rationale
