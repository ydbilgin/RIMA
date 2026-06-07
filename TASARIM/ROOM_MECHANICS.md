> ℹ️ Tarihli tasarım dokümanı. Güncel kararlarla çelişirse en yeni `STAGING/*_DECISION_*.md` + `AI_READER_GUIDE.md` kazanır. (2026-06-07)

---
status: REFERENCE
faz: 1
tarih: 2026-04-30
ozet: "Oda mekanik kuralları ve tag sinerji tablosu"
---
# RIMA — Oda Mekaniği Tasarımı (Mantıksal Spec)
*Gorsel degil mantiksal. Kodlamaya ve rima-codex'e hazir referans.*
*Son güncelleme: 2026-04-17*

---

## 1. Genel İlkeler

### Universal Oda Döngüsü
1. Kapıdan giriş
2. Oda içeriği tetiklenir (tipe göre)
3. Bitiş koşulu sağlanır
4. Ödül verilir
5. Harita Parçası düşer (sadece Combat/Elite)
6. Kapılar görünür hale gelir

### Kapı Kilidi Kuralı
- **Combat / Elite:** Giriş anında kilitlenir → tüm düşman ölünce açılır
- **Shop / Spirit / Event / Curse Gate:** Kilitlenmez, istediğin zaman çık

---

## 2. Oda Sayıları — Act Başına

| Act | Min | Max | Combat | Elite | Shop | Spirit | Unknown | Boss |
|-----|-----|-----|--------|-------|------|--------|---------|------|
| Act 1 | 8 | 9 | 5-6 | 1 | 0-1 | 0 | 1-2 | 1 |
| Act 2 | 9 | 11 | 5-6 | 1-2 | 1 | 1 | 1-2 | 1 |
| Act 3 | 9 | 11 | 5-6 | 1-2 | 1 | 1-2 | 1-2 | 1 |
| Final | 5 | 6 | 3-4 | 1 | 0 | 0-1 | 0-1 | 1 |
| **Toplam** | **31** | **37** | 18-22 | 4-7 | 2-3 | 2-4 | 3-7 | 4 |

**Beklenen run süresi:** 55-70 dakika

**Build-up doğrulaması:**
- Act 1: 5-6 draft → 4 slot dolar + 1-2 tier upgrade → doğru tempo
- Act 2: 5-6 draft → 2 yeni slot + 3-4 upgrade → cross-class şekillenir
- Act 3: 5-6 draft → Legendary hedefi + polish
- Final: 3-4 draft → son hazırlık

**Garanti kuralları:**
- Her act'te min 1 Elite oda
- Act 1 Shop: %50 şansla var (%50 Unknown olur)
- Spirit Encounter: Act 2'den itibaren, her act'te tam 1 tane
- Boss'a giden son oda: Combat veya Unknown (hazırlık ritüeli)
- Boss odası: her zaman haritada görünür (act sonunda belli)

---

## 3. Düşman Bütçe Sistemi (Threat Points)

### Tehdit Puanları
| Düşman Tipi | Puan |
|-------------|------|
| Swarm (Hollow Mite vb.) | 1 |
| Grunt | 2 |
| Elite Grunt | 3 |
| Special | 4 |
| Elite Mob (Elite oda özel) | 5 |

### Act Bütçeleri
| | Combat Odası | Elite Odası |
|--|-------------|-------------|
| Act 1 | 8-12 | 14-18 |
| Act 2 | 14-20 | 20-26 |
| Act 3 | 20-28 | 28-36 |
| Final | 16-22 | — |

### Spawn Kuralları
- **İlk dalga:** bütçenin %40'ı başta spawn
- **İkinci dalga:** ilk dalga %50 ölünce tetiklenir (Act 1'de opsiyonel)
- **Üçüncü dalga:** Act 2+ özel odalarda (Elite dışı)
- Aynı türden max 4 düşman aynı anda
- Act 1'de Special mob yok (The Wound nadir %5)
- Elite oda: her zaman en az 1 Elite mob içerir

---

## 4. Oda Tipleri — Tam Mekanik

### 4.1 Combat Odası ⚔️

**Tetikleyici:** Giriş
**Kilit:** Evet — tüm düşman ölünce açılır

**İçerik:**
- Tehdit bütçesine göre düşman kompozisyonu
- Düşman tipi ağırlıkları: Grunt %60, Swarm %25, Special %15 (Act 2+)
- 1-2 dalga (Act 1) / 2-3 dalga (Act 2+)

**Bitiş:** Tüm düşmanlar ölü

**Ödül sırası:**
1. Harita Parçası zemine düşer (alınması zorunlu — almadan kapı açılmaz)
2. Skill Draft ekranı açılır (bkz. §7)
3. Kapılar görünür, oda tipi ikonları gösterilir

**HP Kaynağı:** Yok. Sadece combat içi lifesteal çalışır.
**Gold:** 5-15 (act'e göre, düşman sayısına bağlı)

---

### 4.2 Elite Odası 💀

**Tetikleyici:** Oyuncu seçer (harita ikonundan görür)
**Kilit:** Evet

**İçerik:**
- Elite Odası bütçesiyle düşman kompozisyonu
- Min 1 Elite mob zorunlu (HP × 2.5, 1 Affix — bkz. §5)
- Geri kalan: normal grunt/swarm

**Bitiş:** Tüm düşmanlar ölü

**Ödül sırası:**
1. HP Yenileme: Max HP × %12 (otomatik)
2. Ayrı "Elite Reward" ekranı → garantili Rare+ offer, 1 seçenek (normal draftan bağımsız)
3. %25 şansla Relic drop (yerden toplanır, zorunlu değil)
4. Harita Parçası (alınması zorunlu)
5. Kapılar açılır

**Gold:** 20-35 (Elite kill'den)

---

### 4.3 Shop 🛒

**Tetikleyici:** Oyuncu seçer
**Kilit:** Hayır

**Envanter (her run yenilenir):**

| İtem | Gold Maliyeti | Görünme Şansı |
|------|--------------|--------------|
| HP Restore Küçük | 30 | Her zaman |
| HP Restore Büyük | 70 | Her zaman |
| Skill Reroll | 0 (1× ücretsiz) | Her zaman |
| Skill Modifier | 80-120 | %80 |
| Tier Upgrade (oyuncu seçer skili) | 150 | %70 |
| Relic | 200 | %30 |
| Class Upgrade (class'a özel pasif) | 250 | %40 |

**Reroll:** 1 ücretsiz / run. Kullanılmaz → biriktirmez. Ek reroll Shop'tan 150 Gold.

**Not:** Shop'ta Skill Draft açılmaz. Çıkmak için kapıya yürü.
**Gold:** Yok (harcama odası)

---

### 4.4 Spirit Encounter 👁️ *(Faz 3+)*

**Tetikleyici:** Oyuncu seçer
**Kilit:** Hayır
**Frekans:** Act 2'den itibaren, her act'te tam 1

**6 Spirit Tipi:**

| Spirit | Tema | Koşulsuz Teklif | Koşullu Ek |
|--------|------|----------------|-----------|
| Forge Wraith 🔥 | Saldırı | 2-3 seçenek her zaman | — |
| Shadow Hound 🐾 | Hareket | 2 seçenek | Son oda ≤5s temizlendiyse +1 |
| Blood Oracle 🩸 | HP trade | 2 seçenek | HP %60 altı → en güçlü görünür |
| Void Seer 👁️ | Kaos | İkonlar kapalı, kör seçim | — |
| Fallen Champion ⚔️ | Kombo | Yok (gelirse reddeder) | Son 3 oda chain'le geçildiyse unlock |
| Ancient Relic 💀 | Güçlü+bedel | Her offer'a curse eşlik eder | — |

**Offer İçeriği** (normal skill değil):
- Mevcut skile "Spirit Tag" → koşullu efekt
- Kaynak döngüsünü etkileyen pasif
- Cross-class sinerji güçlendirme
- Echo Imprint kategorisinden 1 seçenek (bkz. §8)

**Bitiş:** Seçim yapınca Spirit kaybolur
**HP Kaynağı:** Sadece Blood Oracle koşullu senaryoda (koşul sağlanmamışsa yok)

---

### 4.5 Curse Gate 🌀 *(Faz 4+)*

**Tetikleyici:** Oyuncu seçer
**Kilit:** Hayır
**Frekans:** Act başına max 1. Yoksa oda Unknown veya Combat olur.

**İçerik:** 1 aktif teklif — kabul veya ret:

| Teklif Kategorisi | Örnek Maliyet | Örnek Ödül |
|------------------|---------------|-----------|
| Kırık Güç | HP -%20 (anlık) | Tüm aktif skill'ler 1 tier atlar |
| Kan Bedeli | Max HP -%15 (kalıcı) | Garantili Legendary offer |
| Rift Bedeli | Dash CD +%50 (run boyunca) | V Meter her dolumda 2× güç |
| Serim | Mevcut Gold'un %40'ı | 3× Relic drop şansı |

**Ret:** Kapıdan çık → Max HP %5 yenileme
**Kabul:** Teklif anında uygulanır, geri dönüş yok

---

### 4.6 Event Odası 🎲 *(Faz 4+)*

**Tetikleyici:** Oyuncu seçer veya Unknown'dan çıkabilir
**Kilit:** Hayır (bazı eventlar kısa combat başlatır)

**10 Event — İkişer Seçenek:**

| Event | Seçenek A | Seçenek B |
|-------|----------|----------|
| Kırık Sandık | +60 Gold | +30 HP |
| Yabancı Savaşçı | Savaş → Relic ödül | Geç → hiçbir şey yok |
| Eski Rün | Bir skili 1 tier düşür → başka skill Rare'a çıkar | Geç |
| Rift Yankısı | Bir skill +1 tier → random curse | Geç |
| İz | En uzun kullandığın skill CD -%30 (run boyunca) | +3 skill teklifi gör, 1 al |
| Kayıp Savaşçı | +50 Max HP + "last stand" pasif | +100 Gold |
| Kırık Ayna | Güçlü pasif + "Shattering" curse (skill/HP -%1) | Geç |
| Rift Kapı | Bonus gizli oda erişimi (Elite ödüllü) | Normal devam |
| Çoraklaşma | Bir skili kaldır → V Meter anında %100 | Geç |
| Harita Ruhu | Sonraki 5 odanın tipini gör | — (tek seçenek) |

---

### 4.7 Unknown Odası ❓

**Tetikleyici:** Oyuncu seçer (haritada ikon yok)
**İçerik Ağırlıkları:**

| İçerik | Ağırlık |
|--------|---------|
| Combat | %25 |
| Mini-boss (özel Elite) | %15 |
| Gizli Shop | %15 |
| Spirit Encounter | %10 |
| Curse Gate | %10 |
| Event | %10 |
| Max HP odası (+15 Max HP) | %10 |
| Küçük ödül (30-60 Gold) | %5 |

**Notlar:**
- Boş oda asla çıkmaz
- Spirit/Curse Gate/Event: sadece mevcut oldukları fazlarda Unknown'dan çıkabilir
- Mini-boss: normal Elite stat ama affix %2 (iki affix birden)

---

## 5. Elite Affix Sistemi

Her Elite mob 1 affix alır. Birden fazla Elite varsa her biri farklı affix alır.

| Affix | Mekanik | İlk Act |
|-------|---------|---------|
| **Ironclad** | %40 hasar azaltma. 1.5s CC uygulanana kadar aktif. CC → azaltma kırılır | 1 |
| **Volatile** | Ölünce 3m AoE, max HP'nin %25 hasar | 1 |
| **Regenerating** | Her 3s HP %6 yenilenir → DPS check | 1 |
| **Frenzied** | HP %30 altı: saldırı ×1.5, hareket +%40 | 1 |
| **Echoing** | HP %50'ye düşünce %50 HP klon spawn | 2 |
| **Sapping** | Saldırılar oyuncu skill CD +%20 (3s, stack değil) | 2 |
| **Armored** | İlk 2 CC etkisine bağışık, 3.'te normal alır | 2 |
| **Cursed** | Ölünce 5s curse bölgesi. İçinde kaynak regen -%50 | 3 |

---

## 6. Harita Sistemi

### DungeonGraph Bağlantı Mantığı

**Act 1 (lineer + hafif dallanma):**
```
Giriş → Combat/Unknown → [Combat veya Elite fork]
                              ↓
                        Combat → [Shop(opsiyonel)/Unknown fork] → Boss
```

**Act 2-3 (dallanma):**
```
Act boss çıkışı → [3-way fork: Combat / Elite / Unknown]
                        ↓              ↓             ↓
                  [ayrı yollar: 2-3 oda]
                        ↓
                  [merge] → Spirit/Shop → Boss approach (1-2 oda) → Boss
```

### Görünürlük Kuralları

| Durum | Görünürlük |
|-------|-----------|
| Mevcut oda | Tam görünür |
| Bitişik oda (kapı açık) | Oda tipi ikonu görünür |
| 2+ oda uzak | "?" — bilinmiyor |
| Harita Parçası alındıktan sonra | +1 oda ötesinin ikonu görünür |
| Boss odası | Her zaman görünür (act sonunda hep belli) |

### Kapı İkon/Renk Tablosu
| İkon | Renk | Tip |
|------|------|-----|
| ⚔️ | Beyaz | Combat |
| 💀 | Kırmızı | Elite |
| 🛒 | Altın | Shop |
| 👁️ | Mor | Spirit Encounter |
| 🌀 | Koyu mor | Curse Gate |
| 🎲 | Yeşil | Event |
| ❓ | Gri | Unknown |
| ⚡ | Parlak altın | Boss |

---

## 7. Skill Draft Mekaniği

### Tetiklenme
Combat veya Elite oda temizlenince açılır. Boss kill → özel boss ödülü (draft değil).

### Draft Havuzu Oluşturma

**Adım 1 — Offer tipi ağırlıkları:**

| Oyuncu Durumu | New Skill | Tier Upgrade | Echo Imprint |
|---------------|----------|-------------|-------------|
| Slotlar dolu değil + tier upgrade mevcut | %40 | %40 | %20 |
| Tüm slotlar dolu | %10 | %70 | %20 |
| Hiç tier upgrade yok (hepsi Common) | %60 | — | %40 |

**Garanti:** 3 offer içinde min 1 "New Skill VEYA Tier Upgrade" olmalı (hepsi Echo Imprint olamaz).

**Adım 2 — Class pool ağırlıkları:**

| Act | Primary | Secondary | Neutral |
|-----|---------|-----------|---------|
| Act 1 | %100 | — | — |
| Act 2 erken (ilk 3 oda) | %65 | %20 | %15 |
| Act 2 geç | %55 | %30 | %15 |
| Act 3 | %45 | %45 | %10 |

**Adım 3 — Tier havuzu (hangi tier teklif edilir):**
- Common → Rare: Act 1'den itibaren
- Rare → Epic: Act 2'den (başlangıç %30, Act 2 sonu %70)
- Epic → Legendary: SADECE Act 3+ (ağırlık %15, nadir)

**Tier upgrade sıralaması:** Oyuncunun en uzun taşıdığı skill önce önerilir (FIFO).

**Pity sistemi:** Bir skill 5 ardışık draftta çıkmadıysa → sonraki draftta %80 şansla çıkar.

### Reroll
- 1 ücretsiz / run (birikimsiz — kullanılmaz kaybolur)
- Shop: 150 Gold → ek reroll

### Elite Garantili Offer (ayrı ekran)
Elite oda temizlenince normal Draft yerine "Elite Reward" açılır:
- 1 seçenek sunulur (tercih yok, direkt Rare+)
- Oyuncunun mevcut havuzundan çıkar

---

## 8. Echo Imprint Sistemi

**Tetiklenme:** Her 3 combat odada bir kez, normal Skill Draft'a EK olarak sunulur.
**Slot sınırı:** Max 4 / run (act başına 1 slot açılır).

### Kategoriler
| Kategori | Neyi Güçlendirir |
|----------|----------------|
| **Strike Form** | LMB (temel melee/ranged saldırı) |
| **Outlet Form** | RMB (yardımcı/reaktif aksiyon) |
| **Surge Form** | Dash ve Resource döngüsü |

**Örnek Imprints (Warblade):**
- Strike Form: "Iron Combo 3. vuruş → 2s armor break ekler"
- Outlet Form: "Rage Outlet çevredeki tüm düşmanlara 1.5m AoE yanma DoT başlatır"
- Surge Form: "Her dash → Rage+8, boşta Rage drain -%50"

**Faz dağılımı:**
- Faz 1: Yok (Warblade ile test edilecek)
- Faz 2: 4 class için temel imprints (~12 imprint)
- Faz 3+: 10 class × 3 kategori × 3-4 seçenek (full set)

---

## 9. Tag Sinerji Bonusu

Aktif 6 skill slotundan 2 tanesi aynı tag taşıyorsa otomatik pasif bonus tetiklenir. Draft sonrası anlık hesaplanır.

| Tag Çifti | Bonus |
|-----------|-------|
| 2× `▶` (movement) | Dash CD -%20 |
| 2× `💥` (burst/execute) | Burst skill'ler +%10 hasar |
| 2× `⬡` (area) | AoE yarıçapı +0.5m |
| 2× `↑` (defense/sustain) | Alınan hasar -%8 |
| 2× `⚡` (instant/fast) | Tüm CD'ler -0.5s |
| 2× `⚓` (anchor/control) | CC süresi +%20 |
| `▶` + `💥` | Dash sonrası ilk skill +%30 hasar (4s pencere) |
| `✦` + `⚓` | Resource max +20 |
| `↑` + `✦` | Resource dolunca 2s mini-shield |

**Hesaplama:** 6 slot taranır → eşleşen tag çiftleri → bonus uygulanır.
**Limit:** Max 2 sinerji bonusu aynı anda aktif. Fazla eşleşme varsa en güçlü 2 seçilir.

---

## 10. Ekonomi Akışı

### Gold (In-run, geçici)
| Kaynak | Miktar |
|--------|--------|
| Combat oda | 5-15 |
| Elite kill | 20-35 |
| Boss kill | 50-75 |
| Event seçimi (bazıları) | 30-100 |
| Unknown küçük ödül | 30-60 |

**Harcama:** Sadece Shop.
**Act başına beklenen:** Act 1: 50-80 | Act 2: 90-140 | Act 3: 100-150 | Final: 40-70

### Shards (In-run, toplanır)
- Düşman başına: 1-5 Shard (yerden toplanır, otomatik yok)
- Kullanım: Faz 2+ sandık/özel etkileşim

### Echoes (Meta currency, kalıcı — Faz 3+)
| Kaynak | Miktar |
|--------|--------|
| Boss kill | +10 |
| Act tamamlama | +5 |
| İlk kez class seçme | +5 bonus |
| Run sonunda (kill×0.1) | değişken |

**Harcama (Faz 4+):** Hub NPC, class unlock, meta-prog upgrade

---

## 11. HP Ekonomisi

**Rest odası yok. HP dağıtık kaynaklardan gelir — bu kasıtlı.**

| Kaynak | Miktar | Koşul |
|--------|--------|-------|
| Elite temizleme | Max HP %12 | Her Elite oda |
| Shop Küçük HP | Max HP %15 | 30 Gold |
| Shop Büyük HP | Max HP %35 | 70 Gold |
| Curse Gate ret | Max HP %5 | Reddedince |
| Event (bazıları) | Max HP %10-20 | Koşullu |
| Lifesteal skill'leri | Anlık | Combat içi |
| Boss kill | Max HP %50 | Oda sonrası yenileme |

**Act başına beklenen kazanım:** Act 1: 0-12% | Act 2: 10-25% | Act 3: 15-30%
**Tasarım niyeti:** Shop kararları HP baskısı altında alınır. Bu gerginlik kasıtlı.

---

## 12. Boss Odaları

### Giriş
- Harita Parçası tüm combat/elite odalardan alınmış olmalı (kontrol edilir)
- Boss kapısı: ayrı görsel, act'e özel renk/ikon
- Giriş öncesi kısa animasyon (0.5s) — savaş başlamadan önce atmosfer

### Savaş Yapısı
- Ayrı Boss HP bar (oyuncu HP barı üzerinde)
- Arena: geniş, engelsiz, pozisyon önemli
- Faz geçişi: HP bazlı VEYA oynanış bazlı

### Faz Geçiş Mantığı
| Boss | Geçiş Koşulu |
|------|-------------|
| Act 1 Boss (Warden varyantı) | HP bazlı (%50) |
| Act 2 Boss (Fractured King) | Oynanış: 10s içinde 3+ farklı skill kullanılırsa |
| Act 3 Boss (Hollow Sovereign) | Build-adaptive: oyuncunun tag'lerine göre |
| Final Boss (Nexus Core) | Multi-faz: primary class signature → cross-class pasif → Legendary |

### Boss Sonrası Ödüller
| Boss | HP Restore | Gold | Özel |
|------|-----------|------|------|
| Act 1 Boss | Max HP %50 | 75 | Secondary class seçim ekranı → +2 slot → Cross-class Pasif |
| Act 2 Boss | Max HP %50 | 75 | Cross-class Ultimate açılır |
| Act 3 Boss | Max HP %50 | 75 | Legendary tier unlock |
| Final Boss | — | — | Oyun sonu (3 kapanış seçeneği) |

---

## 13. Tam Run Akışı (Referans)

```
HUB → Primary class seç
  ↓
ACT 1 (8-9 oda)
  Her combat/elite → Harita Parçası → Skill Draft
  Her 3 combat oda → Echo Imprint sunusu (1/3 odada)
  Slot dolunca → Tier upgrade ağırlığı artar
  Tag Sinerji Bonusu → draft sonrası otomatik hesaplanır
  Boss (Act 1 Warden varyantı)
    → Secondary class seçim ekranı (2 random → 1 seç)
    → +2 aktif slot açılır
    → Cross-class Pasif aktive olur
  ↓
ACT 2 (9-11 oda)
  Draft: primary+secondary+neutral karışık
  Spirit Encounter eklenir (1 tane)
  Shop eklenir (1 tane)
  Boss (Fractured King) — oynanış bazlı faz geçişi
    → Cross-class Ultimate açılır
  ↓
ACT 3 (9-11 oda)
  Draft: %45/%45/%10
  Legendary tier mevcut (nadir)
  Spirit Encounter (1-2 tane)
  Curse Gate (1 tane) — Faz 4+
  Boss (Hollow Sovereign) — build-adaptive
  ↓
FINAL (5-6 oda)
  Mixed draft
  1 özel Spirit veya Curse Gate garantili
  Final Boss (Nexus Core) — çok faz, build-aware
    Faz 1: primary class signature mechanic
    Faz 2: cross-class pasif zayıflığını hedef alır
    Faz 3 (Legendary varsa): Legendary skill yansıması
  ↓
SONUÇ
  Ölüm → istatistik → Echoes toplanır → Hub
  Zafer → 3 kapanış seçeneği (Kal / Kır / Taşı)
```

