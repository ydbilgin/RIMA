# REFORGE / BUILD PIVOT SİSTEMİ — FLUX
*2026-03-27 | Ollama (8 bölüm) + Web (31 kaynak) + Tasarım sentezi*

**Dosya içeriği:**
1. Özet + isim
2. Temel mekanik
3. Neutral Skill + Pasif tam liste
4. Flux tetikleme mantığı — sıklık, aşama, havuz
5. Grudge Tooltip — ne, neden, nasıl
6. Ekstra fikirler (Grudge Token, Rage Yakma vb.)
7. Uygulama önceliği
8. UI notları
9. Web araştırması bulguları

---

## 1. ÖZET — NE YAPIYORUZ?

Run ortasında build yönünü değiştirme imkânı.
Oyuncu oda sonrası bazen "büyüt" yerine "değiştir" seçeneğiyle karşılaşıyor.

**İsim seçenekleri:** Reforge / Drift / Flux / Fracture / Pivot
→ **Önerim:** **FLUX** — hem "akış değişimi" hem de "belirsizlik/esneklik" çağrışımı. Oyunun MMORPG tonuyla uyumlu.

---

## 2. TEMEL MEKANİK (Senin Tanımın)

Oda sonrası **%5-8** olasılıkla, 3 yükseltme seçeneği yerine Flux ekranı çıkar:

```
[ FLUX ]

  [SKİLL DEĞİŞİMİ]
    Elindeki bir skill →
      • Aynı sınıfın başka bir skilli
      • Cross-class skill (3. sınıftan)
      • Neutral Skill (sınıfsız, evrensel)

  [PASİF DEĞİŞİMİ]
    Aktif pasifin →
      • Aynı sınıfın başka pasifi
      • Cross-class pasif
      • Neutral Pasif

  [AUGMENT DEĞİŞİMİ]
    Elindeki bir augment/item →
      • Seçilen seçeneklerden biri (Hades reroll tarzı)
```

### Araştırmadan Gelen Kararlar

| Soru | Karar | Neden |
|------|-------|-------|
| Blind mi, preview mi? | **Preview** — ne alacağını göster | Regret minimizer. TFT blind = frustrasyona dönüşüyor |
| Ücretsiz mi? | **Evet, ücretsiz** | Zaten nadir geliyor. Kaynak eklersen daha da nadir kullanılır |
| Sıklık? | **%5-8** | Yeterince nadir = önemli hissettiriyor, çok nadir değil = varlığı fark edilmiyor |
| Ne zaman çıkabilir? | **Run ortasında** (ilk 2 odadan sonra) | Erken gelirse build daha oluşmadı = anlamsız |
| Cross-class limit? | **Max 1 cross-class skill per run** | Dual class seçimini anlamsızlaştırmaz |
| Cross-class maliyeti? | **Ücretsiz ama cap var** | Soul Dust maliyet ekleme → karmaşıklaştırıyor, solo dev kapsamı aşar |
| Görsel ayırt edici? | **Evet** — cross-class skill farklı frame/renk | Oyuncu hemen anlamalı: "Bu benim sınıfımdan değil" |

---

## 3. NEUTRAL SKİLL/PASİF HAVUZU — TAM LİSTE

### Tasarım Kuralları

1. **Sınıf kaynağına dokunmaz** — Mana, Energy, Holy Power, Fury, Chi, Hex Stacks, Charges'a etkileşmez
2. **Rage ile etkileşebilir** — Rage evrensel sütun (Sütun 4), tüm sınıflarda var
3. **Proc koşulu herhangi bir sınıfın tetikleyebileceği bir şey olmalı** — "Stun", "kill", "dash", "hasar al" hepsi evrensel
4. **Güçlü ama sinerji tanımlayıcı değil** — Build yönünü değiştirmez, boşlukları doldurur
5. **Sayı:** ~8 aktif + ~8 pasif başlangıç. Oyun büyüdükçe genişler.

---

### Neutral Aktif Skilleri (8 adet)

| # | İsim | Tip | Mekanik | CD | Grudge İlişkisi |
|---|------|-----|---------|-----|-----------------|
| N1 | **Surge Step** | Mobility | 5m dash. Sonraki 1.5s içinde skill kullanılırsa +%30 hasar | 10s | — |
| N2 | **Fracture Point** | Hasar | Hareketsiz hedefe (stun/root/knockdown) %200 hasar | 8s | ⚠ Stun ile öldürme Grudge tetikler |
| N3 | **Vital Strike** | Hasar | Hedef max HP'sinin %5'i flat hasar — zırh/direnç tanımaz | 12s | — |
| N4 | **Chain Reaction** | AoE | Bir düşman öldüğünde 2.5m çevresine patlama | 6s | ⚠ AoE ölüm Grudge tetikler |
| N5 | **Recoil** | CC+Survival | Gelen saldırıya zamanlamalı basınca: 3m knockback + 0.5s invuln | 12s | — |
| N6 | **Adrenaline Rush** | Sustain | 5s süre: her hit +3 HP lifesteal | 15s | — |
| N7 | **Last Stand** | Survival | HP %15 altına düşünce otomatik: 2s invuln + Rage +30. Run başına 1 kez | (auto) | — |
| N8 | **Earthshatter** | AoE+CC | Yere çak: 3m çevresindeki düşmanlara 1s slow + hafif knockback | 14s | ⚠ AoE ile ölüm Grudge tetikler |

---

### Neutral Pasifler (8 adet)

| # | İsim | Tip | Mekanik | Grudge İlişkisi |
|---|------|-----|---------|-----------------|
| P1 | **Evasion** | Aura | %10 dodge şansı. Dodge olunca Rage +5 | — |
| P2 | **Bolstered Resolve** | Reaktif | Son 3s hasar almadıysan gelen hasar %20 azalır | — |
| P3 | **Predator's Mark** | Proc | Hareketsiz düşmana vur → sonraki skill CD -2s | ⚠ Stun ile öldürme Grudge tetikler |
| P4 | **Bloodstained Path** | Aura | Her kill → Rage +5 (evrensel, Sütun 4) | — |
| P5 | **Momentum** | Proc | Dash sonrası 2s içinde ilk skill +%25 hasar | — |
| P6 | **Glass Cannon** | Aura | Hasar +%12, max HP -10 | — |
| P7 | **Iron Skin** | Reaktif | HP %50 altında iken gelen hasar %15 azalır | — |
| P8 | **Hunter's Focus** | Proc | Aynı düşmana üst üste 3 vuruş → 4. vuruş guaranteed crit | — |

---

### Neutral vs. Sınıf Skili — Nasıl Ayrışıyor?

| Özellik | Sınıf Skili | Neutral Skili |
|---------|-------------|---------------|
| Kaynak ile etkileşim | ✅ Evet (Mana, Energy vb.) | ❌ Hayır |
| Proc koşulu | Sınıfa özgü (5 combo point vb.) | Evrensel (stun, dash, kill) |
| Build yönünü tanımlar | ✅ Evet | ❌ Hayır — destekler |
| Sınıf kimliği hissi | Güçlü | Nötr/generik |
| Normal ödüllerde | Ana havuz | Düşük ağırlık |
| Flux'ta görünüm | 2 seçenekten biri | Her Flux'ta 1 Neutral garantili |

---

## 4. FLUX TETİKLEME MANTIĞI — SAYI, AŞAMA, HAVUZ

### Ne Zaman Çıkabilir?

```
Run başlangıcı: Flux çıkamaz (ilk 2 oda)
Normal run: Her oda sonrası %5-8 olasılık
Boss öncesi son oda: Flux çıkamaz
Hard cap: Run başına maksimum 1 Flux odası
```

**Neden max 1?**
TFT augment sistemi run başına 3 kez veriyor ve build inflection point olarak tasarlanmış. Bizde Flux "nadir windfall" olarak kodlanmalı, "beklenen araç" değil. Max 1 = her görünümü önemli.

---

### Flux İçindeki Seçim Akışı

```
[FLUX ODASI]
  ↓
Oyuncu önce SWAP TİPİNİ seçer:
  [A] SKİLL SWAP
  [B] PASİF SWAP
  [C] AUGMENT SWAP  ← FAZ 3+, başta yok
  ↓
3 seçenek sunulur (hangisini ALACAĞINI gösterdir)

  Seçenek 1: Aktif 1. sınıftan bir skill/pasif   [normal çerçeve]
  Seçenek 2: Aktif 2. sınıftan bir skill/pasif   [normal çerçeve]
  Seçenek 3: Neutral havuzdan                    [gümüş çerçeve]
  (+ Bazen): Corrupted versiyon                  [kırmızı çerçeve]
  ↓
Oyuncu hangi seçeneği ALMAK istediğine karar verir
  ↓
Sonra mevcut slotlarından hangisini KALDIRECEĞINI seçer
  (önce ne alacağını bil, sonra ne vereceğine karar ver — StS campfire mantığı)
  ↓
Kaldırılan skill silinir. Gelen skill UPGRADE LEVEL 0 gelir (soft cost)
```

---

### Aşama Bazlı Havuz Kuralları

| Aşama | Odalar | Havuz Özellikleri | Corrupted Şansı |
|-------|--------|-------------------|-----------------|
| **Erken** | 3–6 | Temel+utility skill öne çıkar, tanıdık seçenekler | %10 |
| **Orta** | 7–14 | Tüm skill havuzu açık, Grudge tooltip aktif | %20 |
| **Geç** | 15+ | En güçlü skill varyantları ağırlıklı | %30 |

**Erken havuz kısıtı neden var?**
Oyuncu build'i daha oluşmamışken güçlü/niche bir skill görürse değerini bilemez. Slay the Spire'da yüksek rarity kartlar erken değil geç önerilir — aynı mantık.

---

### Skill Havuzu Ağırlık Tablosu (SKILL SWAP örneği)

Aktif 1. sınıf = Warblade, Aktif 2. sınıf = Elementalist, Aşama = Orta:

```
Seçenek 1 havuzu (Warblade):
  → Warblade skill havuzundan zaten ALMADIKLAINI filtrele
  → Erken değil geç utility seçenekleri (Charge, Execute, War Stomp)
  → Ağırlık: eşit

Seçenek 2 havuzu (Elementalist):
  → Elementalist skill havuzundan zaten almadıkları
  → Aynı filtre

Seçenek 3 havuzu (Neutral):
  → 8 Neutral aktiften 1 tanesi
  → Her zaman garantili 1 Neutral seçenek var

Corrupted (Orta aşama %20 şans):
  → Seçenek 1, 2 veya 3'ten biri Corrupted versiyona dönüşebilir
  → 3 seçenekten maksimum 1 tanesi Corrupted olabilir
```

---

## 5. GRUDGE TOOLTIP — NE, NEDEN, NASIL

### Grudge Sistemi Hatırlatma

Grudge sistemi: Elite düşmanlar nasıl öldürüldüklerini hatırlıyor → o yönteme direnç kazanıyor.

Örnek:
```
Elementalist oynarken → sürekli Fireball ile öldürüyorsun
→ Elite "Molten Grunt" Ateş Grudge kazandı (Grudge Badge: 🔥)
→ Bir sonraki spawn'da bu düşman Ateş hasarına +%40 direnç taşıyor
```

---

### Grudge Tooltip Nedir?

Flux ekranında, swap etmeyi düşündüğün skill veya pasif şu anda **aktif bir Grudge'u tetikleyen** kaynak ise, o seçeneğin yanında küçük bir uyarı ikonu çıkar.

**Görsel:** Skill kartının köşesinde sarı ⚠ simgesi. Üzerine gelinince:

```
⚠ Bu skill [Molten Grunt]'ın Ateş Grudge'unu tetikliyor.
   Değiştirirsen bu düşmanın Ateş direnci takibi sıfırlanır.
   → Bir sonraki görüşmede bu düşman Ateş'e yeniden savunmasız olur.
```

---

### Ne İşe Yarıyor?

**Bilgi olarak:** Oyuncu "bu düşman bana Grudge uygulamış" gerçeğini Flux sırasında görüyor. Farkında olmayabilir.

**Strateji olarak:** İki farklı kullanım:

```
Kullanım A — Grudge'u kırmak için swap:
  Sürekli Fireball kullanıyorsun → Elite Ateş Grudge kazandı
  Skili değiştir → Grudge sıfırlanır → o düşman tekrar açık

Kullanım B — Grudge'u kasıtlı korumak:
  "Ben bu düşmanı zaten Buz hasarına programlamak istiyorum"
  Tooltip görünse bile Fireball'ı değiştirmiyorsun
  → Elite Ateş direncini korur, ama sen Buz'a geçiş yapıyorsun
  → İki farklı Grudge baskı → düşman hangisine adapte olacak?
```

**Uzun oyun:** Grudge Tooltip, Flux'u Grudge sistemiyle organik bağlıyor. "Build değiştir" eylemi aynı zamanda "düşman manipülasyonu" oluyor.

---

### Hangi Skillerde Görünür?

Neutral listemizden ⚠ işaretli olanlar Grudge tetikleyebilir. Sınıf skilleri için:

| Eleman/Yöntem | Grudge Örneği | Tooltip Tetikleyen Sınıf Skili |
|---------------|---------------|-------------------------------|
| Ateş | Ateş direnci | Elementalist: Fireball, Fire Path |
| Buz | Buz direnci | Elementalist: Ice Nova |
| Bleed | Bleed direnci | Rogue: Hemorrhage, Warblade: Hamstring |
| Stun | Stun immun | Warblade: Ground Stomp, War Stomp |
| AoE ölüm | AoE direnci | Warblade: Whirlwind, Elementalist: Firestorm |
| Poison/Hex | Hex direnci | Hexer: tüm hex stack skilleri |
| Minyon ile öldürme | Minyon hasarı direnci | Summoner: tüm minyon skilleri |

**Tasarım notu:** Tüm skillerin Grudge tetikleyip tetiklemediğini tasarım aşamasında tabloya işle. Tooltip sadece aktif bir Grudge varsa çıkıyor — her skill kartında gösterme, sadece ilgili olduğunda.

---

## 4. EKSTra FİKİRLER — SADECE NADIR ODA OLMAKTAN ÖTEDE

Bunlar Flux'ı daha organik yapan, mevcut 4 sütunla bağlantı kuran tasarımlar. Hepsi opsiyonel — birini seç veya kombinle.

---

### A. GRUDGE TOKEN — "Hafıza Çalma"

**Fikir:** Grudge sistemi şu an tek yönlü. Düşman seni hatırlıyor, adapte oluyor.
**Ters çevir:** Sen de düşmanın hafızasını çal.

```
Elite düşmanı belirli bir yöntemle öldür:
  → Grudge Badge bozulur
  → Flux Token düşer (görünür ikon, üst köşede)

2-3 Token biriktir → istediğin anda Flux menüsünü kendin aç
```

**Ne kazandırıyor:** Flux artık sadece şans değil — kasıtlı strateji. "Şu düşmanı stun'dayken öldüreyim, token kazanayım" kararı.
**Solo dev kolaylığı:** Token sayacı + Flux menüsü, zaten var olan sistemin küçük uzantısı.

---

### B. RAGE YAKMA — "Fedakarlık"

**Fikir:** Rage barını doldurup [V] basmak yerine: **Rage'i tamamen yak → Flux hakkı**.

```
Rage = 100
  [V] → Normal ultimate (6-8s güç patlaması)
  [V + özel tuş, örn. hold 1.5s] → Rage tamamen boşalır → Flux menüsü açılır
```

**Risk:** O ana kadar biriktirdiğin Rage gitti.
**Ödül:** İstediğin anda, istediğin skilli değiştirme hakkı.

**Ne kazandırıyor:** Her run'da bilinçli bir "ultimate mi, pivot mı?" gerilimi.
**Solo dev kolaylığı:** Input variation + Flux menüsü çağırma, 1 ekstra input kontrolü.

---

### C. CORRUPTED FLUX — "Güç Karşılığı Lanet"

**Fikir:** Flux menüsünde 3. seçenek: Corrupted versiyon.

```
Normal Flux seçeneği:
  [A] Cleave → Ground Stomp (normal)
  [B] Neutral: Last Stand (normal)
  [C] CORRUPTED: Whirlwind+ (hasar ×1.5, ama her kullanımda -8 HP)
```

Corrupted skill görsel olarak ayrılıyor: kırmızı çerçeve, bozulmuş ikon.

**Tasarım kuralı:** Corrupted versiyon her zaman daha güçlü ama her zaman bir downside taşıyor. Downside türleri:
- HP kayıp per use
- CD uzaması
- Diğer skilde negatif proc
- Rage doldurma miktarı azalır

**Ne kazandırıyor:** Risk/ödül katmanı. "Boss öncesiyim, Corrupted alayım ama HP yönetimine dikkat etmeliyim."

---

### D. DRIFT / SINIF ERİMESİ — "Kimlik Kayması"

**Fikir:** Kasıtlı bir mekanik değil — oyunu oynarken kendiliğinden fark edilir.

```
Run boyunca cross-class skill/pasif aldıkça sistem takip eder:
  3+ cross-class slot → belirteç çıkar: "Drift başladı..."
  4+ cross-class slot → "Bu build artık [Arketip İsmi] gibi davranıyor"

Oyuncu onaylarsa: Arketip kimliği kilitlenir
  → Her iki sınıfın özel hybrid pasifinden biri aktif olur
  → İsim değişir (örn: Warblade + Elementalist = "Runeguard")
  → UI'da görünür: arketip simgesi

Reddederse: Devam eder, kimlik değişmez
```

**Dikkat:** Bu sistemi FAZ 2-3'te değil, FAZ 5-6'da düşün. Erken scope'a girmesin.

---

### E. BOSS SOUL GENIŞLETME — "Nasıl Dönüştür?"

Şu anki Boss Soul: Bir skill mutasyona uğruyor.
**Genişletme:** Hangi skili değil, *nasıl* dönüştüreceğini de sen seçiyorsun.

```
Boss öldürüldü → Soul düşer

Hangi skili dönüştürmek istiyorsun? [Q / W / E / R]
→ Seçtikten sonra:
  [HASAR] → Hasar odaklı mutasyon
  [CC]    → Kontrol odaklı mutasyon
  [PROC]  → Zincir/sinerji odaklı mutasyon

Örnek: Warblade'in "Charge" + Frost Boss Soul
  [HASAR] → "Glacial Rush" — freeze + %200 hasar
  [CC]    → "Cryogenic Tackle" — 3s freeze, hasar orta
  [PROC]  → "Frost Momentum" — Charge sonraki skill CD -5s, küçük slow
```

**Ne kazandırıyor:** Boss Soul zaten varolan Reforge mekanik. Bunu genişletmek ayrı bir sistem kurmaktan ucuz — ama hissettirdiği çok farklı.
**Not:** Bu en basit uygulama, FAZ 3'e kadar planlanabilir.

---

## 5. HANGİSİNİ UYGULA — ÖNERİ

| Seçenek | Faz | Zorluk | Etki |
|---------|-----|--------|------|
| Temel Flux (%5-8 oda) | FAZ 2 | Düşük | Temel esneklik |
| Boss Soul genişletme | FAZ 3 | Düşük | Çok yüksek his |
| Rage Yakma | FAZ 2 | Düşük | Risk gerilimi |
| Grudge Token | FAZ 3 | Orta | Sistemik derinlik |
| Corrupted Flux | FAZ 4 | Orta | Risk/ödül |
| Drift / Kimlik Erimesi | FAZ 5 | Yüksek | Efsane his ama scope riski |

**Önerilen öncelik sırası:**
1. **Boss Soul genişletme** — zaten var olan sistemi derinleştir, yeni sistem yok
2. **Temel Flux** — %5-8 nadir oda, preview'lü, ücretsiz
3. **Rage Yakma** — 1 input kontrolü, büyük his
4. **Grudge Token** — Grudge sistemi zaten var, küçük uzantı

---

## 6. FAZA GÖRE GİRMELİ?

**FAZ 2 kapsamı:** Temel Flux + Rage Yakma
**FAZ 3 kapsamı:** Boss Soul genişletme + Grudge Token
**FAZ 4 ileride:** Corrupted Flux, Drift

---

## 7. UI NOTLARI

- Flux ekranı normal oda sonu ekranından görsel olarak ayrılmalı — başka renk, başka müzik
- Cross-class skill/pasif: **altın çerçeve**
- Neutral skill/pasif: **gri-gümüş çerçeve**
- Corrupted seçenek: **kırmızı çerçeve + uyarı ikonu**
- Preview zorunlu: tooltip'te "Ne alacaksın, ne kaybedeceksin" net görünmeli
- Drift uyarısı: ekranın üstünde küçük, duyuru değil — sadece fark edilebilir
- **Sıralama önemli:** Oyuncu önce gelen 3 seçeneği görür, SONRA hangisini kaldıracağını seçer. Tersi değil. (StS campfire mantığı)
- **Grudge tooltip:** Flux ekranında swaplanan skill'in Grudge takibini nasıl etkileyeceği belirtilmeli — "Bu skili değiştirirsen [Elite Adı]'nın Grudge'ı sıfırlanır"

---

## 8. WEB ARAŞTIRMASI — ÖZGÜN BULGULAR

*Web araştırması (31 kaynak) Ollama çıktısını tamamlayan somut veriler sağladı.*

### TFT — "Legends" Mekanik: Kritik Ders

TFT, "Legends" özelliğiyle augment sistemi sorununu çözdü: oyun öncesi bir seçimle augment seçeneklerinden birinin belirli bir profile **yatkın olmasını** sağlıyorsun. Tam deterministik değil ama tam random da değil.

**Bizim için:** Flux odasında 3 seçeneğin biri her zaman "güvenli" bir Neutral olmalı. TFT'nin trait-agnostic augment tasarımı bu yüzden var — "Bunların hiçbiri bana uymuyor" frustrasyon noktasını yok eder.

---

### StS Colorless Kart Tasarımı — Detaylı Veri

- Normal kart ödüllerinde **çıkmıyor** — sadece shop, event, veya özel relic.
- Güçlü olanlar (Apotheosis, Metamorphosis, Violence) Rare tier — nadirlik hem lore hem denge aracı.
- Prismatic Shard relici onları normal ödüllere ekliyor → bu yüzden Prismatic Shard güçlü bir relic.
- StS 2 yeni kategori: Necrobinder sınıfı — yenilen düşman kartlarını kullanabiliyor. **Üçüncü kategori:** sınıf kartı değil, colorless değil — combat-conditional neutral. Grudge Token fikrimizle aynı felsefe.

**Bizim için:** Neutral skill/pasif havuzunu normal oda ödüllerinde düşük ağırlıkla bırak (yok etme), ama Flux'a özel değil. Erken tanısınlar, sonra Flux'ta karşılaştıklarında zaten bilisinler.

---

### StS Kart Silme — Somut Maliyet Verisi

- Shop'ta ilk silme: **75 altın**. Her ek silme: **+25 altın**.
- Bu artan maliyet "her şeyi sil" yerine "en gereksiz olanı sil" kararı yaratıyor.

**Bizim için (Soft Cost önerisi):** Flux'ta alınan yeni skill, **upgrade level 0 geliyor** — eski skill'in varsa upgrade'i kaybedilir. Altın gibi hard resource değil ama anlamlı bir fiyat. Run ilerledikçe skill'leri upgrade etmiş oyuncu bu swap'ı daha zor yapacak.

---

### Hades — Change of Fate Somut Veri

- 1. reroll: 1 Fate. 2. reroll: 2 Fate. 3. reroll: 3 Fate (exponential artış).
- Deneyimli oyuncular bunu **Styx'e** (son bölge) saklar — o noktada build netleşmiş, ne aradıklarını biliyorlar.
- Hades 2 community şikayeti: Hammer Upgrade'ler (silah modifikasyonları) hâlâ reroll edilemiyor. "En kritik seçimler reroll'dan muaf tutulmuş" = frustrasyon.

**Bizim için:** Oyuncu skill upgrade'ini zaten yaptıktan sonra "değiştirebileceğini" bilmek, daha cesur seçimler yapmasını sağlar — erken seçim daha az kalıcı hissettiriyor, bu sağlıklı.

---

### RoR2 Printer — "Scrap Tercihi" Kuralı

RoR2 Printer, gerçek item'dan önce **Scrap'ı** tüketiyor. Bu küçük kural bütün sistemi dengeli tutuyor — değerli item'larını riske atmadan deneyebiliyorsun.

**Bizim için:** Oyuncunun "bedelini göre" swap yapması için soft cost (upgrade kaybı) iyi bir analog. Tamamen free swap = RoR2'nin "solver" eleştirisiyle aynı sorunu yaratır.

---

### Balatro — 5 Joker Slot Psikolojisi

Balatro'nun sistemi neden çalışıyor: **slot kıtlığı anlam üretiyor.** 5 slot = her Joker birey olarak önemli. Satmak "geçmiş kararlarını düzeltme" değil "run'ını geliştirme" olarak kodlanıyor.

Topluluk gözlemi: Joker'lar tek başına ortalama, birleşince patlıyor. Sell/replace bu sinerji kürasyonunu mümkün kılıyor.

**Bizim için:** Max 6 skill slot + 2 pasif = Balatro'nun slot kıtlığı dinamiğini zaten taşıyor. Flux sadece "ne var"ı değiştirmiyor, oyuncu o kıtlıkla daha derin ilişki kuruyor.

---

### Dual-Class Sistemi için Özel Önerim (Web Araştırmasından)

Warblade + Elementalist oynayan biri için önerilen Flux SKILL SWAP teklifi:

| Seçenek | Tip | Açıklama |
|---------|-----|---------|
| Seçenek A | Warblade skilli | Aktif sınıflarından biri |
| Seçenek B | Elementalist skilli | Aktif sınıflarından diğeri |
| Seçenek C | Neutral Skill | Sınıfsız |

**Cross-class (3. sınıftan) erişim:** Sadece özel Flux varyantlarında (Corrupted Flux veya Chaos Flux). Normal Flux'ta aktif 2 sınıf + neutral — başka sınıftan yok. Bu dual-class seçiminin anlamını korur.

---

### Kaynak Özeti

| Oyun | Kaynak | Kritik Veri |
|------|--------|-------------|
| TFT | leagueoflegends.fandom.com | Reforger: randomize items, shadow→shadow, emblem→emblem |
| StS | slaythespire.wiki.gg | Silme: 75g + 25g artan, Transform: blind RNG |
| StS 2 | sts2.untapped.gg | Unlimited upgrade/remove campfire, Necrobinder 3. kategori |
| Hades | hades.fandom.com | Change of Fate: 1→2→3 Fate escalating |
| RoR2 | riskofrain2.fandom.com | Printer: Scrap önce tüketilir |
| Balatro | balatrogame.fandom.com | 5 slot hard cap, sell = run evolution |
