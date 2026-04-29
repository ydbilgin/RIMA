# SINIF SKILL HAVUZU — FİNAL TASARIM
*2026-03-28 12:14 | 8 Sınıf × 12 Skill + Tag Sistemi*

---

## BÖLÜM 1 — 12→4 SEÇİM MİMARİSİ: Hangi Sistem Bu Oyuna Uyar?

### SYSTEM NAME: **"Dynamic Synergy Selection" (DSS)**

### RULE SET:
1. **Starting Skills**: At the start of each run, players receive 2 core skills per class (totaling 16), chosen to highlight each class's unique mechanics without overwhelming complexity.
2. **Room Offers**: Each room presents 3 skill offers, weighted as follows: 1 from the player's primary class, 1 from a secondary, and 1 neutral to encourage diversity.
3. **Progression Bias**: Early rooms offer simpler, foundational skills, while later rooms introduce more complex, synergistic options to build upon initial choices.
4. **Skill Management**: When slots are full (6 active), players can either auto-upgrade the weakest skill or replace it with a new one, ensuring continuous improvement.
5. **Tag Integration**: The UI displays skill tags as icons, highlighting potential synergies. Hover tooltips explain tag interactions without requiring prior knowledge.
6. **Synergy Prevention**: The system ensures at least two recommended synergistic skills are present in each room's offers, guiding players toward balanced builds.

### ROOM OFFER WEIGHTS:
| Room Offer Position | Primary Class Skill | Secondary Class Skill | Neutral Skill |
|---------------------|--------------------|-----------------------|--------------|
| 1                   | 1                  | 0                     | 0            |
| 2                   | 0                  | 1                     | 0            |
| 3                   | 0                  | 0                     | 1            |

### UI FLOW:
1. **Run Start**: Players receive 2 core skills per class, displayed in a compact grid with tags highlighted.
2. **Room Entry**: Three skill offers appear, each with their tags visible. A tooltip shows synergy potential.
3. **Skill Selection**: Player picks one skill. If slots are full, a prompt offers auto-upgrade or replacement.
4. **Skill Addition**: Selected skills are added to the active list, with passive and ultimate skills automatically activated.

### WHY THIS WORKS:
- **Balanced Discovery**: Combines structured discovery with choice, preventing RNG-induced bad runs.
- **Encourages Diversity**: Room offers push players toward diverse builds, enhancing replayability.
- **Contextual Synergy**: UI tooltips guide without overwhelming, ensuring meaningful decisions.

---

## BÖLÜM 2 — TAG SİSTEMİ: Nasıl Çalışır, Nasıl Görünür?

### FINAL TAG COUNT AND DEFINITIONS:
- **Core Tags (6):** ANCHOR, OPENER, CHAIN, BUILDER, SPENDER, FINISHER
- **Advanced Tags (2):** CONTROL, AMPLIFIER (discovered through gameplay)

### TAG UI DESIGN:
- **Color Coding:**
  - ANCHOR: Red
  - OPENER: Blue
  - CHAIN: Green
  - BUILDER: Yellow
  - SPENDER: Purple
  - FINISHER: Black
  - CONTROL: White (discovered)
  - AMPLIFIER: Gold (discovered)
- **Icons and Tooltips:** Each tag has an icon and tooltips explaining effects. Advanced interactions are revealed mid-run.

### IDEAL 4-SKILL TAG DISTRIBUTIONS:

| Class         | Tag Distribution                          |
|---------------|-------------------------------------------|
| **Warblade**  | [CONTROL, BUILDER, OPENER, FINISHER]    |
| **Elementalist** | [BUILDER, CHAIN, AMPLIFIER, SPENDER]   |
| **Rogue**     | [CHAIN, AMPLIFIER, OPENER, FINISHER]    |
| **Ranger**    | [CONTROL, BUILDER, SPENDER, ANCHOR]     |
| **Brawler**   | [BUILDER, SPENDER, CHAIN, ANCHOR]        |
| **Paladin**   | [BUILDER, SPENDER, OPENER, AMPLIFIER]   |
| **Summoner**  | [BUILDER, SPENDER, CHAIN, ANCHOR]        |
| **Hexer**     | [BUILDER, CHAIN, SPENDER, FINISHER]      |

### DISCOVERY TAGS:
- **Hidden Interactions:**
  - Cross-class chains (e.g., Warblade's CONTROL + Rogue's CHAIN)
  - Advanced AMPLIFIER effects
  - Rare synergies between passive and active skills
- **Reveal Mechanic:** Discover through using specific skill combinations or completing challenges, enhancing gameplay depth.

---

## BÖLÜM 3 — WARBLADE + ELEMENTALİST: 12'ye Genişletme

### Warblade — NEW SKILLS (4)
| # | Name | Tags | Effect | Chain Bonus | DNA |
|---|------|------|--------|-------------|-----|
| 9 | Defensive Leap | ANCHOR | 8m dash to reposition, Rage+10, grants 2s invulnerability | - | WoW Warrior's Defensive Stance, LoL Master Yi's Alpha |
| 10 | Bloodletting | BUILDER | Deals minor bleed damage over time, grants +5 Rage per bleed tick | - | WoW Blood DK, LoL Tryndamere |
| 11 | Armor Shredder | CONTROL | Reduces target's armor by 20% for 6s, chain: Mortal Strike→30% reduction | - | WoW Warrior, LoL Nasus |
| 12 | Magic Ward | AMPLIFIER | Grants 50% magic resistance for 4s, reduces all incoming spell damage by 50% | - | WoW Paladin, LoL Cho'Gath |

### Warblade — FULL 12 SKILL TABLE WITH TAGS
| # | Name | Tags | One-line summary |
|---|------|------|-----------------|
| 1 | Charge | OPENER CONTROL | 8m dash+stun, Rage+20 |
| 2 | Mortal Strike | CHAIN FINISHER | big dmg+heal reduction, chain: Charge→100% reduction |
| 3 | Colossus Smash | AMPLIFIER | 6s window: all dmg sources +30% |
| 4 | Whirlwind | ANCHOR SPENDER | 2s spin AoE, consumes Rage |
| 5 | Shield Slam | CONTROL | 100% knockback, wall=stun, Rage-20 |
| 6 | Execute | FINISHER | HP<30%: 400% dmg, chain: Mortal Strike→600% |
| 7 | Hamstring | CONTROL OPENER | 50% slow+bleed, chain: Charge→3s stun |
| 8 | War Stomp | CONTROL BUILDER | 3m knockup 2s, Rage+25 |
| 9 | Defensive Leap | ANCHOR | 8m dash to reposition, Rage+10, grants 2s invulnerability |
| 10 | Bloodletting | BUILDER | Deals minor bleed damage over time, grants +5 Rage per bleed tick |
| 11 | Armor Shredder | CONTROL | Reduces target's armor by 20% for 6s, chain: Mortal Strike→30% reduction |
| 12 | Magic Ward | AMPLIFIER | Grants 50% magic resistance for 4s, reduces all incoming spell damage by 50% |

### Warblade — BUILD AXES (3 viable 4-skill combos)
**Build A "Defensive Tank":** Charge + Shield Slam + War Stomp + Defensive Leap — This build focuses on high survivability and crowd control, using Defensive Leap to reposition and avoid damage while maintaining a strong defensive presence.

**Build B "Bleed and Armor Shredder":** Mortal Strike + Hamstring + Bloodletting + Armor Shredder — A build that focuses on sustained damage over time through bleed effects and armor reduction, maximizing damage output over multiple engagements.

**Build C "Execute Finisher":** Execute + Colossus Smash + War Stomp + Defensive Leap — A high-risk, high-reward build that relies on maintaining high health to unleash powerful finishers, using Defensive Leap to reset the cycle.

---

### Elementalist — NEW SKILLS (4)
| # | Name | Tags | Effect | Chain Bonus | DNA |
|---|------|------|--------|-------------|-----|
| 9 | Thunderstorm | BUILDER | Summons a storm that deals lightning damage over time, grants +10 Mana per tick | - | WoW Shaman, LoL Blitzcrank |
| 10 | Ice Shield | ANCHOR | Creates a shield absorbing 50% of incoming damage, lasts 5s | - | WoW Frost DK, LoL Annie |
| 11 | Arcane Pulse | ANCHOR | Instant AoE spell, deals moderate damage and reduces all enemy movement speed by 30% for 4s | - | WoW Mage, LoL Lux |
| 12 | Frost Nova | CONTROL | Freezes all enemies in a 5m radius, chain: Blink→instantly unfreeze self | - | WoW Mage, LoL Morgana |

### Elementalist — FULL 12 SKILL TABLE WITH TAGS
| # | Name | Tags | One-line summary |
|---|------|------|-----------------|
| 1 | Fireball | BUILDER OPENER | fire DoT, Fire State builder |
| 2 | Frostbolt | CONTROL BUILDER | slow+Mana regen, Brain Freeze proc |
| 3 | Living Bomb | CHAIN SPENDER | 5s delayed explosion, spreads on kill |
| 4 | Blink | OPENER ANCHOR | 6m instant teleport, next spell+20% dmg |
| 5 | Frozen Orb | CONTROL ANCHOR | slow-moving orb, chills path |
| 6 | Arcane Blast | BUILDER SPENDER | escalating stack, 4th cast→Barrage |
| 7 | Meteor | FINISHER CONTROL | 1.5s channel, big AoE, knockdown |
| 8 | Mirror Image | ANCHOR | 2 copies, absorbs 1 hit each, death explosion |
| 9 | Thunderstorm | BUILDER | Summons a storm that deals lightning damage over time, grants +10 Mana per tick |
| 10 | Ice Shield | ANCHOR | Creates a shield absorbing 50% of incoming damage, lasts 5s |
| 11 | Arcane Pulse | ANCHOR | Instant AoE spell, deals moderate damage and reduces all enemy movement speed by 30% for 4s |
| 12 | Frost Nova | CONTROL | Freezes all enemies in a 5m radius, chain: Blink→instantly unfreeze self |

### Elementalist — BUILD AXES (3 viable 4-skill combos)
**Build A "Ice and Shield":** Frostbolt + Blink + Ice Shield + Frozen Orb — A defensive build that focuses on crowd control and survivability, using Blink to reposition and Ice Shield to mitigate damage.

**Build B "Lightning and AoE":** Thunderstorm + Blink + Arcane Pulse + Living Bomb — A high-damage build that combines sustained lightning damage with area-of-effect spells, maximizing damage output through Blink's mobility.

**Build C "Meteor Barrage":** Arcane Blast + Blink + Meteor + Mirror Image — A high-risk, high-reward build that focuses on massive single-target damage and area-of-effect barrages, using Mirror Image to extend survival and damage potential.

---

## BÖLÜM 4 — ROGUE + RANGER: 12'ye Genişletme

### ROGUE (Expanded to 12 Skills)

#### New Skills:
1. **Poisoned Blade** [ANCHOR][BUILDER]
   - **Description:** Attacks apply poison DoT for 10s, stacks up to 3 times. Each stack increases damage by 10% and reduces healing received by 5%.
   - **Gap Filler:** Introduces a distinct poison mechanic separate from bleed, adding a new layer of damage-over-time and debuffing enemy healing.

2. **Smoke Bomb** [CONTROL][OPENER]
   - **Description:** Throws a smoke bomb, creating a cloud that reduces enemy vision range by 50% for 5s. Grants stealth for 3s.
   - **Gap Filler:** Provides directional burst movement and escape mechanism, allowing the Rogue to reposition quickly in combat.

3. **Preparation** [ANCHOR][CONTROL]
   - **Description:** Instantly resets the cooldown of all active skills. Grants 2 combo points.
   - **Gap Filler:** Offers a reset-all-cooldown utility, similar to the iconic "Preparation" ability from World of Warcraft.

4. **Feint** [ANCHOR][BUILDER]
   - **Description:** Feints an attack, reducing the next incoming damage by 50% and granting 2 combo points. Can be used once every 30s.
   - **Gap Filler:** Provides a panic button skill for when the Rogue is cornered or surrounded, allowing them to survive and reposition.

### RANGER (Expanded to 12 Skills)

#### New Skills:
1. **Rapid Fire** [ANCHOR][CHAIN]
   - **Description:** Enters a rapid-fire state for 5s, firing arrows at a rate of 2 per second. Focus cost: 50%, reduced damage if Focus is below 75%.
   - **Gap Filler:** Introduces a sustained rapid-fire attack, allowing the Ranger to deal continuous damage in close range.

2. **Hawk Companion** [ANCHOR][CONTROL]
   - **Description:** Summons a hawk that attacks enemies within 10m, dealing poison damage and reducing their movement speed by 30%. Lasts 30s.
   - **Gap Filler:** Provides a persistent pet companion that can assist in combat and apply additional debuffs.

3. **Backlash** [ANCHOR][BUILDER]
   - **Description:** When hit by an enemy attack, the Ranger counters with a backstab that deals 150% damage. Grants 2 combo points if successful.
   - **Gap Filler:** Offers a close-range desperation skill, allowing the Ranger to turn the tide when cornered.

4. **Hunter's Eye** [ANCHOR][CONTROL]
   - **Description:** Reveals enemy positions within 30m for 15s, granting improved vision and detection. Increases critical hit chance by 10%.
   - **Gap Filler:** Provides tactical information and utility, enhancing the Ranger's situational awareness and combat effectiveness.

### Skill Structure Overview

**Rogue:**
- **Anchor:** 4
- **Opener:** 3
- **Chain:** 2
- **Builder:** 3
- **Spender:** 2
- **Finisher:** 3
- **Control:** 3

**Ranger:**
- **Anchor:** 4
- **Opener:** 3
- **Chain:** 2
- **Builder:** 3
- **Spender:** 2
- **Finisher:** 3
- **Control:** 3

These new skills expand the Rogue and Ranger classes, addressing the identified gaps and providing players with more diverse and powerful build options.

---

## BÖLÜM 5 — BRAWLER + PALADİN: 12'ye Genişletme

### BRAWLER (Expanded to 12 Skills)

| Skill Name           | Tags                | Description                                                                 |
|----------------------|---------------------|----------------------------------------------------------------------------|
| Bloodlust Strike     | CHAIN, FINISHER     | HP-scaled damage, chain: Fury 80% → Slaughter unlocks.                    |
| Whirlwind            | ANCHOR, SPENDER     | 2s spin attack, each hit reduces defense (max -30%).                       |
| Frenzied Leap        | OPENER, BUILDER     | Jump to target, resets cooldown on hit, 3x consecutive → Frenzy 5s.         |
| Reckless Swing       | FINISHER, SPENDER   | Massive single hit, 2s vulnerable window.                                  |
| Bloodthirst          | ANCHOR, BUILDER     | 5 fast hits, each heals, more heal at low HP.                              |
| Intimidating Shout   | CONTROL             | 3m panic for 3s, chain: panicked enemy hit → backstab counts.               |
| Barbaric Charge      | OPENER, CONTROL     | CC-immune dash, stuns enemies in path.                                      |
| Last Rites           | FINISHER            | 600% damage when HP < 15%, 4s vulnerable after.                            |
| **New Skill 1**      | **ANCHOR, BUILDER**| **Fury Surge** — Instantly fills Fury to 50%, then gain +20 Fury per hit.   |
| **New Skill 2**      | **CHAIN, CONTROL**  | **Rage Induction** — Grab and throw enemy, chain: thrown enemy takes 2x dmg.|
| **New Skill 3**      | **ANCHOR, SPENDER** | **Armor Shatter** — 100% damage through armor, chain: next attack ignores armor.|
| **New Skill 4**      | **FINISHER, CONTROL| **Battle Hardened** — 2s invulnerability, then counterattack with +50% damage.|

### PALADIN (Expanded to 12 Skills)

| Skill Name                | Tags                 | Description                                                                 |
|---------------------------|----------------------|----------------------------------------------------------------------------|
| Crusader Strike           | BUILDER, OPENER      | Basic melee attack, chain: 3-strike pattern → +60% damage.                 |
| Divine Storm              | BUILDER, AMPLIFIER   | 360° melee AoE, HP+15/target (3+ targets = HP+50).                         |
| Judgment                  | BUILDER, CHAIN       | Ranged holy blast, debuffed enemy = +50% damage.                          |
| Consecration              | BUILDER, ANCHOR      | Sacred ground 5s, tick damage + HP+5/s while standing.                     |
| Hammer of Wrath           | FINISHER, CHAIN      | HP < 20% targets only, HP+30, chain: Execute (Warblade).                   |
| Avenger's Shield          | CONTROL, BUILDER     | Bouncing shield, 3 targets, each = silence + HP+15.                         |
| Holy Shock                | ANCHOR, BUILDER      | Dual use: enemy = damage + HP+15 / self = heal (×3 if HP < 30%).            |
| Shield of Retribution     | CHAIN, SPENDER       | 3s block, blocked damage released as AoE (Consecration = 2×).              |
| **New Skill 1**           | **ANCHOR, BUILDER**  | **Holy Aura** — 5s aura around self, +10% damage and +10% healing.         |
| **New Skill 2**           | **ANCHOR, SPENDER**  | **Divine Shield** — Instant large heal, chain: next attack heals for 20%.   |
| **New Skill 3**           | **CONTROL, BUILDER** | **Repel** — Pushes all enemies back, chain: pushed enemies take 2x damage.  |
| **New Skill 4**           | **ANCHOR, CONTROL**  | **Turn Undead** — Instantly kills undead minions, chain: next attack stuns.|

### Build Axes
- **BRAWLER**
  - **Fury Surge** + **Rage Induction** + **Armor Shatter** + **Battle Hardened**: Focus on rapid Fury generation and control mechanics.
  - **Frenzied Leap** + **Intimidating Shout** + **Bloodthirst** + **Last Rites**: Emphasizes crowd control and high-risk, high-reward damage output.
  
- **PALADIN**
  - **Crusader Strike** + **Judgment** + **Holy Shock** + **Turn Undead**: Balanced damage and healing, with strong anti-undead utility.
  - **Divine Storm** + **Consecration** + **Avenger's Shield** + **Repel**: Area control and sustained damage, with crowd management options.

These expansions aim to fill the identified gaps while maintaining the thematic and mechanical integrity of each class.

---

## BÖLÜM 6 — SUMMONER + HEXER: 12'ye Genişletme

### SUMMONER (Expanded to 12 Active Skills)

| Skill Name                | Tags                         | Description                                                                 |
|---------------------------|------------------------------|-----------------------------------------------------------------------------|
| **Raise Skeleton**        | BUILDER, OPENER             | 1Charge→melee skeleton (max 3), 3 together→Rally+40%                        |
| **Summon Golem**          | ANCHOR, CONTROL             | 2Charges→1 big Golem, blocks path, HP<20%=self-explodes                     |
| **Rally Cry**             | AMPLIFIER                   | all minions +20% dmg+speed 10s (mixed types=+40%)                          |
| **Corpse Explosion**      | FINISHER, CHAIN             | detonate corpse→AoE, 3+ corpses=chain reaction                              |
| **Death Nova**            | CHAIN, CONTROL              | sacrifice 1 minion→8s poison cloud (Hexer dual=spread debuffs)              |
| **Commanding Strike**     | AMPLIFIER, CONTROL          | order minion: 4× dmg attack+invuln (no minion=Summoner self-hits 2×)       |
| **Blood for Power**       | BUILDER, SPENDER            | sacrifice minion→Charge+1+CD-30% all skills                                |
| **Bone Shield**           | ANCHOR, BUILDER             | 3s absorption using minion as shield, absorbed dmg=Charge+1                 |
| **Raise Archer**          | BUILDER, OPENER             | 1Charge→ranged skeleton (max 2), 2 together→Rally+40%                       |
| **Soul Pact**             | ANCHOR, CONTROL             | Sacrifice all minions for 3 Charges and a 5s invulnerability shield         |
| **Lich Form**             | TRANSFORMATION, CONTROL     | Summoner enters melee form, +100% dmg for 15s, -50% HP, no skill cooldowns |
| **Dark Pact**             | BUILDER, SPENDER            | Sacrifice 20% HP→Charge+2, 30% HP→Charge+3, 50% HP→Charge+4                |

**Build Axes:**
- **Sacrifice for Power:** Focus on skills that consume minions for Charges and buffs.
- **Minion Control:** Use skills to control the battlefield with minions.
- **Emergency Sustain:** Use Soul Pact and Lich Form for temporary invulnerability and damage boosts.
- **Resource Management:** Manage Charges efficiently to avoid being helpless without minions.

**Note on Tempo Fix:**
The new skills, especially Raise Archer, Soul Pact, Lich Form, and Dark Pact, provide the Summoner with tactical diversity and emergency sustain options. These skills help the Summoner adapt to different combat scenarios and maintain a strong presence even when minions are low or dead.

### HEXER (Expanded to 12 Active Skills)

| Skill Name                | Tags                         | Description                                                                 |
|---------------------------|------------------------------|-----------------------------------------------------------------------------|
| **Corruption**            | OPENER, BUILDER             | instant 3 stacks, 4s moderate DoT                                              |
| **Agony**                 | BUILDER                     | slow-applying DoT, 2 stacks/tick (continuous)                                 |
| **Pandemic**              | AMPLIFIER, CHAIN            | copy ALL stacks from one target to all nearby                                 |
| **Hexblast**              | FINISHER, SPENDER           | 10-stack detonation, CD resets on kill, chain-explodes                        |
| **Empathy**               | CHAIN, CONTROL              | curse: enemy attacks return 30% dmg to self                                   |
| **Haunt**                 | BUILDER, CONTROL            | attach ghost: follows+ticks+3stacks, ends at 10 stacks auto-Hexblast         |
| **Unstable Affliction**   | FINISHER, CHAIN             | if dispelled/healed→instant explode (stun). Anti-heal skill                  |
| **Enervate**              | CONTROL, AMPLIFIER          | -50% move/-40% atk speed 10s, 5+stacks=duration×2                            |
| **Mass Curse**            | BUILDER, CONTROL            | Apply 2 stacks to all enemies in a large AoE                                  |
| **Silence**               | CONTROL                     | Silences target enemy for 5s, interrupting any cast or channel                |
| **Shadow Ward**           | ANCHOR, CONTROL             | Gain a protective ward that absorbs 50% of damage for 10s                    |
| **Soul Bargain**          | BUILDER, SPENDER            | Sacrifice 20% HP→5 stacks, 30% HP→7 stacks, 50% HP→10 stacks                 |

**Build Axes:**
- **Mass Hexing:** Focus on skills that apply stacks to multiple enemies.
- **Control:** Use skills to control enemy movement and actions.
- **Self-Protection:** Utilize Shadow Ward for temporary self-protection.
- **Resource Management:** Manage stack count and phase transitions efficiently.

**Note on Tempo Fix:**
The new skills, particularly Mass Curse, Silence, Shadow Ward, and Soul Bargain, provide the Hexer with the ability to control large groups of enemies and protect themselves in critical moments. These skills help the Hexer maintain tempo and adapt to different combat scenarios, addressing the tempo issues in short roguelite fights.

---

## BÖLÜM 7 — CROSS-CLASS SINERJILER: 12 Havuzunda Yeni Etkileşimler

### Top 10 Cross-Class Synergies

| **Rank** | **Combo Name**                  | **Classes**          | **Skills Involved**                          | **What Happens**                                                                 | **Why It's "Insane"**                                                                 | **Build Risk**                                                                 | **Rating** |
|----------|---------------------------------|----------------------|----------------------------------------------|----------------------------------------------------------------------------------|---------------------------------------------------------------------------------------|-------------------------------------------------------------------------------|------------|
| **1**    | **Hex and Leap**               | Warblade + Hexer     | Heroic Leap (Warblade), Hexblast (Hexer)      | Hexblast hits all enemies in the area, triggering Hexer's Mass Hex if any enemy is above 7 hex stacks. | Hexblast combined with Mass Hex creates a massive AOE Hex application, dealing huge damage and crowd control. | Requires precise timing and positioning.                                       | S          |
| **2**    | **Blazing Leap**               | Warblade + Elementalist | Heroic Leap (Warblade), Combustion (Elementalist) | Combustion applies a DoT to all enemies hit by Heroic Leap, triggering Chain Lightning if any enemy is above 75% HP. | Combines leap with massive AOE fire damage and chain lightning for explosive damage. | Requires managing mana and Combustion timing.                                   | S          |
| **3**    | **Hex and Sprint**             | Rogue + Hexer        | Deadly Poison (Rogue), Mass Hex (Hexer)        | Mass Hex applies to all enemies, triggering Deadly Poison's stacking effect for instant poison proc. | Creates a powerful poison DoT chain across all enemies, with immediate damage procs. | Requires positioning and poison application.                                    | S          |
| **4**    | **Frost Nova Reckoning**       | Brawler + Elementalist | War Cry (Brawler), Blizzard (Elementalist)     | War Cry enrages enemies, triggering Blizzard's AOE frostnova, freezing all enemies in place. | Enrages enemies into a cluster, then freezes them for massive damage and control. | Managing enrage timing and resource management.                                 | A          |
| **5**    | **Divine Hex Cleansing**       | Paladin + Hexer      | Lay on Hands (Paladin), Mass Hex (Hexer)       | Lay on Hands cleanses all hexes, triggering a healing wave that also applies Hex to enemies. | Creates a cycle of cleansing and hexing, leading to massive damage and sustain.     | High risk due to resource dependency and timing.                               | A          |
| **6**    | **Hex Wolf Pack**              | Ranger + Hexer       | Spirit Wolf (Ranger), Mass Hex (Hexer)         | Spirit Wolf summons a wolf, triggering Mass Hex on all enemies in the area. | Wolf synergizes with Mass Hex for AOE hex application and damage amplification.   | Requires wolf summoning and hex application timing.                            | B          |
| **7**    | **Hex and Raise**              | Summoner + Hexer     | Raise Archer (Summoner), Mass Hex (Hexer)       | Raise Archer summons archers, triggering Mass Hex on all enemies in the area. | Archers attack while Hexed enemies take massive damage from the hex application.   | Requires minion management and hex timing.                                     | B          |
| **8**    | **Hex and Rally Cry**          | Warblade + Hexer     | Rallying Cry (Warblade), Mass Hex (Hexer)       | Rallying Cry buffs allies, triggering Mass Hex on all enemies in the area. | Buffs allies while hexing enemies for massive damage and crowd control.            | Requires positioning and rally cry timing.                                     | B          |
| **9**    | **Hex and Sprint**             | Rogue + Hexer        | Sprint (Rogue), Mass Hex (Hexer)               | Sprint allows the Rogue to reposition, triggering Mass Hex on all enemies in the area. | Creates a powerful hit-and-run strategy with AOE hex application.                 | Requires precise positioning and timing.                                       | B          |
| **10**   | **Hex and Devotion**           | Paladin + Hexer      | Devotion Aura (Paladin), Mass Hex (Hexer)       | Devotion Aura buffs allies, triggering Mass Hex on all enemies in the area. | Buffs allies while hexing enemies for massive damage and crowd control.            | Requires positioning and aura timing.                                           | B          |

---

### Hidden and Broken Synergies

1. **Hidden Surprise Combo**: 
   - **Hexer + Summoner**: Soul Bargain (Hexer) + Raise Archer (Summoner). 
     - **What Happens**: Raise Archer summons archers, which can be targeted by Soul Bargain for massive damage.
     - **Why It's Hidden**: Requires using both classes' unique mechanics (minion summoning and hex manipulation) for a powerful synergy.

2. **Broken Synergy**:
   - **Hexer + Hexer**: Cursed Mirror (Hexer) + Mass Hex (Hexer).
     - **What Happens**: Reflects all damage back to the user, triggering Mass Hex on all enemies.
     - **Why It's Broken**: Creates an infinite loop of damage amplification and hex application, making it overpowered.

---

### Best Plague Doctor Combo (Summoner + Hexer)

- **Combo Name**: **Hex and Raise**
  - **Classes**: Summoner + Hexer
  - **Skills Involved**: Raise Archer (Summoner), Mass Hex (Hexer)
  - **What Happens**: Raise Archer summons archers, triggering Mass Hex on all enemies in the area.
  - **Why It's Insane**: Creates a powerful minion swarm with AOE hex application for massive damage and crowd control.
  - **Build Risk**: Requires minion management and hex timing.
  - **Rating**: S

---

### Summary

The top cross-class synergies create exciting, impactful moments that reward strategic play and class knowledge. The hidden and broken interactions highlight the need for careful balancing to maintain game integrity while encouraging creative builds.

---

## BÖLÜM 8 — FİNAL SENTEZ: Kesin Sistem Dokümanı

### 1. SYSTEM OVERVIEW

#### Why 12 Skills per Class (vs 8)
Expanding from 8 to 12 skills per class increases the diversity of viable builds, allowing players to explore more nuanced and complex strategies. With 12 skills, there's a broader range of interactions, setups, and finishers available, enhancing the depth of gameplay without overwhelming the player.

#### The Selection Architecture (Hybrid Model)
The hybrid model combines a fixed set of core skills with a dynamic pool of advanced and mastery skills. Core skills are always available, providing a baseline of functionality. Advanced and mastery skills are introduced as the player progresses through the dungeon, offering more complex and powerful options.

#### Tag System Summary
The tag system categorizes skills based on their primary function and interaction with other skills. This helps players quickly understand the role of each skill in their build and plan their strategy accordingly.

### 2. SELECTION FLOW — STEP BY STEP

#### Run Start
- **Initial Offer Pool**: 4 core skills (Anchor, Opener, Builder, Spender) + 2 advanced skills (randomly selected from the remaining pool).
- **Player Choice**: Select 1 skill from the 6 offered.

#### Room-by-Room Progression
- **Room 1-2**: Core skills only.
- **Room 3-4**: Core + 1 advanced skill.
- **Room 5-6**: Core + 2 advanced skills.
- **Room 7-8**: Core + 2 advanced + 1 mastery skill.

#### Offer Weighting
- **Core Skills**: Always available but less frequently offered as the player progresses.
- **Advanced Skills**: Introduced gradually, with a higher frequency in later rooms.
- **Mastery Skills**: Rarely offered, typically in the final few rooms.

### 3. TAG SYSTEM — FINAL DEFINITIONS

| Symbol | Color | Definition | Example Skill (Warblade) |
|--------|-------|------------|--------------------------|
| ⚓ | Blue | Standalone power, no setup needed | **Raging Blow** (Unleashes a powerful strike that ignores enemy armor) |
| → | Green | Best as first in a combo sequence | **Piercing Strike** (Deals physical damage and applies Bleed) |
| ⚡ | Yellow | Gets bonus when used after specific skill | **Savage Strike** (Increases damage based on previous Bleed applications) |
| ↑ | Orange | Generates resource / creates setup | **Battle Cry** (Boosts allies' morale for a short duration) |
| ↓ | Red | Consumes resource / cashes out setup | **Cleave** (Spends Fury to deal massive damage to multiple enemies) |
| 💥 | Purple | Conditional massive damage | **Overpower** (Critical hit chance increases with consecutive successful blocks) |
| ⬡ | Cyan | CC (stun, root, slow, knockback, silence) | **Hammer Shock** (Stuns nearby enemies) |
| ✦ | Grey | Makes other skills more powerful | **Bloodlust** (Increases damage and resource generation for allies) |

### 4. POOL STRUCTURE PER CLASS (Summary Table)

| Class | # | Name | Tags | Type | Key Interaction |
|-------|---|------|------|------|-----------------|
| Warblade | 1 | Raging Blow | ⚓ | Core | Standalone damage |
| Warblade | 2 | Piercing Strike | →, ⚡ | Core | Bleed application |
| Warblade | 3 | Battle Cry | ↑ | Core | Resource generation |
| Warblade | 4 | Cleave | ↓ | Core | Resource consumption |
| Warblade | 5 | Overpower | 💥 | Advanced | Critical hit chance |
| Warblade | 6 | Hammer Shock | ⬡ | Advanced | Crowd control |
| Warblade | 7 | Bloodlust | ✦ | Mastery | Buff amplification |
| Warblade | 8 | Berserker's Rage | ⚓, ↑ | Mastery | Resource generation and damage |
| Warblade | 9 | Overwhelming Blow | ⚓, ↓ | Mastery | Resource consumption and damage |
| Warblade | 10 | Bloodletting | ⚓, ⚡ | Mastery | Damage and resource generation |
| Warblade | 11 | Raging Roar | ⚓, ⬡ | Mastery | Crowd control and damage |
| Warblade | 12 | Frenzy | ⚓, ✦ | Mastery | Buff amplification and damage |

### 5. BUILD DIVERSITY MATH

#### Viable 4-Skill Combos per Class
- **Core Skills**: 4
- **Advanced Skills**: 4 (assuming 8 total advanced skills)
- **Mastery Skills**: 4 (assuming 8 total mastery skills)
- **Total Combinations**: \( \binom{12}{4} = 495 \) combinations

#### Dual-Class Multiplication
- **Class A**: 495 combinations
- **Class B**: 495 combinations
- **Dual-Class Combinations**: \( 495 \times 495 = 245,025 \)

#### Why 12>8 for Replayability
- **Increased Depth**: More complex interactions and strategies.
- **Enhanced Replay Value**: Diverse builds and playstyles.
- **Player Engagement**: Longer learning curve and discovery process.

### 6. IMPLEMENTATION NOTES (Solo Dev, Unity 2D)

#### Minimum Viable Implementation for FAZ 2
- **Core Skills**: Implement all core skills with basic functionality.
- **Advanced Skills**: Introduce a subset of advanced skills.
- **Mastery Skills**: Introduce a limited set of mastery skills.

#### Features to Add Later Without Redesign
- **Tag System Expansion**: Add more tags and refine existing ones.
- **Skill Interactions**: Implement additional skill interactions and combos.
- **Visual Feedback**: Enhance visual effects for skill interactions.

#### Tag System in Unity: ScriptableObject Design Suggestion
- **SkillTag.cs**: Enumerate all tags.
- **Skill.cs**: Use ScriptableObject to define each skill, including tags.
- **SkillSelection.cs**: Manage the logic for offering and selecting skills based on tags.

This document provides a comprehensive framework for expanding the skill pool and enhancing the gameplay experience in "2D Roguelite."

---

