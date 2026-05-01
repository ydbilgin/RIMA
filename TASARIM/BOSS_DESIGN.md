# RIMA — Boss Tasarımı: Çok Fazlı Savaşlar

*Son güncelleme: 2026-04-06 | Tasarım kararı: Claude*

---

## Tasarım Felsefesi

Her boss, oyun akışındaki o anın duygusunu somutlaştırır.

| Boss | An | Duygu |
|------|-----|-------|
| Act 1 Boss | Kırılma öncesi | "Bir şeyler bitiyor — ve değişiyor" |
| Act 2 Boss | Dual-class ilk master | "Ben artık iki şeyim" |
| Act 3 Boss | Full build doruk | "Bu build insane — ve bu boss bunu sınıyor" |
| Final Boss | Her şeyin kapanışı | "Buraya kadar geldim. Ve şimdi her şey açık." |

Multi-faz geçişleri sadece HP eşiği değil — her geçiş **anlatı momenti**. Kamera duraklıyor, efekt çalıyor, müzik kırılıyor.

---

## ACT 1 BOSS — The Penitent Sovereign (3 Faz) [S42 — GÜNCEL]

> **[S43 GÜNCEL]** 2-fazlı eski tasarım geçersiz. Aşağıdaki 3-fazlı versiyon canonical.

**Tema:** Boyun eğmiş ama kırılmamış. Zincirleriyle savaşır — ta ki onları kırana kadar.

**Lore:** Bölgenin eski koruyucusu. Kendini cezalandırmak için rift enerjisini içinde hapsetti. Zincirleri artık fiziksel kilit değil, kendi öz disiplinin metaforu — kırıldıkları an yeni bir form ortaya çıkıyor.

**Arena:** 14×14 tile dairesel taş platform (Act 1 ritüel kalıntısı). 2 dash lane karşılıklı (kuzey-güney). Faz 2'de orta noktada Rift Tear hazard belirir, dash lane'ler tek-yönlü olur.

**Overlap-safety kuralı:** Shackle Cast → Chain Detonation arasında minimum 0.8s gap zorunlu.

**Sovereign's Wrath güvenli daire:** Center sabit OLMAZ — center / edge / boss-behind arc arasında döner (pattern okuma zorunlu).

### Faz 1 — "Zincirin Altında" (HP: 100% → 66%)

Telegraphed kit, oyuncu öğrenir. Pattern: Whip → Surge → Whip → Shackle.

| Skill | Tell | Etki | Window |
|---|---|---|---|
| **Chain Whip** | 0.8s — kol geriye | 6m düz çizgi 30 hasar | 1.0s |
| **Penitent Surge** | 1.2s — yumruk yere | 4m radius itme + 35 hasar | 1.5s |
| **Shackle Cast** | 1.0s — zincir havalanır | 8m menzil tek hedef chain → 2s slow %50 + 15 hasar | 1.2s |

**Faz geçiş eşiği: %66 HP**

**Geçiş sahnesi (1.5s):** Sovereign yere çöker. Göğsünden mor ışık fışkırır. "...artık yetmez." → zincirleri kırılır, hız +%30. Arena merkezinde Rift Tear (3m radius hazard) belirir.

### Faz 2 — "Kırılan Zincir" (HP: 66% → 33%)

Hız +%30 (Faz 1'e göre). Rift Tear orta hazard aktif.

| Skill | Tell | Etki | Window |
|---|---|---|---|
| **Fracture Strike** | 0.5s — dash forward | 3 ardışık swing (sol-sağ-orta), her biri 22 hasar | 0.8s son swing'den sonra |
| **Chain Detonation** | 1.0s — zincir parçaları zemine saplanır | 3 nokta marker, 2.5s sonra her biri 2m radius patlama (40 hasar) | 1.5s (parça yerleştirme sonrası) |
| **Shackle Cast** (carry-over) | 1.0s | Faz 1 ile aynı | 1.2s |

**Faz geçiş eşiği: %33 HP**

**Geçiş sahnesi (2.0s):** Sovereign havaya kalkar, kalan tüm zincirler erir, gövdesi yarısı rift enerjisiyle değişir. Müzik tema değişir. Hız toplam +%50 (Faz 1'e göre).

### Faz 3 — "Sovereign Awakened" (HP: 33% → 0%)

Desperation faz. Wrath'ın güvenli dairesi merkezdedir — ama Faz 2'den kalan Rift Tear orada (dış kenar = Charge riski, iç merkez = Tear hazard). 30-45s kill hedefi.

| Skill | Tell | Etki | Window |
|---|---|---|---|
| **Fracture Charge** | 0.6s — başlangıç pozisyonu glow | Arena boyunca dash + 50 hasar düz çizgi | 1.5s charge sonrası |
| **Sovereign's Wrath** | 1.5s — zemine kök salar, çevre kızarır | Tüm arena hasar (60) HARİÇ orta 2m güvenli daire. **Daire center/edge/boss-behind arc arasında döner.** | 2.0s recovery |
| **Chain Detonation** (hızlı) | 0.7s tell, 4 nokta, 1.5s patlama | Faz 2 versiyonu hızlandı | 1.0s |
| **Fracture Strike** (hızlı) | 0.4s | 3-hit combo +%20 hasar | 0.6s |

**Ölüm sahnesi (2.5s):** Sovereign çöker. "...sonunda boş." Zemin çatlar → secondary class seçimi açılır (Faz 2 unlock cue; Faz 1'de placeholder credits).

**Özet tablosu güncelleme:**

| Boss | Faz Sayısı | Geçiş HP | Süre Hedefi |
|---|---|---|---|
| Act 1 — Penitent Sovereign | **3** | **%66 / %33** | **3-4 dk** |

---

## ACT 2 BOSS — The Echo Twin (2 Faz)

**Tema:** Oyuncunun dual-class dönüşümünün yansıması. İki varlık — ama tek beden.

**Lore:** Fracturing'de iki kimliği aynı anda taşıyan biri. İkisi de çıkmak istiyor. Oyuncu dual-class'ını ustalıkla kullanarak bu ikiliği bozabilir.

**Tasarım amacı:** Cross-class Ultimate'in kullanılmasını mekanik olarak teşvik et.

### Faz 1 — "Birinci Kimlik" (HP: 100% → 40%)

Boss, dominant kimliğiyle savaşır. Saldırıları okunabilir, pattern'ı nettir.

*Run'da hangi primary class seçildiğine bağlı olarak Echo Twin'in birinci kimliği değişir:*
- Warblade aldıysan → Echo Twin melee agresif
- Elementalist aldıysan → Echo Twin ranged mage
- Shadowblade aldıysan → Echo Twin stealth/burst
- Ranger aldıysan → Echo Twin kiting saldırgan

*(Her varyant için 4 ayrı saldırı seti — implementation sonraya bırakılabilir, ilk build'de tek varyant yeterli)*

**Örnek (Melee varyant):**

| Saldırı | Mekanik |
|---------|---------|
| **Mirror Slash** | Oyuncunun son kullandığı skill'in ayna kopyası |
| **Identity Strike** | 5m dash + hasar |
| **Echo Barrier** | 3s kalkan — hasar azaltır |
| **Twin Pulse** | 2m AoE patlama, oyuncuyu iter |

**Faz geçiş eşiği: %40 HP**

**Geçiş sahnesi (2 sn):**
> Boss ikiye "yırtılıyor" gibi görünür — iki silüet belirir, sonra tekrar birleşir. İkinci kimlik dominant olur. Ses tonu değişir, renk paleti kayar.

### Faz 2 — "İkinci Kimlik" (HP: 40% → 0%)

Tamamen farklı saldırı seti. Önceki faza hiç benzemiyor.

| Saldırı | Mekanik | Tasarım Amacı |
|---------|---------|---------------|
| **Duality Surge** | Her iki kimliğin gücü birden — büyük AoE | Cross-class Ultimate tetiklemezsen çok zor |
| **Phase Shift** | Birkaç saniye görünmez, sonra arkana çıkar | Pozisyon yönetimi |
| **Echo Cascade** | Birinci fazın saldırılarını 2x hızda tekrarlar | Oyuncu artık bunları biliyor |
| **Twin Collapse** | Enerjisini toplar, 3s sonra patlama — yüksek hasar | Yeterince burst yapmazsan yüksek hasar |

**Tasarım notu:** Faz 2'ye girince "Cross-class Ultimate kullan" baskısı hissettirmeli ama zorunlu değil. İyi bir oyuncu Ultimate'siz de geçebilmeli — ama Ultimate kullanınca belirgin şekilde daha kolay.

---

## ACT 3 BOSS — The Fracture Sovereign (3 Faz)

**Tema:** Fracturing'in ta kendisi. Bu bir varlık değil — yaranın kendisi.

**Lore:** Fracturing ilk burada açıldı. Bu "alan" zaman içinde bir bilinç kazandı. Oyuncu burada dünya yarığının merkezinde savaşıyor.

**Tasarım amacı:** Full dual-class build'in tüm bileşenlerini zorlasın. Her faz farklı bir yetenekler setini zorunlu kılar.

### Faz 1 — "Yara Açıldı" (HP: 100% → 60%)

Saldırılar basit ama sert. Boss stagger'lanamaz.

| Saldırı | Mekanik |
|---------|---------|
| **Fracture Beam** | Yavaş dönen lazer çizgisi |
| **Void Shard** | 3 yönde projectile |
| **Sovereign Step** | Boss teleport — rastgele konuma |
| **Gravity Pull** | Oyuncuyu kenara çeker, sonra bırakır |

### Faz 2 — "Çevre Uyanıyor" (HP: 60% → 30%)

**Geçiş sahnesi (2 sn):** Zemin çatlar. Arena'nın 4 köşesinde "Fracture Node" aktive olur — bunlar hasar veriyor. Boss kendi etrafında bir koruyucu enerji döndürmeye başlar.

Faz 2'de arena küçülür (hazard zone genişler). Çalışma alanı daha dar.

| Saldırı | Mekanik |
|---------|---------|
| **Node Pulse** | Her 15 saniyede nodlar patlıyor | 
| **Fracture Wave** | Zemin boyunca yayılan dalga — atlamak için pencere |
| **Shatter Field** | Alanda dönen orbs, temas = slow |
| **Sovereign Gaze** | 3s işaret + büyük hasar — block edilebilir |

**Ek mekanik:** Fracture Node'ları vurarak öldürebilirsin → her öldürme boss'a %5 hasar. Boss'u daha hızlı öldürmenin alternatif yolu.

### Faz 3 — "Tam Kırılma" (HP: 30% → 0%)

**Geçiş sahnesi (2.5 sn):** Boss havaya yükselir. Müzik sıfırlanır — sadece sessizlik, sonra yeni tema başlar. Arena tamamen değişir: zemin "boşluk"a dönüşür, sadece platform adacıkları kalır.

| Saldırı | Mekanik |
|---------|---------|
| **Fracture Surge** | Boss'un tüm önceki saldırıları sırayla — ama 2x hızda |
| **Void Collapse** | Arena'nın bir bölümü çöker, platform kayar |
| **Sovereign's Final Form** | 5s hazırlanır, sonra tüm alana hasar (hayatta kalmak için belirli platforma geçmek lazım) |
| **Echo of Pain** | Oyuncunun aldığı son büyük hasarın kopyasını geri fırlatır |

**Ölüm sahnesi:** Arena yeniden birleşir. Zemin sağlamlaşır. Sessizlik. Final Boss kilidi açılır.

---

## FINAL BOSS — The Architect (4 Faz)

**Tema:** Fracturing'i yaratan varlık. Her şeyin kaynağı — ve belki, bir oyuncu kadar eskiyen biri.

**Lore:** Fracturing bir kaza değildi. Architect, dünyayı kasıtlı kırdı — çünkü kırık dünya daha fazla "potansiyel" barındırır. Oyuncu tüm bu potansiyeli içinde topladı. Şimdi asıl savaş bu.

**Tasarım amacı:** Her faz önceki boss'lardan bir ders alır. Oyuncu tüm run boyunca öğrendiklerini burada kullanmak zorunda kalır.

### Faz 1 — "Tanıdık" (HP: 100% → 75%)

Act 1 boss'u mirror'lar. Neredeyse birebir aynı saldırılar — ama oyuncu artık çok daha güçlü.

> "Beni tanıyor musun? İlk engelin bendim."

Tasarım amacı: Oyuncuya ne kadar ilerlediğini hissettir. Act 1 boss'u dev gibi hissettirmiş olabilir — burada aynı saldırılar artık kolay geliyor.

### Faz 2 — "Bozulan" (HP: 75% → 45%)

**Geçiş sahnesi (2 sn):** Architect'in görüntüsü "glitch"ler. Fiziksel formu bozulmaya başlar. Sesi kırık çıkar.

Faz 2 saldırıları Act 2 boss'tan alınmış — ama daha hızlı ve unpredictable.

| Saldırı | Özellik |
|---------|---------|
| **Fractured Mirror** | Oyuncunun son 3 skill'ini sırayla kopyalar |
| **Glitch Step** | 4-5 rapid teleport, son konumda saldırı |
| **Architect's Echo** | Geçmişte öldürülen tüm bossların bir saldırısı rastgele çıkar |
| **Identity Collapse** | Tüm arena 3s "inverted" olur — hasar kaynaklarının yerleri değişir |

### Faz 3 — "Boşluk" (HP: 45% → 20%)

**Geçiş sahnesi (3 sn):** Müzik tamamen kesilir. Arena yavaşça karanlığa gömülür — sadece Architect ve oyuncu kalır. Sessizlikte bir ses: *"Senden önce binlercesi buraya geldi."*

Act 3 boss'un arena mekaniği burada da var — ama bu sefer Architect'in kendisi de bir platform üzerinde, bazı saldırılar sadece doğru platformdan yapılabilir.

| Saldırı | Mekanik |
|---------|---------|
| **Void Architecture** | Architect yeni platformlar yaratır — bazıları tuzak |
| **Gravity Inversion** | 5s: yukarı yürünüyor, saldırı yönleri ters |
| **The Blueprint** | Zemine pattern çizer, 3s içinde hasar alanı aktive |
| **Silence** | Tüm skill'leri 4s bloklar — sadece hareket |

**"Silence" mekaniği önemi:** Oyuncu 4 saniye hiçbir skill kullanamaz. İlk kez yaşayan oyuncu panikler. İkinci kez sakin kalır. Bu faz "büyümüş oyuncu" testi.

### Faz 4 — "Gerçek Form" (HP: 20% → 0%)

**Geçiş sahnesi (3.5 sn — en uzun geçiş):**
> Arena patlar. Tüm karanlık dağılır. Architect'in gerçek hali görünür — dev değil, küçük. Bir insan. Müzik: yeni, beklenmedik, melodik. "Tamam. Gerçekten görmek istiyorsan..." Birkaç saniye sessizlik. Sonra: **tema müziği başlar.**

Faz 4'te tüm önceki mekanikler **aynı anda** devrede:
- Arena'nın köşeleri hasar veriyor (Act 3)
- Bazı saldırılar skill'leri kopyalıyor (Act 2)
- Hız Act 1'in 2 katı

Ama oyuncu da en güçlü halinde. Full build, Cross-class Ultimate kullanılabilir.

| Saldırı | Mekanik |
|---------|---------|
| **Fracture Everything** | Tüm önceki boss saldırılarından rastgele 3 tanesi arka arkaya |
| **The Original Sin** | Architect'in ilk saldırısı — basit, yavaş, ama gülünç derecede güçlü |
| **Build Breaker** | Oyuncunun en çok kullandığı skill'i 8s devre dışı bırakır |
| **Final Architecture** | Büyük AoE + kısa süreliğine yenilemez — oyuncunun *hareket ederek* hayatta kalması lazım |

**Ölüm sahnesi:** Architect yavaşça çöker. "...iyi." Ekran siyaha gider. Credits değil — önce sessizlik, sonra hub sahnesine dönüş. Her şey aynı görünüyor ama bir şey farklı.

---

## Özet Tablo

| Boss | Faz Sayısı | Geçiş HP | Süre Hedefi | Epic Faktör |
|------|-----------|----------|-------------|-------------|
| Act 1 — Penitent Sovereign | **3** [S42] | **%66 / %33** | **3-4 dk** | Kırılma anının öngörüsü |
| Act 2 — Echo Twin | 2 | %40 | 3-4 dk | Dual-class yansıması |
| Act 3 — Fracture Sovereign | 3 | %60 / %30 | 5-6 dk | Çevre + platform mekaniği |
| Final — The Architect | 4 | %75 / %45 / %20 | 8-10 dk | Her şeyin kapanışı |

---

## Implementation Notu

Öncelik sırası:
1. Act 1 Boss — en kritik (kırılma anı)
2. Final Boss Faz 1 + 2 (erken prototip için yeterli)
3. Act 3 Boss — platform mekaniği ayrı sistem gerektirir
4. Tüm varyantlar (faz sistemleri tamamlandıktan sonra)

Faz geçiş sahneleri: `BossPhaseTransition.cs` — coroutine + müzik değişimi + brief slow-mo.
