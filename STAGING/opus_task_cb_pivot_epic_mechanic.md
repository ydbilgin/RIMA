# Opus rima-design Task — CB Pivot Deep Analysis + Epic RIMA Signature Mechanic Brainstorm (COMBINED)

ACTIVE RULES: (1) think before deciding (2) honest verdict no flattery (3) compare RIMA vs CB on equal terms (4) BLOCKED if unclear.

NLM ACCESS:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

---

## Mission — two parallel deep analyses

User is sleeping, wants morning verdict on TWO connected strategic questions:

### Question A: CB Pivot — Should RIMA pivot to CircuitBreaker?

- Read **`F:\LaurethStudio\02_GAMES\CircuitBreaker\`** fully (all design docs, README, vision)
- Read **`STAGING/CB_VISION_DOC_draft.md`** (37KB CB design synthesis)
- Read **`STAGING/CODEX_TASK_cb_*_DONE.md`** files (prior CB review iterations)

Honest analysis:
- What's CB locked vs open?
- What's missing to make CB ship-worthy?
- Compare CB pitch clarity vs RIMA pitch clarity (prior verdict said CB has tighter pitch)
- If RIMA were dropped tonight, could CB ship in 6-12 months solo dev?
- If NOT, what gaps?

### Question B: Epic RIMA Signature Mechanic Brainstorm

Prior Opus verdict (`STAGING/RIMA_MECHANIC_ANTI_GENERIC_OPUS.md`) said RIMA's signature = 10-class × 10 unique resource economies, but unproven. User wants EPIC mechanic ideas — what would elevate RIMA beyond "Hades-clone-with-more-classes" into something genuinely talked-about.

Use **mechanic bank**: `F:\LaurethStudio\03_IDEAS\MECHANIC_BANK` — pull out ideas that fit RIMA's 10-class roguelite scaffold.

Generate 5 EPIC signature candidate mechanics:
- Each must be: implementable solo dev, original (not Hades/DC/Spire clone), proves "RIMA is generic değil"
- For each: pitch single-sentence, integration cost estimate, risk
- Don't just brainstorm — RANK them by signature_strength × ship_realism

## Required output structure

`STAGING/EPIC_MECHANIC_AND_CB_PIVOT_OPUS.md`:

```
# PART A: CB PIVOT VERDICT
[Single recommendation: RIMA continue / RIMA pivot / Hybrid (one team, parallel both)]

## A1. CB design state honest read
## A2. CB ship realism vs RIMA ship realism
## A3. Pivot cost (RIMA 6-month progress preserved or lost?)
## A4. Final pivot recommendation

# PART B: EPIC RIMA SIGNATURE MECHANIC BRAINSTORM

## B1. 5 epic signature candidates
[Per candidate: pitch + integration cost + risk + signature strength score 1-10 + ship realism score 1-10]

## B2. Ranked recommendation
[Top 1 should be the "go signature" — actionable for morning dispatch]

## B3. Mechanic bank entries that triggered each candidate
[Trace each candidate to source idea]

# PART C: COMBINED RECOMMENDATION
[Single concrete morning action item — possibly: RIMA continue + add Mechanic X as core feature]
```

Effort: deep. Read mechanic bank thoroughly. Web search OK for industry mechanic comparisons. Don't propose more code dispatches yet — design first.

## Hard constraints

- DON'T propose visual asset gen (frozen tonight)
- DON'T propose more agent dispatches in your output (that's orchestrator's call)
- DO be honest — if CB pivot is actually right call, say so
- DO consider sunk cost (10 character anchor + Brush V1 + Multi-Layer Painter + 78 tests in RIMA)
