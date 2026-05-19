# Master Index

Scope: design backlog seed for Warblade, Elementalist, Ranger, and Shadowblade. Each class keeps 4 active slots in-run, with 12 pool entries: 4 active skills, 4 passive upgrades, and 4 cross-class Echo trigger conditions.

Canonical family tags used here: Fracture, Echo, Veil, Pierce, Bleed, Cut, Pressure, Strike, Rift. I keep the full 9-tag vocabulary because the task asks for it, while the T4 Rift Proc detection can still read the locked core subset when implementation narrows tags.

Mechanic bank anchors used: M05 Element Combo, M22 Event-Driven Combo, M23 Stamina Bank, M24 Decoupled Skill Tree, M56 Phase State, M57 Density Topology, M58 Shape-as-Verb, M59 Combo Window, M60 Parry/Riposte, M61 Dash i-Frame, M62 Status Layering, M64 Hitstop, M68 Build Synergy Detection.

## Warblade - Rage resource

Core loop: close, pin, crack armor, spend Rage on decisive impact. Rage is built by damage events and especially rewarded by controlled or CC'd targets.

### Active 1-4

| # | Name | Behavior | Resource cost | Cooldown | Damage type | Family tag | Synergy hooks | Visual/Audio signature |
|---|---|---|---|---|---|---|---|---|
| 1 | Iron Rhythm | LMB-equivalent three-hit chain that widens on beat 3 and applies Cracked to enemies hit by the final swing. | 0 Rage; generates 3 Rage per hit and +8 on beat 3 vs CC'd targets. | Basic chain timing only. | Physical | Strike, Fracture | T1 Commit-Beat Echo; Ranger roots make beat 3 instant-confirm; Shadowblade Scar collapse benefits from Cracked targets. | Heavy boot scrape, low steel pulse, orange fracture sparks on beat 3. |
| 2 | Sunder Hook | RMB main skill: short hook-slash that pulls the nearest marked enemy 1.5m, applies Sundered, and refunds Rage if it interrupts an attack. | 20 Rage, refunds 10 on interrupt. | 6s | Physical | Fracture, Pressure | Elementalist Frost Wall traps pulled enemies; Ranger Wireline/Bone Trap converts pull into kill zone; T3 Empowered Skill Echo. | Chain-grind pull, shield-crack snap, brass-orange impact ring. |
| 3 | Bladestorm Vow | F V Burst ultimate: spend full Rage to spin for 5s with CC immunity, dealing pulsing AoE and refreshing Fracture flags. | 100 Rage | V meter gate only; no normal cooldown. | Physical | Fracture, Cut | Echo T4 setup by refreshing Fracture while secondary Echo/Rift tags are active; Ranger marked packs multiply value; Elementalist burn fields add layered ticks. | War drum roll into continuous blade wind, screen-edge orange shake, short hitstop on first contact. |
| 4 | Iron Counter Step | R Mobility/Parry: short armored step with a 0.25s parry window; successful parry deals riposte damage and grants Rage. | 15 Rage; free if Rage below 15 but no riposte bonus. | 5s | Physical | Strike, Pressure | Mechanic bank M60/M61; triggers Warblade T3 Echo when drafted as secondary; Shadowblade rewards the created back-exposure. | Sword hilt clang, white-orange guard flash, bass counter thud. |

### Passive 1-4

| # | Name | Behavior | Resource cost | Cooldown | Damage type | Family tag | Synergy hooks | Visual/Audio signature |
|---|---|---|---|---|---|---|---|---|
| 1 | Red Reserve | Rage cap increases to 120 and Rage decay waits 1s longer after combat contact. | Passive run pickup. | None | Utility | Pressure | Enables more frequent Bladestorm Vow and keeps T3 Warblade echo windows available after Ranger distance play. | Resource bar gains a second darker red notch with a low heartbeat tick. |
| 2 | Crack Momentum | Hitting Sundered or Cracked enemies grants +10% move speed for 2s and +2 Rage per hit. | Passive run pickup. | None | Physical | Fracture, Strike | Ranger roots and Elementalist Frost locks keep targets in Crack Momentum uptime; Shadowblade Death Mark benefits from cracked targets. | Small orange heel sparks, layered metal clack per accelerated hit. |
| 3 | Tempered Riposte | Perfect parries apply Fracture and reduce the next Warblade active cost by 10 Rage. | Passive run pickup. | 0.8s internal cooldown | Physical | Fracture, Pressure | Directly supports M60 skill-cap path; gives T4 setups a reliable Fracture source. | Brief shield outline flash, tight anvil ping. |
| 4 | Execution Weight | Enemies below 35% health take +25% Warblade damage if they are CC'd, Sundered, or tagged by Rift. | Passive run pickup. | None | Physical | Strike, Rift | Ranger Pinning Shot, Elementalist Frost/Lightbreak, and Shadowblade Rift Scar all expose execute windows. | Target outline deepens from orange to dark red; muted crowd-hitstop thump. |

### Echo triggers 1-4

| # | Name | Behavior | Resource cost | Cooldown | Damage type | Family tag | Synergy hooks | Visual/Audio signature |
|---|---|---|---|---|---|---|---|---|
| 1 | T1 Commit: Iron Afterbeat | Trigger condition: another class lands LMB beat 3, summoning Warblade to perform a 35% cleave on the same facing line. | Echo tier condition only; no Rage cost. | 1.2s ICD | Echo Physical | Fracture, Strike | Best with Shadowblade combo finishers and Ranger Aim Shot beat confirmations. | Cyan phantom greatsword smear with orange crack flakes. |
| 2 | T2 Resonance: Crack on Contact | Trigger condition: any basic hit has 15-25% Altar-scaled chance to add a 25% Warblade shoulder bash that applies Cracked. | Echo tier condition only. | 0.8s ICD | Echo Physical | Fracture | Builds T4 Fracture flag for Elementalist/Ranger primaries without adding active buttons. | Short cyan silhouette impact, stone chip pop. |
| 3 | T3 Empowered: Sunder Shadow | Trigger condition: drafted Q/E/R/F active cast tagged Pressure or Strike calls Sunder Hook at 50% damage and grants -10% primary cooldown. | Echo tier condition only. | Skill cooldown | Echo Physical | Pressure, Fracture | Pairs with Ranger traps, Elementalist wall/orb shapes, Shadowblade pins. | Phantom chain yanks through cyan afterimage, low chain rattle. |
| 4 | T4 Rift Proc: Armor Break Bond | Trigger condition: enemy has 3 different active family tags including Fracture; next primary impact freezes 0.3s and Warblade echoes for 100% damage plus armor penetration. | Consumes T4 tag state. | T4 tag immunity 4s after proc | Echo Physical | Rift, Fracture | Uses full tag matrix; strongest when Elementalist supplies Echo and Shadowblade supplies Veil/Bleed. | Cyan portrait flash, orange armor shell bursts outward. |

## Elementalist - Mana + Element

Core loop: spend Mana on spell shapes, alternate Fire and Frost, then cash rhythm through Lightbreak/Light state. Damage identity is reaction, geometry, and readable spell forms.

### Active 1-4

| # | Name | Behavior | Resource cost | Cooldown | Damage type | Family tag | Synergy hooks | Visual/Audio signature |
|---|---|---|---|---|---|---|---|---|
| 1 | Rift Bolt Cycle | LMB-equivalent bolt chain that alternates Fire/Frost by active element; every third bolt is empowered and adds resonance. | 0 Mana; empowered hit restores 3 Mana. | Basic chain timing only. | Element | Echo, Strike | T1 Commit-Beat Echo; Warblade cracks improve bolt finishers; Ranger marks make third bolt home slightly. | Small fire/frost motes, third bolt emits gold-white ping. |
| 2 | Convergence Orb | RMB main skill: launch a slow orb that absorbs the current element, then detonates differently if the next cast is the opposite element. | 30 Mana | 7s | Element | Echo, Pressure | M05 Element Combo; Fire then Frost roots, Frost then Fire bursts; Shadowblade Scar collapse can detonate orb early. | Floating rune sphere, inhale hum, glassy reaction pop. |
| 3 | Trinity Storm | F V Burst ultimate: for 7s, Fire, Frost, and Light pulses orbit the player and each cast attempts one free Lightbreak check. | Full Convergence/V meter or 100 Mana if no V meter exists. | V meter gate only | Element | Echo, Rift | T4 setup machine: supplies Echo/Rift while Warblade or Shadowblade provides physical tags; Ranger kill zones hold targets inside. | Three-color central rune, choir swell, layered thunder ticks. |
| 4 | Blink Sigil | R Mobility: short blink that leaves a sigil; recasting a different element within 2s detonates the sigil as Fire burst or Frost snare. | 20 Mana | 5s | Element | Veil, Echo | Shadowblade phase routes through the sigil for Scar detonation; Ranger disengage lines extend safe casting distance. | Thin prism blink line, soft chime, delayed rune crack. |

### Passive 1-4

| # | Name | Behavior | Resource cost | Cooldown | Damage type | Family tag | Synergy hooks | Visual/Audio signature |
|---|---|---|---|---|---|---|---|---|
| 1 | Alternating Current | Casting Fire after Frost or Frost after Fire refunds 8 Mana and adds +1 resonance to the new element. | Passive run pickup. | None | Utility | Echo | Reinforces Convergence Orb and Lightbreak; Warblade CC gives time to alternate safely. | Mana bar flickers split red/blue with a clean tick. |
| 2 | Lightbreak Memory | First Lightbreak in each room stores one Light charge; next ultimate pulse consumes it for extra Echo damage. | Passive run pickup. | Once per room storage | Element | Echo, Rift | M99-like memory-as-resource adapted run-local; helps T4 Rift Proc without permanent power creep. | Tiny gold rune stored near resource UI, bell-over-glass sound. |
| 3 | Phase State Mastery | Fire DoT on Frosted targets becomes Steam Burst, while Frost on Burning targets becomes Brittle Lock. | Passive run pickup. | 1s per target | Element | Echo, Pressure | M56 Phase State and M62 Status Layering; Ranger traps and Warblade pulls cluster reactions. | Steam puff or ice splinter overlays matching reaction. |
| 4 | Prism Economy | Mana regen increases by +25% while no enemy is within 3m, but direct hits within 3m cost +5 Mana. | Passive run pickup. | None | Utility | Pressure | Creates caster spacing identity parallel to Ranger Focus without stealing it; Shadowblade can peel threats away. | Resource wave accelerates with blue-gold ripple when safe. |

### Echo triggers 1-4

| # | Name | Behavior | Resource cost | Cooldown | Damage type | Family tag | Synergy hooks | Visual/Audio signature |
|---|---|---|---|---|---|---|---|---|
| 1 | T1 Commit: Third Spark | Trigger condition: another class lands LMB beat 3, summoning a 35% elemental bolt matching the target's most recent status. | Echo tier condition only. | 1.2s ICD | Echo Element | Echo, Strike | Ranger and Shadowblade basic chains gain status texture; Warblade beat 3 can add Echo tag for T4. | Cyan mage phantom flicks a red/blue/gold spark. |
| 2 | T2 Resonance: Arc Contact | Trigger condition: any basic hit has 15-25% Altar-scaled chance to arc 25% damage to one nearby target and apply Echo. | Echo tier condition only. | 0.8s ICD | Echo Element | Echo | Strong in Warblade cleave packs and Ranger marked clusters. | Thin cyan lightning fork, crystalline snap. |
| 3 | T3 Empowered: Convergent Cast | Trigger condition: drafted Q/E/R/F active tagged Echo or Pressure calls Convergence Orb at 50% damage and -10% primary cooldown. | Echo tier condition only. | Skill cooldown | Echo Element | Echo, Pressure | Best on Ranger trap detonations, Warblade Sunder Hook, Shadowblade Shadow Pin. | Phantom rune circle opens behind caster, orb trails cyan. |
| 4 | T4 Rift Proc: Lightbreak Bond | Trigger condition: enemy has 3 different active family tags including Echo; next impact freezes 0.3s and Elementalist echoes Light damage with armor penetration. | Consumes T4 tag state. | T4 tag immunity 4s after proc | Echo Element | Rift, Echo | Converts Warblade Fracture plus Shadowblade Bleed/Veil into a readable Lightbreak burst. | Portrait cyan flash becomes gold-white prism flare. |

## Ranger - Focus

Core loop: maintain range, mark zones, punish enemies that enter the kill line. Focus rewards distance discipline and trap confirmation rather than run-and-gun.

### Active 1-4

| # | Name | Behavior | Resource cost | Cooldown | Damage type | Family tag | Synergy hooks | Visual/Audio signature |
|---|---|---|---|---|---|---|---|---|
| 1 | Aim Shot Bank | LMB-equivalent tap/aim shot that banks Draw Weight up to 3; release consumes bank for crit chance and Pierce length. | 0 Focus; Focus 100 makes next release free and fully banked. | Aim charge timing only | Physical | Pierce, Strike | M23 banking; Warblade pulls enemies into line; Elementalist Lightbreak beam shares aim discipline. | Bowstring tension rise, amber reticle lock, clean release snap. |
| 2 | Pinning Kill Line | RMB main skill: fire a pinning arrow that roots the first enemy hit and marks a straight kill line behind it. | 25 Focus | 6s | Physical | Pierce, Pressure | Warblade cleave through line; Elementalist orb travels along line; Shadowblade phase-through creates backline Scar. | Green line flash on ground, nail-hit thunk, taut wire hum. |
| 3 | Spirit Bow | F V Burst ultimate: for 6s, shots cost no Focus, mark all visible enemies, and trap detonations fire bonus spirit arrows. | Full Kill Zone/V meter or 100 Focus if no V meter exists. | V meter gate only | Physical/Echo | Pierce, Echo | Current canon Spirit Bow; T4 setup with Pierce/Echo while Warblade adds Fracture; Elementalist Echo bolts multiply marked targets. | Ethereal bow overlay, rapid feathered impacts, airy chorus. |
| 4 | Hunter's Step | R Mobility: backstep 2.5m with brief i-frame, leaves a slowing snare patch, and grants Focus if it exits melee range. | 10 Focus; refunds 15 if nearest enemy becomes 4m+ away. | 5s | Physical | Veil, Pressure | M61; triggers instant Aim Shot bank; Shadowblade can phase through snared enemies; Warblade counter covers retreat. | Leaf-sweep dash, muted footfall, snare twang. |

### Passive 1-4

| # | Name | Behavior | Resource cost | Cooldown | Damage type | Family tag | Synergy hooks | Visual/Audio signature |
|---|---|---|---|---|---|---|---|---|
| 1 | Long Sight | Focus gain begins at 3.5m instead of 4m and Focus 75+ grants +10% crit chance in addition to damage. | Passive run pickup. | None | Utility | Pierce | Smooths distance identity without removing close-range danger; Warblade peel keeps uptime. | Focus bar gains amber eye notch. |
| 2 | Trap Cartography | First trap or kill-line trigger on each enemy applies Pressure for 2s and restores 8 Focus. | Passive run pickup. | Once per enemy | Physical | Pressure | M68 build detection: trap-heavy Ranger; Elementalist area spells consume held targets. | Small ground map glyph appears under triggered enemy. |
| 3 | Overdraw Promise | Spending Focus above 75 adds a delayed second arrow for 30% damage after 0.4s. | Passive run pickup. | 0.7s ICD | Physical | Pierce, Echo | Echo-like delayed action from M22/M59; T1/T2 echoes can chain from the delayed hit. | Ghost string pluck after the main shot. |
| 4 | Clean Distance | Killing an enemy at 6m+ refreshes Hunter's Step by 40% and grants one Draw Weight. | Passive run pickup. | 1s ICD | Utility | Strike | Rewards precision kill zones; Warblade/Elementalist setup enables safe long kills. | Distant kill ping, green wind ring under Ranger. |

### Echo triggers 1-4

| # | Name | Behavior | Resource cost | Cooldown | Damage type | Family tag | Synergy hooks | Visual/Audio signature |
|---|---|---|---|---|---|---|---|---|
| 1 | T1 Commit: Phantom Shot | Trigger condition: another class lands LMB beat 3, summoning a 35% Ranger arrow from behind the player toward the target. | Echo tier condition only. | 1.2s ICD | Echo Physical | Pierce, Strike | Adds ranged confirmation to Warblade and Shadowblade chains; can hit pinned lines. | Cyan archer silhouette, thin green-cyan arrow whistle. |
| 2 | T2 Resonance: Marking Nick | Trigger condition: any basic hit has 15-25% Altar-scaled chance to apply a 25% Ranger nick and Marked for 2s. | Echo tier condition only. | 0.8s ICD | Echo Physical | Pierce | Lets Elementalist and Warblade prime Ranger-style kill zones without extra active slots. | Small cyan feather cut, target mark blink. |
| 3 | T3 Empowered: Kill Line Echo | Trigger condition: drafted Q/E/R/F active tagged Pierce or Pressure calls Pinning Kill Line at 50% damage and -10% primary cooldown. | Echo tier condition only. | Skill cooldown | Echo Physical | Pierce, Pressure | Best on Warblade Sunder Hook, Elementalist orb/wall, Shadowblade Shadow Pin. | Cyan bow draw over ground wire, high-tension twang. |
| 4 | T4 Rift Proc: Deadeye Bond | Trigger condition: enemy has 3 different active family tags including Pierce or Pressure; next impact freezes 0.3s and Ranger echoes a marked crit line. | Consumes T4 tag state. | T4 tag immunity 4s after proc | Echo Physical | Rift, Pierce | T4 line hits all enemies sharing Marked; Warblade Fracture and Elementalist Echo complete common setup. | Cyan portrait flash, full-screen reticle blink, distant crit crack. |

## Shadowblade - Energy + Combo

Core loop: spend Energy to enter angles, build Combo Points, then cash finishers. Current code also exposes Sever and Rift Scar; this bank keeps Energy + Combo as task authority and uses Sever/Scar as optional hooks.

### Active 1-4

| # | Name | Behavior | Resource cost | Cooldown | Damage type | Family tag | Synergy hooks | Visual/Audio signature |
|---|---|---|---|---|---|---|---|---|
| 1 | Veil Cut Chain | LMB-equivalent dagger chain that grants 1 Combo Point on beat 1/2 and applies Bleed plus a short Veil mark on beat 3 from behind. | 0 Energy; generates CP. | Basic chain timing only | Physical/Veil | Cut, Bleed | T1 Commit-Beat Echo; Warblade cracks make rear finishers safer; Ranger pinning exposes back arcs. | Soft dagger swishes, violet-cyan slice on beat 3. |
| 2 | Backstab Mark | RMB main skill: dash behind a marked or bleeding target and strike; from rear it grants +2 CP and applies Rift Scar. | 30 Energy | 6s | Physical/Veil | Veil, Bleed | Elementalist Blink Sigil and Ranger Pinning Line create safe target anchors; Warblade pulls expose rear. | Shadow pop behind target, close dagger inhale, scar glyph hiss. |
| 3 | Shadow Dance | F V Burst ultimate: requires high Energy and 5 CP; for 8s, first hit after each skill briefly re-enters stealth and finishers refund Energy. | 80 Energy + 5 CP | V meter gate only | Veil | Veil, Echo | Echo trigger density rises during repeated re-stealth hits; Elementalist Echo tags complete T4. | Screen desaturates, violet afterimages, heartbeat-muted strikes. |
| 4 | Phase Riposte | R Mobility/Parry: phase through 2.4m, ignoring collision briefly; if passing through an enemy attack, gain 1 CP and add Sever/Scar. | 25 Energy | 5s | Veil | Veil, Rift | M61 plus M60; Warblade Counter Step and Shadowblade Phase Riposte define different defensive flavors. | Body smears into cyan-violet line, attack whoosh cuts out, scar crackle. |

### Passive 1-4

| # | Name | Behavior | Resource cost | Cooldown | Damage type | Family tag | Synergy hooks | Visual/Audio signature |
|---|---|---|---|---|---|---|---|---|
| 1 | Cut Ledger | Every third Combo Point gained stores a Ledger Cut; next finisher consumes all Ledger Cuts for +12% damage each. | Passive run pickup. | None | Physical/Veil | Cut, Bleed | M59 combo window and M99-like run-local memory; Ranger marks help preserve combo route. | Small dagger notches light under CP pips. |
| 2 | Scar Interest | Rift Scar collapse restores 12 Energy and refreshes Bleed duration by 1.5s. | Passive run pickup. | 1s per target | Veil | Rift, Bleed | Bridges current Sever/Scar code with task's Energy+Combo identity; Elementalist orb can early-collapse. | Scar glyph folds inward with a low suction pop. |
| 3 | Predatory Patience | Staying unseen or not attacking for 1.2s makes the next Backstab Mark cost 10 less Energy and grants +1 CP on hit. | Passive run pickup. | None | Utility | Veil | Supports stealth cadence without requiring Vanish spam; Warblade/Ranger CC buys patience window. | Violet eye closes then opens above skill icon. |
| 4 | Hemorrhage Arithmetic | Bleed ticks on targets with Rift Scar have a 20% chance to add 1 CP, capped once per second. | Passive run pickup. | 1s ICD | Physical/Veil | Bleed, Rift | M62 status layering; Warblade Fracture and Elementalist Echo complete T4 flag sets. | Thin red-violet tick sparks, quiet counting click. |

### Echo triggers 1-4

| # | Name | Behavior | Resource cost | Cooldown | Damage type | Family tag | Synergy hooks | Visual/Audio signature |
|---|---|---|---|---|---|---|---|---|
| 1 | T1 Commit: Veil Aftercut | Trigger condition: another class lands LMB beat 3, summoning Shadowblade behind the target for a 35% rear slash if space exists. | Echo tier condition only. | 1.2s ICD | Echo Veil | Veil, Cut | Excellent on Ranger pinning and Elementalist root/frost; adds Veil flag to Warblade chains. | Cyan-violet backstab blink, whispered cloth snap. |
| 2 | T2 Resonance: Bleeding Shade | Trigger condition: any basic hit has 15-25% Altar-scaled chance to add 25% shadow nick and 2s minor Bleed. | Echo tier condition only. | 0.8s ICD | Echo Veil | Bleed | Supplies Bleed flag for T4 without forcing primary class to carry bleed skills. | Small dark-cyan dagger trace, wet tick. |
| 3 | T3 Empowered: Phase Twin | Trigger condition: drafted Q/E/R/F active tagged Veil, Cut, or Bleed calls Backstab Mark at 50% damage and -10% primary cooldown. | Echo tier condition only. | Skill cooldown | Echo Veil | Veil, Bleed | Warblade Sundered targets, Ranger pinned targets, and Elementalist frozen targets improve rear access. | Twin phantom crosses target, violet scar remains. |
| 4 | T4 Rift Proc: Scar Collapse Bond | Trigger condition: enemy has 3 different active family tags including Veil or Bleed; next impact freezes 0.3s and Shadowblade collapses Scar for 100% echo damage. | Consumes T4 tag state. | T4 tag immunity 4s after proc | Echo Veil | Rift, Bleed | Most reliable T4 bridge with Warblade Fracture plus Elementalist Echo or Ranger Pierce. | Cyan portrait flash folds into black-violet tear, sharp inhale silence. |

# Synergy matrix

| Primary pair | Combo worth highlighting | Why it matters | Tags likely produced |
|---|---|---|---|
| Warblade + Ranger | Sunder Hook pulls enemies through Pinning Kill Line, then Aim Shot Bank pierces the stacked lane. | Gives Warblade a ranged payoff and Ranger a reliable clump without adding active slots. | Fracture, Pressure, Pierce |
| Warblade + Elementalist | Sundered enemies are held inside Convergence Orb reaction zones; Bladestorm refreshes Fracture while Lightbreak adds Echo/Rift. | Clean T4 setup with readable heavy impact plus spell reaction. | Fracture, Echo, Rift |
| Warblade + Shadowblade | Iron Counter Step creates hitstop and back-exposure; Backstab Mark consumes that opening for Bleed/Veil. | Lets defensive Warblade timing feed assassin execution instead of raw DPS only. | Fracture, Veil, Bleed |
| Elementalist + Ranger | Pinning Kill Line and traps hold enemies long enough for Convergence Orb and Trinity Storm pulses. | Builds a strong area-control archetype without turning Ranger into a caster. | Pierce, Pressure, Echo |
| Elementalist + Shadowblade | Blink Sigil and Rift Scar create geometry points; Scar collapse can detonate orb reactions early. | Converts positioning into reaction timing, matching both class identities. | Echo, Veil, Rift |
| Ranger + Shadowblade | Pinning Kill Line exposes rear arcs; Phase Riposte crosses pinned enemies and adds Scar/Bleed. | Turns trap play into assassin route planning. | Pierce, Pressure, Bleed |
| Any + Warblade Echo | T2 Crack on Contact supplies Fracture flag to non-melee primaries. | Necessary for scalable T4 without curated per-skill pair lists. | Fracture, Rift |
| Any + Elementalist Echo | T2 Arc Contact supplies Echo flag and small chain damage to basic hits. | Makes Echo tag accessible from simple play while keeping full reactions on Elementalist. | Echo, Rift |
| Any + Ranger Echo | T3 Kill Line Echo adds Pierce/Pressure to major active casts. | Creates battlefield geometry from any primary's empowered skill. | Pierce, Pressure |
| Any + Shadowblade Echo | T2 Bleeding Shade supplies Bleed and T3 Phase Twin supplies Veil/Bleed. | Lets T4 reach the Bleed/Veil side without requiring all primaries to add DoT skills. | Bleed, Veil |

# Implementation priority order

| Day | Priority | Output |
|---|---|---|
| Day 1 | Lock data schema for SkillDesignDoc entries: slot intent, resource cost, cooldown, damage type, 1-2 family tags, synergy notes, VFX/SFX placeholder. | One ScriptableObject/doc schema that can hold all 48 entries without code behavior yet. |
| Day 2 | Implement or map active baseline for Warblade and Ranger because their current controllers already expose close/line/control identities clearly. | Warblade/Ranger pool entries mapped to existing code names where possible; missing behavior flagged. |
| Day 3 | Implement or map active baseline for Elementalist and Shadowblade, preserving Elementalist Lightbreak and Shadowblade Energy+Combo while noting Sever/Scar bridge hooks. | Elementalist/Shadowblade pool entries mapped; resource authority documented. |
| Day 4 | Add passive upgrade data only, no behavior rewrites: cost modifiers, refund triggers, tag durations, and UI text. | 16 passive entries draftable as run-acquired buffs. |
| Day 5 | Add Echo trigger data T1-T3 algorithmically by class: Commit-Beat, Resonance Hit, Empowered Skill. | 12 trigger entries wired to class tags and generic chance/ICD values. |
| Day 6 | Add T4 Rift Proc detection pass using active family tag flags and 4s post-proc immunity. | Four class-specific T4 echo profiles plus shared tag consumption rules. |
| Day 7 | QC pass: 4 active slot limit, no extra active buttons, tag coverage, cooldown sanity, resource loop sanity, visual readability. | Balance review sheet and next implementation tickets. |
