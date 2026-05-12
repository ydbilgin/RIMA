---
status: LOCKED
faz: 1
tarih: 2026-05-13
ozet: "Kararlar #82+84 mob 3-tier skill ve staged budget"
---
# Mob/Boss Skill Expansion — Draft Decision (Opus)
**Tarih:** 2026-05-13 (S60)
**Status:** DRAFT for Codex review
**Trigger:** User 2026-05-12 — "moblar bosslar skillerini yap last epoch gibi oyunlardan esinlenebilirsin ama rimaya ozgu olmali. sadece buyuk olmasi yetmez skilleri olmali bi sey summonlamasi bi yerlerden bi seyler cikarip vurmasi gibi gibi bir suru varyasyon."

---

## Konu

Mevcut mob/boss skill set (Faz 1, NLM canonical):
- Shard Walker: Triple Shard + Fracture Burst (death AoE) — 2 skill
- Void Thrall: Void Pulse + Death Split (HalfThrall spawn) — 2 skill
- Penitent Bruiser: Anti-Heal Aura + Penitent Surge — 2 skill
- Chain Warden Echo: Triple Chain + Chain Pull — 2 skill
- Relic Caster: Summon Shardling + Aegis Mark — 2 skill
- Fracture Imp: Rift Lunge + Death Splatter — 2 skill
- Seam Crawler: Submerge + Burst Strike — 2 skill
- Twice-Born (Elite): shared damage + berserk on pair death — 2 mekanik
- Penitent Sovereign Boss: 3-phase (Chain Whip, Shackle Cast, Rift Tear, Chain Detonation, Sovereign's Wrath)

User istegi: daha fazla skill variation — summon, environmental, multi-source. Last Epoch esinli, RIMA ozgu.

---

## Proposed System: 3-Tier Skill Architecture Per Mob

Her mob 3 tier'da skill var:

| Tier | Adi | Kullanim | Notlar |
|------|-----|----------|--------|
| **T1 Core** | Signature behavior | Always active, mevcut mekanik | Tek mechanical lesson per mob — KEEP |
| **T2 Environmental** | Terrain/rift/seal etkilesimi | Per-arena, conditional | Yeni — Last Epoch esinli, RIMA lore-tied |
| **T3 Synergy** | Komposisyona bagli combo | Karsi mob aktif olursa | Yeni — pair-based emergence, mevcut anti-pattern listesinin pozitif tarafi |

---

## Mob T2+T3 Skill Library (8 Mob)

### M01 — Shard Walker (Ranged Caster)
- **T1 (KEEP):** Triple Shard fan + Fracture Burst death AoE
- **T2 — Crystal Bloom:** Her 8s, yere kristal node ekiyor (2.5s telegraph, telegraph sonra patlama 1.5m AoE 22 dmg). Zone control forcing. Lore: kirik kristal kalintilarinin yansimasi.
- **T3 — Cleric Catch (with Chain Warden):** Warden'in pull'u oyuncuyu Walker'in Triple Shard line'ina koyar. Walker pre-aim 0.5s. Yan etki: lethal positioning combo.

### M02 — Void Thrall (Splitter)
- **T1 (KEEP):** Void Pulse melee + Death Split (2 HalfThrall)
- **T2 — Tear Open:** 2s channel, kucuk rift portal acar; portal her 5s 1 HalfThrall spawn'lar (max 3 active). Portal 40 HP, destroyable. Lore: thrall'in icindeki void enerjisinin sizmasi.
- **T3 — Brood Watch (with Relic Caster):** Caster Aegis Mark verirse Thrall'a, Thrall split olmadan once 3s "shielded berserk" girer (+%50 speed). Risk: AoE'la oldurus daha pahalı.

### M03 — Penitent Bruiser (Bruiser)
- **T1 (KEEP):** Anti-Heal Aura (3m) + Penitent Surge
- **T2 — Self-Mortification:** HP <30%, kneels 1.5s, sonra 4m AoE (40 dmg, knockback). Eger survival (oyuncu kacarsa): +%40 damage 8s buff. Risk/reward kill timing.
- **T3 — Shared Pain (with Twice-Born):** Bruiser + Twice-Born ayni odadaysa, Bruiser'in aldigi hasarin %30'u Twice-Born'a yansir (vice versa). Focus fire prevention.

### M04 — Chain Warden Echo (Controller)
- **T1 (KEEP):** Triple Chain (slow fan) + Chain Pull (drag, dash-immune)
- **T2 — Anchor Chain:** Far wall/pillar'a zincirler kendini, immobile olur ama Triple Chain menzil x2 (12m). Anchor node 50 HP, destroyable. Lore: hapishane zincirlerinin mekan-tasi metafora.
- **T3 — Lockdown Net (with Seam Crawler):** Crawler submerge altta, Warden ust pull (combo): oyuncu pull edilince Crawler ground-burst (dash-immune pull + dash-immune burst = forced damage). v1 OG anti-pattern listesinden retroaktif sinerji.

### M05 — Relic Caster (Support)
- **T1 (KEEP):** Summon Shardling (15 HP minion) + Aegis Mark (ally shield)
- **T2 — Seal Repair Channel:** 3s channel, en zayif allied mob'a %50 HP heal. Telegraph: gold beam between caster+target. Priority kill prompt (channel during).
- **T3 — Echo Choir (with another Caster):** 2 Caster'in cluster — biri channel basasinda obur "Echo Mark" verir (mark'li skill 50% daha hizli). Anti-pattern (eski: 2 Caster yasak); yeni T3 ile YENI anti-pattern tipi (cluster control).

### M06 — Fracture Imp (Swarm)
- **T1 (KEEP):** Rift Lunge + Death Splatter (rift goo slow)
- **T2 — Imp Tide:** 3+ imp 4s icinde olunce, nearby rift crack'tan 4. wave (3 imp) spawn. Lore: catlaktan surekli sizma.
- **T3 — Goo Web (with Seam Crawler):** Imp death splatter + Crawler seam lines (T2'ye baglanir) — overlap zone 2s slow + 8 dmg/s. Pathing lock.

### M07 — Seam Crawler (Skirmisher)
- **T1 (KEEP):** Submerge (50% hidden) + Burst Strike (ground emerge)
- **T2 — Subterranean Web:** Submerge boyunca yerde "seam lines" iz birakir (gorunur catlak). Lines uzerinde yuruyen oyuncu 5 dmg/s + 30% slow. 3s sonra fade. Lore: yerin altinda gezerken yararlarin acmasi.
- **T3 — Pincer (with Fracture Imp):** Imp swarm pre-position, Crawler other side burst — sandwich. Imp count 4+ olursa T3 trigger.

### M08 — Twice-Born (Elite Pair, Faz 2)
- **T1 (KEEP):** Shared damage %50 + berserk on partner death
- **T2 — Mirror Step:** Her 6s, her iki Born birbirinin pozisyonuna teleport (0.5s telegraph, takip edemezse oyuncu shuffled). Lore: iki yuzun yer degistirmesi.
- **T3 — Twin Bind (paired with self):** Her iki Born allied mob'lara (1 yakin her birine) "Bind Aura" verir — bind alanindaki mob'lar her 4s Twice-Born'a hasarin %20'sini "lend" eder. Effective HP +%40. Focus fire still works ama %40 daha pahalı.

---

## Boss T2 Environmental Hazard Per-Phase

### Penitent Sovereign — 3 Phase + Environmental Per Phase

**Phase 1 (100->66 HP) — "Zincirin Altinda":**
- T1: Chain Whip + Penitent Surge + Shackle Cast (KEEP from NLM)
- **T2 ENV — Litany of Restraint:** 4 chain anchor pusula koselerinde. Chain line'lari arasi gecmek = 1.5s slow + 15 dmg. Anchors destroyable (40 HP each). Lore: kendi disiplinin metaforu.

**Phase 2 (66->33 HP) — "Kirilan Zincir":**
- T1: Rift Tear (KEEP) + Chain Detonation + 3x Fracture Strike combo
- **T2 ENV — Rift Bloom:** Her 8s yere yeni void crack acar (telegraph 1s). Crack'in uzerinde durmak 25 dmg/s + 30% slow. Arena gradually narrowing. Lore: kirilmis muhrun yararlanmasi.

**Phase 3 (33->0 HP) — "Sovereign Awakened":**
- T1: Fracture Charge (KEEP) + Sovereign's Wrath (KEEP, safe zone follow)
- **T2 ENV — Echo Phantom Summon:** Her 10s "Chain Warden Echo Phantom" cagirir (50 HP, 12s lifespan). Phantom Triple Chain + Chain Pull. 3 phantom max active. Lore: muhur tamamen kirilinca eski muhafizlarin yankisi geri donüyor.

**Rift Break (Karar #22-23 LOCKED) hala uygulanabilir:** Faz gecisleri slow-mo interactive (kara duel).

### Future Bosses (3-fazli formul, henuz tasarlanacak)

- **Echo Twin (F2, 2-phase):** Mirror summons + arena split hazard
- **Fracture Sovereign (F3-4, 3-phase):** Reality fracture + zone-of-rift environmental
- **The Architect (Final, 4-phase):** World-control mekanik — environmental olarak ARENAYI manipule eder (parallax katmanlari hareket ediyor, Open Vista'dan benefit alir)

---

## Lore Cohesion (User'in "RIMA'ya ozgu" istegi)

Her skill RIMA core lore'a baglanir:
- **The Fracturing operasyonu** = mob'larin "boyun egmis muhur muhafizlari" olmasi
- **Rift March** = environmental hazards (rift cracks, void cracks, world bleed)
- **Seal/Muhur metaforu** = chain, anchor, shackle, sigil temalari
- **Self-discipline kirilmasi** = boss faz gecisleri (Penitent Sovereign 1→2→3)
- **World-merging** = environmental skill'ler "icinden geliyor" — yer altindan, duvar catlaklarindan, rift portallardan

Mekanik diversification:
- Summon: 4 mob T2 var (Walker Crystal Bloom = sub-summon, Thrall Tear Open, Caster Seal Repair, Imp Tide)
- Environmental: 8/8 mob T2 environmental etkilesim
- Multi-source: 5 T3 sinerji combo
- Bossun environmental hazard per phase: 12 total (4 boss x 3 phase)

---

## Yeni LOCKED Karar Onerisi

### Karar #82 — Mob 3-Tier Skill System
> Her mob 3-tier skill: T1 Core (mevcut signature), T2 Environmental (terrain/rift/seal etkilesim, lore-tied), T3 Synergy (pair-conditional combo). Boss her phase 1 environmental hazard. v1 scope: 8 mob x 3 tier = 24 unique skill variation + 4 boss x 3 phase x 1 env = 12 hazard. Detay: `TASARIM/MOB_BOSS_SKILL_EXPANSION_2026-05-13.md`.

---

## Production Impact

**Implementation (Codex/dev scope):**
- T2 environmental skiller mevcut mob behavior'larina ek state machine (ScriptableObject driven)
- T3 synergy = MobComposition rule check at room spawn — eger pair active, T3 active
- Boss environmental hazards = arena prefab'a baglanir (existing Rift Tear gibi)

**Asset impact:**
- Yeni VFX: Crystal Bloom (kristal explosion), Tear Open (rift portal), Anchor Chain (chain stretch), Echo Phantom (translucent warden silhouette), Mirror Step (teleport flash), Seam Lines (ground crack), Bind Aura (red tether), Litany Chains (compass-anchor chains), Rift Bloom (void crack expanding), etc. — ~12-15 yeni VFX 64-128px range
- Mob anim: T2 channel/cast anim per mob (1-2 anim ek/mob = 8-12 anim toplam)
- Sound: T2/T3 her trigger SFX

**Test impact:**
- Mob behavior tree expansion (state per tier)
- Mob composition pair detection (T3 trigger)
- Boss env hazard timer + telegraph system

**Faz allocation:**
- T1+T2 = Faz 1-2 scope (mob roster paralel)
- T3 + Boss env = Faz 3 scope
- Final Boss "world-control" = Faz 5

---

## Riskler ve Mitigasyon

| Risk | Mitigasyon |
|------|------------|
| Mob overload (3 skill + sinerji = combat readability bozar?) | Telegraph zorunlu T2/T3 (min 0.6s tell), max 1 T3 active per room |
| Boss env hazard + skill combo overload | Phase environmental + skill window'lar overlap etmemeli (timing matrix) |
| Synergy unlock'lari spawn-rule complexity'i | MobCompositionRules.md (mevcut dosya) genisletilir, deterministic spawn-check |
| VFX uretim cost (~12-15 yeni VFX) | Faz 1-2 production phase, asset budget icinde |
| Player learning curve | Per-room composition: max 1 T2 skill aktif baslangicta, T3 sadece elite/boss odalarinda |

---

## Sonraki Adimlar (Codex review sonrasi)

1. Karar #82 LOCKED → MASTER_KARAR_BELGESI'ne ekle.
2. TASARIM/MOB_BOSS_SKILL_EXPANSION_2026-05-13.md (yeni dosya — bu draftin final hali).
3. MOB_COMPOSITION_RULES.md UPDATE — T3 synergy trigger conditions.
4. Faz 1-2 mob behavior tree expansion (Codex dispatch — sonraki sprint).
5. PixelLab brief: 12-15 yeni VFX 64-128px (Faz 2 batch).
6. Test plan: mob T2/T3 trigger E2E.

