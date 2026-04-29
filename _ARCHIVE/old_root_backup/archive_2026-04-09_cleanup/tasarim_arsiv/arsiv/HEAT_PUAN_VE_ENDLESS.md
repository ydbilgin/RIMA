# HEAT SİSTEMİ + PUAN + ENDLESS MOD — Tasarım Araştırması
*2026-03-27 | Claude analizi (Bölüm A) + Ollama sentezi (Bölüm B — eklenecek)*

---

## BÖLÜM A — CLAUDE ANALİZİ

### A1. Heat Sistemleri — Referans Oyunlar Karşılaştırması

#### Hades — Pact of Punishment

Hades'in Heat sistemi en iyi örneklerden biri. Neden:

**Modifikasyon kategorileri:**
```
Stat modlar (sıkıcı ama gerekli zemin):
  - Hard Labor: düşman HP +20%/+40%/+60%
  - Lasting Consequence: iyileşme %30/60/80 azaldı

Kural değiştiren modlar (asıl ilginç olanlar):
  - Extreme Measures: boss pattern'ları tamamen değişiyor
  - Tight Deadline: odada 60 saniye geçirince mini boss çıkıyor
  - Olympian Favor: boon seçimlerinde sadece 1 seçenek
  - Forced Overtime: düşmanlar +50% hız AMA sen de +25% hız

Ceza + ödül dengesi:
  - Chaos Boon benzeri: birini alırsan bonus puan var
  - Kombinasyon bonusu: birbirine zıt iki modu aynı anda seç → ekstra reward
```

**Neden işe yarıyor:** Her modifier bağımsız değil, kombinasyon yaparken sinerji veya anti-sinerji yaratıyor. "Tight Deadline + Extreme Measures" = hem acele hem farklı boss. "Olympian Favor + Hard Labor" = daha zayıf build ama daha güçlü düşman.

**Bizim oyunumuz için ders:** Grudge sistemi "Tight Deadline"ın karşılığını kendi başına veriyor — ama Heat modları bu ilişkiyi daha sıkı hale getirebilir.

---

#### Dead Cells — Boss Cells (BC)

**BC sistemi nasıl çalışır:**
```
BC0: Temel oyun
BC1: Düşmanlar daha agresif, scrolls azaldı
BC2: Bazı biome'lar "cursed" — hasar alırsan anında ölürsün (1 hit kill koridoru)
BC3: Elitler daha sık, daha güçlü
BC4: Starter loadout kısıtlandı
BC5: Full curse everywhere + Harder elites
```

**En ilginç mekanik — Curse (BC2+):**
Belirli koridorlarda her hasar = anlık ölüm. Bu koridor görünür ama içindeki güçlü item görülüyor. "Giriyor musun?" kararı.

**Bizim oyunumuz için ders:** Cursed room = Elite odanın Grudge versiyonu. "Bu odaya girersen Grudge düşman kesinlikle programını kullanacak" mod eklenilebilir.

---

#### Slay the Spire — Ascension

**Ascension 1-20 yapısı:**
```
A1-A4:   Stat artışları (düşman HP, başlangıç altın azaldı)
A5-A8:   Kural değişimleri (daha az relic, event seçenekleri azaldı)
A9-A12:  Mekanik değişimler (rest site'ları zayıfladı, elitler güçlendi)
A13-A16: Fundamental changes (başlangıç curse kartı, boss relics forced)
A17-A20: Near-impossible (kombinasyon, HP kısıtı, tüm modifier aktif)
```

**Neden StS Ascension diğerlerinden iyi:**
- Her Ascension tek bir şeyi değiştiriyor — öğrenmesi kolay, hissetmesi net
- Hiçbir Ascension "sadece daha zor" değil — stratejiyi değiştiriyor
- A20 = birikimlerin toplamı, tüm kararların test edilmesi

**Bizim oyunumuz için ders:** Her Heat = tek ve net bir kural. "Heat 3: Grudge düşmanlar 2. direnç ile geliyor" gibi. Birden fazla şey değiştirme aynı seviyede.

---

#### Path of Exile — Juiced Maps

**PoE'nin "juice" sistemi:**
```
Map mods: hasar alıyorsun ama daha fazla item düşüyor
Kirac mods: belirli encounter türleri ekleniyor
Corruption: modifier ekler AMA randomize olur (kontrol kaybı = yüksek ödül)
```

**Juicing prensibi:** Oyuncu istediği zorlukla ne kadar "sos eklediğini" seçiyor. Az sos = kolay ama az drop. Çok sos = ölme riski ama muhteşem drop.

**Bizim oyunumuz için ders:** Shop fiyatları Heat'le artmasın — ama daha nadir itemlar çıksın. "Heat 5: Shop'ta artık Rare item şansı +40%" gibi ödül dengesi.

---

### A2. Performans Çarpanı — Neyi Ölçmeli, Neyi Ölçmemeli

#### Geçerli Metrikler (beceri ölçer)

```
✓ No-hit oda temizleme
  Neden geçerli: Yeterli bilgi ile her saldırı dodge edilebilir
  Nasıl ölçülür: Boolean per oda, run içinde biriktirilir
  Sorun: Ranged düşmanlar + kalabalık oda = şans faktörü var
  Çözüm: Sadece Elite odada no-hit sayılsın (daha kontrollü savaş)

✓ Grudge mastery
  Neden geçerli: Tamamen kasıtlı karar — şansla ilgisi yok
  Nasıl ölçülür: Elite'i önceki run'da programladığın weakness ile mi öldürdün?
  Değer: Oyunun özgün mekaniği — başka hiçbir oyun bunu ölçemiyor

✓ Skill synergy kullanımı
  Neden geçerli: Cross-class combo tetiklenince → oyuncu kombinasyonu kurdu
  Nasıl ölçülür: Cross-class pasif kaç kez tetiklendi / oda başına
  Sorun: Pasifler otomatik — tetiklenmesi kısmen şans
  Çözüm: "aktif skill zincirleme" sayılsın (kasıtlı sequence)
```

#### Geçersiz Metrikler (şans ölçer)

```
✗ Toplam hasar verilen
  Neden geçersiz: Build power = hasar, beceri değil
  Örnek: Fallen Saint combo normalde 3x daha fazla hasar veriyor

✗ Oda temizleme süresi (tek başına)
  Neden geçersiz: Kalabalık oda rastgele → süre kısmen şans
  Ama: Kombinasyon olarak kullanılabilir (no-hit AND hızlı = geçerli)

✗ Hayatta kalınan HP miktarı
  Neden geçersiz: Paladin/Berserker HP yönetimi çok farklı
  Örnek: Fallen Saint kasıtlı düşük HP'de = daha iyi build ama düşük HP puanı
  Çözüm: HP puanı sınıf bazlı normalize edilmeli
```

#### Fallen Saint Problemi

Fallen Saint (Berserker + Paladin) kasıtlı olarak %20 HP'de kalıyor. Bu oyunun tasarımı. Ama standart HP puanı bunu cezalandırır.

**Çözüm:** Sınıf kombosuna göre HP puanı normalize et:
```
Fallen Saint kullandıysa:
  HP puanı hesaplama: (currentHP / optimalHP_for_combo) yerine
  "Martyr's Loop aktif miydi? Kaç saniye aktif kaldı?" ölçülsün
```

---

### A3. Heat Sistemi — Bizim Oyunumuza Özel 20 Modifier

**Tasarım prensibi:** Her modifier kategorize edilmiş. "Rule changer" olanlar öncelikli.

#### Tier 1 — Erişilebilir (Heat 1-5)

| Heat | Modifier Adı | Efekt | Kategori | Neden İlginç |
|------|-------------|-------|----------|--------------|
| 1 | **Iron Resolve** | Düşman HP +25% | Stat | Zemin kuralı |
| 2 | **Scarce Blessings** | Skill acquisition: 3 yerine 2 kart | Rule | Build hızı yavaşlıyor |
| 3 | **Grudge Memory** | Elite düşmanlar 2. run'da 2 direnç ile geliyor (1 yerine) | Grudge | Programlama daha kritik |
| 4 | **Tight Economy** | Shop fiyatları +50% AMA Rare item şansı +30% | Economy | Risk/ödül dengesi |
| 5 | **Aggressive Pursuit** | Normal düşmanlar detect range 2x (seni daha erken görüyor) | Tempo | Keşif azalıyor |

#### Tier 2 — Strateji Değişiyor (Heat 6-10)

| Heat | Modifier Adı | Efekt | Kategori | Neden İlginç |
|------|-------------|-------|----------|--------------|
| 6 | **Cursed Flux** | Flux odası (reforge) artık %50 ihtimalle skill'i downgrade ediyor | Chaos | Güvenli değil artık |
| 7 | **Deadline** | Odada 75 saniye geçirince mini-elite spawn oluyor | Tempo | Temizlemeye zorluyor |
| 8 | **Grudge Network** | Tüm aynı türdeki Elite'ler aynı ağda: birini doğru öldürünce diğerleri de hatırlıyor | Grudge | Planlama run boyunca bağlı |
| 9 | **Olympian Scarcity** | Oda ödülü: yalnızca 1 seçenek (3 yerine) | Rule | Her seçim daha ağırlıklı |
| 10 | **Gauntlet Mandatory** | Her boss sonrası Gauntlet zorunlu (isteğe bağlı değil) | Structure | Survival skill şart |

#### Tier 3 — Ciddi Zorluk (Heat 11-15)

| Heat | Modifier Adı | Efekt | Kategori | Neden İlginç |
|------|-------------|-------|----------|--------------|
| 11 | **Withered Healing** | Act geçişi HP recovery %30 → %10 | Attrition | Hasarı azaltmak şart |
| 12 | **Phantom Grudge** | Elite öldürülürse bile Grudge kaydı silinmiyor — ölmeden önce ruh hasar veriyor | Grudge | Öldürmek yetmiyor |
| 13 | **Class Lockout** | Run başında 1 sınıf rastgele yasaklı (8'den 7'ye) | Build | Build çeşitliliği zorlanıyor |
| 14 | **Corrupted Drops** | Skill kartlarının %30'u "Corrupted" gelir: +güç ama downside var | Chaos | Her seçim riskli |
| 15 | **Extreme Grudge** | Tüm Elite'ler run başından programlanmış geliyor (önceki run'dan hatırlıyorlar) | Grudge | Planlama run öncesine taşındı |

#### Tier 4 — Masokist (Heat 16-20)

| Heat | Modifier Adı | Efekt | Kategori | Neden İlginç |
|------|-------------|-------|----------|--------------|
| 16 | **One Mistake** | Herhangi bir odada %50+ hasar alırsan → Cursed Wound: run boyunca max HP -20% | Punishment | Agresif defensive play |
| 17 | **Boss Memory** | Boss her Act'te öncekinden öğrenmiş: en çok kullandığın skill'e karşı pattern var | Adaptive | Her run farklı |
| 18 | **Stripped Arsenal** | Run başında 2 aktif skill slot (6 yerine), oda tamamladıkça kazanıyorsun | Start Weak | Erken game çok zor |
| 19 | **Gauntlet Tithe** | Gauntlet'te hayatta kalamazsan run puanı %40 azalır | Stakes | Gauntlet artık kritik |
| 20 | **True Ascension** | Tüm Tier 1-3 modifier aktif aynı anda | Summit | Oyunun gerçek sonu |

---

### A4. Puan Sistemi — Final Formül

#### Taban Puanlar

```
Normal oda temizlendi:           100 puan
Elite oda temizlendi:            400 puan
Boss öldürüldü:                 1500 puan
Gauntlet (hayatta kalındı):      10 puan/sn + 5 puan/kill + 50/drop
Shop ziyareti:                    0 puan (servis)
Flux odası (reforge yapıldı):   200 puan (cesaret bonusu)
Secret oda bulundu:              300 puan
```

#### Performans Çarpanı (Oda Bazlı)

Her oda temizlenince bu çarpanlardan EN YÜKSEK BİR TANESİ uygulanır (üst üste binmez):

```
No-hit (sadece Elite + Boss odada sayılır):  × 2.5
No-hit (Normal oda):                         × 1.8
Hızlı temizleme (< 40 saniye):              × 1.4
Sağlıklı çıkış (HP > %80):                  × 1.3
Standart temizleme:                           × 1.0
Zorlanarak temizleme (HP < %30):             × 0.9
```

**Özel çarpanlar (üsttekilerle çarpışmaz — her zaman eklenir):**
```
Grudge counter kill:      + %100 oda puanı
Skill zincirleme (proc):  + 20 puan/proc tetiklenme
Cross-class pasif aktif:  + 50 puan/tetiklenme
```

#### Run Sonu Çarpanı

```
Heat seviyesi:         × (1.0 + Heat × 0.12)
  → Heat 0:  × 1.0
  → Heat 5:  × 1.6
  → Heat 10: × 2.2
  → Heat 20: × 3.4

Kombo zorluk çarpanı:
  → Fallen Saint, Plague Doctor (kasıtlı OP):  × 0.85
  → Standart kombolar:                          × 1.0
  → Düşük sinerji / zor kombolar:              × 1.2
  → İlk kez oynanan kombo (yeni keşif):        × 1.1

HP bonusu (run sonu):
  → Normal sınıflar: (currentHP / maxHP) × 500
  → Fallen Saint: (Martyr's Loop aktif süre / total savaş süresi) × 500
```

#### Final Formül

```
Run Puanı =
  [Σ (oda_taban × performans_carpan) + Σ (grudge_bonus + proc_bonus)]
  × heat_carpan
  × kombo_carpan
  + hp_bonus
  + gauntlet_puani
```

---

### A5. Leaderboard Yapısı

#### Kategori Ayrımı (zorunlu)

```
Primary filter:  Heat Seviyesi (0-20, her seviye ayrı)
Secondary filter: Mod tipi (Normal Run / Gauntlet dahil / Survival — gelecek)
Optional filter:  Kombo (W+E, W+Rogue, Plague Doctor vs.)
Optional filter:  Sınıf (Primary class bazlı)
```

#### Leaderboard Satırı

```
[Rank] [Oyuncu Adı] [Puan] [Kombo] [Heat] [Süre] [Tarih]
   1.  Laureth       16054  W+Hxr   H-5   41:22  27 Mar
```

**Satıra tıklayınca detay:**
```
Run özeti:
  Oda sırası:        N→E→N→B→N→E→N→E→B→Gauntlet→B
  Grudge counter:    2/3 (%66 mastery)
  En yüksek oda:     Elite No-Hit + Grudge = 1600 puan
  Aktif build:       Charge | Mortal Strike | Whirlwind | Corruption | Pandemic
  Cross pasif:       Cursed Steel (aktif)
  Gauntlet süresi:   87 saniye (1520 puan)
  Heat modları:      Scarce Blessings + Grudge Memory + Tight Economy
```

**Saklanan veri (Steam leaderboard detail array — max 64 int):**
```csharp
int[] details = {
  heatLevel,           // 0-20
  primaryClass,        // enum int
  secondaryClass,      // enum int
  totalTime,           // saniye
  grudgeMastery,       // 0-100 yüzde
  noHitRooms,          // sayı
  gauntletTime,        // saniye (0 = girmedi)
  finalHP,             // 0-100 yüzde
  // ... diğer metrikler
};
```

---

### A6. Gauntlet Modu — Final Tasarım

#### Wave Yapısı (Her Boss Sonrası)

```
Saniye 0-15:   Hafif dalgalar — önceki act'teki Normal oda düşmanları
Saniye 15-35:  Orta — Elite miniyon tiplerinden kalabalık sürü
Saniye 35-55:  Yoğun — run boyunca karşılaşılan tüm türler karışık
Saniye 55-75:  Bullet hell — hız +50%, sayı +100%, alan mermiler başlıyor
Saniye 75+:    Sonsuz — 15 saniyede bir dalga yoğunlaşıyor (cap yok)
```

#### Drop Sistemi

Düşen droplar:
```
Soul Dust küçük (+3):      her 5 kill
Soul Dust büyük (+15):     mini-elite ölümü
Geçici buff:               15 sn süre, run sonrası siliyor
  → Hasar +20%
  → Hız +30%
  → Lifesteal +10%
  → Shield (bir sonraki hit'i engelle)
```

Drop yerleşimi: Düşman öldüğü yerde düşüyor (Vampire Survivors gibi). Sen hareket ederken topluyorsun.

#### Gauntlet + Heat Etkileşimi

```
Heat 0-4:  Gauntlet isteğe bağlı (kapı görünür, girmeyebilirsin)
Heat 5-9:  Gauntlet girmeyince -10% run puanı (cezalandırılmak yok ama kayıp var)
Heat 10+:  Gauntlet zorunlu (kapıdan geçmeden oda tamamlanmış sayılmıyor)
Heat 15+:  Gauntlet'te Grudge elite'ler de spawn oluyor (Grudge Network ile)
```

---

### A7. Yeni Ekleme Önerileri — Araştırmadan Çıkan Fikirler

Aşağıdakiler henüz tasarımda yok ama güçlü sinerji oluşturabilir:

#### Öneri 1: Cursed Skills (Lanetli Skill Kartları)

Heat 14 modifier olarak: kartların %30'u "Corrupted" gelebilir.
Corrupted kart = güçlü versiyon + downside:

| Corrupted Skill | Normal Versiyon | Corrupted Efekt | Downside |
|-----------------|-----------------|-----------------|----------|
| Cursed Charge | Charge +stun | Charge +stun +AoE | Her kullanımda Rage -10 |
| Bleeding Whirlwind | Whirlwind AoE | Whirlwind + Bleed tüm düşmanlara | Whirlwind süresinde sen de bleed alıyorsun |
| Soul Rend | Corruption DoT | Corruption + anında %30 hasar | Cast ederken 2 saniye hareketsiz kalırsın |
| Hollow Execute | Execute HP eşiği | Execute threshold %40'a çıktı | Execute sonrası 3 saniye zayıf (hasar -40%) |

---

#### Öneri 2: Boss Soul Zinciri (3 Mutation)

Şu an: 1 boss = 1 skill mutasyonu.
Önerilen: Her boss farklı mutation *tipi* sunar:
```
Act 1 Boss Soul: "Enhance" — skill güçleniyor ama CD +2s
Act 2 Boss Soul: "Corrupt"  — skill değişiyor + downside (ama çok güçlü)
Act 3 Boss Soul: "Ascend"   — önceki iki mutation'ı birleştirir, CD yarıya iner
```

3 Act boyunca bir skill'in "yolculuğu" var. Hangi skill'i mutasyona sokacağını run başından planlıyorsun.

---

#### Öneri 3: Shrine Sistemi (Curse-Blessing Olayları)

Harita'da %15 ihtimalle "Shrine" odası çıkabilir (Secret'ın varyantı):
```
Shrine örneği — Kan Fısıltısı:
  "Soul Dust +30 al. Ama bir sonraki Elite sana karşı programlanmış başlar."
  → Evet / Hayır
  → "Evet" = zengin ama hazır olmayan Elite

Shrine örneği — Kirli Anlaşma:
  "Aktif skill'lerinden birini kaybet. Karşılığında run sonuna kadar +1 skill slot (7)."
  → Ödül büyük ama build pivot gerekiyor

Shrine örneği — Gölge Fısıltısı:
  "Bir sonraki Grudge Elite'in direncini öğren (savaşmadan)."
  → Ücretsiz bilgi — ama sadece o Elite için
```

---

#### Öneri 4: Adaptive Boss Sistemi

Şu an: Boss script'i static pattern.
Önerilen: Boss run boyunca seni izledi, en çok kullandığın 2 skill türüne karşı pattern geliştirdi:

```
GameManager → BossObserver:
  Track: en sık kullanılan element tipi (Fire, Frost, Physical vs.)
  Track: en sık tetiklenen cross-class pasif

Boss spawn ederken:
  pattern_modifier = BossObserver.GetTopUsedElement()
  if (Fire) → Boss: Phase 2'de fire-absorb shield ekle
  if (Frost) → Boss: Phase 2'de slow-immune + hız burst
  if (Rogue combo) → Boss: Phase 2'de blink yok eden alan var
```

**Neden:** Her run'da boss gerçekten farklı hisseder. Grudge sisteminin boss versiyonu.

---

#### Öneri 5: Run Başı "Contract" (Opsiyonel Challenge)

Run başında Hub'da seçilebilir:

```
"Bu run için bir şart koş. Yerine getirirsen bonus ödül."

Örnek contractlar:
  □ "5 Elite'i doğru Grudge ile öldür → Run puanı × 1.3"
  □ "Shop'a girme → Run sonunda +50 Soul Dust"
  □ "Gauntlet'te 2 dakika dayan → Ekstra Boss Soul mutation"
  □ "Yalnızca 1 sınıf kullan (ikincisini seçme) → Heat +2 sayılır"
```

Solo dev scope: bunlar sadece flag + sonuç kontrol. Yeni sistem gerektirmiyor.

---

*Bölüm B — Ollama Sentezi eklenecek (araştırma tamamlanınca)*

---

## BÖLÜM B — OLLAMA SENTEZİ

*deepseek-r1:14b | 2026-03-27 | 7 bölüm araştırma ham çıktısından çıkarılan net bulgular*

---

### B1. Ollama'nın Desteklediği Kararlar (Claude analizi ile örtüşüyor)

```
✓ Performans çarpanı aralığı: 0.5x - 3.0x (minimum cap önemli — cezalandırma değil)
✓ Leaderboard Heat bazlı ayrılmalı (aynı tabloda olmamalı farklı Heat'ler)
✓ Grudge mastery puanı: +15% oda puanı (kendi analizimde +100% — Ollama daha konservatif)
✓ Survival mode oturumu: 10-20 dakika ideal (main game ile çakışmıyor)
✓ Curse-blessing eventleri erken başlasın (Heat 0'da da olabilir, sadece Heat 3+ değil)
```

---

### B2. Ollama'nın Eklediği Yeni Fikirler

**Predictive AI (Bölüm 01 — Rule Change Modifier):**
> "Enemies predict and dodge certain attack patterns."

Bu Heat 12-14 için mükemmel bir kural değiştirici. Grudge sisteminin doğal uzantısı:
- Elite düşman artık sadece direnç değil, aktif dodge geliştiriyor
- "Sürekli Charge kullanıyorsun → düşman Charge'ın yolundan çıkıyor"
- Oyunun kendi öğrenme mekanizmasını (Grudge) düşmanın öğrenmesine uyguluyorsun

**Habitual Killer Pasifi (Bölüm 05):**
> "Gain increased damage/resistance against enemies you consistently defeat with the same strategy."

Bu mevcut neutral pasif listesine eklenebilir (N-P15 olarak):
- Grudge sisteminin "tersine" versiyonu — sen de bir alışkanlık geliştiriyorsun
- "3 farklı run'da aynı elit türünü aynı yöntemle öldürdün → +%20 hasar o türe"
- Meta-run arası hafıza: sadece o run değil, birden fazla run boyunca

**Dual-Dimension Leaderboard (Bölüm 03):**
> "Dual Metrics: primary score + secondary Grudge Mastery score visible alongside."

Satırda iki değer göster:
```
[Rank] [Oyuncu] [Puan] [Grudge %] [Kombo] [Heat]
  1.   Laureth   16054    87%       W+Hxr   H-5
```
Grudge mastery yüzdesi ayrı takip edilirse "perfect grudge" run ayrıca görünüyor.

**Shrine Eventleri Heat 0'da (Bölüm 06):**
> "Introduce these events at Heat 0 to provide immediate variety."

Kendi analizimde Heat 3+'ta açılıyordu. Ollama'nın argümanı güçlü: erken çeşitlilik oyuncu tutma için önemli. FAZ 3'te eklenecek ama Heat kısıtı olmadan. Basit 2-3 shrine ile başla.

**Survival Mode — Anlık Skill Upgrade (Bölüm 04):**
> "Players can instantly upgrade abilities using Soul Dust found during survival waves."

B modu (Survival) için skill acquisition sistemini basitleştir:
- Main game: 3 kart seç (yavaş, taktiksel)
- Survival mode: Soul Dust biriktir → anlık upgrade seç (hızlı, aksiyon)
Bu iki mod arasında tempo farkını netleştiriyor.

---

### B3. Ollama ile Görüş Ayrılıkları — Kesin Kararlar

**Anlaşmazlık 1: "Grudge System Disabled" modifier (Bölüm 07, Tier 3)**
> Ollama önerdi: Heat 10'da Grudge sistemi devre dışı bırakılsın.

**KARAR: REDDEDİLDİ.**
Grudge bu oyunun kimliği. Devre dışı bırakmak = oyunun en özgün mekaniğini silmek. Heat modları oyunun kalbini kesmemeli. Bunun yerine: "Grudge Network" (Tier 2) gibi sistemi daha zor hale getir ama kaldırma.

**Anlaşmazlık 2: Puan taban değerleri (Bölüm 07)**
> Ollama önerdi: Normal=100, Elite=200, Boss=300, Secret=400.

**KARAR: Kendi analizim geçerli.**
Elite odası Boss'tan daha değersiz olamaz. Zorluk sırası: Normal < Elite < Boss.
Doğru: Normal=100, Elite=400, Boss=1500, Secret=300.
Boss puanı yüksek çünkü run'ın zirvesi — motivasyon için kritik.

**Anlaşmazlık 3: Gauntlet "5 sabit dalga" (Bölüm 07)**
> Ollama önerdi: 5 dalga, her dalga +%50 düşman.

**KARAR: Kendi analizim geçerli (saniye bazlı + sonsuz).**
5 dalga sınırlı ve bitmek üzere his veriyor. Saniye bazlı eskalasyon + sonsuz → "ne zaman çıkacağım?" gerilimi daha iyi.

**Anlaşmazlık 4: Skill Cooldowns Extended (Tier 1 modifier)**
> Ollama önerdi: Heat 3'te skill CD +%20.

**KARAR: Kısmi kabul — ama farklı şekilde.**
Flat CD artışı sıkıcı. Bunun yerine: "Scarce Blessings" (kart sayısı 3→2) daha stratejik hissettiriyor. CD artışı Tier 3+'a taşınabilir ve belirli skill türüyle sınırlı olsun.

---

### B4. Sentezden Çıkan Final Eklemeler

Bölüm A analizine Ollama bulgularıyla eklenen 3 yeni eleman:

**Ek 1: Predictive AI Heat Modifier (Tier 3 — Heat 13)**

| Heat | Modifier Adı | Efekt |
|------|-------------|-------|
| 13 | **Echo Chamber** | Bir run'da 3+ kez kullandığın aktif skill → elite düşmanlar o skill'in yolundan çıkıyor (dodge). |

Implementasyon: `SkillUsageTracker` → en sık 1-2 skill takip → EnemyAI'ye geçilir → o skill'e dodge animasyonu ekler.

**Ek 2: Habitual Killer — Neutral Pasif N-P15**

| # | İsim | Etki |
|---|------|------|
| N-P15 | **Habitual Killer** | Bir elite türünü 3 farklı run'da aynı yöntemle öldürürsen → o türe kalıcı +%20 hasar (meta-run hafızası). |

**Ek 3: Leaderboard Dual-Dimension**

Leaderboard satırına Grudge Mastery yüzdesi eklendi:
```
[Rank] [Oyuncu] [Run Puanı] [Grudge %] [Kombo] [Heat] [Süre]
```
İki ayrı kategori ödüllendirilir: "En yüksek puan" ve "En yüksek Grudge mastery."

---

## BÖLÜM C — FİNAL KARARLAR (Eksiksiz Referans)

### C1. Heat Sistemi — 20 Modifier Final Liste

| # | Heat | Modifier | Tier | Kategori |
|---|------|----------|------|---------|
| 1 | H1 | **Iron Resolve** — düşman HP +25% | 1 | Stat |
| 2 | H2 | **Scarce Blessings** — skill kart sayısı 3→2 | 1 | Rule |
| 3 | H3 | **Grudge Memory** — elite 2. karşılaşmada 2 direnç | 1 | Grudge |
| 4 | H4 | **Tight Economy** — shop +%50 pahalı, Rare şansı +%30 | 1 | Economy |
| 5 | H5 | **Aggressive Pursuit** — detect range 2x | 1 | Tempo |
| 6 | H6 | **Cursed Flux** — Flux reforge %50 ihtimalle downgrade | 2 | Chaos |
| 7 | H7 | **Deadline** — odada 75sn+ → mini-elite spawn | 2 | Tempo |
| 8 | H8 | **Grudge Network** — aynı türdeki tüm elitler ortak hafıza | 2 | Grudge |
| 9 | H9 | **Olympian Scarcity** — oda ödülü 3→1 seçenek | 2 | Rule |
| 10 | H10 | **Gauntlet Mandatory** — her boss sonrası Gauntlet zorunlu | 2 | Structure |
| 11 | H11 | **Withered Healing** — act geçişi HP recovery %30→%10 | 3 | Attrition |
| 12 | H12 | **Phantom Grudge** — öldürülen elite ruh bırakıyor (kısa hasar) | 3 | Grudge |
| 13 | H13 | **Echo Chamber** — 3+ kez kullandığın skill'e elite dodge yapıyor | 3 | Adaptive AI |
| 14 | H14 | **Corrupted Drops** — kart %30 ihtimalle Corrupted (güç+downside) | 3 | Chaos |
| 15 | H15 | **Extreme Grudge** — elitler run başından programlanmış başlıyor | 3 | Grudge |
| 16 | H16 | **One Mistake** — %50+ hasar alırsan → max HP -%20 kalıcı (o run) | 4 | Punishment |
| 17 | H17 | **Boss Memory** — boss en sık kullandığın 2 skill türüne karşı pattern geliştirdi | 4 | Adaptive |
| 18 | H18 | **Stripped Arsenal** — 2 slot ile başla, oda tamamladıkça kazan | 4 | Rule |
| 19 | H19 | **Gauntlet Tithe** — Gauntlet hayatta kalamazsan run puanı -%40 | 4 | Stakes |
| 20 | H20 | **True Ascension** — H1-H15 arası tüm modifier aynı anda | 4 | Summit |

---

### C2. Performans Çarpanı — Final

```
Oda bazlı çarpanlar (EN YÜKSEK BİR TANESİ uygulanır):
  Elite/Boss No-hit:          × 2.5
  Normal oda No-hit:          × 1.8
  Hızlı temizleme (<40sn):    × 1.4
  Sağlıklı çıkış (HP >%80):   × 1.3
  Standart:                    × 1.0
  Zorlanarak (HP <%30):        × 0.9
  Minimum cap:                 × 0.5 (asla bunun altına inmiyor)

Her zaman eklenen bonuslar (üsttekilerle toplanır):
  Grudge counter kill:        + %100 oda puanı
  Skill zincirleme proc:      + 20 puan/proc
  Cross-class pasif tetiklem: + 50 puan/tetiklenme
```

---

### C3. Puan Formülü — Final

```
Taban puanlar:
  Normal oda:   100
  Elite oda:    400
  Boss:        1500
  Flux (reforge yapıldı): 200
  Secret oda:   300
  Gauntlet:     10/sn + 5/kill + 50/drop

Run puanı =
  [Σ (taban × çarpan) + Σ (grudge + proc bonusları)]
  × heat çarpanı [1.0 + Heat × 0.12]
  × kombo çarpanı [OP combo: ×0.85 / standart: ×1.0 / zor: ×1.2]
  + HP bonusu [(currentHP/maxHP) × 500 — sınıf normalize edilmiş]
  + Gauntlet puanı
```

---

### C4. Leaderboard — Final Yapı

```
Zorunlu ayrım: Heat seviyesi (0-20)
İsteğe bağlı filtre: Kombo | Sınıf | Mod tipi

Satır görünümü:
  [Rank] [İsim] [Puan] [Grudge%] [Kombo] [Heat] [Süre] [Tarih]

Tıklayınca detay:
  Oda sırası | En iyi oda | Aktif build | Cross pasif | Gauntlet süresi | Heat modları

Kategori ödülleri:
  "En Yüksek Puan" (ana kategori)
  "En Yüksek Grudge Mastery" (ayrı kategori — puan düşük bile olsa)
  "En Hızlı Boss Kill" (speed category)
```

---

### C5. Gauntlet Modu — Final

```
Wave yapısı (saniye bazlı, sonsuz):
  0-15sn:   Normal oda düşman tipleri, hafif
  15-35sn:  Elite miniyon sürüsü
  35-55sn:  Tüm türler karışık, hız +30%
  55-75sn:  Bullet hell: yoğun, alan mermiler başlar
  75sn+:    Her 15sn dalga yoğunlaşır, cap yok

Drop sistemi:
  Soul Dust küçük (+3): her 5 kill
  Soul Dust büyük (+15): mini-elite kill
  Geçici buff: 15sn süre
    → Hasar +20% / Hız +30% / Lifesteal +10% / Tek vuruş shield

Heat etkileşimi:
  H0-4:    Gauntlet isteğe bağlı (girmeyebilirsin)
  H5-9:    Girmezsen run puanı -%10
  H10+:    Gauntlet zorunlu
  H15+:    Gauntlet'te Grudge elite'ler de spawn oluyor
```

---

### C6. Yeni Eklenecek Mekaniğin Özeti

| # | Mekanik | Ne Zaman | Scope |
|---|---------|---------|-------|
| 1 | Shrine odaları (curse-blessing event) | FAZ 3 | Basit — flag + sonuç |
| 2 | Run Contract (Hub'da opsiyonel şart) | FAZ 5 | Basit — flag + kontrol |
| 3 | Adaptive Boss (run boyunca skill takip) | FAZ 4 | Orta — SkillTracker + BossAI |
| 4 | Boss Soul Zinciri (3 mutation, Act bazlı) | FAZ 4 | Orta — mutation chain |
| 5 | Cursed Skill Kartları (Heat 14 ile) | FAZ 5 | Basit — extra SkillData field |
| 6 | Habitual Killer pasifi (N-P15) | FAZ 4 | Orta — meta-run data store |
| 7 | Dual-dimension leaderboard | FAZ 6 | Steam entegrasyon |

---

*Dosya: TASARIM/HEAT_PUAN_VE_ENDLESS.md*
*Kaynak: Claude analizi + Ollama deepseek-r1:14b (7 bölüm, 2026-03-27)*
*İlgili: HARITA_SISTEMI.md, PASIF_VE_SKILL_SISTEMI.md, UNITY_BASLANGIC_PLANI.md, GOREVLER.md*
*Son güncelleme: 2026-03-27*
