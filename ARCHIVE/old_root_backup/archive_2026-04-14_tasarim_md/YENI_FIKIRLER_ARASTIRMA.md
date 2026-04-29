# RIMA — Yeni Fikirler: Mob, Anlatım, Sinerji
*2026-04-01 — Web araştırması + RIMA tema sentezi —claude*

---

## BÖLÜM 1 — 5 YENİ MOB

### 1. Seam Weaver (Dikiş Dokuyan)
**Lore:** Boyutları bir arada tutan "dikiş dokusu" Fracturing'de bilinç kazandı. Var olmak için sürekli yeniden dikiyor ama her diktiği şey daha deforme oluyor.
**Görsel:** Uzun ipliksi kollar, gövdede iki boyutu birbirine bağlayan kırmızı yara izi, hareket ederken titreşen kesik çizgiler. Palet: kırık beyaz + açık kırmızı + soluk mor.
**Mekanik:** Odada iki nokta arasına görünmez "dikiş hattı" çeker. Dokunan oyuncu aynı odanın ayna versiyonuna geçer (çıkış ters tarafta). İki Seam Weaver birlikte → dikiş hattı kalıcı.
**Grudge:**
- Ateşle öldü → sonrakiler dikiş hatlarını ateş boyunca çizer
- Buz/slowla öldü → sonrakiler çok daha hızlı dikiş çeker
- Kendi dikişinin üstünde öldü → o dikiş o odada bir sonraki run'a kalır

---

### 2. Probability Ghost (Olasılık Hayaleti)
**Lore:** Fracturing birden fazla zaman çizgisini üst üste yığdı. Bu varlık aynı anda birden fazla olasılıkta var ama hiçbirinde tam değil.
**Görsel:** Aynı figürün 3 yarı-saydam silueti, RGB kaymalı (chromatic aberration). Hareket senkronize değil — üç karar bir arada.
**Mekanik:** Hasar sadece "gerçek" siluete işler (hafifçe daha opak olan). Doğru siluete 2 vuruş → diğerleri yok olur + zamansallık patlaması → etraftaki düşmanlar 1.5s dondurulur.
**Grudge:**
- Normal silahla öldü → sonrakiler silueti daha hızlı değiştirir
- AOE ile öldü → Grudge yok, sonrakiler AOE'ye immün
- 5+ saniye izlenerek öldü → sonrakiler çok daha yavaş kayar (izlenmeyi öğrendi)

---

### 3. The Unfinished (Yarım Kalanlar)
**Lore:** Fracturing yaratılış süreçlerini kesti. Bu varlıklar hangi formda var olacaklarını bilmiyor — bu acıyı etraftakilere yansıtıyor.
**Görsel:** Üst yarısı bir yaratıktan, alt yarısı başkasından. İki parça tutturulmuş görünüm, hareket animasyonu rahatsız edici (üst ve alt farklı ritimde). Referans: Hollow Knight Crystal Guardian.
**Mekanik:** Üst ve alt yarısı ayrı HP. Alt önce ölürse üst sürünür + yeni alt büyütür. Üst önce ölürse alt diğer düşmanlara yapışır, onlara hasar bonusu verir. 2 saniye içinde ikisi birden → "Completion Burst": tüm düşmanlar kısa sure serseme.
**Grudge:**
- Her zaman altta öldü → sonrakiler alt için zırh katmanı
- Her zaman üstte öldü → sonrakiler üste uzaktan saldırı kazanır
- Completion Burst ile → Grudge yok, ama Threshold'da lore parçası açar

---

### 4. Echo Anchor (Yansıma Çapası)
**Lore:** Bazı Shattered Echo parçaları yerde kaldı ve etraftaki gerçekliği sabitlemek için araç haline geldi. Bir class'ın gücünü absorbe etmiş, patlama eşiğinde.
**Görsel:** Merkezi kristal çapa + 2-4 küçük bağlı düşman, aralarında titreyen enerji çizgileri. Renk hangi class Echo'sunu tuttuğuna göre değişir.
**Mekanik:** Çapayı direkt vuramazsın. Bağlı düşmanlar öldürülünce çapa savunmasız OLUYOR AMA bağlı düşman öldürülünce enerji çapaya akar ve güçlenir. Önce doğru tip saldırıyla çapayı zayıflat (class lore ipucu), sonra bağlı düşmanları öldür. Doğru sıra → Shattered Echo fragmanı düşer.
**Grudge:**
- Yanlış sırayla → sonrakiler çapaya alan kalkanı ekler
- Sabırla doğru sırayla → sonrakiler çap-ağı kurar (iki çapa birbirine bağlı)
- O run'da hiç Echo toplanmadıysa → bağlı düşmanları serbest bırakır, kendisi agresif saldırır

---

### 5. The Recursion (Özyineleme)
**Lore:** Fracturing bazı boyutları sadece kırmadı — döngüye soktu. Bu varlık kendi içinde sonsuz kez tekrar ediyor. Karşılaştığın şey katman 47.
**Görsel:** Merkezi figür + etrafında zayıflayan 3-5 "geçmiş versiyon" hayaleti. Hareket ettiğinde hayaletler yarım saniye gecikmeli aynı hareketi tekrarlar — stroboskopik.
**Mekanik:** Her saldırı bir katman hayalet aktive eder → 3 saniye sonra o hayalet merkezi figürü "iyileştirir". Seçim: ya çok hızlı öldür (hayaletler aktive olmadan), ya önce hayaletleri temizle (ama temizledikçe merkezi hızlanır), ya da senkronize AOE ile ikisini birden yok et → "Recursion Break" → loot 2 katı.
**Grudge:**
- Hızlı burst → sonrakiler ilk anda hasar absorbu ekler
- Hayaletler temizlenerek → sonrakiler hayaletler ölünce saldırır
- Recursion Break → Grudge yok, "The Loop was broken" achievement, özel Threshold diyalogu

---

## BÖLÜM 2 — 5 ANLATIM TEKNİĞİ

### 1. Fracture Memory Odaları
Bazı odalarda düşman yok — sadece ortada bir parıltı. Basınca oyuncunun geçmişte verdiği kararın kısa sessiz animasyonu. Her ziyaret Threshold'da yeni diyalog açar. Akt 3'teki son oda kararın nedenini gösterir — final boss öncesi en güçlü anlatım anı.

### 2. Grudge Epitaphs (İntikam Yazıtları)
Öldürdüğün düşmanlar oda duvarlarında semboller bırakır. Bir sonraki run'da bu semboller "o mob ne yaşadı" yazıtları haline gelir. Yeterli Grudge biriktiğinde semboller birleşir → o mob'un kırılma öncesi neye benzediğini anlatan lore sayfası.

### 3. The Threshold Echo (Hub Anlatımı)
Her ölüm/run bitişinde Hub'ın köşesinde o run'un "en anlamlı anının" kısa sessiz canlandırması oynar. NPC'ler bunu görür ve spesifik yorum yapar. Probability Ghost'u bekleyerek öldürdüysen: *"Onu izledin. Uzun süre. Bu sabrı nereden biliyorsun?"*

### 4. Shard Text (Kırık Yazı)
Pixel art duvarlarındaki "bozuk" pikseller aslında kasıtlı. Belirli bir konuma gelince hizalanır ve 5-10 kelimelik bir ses ortaya çıkar — kırılma öncesinden: *"Bunu yapmalıyız." / "Başka yolu yok." / "Beni affet."* Yeterli parça toplandığında Threshold'da "Shard Library" açılır.

### 5. Class Ghost Monologues (Sınıf Hayaletleri)
Her Shattered Echo bir class yüzü. Her class'tan yeterli Echo toplandığında o class'ın "hayaleti" 2-3 cümlelik monolog söyler — 5 Echo: tanıtım, 10 Echo: kişisel anı, 20 Echo: Fracturing'de ne hissettikleri. Oyuncunun kendi ghost'u en sonda konuşur — final boss girişiyle örtüşür.

---

## BÖLÜM 3 — 5 CROSS-CLASS SİNERJİ

### 1. RESONANCE CASCADE (Void Dancer + Fracture Mage)
Void Dancer'ın her dash'i "Void Echo" bırakır. Fracture Mage'in Rift patlaması bu echo üstüne düşerse "Resonance Point" oluşur → 4 saniye boyunca o alana giren tüm düşmanlara zincirli hasar. **"Bu build insane" anı:** Koş → echo bırak → rift fırlat → sırtını dön → tüm düşmanlar birbirini tetikleyerek patlıyor.

### 2. MEMORY FORGE (Grudge Warden + Echo Collector)
Grudge Warden her Grudge tetiklendiğinde "Grudge Charge" üretir. Echo Collector her Echo'da "Echo Resonance" katmanı ekler. İkisi birleşince: Grudge Charge biriktiğinde mevcut Echo Resonance katmanını 2 katlar. **"Bu build insane" anı:** Kasıtlı Grudge biriktirerek gelen oyuncu 40 Charge + 12 Resonance ile boss odası → health bar 10 saniyede yarıya iniyor.

### 3. PHASE ECHO STRIKE (Seam Stalker + Temporal Striker)
Temporal Striker her güçlü saldırının gecikmeli kopyasını bırakır. Seam Stalker teleport yaptığında giriş ve çıkış noktası aktif kalır. Birleşince: gecikmiş kopya hem giriş hem çıkış noktasında ateşlenir. Düşman iki nokta ortasındaysa ikisinden birden vurulur. **"Bu build insane" anı:** Boss merkezde, oyuncu iki Seam arasında gidip geliyor, 4 farklı yerden aynı anda hasar alıyor.

### 4. SCHRÖDINGER'S BUILD (Null Weaver + Probability Dancer)
Probability Dancer her kullanımda 3 seçenekten rastgele ateşler. Null Weaver absorbe ettiği her hasar "Probability Token" üretir. İkisi birleşince: kasıtlı hasar al → token biriktir → çok daha fazla rastgele yetenek patlaması. **"Bu build insane" anı:** Boss kasıtlı vurulsun diye 8 token biriktiren oyuncu tek turda 8 rastgele patlama — 3 tanesi en güçlü variant çıkıyor.

### 5. THE FEEDBACK LOOP (Fracture Herald + Class Mimic Hunter)
Fracture Herald ne kadar lore açarsa o kadar güçlü. Class Mimic Hunter Class Mimic'ten yetenek "çalabilir." İkisi birleşince: Mimic'ten çalınan yetenek o run'da açılan lore miktarıyla ölçeklenir. **"Bu build insane" anı:** Tüm Fracture Memory odaları + Shard Text + Monologları toplayan oyuncu final Class Mimic'ten tüm run'ın birikmiş gücünü geri çalarak Nexus Core'a giriyor. Anlatı ve mekanik aynı anda zirveye ulaşıyor.

---

## TASARIM PRENSİBİ — HER MEKANİK AYNI ZAMANDA ANLATI

- **Grudge sistemi** → sadece zorluk değil, düşmanların hafızası lore oluşturuyor
- **Sinerji sistemi** → sadece güç değil, class seçimi kararın parçaları hakkında spekülasyon yaptırıyor  
- **Anlatım teknikleri** → sadece hikaye değil, combat dışında da keşif güdüsünü canlı tutuyor
- **Nexus Core'a ulaşınca** → oyuncu bir boss'la değil, yavaş geri kazandığı hafızasıyla yüzleşiyor

*Kaynak araştırmalar: Supergiant Games enemy design, Hades narrative design (Greg Kasavin), Dead Cells level design, Returnal narrative horror, GW1/WoW skill systems*
