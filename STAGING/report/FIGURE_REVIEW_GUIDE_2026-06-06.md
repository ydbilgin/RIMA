# RAPOR ŞEKİL REVIEW REHBERİ (ChatGPT'ye verilecek)

Her şekil için: **ne göstermeli** + **kontrol kriterleri**. Görselle birlikte ChatGPT'ye yapıştır; "bu kriterleri sağlıyor mu, akademik raporda anlaşılır mı?" diye sor.

---

## Şekil 1 — `02_chamber_overview.png` — Attunement Chamber genel görünümü
**Ne göstermeli:** Karakter seçim odası (Attunement Chamber) geniş açıdan: hilal (crescent) dizilimde 10 taş pedestal, her birinin üstünde donuk/gri sınıf heykeli (echo), mor void arka plan, cyan rün/vurgu detayları, ortada gerçek oyuncu karakteri.
**Kriterler:** 10 pedestal'ın hepsi kadrajda mı? Kilitli sınıflar görsel olarak ayırt ediliyor mu (siyah silüet)? Oyuncu karakteri seçilebilir boyutta görünüyor mu? UI çöpü (debug yazısı, editör gizmosu) var mı?

## Şekil 2 — `03_chamber_g_prompt.png` — [G] bürünme anı
**Ne göstermeli:** Oyuncu bir pedestal'ın yakınında, ekranda dünya-içi **"[G] Bürün — <SINIF ADI>"** prompt'u görünür halde. İdealde bürünme anının cyan enerji efekti.
**Kriterler:** Prompt metni okunaklı mı? Hangi sınıfa bürünüleceği belli mi? Prompt karakterin yakınında mı (ekran köşesinde sabit UI değil, Hades tarzı dünya-içi)?

## Şekil 3 — `05_combat.png` — Tipik combat odası
**Ne göstermeli:** Aktif çatışma: oyuncu + en az 2-3 düşman, checker (damalı) zemin deseni, odaya yerleştirilmiş prop'lar (kaya/kemik/sütun), hasar/vuruş efekti anı idealdir. HUD görünür (can, skill bar).
**Kriterler:** Düşmanlar zeminde mi (havada görünen var mı)? Checker zemin algılanıyor mu? Prop'lar adanın dışına taşmış mı? HUD okunaklı mı?

## Şekil 4 — `06_draft_cards.png` — 3-kart skill draft
**Ne göstermeli:** Oda temizlenince açılan 3 skill kartı yan yana; bir kartın üstünde **hover tooltip açık** (skill açıklaması), varsa sinerji chip'i/pulse. Kart çerçeveleri (rarity glow) görünür.
**Kriterler:** 3 kart da tam kadrajda mı? Tooltip metni okunaklı mı? Kart ikonları yüklü mü (pembe kare/eksik sprite var mı)? Arka plan oyunu yeterince karartıyor mu?

## Şekil 5 — `07_branching_doors.png` — Çoklu kapı çıkışı
**Ne göstermeli:** Oda temizlendikten sonra odanın ARKASINDA (kuzey) yan yana 2-3 kapı (Hades tarzı), her kapının üstünde gideceği oda türünün rün simgesi (combat/elite/chest), kapılarda cyan rift parıltısı.
**Kriterler:** Kapılar düzgün hizalı tek sıra mı? Rün simgeleri ayırt edilebiliyor mu (hangisi elite, hangisi combat)? Kapı sayısı = dallanma sayısı anlatımıyla tutarlı mı?

## Şekil 6 — ÜRETİLECEK — Warblade karakteri
**Ne göstermeli:** Warblade'in büyük boy tek karakter görseli: omuz zırhı + iki elli kılıç silueti net seçilir; idealde idle_south sprite'ı 4-8x büyütülmüş (Point filter, pikseller keskin), nötr koyu arka plan.
**Kriterler:** Pixel art keskin mi (blur yok)? Silüet okunaklı mı? 64px karakterin detayı bu boyutta anlaşılıyor mu?

## Şekil 7 — `04_run_room_overview.png` — IsoRoomBuilder örnek oda
**Ne göstermeli:** Çalışma zamanında inşa edilmiş bir izometrik oda geniş açıdan: elmas zemin (floor), kenarlarda cliff (uçurum yüzü), serpiştirilmiş prop'lar, altta açık güney kenarı (void'e bakan cliff). Düşman olmadan ya da az düşmanla — amaç ODA SİSTEMİNİ göstermek.
**Kriterler:** Cliff'ler zeminle hizalı mı (taşma/boşluk var mı)? Ada hissi (void üstünde yüzen oda) veriyor mu? Prop'lar zeminin İÇİNDE mi?

## Şekil 8 — ÜRETİLECEK — JSON → RoomTemplateSO akış şeması
**Ne göstermeli:** Diyagram: `oda JSON dosyası → RoomJsonImporter (editor tool) → RoomTemplateSO (.asset) → IsoRoomBuilder (runtime) → sahnedeki oda`. Kutular + oklar; her adımda kısa açıklama (validation, socket'ler).
**Kriterler:** Akış yönü net mi? Teknik terimler raporla tutarlı mı? Akademik şema standardına uygun mu (el çizimi değil, temiz vektör/draw.io tarzı)?

## Şekil 9 — `11_map_designer.png` — Map Designer penceresi
**Ne göstermeli:** Unity editöründe RIMA/Map Designer penceresi, **8 sekme görünür** (Rooms sekmesi aktif): 26 template listesi + arama kutusu + seçili odanın 2D şematik önizlemesi + alt butonlar (Build in Arena yeşil / Auto Props mavi / Save turuncu).
**Kriterler:** 8 sekme sayılabiliyor mu? Önizleme şeması seçili odayla eşleşiyor mu? Buton renkleri/etiketleri okunaklı mı? Pencere kırpılmış mı?

## Şekil 10 — `12_room_browser_scene.png` — Room Browser + kurulu oda
**Ne göstermeli:** İki öğe birlikte: RIMA/Room Browser penceresi (template listesi) + arkada _Arena sahnesinde TEK TIKLA kurulmuş oda (Scene veya Game view). "Araçtan sahneye" ilişkisini göstermeli.
**Kriterler:** Pencere ile sahnedeki oda aynı template mi (isim eşleşiyor mu)? Sahne görünümü anlaşılır mı?

## Şekil 11 — ÜRETİLECEK — 8 yön sprite sheet
**Ne göstermeli:** Bir sınıfın (örn. Warblade) 8 yön idle sprite'ı tek satır/grid: S, SE, E, NE, N doğrudan üretilen 5 yön + W, SW, NW aynalanan 3 yön. İdealde aynalananlar etiketle işaretli ("mirror").
**Kriterler:** 8 yön de aynı boyda/hizada mı? Hangi yönlerin aynalandığı raporda anlatıldığı gibi mi (W←E, SW←SE, NW←NE)? Pikseller keskin mi?

## Şekil 12 — `02_chamber_overview.png` (Şekil 1 ile AYNI dosya)
**Ne göstermeli:** Şekil 1 ile aynı ekran ama bu kez **sınıf istasyonu/atmosfer** vurgusu: 10 sınıf istasyonu, mor void arka plan, cyan rünler.
**⚠️ Review notu:** Aynı görselin iki kez kullanılması akademik raporda zayıf durur — ChatGPT'ye sor: "Şekil 12 için farklı açı/yakınlaştırma gerekir mi?" Önerimiz: Şekil 12'yi pedestal yakın çekimi (tek heykel + rün detayı) ile değiştirmek.

## Şekil 13 — ÜRETİLECEK — Unity Test Runner sonucu
**Ne göstermeli:** Unity Test Runner penceresi, tüm testler yeşil (rapor "410 Pass / 0 Fail" diyor — güncel sayı neyse o görünmeli ve rapor metniyle TUTARLI olmalı).
**Kriterler:** Pass sayısı rapor metnindekiyle aynı mı? Yeşil tikler görünür mü? Pencere kırpılmadan okunaklı mı?

## Şekil 14 — ÜRETİLECEK — Oda QC öncesi/sonrası
**Ne göstermeli:** Yan yana iki kare: combat_large_diamond_01 odasının HATALI hali (örn. ada dışına taşan prop'lar) ve DÜZELTİLMİŞ hali. Hata bölgesi kırmızı daire/ok ile işaretli.
**Kriterler:** Fark ilk bakışta görülüyor mu? Hangi karenin "önce" hangisinin "sonra" olduğu etiketli mi?

---

## Kullanılmayan ekstra screenshot'lar (gerekirse yedek)
- `01_mainmenu.png` — ana menü (rapora "oyun açılışı" şekli eklenecekse)
- `08_run_map_overlay.png` — M tuşu run haritası (dallanma sistemini Şekil 5'ten daha iyi anlatabilir — ChatGPT'ye alternatif olarak sor)
- `09_boss_arena.png` — boss odası
- `10_end_screen.png` — victory/ölüm ekranı (+Shattered Echo ödülü)
- `12_room_browser_scene_editor.png` — Room Browser editör görünümü varyantı

## ChatGPT'ye önerilen soru kalıbı
> "Bu görsel, ekteki açıklama ve kriterlere göre bir bitirme projesi raporu için yeterli mi? (a) kriter kontrol listesi, (b) görsel kalite (okunabilirlik, kırpma, çözünürlük), (c) altyazı önerisi, (d) daha iyi bir alternatif kadraj öner."
