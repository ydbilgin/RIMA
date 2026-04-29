# GÜÇLENME SİSTEMİ — TAM KAPSAMLI
*2026-03-27 | Oyuncunun run boyunca nasıl, nerede, ne şekilde güçlendiğinin tam haritası*

---

## ÖZET — KAÇ YOLDAN GÜÇLENİYORSUN?

```
A. ODA SONU ÖDÜLÜ      → Skill ekle / upgrade et / pasif al (her oda)
B. SHOP                → Soul Dust harca, augment/item al (nadir oda)
C. FLUX (Reforge)      → Skill/pasif değiştir (run başına max 1, %5-8)
D. BOSS SOUL           → Bir skill kalıcı mutasyona uğrar (her boss sonrası)
E. RAGE (SÜTUN 4)      → Evrensel güç patlaması [V], run boyunca birikir
F. META PROGRESSION    → Hub'da kalıcı unlock (run sonrası)
```

Her sistem farklı bir ritimde çalışıyor, farklı bir his veriyor. Bunların beraber çalışması tasarımın özü.

---

## A. ODA SONU ÖDÜLÜ — "Hades Boon" Sistemi

### Nasıl Çalışır?

Her oda temizlenince ekran açılır. **3 seçenek** sunulur. 1'ini al, diğerleri kaybolur.

```
[ODA SONU]
  ↓
3 seçenek:
  [SKİLL EKLE]   → Aktif slot'larına yeni bir skill ekle (max 6 slot)
  [SKİLL UPGRADE]→ Mevcut bir skill'i güçlendir (1 sınırlı upgrade/skill)
  [PASİF EKLE]   → Pasif slotuna yeni bir pasif ekle (max 2 pasif + bonus)
  [AUGMENT EKLE] → Pasif olmayan küçük bonus ekle (FAZ 3+)
  ↓
Seçim kalıcı (run sonuna kadar)
```

### Seçenek Havuzu — Ne Çıkabilir?

```
Aktif sınıfların havuzundan:
  → 1. sınıf (örn. Warblade) skilleri — ağırlık %40
  → 2. sınıf (örn. Elementalist) skilleri — ağırlık %40
  → Neutral havuz — ağırlık %20

Upgrade seçeneği:
  → Sadece zaten elimde olanlar upgrade olabilir
  → Aynı skill iki kez upgrade edilemez
```

### Skill Upgrade Ne Yapar?

Her skill'in bir upgrade versiyonu var. Upgrade genellikle şunlardan birini yapar:
- Hasar/etki +%20-30
- CD azalır (-2 ile -4 saniye)
- Proc koşulunu kolaylaştırır
- Ekstra efekt ekler (ör: "Artık 2 hedefi etkiliyor")

Upgrade seçenekleri SINIF_SKILL_HAVUZU.md'de her skill için tanımlanacak.

### Slay the Spire Farkı

StS'de deck'e ekleme zorunlu (Skip var ama taktiksel). Bizde:
- **Skip yok** — 3 seçenekten birini almak zorundayken max 6 slot doluysa...
- Max 6 slot doluysa oda sonu otomatik UPGRADE moduna geçer (yeni skill eklemek yerine)
- Bu kararı oyuncuya bırakmak mı, otomatik mi? → **Otomatik upgrade modu** (FAZ 2'de kararlaştır)

---

## B. SHOP ODASI — "Bazaar / Market"

### Nasıl Çıkar?

Oda rotasyonunda ~%10-15 olasılıkla. Hades'teki Well of Charon benzeri.

### Ne Satar?

```
[SHOP]

  AUGMENT BÖLÜMÜ (Soul Dust ile):
    • Küçük stat bonus (max HP +15, hasar +8% vb.)
    • Sınıf özel augment (Warblade için özel pasif benzeri küçük bonus)
    • Neutral augment

  ITEM BÖLÜMÜ (Soul Dust ile):
    • Run'a özgü item — aktif efektli veya pasif
    • "Trinket" tipi: belirli koşulda tetiklenen tek seferlik efekt

  SERVİS BÖLÜMÜ (Soul Dust ile):
    • Skill upgrade (bedelli — oda ödülüyle aynı ama burada istediğin anda)
    • Skill silme (FAZ 3+ — max 6 slot doluyken gereksiz skill'den kurtul)
```

### Soul Dust Kaynakları

Soul Dust run boyunca birikir:
- Elite öldürünce: 5-10 Soul Dust
- Boss öldürünce: 20-30 Soul Dust
- Gizli oda: 15 Soul Dust
- Meta upgrade ile başlangıç Soul Dust bonusu

---

## C. FLUX ODASI — "Reforge / Build Pivot"

**→ Detaylı doküman: `REFORGE_SISTEM.md`**

Özet:
- %5-8 olasılık, run başına max 1 kez
- Skill / pasif / augment **değiştir** (ekle değil)
- 3 seçenek: aktif sınıf 1 / aktif sınıf 2 / Neutral
- Önce ne alacağını gör, sonra ne kaldıracağını seç
- Gelen skill/pasif upgrade level 0 gelir (soft cost)
- Corrupted varyant: daha güçlü + downside (%10-30, aşamaya göre)
- Grudge Tooltip: eğer değiştirilecek skill aktif bir Grudge tetikliyorsa uyarı

---

## D. BOSS SOUL SİSTEMİ — "Skill Mutasyonu"

### Akış

```
Boss öldürüldü
  ↓
"Soul Drop" animasyonu (Aria of Sorrow tarzı)
  ↓
Oyuncu hangi skill slotunu dönüştürmek istediğini seçer
  ↓
3 mutasyon seçeneği sunulur:
  [HASAR]  → Bu skill hasar odaklı mutasyona uğrar
  [CC]     → Kontrol/utility odaklı mutasyon
  [PROC]   → Zincir/sinerji odaklı mutasyon
  ↓
Seçilen mutasyon kalıcı (bu run için)
Skill görsel olarak değişir (renk, ikon efekti)
  ↓
Run sonunda Boss Soul → Soul Dust'a dönüşür (Meta Progression için)
```

### Mutasyon Örnekleri

**Warblade: Ground Stomp + Frost Boss Soul**

| Mutasyon | Yeni İsim | Yeni Mekanik |
|----------|-----------|--------------|
| HASAR | Glacial Stomp | Stun + 2s freeze + %150 hasar |
| CC | Cryogenic Shatter | Stun 3s + alan slow %40, hasar orta |
| PROC | Frost Momentum | Stun sonrası tüm CD'ler -3s, küçük slow |

**Elementalist: Fireball + Shadow Boss Soul**

| Mutasyon | Yeni İsim | Yeni Mekanik |
|----------|-----------|--------------|
| HASAR | Void Bolt | %300 hasar, sinir tetikleme |
| CC | Shadowfire | Yanık + körleştirme (2s) |
| PROC | Phantom Flame | Her Fireball → 1s stealth girme şansı |

### Boss Soul Augment Slot

Oyuncu aynı anda **2 Boss Soul** taşıyabilir.
2 slotu doluyken yeni boss öldürülünce: hangisini değiştireceğini seç.

---

## E. RAGE BARINI (SÜTUN 4) — Evrensel Güç Döngüsü

### Temel Mekanik

```
Rage Barı: 0-100 (evrensel, tüm sınıflarda var)
  Dolar: Hasar verince +5/vuruş, hasar alınca +10
  Boşalır: Boşta -5/sn
  [V] tuşu: Rage boşaltır → sınıfa özel güç patlaması (6-8s)
```

### Rage Ultimate Örnekleri (sınıf başına)

| Sınıf | Rage Ultimate | Süre |
|-------|---------------|------|
| Warblade | Bladestorm — spin CC immune, sürekli AoE | 6s |
| Elementalist | Inferno — arena-wide hasar, sürekli Fireball | 7s |
| Rogue | Shadow Dance — stealth CD 0, her vuruş crit | 6s |
| Ranger | Rain of Arrows — tüm alanı yağmurla | 6s |
| Brawler | Chi Surge — hasar ×2, knockback her vuruşta | 7s |
| Paladin | Avenging Wrath — %30 invuln + knockback aura | 8s |
| Summoner | Army of the Dead — tüm minyonlar +%100 hasar | 7s |
| Hexer | Hex Cascade — tüm hex stack'ler anında Hexblast | 6s |

### Identity Transform (Meta Unlock)

~7. run sonrası açılır. Rage barı dolunca [V] yerine [V] uzun bas → **Identity Transform:**
- 10s süre
- Tamamen farklı 4 skill seti açılır (sınıfın "transformed" hali)
- Rage transform sırasında dolmaz
- Bitince orijinal kit geri döner

---

## F. META PROGRESSION — Hub Unlock'ları

### Ne Zaman?

Run sonu (ölüm veya zafer) → Hub'a dön → Soul Dust harca.

### Unlock Ağacı

```
TEMEL UNLOCK'LAR (ilk 3 run):
  → Stance Modu unlock (Run 3 sonrası)
  → Yeni sınıf aç (Hexer, Summoner — başlangıçta kilitli)
  → Başlangıç Soul Dust +10

ORTA UNLOCK'LAR (run 4-10):
  → Pasif slot +1 (2 → 3)
  → Identity Transform unlock (~Run 7)
  → Flux "Chaos" modu (3. sınıftan skill alınabilen nadir Flux varyantı)
  → Boss Soul 3. slot (başlangıç 2, bu ile 3)

GEÇ UNLOCK'LAR (run 10+):
  → Yeni sınıflar (Phantom, Blade Dancer, Runecaster)
  → Başlangıç skill seçimi (1. oda atlıp direkt skill seçimi)
  → Grudge hafıza sistemi (düşman Grudge'ları run'lar arasında kısmen taşınır)
```

---

## G. TÜM SİSTEMLERİN ZAMAN ÇİZELGESİ — Tipik Bir Run

```
RUN BAŞLANGICI
  │ Sınıf seçimi (2 sınıf + Fusion/Stance)
  │ Başlangıç skili (meta unlock varsa)
  │
  ODA 1 (Normal)
  │ → A: Oda sonu ödülü — skill ekle (havuz dar, erken aşama)
  │
  ODA 2 (Normal)
  │ → A: Oda sonu ödülü — pasif ekle
  │
  ODA 3 (Elite)
  │ → A: Oda sonu ödülü — skill upgrade veya yeni skill
  │ → E: Elite öldürdüm, Soul Dust +8
  │ → [%5-8 şans] C: FLUX odası gelebilir (run başına 1 hak)
  │
  ODA 4 (Shop)
  │ → B: Soul Dust harcayabilirsin
  │
  ODA 5 (Normal)
  │ → A: Oda sonu ödülü
  │
  ...
  │
  ODA N (Boss)
  │ → D: Boss Soul — skill mutasyonu seç
  │ → Soul Dust +25
  │
  BÖLÜM 2 BAŞLANGICI...
  ...
  │
  BOSS 2 (Son Boss)
  │ → D: Boss Soul (2. slot)
  │ → Meta Progression Soul Dust birikti
  │
RUN SONU
  │ → F: Hub'a dön, Soul Dust harca
```

---

## H. SİSTEMLER ARASI ETKİLEŞİM — "Bu Build İnsane" Anı Nasıl Oluşur?

```
ODA ÖDÜLÜ (A) → Build yönü belirleniyor (erken seçimler)
  ↓
BOSS SOUL (D) → Belirlenen yön mutasyona uğruyor (ilk güç sıçraması)
  ↓
FLUX (C) → Run ortasında yön netleşiyor veya tamamen pivot
  ↓
SHOP (B) → Boşluklar augmentle dolduruluyor
  ↓
RAGE (E) → Her rotasyon döngüsünde [V] patlaması
  ↓
PROC ZİNCİRLERİ + COMBO MULTIPLIER (Sütun 2)
  → Tüm sistemler senkrona girdi → "Bu build insane" anı
```

---

## İ. TASARIM PRENSİPLERİ — Neden Bu Yapı?

| Prensip | Nasıl Uygulandı |
|---------|-----------------|
| Her run farklı hissettir | A+C kombinasyonu farklı sonuçlar üretiyor |
| Güç eğrisi tutarlı | Boss Soul'lar run ortasında belirgin sıçrama yaratıyor |
| Oyuncu ajansı yüksek | C'de önce ne alacağını gör, sonra ne vereceğini seç |
| Scope kontrolü | Meta sistemler (F) kademeli — FAZ 5'e kadar basit tut |
| "Nadir = Anlamlı" | Flux max 1/run, Boss Soul her boss'ta — ikisi de ödüllendirici |

---

## ÖZET TABLO — Güçlenme Kaynakları

| Sistem | Sıklık | Tip | Hissi | Faz |
|--------|--------|-----|-------|-----|
| A: Oda ödülü | Her oda | Ekle + Upgrade | "Büyüyorum" | FAZ 1 |
| B: Shop | ~%10-15 oda | Satın al | "Optimize ediyorum" | FAZ 2 |
| C: Flux | Max 1/run | Değiştir | "Pivot" | FAZ 2 |
| D: Boss Soul | Her boss | Mutasyona uğrat | "Dönüştüm" | FAZ 3 |
| E: Rage | Her run | Patlama | "Şimdi güçlüyüm" | FAZ 1 |
| F: Meta | Run sonu | Kalıcı unlock | "Run'lar arası büyüme" | FAZ 5 |
