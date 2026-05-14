# Room Designer QC Test Plan -- 14 Partials

Codex iter 2 implementasyonundan sonra her partial icin PASS/FAIL kanıtla.
Test ortami: Unity Editor, sahne = `Assets/Scenes/Demo/RoomPipelineTest.unity`,
template = `Assets/Art/Tiles/Keep/Keep_Combat.asset`.

---

## Pre-flight Checklist

1. Unity Editor open, no compile errors in Console
2. RIMA > Room Designer menusu acinir (EditorWindow spawn)
3. `Assets/Scenes/Demo/RoomPipelineTest.unity` sahnesi yuklenir
4. Sol ObjectField'a `Keep_Combat.asset` atanir
5. Console clear (Edit > Clear Console)

---

## 1. Canvas Render

### Hazirlik
- Room Designer Window acik
- Window boyutu minimum 400x400 px olacak sekilde ayarlanir
- Keep_Combat template atanir (adim 4)

### PASS Kriterleri
- [ ] imguiContainer icinde bir Render Texture gorsel alani mevcut ve siyah degil
- [ ] Canvas arkaplan rengi koyu (floor yok ise dark BG, tile varsa en az 1 tile gorsel)
- [ ] Render Texture boyutu window boyutuyla uyumlu (resized event'te degisir)
- [ ] Preview Camera aktif ve RenderTexture'a atanmis: `camera.targetTexture != null`

### FAIL Signatures
- Tum canvas alani siyah veya soyut gri gradient
- imguiContainer yuksekligi 0 veya gorunmez
- RenderTexture allocation exception (Console: "Failed to create RenderTexture")
- Camera.targetTexture null reference exception

### Test Komutu
```csharp
// execute_code ile: Canvas camera ve RT kontrol
var win = EditorWindow.GetWindow(
    System.Type.GetType(
        "RimaRoomDesignerWindow, Assembly-CSharp-Editor"));
var canvasField = win.GetType()
    .GetField("_canvas",
        System.Reflection.BindingFlags.NonPublic |
        System.Reflection.BindingFlags.Instance);
var canvas = canvasField?.GetValue(win);
var camField = canvas?.GetType()
    .GetField("_previewCamera",
        System.Reflection.BindingFlags.NonPublic |
        System.Reflection.BindingFlags.Instance);
var cam = camField?.GetValue(canvas) as Camera;
Debug.Log($"[QC-1] Camera null={cam == null}, " +
    $"RT null={cam?.targetTexture == null}");
```
Manual fallback: Window'u gozle denetim -- icerik gorsel mi?

---

## 2. Brush Stamp Mode

### Hazirlik
- Canvas Render PASS
- Brush mode = Stamp secili (toolbar butonu)
- Aktif tile = herhangi bir floor tile (Keep_Combat.singleFloorTile)
- Aktif layer = Floor

### PASS Kriterleri
- [ ] Sol tik sonrasi: mouse altindaki cell'de `floorTilemap.GetTile(cell) != null`
- [ ] Stamp yalnizca 1 hucreyi etkiler (komsu hucreler degismez)
- [ ] Aktif layer = Walls iken stamp; WallsTilemap'i etkiler, FloorTilemap'i etkilemez
- [ ] Undo (Ctrl+Z) sonrasi tile temizlenir

### FAIL Signatures
- Tiklanan cell degil komsu bir cell'de tile cikiyor (coord offset hatasi)
- SceneView tilemapi degisiyor, canvas tilemapi degismiyor
- ActiveTile null iken exception (NullReferenceException satir numarasi)
- Brush her mouse move'da stamp yapiyor (mouseDown degil mouseMove'da tetikleniyor)

### Test Komutu
```csharp
// Belirli bir cell'e stamp sonrasi tile varligini dogrula
var grid = GameObject.Find("RoomDesignerGrid");
var floorTilemap = grid?.transform.Find("FloorTilemap")
    ?.GetComponent<Tilemap>();
var cell = new Vector3Int(0, 0, 0);
var tile = floorTilemap?.GetTile(cell);
Debug.Log($"[QC-2] Cell(0,0) tile={tile?.name ?? "NULL"}");
```

---

## 3. Brush Fill Mode

### Hazirlik
- Canvas Render PASS
- 3x3 floor tile blogu manuel stamp ile yerlestir (9 hucre)
- Brush mode = Fill secili
- Aktif tile = farkli bir tile (accent tile)

### PASS Kriterleri
- [ ] Fill click sonrasi: connected 9 hucre accent tile ile doldu
- [ ] 9 hucrenin disindaki hucreler degismedi
- [ ] Fill, bos (null) hucre sinirinda durdu
- [ ] 100x100 grid uzerinde fill: <500ms tamamlaniyor (performance)

### FAIL Signatures
- Tum tilemap doldu (sinir kontrolu yok)
- Hic hucre degismedi (fill calismadi)
- Fill cagrisinda Unity Editor dondu / crash
- Wrong tile: doldurulan hucrelerde aktif tile degil baska tile var

### Test Komutu
```csharp
// Fill sonrasi 9 hucreli alan kontrolu
var grid = GameObject.Find("RoomDesignerGrid");
var floorTilemap = grid?.transform.Find("FloorTilemap")
    ?.GetComponent<Tilemap>();
int filled = 0;
for (int x = -1; x <= 1; x++)
    for (int y = -1; y <= 1; y++)
        if (floorTilemap?.GetTile(new Vector3Int(x, y, 0)) != null)
            filled++;
Debug.Log($"[QC-3] Filled cells in 3x3 area: {filled} (expected 9)");
```

---

## 4. Brush Erase Mode

### Hazirlik
- Canvas Render PASS
- En az 3 floor tile mevcut
- Brush mode = Erase secili
- Aktif layer = Floor

### PASS Kriterleri
- [ ] Sol tik sonrasi: hovered cell'de `floorTilemap.GetTile(cell) == null`
- [ ] Komsu hucreler etkilenmez
- [ ] Aktif layer = Walls iken erase; WallsTilemap'i etkiler, FloorTilemap'i etkilemez
- [ ] Bos hucrede erase: exception yok, sessizce devam

### FAIL Signatures
- Erase tum tilemapi sildi (GetAllTiles() bos dondurdu)
- Hic tile silinmedi (erase calismadi)
- Yanlis layer silindi (Floor silinmesi gerekirken Wall silindi)
- NullReferenceException erase sirasinda

### Test Komutu
```csharp
// Erase sonrasi cell kontrolu
var grid = GameObject.Find("RoomDesignerGrid");
var floorTilemap = grid?.transform.Find("FloorTilemap")
    ?.GetComponent<Tilemap>();
var cell = new Vector3Int(0, 0, 0);
var tile = floorTilemap?.GetTile(cell);
Debug.Log($"[QC-4] Cell(0,0) after erase: tile={tile?.name ?? "NULL (erased OK)"}");
```

---

## 5. Layer Dropdown

### Hazirlik
- Room Designer Window acik
- Dropdown'da Floor secili (varsayilan)

### PASS Kriterleri
- [ ] "Walls" secilince `ActiveLayer == RoomLayer.Walls`
- [ ] "Decals" secilince `ActiveLayer == RoomLayer.Decals`
- [ ] `GetActiveTilemap()` Floor=FloorTilemap, Walls=WallsTilemap, Decals=DecalsTilemap dondurur
- [ ] Layer degisince canvas'taki hover highlight rengi degisir (opsiyonel gosel feedback)

### FAIL Signatures
- Dropdown tiklandi ama ActiveLayer enum degismedi
- GetActiveTilemap() herzaman FloorTilemap dondurdu (switch case eksik)
- Dropdown UI'i yeniden acilinca onceki seciyi kaybetti
- 3'ten fazla veya 3'ten az layer option listeleniyor

### Test Komutu
```csharp
// Reflection ile ActiveLayer degerini oku
var win = EditorWindow.GetWindow(
    System.Type.GetType(
        "RimaRoomDesignerWindow, Assembly-CSharp-Editor"));
var layerProp = win?.GetType()
    .GetProperty("ActiveLayer",
        System.Reflection.BindingFlags.Public |
        System.Reflection.BindingFlags.Instance);
Debug.Log($"[QC-5] ActiveLayer={layerProp?.GetValue(win)}");
```
Manual: Dropdown'u Walls'a cek, stamp yap, WallsTilemap'in degistigini kontrol et.

---

## 6. Template ObjectField

### Hazirlik
- Room Designer Window acik
- Template alani bos (null)

### PASS Kriterleri
- [ ] `Keep_Combat.asset` surukle-birak veya picker ile ataninca field doldu
- [ ] Generate butonu assignment'tan sonra interaktif (disabled=false)
- [ ] Template null iken Generate butonu disabled
- [ ] Farkli bir template ataninca onceki temizleniyor (stale ref yok)
- [ ] Assignment undo edilebilir (Ctrl+Z)

### FAIL Signatures
- Null assignment: field gorsel dolu gorunuyor ama template.singleFloorTile null
- Generate butonu null template ile calisiyor (null check eksik = crash)
- Template degisince Generate onceki template datasi ile calisiyor (stale)
- ObjectField tipi yanlis (RimaRoomBaselineTemplate yerine UnityEngine.Object)

### Test Komutu
```csharp
// Aktif template kontrolu
var win = EditorWindow.GetWindow(
    System.Type.GetType(
        "RimaRoomDesignerWindow, Assembly-CSharp-Editor"));
var tmplField = win?.GetType()
    .GetField("_activeTemplate",
        System.Reflection.BindingFlags.NonPublic |
        System.Reflection.BindingFlags.Instance);
var tmpl = tmplField?.GetValue(win);
Debug.Log($"[QC-6] Template={tmpl?.GetType().Name ?? "NULL"}, " +
    $"FloorTile={((tmpl as RimaRoomBaselineTemplate)?.singleFloorTile?.name ?? "NULL")}");
```

---

## 7. Generate Button (LayeredRoomGenerator)

### Hazirlik
- Keep_Combat template atanmis
- Canvas bos (tum tilemaplar temizlenmis)

### PASS Kriterleri
- [ ] Generate tiklaninca canvas'ta CA-tabanli cave oda sekli olusur (dikdortgen degil)
- [ ] Olusturulan floor hucreleri template.singleFloorTile ile dolu
- [ ] Olusturulan wall hucreleri template.singleWallTile ile dolu
- [ ] RoomBlueprint (ScriptableObject) guncellendi: floorCells.Count > 0
- [ ] `LayeredRoomGenerator` class'i cagriliyor (eski `RimaRoomBaselineGenerator` degil)
- [ ] Console'da Debug.Log: "Generated room: X floor cells, Y wall cells"

### FAIL Signatures
- Canvas'ta dikdortgen oda cikti (RimaRoomBaselineGenerator hala kullaniliyor)
- Tile atamasiz hata: "NullReferenceException: singleFloorTile is null"
- RoomBlueprint.floorCells.Count == 0 (veri kaydedilmedi)
- Generate her tiklamada farkli boyut uretmiyor (CA seed sabit veya calismiyor)

### Test Komutu
```csharp
// Generate sonrasi blueprint ve tilemap kontrolu
var grid = GameObject.Find("RoomDesignerGrid");
var floorTilemap = grid?.transform.Find("FloorTilemap")
    ?.GetComponent<Tilemap>();
int floorCount = 0;
if (floorTilemap != null)
{
    floorTilemap.CompressBounds();
    var bounds = floorTilemap.cellBounds;
    foreach (var pos in bounds.allPositionsWithin)
        if (floorTilemap.GetTile(pos) != null) floorCount++;
}
Debug.Log($"[QC-7] Floor tile count={floorCount} (>0 = PASS, rect shape = FAIL)");
```

---

## 8. Paint Decals Button

### Hazirlik
- Generate PASS (floor tilemapi dolu)
- Template.decalSprites.Length > 0
- Template.decalDensity ayarli (ornek: 0.15 = %15)

### PASS Kriterleri
- [ ] Paint Decals sonrasi DecalsTilemap'te en az 1 tile mevcut
- [ ] Decal yerlestirilen hucre sayisi: floor hucre sayisi * decalDensity +/- %20 tolerans
- [ ] Tum decal sprite'lari template.decalSprites listesinden geliyor
- [ ] Decal yalnizca floor hucreleri uzerine yerlestirilmis (wall veya bos hucre yok)
- [ ] Console'da error yok

### FAIL Signatures
- DecalsTilemap.GetAllTiles().Length == 0 (hic decal yok)
- DecalsTilemap tamamen dolu (density hesabi calismiyor, her hucreye decal atandi)
- Yanlis sprite: template.decalSprites disinda bir sprite gorsel
- Console: "Debug.LogError" icerikli satir (DecalPainter hata firlatmis)
- Decal wall uzerine yerlestirilmis

### Test Komutu
```csharp
var grid = GameObject.Find("RoomDesignerGrid");
var decalTilemap = grid?.transform.Find("DecalsTilemap")
    ?.GetComponent<Tilemap>();
int decalCount = 0;
if (decalTilemap != null)
{
    decalTilemap.CompressBounds();
    foreach (var pos in decalTilemap.cellBounds.allPositionsWithin)
        if (decalTilemap.GetTile(pos) != null) decalCount++;
}
Debug.Log($"[QC-8] Decal tile count={decalCount}");
```

---

## 9. Place Props Button

### Hazirlik
- Generate PASS (oda mevcut)
- Template.propPlacements.Length > 0 (en az 1 prop tanimi)
- Sahnede "StageRoot" veya belirlenmis parent GameObject mevcut

### PASS Kriterleri
- [ ] Place Props sonrasi stageRoot altinda en az 1 yeni GameObject var
- [ ] Spawn edilen prefab template'in propPlacements listesinden geliyor
- [ ] Her prop AnchorZone siniri icinde konumlandirilmis (bounds check)
- [ ] Ayni prop iki kez spawn edilmedi (duplicate guard calisiyor)
- [ ] Console'da NullReferenceException yok

### FAIL Signatures
- stageRoot altinda yeni GameObject yok (Place Props calismadi)
- Prop, odanin disinda (AnchorZone siniri atlanmis) spawn oldu
- Duplicate: ayni prefab birden fazla spawn edildi
- Console: "NullReferenceException: prefab is null" (template prop tanimi bos)
- Prop Scene root'una spawn oldu (yanlis parent)

### Test Komutu
```csharp
var stageRoot = GameObject.Find("StageRoot");
int propCount = stageRoot != null ? stageRoot.transform.childCount : -1;
Debug.Log($"[QC-9] StageRoot child count after PlaceProps={propCount} (>0 = PASS)");
```

---

## 10. Apply Transitions Button

### Hazirlik
- Generate PASS (floor tilemapi dolu)
- Template biome transition tile'lari tanimli

### PASS Kriterleri
- [ ] Apply Transitions sonrasi FloorTilemap'in edge hucrelerinde transition tile'lari gorsel
- [ ] Oda ic hucreleri (non-edge) degismedi (singleFloorTile korundu)
- [ ] Transition yalnizca floor-wall sinirinda uygulandi
- [ ] Console'da hata yok

### FAIL Signatures
- FloorTilemap degismedi (BiomeTransitionPainter calismadi)
- Tum floor hucreleri transition tile ile degistirildi (edge check eksik)
- Non-floor hucrelerde transition tile goruldu
- Console: herhangi bir LogError satirı

### Test Komutu
```csharp
// Edge vs interior tile farkliligi kontrolu
var grid = GameObject.Find("RoomDesignerGrid");
var floorTilemap = grid?.transform.Find("FloorTilemap")
    ?.GetComponent<Tilemap>();
floorTilemap?.CompressBounds();
var bounds = floorTilemap.cellBounds;
// Center cell (interior) -- hala singleFloorTile olmali
var centerCell = new Vector3Int(
    (bounds.xMin + bounds.xMax) / 2,
    (bounds.yMin + bounds.yMax) / 2,
    0);
var centerTile = floorTilemap?.GetTile(centerCell);
Debug.Log($"[QC-10] Center cell tile={centerTile?.name ?? "NULL"} (must NOT be null)");
```

---

## 11. Save Button

### Hazirlik
- Generate PASS (canvas'ta oda var)
- `Assets/Rooms/Generated/` klasoru mevcut veya olusturulabilir
- Proje write-izinli

### PASS Kriterleri
- [ ] Save sonrasi `Assets/Rooms/Generated/` altinda `.prefab` dosyasi olusturuldu
- [ ] Ayni klasorde `RoomBlueprint_<name>.asset` dosyasi olusturuldu
- [ ] Console'da Debug.Log: saved prefab path
- [ ] Project window Refresh sonrasi dosyalar gorulur
- [ ] Ikinci Save: dosyayi override eder, duplikat olusturmaz

### FAIL Signatures
- Hic dosya olusturilmadi
- Exception: "UnauthorizedAccessException" veya "PathTooLongException"
- Prefab olusturuldu, .asset olusturulmadi (RoomBlueprint eksik)
- Path null veya bos string (Debug.Log path="" goruldu)

### Test Komutu
```csharp
// Dosya varligini kontrol et
bool prefabExists = System.IO.File.Exists(
    "Assets/Rooms/Generated/Keep_Combat_Room.prefab");
bool assetExists = System.IO.File.Exists(
    "Assets/Rooms/Generated/RoomBlueprint_Keep_Combat_Room.asset");
Debug.Log($"[QC-11] Prefab exists={prefabExists}, Blueprint exists={assetExists}");
```
Manual: Project window'da `Assets/Rooms/Generated/` klasorunu ac.

---

## 12. Tile Library Panel (sol)

### Hazirlik
- Keep_Combat template atanmis
- Room Designer Window yeterince genis (panel gorulebilir)

### PASS Kriterleri
- [ ] Sol panel'de template.singleFloorTile icin thumbnail + label gorulur
- [ ] Sol panel'de template.singleWallTile icin thumbnail + label gorulur
- [ ] template.accentTiles listesindeki her tile icin ayri buton mevcut
- [ ] template.decalSprites icin sprite thumbnail gorulur
- [ ] Tile butonuna tiklayinca ActiveTile o tile'a setlendi
- [ ] Template degisince panel yenilendi (stale thumbnail yok)

### FAIL Signatures
- Sol panel tamamen bos (tile butonlari render edilmedi)
- Thumbnail bos / kirmizi soru isareti (sprite preview yuklenmedi)
- Tile butonuna tiklayinca ActiveTile degismedi
- accentTiles listesi 3 tile iken panelde yalnizca 1 tile goruldu
- Template degisince panel guncellenmedi (eski template tile'lari hala goruluyor)

### Test Komutu
```csharp
// ActiveTile kontrolu (tile butonuna tiklama simule edilemiyor; manual test)
var win = EditorWindow.GetWindow(
    System.Type.GetType(
        "RimaRoomDesignerWindow, Assembly-CSharp-Editor"));
var activeTileField = win?.GetType()
    .GetField("_activeTile",
        System.Reflection.BindingFlags.NonPublic |
        System.Reflection.BindingFlags.Instance);
Debug.Log($"[QC-12] ActiveTile={activeTileField?.GetValue(win)?.ToString() ?? "NULL"}");
```
Manual: Sol paneli gozle denetle -- thumbnail'lar gorulur mu?

---

## 13. Hover Cell Calc

### Hazirlik
- Canvas Render PASS
- Canvas uzeri mouse hareketi

### PASS Kriterleri
- [ ] Mouse canvas icinde hareket ederken cell-debug label guncelleniri (ornek: "Cell: (3, -2)")
- [ ] Mouse canvas kenarinda: label son gecerli cell'i gosteriyor (canvas-dis ray atilmiyor)
- [ ] Stamp yapilinca: Debug.Log veya label'daki cell ile TileMap'te degisen cell ayni
- [ ] Scene View aktif iken canvas hover etkilenmiyor (Scene View ray'i kullaniyor ise FAIL)

### FAIL Signatures
- Cell label hic degismiyor (hover hesabi calismıyor)
- Canvas'in sol ustu ile sag alti arasin koordinatlar terslenmiş
- Hover cell ile stamp cell farkli (offset: ornek hover=(2,1) stamp=(3,0))
- Scene View'da mouse oynatinca Room Designer canvas'i etkiliyor

### Test Komutu
```csharp
// Hover calc test: canvas'in tam ortasinda beklenen cell
// (manual adim -- execute_code ile sim edilemez)
// Manual: canvas ortasina gidip cell label'in beklenen degeri
// gosterdigini dogrula.
// Beklenti: canvas 400px wide, PPU=64 -> orta cell = (0,0) veya (3,3)
// gibi grid center'a yakin bir deger
Debug.Log("[QC-13] Manual test required: hover mouse center of canvas, verify cell label");
```

---

## 14. Pixel-Perfect Preview

### Hazirlik
- Canvas Render PASS
- Toolbar'da PP (Pixel-Perfect) toggle butonu gorulur

### PASS Kriterleri
- [ ] PP toggle acikken: canvas uzerinde grid overlay cizgileri gorulur
- [ ] PP toggle kapali iken: grid overlay cizgileri kaybolur
- [ ] Toggle durumu window kapanip acilinca korunur (EditorPrefs veya field ile)
- [ ] PP modu: kamera ortografik, boyut kilit uygulaniyor (pixel snap)
- [ ] Toggle sirasinda exception yok

### FAIL Signatures
- Toggle tiklandi, hicbir gosel degisiklik olmadi (overlay render edilmedi)
- Toggle calisuyor goruluyor fakat canvas tamamen siyah oldu
- Exception: "OnGUI is not allowed" veya benzeri IMGUI hata
- Overlay kapaniyor ancak tekrar acilamiyor (boolean stuck)

### Test Komutu
```csharp
// PP toggle state kontrolu
var win = EditorWindow.GetWindow(
    System.Type.GetType(
        "RimaRoomDesignerWindow, Assembly-CSharp-Editor"));
var ppField = win?.GetType()
    .GetField("_pixelPerfect",
        System.Reflection.BindingFlags.NonPublic |
        System.Reflection.BindingFlags.Instance);
Debug.Log($"[QC-14] PixelPerfect toggle state={ppField?.GetValue(win)}");
```
Manual: Toggle butonuna iki kez bas, canvas'ta grid overlay acar/kapanir mi gozlemle.

---

## Genel Test Sirasi

1. **Pre-flight**: Unity refresh, console clear, RIMA > Room Designer ac, Keep_Combat template ata
2. **Sequential test**: 1 --> 14 sirasıyla
3. **Her FAIL'de**: Codex'e iter 3 task ile geri gonder (FAIL signature + evidence)
4. **Tum PASS sonrasi**: "tum partial'lar calisiyor, test et" raporu ile user'a ilet

---

## FAIL -> Codex Yonlendirme Sablonu

```
PARTIAL <N> FAIL
Evidence: <test komutu ciktisi veya manual gozlem>
FAIL Signature: <hangi signature eşlesti>
Affected file: Assets/Editor/RoomDesigner/<dosya>
Fix scope: <sadece bu partial>
```

---

## Pass/Fail Ozet Tablosu (iter 2 sonrasi doldur)

| # | Partial | Durum | Notlar |
|---|---------|-------|--------|
| 1 | Canvas Render | - | |
| 2 | Brush Stamp | - | |
| 3 | Brush Fill | - | |
| 4 | Brush Erase | - | |
| 5 | Layer Dropdown | - | |
| 6 | Template ObjectField | - | |
| 7 | Generate Button | - | |
| 8 | Paint Decals | - | |
| 9 | Place Props | - | |
|10 | Apply Transitions | - | |
|11 | Save Button | - | |
|12 | Tile Library Panel | - | |
|13 | Hover Cell Calc | - | |
|14 | Pixel-Perfect Preview | - | |
