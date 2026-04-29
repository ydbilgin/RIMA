# Skill Revizyon Analizi
*2026-03-29 | ChatGPT Deep Research çıktısı vs mevcut SINIF_SKILL_SISTEMI_FINAL.md karşılaştırması*

---

## ÖZET KARAR TABLOSU

| Sınıf | Mevcut (kaldırılacak) | Öneri | Karar | Gerekçe |
|-------|----------------------|-------|-------|---------|
| Warblade | Hamstring | Gravity Cleave | ✅ KABUL | "Geçilmez" fantasy'ye tam uyuyor |
| Warblade | Whirlwind | Ironclad Momentum | ✅ KABUL* | Whirlwind Brawler'da da var — duplication sorun |
| Elementalist | Frostbolt | Glacial Spike | ⚠️ KOŞULLU | Fire/Frost State yönetimi korunmalı |
| Elementalist | Mana Shield | Arcane Surge | ✅ KABUL | Burst window hissi çok daha güçlü |
| Rogue | Sprint | Mirage Blade | ✅ KABUL | Mobilitenin CP üretimine dönüşmesi özgün |
| Rogue | Deadly Poison | Toxic Eruption | ✅ KABUL | Aktif SPENDER/FINISHER >> pasif DoT |
| Ranger | Serpent Sting | Barbed Net Shot | ✅ KABUL | GW1 Net Shot referansı, mesafe fantezisine uyuyor |
| Ranger | Spirit Wolf | ❌ Öneri gelmedi | 🔶 BEKLE | Ben tamamlarım |
| Brawler | Last Rites | Undying Tenacity | ✅ KABUL | %15 eşik yerine 4s direnç — çok daha kullanılabilir |
| Brawler | War Cry | ❌ Öneri gelmedi | 🔶 BEKLE | Ben tamamlarım |
| Paladin | Lay on Hands | Divine Intervention | ⚠️ KOŞULLU | 90s CD kaldırılmalı, HP miktarı revize |
| Paladin | Blessed Weapon | ❌ Öneri gelmedi | 🔶 BEKLE | Ben tamamlarım |
| Summoner | Raise Archer | Soul Siphon Totem | ✅ KABUL | PoE Totem mantığı — Charge üretimi özgün |
| Summoner | Sacrificial Pact | ❌ Öneri gelmedi | 🔶 BEKLE | Ben tamamlarım |
| Hexer | Enervate | Hex Contagion | ❌ RED | Pandemic ile çakışıyor (zaten stack kopyalıyor) |
| Hexer | Silence Hex | ❌ Öneri gelmedi | 🔶 BEKLE | Ben tamamlarım |

---

## DETAYLI ANALİZ

### ⚔️ WARBLADE

**Hamstring → Gravity Cleave ✅**
- Mevcut: %50 slow 8s + bleed DoT — "angarya" hissi, flat top-down'da slow etkisi sınırlı
- Yeni (Gravity Cleave): 4m çapında tüm düşmanları merkeze çeker + %140 hasar, 0.8s slow
- Neden daha iyi: Stun → topla → Whirlwind/Execute zinciri çok daha tatmin edici
- Tag: ⬡⚡ (CONTROL / CHAIN) — Core

**Whirlwind → Ironclad Momentum ✅ (Warblade versiyonu)**
- Mevcut Sorun: Whirlwind hem Warblade hem Brawler'da var. İki sınıf aynı skill paylaşıyor → build kimliği erozyon
- Mevcut: 2s spin AoE, Rage -25/sn
- Yeni (Ironclad Momentum): 6s boyunca alınan hasar %30 yok sayar + her 10 hasar = +10 Rage
- Neden daha iyi: "Hasar al ve güçlen" döngüsü Warblade'in "gittikçe tehlikeliyim" fantasy'sine uyuyor
- Tag: ↑✦ (BUILDER / AMPLIFIER) — Core
- NOT: Brawler'ın Whirlwind'i kalır — her iki sınıf için farklı spin mekaniği

**Mortal Strike — DOKUNMA**
- ChatGPT önermedi, doğru karar. Mortal Strike Warblade'in **imza skill'i** (README'de garanti teklif olarak belirtilmiş). Kaldırmak run başı kimliği bozar.

---

### 🔥 ELEMENTALİST

**Frostbolt → Glacial Spike ⚠️ KOŞULLU**
- Mevcut: Orta hasar + %30 slow + Fire State tüketir (Fireball DoT varsa Shatter)
- Yeni (Glacial Spike): 6m buz hattı + Frost State+2, Fireball DoT altında → Freeze 2s + %150 hasar
- Endişe: Frostbolt Fire State tüketiyordu — ritim döngüsünün kritik parçası. Glacial Spike Frost State +2 üretir ama tüketme mekanizması kaybolur.
- Çözüm: Glacial Spike efektine "Fire State 1 stack tüketir, karşılığında Frost State+2 kazanır" ekle
- Tag: ⬡↑ (CONTROL / BUILDER) — Core ✅ kabul, ama efekt revize gerekiyor

**Mana Shield → Arcane Surge ✅**
- Mevcut: 6s hasar Mana'ya gelir — pasif savunma, Elementalist ritmine katkı az
- Yeni (Arcane Surge): 8s Arcane Field — mana regen +%100, cast süresi -%50
- Neden çok daha iyi: FFXIV Black Mage Ley Lines referansı — "burst window" zaten oyunun DNA'sında. Mana kullanmak güçlendiriyor, sınırlayan değil.
- Tag: ✦▶ (AMPLIFIER / OPENER) — Advanced ✅

---

### 🗡️ ROGUE

**Sprint → Mirage Blade ✅**
- Mevcut: 3s hız +%100 — basit kaçış, roguelite'ta slot israfı
- Yeni (Mirage Blade): 3s içinde geçilen konumlara gölge bırakır, dokunan düşman %100 hasar + +1CP
- Neden çok daha iyi: Hareket = CP üretimi. "Assassin" build'ine tam uyuyor
- Tag: ⚡⚓ (CHAIN / ANCHOR) — Advanced ✅

**Deadly Poison → Toxic Eruption ✅**
- Mevcut: 10s pasif silah zehiri — roguelite temposunda (15-20s oda) sonuç göremiyorsun
- Yeni (Toxic Eruption): 5CP → tüm debuffları patlatır, zehir/kanama stack başına %150 hasar
- Neden çok daha iyi: SPENDER/FINISHER döngüsünü tamamlar, Hemorrhage+Rupture ile üçlü zincir
- Tag: ↓💥 (SPENDER / FINISHER) — Advanced ✅

---

### 🏹 RANGER

**Serpent Sting → Barbed Net Shot ✅**
- Mevcut: Zehir DoT 10s + armor debuff — mesafeyi korumaya yardımcı olmaz
- Yeni (Barbed Net Shot): Ağ fırlatır, 2s root + 4s kanama %40
- Neden daha iyi: "Sana ulaşamazsın" fantasy'si için aktif engelleme >> pasif DoT
- Tag: ⬡▶ (CONTROL / OPENER) — Core ✅

**Spirit Wolf için eksik öneri — BENİM TAMAMLAMAM:**
- Sorun: Advanced skill olarak kurt çağırmak Summoner ile kimlik karışıklığı yaratıyor
- Önerim → **Terrain Spike** (▶💥 OPENER/FINISHER — Advanced)
  - Efekt: Zemine bir tuzak yerleştir. Düşman yaklaştığında (≤3m) tüm kökler/slowlar patlar + %200 hasar. Focus +20.
  - Chain: Disengage sonrası → Terrain Spike anında aktive olur, Rapid Fire'ın ilk oku kritik
  - Neden uyuyor: Zemin kontrolü Ranger'ın core fantasy'sini pekiştirir, "güvenli bölge" inşa eder

---

### 👊 BRAWLER

**Last Rites → Undying Tenacity ✅**
- Mevcut: SADECE HP<%15 — çok dar eşik, mermi oyunu oyuncuyu saniyeler içinde öldürür
- Yeni (Undying Tenacity): 4s direnç modu, hasar %40 yok sayar, HP %20 altına düşemez
- Neden çok daha iyi: Hem hayatta kalma hem Fury üretimi — Death Wish ile zincir kurar
- Tag: ⚓↑ (ANCHOR / BUILDER) — Core ✅

**War Cry için eksik öneri — BENİM TAMAMLAMAM:**
- Sorun: Sadece Fury üretimi artırır — ama Fury zaten sadece hasar alarak dolduğu için "kasıtlı hasar almayı" ödüllendiren, daha aktif bir mekanik gerekiyor
- Önerim → **Bloodlust Echo** (✦⚡ AMPLIFIER/CHAIN — Advanced)
  - Efekt: 8s: her öldürme = +1 Fury stack (max 5). Her stack +%10 hasar. Fury %100'e ulaşırsa → sonraki Brawler skill'i iki kez ateşlenir.
  - Chain: Frenzied Leap öldürme sonrası → Echo stack anında 3'e çıkar
  - Neden uyuyor: Hasar alma dışında kaynak üretme yolu açıyor — öldürerek de "deli" olabilirsin

---

### ⚖️ PALADİN

**Lay on Hands → Divine Intervention ⚠️ KOŞULLU**
- Mevcut: Anlık tam HP iyileşme, 90s CD — roguelite'ta 2-3 kez kullanılabilir
- Yeni (Divine Intervention): 50 HP harcar, 4m kutsal bölge 4s, mermi siler, hasar -%50
- Endişe: 50 HP maliyet Paladin'in Builder/Spender döngüsüyle çakışabilir. HP üretim hızına göre değer değişir.
- Revize öneri: CD'yi 45s'ye düşür, HP maliyetini 30'a çek, süreyi 6s yap. Daha kullanılabilir.
- Tag: ⚓↓ (ANCHOR / SPENDER) — Advanced ✅ (revize ile)

**Blessed Weapon için eksik öneri — BENİM TAMAMLAMAM:**
- Sorun: Vuruş başına 15 HP geç oyunda düşman hasarı yanında anlamsızlaşır
- Önerim → **Consecrated Strike** (↑⚡ BUILDER/CHAIN — Advanced)
  - Efekt: 10s: her melee vuruşu zeminde Consecration mirası bırakır (0.5s). Üst üste binen alanlar güçlenir.
  - Chain: Shield of Retribution patlamasından hemen sonra → birikmiş tüm alanlar anında tetiklenir
  - Neden uyuyor: Paladin'in "kutsal zemin yönetimi" build eksenini güçlendirir

---

### 💀 SUMMONER

**Raise Archer → Soul Siphon Totem ✅**
- Mevcut: Uzak archer iskelet — Raise Skeleton ile çok benzer, sadece menzil farkı
- Yeni (Soul Siphon Totem): Totem 8s boyunca 5m çevresinden ruh emer, 0.5 Charge/sn üretir
- Neden çok daha iyi: PoE Totem konsepti — bağımsız yapı, Summoner'ın "kurban/kontrol" fantezisine yeni boyut
- Tag: ↑⚓ (BUILDER / ANCHOR) — Core ✅

**Sacrificial Pact için eksik öneri — BENİM TAMAMLAMAM:**
- Sorun: Tüm minyonları feda edince tamamen savunmasız kalırsın — çok yüksek risk
- Önerim → **Soulbind** (⚡↑ CHAIN/BUILDER — Advanced)
  - Efekt: Seçilen bir minyonla "ruh bağı" kur. Bu minyon ölünce patlamaz — bunun yerine tüm CD'ler -%40 azalır ve Summoner 3s invulnerable olur.
  - Chain: Lich Form aktifken Soulbind → CD sıfırlanır, Lich Form süresi uzar
  - Neden uyuyor: Minyon ölümünü "kayıp" değil "koz" olarak çerçeveler — feda loop'unu güçlendirir

---

### 🔮 HEXER

**Enervate → Hex Contagion ❌ RED**
- ChatGPT önerisi: Stack'leri yakın düşmanlara kopyalar + root
- Sorun: Bu zaten Pandemic'in yaptığı şey (TÜM stack'leri kopyalar). Hex Contagion daha zayıf versiyonu oluyor.
- Karar: Enervate kalır (%50 slow + saldırı hızı -%40 — CC olarak hala özgün)

**Silence Hex için eksik öneri + Enervate yerine daha iyi:**
- Silence Hex sorun: Roguelite düşmanları genellikle "cast" yapmıyor, işlevsiz kalıyor
- Önerim — Silence Hex → **Hex Overload** (⚡💥 CHAIN/FINISHER — Advanced)
  - Efekt: 6s: hedef Pressure Phase (4-6 stack) veya üstündeyken aldığı her hasar +1 stack kazandırır. Hexblast bu sürede 2× hasar verir.
  - Chain: Corruption sonrası → Hex Overload süresi 10s'ye çıkar
  - Neden uyuyor: Hexer'ın "sabır" döngüsünü hızlandırır — 4+ stack'i gerçek tehdit haline getirir

---

## BÜYÜK SORU: SINIF SİSTEMİNİ BAŞTAN MÜ DÜŞÜNELIM?

ChatGPT'nin önerileri iyiydi ama şu temel sorular hala açık:

**1. Warblade ve Brawler çok benzer mi?**
Her ikisi: melee, CC var, knockback var, hasar odaklı. Fark sadece kaynak sistemi.
Warblade = "giriyorum, çıkmıyorum" / Brawler = "hasar alarak güçleniyorum"
Bu yeterince farklı bir his mi? Playtestе görülmeli.

**2. Mevcut 8 sınıfın "duygusal renk paleti" tamamlanmış mı?**
- Hızlı/kinetik sınıf yok (hız = güç fantezisi)
- Zamancı/ritim manipülasyonu yok
- Terrain/taktik odaklı sınıf yok (Ranger kısmen ama yeterli mi?)

**3. Hangi sınıf "ilk run için ideal" olmalı?**
Şu an bu hiçbir belgede tanımlanmamış. Warblade en basiti ama bu kararlaştırılmadı.

---

## SONUÇ — NE YAPALIM

### Hemen uygulanabilir (bu session'da):
- [ ] Warblade: Hamstring → Gravity Cleave, Whirlwind → Ironclad Momentum
- [ ] Elementalist: Frostbolt → Glacial Spike (revize ile), Mana Shield → Arcane Surge
- [ ] Rogue: Sprint → Mirage Blade, Deadly Poison → Toxic Eruption
- [ ] Ranger: Serpent Sting → Barbed Net Shot, Spirit Wolf → Terrain Spike
- [ ] Brawler: Last Rites → Undying Tenacity, War Cry → Bloodlust Echo
- [ ] Paladin: Lay on Hands → Divine Intervention (revize), Blessed Weapon → Consecrated Strike
- [ ] Summoner: Raise Archer → Soul Siphon Totem, Sacrificial Pact → Soulbind
- [ ] Hexer: Silence Hex → Hex Overload (Enervate kalır)

### Karar gerektiren:
- [ ] Warblade + Brawler benzerlik sorunu — kabul mü, revize mi?
- [ ] Eksik "duygusal renkler" için yeni sınıf mı, mevcut sınıf revizyonu mu?
- [ ] "İlk run için önerilen sınıf" kararı
