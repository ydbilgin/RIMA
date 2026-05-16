# MEKANİK BANKASI → RIMA Adaptation Proposal
**Date:** 2026-05-17 S87 night
**Source:** `F:\LaurethStudio\03_IDEAS\MECHANIC_BANK\_MEKANIK_BANKASI.md` (58 mekanik, 9 kategori, Cozy/Heirloom/Anadolu/Tiny pillar odaklı)
**Target:** RIMA — 2D top-down chibi roguelite, 10 sınıf × ~80 skill, room-based dungeon, combat-focused

---

## Özet

Bankadaki 58 mekanik çoğunlukla **farming / cozy management / restoration / mystery / craftling** odaklı — RIMA'nın combat-roguelite kimliğine doğrudan transfer çoğu için anlamsız (M25 Kontrat, M30 Trade, M31 Kirli-Temiz, M34 Habitat, M41 Festival, M51 Diorama, vb).

Ancak bazı mekaniklerin **soyut çekirdeği** (adjacency synergy, helper AI, tier upgrade, swarm logic, weapon morphology) RIMA combat-roguelite çerçevesine **çevrilmesi** mantıklı. Aşağıda 3 tier halinde RIMA-adapte önerileri:

---

## Tier S — Mevcut Designda Zaten Var veya Anında Eklenebilir

### S1 — M01 Komşuluk Synergy → Skill Adjacency Synergy
RIMA'da 80 skill, 2 slot mevcut. **Adaptation:** Skill A + Skill B aynı loadout'taysa bonus efekt (ör. "Burst + Ignite = Inferno"). Cross-class skill design'da [[cross-class-skills]] memory'de zaten "ghost VFX, 2 slot" var; M01 bu sistemin ADJACENCY tarafını formalize eder. Skill grid V2'de 4-slot grid + adjacency bonus.

**Effort:** Low — mevcut skill sistemine `synergyPairs` ScriptableObject ekle.
**Phase:** Phase 2 (Combat Polish).

### S2 — M11 Junimo Helper + M12 Pikmin Grup + M14 Specialist (4-6 tip) → Summoner Class Expansion
RIMA'da **Summoner** sınıfı zaten ghost summon + autonomous attack ile bu mekaniği taşıyor. **Adaptation:** Summoner'a 4 specialist tip ekle:
- **Attacker** (yakın dövüş ghost)
- **Tank** (player'a vuruşları çeker)
- **Debuffer** (slow/poison)
- **Healer** (player HP regen)

Memory [[ghost-attack-system]] (12 frames ghost sprite) zaten temel oluşturmuş.

**Effort:** Medium — 3 yeni ghost AI behavior tree + sprite varyasyonları.
**Phase:** Phase 2.

### S3 — M19 Tier Upgrade Path → Skill Tier Ascension
**Adaptation:** Her skill 3 tier (T1/T2/T3). Tier-up requires Rift Break meta currency (memory [[rift-break]]). T2 buff stat, T3 unlock yeni efekt veya cap raise.

Stardew/Hades pattern. RIMA'da memory [[class-balance]] Build vs Boss matrix'ine kolay map'lenir.

**Effort:** Low — skill SO'lara `tier` field + meta-progression UI panel.
**Phase:** Phase 3 (Meta).

### S4 — M20 Modüler Tool → Weapon Component Swap (Karar #144 ile MUKEMMEL FIT)
RIMA'da **Karar #144** silahsız body + WeaponSR child SR LIVE. **Adaptation:** Weapon = 3 component (head + handle + aura). Player runtime'da swap eder.

Slime Rancher Vacpack mantığı + Subnautica vehicle module. RIMA için:
- **Head** = damage type (slash/pierce/blunt)
- **Handle** = swing animation (fast/heavy/charge)
- **Aura** = passive bonus (life-steal / crit / fire damage)

10 sınıf × 5 weapon × 3 component slot × 4 option = **600 combo** runtime. Karar #144 zaten child SR ayrımı yapıyor, görsel hazırlığı kolay.

**Effort:** Medium — WeaponSO'ya 3 component slot + UI swap menüsü.
**Phase:** Phase 2 (Build Diversity).

### S5 — M22 Event-Driven Combo → Skill Combo System
Magicka spell mix + Hades cast+attack. **Adaptation:** İki skill birbirini takip ettiğinde X saniye içinde bonus damage / unique effect.

Memory [[combat-feel-research-combined]] Bandit Knight + ReBlade tuning matrix'inde dolaylı olarak combat feel için var. Combo trigger formalize edilebilir.

**Effort:** Low — combo detection state machine.
**Phase:** Phase 2.

### S6 — M55 Boids Swarm → Swarm Mob AI
**Adaptation:** Belirli mob türleri (Shard Walker, Spire Choirling — memory [[mob-sprites]] mevcut) sürü AI ile hareket eder. Pikmin/Reynolds Boids local rules → flocking emergent pattern.

Visual amplifier — yeni asset değil, mevcut mobların behavior değişikliği.

**Effort:** Medium — Boids behavior script + mob AI integration.
**Phase:** Phase 2 (Mob Variety).

---

## Tier A — Phase 2-3 Backlog Önerisi

### A1 — M02 Mutasyon Komşuluk → Skill Mutation
N run sonrası belirli skill combo'su 3. yan etki unlock eder (mutation). Procedural skill discovery.

**Phase:** Phase 3+ (meta-discovery layer).

### A2 — M07 Mevsim Buff Tile → Biome Affinity Skill
Her biome (ShatteredKeep / CrystalCaves / WhisperingForest / vb) belirli skill kategorilerine pasif buff verir. Memory [[room-design]] biome architecture'a entegre.

**Phase:** Phase 2.

### A3 — M24 Decoupled Skill Tree → Meta Dual Tree
Rift Break meta-progression iki branch: passive (always-on) vs active (loadout-selectable). Hades mirror benzeri. Memory [[rift-break]] zaten meta unlock yapıyor; M24 yapıyı formalize eder.

**Phase:** Phase 3.

### A4 — M42 Aile Yadigarı → Heirloom Token Run-Persistent
Her başarılı run'da 1 token kazanılır → ileride run'larda kullanılır. Hades darkness pattern. Memory [[rift-break]] tier unlocks'la birleştirilebilir.

**Phase:** Phase 3 (Meta layer).

### A5 — M50 Hava Durumu → Room Weather Modifier
Bazı odalarda hava (storm/fog/rain) gameplay efekti yapar (slowed movement, reduced visibility). Mob spawn pool ona göre değişir.

**Phase:** Phase 2 (Room Variety).

### A6 — M52 Rüzgar Yön → Projectile Wind
Ranger/Gunslinger sınıfları için per-room wind direction modifier — projectile yön etkisi. Memory [[combat-feel-research-combined]] juice-feature olarak iyi.

**Phase:** Phase 2.

### A7 — M58 Shape-as-Verb → Weapon Stance/Style Swap
Aynı weapon, farklı stance: slash / thrust / sweep. Real-time swap. Memory [[combat-feel-research-combined]] ReBlade benzeri.

**Phase:** Phase 2.

### A8 — M57 Density Topology → AOE Spread Tradeoff
AOE skills için "spread vs focused" toggle. Geniş yay = düşük damage, dar = yüksek damage. Common roguelite pattern, build diversity artırır.

**Phase:** Phase 2.

---

## Tier B — İlginç ama Karmaşık (Phase 4+ Backlog)

- **M05 Element Combo** — Elementalist sınıfı için fire+water=steam shock benzeri compound spell sistemi. Magicka pattern. Deep dive gerekiyor.
- **M09 Pressure/Flow Sim** — Rage system flow ve adjacent enemy bulaşma. Memory [[combat-architecture]] v4 rage var; pressure ekleme deep refactor.
- **M10 Time-Phase Adjacency** — Run cycle phase (early/mid/late) boon pool değişimi. Hades zaten pattern.
- **M16 İhtiyaç-Driven Behavior** — Enemy fatigue/morale stat. Complex AI.
- **M44 Genetik Tohum Seçimi → Skill Genetic Crossbreed** — Procedural skill generation; 2 parent skill → child skill mix. Out-of-scope V1.

---

## RIMA-ANCHORED YENİ MEKANİKLER (Bankaya Append)

Aşağıdaki 10 mekanik RIMA'nın combat-roguelite cephesi tarafından bankaya katkı önerisi. Hepsi roguelite klasiklerinden, mekanik bankasında henüz formalize edilmemiş.

### M59 — Combo Window Chain ⚔️

Saldırı serisi belirli zaman penceresinde devam ederse bonus damage / unique animation tetiklenir. **Klasik:** Hades attack+special+cast combos, Devil May Cry style ranking, Nine Sols parry chain. **Stüdyo entegrasyon:** RIMA combat polish — silah swap stance ile kombine. **Chill 6/10 · AI 7/10 · Complexity MED.**

### M60 — Parry/Riposte Window 🛡️

Düşman saldırısının başlangıcında doğru zamanda block = counter-damage + stun. **Klasik:** Sekiro deflect, Soulslike parry, Hades Aphrodite weapon. **Stüdyo entegrasyon:** RIMA defansif sınıflar (Warblade / Ronin) için skill cap. **Chill 5/10 · AI 7/10 · Complexity MED.**

### M61 — Dash i-Frame Window 💨

Dash sırasında belirli süre invulnerability. **Klasik:** Hades dash, Dark Souls roll, Hollow Knight dash. **Stüdyo entegrasyon:** RIMA tüm sınıflar için temel mekanik (memory [[combat-architecture]] dash v2). **Chill 7/10 · AI 8/10 · Complexity LOW.**

### M62 — Status Effect Layering 🔥💀

Birden fazla DoT stack edilir; belirli kombinasyonlar yeni status üretir (burn + bleed = decay). **Klasik:** Path of Exile ailment, Hades curse stack, ARPG genel. **Stüdyo entegrasyon:** RIMA Hexer + Elementalist sınıfları için skill cap. **Chill 7/10 · AI 8/10 · Complexity MED.**

### M63 — Boss Phase Transition 👹

Boss HP eşiklerinde farklı saldırı setine geçer; HP%75/50/25 cap'leri. **Klasik:** Hades boss phases, Hollow Knight Watcher Knights, Soulslike standart. **Stüdyo entegrasyon:** RIMA Penitent Sovereign vb boss tasarımı. Memory [[room-design]] boss-room patterns. **Chill 5/10 · AI 8/10 · Complexity MED.**

### M64 — Hitstop / Slow-mo Feedback 💥

Vuruş anında 50-200ms frame freeze, sonra slow-mo. **Klasik:** Hades hitstop, Hollow Knight crit pause, action game feel staple. **Stüdyo entegrasyon:** RIMA Phase 2 polish (memory [[combat-feel-research-combined]]). **Chill 9/10 · AI 9/10 · Complexity LOW.**

### M65 — Encounter Choice Branching 🛤️

Pre-room player choice: "harder + better reward" vs "easier + nothing". **Klasik:** Slay the Spire path, Hades door icons, Darkest Dungeon. **Stüdyo entegrasyon:** RIMA map system (memory [[map-system]] STS2 DungeonMapUI 3-fork depth). Doğrudan map'leniyor. **Chill 8/10 · AI 9/10 · Complexity LOW.**

### M66 — Meta-Currency Run Earn 💎

Premium currency sadece başarılı run veya specific achievement'tan kazanılır. **Klasik:** Hades darkness vs nectar, Slay the Spire keys, Dead Cells cells/scrolls. **Stüdyo entegrasyon:** RIMA Rift Break meta-progression (memory [[rift-break]] tier unlocks). **Chill 8/10 · AI 9/10 · Complexity MED.**

### M67 — Death Echo / Ghost Trail 👻

Önceki run'ın ölüm noktasında ghost-marker veya echo gözükür. Player bu marker'dan loot çıkarabilir veya tehlike öğrenir. **Klasik:** Spelunky 2 ghost runs, Dark Souls bloodstains, Death's Door. **Stüdyo entegrasyon:** RIMA run-persistent meta layer. **Chill 7/10 · AI 8/10 · Complexity MED.**

### M68 — Build Synergy Detection UI 🎯

Player'ın aktif loadout'unda named archetype (3+ synergistic skill) tespit edilir, UI bildirir ("Pyromancer Triad Active +X%"). **Klasik:** Slay the Spire deck archetype, Hades aspect of, Path of Exile gem hubs. **Stüdyo entegrasyon:** RIMA cross-class skill system + S1 adaptation (M01 Skill Adjacency). **Chill 7/10 · AI 10/10 · Complexity MED.**

---

## RIMA Phase 2-3 Aksiyon Önerisi

| Sprint | Tier | Mechanic ID | Status |
|---|---|---|---|
| Sprint 14 (Combat polish) | S1 | Skill Adjacency Synergy + M68 Build Detection UI | Add to Sprint 14 spec |
| Sprint 14 | S4 | Weapon Component Swap (Karar #144 extension) | Karar #146 candidate |
| Sprint 15 (Mob expand) | S6 | Boids Swarm AI for Shard Walker/Spire Choirling | Add to Sprint 15 spec |
| Sprint 15 | S2 | Summoner Specialist 4-tip | Memory [[ghost-attack-system]] extension |
| Sprint 16 (Build polish) | S3 + A3 | Skill Tier + Dual Tree meta | Phase 3 backlog |
| Sprint 17 (Room variety) | A5 + A6 | Weather Modifier + Wind Direction | Phase 2-3 |
| Sprint 18+ (Run-persistence) | A4 + M66 + M67 | Heirloom Token + Meta Currency + Death Echo | Phase 3 meta layer |
| Phase 2 polish | M64 + M61 + M59 | Hitstop + i-Frame + Combo Window | Memory [[combat-feel-research-combined]] LIVE candidates |

---

## Sonuç

- **58 mekanikten 6 tier-S** doğrudan RIMA design'a uyuyor — çoğu zaten partially designda.
- **8 tier-A** Phase 2-3 backlog candidate, çoğu memory bağıyla.
- **5 tier-B** complex / out-of-scope.
- **39 mekanik** RIMA için irrelevant (cozy / farming / mystery / restoration).
- **10 yeni mekanik** (M59-M68) RIMA combat-roguelite tarafından bankaya katkı.

Master listenin sonuna M59-M68 ekleme yapıldı (`_MEKANIK_BANKASI.md` append).
