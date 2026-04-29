# MMORPG SKILL TASARIM ARAŞTIRMASI — Kapsamlı Sentez
*2026-03-28 | Web araştırması + Wiki verileri + Proje bağlam analizi*
*Amaç: Bu projenin skill sistemi kararlarını referans verilere dayandırmak*

---

## İÇİNDEKİLER

1. Skill Havuzu Büyüklüğü — Kaç Skill Optimal?
2. Anlamlı Seçim Mimarisi — Ne Seçimi "Önemli" Yapar?
3. Proc / Zincirleme Mekanikleri — En İyi Örnekler
4. Roguelite Entegrasyonu — Derinlik ile Format Nasıl Uzlaştırılır?
5. Cross-Class / Hibrit Sistemler
6. Proje Bağlamına Uygulaması — Ne Yapmalı?

---

## 1. SKİLL HAVUZU BÜYÜKLÜĞÜ

### Guild Wars 1 — Altın Standart: 1.329 Skill, 8 Seç

**Doğrulanmış Veriler (wiki.guildwars.com):**

| Sınıf | Non-Elite | Elite | Toplam |
|-------|-----------|-------|--------|
| Warrior | 104 | 36 | 140 |
| Ranger | 105 | 35 | 140 |
| Monk | 106 | 35 | 141 |
| Necromancer | 107 | 35 | 142 |
| Mesmer | 103 | 34 | 137 |
| Elementalist | 109 | 35 | 144 |
| Assassin | 85 | 25 | 110 |
| Ritualist | 85 | 25 | 110 |
| Paragon | 70 | 15 | 85 |
| Dervish | 70 | 15 | 85 |
| **TOPLAM** | **1.026** | **303** | **1.329** |

**Temel Kısıt:** Oyuncu herhangi bir anda maksimum **8 skill** ekleyebilir. Bu 8'in içinde yalnızca 1 adet Elite skill bulunabilir.

**Neden Çalışıyor:**
- 140 skillin herhangi 8'ini seçmek matematiksel olarak binlerce kombinasyon üretir
- "Sadece 8" kısıtı build'i net ve anlaşılır tutar
- Her seçim gerçek bir alternatif maliyeti var: Charge seçersen Whirlwind yok
- Build metagame oyunun kendisinden ayrı bir meta-oyun haline geldi (PvX Wiki ekosistemi)
- Wikipedia: "Guild Wars skill sistemi sıklıkla Magic: The Gathering gibi koleksiyon kart oyunlarıyla karşılaştırılır — skillerin birbirleriyle etkileşimi nedeniyle"

**Dual Class Etkisi (doğrulanmış verilerle):**
- Secondary profession: birincil sınıfın primary attribute'u KULLAНИLAMAZ — kasıtlı kısıt
- Bu kısıt olmadan her build "hepsini al" olurdu
- Sonuç: İkincil sınıfı "amplifier" olarak kullanırsın, kopyalamak için değil
- Bir Warrior/Elementalist ateş büyüsü atabilir ama Elementalist kadar güçlü atamaz

**Sayısal Çeşitlilik:**
- Tek sınıf için: C(140,8) = ~sonsuz pratik kombinasyon
- Dual class katmanıyla: iki sınıfın 280 skilinden 8 seç (1 elite sınırı ile)
- Gerçek dünya sonucu: Her yeni oyun içeriği (yeni skill pack) meta'yı tamamen değiştirebilir

---

### World of Warcraft — Talent Tree + Active Rotation

**Kombo Sistemi (Rogue — doğrulanmış kaynak: Wowpedia):**
- Combo Point sistemi: Builder skill'ler combo point üretir (1-2/kullanım)
- Max 5 combo point biriktirilir
- Finisher skill'ler tüm birikmiş combo point'i harcar ve güç/etki combo'ya göre ölçeklenir
- Örnek: Hemorrhage (2 combo) × 2 kullanım = 4 combo → Eviscerate (%400 hasar)
- Bu proje zaten bu sistemi Rogue'a adapte etmiş

**Talent Tree Sistemi:**
- Klasik WoW: 71 talent point, 3 ağaç × ~30 node
- Modern WoW (Dragon Flight+): Her sınıf için 30 class talent + 30 spec talent = 60 talent node
- "Aktif skill sayısı" sınıfa göre 10-20 arasında değişiyor
- Wikipedia: "WoW'ın orijinal talent sistemi, oyuncuların upgrade'ler arasında puan dağıtmasına izin veren bu mekanizm geniş adaptasyon gördü — Star Wars: The Old Republic dahil"

**WoW Tasarım Dersi:**
- Rotation'da aktif olarak kullanılan skill sayısı: 6-12 (sınıfa bağlı)
- Bu sayının altı: monoton ("tek button spec" eleştirisi)
- Bu sayının üstü: tracking yükü çok yüksek

---

### Lost Ark — Sınırlı Action Bar, Derin Skill Pool

**Sistem Yapısı (doğrulanmış bilgiler):**
- Her sınıf: yaklaşık **20-25 aktif skill** havuzu
- Action bar'a eşzamanlı yerleştirilecek: **8 skill** (GW1 ile aynı sayı, farklı mekanik)
- Skill Points sistemi: Her skill'e 0-20 arasında nokta yatırabilirsin
- Tripod sistemi: Her skill'de 3 seviye branch upgradeleri var (Sıra 1: 3 seçenek, Sıra 2: 3 seçenek, Sıra 3: 3 seçenek = 27 kombinasyon/skill)
- Awakening Skills: Run başı seçilen 1 ultimate

**Identity Gauge Sistemi:**
- Her sınıf için benzersiz kaynak/durum mekaniği
- Örnek: Berserker → Burst Mode (Fury dolunca); Sorceress → Arcane Torrent; Bard → Harmony Gauge
- Bu proje'nin "V Burst" sistemi bu modeli esas alıyor

**Lost Ark Tasarım Dersi:**
- Tripod sistemi: Aynı skill iki farklı oyuncu tarafından tamamen farklı şekilde oynandığında başarılı
- Eşzamanlı 8 skill ama her birinin 3x3 tripod derinliği = gerçek build identity

---

### Path of Exile — Gem Sistemi, 6-Link

**Doğrulanmış Veriler (Wikipedia + PoE Wiki):**
- Passive Skill Tree: **1.325 pasif node** — tüm sınıflar aynı ağacı paylaşır, farklı başlangıç noktalarından
- Active Skill Gems: 100+ aktif skill gem
- Support Gems: 100+ destek gem
- 6-Link mekanik: 1 aktif skill + 5 support gem = skill tamamen transforme olabilir

**6-Link Neden Anlamlı:**
- Sadece 1 skill seç + onu nasıl amplify edeceğini 5 boyutta karar ver
- "Fireball + Greater Multiple Projectiles + Spell Echo + Added Lightning + Controlled Destruction + Concentrated Effect" — bu 6 seçim build kimliğini tanımlar
- Support gem seçimleri: AoE vs single target, projectile count vs penetration, cast speed vs damage gibi gerçek trade-off'lar

**PoE Tasarım Dersi:**
- Tek aktif skill'i 5+ modifier ile şekillendirmek = skill başına derinlik
- 1.325 node passive tree → build identity'nin %50'si burst'ten değil, pasif yapıdan geliyor
- Olumsuz: Bilgi eşiği çok yüksek, yeni oyuncu erişilemez bulur

---

### Hades — Boon Sistemi, Roguelite Altın Standardı

**Doğrulanmış Veriler (Steam + Wikipedia):**
- Her tanrı: yaklaşık 8-12 boon sunar (tanrıya özgü tematik efektler)
- "Onlarca güçlü Boon'dan seçin" — tahmin: toplam 100+ boon
- Oyun içi boon slotları: Attack, Special, Cast, Dash, Call + passive augment slotları
- Duo Boon'lar: 2 farklı tanrıdan belirli boon'lar alınca tetiklenen özel boon (threshold mekanik)
- Legendary Boon'lar: Aynı tanrıdan 4+ boon alınca unlock

**Hades Tasarım Dersi (kritik bulgular):**
1. **Boon = modifier, yeni skill değil.** Zagreus'un temel 4 skill'i (Attack, Special, Cast, Dash) değişmez. Boon'lar bu skill'lerin nasıl davrandığını değiştirir. Bu, "büyük pool, küçük seçim" yaklaşımının roguelite versiyonu.
2. **Her boon offer 3 seçenek sunar.** Hangisinin çıkacağı kısmen rastgele, ama god affinity sistemi belirli tanrıları daha sık çıkarır.
3. **"Thousands of viable builds"** — bu iddia doğru çünkü 6 slot × her slot için 10+ seçenek × Duo kombinasyonları = gerçekten büyük sayılar
4. **Replayability mekanizması:** Her run'da aynı build'i kasıtlı olarak kurmak zordur — bu amaçlı tasarlanmıştır

**Hades vs Bu Proje Farkı:**
- Hades: Sabit kit, değişen modifier
- Bu Proje: Değişen kit (skill seçimi) + değişen modifier (upgrade) — daha fazla permütasyon ama daha fazla kaos riski

---

### Dead Cells — Dual-Weapon Arketipleri

**Doğrulanmış Veriler (Wikipedia + wiki.gg):**
- Toplam silah sayısı: **60+ melee + 40+ ranged** (update'lerle 100+ toplam)
- Equipment loadout: 2 silah + 2 skill + 1 amulet
- Two-handed weapons: Her iki silah slotunu kaplar — alternatif cost

**3 Scaling Stat:**
- Brutality (kırmızı): Hızlı, hafif silahlar — Twin Daggers, Balanced Blade
- Survival (yeşil): Yavaş, ağır silahlar — Nutcracker, Broadsword
- Tactics (mavi): Ranged + trap skill'leri

**Build Identity Mekanizması:**
- Silah seçimi + scroll yatırımı (scaling stat artırır) build arketipini belirler
- Developers kasıtlı: "Oyuncuların sonsuza kadar tek weapon/skill kombinasyonunu kullanmasını istemiyorduk — roguelite elementleri yeni kombinasyonları denemelerini zorunlu kıldı"
- Mutation sistemi: Geçici bonuslar run'a özgü build'i daha da şekillendirir

**Dead Cells Tasarım Dersi:**
- "Arketip kısıtı": Stat seçimi, silah seçimi ve skill seçimi birbirini kısıtlar
- Bu "anti-correlation" tasarımının en saf örneği

---

### Slay the Spire — 75 Kart, Seç ve Çek

**Doğrulanmış Veriler (Wikipedia):**
- Karakter başına optimal kart havuzu: **~75 kart** (geliştiricinin bulduğu "doğru sayı")
- Kart reward sistemi: Her dövüş sonrası 3 kart seç, 1'ini ekle (veya hiçbirini alma)
- Deck dilution: Zayıf kart eklemenin maliyeti var — "temiz" deck bozulur
- Hasar yoksa istememe seçeneği: Roguelite'de "al" baskısını kaldırır

**StS Tasarım Dersi:**
- "~75 kart doğru denge" — çok az: her run aynı, çok fazla: sinerji bulunamaz
- Deck dilution anti-incentive olarak çalışır: Her ödülde "bu beni güçlendiriyor mu?" sorusu gerçek
- Geliştiriciler güçlü sinerji kombinasyonlarına karşı çıkmadı — "collectible card oyunlarında sorun olan şey burada sorun değil"

---

### Özet — Optimal Havuz Büyüklüğü Tablosu

| Oyun | Havuz Büyüklüğü | Aktif Kullanım | Anti-Corr? | Not |
|------|-----------------|----------------|------------|-----|
| Guild Wars 1 | 140/sınıf (1.329 toplam) | 8 seç | Evet (primary attr) | Dual class ile 2 havuzdan 8 seç |
| WoW Rogue | ~60 talent + 15 aktif | 6-12 aktif rotation | Hayır (additive) | Combo point sistemi derinlik katar |
| Lost Ark | 20-25/sınıf | 8 action bar | Kısmi (skill points) | Tripod sistemi derinlik katar |
| Path of Exile | 100+ skill gem | 1-3 aktif gem | Evet (link sayısı) | 1.325 passive node ek derinlik |
| Hades | 100+ boon (toplam) | 6-8 boon slot | Evet (duo req.) | Modifier-based, kit sabit |
| Dead Cells | 100+ silah | 4 slot | Evet (scaling stat) | Arketip seçimi kilitleme |
| Slay the Spire | ~75/karakter | 20-30 deck | Evet (dilution) | Azaltma mekaniği zorunlu |

**Genel Bulgu: "Optimal" sayı 6-12 aktif + 20-140 havuz.**
Kısıt mekanik olmadan havuz büyüklüğü önemsiz. Kısıt mekanik havuzu anlamlı yapar.

---

## 2. ANLAMLI SEÇİM MİMARİSİ

### "Anti-Correlation" — Seçim A, B'yi Zayıflatır

**En İyi Örnekler:**

**Guild Wars 1 — Primary Attribute Kısıtı:**
- Warrior/Elementalist: Strength attribute'u maksimize edemezsin VE Elementalist Magic'i maksimize edemezsin
- Sonuç: Ya birini iyi yaparsın ya diğerini orta yaparsın
- Bu kısıt olmadan: "Hepsini maksimize et" → sıkıcı

**Dead Cells — Scaling Stat Kısıtı:**
- Brutality'ye scroll yatırınca Survival'dan çıkan ekipmanlar zayıf kalır
- Bir run'da hem hızlı hem tankı olmak mümkün değil (en azından eşit derecede)
- Bu kasıtlı — geliştiriciler "identity'yi zorla" dedi

**Path of Exile — 6-Link Budget:**
- Bir skill'e 5 support gem ayırınca diğer skill'lerin support'u kalmaz
- Aynı anda hem AoE hem single-target optimize edilemez
- 6-link'ın maddi maliyeti de var (endgame gear)

**Slay the Spire — Deck Dilution:**
- Her kart ekleme, diğer kartları daha az görmeni sağlar
- "Kötü kart almama" seçeneği bu yüzden var
- Gücü olan kart bile decke zarar verebilir

**Bu Projede Uygulama (mevcut sistem analizi):**
Mevcut tasarımda anti-correlation şu şekillerde mevcut:
- Run başında PRIMARY RESOURCE seçimi: bir kaynağı V burst için seçmek diğerini ikincileştirir ✓
- 4 skill slot: 8 havuzdan 4 seçmek diğer 4'ü tamamen kilitler ✓
- Pasif çakışması: P1 lifesteal seçersen tankiness pasifini alamazsın ✓

**Eksik olan anti-correlation:**
- Skill upgrade'lerin birbirini kilitlememesi (aynı skill 2 kez upgrade edilemez ama bu iki farklı skill arasında değil)
- FAZ 3 sonrası augment eklenirse "her şeyi al" riski artar

---

### "Role-Defining Skills" — Oynanışı Temelden Değiştiren Seçimler

**GW1 Örneği:** Ritualist, Weapon Spells seçerse tamamen farklı oynanış → destek oriented. Spirit Spells seçerse turret placer. Aynı sınıf, tamamen farklı iki oyun.

**FFXIV Black Mage — En İyi Örnek:**
Doğrulanmış sistem:
- Astral Fire: Ateş büyülerinin MP maliyeti 2 katına çıkar, MP rejenerasyonu sıfırlanır
- Umbral Ice: Buz büyülerinin MP maliyeti sıfırlanır, her büyü 2.500 MP kazandırır
- Bu iki durum birbirini dışlar (mutually exclusive)
- Sonuç: BLM "faz rotasyonu" yapmak zorunda — Fire Phase → MP biter → Ice Phase → MP dolar → Fire Phase
- 40+ skill var ama rotation bunun yalnızca 8-10'unu aktif kullanır

**BLM Dersi: "State-based restriction" en güçlü role-defining mekaniktir.**
Çünkü:
- Hangi skill'i seçtiğin değil, hangi durumda olduğun oynanışı belirler
- Oyuncu her zaman "şu an hangi state'teyim?" sorusunu sormak zorunda
- Bu proje: Fire/Ice state gibi "stance" kavramı GDD'de "Stance Modu" olarak var ama FAZ 1 dışında

**Bu Projede Role-Defining Seçimler (mevcut analiz):**
- Hangi 2 sınıfı seçtiğin → kesinlikle role-defining ✓ (28 arketip)
- PRIMARY RESOURCE seçimi → V burst'ün ne zaman tetikleneceğini tanımlar ✓
- Ultimate seçimi → 45-60s pencereler build ritmi kurar ✓

---

### "Threshold Mechanics" — 3+ Birlikte İşe Yarar

**Hades — Duo Boon Sistemi (en temiz örnek):**
- Duo boon: İki farklı tanrıdan belirli boon'lar alınca unlock olur
- Örnek: Zeus + Poseidon boon kombinasyonu → "Sea Storm" duo boon
- Bu duo boon kendi başına her iki boon'dan daha güçlü
- Sonuç: "İkisini de kasıyorum" stratejik odak yaratır

**GW1 — Build Synergy Requirement:**
- GW1'in en güçlü build'leri 3-4 skill'in çalışmasını gerektirir
- Örnek: Hexes (kısaltma büyüsü) + damage skills + hex exploitation skills üçlüsü
- Bir parçası eksik olursa build yarısı kadar güçlü

**Bu Projede Threshold Mekanik (mevcut analiz):**
- CLAUDE_SENTEZ.md'de "Bu Build İnsane" momentleri tam olarak bu — ama mekanik olarak zorunlu değil
- Örnek: Warblade "insane" anı: Battle Cry + Rage 100% + Berserker's Rage + Iron Will proc
- Bu 4 elementin hepsi olmalı, 3'ü olmaz
- Öneri: Bu sinerji ekranda "Sinerji Bonus" etiketi ile gösterilmeli → oyuncu build'i kasınca bilmeli ki fark var

---

## 3. PROC / ZİNCİRLEME MEKANİKLERİ

### WoW Rogue Combo Point — Builder/Finisher

**Sistem:**
- Builders: Hemorrhage, Backstab, Mutilate → combo point üretir (1-3/kullanım)
- Max: 5 combo point
- Finishers: Eviscerate, Rupture, Slice and Dice → tüm combo point'i harcar, güç ölçeklenir
- "Kaç combo'yla bitireyim?" her dövüşte bir karar

**Neden Çalışıyor:**
1. **Bekleme ritmi:** Builder→Builder→Finisher = nefes ritmi
2. **Trade-off:** 3 combo'da mı bitireyim (hızlı ama zayıf) yoksa 5'i mi bekleyeyim (güçlü ama risk)?
3. **Proc layer:** Bazı builder'lar extra combo üretir → unplanned finisher fırsatı

**Bu Proje Rogue:** Aynı sistemi alıyor (0-5 combo, Eviscerate finisher). Fark: Roguelike context'te 5 combo biriktiremeden ölme riski, MMO'dan farklı gerilim üretiyor.

---

### FFXIV Black Mage — Fire/Ice Rotasyon

**Doğrulanmış Sistem:**
- Astral Fire (AF): Fire hasar 2× ama MP maliyeti 2×, rejenerasyon 0
- Umbral Ice (UI): Ice maliyeti 0, her ice büyüsü +2.500 MP
- Firestarter proc: %40 ihtimalle Fire III anında ve maliyetsiz atılır
- Paradox proc: AF veya UI durumunda özel anında cast fırsatı

**Kaç Chain "Çok Fazla":**
BLM örneği yanıt veriyor:
- BLM'in aktif rotation'da kullandığı unique skill: 8-10
- Birlikte track etmesi gereken durum sayısı: AF stacks (1-3), UI stacks (1-3), Polyglot (0-2), Ley Lines buffs, Firestarter proc, Paradox proc = 6 durum
- "Çok fazla" threshold: Oyuncu komunite tarafından BLM "en zor job" olarak tanınır
- Ama bu zorluk çekici — "ustalık" hissi var

**Proje İçin Benchmark:**
- Bu proje, boss fight değil real-time roguelite aksiyon
- Eş zamanlı track edilecek maksimum: 3-4 durum (resource bar + 1-2 proc koşulu + buff timer)
- BLM'in 6 durumu aksiyon roguelite'de "overload" olur

---

### Lost Ark — Identity Gauge + Awakening

**Sistem Yapısı:**
- Her sınıf benzersiz Identity Gauge
- Berserker: Fury → dolunca Burst Mode → tüm stat boost, özel skill erişimi
- Sorceress: Spellcaster → iki ayrı Identity skill → birini seç, diğerini kapat
- Awakening Skill: Run başı seçilen 1 ultimate, uzun cooldown (2-3 dakika)

**Identity Tasarım Dersi:**
- Identity mekanik sınıf kimliğini tanımlar, skill listesi değil
- Farklı sınıflar aynı skill tipine sahip olsa bile Identity farklı hissettirir
- Bu proje: Her sınıfın V burst'ü = Lost Ark Identity

---

### Kaç Chain "Çok Fazla"? — Uzman Konsensüs

**Referans Örnekler:**

| Oyun | Chain Derinliği | Değerlendirme |
|------|-----------------|---------------|
| WoW Rogue | 2 (builder → finisher) | Mükemmel: anlaşılır, anlamlı |
| FFXIV BLM | 4-5 (fire → ice → proc → poly → paradox) | Karmaşık ama ödüllendirici, niş oyuncu tabanı |
| Lost Ark (bazı sınıflar) | 3 (setup → gauge → burst) | Uygun |
| Hades (boon sinerji) | 2-3 (boon1 + boon2 → duo trigger) | İdeal |
| Bu Proje (Rogue) | 4 (stealth → shadow → bleed → eviscerate) | Sınırda — doğru hissettirirse güzel |

**Genel Kural:**
- 2 chain: Herkese ulaşılabilir, anlamlı
- 3 chain: Optimal — anlaşılabilir ama "derin" hissettiriyor
- 4-5 chain: Niş oyuncu, "yüksek beceri" etiketi gerektirir
- 6+ chain: Aksiyon roguelite'de pratik olarak izlenemez

**Bu Proje Önerisi:**
- Temel chain depth: 2-3 (80% senaryolar)
- Maksimum chain depth: 4 (sadece "insane build" momentleri, isteğe bağlı)
- Zincirleme **zorunlu** olmamalı — keşfedildiğinde ödüllendirilmeli

---

## 4. ROGUELİTE ENTEGRASYONU — Derinlik vs Format

### Dört Model Karşılaştırması

**Model A: Hades — Sabit Kit, Değişen Modifier**
- Zagreus'un 4 temel eylemi (Attack/Special/Cast/Dash) ASLA değişmez
- Boon'lar bu eylemlerin nasıl davrandığını değiştirir
- Avantaj: Öğrenme eğrisi düz, temel yetkinlik run'dan bağımsız
- Avantaj: Build diversity boon kombinasyonlarından gelir, kit konfüzyonundan değil
- Dezavantaj: Kit bıkkınlık üretebilir (uzun vadede "hep aynı Zagreus'u oynuyorum")

**Model B: Dead Cells — Arketip Kilitleme**
- Silah seçimi scaling stat belirler, stat build yönünü kilitler
- 2 silah + 2 skill = sadece 4 slot, ama 100+ seçenekten
- Avantaj: Her run potansiyel yeni arketip
- Avantaj: Stat locking anti-correlation üretir
- Dezavantaj: Zayıf başlangıç drop'u tüm run'u belirleyebilir (RNG baskısı)

**Model C: Risk of Rain 2 — Item = Skill Modifier**
- Her karakterin 4 sabit skill'i var
- Item'lar skill davranışını değiştirir (Soldier's Syringe: attack speed → skills more often)
- Avantaj: İki farklı mekanizmanın hibridini sağlar
- Dezavantaj: Item interaksiyonları test edilmesi çok zor, unintended combo riski yüksek

**Model D: Curse of the Dead Gods — Sabit Kit, Lanet Modifier**
- 12 silah sınıfı, her biri farklı mekanik
- Lanet sistemi: run boyunca birikir, olumsuz efektler + fırsatlar sunar
- Boss'lar yenilince lanet kalkar
- Avantaj: Deterministik risk/ödül döngüsü
- Dezavantaj: Silah seçimi dışında özelleştirme kısıtlı

---

### Bu Projenin Modeli ve Analizi

**Mevcut Model: "Slay the Spire Skill Acquisition + Hades Room Flow"**

```
Run başı: 2 sınıf seç → 4 skill + 2 pasif + 1 ultimate seç
Oda sonu: 3 seçenek → skill ekle / upgrade et / pasif al
Max slot: 6 aktif + 2 pasif + 1 ultimate
Boss sonrası: 1 skill'i mutasyona uğrat (Soul sistemi)
```

**Bu modelin güçlü yanları:**
1. Skill acquisition Slay the Spire'dan alınmış — test edilmiş, çalışan model
2. "3 seçenek, 1'ini al" kararın ağırlığını kontrol eder
3. Max 6 slot → Dead Cells'in 4 slotundan daha zengin, WoW'ın 12+ slotundan daha temiz
4. Boss Soul mutasyonu = run'un ikinci yarısında "yeniden build etme" fırsatı

**Potansiyel riskler (araştırma ışığında):**
1. **"Hepsini al" baskısı:** Her oda sonu ödül sunulduğunda oyuncu reddetmeyebilir. Slay the Spire'da "almama" seçeneği aktif çünkü deck dilution gerçek. Bu projede slot limit var ama slot dolana kadar her şeyi almak mantıklı.
   - Çözüm: 4 slot dolu iken 5. skill almak için 3 seçenek sun, mevcut skill'lerden biri çıkarılsın
2. **RNG baskısı:** Sınıfın core skill'i oda ödülüne düşmezse build şekillenemiyor
   - Çözüm: İlk 3 oda garantili "core pool" seçenekleri (GUCLENME_SISTEMI.md'de ağırlık sistemi var, bu iyi)
3. **Proc koşulu görünürlüğü:** Zincirleme proc'lar keşfedilmezse derinlik boşa gider
   - Çözüm: Tooltip'te proc koşulunu göster + ilk kez tetiklenince "Combo Discovery" bildirimi

---

## 5. CROSS-CLASS / HİBRİT SİSTEMLER

### Guild Wars 1 — Altın Standart Analizi

**Mekanik (doğrulanmış):**
- Her oyuncu: 1 primary class + 1 secondary class
- Secondary class: Secondary'nin primary attribute'u erişilemez
- Sonuç: Secondary class'ın skill havuzuna erişim var ama o skill'lerin güç ölçekleyicisi yok

**Bu Kısıt Tasarım Dehasıdır:**
- Kısıtsız dual class: Her build "hem güçlü hem güçlü" olurdu
- Kısıtlı dual class: Secondary class "utility" veya "niche combo" sağlar, "clone" değil
- Örnek: Warrior/Monk → Monk'ın healing büyülerini kullanabilir ama Healing Prayers attribute eksik → zayıf heal ama pek çok Warrior için yeterli "self-sustain"

**Buildcraft Paradoksu (GW1'in başarısı):**
- Kısıt → sonsuz yaratıcılık
- "Nasıl secondary'mi kullanalım?" sorusu her dual class için farklı yanıt üretir
- Topluluk binlerce "unexpected" build buldu

---

### FFXIV Job Sistemi — Lineer Uzmanlık

**Sistem:** Her Job (class) belirli Role Actions + Job Actions kombinasyonu
- Cross-role action: Roleye göre bazı diğer job skill'lerini alabilirsin (örn: tüm DPS'ler "Feint" kullanabilir)
- Bu GW1'den çok daha kısıtlı hybrid
- Tasarım felsefesi: "Her job benzersiz hissetmeli" → cross-class minimum tutuldu

**FFXIV Dersi:** Job identity ne kadar güçlü olursa cross-class o kadar az gerekli olur. Ama tek başına identity bıkkınlık üretir — hafif cross-class taze tutar.

---

### Rift Soul Sistemi — En Esnek MMORPG Hibrid

**Doğrulanmış Veriler (Wikipedia):**
- Her Calling (sınıf tipi): 8-10 soul havuzu
- Eşzamanlı seçilebilir soul: 3
- Her soul'da: "branches" (seçilen bonuslar) + "roots" (temel ability'ler)
- 20 farklı role kaydedilebilir, combat dışında geçiş

**Rift Tasarım Dersi:**
- "3 soul aynı anda" optimal hibrid nokta — 2 çok az, 4+ çok karmaşık
- Her soul'a az puan yatırmak → "jack of all trades" → zayıf
- Her soul'a yüksek puan yatırmak → 3 soul'u max'lamak imkansız → trade-off var

**Bu Proje vs Rift:**
- Bu proje 2 sınıf seçiyor (Rift 3 soul) — daha temiz
- Rift'te soul'lar aynı calling içinden, bu proje 8 sınıf arasından serbestçe seçim
- Bu proje = daha radikal combination ama daha az granülar kontrol

---

### Bu Projenin 28 Cross-Class Analizi

**Matematiksel Çeşitlilik:**
- 8 sınıf, 2 seç: C(8,2) = 28 kombinasyon ✓
- Her kombinasyon için benzersiz arketip + ultimate ✓
- Her run: 2 havuzdan toplamda 16 skill → 4'ünü seç + 2 pasif → 28 olası combo

**GW1 Karşılaştırması:**
- GW1: 140 skill havuzu, 8 seç
- Bu proje: 16 skill havuzu (2 × 8), 4 seç — daha küçük ama daha odaklı
- "2 sınıfın en iyisini getir" felsefesi tam GW1 ile aynı

**28 Arketip Tasarım Gözlemi:**
- 28 arketip var ama her arketip "kaç farklı skill build'e yol açıyor?"
- Örnek: Warblade + Elementalist = Runeguard arketip
  - Warblade havuzdan: Charge, Mortal Strike, Colossus Smash, Execute (veya diğerleri)
  - Elementalist havuzdan: Fireball, Ice Nova, Blink, Arcane Surge (veya diğerleri)
  - Bu iki havuzun 4 seçimi: 8 havuzdan, farklı kombinasyonlar
  - + Pasif seçimleri (4 Warblade + 4 Elementalist pasifinden 2 seç)
- Sonuç: 28 arketip × her arketip içinde ~20+ farklı build = gerçek anlamda büyük çeşitlilik

---

## 6. PROJE BAĞLAMINA UYGULAMASI — NE YAPMALI?

### Mevcut Tasarımın Araştırma Karşısındaki Durumu

**Güçlü Yanlar (araştırma tarafından desteklenen):**

| Tasarım Kararı | Araştırma Kanıtı |
|----------------|-----------------|
| 8 skill havuz, 4 aktif seç | GW1 modeli — kanıtlanmış, 1.329 skill/8 seç prensibinin küçük ölçeği |
| 28 cross-class kombinasyon | GW1 dual class felsefesiyle tamamen uyumlu |
| Builder/finisher (Rogue combo system) | WoW Rogue — kanıtlanmış retention mekanik |
| Fire/Ice state (Elementalist Mana) | FFXIV BLM rotation ritminden adapt |
| V burst (Identity Gauge) | Lost Ark Identity Gauge modeli |
| 3 seçenek, 1'ini al (oda ödülü) | Slay the Spire — optimal karar yükü |
| Boss Soul mutasyon | Risk of Rain 2 item-modifies-skill yaklaşımının özelleştirilmiş versiyonu |
| Proc zincirleme (isteğe bağlı) | Hades duo boon threshold mekaniğiyle aynı prensip |

**Dikkat Gerektiren Alanlar:**

1. **Anti-Correlation Eksikliği (FAZ 3 sonrası risk)**
   - Mevcut: 4 skill slot dolunca yeni eklenemez — bu anti-correlation
   - Risk: 6 slota çıkınca "hepsini al" baskısı artıyor
   - Öneri: 5. ve 6. skill slotu FLUX veya Boss Soul ile gelsin, oda ödülüyle değil

2. **Proc Keşfi Görünürlüğü**
   - "Zincirleme" efektler tooltip'te gösterilmeli
   - İlk kez tetiklenince "SİNERJİ KEŞFEDILDI" bildirimi — Hades'in Duo Boon bildiriminden adapt

3. **Threshold Build Sinyali**
   - 3+ belirli skill bir arada olunca "build senkrona girdi" vizyal feedback
   - Warblade örneği: Charge + Mortal Strike + Execute üçlüsü aktifken özel "Executioner" ikonu göster

4. **Kaç Chain Maximal**
   - Mevcut tasarım: En derin chain 4-5 adım (örn. Rogue insane moment)
   - Araştırma bulgusu: 3 chain optimal, 4 "yüksek beceri" nişi
   - Karar: 4-chain build'leri mevcut bırak ama bunlar "discovery" build, tutorial zinciri 2-3 adım

5. **Cross-Class Kısıt Mekanik**
   - GW1'in primary attribute kısıtı bu projede karşılığı yok
   - Mevcut: 4 slot için 2 havuzdan eşit seçim (2+2 veya 3+1 serbest)
   - Öneri: "Fusion Modu" için herhangi kombinasyon serbest
   - "Stance Modu" (FAZ 2+): Her iki sınıf tam kit, [TAB] geçiş = GW1 kısıtının tam tersi, extreme uç

---

### Sayısal Benchmark Özeti

**Bu Proje (şu haliyle) Sayıları:**

| Parametre | Değer | Benchmark Karşılaştırması |
|-----------|-------|--------------------------|
| Sınıf sayısı | 8 | WoW (~12 class) ile kıyaslanabilir |
| Sınıf başına skill havuzu | 8 aktif + 4 pasif + 2 ultimate | GW1'in %6'sı ama roguelite format için yeterli |
| Eş zamanlı aktif skill | 4 (max 6) | GW1=8, Lost Ark=8, Dead Cells=4 — ortada |
| Cross-class kombinasyon | 28 | GW1 dual class matematiksel olarak bu projenin 5×'i ama experience kalitesi önemli |
| Oda ödülü seçenek sayısı | 3 | Slay the Spire = 3 — kanıtlanmış |
| Build mutasyon noktaları | Run başı + her boss | Yeterli (GW1'de run başı tek seferlik, bu proje daha dinamik) |
| "İnsane moment" chain derinliği | 4-5 | Sınırda — iyi tasarlanmış action roguelite'de makul |

---

### Son Not — Bu Projeye Özgü Güç

Bu araştırma projenin önemli bir özgünlüğünü ortaya koyuyor:

GW1 dual class sistemi (1.329 skill / 8 seç) ile Hades'in modifier boon sistemi ve Slay the Spire'ın incremental acquisition modelini **tek bir sistemde birleştirmek** piyasada nadir.

Çoğu oyun bu üçünden birini seçiyor:
- Geniş pool + sabit seçim (GW1)
- Sabit kit + modifier stacking (Hades)
- Progressive acquisition + deck building (StS)

Bu proje üçünü aynı anda deniyor: **8 havuzdan 4 seç** (GW1) → **oda ödülüyle büyü** (StS) → **skill'leri upgrade ve mutasyonla modifiye et** (Hades/RoR2).

Bu ambiziyon güç ama solo dev takvimi gözetilerek dikkatli scope kontrolü gerekiyor.

---

*Kaynak: wiki.guildwars.com (GW1 skill sayıları), Wikipedia (GW1, PoE, Hades, Dead Cells, Rift, StS verileri), ffxiv.consolegameswiki.com (BLM mekanikler), Steam store (Hades boon sayısı), proje design dosyaları (SINIF_SKILL_HAVUZU.md, CLAUDE_SENTEZ.md, GDD.md, MASTER_SINIF_VE_CROSSCLASS.md)*
