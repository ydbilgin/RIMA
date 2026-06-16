# Run-Map Kod Denetimi — Neden Sabit-Lineer + Dallanma Hook Noktalari

Tarih: 2026-06-16 · Kapsam: M-tusu kosu-yolu overlay'i, sabit-lineer kok neden, StS-dallanma icin min-degisiklik haritasi
Yontem: salt-okuma (Read/Grep), Unity calistirilmadi. NLM auth suresi dolmustu (read-only ajan login yapamaz) → canon asset-pipeline karar dosyasindan dogrulandi.

---

## TL;DR (yonetici ozeti)

**Dallanma altyapisi ZATEN VAR ve calisir durumda.** Lineer goruntunun TEK nedeni tek bir serialize-bool: `forceDemoSequence = true`. Branching prosedurel uretici (`DungeonGraph.Generate`) tam fork/merge graf uretir, EditMode testleri gecer, ve M-tusu IMGUI overlay coklu cocuk/ebeveyn cizimini ZATEN destekler. Sifirdan graf/branch yazmaya GEREK YOK. "Enum koprusu" blokeri bu canli akisi ETKILEMEZ — o, AYRI ve su an OLU olan `UI/Map` sprite-overlay alt-sistemine aittir.

---

## (a) Lineer dizi NEREDE uretiliyor — kesin satirlar

1. **Tetikleyici flag (asil neden):**
   `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs:105`
   `[SerializeField] private bool forceDemoSequence = true;`
   Sahnede serialize edilmis: `Assets/Scenes/_Arena.unity:3938  forceDemoSequence: 1` (canli `RoomRunDirector` GO).

2. **Dallanmis uretici yerine lineer'e dallanma:**
   `RoomRunDirector.cs:190-199` — `if (forceDemoSequence) graph = DungeonGraph.BuildDemoSequence(); else graph = DungeonGraph.Generate(runSeed, depthCount);`

3. **Lineer graf uretimi:**
   `Assets/Scripts/MapDesigner/Room/Runtime/DungeonGraph.cs:73-115` — `DemoSequence` dizisi {Combat,Combat,Merchant,Combat,Boss,Combat} + `BuildDemoSequence()`: her node TEK cocuk (`childIds.Add(i+1)`), son post-boss Combat terminal. Yani M-overlay'de gordugun 0→1→2→3→4→5 tek-zincir tam olarak burasi.

## (b) Graf/branching veri yapisi VAR mi — EVET, sifirdan gerekmez

- **Canli graf modeli** (saf C# class, MonoBehaviour degil): `DungeonGraph.cs` — `DungeonNode{ id, depth, RoomType roomType, List<int> childIds }`. Coklu cocuk = dallanma, coklu ebeveyn = birlesme zaten ifade edilebilir.
- **Branching prosedurel uretici HAZIR:** `DungeonGraph.Generate(seed, depthCount)` (DungeonGraph.cs:117-158) — her derinlikte 1-3 node (`NodeCountAtDepth` derinlik ortasinda `random.Next(2,4)`), `ConnectRows` ile fork+merge baglar, oksuz-node garantisi var (parentCounts kontrolu). Roller `RoomTypeAtDepth`: Combat/Elite/Chest agirlikli, derinlik 0=Combat start, son=Boss. **Determinist+orphan-free EditMode testleri gecer** (`Assets/Tests/EditMode/Room/RoomRuntimeDungeonGraphTests.cs`).
- **Navigasyon HAZIR:** `RoomRunDirector.AdvanceTo(choiceIndex)` (1795), `TryEnterDoor(choiceIndex)` (1814); kapilar zaten cocuk-secimine bagli: `BuildExitDoors(doorTypes)` her cocuk icin bir kapi acar (358-371), `RoomRunExitDoorTrigger` G ile `choiceIndex`'i iletir (1843-1879). Yani oyuncu fork'ta hangi kapiya girerse o dala gecer — secim mekanigi ZATEN var, sadece su an her oda 1 cocuk oldugu icin tek kapi cikiyor.

## (c) Overlay dallanmayi cizebilir mi — EVET (kod-onayli)

`RunMapOverlay.cs` (M-tusu IMGUI):
- Satir 66-83: her derinligi `NodesAtDepth(depth)` ile bir SATIR olarak cizer, satirdaki node sayisina gore yatay yayar (coklu node/satir = fork gorseli). Tek-node'a ozel kod yok; N node sorunsuz.
- Satir 86-105: her node icin `parent.childIds` uzerinde gezip ebeveyn→cocuk cizgisi ceker. Coklu cocuk VE coklu ebeveyn (merge) otomatik cizilir.
- Renkler tip-bazli `ColorFor(RoomType)` (133-145; Combat/Elite/Boss/Chest/Merchant/Event). Mevcut node cyan border (115,126).
→ **Overlay'de degisiklik GEREKMEZ;** branching graf verilirse aynen dallanmis cizer. (Asset-pipeline'in sprite ikonlu guzel surumu AYRI is — bkz. (d).)

## (d) "Enum koprusu" blokeri tam ne — ve CANLI AKISI ETKILEMEZ

- Kaynak: `STAGING/RUNMAP_UI_ASSET_PRODUCTION_DECISION_2026-06-11.md:10-12,52,99,106`.
- Iki AYRI enum var: `RIMA.RoomType` (Combat/Elite/Boss/Chest/Merchant/Forge/Event/Curse/Corridor — `Assets/Scripts/Core/RoomType.cs`) vs `RIMA.UI.Map.MapNodeType` (Combat/Elite/Rest/Boss/Event/Shop/CurseGate/Mystery/Entry — `Assets/Scripts/UI/Map/MapNodeData.cs`). Esit DEGIL (Merchant↔Shop, Chest↔? yok, Rest/Mystery yok).
- "Enum koprusu" = ikon-atlas pipeline'i baslamadan once netlesmesi gereken `RoomType→MapNodeType` (RoomTypeToNode) sozlugu; netlesmeden ikonlar yanlis slota duser (dosyada "en yuksek risk").
- **KRITIK:** Bu koprù SADECE `Assets/Scripts/UI/Map/*` (MapNodeUI/MapGraphData/MapPanelUI/MapProgressController + MapNodeType) sprite-ikonlu overlay icindir. M-tusu canli overlay (`RunMapOverlay`) bu tiplerin HICBIRINE referans vermez (Grep dogrulandi: RunMapOverlay yalniz `RIMA.RoomType` + `DungeonGraph` kullanir). UI/Map alt-sistemi canli demo akisinda KULLANILMIYOR (olu/legacy). → Enum koprusu, branching'i ACMAK icin bloker DEGIL; sadece "guzel sprite-ikonlu harita" istenirse devreye girer.

## (e) Min-degisiklik dallanma hook noktalari (oncelik sirali)

1. **EN KUCUK (1 deger):** `_Arena.unity:3938` → `forceDemoSequence: 0`. Aninda `Generate(runSeed, depthCount)` devreye girer; M-overlay dallanmis cizer, kapilar coklu-cikis acar, AdvanceTo dogru dala goturur. **Sifir kod.** (Demo-tutarlilik kaybi: artik her run determinist degil — ama gorev tam bunu istiyor.)
2. **HER-RUN DEGISEN (1 satir):** `RoomRunDirector.cs:198` `Generate(runSeed, ...)` → `Generate(seed)` her BeginRun'da rastgele tohum (or. `runSeed = Random.Range(...)` BeginRun basinda) → run-bazli farkli harita. Aksi halde sabit `runSeed=12345` her run ayni grafi uretir.
3. **Reveal/fog-of-war karari (ACIK SORU — orchestrator+council):** Su an overlay graf'in TAMAMINI cizer (StS-tam-stratejik). Asamali/fog istenirse `DungeonNode`'a `revealed` bayragi eklenip overlay'de silüet ciz; canli graf modelinde reveal YOK (legacy `Core/DungeonGraph.cs`'de `RevealAhead`/`revealed` VAR ama o obsolete MonoBehaviour, canli degil). Karar verilmeden kodlanmasin.
4. **Canon-tip eslesmesi (orta is, opsiyonel demo-disi):** `Generate` su an Event'i demo-disi tutmus, Merchant uretmiyor (yalniz Combat/Elite/Chest/Boss). StS-hissi icin `RoomTypeAtDepth`'e Merchant + (canon: Spacial Cliffs/Steel Aegis Cave/Mystifying Forest bolgeleri, RIMA element node'lari) eklenmeli — ama bu tasarim/canon isi, NLM dogrulamasi gerek (auth yenilenince).
5. **Sprite-ikonlu guzel harita (BUYUK, demo-disi):** UI/Map alt-sistemini canliya baglamak + enum koprusu (RoomTypeToNode) + NodeIconLibrary + atlas. Branching'i acmak icin GEREKMEZ; sadece gorsel kalite. Ayri faz.

---

## Acik karar (orchestrator'a)
- **Fog-of-war mi tam-reveal mi** (e/3) — kodlamadan once karar; NLM auth yenilenip canon sorgulanmali ("tum haritayi bastan gor" vs "asamali reveal").
- **Generate'in tip dagilimi** (e/4) — Merchant/Event/canon-bolge node'lari demo-icinde mi? NLM canon + council.
- En hizli "calisir dallanma" demosu icin sadece (e/1)+(e/2) yeterli; gerisi cila.
