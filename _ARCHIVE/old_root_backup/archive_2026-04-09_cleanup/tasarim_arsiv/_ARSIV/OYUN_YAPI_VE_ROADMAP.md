# Oyun Yapısı ve Yol Haritası
*Son güncelleme: 2026-03-29 | Tüm kararlar onaylandı*

---

## OYUNUN MERKEZ CÜMLESİ

> "Run tek bir class kimliğiyle başlar, ilk boss sonrası ikinci class ile kırılır,
> build oda oda delice bir şeye dönüşür."

---

## CORE LOOP — RUN AKIŞI

```
[HUB — The Threshold]
        ↓
  Primary Class seç (1 class)
        ↓
╔══════════════════╗
║     ACT 1        ║  6-7 oda + 1 Boss
║  Sadece Primary  ║  Class kimliği öğretilir
╚══════════════════╝
        ↓
  ACT 1 BOSS ÖLÜR
        ↓
  2 random Secondary Class kartı çıkar
  Oyuncu 1'ini seçer
  → 2 yeni aktif slot açılır
  → Cross-class Pasif aktif olur
  → Secondary skill havuzu draft'a girer
        ↓
╔══════════════════╗
║     ACT 2        ║  8-9 oda + 1 Boss
║  Karışık draft   ║  60% primary / 25% secondary / 15% nötr
╚══════════════════╝
        ↓
  ACT 2 BOSS ÖLÜR
  → Cross-class Ultimate kilidi açılır
  → Draft oranı: 45% primary / 45% secondary / 10% nötr
        ↓
╔══════════════════╗
║     ACT 3        ║  10-11 oda + 1 Boss
║  Tam dual-class  ║  Legendary/Heroic şansı
╚══════════════════╝
        ↓
  ACT 3 BOSS ÖLÜR → Final Boss kilidi
        ↓
╔══════════════════╗
║   FINAL BOSS     ║  5 küçük oda + Boss
║  The Nexus Core  ║  Oyunun sonu
╚══════════════════╝
        ↓
  Ölüm → HUB'a dön, meta-progression
  Zafer → Hikaye sonu + yeni run seçeneği
```

---

## ACT YAPISI

| Act | Oda Sayısı | Boss | Ödül |
|-----|-----------|------|------|
| Act 1 | 6-7 + boss | Confluence Guardian | Secondary class seçimi |
| Act 2 | 8-9 + boss | The Fractured King | Cross-class Ultimate |
| Act 3 | 10-11 + boss | The Hollow Sovereign | Final Boss kilidi |
| Final | 5 + boss | The Nexus Core | Kaçış / hikaye sonu |

---

## ODA TİPLERİ

### Oda Sembolleri

| İkon | Tip | Açıklama |
|------|-----|----------|
| ⚔️ | **Combat** | Standart düşman odası. Her oda temizlenince skill offer çıkar. |
| 💀 | **Elite** | Daha zor düşmanlar. Garanti Rare+ ödül + küçük HP yenilemesi. |
| 👁️ | **Spirit Encounter** | Bir ruh varlık belirir, 2-3 build bonusu sunar. Koşullu offer. |
| 🛒 | **Shop** | Altınla HP, max HP, skill tier atlatma, gear al. |
| 🌀 | **Curse Gate** | HP harca → güçlü build bonusu. Risk / ödül. |
| 🎲 | **Event** | Seçim / hikaye odası. İki seçenek, ikisi de sonuç üretir. |
| ❓ | **Unknown** | Kapıda hiçbir ikon yok. İçerde ne çıkacağı belli değil. |

### Unknown Oda İçerik Tablosu (Ağırlıklı)
| İçerik | Ağırlık |
|--------|---------|
| Combat (normal zorluk) | %25 |
| Mini-boss (garanti iyi ödül) | %20 |
| Gizli shop (%60 fiyat) | %15 |
| Bonus Spirit Encounter | %15 |
| Tuzak odası (zor ama büyük ödül) | %10 |
| Sessiz oda: sadece +Max HP | %10 |
| Minor reward (küçük ödül, hiç düşman yok) | %5 |

> ❌ Tamamen boş oda yok — "oyun beni trollüyor" hissi engellenir.

### Oda Akışı — Her Oda Temizlendikten Sonra

```
Oda temizlenir
     ↓
Harita parçası düşer (her odada, zorunlu)
     ↓
Oyuncu alır → 1 saniyelik "ritual" animasyon
(parça yerine oturur, harita o bölge açılır)
     ↓
Skill offer / oda ödülü sunar
     ↓
Oyuncu 2-3 kapı seçeneği görür (sonraki oda tipleri)
     ↓
Kapı seçilir → ilerlenir
```

---

## YIRTIK HARİTA SİSTEMİ

### Nasıl Görünür
- Köşede eski kumaş/parşömen dokulu harita
- Keşfedilmemiş bölgeler karanlık/yırtık
- Harita parçası alınınca o bölge mürekkep/karakalem çizgilerle belirir
- Oda tipleri küçük ikonlarla gösterilir

### Nasıl Çalışır
- Her odada 1 harita parçası zorunlu düşer
- Parça alınmadan kapı açılmaz (Hades mantığı)
- Parça alınca → anlık animasyon (1s), haritada sonraki 3-4 oda görünür
- Bulmaca YOK — sadece "al ve yerleştir" etkileşimi

### Unity Implementation
```
MapPiece → ScriptableObject (int roomID, Vector2 position)
MapReveal → fog-of-war texture mask açma (Sprite Mask)
Animasyon → DOTween ile parçanın yerine "snap" olması
Kapı lock → bool flag, parça alınınca false
```

---

## SKILL DRAFT SİSTEMİ

### Offer Mantığı
- Her oda temizlenince 3 kart sunulur
- **Garantili 1 kart:** mevcut equipped skill'lerden birini tier atlat
- **Diğer 2 kart:** yeni skill önerisi veya başka bir skill tier atlaması
- Tüm skillerin Legendary olduğunda: 3 yeni skill kartı

### Act Bazlı Draft Oranı

| Act | Primary | Secondary | Nötr |
|-----|---------|-----------|------|
| Act 1 | %100 | — | — |
| Act 2 başı | %60 | %25 | %15 |
| Act 2 sonu | %50 | %35 | %15 |
| Act 3 | %45 | %45 | %10 |

> Nötr = cross-class köprü skill'leri (her iki class'a da yarar)

### Skill Tier Sistemi

| Tier | Renk | Efekt |
|------|------|-------|
| Common | ⬜ Beyaz | Temel efekt, temel sayılar |
| Rare | 🟦 Mavi | +%30 sayılar + küçük yeni mekanik |
| Epic | 🟣 Mor | +%60 sayılar + anlamlı yeni mekanik |
| Legendary | 🟡 Altın | Skill'in nasıl çalıştığı köklü değişir |

### Tier Dağılımı (Oda Sırasına Göre)

| Oda | Tier Pool |
|-----|-----------|
| 1-5 | Common only |
| 6-10 | Common %70 / Rare %30 |
| Act 1 boss sonrası | Rare %60 / Epic %40 |
| Act 2 boss sonrası | Epic %50 / Legendary %50 |

**Örnek — Iron Charge (Warblade):**
| Tier | Efekt |
|------|-------|
| Common | 8m dash + 1.5s stun, Rage +20 |
| Rare | 10m dash + 1.8s stun, Rage +28, CD -1s |
| Epic | 12m dash + 2s stun, düşmanları iten alan, Rage +35 |
| Legendary | Dash sırasında i-frame, stun AoE'ye döner (3m), hit'lenen tüm düşmanlar merkeze çekilir |

---

## SPIRIT ENCOUNTER SİSTEMİ

### 3 Ana Ruh Tipi (Demo için)

#### 🔥 Forge Wraith — "Saldırı ve Kırma"
Hasar, zırh kırma, melee baskı temalı bonuslar sunar.
- Koşul yok — her zaman 2-3 seçenek sunar
- Örnek offer'lar: Rage/Fury/Momentum boost, zırh kırma efekti, saldırı hızı

#### 🐾 Shadow Hound — "Hareket ve Konumlama"
Hız, kaçış, alan kontrolü temalı bonuslar.
- Koşul: Son odayı 5s'den hızlı temizlediysen → ekstra seçenek görünür
- Örnek offer'lar: dash CD düşür, skill sonrası hız patlaması, slow alanı efekti

#### 🩸 Blood Oracle — "HP Trade ve Sustain"
HP harcama, lifesteal, max HP temalı bonuslar.
- Koşul: HP %60'ın altındaysa → en güçlü seçenek görünür
- Örnek offer'lar: lifesteal, HP harcayarak hasar artır, max HP kalıcı artış

### Sonraki Fazlarda Eklenecek Ruhlar
- **Void Seer**: 3 seçenek ama ikonlar kapalı — körlemesine seçersin
- **Fallen Champion**: Kombo ve chain bonusları
- **Ancient Relic**: Güçlü ama her offer'a curse eşlik eder

---

## HP VE SHOP EKONOMİSİ

### HP Kaynakları (Dağıtık)
| Kaynak | Miktar |
|--------|--------|
| Elite oda tamamlama | Max HP'nin %15'i |
| Shop — Essence Flask | +25 HP (1-2 adet stok) |
| Shop — Vitality Shard | +10 Max HP kalıcı |
| Shop — Hardened Core | +20 Max HP + 1 absorb kalkan |
| Combat drop (nadir) | +5-10 HP |
| Event seçimleri | Değişken |
| Curse Gate risk ödülü | Değişken |

### Shop İçeriği (Dönen Stok)
Her shop'ta 3-5 item rastgele belirir:
- Healing items (sınırlı stok)
- Max HP upgrades
- Skill tier atlatma taşı
- Altınla doğrudan skill al
- Run'a özel gear (pasif efektler)

---

## CROSS-CLASS SİSTEMİ

| An | Olay |
|----|------|
| Act 1 Boss kill | 2 random secondary class kartı → 1 seç |
| Seçim anında | +2 aktif slot açılır, cross-class pasif aktif |
| Act 2 boyunca | Karışık draft, secondary skill havuzu girer |
| Act 2 Boss kill | Cross-class Ultimate kilidi açılır |
| Act 3 | Legendary/Heroic şansı başlar |

> Cross-class Ultimate → Act 2 boss ödülü, Demo dışı.

---

## META-PROGRESSION (Runlar Arası)

Hub'da (The Threshold) yapılır:
- **Shattered Echoes** (run içi toplanan paralar) → kalıcı stat artışları
  - Başlangıç HP artışı
  - Starting gold artışı
  - Daha yüksek rarity ile başlama şansı
  - İlk offer'da Signature skill garantisi
- **Unlock Pool** → yeni skill'ler, ruh tipleri, event'ler havuza eklenir
- Hub karakterleriyle ilişki → özel bonuslar

---

## DEMO FAZ 1 — KAPSAM

```
Classes:    Warblade, Elementalist, Shadowblade, Ranger
Act:        Sadece Act 1 (6-7 oda + boss)
Skill tier: Common / Rare
Draft:      Sadece Primary class
Cross-class: Boss sonrası 2 seçenek, Pasif açılır (Ultimate yok)
Oda tipleri: Combat + Elite + Shop + Spirit (3 tip) + Unknown
Map:        Yırtık harita, 1s reveal animasyonu
Meta:       Temel (sadece Echoes toplanır, henüz harcanmaz)
```

## FAZ 2 — FULL DEMO

```
+ Act 2 eklenir
+ Epic tier
+ Cross-class Ultimate (Act 2 boss)
+ Event odaları
+ Curse Gate odaları
+ Void Seer + Fallen Champion ruh tipleri
+ Temel meta-progression (Echoes harcanabilir)
```

## FAZ 3 — EARLY ACCESS

```
+ Tüm 8 class
+ Act 3 + Final Boss
+ Legendary tier + Heroic skills
+ Tüm cross-class kombinasyonları (28)
+ Ancient Relic ruh tipi
+ Tam meta-progression
+ Difficulty (Heat muadili)
```

## FAZ 4 — 1.0 RELEASE

```
+ Gizli odalar / alternatif rotalar
+ Alternate boss fazları
+ Tam hikaye/diyalog
+ Balance ve polish
+ Achievements
```
