# RIMA Act 2-3 Enemy Archetype Research and Proposals

## Step 1 -- Reference Behavioral Archetypes Not Yet Covered

1. Delayed homing death hazard: a killed target releases a visible orb that chases briefly before detonating, asking the player to keep playing after the kill.
2. Line-of-sight nuke: a large windup attack that is avoided by breaking sight, not by pure distance.
3. Proximity stance swap: enemy changes kit when the player is close versus far, punishing one-range autopilot.
4. Projectile tax screen: stationary or slow enemy forces either burst, cover, or route discipline through persistent ranged pressure.
5. Anti-tunnel artillery: enemy attacks the player from outside normal melee focus and must be located by telegraph direction.
6. Death barrage: enemy drops or releases a pattern on death, turning greedy executes into spacing checks.
7. Corpse consumer: enemy consumes corpses or dead minions to empower itself or create zones.
8. Summoner with decaying minions: adds are temporary and fragile but force command-priority decisions.
9. Shield-facing defender: enemy is weak from one angle and resilient from another, asking the player to reposition.
10. Splitter: enemy divides into lesser threats, asking whether burst is still the right answer.
11. Low-health desperation cast: enemy gains one final dangerous behavior at threshold, asking whether the player can pause burst.
12. Area denial turret: stationary enemy owns a radius until killed or disabled.
13. Pull-and-slam bruiser: enemy relocates the player into a danger zone, asking whether range is actually safe.
14. Modifier carrier: elite enemy projects a rule change onto nearby enemies rather than dealing most damage itself.
15. Marked target punisher: enemy reacts to repeated hits, focus fire, or high stack application.
16. Trap invalidator: enemy detects or consumes placed hazards, asking trap builds to manage bait and timing.
17. Minion predator: enemy prioritizes summons and converts them into tempo or shields.
18. Pattern rotator: enemy cycles attack geometry in a fixed sequence, rewarding memory and observation over reflex.
19. Resource drain aura: enemy pressures class-specific resource loops rather than raw HP.
20. Weak-spot window enemy: enemy is briefly vulnerable after overcommitting, rewarding timed punish instead of constant DPS.

## [M01] -- Oathglass Sentinel
**Role:** facing shield defender / weak-spot window
**Threat:** 3pt | **Size:** 72px
**Habit Broken:** "Do you always attack from the nearest side?"
**Lore Hook:** A prison guard statue whose oath crystal still remembers the front gate it failed to defend.
**Skills:**
- Auto: Slow halberd jab in a 70-degree front arc, low damage and clear 0.6s arm glow.
- Skill 1 -- Mirror Guard: 0.5s tell -> faces the highest recent damage source and gains 80% frontal damage reduction for 3s; rear hits apply 1 Cracked stack to it | CD: 7s | Window: 3s
- Skill 2 -- Oath Sweep: 0.8s tell -> 160-degree sweep that knocks back 1.2m and leaves a 2s cracked-stone slow strip | CD: 8s | Window: 0.9s after sweep
- Skill 3 -- Vow Fracture: after receiving 4 Cracked or Broken stacks -> kneels for 1.2s, taking +35% damage from rear attacks | CD: reactive | Window: 1.2s
**VFX Note:** Front shield is a blue-white arc overlay; weak rear window uses amber outline pulse. Hits use standard white/accent flash and 50ms hit-stop; weak rear hit uses 90ms.
**Class Interactions:** Shadowblade and Ronin can phase or time behind it for clean punish. Brawler and Warblade can force Vow Fracture through Cracked/Broken stacks. Ranger struggles if kiting in a straight line into the shield face; Summoner must command minions behind it or lose DPS.
**Spawn Synergies:** Works with Fracture Imps or low-value swarms that make rear access messy. Good with Relic Caster if the shield is not also shielding the Sentinel.
**Anti-patterns:** Do not spawn more than two in a narrow bridge room. Do not pair with Chain Warden in Act 2 corridors because forced facing plus pull can erase the reposition lesson.
**Acts:** Act 2
**Inspired by:** Hades shield enemies and Dead Cells shield-facing pressure, adapted into isometric angle play with a status-driven weak window.

## [M02] -- Mortar Penitent
**Role:** anti-tunnel artillery / death barrage
**Threat:** 4pt | **Size:** 64px
**Habit Broken:** "Did you forget the enemy you cannot currently see?"
**Lore Hook:** A broken chapel penitent fused to a cracked censer that lobs rift ash from behind prison walls.
**Skills:**
- Auto: None; it retreats instead of basic attacking.
- Skill 1 -- Ash Bell: 0.9s tell -> marks three ground circles in sequence; each lands after 0.7s, dealing medium damage and applying Wounded for 4s | CD: 7s | Window: 0.7s per circle
- Skill 2 -- Close Confession: 0.5s tell when player is within 2.5m -> short cone ash blast, low damage, 40% slow for 1.5s, then it hops back 2m | CD: 6s | Window: 0.5s
- Skill 3 -- Final Censer: on death -> drops one expanding ash ring after 1s; ring applies Hexed 2 stacks and low damage | CD: once | Window: 1s
**VFX Note:** Ground target rings and falling ash pips are engine overlays; death ring is a radial expanding decal. No custom death animation needed.
**Class Interactions:** Ranger and Gunslinger can punish if they maintain sight lines while moving. Elementalist can use Frost zones to keep it from retreating. Hexer benefits from self-Hexed targets but must avoid death rings. Summoner risks minions body-blocking pursuit while artillery keeps firing.
**Spawn Synergies:** Pairs well with Oathglass Sentinel or Ruin Hulk variants because players may tunnel on visible bodies while shells land.
**Anti-patterns:** Do not pair with more than one other offscreen-capable attacker. Avoid in rooms without enough floor contrast for target rings.
**Acts:** Act 2 / Act 3
**Inspired by:** Hades Inferno-Bomber ranged bombs and Dead Cells Bombardier pressure, adapted to visible sequential ground markers and RIMA status hooks.

## [M03] -- Rift Usher
**Role:** line-of-sight nuke / cover lesson
**Threat:** 5pt | **Size:** 88px
**Habit Broken:** "Can you use the room, not just your dash?"
**Lore Hook:** A ritual usher split open by the Rift, still directing intruders to stand where the ceremony can see them.
**Skills:**
- Auto: Slow staff tap at melee range, very low damage, mostly to discourage standing inside it.
- Skill 1 -- Witness Beam: 1.3s tell -> locks a bright sight line to the player; if line of sight remains at release, fires a wide beam for high damage and 2 Broken stacks | CD: 10s | Window: 1.3s
- Skill 2 -- Seat the Faithful: 0.7s tell -> creates two 3s rectangular rift pew zones that block movement lightly and count as cover for Witness Beam | CD: 12s | Window: zone placement readable for 0.7s
- Skill 3 -- Open Congregation: at 40% HP -> gains 20% move speed and Witness Beam CD drops to 7s for 8s | CD: once | Window: 8s state
**VFX Note:** Beam uses a straight red-purple warning line and tile-highlighted cover rectangles. Damage flash is accent purple; posture-like weak punish after beam miss uses 90ms hit-stop.
**Class Interactions:** Ranger and Gunslinger must break line instead of only backpedaling. Shadowblade can phase through pew zones but still needs LOS discipline. Ronin can punish the long recovery after a missed beam. Summoner can use minions to pressure during cover movement but minions should not block the beam for the player.
**Spawn Synergies:** Strong with slow melee grunts that push the player out of cover. Good mini-elite anchor for Act 3 ritual rooms.
**Anti-patterns:** Do not spawn in tiny rooms with no pillars or generated cover. Avoid pairing with Chain Warden pulls until players have learned the LOS rule.
**Acts:** Act 3
**Inspired by:** Risk of Rain 2 Wandering Vagrant LOS avoidance and PoE-style huge telegraphs, scaled down into a room-reading special.

## [M04] -- Ossuary Indexer
**Role:** corpse consumer / modifier carrier
**Threat:** 4pt | **Size:** 56px
**Habit Broken:** "Are dead bodies still part of the fight?"
**Lore Hook:** An archivist construct that catalogs bone fragments until the Rift teaches it to file them inside living guards.
**Skills:**
- Auto: Bone stylus poke, low damage, 0.5s tell.
- Skill 1 -- Catalog Remains: 0.8s tell -> consumes up to 3 nearby corpses or destroyed swarm bodies; each consumed corpse grants nearby enemies +8% move speed for 5s and gives the Indexer one Bone Page | CD: 8s | Window: 0.8s
- Skill 2 -- Errata Burst: 0.6s tell -> spends Bone Pages to fire that many slow bone shards in a fan; shards apply Cracked 1 stack | CD: 6s | Window: 0.6s
- Skill 3 -- Redaction Field: if Hexed at 5+ stacks -> purges 3 Hex stacks and creates a 3s silence-like curse dampening zone where new Hex stacks on enemies are reduced by 50% | CD: 12s | Window: 3s zone
**VFX Note:** Corpse consumption is a thin line overlay from corpses to the Indexer; buffs are small bone-page icons over affected enemies. No corpse animation required.
**Class Interactions:** Hexer must decide whether to spend Hexblast before Redaction Field. Summoner can feed it if minions die carelessly, but sacrifice timing can bait Catalog before a burst. Brawler likes shard-applied Cracked for reversals if it survives. Warblade executes can deny corpses if implementation supports corpse-clearing tags.
**Spawn Synergies:** Excellent with Fracture Imp waves and Mortar Penitent death zones. Turns swarm rooms into sequencing puzzles.
**Anti-patterns:** Do not spawn with multiple high-corpse producers and a Relic Caster in the same 8-12pt room; it becomes priority overload.
**Acts:** Act 2
**Inspired by:** PoE corpse interaction and Last Epoch minion/rare support behaviors, adapted into clear corpse-line VFX and status economy.

## [M05] -- Scar-Eater Shade
**Role:** repeated-hit punisher / trap invalidator
**Threat:** 3pt | **Size:** 48px
**Habit Broken:** "Do you always stack your full combo on one target?"
**Lore Hook:** A Rift shade that feeds on repeated cuts in the same patch of reality, mistaking combat patterns for doors.
**Skills:**
- Auto: Quick claw, low damage, only used after blinking in.
- Skill 1 -- Pattern Taste: reactive -> every third hit received from the same player source within 2s grants it a Scar Charge; at 2 charges it blinks 2m sideways and drops a 2s void stain | CD: 5s internal | Window: blink punish after charge
- Skill 2 -- Snare Sip: 0.4s tell near a player trap or static summon -> consumes one trap-like object or damages one minion for low damage, gaining 30% dodge chance for 3s | CD: 9s | Window: 0.4s
- Skill 3 -- Open Cut: 0.7s tell -> dashes through the player in a line, applying Hexed 1 and Wounded for 3s | CD: 7s | Window: 0.7s
**VFX Note:** Scar Charge is shown by two small violet ticks above the sprite; blink is a short afterimage overlay, no new animation frames.
**Class Interactions:** Shadowblade must vary Scar geometry instead of repeating one echo line. Gunslinger and Elementalist rapid-hit builds can accidentally trigger Pattern Taste. Ranger trap builds must bait Snare Sip with low-value traps. Ronin and Warblade single timed hits are advantaged.
**Spawn Synergies:** Good with Oathglass Sentinel because repositioning behind one enemy can expose repeated-hit habits on another.
**Anti-patterns:** Avoid packs of more than three; multiple side blinks create visual clutter. Do not pair with Chain Warden in dense rooms.
**Acts:** Act 2 / Act 3
**Inspired by:** Dead Cells agile enemies and PoE anti-burst rare behaviors, adapted to RIMA's class-specific source tracking.

## [M06] -- Choir Larva
**Role:** splitter / aura chorus
**Threat:** 2pt | **Size:** 40px
**Habit Broken:** "Is killing the small thing immediately always correct?"
**Lore Hook:** Rift larvae hatched inside ossuary hymnals; each carries one missing note of a broken sacred chord.
**Skills:**
- Auto: Tiny bite, very low damage.
- Skill 1 -- Harmonic Split: on death while not Broken -> splits into two 1pt Echo Notes with 35% HP each; Echo Notes live 7s and apply a stacking 5% enemy attack speed aura in 2.5m | CD: once | Window: death decision
- Skill 2 -- Discord Pulse: 0.7s tell -> if at least two Choir Larvae or Echo Notes are within 4m, pulse low damage and apply Cracked 1 stack in a ring | CD: 8s | Window: 0.7s
- Skill 3 -- Shattered Note: if killed while Broken -> does not split and instead drops a 1s harmless silence flash that clears one nearby enemy aura | CD: reactive | Window: setup before kill
**VFX Note:** Split is two small tinted copies using existing sprite scale/alpha; aura is a thin musical ring overlay. Standard hits only.
**Class Interactions:** Warblade can Broken-tag before execute to deny split. Brawler can use Cracked interactions but risks feeding Discord Pulse. Elementalist AoE may create too many Echo Notes if careless. Summoner can assign minions to hold Notes away from the pack.
**Spawn Synergies:** Good with Mortar Penitent or Oathglass Sentinel because delayed small threats amplify larger reads.
**Anti-patterns:** Do not spawn with Fracture Imp swarms in early Act 2 rooms; too much small-body accounting. Cap total Echo Notes per room.
**Acts:** Act 2 / Act 3
**Inspired by:** Hades Splitter-style enemies and Dead Cells swarm escalation, adapted with a Broken counterplay gate.

## [M07] -- Heat Leech Jailer
**Role:** resource drain aura / proximity stance swap
**Threat:** 4pt | **Size:** 68px
**Habit Broken:** "Are you managing your resource, or just spending on rhythm?"
**Lore Hook:** A jailer whose key-ring melted into a Rift siphon that drinks effort, heat, fury, and focus from prisoners.
**Skills:**
- Auto: Chain key swipe, low damage with a 0.6s tell.
- Skill 1 -- Lockstep Drain: 0.8s tell -> 4s aura; players inside 3m lose class tempo faster: Heat rises +20%, Focus decay +35%, Fury generation -25%, Tension gain -25% | CD: 11s | Window: 4s
- Skill 2 -- Far Key: if player stays beyond 5m for 2s -> throws a slow key projectile that creates a 2s small pull zone on landing | CD: 7s | Window: projectile travel 0.8s
- Skill 3 -- Close Accounts: if player remains within 2m for 1.5s during Lockstep Drain -> short STRIKE slam, medium damage, applies Wounded 3s | CD: 6s | Window: 0.6s slam tell
**VFX Note:** Drain aura is a circular lock glyph under the enemy; resource effects are UI-side debuff icons, not bespoke animation.
**Class Interactions:** Gunslinger must manage Overheat instead of burst dumping inside aura. Ranger must rotate through midrange because Focus collapses close. Ravager loses Fury tempo but can exploit low-life windows if willing to risk Wounded. Ronin must time draws outside drain windows.
**Spawn Synergies:** Excellent with medium melee pressure that herds players through aura edges. Good Act 2 prison identity enemy.
**Anti-patterns:** Do not pair with Penitent Bruiser; anti-heal plus resource suppression is too punitive. Avoid two Heat Leech Jailers in one room.
**Acts:** Act 2
**Inspired by:** Last Epoch rare aura/modifier pressure and Hades proximity kit swaps, adapted to RIMA's class resource identities.

## [M08] -- Prism Apostate
**Role:** pattern rotator / status vulnerability puzzle
**Threat:** 5pt | **Size:** 80px
**Habit Broken:** "Can you change damage plan when the enemy changes rules?"
**Lore Hook:** A ritual mathematician crystallized mid-proof, now rotating broken sacred geometry through living targets.
**Skills:**
- Auto: None; it floats slowly and rotates.
- Skill 1 -- Threefold Proof: 1s tell -> fires one of three repeating patterns in order: line beam, triangle burst, ring pulse. Each pattern applies a different public state: Broken, Cracked, Wounded | CD: 5s | Window: 1s
- Skill 2 -- False Symmetry: 0.6s tell -> gains one rotating resistance for 5s: resists the last public state applied to it by 60%, but becomes vulnerable to the next state in cycle by +40% | CD: 9s | Window: 5s
- Skill 3 -- Prism Collapse: after all four public states have been applied at least once -> self-stuns for 2s and emits harmless crystal shards | CD: once per 18s | Window: 2s
**VFX Note:** Uses colored geometric overlays: line, triangle, ring, and rotating state icons. Weak window uses bright crystal crack overlay plus 90ms hit-stop.
**Class Interactions:** Elementalist naturally understands cycling and can align spell shapes with pattern gaps. Warblade, Brawler, and Hexer coordinate public states for Prism Collapse. Ronin benefits from the 2s collapse window. Pure single-state builds must hold damage until the vulnerability rotates.
**Spawn Synergies:** Works as a room centerpiece with simple grunts or swarms. Good Act 3 lesson enemy before ritual bosses.
**Anti-patterns:** Do not pair with other enemies that heavily modify public states in the same introductory room. Avoid low-contrast crystal floors unless telegraphs are recolored.
**Acts:** Act 3
**Inspired by:** Hades fixed-pattern clarity, Last Epoch ward/break pacing, and PoE resistance puzzle enemies, translated into RIMA's public-state system.

## [M09] -- Hollow Bailiff
**Role:** minion predator / summon converter
**Threat:** 4pt | **Size:** 72px
**Habit Broken:** "Are your minions disposable, or are they feeding the room?"
**Lore Hook:** A prison court officer whose writs now bind any servant, corpse, or summoned thing into Rift custody.
**Skills:**
- Auto: Gavel strike, slow and low damage.
- Skill 1 -- Seize Servant: 0.9s tell targeting nearest minion/summon -> deals high minion damage; if it kills or consumes the target within 1s, Bailiff gains a 4s shield equal to 12% max HP | CD: 9s | Window: 0.9s interrupt/recall
- Skill 2 -- Contempt Ring: 0.7s tell -> creates a 3m ring for 3s; player damage from outside the ring is reduced by 35%, but damage from inside is normal | CD: 11s | Window: 3s
- Skill 3 -- Writ Returned: when shield is broken -> releases a short cone toward the last minion source, applying Hexed 2 to enemies hit and Wounded to player if in cone | CD: reactive | Window: shield-break read
**VFX Note:** Seize uses a chain-line overlay to minion; shield is a gold-purple outline. No grapple animation; target simply takes damage.
**Class Interactions:** Summoner must recall, sacrifice early, or command focus to break the shield. Ranger and Gunslinger are forced into dangerous inside-ring angles. Ravager can trade HP to break shield quickly. Hexer can profit from Writ Returned if positioning turns it against other enemies.
**Spawn Synergies:** Good with Oathglass Sentinel because inside-ring movement matters. Strong with slow bruisers, not with fast swarms.
**Anti-patterns:** Do not spawn in rooms designed as Summoner tutorial spaces unless explicitly teaching minion recall. Avoid pairing with Ossuary Indexer in high-summon rooms.
**Acts:** Act 2 / Act 3
**Inspired by:** PoE minion/corpse exploitation and Last Epoch summonable minion pressure, built as an explicit Summoner counter with non-Summoner range implications.

## [M10] -- Volatile Reliquary
**Role:** delayed homing death hazard / low-health desperation cast
**Threat:** 3pt | **Size:** 52px
**Habit Broken:** "Did you stop respecting the enemy after it died?"
**Lore Hook:** A reliquary box filled with saint bones and Rift glass, walking until the relic inside chooses a new martyr.
**Skills:**
- Auto: Slow box slam, low damage, 0.7s tell.
- Skill 1 -- Martyr Spark: on death -> releases one visible slow homing spark for 4s; spark arms after 0.8s, accelerates after 2s, detonates on contact or expiry for medium damage and Hexed 2 | CD: once | Window: 0.8s arming plus kite time
- Skill 2 -- Panic Litany: at 30% HP -> pulses red for 1.2s, then gains 35% move speed and tries to die near the player for 4s | CD: once | Window: 1.2s
- Skill 3 -- Reliquary Seal: if killed while Wounded -> Martyr Spark duration is reduced to 2s and does 25% less damage | CD: reactive | Window: pre-kill setup
**VFX Note:** Spark is a small engine-side orb with outline and expanding detonation ring; death flash uses the existing accent flash. No custom on-death frames.
**Class Interactions:** Ranger and Gunslinger must keep moving after kills instead of looting mentally. Warblade execute timing can be dangerous unless Wounded is applied first by allies. Hexer can trigger high burst but must plan Spark chase. Elementalist Frost can slow the Reliquary before Panic Litany reaches melee.
**Spawn Synergies:** Good with single elite anchors; the Spark forces movement during the next threat. Pairs with Rift Usher if cover placement leaves safe kite lanes.
**Anti-patterns:** Do not spawn more than three in one room. Do not pair with heavy visual-noise enemies or invisible floor hazards; death hazard clarity is mandatory.
**Acts:** Act 3
**Inspired by:** PoE Volatile Flameblood/Volatile Dead and Hades Inferno-Bomber death bombs, adapted with arming time and Wounded counterplay.

## Behavioral Archetypes Analysis

Last Epoch's useful lesson for RIMA is not one iconic enemy pattern but the way enemies exist inside a modifier ecosystem. A normal creature can become a different tactical read when rarity, affixes, summons, zone corruption, or boss ward pacing alter the context. RIMA can borrow that at a smaller scale by giving Act 2-3 enemies rule-based hooks into public states, class resources, and room composition. The danger is opacity: if a modifier changes the question, the visual language must change as well.

Dead Cells is strongest at enemy role readability. A player often understands the immediate job after one death or one clean observation: kill the Shocker before committing inside the radius, respect the Bomber's overhead angle, punish the Zombie's leap recovery, route around projectile pressure. Its mobs are compact verbs. RIMA should copy the discipline, not the side-view specifics: each enemy should own one main question, one backup behavior, and a readable recovery. Dead Cells also shows how biome placement makes enemies feel deeper than their move lists; a simple role becomes harder beside ledges, doors, traps, or narrow routes.

Hades gives RIMA a model for dense room readability under high speed. Many Hades enemies are simple alone but strong in combined grammar: armored bodies buy time for bombers, witches, chariots, crystals, and shield carriers. Armor also lets a design survive long enough to express its question without requiring complex animation. RIMA's threat budget can use this by treating specials as sentence fragments: one asks for movement, one asks for priority, one asks for angle, but the room should not ask five new questions at once.

Risk of Rain 2 contributes scale lessons. Its enemies often ask spatial questions that grow harder over time: line of sight, long-range tracking, projectile density, flying targets, and delayed super attacks. The important adaptation for RIMA is not enormous arenas but environmental counterplay. Rift Usher-style LOS checks can make isometric rooms matter if the cover is reliable, visible, and never contradicted by unclear collision.

Path of Exile is most valuable as both inspiration and warning. Corpse, volatile, aura, and rare-mod enemies create post-kill and post-burst decisions that ARPG players remember. They also become hated when unreadable, instant, or hidden under effects. RIMA can use PoE's idea of delayed consequences while enforcing stricter clarity: arming windows, ground rings, maximum counts, and status-based mitigation. The takeaway is to make every unfair-feeling ARPG trope into a fair question with a visible answer.

## Research Sources

- Last Epoch official wiki and patch notes: enemy clusters, rarity affixes, summons, ward pacing, Liath teleport/summon reference.
- Dead Cells official wiki: enemy role examples including Zombie, Shocker, Bomber, Bombardier, and biome role tables.
- Hades wiki: Inferno-Bomber ranged bomb, close barrage, and death barrage behavior; general enemy roster references.
- Risk of Rain 2 wiki: Wandering Vagrant orb, LOS-style super attack counterplay, long-range boss pressure.
- PoEDB / Path of Exile community wiki mirror: Volatile Dead and Volatile-style homing orb behavior, corpse consumption, delayed explosive pressure.
