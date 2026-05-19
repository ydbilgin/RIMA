---
name: pixellab-character-states-workflow
description: "RIMA-LOCAL — Character States workflow applied to RIMA's 10-class roster, 64×64 native, weapon-less. Karar"
metadata: 
  node_type: memory
  type: project
  originSessionId: 800899c1-572c-4f23-9c89-2fd4b064f5a0
---

# PixelLab Character States — RIMA Implementation

**Karar:** #145 LOCKED 2026-05-16 S86
**Studio-wide canonical source:** [[pixellab-character-states-animation-workflow]] (Lauret Studio global, do not duplicate workflow here — only RIMA specifics)

---

## RIMA-specific LOCKs

| RIMA LOCK | Value | Why |
|---|---|---|
| Native canvas | **64×64 square** | Karar #74 LIVE LOCK; **custom width 48×64 deneme YAPILMADI** (user kararı 2026-05-16) |
| PPU | **64** | Karar #100 |
| Camera angle | **High top-down 30-35°** | Karar #100 LIVE |
| Weapon convention | **Weapon-less body** | Karar #144 — character states-with weapon variant ÜRETMEZ. Weapons = Unity child SR |
| Mirror | **PixelLab Web UI mirror** | Karar #144 weapon child SR breaks under Unity flipX |
| Directions | **5 produce + 3 mirror** | S, SE, E, NE, N produce → SW, W, NW mirror |

---

## Pilot rollout (Karar #145)

| Phase | Class | Owner | Status |
|---|---|---|---|
| Pilot #1 | **Warblade** (heavy melee anchor) | USER (PixelLab Web UI V3) | 2026-05-16 starting |
| Pilot #2 | Ranger (medium ranged, simetrik silüet) | USER | Pilot #1 PASS sonrası |
| Pilot #3 | Shadowblade (agile assassin) | USER | Pilot #2 PASS sonrası |
| Pilot #4 | Elementalist (slim caster, orb pivot) | USER | Pilot #3 PASS sonrası |
| Rollout | Brawler + Gunslinger + Hexer + Cursemark + Necromancer + Frostborn | USER | 4 pilot PASS sonrası |

**Mid-walk anchor decision:** **Try-and-see** per class. Warblade test → kalite yüksekse pilot 4 için lock, düşükse idle→run animasyon fallback (no mid-walk state).

---

## Per-class state inventory (full rollout target)

Per class × per direction (S, SE, E, NE, N) state'ler:

| State | Purpose | Generated frames | Final anim frames |
|---|---|---|---|
| `idle_{dir}` | Idle 4f animation anchor | 3 | 4 |
| `midwalk_{dir}` (try) | Run 6f animation anchor | 5 | 6 |
| `attack_anticipation_{dir}` | Attack 3-seg anchor | 2 | 3 |
| `dash_lean_{dir}` (optional) | Dash 3f anchor | 2 | 3 |
| `hit_recoil_{dir}` | Hit 3f anchor | 2 | 3 |
| `death_start_{dir}` | Death 6-8f anchor | 5-7 | 6-8 |

**Per class state total:** 5 dir × 5-6 states = 25-30 states + V3 base = ~26-31 generations  
**Per class anim total:** 5 dir × 5 anim categories = 25 produced anims + 15 mirrored = **40 animations**  
**Per class credit estimate:** ~70-90 credits (per-state cost unknown, monitor in UI)

---

## 5 Use Cases applied to RIMA

### Use #1 — Animation anchor (PRIMARY, pilot scope)
Idle/Run/Attack/Dash/Hit/Death per class × 5 dir → first-frame-locked. Karar #144 weapon-less body — weapons added in Unity afterwards.

### Use #2 — Enemy variant matrix (DEFERRED to mob roster sprint)
Mob example workflow:
- Base: Bandit (existing Create Image Pro flow)
- State #1: "same bandit, heavy armor variant, helmet"
- State #2: "same bandit, elite captain, gold trim insignia"
- State #3: "same bandit, boss variant, large pauldron, red eye glow"
- Each variant inherits identity, only outfit changes
- Per variant → animation production with state-first

Applies to mob roster ([[mob-sprites]]): 8 mob × 3-4 variant = 24-32 visually distinct enemies from 8 base sprites.

### Use #3 — Boss multi-phase (DEFERRED to boss design)
Boss states per phase:
- `boss_idle_phase1`
- `boss_enraged_phase2` (visible damage)
- `boss_armor_cracked_phase3`
- `boss_on_fire_phase4` (final form / desperation)
- State-to-state interpolation for transition animations

### Use #4 — Class skin variants (DEFERRED to Rift Break meta)
Per class meta-progression tier outfit variants:
- Tier 1: base outfit
- Tier 2: minor variant (e.g., color shift, new accessory)
- Tier 3: major variant (rank insignia, refined gear)
- Tier 4: prestige variant (signature legendary outfit)

Aligns with [[rift-break]] meta progression tiers.

### Use #5 — State-to-state interpolation (PHASE 2 anim work)
- Death sequence: idle state + death_start state → AI fills falling motion
- Transformation: base state + rage_form state → AI fills transform sequence
- Stand-up after hit: hit_recoil state + idle state → AI fills recovery

Karar #47 (KF+Interp) is the **birinci-class realization** of this feature.

### Use #6 — Conditional Variant via Natural Language State Prompt (NEW S87 2026-05-18)

**User-discovered capability (concept proven — test ID'leri deprecate, yeni create character workflow ile gelir):**
- Existing anchor: mevcut canonical class anchor (PixelLab create character)
- Action: PixelLab Web UI → "create state" → natural language prompt: **"make him female"** (örnek)
- Result: identity-preserving variant of same anchor
- **Identity preserved:** kıyafet/silüet/palette canonical kalır; sadece prompt'taki specific element değişir

**Scope of this technique (BROAD — base'den herhangi bir element değişikliği):**
- **Gender swap** (male↔female) ✅ proven (Ravager)
- **Spesifik aksesuar:** "add a cracked helmet", "remove the cape", "add bone necklace"
- **Spesifik outfit element:** "change boots to greaves", "swap leather harness for chainmail"
- **Yara/durum:** "add facial scar across left cheek", "add burn mark on forearm", "blood splatter on chest"
- **Age variation:** "make him older with grey beard", "make her younger"
- **Ethnicity variation:** "change skin to dark", "change to pale"
- **Outfit color:** "change palette to frost-blue", "change palette to bloodied"
- **Body type:** "make him bulkier", "make her leaner"
- **Pose subtle:** "shift weight onto right foot", "raise left hand slightly"
- **Hair:** "longer hair", "shaved sides", "topknot tied higher"

**Anchor "base"den parçacık-seviye değişiklik** — yeni anchor üretmeden tek-state output ile manipülasyon. Use #2/#3/#4'ün tümünün **natural language wrapper'ı**.

### Gen Budget Update (S87 2026-05-18)

**5000 gen kredi yarın geliyor (2026-05-19).** Önceki "HIGH GEN COST sınırlı kullan" caveat'i **kaldırıldı** — anchor lock sonrası state workflow **serbestçe** kullanılabilir:

| İş | Gen tahmin | 5000 budget'ta yer |
|---|---|---|
| 10 sınıf full animation states (idle/run/attack/hit/death/parry × 5 dir) | ~300 gen | ✅ Çok rahat |
| Use #6 gender/variant swaps (1-2/sınıf) | 10-20 gen | ✅ Çok rahat |
| Sprint 17 Skin Pilot (3-4 variant full state set) | ~120 gen | ✅ Rahat |
| Boss multi-phase states (4 phase × 4 boss) | ~50 gen | ✅ Rahat |
| Mob variant matrix (8 mob × 3-4 variant) | ~50 gen | ✅ Rahat |
| **TOPLAM kullanım hedefi** | **~530 gen** | **10% of budget** |

**Aksiyon:** Anchor lock tamamlanınca state production **aggressive** yapılabilir — eskiden "sınırlı kullan" kuralı **REVOKED**.

**⚠️ COST CAVEAT (LOCK):**
- **Yüksek gen kredisi harcar** — her conditional variant ayrı state = full credit cost
- Use sparingly: 1-2 variant per anchor pilot, no broad-batch generation
- Diversity expansion için **ucuz çözüm DEĞİL** — sadece her anchor için sınırlı varyant üret

**RIMA implications:**
- **Sprint 17 Skin Pilot için yeni opsiyon:** Brawler Female canonical anchor yerine "make him female" state on (2,2) Brawler male anchor → instant variant
- **Phase 2 diversity batch:** Her sınıf için 1 alternate-gender variant state = 10 credit batch (kabul edilebilir scope)
- **Boss variant:** "make him enraged" / "make him cracked" — Use #3'ün natural language versiyonu, daha esnek

**Karar #145 v2 expansion:** This use case is **LOCK ADD** to Karar #145, 2026-05-18 S87. 5 use case → **6 use case**.

---

## Production specs

- **Spec file:** `STAGING/character_production_prompts.md` (Warblade section state-first revised 2026-05-16)
- **Anim contract:** `STAGING/animation_spec_weaponless.md` (Karar #144 spec — first-frame anchor convention to be added)
- **Pose contract:** §C of `character_production_prompts.md` — symmetric, weapon-less, both hands visible
- **Identity contract:** §B style + per-class identity block (no changes from current spec)

---

## Cross-links

- [[pixellab-character-states-animation-workflow]] — Lauret Studio canonical workflow source
- [[weaponless-animation-v1]] — Karar #144 weapon-less anim spec
- [[pixellab-character-via-web-ui-v3]] — User manual Web UI workflow (states inherit this LOCK)
- [[karar-143-layered-pipeline]] — Map layer system (not character but related rollout discipline)
- [[mob-sprites]] — Enemy variant matrix target
- [[rift-break]] — Class skin variant target
- [[room-library-architecture]] — Production cadence reference

---

## PASS criteria (per pilot class)

A pilot class is **PASS** when:
- [ ] Body identity preserved across 5 idle states (face, palette, silhouette stable)
- [ ] Face/mouth drift minimal across animation frames (<10% delta), Pixelorama cleanup viable
- [ ] All 5 directions read correctly (silhouette legibility)
- [ ] Run cycle smooth, no jarring frame jumps
- [ ] Attack 3-seg reads as wind-up → strike → follow-through
- [ ] Mirror W/SW/NW directions visually correct (no anchor flip artifacts)
- [ ] South direction usable after max 2× reroll
- [ ] Weapon-less hands consistent (no hallucinated weapons)
- [ ] Pixelorama cleanup pass <30 min per direction

FAIL on any item → diagnose: prompt revision, fallback to hand-crafted, or workflow tune.

---

## Status log

- **2026-05-16 S86:** Karar #145 LOCK, Warblade pilot starting (user PixelLab Web UI). Memory + spec docs in place.
