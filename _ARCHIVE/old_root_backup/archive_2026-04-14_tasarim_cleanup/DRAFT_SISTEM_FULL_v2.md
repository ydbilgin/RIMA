# RIMA — Draft & Progression Sistemi — TAM TASARIM
*Oluşturulma: 2026-04-11 | Review: ChatGPT + Gemini*

> Bu belge "ne alıyoruz" sorusunu tamamen yanıtlar.
> LMB/RMB güçlendirme, kalıcı özellikler, aktif/pasif sistem, sinerji, relics — hepsi bir arada.
> Amaç: AI review ile finalleştir, sonra koda yansıt.

---

## 0. TEMEL SORU: KAÇ KATMAN OLMALI?

Şu an sistem tek katmanlı: "3 skill sun, birini al." Bu yeterli değil.

**Önerilen katmanlar:**

```
KATMAN 1 — TEMEL SALDIRI (LMB / RMB)
  Her zaman var. Upgrade ile güçlenir. Slot ALMAZ.

KATMAN 2 — AKTİF SKİLLER (Q/E/R/F + Z/X)
  Slot alır (max 4+2). Draft'tan seçilir. Değiştirilebilir.

KATMAN 3 — PASİF SKİLLER
  Slot ALMAZ. Max 3 seviye. Draft'tan seçilir. Upgrade edilir.

KATMAN 4 — KALıCı ÖZELLİKLER (Traits)
  Stat bazlı. Birikirler. Slot yok, seviye yok. Run boyunca kalıcı.

KATMAN 5 — RELİKS (Relic)
  Çok güçlü, benzersiz efektler. Boss kill'den düşer. Max 3 per run.
```

Her katmanın frekansı, kaynağı ve etkisi farklı olmalı.

---

## 1. KATMAN 1 — LMB / RMB SİSTEMİ

### 1A. Neden LMB/RMB güçlenmeli?

- LMB her saniye kullanılıyor. Run sonuna kadar 500+ kez basılıyor.
- Şu an sıfır progression — başta ne ise sonda o.
- Hades'de "Cast" ve "Attack" boon'ları en core loop değişkenler.
- RIMA'da LMB/RMB güçlendirmesi → build kimliği.

### 1B. Kaç çeşit LMB/RMB upgrade?

**Önerilen model: PER CLASS, 3 EKOLETLİ SEÇİM**

Her class için 3 farklı LMB upgrade "ekolü" var.
Oyuncu run'da bunlardan biri yönünde güçlenir.
Bir seferinde bir ekol seçilir, ama aynı ekol birden fazla güçlendirilebilir.

**Warblade LMB Ekolları:**

| Ekol | İsim | Lv1 | Lv2 | Lv3 |
|------|------|-----|-----|-----|
| Rage | **Fury Strikes** | Her LMB hit +2 Rage | Her 3. hit Rage +5 bonus | Rage %80+ iken LMB +%30 hasar |
| Bleed | **Savage Edge** | 3. LMB hit bleed uygular (3s) | Bleed stack'lı hedefe LMB +%20 hasar | Bleed tick LMB CD'yi -%0.1s azaltır |
| Execute | **Bone Breaker** | LMB son vuruşu target'ı yavaşlatır | HP<%50 hedefe LMB +%25 hasar | Execute animasyonu ile saldırı geri iter |

**Warblade RMB Ekolları:**

| Ekol | İsim | Lv1 | Lv2 | Lv3 |
|------|------|-----|-----|-----|
| Block | **Iron Guard** | Block süre +0.3s | Başarılı block sonrası Rage +15 | Mükemmel block → stun 1.5s |
| Counter | **Blade Mirror** | Block sırasında alınan hasarın %30'unu yansıt | Yansıma hasarı +%50 | Yansıma AoE'ye döner (1m) |
| Drain | **Blood Feast** | Block başına +5 HP iyileşme | Block süresinde healing +%100 | Block bozulursa çevreye Rage patlaması |

> **Soru (AI için):** Her sınıfta 3 ekol × 3 seviye = 9 upgrade yeterli mi? Yoksa 2 ekol × 3 = 6 mi daha net?

### 1C. LMB/RMB upgrade NEREDEN GELİR?

**Seçenek A — Draft'tan (Normal Pool)**
- Ana draft havuzuna "Attack Upgrade" tipi eklenir
- Pro: Sık görünür, oyuncu aktif karar verir
- Con: Skill/pasif kararlarını seyreltir

**Seçenek B — Milestone Anı (Ayrı Ekran)**
- Oda 3, 6, 9 → normal draft değil "Attack Forge" ekranı açılır
- Pro: Temiz separation, dramatik an
- Con: Cooldown hissi, bazı run'larda uzun bekleyiş

**Seçenek C — Elite/Boss Kill Reward**
- Elite öldürünce küçük Attack Upgrade seçimi
- Boss öldürünce büyük Attack Upgrade (2 katman atlayabilir)
- Pro: Risk/reward, combat teşvik
- Con: Elite kill frekansı değişken → tutarsız

**Seçenek D — Hibrit (ÖNERİ)**
```
Oda 1-3: Normal draft (skill/pasif)
Oda 4  : İlk Attack Upgrade seçimi (milestone)
Oda 5-8: Normal draft
Oda 9  : İkinci Attack Upgrade seçimi (milestone)
Boss   : Relic (ayrı sistem)
```
→ Run'da toplam 2 LMB + 2 RMB upgrade fırsatı

> **Soru (AI için):** Hibrit model mi yoksa başka bir model mi?

---

## 2. KATMAN 4 — KALıCı ÖZELLİKLER (Traits)

### 2A. Trait nedir?

- "Stat bazlı küçük kalıcı bonus" — slot veya seviye yok
- Bir kez alınır, o stat kalıcı olarak değişir
- Birikebilir: aynı trait'ten 2-3 tane alınabilir (stacks)
- Skill kadar dramatik değil, ama run sonunda %20-30 güç farkı yaratır

### 2B. Trait listesi (önerilen)

| İsim | Değer | Stackable |
|------|-------|-----------|
| **Toughened Hide** | Max HP +20 | 3x |
| **Honed Reflexes** | Tüm CD -5% | 2x |
| **Iron Will** | CC süresi -%20 | 2x |
| **Killing Momentum** | Her kill sonrası 3s hareket hızı +%15 | 3x |
| **Deep Reserves** | Kaynak (Rage/Mana/Energy/Focus) max +15 | 3x |
| **Veteran's Scar** | Her oda temizlenince perm +2 hasar (max +30) | 1x (6 oda) |
| **Stoic Endurance** | HP regen +1/sn (savaş dışında) | 3x |
| **Opportunistic Strike** | Düşman stun/root altındayken +%10 crit şans | 2x |

### 2C. Trait NEREDEN GELİR?

**Seçenek A — Sandık Ödülü**
- Sandık açınca: gold veya skill veya trait seçimi
- Pro: Exploration teşvik eder
- Con: Sandık zaten var, overload olabilir

**Seçenek B — Alt-Hasar Ödülü**
- Oda temizlerken sıfır hasar alırsan bonus trait sunulur
- Pro: Skill teşvik, mastery hissi
- Con: Yeni oyuncular için hayal kırıklığı yaratabilir

**Seçenek C — Draft'a Ara Ara Girer**
- Her ~4. draft'ta 3 seçenek yerine 2 skill + 1 trait seçimi
- Pro: Her draft kararı farklı hisseder
- Con: Skill almak isteyen oyuncuyu zorlar

**Seçenek D — Özel Oda: "Trait Forge"**
- Her 2 boss arasında bir "Forge" oda: sadece trait verir
- Pro: Anlamlı milestone, temiz
- Con: Ekstra oda tasarımı gerekir

> **ÖNERİ:** Seçenek A + C hibrit: Sandıklar trait verebilir, ayrıca her 5. draft'ta bir trait seçeneği görünür.

---

## 3. KATMAN 5 — RELİKS (Relic)

### 3A. Relic nedir?

- Boss öldürünce düşen özel eşya
- Aktif değil, daima pasif efekt
- Run'da max 3 relic — çok güçlü
- ScriptableObject olarak tanımlanır, her boss'un kendi Relic havuzu var

### 3B. Örnek Relicler

| İsim | Kaynak | Efekt |
|------|--------|-------|
| **Bone Trophy** | Boss 1 | Her kill'de kalıcı +1 hasar birikir (max +25) |
| **Warblade's Crest** | Boss 2 | Rage max +30, Rage asla 0'a inmez |
| **Void Shard** | Boss 3 | Kill başına %5 ihtimalle düşman patlıyor (AoE) |
| **Echo Stone** | Boss 2 | Son kullandığın aktif skill 3s sonra otomatik tekrar kullanılır |
| **Blood Pact** | Boss 1 | Her oda başında HP -10, ama tüm hasar +%15 |
| **Phantom Grip** | Boss 3 | 2 aktif skill aynı anda kullanılabilir (CD +%30) |

> **Soru (AI için):** Relic'ler ne kadar güçlü olmalı? Hades'de "keepsake" + "boon" ayrımı vardı. RIMA'da relic, skill'den belirgin şekilde güçlü mü olmalı yoksa aynı seviyede mi?

---

## 4. DRAFT SİSTEMİ — REVİZE EDİLMİŞ TAM TABLO

| Ne | Katman | Kaynak | Sıklık | Slot |
|----|--------|--------|--------|------|
| LMB upgrade | 1 | Milestone (Oda 4+9) | 2× per run | Yok |
| RMB upgrade | 1 | Milestone (Oda 4+9) | 2× per run | Yok |
| Aktif skill | 2 | Normal draft | Her oda | 4+2 |
| Pasif skill | 3 | Normal draft | Her oda | Yok (max 3 lv) |
| Sinerji skill | 2 | Normal draft (secondary sonrası) | Nadir | 4+2 |
| Trait | 4 | Sandık + Her 5. draft | Orta sıklık | Yok, birikir |
| Relic | 5 | Boss kill | 1-3× per run | Max 3 |

---

## 5. NORMAL DRAFT'IN YENİDEN TANIMLANMASI

### 5A. Draft Sunumu (Şu Anki)
3 kart: Hepsi skill/pasif karışık.

### 5B. Draft Sunumu (Önerilen)
Her draft'ın bir **"draft tipi"** var:

```
TİP A — Standard (en yaygın)
  Kart 1: Aktif skill
  Kart 2: Aktif skill veya pasif
  Kart 3: Pasif veya trait
  
TİP B — Passive Focus (slot dolmak üzereyken)
  Kart 1: Pasif upgrade
  Kart 2: Pasif upgrade
  Kart 3: Aktif skill
  
TİP C — Forge (her 5. draft)
  Kart 1: Aktif skill
  Kart 2: Pasif
  Kart 3: Trait
  
TİP D — New Unlock (secondary seçilince)
  Kart 1: Secondary aktif
  Kart 2: Sinerji pasif veya sinerji aktif
  Kart 3: Neutral pasif
```

→ Oyuncu her draft açıldığında farklı bir "an" yaşıyor.

---

## 6. CROSS-CLASS CROSS-SINERJI (Önceki belgeden özet)

Önceki belgede detaylı yazıldı. Burada sadece sistem etkisini özetle:

- Cross-class aktifler: Z/X slotlara gider (2 slot)
- Cross-class pasifler: Draft'a girer, secondary seçilince havuza eklenir
- Sinerji skilleri: Sadece o secondary seçilince havuza girer, Epic tier

> Önceki belge: `DRAFT_SKILL_TASARIM.md`

---

## 7. SIRALAMA: NE ZAMAN NE GELİR?

```
ODA 1: İlk draft → 2 WB aktif + 1 neutral pasif (tanıtım)
ODA 2: Normal draft
ODA 3: Normal draft
ODA 4: ATTACK FORGE (LMB veya RMB upgrade seç)
ODA 5: Normal draft
ODA 6: Normal draft
BOSS 1: Boss kill → Relic seçimi (1 of 3)
ODA 7: TİP C draft (1 aktif + 1 pasif + 1 trait)
ODA 8: Normal draft
ODA 9: ATTACK FORGE #2
ODA 10: Normal draft
BOSS 2: Boss kill → Relic #2
...
```

> **Soru (AI için):** Oda 4'te Attack Forge çok erken mi? Belki oda 5 veya 6?

---

## 8. CLASS-SPECIFIC KARŞILAŞTIRMA

Farklı class kombinasyonlarının ne hissetmesi gerektiği:

| Combo | LMB ekolü ideal | Relic ideal | Trait ideal |
|-------|-----------------|-------------|-------------|
| WB solo | Fury Strikes (Rage loop) | Warblade's Crest | Deep Reserves |
| WB+Elem | Savage Edge (bleed+mana sinerji) | Echo Stone | Opportunistic Strike |
| WB+Shadow | Bone Breaker (execute) | Blood Pact | Killing Momentum |
| WB+Ranger | Fury Strikes (Rage → sustain) | Bone Trophy | Veteran's Scar |

---

## 9. AÇIK SORULAR — AI REVİEW İÇİN

### LMB/RMB Soruları
1. **Ekol sistemi mi, serbest upgrade mi?** 3 ekol × 3 seviye mi, yoksa 9 ayrı bağımsız upgrade mı?
2. **Milestone frekansı:** Oda 4 ve 9 mu, yoksa her 3 odada bir küçük upgrade mi?
3. **LMB ve RMB ayrı mı seçilsin?** Yani hem LMB hem RMB upgrade aynı anda mı sunulur, yoksa ayrı ayrı mı?

### Trait Soruları
4. **Trait stacklenebilir mi?** Aynı trait 3 kez alınabilirse +60 HP (3×20) mümkün — çok mu güçlü?
5. **Trait vs Pasif farkı nasıl net hissettirileceği?** Oyuncu UI'da hangisi pasif hangisi trait olduğunu anlayacak mı?

### Relic Soruları
6. **Relic per boss mu, yoksa sabit 3 boss'a mı bağlı?** Eğer Faz 2'de 5 boss varsa ne olur?
7. **Relic çakışması:** İki incompatible relic birden alınabilir mi? (Ör: Blood Pact + Warblade's Crest)

### Genel Sistem Soruları
8. **Kaç katman çok?** 5 katman (LMB/RMB + Aktif + Pasif + Trait + Relic) — acemi oyuncu için bunaltıcı mı?
9. **Hangi katmanın en çok kararı var?** Aktif skill seçimi mi, yoksa LMB ekolü mi build kimliğini daha çok belirler?
10. **Faz 1 scope (sadece Warblade):** Şimdi hangi katmanları implement etmeli, hangisi Faz 2'ye bırakılmalı?

---

## 10. FAZ 1 MINIMUM VİABLE DRAFT (Öneri)

Faz 1'de her şeyi yapmaya gerek yok. Minimum set:

| Katman | Faz 1 | Faz 2+ |
|--------|-------|--------|
| LMB upgrade | ✅ Basit versiyon (3 upgrade, milestone) | Ekol sistemi |
| RMB upgrade | ❌ Faz 2 | Ekol sistemi |
| Aktif skill | ✅ Mevcut sistem | Cross-class |
| Pasif skill | ✅ Mevcut sistem | Class-specific pasifler |
| Trait | ✅ Sadece sandıktan (basit) | Draft + Forge |
| Relic | ❌ Faz 2 (boss yok henüz) | Boss kill reward |
| Sinerji skill | ❌ Faz 2 (secondary sistem) | Secondary unlock sonrası |

**Faz 1 eklenecek:** LMB upgrade (milestone, 3 seçenek) + Trait (sandıktan)

---

## 11. MEVCUT KOD DURUMU

Şu an çalışan sistemler:
- ✅ `DraftManager.cs` — aktif/pasif seçim, replace modu
- ✅ `SkillOfferGenerator.cs` — 5 senaryo, tier weight
- ✅ `SkillDatabase.cs` — 13 WB + 10 cross + 6 pasif
- ✅ `PassiveBase.cs` — 3 level upgrade
- ✅ `SkillOfferUI.cs` — kart layout, replace mode

Eksik (bu tasarımdan gelecekler):
- ❌ `LMBUpgradeSystem.cs` — LMB upgrade tanımları + milestone trigger
- ❌ `TraitSystem.cs` — trait birikimi, sandık/forge entegrasyonu
- ❌ `RelicSystem.cs` — boss kill reward (Faz 2)
- ❌ `AttackForgeUI.cs` — milestone anda gösterilecek ekran
- ❌ Oda sayacı → milestone trigger (DraftManager'a eklenecek)

---

*AI review tamamlanınca kararlar → `MASTER_KARAR_BELGESI.md` + kod implementasyonu*
