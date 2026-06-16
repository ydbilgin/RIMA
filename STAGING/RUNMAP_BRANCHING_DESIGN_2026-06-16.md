---
status: DECISION
tarih: 2026-06-16
ozet: "Run-map SABIT-LINEER -> her-run procedural DALLANAN StS-tarzi node haritasi: topoloji + reveal + generation + implementasyon plani"
demo: 2026-06-19 (golden-path)
---
# RIMA Run-Map Branching Design (KARAR + PLAN)

> Sentez kaynagi: 3 arastirma dosyasi (`_research_roguelite.md` / `_audit_code.md` / `_design_rimafit.md`) + LOCKED canon dogrulama (`GATE_SOCKET_AND_MAP_REVEAL_BLUEPRINT`, `dungeon_act1_map.md` Karar #62) + kod denetimi.
> NLM auth EXPIRED (interaktif login gerek) -> canon LOCKED design doc'lardan BIREBIR dogrulandi (drift: NLM > local; LOCKED docs canonical proxy).
> Konu: M tusu "KOSU YOLU" overlay'i SABIT-LINEER (0->1->2->3->4->5) -> HER RUN procedural DEGISEN, dallanan, oyuncu-yol-secer harita.

---

## 0. EN KRITIK BULGU (kod denetimi) — sistem ZATEN VAR, demo-flag kapatiyor

Sifirdan graf/branch yazmaya GEREK YOK. Kod tabaninda iki yol mevcut:
- `DungeonGraph.Generate(seed, depthCount)` — **gercek StS-tarzi dallanan DAG uretir** (depth basina 2-3 lane, cok-ebeveynli fork+merge, orphan-free; %55 Combat/%20 Elite/%15 Chest; entry=Combat depth0, boss=tek son-depth). EditMode testleri gecer (determinist+orphan-free).
- `DungeonGraph.BuildDemoSequence()` — sabit lineer zincir (gordugumuz 0->5).

`RoomRunDirector.cs:105` `forceDemoSequence = true` (sahne `_Arena.unity:3938 forceDemoSequence:1`) lineer'i cagiriyor. `RunMapOverlay.OnGUI` dallari ZATEN ciziyor (depth-band satir + lane-spread + parent->child edge). Navigasyon ZATEN dal-secimine bagli (`AdvanceTo`/`TryEnterDoor`/`BuildExitDoors` her child icin kapi). **Sonuc: dallanma = generator'i acmak + RIMA-canon enjeksiyon + per-run seed + reveal katmani. Refactor DEGIL, cerrahi patch.**

---

## 1. KARAR — Run-Map TOPOLOJISI (demo-olcek)

Canon Act-1 = 15-node (LOCKED #62). Demo icin agir -> canon iskeletini koruyan **6-depth, her-run degisen** olcek:

| Depth | Icerik | Lane | Not |
|---|---|---|---|
| 0 | Combat (Entry) | 1 | Sabit baslangic tek-node (ogrenme; canon "ilk node lineer") |
| 1 | Combat | 1 | Hala lineer (canon "ilk 3 node dal-yok, sistem ogretimi") |
| 2 | Combat / Chest — **dal baslar** | 2 | Ilk gercek secim (risk vs guvenli) |
| 3 | Elite / Merchant(Shop) | 2-3 | Risk-odul catali (Elite=guc, Shop=ekonomi); Shop guaranteed-1 |
| 4 | Combat / Event(Mystery) — **convergence** | 2 | Dallar boss-oncesi birlesir |
| 5 | **Boss** | 1 | Tek, en derin, tum dallar buraya akar; alt-merkez siluet hep gorunur |
| (5+) | post-boss Combat (dual-class arena) | 1 | Mevcut demo-terminal node — korunur |

- **Dallanma kurali:** depth 0/1/son = 1 lane (canon: lineer giris + tek boss). Mid-depth max **3 lane** (overlay okunakliligi + StS hissi). Node basina 1-3 cikis; yollar kesismez (planar). En az 2 baslangic-secimi depth-2'de acilir.
- **Node-mix (demo-kapsam):** Combat baskin (~%50) · Elite %20 (canon) · Chest ~%15 · Merchant/Shop = guaranteed-1 (mid) · Event(Mystery) = dal-only opsiyonel · Boss = tek guaranteed. **Forge / Curse-Gate / Rest = post-demo** (combine/curse/rest akislari demo-disi).
- **Bolge:** Demo = tek bolge **Spacial Cliffs** (cliff-tile yuzen ada canon). Cliffs->Cave->Forest gecisi = post-demo (full multi-Act); demo'da depth-band gorsel-ima yeterli.

---

## 2. KARAR — REVEAL MODELI = **FOG-OF-WAR (asamali), full-map DEGIL**

Acik soru ("tum haritayi bastan mi gorur yoksa fog mi?") -> **CANON FOG-OF-WAR.** Dogrudan LOCKED doc dogrulamasi:
- `GATE_SOCKET ... §Map Reveal Rule`: "next 1-2 nodes" = current'tan graf-kenari · Step1=dogrudan cikislar (oda-clear sonrasi netlesir) · Step2=fragment ile · **Hidden: "do not reveal far route / far-node count / hidden node icons"**.
- `dungeon_act1_map Q4 (CLOSED)`: Map UI = **HIBRIT** — StS macro-panel (TAB) + Hades-style sol-ust minimap. Map Fragment ile +1 reveal (Scout).

**Gerekce:** Canon kesin — full-map = optimal-path gorunur -> secim sahteleshir. RIMA tezi "lokal kararlar gercek + backend tutarli". Fog-of-war hem canon-sadik hem "her run kesfet" tezini guclendirir.

> ⚠️ KARAR-CATISMASI NOTU (orchestrator'a): `_research_roguelite.md` demo-sunum-degeri icin **full-map** onerdi (izleyici dallanmayi tek bakista gorsun). LOCKED canon bunu REDDEDIYOR. **Cozum = canon kazanir** ama demo-pragmatik koruma: **demo reveal = fog-of-war ama "current+1 tip-ikon gorunur + Boss-siluet hep gorunur"** -> izleyici yine "dallanma + secim var" gorur (current node'dan 2-3 dal ACIK cizilir), uzak rota fog. Sunumda "her run farkli" tezi seed-degistir + 2-ileri dal ile yine NET. Full-stratejik-plan kaybi = kabul (canon + secim-gercekligi kazanir).

**Demo reveal implementasyon:** current=parlak+cyan border (var) · ziyaret=koyu tint · current+1 (childIds)=tip-renk gorunur · current+2="?" gri siluet · 3+=cizilmez · Boss=hep alt-merkez siluet. `RunMapOverlay` dongusune node-basi `revealLevel` (current'tan BFS-mesafe) + tint/"?" ekle (~30-40 satir). Map Fragment Scout (+1) = post-demo polish.

---

## 3. KARAR — GENERATION algoritmasi (her-run procedural + seed)

StS DAG modeli (zaten `Generate`'de): seed -> depth-band node-sayisi + tipleri + lane-layout + fork/merge. **Demo'da TOPOLOJI de seed'le degissin** (kullanicinin "her run procedural DEGISEN" / "en can alici" tezi bunu istiyor).

> ⚠️ CANON NUANS: `dungeon_act1_map` full-Act-1 icin "**topoloji SABIT, icerik random**" der (15-node fixed). Bu canon **full-Act-1'e** ait. **Demo-karar: demo'da topoloji DE seed'le degisir** (tez geregi) — full-Act-1 sabit-topoloji canon'u post-demo'ya saklanir. (warn-then-apply: canon-sapma bilincli + tez-gerekce ile.)

- **Seed:** `runSeed` her `BeginRun`'da rastgele (`Environment.TickCount` veya `Guid`-hash). Determinist (ayni seed=ayni harita -> paylasilabilir/demo-tekrar). Sunum-jesti: "seed degistir -> bak, baska harita".
- **Generate patch:** depth-0/1 lineer guard · mid max-3 lane · Merchant guaranteed-1 (mid) · Elite %20 · Event dal-only · Forge/Curse/Rest exclude (demo). Mevcut fonksiyona enjeksiyon, yeni fonksiyon degil.

---

## 4. KARAR — "ENUM KOPRUSU" blokeri = demo'da DEVRE-DISI (yanlis bloker)

Iki paralel UI: (1) `RunMapOverlay` (IMGUI, M-tusu, `RIMA.RoomType` + renk-tint, **canli graph'a bagli — demo'da calisan BU**) · (2) `MapPanelUI`+`MapNodeUI` (uGUI, ikon-sprite + `MapNodeType`, **placeholder 5-node, canli director'a bagli DEGIL — olu/legacy**).

"Enum koprusu" = `RoomType <-> MapNodeType` esleme; SADECE (2) ikon-sprite hatti icin gerekir. `RunMapOverlay` `MapNodeType`'a hic dokunmaz. **Demo karar: TEK UI tut = `RunMapOverlay`. Enum koprusu demo-yolundan CIKAR.** Post-demo: `RoomTypeToNode` dict + MapPanelUI'yi canli `director.Graph`'a bagla + ikon-sprite + TAB panel.

---

## 5. IMPLEMENTASYON PLANI (dosya-dosya, min-degisiklik) — cx/Opus dispatch icin

| # | Dosya | Degisiklik | Olcek |
|---|---|---|---|
| 1 | `RoomRunDirector.cs:105` + `_Arena.unity:3938` | `forceDemoSequence = false` | 1 deger (aninda dallanma) |
| 2 | `RoomRunDirector.cs` (BeginRun) | `runSeed` her run rastgele (TickCount/Guid-hash) | 1-2 satir (her-run degisir) |
| 3 | `DungeonGraph.Generate` | RIMA-canon enjeksiyon: depth0/1 lineer guard · mid max-3 lane · Merchant guaranteed-1 · Elite %20 · Event dal-only · Forge/Curse/Rest exclude | cerrahi patch (mevcut fn) |
| 4 | `RunMapOverlay.OnGUI` | reveal katmani: BFS-mesafe -> revealLevel -> tint/"?"/Boss-always-siluet | ~30-40 satir |
| 5 | VERIFY | Play -> M -> her run farkli topoloji+dal+fog · `read_console` 0-error · golden-path bozulmadi (AdvanceTo dogru dala goturuyor) | test |

- **DEMO-KAPSAM (19 Haz, golden-path):** #1-#5 hepsi. Cerrahi, scope-lock NO-REFACTOR. Mevcut iki-fonksiyon sistemine enjeksiyon (yeni sistem yok).
- **POST-DEMO backlog:** enum koprusu (`RoomTypeToNode`) + MapPanelUI canli-bagla + ikon-sprite (PixelLab sembol) + TAB macro-panel + Hades sol-ust minimap + Map Fragment Scout (+1 reveal) + Forge/Curse/Rest node-tipleri + cok-bolge band (Cliffs/Cave/Forest) + full-Act-1 15-node sabit-topoloji.

---

## 6. ASSET IHTIYACI

- **Demo:** YOK (sifir yeni asset). `RunMapOverlay` IMGUI renk-tint kutu/cizgi kullanir — kod-ureti. (Renk-canon yakin: Elite=mor, Boss=kirmizi; ikon-sprite gecisinde tam palet post-demo.)
- **Post-demo:** PixelLab node-sembolleri (Combat/Elite/Boss/Chest/Merchant/Event ikonlari) + Unity edge-cizgi/chrome (pipeline: chrome=ChatGPT, sembol=PixelLab, cizgi=Unity) + gate-socket sprite (GATE_SOCKET blueprint: combat/elite/chest/forge/merchant/event/curse/boss/unrevealed varyant).

---

## 7. ACIK SORULAR (orchestrator'a — kodlamadan once teyit)
- [TEYIT] Demo topolojinin seed'le degismesi (canon "sabit-topoloji"yi demo'da bilincli kiriyoruz) — kullanici "her run procedural DEGISEN" dedigi icin EVET varsayildi; warn-then-apply geregi orchestrator onaylasin.
- [POST-DEMO] NLM auth yenilenince canon node-mix/region/element-sembol son dogrulama (auth EXPIRED su an).
