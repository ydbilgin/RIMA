# Cross-Class Visual+Mechanical Design — Production Cost & Scope Analysis

## Context

RIMA: 2D pixel art roguelite, top-down 35°, chibi 64x64, ETG × Alabaster Dawn fusion, dark fantasy. 10 playable classes. Each run: player picks PRIMARY + CROSS-CLASS (10×9 = 90 combinations).

**New locked architecture (Yol A):** Weapons decoupled from body sprite. Body (weaponless) + separate weapon sprite + Unity transform-based attach. Pattern: Hades/ETG/Brotato. Enables weapon swap, attach/detach, transformation trivially. Cross-class can now be expressed VISUALLY for the first time.

## Class Weapon Inventory (decouple sprites)

- Warblade: greatsword (two-hand)
- Ranger: compound bow (left hand)
- Shadowblade: twin blades (reverse-grip)
- Elementalist: rune disc (palm-attached, sprite + Unity VFX glow)
- Ravager: dual hatchets
- Ronin: katana + sheath (sheath stays in body sprite)
- Gunslinger: dual pistols
- Brawler: UNARMED (bare fists, body part)
- Summoner: soul lantern (left hand)
- Hexer: grimoire (chest) + curse staff (right hand)

## Production Constraints

- Single developer (Yasin)
- School deadline 1-2 weeks
- MVP target: 1 class + 1 mob + 1 room playable
- Full roster (10 class × 6 mob) after MVP
- Remaining PixelLab gen budget: ~250
- Unity weapon-attach: Level 1 (orbit/parent) done → Level 2 (per-frame anchor) planned
- Existing locked rules: #80 silhouette bible, #99 weapon visibility (no puff), #71 single-state weapon (Ronin exception), #59 VFX vs sprite separation, #42 run anim only (no walk), #109 ambient idle per class

## 7 Design Options to Rank

**1) Off-hand Weapon (dual-wield):** Primary weapon (right) + cross-class weapon (left palm/off-hand). Toggle button switches active weapon.

**2) Ghost Echo:** Translucent cross-class character follows primary. Echo auto-casts or cooldown-casts cross-class skill.

**3) Skill Borrow (no visual):** Cross-class = 1-2 extra skill buttons. No visual change.

**4) Stance Toggle (transform):** Toggle stance = primary weapon goes to back, cross-class weapon becomes active. Two full attack sets per combo.

**5) Off-hand Summon (limited use):** Cross-class skill triggered = off-hand item appears from belt/back, 1-2 skills used, returns. Primary weapon stays active throughout.

**6) Echo Form (transformation ulti):** Echo Meter fills via combat. Full meter = brief hybrid form (primary + cross-class weapon floating + VFX). Limited duration, ulti payoff.

**7) Passive only:** Cross-class = passive bonus (e.g., burning hits). No visual change.

## Questions I Need Answered

For EACH option, give:

1. **PixelLab gen cost estimate:** How many new generations needed for full 10×9=90 combo roster? Show math (per-class anim, per-weapon-swap anim, etc.)

2. **Unity code hours:** Implementation work, weapon-attach upgrade needs, animation state machine complexity

3. **Animation clip count:** Extra clips per class beyond MVP baseline

4. **Balance complexity:** How hard to balance 90 combos? Combinatorial explosion risk?

5. **MVP fit (1-2 week scope):** Does it fit current MVP (1 class + 1 mob + 1 room)? Or v2 polish phase?

6. **Scaling:** Linear scaling (each new class = +N work) vs combinatorial (each new combo = +N work)?

## Output Format

```
RANKING BY PRODUCTION COST (cheap → expensive):
1. [Option X] — Gen: N, Code: Mh, Anim: K clips, MVP-fit: YES/NO
2. [Option Y] — ...
...

RANKING BY MECHANICAL DEPTH (shallow → deep):
1. ...

HYBRID PROPOSAL: [e.g., 5+6 or 5+3] — why this combo is optimal for RIMA's constraints

MVP RECOMMENDATION: [which option for 1-2 week scope]
V2 RECOMMENDATION: [which option for polish phase]

REASONING: 2-3 paragraphs on production trade-offs
```

Be specific with numbers. This is a final design lock decision.
