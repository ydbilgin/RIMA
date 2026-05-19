# Circuit Breaker — VISION DOCUMENT

**Tarih:** 2026-05-18
**Statü:** LIVE Vision Lock — long-term truth document
**Vade:** 12+ ay (Faz 4 dahil)
**Yazar:** Yasin Derya Bilgin (laurethgame)
**AI orchestrator:** Claude Opus 4.7 + Codex GPT-5.5 verdict

---

## 🔖 FUTURE-SELF PICKUP NOTU

Bu dosyayı sonradan açtığında (1 ay sonra, 6 ay sonra, RIMA bittikten sonra) hızlı geri dönmek için:

1. **Bu CB projesidir** — `F:\LaurethStudio\02_GAMES\CircuitBreaker\`
2. **RIMA'dan önce mi sonra mı yapılacak:** Karar açık — RIMA Faz 1 ship sonrası VEYA RIMA içinde POC pivot olursa direkt CB
3. **Bu doküman = VİZYON BELGESİ.** Burada *long-term identity* var. 16 hafta MVP için → `01_MVP_PLAN.md`. Faz sonrası planlama için → `07_ROADMAP.md`
4. **Sub-genre lock:** **REAL-TIME GENERATIVE ACTION ROGUELIKE** — bu cümle pivot edilemez, başka her şey iterate edilebilir
5. **Şu an yapacak iş — eğer bu dosyayı bugün okuyorsan:**
   - MVP_PLAN.md aç, Week 1-16 listesinin neresindeyim?
   - Eğer henüz başlamadıysa: Week -3 (CB design doc final) → Week -2/-1 (RIMA POC) → Week 1 (CB Unity setup)
   - Eğer ortadaysa: ilgili sprint'in deliverable listesini kontrol et
6. **Klasör yapısı için:** `README.md` aç
7. **Codex/Opus orchestrator config:** RIMA'da kuruldu (`cx_dispatch.py` + UnityMCP), CB Unity setup'a port edilecek
8. **Sezon 2 hedefi:** Steam Next Fest demo (eğer 2026 H2'de katılma şansı varsa)
9. **Asıl niyet:** Bu okul ödevi RIMA olarak teslim edildi — CB *scope iteration* olarak sunulacak (proje terki değil, refinement)

---

## 1. TEK CÜMLE VİZYON

> **Circuit Breaker — Real-Time Generative Action Roguelike.**
> Düşmanları öldür, ölüleri arenanı boyasın. Element zeminlerini kendi skill'lerinle birleştir, swap ile silah değiştir, doğru anda tetikle: bir tıkla zincir reaksiyon ekranı temizlesin.

---

## 2. SUB-GENRE LOCK — Tartışılmaz

**Tür adı (LOCK):** **REAL-TIME GENERATIVE ACTION ROGUELIKE**

Bu cümle final. Pivot edilemez. Aşağıdaki tüm kararlar bu cümlenin alt-anlamlarıdır:

| Kelime | Anlamı |
|---|---|
| **Real-Time** | Turn-based DEĞİL. Hades/Risk of Rain tempo. |
| **Generative** | Reactive DEĞİL. Oyuncu **arena state'i kendisi kurar**, sonra tetikler. |
| **Action** | Auto-combat DEĞİL. Manuel aim, dash, skill timing. |
| **Roguelike** | Run-based, ölüm = restart, meta progression run-arası kalır (teknik olarak "Roguelite" daha doğru ama oyuncu beklenti diliyle "Roguelike" tercih edildi). |

### 2.1 Pazardaki yer

CB **rare combination** (boş niş değil, ama eşi az):

| Combat tipi | Örnek | CB ile ilişki |
|---|---|---|
| Reactive Action | Hades, Dead Cells | Dodge/aim hissi paylaşılır, USP değil |
| Passive Horde | Vampire Survivors, Brotato, Soulstone | Volume paylaşılır, manuel olunmadığı için DEĞİL |
| Reactive ARPG | Diablo, PoE, Last Epoch, Hero Siege | Mass-clear + class paylaşılır, spatial setup farkı |
| Turn-Based Generative | Into the Breach | Generative philosophy paylaşılır, real-time farkı |
| Real-Time Physics Generative | Noita | Element reaction paylaşılır, controlled + discrete farkı |
| **REAL-TIME GENERATIVE ACTION ROGUELIKE** | **CB** | **— Hiç tam emsali yok** |

### 2.2 Public-facing labels

- **Steam capsule başlık (kısa):** "Real-Time Generative Action Roguelike"
- **Steam ana açıklama (descriptor):** "Battlefield Alchemy Roguelike"
- **Trailer card subtitle:** "Paint the floor. Trigger the chain. Erase the room."
- **Internal design-pattern adı:** Environmental Cascade Combat

### 2.3 Capsule cümleleri

- **Player-facing:** "Düşman ölüleri arenanı boyar. Element zeminlerini birleştir, swap, tetikle — kontrollü bir Noita, ARPG yoğunluğunda."
- **Streamer cümlesi:** "Hades hands, ARPG mobs, Noita-style reactions — on a controllable battlefield grid."
- **Press kit:** "CB sits between Noita's emergent elemental chaos and Hades-style readable action, but moves the core decision onto a controllable top-down battlefield grid."

---

## 3. DESIGN PILLARS — 5 ayak

Bu 5 pillar üst-pivotlanamaz. Tüm sistem kararları bunlara hizmet eder.

1. **Pillar 1 — Real-Time Generative:** Combat anlık ama oyuncu state üretkenliği var. Oyuncu *önce kurar, sonra tetikler*.
2. **Pillar 2 — Manuel Skill:** Aim, dash, timing, swap. Auto-attack YOK. Manuel skill ceiling oyuncu zaferinin gerçek sebebi.
3. **Pillar 3 — Spatial Setup-Payoff:** Arena = combo system. Floor okuma cevvalliği skill ceiling kaynağı.
4. **Pillar 4 — Mass-Clear Spectacle:** 30-50 düşman peak. Cascade payoff = screen-clear satisfaction. TikTok klip pattern.
5. **Pillar 5 — Build Authorship:** Class kit + universal trigger + modifier sentezi. *Build* = "şu an arena'da ne yapabilirim?"

---

## 4. PLAYER FANTASY

Oyuncu hangi rolde oynuyor?

**"Sen bir element büyücüsüsün, ama büyü sayfan ekrandaki yer. Sayfayı kendin yazıyorsun, sonra ateşliyorsun."**

3 ana duygu beklentisi:
1. **Kurma zevki:** "Şuraya su, oraya yağ, ortaya statik — fark edilmedim henüz"
2. **Tetik anı:** "ŞIMDI" — Q'ya basıp tetikle
3. **Cascade payoff:** Ekran beyazlanır, slow-mo finish, 30 düşman ölü, ses bass-drop

---

## 5. CORE LOOP

### 5.1 Saniyelik loop (combat döngüsü)

```
1. Düşmanlar yaklaşır (1-3 sn)
2. Sen drop tile + swap + aim hazırlık (1-2 sn)
3. Tetik + cascade (0.5-2 sn)
4. Kalan düşmanları manuel mow (3-8 sn)
5. Combo charge dolar, kalanı al
6. Sonraki wave'e dönüş (3-5 sn nefes)
```

= **8-20 saniyelik mini-loop**. Her oda 3-5× bu döngü.

### 5.2 Oda loop

```
A. SETUP faz (20 sn, 8-12 düşman) → drop pratiği
B. SURGE faz (40 sn, 30-50 düşman) → manuel kes + state çoğalt
C. CASCADE faz (8 sn) → mega tetik, screen clear
D. AFTERMATH (12 sn) → loot, modifier seçim
```

### 5.3 Run loop

```
1. Class seç (3 MVP class)
2. Act 1 başla, Floor 1
3. 6 floor sırayla (her biri ~3 dk + 1 fork + 1 shop)
4. Act boss (4-5 dk)
5. Run sonu: Cinder + Spark currency, modifier collection, achievement
6. Hub → Cinder harca → unlock yeni skill/class
7. Tekrar
```

= **20-25 dk per run**. 100 run hedefi = 35-40 saat oynanış (Hades referans muadili).

### 5.4 Meta loop (run-arası)

- Cinder ile **Class unlock** (3 → 5 → 8 → 12+ Faz 4)
- Cinder ile **Skill variant unlock** (RoR2 challenge muadili) — Faz 2
- **Voltage** zorluk yükselt → daha çok Echo currency
- **Lore parça** topla → secret content unlock — Faz 3+

---

## 6. COMBAT GRAMMAR

### 6.1 Input scheme (LOCKED)

| Input | Aksiyon | Aim |
|---|---|---|
| **WASD** | Movement | — |
| **Mouse** | Aim direction | — |
| **LMB** | Primary attack (class-bound, basit damage) | Precise + nearest-snap |
| **RMB** | Current trigger element **DROP** (yere zemin bırakır) | Kaba pozisyon, footprint preview |
| **Q / E** | Trigger element swap (5 element cycle) | — |
| **LMB during trigger-mode** | Trigger silahını ateşle (tile reaction tetikler) | Tile/yön |
| **Space veya Shift** | Dash (sınıf mobility) | WASD yön |
| **1 / 2 / 3** | Sınıf skill (3 active skill) | WASD yön + WASD pos |
| **R** | Form Ultimate (element transform 3-7 sn) | — |
| **Tab** | HUD/modifier ekranı | — |

### 6.2 Trigger silahı 2-mod yapısı (CORE USP)

Her trigger silahı **iki moda sahip**:

| Trigger | Trigger mode (LMB, anlık) | Drop mode (RMB, zemin) |
|---|---|---|
| ⚡ Elektrik | Bolt, tile reaction tetikler | Statik field 2x2 (5 sn) |
| 🔥 Ateş | Bolt, yangın tetikler | Yağ patch 3x3 (kaygan + yanar) |
| 💧 Su | Atış, söndürür/seyreltir | Su gölü 3x3 (8 sn) |
| ❄️ Buz | Atış, freeze | Buz yüzey 4x4 (kaygan + freeze) |
| 🌪️ Vakum | Anlık çekiş, mermi yönlendir | Toz bulutu 4x4 (görüş -, slow) |

**Self-combo loop:** Drop tile → swap silah → trigger → cascade. Düşman olmasa bile combo kurabilirsin.

### 6.3 Element grammar

5 element × 7 tile state × 3 hibrit (MVP) × 3 status hibrit = **~70 reaksiyon**.

Reaksiyon hierarchy:
- **Element + Trigger** → Damage (su+elektrik=şok)
- **Element + Element** → Hibrit tile (su+yağ=emülsiyon)
- **Status + Status** → Hibrit status (wet+shocked=conductive)
- **Hibrit + Trigger** → Mega cascade (volatile+ateş=kitle patlama)

---

## 7. ENVIRONMENTAL CASCADE COMBAT — Pattern Rules

Bu CB'nin **internal design-pattern adı**. Codex bu pattern'i adlandırdı.

### 7.1 4 Pattern kuralı

1. **Tile state visible olmalı GÜÇLÜ olmadan ÖNCE.** Oyuncu reactionı tetiklemeden 0.3+ sn görmeli.
2. **Player-authored terrain WIN'i belirlesin.** En iyi build floor okumadan kazanamamalı.
3. **Enemy-death terrain fırsat eklemeli, AMAÇ değil.** Oyuncu kendi setup'ını kurabilmeli (boss arena boş bile olsa).
4. **Trigger MANUEL, READABLE, SWAPPABLE.** Otomatik tetik YOK. Aim'siz tetik OK ama swap manuel.

### 7.2 Cascade chain kuralları

- Maksimum chain depth = **5 reaksiyon** (sonsuz zincir = oyunu kırar)
- ProcLimiter: aynı reactionId per tile **cooldown** + **max 64 proc/frame**
- Hibrit tile spawn = **0.3 sn arm delay** (oyuncu görsel feedback)
- Chain bittiğinde **slow-mo 0.3 sn** (Hades signature finish)

### 7.3 USP guardrails (kırılırsa USP öldü)

- VFX terrain state'i gizlemesin
- Auto-attack build optimal OLMASIN (manuel skill ceiling korunsun)
- En iyi cascade ezberlenebilir olsun (random ve unrepeatable olmasın)
- Best build floor okumadan kazanamasın

---

## 8. ELEMENT TAXONOMY — 5 + 6 element vision

### 8.1 MVP 5 element

| # | Element | Renk | Drop tile | Status verb |
|---|---|---|---|---|
| 1 | ⚡ Elektrik | Yellow #FFFF00 | Statik field | Shocked |
| 2 | 🔥 Ateş | Orange-red #FF4500 | Yağ patch | Burning |
| 3 | 💧 Su | Cyan #00BFFF | Su gölü | Wet |
| 4 | ❄️ Buz | Ice white #B0E0E6 | Buz yüzey | Frozen |
| 5 | 🌪️ Vakum | White-grey | Toz bulutu | Slowed |

### 8.2 Faz 2-3 ek element vision

| # | Element | Drop tile | Status verb | Faz |
|---|---|---|---|---|
| 6 | 💚 Asit | Asit göl | Corroded | Faz 2 |
| 7 | 🟣 Manyetik | Manyetik field | Magnetized | Faz 2 |
| 8 | 🟢 Zehir | Zehir web | Poisoned | Faz 3 |
| 9 | ☁️ Gaz | Patlayıcı gaz | Volatile | Faz 3 |
| 10 | 🔴 Lav | Sürekli yanma | Molten | Faz 3 |
| 11 | ⚫ Karanlık | Karanlık zone | Blinded | Faz 3 |

= Faz 3 sonu **11 element**, Faz 4 atlas content drip ile devamı.

---

## 9. TILE-STATE TAXONOMY

### 9.1 MVP 7 base tile state

1. Normal (default floor)
2. Water (su gölü)
3. Fire (yangın)
4. Oil (yağ patch)
5. Static (statik field)
6. Ice (buz yüzey)
7. Dust (toz bulutu)

### 9.2 Tile lifecycle

| Property | Değer |
|---|---|
| Default lifetime | 8 sn (drop) |
| Kill-drop lifetime | 12 sn (düşman ölüsü) |
| Hibrit spawn delay | 0.3 sn |
| Hibrit lifetime | 5 sn (kısa, balance) |
| Spread speed (ateş, asit) | 2 sn/tile |
| Cascade chain prop | 0.1 sn/tile |

### 9.3 Tile state machine (mimari)

- Event-driven dirty cells (per-frame scan YOK)
- 32x24 = 768 tile capacity
- TileRuntimeState struct: baseState, hybridState, ownerId, expireAt, charges, tags
- Transition data table: A+B adjacency → hybrid, triggerTag → reaction
- Runtime overlay (debug): state color, expire timer, last reaction

---

## 10. HYBRID TAXONOMY

### 10.1 MVP 3 hibrit (LOCK)

| Zemin A + B | Hibrit | Reaksiyon |
|---|---|---|
| Su + Buz | **Slush** | Elektrik = 2x AOE (geniş) |
| Su + Yağ | **Emülsiyon** | Ateş = yavaş yanma (10 sn) |
| Yağ + Statik | **Volatile** | Herhangi tetik = patlama |

### 10.2 Faz 2-3 ek hibrit vision

| Zemin A + B | Hibrit | Faz |
|---|---|---|
| Ateş + Toz | Sıcak duman | Faz 2 |
| Asit + Su | Seyreltik asit | Faz 2 |
| Manyetik + Statik | EMP field | Faz 2 |
| Buz + Yağ | Donmuş kaygan | Faz 3 |
| Lav + Su | Buharlaşma → kaya | Faz 3 |
| Zehir + Gaz | Toksik bulut | Faz 3 |

= Faz 3 sonu **~12 hibrit**, Faz 4 atlas content ile ek.

---

## 11. STATUS HYBRID TAXONOMY

### 11.1 MVP 3 status hibrit (LOCK)

| Status A + B | Hibrit | Etki |
|---|---|---|
| Wet + Shocked | **Conductive** | Yakın düşmana zincir |
| Burning + Wet | **Steaming** | Self-damage + boss aim'i bozar |
| Oiled + Burning | **Combusting** | 3 sn sonra patlar (zincir) |

### 11.2 Faz 2 ek status hibrit

- Frozen + Physical hit → **Shatter** (anında ölüm)
- Magnetized + Anything → mermi çekiş
- Corroded + Burning → armor melt + dmg amp

---

## 12. TRIGGER WEAPON PHILOSOPHY

**Universal 5-trigger + class affinity.** Class-bound DEĞİL.

### 12.1 Neden universal?

- Class-bound = element grammar'i öldürür
- Aquamancer sadece su tetikleyebilir → wand-style game olur
- Universal = grammar derinliği + class identity korunabilir (affinity ile)

### 12.2 Class affinity sistem

Her class:
- **2 favored trigger** (utility/reliability bonus, raw DPS değil)
- **1 awkward trigger** (penalty değil, sadece bonus alamaz)
- **5 trigger her zaman erişilebilir**

| Class | Favored 1 | Favored 2 | Awkward |
|---|---|---|---|
| Aquamancer | Su | Elektrik | Ateş |
| Pyrotechnist | Ateş | Vakum | Su |
| Stormcaller | Vakum | Buz | Elektrik |

### 12.3 Trigger silahı upgrade (Faz 2)

- Cinder ile her trigger silahının **3 variant unlock** (RoR2 model)
- Örnek: Elektrik default → Chain Lightning variant (zincir +2 jump) / Concentrated Bolt (single target +%50 dmg)

---

## 13. CLASS SYSTEM PHILOSOPHY

### 13.1 RoR2 chassis + Form ultimate

Her class:
- 1 Primary (LMB, simple class damage)
- 1 Drop/Trigger control (RMB, current element)
- 3 Active skill (1/2/3 tuş)
- 1 Mobility (Shift veya class slot)
- 1 Form Ultimate (R, 45 sn cooldown, 3-7 sn duration)
- Q/E swap için universal trigger weapon

### 13.2 Class identity preservation

Her class farklı:
- **Main terrain verb** (Aquamancer "connect", Pyrotechnist "ignite", Stormcaller "compress")
- **Main payoff** (Conductive chain / Volatile boom / Vacuum collapse)
- **Mobility flavor** (slide / dash / glide)
- **Defense pattern** (slow+control / kill-before-hit / displacement)

### 13.3 Class progression yapısı

| Sezon | Class sayı | Faz |
|---:|---:|---|
| MVP | 3 | Aquamancer, Pyrotechnist, Stormcaller |
| Phase 2 | 5 | + Alchemist, + Magnetist |
| Phase 3 | 8 | + Cryomancer, + Geomancer, + Necromancer |
| Phase 4 | 12+ | + 4-6 ek class (Hero Siege scale) |

---

## 14. FULL CLASS ROSTER VISION

### MVP 3 class

**Aquamancer — Tide Authority**
- Capsule: "Su göllerini ezberle, elektrikle zincirle"
- Element subset: Su, Buz, Buhar
- Form Ultimate: Deluge Form (5 sn — geniş su/elektrik chain)

**Pyrotechnist — Volatile Engineer**
- Capsule: "Arenayı yağla boya, kıvılcımla cehennem"
- Element subset: Yangın, Statik, Plazma, Yağ
- Form Ultimate: Inferno Form (5 sn — yağ/ateş yayılma 3x)

**Stormcaller — Pressure Drift**
- Capsule: "Tozu topla, vakumla tek noktada toplama"
- Element subset: Rüzgar, Toz, Gaz, Buz
- Form Ultimate: Tempest Form (5 sn — vakum radius 2x)

### Faz 2 ek class (5 total)

**Alchemist — Caustic Painter**
- Asit + Zehir + Gaz
- Capsule: "Eriten DOT zinciri"
- Form Ultimate: Acid Pool Form

**Magnetist — Iron Conductor**
- Manyetik + Demir + Statik
- Capsule: "Mermi yönü hep değişir"
- Form Ultimate: Magnet Field

### Faz 3 ek class (8 total)

**Cryomancer — Glacier Strategist**
- Buz + Su saf
- Skill ceiling: freeze chain + shatter combo

**Geomancer — Earth Architect**
- Kaya + Lav + Demir
- Capsule: "Engelleri sen kuruyorsun"

**Necromancer — Corpse Painter**
- Karanlık + Spor + Zehir
- Capsule: "Ölüleri yan minionun yapsın"

### Faz 4 vision (12+ class)

- **Stormbringer** — Yıldırım fırtınası uzmanlığı
- **Mistweaver** — Sis + buhar görüş manipülasyonu
- **Tinkerer** — Mekanik trap + gadget
- **Spellbreaker** — Anti-element, neutralizer

---

## 15. FULL SKILL VARIANT PHILOSOPHY

RoR2 challenge unlock muadili. Her skill **2-3 variant**.

### 15.1 MVP: Variant YOK

MVP'de her skill **tek default** behavior. Variant unlock = Faz 2.

### 15.2 Faz 2 variant sistem

- Her class × 4 skill × 3 variant = **12 variant/class**
- 3 class × 12 = 36 variant Faz 2 sonu
- Unlock: Cinder + class-spesifik challenge (RoR2 model)
- Örnek: "Aquamancer Tide Pool 10 cascade trigger" → unlock Tsunami Wave variant

### 15.3 Variant types

| Variant tipi | Etki |
|---|---|
| **Range** | Skill alanı büyür/küçülür + dmg trade |
| **Speed** | Cooldown -/+ + power trade |
| **Element shift** | Skill farklı element drop'lar |
| **Hybrid favor** | Skill belli hibrit'le bonus |
| **Cascade chain** | Skill cascade chain genişler |

---

## 16. FORM ULTIMATE PHILOSOPHY

### 16.1 Form spec (LOCK)

- **Base cooldown:** 45 sn (NOT 30 — 30-50 mob'da spam riski)
- **Duration:** 5-7 sn
- **Combo charge bonus:** Combo kills cooldown azaltır (capped)
- **Voltage scaling:** Cooldown artar Voltage 5+ tier'da

### 16.2 Form variant scheme

Her class'ın Form'u **3 variant'a sahip** (Faz 2):
- Default: Identity hareketi
- Power: Daha güçlü ama kısa duration
- Tactical: Daha uzun duration, daha az power

### 16.3 Form transform identity

Her Form **görsel transformation**:
- Aquamancer Deluge → su damlası
- Pyrotechnist Inferno → alev koru
- Stormcaller Tempest → rüzgar girdabı
- (Faz 2+) — class-tema'lı transform animasyon

= Form = **identity'nin görsel hücumcu ifadesi**.

---

## 17. ACT/FLOOR LONG-TERM STRUCTURE

### 17.1 MVP yapısı (B-lite, Codex onaylı)

```
ACT 1: The Tidekeeper's Awakening
├── Floor 1 (tutorial arena, 2:00)
├── Floor 2 (normal, 2:30)
├── Fork (0:30 — Bloodlust / Gold choice)
├── Floor 3 (reward-risk, 3:00, 30-40 peak)
├── Floor 4 (elite/hazard, 3:00)
├── Floor 5 (build test, 3:00)
├── Shop/shrine (1:00)
├── Floor 6 (pre-boss, 2:30, 40-50 peak)
└── Act Boss: Tidekeeper (4:00, 3 phase + arena state)
```

Total: 21:30 per run.

### 17.2 Faz 2 — 2 Act

```
ACT 1: Tidekeeper Sanctum (su + buz dominant)
ACT 2: Foundry Furnace (yağ + statik dominant)
```

Her Act 1 boss, run = 2 Act linear (Hero Siege Act 1+2 muadili).

### 17.3 Faz 3 — 5 Dungeon × 3 Floor yapı

```
ACT 1
├── Dungeon 1: Drowned Marsh (3 floor + mini-boss)
├── Dungeon 2: Industrial Foundry (3 floor + mini-boss)
├── Dungeon 3: Frostvault (3 floor + mini-boss)
├── Dungeon 4: Sulfur Mines (3 floor + mini-boss)
├── Dungeon 5: Stormspire (3 floor + mini-boss)
└── ACT BOSS: Tidekeeper Sanctum
```

5 dungeon = element-tema'lı biome. Player linear progress.

### 17.4 Faz 4 — Atlas mode

PoE Atlas of Maps muadili. Player **dungeon path seçer**.

```
ATLAS GRID
├── Tier 1 maps (Act 1 unlock'tan)
├── Tier 2 maps (Tier 1 clear ile)
├── Tier 3 maps (special unlock)
└── Pinnacle bosses (Tier 4-5)
```

= Endgame retention. 200+ saat oynanış hedefi.

---

## 18. ATLAS / ENDGAME VISION

### 18.1 Atlas felsefesi (Faz 4)

- Atlas = dungeon ağacı, player path seçer
- Her dungeon **3 modifier** ile başlar (PoE map mod muadili)
- Modifier'lar zorluk + ödül scaling
- Atlas tier yükselt → daha iyi loot/Echo
- **Voltage modu Atlas'la birleşir** — Voltage 16+ atlas access

### 18.2 Pinnacle bosses (Faz 4)

- Tier 4-5 unique boss'lar (Atlas top)
- Her class için 1 pinnacle boss (skill mastery testi)
- Drop: cosmetic + leaderboard tag

### 18.3 Endgame loop

```
Run 1-30: Act 1 LinearAct → unlock Act 2
Run 31-60: Act 1+2 → unlock Act 3 + Voltage 10
Run 61-100: Full 5 dungeon × 3 floor → unlock Atlas
Run 100+: Atlas mode + Voltage 16-32 + Pinnacle boss + leaderboard
```

---

## 19. VOLTAGE FULL DIFFICULTY VISION

Hades Heat / PoE map mod muadili. **0-32 voltage tier**.

### 19.1 MVP: Voltage 0-3 shell

- Voltage 0 = baseline
- Voltage 1-3 = ilk modifier tier (Codex önerisi: 3 voltage modifier minimum)
- Full 0-32 sistem post-MVP

### 19.2 Faz 2-3 ek voltage

- Faz 2: Voltage 0-8 (8 modifier)
- Faz 3: Voltage 0-16 (15 modifier)
- Faz 4: Voltage 0-32 (full 32 modifier + Pact of Element)

### 19.3 Voltage modifier categories

| Kategori | Örnek modifier |
|---|---|
| **Enemy buff** | +%15 HP, +%10 speed, +%20 damage |
| **Player nerf** | Max HP -%20, dash cooldown +%20 |
| **Tile nerf** | Tile lifetime -%30, hibrit spawn -%50 |
| **Boss buff** | Extra phase, faster telegraph, minion respawn |
| **Pace stress** | Wave +1, time pressure +%20 |
| **Risk/reward** | Self-cascade damage +%50, but +%200 Echo |

---

## 20. CURRENCY VISION

### 20.1 3-tier currency

| Currency | Drop | Harcama | Persistent |
|---|---|---|---|
| **Spark** | Düşman + dalga | Run içi heal/reroll/upgrade | Run sonu silinir |
| **Cinder** | Boss + secret room | Meta unlock (class/skill/silah) | Permanent |
| **Echo** | Voltage 5+ boss + Pinnacle | Pact of Element + cosmetic + leaderboard | Permanent + Atlas access |

### 20.2 MVP

- Spark + Cinder (no Echo)
- Voltage 0-3 → Echo unlock için yetersiz

### 20.3 Faz 2+

- Echo unlock (Voltage 5+)
- Pact of Element (Hades Heat 32 muadili)
- Cosmetic shop (skin, frame, leaderboard tag)

### 20.4 ❗ NOT — Item/Loot Sistemi Açık Karar

**Bu kısım defer edildi (Faz 2 boundary):**

Codex anti-klon uyarısı: "Full loot economy = Hero Siege klonu". Ama RoR2/Megabonk feel'inde "kutu açma" + "loot drop" map exploration zevki var.

**3 farklı yaklaşım — Faz 2'de karar verilecek:**

1. **Loot YOK (mevcut plan):** Sadece modifier + currency. Build = run-content. PRO: temiz, anti-klon. CON: pickup spectacle eksik.
2. **Light loot (RoR2-lite):** Floor'da chest spawn'lar, **temporary run-item** drop (run sonu silinir). Item = trigger/skill modifier (event-driven proc). PRO: pickup spectacle + USP korunur. CON: modifier sistemiyle çakışma riski.
3. **Mastery tree (PoE-lite):** Persistent skill tree, run-arası unlock. Cinder ile node aç. PRO: Diablo audience cazip. CON: roguelite döngüsünü ARPG'ye sürükler.

**Öneri:** MVP'de hiçbiri. Faz 2 başında **playtest verisi ile karar**: "RoR2/Megabonk feel eksik mi hissediyor oyuncu?" → evet ise opsiyon 2, hayır ise opsiyon 1 sabit.

---

## 21. RELIC / MODIFIER FULL VISION

### 21.1 ModifierDef SO mimarisi (LOCK)

- **Node graph YOK** — ScriptableObject + interpreter
- Trigger enum: OnCast / OnTileSpawn / OnReaction / OnKill / OnDash / OnFormEnter
- Filter: class / element / tile-tag / status-tag / skill-id
- Operation: radiusAdd / cooldownMul / damageMul / durationAdd / spawnTile / addStatus
- ProcRules: cooldown / maxPerRoom / maxPerEvent / priority
- VFX tags + description template

### 21.2 Modifier count roadmap

| Sezon | Modifier sayı | Class başına |
|---:|---:|---:|
| MVP | 12-18 | 4-6 |
| Faz 2 | 25-30 | 5-6 |
| Faz 3 | 40-60 | 8-12 |
| Faz 4 | 100+ | 20+ |

### 21.3 Modifier kategori dengelemesi

Her modifier tier en az 1:
- Terrain modifier (tile state'i değiştirir)
- Cascade modifier (chain davranışı)
- Trigger modifier (silah davranışı)
- Class skill modifier (skill behavior)
- Defensive modifier (player nerf/buff)

**Kural:** Auto-attack build hiçbir zaman optimal olmamalı. Modifier'lar **terrain/cascade'e bağlı** olmalı.

---

## 22. ENEMY FAMILY VISION

### 22.1 MVP 4-6 enemy family

| Family | Tile drop | MVP biome |
|---|---|---|
| Su Wisp | Su gölü | Act 1 baseline |
| Yağ Sümük | Yağ patch | Act 1 baseline |
| Ateş Geist | Yangın | Act 1 baseline |
| Statik Yarasa | Statik field | Act 1 baseline |
| Buz Crawler | Buz yüzey | Act 1 baseline |
| Toz Solucan | Toz bulutu | Act 1 baseline |

### 22.2 Faz 2-3 ek family

| Family | Faz | Notes |
|---|---|---|
| Asit Pup | Faz 2 | Asit drop |
| Demir Bicep | Faz 2 | Manyetik drop |
| Zehir Örümcek | Faz 3 | Zehir web |
| Lav Karıncası | Faz 3 | Lav drop |
| Gaz Balon | Faz 3 | Gaz cloud |
| Sarmaşık Tohum | Faz 3 | Bitki yayıl |
| Cam Hayalet | Faz 3 | Cam parça |
| Karanlık Kurt | Faz 3 | Karanlık zone |
| Mantar Mob | Faz 3 | Spor yayıl |

= Faz 3 sonu 15+ family.

### 22.3 Enemy archetype pattern

Her family için:
- **Type A — Normal:** Standart spawn, drop tile ölünce
- **Type B — Charged:** Yaşarken vurursan **mini-drop** çıkarır
- **Type C — Elite (Faz 2):** Family-spesifik elite varyant

---

## 23. BOSS DESIGN VISION

### 23.1 MVP — 1 boss: Tidekeeper

- Su-themed Act 1 boss
- 3 phase × arena state interaction
- 7 skill (3 her phase + 4 phase-spesifik)
- Voltage 5+ extra phase, V16+ "Last Tide" phase 4

### 23.2 Faz 2 — 2-3 boss

- **Foundry Forgemaster** (Act 2 boss — yağ + statik)
- **Frost Maiden** (secret boss Act 1 V10+)

### 23.3 Faz 3 — 5+ boss

- Her dungeon mini-boss + Act boss
- Pinnacle boss (Faz 4 ile birlikte Atlas tier)

### 23.4 Boss design rules (her boss için)

- 3 phase MINIMUM
- Phase 2 mutlaka arena state etkileşimi (oyuncu kendi tile'ı kullansın)
- Telegraph 0.7-2.0 sn (insan reaksiyon margin)
- Telegraph rengi = damage element rengi
- Boss READABILITY > polish (Hades referans)
- Voltage scaling: V5/V16 extra phase

---

## 24. ART DIRECTION

### 24.1 Visual pillars

- **32px karakter** chibi pixel art
- **Top-down PURE** kamera (NO Hades 35° angle)
- **URP 2D + Pixel Perfect Camera + 2D Lights**
- **PPU 64**
- **Asset minimalist** — VFX merkez, sprite detay düşük
- **Color grammar** — element renk binding mutlak

### 24.2 Element renk binding (LOCK)

| Element | Renk | Hex |
|---|---|---|
| Su | Cyan | #00BFFF |
| Buz | Ice white | #B0E0E6 |
| Ateş | Orange-red | #FF4500 |
| Statik | Yellow | #FFFF00 |
| Yağ | Dark gold | #8B6914 |
| Toz | Beige | #C2B280 |
| Asit | Lime | #BFFF00 |
| Manyetik | Purple | #8B00FF |
| Lav | Red-orange | #FF6347 |
| Zehir | Magenta | #FF1493 |
| Gaz | Pale green | #98FB98 |

### 24.3 Biome paletleri (Faz 3)

- **Drowned Marsh:** Cyan + bataklık yeşili + mavi-gri
- **Industrial Foundry:** Orange + dark steel + statik sarı
- **Frostvault:** Ice white + deep blue + crystal
- **Sulfur Mines:** Lime + asit yeşili + kahverengi
- **Stormspire:** Purple + grey + lightning

---

## 25. VFX READABILITY RULES

### 25.1 5 mutlak kural

1. **VFX terrain state'i gizlemesin.** Tile color her zaman görünür.
2. **VFX duration < 2 sn** (uzun VFX = oyuncu kafası karışır)
3. **VFX color = element color** (element binding mutlak)
4. **Particle count cap** = 1500 peak, 500 normal
5. **Cascade VFX = slow-mo 0.3 sn finish** (Hades signature)

### 25.2 VFX tier

| Tier | Trigger | Asset cost |
|---|---|---|
| Tier 1 (MVP) | Hit flash + hit pause + screen shake + particle burst + damage number | Low |
| Tier 2 (post-MVP) | Bloom, trail, afterimage, palette swap | Medium |
| Tier 3 (Faz 3+) | Custom shader, depth distortion, time bend | High |

---

## 26. UI / HUD PHILOSOPHY

### 26.1 HUD essentials (LOCK)

| HUD element | Pozisyon | İçerik |
|---|---|---|
| HP bar | Top-left | Numerik + bar |
| Current trigger | Bottom-right | LMB icon + RMB footprint preview |
| Trigger swap wheel | Q/E hold | 5 element circular |
| Skill cooldown | Bottom-center | 3 skill + Form + dash |
| Combo streak | Top-center | 5/15/30/50 tier |
| Modifier list | Right side | Active modifier sayı |
| Currency | Top-right | Spark / Cinder |
| Voltage indicator | Top-left below HP | V0-32 |

### 26.2 UI yasak patternleri

- **Hades boon UI** mimicry YOK (god boon presentation klonu)
- **PoE passive web** YOK (loot/tree obsession)
- **Item rarity color** obsession YOK (terrain grammar oturana kadar)

### 26.3 HUD readability

- Tile state overlay (debug mode) toggle
- Color blind support (element renk hat'ı + sembol)
- VFX limiter slider (oyuncu seçer)
- Cascade preview (RMB drop sırasında danger border)

---

## 27. AUDIO FANTASY

### 27.1 Audio pillars

1. **Cascade audio = bass drop** (TikTok klip için kritik)
2. **Element audio binding** — su splash, ateş crack, elektrik zap (her tetik tanıdık)
3. **Combo streak audio escalation** — 5/15/30/50 tier farklı ses
4. **Boss telegraph audio cue** — visual telegraph + 0.2 sn audio anticipation
5. **Slow-mo finish audio** — ambient → silent → boss death sting

### 27.2 Music

- **Ambient combat track** her biome için (Faz 3+)
- **Boss music** unique per boss
- **Death/respawn sting** memorable
- **TR composer referans:** Aytaç Doğan (electronic + Turkish) potansiyel

### 27.3 Audio mixer cap

- Aynı SFX max 4 paralel (audio chaos önleme)
- Cascade VFX audio prioritize (UI ses azalır)

---

## 28. MARKET POSITIONING

### 28.1 Tür konumu

**"Real-Time Generative Action Roguelike"** = kategori adı (public).

Pazardaki en yakın 3:
- **Magicraft** (700K, $15.99) — spellcraft action roguelike
- **Hero Siege** (1M+, $7.99-14.99) — pixel ARPG roguelike
- **Risk of Rain 2** (4M+, $24.99) — 3D action roguelike

CB pozisyon: **2D pixel, 32px, $14.99, ARPG-lite + spatial USP**.

### 28.2 Steam tag öncelik (LOCK)

1. Action Roguelike
2. Action RPG
3. Roguelike
4. Top-Down
5. Magic
6. Bullet Hell (eğer projectile pressure visible)
7. Hack and Slash (ARPG sense)
8. Pixel Graphics
9. Arena Shooter
10. 2D

### 28.3 Yasak tag (primary)

- Survivor-like (passive expectation)
- Auto Battler (wrong input)
- Deckbuilder (wrong system)
- Simulation (Noita comparison)
- Souls-like (combat promise wrong)
- Metroidvania (progression wrong)
- Loot (avoid until item economy exists)
- MMO (scale wrong)

### 28.4 Fiyat (LOCK)

- **EA + launch: $14.99**
- Launch discount: 10%
- Faz 2 update discount: 15-20%

### 28.5 Sales target

- **Year 1 hedef:** 100K satış
- **Wishlist KPI:** 50K minimum, 100K+ breakout
- **Demo KPI:** Median 25 dk + 1 run completion / player
- **Trailer KPI:** First 5 sec = cascade (not walking)

### 28.6 TR/Asya pazarı not

- Hades / Diablo / PoE / RoR2 audience Türkiye'de devasa
- TR/ZH/KO yerelleştirme Faz 2-3 stratejik
- Magicraft Asya'da güçlü → CB için TR/Asya potansiyel

---

## 29. ANTI-CLONE GUARDRAILS

### 29.1 Hard rules (her trailer beat'i için)

1. Tile causality görünmeli (her hit/cascade)
2. Class'ın **terrain verb**'ü olmalı (damage verb değil)
3. Modifier tier'ında **terrain/cascade modifier** olmalı
4. Boss arena state ile **mutlaka etkileşmeli**
5. Auto-attack build optimal **OLMAMALI**
6. Wand graph **YASAK** (Magicraft klonu)
7. Continuous physics **YASAK** (Noita klonu)
8. Full loot economy **MİNİMUM MVP** (Hero Siege klonu)
9. UI patterns Hades boon mimicry **YASAK**
10. Item rarity color obsession terrain grammar oturana kadar **YASAK**

### 29.2 Klon riski oyun bazlı

| Oyun | Risk | Mitigasyon cümle |
|---|---|---|
| Hero Siege | Med | "Hero Siege asks what your build can kill; CB asks how you reshape the room before you pull the trigger." |
| RoR2 | Low | "RoR2 is survival through item-stack momentum; CB is battlefield authorship through elemental terrain and timing." |
| Magicraft | Med-High | "Magicraft builds spells; CB builds rooms." |
| Noita | Med | "Noita lets the world escape control; CB lets the player weaponize control in real time." |
| Hades | Med | "Hades is reaction mastery; CB is setup-payoff mastery." |
| Magicka | Med | "Magicka recipes happen in your hand; CB recipes happen on the floor." |
| Vampire Survivors | Low | "Survivor-likes reward surviving your build; CB rewards designing the next screen clear under pressure." |

---

## 30. LONG-TERM CONTENT PHASES

### Phase 1 — MVP (Week 1-16)

- 3 class, 5 trigger, 7 base tile, 3 hibrit, 3 status hibrit
- 12-18 modifier
- 4-6 enemy family
- 6 floor + 1 fork + 1 shop + 1 Act boss
- Voltage 0-3 shell
- Spark + Cinder currency
- 1 biome (Act 1)
- Demo + Steam Next Fest target

### Phase 2 — Months 4-6

- 5 class (+ Alchemist, Magnetist)
- Skill variant unlock sistemi (12 variant/class × 3 = 36 variant)
- 25-30 modifier
- 2 Act (Act 2 unlock)
- 2 boss (Act 2 boss)
- Voltage 0-8
- 7 enemy family (+ Asit Pup, Demir Bicep)
- Item/mastery tree **decision point** — playtest verisi ile karar

### Phase 3 — Months 6-12

- 8 class (+ Cryomancer, Geomancer, Necromancer)
- 5 dungeon × 3 floor full structure
- 40-60 modifier
- 5+ boss (her dungeon mini-boss + Act boss)
- 12+ enemy family
- Voltage 0-16
- 8+ tile state, 8+ hibrit
- 11 element (full taxonomy)
- Echo currency unlock
- Secret boss + hidden rooms
- TR/ZH/KO localization

### Phase 4 — Months 12+

- 12+ class
- Atlas mode (PoE map grid)
- Voltage 0-32 + Pact of Element
- Pinnacle boss × class
- Daily/Weekly seed + leaderboard
- Seasonal content drop (3-6 ay)
- Steam Workshop / mod support (opsiyonel)
- Co-op (opsiyonel — Magicka pattern)

---

## 31. EXPLICIT NON-GOALS

CB **bunları YAPMAYACAK** — vizyon dışı:

1. **3D oyun** — 2D pixel art final
2. **Auto-combat** — manuel zorunlu
3. **Open world** — run-based instances
4. **Always-online** — singleplayer öncelik
5. **MMO scale** — co-op max 4 player (opsiyonel)
6. **Lootbox / TCK 228 risk** — premium oyun, run içi RNG parasız
7. **NFT / blockchain** — yasak
8. **Story-heavy narrative** — lore parça opsiyonel, ana mekan değil
9. **Sandbox/editor** — Faz 4 sonrası opsiyonel mod support
10. **Mobile port** — desktop öncelik
11. **F2P** — premium $14.99

---

## 32. DECISION LOG

Bu doküman boyunca alınan ana kararlar (kronolojik):

| Tarih | Karar | Niye | Status |
|---|---|---|---|
| 2026-05-18 | RIMA'dan CB'ye pivot | RIMA tile pipeline drift, CB scope temiz | LOCKED |
| 2026-05-18 | Sub-genre = Real-Time Generative Action Roguelike | User explicit lock, Codex Cascade ARPG alternative | LOCKED (user) |
| 2026-05-18 | 2-mod trigger weapon (LMB trigger + RMB drop) | Self-combo USP | LOCKED |
| 2026-05-18 | 3 MVP class (Aquamancer, Pyrotechnist, Stormcaller) | Codex onaylı scope | LOCKED |
| 2026-05-18 | Universal 5-trigger + class affinity | Codex onaylı (class-bound element grammar'i öldürür) | LOCKED |
| 2026-05-18 | Form cooldown 45 sn baseline | Codex onaylı (30 sn spam riski) | LOCKED |
| 2026-05-18 | Map = Hero Siege Act/Floor B-lite (MVP) | Codex onaylı, vizyon B-full | LOCKED |
| 2026-05-18 | 6 floor + fork + shop + boss (MVP) | Codex onaylı | LOCKED |
| 2026-05-18 | 30-50 mob peak (MVP) | User confirmed mass-clear feel | LOCKED |
| 2026-05-18 | Item/Mastery tree = DEFER (Faz 2 boundary) | RoR2/Megabonk feel vs anti-klon | OPEN |
| 2026-05-18 | Modifier = ScriptableObject + interpreter | Codex onaylı (node graph scope creep) | LOCKED |
| 2026-05-18 | 3 hibrit MVP (Slush, Emulsion, Volatile) | Codex onaylı | LOCKED |
| 2026-05-18 | Tile state event-driven dirty cells | Codex mimari onaylı | LOCKED |
| 2026-05-18 | Voltage 0-3 shell (MVP) | Codex onaylı (full 0-32 Faz 4) | LOCKED |
| 2026-05-18 | Spark + Cinder MVP, Echo Faz 2+ | Codex onaylı | LOCKED |
| 2026-05-18 | Karar #143 6-layer composition SİL | RIMA legacy, CB için irrelevant | LOCKED |
| 2026-05-18 | RIMA Map Designer port (palette/RuleTile/brush/RoomTemplate sakla) | Codex onaylı | LOCKED |
| 2026-05-18 | Pivot timing = Option C Hybrid (1 wk doc + 2 wk RIMA POC + 5 gate) | Codex onaylı | LOCKED |
| 2026-05-18 | Fiyat $14.99 EA + launch | Codex pazar analizi | LOCKED |
| 2026-05-18 | Year 1 hedef 100K satış | Codex Magicraft/Soulstone benchmark | TARGET |
| 2026-05-18 | TR/Asya yerelleştirme Faz 2-3 | TR pazarı + Magicraft Asya başarı | TARGET |
| 2026-05-18 | 3-document yapı (VISION/MVP/ROADMAP) | Codex onaylı, user istek | LOCKED |

---

## 📁 KLASÖR YAPISI

Bu doküman ekosistemine yön bulmak için:

```
F:\LaurethStudio\02_GAMES\CircuitBreaker\
├── README.md                          # Folder index + future-self pickup
├── 00_FULL_DESIGN_DOC.md              # ORIGINAL design doc (S81 versiyonu — legacy reference)
├── 01_MVP_PLAN.md                     # 16-week concrete plan (REVISE post-VISION_DOC)
├── 02_VFX_LIBRARY.md                  # 20 VFX teknik
├── 03_MARKET_REFERENCE.md             # Pazar analizi
├── 04_AI_RESEARCH_INDEX.md            # AI brainstorm kaynaklar
├── 05_PIVOT_DECISION_2026-05-18.md    # Pivot karar + 5 gate
├── 06_VISION_DOC.md                   # ← BU DOSYA (long-term identity)
├── 07_ROADMAP.md                      # Faz 2/3/4 timeline
├── CLAUDE.md                          # Claude orchestrator config
├── CURRENT_STATUS.md                  # Anlık session status
├── MEMORY/
│   └── MEMORY.md                      # CB-spesifik memory
└── STAGING/
    ├── CB_Mechanic_Brainstorm_*.md    # Brainstorm draft
    └── CB_V4_FAUX_ISO_PROTOTYPE_PLAN.md  # Eski iteration
```

**Doc öncelik sırası (ne zaman aç):**
- Session başında: `README.md` + `CURRENT_STATUS.md`
- Vizyon sorgusu: **bu dosya** (06_VISION_DOC.md)
- "Bu hafta ne yapacam?": `01_MVP_PLAN.md`
- "MVP sonrası ne?": `07_ROADMAP.md`
- "Pivot karar nedir?": `05_PIVOT_DECISION_2026-05-18.md`

---

## 🔚 BU DOSYANIN OMÜRÜ

Bu dosya **canlı vizyon belgesi**. Aylık güncelleme normal:
- Yeni karar = Section 32 Decision Log'a ekle
- Sub-genre LOCK (Section 2) **NEVER CHANGE** — user lock
- Anti-klon guardrails (Section 29) **NEVER WEAKEN** — sadece güçlendir
- Phase timing (Section 30) — gerçekle uyuşmazsa güncelle (Faz 2 6 ay → 9 ay olabilir)

**Son revize:** 2026-05-18 (initial lock)
**Sonraki review:** MVP Week 8 (Aquamancer + Pyrotechnist playable sonrası — vizyon hala valid mı?)
**Sonraki major revize:** MVP ship sonrası (Faz 2 başlangıcı, item/mastery karar noktası)

---

VISION DOC v1 — END
