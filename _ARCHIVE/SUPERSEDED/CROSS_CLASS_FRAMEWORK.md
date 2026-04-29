# RIMA — Cross-Class Framework & Tam Sınır Kararları
*Oluşturulma: 2026-04-12 | Referans belge*

---

## 1. TEMEL KURAL: PRIMARY ≠ SECONDARY

Aynı iki class seçilse bile sıra önemli. WB primary + SB secondary FARKLIDIR.

```
PRIMARY CLASS belirler:
  ✓ LMB / RMB mekanikleri
  ✓ Birincil kaynak (Rage / Energy / Mana / vs.)
  ✓ Q/E/R/F slot havuzu (12 skillden 4 tanesini alırsın)
  ✓ Draft pasif havuzunun ağırlıklı kısmı
  ✓ Temel kimlik ("ben bir Warblade'im")

SECONDARY CLASS belirler:
  ✓ Z/X slot havuzu (6 skillden 2 tanesini alırsın)
  ✓ İkincil kaynak erişimi (kısıtlı, built-in regen yok — aktif kazanılır)
  ✓ Draft pasif havuzuna 3 cross-class pasif girer
  ✓ 1 sinerji skill havuza girer (koşullu)
  ✓ Otomatik cross-class pasif eklenir (PlayerClassManager)
```

---

## 2. PRIMARY vs SECONDARY — FARK TABLOSU

| Özellik | Primary | Secondary |
|---------|---------|-----------|
| LMB/RMB | ✅ Tam kontrol | ❌ Değişmez |
| Kaynak | Birincil, regen var | İkincil, aktif kazanılır |
| Slot sayısı | 4 (Q/E/R/F) | 2 (Z/X) |
| Skill havuz boyutu | 12 aktif | 6 aktif (özenle seçilmiş) |
| Draft pasif | 5 class-specific | 3 cross-class pasif |
| Sinerji skill | — | 1 adet (koşullu) |
| Otomatik pasif | — | CrossClassPassive_X |
| LMB ekol | ✅ 3 ekol × 3 lv | ❌ Yok |

---

## 3. WARBLADE PRIMARY — FULL SKILL HAVUZU (12 aktif)

Bunlar Q/E/R/F slotlarına girer. Run boyunca max 4 tanesini tutarsın.

| # | İsim | Tier | CD | Özet |
|---|------|------|----|------|
| 1 | Iron Charge | Common | 3s | Dash + hasar + Rage +15/isabet |
| 2 | Cleave | Common | 5s | 360° AoE, Rage %30 ekstra hasar |
| 3 | Deep Wound | Common | 8s | Anında 20 + 8s kanama, Rage +20 |
| 4 | Sunder Mark | Common | 14s | Zırh -%40 (8s), Death Blow ile -%60 |
| 5 | Crippling Blow | Common | 9s | 3s yavaş+sersem, Death Blow ile %600 |
| 6 | War Stomp | Rare | 10s | 3m geri fırlat + 2s sersem + Rage +25 |
| 7 | Blade Rush | Rare | 10s | 20u/s hücum, yoldakilere 48 hasar |
| 8 | Gravity Cleave | Rare | 8s | Çeker + ağır hasar |
| 9 | Iron Counter | Rare | 14s | 2s savunma, gelen saldırıyı yansıt |
| 10 | Iron Crush | Epic | 12s | 6s buff: temel hasar +%30, Rage 30 |
| 11 | Battle Surge | Epic | 16s | 8s: Rage harcaması = HP regen |
| 12 | Death Blow | Epic | 12s | HP<%30: %400 execute, tüm Rage boşalt |

---

## 4. SECONDARY HAVUZLARI — "BORROWED POWER" (6 aktif/class)

Secondary seçilince Z/X'e giden bu skillerin tasarım prensibi:
- WB'nin yapamadığı şeyi yapmalı (range, mobility, CC, utility)
- Veya WB mekanikleriyle sinerji kurmalı
- Kendi kaynağını kullanmalı (Mana/Energy/Focus)
- Daha az ama daha güçlü hissettirmeli

### 4A. Elementalist Secondary (Warblade için Z/X havuzu)

| # | İsim | Tier | Kaynak | WB Sinerjisi |
|---|------|------|--------|-------------|
| 1 | Fireball | Common | Mana 15 | ✅ WB_Elem pasifi ile ateş+Rage |
| 2 | Blink | Rare | Mana 20 | ✅ Kaçış + sonraki spell +%20 |
| 3 | Glacial Spike | Rare | Mana 20 | ✅ Slow → Death Blow setup |
| 4 | Chain Lightning | Rare | Mana 25 | ⚠️ Standalone temizlik |
| 5 | Arcane Blast | Epic | Mana 30 | ✅ War Stomp knockup sinerji |
| 6 | Mirror Image | Rare | Mana 0 | ⚠️ Tank/distraction utility |

### 4B. Shadowblade Secondary (Warblade için Z/X havuzu)

| # | İsim | Tier | Kaynak | WB Sinerjisi |
|---|------|------|--------|-------------|
| 1 | Backstab | Common | Energy 20 | ✅ Iron Charge arkaya geçince 3x |
| 2 | Shadow Step | Rare | Energy 30 | ✅ Gap-close + 0.5s stun |
| 3 | Hemorrhage | Common | Energy 15 | ✅ Bleed + Rage (WB_Shadow pasifi) |
| 4 | Kidney Shot | Rare | Energy 35 | ✅ Stun → Death Blow setup |
| 5 | Evasion | Rare | Energy 25 | ✅ WB'nin savunma açığını kapatır |
| 6 | Fan of Knives | Rare | Energy 30 | ⚠️ AoE, Cleave alternatifi |

### 4C. Ranger Secondary (Warblade için Z/X havuzu)

| # | İsim | Tier | Kaynak | WB Sinerjisi |
|---|------|------|--------|-------------|
| 1 | Aimed Shot | Common | Focus 20 | ✅ Range opener → WB approach |
| 2 | Disengage | Common | Focus 15 | ✅ WB'nin kaçış eksikliği |
| 3 | Concussive Arrow | Common | Focus 10 | ✅ Knockback+root → Death Blow |
| 4 | Barbed Net Shot | Rare | Focus 20 | ✅ Root + bleed, Iron Charge sinerji |
| 5 | Explosive Trap | Rare | Focus 15 | ✅ Zemin kontrolü |
| 6 | Volley | Rare | Focus 25 | ⚠️ Standalone AoE temizlik |

---

## 5. PRIMARY DEĞİŞİNCE NE DEĞİŞİR — ÖRNEK

### WB Primary + SB Secondary
```
Kimlik: "Savaşçı, ama elini kirletmeyi biliyor"
LMB: Warblade 3-hit combo (Rage biriktirir)
Kaynak birincil: Rage (pasif regen + kill + skill)
Kaynak ikincil: Energy (skill kullanınca kazanılır, regen yok)
Q/E/R/F: WB skilleri (Iron Charge, Cleave vb.)
Z/X: SB skilleri (Backstab, Shadow Step vb.)
Otomatik pasif: CrossClassPassive_WB_Shadow (bleed → Rage sinerji)
Sinerji skill: Iron Phantom (iki sınıftan yatırım sonrası)
```

### SB Primary + WB Secondary
```
Kimlik: "Suikastçı, ama gerekince eziyor"
LMB: Shadowblade combo (hızlı, CP biriktirir, bleed uygular)
Kaynak birincil: Energy + Combo Points
Kaynak ikincil: Rage (sınırlı — sadece WB skilleri kullanınca kazanılır)
Q/E/R/F: SB skilleri (Hemorrhage, Backstab, Evasion, Fan of Knives vb.)
Z/X: WB skilleri (Iron Charge power move olarak, Cleave burst)
Otomatik pasif: CrossClassPassive_SB_WB (Rage dump → sonraki backstab güçlenir)
Sinerji skill: Crimson Surge (SB+WB'ye özel — farklı Iron Phantom'dan)
```

**Kritik fark:** WB primary'de Iron Charge bir "açılış hamlesi", SB primary'de Z'ye basınca "güç bombası" gibi hissettirmeli. Aynı skill, farklı ritim ve frekans.

---

## 6. TÜM CLASS MATRIX — FAZ BAZLI TASARIM PLANI

Her satır bir primary class. Her sütun secondary.
✅ = tasarlandı | 🔧 = taslak var | ❌ = tasarlanmadı

|  | WB | Elem | Shadow | Ranger | Ronin | Gunslinger | Ravager | Brawler | Summoner | Hexer |
|--|----|----|-------|--------|-------|-----------|---------|---------|----------|-------|
| **WB** | — | ✅ | ✅ | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| **Elem** | 🔧 | — | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| **Shadow** | 🔧 | ❌ | — | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| **Ranger** | 🔧 | ❌ | ❌ | — | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| **Ronin** | ❌ | ❌ | ❌ | ❌ | — | ❌ | ❌ | ❌ | ❌ | ❌ |
| **Gunslinger** | ❌ | ❌ | ❌ | ❌ | ❌ | — | ❌ | ❌ | ❌ | ❌ |
| **Ravager** | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | — | ❌ | ❌ | ❌ |
| **Brawler** | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | — | ❌ | ❌ |
| **Summoner** | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | — | ❌ |
| **Hexer** | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | — |

**Faz 1:** Sadece WB satırı (WB+Elem/Shadow/Ranger)
**Faz 2:** WB satırı tam + Elem/Shadow/Ranger satırları (WB secondary olarak)
**Faz 3:** Ronin/Gunslinger/Ravager/Brawler eklenir
**Faz 4-5:** Summoner/Hexer + tüm matrix

---

## 7. NET SINIRLAMALAR (KESİN KARAR)

### Aktif Skill Sınırları
| Kural | Değer |
|-------|-------|
| Max primary slot | 4 (Q/E/R/F) |
| Max secondary slot | 2 (Z/X) — secondary seçilince açılır |
| Toplam max aktif | 6 |
| Secondary seçimi | Run başında değil, oda 3-5 arası (trigger zone) |
| Secondary değiştirme | ❌ Bir kez seçilir, run boyunca kalır |
| Slot replace ücreti | Ücretsiz |

### Pasif Sınırları
| Kural | Değer |
|-------|-------|
| Max pasif sayısı | Sınırsız (slot yok) |
| Max seviye/pasif | 3 |
| Upgrade koşulu | Aynı pasifte tekrar çıkarsa seçilir |

### Trait Sınırları
| Kural | Değer |
|-------|-------|
| Max stack/trait | 3 |
| Kaynak | SADECE sandık + forge (draft'a girmez) |
| Toplam farklı trait | Sınırsız |

### LMB Ekol Sınırları
| Kural | Değer |
|-------|-------|
| Aynı anda aktif ekol | 1 |
| Forge #1 (Oda 4) | Seç — 3 ekol arası |
| Forge #2 (Oda 8) | Sadece yükselt (değiştirme yok) |
| Ekol switch | Sadece özel boss/forge ödülü |
| Max seviye | 3 |

### Sinerji Skill Sınırları
| Kural | Değer |
|-------|-------|
| Her combo için | 1 sinerji skill |
| Açılma koşulu | Secondary seçilmiş + her iki sınıftan ≥1 skill |
| Slot tipi | Aktif slot (Z/X veya primary'den birini iter) |
| Aynı anda | Max 1 sinerji skill |

### Relic Sınırları
| Kural | Değer |
|-------|-------|
| Max/run | 3 |
| Kaynak | Sadece boss kill |
| Incompatible pair | Yok (her kombinasyon alınabilir — oyuncu riski taşır) |

### Tier Unlock (run depth lock)
| Oda aralığı | Çıkabilir tier'lar |
|-------------|-------------------|
| 1-3 | Common, Rare |
| 4-6 | Common, Rare, Epic (ağırlık düşük) |
| 7+ | Common, Rare, Epic, Legendary |

---

## 7B. AUTO PASİF KURALI (review sonrası eklendi)

Her CrossClassPassive şu formatı taşır:
```
"[Primary aksiyonu] → [Secondary'den cevap]"
veya
"[Secondary aksiyonu] → [Primary'ye etki]"
```

Salt "+15 Rage" veya "+10 Mana" geçersiz.
Davranış tetiklenmeli: "Bleed kill → Rage", "Stealth çıkışı → spell güçlenir", "Root hedefe melee → Focus".

---

## 8. SECONDARY KAYNAK TASARIMI

Secondary class'ın kaynağı (Mana/Energy/Focus) birincil değil:
- Otomatik regen YOK
- Başlangıç değeri düşük (örn. 30/100)
- Sadece spesifik aksiyonla kazanılır:
  - Elem secondary: kill başına +5 Mana, veya birincil skill kullanınca +3 Mana
  - Shadow secondary: kill başına +8 Energy, backstab +15 Energy
  - Ranger secondary: uzak mesafe kill +10 Focus, hareket halinde +1/sn Focus

Bu "kısıtlı ama güçlü" hissi yaratır. Z/X her saniye basılmaz — birikiyor, sonra patlatıyorsun.

**Denge kuralı (review sonrası):**
Secondary kaynak bir oda içinde en az 2-3 kez Z/X kullanımına izin vermelidir.
"Spam" olmaz ama "hiç erişilemeyen güç" da olmaz.
Hedef: her combat döngüsünde 1 Z/X kullanımı doğal hissettirsin.

---

*Faz bazlı implementasyon: CURRENT_STATUS.md*
*Skill detayları: DRAFT_SKILL_TASARIM.md*
*Progression: PROGRESSION_SCHEMA_FINAL.md*
