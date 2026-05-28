# Heroes of Hammerwatch II — RIMA + LaurethStudio Analiz

**Tarih:** 2026-05-16
**Hedef:** RIMA (Karar #143 6-layer map LIVE, Rift Break meta progression aday) + LaurethStudio universal pattern
**Karsilastirma noktasi:** Heroes of Hammerwatch (2018, Crackshell) — RIMA tematik akraba (top-down pixel art roguelite, town hub + dungeon run + meta progression)

---

## 0. Hizli Ozet (TL;DR)

- **HW2 = "more of the same + more depth"**: 7 class (3 spec/class), 8 floor type, town hub upgrade, 4-player co-op, NG+ endless. Steam **Very Positive %84 / 3996 review**, son 30 gun **Mostly Positive %71** — yani momentum biraz dususte.
- En guclu yan: **town hub progression loop** + co-op chemistry. En zayif yan: **HW1'den "geri adim" hissi** (daha az relic, daha az build derinligi, "fun sucked out" senaryosu).
- RIMA icin dogrudan referans: **Town building -> dungeon altering buffs**, **Blueprint permanent unlock**, **floor-spesifik material drop ekonomi**, **revive currency (key yerine)**.
- LaurethStudio icin: **Town hub + dungeon run yapisi her uc oyun (RIMA, CircuitBreaker, Caterpillar) icin viable shell** — ama "sequel reuse %85" tuzagi konusunda ders var.

---

## 1. HW2 Genel Tanim + 1'den Farki

### Ana veriler
- **Cikis:** 14 Ocak 2025 (full release, NOT Early Access)
- **Stüdyo:** Crackshell (orjinal Hammerwatch yaraticilarinin devam stüdyosu)
- **Tur etiketleri:** Action RPG, Action Roguelike, Dungeon Crawler, Roguelite, Pixel Graphics, Top-Down 2D
- **Steam app id:** 619820 (HoH2). DIKKAT: Ayri bir oyun olan **"Hammerwatch II" (app 1538970)** non-roguelike RPG sequel — review skoru cok daha dusuk (Mixed %60). Bu ikisi karistirilmasin.
- **Co-op:** Online 4-player

### HW1 -> HW2 ana farklar
| Sistem | HW1 (2018) | HW2 (2025) |
| --- | --- | --- |
| Class sayisi | 7 base + DLC | 7 base, her biri **3 specialization** |
| Movement | Yok dash | **Dash eklendi** |
| Hasar tipi | Tek elemental | **4 element** (fire/ice/lightning/poison) |
| Resource | Gold + Ore | Gold + Ore + **stone fragmented** |
| Death penalty | Key-based revive | **Revive cost system** (chapel ile reduce) |
| Floor sayisi | Tek tower (kat sirali) | **8 distinct floor type**, daha modular |
| Story | Minimal | **Dialog-driven NPC quest sistemi** (zamansal aciliyetli) |
| Buildings | Sabit liste | **Guild Hall tier-gated** unlock |
| World | Kapali tower | **Over-world + multi-level dungeons** |
| Boss | Klasik bullet hell pattern | 6 act boss + multi-phase (Totem Pole = 4 element fazi) |

### HW1 fanlarinin reaksiyonu (Steam community ozeti)
- "Amazing sequel to the OG HoH" — beastlycooner [478 helpful, 102.4 hr]
- AMA: "Less is the key word for this sequel. Less complexity, less depth, less content" — Cybersquidz [796 helpful, 31 hr] (negative)
- "Heroes of Hammerwatch 2 feels like it is reusing 85% of Heroes of Hammerwatch 1, with so many of the same items, enemies, and such similar graphics" — yaygin elestiri
- Takimlasma: HW1 pure "tower climb arcade" hissini ozluyorlar; HW2 "open-world quest" tercihi bolunmus tepkide.

**RIMA notu:** Steam reviews patterni RIMA icin uyarici — RIMA Karar #143 6-layer map ile zaten dramatic visual upgrade vaat ediyor, ama core gameplay loop (room run + meta progression) eger "HW1->HW2 reuse" hissi verirse same trap. Cozum: visual upgrade + **mekanik gercek farklilasma** (10 class + cross-class skill 80 toplam — HW2'nin 7x3=21 spec ile karsilastirilabilir).

---

## 2. Visual Direction

### HW2 sanat profili
- "Stunning pixel art, perfect mix of nostalgia and polish that looks like SNES/early PC days but much more refined" (Superstar Reviews)
- HW1'e gore: **daha varied interiors, daha distinctive character sprites, dense over-world**
- Lighting effects, weather, environmental detail — atmospheric upgrade
- Pixel density: HW1 ile karsilastirilabilir (rough ~32px hero scale), DETAIL artisi sprite-per-tile dansitesinde

### Biome/floor cesitliligi
- **8 floor type:** Forest, Dark Caves, Temple, Crypt, Dungeon, Barracks, ve daha fazlasi
- Her floor distinct enemy archetype: Forest/Caves = beasts, Temple = cultists, Crypt = undead, Dungeon = thorn plant hazard
- Environmental hazard variation (thorn spikes, plant patches) = floor identity'ye katki

### Karar #143 6-layer map icin referans noktalar
RIMA'nin 6-layer mimarisi (PatchAtlas + CornerWang + WallOverlay + AccentPainter + DetailDecal + TransitionBrush) HW2'nin **multi-pass dungeon visual layering** stratejisiyle uyumlu:

1. **Floor identity per biome zorunlu**: HW2 8 floor type icin floor texture + foliage + decal + hazard variation kullaniyor — RIMA'nin PatchAtlas + AccentPainter combo'su buna birebir denk dusuyor.
2. **Distinctive character vs environment kontrasti**: HW2 incelemelerinde "distinctive character sprites" oviluyor — RIMA 128px native (Karar #128 LOCKED) ile zaten avantajli, ama floor pattern'in karakterle ayrismasi icin **AccentPainter density** test gerekiyor.
3. **Lighting/weather katmani**: HW2 atmospheric immersion icin lighting kullaniyor — RIMA `project_lighting_wip.md` Ph 2 torch fix ile bu yonu zaten on-roadmap'te.

**Eksi:** "Blurry/Fuzzy Foilage?" Steam thread'i — pixel art smooth-filter glitch sikayetler. RIMA pixel-perfect render policy zaten lock'lu (`project_visual_quality.md`).

---

## 3. Combat / Class System

### Class roster
**HW2: 7 class** (4 unlock'lu + 3 unlockable)
- Warrior, Paladin, Ranger, Wizard (start)
- Rogue, Warlock, Sorcerer (unlock via gameplay)
- Her class **3 specialization** = 21 build base

**RIMA: 10 class** (Warblade/Ravager/Brawler + 7 daha) + **80 cross-class skill** (`project_cross_class_skills.md`)

### Karsilastirma
| Boyut | HW2 | RIMA |
| --- | --- | --- |
| Base class | 7 | **10** (+%43) |
| Per-class build path | 3 spec | 1 main + 2 cross-class slot |
| Toplam build matrix | 21 | **10 x 80 cross combo** = cok daha buyuk |
| Respec | Free, anytime | TBD (henuz spec'te yok) |
| Element split | 4 (fire/ice/lightning/poison) | Class-spesifik (Elem orb pivot — `project_class_identity_pivots_s43.md`) |

### Skill design
- HW2 incelemesi: "Engaging enough", "skills lack impact" sikayeti yaygin
- En kotu negative quote: "tiny fireball, or just a couple homing projectiles" (Cybersquidz)
- Boss design: Erken bosslar "stationary objects that endlessly spawn hordes" — RIMA boss tasarimi icin ANTI-pattern

### Hit feedback / juice
- HW2 OP yorumu: "Town building feels rough, no interesting feedback. Buildings just instantly materialise out of thin air. Combo lacks visual counter and warning indicators."
- "Money pickups feel less satisfying overall"
- RIMA `project_feel_toggles.md` (Shake/Vignette/Hitstop ON LOCKED) = HW2'nin tam aksini garanti ediyor — bu bir RIMA differentiator.

### Boss design (RIMA icin not)
- HW2'de **6 act boss**: Totem Pole (4-phase, fire/ice/earth/air random siralama), Shadow Face (cult summon), Plant Boss, Harbingers of Dread, Serpents of Demise
- **Multi-phase + element rotation** bir sablon — RIMA Phase 2/3 boss icin uygulamabilir

---

## 4. Map / Dungeon Design (RIMA icin EN KRITIK bolum)

### Procgen yapi
- **Randomized layout per dungeon**: enemy placement, temporary buffs, trinket chest position
- 8 floor type her run icin rotation (sequential floor progression)
- "Each dungeon is completely randomised" (Rogueliker review)
- AMA: floor **template yapisi handcrafted** — HW1'in tower struktur falsefiyesi korundu

### Floor structure
- **Sequential floor**: floor 1 -> boss check -> floor 2 -> boss -> ...
- Her 2 floor'da bir **area level +1** (NG+1'den itibaren)
- Per-floor: rooms + corridors + secret doors + NPC encounter slots

### Town hub yapi (RIMA Sanctum karsiligi)
- **Guild Hall = ana bina**, tier-gated upgrade
- Buildings list:
  - **Hunters Cabin**: Forest enemies material drop (gold conversion run sonu)
  - **Chapel**: Revive cost reduce
  - **Treasury, General Store, Blacksmith, Ore Trader, Apothecary, Fountain, Tavern, Magic Shop**
- Yapi mantigi: **Floor X material -> Building Y unlock -> Run Z buff**
- Town building = "long meta loop" + dungeon = "short loop"

### Multiplayer co-op map streaming
- **Drop-in seamless**: ready-up otomatik check, host start
- Map state shared (host authoritative)
- AMA difficulty: **highest level player'a scale eder** -> low-level players "tedium" yasar
- Loot: Gold/ore/keys SHARED, equipment chest = **per-player visit gerekli** (kendi item'i)

### Discovery / secret density
- "Sprawling, multi-level dungeons filled with plenty of secrets and deadly traps" (Windows Central preview)
- Secret door + encounter slots procgen'de seed-locked
- Steam guide var: "Secrets & Encounters in Heroes of Hammerwatch 2" — community-driven discovery culture

### RIMA icin direkt aksiyon
**Karar #143 6-layer pipeline ZATEN HW2 floor identity quality bar'ini hedefliyor.** RIMA spesifik avantaj/risk:
- **Avantaj:** RIMA procedural pivot (Karar #134) + 6-layer mimari = HW2'den daha modular floor variation
- **Risk:** HW2'nin "8 floor type" sayisi yetersiz bulunmus (community thread "Dungeons?" — daha fazla biome talebi)
- **Aksiyon:** RIMA icin minimum **6 act biome + Sanctum hub** = HW2 paritesi. Karar adayi: **#150 Biome Roster Lock**.

---

## 5. Meta Progression (RIMA Rift Break icin DOGRUDAN REFERANS)

`project_rift_break.md` (Phase 4-5 meta) bu bolumun ana hedefi.

### Town upgrade ekonomisi
HW2 mimarisi:
- **Gold (per-run + carry)**: Town building purchase, fountain buff
- **Ore**: Building tier upgrade material
- **Stone (split resources)**: Specific blueprint requirement
- **Material (floor-spesifik)**: Hunters Cabin = Forest material; her building bir floor'a bagli

### Permanent unlock currency
- **Blueprint discoveries**: dungeon'da bulunup town'da unlock — permanent character/equipment ozellik
- **Star perk**: HW1'den taşinmis (achievement-based stat boost)
- **Character level**: persistent (ölmesen NG+)
- **Permanent equipment collection**: bulduklarin lock-in
- **Run-spesifik trinket**: passive bonus, run-only

### Per-run vs persistent dengesi (RIMA icin EN onemli takeaway)
| Layer | HW2 ornegi | RIMA Rift Break karsilik onerisi |
| --- | --- | --- |
| **Per-run only** | Trinket pickup, fountain fortune, temporary buff | Run modifier slot |
| **Per-run -> persistent convert** | Material drop -> end-of-run gold | Rift fragment -> meta currency |
| **Persistent unlock** | Blueprint, building tier, class spec | Cross-class skill unlock, sanctum tier |
| **Account-level** | Star perks, character collection | Class roster expansion |

**4 layer separation = derinlik garantisi.** HW2 incelemelerinde "multiple progression systems dilute individual upgrade impact" (OP fun-sucked-out thread) sikayeti var — RIMA layer baginda **her upgrade tek seviyede pop ettirilmeli** (not "+5% small bonus" trap).

### NG+ pattern
- **Endless scaling**: NG+ unlock = Avatar boss kill sonra
- **Level cap formula:** `20 + (NG_level x 5)`
- **Area level:** NG+1'de baslangic 18, her 2 floor +1, her NG +5
- **Difficulty:** NG+1 = enemy size +%15, enemy count +%33 (multiplicative stack with Bigger Floors fountain)
- **Reward:** Gold/ore +%(20 x NG_level)
- **Sikayet:** Ovetuned, "from positive armor to negative armor" cliff (NG+ scaling thread)

### "Why come back?" hook (RIMA icin)
HW2 hook ranking (Steam community):
1. **Co-op chemistry** (en guclu): "Town building, loot, classes and killing equals tons of fun, even better with friends" — Unzeal [296 helpful, 110.6 hr]
2. **Town building progression visibility**: "solid grind has been so fun and rewarding" — Scrufington [623 helpful, 139.2 hr]
3. **Class spec variety**: 7x3 = 21 build, respec free
4. **NG+ endless**: leaderboard/competitive depth

**RIMA Rift Break icin:** RIMA solo-first roadmap'inde "co-op chemistry" hook YOK — bu nedenle **Sanctum visual + Cross-class build matrix + Rift tier prestige** uclu hook olmasi sart. HW2'nin 21 build vs RIMA'nin **10 x 80** cross potansiyeli zaten teorik avantaj — exposeR ekrani gerekli.

---

## 6. Audio

- **Composer:** Two Feathers Studio (Elvira Björkman + Nicklas Hjertberg) — HW1 muzigini de yapan ekip
- **Trailer music:** Enzo Margaglio (separate)
- **Community sentiment:** "Music isn't as hype as it should be" thread sikayeti
  - HW1 muzigi "catchy, upbeat, combat-focus"
  - HW2 muzigi "exploration/atmosphere vibe" — combat hype dustu
- **Combat sting:** Spesifik "hit/kill sting" critical mention yok — buyuk bir bosluk

**RIMA icin not:** RIMA SFX pipeline (Stable Audio Open 1.0, RTX 5080 — `project_sfx_pipeline.md`) zaten **per-class signature sound** + **combat juice sting** yonunde — HW2'nin yapamadigi seyi RIMA'nin sahiplenebilecegi alan.

---

## 7. Storytelling

### HW1 -> HW2 fark
- HW1: Minimal lore, "tower climb" silent
- HW2: **Dialog-driven NPC quest sistemi**, plot daha "fleshed out" (Rogueliker)

### Quest mekanik
- NPC dialog **aciliyet** ileten yapiyla (water filter NPC ornegi: oyuncu olmazsa NPC kendisi cozer, time-sensitive)
- AMA UX problem: "no quest indicators for NPCs", "failing timed quests with no indication that the quest is timed"
- Class unlock NPC dungeon icinde gizli — explore reward

### RIMA icin potansiyel
RIMA halen "silent class identity" yonde gidiyor (`project_class_identity_pivots_s43.md`). HW2'den ders:
- **Quest layer eklemek istiyorsan UX indicator zorunlu** (HW2 hatasi: indicator yok)
- **NPC = building/class unlock gateway** = town progression'a bagli loop
- **Time-sensitive quest** = roguelite yapida tehlikeli (run-time sinirli zaten)
- **Onerilmiyor:** RIMA'da story depth = scope creep riski. Class identity + sanctum NPC dialog (3-4 satir tipi) yeterli.

---

## 8. Multiplayer Design (RIMA solo/co-op kararı icin)

### HW2 multiplayer
- **Online co-op 4-player**, drop-in
- Difficulty: **highest level scale** (problem)
- Loot: hybrid (gold/ore shared, equipment per-player visit)
- Lobbies: any size, public + friend
- **Server populated** (Rogueliker, healthy)

### Sikayetler (multiplayer)
- "coop scaling is too hard" thread
- "Multiplayer scaling is WACK" thread
- "Public online is a $%$ show. Needs balance" thread
- "host offline = karaktere giremiyorsun" cloud save eksikligi

### RIMA karari icin oneri
RIMA su an **solo-first** scope'lu (CURRENT_STATUS check gerekli — `.claude/PROJECT_RULES.md` ref). HW2 verisinden cikan tavsiye:

**Solo-first lock'u koruyun. Co-op = Phase 4+ post-launch DLC scope.** Sebep:
1. HW2 development effort'unun buyuk yuzdesi multiplayer scaling balance icin harcanmis (community sikayet hacmi gosterge)
2. RIMA combat (`project_combat_architecture.md`) v4 dash + projectile deflect = network sync zor
3. Co-op olmadan da hook viable: HW2 single-player fan'lari (mau64 [60 helpful, 34 hr Steam Deck go-to]) varlar.

---

## 9. Steam Reviews — Ozet

### Genel skor
- **Lifetime:** Very Positive %84 / 3996 review (English)
- **Son 30 gun:** Mostly Positive %71 / 105 review
- **Trend:** Yumusak dusus (launch'tan 1+ yil sonra mid-cycle fatigue)

### Top 5 BEGENILEN nokta

1. **Town building loop** — Steam user Scrufington [623 votes, 139.2 hr]: "solid grind has been so fun and rewarding"
2. **Co-op chemistry** — Steam user Unzeal [296 votes, 110.6 hr]: "Town building, loot, classes and killing equals tons of fun, even better with friends"
3. **Long-haul replayability** — Steam user beastlycooner [478 votes, 102.4 hr]: "Amazing sequel to the OG HoH" (102 saat = derinlik kaniti)
4. **Solid combat foundation** — Steam user Spence [343 votes, 12.1 hr]: "solid action rpg with impactful abilities and a slow-to-start progression"
5. **Steam Deck portability** — Steam user mau64 [60 votes, 34 hr]: "really enjoying my time" (Deck go-to setup)

### Top 3 SIKAYET

1. **HW1 reuse + "less depth" hissi** — Steam user Cybersquidz [796 votes, 31 hr] (TOP NEGATIVE): "Less is the key word for this sequel. Less complexity, less depth, less content" + "98 vs 130 relic"
2. **End-game cap'leme** — Steam user Fengtorin [362 votes, 390.5 hr]: "all of it caps out while the enemies keep on improving" (NG+ scaling cliff)
3. **Combat feel underwhelming** — Steam OP "fun sucked out" thread: "tiny fireball, or just a couple homing projectiles", "town buildings just instantly materialise out of thin air", "buildings instantly materialize" feedback eksikligi

### "1. oyun fan'i icin" verdict
**MIXED.** Casual HW1 fan'i memnun (kor takibi) AMA hardcore HW1 fan'i (300+ saat oyna) sequel'i "step back" goruyor. Buyuk asset reuse + sistem fragmentation hissi.

### "Buy if you like X" karsilastirma
- Diablo (orijinal) + roguelite layering
- Hammerwatch 1 (acelesi yok, derin meta)
- Risk of Rain 2 / Curse of the Dead Gods (run-based action)
- DEGIL: Hades (run-spesifik narrative + boss spectacle), Dead Cells (movement tightness)

---

## 10. RIMA icin BORROW Listesi (Karar Adayi)

### #150 — Floor-Spesifik Material Drop Ekonomi
**Pattern:** HW2 Hunters Cabin = Forest enemy material -> end-of-run gold conversion.
**RIMA uygulama:** Her biome 1 unique material drop -> Sanctum specific building unlock material. 6 biome = 6 material loop.
**Etki:** Biome'lara replay value yukler, "skip biome" yapilamaz.

### #151 — Town Building Tier-Gate (Guild Hall pattern)
**Pattern:** HW2 Guild Hall = ana bina, tier upgrade her seferinde N building/upgrade prerequisite ister.
**RIMA uygulama:** Sanctum Core = ana yapi, tier 2 = 5 building + 10 upgrade gerekli, vs.
**Etki:** Long-term progression visibility + soft pacing.

### #152 — Blueprint Discovery (Permanent Unlock Currency)
**Pattern:** HW2 dungeon'da bulunan blueprint -> town'da inşa.
**RIMA uygulama:** Rift Break icinde "Rift Schema" pickup -> Sanctum'da unlock (cross-class skill veya class spec).
**Etki:** Rift Break exploration reward = permanent yeni icerik, run currency degil.

### #153 — Revive Currency System (Key Yerine)
**Pattern:** HW2 Chapel = revive cost reduce, key yerine economic decision.
**RIMA uygulama:** Death = "Soul Tax" (next run starting gold/skill - %X). Sanctum building = tax indirim.
**Etki:** Permadeath softening + meta sink.

### #154 — Element Split (4 element lock)
**Pattern:** HW2 hasar split: fire/ice/lightning/poison (4 layer).
**RIMA uygulama:** Class identity zaten elemental (Elem orb pivot); 4 base element + 1 class-signature element formaliste hale getirilebilir.
**Etki:** Build diversity + element resist enemy design alani.

### #155 — Multi-Phase Element Rotation Boss (Totem Pole pattern)
**Pattern:** HW2 Totem Pole boss = 4 faz (fire/ice/earth/air), random siralama.
**RIMA uygulama:** Phase 2 ana boss = 4 element fazi, run sirasinda random rotation -> 24 permutation.
**Etki:** Boss replayability tek bossta ucla.

### #156 — Free Respec Per Visit
**Pattern:** HW2 spec/skill respec free (Sanctum visit).
**RIMA uygulama:** Sanctum'da return ile cross-class slot respec = free, dungeon icinde locked.
**Etki:** Build experimentation friction sifir, decision-fatigue az.

### #157 — Time-Sensitive NPC Hook (UX FIXED)
**Pattern:** HW2 NPC quest aciliyet (water filter), AMA UX indicator yok = sikayet.
**RIMA uygulama:** Sanctum NPC = N run'da quest tamamla yoksa NPC kendisi cozer + reward kayipla. AMA RIMA UX zorunlulugu: **Quest indicator + countdown badge** yapilmis olmali.
**Etki:** Sanctum hub'a "yasayan" his katiyor, scope minimal.

### #158 — Floor-Type Hazard Identity (Plant thorn pattern)
**Pattern:** HW2 Dungeon floor = thorn plant patch hazard (floor identity = mechanic identity).
**RIMA uygulama:** Her biome kendine ozel hazard (Lava biome = magma jet, Crypt = bone trap, vs).
**Etki:** Karar #143 6-layer mimaride **AccentPainter + DetailDecal layer'i** mekanik tetikleyiciye baglanir.

### #159 — Run Trinket Slot (Per-Run Passive)
**Pattern:** HW2 trinket = run-spesifik passive bonus (chest pickup).
**RIMA uygulama:** Run-spesifik 2-3 trinket slot (mevcut cross-class skill 2 slot'tan AYRI). Run sonu kaybolur.
**Etki:** Per-run identity + RNG variety, persistent power degil.

---

## 11. RIMA icin REJECT Listesi

### Reject #1 — "%85 Asset Reuse" Stratejisi
**HW2 hatasi:** Cybersquidz [796 votes]: "98 vs 130 relic, copied items renamed, almost every relic from HOH1 split into 4 elemental".
**RIMA icin:** RIMA Karar #143 ile zaten 6-layer rebuild. ESKI sprite library (`Assets/Art/_archive_faz1`) referans amacli korunsun, AMA prod path'e geri dahil edilmesin. Yeni biome assetleri TAM original.

### Reject #2 — Multiplayer-First Scaling Balance
**HW2 hatasi:** Difficulty highest-player scale = solo + co-op tedium (community thread hacmi).
**RIMA icin:** Solo-first lock korunsun. Co-op scope = Phase 4+ post-launch (yukarida 8. bolum). Combat tuning solo benchmark uzerinden.

### Reject #3 — "Small +5% Upgrade" Pattern
**HW2 hatasi:** OP fun-sucked-out thread: "upgrades are mostly measly +5% increases that you won't even notice".
**RIMA icin:** Sanctum + Rift Break upgrade'leri **noticeable mekanik degisiklik** uzerinden tasarlansin (yeni skill, yeni slot, yeni hazard immune), pure stat +%X degil.

### Reject #4 — Combat Sting/Hype Eksikligi
**HW2 hatasi:** Two Feathers HW2 OST exploration-vibe dustugunde combat sting eksik kaldi (community: "music isn't as hype").
**RIMA icin:** SFX pipeline'da **per-action audio sting + class-signature combat motif** zorunlu (`project_sfx_v2.md` ile uyumlu). Hitstop + screen shake + audio sting trio.

### Reject #5 — Quest UX Indicator Eksikligi
**HW2 hatasi:** "no quest indicators for NPCs" + "failing timed quests with no indication".
**RIMA icin:** Eger NPC quest layer eklenirse **HUD overlay decision** (`project_hud_overlay_decision.md` 3-layer LOCKED) ile uyumlu icon + countdown zorunlu. Quest sistemini koymadan once UX prototype.

---

## 12. LaurethStudio Universal Pattern

### Town hub + dungeon run yapisi her studio oyuna uyumlu mu?

**RIMA:** EVET — Sanctum + Rift Break zaten bu yapida (`project_rima.md`, `project_rift_break.md`).

**CircuitBreaker** (puzzle/action hybrid hipotezi — RIMA-disi):
- "Town hub" -> "Lab/Garage" karsiligi mantikli
- "Dungeon run" -> "Circuit puzzle level" karsiligi
- Town building -> permanent tool/gadget unlock
- AMA: Co-op multiplayer puzzle = farkli sorun (HW2 sikayetlerini incele)

**Caterpillar/Wingspan** (henuz scope'u net olmayan studio third project):
- Town hub + dungeon run yapisi UYUMSUZ olabilir (genre tahmini gerek)
- Eger arcade/casual ise: town hub overhead = scope creep
- Eger sim/management ise: town hub zaten core
- **Karar gerekli:** Caterpillar genre lock'u olmadan studio pattern kararlari premature

### Studio-level cikarim
HW2 incelemelerinden 3 universal Studio rule:

1. **Sequel reuse %50 ceiling.** HW2'nin "%85 reuse" sikayeti gosterge — bir oyun sequel/follow-up yapildiginda asset/sistem reuse %50 ustune cikmasin.
2. **Town hub progression visibility = retention silahi.** HW2'nin en pozitif quote'lari (Scrufington 139 hr, Unzeal 110 hr) town hub'a bagli. Studio her oyununda **persistent visible progression** garanti.
3. **Solo + Co-op hybrid scaling = test cost x3.** HW2 development effort'unun buyuk slice'i bu balance icin. Studio default "solo-first" + post-launch co-op scope tercihi safer.

### Universal patterns
- **Multi-phase element rotation boss** (Totem Pole pattern) = arcade'den ARPG'ye genel uygulanabilir
- **Floor-spesifik material drop** ekonomisi = her biome/level/world tabanli oyuna uyumlu
- **Blueprint discovery + town inşa** = roguelite/sim/strategy hybrid'e uygun
- **Free respec at hub** = decision-fatigue azaltma genel quality-of-life

---

## 13. Onerilen Sonraki Aksiyonlar

1. **Karar adayi #150 (Biome Roster Lock)** + **#151 (Sanctum Tier-Gate)** TASARIM/MEMORY veya CURRENT_STATUS'a tasi.
2. RIMA Rift Break spec revisesinde **4-layer progression separation** (per-run / convert / persistent / account) explicit yaz.
3. HW2 OP "fun sucked out" thread'i full read et ve RIMA combat juice checklist'ine **anti-pattern reference** olarak ekle.
4. Two Feathers OST karsilastirmasi: RIMA SFX/OST pipeline'i "combat sting + atmosphere" denge sablonu icin mock-up.
5. Studio-level: CircuitBreaker + Caterpillar genre lock'lari yapilana kadar town-hub pattern adoption ertelensin.

---

## Kaynaklar

- [Heroes of Hammerwatch II Steam page](https://store.steampowered.com/app/619820/Heroes_of_Hammerwatch_II/)
- [Steam top-rated reviews](https://steamcommunity.com/app/619820/reviews/?browsefilter=toprated)
- [TheSixthAxis review](https://www.thesixthaxis.com/2025/01/24/heroes-of-hammerwatch-2-review-roguelike-perfection/)
- [Rogueliker review](https://rogueliker.com/heroes-of-hammerwatch-ii-review/)
- [Superstar Reviews](https://superstarreviews.tech/heroes-of-hammerwatch-2/)
- ["Fun sucked out" Steam thread](https://steamcommunity.com/app/619820/discussions/0/777534074828887777/)
- [Music thread](https://steamcommunity.com/app/619820/discussions/0/777534074828885866/)
- [NG+ scaling thread](https://steamcommunity.com/app/619820/discussions/0/777534492476752390/)
- [Multiplayer scaling threads](https://steamcommunity.com/app/619820/discussions/0/603023006927725265/)
- [HoH2 Wiki — Buildings/Bosses/NG+](https://wiki.heroesofhammerwatch2.com/)
- [Two Feathers Studio (composer)](https://twofeathersstudio.com/project/hammerwatch/)
- [TheGamer beginner tips](https://www.thegamer.com/heroes-of-hammerwatch-2-beginner-tips-tricks-guide/)
- [GameRant beginner tips](https://gamerant.com/beginner-tips-heroes-hammerwatch-2/)
