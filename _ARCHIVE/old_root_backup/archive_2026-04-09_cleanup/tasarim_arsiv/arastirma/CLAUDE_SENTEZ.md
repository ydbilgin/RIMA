# CLAUDE SENTEZİ — Ollama V2+V3 Analizi × Gemini Çıktısı
*2026-03-25 | Kaynak: SKILL_ARASTIRMA_V2 + V3 + gemini_strateji_ve_yeni_konseptler.md*

---

## BÖLÜM 0 — ÖNCELİKLE: ARAŞTIRMADAN ÇIKARDIM BUNLARI

Ollama'nın deepseek-r1:14b çıktıları iki sorun taşıyor:
1. V2 bölümlerinde Çince karakterler var, "karatek" placeholder'lar var — bunlar draft kalitesi
2. V3'te Elsword, Dragon Nest, 2D Sentez bölümleri Ollama connection error ile boş kaldı

**Bu sentezde sadece temiz veriyi kullandım.** Eksik bölümler için kendi MMORPG bilgimi devreye aldım — ama kaynak göstererek.

---

## BÖLÜM 1 — 8 SINIF: TAM SKİLL TABLOSU

> Kaynak kural: Her skill için DNA oyununu belirt. "Neden iyi hissettiriyor" mutlaka yaz.

---

### WARRIOR
**Core Fantasy:** "Duruyorum, geçemiyorsun. Ve ben gittikçe tehlikeliyorum."
**Kaynak Sistemi:** Rage — hasar alarak VE vererek dolar. Sadece vererek doldurmak YOK.
**Tasarım Felsefesi:** WoW Arms Warrior + KO Fighter. Kısa ama yıkıcı burst pencereler.

| Slot | İsim | Tip | Mekanik | CD | Proc Koşulu | DNA |
|------|------|-----|---------|-----|-------------|-----|
| Q | **Ground Stomp** | CC | Yakındaki düşmanları 2s stun eder | 10s | — | KO Ground Hammer |
| W | **Cleave** | Hasar | Koni AoE, Rage +15 | 8s | Ground Stomp sonrası → +50% hasar | WoW Sweeping Strikes |
| E | **Focused Strike** | Hasar | Aynı düşmana 3. vuruş → guaranteed crit | 6s | Rage 60+ ise → crit × 1.5 | BDO Critical Stack |
| R | **Battle Cry** | Buff+Taunt | 8s süre, düşmanlar Warrior'ı hedefler, +20% hasar al ama +40% hasar ver | 15s | Iron Will aktifken kullanılırsa → invuln 1s | L2 Berserker Rage |
| Pasif 1 | **Bloodlust** | Proc | Her kill → 3s süre %8 lifesteal | — | Kill → tetiklenir | WoW Bloodlust |
| Pasif 2 | **Iron Will** | Reaktif | HP %30 altına düşünce → 3s süre %40 hasar azaltma | — | HP eşiği | KO Iron Will |
| Ultimate | **Berserker's Rage** | Transform | 15s süre: defense ignore, +200% hasar, ekran kırmızı | 60s | Rage %100 gerekir | D2 Berserk + WoW Enrage |

**"Bu Build İnsane" Moment:** Battle Cry → Rage %100 → Berserker's Rage → Iron Will proc → ölümsüzsün ve hasar veriyorsun. 15 saniye boyunca düşmanlar sana koşuyor, sen de hepsini eziyorsun.
**Best Pair:** Berserker (W+B sinerjisi aşağıda), Hexer (CC + debuff)
**Worst Pair:** Ranger (çok farklı range gereksinimi)
**Hidden:** Warrior + Paladin → Battle Cry taunt + Holy Endurance heal = kendi kendini döngüde tutar

---

### MAGE
**Core Fantasy:** "Her şeyi yakıyorum. Ama önce soğutuyorum."
**Kaynak Sistemi:** Mana — hızlı regen, ama büyük büyüler mana açıyor
**Tasarım Felsefesi:** FFXIV Black Mage rotation ritmi. "Enochian durumu" gibi — Fire buff aktifken Ice kullanma. Ama basit hali.

| Slot | İsim | Tip | Mekanik | CD | Proc Koşulu | DNA |
|------|------|-----|---------|-----|-------------|-----|
| Q | **Fireball** | Hasar | Tek hedef, ateş hasarı, DoT 3s | 8s | — | WoW Fire Mage |
| W | **Ice Nova** | CC | 3m çevresel slow %40, 2s | 10s | Fireball'dan sonra → target frozen 1s | FFXIV Freeze combo |
| E | **Blink** | Mobility | 6m teleport, sonraki büyü +20% hasar | 12s | — | WoW Mage Blink |
| R | **Arcane Surge** | Hasar | Şarj 1.5s → büyük patlama, %300 hasar, AoE | 20s | Mana %80+ ise anında ateşle (şarj yok) | ArcheAge Sorcery |
| Pasif 1 | **Fire Focus** | Proc | Fireball hit → diğer büyülerin CD -2s | — | Her Fireball | FFXIV Firestarter proc |
| Pasif 2 | **Burnout Recovery** | Reaktif | Hasar alınca mana +10 | — | Her hasar | GW1 Energy management |
| Ultimate | **Firestorm** | AoE | 5s süre arena-wide yağmur, mana tükenirse durur | 45s | Mana %50+ gerekir | GW1 Elementalist |

**"Bu Build İnsane" Moment:** Fireball → Ice Nova (freeze) → Blink (teleport yakın) → Arcane Surge (anında çünkü mana yüksek) → Fire Focus proc × 2 → tüm CD'ler azaldı → tekrar başla.
**Best Pair:** Hexer (DoT yığma × ateş), Berserker (kite + burst combo)
**Worst Pair:** Summoner (minyon yönetimi Mage'in ritmini bozar)
**Hidden:** Mage + Rogue → Blink + Shadow Strike = iki teleport → maksimum pozisyon manipülasyonu

---

### ROGUE
**Core Fantasy:** "Göremiyorsun. Ama canın gidiyor."
**Kaynak Sistemi:** Energy — hızlı regen, stealth girince dondurulur
**Tasarım Felsefesi:** WoW Subtlety Rogue. Her öldürme bir kombonun finali.

| Slot | İsim | Tip | Mekanik | CD | Proc Koşulu | DNA |
|------|------|-----|---------|-----|-------------|-----|
| Q | **Hemorrhage** | Hasar+DoT | Bleed 8s, 2 combo point üretir | 6s | — | WoW Hemorrhage |
| W | **Shadow Strike** | Mobility+Hasar | Hedef arkasına teleport, %150 hasar, 3 combo | 15s | Stealth sonrası → %250 hasar | BDO Dark Knight Shadow |
| E | **Rupture** | CC | Knockback + 1s stun, combo reset | 12s | Bleeding hedef → stun 2s | ArcheAge Shadowplay |
| R | **Eviscerate** | Finisher | 5 combo gerekir → %400 hasar | 0s CD / combo gated | Target %30 altında → %600 | WoW Eviscerate |
| Pasif 1 | **Parry Riposte** | Reaktif | Gelen saldırıya zamanlamalı basınca → energy +30 + sonraki saldırı guaranteed crit | — | Player reaksiyonu | BnS Counter Frame |
| Pasif 2 | **Shadow Focus** | Aura | Stealth'te +50% mov speed, hasar %30 azalır | — | Stealth süresince | L2 Dagger Stealth |
| Ultimate | **Marked for Death** | Debuff | 12s süre: Rogue'nin tüm saldırıları +100% hasar, stealth CD 0 | 45s | 5 combo gerekir | WoW Shadow Dance |

**"Bu Build İnsane" Moment:** Stealth → Shadow Strike (arkaya, %250) → Hemorrhage (bleed, 2 combo) → Hemorrhage (bleed refresh, 4 combo) → Rupture (bleed hedef = 2s stun) → Eviscerate (%600). Bütün bu süre 4 saniye.
**Best Pair:** Hexer (debuff → Rogue özgürce vuruyor), Ranger (uzak setup + yakın finisher)
**Worst Pair:** Summoner (minyon yönetimi Rogue'un kısa pencerelerini bozar)
**Hidden:** Rogue + Paladin → Parry Riposte + Holy Power = her parry hem criti hem Holy Power üretiyor

---

### RANGER
**Core Fantasy:** "Seni yavaş yavaş öldürüyorum. Ve sen bana ulaşamıyorsun."
**Kaynak Sistemi:** Cooldown-only — resource bar yok, tüm güç zamanlama ve pozisyonda
**Tasarım Felsefesi:** GW1 Ranger expertise sistemi + WoW Hunter dead zone'un TERSİ. Dead zone yok, ama "sweet spot range" var — orta uzaklıkta en yüksek hasar.

| Slot | İsim | Tip | Mekanik | CD | Proc Koşulu | DNA |
|------|------|-----|---------|-----|-------------|-----|
| Q | **Concussive Arrow** | CC | 4m knockback + 2s root | 10s | — | ArcheAge Ranger |
| W | **Charged Shot** | Hasar | 1.5s şarj → çok yüksek hasar, kısa CD reset if crit | 15s | Rooted hedef → anında ateş (şarj yok) | BDO Archer |
| E | **Backward Dash** | Mobility | 5m geri atlama + hemen ardından otomatik ateş | 8s | — | BDO kite mekanik |
| R | **Explosive Trap** | Kontrol | Zemine yerleştir, düşman basınca patlama + slow 4s | 18s | 3 trap aynı anda → bölge kapatır | GW1 Trap Ranger |
| Pasif 1 | **Expertise** | Aura | Tüm CD'ler %15 daha hızlı | — | Sürekli | GW1 Expertise attribute |
| Pasif 2 | **Dead Zone Mastery** | Proc | Düşman yakınlaşınca (2m) → Backward Dash otomatik tetiklenir | — | Mesafe eşiği | WoW Hunter kite |
| Ultimate | **Rain of Arrows** | AoE | 4s süre tüm alanı yağmurlar, her saniye hasar | 50s | — | GW1 Barrage / BDO Archer ultimate |

**"Bu Build İnsane" Moment:** Concussive Arrow (root) → Charged Shot (anında, şarj yok = %200) → Backward Dash otomatik (mesafe koru) → Expertise pasifi sayesinde trap hazır → Explosive Trap → düşman slow'da, sen Rain of Arrows.
**Best Pair:** Rogue (uzak setup + yakın finisher), Warrior (Warrior yakında tankar, Ranger uzaktan vurar)
**Worst Pair:** Warrior (ikisi de farklı range istiyor — sinerjisiz)
**Hidden:** Ranger + Hexer → Concussive Arrow push + Debuff Threshold = düşman her seferinde uzaklaşınca debuff birikimi sıfırlanmadan geri gelir

---

### BERSERKER
**Core Fantasy:** "Ölmek üzereyim. Ve bu beni daha tehlikeli yapıyor."
**Kaynak Sistemi:** Fury — hasar ALARAK dolar. Vermek doldurmaz. Fury = can riskini ödüllendiriyor.
**Tasarım Felsefesi:** WoW Enrage + D2 Berserk + PoE Berserker Ascendancy. Düşük HP = güç skalası.

| Slot | İsim | Tip | Mekanik | CD | Proc Koşulu | DNA |
|------|------|-----|---------|-----|-------------|-----|
| Q | **Bloodlust Strike** | Hasar | Koni saldırı. HP %30 altında → +%100 hasar | 8s | — | WoW Enrage |
| W | **Slaughter** | Hasar | Çizgi saldırı, knockdown şansı %50 | 12s | Bloodlust Strike sonrası → %50 knockdown garantiye döner | D2 Berserk |
| E | **Frenzied Leap** | Mobility | Hedefe atla, AoE. Düşmana inersen CD reset | 18s | Fury %80+ → CD 10s'e düşer | PoE Berserker |
| R | **Reckless Abandon** | Buff | 6s süre: tüm hasar %50 artar ama savunma %30 azalır | 20s | — | WoW Bloodrage |
| Pasif 1 | **Unyielding** | Reaktif | HP %70 üstünde → gelen hasarın %20'si Fury'e dönüşür | — | HP eşiği | PoE Berserker node |
| Pasif 2 | **Ruthless** | Proc | HP %30 altında → atk speed +%15, lifesteal +%5 | — | HP eşiği | WoW Execute window |
| Ultimate | **Berserk Mode** | Transform | 15s: defense ignore, +200% hasar, +%50 hız | 60s | Fury %100 gerekir | D2 Berserk + WoW Berserk |

**"Bu Build İnsane" Moment:** HP %25'e düşüyor → Ruthless aktif → Frenzied Leap CD kısaldı → atlıyorsun, AoE → Fury %100 → Berserk Mode → 15 saniye boyunca her şeyi öldürüyorsun, ölmüyorsun çünkü lifesteal yüksek.
**Best Pair:** Paladin (Holy Endurance → Berserker'ı tam ölü noktada tutar), Summoner (minyon ölümleri Fury dolduruyor?)
**Worst Pair:** Mage (Mage uzaktan oynar, Berserker yakın olmak zorunda)
**Hidden:** Berserker + Paladin → Berserk Mode aktifken Avenging Wrath tetiklenirse → 15s süre tam invulnerability + maksimum hasar. Unintended broken combo.

---

### PALADIN
**Core Fantasy:** "Kesilemem. Ve bu bana güç veriyor."
**Kaynak Sistemi:** Holy Power — rhythmic build/spend. 0'dan 100'e çıkar, 100'de spender skill açılır.
**Tasarım Felsefesi:** WoW Ret Paladin ritmi + FFXIV Paladin "party buffer" hibrit rolü.

| Slot | İsim | Tip | Mekanik | CD | Proc Koşulu | DNA |
|------|------|-----|---------|-----|-------------|-----|
| Q | **Crusader Strike** | Hasar | Melee hit, +20 Holy Power | 5s | — | WoW Crusader Strike |
| W | **Consecration** | AoE+Savunma | Ayak altı AoE 4s, gelen hasar -%30 | 12s | Holy Power %60+ → CD 8s | WoW Consecration |
| E | **Starfire Strike** | Hasar | Instant holy magic saldırı, +15 HP | 10s | Crusader Strike sonrası 2s içinde → free cast | FFXIV Paladin Requiescat |
| R | **Shield of Justice** | Savunma+CC | 3s tam savunma, sonra knockback | 20s | HP %30 altında → CD 10s | WoW Shield of the Righteous |
| Pasif 1 | **Divine Focus** | Proc | 5 yard içinde düşmana saldırı → CD -10%, 5s | — | Yakın saldırı her birinde | GW1 Smiting Monk |
| Pasif 2 | **Holy Endurance** | Reaktif | Hasar alınca HP'nin %10'u 5s içinde geri kazanılır | — | Her hasar | WoW Paladin sustain |
| Ultimate | **Avenging Wrath** | Transform | 10s: %30 invuln + %50 hasar artışı + knockback aura | 90s | Holy Power %100 gerekir | WoW Avenging Wrath |

**"Bu Build İnsane" Moment:** Düşük HP + Holy Power %100 → Avenging Wrath (invuln+hasar) → Holy Endurance sürekli iyileştiriyor → Shield of Justice CD düşük → taş gibi duruyorsun ve düşmanlar senden kaçıyor.
**Best Pair:** Berserker (aşağı bak: B+P = Broken), Warrior (her ikisi de frontliner)
**Worst Pair:** Mage (tamamen farklı oynanış ritmi)
**Hidden:** Paladin + Rogue → her Parry Riposte hem Energy hem Holy Power biriktiriyor

---

### SUMMONER
**Core Fantasy:** "Ben savaşmıyorum. Ordularım savaşıyor. Ben yönetiyorum."
**Kaynak Sistemi:** Charges — zamanla dolar (her 8s +1 charge, max 4). Minyon sayısı = karmaşıklık.
**Tasarım Felsefesi:** D2 Necromancer + GW1 Minion Master. Kalabalık yönetimi, pozisyon okuma.

| Slot | İsim | Tip | Mekanik | CD | Proc Koşulu | DNA |
|------|------|-----|---------|-----|-------------|-----|
| Q | **Summon Minion** | Summon | 1 Charge → 1 minyon (3 tip: Fighter/Archer/Exploder) | 0s (charge gated) | — | D2 Necro Raise Skeleton |
| W | **Rally Cry** | Buff | Tüm minyonlara +20% atk+spd 10s | 12s | 3+ minyon varsa → +40% | GW1 Minion Master |
| E | **Commanding Strike** | Kontrol | Seçilen minyon → hedefli güçlü saldırı | instant | — | D2 Necro Revive targeting |
| R | **Corpse Explosion** | AoE | Ölen düşman/minyon cesedini patlatır, büyük AoE | 8s | 3+ cesetle patlatılırsa → tüm alanda | D2 Corpse Explosion |
| Pasif 1 | **Army's Strength** | Aura | Her canlı minyon → +5% tüm hasar, max 3 stack | — | Minyon sayısı | GW1 Death Magic |
| Pasif 2 | **Blood for Power** | Reaktif | Minyon ölünce → 1 geçici Charge + CD'ler -30%, 5s | — | Minyon ölümü | WoW Warlock soul system |
| Ultimate | **Sacrificial Purge** | AoE Nuke | Tüm minyonları feda et → devasa AoE patlama | 40s | 3+ minyon gerekir | GW1 "We Shall Return" |

**"Bu Build İnsane" Moment:** 3 minyon → Rally Cry → Commanding Strike (Exploder hedef) → minyon ölünce Blood for Power → Corpse Explosion (cesetle) → tüm alanda patlama. Sonra Sacrificial Purge ile kalanları bitir. Chaos.
**Best Pair:** Hexer (minyonlar debuff yığar, Hexplode ile patlat), Berserker (minyon ölümleri Fury mi? hayır ama: Blood for Power + Berserk Mode sinerji olabilir)
**Worst Pair:** Warrior (Warrior yakın savaşır, minyonlar karmaşa yaratır)
**Hidden:** Summoner + Hexer → minyon her hasar edince debuff stack birikiyor → Summoner Sacrificial Purge + Hexplode = çifte nuke

---

### HEXER
**Core Fantasy:** "Saldırmam gerek yok. Sen kendini mahvediyorsun."
**Kaynak Sistemi:** Hex Stacks — her düşmana ayrı, max 10, 5s decay
**Tasarım Felsefesi:** WoW Affliction Warlock DoT yönetimi + GW1 Mesmer "punishment" tasarımı + PoE Hexblast stack explode.

| Slot | İsim | Tip | Mekanik | CD | Proc Koşulu | DNA |
|------|------|-----|---------|-----|-------------|-----|
| Q | **Pandemic** | DoT | Anında DoT, her 3s 2 stack biriktirir | 0s (instant, 4s CD) | Hedef 3+ stack → DoT ×2 | WoW Pandemic mechanic |
| W | **Empathy** | Punishment | Sonraki 6s: hedef skill kullanırsa kendine hasar verir | 8s | Stack 5+ → skill kullanımı stun'a dönüşür | GW1 Empathy |
| E | **Shadowflame** | DoT+Multi | Fire+Shadow ikili DoT, 2 stack ekler | 10s | Pandemic aktifse → anında 4 stack | D2 Necro Amplify |
| R | **Hexblast** | Finisher | Tüm stack'leri patlatır, stack başına %100 hasar | 12s | Max 10 stack → CD reset | PoE Hexblast |
| Pasif 1 | **Debuff Threshold** | Proc | Düşmanda 5+ stack → tüm hasar +%20 | — | Stack eşiği | PoE Wither stacks |
| Pasif 2 | **Punishing Hex** | Reaktif | Hexer'a saldıran düşmana anında 2 stack + hasar ricochet | — | Her saldırı Hexer'a | GW1 Spiteful Spirit |
| Ultimate | **Hexplode** | AOE Doom | Max stack (10) patlatır, yakındaki düşmanlara 2 stack yayar | 60s | 10 stack gerekir | PoE Hexblast + GW1 Mark of Pain |

**"Bu Build İnsane" Moment:** Pandemic → Shadowflame (4 stack anında) → Empathy (artık saldıramazlar) → Hexblast (6 stack = %600 hasar) → Punishing Hex düşmanlar saldırırsa kendi kendine zarar → Hexplode boss'ta 10 stack → yakınlara yayılıyor → zincirleme patlama.
**Best Pair:** Summoner (minyonlar debuff için ideal hedefler), Mage (Mage'in DoT'ları stack üstüne)
**Worst Pair:** Berserker (Berserker hızlı öldürür, Hexer'ın stack biriktirmeye zamanı olmaz)
**Hidden:** Hexer + Warrior → Battle Cry (düşmanlar Warrior'ı hedefler) + Empathy = düşmanlar saldırdıkça kendi kendine zarar veriyor

---

## BÖLÜM 2 — 28 KOMBİNASYON: ARKETİP İSİMLERİ + NEDEN FARKLI HİSSETTİRİYOR

> Kural: Her arketip adı ArcheAge'in "Darkrunner", "Abolisher", "Daggerspell" geleneğinden ilham alır. İsim = fantezi = oynanış felsefesi.

### S-TİER — BROKEN SYNERGY

| # | Kombo | Arketip Adı | Neden Farklı Hissettirir | Broken Nokta |
|---|-------|-------------|--------------------------|--------------|
| 1 | **Mage + Hexer** | **Plague Caster** | Mage'in DoT'ları (Fireball DoT + Shadowflame) otomatik stack biriktirir. Hexblast patlatınca sayılar patlar. Rotation hissettiriyor ama aslında "kurban yap ve patlat" stratejisi. | Pandemic + Fireball DoT = dakikada 20+ stack. Hexplode her ~15s'de kullanılabilir. |
| 2 | **Berserker + Summoner** | **Blood Shepherd** | Minyonlar düşmanları tutarken Berserker hasarı alır → Fury dolar → Berserk Mode. Sonra minyon feda → Blood for Power → CD'ler sıfır. Çifte tetik mekanizması. | Blood for Power + Berserk Mode aynı anda aktif = 15s boyunca minyonlar CD'leri sıfırlıyor. |

### A-TİER — STRONG + REWARDING

| # | Kombo | Arketip Adı | Neden Farklı Hissettirir | Core Rotation |
|---|-------|-------------|--------------------------|---------------|
| 3 | **Warrior + Mage** | **Runeguard** | Warrior yakında duruyor, Mage blink ile etrafında dönüyor. İki farklı range ama aynı düşman. Bir sınıf savaş açar, diğer bitirir. | Ground Stomp (W) → Ice Nova (M, freeze) → Arcane Surge (M, anında) |
| 4 | **Ranger + Berserker** | **Wild Hunter** | Ranger uzaktan zayıflatır (slow, knockback), Berserker hasarı alarak Fury dolduruyor. Ranger minyatür kite, Berserker yakın baskı — aynı düşmana. | Concussive Arrow → Berserker atlar → Bloodlust Strike (hasarı artık aldı) |
| 5 | **Rogue + Paladin** | **Shadow Saint** | Rogue'un stealth + burst → Holy Power birikiyor (Parry Riposte × 2). Paladin'in Avenging Wrath → Rogue anında yeniden stealth. İki "burst window" katmanlanıyor. | Stealth → Parry Riposte (holy power) → Holy Power %100 → Avenging Wrath → Marked for Death |

### B-TİER — FUNCTIONAL + FLAVOURFUL

| # | Kombo | Arketip Adı | Oynanış Stili |
|---|-------|-------------|---------------|
| 6 | Warrior + Rogue | **Bloodblade** | Warrior açıyor (Ground Stomp), Rogue bitiyor (Eviscerate). Basit ama tutarlı. |
| 7 | Warrior + Ranger | **Siege Master** | Warrior kalabalığı durdurur, Ranger'ın trapleri koridorları kapatır. Bölge kontrolü. |
| 8 | Warrior + Berserker | **Iron Tide** | İki frontliner. Hasar resmen saçılıyor ama sürdürülebilir. Battle Cry + Berserk Mode = 25s toplam transform. |
| 9 | Warrior + Paladin | **Warlord** | Battle Cry (taunt, düşmanlar geliyor) + Consecration (gelenlere AoE hasar). Her tanker'ın hayal ettiği. |
| 10 | Warrior + Summoner | **Commander** | Minyonlar flanklardan geliyor, Warrior merkezde duruyor. Battlefield control. |
| 11 | Warrior + Hexer | **Rune Breaker** | Battle Cry (taunt) + Empathy (saldırılar geri dönüyor). Düşmanlar Warrior'ı vurarak kendileri ölüyor. |
| 12 | Mage + Rogue | **Arcane Shadow** | Blink (M) + Shadow Strike (R) = 2 teleport aynı anda. Neredeysin hiç belli değil. |
| 13 | Mage + Ranger | **Storm Warden** | Rain of Arrows + Firestorm aynı anda = tam alan kapıyor. Pasif oynanış, aktif hasar. |
| 14 | Mage + Berserker | **Chaos Mage** | Arcane Surge + Berserk Mode = iki büyük cooldown. Patlama dönemi. Ama timing zor. |
| 15 | Mage + Paladin | **Templar** | Holy Power ritmi + Mage rotation ritmi. İki ayrı ritim sistemini yönetmek beyin jimnastiği. |
| 16 | Mage + Summoner | **Arcane Lord** | Minyonlar düşmanları tutuyor, Mage kuyruğa dizilmiş AoE saldırıyor. Çok clean. |
| 17 | Rogue + Ranger | **Phantom Hunter** | Uzaktan zayıflat (Concussive + trap), sonra stealth ile içeri gir. Dedektif oyunculara. |
| 18 | Rogue + Berserker | **Bloodfist** | Hemorrhage (bleed) + Ruthless (<30% HP) = bleed oynarken Berserker güç kazanıyor. High risk. |
| 19 | Rogue + Summoner | **Shadow Puppeteer** | Minyonlar dikkat çekerken Rogue arkadan vurur. Klasik ama etkili. |
| 20 | Ranger + Paladin | **Vigilant** | Backward Dash (otomatik mesafe) + Shield of Justice (yakına gelince). İki ayrı savunma katmanı. |
| 21 | Ranger + Summoner | **Beastmaster** | Exploder minyon + Explosive Trap = alan tamamen patlama. Trap-based area denial. |
| 22 | Berserker + Paladin | **Fallen Saint** ⭐ | *Bakınız: Hidden Broken Combo açıklaması* | Berserk Mode + Avenging Wrath = 15+10s transform zinciri |
| 23 | Berserker + Hexer | **Wraith Fury** | Reckless Abandon + Debuff yığma. Berserker saldırırken Hexer stack biriktirir, Berserker Hexblast tetikler. |
| 24 | Paladin + Summoner | **Divine Shepherd** | Minyonlar savaşır, Paladin ritim tutar. Army's Strength → Holy Power → Avenging Wrath. Ağır ama güvenli. |
| 25 | Paladin + Hexer | **Exorcist** | Empathy + Holy Power. Düşman saldırırsa hem kendi kendine zarar verir hem Holy Power birikir. |
| 26 | Summoner + Hexer | **Plague Doctor** ⭐ | *Tematik açıdan oyunun ruhuyla en uyumlu kombinasyon. Bakınız detay.* |

### C-TİER — NICHE / ZAYIF SİNERJİ

| # | Kombo | Arketip Adı | Neden Zayıf |
|---|-------|-------------|-------------|
| 27 | Rogue + Hexer | **Shadowblight** | Rogue hızlı öldürüyor, Hexer'ın stack biriktirmeye zamanı yok. Birbirini frenliyor. |
| 28 | Ranger + Hexer | **Cursed Arrow** | İyi fikir: uzaktan debuff yığma. Ama Hexer'ın Empathy/Punishing Hex yakın savaş gerektiriyor. Konsept çelişkisi. |

---

### DETAY: 3 KRİTİK KOMBİNASYON

#### ⭐ Summoner + Hexer = "PLAGUE DOCTOR"
Bu kombinasyon oyunun ADI ile örtüşüyor (Pestis — Plague Doctor teması).

**Oynanış:**
- Minyonlar sürekli düşmanlara vurarak Pandemic DoT yayıyor (Summoner Commanding Strike → Hexer Pandemic AoE trigger)
- Her minyon vurduğunda Hexer Debuff Threshold aktif oluyor (+%20 hasar)
- Hexer Empathy → düşmanlar Summoner minyonlarına saldırdığında kendi kendilerine zarar veriyor
- Summoner Sacrificial Purge + Hexer Hexplode aynı anda = çifte patlama

**Tematik Uyum:** Veba doktoru tıpkı bu kombinasyon gibi çalışır — hastayı gözlemler, hastalık yayılır, sonunda büyük bir patlama.

**Solo Dev Yapılabilirlik:** Tüm mekanikler bağımsız çalışıyor. Sinerji EMERGENT — yani sistemi kurunca kendiliğinden ortaya çıkıyor, her kombinasyon için ayrı kod yazmana gerek yok.

---

#### ⭐ Berserker + Paladin = "FALLEN SAINT"
Bu oyunun en "broken" hissettiren kombinasyonu — ama aslında tasarlanmış.

**Hidden Broken Interaction:**
- Paladin'in Holy Endurance: hasar alınca HP %10 regen 5s boyunca
- Berserker'ın Ruthless: HP %30 altında aktif, lifesteal veriyor
- Bu ikisi birlikte: Berserker HP'yi kasıtlı düşük tutar → Holy Endurance sürekli hafifçe iyileştiriyor → HP %30 üstüne asla çıkmıyor → Ruthless permanentle aktif kalıyor

**"Ben Şunu Keşfettim" Anı (PoE broken build hissi):** Bunu keşfeden oyuncu foruma yazar. "Paladin/Berserker ile ölümsüz build yaptım" — ama aslında oyun bunu BİLEREK tasarladı. Oyunun merakını uyandırması için böyle bir "sır" olması gerekiyor.

---

#### ⭐ Rogue + Ranger = "PHANTOM HUNTER" — Oyunun Kimliği
Araştırma verisi bunu defalarca "oyunun kimliğini en iyi yansıtan kombinasyon" olarak işaretledi.

**Neden:**
- Rogue = anlık, yakın, risk
- Ranger = uzak, kontrol, setup
- İkisi birlikte = "önce hazırla, sonra bitir" — her roguelite'ın temel döngüsü ama hissettiriyor

**Combat Flow:** Concussive Arrow (root 4m uzaktan) → Charged Shot (hasar) → düşman yaklaşınca Backward Dash (otomatik) → Shadow Strike (stealth teleport backstab) → Eviscerate yok çünkü Rogue skill'i → bu yüzden Hemorrhage → Concussive Arrow yeniden

Bu akış ASLA DURMUYOR. İdeal roguelite combat loop.

---

## BÖLÜM 3 — GRUDGE WAVES: SOMUT UYGULAMA TASARIMI

### 3.1 Temel Mekanik — "Savaş Kartı"

Her elite düşmanın üzerinde küçük bir "Grudge Badge" görünür:
```
[GRAVECRAWLER]
Adaptasyon: 🔥 Ateşe direnç (2x öldürüldü)
Zayıflık:  ❄️ Buz (hiç görmedi)
```

**Kaynak:** BnS combat grammar — her eylemin görünür bir karşılığı var. GW1 PvX wiki kültürü — build bilgisi şeffaf, herkes görebilir.
**Şeffaflık prensibi (Gemini katkısı):** Gizli RNG'yi sevmiyorlar. Grudge kartı tamamen şeffaf — ne biliyorsun, görebiliyorsun.

---

### 3.2 Adaptasyon Tablosu

| Ölüm Türü | Grudge Adaptasyonu | Görsel İpucu | Karşı Strateji |
|-----------|---------------------|--------------|----------------|
| AoE ile öldürüldü | Dağıt formasyonu, %30 AoE direnç | Düşmanlar spread oluyor | Single-target build |
| Bleed ile öldürüldü | Bleed immunity + %5 lifesteal | Kırmızı aura | True damage veya instant burst |
| Stun zinciri | CC immunity süresi +50% | Sarı kalkan animasyonu | DoT veya burst damage |
| Kite edildi | Dash attack kazandı + root | Düşman hız + kırmızı ok | Stand-and-fight |
| Minyon ile öldürüldü | AoE cleave (minyonları vurur) | Mavi aura | Direkt combat |
| Kritik vuruşla | %20 evasion | Gümüş parıltı | Guaranteed hit skills |
| Fire ile öldürüldü | %40 fire resist, cold vulnerability | Buzlu görünüm | Ice Nova |
| Stealth ile | Reveal aura (stealth'i etkisizleştiriyor) | Mor sis | Frontal combat |

---

### 3.3 Boss Tasarımı: "Tüm Run'u İzledi"

Boss odaya girince:
1. **Highlight göster:** "3 run boyunca en çok kullandığın taktik: [Bleed Combo]"
2. **Adaptasyon göster:** "Bu run için bleed immunite + lifesteal hazırlığında"
3. **Zayıflık göster:** "Ama buz saldırısına hiç maruz kalmadı"

Bu tasarım:
- Shadow of Mordor nemesis sistemi (hatırliyor)
- PoE passive tree şeffaflığı (görünür)
- WoW raid preparation kültürü (önceden hazırlanma)

**"Kasıtlı Programlama" Mekanizması:**
Oyuncu önceki odalarda kasıtlı olarak belirli elemanla öldürürse, boss o elemana karşı zayıf kalabilir. Örn: her düşmanı buz ile öldürdüysen boss ateşe hazırlandı ama buz'a güvensiz — ama bunu biliyorsun ve buz build'in de var.

**Tasarım dikkat:** Eğer oyuncu kasıtlı çeşitlilik yaparsa boss daha az adapte oluyor. Tek strateji → çok adapte boss. Çeşitli strateji → orta adapte boss. Bu, Rotation Rogue sütununu güçlendiriyor: çok skill var, hepsini kullanmak mantıklı.

---

### 3.4 "Grudge Codex" — Meta Aktivite (Gemini Katkısı)

Gemini'nin "sequel fatigue" uyarısından: Core loop dışında bir meta-aktivite gerekiyor.

**Önerim:** Hub'da "Grudge Codex" — her karşılaşılan düşmanın kartı birikiyor.
- Düşmanı ilk kez gördüğünde: boş kart
- Her farklı şekilde öldürdüğünde: karta not ekleniyor ("Bu ateşe tepki verdi")
- Boss'ları yenince: boss sayfasına "Bunu şöyle yendim" ekleniyor

Bu hem lore (oyunun Plague Doctor temasıyla uyumlu — doktor hasta kayıtları tutar) hem strateji rehberi. Oyuncu kendi kodeksini yazıyor.

---

## BÖLÜM 4 — İLK 30 SANİYE TASARIMI

> Kaynak: Dragon Nest (iframe + anında feedback), Elsword (ZZZ chain → skill), Lost Ark (identity gauge anında görünür), Hades (Hub → ilk oda geçişi).

### 0-5s: SINIF SEÇİMİ

```
Ekranda 8 kart görünüyor.
Her karta hover edince:
  - 1 saniyelik animasyon klip oynuyor (o sınıfın "THIS IS MY BUILD" anı)
  - 3 kelimelik açıklama: "YAKLAŞ. VUR. KAÇMA."

İki kart seçince:
  - Oluşan kombinasyonun ARKETİP İSMİ beliriyor: "PLAGUE DOCTOR"
  - Aktif olacak 8 skill'den hangisi seçilecek → drag & drop veya sayı tuşları
  - Pasifler otomatik aktif
```

**Tasarım notu:** Seçim ekranı MAX 20 saniye. Timer yok ama animasyon hızlı ve akıcı. Kart seçme fiziksel hissettirmeli (Gemini: "Physicality" — kartlar gerçek nesne gibi).

---

### 5-15s: İLK ODA — FEEDBACK ANINDA GELMELİ

```
Kapı açılır. 3 normal düşman görünür.
Ekranın alt köşesinde:
  Q W E R — skill ikonları, CD göstergesi, proc koşulu göstergesi

İlk Q basışı:
  - Karakter ANİMASYON ÖNCE, hasar SONRA (Dragon Nest prensibi)
  - Hit-stop: 2 frame duraklama
  - Hasar sayısı ekranda beliriyor: "124"

İkinci Q:
  - "COMBO ×1.5" yazısı beliriyor (ilk proc koşulu)
  - Ses farklı — daha güçlü impact

İlk kill:
  - Küçük ışık efekti, "KILL" yazısı değil — sadece "canavar çöküyor"
  - Resource bar (Rage/Fury/HP vs) değişimi görünür
```

---

### 15-25s: İLK PROC ANI

```
Skill Q → W zinciri kurulunca:
  - Ekranda 1 saniyelik "ROTATION!" yazısı
  - Ses efekti: satisfying "click"
  - Hasar sayısı daha büyük renkte beliriyor: "347"

Bu andaki design hedefi (WoW ilk Heroic kill hissi):
  "Bunu YAPTIM. Kaza değildi. Ben bunu bilerek yaptım."
```

---

### 25-35s: İLK ELİTE / GRUDGE GİRİŞİ

```
Kapı açılır. Elite düşman görünür.
Elite'in üzerinde ilk kez Grudge Badge beliriyor:
  [GRAVECRAWLER — YENİ]
  Adaptasyon: (boş)
  Zayıflık: (bilinmiyor)

Oyuncu: "Bu ne demek?"

Savaş sonrası (öldürdükten sonra):
  Badge güncelleniyor:
  Adaptasyon: (bir sonraki görünüşte doldurulacak)
  Zayıflık: ❄️ Buz (bu seferde buz kullandıysan)
```

**Öğretici yok.** Sistem kendini gösteriyor. Oyuncu merak ediyor.

---

## BÖLÜM 5 — GEMİNİ SENTEZİ: NELER EKLEDİ, NE DEĞİŞTİ

### 5.1 Gemini'nin Katkıları (Direkt Uygulanabilir)

| Gemini İçgörüsü | Bizim Oyuna Uygulaması |
|-----------------|------------------------|
| Sequel Fatigue → Meta aktivite şart | Grudge Codex (hub'da düşman kartları biriktirme) |
| Transparency → Gizli RNG'yi sevmiyorlar | Grudge Badge tamamen şeffaf, oyuncu her şeyi biliyor |
| Physicality → Kartlar/objeler fiziksel hissettirmeli | Sınıf seçim ekranı drag & drop, skill kartlar |
| "Information vs Decision" — bilgi ver, kararı oyuncuya bırak | Grudge kartı bilgi verir, strateji oyuncunun |
| SYNTAX_ERROR konsepti — büyü yaratma = programming | Rotation Rogue zaten bu hissi veriyor — görselleştir |
| BAG OF BUFFS — inventory = savaş mekaniği | Skill slot seçimi tetris mantığı: bazı skiller 2 slot alır? |

### 5.2 Gemini'nin Farklı Oyun Konseptleri (Ayrı Değerlendirme)

Gemini 20+ konsept üretti ama bunların çoğu tamamen farklı oyunlar (MEMORY LEAK, CARTOGRAPHER'S LIE, vb.). Pestis için bunların direktiflerini ALAMAYIZ çünkü kapsam dışı.

Ama şunu aldım:
- **"Eerie Cozy"** pazar boşluğu → Pestis'in Plague Doctor teması tam buna uyuyor. Karanlık ama ritualistik, grotesque ama ritimli. Bu tonu vurgulamalıyız.
- **SHADOW REPLAY** (kendi ghost'unla çalış) → Grudge sistemine eklenebilir: önceki run'undaki kendi "ghost" hareketlerin boss'un saldırı paternini şekillendiriyor. Kapsam dışı ama gelecek versiyon için not.

### 5.3 Sentezde Ortaya Çıkan Yeni Şeyler

**Ollama tek başına yapmadı:**
- Arketip isimleri (Darkrunner, Abolisher modeli) — sadece liste verdi
- Grudge Waves'in "kasıtlı programlama" boyutu — sadece "adapte oluyor" dedi
- 30 saniyelik onboarding tasarımı — sadece mekanik listesi

**Gemini tek başına yapmadı:**
- MMORPG derinliği (8 sınıf DNA), skill düzeyinde bağlantı
- Grudge sistemi için somut tablo
- Kombinasyon tier listesi + broken interactions

**Sentezde ortaya çıkan:**
1. Plague Doctor kombinasyonu = oyunun tematik kimliği
2. Grudge Codex = meta aktivite + lore birleşimi
3. Berserker + Paladin "kasıtlı broken" design felsefesi
4. İlk 30 saniye → Dragon Nest/Elsword feedback + Hades structure + Gemini physicality

---

## BÖLÜM 6 — SONRAKI ADIMLAR (Öncelik Sırası)

1. **OYUN_KONSEPT.md güncelle** — 8 sınıf skill tablosu + 28 kombinasyon arketip isimleri ekle
2. **Grudge Badge sistemi** → Unity MCP ile prototip yapılabilir ilk sistem bu
3. **Sınıf seçim ekranı** → Aseprite'ta 8 sınıf için birer "THIS IS MY BUILD" klip çiz
4. **Plague Doctor kombinasyonu** → Oyunun pazarlama materyali için öne çıkar
5. **Fallen Saint broken interaction** → Test et, kasıtlıysa tutabilirsin, değilse nerf et ama öyle göster ki oyuncu "keşfetti" hissini yaşasın

---

*Bu doküman: Claude Sonnet 4.6 × deepseek-r1:14b V2+V3 çıktıları × Gemini CLI strateji çıktısı sentezi*
*Referanslar: WoW, BDO, KO, ArcheAge, GW1, FFXIV, D2, PoE, Hades, Lost Ark, BnS, Dragon Nest, Elsword, Shadow of Mordor*
