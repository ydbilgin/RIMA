---
status: REFERENCE
faz: 1
tarih: 2026-05-14
ozet: "RIMA referans dokumani"
---
# FAZ 3 — SECONDARY CLASS BREAK

*Updated: 2026-04-29 — S43 canonical class list applied*

*Claude: Sadece bu dosyayı oku. Faz 1-2 tamamlanmış varsay.*

---

## SCOPE

**Hedef:** Act 1 boss sonrası secondary class seçimi, +4 yeni class (Ravager, Ronin, Gunslinger, Brawler), +2 slot, cross-class pasif, class-specific parry/deflect mechanics, Spirit Encounter, Act 2 başlangıcı.

**Ne VAR (Faz 2'ye ek):**
- **+4 yeni class: Ravager, Ronin, Gunslinger, Brawler**
- **Class-specific parry/deflect mechanics** (see SINIF_VE_SKILL_KARAR_BELGESI.md per class)
- Secondary class seçim mekanizması (Act 1 boss sonrası)
- +2 aktif slot (toplam 6)
- Cross-class pasif sistemi (56 kombo: 8×7, her kombo için 1 pasif)
- Mixed draft oranları (Act 2'de iki class'tan kart çıkıyor)
- Spirit Encounter (3 tip: Forge Wraith, Shadow Hound, Blood Oracle)
- Epic skill tier
- Act 2 başlangıcı (ilk 3-4 oda)
- +2 mob: Echo Hound, Fracture-Born
- Echo Twin boss (Act 2 sonu — tam 2 faz)

**Ne YOK:**
- Kalan 2 class (Summoner, Hexer → Faz 4)
- Cross-class Ultimate
- Legendary tier
- Curse Gate, Event odası
- Meta-progression harcama
- Grudge sistemi
- Fracture Echoes (Faz 4)

---

## SECONDARY CLASS MEKANİĞİ

### Kırılma Noktası (Act 1 Boss Sonrası)

```
1. Penitent Sovereign öldürüldü
2. Geçiş sahnesi: zemin çatlar, rift açılır
3. 2 random secondary class kartı belirir
4. Oyuncu 1 seç → secondary class aktive olur
5. +2 yeni aktif slot açılır (Q E R F → + 1, 2)
6. Cross-class pasif otomatik aktive olur
7. Act 2 başlar
```

### Secondary Class Offer Kuralları
- Primary class tekrar teklif edilmez
- 7 class'tan 2'si rastgele gösterilir (Warblade hariç: Elementalist, Shadowblade, Ranger, Ravager, Ronin, Gunslinger, Brawler)
- Her kartın üstünde: class ismi + core fantasy + imza skill + kaynak tipi

### Draft Oranları (Act 2)

| Aşama | Primary | Secondary | Nötr |
|-------|---------|-----------|------|
| Act 2 başı | %60 | %25 | %15 |
| Act 2 ortası | %50 | %35 | %15 |

---

## CROSS-CLASS PASİFLER

Secondary class seçilince otomatik aktive olur. Her combinasyon için benzersiz:

| Primary | Secondary | Arketip | Pasif Efekt |
|---------|-----------|---------|-------------|
| Warblade | Elementalist | **Runeguard** | Rage 50+: spell'ler %20 bonus hasar verir |
| Warblade | Shadowblade | **Iron Shadow** | Stun'daki düşmana vurunca Energy +15 |
| Warblade | Ranger | **Vanguard** | Iron Charge mesafesi Focus kazandırır |
| Elementalist | Warblade | **Runeguard** | Fire DoT aktif düşmana melee: Rage +10 |
| Elementalist | Shadowblade | **Arcane Shadow** | Blink sonrası 2s stealth |
| Elementalist | Ranger | **Stormchaser** | Frozen/slowed hedeflere Focus decay yok |
| Shadowblade | Warblade | **Iron Shadow** | Backstab crit: Rage +20 |
| Shadowblade | Elementalist | **Arcane Shadow** | Stealth'ten çıkınca Mana +30 |
| Shadowblade | Ranger | **Phantom Archer** | Stealth'ten ranged saldırı: Focus +25 |
| Ranger | Warblade | **Vanguard** | Point Blank knockback'li hedefe Rage +15 |
| Ranger | Elementalist | **Stormchaser** | Focus 100'de Fireball'lar 2 hedefe sekiyor |
| Ranger | Shadowblade | **Phantom Archer** | Disengage sonrası 1.5s stealth |

> **Implementation:** `CrossClassPassiveData.asset` (ScriptableObject) — 12 kombinasyon × primary/secondary yönü

---

## SPİRİT ENCOUNTER (3 Tip)

### Forge Wraith 🔥
**Tema:** Saldırı, zırh kırma
**Koşul:** Koşulsuz — her zaman 2-3 seçenek
**Offer örnekleri:**
- "Silah Bileme": Sonraki 3 oda tüm hasar +%15
- "Çatlak Darbesi": Bir sonraki attack skill armor ignore
- "Demir Hatıra": Max HP +5

### Shadow Hound 🐾
**Tema:** Hareket, reposition
**Koşul:** Son odayı 5 saniyede temizlediysen +1 seçenek
**Offer örnekleri:**
- "Kaçış İzi": Dash CD -%20
- "Gölge Hız": 8s hareket hızı +%30
- "Yankı Adımı": Dash sırasında dodging frame +0.5s

### Blood Oracle 🩸
**Tema:** HP trade, lifesteal
**Koşul:** HP %60 altındaysa en güçlü seçenek görünür
**Offer örnekleri:**
- "Kan Paktı": HP -%15, tüm hasar +%25 (kalıcı, run süresince)
- "Sızıntı": Her kill'den +3 HP
- *(HP %60 altı)* "Son Damlam": HP -%30, tüm skill'ler 1 tier atladı

---

## MOB'LAR (Faz 2'ye ek — Act 2)

### Yeni Grunt (96px)

| Mob | Act | Mekanik | Sprite |
|-----|-----|---------|--------|
| **Echo Hound** | 2, 3 | Hızlı blink + echo afterimage (iki noktadan hasar), ses çeker | ❌ Üretilecek |

### Yeni Elite (160px)

| Mob | Act | Mekanik | Sprite |
|-----|-----|---------|--------|
| **Fracture-Born** | 2, 3 | 4 aşamalı spawn (zemin çatlağından çıkış), spawn sırasında öldürülebilir | ❌ Üretilecek |

### Act 2 Mob Dağılımı

| Mob | Act 2'de Var | Tier |
|-----|-------------|------|
| ShardWalker (mor renk) | ✅ | Grunt |
| VoidThrall | ✅ | Grunt |
| Penitent | ✅ | Grunt |
| ChainWarden | ✅ | Grunt |
| SeamCrawler | ✅ | Grunt |
| **Echo Hound** | ✅ (YENİ) | Grunt |
| Hollow Mite | ✅ | Swarm |
| Twice-Born | ✅ | Elite |
| **Fracture-Born** | ✅ (YENİ) | Elite |

---

## BOSS: ECHO TWİN (Act 2 — 2 Faz)

**Tema:** Oyuncunun dual-class dönüşümünün yansıması.

### Faz 1 — "Birinci Kimlik" (HP: 100% → 40%)

Boss'un primary class'ı oyuncunun seçimine göre değişir:
- Warblade → melee agresif
- Elementalist → ranged mage
- Shadowblade → stealth/burst
- Ranger → kiting saldırgan

| Saldırı | Mekanik |
|---------|---------|
| **Mirror Slash** | Oyuncunun son skill'inin ayna kopyası |
| **Identity Strike** | 5m dash + hasar |
| **Echo Barrier** | 3s kalkan |
| **Twin Pulse** | 2m AoE patlama + itme |

### Faz 2 — "İkinci Kimlik" (HP: 40% → 0%)

**Geçiş sahnesi (2s):** İkiye yırtılıp birleşir, renk paleti kayar.

| Saldırı | Mekanik | Amacı |
|---------|---------|-------|
| **Duality Surge** | Her iki kimliğin gücü — büyük AoE | Cross-class kullanmayı teşvik |
| **Phase Shift** | Görünmez → arkana çıkar | Pozisyon yönetimi |
| **Echo Cascade** | F1 saldırılarını 2× hızda tekrarlar | Öğrenilmiş patternları test |
| **Twin Collapse** | 3s enerji toplama → patlama | Burst window |

---

## SİSTEMLER

| Sistem | Açıklama |
|--------|----------|
| **SecondaryClassSelector.cs** | Boss sonrası 2 kart offer → seçim |
| **SlotUnlock.cs** | +2 slot açılışı animasyonu + HUD güncelleme |
| **CrossClassPassiveData.asset** | 12 kombo × ScriptableObject |
| **CrossClassPassiveManager.cs** | Pasifi aktive et, buff uygula |
| **MixedDraftCalculator.cs** | Act 2 draft oranları (Primary/Secondary/Nötr) |
| **SpiritEncounterManager.cs** | Spirit oda mantığı + koşullu offer |
| **SpiritOfferUI.cs** | Spirit offer seçim ekranı |
| **Epic Skill Tier** | Rare'den 1 tier: +%60 güç + anlamlı mekanik değişim |
| **Act2MapGenerator.cs** | 8-9 oda, Act 2 tileset (koyu mor palet) |

---

## ANİMASYON BÜTÇESİ

| İçerik | PixelLab Gen |
|--------|-------------|
| Echo Hound tüm set | ~32 gen |
| Fracture-Born (spawn anim + tam set) | ~50 gen |
| Echo Twin boss tüm set (2 faz × 4 saldırı + geçiş) | ~80 gen |
| Act 2 tileset (koyu mor bataklık) | ~20 gen |
| Spirit NPC sprite'ları (3 tip) | ~18 gen |
| **Toplam** | **~200 gen** |

---

## ÇIKIŞ KRİTERLERİ

- [ ] Act 1 boss sonrası 2 secondary class kartı sunulur
- [ ] Seçim yapılınca +2 slot açılır, cross-class pasif aktive olur
- [ ] Act 2'de mixed draft çalışır (iki class'tan kart geliyor)
- [ ] Spirit Encounter odası: koşullu offer çalışır (hız bonus, HP koşulu)
- [ ] Epic tier: skill mekanik değişimi çalışır
- [ ] Echo Twin 2 faz — geçiş sahnesi + adaptive saldırılar
- [ ] Tam run: Act 1 (6-7 oda) + Act 2 (8-9 oda) + 2 boss = 30 dakika+

