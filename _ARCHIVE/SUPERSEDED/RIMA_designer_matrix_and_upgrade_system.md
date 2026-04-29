# RIMA — Designer Matrix + Upgrade Tier System

## DELIVERABLE 1: DESIGNER MATRIX

| Class | Identity Tag | Resource Type | Resource Risk | Run Mutability | Overlap Risk | Itemization Tags | Redesign Priority |
|---|---|---|---|---|---|---|---|
| Warblade | execute predator | builder/spender gauge | Med | Med | **Ravager** - both are bruiser melee with armor break, CC, and execution pressure; Warblade is cleaner control, Ravager is self-risk berserk | #execute #armor-break #hard-cc #counter-hit #pull | **Low** - identity is clear and functional, but its build axes blur together more than most classes |
| Elementalist | rhythm caster | regen gauge + state stacks | Med | High | **Hexer** - both can drift into planned sequencing and state-management rotations, though Elementalist is burst rhythm and Hexer is ramp payoff | #element-swap #freeze #ignite #spell-chain #blink-caster | **Med** - strong fantasy, but easiest class to become rotation-locked in a roguelite |
| Shadowblade | stealth detonator | regen gauge + combo stacks | Med | Med | **Ronin** - both are agile melee burst classes with reposition tools and reactive defense flavor; Shadowblade needs harder stealth/debuff separation | #stealth #backstab #bleed-burst #combo-finisher #reset-chain | **High** - strongest overlap issue in roster; needs clearer distinction from Ronin in play feel |
| Ranger | kite sniper | positional gauge | High | Med | **Gunslinger** - both are mobile ranged weapon users, but Ranger is spacing/control and Gunslinger is heat-pressure aggression | #range-threshold #root #trap-control #aimed-shot #disengage | **High** - Focus collapse at close range is too punishing for solo roguelite room pressure |
| Ronin | motion duelist | movement gauge | High | High | **Shadowblade** - both occupy agile melee execution space; Ronin must lean harder into momentum, sheathe timing, and line-cuts instead of evasive trickery | #quickdraw #movement-fuel #line-cut #iaido #dash-chain | **High** - concept is exciting, but counter/deflect tools muddy class boundary with Shadowblade |
| Gunslinger | heat skirmisher | reverse gauge / overheat cycle | High | High | **Ranger** - both are ranged mobility archetypes, but Gunslinger is close-pressure tempo and heat gambling | #overheat #bullet-storm #slide-shot #ricochet #point-blank | **None** - one of the cleanest roguelite-ready kits with strong mutability and distinct resource identity |
| Ravager | berserk martyr | damage-taken builder gauge | High | High | **Warblade** - both are melee powerhouses with control and armor pressure, but Ravager is defined by low-HP risk and fury spikes | #missing-hp #self-risk #berserk #lifesteal-spike #damage-conversion | **Low** - identity is strong, though build axes can blur because Fury engine and glass-cannon fantasy overlap naturally |
| Brawler | rhythm striker | hit-count charge stacks | Med | Med | **Shadowblade** - both use a 0-5 buildup structure, though Brawler spends on timing thresholds instead of finisher choice | #perfect-weave #charge-threshold #counter-rhythm #launch-juggle #combo-rush | **Med** - works in practice, but needs a forked 5-Charge payoff to avoid structural similarity with combo-point classes |
| Summoner | sacrifice commander | auto-charge cooldown resource | Med | High | **Hexer** - both are setup-to-payoff classes with delayed release moments, though Summoner is body economy and Hexer is curse phase economy | #minion-sacrifice #corpse-chain #command-target #charge-engine #lich-form | **None** - one of the most distinct classes in the roster with excellent run mutation hooks |
| Hexer | curse escalator | per-target stacks / phase ladder | Med | High | **Elementalist** - both can become sequence-solvers, but Hexer is slower pressure-ramp and breakpoint detonation | #hex-stacks #phase-breakpoint #dot-ramp #spread-curse #hexblast | **Med** - very strong identity, but phase clarity can become rigid rotation unless items actively distort breakpoint logic |

---

## DELIVERABLE 2: UNIVERSAL UPGRADE VOCABULARY + TIER SYSTEM

| Category | T1 Common (example) | T2 Uncommon (example) | T3 Rare (example) | T4 Legendary (example) |
|---|---|---|---|---|
| Damage | **Sharpened Edge** - Skill deals +12% more damage | **Predator's Window** - Skill deals +20% damage to slowed, stunned, rooted, or marked targets | **Second Wound** - Every 4th hit from this skill repeats at 45% power after 0.25s | **Identity Fracture** - Skill splits into two damage instances with separate on-hit checks and scaling rules |
| Cooldown | **Quick Recovery** - Skill cooldown -8% | **Tempo Refund** - Hitting 3+ enemies or critting 1 elite refunds 15% of remaining cooldown | **Rolling Engine** - Skill stores 1 extra charge and recharges while another charge is held | **Loop Breaker** - First cast starts cooldown; follow-up recast within 2s is free and alters shape/trajectory |
| Area | **Wider Reach** - Radius/width/length +15% | **Lingering Edge** - Skill leaves a 2s residual zone at 35% effect | **Area Echo** - Skill repeats in a smaller secondary area near the far edge or final target | **Territory Rewrite** - Skill becomes a persistent field/line/object that re-triggers itself on intervals |
| Multi-hit | **Extra Impact** - Skill gains +1 hit/tick/projectile | **Split Pattern** - Final hit or projectile forks into 2 smaller follow-ups | **Cascade Trigger** - If 2+ hits land on same target, create a localized extra strike/ricochet | **Endless Sequence** - Skill keeps chaining, orbiting, or re-firing while valid targets remain in range |
| Resource | **Efficient Casting** - Skill cost -10% or generation +10% | **Overflow Capture** - Excess generation above cap is stored as a short-lived overcap buffer | **Dual Economy** - Spending resource grants a secondary bonus: shield, haste, mark, or refund shard | **Rule Violation** - Skill uses a different economy entirely: HP, cooldown charge, overcap, or enemy count |
| Survivability | **Hardened Skin** - Gain 8% damage reduction for 1.5s after cast | **Blood Return** - Skill heals 2% max HP on elite hit or 0.4% per target hit | **Emergency Layer** - While below 35% HP, this skill grants barrier or brief unstoppable on cast | **Death Clause** - Skill gains a cheat-death interaction, invulnerability beat, or damage-to-barrier conversion window |
| Utility | **Snaring Touch** - Skill applies brief slow / mini-knock / soft pull | **Execution Setup** - Skill inflicts expose: target takes +12% from your next different skill | **Spatial Theft** - Skill steals speed, pulls enemies inward, or pins them to terrain/object | **Control Mutation** - Skill changes enemy behavior class: fear, orbit, forced face, delayed detonation, target fixation |

### Category Rules

| Category | Stack Limit | Synergy Flag | Class Exclusivity Note |
|---|---|---|---|
| Damage | 8 soft stacks, then diminished by 35% per extra pick | Cooldown, Multi-hit, Utility | Never fully lock out; all classes can use it |
| Cooldown | 6 soft stacks, charge-based versions capped at 3 | Damage, Resource, Utility | **Hexer** should rarely receive raw cooldown compression on `Hexblast`-type effects without breakpoint tradeoff |
| Area | 6 soft stacks, shape-conversion upgrades ignore cap | Damage, Multi-hit, Utility | **Brawler** and **Shadowblade** should get fewer generic radius boosts and more directional/arc variants instead |
| Multi-hit | 5 hard stacks per skill family | Damage, Resource, Area | **Warblade** should not receive excessive projectile-style multi-hit chains; keep it slash/impact themed |
| Resource | 7 soft stacks, only 1 Rule Violation active per build branch | Cooldown, Survivability, Damage | **Ranger** should never get upgrades that erase spacing identity completely; **Ravager** should not get pure passive Fury with zero risk |
| Survivability | 5 hard stacks on universal defensive layers | Resource, Utility, Cooldown | **Gunslinger** and **Shadowblade** should avoid heavy passive tank stats; prefer evasive/tempo survival |
| Utility | 6 soft stacks, 2 control mutations max | Area, Damage, Multi-hit | **Summoner** should not get too many direct forced-movement tools on personal casts; keep control routed through minions/corpses where possible |

---

## UNIVERSAL + CLASS-EXCLUSIVE POOL RULES

| Pool Type | Rule | Scaling Logic | Infinite Behavior |
|---|---|---|---|
| Universal Pool | Can appear for all classes if it does not erase class identity | Broad verbs: damage shape, cast tempo, area persistence, chain count, conditional refunds | Continues through soft caps, branching conversions, and cross-category recombination |
| Class-Exclusive Pool | Only appears for matching class and reinforces fantasy-specific verbs | Resource mutations, signature status interactions, exclusive state rewrites, cap-breakers | Adds new thresholds, alternate spenders, cap extensions, or state inversions instead of flat numbers |
| Hybrid Pool | Universal shell with class-specific rider | Example: Area upgrade becomes `blood zone` for Ravager, `curse field` for Hexer, `smoke trail` for Gunslinger | Keeps late-run drafting meaningful without making every build converge |
| Anti-Completion Rule | Any category can roll a conversion variant after soft cap | Example: extra Area becomes persistence, extra Cooldown becomes charge storage, extra Resource becomes overcap buffer | Prevents dead picks after stat saturation |
| Cap-Break Rule | Legendary and certain Rare upgrades may shift caps instead of adding value | Example: max stacks 10→14, charge 5→7, overheat 100→130, summon cap 3→5 | Creates endless scaling lanes without raw % inflation only |

---

## CLASS-EXCLUSIVE VOCABULARY TAGS

| Class | Exclusive Vocabulary |
|---|---|
| Warblade | execute threshold, armor break, pull-collapse, counter-crush, rage burst |
| Elementalist | element swap, ignite/freeze conversion, spell rhythm, dual-state chaining, mana overdrive |
| Shadowblade | stealth re-entry, rear-angle bonus, bleed detonation, combo finisher, shadow reset |
| Ranger | range threshold, tether, trap layering, precision hold, disengage window |
| Ronin | sheathe release, motion-fed tension, line cut, afterimage slash, stance compression |
| Gunslinger | overheat, ricochet, slide-fire, bullet fan, point-blank trigger |
| Ravager | missing-HP scaling, fury spike, self-risk reward, berserk reset, lifedrain conversion |
| Brawler | perfect weave, charge threshold, juggle route, momentum carry, counter rhythm |
| Summoner | corpse economy, sacrifice payout, command priority, minion cap break, lich state |
| Hexer | breakpoint stacks, curse spread, overload trigger, delayed blast, phase skip |

---

## 5 EXAMPLE INFINITE SCALING UPGRADE CHAINS

| Chain | T1 Common | T2 Uncommon | T3 Rare | T4 Legendary |
|---|---|---|---|---|
| Chain 1: Single Hit → Boss Killer | **Damage:** +12% skill damage | **Utility:** next different skill deals +12% more to target hit by this skill | **Cooldown:** elite hit refunds 15% remaining cooldown | **Damage:** skill splits into two separate execution instances, each can crit and trigger on-hit effects |
| Chain 2: Small AoE → Room Engine | **Area:** +15% radius | **Multi-hit:** +1 extra pulse/tick | **Area:** secondary echo zone appears at edge of impact | **Area:** skill becomes persistent territory that re-triggers itself periodically |
| Chain 3: Expensive Cast → Self-Fueling Loop | **Resource:** cost -10% | **Cooldown:** stores 1 extra charge | **Resource:** excess generation becomes temporary overcap buffer | **Resource:** skill changes economy, spending overcap/HP/enemy count instead of normal resource |
| Chain 4: Safe Utility → Control Monster | **Utility:** brief slow on hit | **Area:** lingering zone remains after cast | **Multi-hit:** repeated hits create local extra strike | **Utility:** enemies hit are behavior-mutated: feared, orbiting, facing locked, or delayed-exploding |
| Chain 5: Fast Spam → Endless Sequencer | **Cooldown:** -8% cooldown | **Multi-hit:** final projectile/hit forks into 2 follow-ups | **Damage:** every 4th hit repeats at 45% power | **Multi-hit:** skill continues chaining/firing/orbiting while valid targets remain |

---

## CATEGORY-SPECIFIC INFINITE BRANCHES

| Category | Infinite Extension 1 | Infinite Extension 2 | Infinite Extension 3 | Infinite Extension 4 |
|---|---|---|---|---|
| Damage | conditional damage windows | split-hit logic | execute threshold scaling | damage-type conversion riders |
| Cooldown | stored charges | partial refunds | recast windows | cooldown starts on first cast, not final cast |
| Area | persistent ground effects | edge echoes | shape conversion line/cone/ring | field duration instead of radius only |
| Multi-hit | fork count | chain distance | repeat cadence | orbiting / returning hit logic |
| Resource | overcap storage | alternate spend rule | cap extension | spend-to-defense or spend-to-speed conversion |
| Survivability | cast barrier | low-HP clause | dodge/ghost frames | damage-taken-to-resource conversion |
| Utility | slow/pull/pin strength | target fixation | behavior rewrite | terrain interaction |

---

## SOFT-CAP CONVERSION RULES

| After Soft Cap In... | Future Picks Convert To... | Why It Stays Meaningful |
|---|---|---|
| Damage | conditional damage or split-hit chance | avoids dead flat-stat drafting |
| Cooldown | charge storage or refund clauses | prevents perma-spam from pure reduction only |
| Area | persistence, edge echoes, moving zones | keeps large skills evolving functionally |
| Multi-hit | chain behavior, return hits, fork logic | changes pattern instead of just cluttering screen |
| Resource | overcap, alternate spend, cap break | preserves class identity while opening new loops |
| Survivability | reactive defense windows, barriers, cheat-death clauses | avoids immortal stat piles |
| Utility | stronger behavior control or combo setup | upgrades continue changing routing decisions |

---

## CLASS-EXCLUSIVE T4 LEGENDARY EXAMPLES

| Class | T4 Legendary Example |
|---|---|
| Warblade | **Execution Law** - any target affected by armor break and hard CC is treated as 10% lower HP for execute effects |
| Elementalist | **Double State** - Fire and Frost states coexist; every 3rd cast triggers both state riders |
| Shadowblade | **Invisible Loop** - every finisher grants 0.75s stealth and teleports you to rear-angle if path exists |
| Ranger | **Predator Radius** - the 4m+ Focus zone moves with your last marked target, letting you fight around a shifting safe ring |
| Ronin | **Unsheathed Horizon** - movement paths remain as delayed cut-lines that trigger quickdraw slashes after 0.4s |
| Gunslinger | **Controlled Meltdown** - Overheat no longer locks you out; instead each cast during overheat burns HP and doubles muzzle AoE |
| Ravager | **Death Is Fuel** - damage taken below 30% HP no longer kills instantly once per room; overflow converts to Fury and lifesteal window |
| Brawler | **Fifth Beat Split** - reaching 5 Charge lets you choose: empower next skill, store an Overdrive shard, or auto-counter next hit |
| Summoner | **Funeral Economy** - every minion death creates a corpse node that can be commanded, exploded, shielded, or resurrected once |
| Hexer | **Broken Threshold** - Hexblast triggers at 7 stacks; targets that would reach 10 instead detonate twice with reduced individual damage |
