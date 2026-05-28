# Cross-Class Visual Design — Final Karar

## Önerilen Tasarım

**Staged hybrid: MVP = Option 5 (Off-hand Rift Summon) — minimal/single-pair scope. V2 = Option 5 full roster. V3 polish = Option 6 (Echo Form) layered on top.**

Cross-class = primary class reaches into the rift, pulls the other-self's weapon for the duration of the borrowed skill, returns it. Primary silhouette dominant 95% of time; cross-class flashes for 1-2sec during active skill use.

## Rationale

- **Identity (Karar #80) preserved:** Primary class silhouette uninterrupted outside the brief summon window. No permanent dual-wield disruption.
- **Lore fit:** "Rift" is RIMA's name. Pulling another self's weapon FROM the rift is the strongest thematic fit of all 7 options — turns a mechanic into worldbuilding.
- **Locked-rule pass:** Survives #80 silhouette, #99 no-puff (rift VFX is explicit appearance, not magic poof), #71 single-state weapon (primary untouched; cross is temporary not state-changed), #59 (rift glow = Unity VFX, weapon = sprite), #77 tonal (echoes of fallen mages).
- **Linear scaling:** 10 base summon anims + 10 weapon sprites (Yol A already provides). NOT 90.
- **Stress-test survived:** Brawler-cross-Gunslinger PASS, Hexer-cross-Warblade edge-case (staff displacement rule), Ronin-cross-Shadowblade PASS.

### Eliminated alternatives (top 3 numerical compare)

- **Option 4 (Stance Toggle):** Codex 1440 gen @ 4-dir (2880 @ 8-dir per Karar #114). Auto-fail. Combinatorial.
- **Option 1 (Off-hand dual-wield):** 68-136 gen + permanent silhouette disruption + balance hell across 90 combos. Fails #80.
- **Option 3 (no visual):** Cheapest but wastes Yol A's entire architectural win. Demo to school = flat.

## Production Cost

**MVP (1-2 weeks, single class + 1 cross pair):**
- Gen: **~5 PixelLab gen** (1 rift-summon gesture, 1 class, 8-dir bracket = 2-3 gen per Karar #114 + cross weapon sprite reuse from Yol A inventory). Possibly **0 gen** if MVP reuses existing run-anim hand-raise frame + Unity VFX particle.
- Code: **12-18h** Unity (cross-skill state machine, weapon transform-attach trigger, summon/dismiss VFX hook, skill-data schema with `source_class_id`/`prop_id`/`window_ms` fields).
- Anim: **+1 clip** (rift-summon gesture for the MVP primary class). Cross-class attack visual = reuse primary attack with swapped weapon sprite via Unity transform.

**V2 (full 10-class roster):**
- Gen: ~40-80 gen (10 classes × 1 summon anim × 8-dir bracket; weapons exist). Within 250 budget.
- Code: +10-15h to generalize state machine across all classes.

**V3 (Echo Form layer):**
- Gen: +40-60 gen (form aura overlays + UI burst). Reuses Opt 5 weapon library.
- Code: +30-50h (meter, duration, transform state).

## MVP Etkisi

**Fits school deadline.** Codex flagged Opt 5 as "YES for one prototype pair, NO for full roster" — exactly the MVP scope: 1 class + 1 mob + 1 room + 1 cross-pair proof.

Concrete MVP cut:
- 1 primary class (likely Warblade — locked silhouette already designed)
- 1 cross-class (Elementalist — VFX-heavy showcase moment)
- 1 cross-skill button (rune-disc fire-blast summon, 2sec window, 8sec cooldown)
- 0 new sprites if rift-summon = run-anim hand-raise reuse + Unity VFX
- Demo reads: "I'm a Warblade who can briefly channel Elementalist fire through the rift"

**Faz 1.** Not v2.

## Karar Önerisi

**Lock as Karar #122: Cross-Class Visual Mechanic — Rift Summon Staged.**

Memory file integration:
- New: `MEMORY/project_cross_class_rift_summon.md` (mechanic spec)
- Updates: 
  - `MASTER_KARAR_BELGESI.md` — add #122 entry, note compatibility with #71, #80, #99, #59
  - Karar #80 silhouette bible — append "brief summon window exempt from primary silhouette dominance rule"
  - Karar #99 weapon visibility — append "cross-class rift summon is explicit rift-VFX appearance, not puff; weapon must visibly travel from rift particle to hand within 0.3sec"
- Schema for skill data must include: `source_class_id`, `prop_sprite_id`, `summon_window_ms`, `cooldown_ms`, `vfx_id`

**Hard constraint locked into design:** Summon ≤0.3sec. Snappy. Slow draw = gameplay flow break.

## Codex vs Opus Sentez Notu

**Codex önerdi MVP = Option 3 (no visual). Ben Opus diyorum MVP = Option 5 minimal (single pair).**

Sebep: Codex'in 56-gen Opt 5 math'i full 10×9 roster için — yani **full coverage**. MVP scope = 1 class + 1 cross, dolayısıyla math 5-10 gen'e düşer, hatta sıfıra (anim reuse + VFX). Bu seviyede Opt 5 = Opt 3 maliyet, ama görsel payoff demo'da var.

Codex'in **isabetli uyarısı**: skill data schema'sı Opt 5'i "later attach" edebilmeli. Ben de bunu benimsedim — MVP schema cross-class kaydı tam tutmalı (source_class_id, prop_id, vfx_id) bile MVP'de görsel render etmiyorsa. Codex bu mimari foresight'ta yüzde yüz haklı.

Codex 4-dir varsaymış — RIMA 8-dir locked (Karar #114). V2 full roster gen cost ~iki kat: 80-120 gen aralığında, hâlâ 250 budget'a sığar. Flag eklendi.

**Net divergence:** Codex risk-averse (Opt 3 MVP). Ben Yasin'in tek developer + okul demo gerçeğini hesaba katarak — visual moment'siz MVP "flat" demo = düşük etki. 1 anim ek maliyet, devasa visual payoff.

**Final: Karar #122 — Rift Summon, MVP minimal, V2 full, V3 Echo Form.**
