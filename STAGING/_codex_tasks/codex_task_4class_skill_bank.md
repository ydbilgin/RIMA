# Codex Task — 4-Class Skill Design Bank (Warblade / Elementalist / Ranger / Shadowblade)

ACTIVE RULES: (1) think before deciding (2) min code, no speculation (3) surgical — design docs only, no code (4) BLOCKED if unclear.

NLM ACCESS:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

---

## Mission

Write **skill design docs** (NOT code) for 4 RIMA classes that have existing code base:
- Warblade (Rage resource)
- Elementalist (Mana + Element)
- Ranger (Focus)
- Shadowblade (Energy + Combo)

User has approved skill POOL of 12 per class (4 active + 4 passive/upgrade + 4 ultimate variants). User confirmed pool size = fine, active slot count is 4 (standard roguelite). Don't propose cuts.

## Required design per class (12 skills each, 48 total)

For each class:
- **4 Active Skills** (LMB equivalent variants, RMB main skill, F V Burst ultimate, R Mobility/Parry)
- **4 Passive Upgrades** (run-acquired buffs, modify resource generation or damage)
- **4 Cross-class Echo trigger conditions** (algorithmic auto-bond, what triggers this class's echo from another class)

For each skill, design:
- Name (English working title)
- Single-sentence behavior
- Resource cost
- Cooldown (if any)
- Damage type (Physical/Element/Veil/Echo/etc.)
- Family Tag (Fracture/Echo/Bleed/Pierce/Cut/Pressure/Strike — pick 1-2)
- Synergy hooks (what other skill/class this combos with)
- Visual/Audio signature (one-liner — placeholder OK, no asset prod)

## Mechanic bank usage

Read `F:\LaurethStudio\03_IDEAS\MECHANIC_BANK\` for inspiration. RIMA has memory ref `reference_mechanic_bank_studio.md` mentioning 68 mechanics in 10 categories. Pull ideas that fit each class's resource theme.

## Hard rules

- **DON'T write code.** Design docs only.
- Don't propose ditching skill count (12 stays)
- Family Tag pool: stick to canonical 9 (Fracture/Echo/Veil/Pierce/Bleed/Cut/Pressure/Strike + Rift meta) but recent Opus suggested cutting to 4 — if you propose using fewer, justify
- 4 active slot LIMIT — player has only 4 active at once, even with 12 pool
- Cross-class echo: T1-T4 algorithmic. T1 = entry-level (single proc), T4 = boss-tier. Don't propose all 12 echoes per class — 4 echo triggers max per class

## Required output structure

`STAGING/RIMA_4CLASS_SKILL_DESIGN_BANK.md`:

```
# Master Index

## Warblade — Rage resource
### Active 1-4
### Passive 1-4
### Echo triggers 1-4

## Elementalist — Mana + Element
[same structure]

## Ranger — Focus
[same structure]

## Shadowblade — Energy + Combo
[same structure]

# Synergy matrix
[Table: cross-class combos worth highlighting]

# Implementation priority order
[Day 1-7 plan if user decides to implement]
```

Effort: deep. This is design backlog seed for next 2-4 weeks. Quality over speed.
