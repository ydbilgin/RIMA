# SKILL AUDIT IDENTITY CODEX 2026-04-30

Independent identity-fit audit. Claude reconciled decision files were not read.

Inputs used:
- TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md
- TASARIM/MASTER_KARAR_BELGESI.md
- TASARIM/COMBAT_ROSTER.md
- TASARIM/FAZLAR/FAZ3_SECONDARY_CLASS.md
- CURRENT_STATUS.md
- STAGING/SKILL_AUDIT_CODEX_2026-04-30.md

Role codes: DMG, CC, MOB, DEF, SET, RES, BST.
Counts are for 12 numbered skills only, not LMB/RMB/dash/V/R4 extras.

## 1. EXECUTIVE SUMMARY

### Per-class verdict

| Class | Identity | Current role shape | Verdict | Primary action |
|---|---|---|---|---|
| Warblade | engage, break, execute | DMG/BST 5, CC 2, MOB 2, DEF 2, SET 1 | FIT-TIGHTEN | Remove duplicate dash/ground entries; convert Death Blow to state gate. |
| Elementalist | switch, shape, detonate | DMG/BST 5, CC 4, MOB 1, RES 1, SET 1 | FIT | Keep; enforce spell shape separation. |
| Shadowblade | phase, scar, collapse | DMG/BST 5, MOB 2, DEF 2, SET 2, CC 1 | OVER-MOB/SET | Collapse Scar ownership; reduce generic phase/mark tools. |
| Ranger | distance, mark, trap, detonate | DMG/BST 4, MOB 4, CC 2, SET 2 | OVER-MOB | Keep one skill movement beyond base roll; merge mass marks. |
| Ravager | suffer, trade, frenzy | DMG/BST 5, MOB 3, CC 2, DEF 2 | FIT-TIGHTEN | Merge leap/undying pairs; avoid Warblade armor ownership. |
| Ronin | wait, draw, punish | DMG/BST 5, MOB 3, DEF 3, SET 1 | OVER-MOB, UNDER-SET | Move power from mobility to stance/timing/Opened state. |
| Gunslinger | slide, shoot, reload | DMG/BST 6, CC 3, MOB 1, RES 1, SET 1 | FIT-TIGHTEN | Preserve heat/reload as identity, not just gun damage. |
| Brawler | weave, combo, launch | DMG/BST 6, CC 4, DEF 1, SET 1 | OVER-DMG-INTERNAL | Merge duplicate punches/spins/slams; keep weave/juggle/state payoffs. |
| Summoner | command, sacrifice, raise | DMG/BST 4, SET 3, RES 3, DEF 2 | FIT-WITH-CAP | Utility is identity; cap minion/readability clutter. |
| Hexer | stack, spread, blast | SET 5, DMG/BST 3, CC 2, DEF 2 | OVER-SET | Too many stack appliers; protect patient choice from autopilot. |

### Top 5 identity-fit problems

| Priority | Problem | Why it matters |
|---:|---|---|
| 1 | Ranger has four movement skills. | Distance class should win by zone planning, mark timing, and trap placement, not repeated escape buttons. |
| 2 | Hexer has too many stack appliers. | WoW Affliction-style curse identity is valid, but too many appliers turn patience into autopilot upkeep. |
| 3 | Shadowblade has too many phase/mark helpers before Scar rules are strict. | Phase is identity only if it creates/collapses Scars; otherwise it becomes generic mobility. |
| 4 | Ronin mobility density conflicts with iaido stillness. | Ronin should feel fast through timing release, not through three separate dash buttons. |
| 5 | Brawler redundancy is inside damage actions, not helper count. | The class needs different boxing jobs: jab chain, launch, guard break, pivot finisher, not many "hit harder" variants. |

## 2. PER-CLASS IDENTITY ANALYSIS

### 2.1 Warblade

**Identity statement**

Warblade is for forcing entry, breaking enemy stability/armor, then executing a controlled punish.

**Required role distribution**

Ideal for 12 numbered skills: DMG/BST 4-5, CC 2-3, DEF 2, MOB 1-2, SET 1-2, RES 0. This is close to a Diablo Barbarian or Hades melee weapon pattern: the fantasy is direct damage, but the kit must own engage and break states so burst is earned.

**Current actual distribution**

| Role | Count | Percent |
|---|---:|---:|
| DMG/BST | 5 | 42% |
| CC | 2 | 17% |
| MOB | 2 | 17% |
| DEF | 2 | 17% |
| SET | 1 | 8% |
| RES | 0 | 0% |

**Verdict**

FIT-TIGHTEN. The macro distribution fits. Problems are duplicate entry shapes and one non-canonical execute gate, not identity failure.

**Concrete actions**

| Action | Skill citation | Reason |
|---|---|---|
| MERGE | Warblade.Blade Rush (#9) into Warblade.Iron Charge (#1) | Two line-entry skills compete; Warblade needs one signature engage plus upgrades. |
| MERGE | Warblade.Quake Slam (R4) into Warblade.Earthsplitter (#6) | Ground crack/knockup should not split into multiple near-identical picks. |
| REDESIGN | Warblade.Death Blow (#12) | Convert HP<30 execute to Broken/Sundered gate per MASTER #56. |
| KEEP | Warblade.Sunder Mark (#5), Warblade.Iron Counter (#8) | These are the identity anchors: break ownership and absorb/break counter. |

**Cross-class reference**

Warblade should own Sundered/Broken and absorb-counter. Ravager can trade HP, Brawler can crack/shatter bodies, but neither should become the armor-break owner.

### 2.2 Elementalist

**Identity statement**

Elementalist is for switching elements, shaping battlefield geometry, and detonating reactions at the right time.

**Required role distribution**

Ideal: DMG/BST 4-5, CC 3-4, SET 1-2, RES 1, MOB 1, DEF 0. This is closest to Path of Exile Witch/Elementalist and Magicka-style element chaining: spell identity comes from reaction routing and shape diversity, not raw projectile count.

**Current actual distribution**

| Role | Count | Percent |
|---|---:|---:|
| DMG/BST | 5 | 42% |
| CC | 4 | 33% |
| MOB | 1 | 8% |
| RES | 1 | 8% |
| SET | 1 | 8% |
| DEF | 0 | 0% |

**Verdict**

FIT. The current spread strongly matches switch/shape/detonate.

**Concrete actions**

| Action | Skill citation | Reason |
|---|---|---|
| KEEP | Elementalist.Fireball (#1), Elementalist.Glacial Spike (#2), Elementalist.Living Bomb (#3) | Fire/Frost/delay triangle is clear. |
| KEEP | Elementalist.Frozen Orb (#5), Elementalist.Frost Wall (#8), Elementalist.Blizzard (#12) | Control density is on-fantasy because battlefield shaping is core. |
| TIGHTEN | Elementalist.Prism Beam (#6) vs Elementalist.Solar Flare (#9) | Beam must be line channel; Solar Flare must remain cone/radiant burst. |
| TIGHTEN | Elementalist.Element Charge (#11) | If it stays Fire-only, it should not flatten the three-element identity. |

**Cross-class reference**

Elementalist can mirror Ranger's trap/detonate rhythm, but it should do so through elemental shapes. Ranger owns physical trap/mark networks.

### 2.3 Shadowblade

**Identity statement**

Shadowblade is for crossing through a target, leaving a spatial wound, and collapsing that wound from the correct angle.

**Required role distribution**

Ideal: DMG/BST 4, MOB 2, SET 3, CC 1, DEF 1-2. Comparable to a rogue only at surface level; the stronger analog is a positional combo class where mobility is a state application tool. Phase is valid only when it creates Scar/collapse decisions.

**Current actual distribution**

| Role | Count | Percent |
|---|---:|---:|
| DMG/BST | 5 | 42% |
| MOB | 2 | 17% |
| DEF | 2 | 17% |
| SET | 2 | 17% |
| CC | 1 | 8% |
| RES | 0 | 0% |

**Verdict**

OVER-MOB/SET. The count is not wildly wrong, but too many skills are generic phase/mark helpers instead of explicit Scar creation, Scar reposition, or Scar collapse.

**Concrete actions**

| Action | Skill citation | Reason |
|---|---|---|
| TIGHTEN | Shadowblade.Phase Step (#2) | Must be non-Scar reposition or it overlaps Veil Flicker/RMB. |
| KEEP | Shadowblade.Veil Flicker (RMB), Shadowblade.Seam Rend (dash) | Best candidates for canonical Scar application. |
| REDESIGN | Shadowblade.Severance (#7) | Replace low-HP execute with Scar collapse finisher. |
| TIGHTEN | Shadowblade.Smoke Veil (#8) | Solo payoff must be Scar/angle related, not generic stealth utility. |
| KEEP | Shadowblade.Night Aperture (#12), Shadowblade.Mirror Cut (R4) | These finally make Scar geometry special. |

**Cross-class reference**

Ronin is fast timing; Shadowblade is spatial discontinuity. If both read as "teleport cut," Shadowblade should keep Scar/collapse while Ronin keeps draw/Open timing.

### 2.4 Ranger

**Identity statement**

Ranger is for staying out of reach, defining a kill zone with traps/marks, then detonating that setup.

**Required role distribution**

Ideal: DMG/BST 4, CC 3, SET 3, MOB 1-2, DEF 0, RES 0. A useful analog is a Hades bow/trap hunter hybrid: distance is maintained by planning and root zones, not by stacking escape skills.

**Current actual distribution**

| Role | Count | Percent |
|---|---:|---:|
| DMG/BST | 4 | 33% |
| MOB | 4 | 33% |
| CC | 2 | 17% |
| SET | 2 | 17% |
| DEF | 0 | 0% |
| RES | 0 | 0% |

**Verdict**

OVER-MOB, UNDER-SET/CC. The fantasy says mark/trap/detonate; current distribution spends too many slots on movement.

**Concrete actions**

| Action | Skill citation | Reason |
|---|---|---|
| MERGE | Ranger.Skirmish Shot (#7) into Tactical Roll/RMB branch | Moving shot is base kit behavior, not a full skill slot. |
| MERGE | Ranger.Rift Step (#11) into Ranger.Hunter's Step (#4) | One skill movement beyond base roll is enough under MASTER #58. |
| MERGE | Ranger.Multi-Mark (#9) into Ranger.Predator's Mark (#8) | Two mass-mark helpers erase choice. |
| KEEP | Ranger.Bone Trap (#5), Ranger.Wireline Trap (R4) | Trap identity is under-served, not over-served. |
| REDESIGN | Ranger.Final Strike (#10) | Gate by Marked+Trapped or long-distance confirm, not low HP. |

**Cross-class reference**

Gunslinger owns run-and-gun movement. Ranger should avoid mirroring that and instead own "you crossed my line, so the room punishes you."

### 2.5 Ravager

**Identity statement**

Ravager is for turning self-endangerment into escalating violence: suffer, trade HP, then chain frenzy.

**Required role distribution**

Ideal: DMG/BST 5, DEF 2, RES/HP trade 1-2, CC 2, MOB 1-2, SET 0-1. Diablo Berserker/Path of Exile low-life archetypes are good references: danger is not a drawback bolted on; it is the engine.

**Current actual distribution**

| Role | Count | Percent |
|---|---:|---:|
| DMG/BST | 5 | 42% |
| MOB | 3 | 25% |
| CC | 2 | 17% |
| DEF | 2 | 17% |
| SET | 0 | 0% |
| RES | 0 | 0% |

**Verdict**

FIT-TIGHTEN. HP trade and low-life logic are on-identity. Mobility is a little high because two leap skills compete.

**Concrete actions**

| Action | Skill citation | Reason |
|---|---|---|
| MERGE | Ravager.Frenzied Leap (#3) with Ravager.Blood-Drunk Leap (#10) | Two leap attacks dilute suffer/trade/frenzy. |
| MERGE | Ravager.Undying Tenacity (#8) with Ravager.Death Wish (#12) | Two "cannot die now" skills compete. |
| REDESIGN | Ravager.Shatter Armor (#11) | Armor break collides with Warblade state ownership. Make it self-wound or frenzy exploit instead. |
| KEEP | Ravager.Blood Pact (RMB), Ravager.Reckless Swing (#4), Ravager.Death Wish (#12) | These best express risk-for-power. |

**Cross-class reference**

Ravager can use CC as stagger from brutality, but Warblade owns break/armor. Brawler owns body state (Cracked/Shattered). Ravager owns pain conversion.

### 2.6 Ronin

**Identity statement**

Ronin is for building tension through restraint and movement discipline, then releasing a precise draw punish.

**Required role distribution**

Ideal: DMG/BST 4-5, SET/RES 2-3, DEF/timing 2-3, MOB 1-2, CC 0-1. Slay the Spire Watcher is a useful analog: state preparation is the identity. Ronin should be about sheathe/open/release, not generic speed.

**Current actual distribution**

| Role | Count | Percent |
|---|---:|---:|
| DMG/BST | 5 | 42% |
| MOB | 3 | 25% |
| DEF | 3 | 25% |
| SET | 1 | 8% |
| CC | 0 | 0% |
| RES | 0 | 0% |

**Verdict**

OVER-MOB, UNDER-SET. Ronin's problem is not lack of damage; it is that mobility has more slot weight than stillness/timing/Opened-state preparation.

**Concrete actions**

| Action | Skill citation | Reason |
|---|---|---|
| TIGHTEN | Ronin.Haste Dash (#2), Ronin.Wind Step (#5), Ronin.Phantom Step (#7) | Three movement/deception tools push Ronin toward Shadowblade/Gunslinger territory. |
| MERGE | Ronin.Stillness (R4) into Ronin.Iaido Stance (#4) | Stillness is core identity, so it should strengthen stance rather than create another button. |
| REDESIGN | Ronin.Flash Draw (#10) | Remove HP execute clause; gate on Opened or perfect Tension timing. |
| KEEP | Ronin.Counter Draw (#6), Ronin.Sakura Veil (#8), Ronin.Void Cleave (#12) | These establish the pre-draw counter and release identity. |

**Cross-class reference**

Shadowblade phases through space; Ronin cuts after a read. Brawler reacts to whiffs with body movement; Ronin reacts from sheath timing.

### 2.7 Gunslinger

**Identity statement**

Gunslinger is for moving while firing, creating Heat pressure, and converting reload/position timing into burst.

**Required role distribution**

Ideal: DMG/BST 5-6, CC 2-3, MOB 1-2, RES/reload 1-2, SET 0-1. A good analog is run-and-gun action design, not a stationary shooter: movement and resource are part of firing, not separate support.

**Current actual distribution**

| Role | Count | Percent |
|---|---:|---:|
| DMG/BST | 6 | 50% |
| CC | 3 | 25% |
| MOB | 1 | 8% |
| RES | 1 | 8% |
| SET | 1 | 8% |
| DEF | 0 | 0% |

**Verdict**

FIT-TIGHTEN. The distribution fits. Risk is that reload/Heat becomes too minor compared with plain gun damage.

**Concrete actions**

| Action | Skill citation | Reason |
|---|---|---|
| KEEP | Gunslinger.Rift Dash (#1), Gunslinger.Hip Shot (RMB) | Movement is identity and already compact. |
| KEEP | Gunslinger.Reload Dance (#10) | Reload must stay visible enough to distinguish from Ranger. |
| MERGE | Gunslinger.Reload Roll (R4) into Gunslinger.Reload Dance (#10) | Reload movement should not split across multiple picks. |
| REDESIGN | Gunslinger.Point Blank Execute (#12) | Reframe as Heat-zero, Empty Mag, or Exposed Line payoff. |
| TIGHTEN | Gunslinger.Smoke Grenade (#5) | It should blind/suppress, not become Shadowblade stealth. |

**Cross-class reference**

Ranger is the ranged planner. Gunslinger is the ranged mover. Elementalist uses shaped magic zones; Gunslinger uses muzzle/Heat/reload rhythm.

### 2.8 Brawler

**Identity statement**

Brawler is for staying in the pocket, weaving attacks, building combo/Charge, launching bodies, and cashing out with a clean hit.

**Required role distribution**

Ideal: DMG/BST 4-5, CC/launch 3-4, DEF/weave 1-2, SET/body state 2, MOB 0-1. A fighting game analog fits better than ARPG fighter design: every strike should have a role in the combo grammar.

**Current actual distribution**

| Role | Count | Percent |
|---|---:|---:|
| DMG/BST | 6 | 50% |
| CC | 4 | 33% |
| DEF | 1 | 8% |
| SET | 1 | 8% |
| MOB | 0 | 0% |
| RES | 0 | 0% |

**Verdict**

OVER-DMG-INTERNAL. Helper count is not the issue. Damage actions overlap inside punch/kick/spin/slam families.

**Concrete actions**

| Action | Skill citation | Reason |
|---|---|---|
| MERGE | Brawler.Shockwave Slam (#2) into Brawler.Seismic Stomp (#10) | Two ground shock skills are unnecessary. |
| MERGE | Brawler.Tornado Kick (#3) into Brawler.Cyclone Drive (#9) | One spin-family action is enough. |
| REDESIGN | Brawler.Unstoppable Force (#12) | Too close to Overdrive; should route Charge/banking rather than duplicate V. |
| KEEP | Brawler.Weave (RMB), Brawler.Counter Blow (#7) | Whiff/body counter separates Brawler from Warblade/Ronin. |
| KEEP | Brawler.Aerial Rave (#8), Brawler.Pivot Hook (#11), Brawler.Pulverize (R4) | Launch, cashout, and Cracked/Shattered payoff are the identity. |

**Cross-class reference**

Brawler should not steal Warblade Sundered ownership or Ronin pre-draw counter. Its state lane is Cracked -> Shattered, with whiff and launch.

### 2.9 Summoner

**Identity statement**

Summoner is for avoiding direct hero combat, commanding bodies, sacrificing them, and converting corpses/minions into payoff.

**Required role distribution**

Ideal: SET/command 3-4, RES/minion economy 2-3, DEF/body shielding 1-2, DMG/BST/sacrifice 3-4, MOB 0-1. Diablo Necromancer is the right reference: utility is not bloat when minions, curses, corpses, and sacrifice all serve the fantasy.

**Current actual distribution**

| Role | Count | Percent |
|---|---:|---:|
| DMG/BST | 4 | 33% |
| SET | 3 | 25% |
| RES | 3 | 25% |
| DEF | 2 | 17% |
| CC | 0 | 0% |
| MOB | 0 | 0% |

**Verdict**

FIT-WITH-CAP. Eight helpers looked high in the prior audit, but most are on-identity. The real risk is visual/UI readability, not fantasy mismatch.

**Concrete actions**

| Action | Skill citation | Reason |
|---|---|---|
| KEEP | Summoner.Raise Skeleton (#1), Summoner.Summon Golem (#2) | Raise/command identity needs body variety. |
| KEEP | Summoner.Corpse Explosion (#4), Summoner.Death Nova (#5), Summoner.Mass Sacrifice (#10) | Sacrifice payoff is the class. |
| TIGHTEN | Summoner.Soul Siphon Totem (#9) | Totem must not become a generic resource pillar; tie it to minion death. |
| REDESIGN | Summoner.Soul Tax (R4) | Delayed 6s summon is hard to value unless tied to explicit command/sacrifice timing. |
| CAP | Summoner.Bone Tide (R4) | Respect current minion cap/readability before adding mass summon. |

**Cross-class reference**

Summoner and Hexer can both be "utility-heavy" without converging: Summoner manipulates bodies and corpses; Hexer manipulates stacks and phases on enemies.

### 2.10 Hexer

**Identity statement**

Hexer is for patiently building per-target stacks, spreading pressure, then choosing when to cash out at 7-10 stacks.

**Required role distribution**

Ideal: SET 3-4, DMG/BST 3, CC/debuff 2-3, DEF/reflect 1-2, MOB 0-1. WoW Affliction Warlock is the best warning: DoT/curse helpers are on-fantasy, but too many appliers create autopilot and reduce the payoff choice.

**Current actual distribution**

| Role | Count | Percent |
|---|---:|---:|
| SET | 5 | 42% |
| DMG/BST | 3 | 25% |
| CC | 2 | 17% |
| DEF | 2 | 17% |
| MOB | 0 | 0% |
| RES | 0 | 0% |

**Verdict**

OVER-SET. Stack identity is correct, but stack application is over-supplied. The class risks becoming "press every applier, then Hexblast."

**Concrete actions**

| Action | Skill citation | Reason |
|---|---|---|
| TIGHTEN | Hexer.Corruption (#1), Hexer.Agony (#2), Hexer.Haunt (#6) | These must occupy different jobs: opener, sustained DoT, pursuing pressure. |
| TIGHTEN | Hexer.Pandemic (#3), Hexer.Mass Hex (#9), Hexer.Whisper Mark (R4) | Too much spread automation erases target choice. |
| MERGE | Hexer.Cursed Mirror (#11) with Hexer.Empathy (#5) | Two reflect curses are identity-adjacent but redundant. |
| KEEP | Hexer.Hexblast (#4) | Early cashout at 7-9 preserves choice and breaks pure 10-stack linearity. |
| KEEP | Hexer.Blight Sigil (#12), Hexer.Curse Bargain (R4) | Ground zone and HP trade stack burst are distinct enough. |

**Cross-class reference**

Hexer should not become Summoner without minions. Its "utility" is enemy-state math: stack, spread, overload, blast.

## 3. CROSS-CLASS IDENTITY DIFFERENTIATION

| Pair | Convergence risk | Class A must own | Class B must own | Avoid |
|---|---|---|---|---|
| Shadowblade vs Ronin | Both can read as fast cutter. | Shadowblade owns Scar placement/collapse and phase-through geometry. | Ronin owns sheathe timing, Opened state, and precision release. | Generic teleport slash on both. |
| Ranger vs Gunslinger | Both are ranged weapon classes. | Ranger owns trap lines, marks, and kill zones. | Gunslinger owns Heat, reload, and moving fire. | Ranger becoming run-and-gun; Gunslinger becoming mark/trap planner. |
| Warblade vs Ravager | Both are heavy melee. | Warblade owns break, Sundered/Broken, absorb-counter. | Ravager owns HP trade, low-life danger, frenzy chains. | Shared armor-break language. |
| Warblade vs Brawler | Both can ground slam and control bodies. | Warblade owns weapon impact and armor state. | Brawler owns Cracked/Shattered, launch, wall/body combos. | Duplicate ground crack skills without state distinction. |
| Ronin vs Brawler | Both have counters. | Ronin owns pre-draw/perfect timing. | Brawler owns whiff/evade body movement. | Same parry window with different VFX. |
| Summoner vs Hexer | Both are utility-heavy. | Summoner owns minion bodies, corpses, sacrifice economy. | Hexer owns enemy stacks, spread, and curse thresholds. | Invisible passive automation on both. |
| Elementalist vs Ranger | Both can zone and detonate. | Elementalist owns spell shapes and element reactions. | Ranger owns traps, marks, and physical kill-zone prep. | Generic "zone then explode" without material difference. |
| Shadowblade vs Gunslinger | Both can use smoke. | Shadowblade owns stealth/Scar angle payoff. | Gunslinger owns blind/suppression smoke. | Smoke Grenade granting rogue stealth as its main identity. |

## 4. ROLE TYPE BUDGET ACROSS THE ROSTER

### Current roster-wide role budget

| Role | Count across 120 numbered skills | Percent | Roster-level read |
|---|---:|---:|---|
| DMG/BST | 48 | 40% | Healthy but internally redundant in Brawler and some melee kits. |
| CC | 20 | 17% | Good total, but uneven: Elementalist/Brawler high, Ronin/Summoner low. |
| SET | 17 | 14% | Healthy total, but over-concentrated in Hexer/Summoner/Ranger. |
| MOB | 16 | 13% | Too high in Ranger/Ronin/Ravager; MASTER #58 suggests tighter skill movement. |
| DEF | 14 | 12% | Good spread across melee and utility classes. |
| RES | 5 | 4% | Low by count, but many basics/RMBs carry resource identity outside numbered skills. |

### Roster-level conclusions

| Finding | Conclusion |
|---|---|
| Mobility is not globally excessive, but it is clustered in the wrong identities. | Ranger and Ronin should lose/merge movement slots; Gunslinger can keep movement because it is the fantasy. |
| Damage payoffs are plentiful. | The next pass should not add more generic finishers; it should sharpen state gates. |
| Setup is correctly high in Hexer/Summoner but needs anti-autopilot caps. | Keep on-fantasy utility, remove duplicate appliers. |
| CC is under-supplied for Ronin and Summoner if defined as hard CC, but not necessarily identity failure. | Ronin needs Opened/timing, Summoner needs body control, not generic stun/root. |
| Resource skills are undercounted because many resources live in basics/RMB. | Do not add resource buttons just to fix table count. |

## 5. CONCRETE FOLLOW-UPS TO PRIOR AUDIT

### Prior audit under-weighted on identity grounds

| Skill | Prior tendency | Identity correction |
|---|---|---|
| Summoner.Command Beacon (#3) | Helper-heavy risk. | Keep; mass command is central, not optional utility. |
| Summoner.Bone Shield (#8) | Helper-heavy risk. | Keep; minion-as-defense is on-fantasy. |
| Hexer.Corruption (#1) | Another stack applier. | Keep as opener; the problem is too many adjacent appliers, not Corruption itself. |
| Ranger.Wireline Trap (R4) | Possible redundancy with Bone Trap. | Keep; trap density is exactly Ranger identity, mobility density is the issue. |
| Ronin.Iaido Stance (#4) | Setup helper. | Keep/strengthen; stillness is identity. |

### Prior audit over-marked or needs reframing

| Skill | Earlier concern | Identity-fit read |
|---|---|---|
| Summoner helper count overall | Eight helpers flagged. | Accept high helper count if command/sacrifice/corpse/readability caps are enforced. |
| Hexer helper count overall | Eight helpers flagged. | Utility is on-fantasy, but stack appliers must be reduced or role-separated. |
| Brawler helper count low | Looked healthy by helper metric. | Misleading; Brawler's issue is damage-action redundancy. |
| Ranger mobility issue | Marked generically. | It is identity-critical: Ranger should win by kill-zone design, not mobility abundance. |
| Shadowblade phase issue | Marked generic redundancy. | It is acceptable only if phase produces Scar/collapse decisions. |

### Skills CT-AUDIT-01 missed or under-weighted

| Skill | Identity reason |
|---|---|
| Ronin.Stillness (R4) | Should probably be folded into Iaido Stance and treated as core identity, not extra support. |
| Ranger.Wireline Trap (R4) | Stronger identity fit than some current numbered skills; it should displace a movement slot. |
| Shadowblade.Mirror Cut (R4) | Strong identity fit because it uses active Scars; safer than another generic phase slash. |
| Gunslinger.Empty Mag Burst (R4) | Excellent reload/Heat identity; stronger than generic Point Blank Execute. |
| Brawler.Off-Balance (R4) | Important for whiff/body-counter ownership under MASTER #57. |

## 6. NOTES FOR RECONCILIATION WITH CLAUDE

1. Ranger should likely be the largest identity correction: reduce to base Tactical Roll plus one skill movement, then give the freed slots to trap/mark payoff.
2. Hexer should keep curse density but reduce duplicate stack automation. Pandemic, Mass Hex, and Whisper Mark need clear exclusivity or caps.
3. Summoner should not be flattened by generic helper-count rules. Its utilities are on-identity if command lines, minion cap, corpse cap, and sacrifice cooldown are visible.
4. Shadowblade needs a canonical Scar source list before final skill cuts. Current status says only RMB + Dash create Scars; this audit supports that as a stabilizing rule.
5. Ronin needs more power in Iaido Stance/Opened/Stillness and less in generic movement/deception.
6. Brawler should consolidate by action family before art production: one spin, one slam/line launch, one combo chain, one pivot cashout.
7. HP execute text still conflicts with MASTER #56 across Warblade, Shadowblade, Ranger, Ronin, and naming/logic around Gunslinger.
8. Warblade/Ravager/Brawler state ownership should remain strict: Warblade = Broken/Sundered, Ravager = pain/frenzy, Brawler = Cracked/Shattered.

## EVIDENCE TABLE

| Source | Claim supported | Confidence |
|---|---|---|
| TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md | Core fantasies, 12 numbered skill lists, build axes, R4 extras. | High |
| TASARIM/MASTER_KARAR_BELGESI.md | Class roster, movement Option C, counter archetype separation, execute gate ban, state ownership locks. | High |
| TASARIM/COMBAT_ROSTER.md | Enemy pressure types requiring distance, priority targeting, anti-sustain, mobility punishment. | Medium |
| TASARIM/FAZLAR/FAZ3_SECONDARY_CLASS.md | Secondary/cross-class pressure for Ranger, Gunslinger, Ronin, Ravager, Brawler. | Medium |
| CURRENT_STATUS.md | Active locks: movement, counter separation, Brawler Shattered, Summoner cap, pending Shadowblade/Hexer issues. | High |
| STAGING/SKILL_AUDIT_CODEX_2026-04-30.md | Prior role counts and helper-density context. | High |

## ASSUMPTIONS AND GAPS

- This audit judges identity fit, not numeric balance.
- Role counts use 12 numbered skills only; basics, V bursts, and R4 extras are discussed but not counted in the distribution table.
- Some canonical text includes older HP execute clauses that conflict with MASTER #56; MASTER is treated as the later authority.
- Game analogs are used as design references, not templates to copy.
- PixelLab feasibility is mentioned only where it affects identity readability.

## REVIEWER CHECKLIST

| Check | Status |
|---|---|
| All 10 classes covered | PASS |
| Core Fantasy distilled per class | PASS |
| Required role distribution per class | PASS |
| Current actual distribution per class | PASS |
| Identity-fit verdict per class | PASS |
| Concrete actions with skill citations | PASS |
| Cross-class differentiation section | PASS |
| Roster-wide role budget | PASS |
| Follow-ups to prior Codex audit | PASS |
| Claude reconciliation notes | PASS |
| Claude audit/decision files avoided | PASS |
| ASCII-only content intended | PASS |
