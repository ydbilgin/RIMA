# RIMA — MAP + ITEM SYSTEM
> Final Karar Belgesi | 2026-04-12
> ChatGPT + Gemini review entegrasyonu → Claude final kararları

---

## BÖLÜM 1: HARİTA SİSTEMİ

### Temel Model

```
ACT başında STS2 tarzı tam graph arka planda üretilir.
Oyuncu hiçbir zaman grafın tamamını görmez.
Görünürlük kuralı:
  ✅ Ziyaret edilmiş   → tam görünür
  👁️ 1 adım ilerideki → oda tipi ikonu görünür, detay yok
  ❓ 2+ adım ilerisi   → fog / "?" — Scout mechanic ile açılabilir
```

**Neden bu model:**
- Tam harita görünürse optimal path bellidir → seçim gerçek hissetmez
- Sadece Hades kapısı olursa arka plan mantığı yoktur → lucky/unlucky seriler dominar
- Hibrit = oyuncu lokal kararlar alır, ama backend tutarlı üretim garantiler

---

### Graph Yapısı

**Sabitler (Destiny Nodes — her Act'ta aynı pozisyon):**
- Node 1-3: **Linear** (dal yok, oyuncu sistemi öğrenir)
- Node ~4: **Forge Room** (guaranteed, sabit)
- Node ~8: **Rest Node** (guaranteed, sabit)
- Node son: **Boss** (guaranteed, sabit)

**Parametreler (Act başında procedural):**
- Toplam node: ~15
- Dal sayısı: 3-4 (linear bölgeden sonra açılır)
- Elite oranı: **%20** (~2-3 elite/act)
- Path'lar mid-act'ta convergence noktasına gelir (iki dal birleşir, sonra boss'a gider)

**Path tradeoff şablonu:**
```
[Combat]─[Combat]─[Elite]──┐
                            ├──[Forge]──[Combat]──[Boss]
[Combat]─[Chest]─[Merchant]─┘
```
Üst yol: güçlenme (elite drop) ama risk  
Alt yol: güvenli ama item birikimi daha yavaş

---

### Scout Mechanic

**Map Fragment** — bir item/event sub-result olarak oyuncuya verilir:
- Chest Room'dan bazen "Map Fragment" çıkar (Heal / Gold / **Map Fragment** seçimi)
- Bazı Event sonuçları "keşif" verir
- Kullanınca: mevcut konumdan **2 adım ilerisi** ikon detaylarıyla görünür
- Etkisi kalıcı (görünür kalan fog açılmaz geri)

**Neden değerli:** "Elite mi var ileride, Merchant mı?" bilmek path seçimini gerçek stratejiye dönüştürür. Map Fragment = bilgi = güç; ayrı bir resource kategorisi.

---

### Oda Tipleri (8 tip)

| İkon | Oda | Ne olur | Drop |
|------|-----|---------|------|
| ⚔️ | **Combat** | Normal mob temizle | %20 component |
| 💀 | **Elite** | Güçlü mob + affix | 100% component (2 seçenek arasından seç) |
| 👑 | **Boss** | Act boss, Fracture Echo | Relic + class legendary seçimi (3 seçenek) |
| 📦 | **Chest** | Sandık: 3 seçenek | Heal / Gold / Component / Map Fragment |
| 🛒 | **Merchant** | 4 slot satış | Gold ile alım; rare: Anvil etkileşimi |
| 🔨 | **Forge** | 3 tab: Ecol/Skill/Item | Destiny node, visit başına 1 ana işlem |
| 📜 | **Event** | Narrative/risk event | Değişken; bazen Map Fragment |
| ☠️ | **Curse** | Yüksek risk, yüksek ödül | Legendary shortcut (detay: Item Bölümü) |

**Rest Node hakkında:**
Ayrı node olarak Act ortasında 1 guaranteed. Chest / Event / Merchant içinden bazen heal sub-result çıkar, ama garantili tam heal = sadece Rest Node. Kaynak çok kıt olmasın diye korundu.

**Curse Room dengesi:**
- Curse Room = "her zaman git" olmamalı
- Ödül: **%60 legendary şansı** (random class-specific legendary)
- Ceza: kalıcı curse debuff (örnekler: Max HP -%15, bir skill slot kilitlenir, crit -%10)
- Garantili değil: %40 ihtimalle sadece curse alır, item yoktur
- Seçim: bilinçli risk, meta değil

---

### Kapı UI (Hades Tarzı)

Oda temizlenince 2-3 kapı açılır. Her kapıda sonraki oda tipi ikonu görünür.

```
[⚔️ Combat]  [💀 Elite]  [📦 Chest]
     ↑ seç
```

Oyuncu ne seçtiğini bilir, ama o odanın içindeki detayı (hangi mob, ne drop) bilmez.

---

## BÖLÜM 2: ITEM SİSTEMİ

### Mevcut 5-Katman ile Ayrışma

| Katman | Ne | Kimlik |
|--------|-----|--------|
| LMB Ecol | Saldırı stili | Forge #1'de seçilir |
| Aktif Skill | Aktif güçler | Draft'tan |
| Pasif | Davranış modifier | Draft/auto |
| Trait | Build kimliği | Sandık only |
| Relic | Kural bozan | Boss + özel Event ONLY |
| **Item** | **Stat + sinerji, combine edilebilir** | **Combat / Forge / Merchant** |

**Relic ≠ Item ayrımı (kritik):**
- Item = ekonomik, craft edilebilir, kombinasyonla büyür ("+12% Phys + proc")
- Relic = nadir, kalıcı, oyun kurallarını büker ("Her 10 kill'de rage +20")
- Relic artık **sandıktan çıkmaz** — sadece Boss kill + nadir özel Event
- Chest'te relic = node çok güçlü olurdu, diğer nodeları gölgelerdi

---

### Item Slot Sistemi

**4 slot** (kesin, tartışmasız):
```
[Slot 1] [Slot 2] [Slot 3] [Slot 4]
```
- Component: 1 slot
- Combined: 1 slot (2 component tüketildi ama tek slot)
- Legendary: 1 slot (2 combined tüketildi ama tek slot) — **Legendary Slot ayrı değil, 4 slot içinden biri**

Yani legendary takan oyuncu: Legendary + 3 diğer item taşır.  
Combine = slot serbest kalır + güç artar → combine her zaman kazançlı.  
**Ama:** 4 slot dolduğunda yeni component için yer açmak zorundaydı → gerçek karar baskısı.

---

### Component Listesi (7 — Faz 1)

| Component | Pasif Efekt | Bağlı Debuff |
|-----------|-------------|--------------|
| Iron Shard | +6% Phys Dmg | — |
| Void Fragment | +6% Ability Power | RiftMark |
| Chain Links | +8% Armor | — |
| Shadow Veil | +6% Dodge | — |
| Blood Gem | +6% Life Steal | — |
| Rift Stone | +6% Crit | RiftMark |
| Soul Ember | +8% Attack Speed | — |

> Bone Dust (Elite bonus) + Mana Shard (Skill charge) → **Faz 2'de eklenir**  
> 9 component başlangıç için fazla; oyuncu önce 7'yi öğrensin

---

### Combined Item Listesi (9 — Faz 1)

**Kural:** Her combined item, bileşenlerinin efektlerini taşır + **yeni sinerji efekti** ekler.  
**Bağlanma kuralı:** Sinerji efektleri mümkün olduğunca mevcut debufflara hook olur (RiftMark, Sunder Mark, Toxic Stack).

| Formül | Sonuç | Efekt |
|--------|-------|-------|
| Iron Shard + Blood Gem | **Vampiric Blade** | +12% Phys + 18% LS + overkill = full momentary heal |
| Void Fragment + Shadow Veil | **Phantom Weave** | +12% AP + 12% Dodge + dodge → void burst proc |
| Rift Stone + Soul Ember | **Frenzy Core** | +14% Crit + 14% Atk Spd + crit → 0.5s haste |
| Iron Shard + Chain Links | **Warlord's Plate** | +10% Phys + 22% Armor + hit taken → +rage |
| Iron Shard + Rift Stone | **Rift Piercer** | +12% Phys + 12% Crit + **RiftMark stack başına armor ignore** |
| Blood Gem + Soul Ember | **Soul Tap** | +8% LS + 10% Atk Spd + on-kill → skill charge |
| Void Fragment + Rift Stone | **Fracture Amp** | +12% AP + 20% bonus dmg on **RiftMarked** enemies |
| Shadow Veil + Soul Ember | **Ghost Step** | +10% Dodge + 12% Atk Spd + dodge → phantom strike |
| Chain Links + Blood Gem | **Iron Will** | +20% Armor + 8% LS + skill use → brief shield burst |

> Bone Dust + Mana Shard kombinasyonları Faz 2'de eklenir.

---

### Legendary Sistem — BUILD ANCHOR

**Temel fikir:**
Legendary = run'ın "taç parçası". Oyuncu run'a başlarken hangi legendary alacağını bilmez; Boss ölünce class'ına özel **3 legendary seçeneği** sunar. Hangisini seçerse geri kalan item kararları o anchor'a göre şekillenir.

**Neden class-specific (cross-class değil):**
- 1 slot var, yanlış legendary = run zayıflar
- Cross-class suboptimal legendary oyuncuyu cezalandırır
- Class anchor = daha güçlü kimlik + build yönü netleşir
- Her class 3 seçenek → aynı class'ta farklı run tarzları

**Legendary'nin combined item'larla ilişkisi:**
Her legendary'nin "doğal" combined partner'ı var. Örn. Berserker's Gospel alırsan → Warlord's Plate + Rift Piercer doğal combo. Bu oyuncu keşfeder, zorunlu değil. Cross-class combined'larla da çalışır ama suboptimal.

---

### Legendary Craft Yolları

**Normal Yol (Forge Tab 3):**
```
Component + Component → Combined (Forge visit 1)
Combined + Combined → Legendary (Forge visit 2)
```
En az 2 Forge ziyareti gerekli. Planlama gerektiriyor.

**Boss Drop:**
Act Boss öldüğünde class'a özel 3 legendary seçeneği sunulur. Craft gerekmez.
Bu = guaranteed legendary yolu. Normal run'da boss = legendary kaynağı.

**Legendary Shortcut — Curse Room:**
```
Curse Room + 3 bare component bırak → Random class legendary al
                                       AMA kalıcı curse debuff al
```
**Shortcut nedir tam olarak:** Normal yolda 4 component topla, 2 combined yap, sonra legendary yap. Curse shortcut'ta bu adımları atlayıp sadece 3 ham parçayı kurban ederek legendary alırsın. Ama karşılığında run boyunca kalıcı bir ceza alırsın.

**Curse debuff örnekleri:**
- Max HP -%15
- 1 skill slot kilitlenir (açık slotlardan biri)
- Crit şansı -%10 (combined'lardan kazandığın dahil)
- Merchant fiyatları +%30
- Crit şansı -%10 ama hareket hızı +%15 (tradeoff curse)

**Risk:** %40 ihtimalle sadece curse alır, legendary yoktur.  
**Sonuç:** "Erken legendary = run kazanırım" değil. "Şu an kritik noktadayım, risk alıyorum" kararı.

---

### Forge Room Kuralları

**3 Tab:**
1. LMB/RMB Ecol seçim + yükseltme
2. Skill upgrade
3. Item combine (Component→Combined, Combined→Legendary)

**Visit başına 1 ana işlem:**
- Ya ecol değiştir/yükselt
- Ya skill yükselt
- Ya item combine yap
- Aynı ziyarette iki işlem yok
- Inventory bakma (item detayları görme) her zaman serbest

**Neden:** Forge'un her ziyareti değerli karar noktası olsun. "Şimdi skill mi yoksa item mi?" gerçek seçim.

---

### Merchant + Anvil

**Merchant Satışı:**
- 2-3 component (rastgele)
- Nadir: 1 combined item (pahalı)
- Nadiren: Map Fragment
- **Anvil etkileşimi** (rare, pahalı): Forge'a gitmeden 1 item combine yap

**Anvil:** Haritada Forge'a uzak kalınca safety valve. "Forge'a yetişemedim, combine yapamadum" dead-end'ini önler. Pahalı tutulur ki "her zaman Anvil git" olmasın.

---

### Drop Ekonomisi

| Kaynak | Drop |
|--------|------|
| Normal Combat | %20 random component |
| Elite Combat | 100% component (2 seçenekten seç) |
| Boss | Relic + class legendary seçimi (3 seçenek) |
| Chest | Heal / Gold / Component / Map Fragment (seçim, combined nadiren) |
| Merchant | 2-3 component; rare combined; rare Anvil |
| Event | Özel item, Map Fragment, bazen Relic |
| Curse | Legendary shortcut (risk) |

---

## BÖLÜM 3: CLASS LEGENDARY TABLOSU

Her class için **3 legendary**. Boss öldüğünde 3'ü sunulur, 1'i seçilir.  
Her legendary = 1 build anchor (o class'ın farklı oynanış yönü).

---

### WARBLADE *(Faz 1 — detaylı)*

| Legendary | Build Anchor | Efekt |
|-----------|-------------|-------|
| **Berserker's Gospel** | Rage management | Fury (50) ve Bloodrage (100) threshold'larına girerken 2s invincibility frame + tüm skill CD'leri -%30. Rage max'ta kalındığı sürece hasar +%20. Doğal partner: Warlord's Plate + Rift Piercer |
| **Crimson Covenant** | Aggression/sustain | Life steal cap kaldırılır. Overkill hasar anında Rage'e dönüşür. Her 10 kill'de en uzun CD'li skill sıfırlanır. Doğal partner: Vampiric Blade + Soul Tap |
| **Ironclad Edict** | Brawler/punish | Hasara uğrayınca Rage gain ×3. Alınan hasarın %25'i 2 saniye gecikmeli counter olarak geri döner. Counter hasar Sunder Mark tetikler. Doğal partner: Iron Will + Warlord's Plate |

---

### ELEMENTALIST *(Faz 2)*

| Legendary | Build Anchor | Efekt |
|-----------|-------------|-------|
| **Convergence Matrix** | Multi-element combo | Farklı 3 elementten vuruş sonra → fusion explosion (tüm elementlerin toplam hasarı). AP scaling +%15. |
| **Arcane Singularity** | Mono-element mastery | Tüm aktif skill'ler aynı elementte ise hasar +%40. Element değiştirilirse 5s boyunca bonus kaybolur. |
| **Mirror Cascade** | Summon/explosion | Mirror Image +1 kopya. Kopyalar öldüğünde chain elemental explosion. Kopya sayısına göre passive AP artar. |

---

### SHADOWBLADE *(Faz 2)*

| Legendary | Build Anchor | Efekt |
|-----------|-------------|-------|
| **Void Predator** | Burst build | Stealth'ten ilk vuruş guaranteed crit + poison stack max. Stealth süresi +1s. |
| **Hemorrhage** | Stacking DoT | Poison stack'leri bleed'e dönüşebilir (aktif skill ile tetikle). DoT composite hasar artar. |
| **Phantom Protocol** | Mobility loop | Her dodge = 0.5s mini-stealth. Stealth içinde kill yapılırsa stealth CD sıfırlanır. |

---

### RANGER *(Faz 2)*

| Legendary | Build Anchor | Efekt |
|-----------|-------------|-------|
| **Eagle's Requiem** | Precision | Focus max'a ulaşırsa sonraki ok guaranteed headshot (instant crit, bonus hasar). |
| **Volley Tyrant** | Sustained DPS | Multi-shot +2 ok. Her ok ayrı wound stack sayar. Wound stack başına hasar artar. |
| **Tether Sovereign** | CC/AoE | Tethering Arrow süresi ×2. Tethered düşmanlar aynı hasar havuzu paylaşır (birine vur, hepsi alır). |

---

### RONIN *(Faz 3)*

| Legendary | Build Anchor | Efekt |
|-----------|-------------|-------|
| **Void Resonance** | Tension burst | Max tension'da release = 3 phantom strike (görünmez ek vuruş). Void Cleave cone +%30 hasar. |
| **Sheathed Storm** | Speed iaido | Draw hızı ×2. Sheath/draw döngüsü her 3 kez = bedava skill cast. |
| **Cursed Edge** | Persistent tension | Her hit tension decay durdurur. Tension max'ta donup kalır. Max tension'da saldırılar bleed stack ekler. |

---

### GUNSLINGER *(Faz 3)*

| Legendary | Build Anchor | Efekt |
|-----------|-------------|-------|
| **Overheat Protocol** | Controlled overheat | Overheat = AoE explosion + heat 50'ye reset (sıfırlanmaz). "Kontrolsüz" overheat artık yok. |
| **Rift Archer** | Mobility | Rift Dash mesafesi ×1.5. Dash sonrası 2s boyunca tüm atışlar armor-piercing. |
| **Chrome Dominion** | Heat accumulation | Her 10 Heat = +%1 hasar (max +%15 @150 Heat). Ricochet hit başına +5 Heat. |

---

### RAVAGER *(Faz 3)*

| Legendary | Build Anchor | Efekt |
|-----------|-------------|-------|
| **Ruin Eternal** | Infinite berserk | Berserk Mode'da kill-gate CD reset limiti kaldırılır. Her kill berserk süresini +0.5s uzatır. |
| **Warlord's Grip** | Mass aggro | Taunted düşman sayısı başına hasar +%10 (max +%40). Taunt süresi ×1.5. |
| **Gore Tide** | Sustain berserk | Berserk Mode'da hasar aldığında flat heal (life steal değil). Berserk'te ölmeden önce 1 kez survive (run başına 1). |

---

### BRAWLER *(Faz 3)*

| Legendary | Build Anchor | Efekt |
|-----------|-------------|-------|
| **Knockout Theory** | Precision combo | Perfect timing window +0.15s. Her perfect hit +2 Charge. Perfect hit serisi kopmazsa hasar artar. |
| **Unstoppable Mass** | Bulldozer | Charge hareketi ×2 hız. Charge hit = 0.5s stun. Charge mesafesine göre hasar artar. |
| **Iron Knuckle** | Sustained combo | Combo sayacı interrupt'a rağmen 3s boyunca korunur (resetlenmez). Max combo threshold bonusları güçlenir. |

---

### SUMMONER *(Faz 4)*

| Legendary | Build Anchor | Efekt |
|-----------|-------------|-------|
| **Lich Ascension** | Lich sustain | Lich Form süresi ×2. Lich Form'da minyon HP drain yerine yakın düşman HP drain. |
| **Swarm Intelligence** | Minion army | Aktif minyon limiti +2. Minyonlar proximity'de birbirini buff eder (+%10 hasar/komşu). |
| **Soul Nexus** | Sacrifice engine | Her minyon ölümünde 5s boyunca +%15 hasar (stack, max 3). Minyon sacrifice CD -%50. |

---

### HEXER *(Faz 4)*

| Legendary | Build Anchor | Efekt |
|-----------|-------------|-------|
| **Mirror Abyss** | Reflection damage | Empathy reflection hasarı %150. Reflection AoE patlama yapar. |
| **Hex Convergence** | Multi-hex | 3 farklı hex aktifken hasar aura (passive, yakın düşmanlar hasar alır). |
| **Soul Debt** | Corruption escalation | Soul Bargain acceleration stack'leri kalıcı (run boyunca birikerek artar). Stack başına AP +%2. |

---

## ÖZET — FINAL KARARLAR

| Konu | Karar |
|------|-------|
| Harita modeli | STS2 graph backend + Hades kapı ikonları + fog of war |
| İlk 3 node | Linear (forced), sonrası dallanır |
| Elite oranı | %20 |
| Rest | Act başına 1 guaranteed destiny node |
| Scout | Map Fragment / Event sub-result → 2 adım görünür |
| Curse Room | %60 legendary şansı, %40 sadece curse; ceza kalıcı |
| Oda tipi | 8 tip (Rest node dahil) |
| Component | 7 (Faz 1), +2 Faz 2 |
| Combined | 9 (Faz 1) + debuff-hook kuralı |
| Legendary | Class-specific, 3 seçenek/class, Boss = guaranteed |
| Legendary craft | 2 combined → 1 legendary (Forge) |
| Legendary shortcut | Curse Room = 3 bare component + risk → random legendary |
| Slot | 4 (legendary dahil, ayrı değil) |
| Forge | 3 tab, visit başına 1 ana işlem |
| Anvil | Merchant'ta rare + pahalı safety valve |
| Relic kaynağı | Boss + nadir özel Event ONLY (sandıktan kaldırıldı) |
| Drop ekonomisi | Normal %20, Elite 100% seçimli, Boss = Relic+Legendary seçimi |
