# Opus rima-design Task — 4-Class Skill Bank Balance Review

ACTIVE RULES: (1) think before deciding (2) honest design judgment (3) cross-system balance audit (4) BLOCKED if unclear.

NLM ACCESS:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

---

## Mission

Codex produced `STAGING/RIMA_4CLASS_SKILL_DESIGN_BANK.md` — 48 skill designs across Warblade / Elementalist / Ranger / Shadowblade (12 each: 4 active + 4 passive + 4 echo triggers).

Your job: design balance audit + cross-class synergy quality.

## Review questions

1. **Family Tag distribution** — are 9 canonical tags (Fracture/Echo/Veil/Pierce/Bleed/Cut/Pressure/Strike/Rift) used evenly? Any tag overloaded? Any tag absent?

2. **Cross-class echo coherence** — T1-T4 echo triggers for each class:
   - T1 placeholder vs T4 boss-tier escalation
   - Are echo triggers genuinely procedural (auto-bond, not button) per Karar #27?
   - Visual clutter risk (every class echoes simultaneously?)

3. **Resource economy per class** — does each class's 12 skills genuinely USE the class's signature resource?
   - Warblade Rage: build via damage, spend on decisive
   - Elementalist Mana+Element: alternate Fire/Frost, Lightbreak cash
   - Ranger Focus: aim/precision economy
   - Shadowblade Energy+Combo: combo finisher reward

4. **Build identity differentiation** — if a player draws 4 active from Warblade's 12 pool, are there genuinely different builds (aggressive vs counter-play vs synergy-feeder)?

5. **Signature strength check** — does this skill bank support the "10 unique resource ritimleri" signature claim?

6. **Cut candidates** — which of the 48 skills are weak / forgettable / clone-y? Flag for removal.

7. **Synergy matrix completeness** — Codex provided synergy hooks per skill. Are cross-class combos covered? Best 5-10 standout combos?

8. **Echo Imprint Cascade integration** — top epic mechanic candidate (`STAGING/EPIC_MECHANIC_AND_CB_PIVOT_OPUS.md`). Does this skill bank naturally feed into "Death-as-Architect" mechanic, or is friction?

## Required output

`STAGING/OPUS_DONE_skill_bank_balance_review.md`:

```
# VERDICT
[Single paragraph: skill bank PASS for implementation / NEEDS REVISION / blocker found]

# 1. Family tag distribution audit
[Table per tag count, balance score]

# 2. Cross-class echo coherence
[T1-T4 escalation per class, clutter risk]

# 3. Resource economy validation
[Per class: does skill pool USE the resource?]

# 4. Build identity differentiation
[Sample 3 builds per class, are they distinct?]

# 5. Signature strength
[Does this back the 10-resource-economy thesis?]

# 6. Cut candidates
[List of 48 skills, mark CUT/POLISH/KEEP]

# 7. Top 10 cross-class combos
[Best synergy moments worth highlighting in playtest]

# 8. Echo Imprint Cascade integration
[Naturally feeds it / friction]

# 9. Implementation priority order
[Day 1-7 plan if user decides to commit]
```

Effort: deep. ~30-40 min. Cross-system judgment is your domain.

Hard rules:
- DON'T propose visual asset work (gen frozen tonight)
- DON'T propose more agent dispatches (orchestrator's call)
- DO be honest about clone-y skills
- DO suggest specific renames if skills feel generic
