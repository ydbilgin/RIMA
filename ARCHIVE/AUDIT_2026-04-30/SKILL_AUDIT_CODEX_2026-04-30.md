# SKILL AUDIT CODEX 2026-04-30

Independent audit. Claude audit files were not read. Inputs used: canonical class skill document, combat roster, master decisions, Faz3 secondary class notes, current status, and prior Codex visual feedback.

Scoring columns: A=standalone, B=in-class combo, C=cross-class combo, R=role, H=helper, X=build axis count, VD=visual distinct, PF=PixelLab feasibility, P=pick value, K=verdict.

Role codes: DMG, SETUP, CTRL, DEF, MOB, RES, BURST.

## 1. EXECUTIVE SUMMARY

Verdict counts across 192 audited rows:

| Verdict | Count |
|---|---:|
| KEEP | 75 |
| TIGHTEN | 72 |
| MERGE | 20 |
| REDESIGN | 24 |
| CUT | 1 |

Top problems:

1. HP execute gates conflict with MASTER #56 and still appear in multiple classes.
2. Movement skills are overrepresented and often weakly differentiated.
3. Helper density is high in Ranger, Shadowblade, Summoner, and Hexer.
4. Brawler has strong mechanics but too many "punch harder" siblings.
5. Ravager has good risk logic but red self-damage/frenzy visuals can collapse.
6. Ranger mark skills overlap: Rift Arrow, Predator's Mark, Multi-Mark.
7. Shadowblade phase skills overlap unless Scar ownership is stricter.
8. Summoner has many pickable helpers, but battlefield readability risk is high.
9. Hexer has strong math but stack/spread/blast needs clearer branching.
10. Warblade Death Blow is the clearest state-gate redesign target.

Top priority classes for redesign/tightening:

| Priority | Class | Reason |
|---:|---|---|
| 1 | Brawler | Mechanical promise high, visual/action redundancy high. |
| 2 | Shadowblade | Phase/scar/collapse identity not fully carried by skill list. |
| 3 | Ranger | Mark/trap/detonate works, but too many arrow helpers. |
| 4 | Ravager | Needs suffer/trade/frenzy split, not all blood impact. |
| 5 | Hexer | Needs stack branch clarity to avoid linear rotation lock. |

## 2. PER-CLASS AUDIT

### Warblade

| Skill | A | B | C | R | H | X | Redundancy | VD | PF | P | K | Why |
|---|---:|---:|---:|---|---|---:|---|---|---:|---:|---|---|
| LMB Iron Combo | 4 | 4 | 2 | DMG | N | 0 | Blade Rush, Deep Wound | Y | 5 | 5 | KEEP | Core loop is readable and useful. |
| RMB Rage Outlet | 4 | 3 | 2 | BURST | N | 0 | Gravity Cleave | Y | 4 | 4 | TIGHTEN | Needs distinct rage dump silhouette. |
| Dash Momentum Slam | 3 | 3 | 2 | MOB | N | 0 | Iron Charge | N | 4 | 3 | TIGHTEN | Must stay shoulder entry, not charge copy. |
| 1 Iron Charge | 5 | 5 | 5 | MOB | N | 1 | Momentum Slam, Blade Rush | Y | 5 | 5 | KEEP | Best engage and combo starter. |
| 2 Crippling Blow | 4 | 4 | 2 | DMG | N | 1 | Deep Wound | Y | 4 | 4 | KEEP | Anti-heal gives clear tactical reason. |
| 3 Iron Crush | 3 | 5 | 2 | SETUP | Y | 1 | Battle Surge | N | 3 | 4 | TIGHTEN | Strong but generic self-buff. |
| 4 Gravity Cleave | 4 | 5 | 3 | CTRL | N | 1 | Quake Slam, Earthsplitter | Y | 4 | 5 | KEEP | Pull plus stun conversion is strong. |
| 5 Sunder Mark | 3 | 5 | 4 | SETUP | Y | 1 | Shatter Armor, Iron Roar | Y | 3 | 5 | KEEP | State ownership is valuable. |
| 6 Earthsplitter | 4 | 4 | 4 | CTRL | N | 1 | Quake Slam, Seismic Stomp | Y | 4 | 4 | TIGHTEN | Needs line knockup, not radial slam. |
| 7 Ironclad Momentum | 3 | 4 | 2 | DEF | Y | 1 | Battle Surge | Y | 3 | 4 | KEEP | Defense-to-rage loop is clean. |
| 8 Iron Counter | 5 | 4 | 3 | DEF | N | 1 | Counter Draw, Counter Blow | Y | 4 | 5 | KEEP | Absorb/break counter identity is strong. |
| 9 Blade Rush | 4 | 3 | 3 | MOB | N | 0 | Iron Charge | N | 4 | 3 | MERGE WITH IRON CHARGE | Second dash line weakens slot economy. |
| 10 Battle Surge | 3 | 4 | 2 | DEF | Y | 1 | Ironclad Momentum | N | 3 | 4 | TIGHTEN | Healing trigger is useful but visually bland. |
| 11 Deep Wound | 3 | 4 | 2 | DMG | N | 1 | Crippling Blow | N | 4 | 3 | TIGHTEN | Bleed must differ from anti-heal strike. |
| 12 Death Blow | 4 | 5 | 2 | BURST | N | 3 | Severance, Final Strike | Y | 4 | 5 | REDESIGN | HP gate violates MASTER #56. |
| V Bladestorm | 5 | 3 | 2 | BURST | N | 0 | Carnage Spin, Cyclone Drive | Y | 5 | 5 | KEEP | Clear burst, but protect class flavor. |
| R4 Quake Slam | 4 | 4 | 3 | CTRL | N | 0 | Earthsplitter | N | 4 | 4 | MERGE WITH EARTHSPLITTER | Same ground crack family. |
| R4 Iron Roar | 4 | 4 | 3 | SETUP | Y | 0 | Sunder Mark | Y | 4 | 4 | KEEP | Good AoE Sundered applicator. |

Summary: Warblade is structurally healthy. The main flaw is too many ground/charge entries and HP-gated Death Blow. Keep Sundered/Broken as the class anchor.

### Elementalist

| Skill | A | B | C | R | H | X | Redundancy | VD | PF | P | K | Why |
|---|---:|---:|---:|---|---|---:|---|---|---:|---:|---|---|
| LMB Rift Bolt | 4 | 4 | 2 | DMG | N | 0 | Fireball | Y | 5 | 5 | KEEP | Good mobile filler and state drip. |
| RMB Element Switch | 3 | 5 | 3 | RES | Y | 0 | Element Charge | Y | 3 | 5 | KEEP | Defines class rhythm. |
| Dash Elemental Pulse | 3 | 4 | 3 | MOB | N | 0 | Solar Flare | Y | 4 | 4 | KEEP | Mobile caster outlet is useful. |
| 1 Fireball | 4 | 5 | 5 | DMG | N | 1 | Rift Bolt | Y | 5 | 5 | KEEP | Simple, exportable, combo-rich. |
| 2 Glacial Spike | 5 | 5 | 5 | CTRL | N | 1 | Blizzard | Y | 5 | 5 | KEEP | Best reaction bridge in kit. |
| 3 Living Bomb | 4 | 5 | 4 | BURST | N | 1 | Meteor, Rune Anchor | Y | 4 | 5 | KEEP | Delay payoff has strong flow. |
| 4 Blink | 4 | 4 | 4 | MOB | N | 0 | Phase Step, Rift Step | Y | 4 | 4 | TIGHTEN | Needs mage geometry, not rogue phase. |
| 5 Frozen Orb | 4 | 5 | 3 | CTRL | N | 1 | Blizzard | Y | 4 | 5 | KEEP | Moving control object is distinct. |
| 6 Prism Beam | 4 | 5 | 3 | DMG | N | 1 | Solar Flare | Y | 4 | 5 | KEEP | Light spender gives clear shape. |
| 7 Meteor | 5 | 5 | 4 | BURST | N | 2 | Living Bomb | Y | 5 | 5 | KEEP | Big payoff with slow/freeze dependency. |
| 8 Frost Wall | 4 | 4 | 4 | CTRL | Y | 1 | Frozen Orb | Y | 3 | 4 | KEEP | Wall placement creates real choices. |
| 9 Solar Flare | 4 | 4 | 3 | DMG | N | 1 | Prism Beam | Y | 4 | 4 | TIGHTEN | Cone must not become beam variant. |
| 10 Radiant Pillar | 3 | 5 | 2 | SETUP | Y | 1 | Element Charge | Y | 3 | 4 | KEEP | Radiant echo bridge is good. |
| 11 Element Charge | 3 | 4 | 2 | RES | Y | 1 | Radiant Pillar | N | 3 | 4 | TIGHTEN | Fire-only buff risks helper blandness. |
| 12 Blizzard | 4 | 5 | 3 | CTRL | N | 1 | Frozen Orb | Y | 4 | 5 | KEEP | Large control payoff earns slot. |
| V Trinity Storm | 5 | 4 | 3 | BURST | N | 0 | Radiant Pillar | Y | 5 | 5 | KEEP | Strong three-element capstone. |
| R4 Rune Anchor | 4 | 5 | 4 | SETUP | Y | 0 | Living Bomb | Y | 3 | 5 | KEEP | Triggerable rune deepens combo math. |
| R4 Element Trail | 3 | 3 | 3 | CTRL | Y | 0 | Dash Pulse | Y | 3 | 3 | TIGHTEN | Movement trail can become passive clutter. |

Summary: Elementalist is the healthiest design. Its risk is not mechanics, but too many spell helpers if shapes are not hard-separated.

### Shadowblade

| Skill | A | B | C | R | H | X | Redundancy | VD | PF | P | K | Why |
|---|---:|---:|---:|---|---|---:|---|---|---:|---:|---|---|
| LMB Veil Strike | 3 | 4 | 2 | SETUP | N | 0 | Twin Carve | N | 4 | 4 | TIGHTEN | Mark slash is too generic alone. |
| RMB Veil Flicker | 4 | 5 | 4 | MOB | N | 0 | Phase Step, Seam Rend | Y | 3 | 5 | KEEP | Best Scar applicator candidate. |
| Dash Seam Rend | 4 | 5 | 3 | MOB | N | 0 | Phase Step | Y | 3 | 5 | KEEP | Dash attack can own collapse setup. |
| 1 Phase Step | 4 | 4 | 5 | MOB | Y | 1 | Veil Flicker | N | 4 | 5 | TIGHTEN | Needs specific non-Scar role. |
| 2 Backstab Mark | 3 | 5 | 5 | SETUP | Y | 1 | Veil Strike | Y | 3 | 5 | KEEP | Exportable crit setup works. |
| 3 Shadow Clone | 3 | 3 | 2 | DEF | Y | 1 | Phantom Step | Y | 4 | 3 | TIGHTEN | Decoy value depends on AI clarity. |
| 4 Death Mark | 4 | 5 | 5 | BURST | N | 1 | Living Bomb | Y | 4 | 5 | KEEP | Delayed mark payoff is strong. |
| 5 Veil Burst | 4 | 5 | 5 | BURST | N | 1 | Chain Cull | Y | 4 | 5 | KEEP | Teleport strikes suit identity. |
| 6 Severance | 3 | 4 | 2 | BURST | N | 1 | Death Blow, Final Strike | N | 4 | 3 | REDESIGN | Low-HP execute conflicts with #56. |
| 7 Smoke Veil | 2 | 4 | 5 | DEF | Y | 1 | Smoke Grenade | N | 3 | 4 | TIGHTEN | Stealth helper needs solo payoff. |
| 8 Chain Cull | 4 | 5 | 4 | BURST | N | 1 | Veil Burst | Y | 4 | 5 | KEEP | Mark-to-mark hop is slot-worthy. |
| 9 Shadow Pin | 4 | 5 | 5 | CTRL | N | 1 | Pinning Shot | Y | 4 | 5 | KEEP | Root dagger supports combos well. |
| 10 Twin Carve | 4 | 4 | 3 | DMG | N | 1 | Veil Strike | N | 4 | 4 | TIGHTEN | Needs distinct back-cross movement. |
| 11 Night Aperture | 3 | 5 | 3 | SETUP | Y | 1 | Scar Echo | Y | 3 | 5 | KEEP | Scar mirroring creates unique ceiling. |
| V Wraith Form | 4 | 4 | 3 | BURST | N | 1 | Phase Step | Y | 4 | 5 | KEEP | Burst mobility is clear. |
| R4 Mirror Cut | 4 | 5 | 4 | MOB | N | 0 | Veil Flicker | Y | 3 | 5 | KEEP | Active Scar teleport is important. |
| R4 Scar Echo | 3 | 5 | 3 | SETUP | Y | 0 | Night Aperture | N | 3 | 4 | TIGHTEN | Good passive, but overlaps mirror Scar loop. |

Summary: Shadowblade can work only if Scar apply, Scar persist, and Scar collapse are different verbs. Current list still has too many phase/slash siblings.

### Ranger

| Skill | A | B | C | R | H | X | Redundancy | VD | PF | P | K | Why |
|---|---:|---:|---:|---|---|---:|---|---|---:|---:|---|---|
| LMB Rift Arrow | 4 | 5 | 5 | SETUP | N | 1 | Pinning Shot | Y | 5 | 5 | KEEP | Mark source is essential. |
| RMB Tactical Roll | 4 | 3 | 2 | MOB | N | 0 | Hunter's Step, Rift Step | Y | 4 | 4 | KEEP | Base reposition has clear identity. |
| Dash Attack | 3 | 3 | 2 | MOB | N | 0 | Hunter's Step | N | 4 | 3 | TIGHTEN | Define if separate from roll. |
| 1 Pinning Shot | 4 | 5 | 5 | CTRL | N | 1 | Bone Trap | Y | 4 | 5 | KEEP | Root bridge has high combo value. |
| 2 Marked Detonate | 4 | 5 | 5 | BURST | N | 1 | Final Strike | Y | 4 | 5 | KEEP | Mark payoff is required. |
| 3 Hunter's Step | 3 | 4 | 5 | MOB | Y | 1 | Tactical Roll, Rift Step | N | 4 | 4 | TIGHTEN | Too many Ranger movement helpers. |
| 4 Bone Trap | 4 | 5 | 5 | CTRL | Y | 1 | Wireline Trap | Y | 3 | 5 | KEEP | Trap class identity is strong. |
| 5 Sweep Volley | 4 | 4 | 4 | DMG | N | 1 | Spirit Bow | Y | 5 | 4 | KEEP | Cone clear gives non-mark damage. |
| 6 Skirmish Shot | 3 | 3 | 3 | MOB | N | 1 | Hunter's Step | N | 4 | 3 | MERGE WITH TACTICAL ROLL | Moving shot is base-kit material. |
| 7 Predator's Mark | 3 | 5 | 5 | SETUP | Y | 1 | Multi-Mark | N | 3 | 5 | TIGHTEN | Must be zone mark, not multi-mark. |
| 8 Multi-Mark | 3 | 5 | 3 | SETUP | Y | 1 | Predator's Mark | N | 3 | 4 | MERGE WITH PREDATOR'S MARK | Two mass-mark helpers compete. |
| 9 Final Strike | 4 | 5 | 3 | BURST | N | 1 | Marked Detonate | N | 4 | 4 | REDESIGN | Remove low-HP execute gate. |
| 10 Rift Step | 3 | 4 | 5 | MOB | Y | 1 | Hunter's Step | N | 4 | 3 | MERGE WITH HUNTER'S STEP | Second short dash is redundant. |
| V Spirit Bow | 5 | 4 | 3 | BURST | N | 1 | Sweep Volley | Y | 5 | 5 | KEEP | Strong burst and mark-all fantasy. |
| R4 Wireline Trap | 4 | 5 | 4 | CTRL | Y | 0 | Bone Trap | Y | 2 | 5 | KEEP | Great trap identity, hard PixelLab read. |
| R4 Quiver Pulse | 4 | 5 | 4 | BURST | Y | 0 | Marked Detonate | Y | 3 | 5 | KEEP | Mark network payoff is valuable. |
| R4 Hawk Eye | 2 | 2 | 2 | SETUP | Y | 0 | Rift Arrow | N | 4 | 2 | CUT | Already merged; keep as upgrade only. |

Summary: Ranger has the right macro plan, but too many helper/movement/mark entries. Consolidate mass mark and dash variants.

### Ravager

| Skill | A | B | C | R | H | X | Redundancy | VD | PF | P | K | Why |
|---|---:|---:|---:|---|---|---:|---|---|---:|---:|---|---|
| LMB Brutal Swing | 4 | 4 | 2 | DMG | N | 0 | Bloodlust Strike | Y | 5 | 5 | KEEP | Good fury and heavy rhythm. |
| RMB Blood Pact | 4 | 5 | 5 | RES | Y | 0 | Crimson Pact, Dark Pact | Y | 4 | 5 | KEEP | HP trade identity is essential. |
| Dash Fury Tackle | 4 | 4 | 3 | MOB | N | 0 | Barbaric Charge | Y | 4 | 4 | KEEP | Body tackle differs from weapon charge. |
| 1 Bloodlust Strike | 4 | 5 | 3 | DMG | N | 1 | Bloodthirst | Y | 5 | 5 | KEEP | HP-scaled cone is pickable. |
| 2 Carnage Spin | 4 | 4 | 3 | DMG | N | 1 | Bladestorm | Y | 5 | 4 | TIGHTEN | Keep brute debris, not elegant spin. |
| 3 Frenzied Leap | 4 | 4 | 3 | MOB | N | 1 | Blood-Drunk Leap | N | 4 | 4 | MERGE WITH BLOOD-DRUNK LEAP | Two leap attacks compete. |
| 4 Reckless Swing | 5 | 5 | 3 | BURST | N | 1 | Death Wish | Y | 5 | 5 | KEEP | Vulnerability-for-power is excellent. |
| 5 Bloodthirst | 4 | 4 | 3 | DMG | N | 1 | Bloodlust Strike | N | 4 | 4 | TIGHTEN | Lifesteal multi-hit needs clearer niche. |
| 6 Bloodied Roar | 3 | 5 | 3 | CTRL | Y | 1 | Iron Roar | Y | 4 | 5 | KEEP | Stagger setup supports low-HP burst. |
| 7 Barbaric Charge | 4 | 4 | 3 | MOB | N | 1 | Fury Tackle | N | 4 | 3 | TIGHTEN | Needs wall collision identity. |
| 8 Undying Tenacity | 3 | 5 | 3 | DEF | Y | 1 | Death Wish | N | 2 | 5 | MERGE WITH DEATH WISH | Fatal-save and undying overlap. |
| 9 Iron Grab | 4 | 4 | 3 | CTRL | N | 1 | Brawler wall skills | Y | 3 | 4 | TIGHTEN | Grab/throw may violate pixel constraint. |
| 10 Blood-Drunk Leap | 4 | 5 | 3 | MOB | N | 1 | Frenzied Leap | N | 4 | 4 | MERGE WITH FRENZIED LEAP | Fury spend leap should be upgrade branch. |
| 11 Shatter Armor | 3 | 4 | 4 | SETUP | Y | 1 | Sunder Mark | N | 3 | 4 | REDESIGN | Armor break ownership collides with Warblade. |
| 12 Death Wish | 4 | 5 | 3 | DEF | Y | 1 | Undying Tenacity | Y | 3 | 5 | KEEP | Core risk window works. |
| V Berserk Mode | 5 | 5 | 3 | BURST | N | 0 | Bladestorm, Overdrive | Y | 5 | 5 | KEEP | Kill-chain extension is strong. |
| R4 Wound Echo | 4 | 5 | 4 | BURST | Y | 0 | Iron Counter | Y | 3 | 5 | KEEP | Damage-taken payoff fits perfectly. |
| R4 Pain Reservoir | 3 | 4 | 3 | RES | Y | 0 | Death Wish | N | 2 | 4 | TIGHTEN | Passive multiplier risks invisible power. |
| R4 Crimson Pact | 4 | 5 | 5 | RES | Y | 0 | Blood Pact | N | 4 | 5 | MERGE WITH BLOOD PACT | Same HP-pay fantasy. |

Summary: Ravager's engine is good, but leap and undying pairs should be consolidated. Avoid Warblade armor-break ownership drift.

### Ronin

| Skill | A | B | C | R | H | X | Redundancy | VD | PF | P | K | Why |
|---|---:|---:|---:|---|---|---:|---|---|---:|---:|---|---|
| LMB Sheath Walk | 4 | 4 | 2 | DMG | N | 0 | Quickdraw Slash | Y | 4 | 5 | KEEP | Good always-on flow builder. |
| RMB Drawn Edge | 5 | 5 | 3 | BURST | N | 0 | Quickdraw Slash | Y | 4 | 5 | KEEP | Defines draw timing. |
| Dash Iaido Blur | 4 | 4 | 3 | MOB | N | 0 | Haste Dash | N | 4 | 4 | TIGHTEN | Needs dash finisher, not generic fast cut. |
| 1 Quickdraw Slash | 5 | 5 | 3 | BURST | N | 1 | Drawn Edge | Y | 4 | 5 | KEEP | Signature one-button payoff. |
| 2 Haste Dash | 4 | 4 | 4 | MOB | N | 1 | Wind Step | N | 4 | 4 | TIGHTEN | Must not become second Wind Step. |
| 3 Soken-giri | 4 | 4 | 2 | DMG | N | 1 | Crescent Arc | Y | 4 | 4 | KEEP | Fan slash gives wave-clear. |
| 4 Iaido Stance | 3 | 5 | 2 | SETUP | Y | 1 | Stillness | Y | 3 | 5 | KEEP | Stance makes wait/draw playable. |
| 5 Wind Step | 4 | 5 | 4 | MOB | N | 2 | Haste Dash | Y | 4 | 5 | KEEP | Direction sequence is distinct. |
| 6 Counter Draw | 5 | 5 | 3 | DEF | N | 0 | Iron Counter, Counter Blow | Y | 4 | 5 | KEEP | Pre-draw counter identity is clear. |
| 7 Phantom Step | 3 | 4 | 4 | DEF | Y | 1 | Shadow Clone | N | 4 | 4 | TIGHTEN | Deception overlaps Shadowblade. |
| 8 Sakura Veil | 4 | 5 | 3 | DEF | N | 0 | Counter Draw | Y | 4 | 5 | KEEP | Deflect window supports V fill. |
| 9 Crescent Arc | 4 | 4 | 2 | DMG | N | 1 | Soken-giri | Y | 4 | 4 | TIGHTEN | Circular clear must differ from fan cuts. |
| 10 Flash Draw | 4 | 4 | 3 | MOB | N | 1 | Chain Cull | Y | 4 | 4 | REDESIGN | Low-HP execute clause violates #56. |
| 11 Iai Pressure | 3 | 5 | 2 | SETUP | Y | 1 | Element Charge | N | 3 | 4 | KEEP | Makes dash branch meaningful. |
| 12 Void Cleave | 5 | 5 | 3 | BURST | N | 1 | Death Blow | Y | 4 | 5 | KEEP | Excellent directional finisher. |
| V Mugen no Kiri | 5 | 4 | 3 | BURST | N | 0 | Overdrive | Y | 4 | 5 | KEEP | Strong speed fantasy. |
| R4 Stillness | 3 | 5 | 2 | RES | Y | 0 | Iaido Stance | N | 2 | 4 | MERGE WITH IAIDO STANCE | Motionless tension duplicates stance. |
| R4 Sheath Pressure | 3 | 4 | 2 | RES | Y | 0 | Stillness | N | 2 | 3 | TIGHTEN | Passive tension risks invisible helper. |
| R4 Wind Read | 4 | 5 | 3 | DEF | Y | 0 | Sakura Veil | Y | 3 | 5 | KEEP | Whiff-read expands counter identity. |

Summary: Ronin is mostly solid. Biggest risk is fast-slash overlap with Shadowblade; keep negative space and Opened-state timing central.

### Gunslinger

| Skill | A | B | C | R | H | X | Redundancy | VD | PF | P | K | Why |
|---|---:|---:|---:|---|---|---:|---|---|---:|---:|---|---|
| LMB Dual Fire | 4 | 4 | 2 | DMG | N | 0 | Quickdraw | Y | 5 | 5 | KEEP | Mobile fire loop is clear. |
| RMB Hip Shot | 4 | 4 | 3 | MOB | N | 0 | Quickdraw, Rift Dash | Y | 4 | 5 | KEEP | Lateral gun handling is valuable. |
| Dash Crossfire Entry | 4 | 4 | 3 | MOB | N | 0 | Rift Dash | Y | 4 | 4 | TIGHTEN | Must be entry burst, not second slide. |
| 1 Rift Dash | 5 | 5 | 4 | MOB | N | 1 | Crossfire Entry | Y | 4 | 5 | KEEP | Signature movement attack. |
| 2 Quickdraw | 4 | 5 | 3 | BURST | N | 1 | Deadshot | N | 5 | 4 | TIGHTEN | Needs snap-shot identity. |
| 3 Cursor Storm | 5 | 5 | 3 | CTRL | N | 2 | Suppression Fire | Y | 4 | 5 | KEEP | Area fire solves crowd role. |
| 4 Deadshot | 5 | 4 | 3 | BURST | N | 1 | Point Blank Execute | Y | 5 | 5 | KEEP | Precision line is distinct. |
| 5 Smoke Grenade | 3 | 4 | 5 | CTRL | Y | 1 | Smoke Veil | N | 3 | 4 | TIGHTEN | Needs gun class utility, not stealth copy. |
| 6 Fan the Hammer | 5 | 5 | 3 | DMG | N | 1 | Dual Fire | Y | 5 | 5 | KEEP | Heat spike is satisfying. |
| 7 Suppression Fire | 4 | 4 | 3 | CTRL | N | 1 | Cursor Storm | Y | 4 | 4 | KEEP | Push line has tactical use. |
| 8 Rift Grenade | 4 | 5 | 5 | BURST | N | 0 | Cursor Storm | Y | 4 | 5 | KEEP | Delayed root/stun payoff is strong. |
| 9 Ricochet | 4 | 5 | 3 | DMG | N | 0 | Fan the Hammer | Y | 4 | 4 | KEEP | Heat refund makes routing interesting. |
| 10 Reload Dance | 4 | 5 | 3 | RES | Y | 0 | Reload Roll | Y | 3 | 5 | KEEP | Reload rhythm needs a visible skill. |
| 11 Burning Ammo | 3 | 4 | 4 | SETUP | Y | 1 | Elementalist fire | N | 3 | 4 | TIGHTEN | Bullet DoT risks generic buff. |
| 12 Point Blank Execute | 4 | 5 | 3 | BURST | N | 1 | Deadshot | Y | 4 | 4 | REDESIGN | Execute naming/gate needs state conversion. |
| V Full Metal Storm | 5 | 4 | 3 | BURST | N | 1 | Cursor Storm | Y | 5 | 5 | KEEP | Clear V identity. |
| R4 Empty Mag Burst | 4 | 5 | 4 | BURST | N | 0 | Deadshot | Y | 4 | 5 | KEEP | Last-bullet payoff is excellent. |
| R4 Reload Roll | 4 | 5 | 3 | MOB | Y | 0 | Reload Dance | N | 4 | 4 | MERGE WITH RELOAD DANCE | Same reload-movement family. |
| R4 Exposed Line | 4 | 5 | 4 | SETUP | Y | 0 | Deadshot | Y | 3 | 5 | KEEP | Heat MAX risk line is valuable. |

Summary: Gunslinger is strong if reload/heat gets equal weight with muzzle flash. Consolidate reload movement and fix execute language.

### Brawler

| Skill | A | B | C | R | H | X | Redundancy | VD | PF | P | K | Why |
|---|---:|---:|---:|---|---|---:|---|---|---:|---:|---|---|
| LMB Jab | 3 | 5 | 2 | RES | N | 0 | Mach Punch | Y | 4 | 5 | KEEP | Basic charge builder is mandatory. |
| RMB Weave | 4 | 5 | 3 | DEF | Y | 0 | Counter Blow | Y | 3 | 5 | KEEP | Whiff timing defines class. |
| Dash Flying Knee | 4 | 4 | 3 | MOB | N | 0 | Aerial Rave | Y | 4 | 4 | KEEP | Aggressive entry is distinct enough. |
| 1 Mach Punch | 4 | 5 | 3 | DMG | N | 1 | Jab, Combo Chain | N | 4 | 4 | TIGHTEN | Needs multi-hit visual contract. |
| 2 Shockwave Slam | 4 | 4 | 3 | CTRL | N | 1 | Seismic Stomp, Quake Slam | N | 4 | 4 | MERGE WITH SEISMIC STOMP | Two ground shock skills compete. |
| 3 Tornado Kick | 4 | 4 | 2 | DMG | N | 0 | Cyclone Drive | N | 4 | 3 | MERGE WITH CYCLONE DRIVE | Spin kick and spin drive overlap. |
| 4 Combo Chain | 5 | 5 | 3 | DMG | N | 1 | Mach Punch | Y | 4 | 5 | KEEP | Core combo chain earns slot. |
| 5 Guard Break | 4 | 5 | 4 | SETUP | N | 1 | Pin Strike | Y | 3 | 5 | KEEP | Defense break supports payoff. |
| 6 Repulse | 3 | 4 | 2 | CTRL | N | 1 | Shockwave Slam | N | 4 | 3 | TIGHTEN | Push ring must avoid generic AoE. |
| 7 Counter Blow | 5 | 5 | 3 | DEF | N | 1 | Weave | Y | 4 | 5 | KEEP | Counter archetype is clear. |
| 8 Aerial Rave | 5 | 5 | 4 | CTRL | N | 1 | Flying Knee | Y | 3 | 5 | KEEP | Launch/juggle is key differentiator. |
| 9 Cyclone Drive | 4 | 4 | 2 | DMG | N | 1 | Tornado Kick | N | 4 | 3 | TIGHTEN | Could be upgraded Tornado branch. |
| 10 Seismic Stomp | 4 | 5 | 4 | CTRL | N | 1 | Shockwave Slam | Y | 4 | 5 | KEEP | Line launch gives strong combo payoff. |
| 11 Pivot Hook | 5 | 5 | 3 | BURST | N | 1 | Mach Punch | Y | 4 | 5 | KEEP | Best single-hit identity. |
| 12 Unstoppable Force | 4 | 5 | 3 | BURST | Y | 1 | Overdrive | N | 3 | 5 | REDESIGN | Too close to V burst. |
| V Overdrive | 5 | 5 | 3 | BURST | N | 1 | Unstoppable Force | Y | 4 | 5 | KEEP | Strong capstone. |
| R4 Pulverize | 4 | 5 | 4 | BURST | N | 0 | Combo Chain | Y | 4 | 5 | KEEP | Cracked detonation gives state payoff. |
| R4 Off-Balance | 4 | 5 | 3 | SETUP | Y | 0 | Guard Break | Y | 3 | 5 | KEEP | Whiff/body state is useful. |
| R4 Glass Strike | 4 | 5 | 4 | BURST | N | 0 | Pivot Hook | Y | 4 | 5 | KEEP | Shattered/Sundered consumer is good. |
| R4 Wall Slam Combo | 4 | 5 | 3 | CTRL | N | 0 | Iron Grab | Y | 2 | 4 | TIGHTEN | Needs no-wall fallback in implementation. |
| R4 Pin Strike | 4 | 5 | 4 | SETUP | N | 0 | Guard Break | Y | 3 | 5 | KEEP | Pinned branch adds body-shot window. |

Summary: Brawler needs the most consolidation. Keep weave, launch, cracked/shattered, pivot hook; merge duplicate spins and slams.

### Summoner

| Skill | A | B | C | R | H | X | Redundancy | VD | PF | P | K | Why |
|---|---:|---:|---:|---|---|---:|---|---|---:|---:|---|---|
| LMB Command Strike | 4 | 5 | 3 | SETUP | N | 0 | Commanding Strike | Y | 3 | 5 | KEEP | Never-empty command loop is correct. |
| RMB Soul Dart | 4 | 5 | 3 | SETUP | N | 0 | Command Beacon | Y | 4 | 5 | KEEP | Single-target command separates from LMB. |
| Dash Spirit Surge | 3 | 4 | 2 | MOB | Y | 0 | Command Strike | N | 3 | 3 | TIGHTEN | Retarget dash risks weak slot read. |
| 1 Raise Skeleton | 4 | 5 | 3 | SETUP | Y | 1 | Bone Tide | Y | 4 | 5 | KEEP | Baseline minion source. |
| 2 Summon Golem | 4 | 5 | 3 | DEF | Y | 1 | Bone Shield | Y | 4 | 5 | KEEP | Tank body changes battlefield. |
| 3 Command Beacon | 3 | 5 | 4 | SETUP | Y | 1 | Soul Dart | Y | 3 | 5 | KEEP | Mass command is important. |
| 4 Corpse Explosion | 5 | 5 | 4 | BURST | N | 1 | Mass Sacrifice | Y | 4 | 5 | KEEP | Best corpse payoff. |
| 5 Death Nova | 4 | 5 | 5 | BURST | N | 1 | Corpse Explosion | Y | 4 | 5 | KEEP | Sacrifice cloud supports Hexer. |
| 6 Commanding Strike | 4 | 5 | 3 | BURST | N | 1 | Command Strike | N | 3 | 4 | TIGHTEN | Needs selected-minion spotlight. |
| 7 Blood for Power | 4 | 5 | 4 | RES | Y | 1 | Mass Sacrifice | Y | 3 | 5 | KEEP | Sacrifice-to-resource engine is good. |
| 8 Bone Shield | 3 | 5 | 3 | DEF | Y | 1 | Summon Golem | Y | 3 | 4 | KEEP | Minion-as-shield adds defense branch. |
| 9 Soul Siphon Totem | 3 | 5 | 3 | RES | Y | 1 | Command Beacon | Y | 3 | 4 | TIGHTEN | Totem can add clutter. |
| 10 Mass Sacrifice | 5 | 5 | 4 | BURST | N | 1 | Corpse Explosion | Y | 4 | 5 | KEEP | Strong class-defining payoff. |
| 11 Dark Pact | 3 | 5 | 5 | RES | Y | 1 | Blood Pact | Y | 3 | 5 | KEEP | HP trade summoning opens dual class. |
| 12 Lich Form | 4 | 5 | 3 | BURST | Y | 1 | Army of the Dead | Y | 4 | 5 | KEEP | Sacrifice multiplier has high ceiling. |
| V Army of the Dead | 5 | 4 | 3 | BURST | N | 0 | Bone Tide | Y | 4 | 5 | KEEP | Strong fantasy, needs VFX budget. |
| R4 Bone Tide | 4 | 4 | 3 | SETUP | Y | 0 | Raise Skeleton | N | 3 | 4 | TIGHTEN | Mass summon may overwhelm cap/readability. |
| R4 Soul Tax | 3 | 5 | 3 | RES | Y | 0 | Blood for Power | N | 2 | 3 | REDESIGN | Delayed summon is hard to value. |
| R4 Beacon Pull | 3 | 5 | 3 | SETUP | Y | 0 | Command Beacon | Y | 3 | 4 | KEEP | Recall solves minion control problem. |

Summary: Summoner has many helpers but they are functionally different. The danger is not slot value; it is visual overload and minion command readability.

### Hexer

| Skill | A | B | C | R | H | X | Redundancy | VD | PF | P | K | Why |
|---|---:|---:|---:|---|---|---:|---|---|---:|---:|---|---|
| LMB Hex Bolt | 4 | 5 | 2 | SETUP | N | 0 | Corruption | Y | 4 | 5 | KEEP | Stack filler is essential. |
| RMB Curse Grasp | 4 | 5 | 3 | SETUP | N | 0 | Corruption | Y | 3 | 5 | KEEP | Hold pressure gives active rhythm. |
| Dash Curse Step | 3 | 4 | 2 | MOB | N | 0 | Mass Hex | Y | 3 | 4 | KEEP | Close stack burst is useful. |
| 1 Corruption | 4 | 5 | 4 | SETUP | N | 1 | Hex Bolt | Y | 4 | 5 | KEEP | Fast 3-stack opener works. |
| 2 Agony | 3 | 5 | 3 | DMG | N | 1 | Corruption | N | 3 | 4 | TIGHTEN | DoT must not be invisible upkeep. |
| 3 Pandemic | 4 | 5 | 5 | SETUP | Y | 1 | Mass Hex | Y | 3 | 5 | KEEP | Spread skill is central. |
| 4 Hexblast | 5 | 5 | 4 | BURST | N | 2 | Hex Cascade | Y | 4 | 5 | KEEP | Early cashout fixes rotation lock. |
| 5 Empathy | 3 | 5 | 3 | DEF | Y | 1 | Cursed Mirror | N | 3 | 4 | TIGHTEN | Reflection pair needs split. |
| 6 Haunt | 4 | 5 | 4 | SETUP | N | 1 | Corruption | Y | 4 | 5 | KEEP | Follow/tick/auto blast is distinct. |
| 7 Unstable Affliction | 3 | 4 | 4 | CTRL | Y | 1 | Hex Overload | Y | 3 | 4 | TIGHTEN | Dispel/heal trigger may be rare. |
| 8 Enervate | 3 | 4 | 3 | CTRL | Y | 0 | Frost slows | N | 3 | 3 | TIGHTEN | Generic slow needs Hex phase hook. |
| 9 Mass Hex | 4 | 5 | 3 | SETUP | Y | 1 | Pandemic | N | 3 | 5 | TIGHTEN | Screen-wide stack source risks autopick. |
| 10 Hex Overload | 4 | 5 | 4 | SETUP | Y | 2 | Blight Sigil | Y | 3 | 5 | KEEP | Damage-to-stack bridge is strong. |
| 11 Cursed Mirror | 3 | 4 | 3 | DEF | Y | 0 | Empathy | N | 3 | 3 | MERGE WITH EMPATHY | Two reflection curses compete. |
| 12 Blight Sigil | 4 | 5 | 3 | SETUP | Y | 1 | Hex Overload | Y | 3 | 5 | KEEP | Ground curse zone is valuable. |
| V Hex Cascade | 5 | 5 | 3 | BURST | N | 0 | Hexblast | Y | 4 | 5 | KEEP | Room-scale stack payoff is clear. |
| R4 Whisper Mark | 3 | 5 | 3 | SETUP | Y | 0 | Pandemic | N | 2 | 4 | TIGHTEN | Auto-spread can erase decisions. |
| R4 Curse Bargain | 4 | 5 | 4 | SETUP | Y | 0 | Blight Sigil | Y | 3 | 5 | KEEP | HP trade stack burst is useful. |

Summary: Hexer has the best long-form math, but many helpers are stack accelerants. Preserve distinct branches: stack, spread, reflect, sigil, blast.

## 3. CROSS-CLASS REDUNDANCY MAP

| Cluster | Canonical keeper | Others must differ by |
|---|---|---|
| Charge/dash engage | Iron Charge | Momentum Slam=shoulder, Rift Dash=gun slide, Haste Dash=draw, Hunter Step=position. |
| Short mobility helpers | Tactical Roll | Hunter Step crit, Rift Step void, Blink spell trigger, Phase Step stealth. |
| Spin AoE | Bladestorm | Carnage Spin=brutal defense shred, Cyclone Drive=body contact, Tornado Kick=merge. |
| Ground slam/crack | Earthsplitter | Quake Slam should merge; Shockwave/Seismic need Brawler state. |
| HP trade | Blood Pact | Dark Pact=summon, Curse Bargain=stacks, Crimson Pact merge with Blood Pact. |
| Reflection counter | Iron Counter | Counter Draw=pre-draw, Counter Blow=whiff/body, Empathy/Mirror merge. |
| Smoke/stealth | Smoke Veil | Smoke Grenade must be blindness/control, not stealth source. |
| Mark mass application | Predator's Mark | Multi-Mark merge; Quiver Pulse becomes payoff. |
| Delayed explosion | Living Bomb | Death Mark=assassin target, Rift Grenade=gun zone, Rune Anchor=trigger rune. |
| Low-HP execute | None | Convert to class-state gate per MASTER #56. |

## 4. ROLE DISTRIBUTION CHART

12 numbered skills only.

| Class | DMG/BURST | CTRL | MOB | DEF | RES | SETUP | Health |
|---|---:|---:|---:|---:|---:|---:|---|
| Warblade | 5 | 2 | 2 | 2 | 0 | 1 | PASS |
| Elementalist | 5 | 4 | 1 | 0 | 1 | 1 | PASS |
| Shadowblade | 5 | 1 | 2 | 2 | 0 | 2 | WARN control low |
| Ranger | 4 | 2 | 4 | 0 | 0 | 2 | WARN mobility high |
| Ravager | 5 | 2 | 3 | 2 | 0 | 0 | PASS |
| Ronin | 5 | 0 | 3 | 3 | 0 | 1 | WARN control low |
| Gunslinger | 6 | 3 | 1 | 0 | 1 | 1 | PASS |
| Brawler | 6 | 4 | 0 | 1 | 0 | 1 | PASS |
| Summoner | 4 | 0 | 0 | 2 | 3 | 3 | WARN helper-heavy |
| Hexer | 3 | 2 | 0 | 2 | 0 | 5 | WARN setup-heavy |

## 5. HELPER DENSITY TABLE

12 numbered skills only. Threshold flag: more than 4 helpers.

| Class | Helper count | Flag | Notes |
|---|---:|---|---|
| Warblade | 4 | OK | Mostly defense/setup, acceptable. |
| Elementalist | 4 | OK | Shape diversity keeps helpers playable. |
| Shadowblade | 5 | FLAG | Phase/stealth/mark helpers need sharper payoff. |
| Ranger | 5 | FLAG | Mark and movement helpers are crowded. |
| Ravager | 4 | OK | HP trade helpers are on-theme. |
| Ronin | 3 | OK | Stance/deception helpers manageable. |
| Gunslinger | 3 | OK | Reload and heat helpers are needed. |
| Brawler | 1 | OK | Low helper density, high action redundancy. |
| Summoner | 8 | FLAG | Expected for minion class, but UI clarity critical. |
| Hexer | 8 | FLAG | Stack accelerants risk autopilot. |

## 6. BUILD AXIS ORPHANS

Axis count 0 among numbered skills, excluding basics and R4:

| Class | Orphans | Risk |
|---|---|---|
| Warblade | Blade Rush | Medium: second dash line lacks build home. |
| Elementalist | Blink | Low: base mobility can stand alone. |
| Shadowblade | None | Good. |
| Ranger | None | Good, but too many movement skills anyway. |
| Ravager | None | Good. |
| Ronin | Counter Draw, Sakura Veil | Medium: V-fill skills need build-axis mention. |
| Gunslinger | Rift Grenade, Ricochet, Reload Dance | Medium: strong skills but axes omit them. |
| Brawler | Tornado Kick | High: no axis and overlaps Cyclone Drive. |
| Summoner | None | Good. |
| Hexer | Enervate, Cursed Mirror | High: both feel optional/weak. |

## 7. PIXELLAB FEASIBILITY SUMMARY

Rows counted across each class audit including basics, V, and R4.

| Class | PF 4-5 | PF 2-3 | PF 1 | Summary |
|---|---:|---:|---:|---|
| Warblade | 13 | 5 | 0 | Strong poses; state overlays should be Unity-owned. |
| Elementalist | 11 | 6 | 0 | Spell shapes feasible; rune/wall logic engine-owned. |
| Shadowblade | 9 | 8 | 0 | Cool phase easy; persistent Scar needs Unity. |
| Ranger | 7 | 10 | 0 | Archer easy; trap/mark endpoints need engine. |
| Ravager | 11 | 7 | 0 | Brutality easy; self-cost feedback engine-owned. |
| Ronin | 13 | 5 | 0 | Poses easy; timing/Open state engine-owned. |
| Gunslinger | 13 | 5 | 0 | Shooting easy; heat/reload UI engine-owned. |
| Brawler | 12 | 7 | 0 | Punches feasible; whiff/launch state harder. |
| Summoner | 7 | 11 | 0 | Concepts strong; production swarm readability risky. |
| Hexer | 7 | 10 | 0 | Sigils feasible; stack thresholds engine-owned. |

## 8. FINAL REDESIGN/MERGE/CUT LIST

### REDESIGN

| Skill | Reason |
|---|---|
| Warblade Death Blow | HP gate conflicts with MASTER #56; convert to Broken/Sundered gate. |
| Shadowblade Severance | Low-HP execute conflicts with #56; make Scar collapse finisher. |
| Ranger Final Strike | Low-HP execute conflicts; require Marked+Trapped or distance gate. |
| Ravager Shatter Armor | Armor-break ownership collides with Warblade Sunder. |
| Ronin Flash Draw | Remove HP execute clause; use Opened or Tension gate. |
| Gunslinger Point Blank Execute | Rename/reframe as Heat-zero or Exposed Line payoff. |
| Brawler Unstoppable Force | Too close to Overdrive; make charge routing modifier. |
| Summoner Soul Tax | Delayed value is hard to read; needs active command hook. |

### MERGE

| Skill | Merge target |
|---|---|
| Warblade Blade Rush | Iron Charge branch or upgrade. |
| Warblade Quake Slam | Earthsplitter branch. |
| Ranger Skirmish Shot | Tactical Roll branch. |
| Ranger Multi-Mark | Predator's Mark. |
| Ranger Rift Step | Hunter's Step. |
| Ravager Frenzied Leap | Blood-Drunk Leap. |
| Ravager Undying Tenacity | Death Wish. |
| Ravager Crimson Pact | Blood Pact. |
| Ronin Stillness | Iaido Stance. |
| Gunslinger Reload Roll | Reload Dance. |
| Brawler Shockwave Slam | Seismic Stomp. |
| Brawler Tornado Kick | Cyclone Drive. |
| Hexer Cursed Mirror | Empathy. |

### CUT

| Skill | Reason |
|---|---|
| Ranger Hawk Eye | Already marked as upgrade, not active skill. |

## 9. SKILLS SAFE TO KEEP AS-IS

High-confidence keep list:

Warblade: Iron Combo, Iron Charge, Crippling Blow, Gravity Cleave, Sunder Mark, Iron Counter, Bladestorm, Iron Roar.

Elementalist: Rift Bolt, Element Switch, Fireball, Glacial Spike, Living Bomb, Frozen Orb, Prism Beam, Meteor, Frost Wall, Radiant Pillar, Blizzard, Trinity Storm, Rune Anchor.

Shadowblade: Veil Flicker, Seam Rend, Backstab Mark, Death Mark, Veil Burst, Chain Cull, Shadow Pin, Night Aperture, Wraith Form, Mirror Cut.

Ranger: Rift Arrow, Tactical Roll, Pinning Shot, Marked Detonate, Bone Trap, Sweep Volley, Spirit Bow, Wireline Trap, Quiver Pulse.

Ravager: Brutal Swing, Blood Pact, Fury Tackle, Bloodlust Strike, Reckless Swing, Bloodied Roar, Death Wish, Berserk Mode, Wound Echo.

Ronin: Sheath Walk, Drawn Edge, Quickdraw Slash, Soken-giri, Iaido Stance, Wind Step, Counter Draw, Sakura Veil, Void Cleave, Mugen no Kiri, Wind Read.

Gunslinger: Dual Fire, Hip Shot, Rift Dash, Cursor Storm, Deadshot, Fan the Hammer, Suppression Fire, Rift Grenade, Ricochet, Reload Dance, Full Metal Storm, Empty Mag Burst, Exposed Line.

Brawler: Jab, Weave, Flying Knee, Combo Chain, Guard Break, Counter Blow, Aerial Rave, Seismic Stomp, Pivot Hook, Overdrive, Pulverize, Off-Balance, Glass Strike, Pin Strike.

Summoner: Command Strike, Soul Dart, Raise Skeleton, Summon Golem, Command Beacon, Corpse Explosion, Death Nova, Blood for Power, Bone Shield, Mass Sacrifice, Dark Pact, Lich Form, Army of the Dead, Beacon Pull.

Hexer: Hex Bolt, Curse Grasp, Curse Step, Corruption, Pandemic, Hexblast, Haunt, Hex Overload, Blight Sigil, Hex Cascade, Curse Bargain.

## 10. NOTES FOR RECONCILIATION

1. Resolve MASTER #56 versus current HP execute text before final lock.
2. Decide whether Shadowblade Scar sources are only RMB+Dash, as current status suggests.
3. Give Ronin Counter Draw and Sakura Veil explicit build-axis homes.
4. Decide whether Ranger should have exactly one skill movement beyond base roll.
5. Decide if Ravager can apply armor break, or if all armor fracture belongs to Warblade.
6. Decide Brawler consolidation before any PixelLab skill production.
7. Confirm Summoner visual budget: minion cap, corpse field cap, command line priority.
8. Confirm Hexer spread automation limits so Pandemic/Whisper/Mass Hex do not erase decisions.

## EVIDENCE TABLE

| Source | Claim supported | Confidence |
|---|---|---|
| TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md | Canonical skill names, chains, build axes, R4 extras. | High |
| MASTER_KARAR_BELGESI.md | HP execute ban, movement limits, state ownership, counter archetypes. | High |
| TASARIM/FAZLAR/FAZ3_SECONDARY_CLASS.md | Cross-class passive direction and export pressure. | Medium |
| COMBAT_ROSTER.md | Enemy pressure types that test roles and combo value. | Medium |
| CURRENT_STATUS.md | Active R4 locks and pending Shadowblade/Hexer concerns. | High |
| Prior Codex visual feedback | PixelLab feasibility and visual redundancy risk. | Medium |

## ASSUMPTIONS AND GAPS

- Damage numbers, cooldowns, and costs were not balanced.
- Build axis count is based on explicit Build Eksenleri text only.
- Cross-class score favors documented Faz3 synergies and exportable pools.
- PixelLab score judges 4-8 frame 128px readability, not concept-sheet quality.
- Some R4 extras are upgrades/passives; they were still audited because task requested coverage.

## REVIEWER CHECKLIST

| Check | Status |
|---|---|
| 10 classes covered | PASS |
| 3 basics per class covered | PASS |
| 12 numbered skills per class covered | PASS |
| V burst per class covered | PASS |
| R4 extras covered | PASS |
| Helper density table included | PASS |
| Role distribution included | PASS |
| Redundancy clusters included | PASS |
| Cross-class collision map included | PASS |
| Build axis orphans included | PASS |
| PixelLab feasibility summary included | PASS |
| Critical findings included | PASS |
| ASCII-only intended | PASS |
