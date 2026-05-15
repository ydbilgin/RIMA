# Class Skill Gap Analysis — S85 (2026-05-16)

**Type:** Audit + design proposal for user approval
**Status:** Awaiting user review (sabah onayı)
**Source:** `Assets/Scripts/Skills/` Glob + memory analysis (project_class_balance, project_cross_class_skills, project_class_colors, project_class_identity_pivots_s43)
**NLM status:** Auth expired (Chrome login gerek — kullanıcı uyuyor); analiz tamamen yerel memory + kod tabanlı

---

## 1. Implementation Status — 10 Classes

| # | Class | Color Palette | .cs Skill Count | .asset (Main) | .asset (CCS) | Status |
|---|---|---|---|---|---|---|
| 1 | **Warblade** | Dark Gray / Cold Blue blade | **14** | 7 | 1 (IronFragment) | 🟢 LIVE |
| 2 | **Shadowblade** | Midnight Blue | **22** | 0 | 1 (VoidTrace) | 🟡 Code complete, no .asset |
| 3 | **Elementalist** | Deep Indigo / Amber orb | **13+** | 0 | 1 (EmberTouch) | 🟡 Code complete, no .asset |
| 4 | **Ranger** | Forest Green / Olive | **18** | 0 | 1 (HuntersMark) | 🟡 Code complete, no .asset |
| 5 | **Brawler** | Burnt Orange / Void Purple knuckles | **0** | 0 | 1 (MomentumBurst) | 🔴 EKSİK |
| 6 | **Ravager** | Brown Fur | **0** | 0 | 1 (Bloodfuel) | 🔴 EKSİK |
| 7 | **Ronin** | Sage Green / Silver-Blue blade | **0** | 0 | 1 (FirstBlood) | 🔴 EKSİK |
| 8 | **Gunslinger** | Burgundy / Dark Red | **0** | 0 | 1 (QuickReload) | 🔴 EKSİK |
| 9 | **Hexer** | Dark Crimson / Green-Purple lantern | **0** | 0 | 1 (HexLeech) | 🔴 EKSİK |
| 10 | **Summoner** | Black / Bone White / Cold Blue crystal | **0** | 0 | 1 (GravePact) | 🔴 EKSİK |

**Total skill .asset hedef:** 10 class × 8 = 80 skill pool (per `project_cross_class_skills` memory)
**Total skill .asset şu an:** 7 main + 10 cross-class = 17 / 80
**Eksik:** 63 main skill .asset

---

## 2. Priority Order (per `project_class_balance` simulator v3)

**Tier S (balance priority — implement first):**
- Brawler (Charge banking fork mechanic)
- Ronin (First Blood resonance)
- Warblade (✓ already done)

**Tier A/B (after S):**
- Shadowblade (✓ code done, needs .asset)
- Gunslinger (Late-game scaling issues)
- Hexer

**Tier C (last, complex mechanics):**
- Ravager (Carnage kill chain + V Burst Kan Siklozu redesign — complex)
- Summoner (Grave Pact + minion mgmt)
- Elementalist (✓ code done, needs .asset; Light synthesis post-pivot)
- Ranger (✓ code done, needs .asset)

---

## 3. Per-Class Skill Design Proposals (8 each, from memory cues)

### 3.1 Brawler — Burnt Orange / Void Purple — Tier S

Mechanic theme: **Charge banking + Combo escalation + Momentum** (per `project_class_balance` "Charge banking fork")

| # | Skill | Type | Mechanic Sketch |
|---|---|---|---|
| 1 | Knuckle Slam | Basic | Single-target heavy punch, builds Charge (1) |
| 2 | Triple Jab | Basic Combo | 3-hit chain, each adds Charge (3 total) |
| 3 | Momentum Burst | Cross (✓ exists) | Spend all Charge → forward dash + AoE knockback |
| 4 | Banked Strike | Charge-spender | Consumes Charge stacks for scaling damage (×Charge) |
| 5 | Void Counter | Reactive | After taking hit, next melee = guaranteed crit |
| 6 | Charge Bank | Passive | Charge stacks decay 50% slower |
| 7 | Concussion Hook | Spend 3 Charge | Stun + push, low cooldown |
| 8 | Final Frenzy | Ultimate | 8s rapid auto-attack, every hit at max Charge damage |

### 3.2 Ronin — Sage Green / Silver-Blue blade — Tier S

Mechanic theme: **First Blood + Single decisive strike + Honor stance** (per `CCS_Ronin_FirstBlood`)

| # | Skill | Type | Mechanic Sketch |
|---|---|---|---|
| 1 | Iaijutsu Draw | Basic | Slow wind-up, massive damage burst, then sheathe |
| 2 | Kiri Step | Mobility | Short dash that passes through enemy, leaving slash trail |
| 3 | First Blood | Cross (✓ exists) | First hit on full-HP enemy deals +200% damage |
| 4 | Bushido Stance | Toggle | While stationary 2s+, next attack guaranteed crit + heal 10% |
| 5 | Crescent Cleave | Wide arc | 180° front sweep, lower damage but multi-target |
| 6 | Honor Riposte | Reactive | Block-counter window 0.3s after enemy hit |
| 7 | Patience Mark | Passive | Damage taken in last 3s converts to next-attack bonus |
| 8 | Final Severance | Ultimate | Mark up to 5 enemies; teleport-strike each |

### 3.3 Ravager — Brown Fur — Tier C (complex)

Mechanic theme: **Carnage kill chain + V Burst (Kan Siklozu — blood cycle)** (per `project_class_balance`)

| # | Skill | Type | Mechanic Sketch |
|---|---|---|---|
| 1 | Claw Rip | Basic | Bleed DoT, builds Carnage on kill (1 stack) |
| 2 | Frenzy Pounce | Mobility | Leap to target, reset cooldowns on kill |
| 3 | Bloodfuel | Cross (✓ exists) | Low HP → +damage scaling |
| 4 | Carnage Chain | Passive | Each kill within 3s = +5% damage + heal 2% HP |
| 5 | Sunder Maw | Heavy | Single target bleed amplifier (+200% bleed damage) |
| 6 | Wild Charge | Mobility+Damage | Charge through enemy line, knockup all |
| 7 | Kan Siklozu (V Burst) | Ultimate | While V meter full: 5s berserk, infinite stam, +50% atk speed |
| 8 | Trophy Hunt | Passive | Boss/elite kills grant permanent +1 max HP (run-scope) |

### 3.4 Gunslinger — Burgundy / Dark Red — Tier A

Mechanic theme: **Reload windows + Brass/precision + Multi-shot** (per `CCS_Gunslinger_QuickReload`)

| # | Skill | Type | Mechanic Sketch |
|---|---|---|---|
| 1 | Hip Shot | Basic | Single-shot, fast, low damage |
| 2 | Fan the Hammer | Combo | 6 rapid shots, then forced reload (1.2s window) |
| 3 | Quick Reload | Cross (✓ exists) | Reload cancel via timed input → instant skip |
| 4 | Marksman Aim | Channel | 1s aim → guaranteed crit + pierce |
| 5 | Ricochet Round | Active | Bullet bounces 3 times between enemies |
| 6 | Reload Punish | Passive | During reload window, melee strike deals 200% |
| 7 | Brass Knuckle Pistol Whip | Reactive | Out-of-ammo melee that auto-reloads |
| 8 | Last Chamber | Ultimate | 1 bullet remaining → next shot deals 800% damage |

### 3.5 Hexer — Dark Crimson / Green-Purple lantern — Tier A

Mechanic theme: **Curse stacks + Lantern AoE + Sickly Yellow accent** (per `CCS_Hexer_HexLeech` + color palette)

| # | Skill | Type | Mechanic Sketch |
|---|---|---|---|
| 1 | Curse Bolt | Basic | Single-target, applies Curse stack (max 5) |
| 2 | Lantern Pulse | AoE | Ring damage from Hexer position, +damage per Curse on hit target |
| 3 | Hex Leech | Cross (✓ exists) | Cursed enemy deaths heal Hexer |
| 4 | Wither Touch | Melee | Strong single target, applies 3 Curse stacks |
| 5 | Plague Mark | Spread | On cursed enemy death, curse jumps to 2 nearest |
| 6 | Soul Drain | Channel | Drain HP from cursed targets in range |
| 7 | Effigy Doll | Summon | Decoy that draws aggro, explodes for curse splash on death |
| 8 | Final Verdict | Ultimate | All cursed enemies in range take 200% × Curse stack damage |

### 3.6 Summoner — Black / Bone White / Cold Blue crystal — Tier C

Mechanic theme: **Grave Pact + Minion mgmt + Necro green ghosts**

| # | Skill | Type | Mechanic Sketch |
|---|---|---|---|
| 1 | Bone Bolt | Basic | Single-target spell, low damage |
| 2 | Raise Skeleton | Summon | Spawn skeleton minion (max 3), follows Summoner |
| 3 | Grave Pact | Cross (✓ exists) | Minion death = Summoner buff |
| 4 | Marrow Spike | AoE | Bone spikes erupt at cursor, multi-hit |
| 5 | Soul Shield | Defensive | Minion sacrifices itself to absorb fatal hit |
| 6 | Mass Reanimate | Spend | Convert nearby corpse cells to skeleton minions (cap 5 total) |
| 7 | Crystal Conduit | Passive | Each active minion = +5% damage to Summoner |
| 8 | Bone Storm | Ultimate | All minions dash to target point, explode |

---

## 4. Effort Estimate (per class)

Per class (assuming Warblade pattern):
- 8 .cs skill files × ~50-150 lines each = 400-1200 lines per class
- 8 .asset files
- `<Class>SkillBase.cs` + `<Class>_SkillController.cs` per class
- VFX prefab references (later)

**Total for 6 missing classes:** ~24-48 hours Codex effort (mechanical implementation following Warblade/Shadowblade pattern)

**For S85 night:** unrealistic to do all 6. Pick TOP 2 (Brawler + Ronin = Tier S) for tonight's queue if Map V1 wraps early.

**For 4 incomplete (Shadowblade/Elementalist/Ranger have .cs but no .asset):** much smaller scope — generate .asset SOs from existing .cs definitions. Could be done by Codex in ~2-3 hours total.

---

## 5. Sabah İçin Karar Noktaları (kullanıcı onayı gereken)

**Q1.** Sprint 1-8 (Map V1) önce mi bitsin, sonra class skill mi? Şu an yatkın olduğum: **Map V1 önce, class skill paralel hazırlık + sabah dispatch**.

**Q2.** "Missing 6 classes" sırası onay:
- Tier S: Brawler + Ronin (önce — balance priority)
- Tier A: Gunslinger + Hexer (sonra)
- Tier C: Ravager + Summoner (en son — complex mechanics)

**Q3.** "4 incomplete classes" (Shadowblade/Elementalist/Ranger .asset eksiği) için: Codex'e tek dispatch yazıp existing .cs'lerden .asset generate edebilir mi? Bu ~2-3 saat iş.

**Q4.** Yukarıdaki skill önerileri tasarım açısından OK mi? Eksik/yanlış varsa NLM'den karar çekmek gerek (sabah Chrome auth sonrası).

**Q5.** VFX detayları (ghost VFX colors, slash trails, burst effects) — class implementation sırasında mı, ayrı VFX sprint mi?

---

## 6. Sabah Aksiyon Önerisi

1. **Önce:** Map V1 progress check (Sprint 2 PASS? Sprint 4 dispatch?)
2. **Q1 onay:** Map V1 önce mi class skill paralel mi
3. **Q2 onay:** Sıralama
4. **Q3 dispatch:** Mevcut 4 class için .asset gen (Codex, ~2-3 saat)
5. **Q4 NLM çek:** Auth fix + skill design kararları cross-check
6. **Sonra:** Brawler skill dispatch (Codex, mekanik, Warblade pattern)

---

## 7. Mevcut Code Reuse Patterns

Yeni class skill'leri yazılırken Warblade pattern referans (LIVE):
- `Assets/Scripts/Skills/Base/SkillBase.cs` — abstract base
- `Assets/Scripts/Skills/Base/Warblade_SkillController.cs` — class controller pattern
- `Assets/Scripts/Skills/Warblade/*.cs` — skill implementation pattern (14 skill file)
- `Assets/Data/Skills/Skill_*.asset` — .asset format (7 Warblade .asset)
- `Assets/Data/CrossClass/CCS_*.asset` — cross-class .asset format (10 CCS)

Yeni class için boilerplate:
1. `Assets/Scripts/Skills/<Class>/<Class>SkillBase.cs` — class-specific base
2. `Assets/Scripts/Skills/<Class>/<Class>_SkillController.cs` — controller
3. `Assets/Scripts/Skills/<Class>/<SkillName>.cs` × 8 — skill implementations
4. `Assets/Data/Skills/Skill_<Name>.asset` × 8 — data SOs

Codex'e dispatch için ideal scope: 1 class per dispatch (8 .cs + 8 .asset + 1 base + 1 controller = 18 files, split into 3-4 sub-dispatches per 5-file limit).

---

**End of analysis.** Map V1 öncelikli; class skill iş Map V1 sonrası ya da paralel sabah dispatch.
