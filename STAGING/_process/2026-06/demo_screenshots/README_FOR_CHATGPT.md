# RIMA — Ekran Görüntüleri + Güzelleştirme İncelemesi (ChatGPT için)
> RIMA = solo-geliştirilen 2D top-down ARPG roguelite (Unity URP 2D, pixel-art, high top-down 3/4 açı). Demo: 19 Haziran. Stil canon: koyu-ama-okunur, void MOR + slate gri zemin + cyan ≤%15 vurgu + ember turuncu; "dark fantasy / grimdark" DEĞİL. 8-yön karakterler, 120x120, PPU64.
>
> **ChatGPT'den istenen:** Aşağıdaki her ekran için — (a) görsel hiyerarşi/okunabilirlik/estetik değerlendirmesi, (b) somut **güzelleştirme** önerileri (renk, kontrast, spacing, tipografi, juice), (c) solo-geliştirici 3 günde yapabileceği "hızlı kazanç" vs "post-demo" ayrımı. Mekanik değil, GÖRSEL odak.
>
> Her başlıkta: **NE** (ekran ne) · **OLMASI GEREKEN** (tasarım niyeti) · **ŞU AN** (gözlem) · **🎨 GÜZELLEŞTİRME** (öneri tohumları — sen geliştir).

---

## 01_mainmenu.png — Ana Menü (DÜZ versiyon)
- **NE:** Açılış menüsü. "RIMA / Rift Avcıları" + "Yine geldin." + BAŞLA / AYARLAR / ÇIKIŞ.
- **OLMASI GEREKEN:** Atmosferik, oyunun void/rift temasını ilk saniyede satan bir karşılama.
- **ŞU AN:** Logo tipografisi şık (geniş harf aralığı, soğuk beyaz). AMA arka plan TAMAMEN DÜZ koyu gradyan — boş. Menü sol-altta sıkışık.
- ⚠️ **TUTARSIZLIK:** `01_arena_opening.png` AYNI menüyü **painted Shattered Keep backdrop** ile gösteriyor (çok daha güzel). Yani backdrop bazen yükleniyor bazen yüklenmiyor (timing/koşullu). İkisini karşılaştır.
- **🎨 GÜZELLEŞTİRME:** Backdrop'u HER ZAMAN göster + üstüne hafif parallax/ember-parçacık; menüyü dikey ortala veya sağ-panel kompozisyonu; logo'ya hafif cyan rim-glow.

## 01_arena_opening.png — Ana Menü (painted backdrop versiyonu)
- **NE:** Yukarıdakiyle aynı menü AMA arkada painted ruined-keep manzarası (mor gökyüzü, cyan rün-çatlakları, uçurum).
- **ŞU AN:** Bu versiyon belirgin biçimde daha iyi. Hedef bu olmalı. (01_mainmenu neden düz çıktı → araştırılacak.)
- **🎨 GÜZELLEŞTİRME:** Backdrop + menü kontrastını artır (menü arkasına hafif koyu vignette/gradient şerit ki yazı backdrop üstünde net okunsun).

## 02_settings.png — Ayarlar (AYARLAR)
- **NE:** Ayar paneli — OYNANIŞ (Dil), ERİŞİLEBİLİRLİK (5 toggle: ekran sarsıntısı/vuruş duraklaması/düşük-can efekti/hasar sayıları/kromatik sapma), SES (3 slider @80), KONTROLLER (tuş atamaları). Butonlar: Kontrolleri Sıfırla / Devam Et / Ana Menüye Dön.
- **OLMASI GEREKEN:** Net, taranabilir ayar listesi.
- **ŞU AN:** İşlevsel + iyi organize (gold başlıklar, teal butonlar, cyan slider'lar). AMA panel menünün üstünde "yüzüyor" (arka plan dim yok → menü logosu soldan sızıyor); satırlar biraz sıkışık; cyan slider dolu-kısmı çok parlak.
- **🎨 GÜZELLEŞTİRME:** Panel arkasına tam-ekran dim/blur; satır yüksekliği +biraz nefes; toggle'lara AÇIK=cyan / KAPALI=gri net renk-kod; panel'e ince köşe-çerçeve.

## 03_characterselect.png — Karakter/Sınıf Seçim Odası
- **NE:** Oyun-içi oda; oyuncu (warblade) ortada, yeşil-saçlı bir NPC + küçük kılıçlı figür + mavi portal ("1") + sağda bir SIRA düşman silüeti. Altta "RAVAGER — Kiiiiii" monolog.
- **OLMASI GEREKEN:** Sınıf seçtiğin atmosferik giriş odası.
- **ŞU AN:** ⚠️ Düşmanlar **düz SİYAH siluet blob** olarak görünüyor (placeholder hissi; sprite yok ya da tamamen gölgede). Kompozisyon dağınık — oyuncunun ne yapması gerektiği belirsiz. Portal güzel duruyor.
- **🎨 GÜZELLEŞTİRME:** Düşman silüetlerine en azından rim-light/iç-renk ver (tam siyah blob kötü); sınıf-seçim noktalarını net işaretle (parlama/ikon); monolog barını okunur kontrastta yap.

## 06_opening_draft.png + 05a_fullflow_gameplay.png — Ödül/Kit Draft (ÖDÜL SEÇ)
- **NE:** Oda temizlenince çıkan 3-kart ödül seçimi ("ODA 1 — ÖDÜL SEÇ"). Kartlar: IRON CHARGE / EARTHSPLITTER / GRAVITY CLEAVE — ikon + başlık + rarity + açıklama + SEÇ butonu. (06'da oyuncu altta görünür.)
- **OLMASI GEREKEN:** StS-tarzı net, iştah açıcı kart seçimi; rarity bir bakışta okunur.
- **ŞU AN:** Kart çerçeveleri makul (koyu teal), ikonlar var. AMA: açıklama yazısı ÇOK küçük/soluk; rarity şeridi küçük; kartlar arası boşluk dar; arka plan oyun-dünyası bulanık değil → dikkat dağıtıyor.
- **🎨 GÜZELLEŞTİRME:** Kart açıklamasını büyüt + kontrast; rarity'yi renk+kenar-glow ile güçlendir (Common gri / Rare mavi / Epic mor); kart hover'da hafif scale+glow; arka planı koyu blur'la kartları öne çıkar.

## 07_gameplay_hud.png — Oynanış + HUD (temiz)
- **NE:** Aktif oda; oyuncu ortada cyan-kılıçla + kırmızı NİŞAN ÇEMBERİ; üst-solda küçük HUD; alt-ortada 3 skill slotu.
- **OLMASI GEREKEN:** Net, okunur combat HUD; zemin/karakter ayrışır.
- **ŞU AN:** ⚠️ EKRANI AĞIR **KIRMIZI TINT** kaplıyor (düşük-can vignette ya da bir tint efekti çok güçlü) → zemin rengini eziyor, alarm hissi veriyor. HUD çok minimal (üst-sol minik, alt 3 boş slot kutusu) → kaynak barları/can görünmüyor. Nişan çemberi okunur.
- **🎨 GÜZELLEŞTİRME:** Kırmızı tint'i sadece GERÇEKTEN düşük-can'da + daha hafif/kenarlardan uygula (full-screen kırmızı yanlış izlenim); HUD'a net can barı + kaynak barı + skill slot ikonları/cooldown; slot kutularına ikon+tuş-harfi.

## 08_runmap.png — Koşu Haritası (KOŞU YOLU, M tuşu)
- **NE:** StS-tarzı dallı düğüm haritası — altta 0:Combat, yukarı doğru Combat/Elite/Merchant dalları, tepede 13:Boss. Renk-kod: teal=combat/merchant, mor=elite, kırmızı=boss.
- **OLMASI GEREKEN:** Stratejik rota planı; düğüm tipleri ikonla bir bakışta okunur.
- **ŞU AN:** ✅ Branching ÇALIŞIYOR (dallanma doğru). AMA düğümler düz DİKDÖRTGEN+metin (ikon yok); bağlantı çizgileri çok soluk/belirsiz; "mevcut konum" vurgusu zayıf; alt iki panel boş/soluk.
- **🎨 GÜZELLEŞTİRME:** Düğümlere tip-ikonu (kılıç/elite-kafatası/kese/boss-kuru kafa); bağlantı yollarını belirgin kesik-çizgi; mevcut düğüme parlak halka; tamamlanan/erişilebilir/kilitli düğüm durumlarını renkle ayır.

## 09_director.png — Director Mode (canlı sunum aracı, ` tuşu — dev/demo)
- **NE:** Hoca-sunumu canlı kontrol aracı: SPAWN paneli (düşman ekleme), zaman-skalası, stat okuması (ODA/KILLS/SKORE/SHATTERED ECHO), TEKRAR DENE / ANA MENÜ. Turuncu sci-fi çerçeve UI.
- **OLMASI GEREKEN:** Sunumda etkileyici "tanrı-modu" gösterge paneli.
- **ŞU AN:** İşlevsel + tema tutarlı (turuncu HUD). AMA ÇOK KALABALIK — köşe çerçeveleri + merkez monolog + stat bloğu + alt bar üst üste; "Director bleed" (room-clear monologu merkeze sızıyor) okunabilirliği bozuyor. Bilgi yoğun, hiyerarşi zayıf.
- **🎨 GÜZELLEŞTİRME:** Panelleri grupla + boşlukla; merkez monolog'u Director açıkken gizle (bleed-fix kodda var, doğrula); stat bloğunu tek temiz panele al; aktif araç vurgusu.

## 10_buildmode.png — Build Mode (oyun-içi seviye editörü, F2 — dev/demo CENTERPIECE)
- **NE:** Oyun-içi prop/tile yerleştirme editörü. Sol: BUILD paleti (PROP/TILE sekmeleri + asset ikonları: kaya, sütun, mangal...). Merkez: arena mor wireframe-grid olarak + oyuncu + nişan çemberi.
- **OLMASI GEREKEN:** Sunumun "WOW" anı — canlı edit-to-play.
- **ŞU AN:** İşlevsel editör görünümü; grid + palet net. AMA grid çok baskın/parlak (mor) zemini gölgeliyor; palet ikonları küçük/etiketsiz; "build modundasın" durum-göstergesi zayıf.
- **🎨 GÜZELLEŞTİRME:** Grid'i daha soluk/ince; palet ikonlarına etiket+hover; aktif araç + seçili asset vurgusu; "BUILD MODE" durum-rozeti; yerleştirilebilir alan vs değil renk-feedback.

## 11_codex.png — Yetenek Kodeksi (YETENEK KODEKSİ, ESC)
- **NE:** Tüm sınıfların yetenek ansiklopedisi. Üst: 10 sınıf sekmesi (Warblade/Elementalist/Shadowblade/Ranger/Ravager/Ronin/Gunslinger/Brawler/Summoner/Hexer). Warblade seçili → skill listesi (ikon+ad+açıklama+rarity/CD).
- **OLMASI GEREKEN:** Kapsamlı, taranabilir referans.
- **ŞU AN:** ✅ EN İYİ EKRANLARDAN BİRİ — iyi organize, gold başlık, sınıf sekmeleri, rarity renk-kodu (Common/Rare/Epic). AMA satırlar çok sık (yoğun); ikonlar küçük; uzun listede scroll göstergesi belirsiz.
- **🎨 GÜZELLEŞTİRME:** Satır yüksekliği +nefes; ikon büyüt; rarity'yi sol-kenar renk şeridiyle de göster; seçili sınıf sekmesini daha belirgin; arama/filtre.

## 12_pause.png — Duraklatma Menüsü (PAUSED, ESC)
- **NE:** Resume / Settings / Skill Codex / Main Menu / Quit. Küçük ortalı koyu panel.
- **ŞU AN:** İşlevsel (PauseMenu artık VAR). AMA panel çok küçük/sade; ARKA PLAN SIZINTISI — arkadaki room-clear "TEKRAR DENE" butonları + monolog + nişan çemberi panelin yanından görünüyor (dağınık).
- **🎨 GÜZELLEŞTİRME:** Pause açıkken tam-ekran dim/blur (arka sızıntıyı kapat); panel'i biraz büyüt + başlığa cyan accent; buton hover.

## 13_tab_skillbar.png — Oda-Temizlendi / Run Özeti Overlay
- **NE:** Oda-clear/run-bitti özeti — "Rift hatırlar. Sen hatırlamayacaksın." monolog + stat (ODA/KILLS/SKORE/SHATTERED ECHO) + TEKRAR DENE [R] / ANA MENÜ.
- **ŞU AN:** Monolog atmosferik. AMA stat bloğu küçük/ortada dağınık; "ODA TEMİZLENDİ" benzeri net başlık yok; düşmanlar yine arka planda siyah blob.
- **🎨 GÜZELLEŞTİRME:** Net büyük başlık ("ODA TEMİZLENDİ" / "ZAFER"); stat'ları hizalı panel; ödül/echo kazanımını vurgula; geçiş animasyonu.

---
## ULAŞILAMAYAN EKRANLAR (oda/duruma bağlı — bu pakette YOK)
ChestUI (sandık odası) · ForgeUI (örs odası) · BossHealthBar (boss savaşı) · DeathScreen (ölüm) · DemoCompleteOverlay (run bitişi). Bunlar golden-path otomasyonunda kolay ulaşılmadı; istenirse ayrı tur ile yakalanır.

## GENEL TEMALAR (ChatGPT'nin bütüncül bakması için)
1. **Arka-plan dim/blur eksikliği** birçok overlay'de (Settings/Pause/Director) → sızıntı + okunabilirlik sorunu. Tek bir paylaşılan "modal scrim" çözer.
2. **Kırmızı tint (07)** çok agresif → combat okunabilirliği.
3. **Düşman sprite'ları siyah blob** (03/13) → karakter-okunabilirlik.
4. **Tipografi/spacing:** kart + codex + stat bloklarında yazılar küçük/sık.
5. **İkonografi:** run-map düğümleri + build paleti + skill slotları ikonsuz/etiketsiz.
6. **Güçlü yanlar:** logo tipografisi, painted backdrop, codex organizasyonu, turuncu Director teması, renk canon tutarlılığı.
