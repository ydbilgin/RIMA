# Codex Task — Room Designer F1 PALETTE

**Status:** WAIT for SKELETON commit (depends on `IRoomDesignerContext`)
**Branch:** master
**Estimated:** ~600-900 LOC
**Allowed paths:**
- `Assets/Editor/RoomDesigner/Palette/**`
- `Assets/Editor/RoomDesigner/UI/RoomDesignerWindow.uss` (sadece `.rd-tile-card`, `.rd-tile-grid`, `.rd-biome-tab` selector'ları ekle — başka selector'lara dokunma)

## Hedef

Tile Library panel — sol panele entegre, biome filter (Act1: Keep / Crypt / Volcanic), AssetPreview cache, drag-drop yok ama click-select çalışıyor (`ctx.ActiveTile = selected`). Mario Maker tile palette hissi.

## Bağımlılık (LOCKED)

- `IRoomDesignerContext` interface SKELETON task tarafından commit edildikten sonra çalış
- `ctx.LeftPanel` (VisualElement) içine kendi paneli ekle — root'a değil
- `ctx.ActiveTile` set ettiğinde Brush onu okuyacak

## Yapılacak Dosyalar

### 1. `Assets/Editor/RoomDesigner/Palette/TileLibraryPanel.cs` (~350 LOC)
- `TileLibraryPanel : VisualElement` — UI Toolkit element
- Constructor: `TileLibraryPanel(IRoomDesignerContext ctx)` — ctx referansı sakla
- Sections:
  - **Top:** Biome tabs (`Keep`, `Crypt`, `Volcanic`, `All`) — radio button davranışı
  - **Middle:** Layer filter chips (`Floor`, `Walls`, `Decals`) — `ctx.ActiveLayer` ile sync
  - **Bottom:** Virtualized `ListView` (UI Toolkit) veya scroll grid — tile thumbnail'ları
- Tile asset kaynağı: `Assets/Art/Tiles/**` altındaki tüm `TileBase` asset'leri `AssetDatabase.FindAssets("t:TileBase")` ile çek. Path filtresi: biome path content'i (örn `/Keep/`, `/Crypt/`, `/Volcanic/`) — biome assignment heuristic.
- Click → `ctx.ActiveTile = tile`, kart highlight (USS class `rd-tile-card--selected`)
- Hover tooltip: tile asset adı + path

### 2. `Assets/Editor/RoomDesigner/Palette/AssetPreviewCache.cs` (~150 LOC)
**Codex review Soru 2 cevabı pattern'i — uygula:**
```csharp
public class AssetPreviewCache
{
    readonly Dictionary<int, Texture2D> cache = new();
    Action onAnyLoaded;

    public AssetPreviewCache(Action repaintCallback)
    {
        onAnyLoaded = repaintCallback;
        EditorApplication.update += Tick;
    }

    public Texture2D Get(Object asset)
    {
        if (asset == null) return null;
        int id = asset.GetInstanceID();
        if (cache.TryGetValue(id, out var tex) && tex != null) return tex;
        tex = AssetPreview.GetAssetPreview(asset) ?? AssetPreview.GetMiniThumbnail(asset);
        if (tex != null) cache[id] = tex;
        return tex;
    }

    void Tick()
    {
        if (AssetPreview.IsLoadingAssetPreviews()) onAnyLoaded?.Invoke();
    }

    public void Dispose() { EditorApplication.update -= Tick; cache.Clear(); }
    public void Invalidate() => cache.Clear();   // domain reload / asset import sonrası
}
```
- Domain reload sonrası `Invalidate()` — `[InitializeOnLoad]` veya `AssetPostprocessor.OnPostprocessAllAssets` callback'i ile.

### 3. `Assets/Editor/RoomDesigner/Palette/BiomeFilter.cs` (~80 LOC)
- Static helper: `string BiomeOf(string assetPath)` → "keep" / "crypt" / "volcanic" / "unknown"
- Path heuristic: substring check `/Keep/`, `/Crypt/`, `/Volcanic/` (case-insensitive)
- Tile metadata via `RoomBlueprint.biome` ile gelecekte geliştirilebilir — şimdilik path-based yeterli

### 4. UXML/USS değişiklikleri

**UXML — DOKUNMA.** `LeftPanel` slot'u SKELETON tarafından oluşturuldu, runtime'da `ctx.LeftPanel.Add(new TileLibraryPanel(ctx))` çağır.

**USS — sadece kendi class'larını ekle:**
```css
.rd-tile-grid { flex-direction: row; flex-wrap: wrap; padding: 4px; }
.rd-tile-card { width: 56px; height: 56px; margin: 2px; border-width: 1px; border-color: #333; }
.rd-tile-card--selected { border-color: #7BA7BC; border-width: 2px; }
.rd-biome-tab { padding: 4px 8px; margin: 2px; }
.rd-biome-tab--active { background-color: #2A3848; }
.rd-layer-chip { padding: 2px 6px; margin: 1px; border-radius: 4px; }
```

## Performans

- 100+ tile için `ListView` virtualized binding — visible range outside repaint atla
- AssetPreview lazy load — placeholder thumbnail (gri kare) göster, hazır olunca swap
- Per-frame her asset'e preview sorma — sadece visible olanlara

## Acceptance Criteria

- [ ] Window'u açıp `RIMA/Test/Seed Tile Library` gibi bir test menü item ile geçici TileBase asset'leri (50+ tane) ürettiğinde panel hepsini gösteriyor (test menü item bu task içinde implement edilebilir, opsiyonel)
- [ ] Boş `Assets/Art/Tiles/` durumunda "no tiles" empty state göster
- [ ] Biome tab değiştirince filtre çalışıyor
- [ ] Layer chip değiştirince `ctx.ActiveLayer` güncelleniyor
- [ ] Tile click → `ctx.ActiveTile` set, görsel highlight
- [ ] AssetPreview ilk frame null sonra dolma akışı çalışıyor — repaint tetiklen­iyor
- [ ] 50+ tile yüklenirken UI 60fps+ kalıyor
- [ ] Compile error yok

## CODEX_DISPATCH Global Kurallar

- Model: **gpt-5.5**
- Yorum yazma — WHY açık değilse istisna
- Test: `Assets/Tests/EditMode/Editor/PaletteTests.cs`:
  - `BiomeFilter.BiomeOf` path heuristic happy path
  - `AssetPreviewCache` get + invalidate

## Güven Döngüsü

1. Implementasyon → "%100 güven var mı?"
2. Açıklar → düzelt
3. %100 güven → commit

## Commit Message

```
feat(room-designer): F1 palette — Tile Library panel + AssetPreview cache + biome/layer filter

- TileLibraryPanel (VisualElement, ctx.LeftPanel'e mount)
- AssetPreviewCache (lazy load + EditorApplication.update tick)
- BiomeFilter (path heuristic Keep/Crypt/Volcanic)
- USS palette selectors (rd-tile-card, rd-biome-tab, rd-layer-chip)
- EditMode tests (biome filter + cache)
```

## Kaynak

- `STAGING/roomdesigner_codex_review.md` — Soru 2 (AssetPreview pattern)
- `MEMORY/project_room_designer_plan.md`
- SKELETON commit'i — `IRoomDesignerContext` API sözleşmesi
