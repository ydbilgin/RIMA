# TASK 11 — Build Mode FONKSİYONEL bug fix (kullanıcı canlı-test + Explore tanısı)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
UNITY ERROR CHECK: iş bitince read_console (Error+Warning); kendi hatanı ÇÖZ, ilgisiz/önceden-var hatayı BİLDİR (silme), raporda console durumunu yaz.
GRAPHIFY: cross-file soruda önce graphify query (graph.json: `STAGING/_process/2026-06/graphify_fullmap/graphify-out/`).

## Bağlam
Kullanıcı F2 Build Mode'da canlı-testte 4 bug buldu; Explore tanısı kök-nedenleri çıkardı (aşağıda dosya:satır). Karar: `STAGING/CHATGPT_REV2_COUNCIL_DECISION_2026-06-17.md` (Build = polish, grid GEOMETRİ DEĞİŞMEZ). Bunlar fonksiyonel bug — geometri değil snap/paint/asset doğruluğu.

## Fix'ler (her birini runtime doğrula)

### #1 — Floor/Walkable boyanamıyor (KRİTİK)
Kök-neden: `BuildTileBrushController.cs:253` `HandleCursor()` her vuruşu `IsPointerOverUi()` (:749-752) ile kesiyor; bu bare `EventSystem.IsPointerOverGameObject()` ve fırça-UI canvas tam-ekran ScreenSpaceOverlay+GraphicRaycaster (sortingOrder 5001) → **dünya viewport'unda bile TRUE** → boya hiç çalışmıyor.
- Fix: kontrolü **gerçek UI panel rect'ine** daralt (viewport üstündeyken boyamaya izin ver). RectTransformUtility.RectangleContainsScreenPoint ile sadece fırça-panel/asset-panel bölgesini blokla; viewport'u serbest bırak. Tam-ekran raycaster'ı bloklamasın.

### #2 — Tile snap "tam oturmuyor" (grid↔tile hizası)
Snap math DOĞRU (`BuildPlacementController` `GetCellCenterWorld`) ama grid cellSize=0.96×0.59 çizilen diamond ile tile-sprite footprint'i eşleşmiyor → tile snap'liyor ama görsel hizalanmıyor (2D'de belli).
- Runtime'da TESPİT ET: komşu floor tile'ları boya → **üst üste mi biniyor / arada boşluk mu / çizgi-diamond tile'dan farklı boyutta mı?**
- Fix: grid overlay diamond'ı ile tile-sprite world-footprint'ini **hizala** (komşu tile'lar dikişsiz tessellate olsun, çizilen diamond = yerleşen tile). Geometri-açısını/iso-konseptini DEĞİŞTİRME — sadece çizim-boyut/tile-scale eşleşmesini doğru yap.

### #3 — Hayalet asset (wooden barrel + benzerleri)
`Data/Brush/Props/Barrel/barrel_001.asset` icon/worldSprite/variantSprites NULL ama katalog `enabled=true` listeliyor → browser glyph gösteriyor, yerleştirme PATLIYOR (`BuildModeAssetCatalog.BuildProps()` ~114-128; thumbnail :194-201).
- Fix (GENEL): katalog **sadece sprite'ı resolve olan (placeable) propdef'leri listelesin** → sprite'sız tüm hayalet entry'ler gizlenir. (Barrel'a sprite varsa wire et; yoksa filtrele.) Tek seferde barrel + diğer kırık entry'leri çözer.

### #4 — Grid kısıtlı çizilmiş (kullanıcı daha geniş istiyor)
`BuildPlacementController.cs:159` `GridOverlayRadius=14` → sabit 29×29 pencere (GC-cap). Kullanıcı daha geniş authoring alanı istiyor.
- Fix: pencereyi **makul genişlet** (ör. tipik oda + edit-margin'i kapsayacak; radius'u ölçülü artır) — perf'i ölçüp aşırıya kaçma. Sınırsız YAPMA (per-frame GC). Gerçek perf-maliyeti varsa raporla, makul bir değerde bırak.

## Kısıt
- Cerrahi: yukarıdaki dosyalar. Grid GEOMETRİ (iso-açı, oda-dışı uzama) DEĞİŞMEZ. Working-copy/undo/8-8-assert KORUNUR. Combat/oyun mantığına dokunma.
- git'e DOKUNMA.

## VERIFY (RUNTIME — gerçekten BUILD MODE'a gir, F2)
Full-flow veya `_Arena`'dan F2 Build Mode aç (GATE fix'i sayesinde çalışıyor):
- **#1:** floor tile seç → viewport'a BOYA → tile YERLEŞİYOR mu? Walkable layer'a boya → yerleşiyor mu? (screenshot kanıt)
- **#2:** 2-3 komşu floor tile boya → dikişsiz + grid-diamond ile hizalı mı? overlap/gap yok mu?
- **#3:** asset browser'da hayalet barrel YOK mu (veya yerleşiyor mu)?
- **#4:** grid penceresi makul genişledi mi.
- `read_console` 0-error. 8/8 placement assert hâlâ PASS.

## ÇIKTI (E1: ≤10 satır)
Evidence + screenshot → `STAGING/_process/2026-06/demo_fix_tasks/DONE_11_buildmode.md`. Dönüşte: değişen dosyalar + her bug (#1-#4) runtime-PASS/durum + #2 tespiti (overlap/gap/size) + console + 8/8 assert. Rapor içeriğini gömme.
