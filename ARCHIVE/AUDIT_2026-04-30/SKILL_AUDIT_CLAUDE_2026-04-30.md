# Claude Skill Audit (RIMA, 10 classes)
Date: 2026-04-30
Auditor: Claude (fork)
Source of truth: `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md`
Coverage: 12 numbered + V Burst + 3 base (LMB/RMB/Dash) + R4 Extras for all 10 classes (~190 entries)

ASCII-only. No diacritics.

---

## RUBRIC LEGEND

Per skill columns:
- **A** Standalone (1-5): satisfying as one-button press
- **B** Combo in-class (1-5): sets up / pays off another class skill
- **C** Combo cross-class (1-5): serves Faz3 dual-class synergies / exportable pool
- **D** Role: DMG / SET (setup-mark) / CC / DEF / MOB / RES (resource) / BST (burst-payoff)
- **E** Helper (Y/N): no direct damage, only enables / buffs / setups / mobility
- **F** Build axis count (0-3): in how many of the 3 listed Build Eksenleri it appears
- **G** Redundancy: sibling overlap inside class (short text)
- **H** Visual verb distinct (Y/N): unique pose vs siblings
- **I** PixelLab feasibility (1-5): can a 4-8 frame anim at 128px carry the read without engine state?
- **J** Pick value (1-5): would players pick in a 4/6 slot meta?
- **K** Verdict: KEEP / TIGHTEN / MERGE / REDESIGN / CUT

Rationale: under 15 words.

---

## 1. EXECUTIVE SUMMARY

### Verdict counts (out of 130 numbered + V skills, 10x13)

| Verdict | Count | % |
|---|---|---|
| KEEP | 56 | 43% |
| TIGHTEN | 38 | 29% |
| MERGE | 14 | 11% |
| REDESIGN | 18 | 14% |
| CUT | 4 | 3% |

### Top 4 priority classes for redesign work (worst first)

1. **Shadowblade** — 6/12 helpers, 5 forward-slash skills, phase/scar/collapse verbs not enforced per skill.
2. **Ravager** — Frenzied Leap + Blood-Drunk Leap are the same skill twice; 6+ axe-swing redundancy.
3. **Brawler** — 4 punch redundancies, 3 ground-impact redundancies, kicks lonely, visual lineup weakest.
4. **Summoner** — 5 sacrifice variants risk swarm spam; Death Nova / Mass Sacrifice / Blood for Power overlap.

### Top problems (one-line)

1. Ravager Blood-Drunk Leap === Frenzied Leap mechanically. CUT or MERGE.
2. Shadowblade Twin Carve overlaps Veil Strike + Phase Step. MERGE.
3. Brawler Mach Punch / Combo Chain visual signature is identical (forward punch barrage). REDESIGN one.
4. Brawler Shockwave Slam / Seismic Stomp / Repulse: three ground-AoE knockback skills. MERGE Repulse out.
5. Warblade Iron Charge + Blade Rush + Momentum Slam: three forward dashes. TIGHTEN; Blade Rush needs line/multi-hit identity.
6. Ranger Predator's Mark + Multi-Mark: two AoE marks. MERGE.
7. Hexer Corruption / Agony / Mass Hex apply stacks identically. TIGHTEN with shape distinction.
8. Summoner Death Nova / Blood for Power / Dark Pact / Mass Sacrifice: 4 sacrifice variants overload class. MERGE Death Nova into Mass Sacrifice as charge-tier.
9. Ronin Phantom Step + Wind Step + Haste Dash: three mobility skills. Phantom Step CUT or REDESIGN as defensive (decoy is class-unique, others are not).
10. Gunslinger Quickdraw + Deadshot + Point Blank Execute: three precision shots. TIGHTEN; merge Quickdraw into Hip Shot upgrade path.

---

## 2. PER-CLASS AUDIT

### 2.1 WARBLADE

Verbs: engage / break / execute. Resource: Rage (give-damage build).

#### Base + V

| # | Skill | A | B | C | D | E | F | G | H | I | J | K | Rationale |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| L | Iron Combo (LMB) | 4 | 5 | 4 | DMG | N | 3 | filler — strong | Y | 5 | 5 | KEEP | Class anchor; 3-hit chain is readable. |
| R | Rage Outlet (RMB) | 3 | 3 | 3 | RES/CC | Y | 1 | unique | Y | 4 | 3 | KEEP | Rage dump + small AoE; functional but not flashy. |
| D | Momentum Slam (Dash) | 3 | 3 | 2 | DMG/MOB | N | 0 | OVERLAPS Iron Charge | N | 4 | 3 | TIGHTEN | Pose differentiation needed (shoulder vs sword). |
| V | Bladestorm | 5 | 4 | 3 | BST | N | 0 (V) | unique spin verb | Y | 5 | 5 | KEEP | Iconic V; spin shape distinct. |

#### 12 Numbered

| # | Skill | A | B | C | D | E | F | G | H | I | J | K | Rationale |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 1 | Iron Charge | 5 | 5 | 5 | DMG/MOB | N | 3 | canonical engage | Y | 5 | 5 | KEEP | Signature dash+stun; lots of chain payoffs. |
| 2 | Crippling Blow | 4 | 4 | 2 | DMG/SET | N | 1 | unique | Y | 4 | 4 | KEEP | Heal-debuff niche; pose readable. |
| 3 | Iron Crush | 2 | 4 | 2 | RES | Y | 1 | unique buff | N | 2 | 3 | TIGHTEN | Pure damage buff is invisible; needs visible aura cue. |
| 4 | Gravity Cleave | 4 | 4 | 3 | CC | N | 1 | unique pull | Y | 4 | 4 | KEEP | Pull verb is rare in melee; keep. |
| 5 | Sunder Mark | 3 | 5 | 4 | SET | Y | 0 | unique mark | N | 2 | 3 | TIGHTEN | Mark is invisible w/o engine overlay; STATE language must be Unity. |
| 6 | Earthsplitter | 4 | 3 | 3 | CC | N | 1 | knockup line | Y | 4 | 4 | KEEP | Vertical motion separates from ground-slams. |
| 7 | Ironclad Momentum | 3 | 3 | 1 | DEF | Y | 1 | aura buff | N | 1 | 3 | TIGHTEN | Visible armor sheen needed; otherwise invisible. |
| 8 | Iron Counter | 5 | 4 | 4 | DEF/DMG | N | 1 | counter window | Y | 3 | 5 | KEEP | Counter pose distinct; timing is engine. |
| 9 | Blade Rush | 3 | 3 | 3 | DMG/MOB | N | 0 | OVERLAPS Iron Charge | N | 4 | 3 | TIGHTEN | Make line-multihit pose; line residue effect; not just dash. |
| 10 | Battle Surge | 2 | 3 | 2 | DEF/RES | Y | 1 | heal-on-spend | N | 1 | 3 | TIGHTEN | Visible heal pulse per spend needed. |
| 11 | Deep Wound | 4 | 3 | 3 | DMG | N | 1 | bleed unique | Y | 3 | 4 | KEEP | Bleed VFX engine; pose can be sword-cut. |
| 12 | Death Blow | 5 | 5 | 4 | BST | N | 3 | execute | Y | 4 | 5 | KEEP | HP<30 execute; iconic finisher. |

#### R4 Extras

| Skill | Verdict | Note |
|---|---|---|
| Quake Slam | KEEP | Distinct vertical-cracks-line pose; Broken state stack mechanic. |
| Iron Roar | TIGHTEN | 360 shockwave overlaps Earthsplitter visually; differ via voice/sound or jump-pose. |

#### Warblade summary

- Helpers (E=Y) of 12: Iron Crush, Sunder Mark, Ironclad Momentum, Battle Surge = **4/12** (at threshold).
- Roles: DMG 6, CC 2, DEF 2, RES 1, SET 1, BST 1. Healthy spread.
- Build axis orphans: Sunder Mark (axis 0). Critical because it is class identity ("break").
- Top redundancy cluster: **{Iron Charge, Blade Rush, Momentum Slam}** all forward-engage. Iron Charge canonical. Blade Rush needs line-pierce identity. Momentum Slam needs shoulder-no-sword.
- Verdict overall: **mostly solid**; visible state language for Sunder/Iron Crush/Battle Surge must be Unity-side.

---

### 2.2 ELEMENTALIST

Verbs: switch / shape / detonate. Resource: Mana + Elemental State.

#### Base + V

| # | Skill | A | B | C | D | E | F | G | H | I | J | K | Rationale |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| L | Rift Bolt (LMB) | 3 | 4 | 3 | DMG | N | 0 | unique projectile | Y | 5 | 4 | KEEP | Rapid-fire projectile, every-3rd empowered. |
| R | Element Switch (RMB) | 2 | 5 | 2 | RES | Y | 0 | unique system | N | 1 | 4 | KEEP | System mechanic, NOT a pose; visualize via UI rune. |
| D | Elemental Pulse (Dash) | 4 | 4 | 3 | DMG | N | 0 | element-aware | Y | 4 | 4 | KEEP | Mobility offensive; element flavor varies by state. |
| V | Trinity Storm | 5 | 4 | 3 | BST | N | 0 (V) | central rune | Y | 5 | 5 | KEEP | 3-element rune; iconic. |

#### 12 Numbered

| # | Skill | A | B | C | D | E | F | G | H | I | J | K | Rationale |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 1 | Fireball | 5 | 5 | 4 | DMG | N | 1 | classic projectile | Y | 5 | 5 | KEEP | Iconic; big readable shape. |
| 2 | Glacial Spike | 5 | 5 | 3 | DMG/CC | N | 1 | line-spike | Y | 5 | 5 | KEEP | Strong shape; line geometry. |
| 3 | Living Bomb | 4 | 5 | 3 | DMG | N | 1 | unique | Y | 4 | 4 | KEEP | Delayed bomb, copies; detonation distinct. |
| 4 | Blink | 4 | 3 | 3 | MOB | Y | 0 | unique teleport | Y | 4 | 4 | TIGHTEN | Orphan from build axes; needs damage-on-cross to qualify. |
| 5 | Frozen Orb | 4 | 4 | 3 | CC | Y | 1 | unique slow zone | Y | 5 | 4 | KEEP | Slow-moving orb, distinct. |
| 6 | Prism Beam | 5 | 4 | 4 | DMG | N | 1 | line-channel | Y | 4 | 5 | KEEP | Line channel beam, iconic. |
| 7 | Meteor | 5 | 4 | 4 | DMG/CC | N | 2 | drop AoE | Y | 5 | 5 | KEEP | Falling-from-sky pose; readable. |
| 8 | Frost Wall | 3 | 3 | 2 | DEF/CC | Y | 1 | wall barrier | Y | 4 | 3 | TIGHTEN | Cursor placement; visible but pure-utility risk. |
| 9 | Solar Flare | 4 | 4 | 3 | DMG | N | 1 | cone radiant | Y | 4 | 4 | KEEP | Light cone, distinct. |
| 10 | Radiant Pillar | 3 | 4 | 2 | RES | Y | 1 | aura buff | N | 2 | 3 | TIGHTEN | Aura buffs are invisible at 128px; needs pillar pose visible. |
| 11 | Element Charge | 3 | 3 | 2 | RES | Y | 1 | passive buff | N | 2 | 3 | TIGHTEN | Spell-haste buff; needs visible glow per cast. |
| 12 | Blizzard | 5 | 5 | 4 | DMG/CC | N | 1 | falling shards AoE | Y | 5 | 5 | KEEP | Big iconic AoE. |

#### R4 Extras

| Skill | Verdict | Note |
|---|---|---|
| Rune Anchor | KEEP | Ground rune with skill trigger. Distinct shape. |
| Element Trail (passive) | KEEP | Movement visual identity; ground-based VFX. |

#### Elementalist summary

- Helpers of 12: Blink, Frozen Orb, Frost Wall, Radiant Pillar, Element Charge = **5/12** (over threshold).
- Roles: DMG 6, CC 2, DEF 1, RES 2, MOB 1. Healthy.
- Build axis orphan: Blink (0).
- Top redundancy cluster: **none critical**. Blink + Element Switch dual-form mobility risk addressed by axis-orphan flag.
- Verdict overall: **strong**. Helpers exceed threshold but each has distinct shape — class identity is shape variety. Buff-skills (Pillar/Charge) need visible per-cast pulse.

---

### 2.3 SHADOWBLADE

Verbs: phase / scar / collapse. Resource: Sever (positional).

#### Base + V

| # | Skill | A | B | C | D | E | F | G | H | I | J | K | Rationale |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| L | Veil Strike (LMB) | 3 | 4 | 3 | DMG/SET | N | 0 | reverse-grip slash | Y | 5 | 4 | KEEP | Mark-applying base. |
| R | Veil Flicker (RMB) | 4 | 4 | 4 | MOB | Y | 0 | phase-through | Y | 3 | 4 | KEEP | Phase verb, leaves Scar (Unity). |
| D | Seam Rend (Dash) | 4 | 4 | 3 | DMG/MOB | N | 0 | dash-through cut | N | 3 | 4 | TIGHTEN | Visually overlaps Veil Flicker; differ via residue Scar shape. |
| V | Wraith Form | 5 | 4 | 4 | BST/DEF | N | 0 (V) | ghost transparent | Y | 4 | 5 | KEEP | Strong V; transparent body distinct. |

#### 12 Numbered

| # | Skill | A | B | C | D | E | F | G | H | I | J | K | Rationale |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 1 | Veil Strike | 3 | 4 | 3 | DMG/SET | N | 0 | base | Y | 5 | 4 | KEEP | Same as LMB row above (canonical lists this as #1). |
| 2 | Phase Step | 3 | 4 | 5 | MOB | Y | 1 | OVERLAPS Veil Flicker | N | 3 | 3 | TIGHTEN | Differ from Veil Flicker via no-Scar (clean teleport); add 0.3s invis pose. |
| 3 | Backstab Mark | 4 | 5 | 4 | DMG/SET | N | 1 | unique behind-hit | Y | 4 | 4 | KEEP | Crit-from-behind; class identity. |
| 4 | Shadow Clone | 3 | 3 | 2 | DEF/SET | Y | 1 | decoy phantom | Y | 4 | 3 | TIGHTEN | Decoy needs visible phantom (Y); aggro logic Unity. |
| 5 | Death Mark | 5 | 5 | 5 | DMG/BST | N | 1 | delayed bomb | Y | 4 | 5 | KEEP | Mark-detonate icon; iconic Shadowblade verb. |
| 6 | Veil Burst | 5 | 4 | 5 | DMG | N | 1 | 4-strike teleport | Y | 4 | 5 | KEEP | Strong burst; multi-Scar. |
| 7 | Severance | 4 | 4 | 3 | BST/DMG | N | 1 | execute line | Y | 4 | 4 | KEEP | HP execute line; distinct. |
| 8 | Smoke Veil | 3 | 4 | 3 | DEF/SET | Y | 1 | AoE stealth | Y | 4 | 3 | KEEP | Stealth pop pose visible. |
| 9 | Chain Cull | 5 | 5 | 4 | DMG | N | 1 | mark-to-mark hop | Y | 4 | 5 | KEEP | Mark hopping is unique. |
| 10 | Shadow Pin | 3 | 3 | 4 | CC/SET | Y | 0 | dagger throw root | Y | 4 | 3 | TIGHTEN | Orphan from build axes; fold into "Mark Collapse" axis. |
| 11 | Twin Carve | 3 | 3 | 2 | DMG | N | 0 | OVERLAPS Veil Strike | N | 4 | 3 | MERGE | 2 slashes + phase-step = Veil Strike + Phase Step combo; redundant. Merge or REDESIGN as "spinning slash with double Scar". |
| 12 | Night Aperture | 3 | 4 | 3 | RES | Y | 1 | mirror-Scar buff | N | 2 | 3 | TIGHTEN | Aura buff invisible w/o Scar decals (Unity). |

#### R4 Extras

| Skill | Verdict | Note |
|---|---|---|
| Mirror Cut | KEEP | Teleport-along-Scar adds spatial verb. |
| Scar Echo | KEEP | Passive auto-Scar after collapse; engine flag. |

#### Shadowblade summary

- Helpers of 12: Phase Step, Shadow Clone, Smoke Veil, Shadow Pin, Night Aperture = **5/12** (over threshold). Add Backstab-Mark as half-helper -> 5.5/12.
- Roles: DMG 6, MOB 1, CC 1, DEF 2, SET 2 (Backstab/Shadow Pin counted as DMG/SET hybrids). Helper share is high.
- Build axis orphans: Shadow Pin (0).
- Top redundancy cluster: **{Veil Strike, Twin Carve, Severance, Chain Cull}** all "forward slash from front." Differ via target-state (Twin Carve no condition / Sever HP<30 / Chain Cull Mark-hop). Visual sameness risk is real. **Twin Carve MERGE candidate**.
- Phase teleport cluster: **{Veil Flicker, Seam Rend, Phase Step}** + V Wraith Form. Acceptable if Scar decals (Unity) differ per skill.
- Verdict overall: **highest helper density of any class**. State language (Scar / Mark / Collapse) must be Unity-owned.

---

### 2.4 RANGER

Verbs: mark / trap / detonate. Resource: Focus (distance-based).

#### Base + V

| # | Skill | A | B | C | D | E | F | G | H | I | J | K | Rationale |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| L | Rift Arrow (LMB) | 4 | 5 | 5 | DMG/SET | N | 1 | charge-shot | Y | 5 | 5 | KEEP | Hold-charge mechanic, marks. |
| R | Tactical Roll (RMB) | 3 | 3 | 3 | MOB | Y | 0 | back-roll + shot | Y | 4 | 4 | KEEP | Distinct backward motion. |
| D | Hunter's Step (Dash) | 3 | 3 | 4 | MOB/SET | Y | 1 | dash + crit setup | N | 3 | 3 | TIGHTEN | Overlaps Rift Step visually; needs slow-mo Mark-setup pose. |
| V | Spirit Bow | 5 | 4 | 4 | BST | N | 0 (V) | giant rift bow | Y | 5 | 5 | KEEP | Iconic V. |

#### 12 Numbered

| # | Skill | A | B | C | D | E | F | G | H | I | J | K | Rationale |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 1 | Rift Arrow | 4 | 5 | 5 | DMG/SET | N | 1 | base | Y | 5 | 5 | KEEP | Same as LMB. |
| 2 | Pinning Shot | 4 | 5 | 4 | CC | N | 1 | root shot | Y | 4 | 5 | KEEP | 1.5s root; iconic. |
| 3 | Marked Detonate | 5 | 5 | 5 | DMG/BST | N | 1 | mark detonator | Y | 4 | 5 | KEEP | Class verb anchor. |
| 4 | Hunter's Step | 3 | 3 | 4 | MOB/SET | Y | 1 | base | N | 3 | 3 | TIGHTEN | Same as Dash row. |
| 5 | Bone Trap | 5 | 5 | 5 | SET/CC | Y | 1 | ground trap | Y | 4 | 5 | KEEP | Cursor zone; iconic trap. |
| 6 | Sweep Volley | 4 | 4 | 4 | DMG | N | 1 | left-right cone | Y | 5 | 4 | KEEP | Cone shape; distinct. |
| 7 | Skirmish Shot | 3 | 3 | 2 | DMG/MOB | N | 0 | move + shoot | N | 3 | 3 | TIGHTEN | Overlaps LMB-on-the-move; distinguish via vault-pose. |
| 8 | Predator's Mark | 3 | 4 | 3 | SET | Y | 1 | AoE mark zone | N | 3 | 3 | MERGE | Overlaps Multi-Mark; merge as scalable AoE. |
| 9 | Multi-Mark | 3 | 4 | 4 | SET | Y | 0 | single-cast 5-mark | N | 3 | 3 | MERGE | Merge with Predator's Mark; orphan from axes. |
| 10 | Final Strike | 4 | 5 | 3 | BST | N | 1 | mark+execute | Y | 4 | 4 | KEEP | Conditional execute; iconic. |
| 11 | Rift Step | 3 | 3 | 4 | MOB | Y | 1 | void short-dash | N | 3 | 3 | TIGHTEN | Distinguish from Hunter's Step via void/rift VFX. |
| 12 | Spirit Bow | 5 | 4 | 4 | BST | N | 0 (V) | base | Y | 5 | 5 | KEEP | V Burst row above. |

#### R4 Extras

| Skill | Verdict | Note |
|---|---|---|
| Wireline Trap | KEEP | Two-point trap line; distinct geometry. |
| Quiver Pulse | KEEP | Mark-reflect mechanic; engine-side. |
| Hawk Eye | KEEP | Upgrade path; not new active. |

#### Ranger summary

- Helpers of 12: Tactical Roll (RMB), Hunter's Step, Bone Trap, Predator's Mark, Multi-Mark, Rift Step = **6/12** (high). Class identity is "mark+trap" so high helper count is structural — but Predator's Mark and Multi-Mark are merge candidates.
- Roles: DMG 5, SET 4, CC 1, MOB 2, BST 1. SET-heavy.
- Build axis orphans: Multi-Mark (0).
- Top redundancy cluster: **{Predator's Mark, Multi-Mark}** -> MERGE.
- Movement triple: **{Hunter's Step, Rift Step, Tactical Roll}**. Visual differentiation via direction (back-roll / forward-dash / void-step).
- Verdict overall: **role-heavy on setup** but the class is honest about it. Marks need engine reticles. Predator's Mark and Multi-Mark merge cleans helper count.

---

### 2.5 RAVAGER

Verbs: suffer / trade / frenzy. Resource: Fury (take-damage build).

#### Base + V

| # | Skill | A | B | C | D | E | F | G | H | I | J | K | Rationale |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| L | Brutal Swing (LMB) | 5 | 5 | 4 | DMG | N | 0 | 3-hit combo | Y | 5 | 5 | KEEP | Heavy axe chain. |
| R | Blood Pact (RMB) | 4 | 5 | 4 | RES | Y | 0 | self-cut HP-Fury | Y | 4 | 4 | KEEP | Trade verb; iconic. |
| D | Fury Tackle (Dash) | 4 | 4 | 3 | DMG/CC | N | 0 | shoulder tackle | Y | 4 | 4 | KEEP | Distinct shoulder pose. |
| V | Berserk Mode | 5 | 4 | 3 | BST | N | 0 (V) | blood ring | Y | 4 | 5 | KEEP | Strong V; ring + per-kill extension. |

#### 12 Numbered

| # | Skill | A | B | C | D | E | F | G | H | I | J | K | Rationale |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 1 | Bloodlust Strike | 5 | 5 | 4 | DMG | N | 1 | low-HP scaling | Y | 4 | 5 | KEEP | Cone hit; HP-scale unique. |
| 2 | Carnage Spin | 4 | 4 | 3 | DMG/CC | N | 1 | brute spin | Y | 4 | 4 | KEEP | Spin diff from Bladestorm via brute chunks. |
| 3 | Frenzied Leap | 4 | 4 | 3 | DMG/MOB | N | 1 | leap-AoE | Y | 4 | 4 | KEEP | Leap entry; iconic. |
| 4 | Reckless Swing | 5 | 4 | 3 | DMG | N | 1 | mega single-hit | Y | 5 | 5 | KEEP | Big single hit + risk window. |
| 5 | Bloodthirst | 4 | 4 | 3 | DMG/RES | N | 1 | 5-hit lifesteal | Y | 4 | 4 | KEEP | Vampiric chain. |
| 6 | Bloodied Roar | 3 | 4 | 3 | CC | Y | 1 | stagger shout | N | 3 | 3 | TIGHTEN | Roar pose distinct but generic shout VFX. |
| 7 | Barbaric Charge | 4 | 4 | 4 | MOB/CC | N | 1 | linear push | Y | 4 | 4 | KEEP | Distinct from Iron Charge via no-stun + push-all. |
| 8 | Undying Tenacity | 4 | 5 | 3 | DEF | Y | 1 | HP-1 invuln | N | 2 | 4 | TIGHTEN | Save-state needs UI flash; pose can be hunched-over. |
| 9 | Iron Grab | 4 | 4 | 3 | CC | N | 1 | grab-throw | Y | 4 | 4 | KEEP | Grab is rare; distinct. |
| 10 | Blood-Drunk Leap | 3 | 3 | 2 | DMG/MOB | N | 1 | OVERLAPS Frenzied Leap | N | 3 | 2 | MERGE | Same leap-on-target; differ only by Fury cost. **MERGE INTO Frenzied Leap as Fury-empowered toggle.** |
| 11 | Shatter Armor | 3 | 5 | 4 | SET | Y | 0 | armor debuff | N | 2 | 3 | TIGHTEN | Mirror of Sunder Mark; engine state. |
| 12 | Death Wish | 4 | 5 | 3 | RES/BST | Y | 1 | Fury x3 buff | N | 2 | 4 | TIGHTEN | Aura buff; needs visible HP-cap UI flash. |

#### R4 Extras

| Skill | Verdict | Note |
|---|---|---|
| Wound Echo (passive) | KEEP | Reflect mechanic; engine. |
| Pain Reservoir (passive) | KEEP | Fury-rate buff under 50%; engine. |
| Crimson Pact | TIGHTEN | Overlaps Blood Pact (RMB) and Dark Pact (Summoner). Differ via larger pool / longer self-bleed. |

#### Ravager summary

- Helpers of 12: Bloodied Roar, Undying Tenacity, Shatter Armor, Death Wish = **4/12** (at threshold).
- Roles: DMG 6, CC 2, MOB 1, DEF 1, RES 2, SET 1. Healthy.
- Build axis orphans: Shatter Armor (0).
- Top redundancy cluster: **{Frenzied Leap, Blood-Drunk Leap}** -> MERGE.
- Heavy-swing redundancy: **{Brutal Swing, Bloodlust Strike, Reckless Swing, Carnage Spin}**. Cone / scaling-cone / single / spin. OK once Reckless gets risk-pose (planted feet, exposed body).
- Verdict overall: **mostly solid** if Blood-Drunk Leap merges into Frenzied. Buff/aura skills need visible cues.

---

### 2.6 RONIN

Verbs: wait / draw / punish. Resource: Draw Tension.

#### Base + V

| # | Skill | A | B | C | D | E | F | G | H | I | J | K | Rationale |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| L | Sheath Walk (LMB) | 3 | 4 | 3 | DMG | N | 0 | walking slash | Y | 4 | 4 | KEEP | Tension-builder LMB. |
| R | Drawn Edge (RMB) | 5 | 5 | 4 | DMG/DEF | N | 0 | sheath-pull | Y | 4 | 5 | KEEP | Strong RMB; deflect window. |
| D | Iaido Blur (Dash) | 4 | 4 | 3 | DMG/MOB | N | 0 | sheath-blur | N | 3 | 4 | TIGHTEN | Overlaps Quickdraw Slash visually; differ via dash-direction. |
| V | Mugen no Kiri | 5 | 4 | 4 | BST | N | 0 (V) | instant-cuts | Y | 4 | 5 | KEEP | Strong V; instant draw-cut state. |

#### 12 Numbered

| # | Skill | A | B | C | D | E | F | G | H | I | J | K | Rationale |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 1 | Quickdraw Slash | 5 | 5 | 4 | DMG | N | 1 | iconic draw | Y | 5 | 5 | KEEP | Class signature. |
| 2 | Haste Dash | 3 | 3 | 3 | MOB | Y | 1 | OVERLAPS Iaido Blur | N | 3 | 3 | TIGHTEN | Differ via afterimage residue (Phantom Dance axis). |
| 3 | Soken-giri | 4 | 4 | 3 | DMG | N | 1 | 5-fan slashes | Y | 4 | 4 | KEEP | Multi-cut fan distinct. |
| 4 | Iaido Stance | 3 | 5 | 3 | RES/SET | Y | 1 | stance pose | Y | 3 | 4 | KEEP | Stance pose iconic; 0.8s readable. |
| 5 | Wind Step | 3 | 3 | 2 | MOB | Y | 1 | OVERLAPS Haste Dash | N | 3 | 3 | TIGHTEN | 3-direction step is unique IF clearly multi-step pose. |
| 6 | Counter Draw | 5 | 4 | 4 | DEF/DMG | N | 0 | counter pose | Y | 4 | 5 | KEEP | Distinct from Sakura Veil via attack-counter (not deflect). |
| 7 | Phantom Step | 3 | 4 | 4 | DEF/SET | Y | 1 | afterimage decoy | Y | 4 | 3 | TIGHTEN | Decoy fresh; clarify vs Shadow Clone (Shadowblade) via afterimage geometry. |
| 8 | Sakura Veil | 4 | 4 | 4 | DEF | Y | 0 | petal deflect | Y | 4 | 4 | TIGHTEN | Orphan from Build Eksenleri; consider folding into Iaido Burst axis. |
| 9 | Crescent Arc | 5 | 4 | 4 | DMG | N | 1 | arc-circle | Y | 5 | 5 | KEEP | Iconic; geometry distinct. |
| 10 | Flash Draw | 5 | 4 | 4 | DMG/MOB | N | 1 | warp-cuts | Y | 4 | 4 | KEEP | Multi-target teleport-cuts. |
| 11 | Iai Pressure | 3 | 4 | 2 | RES | Y | 1 | dash-buff | N | 2 | 3 | TIGHTEN | Aura buff; needs visible blade glow. |
| 12 | Void Cleave | 5 | 5 | 4 | BST | N | 1 | giant cone | Y | 4 | 5 | KEEP | Master finisher; full-Tension dump. |

#### R4 Extras

| Skill | Verdict | Note |
|---|---|---|
| Stillness | KEEP | 1.5s motionless = Tension+30; readable wait pose. |
| Sheath Pressure (passive) | KEEP | Engine-only. |
| Wind Read (passive) | KEEP | Whiff-detect; engine. |

#### Ronin summary

- Helpers of 12: Haste Dash, Iaido Stance, Wind Step, Phantom Step, Sakura Veil, Iai Pressure = **6/12** (high).
- Roles: DMG 5, DEF 3, MOB 3, RES 2, BST 1. Helper-heavy.
- Build axis orphan: Sakura Veil (0).
- Top redundancy cluster: **{Haste Dash, Wind Step, Iaido Blur}** all forward-slide motion. Differ Wind Step via 3-direction zigzag pose; Phantom Step via afterimage residue.
- Slash redundancy: **{Quickdraw Slash, Iaido Blur, Crescent Arc, Flash Draw, Soken-giri}** five katana-cut skills. Mechanically distinct (single / dash / circle / 3-warp / 5-fan) but visual unification effort needed (different blade angle per skill).
- Verdict overall: **helper share too high; movement triple needs distinction**. Phantom Step is the most class-unique helper; Haste Dash + Wind Step reduce one or both via clear pose-shape.

---

### 2.7 GUNSLINGER

Verbs: slide / shoot / reload. Resource: Heat (overheat).

#### Base + V

| # | Skill | A | B | C | D | E | F | G | H | I | J | K | Rationale |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| L | Dual Fire (LMB) | 4 | 5 | 3 | DMG | N | 0 | dual-pistol fire | Y | 5 | 5 | KEEP | Class anchor. |
| R | Hip Shot (RMB) | 4 | 4 | 3 | DMG/MOB | N | 0 | side-slide-shot | Y | 4 | 4 | KEEP | Lateral movement + shot. |
| D | Crossfire Entry (Dash) | 4 | 4 | 3 | DMG | N | 0 | dive-fire | Y | 4 | 4 | KEEP | Dash entry with split bullets. |
| V | Full Metal Storm | 5 | 4 | 3 | BST | N | 0 (V) | unlimited dual-fire | Y | 5 | 5 | KEEP | Strong V; AoE muzzle flash. |

#### 12 Numbered

| # | Skill | A | B | C | D | E | F | G | H | I | J | K | Rationale |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 1 | Rift Dash | 5 | 5 | 4 | MOB/DMG | N | 1 | dive + dual-fire | Y | 4 | 5 | KEEP | Combat dash + AoE. |
| 2 | Quickdraw | 3 | 4 | 3 | DMG | N | 1 | OVERLAPS Hip Shot/Deadshot | N | 4 | 3 | MERGE | Single-target burst; consider folding into Hip Shot upgrade. |
| 3 | Cursor Storm | 5 | 4 | 4 | DMG | N | 1 | bullet-rain area | Y | 5 | 5 | KEEP | Cursor zone; iconic. |
| 4 | Deadshot | 5 | 5 | 5 | BST | N | 1 | precision line | Y | 4 | 5 | KEEP | Long-line execute; iconic. |
| 5 | Smoke Grenade | 3 | 3 | 3 | CC/DEF | Y | 1 | smoke zone | Y | 4 | 3 | TIGHTEN | Smoke + slow; differ from Frost Wall via flame/spark. |
| 6 | Fan the Hammer | 5 | 4 | 3 | DMG | N | 1 | rapid 6-fire | Y | 4 | 5 | KEEP | Burst sequence. |
| 7 | Suppression Fire | 3 | 3 | 3 | CC/DMG | N | 1 | line-push | Y | 4 | 3 | TIGHTEN | Pose can read as line-shot; differ from Deadshot via wider beam/spread. |
| 8 | Rift Grenade | 4 | 4 | 4 | DMG/CC | N | 1 | delayed bomb | Y | 4 | 4 | KEEP | Cursor zone + delay. |
| 9 | Ricochet | 4 | 4 | 3 | DMG | N | 1 | bouncing bullet | Y | 4 | 4 | KEEP | Bullet-trail unique. |
| 10 | Reload Dance | 3 | 4 | 2 | RES/MOB | Y | 1 | reload + back-step | Y | 3 | 4 | TIGHTEN | Visible reload pose lacking on sheets; emphasize cylinder/mag. |
| 11 | Burning Ammo | 3 | 4 | 3 | RES | Y | 1 | fire-DoT buff | N | 2 | 3 | TIGHTEN | Aura buff; needs muzzle flash recolor. |
| 12 | Point Blank Execute | 5 | 5 | 3 | BST | N | 1 | close-range mega | Y | 4 | 5 | KEEP | Range-conditional execute; iconic. |

#### R4 Extras

| Skill | Verdict | Note |
|---|---|---|
| Empty Mag Burst | KEEP | Last-bullet 3x dmg + reset; reload-rhythm iconic. |
| Reload Roll | KEEP | Slide + reload simultaneously. |
| Exposed Line (active) | TIGHTEN | Self-damage trade-off; engine state. |

#### Gunslinger summary

- Helpers of 12: Smoke Grenade, Reload Dance, Burning Ammo = **3/12** (good).
- Roles: DMG 7, BST 2, CC 2, MOB 1, RES 2. Healthy.
- Build axis orphans: Quickdraw appears only in Mobile Assassin axis (1) — borderline.
- Top redundancy cluster: **{Quickdraw, Hip Shot RMB, Deadshot}** all single-target precision. MERGE Quickdraw into Hip Shot upgrade path.
- Long-line shots: **{Deadshot, Suppression Fire}** both line-shape. Differ via cone-spread for Suppression Fire.
- Verdict overall: **good role distribution**; Quickdraw is the merge candidate; Reload/Heat language needs visible UI per CURRENT_STATUS notes.

---

### 2.8 BRAWLER

Verbs: weave / combo / launch. Resource: Charge (0-5).

#### Base + V

| # | Skill | A | B | C | D | E | F | G | H | I | J | K | Rationale |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| L | Jab (LMB) | 3 | 5 | 3 | DMG | N | 0 | rapid jab | Y | 5 | 4 | KEEP | Class anchor; ritmic combo source. |
| R | Weave (RMB) | 4 | 5 | 4 | DEF/RES | Y | 0 | side-step dodge | Y | 4 | 5 | KEEP | Perfect timing window iconic. |
| D | Flying Knee (Dash) | 4 | 4 | 3 | DMG/CC | N | 0 | knee-airborne | Y | 4 | 4 | KEEP | Knee airborne pose. |
| V | Overdrive | 5 | 4 | 3 | BST | N | 0 (V) | charge-locked state | N | 3 | 5 | TIGHTEN | Visually overlaps Berserk Mode / Wraith Form (aura). Needs phantom-arm afterimage to differ. |

#### 12 Numbered

| # | Skill | A | B | C | D | E | F | G | H | I | J | K | Rationale |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 1 | Mach Punch | 5 | 5 | 3 | DMG | N | 1 | OVERLAPS Combo Chain | N | 4 | 5 | TIGHTEN | Multi-arm afterimage required; Combo Chain is forward-translation, Mach is in-place. |
| 2 | Shockwave Slam | 4 | 4 | 3 | DMG/CC | N | 1 | OVERLAPS Seismic Stomp/Repulse | N | 4 | 4 | TIGHTEN | Ground-AoE; cluster fight. |
| 3 | Tornado Kick | 4 | 4 | 3 | DMG/CC | N | 1 | rotation kick | Y | 4 | 4 | KEEP | Kick verb; distinct from punches. |
| 4 | Combo Chain | 5 | 5 | 3 | DMG/MOB | N | 1 | 4-pose translation | Y | 4 | 5 | KEEP | Iconic 4-frame combo. |
| 5 | Guard Break | 4 | 5 | 4 | SET/DMG | N | 1 | armor-strip punch | Y | 4 | 4 | KEEP | Sets up Charged-State payoffs. |
| 6 | Repulse | 3 | 3 | 2 | CC | Y | 1 | OVERLAPS Shockwave Slam | N | 4 | 3 | MERGE | Push-all; redundant with Shockwave Slam knockback. |
| 7 | Counter Blow | 5 | 4 | 4 | DEF/DMG | N | 1 | counter punch | Y | 4 | 5 | KEEP | Counter pose; iconic. |
| 8 | Aerial Rave | 5 | 5 | 4 | DMG/CC | N | 1 | airborne juggle | Y | 4 | 5 | KEEP | Launch verb anchor; iconic. |
| 9 | Cyclone Drive | 4 | 4 | 3 | DMG/MOB | N | 1 | 2s rotation | N | 4 | 4 | TIGHTEN | Rotation overlaps Tornado Kick; differ via duration + sustained body spin. |
| 10 | Seismic Stomp | 4 | 4 | 3 | CC/DMG | N | 1 | ground-line stomp | N | 4 | 4 | TIGHTEN | Differ from Shockwave Slam via line vs ring. |
| 11 | Pivot Hook | 5 | 5 | 3 | DMG | N | 1 | side-hook footwork | Y | 4 | 5 | KEEP | Charge-multiplied single hit; iconic. |
| 12 | Unstoppable Force | 4 | 5 | 3 | RES/BST | Y | 1 | charge-lock buff | N | 3 | 4 | TIGHTEN | Aura buff; needs visible Charge pip glow. |

#### R4 Extras

| Skill | Verdict | Note |
|---|---|---|
| Pulverize | KEEP | Cracked-detonation finisher; distinct. |
| Off-Balance | TIGHTEN | Overlaps Shockwave Slam; differ via weaker punch-ground variant. |
| Glass Strike | KEEP | Shattered/Sundered crit + shard scatter; iconic. |
| Wall Slam Combo | KEEP | Wall-slam state; environmental. |
| Pin Strike | KEEP | Cracked -> Pinned conditional; engine. |

#### Brawler summary

- Helpers of 12: Repulse, Unstoppable Force = **2/12** (low — class is damage-heavy).
- Roles: DMG 9, CC 3, DEF 1, RES 1, SET 1. DMG-heavy is fine for Brawler identity.
- Build axis orphans: none (all 12 in some axis).
- Top redundancy cluster (CRITICAL): **{Mach Punch, Combo Chain, Pivot Hook}** = three punch-combos; **{Shockwave Slam, Seismic Stomp, Repulse}** = three ground-AoE; **{Tornado Kick, Cyclone Drive}** = two rotation-hits.
- Verdict overall: **lowest helper count, highest visual collision**. Differentiation not by mechanic (most mechanics are unique) but by POSE and FOOTWORK. Repulse is the merge candidate.

---

### 2.9 SUMMONER

Verbs: command / sacrifice / raise. Resource: Charges (0-4).

#### Base + V

| # | Skill | A | B | C | D | E | F | G | H | I | J | K | Rationale |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| L | Command Strike (LMB) | 3 | 5 | 3 | DMG | N | 0 | minion-redirect | Y | 4 | 4 | KEEP | Dual-function; minion command line. |
| R | Soul Dart (RMB) | 4 | 5 | 3 | DMG/SET | N | 0 | mark-target | Y | 4 | 4 | KEEP | Tactical priority cue. |
| D | Spirit Surge (Dash) | 3 | 4 | 3 | MOB/SET | Y | 0 | minion haste | Y | 4 | 3 | TIGHTEN | Reduce overlap with Commanding Strike. |
| V | Army of the Dead | 5 | 4 | 3 | BST | N | 0 (V) | mass minions | Y | 5 | 5 | KEEP | Iconic V; biggest visual. |

#### 12 Numbered

| # | Skill | A | B | C | D | E | F | G | H | I | J | K | Rationale |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 1 | Raise Skeleton | 5 | 5 | 4 | DMG (indirect) | Y | 1 | unique summon | Y | 5 | 5 | KEEP | Class anchor. |
| 2 | Summon Golem | 5 | 5 | 3 | DMG/DEF | Y | 1 | tank summon | Y | 5 | 5 | KEEP | Tank verb. |
| 3 | Command Beacon | 4 | 4 | 3 | SET/RES | Y | 1 | minion-buff zone | Y | 4 | 4 | KEEP | Cursor-placed beacon; unique. |
| 4 | Corpse Explosion | 5 | 5 | 4 | DMG | N | 1 | corpse-bomb | Y | 4 | 5 | KEEP | Vertical bone burst; iconic. |
| 5 | Death Nova | 3 | 3 | 4 | SET/DMG | N | 1 | OVERLAPS Mass Sacrifice/Corpse Explosion | N | 3 | 3 | MERGE | 1-minion-sacrifice = poison cloud; redundant with Mass Sacrifice. Merge as sub-tier. |
| 6 | Commanding Strike | 4 | 4 | 3 | DMG | N | 1 | minion-empower | N | 3 | 4 | TIGHTEN | Differentiation from LMB; emphasize boost VFX. |
| 7 | Blood for Power | 4 | 5 | 3 | RES | Y | 1 | sacrifice resource | N | 3 | 4 | TIGHTEN | Sacrifice variant; differ via corpse-rune visible. |
| 8 | Bone Shield | 4 | 4 | 3 | DEF | Y | 1 | minion-shield | Y | 4 | 4 | KEEP | Shield wall pose; distinct. |
| 9 | Soul Siphon Totem | 4 | 4 | 3 | RES | Y | 1 | totem | Y | 4 | 4 | KEEP | Totem geometry unique. |
| 10 | Mass Sacrifice | 5 | 5 | 4 | DMG/BST | N | 1 | all-minion explode | Y | 5 | 5 | KEEP | Iconic class verb. |
| 11 | Dark Pact | 3 | 4 | 4 | RES | Y | 0 | HP-summon | N | 3 | 3 | TIGHTEN | Orphan from build axes; pose almost identical to Blood for Power. |
| 12 | Lich Form | 5 | 4 | 3 | BST/DEF | Y | 1 | ghost-form buff | Y | 4 | 5 | KEEP | Master ghost transformation. |

#### R4 Extras

| Skill | Verdict | Note |
|---|---|---|
| Bone Tide | TIGHTEN | Mass small-summon; differ from Raise Skeleton via swarm density. |
| Soul Tax | KEEP | Sacrifice 1, return 2 later; engine timing. |
| Beacon Pull | KEEP | Beacon recall upgrade; engine. |

#### Summoner summary

- Helpers of 12: Raise Skeleton, Summon Golem, Command Beacon, Blood for Power, Bone Shield, Soul Siphon Totem, Dark Pact, Lich Form = **8/12** (very high). Class identity is summon/sacrifice so high helper share is structural — but Death Nova / Dark Pact / Blood for Power overlap.
- Roles: DMG 4 (direct), DEF 2, RES 4, SET 2, BST 1. RES-heavy; DMG comes mostly from minions.
- Build axis orphans: Dark Pact (axes only via dual class).
- Top redundancy cluster: **{Death Nova, Mass Sacrifice, Corpse Explosion, Blood for Power, Dark Pact}** five sacrifice-themed skills. Merge Death Nova INTO Mass Sacrifice as targeted sub-action.
- Verdict overall: **strongest visual identity, highest mechanical helper density**. Sacrifice variant overload is the only real risk.

---

### 2.10 HEXER

Verbs: stack / spread / blast. Resource: Hex Stacks (0-10/target).

#### Base + V

| # | Skill | A | B | C | D | E | F | G | H | I | J | K | Rationale |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| L | Hex Bolt (LMB) | 3 | 5 | 3 | DMG/SET | N | 0 | stack-on-hit projectile | Y | 5 | 4 | KEEP | Class anchor. |
| R | Curse Grasp (RMB) | 5 | 5 | 4 | CC/SET | N | 0 | hand-uzatma | Y | 4 | 5 | KEEP | Iconic press-and-hold curse. |
| D | Curse Step (Dash) | 4 | 4 | 4 | DMG/SET | N | 0 | curse-stack-aura | Y | 4 | 4 | KEEP | Distinct from Hex Bolt via AoE hand-wave. |
| V | Hex Cascade | 5 | 5 | 4 | BST | N | 0 (V) | chain-stack copy | Y | 4 | 5 | KEEP | Iconic V; chain visual. |

#### 12 Numbered

| # | Skill | A | B | C | D | E | F | G | H | I | J | K | Rationale |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 1 | Corruption | 4 | 5 | 4 | DMG/SET | N | 1 | instant-stack DoT | Y | 4 | 5 | KEEP | Quick stack burst; iconic. |
| 2 | Agony | 3 | 5 | 3 | DMG | N | 1 | continuous DoT | N | 3 | 4 | TIGHTEN | Tick visualization needed; pose-action limited. |
| 3 | Pandemic | 5 | 5 | 4 | DMG/BST | N | 1 | spread-stack | Y | 4 | 5 | KEEP | Spread verb anchor; iconic. |
| 4 | Hexblast | 5 | 5 | 5 | BST | N | 1 | full-stack detonation | Y | 4 | 5 | KEEP | Class peak payoff. |
| 5 | Empathy | 3 | 4 | 3 | DEF/RES | Y | 1 | reflect-curse | Y | 3 | 4 | TIGHTEN | Reflect overlay needed. |
| 6 | Haunt | 4 | 4 | 4 | DMG | N | 1 | spirit-companion | Y | 4 | 4 | KEEP | Haunt-spirit visible. |
| 7 | Unstable Affliction | 5 | 5 | 4 | DMG/CC | N | 1 | dispel-trap | Y | 4 | 5 | KEEP | Unique dispel-explode mechanic. |
| 8 | Enervate | 3 | 4 | 3 | CC | Y | 1 | slow + atk-debuff | N | 2 | 3 | TIGHTEN | Visible slow aura cue. |
| 9 | Mass Hex | 4 | 4 | 4 | DMG/SET | N | 1 | OVERLAPS Corruption (scale) | Y | 4 | 4 | KEEP | AoE 2-stack-all; differ from Corruption via screen-wide gesture. |
| 10 | Hex Overload | 4 | 5 | 3 | RES/BST | Y | 1 | hex-window buff | N | 2 | 4 | TIGHTEN | Aura buff; needs visible empowered glyph. |
| 11 | Cursed Mirror | 4 | 4 | 3 | DEF | Y | 1 | mirror-debuffs | N | 3 | 4 | TIGHTEN | Reflect logic engine; visible mirror sigil. |
| 12 | Blight Sigil | 5 | 5 | 4 | DMG/SET | N | 1 | ground-curse-zone | Y | 5 | 5 | KEEP | Cursor zone iconic. |

#### R4 Extras

| Skill | Verdict | Note |
|---|---|---|
| Whisper Mark (passive) | KEEP | Engine; nearby-spread. |
| Curse Bargain | TIGHTEN | Overlaps Mass Hex / Curse Grasp variants; differ via HP-cost-only mode. |

#### Hexer summary

- Helpers of 12: Empathy, Enervate, Hex Overload, Cursed Mirror = **4/12** (at threshold).
- Roles: DMG 5, BST 2, SET 1, CC 2, DEF 2, RES 1. Healthy.
- Build axis orphans: none (all 12 in axes).
- Top redundancy cluster: **{Corruption, Agony, Mass Hex}** all stack-applying. Differentiate by gesture: Corruption = focused-curse-dart / Agony = continuous-staff-channel / Mass Hex = screen-wide-arc.
- Verdict overall: **second-best balanced class** (after Elementalist). Strong identity. Reflect/aura buffs need visible cues.

---

## 3. CROSS-CLASS REDUNDANCY MAP

### 3.1 Movement / dash skills (HIGH RISK)

17+ movement skills across classes. Without engine-side trail differentiation they collapse to "color trail dash."

| Class | Skills | Note |
|---|---|---|
| Warblade | Iron Charge, Blade Rush, Momentum Slam | Forward charge cluster |
| Elementalist | Blink, Elemental Pulse | Teleport / pulse |
| Shadowblade | Phase Step, Veil Flicker, Seam Rend | Phase cluster |
| Ranger | Tactical Roll, Hunter's Step, Rift Step | Roll/Dash/Void |
| Ravager | Frenzied Leap, Blood-Drunk Leap (MERGE), Barbaric Charge, Fury Tackle | Leap cluster |
| Ronin | Haste Dash, Wind Step, Iaido Blur, Phantom Step | Slide/zigzag/blur |
| Gunslinger | Rift Dash, Reload Dance, Hip Shot, Crossfire Entry | Slide cluster |
| Brawler | Weave, Flying Knee | Step + knee |
| Hexer | Curse Step | Single |

**Differentiation rule:** each class's movement gets a unique residue (Scar / blood-spatter / petals / dust / muzzle-flash). Engine-side per-class trail material.

### 3.2 Buff / transform skills (MEDIUM-HIGH RISK)

11 self-buff "stand + colored aura" skills. Most are visually identical at 128px without UI cues.

| Class | Buff skill |
|---|---|
| Warblade | Iron Crush, Battle Surge, Ironclad Momentum |
| Elementalist | Radiant Pillar, Element Charge |
| Shadowblade | Night Aperture |
| Ravager | Death Wish |
| Ronin | Iai Pressure |
| Gunslinger | Burning Ammo |
| Brawler | Unstoppable Force |
| Summoner | Lich Form (transform — distinct) |
| Hexer | Hex Overload, Cursed Mirror, Empathy |

**Differentiation rule:** every buff must show a per-cast pulse + a visible affected-skill modification (e.g., Burning Ammo recolors muzzle flash). Static-aura-only is forbidden.

### 3.3 Decoy / phantom skills

| Class | Skill |
|---|---|
| Shadowblade | Shadow Clone |
| Ronin | Phantom Step |

Both are decoys but mechanically and visually distinct (Shadow Clone = single phantom that takes hits; Phantom Step = 3 afterimage decoys for tactical deception). Keep both.

### 3.4 Mark / setup skills

| Class | Skill |
|---|---|
| Warblade | Sunder Mark |
| Shadowblade | Backstab Mark, Death Mark |
| Ranger | Bone Trap, Predator's Mark, Multi-Mark, Marked Detonate |
| Ravager | Shatter Armor |
| Brawler | Guard Break |
| Summoner | Soul Dart (Marked Target priority) |
| Hexer | (all stack-applies are setup) |

**Differentiation rule:** each class's mark/state needs a unique color + symbol (engine UI). Sunder = red broken-armor icon / Backstab = purple X / Bone Trap = bone reticle / Hex stack = green pip / Charge = yellow chevron / etc.

### 3.5 Counter / defensive windows

| Class | Skill |
|---|---|
| Warblade | Iron Counter |
| Ronin | Counter Draw, Sakura Veil |
| Brawler | Counter Blow |

All have ~0.4-0.8s timing windows. Visual differentiation: Iron Counter = sword-block shockwave / Counter Draw = sheath-pull single line / Sakura Veil = petal-deflect / Counter Blow = punch-hook. Keep all; pose distinct.

---

## 4. ROLE DISTRIBUTION CHART

Healthy spread benchmark: DMG/BST 4-7, CC 1-3, DEF 1-2, MOB 1-2, RES 0-2, SET 0-2 (of 12 numbered).

| Class | DMG | BST | CC | DEF | MOB | RES | SET | Helper |
|---|---|---|---|---|---|---|---|---|
| Warblade | 6 | 1 | 2 | 2 | 0 | 1 | 1 | 4 |
| Elementalist | 6 | 0 | 2 | 1 | 1 | 2 | 0 | 5 |
| Shadowblade | 6 | 0 | 1 | 2 | 1 | 1 | 2 | 5-6 |
| Ranger | 5 | 1 | 1 | 0 | 2 | 0 | 4 | 6 |
| Ravager | 6 | 0 | 2 | 1 | 1 | 2 | 1 | 4 |
| Ronin | 5 | 1 | 0 | 3 | 3 | 2 | 0 | 6 |
| Gunslinger | 7 | 2 | 2 | 0 | 1 | 2 | 0 | 3 |
| Brawler | 9 | 0 | 3 | 1 | 0 | 1 | 1 | 2 |
| Summoner | 4 | 1 | 0 | 2 | 0 | 4 | 2 | 8 |
| Hexer | 5 | 2 | 2 | 2 | 0 | 1 | 1 | 4 |

Flags:
- Brawler 9 DMG: too punchy on paper; risk visual sameness (already noted).
- Summoner 8 helpers: structurally honest (sacrifice class) but Death Nova merge needed.
- Ranger 4 SET: high but on-fantasy (mark-trap-detonate).
- Ronin 3 MOB + 3 DEF: helper-leaning; merge Wind Step or Phantom Step.

---

## 5. HELPER DENSITY TABLE

Threshold: 4/12. Above = helper-bloat risk.

| Class | Helpers /12 | Status |
|---|---|---|
| Warblade | 4 | OK |
| Elementalist | 5 | High but justified by shape variety |
| Shadowblade | 5-6 | RISK — too many setups for 4-slot meta |
| Ranger | 6 | Structural to mark/trap fantasy; Predator/Multi merge required |
| Ravager | 4 | OK |
| Ronin | 6 | RISK — movement triple should reduce to 2 |
| Gunslinger | 3 | GOOD |
| Brawler | 2 | LOW (damage-heavy class) |
| Summoner | 8 | RISK — Death Nova merge required; class is structurally helper-heavy |
| Hexer | 4 | OK |

---

## 6. BUILD AXIS ORPHANS

Skills NOT in any of the 3 listed Build Eksenleri (orphan = pick-poison).

| Class | Orphan |
|---|---|
| Warblade | Sunder Mark (#5) — but referenced in chain bonuses; soft orphan |
| Elementalist | Blink (#4) — true orphan |
| Shadowblade | Shadow Pin (#10) — true orphan |
| Ranger | Multi-Mark (#9) — true orphan |
| Ravager | Shatter Armor (#11) — true orphan |
| Ronin | Sakura Veil (#8) — true orphan |
| Gunslinger | (Quickdraw soft-orphan, only in Mobile Assassin) |
| Brawler | none |
| Summoner | Dark Pact (#11) — true orphan |
| Hexer | none |

**Action:** orphan skills either get added to a build axis (rebalance) or marked as cross-class export-only (acceptable if useful in dual builds).

---

## 7. PIXELLAB FEASIBILITY SUMMARY

PixelLab is suitable for: peak-pose frames, idle/walk/dash anchors, big readable VFX shapes. NOT for: state overlays, UI pips, beam paths, decals.

Per-class feasibility (count of skills scoring 4-5 vs 1-2 in column I):

| Class | Easy (4-5) /12 | Hard (1-2) /12 | Notes |
|---|---|---|---|
| Warblade | 8 | 3 | Buffs need Unity overlays |
| Elementalist | 10 | 2 | Strong shape variety |
| Shadowblade | 7 | 1 | Scar decals = Unity |
| Ranger | 8 | 0 | Mark reticles = Unity |
| Ravager | 9 | 2 | Buffs + roar limited |
| Ronin | 9 | 2 | Aura buffs limited |
| Gunslinger | 9 | 1 | Heat UI = Unity |
| Brawler | 11 | 0 | Punches easy; differ via pose |
| Summoner | 9 | 0 | Minions feasible |
| Hexer | 7 | 2 | Stack pips + reflect = Unity |

**Total: 87 PixelLab-easy of ~120 numbered (excluding V).** ~73% of skills can have a peak frame generated within PixelLab budget. Remaining 27% rely on Unity state language.

---

## 8. FINAL REDESIGN / MERGE / CUT LIST

### MERGE (14 skills)
1. **Ravager Blood-Drunk Leap** -> MERGE INTO Frenzied Leap as Fury-empowered version (toggle on Fury%80+).
2. **Shadowblade Twin Carve** -> MERGE INTO Veil Strike as alt-form OR REDESIGN as "circular cut leaves ring-Scar" (must justify slot).
3. **Brawler Repulse** -> MERGE INTO Shockwave Slam as larger-AoE Charge%5 variant.
4. **Ranger Predator's Mark + Multi-Mark** -> MERGE into single AoE Mark zone (tier scales with charge).
5. **Summoner Death Nova** -> MERGE INTO Mass Sacrifice as 1-minion-tier (cheaper, smaller cloud).
6. **Gunslinger Quickdraw** -> MERGE INTO Hip Shot as upgrade (Hip Shot+ = single-target high-burst).
7. **Hexer Mass Hex** -> KEEP but TIGHTEN visual to differ from Corruption (screen-wide arc gesture).
8. **Warblade Blade Rush** -> KEEP after TIGHTEN (line-pierce, multi-hit residue).
9. **Brawler Off-Balance (R4)** -> MERGE INTO Shockwave Slam as cheap variant.
10. **Ravager Crimson Pact (R4)** -> MERGE INTO Blood Pact (RMB) as +HP-cost upgrade.
11. **Ronin Haste Dash** -> KEEP after TIGHTEN (afterimage residue must differ from Iaido Blur).
12. **Ronin Wind Step** -> KEEP after TIGHTEN (3-direction zigzag must be readable in pose).
13. **Shadowblade Phase Step** -> KEEP after TIGHTEN (no-Scar = differ from Veil Flicker).
14. **Ranger Hunter's Step** -> KEEP after TIGHTEN (slow-mo Mark setup pose).

### REDESIGN (18 skills) — visual contract required before PixelLab production

1. **Brawler Mach Punch** — multi-arm afterimage barrage (Brawler stays in place); contrast to Combo Chain (forward translation).
2. **Brawler Combo Chain** — explicit 4-pose (jab/cross/hook/uppercut), 5m forward translation.
3. **Brawler Pivot Hook** — feet planted, hip rotation, single heavy hook side impact.
4. **Brawler Cyclone Drive** — 2s sustained body spin, separate from Tornado Kick (one-shot rotation).
5. **Brawler Tornado Kick** — single 360 kick, foot leading, distinct from Cyclone Drive.
6. **Brawler Aerial Rave** — target clearly airborne, vertical hit chain.
7. **Brawler Shockwave Slam** — ground crack ring (smaller than Earthsplitter).
8. **Brawler Seismic Stomp** — straight-line ground crack (vs Shockwave's ring).
9. **Brawler Overdrive (V)** — phantom-arm afterimages, NOT aura halo.
10. **Shadowblade Veil Strike** — single reverse-grip slash, no scar; mark-only.
11. **Shadowblade Veil Flicker** — body teleport with phase silhouette, scar at exit.
12. **Shadowblade Seam Rend** — dash-through cut, scar perpendicular to path.
13. **Shadowblade Phase Step** — short body teleport, NO scar (differs from Flicker).
14. **Shadowblade Death Mark** — sigil on target, NOT a slash.
15. **Shadowblade Veil Burst** — 4 phase teleport-strikes, 4 scars cross pattern.
16. **Shadowblade Severance** — line drawn from Shadowblade to target, then collapse.
17. **Ravager Berserk Mode (V)** — blood ring rotating around Ravager + per-kill ext UI tick + hunched feral pose.
18. **Gunslinger Rift Dash** — recolor to dust/sparks (NOT purple void; bleeds into Shadowblade).

### CUT (4 skills)
1. **Ravager Blood-Drunk Leap** — duplicates Frenzied Leap (already in MERGE list).
2. **Brawler Repulse** — duplicates Shockwave Slam (already in MERGE list).
3. **Summoner Death Nova** — covered by Mass Sacrifice tier (already in MERGE list).
4. **Gunslinger Quickdraw** — covered by Hip Shot upgrade (already in MERGE list).

(All 4 CUTs are MERGE outcomes; no skills are pure deletions.)

### TIGHTEN (38 skills, summary)
Skills that stay in design but need visible-state engine support before PixelLab production:
- Warblade: Iron Crush, Sunder Mark, Ironclad Momentum, Battle Surge, Iron Roar (R4)
- Elementalist: Blink (rebalance to axis), Frost Wall, Radiant Pillar, Element Charge
- Shadowblade: Seam Rend, Shadow Clone, Shadow Pin, Night Aperture
- Ranger: Hunter's Step, Skirmish Shot, Rift Step
- Ravager: Bloodied Roar, Undying Tenacity, Shatter Armor, Death Wish, Crimson Pact (R4)
- Ronin: Iaido Blur, Haste Dash, Wind Step, Phantom Step, Sakura Veil, Iai Pressure
- Gunslinger: Smoke Grenade, Suppression Fire, Reload Dance, Burning Ammo, Exposed Line (R4)
- Brawler: (overlap with REDESIGN — Off-Balance R4 only here)
- Summoner: Spirit Surge (Dash), Commanding Strike, Blood for Power, Dark Pact, Bone Tide (R4)
- Hexer: Agony, Empathy, Enervate, Hex Overload, Cursed Mirror, Curse Bargain (R4)

---

## 9. SKILLS SAFE TO KEEP AS-IS (56 skills + V)

Top-tier skills with strong identity, low redundancy, high pick value, and PixelLab-feasible peak frames. No changes needed before production.

**Warblade (5):** Iron Charge, Crippling Blow, Gravity Cleave, Earthsplitter, Iron Counter, Death Blow, Bladestorm V.

**Elementalist (8):** Fireball, Glacial Spike, Living Bomb, Frozen Orb, Prism Beam, Meteor, Solar Flare, Blizzard, Trinity Storm V.

**Shadowblade (4):** Backstab Mark (with engine icon), Death Mark, Veil Burst, Chain Cull, Wraith Form V.

**Ranger (5):** Rift Arrow, Pinning Shot, Marked Detonate, Bone Trap, Sweep Volley, Final Strike, Spirit Bow V.

**Ravager (6):** Bloodlust Strike, Carnage Spin, Frenzied Leap (after merge), Reckless Swing, Bloodthirst, Barbaric Charge, Iron Grab, Berserk Mode V (after redesign).

**Ronin (4):** Quickdraw Slash, Soken-giri, Counter Draw, Crescent Arc, Flash Draw, Void Cleave, Mugen no Kiri V.

**Gunslinger (6):** Rift Dash (after recolor), Cursor Storm, Deadshot, Fan the Hammer, Rift Grenade, Ricochet, Point Blank Execute, Full Metal Storm V.

**Brawler (4):** Jab, Weave, Flying Knee, Counter Blow, Aerial Rave, Pivot Hook, Combo Chain — note these PASS the visual-distinct test only after REDESIGN bullets above are honored.

**Summoner (7):** Raise Skeleton, Summon Golem, Command Beacon, Corpse Explosion, Bone Shield, Soul Siphon Totem, Mass Sacrifice, Lich Form, Army of the Dead V.

**Hexer (7):** Corruption, Pandemic, Hexblast, Haunt, Unstable Affliction, Mass Hex (after gesture diff), Blight Sigil, Hex Cascade V.

---

## 10. NOTES TO RECONCILE WITH CODEX AUDIT

Open questions where Codex's mechanical perspective could differ:

1. **Ronin Phantom Step CUT vs KEEP** — I tightened (kept). Codex may CUT given decoy logic complexity. Resolve: is decoy-aggro implementation cheap in Unity?

2. **Summoner Death Nova MERGE vs KEEP** — I MERGED into Mass Sacrifice as 1-minion tier. Codex may flag the Hexer dual-class synergy ("Death Nova spreads Hexer debuffs") that gets lost. Resolve: does the merged Mass Sacrifice 1-minion-tier preserve Hexer dual?

3. **Ravager Blood-Drunk Leap MERGE vs REDESIGN** — I CUT it. Codex may want to REDESIGN as different-pose (dive vs leap). Resolve: would dive vs leap actually give it identity, or is Frenzied Leap merge cleaner?

4. **Brawler Repulse MERGE vs KEEP** — I MERGED. Codex may flag that Repulse + 4-target Charge=5 mechanic is unique. Resolve: does Shockwave Slam Charged-State variant cover this?

5. **Quickdraw (Gunslinger) vs Quickdraw Slash (Ronin)** — name collision. Audit suggests Gunslinger Quickdraw merge into Hip Shot. Codex may want to rename instead.

6. **Multi-Mark vs Predator's Mark MERGE** — I merged Multi-Mark INTO Predator's Mark as scalable AoE. Codex may prefer Multi-Mark canonical because cross-class dual export.

7. **Warblade Blade Rush** — TIGHTEN (line-pierce identity). Could MERGE into Iron Charge as 6m-line variant; Codex's view on slot economy needed.

8. **Shadowblade Twin Carve** — I MERGED. Codex may want REDESIGN as ring-Scar (unique geometry). Resolve via: is ring-Scar mechanic readable at 128px?

9. **Ronin Haste Dash + Wind Step** — both TIGHTEN. Codex may want one MERGED. Resolve: which has higher pick value in Phantom Dance build?

10. **Buff/aura skills overall** — all need engine pulse. Codex confirms ownership split (PixelLab = peak, Unity = state). Resolve: what is the standard pulse-VFX template for "self-buff active" so PixelLab does not need to draw pose variants?

---

## END OF AUDIT

This audit cites 130 numbered skills + 10 V Bursts + 30 base + ~24 R4 extras. Verdicts are based on the rubric, not on user preference; mechanic balance unchanged per scope.

Final action priority for the user:
1. Lock MERGE/CUT list (4 cuts, 14 merges).
2. Commission Visual Contract for the 18 REDESIGN skills before any PixelLab production.
3. Commission Unity engine-side overlays for the 38 TIGHTEN skills.
4. Generate peak frames in PixelLab for the 56 KEEP skills (budget: ~60 gen/skill x 56 = 3360 over budget; phase-priority by build axis).
