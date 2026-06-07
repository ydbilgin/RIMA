# 01 — Kare Kare UX Akış Review

## Genel not

Bu akışta oyunun ana loop'u okunuyor: Menü → Chamber → bürünme → run → combat → execute → reward/draft → portal seçimi → boss → sonuç. Bu iyi. Ama ekranda görünenler çoğu yerde oyuncuya "bir sonraki doğru hareket"i yeterince temiz söylemiyor. Oyun sistemi var, sunum katmanı hâlâ biraz "debugsız debug" gibi.

## Kare verdict tablosu

| # | Ekran | Verdict | Sorun | Somut düzeltme |
|---|---|---|---|---|
| 01 | Main Menu | NET | Menü okunuyor, mood iyi. Ama `RIMA The Rift Hunters` EN, butonlar TR; "Yine geldin." çok küçük/garip konumda. | Tek dil seç. Demo TR ise subtitle ve geri dönüş mesajı TR. `Yine geldin.` yerine `Tekrar hoş geldin, Avcı.` gibi daha diegetic küçük satır. |
| 02 | Settings | NET ama dil kirli | Başlıklar EN, butonlar TR/EN karışık. Panel çok dar ve metin küçük. | TR demo: `AYARLAR / OYNANIŞ / ERİŞİLEBİLİRLİK / SES / KONTROLLER`. Panel genişlet, satır aralıklarını aç. |
| 03 | Chamber wide | BELİRSİZ | "Karakter seçiyorum" bilgisi tam oturmuyor. Pedestallar çok büyük, karakterleri ve label'ları kapatıyor. Sağdaki label'lar kırpılmış. | Pedestalları %25-35 küçült. 5+5 iki yay dizilimi. Her pedestal üstünde küçük class adı + seçili glow. Merkez rift/altar net olsun. |
| 04 | Attune prompt | YARI NET | `[E] Bürün` görünüyor ama metin kesiliyor/kenarda kalıyor. Pedestal kalabalığı prompt'u yutuyor. | Prompt'u karakterin üstüne değil, pedestal önünde sabit world label olarak ver: `[E] Bürün: Warblade`. Background karartma yoksa text kayboluyor. |
| 05 | Attuned | BELİRSİZ | Attune sonrası ne değiştiği çok zayıf. "Attuned Echo" yazısı var ama sıradaki hedef belli değil. | Büyük kısa feedback: `Warblade Echo bound.` + 0.5s cyan burst + yerde rift kapısına doğru ince cyan path. |
| 06 | Rift gate prompt | NET | `[G] Rift Gir` anlaşılır ama dil yanlış/eksik. | `Rift'e Gir [G]` veya `Koşuya Başla [G]`. Kapı/portal terimi tekleşsin. |
| 07 | Run room spawn | BELİRSİZ | Oyuncu odaya geldi ama amaç yok. Oda çok büyük ve boş. Spawn sağ kenara fazla yakın; oyuncu sahneye "sonradan bırakılmış" gibi. | Kısa title card: `Oda 1 — Düşmanları Temizle`. Spawn güney-alt ama merkeze daha yakın. Arrival ring VFX. |
| 08 | Combat | BELİRSİZ | Düşmanlar placeholder diye değerlendirmiyorum, ama combat UI/amaç/tehlike feedback yok. Oda dev, odak sağ-alt kenarda sıkışıyor. | Minimal combat HUD: HP, skill bar, room objective. Kamera %10-15 yakın. Düşmanlar merkeze yayılmalı, oyuncu cliff kenarına sıkışmamalı. |
| 09 | Execute prompt | NET | `[RMB] İnfaz` iyi. Ama prompt çok büyük/üstte ve bağlam yok: düşman neden infazlık? | Target üstünde küçük altın kırık ikon + `[RMB] İnfaz`. Broken/Sundered ring ekle. |
| 10 | Room clear reward | NET ama sönük | `[G] Ödülü Al` net. Ama room clear payoff zayıf, portal/reward ayrımı karışabilir. | `ODA TEMİZLENDİ` kısa banner + reward object glow + clear SFX/VFX. |
| 11 | Draft | NET ama hiyerarşi zayıf | Kartlar okunuyor ama metin küçük, sinerji/tag yok. Üst başlık "ODA 1 — ÖDÜL SEÇ" çok büyük. | Başlık küçült. Kartlara tag chip + synergy line. `SEÇ` butonlarını aynı hizaya al. Background blur/dim iyi ama portal arkası dikkat dağıtıyor. |
| 12 | Portals | BELİRSİZ | Portallar güzel ama hepsi aynı görünüyor. Hangi portal ne? Nereye gidiyorum? Crossed-sword ikonları uzakta ve bağlamsız. | Portal üstünde tür label/rune: `COMBAT`, `ELITE`, `CHEST`, `BOSS`. 2 çıkışta center boş kalabilir ama oyuncuya "2 seçenek" açık okunmalı. |
| 13 | Map overlay | NET | Graph okunuyor. Ama EN/TR karışık: `RUN PATH`, `M ile kapat`, `cyan = bulunduğun oda`. | TR: `KOŞU YOLU`. Cyan değil `camgöbeği` deme, sadece `parlayan = bulunduğun oda`. Node tip renkleri portal renkleriyle eşleşsin. |
| 14 | Character sheet | BELİRSİZ | Sol liste çok küçük, sağdaki büyük boş kutu placeholder gibi. Oyuncu ne işine yarıyor anlamaz. | Sol paneli büyüt. Sağda route mini-map veya class portrait gerçekten dolu olmalı. Boş kutu kalmasın. |
| 15 | Skill codex | BELİRSİZ | Çok küçük, çok tablo gibi. Demo sırasında kimse okumaz. | Sunumda açma veya sadece 1-2 sn göster. Daha sonra kategori filter + büyük tooltip gerekir. |
| 16 | Boss room | BELİRSİZ/KRİTİK | Boss placeholder kabul ama yeşil debug kare kabul edilemez. Boss HP bar düz sarı blok gibi. Oda yine normal oda gibi; boss anı yok. | Yeşil kareyi kaldır. Boss intro title + kırmızı/rune telegraph + ritual circle. Boss HP bar çerçeveli olmalı, sarı düz blok değil. |
| 17 | Victory | NET ama görsel kalite çakılıyor | `DEMO COMPLETE` net. Ama dev sarı panel çok kötü görünüyor, background aşırı parlak ve metin kontrastı düşük. | Sarı paneli kaldır. RIMA UI 9-slice koyu panel + cyan/gold accent. `Wishlist on Steam` CTA kalabilir ama daha premium. |
| 18 | Death | NET | Ölüm akışı net. Ama EN/TR karışık, `QUIT` başlığı ile TR butonlar uyumsuz. | TR veya EN tek dil. `Run Bitti` + Echo dökümü daha okunur. |
| 19 | Return menu | NET | Menü dönüşü net. Ama sadece sol alt metin değişimi çok az payoff. | Geri dönüşte küçük Echo toplamı / son run notu: `Son koşu: +5 Echo`. |

## En kritik UX sorunları

1. **Her yeni state'te "şimdi ne yapacağım?" cümlesi yok.**  
   Çözüm: kısa durum banner'ları.
   - `Echo Seç`
   - `Rift'e Gir`
   - `Düşmanları Temizle`
   - `Ödülünü Seç`
   - `Bir Portal Seç`
   - `Boss Yaklaşıyor`

2. **Portal seçimi rota kararı gibi okunmuyor.**  
   Portallar güzel glow veriyor ama gameplay bilgisi eksik. Tür rünü/label şart.

3. **Chamber karakter seçimi gibi değil, büyük yuvarlak disk kalabalığı gibi.**  
   Pedestallar küçülmeli, karakterler/statüler öne çıkmalı.

4. **Boss anı demo kalitesini düşürüyor.**  
   Yeşil kare ve düz sarı HP bar, bütün ciddiyeti kırıyor. Bu kare jüriye gösterilecekse önce düzelmeli.

5. **UI dili tekleşmeli.**  
   Şu an "Başla / Settings / Run Path / Skill Codex / Main Menu / Tekrar Dene" karışımı var. İnsan beyni zaten yeterince çile çekiyor.
