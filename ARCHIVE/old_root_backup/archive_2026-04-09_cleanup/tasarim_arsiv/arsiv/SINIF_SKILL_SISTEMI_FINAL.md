# SINIF SKILL SİSTEMİ — FİNAL TASARIM
*2026-03-28 | Claude + Ollama (qwen2.5:14b + deepseek-r1:14b) sentezi*
*8 Sınıf × 12 Aktif Skill + Tag Sistemi + Seçim Mimarisi*

> **Bu belge SINIF_SKILL_HAVUZU.md'nin yerini alır.**
> Tüm tasarım kararları burada kesinleştirilmiştir.

---

## İÇİNDEKİLER

1. Neden 12 Skill?
2. Seçim Mimarisi — "Signature + Weighted Draft"
3. Tag Sistemi — Kesin Tanımlar
4. Oda Akışı ve Ağırlık Tablosu
5. 8 Sınıf × 12 Skill Tam Tabloları
6. Temel Cross-Class Sinerjiler (Yeni Skilllerle)
7. Uygulama Notları (Solo Dev, Unity 2D)

---

## 1. NEDEN 12 SKİLL?

| Metrik | 8 Skill | 12 Skill |
|--------|---------|---------|
| 4'ünü seç kombinasyon | C(8,4) = **70** | C(12,4) = **495** |
| Dual class toplam | ~70² ≈ 5K | ~495² ≈ **245K** |
| Run'da görünen oran | %100 (hepsini görürsün) | ~%60 (her run farklı) |
| Replayability | Düşük | **Yüksek** |

**Kritik fark:** 8 skill havuzunda oyuncu zamanla hepsini görür — "artık sürpriz yok" hissi gelir. 12'de her run'da farklı 6-8'i yüzeye çıkar; tıpkı Slay the Spire'da 75 karttan 25'ini asla aynı sırayla görmemen gibi.

**Oyuncu asla "12 skill'den 4 seç" yapmıyor.** 3'er 3'er görüyor, birikerek inşa ediyor.

---

## 2. SEÇİM MİMARİSİ — "Signature + Weighted Draft"

### Run Başı — Sınıf Seçimi

```
┌──────────────────────────────────────────────────────┐
│  SINIF SEÇ                                           │
│                                                      │
│  [WARBLADE]              [ELEMENTALİST]              │
│   ★ Mortal Strike         ★ Fireball                 │
│   "İmza skill —            "İmza skill —             │
│    ilk oda teklifinde       ilk oda teklifinde        │
│    garanti gelir"           garanti gelir"            │
│                                                      │
│  Primary Resource: [ RAGE v ] [ MANA ]               │
│                                                      │
│  [BAŞLA]                                             │
└──────────────────────────────────────────────────────┘
```

**İmza skill sistemi:**
- Her sınıfın 1 tanımlayıcı skill'i "önerilmiş" olarak gösterilir
- Otomatik verilmez — ama ilk oda teklifinde GARANTI olarak belirir
- Oyuncu run'a bir beklentiyle gider: "Mortal Strike'ı alacağım büyük ihtimalle"

**Neden bu iyi:** Belirsizlik değil, güven. GW1'in "skill'leri önceden planla" hissini roguelite formatına taşır.

---

## 3. TAG SİSTEMİ — KESİN TANIMLAR

8 fonksiyonel etiket. Her skill **1-2 tag** alır, asla 3+.

| Tag | Sembol | Renk | Tanım | Tipik Örnek |
|-----|--------|------|-------|------------|
| **ANCHOR** | ⚓ | Beyaz | Tek başına güçlü, setup gerektirmez | Whirlwind, Evasion, Multi-Shot |
| **OPENER** | ▶ | Turuncu | Zincirin ilk halkası, diğerleri için kapı açar | Charge, Shadowstep, Corruption |
| **CHAIN** | ⚡ | Sarı | Belirli skill sonrası bonus tetiklenir | Mortal Strike, Execute, Judgment |
| **BUILDER** | ↑ | Mavi | Kaynak üretir veya setup yapar | Crusader Strike, Hemorrhage, Agony |
| **SPENDER** | ↓ | Altın | Kaynağı harcar, setup'ı tüketir | Rupture, Shield of Retribution |
| **FINISHER** | 💥 | Kırmızı | Koşullu büyük hasar (HP eşiği, stack, vs) | Execute, Last Rites, Hexblast |
| **CONTROL** | ⬡ | Mor | CC: stun / root / slow / knockback / silence | War Stomp, Kidney Shot, Enervate |
| **AMPLIFIER** | ✦ | Yeşil | Diğer skill'lerin/hasar kaynaklarının gücünü artırır | Colossus Smash, Pandemic, Rally Cry |

### Keşif Tag'ları (Ollama katkısı — iyi fikir)

**CONTROL** ve **AMPLIFIER** tag'ları başlangıçta **gizli** tutulabilir:
- İkon grileştirilmiş "?" gösterilir
- İlk kez o etkileşim tetiklenince açılır
- "SİNERJİ BULUNDU: Colossus Smash + Mortal Strike" bildirimi

Bu Hades'in Duo Boon keşfinden ilham alıyor — oyuncuyu kendi keşfini yapıyor gibi hissettiriyor.

### İdeal 4-Skill Tag Dağılımı

| Sınıf | Slot 1 | Slot 2 | Slot 3 | Slot 4 |
|-------|--------|--------|--------|--------|
| Warblade | ▶ OPENER | ⚡ CHAIN | ✦ AMPLIFIER | 💥 FINISHER |
| Elementalist | ↑ BUILDER | ↑ BUILDER | ⚡ CHAIN | 💥 FINISHER |
| Rogue | ▶ OPENER | ↑ BUILDER | ↓ SPENDER | ⚓ ANCHOR |
| Ranger | ▶ OPENER | ⬡ CONTROL | ⚓ ANCHOR | 💥 FINISHER |
| Brawler | ⚓ ANCHOR | ↑ BUILDER | ⚡ CHAIN | 💥 FINISHER |
| Paladin | ↑ BUILDER | ↑ BUILDER | ↓ SPENDER | ⚓ ANCHOR |
| Summoner | ↑ BUILDER | ✦ AMPLIFIER | ↓ SPENDER | ⚓ ANCHOR |
| Hexer | ↑ BUILDER | ↑ BUILDER | ✦ AMPLIFIER | 💥 FINISHER |

---

## 4. ODA AKIŞI VE AĞIRLIK TABLOSU

### Skill Tür Sınıflandırması

Her 12 skill üç gruba ayrılır:

| Tür | Tanım | Oda Grubu |
|-----|-------|-----------|
| **Core** | ⚓ ANCHOR ağırlıklı, standalone güçlü | Odalar 1-3 |
| **Advanced** | ⚡ CHAIN / ✦ AMPLIFIER ağırlıklı, 1+ skill ile synerji | Odalar 4-7 |
| **Master** | 💥 FINISHER / özel koşul, 2+ class skill gerektirir | Odalar 8+ |

### Oda Teklif Ağırlıkları

| Oda Grubu | Core | Advanced | Master | Upgrade | Neutral |
|-----------|------|----------|--------|---------|---------|
| 1-3 (erken) | %60 | %20 | %0 | %10 | %10 |
| 4-7 (orta) | %35 | %40 | %10 | %10 | %5 |
| 8+ (geç) | %20 | %30 | %30 | %15 | %5 |

**Master skill gating kuralı:**
- Bir sınıftan 2+ skill alınmadan o sınıfın Master skill'leri offer pool'a GİRMEZ
- Bu "ilk planlamanın meyvesi" hissini yaratır

### Sinerji Güvencesi

Eğer 3. skill alımı sonunda oyuncunun hiçbir ⚡ CHAIN linki yoksa:
→ 4. oda teklifinde 3 seçeneğin **en az biri** mevcut build'iyle zincirli bir skill olur.

### Slot Doluluğu Kuralı (Değişiklik)

```
Slot 1-4:  Normal oda ödülleri
Slot 5:    FLUX veya Elite ödülü (run başına max 1 FLUX)
Slot 6:    Boss Soul ile açılır

→ "Hepsini al" baskısı yok. Slot 5-6 kazanılmak zorunda.
```

---

## 5. 8 SINIF × 12 SKİLL TABLOLARI

---

### ⚔️ WARBLADE
**Core Fantasy:** "Duruyorum, geçemiyorsun. Ben gittikçe tehlikeliyim."
**Kaynak:** Rage (0-100) — hasar verince +5/vuruş, alınca +10, boşta -5/sn
**[V] Burst:** BLADESTORM — Rage %100: 5s spin, CC immune, her 0.5s AoE

| # | İsim | Tags | Tür | Efekt + Zincirleme |
|---|------|------|-----|-------------------|
| 1 | **Charge** | ▶⬡ | Core | 8m dash+stun 1.5s, Rage+20. *Chain:* Stun'daki hedefe ilk saldırı +%80 hasar |
| 2 | **Mortal Strike** | ⚡💥 | Core | Büyük hasar+iyileşme -%50 (6s). *Chain:* Charge sonrası → iyileşme -%100 |
| 3 | **Colossus Smash** | ✦ | Core | 6s: tüm hasar kaynakları +%30. *Chain:* Dual-class burst window ile amplify |
| 4 | **Whirlwind** | ⚓↓ | Core | 2s spin AoE, hareket edebilirsin, Rage -25/sn. *Chain:* Rage biter → Cleave açılır |
| 5 | **Shield Slam** | ⬡ | Core | Guaranteed knockback 3m, duvar=+1.5s stun, Rage-20. *Chain:* War Stomp sonrası → CD yarıya |
| 6 | **Execute** | 💥 | Core | Sadece HP<%30: %400 hasar, Rage boşaltır. *Chain:* Mortal Strike aktifken → %600 |
| 7 | **Hamstring** | ⬡▶ | Core | %50 slow 8s + bleed 3s DoT. *Chain:* Hamstring'li hedefe Charge → stun 3s |
| 8 | **War Stomp** | ⬡↑ | Core | 3m knockup 2s, Rage+25. *Chain:* Whirlwind sırasında → +1s uzar |
| 9 | **Heroic Leap** | ▶⬡ | Advanced | 12m herhangi konuma sıçra (hedef zorunlu değil), iniş AoE 3m, Rage+15. *Chain:* İnişte 2+ düşman → Rage+30 |
| 10 | **Rallying Cry** | ✦↑ | Advanced | 8s: her Rage harcaması = HP'nin +%5'i iyileşme. Hem saldırı hem sustain. *Chain:* Rage %80+ aktive edilirse → süre 12s |
| 11 | **Rupture Strike** | ⚡↑ | Advanced | Derin yara: bleed DoT 8s + Rage+20. *Chain:* Colossus Smash window'unda → bleed tick 2× hızlı (Hexer dual'da patlar) |
| 12 | **Last Man Standing** | ⚓↑ | Master | 4s: HP 1'in altına düşemez (ölümsüz). Süre bitince Rage tamamen dolar. *Chain:* Execute ile birleşince ölümsüzlük içinde %600 |

**Build Eksenleri:**
- **"Executioner"** → Charge + Mortal Strike + Colossus Smash + Execute — klasik WoW Arms rotasyonu
- **"Bleed Lord"** → Hamstring + Rupture Strike + Whirlwind + Rallying Cry — sustain attrition
- **"Last Stand"** → Last Man Standing + War Stomp + Shield Slam + Heroic Leap — CC + ölümsüzlük

---

### 🔥 ELEMENTALİST
**Core Fantasy:** "Her şeyi yakıyorum. Ama önce ritmi buluyorum."
**Kaynak:** Mana (0-100, +8/sn regen) + Elemental State (Fire veya Frost)
**[V] Burst:** INFERNO — Mana %100: 7s arena-wide ateş yağmuru

| # | İsim | Tags | Tür | Efekt + Zincirleme |
|---|------|------|-----|-------------------|
| 1 | **Fireball** | ↑▶ | Core | Orta hasar+ateş DoT 4s, Fire State builder (+1 stack, max 5). *Chain:* 3 ard arda → 3.'de Living Bomb ücretsiz |
| 2 | **Frostbolt** | ⬡↑ | Core | Orta hasar+%30 slow 3s, Fire State tüketir (+Mana regen). *Chain:* Fireball DoT aktifken → Shatter (+%60 hasar aldırma) |
| 3 | **Living Bomb** | ⚡↓ | Core | 5s sonra patlama, öldürünce 3 komşuya kopyalanır. *Chain:* Frostbolt slow altında → patlama yarıçapı 2× |
| 4 | **Blink** | ▶⚓ | Core | 6m anında ışınlanma, geçtiğin düşmanlara hasar, sonraki spell +%20. *Chain:* Düşmanın tam içinden geçilirse → 0.5s stun |
| 5 | **Frozen Orb** | ⬡⚓ | Core | Yavaş hareket eden küre, yolundakileri chills 5s. *Chain:* Orb üzerinden Blink → Orb patlar, chilled=Frozen 2s |
| 6 | **Arcane Blast** | ↑↓ | Core | Her cast +%20 hasar ama +%30 mana maliyet. 4. cast → Arcane Barrage açılır. *Chain:* Colossus Smash (Warblade) window'da Barrage → cap kaldırılır |
| 7 | **Meteor** | 💥⬡ | Core | 1.5s kanal → büyük AoE knockdown. *Chain:* Frozen/slowed hedef → knockdown 3s + hasar +%50 |
| 8 | **Mirror Image** | ⚓ | Core | 2 kopya, 8s. Her kopya random skill atar, hasar önce kopyaya gelir. *Chain:* Kopyalar ölünce → ölüm patlaması AoE |
| 9 | **Chain Lightning** | ⚓✦ | Advanced | 5 hedefe sekiyor, her seki ayrı hasar. *Chain:* Hedef yavaşlamışsa → 7 seki. Üçüncü element: Storm |
| 10 | **Mana Shield** | ↑⚓ | Advanced | 6s: hasar HP yerine Mana'ya gelir. Mana biter = shield düşer. Tersine kaynak mantığı. *Chain:* Mana %100'ken aktive → +2s süre |
| 11 | **Combustion** | ✦▶ | Advanced | 8s Combustion State: tüm Fire spell'ler instant cast, ama mana maliyet ×2. *Chain:* Fire State 5 stack'ta aktive → mana maliyet artışı yok |
| 12 | **Blizzard** | ⬡↑ | Master | 3s kanal → 5m alana 8s devam eden slow+tick hasar. *Chain:* Meteor'dan önce → Meteor knockdown 4s'e çıkar. Duran düşman = Meteor setup |

**Build Eksenleri:**
- **"Fire Burst"** → Combustion + Fireball + Living Bomb + Meteor — kısa pencerede patlama
- **"Frost Control"** → Frostbolt + Blizzard + Frozen Orb + Meteor — zemin kontrol
- **"Arcane Storm"** → Chain Lightning + Arcane Blast + Mana Shield + Blink — güvenli kademeli

---

### 🗡️ ROGUE
**Core Fantasy:** "Görmüyorsun. Zaten geç."
**Kaynak:** Energy (0-100, +15/sn) + Combo Points (0-5, builder→finisher)
**[V] Burst:** SHADOW DANCE — Energy %100+CP 5: 8s her saldırı sonrası otomatik stealth

| # | İsim | Tags | Tür | Efekt + Zincirleme |
|---|------|------|-----|-------------------|
| 1 | **Backstab** | ▶↑ | Core | Hedefin arkasında: %200 hasar+3CP. Önden: normal hasar, CP yok. *Chain:* Shadowstep sonrası → +%50 ekstra |
| 2 | **Hemorrhage** | ↑ | Core | Bleed 8s DoT+2CP. Öldürünce yakına yayılır. *Chain:* Bleed aktif hedefe Rupture → hasar +%100 |
| 3 | **Rupture** | 💥↓ | Core | 5CP finisher: bleed+hasar, CP sayısına göre süre uzar (max 12s). *Chain:* Zaten bleed varsa → birikmiş hasar anında patlar |
| 4 | **Shadowstep** | ▶⬡ | Core | Hedefe anında ışınlan (8m), 0.5s stun, Energy-25. *Chain:* Evasion aktifken → CD sıfırlanır |
| 5 | **Kidney Shot** | ⬡↓ | Core | 5CP: 4s stun, CP sayısına göre uzar. *Chain:* Mortal Strike (Warblade dual) aktifken → stun'da iyileşme yok |
| 6 | **Ambush** | 💥▶ | Core | Sadece stealth'ten: %300 hasar+4CP+%20 slow. *Chain:* 3s+ stealth → "Cold Blood" +%100 ekstra hasar |
| 7 | **Fan of Knives** | ⚓⬡ | Core | Anında 360° AoE, tüm aktif bleed/zehir/debuffleri tüm düşmanlara uygular, Energy-40. *Chain:* Hexer dual → Hexer debuffları da yayılır |
| 8 | **Evasion** | ⚓↑ | Core | 4s %100 dodge, her dodge=+1CP, kill=CD sıfırlanır. *Chain:* Evasion bitince sonraki saldırı guaranteed crit |
| 9 | **Deadly Poison** | ↑▶ | Advanced | 10s silah zehiri: her saldırı ayrı zehir DoT uygular (bleed'den bağımsız stack). *Chain:* Hexer dual → her zehir DoT 1 Hex stack uygular |
| 10 | **Sprint** | ▶⚓ | Advanced | 3s hız +%100, geçtiğin düşmanlara hasar. Shadowstep'ten farkı: yön odaklı (hedef yok). *Chain:* Sprint sırasında Backstab → arka pozisyon garantilenir |
| 11 | **Preparation** | ↑✦ | Advanced | Tüm Rogue skill CD'lerini sıfırla. 90s CD. *Chain:* Evasion aktifken kullanılırsa → Preparation kendi CD'si 60s'ye iner |
| 12 | **Vanish** | ⚓⬡ | Master | Savaşta anlık stealth, tüm düşmanlar hedef kaybeder. 3s tam görünmezlik. 50s CD. *Chain:* Vanish sonrası Ambush → "Cold Blood" bonus garantili |

**Build Eksenleri:**
- **"Assassin"** → Ambush + Vanish + Backstab + Preparation — stealth centric
- **"Bleeder"** → Hemorrhage + Deadly Poison + Rupture + Fan of Knives — çift DoT farm
- **"Duelist"** → Evasion + Sprint + Shadowstep + Kidney Shot — reaktif pozisyon

---

### 🏹 RANGER
**Core Fantasy:** "Sana ulaşamazsın. Her saniye kayıp veriyorsun."
**Kaynak:** Focus (0-100) — 4m+ uzakta +10/sn, 2m altında -20/sn
**Focus 75+:** tüm oklar +%25 hasar | **Focus 100:** sonraki skill free cast
**[V] Burst:** RAIN OF ARROWS — 30s sabit CD: 5s tüm arena yağmur

| # | İsim | Tags | Tür | Efekt + Zincirleme |
|---|------|------|-----|-------------------|
| 1 | **Aimed Shot** | 💥⚡ | Core | 1.5s şarj → büyük tek hasar+%50 crit şansı. Hedef immobile=anında ateş. *Chain:* Concussive Arrow root'u sonrası → guaranteed instant |
| 2 | **Concussive Arrow** | ⬡▶ | Core | Anında knockback 4m+root 2s. *Chain:* Backward Dash sırasında → uzaklık 6m+slow 3s |
| 3 | **Serpent Sting** | ↑▶ | Core | Zehir DoT 10s + armor debuff -%20. Her 2s yenilenirse max 30s devam. *Chain:* Disengage+Serpent Sting → zehir daha geniş alana |
| 4 | **Explosive Trap** | ⬡↑ | Core | Zemine tuzak, 3s sonra patlama+slow 3s. Zincirleme: birden fazla trap tetiklenir. *Chain:* Summoner dual → trap minyona konulabilir (mobil) |
| 5 | **Multi-Shot** | ⚓↑ | Core | Delici ok: tüm düşmanlardan geçer, her birine Serpent Sting stack. *Chain:* 5+ düşman vurulursa → tüm CD -3s |
| 6 | **Disengage** | ▶⬡ | Core | 6m geri atla, slow alanı bırak. Havada hasar -%30. *Chain:* Disengage+anında Aimed Shot → atlama sırasında ateş, ekstra hasar |
| 7 | **Black Arrow** | ↑⚡ | Core | DoT+özel: bu DoT ile ölen düşman 8s ruh bırakır. *Chain:* Summoner dual → ruh minyon sayılır (Blood for Power tetikler) |
| 8 | **Volley** | ⬡⚓ | Core | 4m'lik alana 3s yağmur, slow+tick. Sen hareket edebilirsin. *Chain:* Explosive Trap üzerine → tam kilitlenme combo |
| 9 | **Rapid Fire** | ⚓↓ | Advanced | 2s kanal: 8 hızlı ok, tek ok küçük hasar ama toplam>Aimed Shot. Focus -30. *Chain:* Focus 100'de başlarsa → 10 ok, Focus maliyeti yok |
| 10 | **Spirit Wolf** | ↑⚓ | Advanced | 12s hayalet kurt companion. Bağımsız saldırır, Serpent Sting otomatik uygular. *Chain:* Summoner dual → minyon sayılır, Blood for Power tetikler |
| 11 | **Flare** | ⬡✦ | Advanced | 6m flare: stealthed düşmanları açığa çıkarır+6s slow alanı. Focus+20. *Chain:* Rogue dual → Vanish/stealth'i iptal eder, CP kaybettirir |
| 12 | **Point Blank** | 💥⬡ | Master | ≤2m yakınlıkta: ×3 hasar+5m knockback. Focus 100 gerektirir. *Chain:* Disengage sonrası anında kullanılırsa → CD sıfırlanır |

**Build Eksenleri:**
- **"Sniper"** → Aimed Shot + Concussive Arrow + Rapid Fire + Focus mekanik
- **"Trap Master"** → Explosive Trap + Volley + Flare + Serpent Sting
- **"Hunter Pack"** → Spirit Wolf + Black Arrow + Multi-Shot + Disengage

---

### 👊 BRAWLER
**Core Fantasy:** "Az canken daha tehlikeliyim. Bu hata değil, strateji."
**Kaynak:** Fury (0-100) — SADECE hasar ALARAK dolar (+15/vuruş). HP düştükçe daha hızlı.
**[V] Burst:** BERSERK MODE — Fury %100: 15s defense ignore + %200 hasar + tüm CD sıfır

| # | İsim | Tags | Tür | Efekt + Zincirleme |
|---|------|------|-----|-------------------|
| 1 | **Bloodlust Strike** | ⚡💥 | Core | Koni saldırı, HP'ye göre hasar artar (%100HP=baz, %30HP=+%120). *Chain:* Fury %80+ → Slaughter anında açılır |
| 2 | **Whirlwind** | ⚓↓ | Core | 2s spin AoE, her düşman vuruşu savunma -%5 (max -%30). *Chain:* Savunma -%30'da Whirlwind → Fury +20/spin |
| 3 | **Frenzied Leap** | ▶↑ | Core | Hedefe atla, iniş AoE. Hit → CD anında sıfırlanır. *Chain:* 3 ard arda farklı hedefe → 5s Frenzy buff +%50 hasar |
| 4 | **Reckless Swing** | 💥↓ | Core | Devasa tek hasar, 2s tam savunmasız. *Chain:* Hasar alırsan savunmasızlıkta → Fury+40+0.8s invuln counter |
| 5 | **Bloodthirst** | ⚓↑ | Core | Hızlı 5 vuruş, her vuruş küçük iyileşme. HP düşükse iyileşme artar. *Chain:* HP<%20+Fury%100 → 8 vuruşa yükselir |
| 6 | **Intimidating Shout** | ⬡ | Core | 3m çevresindeki düşmanlar 3s panik/kaçar. *Chain:* Panikleyen düşmana Bloodlust Strike → +%100 hasar (sırta vuruluyor) |
| 7 | **Barbaric Charge** | ▶⬡ | Core | Düz çizgide her şeyi iter, stun/root immune. *Chain:* Duvara çarparsa → itilen düşmanlar stun 2s |
| 8 | **Last Rites** | 💥 | Core | SADECE HP<%15: %600 hasar, sonra 4s tam savunmasız. *Chain:* Eğer öldürürse → savunmasızlık 2s'ye iner |
| 9 | **Iron Grab** | ⬡▶ | Advanced | Yakala (≤2m): 1.5s hold, sonra seçtiğin yöne fırlat. Duvara=stun. Fury+30. *Chain:* Fırlatılan düşman 3. bir düşmana çarparsa → her ikisi de stun |
| 10 | **War Cry** | ↑✦ | Advanced | 8s: Fury üretimi ×2. Kasten hızlı doldurmak için. *Chain:* Fury %50 altındayken aktive → süre 12s |
| 11 | **Shatter Armor** | ✦▶ | Advanced | Hedefin savunması -%40, 10s (tüm kaynaklar yararlanır). *Chain:* Warblade dual Colossus Smash window'unda → savunma -%60 |
| 12 | **Death Wish** | ⚓↑ | Master | 5s: HP 1'in altına düşemez (ölümsüz). Fury bu sürede ×3 hızlı dolar. Sonra 8s +%80 hasar. *Chain:* Fury %100'e ulaşırsa Death Wish sırasında → [V] Burst anında tetiklenebilir |

**Build Eksenleri:**
- **"Glass Cannon"** → Reckless Swing + Bloodlust Strike + Last Rites + Death Wish
- **"Control Brawler"** → Iron Grab + Barbaric Charge + Intimidating Shout + Frenzied Leap
- **"Fury Engine"** → War Cry + Bloodthirst + Whirlwind + Shatter Armor

---

### ⚖️ PALADİN
**Core Fantasy:** "Hem kesilemiyorum hem öldürüyorum. Bu çelişki değil, tasarım."
**Kaynak:** Holy Power (0-100, builder skill'lerle dolar, spender skill'lerle boşaltılır)
**[V] Burst:** AVENGING WRATH — HP ritmi: 3 mükemmel zincir tamamlanınca: 10s %30 invuln+%50 hasar

| # | İsim | Tags | Tür | Efekt + Zincirleme |
|---|------|------|-----|-------------------|
| 1 | **Crusader Strike** | ↑▶ | Core | Temel melee+HP+25. *Chain:* Crusader→Starfire→Crusader zinciri → her 3.'de +%60 hasar |
| 2 | **Divine Storm** | ↑✦ | Core | 360° melee AoE+HP+15/hedef. *Chain:* 3+ düşman vurulursa → HP+50 (flat yerine) |
| 3 | **Judgment** | ↑⚡ | Core | Ranged holy blast (6m), debuffluysa +%50 hasar. *Chain:* Hexer dual: Hexer debuff → +%100 hasar |
| 4 | **Consecration** | ↑⚓ | Core | 5s kutsal zemin, tick hasar+HP+5/sn. *Chain:* Consecration + Warblade Battle Cry → düşmanlar koşarak kutsal zeminde patlıyor |
| 5 | **Hammer of Wrath** | 💥⚡ | Core | SADECE HP<%20 hedefe: büyük hasar+HP+30. *Chain:* Execute (Warblade dual) ile arka arkaya → her ikisi de HP eşiği |
| 6 | **Avenger's Shield** | ⬡↑ | Core | Kalkan fırlat: 3 hedefe sekip silence+HP+15/düşman. *Chain:* 3 farklı hedefe sekerse → her biri 2s slow alır |
| 7 | **Holy Shock** | ⚓↑ | Core | Düşman=hasar+HP+15 / kendin=iyileşme. *Chain:* HP<%30'da kendi üzerine → iyileşme ×3 |
| 8 | **Shield of Retribution** | ⚡↓ | Core | 3s blok, engellenen hasar biriktirilir, AoE olarak serbest. *Chain:* Consecration üzerindeyken → AoE ×2 |
| 9 | **Blessed Weapon** | ↑✦ | Advanced | 12s: tüm melee saldırılar +15 HP/hit (normal 5-10 yerine). HP motoru hızlandırır. *Chain:* HP %80+ başlatılırsa → süre 18s |
| 10 | **Lay on Hands** | ⚓↓ | Advanced | Anlık tam HP iyileşme. HP tamamen sıfırlar. 90s CD. *Chain:* HP sıfırlandıktan sonraki 5s → HP üretim ×2 ("açlık sonrası ödül") |
| 11 | **Devotion Aura** | ✦↑ | Advanced | 8s: sen+minyonlar -%30 hasar, HP+5/sn pasif. *Chain:* Summoner dual → 3+ minyon hayattaysa süre 12s |
| 12 | **Divine Sacrifice** | ⚡↑ | Master | 5s: minyon/companion hasarının %60'ını üstlenirsin AMA HP ×3 üretirsin. *Chain:* Fallen Saint (Brawler dual) → üstlenilen hasar Fury doldurur |

**Build Eksenleri:**
- **"Tank Rhythm"** → Crusader Strike + Consecration + Shield of Retribution + Blessed Weapon
- **"Holy Burst"** → Divine Storm + Judgment + Avenging Wrath + Lay on Hands
- **"Army Support"** → Devotion Aura + Divine Sacrifice + Consecration + Hammer of Wrath

---

### 💀 SUMMONER
**Core Fantasy:** "Ben savaşmıyorum. Feda ediyorum. Ve feda anı en güçlü andır."
**Kaynak:** Charges (0-4, auto +1/8s, minyon ölünce +1 anında, feda edince +charge)
**[V] Burst:** ARMY OF THE DEAD — tüm charge doluyken: 6s tüm minyonlar +%150 hasar ve ölümsüz

| # | İsim | Tags | Tür | Efekt + Zincirleme |
|---|------|------|-----|-------------------|
| 1 | **Raise Skeleton** | ↑▶ | Core | 1 Charge→melee iskelet, max 3. 3 birden→Rally Cry +%40 (normal +%20 yerine). *Chain:* 3 iskelet varken→sıradaki iskelet dördüncüyü kitleyen +%20 hasar |
| 2 | **Summon Golem** | ⚓⬡ | Core | 2 Charge→1 büyük Golem. Yolu bloke eder. HP<%20→kendini patlatır AoE. *Chain:* Golem'e Commanding Strike → duvara çarpma stun |
| 3 | **Rally Cry** | ✦ | Core | Tüm minyonlar +%20 hasar+hız 10s. *Chain:* Karışık minyon tipleri mevcutsa (iskelet+golem vb.) → bonus +%40'a çıkar |
| 4 | **Corpse Explosion** | 💥⚡ | Core | Düşman veya minyon cesedini patlatır, AoE. *Chain:* 3+ cesetle → zincir patlama (patlama patlamayı tetikler) |
| 5 | **Death Nova** | ⚡⬡ | Core | 1 minyonu feda: 8s zehir bulutu bırakır. Minyon ölüyor ama zehir devam eder. *Chain:* Hexer dual → zehir bulutu Hexer debufflarını yayar |
| 6 | **Commanding Strike** | ✦⬡ | Core | Seçili minyona emir: ×4 hasar+invuln. Minyon yoksa → Summoner kendisi ×2 hasar atar. *Chain:* Golem'e emir → hedefi duvara çarpar |
| 7 | **Blood for Power** | ↑↓ | Core | Minyon feda → Charge+1+tüm CD -%30. *Chain:* 3 minyon ard arda feda edilirse → Charge+3+[V] Burst meter +30 |
| 8 | **Bone Shield** | ⚓↑ | Core | 3s: minyon kalkan olarak kullanılır, gelen hasar absorbe, absorb→Charge+1. *Chain:* Golem kalkan olursa → absorbe ×2 |
| 9 | **Raise Archer** | ↑⚓ | Advanced | 1 Charge→uzaktan saldıran iskelet, max 2. Ranger dual → Serpent Sting otomatik uygular. *Chain:* 2 archer+1 melee iskelet aynı anda → Rally Cry +%50'ye çıkar |
| 10 | **Sacrificial Pact** | ↓⚓ | Advanced | Tüm minyonlar intihar: HP+%8/minyon iyileşme+tüm Charge geri. 3 minyon=+%24 HP+full reset. *Chain:* Boss fight'ta Golem dahil tüm fedayla → tek seferlik +%40 HP |
| 11 | **Dark Pact** | ↑ | Advanced | HP-%12 öde → Charge olmadan minyon çağır. Charge tükenince devam etmek için. *Chain:* Brawler dual → HP kaybı Fury doldurur (sonsuz döngü potansiyeli) |
| 12 | **Lich Form** | ✦⚓ | Master | 10s: Summoner ghostal (melee immune), kendi melee=bone strike. Tüm minyonlar +%60 hasar. *Chain:* Lich Form sırasında minyon feda edilirse → feda hasarı ×3 |

**Build Eksenleri:**
- **"Sacrifice Engine"** → Blood for Power + Death Nova + Sacrificial Pact + Rally Cry
- **"Army Commander"** → Raise Skeleton + Raise Archer + Commanding Strike + Rally Cry
- **"Lich Burst"** → Lich Form + Dark Pact + Corpse Explosion + Bone Shield

---

### 🔮 HEXER
**Core Fantasy:** "Sabır. 10'a gelince sen bitiyorsun."
**Kaynak:** Hex Stacks per enemy (0-10). Faz sistemi:
```
0-3 stack:  Debuff Phase    — zayıf tick hasar
4-6 stack:  Pressure Phase  — tüm skill'ler +%20 güç
7-9 stack:  Overload Phase  — düşman titriyor, +%30 daha fazla hasar alır
10 stack:   HEXBLAST        — anında kullan, CD sıfır, zincir patlama
Her faz geçişinde: küçük bonus efekt + sesli+görsel feedback
```
**[V] Burst:** HEX CASCADE — bir hedefte 10 stack: tüm düşmanlara 3 stack kopyalanır

| # | İsim | Tags | Tür | Efekt + Zincirleme |
|---|------|------|-----|-------------------|
| 1 | **Corruption** | ▶↑ | Core | Anında 3 stack+4s orta DoT. *Chain:* Corruption→Agony ard arda → Agony başlangıç hızı ×2 |
| 2 | **Agony** | ↑ | Core | Süregelen DoT, 2 stack/tick. Durdurulabilir değil. *Chain:* Hedef Pressure Phase'deyken Agony → stack tick'i 3'e çıkar |
| 3 | **Pandemic** | ✦⚡ | Core | Bir hedefteki TÜM stack'ları yakın düşmanlara kopyalar. *Chain:* Hedef Overload Phase'de → kopyalanan stack miktarı +3 |
| 4 | **Hexblast** | 💥↓ | Core | 10 stack patlaması: %100/stack, kill=CD sıfır, yakına 2 stack yayılır. *Chain:* Tüm odada 3+ hedef Pressure Phase'deyken → Hexblast zincirlenerek herkese yayılır |
| 5 | **Empathy** | ⚡⬡ | Core | Lanet: düşmanın saldırılarının %30'u kendine döner. *Chain:* Hedef Overload Phase'de → dönen hasar %60'a çıkar |
| 6 | **Haunt** | ↑⬡ | Core | Hayalet bağla: takip eder+tick+3 stack. 10 stack'ta otomatik Hexblast tetikler. *Chain:* Fan of Knives (Rogue dual) → Haunt'un stack'ları tüm düşmanlara |
| 7 | **Unstable Affliction** | 💥⚡ | Core | Dispel/heal edilirse → anında patlama+stun. *Chain:* Hedef stun/CC altındayken → patlama garantili full stack |
| 8 | **Enervate** | ⬡✦ | Core | Hız -%50, saldırı hızı -%40, 10s. *Chain:* 5+ stack hedefte Enervate → süre ×2 |
| 9 | **Mass Hex** | ▶⬡ | Advanced | Görüntüdeki TÜM düşmanlara aynı anda 2 stack. HP -%8 maliyeti. *Chain:* Pressure Phase'deki düşmanlar varsa → 3 stack uygular |
| 10 | **Silence Hex** | ⬡↑ | Advanced | 5s özel yetenek kilidi (elite burst, boss mekanik suppressed)+3 stack. *Chain:* Boss fight'ta 7+ stack varken → silence 8s |
| 11 | **Cursed Mirror** | ⚡↑ | Advanced | 8s: sana uygulanan her debuff → en yakın düşmana %50 güçle yansır, +2 stack/yansıma. *Chain:* Enervate sana uygulanırsa → düşman kendi yavaşlatmasını alır |
| 12 | **Soul Bargain** | 💥↓ | Master | HP -%25 feda → hedefe anında 5 stack. *Chain:* Hedef zaten Overload Phase'deyse (7-9 stack) → Soul Bargain 10'a tamamlar, anında Hexblast |

**Build Eksenleri:**
- **"Patient DoT"** → Corruption + Agony + Pandemic + Hexblast — klasik Hexer rotasyonu
- **"Mass Hexer"** → Mass Hex + Enervate + Cursed Mirror + Silence Hex — kalabalık kontrol
- **"Soul Burst"** → Soul Bargain + Haunt + Unstable Affliction + Hexblast — agresif yüksek risk

---

## 6. TEMEL CROSS-CLASS SİNERJİLER (Yeni Skilllerle)

En dikkat çekici yeni etkileşimler:

| Combo | Sınıflar | Skill'ler | Ne Oluyor | Neden İnsane | Tier |
|-------|---------|-----------|-----------|-------------|------|
| **"Leaping Hex"** | Warblade+Hexer | Heroic Leap + Mass Hex | Leap iner, Mass Hex anında her düşmana 2 stack | Her giriş anında Pressure Phase başlar | S |
| **"Combustion Chain"** | Elementalist+Warblade | Combustion + Colossus Smash | 8s instant Fire spell, Smash window'unda her Fireball cap kaldırılmış | DPS tavanı yok | S |
| **"Poison Web"** | Rogue+Hexer | Deadly Poison + Fan of Knives | Fan: tüm zehirleri yayar + Hexer dual → her zehir 1 stack | Tek oda temizlemede 5+ hedefe instant Pressure Phase | A |
| **"Dark Army"** | Summoner+Brawler | Dark Pact + Fury doldurma | HP harcayarak minyon çağırınca Brawler → HP kaybı Fury doldurur | Ne kadar ağrırsam o kadar güçleniyorum + ordum büyüyor | A |
| **"Eternal Fallen Saint"** | Brawler+Paladin | Bloodthirst + Holy Endurance + Ruthless | HP %30'da kilitlenir: Ruthless aktif+Bloodthirst iyileştirir+Holy Endurance döngüsü | Sürekli aktif Ruthless, teorik olarak sonsuz |⭐ S (kasıtlı broken) |
| **"Lich Pandemic"** | Summoner+Hexer | Lich Form + Death Nova + Pandemic | Lich Form'da minyon feda ×3 hasar bulutu → Pandemic ile tüm odaya | 10 saniyede tüm oda zehirli + stacklı | S |
| **"Mirror Hex"** | Elementalist+Hexer | Cursed Mirror + Blizzard | Blizzard slow → düşman sana slow uygularsa mirror → düşman double slow | Düşman kendi debuffıyla boğuluyor | A |
| **"Plague Arrow Field"** | Ranger+Hexer | Spirit Wolf + Mass Hex | Wolf Serpent Sting uygularken Mass Hex → zehir+stack aynı anda tüm alana | Ranger hiç yaklaşmadan alan hex'liyor | A |

### Plague Doctor (Summoner+Hexer) — Yeni İmza Combo
**"Pandemic Lich":**
```
Lich Form aktive → Raise Archer (×3 hasar) → archer'lar Serpent Sting →
Mass Hex (tüm odaya 2 stack) → Death Nova (feda bulutu) →
Pandemic (tek hedefteki stack tümüne) → Hexblast zincir patlama
```
Bu combo 45 saniye kurulum gerektiriyor ama patladığında oda biter.

---

## 7. UYGULAMA NOTLARI (Solo Dev, Unity 2D)

### ScriptableObject Yapısı

```csharp
// SkillTag enum
public enum SkillTag { ANCHOR, OPENER, CHAIN, BUILDER, SPENDER, FINISHER, CONTROL, AMPLIFIER }

// SkillData ScriptableObject
[CreateAssetMenu]
public class SkillData : ScriptableObject {
    public string skillName;
    public SkillTag[] tags;          // max 2
    public SkillTier tier;           // Core / Advanced / Master
    public string chainRequirement;  // hangi skill sonrası bonus
    public string chainEffect;       // bonus efektin açıklaması
    public bool isHidden;            // keşif tag sistemi için
}

// SkillOfferSystem
public class SkillOfferSystem {
    // Room tier'a göre Core/Advanced/Master ağırlığını döndürür
    public SkillData[] GenerateOffer(int roomNumber, List<SkillData> equipped) {
        // imza skill garantisi (ilk oda)
        // sinerji güvencesi (3. alım sonrası)
        // Master gating (2+ class skill olmadan çıkmaz)
    }
}
```

### FAZ 2 Minimum Viable

**Mutlaka FAZ 2'de:**
- Her sınıf için 8 Core skill (mevcut)
- Tag sistemi (ScriptableObject — düşük maliyet)
- Oda ağırlık sistemi (basit randomizer)
- İmza skill garantisi

**FAZ 4'e bırakılabilir:**
- Advanced ve Master skill'ler (4+4 per class)
- Keşif tag sistemi (gizli CONTROL/AMPLIFIER)
- Slot 5-6 FLUX/Boss Soul gating
- "Sinerji Keşfedildi" bildirimi

### Tasarım Tamamlanmış — Beklenen Sayılar

| Metric | Değer |
|--------|-------|
| Tek sınıf build çeşidi | 495 |
| Dual class build çeşidi | ~245,000 |
| Sınıf başına yeni skill | 4 |
| Toplam yeni skill | 32 |
| Toplam aktif skill havuzu | 96 (8×12) |
| Cross-class ultimate | 28 (değişmedi) |

---

*Bu belge kesinleştirilmiştir. Revizyon gerekirse GOREVLER.md'ye task ekle.*
*İlgili dosyalar: SINIF_SKILL_HAVUZU.md (eski versiyon, arşiv), MASTER_SINIF_VE_CROSSCLASS.md (combo listesi)*
