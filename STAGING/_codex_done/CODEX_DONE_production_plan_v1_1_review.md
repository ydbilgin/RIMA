# Production Plan v1.1 Re-Review — Codex Report

## Empirical Data
- list_characters: 17 total chars
- get_character sample: all 17 checked; 10 completed base-rotation chars, 7 failed chars
- Sampled smooth-state chars: []
- Avg gen/state observed: N/A
- Get_balance baseline: 2567 / 5000 live remaining generations; this differs from the plan's 2433 / 5000 baseline
- Metadata note: list_characters and get_character expose status, directions, size, view, rotations, available template animations, and current animations. They do not expose historical generation cost per character/state. All completed sampled chars show `animations: none`.
- Missing input note: requested lowercase `memory/reference_pixellab_v3_budget_formula.md`, `memory/feedback_pixellab_character_via_web_ui_v3.md`, and `memory/project_warblade_anim.md` were not present. Related available memory checked: `MEMORY/pixellab_budget.md`, `MEMORY/feedback_pixellab_direction.md`, `MEMORY/feedback_animate_character.md`, `MEMORY/PIXELLAB_TOOL_GUIDE.md`.

## Q1 Verdict
- Formula avg 32: INCONCLUSIVE
- Notes: The live 17-character inventory cannot validate real V3 smooth-state cost. The completed characters are base 8-direction rotations only, with no stored animation/state cost. The formula remains modeled; Pilot B real-cost logging is mandatory before firing the remaining 14 Warblade states.

## Q2 Verdict (Warblade 544 avg)
- Verdict: PASS_WITH_NOTES
- Notes: The proposed frame counts are realistic for a readable ARPG sprite set: idle 4-6, walk 6-8, strike 8-10, heavy 10-12, hit 3-4, death 6-10, signature 10-15. These ranges do not by themselves prove a 40+ gen/state average.
- The 544 avg model equals 17 x 32 gen/state and is usable as a planning reserve only. Because no empirical smooth-state characters exist in the sampled data, the 650 worst-case cap should stay active and Pilot B should remain a hard gate.

## Q3 Verdict (Mob 200)
- Verdict: Accept 200
- Notes: 6-8 states x 30 avg = 180-240, so 200 is a reasonable midpoint for one MVP mob if it must feel animated in the vertical slice.
- Budget-pressure option: a 4-state minimum set, idle/walk/attack/die, is viable for demo combat and should be the first cut if Q4 margin becomes production-critical. That cut would target roughly 120 gen.

## Q4 Verdict (Budget margin)
- Verdict: TIGHT
- Recommendation: Keep Ronin/Gunslinger/Elementalist hard-deferred, and add a conditional defer gate for Faz 2.2 mob unless Pilot B lands clearly under model. If preserving Act 2 + Act 3 headroom is the priority, cut the mob to 4 states or defer it.
- Notes: Using the plan baseline, 1264 / 2433 leaves 1169 gen, which is short of a modeled Act 2 + Act 3 need of ~1400 gen. Using the live balance, 1264 / 2567 leaves 1303 gen, still short of ~1400 by ~97 gen before retries. Worst case is tighter: 1550 / 2567 leaves 1017 gen. The current plan is viable for Act 1 plus Warblade, but not comfortably sufficient for full Act 2/3 continuation without discipline.

## Q5 Verdict (Pilot B gate)
- Verdict: PASS
- Notes: 3-state scope is the correct minimum because it samples movement idle, movement locomotion, and combat. A 2-state pilot would miss attack-state cost and would be weaker evidence for the 17-state Warblade plan.
- Gate threshold is realistic: <=35 confirms the modeled avg/worst band, 35-45 catches drift while still allowing a reduced plan, and >45 correctly blocks escalation.

## Overall Verdict
PASS_WITH_REVISIONS

## Suggested Revisions (varsa)
- Replace the hard-coded balance line with the live observed balance or explicitly label 2433 as the earlier planning baseline. Live check returned 2567 / 5000 remaining generations.
- Change margin verdict from "SUFFICIENT for Act 2/3 iteration + drift redo" to "TIGHT for Act 2/3; sufficient for current Act 1 + Warblade scope only."
- Add an explicit Faz 2.2 gate: if Pilot B avg/state >=35, use 4-state mob minimum or defer mob; if Pilot B avg/state <30, 6-8 state mob remains acceptable.
- State clearly that Q1 empirical validation is unavailable from current PixelLab metadata, so `STAGING/RIMA_PixelLab_BalanceLog.md` is the authoritative future evidence source.

## Açık Sorular (varsa)
- Is the balance discrepancy expected: plan says 2433 / 5000, live PixelLab says 2567 / 5000?
- Should Faz 2.2 mob be required for the school-demo vertical slice, or can static enemy prefab + VFX carry the first demo until Act 2 budget is clearer?
