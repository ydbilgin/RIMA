---
status: LOCKED
faz: 1
tarih: 2026-04-30
ozet: "R4 skill audit kararları"
---
# SKILL AUDIT — FINAL DECISION
Date: 2026-04-30 (v2, identity-fit reconciled)
Status: LOCKED
Authority: Claude (reconciled Claude + Codex audits, 3 audit rounds)

> **Note (2026-05-01 S43 dirty-pass):** Skill names below reflect canonical AS-OF audit lock (2026-04-30). 15 cosmetic renames applied 2026-05-01 for Brawler/Ravager/Hexer/Gunslinger street/foul/Western tone. Mapping table: `SINIF_VE_SKILL_KARAR_BELGESI.md` → "[S43] Dirty-pass renames". Mechanics unchanged. Examples: Mach Punch→Bully, Combo Chain→Crackjaw, Seismic Stomp→Curbstomp, Mass Hex→Foul Wave, Burning Ammo→Hot Lead.

Inputs:
- `STAGING/SKILL_AUDIT_CLAUDE_2026-04-30.md` (Claude generic audit)
- `STAGING/SKILL_AUDIT_CODEX_2026-04-30.md` (Codex generic audit)
- `STAGING/SKILL_AUDIT_IDENTITY_CODEX_2026-04-30.md` (Codex identity-fit audit) **NEW**
- `STAGING/SPRITESHEET_SKILL_IDENTITY_FEEDBACK_TO_CLAUDE_2026-04-30.md` (Codex visual feedback)
- `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md` (canonical)
- `TASARIM/MASTER_KARAR_BELGESI.md` (decisions, esp. #55, #56, #57, #58)

---

## v2 UPDATE — Identity-Fit Layer (CT-AUDIT-02)

User pushback: "did you check identity-fit for ALL classes consistently, not just helper count?" Answer: only partially in v1. CT-AUDIT-02 (Codex independent identity-fit audit) + Claude inline analysis confirm:

**Both audits independently reached the same identity verdicts (10/10 alignment):**

| Class | Identity verdict (Claude + Codex consensus) |
|---|---|
| Warblade | FIT |
| Elementalist | FIT |
| Shadowblade | OVER-MOB (3 phase-tools, 1 too many) |
| Ranger | OVER-MOB + UNDER-SET/CC (4 mobility, identity is distance/trap) |
| Ravager | FIT |
| Ronin | OVER-MOB + UNDER-SET (3 mobility, iaido = stillness) |
| Gunslinger | FIT |
| Brawler | OVER-DMG-INTERNAL (9 damage with overlapping families) |
| Summoner | FIT-WITH-CAP (utility IS identity, Diablo Necro model) |
| Hexer | OVER-SET (6 stack appliers, WoW Affliction autopilot risk) |

**Codex sharpened 3 calls Claude under-weighted:**

1. **Ronin under-supplied is SET (Iaido Stance / Opened state), not CC.** Freed mobility slots should boost stance/timing, not generic CC. Sharper read.
2. **Ranger freed mobility slots should EXPAND trap/mark payoff** — not just be cut. Wireline Trap (R4) should displace a numbered movement slot.
3. **Brawler Off-Balance (R4) is identity-critical for #57 ownership** — REVERSED from v1 MERGE. Off-Balance defines Brawler-side whiff/body-counter state.

### v2 changes to v1 locked decisions

| Item | v1 | v2 | Why |
|---|---|---|---|
| Brawler Off-Balance (R4) | MERGE into Guard Break | **KEEP** | #57 owns Brawler whiff/body counter; Off-Balance is the state name |
| Ranger merges | 3 merges, no slot replacement | 3 merges + **promote Wireline Trap (R4) to numbered #11** | Trap density UNDER-SET; freed mobility -> trap |
| Ronin Phantom Step (#7) | TIGHTEN | **CUT — slot stays empty (11 numbered)** | Identity OVER-MOB; pushes toward Shadowblade; decoy AI cost not justified |
| Ronin Iaido Stance | (no action) | **TIGHTEN — fold Stillness R4 power into stance** | UNDER-SET; stance is identity anchor |
| Hexer Pandemic / Mass Hex / Whisper Mark | Mass Hex+Whisper merge | + **explicit exclusivity rule** (only one auto-spread can be active per fight) | Stack autopilot risk |
| Gunslinger Point Blank Execute | REDESIGN (vague) | **REDESIGN as Heat-zero gate: <=2m + Heat <=20 = %400, post-fire Heat jumps to 60** | Empty Mag and Exposed Line already R4-owned; Heat-zero is the unclaimed gate |
| Summoner Soul Tax (R4) | (TIGHTEN) | **REDESIGN — tie to active command/sacrifice timing** | Delayed 6s summon hard to value |

### Three open decisions — LOCKED 2026-04-30 evening

After Codex CT-AUDIT-02 + Claude inline analysis + user approval:

1. **Ronin Phantom Step (#7) -> CUT.** No backfill. Ronin runs with 11 numbered + V + 3 base + 3 R4. Identity demands fewer-but-sharper skills. Decoy aggro mechanic Unity-cost not justified; verb overlaps Shadowblade Shadow Clone (#4).

2. **Ranger Wireline Trap (R4) -> promote to #11 numbered slot** (replacing Rift Step which merges into Hunter's Step). Ranger now has 3 traps with distinct geometry: Bone Trap (zone), Wireline Trap (line between 2 points), Predator's Mark (AoE mark). Freed mobility slot returns to identity (trap density). Quiver Pulse stays R4.

3. **Gunslinger Point Blank Execute (#12) -> REDESIGN as Heat-zero gate.** New canonical text: `<=2m + Heat <=20 = %400 hasar; ate sonrasi Heat anida 60'a cikar`. Reasoning: Empty Mag Burst (R4) already owns last-bullet payoff, Exposed Line (R4) already owns Heat-MAX payoff. Heat-zero is the third unclaimed state gate. Creates real choice point: vent heat to cool barrel and commit close, or ride hot for sustained DMG.

### Identity Anchors (LOCKED — what each class MUST own and AVOID)

Per Codex CT-AUDIT-02 cross-class differentiation map. Use as canonical for all future skill design and asset prompts.

| Class | OWNS | AVOIDS |
|---|---|---|
| Warblade | Sundered/Broken state, absorb-counter, weapon-impact armor crack | armor language by Ravager/Brawler |
| Elementalist | spell shapes (line/wall/orb/beam), element reactions, Lightbreak | physical traps (Ranger territory) |
| Shadowblade | Scar placement/collapse, phase-through geometry | generic teleport-slash (overlaps Ronin) |
| Ranger | trap lines, marks, kill zones, distance discipline | run-and-gun (Gunslinger territory) |
| Ravager | HP trade, low-life danger, frenzy chain | armor break (Warblade owned) |
| Ronin | sheathe timing, Opened state, single draw release | generic mobility (Shadowblade overlap) |
| Gunslinger | Heat rhythm, reload, slide+shoot, muzzle flash | mark/trap planning (Ranger territory) |
| Brawler | Cracked/Shattered state, launch/juggle, whiff body counter | weapon-armor break (Warblade), pre-draw counter (Ronin) |
| Summoner | minion bodies, corpses, sacrifice economy, command line | enemy-state stacks (Hexer territory) |
| Hexer | enemy stack accumulation, spread, curse phases (Debuff/Pressure/Overload) | minion bodies / sacrifice (Summoner territory) |

Counter archetype split (per #57): Warblade = absorb/break, Ronin = pre-draw timing, Brawler = whiff/evade body. **No overlap permitted.**

### Roster-level role budget (Codex roster-wide pass)

| Role | Total skills (10x12) | % | Read |
|---|---:|---:|---|
| DMG/BST | 48 | 40% | Healthy total, internally redundant in Brawler |
| CC | 20 | 17% | Good total, uneven distribution (Brawler high, Ronin/Summoner low) |
| SET | 17 | 14% | Healthy total, over-concentrated in Hexer/Summoner/Ranger |
| MOB | 16 | 13% | Globally OK but clustered in wrong identities (Ranger/Ronin/Ravager) |
| DEF | 14 | 12% | Good spread |
| RES | 5 | 4% | Numbered count low because resources live in basics/RMB |

**Roster-level conclusion:** mobility is not roster-wide excessive — it is misallocated. Ranger/Ronin should reduce, Gunslinger should keep (movement = fantasy).

---

## 0. SHOWSTOPPER — MASTER #56 VIOLATIONS

Codex caught what Claude missed: canonical skill doc still contains HP-execute clauses banned by **MASTER #56 (locked 2026-04-30):** "HP<%30 execute tum class'larda YASAK. Her class kendi state gate'ini kullanir."

These four canonical skill texts must be rewritten. **Mechanical change required** — not just visual.

| # | Skill | Current canonical clause | Required gate (per #56) |
|---|---|---|---|
| 1 | Warblade Death Blow | "SADECE HP<%30: %400 hasar" | Broken or Sundered gate |
| 2 | Shadowblade Severance | "low-HP execute line" | Scar collapse / 3+ Scars on path |
| 3 | Ranger Final Strike | "mark'li + low-HP execute" | Marked + Trapped (drop low-HP clause) |
| 4 | Ronin Flash Draw | chain bonus "Son hedef HP<%30 -> %200 execute cut" | Opened or Tension 100+ |

**Action:** Codex follow-up task to rewrite these four skill rows in `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md` per #56. Other skills with HP-related text (Ravager Death Wish "HP 1 altina dusemez" = self-state, OK; Gunslinger Point Blank Execute "<=2m" = range gate, OK) are compliant.

---

## 1. RECONCILIATION SUMMARY

### Where the audits agreed (high confidence)

Both audits independently flagged:
- Brawler, Shadowblade, Ravager as top-3 priority redesign classes
- Same 4 ground-impact / spin / sacrifice / stack-application redundancy clusters
- Same cross-class movement-skill convergence (Phase Step / Curse Step / Tactical Roll / etc.)
- Helper density risk in Shadowblade, Ranger, Summoner
- PixelLab 73-78% feasibility (peak frames yes, state language no)

### Where the audits diverged — resolutions

| # | Topic | Claude said | Codex said | Resolution | Why |
|---|---|---|---|---|---|
| 1 | Summoner Death Nova | MERGE into Mass Sacrifice | KEEP (Hexer dual) | **KEEP** | Cross-class synergy unique; Codex right |
| 2 | Brawler Repulse | MERGE into Shockwave | TIGHTEN | **TIGHTEN** | 4-target Charge=5 is unique trigger |
| 3 | Gunslinger Quickdraw | MERGE into Hip Shot | KEEP | **KEEP** | Rift Dash chain is real game value |
| 4 | Brawler Tornado Kick / Cyclone Drive | REDESIGN both | MERGE | **MERGE** | One spin skill cleaner |
| 5 | Ravager Undying Tenacity / Death Wish | KEEP both | MERGE | **MERGE** | Both are "I survive" — consolidate |
| 6 | Ravager Shatter Armor | KEEP/TIGHTEN | REDESIGN (state collision) | **REDESIGN** | Per #55 Sundered = Warblade only; rename to weapon notch |
| 7 | Hexer helper count | 4 (OK) | 8 (FLAG) | **FLAG** | Codex counts conservatively, structural risk real |
| 8 | Ronin helper count | 6 (RISK) | 3 (OK) | **5 effective** | Phantom Step cuts to RISK level |
| 9 | Summoner top-4 priority | YES | NO | **demote to top-5** | Helpers are functional, not redundant |
| 10 | Shadowblade Twin Carve | MERGE | not listed | **MERGE** | Overlaps Veil Strike + Phase Step |

---

## 2. FINAL VERDICT TABLES

### REDESIGN (22 skills) — visual contract or mechanic change required

**Mechanical (5) — MASTER #56 + #58 compliance:**
1. Warblade Death Blow -> Broken/Sundered gate
2. Shadowblade Severance -> Scar collapse finisher
3. Ranger Final Strike -> Marked + Trapped gate (no HP)
4. Ronin Flash Draw -> Opened or Tension 100 gate (chain bonus rewrite)
5. Gunslinger Point Blank Execute -> Heat-zero gate: `<=2m + Heat <=20 = %400 hasar; ate sonrasi Heat anida 60'a cikar`. Empty Mag (R4) and Exposed Line (R4) already own other Heat gates.

**State ownership (1) — MASTER #55 compliance:**
6. Ravager Shatter Armor -> rename + reframe (no "armor" language; Sundered = Warblade only). Suggested: "Bone Crack" — exposes vital point for Bloodlust Strike +%50 next hit, not a defense debuff.

**Brawler visual contract (8):**
7. Brawler Mach Punch — multi-arm afterimage barrage, Brawler stays in place
8. Brawler Combo Chain — explicit 4-pose (jab/cross/hook/uppercut), 5m forward translation
9. Brawler Pivot Hook — feet planted, hip rotation, single heavy hook
10. Brawler Aerial Rave — target clearly airborne, vertical chain
11. Brawler Shockwave Slam — small ground crack ring (smaller than Earthsplitter)
12. Brawler Seismic Stomp — straight-line ground crack (vs Shockwave's ring)
13. Brawler Unstoppable Force — too close to Overdrive; reframe as "Charge routing modifier" (Charge spends become combo extenders, not aura)
14. Brawler Overdrive (V) — phantom-arm afterimages around Brawler, NOT colored aura

**Shadowblade visual contract (6):**
15. Shadowblade Veil Strike — single reverse-grip slash, NO Scar
16. Shadowblade Veil Flicker — body teleport with phase silhouette, Scar at exit
17. Shadowblade Seam Rend — dash-through cut, Scar perpendicular to path
18. Shadowblade Phase Step — short body teleport, NO Scar (differs from Flicker)
19. Shadowblade Death Mark — sigil on target, NOT slash
20. Shadowblade Veil Burst — 4 phase teleports, cross-pattern Scars

**Ravager / Gunslinger visual (2):**
21. Ravager Berserk Mode (V) — blood ring + per-kill tick UI + hunched feral pose; differ from Bladestorm
22. Gunslinger Rift Dash — recolor dust/sparks, NOT purple void (bleeds into Shadowblade)

**Other identity REDESIGN (added in v2):**
23. Summoner Soul Tax (R4) — tie delayed summon to active command/sacrifice timing (current 6s passive too hard to value)

---

### MERGE (14 skills) — v2 reduced from 15 (Off-Balance reverted to KEEP)

| # | From | Into | Mechanic |
|---|---|---|---|
| 1 | Ravager Blood-Drunk Leap | Frenzied Leap | Fury%80+ empowered version |
| 2 | Ravager Undying Tenacity | Death Wish | Consolidate "I survive" — Death Wish gets near-fatal protection clause |
| 3 | Ravager Crimson Pact (R4) | Blood Pact (RMB) | HP-cost upgrade; +%30 dmg buff added |
| 4 | Warblade Blade Rush | Iron Charge | Line-pierce variant on charge (multi-hit residue) |
| 5 | Warblade Quake Slam (R4) | Earthsplitter | 3-wave variant on Earthsplitter |
| 6 | Brawler Tornado Kick | Cyclone Drive | Single spin skill — Cyclone keeps body-rotation identity |
| 7 | Brawler Shockwave Slam | Seismic Stomp | Already in REDESIGN — Seismic Stomp wins (line-launch); Shockwave Slam becomes the short-range variant |
| 8 | Ranger Multi-Mark | Predator's Mark | Scalable AoE (Charge = mark count tier) |
| 9 | Ranger Skirmish Shot | Tactical Roll | Move + shoot branch on RMB |
| 10 | Ranger Rift Step | Hunter's Step | Single mobility skill |
| 11 | Shadowblade Twin Carve | Veil Strike alt | 2-slash + back phase-step variant on LMB chain |
| 12 | Ronin Stillness (R4) | Iaido Stance | Stillness = held-stance Tension passive built into Iaido |
| 13 | Gunslinger Reload Roll (R4) | Reload Dance | One reload skill with slide variant |
| 14 | Hexer Cursed Mirror | Empathy | One reflection skill, scaled by phase |

**v2 reversal:** Brawler Off-Balance (R4) now KEEP, not MERGE. Per #57 it owns Brawler whiff/body counter state — identity-critical.

---

### PROMOTE (1 skill, v2 addition) — R4 -> numbered slot

1. **Ranger Wireline Trap (R4) -> #11 numbered slot** (replacing Rift Step which merged into Hunter's Step). Reasoning: Ranger UNDER-SET, freed mobility slot returns to identity. Wireline geometry distinct from Bone Trap (zone) and Predator's Mark (AoE). Snared+Marked combo feeds Marked Detonate.

---

### CUT (2 skills)

1. **Ranger Hawk Eye (R4)** — already marked "upgrade path, not new active skill" in canonical. Remove from R4 active list.
2. **Ronin Phantom Step (#7)** — v2 lock. No backfill. Ronin runs with 11 numbered + V + 3 base + 3 R4. Identity OVER-MOB; verb overlaps Shadowblade Shadow Clone; decoy AI implementation cost not justified.

---

### TIGHTEN (38 skills) — visible state / engine support before PixelLab production

These stay in design but require Unity-side state language before sprite production proceeds:

- **Warblade:** Iron Crush, Sunder Mark, Ironclad Momentum, Battle Surge, Iron Roar (R4) — Sundered overlay decal
- **Elementalist:** Blink (rebalance to axis), Frost Wall, Radiant Pillar, Element Charge — Light State band UI
- **Shadowblade:** Backstab Mark, Shadow Clone, Shadow Pin, Night Aperture — Scar persistent decal system
- **Ranger:** Hunter's Step, Wireline Trap, Quiver Pulse — Mark reticle, trap line geometry
- **Ravager:** Bloodied Roar, Bloodthirst, Iron Grab — Fury bar pulse, Blood Debt UI
- **Ronin:** Iaido Blur, Haste Dash, Wind Step, Phantom Step, Sakura Veil, Iai Pressure, Wind Read (R4) — Tension meter, Opened-state target indicator
- **Gunslinger:** Smoke Grenade, Suppression Fire, Burning Ammo, Empty Mag Burst (R4), Exposed Line (R4) — Heat bar, Exposed Line ground line
- **Brawler:** Repulse, Wall Slam Combo (R4), Pulverize (R4), Glass Strike (R4), Pin Strike (R4) — Charge pips (1-5), Cracked/Shattered/Pinned/Launched body state overlay
- **Summoner:** Spirit Surge (Dash), Commanding Strike, Soul Siphon Totem, Bone Tide (R4), Beacon Pull (R4), Soul Tax (R4 — REDESIGN if not workable) — command line VFX, sacrifice mark, corpse field decal cap
- **Hexer:** Agony, Pandemic, Empathy, Enervate, Mass Hex, Hex Overload, Whisper Mark (R4), Curse Bargain (R4) — Hex stack pips (1-10), Phase color bands

---

### KEEP (rest, ~70 skills + 10 V Bursts)

High-confidence keep list — strong identity, low redundancy, high pick value, PixelLab-feasible peak frames. No changes required before production.

(See `STAGING/SKILL_AUDIT_CLAUDE_2026-04-30.md` section 9 for itemized keep list.)

Notable preserved skills (against initial Claude verdict, kept after Codex reconciliation):
- Summoner Death Nova — Hexer dual synergy preserved
- Brawler Repulse — 4-target Charge=5 trigger preserved (TIGHTEN only)
- Gunslinger Quickdraw — Rift Dash chain preserved (TIGHTEN with recolor)
- Hexer Mass Hex — kept after gesture-distinction tightening

---

## 3. PRIORITY CLASS ORDER (Final)

For visual contract / production work:

1. **Brawler** (mechanical promise high, visual identity weakest, 8 REDESIGN items)
2. **Shadowblade** (Scar/Phase/Collapse verbs not enforced per skill, 6 REDESIGN items)
3. **Ravager** (Blood-Drunk Leap merge + Berserk redesign + Shatter Armor reframe)
4. **Ranger** (helper density 6, mark consolidation, 1 mechanical fix)
5. **Hexer** (8 helpers — stack autopilot risk, Cursed Mirror merge)
6. **Summoner** (helper-heavy but functional; minion swarm visual budget critical)
7. **Warblade** (1 mechanical fix + 1 merge — light work)
8. **Ronin** (1 mechanical fix + 1 merge — light work)
9. **Gunslinger** (1 visual recolor — Rift Dash; otherwise solid)
10. **Elementalist** (no major changes — strongest visual class as-is)

---

## 4. PRODUCTION GATE — STATUS

**HOLD** all PixelLab production sprites until:

1. ~~**Canonical doc updated**~~ ✅ **DONE 2026-04-30 (commit 22ed58c)** — all 5 mechanical fixes + Bone Crack rename + 14 merges + 1 promote + 2 cuts + 10 Identity Anchors + Hexer exclusivity rule applied.
2. **Visual Contract template** written: `TASARIM/SKILL_VISUAL_CONTRACT.md` (per skill: verb + shape + state + scale + forbidden overlap + PixelLab pose + Unity VFX list).
3. **First 4 priority classes** filled in Visual Contract: Brawler, Shadowblade, Ravager, Ranger.
4. **Unity state overlay spec**: Sundered decal, Scar decal, Mark reticle, Hex pips, Heat bar, Charge pips. Spec doc, not implementation yet.
5. **Brawler char_id idle/walk/dash anchor** locked (already in S43 active block per CURRENT_STATUS).

After gate clears: V3 keyframe REST workflow per Codex's earlier validation (15 gen/dir per skill, peak frames only). Per-class budget plan based on remaining 2414 PixelLab gen.

---

## 5. ACTION ITEMS (priority order)

### P0 — Codex canonical rewrite — DONE 2026-04-30 (commit 22ed58c)

CT-DOC-CANONICAL bundled task completed all P0 categories in one commit:
- 5 mechanical fixes applied (Categories A1-A5: Death Blow, Severance, Final Strike, Flash Draw, Point Blank Execute)
- 1 state-ownership rename (Category B: Shatter Armor -> Bone Crack)
- 14 merges applied (Category C; Off-Balance reverted to KEEP per v2 lock)
- 1 promote (Category D: Wireline Trap R4 -> Ranger #11)
- 2 cuts (Category E: Hawk Eye R4 + Ronin Phantom Step #7)
- 10 Identity Anchor annotations added (Category F)
- Hexer auto-spread exclusivity rule added (Category G)

QC verification:
- No HP-execute clauses remain in canonical doc (rg check)
- All 10 Identity Anchor lines present
- Build Eksenleri updated for Ravager (Bone Crack) and Ranger (Wireline Trap)
- Cross-class exportable pool updated for Ranger (Wireline Trap replaces Rift Step)

Open historical-cleanup item (low priority): the v3 audit summary table near doc bottom still references old skill names (Phantom Step, Cursed Mirror) as historical context. Codex correctly preserved these as history. If the v3 history block becomes confusing, mark with "[superseded by v2 audit 2026-04-30]" annotation later.

### P1 — Claude design tasks

4. **Visual Contract template** (`TASARIM/SKILL_VISUAL_CONTRACT.md`): per-skill schema (verb / shape / state-applied / state-consumed / scale tier / forbidden overlap / PixelLab pose / Unity VFX list / 128px readable Y/N).

5. **Fill Visual Contract for top 4 priority classes** (Brawler, Shadowblade, Ravager, Ranger). 12 + V + 3 base each = ~64 contracts.

6. **Unity state overlay spec** (one doc, ~1 page): list of state decals/UI/pulses needed before sprite-skill integration.

### P2 — Production prep

7. After P0 + P1: PixelLab Brawler animation pilot (Cracked → Shattered combo chain pose set, ~60 gen).

8. After Brawler pilot: Shadowblade Scar mechanic validation (Veil Flicker exit-Scar + collapse — needs Unity Scar decal first).

### P3 — Cleanup

9. Archive `STAGING/SPRITESHEET_SKILL_IDENTITY_FEEDBACK_TO_CLAUDE_2026-04-30.md` -> `ARCHIVE/`
10. Archive `STAGING/SKILL_SHEET_DECISION_2026-04-30.md` -> `ARCHIVE/`
11. Archive `STAGING/SKILL_AUDIT_CLAUDE_2026-04-30.md` -> `ARCHIVE/` (after canonical updates land)
12. Archive `STAGING/SKILL_AUDIT_CODEX_2026-04-30.md` -> `ARCHIVE/` (after canonical updates land)
13. Update `CURRENT_STATUS.md` Active Block to reflect this audit lock + production gate

---

## 6. SUMMARY

User's instinct ("skills feel similar, especially helpers") is **validated by both audits**. The fix is mostly visual contract + 1 mechanical compliance pass (MASTER #56), not a redesign of the skill set.

Final numbers:
- 5 mechanical changes (4 #56 + 1 #55)
- 16 visual contract redesigns
- 15 merges (consolidations, not deletions)
- 1 cut (Hawk Eye, already non-active)
- 38 tightens (Unity engine state work)
- ~115 skills KEEP

Two audits independently confirmed the same priority class order in the top 3. Where they differed, Codex's mechanical perspective (cross-class synergy preservation, MASTER doc compliance) won most reconciliations. Claude's visual identity perspective drove the Brawler / Shadowblade / Ravager redesign list.

**Production gate held** until canonical doc rewrites + Visual Contract template + top-4 contracts filled. Estimated 2-3 working sessions to clear gate.

