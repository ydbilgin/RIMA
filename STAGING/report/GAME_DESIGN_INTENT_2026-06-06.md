# RIMA — OYUNDA OLMASI GEREKEN (TASARIM NİYETİ)
ChatGPT review için: her sistemin oyunda **ideal hali** + bilinen eksikler. Şekil numaraları FIGURE_REVIEW_GUIDE ile eşleşir.

## Genel kimlik
- **Tür:** 2D top-down roguelite (Hades akrabası), pixel art, izometrik "yüzen ada" odalar — her oda mor void üstünde bir taş platform, güney kenarı açık uçurum (cliff), kapılar hep arkada.
- **Görsel dil:** koyu taş + mor void + **cyan** = Rift/Echo enerjisi (mühür, rün, portal hep cyan), sıcak turuncu = brazier/ateş vurgusu. Karakterler 64px gerçek boy (canvas 120-128px), PPU 64, 8 yön (5 üretilen + 3 mirror), kamera yüksek 3/4 açı (gerçek izometrik matematik yok).
- **Çekirdek döngü:** Ana menü → Attunement Chamber'da sınıf seç (yürüyerek) → Rift kapısından run'a gir → oda temizle → ödül/draft → kapı seç → ... → boss → victory/ölüm → Shattered Echo kazan → Chamber'a dön.

## 1-2. Attunement Chamber (karakter seçimi) — Şekil 1, 2, 12
**Olması gereken:** Karakter seçimi MENÜ DEĞİL, oynanan bir oda. Oyuncu gerçek karakteriyle WASD ile yürür; hilal dizilimde 10 taş pedestal, her birinde o sınıfın DONUK (taş-gri) echo heykeli durur. Yaklaşınca dünya-içi "[G] Bürün — <SINIF>" prompt'u çıkar; G'ye basınca oyuncu o sınıfa "bürünür" (sprite + skill seti değişir — büründüğün bedenle dolaşırsın). Kilitli sınıflar SİYAH silüet + kilit şartı yazısı (Shattered Echo bedeli VEYA achievement yolu); skill'ler asla parayla açılmaz (sadece sınıflar). Odada gerçek-combat dummy var: seçtiğin sınıfı kapıdan çıkmadan test edersin (vur, HP düşer, regen olur). Çıkış = arka taraftaki Rift kapısı, "[G] Rift'e Gir" ile run başlar. TAB = klasik ekran fallback.
**Bilinen eksik/risk:** WASD his ayarı kullanıcı feel-test bekliyor.

## 3. Combat odası — Şekil 3
**Olması gereken:** Her oda RoomTemplateSO'dan inşa edilir: elmas zemin (hafif checker deseni `(x+y)%2`), kenar cliff'leri, 0-45 arası prop (kaya, kemik yığını = "başarısız muhafazalar", sütun) — prop'lar SADECE yürünebilir zeminde, yoğunluk oda merkezini boğmaz. Oyuncu odaya GÜNEY-alt ortadan girer (açık cliff tarafı = giriş hissi), düşmanlar socket noktalarında belirir (threat-budget ile oda zorluğuna göre). Tüm varlıklar ayakları zeminde, Y-sort doğru (alttaki üstte çizilir). Combat: vuruş hissi (hit-flash, knockback), ağır vuruş + Broken/Sundered durumdaki hedefte KNOCKDOWN (yere serilme + kalkışta i-frame), ölen moblar kalıcı zemin decal'i (kemik/leke) bırakır.
**Bilinen eksik:** spawn/kapı konumlandırması ŞU AN düzeltiliyor (socket-tabanlı sisteme geçiş); karakter pivot'ları nedeniyle bazı sprite'lar "havada" görünebiliyor (kalibrasyon aracı sırada); player sorting layer düzeltmesi sırada.

## 4. Skill draft — Şekil 4
**Olması gereken:** Oda temizlenince kısa slow-mo + ödül küresi; G ile alınca 3 KART açılır (Hades boon tarzı): her kart skill adı + ikon + rarity çerçevesi. Hover'da tooltip (tam açıklama) + elindeki skill'lerle SİNERJİ varsa kart üstünde sinerji vurgusu (pulse/chip). Kart seçimi tıkla; seçilen skill o run'a eklenir (kalıcı meta-unlock YOK — build her run'da draft'la kurulur). Skill havuzu sınıfa özel; "yakında" skill'ler draft'a sızmaz.
**Bilinen eksik:** sinerji vurgusunun görsel netliği feel-test bekliyor.

## 5. Dallanan kapılar — Şekil 5
**Olması gereken:** Run = StS-lite dallanan graph (1-3 çocuk). Oda temizlenince odanın ARKASINDA dal sayısı kadar kapı yan yana belirir (Hades tarzı tek sıra, asla odanın yanlarına dağılmaz). Her kapının üstünde gideceği oda türünün rünü (combat/elite/chest/boss) — oyuncu riski/ödülü görerek seçer. Kapı konumları template'te sabit slot; hangi türlerin geleceği her run random. Kapılar combat sırasında kilitli (sönük), clear + ödül sonrası cyan parıltıyla açılır; kapıya YÜRÜYEREK geçilir. M tuşu = run haritası overlay (gidilen yol + kalan dallar).
**Bilinen eksik:** sabit-slot konumlandırma şu an cx'te yapılıyor (önceden geometrik tahminle diziliyordu).

## 6. Karakterler — Şekil 6
**Olması gereken:** 10 sınıf, her biri okunaklı silüet (Warblade = omuz zırhı + iki elli kılıç; Elementalist = rün-diski; Ranger = yay; Shadowblade = ikiz hançer [tek sprite + flipX]...). Hepsi aynı 64px ölçek, aynı paylaşılan hitbox (capsule, prefab'da sabit — animasyondan bağımsız). Silahlar ele ANCHOR ile oturur (yön başına 1 anchor, gerekirse state override) — frame frame oturtma yok. Yeni animasyon üretimi demoda SIFIR: knockdown vb. kod-only (eğme + squash + gölge).

## 7. Oda inşa sistemi (IsoRoomBuilder) — Şekil 7
**Olması gereken:** Oda = veri (RoomTemplateSO: zemin hücreleri, cliff, prop socket'leri, düşman socket'leri, spawn, kapı slot'ları). Builder runtime'da bunlardan odayı kurar: zemin karoları → kenar analizi ile yönlü cliff (SW/SE void komşusuna göre, içeri tuck) → prop'lar → marker'lar. Hiçbir oda elle sahneye dizilmez; tek _Arena sahnesi her odayı data'dan kurar. Aynı sistem Chamber'ı da kurar (özel template).

## 8. İçerik üretim hattı — Şekil 8
**Olması gereken:** Yeni oda eklemek KOD GEREKTİRMEZ: oda JSON'u (elle/LLM üretimi) → RoomJsonImporter doğrular + RoomTemplateSO'ya çevirir → validator kontrol eder (zemin bütünlüğü, spawn yürünebilir, kapı kuralları) → otomatik prop yerleşimi (Poisson disk + kompozisyon rolleri) → smoke test (26/26 odanın hepsi exception'sız kurulmalı).

## 9-10. Geliştirme araçları — Şekil 9, 10
**Olması gereken:** Map Designer = tek pencere 8 sekme; Rooms sekmesi: 26 template listesi + arama + 2D şematik önizleme + tek tık "Build in Arena" + "Auto Props" (Undo'lu, 🎲 re-seed) + Save. Room Browser = sunum/hızlı-test aracı: listeden odayı tıkla → sahnede kurulu. Amaç: tasarımcı (tek kişi) odaları koda dokunmadan görsel olarak yönetir.

## 11. Sprite pipeline — Şekil 11
**Olması gereken:** Sınıf başına 5 yön üretilir (S, SE, E, NE, N — PixelLab), 3 yön runtime mirror (W←E, SW←SE, NW←NE — flipX). Tüm sprite'lar Point filter + PPU 64 + uncompressed; pivot = ayak çizgisi (alpha-scan ile ölçülür) → karakter karoya tam basar. Animasyon 10-12 fps.

## 13. Test güvencesi — Şekil 13
**Olması gereken:** EditMode + PlayMode test paketi her zaman yeşil; kritik akışlar (oda graph'ı, Echo ödül hesabı, knockback) unit-test'li; her oda template'i otomatik smoke-test'ten geçer. Rapordaki sayı güncel Test Runner sonucuyla birebir aynı olmalı.

## 14. Oda QC süreci — Şekil 14
**Olması gereken:** Her template görsel QC'den geçer: prop'lar ada dışına taşmaz, cliff dikişleri kapalı, sparse odalar yeniden seed'lenir. Hata bulunursa kök neden (ör. placer'ın yürünebilirlik kontrolü) düzeltilir + tüm odalar re-audit edilir — tek odayı elle yamamak yerine sistemik düzeltme.

## Ekonomi/meta (bağlam)
- **Shattered Echo (◈):** TEK meta para; run sonunda kazanılır (oda*3 + kill/5, 5-60 clamp). SADECE sınıf kilidi açar (80/150/200/250 veya achievement yolu). Skill/item satın alma YOK — "mağaza hissi" bilinçli reddedildi.
- **Ölüm:** ceza yumuşak — Echo'nu alırsın, Chamber'a dönersin, tekrar denersin (roguelite döngüsü).
