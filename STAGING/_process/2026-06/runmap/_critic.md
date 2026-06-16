# RUN-MAP BRANCHING DESIGN — ACIMASIZ SKEPTIK ELESTIRI (council critic lensi)

Tarih: 2026-06-16 · Lens: adversarial skeptik · Yontem: design + audit + CANLI KOD okuma (DungeonGraph x2, RoomRunDirector, RunMapOverlay, IsoRoomBuilder.BuildExitDoors)

## VERDICT: **NEEDS-FIX** (yon dogru, plan eksik-test + 3 sessiz risk + 1 canon-yalan)

Cekirdek tez (sistem ZATEN VAR, flag kapatiyor) DOGRU ve kod-onayli. Ama "cerrahi 5-satir, sifir risk" iddiasi ASIRI-IYIMSER. Branching'i acmak demo-yolunu YENI BIR koda sokuyor (Generate + coklu-kapi geometri) ki o yol BUGUNE KADAR PLAY'DE HIC kosmadi (forceDemoSequence=true 1+ aydir). "Test edilmemis kod = bilinmeyen kod."

---

## CURUTME (a)-(e)

### (a) Demo-kapsam (19 Haz, 3 gun) — branching map demo'ya mi? → **EVET ama reveal POST-DEMO'ya it**
- Branching ACMA (#1+#2+#3) gercekci: enjeksiyon-patch, mevcut testli generator. KABUL.
- AMA reveal/fog katmani (#4, ~30-40 satir YENI overlay state + BFS) demo-kritik DEGIL ve risk/getiri kotu. "En can alici" parca = DALLANMA gorunur olmasi; fog onu GIZLER. Demo'da fog izleyiciye dallanmayi SAKLAR → tezi ZAYIFLATIR. **BINDING: demo = full-reveal overlay (dallanma tam gorunur), fog POST-DEMO.** Design'in kendi "karar-catismasi notu" bunu zaten itiraf ediyor ama yanlis tarafa cozuyor (canon'u demo-sunum-degerinin onune koymus — demo HEDEFI tam tersi).

### (b) "Enum koprusu" cozumu saglam mi? → **EVET, dogru teshis**
- Audit kod-onayli: RunMapOverlay yalniz RIMA.RoomType+DungeonGraph kullanir, MapNodeType'a HIC dokunmaz. Kopru sadece olu UI/Map sprite-hatti icin. Demo-yolundan cikarmak DOGRU. Itiraz yok.

### (c) Generation gercekten her-run-farkli + ADIL mi? → **HAYIR, uc kanitlanmis kusur**
1. **MERCHANT URETILMIYOR.** `RoomTypeAtDepth` (DungeonGraph.cs:170-200) yalniz Combat/Elite/Chest/Boss dondurur — **Merchant ASLA cikmaz** (roll>=90 → Combat fallback, Event guard'li). Design "Merchant guaranteed-1" diyor ama kod bunu YAPMIYOR. forceDemoSequence=false yapinca demo TUCCAR ODASINI KAYBEDER (ekonomi beat'i gider). Design #3 "Merchant guaranteed-1 enjeksiyon" diyor — bu YAZILMAMIS kod, "min-degisiklik" degil yeni dagitim-mantigi.
2. **ARDISIK-ELITE / ADALET guard YOK.** `RoomTypeAtDepth` her depth bagimsiz roll; iki ardisik Elite, hatta tum mid-depth Elite mumkun. "Adil" iddiasi (ardisik-elite engeli) kodda YOK → eklenecek (yine yeni mantik). Design metni iddia ediyor, generator garanti etmiyor.
3. **CIKMAZ-YOL: orphan-free VAR ama "her child erisilebilir" ≠ "her path boss'a gider."** ConnectRows orphan (ebeveynsiz) onler; ama dejenere durumda bir mid-node'un TEK cocugu olabilir → secim sahte (StS hissi yok). depthCount=5 (default) → maxDepth=4, design 6-depth (0-5) istiyor; **depthCount degeri design ile UYUMSUZ** (inspector'da 5, design tablo 6 satir). Boss her zaman son-depth tek-node (kod garanti) → boss-erisim OK.

### (d) Reveal-modeli RIMA vizyonuyla ("environment+vertical slice") tutarli mi? → **KISMEN, ama demo-hedefiyle CATISIR**
- Fog canon-sadik (LOCKED doc dogru aktarilmis). AMA sunum tezi = "her run procedural DEGISEN harita izleyici GORSUN." Fog tam onu gizler. Vizyon-tutarlilik (canon) ile DEMO-ETKI (gorunur dallanma) catisiyor; design canon'u secip demo-etkiyi feda etmis. Skeptik karar: **demo = full-reveal (gorunur dallanma kazanir), canon-fog post-demo backlog** — cunku bu demo'nun "en can alici" amaci dallanmayi GOSTERMEK.

### (e) Min-degisiklik mi gizli-refactor mi? → **Sundugundan BUYUK (gizli is var)**
- #1 (flag) + #2 (seed) = gercekten cerrahi. KABUL.
- #3 "enjeksiyon" = ASLINDA RoomTypeAtDepth yeniden-yaziliyor (Merchant guaranteed + Elite cap + ardisik-guard + lane-cap). Bu YENI dagitim-algoritmasi, "1 deger" degil → mevcut EditMode testleri (determinist+orphan-free) BU degisiklikten sonra TEKRAR yazilmali/gunceli; design test-guncellemesini ANMIYOR. **Test borcu gizli.**
- #4 fog = yeni overlay state. ↑(a)/(d) geregi POST-DEMO.
- **KANITSIZ ANA RISK:** Generate yolu (coklu-kapi) PLAY'DE hic kosmadi. `BuildExitDoors` coklu-kapi icin once template exit-socket'lerine bel baglar (`TryResolveExitSlotsForDoorCount`); _Arena template'i 2-3 kapilik exit-socket'e SAHIP MI bilinmiyor → degilse fallback-row (rastgele floor-cell satiri) devreye girer = kapilar cirkin/ust-uste/erisilemez yerde olabilir. **Bu demo-stopper olabilir ve audit/design ikisi de play-test etmedi.**

---

## CANON-YALAN (binding)
- Design satir 10/47-51: "NLM auth EXPIRED → LOCKED docs canonical proxy." Brief NLM'i ZORUNLU canon-kaynak yapiyor. Node-mix/region/element-sembol/Merchant-orani LOCKED-proxy'den TUREMIS, NLM-dogrulanmamis. Design #7 bunu "post-demo teyit" diye erteliyor — KABUL EDILEMEZ: demo node-dagitimini sekillendiren sayilar (%55/%20/%15, Merchant-guaranteed) dogrulanmadan kodlanirsa demo yanlis-canon gosterebilir. **BINDING: kodlamadan ONCE NLM auth yenile + node-mix/Merchant-kurali/region tek-sorgu dogrula (brief'teki nlm query komutu); auth gercekten imkansizsa kullaniciya ACIKCA "canon dogrulanmadi, proxy ile gittik" uyarisi.**

---

## BINDING FIX'LER (demo gecmeden ONCE)
1. **PLAY-TEST ZORUNLU (en kritik):** forceDemoSequence=false + Generate ile gercek run koş; M-overlay dallanma ciziyor mu, 2-3 kapi FIZIKSEL dogru yerlesiyor mu (BuildExitDoors socket vs fallback-row), AdvanceTo(choiceIndex) dogru dala gidiyor mu, read_console 0-error. Kanit olmadan SOUND degil.
2. **depthCount tutarlilik:** inspector 5 vs design 6 — birini sec, BeginRun cagrisi + design tablosu hizala.
3. **Merchant + adalet generator'da GERCEKTEN yaz:** RoomTypeAtDepth'e Merchant-guaranteed-1 (mid) + Elite cap (max N, ardisik-engel) + min-2-cikis-mid garantisi. "Enjeksiyon" deme — yeni mantik; **EditMode testlerini GUNCELLE** (yeni invariant'lar: Merchant>=1, Elite<=cap, her mid-node>=? cikis).
4. **Reveal = demo'da FULL, fog POST-DEMO:** #4 demo-kapsamindan CIKAR; dallanma gorunur kalsin (demo tezi). Canon-fog backlog'a, kullanici onayli.
5. **Seed: Random.Range UnityEngine kullan + log:** runSeed = UnityEngine.Random.Range(int.Min,int.Max) BeginRun basinda; seed'i Debug.Log + (opsiyon) overlay'de goster — "seed degistir → baska harita" sunum-jesti icin.
6. **NLM canon dogrula** (↑canon-yalan) — kodlamadan once, yoksa kullaniciya acik uyari.

## NET
Yon DOGRU, "sistem var flag kapatiyor" tezi kod-onayli ve degerli. Ama plan kendini "5-satir sifir-risk" diye satiyor; gercekte = test-edilmemis kod-yolu + yazilmamis generator-mantigi + gizli test-borcu + canon-proxy. **Play-test + Merchant/adalet gercek-implementasyon + full-reveal(demo) + NLM-dogrula → SOUND. Bunlarsiz NEEDS-FIX.**
