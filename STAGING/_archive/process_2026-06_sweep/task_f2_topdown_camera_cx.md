# TASK: F2 oyun-içi designer'a TOP-DOWN OVERVIEW kamerası ekle (cx)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — sadece 1 dosya (4) BLOCKED yaz belirsizse.

NLM ACCESS: gerekirse: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"

Amaç: Kullanıcı oyun-içi F2 map designer'ında haritayı **ÜSTTEN / tüm oda görünecek şekilde** görüp tasarlamak istiyor. Şu an F2 açılınca pause oluyor ama kamera yakın gameplay zoom'da kalıyor → tüm oda görünmüyor. F2 AÇILINCA kamerayı odanın tamamını gösteren overview'a al, F2 KAPANINCA gameplay kamerasına geri dön. Sen yazarsın (writer); Opus + Unity compile review.

## DOSYA: `Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs`
(F2 overlay; `SetVisible(bool)` ~satır 325-340 pause/restore yapıyor; `_floorTilemap`, `_grid`, `Camera.main` mevcut.)

## ⭐ FIX 0 (KRİTİK — ÖNCE BUNU): DiscoverTilemaps floor'u bulamıyor
`DiscoverTilemaps()` (~satır 983-989) `name.Contains("floor")` arıyor ama bizim iso floor tilemap'inin adı **"Ground"** (IsoGrid/Ground). Bu yüzden F2 "<no Floor tilemap found>" diyor + boyanamıyor.
DÜZELT: floor eşleşmesini genişlet —
```csharp
if (_floorTilemap == null && (name.Contains("floor") || name.Contains("ground"))) _floorTilemap = tm;
```
(cliff zaten "CliffTilemap" → Contains("cliff") ✓.) Bu olmadan F2 hiç boyamaz; overview kamerası da odanın bounds'unu floorTilemap'ten alıyor.

## YAP (overview kamera — top-down DEĞİL, ISO zoom-out)
NOT: Kullanıcı top-down projeksiyon İSTEMİYOR. Kamera açısı/projeksiyonu DEĞİŞMEZ — sadece orthographicSize büyütülüp odanın tamamı görünür (iso görünüm korunur). Aşağıdaki adımlar bunu yapıyor zaten.
1. Yeni alanlar ekle: kamera durumu cache (Vector3 _camPosCache; bool _camCached; float _orthoCache; bool _ppcWasEnabled).
2. `SetVisible(true)` içinde (pause'dan SONRA) `EnterOverviewCamera()` çağır.
3. `SetVisible(false)` / `RestoreTimeScale` yolunda `ExitOverviewCamera()` çağır (idempotent — sadece _camCached ise).
4. `EnterOverviewCamera()`:
   - `var cam = Camera.main; if(cam==null) return;` _camCached ise return.
   - Cache: _camPosCache=cam.transform.position; _orthoCache=cam.orthographicSize.
   - **Oda bounds:** `_floorTilemap` varsa `_floorTilemap.CompressBounds(); Bounds b = _floorTilemap.localBounds;` world center = `_floorTilemap.transform.TransformPoint(b.center)`. (floorTilemap yoksa _grid'den FindObjectsByType<Tilemap> floor; o da yoksa return.)
   - Kamerayı odaya ortala: `cam.transform.position = new Vector3(worldCenter.x, worldCenter.y, _camPosCache.z);`
   - **PixelPerfectCamera** (UnityEngine.Rendering.Universal.PixelPerfectCamera) varsa: cache _ppcWasEnabled=ppc.enabled; **ppc.enabled=false** (serbest zoom için).
   - orthographicSize'ı odayı sığdıracak şekilde ayarla: `float halfH=b.size.y*0.5f; float halfW=(b.size.x*0.5f)/cam.aspect; cam.orthographicSize = Mathf.Max(halfH, halfW) * 1.15f;` (1.15 = kenar payı).
   - _camCached=true.
5. `ExitOverviewCamera()`:
   - if(!_camCached) return; var cam=Camera.main; if(cam!=null){ cam.transform.position=_camPosCache; cam.orthographicSize=_orthoCache; var ppc=cam.GetComponent<UnityEngine.Rendering.Universal.PixelPerfectCamera>(); if(ppc!=null) ppc.enabled=_ppcWasEnabled; } _camCached=false;
   - (PPC tekrar enable olunca refResolution'dan gameplay zoom'u kendi hesaplar.)
6. OnDisable/OnDestroy'da da ExitOverviewCamera() çağır (güvenli restore).

## SUCCESS
- Unity compile 0 hata (read_console / dotnet build).
- F2 açınca kamera tüm odayı üstten gösterir (overview), CellUnderMouse boyama doğru çalışır (ScreenToWorldPoint zaten kameradan bağımsız).
- F2 kapanınca gameplay kamerası (yakın zoom + PPC) geri gelir.
- CameraFollow varsa: overview sırasında player'ı takip etmesin — gerekirse cam'deki CameraFollow component'ini overview'da disable et, exit'te restore et (cache _camFollowWasEnabled). (Camera.main'de `CameraFollow` aranır.)

ASCII, Türkçe. Sadece bu dosya.
