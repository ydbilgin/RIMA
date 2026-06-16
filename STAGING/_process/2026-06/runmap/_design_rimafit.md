# RIMA-Fit Branching Run Map — Design (demo-scoped)

> Sentez: RIMA-fit tasarımcı (Opus sub-agent). Tarih: 2026-06-16.
> Kaynak: TASARIM/ LOCKED docs (NLM canonical proxy — NLM auth EXPIRED, interaktif login gerektiriyor; drift-hiyerarşisi: NLM > local; LOCKED design docs canonical kabul edildi) + kod doğrulaması (`DungeonGraph.cs`, `RoomRunDirector.cs`, `RunMapOverlay.cs`, `MapPanelUI.cs`).
> Konu: M tuşu run-map'i SABİT-LİNEER'den her-run DEĞİŞEN, dallanan StS-tarzı node haritasına çevirme.

---

## 0. EN KRİTİK BULGU — Dallanan generator ZATEN VAR, sadece demo-flag onu kapatıyor

Sıfırdan sistem GEREKMİYOR. Kod tabanında **iki yol mevcut**:

- `DungeonGraph.Generate(seed, depthCount)` — **gerçek StS-tarzı dallanan graph üretir**: depth başına 2-3 lane (`NodeCountAtDepth`), çok-ebeveynli bağlantı (`ConnectRows` + `PreferredLaneOrder`), %55 Combat / %20 Elite / %15 Chest dağılımı. Entry=Combat (depth 0), Boss=tek (son depth). Lane-cross düzeni StS planar layout'a uygun.
- `DungeonGraph.BuildDemoSequence()` — sabit lineer zincir (Combat→Combat→Merchant→Combat→Boss→Combat).

`RoomRunDirector` şu an `forceDemoSequence = true` (satır 105) → lineer olanı çağırıyor. `RunMapOverlay.OnGUI` zaten `graph.childIds` üzerinden dalları çiziyor (depth-banded, lane-spread, edge-line) — **overlay dallanmayı render edebiliyor, sadece besleyen graph lineer**.

**Sonuç:** "her run değişen dallanan harita" = (a) `forceDemoSequence=false` + (b) `Generate`'i RIMA-canon'a göre zenginleştir + (c) her run farklı seed + (d) enum köprüsü/ikon. Refactor değil, mevcut iki-fonksiyonlu sisteme RIMA-canon enjeksiyonu.

---

## 1. CANON (TASARIM LOCKED) — RIMA harita modeli

- **Model:** "STS2 graph backend + Hades kapı ikonları + **fog-of-war**" (MAP_ITEM_SYSTEM §Özet; ROOM_STAGING_AND_MAP_VARIANTS; dungeon_act1_map Q4). **Full-map reveal REDDEDİLDİ** — "tam harita görünürse optimal path bellidir, seçim gerçek hissetmez". Bu = açık-karar'ın canon cevabı: **fog-of-war / aşamalı reveal, full-stratejik DEĞİL.**
- **Topoloji canon (Act 1, 15-node "LOCKED v1"):** ilk 3 node lineer (sistem öğretimi) → sonra dallanır → mid-act convergence (iki dal birleşir) → boss. Elite %20, Rest 2 guaranteed, Shop 1, Curse-Gate dal, Mystery dal. Boss = tek, en derin, alt-merkez silüet her zaman görünür.
- **Reveal kuralı (GATE_SOCKET + map_fragment_system):** current=tam görünür · current+1 (bitişik)=oda **tip ikonu** görünür · current+2="?" siluet · 3+ = görünmez · Boss=her zaman alt-merkez silüet. Map Fragment toplama → +1 adım daha açar (Scout mechanic).
- **"Full-run planı yapmadan ilerle" (kullanıcı sorusu):** RIMA canon bunu zaten BENİMSİYOR. Gerekçe canon-içi: full-map = lucky/unlucky değil ama optimal-path görünür → seçim sahteleşir; sadece Hades-kapı (1 ileri) = backend mantığı görünmez. **Hibrit kazanır:** lokal kararlar gerçek, backend tutarlı. → Demo reveal = **fog-of-war, 1-ileri-tip + Map Fragment ile 2-ileri** (full-stratejik DEĞİL).

---

## 2. DEMO İÇİN RIMA-FİT DALLANAN HARİTA SPEC (15-node canon → demo-makul ölçek)

Tam 15-node canon demo için ağır. Demo-makul = **6 kat (depth), her-run değişen**, canon iskeletini koruyarak:

| Depth | İçerik | Lane | Not |
|---|---|---|---|
| 0 | **Combat (Entry)** | 1 | Sabit başlangıç, tek node (öğrenme) |
| 1 | Combat | 1 | Hâlâ lineer (canon: ilk node'lar dal-yok) |
| 2 | Combat / Chest **dal başlar** | 2 | İlk gerçek seçim (üst=Combat-risk, alt=Chest-güvenli) |
| 3 | Elite / Merchant(Shop) | 2-3 | Risk-ödül çatalı: Elite=güçlenme, Shop=ekonomi |
| 4 | Combat / Event(Mystery) **convergence** | 2 | İki dal boss-öncesi birleşmeye başlar |
| 5 | **Boss** | 1 | Tek, en derin, convergence sonrası |
| (5+) | post-boss Combat (dual-class arena) | 1 | Demo-özel terminal node — opsiyonel, mevcut |

- **Kol-genişliği:** mid-depth max 3 lane (overlay okunaklılığı + StS hissi); depth 0/1/son = 1 lane (canon "ilk lineer + boss tek"). `NodeCountAtDepth` zaten `random.Next(2,4)` veriyor — depth-1'i de lineer yapmak için küçük guard ekle.
- **Node-tipi dağılımı (RIMA-fit, demo-kapsam):** Combat baskın (~%50), Elite %20 (canon), Chest ~%15, Merchant/Shop = guaranteed 1 (mid-act, canon "Shop 1"), Event(Mystery) = dal-only opsiyonel, **Curse-Gate = post-demo** (curse debuff sistemi demo-dışı), **Forge/Rest = post-demo** (combine + rest akışı demo-dışı). Boss = tek guaranteed.
- **Boss konumu:** her zaman en derin depth, tek node, tüm dallar oraya convergence. Alt-merkez silüet reveal'dan bağımsız her zaman görünür (canon).
- **Her-run değişim:** `runSeed` her `BeginRun`'da farklı (örn. `Environment.TickCount` veya `Guid`-hash) → topoloji + tip dağılımı + dal-genişliği run-by-run değişir. Canon "topoloji sabit, içerik random" Act-1-15-node'a aitti; **demo tezi = "her run procedural DEĞİŞEN" olduğu için demo'da TOPOLOJİ de seed'le değişsin** (kullanıcının "en can alıcı" tezi bunu istiyor; 15-node sabit-topoloji canon'u post-demo full-Act-1'e ait).

---

## 3. BÖLGE-GEÇİŞLERİ (Cliffs / Cave / Forest) — depth→bölge bandı

Canon bölgeler: Spacial Cliffs → Steel Aegis Cave → Mystifying Forest (Act ilerledikçe). dungeon_act1_map "depth band → tile pool" mantığını bölgeye genişlet:

- **Demo (tek-Act vertical slice):** Act 1 = Spacial Cliffs (cliff-tile yüzen ada canon'u ile birebir — bkz. room_canon_cliff_tile). Tek bölge yeterli; bölge-geçişi **görsel band** olarak depth ile ima edilir (depth 0-2 "eşik/temiz", 3-4 "derin", 5 "boss ritüel") — ayrı bölge-tile pool'u demo-dışı.
- **Post-demo:** depth-band → bölge eşlemesi (Cliffs 0-4, Cave 5-9, Forest 10-14) Act-geçişinde node-rengi/ikon-zemin tint'i değişir. Harita panelinde bölge başlıkları ("Spacial Cliffs") band ayracı olarak. Demo'da SKIP — tek bölge.

**Karar:** Demo = tek bölge (Cliffs), bölge-geçişi görsel-band ima. Çok-bölge run = post-demo (full multi-Act).

---

## 4. ENUM KÖPRÜSÜ BLOKERİ — kök neden + çözüm

İki enum çatışıyor (RUNMAP_UI_ASSET_PRODUCTION_DECISION §0 doğrulandı):

- `RIMA.RoomType` (runtime, `Core/RoomType.cs`): Combat, Elite, Boss, Chest, Merchant, Forge, Event, Curse, Corridor — **RunMapOverlay + RoomRunDirector bunu kullanıyor**.
- `RIMA.UI.Map.MapNodeType` (UI, MapNodeUI/MapNodeData): Combat, Elite, Rest, Boss, Event, Shop, CurseGate, Mystery, Entry — **MapPanelUI placeholder graph bunu kullanıyor**.

İki paralel harita-UI var: (1) `RunMapOverlay` (IMGUI, M tuşu, `RoomType` + renk-tint, **canlı graph'a bağlı** — demo'da çalışan bu) · (2) `MapPanelUI`+`MapNodeUI` (uGUI, ikon-sprite + `MapNodeType`, **placeholder 5-node graph, canlı director'a bağlı DEĞİL**).

**Köprü çözümü (demo-minimal, refactor değil):**
- Tek yönlü eşleme tablosu `RoomType → MapNodeType` (örn: Merchant→Shop, Chest→Chest [yeni MapNodeType anahtarı veya Event'e geçici map değil — gerçek Chest], Combat→Combat, Elite→Elite, Boss→Boss, Event→Mystery, Entry depth0→Combat). Tek `static Dictionary` veya `switch` — `RoomTypeToNode()` helper.
- **Demo kararı: TEK UI tut.** `RunMapOverlay` (M tuşu, canlı graph, çalışıyor) demo'nun harita'sı kalır → enum köprüsü demo için **gerekmez** (overlay zaten `RoomType` kullanıyor, MapNodeType'a hiç dokunmuyor). `MapPanelUI`/`MapNodeType` ikon-sprite hattı = post-demo (ikon asset'leri + TAB panel). Bu, blokeri demo-yolundan ÇIKARIR.
- **Post-demo:** enum köprüsünü kalıcı kur, `MapPanelUI`'yi canlı director graph'ına bağla (placeholder yerine `director.Graph`'tan `MapGraphData` üret), ikon-sprite swap (RUNMAP_UI_ASSET §3 NodeIconLibrary). RoomType↔MapNodeType drift'i tek `RoomTypeToNode` dict'inde netleşsin.

**Renk-canon notu:** `RunMapOverlay.ColorFor` mevcut tint'ler canon-yakın (Elite=mor, Boss=kırmızı, slate-default) ama Boss saf-kırmızı; canon ember/void-mor/slate. Cyan yalnız current-node border'da (zaten öyle). Demo için kabul-edilebilir; ikon-sprite geçişinde canon palet uygulanır (post-demo).

---

## 5. REVEAL MODELİ — demo implementasyonu

Canon = fog-of-war 1-ileri-tip. Mevcut `RunMapOverlay` **tüm node'ları tam gösteriyor** (fog YOK). Demo-fit reveal:

- **Demo-minimal (önerilen):** current=parlak+cyan border (var) · ziyaret-edilmiş=koyu tint · current+1 (childIds)=tip-renk görünür · current+2="?" gri silüet · 3+=çizilmez · Boss=her zaman alt-merkez görünür. `RunMapOverlay` döngüsüne node başına `revealLevel` hesabı (current'tan BFS-mesafe) + tint/“?” ekle. ~30-40 satır, refactor değil.
- **Map Fragment Scout (canon):** oda-clear'da fragment → +1 reveal. Demo'da fragment-pickup akışı varsa bağla; yoksa demo-minimal reveal yeter, fragment = post-demo polish.
- **Full-map gösterme YOK** (canon ihlali). Kullanıcının "full-run planı yapmadan ilerle" sorusunun cevabı = **fog-of-war, canon ile birebir**.

---

## 6. AÇIK KARAR CEVABI (kullanıcı sorusu)

> "Oyuncu tüm haritayı baştan mı görür (StS-tam-stratejik) yoksa aşamalı/fog-of-war reveal mi?"

**CEVAP: Fog-of-war / aşamalı reveal.** Canon kesin (MAP_ITEM_SYSTEM, GATE_SOCKET, map_fragment_system hepsi LOCKED): full-map = optimal-path görünür → seçim sahteleşir. RIMA = "lokal kararlar gerçek + backend tutarlı". 1-ileri-tip görünür + Map Fragment ile 2-ileri. **StS-tam-stratejik (tüm harita baştan) RIMA-canon DEĞİL.** Demo bunu fog-of-war ile gösterirse hem canon-sadık hem "her run keşfet" tezini güçlendirir.

---

## 7. ÜRETİM SIRASI (orchestrator → cx/Opus dispatch için)

1. **`DungeonGraph.Generate` RIMA-canon enjeksiyon:** depth-0/1 lineer guard, mid max-3 lane, Merchant guaranteed-1, Elite %20, Event dal-only, Forge/Curse/Rest demo-dışı. (cerrahi, mevcut fonksiyona patch)
2. **`RoomRunDirector`:** `forceDemoSequence=false`, `runSeed` her BeginRun'da rasgele (her run değişir). Demo terminal post-boss node korunur.
3. **`RunMapOverlay` reveal katmanı:** current'tan BFS-mesafe → revealLevel → tint/"?"/Boss-always (§5).
4. **Enum köprüsü:** demo'da SKIP (tek UI = RunMapOverlay/RoomType). Post-demo: `RoomTypeToNode` dict + MapPanelUI canlı-bağla + ikon-sprite.
5. **Verify:** Play → M → her run farklı topoloji + dallar + fog. read_console 0-error.

**Demo scope-lock notu:** Bu spec mevcut iki-fonksiyon sistemine cerrahi enjeksiyon — yeni sistem/refactor YOK (Karpathy #2/#3). Forge/Curse/Rest/Map-Fragment-Scout/çok-bölge/ikon-sprite-panel = post-demo backlog.
