# FAZ 5 — FULL GAME (Early Access)

*Claude: Sadece bu dosyayı oku. Faz 1-4 tamamlanmış varsay.*

---

## SCOPE

**Hedef:** Act 1-2-3 + Final Boss tam, Fracture Echoes (tüm bosslar), Grudge sistemi, zorluk modları, tam meta-progression. Early Access'e hazır.

**Ne VAR (Faz 4'e ek):**
- Act 3 tam (10-11 oda)
- Final Boss — The Architect (4 faz)
- Fracture Sovereign tam (3 faz)
- **Fracture Echoes** tüm bosslar (5 echo/boss × 4 boss = 20 echo toplam)
- 90 cross-class pasif + 90 cross-class ultimate (10×9 tam set)
- Grudge / Nemesis sistemi
- Zorluk modları (Echo / Rift / Fracture / Void)
- +3 mob: Rift Maw, Class Mimic, Remnant Host
- 2 ek Spirit: Void Seer, Fallen Champion
- Ancient Relic spirit (güçlü ama bedelli)
- Lokalizasyon altyapısı (TR/EN)

> **Not:** Tüm 10 class Faz 3-4'te zaten eklenmiş. Bu fazda yeni class YOK.

---

## YENİ CLASS'LARIN DETAYLARI

> **Not:** Ravager, Crusader, Lancer, Gunslinger → Faz 3'te eklendi.
> Summoner, Hexer → Faz 4'te eklendi.
> Bu fazda yeni class eklenmez. Skill tabloları için: `../SINIF_VE_SKILL_KARAR_BELGESI.md`
> Lancer + Gunslinger detayları: `../MASTER_KARAR_BELGESI.md`

---

### 💀 SUMMONER

**Core Fantasy:** "Ben savaşmıyorum. Feda ediyorum."
**Kaynak:** Charges (0-4, +1/8s, minyon ölünce +1 anında)
**[V] Burst:** ARMY OF THE DEAD — tüm Charge dolu: 6s minyonlar +%150, ölümsüz
**Unlock:** 200 Echo VEYA üst üste 3 run'da Act 2'ye ulaş

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | **Raise Skeleton** ★ | ↑▶ | Core | 1 Charge → melee iskelet (max 3) | 3 iskelet → sonraki +%20 hasar |
| 2 | **Summon Golem** | ⚓⬡ | Core | 2 Charge → tank Golem, HP<%20 patlama AoE | — |
| 3 | **Rally Cry** | ✦ | Core | Tüm minyonlar +%20 hasar+hız 10s | Karışık tip → +%40 |
| 4 | **Corpse Explosion** | 💥⚡ | Core | Cesedi patlatır, AoE | 3+ cesetle → zincir patlama |
| 5 | **Death Nova** | ⚡⬡ | Core | 1 minyon feda: 8s zehir bulutu | — |
| 6 | **Commanding Strike** | ✦⬡ | Core | Seçili minyon ×4 hasar+invuln | Golem'e emir → duvara çarpar stun |
| 7 | **Blood for Power** | ↑↓ | Core | Minyon feda → Charge+1 + CD -%30 | 3 feda → Charge+3 + Burst meter +30 |
| 8 | **Bone Shield** | ⚓↑ | Core | 3s: minyon kalkan, hasar absorbe | Golem kalkan → ×2 absorbe |
| 9 | **Soul Siphon Totem** | ↑⚓ | Advanced | 8s totem: 5m'den ruh emer, 0.5 Charge/sn | Yakınında minyon ölürse → totem patlar, +2 Charge |
| 10 | **Mass Sacrifice** | 💥⬡ | Advanced | Tüm minyonları patlatır, her biri %150 AoE | 3+ minyon → 3s grounded + Summoner +2 Charge |
| 11 | **Dark Pact** | ↑ | Advanced | HP -%12 → Charge olmadan minyon çağır | — |
| 12 | **Lich Form** | ✦⚓ | Master | 10s: melee immune, minyonlar +%60 | Feda sırasında → hasar ×3 |

**Build Eksenleri:** Sacrifice Engine / Army Commander / Lich Burst

**Ek Sprite:** Skeleton minion (48px) + Golem (80px) — ayrı sprite setleri

---

### 🔮 HEXER

**Core Fantasy:** "Sabır. 10'a gelince sen bitiyorsun."
**Kaynak:** Hex Stacks (0-10/düşman, 5s decay)
**Faz sistemi:** 0-3 Debuff / 4-6 Pressure (+%20 güç) / 7-9 Overload (+%30 hasar alır) / 10 HEXBLAST
**[V] Burst:** HEX CASCADE — 10 stack: tüm düşmanlara 3 stack kopyalanır
**Unlock:** 200 Echo + Elementalist ile run bitir

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | **Corruption** ★ | ▶↑ | Core | Anında 3 stack + 4s DoT | Corruption→Agony → Agony ×2 hızlı |
| 2 | **Agony** | ↑ | Core | Süregelen DoT, 2 stack/tick | Pressure Phase → 3 tick |
| 3 | **Pandemic** | ✦⚡ | Core | Bir hedefteki TÜM stack'ları yakına kopyalar | Overload hedefe → kopyalanan +3 |
| 4 | **Hexblast** | 💥↓ | Core | 10 stack: %100/stack, kill=CD sıfır | 3+ Pressure → zincirleme tüm odaya |
| 5 | **Empathy** | ⚡⬡ | Core | Lanet: saldırıların %30'u kendine döner | Overload → %60 |
| 6 | **Haunt** | ↑⬡ | Core | Hayalet bağla: takip+tick+3 stack, 10=Hexblast | — |
| 7 | **Unstable Affliction** | 💥⚡ | Core | Dispel/heal → patlama+stun | CC altında → guaranteed full stack |
| 8 | **Enervate** | ⬡✦ | Core | Hız -%50, saldırı hızı -%40, 10s | 5+ stack → süre ×2 |
| 9 | **Mass Hex** | ▶⬡ | Advanced | TÜM düşmanlara 2 stack, HP -%8 | Pressure Phase → 3 stack |
| 10 | **Hex Overload** | ⚡💥 | Advanced | 6s: 4+ stack düşmana her hasar +1 stack, Hexblast ×2 | Corruption sonrası → 10s |
| 11 | **Cursed Mirror** | ⚡↑ | Advanced | 8s: sana her debuff → düşmana %50 yansır | — |
| 12 | **Soul Bargain** | 💥↓ | Master | HP -%25 → anında 5 stack | Overload hedefe → anında 10, Hexblast |

**Build Eksenleri:** Patient DoT / Hex Overload / Soul Burst

---

## CLASS UNLOCK SİSTEMİ

| Class | Nasıl Açılır |
|-------|-------------|
| Warblade | Başlangıçta açık |
| Elementalist | 80 Echo |
| Ranger | 80 Echo |
| Shadowblade | 150 Echo VEYA Act 1'i 3 kez tamamla |
| Ravager | 150 Echo VEYA Warblade ile Act 2 boss öldür |
| Crusader | 150 Echo VEYA 3 farklı class ile Act 1'i tamamla |
| Lancer | 200 Echo VEYA Warblade+Ranger ile run bitir |
| Gunslinger | 200 Echo VEYA Ranger ile Act 2'ye ulaş |
| Summoner | 200 Echo VEYA üst üste 3 run'da Act 2'ye ulaş |
| Hexer | 250 Echo + Elementalist ile run bitir |

---

## GRUDGE / NEMESİS SİSTEMİ

Mob'lar nasıl öldüğünü hatırlıyor:
- **ShardWalker** yakıldığında → sonraki ShardWalker ateş direnci + ateşli görünüm
- **VoidThrall** büyüyle öldürülünce → void enerji koyulaşır, büyü direnci
- **Class Mimic** → zaten Grudge'ın formü: senin taktiklerini kopyalıyor
- Boss'a 3+ kez öldüysen → o boss o kill method'una +%35 direnç

---

## YENİ MOB'LAR

| Mob | Act | Tier | Mekanik |
|-----|-----|------|---------|
| **Rift Maw** | 3 | Elite (160px) | Sabit, çekim alanı, düşman spawn'lar |
| **Class Mimic** | 2,3 | Special (128px) | Oyuncunun class skill'lerini kopyalar |
| **Remnant Host** | 3 | Special (160px) | 3 ruh, her 15s ruh değişir, farklı direnç/saldırı |

---

## BOSS: FRACTURE SOVEREİGN (Tam — 3 Faz)

### Faz 1 — "Yara Açıldı" (100% → 60%)
> Faz 4'ten aynen.

### Faz 2 — "Çevre Uyanıyor" (60% → 30%)
Arena köşelerinde Fracture Node aktive olur. Arena küçülür.

| Saldırı | Mekanik |
|---------|---------|
| **Node Pulse** | 15s'de nodlar patlar |
| **Fracture Wave** | Zemin dalgası — atlama penceresi |
| **Shatter Field** | Dönen orbs, temas = slow |
| **Sovereign Gaze** | 3s işaret + büyük hasar |

**Ek:** Node'ları vurarak öldürebilirsin → boss'a %5 hasar.

### Faz 3 — "Tam Kırılma" (30% → 0%)
Arena tamamen değişir: zemin "boşluk"a dönüşür, platform adacıkları kalır.

| Saldırı | Mekanik |
|---------|---------|
| **Fracture Surge** | Önceki tüm saldırılar 2× hızda |
| **Void Collapse** | Arena bölümü çöker |
| **Sovereign's Final Form** | Tüm alana hasar — belirli platforma geçmek zorunlu |
| **Echo of Pain** | Son büyük hasarı geri fırlatır |

---

## BOSS: THE ARCHİTECT (Final — 4 Faz)

### Faz 1 — "Tanıdık" (100% → 75%)
Act 1 boss saldırılarını mirror'lar. Ne kadar ilerlediğini hissettirir.

### Faz 2 — "Bozulan" (75% → 45%)

| Saldırı | Özellik |
|---------|---------|
| **Fractured Mirror** | Son 3 skill'ini kopyalar |
| **Glitch Step** | 4-5 rapid teleport → saldırı |
| **Architect's Echo** | Geçmiş boss saldırılarından rastgele |
| **Identity Collapse** | 3s hasar kaynakları yer değiştirir |

### Faz 3 — "Boşluk" (45% → 20%)
Arena karanlığa gömülür. Sessizlik → yeni müzik.

| Saldırı | Mekanik |
|---------|---------|
| **Void Architecture** | Yeni platformlar (bazıları tuzak) |
| **Gravity Inversion** | 5s ters yürüüş |
| **The Blueprint** | Zemine pattern → 3s'de aktive |
| **Silence** | 4s tüm skill bloklanır — sadece hareket |

### Faz 4 — "Gerçek Form" (20% → 0%)
Architect küçülür — bir insan. Tema müziği başlar.
Tüm önceki mekanikler aynı anda:

| Saldırı | Mekanik |
|---------|---------|
| **Fracture Everything** | Rastgele 3 eski boss saldırısı arka arkaya |
| **The Original Sin** | Basit, yavaş, gülünç derecede güçlü |
| **Build Breaker** | En çok kullanılan skill 8s devre dışı |
| **Final Architecture** | Büyük AoE + yenilemez — hareket ederek hayatta kal |

---

## EK SPİRİT'LER

| Spirit | Tema | Koşul | Act |
|--------|------|-------|-----|
| **Void Seer** | Kaos, şans | İkonlar kapalı — kör seçim | 2, 3 |
| **Fallen Champion** | Kombo, zincir | Son 3 odayı chain ile geçtiysen unlock | 3 |
| **Ancient Relic** | Güçlü ama bedelli | Her offer'a curse eşlik eder | 3 |

---

## ZORLUK MODLARI

| Mod | Açıklama | Unlock |
|-----|----------|--------|
| **Echo** (kolay) | Düşman HP -%25, cooldown -%15 | Başlangıçta |
| **Rift** (normal) | Standart | Başlangıçta |
| **Fracture** (zor) | Düşman HP +%50, Elite frekansı ×2 | Act 3'e ulaş |
| **Void** (permadeath) | Ölünce: hub'daki tüm meta-progress sıfırlanır | Tüm 10 class ile run bitir |

---

## ANİMASYON BÜTÇESİ

| İçerik | PixelLab Gen |
|--------|-------------|
| ~~Ravager~~ (Faz 3'te) | — |
| ~~Crusader~~ (Faz 3'te) | — |
| ~~Lancer~~ (Faz 3'te) | — |
| ~~Gunslinger~~ (Faz 3'te) | — |
| ~~Summoner~~ (Faz 4'te) | — |
| ~~Hexer~~ (Faz 4'te) | — |
| Rift Maw sprite + anim | ~24 gen |
| Class Mimic (10 class varyant) | ~100 gen |
| Remnant Host (3 ruh formu) | ~48 gen |
| Fracture Sovereign F2-F3 | ~60 gen |
| The Architect (4 faz) | ~120 gen |
| Act 3 tileset (siyah + altın) | ~20 gen |
| Fracture Echoes VFX (20 echo × görsel) | ~40 gen |
| **Toplam (sadece Faz 5)** | **~412 gen** |

---

## ÇIKIŞ KRİTERLERİ

- [ ] 10 class seçilebilir ve oynanabilir
- [ ] Act 1 + Act 2 + Act 3 + Final: tam run 45-60 dakika
- [ ] 90 cross-class pasif + 90 ultimate çalışır
- [ ] Fracture Echoes: tüm 4 boss × 5 echo = 20 echo çalışır
- [ ] Grudge: mob'lar öldürme yöntemine göre adaptasyon yapıyor
- [ ] Class unlock: Echoes ile açılıyor
- [ ] 4 zorluk modu çalışır
- [ ] Hub'da 4 NPC etkileşimi anlamlı
- [ ] Lokalizasyon: TR + EN
- [ ] **Bu build Steam Early Access olarak yayınlanabilir**

---

## POST-LAUNCH EXPANSION (Ayrı Dönem)

Bu class'lar Faz 5 sonrasında gelir:

| Class | Fantasy | Kaynak |
|-------|---------|--------|
| **Tempest** | "Durursam ölürüm. İvmem silahım." | Momentum (hareketle dolar) |
| **Hemomancer** | "Canım en keskin silahım." | HP + Blood Pool |

> Tam skill tabloları: `SINIF_VE_SKILL_KARAR_BELGESI.md` §9-10
