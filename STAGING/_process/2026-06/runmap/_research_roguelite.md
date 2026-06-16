# Roguelite Run-Map Design Research — Branching Procedural Maps

> Arastirmaci: map-design subagent · Tarih: 2026-06-16
> Amac: RIMA "KOSU YOLU" (M tusu) overlay'ini SABIT-LINEER listeden -> HER RUN procedural DEGISEN, StS-tarzi DALLANAN node haritasina cevirmek icin tasarim referansi.
> NLM canon: query DENENDI ama `nlm login` suresi DOLMUS (auth expired) -> NLM canon dogrulanamadi; asagidaki RIMA bilgileri orchestrator brief'inden ve global memory'den alindi (bolgeler Spacial Cliffs / Steel Aegis Cave / Mystifying Forest; node tipleri Combat/Elite/Boss/Merchant/Chest/Event; pipeline chrome=ChatGPT, sembol=PixelLab, cizgi=Unity; bloker "enum koprusu"). YENI SESSION'da `nlm login` sonrasi canon node-mix/region dogrulamasi onerilir.

---

## 1. Slay the Spire — Act-Map Uretim Algoritmasi (CANON REFERANS)

StS endustride dallanan run-map'in altin standardi. Algoritma tam asagida.

### 1a. Grid & boyut
- Her ACT = **17 kat (floor)**, alttan (floor 1) yukari (floor 17) cikilir.
- Grid = **7 kolon x 15 satir** "irregular isometric" (ucgenlerden olusan) grid; her katta **en fazla 6 konum (node)** yatay sirada.
- Dikey akis: oyuncu HER ZAMAN bir kat yukari cikar (geri donus yok), ama AYNI kat icinde dallar arasinda secim yapar.

### 1b. Path (yol) uretimi — cekirdek dongu
1. Algoritma 1. kattaki rastgele bir node'dan baslar.
2. O node'u, **2. kattaki en yakin 3 node'dan birine** bir kenarla (edge) baglar.
3. Bunu bir sonraki kat icin tekrarlar... ta ki en uste kadar tek bir tam yol cizilene dek.
4. Bu tum-yol-cizme islemi **6 kez** tekrarlanir (6 baslangic gezintisi).
> Sonuc: yollar cakisip orgu gibi birlesir; tek bir node'a alttan **1-3 yol girer**, ustten **1-3 yol cikar** (en alt ve en ust kat haric).

### 1c. Path KISITLARI (kritik kurallar)
- **Baslangic cesitliligi:** ilk 2 rastgele secilen 1.-kat node'u AYNI olamaz -> her zaman EN AZ 2 farkli baslangic noktasi garanti.
- **Yollar BIRBIRINI KESEMEZ** (no path-crossing) — gorsel okunabilirlik + planlanabilirlik icin. (Teknik mekanizma: komsu node'a baglarken, sol/sag komsunun zaten kullandigi capraz kenari secmeyi engelle.)
- **Kardes-benzersizligi:** 2+ cikis yolu olan bir node'un tum varis node'lari benzersiz olmali.

### 1d. Node-tipi ATAMA kurallari & agirliklari
Yollar cizildikten ve esi olmayan node'lar silindikten SONRA tipler atanir.

Taban agirliklari (StS act-1):
| Tip | Olasilik |
|---|---|
| Normal Combat (Monster) | **53%** |
| Unknown (`?` Event) | **22%** |
| Rest Site (kamp) | **12%** |
| Elite | **8%** |
| Merchant (Shop) | **5%** |
> `Unknown` node girilince dinamik cozulur (event/combat/shop/treasure) ve kalan tipler arasinda olasilik yeniden dengelenir.

Kat-bazli GARANTILER + kisitlar:
- **Floor 1:** tum node'lar = kolay-havuz Combat (yumusak baslangic).
- **Floor 9:** tum node'lar = Treasure/Chest (orta-act odul kati).
- **Floor 15:** tum node'lar = Rest Site (boss oncesi son kamp).
- **Floor 16:** TEK node = **Boss** (tum yollar buraya akar — boss daima tek-dugum tunel).
- **Elite ve Rest 6. kattan asagi atanamaz** (erken oyunda yok).
- **Elite / Merchant / Rest ardisik OLAMAZ** (alt-ust ayni ozel tip yasak).
- **Rest Site 14. kata konamaz** (15 zaten rest oldugu icin cifte-kamp engeli).

### 1e. Seed / "her run farkli" mekanigi
- TUM rastgelelik = embark aninda sabitlenen **alfanumerik seed**.
- Seed -> her kattaki node SAYISI+TIPLERI + yol duzeni -> tutarli, paylasilabilir.
- Ayni seed = ayni harita (speedrun/daily-run dogrulanabilirligi).

---

## 2. Alternatif Topolojiler (kiyas)

| Oyun | Topoloji | Reveal | Karar dokusu | RIMA dersi |
|---|---|---|---|---|
| **Slay the Spire** | Dallanan DAG, 6-genis, alttan-yukari, boss=tek-dugum | TUM harita bastan gorunur | "Yol planla" (kaynak/risk rotasi) | Ana sablon. Tam-stratejik plan. |
| **Hades** | Lineer oda-zinciri; her odada KAPI uzerinde sonraki odanin ODULU gorunur | Sadece komsu kapi(lar) gorunur (1-adim ileri) | "Odul sec" (harita degil, kapi) | Reveal-lite; topoloji yok ama "1-adim onizleme" gerilimi RIMA fog'una model. |
| **Inscryption** | Act-1: dar dallanan; Act-3: acik grid-kesif | Asamali (ilerledikce gorunur) | Kategori-ici secim (ayni tip farkli yollarda) | Fog-of-war'in basit hali; "kategori cesitliligi" hissi. |
| **Monster Train** | Ring-bazli (kat=ring), her ring'de sinirli secim | Tum-ring gorunur | Dikey savunma + ring rotasi | Daha az dallanan; RIMA icin fazla dar. |
| **Across the Obelisk** | Genis dallanan dunya-haritasi + side-quest + key-item kapilari | Buyuk olcude gorunur, sabit eventler | Sidequest/key dallari + 4-kahraman parti | Dallar arasi "side-content" cesitliligi (Chest/Event yan-dal) RIMA'ya ilham. |

Ana ayrim: **Hades = odul-secimi** (topolojisiz, tek-adim), **StS = harita-navigasyonu** (onceden uretilmis topolojide yol-secimi). RIMA'nin hedefi acikca StS-tarafi (dallanan node haritasi).

---

## 3. Full-Map Reveal (StS) vs Fog-of-War Asamali Reveal — Trade-off

### Full-map bastan goster (StS modeli)
- **Arti:** stratejik PLANLAMA mumkun — oyuncu en ustteki boss'a kadar tum rotayi gorur, "su elite'i atlayip merchant'a, sonra rest'e" gibi cok-adim build-rotasi kurar. Bu = StS'in derin karar katmani. Okunabilirlik yuksek, adaletli (sürpriz ceza yok).
- **Eksi:** kesif-gerilimi/bilinmezlik AZ; harita statik bir bulmaca gibi hissedilebilir.

### Fog-of-war / asamali reveal
- **Arti:** kesif merakı + gerilim (roguelike'in cekirdek "beklenmedikle karsilas" hazzi); her adim mini-karar.
- **Eksi:** uzun-vadeli build planlamasini KIRAR; oyuncu rastgele-hissi/adaletsizlik yasayabilir; bilgi eksikligi = "scout etmem lazim" yuku. Tam-fog ayrica gorsel/performans karmasikligi ekler.

### RIMA icin TAVSIYE: **HIBRIT — "StS full-map + 1-adim ileri vurgu"**
Gerekce:
1. RIMA'nin demo-tezi = **environment + vertical slice**, sunum/hocaya gosterilen "en can alici" parca. Sunumda HARITA-OKUNAKLILIGI ve "her run farkli + oyuncu yol seciyor" mesaji NET gorunmeli -> **full-map reveal** bunu aninda iletir (StS gibi tek bakista "dallanma var, secim var" anlasilir).
2. Tam fog-of-war demo'da iyi sunum-degeri vermez (izleyici dallanmayi goremez) + ekstra implementasyon/gorsel risk; RIMA scope-lock disiplinine aykiri.
3. KESIF-gerilimini kaybetmemek icin Hades'ten odunc: tum harita gorunur AMA **mevcut node'dan erisilebilir (komsu) node'lar VURGULU/parlak**, uzaktakiler hafif soluk/desature. Boylece "plan gorunur ama bir sonraki adim one cikar" — hem stratejik plan hem adim-adim odak.
4. (Opsiyonel post-demo) Event/Chest gibi `?`-tipi node'lar StS-vari "girince cozulur" tutulabilir = full-map icinde mini-bilinmezlik, fog kurmadan kesif hazzi.
> Net: **demo = StS-tarzi FULL-MAP reveal + erisilebilir-node vurgusu.** Fog-of-war'i post-demo deneysel backlog'a birak.

---

## 4. "Procedural / her-run-farkli" hissini NE veriyor

1. **Seed-tabanli uretim** — her embark'ta yeni seed; ayni seed dogrulanabilir (StS). RIMA: run baslarken seed uret/kaydet.
2. **Dallanma genisligi (branching factor)** — kat basina 3-6 node + node basina 1-3 cikis. Cok dar = lineer hisseder (mevcut RIMA hatasi), cok genis = kaotik. StS sweet-spot ~6-genis.
3. **Coklu baslangic** — en az 2 farkli giris (StS kurali) -> ilk karar runun ilk anindan baslar.
4. **Node-mix dengesi (agirliklar)** — Combat baskin (~%50+) ama Elite/Shop/Rest/Event serpistirilmis; ardisik-ozel-tip yasagi her haritayi "farkli ritimde" hissettirir.
5. **Yol-secim sonucu** — secilmeyen dal kaybedilir (geri donus yok) -> firsat-maliyeti her run'i benzersiz kilar (StS: rest mi elite mi).
6. **Sabit cikis-noktasi (boss=tek-dugum)** — degisken govde + sabit zirve = tanidik-ama-taze; her run ayni boss'a FARKLI rotadan varir.

---

## 5. RIMA'ya Uygulanabilir Desenler (somut)

- **Topoloji:** StS DAG'i benimse. Demo-olcek: act basina ~6-8 kat (17 demo icin uzun), kat basina 2-4 node, ~3-4 baslangic dali. Boss = tek-dugum tepe; bolge-temali (Spacial Cliffs ilk act).
- **Node-tipleri (RIMA canon):** Combat (baskin) · Elite · Boss (tek) · Merchant/Shop · Chest · Event. StS agirliklarini taban al, demo icin Elite/Shop garantisi koy (sunum tum tipleri gostersin).
- **Kisitlar (StS'ten birebir):** ilk kat hep Combat · boss oncesi Rest/Chest garanti · ardisik Elite/Merchant yasak · yollar kesismez · coklu baslangic.
- **Reveal:** full-map + erisilebilir-node vurgusu (bkz. Bolum 3 tavsiye).
- **Seed:** run-start'ta seed uret -> "her run procedural degisir" tezi kanitlanir; demo'da seed-degistir butonu sunumda "bak, baska harita" gosterisi yapar.
- **Pipeline notu:** mevcut "enum koprusu" blokeri = node-tipi enum <-> sembol/asset eslemesi muhtemelen; procedural uretici node-tipi enum'unu uretip sembol (PixelLab) + cizgi (Unity edge) ile baglamali. Generator'i veri-katmaninda (NodeType enum + seed'li layout) tut, gorsel katmandan ayir (modulerlik refleksi: layout-algoritmasi reusable, sembol-asset bespoke).

---

## Kaynaklar
- Slay the Spire Wiki — Map Generation: https://slaythespire.wiki.gg/wiki/Map_Generation
- Steam Community — Map Generation in Slay the Spire (detayli kilavuz): https://steamcommunity.com/sharedfiles/filedetails/?id=2830078257
- Slay the Spire Wiki — Map Locations: https://slaythespire.wiki.gg/wiki/Map_Locations
- Hades Wiki — Chambers and Encounters: https://hades.fandom.com/wiki/Chambers_and_Encounters
- Inscryption map symbols (Prima Games): https://www.pcinvasion.com/inscryption-map-symbols-act-1-guide/
- Monster Train (Wikipedia): https://en.wikipedia.org/wiki/Monster_Train
- Across the Obelisk (Steam): https://store.steampowered.com/app/1385380/Across_the_Obelisk/
- Fog of War design (Grid Sage Games): https://www.gridsagegames.com/blog/2013/11/fog-war/
- StS map in Unity (referans implementasyon): https://github.com/silverua/slay-the-spire-map-in-unity
