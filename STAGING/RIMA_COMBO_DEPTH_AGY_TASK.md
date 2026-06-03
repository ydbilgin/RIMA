# RIMA Combo-Depth Analysis — AGY (combo design + genre benchmark)

ACTIVE RULES: (1) think before answering (2) dense, no filler (3) ANALYSIS ONLY — no files (4) flag uncertainty.

NLM ACCESS (worked before): uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>".

## READ FIRST
- `STAGING/RIMA_SKILL_MECHANIC_FINAL_REPORT_S6.md` · `STAGING/RIMA_CANON_BRIEF_FROM_NLM.md` (canon Commit-Beat, 9-family tags, 3+→Rift proc, states broken/burning/scar/marked).

## The question (user, verbatim intent)
"Should there be MORE skills? Are the skills COMPATIBLE with each other? Can they COMBO? Can MANY combos be made?" — this is about COMBO RICHNESS and synergy feel, not code architecture.

## Grounding — WB 12 demo skills → states (from descriptions)
Iron Charge=Stun · Cleave=Rage-scaled dmg · Deep Wound=Bleed · Sunder Mark=Sundered(armor) · Crippling Blow=Slow+Stun (→ chains Death Blow %600) · Earthsplitter=Broken+Stun · Blade Rush=knockback · Gravity Cleave=Pull(group) · Iron Counter=parry · Iron Crush=+dmg buff (→Bladestorm) · Battle Surge=Rage→heal · Death Blow=execute<30%. **Reality:** these combos are written in DESCRIPTIONS but NOT wired (tags inert, no chain system) → skills currently feel isolated.

## Your task (design / genre lens)
1. **Map the WB combo graph:** draw the actual setup→payoff combos latent in the 12 (e.g. Gravity Cleave pull → Earthsplitter AoE; Crippling stun → Death Blow execute; Sunder armor → big hit). How many GOOD combos exist? Which skills are ISOLATED (no setup, no payoff) and feel like filler?
2. **Is 12 the right number?** Benchmark combo density vs Hades (5 inputs, deep via Boons), Dead Cells (freeze→crit, bleed→melee), Risk of Rain 2 (item stacking), Devil May Cry (style chains). Does combo richness come from MORE skills or from MORE INTERACTIONS between fewer skills? Give a verdict for RIMA's 10-min demo and for the full game.
3. **What makes "many combos possible" feel good:** the state vocabulary (Stun/Bleed/Sundered/Broken/Pull/Mark...), readability, the "I discovered a combo" moment, build-defining vs micro-combos. What's missing from RIMA's current set to hit that?
4. **Concrete additions/cuts:** if some of the 12 are isolated filler, what would you re-author them into (a setup or a payoff)? Do we NEED more skills, or re-author existing ones to interlock?
5. **Cross-class combo dream (Phase-2):** the dual-class break + 9 families — paint the combo fantasy (e.g. Warblade Sundered + Elementalist Fire = ?). How many "wow" cross-combos should the full game target?

~800-1000 words, bulleted. Cite skill names. Be generative — propose the combo lattice you'd want.
