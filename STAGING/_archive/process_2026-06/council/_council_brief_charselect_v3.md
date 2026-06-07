# COUNCIL BRIEF — CharacterSelect v3 + Skill-Echo-Unlock Decision (2026-06-05)

## Amaç
Kullanıcı bir CharacterSelect konsept görseli üretti (ChatGPT image). Council bunu inceleyecek, 
(1) skill'lerin de Echo ile unlock'lanıp lanmayacağına karar verecek (precedent araştırması ile), 
(2) görsel iyileştirme yönü önerecek. Görsel = YÖN REFERANSI, birebir kopya DEĞİL 
(kullanıcı: "tam olarak böyle olmasına gerek yok").

## Girdiler (OKU)
- **Konsept görsel:** `STAGING/mockups/charselect_concept_ref_2026-06-05.png` (1672×941, görüntüleyebiliyorsan BAK)
- **ChatGPT'nin tasarım önerisi:** `STAGING/mockups/charselect_chatgpt_brief_2026-06-05.md` 
  ⚠️ Bunu DİREKT BENİMSEME — sadece girdi/ilham. Kendi bağımsız fikrini sun. ChatGPT'nin önerileriyle 
  çelişen daha iyi fikrin varsa söyle.
- **Mevcut HTML mockup:** `STAGING/mockups/charselect_mockup.html` (önceki iterasyon)
- **Önceki kararlar:** `STAGING/SODAMAN_LEARNINGS_DECISION_2026-06-04.md` (currency çakışması §), 
  `STAGING/CHARSELECT_ROSTERROOM_DECISION_2026-06-04.md` (varsa)

## Görselin metin tarifi (görüntüleyemeyenler için)
- Üst bar: sol "RIMA — KARAKTER SEÇ", sağ "80 ECHO".
- Merkez: büyük iso yüzen taş ada (cyan çatlaklı karolar), 2 sütun + brazier (turuncu alev), void-mor gökyüzü.
- 10 karakter 2 sıra (5+5) karoların üstünde: ön sıra Warblade (SEÇİLİ — ayak altında cyan halka/glow, 
  isim etiketi), Elementalist/Ranger/Shadowblade/Ronin (kilit ikonu); arka sıra Ravager 120 / 
  Gunslinger 160 / Brawler 160 / Summoner 160 / Hexer 200 ECHO (kilit + maliyet etiketi).
- Sol panel: WARBLADE / HEAVY·MELEE·RAGE / motto / açıklama / RAGE kaynak açıklaması / 
  5 stat barı (Hasar, Dayanıklılık, Hız, Kontrol, Zorluk).
- Sağ panel: YETENEKLER — 3 aktif skill (ikon+kısa açıklama: Demir Hamle, Yarık, Öfke Patlaması); 
  altında "USTALIK YETENEKLERİ" — 3 kilitli slot "??? Açmak için 400 ECHO gerekli".
- Alt orta: büyük "SEÇ" butonu.

## KULLANICI DİREKTİFLERİ (SABİT — tartışılmaz, tasarım bunlara uymak ZORUNDA)
1. Tek ekran, **dikey scroll YOK**; öğeler **yukarı-aşağı hareket de etmesin** (bob/parallax yok — 
   her iki yorum da kapsanacak).
2. **Adanın altında resim/dekor olmasın** (alt boşluk temiz/karanlık void).
3. Karakterler **karolara gerçekçi otursun** (tile-aligned, ayaklar karo üstünde, havada durmasın).
4. Karakter **boyutları aynı kalsın** (seçilince büyüme yok; tile ölçeğiyle orantılı).
5. **Kilitli karakterler = OPAK SİYAH silüet** — saydam/dim DEĞİL, kapkaranlık. 
   ⚠️ ChatGPT "tamamen siyah blob olmasın" diyor — KULLANICI BUNU OVERRIDE ETTİ: siyah olacak. 
   (Mevcut Unity implementasyonunda siyah-silüet zaten var, `567b8c75`.)
6. Echo **satın alınamaz** — sadece run'lardan kazanılır; ekranda sadece mevcut miktar görünür, 
   mağaza hissi YASAK.
7. Seçim = karakterin **üstüne tıklama**; seçili vurgusu = sadece glow/halka/aura.
8. Alt portrait strip / pedestal / hero-showcase YOK.

## AÇIK KARARLAR (council'in işi)
### K1 — Skill'ler de Echo ile mi unlock'lansın? (ANA KARAR)
Görseldeki "USTALIK YETENEKLERİ — 400 ECHO" fikri: karakter unlock'una ek olarak per-class mastery 
skill'leri de aynı meta-currency ile açılsın mı?
- **DETAYLI PRECEDENT ARAŞTIRMASI YAP (web):** Hangi roguelite/ARPG'ler meta-currency ile hem karakter 
  hem skill unlock yapıyor? (örn. Hades Mirror/Aspects, Dead Cells, Rogue Legacy 2, Vampire Survivors, 
  Halls of Torment, Soulstone Survivors, Death Must Die...) Nasıl çalışıyor, neyi iyi/kötü yapıyor?
- Tek havuz (Echo her şeye) vs ayrı para vs skill≠para (skill'ler achievement/oynayışla açılır) — 
  artı/eksi + RIMA için NET ÖNERİ ver.
- Ekonomi dengesi: karakter 120-200 Echo iken skill 400 Echo mantıklı mı? Run başına kazanç ne olmalı?
- RIMA bağlamı: skill'ler run-içi draft ile seçiliyor (3-kart Rift Seal). Meta-unlock = draft havuzuna 
  ekleme mi, yoksa kalıcı aktif mi? Bunun draft sistemiyle etkileşimini düşün.
### K2 — Currency adı
"Echo" çakışması: meta-unlock-currency Echo ↔ gameplay "shadow Echo" mekaniği aynı isim; run-parası Gold. 
NLM canon'a göre öneri ver (örn. meta-currency rename "Vestige"?). Kullanıcı "Echo"dan emin değil.
### K3 — Görsel iyileştirme ("şurayı güzelleştirelim")
Direktiflere uyarak: panel tasarımı, üst bar, alt buton, ada kompozisyonu, brazier/sütun yerleşimi, 
karakter dizilimi (2 sıra 5+5 mi, yay mı?), isim/kilit/maliyet etiket tasarımı. 
HTML-first akış: önce `charselect_mockup.html` iterasyonu → kullanıcı onayı → cx Unity'ye benzetir.

## RIMA mevcut durum (bağlam)
- CharacterSelect roster-room Unity'de ÇALIŞIYOR (fonksiyonel: tıkla-seç, unlock Echo harcama, 
  locked-seçilemez, siyah silüet — commit `567b8c75`). Görsel redesign = HTML-first.
- SkillDatabase'de sadece 4 sınıf kayıtlı (Warblade/Elementalist/Shadowblade/Ranger); per-skill lock 
  kodu YOK (sadece class-level lock var).
- Skill ikonları: Warblade+Elementalist 8 ikon var (`Assets/Sprites/UI/Icons`).
- 10 sınıf: Warblade, Elementalist, Ranger, Shadowblade, Ronin, Ravager, Gunslinger, Brawler, 
  Summoner, Hexer. Default = Warblade.
- Tema canon: dark-fantasy + cyan rift (#00FFCC) + void-mor + warm-orange brazier (#E89020); 
  "UI yoktur sadece bilgi vardır" (opak kutu yasak, ink-on-paper).

## 📜 NLM CANON (sorgu 2026-06-05 — advisorlar BUNU baz alsın; tam çıktı `STAGING/_nlm_charselect_currency.json`)
- **Meta-currency canonical adı = "Shattered Echoes"** ("Echoes"). Kullanımı: class unlock + hub NPC 
  upgrade'leri (Cartographer 100 Echo harita-görüş, Vrel 50 Echo augment craft). Lore: Echoes = 
  Fracturing'de saçılan class "yüzleri"; toplayınca bütünleşiyorsun.
- **Echo isim çakışması canon'da GERÇEK ve YAYGIN:** Echo Mode (kolay zorluk) · Echo Imprints (draft 
  mekaniği) · Shadow Echo (Shadowblade mimic) · Cross-Class Echoes · Echo Twin (Act2 boss) · Fracture 
  Echoes (boss varyant) · Chain Warden Echo / Echo Hound / Echo Striker (moblar). K2 bunu çözmeli.
- **⚠️ CANON: Skill'ler meta-currency ile UNLOCK'LANMAZ.** Kalıcı skill tree YOK. Her class'ın 12 common 
  skill'i BAŞTAN açık; build tamamen run-içi 3-kart draft ile (StS-style); ölünce build sıfırlanır. 
  → K1 = "canon'u değiştirelim mi?" sorusudur. Görseldeki "Ustalık 400 Echo" fikri canon'a AYKIRI. 
  Council: canon'u korumak vs amend etmek — precedent araştırmasıyla gerekçeli karar ver.
- **Karakter unlock canon tablosu (Echo VEYA achievement alternatifi — çoğunda OR yolu var):**
  Warblade=default · Elementalist&Ranger=80 · Shadowblade=150 OR Act1×3 · Ravager=150 OR Act2-boss-w-Warblade · 
  Ronin=150 OR Act2-boss-w-Shadowblade · Gunslinger=200 OR Act2-w-Ranger · Brawler=200 OR Act2-w-Ravager · 
  Summoner=200 OR Act2-in-3-consecutive-runs · Hexer=250 AND Elementalist-run-complete.
  (Görseldeki 120/160/200 fiyatlar canon'dan farklı; dual-path unlock görselde hiç yok.)

## Çıktı formatı (her advisor)
1. **K1 kararı:** precedent tablosu (oyun → mekanik → ders) + RIMA önerisi + ekonomi taslağı.
2. **K2:** currency adı önerisi + gerekçe.
3. **K3:** görsel iyileştirme önerileri (direktiflere uygun, somut).
4. **Görsel kritik:** konsept görselde işleyen/işlemeyen 3-5 madde.
5. ChatGPT önerisinden AL/ATLA listesi (neden ile).
