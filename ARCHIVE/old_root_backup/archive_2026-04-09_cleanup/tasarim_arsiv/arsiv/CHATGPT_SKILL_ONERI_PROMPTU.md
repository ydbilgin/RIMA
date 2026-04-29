# ChatGPT Skill Öneri Promptu (v2)
*Bu dosyayı kopyalayıp ChatGPT'ye yapıştır. "Derin araştırma" modunu aç.*

---

## KİMİM VE NE YAPIYORUM

Ben **solo bir indie game developer**ım. Tek başıma, 6 aylık bir zaman diliminde Steam'e çıkarmayı hedeflediğim bir **2D roguelite aksiyon oyunu** geliştiriyorum. Engine: Unity 6.3 LTS. Art stili: pixel art (Aseprite + PixelLab). Programcı sanatçı değilim — üretim sürecini de optimize etmek zorundayım. Her tasarım kararı hem "oyunu iyi yapar mı" hem de "solo dev olarak uygulayabilir miyim" testinden geçmek zorunda.

### Oyunun Ruhu — Ne Yapmak İstiyorum

Benim tek bir hedefim var: **oyuncuya MMORPG'deki "bu build insane" anını yaşatmak — ama roguelite formatında.**

MMORPG oynayan herkes o anı bilir: cooldown'ların senkrona girdiği, proc'ların üst üste bindiği, düşmanların adeta eriyip gittiği o 10-15 saniyelik pencere. Guild Wars 1'de aylarca çalışarak kurduğun 8-skill kombinasyonu ilk kez tıkladığında, WoW'da mükemmel bir rotasyon tutturduğunda, FFXIV'de Black Mage olarak Astral Fire/Umbral Ice döngüsünü ilk kez tam zamandığında hissedilen o his. Bu his şimdiye kadar **sadece çok oyunculu oyunlarda** mümkün olabildi — çünkü onlarda yüzlerce saatlik öğrenme eğrisi, topluluk teoricraftı, karaktere yatırım vardı.

Ben bunu **tek oyunculu, 45-60 dakikalık bir roguelite run'ına** sığdırmak istiyorum. Her run kendi başına tamamlanmış bir "build hikayesi" olsun. Oyuncu run bitince "bir dahaki sefere şu combo'yu denerim" desin.

### Oyunun Referansları ve Bunları Nasıl Harmanlıyorum

| Referans Oyun | Aldığım İlham | Nasıl Uyarlıyorum |
|---------------|---------------|-------------------|
| **Guild Wars 1** | Secondary profession sistemi — 1329 skill'den sadece 8 seçersin. Her kombinasyonun kendine özgü "broken build" potansiyeli var. | 8 sınıftan 2 seçilir. Her sınıfın 12 skill havuzu var. C(12,4)=495 kombinasyon — oyuncu asla "tümünü gördüm" hissi yaşamaz. |
| **Slay the Spire** | Kart acquisition sistemi — her oda 3 teklif, zamanla build kendiliğinden şekillenir. "Build click" anı oyunun kalbi. | "Weighted Draft" sistemi: erken odalar temel skill, geç odalar güçlü skill ağırlıklı. Oyuncu asla 12 skill'den seç ekranı görmez. |
| **Hades** | Oda yapısı, Duo Boon sürprizi, "SİNERJİ BULUNDU" hissi. | Tag sistemi: CONTROL ve AMPLIFIER tag'ları başlangıçta gizli, ilk tetiklenince açılıyor — aynı Duo Boon discovery hissi. |
| **Shadow of Mordor** | Nemesis sistemi — düşmanlar seni hatırlıyor ve adapte oluyor. | Grudge sistemi: Elite'ler nasıl öldürüldüklerini hatırlıyor, o yönteme +%35 direnç kazanıyor. Oyuncuyu kasıtlı build çeşitlendirmeye itiyor. |
| **WoW / FFXIV / Lost Ark** | Her sınıfın kendine özgü kaynak döngüsü ve rotation ritmi. | 8 sınıfın her biri tamamen farklı kaynak sistemi: Rage/Mana/Energy+ComboPoints/Focus/Fury/HolyPower/Charges/HexStacks. |

### Oyunun Teknik Özeti

- **Tür:** Flat top-down 2D roguelite aksiyon (izometrik yok, 3D yok — kesin karar)
- **Platform:** PC — Steam
- **Engine:** Unity 6.3 LTS, 2D URP
- **Geliştirici:** Solo — Laureth
- **Hedef:** 6 ayda Steam'e çıkış

---

## BU PROMPTUN AMACI

Senden iki şey istiyorum:

1. **Her sınıf için eksik veya değiştirilebilecek skill'lerin yerine somut YENİ skill önerileri** — isim, tag, tier, efekt, chain koşulu ile birlikte
2. **Heroic/Legendary skill sistemi tasarımı** — ne zaman, nasıl, kaç tane, nasıl hissettirmeli

Araştırma özeti **istemiyorum.** Öneri istiyorum. Çıktı formatını aşağıda verdim, ona uyu.

---

## OYUN BAĞLAMI — HIZLI ÖZET

**Tür:** Flat top-down 2D roguelite aksiyon (Unity 6.3, solo dev)
**Temel his:** MMORPG'nin "bu build insane" anı — roguelite formatında.
**Referanslar:** Guild Wars 1 dual-class + Slay the Spire acquisition + Hades oda yapısı

### Dual-Class Sistemi
- Run başında 8 sınıftan **2** seçilir
- Her sınıfın **12 aktif skill havuzu** var (toplam 96 skill)
- Oyuncu "Signature + Weighted Draft" ile oda oda **4 aktif skill** biriktirir
- Slot 5: sadece FLUX/Elite ödülü | Slot 6: sadece Boss Soul ile açılır
- 28 benzersiz sınıf kombinasyonunun her birinin cross-class pasifi ve cross-class ultimate'ı var

### Tag Sistemi (her skill 1-2 tag taşır)
| Tag | Sembol | Tanım |
|-----|--------|-------|
| ANCHOR | ⚓ | Tek başına güçlü, setup gerektirmez |
| OPENER | ▶ | Zincirin ilk adımı |
| CHAIN | ⚡ | Belirli skill sonrası bonus tetiklenir |
| BUILDER | ↑ | Kaynak/setup üretir |
| SPENDER | ↓ | Kaynağı/setup'ı harcar |
| FINISHER | 💥 | Koşullu büyük hasar |
| CONTROL | ⬡ | CC: stun/root/slow/knockback (başlangıçta gizli) |
| AMPLIFIER | ✦ | Diğer skill'lerin gücünü artırır (başlangıçta gizli) |

### Skill Tier Sistemi
- **Core (8 skill):** Tek başına güçlü, setup gerektirmez
- **Advanced (3 skill):** 1+ class skill ile çalışır, sinerji gerektirir
- **Master (1 skill):** 2+ class skill yoksa offer pool'a girmez

### Oda Teklif Ağırlıkları
| Oda | Core | Advanced | Master |
|-----|------|----------|--------|
| 1-3 | %60 | %20 | %0 |
| 4-7 | %35 | %40 | %10 |
| 8+ | %20 | %30 | %30 |

### 8 Sınıf — Kaynak Özeti
| Sınıf | Kaynak | [V] Burst |
|-------|--------|-----------|
| **Warblade** | Rage (0-100, al/ver) | Bladestorm 5s spin CC immune AoE |
| **Elementalist** | Mana (+8/sn) + Fire/Frost State | Inferno 7s arena-wide ateş |
| **Rogue** | Energy (+15/sn) + Combo Points (0-5) | Shadow Dance 8s her saldırı→stealth |
| **Ranger** | Focus (4m+: +10/sn, 2m-: -20/sn) | Rain of Arrows 30s sabit CD |
| **Brawler** | Fury (SADECE hasar alarak +15/vuruş) | Berserk Mode 15s defense ignore +%200 |
| **Paladin** | Holy Power (Builder/Spender döngüsü) | Avenging Wrath 10s %30 invuln +%50 hasar |
| **Summoner** | Charges (0-4, auto +1/8s) | Army of Dead 6s tüm minyonlar +%150 |
| **Hexer** | Hex Stacks (0-10/düşman, 4 faz: 0-3/4-6/7-9/10) | Hex Cascade tüm düşmanlara 3 stack |

---

## MEVCUT SKİLL LİSTELERİ

### ⚔️ WARBLADE — Mevcut 12 Skill

**Core Fantasy:** "Duruyorum, geçemiyorsun."
**Kaynak:** Rage (0-100) — hasar verince +5, alınca +10, boşta -5/sn

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | Charge | ▶⬡ | Core | 8m dash+stun 1.5s, Rage+20 | Stun'daki hedefe ilk vuruş → +%80 hasar |
| 2 | Mortal Strike | ⚡💥 | Core | Büyük hasar + iyileşme -%50 (6s) | Charge sonrası → iyileşme -%100 |
| 3 | Colossus Smash | ✦ | Core | 6s window: tüm hasar +%30 | Dual burst window → amplify katlanır |
| 4 | Whirlwind | ⚓↓ | Core | 2s spin AoE, hareket edilebilir, Rage -25/sn | Rage biterken → Cleave açılır |
| 5 | Shield Slam | ⬡ | Core | Knockback 3m, duvara çarparsa +1.5s stun, Rage-20 | War Stomp sonrası → CD yarıya |
| 6 | Execute | 💥 | Core | SADECE HP<%30: %400 hasar, Rage boşaltır | Mortal Strike aktifken → %600 hasar |
| 7 | Hamstring | ⬡▶ | Core | %50 slow 8s + bleed 3s DoT | Hamstring'li hedefe Charge → stun 3s |
| 8 | War Stomp | ⬡↑ | Core | 3m knockup 2s, Rage+25 | Whirlwind sırasında → +1s uzar |
| 9 | Heroic Leap | ▶⬡ | Advanced | 12m atla, iniş AoE 3m, Rage+15 | İnişte 2+ düşman → Rage+30 |
| 10 | Rallying Cry | ✦↑ | Advanced | 8s: her Rage harcaması = HP +%5 | Rage %80+'ta aktive → süre 12s |
| 11 | Rupture Strike | ⚡↑ | Advanced | Bleed DoT 8s + Rage+20 | Colossus Smash window → bleed tick 2× hızlı |
| 12 | Last Man Standing | ⚓↑ | Master | 4s: HP 1 altına düşemez, sonra Rage tamamen dolar | Execute ile birlikte → ölümsüzlük içinde %600 |

**Build eksenleri:** "Executioner" / "Bleed Lord" / "Last Stand"

---

### 🔥 ELEMENTALİST — Mevcut 12 Skill

**Core Fantasy:** "Her şeyi yakıyorum. Ama önce ritmi buluyorum."
**Kaynak:** Mana (0-100, +8/sn) + Elemental State (Fire veya Frost)

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | Fireball | ↑▶ | Core | Orta hasar + ateş DoT 4s, Fire State+1 | 3 ard arda → 3.'de Living Bomb ücretsiz |
| 2 | Frostbolt | ⬡↑ | Core | Orta hasar + %30 slow 3s, Fire State tüketir | Fireball DoT aktifken → Shatter (+%60 hasar) |
| 3 | Living Bomb | ⚡↓ | Core | 5s sonra patlama, öldürünce 3 komşuya kopyalanır | Frostbolt slow → patlama yarıçapı 2× |
| 4 | Blink | ▶⚓ | Core | 6m ışınlanma, geçilen düşmanlara hasar, sonraki spell +%20 | Düşmanın içinden geçilirse → 0.5s stun |
| 5 | Frozen Orb | ⬡⚓ | Core | Yavaş hareket eden küre, yolundakileri 5s chill | Orb üzerinden Blink → Orb patlar, chill=Frozen 2s |
| 6 | Arcane Blast | ↑↓ | Core | Her cast +%20 hasar ama +%30 mana maliyet, 4. cast Barrage açar | Colossus Smash window → cap kaldırılır |
| 7 | Meteor | 💥⬡ | Core | 1.5s kanal → büyük AoE knockdown | Frozen/slowed hedef → knockdown 3s + hasar +%50 |
| 8 | Mirror Image | ⚓ | Core | 2 kopya 8s, her kopya random skill atar, hasar önce kopyaya gelir | Kopyalar ölünce → ölüm patlaması AoE |
| 9 | Chain Lightning | ⚓✦ | Advanced | 5 hedefe sekiyor, her seki ayrı hasar | Yavaşlamış hedef → 7 seki |
| 10 | Mana Shield | ↑⚓ | Advanced | 6s: hasar HP yerine Mana'ya gelir | Mana %100'ken aktive → +2s süre |
| 11 | Combustion | ✦▶ | Advanced | 8s: tüm Fire spell'ler instant cast, mana maliyet ×2 | Fire State 5 stack → mana maliyet artışı yok |
| 12 | Blizzard | ⬡↑ | Master | 3s kanal → 5m alana 8s devam eden slow+tick hasar | Meteor'dan önce → Meteor knockdown 4s'e çıkar |

**Build eksenleri:** "Fire Burst" / "Frost Control" / "Arcane Storm"

---

### 🗡️ ROGUE — Mevcut 12 Skill

**Core Fantasy:** "Görmüyorsun. Zaten geç."
**Kaynak:** Energy (0-100, +15/sn) + Combo Points (0-5)

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | Backstab | ▶↑ | Core | Arkadan: %200 hasar+3CP. Önden: normal | Shadowstep sonrası → +%50 hasar |
| 2 | Hemorrhage | ↑ | Core | Bleed 8s DoT+2CP, öldürünce yakına yayılır | Bleed aktif hedefe Rupture → hasar +%100 |
| 3 | Rupture | 💥↓ | Core | 5CP finisher: bleed+hasar, CP'ye göre süre uzar | Zaten bleed varsa → birikmiş hasar anında patlar |
| 4 | Shadowstep | ▶⬡ | Core | Hedefe anında ışınlan 8m, 0.5s stun, Energy-25 | Evasion aktifken → CD sıfırlanır |
| 5 | Kidney Shot | ⬡↓ | Core | 5CP: 4s stun, CP'ye göre uzar | Mortal Strike aktifken → stun'da iyileşme yok |
| 6 | Ambush | 💥▶ | Core | Sadece stealth'ten: %300 hasar+4CP+%20 slow | 3s+ stealth → Cold Blood +%100 ekstra hasar |
| 7 | Fan of Knives | ⚓⬡ | Core | 360° AoE, tüm aktif debuffları tüm düşmanlara uygular, Energy-40 | Hexer dual → Hexer debuffları da yayılır |
| 8 | Evasion | ⚓↑ | Core | 4s %100 dodge, her dodge=+1CP, kill=CD sıfır | Evasion bitince → sonraki saldırı guaranteed crit |
| 9 | Deadly Poison | ↑▶ | Advanced | 10s silah zehiri: her saldırı ayrı zehir DoT | Hexer dual → her zehir DoT 1 Hex stack uygular |
| 10 | Sprint | ▶⚓ | Advanced | 3s hız +%100, geçilen düşmanlara hasar | Sprint sırasında Backstab → arka pozisyon garantilenir |
| 11 | Preparation | ↑✦ | Advanced | Tüm Rogue skill CD'lerini sıfırla, 90s CD | Evasion aktifken → Preparation kendi CD'si 60s'ye iner |
| 12 | Vanish | ⚓⬡ | Master | Savaşta anlık stealth, 3s, 50s CD | Vanish sonrası Ambush → Cold Blood garantili |

**Build eksenleri:** "Assassin" / "Bleeder" / "Duelist"

---

### 🏹 RANGER — Mevcut 12 Skill

**Core Fantasy:** "Sana ulaşamazsın. Her saniye kayıp veriyorsun."
**Kaynak:** Focus (4m+: +10/sn | 2m-: -20/sn) — Focus 75+: +%25 hasar | Focus 100: next skill free cast

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | Aimed Shot | 💥⚡ | Core | 1.5s şarj → büyük hasar+%50 crit | Concussive Arrow root sonrası → guaranteed instant |
| 2 | Concussive Arrow | ⬡▶ | Core | Knockback 4m + root 2s | Backward Dash sırasında → uzaklık 6m + slow 3s |
| 3 | Serpent Sting | ↑▶ | Core | Zehir DoT 10s + armor debuff -%20 | Disengage+Serpent Sting → daha geniş alana zehir |
| 4 | Explosive Trap | ⬡↑ | Core | Zemine tuzak, 3s sonra patlama+slow | Summoner dual → trap minyona konulabilir (mobil) |
| 5 | Multi-Shot | ⚓↑ | Core | Delici ok: tüm düşmanlardan geçer, her birine Serpent Sting stack | 5+ düşman vurulursa → tüm CD -3s |
| 6 | Disengage | ▶⬡ | Core | 6m geri atla, slow alanı bırak | Disengage+anında Aimed Shot → ekstra hasar |
| 7 | Black Arrow | ↑⚡ | Core | DoT + özel: bu DoT ile ölen düşman 8s ruh bırakır | Summoner dual → ruh minyon sayılır |
| 8 | Volley | ⬡⚓ | Core | 4m alana 3s yağmur, slow+tick | Explosive Trap üzerine → tam kilitlenme combo |
| 9 | Rapid Fire | ⚓↓ | Advanced | 2s kanal: 8 hızlı ok, Focus-30 | Focus 100'de → 10 ok, maliyet yok |
| 10 | Spirit Wolf | ↑⚓ | Advanced | 12s hayalet kurt, bağımsız saldırır | Summoner dual → minyon sayılır |
| 11 | Flare | ⬡✦ | Advanced | Stealth açığa çıkar + 6s slow alanı, Focus+20 | Rogue dual → Vanish iptal eder, CP kaybettirir |
| 12 | Point Blank | 💥⬡ | Master | ≤2m: ×3 hasar + 5m knockback, Focus 100 gerektirir | Disengage sonrası anında → CD sıfırlanır |

**Build eksenleri:** "Sniper" / "Trap Master" / "Hunter Pack"

---

### 👊 BRAWLER — Mevcut 12 Skill

**Core Fantasy:** "Az canken daha tehlikeliyim. Bu hata değil, strateji."
**Kaynak:** Fury (SADECE hasar alarak +15/vuruş; HP düştükçe daha hızlı)

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | Bloodlust Strike | ⚡💥 | Core | Koni saldırı, HP'ye göre hasar artar | Fury %80+ → Slaughter anında açılır |
| 2 | Whirlwind | ⚓↓ | Core | 2s spin AoE, her vuruş savunma -%5 (max -%30) | Savunma -%30 → Fury +20/spin |
| 3 | Frenzied Leap | ▶↑ | Core | Hedefe atla, iniş AoE, hit=CD anında sıfır | 3 farklı hedefe → 5s Frenzy +%50 hasar |
| 4 | Reckless Swing | 💥↓ | Core | Devasa tek hasar, 2s tam savunmasız | Savunmasızken hasar alırsan → Fury+40 + 0.8s invuln |
| 5 | Bloodthirst | ⚓↑ | Core | Hızlı 5 vuruş, her vuruş küçük iyileşme | HP<%20 + Fury%100 → 8 vuruşa yükselir |
| 6 | Intimidating Shout | ⬡ | Core | 3m çevresinde 3s panik/kaçar | Panikleyen hedefe Bloodlust Strike → +%100 hasar |
| 7 | Barbaric Charge | ▶⬡ | Core | Düz çizgide her şeyi iter, stun/root immune | Duvara çarparsa → stun 2s |
| 8 | Last Rites | 💥 | Core | SADECE HP<%15: %600 hasar, sonra 4s savunmasız | Eğer öldürürse → savunmasızlık 2s'ye iner |
| 9 | Iron Grab | ⬡▶ | Advanced | Yakala (≤2m): 1.5s hold, fırlat, Fury+30 | Fırlatılan 3.'e çarparsa → her ikisi de stun |
| 10 | War Cry | ↑✦ | Advanced | 8s: Fury üretimi ×2 | Fury %50 altında aktive → süre 12s |
| 11 | Shatter Armor | ✦▶ | Advanced | Hedefin savunması -%40, 10s | Warblade Colossus Smash window → savunma -%60 |
| 12 | Death Wish | ⚓↑ | Master | 5s: HP 1 altına düşemez, Fury ×3 hızlı dolar, sonra +%80 hasar | Fury %100'e ulaşırsa içinde → [V] Burst anında tetiklenebilir |

**Build eksenleri:** "Glass Cannon" / "Control Brawler" / "Fury Engine"

---

### ⚖️ PALADİN — Mevcut 12 Skill

**Core Fantasy:** "Hem kesilemiyorum hem öldürüyorum. Bu çelişki değil, tasarım."
**Kaynak:** Holy Power (0-100, Builder/Spender döngüsü)

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | Crusader Strike | ↑▶ | Core | Temel melee + HP+25 | Crusader→Judgment→Crusader → her 3.'de +%60 hasar |
| 2 | Divine Storm | ↑✦ | Core | 360° AoE + HP+15/hedef | 3+ düşman vurulursa → HP+50 |
| 3 | Judgment | ↑⚡ | Core | Ranged holy blast 6m, debuffluysa +%50 hasar | Hexer dual: Hexer debuff → +%100 hasar |
| 4 | Consecration | ↑⚓ | Core | 5s kutsal zemin, tick hasar + HP+5/sn | Warblade Battle Cry combo → patlamalı zemin |
| 5 | Hammer of Wrath | 💥⚡ | Core | SADECE HP<%20 hedefe: büyük hasar + HP+30 | Execute (Warblade dual) → her ikisi de eşik |
| 6 | Avenger's Shield | ⬡↑ | Core | Kalkan fırlat: 3 hedefe sekip silence + HP+15/düşman | 3 farklı hedefe sekerse → her biri 2s slow |
| 7 | Holy Shock | ⚓↑ | Core | Düşman=hasar+HP+15 / kendin=iyileşme | HP<%30'da kendin → iyileşme ×3 |
| 8 | Shield of Retribution | ⚡↓ | Core | 3s blok, birikmiş hasar → AoE serbest | Consecration üzerindeyken → AoE ×2 |
| 9 | Blessed Weapon | ↑✦ | Advanced | 12s: tüm melee +15 HP/hit | HP %80+'ta başlarsa → süre 18s |
| 10 | Lay on Hands | ⚓↓ | Advanced | Anlık tam HP iyileşme, 90s CD | HP sıfırlandıktan sonraki 5s → HP üretim ×2 |
| 11 | Devotion Aura | ✦↑ | Advanced | 8s: sen+minyonlar -%30 hasar, HP+5/sn | Summoner dual 3+ minyon → süre 12s |
| 12 | Divine Sacrifice | ⚡↑ | Master | 5s: minyon hasarının %60'ını üstlen, HP ×3 üretirsin | Fallen Saint (Brawler dual) → üstlenilen hasar Fury doldurur |

**Build eksenleri:** "Tank Rhythm" / "Holy Burst" / "Army Support"

---

### 💀 SUMMONER — Mevcut 12 Skill

**Core Fantasy:** "Ben savaşmıyorum. Feda ediyorum. Ve feda anı en güçlü andır."
**Kaynak:** Charges (0-4, auto +1/8s; minyon ölünce +1 anında)

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | Raise Skeleton | ↑▶ | Core | 1 Charge → melee iskelet (max 3) | 3 iskelet varken → sonraki iskelet +%20 hasar |
| 2 | Summon Golem | ⚓⬡ | Core | 2 Charge → 1 tank Golem. HP<%20=patlama AoE | Golem'e Commanding Strike → duvara çarpma stun |
| 3 | Rally Cry | ✦ | Core | Tüm minyonlar +%20 hasar+hız 10s | Karışık minyon tipleri → bonus +%40 |
| 4 | Corpse Explosion | 💥⚡ | Core | Düşman veya minyon cesedini patlatır, AoE | 3+ cesetle → zincir patlama |
| 5 | Death Nova | ⚡⬡ | Core | 1 minyonu feda: 8s zehir bulutu | Hexer dual → zehir bulutu Hexer debufflarını yayar |
| 6 | Commanding Strike | ✦⬡ | Core | Seçili minyon ×4 hasar+invuln; minyon yoksa Summoner ×2 | Golem'e emir → hedefi duvara çarpar |
| 7 | Blood for Power | ↑↓ | Core | Minyon feda → Charge+1 + tüm CD -%30 | 3 minyon feda → Charge+3 + [V] meter +30 |
| 8 | Bone Shield | ⚓↑ | Core | 3s: minyon kalkan olur, hasar absorbe → Charge+1 | Golem kalkan → absorbe ×2 |
| 9 | Raise Archer | ↑⚓ | Advanced | 1 Charge → uzak archer iskelet (max 2) | 2 archer+1 melee → Rally Cry +%50 |
| 10 | Sacrificial Pact | ↓⚓ | Advanced | Tüm minyonlar intihar: HP+%8/minyon + tüm Charge geri | Boss fight tüm feda → +%40 HP tek seferlik |
| 11 | Dark Pact | ↑ | Advanced | HP -%12 → Charge olmadan minyon çağır | Brawler dual → HP kaybı Fury doldurur |
| 12 | Lich Form | ✦⚓ | Master | 10s: Summoner ghostal (melee immune), minyonlar +%60 hasar | Lich Form sırasında feda → hasar ×3 |

**Build eksenleri:** "Sacrifice Engine" / "Army Commander" / "Lich Burst"

---

### 🔮 HEXER — Mevcut 12 Skill

**Core Fantasy:** "Sabır. 10'a gelince sen bitiyorsun."
**Kaynak:** Hex Stacks (0-10/düşman, 5s decay)
**Faz sistemi:** 0-3 Debuff / 4-6 Pressure (+%20 güç) / 7-9 Overload (+%30 hasar alır) / 10 HEXBLAST

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | Corruption | ▶↑ | Core | Anında 3 stack + 4s orta DoT | Corruption→Agony ard arda → Agony ×2 hızlı |
| 2 | Agony | ↑ | Core | Süregelen DoT, 2 stack/tick, durdurulamaz | Pressure Phase → tick 3'e çıkar |
| 3 | Pandemic | ✦⚡ | Core | Bir hedefteki TÜM stack'ları yakın düşmanlara kopyalar | Overload Phase hedef → kopyalanan stack +3 |
| 4 | Hexblast | 💥↓ | Core | 10 stack patlaması: %100/stack, kill=CD sıfır | 3+ hedef Pressure Phase → zincirlenerek tüm odaya |
| 5 | Empathy | ⚡⬡ | Core | Lanet: saldırıların %30'u kendine döner | Overload Phase → dönen hasar %60'a çıkar |
| 6 | Haunt | ↑⬡ | Core | Hayalet bağla: takip eder+tick+3 stack, 10=otomatik Hexblast | Fan of Knives (Rogue dual) → Haunt tüm düşmanlara |
| 7 | Unstable Affliction | 💥⚡ | Core | Dispel/heal edilirse → anında patlama+stun | Stun/CC altında hedef → guaranteed full stack |
| 8 | Enervate | ⬡✦ | Core | Hız -%50, saldırı hızı -%40, 10s | 5+ stack hedef → süre ×2 |
| 9 | Mass Hex | ▶⬡ | Advanced | Görüntüdeki TÜM düşmanlara 2 stack, HP -%8 maliyet | Pressure Phase düşmanlar varsa → 3 stack |
| 10 | Silence Hex | ⬡↑ | Advanced | 5s burst suppressed + 3 stack | Boss fight 7+ stack → silence 8s |
| 11 | Cursed Mirror | ⚡↑ | Advanced | 8s: sana uygulanan her debuff → düşmana %50 güçle yansır | Enervate sana uygulanırsa → düşman kendi slow'unu alır |
| 12 | Soul Bargain | 💥↓ | Master | HP -%25 feda → hedefe anında 5 stack | Hedef zaten Overload (7-9) → anında 10, Hexblast |

**Build eksenleri:** "Patient DoT" / "Mass Hexer" / "Soul Burst"

---

## BÖLÜM 1 — SKILL ÖNERİLERİ (ANA GÖREV)

Her sınıf için aşağıdaki görevleri yap:

### Görev 1A — Zayıf Skill Tespiti
Mevcut 12 skill'i analiz et. Her sınıf için **en az 2, en fazla 4** skill işaretle:
- "Bu skill WoW/FFXIV/PoE'den kopya — mekanik olarak yeni değil, neden seçeyim?" hissi verenler
- "Bu sınıfın core fantasy'sine hizmet etmiyor" olanlar
- "Bu diğer skill'le çok benzer işlev yapıyor — gereksiz tekrar" olanlar

### Görev 1B — YENİ SKİLL ÖNER
Tespit ettiğin zayıf skill'lerin yerini alacak **somut yeni skill'ler** yaz. Her öneri şu formatta olacak:

```
**[Sınıf] — [Eski Skill] YERİNE → [Yeni Skill Adı]**

Tag: [1-2 tag]
Tier: [Core / Advanced / Master]
Efekt: [Tam mekanik açıklama — sayılarla]
Chain Koşulu → Bonus: [Hangi skill sonrası tetiklenirse ne olur]
Neden bu sınıfa uyuyor: [Core fantasy ile bağlantı]
Hangi referans oyundan ilham aldı: [Oyun adı + mekanik]
Build ekseni: [Bu skill hangi build eksenini güçlendiriyor]
```

### Görev 1C — EKSİK BUILD EKSENİ TESPITI
Her sınıfın 3 build ekseni var. Şu soruları sor:
- Eksik bir 4. build ekseni var mı? (Örneğin Warblade'in "tanky sustain" ekseni yok)
- Herhangi bir build ekseni "bu 4 skill bulunursa garantili OP" değil mi, yani zaten güçlü değil mi?
- Dual-class olan sınıflara özel bir build ekseni eksik mi? (Örneğin Summoner+Ranger cross-class ekseni)

---

## BÖLÜM 2 — HEROIC/LEGENDARY SKİLL SİSTEMİ TASARIMI

Bu oyunda henüz tasarlanmamış ama eklenebilecek bir sistem: **Heroic/Legendary skill'ler.**

Bu kategorideki skill'ler normal 12 skill havuzunun dışındadır. Çok daha güçlü, çok daha özgün. Oyuncunun "Bu run'da bu skill vardı" diye sonraki gün arkadaşına anlattığı skill'ler.

### Ne Zaman Geleceğine Dair Seçenekler (her birini değerlendir)

**Seçenek A — Sadece Son Boss (Act 3)**
- Avantaj: Gerçekten efsanevi his. Run'un doruk noktası.
- Dezavantaj: Run bitmek üzere, bu skill'le sadece 1-2 boss.
- Soru: Oyuncu bu skill'i kazandığında "çok geç" hisseder mi?

**Seçenek B — Her Act Boss'u (3 tane/run)**
- Avantaj: Her act'te bir momentum kırılma noktası var.
- Dezavantaj: Çok fazla legendary skill birikiyor, seyreltiliyor mu?
- Soru: Her act boss'undan 1 tane → run sonu en fazla 3 legendary slot, bu Slot 5-6 gating ile çelişir mi?

**Seçenek C — Nemesis Çözümü Ödülü**
- Run başında belirlenen Nemesis Elite Act 3'te çözülünce legendary skill.
- Avantaj: Oyuncuyu kasıtlı olarak Nemesis'i takibe iter (güçlü anlatı loop).
- Dezavantaj: Her run'da sadece 1, ve sadece Nemesis'i kovalayan oyuncular alabilir.

**Seçenek D — Boss Soul Zincirinin Doruk Noktası**
- Mevcut sistemde Boss Soul skill'leri Act 1 ENHANCE → Act 2 CORRUPT → Act 3 ASCEND.
- ASCEND zaten güçlü. ASCEND üstü bir "TRANSCEND" seviyesi mi olsun?
- Bu seçenekte Heroic = "Act 3 ASCEND olan skill TRANSCEND'e yükseliyor."

**Seçenek E — Cross-Class Milestone**
- Dual-class'ta belirli bir koşul sağlanınca (örneğin: cross-class pasif 20 kez tetiklendi) legendary skill sunuluyor.
- Avantaj: Build'i ne kadar iyi oynadığına göre kazanıyorsun, random değil.
- Dezavantaj: Yeni oyuncular asla göremeyebilir.

### Tasarım Soruları

1. **Kaç tane / run?** 1 mi ideal, 3 mü, hiç sınır yok mu?
2. **Slot sorunu:** Mevcut sistemde 6 slot var (slot 5-6 gating var). Legendary skill 7. slot mu açıyor, yoksa mevcut slotlardan birini mi dönüştürüyor? Yoksa ayrı bir "Legendary Slot" mu?
3. **Dual-class uyumu:** Legendary skill hangi sınıfa ait? Her iki sınıfa da mı ait olabilir? Yoksa sadece cross-class özel legendary'ler mi?
4. **Görsel/ses dili:** Legendary skill geldiğinde oyuncu nasıl anlamalı? Hades'in Legendary Boon ekranı, Slay the Spire'ın rare card animasyonu gibi örnekler var. En doğru his nasıl yaratılır?
5. **Seyreltme riski:** Çok çok güçlü skill → oyuncu her run'da bunu bulmaya çalışır → diğer skill'ler değersizleşir. Bu riski nasıl yönetirsin?

### Referans Analizi

Bu konuları araştır ve yukarıdaki sorulara referans göstererek cevap ver:
- **Slay the Spire — Rare card sistemi:** Rare card'lar ne sıklıkta çıkıyor, oyuncu davranışını nasıl etkiliyor?
- **Hades — Legendary/Duo Boon:** Legendary Boon ne zaman sunuluyor? Oyuncular ilk gördüklerinde ne hissediyor?
- **Path of Exile — Unique item:** "Build-defining unique" kavramı. Bir tane bile build'i nasıl dönüştürüyor?
- **Binding of Isaac — Trinkets vs Active items:** Güç seviyesi farkı nasıl hissettiriyor?
- **Risk of Rain 2 — Legendary item (kırmızı):** Her run'da kaç tane çıkıyor, oyuncular için ne ifade ediyor?

### Benim Oyunuma Önerilen Heroic Sistem

Araştırma ve analizin bittikten sonra, **senin önerdiğin Heroic/Legendary sistemi** şu formatta yaz:

```
## ÖNERİLEN HEROİC SİSTEM

**Ne zaman çıkar:** [Seçenek + varyant]
**Kaç tane/run:** [Sayı + neden]
**Slot yönetimi:** [Mevcut sistemle nasıl entegre]
**Dual-class ilişkisi:** [Nasıl çalışır]

**Her sınıf için 1 örnek Heroic skill:**

### Warblade — [Heroic Skill Adı]
Efekt: [Güçlü, özgün, "WTF" hissettiren efekt]
Ne zaman/nasıl kazanılır: [Koşul]
Neden efsanevi: [Bu skill olmadan mümkün olmayan şeyi ne yapıyor]

### Elementalist — [Heroic Skill Adı]
[aynı format]

[8 sınıf için devam et]
```

---

## BÖLÜM 3 — ÇAPRAZ SINIF SKILL SİNERJİSİ BOŞLUKLARI

28 kombinasyondan bazılarının skill bazında çok az sinerji ürettiğini düşünüyorum. Mevcut skill listelerine bakarak:

1. **En az skill sinerjisi olan 5 dual-class kombinasyonunu** tespit et (iki sınıfın skill'leri birbirini tetikleyecek chain koşulu neredeyse yok)
2. **Bu boşluğu doldurmak için** her biri için **1 yeni skill öner** — yeni skill ya sınıf A'nın havuzuna girer, ya sınıf B'ye, ya da cross-class özel olur
3. Öneri formatı yine Görev 1B ile aynı olacak

---

## ÇIKTI FORMATI

Her bölüm için şu başlıkları kullan:

```
## BÖLÜM 1 — SKILL ÖNERİLERİ

### [Sınıf Adı]

**Zayıf olarak tespit ettiğim skill'ler:**
1. [Skill Adı] — [Neden zayıf/tekrar/kopya]
2. ...

**Yeni öneri:**
[Görev 1B formatında]

**Eksik build ekseni:**
[Varsa]

---

## BÖLÜM 2 — HEROİC/LEGENDARY SİSTEM

### Seçenek Değerlendirmesi
[Her seçenek için artı/eksi]

### Önerilen Sistem
[Yukarıdaki format]

---

## BÖLÜM 3 — ÇAPRAZ SINIF BOŞLUKLAR

[5 kombinasyon + öneriler]
```

---

## ÖNEMLİ NOTLAR

- **Araştırma özeti istemiyorum.** Analiz ve öneri istiyorum.
- Tüm çıktıyı **Türkçe** yaz.
- Sayısal değerler ver — "daha güçlü hasar" değil, "%150 hasar" gibi.
- Mevcut tag sistemine uymayan öneri yapma. Yeni tag öneri yapabilirsin ama neden gerekli olduğunu açıkla.
- Solo dev kısıtını unut. Bu bölüm tasarım, scope değil.
