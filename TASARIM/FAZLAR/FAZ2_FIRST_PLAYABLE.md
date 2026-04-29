# FAZ 2 — FIRST PLAYABLE LOOP

*Claude: Sadece bu dosyayı oku. Faz 1 tamamlanmış varsay.*

---

## SCOPE

**Hedef:** 4 class seçilebilir, Act 1 tam (6-7 oda), shop, boss, yırtık harita. İlk oynanabilir döngü.

**Ne VAR (Faz 1'e ek):**
- +3 class: Elementalist, Shadowblade, Ranger
- ClassData + ClassManager + ClassSelectUI
- Mana/Energy/Focus kaynak sistemleri
- Act 1 tam harita (6-7 oda prefabı)
- Shop sistemi (Shards currency)
- Unknown oda tipi
- Sandık sistemi (3 tip)
- Rare skill tier
- Reroll sistemi (1 ücretsiz)
- Penitent Sovereign tam (2 faz)
- +2 mob: SeamCrawler (tam), Twice-Born (elite)
- Shards + Echoes (basit — sadece biriktirme, harcama yok)

**Ne YOK:**
- Secondary class seçimi
- Cross-class pasifler/ultimate
- Spirit Encounter, Curse Gate, Event
- Epic/Legendary tier
- Meta-progression harcama

---

## CLASS'LAR (4 Aktif)

### ⚔️ WARBLADE — Faz 1'den hazır
> Tam skill tablosu `FAZ1_CORE_LOOP.md`'de. Bu fazda değişiklik yok.

---

### 🔥 ELEMENTALİST

**Core Fantasy:** "Her şeyi yakıyorum. Ama önce ritmi buluyorum."
**Kaynak:** Mana (0-100, +8/sn) + Elemental State (Fire veya Frost, max 5 stack)
**[V] Burst:** INFERNO — Mana %100: 7s arena-wide ateş yağmuru

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | **Fireball** ★ | ↑▶ | Core | Orta hasar + ateş DoT 4s, Fire State+1 | 3 ard arda → 3.'de Living Bomb ücretsiz |
| 2 | **Glacial Spike** | ⬡↑ | Core | 6m buz hattı: %40 slow + %180 hasar, Frost State+2 | Fireball DoT aktif hedefe → Freeze 2s + DoT patlama |
| 3 | **Living Bomb** | ⚡↓ | Core | 5s sonra patlama, öldürünce 3 komşuya kopyalanır | Glacial Spike slow alt. → patlama yarıçapı 2× |
| 4 | **Blink** | ▶⚓ | Core | 6m ışınlanma, geçilen düşmanlara hasar, sonraki spell +%20 | Düşmanın içinden → 0.5s stun |
| 5 | **Frozen Orb** | ⬡⚓ | Core | Yavaş küre, yolundakileri 5s chill | Orb üzerinden Blink → Orb patlar, Frozen 2s |
| 6 | **Arcane Blast** | ↑↓ | Core | Her cast +%20 hasar, +%30 mana maliyet. 4. cast Barrage açar | — |
| 7 | **Meteor** | 💥⬡ | Core | 1.5s kanal → büyük AoE knockdown | Frozen/slowed hedef → knockdown 3s + hasar +%50 |
| 8 | **Mirror Image** | ⚓ | Core | 2 kopya 8s, hasar önce kopyaya gelir | Kopyalar ölünce → AoE patlama |
| 9 | **Chain Lightning** | ⚓✦ | Advanced | 5 hedefe sekiyor | Yavaşlamış hedef → 7 seki |
| 10 | **Arcane Surge** | ✦▶ | Advanced | 8s: mana regen +%100, cast süresi -%50 | Blink sonrası → Blink noktasında patlama, sonraki Meteor mana=0 |
| 11 | **Combustion** | ✦▶ | Advanced | 8s: tüm Fire spell instant cast, mana maliyet ×2 | Fire State 5 → mana artışı yok |
| 12 | **Blizzard** | ⬡↑ | Master | 3s kanal → 5m alana 8s slow+tick | Meteor'dan önce → knockdown 4s |

**Build Eksenleri:** Fire Burst / Frost Lock / Arcane Storm

**Resource Bar UI:** Akışkan, dalga hareketli mana bar — sakin dolup boşalan

---

### 🗡️ SHADOWBLADE

**Core Fantasy:** "Görmüyorsun. Zaten geç."
**Kaynak:** Energy (0-100, +15/sn) + Combo Points (0-5)
**[V] Burst:** SHADOW DANCE — Energy %100 + CP 5: 8s her saldırı sonrası stealth

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | **Backstab** ★ | ▶↑ | Core | Arkadan: %200 hasar+3CP. Önden: normal | Shadowstep sonrası → +%50 hasar |
| 2 | **Hemorrhage** | ↑ | Core | Bleed 8s DoT+2CP, öldürünce yakına yayılır | Bleed aktif → Rupture hasar +%100 |
| 3 | **Rupture** | 💥↓ | Core | 5CP finisher: bleed+hasar, CP'ye göre süre uzar | Zaten bleed → birikmiş hasar patlar |
| 4 | **Shadowstep** | ▶⬡ | Core | Hedefe 8m ışınlan, 0.5s stun, Energy-25 | Evasion aktifken → CD sıfırlanır |
| 5 | **Kidney Shot** | ⬡↓ | Core | 5CP: 4s stun, CP'ye göre uzar | — |
| 6 | **Ambush** | 💥▶ | Core | Sadece stealth'ten: %300 hasar+4CP+%20 slow | 3s+ stealth → Cold Blood +%100 |
| 7 | **Fan of Knives** | ⚓⬡ | Core | 360° AoE, tüm aktif debuffları tüm düşmanlara uygular | — |
| 8 | **Evasion** | ⚓↑ | Core | 4s %100 dodge, her dodge=+1CP | Evasion bitince → sonraki saldırı guaranteed crit |
| 9 | **Mirage Blade** | ⚡⚓ | Advanced | 3s: gölge bırakır, dokunan düşman %100 hasar+1CP | Shadowstep sonrası → tüm gölgeler hedefe atılır |
| 10 | **Toxic Eruption** | ↓💥 | Advanced | 5CP: tüm debuffları patlatır, her stack %150 hasar | Hemorrhage aktif → debufflar tüketilmez |
| 11 | **Preparation** | ↑✦ | Advanced | Tüm Rogue CD'leri sıfırla, 90s CD | Evasion aktifken → CD 60s |
| 12 | **Vanish** | ⚓⬡ | Master | Savaşta anlık stealth 3s, 50s CD | Vanish sonrası Ambush → Cold Blood garantili |

**Build Eksenleri:** Assassin / Bleed Lord / Phantom

**Resource Bar UI:** Segmentli hızlı bar + 5 ayrı CP noktası

---

### 🏹 RANGER

**Core Fantasy:** "Sana ulaşamazsın. Her saniye kayıp veriyorsun."
**Kaynak:** Focus (4m+: +10/sn | 2m-: -20/sn) — Focus 75+: +%25 hasar | Focus 100: next skill free cast
**[V] Burst:** RAIN OF ARROWS — 30s sabit CD: 5s tüm arena yağmur

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | **Aimed Shot** ★ | 💥⚡ | Core | 1.5s şarj → büyük hasar+%50 crit | Root/stun altındaki hedefe → instant |
| 2 | **Concussive Arrow** | ⬡▶ | Core | Knockback 4m + root 2s | Disengage sırasında → uzaklık 6m + slow 3s |
| 3 | **Barbed Net Shot** | ⬡▶ | Core | Ağ fırlatır, 2s root + 4s kanama | Disengage sonrası → ağ 4m alana yayılır |
| 4 | **Explosive Trap** | ⬡↑ | Core | Zemine tuzak, 3s sonra patlama+slow 3s | — |
| 5 | **Multi-Shot** | ⚓↑ | Core | Delici ok: tüm düşmanlardan geçer, her birine kanama | 5+ düşman → tüm CD -3s |
| 6 | **Disengage** | ▶⬡ | Core | 6m geri atla, slow alanı bırak | Disengage+Aimed Shot → atlama sırasında instant ateş |
| 7 | **Black Arrow** | ↑⚡ | Core | DoT + öldürünce 8s ruh bırakır | — |
| 8 | **Volley** | ⬡⚓ | Core | 4m alana 3s yağmur, slow+tick | Explosive Trap üzerine → tam kilitlenme |
| 9 | **Rapid Fire** | ⚓↓ | Advanced | 2s kanal: 8 hızlı ok, Focus-30 | Focus 100 → 10 ok, maliyet yok |
| 10 | **Tethering Arrow** | ⬡↑ | Advanced | Hedefe bağ ok: 4s 4m çember içine hapseder | 8m+ uzağa çıkarsa → zincir kopar, %300 kritik |
| 11 | **Flare** | ⬡✦ | Advanced | Stealth açığa çıkar + 6s slow alanı, Focus+20 | — |
| 12 | **Point Blank** | 💥⬡ | Master | ≤2m: ×3 hasar + 5m knockback. Focus 100 gerektirir | Disengage sonrası → CD sıfırlanır |

**Build Eksenleri:** Sniper / Trap Master / Kite Lord

**Resource Bar UI:** Menzile göre dolan ince halka — uzaktan dolu, yakından boşalır

---

## MOB'LAR (9 Mob — Act 1)

> Faz 1'deki 7 mob'a ek olarak:

### Yeni Grunt

| Mob | Mekanik | Sprite |
|-----|---------|--------|
| **SeamCrawler** | Zemin çatlaklarında kayar, pençe saldırısı, ambush | ✅ Anim hazır |

### Yeni Elite (160px)

| Mob | Mekanik | Sprite |
|-----|---------|--------|
| **Twice-Born** | İki bağlı varlık, hasar paylaşımı %50, biri ölünce diğeri berserk | ❌ Üretilecek |

---

## BOSS: PENİTENT SOVEREİGN (Tam — 2 Faz)

### Faz 1 — "Zincirlerin Altında" (HP: 100% → 50%)
> Faz 1'den aynen. Aynı saldırı seti.

### Faz 2 — "Kırılan Zincir" (HP: 50% → 0%)

**Geçiş sahnesi (1.5s):** Sovereign yere çöker, zincirleri koparır, hız +%40.

| Saldırı | Mekanik | Uyarı |
|---------|---------|-------|
| **Fracture Strike** | Üç hızlı vuruş — sol, sağ, orta | Hızlı dash anim |
| **Chain Explosion** | Kırılan zincir parçaları zemine saplanır, 3s sonra patlar | Zincir parçaları görünür |
| **Sovereign's Wrath** | Tüm alana zemin hasar, ortada güvenli bölge | Zemin kızarır |
| **Fracture Charge** | Arena boyunca dash + hasar çizgisi | Başlangıç pozisyonu ışınlanır |

**Ölüm sahnesi:** Zemin çatlar → Faz 2'de (ileride) secondary class seçimi başlayacak. Bu fazda sadece "tebrikler" ekranı.

---

## ODA TİPLERİ (Faz 1'e ek)

| Tip | İkon | İçerik |
|-----|------|--------|
| **Shop** | 🛒 | Shards ile: HP satın al, max HP arttır, skill tier atlatma |
| **Unknown** | ❓ | İkon yok. İçerik: Combat %25, mini-boss %20, gizli shop %15, tuzak %10, max HP %10, minor reward %5 |

### Shop İçeriği

| Ürün | Fiyat (Shards) | Efekt |
|------|---------------|-------|
| Small Potion | 50 | +20 HP |
| Max HP Up | 150 | +10 max HP (kalıcı, run süresince) |
| Skill Reforge | 200 | Mevcut bir skill'i 1 tier atlatır |
| Rift Chest Key | 100 | Sonraki Rift Chest'i açar |

### Sandık Sistemi (3 tip)

| Sandık | İçerik | Frekans |
|--------|--------|---------|
| Common | Shards, küçük augment | Sık |
| Rare | Güçlü item / skill tier atlatma | Seyrek |
| Rift Chest | Build-shaping (nadir pasif, early secondary skill) | Çok nadir |

---

## SİSTEMLER

### Yeni İmplemente Edilecek

| Sistem | Açıklama |
|--------|----------|
| **ClassData + ClassManager** | ScriptableObject: her class'ın skill havuzu, kaynak tipi, burst tanımı |
| **ClassSelectUI** | Run başı: 4 class'tan 1 seç (Warblade başta açık, diğerleri Echo ile) |
| **ManaSystem.cs** | Elementalist: 0-100 mana, +8/sn regen, Elemental State tracking |
| **EnergyComboSystem.cs** | Shadowblade: 0-100 energy + 0-5 CP, finisher mekaniği |
| **FocusSystem.cs** | Ranger: mesafe bazlı resource, 4m+ kazanır, 2m- kaybeder |
| **ShopManager + ShopUI** | Shards harcama, ürün listesi, UI |
| **ShardsManager.cs** | In-run para birimi: düşmandan düşer, run bitince sıfırlanır |
| **EchoesManager.cs** | Meta para: düşmandan düşer, hub'da birikir (henüz harcama yok) |
| **RerollSystem.cs** | Skill draft'ta 1 ücretsiz reroll, ek reroll Shards ile |
| **ChestSystem.cs** | 3 tip sandık: Common, Rare, Rift Chest |
| **Act1MapGenerator.cs** | 6-7 oda prefabı, lineer/dallanma harita üretimi |
| **Rare Skill Tier** | Common'dan 1 tier atlatma: +%30 güç + küçük mekanik ekleme |

---

## ANİMASYON BÜTÇESİ

| İçerik | PixelLab Gen | Durum |
|--------|-------------|-------|
| Elementalist attack redesign (elemental cast) | ~24 gen | ❌ |
| Shadowblade attack redesign (3-combo slash) | ~60 gen | ❌ |
| Ranger attack redesign (arrow shot) | ~20 gen | ❌ |
| Twice-Born (elite) tüm set | ~40 gen | ❌ |
| Penitent Sovereign Faz 2 (4 yeni saldırı + transition) | ~30 gen | ❌ |
| **Toplam** | **~174 gen** | |

---

## ÇIKIŞ KRİTERLERİ

- [ ] Run başında 4 class'tan 1 seçilebilir (class select ekranı)
- [ ] Her class'ın kaynak sistemi çalışır (Rage/Mana/Energy+CP/Focus)
- [ ] Act 1 tam harita: 6-7 oda lineer/dallanma
- [ ] Shop odası: Shards ile alışveriş yapılabilir
- [ ] Unknown odası: rastgele içerik spawn olur
- [ ] Rare tier: skill 1 tier atlatılabilir (Common → Rare)
- [ ] Penitent Sovereign tam (2 faz) — geçiş sahnesi çalışır
- [ ] Ölüm: restart + run recap ekranı
- [ ] Echoes birikir (hub'da görünür, henüz harcanamaz)
- [ ] Twice-Born elite: hasar paylaşımı + berserk çalışır
