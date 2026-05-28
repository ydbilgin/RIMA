# Opus Skill Bank Balance Review — VERDICT (S93 night)

(Inline output from rima-design sub-agent — written here by orchestrator per sub-agent rules.)

# VERDICT

**NEEDS REVISION — minor.** The 48-skill bank is structurally sound and ready for SkillDesignDoc schema implementation, but has **4 concrete weaknesses** to fix before Day-1 code:
1. Family tag distribution lopsided — `Echo` and `Pierce` overloaded, `Cut/Bleed/Rift/Strike` under-used
2. T1/T2/T3 echo template **mechanically identical across all 4 classes** — signature claim undermined
3. Elementalist + Shadowblade have 2 weak/clone-y entries each
4. **Zero hooks for Echo Imprint Cascade (Death-as-Architect)** — top epic signature gap

Fix 4 issues → PASS.

# 1. Family tag distribution audit

| Tag | Active | Passive | Echo | Total | Balance |
|---|---:|---:|---:|---:|---|
| Echo | 6 | 3 | 12 | **21** | OVERLOADED (system tag) |
| Pierce | 3 | 2 | 4 | 9 | High but Ranger-justified |
| Pressure | 4 | 2 | 4 | 10 | Healthy |
| Fracture | 4 | 2 | 4 | 10 | Healthy, Warblade-anchored |
| Veil | 3 | 1 | 4 | 8 | OK, Shadowblade-anchored |
| Bleed | 1 | 2 | 4 | 7 | UNDER — only Shadowblade |
| Cut | 2 | 1 | 1 | 4 | UNDER — weakest |
| Strike | 4 | 1 | 2 | 7 | UNDER (should be on every LMB) |
| Rift | 1 | 2 | 8 | 11 | All T4 only, never active-produced |

**Recommendation:** Reclassify into **7 damage/state tags** (Fracture/Cut/Bleed/Pierce/Veil/Pressure/Strike) + **2 meta-tags** (Echo, Rift). Karar #65 candidate update.

# 2. Cross-class echo coherence — IDENTICAL TEMPLATE PROBLEM

T1-T4 escalation **correct structurally** (Karar #27 procedural auto-bond):
- T1: LMB beat 3 lands, 35%, 1.2s ICD
- T2: 15-25% basic hit chance, 25%, 0.8s ICD
- T3: Drafted active matching tag, 50%, skill CD
- T4: 3 family tags incl signature, 100%+freeze, 4s post-proc ICD

**Problem:** Codex literally repeats "another class lands LMB beat 3" for all 4 classes. Only proc visual changes.

**Fix:** Pick option B — trigger anomaly for 2 classes (melee bracket):
- Warblade T1 → triggers on **parry success** (Iron Counter Step)
- Shadowblade T1 → triggers on **CP-3 finisher land**
- Elementalist + Ranger keep LMB beat 3

Gives **2 echo trigger rhythms across 4 classes** — variety + still teachable.

**Visual clutter mitigation:**
- HARD CAP: max 2 echo VFX simultaneously (queue/drop 3rd)
- T2 Resonance ICD → **per-target** not global

# 3. Resource economy validation

- **Warblade Rage:** PASS — clean build/spend
- **Elementalist Mana+Element:** MOSTLY PASS, **1 hole**: Lightbreak barely present in actives. **Fix:** Rift Bolt Cycle 3rd bolt = "Lightbreak proc on resonance ≥3"
- **Ranger Focus:** PASS — strongest of 4 (distance-discipline rhythm)
- **Shadowblade Energy+Combo:** PASS with friction. **Shadow Dance 80 Energy + 5 CP too steep**. Drop to 60 Energy + 5 CP OR raise Energy cap to 120.

# 4. Build identity differentiation — passes when measured correctly

Active draft DOESN'T exist (4 actives fixed per class). Differentiation comes from passives + cross-class slots + relics. With this lens: **3 distinct builds per class verified**:
- Warblade: Tempo Hammer / Anvil / Fracture Battery
- Elementalist: Pure Caster / Reaction Detonator / Lightbreak Cycler
- Ranger: Trap Architect / Sniper / Kite Burst
- Shadowblade: Combo Finisher / Scar Collapse Spam / Phase Tank

# 5. Signature strength check

**Passes for 4-class proof. Does NOT prove 10.** 4 rhythms here genuinely distinct. Remaining 6 classes (Ronin/Gunslinger/Ravager/Hexer/Brawler/Summoner) must each land similarly distinct economy. **Risk: collapse into "Rage variant" or "Mana variant".**

**Ronin stress test critical:** must land **5th distinct rhythm** (e.g., "anti-resource: lowest Tension = highest first-strike damage").

# 6. Cut candidates — 0 CUT, 5 POLISH, 43/48 KEEP

| Class | Skill | Verdict |
|---|---|---|
| Warblade | Execution Weight | POLISH — simplify "below 35% + 3 conditions" |
| Elementalist | Blink Sigil | POLISH — detonates on any next cast (drop element requirement) |
| Elementalist | Prism Economy | POLISH — pick regen bonus only, drop negative |
| Shadowblade | Shadow Dance | POLISH — 80E+5CP gate too steep |
| Shadowblade | Hemorrhage Arithmetic | POLISH — drop RNG, make deterministic ("every 3rd Bleed tick on Scarred = +1 CP") |

# 7. Top 10 cross-class combos

1. Sunder Hook → Pinning Kill Line → Aim Shot Bank (Warblade+Ranger)
2. Pinning Kill Line → Convergence Orb (Fire then Frost) (Ranger+Elem)
3. Iron Counter Step parry → Phase Riposte exposed back (Warblade+Shadow)
4. Blink Sigil → Backstab Mark detonation (Elem+Shadow) — **best two-class one-rhythm moment**
5. Bladestorm Vow refreshes Fracture → T4 Armor Break Bond proc
6. Trinity Storm + marked targets → T4 Lightbreak Bond (Elem+Ranger) — **boss-tier**
7. Hunter's Step backstep → snare → Sunder Hook pulls into snare (Ranger+Warblade reverse flow)
8. Cut Ledger 3 stacks → Backstab Mark + T3 doubled (Shadow solo)
9. Phase State Mastery dual reactions (Elem solo + Warblade CC)
10. Iron Rhythm beat 3 → 4 echo T1s → T4 from single basic chain (theoretical max-density)

Missing: 5 temporal combos (Day-7 QC).

# 8. Echo Imprint Cascade integration — **MAJOR GAP**

**Zero hooks for Death-as-Architect** in 48 skills. Most important finding.

3 affordances needed:
1. **Class-signature death imprint** — what does each class's death leave behind?
2. **Imprint interaction tags** — skills respond to prior-run imprints
3. **Cursed Room hook** — when 3+ death imprints accumulate, skills behave differently

**Fix (4 one-liners, negligible cost):**
- Warblade Execution Weight + "On death, 2m Sundered shockwave field for next run"
- Elementalist Prism Economy + "On death, last-cast element 3m lingering field for next run"
- Ranger Clean Distance + "On death, last 2 traps persist inert; first hit re-arms"
- Shadowblade Hemorrhage Arithmetic + "On death, Rift Scar at position; next run Phase-Riposted for Energy refund"

Plus: Day-1 SO schema reserves `DeathImprintBehavior` field.

# 9. Implementation priority — 7-day plan (Codex base + Opus modifications)

| Day | Task |
|---|---|
| 1 | SkillDesignDoc SO schema (Tag[1-2] + MetaTag[0-1] + DeathImprintBehavior) — reclassify 9→7+2 |
| 2 | Warblade + Ranger active mapping (clean, no changes) |
| 3 | Elementalist + Shadowblade actives — APPLY POLISH first (Blink Sigil simplify, Shadow Dance cost, Lightbreak hook on Rift Bolt 3rd) |
| 4 | 16 passives + Death Imprint behavior on each class's Passive #4 |
| 5 | Echo T1-T3 algorithmic + trigger anomaly for Warblade/Shadowblade |
| 6 | T4 Rift Proc detection + per-target ICD + 2-VFX cap |
| 7 | QC pass + 5 temporal combos + Shadow Dance cap validation + Cursed Room smoke test |

# Conflicts with locked rules

**NONE except 1 candidate update:**
- Karar #65 (9 family tags) — Opus recommends reclassify 9 → 7+2. Needs user approval.
- Karar #24, #27, #80, #143, #147 — untouched

# Orchestrator next step

Wake user with verdict + 4-class skill bank. Single decision needed:
1. Approve POLISH list (§6)
2. Approve trigger anomaly for Warblade/Shadowblade (§2)
3. Approve tag reclassification 9 → 7+2 (§1)
4. Approve Death Imprint passive additions (§8)

If approved → dispatch rima-doc + Codex SO schema. If not → re-dispatch rima-design with objections.
