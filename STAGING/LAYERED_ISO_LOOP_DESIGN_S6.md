# LAYERED ISO SCENE + PORTAL/PREVIEW/ORB LOOP — DESIGN (S6, Opus)

**Amaç:** `_IsoGame`'i kullanıcının vizyonuna göre temiz kur: floor=oynanan iso-sınırlı zemin (karakter matematiksel sınır dışına çıkamaz) / altında havada asılı cliff'ler / derinlik için ara (parallax) cliff'ler / en altta bg / cliff-bg arasında her run değişen GERÇEK preview odalar (N portal = N gerçek referans oda görünür) / portaldan geçince orb'a dön → git.

**Status:** DRAFT — ax (design/visual) + cx (technical) + Opus-judge review BEKLİYOR. Onay sonrası M1'den başla.

---

## 0. MEVCUT SAHNE (doğrulandı, _IsoGame)
- **IsoGrid** (Grid, iso cellSize ~0.96×0.585):
  - `Ground` (Tilemap) — floor451 ~560 hücre = oynanan zemin / iso elmas.
  - `Walls` (Tilemap + TilemapCollider2D + CompositeCollider2D + WallOcclusionFader) — collision.
  - `Obstacle` (+collider) + `ObstacleInstances` (6 sütun).
  - `CliffRing` (CliffAutoPlacer) — ada kenarına cliff yerleştiriyor.
  - `Cyan Crack Accent Lights` (28 Light2D) + `Cyan Crack Overlay Sprites` (28).
- **Main Camera** (PixelPerfectCamera 640×360, CameraFollow) → child `Void_BG` (tek mor sprite, kamera-kilitli, en arka, localZ=20).
- **Systems:** RuntimeRoomManager (useAuthoredSceneRoom=true _IsoGame'de), **DungeonGraph (12-node STS branching graph, Navigate(dir)+CurrentNode.exits HAZIR)**, DraftManager, RoomTransitionFX, LargeDungeonMapPainter, PlayerClassManager.
- **DoorNorth(aktif)/DoorSouth(pasif):** DoorTrigger + GateBehavior = mevcut oda-çıkış.
- **Player:** PlayerController, Health, PlayerAttack, IsoSorter, YSortBehaviour, CapsuleCollider2D.

## A. KATMAN DERİNLİK MODELİ (temiz yeniden kur)
Hedef: derin void'de yüzen iso ada. Derinlik = ekran-Y kaydırma + sorting layer + ölçek + karartma (gerçek 3D yok). Kamera yakın gameplay zoom.

ÖN→ARKA (ne neyi örter), sorting layer + yerleşim:
1. **Entities** (Player/mob/portal) — sortingLayer "Entities", custom-axis (0,1,0) pivot Y-sort.
2. **Floor (Ground)** — adanın üstü, oynanan iso elmas. Entities'in altı.
3. **Island Cliffs (CliffRing)** — floor kenarından AŞAĞI sarkan kaya yüzleri (ön/yan görünür). Floor'un altı, preview-band'in önü. sortingLayer "Cliffs".
4. **Preview Room band** — N gerçek referans oda, ada ALTINDA void'de yüzer (cliff ile bg arası). Küçük ölçek (~0.45-0.65), karartılmış (<%25), mob-suz. sortingLayer "PreviewBand". Yatayda portal sayısı/konumuna hizalı. Savaşta kamera-altı/çok-dim; oda temizlenince kamera hafif zoom-out/pan-down → oyuncu N preview adayı portal altında GÖRÜR.
5. **Depth Cliffs (parallax)** — preview-band ile bg arasında derinlik için birkaç ek cliff silüeti. Daha küçük/koyu. sortingLayer "FarParallax".
6. **Void BG** — en arka mor void (kamera-child). sortingLayer "Background", order -100.

## B. İSO FLOOR SINIRI (karakter dışarı çıkamaz)
Floor = iso elmas (Ground footprint). Player ona clamp'lenir.
**Seçim: çevre sınır collider'ı** — Ground'un dolu hücrelerinin DIŞ kenarına bir PolygonCollider2D (veya EdgeCollider2D loop), player gövde collider'ının çarptığı fizik katmanında.
- Artı: saf fizik, per-frame clamp kodu yok, knockback'i doğal kaldırır, "matematiksel sınır" ile uyumlu.
- Demo: Ground bounds'tan 4-köşe iso elmas polygon (N/E/S/W uç hücreler). Organik oda gelince → dolu-hücre kenarı trace.
- Reddedilen: PlayerController runtime clamp (knockback/dash ile çatışır), ters tilemap collider (ağır).
- NOT: mevcut Walls/CompositeCollider2D bir sınır sağlıyor olabilir → önce onu ölç, yetiyorsa onu iso-elmasa indirge, yoksa yeni boundary.

## C. GRAPH-AWARE ODA AKIŞI (kritik blocker)
DungeonGraph run'ı modelliyor; eksik = oda İÇERİĞİ yüklemesi graph'a bağlı değil + portal'lar graph exit'lerine bağlı değil.
- **RunController** (RuntimeRoomManager'ı genişlet / yeni): oda-temizlenince `DungeonGraph.CurrentNode.exits` oku → her exit için 1 portal spawn, paylaşılan `ExitChoice {index, DoorDirection dir, int targetNodeId, RoomType type}` ile bağla.
- Player portal i'den geçer → (orb travel) → `DungeonGraph.Navigate(dir)` → yeni CurrentNode için oda içeriği yükle (RoomType'a göre procedural / authored).
- RuntimeRoomManager.OnPlayerEnteredDoor→Navigate paterni zaten var → onu kullan.

## D. PORTAL (bind + count)
- `Portal.cs` genişlet: graphDirection, targetNodeId, roomType, portalColor, `Configure(ExitChoice)`, child rune-icon SR (codex art per-type).
- Portal sayısı + konumu = DungeonGraph exits (RASTGELE DEĞİL). Eşleşen ekran yönüne (N→kuzey kenar) yakın ada kenarına yerleştir.
- Enter: OnEntered(ExitChoice) → RunController travel+load yapar.

## E. PREVIEW ADALAR (void band'de gerçek odalar)
- Her exit i için preview band'de (katman 4) bir `RoomPreviewIsland`, portal i altına yatay hizalı.
- Demo: statik düşük-detay prefab (floor+cliff+1 prop), koyu/dim, mob-suz, collider/AI yok. Portal hover → aydınlat+ölçekle.
- Portal i ile AYNI ExitChoice'a bağla (drift olamaz).
- Final: RoomData'dan runtime composer ile data-driven (ertelendi).

## F. ORB TRAVEL (animasyon sonra)
- `PortalTravelDirector`: portal enter → player'ı orb'a morph (procedural squash+fade) → void'de Bezier yay ile seçili preview ada konumuna → crash flash → gerçek oda yükle + player restore.
- `PlayerTravelVisualState`: PlayerController + PlayerAttack disable, visualRoot gizle, rb/collider cache/restore. Kamera orb'u önden takip.
- Coroutine + AnimationCurve, DOTween YOK. ~0.6s. (Sanat/animasyon parlatma sonra — kullanıcı "sonradan animasyonla ayarlayacaz".)

## BUILD MİLESTONE'LARI
- **M1 (temel, düşük-risk):** temiz katman derinlik modeli + sorting layer'lar + iso boundary collider. Görsel + collision F5 doğrula.
- **M2 (loop omurgası):** RunController graph-driven portal (count+bind) + portal enter'da sonraki odayı yükle (önce orb'suz, instant). Branching çalışır doğrula.
- **M3 (derinlik preview):** preview adalar band'de, portala bağlı, oda-temizlenince kamera reveal.
- **M4 (juice):** orb travel morph+trail+crash, portal/reward art swap-in, hover-glow.

## REVIEWER'LARA AÇIK SORULAR (ax / cx / judge)
1. **Boundary:** 4-köşe elmas polygon vs trace dolu-hücre kenarı — demo + gelecek organik odalar için hangisi? Mevcut Walls collider yeniden kullanılabilir mi?
2. **Preview reveal:** oda-temizlenince kamera zoom-out vs pan-down — 640×360 yakın zoom'da "derinlik"i en iyi hangi okutur, combat readability'yi bozmadan?
3. **Preview detay:** demo'da preview adalar gerçek mini-oda geometrisi mi yoksa silüet mi? (spec: gerçek layout, düşman gizli)
4. **Loader:** graph-driven için RuntimeRoomManager (legacy, sahnede, OnPlayerEnteredDoor→Navigate var) vs Systems/Map RoomLoader (linear) — hangisini omurga yap?
5. **Depth cliffs:** gerçek cliff sprite scale/karart vs basit silüet şekil?
6. **CLIFF OVERFLOW (gece kuyruğundan):** cliff sprite (ref_kit_b 128×192 = 2×3 hücre) floor tile'dan (64=1 hücre) büyük → ada silüetini taşırıyor. Çözüm? (scale-down / crop / per-cell clip / void'e vs floor'a yerleştir). Mevcut CliffRing/CliffAutoPlacer ne kullanıyor, taşma var mı?
7. **Tek-yön cliff yeter mi** 8-yön yerine (high-top-down iso'da arka/yan yüzler görünür mü)? görsel-doğruluk vs üretim-basitliği.
