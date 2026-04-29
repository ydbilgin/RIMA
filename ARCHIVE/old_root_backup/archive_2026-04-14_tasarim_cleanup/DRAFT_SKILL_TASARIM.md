# RIMA — Draft Skill Sistemi Tasarım Belgesi
*Oluşturulma: 2026-04-11 | Revize edilecek: ChatGPT + Gemini*

> Bu belge draft sisteminde sunulacak tüm skill havuzunu analiz eder.
> Amaç: Hades-benzeri anlamlı seçim, cross-class sinerji, pasif derinlik.

---

## 1. MEVCUT DURUM (Kod'da Ne Var)

### SkillDatabase'de kayıtlı olanlar

**Warblade aktif (13):**
Iron Charge, Cleave, Deep Wound, War Stomp, Blade Rush, Sunder Mark, Iron Crush, Battle Surge, Death Blow, Crippling Blow, Gravity Cleave, Iron Counter, Ironclade Momentum (pasif olarak işaretli)

**Cross-class aktif — Elementalist (4):**
Fireball, Chain Lightning, Blink, Arcane Blast

**Cross-class aktif — Shadowblade (3):**
Backstab, Shadow Step, Fan of Knives

**Cross-class aktif — Ranger (3):**
Aimed Shot, Disengage, Multi Shot

**Neutral pasif (5):**
Iron Body, Berserker's Blood, Predator's Eye, War Veteran, Unyielding

**Warblade pasif (1):**
Ironclade Momentum

**Cross-class pasif (otomatik, draft'a GİRMİYOR — PlayerClassManager ekliyor):**
CrossClassPassive_WB_Elem, CrossClassPassive_WB_Shadow, CrossClassPassive_WB_Ranger

---

## 2. SORUNLAR & EKSİKLİKLER

### A) Cross-class aktif sayısı yetersiz
- Elem: 4 skill → 2 slot için yeterli ama çeşitlilik yok
- Shadow: 3 skill → minimum seviyede
- Ranger: 3 skill → minimum seviyede
- **Hedef:** Her secondary için 6-7 seçenek, oyuncu her run'da farklı kombinasyon yaşasın

### B) Cross-class sinerjisi olmayan skillerin havuzda olması sorunu
Fan of Knives, Chain Lightning gibi skill'ler WB'de tuhaf hissettiriyor.
Bazı secondary skilleri WB mekanikleriyle HIÇ etkileşime girmiyor.
**Çözüm:** Her secondary için "WB sinerjili" ve "standalone" olarak etiketle.
Standalone'lar da sunulabilir ama sinerji etiketi UI'da gösterilmeli.

### C) Class-specific draft pasif neredeyse yok
WB'nin Ironclade dışında pasif yok. 
Elem/Shadow/Ranger'ın hiç draft pasifleri yok.
**Çözüm:** Her class 3-4 pasif — build yönünü tanımlar.

### D) Cross-class sinerjisi olan ÖZEL skillerin olmaması
Hades'de "Duo Boon" konsepti: sadece 2 belirli boon sahibindeysen çıkan.
RIMA'da bu şu an YOK. Tasarım gerekiyor.
**Öneri:** "Sinerji Skill" — sadece Secondary seçildikten sonra havuza girer.
Örnek: WB+Shadowblade → "Iron Phantom" (WB'nin Iron Charge'ı + SB'nin stealth'i birleşir)

---

## 3. ÖNERİLEN CROSS-CLASS AKTİF SKİLLER

### 3A. Warblade + Elementalist (Z/X)

Seçim kriteri: WB Rage mekaniğiyle etkileşen veya WB'nin boş olduğu alana (range/burst) katkı yapan.

| # | İsim | Tier | Kaynak | WB Sinerjisi | Açıklama |
|---|------|------|---------|--------------|----------|
| 1 | **Fireball** | Common | Mana 15 | ✅ Ateş hasarı + Rage bonusu WB_Elem pasifiyle | 3m patlama, 35 ateş hasarı, 4s yanma DoT |
| 2 | **Blink** | Rare | Mana 20 | ✅ WB'nin gap-close araçlarını tamamlar | 6m ışınlanma, sonraki spell +%20 hasar |
| 3 | **Glacial Spike** | Rare | Mana 20 | ✅ Slow/stun → WB'nin Death Blow setup'ı | 6m buz hattı, %40 slow + %180 hasar |
| 4 | **Chain Lightning** | Rare | Mana 25 | ⚠️ Standalone, WB'ye direkt sinerji yok | 5 hedefe sekiyor, temizlik için |
| 5 | **Arcane Blast** | Epic | Mana 30 | ✅ AoE burst → War Stomp knockup'ı sinerji | Çevredeki tüm düşmanlar, 50 arkan hasarı |
| 6 | **Mirror Image** | Rare | Mana 0 | ⚠️ Standalone, WB playstyle'ına yabancı ama tanky | 2 kopya 8s, önce kopyalara hasar |

**Önerilen slot count:** 6 ✓
**Çıkarılmalı:** Hiçbiri çıkarılmaz, ama Chain Lightning + Mirror Image'ın "düşük sinerji" etiketi olmalı.

---

### 3B. Warblade + Shadowblade (Z/X)

Seçim kriteri: WB'nin yakın dövüş tarzını deepen eden, CC setup veya execute sinerjisi.

| # | İsim | Tier | Kaynak | WB Sinerjisi | Açıklama |
|---|------|------|---------|--------------|----------|
| 1 | **Backstab** | Common | Energy 20 | ✅ WB zaten arkaya geçiyor (Iron Charge) | Arkadan %200 hasar. Önden normal. |
| 2 | **Shadow Step** | Rare | Energy 30 | ✅ Ek gap-close, Bladestorm escape | Hedefe 8m ışınlan, 0.5s stun |
| 3 | **Hemorrhage** | Common | Energy 15 | ✅ Bleed + Rage sinerji (WB_Shadow pasifi) | 8s bleed DoT, öldürünce yayılır |
| 4 | **Kidney Shot** | Rare | Energy 35 | ✅ 5CP stun → Death Blow execute setup | 4-5s stun (Combo Point'e göre) |
| 5 | **Evasion** | Rare | Energy 25 | ✅ WB'nin savunma açığını kapatır | 4s %100 dodge, her dodge +1CP |
| 6 | **Fan of Knives** | Rare | Energy 30 | ⚠️ Standalone AoE, Cleave'in yedek alternatifi | 360° AoE, tüm debuffları yayar |

**Önerilen slot count:** 6 ✓
**Çıkarılmalı:** Vanish, Preparation, Ambush (çok Rogue-spesifik, WB playstyle'ına yabancı).

---

### 3C. Warblade + Ranger (Z/X)

Seçim kriteri: WB'nin yokluğu olan range/zoning'i veya CC setup'ı karşılar.

| # | İsim | Tier | Kaynak | WB Sinerjisi | Açıklama |
|---|------|------|---------|--------------|----------|
| 1 | **Aimed Shot** | Common | Focus 20 | ✅ Range opener → WB approach + CC setup | Hold 1s = %250 + crit, anında da %150 |
| 2 | **Disengage** | Common | Focus 15 | ✅ WB'nin kaçış eksikliğini giderir | 6m geri atla + slow alanı bırak |
| 3 | **Concussive Arrow** | Common | Focus 10 | ✅ Knockback+root → Death Blow setup | 4m knockback + 2s root |
| 4 | **Barbed Net Shot** | Rare | Focus 20 | ✅ Root → Death Blow / Iron Charge sinerji | 2s root + 4s bleed, alana yayılır |
| 5 | **Explosive Trap** | Rare | Focus 15 | ✅ Zemin kontrolü, WB'nin yaklaşmasını güvenli yapar | 3s sonra patlama + slow 3s |
| 6 | **Volley** | Rare | Focus 25 | ⚠️ Standalone, temizlik için | 4m alana 3s ok yağmuru, slow |

**Önerilen slot count:** 6 ✓
**Çıkarılmalı:** Point Blank (WB zaten yakında, gereksiz), Rapid Fire (too Ranger-specific), Tethering Arrow (çok spesifik).

---

## 4. SİNERJİ SKİLLERİ (YENİ KONSEPT)

> Sadece o secondary seçilince havuza girer. "Duo Boon" muadili.
> Bu skillerin **her iki sınıfın mekaniğini** kullanması şart.

### WB + Elementalist → "Molten Wrath"
**Koşul:** Warblade primary + Elementalist secondary
**Tier:** Epic
**Açıklama:** Rage 50+ iken Fireball kullanınca ekstra lav patlaması (5m AoE), her düşmana 2s yanma + knockback. Yanmakta olan düşmana Iron Charge → yanma süresi ×2.
**Neden çalışır:** Rage (WB) + Ateş (Elem) → WB'nin hamle + Elem'in DoT birleşir.

### WB + Shadowblade → "Iron Phantom"
**Koşul:** Warblade primary + Shadowblade secondary
**Tier:** Epic
**Açıklama:** Iron Charge kullandıktan 2s içinde Shadow Step kullanılırsa: sonraki saldırı backstab bonus kazanır (%150 hasar), 3s görünmezlik (mini-stealth). Bleed aktif hedefe Iron Charge → stun süresi +1s.
**Neden çalışır:** WB'nin dash (Iron Charge) + SB'nin reposition (Shadow Step) → agresif gizlilik loop'u.

### WB + Ranger → "Predator's Advance"
**Koşul:** Warblade primary + Ranger secondary
**Tier:** Epic
**Açıklama:** Root/CC altındaki hedefe Iron Charge → hasar ×2 + Rage +30 yerine normale ek +50 Rage. Gravity Cleave'den sonra 4s Aimed Shot CD yok ve maliyet 0. 
**Neden çalışır:** Ranger'ın CC setup → WB'nin execute döngüsü mükemmel zincir.

---

## 5. DRAFT PAsifleri — SINIFA ÖZEL

> Bunlar draft havuzuna GİRER (slot almaz, max 3 seviye).
> Hem primary hem secondary oynuyorsa her ikisinin pasif havuzu aktif.

### 5A. Warblade Pasifleri (Draft)

| İsim | Tier | Etki (Lv1 / Lv2 / Lv3) | Build yönü |
|------|------|--------------------------|------------|
| **Ironclade Momentum** | Epic | Hareket halinde hasar +%20 / +%40 / +%60 | Agresif oynayan |
| **Blood Drinker** | Common | Kill başına +8 / +12 / +16 HP iyileşme | Hayatta kalma |
| **Wrath Protocol** | Rare | HP <%50'de Rage kazanımı +%20 / +%35 / +%50 | Burst / son stand |
| **Tempered Fury** | Rare | Rage asla 20 / 30 / 40 altına inmez (savaşta) | Rage-bağımlı build |
| **Berserker's Resolve** | Common | Her knockback aldığında Rage +5 / +8 / +12 | Savunma-Rage dengesi |

**Toplam:** 5 WB pasifleri (birini sil istersen, ama biri Legacy)

---

### 5B. Elementalist Cross-Class Pasifleri (Draft — Secondary seçilince havuza girer)

| İsim | Tier | Etki (Lv1 / Lv2 / Lv3) |
|------|------|--------------------------|
| **Arcane Attunement** | Common | Mana regen +1 / +2 / +3 /sn |
| **Elemental Surge** | Rare | Her Elemental State stack'ı hasar +%2 / +%3 / +%5 |
| **Mana Shield** | Rare | HP <%30'da mana otomatik hasar absorbe eder (max 20 / 35 / 50) |

---

### 5C. Shadowblade Cross-Class Pasifleri (Draft)

| İsim | Tier | Etki (Lv1 / Lv2 / Lv3) |
|------|------|--------------------------|
| **Shadow's Embrace** | Common | Stealth girer/çıkınca +5 / +8 / +12 HP |
| **Crimson Edge** | Rare | Bleed tick başına +3 / +5 / +8 Energy |
| **Opportunist** | Rare | CC altındaki hedefe hasar +%15 / +%25 / +%40 |

---

### 5D. Ranger Cross-Class Pasifleri (Draft)

| İsim | Tier | Etki (Lv1 / Lv2 / Lv3) |
|------|------|--------------------------|
| **Eagle Eye** | Common | Menzil etkisi +%10 / +%20 / +%30 uzar (ok/ranged) |
| **Survival Instinct** | Rare | Disengage / geri atlama CD -2s / -3s / -4s |
| **Hunter's Focus** | Rare | Hareket halinde ateşleme %10 / %20 / %30 hasar kaybı azalır |

---

## 6. NEUTRAL PAsifleri (Revize)

> Tüm class kombinasyonlarına açık. Özellikle build yönünü belirlemez, genel güçlenme.

| İsim | Tier | Etki | Notlar |
|------|------|------|--------|
| **Iron Body** | Common | Max HP +25 / +40 / +60 | Temel hayatta kalma |
| **Berserker's Blood** | Rare | Rage/Energy/Focus kazanımı +%15 / +%25 / +%35 | Kaynak universally iyi |
| **Predator's Eye** | Rare | Tüm hasar +%8 / +%15 / +%22 | Saf hasar |
| **War Veteran** | Common | Kill → +5 / +8 / +12 kaynak | Snowball potansiyel |
| **Unyielding** | Epic | HP <%50: 3 / 4 / 5s hasar bağışıklığı (CD 60/45/30s) | Yüksek riskli an koruyucu |
| **Battle Scars** | Common | Her oda temizlenince kalıcı +3 / +5 / +8 max HP | Run birikimi |
| **Adrenaline Rush** | Rare | Kill başına 3s +%20 / +%30 / +%40 hız | Hareketlilik / snowball |
| **Ancient Instinct** | Epic | Düşman saldırısı algılanınca 0.2s önce hasar -%20 / -%30 / -%40 | Aktif oyuncuya ödül |

**Toplam:** 8 neutral pasif (önceki 5'in üzerine +3)

---

## 7. SAYISAL ÖZET

### Aktif Skill Havuzu
| Sınıf | Aktif skill | Draft'ta çıkabilir |
|-------|------------|-------------------|
| Warblade (primary) | 12 | 12 (tümü) |
| Elementalist (cross) | 12 tasarlandı → **6 seçildi** | 6 |
| Shadowblade (cross) | 12 tasarlandı → **6 seçildi** | 6 |
| Ranger (cross) | 12 tasarlandı → **6 seçildi** | 6 |
| Sinerji skilleri (WB+X) | 3 (birer tane her combo) | Sadece o combo seçildiyse |
| **TOPLAM** | **33 + 3 sinerji** | Duruma göre 18-39 |

### Pasif Havuzu
| Kategori | Adet |
|----------|------|
| Neutral pasifler | 8 |
| Warblade pasif (draft) | 5 |
| Elementalist cross pasif | 3 |
| Shadowblade cross pasif | 3 |
| Ranger cross pasif | 3 |
| **TOPLAM** | **22 pasif** |

### Otomatik Cross-Class Pasifler (Draft'a GİRMEZ, PlayerClassManager ekler)
| Pasif | Koşul |
|-------|-------|
| CrossClassPassive_WB_Elem | Elem secondary seçince |
| CrossClassPassive_WB_Shadow | Shadow secondary seçince |
| CrossClassPassive_WB_Ranger | Ranger secondary seçince |

---

## 8. DRAFT HAVUZU — NORMAL OYUNDA BOYUT

Warblade primary + Shadowblade secondary seçerse:
- Aktifler: 12 WB + 6 Shadow + 1 Sinerji = 19 aktif
- Pasifler: 8 neutral + 5 WB + 3 Shadow = 16 pasif
- **Toplam havuz: 35 seçenek** → Draft başına 3 kart sunulur

Bu boyut Hades'e yakın (Hades havuz genellikle 20-30 boon/god).

---

## 9. REVIZE GEREKTİREN SORULAR (ChatGPT/Gemini için)

1. **Sinerji Skill'i ne zaman göster?**
   - A) Secondary seçilir seçilmez (ilk draft)
   - B) Her iki class'tan birer skill alındıktan sonra
   - C) Rastgele ama ağırlıklı (en az 3 normal draft sonrası)

2. **Cross-class aktiflerin tier dağılımı doğru mu?**
   - Şu an hepsi Common/Rare. Epic cross-class skill olmalı mı?

3. **WB pasifleri arasında "Wrath Protocol" çok güçlü mü?**
   - HP<%50 + Rage kazanımı +%50 → Last Stand build'lerde abuse riski?

4. **Neutral pasif sayısı 8 mi fazla?**
   - 5-6 yeterli mi? Yoksa 8 çeşitlilik için gerekli mi?

5. **Sinerji skilleri "iki sınıf mekanığini" gerçekten kombine ediyor mu?**
   - Iron Phantom (WB+SB) tasarımı yeterince ilgi çekici mi?
   - Predator's Advance (WB+Ranger) mekanik açıdan uygulanabilir mi?

6. **Cross-class pasif draft'a girmeli mi?**
   - Şu an otomatik. Bazıları draft'a girip "seçim" yapılabilir mi?
   - (Ör: WB+Elem için 2 farklı pasif sunulur, biri seçilir)

---

## 10. EKSIK OLAN VE YAPILACAKLAR

- [ ] Sinerji Skill scriptleri yazılmadı (Molten Wrath, Iron Phantom, Predator's Advance)
- [ ] Warblade draft pasiflerinin 4'ü yazılmadı (Blood Drinker, Wrath Protocol, Tempered Fury, Berserker's Resolve)
- [ ] Cross-class pasifler (Arcane Attunement, Shadow's Embrace vb.) yazılmadı
- [ ] Neutral pasif +3 (Battle Scars, Adrenaline Rush, Ancient Instinct) yazılmadı
- [ ] SkillDatabase'e sadece 6 Elem/Shadow/Ranger skill girilmesi (şu an az var)
- [ ] Sinerji skill koşulu: secondary seçildikten sonra havuza girme mantığı

---

*Bu belge ChatGPT ve Gemini review sonrası güncellenecek.*
*Kabul edilen değişiklikler → SkillDatabase.cs + SINIF_VE_SKILL_KARAR_BELGESI.md*
