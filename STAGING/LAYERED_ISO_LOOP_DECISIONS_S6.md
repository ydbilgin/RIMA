# LAYERED ISO LOOP — LOCKED DECISIONS (3-hakem review sonrası, Opus sentez)

**Girdi:** `LAYERED_ISO_LOOP_DESIGN_S6.md` + ax (görsel) + cx (`REVIEW_DESIGN_CX.md`, teknik) + Opus-judge (mimari) review'ları + sahne doğrulaması.

## SAHNE GERÇEĞİ (doğrulandı, _IsoGame)
- Loader = **SADECE RuntimeRoomManager** (count=1), Systems.Map.RoomLoader sahnede YOK (count=0). DungeonGraph + DraftManager var. 2 DoorTrigger (North aktif, South pasif), 0 Portal.
- Grid Isometric cellSize (0.96,0.59). Floor 560 hücre, elmas tips: S(0.24,1.32) N(4.08,14.77) E(13.20,9.21) W(-8.88,6.87), merkez ~(2.16,8.04).
- "Boundary" fizik layer'ı var + Player onunla collide. WalkabilityMap + PlayerController.IsWalkableWorld floor-clamp KODDA var.

## KİLİTLİ KARARLAR
1. **LOADER = RRM (_IsoGame'de).** Hakemler "RoomLoader kullan" dedi ama onlar iki-loader varsaydı; gerçek: _IsoGame RRM-only + graph-aware (OnPlayerEnteredDoor→Navigate→StartRoom ZATEN var) + çalışan demo bunun üstünde. RoomLoader'ı _IsoGame'e SOKMA (gereksiz büyük refactor, demo'yu riske atar). Obsolete notu scene-scoped rolü yansıtacak şekilde güncellenecek (RoomLoader=procedural/linear spine diğer sahneler; RRM=authored-iso demo spine). **Tek path:** Portal.OnEntered → RRM.NavigateThroughPortal(ExitChoice) → (orb travel) → RRM oda içeriği swap. Başka yerden DungeonGraph.Navigate çağrısı YOK.
2. **BOUNDARY = mevcut WalkabilityMap + PlayerController.IsWalkableWorld birincil** (kod-clamp zaten var) + knockback/dash backstop olarak 4-köşe iso-elmas EdgeCollider2D, "Boundary" layer'da. Walls collider'ı boundary olarak kullanma. Organik oda gelince: dolu-hücre dış kenar trace (tek seferlik util). ÖNCE WalkabilityMap'in _IsoGame'de aktif/dolu olduğunu doğrula.
3. **SORTING = 3-tier.** Yeni katman icat etme; RoomDepthStack'in `Floor`/`Ground`/`BackwallLandmark` + `Entities` adlarını kullan. Band root'larına (preview/parallax/cliff) IsoSorter/YSortBehaviour EKLEME — sabit layer/order. Portal = `Entities` layer + **taban pivot** + hardcoded sortingOrder KALDIR (mevcut `Portal.cs:46 sortingOrder=5` = custom-axis ihlali, düzelt). Void-bg + far-parallax + preview band = Background tarafı (negatif order).
4. **PORTAL.** `Portal.Configure(ExitChoice, Action<ExitChoice>)` ekle; DestinationType sadece visual fallback. One-shot guard: `bool entered` + collider disable + `IsTravelling` guard. Sayı/konum = `DungeonGraph.CurrentNode.exits` (RASTGELE DEĞİL — PortalSpawnController random path graph-mode'da bypass). **≤3 portal** (ax: 4+ kalabalık), yay düzeni, adalar dikey hizada altta.
5. **PREVIEW = `ExitChoice` REFERENCE tip** portal↔preview paylaşır (value-copy drift YOK). Demo: elle `RoomPreviewIsland` prefab + `PreviewOnlySanitizer` (Collider2D/AI/Health/spawner strip). Final: composer scale=0.5 + mob-suz preset (hardcoded prefab değil — demo→final rewrite önle). Combat'ta %15-20 parlaklık + void-purple sis; temizlenince aç. Yatay min-spacing = ada_genişliği×1.4, yakın kenarda lane-offset.
6. **ORB TRAVEL tek otorite = PortalTravelDirector.** RRM travel sırasında self-teleport ETMEZ (`travelActive ? skip`). Cache/restore TAM liste: PlayerController.enabled, PlayerAttack.enabled, Collider2D.enabled, rb.bodyType/linearVelocity, SpriteRenderer enabled'lar, Health immunity, IsoSorter/YSortBehaviour aktiflik. visualRoot serialize (yoksa GetComponentInChildren<Animator>().transform fallback). Coroutine+AnimationCurve+Bezier, ~0.6s, DOTween YOK. RoomTransitionFX fade ile EXCLUSIVE.
7. **CLIFF = tek ön-yüz (S/SE/SW)** yeter (high-top-down arka/yan gizli). CliffAutoPlacer zaten floor-cell + top-pivot + southClearCells=5 overflow filtresi yapıyor → büyük sprite scale-down + per-cell crop. **DEPTH CLIFFS = koyu monokrom mor SİLÜET** (küçültülmüş gerçek sprite shimmer yapar), collision/y-sort YOK.
8. **ax EKSTRALAR:** kamera-BG arası **void toz/partikül parallax** katmanı (yoksa void "boş arka plan"); reveal = ağırlık **pan-down** + hafif float-zoom (integer-snap kaçın).

## BUILD ORDER (revize, M0 eklendi)
- **M0 (KARAR — bu belge):** Loader otoritesi = RRM (_IsoGame). RoomLoader sokulmaz. Obsolete not güncellenir. ✓ kararlaştırıldı.
- **M1:** 3-tier sorting kurulumu + boundary (WalkabilityMap doğrula + 4-köşe backstop collider) + katman derinlik düzeni (cliff/preview-band/parallax/bg root'ları + void partikül). F5 görsel + "floor dışına çıkamıyor" doğrula.
- **M2:** Graph-driven portal: DoorTrigger→Portal, count/bind = exits, ExitChoice, one-shot enter → RRM içerik swap (önce orb'suz instant). Branching çalışır doğrula.
- **M3:** Preview adalar band'de (sanitized prefab → composer-scale=0.5), portala ExitChoice-reference bağlı, oda-temizlenince pan-down reveal.
- **M4:** Orb travel morph+trail+crash + portal/reward art import+wire + hover-glow + combat dim/tint.

## EN RİSKLİ 3 (izlenecek)
1. Loader path tutarlılığı (RRM tek otorite kalmalı; ikinci Navigate çağrısı girmesin).
2. Boundary: WalkabilityMap aktif değilse player floor'dan çıkar → M1'de İLK doğrula.
3. Portal/preview double-state: one-shot guard + ExitChoice reference şart.
