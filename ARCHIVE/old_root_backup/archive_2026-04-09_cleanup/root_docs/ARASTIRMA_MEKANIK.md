# RIMA — Mekanik Araştırma Çıktısı
*Model: deepseek-r1:14b | 2026-04-03 16:16 | 7 bölüm*

---

## BOLUM 01 — Ekonomi ve Ödül Sistemleri (Sandık, Para, Dükkan)

### PART 1 — RAW DATA

#### **Game: Hades (2020)**
- **Currency Types**: 
  - *Souls*: Earned by defeating enemies, completing Contracts, or selling items. Used to purchase items from the Shop or upgrade Contracts in the meta-game.
- **Chest/Loot Chest Types**:
  - Randomly dropped items (common/rare).
  - Contract Chests: Require soul investment for a chance to open.
- **Shop Types**:
  - In-run shop: Purchases items during the current run.
  - Meta-shop: Purchases items with Rubies (meta-currency) between runs.
- **Reward Pacing**: Frequent, with rewards from defeating enemies, completing Contracts, and opening chests.
- **Special Economy Mechanics**:
  - Curse System: Punishes failure with debuffs (e.g., loss of currency or items).
- **Meta-Progression Currencies**:
  - Rubies: Earned by selling items or completing achievements. Used to purchase items in the meta-shop.
  - Heroic Feats: Unlockable achievements that provide permanent buffs.

#### **Game: Hades II (2024)**
- **Currency Types**: 
  - *Souls*: Same as above, but with additional mechanics for inter-run progression.
- **Chest/Loot Chest Types**:
  - Randomly dropped items.
  - Contract Chests: Require soul investment.
- **Shop Types**:
  - In-run shop and meta-shop similar to Hades.
- **Reward Pacing**: Similar to Hades, with additional rewards for completing longer-term objectives.
- **Special Economy Mechanics**:
  - Curse System: Enhanced with more varied penalties/rewards.
- **Meta-Progression Currencies**:
  - Rubies and Heroic Feats as in Hades.

#### **Game: TMNT: Shredder's Revenge (2022)**
- **Currency Types**: 
  - *Cash*: Earned by defeating enemies, completing missions, or breaking objects. Used to purchase items from the Shop.
- **Chest/Loot Chest Types**:
  - Randomly dropped items.
  - Black Market: Requires a large amount of Cash to access rare items.
- **Shop Types**:
  - In-run shop and meta-shop (using unlockables between runs).
- **Reward Pacing**: Frequent, with rewards from defeating enemies and completing missions.
- **Special Economy Mechanics**:
  - No explicit curse system, but economy is competitive.
- **Meta-Progression Currencies**:
  - None explicitly mentioned.

#### **Game: TMNT: Splintered Fate (2024)** 
- **Currency Types**: 
  - *Shells*: Earned by exploring and defeating enemies. Used to purchase items from the Shop.
- **Chest/Loot Chest Types**:
  - Randomly dropped items.
  - Shrines: Offer rare items for a large amount of Shells.
- **Shop Types**:
  - In-run shop and meta-shop.
- **Reward Pacing**: Moderate, with rewards from defeating enemies and exploring.
- **Special Economy Mechanics**:
  - No explicit curse system, but economy is exploration-focused.
- **Meta-Progression Currencies**:
  - None explicitly mentioned.

#### **Game: Dead Cells (2018)**
- **Currency Types**: 
  - *Zombies*: Earned by defeating enemies or selling items. Used to purchase items from the Shop.
- **Chest/Loot Chest Types**:
  - Randomly dropped items.
  - Crafting: Requires materials to create rare items.
- **Shop Types**:
  - In-run shop and meta-shop with reroll costs.
- **Reward Pacing**: Frequent, with rewards from defeating enemies and opening chests.
- **Special Economy Mechanics**:
  - Debt System: Failing to repay debts causes penalties.
- **Meta-Progression Currencies**:
  - Blueprint Shards: Unlock new content.

#### **Game: Enter the Gungeon (2016)**
- **Currency Types**: 
  - *Gold*: Earned by defeating enemies and opening chests. Used to purchase items from the Shop.
- **Chest/Loot Chest Types**:
  - Randomly dropped items.
- **Shop Types**:
  - In-run shop with prices increasing each run.
- **Reward Pacing**: Frequent, with rewards from defeating enemies and opening chests.
- **Special Economy Mechanics**:
  - Gambling System: Use Gold to gamble for rare items.
- **Meta-Progression Currencies**:
  - None explicitly mentioned.

#### **Game: Returnal (2021)**
- **Currency Types**: 
  - *Credits*: Earned by defeating enemies and completing challenges. Used to purchase items from the Shop.
- **Chest/Loot Chest Types**:
  - Randomly dropped items.
- **Shop Types**:
  - In-run shop with items becoming cheaper each attempt.
- **Reward Pacing**: Frequent, with rewards from defeating enemies and completing challenges.
- **Special Economy Mechanics**:
  - No explicit curse system, but economy is fast-paced.
- **Meta-Progression Currencies**:
  - None explicitly mentioned.

#### **Game: Risk of Rain 2 (2020)**
- **Currency Types**: 
  - *Credits*: Earned by defeating enemies and interacting with the environment. Used to purchase items from the Shop.
- **Chest/Loot Chest Types**:
  - Randomly dropped items.
- **Shop Types**:
  - In-run shop and meta-shop with costs increasing on reset.
- **Reward Pacing**: Frequent, with rewards from defeating enemies and opening chests.
- **Special Economy Mechanics**:
  - No explicit curse system, but economy is competitive.
- **Meta-Progression Currencies**:
  - None explicitly mentioned.

---

### PART 2 — ANALYSIS

#### **Design Lessons**
1. **Currency Systems**: 
   - Use a primary currency (e.g., *Runes*) earned through defeating enemies and completing objectives. Introduce secondary currencies (e.g., *Shards*) for meta-progression.
2. **Chest/Loot Drop Rates**:
   - Implement multiple chest types (Common, Rare, Unique) with varying drop rates to create anticipation and reward players for exploration.
3. **Shop Types**:
   - Use in-run shops for immediate rewards and meta-shops for long-term progression. Ensure reroll costs are balanced to encourage strategic play.
4. **Reward Pacing**:
   - Balance frequent rewards with meaningful progression to keep players engaged without trivializing the challenge.
5. **Special Mechanics**:
   - Incorporate a curse/debt system to add risk-reward dynamics. Introduce a gambling mechanic (e.g., *Rift Gambit*) for high-risk, high-reward opportunities.
6. **Meta-Progression**:
   - Use meta-currencies (e.g., *Runes of Power*) to unlock new abilities or items, providing long-term goals.

#### **Adaptations**
1. **Exploration Rewards**: 
   - Reward players with hidden chests and special items for exploring every area thoroughly.
2. **Cursed Economy**:
   - Introduce penalties for failure (e.g., losing currency or items) to encourage strategic play.
3. **Crafting System**:
   - Allow players to craft rare items using materials found during runs, adding depth to the economy.

#### **Notes**
- The combination of a balanced currency system, varied chest types, strategic shop mechanics, and special economy features creates an engaging loop. Players should feel rewarded for their actions while also challenged to improve their strategies for long-term success.

---

## BOLUM 02 — Build Çeşitliliği ve Cross-Class Skill Sistemleri

### PART 1 — RAW DATA

**Games and Their Mechanics:**
- **Hades:**
  - Boon system (Olympian gods grant passive upgrades).
  - Duo boons (Grant two effects simultaneously).
  - Legendary boons (Unique, powerful effects).
  - Chaos boons (Random effects).

- **Hades II:**
  - Additional aspects (New types of boons).
  - Arcana cards (Active abilities).
  - Incantations (Passive or active buffs).

- **Dead Cells:**
  - Mutation system (3 mutation slots, synergies between weapon types).

- **Risk of Rain 2:**
  - Item stacking system (Items with effects that stack).
  - Character-specific abilities.
  - Printer/cauldron (Item generation systems).

- **Vampire Survivors:**
  - Weapon evolution system (Upgrading weapons with materials).
  - Passive synergies (Passive item effects that combine).

- **Noita:**
  - Physics-based spell crafting (Physics affects spell interactions).
  - Wand combinations (Different wands create unique spells).

- **Slay the Spire:**
  - Card synergy deck building (Cards that work better together).

**RIMA Details:**
- Classes: Warblade, Elementalist, Shadowblade, Ranger.
- Each class has 12 skills.
- Skill draft system (Details unspecified).
- Questions:
  1. How many build paths exist per character?
  2. How cross-class or cross-character synergies work.
  3. How the player discovers builds (curation vs random).
  4. Reroll mechanics and choice mitigation.
  5. Most memorable/broken synergies that made the game famous.

---

### PART 2 — ANALYSIS

**Lessons from Roguelites:**

1. **Build Paths:**
   - **Hades**: Boon system creates exponential build paths due to passive upgrades.
   - **Dead Cells**: Mutation slots and weapon synergies allow for diverse builds per character.
   - **Slay the Spire**: Card synergy deck building creates numerous viable paths.

2. **Cross-Class/Cross-Character Synergies:**
   - **Hades II**: Arcana cards and incantations can synergize across classes.
   - **Risk of Rain 2**: Items like Runes and Relics work across classes.
   - **Noita**: Physics-based spells create cross-class interactions (e.g., fire and water interactions).

3. **Build Discovery:**
   - **Hades**: Curation through推荐系统 and player guides.
   - **Dead Cells**: Experimental discovery through mutation and weapon testing.
   - **Slay the Spire**: Mix of curation (recommended decks) and random discovery (card pulls).

4. **Reroll Mechanics:**
   - **Hades**: No rerolls, forcing players to adapt.
   - **Risk of Rain 2**: Printer and cauldron allow for item generation and rerolls.
   - **Slay the Spire**: Card pulls and rerolls allow for choice mitigation.

5. **Memorable Synergies:**
   - **Hades**: Duo boons combining multiple effects (e.g., Hades + Hermes).
   - **Noita**: Physics-based interactions (e.g., fire + water = steam).
   - **Dead Cells**: Mutation and weapon type synergies (e.g., Melee + Throwable).

---

**Recommendations for RIMA:**

1. **Build Diversity Mechanics:**
   - Introduce item crafting with varying effects (inspired by Noita).
   - Add passive bonuses that stack or synergize across classes (like Risk of Rain 2).
   - Implement a skill customization system where skills can be modified in different ways.

2. **Cross-Class/Cross-Character Synergies:**
   - Allow items or boons to work across classes, with unique effects for each class.
   - Create全局系统 that encourage cross-class interactions (e.g., elemental affinities).
   - Include全局事件 or全局被动 that synergize across characters.

3. **Build Discovery:**
   - Combine curation (recommended builds) with experimental discovery.
   - Use a mix of deterministic and random loot generation to encourage exploration.
   - Include tutorials or guides for new players.

4. **Reroll Mechanics:**
   - Implement a limited reroll system to mitigate bad choices.
   - Use a skill draft system to allow players to adapt their builds mid-game.

5. **Memorable Synergies:**
   - Design全局被动或物品组合 that create unique and memorable interactions.
   - Balance broken synergies to ensure they are fun but not overpowered.
   - Include hidden interactions that reward experimentation.

---

**Final Notes:**
- RIMA should focus on creating a balance between structured builds and emergent gameplay.
- Cross-class mechanics should feel natural and rewarding, not forced.
- Build diversity is achieved through both player choice and procedural generation.

---

## BOLUM 03 — Mob Tasarımı: Normal, Elite, Sürü Mekaniği

### PART 1 — RAW DATA

#### Hades: Underworld Enemies
- **Realms**: Tartarus, Asphodel, Elysium, Styx
- **Enemy Types**:
  - **Tartarus**: Torture mechanics (e.g., Meat Hook swinging)
  - **Asphodel**: Soul-ripping mechanics (e.g., Revenants)
  - **Elysium**: Energy drain mechanics (e.g., Sirens)
  - **Styx**: Poisonous melee enemies
- **Elite/Champion System**: Unique dialogue, increased health, and special attacks.
- **Swarm Mechanics**: Large numbers of small enemies with minimal threat individually but overwhelming in groups.
- **Enemy Combinations**: Mix of fast-paced melee, ranged projectiles, and bosses.
- **Visual Telegraphing**: Verbal warnings, visual effects (e.g., glowing weapons), and positional cues.
- **Enemy AI Escalation**: Increasing enemy speed, health, and attack patterns with difficulty.

#### Dead Cells: Enemy Design
- **Normal Mob Types**:
  - **Melee**: Fast-paced, aggressive enemies with parry windows.
  - **Ranged**: Lobbers and shooters with projectile attacks.
  - **Summoners**: Enemies that call in reinforcements or create hazards.
- **Elite/Champion System**: Modified stats (health, speed), unique affixes (e.g., Fireproof, Undead), and special abilities.
- **Swarm Mechanics**: Groups of small enemies overwhelm the player with numbers.
- **Enemy Combinations**: Mix of melee, ranged, and summoners to test player adaptability.
- **Visual Telegraphing**: Visual effects (e.g., charge indicators) and sound cues (e.g., weapon sounds).
- **Enemy AI Escalation**: Increased enemy count, speed, and complexity with difficulty.

#### Enter the Gungeon: Bullet Pattern Design
- **Normal Mob Types**:
  - **Snipers**: Long-range, high-damage shots.
  - **Melee**: Fast-paced, close-range attacks.
  - **Summoners**: Call in projectiles or turrets.
- **Elite/Champion System**: Modified stats, unique abilities (e.g., fire, ice), and health bonuses.
- **Swarm Mechanics**: Groups of small enemies with coordinated bullet patterns.
- **Enemy Combinations**: Mix of melee, ranged, and environmental hazards to test player positioning.
- **Visual Telegraphing**: Bullet trajectories, color-coded projectiles, and sound effects.
- **Enemy AI Escalation**: Increased bullet speed, enemy count, and pattern complexity.

#### TMNT: Splintered Fate: Enemy Variety
- **Normal Mob Types**:
  - **Melee**: Fast-paced, aggressive enemies.
  - **Ranged**: Throwable weapons (e.g., grenades).
  - **Summoners**: Call in reinforcements or create hazards.
- **Elite/Champion System**: Modified stats, unique abilities (e.g., fire, ice), and health bonuses.
- **Swarm Mechanics**: Large groups of enemies overwhelm the player with numbers and environmental hazards.
- **Enemy Combinations**: Mix of melee, ranged, and environmental hazards to test player adaptability.
- **Visual Telegraphing**: Visual effects (e.g., charge indicators) and sound cues (e.g., weapon sounds).
- **Enemy AI Escalation**: Increased enemy count, speed, and complexity with difficulty.

#### Risk of Rain 2: Elite Affixes
- **Elite Affixes**:
  - **Blazing**: Ignite attacks.
  - **Glacial**: Freeze attacks.
  - **Overloading**: Electric attacks.
  - **Corrosive**: Acid attacks.
  - **Piercing**: Penetrate shields.
  - **Bleeding**: Cause bleeding damage over time.
- **Champion Elites**: Unique mechanics, increased health, and special abilities (e.g., summoning minions).
- **Swarm Mechanics**: Groups of small enemies overwhelm the player with numbers and coordinated attacks.
- **Enemy Combinations**: Mix of melee, ranged, and environmental hazards to test player positioning.
- **Visual Telegraphing**: Color-coded projectiles, sound effects, and visual indicators (e.g., glowing weapons).
- **Enemy AI Escalation**: Increased enemy count, speed, and pattern complexity with difficulty.

#### Returnal: Enemy Phases
- **Normal Mob Types**:
  - **Melee**: Fast-paced, aggressive enemies.
  - **Ranged**: Lobbers and shooters.
  - **Summoners**: Call in reinforcements or create hazards.
- **Elite/Champion System**: Modified stats, unique abilities (e.g., fire, ice), and health bonuses.
- **Swarm Mechanics**: Large groups of enemies overwhelm the player with numbers and coordinated attacks.
- **Enemy Combinations**: Mix of melee, ranged, and environmental hazards to test player adaptability.
- **Visual Telegraphing**: Visual effects (e.g., charge indicators) and sound cues (e.g., weapon sounds).
- **Enemy AI Escalation**: Increased enemy count, speed, and complexity with difficulty.

#### RIMA Mob Design
- **Normal Mobs**:
  - **ShardWalker**: Throwing shards.
  - **VoidThrall**: Summoning Voidlings.
  - **SeamCrawler**: Throwable weapons.
  - **ChainWarden**: Long-range attacks.
  - **Penitent**: melee attacks.
  - **RelicCaster**: projectile attacks.
  - **FractureImp**: melee and projectile attacks.
- **Elites**:
  - **IronWarden**: Modified stats, unique abilities (e.g., charge attacks).
  - **FractureKnight**: Modified stats, unique abilities (e.g., ice projectiles).
  - **TwiceBorn**: Unique mechanics, increased health, and special abilities.

### PART 2 — ANALYSIS

#### Mob Design Principles for RIMA
1. **Unique Mechanics**:
   - Ensure each mob type has a distinct mechanic beyond basic melee or ranged attacks.
   - Example: ShardWalkers could have shrapnel explosions, while VoidThralls summon minions.

2. **Elite/Champion System**:
   - Introduce clear affixes (e.g., Blazing, Glacial) to differentiate elites from normals.
   - Provide unique rewards for defeating elites.

3. **Swarm Mechanics**:
   - Use environmental hazards or area-effect attacks to make small enemies dangerous in groups.
   - Example: FractureImps could drop bombs that explode after a short timer.

4. **Enemy Combination Design**:
   - Mix melee, ranged, and summoner types to create challenging encounters.
   - Example: Pair ShardWalkers with VoidThralls to force players to dodge both shards and summoned minions.

5. **Visual Telegraphing**:
   - Use color-coded projectiles, sound effects, and visual indicators (e.g., glowing weapons) to help players anticipate attacks.
   - Example: RelicCasters could have a distinct sound when casting spells.

6. **Enemy AI Escalation**:
   - Increase enemy speed, health, and attack pattern complexity with difficulty levels.
   - Example: Penitents could become more aggressive and faster in later acts.

#### Unique Mechanics for RIMA Mobs
- **ShardWalker**: Throwing shards that explode on impact, creating AoE damage.
- **VoidThrall**: Summons Voidlings periodically, which explode upon death.
- **SeamCrawler**: Fires rapid, homing projectiles that leave trails of damage.
- **ChainWarden**: Uses a long-range chain to pull players in and deal damage.
- **Penitent**: Performs a series of rapid melee attacks with occasional overhead smashes.
- **RelicCaster**: Casts spells that create walls or barriers, forcing players to dodge or find openings.
- **FractureImp**: Combines melee attacks with projectile shards that shatter into smaller fragments.

By applying these principles, RIMA can create a more engaging and diverse enemy roster that challenges players in unique and memorable ways.

---

## BOLUM 04 — Boss Tasarımı: Faz Geçişleri, AoE Tells, Pattern Tasarımı

### PART 1 — RAW DATA

#### Hades Bosses:
- **Meg**: 
  - Phase transitions: HP-based.
  - Arena mechanics: Static with environmental hazards.
- **Lernaean Bone Hydra**:
  - Phase transitions: Mechanic-based (head switching).
  - Arena mechanics: Dynamic with arena-wide attacks.
- **Theseus & Asterius**:
  - Phase transitions: HP-based and mechanic-based (arena collapse).
  - Arena mechanics: Dynamic with environmental hazards.
- **Hades**:
  - Phase transitions: HP-based and mechanic-based (portal phases).
  - Arena mechanics: Dynamic with environmental hazards.

#### Hades II:
- **New Bosses**: Not specified.
- **Chronos Fight**:
  - Phase transitions: Time-based and mechanic-based.
  - Arena mechanics: Dynamic with time manipulation.

#### Dead Cells:
- **Boss Telegraphing**: Uses visual and audio cues for attacks.
- **Parry-Heavy Design**: Emphasizes defensive mechanics like parrying and dodging.

#### Enter the Gungeon:
- **Bullet Hell Boss Patterns**: Complex, fast-paced shootouts.
- **Phase Transitions**: Triggered by HP thresholds and mechanic-based.

#### TMNT: Splintered Fate:
- **Boss Mechanics**: Unique to each boss, with specific arena hazards.
- **Arena Design**: Dynamic with environmental interactions.

#### Hollow Knight:
- **Boss Design Philosophy**: Focus on clear tells and punish windows.
- **Attack Patterns**: Visual and audio cues for attacks.

#### Returnal:
- **Phase-Based Bosses**: Each phase introduces new mechanics.
- **Sci-Fi Elements**: Incorporates unique environmental hazards.

### PART 2 — ANALYSIS

#### Design Lessons:
1. **Phase Transitions**:
   - Use HP thresholds or mechanic-based triggers for smooth transitions.
   - Dynamic arenas add depth and challenge.

2. **AoE Tell Design**:
   - Clear visual and audio cues for attacks.
   - Provide enough reaction time for player actions.

3. **Unique Mechanics**:
   - Each boss should have distinct mechanics not shared with others.
   - Incorporate environmental hazards or time manipulation.

4. **Enrage Mechanics**:
   - Triggers based on HP or specific actions.
   - Intensify difficulty with increased attack speed or area damage.

5. **Reward Structure**:
   - Drop unique items or currency that impact the game's meta.
   - Reward structure should be meaningful and balanced.

6. **Arena Design**:
   - Use static arenas for simplicity, dynamic for added complexity.
   - Environmental hazards should complement boss mechanics.

#### RIMA's TwiceBorn Boss Fight Design:

1. **Phase 1 Mechanics**:
   - **Trigger**: HP threshold (50%).
   - **Arena Hazards**: Floating debris that damages on contact.
   - **Attack Patterns**:
     - **Pattern 1**: Ground slam with a 2-second charge-up animation and a loud growl.
     - **Pattern 2**: Fireballs launched in waves, with a visual trail and sound cue 3 seconds before impact.
     - **Pattern 3**: Melee swipes with arm extending and a low growl.

2. **Phase 2 Mechanics**:
   - **Trigger**: HP threshold (25%) or after defeating mini-bosses.
   - **Arena Hazards**: Arena begins to collapse, requiring players to dodge falling debris.
   - **Attack Patterns**:
     - **Pattern 1**: Summoned minions that mimic the boss's moves, with a visual cue when they spawn.
     - **Pattern 2**: Increased speed and damage output, with no visible cues but accompanied by a high-pitched screech.
     - **Pattern 3**: Arena-wide shockwave attack preceded by a glowing core at the center.

3. **Reward Structure**:
   - **Drop**: Unique weapon "TwiceBorn's Fang" that deals double damage and has a chance to stun enemies.
   - **Meta Impact**: The Fang can be used in subsequent acts and upgrades at the blacksmith.

4. **Arena Design**:
   - **Phase 1**: Static with floating platforms and debris fields.
   - **Phase 2**: Dynamic with collapsing ceilings and shifting hazards.

This design incorporates clear phase transitions, distinct attack patterns with sufficient tells, dynamic arena mechanics, and a rewarding item that enhances the game's progression.

---

## BOLUM 05 — Hikaye Anlatımı ve Meta-Progression Narrative

### PART 1 — RAW DATA

**Hades:**
- Dialogue system with repeated runs
- Relationship system with NPCs
- Greek mythology reinterpretation
- Story progression through repeated runs

**Hades II:**
- Continued story from Hades
- New characters introduced
- Expanded Greek mythology lore

**Dead Cells:**
- Environmental storytelling
- Lore tablets as collectibles
- Unreliable narrator in the story

**Returnal:**
- Loop-based psychological horror narrative
- Collectibles for story progression
- Relatable characters and items

**Disco Elysium:**
- Skill-as-personality system
- Dialogue choices based on skills
- Storytelling through player decisions

**Risk of Rain 2:**
- Codex lore system
- Item descriptions with lore
- Exploration-based storytelling

**Hollow Knight:**
- Silent protagonist
- Environmental exploration for world-building
- Sparse dialogue and atmosphere-driven story

---

### PART 2 — ANALYSIS

#### Lessons and Notes:

1. **Story Delivery:**
   - Use varied methods (dialogue, environmental, item descriptions) to cater to different player preferences.
   - Environmental storytelling can be layered for depth without overwhelming new players.

2. **Roguelite Repetition:**
   - Repetition should enhance narrative discovery rather than frustrate players.
   - Build systems that reward repeated playthroughs with new insights or story elements.

3. **NPC/Relationship Systems:**
   - NPCs should have meaningful interactions that impact gameplay and narrative.
   - Relationships can add emotional weight and mechanical benefits.

4. **World-Building:**
   - Layer lore through exploration, collectibles, and character interactions.
   - Balance depth with accessibility to keep new players engaged.

5. **Player Motivation:**
   - Use narrative hooks like unresolved mysteries or character arcs to encourage repeated runs.
   - Reward players with new lore, abilities, or perspectives on the world.

---

### RIMA Narrative Framework

**Who is the player?**  
The player is an explorer or survivor navigating a rift world, seeking to understand or control the rift's energy. Their role ties them to the rift's origins and its impact on reality.

**Why do they keep running?**  
- Unraveling the mystery of the rift's origins.
- Protecting or altering reality from the rift's corruption.
- Pursuing personal stakes tied to the rift's existence (e.g., saving loved ones, seeking power).

**NPCs at the Hub:**
- **Rift Guide:** A mysterious figure who provides cryptic advice and lore hints.
- **Local Blacksmith:** Offers upgrades and shares stories of the rift's effects on the world.
- **Traveler NPCs:** Characters with unique backstories tied to the rift, offering quests or insights.

**Lore Delivery:**
- Use environmental clues like rift-tainted artifacts and ruins.
- Implement item descriptions that reveal lore about the rift and its history.
- Add codex-like systems for collecting and unlocking deeper narratives.

This framework ensures a balance between depth and accessibility, using repetition to enhance narrative discovery while keeping players motivated by unresolved mysteries and meaningful interactions.

---

## BOLUM 06 — UI/HUD Tasarımı ve Oyuncu İletişimi

### PART 1 — RAW DATA

**Analysis of HUD/UI Design in Action Roguelites**

1. **Hades**
   - **Always Visible:** Health, boons active.
   - **Cooldowns:** On items/abilities.
   - **Resources:** Health, boons.
   - **Death Feedback:** Learnings from run.
   - **Run Summary:** Boons, enemies killed, damage dealt.
   - **Accessibility:** Colorblind mode, zoom.

2. **Dead Cells**
   - **Always Visible:** Blueprint system, mutations.
   - **Cooldowns:** HUD or blueprint.
   - **Resources:** Health, Essence.
   - **Death Feedback:** Nearby enemies, health at death.
   - **Run Summary:** Kills, damage, blueprint progress.
   - **Accessibility:** Colorblind mode, zoom.

3. **Enter the Gungeon**
   - **Always Visible:** Item synergy, ammo, health.
   - **Cooldowns:** Abilities.
   - **Resources:** Health, ammo.
   - **Death Feedback:** Equipped items, run summary.
   - **Run Summary:** Damage, kills, item effects.
   - **Accessibility:** Colorblind mode.

4. **Risk of Rain 2**
   - **Always Visible:** Item count, DPS meter.
   - **Cooldowns:** Skills/items.
   - **Resources:** Health, Metal.
   - **Death Feedback:** Last actions, rerolls.
   - **Run Summary:** Damage, kills, currency, item levels.
   - **Accessibility:** Colorblind mode, text size.

5. **Returnal**
   - **Always Visible:** Health, ammo.
   - **Cooldowns:** Abilities.
   - **Resources:** Health, ammo.
   - **Death Feedback:** Learnings from run.
   - **Run Summary:** Damage, kills, item effects.
   - **Accessibility:** Colorblind mode.

6. **TMNT: Splintered Fate**
   - **Always Visible:** Multiplayer HUD, health/combo.
   - **Cooldowns:** Abilities.
   - **Resources:** Health, combo points.
   - **Death Feedback:** Team status, tips.
   - **Run Summary:** Stats, achievements.
   - **Accessibility:** Zoom.

**RIMA HUD Design**

- **Always Visible:** Class resources (Rage, Mana, Energy/ComboPoints, Focus), active skills with cooldowns, health, current buffs/debuffs.
- **Skill Draft Screen:** Radial menu or grid with ability categories for each class, customizable shortcuts.
- **Death Screen:** Resource levels at death, nearby enemies, missed opportunities.
- **Accessibility:** Colorblind modes, text size, sound cues.

### PART 2 — ANALYSIS

**Design Lessons and Adaptations for RIMA HUD**

1. **Clarity and Differentiation:** Use icons or colors for each class's resources to avoid clutter and ensure visibility.
2. **Contextual Information:** Implement overlays or tooltips for contextual data without overwhelming the HUD.
3. **Skill Management:** Provide an intuitive skill draft screen with categories, allowing easy access and customization.
4. **Death Feedback:** Show resource states and learning opportunities to enhance player improvement.
5. **Accessibility:** Offer high contrast, text resizing, and sound cues to cater to diverse needs.

**Notes for Implementation**

- Ensure flexibility in HUD layout to accommodate the four classes without sacrificing clarity.
- Test HUD designs to ensure they don't overwhelm players while providing necessary information.
- Regularly iterate based on player feedback to refine HUD effectiveness and accessibility.

---

## BOLUM 07 — Multiplayer ve Co-op Roguelite Mekaniği

### PART 1 — RAW DATA

- **Games Analyzed**:
  - **Hades II**: Added co-op elements (player count not specified).
  - **TMNT: Shredder's Revenge**: Supports up to 6 players, combo system between players.
  - **TMNT: Splintered Fate**: Co-op roguelite mechanics (player count unspecified).
  - **Risk of Rain 2**: Full multiplayer scaling, item sharing vs individual.
  - **Deep Rock Galactic**: Class synergy in co-op, shared objectives, 4-player.
  - **It Takes Two**: Forced co-op mechanics, 2-player.
  - **Gunfire Reborn**: Co-op roguelite with class synergies, up to 4 players.

- **Features**:
  - **Player Count**: 2-6 players across games.
  - **Difficulty Scaling**: Adjusted based on player count; Risk of Rain 2 scales fully.
  - **Loot System**: Shared vs individual; Risk of Rain 2 has shared loot, Hades II has individual.
  - **Revive System**: Most have revive systems where dead players can assist alive ones.
  - **Class Synergy**: Required in Deep Rock Galactic and Gunfire Reborn.
  - **Communication Tools**: Built-in pings and voice chat common.
  - **Build Strategy**: Co-op affects by allowing complementary builds.

### PART 2 — ANALYSIS

- **Should RIMA Have Co-op?**:
  - **Advantages**: Enhances replayability, shared experiences, accessibility, depth through synergy.
  - **Disadvantages**: Complexity in design, potential imbalance, requires coordination.

- **Player Count**:
  - **Recommendation**: Support 2-4 players. Balances simplicity and varied dynamics.

- **Class Synergy**:
  - **Design**: Interdependent classes (e.g., tank and healer) with flexibility, allowing non-rigid roles.

- **Communication Tools**:
  - **Implementation**: Include basic pings and voice chat for effective coordination.

- **Revive Mechanic**:
  - **Design**: Align with dark fantasy tone; dead players assist in carrying alive ones, adding tension.

- **Skill Draft**:
  - **Approach**: Allow skill sharing or trading, ensuring balance to prevent overpowering.

- **Difficulty Scaling**:
  - **Adjustment**: Increase enemies or their strength with more players, adjust item drops.

- **Loot System**:
  - **Hybrid Model**: Mix shared and individual loot for fairness and personalization.

- **Testing**:
  - **Necessity**: Essential playtesting to refine balance and synergy in co-op mode.

---

