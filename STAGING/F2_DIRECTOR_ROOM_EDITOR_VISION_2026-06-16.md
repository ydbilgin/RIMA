# F2 / Director — In-Play Room Editor Vision (2026-06-16, kullanıcı)

> Demo centerpiece = "Edit-to-Play" (kilitli strateji: %60 mimari, wow = F2 ile oda kur → oyna). Bu doküman kullanıcının 2026-06-16 vizyonunu + mevcut altyapı eşleşmesini kaydeder.

## Kullanıcı vizyonu (F2/Director'dan oda düzenle)
1. **Daha uzaktan oyun açısı** edit sırasında (zoom-out edit kamerası).
2. **İstenen dungeon prop'larını yerleştir** (palet).
3. **Floor tile yerleştir** → **"Regenerate Cliff" seçeneği** → cliffleri otomatik doldur.
4. Akış: tile/prop koy → cliff üret → (sonra) oyna.

## Mevcut altyapı (REDO ETME — bağla/konsolide et)
| İhtiyaç | Mevcut parça | Yol |
|---|---|---|
| Floor/Cliff/Prop boyama (oyun-içi) | `Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs` (PaintLayer Floor/Cliff/Prop, mouse-drag) | konsolidasyon (eski IMGUI; F2'ye sur) |
| Tile/prop yerleştir + PLACE/ERASE/PAINT/INSPECT | Build Mode: `BuildModeController` + `BuildPlacementController` + `BuildTileBrushController` | F2 toggle (mevcut) |
| **Floor'dan cliff üret** | **`RoomPainter/RoomCliffSolver.Solve(floorCells, southClearCells=5)`** + `SolveFromRoom(RoomData)` | "Regenerate Cliff" butonu → Solve → cliff tilemap'e uygula (`CliffGenerateAction`/`CliffMeshGenerator` de var) |
| Prop paleti (Director) | `DirectorMode.directorPlaceableProps` + `propBindings` + Spawn/Stats/Telemetry sekmeleri | mevcut |
| Zoom-out edit kamerası | kamera sabit `useFixedDemoCamera=true` (RoomRunDirector l.110) | edit-mod zoom (master plan T3) |
| Cliff-tile oda canon | `IsoRoomBuilder` (yüzen ada) + `_Arena` canonical | mevcut |

## Konsolidasyon uyarısı (memory [[project-buildmode-editor-2026-06-13]])
Birden fazla harita-düzenleme sistemi var: `InPlayMapPaintOverlay` (DevTools) · BuildMode (UI/BuildMode) · `UnifiedMapDesigner` (Editor) · RoomPainter. F2 oda-editörü = bunları **tek oyun-içi deneyime** birleştirmek (sıfırdan kurma).

## Sıralama (öneri — MVP/golden-path sonrası, ama bu DEMO'nun WOW'u)
1. Edit-mod **zoom-out kamera** (T3) — editörde geniş görüş.
2. **"Regenerate Cliff" butonu** → `RoomCliffSolver.Solve(paintedFloorCells)` → cliff tilemap. (En yüksek "vay be" / en az risk: motor hazır.)
3. Prop paleti + Floor boyama F2 akışında konsolide.
4. (stretch) çiz → "Play" → kurduğun odada oyna (edit-to-play tam döngü).

⚠️ Bağlamadan önce: `RoomCliffSolver.Solve` + `InPlayMapPaintOverlay` tam API'sini doğrula; cliff sonucu hangi Tilemap'e/RoomData'ya yazılıyor netleştir. 8-yön + cliff-tile canon LOCKED.
