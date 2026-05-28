# RIMA Mob Ideas — Gemini Research Report
*Generated: 2026-05-08*

---

## Part 1: Research Summary

Modern roguelite enemy design centers on "behavioral questioning" — forcing players to adapt their established patterns rather than inflating statistics. Across *Last Epoch*, *Dead Cells*, *Hades*, *Risk of Rain 2*, and *Path of Exile*, the common thread is the shift from "damage sponges" to "tactical obstacles." *Last Epoch* emphasizes arena-wide positional challenges, while *Dead Cells* prioritizes distinct, telegraphed behaviors that punish greedy inputs (e.g., panic rolling). *Hades* introduces biome-specific escalation where enemies evolve in complexity, and *Risk of Rain 2* utilizes elite modifiers (Glacial, Celestine) to recontextualize existing assets by manipulating space and vision. *Path of Exile* provides the most robust template for tactical zones, using auras and death effects (e.g., Soul Conduit) to create kill-priority hierarchies.

Unique to each: *Dead Cells* excels at "frame-trap" enemy design (Shieldbearers immune to frontal attacks, Lacerators that punish panic-rolls); *RoR2* proves that modifier overlays can simulate infinite variety with limited assets; *PoE* pioneers the "tactical zone" mechanic where positioning relative to the mob defines the encounter. These games move away from memorization-heavy encounters, rewarding reactive, instinctual play. The shared mechanic is deliberate creation of "threat zones" that disrupt the player's optimal output window, demanding execution priority and disciplined movement.

---

## Part 2: Behavioral Gap Analysis

**Gap #1** | [Dead Cells] | [Blind aggression] | [A mob that reflects damage if attacked from the front during its charge creates a commit-or-dodge decision.]
**Gap #2** | [PoE] | [Neglecting kill-priority] | [A "Beacon" mob granting invulnerability to allies in a 5m radius forces immediate execution switching.]
**Gap #3** | [RoR2] | [Ignoring ground effects] | [A mob leaving a decay trail dealing %HP damage per tick teaches floor-awareness over pure DPS focus.]
**Gap #4** | [Hades] | [Fixed-point pathing] | [An enemy that teleports behind the player upon taking any damage punishes attacking without repositioning intent.]
**Gap #5** | [Last Epoch] | [Stationary combat] | [An elite that progressively corrupts arena sectors forces players to move even during their DPS windows.]
**Gap #6** | [Dead Cells] | [Panic rolling] | [A mob with a tracking 360-degree spin punishes reflex-dashes during combat.]
**Gap #7** | [PoE] | [Safe-distance kiting] | [A summoner that creates wall-penetrating projectiles invalidates terrain as a defensive tool.]
**Gap #8** | [RoR2] | [Visual tunnel-vision] | [A mob that generates a fog-shroud around nearby enemies tests whether the player tracks enemy count under fog.]
**Gap #9** | [Hades] | [Over-committing to a single target] | [A Splitter that spawns two faster versions upon death punishes burst-then-ignore habits.]
**Gap #10** | [Last Epoch] | [Single-target focus] | [An elite that links its own damage taken to a nearby mob punishes AoE-neglect while tanking.]
**Gap #11** | [PoE] | [Lifesteal abuse] | [An enemy that heals for every 5% HP the player loses within 5m flips the value of lifesteal builds.]
**Gap #12** | [Dead Cells] | [No crowd control] | [A hookshot mob that pulls players into melee range tests whether the player respects minimum distance.]
**Gap #13** | [RoR2] | [Cooldown mismanagement] | [A "Silence aura" mob disabling skills for 2s on contact forces skill timing discipline.]
**Gap #14** | [Hades] | [Ignoring flanking] | [A mob that crits if it hits the player's back trains spatial awareness and rotation habits.]
**Gap #15** | [PoE] | [Overstaying welcome] | [A mob that detonates on death with a large blast radius punishes players who linger in kill-zones.]

---

## Part 3: 10 Enemy Proposals for RIMA Act 2-3

---

### [M01] — Rift-Eye Stalker
**Role:** Back-attack punisher
**Threat:** 2pt | **Size:** 40px
**Habit Broken:** "Can I safely dash through or behind every enemy?"
**Lore Hook:** A lingering gaze pulled from the rift now materializes in the Prison corridors, orienting always toward the player's back.
**Skills:**
- Auto: None — skill-only threat
- Skill 1: Blink-Back | 0.8s tell → Teleports behind player + 10-15 dmg melee | CD: 4s
- Skill 2: Shadow-Slash | 0.5s tell → Short-range cleave with 1s slow on player | CD: 3s
**VFX (engine-side only):** Purple void-flicker shader pulse on mob body at Blink-Back tell; teleport destination marked by brief violet ring particle (0.3s); shadow trail particles on dash.
**Best vs:** Shadowblade (high-mobility play backfires), Warblade (armor-break requires facing enemy — Stalker invalidates that axis).
**Spawn Combos:** Pair with Seam Crawlers (underground pressure from below, blink pressure from behind forces a non-existent safe angle).
**Forbidden Spawns:** Cannot spawn with Chain Warden (both demand high movement investment; combined the player has no valid response window).
**Acts:** Act 2
**Inspired by:** Dead Cells — Dark Tracker (back-crit mechanic); adapted to teleport-repositioning rather than static flanking.

---

### [M02] — Crystal Ossuary
**Role:** Summon-that-must-die / kill-priority anchor
**Threat:** 5pt | **Size:** 60px
**Habit Broken:** "Should I clear the room or destroy the source first?"
**Lore Hook:** Infused with rift energy leaking through the Ossuary walls, this crystalline structure animates skeletal remains with increasing frequency.
**Skills:**
- Auto: None — stationary
- Skill 1: Animate Bones | 1.2s tell → Spawns 2 Shard-Grunts (1pt each) | CD: 8s
- Skill 2: Shield-Link | 1.0s tell → Grants 50% damage reduction to all mobs within 5m for 4s | CD: 10s
**VFX (engine-side only):** Glow-pulse shader cycling green→white on Animate Bones tell; floating rift-crystal particle system orbiting body; Shield-Link emits white motes toward linked mobs via particle lines.
**Best vs:** Gunslinger (needs rapid target switching mid-rhythm), Summoner (competing minion economy confuses threat reading).
**Spawn Combos:** Pair with Ruin Hulks (hulk becomes dangerously tanky under Shield-Link; false threat becomes real threat).
**Forbidden Spawns:** Cannot spawn with Fracture Imps (Shard-Grunt spawns combine with existing swarm to overwhelm room threat budget).
**Acts:** Both (Act 2 and Act 3)
**Inspired by:** Path of Exile — Necromancer modifier; adapted to a stationary anchor role that forces kill-priority over room-clearing habits.

---

### [M03] — Ashen Weaver
**Role:** Zone-denier with decay trail
**Threat:** 2pt | **Size:** 45px
**Habit Broken:** "Is this floor space safe for the next 5 seconds?"
**Lore Hook:** Corrupted embers of former Shattered Keep guards, trailing rot through the Ossuary as they shamble.
**Skills:**
- Auto: Weak melee slash (25% DPS ceiling respected)
- Skill 1: Embers Trail | 0.5s tell → Leaves a 3s burning ground patch (5 dmg/tick, 0.5s interval) behind movement path | CD: 3s
- Skill 2: Flame Burst | 1.0s tell → Detonates any existing patch within 4m for 20 dmg burst | CD: 6s
**VFX (engine-side only):** Orange/red smoke particle trail spawned continuously at mob feet during Embers Trail; lingering fire shader overlay on ground tiles (alpha fades over 3s); Flame Burst triggers screen-edge orange flash + local particle explosion.
**Best vs:** Ravager (low-life builds cannot sustain %HP tick damage), Brawler (needs stationary positioning to complete juggle combos).
**Spawn Combos:** Pair with Penitent Bruiser — decay zones force player into the Bruiser's anti-heal aura radius.
**Forbidden Spawns:** No hard restrictions.
**Acts:** Act 2
**Inspired by:** Risk of Rain 2 — Blazing Elite trail mechanic; adapted to ground-persistence and a detonation skill for player-initiated interaction.

---

### [M04] — Mirror-Guard
**Role:** Reflect / counter mechanic
**Threat:** 3pt | **Size:** 50px
**Habit Broken:** "Can I unleash my highest-damage skill the moment I enter range?"
**Lore Hook:** Prison guards whose polished plate armor was ritually treated to reflect the rift's own energy back at attackers.
**Skills:**
- Auto: Slow deliberate thrust (short range)
- Skill 1: Mirror Stance | 0.3s tell (shield raise animation) → Reflects 50% of incoming frontal damage for 2s | CD: 7s
- Skill 2: Retaliate | 0.6s tell → Lunging stab if hit during Mirror Stance; deals 25-35 dmg | CD: 5s
**VFX (engine-side only):** Shimmering chrome shader on chest/shield during Stance; player-hit during Stance spawns reversed directional particle burst back toward player; Retaliate has bright white flash (engine hit reaction, extended to 0.15s for feedback clarity).
**Best vs:** Ranger (burst-on-mark habit punished), Ronin (Iaido commit timing collides directly with Stance window).
**Spawn Combos:** Pair with Riftbound Augur — Augur time-slow forces player into face-to-face range where Stance applies.
**Forbidden Spawns:** Cannot spawn with Fracture Imps (AoE from swarm hits Mirror-Guard incidentally, causing reflect to splash unpredictably — untestable damage scenarios).
**Acts:** Act 3
**Inspired by:** Dead Cells — Shieldbearer (frontal immunity); adapted with an explicit punish-counter skill instead of passive immunity only.

---

### [M05] — Void-Shade
**Role:** Vision disruptor / swarm support
**Threat:** 1pt | **Size:** 35px
**Habit Broken:** "Do I have perfect information about how many enemies are in this room?"
**Lore Hook:** Rift-matter coalescing into barely-visible forms, existing primarily to obscure the truth of the depths from living eyes.
**Skills:**
- Auto: Weak touch (almost negligible)
- Skill 1: Void Fog | 0.9s tell → Spawns 5m radius cloud (50% visual obscuration via shader, 4s duration) | CD: 6s
- Skill 2: Stealth Shift | 0.5s tell → Mob becomes visually near-transparent for 3s (still targetable via audio cues) | CD: 5s
**VFX (engine-side only):** Fog cloud = large grey-scale particle system with low opacity; Stealth Shift = mob sprite alpha drops to 15% + white-noise edge shader; audio cue (hiss) triggers at Stealth Shift to preserve fairness.
**Best vs:** Gunslinger (Heat-rhythm needs visible targets to maintain cadence), Ranger (Focus resource and mark system requires target identification).
**Spawn Combos:** Pair with Hollow Hulk — hulk hidden inside fog until melee range; player realizes too late.
**Forbidden Spawns:** No hard restrictions but should not appear in more than 2-per-room — stacking fog clouds creates untestable visibility conditions.
**Acts:** Act 3
**Inspired by:** Risk of Rain 2 — Celestine Elite (enemy-concealment mechanic); adapted to a dedicated swarm unit rather than a modifier overlay.

---

### [M06] — Corrupt Satyr
**Role:** Agile harasser / flanking dart-thrower
**Threat:** 2pt | **Size:** 40px
**Habit Broken:** "Can I track and lock down a fast-moving target while managing other threats?"
**Lore Hook:** Bestial prisoners corrupted by rift seepage, their speed amplified into something feral and unreadable.
**Skills:**
- Auto: Stone throw (weak, 8m range)
- Skill 1: Dart Volley | 0.7s tell → 3 projectiles in 45-degree cone, each applies 2s Wounded stack | CD: 4s
- Skill 2: Back-Hop | Instant reaction → Repositions 3m away when player closes to melee range | CD: 3s
**VFX (engine-side only):** Dark-matter trail on darts (particle streaks); Back-Hop uses existing micro-knockback particle burst in reverse direction; Wounded status overlay via existing shader system.
**Best vs:** Brawler (needs closing distance to stack Cracked/Shattered; Satyr invalidates that), Warblade (melee commit punished by Back-Hop).
**Spawn Combos:** Pair with Shard Walkers — Walkers occupy player attention at melee range while Satyr applies Wounded from flanks.
**Forbidden Spawns:** No hard restrictions.
**Acts:** Act 3
**Inspired by:** Hades — Satyr Cultist (agile poison-dart archetype); adapted with repositioning Back-Hop skill and Wounded status alignment.

---

### [M07] — Rift-Core Hulk
**Role:** MiniBoss — arena-wide corruption pressure
**Threat:** 8pt | **Size:** 100px
**Habit Broken:** "Can I greed my DPS window or must I sacrifice it for safe positioning?"
**Lore Hook:** The beating heart of the prison's collapse: a massive construct whose rift-core irradiates everything around it as it destabilizes.
**Skills:**
- Auto: Heavy smash (slow, short range, high damage)
- Skill 1: Corruption Wave | 1.5s tell → Arena-wide radial wave (jumpable via movement at correct frame); corrupts edge sectors progressively | CD: 10s
- Skill 2: Rift Summon | 1.2s tell → Spawns 4 Rift-Imps (1pt each) from cracks in floor | CD: 15s
- Skill 3: Core Burst | 1.4s tell → Activates shield threshold; if not burst through in 3s, arena-wide explosion (heavy damage) | CD: 20s
**VFX (engine-side only):** Screen-shake (engine-side) on Corruption Wave; heavy bloom shader on body; corrupted sectors get persistent red ground-tint particle overlay; Core Burst charges with expanding ring particle + escalating audio cue; Posture meter UI displays above mob.
**Best vs:** Ravager (high-risk low-life play cannot afford arena ticks), Hexer (Hex stacking requires time; Corruption Wave interrupts accumulation rhythm).
**Spawn Combos:** Pair with Rift-Eye Stalkers — Stalkers blink to player back during Corruption Wave dodging.
**Forbidden Spawns:** Never in rooms smaller than 16x12 tiles — Corruption Wave requires navigable space to be fair.
**Acts:** Act 3
**Inspired by:** Last Epoch — Aberroth boss (arena-wide corruption with safe-gap mechanic, shield-threshold interrupt); scaled down to an elite MiniBoss tier with added summon phase.

---

### [M08] — Stone-Splitter
**Role:** Elite Grunt — death-split pressure
**Threat:** 5pt | **Size:** 55px
**Habit Broken:** "Is this enemy dead when its HP reaches zero?"
**Lore Hook:** Their essence is bound to the architecture of the Shattered Keep — destroying them only releases two smaller, angrier pieces.
**Skills:**
- Auto: Heavy mace swing (slow)
- Skill 1: Ground Slam | 1.1s tell → 3m radius knockback + 15 dmg | CD: 7s
- Skill 2: Divide | 0s (on death) → Splits into two Split-Shards (Grunt 2pt each, reduced HP) | CD: N/A (death trigger)
**VFX (engine-side only):** Rock-debris particle burst on death-split; crack shader applied progressively as HP decreases (25% increments); Split-Shards spawn from impact point with scatter particle vectors.
**Best vs:** Elementalist (burst-and-move habit breaks against split trigger), Ronin (Iaido kill-shot timing miscalculates against split).
**Spawn Combos:** Pair with Fracture Imps — Splitter draws focus, death-split adds two more grunts to existing swarm mid-fight.
**Forbidden Spawns:** No hard restrictions.
**Acts:** Act 2
**Inspired by:** Hades — Splitter enemy (death-split mechanic); adapted with a setup Ground Slam skill and visual HP-crack shader to telegraph incoming split.

---

### [M09] — Gale Warden
**Role:** Skill-silence zone anchor
**Threat:** 3pt | **Size:** 50px
**Habit Broken:** "Can I always use my skills on demand inside melee range?"
**Lore Hook:** A prison warden whose chains now crackle with stolen rift energy, dampening the flow of power in those who approach.
**Skills:**
- Auto: Chain whip (moderate range)
- Skill 1: Dampening Aura | Passive → 3m radius zone; player skills within zone have +1.5s added cooldown | CD: Passive (always active)
- Skill 2: Static Lash | 0.9s tell → 5m chain-whip arc; applies 2s Broken on hit | CD: 6s
**VFX (engine-side only):** Continuous faint blue particle ring at 3m radius to mark aura edge; player skill-icon UI tint shifts to grey when inside radius (UI shader); Static Lash emits electric particle arc.
**Best vs:** Hexer (Hex stacking requires rapid-fire skills; aura halts accumulation), Brawler (juggle combo requires skill spam; aura breaks cadence).
**Spawn Combos:** Pair with Stone-Splitter — player cannot use skills to kite Splitter before split while inside Gale Warden aura.
**Forbidden Spawns:** Cannot spawn with Chain Warden (two chain-type enemies create overlapping identity; design confusion).
**Acts:** Act 2
**Inspired by:** Risk of Rain 2 — Malachite Elite (suppresses player resource — in RoR2 healing, in RIMA skill cooldowns); adapted to a passive aura that punishes standing inside melee range.

---

### [M10] — Effigy of Ruin
**Role:** Reflect zone / damage-conduit anchor
**Threat:** 5pt | **Size:** 70px
**Habit Broken:** "Do I prioritize killing this immobile mob while enemies redirect damage back at me?"
**Lore Hook:** An ancient Ossuary statue that absorbed the sins of centuries of prisoners, now actively redirecting harm inflicted near it back onto attackers.
**Skills:**
- Auto: None — stationary
- Skill 1: Effigy Aura | Passive → Reflects 30% of damage dealt to any mob within 6m back to the attacker | CD: Passive
- Skill 2: Soul Pulse | 1.2s tell → Heals all mobs within 6m for 10% max HP | CD: 8s
**VFX (engine-side only):** Ghostly spectral shader on statue body; reflected damage triggers reversed-direction white flash from mob back toward player; Soul Pulse emits green mote particles from statue to each mob; aura radius shown as subtle ground-glow ring.
**Best vs:** Ravager (damage-suicide loop under reflect kills Ravager's HP-trade mechanic), Hexer (cannot safely stack Hex while reflect is active against dense packs).
**Spawn Combos:** Pair with any Act 2 Grunt cluster — reflect makes the cluster effectively self-defending.
**Forbidden Spawns:** Cannot spawn with Mirror-Guard (two reflect-type mechanics in same room creates untestable reflect stacking interactions).
**Acts:** Act 2
**Inspired by:** Path of Exile — Effigy Archnemesis modifier (damage reflection); adapted to a stationary totemic unit with an added heal skill to make the kill-priority decision more explicit.

---

## Part 4: Cross-Game Design Lessons

**Last Epoch** teaches RIMA to value arena-wide positional pressure. Aberroth's mechanic — a progressive corruption that covers the arena while leaving only small navigable gaps — reframes the environment as an active combatant. RIMA should use this for MiniBoss design: the room layout is not neutral backdrop but a tool the boss leverages. The Rift-Core Hulk (M07) directly inherits this principle, with sector-by-sector corruption forcing movement discipline even during otherwise favorable DPS windows.

**Dead Cells** demonstrates the value of "frame-traps" — mobs that do not merely deal damage but punish incorrect timing through specific, readable tells. The Shieldbearer's frontal immunity and the Dark Tracker's back-crit are elegant because they require a setup action, not a stat check. RIMA should adopt this for every Elite and above: each should have a discrete window where attacking is wrong, teaching players to identify that window before committing. Mirror-Guard (M04) applies this directly.

**Hades** shows that biome escalation is most effective when later enemies counter the strategies the player learned earlier, not just amplify existing threats. Elysium's Strongbow Exalted regenerates off-screen — punishing players who learned in Tartarus to focus visible threats. RIMA Act 2-3 enemies should explicitly reference Act 1 habits and invert them. Stone-Splitter (M08) targets the Iaido/burst habits built against the Hollow Hulk.

**Risk of Rain 2** provides the most efficient design template: the modifier overlay system. By applying a small number of archetypal modifiers (spatial threat, temporal threat, vision obstruction, healing denial) across existing mob types, RoR2 generates near-infinite variety with minimal asset cost. RIMA can replicate this via Elite-tier versions of existing mobs with one behavioral modifier added — the Gale Warden (M09) is essentially a "Malachite Elite" version of Act 1's Chain Warden concept.

**Path of Exile** excels at creating hierarchical threat through tactical zones. The Effigy, Benevolent Guardian, and Temporal Bubble modifiers all create a radius within which player behavior must change. This teaches RIMA that enemies should not just react to the player — they should actively reshape the room geometry through invisible tactical zones. Effigy of Ruin (M10) applies this as a stationary totem that converts the surrounding space into a damage-hazard zone.

**Synthesis:** The unifying principle across all five games is **Information-Restriction Balance**. Every effective enemy provides clear telegraphed information (a visible tell, an audio cue, a particle ring marking an aura) paired with a meaningful restriction (a threat zone, a punish window, a forbidden angle of attack). The player always has the information needed to succeed; the challenge is executing the correct response in the available time. When a mob's tell is opaque or its restriction is arbitrary, it fails as design regardless of its mechanical novelty. RIMA's mob roster for Act 2-3 should be audited against this lens: if the player cannot identify what the mob is asking within 2 seconds of first encounter, the tell needs revision — not the mechanic.

---

*Research sources: Last Epoch Wiki (lastepoch.fandom.com), Dead Cells Wiki (deadcells.wiki.gg), Hades Wiki (hades.wiki.fextralife.com), Parry Everything — RoR2 Elite Analysis (parryeverything.com), Expert Game Reviews — PoE Archnemesis Modifiers (expertgamereviews.com), Maxroll Last Epoch Monolith Guide (maxroll.gg)*
