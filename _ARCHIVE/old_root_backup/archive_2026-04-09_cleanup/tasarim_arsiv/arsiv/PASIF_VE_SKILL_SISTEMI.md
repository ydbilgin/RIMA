# PASİF + CROSS-CLASS + SKILL SEÇİM SİSTEMİ
*2026-03-27 | Claude sentezi + Ollama araştırması (devam ediyor)*

---

## YAPI — BU DOSYADA NE VAR?

```
A. Her sınıfın pasifleri (4 adet) + upgrade versiyonları
B. Neutral pasifler (14 adet)
C. Cross-class pasifler (28 combo, 1 pasif per combo)
D. Seçilebilir skill sistemi — oda ödülü havuzu, seçim akışı
E. Ultimate + Identity Transform
```

---

## A. SINIF PASİFLERİ + UPGRADE VERSİYONLARI

Her sınıfın 4 pasifi var. Run başında 2'sini seçiyorsun. Oda ödülüyle veya Shop'ta upgrade edebilirsin.

**Upgrade kuralı:** Aynı pasifi iki kez upgrade edemezsin. Upgrade genellikle şunlardan birini yapar:
- Sayısal güçlendirme (+%20-30)
- İkinci tetikleme koşulu ekleme
- Kısıtlamayı kaldırma
- Yeni ama tematik tutarlı efekt ekleme

---

### WARBLADE (Rage: hasar al/ver → dolar)

| # | Pasif | Etki | Upgrade |
|---|-------|------|---------|
| P1 | **Bloodlust** | Kill → 4s %8 lifesteal | Kill → 6s %12 lifesteal + Rage +15 |
| P2 | **Iron Will** | HP %30- → 3s %45 hasar azaltma (15s CD) | CD 8s'e düşer + aktifken Rage +20 |
| P3 | **Juggernaut** | Charge sırasında stun/root immune + hareket ederken gelen hasar -%15 | Hareket ederken gelen hasar -%25 + Charge'dan sonra 2s CC immune |
| P4 | **Wrecking Ball** | CC sonrası ilk saldırı +%100 hasar (8s CD) | CC sonrası 2 saldırı +%100, CD 5s |

*DNA: WoW Arms Warrior × BDO Warrior × KO Berserker*

---

### ELEMENTALIST (Mana + Fire/Frost State)

| # | Pasif | Etki | Upgrade |
|---|-------|------|---------|
| P1 | **Fire Focus** | Fire spell vurunca → tüm CD'ler -2s | -3s + Fire State sayacı sıfırlanmaz |
| P2 | **Burnout Recovery** | Hasar alınca Mana +8 | +12 Mana + kısa Frost State tetiklenir (2s slow aura) |
| P3 | **Shatter** | Slow/frozen hedefe tüm spell'ler +%60 hasar | +%80 hasar + Shatter proc Living Bomb'u anında patlatır |
| P4 | **Temporal Flux** | Sen yavaşlattığın düşmanlar tüm hasar kaynaklarından +%25 alır | +%35 + sen de yavaşlattığın düşman sayısı × %3 hız bonusu |

*DNA: FFXIV Black Mage × WoW Fire Mage × GW1 Elementalist*

---

### ROGUE (Energy + Combo Points 0-5)

| # | Pasif | Etki | Upgrade |
|---|-------|------|---------|
| P1 | **Parry Riposte** | Perfect dodge (0.2s window) → Energy +30 + sonraki saldırı guaranteed crit | Window 0.3s'e çıkar + Combo Point +2 de kazanılır |
| P2 | **Shadow Focus** | Stealth'te hız +%40, ilk saldırı Energy maliyeti -%50 | +%60 hız + stealth süresi +2s |
| P3 | **Venomous Wounds** | Bleed tick → Energy +2/tick | +3/tick + 5 bleed tick birikince ücretsiz Combo Point finisher hakkı |
| P4 | **Murderous Intent** | Düşman HP %35- → tüm Rogue saldırıları +%30 hasar + hız +%20 | HP eşiği %40'a çıkar + Combo Points maliyetsiz hale gelir |

*DNA: WoW Subtlety Rogue × BDO Dark Knight × L2 Dagger*

---

### RANGER (Cooldown-only, kaynak barı yok)

| # | Pasif | Etki | Upgrade |
|---|-------|------|---------|
| P1 | **Expertise** | Tüm CD'ler -%15, skill maliyetleri -%20 | -%20 CD + krit şansı +%8 |
| P2 | **Dead Zone Mastery** | Düşman 2m'ye girince otomatik Disengage tetiklenir (12s CD) | CD 7s + Disengage sırasında 1 otomatik Concussive Arrow atar |
| P3 | **Marked Target** | 5 ok vuruşu birikince: hedef +%30 tüm hasar alır | 4 vuruşta tetiklenir + Marked düşman ölünce yakına 2 stack geçer |
| P4 | **Predator's Focus** | 2s hareketsiz sonra: hasar +%20, krit +%15 | 1.5s sonra aktif + aktifken ilk Charged Shot anında ateşlenir |

*DNA: GW1 Ranger Expertise × WoW BM Hunter × BDO Archer*

---

### BERSERKER (Fury: SADECE hasar alınca dolar)

| # | Pasif | Etki | Upgrade |
|---|-------|------|---------|
| P1 | **Unyielding** | HP %70+ → alınan hasarın %20'si Fury'e dönüşür | %25'e çıkar + HP %70+ iken CD'ler -%10 |
| P2 | **Ruthless** | HP %30- → atk hızı +%15, lifesteal +%5 | +%20 atk hızı + her kill %3 HP geri verir |
| P3 | **War Machine** | Kill → Fury +40 + 0.3s invuln | Fury +50 + 0.5s invuln + sonraki saldırı guaranteed knockdown |
| P4 | **Pain is Power** | Her %10 HP kaybı → +%5 hasar (max %50 at %1 HP) | Her %10 kayıp → +%6 hasar (max %60) + Fury regen hızı +%20 |

*DNA: WoW Fury Warrior × PoE Berserker Ascendancy × D2 Berserk*

---

### PALADIN (Holy Power: ritmik 0-100, 100'de spender açılır)

| # | Pasif | Etki | Upgrade |
|---|-------|------|---------|
| P1 | **Divine Focus** | 5m içindeki düşmana vuruş → tüm CD'ler -%10 (5s) | -%15 + vuruş Holy Power +5 ekstra üretir |
| P2 | **Holy Endurance** | Hasar alınca: max HP'nin %10'u 5s içinde geri gelir | %13'e çıkar + aktif iyileşme sırasında gelen hasar -%10 |
| P3 | **Sanctified Wrath** | Holy Power %80+ → builder skill'ler +%50 Holy Power üretir | +%60 üretim + Holy Power %80+ iken tüm hasar +%10 |
| P4 | **Light's Grace** | Spender skill sonrası → bir sonraki builder skill free cast | Free cast + builder sonraki hasar +%25 |

*DNA: WoW Ret Paladin × FFXIV Paladin combo zinciri × Lost Ark Paladin*

---

### SUMMONER (Charges: zamanla dolar, max 4)

| # | Pasif | Etki | Upgrade |
|---|-------|------|---------|
| P1 | **Army's Strength** | Her canlı minyon → tüm hasar +%5 (max 3 minyon = +%15) | +%7 per minyon (max +%21) + minyonlar %15 daha sağlam |
| P2 | **Blood for Power** | Minyon ölünce → Charge +1 + tüm CD'ler -%30 (5s) | CD -%40 + minyon ölümü Rage +10 de verir |
| P3 | **Undying Hunger** | Minyonlar verdiği hasarın %3'ü Summoner'ı iyileştirir | %5'e çıkar + minyon kill'leri de %2 iyileştirir |
| P4 | **Corpse Lord** | Minyon öldürdüğü düşman otomatik patlayan cesetle başlar | Patlama alanı ×2 + minyon kill sonrası yeni minyon spawn şansı %25 |

*DNA: D2 Necromancer × GW1 Minion Master × WoW Demonology*

---

### HEXER (Hex Stacks: düşman başına 0-10, Hexblast tetikler)

| # | Pasif | Etki | Upgrade |
|---|-------|------|---------|
| P1 | **Debuff Threshold** | Hedef 5+ stack → tüm kaynaktan +%20 hasar alır | 4+ stack'te aktif, +%25 hasar |
| P2 | **Punishing Hex** | Hexer'a saldıran düşman → 2 stack + hasar ricochet | 3 stack + ricochet zıplama (2 hedefe yansır) |
| P3 | **Lingering Curse** | Stack azalma hızı -%50 | -%70 azalma + düşman ölünce stackler yakındaki 1 hedefe geçer |
| P4 | **Soul Rend** | 10 stack hedefe uygulayınca → 8s HP regen blok | 10s blok + hedef 10 stack'te öldürülürse Soul Dust +5 bonus |

*DNA: PoE Chaos builds × WoW Affliction Warlock × GW1 Mesmer*

---

## B. NEUTRAL PASİFLER (14 adet)

Sınıfsız, herhangi bir rotasyona sığar. Her Flux odasında 1 Neutral garantili. Normal oda ödülünde düşük ağırlıkla çıkabilir.

### Mevcut 8 (değişmiyor)

| # | İsim | Etki | Kaynak İlham |
|---|------|------|--------------|
| N-P1 | **Evasion** | %10 dodge, dodge → Rage +5 | Hades Athena boon × GW1 Dodge |
| N-P2 | **Bolstered Resolve** | 3s hasar yok → gelen hasar -%20 | Hades Chaos boon "still" bonus |
| N-P3 | **Predator's Mark** | CC'li hedefe vur → sonraki skill CD -2s | Enter the Gungeon "synergizes with freeze" |
| N-P4 | **Bloodstained Path** | Kill → Rage +5 | Vampire Survivors kill momentum |
| N-P5 | **Momentum** | Dash sonrası 2s → ilk skill +%25 hasar | Hades dash-attack chain |
| N-P6 | **Glass Cannon** | Hasar +%12, max HP -10 | PoE CI keystones, StS Corruption |
| N-P7 | **Iron Skin** | HP %50- → gelen hasar -%15 | Dead Cells survival passive |
| N-P8 | **Hunter's Focus** | Aynı hedefe 3 vuruş → 4. guaranteed crit | Lost Ark "stack-to-crit" |

### Yeni 6

| # | İsim | Etki | Kaynak İlham |
|---|------|------|--------------|
| N-P9 | **Last Rites** | Run başına 1 kez: HP 0'a düşünce → 1 HP kal + 2s invuln + Rage max | Hades Death Defiance × StS Fairy in a Bottle |
| N-P10 | **Fallen Tribute** | Elite veya boss kill → Soul Dust +5 (shop'ta kullanılır) | Hades Darkness currency × D2 gold drop |
| N-P11 | **Volatile Grudge** | Grudge Badge taşıyan düşmanlar sana karşı her girişimde hasar -%8 (max -%24, 3 Grudge) | Oyunun Grudge sistemiyle senkron |
| N-P12 | **Adaptive Armor** | Bir savaşta aynı hasar tipinden 3+ kez vurulunca: o tipe -%15 direnç | Enter the Gungeon armor sistem × Grudge reverse |
| N-P13 | **Sharpened Instincts** | Oda başında 5s boyunca: tüm hasarın +%25 artış ("ambush" penceresi) | Inscryption first-strike × StS Strike synergy |
| N-P14 | **Chain Kill** | Kill sonrası 1s: bir sonraki kill'de patlama (2m AoE, önceki hasar × %30) | Enter the Gungeon casey jones × Hades Ares boon |

---

## C. CROSS-CLASS PASİFLER (28 COMBO)

Her dual-class kombinasyon için özel bir pasif. Run başında sınıf seçince **otomatik aktif** olur — slot kullanmaz, seçim gerektirmez.

**Güç seviyesi:** Sınıf pasiflerinden biraz daha zayıf (slot almıyor çünkü). Ama anlamlı — fark edilir.

**Tasarım prensibi:** Her cross-class pasifi "neden bu ikisini seçeyim?" sorusuna mekaniğin kendisiyle cevap veriyor.

---

### WARBLADE kombinasyonları (7)

| Combo | Arketip | Cross-Class Pasif | Tetikleyici | Etki | İlham |
|-------|---------|-------------------|-------------|------|-------|
| W + Elementalist | **Runeguard** | *Arcane Armor* | Rage %100'e ulaşınca | 6s Fire/Frost aura: yakın düşmanlara hasar + sen hasar -%20 | WoW Spellbreaker × FFXIV Paladin magic stance |
| W + Rogue | **Iron Shadow** | *Exposed Weakness* | CC uyguladıktan sonra | Stun/knock hedefe Rogue finisher hasarı +%50 (3s window) | WoW CC-combo PvP zinciri × BDO awakening combo |
| W + Ranger | **Vanguard** | *Advance Fire* | Charge kullanınca | Charge hedefine yakın 3 saniye Ranger ok'ları otomatik atılır (pasif turret) | Hades duo boon "Zeus+Artemis" × FFXIV LB chain |
| W + Berserker | **Juggernaut** | *Fury of Iron* | Her ikisinin kaynağı (Rage + Fury) aynı anda %50+ | Tüm hasar +%20, CC immune | WoW Protection Warrior × PoE Juggernaut Ascendancy |
| W + Paladin | **Sentinel** | *Blessed Rampage* | Battle Cry (Warblade taunt) aktifken | Taunt süresi boyunca Holy Power +5/vuruş, gelen hasar -%15 ekstra | WoW Prot Warrior + Prot Paladin tank meta |
| W + Summoner | **Death Commander** | *Vanguard Rush* | Charge sonrası | Charge hedefi etrafına 2 minyon anında spawn (3s, sonra kaybolur) | Hades "summon on dash" boon × D2 Necro+Barb |
| W + Hexer | **Doombringer** | *Cursed Steel* | Ground Stomp/War Stomp ile stun yapınca | Stun hedefine otomatik 3 Hex Stack uygulanır | WoW DK Chains of Ice × PoE Hexblast melee |

---

### ELEMENTALIST kombinasyonları (6)

| Combo | Arketip | Cross-Class Pasif | Tetikleyici | Etki | İlham |
|-------|---------|-------------------|-------------|------|-------|
| E + Rogue | **Arcane Shadow** | *Phantom Flame* | Blink (Elementalist) veya Shadowstep (Rogue) sonrası | Teleport varış noktasına 2s ateş alanı bırakılır | Hades Demeter "chill on dash" × WoW Fire+Shadow |
| E + Ranger | **Storm Archer** | *Conductor* | Concussive Arrow (root) + herhangi bir Elementalist spell | Root süresi boyunca tüm spell'ler o hedefe +%40 hasar | FFXIV Bard buffs + BLM nukes × GW1 ele+ranger |
| E + Berserker | **Chaos Incarnate** | *Mana Burn* | Berserker HP %50- düşünce | Mana yerine HP harcanabilir (1 Mana = 2 HP), 10s süre | WoW Mana Burn PvP × PoE Blood Magic keystone |
| E + Paladin | **Divine Flamecaller** | *Holy Combustion* | Holy Power %100 + Fire State aktif | Spender skill ateş hasarı kazanır + AoE +50% genişler | FFXIV Paladin+BLM hybrid fantasy |
| E + Summoner | **Plague Caster** | *Toxic Catalyst* | Minyon kill → | Minyon her kill'de 3 Fireball stack tetikler (Mana maliyetsiz) | D2 Necro+Sorc × GW1 Minion+Fire |
| E + Hexer | **Voidcaster** | *Resonant Curse* | Fire DoT aktif hedef Hex Stack alınca | Her Fire tick 1 Hex Stack ekler (iki DoT senkron birikir) | WoW Affliction+Fire × PoE DoT stacking builds |

---

### ROGUE kombinasyonları (5)

| Combo | Arketip | Cross-Class Pasif | Tetikleyici | Etki | İlham |
|-------|---------|-------------------|-------------|------|-------|
| R + Ranger | **Phantom Hunter** | *Ghost Protocol* | Shadowstep + hemen Backward Dash | Kombine kullanımda: 3s stealth girişi (hareket edebilirsin) | Hades Duo boon "Zeus+Poseidon" instant combo |
| R + Berserker | **Bloodfang** | *Frenzy Bleed* | Bleed altındaki düşmana Berserker skill | Bleed tick hızı 2× olur + her tick Fury +3 | D2 Vampire build × WoW Arms+Fury hybrid |
| R + Paladin | **Shadow Saint** | *Sacred Riposte* | Parry Riposte başarılı olunca | Holy Power +20 + Energy +30 aynı anda (iki kaynağı birden doldurur) | Bu oyunun kendi özgün sinerjisi — Parry = ikili kaynak |
| R + Summoner | **Deathmask** | *Shadow Puppets* | Stealth'te (Shadow Focus aktif) | Stealth sırasında minyonlar oyuncunun son konumunda bekler ve decoy görevi görür | Hades "shade" mechanic × D2 Necro illusion |
| R + Hexer | **Venomancer** | *Bleeding Curse* | Bleed + Hex Stack aynı hedefte | Bleed tick her 2s'de 1 Hex Stack üretir (DoT birbirini besler) | WoW Afflock+Rogue × PoE Poison+Bleed chaos |

---

### RANGER kombinasyonları (4)

| Combo | Arketip | Cross-Class Pasif | Tetikleyici | Etki | İlham |
|-------|---------|-------------------|-------------|------|-------|
| Rng + Berserker | **Wild Hunter** | *Pain Shot* | Berserker HP %50- | Tüm ok hasarı +%30, Berserker Fury regen 2× (mesafeden vuruş = aldığın hasarla aynı his) | GW1 Ranger+Warrior × RoR2 "on-hit" items |
| Rng + Paladin | **Sacred Archer** | *Judgment Arrow* | Holy Power %80+ | Menzilli saldırılar Holy damage kazanır + Concussive Arrow root süresi +1s | FFXIV Paladin+Archer hybrid fantasy |
| Rng + Summoner | **Beast Commander** | *Pack Hunt* | Düşman Marked Target (5 ok) aldıktan sonra | Minyonlar otomatik o hedefe odaklanır + marklandı hasar bonusu minyonlara da uygulanır | WoW BM Hunter+Pet × GW1 Ranger+MM |
| Rng + Hexer | **Plague Arrow** | *Toxic Volley* | Explosive Trap patladıktan sonra | Trap patlaması 3 Hex Stack uyguluyor (menzilli hex dağıtımı) | PoE Toxic Rain × D2 Poison Javelin |

---

### BERSERKER kombinasyonları (3)

| Combo | Arketip | Cross-Class Pasif | Tetikleyici | Etki | İlham |
|-------|---------|-------------------|-------------|------|-------|
| B + Paladin | **Fallen Saint** ⭐ | *Martyr's Loop* | HP %30- (Berserker güç zonu) + Holy Endurance aktif | İyileşme tam ölümü engelliyor ama HP %20'nin altında kalmaya devam ediyor — Berserker bonusları kalıcı aktif | Kasıtlı broken: WoW DK Anti-Magic + Prot Paladin × PoE Eldrich Battery |
| B + Summoner | **Blood Shepherd** | *Death Fuels Fury* | Minyon ölümü | Her minyon ölümü Fury +25 (minyonlar pil gibi) | Enter the Gungeon "dead friends" item × D2 CE loop |
| B + Hexer | **Plague Berserker** | *Toxic Rage* | HP %50- + hedef 5+ Hex Stack | Hexblast hasarı +%40, Hexblast Fury +30 verir (düşük HP'de büyü gücü artar) | WoW Demon Hunter × PoE Chaos Berserker |

---

### PALADIN kombinasyonları (2)

| Combo | Arketip | Cross-Class Pasif | Tetikleyici | Etki | İlham |
|-------|---------|-------------------|-------------|------|-------|
| P + Summoner | **Holy Shepherd** | *Blessed Horde* | Spender skill sonrası | Sonraki Charge minyon spawn → minyonlar 3s Holy damage buffu alır | WoW Paladin aura × FFXIV Paladin party buff |
| P + Hexer | **Inquisitor** ⭐ | *Judgment Curse* | Holy Power %100'de spender atılınca | Spender otomatik 5 Hex Stack atar + "Condemnation" debuff: 6s hasar +%25 alır | WoW Inquisitor lore × ArcheAge Hierophant |

---

### SUMMONER + HEXER (1)

| Combo | Arketip | Cross-Class Pasif | Tetikleyici | Etki | İlham |
|-------|---------|-------------------|-------------|------|-------|
| S + Hexer | **Plague Doctor** ⭐⭐ | *Epidemic* | Minyon öldürdüğü düşman | Ölüm anında tüm Hex Stack'leri çevredeki 2 düşmana kopyalar (salgın yayılımı) | D2 CE+Poison Nova × GW1 Minion+Necro DoT |

---

## D. SEÇİLEBİLİR SKILL SİSTEMİ — ODA ÖDÜLÜ MANTIGI

### Başlangıç Loadout'u

```
Run başında 2 sınıf seçilir (örn. Warblade + Elementalist).

Başlangıç: Oyun otomatik olarak her sınıftan 1 "primer" skill verir:
  → Warblade: Ground Stomp (temel CC, herkes için kullanışlı)
  → Elementalist: Fireball (temel hasar, herkes için kullanışlı)

İlk Oda sonrası:
  → 3 seçenek: 1 Warblade skilli, 1 Elementalist skilli, 1 Neutral
```

**Neden otomatik primer?** StS'in "Strike + Defend" başlangıcı gibi — oyuncuyu ilk odada sıfır skill ile bırakmak can sıkıcı. Ama primer skill her sınıfın en "bağlamsal" skilli olmalı.

### Primer Skill Listesi (her sınıfın 1 başlangıç skili)

| Sınıf | Primer Skill | Neden? |
|-------|-------------|--------|
| Warblade | Ground Stomp | Temel CC, herhangi bir build'e girer |
| Elementalist | Fireball | Temel hasar, Fire State sistemini öğretir |
| Rogue | Hemorrhage | Bleed + Combo Point — iki mekaniği birden öğretir |
| Ranger | Concussive Arrow | CC + kite — Ranger'ın kimliğini hemen anlatır |
| Berserker | Bloodlust Strike | HP'ye göre hasar — Berserker fantezisini hemen yaşatır |
| Paladin | Crusader Strike | Holy Power üretici — ritim mekanikini başlatır |
| Summoner | Raise Dead | İlk minyon — Summoner'ın temelini kurar |
| Hexer | Wither | İlk Hex Stack uygulaması — stack mekaniğini öğretir |

### Oda Ödülü Seçenek Kompozisyonu

```
Her oda sonu → 3 seçenek:

  Seçenek 1: 1. sınıftan skill/pasif        [ağırlık %38]
  Seçenek 2: 2. sınıftan skill/pasif        [ağırlık %38]
  Seçenek 3: Neutral havuzdan               [ağırlık %24]

+ Nadir (%12 şans): Seçeneklerden biri UPGRADE olur
  (elindeki bir skill veya pasif için upgrade seçeneği)
```

### Aşama Bazlı Havuz Filtresi

| Aşama | Odalar | Skill Havuzu | Pasif Havuzu |
|-------|--------|-------------|-------------|
| Erken | 1–4 | Utility + mobility ağırlıklı (Charge, Blink, Shadowstep) | Survival pasifleri ağırlıklı |
| Orta | 5–11 | Tüm havuz açık, proc zincirleri ağırlıklı | Proc + sinerji pasifleri |
| Geç | 12+ | Yüksek hasar + combo enabler ağırlıklı | Build-defining pasifleri |

### Max Slot Dolduğunda Ne Olur?

```
Aktif skill slotları (max 6) dolunca:
  → Seçenek 1 ve 2 otomatik "UPGRADE" seçeneğine döner
  → Seçenek 3 hâlâ yeni skill olabilir (ama almak için birini bırakmak gerekir)

Pasif slotları (max 2) dolunca:
  → Pasif seçenekleri görünmez (sadece aktif skill'ler ve upgrade'ler)
  → Bonus slot: meta-progression unlock ile max 3'e çıkabilir

SKILL DROP seçeneği:
  → Elindeki bir skilli kurban edip yeni skili al
  → FAZ 3+'da: kurban edilen skill "Shard"a dönüşür (küçük augment kazanılır)
  → Şimdilik (FAZ 1-2): kurban = sil
```

### Ultimate Acquisition

```
Ultimate seçimi RUN BAŞINDA yapılır:
  → Her sınıfın 2 ultimate'i gösterilir
  → Oyuncu her sınıftan 1 ultimate seçer (2 ultimate, 1 slot)
  → Başlangıçta hangisi aktif olduğunu seçer

Stance modu unlock (meta-progression, Run 3 sonrası):
  → [TAB] ile kit değiştirilince o sınıfın ultimate'i aktif olur
  → Farklı stance = farklı ultimate
```

---

## E. IDENTITY TRANSFORM — 8 SINIF DÖNÜŞÜM KITI

Meta-progression unlock (~Run 7). Rage %100 + [V] hold 1.5s → 10s dönüşüm.

| Sınıf | Dönüşüm Adı | 4 Skill (kısa) | Pasif Bonus |
|-------|-------------|-----------------|-------------|
| Warblade | **Avatar of War** | AoE knockup + unbreakable charge + zemin parçalama + execute | CC immune, tüm hasar +%40 |
| Elementalist | **Primordial** | Instant meteor + frozen nova + fire tornado + time slow | Sonsuz mana, spell cast anında |
| Rogue | **Death's Shadow** | Teleport chain + instant kill (HP%30-) + area stealth + mark all | Tüm saldırılar crit, görünmez |
| Ranger | **Apex Predator** | Guaranteed crit volley + trap storm + mark all enemies + piercing shot | Backward Dash CD 0, tüm ok piercing |
| Berserker | **Chaos Incarnate** | Spinning slam + berserker jump + shockwave + fury nova | Defense ignore, HP drain = hasar |
| Paladin | **Divine Champion** | Holy nova (5m) + invuln bubble + judgment column + sacred ground | %50 invuln sürekli, ally minyonlar iyileşir |
| Summoner | **Death Lord** | Army command + corpse explosion chain + plague nova + raise elite | Her kill yeni minyon, minyonlar ölümsüz |
| Hexer | **Curse Embodied** | Hex flood (all enemies) + stack burst + wither aura + soul drain | Sen zaten curse'sin — yakın düşmanlar otomatik stack alır |

---

---

## F. OLLAMA SENTEZ NOTLARI — Araştırma Bulguları

*deepseek-r1:14b | 10 bölüm | 2026-03-27 | Tamamlandı*

### Cross-Class Tasarım Prensipleri (Ollama doğrulaması)

Ollama araştırmasının temel teyitleri ve eklemeleri:

1. **Trigger conditions: common but impactful** — Tetikleyici çok nadir olmamalı, oyuncu her oda sonu görebilmeli ama anlamlı hissetmeli. Bizim %38/%38 kompozisyonu bunu karşılıyor.

2. **Avoid stat inflation** — Rakamsal artıştan ziyade conditional bonus ve yeni etkileşimler. PASIF_VE_SKILL_SISTEMI.md'deki tasarım bu prensibe uygun.

3. **Cross-class passives should feel integral to both classes** — Örn. Shadow Saint (*Sacred Riposte*): Parry = Energy + Holy Power. İki sınıfı birden besliyor, organic.

### Fallen Saint Tasarım Notu (Ollama özel vurgusu)

**Fallen Saint (Berserker + Paladin)** — Ollama bunu "kasıtlı broken loop" olarak özellikle işaretledi:

> "Creates a feedback loop where the Berserker thrives on low HP while the Paladin barely keeps them alive. Broken Build Note: Combines 'War Machine' (Berserker) with 'Holy Endurance' (Paladin) for permanent healing and Fury generation, enabling endless loops."

Bu loop'u kasıtlı bırakmak doğru karar — oyuncunun "bu build broken" deyip gülmesi istenen an.

### Plague Doctor Endgame Identity (Ollama teyidi)

**Plague Doctor (Summoner + Hexer)** — Ollama: "The endgame build everyone wants to try, with a strong emphasis on minion management and hex stacking creating an unstoppable feedback loop."

Bu combo ⭐⭐ işaretli kalmalı ve ilerleyen bölümlerde tutorial olarak öğretilmemeli — oyuncunun kendi keşfetmesi gerekiyor.

### Neutral Pasif Tasarım Gözlemi

Ollama Bölüm 07'de önerilen 6 "neutral" pasifin tümü belirli cross-class kombinasyon gerektiriyordu (Summoner+Hexer, Rogue+Ranger, vb.) — bunlar gerçek neutral değil, cross-class özel augment. Bu tasarım hatası: neutral pasifler herhangi rotasyona girmeli. Mevcut N-P1/N-P14 listesi Ollama'nın önerisinden daha sağlam.

---

*Ollama araştırması: 10/10 bölüm tamamlandı. Sentez: 2026-03-27.*
