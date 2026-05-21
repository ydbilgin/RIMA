
# CB Synthesis Sub-Genre + Class System Final - Codex Verdict

## 0. Context

- Date: 2026-05-18.
- Inputs read: `CODEX_TASK_yasinderyabilgin.md`, `STAGING/CODEX_TASK_cb_pivot_design_review_DONE.md`, `F:/LaurethStudio/02_GAMES/CircuitBreaker/00_FULL_DESIGN_DOC.md`.
- Market sanity sources checked: Hero Siege Steam, Magicraft Steam, Magicraft public sales note, Magicraft SteamRev, Soulstone Survivors Steam, Soulstone Survivors owner/sales estimate.
- Main verdict: public label should be **Cascade ARPG**; long descriptor should be **Battlefield Alchemy Roguelite**; internal pattern should remain **Environmental Cascade Combat**.
- MVP verdict: the synthesis works in 16 weeks only as a one-act vertical slice, not as full 5 dungeon x 3 floor content.

---

## 1. Sub-genre 3 oneri

### 1.1 Cascade ARPG

- Capsule pitch: A top-down action RPG roguelite where every kill paints the arena, class skills reshape the floor, and one trigger can turn the room into a controlled chain reaction.
- One-line USP: You do not only cast spells; you build the battlefield and detonate it.
- Best use: Steam capsule, trailer title card, short store description, creator shorthand.
- Strength: short, memorable, says mass-clear/action/build without explaining the whole system.
- Risk: ARPG can overpromise loot depth; trailer must show that "cascade" is the real hook.

| Priority | Steam tag | Why |
|---:|---|---|
| 1 | Action Roguelite | Primary discovery bucket. |
| 2 | Action RPG | Class/build/manual combat promise. |
| 3 | Roguelite | Run loop and meta expectation. |
| 4 | Top-Down | Camera clarity. |
| 5 | Magic | Element fantasy. |
| 6 | Bullet Hell | Valid if projectile/boss density is visible. |
| 7 | Hack and Slash | Use in ARPG mass-clear sense. |
| 8 | Pixel Graphics | Visual format. |
| 9 | Arena Shooter | Room combat and manual aim. |
| 10 | 2D | Format support. |

| Reference | CB overlap | CB difference | Clone risk |
|---|---|---|---|
| Hero Siege | Acts, classes, mobs, ARPG-lite promise. | Floor-state cascade is power source, not loot economy. | Medium. |
| Hades | Manual action, rooms, readable boss combat. | Not god boons, not narrative chamber economy, not melee dash-strike. | Medium. |
| Magicraft | Spell spectacle and roguelite rooms. | No wand graph; spatial terrain setup is core. | Medium-high. |
| RoR2 | Class kit mastery and variant inspiration. | Not 3D item-stack survival; CB is tile-state arena combat. | Low. |

### 1.2 Battlefield Alchemy Roguelite

- Capsule pitch: A manual-combat roguelite where water, oil, fire, static, ice, dust, and hybrids become an alchemy board you fight on in real time.
- One-line USP: The arena is the spellbook.
- Best use: press kit, design doc, longer store description, feature explanation.
- Strength: explains the USP more accurately than Cascade ARPG.
- Risk: too long for capsule; "alchemy" can imply crafting menus instead of action.

| Priority | Steam tag | Why |
|---:|---|---|
| 1 | Action Roguelite | Commercial bucket. |
| 2 | Magic | Alchemy fantasy. |
| 3 | Top-Down | Camera promise. |
| 4 | Roguelite | Run structure. |
| 5 | Action RPG | Class/build layer. |
| 6 | Bullet Hell | If pressure is visible. |
| 7 | Pixel Graphics | Art format. |
| 8 | Strategy | Secondary setup-payoff signal. |
| 9 | Tactical | Secondary; avoid turn-based confusion. |
| 10 | Hack and Slash | ARPG-adjacent mass clear. |

| Reference | CB overlap | CB difference | Clone risk |
|---|---|---|---|
| Hero Siege | Act/Floor and enemy density. | Alchemy terrain is the main build expression. | Low-medium. |
| Hades | Manual dodge/aim and boss polish. | No Hades-style boon/narrative chamber identity. | Low-medium. |
| Magicraft | Elemental spell chaos. | Terrain grammar is controlled and spatial, not wand sequence. | Medium. |
| RoR2 | Class structure inspiration. | No shrine/time-pressure/item-stack core. | Low. |

### 1.3 Elemental Cascade Roguelite

- Capsule pitch: A real-time roguelite where class skills and enemy deaths create elemental ground states, then trigger shots convert the arena into chain reactions.
- One-line USP: Every room becomes a live elemental circuit.
- Best use: fallback public label if ARPG creates wrong expectations.
- Strength: more precise than Cascade ARPG for MVP if loot depth is low.
- Risk: less commercial and less sharp than Cascade ARPG.

| Priority | Steam tag | Why |
|---:|---|---|
| 1 | Action Roguelite | Primary bucket. |
| 2 | Roguelite | Run loop. |
| 3 | Magic | Element identity. |
| 4 | Top-Down | Camera. |
| 5 | Action RPG | Class/build support. |
| 6 | Bullet Hell | Combat pressure. |
| 7 | Arena Shooter | Room fighting. |
| 8 | Pixel Graphics | Art. |
| 9 | 2D | Format. |
| 10 | Hack and Slash | Mass-clear adjacency. |

### 1.4 NET KARAR

- Primary public sub-genre: **Cascade ARPG**.
- Secondary descriptor: **Battlefield Alchemy Roguelite**.
- Internal design-pattern name: **Environmental Cascade Combat**.
- Store sentence: `Circuit Breaker is a Cascade ARPG: paint the arena with elemental ground states, merge them into volatile hybrids, then trigger real-time chain reactions to erase entire mob waves.`
- Streamer sentence: `Hades hands, ARPG mobs, Noita-style reactions, but on a controllable battlefield grid.`

---

## 2. Real-time generative combat USP dogrulama

### 2.1 Verdict

- Claude analysis is directionally correct, but the wording should be less absolute.
- CB is not the only real-time generative combat game.
- CB is rare in this exact combination: real-time manual action + player-authored ground setup + enemy-death terrain + discrete tile reactions + ARPG mob density + class kit mastery + roguelite loop.
- Stronger wording: `CB targets the gap between Noita's uncontrolled real-time simulation and Into the Breach's controlled board-state causality: readable real-time action where the player deliberately builds and detonates the arena.`

| Combat type | Example | CB relation | Verdict |
|---|---|---|---|
| Reactive action | Hades, Dead Cells | Shares dodge/aim/punish feel. | Not core USP. |
| Passive survivor-like | Vampire Survivors, Brotato | Shares mob volume/build escalation. | Input model is different. |
| Reactive ARPG | Diablo, PoE, Last Epoch | Shares mass-clear/class fantasy. | Spatial setup grammar differs. |
| Turn-based generative | Into the Breach | Shares causal clarity. | Tempo is different. |
| Real-time physics generative | Noita | Shares element reactions. | CB must be controlled/readable. |
| Real-time spellcraft | Magicraft | Shares spell chaos. | CB is terrain-first, not wand-first. |
| Element combo action | Magicka | Shares element interaction. | CB is persistent battlefield state, not input recipe. |

### 2.2 Nearby games

| Game | Similarity | Difference | Anti-clone requirement |
|---|---|---|---|
| Noita | Environmental reactions, fire/water/acid chaos, emergent kill chains. | Pixel physics, destructible world, side-view cave traversal, wand crafting. | Avoid continuous sim and terrain destruction as core. |
| Magicka | Element combination and timing. | Instant recipes, co-op comedy, no persistent battlefield economy. | Avoid recipe-casting identity. |
| Magicraft | Spellcraft roguelite, high VFX, room choices. | Wand/spell chain is core; arena state is not the main grammar. | Avoid wand graph and projectile-only modifiers. |
| Wizard of Legend | Fast top-down spell action. | Loadout spells, not persistent terrain setup. | Low. |
| Soulstone Survivors | Horde clearing, classes, build growth. | Passive/semiauto survivor-like pressure. | Keep manual triggers central. |
| Spell Disk | Spell-trigger chaining. | Proc graph, not spatial tile arena. | Keep floor causality primary. |
| RoR2 | Class kits and unlock inspiration. | 3D item-stack survival. | Do not copy shrine/time-pressure identity. |

### 2.3 USP guardrails

- The tile state must be visible before it is powerful.
- Player-authored terrain must decide the best cascades.
- Enemy death terrain should add opportunity, not replace player intention.
- Triggers must be manual, readable, and swappable.
- Modifiers must enhance cascade grammar, not replace it with passive DPS.
- If the best build wins without reading the floor, the USP failed.
- If the best cascade is random and unrepeatable, the USP failed.
- If the VFX hides the terrain state, the USP failed.

### 2.4 Final USP sentence

- `CB is not a spellcraft game where spells happen to hit the floor; it is a battlefield-state game where the floor is the combo system.`

---

## 3. Map yapisi final lock (A/B/C)

### 3.1 Comparison

| Option | Structure | Strength | Weakness | 16-week fit | CB fit |
|---|---|---|---|---|---|
| A. RoR2 linear levels | 6 levels + final boss | Simple, proven, easy to scope. | Weak act identity; can feel like flattened RoR2. | Good. | Medium. |
| B. Hero Siege Act/Floor | Act selection, dungeon/floor stack, act boss. | Strong ARPG-lite identity, scalable, class/content friendly. | Full 5 dungeon x 3 floor is too large for MVP. | Good only as B-lite. | High. |
| C. PoE Atlas | Endgame map grid. | Strong long-tail retention. | Requires map mods, rewards, economy, UI, content depth. | Bad. | Phase 4 only. |

### 3.2 NET recommendation

- Final vision lock: **B. Hero Siege Act/Floor**.
- MVP implementation lock: **B-lite**.
- B-lite means:
  - 1 Act.
  - 6 combat floors/rooms.
  - 1 shop/shrine intermission.
  - 1 optional fork after floor 2 or 3.
  - 1 Act boss.
  - No full 5 dungeon x 3 floor content in 16-week MVP.
- Post-MVP expansion path:
  - Phase 2: 2 acts or 2 dungeon archetypes.
  - Phase 3: 5 dungeon x 3 floor act structure.
  - Phase 4: Atlas/endgame grid.

### 3.3 Why not A

- A is useful as prototype scaffolding but too identity-thin for final design.
- A says survival-run more than ARPG-lite campaign.
- A weakens act bosses, biome rules, and class unlock theming.
- A increases RoR2 comparison without inheriting RoR2's 3D strengths.

### 3.4 Why B

- B gives the game a scalable ARPG spine.
- B supports class unlocks, biome themes, floor rules, and act bosses naturally.
- B lets the MVP be one clean slice instead of a fake endgame.
- B lets future content be added without replacing the whole map model.

### 3.5 Why C later

- Atlas without content depth feels fake.
- Atlas needs node UI, map modifiers, reward types, unlock logic, economy sinks, and endgame balance.
- Atlas should only arrive after players already want more CB runs.

### 3.6 MVP run map

| Segment | Duration target | Purpose |
|---|---:|---|
| Floor 1 | 2:00 | Teach drop + trigger. |
| Floor 2 | 2:30 | Teach first hybrid. |
| Fork | 0:30 | Reward vs elite agency. |
| Floor 3 | 3:00 | First 30-40 peak surge. |
| Floor 4 | 3:00 | Hazard/elite readability. |
| Floor 5 | 3:00 | Modifier payoff. |
| Shop/shrine | 1:00 | Heal/reroll/build pacing. |
| Floor 6 | 2:30 | Pre-boss 40-50 peak. |
| Act boss | 4:00 | 3 phases, arena-state requirement. |
| Total | 21:30 | 20-25 min target. |

### 3.7 16-week scope impact

| System | Full B request | MVP B-lite cut |
|---|---|---|
| Dungeons | 5 dungeons | 1 dungeon/act slice. |
| Floors | 15 floors | 6 floors/rooms. |
| Bosses | 5 dungeon bosses + act boss | 1 polished act boss. |
| Biomes | 5 themed areas | 1 Act 1 theme. |
| Reward map | Multi-branch | 1 fork + 1 shop/shrine. |
| Meta unlock | Full act progression | Cinder shell. |
| Atlas | Future | None. |

### 3.8 Final map sentence

- Lock Hero Siege Act/Floor as the long-term spine, but ship one B-lite act slice in MVP. Full 5 dungeon x 3 floor is roadmap, not 16-week scope.

---

## 4. Class system spec final

### 4.1 Verdict

- RoR2 class chassis + Form ultimate + post-MVP skill variants is correct.
- MVP class count: 3.
- MVP class shape: 4 skills + Form per class, where one skill can be mobility/simple utility.
- Trigger weapon: universal 5-element trigger weapon for all classes, with class affinity.
- Form cooldown: 40-45 sec base, not flat 30 sec.
- Skill variants: Phase 2, not MVP.

### 4.2 Final class chassis

| Slot | Input | Role | Complexity |
|---|---|---|---|
| Primary | LMB | Basic damage and tile interaction. | Low. |
| Drop/trigger | RMB | Current element drop/trigger behavior. | Medium. |
| Skill 1 | 1 | Main terrain placement. | Medium. |
| Skill 2 | 2 | Secondary terrain/control. | Medium. |
| Skill 3 | 3 | Burst/booster/utility. | Medium. |
| Mobility | Shift or class slot | Dash/glide/slide. | Low-medium. |
| Form | R | Ultimate identity and cascade amplifier. | Medium-high. |
| Swap | Q/E | Cycle trigger element. | Low. |

### 4.3 Identity preservation

| Identity axis | Aquamancer | Pyrotechnist | Stormcaller |
|---|---|---|---|
| Main terrain verb | Pool, connect, freeze, preserve. | Ignite, prime, explode, zone-deny. | Pull, compress, scatter, reposition. |
| Main payoff | Conductive water chains. | Volatile oil/fire detonations. | Dust/gas grouping and vacuum collapse. |
| Mobility | Slide/skate through water/ice. | Flame dash and risky burst. | Glide/blink across dust/gas. |
| Defense | Slow and route control. | Kill before danger reaches player. | Avoidance and displacement. |
| Weakness | Slower burst without setup. | Self-harm/overcommit risk. | Low direct damage if enemies spread. |
| Best hybrid | Slush/Conductive. | Emulsion/Volatile. | Dust/Gas compression. |

### 4.4 MVP class kits

| Class | Skill 1 | Skill 2 | Skill 3 | Mobility | Form |
|---|---|---|---|---|---|
| Aquamancer | Tide Pool: 3x3 water. | Frost Wall: ice line slow/block. | Pressure Burst: push/pull from water. | Slipstream Dash. | Deluge Form: wider water/electric chain. |
| Pyrotechnist | Oil Slick: 3x5 oil. | Static Plate: high-risk static primer. | Spark Dance: dash emits current trigger sparks. | Flame Dash. | Inferno Form: oil/fire reactions spread faster. |
| Stormcaller | Dust Cloud: 4x4 slow/vision field. | Gas Pocket: 2x2 volatile gas. | Cyclone: pull enemies/tile effects. | Wind Glide. | Tempest Form: vacuum radius grows. |

### 4.5 Skill variant unlock: 1 week or 2 weeks?

| Version | Scope | Cost | Verdict |
|---|---|---:|---|
| Debug prototype | Data flag + one alternate behavior + save bit. | 1 week | Fine for internal test. |
| Player-facing Phase 2 | UI, unlock text, Cinder/challenge rule, save/load, balance, tooltip, incompatible modifier handling. | 2 weeks minimum | Realistic. |
| Full roster support | Variants for 5+ classes and many skills. | 3-5 weeks | Roadmap. |

- Answer: real skill variant unlock is **2 weeks**, not 1, unless it is only a debug prototype.

### 4.6 Class-bound vs universal trigger weapon

| Option | Pros | Cons | Verdict |
|---|---|---|---|
| Class-bound trigger | Strong identity, easier balance. | Kills 5-element grammar and experimentation. | Reject. |
| Universal trigger, no affinity | Full grammar. | Classes can overlap. | Acceptable but bland. |
| Universal trigger + class affinity | Full grammar plus identity. | Needs UI and tuning. | Lock. |

- Every class can use all 5 triggers.
- Each class gets 2 favored trigger relationships and 1 awkward relationship.
- Affinity should add utility or reliability, not mandatory raw DPS.
- Shared trigger weapon is the common language; class skills decide the easiest grammar.

### 4.7 Form cooldown

| Model | Pros | Cons | Verdict |
|---|---|---|---|
| Flat 30 sec | Frequent, flashy. | Too spammy at 30-50 mobs; weakens setup value. | Reject default. |
| Flat 45 sec | Strong moment, easier boss balance. | Slightly less flashy. | Lock base. |
| Voltage-scaled 45-60 sec | Endgame pressure knob. | Needs tuning. | Use later. |
| Kill-charge reduction | Rewards cascades. | Snowball risk. | Use capped. |

- Final Form spec:
  - Base cooldown: 45 sec.
  - Duration: 5-7 sec.
  - Combo kills can reduce cooldown in capped chunks.
  - Voltage can increase cooldown or reduce cooldown gain.
  - Demo/tutorial may start with Form charged for trailer pacing.

### 4.8 Final class sentence

- Lock 3 MVP classes, each with 4 active verbs plus Form, universal 5-trigger weapon with class affinity, and no variants until Phase 2.

---

## 5. 16 hafta MVP fit

### 5.1 Five synthesis layers

| Layer | In MVP? | MVP expression | Reason |
|---|---|---|---|
| ARPG skeleton | Yes | 3 classes, cooldown skills, 1 act run, 1 boss. | Genre promise. |
| Roguelite loop | Yes | Run reset, modifier choices, Spark/Cinder minimal. | Replay promise. |
| Mass-clear volume | Yes | 30-50 peak in surge rooms. | USP proof. |
| Element grammar | Yes | 5 triggers, 7 base states, 3 hybrids. | Core grammar. |
| Spatial cascade USP | Yes | 2-mod weapon, hybrid reactions, Form amplifier. | Differentiation. |

- All five layers must appear.
- None can be full-scale in 16 weeks.
- Do not cut a layer; cut content count inside every layer.

### 5.2 Mandatory post-MVP cuts

| System | Verdict | Reason |
|---|---|---|
| Full 5 dungeon x 3 floor | Post-MVP | Content multiplier. |
| PoE Atlas | Phase 4 | Endgame UI/economy/content debt. |
| 5+ classes | Post-MVP | Class polish and balance debt. |
| Skill variants | Phase 2 | UI/save/balance cost. |
| 30-50 modifiers | Post-MVP | 12-18 visible modifiers are enough. |
| 7+ hybrids | Post-MVP | 3 hybrids prove grammar. |
| Echo currency | Post-MVP | Needs endgame loop. |
| Secret boss | Post-MVP | Boss polish is expensive. |
| Full loot economy | Post-MVP or never | High Hero Siege/Diablo clone risk. |

### 5.3 Fit verdict on revised scope

- 3 class + 2-mod + 3 hybrid + 12-18 modifier + 6 rooms + 1 boss: fits with discipline.
- Hero Siege Act/Floor language: fits if implemented as B-lite.
- 6 dungeon x 3 floor: does not fit.
- 30-50 mob peak: fits technically, but requires spawn director, VFX limiter, and readability budget.
- 220-280 total enemies/run: fits only if enemy families are limited to 4-6 MVP families.
- 60 peak boss arena: risky; prefer 40-50 unless performance/readability pass.

### 5.4 Revised 16-week breakdown

| Week | Goal | Deliverable | Cut discipline |
|---:|---|---|---|
| 1 | Foundation | Controller, aim, dash, camera, test arena. | No map generator. |
| 2 | Combat core | Event bus, health, damage, hit pause, dummy enemies. | No modifier content. |
| 3 | GroundMark v1 | Runtime grid and 7 base states. | No hybrids yet. |
| 4 | Trigger weapon | 5 triggers, Q/E swap, HUD. | No class skills yet. |
| 5 | 2-mod UX | Drop/trigger preview, arming delay, danger border, input buffer. | Lock controls. |
| 6 | Reactions | Slush, Emulsion, Volatile, 3 status hybrids. | No extra elements. |
| 7 | Spawn density | 4-6 enemies, death tiles, 3-phase room rhythm. | No 15-family list. |
| 8 | Aquamancer | Full playable kit + Form. | Simple VFX. |
| 9 | Pyrotechnist | Full playable kit + Form. | Reuse hooks. |
| 10 | Stormcaller | Full playable kit + Form. | No variants. |
| 11 | Modifiers | ModifierDef + 12 modifiers + proc limiter. | No node graph. |
| 12 | Run loop | 6 floors, fork, reward, shop/shrine, Spark. | No atlas. |
| 13 | Boss | 1 act boss, 3 phases, arena-state requirement. | No secret boss. |
| 14 | Readability/art | Act 1 tileset, VFX budget, tile overlay, color grammar. | No 5 biomes. |
| 15 | Balance/onboarding | 20-25 min run, tutorial, Voltage 0-3 shell, Cinder minimal. | No Echo. |
| 16 | Build/trailer | Demo build, 15 sec cascade clip, bugfix, content lock. | No new features. |

### 5.5 MVP content lock

| Content | Count | Notes |
|---|---:|---|
| Classes | 3 | Aquamancer, Pyrotechnist, Stormcaller. |
| Active kits | 3 x 4 + Form | One mobility/simple utility per class. |
| Triggers | 5 | Universal. |
| Base tile states | 7 | Normal, Water, Fire, Oil, Static, Ice, Dust. |
| Hybrid tiles | 3 | Slush, Emulsion, Volatile. |
| Status hybrids | 3 | Conductive, Steaming, Combusting. |
| Enemy families | 4-6 | Enough for room grammar. |
| Modifiers | 12-18 | Visible, not numerous. |
| Floors/rooms | 6 | One act slice. |
| Bosses | 1 | High polish. |
| Currencies | 2 | Spark and Cinder. |
| Voltage | 0-3 shell | Full 0-32 later. |

### 5.6 MVP success tests

| Test | Pass condition |
|---|---|
| 5-minute comprehension | Player creates water + electric without a manual. |
| 15-second clip | Clip shows setup, trigger, cascade, mass kill. |
| Class identity | Same room feels different across 3 classes. |
| Readability | Player can name safe/dangerous tile zones during surge. |
| Extensibility | New tile reaction can be added in under 2 hours. |
| Performance | 50 enemies + cascade holds target frame rate. |
| Build loop | Modifier choices visibly alter cascade behavior. |

### 5.7 Final MVP sentence

- One act, three classes, five triggers, three hybrids, twelve to eighteen modifiers, six rooms, one boss, one unforgettable cascade clip.

---

## 6. Pazar pozisyon + Steam tag + fiyat

### 6.1 Market position

| Dimension | Hero Siege | Magicraft | Soulstone Survivors | CB target |
|---|---|---|---|---|
| Genre | Pixel ARPG, hack and slash, roguelike/RPG elements. | Spellcraft action roguelike. | Action roguelite/survivor-like with RPG unlocks. | Cascade ARPG / Battlefield Alchemy Roguelite. |
| Combat input | Manual ARPG. | Manual spellcraft. | Horde action with build emphasis. | Manual aim/drop/swap/trigger. |
| Build expression | Classes, items, economy, seasons. | Wand/spell combinations, relics. | Characters, weapons, skills, meta. | Class kit + terrain grammar + modifiers. |
| Map structure | Acts/dungeons/seasons. | Rooms and choices. | Arenas/maps. | Act/Floor B-lite, later atlas. |
| Main spectacle | Loot/build power and horde kill. | Spell chaos. | Screen-filling horde clear. | Controlled environmental cascade. |
| Price signal | Steam current check showed $7.99 base for Hero Siege. | Steam pages showed $15.99; public note says 700K+ sold. | Steam page showed $14.99 base; sale periods vary. | $14.99 recommended. |

### 6.2 Steam tag priority

| Priority | Tag | Use |
|---:|---|---|
| 1 | Action Roguelite | Primary discoverability. |
| 2 | Action RPG | Class/build/mass-clear expectation. |
| 3 | Roguelite | Run loop. |
| 4 | Top-Down | Camera clarity. |
| 5 | Magic | Element fantasy. |
| 6 | Bullet Hell | Use if projectile/enemy pressure is visible. |
| 7 | Hack and Slash | ARPG mass-clear sense. |
| 8 | Pixel Graphics | Visual identity. |
| 9 | Arena Shooter | Room combat and manual aim. |
| 10 | 2D | Format. |
| 11 | RPG | Build/class support. |
| 12 | Difficult | Voltage support. |
| 13 | Singleplayer | Default. |
| 14 | Strategy | Setup planning, secondary. |
| 15 | Colorful | Optional if art supports it. |

### 6.3 Tags to avoid as primary

| Tag | Reason |
|---|---|
| Survivor-like | Implies passive weapons and auto-DPS. |
| Auto Battler | Wrong input expectation. |
| Deckbuilder | Wrong system expectation. |
| Simulation | Invites Noita physics comparison. |
| Souls-like | Wrong combat promise. |
| Metroidvania | Wrong progression promise. |
| Loot | Avoid until item economy exists. |
| MMO | Wrong scale and online promise. |

### 6.4 Price point

- Recommended Early Access / launch price: **$14.99**.
- Launch discount: 10 percent.
- First content-update discount: 15-20 percent.
- Avoid $7.99 unless scope collapses into a small survivor-like.
- Avoid $19.99 unless content, art, bosses, class count, and polish are much larger at launch.
- Rationale:
  - Magicraft and Soulstone sit around the same visible price tier in current store checks.
  - CB promises more manual/class/system depth than a minimal survivor-like.
  - $14.99 supports ARPG-lite positioning without overpricing a solo-dev MVP.

### 6.5 Year 1 sales target

| Scenario | Year 1 units | Conditions |
|---|---:|---|
| Conservative | 25K-50K | Solid demo, niche visibility, limited creator pickup. |
| Base target | 75K-150K | Strong Steam Next Fest demo, readable trailer, 85%+ reviews. |
| Stretch | 250K-500K | Viral cascade clips, creator adoption, frequent updates. |
| Outlier | 700K+ | Magicraft/Soulstone-level breakout. |

- Recommended internal target: **100K Year 1 units**.
- Pre-launch KPI: 50K wishlists minimum for confident launch; 100K+ for breakout attempt.
- Trailer KPI: first 5 seconds must show cascade, not walking.
- Demo KPI: median playtime above 25 minutes and at least one completed run attempt per player.

### 6.6 Positioning statement

- Player-facing: `If you like Hades-level manual control, ARPG horde clearing, and spell reactions you can actually plan, CB is built for you.`
- Store short description: `A Cascade ARPG where every kill paints the floor, every class reshapes the battlefield, and every trigger can detonate a room-wide elemental chain reaction.`
- Press kit: `CB sits between Noita's emergent elemental chaos and Hades-style readable action, but moves the core decision onto a controllable top-down battlefield grid.`

---

## 7. Anti-klon mitigasyon

| Clone risk | Mitigation | CB difference sentence |
|---|---|---|
| Hero Siege clone | Keep loot economy minimal in MVP and make terrain cascade, not gear score, the main power expression. | Hero Siege asks what your build can kill; CB asks how you reshape the room before you pull the trigger. |
| RoR2 clone | Use RoR2 only as class kit structure; avoid shrine/time-pressure/item-stack identity. | RoR2 is survival through item-stack momentum; CB is battlefield authorship through elemental terrain and timing. |
| ARPG general clone | Do not chase loot depth in MVP; let class skills and terrain reactions carry buildcraft first. | CB's buildcraft starts on the floor, not in the inventory. |
| VS/Brotato clone | Keep attacks manual and cascades dependent on spatial setup rather than passive aura DPS. | Survivor-likes reward surviving your build; CB rewards designing the next screen clear under pressure. |
| Magicraft clone | Avoid wand/spell-chain editor as core; modifiers should alter terrain reactions and trigger behavior. | Magicraft builds spells; CB builds rooms. |
| Noita clone | Stay discrete, readable, top-down, and controlled; no continuous pixel physics or terrain destruction as the core promise. | Noita lets the world escape control; CB lets the player weaponize control in real time. |
| Hades clone | Avoid god boon presentation, narrative chamber economy, and melee dash-strike rhythm. | Hades is reaction mastery; CB is setup-payoff mastery. |

### 7.1 Core anti-clone rules

- Every trailer beat must show tile causality.
- Every class must have a terrain verb, not only damage verbs.
- Every modifier tier must include terrain/cascade modifiers.
- Every boss must interact with arena state.
- Every room must contain a reason to place, merge, or trigger ground states.
- Avoid UI patterns that visually mimic Hades boons or PoE passive webs in MVP.
- Avoid item-rarity color obsession until terrain grammar is understood.
- Avoid auto-attacking builds becoming optimal.
- Avoid a wand graph.
- Avoid continuous physics simulation.
- Avoid full loot economy until CB's own grammar is stable.

---

## 8. 3-belge yapi onerisi

### 8.1 Document structure

| Document | Purpose | Time horizon |
|---|---|---|
| VISION_DOC.md | Full game identity and long-term design truth. | 12+ months. |
| MVP_PLAN.md | 16-week concrete execution plan. | Now to demo. |
| ROADMAP.md | Phased expansion after MVP. | Month 4 onward. |

### 8.2 VISION_DOC.md sections

1. One-sentence vision.
2. Sub-genre lock.
3. Design pillars.
4. Player fantasy.
5. Core loop.
6. Combat grammar.
7. Environmental Cascade Combat rules.
8. Element taxonomy.
9. Tile-state taxonomy.
10. Hybrid taxonomy.
11. Status hybrid taxonomy.
12. Trigger weapon philosophy.
13. Class system philosophy.
14. Full class roster vision.
15. Full skill variant philosophy.
16. Form ultimate philosophy.
17. Act/Floor long-term structure.
18. Atlas/endgame vision.
19. Voltage full difficulty vision.
20. Currency vision.
21. Relic/modifier full vision.
22. Enemy family vision.
23. Boss design vision.
24. Art direction.
25. VFX readability rules.
26. UI/HUD philosophy.
27. Audio fantasy.
28. Market positioning.
29. Anti-clone guardrails.
30. Long-term content phases.
31. Explicit non-goals.
32. Decision log.

### 8.3 Decisions for VISION_DOC.md

- Cascade ARPG public label.
- Battlefield Alchemy Roguelite descriptor.
- Environmental Cascade Combat pattern name.
- Full 5+ class roster.
- Full 5 dungeon x 3 floor act ambition.
- Atlas as endgame ambition.
- Voltage 0-32 long-term ambition.
- Echo endgame currency.
- Seasonal content possibility.
- Full skill variant unlock strategy.
- Full enemy family list.
- Full hybrid list.
- Full boss philosophy.
- Full anti-clone rules.

### 8.4 MVP_PLAN.md sections

1. MVP objective.
2. 16-week success criteria.
3. Non-negotiable player experience.
4. MVP content lock.
5. MVP content cut list.
6. Week-by-week plan.
7. Sprint deliverables.
8. Technical architecture lock.
9. Input/control lock.
10. Camera lock.
11. Class lock.
12. Trigger weapon lock.
13. Tile-state lock.
14. Hybrid lock.
15. Modifier lock.
16. Room/floor lock.
17. Boss lock.
18. Economy lock.
19. Voltage MVP shell.
20. Enemy MVP list.
21. Art/VFX MVP requirements.
22. Performance budget.
23. Readability tests.
24. Playtest gates.
25. Trailer/demo capture requirements.
26. Risk register.
27. Blocker criteria.
28. Definition of done.

### 8.5 Decisions for MVP_PLAN.md

- 3 MVP classes only.
- Universal 5-trigger weapon.
- 7 base tile states.
- 3 hybrid tiles.
- 3 status hybrids.
- 12-18 modifiers.
- 4-6 enemy families.
- 6 floors/rooms.
- 1 boss.
- Spark + Cinder only.
- No Echo.
- No Atlas.
- No full 5 dungeon x 3 floor.
- No skill variants.
- No secret boss.
- No full loot economy.
- No node graph.
- 45 sec Form cooldown baseline.
- 30-50 peak only in surge rooms.

### 8.6 ROADMAP.md sections

1. Roadmap principle.
2. MVP exit criteria.
3. Phase 2: months 4-6.
4. Phase 3: months 6-12.
5. Phase 4: 12+ months.
6. Content expansion order.
7. Class expansion order.
8. Skill variant rollout.
9. Dungeon/act expansion.
10. Boss expansion.
11. Enemy family expansion.
12. Modifier/relic expansion.
13. Currency expansion.
14. Voltage expansion.
15. Atlas/endgame rollout.
16. Art/biome expansion.
17. Localization and accessibility.
18. Steam demo/EA/launch milestones.
19. Marketing beats.
20. Risks per phase.
21. Kill criteria for features.
22. Decision backlog.

### 8.7 Decisions for ROADMAP.md

- Phase 2: 5 classes.
- Phase 2: skill variant unlocks.
- Phase 2: Cinder meta expansion.
- Phase 2: second act/dungeon type.
- Phase 2: 25-30 modifiers.
- Phase 2: Voltage 0-8.
- Phase 3: 8 classes.
- Phase 3: 5 dungeon x 3 floor structure.
- Phase 3: Echo currency.
- Phase 3: more bosses and secret rooms.
- Phase 3: 40-60 modifiers.
- Phase 3: Voltage 0-16.
- Phase 4: 12+ classes.
- Phase 4: Atlas/endgame grid.
- Phase 4: season system.
- Phase 4: leaderboard/challenge modes.
- Phase 4: Voltage 0-32.

### 8.8 Boundary rules

| Question | Document |
|---|---|
| What is CB trying to become? | VISION_DOC.md |
| What must be built in 16 weeks? | MVP_PLAN.md |
| What happens after the demo works? | ROADMAP.md |
| Is this feature allowed someday? | VISION_DOC.md |
| Is this feature allowed now? | MVP_PLAN.md |
| When does this cut feature return? | ROADMAP.md |
| What is the public genre identity? | VISION_DOC.md. |
| What is the current build target? | MVP_PLAN.md. |
| What is Phase 2 content order? | ROADMAP.md. |

---

## 9. EXECUTIVE SUMMARY

1. Sub-genre lock: Use **Cascade ARPG** as the public label, **Battlefield Alchemy Roguelite** as the descriptor, and **Environmental Cascade Combat** as the internal pattern name.
2. USP validation: The market gap is real but not empty. Noita, Magicraft, Magicka, Hades, Soulstone, and Hero Siege are adjacent; CB is distinct only if floor-state cascade remains the core verb.
3. Map lock: Choose **Hero Siege Act/Floor** for full vision, but ship one **B-lite act slice** in MVP: 6 floors/rooms, one fork, one shop/shrine, one Act boss.
4. Class lock: RoR2-style 4-skill kit + Form is correct. Use universal 5-element trigger weapons with class affinity. Defer variants to Phase 2 and budget two weeks for a real unlock system.
5. MVP lock: The five synthesis layers fit only as a vertical slice. Do not cut ARPG, roguelite, mass-clear, element grammar, or spatial cascade; cut content count inside each layer.

CODEX SYNTHESIS COMPLETE
