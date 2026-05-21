# RIMA Progression Plan v2.3 LOCK (FINAL)

**Status:** LOCK FINAL. User onayı 2026-05-21. Önceki versiyonlar (v2.2 LOCK, v2.1 review, v2 FINAL) **SUPERSEDED** — archive'a taşınacak.
**Basis:** Orchestrator + Codex + Antigravity 3-way verdict birleşik.
**Filozofi:** Basit tut, scope dar, her run farklı hissi şart, mekanik şişirme YOK, demo önce.

---

## Section 0 — Verdict Summary (LOCK karar seti)

| Konu | LOCK Değeri |
|---|---|
| Gate 1 Forge | **A1: B01 Curse Gate VEYA B02 Forge mutually exclusive branch** (Karar #62 reopen, 15 node total korunur, per-run random) |
| Topology | **StS2-style procedural branching graph + fog of war + fragment-driven node reveal** (Karar #62 revised 2026-05-21) |
| Gate 2 Echo Imprint Trigger | **B1: Canonical "every 3 combat + slot cap"** (Act 1 pratikte first Elite'e denk gelir, sub-system minimal) |
| Gate 3 Architect Break | **C1: Phase 1 narrative-only**, NPC removal POST-LAUNCH defer |
| Gate 4 Hub Class Start | **D2: 4-class start** (Warblade + Elementalist + Ranger + Shadowblade) Karar #150 priority + canonical override note |
| Gate 5 Image 13 | **E1: Manual crop** — bottom band sil, Architect altına "Ending Choice" strip |
| Gate 6 Demo Scope | **F1: 4-class demo strict**, post-demo 6 class expansion |
| Bug #1 Rest Node | **Hybrid Rest** — 1 Rest/Act, choice "%25 HP restore" VEYA "1 Skill Tier Upgrade" |
| Bug #2 Branching Soft-Lock | **Boss Door Depth 12 auto-unlock**, 8/8 fragment = "Tablet Reconstruction" bonus (+50 Gold veya re-roll) |
| Bug #3 Sub-Room Premature Spawn | **MapFragment sadece MacroRoomController üzerinden**, transition portal'larda spawn YOK |
| Open Q1 Forge Branch Risk | **Elite mob guard** (mor sigil) yüksek craft ödülünü dengeler |
| Open Q2 Hybrid Rest UI | **"%25 HP VEYA 1 Skill Tier Upgrade" choice** approved |
| Open Q3 Locked Class UI | **"Coming Soon" silhouette** hub'da görünür (scope motivation) |

---

## Section 1 — Locked Mechanics (v2.3 final)

### 1.1 Class Anchor (run başı)

- **10 class total:** Warblade, Elementalist, Ranger, Shadowblade (UNLOCKED Phase 1) + Ronin, Gunslinger, Ravager, Hexer, Brawler, Summoner (LOCKED, **demo'da Echoes ile unlockable**)
- **4 starting class:** Warblade / Elementalist / Ranger / Shadowblade — Karar #150 playtest priority, demo'da hemen oynanabilir
- **6 locked class hub UI:** Hub ana ekranında **fiyatla görünür** (silhouette + unlock cost), Phase 1 demo'da unlock yapılabilir
- **Ravager weapon canonical:** Dual axe (çift balta) — user 2026-05-21 confirm
- **Unlock economy (canonical override + revised):**
  - Ronin: 120 Echoes
  - Ravager: 120 Echoes
  - Gunslinger: 180 Echoes
  - Brawler: 180 Echoes
  - Summoner: 180 Echoes
  - Hexer: 250 Echoes + "Elementalist ile 1 run yap" precondition
- **Stay/Break/Carry = ending choice only** (post-Architect, NOT run-start)
- **No starting buff differential**

### 1.2 Act 1 — StS2-Style Branching Graph + Fog of War + Penitent Sovereign

**Karar #62 REVISED 2026-05-21:** "Fixed topology" yorumu yerine **StS2-style procedural branching graph** kabul. Her run farklı topology üretilir AMA logical constraints'le balance korunur.

#### Topology Structure

| Parametre | Değer |
|---|---|
| Total nodes | 15 (Act 1) |
| Rows (depth) | 7 (boss row dahil) |
| Columns (max width) | 3 parallel path |
| Branching | Each node max 2-3 edges to next row |
| Convergence | Paths can merge/diverge |

#### Node Distribution Constraints (logical adjacency rules)

| Type | Count | Constraint |
|---|---|---|
| Entry | 1 | Row 0, single start point |
| Combat | 6 | Spread, no 2 Combat in same row |
| Elite | 2 | Min 2 rows apart, never adjacent to Boss row |
| Rest (Hybrid) | 1 | Mid-act, ~row 4 |
| Shop | 1 | Never adjacent to another Shop (no 2-Shop chain) |
| Boss | 1 | Row 6 (last) |
| Branch (Curse Gate or Forge) | 1 | Per-run **mutually exclusive** random: B01 Curse Gate OR B02 Forge (Codex önerisi LOCK) |

**Mutually exclusive branch:** Her run rastgele Curse Gate VEYA Forge spawn olur — ikisi birden değil. Total = 15 node (Karar #62 LOCK korunur). Replay variety per-run randomization'la sağlanır.

**Adjacency Rules (hard constraint per Codex review):**
- No 2 Shop adjacent
- No 2 Elite adjacent
- No Boss-adjacent Elite (max difficulty curve)
- Rest must be between Combat-Combat (transit role)

#### Fog of War + Fragment-Driven Reveal (yeni mekanik LOCK)

**Act başında player şunu görür:**
- **Graph topology görünür** (hangi node hangisine bağlanıyor — connection lines)
- **Node tipleri GİZLİ** (her node "?" simgesi, karanlık)
- **Sadece N00 Entry ve N12 Boss tipi görünür** (start + end anchored)

**Map Fragment pickup → reveal:**
- Combat clear → fragment drops (Karar #63 mevcut)
- Pickup → **1-2 node ileri tip reveal** (Karar #63 reveal odds: 1 node %65, 2 node %30, 3 node %5)
- Branch node tipi (Curse Gate VEYA Forge) ancak yaklaşırken reveal edilir

**Visual states (5 state per node):**
- `unrevealed` — "?" silhouette, dark
- `revealed` — type icon visible (combat/elite/shop/etc.)
- `current` — pulsing border, player burada
- `visited` — gray checkmark
- `next-up` — bright border (next reachable from current)

**Implementation:**
- Map graph procedurally generated at run start (seed-deterministic)
- Each node has `Revealed` bool state
- Fragment pickup triggers BFS reveal from current node, depth = roll result
- HUD: "Tablet Reconstruction X/8" still tracks fragment collection (bonus reward at 8/8)

#### Boss Door (Bug #2 fix integrated)

- Boss Door auto-unlocks when player reaches **Row 6** (Boss node neighbor)
- Fragment count DOES NOT gate boss access
- 8/8 fragment = bonus reward (+50 Gold OR +1 free Legendary re-roll)
- **No soft-lock risk** — branch path or main path both reach boss row

#### Boss Reward — Penitent Sovereign (Act 1)

- WIN: Max HP +50%, 75 Gold, Relic + class-specific Legendary 3-choice → Secondary Class selection (1 random from remaining 3 starting classes) → +2 active slot (total 6) → Cross-class Passive activates.
- LOSE: Standard death, Shattered Echoes earned, no secondary class unlock.

#### Threat Budgets
- Entry: tutorial, no threat
- Combat: 8-12 threat
- Elite: 14-18 threat, min 1 Elite mob

**Boss reward — Penitent Sovereign:**
- WIN: Max HP +50%, 75 Gold, Relic + class-specific Legendary 3-choice → Secondary Class selection (1 random remaining from 4-start) → +2 active slot (total 6) → Cross-class Passive activates.
- LOSE: Standard death, Shattered Echoes earned, no secondary class unlock.

### 1.3 Threshold UI — Universal Shader × 8 Variant

Değişmedi (v2.2'den taşındı): Dormant/Combat/Elite/Rest/Shop/Curse/Forge/Boss Sealed. **Mystery threshold silindi (B02 → Forge oldu).**

### 1.4 Sub-Room Encounter (BUG #3 fix dahil)

- Macro Combat = 2+ sub-rooms (Faz-1 = entry_chamber + pillar_arena)
- 5 canonical tag: entry_chamber, pillar_arena, collapse_corridor, ritual_hall, crypt_cell
- **MapFragment instantiation:** ONLY via `MacroRoomController` after FINAL sub-room cleared
- **HARD GUARD:** Sub-room transition portal'larda spawn YASAK (Bug #3 fix)
- Internal sub-room transition = fade
- Fragment mandatory before door opens

### 1.5 Map Fragment + Kırık Taş Tablet (BUG #2 fix integrated)

**Drop rules (revised):**

| Node Type | Fragment Rule | Boss Quota? |
|---|---|---|
| Combat | Guaranteed | **N/A — boss auto-unlocks Depth 12** |
| Elite | Guaranteed | **N/A** |
| Forge (B02) | None | **N/A** |
| Curse Gate (B01) | None | **N/A** |
| Rest / Shop | None | **N/A** |
| Boss | **Door auto-opens at Depth 12 regardless** | Fragment count = bonus |

**Fragment role redefined:** "Tablet Reconstruction Level" — 8/8 → bonus reward (+50 Gold OR +1 free Legendary re-roll). Boss gate'i ETKİLEMEZ.

**HUD:** "Tablet Reconstruction X/8" badge + small "+Y bonus reveal" badge for Mystery/branch fragments (Bug #2 + Codex Conflict 4 integrated).

**Pickup flow:**
1. Macro encounter clears.
2. Fragment spawns (cyan #00FFCC, bobbing ±0.10u @ 2.2hz, 2.5u radius).
3. Player presses G.
4. Reveal roll: 1 node 65% / 2 nodes 30% / 3 nodes 5%. Open node +1 hop.
5. Skill Draft 3-choice opens auto.

**Map UI — Kırık Taş Tablet:**
- TAB = full-screen MapPanel (StS-style abstract graph, 14+2 node, center 70%)
- Top-left MiniMap 128×128 (current room + door arrows, persistent)
- Frame: rusty gold, cyan rift cracks
- Act visual evolution: Act 1 castle carvings / Act 2 veined flesh / Act 3 floating pieces / Act 4 mirror
- HUD counter: top-center "Tablet X/8" + "+N bonus reveal" badge

### 1.6 Skill Draft 3-Choice + Echo Imprint

**Flow:** Room clear → Fragment pickup → 3-choice Skill Draft.

**3 offer types:** New active skill / Tier upgrade / Echo Imprint.

**Offer weights:** (Codex Conflict 1 fix dahil)
| State | New Skill | Tier Upgrade | Echo Imprint |
|---|---|---|---|
| Slots not full + upgrades available | 40% | 40% | 20% |
| **All slots full (Act 1 4/4)** | **0%** (suppressed) | 80% | 20% |
| No upgrades (all Common) | 60% | — | 40% |

**Tier flow:** Common → Rare → Epic → Legendary. Max 3 upgrade slots per skill. Resource-free.

**Echo Imprint:**
- Parallel lane (NOT active skill slot)
- Max 4/run (1 slot per act)
- **Trigger:** Canonical "every 3 combat/elite clear" — Act 1 pratikte N03 ilk Elite'e denk gelir
- Slot full ise trigger ekstra Draft seçenek olarak gelir, yeni slot AÇMAZ
- Categories: Strike Form (LMB), Outlet Form (RMB), Surge Form (Dash/resource)

**Reroll:** 1 free/run. Shop: 150 Gold extra.

**Pity:** Codex Conflict 3 fix — sadece normal 3-choice Skill Draft sayar (Elite Reward + Boss reward sayılmaz). Skill absent 5 consecutive drafts → 80% next.

### 1.7 Item Economy (BUG #1 + Forge B02 integrated)

**Three channels:**
1. **Drop:** Combat 20% random Component. Elite 100% Component (2-choice). Boss = Relic + class Legendary 3-choice (Codex Conflict 7: class's 3 canonical anchors, NOT random).
2. **Shop:** Gold purchase, in-run only. HP Restore Small 30G, HP Restore Large 70G, Skill Reroll 150G, Skill Modifier 80-120G (%80), Tier Upgrade 150G (%70), Relic 200G (%30), Class Upgrade 250G (%40). Anvil = rare expensive safety valve only (NOT main craft path).
3. **Forge:** **Forge B02 branch node Act 1** — Combined craft (visit 1) + Legendary craft (visit 2). 1 main action per visit. **Free** (no gold cost). Elite mob guard before access (Q1 answer).

**7 Components:** Iron Shard, Void Fragment, Chain Links, Shadow Veil, Blood Gem, Rift Stone (Memory Shard visual repurposed), Soul Ember.

**9 Combined Items (CANONICAL — user lock 2026-05-21):**
| Recipe | Result |
|---|---|
| Iron Shard + Blood Gem | Vampiric Blade |
| Void Fragment + Shadow Veil | Phantom Weave |
| Rift Stone + Soul Ember | Frenzy Core |
| Iron Shard + Chain Links | Warlord's Plate |
| Iron Shard + Rift Stone | Rift Piercer |
| Blood Gem + Soul Ember | Soul Tap |
| Void Fragment + Rift Stone | Fracture Amp |
| Shadow Veil + Soul Ember | Ghost Step |
| Chain Links + Blood Gem | Iron Will |

**Item slots:** 4 total. Component/Combined/Legendary share.

**Relic:** Rule-bender. Sources: Boss + rare Curse Gate Legendary Shortcut (Codex Conflict 5 PARTIAL: Curse Gate Phase 1 = ONLY Legendary Shortcut lane, other 2 lanes discarded).

**Legendary:** Class-specific 3 canonical anchors per class (NOT random pool, Codex Conflict 7). Boss guaranteed 3-choice. Forge path: 2 Combined → Legendary.

**Health:** No floor drop. Elite +12%, Shop 30G/70G, Boss +50%, Rest Hybrid %25 (option A), Curse Gate reject +5%.

**Gold:** Combat 5-15, Elite 20-35, Boss 50-75, Event 30-100. In-run only.

**Shards:** 1-5/enemy. Phase 2+ use.

**Shattered Echoes:** Meta-currency persistent. Earn: Boss kill +10, Act complete +5, first class +5, run-end kill×0.1. Spend: Hub Phase 1 = class unlock only (4 starting + 6 lockable). Phase 2+ = cosmetic/QoL/challenge mode.

### 1.8 Cross-Class Progression

(Değişmedi v2.2'den, sadece 4-start integration)

**Act 1 (primary class only):** 4 active slots (Q/E/R/F), 2 visible-locked. Skill Draft pool 100% primary.
- **Codex Conflict 1 fix:** 4 slot full ise New Skill weight 0%, suppress veya replace flow.
- **Codex Conflict 2 fix:** CrossClassProcManager disabled until secondary class selected.

**Act 1 Boss → Cross-Class unlock:**
- Remaining 3 class'tan 2 random presented → pick 1 secondary
- +2 active slot (total 6), Cross-class passive activates
- Pool shift Act 2 early: primary 65% / sec 20% / neutral 15%

**Act 2 progression:** Pool Act 2 late: primary 55% / sec 30% / neutral 15%. Spirit Encounter +1.

**Act 2 Boss → Cross-Class Ultimate access.**

**Act 3 progression:** Pool primary 45% / sec 45% / neutral 10%. Legendary tier available.

**Act 3 Boss (Fracture Sovereign):** HP +50%, 75G, Relic + Legendary, Legendary tier Skill Draft unlock.

**Final Act 4 — The Architect:** Mixed draft. Boss multi-phase. WIN → Shattered Echoes 50-75 → "Architect defeated" meta-unlock (TBD post-launch) → **Stay/Break/Carry ending** (Phase 1 NARRATIVE ONLY, NPC removal post-launch).

**Cross-Class proc:** LMB commit-beat, 1.2s cooldown, 35% origin damage.

### 1.9 Boss Gate (REVISED — Auto-unlock)

**Bug #2 fix:**
- Boss door **auto-unlocks at Depth 12 (N12 reached)** — fragment count IGNORED for gate
- 8 Tablet fragments collected = bonus (+50 Gold OR +1 free Legendary re-roll)
- HUD: "Tablet X/8" badge (no game progress lock implication)

| Boss | HP | Gold | Special |
|---|---|---|---|
| Act 1 — Penitent Sovereign | +50% | 75 | Relic + Legendary + Secondary Class + Cross-class Passive |
| Act 2 — Echo Twin | +50% | 75 | Relic + Legendary + Cross-Class Ultimate |
| Act 3 — Fracture Sovereign | +50% | 75 | Relic + Legendary + Legendary tier Skill Draft |
| Act 4 — The Architect | None | None | Shattered Echoes 50-75 + ending choice |

### 1.10 Shattered Echoes Meta-Currency

- Persistent
- Visual: repurposed Echo Orb. NOT "Echo Essence"
- Earn rates unchanged
- Spend Phase 1: class unlock (Section 1.1 economy)
- Phase 2+: cosmetic / starting Imprint pre-pick / run modifier

---

## Section 2 — Deferred / Post-Launch

### 2.1 Death Imprint Echo Drop (Option C) — POST-LAUNCH DEFER

Spec değişmedi v2.2'den. Implementation Phase 2+.

### 2.2 Tag Synergy Bonus System — Phase 2+

### 2.3 Spirit Encounter Node — Phase 3+

### 2.4 Shards Phase 2 Use

### 2.5 Architect Break NPC Permanent Removal — POST-LAUNCH

Q3 + Gate 3 LOCK: Phase 1 narrative only. GDD spec korunur ama implementation post-launch. Break choice presented, NPC removal scripting deferred.

### 2.6 Other 6 Class Skill Expansion — POST-DEMO

Ronin 4 skill → 12-15 skill, Ravager/Hexer/Brawler/Summoner/Gunslinger 8 skill → 12-15 skill expansion post-demo.

---

## Section 3 — Discarded (v2.2 + v2.3 cumulative)

| # | Item | Reason |
|---:|---|---|
| 3.1 | Stay/Break/Carry as run-start mechanic | Ending choice only |
| 3.2 | Rune system | Karar #60 NO RUNE |
| 3.3 | Boss Key item | Karar #63 (now Boss Door auto-unlock) |
| 3.4 | Health Orb drop | No floor HP drop |
| 3.5 | Echo Essence naming | Renamed Shattered Echoes |
| 3.6 | Corridor / Chest room types | Karar #62 |
| 3.7 | Combined Item placeholder names | Canonical 9 adopted |
| 3.8 | "All Max" Act 4 mechanical drop | Render shorthand |
| 3.9 | Relic tiered naming ("Epic Relic") | Relic = singular rule-bender |
| 3.10 | Mystery branch node (B02) | **NEW: Replaced with Forge (Gate 1 A1)** |
| 3.11 | 2 Rest nodes per Act (N04 + N09) | **NEW: Reduced to 1 Rest/Act (Bug #1 Hybrid)** |
| 3.12 | 8-fragment hard boss gate | **NEW: Auto-unlock Depth 12 (Bug #2)** |
| 3.13 | Curse Gate Burden/Gift + bonus reveal lanes | **NEW: Phase 1 = Legendary Shortcut lane only (Codex Conflict 5 PARTIAL)** |
| 3.14 | Anvil as main Act 1 craft path | **NEW: Anvil rare safety valve only, Forge B02 is main path** |

---

## Section 4 — Antigravity / Codex Bonus Conflict Resolutions (v2.3 baked-in)

| Source | Conflict | Fix Baked-In |
|---|---|---|
| Codex Conflict 1 | Skill slot 4→6 draft routing | All slots full → New Skill 0%, Tier 80%, Imprint 20% (Section 1.6) |
| Codex Conflict 2 | Cross-class proc Act 1 disabled | CrossClassProcManager guard until secondary selected (Section 1.8) |
| Codex Conflict 3 | Pity counter source | Normal 3-choice only (Section 1.6) |
| Codex Conflict 4 | Mystery HUD 9/8 | Tablet 8/8 cap + bonus badge (Section 1.5) |
| Codex Conflict 5 | Curse Gate 3 lane conflict | Phase 1 = Legendary Shortcut lane only (Section 1.7) |
| Codex Conflict 6 | Forge/Anvil pricing | Forge free + 1 action, Anvil gold + rare safety valve (Section 1.7) |
| Codex Conflict 7 | Boss Legendary 3-choice | Class's 3 canonical anchors, no randomization (Section 1.7) |
| Antigravity Bug #1 | Rest Node contradiction | Hybrid Rest 1/Act, choice HP/Tier (Section 1.2, 1.7) |
| Antigravity Bug #2 | Branching Soft-Lock | Boss Depth 12 auto-unlock (Section 1.9) |
| Antigravity Bug #3 | Sub-room premature spawn | MacroRoomController guard (Section 1.4) |

---

## Section 5 — Open Items (deferred, NON-blocking)

| # | Item | Note |
|---|---|---|
| ~~O1~~ | ~~B01/B02 mutual exclusivity~~ | **RESOLVED 2026-05-21:** Mutually exclusive per run, 15 node total korunur (Section 1.2) |
| O2 | Architect ending meta-unlock content | Phase 2+ patch — yeni class / keepsake / hub feature / story-only ending? Phase 2 planning'e ertelendi. |
| O3 | Hub Phase 2+ catalog | Cosmetic + starting Imprint + run modifier — Phase 2 planning'e ertelendi. |

**Hiçbiri Phase 1 implementation blocking değil.**

---

## Section 6 — Implementation Roadmap

### 6.1 UI Foundation
- Kırık Taş Tablet MapPanel (TAB, **StS2-style procedural branching graph**, 15 node, 5 reveal state, Act visual evolution)
- **Fog of war reveal system** — graph connections görünür, node types fragment pickup ile reveal
- MiniMap 128×128 (persistent, current row + reachable next-nodes)
- Tablet HUD "X/8 + bonus" badge
- Boss row auto-unlock visual (Row 6 reached)
- Hub class unlock UI — 4 starting + 6 locked priced (Coming Soon değil, "unlock for X Echoes")

### 6.2 Reward Flow
- Map Fragment spawn + G pickup + **BFS node reveal** (1/2/3 hop = %65/%30/%5) + Skill Draft trigger chain (Bug #3 MacroRoomController guard)
- Skill Draft 3-choice screen (weights table + pity counter normal-only + reroll)
- Slot routing guard (4 full → New Skill suppress)
- Tablet bonus reward trigger (8/8 → +50 Gold OR re-roll)

### 6.1b Procedural Map Generation (NEW)
- **Per-run seeded graph generator** — Act 1 = 15 node, 7 row, 3 column max
- Hard constraints: no Shop-Shop adjacent, no Elite-Elite adjacent, Rest mid-act, Boss row 6
- Branch (B01 Curse Gate OR B02 Forge) mutually exclusive random
- BFS reveal traversal on fragment pickup
- Connection edges generated with branching factor 2-3 per node
- Output: `GraphNode` ScriptableObject + runtime `MapInstance` populated per run

### 6.3 Sub-Room System
- EncounterController sub-room sequence (Faz-1 LIVE)
- Mirror edge validator (±2 tolerance)
- Macro clear detection (LAST sub-room only)
- Fragment spawn guard

### 6.4 Cross-Class (Act 1 boss kill onward)
- Secondary class selection screen (2 from remaining 3 4-start classes)
- +2 slot unlock
- Cross-class passive activation
- CrossClassProcManager enable gate
- Skill Draft pool weight shift per Act
- Act 2 Ultimate unlock

### 6.5 Boss Gate (Bug #2 fix)
- Boss Door auto-unlock at Depth 12 (N12)
- 8/8 Tablet bonus reward (50 Gold + re-roll select)
- Act-specific boss reward unlock

### 6.6 Item / Craft Economy (Bug #1 + Forge B02 integration)
- Component drop system
- **Forge B02 branch node** (Combined + Legendary craft, free + 1 action)
- Anvil safety valve (Shop, gold + rare)
- 4-slot inventory
- Shop inventory randomization
- Relic award (Boss + Curse Gate Legendary Shortcut only)
- Shattered Echoes earn tracking

### 6.7 Hybrid Rest Node (Bug #1 fix)
- 1 Rest/Act (N04)
- Choice UI: %25 HP restore OR 1 Skill Tier Upgrade
- F1→F2 transit

### 6.8 4-Class Hub (Gate 4 + Karar #150)
- Warblade + Elementalist + Ranger + Shadowblade unlocked Phase 1
- 6 class locked (Coming Soon silhouette)
- Echo Keeper NPC (unlock UI)

### 6.9 Image 13 Manual Crop (Gate 5)
- Bottom S/B/C band silinir
- Architect altına compact "Ending Choice: Stay / Break / Carry" strip

### 6.10 Death Imprint Echo Drop (POST-LAUNCH) — Section 2.1

---

## Section 7 — Final Decision

Progression Plan v2.3 LOCK. v2.2 + earlier versions SUPERSEDED → archive.

3 doc çelişkisi çözüldü:
1. ROOM_MECHANICS "Rest yok" vs MAP_ITEM_SYSTEM "Rest guaranteed" → **Hybrid Rest 1/Act**
2. MAP_ITEM_SYSTEM Forge guaranteed Act 1 vs Karar #62 8-type → **B02 → Forge swap**
3. Boss gate 8-fragment hard vs branching = soft-lock risk → **Depth 12 auto-unlock**

Codex 7 logic conflict + Antigravity 3 critical bug — hepsi v2.3'e baked-in.

3 open question (O1/O2/O3) Phase 2+ planning'e ertelendi (Phase 1 blocking değil).

Next step: Code implementation spec per Section 6 roadmap, sırayla 6.1 → 6.10.
