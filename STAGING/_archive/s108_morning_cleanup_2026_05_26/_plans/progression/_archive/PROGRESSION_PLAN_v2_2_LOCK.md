# RIMA Progression Plan v2.2 LOCK

**Status:** v2.1 review onaylı + user yeni kararları (2026-05-21) + TASARIM canonical batch. v2.2 = LOCK kandidatı (5 açık karar gate'i var, user onayı sonrası FINAL).
**Filozofi:** Basit tut, her run farklı hissi şart, mekanik şişirme YOK.
**Basis:** v2 FINAL + v2.1 Codex review + user decisions + TASARIM canonical reconciliation (MAP_ITEM_SYSTEM.md, ROOM_MECHANICS.md, map_fragment_system.md, GDD.md, CROSS_CLASS_PROC_SYSTEM.md, SUBROOM_TEMPLATES_ACT1.md).

---

## Section 0 — NLM Findings (10 sorgu, TASARIM canonical)

**Q1 — Item ekonomisi nasıl çalışıyor?**
Three-channel economy: (1) Drop: Combat 20% Component, Elite 100% Component (2 seçenek). (2) Shop/Merchant: Gold ile HP/Skill Mod/Tier Up/Relic/Class Upgrade. (3) Forge: Component+Component → Combined, Combined+Combined → Legendary. Karar: MAP_ITEM_SYSTEM.md "Final Karar Belgesi 2026-04-12".

**Q2 — Shop ne satar, Gold nasıl harcanır?**
HP Restore Small 30G, HP Restore Large 70G, Skill Reroll 150G (1 free/run), Skill Modifier 80-120G (%80), Tier Upgrade 150G (%70), Relic 200G (%30), Class Upgrade 250G (%40). Gold earn: Combat 5-15, Elite 20-35, Boss 50-75, Event 30-100. Gold spend = Shop only.

**Q3 — Combined Item craft, recipe sabit mi?**
FIXED recipes (random DEĞİL). 9 canonical recipes (MAP_ITEM_SYSTEM.md):
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

**Q4 — Cross-Class Act 1 sonrası?**
Act 1 boss kill → 2 random secondary class presented → pick 1 → +2 active slot (total 6) → Cross-class passive activates. Skill Draft pool: Act 2 early primary 65% / secondary 20% / neutral 15%. Act 2 late primary 55% / secondary 30% / neutral 15%. Act 3 primary 45% / secondary 45% / neutral 10%.

**Q5 — Act 2 Cross-Class Ultimate?**
Act 2 boss kill → secondary class's ULT unlocks for current run. No separate slot. Full dual-class build expression point.

**Q6 — Skill slot ve Echo Imprint sayıları?**
Active skill slots: Act 1 = 4 (Q/E/R/F), after Act 1 boss = 6 total. Echo Imprint = parallel lane, max 4/run (1 slot per act). Trigger: every 3 combat rooms as Skill Draft choice. Categories: Strike Form (LMB), Outlet Form (RMB), Surge Form (Dash/resource).

**Q7 — Sub-room encounter spec?**
Macro Combat encounter = 2+ sub-rooms (Faz-1 = 2). 5 canonical tags: entry_chamber, pillar_arena, collapse_corridor, ritual_hall, crypt_cell. Map Fragment drops after MACRO clear, NOT per sub-room. Skill Draft after Fragment pickup. Internal sub-room transition = fade. Mirror edge validator (compatible-edge rule, ±2 cell tolerance).

**Q8 — Map Fragment G key + Skill Draft otomatik?**
G key canonical. Flow: Encounter clear → Fragment spawn (cyan #00FFCC, bobbing ±0.10u @ 2.2hz, 2.5u radius) → G press → reveal roll → Skill Draft AUTO opens. Fragment mandatory before door opens.

**Q9 — Death Imprint Echo Drop detail?**
User onayı: Seçenek C (Echo Drop), POST-LAUNCH defer. Mevcut spec v2.1 review'dan: ghost echo (class silhouette, weapon, attack rhythm), +25-40 Shattered Echoes kill reward. Anti-farming rules. Boss gate/Map Fragment/Skill Draft etkisi YOK.

**Q10 — Full run timeline?**
GDD canonical: 55-70 min full run. Act 1 = 15 nodes (Karar #62). Act 2-3 = 9-11 rooms each. Final = 5-6 rooms. Calculated per-act timing: Act 1 ~15-20 min, Act 2-3 ~15-18 min each, Final ~8-12 min. Sub-room duration: NLM BLOCKED (canonical timing data yok).

---

## Section 1 — Locked Mechanics (v2.2 final)

### 1.1 Class Anchor (run başı)

- 10 class: Warblade, Elementalist, Shadowblade, Ranger, Ravager, Ronin, Gunslinger, Brawler, Summoner, Hexer.
- Player selects 1 primary class at hub start.
- **Stay/Break/Carry = ending choice only** (post-Architect), NOT run-start mechanic. Image 13 fix gerekli.
- No starting buff differential. No playstyle modifier. No meta-track selector per run start.

### 1.2 Act 1 — 15 Node + Penitent Sovereign

Karar #62 LOCK: 15 nodes (13 main + 2 branch), fixed topology, random content.

| Node | Type | Count | Threshold Visual | Reward Rule |
|---|---|---|---|---|
| N00 | Entry | 1 | Dormant cyan rift | No threat, no drop |
| N01-02 | Combat | 2/6 | Cyan rift, low pulse | Guaranteed Map Fragment → Skill Draft |
| N03 | Elite | 1/2 | Cyan rift + heavy sigil | Guaranteed Fragment, HP +12% |
| N04 | Rest | 1/2 | Quiet rift, transit seal | No fragment, F1→F2 transit |
| N05 | Combat | 3/6 | Cyan rift, low pulse | Guaranteed Fragment → Skill Draft |
| N06 | Shop | 1 | Warm safe NPC overlay | No fragment, Gold spend |
| N07 | Combat | 4/6 | Cyan rift, low pulse | Guaranteed Fragment → Skill Draft |
| N08 | Elite | 2/2 | Cyan rift + heavy sigil | Guaranteed Fragment, HP +12% |
| N09 | Rest | 2/2 | Quiet rift, transit seal | No fragment, F2→F3 transit |
| N10-11 | Combat | 5-6/6 | Cyan rift, low pulse | Guaranteed Fragment → Skill Draft |
| N12 | Boss (Penitent Sovereign) | 1 | Sealed boss rift, 8-fragment gate | Opens after 6 Combat + 2 Elite mandatory |
| B01 | Curse Gate | 1 branch | Red-black burden/gift rift | No fragment, risk/reward |
| B02 | Mystery | 1 branch | Asymmetric event rift | Chance-based Fragment (bonus, not quota) |

**Boss reward — Penitent Sovereign (Act 1):**
- WIN: Max HP +50%, 75 Gold, Relic + class-specific Legendary 3-choice → Secondary Class selection (2 random → pick 1) → +2 active skill slot (total 6) → Cross-class Passive activates.
- LOSE: Standard death, Shattered Echoes earned so far, no secondary class unlock.

Threat budgets: Entry tutorial, Combat 8-12, Elite 14-18 min 1 Elite mob.

### 1.3 Threshold UI — Universal Shader × 8 Variant

8 threshold states (Dormant/Combat/Elite/Rest/Shop/Curse/Mystery/Boss Sealed). Universal rift material + state config table. No separate art per node; shader parameter swap. Production %90 saving.

### 1.4 Sub-Room Encounter

Macro Combat = 2+ sub-rooms (Faz-1 = entry_chamber + pillar_arena). 5 canonical tags. Map Fragment drops after MACRO clear, NOT per sub-room. Skill Draft after Fragment pickup. Internal fade transition. Sub-room grid 32×22. Mirror edge validator: compatible-edge (entry_chamber S-exit ↔ pillar_arena N-entry, ±2 cell tolerance).

### 1.5 Map Fragment + Kırık Taş Tablet

**Drop rules (Karar #63):**
| Node Type | Fragment Rule | Boss Quota? |
|---|---|---|
| Combat | Guaranteed | Yes — 6 mandatory |
| Elite | Guaranteed | Yes — 2 mandatory |
| Mystery/B02 | Chance ~50% | No — bonus |
| Rest / Shop / Curse Gate | None | No |
| Boss | Gate checks 8 mandatory | N/A |

**Pickup flow:**
1. Macro encounter clears.
2. Fragment spawns (cyan #00FFCC, bobbing ±0.10u @ 2.2hz, 2.5u radius).
3. Player presses G.
4. Reveal roll: 1 node 65% / 2 nodes 30% / 3 nodes 5%. Open node +1 hop.
5. Skill Draft 3-choice opens auto.

**Map UI — Kırık Taş Tablet:**
- TAB = full-screen MapPanel (StS-style 15-node graph, center 70%).
- Top-left MiniMap 128×128 (current room + door arrows, persistent).
- Frame: rusty gold, cyan rift cracks.
- Act visual evolution: Act 1 castle carvings / Act 2 veined flesh / Act 3 floating pieces / Act 4 mirror.
- HUD counter: top-center "X / 8" fragment counter, pulses on pickup.
- Node states: `visited` (checked), `current` (pulsing), `step1` (icon visible), `step2` ("?" dark), `missed` (40% brightness).

### 1.6 Skill Draft 3-Choice + Echo Imprint

**Flow:** Room clear → Fragment pickup → 3-choice Skill Draft.

**3 offer types:** New active skill, Tier upgrade (Common→Rare→Epic→Legendary), Echo Imprint.

**Offer weights:**
| State | New Skill | Tier Upgrade | Echo Imprint |
|---|---|---|---|
| Slots not full + upgrades available | 40% | 40% | 20% |
| All slots full | 10% | 70% | 20% |
| No upgrades (all Common) | 60% | — | 40% |
Guarantee: min 1 New OR Tier per 3 offers.

**Tier flow:** Common → Rare → Epic → Legendary. Max 3 upgrade slots per skill. Resource-free.

**Echo Imprint:**
- Parallel lane (NOT active skill slot).
- Max 4/run (1 slot per act).
- Trigger: every 3 combat rooms as 3-choice option.
- Categories: Strike (LMB), Outlet (RMB), Surge (Dash/resource).

**Reroll:** 1 free/run. Shop: 150 Gold extra.

**Pity:** Skill absent 5 consecutive drafts → 80% next draft.

### 1.7 Item Economy

**Three channels:**
1. **Drop:** Combat 20% random Component. Elite 100% Component (2 seçenek). Boss = Relic + class Legendary 3-choice. Rare Event/Mystery = Relic possible.
2. **Shop:** Gold purchase, in-run only. HP/Skill Mod/Tier Up/Relic/Class Upgrade with probabilities (Section 0 Q2).
3. **Forge:** Forge Tab 3 — Component+Component → Combined (visit 1), Combined+Combined → Legendary (visit 2). 1 main action per visit.

**7 Components:** Iron Shard, Void Fragment, Chain Links, Shadow Veil, Blood Gem, Rift Stone (repurposed Memory Shard visual), Soul Ember.

**9 Combined Items (CANONICAL — user lock 2026-05-21):** Section 0 Q3 tablo.

**Item slots:** 4 total slot. Component/Combined/Legendary share slots. Combine = slot efficient + power increase.

**Relic:** Rule-bender. Sources: Boss + rare Event/Mystery. No tier label ("Epic Relic" YASAK).

**Legendary:** Class-specific. Boss guaranteed 3 choices. Forge path: 2 Combined → Legendary (2 visits). Curse Gate shortcut: 3 bare Components + risk → random Legendary (40% curse).

**Health:** No floor drop. Elite +12%, Shop 30G/70G, Boss +50%, Curse Gate reject +5%, Events conditional.

**Gold:** In-run only. Earn per Section 0 Q2.

**Shards:** 1-5 per enemy. Phase 2+ use.

**Shattered Echoes:** Meta-currency (persistent). Earn: Boss kill +10, Act complete +5, first class +5, run-end kill×0.1. Hub spend Phase 4+ (catalog open gate).

### 1.8 Cross-Class Progression

**Act 1 (primary only):** 4 active slots, 2 locked/visible. Skill Draft pool 100% primary.

**Act 1 Boss → Cross-Class unlock:**
- 2 random secondary class → pick 1.
- +2 active slot (total 6).
- Cross-class passive activates.
- Pool shift: Act 2 early primary 65% / sec 20% / neutral 15%.

**Act 2:** Pool escalates Act 2 late primary 55% / sec 30% / neutral 15%. Spirit Encounter +1 (Act 2+). Shop +1.

**Act 2 Boss → Cross-Class Ultimate:** Secondary class ULT unlocks. Full dual-class expression.

**Act 3:** Pool primary 45% / sec 45% / neutral 10%. Legendary tier (rare, Epic→Legendary unlock). Spirit 1-2. Curse Gate 1.

**Act 3 Boss (Fracture Sovereign):** HP +50%, 75G, Relic + Legendary choice, Legendary tier Skill Draft unlock.

**Final Act 4 — The Architect:** Mixed draft, 1 Spirit/Curse guaranteed. Boss multi-phase: Faz 1 primary signature, Faz 2 cross-class passive weakness, Faz 3 Legendary reflection.
- WIN: Shattered Echoes 50-75 → Architect defeated meta-unlock (TBD) → Stay/Break/Carry ending.
- LOSE: Standard death, Shattered Echoes earned.

**Cross-Class proc (LIVE):** LMB commit-beat only. 1.2s cooldown. 35% origin-class damage. Per-class proc identity canonical in CROSS_CLASS_PROC_SYSTEM.md.

### 1.9 Boss Gate — 8 Fragment

Boss door locked until 8 mandatory: 6 Combat (N01,02,05,07,10,11) + 2 Elite (N03,08). Branch fragments (B02 Mystery, B01 Curse) = bonus, do NOT count. Boss icon visible. Gate UI pulses when count = 8.

**Per-Act boss reward:**
| Boss | HP | Gold | Special |
|---|---|---|---|
| Act 1 — Penitent Sovereign | +50% | 75 | Relic + Legendary 3-choice + Secondary Class + Cross-class Passive |
| Act 2 — Echo Twin | +50% | 75 | Relic + Legendary + Cross-Class Ultimate unlock |
| Act 3 — Fracture Sovereign | +50% | 75 | Relic + Legendary + Legendary tier Skill Draft unlock |
| Act 4 — The Architect | None | None | Shattered Echoes 50-75 + Architect meta-unlock (TBD) + Stay/Break/Carry ending |

"All Max" image 13'te = render shorthand, mekanik drop YOK.

### 1.10 Shattered Echoes Meta-Currency

- Persistent (carries between runs).
- Visual: repurposed Echo Orb. NOT "Echo Essence".
- Earn: Boss kill +10, Act complete +5, first class +5, run-end kill×0.1, Death Imprint Echo Drop +25-40 (POST-LAUNCH).
- Spend: Hub Phase 4+. Catalog = open karar gate (Section 4).

---

## Section 2 — Deferred / Post-Launch

### 2.1 Death Imprint Echo Drop (Seçenek C) — POST-LAUNCH DEFER

**Status:** User onayı 2026-05-21, post-launch defer.

**Full spec (future reference):**
- Trigger: Sonraki run ölüm sub-room signature match (`encounterId` + `subRoomIndex` + `subRoomTag` + mob composition tags).
- Spawn: Ghost echo actor (class silhouette, weapon, hatırlanmış attack rhythm, killer mob pressure).
- Reward: +25-40 Shattered Echoes kill. NOT Map Fragment / Skill Draft / Relic / boss gate kaynağı.
- Anti-farming: 1 echo per encounter per run, expires after kill, suicide-farm engelli.
- Record fields: `encounterId`, `subRoomIndex`, `subRoomTag`, mob composition, lighting context.
- Manifesto fit: PASS/WARN. Sparse + readable + emotionally framed (revenge shade DEĞİL, loot piñata DEĞİL).
- Cost: HIGH (ghost actor + VFX + spawn rules + persistence + reward UI + anti-farming).

### 2.2 Tag Synergy Bonus System — Phase 2+

Tag matching across active 6 skills → auto-passive bonus. Max 2 synergy active. Full table ROOM_MECHANICS.md §9. Phase 1 blocking DEĞİL.

### 2.3 Spirit Encounter Node — Phase 3+

6 spirit type (Forge Wraith, Shadow Hound, Blood Oracle, Void Seer, Fallen Champion, Ancient Relic). Act 2+ only. Offers: Spirit Tags, resource passives, cross-class synergy, Echo Imprint choices.

### 2.4 Shards Phase 2 Use

1-5 Shard per enemy. Floor collect. Phase 2+ chest/special interaction. Exact spend = open gate.

---

## Section 3 — Discarded

| # | Item | Reason |
|---:|---|---|
| 3.1 | Stay/Break/Carry as run-start mechanic | Ending choice only |
| 3.2 | Rune system | Karar #60 NO RUNE |
| 3.3 | Boss Key item | Karar #63 8-fragment gate |
| 3.4 | Health Orb drop | No floor HP drop |
| 3.5 | Echo Essence naming | Renamed Shattered Echoes |
| 3.6 | Corridor / Chest room types | Karar #62 |
| 3.7 | Combined Item placeholder names (Iron Veil/Cursebound Coil/vs.) | Canonical 9 adopted |
| 3.8 | "All Max" Act 4 mechanical drop | Render shorthand |
| 3.9 | Relic tiered naming ("Epic Relic") | Relic = rule-bender, no tier |

---

## Section 4 — Self-Review (Sonnet sub-agent + Orchestrator pass)

### 4.1 Mantıksızlık Testi

| Section | Verdict | Issue |
|---|---|---|
| 1.1 Class Anchor | PASS | Clean. |
| 1.2 Act 1 15-node | PASS | Karar #62 uyumlu. |
| 1.3 Threshold UI | PASS | 8 variant cover. |
| 1.4 Sub-room | WARN | Code-level guard: sub-room EncounterController fragment event emit ETMEMELI. |
| 1.5 Map Fragment | PASS | G key, reveal odds, TAB/MiniMap consistent. |
| 1.6 Skill Draft | WARN | Echo Imprint "every 3 combat rooms" + "max 4/run" math conflict (Act 1 = 6 combat = 2 trigger, 1 slot cap). CLARIFICATION NEEDED. |
| 1.7 Item Economy | WARN | **Forge node conflict** — MAP_ITEM_SYSTEM.md Forge = guaranteed destiny node, Karar #62 Act 1 node table'da YOK. Combined Item Act 1 craft path: sadece rare Shop Anvil. By design mı oversight mı? FLAG. |
| 1.8 Cross-Class | PASS | Flow consistent. |
| 1.9 Boss Gate | PASS | 8 = 6+2. |
| 1.10 Shattered Echoes | WARN | Hub spend catalog tanımsız (Phase 4+ vague). |

### 4.2 "Her Run Farklı Hissi" Testi

13 random channel sayıldı. Ideal 8-10. WARN.
- Channels 10-13 (Boss 3-Legendary choice, Shop inventory, Mystery fragment, Elite affix) low-impact + player-facing.
- Net effective player-decision channels ~9-10.
- Verdict: PASS with monitoring. Phase 2-3'te overcomplicate risk varsa kanal kesilir.

### 4.3 "Mekanik Şişirme" Testi

| System | Sub-systems | Verdict |
|---|---|---|
| Skill Draft 3-choice | 3 (weights, pity, reroll) | Acceptable |
| Combined Item recipe | 3 (drop, Forge, fixed recipe) | Acceptable |
| Cross-class passive + proc | 4 (commit-beat, cooldown, %35 damage, family tag) | WARN, LOCKED |
| Echo Imprint (4 slots) | 4 (trigger, category, cap, act unlock) | WARN |
| Boss gate 8-fragment | 3 (counter, reveal, mandatory/bonus) | Acceptable |
| Death Imprint | HIGH | Correctly deferred |

Cuttable: Tag Synergy = Phase 2+ confirmed (low philosophy fit, invisible complexity).

Overall risk: MODERATE. MVP manageable.

### 4.4 Açık Karar Gate'leri (Orchestrator / User)

1. **Forge room node conflict:** MAP_ITEM_SYSTEM.md Forge = destiny node, Karar #62 node table'da YOK. Options: (a) Forge node ekle Act 1'e (topology değişir), (b) Confirm Act 1 NO Forge — sadece Shop Anvil craft path (Combined Item Act 1 rare).
2. **Echo Imprint trigger vs slot math:** "Every 3 combat rooms" + "1 slot per act" + Act 1 = 6 combat. Proposal: trigger Skill Draft OFFER üretir, cap 4 total run; Act 1 max 1 slot (room 3 ilk trigger), room 6 ikinci trigger normal Skill Draft option ama no new Imprint slot.
3. **Architect meta-unlock:** Yeni class / keepsake / hub feature / story-only ending — hangisi?
4. **Shattered Echoes hub spend catalog:** Phase 4+ ne satar — class unlock / meta passives / cosmetic / starting relic?
5. **Image 13 handling:** Regenerate veya relabel — Stay/Break/Carry'yi Act 4 / Architect outcome area'sına taşı.

---

## Section 5 — Implementation Roadmap (faz sırası, kod YOK)

### 5.1 UI Foundation
- Kırık Taş Tablet MapPanel (TAB, StS-style 15-node, 5 state, act visual evolution)
- MiniMap 128×128 (persistent)
- Fragment counter HUD top-center "X / 8"
- Boss gate UI (count = 8 → unlock state)

### 5.2 Reward Flow
- Map Fragment spawn + G pickup + reveal roll + Skill Draft trigger chain
- Skill Draft 3-choice screen (offer weights, tier pool, pity, reroll)
- Door-lock guard (door stays locked until G pickup)

### 5.3 Sub-Room System
- EncounterController sub-room sequence (Faz-1: entry_chamber → pillar_arena)
- Mirror edge validator (compatible-edge ±2 tolerance)
- Macro clear detection (LAST sub-room only)
- Fragment spawn guard (macro clear only, NEVER sub-room clear)

### 5.4 Cross-Class
- Secondary class selection screen (2 random → pick 1, post Act 1 boss)
- +2 slot unlock (total 6)
- Cross-class passive layer activation
- Skill Draft pool weight shift per Act
- Act 2 boss → Cross-class Ultimate unlock gate

### 5.5 Boss Gate
- 8-fragment mandatory tracking (6 Combat + 2 Elite, branch excluded)
- Boss door unlock on count = 8
- Boss reward surface: Relic + Legendary 3-choice
- Act-specific reward unlock (Cross-class / Ultimate / Legendary tier / Architect)

### 5.6 Item / Craft Economy
- Component drop (Combat 20%, Elite 100% 2-choice)
- Forge Tab 3 (Combined craft, Legendary craft, 1-action-per-visit)
- 4-slot item inventory (Component/Combined/Legendary share)
- Shop inventory randomization (appearance weights)
- Relic award surface (Boss + rare Event)
- Shattered Echoes earn tracking

### 5.7 Death Imprint Echo Drop — POST-LAUNCH ONLY
- Death signature persistence
- Ghost echo actor
- Spawn rule (signature match)
- Reward +25-40 Shattered Echoes
- Anti-farming rules
- Implement ONLY after Phase 1-4 core stable
