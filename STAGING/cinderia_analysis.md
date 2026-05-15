# Cinderia (Steam app 3214610) — RIMA + LaurethStudio Analizi

**Tarih:** 2026-05-16
**Analist:** Codex (Opus 4.7 / 1M context)
**Kaynaklar:** Steam page, Steam community reviews (top-rated), niche gamer, monstervine, gamersheroes, games.gg, gam3s.gg, cinderia.wiki, YouTube gameplay trailer
**Bag-lam:** RIMA (Karar #143 6-layer map mimarisi LIVE, 128px native res, 30-35 derece 3/4 ARPG view) + LaurethStudio (RIMA + CircuitBreaker + Caterpillar/Wingspan)

---

## 0. TL;DR (5 madde)

1. **Cinderia, "dark fairytale" tonunda 2.5D action roguelite** — Hades + Dead Cells'in kesisimi, ama "yanmis krallik / ash + black magic" temasi ile tonal olarak agir. Karar #143'un 6-layer goerselligine **ton referansi** olarak ideal: L4 (ground accents) + L5 (decals) icin "kuel + yanik leke" dili.
2. **Build-as-identity sistemi:** 4 karakter x 180+ skill x 500+ upgrade variant x 130+ equipment + Ember Fusion. Cinderia'nin kazanma sebebi "her run farkli oynanis hissi". RIMA cross-class skill sisteminde (80 skill, 2 slot) **fusion / synergy katmani** acigi gosteriyor.
3. **Erosion & Curse sistemi** (guclu skill kullanimi corruption meter doldurur, esik gecince permanent run curse) — RIMA Rage / Cursemark mekaniklerine **yapisal benzer pivot adayi**. Karar adayi #150.
4. **Visual clarity gerilimi:** Top reviewlerin ortak elestirisi "late-game visual clutter" ve "RNG item dagilimi". RIMA Karar #143 6-layer mimaride **L5 (decals) + L6 (overlay) yogunluk profili** icin uyari sinyali — density curve + readability lock gerekli.
5. **LaurethStudio universal lesson:** Cinderia "thematic weight" + "tight controls" kombinasyonuyla crowded roguelite pazarinda 96% Very Positive aldi. Studio icin formul: **(net mekanik kimlik) x (atmosferik ton dramatize) = discoverability**. Caterpillar'in "ekoloji + dort mevsim" temasi icin direkt transferli.

---

## 1. Oyun Genel Tanim

### Kunye
- **Adi:** Cinderia
- **Gelistiri:** MyACG Studio
- **Yayinci:** MyACG Studio + NPC Entertainment
- **Cikis:** 30 Mart 2026, **Steam Early Access** (~12 ay planli)
- **Fiyat:** 8.36 USD (10% indirimli, normal 9.29 USD) — 1.0'da fiyat artacak
- **Platform:** Sadece Windows
- **Diller:** 11 destekli dil

### Tur ve Kategori
- **Birincil tur:** Action Roguelite
- **Etiket havuzu:** Action, Adventure, Indie, RPG, Early Access
- **Alt-genre:** Build-craft / synergy roguelite (Hades + Dead Cells + Slay the Spire deck-craft hybridini cagrisitiriyor)

### Setting / Ton
- "Dying dark fairytale world" — Steam page acik attribution
- "World has already fallen, ancient powers are gone, kingdoms reduced to scorched remains" (niche gamer)
- "Ash and black magic" — civilization collapsed sonrasi sahnede gecen oyun
- Player rolu: "one of the few people capable of resisting the corruption spreading across the land, using a dangerous power tied to black magic without losing themselves to it"
- Mottolar: "weaponize the corruption", "break the limits"

### Visual Style
- **2.5D isometric perspective** (Steam page + games.gg confirm)
- "Atmospheric dark fantasy aesthetic"
- Pixel art **degil** — daha cok hand-painted / stylized 2.5D (RIMA'nin 128px native res yaklasiminin **disinda** bir damar)
- Dimensional depth + accessible side-scrolling action karisimi

### Reception (15 Mayis 2026 itibariyle)
- **English Reviews:** Very Positive (96% / 381 review)
- **Recent Reviews:** Very Positive (85% / 617 review)
- **Total Reviews:** ~2,767 (tum diller)
- 3.parti agregator: 941 pozitif / 149 negatif = 8.2/10
- "Best discovery of 2026" tonunda erken cover age — overhype riski mevcut

---

## 2. Visual Direction

### Steam Page'den Atif
> Steam page acik: "fast-paced action roguelite in a dying dark fairytale world"

> Gam3s.gg: "wrapping it in a heavier fairytale-inspired setting built around ash, black magic, and a world that has already burned down"

> Games.gg: "ability effects communicate their function through recognizable visual language"

### Karar #143 6-Layer Mimari Karsilastirmasi

| Cinderia Damar | RIMA L1-L6 Karsiligi | Borrow Notu |
| :--- | :--- | :--- |
| Yanmis taban / kul leke | **L1 base** + **L4 ground accents** | Ash overlay tile-set adayi |
| Black magic glow purple | **L6 overlay** (color grading + atmospheric) | Cold purple/blue accent — RIMA `project_vfx_production.md` ile uyumlu |
| 2.5D dimensional depth | RIMA: 30-35 derece 3/4 ARPG view (Camera LOCKED) | **Borrow ETME** — RIMA pivot disi; ama **shadow pass** Cinderia'nin volumetric hissinden ders alabilir |
| Boss visual telegraph clarity | Karar #143 L5 (decals) + L6 (UI overlay) | "Recognizable visual language" prensibi — VFX shape language disiplin |
| Late-game visual clutter (Nyanco review) | RIMA L5 density risk | **Anti-pattern** — density curve gerekli |

### Color Grammar Ipuclari
- Signature accent: **ember orange** (yangin sonu) + **void purple** (black magic) + **ash gray** (dunya)
- 3-color hierarchy modeli — RIMA `project_class_colors.md` ve PatchAtlas Moss density tartismasiyla **direkt karsilastirilabilir**
- "Dimensional clarity over detail" — yuksek kontrast, dusuk noise

### Camera / Perspective
- 2.5D isometric — RIMA pivotu degil
- Ama **silhouette-first character readability** prensibi RIMA 4-cardinal direction yaklasimiyla aligned

---

## 3. Combat / Mechanics

### Cekirdek Loop
> Steam page: "razor-sharp controls", "skill-based combat", "every fight is a test"

> Wiki: "Combat flows through basic attack chains, energy-spending active skills, and dash-triggered passive procs"

### Sistem Katmanlari (games.gg breakdown)
- **Active Skills:** 100+ — direct damage / utility
- **Passive Abilities:** 500+ upgrade variant — skill behavior modifier
- **Relics:** 130+ equipment — playstyle transformer
- **Ember Fusion:** skill kombinasyon mekanigi — synergy discovery driver
- **Spellcards:** spell-based active layer

### Combat Feel (review sentezi)
- Kheryo (27.1h): "snappy character response, fast movement, instant dashes"
- Nyanco (12.2h): "highly responsive, satisfying combat"
- Yayali (12h): "boss designs and visual presentation are outstanding"
- Tantalum (42.5h): "good skeleton for what's to come" — yuksek saat / iyimser ton

### Boss Design
- 5 chapter, ~8 boss (EA)
- Her boss "unique phases, attack patterns, counters"
- Adlandirilmis bosslar: **Black Knight** ("precise timing"), **Evil Mage** ("tactical flexibility")
- RIMA `project_combat_architecture.md` v4 combat icin **multi-phase boss telegraph** disiplini referansi

### Erosion & Curse (Wiki)
> "Powerful skills fill a corruption meter; exceeding threshold grants permanent run curses"

Bu, RIMA Cursemark sisteminin **risk/reward escalation** mekaniginin tam karsiligi. Karar adayi #150 icin guclu sinyal.

---

## 4. Map / World Design

### Sistem Tipi
- **Procedurally arranged encounters** (random rooms + events)
- "No two playthroughs feel identical"
- Run uzunlugu: **30-60 dakika** (Wiki)

### Hub / Meta
- **Nun's Shrine** — soulfire spend ile permanent upgrade
- 5 chapter struktur — chapter bazli unlock (ornek: Valkyrie Chapter 1 sonrasi, Forest Princess Chapter 2 boss sonrasi acilir)

### Discovery / Atmosphere
- "Dozens of random events and room types"
- Map yapisi STS2-tarzi degil — daha cok Dead Cells **branching biome** hissi (review tonundan inferene; **dogrulanmamis**)

### RIMA Karsilastirma
- RIMA STS2 DungeonMapUI 3-fork depth ile farkli paradigma
- Ama **room type variety** (RIMA 7 room type) Cinderia'nin "dozens of room types"undan az kalir — gozlem
- **Borrow:** chapter bazli karakter unlock — RIMA progression hook olarak deger

---

## 5. Audio

> Snapp_ review: "excellent soundtrack"

> Tantalum review: "enjoyable music"

- **DLC:** Cinderia Soundtrack ($2.99) — soundtrack ayri urun, **dev soundtrack'e ciddi yatirim yaptigi sinyali**
- Voice acting: review/sayfa atfi yok — muhtemelen **silent narrative** veya minimal VO
- Trailer: dark orchestral / ambient pad layer + percussive combat hits beklenir (trailer dogrudan izlenmedi, comment yok)

### RIMA Karsilastirma
- RIMA `project_sfx_pipeline.md` Stable Audio Open 1.0 + RTX 5080 pipeline ile soundtrack **uretilebilir**
- Cinderia'nin "OST as separate product" stratejisi — RIMA icin **Steam page'de OST bundle** Karar adayi

---

## 6. Storytelling / Narrative

### Aktarim Modu
- **Environmental + minimal text** (review/sayfa atfindan inferene)
- Cutscene varligi atfi yok
- Lore fragmentleri: "ancient powers are gone", "kingdoms reduced to scorched remains" — Dark Souls-vari **collapsed-world ambient lore**
- Karakter motivasyonu: "resist corruption without losing self" — moral struggle hooku

### RIMA Karsilastirma
- RIMA narrative onceligi dusuk (combat-first), Cinderia ayni hatta
- Caterpillar/Wingspan icin: **collapsed-world lore vs ecological-cycle lore** — kontrast olarak Studio dilini ayristirir

---

## 7. Steam Reviews Sentezi

### Pozitif Tema (cogul sira)
1. **Combat feel** — "snappy", "instant dashes", "razor-sharp" (Kheryo, Nyanco, Steam)
2. **Build variety** — "even with few unlocks" (Snapp_), 15+ active / 20+ passive per char
3. **Art / atmosphere** — "gorgeous dark fantasy aesthetic" (Nyanco), "outstanding visual presentation" (Yayali)
4. **Soundtrack** — "excellent" (Snapp_)
5. **Replayability / addictive loop** — multiple reviews

### Negatif Tema
1. **RNG item distribution** — multiple reviews
2. **Visual clutter** — "late-game" (Nyanco)
3. **Bazi text untranslated** (multi-language EA tahlili)
4. **Content depth EA limit** — "good skeleton for what's to come" (Tantalum 42.5h ile bile)

### Karsilastirma Pattern ("Compared to X")
- **Dead Cells** — Tantalum review explicit
- **Hades** — implicit (build-craft + boss-driven roguelite + 4 character)
- **Slay the Spire** — synergy/deck-build prensibi (implicit)

---

## 8. RIMA icin BORROW Listesi (8 madde, aksiyonlu)

### B1. Erosion & Curse Mekanigi
> Karar adayi #150: Cinderia Erosion + Curse sistemini RIMA Cursemark ile sentezle

- Mevcut RIMA: Cursemark idle pose (`project_class_identity_pivots_s43.md`)
- Cinderia: powerful skill kullanimi corruption meter doldurur, esik permanent run curse
- **Aksiyon:** Cursemark karakteri icin "skill spam = curse stack" loop prototipi (`Assets/Scripts/Systems/Combat/` icine yeni `CurseStackController.cs`)

### B2. Ember Fusion / Skill Synergy Layer
> Karar adayi #151: Cross-class skill slot uzerine "fusion" katmani

- Mevcut RIMA: `project_cross_class_skills.md` — 80 skill, 2 slot
- Cinderia: Ember Fusion skill kombinasyonu yeni effect uretir
- **Aksiyon:** 2-skill slot sahipligi -> belirli ciftler icin "fusion bonus" tetikleyici tasarim brief'i

### B3. Visual Clarity Density Curve
> Karar adayi #152: L5 (decals) + L6 (overlay) icin density profili kilidi

- Mevcut RIMA: Karar #143 6-layer LIVE
- Cinderia uyarisi: "late-game visual clutter" review-confirmed
- **Aksiyon:** `Assets/Scripts/Systems/Map/CornerWangPainter.cs` density alaninda max-cap + readability override flag

### B4. Boss Multi-Phase Telegraph Disiplini
> Karar adayi #153: Phase 2+ boss design icin "unique phases + counters" zorunlulugu

- Mevcut RIMA: combat v4 (`project_combat_architecture.md`)
- Cinderia: 8 boss / 5 chapter, her biri unique phases
- **Aksiyon:** Boss design template'ine 3-phase + 1-counter-window minimum sart eklenmesi

### B5. Chapter-Gated Character Unlock
> Karar adayi #154: 10 class icin chapter-progression unlock haritasi

- Mevcut RIMA: 10 class her birinin unlock-state'i belirsiz
- Cinderia: Valkyrie Ch1, Forest Princess Ch2 boss sonrasi
- **Aksiyon:** `project_rima_backlog.md` icine 10 class chapter unlock matrisi

### B6. Procedural Room Type Genisletme
> Karar adayi #155: 7 room type'i 12+ varyantla zenginlestir

- Mevcut RIMA: 7 room type (`project_room_design.md`)
- Cinderia: "dozens of room types"
- **Aksiyon:** `Assets/Scripts/MapDesigner/ProceduralRoomGenerator.cs` icine 5 yeni room archetype slot (event/shop/elite/treasure/lore)

### B7. OST DLC Bundle Steam Stratejisi
> Karar adayi #156: RIMA Steam page hazirligi icin OST ayri urun + bundle

- Cinderia OST $2.99 ayri DLC + bundle $10.21
- Mevcut RIMA: SFX pipeline mevcut (`project_sfx_pipeline.md`), soundtrack stratejisi yok
- **Aksiyon:** Phase 4 sonu Steam page icin OST production hedefi (~30 dk)

### B8. Color Grammar 3-Lock Disiplini
> Karar adayi #157: RIMA signature accent locki — ember orange + void purple + ash gray vs RIMA mevcut palette

- Cinderia: ember/void/ash 3-lock signature
- Mevcut RIMA: per-class color lock var (`project_class_colors.md`), ama global signature accent yok
- **Aksiyon:** Karar #143 ile birlikte 3-color global signature lock toplantisi

---

## 9. RIMA icin REJECT Listesi

### R1. 2.5D Isometric Perspective
- Cinderia 2.5D — RIMA Karar `project_camera_angle_revisit_s43.md` LOCKED 30-35 derece 3/4 ARPG view
- **Borrow ETME** — pivot disi
- Sadece **shadow volumetric hissi** prensip olarak alinabilir, perspective degil

### R2. Hand-Painted / Stylized Non-Pixel Art
- Cinderia pixel art degil — RIMA 128px native res LOCKED
- **Borrow ETME** — `project_128px_pivot_s43.md` ile celisir
- Sadece **ton + atmosfer + post-process color grading** prensip olarak alinabilir

### R3. 4-Karakter Hub Yapisi (sadece 4 oynanis)
- Cinderia: 4 karakter, RIMA: 10 class
- RIMA'nin 10 class scope'u Cinderia'dan farkli urun konumlamasi yapar
- **Daraltma ETME** — 10 class lock (`project_class_balance.md`) korunmali

### R4. 30-60 Dakika Run Uzunlugu Default
- Cinderia 30-60 dk — RIMA STS2 DungeonMapUI 3-fork yapisi farkli pacing
- **Direkt kopyalama ETME** — RIMA pacing modeli kendi STS2 templati ile dogrulanmali
- Karar: A/B test asamasinda referans olabilir, default lock degil

---

## 10. LaurethStudio Universal Pattern

### U1. "Thematic Weight x Tight Controls" Formulu
- Cinderia 96% Very Positive aldi cunku **(yanmis dunya tonu) x (snappy controls)** kombinasyonu **discoverability** uretti
- **Studio kurali:** Her oyun (RIMA, CircuitBreaker, Caterpillar) icin "1 cumlelik ton + 1 cumlelik mekanik kimligi" zorunlu
- Caterpillar icin: "ekoloji-cycle ton + tactical-zoom mekanigi" — Wingspan ile rekabette differentiation
- CircuitBreaker icin: "circuit-collapse ton + reactive-grid mekanigi" — su an muglak

### U2. Build-as-Identity Sistemi
- Cinderia 100+ skill x 500+ upgrade x 130+ equipment combinatorics ile **her run yeni hikaye** uretiyor
- **Studio kurali:** Her urunde "ucuncu run'a kadar yeni mekanik gor" prensibi (discovery curve)
- Caterpillar icin: card variety + season interaction layer
- CircuitBreaker icin: circuit module fusion modeli arastirilabilir

### U3. Early Access "Skeleton + Promise" Modeli
- Cinderia EA $8.36, "12 ay planli", 1.0'da fiyat artacak — community trust + price ladder
- 42.5 saat oynayan Tantalum bile "good skeleton for what's to come" diyor — bu **vaadin satilabilir oldugunu** gosteriyor
- **Studio kurali:** EA stratejisi standart hale gelmeli — RIMA Phase 4 sonu EA kararsiz, Cinderia template
- Roadmap publik + price ladder + community channel triadu (Discord + X + Bluesky)

### U4. OST as Standalone Asset
- Cinderia OST $2.99 + bundle — soundtrack productionu **revenue stream** olarak konumlandirilmis
- **Studio kurali:** Stable Audio Open pipeline (RIMA `project_sfx_pipeline.md`) tum Studio oyunlari icin OST production hatti uretmeli
- Caterpillar icin: ambient seasonal track set; CircuitBreaker icin: synth-grid track set

---

## Kaynaklar

- Steam page: https://store.steampowered.com/app/3214610/Cinderia/
- Steam Community Reviews (top-rated): https://steamcommunity.com/app/3214610/reviews/?browsefilter=toprated
- Niche Gamer launch coverage: https://nichegamer.com/cinderia-now-available/
- Monstervine launch coverage: https://monstervine.com/2026/03/cinderia-steam-early-access-launch/
- GamersHeroes coverage: https://www.gamersheroes.com/gaming-news/fast-paced-roguelite-cinderia-now-available-in-steam-early-access/
- Games.gg overview: https://games.gg/cinderia/
- Gam3s.gg overview: https://gam3s.gg/cinderia/
- Cinderia Wiki: https://cinderia.wiki/
- YouTube gameplay trailer: https://www.youtube.com/watch?v=QjOvCciZywM
- SteamDB charts: https://steamdb.info/app/3214610/charts/
- Bullet Haven (review sayfasi 200 dondu, icerik cekilemedi): https://bullethaven.com/review/cinderia

## Kaynak Yetersizligi Notlari

- **Bullet Haven review:** sayfa 200 dondu fakat icerik bos — atif yapilamadi
- **YouTube trailer:** description ve comment cekilemedi — trailer audio izleme/comment sentezi **yapilmadi**, sadece title atfi mevcut
- **cinderia.wiki:** ilk fetch 200 dondu (icerik mevcut), ama 2. fetch denenmedi — character/skill listesi **EA snapshot**, 1.0'da degisebilir
- Voice acting yokluk atfi **kanitli degil** — sayfa/review atfi yoktu, "atif yokluk = yok" cikarimi muhakeme ile yapildi
- 2.5D isometric atfi 2 kaynak (Steam page yorum/games.gg) — **dogrulandi**, ama tam screenshot inceleme yapilmadi
